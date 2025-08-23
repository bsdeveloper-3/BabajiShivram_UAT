using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountExpense_PaymentRequestEdit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetPaymentRequest(Convert.ToInt32(Session["InvoiceId"]));
        }
    }

    private void GetPaymentRequest(int InvoiceID)
    {
        DataSet dsDetail = AccountExpense.GetInvoiceDetail(InvoiceID);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            hdnStatusId.Value = dsDetail.Tables[0].Rows[0]["lStatus"].ToString();
            lblJobNumber.Text = dsDetail.Tables[0].Rows[0]["FARefNo"].ToString();
            lblCustomer.Text = dsDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
            txtCongisgneeGSTIN.Text = dsDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
            txtCongisgneeName.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();

            lblBENo.Text = dsDetail.Tables[0].Rows[0]["BOENo"].ToString();
            lblBLNo.Text = dsDetail.Tables[0].Rows[0]["MAWBNo"].ToString();

            txtSupplierGSTIN.Text = dsDetail.Tables[0].Rows[0]["VendorGSTNo"].ToString();
            txtVendorPANNo.Text = dsDetail.Tables[0].Rows[0]["VendorPAN"].ToString();
            txtVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            hdnVendorCode.Value = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            ddGSTINType.SelectedValue = dsDetail.Tables[0].Rows[0]["VendorGSTNTypeId"].ToString();

            ddlPaymentType.SelectedValue = dsDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            txtPaymentTerms.Text = dsDetail.Tables[0].Rows[0]["PaymentTerms"].ToString();

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsRIM"]) == true)
            {
                ddRIM.SelectedValue = "1";
            }
            else
            {
                ddRIM.SelectedValue = "0";
            }

            ddInvoiceType.SelectedValue = dsDetail.Tables[0].Rows[0]["InvoiceType"].ToString();

            if(ddInvoiceType.SelectedValue =="1")
            {
                fldInvoiceItem.Visible = true;
            }
            else
            {
                fldInvoiceItem.Visible = false;
            }

            txtInvoiceNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();

            if (dsDetail.Tables[0].Rows[0]["InvoiceDate"] != DBNull.Value)
            {
                txtInvoiceDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["InvoiceDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["PaymentDueDate"] != DBNull.Value)
            {
                txtPaymentDueDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["PaymentDueDate"]).ToString("dd/MM/yyyy");
            }

            if (dsDetail.Tables[0].Rows[0]["dtDate"] != DBNull.Value)
            {
                txtPaymentRequiredDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["dtDate"]).ToString("dd/MM/yyyy");
            }

            if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsAdvanceReceived"]) == true)
            {
                rblAdvanceReceived.SelectedValue = "1";
                txtAdvanceAmount.Text = dsDetail.Tables[0].Rows[0]["AdvanceAmount"].ToString();
            }
            else
            {
                rblAdvanceReceived.SelectedValue = "0";
            }


            txtTotalInvoiceValue.Text = dsDetail.Tables[0].Rows[0]["TotalAmount"].ToString();

            lblTotalTaxableValue.Text = dsDetail.Tables[0].Rows[0]["TaxAmount"].ToString();
            lblTotalGST.Text = dsDetail.Tables[0].Rows[0]["GSTAmount"].ToString();
            lblTotalValue.Text = dsDetail.Tables[0].Rows[0]["TotalAmount"].ToString();

            // Vendor Detail

            //string strVendorCode = dsDetail.Tables[0].Rows[0]["VendorCode"].ToString();

            //DataSet dsVendor = AccountExpense.GetVedorDetail(strVendorCode);

            //if(dsVendor.Tables[0].Rows.Count>0)
            //{

            //}
        }
    }

    #region Invoice Item

    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        lblChargeError.Text = "";

        int ItemId = Convert.ToInt32(hdnItemId.Value);

        if (Session["InvoiceId"] != null)
        {
            int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

            string strChargeName = "", strChargeCode = "", strHSN = "", strRemark = "";
            decimal decTotalAmount = 0m, decTaxAmount = 0m, decIGSTRate = 0m, decCGSTRate = 0m, decSGSTRate = 0m,
            decIGSTAmount = 0m, decCGSTAmount = 0m, decSGSTAmount = 0m, decOtherDeduction = 0m;

            decimal decTaxableAmount = 0, decGSTRate = 0, decGSTAmount = 0;
            decimal decIGSTAmt = 0, decCGSTAmt = 0, decSGSTAmt = 0;

            if (txtOtherDeduction.Text.Trim() != "")
            {
                decOtherDeduction = Convert.ToDecimal(txtOtherDeduction.Text.Trim());
            }

            decTaxableAmount = Convert.ToDecimal(txtTaxableValue.Text.Trim());

            decGSTRate = Convert.ToDecimal(ddGSTRate.SelectedValue);

            // Deduct OtherDeduction from Taxable valeu and Calculate GST Amount
            decGSTAmount = ((decTaxableAmount - decOtherDeduction) * decGSTRate) / 100;

            decTotalAmount = (decTaxableAmount - decOtherDeduction) + decGSTAmount;

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

            strChargeName = txtChargeName.Text.Trim();
            strChargeCode = txtChargeCode.Text.Trim();
            strHSN = hdnChargeHSN.Value.Trim();
            strRemark = txtChargeRemark.Text.Trim();

            if (ItemId == 0) // Add New Charge
            {
                int resultAdd = AccountExpense.AddInvoiceItem(InvoiceId, 0, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxableAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmt, decCGSTAmt, decSGSTAmt, decOtherDeduction, strRemark, LoggedInUser.glUserId);

                if (resultAdd == 0)
                {
                    lblChargeError.Text = "Invoice Detail Added!";
                    lblChargeError.CssClass = "errorMsg";
                    gvCharges.DataBind();
                    ResetCharges();
                }
                else if (resultAdd == 2)
                {
                    lblChargeError.Text = "Charge Detail Already Exist!";
                    lblChargeError.CssClass = "errorMsg";
                }
                else
                {
                    lblChargeError.Text = "System Error! Please try after sometime";
                    lblChargeError.CssClass = "errorMsg";
                }
            }
            else if (ItemId >= 0) // Update Charge
            {
                int resultUpd = AccountExpense.UpdateInvoiceItem(ItemId, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxableAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmt, decCGSTAmt, decSGSTAmt, decOtherDeduction, strRemark, LoggedInUser.glUserId);

                if (resultUpd == 0)
                {
                    lblChargeError.Text = "Invoice Detail Updated!";
                    lblChargeError.CssClass = "errorMsg";
                    gvCharges.DataBind();
                    ResetCharges();
                }
                else
                {
                    lblChargeError.Text = "System Error! Please try after sometime";
                    lblChargeError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblChargeError.Text = "Login Session Expired! Please Login Again.";
            lblChargeError.CssClass = "errorMsg";

        }

        ModalPopupExtender1.Show();
    }
    protected void btnNewCharge_Click(object sender, EventArgs e)
    {
        btnAddCharges.Text = "Add Charges";
        hdnItemId.Value = "0";

        ModalPopupExtender1.Show();
    }
    protected void lnkEditCharge_Click(object sender, EventArgs e)
    {
        btnAddCharges.Text = "Update Charges";

        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        DataSet dsItem = AccountExpense.GetInvoiceItemById(ChargeId);

        if (dsItem.Tables[0].Rows.Count > 0)
        {
            hdnItemId.Value = dsItem.Tables[0].Rows[0]["lid"].ToString();
            txtChargeCode.Text = dsItem.Tables[0].Rows[0]["ChargeCode"].ToString();
            txtChargeName.Text = dsItem.Tables[0].Rows[0]["ChargeName"].ToString();
            hdnChargeHSN.Value = dsItem.Tables[0].Rows[0]["HSN"].ToString();

            txtTaxableValue.Text = dsItem.Tables[0].Rows[0]["TaxAmount"].ToString();
            txtOtherDeduction.Text = dsItem.Tables[0].Rows[0]["OtherDeduction"].ToString();
            txtChargeRemark.Text = dsItem.Tables[0].Rows[0]["Remark"].ToString();

            decimal decIGSTAmount = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["IGSTAmount"]);
            decimal decIGSTRate = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["IGSTRate"]);
            decimal decCGSTRate = Convert.ToDecimal(dsItem.Tables[0].Rows[0]["CGSTRate"]);

            if (decIGSTAmount > 0)
            {
                rblGSTType.SelectedValue = "1";

                ddGSTRate.SelectedValue = decIGSTRate.ToString();
            }
            else if (decCGSTRate > 0)
            {
                rblGSTType.SelectedValue = "0";

                ddGSTRate.SelectedValue = (decCGSTRate * 2).ToString();
            }
            else
            {
                // GST Not Applicable

                rblGSTType.SelectedValue = "1";
                ddGSTRate.SelectedValue = "0";
            }


            ModalPopupExtender1.Show();
        }

    }
    protected void lnlRemoveCharge_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ChargeId = Convert.ToInt32(lnk.CommandArgument.ToString());

        int result = AccountExpense.DeleteInvoiceItem(ChargeId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Charge Details Removed Successfully!";
            lblError.CssClass = "success";
            gvCharges.DataBind();
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }
    private void ResetCharges()
    {
        txtChargeCode.Text = "";
        txtChargeName.Text = "";
        hdnChargeHSN.Value = "";
        txtTaxableValue.Text = "";
        txtChargeRemark.Text = "";
        txtOtherDeduction.Text = "";
        ddGSTRate.SelectedIndex = -1;
        hdnItemId.Value = "0";

    }

    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int InvoiceId = Convert.ToInt32(Session["InvoiceId"]);

        int StatusId = Convert.ToInt32(hdnStatusId.Value);
        int NewStatusid = 100;

        if(StatusId == 112)
        {
            // Send For Mgmt Approval

            NewStatusid = 100;
        }
        else if (StatusId == 122)
        {
            // Send To Account Department
            NewStatusid = 120;
        }
        
         int result =  AccountExpense.AddInvoiceStatus(InvoiceId, NewStatusid, txtRemark.Text.Trim(), LoggedInUser.glUserId);
        
        if(result == 0)
        {
            btnSubmit.Visible = false;
            lblError.Text = "Invoice Details Updated!";
            lblError.CssClass = "success";

        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvoiceRejected.aspx");
    }
}