using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using Ionic.Zip;
using System.Collections;

public partial class ContMovement_JobDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvConsolidateJobs);
        ScriptManager1.RegisterPostBackControl(gvDocuments);
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(btnUploadBackOfficeDoc);
        ScriptManager1.RegisterPostBackControl(gvBackOfficeDocument);

        if (!IsPostBack)
        {
            fsConsolidateJob.Visible = false;
        }

        DataFilter1.DataSource = SqlDataSourceJobForDocument;
        DataFilter1.DataColumns = gvDocuments.Columns;
        DataFilter1.FilterSessionID = "JobDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Consolidate Jobs TabPanel
    protected void gvConsolidateJobs_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvConsolidateJobs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "documentdoc")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strMovementId = "", strJobRefNo = "";

            if (commandArgs[0].ToString() != "")
                strMovementId = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strJobRefNo = commandArgs[1].ToString();
            DownloadZip(Convert.ToInt32(strMovementId), strJobRefNo);
        }
        else if (e.CommandName.ToLower() == "edit")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            int JobId = 0, MovementId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                MovementId = Convert.ToInt32(commandArgs[1].ToString());

            fsConsolidateJob.Visible = true;
            hdnMovementId.Value = MovementId.ToString();
            hdnConsolidateJobId.Value = MovementId.ToString();
            fvConsolidateJob.DataBind();
        }
    }
    protected void btnEditConsolidateFV_Click(object sender, EventArgs e)
    {
        fvConsolidateJob.ChangeMode(FormViewMode.Edit);
    }
    protected void btnCancelConsolidateFV_Click(object sender, EventArgs e)
    {
        fsConsolidateJob.Visible = false;
    }
    protected void btnUpdateConsolidate_Click(object sender, EventArgs e)
    {
        int NominatedCFSId = 0, JobId = 0;
        DateTime dtEmptyContReturnDate = DateTime.MinValue, dtMovementComplete = DateTime.MinValue,
                    dtShippingLine = DateTime.MinValue, dtConfirmedByLine = DateTime.MinValue;

        if (hdnMovementId.Value != "" && hdnMovementId.Value != "0")
        {
            if (((DropDownList)fvConsolidateJob.FindControl("ddlCFSName")).SelectedValue != "0")
                NominatedCFSId = Convert.ToInt32(((DropDownList)fvConsolidateJob.FindControl("ddlCFSName")).SelectedValue);

            if (((TextBox)fvConsolidateJob.FindControl("txtShippingLineDate")).Text.Trim() != "")
                dtShippingLine = Commonfunctions.CDateTime(((TextBox)fvConsolidateJob.FindControl("txtShippingLineDate")).Text.Trim());

            if (((TextBox)fvConsolidateJob.FindControl("txtConfirmedByLineDate")).Text.Trim() != "")
                dtConfirmedByLine = Commonfunctions.CDateTime(((TextBox)fvConsolidateJob.FindControl("txtConfirmedByLineDate")).Text.Trim());

            if (((TextBox)fvConsolidateJob.FindControl("txtCompleteDate")).Text.Trim() != "")
                dtMovementComplete = Commonfunctions.CDateTime(((TextBox)fvConsolidateJob.FindControl("txtCompleteDate")).Text.Trim());

            if (((TextBox)fvConsolidateJob.FindControl("txtEmptyContReturnDate")).Text.Trim() != "")
                dtEmptyContReturnDate = Commonfunctions.CDateTime(((TextBox)fvConsolidateJob.FindControl("txtEmptyContReturnDate")).Text.Trim());

            if (((HiddenField)fvConsolidateJob.FindControl("hdnJobId")).Value != "")
                JobId = Convert.ToInt32(((HiddenField)fvConsolidateJob.FindControl("hdnJobId")).Value);

            int result = CMOperations.UpdateMovementDetail(Convert.ToInt32(hdnMovementId.Value), dtEmptyContReturnDate, dtMovementComplete,
                                                                dtShippingLine, dtConfirmedByLine, NominatedCFSId, loggedInUser.glUserId);
            if (result == 0)
            {
                /////////// Update Nominated CFS Name in IGM /////////////////
                if (NominatedCFSId > 0)
                {
                    int result_CFSId = CMOperations.UpdateNominatedCFSName(JobId, NominatedCFSId);
                }
                fvConsolidateJob.ChangeMode(FormViewMode.ReadOnly);
                if (hdnMovementId.Value != "" && hdnMovementId.Value != "0")
                {
                    fsConsolidateJob.Visible = true;
                    hdnConsolidateJobId.Value = hdnMovementId.Value;
                    fvConsolidateJob.DataBind();
                }
                lblError.Text = "Successfully updated movement job details.";
                lblError.CssClass = "success";
            }
            else if (result == 2)
            {
                lblError.Text = "Movement detail not found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System error while updating detail. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "System error while updating detail. Please try again later.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnCancelConsolidate_Click(object sender, EventArgs e)
    {
        fvConsolidateJob.ChangeMode(FormViewMode.ReadOnly);
        if (hdnMovementId.Value != "" && hdnMovementId.Value != "0")
        {
            fsConsolidateJob.Visible = true;
            hdnConsolidateJobId.Value = hdnMovementId.Value;
            fvConsolidateJob.DataBind();
        }
    }
    protected void fvConsolidateJob_DataBound(object sender, EventArgs e)
    {
        if (fvConsolidateJob.CurrentMode == FormViewMode.Edit)
        {
            HiddenField hdnBranchId = ((HiddenField)fvConsolidateJob.FindControl("hdnBranchId"));
            HiddenField hdnNominatedCFSId = ((HiddenField)fvConsolidateJob.FindControl("hdnNominatedCFSId"));
            DropDownList ddlCFSName = ((DropDownList)fvConsolidateJob.FindControl("ddlCFSName"));

            if (ddlCFSName != null)
            {
                DBOperations.FillCFS(ddlCFSName, Convert.ToInt32(hdnBranchId.Value));
                ddlCFSName.SelectedValue = hdnNominatedCFSId.Value;
            }
        }
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

    #endregion

    #region Document TabPanel
    protected void gvBackOfficeDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strLid = "", strDocPath = "";

            if (commandArgs[0].ToString() != "")
                strLid = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strDocPath = commandArgs[1].ToString();
            DownloadDoc(strDocPath);
        }
        else if (e.CommandName.ToLower() == "remove")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = CMOperations.DeleteBackOfficeDocument(lid, loggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted document.";
                    lblError.CssClass = "success";
                    gvBackOfficeDocument.DataBind();
                }
                else
                {
                    lblError.Text = "System error. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }
    protected void btnUploadBackOfficeDoc_Click(object sender, EventArgs e)
    {
        string JobRefPath = "";
        DataSet dsGetJobDetail = CMOperations.GetJobDetail(Convert.ToInt32(Session["JobId"]));
        if (dsGetJobDetail != null && dsGetJobDetail.Tables[0].Rows.Count > 0)
        {
            JobRefPath = dsGetJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
        }

        if (fuBackOfficeDoc.HasFile)
        {
            JobRefPath = JobRefPath.Replace(",", "");
            JobRefPath = JobRefPath.Replace("/", "");
            JobRefPath = JobRefPath.Replace("-", "");

            string DocPath = UploadFiles(fuBackOfficeDoc, JobRefPath + "\\");
            if (DocPath != "")
            {
                int result_Doc = CMOperations.AddBackOfficeDocument(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(ddlBackOfficeDocType.SelectedValue),
                                                                        DocPath, loggedInUser.glUserId);
                if (result_Doc == 0)
                {
                    lblError.Text = "Uploaded document successfully.";
                    lblError.CssClass = "success";
                    gvBackOfficeDocument.DataBind();
                }
                else
                {
                    lblError.Text = "Error while uploading document. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblError.Text = "Please browse file to upload!";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fuDocument.HasFile)
        {
            string FilePath = UploadFiles(fuDocument, "");
            int result = CMOperations.AddDocument(Convert.ToInt32(ddlJobForDocument.SelectedValue), FilePath, loggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added document.";
                lblError.CssClass = "success";
                gvDocuments.DataBind();
            }
            else if (result == 2)
            {
                lblError.Text = "Document already exists.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up document.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please browse file before uploading.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void gvDocuments_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strMovementId = "", strDocPath = "";

            if (commandArgs[0].ToString() != "")
                strMovementId = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strDocPath = commandArgs[1].ToString();
            DownloadDoc(strDocPath);
        }
        else if (e.CommandName.ToLower() == "remove")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = CMOperations.DeleteDocument(lid, loggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted document.";
                    lblError.CssClass = "success";
                    gvDocuments.DataBind();
                }
                else
                {
                    lblError.Text = "System error. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }
    protected void DownloadZip(int JobId, string JobRefNo)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            //zip.AddDirectoryByName("MovementFiles");
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
                        zip.AddFile(FilePath, "MovementJobFiles");
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
    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\PNMovement\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "PNMovement\\" + DocumentPath;
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
    protected string UploadFiles(FileUpload FU, string FilePath)
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
    protected string RandomString(int size)
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
            DataFilter1.FilterSessionID = "JobDetail.aspx";
            DataFilter1.FilterDataSource();
            gvDocuments.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
    #endregion

    #region Consolidate Status
    protected void gvConsolidateJobStatus_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    #endregion

    #region Billing TabPanel
    protected void gvDraftInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "draftinvoicenext")
        {
            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = CMOperations.DraftInvoiceJobMoveToDraftCheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Draft Check!.";
                lblError.CssClass = "success";
                gvDraftInvoice.DataBind();
                gvDraftcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Draft Invoice!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblError.Text = "Draft Invoice Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void gvDraftInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string Consolidatedjobno = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Consolidatedjobno = e.Row.Cells[8].Text;
            if (Consolidatedjobno != "&nbsp;")
            {
                e.Row.Cells[9].Visible = false;
                lblConsolidated.Text = "<center><b>This job Consolidated With JobNo. " + Consolidatedjobno + "</b></center> ";
                lblConsolidated.CssClass = "success";
                DraftCheck.Visible = false;
                FinalInvoiceCheck.Visible = false;
                FinalInvoiceTyping.Visible = false;
                Billdispatch.Visible = false;
                BillRejection.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Consolidatedjobno != "&nbsp;")
            {
                e.Row.Cells[9].Visible = false;
            }
        }
    }
    protected void gvFinaltyping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "finaltypingnext")
        {
            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = CMOperations.FinalTypingJobMoveToFinalCheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Final Draft Check!.";
                lblError.CssClass = "success";
                gvFinaltyping.DataBind();
                gvfinalcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Final typing!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblError.Text = "Final typing Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    #endregion

    #region Job Detail TabPanel
    protected void btnEditJobDetail_Click(object sender, EventArgs e)
    {
        fvJobDetail.ChangeMode(FormViewMode.Edit);
    }
    protected void btnCancelJobDetail_Click(object sender, EventArgs e)
    {
        Response.Redirect("JobTracking.aspx");
    }
    protected void btnUpdateJobDetail_Click(object sender, EventArgs e)
    {
        int lid = 0, CustomerId = 0, DivisionId = 0, PlantId = 0;

        lid = Convert.ToInt32(Session["JobId"]);
        if (lid > 0)
        {
            if (((DropDownList)fvJobDetail.FindControl("ddlCustomerMS")).SelectedValue != "0")
                CustomerId = Convert.ToInt32(((DropDownList)fvJobDetail.FindControl("ddlCustomerMS")).SelectedValue);
            if (((DropDownList)fvJobDetail.FindControl("ddDivision")).SelectedValue != "0")
                DivisionId = Convert.ToInt32(((DropDownList)fvJobDetail.FindControl("ddDivision")).SelectedValue);
            if (((DropDownList)fvJobDetail.FindControl("ddPlant")).SelectedValue != "0")
                PlantId = Convert.ToInt32(((DropDownList)fvJobDetail.FindControl("ddPlant")).SelectedValue);

            int result = CMOperations.UpdateJobDetail(lid, CustomerId, DivisionId, PlantId, ((TextBox)fvJobDetail.FindControl("txtRemark")).Text.Trim(), loggedInUser.glUserId);
            if (result == 0)
            {
                fvJobDetail.ChangeMode(FormViewMode.ReadOnly);
                fvJobDetail.DataBind();
                lblError.Text = "Successfully updated job detail.";
                lblError.CssClass = "success";
            }
            else if (result == 2)
            {
                lblError.Text = "Job not found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System error while updating detail. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "System error while updating detail. Please try again later.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnCancelJobDetail2_Click(object sender, EventArgs e)
    {
        fvJobDetail.ChangeMode(FormViewMode.ReadOnly);
        fvJobDetail.DataBind();
    }
    protected void ddlCustomerMS_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCustomerMS = (DropDownList)fvJobDetail.FindControl("ddlCustomerMS");
        DropDownList ddDivision = (DropDownList)fvJobDetail.FindControl("ddDivision");

        if (ddlCustomerMS.SelectedValue != "0")
        {
            DBOperations.FillCustomerDivision(ddDivision, Convert.ToInt32(ddlCustomerMS.SelectedValue));
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
        DropDownList ddDivision = (DropDownList)fvJobDetail.FindControl("ddDivision");
        DropDownList ddPlant = (DropDownList)fvJobDetail.FindControl("ddPlant");

        if (ddDivision.SelectedValue != "0")
        {
            DBOperations.FillCustomerPlant(ddPlant, Convert.ToInt32(ddDivision.SelectedValue));
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }
    protected void fvJobDetail_DataBound(object sender, EventArgs e)
    {
        if (fvJobDetail.CurrentMode == FormViewMode.Edit)
        {
            HiddenField hdnBranchId = ((HiddenField)fvJobDetail.FindControl("hdnBranchId"));
            HiddenField hdnCustId = ((HiddenField)fvJobDetail.FindControl("hdnCustId"));
            HiddenField hdnDivisionId = ((HiddenField)fvJobDetail.FindControl("hdnDivisionId"));
            HiddenField hdnPlantId = ((HiddenField)fvJobDetail.FindControl("hdnPlantId"));
            DropDownList ddlCustomerMS = ((DropDownList)fvJobDetail.FindControl("ddlCustomerMS"));
            DropDownList ddDivision = ((DropDownList)fvJobDetail.FindControl("ddDivision"));
            DropDownList ddPlant = ((DropDownList)fvJobDetail.FindControl("ddPlant"));

            if (ddlCustomerMS != null)
            {
                ddlCustomerMS.SelectedValue = hdnCustId.Value;
                ddlCustomerMS_SelectedIndexChanged(null, EventArgs.Empty);
            }

            if (ddDivision != null)
            {
                ddDivision.SelectedValue = hdnDivisionId.Value;
                ddDivision_SelectedIndexChanged(null, EventArgs.Empty);
            }

            if (ddPlant != null)
            {
                ddPlant.SelectedValue = hdnPlantId.Value;
            }
        }
    }
    #endregion
}