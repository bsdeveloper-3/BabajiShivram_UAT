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
using System.Collections;

public partial class CustomerExport_CustomerEXJobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    public static int a = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(GridViewDocument); //btnPCDUpload
                                                               
        ScriptManager1.RegisterPostBackControl(GridViewPCADoc);
        ScriptManager1.RegisterPostBackControl(GridViewBillingAdvice);
        ScriptManager1.RegisterPostBackControl(fvPCD);
        ScriptManager1.RegisterPostBackControl(GridViewBillingDept);
        ScriptManager1.RegisterPostBackControl(FVSBPrepare);
        ScriptManager1.RegisterPostBackControl(FVShipmentGetIN);
        ScriptManager1.RegisterPostBackControl(GridViewWarehouse);
        
        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("CustomerJobTracking.aspx");
        }

        if (!IsPostBack)
        {
            if (Session["JobId"] != null)
            {
                JobDetailMS(Convert.ToInt32(Session["JobId"]));

                EXOperations.EX_FillChekListDocDetail(ddDocument);
                
            }
            else
            {
                Response.Redirect("CustomerJobTracking.aspx");
            }
        }
    }

    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("CustomerJobTracking.aspx");
            Session["JobId"] = null;
        }

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            lblTitle.Text = "Shipment Detail - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            ViewState["JobNum"] = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            
            if (dsJobDetail.Tables[0].Rows.Count > 0)
            {
                if (dsJobDetail.Tables[0].Rows[0]["Form13Required"] != DBNull.Value &&
                    dsJobDetail.Tables[0].Rows[0]["Form13Required"].ToString().Trim().ToLower() == "yes")
                {
                    // TAB : FORM 13 TAB 
                    accForm13.Visible = true;
                }
                else
                {
                    accForm13.Visible = false; // TAB : FORM 13 TAB 
                }
                
            }
        }

        DataSet dsPCDDetail = EXOperations.EXGetPCDDetail(JobId);

        if (dsPCDDetail.Tables[0].Rows.Count > 0)
        {
            fvPCD.DataSource = dsPCDDetail;
            fvPCD.DataBind();

            int BillingDeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["BillingDeliveryId"]);
            int PCADeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["PCADeliveryId"]);

            Panel pnlDispatchBillingHand = (Panel)fvPCD.FindControl("pnlDispatchBillingHand");
            Panel pnlDispatchBillingCour = (Panel)fvPCD.FindControl("pnlDispatchBillingCour");

            Panel pnlDispatchPCAHand = (Panel)fvPCD.FindControl("pnlDispatchPCAHand");
            Panel pnlDispatchPCACour = (Panel)fvPCD.FindControl("pnlDispatchPCACour");


            if (BillingDeliveryId == 1)
                pnlDispatchBillingHand.Visible = true;
            else if (BillingDeliveryId == 2)
                pnlDispatchBillingCour.Visible = true;

            if (PCADeliveryId == 1)
                pnlDispatchPCAHand.Visible = true;
            else if (PCADeliveryId == 2)
                pnlDispatchPCACour.Visible = true;
        }
    }

    private void GetJobDetail(int JobId)
    {
        DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();
        }
    }
    public string GetBooleanToYesNo(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "YES";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "NO";
        }

        return strReturnText;
    }

    public bool CheckNullBooleanToTrueFalse(object mybValue)
    {
        bool bReturnValue = false;

        if (mybValue != DBNull.Value)
        {
            bReturnValue = Convert.ToBoolean(mybValue);
        }

        return bReturnValue;
    }

    public string GetBooleanToCompletedPending(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Completed";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Pending";
        }

        return strReturnText;
    }

    public string GetBooleanToApprovedRejected(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Approved";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Rejected";
        }

        return strReturnText;
    }
    
    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        string strReutrnUrl = ((Button)sender).CommandArgument.ToString();
        Response.Redirect(strReutrnUrl);
    }
    
    #region JOB DETAIL FORM VIEW EVENTS
    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {
        if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
        {            
            DataRowView drv = (DataRowView)FVJobDetail.DataItem;
            if (drv["TransportBy"] != DBNull.Value && drv["TransportBy"].ToString().Trim().ToLower() == "babaji shivram")
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = true;
            }
            else
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = false;
            }

            if (drv["TransMode"] != DBNull.Value && drv["TransMode"].ToString().Trim().ToLower() == "air")
            {
                TabSeaContainer.Visible = false;
            }
            else
            {
                TabSeaContainer.Visible = true;
            }
                        
        }
        
    }
    
    #endregion

    #region SB PREPARE FORM VIEW EVENTS
    protected void FVSBPrepare_DataBound(object sender, EventArgs e)
    {
        if (FVSBPrepare.CurrentMode == FormViewMode.ReadOnly)
        {            
            // Checklist Download
            LinkButton lnkChecklistDoc = (LinkButton)FVSBPrepare.FindControl("lnkChecklistDoc_JobDetail");
            HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnChecklistDocPath2");
            ScriptManager1.RegisterPostBackControl(lnkChecklistDoc);

            if (hdnChecklistDocPath.Value.Trim() != "")
            {
                lnkChecklistDoc.Text = "Download";
                lnkChecklistDoc.Enabled = true;
            }
        }
    }
        
    #endregion
    
    #region SHIPMENT GET IN FORM VIEW EVENTS
  
    protected void FVShipmentGetIN_DataBound(object sender, EventArgs e)
    {
        if (FVShipmentGetIN.CurrentMode == FormViewMode.ReadOnly)
        {
            // Exporter Copy Download
            LinkButton lnkDwnloadExporterCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkDwnloadExporterCopy");
            HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnExporterCopy");
            ScriptManager1.RegisterPostBackControl(lnkDwnloadExporterCopy);

            if (hdnExporterCopy.Value.Trim() != "")
            {
                lnkDwnloadExporterCopy.Text = "Download";
                lnkDwnloadExporterCopy.Enabled = true;
            }

            // VGM Copy Download
            LinkButton lnkDwnloadVGMCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkDwnloadVGMCopy");
            HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
            ScriptManager1.RegisterPostBackControl(lnkDwnloadVGMCopy);

            if (hdnVGMCopy.Value.Trim() != "")
            {
                lnkDwnloadVGMCopy.Text = "Download";
                lnkDwnloadVGMCopy.Enabled = true;
            }
        }
    }
    #endregion
        
    #region Delivery Details    
    protected void GridViewWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int Mode = (Int32)DataBinder.Eval(e.Row.DataItem, "TransModeId");
            bool IsNForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsNForm");
            bool IsSForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsSForm");
            bool IsOctroi = (bool)DataBinder.Eval(e.Row.DataItem, "IsOctroi");
            bool IsRoadPermit = (bool)DataBinder.Eval(e.Row.DataItem, "IsRoadPermit");

            if (Mode == (Int32)TransMode.Sea) // Delivery Type
            {
                GridViewWarehouse.Columns[2].Visible = true; // Container NO
                GridViewWarehouse.Columns[3].Visible = false; // No Of Packages
            }
            else
            {
                GridViewWarehouse.Columns[2].Visible = false; // Container NO
                GridViewWarehouse.Columns[3].Visible = true; // No Of Packages
            }

            if (IsNForm == true) // NForm Applicable
            {
                GridViewWarehouse.Columns[17].Visible = true; // N Form No
                GridViewWarehouse.Columns[18].Visible = true; // N Form Date
                GridViewWarehouse.Columns[19].Visible = true; // N Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[17].Visible = false; // N Form No
                GridViewWarehouse.Columns[18].Visible = false; // N Form Date
                GridViewWarehouse.Columns[19].Visible = false; // N Closing Date
            }

            if (IsSForm == true) // SForm Applicable
            {
                GridViewWarehouse.Columns[20].Visible = true; // S Form No
                GridViewWarehouse.Columns[21].Visible = true; // S Form Date
                GridViewWarehouse.Columns[22].Visible = true; // S Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[20].Visible = false; // S Form No
                GridViewWarehouse.Columns[21].Visible = false; // S Form Date
                GridViewWarehouse.Columns[22].Visible = false; // S Closing Date
            }
            if (IsOctroi == true) // Octroi Applicable
            {
                GridViewWarehouse.Columns[23].Visible = true; // Octroi Amount
                GridViewWarehouse.Columns[24].Visible = true; // Octroi Receipt No	
                GridViewWarehouse.Columns[25].Visible = true; // Octroi Paid Date
            }
            else
            {
                GridViewWarehouse.Columns[23].Visible = false; // Octroi Amount
                GridViewWarehouse.Columns[24].Visible = false; // Octroi Receipt No	
                GridViewWarehouse.Columns[25].Visible = false; // Octroi Paid Date
            }
            if (IsRoadPermit == true) // Road Permit Applicable
            {
                GridViewWarehouse.Columns[13].Visible = true; // Road Permit No
                GridViewWarehouse.Columns[14].Visible = true; // Road Permit Date
            }
            else
            {
                GridViewWarehouse.Columns[13].Visible = false; // Road Permit No
                GridViewWarehouse.Columns[14].Visible = false; // Road Permit Date
            }
        }
    }
    private void DownloadPODDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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
    #endregion
    
    #region Daily Activity
    
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
                    LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkActDownload");
                    lnkDownload.Visible = false;
                }

                if (strProgressText.Length > 30)
                {
                    lnkMoreProgress.ToolTip = strProgressText;
                    
                }
                else
                {
                    lnkMoreProgress.Visible = false;
                }
            }
        }

    }
    
    #region Billing Status
    protected void gvBillingStatus_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvBillingStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
                HyperLink lnkBillProgress = (HyperLink)e.Row.FindControl("lnkBillProgress");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }

                if (strProgressText.Length > 40)
                {
                    lnkBillProgress.ToolTip = strProgressText;
                }
                else
                {
                    lnkBillProgress.Visible = false;
                }
            }
        }
    }

    #endregion

    #endregion

    #region Document Download / Upload
    protected void FVJobHistory_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void FVJobHistory_prerender(Object Sender, EventArgs e)
    {
        GridView gv = (GridView)Sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            //gv.TopPagerRow.Visible = true;
        }
    }
    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewDocument_RowDataBound(Object Sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkDownload");
        //    ScriptManager1.RegisterPostBackControl(GridViewDocument);
        //}

    }
    protected void GridViewDocument_prerender(Object Sender, EventArgs e)
    {
        GridView gv = (GridView)Sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            //gv.TopPagerRow.Visible = true;
        }
    }
    protected void GridViewPCADoc_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            //DownloadPCADocument(DocPath);
        }
    }
    protected void GridViewBillingAdvice_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            //DownloadBillAdviceDocument(DocPath);
        }
    }
    protected void GridViewBillingDept_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewBackOfficeDoc_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            // DownloadPCADocument(DocPath);
        }
    }
    protected void GrvShipmentDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void lnkChecklistDoc_JobDetail_Click(object sender, EventArgs e)
    {
        HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnChecklistDocPath2");
        string FilePath = hdnChecklistDocPath.Value.Trim();
        DownloadDocumentChecklist(FilePath);
    }
    protected void lnkDwnloadExporterCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnExporterCopy");
        string FilePath = hdnExporterCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkDwnloadVGMCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
        string FilePath = hdnVGMCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkEditDwnloadExporterCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnEditExporterCopy");
        string FilePath = hdnExporterCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkEditDwnloadVGMCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
        string FilePath = hdnVGMCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
            ServerPath = HttpContext.Current.Server.MapPath("~/UploadExportFiles/" + DocumentPath);
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
    protected void DownloadDocumentChecklist(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
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
    protected void DownloadDocument2(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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
    protected void DownloadDocument3(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

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
    protected void DownloadBillAdviceDocument(string DocumentPath)
    {

        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("~/UploadFiles/" + DocumentPath);
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
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        lblError.Visible = true;
        string strFilePath = "", filePath = "";
        int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);
        int JobId = Convert.ToInt32(Session["JobId"]);

        DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
        if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
            strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

        if (JobId > 0)
        {
            if (DocumentId == 0)
            {
                lblError.Text = "Please Select Document Type!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (fuDocument.FileName.Trim() == "")
            {
                lblError.Text = "Please Browse The Document!";
                lblError.CssClass = "errorMsg";
                return;
            }

            filePath = UploadFiles(fuDocument, strFilePath);
            if (filePath != "")
            {
                int result_DocSaved = EXOperations.Ex_AddPreAlertDocs(filePath, DocumentId, JobId, LoggedInUser.glUserId);
                if (result_DocSaved == 0)
                {
                    lblError.Text = "Document uploaded successfully!";
                    lblError.CssClass = "success";
                    GridViewDocument.DataBind();
                    ddDocument.SelectedValue = "0";
                }
                else if (result_DocSaved == 2)
                {
                    lblError.Text = "Document with same type already exists!!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System Error. Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Select File!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            Response.Redirect("SBPreparation.aspx");
        }
    }
    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

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

    #region PCA & PCA Document  
    protected void gvPCDDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void lnkPODCopyDownoad_Click(object sender, EventArgs e)
    {
        LinkButton lnkPODDownload = (LinkButton)sender;

        string FilePath = lnkPODDownload.CommandArgument;

        DownloadDocument3(FilePath);
    }
    protected void PCDDocumentSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            lblError.Text = "System Error! Please contact system administrator. Event Name:PCDDocumentSqlDataSource_Selected.";
            lblError.CssClass = "errorMsg";

            e.ExceptionHandled = true;
        }
    }

    #endregion
    
    #region INVOICE DETAILS EVENTS
    
    protected void gvInvoiceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvInvoiceDetail.PageIndex = e.NewPageIndex;
        gvInvoiceDetail.DataBind();
    }

    #endregion
}




