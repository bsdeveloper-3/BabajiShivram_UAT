using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AccountExpense_InvoiceFinal : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        CalExtInvoiceDate.EndDate = DateTime.Now;
        MskEdtValInvoice.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskEdtValPayDueDate.MaximumValue = DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Final Invoice";

        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        if (!IsPostBack)
        {
            GetPaymentRequest();
        }
    }

    private void GetPaymentRequest()
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        int InvoiceID = Convert.ToInt32(Session["InvoiceId"]);
        int StatusId = 0;

        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            StatusId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lStatus"]);

            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            hdnJobId.Value = dsDetail.Tables[0].Rows[0]["JobId"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            
            lblVendorGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            lblPAN.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();
            lblCreditTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString(); 
            
            lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            lblInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            lblInvoiceDate.Text = dsDetail.Tables[0].Rows[0]["InvoiceDate"].ToString();
            lblPaymentDueDate.Text = dsDetail.Tables[0].Rows[0]["PaymentDueDate"].ToString();
            lblPatymentRequestDate.Text = dsDetail.Tables[0].Rows[0]["dtDate"].ToString(); lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
            
            lblVendorGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            lblPAN.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblVendorType.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNType"].ToString();

            lblRIM.Text = dsDetail.Tables[0].Rows[0]["RIMType"].ToString();
            lblInvoiceType.Text = dsDetail.Tables[0].Rows[0]["InvoiceTypeName"].ToString();

            lblInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

            lblAdvanceReceived.Text = dsDetail.Tables[0].Rows[0]["AdvanceReceived"].ToString();
            lblAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();

            lblBillingPartyName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblBillingGSTN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            lblBillingPAN.Text = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();

            lblRequestRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();
            lblRequestBy.Text = dsDetail.Tables[0].Rows[0]["UserName"].ToString();
            lblPaidAmount.Text = dsDetail.Tables[0].Rows[0]["PaidAmount"].ToString();


            if (dsDetail.Tables[0].Rows[0]["InvoiceAmount"] != DBNull.Value)
            {
                lblTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["InvoiceAmount"].ToString();
            }
            
            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                lblInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                lblPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                lblPatymentRequestDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

        }


        // Final Invoice

        txtCongisgneeGSTIN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
        txtCongisgneeName.Text = dsDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
        hdnConsigneeCode.Value = dsDetail.Tables[0].Rows[0]["ConsigneeCode"].ToString();
        hdnConsigneePANNo.Value = dsDetail.Tables[0].Rows[0]["ConsigneePAN"].ToString();

        if (txtCongisgneeGSTIN.Text == "")
        {
            chkNoGSTINCustomer.Checked = true;
        }
        if(Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsRIM"]) == true)
        {
            ddRIM.SelectedValue = "1";
        }
        // Get Job Detail

        DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value), 1);

        if (dsGetJobDetail.Tables.Count > 0)
        {
            hdnBoEGSTIN.Value = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["InvoiceId"] == null)
        {
            Response.Redirect("PendingInvoiceFinal.aspx");
        }

        int ProformaInvoiceID = Convert.ToInt32(Session["InvoiceId"]);

        bool bIsInputValid = ValidateInput();

        if (bIsInputValid)
        {
            int JobId; string strJobRefNO; int ModuleID; int InvoiceType = 1; bool isRIM = false;
            int VendorGSTNType, PaymentTypeId = 0;
            string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

            string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;
            bool IsAdvanceReceived = false; decimal AdvanceAmount = 0m;
            string strPaymentTerms; string strInvoiceFilePath = "";

            Decimal decInvoiceAmount = 0m, decTaxableAmount = 0m, decGSTAmount = 0m;

            DateTime dtInvoiceDate = DateTime.MinValue;

            JobId = Convert.ToInt32(hdnJobId.Value);

            strConsigneeGSTIN = txtCongisgneeGSTIN.Text.Trim();
            strConsigneeName = txtCongisgneeName.Text.Trim();
            strConsigneeCode = hdnConsigneeCode.Value.Trim();
            strConsigneePAN = hdnConsigneePANNo.Value.Trim();


            strInvoiceNo = txtInvoiceNo.Text.Trim();
            InvoiceType = Convert.ToInt32(ddInvoiceType.SelectedValue);

            strRemark = txtRemark.Text.Trim();

            if (ddRIM.SelectedValue == "1")
            {
                isRIM = true;
            }

            if (txtInvoiceDate.Text != "")
            {
                dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
            }

            if (InvoiceType == 1) // Final Invoice
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
                }
                if (lblTotalTaxableValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());
                }
                if (lblTotalGST.Text.Trim() != "")
                {
                    decGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
                }
            }
            
            int RequestId = AccountExpense.AddFinalInvoice(ProformaInvoiceID,isRIM, strConsigneeGSTIN, strConsigneeName, strConsigneeCode, 
                strConsigneePAN,strInvoiceNo, dtInvoiceDate, decInvoiceAmount, decTaxableAmount, decGSTAmount, strInvoiceFilePath, strRemark, LoggedInUser.glUserId);

            if (RequestId > 0)
            {
                lblError.Text = "Final Invoice Detail Added Successfully!";
                lblError.CssClass = "success";

                // Invoice Uploaded
                AccountExpense.AddInvoiceStatus(RequestId, 100, strRemark, LoggedInUser.glUserId);

                // Invoice Auto Appproved for Mgmt and Submitted To Accounts Dept
                AccountExpense.AddInvoiceStatus(RequestId, 120, "Auto Approved and  Submitted To Account", LoggedInUser.glUserId);

                if (InvoiceType == 1) // Final Invoice
                {
                    AddInvoiceItem(RequestId);
                }

                //if (fuDocument.HasFile)
                //{
                //    string InvoiceFilePath = "VendorInvoice//";

                //    string FileName = UploadInvoice(fuDocument, "InvoceRcvd");
                //    string FileUploadPath = InvoiceFilePath + FileName;

                //    //  int FileOutput = DBOperations.UpdateVendorInvoiceCopy(TokanNo, FileUploadPath);
                //}

                Session["Success"] = lblError.Text;

                Response.Redirect("AccountSuccess.aspx");

            }
            else if (RequestId == 0)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "System Error", "alert('System Error! Please try after sometime!');", true);
            }
            else if (RequestId == -1)
            {
                lblError.Text = "Invoice No, Date Alrready Exixts For Vendor!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('"+ lblError.Text + "');", true);
            }
            else if (RequestId == -2)
            {
                lblError.Text = "Final Invoice Already Submitted!!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('" + lblError.Text + "');", true);
            }
        }//END_IF_InputValid
    }

    private void AddInvoiceItem(int InvoiceId)
    {
        int PaymentId = 0; string strChargeName = "", strChargeCode = "", strHSN = "", strRemark = "";
        decimal decTotalAmount = 0m, decTaxAmount = 0m, decIGSTRate = 0m, decCGSTRate = 0m, decSGSTRate = 0m,
        decIGSTAmount = 0m, decCGSTAmount = 0m, decSGSTAmount = 0m, decOtherDeduction = 0m;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];

            foreach (DataRow dr in dtCharges.Rows)
            {
                strChargeCode = dr["ChargeCode"].ToString();
                strChargeName = dr["ChargeName"].ToString();
                strHSN = dr["HSN"].ToString();
                decTaxAmount = Convert.ToDecimal(dr["TaxableValue"]);
                decCGSTRate = Convert.ToDecimal(dr["CGSTRate"]);
                decSGSTRate = Convert.ToDecimal(dr["SGSTRate"]);
                decIGSTRate = Convert.ToDecimal(dr["IGSTRate"]);

                decCGSTAmount = Convert.ToDecimal(dr["CGSTAmt"]);
                decSGSTAmount = Convert.ToDecimal(dr["SGSTAmt"]);
                decIGSTAmount = Convert.ToDecimal(dr["IGSTAmt"]);

                decTotalAmount = Convert.ToDecimal(dr["ChargeTotal"]);

                strRemark = dr["ChargeRemark"].ToString();

                int result = AccountExpense.AddInvoiceItem(InvoiceId, PaymentId, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmount, decCGSTAmount, decSGSTAmount, decOtherDeduction, strRemark, LoggedInUser.glUserId);
            }
        }
    }
    private bool ValidateInput()
    {
        bool IsValid = true;
        decimal decTotalValue = 0;

        if (txtCongisgneeName.Text.Trim() == "")
        {
            lblError.Text = "Invoice Party Name!";
            txtCongisgneeName.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }
        if (hdnConsigneeCode.Value.Trim() == "")
        {
            lblError.Text = "Invoice Party Details Not Found! Please Select Invoice Party Name/GSTIN From List";
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }

        if (txtTotalInvoiceValue.Text.Trim() == "" || txtTotalInvoiceValue.Text.Trim() == "0")
        {
            lblError.Text = "Please Enter Total Invoice Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }
        else if (decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decTotalValue) == false)
        {
            lblError.Text = "Please Enter Valid Total Invoice Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }

        if (ddInvoiceType.SelectedValue == "1") // Final Invoice
        {
            decimal decChargeTotal = 0m;

            decimal.TryParse(lblTotalValue.Text.Trim(), out decChargeTotal);

            decimal decInvoiceTotal = 0m;

            decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decInvoiceTotal);

            decimal decDiff = decChargeTotal - decInvoiceTotal;

            if (decChargeTotal == 0 || decInvoiceTotal == 0)
            {
                lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                lblError.CssClass = "errorMsg";

                IsValid = false;
                return IsValid;

            }
            else if (decDiff > 1 || decDiff < -1)
            {
                lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                lblError.CssClass = "errorMsg";

                IsValid = false;
                return IsValid;
            }

        }

        return IsValid;

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingInvoiceFinal.aspx");
    }

    #region Invoice Item

    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        decimal decTaxableAmount = 0, decGSTRate = 0, decGSTAmount = 0, decTotalAmount = 0;
        decimal decIGSTRate = 0, decCGSTRate = 0, decSGSTRate = 0;
        decimal decIGSTAmt = 0, decCGSTAmt = 0, decSGSTAmt = 0;

        decTaxableAmount = Convert.ToDecimal(txtTaxableValue.Text.Trim());

        decGSTRate = Convert.ToDecimal(ddGSTRate.SelectedValue);

        decGSTAmount = (decTaxableAmount * decGSTRate) / 100;

        decTotalAmount = decTaxableAmount + decGSTAmount;

        if (rblGSTType.SelectedValue == "1")
        {
            decIGSTRate = decGSTRate;
            decIGSTAmt = decGSTAmount;
        }
        else
        {
            decCGSTRate = decGSTRate / 2;
            decSGSTRate = decGSTRate / 2;

            decCGSTAmt = decGSTAmount / 2;
            decSGSTAmt = decGSTAmount / 2;
        }


        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        DataRow dr = dtCharges.NewRow();

        dr["ChargeCode"] = txtChargeCode.Text.Trim();
        dr["ChargeName"] = txtChargeName.Text.Trim();
        dr["HSN"] = hdnChargeHSN.Value.Trim();

        dr["TaxableValue"] = txtTaxableValue.Text.Trim();
        dr["CGSTRate"] = decCGSTRate.ToString();
        dr["SGSTRate"] = decSGSTRate.ToString();
        dr["IGSTRate"] = decIGSTRate.ToString();

        dr["CGSTAmt"] = decCGSTAmt.ToString();
        dr["SGSTAmt"] = decSGSTAmt.ToString();
        dr["IGSTAmt"] = decIGSTAmt.ToString();

        dr["ChargeTotal"] = decTotalAmount.ToString();
        dr["ChargeRemark"] = txtChargeRemark.Text.Trim();

        //OtherDeduction - Not Required On Payment Request Page
        dr["OtherDeduction"] = "0";

        dtCharges.Rows.Add(dr);
        dtCharges.AcceptChanges();

        ViewState["vwsCharges"] = dtCharges;

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        // Calculate Total Charges
        CalcuateTotalCharges();

        // Reset Charge TextBox Value

        ResetCharges();
    }
    private DataTable GetChargeCodeDataTable()
    {
        DataTable dtChargeCode = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colChargeCode = new DataColumn("ChargeCode", Type.GetType("System.String"));
        DataColumn colChargeName = new DataColumn("ChargeName", Type.GetType("System.String"));
        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colTaxableValue = new DataColumn("TaxableValue", Type.GetType("System.String"));
        DataColumn colOtherDeduction = new DataColumn("OtherDeduction", Type.GetType("System.String"));
        DataColumn colIGSTRate = new DataColumn("IGSTRate", Type.GetType("System.String"));
        DataColumn colIGSTAmt = new DataColumn("IGSTAmt", Type.GetType("System.String"));
        DataColumn colCGSTRate = new DataColumn("CGSTRate", Type.GetType("System.String"));
        DataColumn colCGSTAmt = new DataColumn("CGSTAmt", Type.GetType("System.String"));
        DataColumn colSGSTRate = new DataColumn("SGSTRate", Type.GetType("System.String"));
        DataColumn colSGSTAmt = new DataColumn("SGSTAmt", Type.GetType("System.String"));

        DataColumn colChargeTotal = new DataColumn("ChargeTotal", Type.GetType("System.String"));
        DataColumn colChargeRemark = new DataColumn("ChargeRemark", Type.GetType("System.String"));

        dtChargeCode.Columns.Add(colSL);
        dtChargeCode.Columns.Add(colChargeCode);
        dtChargeCode.Columns.Add(colChargeName);
        dtChargeCode.Columns.Add(colHSN);
        dtChargeCode.Columns.Add(colTaxableValue);
        dtChargeCode.Columns.Add(colOtherDeduction);

        dtChargeCode.Columns.Add(colCGSTRate);
        dtChargeCode.Columns.Add(colSGSTRate);
        dtChargeCode.Columns.Add(colIGSTRate);

        dtChargeCode.Columns.Add(colCGSTAmt);
        dtChargeCode.Columns.Add(colSGSTAmt);
        dtChargeCode.Columns.Add(colIGSTAmt);

        dtChargeCode.Columns.Add(colChargeTotal);
        dtChargeCode.Columns.Add(colChargeRemark);

        dtChargeCode.PrimaryKey = new DataColumn[] { colSL };
        dtChargeCode.AcceptChanges();

        return dtChargeCode;
    }
    private void CalcuateTotalCharges()
    {
        decimal decTotalTaxableValue = 0, decTotalGSTValue = 0, decTotalInvoiceValue = 0;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];

            foreach (DataRow dr in dtCharges.Rows)
            {
                if (dr["TaxableValue"].ToString() != "")
                {
                    decTotalTaxableValue = decTotalTaxableValue + Convert.ToDecimal(dr["TaxableValue"]);
                }
                if (dr["CGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["CGSTAmt"]);
                }
                if (dr["SGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["SGSTAmt"]);
                }
                if (dr["IGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["IGSTAmt"]);
                }
                if (dr["ChargeTotal"].ToString() != "")
                {
                    decTotalInvoiceValue = decTotalInvoiceValue + Convert.ToDecimal(dr["ChargeTotal"]);
                }
            }
        }

        lblTotalTaxableValue.Text = decTotalTaxableValue.ToString();
        lblTotalGST.Text = decTotalGSTValue.ToString();
        lblTotalValue.Text = decTotalInvoiceValue.ToString();
    }

    private void ResetCharges()
    {
        txtChargeCode.Text = "";
        txtChargeName.Text = "";
        hdnChargeHSN.Value = "";
        txtTaxableValue.Text = "";
        txtChargeRemark.Text = "";
        ddGSTRate.SelectedIndex = -1;

    }
    protected void lnlRemove_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string strChargeId = lnk.CommandArgument.ToString();

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        dtCharges.Rows.Find(strChargeId).Delete();

        dtCharges.AcceptChanges();

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        ViewState["vwsCharges"] = dtCharges;

        // Calculate Total Charges
        CalcuateTotalCharges();
    }
    #endregion

}