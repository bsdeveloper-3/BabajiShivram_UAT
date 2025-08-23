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
using System.Data.SqlClient;

public partial class PCA_PCDFinalTypingDate : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    int ModuleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnknonreceive);
        ScriptManager1.RegisterPostBackControl(lnkreceive);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);

        //DropDownList drp = this.Master.FindControl("ddFinYear") as DropDownList;
        //Session["FinYearId"] = drp.SelectedValue;

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending For Bill Final Typing";

            DataFilter1.DataSource = PCDSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDFinalTypingDate.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvNonRecievedJobDetail.DataBind();

            DataFilter2.DataSource = PCDSqlDataSource1;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDFinalTypingDate1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvRecievedJobDetail.DataBind();
        }


        if (TabPCDBilling.TabIndex == 0)
        {
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                lblreceivemsg.Text = "";
                lblMsgforApproveReject.Text = "";
                DataFilter1.DataSource = PCDSqlDataSource;
                DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
                DataFilter1.FilterSessionID = "PCDFinalTypingDate.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            }
        }
        if (TabPCDBilling1.TabIndex == 1)
        {
            if (TabJobDetail.ActiveTabIndex == 1)
            {
                lblreceivemsg.Text = "";
                lblMsgforApproveReject.Text = "";
                DataFilter2.DataSource = PCDSqlDataSource1;
                DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
                DataFilter2.FilterSessionID = "PCDFinalTypingDate1.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
                TextBox lbl = gvRecievedJobDetail.FindControl("txtCommentes1") as TextBox;

            }
        }
        if (gvNonRecievedJobDetail.Rows.Count == 0)
        {
            lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Bill Final Typing!";
            lblMsgforNonReceived.CssClass = "errorMsg"; ;
        }
        else
        {
            lblMsgforNonReceived.Text = "";
        }
        if (gvRecievedJobDetail.Rows.Count == 0)
        {
            lblMsgforReceived.Text = "No Job Found For Recieved file for Bill Final Typing!";
            lblMsgforReceived.CssClass = "errorMsg"; ;
        }
        else
        {
            lblMsgforReceived.Text = "";
        }
    }

    #region NonRecieved

    protected void lnkNonreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "FinalTyping_nonreceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        NonreceivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void NonreceivejoblistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvNonRecievedJobDetail.AllowPaging = false;
        gvNonRecievedJobDetail.AllowSorting = false;
        gvNonRecievedJobDetail.Caption = "";

        gvNonRecievedJobDetail.Columns[1].Visible = false;
        gvNonRecievedJobDetail.Columns[11].Visible = false;

        gvNonRecievedJobDetail.DataSourceID = "PCDSqlDataSource";
        gvNonRecievedJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvNonRecievedJobDetail);

        gvNonRecievedJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }


    protected void gvNonRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        RePopulateValues(); //Checkbox

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status1 = (Label)e.Row.FindControl("rulename");

            string status = status1.Text.ToString();

            if (status == "1")
            {
                e.Row.BackColor = System.Drawing.Color.Aqua;
                e.Row.ToolTip = "FIFO";
            }

            if (status == "2")
            {
                e.Row.BackColor = System.Drawing.Color.Tomato;
                e.Row.ToolTip = "WEEKDAYS";
            }
            if (status == "3")
            {
                e.Row.BackColor = System.Drawing.Color.SkyBlue;
                e.Row.ToolTip = "DAYS";
            }
            if (status == "4")
            {
                e.Row.BackColor = System.Drawing.Color.Pink;
                e.Row.ToolTip = "Urgent Bill";
            }

            if (status == "5")
            {
                e.Row.BackColor = System.Drawing.Color.BurlyWood;
                e.Row.ToolTip = "Quick Paymaster";
            }

            if (status == "6")
            {
                e.Row.BackColor = System.Drawing.Color.Khaki;
                e.Row.ToolTip = "High Credit Days";
            }
            if (status == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                e.Row.ToolTip = "amount";
            }
            if (status == "8")
            {
                e.Row.BackColor = System.Drawing.Color.LightSlateGray;
                e.Row.ToolTip = "User Days";
            }


            e.Row.Cells[8].ToolTip = "Today – FSB"; //aging1	            
            e.Row.Cells[9].ToolTip = "Today – FSFT"; //aging2
            //e.Row.Cells[10].ToolTip = "Today – Clearance Date";//aging3
            e.Row.Cells[11].CssClass = "hidden";//rulename

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[10].ToolTip = "Today – Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[6].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[10].ToolTip = "Today – Job added to additional tab.";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else
                {
                    e.Row.Cells[10].ToolTip = "Today – Clearance Date.";
                    e.Row.Cells[7].Text = "";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[11].CssClass = "hidden";//rulename
        }

    }

    protected void gvNonRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;

        }
    }


    protected void gvNonRecievedJobDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RememberOldValues();
        gvNonRecievedJobDetail.PageIndex = e.NewPageIndex;
        RePopulateValues();


    } //Checkbox

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvNonRecievedJobDetail.Rows)
        {
            index = Convert.ToInt32(gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value);

            bool result = ((CheckBox)row.FindControl("chk1")).Checked;

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!categoryIDList.Contains(index))
                    categoryIDList.Add(index);
            }
            else
            {
                categoryIDList.Remove(index);

                countRow = countRow + 1;
            }
            // }
        }
        if (categoryIDList != null && categoryIDList.Count > 0)
            Session["CHECKED_ITEMS"] = categoryIDList;


        int countRow1 = countRow;

    } //Checkbox


    private void RePopulateValues()
    {

        ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];

        if (categoryIDList != null && categoryIDList.Count > 0)
        {
            foreach (GridViewRow row in gvNonRecievedJobDetail.Rows)
            {
                int index = Convert.ToInt32(gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value);

                bool result = ((CheckBox)row.FindControl("chk1")).Checked;
                if (categoryIDList.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = true;
                }
                else
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = false;

                }

            }
        }
        //gvNonRecievedJobDetail.AllowPaging = true;
    }//Checkbox


    protected void btnReceive_Click(object sender, EventArgs e)
    {
        int i = 0;
        int Result = 0;
        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        gvNonRecievedJobDetail.AllowPaging = false;//Checkbox
        gvNonRecievedJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");
                string JobId = Recv.CommandArgument;
                string s = string.Empty;

                Session["JobRefNo"] = Recv.Text;
                GetModuleId();
                Result = BillingOperation.receivedfile(Convert.ToInt32(JobId), LoggedInUser.glUserId, Convert.ToInt16(EnumBilltype.FinalInvoiceTyping), ModuleId);
                Session["JobRefNo"] = "";
                ModuleId = 0;

                if (Result == 0)
                {
                    lblreceivemsg.Text = "File Recieved Successfully!";
                    lblreceivemsg.CssClass = "success";
                    gvNonRecievedJobDetail.DataBind();
                    gvRecievedJobDetail.DataBind();
                }
                else if (Result == 1)
                {
                    lblreceivemsg.Text = "System Error! Please try after sometime!";
                    lblreceivemsg.CssClass = "errorMsg";
                }
                i++;
            }
            else
            {
                if (i == 0)
                {
                    lblreceivemsg.Text = "Please Check atleast 1 checkbox.";
                    lblreceivemsg.CssClass = "errorMsg";
                }
            }

        }
        gvNonRecievedJobDetail.AllowPaging = true;//Checkbox
        gvNonRecievedJobDetail.DataBind();//Checkbox

    }

    #endregion

    #region Recieved

    protected void lnkreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "FinalTyping_ReceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        receivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void receivejoblistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvRecievedJobDetail.AllowPaging = false;
        gvRecievedJobDetail.AllowSorting = false;
        gvRecievedJobDetail.Caption = "";

        gvRecievedJobDetail.Columns[0].Visible = false;
        gvRecievedJobDetail.Columns[2].Visible = false;
        gvRecievedJobDetail.Columns[14].Visible = false;
        gvRecievedJobDetail.Columns[16].Visible = false;
        gvRecievedJobDetail.Columns[17].Visible = false;
        gvRecievedJobDetail.Columns[19].Visible = false;

        gvRecievedJobDetail.DataSourceID = "PCDSqlDataSource1";
        gvRecievedJobDetail.DataBind();

        //Remove Controls
        //this.RemoveControls(gvRecievedJobDetail);

        gvRecievedJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv1 = (GridView)sender;
        GridViewRow gvr1 = (GridViewRow)gv1.BottomPagerRow;
        if (gvr1 != null)
        {
            gvr1.Visible = true;

        }
    }

    protected void gvRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        TextBox lbl = e.Row.FindControl("txtCommentes1") as TextBox;
        if (lbl != null)
        {
            ScriptManager.GetCurrent(this).SetFocus(lbl.ClientID);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status1 = (Label)e.Row.FindControl("rulename");

            string status = status1.Text.ToString();

            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkDocument");
            if (lnk.Text == "N")
            {
                lnk.Enabled = false;
                lnk.Text = "";
            }
            else
            {
                lnk.Text = "Upload Document";
                lnk.Enabled = true;
            }


            if (status == "1")
            {
                e.Row.BackColor = System.Drawing.Color.Aqua;
                e.Row.ToolTip = "FIFO";
            }

            if (status == "2")
            {
                e.Row.BackColor = System.Drawing.Color.Tomato;
                e.Row.ToolTip = "WEEKDAYS";
            }
            if (status == "3")
            {
                e.Row.BackColor = System.Drawing.Color.SkyBlue;
                e.Row.ToolTip = "DAYS";
            }
            if (status == "4")
            {
                e.Row.BackColor = System.Drawing.Color.Pink;
                e.Row.ToolTip = "Urgent Bill";
            }

            if (status == "5")
            {
                e.Row.BackColor = System.Drawing.Color.BurlyWood;
                e.Row.ToolTip = "Quick Paymaster";
            }

            if (status == "6")
            {
                e.Row.BackColor = System.Drawing.Color.Khaki;
                e.Row.ToolTip = "High Credit Days";
            }
            if (status == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                e.Row.ToolTip = "amount";
            }
            if (status == "8")
            {
                e.Row.BackColor = System.Drawing.Color.LightSlateGray;
                e.Row.ToolTip = "User Days";
            }

            e.Row.Cells[2].CssClass = "hidden"; //Receive all
            e.Row.Cells[5].CssClass = "hidden"; //jobrefno
            e.Row.Cells[14].CssClass = "hidden"; //rulename
            e.Row.Cells[11].ToolTip = "Today – FSB"; //aging1	            
            e.Row.Cells[12].ToolTip = "Today – FRFT"; //aging2
            //e.Row.Cells[13].ToolTip = "Today – Clearance Date";//aging3

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[13].ToolTip = "Today – Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[8].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[13].ToolTip = "Today – Job added to additional tab.";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Text = "";
                }
                else
                {
                    e.Row.Cells[13].ToolTip = "Today – Clearance Date.";
                    e.Row.Cells[9].Text = "";
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].CssClass = "hidden"; //Receive all
            e.Row.Cells[5].CssClass = "hidden"; //jobrefno
            e.Row.Cells[14].CssClass = "hidden"; //rulename
        }

    }

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            ViewState["JobId"] = Session["JobId"];
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
            rptDocument.DataSource = BillingOperation.FillPCDDocumentForFinalTyping(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();
            lblmessage.Text = "";
            ModalPopupDocument.Show();


        }
        if (e.CommandName.ToLower() == "update")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
        }

        if (e.CommandName.ToLower() == "fa" && e.CommandArgument != null)
        {
            int result = 0;

            //lblerror.Text = "";
            ViewState["JobId"] = e.CommandArgument.ToString();
            LinkButton lnkFA = sender as LinkButton;
            Session["status"] = "FA";
            bool bApprove = true;

            result = BillingOperation.ApprovebyFAFinalTyping(Convert.ToInt32(ViewState["JobId"]), bApprove, "", LoggedInUser.glUserId, 0, "");
            if (result == 0)
            {
                lblMsgforApproveReject.Text = "Final Typing Approved! Job Moved Back To Final Typing!.";
                lblMsgforApproveReject.CssClass = "success";
                gvRecievedJobDetail.DataBind();
            }
            else if (result == 1)
            {
                lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                lblMsgforApproveReject.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblMsgforApproveReject.Text = "Final Checked Already Pending!";
                lblMsgforApproveReject.CssClass = "errorMsg";
                //ModalPopupExtender1.Hide();
            }

        }
        if (e.CommandName.ToLower() == "reject" && e.CommandArgument != null)
        {
            lblMsgforReceived.Text = "";
            lblerror.Text = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";
            ViewState["JobId"] = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            Session["JobRefNo"] = commandArgs[1].ToString();

            LinkButton btnsubmit = sender as LinkButton;
            Session["status"] = "Reject";

            this.ModalPopupExtender1.Show();

            foreach (RepeaterItem ri in rpReason.Items)
            {
                CheckBox chkreason = (CheckBox)ri.FindControl("chkreasonofpendency");
                TextBox txtreason = (TextBox)ri.FindControl("txtReason");
                DropDownList Drplrdctype = (DropDownList)ri.FindControl("Drplrdctype");
                Label lbltype = (Label)ri.FindControl("lbltype");
                Label lblReason = (Label)ri.FindControl("lblReason");
                Label lblmailto = (Label)ri.FindControl("lblmailto");
                TextBox txtUser = (TextBox)ri.FindControl("txtUser");
                HiddenField hnduserid = (HiddenField)ri.FindControl("hdnUserId");

                txtUser.Visible = true;
                lblmailto.Visible = true;
                txtUser.Text = "";
                hnduserid.Value = "";

                if (chkreason.Checked == true)
                {
                    chkreason.Checked = false;
                    txtreason.Text = "";
                    Drplrdctype.SelectedIndex = 0;
                }
            }
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
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
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
                //string ext = Path.GetExtension(fuPCDUpload.FileName);
                string ext = Path.GetExtension(FileName);
                //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload.SaveAs(ServerFilePath + FileName);


        }

        return FilePath + FileName;
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(hdnJobId.Value);
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;
        int cnt = 0;
        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
            //Label lblmessage1 = (Label)itm.FindControl("lblmessage");
            if (chk.Checked)
            {
                cnt++;

                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                //CheckBoxList chkDuplicate = (CheckBoxList)(itm.FindControl("chkDuplicate"));
                string strFilePath = "";

                int DocumentId = Convert.ToInt32(hdnDocId.Value);
                //if (chkDuplicate.Items[0].Selected)
                //    IsOriginal = true;

                //if (chkDuplicate.Items[1].Selected)
                //    IsCopy = true;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strUploadPath, fuDocument);
                }

                Result = BillingOperation.AddScanPCDDocument(JobId, DocumentId, strFilePath, LoggedInUser.glUserId, Convert.ToInt16(EnumBilltype.FinalInvoiceTyping));

                if (Result == 0)
                {

                    lblmessage.Text = "Document List Updated For Final Typing.";
                    lblmessage.CssClass = "errorMsg";
                    ModalPopupDocument.Show();
                    //return;

                }
                else if (Result == 1)
                {
                    lblmessage.Text = "System Error! Please try after some time.";
                    lblmessage.CssClass = "errorMsg";
                    ModalPopupDocument.Show();
                    //return;

                }
            }
        }
        if (cnt == 0)
        {
            lblmessage.Text = "Please Check Checkbox First!";
            lblmessage.CssClass = "errorMsg";
            ModalPopupDocument.Show();
            return;
        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void chkDocType_CheckedChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
            FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
            if (chk.Checked)
            {
                fuDocument.Enabled = true;
            }
            else
            {
                fuDocument.Enabled = false;
            }
        }

        ModalPopupDocument.Show();

    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");

            //CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            //CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");

            //chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));

            //chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));

            if (DataBinder.Eval(e.Item.DataItem, "PCDDocId").ToString() != "0")
            {
                chkDocumentType.Checked = true;
                fileUploadDocument.Enabled = true;
            }

            if (chkDocumentType != null && fileUploadDocument != null)
            {
                // CheckBox OnClientClick java Script Functino
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv1('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "');");

                // CheckBoxList Customer Validation Control "ClientValidationFunction="ValidateCheckBoxList"
                // Add Parameter for Javascript Function ValidateCheckBoxList("Update Panel Id","CustomerValidatorId","Control Identifier","CheckBoxlistId","IsValid"); 

                //ScriptManager.RegisterExpandoAttribute(upShipment, CVCheckBoxList.ClientID, "checklistId", chkDuplicate.ClientID, false);

                // Add Javascript On Click Event For Checkbox List Copy/Original
                //foreach (System.Web.UI.WebControls.ListItem lstitem in chkDuplicate.Items)
                //{
                //    lstitem.Attributes.Add("OnClick", "javascript:chkDuplicateChecked('" + chkDuplicate.ClientID + "','" + chkDocumentType.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                //}
            }
        }
    }


    #endregion
    
    #region Reject 
    protected void rpReason_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        TextBox txtUser = (TextBox)e.Item.FindControl("txtUser");
        int count = 2341;
        foreach (RepeaterItem ri in rpReason.Items)
        {
            AjaxControlToolkit.AutoCompleteExtender UserExtender = (AjaxControlToolkit.AutoCompleteExtender)ri.FindControl("UserExtender");

            //txtUser = (TextBox)ri.FindControl("txtUser"); 

            HtmlControl divwidthCust = (HtmlControl)ri.FindControl("divwidthCust");

            if (txtUser != null)
            {
                if (txtUser.Visible != false)
                {
                    UserExtender.TargetControlID = txtUser.ClientID;
                }
                UserExtender.BehaviorID = divwidthCust.ClientID;

                UserExtender.ContextKey = count.ToString();//LoggedInUser.glUserId.ToString();
                count = count + 1;
            }
        }

        CheckBox chkReasonType = (CheckBox)e.Item.FindControl("chkreasonofpendency");
        RequiredFieldValidator RFVRejectReason = (RequiredFieldValidator)e.Item.FindControl("RFVRejectReason");
        TextBox txtReason = (TextBox)e.Item.FindControl("txtReason");
        
        Label lbltype = (Label)e.Item.FindControl("lbltype");
        DropDownList Drplrdctype = (DropDownList)e.Item.FindControl("Drplrdctype");
        Label lblMailTo = (Label)e.Item.FindControl("lblMailTo");

        HiddenField hdnUserId = (HiddenField)e.Item.FindControl("hdnUserId");

        // Reject Department Category
        DropDownList cboDDCategory = (DropDownList)e.Item.FindControl("ddCategory");
        RequiredFieldValidator cboRFVCatgory = (RequiredFieldValidator)e.Item.FindControl("RFVCatgory");

        if (chkReasonType != null && RFVRejectReason != null)
        {
            chkReasonType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkReasonType.ClientID + "','" + cboDDCategory.ClientID + "','" + cboRFVCatgory.ClientID + "','" + txtReason.ClientID + "','" + lbltype.ClientID + "','" + Drplrdctype.ClientID + "','" + RFVRejectReason.ClientID + "','" + chkReasonType.Text + "','" + txtUser.ClientID + "','" + hdnUserId.ClientID + "','" + lblMailTo.ClientID + "');");
            
            txtReason.Style.Add("display", "none");
            cboDDCategory.Style.Add("display", "none");
            lbltype.Style.Add("display", "none");
            Drplrdctype.Style.Add("display", "none");
            txtUser.Style.Add("display", "none");
            lblMailTo.Style.Add("display", "none");
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        lblerror.Text = "";
        int result = 0;
        int result1 = 0;
        lblerror.Text = "";
        lblerror.CssClass = "";
        int reasonforPendency = 0;
        int checkedCount = 0;
        string interests = string.Empty;
        bool IsLRPending = false;

        string Remark = "";
        string LRDC_Type = "";
        string Mailtoid = "";
        if (Session["status"].ToString() == "Reject")
        {
            bool bApprove = false;

                    for (int i = 0; i < rpReason.Items.Count; i++)
            {
                RequiredFieldValidator RFVRejectReason = (RequiredFieldValidator)rpReason.Items[i].FindControl("RFVRejectReason");

                CheckBox chk = (CheckBox)rpReason.Items[i].FindControl("chkreasonofpendency");

                if (chk.Checked)
                {
                    if (RFVRejectReason != null)
                    {
                        RFVRejectReason.Enabled = true;
                    }

                    checkedCount += chk.Checked ? 1 : 0;
                    HiddenField hdnDocId = (HiddenField)rpReason.Items[i].FindControl("hdnDocId");
                    DropDownList Drplrdctype = (DropDownList)rpReason.Items[i].FindControl("Drplrdctype");
                    TextBox txtReason = (TextBox)rpReason.Items[i].FindControl("txtReason");
                    DropDownList CBOddCategory = (DropDownList)rpReason.Items[i].FindControl("ddCategory");

                    HiddenField hdnUserId = (HiddenField)rpReason.Items[i].FindControl("hdnUserId");

                    Remark = txtReason.Text.Trim();
                    reasonforPendency = Convert.ToInt16(hdnDocId.Value);

                    if (chk.Text == "LR/DC")
                    {
                        IsLRPending = true;
                        LRDC_Type = Drplrdctype.SelectedValue;
                    }
                    else
                    {
                        LRDC_Type = "0";
                    }
                    
                    if (chk.Text == "Others")
                    {
                        Mailtoid = hdnUserId.Value;
                    }
                    else
                    {
                        Mailtoid = "0";
                    }

                    if (checkedCount != 0)
                    {
                        int CategoryId = 0;

                        CategoryId = Convert.ToInt32(CBOddCategory.SelectedValue);

                        GetModuleId();
                        result = BillingOperation.ApproveRejectTyping(Convert.ToInt32(ViewState["JobId"]), bApprove, Remark, Convert.ToInt16(reasonforPendency), LRDC_Type, CategoryId, LoggedInUser.glUserId, ModuleId);
                        Session["JobRefNo"] = "";

                        if (result == 0)
                        {
                            if (IsLRPending == true)
                            {
                                int result_LR = DBOperations.AddAdviceToLRPending(Convert.ToInt32(ViewState["JobId"]), 0, LoggedInUser.glUserId);
                            }
                            ModalPopupExtender1.Hide();

                            lblMsgforApproveReject.Text = "Final Typing Rejected! Job Moved Back To Bill Rejected!.";
                            lblMsgforApproveReject.CssClass = "success";
                        }
                        else if (result == 1)
                        {

                            lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                        }
                        else if (result == 2)
                        {
                            lblMsgforApproveReject.Text = "Bill Draft Rejected Already Pending!";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                            ModalPopupExtender1.Hide();
                        }
                    }
                }
            }

            if (checkedCount == 0)
            {
                ModalPopupExtender1.Show();
                lblerror.Text = "Please select atleast 1 checkbox";
                lblerror.CssClass = "errorMsg";
            }

            if (ModuleId != 2)
            {
                int rejectedby = Convert.ToInt16(EnumBilltype.DraftInvoice);
                result1 = BillingOperation.Rejectmailnotification(Convert.ToInt32(ViewState["JobId"]), LoggedInUser.glUserId, rejectedby, Convert.ToInt16(Session["FinYearId"]), Convert.ToString(Mailtoid));
                if (result1 != 0)
                {
                    lblMsgforApproveReject.Text = "System Error in Mail Sending! Please try after sometime.";
                    lblMsgforApproveReject.CssClass = "errorMsg";
                }
            }
            ModuleId = 0;
        }

        gvRecievedJobDetail.DataBind();
    }
    #endregion Reject  
    protected void gvRecievedJobDetail_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strJobId = gvRecievedJobDetail.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtCommentes1 = (TextBox)gvRecievedJobDetail.Rows[e.RowIndex].FindControl("txtCommentes1");
            string txtcomment = Request.Form[txtCommentes1.UniqueID];
            if (strJobId.Trim() != "" && txtcomment != "")
            {
                PCDSqlDataSource1.UpdateParameters["JobID"].DefaultValue = strJobId;
                PCDSqlDataSource1.UpdateParameters["Comments"].DefaultValue = txtCommentes1.Text.Trim();
                PCDSqlDataSource1.Update();
                //lblMsgforReceived.Text = "Comment Updated Sucessfully";
                //lblMsgforReceived.CssClass = "Success";
                gvRecievedJobDetail.EditIndex = -1;
                //gvRecievedJobDetail.DataBind();
            }
            else
            {
                lblMsgforReceived.Text = "Please Enter Required Field!";
                lblMsgforReceived.CssClass = "errorMsg";
            }
            e.Cancel = true;

        }
        catch (Exception ex)
        {
            lblMsgforReceived.Text = ex.Message;
            lblMsgforReceived.CssClass = "errorMsg";
        }

    }
    protected void gvRecievedJobDetail_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvRecievedJobDetail.EditIndex = e.NewEditIndex;
    }
    protected void PCDSqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = false;

        if (e.Exception != null)
        {
            lblMsgforReceived.Text = e.Exception.Message;
            lblMsgforReceived.CssClass = "errorMsg";
        }
        else
        {
            int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

            if (Result == 0)
            {
                lblMsgforReceived.Text = "Detail Updated Successfully !";
                lblMsgforReceived.CssClass = "success";
                gvRecievedJobDetail.DataBind();
            }
            else if (Result == 1)
            {
                lblMsgforReceived.Text = "System Error! Please try after sometime!";
                lblMsgforReceived.CssClass = "errorMsg";
            }
        }

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
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                DataFilter1_OnDataBound();
            }
            else
            {
                DataFilter2_OnDataBound();
            }
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDFinalTypingDate.aspx";
            DataFilter1.FilterDataSource();
            gvNonRecievedJobDetail.DataBind();
            if (gvNonRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Final Typing!";
                lblMsgforNonReceived.CssClass = "errorMsg";
            }
            else
            {
                lblMsgforNonReceived.Text = "";
            }
        }
        catch (Exception ex)
        {
            //DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDFinalTypingDate1.aspx";
            DataFilter2.FilterDataSource();
            gvRecievedJobDetail.DataBind();
            if (gvRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforReceived.Text = "No Job Found For Recieved file for Final Typing!";
                lblMsgforReceived.CssClass = "errorMsg";
            }
            else
            {
                lblMsgforReceived.Text = "";
            }
        }
        catch (Exception ex)
        {
            //DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void Rptpriorities_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblpriorities = (Label)e.Item.FindControl("lblpriorities");
            TextBox txtcolor = (TextBox)e.Item.FindControl("txtpriorities");

            if (lblpriorities.Text == "FIFO")
            {
                txtcolor.BackColor = System.Drawing.Color.Aqua;
            }
            else if (lblpriorities.Text == "Week Day")
            {
                txtcolor.BackColor = System.Drawing.Color.Tomato;
            }
            else if (lblpriorities.Text == "Day")
            {
                txtcolor.BackColor = System.Drawing.Color.SkyBlue;
            }
            else if (lblpriorities.Text == "Urgent Bill")
            {
                txtcolor.BackColor = System.Drawing.Color.Pink;
            }
            else if (lblpriorities.Text == "Quick Paymaster")
            {
                txtcolor.BackColor = System.Drawing.Color.Bisque;
            }
            else if (lblpriorities.Text == "High Credit Days")
            {
                txtcolor.BackColor = System.Drawing.Color.Khaki;
            }
            else if (lblpriorities.Text == "Debit Amount")
            {
                txtcolor.BackColor = System.Drawing.Color.LightSeaGreen;
            }
            else
            {
                txtcolor.BackColor = System.Drawing.Color.LightSlateGray;
            }

        }
    }

    protected void TabJobDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabJobDetail.ActiveTabIndex == 0)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblMsgforApproveReject.Text = "";
            DataFilter1.DataSource = PCDSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDFinalTypingDate.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvNonRecievedJobDetail.DataBind();
        }
        if (TabJobDetail.ActiveTabIndex == 1)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblMsgforApproveReject.Text = "";
            DataFilter2.DataSource = PCDSqlDataSource1;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDFinalTypingDate1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvRecievedJobDetail.DataBind();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }

    protected void GetModuleId()
    {
        DataTable dtModule = BillingOperation.GetModuleId(Session["JobRefNo"].ToString());
        if (dtModule.Rows.Count > 0)
        {
            foreach (DataRow row in dtModule.Rows)
            {
                ModuleId = Convert.ToInt32(row["MODULEID"].ToString());
            }
        }
        else
        {
            ModuleId = LoggedInUser.glModuleId;
        }
    }

}