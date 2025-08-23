using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class FreightOperation_ArrivalNotice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkCreateCANPdf);
        ScriptManager1.RegisterPostBackControl(btnUpdate);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingArrival.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Cargo Arrival Notice";

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }

        MskIGMHBLDate.MaximumValue = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
        MskValidATA.MaximumValue = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
    }

    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {

            lblJobNo.Text = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();

            lblBookingMonth.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");
            txtIGMNo.Text = dsBookingDetail.Tables[0].Rows[0]["IGMNo"].ToString();
            txtItemNo.Text = dsBookingDetail.Tables[0].Rows[0]["ItemNo"].ToString();
            txtRemark.Text = dsBookingDetail.Tables[0].Rows[0]["CANRemark"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                txtIGMDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["IGMDate"]).ToString("dd/MM/yyyy");

            if (dsBookingDetail.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                txtATA.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ATADate"]).ToString("dd/MM/yyyy");

            hdnWeight.Value = dsBookingDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            hdnVolume.Value = dsBookingDetail.Tables[0].Rows[0]["LCLVolume"].ToString();
            hdnUploadPath.Value = dsBookingDetail.Tables[0].Rows[0]["DocDir"].ToString();

            lblGrossWT.Text = dsBookingDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
            lblChargeWT.Text = dsBookingDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["IsGST"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsBookingDetail.Tables[0].Rows[0]["IsGST"]) == true)
                {
                    hdnIsGST.Value = "1";
                }
                else
                {
                    txtTaxRate.Text = "15.00";
                    hdnIsGST.Value = "0";
                }
            }

            lblState.Text = dsBookingDetail.Tables[0].Rows[0]["StateName"].ToString();
            lblGSTN.Text = dsBookingDetail.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();

            string strStateName = dsBookingDetail.Tables[0].Rows[0]["StateName"].ToString();
            string strStateCode = dsBookingDetail.Tables[0].Rows[0]["StateCode"].ToString();
            if (dsBookingDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
                hdnCountryCode.Value = dsBookingDetail.Tables[0].Rows[0]["CountryCode"].ToString();
            string CountryCode = dsBookingDetail.Tables[0].Rows[0]["CountryCode"].ToString();


            if (hdnIsGST.Value == "1")
            {
                if (CountryCode.ToLower().Trim() == "india")
                {
                    if (strStateCode == "")
                    {
                        lblGSTMessage.Text = "Place of Supply Detail Not Found ! Please Update Cargo Place of Supply.";
                    }
                    else if (strStateCode.ToLower() == "mh")
                    {
                        hdnIsStateGST.Value = "1"; // CGST/ SGST Rate (Half Rate) will be applicable
                        lblGSTMessage.Text = "Place of Supply is - " + strStateName + ". CGST and SGST Applicable.";
                    }
                    else
                    {
                        hdnIsStateGST.Value = "0"; // IGST Rate (Full Rate) will be applicable
                        lblGSTMessage.Text = "Place of Supply is - " + strStateName + ". IGST Applicable.";
                    }
                }
                else
                {
                    lblGSTMessage.Text = "";
                }
            }

            if (dsBookingDetail.Tables[0].Rows[0]["lMode"].ToString() == "1") // AIR
            {
                pnlAir.Visible = true;
                pnlSea.Visible = false;

                lblnoOfPkgs.Text = dsBookingDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
                hdnModeId.Value = "1";
            }
            else
            {
                pnlSea.Visible = true;
                pnlAir.Visible = false;

                lblCon20.Text = dsBookingDetail.Tables[0].Rows[0]["CountOf20"].ToString();
                lblCon40.Text = dsBookingDetail.Tables[0].Rows[0]["CountOf40"].ToString();
                lblVolume.Text = dsBookingDetail.Tables[0].Rows[0]["LCLVolume"].ToString();

                lblContainerTypeName.Text = dsBookingDetail.Tables[0].Rows[0]["ContainerTypeName"].ToString();
                hdnContainerTypeId.Value = dsBookingDetail.Tables[0].Rows[0]["ContainerType"].ToString();
                hdnModeId.Value = "2";
            }

        }
        else
        {
            lblError.Text = "Job Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strIGMNo = "", strItemNo = "", strDocName = "", strRemark = "";
        DateTime dtIGMDate = DateTime.MinValue;
        DateTime dtATADate = DateTime.MinValue;

        strIGMNo = txtIGMNo.Text.Trim();
        strItemNo = txtItemNo.Text.Trim();
        strRemark = txtRemark.Text.Trim();
        strDocName = "CAN Copy";

        if (txtIGMDate.Text.Trim() != "")
        {
            dtIGMDate = Commonfunctions.CDateTime(txtIGMDate.Text.Trim());
        }
        if (txtIGMDate.Text.Trim() != "")
        {
            dtATADate = Commonfunctions.CDateTime(txtATA.Text.Trim());
        }

        if (fuCANCopy.HasFile) // Upload CAN Copy
        {
            string strDocFolder = "FreightDoc\\" + hdnUploadPath.Value + "\\";

            string strFilePath = UploadDocument(strDocFolder);
            if (strFilePath != "")
            {
                int DocResult = DBOperations.AddFreightDocument(EnqId, strDocName, strFilePath, LoggedInUser.glUserId);

                if (DocResult == 0)
                {
                    //lblError.Text = "Document uploaded successfully.";
                    //lblError.CssClass = "success";
                }
                else if (DocResult == 1)
                {
                    //lblError.Text = "System Error! Please try after sometime.";
                    //lblError.CssClass = "errorMsg";
                }
            }
        }

        int result = DBOperations.AddCargoArrival(EnqId, strIGMNo, dtIGMDate, dtATADate, strItemNo, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "CAN Detail Updated Successfully!";
            lblError.CssClass = "success";

            if (fuCANCopy.HasFile) // Upload CAN Copy
            {
                btnUpdate.Visible = false;

                lblError.Text = "CAN Copy Uploaded and Job Moved To DO!";
            }
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "CAN Detail Already Exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingArrival.aspx");
    }

    private bool CalculateGST()
    {
        bool IsCalculated = false, IsPercentField = false;
        int UnitOfMeasure = 0;
        decimal decAmount = 0, decRate = 0, decExchangeRate = 0, decTaxAmount = 0, PercentFieldAmount = 0,
             MinUnit = 0, MinAmount = 0, ChargeableWeight = 1, Volume = 1, decGSTRate = 0.00m;

        int FieldId = Convert.ToInt32(ddInvoice.SelectedValue);
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        string strRemark = ""; string strGSTRate = "";

        txtTaxRate.Text = "";
        lblRate.Text = "Rate"; // Default Text
        txtUSDRate.Visible = false;

        DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(FieldId);

        if (dsFieldDetail.Tables[0].Rows.Count > 0)
        {
            UnitOfMeasure = Convert.ToInt32(dsFieldDetail.Tables[0].Rows[0]["UoMid"]);
            lblUOM.Text = dsFieldDetail.Tables[0].Rows[0]["UnitOfMeasurement"].ToString();
            hdnIsTaxRequired.Value = dsFieldDetail.Tables[0].Rows[0]["IsTaxable"].ToString();

            if (dsFieldDetail.Tables[0].Rows[0]["TaxRate"] != DBNull.Value)
            {
                strGSTRate = dsFieldDetail.Tables[0].Rows[0]["TaxRate"].ToString();
            }
            else
            {
                // GST Rate Not Update
                lblError.Text = "Please Update GST Tax % for Invoice Item -  " + ddInvoice.SelectedItem.Text;
                lblInvoiceError.Text = "Please Update GST Tax % for Invoice Item -" + ddInvoice.SelectedItem.Text;

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
                return false;

            }

            decGSTRate = Convert.ToDecimal(strGSTRate);

            txtTaxRate.Text = decGSTRate.ToString();

            if (decGSTRate == 0)
            {
                hdnIsTaxRequired.Value = "false"; // Tax Not Required
            }
            else
            {
                // tax applicable or not based on HSN Code

                int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
                DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
                if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
                {
                    int lMode = 0;
                    if (hdnModeId.Value != "")
                        lMode = Convert.ToInt32(hdnModeId.Value);

                    if (lMode == 1)  //Air
                    {
                        if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] == DBNull.Value)
                        {
                            hdnIsTaxRequired.Value = "false";
                        }
                    }
                    else  //Sea
                    {
                        if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] == DBNull.Value)
                        {
                            hdnIsTaxRequired.Value = "false";
                        }
                    }
                }
            }

            if (dsFieldDetail.Tables[0].Rows[0]["Remark"] != DBNull.Value)
                strRemark = dsFieldDetail.Tables[0].Rows[0]["Remark"].ToString();

            lnkDataTooltip.Attributes.Add("data-tooltip", strRemark);

            if (txtMinUnit.Text.Trim() != "")
                MinUnit = Convert.ToDecimal(txtMinUnit.Text.Trim());

            if (txtMinAmount.Text.Trim() != "")
                MinAmount = Convert.ToDecimal(txtMinAmount.Text.Trim());

            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
            {
                ChargeableWeight = Convert.ToDecimal(hdnWeight.Value);

                if (ChargeableWeight == 0)
                {
                    lblError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";
                    lblInvoiceError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }
            if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
            {
                Volume = Convert.ToDecimal(hdnVolume.Value);

                if (Volume == 0)
                {
                    lblError.Text = "Please check CBM Value! " + hdnVolume.Value;
                    lblInvoiceError.Text = "Please check CBM Value! " + hdnVolume.Value;

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }
            else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
            {
                ddCurrency.SelectedValue = "46"; // Indian Rupee
                ddCurrency.Enabled = false;
                txtExchangeRate.Text = "1";
                txtExchangeRate.Enabled = false;

                IsPercentField = true;
                txtUSDRate.Visible = false;

                lblRate.Text = "Rate (%)  ";

                PercentFieldAmount = CalculatePercentField();

                if (PercentFieldAmount > 0)
                {
                    //if (txtUSDRate.Text.Trim() != "")
                    //{
                    //    try
                    //    {
                    //        PercentFieldExchangeRate = 1; // Indian Rupee //Convert.ToDecimal(txtUSDRate.Text.Trim());
                    //    }
                    //    catch
                    //    {
                    //        lblError.Text = "Please enter valid amount for USD Exchange Rate!";
                    //        lblError.CssClass = "errorMsg";
                    //    }
                    //}
                }
                else
                {
                    // Percent field detail not found

                    lblError.Text = "Please Select Invoice Percent Item: ";
                    lblInvoiceError.Text = "Please Select Invoice Percent Item: ";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }//END_IF_PercentField


            if (Convert.ToBoolean(hdnIsTaxRequired.Value) == false)
            {
                lblTaxAmount.Text = "0";
                lblTaxName.Text = "Tax Amount (N.A.)";
            }
            else
            {
                // lblTaxName.Text = "Tax Amount (" + txtTaxRate.Text.Trim() + " %) Rs";
                lblTaxName.Text = "Tax Amount (Rs)";
            }
        }// END_IF_RowCount
        else
        {
            lblError.Text = "Invoice Item Details Not Found!";
            lblInvoiceError.Text = "Invoice Item Details Not Found!";

            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";

            lblTaxAmount.Text = "";

            return false;
        }

        // Calculate Amount
        if (ddCurrency.SelectedIndex > 0 && txtRate.Text.Trim() != "" && txtExchangeRate.Text.Trim() != "")
        {
            lblInvoiceError.Text = "@@@" + hdnIsGST.Value;
            try
            {
                decRate = Convert.ToDecimal(txtRate.Text.Trim());
                decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

                decExchangeRate = System.Math.Round(decExchangeRate, 2, MidpointRounding.AwayFromZero);

                MinAmount = (MinAmount * decExchangeRate);
                MinUnit = (MinUnit * decRate * decExchangeRate);

                if (UnitOfMeasure == (Int32)EnumUnit.perKG)
                {
                    decRate = (decRate * ChargeableWeight);
                }
                else if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
                {
                    decRate = (decRate * Volume);
                }

                if (decRate == 0 || decExchangeRate == 0)
                {
                    lblError.Text = "Please Enter Valid Invoice Value!";
                    lblInvoiceError.Text = "Please Enter Valid Invoice Value!";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";

                    lblTaxAmount.Text = "";

                    return false;

                }

                if (Convert.ToBoolean(hdnIsTaxRequired.Value) == true)
                {
                    if (IsPercentField == true)
                    {
                        decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

                        //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

                        if (decAmount < MinAmount)
                        {
                            decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
                        }
                    }
                    else
                    {
                        decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

                        var list = new[] { decAmount, MinUnit, MinAmount };
                        decAmount = list.Max();
                    }

                    decTaxAmount = System.Math.Round(((decAmount * decGSTRate) / 100.00m), 2, MidpointRounding.AwayFromZero);
                    lblTaxAmount.Text = decTaxAmount.ToString();

                    lblAmount.Text = decAmount.ToString();
                    lblTotalAmount.Text = (decAmount + decTaxAmount).ToString();

                    IsCalculated = true;
                }
                else
                {
                    if (IsPercentField == true)
                    {
                        decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

                        //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

                        if (decAmount < MinAmount)
                        {
                            decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
                        }
                    }
                    else
                    {
                        decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

                        var list = new[] { decAmount, MinUnit, MinAmount };
                        decAmount = list.Max();
                    }

                    lblAmount.Text = decAmount.ToString();
                    lblTotalAmount.Text = decAmount.ToString();

                    IsCalculated = true;
                }
            }//END_Catch
            catch (Exception ex)
            {
                lblError.Text = "Please Enter Valid Amount Details";
                lblInvoiceError.Text = "Please Enter Valid Amount Details";
                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";

                lblTaxAmount.Text = "";
                lblAmount.Text = "";
                lblTotalAmount.Text = "";

                IsCalculated = false;
            }
        }// END_IF
        else
        {
            lblTaxAmount.Text = "";
            lblAmount.Text = "";
            lblTotalAmount.Text = "";
            IsCalculated = false;
        }

        return IsCalculated;
    }

    private bool CalculateServiceTax()
    {
        // Service Tax Rate = "15.00";

        bool IsCalculated = false, IsPercentField = false;
        int UnitOfMeasure = 0;
        decimal decAmount = 0, decRate = 0, decExchangeRate = 0, decTaxAmount = 0, PercentFieldAmount = 0,
            PercentFieldExchangeRate = 0, MinimumPercentCharge = 0, MinUnit = 0, MinAmount = 0,
            ChargeableWeight = 1, Volume = 1, decServiceTax = 0.00m;

        try
        {
            if (txtTaxRate.Text.Trim() != "")
                decServiceTax = Convert.ToDecimal(txtTaxRate.Text.Trim());
        }
        catch (Exception e)
        {
            lblInvoiceError.Text = "Invalid Service Tax Rate!  Please Enter Numeric Value.";
            lblInvoiceError.CssClass = "errorMsg";
            return false;
        }

        int FieldId = Convert.ToInt32(ddInvoice.SelectedValue);
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        string strRemark = "";

        lblRate.Text = "Rate"; // Default Text
        txtUSDRate.Visible = false;

        DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(FieldId);

        if (dsFieldDetail.Tables[0].Rows.Count > 0)
        {
            UnitOfMeasure = Convert.ToInt32(dsFieldDetail.Tables[0].Rows[0]["UoMid"]);
            lblUOM.Text = dsFieldDetail.Tables[0].Rows[0]["UnitOfMeasurement"].ToString();
            hdnIsTaxRequired.Value = dsFieldDetail.Tables[0].Rows[0]["IsTaxable"].ToString();

            if (decServiceTax == 0)
            {
                hdnIsTaxRequired.Value = "false"; // Tax Not Required
            }

            if (dsFieldDetail.Tables[0].Rows[0]["Remark"] != DBNull.Value)
                strRemark = dsFieldDetail.Tables[0].Rows[0]["Remark"].ToString();

            lnkDataTooltip.Attributes.Add("data-tooltip", strRemark);

            if (txtMinUnit.Text.Trim() != "")
                MinUnit = Convert.ToDecimal(txtMinUnit.Text.Trim());

            if (txtMinAmount.Text.Trim() != "")
                MinAmount = Convert.ToDecimal(txtMinAmount.Text.Trim());

            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
            {
                ChargeableWeight = Convert.ToDecimal(hdnWeight.Value);

                if (ChargeableWeight == 0)
                {
                    lblError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";
                    lblInvoiceError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }
            if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
            {
                Volume = Convert.ToDecimal(hdnVolume.Value);

                if (Volume == 0)
                {
                    lblError.Text = "Please check CBM Value! " + hdnVolume.Value;
                    lblInvoiceError.Text = "Please check CBM Value! " + hdnVolume.Value;

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }
            else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
            {
                ddCurrency.SelectedValue = "46"; // Indian Rupee
                ddCurrency.Enabled = false;
                txtExchangeRate.Text = "1";
                txtExchangeRate.Enabled = false;

                IsPercentField = true;
                txtUSDRate.Visible = false;

                lblRate.Text = "Rate (%)  ";

                PercentFieldAmount = CalculatePercentField();

                if (PercentFieldAmount > 0)
                {
                    //if (txtUSDRate.Text.Trim() != "")
                    //{
                    //    try
                    //    {
                    //        PercentFieldExchangeRate = 1; // Indian Rupee //Convert.ToDecimal(txtUSDRate.Text.Trim());
                    //    }
                    //    catch
                    //    {
                    //        lblError.Text = "Please enter valid amount for USD Exchange Rate!";
                    //        lblError.CssClass = "errorMsg";
                    //    }
                    //}
                }
                else
                {
                    // Percent field detail not found

                    lblError.Text = "Please Select Invoice Percent Item: ";
                    lblInvoiceError.Text = "Please Select Invoice Percent Item: ";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return false;
                }
            }//END_IF_PercentField


            if (Convert.ToBoolean(hdnIsTaxRequired.Value) == false)
            {
                lblTaxAmount.Text = "0";
                lblTaxName.Text = "Tax Amount (N.A.)";
            }
            else
            {
                //lblTaxName.Text = "Tax Amount (" + txtTaxRate.Text.Trim() + " %) Rs";
                lblTaxName.Text = "Tax Amount (Rs)";
            }
        }// END_IF_RowCount
        else
        {
            lblError.Text = "Invoice Item Details Not Found!";
            lblInvoiceError.Text = "Invoice Item Details Not Found!";

            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";

            lblTaxAmount.Text = "";

            return false;
        }

        // Calculate Amount
        if (ddCurrency.SelectedIndex > 0 && txtRate.Text.Trim() != "" && txtExchangeRate.Text.Trim() != "")
        {
            try
            {
                decRate = Convert.ToDecimal(txtRate.Text.Trim());
                decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

                decExchangeRate = System.Math.Round(decExchangeRate, 2, MidpointRounding.AwayFromZero);

                MinAmount = (MinAmount * decExchangeRate);
                MinUnit = (MinUnit * decRate * decExchangeRate);

                if (UnitOfMeasure == (Int32)EnumUnit.perKG)
                {
                    decRate = (decRate * ChargeableWeight);
                }
                else if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
                {
                    decRate = (decRate * Volume);
                }

                if (decRate == 0 || decExchangeRate == 0)
                {
                    lblError.Text = "Please Enter Valid Invoice Value!";
                    lblInvoiceError.Text = "Please Enter Valid Invoice Value!";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";

                    lblTaxAmount.Text = "";

                    return false;

                }

                if (Convert.ToBoolean(hdnIsTaxRequired.Value) == true)
                {
                    if (IsPercentField == true)
                    {
                        decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

                        //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

                        if (decAmount < MinAmount)
                        {
                            decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
                        }
                    }
                    else
                    {
                        decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

                        var list = new[] { decAmount, MinUnit, MinAmount };
                        decAmount = list.Max();
                    }

                    decTaxAmount = System.Math.Round(((decAmount * decServiceTax) / 100.00m), 2, MidpointRounding.AwayFromZero);
                    lblTaxAmount.Text = decTaxAmount.ToString();

                    lblAmount.Text = decAmount.ToString();
                    lblTotalAmount.Text = (decAmount + decTaxAmount).ToString();

                    IsCalculated = true;
                }
                else
                {
                    if (IsPercentField == true)
                    {
                        decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

                        //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

                        if (decAmount < MinAmount)
                        {
                            decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
                        }
                    }
                    else
                    {
                        decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

                        var list = new[] { decAmount, MinUnit, MinAmount };
                        decAmount = list.Max();
                    }

                    lblAmount.Text = decAmount.ToString();
                    lblTotalAmount.Text = decAmount.ToString();

                    IsCalculated = true;
                }
            }//END_Catch
            catch (Exception ex)
            {
                lblError.Text = "Please Enter Valid Amount Details";
                lblInvoiceError.Text = "Please Enter Valid Amount Details";
                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";

                lblTaxAmount.Text = "";
                lblAmount.Text = "";
                lblTotalAmount.Text = "";

                IsCalculated = false;
            }
        }// END_IF
        else
        {
            lblTaxAmount.Text = "";
            lblAmount.Text = "";
            lblTotalAmount.Text = "";
            IsCalculated = false;
        }

        return IsCalculated;
    }

    private decimal CalculatePercentField()
    {
        decimal decPercentAmount = 0;

        foreach (GridViewRow row in gvCanInvoice.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

            if (chkSelect != null)
            {
                if (chkSelect.Checked)
                {
                    Label lblTotal = (Label)row.FindControl("lblAmount");  //Without Tax

                    //Label lblTotal = (Label)row.FindControl("lblTotal");  With Tax

                    decPercentAmount += Convert.ToDecimal(lblTotal.Text);
                }
            }
        }

        return decPercentAmount;
    }

    protected void ddInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Control ctlSender = (Control) sender;

        //if (ctlSender.ID.ToLower() == "ddinvoice")
        //    txtRate.Focus();
        //else if (ctlSender.ID.ToLower() == "txtrate")
        //    ddCurrency.Focus();
        //else if (ctlSender.ID.ToLower() == "ddcurrency")
        //    txtExchangeRate.Focus();

        ddCurrency.Enabled = true;
        txtExchangeRate.Enabled = true;

        if (hdnIsGST.Value == "1")
        {
            CalculateGST();
        }
        else
        {
            CalculateServiceTax();
        }
    }

    protected void gvCanInvoice_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "remove")
        {
            int lId = Convert.ToInt32(e.CommandArgument.ToString());
            int Result = -123;

            Result = DBOperations.DeleteCANInvoiceDetail(lId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Invoice Detail Deleted Successfully!";
                lblInvoiceError.Text = "Invoice Detail Deleted Successfully!";

                lblError.CssClass = "success";
                lblInvoiceError.CssClass = "success";
                gvCanInvoice.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblInvoiceError.Text = "System Error! Please try after sometime.";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Invoice Details Not Found!";
                lblInvoiceError.Text = "Invoice Details Not Found!";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
            }
        }
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (hdnIsGST.Value == "1")
        {
            CalculateGST();

        }
        else
        {
            CalculateServiceTax();
        }
    }

    protected void btnSaveInvoice_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblInvoiceError.Text = "";
        int count = 0;
        bool IsCAN = false;
        decimal CGstTax = 0, CGstTaxAmt = 0, SGstTax = 0, SGstTaxAmt = 0, IGstTax = 0, IGstTaxAmt = 0, Amount = 0, TaxRate = 0;
        int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
        if (lblAmount.Text.Trim() != "")
            Amount = Convert.ToDecimal(lblAmount.Text.Trim());

        // Start - Calculate GST Rate
        //lblInvoiceError.Text = "@@@" + hdnCountryCode.Value;
        if (hdnCountryCode.Value.Trim().ToLower() == "india")
        {
            
            if (lblGSTN.Text.Trim() == "")
            {
                lblError.Text = "Please Update GST No for Enquiry..!!";
                lblInvoiceError.Text = "Please Update GST No for Enquiry..!!";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
                count = 1;
            }
            else
            {
                // Start - Calculate GST Rate
                string GSTNo = lblGSTN.Text.Trim();
                int StateCode = Convert.ToInt32(GSTNo.Substring(0, 2));
                
                if (StateCode == 27) // if Maharashtra (e.g.: MH --> MH)
                {
                    
                    DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
                    if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
                    {
                        int lMode = 0;
                        if (hdnModeId.Value != "")
                            lMode = Convert.ToInt32(hdnModeId.Value);

                        if (lMode == 1)  //Air
                        {
                            if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] != DBNull.Value)
                                TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        }
                        else  //Sea
                        {
                            if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] != DBNull.Value)
                                TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        }

                        TaxRate = TaxRate / 2;
                        CGstTax = TaxRate;
                        SGstTax = TaxRate;
                        IGstTax = 0;

                        CGstTaxAmt = Convert.ToDecimal(Amount * (CGstTax / 100));
                        SGstTaxAmt = Convert.ToDecimal(Amount * (SGstTax / 100));
                        IGstTaxAmt = 0;
                    }
                }
                else
                {
                    
                    DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
                    if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
                    {
                        int lMode = 0;
                        if (hdnModeId.Value != "")
                            lMode = Convert.ToInt32(hdnModeId.Value);

                        if (lMode == 1)  //Air
                        {
                            if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] != DBNull.Value)
                                TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        }
                        else  //Sea
                        {
                            if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] != DBNull.Value)
                                TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        }

                        //TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        CGstTax = 0;
                        SGstTax = 0;
                        IGstTax = TaxRate;

                        CGstTaxAmt = 0;
                        SGstTaxAmt = 0;
                        IGstTaxAmt = Convert.ToDecimal(Amount * (IGstTax / 100));
                    }
                }

                // End - Calculate GST Rate
            }
        }

        bool isCalculated = false;

        if (hdnIsGST.Value == "1")
        {
            isCalculated = CalculateGST();
        }
        else
        {
            isCalculated = CalculateServiceTax();
        }

        int Result = -123;

        //int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
        int CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
        int UnitOfmeasureId = 0;
        bool isTaxable = false;
        bool IsPercentField = false;

        if (hdnIsTaxRequired.Value != "")
        {
            isTaxable = Convert.ToBoolean(hdnIsTaxRequired.Value);
        }

        DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(InvoiceItemId);

        if (dsFieldDetail.Tables[0].Rows.Count > 0)
        {
            UnitOfmeasureId = Convert.ToInt32(dsFieldDetail.Tables[0].Rows[0]["UoMid"]);

            if (UnitOfmeasureId == (Int32)EnumUnit.PercentOf)
            {
                IsPercentField = true;
            }
        }
        if (isCalculated == true)
        {
            string strRate = "0", strExchangeRate = "1", strServiceTax = "0.00";
            string strMinUnit = "0", strMinAmount = "0";
            string strTaxAmount = "0", strAmount = "0", strTotalAmount = "0";
            int EnqId = Convert.ToInt32(Session["EnqId"]);

            if (isTaxable == false)
                strServiceTax = "0.0";
            else
                strServiceTax = txtTaxRate.Text.Trim();

            if (txtMinUnit.Text.Trim() != "")
                strMinUnit = txtMinUnit.Text.Trim();

            if (txtMinAmount.Text.Trim() != "")
                strMinAmount = txtMinAmount.Text.Trim();

            strRate = txtRate.Text.Trim();
            strExchangeRate = txtExchangeRate.Text.Trim();
            strTaxAmount = lblTaxAmount.Text.Trim();

            strAmount = lblAmount.Text.Trim();
            strTotalAmount = lblTotalAmount.Text.Trim();

            // Total = Amount + CGST (Rs) + SGST (Rs) + IGST (Rs)
            strTotalAmount = Convert.ToDecimal(Amount + CGstTaxAmt + SGstTaxAmt + IGstTaxAmt).ToString();

            if (rblIsCAN.SelectedValue == "1")
            {
                IsCAN = true;
            }

            if (strAmount != "" && strTotalAmount != "")
            {
                Result = DBOperations.AddCANInvoiceDetail(EnqId, InvoiceItemId, UnitOfmeasureId, strRate, CurrencyId, strExchangeRate, strMinUnit, strMinAmount,
                          isTaxable, strTaxAmount, strAmount, strTotalAmount, strServiceTax, LoggedInUser.glUserId, CGstTax, CGstTaxAmt, SGstTax, SGstTaxAmt,
                          IGstTax, IGstTaxAmt, IsCAN);
                
                if (Result == 0)
                {
                    lblError.Text = "Invoice Details Added Successfully";
                    lblInvoiceError.Text += "Invoice Details Added Successfully";

                    lblError.CssClass = "success";
                    lblInvoiceError.CssClass = "success";

                    // Add Percent Invoice Field Details

                    if (IsPercentField == true)
                    {
                        string strPercentAmount = "0";
                        int PercentFieldId = 0;

                        foreach (GridViewRow row in gvCanInvoice.Rows)
                        {
                            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                            PercentFieldId = Convert.ToInt32(gvCanInvoice.DataKeys[row.RowIndex].Value);

                            if (chkSelect != null)
                            {
                                if (chkSelect.Checked)
                                {
                                    Label lblTotal = (Label)row.FindControl("lblTotal");

                                    strPercentAmount = lblTotal.Text;

                                    DBOperations.AddCANPercentDetail(EnqId, InvoiceItemId, PercentFieldId, strPercentAmount, LoggedInUser.glUserId);
                                }
                            }//END_IF

                        }//END_ForEach 
                    }

                    ddCurrency.SelectedIndex = 0;
                    ddInvoice.SelectedIndex = 0;
                    txtExchangeRate.Text = "";
                    txtRate.Text = "";
                    lblTaxAmount.Text = "";
                    lblAmount.Text = "";
                    lblTotalAmount.Text = "";

                    gvCanInvoice.DataBind();
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime";
                    lblInvoiceError.Text = "System Error! Please try after sometime";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "Invoice Item details already added.";
                    lblInvoiceError.Text = "Invoice Item details already added.";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";

                    gvCanInvoice.DataBind();
                }
                else
                {
                    lblError.Text = "System Error! Please try after sometime";
                    lblInvoiceError.Text = "System Error! Please try after sometime";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Enter Invoice Details";
                lblInvoiceError.Text = "Please Enter The Invoice Details";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Enter Invoice Details";
            lblInvoiceError.Text = "Please Enter Invoice Details";

            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";
        }
    }

    protected void lnkCreateCANPdf_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        // if (hdnIsGST.Value == "1")
        // {
                GenerateGSTPdf(EnqId);
        //}
        // else
        // {
        //     GenerateCANPdf(EnqId);
        // }
    }

    private void GenerateCANPdf(int EnqId)
    {
        DataSet dsCANPrint = DBOperations.GetOperationDetail(EnqId);

        if (dsCANPrint.Tables[0].Rows.Count > 0)
        {
            int FreightMode = Convert.ToInt32(dsCANPrint.Tables[0].Rows[0]["lMode"]);

            string FRJobNo = dsCANPrint.Tables[0].Rows[0]["FRJobNo"].ToString();
            string Customer = dsCANPrint.Tables[0].Rows[0]["Customer"].ToString();
            string Consignee = dsCANPrint.Tables[0].Rows[0]["Consignee"].ToString();
            string Shipper = dsCANPrint.Tables[0].Rows[0]["Shipper"].ToString();
            string ConsigneeAddress = dsCANPrint.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
            string ShipperAddress = dsCANPrint.Tables[0].Rows[0]["ShipperAddress"].ToString();

            string MBLNo = dsCANPrint.Tables[0].Rows[0]["MBLNo"].ToString();
            string HBLNo = dsCANPrint.Tables[0].Rows[0]["HBLNo"].ToString();
            string InvoiceNo = dsCANPrint.Tables[0].Rows[0]["InvoiceNo"].ToString();
            string PONumber = dsCANPrint.Tables[0].Rows[0]["PONumber"].ToString();
            string VesselName = dsCANPrint.Tables[0].Rows[0]["VesselName"].ToString();
            string VesselNo = dsCANPrint.Tables[0].Rows[0]["VesselNumber"].ToString();
            string LoadingPort = dsCANPrint.Tables[0].Rows[0]["LoadingPortName"].ToString();
            string PortOfDischarged = dsCANPrint.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            string NoOfPackages = dsCANPrint.Tables[0].Rows[0]["NoOfPackages"].ToString();
            string GrossWeight = dsCANPrint.Tables[0].Rows[0]["GrossWeight"].ToString();
            string ChargeableWeight = dsCANPrint.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            string LCLCBM = dsCANPrint.Tables[0].Rows[0]["LCLVolume"].ToString();
            string IGMNo = dsCANPrint.Tables[0].Rows[0]["IGMNo"].ToString();
            string ItemNo = dsCANPrint.Tables[0].Rows[0]["ItemNo"].ToString();
            string strDescription = dsCANPrint.Tables[0].Rows[0]["CargoDescription"].ToString();
            string strCANRemark = dsCANPrint.Tables[0].Rows[0]["CANRemark"].ToString();

            //string strServiceTax    =   "14 %";  // 
            //string strSBCTax        =   "0.5 %"; // Swatchh Bharat Cess - 0.5%

            string ATA = "", IGMDate = "";

            if (IGMNo == "")
                IGMNo = txtIGMNo.Text.Trim();

            if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();
            else if (txtATA.Text.Trim() != "")
            {
                ATA = txtATA.Text.Trim();
            }

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            string date = DateTime.Today.ToShortDateString();

            /************For Operation Detail Funtion
            TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

            if (txtCANDate != null)
            {
                if (txtCANDate.Text.Trim() != "")
                    date = txtCANDate.Text.Trim();
            }
            *******************************/
            DataSet dsCanInvoice = DBOperations.GetFreightCANPrintInvoice(EnqId);

            try
            {
                if (dsCanInvoice.Tables[0].Rows.Count > 0)
                {
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=CAN Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    StringWriter sw = new StringWriter();

                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    Rectangle recPDF = new Rectangle(PageSize.A4);

                    // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                    // Set PDF Document size and Left,Right,Top and Bottom margin

                    Document pdfDoc = new Document(recPDF);

                    // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                    logo.SetAbsolutePosition(380, 720);

                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    string contents = "";
                    contents = File.ReadAllText(Server.MapPath("CANLetter.htm"));
                    contents = contents.Replace("[TodayDate]", date.ToString());
                    contents = contents.Replace("[JobRefNO]", FRJobNo);
                    //   contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[ShipperName]", Shipper);

                    contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
                    contents = contents.Replace("[ShipperAddress]", ShipperAddress);

                    contents = contents.Replace("[MAWBL]", MBLNo);
                    contents = contents.Replace("[HAWBL]", HBLNo);
                    contents = contents.Replace("[InvoiceNo]", InvoiceNo);
                    contents = contents.Replace("[PONo]", PONumber);

                    contents = contents.Replace("[VesselName]", VesselName);
                    contents = contents.Replace("[VesselNo]", VesselNo);
                    contents = contents.Replace("[OriginPort]", LoadingPort);
                    contents = contents.Replace("[DestinationPort]", PortOfDischarged);

                    contents = contents.Replace("[NoofPkgs]", NoOfPackages);
                    contents = contents.Replace("[GrossWeight]", GrossWeight);

                    contents = contents.Replace("[ArrivalDate]", ATA);
                    contents = contents.Replace("[IGMNo]", IGMNo);
                    contents = contents.Replace("[IGMDate]", IGMDate);
                    contents = contents.Replace("[ITEMNo]", ItemNo);
                    contents = contents.Replace("[CargoDescription]", strDescription);

                    if (FreightMode == 1) // AIR
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CHARGEABLE WEIGHT (KGS)");
                        contents = contents.Replace("[ValueChargCBM]", ChargeableWeight);
                    }
                    else // SEA/ Breakbulk
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CBM");
                        contents = contents.Replace("[ValueChargCBM]", LCLCBM);
                    }

                    var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                    foreach (var htmlelement in parsedContent)
                        pdfDoc.Add(htmlelement as IElement);

                    PdfPTable pdftable = new PdfPTable(8);

                    pdftable.TotalWidth = 520f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.1f, 0.8f, 0.2f, 0.2f, 0.3f, 0.3f, 0.25f, 0.28f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                    // Set Table Spacing Before And After html text
                    //   pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 8f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    pdftable.AddCell(cellwithdata);

                    // cellwithdata.Colspan = 1;
                    // cellwithdata.BorderWidth = 1f;
                    // cellwithdata.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    // cellwithdata.VerticalAlignment = Element.ALIGN_CENTER;// Center

                    // Header: Desctiption
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("DESCRIPTION", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: Unit of Measurement
                    PdfPCell cellwithdata21 = new PdfPCell(new Phrase("UOM", GridHeadingFont));
                    cellwithdata21.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata21);

                    // Header: Rate
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("RATE", GridHeadingFont));
                    cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata2);

                    // Header: Currency

                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("CURRENCY", GridHeadingFont));
                    cellwithdata3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata3);

                    // Header: Amount
                    PdfPCell cellwithdata4 = new PdfPCell(new Phrase("AMOUNT (Rs)", GridHeadingFont));
                    cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata4);

                    /*************************************
                    // Header: Service Tax

                    PdfPCell cellwithdata51 = new PdfPCell(new Phrase("SERVICE TAX", GridHeadingFont));
                    cellwithdata51.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata51);

                    // Header: SBC - Swatchh Bharat Cess

                    PdfPCell cellwithdata52 = new PdfPCell(new Phrase("SBC", GridHeadingFont));
                    cellwithdata52.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata52);
                    *************************************/

                    // Header: Tax

                    PdfPCell cellwithdata5 = new PdfPCell(new Phrase("TAX (Rs)", GridHeadingFont));
                    cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata5);

                    // Header: Total Amount
                    PdfPCell cellwithdata6 = new PdfPCell(new Phrase("TOTAL (Rs)", GridHeadingFont));
                    cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata6);

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.UseVariableBorders = false;

                    // Data Cell: Description Of Charges

                    PdfPCell CellDescription = new PdfPCell();
                    CellDescription.Colspan = 1;
                    CellDescription.UseVariableBorders = false;

                    // Data Cell: Unit of Measurement

                    PdfPCell CellUOM = new PdfPCell();
                    CellUOM.Colspan = 1;
                    CellUOM.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellUOM.UseVariableBorders = false;

                    // Data Cell: Rate

                    PdfPCell CellRate = new PdfPCell();
                    CellRate.Colspan = 1;
                    CellRate.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellRate.UseVariableBorders = false;

                    // Data Cell: Currency
                    PdfPCell CellCurrency = new PdfPCell();
                    CellCurrency.Colspan = 1;
                    CellCurrency.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellCurrency.UseVariableBorders = false;

                    /*************************************************************
                    // Data Cell: Service Tax
                    PdfPCell CellServiceTax = new PdfPCell();
                    CellServiceTax.Colspan  = 1;
                    CellServiceTax.HorizontalAlignment  =   Element.ALIGN_RIGHT;
                    CellServiceTax.UseVariableBorders   =   false;

                    // Data Cell: SBC Tax (Swatchh Bharat CESS)
                    PdfPCell CellSBC    = new PdfPCell();
                    CellSBC.Colspan     = 1;
                    CellSBC.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellSBC.UseVariableBorders  = false;
                    *************************************************************/

                    // Data Cell: Tax
                    PdfPCell CellTax = new PdfPCell();
                    CellTax.Colspan = 1;
                    CellTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellTax.UseVariableBorders = false;

                    //  Data Cell: Amount

                    PdfPCell CellAmount = new PdfPCell();
                    CellAmount.Colspan = 1;
                    CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellAmount.UseVariableBorders = false;

                    // Data Cell: Total Amount

                    PdfPCell CellTotalAmount = new PdfPCell();
                    CellTotalAmount.Colspan = 1;
                    CellTotalAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellTotalAmount.UseVariableBorders = false;

                    //  Generate Table Data from CAN Invoice 

                    int rowCount = dsCanInvoice.Tables[0].Rows.Count;

                    foreach (DataRow dr in dsCanInvoice.Tables[0].Rows)
                    {
                        i = i + 1;
                        // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                        // Add Cell Data To Table

                        // Serial number #
                        if (rowCount == i) // last row blank
                        {
                            SrnoCell.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                        }

                        pdftable.AddCell(SrnoCell);

                        // Field Description - Report Header
                        if (rowCount == i) // last row font Bold
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextBoldformat);
                            // CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextBoldformat);
                        }
                        else
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextFontformat);
                            //CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);
                        }

                        pdftable.AddCell(CellDescription);

                        // CellUOM

                        CellUOM.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["UnitOfMeasurement"]), TextFontformat);

                        pdftable.AddCell(CellUOM);

                        // CellRate

                        if (rowCount == i) // last row blank
                        {
                            CellRate.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            CellRate.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Rate"]), TextFontformat);
                        }

                        pdftable.AddCell(CellRate);

                        // CellCurrency
                        if (rowCount == i) // last row blank
                        {
                            CellCurrency.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            string strCurrencyRate = dsCanInvoice.Tables[0].Rows[i - 1]["Currency"].ToString() + " - " +
                                    dsCanInvoice.Tables[0].Rows[i - 1]["ExchangeRate"].ToString();

                            CellCurrency.Phrase = new Phrase(Convert.ToString(strCurrencyRate), TextFontformat);
                        }

                        pdftable.AddCell(CellCurrency);

                        // CellAmount

                        if (rowCount == i) // last row font Bold
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextBoldformat);
                        }
                        else
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellAmount);

                        // CellTax // CellServiceTax // CellSBC
                        if (rowCount == i) // last row font Bold
                        {
                            //CellServiceTax.Phrase = new Phrase("", TextFontformat);
                            //CellSBC.Phrase = new Phrase("", TextFontformat);

                            CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextBoldformat);
                        }
                        else
                        {
                            //CellServiceTax.Phrase = new Phrase(Convert.ToString(strServiceTax), TextFontformat);
                            //CellSBC.Phrase = new Phrase(Convert.ToString(strSBCTax), TextFontformat);
                            CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextFontformat);
                        }

                        //pdftable.AddCell(CellServiceTax);
                        //pdftable.AddCell(CellSBC);

                        pdftable.AddCell(CellTax);

                        // CellTotalAmount
                        if (rowCount == i) // last row font Bold
                        {
                            Int32 intTotal = 0;
                            if (dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"] != DBNull.Value)
                            {
                                intTotal = Convert.ToInt32(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]);
                            }

                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(intTotal), TextBoldformat);

                            //CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextBoldformat);
                        }
                        else
                        {
                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellTotalAmount);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 5;//10

                    if (strCANRemark != "")
                    {
                        strCANRemark = "** " + strCANRemark;
                        pdfDoc.Add(new Paragraph(strCANRemark, FooterFontformat));
                    }

                    pdfDoc.Add(new Paragraph("\nFor Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    string footerText1 = "1. Shipment consigned to the bank, BRO (Bank Release Order) is Mandatory.\n" +
                        "2. Your cargo has not been checked while issuing this notice.\n" +
                        "3. Please produce a letter of authority at the time of collection of your documents & D.O.S.\n" +
                        "4. Please don't pay duty without checking on complete arrival of goods as per document.\n" +
                        "5. This transaction is covered under jurisdiction of Mumbai, Maharashtra.\n" +
                        "6. Computer generated document, hence signature not required.\n";

                    string footerText2 = "E. & O.E.\n";

                    string footerText3 = "For charges collect shipments, the Delivery Order will be issued against the amount indicated above" +
                        " on the receipt of the D.I.C. and Delivery Order will be only issued after the cargo is forwarded to the customs warehouse and the agent is not liable for any claim" +
                        " on warehouse charges as a result of delay on part of air carriers to check the freight and issue D.O.\n" +
                        " Demurrage shall apply as per tariff Till customs clearance is effected." +
                        " Please note that if the said consignment is not cleared on production of" +
                        " proper documents within 30 days from the date of arrival of the consignment. it is liable to be disposed of under provision of section 38 & 150 of the custom's Act, 1962.";

                    string footerText4 = "PAN No:   AAACB0466A  SERVICE TAX NO: AAACB0466AST001, SERVICE TAX CATEGORY - BUSINESS   AUXILIARY SERVICES";
                    //  string footerText5 = "SERVICE TAX CATEGORY - BUSINESS   AUXILIARY SERVICES";
                    string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
                            "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText1, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText2, TextFontformat));
                    pdfDoc.Add(new Paragraph(footerText3, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText4, FooterFontformat));
                    //  pdfDoc.Add(new Paragraph(footerText5, TextFontformat));
                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText6, FooterFontformat));

                    // Footer Image Commented
                    // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
                    // footer.SetAbsolutePosition(30, 0);
                    // pdfDoc.Add(footer);
                    // pdfwriter.PageEvent = new PDFFooter();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }//END_IF

                else
                {
                    lblError.Text = "Please Enter CAN Invoice Details!";
                    lblError.CssClass = "errorMsg";
                }

            }//END_Try

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblError.Text = "CAN Details Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    private void GenerateGSTPdf(int EnqId)
    {
        DataSet dsCANPrint = DBOperations.GetOperationDetail(EnqId);

        decimal decTotalTax = 0.00m; ;

        decimal decCGST = 0.00m;
        decimal decSGST = 0.00m;
        decimal decIGST = 0.00m;

        bool IsStateGST = false;
        //string EnqRefNo = "";
        if (hdnIsStateGST.Value == "1")
        {
            IsStateGST = true;
        }

        if (dsCANPrint.Tables[0].Rows.Count > 0)
        {
            
            int FreightMode = Convert.ToInt32(dsCANPrint.Tables[0].Rows[0]["lMode"]);
            string ModeName = dsCANPrint.Tables[0].Rows[0]["ModeName"].ToString();
            string FRJobNo = dsCANPrint.Tables[0].Rows[0]["FRJobNo"].ToString();
            string ENQRefNo = dsCANPrint.Tables[0].Rows[0]["ENQRefNo"].ToString();
            string Customer = dsCANPrint.Tables[0].Rows[0]["Customer"].ToString();
            string Consignee = dsCANPrint.Tables[0].Rows[0]["Consignee"].ToString();
            string Shipper = dsCANPrint.Tables[0].Rows[0]["Shipper"].ToString();
            string ConsigneeAddress = dsCANPrint.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
            string ShipperAddress = dsCANPrint.Tables[0].Rows[0]["ShipperAddress"].ToString();

            string MBLNo = dsCANPrint.Tables[0].Rows[0]["MBLNo"].ToString();
            string HBLNo = dsCANPrint.Tables[0].Rows[0]["HBLNo"].ToString();
            string InvoiceNo = dsCANPrint.Tables[0].Rows[0]["InvoiceNo"].ToString();
            string PONumber = dsCANPrint.Tables[0].Rows[0]["PONumber"].ToString();
            string VesselName = dsCANPrint.Tables[0].Rows[0]["VesselName"].ToString();
            string VesselNo = dsCANPrint.Tables[0].Rows[0]["VesselNumber"].ToString();
            string LoadingPort = dsCANPrint.Tables[0].Rows[0]["LoadingPortName"].ToString();
            string PortOfDischarged = dsCANPrint.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            string NoOfPackages = dsCANPrint.Tables[0].Rows[0]["NoOfPackages"].ToString();
            string GrossWeight = dsCANPrint.Tables[0].Rows[0]["GrossWeight"].ToString();
            string ChargeableWeight = dsCANPrint.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            string LCLCBM = dsCANPrint.Tables[0].Rows[0]["LCLVolume"].ToString();
            string IGMNo = dsCANPrint.Tables[0].Rows[0]["IGMNo"].ToString();
            string ItemNo = dsCANPrint.Tables[0].Rows[0]["ItemNo"].ToString();
            string strDescription = dsCANPrint.Tables[0].Rows[0]["CargoDescription"].ToString();
            string strCANRemark = dsCANPrint.Tables[0].Rows[0]["CANRemark"].ToString();
            string strConsigneeState = dsCANPrint.Tables[0].Rows[0]["ConsigneeStateName"].ToString();
            string strGSTN = dsCANPrint.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();

            string ContainerType = dsCANPrint.Tables[0].Rows[0]["ContainerTypeName"].ToString();
            string Count20 = dsCANPrint.Tables[0].Rows[0]["CountOf20"].ToString();
            string Count40 = dsCANPrint.Tables[0].Rows[0]["CountOf40"].ToString();
            //string strServiceTax    =   "14 %";  // 
            //string strSBCTax        =   "0.5 %"; // Swatchh Bharat Cess - 0.5%

            string ATA = "", IGMDate = "";

            if (IGMNo == "")
                IGMNo = txtIGMNo.Text.Trim();

            if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();
            else if (txtATA.Text.Trim() != "")
            {
                ATA = txtATA.Text.Trim();
            }
            //EnqRefNo = dsCANPrint.Tables[0].Rows["ENQRefNo"].ToString();
            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            string date = DateTime.Today.ToShortDateString();
                        
            /************ For Operation Detail Funtion ******************
            TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

            if (txtCANDate != null)
            {
                if (txtCANDate.Text.Trim() != "")
                    date = txtCANDate.Text.Trim();
            }
            *************************************************************/

            
            DataSet dsCanInvoice = DBOperations.GetFreightCANPrintInvoice(EnqId);

            try
            {
                if (dsCanInvoice.Tables[0].Rows.Count > 0)
                {

                    
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=CAN Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    StringWriter sw = new StringWriter();

                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    Rectangle recPDF = new Rectangle(PageSize.A4);

                    // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                    // Set PDF Document size and Left,Right,Top and Bottom margin

                    Document pdfDoc = new Document(recPDF);

                    // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                    logo.SetAbsolutePosition(430, 720);
                    logo.ScaleAbsolute(130f, 100f);
                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    string contents = "";
                    contents = File.ReadAllText(Server.MapPath("CANLetterGST.htm"));
                    contents = contents.Replace("[TodayDate]", date.ToString());
                    contents = contents.Replace("[JobRefNO]", FRJobNo);

                    contents = contents.Replace("[ENQRefNo]", ENQRefNo);
                    //   contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[ShipperName]", Shipper);

                    contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
                    contents = contents.Replace("[ShipperAddress]", ShipperAddress);

                    contents = contents.Replace("[MAWBL]", MBLNo);
                    contents = contents.Replace("[HAWBL]", HBLNo);
                    contents = contents.Replace("[InvoiceNo]", InvoiceNo);
                    contents = contents.Replace("[PONo]", PONumber);

                    contents = contents.Replace("[VesselName]", VesselName);
                    contents = contents.Replace("[VesselNo]", VesselNo);
                    contents = contents.Replace("[OriginPort]", LoadingPort);
                    contents = contents.Replace("[DestinationPort]", PortOfDischarged);

                    contents = contents.Replace("[NoofPkgs]", NoOfPackages);
                    contents = contents.Replace("[GrossWeight]", GrossWeight);

                    contents = contents.Replace("[ArrivalDate]", ATA);
                    contents = contents.Replace("[IGMNo]", IGMNo);
                    contents = contents.Replace("[IGMDate]", IGMDate);
                    contents = contents.Replace("[ITEMNo]", ItemNo);
                    contents = contents.Replace("[CargoDescription]", strDescription);
                    contents = contents.Replace("[DivGSTN]", strGSTN);
                    contents = contents.Replace("[PlaceOfDelivery]", strConsigneeState);
                    contents = contents.Replace("[Mode]", ModeName);

                    if (FreightMode == 1) // AIR
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CHARG WEIGHT (KGS)");
                        contents = contents.Replace("[ValueChargCBM]", ChargeableWeight);
                        contents = contents.Replace("[ContainerTypeName]", "-");
                        contents = contents.Replace("[CountOf20]", "-");
                        contents = contents.Replace("[CountOf40]", "-");
                    }
                    else // SEA/ Breakbulk
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CBM");
                        contents = contents.Replace("[ValueChargCBM]", LCLCBM);
                        contents = contents.Replace("[ContainerTypeName]", ContainerType);
                        contents = contents.Replace("[CountOf20]", Count20);
                        contents = contents.Replace("[CountOf40]", Count40);
                    }

                    var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                    foreach (var htmlelement in parsedContent)
                        pdfDoc.Add(htmlelement as IElement);

                    PdfPTable pdftable = new PdfPTable(10);

                    pdftable.TotalWidth = 540f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.06f, 0.4f, 0.15f, 0.15f, 0.25f, 0.2f, 0.20f, 0.2f, 0.2f, 0.2f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                    // Set Table Spacing Before And After html text
                    //   pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 8f;
                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    cellwithdata.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata);

                    // cellwithdata.Colspan = 1;
                    // cellwithdata.BorderWidth = 1f;
                    // cellwithdata.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    // cellwithdata.VerticalAlignment = Element.ALIGN_CENTER;// Center

                    // Header: Desctiption
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("CHARGE CODE", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: Unit of Measurement
                    PdfPCell cellwithdata21 = new PdfPCell(new Phrase("UOM", GridHeadingFont));
                    cellwithdata21.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata21);

                    // Header: Rate
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("RATE", GridHeadingFont));
                    cellwithdata2.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata2);

                    // Header: Currency

                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("CURRENCY", GridHeadingFont));
                    cellwithdata3.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata3);

                    // Header: Amount
                    PdfPCell cellwithdata4 = new PdfPCell(new Phrase("AMOUNT (Rs)", GridHeadingFont));
                    cellwithdata4.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata4);

                    //// Header: CGST Rate
                    //PdfPCell cellwithdata51 = new PdfPCell(new Phrase("CGST %", GridHeadingFont));
                    //cellwithdata51.HorizontalAlignment = Element.ALIGN_LEFT;
                    //pdftable.AddCell(cellwithdata51);


                    // Header: CGST Amount
                    PdfPCell cellwithdata5 = new PdfPCell(new Phrase("CGST (Rs)", GridHeadingFont));
                    cellwithdata5.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata5);

                    //// Header: SGST Rate
                    //PdfPCell cellwithdata6 = new PdfPCell(new Phrase("SGST %", GridHeadingFont));
                    //cellwithdata6.HorizontalAlignment = Element.ALIGN_LEFT;
                    //pdftable.AddCell(cellwithdata6);


                    // Header: SGST Amount
                    PdfPCell cellwithdata7 = new PdfPCell(new Phrase("SGST (Rs)", GridHeadingFont));
                    cellwithdata7.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata7);

                    //// Header: IGST Rate
                    //PdfPCell cellwithdata8 = new PdfPCell(new Phrase("IGST %", GridHeadingFont));
                    //cellwithdata8.HorizontalAlignment = Element.ALIGN_LEFT;
                    //pdftable.AddCell(cellwithdata8);


                    // Header: IGST Amount
                    PdfPCell cellwithdata9 = new PdfPCell(new Phrase("IGST (Rs)", GridHeadingFont));
                    cellwithdata9.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata9);

                    // Header: Total Amount
                    PdfPCell cellwithdata10 = new PdfPCell(new Phrase("TOTAL (Rs)", GridHeadingFont));
                    cellwithdata10.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdftable.AddCell(cellwithdata10);

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    SrnoCell.UseVariableBorders = false;

                    // Data Cell: Description Of Charges

                    PdfPCell CellDescription = new PdfPCell();
                    CellDescription.Colspan = 1;
                    CellDescription.UseVariableBorders = false;

                    // Data Cell: Unit of Measurement

                    PdfPCell CellUOM = new PdfPCell();
                    CellUOM.Colspan = 1;
                    CellUOM.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellUOM.UseVariableBorders = false;

                    // Data Cell: Rate
                    PdfPCell CellRate = new PdfPCell();
                    CellRate.Colspan = 1;
                    CellRate.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellRate.UseVariableBorders = false;

                    // Data Cell: Currency
                    PdfPCell CellCurrency = new PdfPCell();
                    CellCurrency.Colspan = 1;
                    CellCurrency.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellCurrency.UseVariableBorders = false;

                    //  Data Cell: Amount
                    PdfPCell CellAmount = new PdfPCell();
                    CellAmount.Colspan = 1;
                    CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellAmount.UseVariableBorders = false;

                    ////  Data Cell: CGST Tax %
                    //PdfPCell CellCGSTTax = new PdfPCell();
                    //CellCGSTTax.Colspan = 1;
                    //CellCGSTTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //CellCGSTTax.UseVariableBorders = false;

                    //  Data Cell: CGST Tax Amt
                    PdfPCell CellCGSTTaxAmt = new PdfPCell();
                    CellCGSTTaxAmt.Colspan = 1;
                    CellCGSTTaxAmt.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellCGSTTaxAmt.UseVariableBorders = false;

                    ////  Data Cell: SGST Tax %
                    //PdfPCell CellSGSTTax = new PdfPCell();
                    //CellSGSTTax.Colspan = 1;
                    //CellSGSTTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //CellSGSTTax.UseVariableBorders = false;

                    //  Data Cell: SGST Tax Amt
                    PdfPCell CellSGSTTaxAmt = new PdfPCell();
                    CellSGSTTaxAmt.Colspan = 1;
                    CellSGSTTaxAmt.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellSGSTTaxAmt.UseVariableBorders = false;

                    ////  Data Cell: IGST Tax %
                    //PdfPCell CellIGSTTax = new PdfPCell();
                    //CellIGSTTax.Colspan = 1;
                    //CellIGSTTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //CellIGSTTax.UseVariableBorders = false;

                    //  Data Cell: IGST Tax Amt
                    PdfPCell CellIGSTTaxAmt = new PdfPCell();
                    CellIGSTTaxAmt.Colspan = 1;
                    CellIGSTTaxAmt.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellIGSTTaxAmt.UseVariableBorders = false;

                    // Data Cell: Total Amount
                    PdfPCell CellTotalAmount = new PdfPCell();
                    CellTotalAmount.Colspan = 1;
                    CellTotalAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellTotalAmount.UseVariableBorders = false;

                    //  Generate Table Data from CAN Invoice 

                    int rowCount = dsCanInvoice.Tables[0].Rows.Count;

                    foreach (DataRow dr in dsCanInvoice.Tables[0].Rows)
                    {
                        i = i + 1;

                        // Add Cell Data To Table

                        // Serial number #
                        if (rowCount == i) // last row blank
                        {
                            SrnoCell.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                        }

                        pdftable.AddCell(SrnoCell);

                        // Field Description - Report Header
                        if (rowCount == i) // last row font Bold
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextBoldformat);
                            // CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextBoldformat);
                        }
                        else
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextFontformat);
                            //CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);
                        }

                        pdftable.AddCell(CellDescription);

                        // CellUOM

                        CellUOM.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["UnitOfMeasurement"]), TextFontformat);

                        pdftable.AddCell(CellUOM);

                        // CellRate

                        if (rowCount == i) // last row blank
                        {
                            CellRate.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            CellRate.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Rate"]), TextFontformat);
                        }

                        pdftable.AddCell(CellRate);

                        // CellCurrency
                        if (rowCount == i) // last row blank
                        {
                            CellCurrency.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            string strCurrencyRate = dsCanInvoice.Tables[0].Rows[i - 1]["Currency"].ToString() + " - " +
                                    dsCanInvoice.Tables[0].Rows[i - 1]["ExchangeRate"].ToString();

                            CellCurrency.Phrase = new Phrase(Convert.ToString(strCurrencyRate), TextFontformat);
                        }

                        pdftable.AddCell(CellCurrency);

                        // CellAmount

                        if (rowCount == i) // last row font Bold
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextBoldformat);
                        }
                        else
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellAmount);

                        //// SGST Tax %
                        //if (rowCount == i) // last row blank
                        //{
                        //    CellSGSTTax.Phrase = new Phrase("", TextFontformat);
                        //}
                        //else
                        //{
                        //    CellSGSTTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["SGSTTax"]), TextFontformat);
                        //}

                        //pdftable.AddCell(CellSGSTTax);

                        // SGST Tax Amount
                        if (rowCount == i) // last row font Bold
                        {
                            CellSGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["SGSTTaxAmount"]), TextBoldformat);
                        }
                        else
                        {
                            CellSGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["SGSTTaxAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellSGSTTaxAmt);

                        //// CGST Tax %
                        //if (rowCount == i) // last row blank
                        //{
                        //    CellCGSTTax.Phrase = new Phrase("", TextFontformat);
                        //}
                        //else
                        //{
                        //    CellCGSTTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["CGSTTax"]), TextFontformat);
                        //}

                        //pdftable.AddCell(CellCGSTTax);

                        // CGST Tax Amount
                        if (rowCount == i) // last row font Bold
                        {
                            CellCGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["CGSTTaxAmount"]), TextBoldformat);
                        }
                        else
                        {
                            CellCGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["CGSTTaxAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellCGSTTaxAmt);

                        //// IGST Tax %
                        //if (rowCount == i) // last row blank
                        //{
                        //    CellIGSTTax.Phrase = new Phrase("", TextFontformat);
                        //}
                        //else
                        //{
                        //    CellIGSTTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["IGSTTax"]), TextFontformat);
                        //}

                        //pdftable.AddCell(CellIGSTTax);

                        // IGST Tax Amount
                        if (rowCount == i) // last row font Bold
                        {
                            CellIGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["IGSTTaxAmount"]), TextBoldformat);
                        }
                        else
                        {
                            CellIGSTTaxAmt.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["IGSTTaxAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellIGSTTaxAmt);

                        // CellTotalAmount
                        if (rowCount == i) // last row font Bold
                        {
                            Int32 intTotal = 0;
                            if (dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"] != DBNull.Value)
                            {
                                intTotal = Convert.ToInt32(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]);
                            }

                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(intTotal), TextBoldformat);
                        }
                        else
                        {
                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellTotalAmount);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 5;//10

                    if (strCANRemark != "")
                    {
                        strCANRemark = "** " + strCANRemark;
                        pdfDoc.Add(new Paragraph(strCANRemark, FooterFontformat));
                    }

                    /***********************************  SAC Code Rate Table  *****************************************/

                    DataSet dsHSNRates = DBOperations.GetHSNSacRateDetails(EnqId);

                    int rowGstCount = dsHSNRates.Tables[0].Rows.Count;
                    int icount = 0;
                    PdfPTable pdfGSTTable = new PdfPTable(6);

                    pdfGSTTable.TotalWidth = 480f;
                    pdfGSTTable.LockedWidth = true;
                    float[] tblWidths = new float[] { 0.1f, 1.5f, 0.3f, 0.3f, 0.3f, 0.3f };
                    pdfGSTTable.SetWidths(tblWidths);
                    pdfGSTTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    // Set Table Spacing Before And After html text
                    //   pdftable.SpacingBefore = 10f;
                    pdfGSTTable.SpacingAfter = 8f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellSR = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    cellSR.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfGSTTable.AddCell(cellSR);

                    // Header: SAC Code
                    PdfPCell cellCharge = new PdfPCell(new Phrase("Charge Code", GridHeadingFont));
                    pdfGSTTable.AddCell(cellCharge);

                    // Header: HSN/SAC
                    PdfPCell cellSAC = new PdfPCell(new Phrase("HSN/SAC", GridHeadingFont));
                    cellSAC.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfGSTTable.AddCell(cellSAC);

                    // Header: CGST
                    PdfPCell cellCGST = new PdfPCell(new Phrase("CGST (%)", GridHeadingFont));
                    cellCGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfGSTTable.AddCell(cellCGST);

                    // Header: SGST
                    PdfPCell cellSGST = new PdfPCell(new Phrase("SGST (%)", GridHeadingFont));
                    cellSGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfGSTTable.AddCell(cellSGST);

                    // Header: IGST
                    PdfPCell cellIGST = new PdfPCell(new Phrase("IGST (%)", GridHeadingFont));
                    cellIGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfGSTTable.AddCell(cellIGST);

                    foreach (DataRow dr in dsHSNRates.Tables[0].Rows)
                    {
                        icount = icount + 1;

                        // Serial number #
                        cellSR.Phrase = new Phrase(Convert.ToString(icount), TextFontformat);
                        pdfGSTTable.AddCell(cellSR);

                        //Charge code
                        cellCharge.Phrase = new Phrase(Convert.ToString(dsHSNRates.Tables[0].Rows[icount - 1]["FieldName"]), TextFontformat);
                        pdfGSTTable.AddCell(cellCharge);

                        // SAC Code 
                        cellSAC.Phrase = new Phrase(Convert.ToString(dsHSNRates.Tables[0].Rows[icount - 1]["SacNo"]), TextFontformat);
                        pdfGSTTable.AddCell(cellSAC);

                        // CGST - 
                        cellCGST.Phrase = new Phrase(Convert.ToString(dsHSNRates.Tables[0].Rows[icount - 1]["CGSTTax"]), TextFontformat);
                        pdfGSTTable.AddCell(cellCGST);

                        // SGST - 
                        cellSGST.Phrase = new Phrase(Convert.ToString(dsHSNRates.Tables[0].Rows[icount - 1]["SGSTTax"]), TextFontformat);
                        pdfGSTTable.AddCell(cellSGST);

                        //IGST
                        cellIGST.Phrase = new Phrase(Convert.ToString(dsHSNRates.Tables[0].Rows[icount - 1]["IGSTTax"]), TextFontformat);
                        pdfGSTTable.AddCell(cellIGST);

                    }
                    /***************************************************************************/

                    /*********** GST Detail *******************************/

                    //  Generate Table Data from GST Composition

                    //DataSet dsCanGST = DBOperations.GetFreightCANGST(EnqId);

                    //int rowGstCount = dsCanGST.Tables[0].Rows.Count;
                    //int icount = 0;
                    //PdfPTable pdfGSTTable = new PdfPTable(5);

                    //pdfGSTTable.TotalWidth = 250f;
                    //pdfGSTTable.LockedWidth = true;
                    //float[] tblWidths = new float[] { 0.2f, 0.4f, 0.4f, 0.4f, 0.4f };
                    //pdfGSTTable.SetWidths(tblWidths);
                    //pdfGSTTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    //// Set Table Spacing Before And After html text
                    ////   pdftable.SpacingBefore = 10f;
                    //pdfGSTTable.SpacingAfter = 8f;

                    //// Create Table Column Header Cell with Text

                    //// Header: Serial Number
                    //PdfPCell cellSR = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    //pdfGSTTable.AddCell(cellSR);

                    //// Header: SAC Code
                    //PdfPCell cellSAC = new PdfPCell(new Phrase("SAC", GridHeadingFont));
                    //pdfGSTTable.AddCell(cellSAC);

                    //// Header: CGST
                    //PdfPCell cellCGST = new PdfPCell(new Phrase("CGST", GridHeadingFont));
                    //pdfGSTTable.AddCell(cellCGST);

                    //// Header: SGST
                    //PdfPCell cellSGST = new PdfPCell(new Phrase("SGST", GridHeadingFont));
                    //pdfGSTTable.AddCell(cellSGST);

                    //// Header: IGST
                    //PdfPCell cellIGST = new PdfPCell(new Phrase("IGST", GridHeadingFont));
                    //pdfGSTTable.AddCell(cellIGST);

                    //foreach (DataRow dr in dsCanGST.Tables[0].Rows)
                    //{
                    //    icount = icount + 1;

                    //    // Serial number #
                    //    if (rowGstCount == icount) // last row blank
                    //    {
                    //        cellSR.Phrase = new Phrase("", TextFontformat);
                    //    }
                    //    else
                    //    {
                    //        cellSR.Phrase = new Phrase(Convert.ToString(icount), TextFontformat);
                    //    }

                    //    pdfGSTTable.AddCell(cellSR);

                    //    // SAC - 
                    //    if (rowGstCount == icount) // last row font Bold
                    //    {
                    //        cellSAC.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["SACCode"]), TextBoldformat);
                    //    }
                    //    else
                    //    {
                    //        cellSAC.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["SACCode"]), TextFontformat);
                    //    }

                    //    pdfGSTTable.AddCell(cellSAC);

                    //    // CGST - 
                    //    if (rowGstCount == icount) // last row font Bold
                    //    {
                    //        cellCGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["CGST"]), TextBoldformat);
                    //    }
                    //    else
                    //    {
                    //        cellCGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["CGST"]), TextFontformat);
                    //    }

                    //    pdfGSTTable.AddCell(cellCGST);

                    //    // SGST - 
                    //    if (rowGstCount == icount) // last row font Bold
                    //    {
                    //        cellSGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["SGST"]), TextBoldformat);
                    //    }
                    //    else
                    //    {
                    //        cellSGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["SGST"]), TextFontformat);
                    //    }

                    //    pdfGSTTable.AddCell(cellSGST);

                    //    // IGST - 
                    //    if (rowGstCount == icount) // last row font Bold
                    //    {
                    //        cellIGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["IGST"]), TextBoldformat);
                    //    }
                    //    else
                    //    {
                    //        cellIGST.Phrase = new Phrase(Convert.ToString(dsCanGST.Tables[0].Rows[icount - 1]["IGST"]), TextFontformat);
                    //    }

                    //    pdfGSTTable.AddCell(cellIGST);

                    //}
                    /***************************************************************************/

                    string strGSTFooter = "";

                    //if (IsStateGST == true)
                    //{
                    //    decCGST = decTotalTax / 2;
                    //    decSGST = decTotalTax / 2;

                    //    strGSTFooter = "\n" + "CGST - Rs. " + decCGST.ToString() + "\n" +
                    //        "SGST - Rs. " + decSGST.ToString() + "\n";
                    //}
                    //else
                    //{
                    //    strGSTFooter = "IGST - Rs. " + decTotalTax + "\n";
                    //}

                    // pdfDoc.Add(new Paragraph("GST COMPOSITION   : ", TextBoldformat));

                    // pdfDoc.Add(ParaSpacing);
                    // pdfDoc.Add(pdfGSTTable);

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(pdfGSTTable);

                    //if (rowCount < 16 && rowCount == 11)
                    //{
                    //    pdfDoc.NewPage(); // if charge code rows are equal to 10 then display the points to remember at second page.
                    //}

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    string footerText1 = "1. Shipment consigned to the bank, BRO (Bank Release Order) is Mandatory.\n" +
                          "2. Your cargo has not been checked while issuing this notice.\n" +
                          "3. Please produce a letter of authority at the time of collection of your documents & D.O.S.\n" +
                          "4. Please don't pay duty without checking on complete arrival of goods as per document.\n" +
                          "5. This transaction is covered under jurisdiction of Mumbai, Maharashtra. Computer generated document, hence signature not required.\n";

                    string footerText2 = "E. & O.E.\n";

                    string footerText3 = "For charges collect shipments, the Delivery Order will be issued against the amount indicated above" +
                        " on the receipt of the D.I.C. and Delivery Order will be only issued after the cargo is forwarded to the customs warehouse and the agent is not liable for any claim" +
                        " on warehouse charges as a result of delay on part of air carriers to check the freight and issue D.O. Demurrage shall apply as per tariff till customs clearance is effected." +
                        " Please note that if the said consignment is not cleared on production of proper documents within 30 days from the date of arrival of the consignment." +
                        " it is liable to be disposed of under provision of section 38 & 150 of the custom's Act, 1962.";

                    string footerText4 = "PAN No:   AAACB0466A";
                    string footerText5 = "GSTN credit should not be availed, based on cargo arrival notice, separate invoice will be provided." +
                        "GST taxes provided is for estimation purpose only";

                    string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
                            "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText1, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText2, TextFontformat));
                    pdfDoc.Add(new Paragraph(footerText3, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText4, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText5, FooterFontformat));
                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText6, FooterFontformat));

                    // Footer Image Commented
                    // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
                    // footer.SetAbsolutePosition(30, 0);
                    // pdfDoc.Add(footer);
                    // pdfwriter.PageEvent = new PDFFooter();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }//END_IF

                else
                {
                    lblError.Text = "Please Enter CAN Invoice Details!";
                    lblError.CssClass = "errorMsg";
                }

            }//END_Try

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblError.Text = "CAN Details Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    //private void GenerateGSTPdf(int EnqId)
    //{
    //    DataSet dsCANPrint = DBOperations.GetOperationDetail(EnqId);

    //    if (dsCANPrint.Tables[0].Rows.Count > 0)
    //    {
    //        int FreightMode = Convert.ToInt32(dsCANPrint.Tables[0].Rows[0]["lMode"]);

    //        string FRJobNo = dsCANPrint.Tables[0].Rows[0]["FRJobNo"].ToString();
    //        string Customer = dsCANPrint.Tables[0].Rows[0]["Customer"].ToString();
    //        string Consignee = dsCANPrint.Tables[0].Rows[0]["Consignee"].ToString();
    //        string Shipper = dsCANPrint.Tables[0].Rows[0]["Shipper"].ToString();
    //        string ConsigneeAddress = dsCANPrint.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
    //        string ShipperAddress = dsCANPrint.Tables[0].Rows[0]["ShipperAddress"].ToString();

    //        string MBLNo = dsCANPrint.Tables[0].Rows[0]["MBLNo"].ToString();
    //        string HBLNo = dsCANPrint.Tables[0].Rows[0]["HBLNo"].ToString();
    //        string InvoiceNo = dsCANPrint.Tables[0].Rows[0]["InvoiceNo"].ToString();
    //        string PONumber = dsCANPrint.Tables[0].Rows[0]["PONumber"].ToString();
    //        string VesselName = dsCANPrint.Tables[0].Rows[0]["VesselName"].ToString();
    //        string VesselNo = dsCANPrint.Tables[0].Rows[0]["VesselNumber"].ToString();
    //        string LoadingPort = dsCANPrint.Tables[0].Rows[0]["LoadingPortName"].ToString();
    //        string PortOfDischarged = dsCANPrint.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
    //        string NoOfPackages = dsCANPrint.Tables[0].Rows[0]["NoOfPackages"].ToString();
    //        string GrossWeight = dsCANPrint.Tables[0].Rows[0]["GrossWeight"].ToString();
    //        string ChargeableWeight = dsCANPrint.Tables[0].Rows[0]["ChargeableWeight"].ToString();
    //        string LCLCBM = dsCANPrint.Tables[0].Rows[0]["LCLVolume"].ToString();
    //        string IGMNo = dsCANPrint.Tables[0].Rows[0]["IGMNo"].ToString();
    //        string ItemNo = dsCANPrint.Tables[0].Rows[0]["ItemNo"].ToString();
    //        string strDescription = dsCANPrint.Tables[0].Rows[0]["CargoDescription"].ToString();
    //        string strCANRemark = dsCANPrint.Tables[0].Rows[0]["CANRemark"].ToString();

    //        string strConsigneeState = dsCANPrint.Tables[0].Rows[0]["ConsigneeStateName"].ToString();
    //        string strGSTN = dsCANPrint.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();

    //        //string strServiceTax    =   "14 %";  // 
    //        //string strSBCTax        =   "0.5 %"; // Swatchh Bharat Cess - 0.5%

    //        string ATA = "", IGMDate = "";

    //        if (IGMNo == "")
    //            IGMNo = txtIGMNo.Text.Trim();

    //        if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
    //            IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

    //        if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
    //            ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();
    //        else if (txtATA.Text.Trim() != "")
    //        {
    //            ATA = txtATA.Text.Trim();
    //        }

    //        string CanUserName = LoggedInUser.glEmpName;

    //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF.jpg"));
    //        string date = DateTime.Today.ToShortDateString();

    //        /************ For Operation Detail Funtion ******************
    //        TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

    //        if (txtCANDate != null)
    //        {
    //            if (txtCANDate.Text.Trim() != "")
    //                date = txtCANDate.Text.Trim();
    //        }
    //        *************************************************************/
    //        DataSet dsCanInvoice = DBOperations.GetFreightCANPrintInvoice(EnqId);

    //        try
    //        {
    //            if (dsCanInvoice.Tables[0].Rows.Count > 0)
    //            {
    //                // Generate PDF
    //                int i = 0; // Auto Increment Table Cell For Serial number
    //                Response.ContentType = "application/pdf";
    //                Response.AddHeader("content-disposition", "attachment;filename=CAN Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
    //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //                StringWriter sw = new StringWriter();

    //                HtmlTextWriter hw = new HtmlTextWriter(sw);
    //                StringReader sr = new StringReader(sw.ToString());

    //                Rectangle recPDF = new Rectangle(PageSize.A4);

    //                // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
    //                // Set PDF Document size and Left,Right,Top and Bottom margin

    //                Document pdfDoc = new Document(recPDF);

    //                // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
    //                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

    //                pdfDoc.Open();

    //                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
    //                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
    //                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
    //                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

    //                logo.SetAbsolutePosition(380, 720);

    //                logo.Alignment = Convert.ToInt32(ImageAlign.Right);
    //                pdfDoc.Add(logo);

    //                string contents = "";
    //                contents = File.ReadAllText(Server.MapPath("CANLetterGST.htm"));
    //                contents = contents.Replace("[TodayDate]", date.ToString());
    //                contents = contents.Replace("[JobRefNO]", FRJobNo);
    //                //   contents = contents.Replace("[CustomerName]", Customer);
    //                contents = contents.Replace("[ConsigneeName]", Consignee);
    //                contents = contents.Replace("[ShipperName]", Shipper);

    //                contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
    //                contents = contents.Replace("[ShipperAddress]", ShipperAddress);

    //                contents = contents.Replace("[MAWBL]", MBLNo);
    //                contents = contents.Replace("[HAWBL]", HBLNo);
    //                contents = contents.Replace("[InvoiceNo]", InvoiceNo);
    //                contents = contents.Replace("[PONo]", PONumber);

    //                contents = contents.Replace("[VesselName]", VesselName);
    //                contents = contents.Replace("[VesselNo]", VesselNo);
    //                contents = contents.Replace("[OriginPort]", LoadingPort);
    //                contents = contents.Replace("[DestinationPort]", PortOfDischarged);

    //                contents = contents.Replace("[NoofPkgs]", NoOfPackages);
    //                contents = contents.Replace("[GrossWeight]", GrossWeight);

    //                contents = contents.Replace("[ArrivalDate]", ATA);
    //                contents = contents.Replace("[IGMNo]", IGMNo);
    //                contents = contents.Replace("[IGMDate]", IGMDate);
    //                contents = contents.Replace("[ITEMNo]", ItemNo);
    //                contents = contents.Replace("[CargoDescription]", strDescription);
    //                contents = contents.Replace("[DivGSTN]", strGSTN);
    //                contents = contents.Replace("[PlaceOfDelivery]", strConsigneeState);

    //                if (FreightMode == 1) // AIR
    //                {
    //                    contents = contents.Replace("[lblChargeCBM]", "CHARGEABLE WEIGHT (KGS)");
    //                    contents = contents.Replace("[ValueChargCBM]", ChargeableWeight);
    //                }
    //                else // SEA/ Breakbulk
    //                {
    //                    contents = contents.Replace("[lblChargeCBM]", "CBM");
    //                    contents = contents.Replace("[ValueChargCBM]", LCLCBM);
    //                }

    //                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
    //                foreach (var htmlelement in parsedContent)
    //                    pdfDoc.Add(htmlelement as IElement);

    //                PdfPTable pdftable = new PdfPTable(9);

    //                pdftable.TotalWidth = 520f;
    //                pdftable.LockedWidth = true;
    //                float[] widths = new float[] { 0.1f, 0.6f, 0.2f, 0.2f, 0.3f, 0.32f, 0.3f, 0.25f, 0.3f };
    //                pdftable.SetWidths(widths);
    //                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

    //                // Set Table Spacing Before And After html text
    //                //   pdftable.SpacingBefore = 10f;
    //                pdftable.SpacingAfter = 8f;

    //                // Create Table Column Header Cell with Text

    //                // Header: Serial Number
    //                PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
    //                pdftable.AddCell(cellwithdata);

    //                // cellwithdata.Colspan = 1;
    //                // cellwithdata.BorderWidth = 1f;
    //                // cellwithdata.HorizontalAlignment = Element.ALIGN_MIDDLE;
    //                // cellwithdata.VerticalAlignment = Element.ALIGN_CENTER;// Center

    //                // Header: Desctiption
    //                PdfPCell cellwithdata1 = new PdfPCell(new Phrase("DESCRIPTION", GridHeadingFont));
    //                pdftable.AddCell(cellwithdata1);

    //                // Header: Unit of Measurement
    //                PdfPCell cellwithdata21 = new PdfPCell(new Phrase("UOM", GridHeadingFont));
    //                cellwithdata21.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata21);

    //                // Header: Rate
    //                PdfPCell cellwithdata2 = new PdfPCell(new Phrase("RATE", GridHeadingFont));
    //                cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata2);

    //                // Header: Currency

    //                PdfPCell cellwithdata3 = new PdfPCell(new Phrase("CURRENCY", GridHeadingFont));
    //                cellwithdata3.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata3);

    //                // Header: Amount
    //                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("AMOUNT (Rs)", GridHeadingFont));
    //                cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata4);

    //                // Header: GST Rate

    //                PdfPCell cellwithdata51 = new PdfPCell(new Phrase("GST Rate %", GridHeadingFont));
    //                cellwithdata51.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata51);


    //                // Header: GST Amount

    //                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("GST (Rs)", GridHeadingFont));
    //                cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata5);

    //                // Header: Total Amount
    //                PdfPCell cellwithdata6 = new PdfPCell(new Phrase("TOTAL (Rs)", GridHeadingFont));
    //                cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                pdftable.AddCell(cellwithdata6);

    //                // Data Cell: Serial Number - Auto Increment Cell

    //                PdfPCell SrnoCell = new PdfPCell();
    //                SrnoCell.Colspan = 1;
    //                SrnoCell.UseVariableBorders = false;

    //                // Data Cell: Description Of Charges

    //                PdfPCell CellDescription = new PdfPCell();
    //                CellDescription.Colspan = 1;
    //                CellDescription.UseVariableBorders = false;

    //                // Data Cell: Unit of Measurement

    //                PdfPCell CellUOM = new PdfPCell();
    //                CellUOM.Colspan = 1;
    //                CellUOM.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellUOM.UseVariableBorders = false;

    //                // Data Cell: Rate

    //                PdfPCell CellRate = new PdfPCell();
    //                CellRate.Colspan = 1;
    //                CellRate.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellRate.UseVariableBorders = false;

    //                // Data Cell: Currency
    //                PdfPCell CellCurrency = new PdfPCell();
    //                CellCurrency.Colspan = 1;
    //                CellCurrency.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellCurrency.UseVariableBorders = false;


    //                // Data Cell: GST Tax
    //                PdfPCell CellGSTTax = new PdfPCell();
    //                CellGSTTax.Colspan = 1;
    //                CellGSTTax.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellGSTTax.UseVariableBorders = false;


    //                // Data Cell: Tax
    //                PdfPCell CellTax = new PdfPCell();
    //                CellTax.Colspan = 1;
    //                CellTax.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellTax.UseVariableBorders = false;

    //                //  Data Cell: Amount

    //                PdfPCell CellAmount = new PdfPCell();
    //                CellAmount.Colspan = 1;
    //                CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellAmount.UseVariableBorders = false;

    //                // Data Cell: Total Amount

    //                PdfPCell CellTotalAmount = new PdfPCell();
    //                CellTotalAmount.Colspan = 1;
    //                CellTotalAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
    //                CellTotalAmount.UseVariableBorders = false;

    //                //  Generate Table Data from CAN Invoice 

    //                int rowCount = dsCanInvoice.Tables[0].Rows.Count;

    //                foreach (DataRow dr in dsCanInvoice.Tables[0].Rows)
    //                {
    //                    i = i + 1;
    //                    // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

    //                    // Add Cell Data To Table

    //                    // Serial number #
    //                    if (rowCount == i) // last row blank
    //                    {
    //                        SrnoCell.Phrase = new Phrase("", TextFontformat);
    //                    }
    //                    else
    //                    {
    //                        SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
    //                    }

    //                    pdftable.AddCell(SrnoCell);

    //                    // Field Description - Report Header
    //                    if (rowCount == i) // last row font Bold
    //                    {
    //                        CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextBoldformat);
    //                        // CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextBoldformat);
    //                    }
    //                    else
    //                    {
    //                        CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextFontformat);
    //                        //CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellDescription);

    //                    // CellUOM

    //                    CellUOM.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["UnitOfMeasurement"]), TextFontformat);

    //                    pdftable.AddCell(CellUOM);

    //                    // CellRate

    //                    if (rowCount == i) // last row blank
    //                    {
    //                        CellRate.Phrase = new Phrase("", TextFontformat);
    //                    }
    //                    else
    //                    {
    //                        CellRate.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Rate"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellRate);

    //                    // CellCurrency
    //                    if (rowCount == i) // last row blank
    //                    {
    //                        CellCurrency.Phrase = new Phrase("", TextFontformat);
    //                    }
    //                    else
    //                    {
    //                        string strCurrencyRate = dsCanInvoice.Tables[0].Rows[i - 1]["Currency"].ToString() + " - " +
    //                                dsCanInvoice.Tables[0].Rows[i - 1]["ExchangeRate"].ToString();

    //                        CellCurrency.Phrase = new Phrase(Convert.ToString(strCurrencyRate), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellCurrency);

    //                    // CellAmount

    //                    if (rowCount == i) // last row font Bold
    //                    {
    //                        CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextBoldformat);
    //                    }
    //                    else
    //                    {
    //                        CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellAmount);

    //                    // GST Tax %

    //                    if (rowCount == i) // last row blank
    //                    {
    //                        CellGSTTax.Phrase = new Phrase("", TextFontformat);
    //                    }
    //                    else
    //                    {
    //                        CellGSTTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxPercentage"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellGSTTax);

    //                    // CellTax // CellServiceTax // CellSBC
    //                    if (rowCount == i) // last row font Bold
    //                    {
    //                        CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextBoldformat);
    //                    }
    //                    else
    //                    {
    //                        CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellTax);

    //                    // CellTotalAmount
    //                    if (rowCount == i) // last row font Bold
    //                    {
    //                        Int32 intTotal = 0;
    //                        if (dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"] != DBNull.Value)
    //                        {
    //                            intTotal = Convert.ToInt32(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]);
    //                        }

    //                        CellTotalAmount.Phrase = new Phrase(Convert.ToString(intTotal), TextBoldformat);
    //                    }
    //                    else
    //                    {
    //                        CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextFontformat);
    //                    }

    //                    pdftable.AddCell(CellTotalAmount);

    //                }// END_ForEach

    //                pdfDoc.Add(pdftable);

    //                Paragraph ParaSpacing = new Paragraph();
    //                ParaSpacing.SpacingBefore = 5;//10

    //                if (strCANRemark != "")
    //                {
    //                    strCANRemark = "** " + strCANRemark;
    //                    pdfDoc.Add(new Paragraph(strCANRemark, FooterFontformat));
    //                }

    //                pdfDoc.Add(ParaSpacing);

    //                pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

    //                pdfDoc.Add(ParaSpacing);

    //                pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

    //                string footerText1 = "1. Shipment consigned to the bank, BRO (Bank Release Order) is Mandatory.\n" +
    //                    "2. Your cargo has not been checked while issuing this notice.\n" +
    //                    "3. Please produce a letter of authority at the time of collection of your documents & D.O.S.\n" +
    //                    "4. Please don't pay duty without checking on complete arrival of goods as per document.\n" +
    //                    "5. This transaction is covered under jurisdiction of Mumbai, Maharashtra. Computer generated document, hence signature not required.\n";

    //                string footerText2 = "E. & O.E.\n";

    //                string footerText3 = "For charges collect shipments, the Delivery Order will be issued against the amount indicated above" +
    //                    " on the receipt of the D.I.C. and Delivery Order will be only issued after the cargo is forwarded to the customs warehouse and the agent is not liable for any claim" +
    //                    " on warehouse charges as a result of delay on part of air carriers to check the freight and issue D.O. Demurrage shall apply as per tariff till customs clearance is effected." +
    //                    " Please note that if the said consignment is not cleared on production of proper documents within 30 days from the date of arrival of the consignment." +
    //                    " it is liable to be disposed of under provision of section 38 & 150 of the custom's Act, 1962.";

    //                string footerText4 = "PAN No:   AAACB0466A";
    //                string footerText5 = "GSTN credit should not be availed, based on cargo arrival notice, separate invoice will be provided." +
    //                    "GST taxes provided is for estimation purpose only";

    //                string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
    //                        "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

    //                pdfDoc.Add(ParaSpacing);
    //                pdfDoc.Add(new Paragraph(footerText1, FooterFontformat));
    //                pdfDoc.Add(new Paragraph(footerText2, TextFontformat));
    //                pdfDoc.Add(new Paragraph(footerText3, FooterFontformat));
    //                pdfDoc.Add(new Paragraph(footerText4, FooterFontformat));
    //                pdfDoc.Add(new Paragraph(footerText5, FooterFontformat));
    //                pdfDoc.Add(ParaSpacing);
    //                pdfDoc.Add(new Paragraph(footerText6, FooterFontformat));

    //                // Footer Image Commented
    //                // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
    //                // footer.SetAbsolutePosition(30, 0);
    //                // pdfDoc.Add(footer);
    //                // pdfwriter.PageEvent = new PDFFooter();

    //                htmlparser.Parse(sr);
    //                pdfDoc.Close();
    //                Response.Write(pdfDoc);
    //                HttpContext.Current.ApplicationInstance.CompleteRequest();

    //            }//END_IF

    //            else
    //            {
    //                lblError.Text = "Please Enter CAN Invoice Details!";
    //                lblError.CssClass = "errorMsg";
    //            }

    //        }//END_Try

    //        catch (Exception ex)
    //        {
    //            lblError.Text = ex.Message;
    //            lblError.CssClass = "errorMsg";
    //        }
    //    }//END_IF
    //    else
    //    {
    //        lblError.Text = "CAN Details Not Found";
    //        lblError.CssClass = "errorMsg";
    //    }
    //}

    #region CAN PDF Copy

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

    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }

        string FileName = fuCANCopy.FileName;
        FileName = FileServer.ValidateFileName(FileName);

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

        if (fuCANCopy.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuCANCopy.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
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