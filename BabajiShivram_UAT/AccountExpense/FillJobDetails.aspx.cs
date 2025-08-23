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
using AjaxControlToolkit;
using System.Collections.Generic;

public partial class AccountExpense_FillJobDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("PaymentRequest2.aspx");

        JobDetailExtender.ContextKey = Convert.ToString(LoggedInUser.glUserId);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument2);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Fund Request";

            hdnNewPaymentLid.Value = Convert.ToString(AccountExpense.GetNewPaymentLid()); // Get New Payment PkId

            rblAdvanceRecd_OnSelectedIndexChanged(null, EventArgs.Empty);
            DataTable dtAnnexureDoc2 = new DataTable();
            dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
        }
    }

    protected void rblAdvanceRecd_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAdvanceRecd.SelectedValue == "0") //No
        {
            txtAdvanceAmt.Visible = false;
            rfvAdvanceAmt.Enabled = false;
            cvAdvanceAmt.Enabled = false;
        }
        else //Yes
        {
            txtAdvanceAmt.Visible = true;
            rfvAdvanceAmt.Enabled = true;
            cvAdvanceAmt.Enabled = true;
        }
    }

    public int Validation()
    {
        int Isready = 0;

        int R1 = 0;
        int R2 = 0;

        if (ddlExpenseType.SelectedValue == "1")    // For Duty Amount
        {
            // Validation For Duty Amnt & Challan No.

            if (txtDutyAmount.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Duty Amount";
                lblError.CssClass = "errorMsg";
                R1 = 1;
            }
            if (txtChallanNo.Text.Trim() == "")
            {
                lblError.Text = "Please Enter TR6 Challan No.";
                lblError.CssClass = "errorMsg";
                R1 = 1;
            }
            //else
            //{
            //    R1 = 0;
            //}

            // Validation For Penelty Section

            if (rdlInterest.SelectedValue == "")
            {
                lblError.Text = "Please Select Interest Require or Not";
                lblError.CssClass = "errorMsg";
                R1 = 1;
            }
            else if (rdlInterest.SelectedValue == "1")
            {
                if (txtInterestAmnt.Text == "")
                {
                    lblError.Text = "Please Enter Interest Amount";
                    lblError.CssClass = "errorMsg";
                    R1 = 1;
                }
            }
            //else
            //{
            //    R1 = 0;
            //}

            // Validation For Penalty Section

            if (ddlACPNonACP.SelectedValue == "0") // ACP, Non-ACP
            {
                lblError.Text = "Please Select ACP / Non-ACP";
                lblError.CssClass = "errorMsg";
                R2 = 1;
            }

            if (ddlType.SelectedValue == "0") // ACP, Non-ACP
            {
                lblError.Text = "Please Select RD/Duty/Penalty";
                lblError.CssClass = "errorMsg";
                R2 = 1;
            }



            if (rdlPenalty.SelectedValue == "")
            {
                lblError.Text = "Please Select Penalty Require or Not";
                lblError.CssClass = "errorMsg";
                R2 = 1;
            }
            else if (rdlPenalty.SelectedValue == "1")
            {
                if (fuPenaltyCopy.FileName == "")
                {
                    lblError.Text = "Please Select Penalty Mail Copy";
                    lblError.CssClass = "errorMsg";
                    R2 = 1;
                }

                if (txtPenaltyAmnt.Text == "")
                {
                    lblError.Text = "Please Enter Penalty Amount";
                    lblError.CssClass = "errorMsg";
                    R2 = 1;
                }
            }


            if (R1 == 0 && R2 == 0)
            {
                Isready = 0;
            }
            else
            {
                Isready = 1;
            }
        }
        else if (ddlExpenseType.SelectedValue != "1")       //Except For Duty Amount
        {
            // Validation For Amount

            if (txtAmount.Text.Trim() == "")
            {
                lblError.Text = "Please Enter The Amount";
                lblError.CssClass = "errorMsg";
                Isready = 1;
            }
            //else
            //{
            //    Isready = 0;
            //}
        }

        return Isready;     // IF Isready=0 success else Validation
    }

    public void TotalAmnt()
    {
        double DutyAmnt = 0;
        double interestAmnt = 0;
        double peneltyAmnt = 0;
        double TotalAmnt = 0;
        double Amount = 0;

        if (ddlExpenseType.SelectedValue == "1")
        {
            if (txtDutyAmount.Text.Trim() != "")
            {
                DutyAmnt = Convert.ToDouble(txtDutyAmount.Text.Trim());
            }

            if (txtInterestAmnt.Text.Trim() != "")
            {
                interestAmnt = Convert.ToDouble(txtInterestAmnt.Text.Trim());
            }

            if (txtPenaltyAmnt.Text.Trim() != "")
            {
                peneltyAmnt = Convert.ToDouble(txtPenaltyAmnt.Text.Trim());
            }

            TotalAmnt = DutyAmnt + interestAmnt + peneltyAmnt;
            txtTotalAmnt.Text = Convert.ToString(TotalAmnt);
        }
        else if (ddlExpenseType.SelectedValue != "1")
        {
            if (txtDutyAmount.Text.Trim() != "")
            {
                DutyAmnt = Convert.ToDouble(txtDutyAmount.Text.Trim());
            }

            if (txtAmount.Text.Trim() != "")
            {
                Amount = Convert.ToDouble(txtAmount.Text.Trim());
            }

            TotalAmnt = Amount;
            txtTotalAmnt.Text = Convert.ToString(TotalAmnt);
        }
    }

    protected void txtInterestAmnt_OnTextChanged(object sender, EventArgs e)
    {
        TotalAmnt();
    }

    protected void txtPenaltyAmnt_OnTextChanged(object sender, EventArgs e)
    {
        TotalAmnt();
    }

    protected void txtAmount_OnTextChanged(object sender, EventArgs e)
    {
        TotalAmnt();
    }

    protected void txtDutyAmount_OnTextChanged(object sender, EventArgs e)
    {
        TotalAmnt();
    }

    protected void ddlExpenseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtTotalAmnt.Text = txtDutyAmount.Text;

        if (ddlExpenseType.SelectedValue == "1") //  Duty Payment Expense Type
        {
            trDutyInterest.Visible = true;
            trDutyPenalty.Visible = true;
            trDutyAmount.Visible = true;
            txtAmount.Text = "";
            trAmount.Visible = false;
            trACPRD.Visible = true;
            // trPenalApprMail.Visible = true;

            txtTotalAmnt.Text = "";
            TotalAmnt();
            //txtTotalAmnt.Text = txtDutyAmount.Text;
        }
        else
        {
            trDutyInterest.Visible = false;
            trDutyPenalty.Visible = false;
            // txtDutyAmount.Text = "";
            trDutyAmount.Visible = false;
            trAmount.Visible = true;
            trACPRD.Visible = false;
            // trPenalApprMail.Visible = false;
            txtPenaltyMail.Text = "";
            TotalAmnt(); //txtTotalAmnt.Text = "";
        }

        if (ddlExpenseType.SelectedValue == "6") // Other payment request type
        {
            rfvRemark.Visible = true;
        }
        else
        {
            rfvRemark.Visible = false;
        }
    }

    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtJobNumber.Text.Trim() != "")
        {           
            string ModuleId = hdnModuleId.Value;
            txtTotalAmnt.Text = string.Empty;
            ddlBranch.DataBind();
            if (hdnJobId.Value != "0")
            {
                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(hdnModuleId.Value));
                if (dsGetJobDetail != null)
                {
                    //Block Fund Request for Job - Flag - IsFundAllowed 1 = Alloswd, ) -Block
                    if (dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"] != DBNull.Value)
                    {
                        string IsAllowed = dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"].ToString();

                        if (IsAllowed == "0")
                        {
                            lblError.Text = "<b>Fund Request Blocked For Job No - </b>" + txtJobNumber.Text;
                            lblError.CssClass = "errorMsg";

                            txtJobNumber.Text = "";
                            hdnJobId.Value = "0";
                            txtAmount.Text = "";
                            txtPaidTo.Text = "";

                            return;
                        }
                    }

                    else if(ModuleId == "1")
                    {
                        string strNotingStatus = dsGetJobDetail.Tables[0].Rows[0]["NotingStatus"].ToString();
                        string strJobType = dsGetJobDetail.Tables[0].Rows[0]["JobType"].ToString();

                        if (strJobType != "14" && strJobType != "101" && strJobType != "201")
                        {
                            if (strNotingStatus != "True")
                            {
                                ViewState["JobrefNO"] = txtJobNumber.Text;
                                ResetControls();
                                lblError.Text = "<b>First Complete The Noting Details</b>";
                                lblError.CssClass = "errorMsg";
                                txtJobNumber.Text = ViewState["JobrefNO"].ToString();
                                return;
                            }
                        }                     
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"] != DBNull.Value)
                    {
                        ddlBranch.SelectedValue = dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString();
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
                    {
                        txtDutyAmount.Text = Convert.ToString(dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"]);
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"] != DBNull.Value)
                    {
                        if (Convert.ToString(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]) != "")
                        {
                            txtPlanningDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]).ToString("dd/MM/yyyy");
                        }
                    }

                    lblConsignee.Text = dsGetJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
                }
            }
        }
        else
        {
            ddlBranch.DataBind();
            txtPlanningDate.Text = "";
            lblConsignee.Text = "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fuDocument2.HasFile)
        {
            btnSaveDocument2_Click(fuDocument2, EventArgs.Empty);
        }

        int result = -123, JobId = 0, ExpenseTypeId = 0, BranchId = 0, PaymentTypeId = 0, count = 1, Isready = 0;
        int IsInterestReq = 0, IsPenaltyReq = 0, ACPNonACP = 0, RdDutyPenalty = 0;
        decimal dcAmount = 0, dcAdvanceAmount = 0, DutyAmount = 0;
        decimal dcDutyAmount = 0, dcInterestAmount = 0, dcPenaltyAmount = 0;
        Boolean AdvanceRecd = false;
        string strPenaltyCopy = "", challanNo = "", JobRefNo = "";               

        if (fuPenaltyCopy != null && fuPenaltyCopy.HasFile)
        {
            strPenaltyCopy = UploadFiles(fuPenaltyCopy);
        }

        if (rblAdvanceRecd.SelectedValue == "1")
            AdvanceRecd = true;
        if (hdnJobId.Value != "0")
            JobId = Convert.ToInt32(hdnJobId.Value);
        if (ddlExpenseType.SelectedValue != "0")
            ExpenseTypeId = Convert.ToInt32(ddlExpenseType.SelectedValue);

        if (ddlBranch.SelectedValue != "")
        {
            if (ddlBranch.SelectedValue != "0")
                BranchId = Convert.ToInt32(ddlBranch.SelectedValue);
        }
        else
        {
            lblError.Text = "Please Select Branch";
            lblError.CssClass = "errorMsg";
            return;
        }


        if (ddlPaymentType.SelectedValue != "0")
            PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (ExpenseTypeId != 0) // && BranchId != 0
        {
            #region VALIDATE THE REQUEST TYPE DROP DOWN

            if (ViewState["AnnexureDoc2"] != null)
            {
                DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
                if (dtAnnexure != null)
                {
                    if (dtAnnexure.Rows.Count > 0)
                    {
                        count = 0;      // success
                    }
                    else
                    {
                        lblError.Text = "Please browse atleast 1 document.";
                        lblError.CssClass = "errorMsg";
                        count = 1;      // error
                    }
                }
                else
                {
                    lblError.Text = "Please browse atleast 1 document.";
                    lblError.CssClass = "errorMsg";
                    count = 1;      // error
                }
            }
            else
            {
                count = 1;          // error
                lblError.Text = "Please browse atleast 1 document.";
                lblError.CssClass = "errorMsg";
            }

            #endregion

            if(txtJobNumber.Text.Trim()!="")
            {
                JobRefNo = txtJobNumber.Text.Trim();
            }

            if (txtAmount.Text.Trim() != "")
                dcAmount = Convert.ToDecimal(txtAmount.Text.Trim());
            if (txtAdvanceAmt.Text.Trim() != "")
                dcAdvanceAmount = Convert.ToDecimal(txtAdvanceAmt.Text.Trim());

            if (txtInterestAmnt.Text.Trim() != "")
            {
                dcInterestAmount = Convert.ToDecimal(txtInterestAmnt.Text.Trim());
            }
            if (txtPenaltyAmnt.Text.Trim() != "")
            {
                dcPenaltyAmount = Convert.ToDecimal(txtPenaltyAmnt.Text.Trim());
            }

            if (rdlInterest.SelectedValue != "")
            {
                IsInterestReq = Convert.ToInt32(rdlInterest.SelectedValue);
            }
            if (rdlPenalty.SelectedValue != "")
            {
                IsPenaltyReq = Convert.ToInt32(rdlPenalty.SelectedValue);
            }

            challanNo = txtChallanNo.Text.Trim();

            if (txtDutyAmount.Text.Trim() != "")
            {
                DutyAmount = Convert.ToDecimal(txtDutyAmount.Text.Trim());
            }

            Isready = Validation();

            if (ddlExpenseType.SelectedValue == "1")
            {
                ACPNonACP = Convert.ToInt32(ddlACPNonACP.SelectedValue);
                RdDutyPenalty = Convert.ToInt32(ddlType.SelectedValue);
            }

            int ModuleId = 0;
            if (hdnModuleId.Value != "")
            {
                ModuleId = Convert.ToInt32(hdnModuleId.Value);
            }

            if (count == 0 && Isready == 0)
            {
                if (txtTotalAmnt.Text.Trim() == "" || txtTotalAmnt.Text.Trim() == "0")
                {
                    txtTotalAmnt.Text = dcAmount.ToString();
                }

                result = AccountExpense.AddJobDetails(JobId, JobRefNo, ExpenseTypeId, PaymentTypeId, dcAmount, txtPaidTo.Text.Trim(), BranchId,
                     txtRemark.Text.Trim(), LoggedInUser.glUserId, AdvanceRecd, dcAdvanceAmount, ACPNonACP, RdDutyPenalty, txtPenaltyMail.Text.Trim(),
                     DutyAmount, challanNo, IsInterestReq, IsPenaltyReq, dcInterestAmount, dcPenaltyAmount, strPenaltyCopy,txtTotalAmnt.Text.Trim(), ModuleId);

                if (result != 1 && result != 2 && result!= 3)
                {
                    /************************************************/
                    if (txtPlanningDate.Text.Trim() != "")
                    {
                        DateTime PlanningDate = DateTime.MinValue;
                        PlanningDate = Commonfunctions.CDateTime(txtPlanningDate.Text.Trim());

                        if (PlanningDate != DateTime.MinValue)
                        {
                            int AddExamineResult = AccountExpense.AddExamineDetails(JobId, PlanningDate, LoggedInUser.glUserId);
                        }
                    }

                    /**************************************************/

                    #region ADD DOCUMENTS DETAILS
                    if (Convert.ToString(ViewState["AnnexureDoc2"]) != "")
                    {
                        DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
                        if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
                        {
                            string DocPath = "", FileName = "";
                            for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                            {
                                if (dtAnnexure.Rows[i]["DocPath"] != null)
                                    DocPath = dtAnnexure.Rows[i]["DocPath"].ToString();
                                if (dtAnnexure.Rows[i]["DocumentName"] != null)
                                    FileName = dtAnnexure.Rows[i]["DocumentName"].ToString();
                                int result_Doc = AccountExpense.AddExpenseDocDetails(result, DocPath, FileName, LoggedInUser.glUserId);
                            }
                        }
                    }
                    #endregion
                    
                    SendFundRequestMail(result);
                    ResetControls();
                    lblError.Text = "Payment Request Successfully Create!.";
                    lblError.CssClass = "success";

                    DataTable dtAnnexureDoc2 = new DataTable();
                    dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
                    ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
                }
                else if (result == 1)
                {
                    lblError.Text = "Payment Request Already Exists for Job Number and Expense Type and Amount.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 3)
                {
                    lblError.Text = "Please Select Job Number From Search Box!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while Creating Payment Request. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    private bool SendFundRequestMail(int PaymentId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", JobRefno = "", BranchName = "", PaymentTypeName = "", ConsigneeName="",
            ExpenseTypeName = "", Amount = "", PaidTo = "", Remark = "", CreatedBy = "", CreatedByEmail = "", HoldedBy = "", HoldedByEmail = "", HoldReason = "";
        bool bEmailSucces = false;
        StringBuilder strbuilder = new StringBuilder();

        try
        {
            if (PaymentId != 0)
            {
                DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
                if (dsGetPaymentDetails != null)
                {
                    JobRefno        =   dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();
                    ConsigneeName   =   dsGetPaymentDetails.Tables[0].Rows[0]["ConsigneeName"].ToString();
                    BranchName      =   dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();
                    PaymentTypeName =   dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName1"].ToString();
                    ExpenseTypeName =   dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
                    Amount          =   dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();
                    PaidTo          =   dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();
                    Remark          =   dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();
                    CreatedBy       =   dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
                    CreatedByEmail  =   dsGetPaymentDetails.Tables[0].Rows[0]["CreatedByEmail"].ToString();
                    HoldedBy        =   dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByName"].ToString();
                    HoldedByEmail   =   dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByEmail"].ToString();
                    HoldReason      =   dsGetPaymentDetails.Tables[0].Rows[0]["HoldReason"].ToString();

                    if(ExpenseTypeName.Trim().ToLower()== "duty payment")
                    {
                        strCustomerEmail = "bsavita@babajishivram.com";
                    }
                    else
                    {
                        strCustomerEmail = "neeraj.mandowara@babajishivram.com";
                    }                  
                                        
                    strCCEmail = CreatedByEmail;
                    
                    strSubject = "Fund Request of Rs " + Amount + " For " + ExpenseTypeName + " - " + JobRefno;

                    if (strCustomerEmail == "" || strSubject == "")
                        return bEmailSucces;
                    else
                    {
                        MessageBody = "Dear Sir,<BR><BR> Expense Fund Request Generated For Job No - "+ JobRefno +"<BR><BR>";

                        strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:60%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Babaji Job No</th><td style='padding-left: 3px'>" + JobRefno + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Consignee/Shipper</th><td style='padding-left: 3px'>" + ConsigneeName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Branch</th><td style='padding-left: 3px'>" + BranchName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Payment Type</th><td style='padding-left: 3px'>" + PaymentTypeName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Request Type</th><td style='padding-left: 3px'>" + ExpenseTypeName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Amount</th><td style='padding-left: 3px'>" + Amount + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Paid To</th><td style='padding-left: 3px'>" + PaidTo + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Remark</th><td style='padding-left: 3px'>" + Remark + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Created By</th><td style='padding-left: 3px'>" + CreatedBy + "</td></tr>");
                        strbuilder = strbuilder.Append("</table>");

                        MessageBody = MessageBody + strbuilder;
                        MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + CreatedBy;

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

                        bEmailSucces = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                        return bEmailSucces;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception en)
        {
            return false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
        Response.Redirect("FillJobDetails.aspx");
    }

    protected void ResetControls()
    {
        txtJobNumber.Text = "";
        hdnJobId.Value = "0";
        txtAmount.Text = "";
        txtPaidTo.Text = "";
        txtRemark.Text = "";
        txtPlanningDate.Text = "";
        lblConsignee.Text = "";
        ddlExpenseType.Items.Clear();
        ddlBranch.DataBind();
        ddlExpenseType.DataBind();
        ddlPaymentType.Items.Clear();
        ddlPaymentType.DataBind();
        lblError.Text = "";
        hdnNewPaymentLid.Value = Convert.ToString(AccountExpense.GetNewPaymentLid()); // Get New Payment PkId
        ViewState["AnnexureDoc2"] = null;
        rptDocument2.DataSource = "";
        rptDocument2.DataBind();
        rptDocument2.Visible = false;
        rblAdvanceRecd.SelectedValue = "0";
        rblAdvanceRecd_OnSelectedIndexChanged(null, EventArgs.Empty);
        txtAdvanceAmt.Text = "";
        txtTotalAmnt.Text = "";
        ddlACPNonACP.SelectedValue = "0";
        ddlType.SelectedValue = "0";
        txtChallanNo.Text = "";
        txtDutyAmount.Text = "";
        txtInterestAmnt.Text = "";
        txtPenaltyAmnt.Text = "";
        //ddlBranch.SelectedValue = "0";
        //rdlInterest.SelectedValue = "0";
        //rdlPenalty.SelectedValue = "0";
    }

    protected void rdlInterest_OnSelectedIndexChanged(object sender, EventArgs e)  //radio button click
    {
        if (rdlInterest.SelectedItem.Text.ToLower() == "yes")
        {
            //lblInterestAmnt.Visible = true;
            //txtInterestAmnt.Visible = true;   

            tdlblInterestAmnt.Visible = true;
            tdtxtInterestAmnt.Visible = true;
        }
        else
        {
            //lblInterestAmnt.Visible = false;            
            //txtInterestAmnt.Visible = false;
            tdlblInterestAmnt.Visible = false;
            tdtxtInterestAmnt.Visible = false;

            txtInterestAmnt.Text = "";
        }
    }

    protected void rdlPenalty_OnSelectedIndexChanged(object sender, EventArgs e)  //radio button click
    {
        if (rdlPenalty.SelectedItem.Text.ToLower() == "yes")
        {
            //lblPenaltyAmnt.Visible = true;
            //txtPenaltyAmnt.Visible = true;
            //fuPenaltyCopy.Visible = true;
            tdlblPenaltyAmnt.Visible = true;
            tdtxtPenaltyAmnt.Visible = true;
        }
        else
        {
            //lblPenaltyAmnt.Visible = false;            
            //txtPenaltyAmnt.Visible = false;
            //fuPenaltyCopy.Visible = false;
            tdlblPenaltyAmnt.Visible = false;
            tdtxtPenaltyAmnt.Visible = false;

            txtPenaltyAmnt.Text = "";
        }
    }

    #region SUPPORTING DOCUMENT

    protected void btnSaveDocument2_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (fuDocument2 != null && fuDocument2.HasFile)
            fileName = UploadFiles(fuDocument2);

        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
            if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
            {
                for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                {
                    if (dtAnnexure.Rows[i]["PkId"] != null)
                    {
                        PkId = Convert.ToInt32(dtAnnexure.Rows[i]["PkId"].ToString());
                        PkId++;
                    }
                }
            }
            if (dtAnnexure != null)
                OriginalRows = dtAnnexure.Rows.Count;              //get original rows of grid view.

            dtAnnexure.Rows.Add(PkId, fileName, fuDocument2.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["AnnexureDoc2"] = dtAnnexure;
            BindGrid();
            if (OriginalRows < AfterInsertedRows)
            {
                lblError.Text = "Document Added successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void rptDocument2_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["AnnexureDoc2"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["AnnexureDoc2"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.Text = "Successfully Deleted Document.";
                lblError.CssClass = "success";
                rptDocument2.DataBind();
            }
            else
            {
                lblError.Text = "Error while deleting container details. Please try again later..!!";
                lblError.CssClass = "success";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadDoc(FilePath);
        }
    }

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "Expense_" + hdnNewPaymentLid.Value + "\\";

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

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
        else
            ServerPath = ServerPath + DocumentPath;
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    protected void BindGrid()
    {
        if (ViewState["AnnexureDoc2"].ToString() != "")
        {
            DataTable dtAnnexureDoc = (DataTable)ViewState["AnnexureDoc2"];
            rptDocument2.DataSource = dtAnnexureDoc;
            rptDocument2.DataBind();
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
}