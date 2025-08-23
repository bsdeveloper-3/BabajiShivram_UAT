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
using System.Text;
using System.Collections.Generic;
using Ionic.Zip;

public partial class AccountExpense_ExpPaymentDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        //ScriptManager1.RegisterPostBackControl(btnSubmitDuty);
        //ScriptManager1.RegisterPostBackControl(FormView1);
        ScriptManager1.RegisterPostBackControl(FormView1);
        ScriptManager1.RegisterPostBackControl(lnkPenaltyCopy);

        string strQuery = "";
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job Expense Payment Details";
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["PaymentId"]) != null)
            {
                GetPaymentDetails();
            }
            else
            {
                Response.Redirect("JobExpPayment.aspx");
            }          
        }

        if (Request.QueryString.ToString() != "")
        {
            strQuery = Request.QueryString["id"].ToString();
        }

        if (strQuery != "")
        {
            fsRTGS.Visible = false;
            fsCheque_DD.Visible = false;
            fsCash.Visible = false;

            btnSubmit.Visible = false;
            btnCancel.Visible = false;

            TextBox txtIntAmount = (TextBox)fsDutyPayment.FindControl("txtIntAmount");
            TextBox txtPenaltyAmount = (TextBox)fsDutyPayment.FindControl("txtPenaltyAmount");

            txtIntAmount.Enabled = false;
            txtPenaltyAmount.Enabled = false;
            // btn=(ButtonField) 
            // btnsubmit.Visible = false;
        }     
    }

    protected void GetPaymentDetails()
    {
        DataSet dsGetPaymentDetail = new DataSet();
        dsGetPaymentDetail = AccountExpense.GetPaymentRequestById(Convert.ToInt32(Session["PaymentId"]));
        if (dsGetPaymentDetail != null)
        {
            if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"] != null)
                hdnPaymentTypeId.Value = dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

            if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "4")        //RTGS
            {
                fsRTGS.Visible = true;
                fsCheque_DD.Visible = false;
                fsCash.Visible = false;
            }
            else if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "1")   //Cash
            {
                fsCash.Visible = true;
                fsCheque_DD.Visible = false;
                fsRTGS.Visible = false;
            }
            else    // Cheque or DD
            {
                fsCheque_DD.Visible = true;
                fsCash.Visible = false;
                fsRTGS.Visible = false;
            }

            // Show Duty or Stamp Duty section based on Expense Type selected
            if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "1") // duty payment
            {
                //btnSavePayment.Visible = false;
                //btnClearPayment.Visible = false;
                fsDutyPayment.Visible = true;
                fsStampDuty.Visible = false;
                fsPenaltyPayment.Visible = false;

                hdnRequestTypeId.Value = "1";

                // get duty payment details
                lblPartyName.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
                lblIECNo.Text = dsGetPaymentDetail.Tables[0].Rows[0]["IECNo"].ToString();
                lblBoeNo.Text = dsGetPaymentDetail.Tables[0].Rows[0]["BOENo"].ToString();
                //string PenaltyCopyPath = "";

                //PenaltyCopyPath= dsGetPaymentDetail.Tables[0].Rows[0]["BOENo"].ToString();
                hdnPenaltyCopyPath.Value = dsGetPaymentDetail.Tables[0].Rows[0]["PenaltyCopyPath"].ToString();

                if (dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
                {
                    if (Convert.ToString(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]) != "" &&
                        Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy") != "01/01/1900")
                    {
                        lblBoeDate.Text = Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy");
                    }
                }
                else
                    lblBoeDate.Text = "";
                lblLocCode.Text = dsGetPaymentDetail.Tables[0].Rows[0]["LocCode"].ToString();
                lblLocationCode.Text = dsGetPaymentDetail.Tables[0].Rows[0]["LocationCode"].ToString();
                if (dsGetPaymentDetail.Tables[0].Rows[0]["ACPNonACP"] != DBNull.Value && dsGetPaymentDetail.Tables[0].Rows[0]["ACPNonACP"].ToString() != "0")
                {
                    if (dsGetPaymentDetail.Tables[0].Rows[0]["ACPNonACP"].ToString() == "1")
                        ddlACPNonACP.SelectedValue = "1";
                    else
                        ddlACPNonACP.SelectedValue = "2";
                }

                txtChallenNo.Text = dsGetPaymentDetail.Tables[0].Rows[0]["TR6ChallenNo"].ToString();
                lblDutyAmount.Text = dsGetPaymentDetail.Tables[0].Rows[0]["DutyAmount"].ToString();
                txtIntAmount.Text = dsGetPaymentDetail.Tables[0].Rows[0]["InterestAmount"].ToString();
                txtPenaltyAmount.Text = dsGetPaymentDetail.Tables[0].Rows[0]["PenaltyAmount"].ToString();
                lblTotal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["Total"].ToString();
                lblRecdMailFrom_Name.Text = dsGetPaymentDetail.Tables[0].Rows[0]["RecdMailFrom"].ToString();
                if (dsGetPaymentDetail.Tables[0].Rows[0]["RecdMailFromMailId"].ToString() != "")
                {
                    lblRecdMailFrom_Mail.Text = "(" + dsGetPaymentDetail.Tables[0].Rows[0]["RecdMailFromMailId"].ToString() + ")";
                }
                lblApprovedBy.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ApprovedBy"].ToString();
                if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentType"] != DBNull.Value && dsGetPaymentDetail.Tables[0].Rows[0]["PaymentType"].ToString() != "0")
                {
                    ddlType.SelectedValue = dsGetPaymentDetail.Tables[0].Rows[0]["PaymentType"].ToString();
                }
                txtAdvanceDetails.Text = dsGetPaymentDetail.Tables[0].Rows[0]["AdvanceDetails"].ToString();
                lblStatus.Text = dsGetPaymentDetail.Tables[0].Rows[0]["CurrentStatus"].ToString();
                txtPenaltyMail.Text = dsGetPaymentDetail.Tables[0].Rows[0]["PenaltyAppMail"].ToString();
            }
            else if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "5") // stamp duty payment
            {
                //btnSavePayment.Visible = false;
                //btnClearPayment.Visible = false;
                fsPenaltyPayment.Visible = false;
                fsDutyPayment.Visible = false;
                fsStampDuty.Visible = true;
                hdnRequestTypeId.Value = "5";

                // get stamp duty payment details

                //lblClientState.Text = dsGetPaymentDetail.Tables[0].Rows[0]["PartyName"].ToString();
                lblPartyName_StampDuty.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
                lblClientAddress.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ClientAdd"].ToString();
                lblBoeNo_StampDuty.Text = dsGetPaymentDetail.Tables[0].Rows[0]["BOENo"].ToString();
                if (dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
                {
                    if (Convert.ToString(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]) != "" &&
                        Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy") != "01/01/1900")
                    {
                        lblBOEDate_StampDuty.Text = Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy");
                    }
                }
                else
                    lblBOEDate_StampDuty.Text = "";
                lblAssessableValue.Text = dsGetPaymentDetail.Tables[0].Rows[0]["AssessableValue"].ToString();
                lbl_BLNo.Text = dsGetPaymentDetail.Tables[0].Rows[0]["BLNo"].ToString();
                if (dsGetPaymentDetail.Tables[0].Rows[0]["BLDate"] != DBNull.Value)
                {
                    if (Convert.ToString(dsGetPaymentDetail.Tables[0].Rows[0]["BLDate"]) != "" &&
                        Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BLDate"]).ToString("dd/MM/yyyy") != "01/01/1900")
                    {
                        lbl_BLDate.Text = Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BLDate"]).ToString("dd/MM/yyyy");
                    }
                }
                else
                lbl_BLDate.Text = "";
                lblIGM.Text = dsGetPaymentDetail.Tables[0].Rows[0]["IGMNo"].ToString();
                txtCustomDuty.Text = dsGetPaymentDetail.Tables[0].Rows[0]["CustomDuty"].ToString();
                lblAssCustomTotal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["AssCustomTotal"].ToString();
                txtStampDuty.Text = dsGetPaymentDetail.Tables[0].Rows[0]["StampDuty"].ToString();
                txtGSTNo.Text = dsGetPaymentDetail.Tables[0].Rows[0]["GSTNo"].ToString();
            }
            else if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "8") // Penalty
            {
                //btnSavePayment.Visible = false;
                //btnClearPayment.Visible = false;
                fsPenaltyPayment.Visible = true;
                fsDutyPayment.Visible = false;
                fsStampDuty.Visible = false;
                hdnRequestTypeId.Value = "8";

                // get duty payment details
                lblPartyPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
                lblIECPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["IECNo"].ToString();
                lblBOEPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["BOENo"].ToString();
                //string PenaltyCopyPath = "";

                //PenaltyCopyPath= dsGetPaymentDetail.Tables[0].Rows[0]["BOENo"].ToString();
                //hdnPenaltyCopyPath.Value = dsGetPaymentDetail.Tables[0].Rows[0]["PenaltyCopyPath"].ToString();

                if (dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
                {
                    if (Convert.ToString(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]) != "" &&
                        Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy") != "01/01/1900")
                    {
                        lblBOEDtPenal.Text = Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy");
                    }
                }
                else
                lblBoeDate.Text = "";
                lblLocationNmPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["LocCode"].ToString();
                lblLocationPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["LocationCode"].ToString();
                
                lblApprovebyPenal.Text = dsGetPaymentDetail.Tables[0].Rows[0]["ApprovedBy"].ToString();              
            }

            else
            {
                //btnSavePayment.Visible = true;
                //btnClearPayment.Visible = true;
                fsPenaltyPayment.Visible = false;
                fsDutyPayment.Visible = false;
                fsStampDuty.Visible = false;
            }

            /*****************Transporter Advance Backlog Code**********************/
            if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "11") // Advance Payment
            {
                fsPenaltyPayment.Visible = false;
                fsDutyPayment.Visible = false;
                fsStampDuty.Visible = false;

                if (dsGetPaymentDetail.Tables[0].Rows[0]["CreatedDate"] != DBNull.Value)
                {
                    DateTime dtPaymentDate = Convert.ToDateTime(dsGetPaymentDetail.Tables[0].Rows[0]["CreatedDate"]);
                    DateTime dtBackupUpdateDate = Convert.ToDateTime("2019/12/31");

                    if (dtPaymentDate.CompareTo(dtBackupUpdateDate) < 0)
                    {
                        rfvPOD.Enabled = false;
                    }
                }

            }
            /*****************Transporter Advance Backlog Code**********************/
        }
    }

    protected void lnkPenaltyCopy_OnClick(object sender, EventArgs e)
    {
        HiddenField hdnPenaltyCopyPath = (HiddenField)fsDutyPayment.FindControl("hdnPenaltyCopyPath");
        if (hdnPenaltyCopyPath.Value != "")
        {            
            DownloadDocument(hdnPenaltyCopyPath.Value);
        }
    }  

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int PaymentId = 0, JobId = 0, VendorId = 0, CurrencyId = 0, PaymentTypeId = 0, ACPNonACP = 0, PaymentDetailType = 0;
        string VoucherNo = "", Rate = "", ChequeNo = "", BankName = "", RefNo = "", PODDocPath = "", Narration = "", PenaltyCopyPath = "",
            TR6ChallenNo = "", PenaltyApprovalMail = "", GSTNo = "";
        DateTime ChequeDate = DateTime.MinValue, PaymentDate = DateTime.MinValue;
        double dcInterestAmount = 0, dcPenaltyAmount = 0, dcDutyAmount = 0, dcTotal = 0, dcCustomDuty = 0, dcStampDuty = 0, dcAssessableValue = 0;

        PaymentId = Convert.ToInt32(Session["PaymentId"]);
        JobId = Convert.ToInt32(Session["JobId"]);
        

        // Additional Details
        if (hdnRequestTypeId.Value.Trim() != "" && hdnRequestTypeId.Value.Trim() == "1")        // Duty Payment
        {
            ACPNonACP = Convert.ToInt32(ddlACPNonACP.SelectedValue);
            TR6ChallenNo = txtChallenNo.Text.Trim();
            if (lblDutyAmount.Text.Trim() != "")
                dcDutyAmount = Convert.ToDouble(lblDutyAmount.Text.Trim());
            if (txtIntAmount.Text.Trim() != "")
                dcInterestAmount = Convert.ToDouble(txtIntAmount.Text.Trim());
            if (txtPenaltyAmount.Text.Trim() != "")
                dcPenaltyAmount = Convert.ToDouble(txtPenaltyAmount.Text.Trim());
            dcTotal = Convert.ToDouble(dcDutyAmount + dcInterestAmount + dcPenaltyAmount);
            PaymentDetailType = Convert.ToInt32(ddlType.SelectedValue);
            PenaltyApprovalMail = txtPenaltyMail.Text.Trim();

            // Upload penalty document if any
            //if (fuPenaltyCopy != null && fuPenaltyCopy.HasFile)
            //    PenaltyCopyPath = UploadFiles(fuPenaltyCopy);
        }
        else if (hdnRequestTypeId.Value.Trim() != "" && hdnRequestTypeId.Value.Trim() == "5")   // Stamp duty payment
        {
            if (txtCustomDuty.Text.Trim() != "")
                dcCustomDuty = Convert.ToDouble(txtCustomDuty.Text.Trim());
            if (txtStampDuty.Text.Trim() != "")
                dcStampDuty = Convert.ToDouble(txtStampDuty.Text.Trim());
            if (lblAssessableValue.Text.Trim() != "")
                dcAssessableValue = Convert.ToDouble(lblAssessableValue.Text.Trim());
            GSTNo = txtGSTNo.Text.Trim();
        }

        if (hdnPaymentTypeId.Value != "0")
            PaymentTypeId = Convert.ToInt32(hdnPaymentTypeId.Value);

        if (PaymentTypeId != 0)
        {
            if (PaymentTypeId == 4)         // RTGS
            {
                if (txtRefNo.Text.Trim() != "" && fuUploadPOD.HasFile)
                {
                    RefNo = txtRefNo.Text.Trim();
                    if (txtPaymentDt_RTGS.Text.Trim() != "")
                        PaymentDate = Commonfunctions.CDateTime(txtPaymentDt_RTGS.Text.Trim());
                    BankName = txtBankName_RTGS.Text.Trim();

                    // Upload POD Copy
                    if (fuUploadPOD != null && fuUploadPOD.HasFile)
                        PODDocPath = UploadFiles(fuUploadPOD);
                }
            }
            else if (PaymentTypeId == 1)   //Cash
            {
                if (txtPaymentDate.Text.Trim() != "")
                    PaymentDate = Commonfunctions.CDateTime(txtPaymentDate.Text.Trim());

                RefNo = txtRefNo_Cash.Text.Trim();
                // Upload document
                if (fuDoc_Cash.HasFile)
                {
                    UploadTypeWiseDocs(fuDoc_Cash, PaymentId);
                }
            }
            else                        // Cheque or DD
            {
                if (txtChequeDate.Text.Trim() != "")
                    ChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());

                ChequeNo = txtChequeNo.Text.Trim();
                BankName = txtBankName.Text.Trim();
                Narration = txtNarration.Text.Trim();

                // Upload document
                if (fuUploadDoc_ChequeDD.HasFile)
                {
                    UploadTypeWiseDocs(fuUploadDoc_ChequeDD, PaymentId);
                }
            }

            int result = AccountExpense.AddPaymentDetails(PaymentId, "", 0, 0, "", JobId, ChequeNo, ChequeDate, BankName, Narration, 0, RefNo,
                        PODDocPath, PaymentDate, ACPNonACP, TR6ChallenNo, PaymentDetailType, PenaltyApprovalMail, dcAssessableValue, dcCustomDuty, dcStampDuty,
                        GSTNo, dcDutyAmount, dcInterestAmount, dcPenaltyAmount, PenaltyCopyPath, LoggedInUser.glUserId);
            if (result == 0)
            {
                int ApprovedJobExp = AccountExpense.AddPaymentStatus(PaymentId, 4, "", true, LoggedInUser.glUserId);

                SendFundDetailMail(PaymentId);

                lblError.Text = "Successfully added payment details.";
                lblError.CssClass = "success";
                ResetControls();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Record already exists for this request.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("JobExpPayment.aspx");
    }

    protected void ResetControls()
    {
        txtBankName.Text = "";
        txtChequeDate.Text = "";
        txtChequeNo.Text = "";
        txtNarration.Text = "";
        txtPaymentDate.Text = "";
        txtRefNo.Text = "";
        txtRefNo_Cash.Text = "";
        hdnPaymentTypeId.Value = "0";
        fsCash.Visible = false;
        fsCheque_DD.Visible = false;
        fsRTGS.Visible = false;

        fsPenaltyPayment.Visible = false;
        fsDutyPayment.Visible = false;
        fsStampDuty.Visible = false;
    }

    private bool SendFundDetailMail(int PaymentId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", JobRefno = "", BranchName = "", PaymentTypeName = "",
            ExpenseTypeName = "", Amount = "", PaidTo = "", Remark = "", CreatedBy = "", CreatedByEmail = "", HoldedBy = "", HoldedByEmail = "", HoldReason = "",
            RefNo = "", BankName = "", Narration = "", Favouring = "", ChequeNo = "", PODDocPath = "", UpdatedBy = "", UpdatedByEmail = "";
        bool bEmailSucces = false;
        int PaymentTypeId = 0, RequestTypeId = 0;
        DateTime PaymentDate = DateTime.MinValue, ChequeDate = DateTime.MinValue;
        StringBuilder strbuilder = new StringBuilder();

        DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
        if (dsGetPaymentDetails != null)
        {
            JobRefno = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();
            BranchName = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();
            PaymentTypeId = Convert.ToInt32(dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeId"].ToString());
            RequestTypeId = Convert.ToInt32(dsGetPaymentDetails.Tables[0].Rows[0]["RequestTypeId"].ToString());
            PaymentTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();
            ExpenseTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
            Amount = dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();
            PaidTo = dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();
            Remark = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();
            CreatedBy = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            CreatedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedByEmail"].ToString();
            UpdatedBy = dsGetPaymentDetails.Tables[0].Rows[0]["UpdatedBy"].ToString();
            UpdatedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["UpdatedByEmail"].ToString();

            strCustomerEmail = CreatedByEmail; 
            strCCEmail = "neeraj.mandowara@babajishivram.com, bsavita@babajishivram.com"; 
            strSubject = "Fund request details of amount " + Amount + " for Babaji Job " + JobRefno + " and expense type " + ExpenseTypeName + ".";
            if (strCustomerEmail == "" || strSubject == "")
                return bEmailSucces;
            else
            {
                MessageBody = "Dear Sir,<BR><BR> Please find the below fund request details of amount " + Amount + " for Babaji Job "
                                                                + JobRefno + " and expense type " + ExpenseTypeName + ".<BR><BR>";

                strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:50%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Babaji Job No</th><td style='padding-left: 3px'>" + JobRefno + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Branch</th><td style='padding-left: 3px'>" + BranchName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Payment Type</th><td style='padding-left: 3px'>" + PaymentTypeName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Request Type</th><td style='padding-left: 3px'>" + ExpenseTypeName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Amount</th><td style='padding-left: 3px'>" + Amount + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Paid To</th><td style='padding-left: 3px'>" + PaidTo + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Remark</th><td style='padding-left: 3px'>" + Remark + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Created By</th><td style='padding-left: 3px'>" + CreatedBy + "</td></tr>");
                strbuilder = strbuilder.Append("</table>");

                DataSet dsGetFundDetails = AccountExpense.GetPaymentRequestById(PaymentId);
                if (dsGetFundDetails != null)
                {
                    RefNo = dsGetFundDetails.Tables[0].Rows[0]["RefNo"].ToString();
                    PODDocPath = dsGetFundDetails.Tables[0].Rows[0]["PODDocPath"].ToString();
                    PaymentDate = Convert.ToDateTime(dsGetFundDetails.Tables[0].Rows[0]["PaymentDate"].ToString());
                    Narration = dsGetFundDetails.Tables[0].Rows[0]["Narration"].ToString();
                    BankName = dsGetFundDetails.Tables[0].Rows[0]["BankName"].ToString();
                    Favouring = dsGetFundDetails.Tables[0].Rows[0]["Favouring"].ToString();
                    ChequeNo = dsGetFundDetails.Tables[0].Rows[0]["ChequeNo"].ToString();
                    ChequeDate = Convert.ToDateTime(dsGetFundDetails.Tables[0].Rows[0]["ChequeDate"].ToString());
                }

                if (PaymentTypeId == 4)             //RTGS
                {
                    strbuilder = strbuilder.Append("<BR><BR><table style='text-align:left;margin-left-bottom:40px;width:40%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Ref No</th><td style='padding-left: 3px'>" + RefNo + "</td></tr>");
                    strbuilder = strbuilder.Append("</table>");
                }
                else if (PaymentTypeId == 1)       //Cash
                {
                    strbuilder = strbuilder.Append("<BR><BR><table style='text-align:left;margin-left-bottom:40px;width:40%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Payment Date</th><td style='padding-left: 3px'>" + PaymentDate.ToShortDateString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Ref No</th><td style='padding-left: 3px'>" + RefNo + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Favouring</th><td style='padding-left: 3px'>" + Favouring + "</td></tr>");
                    strbuilder = strbuilder.Append("</table>");
                }
                else                               // Cheque or DD
                {
                    strbuilder = strbuilder.Append("<BR><BR><table style='text-align:left;margin-left-bottom:40px;width:40%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Cheque No</th><td style='padding-left: 3px'>" + ChequeNo + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Cheque Date</th><td style='padding-left: 3px'>" + ChequeDate.ToShortDateString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Bank Name</th><td style='padding-left: 3px'>" + BankName + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Favouring</th><td style='padding-left: 3px'>" + Favouring + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Narration</th><td style='padding-left: 3px'>" + Narration + "</td></tr>");
                    strbuilder = strbuilder.Append("</table>");
                }

                #region STAMP DUTY DETAILS

                if (RequestTypeId == 1) // duty payment
                {
                    strbuilder = strbuilder.Append("<BR><BR><table style='text-align:left;margin-left-bottom:40px;width:40%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>PartyName</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["PartyName"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>IEC No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["IECNo"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["BOENo"].ToString() + "</td></tr>");
                    if (dsGetFundDetails.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE Date</th><td style='padding-left: 3px'>" + "" + "</td></tr>");
                    }
                    else
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE Date</th><td style='padding-left: 3px'>" + Convert.ToDateTime(dsGetFundDetails.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy") + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Location Code</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["LocationCode"].ToString() + "</td></tr>");
                    if (dsGetFundDetails.Tables[0].Rows[0]["ACPNonACP"] != DBNull.Value && dsGetFundDetails.Tables[0].Rows[0]["ACPNonACP"].ToString() != "0")
                    {
                        if (dsGetFundDetails.Tables[0].Rows[0]["ACPNonACP"].ToString() == "1")
                        {
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>ACP/NON ACP</th><td style='padding-left: 3px'>" + "ACP" + "</td></tr>");
                        }
                        else
                        {
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>ACP/NON ACP</th><td style='padding-left: 3px'>" + "NON ACP" + "</td></tr>");
                        }
                    }
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>TR6 Challen No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["TR6ChallenNo"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Duty Amount</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["DutyAmount"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Interest Amount</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["InterestAmount"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Penalty Amount</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["PenaltyAmount"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Total</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["Total"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Received Mail From</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["RecdMailFrom"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Approved By</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["ApprovedBy"].ToString() + "</td></tr>");
                    if (dsGetFundDetails.Tables[0].Rows[0]["PaymentType"] != DBNull.Value && dsGetFundDetails.Tables[0].Rows[0]["PaymentType"].ToString() != "0")
                    {
                        if (dsGetFundDetails.Tables[0].Rows[0]["PaymentType"].ToString() == "1")
                        {
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>RD/Duty/Penalty</th><td style='padding-left: 3px'>" + "RD" + "</td></tr>");
                        }
                        else if (dsGetFundDetails.Tables[0].Rows[0]["PaymentType"].ToString() == "2")
                        {
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>RD/Duty/Penalty</th><td style='padding-left: 3px'>" + "Duty" + "</td></tr>");
                        }
                        else
                        {
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>RD/Duty/Penalty</th><td style='padding-left: 3px'>" + "Penalty" + "</td></tr>");
                        }
                    }
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Advance Details (if any)</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["AdvanceDetails"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>PenaltyAppMail</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["PenaltyAppMail"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("</table>");

                }
                else if (RequestTypeId == 5) // stamp duty payment
                {
                    strbuilder = strbuilder.Append("<BR><BR><table style='text-align:left;margin-left-bottom:40px;width:40%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>PartyName</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["PartyName"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Party Address</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["ClientAdd"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["BOENo"].ToString() + "</td></tr>");
                    if (dsGetFundDetails.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE Date</th><td style='padding-left: 3px'>" + "" + "</td></tr>");
                    }
                    else
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BOE Date</th><td style='padding-left: 3px'>" + Convert.ToDateTime(dsGetFundDetails.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy") + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Assessable Value</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["AssessableValue"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BL/No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["BLNo"].ToString() + "</td></tr>");
                    if (dsGetFundDetails.Tables[0].Rows[0]["BLDate"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BL Date</th><td style='padding-left: 3px'>" + "" + "</td></tr>");
                    }
                    else
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>BL Date</th><td style='padding-left: 3px'>" + Convert.ToDateTime(dsGetFundDetails.Tables[0].Rows[0]["BLDate"]).ToString("dd/MM/yyyy") + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>IGM No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["IGMNo"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Custom Duty</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["CustomDuty"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Assessable + Custom Duty</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["AssCustomTotal"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Stamp Duty</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["StampDuty"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>GST No</th><td style='padding-left: 3px'>" + dsGetFundDetails.Tables[0].Rows[0]["GSTNo"].ToString() + "</td></tr>");
                    strbuilder = strbuilder.Append("</table>");

                }
                #endregion

                MessageBody = MessageBody + strbuilder;
                MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + HoldedBy;

                System.Collections.Generic.List<string> lstFilePath = new List<string>();
                DataSet dsGetExpenseDocDetails = AccountExpense.GetExpenseDocDetails(PaymentId);
                if (dsGetExpenseDocDetails != null && dsGetExpenseDocDetails.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGetExpenseDocDetails.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetExpenseDocDetails.Tables[0].Rows[i]["DocPath"] != DBNull.Value)
                        {
                            lstFilePath.Add("ExpenseUpload\\" + dsGetExpenseDocDetails.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                    }
                }

                if (PODDocPath != "")
                {
                    lstFilePath.Add("ExpenseUpload\\" + PODDocPath);
                }

                bEmailSucces = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                return bEmailSucces;
            }
        }
        else
            return false;
    }

    protected void CalculateAmtTotal(object sender, EventArgs e)
    {
        double dcInterestAmount = 0, dcPenaltyAmount = 0, dcDutyAmount = 0, dcTotal = 0, dcCustomDuty = 0,
                dcStampDuty = 0, dcAssessableValue = 0, dcAssCustomTotal = 0;

        

        // amount total
        if (lblDutyAmount.Text.Trim() != "")
            dcDutyAmount = Convert.ToDouble(lblDutyAmount.Text.Trim());
        if (txtIntAmount.Text.Trim() != "")
            dcInterestAmount = Convert.ToDouble(txtIntAmount.Text.Trim());
        if (txtPenaltyAmount.Text.Trim() != "")
            dcPenaltyAmount = Convert.ToDouble(txtPenaltyAmount.Text.Trim());
        dcTotal = Convert.ToDouble(dcDutyAmount + dcInterestAmount + dcPenaltyAmount);
        if (dcTotal > 0)
            lblTotal.Text = dcTotal.ToString();
        else
            lblTotal.Text = "0";

        // assessable value + custom duty
        if (txtCustomDuty.Text.Trim() != "")
            dcCustomDuty = Convert.ToDouble(txtCustomDuty.Text.Trim());
        if (txtStampDuty.Text.Trim() != "")
            dcStampDuty = Convert.ToDouble(txtStampDuty.Text.Trim());
        if (lblAssessableValue.Text.Trim() != "")
            dcAssessableValue = Convert.ToDouble(lblAssessableValue.Text.Trim());
        dcAssCustomTotal = Convert.ToDouble(dcAssessableValue + dcCustomDuty);
        if (dcAssCustomTotal > 0)
            lblAssCustomTotal.Text = dcAssCustomTotal.ToString();
        else
            lblAssCustomTotal.Text = "0";
    }

    #region FORM VIEW EVENTS

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "documents")
        {
            string lid = (string)e.CommandArgument;
            int PaymentId = Convert.ToInt32(lid);
            if (PaymentId != 0)
            {
                DataSet dsGetDocs = AccountExpense.GetExpenseDocDetails(Convert.ToInt32(PaymentId));
                if (dsGetDocs != null)
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectoryByName("Files");
                        if (dsGetDocs.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGetDocs.Tables[0].Rows.Count; i++)
                            {
                                if (dsGetDocs.Tables[0].Rows[i]["DocPath"] != DBNull.Value)
                                {
                                    string filePath = dsGetDocs.Tables[0].Rows[i]["DocPath"].ToString();
                                    string ServerPath = FileServer.GetFileServerDir();

                                    if (ServerPath == "")
                                    {
                                        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + filePath);
                                    }
                                    else
                                    {
                                        ServerPath = ServerPath + filePath;
                                    }

                                    zip.AddFile(ServerPath, "Files");
                                }
                            }
                        }
                        Response.Clear();
                        Response.BufferOutput = false;
                        string zipName = String.Format("Zip_{0}.zip", "AllDocuments_" + DateTime.Now.ToShortDateString());
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        Response.End();
                    }
                }
            }
        }
    }

    #endregion

    #region UPLOAD DOCUMENTS

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "Expense_" + Session["PaymentId"].ToString() + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + FilePath);
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
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }
        return builder.ToString();
    }

    #endregion

    #region DOWNLOAD DOCUMENTS

    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
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

    protected void UploadTypeWiseDocs(FileUpload fuDocument, int PaymentId)
    {
        string DocPath = UploadFiles(fuDocument);
        string FileName = fuDocument.FileName;
        if (PaymentId != 0 && DocPath != "")
        {
            int result = AccountExpense.AddExpenseDocDetails(PaymentId, DocPath, FileName, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added document.";
                lblError.CssClass = "success";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Document already exists for this request.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    //protected void btnUpload_Click(object sender, EventArgs e)
    //{
    //    if (fuDocument.HasFile)
    //    {
    //        string DocPath = UploadFiles(fuDocument);
    //        string FileName = fuDocument.FileName;
    //        if (DocPath != "")
    //        {
    //            int result = AccountExpense.AddExpenseDocDetails(Convert.ToInt32(Session["PaymentId"]), DocPath, FileName, LoggedInUser.glUserId);
    //            if (result == 0)
    //            {
    //                lblError.Text = "Successfully added document.";
    //                lblError.CssClass = "success";
    //                ResetControls();
    //                GetPaymentDetails();
    //                GridViewDocument.DataBind();
    //            }
    //            else if (result == 1)
    //            {
    //                lblError.Text = "System Error. Please try again later.";
    //                lblError.CssClass = "errorMsg";
    //            }
    //            else
    //            {
    //                lblError.Text = "Document already exists for this request.";
    //                lblError.CssClass = "errorMsg";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        lblError.Text = "Please browse document to upload..!!";
    //        lblError.CssClass = "errorMsg";
    //    }
    //}

    #endregion

    #region GridView Event

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                //Response.Redirect("ExpensePayment.aspx");
                Response.Redirect("ExpPaymentDetails.aspx");
            }
        }
    }

    protected void gvJobExpDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvJobExpDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Priority") != DBNull.Value)
            {
                // Change row color based on job priority

                int prioroty = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority"));
                if (prioroty == (int)JobPriority.High)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "High Job Priority";
                }
                else if (prioroty == (int)JobPriority.Intense)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#85f7f7");
                    e.Row.ToolTip = "Intense Job Priority";
                }
            }
        }
    }

    #endregion
}