using System;
using System.Collections.Generic;
using System.Linq;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

public partial class AccountExpense_InvoiceApproval : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Approval";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceApproval.aspx");
        }

        if(!IsPostBack)
        {
            GetPaymentRequest(Convert.ToInt32(Session["InvoiceId"]));
        }
        else
        {
            // Check Invoice Status Change
            bool isStatusChanged = CheckInvoiceStatusChange();

            if (isStatusChanged == true)
            {
                lblError.Text = "Invoice details already updated! Please Check Status History";
                lblError.CssClass = "errorMsg";

                Session["Error"] = lblError.Text;

                Session["InvoiceId"] = null;

                Response.Redirect("AccountSuccess.aspx");
            }
        }
    }

    private void GetPaymentRequest(int InvoiceID)
    {
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        int StatusId = 0;
        int ExpenseTypeId = 0;

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);
            hdnStatusId.Value = StatusId.ToString();

            lblLTRefNo.Text = "LT" + InvoiceID.ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            lblWeight.Text = dsDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            lblContainerCount.Text = dsDetail.Tables[0].Rows[0]["ContainerCount"].ToString();
            rblInvoiceMode.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceMode"].ToString();
            rblInvoiceMode.SelectedItem.Attributes.Add("style", "color:red; font-size:14px;");

            if (dsDetail.Tables[0].Rows[0]["ExpenseTypeId"] != DBNull.Value)
            {
                ExpenseTypeId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["ExpenseTypeId"]);

                lblExpenseType.Text = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
            {
                lblDutyAmount.Text = dsDetail.Tables[0].Rows[0]["DutyAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["IsInterest"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsInterest"]) == true)
                {
                    tdInterestAmt.Visible = true;
                    lblInterestAmount.Text = dsDetail.Tables[0].Rows[0]["InterestAmount"].ToString();
                }
            }
            if (dsDetail.Tables[0].Rows[0]["AssessableValue"] != DBNull.Value)
            {
                lblAssessableValue.Text = dsDetail.Tables[0].Rows[0]["AssessableValue"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["IGSTAmt"] != DBNull.Value)
            {
                lblIGSTAmount.Text = dsDetail.Tables[0].Rows[0]["IGSTAmt"].ToString();
            }

            lblVendorGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            lblPAN.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();
            lblCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();

            lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            lblInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            lblTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();

            lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();


            lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();

            if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            {
                lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["TaxAmount"] != DBNull.Value)
            {
                lblTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["CurrencyName"] != DBNull.Value)
            {
                lblInvoiceCurrency.Text = dsDetail.Tables[0].Rows[0]["CurrencyName"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"] != DBNull.Value)
            {
                lblExchangeRate.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyExchangeRate"].ToString();
            }
            if (dsDetail.Tables[0].Rows[0]["InvoiceCurrencyAmt"] != DBNull.Value)
            {
                lblInvoiceCurrencyAmt.Text = dsDetail.Tables[0].Rows[0]["InvoiceCurrencyAmt"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
            {
                lblGSTValue.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if(dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            { 
                lblPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                lblPatymentRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

            // Invoice Charge Detail

            //if(dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString() == "1")
            //{
            //    fldInvoiceitem.Visible = true;
            //}
            //else
            //{
            //    fldInvoiceitem.Visible = false;
            //}

            // Vendor Detail
            if (dsDetail.Tables[0].Rows[0]["PaymentTerms"] != null)
            {
                lblCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();
            }

            string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            DataSet dsVendor = AccountExpense.GetFAVendorByCode(strVendorCode);

            if (dsVendor.Tables[0].Rows.Count > 0)
            {
                if(dsVendor.Tables[0].Rows[0]["OutStandingAmount"] != null)
                {
                    lblCurrentOutstanding.Text = dsVendor.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                }
            }

            // Billing Party Detail

            if (dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"] != null)
            {
                DataSet dsBillingParty = null;

                string strBillingGSTIN = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();

                if (strBillingGSTIN != "")
                {
                    dsBillingParty = AccountExpense.GetFAVendorByGSTIN(strBillingGSTIN);
                }
                else if(dsDetail.Tables[0].Rows[0]["ConsigneeCode"] != null)
                {
                    string strBillingCode = dsDetail.Tables[0].Rows[0]["ConsigneeCode"].ToString();

                    if(strBillingCode != "")
                    {
                        dsBillingParty = AccountExpense.GetFAVendorByCode(strBillingGSTIN);
                    }
                }

                lblBillingGSTN.Text = strBillingGSTIN;
                lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();

                if (dsBillingParty != null)
                {
                    if (dsBillingParty.Tables[0].Rows.Count > 0)
                    {
                        if (dsBillingParty.Tables[0].Rows[0]["girno"] != null)
                        {
                            lblBillingPAN.Text = dsBillingParty.Tables[0].Rows[0]["girno"].ToString();
                        }
                        if (dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"] != null)
                        {
                            lblCurrentOutstanding.Text = dsBillingParty.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                        }

                    }
                }
            }

        }

        ShowHideFormDetails(StatusId, ExpenseTypeId);
    }

    private void ShowHideFormDetails(int StatusId, int ExpenseTypeId)
    {
        //if (ExpenseTypeId == 1 || ExpenseTypeId == 9)
        //{
        //    fldVendor.Visible = false;
        //}
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 115, strRemark, LoggedInUser.glUserId);

        if(result == 0)
        {
            lblError.Text = "Invoice Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Approved!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Approved!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already Approved!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else 
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnHold_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 111, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice On Hold!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice On Hold!";

            SendHoldRejectMail(InvoiceID, "Management On Hold", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Updated!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already On Hold!";

            Response.Redirect("AccountSuccess.aspx");
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        string strRemark = txtRemark.Text.Trim();

        int result = AccountExpense.AddInvoiceStatus(InvoiceID, 112, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Invoice Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Rejected!";

            SendHoldRejectMail(InvoiceID, "Management Reject", strRemark);

            Response.Redirect("AccountSuccess.aspx");
        }
        else if (result == 2)
        {
            lblError.Text = "Invoice Already Rejected!";
            lblError.CssClass = "success";

            Session["Success"] = "Invoice Already Rejected!";

            Response.Redirect("AccountSuccess.aspx");

        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingInvoiceApproval.aspx");
    }

    private bool CheckInvoiceStatusChange()
    {
        bool isStatusChange = true;
        int PrevStatusId = Convert.ToInt32(hdnStatusId.Value);

        if (Session["InvoiceId"] != null)
        {
            int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);

            int result = AccountExpense.CheckInvoiceStatusChange(InvoiceID, PrevStatusId);

            if (result == 0)
            {
                // Status Not changed

                isStatusChange = false;
            }
            else
            {
                // Status changed

                isStatusChange = true;
            }
        }

        return isStatusChange;
    }

    #region Hold Reject Email

    private bool SendHoldRejectMail(int InvoiceID, string strStatusName, string strRemark)
    {
        string MessageBody = "", strCCEmail = "", strSubject = "";

        bool bEmailSucces = false;
        StringBuilder strbuilder = new StringBuilder();

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        string strJobNumber = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
        string strCustomer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
        string strConsignee = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();

        string strExpenseType = dsDetail.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

        string strVendorName = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();

        string strInvoiceNo = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

        string strTotalInvoiceValue = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();

        string strCreatedByEmail = "amit.bakshi@BabajiShivram.com";

        if (dsDetail.Tables[0].Rows[0]["CreatedByEmail"] != DBNull.Value)
        {
            strCreatedByEmail = dsDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
        }

        string EmailContent = "";

        try
        {

            try
            {
                string strFileName = System.Web.HttpContext.Current.Server.MapPath("~") + "//EmailTemplate/EmailVendorPaymentReject.txt";

                //  string strFileName = "../EmailTemplate/EmailVendorPaymentReject.txt";

                EmailContent = File.ReadAllText(strFileName);

                //StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                //sr = File.OpenText(Server.MapPath(strFileName));
                //EmailContent = sr.ReadToEnd();
                //sr.Close();
                //sr.Dispose();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                return bEmailSucces;
            }

            MessageBody = EmailContent.Replace("@JobRefNo", strJobNumber);

            MessageBody = MessageBody.Replace("@Customer", strCustomer);
            MessageBody = MessageBody.Replace("@Consignee", strConsignee);
            MessageBody = MessageBody.Replace("@ExpenseType", strExpenseType);
            MessageBody = MessageBody.Replace("@VendorName", strVendorName);
            MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
            MessageBody = MessageBody.Replace("@TotalInvoiceAmount", strTotalInvoiceValue);
            MessageBody = MessageBody.Replace("@StatusName", strStatusName);
            MessageBody = MessageBody.Replace("@Remarks", strRemark);
            MessageBody = MessageBody.Replace("@UserName", LoggedInUser.glEmpName);

            ///////////////////////////////////////////////////////////////////////////////

            try
            {
                strSubject = "Vendor Payment " + strStatusName + "/" + strJobNumber + "/ " + strInvoiceNo + "/ " + strConsignee + " / " + strExpenseType;

                string EmailBody = MessageBody;

                bEmailSucces = EMail.SendMail(LoggedInUser.glUserName, strCreatedByEmail, strSubject, EmailBody, "");
            }
            catch (System.Exception exn)
            {
                bEmailSucces = false;
                return bEmailSucces;
            }
        }
        catch (Exception enk)
        {
            return false;
        }

        return bEmailSucces;
    }


    #endregion

    #region Document Download

    protected void gvDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                ViewDocument(strDocpath);
            }
        }
    }
    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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

    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
}