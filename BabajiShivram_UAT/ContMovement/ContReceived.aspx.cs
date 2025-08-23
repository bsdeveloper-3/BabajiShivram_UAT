using System;
using System.Collections.Generic;
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
using System.Net;
using Ionic.Zip;
using System.Text;
using ClosedXML.Excel;

public partial class ContMovement_ContReceived : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvContReceived);
        ScriptManager1.RegisterPostBackControl(btnSaveJob);
        ScriptManager1.RegisterPostBackControl(btnConsolidateJob);

        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Container Received Detail";
        if (!IsPostBack)
        {
            if (gvContReceived.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Container Received Detail!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceContReceived;
        DataFilter1.DataColumns = gvContReceived.Columns;
        DataFilter1.FilterSessionID = "ContReceived.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void ddlCustomerMS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomerMS.SelectedValue != "0")
        {
            DBOperations.FillCustomerDivision(ddDivision, Convert.ToInt32(ddlCustomerMS.SelectedValue));
            mpeConsolidateJobs.Show();
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddDivision.Items.Clear();
            ddDivision.Items.Add(lstSelect);
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddDivision.SelectedValue != "0")
        {
            DBOperations.FillCustomerPlant(ddPlant, Convert.ToInt32(ddDivision.SelectedValue));
            mpeConsolidateJobs.Show();
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }

    protected void gvContReceived_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelectJob = (CheckBox)e.Row.FindControl("chkSelectJob");
            LinkButton lnkbtnContRecdCFSDate = (LinkButton)e.Row.FindControl("lnkbtnContRecdCFSDate");

            if (chkSelectJob != null)
                chkSelectJob.Visible = false;
            if (lnkbtnContRecdCFSDate != null)
            {
                int TotalCont = 0, UpdatedCont = 0, DeliveryType = 0;
                TotalCont = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalContainers"));
                UpdatedCont = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CFSContainerCount"));
                DeliveryType = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DeliveryType"));

                if (DeliveryType == 2)     // De-stuff delivery type
                {
                    lnkbtnContRecdCFSDate.Text = TotalCont + "/" + UpdatedCont;
                    lnkbtnContRecdCFSDate.ToolTip = "Total Container / Container Updated With Received at Yard Date";
                    if (TotalCont == UpdatedCont)
                    {
                        chkSelectJob.Visible = true;
                    }
                }
                else
                {
                    if (DataBinder.Eval(e.Row.DataItem, "LastDispatchDate") != DBNull.Value)
                    {
                        chkSelectJob.Visible = true;
                    }
                    else
                    {
                        lnkbtnContRecdCFSDate.Text = "";
                        chkSelectJob.Visible = false;
                    }
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "NominatedCFSId") == DBNull.Value)
            {
                chkSelectJob.Visible = false;
            }
        }
    }

    protected void gvContReceived_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().ToString().Trim() == "documentdoc")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            int JobId = Convert.ToInt32(commandArgs[0].ToString());
            string JobRefNo = commandArgs[1].ToString();
            DownloadDocument(JobId, JobRefNo);
        }
        else if (e.CommandName.ToLower().ToString().Trim() == "container")
        {
            int JobId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["JobId"] = JobId.ToString();
            Response.Redirect("ContainerDetail.aspx");
        }
    }

    protected void gvContReceived_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void btnDeleteRow_Click(object sender, EventArgs e)
    {
        Button lb = (Button)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowID]);
            }

            if (dt.Rows.Count == 0)
            {
                ViewState["ConsolidateJobs"] = null;
            }
            else
            {
                ViewState["ConsolidateJobs"] = dt;
            }
            gvConsolidateJobs.DataSource = dt;
            gvConsolidateJobs.DataBind();
        }
        SetPreviousData();
        mpeConsolidateJobs.Show();
    }

    protected void DataSourceContReceived_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;
        int result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);
        if (result == 0)
        {
            lblError.Text = "Successfully added container received at yard date.";
            lblError.CssClass = "success";
        }
        else if (result == 2)
        {
            lblError.Text = "Job does not exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Error while adding up container received at yard date.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceContReceived_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection UpdParams = ((SqlDataSourceView)sender).UpdateParameters;

        Hashtable ht = new Hashtable();
        foreach (Parameter UpdParam in UpdParams)
            ht.Add(UpdParam.Name, true);

        for (int i = 0; i < CmdParams.Count; i++)
        {
            if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
                CmdParams.Remove(CmdParams[i--]);
        }
    }

    protected void rptDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");
            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");
            fileUploadDocument.Enabled = false;

            if (chkDocumentType.Checked == true)
            {
                fileUploadDocument.Enabled = true;
            }
        }
    }

    protected void chkDocType_CheckedChanged(object sender, EventArgs e)
    {
        var index = (CheckBox)sender;
        CheckBox chkDocType = (CheckBox)index.FindControl("chkDocType");
        FileUpload fuDoc = (FileUpload)index.FindControl("fuDocument");

        if (fuDoc != null)
        {
            if (chkDocType.Checked == true)
            {
                fuDoc.Enabled = true;
                fuDoc.Focus();
            }
            else
            {
                fuDoc.Enabled = false;
            }
        }
        mpeConsolidateJobs.Show();
    }

    protected void btnSaveJob_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            mpeConsolidateJobs.Show();
            string JobRefPath = "";
            int count = 0, JobId = 0;
            if (lblMovementJobNo.Text.Trim() != "")
            {
                JobRefPath = lblMovementJobNo.Text.Trim();
                JobRefPath = JobRefPath.Replace(",", "");
                JobRefPath = JobRefPath.Replace("/", "");
                JobRefPath = JobRefPath.Replace("-", "");

                //////////// Add Movement Job ////////////////////
                int result = CMOperations.AddJobDetail(lblMovementJobNo.Text.Trim(), Convert.ToInt32(ddBranch.SelectedValue), Convert.ToInt32(ddlCustomerMS.SelectedValue),
                                Convert.ToInt32(ddDivision.SelectedValue), Convert.ToInt32(ddPlant.SelectedValue), txtRemark.Text.Trim(), loggedInUser.glUserId);
                if (result > 0)
                {
                    JobId = result;
                    count = 1;
                    /////////// Add Consolidate Jobs //////////////
                    if (gvConsolidateJobs != null && gvConsolidateJobs.Rows.Count > 0)
                    {
                        for (int i = 0; i < gvConsolidateJobs.Rows.Count; i++)
                        {
                            Label lblJobId = (Label)gvConsolidateJobs.Rows[i].FindControl("lblJobId");
                            if (lblJobId != null)
                            {
                                if (lblJobId.Text.Trim() != "")
                                {
                                    int result_Consolidate = CMOperations.AddConsolidateJobDetail(result, Convert.ToInt32(lblJobId.Text.Trim()), loggedInUser.glUserId);
                                    if (result_Consolidate == 0)
                                    {
                                        count = 1;
                                        int updResult = CMOperations.UpdateReceivedJob(Convert.ToInt32(lblJobId.Text.Trim()));
                                    }
                                }
                            }
                        }
                    }

                    /////////// Add Documents /////////////////////
                    if (rptDocument != null && rptDocument.Items.Count > 0)
                    {
                        foreach (RepeaterItem itm in rptDocument.Items)
                        {
                            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
                            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");

                            if (chk.Checked)
                            {
                                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                                if (fuDocument != null)
                                {
                                    if (fuDocument.HasFile)
                                    {
                                        string DocPath = UploadFiles(fuDocument, JobRefPath + "\\");
                                        if (DocPath != "")
                                        {
                                            int result_Doc = CMOperations.AddBackOfficeDocument(result, Convert.ToInt32(hdnDocId.Value), DocPath, loggedInUser.glUserId);
                                            if (result_Doc == 0)
                                            {
                                                count = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (count == 1)
                    {
                        /////////// Move To Billing Advice ///////////
                        int result_Advice = CMOperations.AddToBackOffice(JobId, loggedInUser.glUserId);

                        /////////// Move To Scrutiny ///////////
                        int result_Scrutiny = CMOperations.AddToScrutiny(JobId, loggedInUser.glUserId);

                        lblError.Text = "Successfully moved job " + lblMovementJobNo.Text.Trim() + " to Billing Scrutiny tab.";
                        lblError.CssClass = "success";
                        mpeConsolidateJobs.Hide();
                        gvContReceived.DataBind();
                    }
                    else
                    {
                        lblError.Text = "System error while adding job. Please try again later!";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else if (result == -2)
                {
                    lblError.Text = "Job already exists!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == -1)
                {
                    lblError.Text = "System error while adding job. Please try again later!";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            Page.Validate("vgConsolidate");
            mpeConsolidateJobs.Show();
        }
    }

    #region Consolidate Jobs

    protected void ddBranch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddBranch.SelectedValue != "0" && ddBranch.SelectedValue != "")
        {
            lblMovementJobNo.Text = CMOperations.GetJobRefNo(Convert.ToInt32(ddBranch.SelectedValue));
            mpeConsolidateJobs.Show();
        }
    }

    protected void btnConsolidateJob_Click(object sender, EventArgs e)
    {
        int i = 0, CFSId = 0, CopyCFSId = 0, c = 0;
        bool isValid = false;
        int Result = 0;
        DateTime dtContRecdCFSDate = DateTime.MinValue;
        RememberOldValues();    //Checkbox
        RePopulateValues();     //Checkbox

        foreach (GridViewRow gvr in gvContReceived.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkSelectJob")).Checked)
            {
                ImageButton imgbtnShowDocuments = (ImageButton)gvr.FindControl("imgbtnShowDocuments");
                string[] commandArgs = imgbtnShowDocuments.CommandArgument.ToString().Split(';');
                if (c == 0)
                {
                    CFSId = Convert.ToInt32(commandArgs[4].ToString());
                    c++;
                }

                if (CFSId == Convert.ToInt32(commandArgs[4].ToString()))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    break;
                }
            }
        }

        if (isValid == true)
        {
            foreach (GridViewRow gvr in gvContReceived.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkSelectJob")).Checked)
                {
                    i++;
                    ImageButton imgbtnShowDocuments = (ImageButton)gvr.FindControl("imgbtnShowDocuments");
                    string[] commandArgs = imgbtnShowDocuments.CommandArgument.ToString().Split(';');
                    int JobId = Convert.ToInt32(commandArgs[0].ToString());
                    string JobRefNo = commandArgs[1].ToString();
                    string Customer = commandArgs[2].ToString();
                    string CFSName = commandArgs[3].ToString();

                    AddNewRow(JobId, JobRefNo, Customer, CFSName);

                    int PCDDocType = Convert.ToInt32(EnumPCDDocType.BackOffice);
                    rptDocument.DataSource = CMOperations.GetChekListDocDetail(JobId, PCDDocType);
                    rptDocument.DataBind();
                    mpeConsolidateJobs.Show();
                }
                else
                {
                    if (i == 0)
                    {
                        lblError.Text = "Please Check atleast 1 checkbox.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }
        }
        else
        {
            lblError.Text = "Consolidate Jobs should be for same CFS. Please check once again before going ahead.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Popup(object sender, EventArgs e)
    {
        mpeConsolidateJobs.Hide();
        ViewState["ConsolidateJobs"] = null;
    }

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvContReceived.Rows)
        {
            index = (int)gvContReceived.DataKeys[row.RowIndex].Value;
            bool result = ((CheckBox)row.FindControl("chkSelectJob")).Checked;

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
        }
        if (categoryIDList != null && categoryIDList.Count > 0)
            Session["CHECKED_ITEMS"] = categoryIDList;

        int countRow1 = countRow;
    }

    private void RePopulateValues()
    {

        ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];

        if (categoryIDList != null && categoryIDList.Count > 0)
        {
            foreach (GridViewRow row in gvContReceived.Rows)
            {
                int index = (int)gvContReceived.DataKeys[row.RowIndex].Value;

                bool result = ((CheckBox)row.FindControl("chkSelectJob")).Checked;
                if (categoryIDList.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelectJob");
                    myCheckBox.Checked = true;
                }
                else
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelectJob");
                    myCheckBox.Checked = false;
                }
            }
        }
    }

    //////////////////// Consolidate Grid View //////////////////////////////////

    protected void AddNewRow(int JobId, string JobRefNo, string Customer, string CFSName)
    {
        int rowIndex = 0;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            DataRow dr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblJobId = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[1].FindControl("lblJobId");
                    Label lblJobRefNo = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
                    Label lblCFSName = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[4].FindControl("lblCFSName");

                    dr = dt.NewRow();
                    dr["RowNumber"] = i + 1;
                    dr["JobId"] = JobId.ToString();
                    dr["JobRefNo"] = JobRefNo;
                    dr["Customer"] = Customer;
                    dr["CFSName"] = CFSName;

                    dt.Rows[i - 1]["RowNumber"] = lblRowNumber.Text;
                    dt.Rows[i - 1]["JobId"] = lblJobId.Text;
                    dt.Rows[i - 1]["JobRefNo"] = lblJobRefNo.Text;
                    dt.Rows[i - 1]["Customer"] = lblCustomer.Text;
                    dt.Rows[i - 1]["CFSName"] = lblCFSName.Text;
                    rowIndex++;
                }

                dt.Rows.Add(dr);
                ViewState["ConsolidateJobs"] = dt;
                gvConsolidateJobs.DataSource = dt;
                gvConsolidateJobs.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("JobId", typeof(string)));
            dt.Columns.Add(new DataColumn("JobRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer", typeof(string)));
            dt.Columns.Add(new DataColumn("CFSName", typeof(string)));

            DataRow dr = null;
            dr = dt.NewRow();

            dr["RowNumber"] = 1;
            dr["JobId"] = JobId.ToString();
            dr["JobRefNo"] = JobRefNo;
            dr["Customer"] = Customer;
            dr["CFSName"] = CFSName;

            dt.Rows.Add(dr);
            ViewState["ConsolidateJobs"] = dt;
            gvConsolidateJobs.DataSource = dt;
            gvConsolidateJobs.DataBind();

            Label lblRowNumber = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
            Label lblJobId = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[1].FindControl("lblJobId");
            Label lblJobRefNo = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
            Label lblCustomer = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
            Label lblCFSName = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[4].FindControl("lblCFSName");

            lblJobId.Text = JobId.ToString();
            lblJobRefNo.Text = JobRefNo;
            lblCFSName.Text = CFSName;
        }
        SetPreviousData();
    }

    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblJobId = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[1].FindControl("lblJobId");
                    Label lblJobRefNo = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[3].FindControl("lblCustomer");
                    Label lblCFSName = (Label)gvConsolidateJobs.Rows[rowIndex].Cells[4].FindControl("lblCFSName");

                    lblRowNumber.Text = dt.Rows[i]["RowNumber"].ToString();
                    lblJobId.Text = dt.Rows[i]["JobId"].ToString();
                    lblJobRefNo.Text = dt.Rows[i]["JobRefNo"].ToString();
                    lblCustomer.Text = dt.Rows[i]["Customer"].ToString();
                    lblCFSName.Text = dt.Rows[i]["CFSName"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {

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
            DataFilter1.FilterSessionID = "ContReceived.aspx";
            DataFilter1.FilterDataSource();
            gvContReceived.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Document Events

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\PNMovement\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "PNMovement\\" + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);
                string FileId = RandomString(5);
                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);
            return FilePath + FileName;
        }
        else
        {
            return "";
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

    private void DownloadDocument(int JobId, string JobRefNo)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("MovementFiles");
            DataSet dsGetDoc = CMOperations.GetDocuments(JobId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\PNMovement\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "PNMovement\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "MovementFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("MovementZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion

    #region Export Events

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "ContRecdDetail" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportData("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void ExportData(string header, string contentType)
    {
        int UserId = 0, FinYearId = 0;

        if (Convert.ToString(Session["UserId"]) != null)
        {
            UserId = Convert.ToInt32(Convert.ToString(Session["UserId"]));
        }

        if (Convert.ToString(Session["FinYearId"]) != null)
        {
            FinYearId = Convert.ToInt32(Convert.ToString(Session["FinYearId"]));
        }

        DataSet dsGetContRecd = CMOperations.ReportContReceivedJobs(UserId, FinYearId);
        DataTable dtGetContRecd = dsGetContRecd.Tables[0];
        dtGetContRecd.TableName = "Container Received Jobs";

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtGetContRecd);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=ContReceived_" + DateTime.Now.ToShortDateString().Replace(@"/", "_").Replace(" ", "_") + ".xls");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                memoryStream.WriteTo(Response.OutputStream);
                memoryStream.Close();
                Response.Flush();
                Response.End();
            }
        }
    }

    //protected void ExportData(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    gvContReceived.AllowPaging = false;
    //    gvContReceived.AllowSorting = false;
    //    gvContReceived.Columns[0].Visible = false;      // Sr.No. update button
    //    gvContReceived.Columns[1].Visible = false;      // Consolidate button
    //    gvContReceived.Columns[2].Visible = false;      // Document linkbutton
    //    gvContReceived.Columns[3].Visible = false;      // edit linkbutton
    //    gvContReceived.Columns[16].Visible = false;     // Cont CFS date templatefield 
    //    gvContReceived.Columns[17].Visible = true;      // Cont CFS date boundfield 
    //    gvContReceived.Columns[19].Visible = true;      // container nos 
    //    gvContReceived.Caption = "Container Received Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

    //    DataFilter1.FilterSessionID = "ContReceived.aspx";
    //    DataFilter1.FilterDataSource();
    //    gvContReceived.DataBind();
    //    gvContReceived.RenderControl(hw);
    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}

    protected void RemoveControls(Control grid)
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    #endregion
}