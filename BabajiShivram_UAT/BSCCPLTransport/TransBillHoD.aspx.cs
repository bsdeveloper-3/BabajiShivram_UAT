using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.Text;
using Ionic.Zip;
using QueryStringEncryption;
public partial class BSCCPLTransport_TransBillHoD : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(imgbtnPackingList);
        ScriptManager1.RegisterPostBackControl(gvSellingDetail);

        if (Session["TRId"] == null)
        {
            Response.Redirect("TransBillHoD.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Bill - HoD Approval";
            hdnTransBillId.Value = Session["TransBillId"].ToString();
            TruckRequestDetail(Convert.ToInt32(Session["TRId"]));
        }
    }

    protected void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            fsGeneralTransportDetails.Visible = true;
            gvTransportJobDetail.Visible = false;
            fsConsolidateJobs.Visible = false;

            int JobId = Convert.ToInt32(dvDetail.Table.Rows[0]["JobId"].ToString());
            if (JobId > 0)
                hdnJobId.Value = JobId.ToString();
            //ddVehicleNo.DataBind();
            //ddVehicleNo_SelectedIndexChanged(null, EventArgs.Empty);
            lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
            lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            if (Session["TRConsolidateId"] != null && Session["TRConsolidateId"].ToString() != "0")
            {
                fsGeneralTransportDetails.Visible = false;
                gvTransportJobDetail.DataBind();
                if (gvTransportJobDetail != null)
                {
                    if (gvTransportJobDetail.Rows.Count > 0)
                    {
                        gvTransportJobDetail.Visible = true;
                        fsConsolidateJobs.Visible = true;
                    }
                }

                if (GridViewVehicle != null)
                {
                    GridViewVehicle.HeaderRow.Cells[15].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[16].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[17].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[18].Visible = false;

                    for (int i = 0; i < GridViewVehicle.Rows.Count; i++)
                    {
                        GridViewVehicle.Rows[i].Cells[15].Visible = false;
                        GridViewVehicle.Rows[i].Cells[16].Visible = false;
                        GridViewVehicle.Rows[i].Cells[17].Visible = false;
                        GridViewVehicle.Rows[i].Cells[18].Visible = false;
                    }
                }
            }
            //DataSet dsGetTransBillDetail = DBOperations.GetBillReceivedDetail(Convert.ToInt32(hdnTransBillId.Value));
            //if (dsGetTransBillDetail != null && dsGetTransBillDetail.Tables.Count > 0)
            //{
            //    if (dsGetTransBillDetail.Tables[0].Rows[0]["StatusId"] != DBNull.Value)
            //    {
            //        ddlStatus.SelectedValue = dsGetTransBillDetail.Tables[0].Rows[0]["StatusId"].ToString();
            //        if (ddlStatus.SelectedValue != "0")
            //        {
            //            ddlStatus.Enabled = false;
            //        }
            //        txtChequeNo.Text = dsGetTransBillDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
            //        if (txtChequeNo.Text.Trim() != "")
            //        {
            //            txtChequeNo.Enabled = false;
            //        }
            //        if (dsGetTransBillDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
            //        {
            //            txtChequeDate.Text = Convert.ToDateTime(dsGetTransBillDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");
            //            txtChequeDate.Enabled = false;
            //        }
            //        txtHoldReason.Text = dsGetTransBillDetail.Tables[0].Rows[0]["HoldReason"].ToString();
            //        if (txtHoldReason.Text.Trim() != "")
            //        {
            //            txtHoldReason.Enabled = false;
            //        }
            //        txtReleaseDate.Text = dsGetTransBillDetail.Tables[0].Rows[0]["ReleaseDate"].ToString();
            //        if (txtReleaseDate.Text.Trim() != "")
            //        {
            //            txtReleaseDate.Enabled = false;
            //        }
            //    }
            //}
            ////gvSellingDetail.DataBind();
            ////if (gvSellingDetail.Rows.Count == 0)
            ////{
            ////    btnSaveBill.Visible = false;
            ////    lblError_BillStatus.Text = "Please add selling rate before proceeding with bill status!";
            ////    lblError_BillStatus.CssClass = "errorMsg";
            ////}
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("TransBillPendingHOD.aspx");
    }

    protected void btnApproveBill_Click(object sender, EventArgs e)
    {
        if (hdnTransBillId.Value != "" && hdnTransBillId.Value != "0")
        {
            int BillId = Convert.ToInt32(hdnTransBillId.Value);
            int TranRequestId = Convert.ToInt32(Session["TRId"]);
            int BillStatus = (Int32) EnumInvoiceStatus.HODApproved;

            if (BillId > 0)
            {
                int result = DBOperations.AddBillReceivedDetail(BillId, BillStatus, txtRemark.Text.Trim(), LoggedInUser.glUserId);

                if (result == 0)
                {
                    result = DBOperations.AddTransBillPaymentRequest(BillId,TranRequestId, txtRemark.Text.Trim(), LoggedInUser.glUserId);

                    lblError.Text = "Bill Payment detail Successfully Sent for Account Audit!";
                    lblError.CssClass = "success";
                    Session["Success"] = lblError.Text;

                    Response.Redirect("../AccountTransport/TransportSuccess.aspx");
                }
                else if (result == 3)
                {
                    lblError.Text = "Previous Fin Year Bill Submission Not Allowed. Please Contact A/C Dept.";
                    lblError.CssClass = "errorMsg";
                    gvBillStatusHistory.DataBind();
                }
                else
                {
                    lblError.Text = "System error. Please try again later.";
                    lblError.CssClass = "errorMsg";
                    gvBillStatusHistory.DataBind();
                }
            }
        }
    }
    protected void btnRejectBill_Click(object sender, EventArgs e)
    {
        if (hdnTransBillId.Value != "" && hdnTransBillId.Value != "0")
        {
            int BillId = Convert.ToInt32(hdnTransBillId.Value);
            int BillStatus = (Int32)EnumInvoiceStatus.HODReject;

            if (BillId > 0)
            {
                int result = DBOperations.AddBillReceivedDetail(BillId, BillStatus, txtRemark.Text.Trim(), LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Bill Rejected and detail Sent back To Transport Team!";
                    lblError.CssClass = "success";
                    Session["Success"] = lblError.Text;

                    Response.Redirect("TransportSuccess.aspx");

                }
                else if (result == 1)
                {
                    lblError.Text = "System error. Please try again later.";
                    lblError.CssClass = "errorMsg";
                    gvBillStatusHistory.DataBind();
                }
                else
                {
                    lblError.Text = "System error. Please try again later.";
                    lblError.CssClass = "errorMsg";
                    gvBillStatusHistory.DataBind();
                }
            }
        }
    }
    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument1(TransRequestId);
        }
    }

    private void DownloadDocument1(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "TransportFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion

    protected void ResetControls()
    {
        //txtRate.Text = "";
        //txtCity.Text = "";
        //txtDetentionAmount.Text = "";
        //txtVaraiExp.Text = "";
        //txtEmptyContCharges.Text = "";
        ////gvSellingRateDetail.DataBind();
        //txtOtherCharges.Text = "";
        //txtMarketBillingRate.Text = "";
    }

    protected void btnCancelSellingRate_Click(object sender, EventArgs e)
    {

    }

    protected bool SendMail(int BillId, string strHoldReason)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();

        if (BillId > 0)
        {
            string strCustomerEmail = "", strCCEmail = "", strSubject = "", MessageBody = "", EmailContent = "",
                strBillNo = "", strTRRefNo = "", strJobNo = "";

            DataView dvDetail = DBOperations.GetTransBillDetailById(BillId);
            if (dvDetail.Table.Rows.Count > 0)
            {
                strTRRefNo = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
                strBillNo = dvDetail.Table.Rows[0]["BillNumber"].ToString();
                strJobNo = dvDetail.Table.Rows[0]["JobRefNo"].ToString();

                strSubject = "Transport Bill holded";
                strCustomerEmail = "tp.helpdesk@babajishivram.com"; // "kivisha.jain@babajishivram.com"; // 
                strCCEmail = "ashwani.singh@babajishivram.com , priyank.jain@babajishivram.com";

                if (strCustomerEmail == "" || strSubject == "")
                    return false;
                else
                {

                    List<string> lstFileDoc = new List<string>();
                    MessageBody = "Dear Sir, <BR><BR>Bill has been holded for Babaji Job " + strJobNo + " and Truck Request "
                                              + strTRRefNo + "." + "<BR><BR>Please check out the reason for the same below.";
                    MessageBody = MessageBody + "<BR><BR><u>" + strHoldReason + "</u>";
                    MessageBody = MessageBody + "<BR><BR>Thank You<BR><b>This is system generated mail, please do not reply to this message via e-mail.</b>";

                    bEmailSuccess = EMail.SendMailBCC(strCustomerEmail, strCustomerEmail, strCCEmail, "javed.shaikh@babajishivram.com", strSubject, MessageBody, "");
                    return bEmailSuccess;
                }
            }
            else
                return false;
        }
        else
            return false;
    }

    #region SELLING RATE
    protected void gvSellingDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // btnSaveBill.Visible = true;
            if (DataBinder.Eval(e.Row.DataItem, "SellingId") != DBNull.Value) // selling data exists
            {
                // e.Row.Cells[17].Text = "Update";
                // e.Row.Enabled = false;

                //btnSaveBill.Visible = true;
                //lblError_BillStatus.Text = "";
            }
            else
            {
                //btnSaveBill.Visible = false;
                lblError_BillStatus.Text = "Please add selling rate before proceeding with bill status!";
                lblError_BillStatus.CssClass = "errorMsg";
            }
        }
    }

    protected void gvSellingDetail_PreRender(object sender, EventArgs e)
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

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
            //ServerPath = ServerPath + "Transport\\" + DocumentPath;
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

    protected void gvBillDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    //
    protected bool DecideHere(string Str)
    {
        if (Str == "0")
            return false;
        else
            return true;
    }

    protected void gvSellingDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "detentioncopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "varaicopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emptycontcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "tollcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "othercopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emailapprovalcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "contractcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
    }

    private void DownloadMultipleDocument(string DocumentPath, string Documentname)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (Documentname == "detentioncopy")
        {
            DocumentPath = "TransportDetentionDoc\\" + DocumentPath;
        }
        else if (Documentname == "varaicopy")
        {
            DocumentPath = "TransportVaraiDoc\\" + DocumentPath;
        }
        else if (Documentname == "emptycontcopy")
        {
            DocumentPath = "TransportEmptyContDoc\\" + DocumentPath;
        }
        else if (Documentname == "tollcopy")
        {
            DocumentPath = "TransportTollDoc\\" + DocumentPath;
        }
        else if (Documentname == "othercopy")
        {
            DocumentPath = "TransportOtherDoc\\" + DocumentPath;
        }
        else if (Documentname == "emailapprovalcopy")
        {
            DocumentPath = "EmailApprovalUpload\\" + DocumentPath;
        }
        else if (Documentname == "contractcopy")
        {
            DocumentPath = "TransportContractCopyUpload\\" + DocumentPath;
        }

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath(DocumentPath);
            ServerPath = ServerPath.Replace("Transport\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    #region Sales Bill Detail

    protected void gvSalesBillDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                DownloadDocument(strFilePath);
            }
            else
            {
                lblError.Text = "Bill Document Not Uploaded!";
                lblError.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                ViewDocument(strFilePath);
            }
            else
            {
                lblError.Text = "Bill Document Not Uploaded!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void gvSalesBillDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DocId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DocId"));
                if (DocId == 0)
                {
                    LinkButton lnkViewDoc = (LinkButton)e.Row.FindControl("lnkBillView");
                    LinkButton lnkBillDownload = (LinkButton)e.Row.FindControl("lnkBillDownload");

                    if (lnkViewDoc != null)
                    {
                        lnkViewDoc.Visible = false;
                    }
                    if (lnkBillDownload != null)
                    {
                        lnkBillDownload.Visible = false;
                    }

                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Document Not Uploaded";
                }
            }
        }
    }

    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
}