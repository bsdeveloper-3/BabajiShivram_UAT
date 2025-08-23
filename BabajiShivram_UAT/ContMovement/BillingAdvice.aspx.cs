using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class ContMovement_BillingAdvice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    /// <summary>
    /// There is a ButtonField column and the Id column
    /// therefore first edit cell index is 2
    /// </summary>
    const int FIRST_EDITABLE_CELL = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        if (!IsPostBack)
        {
            Session["JobId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Billing Advice";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Billing Advice!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = PCDSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "BillingAdvice.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["JobId"] = e.CommandArgument.ToString();
            if (Convert.ToString(Session["JobId"]) != "")
            {
                Response.Redirect("AdviceJobDetail.aspx");
            }
        }
        else if (e.CommandName.ToLower() == "sendforscrutiny")
        {
            int JobId = Convert.ToInt32(e.CommandArgument);
            SendDocumentForScrutiny(JobId);
        }
        else if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            lblMessage.Text = "";
            ScriptManager1.RegisterPostBackControl(btnSaveDocument);
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
            rptDocument.DataSource = DBOperations.FillPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();
            ModalPopupDocument.Show();
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "ScrutinyStatus") != DBNull.Value)
            {
                int ScrutinyStatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ScrutinyStatus"));
                if (ScrutinyStatus == 3) // Rejected
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Scrutiny Status - Rejected";
                }
            }
            // Editable Cell

            AddCell(FIRST_EDITABLE_CELL + 5, "Instructions", e);
        }
    }

    void AddCell(int cellIndex, string cellName, GridViewRowEventArgs e)
    {
        //we will add javascript events to the controls in the cell
        Label lab = (Label)e.Row.Cells[cellIndex].FindControl(string.Format("lab{0}", cellName));
        TextBox txt = (TextBox)e.Row.Cells[cellIndex].FindControl(string.Format("txt{0}", cellName));
        Button btn = (Button)e.Row.Cells[cellIndex].FindControl(string.Format("btn{0}", cellName));

        if (lab != null)
        {
            //if text is empty, we need to grow label's width to ensure user can click this label
            if (string.IsNullOrEmpty(lab.Text))
            {
                lab.Width = 100;
                lab.Text = "Add";
            }

            //add javascript events to:
            //label (we want to hide this label and show textbox on click)
            lab.Attributes.Add("onclick", string.Format("return HideLabel('{0}', event, '{1}');", lab.ClientID, txt.ClientID));
            //and to textbox
            txt.Attributes.Add("onkeypress", string.Format("return SaveDataOnEnter('{0}', event, '{1}', '{2}');", txt.ClientID, lab.ClientID, btn.ClientID));
            //todo: there is an issue with onblur, I am not JS master, but hope it is just a small issue

            txt.Attributes.Add("onblur", string.Format("return SaveDataOnLostFocus('{0}', '{1}');", txt.ClientID, btn.ClientID));

            //highlight a text in textbox
            txt.Attributes.Add("onfocus", "select()");

            //we need to know what row and cell was edited
            //you can use anything else instead, e.g. session or whatever
            btn.CommandName = e.Row.RowIndex.ToString();
            btn.CommandArgument = cellIndex.ToString();

            //set a cursor style
            e.Row.Attributes["style"] += "cursor:pointer;cursor:hand;"; //just a cosmetic thing :)
        }
    }

    protected void txtInstructions_Changed(object sender, CommandEventArgs e)
    {
        //names of controls in GridView row
        string txtInstructions = "txtInstructions";	//textbox
        string labInstructions = "labInstructions";	//label
        //DB column
        string columnName = "Instructions";

        UpdateValue(txtInstructions, labInstructions, columnName, e);
    }

    protected void UpdateValue(string txtName, string labName, string columnName, CommandEventArgs e)
    {
        //todo: you can create e.g. a class and use it instead command argument

        int row = int.Parse(e.CommandName);
        int cell = int.Parse(e.CommandArgument.ToString());

        if (gvJobDetail.Rows[row].RowType == DataControlRowType.DataRow)
        {
            //get the JobID
            int JobId = Convert.ToInt32(gvJobDetail.DataKeys[row]["JobId"]);

            //get the JobRefNo
            LinkButton lnkJobNo = (LinkButton)gvJobDetail.Rows[row].Cells[0].FindControl("lnkJobNo");
            //get the new value
            TextBox txt = (TextBox)gvJobDetail.Rows[row].Cells[cell].FindControl(txtName);
            //update the label, if data are not re-bounded
            Label lab = (Label)gvJobDetail.Rows[row].Cells[cell].FindControl(labName);
            lab.Text = txt.Text;

            string strJobRefno = lnkJobNo.Text.Trim();

            int result = DBOperations.UpdateBillingInstructions(JobId, txt.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Billling Instructions Updated!";
                lblMessage.CssClass = "success";
            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime.";
                lblMessage.CssClass = "errormsg";
            }
            else
            {
                lblMessage.Text = "Job Details Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");

            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");

            chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));

            chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));

            if (DataBinder.Eval(e.Item.DataItem, "PCDDocId").ToString() != "0")
            {
                chkDocumentType.Checked = true;
                fileUploadDocument.Enabled = true;
            }

            if (chkDocumentType != null && fileUploadDocument != null && CVCheckBoxList != null)
            {
                // CheckBox OnClientClick java Script Functino
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");

                // CheckBoxList Customer Validation Control "ClientValidationFunction="ValidateCheckBoxList"
                // Add Parameter for Javascript Function ValidateCheckBoxList("Update Panel Id","CustomerValidatorId","Control Identifier","CheckBoxlistId","IsValid"); 

                ScriptManager.RegisterExpandoAttribute(upShipment, CVCheckBoxList.ClientID, "checklistId", chkDuplicate.ClientID, false);

                // Add Javascript On Click Event For Checkbox List Copy/Original
                foreach (System.Web.UI.WebControls.ListItem lstitem in chkDuplicate.Items)
                {
                    lstitem.Attributes.Add("OnClick", "javascript:chkDuplicateChecked('" + chkDuplicate.ClientID + "','" + chkDocumentType.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                }
            }
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(hdnJobId.Value);
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;

        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");

            if (chk.Checked)
            {
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                CheckBoxList chkDuplicate = (CheckBoxList)(itm.FindControl("chkDuplicate"));
                string strFilePath = "";
                bool IsCopy = false, IsOriginal = false;

                int DocumentId = Convert.ToInt32(hdnDocId.Value);
                if (chkDuplicate.Items[0].Selected)
                    IsOriginal = true;

                if (chkDuplicate.Items[1].Selected)
                    IsCopy = true;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strUploadPath, fuDocument);
                }

                Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblMessage.Text = "Document List Updated For Billing Advice.";
                    lblMessage.CssClass = "success";
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after some time.";
                    lblMessage.CssClass = "errorMsg";

                }
            }
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    private void SendDocumentForScrutiny(Int32 JobId)
    {
        int Result = DBOperations.AddPCDToScrutiny(JobId, LoggedInUser.glUserId);
        if (Result == 0)
        {
            lblMessage.Text = "Job Forwarded For Scrutiny!";
            lblMessage.CssClass = "success";

            gvJobDetail.DataBind();

        }//END_IF
        else if (Result == 1)
        {
            lblMessage.Text = "System Error! Please try after sometime!";
            lblMessage.CssClass = "errorMsg";
        }

    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Documnet Upload/Download/Delete
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        string FileName = fuPCDUpload.FileName;

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuPCDUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(fuPCDUpload.FileName);
                FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }

    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {

        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "PendingBillingAdvice.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "PendingBillingAdvice_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[1].Visible = false; // Link Button job Ref NO
        gvJobDetail.Columns[2].Visible = true;  // Bound Field Job Ref NO

        gvJobDetail.Columns[8].Visible = false;  // Billling Document Link Button
        gvJobDetail.Columns[15].Visible = false;  // Send Document For Scrutiny Link Button

        gvJobDetail.Caption = "Pending Billing Advice On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingPCDBilling.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}