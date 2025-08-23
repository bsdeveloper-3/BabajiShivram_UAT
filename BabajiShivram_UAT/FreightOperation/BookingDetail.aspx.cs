using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;

public partial class FreightOperation_BookingDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingBooking.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Booking Detail";

            DBOperations.FillPackageType(ddPackageType);

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));

            MskValBookingDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            //GenerateBookingNo();

            DBOperations.FillState(ddConsigneeState);
        }
    }

    protected void ddConsigneeState_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        hdnStateCode.Value = "";
        if (ddConsigneeState.SelectedValue != "0")
        {
            DataSet dsGetSateCode = DBOperations.GetStateCodeForLocDetail(Convert.ToInt32(ddConsigneeState.SelectedValue));
            if (dsGetSateCode != null)
            {
                if (dsGetSateCode.Tables[0].Rows[0]["StateCode"] != DBNull.Value)
                    hdnStateCode.Value = dsGetSateCode.Tables[0].Rows[0]["StateCode"].ToString();
            }
        }
    }

    protected void txtGSTN_TextChanged(object sender, EventArgs e)
    {
        btnSave.Enabled = true;
        if (txtGSTN.Text.Trim() != "")
        {
            if (txtGSTN.Text.Length == 15)
            {
                int GSTNo = Convert.ToInt32(txtGSTN.Text.Trim().Substring(0, 2));
                int StateCode = Convert.ToInt32(hdnStateCode.Value);
                if (GSTNo != 0 && StateCode != 0)
                {
                    if (GSTNo != StateCode)
                    {
                        lblError.Text = "GST No does not match with Place Of Supply..!!";
                        lblError.CssClass = "errorMsg";
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                lblError.Text = "GST No should be 15 digit..!!";
                lblError.CssClass = "errorMsg";
                btnSave.Enabled = false;
            }
        }
    }

    #region Freight Detail
    private void SetFreightDetail(int EnqId)
    {
        DataSet dsFreightDetail = DBOperations.GetBookingDetail(EnqId);

        DBOperations.FillFreightAgentCompany(ddAgent, EnqId);

        if (dsFreightDetail.Tables[0].Rows.Count > 0)
        {
            lgtBooking.InnerText = "Booking Confirmation - " + dsFreightDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            hdnModeId.Value = dsFreightDetail.Tables[0].Rows[0]["lMode"].ToString();
            hdnTypeId.Value = dsFreightDetail.Tables[0].Rows[0]["lType"].ToString();

            ddFreightMode.SelectedValue = hdnModeId.Value;
            ddTerms.SelectedValue = dsFreightDetail.Tables[0].Rows[0]["TermsId"].ToString();
            lblEnqNo.Text = dsFreightDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();

            if (dsFreightDetail.Tables[0].Rows[0]["BookingDate"] != DBNull.Value)
            {
                lblAwardeMonth.Text = Convert.ToDateTime(dsFreightDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");
            }
            lblEnqDate.Text = Convert.ToDateTime(dsFreightDetail.Tables[0].Rows[0]["ENQDate"]).ToString("dd/MM/yyyy");
            txtCustomer.Text = dsFreightDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblSalesRep.Text = dsFreightDetail.Tables[0].Rows[0]["SalesRepName"].ToString();
            lblFreightSPC.Text = dsFreightDetail.Tables[0].Rows[0]["EnquiryUser"].ToString();
            lblCountryName.Text = dsFreightDetail.Tables[0].Rows[0]["CountryName"].ToString();
            txtOverseasAgent.Text = dsFreightDetail.Tables[0].Rows[0]["AgentName"].ToString();
            txtGrossWeight.Text = dsFreightDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
            txtChargWeight.Text = dsFreightDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            txtOverseasAgent.Text = dsFreightDetail.Tables[0].Rows[0]["AgentName"].ToString();
            txtConsignee.Text = dsFreightDetail.Tables[0].Rows[0]["Consignee"].ToString();
            txtShipper.Text = dsFreightDetail.Tables[0].Rows[0]["Shipper"].ToString();
            lblCustRefNo.Text = dsFreightDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            txtNoOfPkgs.Text = dsFreightDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();

            if (txtCustomer.Text.Trim() != "")
            {
                CustInFo(txtCustomer.Text.Trim());
            }

            bool IsDangerousGood = Convert.ToBoolean(dsFreightDetail.Tables[0].Rows[0]["IsDangerousGood"]);

            if (IsDangerousGood == true)
                lblDangerousGood.Text = "Yes";
            else
                lblDangerousGood.Text = "No";

            if (dsFreightDetail.Tables[0].Rows[0]["PortOfLoadingId"] != DBNull.Value)
            {
                txtPortLoading.Text = dsFreightDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
                hdnLoadingPortId.Value = dsFreightDetail.Tables[0].Rows[0]["PortOfLoadingId"].ToString();
            }

            if (dsFreightDetail.Tables[0].Rows[0]["PortOfDischargeId"] != DBNull.Value)
            {
                txtPortOfDischarged.Text = dsFreightDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                hdnPortOfDischargedId.Value = dsFreightDetail.Tables[0].Rows[0]["PortOfDischargeId"].ToString();
            }

            if (dsFreightDetail.Tables[0].Rows[0]["lStatus"] != DBNull.Value)
            {
                hdnStatusId.Value = dsFreightDetail.Tables[0].Rows[0]["lStatus"].ToString();
            }

            if (hdnModeId.Value == "1") // AIR
            {
                RFVSea.Enabled = false;
                pnlSea.Visible = false;
            }
            else
            {
                RFVSea.Enabled = true;
                pnlSea.Visible = true;

                txtCont20.Text = dsFreightDetail.Tables[0].Rows[0]["CountOf20"].ToString();
                txtCont40.Text = dsFreightDetail.Tables[0].Rows[0]["CountOf40"].ToString();
                txtLCLVolume.Text = dsFreightDetail.Tables[0].Rows[0]["LCLVolume"].ToString();

                ddContainerType.SelectedValue = dsFreightDetail.Tables[0].Rows[0]["ContainerType"].ToString();
                ddSubType.SelectedValue = dsFreightDetail.Tables[0].Rows[0]["ContainerSubType"].ToString();
            }

            // disable GST calculation if not indian shipment
            if (dsFreightDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
            {
                string CountryCode = dsFreightDetail.Tables[0].Rows[0]["CountryCode"].ToString();
                if (CountryCode.ToLower().Trim() == "india")
                {
                    RFVConsigneeState.Visible = true;
                    RFVGSTN.Visible = true;
                }
                else
                {
                    RFVConsigneeState.Visible = false;
                    RFVGSTN.Visible = false;
                }
            }
            else
            {
                RFVConsigneeState.Visible = false;
                RFVGSTN.Visible = false;
            }

            GetConsigneeAddress(txtConsignee.Text.Trim());

            GetShipperAddress(txtShipper.Text.Trim());
        }
    }

    private void GetConsigneeAddress(string strConsignee)
    {
        txtConsigneeAddress.Text = DBOperations.GetFreightConsigneeAddress(strConsignee);
    }

    private void GetShipperAddress(string strShipper)
    {
        txtShipperAddress.Text = DBOperations.GetFreightShipperAddress(strShipper);
    }

    private string GenerateBookingNo()
    {
        string strJobno = "";

        strJobno = DBOperations.GetNewBookingJobNo(ddBranch.SelectedValue, ddFreightMode.SelectedValue, "1");

        txtJobNo.Text = strJobno;

        return strJobno;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string strBookingNo = lblJobNo.Text.Trim();

        string strBookingNo = txtJobNo.Text.Trim();

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        if (EnqId == 0)
        {
            Response.Redirect("AwaitingBooking.aspx");
        }

        string msg = "";
        if (ddFreightMode.SelectedValue == "2")
        {
            if ((txtCont20.Text == "0" && txtCont40.Text == "0") || (txtCont40.Text == "" && txtCont20.Text == ""))
            {
                msg = "Please enter Cont20 or Cont40" + "<br />";
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "test", "alert('" + msg + "');", true);

        if (msg == "")
        {
            int intFreightMode = Convert.ToInt32(ddFreightMode.SelectedValue); // 1 For Air,2 Sea And 3 Breakbulk

            string strCustomer, strShipper, strConsignee, strShipperAddress, strConsigneeAddress, strAgentName,
                strDescription, strBookingDetails, strGSTN;
            int PortOfLoadingId, PortOfDischargeId, TermsId;
            int NoOfpackages = 0, PackagesType = 0, BranchId = 0, Count20 = 0, Count40 = 0, ContainerTypeId = 0, transportBy = 0, CHABy = 0;
            string strLCLVolume = "0", strGrossWeight = "0", strChargeWeight = "0";
            string strInvoiceNo = "", strPONumber = "", ContainerSubType = "", strTransportBy = "", strCHABy = "";

            int AgentCompId = 0; int ConsigneeStateID = 0;

            DateTime dtInvoiceDate = DateTime.MinValue, dtBookingDate = DateTime.MinValue;

            PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
            PortOfDischargeId = Convert.ToInt32(hdnPortOfDischargedId.Value);

            strCustomer = txtCustomer.Text.ToUpper().Trim();
            strConsignee = txtConsignee.Text.ToUpper().Trim();
            strShipper = txtShipper.Text.ToUpper().Trim();

            if (ddConsigneeState.SelectedValue != "0")
            {
                ConsigneeStateID = Convert.ToInt32(ddConsigneeState.SelectedValue);
            }

            strGSTN = txtGSTN.Text.Trim();
            strConsigneeAddress = txtConsigneeAddress.Text.ToUpper().Trim();

            strShipperAddress = txtShipperAddress.Text.ToUpper().Trim();

            strAgentName = ddAgent.SelectedItem.Text; //txtOverseasAgent.Text.Trim();
            AgentCompId = Convert.ToInt32(ddAgent.SelectedValue);

            TermsId = Convert.ToInt32(ddTerms.SelectedValue);
            BranchId = Convert.ToInt32(ddBranch.SelectedValue);

            strInvoiceNo = txtInvoiceNo.Text.Trim();
            strPONumber = txtPONumber.Text.Trim();
            strDescription = txtDescription.Text.Trim();
            strBookingDetails = txtBookingDetails.Text.Trim();

            int CustId = 0, ConsigneeId = 0, Division = 0, Plant = 0;

            Int32.TryParse(hdnCustId.Value.Trim(), out CustId);
            Int32.TryParse(hdnConsigneeId.Value.Trim(), out ConsigneeId);

            if (CustId == 0)
            {
                lblError.Text = "Invalid Customer. Please Select Customer";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (ConsigneeId == 0)
            {
                lblError.Text = "Invalid Consignee. Please Select Consignee!";
                lblError.CssClass = "errorMsg";
                // return;
            }

            if (ddDivision.SelectedIndex <= 0)
            {
                lblError.Text = "Please Select Customer Division!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                Division = Convert.ToInt32(ddDivision.SelectedValue);
            }

            if (ddPlant.SelectedIndex <= 0)
            {
                lblError.Text = "Please Select Customer Plant!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                Plant = Convert.ToInt32(ddPlant.SelectedValue);
            }

            if (txtNoOfPkgs.Text.Trim() != "")
            {
                NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
                PackagesType = Convert.ToInt32(ddPackageType.SelectedValue);
            }

            if (intFreightMode > 1) // Sea Or Breakbulk
            {
                if (txtCont20.Text.Trim() != "")
                    Count20 = Convert.ToInt32(txtCont20.Text.Trim());

                if (txtCont40.Text.Trim() != "")
                    Count40 = Convert.ToInt32(txtCont40.Text.Trim());

                if (txtLCLVolume.Text.Trim() != "")
                    strLCLVolume = txtLCLVolume.Text.Trim();

                if (ddContainerType.SelectedIndex > 0)
                    ContainerTypeId = Convert.ToInt32(ddContainerType.SelectedValue);

                if (ddSubType.SelectedIndex > 0)
                    ContainerSubType = ddSubType.SelectedValue;
            }

            if (txtGrossWeight.Text.Trim() != "")
            {
                strGrossWeight = txtGrossWeight.Text.Trim();
            }
            if (txtChargWeight.Text.Trim() != "")
            {
                strChargeWeight = txtChargWeight.Text.Trim();
            }

            try
            {
                if (txtInvoiceDate.Text.Trim() != "")
                {
                    dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
                }
            }
            catch (Exception exI)
            {
                lblError.Text = "Invalid Invoice Date! Please Enter Date As DD/MM/YYY";
                lblError.CssClass = "errorMsg";
                return;
            }

            try
            {
                if (txtBookingDate.Text.Trim() != "")
                {
                    dtBookingDate = Commonfunctions.CDateTime(txtBookingDate.Text.Trim());
                }
            }
            catch (Exception exB)
            {
                lblError.Text = "Invalid Booking Date! Please Enter Date As DD/MM/YYY";
                lblError.CssClass = "errorMsg";
                return;
            }

            strTransportBy = txtTransportBy.Text.Trim();
            if (rdlbtnTransport.SelectedValue != "")
            {
                transportBy = Convert.ToInt32(rdlbtnTransport.SelectedValue);
            }
            strCHABy = txtCHABy.Text.Trim();
            if (rdlbtnCHABy.SelectedValue != "")
            {
                CHABy = Convert.ToInt32(rdlbtnCHABy.SelectedValue);
            }

            DataSet dsExistCust = DBOperations.GetCustomerIdByName(txtCustomer.Text);
            if (dsExistCust.Tables[0].Rows.Count > 0)
            {
                int result = DBOperations.AddFreightBooking(strBookingNo, EnqId, intFreightMode, strCustomer, strConsignee, strShipper, strConsigneeAddress, ConsigneeStateID,
                    strGSTN, strShipperAddress, PortOfLoadingId, PortOfDischargeId, TermsId, BranchId, strAgentName, AgentCompId, ContainerTypeId, ContainerSubType, Count20, Count40,
                    strLCLVolume, NoOfpackages, PackagesType, strGrossWeight, strChargeWeight, strInvoiceNo, dtInvoiceDate, strPONumber,
                    strDescription, dtBookingDate, strBookingDetails, CustId, ConsigneeId, Division, Plant,
                    CHABy, strCHABy, strTransportBy, transportBy, LoggedInUser.glUserId);

                if (result == 0)
                {
                    string strReturnMessage = "Booking Confirmation Detail Updated Successfully!";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='AwaitingBooking.aspx';", true);
                    //   Response.Redirect("AwaitingBooking.aspx");
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Freight Job Number Already Exists. Please try again!";
                    lblError.CssClass = "errorMsg";

                    GenerateBookingNo();
                }
                else if (result == 3)
                {
                    lblError.Text = "Freight Booking Already Completed!";
                    lblError.CssClass = "errorMsg";
                }
                else
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }

            }
            else
            {
                lblError.Text = "Please Complete KYC for the customer or Enter KYC customer name";
                lblError.CssClass = "errorMsg";
                btnSave.Visible = false;
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingBooking.aspx");
    }

    public void CustInFo(string Customername)
    {
        DataSet dsGetCustIdByName = DBOperations.GetCustomerIdByNmae(Customername);

        if (dsGetCustIdByName.Tables[0].Rows.Count > 0)
        {
            string strCustId = "";
            strCustId = Convert.ToString(dsGetCustIdByName.Tables[0].Rows[0]["CustomerId"]);
            if (strCustId != "")
            {
                hdnCustId.Value = strCustId;

                int CustomerId = Convert.ToInt32(hdnCustId.Value);
                DBOperations.FillCustomerDivision(ddDivision, CustomerId);
            }
        }
    }

    #endregion

    #region Event

    protected void ddFreightMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnModeId.Value = ddFreightMode.SelectedValue;
        GenerateBookingNo();

        if (hdnModeId.Value == "1") // AIR
        {
            pnlSea.Visible = false;

            ddContainerType.SelectedValue = "0";
            RFVSea.Enabled = false;
        }
        else
        {
            pnlSea.Visible = true;
            RFVSea.Enabled = true;
        }
    }

    protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateBookingNo();
    }

    protected void txtConsignee_TextChanged(object sender, EventArgs e)
    {
        GetConsigneeAddress(txtConsignee.Text.Trim());
    }

    protected void txtShipper_TextChanged(object sender, EventArgs e)
    {
        GetShipperAddress(txtShipper.Text.Trim());
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        if (hdnCustId.Value != "")
        {
            btnSave.Visible = true;
            int CustomerId = Convert.ToInt32(hdnCustId.Value);
            if (txtCustomer.Text.Trim() != "")
            {
                if (CustomerId > 0)
                {
                    DBOperations.FillCustomerDivision(ddDivision, CustomerId);
                }
                else
                {
                    // ListItem lstSelect = new ListItem("-Select-", "0");
                    //ddDivision.Items.Clear();
                    //ddDivision.Items.Add(lstSelect);
                    txtCustomer.Focus();
                }
            }
            else
            {
                //ListItem lstSelect = new ListItem("-Select-", "0");
                //ddDivision.Items.Clear();
                //ddDivision.Items.Add(lstSelect);
                txtCustomer.Focus();
            }
        }
        else
        {
            lblError.Text = "Customer Not Found.Please Select Customer Again";
            lblError.CssClass = "errorMsg";
        }

        DataSet dsExistCust = DBOperations.GetCustomerIdByName(txtCustomer.Text);
        if (dsExistCust.Tables[0].Rows.Count > 0)
        {
            btnSave.Visible = true;
            ddDivision.Enabled = true;
            ddPlant.Enabled = true;
        }
        else
        {
            ddDivision.Enabled = false;
            ddPlant.Enabled = false;
            lblError.Text = "Please Complete KYC for the customer or Enter KYC customer name";
            lblError.CssClass = "errorMsg";
            btnSave.Visible = false;
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);
        DBOperations.FillCustomerPlant(ddPlant, DivisonId);
        ddDivision.Focus();
    }

    protected void ddPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strGSTIN = "";
        if (ddPlant.SelectedValue != "")
        {
            DataSet dsgetPlantDetail = DBOperations.GetExpPlantDetails(Convert.ToInt32(ddPlant.SelectedValue));
            if (dsgetPlantDetail.Tables[0].Rows.Count > 0)
            {
                strGSTIN = dsgetPlantDetail.Tables[0].Rows[0]["GSTNNo"].ToString();
                txtGSTN.Text = strGSTIN;
                txtGSTN.Focus();
            }
        }
    }

    #endregion

    protected void rdlbtnTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlbtnTransport.SelectedValue == "1")
        {
            txtTransportBy.Text = "";
            txtTransportBy.Visible = false;
        }
        else if (rdlbtnTransport.SelectedValue == "2")
        {
            txtTransportBy.Visible = true;
        }
    }

    protected void rdlbtnCHABy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlbtnCHABy.SelectedValue == "1")
        {
            txtCHABy.Text = "";
            txtCHABy.Visible = false;
        }
        else if (rdlbtnCHABy.SelectedValue == "2")
        {
            txtCHABy.Visible = true;
        }
    }


}