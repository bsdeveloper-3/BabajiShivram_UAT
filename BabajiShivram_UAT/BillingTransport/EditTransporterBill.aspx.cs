using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Drawing;

public partial class BillingTransport_EditTransporterBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Rejected Bill Details";
            if (Session["JobId"] != null)
                JobDetailMS(Convert.ToInt32(Session["JobId"]));

            DBOperations.FillVehicleType(ddVehicleType);
            fsEditDelivery.Visible = false;
            fsVehicleDetail.Visible = false;
            fsAllVehicles.Visible = true;
            gvVehicleDetails.Visible = true;
            btnBackToVehicleDetail.Visible = false;
            gvVehicleDetails.DataBind();
            GetInvoiceCopyDetails();
            DataSet dsGetJobDet = new DataSet();
            dsGetJobDet = DBOperations.GetJobDetailByJobId(Convert.ToInt32(Session["JobId"]));

            #region INCLUDE NFORM DETAIL

            int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["JobId"]));
            if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
            {
                fsNform.Visible = true;
                gvNFormDetail.Visible = true;
                gvNFormDetail.DataBind();
            }
            else
            {
                fsNform.Visible = false;
                gvNFormDetail.Visible = false;
            }

            #endregion
        }
    }

    protected void btnUpdateDelivery_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnLid.Value != "")
            {
                int DetentionDays = 0, JobId = 0;

                DateTime dtReportDate = DateTime.MinValue;
                DateTime dtUnloadDate = DateTime.MinValue;
                DateTime dtDeliveredDate = DateTime.MinValue;
                DateTime dtEmptyReturnDate = DateTime.MinValue;
                DateTime dtNClosingDate = DateTime.MinValue;

                if (txtNClosingDate.Text.Trim() != "")
                    dtNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim());
                if (txtReportDate.Text.Trim() != "")
                    dtReportDate = Commonfunctions.CDateTime(txtReportDate.Text.Trim());
                if (txtUnloadDate.Text.Trim() != "")
                    dtUnloadDate = Commonfunctions.CDateTime(txtUnloadDate.Text.Trim());
                if (txtDeliveryDate.Text.Trim() != "")
                    dtDeliveredDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim());
                if (txtEmptyReturnDate.Text.Trim() != "")
                    dtEmptyReturnDate = Commonfunctions.CDateTime(txtEmptyReturnDate.Text.Trim());
                if (txtDetentionDay.Text != "")
                    DetentionDays = Convert.ToInt32(txtDetentionDay.Text);

                JobId = Convert.ToInt32(Session["JobId"]);

                #region CALCULATE TOTAL

                double DetentionCharges = 0.0, OtherCharges = 0.0, Total = 0.0, FrightRate = 0.0, VaraiCharges = 0.0, EmptyOffLoadingCharges = 0.0, TempoUnionCharges = 0.0;

                if (txtFrightRate.Text != "")                                                       // TP Fright Rate
                    FrightRate = Convert.ToDouble(txtFrightRate.Text);
                if (txtDetentionCharges.Text != "")                                                 // Detention Charges
                    DetentionCharges = Convert.ToDouble(txtDetentionCharges.Text);
                if (txtVaraiCharges.Text != "")                                                     // Varai Charges
                    VaraiCharges = Convert.ToDouble(txtVaraiCharges.Text);
                if (txtEmptyOffLoadingCharges.Text != "")                                           // Empty Of Loading Charges
                    EmptyOffLoadingCharges = Convert.ToDouble(txtEmptyOffLoadingCharges.Text);
                if (txtTempoUnionCharges.Text != "")                                                // Tempo Union charges
                    TempoUnionCharges = Convert.ToDouble(txtTempoUnionCharges.Text);
                OtherCharges = VaraiCharges + EmptyOffLoadingCharges + TempoUnionCharges;           // Other Charges = Varai + Empty Of Loading + Tempo Union

                Total = Convert.ToDouble((DetentionCharges + OtherCharges) * FrightRate * Convert.ToDouble(DetentionDays));
                // Total of both w.r.t. fright rate and detention days

                #endregion

                #region DOC UPLOAD

                string strCustDocFolder = "", strJobFileDir = "", strFilePath_LRCopies = "", strUploadPath = "", strFilePath_Receipt = "", strFilePath_Invoice = ""; ;
                DataSet dsJobDetail = DBOperations.GetJobDetail(Convert.ToInt32(Session["JobId"]));                   // Get Job Detail
                if (dsJobDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsJobDetail.Tables[0].Rows[0]["DocFolder"] != DBNull.Value)
                        strCustDocFolder = dsJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\";

                    if (dsJobDetail.Tables[0].Rows[0]["FileDirName"] != DBNull.Value)
                        strJobFileDir = dsJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
                    strUploadPath = strCustDocFolder + strJobFileDir;
                }

                if (fuUploadLrCopies.HasFile != false && fuUploadLrCopies.FileName.Trim() != "")
                    strFilePath_LRCopies = UploadDocument(strUploadPath, fuUploadLrCopies);
                if (fuUploadReceipt.HasFile != false && fuUploadReceipt.FileName.Trim() != "")
                    strFilePath_Receipt = UploadDocument(strUploadPath, fuUploadReceipt);

                #endregion

                int result = DBOperations.UpdateTransportVehicleDetail(Convert.ToInt32(hdnLid.Value), dtDeliveredDate, txtFrightRate.Text.Trim(), dtReportDate, dtUnloadDate,
                             DetentionDays, DetentionCharges.ToString(), Total.ToString(), VaraiCharges.ToString(), EmptyOffLoadingCharges.ToString(), TempoUnionCharges.ToString(),
                             txtRemarks.Text.Trim(), dtEmptyReturnDate, strFilePath_LRCopies, strFilePath_Receipt, Convert.ToInt32(Session["VendorId"]));
                if (result == 0)
                {
                    #region ADD NFORM DETAIL & NFORM DOCUMENT

                    DateTime dtNFormDate = DateTime.MinValue;
                    if (txtNFormDate.Text.Trim() != "")
                        dtNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim());

                    if (lblNFormNo.Text != "" && dtNFormDate != DateTime.MinValue)
                    {
                        int NformInserted = DBOperations.UpdateNformByTransporter(Convert.ToInt32(hdnDeliveryLid.Value), dtNClosingDate, dtNFormDate, Convert.ToInt32(Session["VendorId"]), JobId);

                        ///////////////////////////////////  NFORM DOCUMENT UPLOAD  //////////////////////////////////////////

                        string strFilePath = "";
                        if (fuNformDoc.FileName.Trim() != "")
                        {
                            strFilePath = UploadDocument(strUploadPath, fuNformDoc);
                            int Result_NForm = DBOperations.AddPCDDocument_Nform(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(47), Convert.ToString(""), strFilePath, Convert.ToInt32(Session["VendorId"]));
                        }
                    }

                    #endregion

                    ResetControls();
                    lberror.Text = "Successfully saved billing vehicle details.";
                    lberror.CssClass = "success";
                    fsVehicleDetail.Visible = false;
                    btnBackToVehicleDetail.Visible = false;
                    fsEditDelivery.Visible = false;
                    fsAllVehicles.Visible = true;
                    #region INCLUDE NFORM DETAIL

                    int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["JobId"]));
                    if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
                    {
                        fsNform.Visible = true;
                        gvNFormDetail.Visible = true;
                        gvNFormDetail.DataBind();
                    }
                    else
                    {
                        fsNform.Visible = false;
                        gvNFormDetail.Visible = false;
                    }

                    #endregion
                    gvVehicleDetails.Visible = true;
                    if (Convert.ToString(Session["JobId"]) != "")
                        gvVehicleDetails.DataBind();
                    GetInvoiceCopyDetails();
                }
                else if (result == 1)
                {
                    lberror.Text = "Error while saving billing vehicle details.";
                    lberror.CssClass = "errorMsg";
                }
                else
                {
                    lberror.Text = "Record already exists for transporter and vehicle.";
                    lberror.CssClass = "errorMsg";
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void btnSaveChanges_OnClick(object sender, EventArgs e)
    {
        try
        {
            int result = DBOperations.UpdateJobBillStatus(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(0), Convert.ToInt32(LoggedInUser.glUserId),
                                                          Convert.ToInt32(0), Convert.ToDateTime(DateTime.MinValue), Convert.ToString(""), Convert.ToString(""));
            if (result == 0)
            {
                fsEditDelivery.Visible = false;
                fsVehicleDetail.Visible = false;
                //tblJObDet.Visible = false;
                gvVehicleDetails.Visible = false;
                btnBackToVehicleDetail.Visible = false;
                lberror.Text = "Successfully saved bill changes. Bill moved to Transport department .";
                lberror.CssClass = "success";
                gvVehicleDetails.DataBind();
            }
            else if (result == 1)
            {
                lberror.Text = "Error while approving bill status. Please try again later.";
                lberror.CssClass = "errorMsg";
            }
        }
        catch (Exception en)
        {
        }

    }

    protected void imgbtnDwnInvoice_OnClick(object sender, EventArgs e)
    {
        try
        {
            string InvoiceCopyPath = "";
            DataSet dsTransporterBillDet = new DataSet();
            dsTransporterBillDet = DBOperations.GetBillInvoiceCopy(Convert.ToInt32(Session["JobId"]));
            if (dsTransporterBillDet != null)
            {
                if (dsTransporterBillDet.Tables[0].Rows.Count > 0)
                {
                    InvoiceCopyPath = dsTransporterBillDet.Tables[0].Rows[0]["DocPath"].ToString();
                    DownloadDocument(InvoiceCopyPath);
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void btnBackToVehicleDetail_OnClick(object sender, EventArgs e)
    {
        ResetControls();
        fsVehicleDetail.Visible = false;
        fsEditDelivery.Visible = false;
        fsAllVehicles.Visible = true;
        #region INCLUDE NFORM DETAIL

        int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["JobId"]));
        if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
        {
            fsNform.Visible = true;
            gvNFormDetail.Visible = true;
            gvNFormDetail.DataBind();
        }
        else
        {
            fsNform.Visible = false;
            gvNFormDetail.Visible = false;
        }

        #endregion
        btnBackToVehicleDetail.Visible = false;
        if (Convert.ToString(Session["JobId"]) != "")
            gvVehicleDetails.DataBind();
        gvVehicleDetails.Visible = true;
        //txtJobRefNo.Visible = false;
    }

    protected void lnkDownloadInvoice_OnClick(object sender, EventArgs e)
    {
        string DocPath = hdnCopyPath.Value;
        if (DocPath != "")
            DownloadDocument(DocPath);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
        fsVehicleDetail.Visible = false;
        fsEditDelivery.Visible = false;
        fsAllVehicles.Visible = true;
        #region INCLUDE NFORM DETAIL

        int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["JobId"]));
        if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
        {
            fsNform.Visible = true;
            gvNFormDetail.Visible = true;
            gvNFormDetail.DataBind();
        }
        else
        {
            fsNform.Visible = false;
            gvNFormDetail.Visible = false;
        }

        #endregion
        btnBackToVehicleDetail.Visible = false;
        // txtJobRefNo.Visible = false;
        if (Convert.ToString(Session["JobId"]) != "")
            gvVehicleDetails.DataBind();
        gvVehicleDetails.Visible = true;
    }

    protected void ResetControls()
    {
        txtDeliveryDate.Text = "";
        txtDetentionCharges.Text = "";
        txtDetentionDay.Text = "";
        txtEmptyReturnDate.Text = "";
        txtFrightRate.Text = "";
        txtVaraiCharges.Text = "";
        txtEmptyOffLoadingCharges.Text = "";
        txtTempoUnionCharges.Text = "";
        txtRemarks.Text = "";
        txtReportDate.Text = "";
        txtUnloadDate.Text = "";
        ddVehicleType.SelectedValue = "0";
        lblDeliveryLid.Text = "";
        lblTotal.Text = "";
        lblVehicleType.Text = "";
        lblDeliveryFrom.Text = "";
        lblDestination.Text = "";
        lblVehicleNo.Text = "";
        lblTransporterName.Text = "";
        lblDispatchDate.Text = "";
    }

    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);
        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("Dashboard.aspx");
            Session["JobId"] = null;
        }

        lblTitle.Text = "Rejected Bill Details - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
    }

    #region GRID VIEW EVENTS

    protected void GetInvoiceCopyDetails()
    {
        try
        {

            if (Convert.ToString(Session["StatusId"]) != null && Convert.ToString(Session["StatusId"]) != "")
            {
                if (Convert.ToString(Session["StatusId"]) == "0")
                {
                    //trSaveChanges.Visible = false;
                    //trUploadNewCopy.Visible = true;
                    //trCurrInvCopy.Visible = false;
                }
                else if (Convert.ToString(Session["StatusId"]) == "1")
                {
                    //trSaveChanges.Visible = true;
                    //trUploadNewCopy.Visible = false;
                    // trCurrInvCopy.Visible = true;
                }
                else
                {
                    //trSaveChanges.Visible = false;
                    //trUploadNewCopy.Visible = false;
                    //trCurrInvCopy.Visible = true;
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void gvVehicleDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = gvVehicleDetails.DataKeys[gvrow.RowIndex].Values[0].ToString();
            string strDeliveryId = gvVehicleDetails.DataKeys[gvrow.RowIndex].Values[1].ToString();

            if (strlId != "")
            {
                #region INCLUDE NFORM DETAIL

                int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["JobId"]));
                if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
                {
                    //Then show the nform details section
                    trNFormDetail1.Visible = true;
                    trNFormDetail2.Visible = true;
                }
                else
                {
                    trNFormDetail1.Visible = false;
                    trNFormDetail2.Visible = false;
                }

                #endregion

                fsVehicleDetail.Visible = true;
                fsEditDelivery.Visible = true;
                fsAllVehicles.Visible = false;
                hdnDeliveryLid.Value = strDeliveryId;

                DataSet dsGetJobDelDetails = new DataSet();
                dsGetJobDelDetails = DBOperations.GetTOP_VehicleDetail(Convert.ToInt32(strlId));

                if (dsGetJobDelDetails != null && dsGetJobDelDetails.Tables[0].Rows.Count > 0)
                {
                    fsEditDelivery.Visible = true;
                    fsVehicleDetail.Visible = true;
                    fsAllVehicles.Visible = false;
                    fsNform.Visible = false;
                    gvNFormDetail.Visible = false;
                    //btnBackToVehicleDetail.Visible = true;

                    lblVehicleNo.Text = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleNo"].ToString();
                    ddVehicleType.SelectedValue = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleType"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["VehicleTypeName"].ToString() != "")
                        lblVehicleType.Text = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleTypeName"].ToString();
                    lblTransporterName.Text = dsGetJobDelDetails.Tables[0].Rows[0]["TransporterName"].ToString();
                    lblDeliveryFrom.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryFrom"].ToString();
                    lblDestination.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryPoint"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"].ToString() != "")
                        lblDispatchDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"]).ToString("dd/MM/yyyy");
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["LRDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["LRDate"].ToString() != "")
                        lblLRDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["LRDate"]).ToString("dd/MM/yyyy");
                    lblLRNo.Text = dsGetJobDelDetails.Tables[0].Rows[0]["LRNo"].ToString();

                    hdnLid.Value = strlId.ToString();
                    txtFrightRate.Text = dsGetJobDelDetails.Tables[0].Rows[0]["TPFrightRate"].ToString();
                    txtRemarks.Text = dsGetJobDelDetails.Tables[0].Rows[0]["Remarks"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryDate"].ToString() != "")
                        txtDeliveryDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryDate"]).ToString("dd/MM/yyyy");
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["ReportDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["ReportDate"].ToString() != "")
                        txtReportDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["ReportDate"]).ToString("dd/MM/yyyy");
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["UnloadDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["UnloadDate"].ToString() != "")
                        txtUnloadDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["UnloadDate"]).ToString("dd/MM/yyyy");
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["EmptyContReturnDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["EmptyContReturnDate"].ToString() != "")
                        txtEmptyReturnDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["EmptyContReturnDate"]).ToString("dd/MM/yyyy");
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["DetentionDays"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["DetentionDays"].ToString() != "")
                    {
                        if (dsGetJobDelDetails.Tables[0].Rows[0]["DetentionDays"].ToString() != "0")
                        {
                            txtDetentionDay.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DetentionDays"].ToString();
                        }
                    }
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["DetentionCharges"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["DetentionCharges"].ToString() != "")
                    {
                        if (dsGetJobDelDetails.Tables[0].Rows[0]["DetentionCharges"].ToString() != "0.00")
                        {
                            txtDetentionCharges.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DetentionCharges"].ToString();
                        }
                    }
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["WaraiCharges"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["WaraiCharges"].ToString() != "")
                    {
                        if (dsGetJobDelDetails.Tables[0].Rows[0]["WaraiCharges"].ToString() != "0.00")
                        {
                            txtVaraiCharges.Text = dsGetJobDelDetails.Tables[0].Rows[0]["WaraiCharges"].ToString();
                        }
                    }
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["EmptyOffLoadingCharges"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["EmptyOffLoadingCharges"].ToString() != "")
                        txtEmptyOffLoadingCharges.Text = dsGetJobDelDetails.Tables[0].Rows[0]["EmptyOffLoadingCharges"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["TempoUnionCharges"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["TempoUnionCharges"].ToString() != "")
                        txtTempoUnionCharges.Text = dsGetJobDelDetails.Tables[0].Rows[0]["TempoUnionCharges"].ToString();
                }

                DataSet dsGetNformDetail = new DataSet();
                dsGetNformDetail = DBOperations.GetNformDetail(Convert.ToInt32(Session["JobId"]));
                if (dsGetNformDetail != null && dsGetNformDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsGetNformDetail.Tables[0].Rows[0]["NFormNo"].ToString() != "")
                    {
                        lblNFormNo.Text = dsGetNformDetail.Tables[0].Rows[0]["NFormNo"].ToString();
                        if (dsGetNformDetail.Tables[0].Rows[0]["NClosingDate"].ToString() != null && dsGetNformDetail.Tables[0].Rows[0]["NClosingDate"].ToString() != "")
                            txtNClosingDate.Text = Convert.ToDateTime(dsGetNformDetail.Tables[0].Rows[0]["NClosingDate"]).ToString("dd/MM/yyyy");
                        if (dsGetNformDetail.Tables[0].Rows[0]["NFormDate"].ToString() != null && dsGetNformDetail.Tables[0].Rows[0]["NFormDate"].ToString() != "")
                            txtNFormDate.Text = Convert.ToDateTime(dsGetNformDetail.Tables[0].Rows[0]["NFormDate"]).ToString("dd/MM/yyyy");
                        hdnCopyPath.Value = dsGetNformDetail.Tables[0].Rows[0]["NformDocPath"].ToString();

                        // hdnNFormNo.Value = "1";
                    }
                    else
                    {
                        //hdnNFormNo.Value = "";
                        trNFormDetail1.Visible = false;
                        trNFormDetail2.Visible = false;
                    }
                }
            }
        }

        if (e.CommandName.ToLower() == "downloadcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
    }

    protected void gvVehicleDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgbtnLRDoc = (ImageButton)e.Row.FindControl("imgbtnLRDoc");
            if (imgbtnLRDoc != null)
                ScriptManager1.RegisterPostBackControl(imgbtnLRDoc);

            ImageButton imgbtnReceipt = (ImageButton)e.Row.FindControl("imgbtnReceipt");
            if (imgbtnReceipt != null)
                ScriptManager1.RegisterPostBackControl(imgbtnReceipt);

            string strLrCopies = (string)DataBinder.Eval(e.Row.DataItem, "LrCopiesDocPath").ToString();
            if (strLrCopies == "")
            {
                e.Row.Cells[19].Text = "";
            }

            string strReceipt = (string)DataBinder.Eval(e.Row.DataItem, "ReceiptDocPath").ToString();
            if (strReceipt == "")
            {
                e.Row.Cells[20].Text = "";
            }

            string strVehicleNo = (string)DataBinder.Eval(e.Row.DataItem, "VehicleNo").ToString();
            if (strVehicleNo.ToLower() == "sub total")
            {
                e.Row.Cells[0].Text = "";
                e.Row.Cells[1].Text = "";
            }

            string strRemark = (string)DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
            if (strRemark == "")
            {
                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemarks");
                if (txtRemark != null)
                    txtRemark.Visible = false;
            }

            string strUpdDate = (string)DataBinder.Eval(e.Row.DataItem, "UpdDate").ToString();
            if (strUpdDate != "")
            {
                e.Row.Cells[0].Text = "";
            }

            string strBillStatusID = (string)DataBinder.Eval(e.Row.DataItem, "BillStatusId").ToString();
            if (strBillStatusID != "" && strBillStatusID == "0")
            {
                e.Row.Cells[0].Text = "";
            }

        }
    }

    protected void gvVehicleDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvNFormDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "downloadnformcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
    }

    #endregion

    #region DOCUMENTS DOWNLOAD EVENTS

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

    private string UploadDocument(string FilePath, FileUpload fuUpload)
    {
        string FileName = fuUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "BillVehicle_" + Session["JobId"].ToString() + "\\";

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
        if (fuUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuUpload.SaveAs(ServerFilePath + FileName);
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

    #endregion
}