using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;

public partial class FreightOperation_ExBookingDetail : System.Web.UI.Page
{

    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnUpload);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("ExAwaitingBooking.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Booking Detail";

            DBOperations.FillPackageType(ddPackageType);

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));

           // MskValBookingDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            GenerateBookingNo();

            DBOperations.FillState(ddConsigneeState);

            if (Convert.ToInt32(Session["lType"]) == 2)
            {
                trExport2.Visible = true;

                if (ddlExportType.SelectedValue == "1")
                {
                    trExport.Visible = false;
                    trExport1.Visible = false;
                }
                else if (ddlExportType.SelectedValue == "2")
                {
                    trExport.Visible = true;
                    trExport1.Visible = true;
                }

            }
            else if (Convert.ToInt32(Session["lType"]) == 2)
            {
                trExport.Visible = false;
                trExport1.Visible = false;
                trExport2.Visible = false;
            }
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

        //btnSave.Enabled = true;
        if (txtGSTN.Text.Trim() != "")
        {
            int GSTNo = Convert.ToInt32(txtGSTN.Text.Trim().Substring(0, 2));
            int StateCode = Convert.ToInt32(hdnStateCode.Value);
            if (GSTNo != 0 && StateCode != 0)
            {
                if (GSTNo != StateCode)
                {
                    lblError.Text = "GSTN does not match with Place Of Supply..!!";
                    lblError.CssClass = "errorMsg";
                    //btnSave.Enabled = false;
                    Session["ErrorMsg"] = lblError.Text;
                }
                else
                {
                    lblError.Text = string.Empty;
                    btnSave.Enabled = true;
                    Session["ErrorMsg"] = "";
                }
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
                        Session["ErrorMsg"] = lblError.Text;
                        //btnSave.Enabled = false;
                    }
                    else
                    {
                        // btnSave.Enabled = true;
                        Session["ErrorMsg"] = "";
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
             //   btnSave.Enabled = false;
            }
        }
    }

    #region Freight Detail
    private void SetFreightDetail(int EnqId)
    {
        DataSet dsFreightDetail = DBOperations.GetBookingDetail(EnqId);

        DBOperations.FillFreightAgentCompany(ddAgent, EnqId);
        DBOperations.FillFrAirLineMaster(ddlAirLine, EnqId);

        if (dsFreightDetail.Tables[0].Rows.Count > 0)
        {
            lgtBooking.InnerText = "Booking Confirmation - " + dsFreightDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            hdnModeId.Value = dsFreightDetail.Tables[0].Rows[0]["lMode"].ToString();
            hdnTypeId.Value = dsFreightDetail.Tables[0].Rows[0]["lType"].ToString();

            hdnUploadPath.Value = dsFreightDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();

            int ModeId = 0;

            if (hdnModeId.Value != "")
            {
                ModeId = Convert.ToInt32(hdnModeId.Value);

                if (ModeId == 1)  // Mode=Air
                {
                    ddlExportType.SelectedValue = "2";   // Docs Stuff                  
                    trExport.Visible = true;
                    trExport1.Visible = true;
                    ddlExportType.Enabled = false;
                    ddlExportType.ForeColor = System.Drawing.Color.Black; ;
                }
                else
                {
                    ddlExportType.SelectedValue = "1";   // Factory Stuff  
                    ddlExportType.Enabled = true;
                    trExport.Visible = false;
                    trExport1.Visible = false;
                }

            }

            //if (hdnTypeId.Value == "1")
            //{
            //    lblConsShip.Text = "Consignee";
            //    lblConsShipAddr.Text = "Consignee Address";

            //    lblShipCons.Text = "Shipper";
            //    lblShipConsAddr.Text = "shipper Address";
            //}
            //else if (hdnTypeId.Value == "2")
            //{
            //    lblConsShip.Text = "Shipper";
            //    lblConsShipAddr.Text = "Shipper Address";

            //    lblShipCons.Text = "Consignee";
            //    lblShipConsAddr.Text = "Consignee Address";
            //}

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
            //txtShipper.Text = dsFreightDetail.Tables[0].Rows[0]["Shipper"].ToString();
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
                // trAirMode.Visible = true;
                //RFVSea.Enabled = false;
                pnlSea.Visible = false;
            }
            else
            {
                // trAirMode.Visible = false;
                ddlAirLine.SelectedValue = "0";
                // RFVSea.Enabled = true;
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

            //GetShipperAddress(txtShipper.Text.Trim());
        }
    }

    private void GetConsigneeAddress(string strConsignee)
    {
        txtConsigneeAddress.Text = DBOperations.GetFreightConsigneeAddress(strConsignee);
    }

    private void GetShipperAddress(int ShipperId)
    {
        txtShipperAddress.Text = DBOperations.FOP_GetShipperAddrByCustId(ShipperId);
    }

    private string GenerateBookingNo()
    {
        string strJobno = "";

        strJobno = DBOperations.GetNewBookingJobNo(ddBranch.SelectedValue, ddFreightMode.SelectedValue, "2");

        txtJobNo.Text = strJobno;

        return strJobno;
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
                DBOperations.FillShipperByCustId(ddlShipper, CustomerId);
            }
        }
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        DataSet dsExistCust = DBOperations.GetCustomerIdByName(txtCustomer.Text);
        if (dsExistCust.Tables[0].Rows.Count > 0)
        {
            btnSave.Visible = true;
            int CustomerId = Convert.ToInt32(hdnCustId.Value);
            if (txtCustomer.Text.Trim() != "")
            {
                if (CustomerId > 0)
                {
                    DBOperations.FillCustomerDivision(ddDivision, CustomerId);
                    DBOperations.FillShipperByCustId(ddlShipper, CustomerId);
                }
                else
                {
                    txtCustomer.Focus();
                }
            }
            else
            {
                txtCustomer.Focus();
            }

        }
        else
        {
            //ddDivision.Enabled = false;
            //ddPlant.Enabled = false;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string strBookingNo = lblJobNo.Text.Trim();

        string strBookingNo = txtJobNo.Text.Trim();

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        if (EnqId == 0)
        {
            Response.Redirect("ExAwaitingBooking.aspx");
        }

        string msg = "";
        if (ddFreightMode.SelectedValue == "2")
        {
            if ((txtCont20.Text == "0" && txtCont40.Text == "0") || (txtCont40.Text == "" && txtCont20.Text == ""))
            {
                msg = "Please enter Cont20 or Cont40" + "<br />";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "test", "alert('" + msg + "');", true);
            }
        }
        

        if(Session["ErrorMsg"] != "")
        {
            lblError.Text= Session["ErrorMsg"].ToString();
            lblError.CssClass = "errorMsg";
        }
        if (msg == "" && lblError.Text=="")
        {


            int intFreightMode = Convert.ToInt32(ddFreightMode.SelectedValue); // 1 For Air,2 Sea And 3 Breakbulk

            string strCustomer, strShipper, strConsignee, strShipperAddress, strConsigneeAddress, strAgentName, strAirLineName,
                strDescription, strBookingDetails, strGSTN;
            int PortOfLoadingId, PortOfDischargeId, TermsId;
            int NoOfpackages = 0, PackagesType = 0, BranchId = 0, Count20 = 0, Count40 = 0, ContainerTypeId = 0;
            string strLCLVolume = "0", strGrossWeight = "0", strChargeWeight = "0";
            string strInvoiceNo = "", strPONumber = "", ContainerSubType = "";
            string strCartingPT = "", strCHABy = "", strTransportBy = "", strOptionId = ""; // Export Module
            int transportBy = 0, CHABy = 0, ExportType = 0;                    // Export Module

            int AgentCompId = 0, AirLineId = 0; int ConsigneeStateID = 0;

            DateTime dtInvoiceDate = DateTime.MinValue, dtBookingDate = DateTime.MinValue;

            PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
            PortOfDischargeId = Convert.ToInt32(hdnPortOfDischargedId.Value);

            strCustomer = txtCustomer.Text.ToUpper().Trim();
            strConsignee = txtConsignee.Text.ToUpper().Trim();
            strShipper = "";// txtShipper.Text.ToUpper().Trim();

            if (ddConsigneeState.SelectedValue != "0")
            {
                ConsigneeStateID = Convert.ToInt32(ddConsigneeState.SelectedValue);
            }
            strGSTN = txtGSTN.Text.Trim();
            strConsigneeAddress = txtConsigneeAddress.Text.ToUpper().Trim();

            strShipperAddress = txtShipperAddress.Text.ToUpper().Trim();

            strOptionId = rblAirMode.SelectedValue;

            strAgentName = ddAgent.SelectedItem.Text; //txtOverseasAgent.Text.Trim();
            AgentCompId = Convert.ToInt32(ddAgent.SelectedValue);

            strAirLineName = ddlAirLine.SelectedItem.Text; //txtOverseasAgent.Text.Trim();
            AirLineId = Convert.ToInt32(ddlAirLine.SelectedValue);

            TermsId = Convert.ToInt32(ddTerms.SelectedValue);
            BranchId = Convert.ToInt32(ddBranch.SelectedValue);

            strInvoiceNo = txtInvoiceNo.Text.Trim();
            strPONumber = txtPONumber.Text.Trim();
            strDescription = txtDescription.Text.Trim();
            strBookingDetails = txtBookingDetails.Text.Trim();

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

            // Export Operation

            strCartingPT = txtCartingPoint.Text.Trim();
            strCHABy = txtCHABy.Text.Trim();
            strTransportBy = txtTransportBy.Text.Trim();
            if (rdlbtnTransport.SelectedValue != "")
            {
                transportBy = Convert.ToInt32(rdlbtnTransport.SelectedValue);
            }
            if (rdlbtnCHABy.SelectedValue != "")
            {
                CHABy = Convert.ToInt32(rdlbtnCHABy.SelectedValue);
            }
            // End Export Operation

            int ltype = Convert.ToInt32(hdnTypeId.Value);
            int IsValid = 0;

            if (ltype == 2)
            {
                ExportType = Convert.ToInt32(ddlExportType.SelectedValue);

                if (ExportType == 2)
                {
                    if (txtCartingPoint.Text == "" || rdlbtnCHABy.SelectedValue == "" || rdlbtnTransport.SelectedValue == "")
                    {
                        IsValid++;
                        string strReturnMessage = " Enter the Carting Point, CHA By and Transportation By";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "');", true);
                    }
                }

                if (rdlbtnCHABy.SelectedValue == "2" && txtCHABy.Text == "")
                {
                    IsValid++;
                    string strReturnMessage = " Enter the CHA Name";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "');", true);
                }
            }

            int CustId = 0, ShipperId = 0, Division = 0, Plant = 0;

            Int32.TryParse(hdnCustId.Value.Trim(), out CustId);
            Int32.TryParse(hdnShipperId.Value.Trim(), out ShipperId);

            if (CustId == 0)
            {
                lblError.Text = "Invalid Customer. Please Select Customer";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (ShipperId == 0)
            {
                lblError.Text = "Invalid Shipper. . Please Select Shipper";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (gvFreightDocument.Rows.Count < 2)
            {
                lblError.Text = "Please Upload Booking Copy";
                lblError.CssClass = "errorMsg";
                return;
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

            if (IsValid == 0)
            {
                DataSet dsExistCust = DBOperations.GetCustomerIdByName(txtCustomer.Text);
                if (dsExistCust.Tables[0].Rows.Count > 0)
                {
                    int result = DBOperations.AddFreightBookingExport(strBookingNo, EnqId, intFreightMode, strCustomer, strConsignee, strShipper, strConsigneeAddress, ConsigneeStateID,
                              strGSTN, strShipperAddress, PortOfLoadingId, PortOfDischargeId, TermsId, BranchId, strAgentName, AgentCompId, AirLineId, ContainerTypeId, ContainerSubType, Count20, Count40,
                              strLCLVolume, NoOfpackages, PackagesType, strGrossWeight, strChargeWeight, strInvoiceNo, dtInvoiceDate, strPONumber, Division, Plant, strOptionId,
                              strDescription, dtBookingDate, CustId, ShipperId, strBookingDetails, ExportType, strCartingPT, CHABy, strCHABy, strTransportBy, transportBy, ltype, LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        string strReturnMessage = "Booking Confirmation Detail Updated Successfully!";

                        SendFundRequestMail(EnqId);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='ExAwaitingBooking.aspx';", true);
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
                    ddDivision.Enabled = false;
                    ddPlant.Enabled = false;
                    lblError.Text = "Please Complete KYC for the customer or Enter KYC customer name";
                    lblError.CssClass = "errorMsg";
                    btnSave.Visible = false;
                }
            }
        }
        else
        {
            lblError.Text = lblError.Text;

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExAwaitingBooking.aspx");
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
            // trAirMode.Visible = true;
            //RFVSea.Enabled = false;
            ddlExportType.SelectedValue = "2";   // Docs Stuff                  
            trExport.Visible = true;
            trExport1.Visible = true;
            ddlExportType.Enabled = false;
            ddlExportType.ForeColor = System.Drawing.Color.Black; ;
        }
        else
        {
            pnlSea.Visible = true;
            // trAirMode.Visible = false;
            //ddlAirLine.SelectedValue = "0";
            //RFVSea.Enabled = true;
            ddlExportType.SelectedValue = "1";   // Factory Stuff  
            ddlExportType.Enabled = true;
            trExport.Visible = false;
            trExport1.Visible = false;
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

    //protected void txtShipper_TextChanged(object sender, EventArgs e)
    //{
    //    //GetShipperAddress(txtShipper.Text.Trim());
    //}

    #endregion

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

    protected void rblAirMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAirMode.SelectedValue == "1")
        {
            ddAgent.Visible = false;
            lblAgent.Visible = false;

            ddlAirLine.Visible = true;
            lblAirLine.Visible = true;
        }
        else if (rblAirMode.SelectedValue == "2")
        {
            ddAgent.Visible = true;
            lblAgent.Visible = true;

            ddlAirLine.Visible = false;
            lblAirLine.Visible = false;
        }
    }

    #region Freight Document

    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "removedocument")
        {
            int DocId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightDocument(DocId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Document deleted successfully!";
                lblError.CssClass = "success";

                gvFreightDocument.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Document details not found";
                lblError.CssClass = "errorMsg";

                gvFreightDocument.DataBind();
            }
        }
    }

    protected void btnFileUpload_Click(Object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int DocResult = -123;

        //if (txtDocName.Text.Trim() == "")
        //{
        //    lblError.Text = "Please enter the document name.";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

        if (EnqId > 0) // If EnquiryId Session Not Expired.Update Status Details
        {
            if (fuDocument.HasFile) // Add Enquiry Document
            {
                string strDocFolder = "FreightDoc\\" + hdnUploadPath.Value + "\\";

                string strFilePath = UploadDocument(strDocFolder);
                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddFreightDocument(EnqId, ddl_DocumentType.SelectedItem.ToString(), strFilePath, LoggedInUser.glUserId);

                    if (DocResult == 0)
                    {
                        lblError.Text = "Document uploaded successfully.";
                        lblError.CssClass = "success";
                        gvFreightDocument.DataBind();
                        //gvFreightAttach.DataBind();
                    }
                    else if (DocResult == 1)
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (DocResult == 2)
                    {
                        lblError.Text = "Document Name Already Exists! Please change the document name!.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }//END_IF_FileExists
            else
            {
                lblError.Text = "Please attach the document for upload!";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF_Enquiry
        else
        {
            Response.Redirect("FreightTracking.aspx");
        }

    }

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
        string FileName = fuDocument.FileName;
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

        // ServerFilePath = "C:\\inetpub\\wwwroot\\BabajiShivram\\UploadFiles\\" + FilePath;

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

    protected void ddlShipper_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShipper.SelectedIndex > 0)
        {
            int shipperId = Convert.ToInt32(ddlShipper.SelectedValue);
            GetShipperAddress(shipperId);

            hdnShipperId.Value = ddlShipper.SelectedValue;
        }
        else
        {
            txtShipperAddress.Text = "";
            hdnShipperId.Value = "";
        }
    }

    protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExportType.SelectedValue == "1")  // factory Stuff
        {
            trExport.Visible = false;
            trExport1.Visible = false;

            txtCartingPoint.Text = "";
            rdlbtnCHABy.SelectedValue = null;
            rdlbtnTransport.SelectedValue = null;

        }
        else if (ddlExportType.SelectedValue == "2")  // Doc Stuff
        {
            trExport.Visible = true;
            trExport1.Visible = true;
        }
    }


    private bool SendFundRequestMail(int EnqId)
    {
        string MessageBody = "", strTOEmail = "", strCCEmail = "", strSubject = "", JobRefno = "", BranchName = "", PaymentTypeName = "", ConsigneeName = "",
            ExpenseTypeName = "", Amount = "", PaidTo = "", Remark = "", CreatedBy = "", CreatedByEmail = "", HoldedBy = "", HoldedByEmail = "", HoldReason = "";
        bool bEmailSucces = false;
        string strEnqRefNo = "", FRJobNo = "", strShipper = "", strCustomer = "", strConsignee = "", strPortLoading = "", strPortDischarge = "";
        string strNoOfPkgs = "", strGrossWT = "", strInvoiceNo = "", strPONo = "", ExportType = "", CartingPoint = "", CHABy = "",
                    TransportBy = "", BookingDate = "", strCustRefNo = "", ExportTypeId = "";
        StringBuilder strbuilder = new StringBuilder();

        try
        {
            if (EnqId != 0)
            {
                DataSet dsgetBookingDetail = DBOperations.GetExpBookingDetail(EnqId);
                if (dsgetBookingDetail != null)
                {
                    strEnqRefNo = dsgetBookingDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
                    FRJobNo = dsgetBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
                    strShipper = dsgetBookingDetail.Tables[0].Rows[0]["ShipperName"].ToString();
                    strCustomer = dsgetBookingDetail.Tables[0].Rows[0]["Customer"].ToString();
                    strCustRefNo = dsgetBookingDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
                    strConsignee = dsgetBookingDetail.Tables[0].Rows[0]["Consignee"].ToString();
                    strPortLoading = dsgetBookingDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
                    strPortDischarge = dsgetBookingDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                    //strVesselName = dsgetBookingDetail.Tables[0].Rows[0]["VesselName"].ToString();
                    //strVesselName += " - " + dsgetBookingDetail.Tables[0].Rows[0]["VesselNumber"].ToString();
                    strNoOfPkgs = dsgetBookingDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
                    strGrossWT = dsgetBookingDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
                    strInvoiceNo = dsgetBookingDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    strPONo = dsgetBookingDetail.Tables[0].Rows[0]["PONumber"].ToString();
                    //strDescription = dsgetBookingDetail.Tables[0].Rows[0]["CargoDescription"].ToString(); 
                    ExportType = dsgetBookingDetail.Tables[0].Rows[0]["ExportTypeName"].ToString();
                    CartingPoint = dsgetBookingDetail.Tables[0].Rows[0]["CartingPoint"].ToString();
                    CHABy = dsgetBookingDetail.Tables[0].Rows[0]["CHAByName"].ToString();
                    TransportBy = dsgetBookingDetail.Tables[0].Rows[0]["TransportByName"].ToString();
                    ExportTypeId = dsgetBookingDetail.Tables[0].Rows[0]["ExportType"].ToString();

                    if (dsgetBookingDetail.Tables[0].Rows[0]["BookingDate"] != DBNull.Value)
                    {
                        BookingDate = Convert.ToDateTime(dsgetBookingDetail.Tables[0].Rows[0]["BookingDate"]).ToString("dd/MM/yyyy");
                    }


                    strTOEmail = CreatedByEmail;// "developer@babajishivram.com";// jr.developer@babajishivram.com";
                    CreatedByEmail = LoggedInUser.glUserName;
                    CreatedBy = LoggedInUser.glEmpName;

                    strCCEmail = CreatedByEmail + ", amit.bakshi@babajishivram.com";
                    strSubject = "Export Booking - Enquiry No : " + strEnqRefNo + " // Job No:" + FRJobNo + " // " + strCustomer + " // " + strPortLoading + " // " + strPortDischarge;

                    if (strTOEmail == "" || strSubject == "")
                        return bEmailSucces;
                    else
                    {
                        MessageBody = "Dear Sir,<BR><BR> Please find the below Export Booking details .<BR><BR>";

                        if (ExportTypeId == "1")
                        {

                            strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:60%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Freight Job No</th><td style='padding-left: 3px'>" + FRJobNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Enquiry No</th><td style='padding-left: 3px'>" + strEnqRefNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Customer</th><td style='padding-left: 3px'>" + strCustomer + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Customer Ref No</th><td style='padding-left: 3px'>" + strCustRefNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Shipper</th><td style='padding-left: 3px'>" + strShipper + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Consignee</th><td style='padding-left: 3px'>" + strConsignee + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Port Of Loading</th><td style='padding-left: 3px'>" + strPortLoading + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Port Of Discharge</th><td style='padding-left: 3px'>" + strPortDischarge + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>No. Of Package</th><td style='padding-left: 3px'>" + strNoOfPkgs + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Gross Weight</th><td style='padding-left: 3px'>" + strGrossWT + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Invoice No</th><td style='padding-left: 3px'>" + strInvoiceNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>PO No</th><td style='padding-left: 3px'>" + strPONo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Export Type</th><td style='padding-left: 3px'>" + ExportType + "</td></tr>");

                            strbuilder = strbuilder.Append("</table>");
                        }
                        if (ExportTypeId == "2")
                        {
                            strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:60%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Freight Job No</th><td style='padding-left: 3px'>" + FRJobNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Enquiry No</th><td style='padding-left: 3px'>" + strEnqRefNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Customer</th><td style='padding-left: 3px'>" + strCustomer + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Customer Ref No</th><td style='padding-left: 3px'>" + strCustRefNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Shipper</th><td style='padding-left: 3px'>" + strShipper + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Consignee</th><td style='padding-left: 3px'>" + strConsignee + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Port Of Loading</th><td style='padding-left: 3px'>" + strPortLoading + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Port Of Discharge</th><td style='padding-left: 3px'>" + strPortDischarge + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>No. Of Package</th><td style='padding-left: 3px'>" + strNoOfPkgs + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Gross Weight</th><td style='padding-left: 3px'>" + strGrossWT + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Invoice No</th><td style='padding-left: 3px'>" + strInvoiceNo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>PO No</th><td style='padding-left: 3px'>" + strPONo + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Export Type</th><td style='padding-left: 3px'>" + ExportType + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Carting Point</th><td style='padding-left: 3px'>" + CartingPoint + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>CHA By</th><td style='padding-left: 3px'>" + CHABy + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Transport By</th><td style='padding-left: 3px'>" + TransportBy + "</td></tr>");
                            strbuilder = strbuilder.Append("</table>");
                        }

                        MessageBody = MessageBody + strbuilder;
                        MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + CreatedBy;
                        MessageBody = MessageBody + "<BR>Babaji Shivram Clearing & Carriers Pvt Ltd";
                        MessageBody = MessageBody + "<BR><BR><b><Note* - <I>This is system generated mail, please do not reply to this message via e-mail. </I></b>";

                        System.Collections.Generic.List<string> lstFilePath = new List<string>();
                        DataSet dsGetDocDetailsExp = DBOperations.GetDocumentDetailsExport(EnqId);
                        if (dsGetDocDetailsExp != null && dsGetDocDetailsExp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGetDocDetailsExp.Tables[0].Rows.Count; i++)
                            {
                                if (dsGetDocDetailsExp.Tables[0].Rows[i]["DocPath"] != DBNull.Value)
                                {
                                    //lstFilePath.Add("ExpenseUpload\\" + dsGetDocDetailsExp.Tables[0].Rows[i]["DocPath"].ToString());
                                    lstFilePath.Add(dsGetDocDetailsExp.Tables[0].Rows[i]["DocPath"].ToString());
                                }
                            }
                        }

                        bEmailSucces = EMail.SendMailMultiAttach(strTOEmail, strTOEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
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

    //IEnumerable<Employee> ParseEmployeeTable(DataTable dtEmployees)
    //{
    //    var employees = new ConcurrentBag<Employee>();

    //    Parallel.ForEach(dtEmployees.AsEnumerable(), (dr) =>
    //    {
    //        employees.Add(new Employee()
    //        {
    //            _FirstName = dr["FirstName"].ToString(),
    //            _LastName = dr["Last_Name"].ToString()
    //        });
    //    });

    //    return employees;
    //}

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


}