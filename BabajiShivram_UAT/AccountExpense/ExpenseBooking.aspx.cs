using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountExpense_ExpenseBooking : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Expense Booking";
    }
    private int PostRIMExpense()
    {
        BJVRIMExpense AccExpense = new BJVRIMExpense();
                
        //AccExpense.AY = "";
        AccExpense.Type         =   "M1";
        AccExpense.BillNo       =   Convert.ToInt32(txtInvoiceNo.Text.Trim());
        AccExpense.Suffix       =   "";
        AccExpense.Ser_no       =   1;
        AccExpense.BillDate     =   Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        AccExpense.Par_Code     =   hdnPayToCode.Value.Trim();
        AccExpense.RefNO        =   txtJobNumber.Text.Trim(); 
        AccExpense.Narration    =   txtNarration.Text.Trim();
        AccExpense.Amount       =   140;
        AccExpense.JvpCode      =   hdnVendorCode.Value.Trim();
        AccExpense.Chargable    =   "";
        AccExpense.ChequeNo     =   lblChequeNo.Text.Trim();

        if(lblChequeDate.Text.Trim() != "")
        AccExpense.ChequeDate   = Commonfunctions.CDateTime(lblChequeDate.Text.Trim());

        AccExpense.BankName     =   lblBankName.Text.Trim();
        AccExpense.CreatedBy    =   "";
        AccExpense.CreatedTime  =   "";
        AccExpense.ModifiedBy   =   "";
        AccExpense.ModifiedTime =   "";
        AccExpense.Approve      =   true;
        AccExpense.Approveby    =   "";
        AccExpense.ApproveDate =    DateTime.Now;
        AccExpense.Remarks      =   "Test Expense Remark";
        AccExpense.CSTCODEOne   =   "";
        AccExpense.CSTCODETwo   =   "";
        AccExpense.CSTCODEThree =   "";
        AccExpense.CSTCODEFour  =   "";
        AccExpense.VCode        =   "";
        AccExpense.ChargeCode   =   "";
        AccExpense.Paid_To      =   hdnPayToCode.Value.Trim();
        AccExpense.RNO          =   "";
        AccExpense.Paid_Date    =   DateTime.Now;
        AccExpense.Seq          =   "1";
        AccExpense.OREF         =   "";
        AccExpense.HSN_SC       =   "";
        AccExpense.Txval        =   "1";
        AccExpense.CGSTRT       =   "2";
        AccExpense.CGSTAMT      =   "3";
        AccExpense.SGSTRT       =   "4";
        AccExpense.SGSTAMT      =   "5";
        AccExpense.IGSTRT       =   "12";
        AccExpense.IGSTAMT      =   "560";
        AccExpense.BRDate       =   "";
        //AccExpense.Mark = "";
        AccExpense.BC           =   "";
        //AccExpense.CDate;
        //AccExpense.BalAmt = "";
        //AccExpense.BJVADV = "";

        int result = AccountExpense.FA_PostRIMExpense(AccExpense);

        return result;
    }
    private void AddRimExpense()
    { 
        if (ViewState["vwsExpense"] == null)
        {
            ViewState["vwsExpense"] = GetExpenseDataTable();
        }

        if (ViewState["vwsExpense"] != null)
        {
            DataTable dtExpense;
            dtExpense = (DataTable)ViewState["vwsExpense"];

            DataRow drFinal = dtExpense.NewRow();
            // Data Table

            drFinal["PaidTo"]       =   hdnPayToCode.Value;
            drFinal["ChargeName"]   =   txtChargeCode.Text.Trim();
            drFinal["ChargeCode"]   =   hdnChargeCode.Value.Trim();
            drFinal["GSTIN"]        =   txtGSTIN.Text.Trim();
            drFinal["InvoiceNo"]    =   txtInvoiceNo.Text.Trim();
            drFinal["InvoiceDate"]  =   txtInvoiceDate.Text.Trim();

            drFinal["InvoiceTaxAmount"]     = txtInvoiceTaxAmount.Text.Trim();
            drFinal["InvoiceTotalAmount"]   = txtInvoiceTaxAmount.Text.Trim();

            drFinal["IGST"] = txtIGST.Text.Trim();
            drFinal["CGST"] = txtCGST.Text.Trim();
            drFinal["SGST"] = txtSGST.Text.Trim();

            drFinal["ReceiptNumber"]    =   txtReceiptNumber.Text.Trim();
            drFinal["ReceiptDate"]  =   txtReceiptDate.Text.Trim();
            drFinal["VendorName"]   =   txtVendorName.Text.Trim();
            drFinal["VendorCode"]   =   hdnVendorCode.Value;
            drFinal["Narration"]    =   txtNarration.Text.Trim();

            dtExpense.Rows.Add(drFinal);
            dtExpense.AcceptChanges();

            ViewState["vwsExpense"] = dtExpense;

            gvPaymentDetail.DataSource = dtExpense;
            gvPaymentDetail.DataBind();
        }
    }
    protected void btnSaveBooking_Click(object sender, EventArgs e)
    {
        bool IsValid = false;
        if (ddRIMorAG.SelectedValue == "1")
        {
            AddRimExpense();
            if (ViewState["vwsExpense"] == null && ddRIMorAG.SelectedValue == "1") // For RIM Expense Detail Required
            {
                lblError.Text = "Please Enter Expense Detail!";
                lblError.CssClass = "errorMsg";

                return;
            }
        }
        if(ddRIMorAG.SelectedIndex > 0)
        {
            string strJobRefNo = "", strPayTo = "", strPayToCode = "";

            int JobId = 0, PayModeID = 0, RIMorAGID = 0, ChequeIssueID = 0, BookingID = 0;
            bool bSplitExpense = false;

            strJobRefNo =   txtJobNumber.Text.Trim();
            strPayTo    =   txtPayTo.Text.Trim();
            strPayToCode =  hdnPayToCode.Value;
            PayModeID   =   Convert.ToInt32(ddlPaymentType.SelectedValue);
            RIMorAGID   =   Convert.ToInt32(ddRIMorAG.SelectedValue);

            if (hdnJobId.Value != "")
            {
                JobId = Convert.ToInt32(hdnJobId.Value);
            }

            if (hdnChequeIssueID.Value != "")
            {
                ChequeIssueID = Convert.ToInt32(hdnChequeIssueID.Value);
            }

            // Add Job Booking Detail
            BookingID = AccountExpense.FA_AddExpenseBooking(JobId, strJobRefNo, PayModeID, RIMorAGID, strPayTo, strPayToCode, ChequeIssueID, loggedInUser.glUserId);

            // RIM Expense Detail
            if (BookingID > 0)
            {
                if (RIMorAGID == 1)  // RIM
                {
                    SaveExpenseRIM(BookingID);

                    PostRIMExpense();
                }
                else if (RIMorAGID == 2) // Agency
                {
                    SaveExpenseAgency(BookingID);
                }

                // Add Expense Booking Detail

            }//
            else if (BookingID == -1)
            {
                lblError.Text = "Booking Detail Already Exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
        }//END_ELSE_ViewState
        
    }
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        int PayMode = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (PayMode == (Int32)EnumFAPayMode.Cheque)
        {
            ddlPaymentType.SelectedIndex = 0;
        }

        ModalPopupCheque.Hide();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrackExpenseBooking.aspx");
    }

    #region OnChange Event
    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtJobNumber.Text.Trim() != "")
        {
            if (hdnJobId.Value != "0")
            {
                // Fill Paid To Detail

                //AccountExpense.FillJobExpensePaidTo(ddPaidTo, txtJobNumber.Text.Trim());

                // Get JobDetail
                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value));

                if (dsGetJobDetail.Tables.Count > 0)
                {
                    if (dsGetJobDetail.Tables[0].Rows.Count > 0)
                    {
                        lblCustomer.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                        hdnCustomerId.Value = dsGetJobDetail.Tables[0].Rows[0]["CustomerID"].ToString();

                        if(dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"] != DBNull.Value)
                        {
                            txtGSTIN.Text = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
                        }
                    }
                }
            }
        }
        else
        {
            // Clear Field
            lblCustomer.Text = "";

        }
    }
    protected void ddRIMorAG_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int RIMorAG = Convert.ToInt32(ddRIMorAG.SelectedValue);

        if (RIMorAG == (Int32)EnumFARIMorAG.RIM) // RIM
        {
            pnlRIM.Visible = true;
            pnlAG.Visible = false;
        }
        else
        {
            pnlRIM.Visible = false;
            pnlAG.Visible = true;
        }
    }
    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int PayMode = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (PayMode == (Int32)EnumFAPayMode.Cheque)
        {
            gvChequeDetail.DataBind();
            ChequeSqlDataSource.DataBind();

            if (gvChequeDetail.Rows.Count > 0)
            {
                ModalPopupCheque.Show();
            }
            else
            {
                lblError.Text = "Issued Cheque Detail Not Found for Job No - " + txtJobNumber.Text.Trim();
                lblError.CssClass = "errorMsg";

                ddlPaymentType.SelectedIndex = 0;
            }
        }
        else
        {
            gvChequeDetail.DataSource = null;
            gvChequeDetail.DataBind();

            resetChequeDetail();
            pnlChequeIsssued.Visible = false;
        }
    }
    protected void lnkChequeSelect(object sender, EventArgs e)
    {
        int ChequeIssueID = Convert.ToInt32(((LinkButton)sender).CommandArgument);
        hdnChequeIssueID.Value = ChequeIssueID.ToString();

        DataSet dsResult = AccountExpense.FA_GetIssueInstrumentByID(ChequeIssueID);

        if (dsResult.Tables.Count > 0)
        {
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                pnlChequeIsssued.Visible = true;

                lblChequeNo.Text = dsResult.Tables[0].Rows[0]["ChequeNo"].ToString();

                if (dsResult.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                {
                    lblChequeDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");
                }

                lblBankName.Text = dsResult.Tables[0].Rows[0]["BankName"].ToString();
                lblChequePaidTo.Text = dsResult.Tables[0].Rows[0]["PayTo"].ToString();
                lblChequeIssuedBy.Text = dsResult.Tables[0].Rows[0]["ChequeIssuedBy"].ToString();

                if (dsResult.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                {
                    lblChequeIssuedDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ChequeIssuedDate"]).ToString("dd/MM/yyyy");
                }
            }
            else
            {
                lblError.Text = "Issued Cheque Detail Not Found for Job No - " + txtJobNumber.Text.Trim();
                lblError.CssClass = "errorMsg";

                ddlPaymentType.SelectedIndex = 0;
                hdnChequeIssueID.Value = "0";
            }
        }
        else
        {
            lblError.Text = "Issued Cheque Detail Not Found for Job No - " + txtJobNumber.Text.Trim();
            lblError.CssClass = "errorMsg";

            ddlPaymentType.SelectedIndex = 0;
            hdnChequeIssueID.Value = "0";
        }

        ModalPopupCheque.Hide();
    }

    #region Function
    private void SaveExpenseRIM(int BookingID)
    {
        DataTable dtExpense;
        dtExpense = (DataTable)ViewState["vwsExpense"];

        foreach (DataRow drFinal in dtExpense.Rows)
        {
            decimal decInvoiceTaxAmount = 0.0m;
            decimal decInvoiceTotalAmount = 0.0m;

            int intIGST = 0, intCGST = 0, intSGST = 0;

            DateTime dtReceiptDate = DateTime.MinValue;
            DateTime dtInvoiceDate = DateTime.MinValue;

            string strPaidTo    = drFinal["PaidTo"].ToString();
            string strChargeName = drFinal["ChargeName"].ToString();
            string strChargeCode = drFinal["ChargeCode"].ToString();
            string strGSTIN     = drFinal["GSTIN"].ToString();
            string strInvoiceNo = drFinal["InvoiceNo"].ToString();

            string test = drFinal["InvoiceDate"].ToString();

            dtInvoiceDate = Commonfunctions.CDateTime(drFinal["InvoiceDate"].ToString());

            decInvoiceTaxAmount = Convert.ToDecimal(drFinal["InvoiceTaxAmount"]);
            decInvoiceTotalAmount = Convert.ToDecimal(drFinal["InvoiceTotalAmount"]);

            intIGST = Convert.ToInt32(drFinal["IGST"]);
            intCGST = Convert.ToInt32(drFinal["CGST"]);
            intSGST = Convert.ToInt32(drFinal["SGST"]);

            string strReceiptNumber = drFinal["ReceiptNumber"].ToString();

            dtReceiptDate = Commonfunctions.CDateTime(drFinal["ReceiptDate"].ToString());

            string strVendorName = drFinal["VendorName"].ToString();
            string strVendorCode = drFinal["VendorCode"].ToString();
            string strNarration = drFinal["Narration"].ToString();

            int RIMExpenseID = AccountExpense.FA_AddExpenseBookingRIM(BookingID, strChargeName, strChargeCode, strGSTIN, strInvoiceNo,
              dtInvoiceDate, decInvoiceTaxAmount, decInvoiceTotalAmount, intCGST, intSGST, intIGST, strReceiptNumber,
              dtReceiptDate, strVendorName, strVendorCode, strNarration, loggedInUser.glUserId);

            if (RIMExpenseID >= 0)
            {
                lblError.Text = "RIM Expense Booking Detail Saved Successfully!";
                lblError.CssClass = "success";

                // Post To FA - ERC - Bank/Cheque Details

                //AccountExpense.FA_PostRimERC(BookingID, RIMExpenseID, loggedInUser.glUserId);
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }

        }// END_ForEach_Detail

        ViewState["vwsExpense"] = null;
    }
    private void SaveExpenseAgency(int BookingID)
    {
        string strChargeName = "", strChargeCode = "", ApprovalFilePath = "", strServiceRemark = "";
        decimal decAgencyAmount = 0.0m;
        decimal decPartyChargeAmount = 0.0m;
        bool bChargeToParty = false;
        int ApprovedBy = 0, ServiceId = 0;

        strChargeName = txtAGChargeName.Text.Trim();
        strChargeCode = hdnChargeCode.Value.Trim();

        if (txtAGAmount.Text.Trim() !="")
        {
            decAgencyAmount =Convert.ToDecimal(txtAGAmount.Text.Trim());
        }
        if(rdAGChargeToParty.SelectedValue == "1")
        {
            bChargeToParty = true;
        }
        if (txtAGChargeAmount.Text.Trim() != "")
        {
            decPartyChargeAmount = Convert.ToDecimal(txtAGChargeAmount.Text.Trim());
        }

        if (hdnAGApprovalID.Value != "")
        {
            ApprovedBy =Convert.ToInt32(hdnAGApprovalID.Value);
        }

        ServiceId = Convert.ToInt32(ddServiceRendered.SelectedValue);

        strServiceRemark = txtServiceRemarks.Text.Trim();

        int result = AccountExpense.FA_AddExpenseBookingAgency(BookingID, strChargeName, strChargeCode, decAgencyAmount, bChargeToParty,
            ApprovedBy, ApprovalFilePath, decPartyChargeAmount, ServiceId, strServiceRemark, loggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Agency Expense Booking Detail Saved Successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }

        }// END_ForEach_Detail   
    private void resetChequeDetail()
    {
        lblChequeNo.Text = "";
        lblChequeDate.Text = "";
        lblBankName.Text = "";
        lblChequePaidTo.Text = "";
        lblChequeIssuedBy.Text = "";
        lblChequeIssuedDate.Text = "";
        hdnChequeIssueID.Value = "0";
    }
    private DataTable GetExpenseDataTable()
    {
        DataTable dtExpense = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colPaidTo = new DataColumn("PaidTo", Type.GetType("System.String"));
        DataColumn colChargeName = new DataColumn("ChargeName", Type.GetType("System.String"));
        DataColumn colChargeCode = new DataColumn("ChargeCode", Type.GetType("System.String"));
        DataColumn colGSTIN = new DataColumn("GSTIN", Type.GetType("System.String"));
        DataColumn colInvoiceNo = new DataColumn("InvoiceNo", Type.GetType("System.String"));
        DataColumn colInvoiceDate = new DataColumn("InvoiceDate", Type.GetType("System.String"));

        DataColumn colInvoiceTaxAmount = new DataColumn("InvoiceTaxAmount", Type.GetType("System.String"));
        DataColumn colCGST = new DataColumn("CGST", Type.GetType("System.String"));
        DataColumn colSGST = new DataColumn("SGST", Type.GetType("System.String"));
        DataColumn colIGST = new DataColumn("IGST", Type.GetType("System.String"));

        DataColumn colInvoiceTotalAmount = new DataColumn("InvoiceTotalAmount", Type.GetType("System.String"));
        DataColumn colReceiptNumber = new DataColumn("ReceiptNumber", Type.GetType("System.String"));
        DataColumn colReceiptDate = new DataColumn("ReceiptDate", Type.GetType("System.String"));
        DataColumn colVendorName = new DataColumn("VendorName", Type.GetType("System.String"));
        DataColumn colVendorCode = new DataColumn("VendorCode", Type.GetType("System.String"));
        DataColumn colNarration = new DataColumn("Narration", Type.GetType("System.String"));

        dtExpense.Columns.Add(colSL);

        dtExpense.Columns.Add(colPaidTo);
        dtExpense.Columns.Add(colChargeName);
        dtExpense.Columns.Add(colChargeCode);
        dtExpense.Columns.Add(colGSTIN);
        dtExpense.Columns.Add(colInvoiceNo);
        dtExpense.Columns.Add(colInvoiceDate);

        dtExpense.Columns.Add(colInvoiceTaxAmount);
        dtExpense.Columns.Add(colCGST);
        dtExpense.Columns.Add(colSGST);
        dtExpense.Columns.Add(colIGST);

        dtExpense.Columns.Add(colInvoiceTotalAmount);
        dtExpense.Columns.Add(colReceiptNumber);
        dtExpense.Columns.Add(colReceiptDate);
        dtExpense.Columns.Add(colVendorName);
        dtExpense.Columns.Add(colVendorCode);
        dtExpense.Columns.Add(colNarration);

        dtExpense.AcceptChanges();

        return dtExpense;
    }
    #endregion

    #endregion
}