using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_MiscJobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(GridViewDocument);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job Detail";

        if (!Page.IsPostBack)
        {
            JobDetailMS(Convert.ToInt32(Session["MiscJobId"]));

            Get_BillingInstruction(Convert.ToInt32(Session["MiscJobId"]));

            JobOperation.FillDocumentMS(ddDocument, Convert.ToInt32(hdnModuleID.Value));
        }
    }

    private void JobDetailMS(int JobId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        string strPreAlertId = "0";
        int BranchId = 0;

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Job Detail
        DataSet dsJobDetail = JobOperation.GetMiscJobDetail(JobId);

        // Move customer delivery to warehouse
        // BranchId = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString());

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("MiscJobTrack.aspx");
            Session["MiscJobId"] = null;
        }

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();

            lblTitle.Text += " - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            hdnJobRefNo.Value = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            strPreAlertId = dsJobDetail.Tables[0].Rows[0]["AlertId"].ToString();
            hdnPreAlertId.Value = strPreAlertId;
            hdnCustId.Value = dsJobDetail.Tables[0].Rows[0]["CustomerId"].ToString();
            hdnModuleID.Value = dsJobDetail.Tables[0].Rows[0]["ModuleID"].ToString();
            Session["ModuleID"] = hdnModuleID.Value;
            // Job Directoy Path

            if (dsJobDetail.Tables[0].Rows[0]["DocFolder"] != DBNull.Value)
                strCustDocFolder = dsJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\";

            if (dsJobDetail.Tables[0].Rows[0]["FileDirName"] != DBNull.Value)
                strJobFileDir = dsJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int transMode = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["TransMode"]);

            //if (transMode == (int)TransMode.Sea)
            //{
            //    TabSeaContainer.Visible = true;
            //}

        }
    }
    #region Job Details Cancelation 
    protected void btnCancelJob_Click(object sender, EventArgs e)
    {
        int Jobid = Convert.ToInt32(Session["MiscJobId"]);
        TextBox txtRemark = (TextBox)FVJobDetail.FindControl("txtRemark");

        int result = DBOperations.CancleMiscJobDetails(Jobid, txtRemark.Text, LoggedInUser.glUserId) ;

        if (result == 0)
        {
            lblError.Text = "Job Details Cancle Successfully";
            lblError.CssClass = "success";
          //  gvDailyJob.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["MiscJobId"] = null;

        string strReutrnUrl = ((Button)sender).CommandArgument.ToString();
        //MiscJobTrack.aspx

        Response.Redirect(strReutrnUrl);
    }

    #endregion
    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {

    }

    protected void ddChangeStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            lblError.Text = "download!";
            lblError.CssClass = "errorMsg";

            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = JobOperation.GetMiscDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                lblError.Text = "DocID!";

                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }
        }
    }

    protected void ddContainerType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {

    }

    protected void gvContainer_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gvContainer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvContainer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    #region Document
    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..UploadFiles\\" + DocumentPath);
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
            lblError.Text = "Download Document System Error!";
        }
    }
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            string strURL = "ViewDoc.aspx?ref= " + DocumentPath;

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        int Result = -123;
        lblError.Visible = true;
        string strFilePath = "";
        string strFileName = "";
        int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);

        int JobId = Convert.ToInt32(Session["MiscJobId"]);
        int CustomerId = Convert.ToInt32(hdnCustId.Value);

        if (fuDocument.FileName.Trim() == "")
        {
            lblError.Text = "Please Browse The Document!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (DocumentId == 0)
        {
            lblError.Text = "Please Select Document Type!";
            lblError.CssClass = "errorMsg";
            return;
        }

        strFilePath = hdnUploadPath.Value;
        if (strFilePath == "")
            strFilePath = "MiscJobDoc\\";

        strFileName = UploadDocument(hdnUploadPath.Value);

        if (strFileName != "")
        {
            JobOperation.AddMiscJobDoc(JobId, strFileName, strFilePath, DocumentId, LoggedInUser.glUserId);
            Result = 0;
        }
        else
        {
            Result = 1;
        }

        if (Result == 0)
        {
            lblError.Text = "Document uploaded successfully!";
            lblError.CssClass = "success";
            GridViewDocument.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "Please Select File!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Document not uploaded. Please try again!";
            lblError.CssClass = "errorMsg";
        }
    }

    private string UploadDocument(string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
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
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FileName;
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

    #endregion

    #region Daily Activity

    private void GetJobActivity(int JobID)
    {
        gvDailyJob.DataBind();

        DataSet dsActivity = DBOperations.GetMiscJobActivityDetail(JobID);// added new method for getting job details

        if (dsActivity.Tables.Count > 0)
        {
            if (dsActivity.Tables[0].Rows.Count > 0)
            {
                hdnActivityID.Value = dsActivity.Tables[0].Rows[0]["lId"].ToString();
                ddActivityStatus.SelectedValue = dsActivity.Tables[0].Rows[0]["StatusId"].ToString();
                txtProgressDetail.Text = dsActivity.Tables[0].Rows[0]["DailyProgress"].ToString();

            }
        }
    }

    protected void btnSaveDailyActivity_Click(object sender, EventArgs e)
    {
        int Jobid = Convert.ToInt32(Session["MiscJobId"]);
        int ActivityID = Convert.ToInt32(hdnActivityID.Value);

        int SummaryStatus = Convert.ToInt32(ddActivityStatus.SelectedValue);

       // Boolean bVisibleToCustomer = true;

        string strProgress = txtProgressDetail.Text.Trim();

        string strFilePath = "";

        //if (fuDocument.FileName.Trim() != "")
        //{
        //    strFilePath = UploadDocument(hdnUploadPath.Value);
        //}

        int result = 1;

        if (ActivityID == 0)
        {
            result = DBOperations.AddMiscJobDailyActivity(Jobid, strProgress, strFilePath, SummaryStatus, LoggedInUser.glUserId);//bVisibleToCustomer,
        }
        else
        {
            result = DBOperations.UpdateMiscJobDailyActivity(ActivityID, SummaryStatus, strProgress, LoggedInUser.glUserId);// bVisibleToCustomer,
        }

        if (result == 0)
        {
            lblError.Text = "Job Activity Updated Successfully!";
            lblError.CssClass = "success";
            txtProgressDetail.Text = "";

            GetJobActivity(Jobid);

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        ddActivityStatus.SelectedValue = "0";       
        txtProgressDetail.Text = "";
    }

    protected void gvDailyJob_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvDailyJob_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "DailyProgress").ToString();
                HyperLink lnkMoreProgress = (HyperLink)e.Row.FindControl("lnkMoreProgress");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }
                if (strDocPath == "")
                {
                    LinkButton lnkActivtDownload = (LinkButton)e.Row.FindControl("lnkActivtDownload");
                    lnkActivtDownload.Visible = false;
                }

                if (strProgressText.Length > 50)
                {
                    lnkMoreProgress.ToolTip = strProgressText;

                    //NameHyperLink.Attributes.Add("onmouseover", "ShowToolTip(event, " +
                    //"'" + Server.HtmlEncode(strProgressText) + "','Right');");

                    //NameHyperLink.Attributes.Add("onmouseout", "HideTooTip(event);");
                    //NameHyperLink.Attributes.Add("onmousemove", "MoveToolTip(event,'Right');");
                }
                else
                {
                    lnkMoreProgress.Visible = false;
                }
            }
        }

    }

    protected void gvDailyJob_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "updateaa")
        {
            GridViewRow gvRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int ActivityId = Convert.ToInt32(gvDailyJob.DataKeys[gvRow.RowIndex].Value);

            TextBox txtProgressDetail = (TextBox)gvRow.FindControl("txtProgressDetail");
            DropDownList ddStatusId = (DropDownList)gvRow.FindControl("ddStatusId");
          //  RadioButtonList RDLCustomer = (RadioButtonList)gvRow.FindControl("RDLCustomer");

            string strProgress = txtProgressDetail.Text.Trim();
            int StatusId = Convert.ToInt32(ddStatusId.SelectedValue);
           // Boolean bVisibleToCustomer = Convert.ToBoolean(RDLCustomer.SelectedValue);

            if (StatusId == 0)
            {
                lblError.Text = "Please Select Current Status!";
                lblError.CssClass = "errorMsg";
            }
            else if (strProgress != "")
            {
                int result = DBOperations.UpdateMiscJobDailyActivity(ActivityId, StatusId, strProgress, LoggedInUser.glUserId); //bVisibleToCustomer, 

                if (result == 0)
                {
                    lblError.Text = "Job Activity Updated Successfully!";
                    lblError.CssClass = "success";
                    gvDailyJob.EditIndex = -1;
                    gvDailyJob.DataBind();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Enter Progress Detail!";
                lblError.CssClass = "errorMsg";
            }
        }// END_Activity_Update

        else if (e.CommandName.ToLower() == "activitydelete")
        {
            int ActivityId = Convert.ToInt32(e.CommandArgument.ToString());

            int result = DBOperations.DeleteMiscJobDailyActivity(ActivityId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Job Activity Removed Successfully";
                lblError.CssClass = "success";
                gvDailyJob.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Current Activity!";
                lblError.CssClass = "errorMsg";
            }
        }// END_Activity_Delete
    }

    #endregion

    protected void btnUpdateStatusAdHoc_Click(object sender, EventArgs e)
    {
        int Jobid = Convert.ToInt32(Session["MiscJobId"]);
        int ModuleID = Convert.ToInt32(hdnModuleID.Value);

        int StatusID = Convert.ToInt32(ddChangeStatus.SelectedValue);

        if (StatusID == 11)
        {
            int result = JobOperation.AddBillingAdvice(Jobid, ModuleID, "", LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "File Sent to Billing Scrutiny.";
                lblError.CssClass = "success";

            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Billing Advice Detail Already Updated!";
                lblError.CssClass = "errorMsg";
            }

        }
    }

    #region billing instruction
    protected void Get_BillingInstruction(int JobId)
    {
        int strJobId = JobId;
        DataTable dtBillInstruction = new DataTable();
        dtBillInstruction = DBOperations.Get_BillingInstructionDetail(strJobId);
        if (dtBillInstruction.Rows.Count > 0)
        {
            //dvBillInstruction.Visible = false;
            //dvResult.Visible = true;
            // btnSaveInstruction.Visible = false;

            foreach (DataRow rw in dtBillInstruction.Rows)
            {
                //lblAgencyApply.Text = rw["AlliedAgencyApply"].ToString();
                //lblRefNo.Text = rw["JobRefNo"].ToString();
                if (rw["AlliedAgencyService"].ToString() == "") { lblAlliedAgencyService.Text = "-"; }
                else
                {
                    string args = rw["AlliedAgencyService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblAlliedAgencyService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                //lblAlliedAgencyService.Text = rw["AlliedAgencyService"].ToString(); }
                if (rw["AlliedAgencyRemark"].ToString() == "") { lblAlliedAgencyRemark.Text = "-"; }
                else { lblAlliedAgencyRemark.Text = rw["AlliedAgencyRemark"].ToString(); };

                if (rw["OtherService"].ToString() == "") { lblOtherService.Text = "-"; }
                else
                {
                    string args = rw["OtherService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblOtherService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                if (rw["OtherServiceRemark"].ToString() == "") { lblOtherServiceRemark.Text = "-"; }
                else { lblOtherServiceRemark.Text = rw["OtherServiceRemark"].ToString(); }
                if (rw["InstructionCopy"].ToString() == "") { lnkInstructionCopy.Text = "-"; }
                else { lnkInstructionCopy.Text = rw["InstructionCopy"].ToString(); }
                if (rw["InstructionCopy1"].ToString() == "") { lnkInstructionCopy1.Text = "-"; }
                else { lnkInstructionCopy1.Text = rw["InstructionCopy1"].ToString(); }
                if (rw["InstructionCopy2"].ToString() == "") { lnkInstructionCopy2.Text = "-"; }
                else { lnkInstructionCopy2.Text = rw["InstructionCopy2"].ToString(); }
                if (rw["InstructionCopy3"].ToString() == "") { lnkInstructionCopy3.Text = "-"; }
                else { lnkInstructionCopy3.Text = rw["InstructionCopy3"].ToString(); }
                if (rw["Instruction"].ToString() == "") { lblInstruction.Text = "-"; }
                else { lblInstruction.Text = rw["Instruction"].ToString(); }
                if (rw["Instruction1"].ToString() == "") { lblInstruction1.Text = "-"; }
                else { lblInstruction1.Text = rw["Instruction1"].ToString(); }
                if (rw["Instruction2"].ToString() == "") { lblInstruction2.Text = "-"; }
                else { lblInstruction2.Text = rw["Instruction2"].ToString(); }
                if (rw["Instruction3"].ToString() == "") { lblInstruction3.Text = "-"; }
                else { lblInstruction3.Text = rw["Instruction3"].ToString(); }
                lblUserDate.Text = rw["Userdate"].ToString();
                lblUserId.Text = rw["sName"].ToString();
            }
            // ModalPopupInstruction.Show();
        }
        else
        {
            //dvResult.Visible = false;
        }

    }
    protected void lnkInstructionCopy_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy.Text);
    }

    protected void lnkInstructionCopy1_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy1.Text);
    }

    protected void lnkInstructionCopy2_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy2.Text);
    }

    protected void lnkInstructionCopy3_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy3.Text);
    }

    //private void DownloadDocument(string DocumentPath)
    //{

    //    //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

    //    string ServerPath = FileServer.GetFileServerDir();

    //    if (ServerPath == "")
    //    {
    //        ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
    //        ServerPath = ServerPath.Replace("PCA\\", "");
    //    }
    //    else
    //    {
    //        ServerPath = ServerPath + DocumentPath;
    //    }
    //    try
    //    {
    //        HttpResponse response = Page.Response;
    //        FileDownload.Download(response, ServerPath, DocumentPath);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    #endregion
}