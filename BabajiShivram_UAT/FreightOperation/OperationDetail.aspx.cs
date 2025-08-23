using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QueryStringEncryption;
using System.Data;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Ionic.Zip;
using BSImport.CountryManager.BO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


public partial class FreightOperation_OperationDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    static string JobRefNO = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(fvCAN);
        ScriptManager1.RegisterPostBackControl(fvDelivery);
        ScriptManager1.RegisterPostBackControl(FVFreightDetail);
        ScriptManager1.RegisterPostBackControl(lnkCreateDebitNote);
        ScriptManager1.RegisterPostBackControl(fvAgentInvoice);
        ScriptManager1.RegisterPostBackControl(fvAdvice);
        ScriptManager1.RegisterPostBackControl(gvBillDispatchDocDetail);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(gvTruckRequest);

        //lblError.Text = Session["EnqId"].ToString() + "@@";
        if (Session["EnqId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Session Expired! Please try again');</script>", false);
            Response.Redirect("OperationTrack.aspx");
        }

        if (!IsPostBack)
        {
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));

                DBOperations.FillFreightAgentCompany(ddAgentEnq, Convert.ToInt32(Session["EnqId"]));

                if(ddAgentEnq.Items.Count >= 1)
                {
                    ddAgentEnq.SelectedIndex = 1;
                }

            }
            else
            {

                Response.Redirect("OperationTrack.aspx");
            }
        }
        // Allow  Only Future Date For Reminder
        calVehiclePlaceDate.StartDate = DateTime.Today;

        // Set the minimum value for the MaskedEditValidator to today's date
        mevVehiclePlaceDate.MinimumValue = DateTime.Today.ToString("dd/MM/yyyy");                        //changes For calender
    }

    #region Get Operation Detail
    private void GetFreightDetail(int Enqid)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Freight Operation Detail

        DataSet dsDetail = DBOperations.GetOperationDetail(Enqid);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            fldFundRequest.Visible = false;

            if (dsDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = dsDetail.Tables[0].Rows[0]["CountryCode"].ToString();
            }

            // Allow Vendor Fund Request
            if (LoggedInUser.glUserId == 81 || LoggedInUser.glUserId == 1) // Billing Dept HOD
            {
                if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsFundAllowed"]) == true)
                {
                    rblFundRequest.SelectedValue = "1";
                }
                else
                {
                    rblFundRequest.SelectedValue = "0";
                }

                fldFundRequest.Visible = true;
                accExpense.Visible = true;
                btnAllowFundRequest.Enabled = true;
            }

            //int ddDivision1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["Division"]);
            //int ddPlant1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["Plant"]);
            int CustomerId1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["CustomerId"]);
            int CUST = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["CUST"]);
            Session["CustId"] = CUST;
            DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;
            if (dddiv != null)
            {
                DBOperations.FillCustomerDivision(dddiv, CustomerId1);
            }

            //if (dsDetail.Tables[0].Rows[0]["lStatus"] == "16")
            //{
            //    fvCAN.Enabled = true;
            //}
            //else
            //{
            //     fvCAN.Enabled = false;
            //}

            FVFreightDetail.DataSource = dsDetail;
            FVFreightDetail.DataBind();

            fvAgentPreAlert.DataSource = dsDetail;
            fvAgentPreAlert.DataBind();

            fvCustomerPreAlert.DataSource = dsDetail;
            fvCustomerPreAlert.DataBind();

            fvCAN.DataSource = dsDetail;
            fvCAN.DataBind();

            fvDelivery.DataSource = dsDetail;
            fvDelivery.DataBind();

            fvAdvice.DataSource = dsDetail;
            fvAdvice.DataBind();

            fvAgentInvoice.DataSource = dsDetail;
            fvAgentInvoice.DataBind();

            hdnFRJobNo.Value = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            lblTitle.Text = "Operation Detail - " + dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            lblTitle.Text = "Operation Detail - " + dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            hdnWeight.Value = dsDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            hdnVolume.Value = dsDetail.Tables[0].Rows[0]["LCLVolume"].ToString();

            if (dsDetail.Tables[0].Rows[0]["PortOfLoadingId"] != DBNull.Value)
            {
                hdnLoadingPortId.Value = dsDetail.Tables[0].Rows[0]["PortOfLoadingId"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["PortOfDischargeId"] != DBNull.Value)
            {
                hdnPortOfDischargedId.Value = dsDetail.Tables[0].Rows[0]["PortOfDischargeId"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["IsGST"] != DBNull.Value)
            {
                Label lblTaxType = (Label)fvCAN.FindControl("lblTaxType");

                if (lblTaxType != null)
                {
                    if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsGST"]) == true)
                    {
                        hdnIsGST.Value = "1";
                        txtTaxRate.Text = "";
                        lblTaxType.Text = "GST";
                    }
                    else
                    {
                        hdnIsGST.Value = "0";
                        lblTaxType.Text = "Service Tax";
                        txtTaxRate.Text = "15.00";
                    }
                }
            }

            Panel pnlCancel = (Panel)FVFreightDetail.FindControl("pnlCancel");
            if (dsDetail.Tables[0].Rows[0]["REASON"].ToString() != "0")
            {
                pnlCancel.Visible = true;
            }

            hdnPrevPOS.Value = dsDetail.Tables[0].Rows[0]["ConsigneeStateName"].ToString();
            hdnGSTNo.Value = dsDetail.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();
            txtMailCC.Text = dsDetail.Tables[0].Rows[0]["EnquiryEmail"].ToString() + "," + LoggedInUser.glUserName + ",manish.radhakrishnan@babajishivram.com";
            hdnUploadPath.Value = dsDetail.Tables[0].Rows[0]["DocDir"].ToString();

            hdnModeId.Value = dsDetail.Tables[0].Rows[0]["lMode"].ToString();

            if (dsDetail.Tables[0].Rows[0]["BranchID"] != DBNull.Value)
            {
                int BranchID = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["BranchID"]);

                if (BranchID == (Int32)EnumBranch.Delhi)
                {
                    hdnBranchEmail.Value = ",amit.thakur@babajishivram.com,sravinder@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Chennai)
                {
                    hdnBranchEmail.Value = ",vijayalakshmi@babajishivram.com, thaniga@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Ahmedabad)
                {
                    hdnBranchEmail.Value = ",mithun.sharma@babajishivram.com";
                }

                txtMailCC.Text = txtMailCC.Text + hdnBranchEmail.Value;
            }

            if (Convert.ToInt32(hdnModeId.Value) > 1)
            {
                Panel pnlSea = (Panel)FVFreightDetail.FindControl("pnlSea");

                if (pnlSea != null)
                    pnlSea.Visible = true;
            }

            // Fill Consignee State

            DropDownList ddConsigneeState = (DropDownList)FVFreightDetail.FindControl("ddConsigneeState");

            if (ddConsigneeState != null)
            {
                // Fill State List

                DBOperations.FillState(ddConsigneeState);

                if (dsDetail.Tables[0].Rows[0]["ConsigneeStateID"] != DBNull.Value)
                {
                    string strStateID = dsDetail.Tables[0].Rows[0]["ConsigneeStateID"].ToString();

                    ddConsigneeState.SelectedValue = strStateID;
                }

            }

            // Fill Agent Master

            DropDownList ddAgent = (DropDownList)FVFreightDetail.FindControl("ddAgent");

            if (ddAgent != null)
            {
                // Fill Agent List By Enq ID
                DBOperations.FillFreightAgentCompany(ddAgent, Enqid);

                // Fill All Freight Agent List
                //DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));

                if (dsDetail.Tables[0].Rows[0]["AgentID"] != DBNull.Value)
                {
                    string strAgentID = dsDetail.Tables[0].Rows[0]["AgentID"].ToString();

                    ddAgent.SelectedValue = strAgentID;
                }

            }
            // Fill Pkgs Type

            DropDownList ddPackageType = (DropDownList)FVFreightDetail.FindControl("ddPackageType");

            if (ddPackageType != null)
            {
                string strPackageTypeId = dsDetail.Tables[0].Rows[0]["PackageTypeId"].ToString();

                DBOperations.FillPackageType(ddPackageType);

                ddPackageType.SelectedValue = strPackageTypeId;
            }

            // Fill Final Agent

            DropDownList ddFinalAgent = (DropDownList)fvAgentPreAlert.FindControl("ddFinalAgent");

            if (ddFinalAgent != null)
            {
                // Fill Agent List By Enq ID
                DBOperations.FillCompanyByCategory(ddFinalAgent, 5);

                if (dsDetail.Tables[0].Rows[0]["FinalAgentID"] != DBNull.Value)
                {
                    if (dsDetail.Tables[0].Rows[0]["FinalAgentID"] != DBNull.Value)
                    {
                        string strFinalAgentID = dsDetail.Tables[0].Rows[0]["FinalAgentID"].ToString();

                        ddFinalAgent.SelectedValue = strFinalAgentID;
                    }
                }

            }

            EditBtnInvisible();
        }
    }

    protected void EditBtnInvisible()
    {
        DataSet dsDetail = DBOperations.GetOperationDetail(Convert.ToInt32(Session["EnqId"]));
        if (dsDetail.Tables[0].Rows[0]["Remark"] != DBNull.Value)
        {
            Button btnEditFreightDetail = (Button)FVFreightDetail.FindControl("btnEditFreightDetail");
            if (btnEditFreightDetail != null)
            {
                btnEditFreightDetail.Visible = false;
            }

            Button btnEditAgentPreAlert = (Button)fvAgentPreAlert.FindControl("btnEditAgentPreAlert");
            if (btnEditAgentPreAlert != null)
            {
                btnEditAgentPreAlert.Visible = false;
            }

            Button btnEditCustomerPreAlert = (Button)fvCustomerPreAlert.FindControl("btnEditCustomerPreAlert");
            if (btnEditCustomerPreAlert != null)
            {
                btnEditCustomerPreAlert.Visible = false;
            }

            Button btnEditCAN = (Button)fvCAN.FindControl("btnEditCAN");
            if (btnEditCAN != null)
            {
                btnEditCAN.Visible = false;
            }

            btnSaveInvoice.Visible = false;

            Button btnEditDelivery = (Button)fvDelivery.FindControl("btnEditDelivery");
            if (btnEditDelivery != null)
            {
                btnEditDelivery.Visible = false;
            }

            Button btnEditAgentInvoice = (Button)fvAgentInvoice.FindControl("btnEditAgentInvoice");
            if (btnEditAgentInvoice != null)
            {
                btnEditAgentInvoice.Visible = false;
            }

            btnUpload.Visible = false;
            btnAddContainer.Visible = false;
            btnSaveActivity.Visible = false;
            btnSaveDebitnote.Visible = false;
        }
    }

    public string GetBooleanToYesNo(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Yes";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "No";
        }

        return strReturnText;
    }

    public bool CheckNullBooleanToTrueFalse(object mybValue)
    {
        bool bReturnValue = false;

        if (mybValue != DBNull.Value)
        {
            bReturnValue = Convert.ToBoolean(mybValue);
        }

        return bReturnValue;
    }

    public string GetBooleanToCompletedPending(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Completed";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Pending";
        }

        return strReturnText;
    }

    public string GetBooleanToApprovedRejected(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Approved";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Rejected";
        }

        return strReturnText;
    }

    #endregion

    #region Customer PreAlert

    protected void lnkPreAlertEmailDraft_Click(object sender, EventArgs e)
    {
        btnSendEmail.Enabled = true;
        GenerateEmailDraft();
    }

    private void GenerateEmailDraft()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        //lblCustomerEmail.Text = ((Label)fvCustomerPreAlert.FindControl("lblEmailSentTo")).Text;
        
        lblCustomerEmail.Text = ((TextBox)fvCustomerPreAlert.FindControl("txtViewCustomerEmail")).Text.Trim();

        txtSubject.Text = "Customer PreAlert -" + ((Label)FVFreightDetail.FindControl("lblFRJobNo")).Text.Trim();

        string EmailContent = "";
        string MessageBody = "";
        string strEnqRefNo = "", strHAWNo = "", strMAWNo = "", strShipper = "", strConsignee = "", strPortLoading = "",
            strPortDischarge = "", strVesselName = "", strETADate = "", strNoOfPkgs = "",
            strGrossWT = "", strInvoiceNo = "", strPONo = "", strDescription = "", strFinalAgent = "";

        string strGSTN = "", strPlaceOfSupply = "";

        DataSet dsAlertDetail = DBOperations.GetCustomerPreAlertDetail(EnqId);

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strHAWNo = dsAlertDetail.Tables[0].Rows[0]["HBLNo"].ToString();
            strMAWNo = dsAlertDetail.Tables[0].Rows[0]["MBLNo"].ToString();
            strShipper = dsAlertDetail.Tables[0].Rows[0]["Shipper"].ToString();
            strConsignee = dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strPortLoading = dsAlertDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
            strPortDischarge = dsAlertDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            strVesselName = dsAlertDetail.Tables[0].Rows[0]["VesselName"].ToString();
            strVesselName += " - " + dsAlertDetail.Tables[0].Rows[0]["VesselNumber"].ToString();

            strNoOfPkgs = dsAlertDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            strGrossWT = dsAlertDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
            strInvoiceNo = dsAlertDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            strPONo = dsAlertDetail.Tables[0].Rows[0]["PONumber"].ToString();
            strDescription = dsAlertDetail.Tables[0].Rows[0]["CargoDescription"].ToString();
            strEnqRefNo = dsAlertDetail.Tables[0].Rows[0]["EnqRefNo"].ToString();
            strFinalAgent = dsAlertDetail.Tables[0].Rows[0]["FinalAgent"].ToString();

            strGSTN = dsAlertDetail.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();
            strPlaceOfSupply = dsAlertDetail.Tables[0].Rows[0]["ConsigneeState"].ToString();

            if (dsAlertDetail.Tables[0].Rows[0]["ETA"] != DBNull.Value)
                strETADate = Convert.ToDateTime(dsAlertDetail.Tables[0].Rows[0]["ETA"]).ToString("dd/MM/yyyy");
        }
        else
        {
            lblError.Text = "Booking Details Not Found! Please check details.";
            lblError.CssClass = "errorMsg";
            return;
        }
        try
        {
            string strFileName = "../EmailTemplate/FOP_EmailCustPreAlert.txt";

            StreamReader sr = new StreamReader(Server.MapPath(strFileName));
            sr = File.OpenText(Server.MapPath(strFileName));
            EmailContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
            return;
        }

        MessageBody = EmailContent.Replace("@HAWNumber", strHAWNo);
        MessageBody = MessageBody.Replace("@Shipper", strShipper);
        MessageBody = MessageBody.Replace("@CONSIGNEE", strConsignee);
        MessageBody = MessageBody.Replace("@MAWNumber", strMAWNo);
        MessageBody = MessageBody.Replace("@PortOfLoading", strPortLoading);
        MessageBody = MessageBody.Replace("@PortOfDischarge", strPortDischarge);
        MessageBody = MessageBody.Replace("@VesselName", strVesselName);
        MessageBody = MessageBody.Replace("@ETADate", strETADate);
        MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPkgs);
        MessageBody = MessageBody.Replace("@GrossWeight", strGrossWT);
        MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
        MessageBody = MessageBody.Replace("@PONumber", strPONo);
        MessageBody = MessageBody.Replace("@Description", strDescription);
        MessageBody = MessageBody.Replace("@EnqRefNo", strEnqRefNo);
        MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);
        MessageBody = MessageBody.Replace("@FinalAgent", strFinalAgent);

        MessageBody = MessageBody.Replace("@GSTN", strGSTN);
        MessageBody = MessageBody.Replace("@StateName", strPlaceOfSupply);

        divPreviewEmail.InnerHtml = MessageBody;

        ModalPopupEmail.Show();
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        if (lblCustomerEmail.Text.Trim() == "")
        {
            lblPopMessageEmail.Text = "Please Update Customer Email!";
            lblPopMessageEmail.CssClass = "errorMsg";
            ModalPopupEmail.Show();
        }
        else
        {
            btnSendEmail.Enabled = false;
            // Email Sending
            bool bEmailSuccess = SendPreAlertEmail();

            // Update PreAlert Email Sent Status and Customer Email 

            if (bEmailSuccess == true)
            {
                int Result = DBOperations.UpdateCustomerPreAlertEmailStatus(EnqId, lblCustomerEmail.Text.Trim(), LoggedInUser.glUserId);

                ModalPopupEmail.Hide();

                lblError.Text = "Customer Pre AlertEmail Sent Successfully!";
                lblError.CssClass = "success";

                GetFreightDetail(EnqId);
            }
            else
            {
                lblError.Text = "Email Sending Failed! Please check Comma-Seperated Email Addresses";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    private bool SendPreAlertEmail()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        bool bEmailSucces = false;

        string MessageBody = "", strSubject = "";

        string strCustomerEmail = lblCustomerEmail.Text.Trim();

        string strCCEmail = txtMailCC.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", "");
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();
        strCCEmail = strCCEmail.TrimEnd(',');

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
        strCustomerEmail = strCustomerEmail.TrimEnd(',');

        strSubject = txtSubject.Text.Trim();

        if (strCustomerEmail == "" || strSubject == "")
        {
            lblError.Text = "Please Enter Customer Email!";
            lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            MessageBody = divPreviewEmail.InnerHtml;

            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvFreightAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }

    protected void btnCancelPreAlertEmail_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }
    #endregion

    #region BOOKING DETAIL FORMVIEW EVENTS
    protected void FVFreightDetail_DataBound(object sender, EventArgs e)
    {
        if (FVFreightDetail.CurrentMode == FormViewMode.Edit)
        {
            DataRowView drv = (DataRowView)FVFreightDetail.DataItem;
            if (drv["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = drv["CountryCode"].ToString();
                if (drv["CountryCode"].ToString().ToLower().Trim() == "india")
                {
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = true;
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = true;
                }
                else
                {
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = false;
                    // ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = false;
                }
            }
            else
            {
                ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = false;
                ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = false;
            }

            int CustomerId = 0;
            TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
            DataSet dsGetCustIdByName = DBOperations.GetCustomerIdByName(CustName.Text);

            if (dsGetCustIdByName.Tables[0].Rows.Count > 0)
            {
                string strCustId = "";
                strCustId = Convert.ToString(dsGetCustIdByName.Tables[0].Rows[0]["CustomerId"]);
                if (strCustId != "")
                {
                    CustomerId = Convert.ToInt32(strCustId);
                }
            }
            DropDownList ddDiv = (DropDownList)FVFreightDetail.FindControl("ddDivision");
            DBOperations.FillCustomerDivision(ddDiv, CustomerId);
            DataSet dsDetail = DBOperations.GetOperationDetail(Convert.ToInt32(Session["EnqId"]));
            int Divid = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["Division"].ToString());
            ddDiv.SelectedValue = Divid.ToString();

            DropDownList ddPlant = (DropDownList)FVFreightDetail.FindControl("ddPlant");
            DBOperations.FillCustomerPlant(ddPlant, Divid);
            int Plantid = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["Plant"].ToString());
            ddPlant.SelectedValue = Plantid.ToString();
            //dd.DataBind();

            string IsBilling = dsDetail.Tables[0].Rows[0]["IsFileSentToBilling"].ToString();
            if (IsBilling == "True")
            {
                // string strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.Trim();
                CustName.Enabled = false;
                ddDiv.Enabled = false;
                ddPlant.Enabled = false;
                lblError.Text = "Job already proceed to billing, so you cant change customer name.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                CustName.Enabled = true;
                ddDiv.Enabled = true;
                ddPlant.Enabled = true;
            }
            TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
            TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
        }
    }

    #endregion

    #region CAN

    protected void ddConsigneeState_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        hdnStateCode.Value = "0";
        DropDownList ddConsigneeState = sender as DropDownList;
        if (ddConsigneeState.SelectedValue != "0")
        {
            DataSet dsGetSateCode = DBOperations.GetStateCodeForLocDetail(Convert.ToInt32(ddConsigneeState.SelectedValue));
            if (dsGetSateCode != null)
            {
                if (dsGetSateCode.Tables[0].Rows[0]["StateCode"] != DBNull.Value)
                {
                    hdnStateCode.Value = dsGetSateCode.Tables[0].Rows[0]["StateCode"].ToString();
                    hdnCurrentPOS.Value = ddConsigneeState.SelectedValue;
                    txtGSTN_TextChanged(null, EventArgs.Empty);
                }
            }
        }
    }

    protected void txtGSTN_TextChanged(object sender, EventArgs e)
    {
        Button btnUpdate = (Button)FVFreightDetail.FindControl("btnUpdateFreightDetail");
        TextBox txtGSTN = (TextBox)FVFreightDetail.FindControl("txtGSTN");
        lblError.Text = "";
        btnUpdate.Enabled = true;

        if (txtGSTN.Text.Trim() != "")
        {
            int GSTNo = Convert.ToInt32(txtGSTN.Text.Trim().Substring(0, 2));
            int StateCode = Convert.ToInt32(hdnStateCode.Value);
            if (GSTNo != 0 && StateCode != 0)
            {
                if (GSTNo != StateCode)
                {
                    lblError.Text = "GST No does not match with Place Of Supply..!!";
                    lblError.CssClass = "errorMsg";
                    btnUpdate.Enabled = false;
                }
            }
        }
    }

    private bool CalculateTax()
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

        DataSet dsDetail = DBOperations.GetOperationDetail(EnqId);
        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            if (dsDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = dsDetail.Tables[0].Rows[0]["CountryCode"].ToString();
            }
        }

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

            if (hdnCountryCode.Value.Trim().ToLower() == "india")
            {
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
            }
            else
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

    //private bool CalculateTax()
    //{
    //    bool IsCalculated = false, IsPercentField = false;
    //    int UnitOfMeasure = 0;
    //    decimal decAmount = 0, decRate = 0, decExchangeRate = 0, decTaxAmount = 0, PercentFieldAmount = 0, PercentFieldExchangeRate = 0,
    //       MinimumPercentCharge = 0, MinUnit = 0, MinAmount = 0,
    //       ChargeableWeight = 1, Volume = 1, decServiceTax = 0.00m;

    //    try
    //    {
    //        if (txtTaxRate.Text.Trim() != "")
    //            decServiceTax = Convert.ToDecimal(txtTaxRate.Text.Trim());
    //    }
    //    catch (Exception e)
    //    {
    //        lblInvoiceError.Text = "Invalid Service Tax Rate! Please Enter Numeric Value.";
    //        lblInvoiceError.CssClass = "errorMsg";
    //        return false;
    //    }

    //    int FieldId = Convert.ToInt32(ddInvoice.SelectedValue);
    //    int EnqId = Convert.ToInt32(Session["EnqId"]);
    //    string strRemark = "";

    //    lblRate.Text = "Rate"; // Default Text
    //    txtUSDRate.Visible = false;

    //    DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(FieldId);

    //    if (dsFieldDetail.Tables[0].Rows.Count > 0)
    //    {
    //        UnitOfMeasure = Convert.ToInt32(dsFieldDetail.Tables[0].Rows[0]["UoMid"]);
    //        lblUOM.Text = dsFieldDetail.Tables[0].Rows[0]["UnitOfMeasurement"].ToString();
    //        hdnIsTaxRequired.Value = dsFieldDetail.Tables[0].Rows[0]["IsTaxable"].ToString();

    //        if (decServiceTax == 0)
    //        {
    //            hdnIsTaxRequired.Value = "false"; // Tax Not Required
    //        }
    //        else
    //        {
    //            // tax applicable or not based on HSN Code

    //            int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
    //            DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
    //            if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
    //            {
    //                int lMode = 0;
    //                if (hdnModeId.Value != "")
    //                    lMode = Convert.ToInt32(hdnModeId.Value);

    //                if (lMode == 1)  //Air
    //                {
    //                    if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] == DBNull.Value)
    //                    {
    //                        hdnIsTaxRequired.Value = "false";
    //                    }
    //                }
    //                else  //Sea
    //                {
    //                    if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] == DBNull.Value)
    //                    {
    //                        hdnIsTaxRequired.Value = "false";
    //                    }
    //                }
    //            }
    //        }

    //        if (dsFieldDetail.Tables[0].Rows[0]["Remark"] != DBNull.Value)
    //            strRemark = dsFieldDetail.Tables[0].Rows[0]["Remark"].ToString();

    //        lnkDataTooltip.Attributes.Add("data-tooltip", strRemark);

    //        if (txtMinUnit.Text.Trim() != "")
    //            MinUnit = Convert.ToDecimal(txtMinUnit.Text.Trim());

    //        if (txtMinAmount.Text.Trim() != "")
    //            MinAmount = Convert.ToDecimal(txtMinAmount.Text.Trim());

    //        if (UnitOfMeasure == (Int32)EnumUnit.perKG)
    //        {
    //            ChargeableWeight = Convert.ToDecimal(hdnWeight.Value);

    //            if (ChargeableWeight == 0)
    //            {
    //                lblError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";
    //                lblInvoiceError.Text = "Please check Chargeable Weight for Booking! " + hdnWeight.Value + " k.g.";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";
    //                return false;
    //            }
    //        }
    //        if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
    //        {
    //            Volume = Convert.ToDecimal(hdnVolume.Value);

    //            if (Volume == 0)
    //            {
    //                lblError.Text = "Please check CBM Value! " + hdnVolume.Value;
    //                lblInvoiceError.Text = "Please check CBM Value! " + hdnVolume.Value;

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";
    //                return false;
    //            }
    //        }
    //        else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
    //        {
    //            ddCurrency.SelectedValue = "46"; // Indian Rupee
    //            ddCurrency.Enabled = false;
    //            txtExchangeRate.Text = "1";
    //            txtExchangeRate.Enabled = false;

    //            IsPercentField = true;
    //            txtUSDRate.Visible = false;

    //            lblRate.Text = "Rate (%)  ";

    //            PercentFieldAmount = CalculatePercentField();

    //            if (PercentFieldAmount > 0)
    //            {
    //                //if (txtUSDRate.Text.Trim() != "")
    //                //{
    //                //    try
    //                //    {
    //                //        PercentFieldExchangeRate = Convert.ToDecimal(txtUSDRate.Text.Trim());
    //                //    }
    //                //    catch
    //                //    {
    //                //        lblError.Text       = "Please enter valid amount for USD Exchange Rate!";
    //                //        lblError.CssClass   = "errorMsg";
    //                //    }
    //                //}
    //            }
    //            else
    //            {
    //                // Percent field detail not found

    //                lblError.Text = "Please Select Invoice Percent Item: ";
    //                lblInvoiceError.Text = "Please Select Invoice Percent Item: ";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";
    //                return false;
    //            }
    //        }//END_IF_PercentField


    //        if (Convert.ToBoolean(hdnIsTaxRequired.Value) == false)
    //        {
    //            lblTaxAmount.Text = "0";
    //            lblTaxName.Text = "Tax Amount (N.A.)";
    //        }
    //        else
    //        {
    //            //lblTaxName.Text = "Tax Amount (" + txtTaxRate.Text.Trim() + " %) Rs";
    //            lblTaxName.Text = "Tax Amount (Rs)";
    //        }
    //    }// END_IF_RowCount
    //    else
    //    {
    //        lblError.Text = "Invoice Item Details Not Found!";
    //        lblInvoiceError.Text = "Invoice Item Details Not Found!";

    //        lblError.CssClass = "errorMsg";
    //        lblInvoiceError.CssClass = "errorMsg";

    //        lblTaxAmount.Text = "";

    //        return false;
    //    }

    //    // Calculate Amount
    //    if (ddCurrency.SelectedIndex > 0 && txtRate.Text.Trim() != "" && txtExchangeRate.Text.Trim() != "")
    //    {
    //        try
    //        {
    //            decRate = Convert.ToDecimal(txtRate.Text.Trim());
    //            decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

    //            decExchangeRate = System.Math.Round(decExchangeRate, 2, MidpointRounding.AwayFromZero);

    //            MinAmount = (MinAmount * decExchangeRate);
    //            MinUnit = (MinUnit * decRate * decExchangeRate);

    //            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
    //            {
    //                decRate = (decRate * ChargeableWeight);
    //            }
    //            else if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
    //            {
    //                decRate = (decRate * Volume);
    //            }

    //            if (decRate == 0 || decExchangeRate == 0)
    //            {
    //                lblError.Text = "Please Enter Valid Invoice Value!";
    //                lblInvoiceError.Text = "Please Enter Valid Invoice Value!";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";

    //                lblTaxAmount.Text = "";

    //                return false;
    //            }

    //            if (Convert.ToBoolean(hdnIsTaxRequired.Value) == true)
    //            {
    //                if (IsPercentField == true)
    //                {
    //                    decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

    //                    //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

    //                    if (decAmount < MinAmount)
    //                    {
    //                        decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
    //                    }
    //                }
    //                else
    //                {
    //                    decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

    //                    var list = new[] { decAmount, MinUnit, MinAmount };
    //                    decAmount = list.Max();
    //                }

    //                decTaxAmount = System.Math.Round(((decAmount * decServiceTax) / 100.00m), 2, MidpointRounding.AwayFromZero);
    //                lblTaxAmount.Text = decTaxAmount.ToString();

    //                lblAmount.Text = decAmount.ToString();
    //                lblTotalAmount.Text = (decAmount + decTaxAmount).ToString();

    //                IsCalculated = true;
    //            }
    //            else
    //            {
    //                if (IsPercentField == true)
    //                {
    //                    decAmount = System.Math.Round(((PercentFieldAmount * decRate) / 100.00m), 2, MidpointRounding.AwayFromZero);

    //                    //MinimumPercentCharge = (PercentFieldExchangeRate * 10); // 10 times of exchange rate

    //                    if (decAmount < MinAmount)
    //                    {
    //                        decAmount = MinAmount; // Minimum amount for percentage filed is 10 times of Exchange rate of percent field;
    //                    }
    //                }
    //                else
    //                {
    //                    decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

    //                    var list = new[] { decAmount, MinUnit, MinAmount };
    //                    decAmount = list.Max();
    //                }

    //                lblAmount.Text = decAmount.ToString();
    //                lblTotalAmount.Text = decAmount.ToString();

    //                IsCalculated = true;
    //            }
    //        }//END_Catch
    //        catch (Exception ex)
    //        {
    //            lblError.Text = "Please Enter Valid Amount Details";
    //            lblInvoiceError.Text = "Please Enter Valid Amount Details";
    //            lblError.CssClass = "errorMsg";
    //            lblInvoiceError.CssClass = "errorMsg";

    //            lblTaxAmount.Text = "";
    //            lblAmount.Text = "";
    //            lblTotalAmount.Text = "";

    //            IsCalculated = false;
    //        }
    //    }// END_IF
    //    else
    //    {
    //        lblTaxAmount.Text = "";
    //        lblAmount.Text = "";
    //        lblTotalAmount.Text = "";
    //        IsCalculated = false;
    //    }

    //    return IsCalculated;
    //}

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
        //ddCurrency.Enabled = true;
        //txtExchangeRate.Enabled = true;

        //CalculateTax();

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

    //protected void btnSaveInvoice_Click(object sender, EventArgs e)
    //{
    //    bool isCalculated = CalculateTax();
    //    int Result = -123;

    //    int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
    //    int CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
    //    int UnitOfmeasureId = 0;
    //    bool isTaxable = false;
    //    bool IsPercentField = false;

    //    decimal CGstTax = 0, CGstTaxAmt = 0, SGstTax = 0, SGstTaxAmt = 0, IGstTax = 0, IGstTaxAmt = 0, Amount = 0, TaxRate = 0;
    //    if (lblAmount.Text.Trim() != "")
    //        Amount = Convert.ToDecimal(lblAmount.Text.Trim());

    //    // Start - Calculate GST Rate

    //    if (hdnCountryCode.Value.Trim().ToLower() == "india")
    //    {
    //        if (hdnGSTNo.Value == "")
    //        {
    //            lblError.Text = "Please Update GST No for Enquiry..!!";
    //            lblInvoiceError.Text = "Please Update GST No for Enquiry..!!";

    //            lblError.CssClass = "errorMsg";
    //            lblInvoiceError.CssClass = "errorMsg";
    //            // count = 1;
    //        }
    //        else
    //        {
    //            string GSTNo = hdnGSTNo.Value;
    //            int StateCode = Convert.ToInt32(GSTNo.Substring(0, 2));
    //            if (StateCode == 27) // if Maharashtra (e.g.: MH --> MH)
    //            {
    //                DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
    //                if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
    //                {
    //                    int lMode = 0;
    //                    if (hdnModeId.Value != "")
    //                        lMode = Convert.ToInt32(hdnModeId.Value);

    //                    if (lMode == 1)  //Air
    //                    {
    //                        if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] != DBNull.Value)
    //                            TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    }
    //                    else  //Sea
    //                    {
    //                        if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] != DBNull.Value)
    //                            TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    }

    //                    // TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    TaxRate = TaxRate / 2;

    //                    CGstTax = TaxRate;
    //                    SGstTax = TaxRate;
    //                    IGstTax = 0;

    //                    CGstTaxAmt = Convert.ToDecimal(Amount * (CGstTax / 100));
    //                    SGstTaxAmt = Convert.ToDecimal(Amount * (SGstTax / 100));
    //                    IGstTaxAmt = 0;
    //                }
    //            }
    //            else
    //            {
    //                DataSet dsGetGSTDetails = DBOperations.GetSacDetailAsPerCharge(InvoiceItemId);
    //                if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
    //                {
    //                    int lMode = 0;
    //                    if (hdnModeId.Value != "")
    //                        lMode = Convert.ToInt32(hdnModeId.Value);

    //                    if (lMode == 1)  //Air
    //                    {
    //                        if (dsGetGSTDetails.Tables[0].Rows[0]["AirSacId"] != DBNull.Value)
    //                            TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    }
    //                    else  //Sea
    //                    {
    //                        if (dsGetGSTDetails.Tables[0].Rows[0]["SeaSacId"] != DBNull.Value)
    //                            TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    }

    //                    // TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
    //                    CGstTax = 0;
    //                    SGstTax = 0;
    //                    IGstTax = TaxRate;

    //                    CGstTaxAmt = 0;
    //                    SGstTaxAmt = 0;
    //                    IGstTaxAmt = Convert.ToDecimal(Amount * (IGstTax / 100));
    //                }
    //            }
    //        }
    //    }
    //    // End - Calculate GST Rate

    //    if (hdnIsTaxRequired.Value != "")
    //    {
    //        isTaxable = Convert.ToBoolean(hdnIsTaxRequired.Value);
    //    }

    //    DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(InvoiceItemId);

    //    if (dsFieldDetail.Tables[0].Rows.Count > 0)
    //    {
    //        UnitOfmeasureId = Convert.ToInt32(dsFieldDetail.Tables[0].Rows[0]["UoMid"]);
    //        if (UnitOfmeasureId == (Int32)EnumUnit.PercentOf)
    //        {
    //            IsPercentField = true;
    //        }
    //    }
    //    if (isCalculated == true)
    //    {
    //        string strRate = "0", strExchangeRate = "1", strServiceTax = "0.00";
    //        string strMinUnit = "0", strMinAmount = "0";
    //        string strTaxAmount = "0", strAmount = "0", strTotalAmount = "0";
    //        int EnqId = Convert.ToInt32(Session["EnqId"]);

    //        if (isTaxable == false)
    //            strServiceTax = "0.0";
    //        else
    //            strServiceTax = txtTaxRate.Text.Trim();

    //        if (txtMinUnit.Text.Trim() != "")
    //            strMinUnit = txtMinUnit.Text.Trim();

    //        if (txtMinAmount.Text.Trim() != "")
    //            strMinAmount = txtMinAmount.Text.Trim();

    //        strRate = txtRate.Text.Trim();
    //        strExchangeRate = txtExchangeRate.Text.Trim();
    //        strTaxAmount = lblTaxAmount.Text.Trim();

    //        strAmount = lblAmount.Text.Trim();
    //        strTotalAmount = lblTotalAmount.Text.Trim();

    //        // Total = Amount + CGST (Rs) + SGST (Rs) + IGST (Rs)
    //        strTotalAmount = Convert.ToDecimal(Amount + CGstTaxAmt + SGstTaxAmt + IGstTaxAmt).ToString();

    //        if (strAmount != "" && strTotalAmount != "")
    //        {
    //            Result = DBOperations.AddCANInvoiceDetail(EnqId, InvoiceItemId, UnitOfmeasureId, strRate, CurrencyId, strExchangeRate, strMinUnit, strMinAmount,
    //                isTaxable, strTaxAmount, strAmount, strTotalAmount, strServiceTax, LoggedInUser.glUserId, CGstTax, CGstTaxAmt, SGstTax, SGstTaxAmt,
    //                     IGstTax, IGstTaxAmt);

    //            if (Result == 0)
    //            {
    //                lblError.Text = "Invoice Details Added Successfully";
    //                lblInvoiceError.Text = "Invoice Details Added Successfully";

    //                lblError.CssClass = "success";
    //                lblInvoiceError.CssClass = "success";

    //                // Add Percent Invoice Field Details

    //                if (IsPercentField == true)
    //                {
    //                    string strPercentAmount = "0";
    //                    int PercentFieldId = 0;

    //                    foreach (GridViewRow row in gvCanInvoice.Rows)
    //                    {
    //                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

    //                        PercentFieldId = Convert.ToInt32(gvCanInvoice.DataKeys[row.RowIndex].Value);

    //                        if (chkSelect != null)
    //                        {
    //                            if (chkSelect.Checked)
    //                            {
    //                                Label lblTotal = (Label)row.FindControl("lblTotal");

    //                                strPercentAmount = lblTotal.Text;

    //                                DBOperations.AddCANPercentDetail(EnqId, InvoiceItemId, PercentFieldId, strPercentAmount, LoggedInUser.glUserId);
    //                            }
    //                        }//END_IF

    //                    }//END_ForEach 
    //                }

    //                ddCurrency.SelectedIndex = 0;
    //                ddInvoice.SelectedIndex = 0;
    //                txtExchangeRate.Text = "";
    //                txtRate.Text = "";
    //                lblTaxAmount.Text = "";
    //                lblAmount.Text = "";
    //                lblTotalAmount.Text = "";

    //                gvCanInvoice.DataBind();
    //            }
    //            else if (Result == 1)
    //            {
    //                lblError.Text = "System Error! Please try after sometime";
    //                lblInvoiceError.Text = "System Error! Please try after sometime";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";
    //            }
    //            else if (Result == 2)
    //            {
    //                lblError.Text = "Invoice Item details already added.";
    //                lblInvoiceError.Text = "Invoice Item details already added.";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";

    //                gvCanInvoice.DataBind();
    //            }
    //            else
    //            {
    //                lblError.Text = "System Error! Please try after sometime";
    //                lblInvoiceError.Text = "System Error! Please try after sometime";

    //                lblError.CssClass = "errorMsg";
    //                lblInvoiceError.CssClass = "errorMsg";
    //            }
    //        }
    //        else
    //        {
    //            lblError.Text = "Please Enter The Invoice Details";
    //            lblInvoiceError.Text = "Please Enter The Invoice Details";

    //            lblError.CssClass = "errorMsg";
    //            lblInvoiceError.CssClass = "errorMsg";
    //        }
    //    }
    //    else
    //    {
    //        lblError.Text = "Please Enter The Invoice Details";
    //        lblInvoiceError.Text = "Please Enter The Invoice Details";

    //        lblError.CssClass = "errorMsg";
    //        lblInvoiceError.CssClass = "errorMsg";
    //    }
    //}

    protected void btnSaveInvoice_Click(object sender, EventArgs e)
    {
        bool isCalculated = CalculateTax();
        int Result = -123;

        int InvoiceItemId = Convert.ToInt32(ddInvoice.SelectedValue);
        int CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
        int UnitOfmeasureId = 0;
        bool isTaxable = false;
        bool IsPercentField = false;
        bool IsCAN = false;

        decimal CGstTax = 0, CGstTaxAmt = 0, SGstTax = 0, SGstTaxAmt = 0, IGstTax = 0, IGstTaxAmt = 0, Amount = 0, TaxRate = 0;
        if (lblAmount.Text.Trim() != "")
            Amount = Convert.ToDecimal(lblAmount.Text.Trim());

        // Start - Calculate GST Rate

        if (hdnCountryCode.Value.Trim().ToLower() == "india")
        {
            if (hdnGSTNo.Value == "")
            {
                lblError.Text = "Please Update GST No for Enquiry..!!";
                lblInvoiceError.Text = "Please Update GST No for Enquiry..!!";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
                // count = 1;
            }
            else
            {
                string GSTNo = hdnGSTNo.Value;
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

                        // TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
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

                        // TaxRate = Convert.ToDecimal(dsGetGSTDetails.Tables[0].Rows[0]["TaxRate"].ToString());
                        CGstTax = 0;
                        SGstTax = 0;
                        IGstTax = TaxRate;

                        CGstTaxAmt = 0;
                        SGstTaxAmt = 0;
                        IGstTaxAmt = Convert.ToDecimal(Amount * (IGstTax / 100));
                    }
                }
            }
        }
        // End - Calculate GST Rate

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

            //Added new CAN Yes/No for PDF
            if (rblIsCAN.SelectedValue == "1")
            {
                IsCAN = true;
            }
            DataSet BillingDetail = DBOperations.GetBillingAdvise(EnqId);

            if (BillingDetail.Tables[0].Rows.Count > 0)
            {
                lblInvoiceError.Text = "Billing Advise Already Completed CAN Not Be Modified!";
                lblError.Text = "Billing Advise Already Completed CAN Not Be Modified!";

                lblInvoiceError.CssClass = "errorMsg";
                lblError.CssClass = "errorMsg";
            }
            else
            {

                if (strAmount != "" && strTotalAmount != "")
                {
                    Result = DBOperations.AddCANInvoiceDetail(EnqId, InvoiceItemId, UnitOfmeasureId, strRate, CurrencyId, strExchangeRate, strMinUnit, strMinAmount,
                        isTaxable, strTaxAmount, strAmount, strTotalAmount, strServiceTax, LoggedInUser.glUserId, CGstTax, CGstTaxAmt, SGstTax, SGstTaxAmt,
                             IGstTax, IGstTaxAmt, IsCAN);

                    if (Result == 0)
                    {
                        lblError.Text = "Invoice Details Added Successfully";
                        lblInvoiceError.Text = "Invoice Details Added Successfully";

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
                    lblError.Text = "Please Enter The Invoice Details";
                    lblInvoiceError.Text = "Please Enter The Invoice Details";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblError.Text = "Please Enter The Invoice Details";
            lblInvoiceError.Text = "Please Enter The Invoice Details";

            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";
        }
    }

    protected void lnkCreateCANPdf_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        //if (hdnIsGST.Value == "1")
        //{
            GenerateGSTPdf(EnqId);
        //}
        // else
        // {
        //    GenerateCANPdf(EnqId);
        //}
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

            if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

            string date = DateTime.Today.ToShortDateString();
            TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

            if (txtCANDate != null)
            {
                if (txtCANDate.Text.Trim() != "")
                    date = txtCANDate.Text.Trim();
            }

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

                    pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    string footerText1 = "1. Shipment consigned to the bank, BRO (Bank Release Order) is Mandatory.\n" +
                        "2. Your cargo has not been checked while issuing this notice.\n" +
                        "3. Please produce a letter of authority at the time of collection of your documents & D.O.S.\n" +
                        "4. Please don't pay duty without checking on complete arrival of goods as per document.\n" +
                        "5. This transaction is covered under jurisdiction of the arrival port state.\n";

                    string footerText2 = "E. & O.E.\n";

                    string footerText3 = "Please contact our office between 1000/1630 hours on all working days, except second saturdays &" +
                        " customs holidays, For charges collect shipments, the Delivery Order will be issued against the amount indicated above" +
                        " on the receipt of the D.I.C. and Delivery Order will be only issued after the cargo is forwarded to the customs warehouse and the agent is not liable for any claim" +
                        " on warehouse charges as a result of delay on part of air carriers to check the freight and issue D.O.\n" +
                        " Demurrage will be charges after free storage period as per rates published by the M.I.A.L Till customs clearance is effected. please note your cargo" +
                        " will be shifted to the M.I.A.L warehouse after 14 days of warehousing. Also note that if the said consignment is not cleared on production of" +
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

        if (hdnIsStateGST.Value == "1")
        {
            IsStateGST = true;
        }

        if (dsCANPrint.Tables[0].Rows.Count > 0)
        {
            int FreightMode = Convert.ToInt32(dsCANPrint.Tables[0].Rows[0]["lMode"]);

            ////string strServiceTax    =   "14 %";  // 
            ////string strSBCTax        =   "0.5 %"; // Swatchh Bharat Cess - 0.5%

            //string ATA = "", IGMDate = "";

            ////if (IGMNo == "")
            ////    IGMNo = txtIGMNo.Text.Trim();

            //if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
            //    IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            //if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
            //    ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();
            ////else if (txtATA.Text.Trim() != "")
            ////{
            ////    ATA = txtATA.Text.Trim();
            ////}
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

            if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

            string date = DateTime.Today.ToShortDateString();
            TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

            if (txtCANDate != null)
            {
                if (txtCANDate.Text.Trim() != "")
                    date = txtCANDate.Text.Trim();
            }

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

                        // IGST Tax %
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

    //        if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
    //            IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

    //        if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
    //            ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

    //        string CanUserName = LoggedInUser.glEmpName;

    //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF.jpg"));

    //        string date = DateTime.Today.ToShortDateString();
    //        TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

    //        if (txtCANDate != null)
    //        {
    //            if (txtCANDate.Text.Trim() != "")
    //                date = txtCANDate.Text.Trim();
    //        }

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
    //                contents = contents.Replace("[PlaceOfDelivery]", strConsigneeState);
    //                contents = contents.Replace("[DivGSTN]", strGSTN);

    //                if (FreightMode == 1) // AIR
    //                {
    //                    contents = contents.Replace("[lblChargeCBM]", "CHARG WEIGHT (KGS)");
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
    //                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("AMOUNT(Rs)", GridHeadingFont));
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
    //                pdfDoc.Add(new Paragraph(footerText5, TextFontformat));
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

    protected void gvCanInvoice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ddCurrency.SelectedIndex = 0;
        ddInvoice.SelectedIndex = 0;
        txtExchangeRate.Text = "";
        txtRate.Text = "";
        lblTaxAmount.Text = "";
        lblAmount.Text = "";
        lblTotalAmount.Text = "";
        gvCanInvoice.EditIndex = e.NewEditIndex;
    }

    protected void gvCanInvoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string strExchangeRate = "1";

        string strTaxAmount = "0", strAmount = "0", strTotalAmount = "0";

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        // Get billing Advise Details 
        DataSet BillingDetail = DBOperations.GetBillingAdvise(EnqId);

        if (BillingDetail.Tables[0].Rows.Count > 0)
        {
            lblInvoiceError.Text = "Billing Advise Already Completed CAN Not Be Modified!";
            lblError.Text = "Billing Advise Already Completed CAN Not Be Modified!";

            lblInvoiceError.CssClass = "errorMsg";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            //get countrycode for POD
            DataSet dsDetail = DBOperations.GetOperationDetail(EnqId);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            if (dsDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = dsDetail.Tables[0].Rows[0]["CountryCode"].ToString();
            }
        }

        int InvoiceId = Convert.ToInt32(gvCanInvoice.DataKeys[e.RowIndex].Value);

        TextBox txtCurrencyRate = (TextBox)gvCanInvoice.Rows[e.RowIndex].FindControl("txtCurrencyRate");
        Label lblInvoiceItemId = (Label)gvCanInvoice.Rows[e.RowIndex].FindControl("lblInvoiceItemId");

        if (txtCurrencyRate.Text.Trim() != "")
            strExchangeRate = txtCurrencyRate.Text.Trim();

        if (lblTaxAmount.Text.Trim() != "")
            strTaxAmount = lblTaxAmount.Text.Trim();

        if (lblAmount.Text.Trim() != "")
            strAmount = lblAmount.Text.Trim();

        if (lblTotalAmount.Text.Trim() != "")
            strTotalAmount = lblTotalAmount.Text;

        #region NEW METHOD

        string strRate = "0";
        int UnitOfMeasure = 0;
        decimal ChargeableWeight = 0, decRate = 0, Amount = 0, ExchangeRate = 0, Volume = 0;
        TextBox txtRate = (TextBox)gvCanInvoice.Rows[e.RowIndex].FindControl("txtRate");
        TextBox txtUOMId = (TextBox)gvCanInvoice.Rows[e.RowIndex].FindControl("txtUOMId");

        strRate = txtRate.Text.Trim();
        decRate = Convert.ToDecimal(txtRate.Text.Trim());
        UnitOfMeasure = Convert.ToInt32(txtUOMId.Text.Trim());
        ExchangeRate = Convert.ToDecimal(strExchangeRate);
        if (UnitOfMeasure != 0)
        {
            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
            {
                ChargeableWeight = Convert.ToDecimal(hdnWeight.Value);

                if (ChargeableWeight != 0)
                {
                    if (decRate != 0)
                    {
                        Amount = decRate * ChargeableWeight * ExchangeRate;
                        strAmount = Amount.ToString();
                    }
                }
            }
            else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
            {
                DataSet dsGetCANPercentDetail = DBOperations.GetCANPercentDetail(EnqId, Convert.ToInt32(lblInvoiceItemId.Text.Trim()));
                if (dsGetCANPercentDetail != null && dsGetCANPercentDetail.Tables[0].Rows.Count > 0)
                {
                    int PercentDetailId = 0;
                    decimal TotalAmnt = 0, CalTotalAmnt = 0;
                    for (int c = 0; c < dsGetCANPercentDetail.Tables[0].Rows.Count; c++)
                    {
                        if (dsGetCANPercentDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                            PercentDetailId = Convert.ToInt32(dsGetCANPercentDetail.Tables[0].Rows[0]["lid"]);
                        TotalAmnt = Convert.ToDecimal(dsGetCANPercentDetail.Tables[0].Rows[c]["TotalAmount"].ToString());
                        CalTotalAmnt = CalTotalAmnt + (System.Math.Round((TotalAmnt * (decRate / 100.00m)), 2, MidpointRounding.AwayFromZero));

                        // start - update amount in percent CAN table
                        if (PercentDetailId != 0)
                        {
                            int UpdateMsg = DBOperations.UpdateCANPercentDetail(PercentDetailId, TotalAmnt);
                        }
                        // end - update amount in percent CAN table
                    }

                    Amount = CalTotalAmnt + ExchangeRate;
                    strAmount = Amount.ToString();
                }
            }
            else if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
            {
                Volume = Convert.ToDecimal(hdnVolume.Value);

                //if (Volume == 0)
                //{
                //    lblError.Text = "Please check CBM Value! " + hdnVolume.Value;
                //    lblInvoiceError.Text = "Please check CBM Value! " + hdnVolume.Value;

                //    lblError.CssClass = "errorMsg";
                //    lblInvoiceError.CssClass = "errorMsg";
                //    return false;
                //}

                Amount = (decRate * Volume) * ExchangeRate;
                strAmount = Amount.ToString();
            }
            else
            {
                Amount = decRate * ExchangeRate;
                strAmount = Amount.ToString();
            }
        }
        else
        {
            Amount = decRate * ExchangeRate;
            strAmount = Amount.ToString();
        }

        #endregion

        int InvoiceItemId = 0;
        if (lblInvoiceItemId != null && lblInvoiceItemId.Text.Trim() != "")
            InvoiceItemId = Convert.ToInt32(lblInvoiceItemId.Text);

        decimal CGstTax = 0, CGstTaxAmt = 0, SGstTax = 0, SGstTaxAmt = 0, IGstTax = 0, IGstTaxAmt = 0;
        //if (lblAmount.Text.Trim() != "")
        //    Amount = Convert.ToDecimal(lblAmount.Text.Trim());

        // start - All gst tax amount calculations
        decimal TaxRate = 0;

        if (hdnCountryCode.Value.Trim().ToLower() == "india")
        {
            if (hdnGSTNo.Value != "")
            {
                int StateCode = Convert.ToInt32(hdnGSTNo.Value.Substring(0, 2));
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
            }
        }
        // end - All gst tax amount calculations

        // Total = Amount + CGST (Rs) + SGST (Rs) + IGST (Rs)
        strTotalAmount = Convert.ToDecimal(Amount + CGstTaxAmt + SGstTaxAmt + IGstTaxAmt).ToString();

        if (strExchangeRate != "0" && strAmount != "0" && strTotalAmount != "0")
        {
            int Result = DBOperations.UpdateCANInvoiceDetail(InvoiceId, decRate, strExchangeRate, strTaxAmount, strAmount, strTotalAmount, LoggedInUser.glUserId,
                                                                 CGstTax, CGstTaxAmt, SGstTax, SGstTaxAmt, IGstTax, IGstTaxAmt);

            if (Result == 0)
            {
                gvCanInvoice.EditIndex = -1;

                lblError.Text = "Invoice Details Updated!";
                lblInvoiceError.Text = "Invoice Details Updated!";

                lblError.CssClass = "success";
                lblInvoiceError.CssClass = "success";
                ddCurrency.SelectedIndex = 0;
                ddInvoice.SelectedIndex = 0;
                txtExchangeRate.Text = "";
                txtRate.Text = "";
                lblTaxAmount.Text = "";
                lblAmount.Text = "";
                lblTotalAmount.Text = "";
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblInvoiceError.Text = "System Error! Please try after sometime.";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";

            }
            if (Result == 2)
            {
                lblError.Text = "Invoice Details Not Found!";
                lblInvoiceError.Text = "Invoice Details Not Found!";

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Check Invoice Details!";
            lblInvoiceError.Text = "Please Check Invoice Details!";

            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";

        }
    }
        e.Cancel = true;

    }

    protected void gvCanInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ddCurrency.SelectedIndex = 0;
        ddInvoice.SelectedIndex = 0;
        txtExchangeRate.Text = "";
        txtRate.Text = "";
        lblTaxAmount.Text = "";
        lblAmount.Text = "";
        lblTotalAmount.Text = "";

        gvCanInvoice.EditIndex = -1;
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        CalculateTax();
    }

    protected void txtCurrencyRate_TextChanged(object sender, EventArgs e)
    {
        ddCurrency.SelectedIndex = 0;
        ddInvoice.SelectedIndex = 0;
        txtExchangeRate.Text = "";
        txtRate.Text = "";
        lblTaxAmount.Text = "";
        lblAmount.Text = "";
        lblTotalAmount.Text = "";

        TextBox txtCurrencyRate = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtCurrencyRate.Parent.Parent;

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int lId = Convert.ToInt32(gvCanInvoice.DataKeys[gvRow.DataItemIndex].Value.ToString());

        Label lblChargeableWeight = (Label)FVFreightDetail.FindControl("lblChargeableWeight");
        Label lblLCLVolume = (Label)FVFreightDetail.FindControl("lblLCLVolume");

        DataSet dsCANInvoice = DBOperations.GetCANInvoicebyID(lId);

        bool IsCalculated = false, IsPercentField = false, IsTaxRequired = false;
        int FieldId = 0, UnitOfMeasure = 0;
        decimal decAmount = 0, decRate = 0, decExchangeRate = 0, decTaxAmount = 0, PercentFieldAmount = 0,
            MinUnit = 0, MinAmount = 0, ChargeableWeight = 1, Volume = 1, decServiceTax = 0.00m;

        decRate = Convert.ToDecimal(dsCANInvoice.Tables[0].Rows[0]["Rate"]);
        decServiceTax = Convert.ToDecimal(dsCANInvoice.Tables[0].Rows[0]["TaxPercentage"]);
        FieldId = Convert.ToInt32(dsCANInvoice.Tables[0].Rows[0]["InvoiceItemId"]);
        UnitOfMeasure = Convert.ToInt32(dsCANInvoice.Tables[0].Rows[0]["UOMId"]);
        IsTaxRequired = Convert.ToBoolean(dsCANInvoice.Tables[0].Rows[0]["TaxApplicable"]);
        MinUnit = Convert.ToInt32(dsCANInvoice.Tables[0].Rows[0]["MinUnit"]);
        MinAmount = Convert.ToInt32(dsCANInvoice.Tables[0].Rows[0]["MinAmount"]);

        ChargeableWeight = Convert.ToDecimal(lblChargeableWeight.Text);

        DataSet dsFieldDetail = DBOperations.GetInvoiceFieldById(FieldId);

        if (dsFieldDetail.Tables[0].Rows.Count > 0)
        {
            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
            {

                if (ChargeableWeight == 0)
                {
                    lblError.Text = "Please check Chargeable Weight for Booking! " + lblChargeableWeight.Text + " k.g.";
                    lblInvoiceError.Text = "Please check Chargeable Weight for Booking! " + lblChargeableWeight.Text + " k.g.";

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";
                    return;
                }
            }
            if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
            {
                Volume = Convert.ToDecimal(lblLCLVolume.Text);

                if (Volume == 0)
                {
                    lblError.Text = "Please check CBM Value! " + lblLCLVolume.Text;
                    lblInvoiceError.Text = "Please check CBM Value! " + lblLCLVolume.Text;

                    lblError.CssClass = "errorMsg";
                    lblInvoiceError.CssClass = "errorMsg";

                    return;
                }
            }
            else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
            {
                //do Nothing

                lblError.Text = "Percent Field Can Not Modify!" + lblLCLVolume.Text;
                lblInvoiceError.Text = "Percent Field Can Not Modify! " + lblLCLVolume.Text;

                lblError.CssClass = "errorMsg";
                lblInvoiceError.CssClass = "errorMsg";

                return;

            }//END_IF_PercentField

        }// END_IF_RowCount


        // Calculate Amount
        if (txtCurrencyRate.Text.Trim() != "")
        {
            try
            {
                decExchangeRate = Convert.ToDecimal(txtCurrencyRate.Text.Trim());

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

                    return;
                }

                if (IsTaxRequired == true)
                {
                    decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);
                    var list = new[] { decAmount, MinUnit, MinAmount };
                    decAmount = list.Max();

                    decTaxAmount = System.Math.Round(((decAmount * decServiceTax) / 100.00m), 2, MidpointRounding.AwayFromZero);
                    lblTaxAmount.Text = decTaxAmount.ToString();

                    lblAmount.Text = decAmount.ToString();
                    lblTotalAmount.Text = (decAmount + decTaxAmount).ToString();

                    IsCalculated = true;
                }
                else
                {
                    decAmount = System.Math.Round((decRate * decExchangeRate), 2, MidpointRounding.AwayFromZero);

                    var list = new[] { decAmount, MinUnit, MinAmount };
                    decAmount = list.Max();

                    string test = lblAmount.Text;
                    string test2 = lblTotalAmount.Text;

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
                return;
            }
        }// END_IF
        else
        {
            lblError.Text = "Please Enter Exchange Rate!";
            lblInvoiceError.Text = "Please Enter Exchange Rate!";
            lblError.CssClass = "errorMsg";
            lblInvoiceError.CssClass = "errorMsg";

            lblTaxAmount.Text = "";
            lblAmount.Text = "";
            lblTotalAmount.Text = "";
            IsCalculated = false;
            return;
        }
    }

    #endregion

    #region DO PDF
    protected void lnkCreateDOPDF_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        DataSet dsDOPrint = DBOperations.GetOperationDetail(EnqId);

        if (dsDOPrint.Tables[0].Rows.Count > 0)
        {
            int ModeId = Convert.ToInt32(dsDOPrint.Tables[0].Rows[0]["lMode"]);

            if (ModeId == (Int32)TransMode.Air)
            {
                GeneratePdfAIR(EnqId, dsDOPrint);
            }
            else
            {
                GeneratePdfSEA(EnqId, dsDOPrint);
            }
        }
        else
        {
            lblError.Text = "Job Details Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    private void GeneratePdfAIR(int EnqId, DataSet dsDOAirPrint)
    {
        string strHOAddress1 = "Babaji Shivram Clearing and Carriers Pvt. Ltd.";
        string strHOAddress2 = "Plot no. 2, Behind Excom House";
        string strHOAddress3 = "Saki Vihar Road, Saki Naka";
        string strHOAddress4 = "Mumbai - 400072, INDIA.";

        string FRJobNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["FRJobNo"]);
        string Consignee = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["Consignee"]);
        string ConsigneeAddress = Consignee + ", " + Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ConsigneeAddress"]);

        string VesselNumber = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["VesselNumber"]);
        string IGMNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["IGMNo"]);
        string NoOfPkgs = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["NoOfPackages"]);
        string GrossWeight = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["GrossWeight"]);
        string ChargeableWeight = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ChargeableWeight"]);
        string CargoDescription = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["CargoDescription"]);
        string MBLNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["MBLNo"]);
        string HBLNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["HBLNo"]);
        string PortCity = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["PortCity"]);

        string ATADate = "", IGMDate = "";

        if (dsDOAirPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
        {
            ATADate = Convert.ToDateTime(dsDOAirPrint.Tables[0].Rows[0]["ETA"]).ToString("dd/MM/yyyy");
        }

        if (dsDOAirPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
        {
            IGMDate = Convert.ToDateTime(dsDOAirPrint.Tables[0].Rows[0]["IGMDate"]).ToString("dd/MM/yyyy");
        }

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string dateToday = DateTime.Today.ToString("dd/MM/yyyy");

        try
        {
            // Generate PDF

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrder-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);
            StringReader sr = new StringReader(sw.ToString());

            Rectangle recPDF = new Rectangle(PageSize.A4);

            // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
            // Set PDF Document size and Left,Right,Top and Bottom margin

            Document pdfDoc = new Document(recPDF);

            //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            pdfDoc.Open();

            Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

            logo.SetAbsolutePosition(380, 700);

            logo.Alignment = Convert.ToInt32(ImageAlign.Right);
            pdfDoc.Add(logo);

            string contents = "";
            contents = File.ReadAllText(Server.MapPath("DOLetterAIR.htm"));
            contents = contents.Replace("[TodayDate]", dateToday.ToString());
            contents = contents.Replace("[HOAddress1]", strHOAddress1);
            contents = contents.Replace("[HOAddress2]", strHOAddress2);
            contents = contents.Replace("[HOAddress3]", strHOAddress3);
            contents = contents.Replace("[HOAddress4]", strHOAddress4);
            contents = contents.Replace("[PortCity]", PortCity);

            contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
            contents = contents.Replace("[FlightNo]", VesselNumber);
            contents = contents.Replace("[ArrivalDate]", ATADate);

            contents = contents.Replace("[HAWBL]", HBLNo);
            contents = contents.Replace("[MAWBL]", MBLNo);

            contents = contents.Replace("[IGMNo]", IGMNo);
            contents = contents.Replace("[IGMDate]", IGMDate);
            contents = contents.Replace("[NoOfPkgs]", NoOfPkgs);
            contents = contents.Replace("[GrossWeight]", GrossWeight);
            contents = contents.Replace("[ChargeWeight]", ChargeableWeight);

            contents = contents.Replace("[Description]", CargoDescription);


            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);

            Paragraph ParaSpacing = new Paragraph();
            ParaSpacing.SpacingBefore = 40;//5

            pdfDoc.Add(ParaSpacing);

            // For Footer Signature

            PdfPTable pt = new PdfPTable(1);
            PdfPCell _cell;

            _cell = new PdfPCell(new Phrase("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            _cell.ExtraParagraphSpace = 40;
            pt.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("Authorised Signatory", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            pt.AddCell(_cell);

            pdfDoc.Add(pt);

            // Footer Image Commented
            // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
            // footer.SetAbsolutePosition(30, 0);
            // pdfDoc.Add(footer);
            // pdfwriter.PageEvent = new PDFFooter();

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }//END_Try

        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    private void GeneratePdfSEA(int EnqId, DataSet dsDOSeaPrint)
    {
        string FRJobNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FRJobNo"]).Trim();
        string Customer = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["Customer"]).Trim();
        string FinalAgent = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FinalAgent"]).Trim();
        string VesselName = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["VesselName"]).Trim();
        string IGMNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["IGMNo"]).Trim();
        string MBLNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["MBLNo"]).Trim();
        string HBLNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["HBLNo"]).Trim();
        string ItemNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["ItemNo"]).Trim();
        string ChaName = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["CHAName"]).Trim();
        string ConsigneeName = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["Consignee"]).Trim();

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

        string dateToday = DateTime.Now.ToString("dd/MM/yyyy");

        try
        {
            // Generate PDF

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrder-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);
            StringReader sr = new StringReader(sw.ToString());

            Rectangle recPDF = new Rectangle(PageSize.A4);

            // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
            // Set PDF Document size and Left,Right,Top and Bottom margin

            Document pdfDoc = new Document(recPDF);

            //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            pdfDoc.Open();

            Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

            logo.SetAbsolutePosition(380, 700);

            logo.Alignment = Convert.ToInt32(ImageAlign.Right);
            pdfDoc.Add(logo);

            string contents = "";
            contents = File.ReadAllText(Server.MapPath("DOLetterSEA.htm"));
            contents = contents.Replace("[TodayDate]", dateToday.ToString());

            contents = contents.Replace("[JobRefNo]", FRJobNo);
            contents = contents.Replace("[FinalAgent]", FinalAgent);
            contents = contents.Replace("[VesselName]", VesselName);

            contents = contents.Replace("[MAWBL]", MBLNo);
            contents = contents.Replace("[HAWBL]", HBLNo);

            contents = contents.Replace("[ConsigneeName]", ConsigneeName);
            contents = contents.Replace("[IGMNO]", IGMNo);
            contents = contents.Replace("[ItemNO]", ItemNo);
            contents = contents.Replace("[CHAName]", ChaName);

            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);

            Paragraph ParaSpacing = new Paragraph();
            ParaSpacing.SpacingBefore = 40;//5

            pdfDoc.Add(ParaSpacing);

            // For Footer Signature

            /*************************************
            PdfPTable pt = new PdfPTable(1);
            PdfPCell _cell;

            _cell = new PdfPCell(new Phrase("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            _cell.ExtraParagraphSpace = 40;
            pt.AddCell(_cell);

            pdfDoc.Add(pt);
            *************************************/
            // Footer Image Commented
            // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
            // footer.SetAbsolutePosition(30, 0);
            // pdfDoc.Add(footer);
            // pdfwriter.PageEvent = new PDFFooter();

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }//END_Try

        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }
    #endregion

    #region Debit Note

    protected void btnSaveDebitnote_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int AgentId = Convert.ToInt32(ddAgentEnq.SelectedValue);
        string strDebitAmount;
        string strGSTPercent;
        string strRemark;


        strDebitAmount = txtDebitNote.Text.Trim();
        strGSTPercent = txtGst.Text.Trim();
        strRemark = txtRemarkDebit.Text.Trim();

        int Result = DBOperations.AddDebitNote(EnqId, strDebitAmount, strGSTPercent, AgentId, strRemark, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblError.Text = "Debit Note Add Succesfully";
            lblError.CssClass = "success";
            grdDebitNote.DataBind();
            txtDebitNote.Text = " ";
            txtGst.Text = "";
            txtRemarkDebit.Text = "";
            //ddAgentEnq.SelectedValue = "0";

        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please Try After Some Time";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Debit Note Amount Alerady Exist";
            lblError.CssClass = "errorMsg";
        }

    }
    
    protected void lnkCreateDebitNote_Click(Object sender, EventArgs e)
    {
        lblError.Text = "";

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int AgentId = Convert.ToInt32(ddAgentEnq.SelectedValue);

        GenerateDebitNotePdf(EnqId, AgentId);

    }

    private void GenerateDebitNotePdf(int EnqId, int AgentId)
    {
        DataSet dsDebitNote = DBOperations.GetOperationDetail(EnqId);
        DataView dbAgentDetails = DBOperations.GetCustomerDetail(AgentId.ToString());

        string strAgentAddress = "";

        if (dbAgentDetails.Table.Rows[0]["Address"] != DBNull.Value)
        {
            strAgentAddress = dbAgentDetails.Table.Rows[0]["Address"].ToString();
        }
        string DebitNoteAmount = "0"; string DebitNoteRemark = "";
        string GSTRate = "0"; string GSTAmount = ""; string TotalAmount = "";

        if (dsDebitNote.Tables[0].Rows.Count > 0)
        {
            int FreightMode = Convert.ToInt32(dsDebitNote.Tables[0].Rows[0]["lMode"]);

            if (dsDebitNote.Tables[0].Rows[0]["DebitNoteAmount"] != DBNull.Value)
                DebitNoteAmount = dsDebitNote.Tables[0].Rows[0]["DebitNoteAmount"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["DebitNoteRemark"] != DBNull.Value)
                DebitNoteRemark = dsDebitNote.Tables[0].Rows[0]["DebitNoteRemark"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["GSTRate"] != DBNull.Value)
                GSTRate = dsDebitNote.Tables[0].Rows[0]["GSTRate"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
                GSTAmount = dsDebitNote.Tables[0].Rows[0]["GSTAmount"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["TotalAmount"] != DBNull.Value)
                TotalAmount = dsDebitNote.Tables[0].Rows[0]["TotalAmount"].ToString();

            string FRJobNo = dsDebitNote.Tables[0].Rows[0]["FRJobNo"].ToString();
            string Customer = dsDebitNote.Tables[0].Rows[0]["Customer"].ToString();
            string Consignee = dsDebitNote.Tables[0].Rows[0]["Consignee"].ToString();
            string FinalAgent = dsDebitNote.Tables[0].Rows[0]["FinalAgent"].ToString();
            string Shipper = dsDebitNote.Tables[0].Rows[0]["Shipper"].ToString();
            string FinalAgentAddress = dsDebitNote.Tables[0].Rows[0]["FinalAgentAddress"].ToString();
            string ConsigneeAddress = dsDebitNote.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
            string ShipperAddress = dsDebitNote.Tables[0].Rows[0]["ShipperAddress"].ToString();

            string MBLNo = dsDebitNote.Tables[0].Rows[0]["MBLNo"].ToString();
            string HBLNo = dsDebitNote.Tables[0].Rows[0]["HBLNo"].ToString();
            string InvoiceNo = dsDebitNote.Tables[0].Rows[0]["InvoiceNo"].ToString();
            string PONumber = dsDebitNote.Tables[0].Rows[0]["PONumber"].ToString();
            string VesselName = dsDebitNote.Tables[0].Rows[0]["VesselName"].ToString();
            string VesselNo = dsDebitNote.Tables[0].Rows[0]["VesselNumber"].ToString();
            string LoadingPort = dsDebitNote.Tables[0].Rows[0]["LoadingPortName"].ToString();
            string PortOfDischarged = dsDebitNote.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            string NoOfPackages = dsDebitNote.Tables[0].Rows[0]["NoOfPackages"].ToString();
            string GrossWeight = dsDebitNote.Tables[0].Rows[0]["GrossWeight"].ToString();
            string ChargeableWeight = dsDebitNote.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            string LCLCBM = dsDebitNote.Tables[0].Rows[0]["LCLVolume"].ToString();
            string IGMNo = dsDebitNote.Tables[0].Rows[0]["IGMNo"].ToString();
            string ItemNo = dsDebitNote.Tables[0].Rows[0]["ItemNo"].ToString();
            string strDescription = dsDebitNote.Tables[0].Rows[0]["CargoDescription"].ToString();

            string strConsigneeState = dsDebitNote.Tables[0].Rows[0]["ConsigneeStateName"].ToString();
            string strGSTN = dsDebitNote.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();

            string ATA = "", IGMDate = "";

            if (dsDebitNote.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsDebitNote.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsDebitNote.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsDebitNote.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

            string date = DateTime.Today.ToShortDateString();

            DataSet dsDebitAmount = DBOperations.GetFreighPrintDebitNote(EnqId, AgentId);

            try
            {
                if (dsDebitAmount.Tables[0].Rows.Count > 0)
                {
                    // Generate PDF
                    //int i = 1; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Debit_Note_Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
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
                    contents = File.ReadAllText(Server.MapPath("DebitNoteLetter.htm"));
                    contents = contents.Replace("[TodayDate]", date.ToString());
                    contents = contents.Replace("[JobRefNO]", FRJobNo);
                    contents = contents.Replace("[AgentName]", ddAgentEnq.SelectedItem.Text);
                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[ShipperName]", Shipper);
                    contents = contents.Replace("[FinalAgentAddress]", strAgentAddress);
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
                    contents = contents.Replace("[DebitNoteAmount]", DebitNoteAmount);

                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[GSTRate]", GSTRate);

                    contents = contents.Replace("[DebitNoteRemark]", DebitNoteRemark);
                    contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);

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


                    PdfPTable pdftable = new PdfPTable(6);

                    pdftable.TotalWidth = 520f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { .5f, 2f, 1.5f, 1f, 2f, 1.5f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_CENTER;


                    // Set Table Spacing Before And After html text
                    //   pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 8f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    pdftable.AddCell(cellwithdata);

                    //Remark 
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Charges", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: Desctiption
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Debit Amount" + " Rs.", GridHeadingFont));
                    pdftable.AddCell(cellwithdata2);

                    // Header: GST Rate %
                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("GST Rate %", GridHeadingFont));
                    pdftable.AddCell(cellwithdata3);

                    // Header: GST Amount
                    PdfPCell cellwithdata4 = new PdfPCell(new Phrase("GST Amount Rs", GridHeadingFont));
                    pdftable.AddCell(cellwithdata4);

                    // Header: Total Amount
                    PdfPCell cellwithdata5 = new PdfPCell(new Phrase("Total Amount Rs", GridHeadingFont));
                    pdftable.AddCell(cellwithdata5);


                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.UseVariableBorders = false;

                    // Data Cell: Description Of Charges

                    PdfPCell CellDescription = new PdfPCell();
                    CellDescription.Colspan = 1;

                    PdfPCell CellDebitAmount = new PdfPCell();
                    CellDebitAmount.Colspan = 1;

                    PdfPCell CellGSTRate = new PdfPCell();
                    CellGSTRate.Colspan = 1;

                    PdfPCell CellGSTAmount = new PdfPCell();
                    CellGSTAmount.Colspan = 1;

                    PdfPCell CellTotalAmount = new PdfPCell();
                    CellTotalAmount.Colspan = 1;

                    /**********************************************************/
                    //  Generate Table Data from Agent Debit Note 
                    int rowCount = dsDebitAmount.Tables[0].Rows.Count;
                    int i = 0; // Auto Increment Table Cell For Serial number
                    foreach (DataRow dr in dsDebitAmount.Tables[0].Rows)
                    {
                        i = i + 1;

                        //// Add Cell Data To Table


                        if (rowCount == i)
                        {
                            // SL
                            cellwithdata.Phrase = new Phrase("Total", GridHeadingFont);
                            pdftable.AddCell(cellwithdata);

                            //Remark 
                            cellwithdata1.Phrase = new Phrase(Convert.ToString(dr["DebitNoteRemark"]), GridHeadingFont);
                            pdftable.AddCell(cellwithdata1);


                            //Debit Amount
                            cellwithdata2.Phrase = new Phrase(Convert.ToString(dr["DebitNoteAmount"]), GridHeadingFont);
                            pdftable.AddCell(cellwithdata2);
                            //CellDescription.Phrase = new Phrase(Convert.ToString(dsDebitAmount.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);

                            //GST RATE
                            cellwithdata3.Phrase = new Phrase(Convert.ToString(dr["GSTRate"]), GridHeadingFont);
                            pdftable.AddCell(cellwithdata3);

                            //GST Amount
                            cellwithdata4.Phrase = new Phrase(Convert.ToString(dr["GSTAmount"]), GridHeadingFont);
                            pdftable.AddCell(cellwithdata4);

                            //Total Amount
                            cellwithdata5.Phrase = new Phrase(Convert.ToString(dr["TotalAmount"]), GridHeadingFont);
                            pdftable.AddCell(cellwithdata5);
                        } // END ELSE ROWCOUNT  

                        else
                        {
                            // SL
                            cellwithdata.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                            pdftable.AddCell(cellwithdata);

                            //Remark 
                            cellwithdata1.Phrase = new Phrase(Convert.ToString(dr["DebitNoteRemark"]), TextFontformat);
                            pdftable.AddCell(cellwithdata1);

                            //Debit Amount
                            cellwithdata2.Phrase = new Phrase(Convert.ToString(dr["DebitNoteAmount"]), TextFontformat);
                            pdftable.AddCell(cellwithdata2);
                            //CellDescription.Phrase = new Phrase(Convert.ToString(dsDebitAmount.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);

                            //GST RATE
                            cellwithdata3.Phrase = new Phrase(Convert.ToString(dr["GSTRate"]), TextFontformat);
                            pdftable.AddCell(cellwithdata3);

                            //GST Amount
                            cellwithdata4.Phrase = new Phrase(Convert.ToString(dr["GSTAmount"]), TextFontformat);
                            pdftable.AddCell(cellwithdata4);

                            //Total Amount
                            cellwithdata5.Phrase = new Phrase(Convert.ToString(dr["TotalAmount"]), TextFontformat);
                            pdftable.AddCell(cellwithdata5);


                        }//END Foreach
                    }
                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 5;//10

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    Paragraph ParaSpacingAfter = new Paragraph();
                    ParaSpacing.SpacingBefore = 20;//10

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(ParaSpacing);

                    string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
                            "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

                    pdfDoc.Add(ParaSpacingAfter);
                    pdfDoc.Add(ParaSpacingAfter);

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
                    lblError.Text = "Please Enter Debit Note Details!";
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
            lblError.Text = "Debit Note Details Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    private void GenerateDebitNotePdf(int EnqId)
    {
        DataSet dsDebitNote = DBOperations.GetOperationDetail(EnqId);

        string DebitNoteAmount = "0"; string DebitNoteRemark = "";
        string GSTRate = "0"; string GSTAmount = ""; string TotalAmount = "";

        if (dsDebitNote.Tables[0].Rows.Count > 0)
        {
            int FreightMode = Convert.ToInt32(dsDebitNote.Tables[0].Rows[0]["lMode"]);

            if (dsDebitNote.Tables[0].Rows[0]["DebitNoteAmount"] != DBNull.Value)
                DebitNoteAmount = dsDebitNote.Tables[0].Rows[0]["DebitNoteAmount"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["DebitNoteRemark"] != DBNull.Value)
                DebitNoteRemark = dsDebitNote.Tables[0].Rows[0]["DebitNoteRemark"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["GSTRate"] != DBNull.Value)
                GSTRate = dsDebitNote.Tables[0].Rows[0]["GSTRate"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["GSTAmount"] != DBNull.Value)
                GSTAmount = dsDebitNote.Tables[0].Rows[0]["GSTAmount"].ToString();

            if (dsDebitNote.Tables[0].Rows[0]["TotalAmount"] != DBNull.Value)
                TotalAmount = dsDebitNote.Tables[0].Rows[0]["TotalAmount"].ToString();

            string FRJobNo = dsDebitNote.Tables[0].Rows[0]["FRJobNo"].ToString();
            string Customer = dsDebitNote.Tables[0].Rows[0]["Customer"].ToString();
            string Consignee = dsDebitNote.Tables[0].Rows[0]["Consignee"].ToString();
            string FinalAgent = dsDebitNote.Tables[0].Rows[0]["FinalAgent"].ToString();
            string Shipper = dsDebitNote.Tables[0].Rows[0]["Shipper"].ToString();
            string FinalAgentAddress = dsDebitNote.Tables[0].Rows[0]["FinalAgentAddress"].ToString();
            string ConsigneeAddress = dsDebitNote.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
            string ShipperAddress = dsDebitNote.Tables[0].Rows[0]["ShipperAddress"].ToString();

            string MBLNo = dsDebitNote.Tables[0].Rows[0]["MBLNo"].ToString();
            string HBLNo = dsDebitNote.Tables[0].Rows[0]["HBLNo"].ToString();
            string InvoiceNo = dsDebitNote.Tables[0].Rows[0]["InvoiceNo"].ToString();
            string PONumber = dsDebitNote.Tables[0].Rows[0]["PONumber"].ToString();
            string VesselName = dsDebitNote.Tables[0].Rows[0]["VesselName"].ToString();
            string VesselNo = dsDebitNote.Tables[0].Rows[0]["VesselNumber"].ToString();
            string LoadingPort = dsDebitNote.Tables[0].Rows[0]["LoadingPortName"].ToString();
            string PortOfDischarged = dsDebitNote.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            string NoOfPackages = dsDebitNote.Tables[0].Rows[0]["NoOfPackages"].ToString();
            string GrossWeight = dsDebitNote.Tables[0].Rows[0]["GrossWeight"].ToString();
            string ChargeableWeight = dsDebitNote.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            string LCLCBM = dsDebitNote.Tables[0].Rows[0]["LCLVolume"].ToString();
            string IGMNo = dsDebitNote.Tables[0].Rows[0]["IGMNo"].ToString();
            string ItemNo = dsDebitNote.Tables[0].Rows[0]["ItemNo"].ToString();
            string strDescription = dsDebitNote.Tables[0].Rows[0]["CargoDescription"].ToString();

            string strConsigneeState = dsDebitNote.Tables[0].Rows[0]["ConsigneeStateName"].ToString();
            string strGSTN = dsDebitNote.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();

            string ATA = "", IGMDate = "";

            if (dsDebitNote.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsDebitNote.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsDebitNote.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsDebitNote.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

            string date = DateTime.Today.ToShortDateString();

            try
            {
                if (DebitNoteAmount != "0")
                {
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Debit_Note_Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
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
                    contents = File.ReadAllText(Server.MapPath("DebitNoteLetter.htm"));
                    contents = contents.Replace("[TodayDate]", date.ToString());
                    contents = contents.Replace("[JobRefNO]", FRJobNo);

                    contents = contents.Replace("[AgentName]", FinalAgent);

                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[ShipperName]", Shipper);

                    contents = contents.Replace("[FinalAgentAddress]", FinalAgentAddress);
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

                    PdfPTable pdftable = new PdfPTable(5);

                    pdftable.TotalWidth = 520f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.3f, 2f, 1f, 1f, 1f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Set Table Spacing Before And After html text
                    //   pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 8f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    pdftable.AddCell(cellwithdata);

                    // Header: Desctiption
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase(DebitNoteRemark + " Rs.", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: GST Rate %
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("GST Rate %", GridHeadingFont));
                    pdftable.AddCell(cellwithdata2);

                    // Header: GST Amount
                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("GST Amount Rs", GridHeadingFont));
                    pdftable.AddCell(cellwithdata3);

                    // Header: Total Amount
                    PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Total Amount Rs", GridHeadingFont));
                    pdftable.AddCell(cellwithdata4);


                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.UseVariableBorders = false;

                    // Data Cell: Description Of Charges

                    PdfPCell CellDescription = new PdfPCell();
                    CellDescription.Colspan = 1;

                    PdfPCell CellGSTRate = new PdfPCell();
                    CellGSTRate.Colspan = 1;

                    PdfPCell CellGSTAmount = new PdfPCell();
                    CellGSTAmount.Colspan = 1;

                    PdfPCell CellTotalAmount = new PdfPCell();
                    CellTotalAmount.Colspan = 1;

                    /**********************************************************/
                    //  Generate Table Data from Agent Debit Note 

                    int rowCount = 1;

                    // Add Cell Data To Table

                    SrnoCell.Phrase = new Phrase(Convert.ToString(rowCount), TextFontformat);
                    pdftable.AddCell(SrnoCell);

                    CellDescription.Phrase = new Phrase(DebitNoteAmount, TextFontformat);
                    pdftable.AddCell(CellDescription);

                    CellGSTRate.Phrase = new Phrase(GSTRate, TextFontformat);
                    pdftable.AddCell(CellGSTRate);

                    CellGSTAmount.Phrase = new Phrase(GSTAmount, TextFontformat);
                    pdftable.AddCell(CellGSTAmount);

                    CellTotalAmount.Phrase = new Phrase(TotalAmount, TextFontformat);
                    pdftable.AddCell(CellTotalAmount);

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 5;//10

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    Paragraph ParaSpacingAfter = new Paragraph();
                    ParaSpacing.SpacingBefore = 20;//10

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(ParaSpacing);

                    string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
                            "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

                    pdfDoc.Add(ParaSpacingAfter);
                    pdfDoc.Add(ParaSpacingAfter);

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
                    lblError.Text = "Please Enter Debit Note Details!";
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
            lblError.Text = "Debit Note Details Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void grdDebitNote_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int DebitId = Convert.ToInt32(grdDebitNote.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtDebitRemark = (TextBox)grdDebitNote.Rows[e.RowIndex].FindControl("txtDebitRemark");

        string strRemark = txtDebitRemark.Text.Trim();
        if (strRemark != "")
        {
            int Result = DBOperations.updFrightDebitRemark(DebitId, strRemark, LoggedInUser.glUserId);
            if (Result == 0)
            {
                e.Cancel = true;
                grdDebitNote.EditIndex = -1;
                grdDebitNote.DataBind();
                lblError.Visible = true;
                lblError.Text = "Debit Note Updated Successfully";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try  After Some Time ";
                lblError.CssClass = "errorMasg";
            }

        }
        else
        {
            lblError.Text = "Please Enter Debit Type";
            lblError.CssClass = "errorMsg";

        }
    }

    #endregion

    #region Freight Document
    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "viewdoc")
        {
            string DocPath = e.CommandArgument.ToString();
            ViewDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "download")
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
                        gvFreightAttach.DataBind();
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

    #region Container Detail
    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int ContainerType; //1-FCL, 2 - LCL 
        int ContainerSize; //1 -Twenty, 2 - Forty

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

        if (EnqId > 0)
        {
            if (ContainerType == (Int16)EnumContainerType.FCL)
            {
                if (ContainerSize == 0)
                {
                    lblError.Text = "Please Select FCL Container Size!";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }
            else if (ContainerType == (Int16)EnumContainerType.LCL) //LCL
            {
                ddContainerSize.SelectedValue = "0";
                ContainerSize = 0;
            }

            if (ContainerNo != "")
            {
                int result = DBOperations.AddFreightContainerMS(EnqId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Container No. " + ContainerNo + " Added Successfully.";
                    lblError.CssClass = "success";
                    gvContainer.DataBind();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Container No. " + ContainerNo + " Already Added!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = " Please Enter Container No.!";
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Freight Details Not Found.!";
        }

    }

    protected void ddContainerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddContainerType.SelectedValue == "2")
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddContainerSize.Items.Clear();
            ddContainerSize.Items.Add(lstSelect);

        }
        else
        {
            ddContainerSize.Items.Clear();
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            System.Web.UI.WebControls.ListItem lstSelect20 = new System.Web.UI.WebControls.ListItem("20", "1");
            System.Web.UI.WebControls.ListItem lstSelect40 = new System.Web.UI.WebControls.ListItem("40", "2");

            ddContainerSize.Items.Add(lstSelect);
            ddContainerSize.Items.Add(lstSelect20);
            ddContainerSize.Items.Add(lstSelect40);
        }
    }

    protected void gvContainer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDelete = (LinkButton)e.Row.Cells[6].Controls[2];

            //Delete Button index is 1 
            if (lnkDelete != null && lnkDelete.Text.ToLower() == "delete")
            {
                lnkDelete.OnClientClick = "return confirm('Do you really want to delete?');";

            }
        }
    }

    protected void gvContainer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.Visible = true;
        int ContainerSize, ContainerType;
        string ContainerNo = "";

        int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtContainerNo = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtEditContainerNo");
        DropDownList ddlContainerType = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerType");
        DropDownList ddlContainerSize = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerSize");

        ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddlContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddlContainerType.SelectedValue);

        if (ContainerType == 1) //FCL
        {
            if (ContainerSize == 0)
            {
                lblError.Text = "Please Select FCL Container Size!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
                return;
            }
        }
        if (ContainerType == 2) //LCL
        {
            ddlContainerSize.SelectedValue = "0";
            ContainerSize = 0;
        }
        if (ContainerNo != "")
        {
            int result = DBOperations.UpdateFreightContainerMS(lid, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Container Detail Updated Successfully!";
                lblError.CssClass = "success";
                e.Cancel = true;
                gvContainer.EditIndex = -1;
            }
            else if (result == 2)
            {
                lblError.Text = "Container No Already Exists!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = " Please Enter Container No!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvContainer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;

        int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = DBOperations.DeleteFreightContainerMS(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Container Deleted Successfully!";
            lblError.CssClass = "success";
            e.Cancel = true;
            gvContainer.DataBind();
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "success";
        }
    }

    #endregion

    #region Daily Activity

    protected void btnSaveActivity_Click(object semder, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int result = DBOperations.AddFreightActivity(EnqId, txtDailyProgress.Text.Trim(), "", 0, true, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Daily activity added successfully!";
            lblError.CssClass = "success";
            txtDailyProgress.Text = "";
            gvDailyActivity.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Daily activity addedd successfully!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvDailyActivity_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvDailyActivity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "DailyProgress").ToString();
                LinkButton lnkMoreProgress = (LinkButton)e.Row.FindControl("lnkMoreProgress");
                // HtmlAnchor lnkDataTooltip = (HtmlAnchor)e.Row.FindControl("lnkDataTooltip");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }

                if (strProgressText.Length > 30)
                {
                    lnkMoreProgress.ToolTip = strProgressText;
                    // lnkDataTooltip.Attributes.Add("data-tooltip", strProgressText);

                    //NameHyperLink.Attributes.Add("onmouseover", "ShowToolTip(event, " +
                    //"'" + Server.HtmlEncode(strProgressText) + "','Right');");

                    //NameHyperLink.Attributes.Add("onmouseout", "HideTooTip(event);");
                    //NameHyperLink.Attributes.Add("onmousemove", "MoveToolTip(event,'Right');");
                }
                else
                {
                    lnkMoreProgress.Visible = false;
                }
            }
        }
    }

    protected void gvDailyActivity_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "progresspopup")
        {
            // ModalPopupProgress.Show();
            // lblPopProgress.Text = e.CommandArgument.ToString();
        }
        else if (e.CommandName.ToLower() == "activitydelete")
        {
            int ActivityId = Convert.ToInt32(e.CommandArgument.ToString());

            int result = DBOperations.DeleteFreightActivity(ActivityId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Acitivity Removed Successfully!";
                lblError.CssClass = "success";
                gvDailyActivity.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Current Activity!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    #endregion

    #region EDIT Event

    protected void btnEditFreightDetail_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            FVFreightDetail.ChangeMode(FormViewMode.Edit);
            
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
            CustName.Enabled = false;
            DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision");
            // DBOperations.FillCustomerDivision(dddiv, Convert.ToInt32(Session["CustId"]));
            if (dddiv != null)
            {
                DBOperations.FillCustomerDivision(dddiv, Convert.ToInt32(Session["CustId"]));
            }
            RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
            TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
            RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
            TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
            Label lblTransName = (Label)FVFreightDetail.FindControl("lblTransName");
            Label lblChaName = (Label)FVFreightDetail.FindControl("lblChaName");
            if (rdlbtnCHABy.SelectedValue == "2")
            {
                txtCHABy.Visible = true;
                lblChaName.Visible = true;
            }
            else
            {
                txtCHABy.Visible = false;
                lblChaName.Visible = false;
            }

            if (rdlbtnTransport.SelectedValue == "2")
            {
                
                txtTransportBy.Visible = true;
                lblTransName.Visible = true;
            }
            else
            {
                rdlbtnTransport.SelectedValue = "2";
                txtTransportBy.Visible = false;
                lblTransName.Visible = false;
            }
            
        }
    }

    protected void btnEditAgentPreAlert_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvAgentPreAlert.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnEditCustomerPreAlert_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvCustomerPreAlert.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnEditCAN_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvCAN.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnEditDelivery_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvDelivery.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnEditAgentInvoice_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvAgentInvoice.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    #endregion

    #region UPDATE Event

    protected void btnUpdateFreightDetail_Click(object sender, EventArgs e)
    {
        DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
        DropDownList ddlReason = (DropDownList)FVFreightDetail.FindControl("ddlReason");
        TextBox txtCancelRemark = (TextBox)FVFreightDetail.FindControl("txtCancelRemark");
        if (ddlCancelAllow.SelectedValue == "1")
        {
            if (txtCancelRemark.Text != "" && ddlReason.SelectedValue != "0")
            {
                UpdateFreightDetail();
            }
            else
            {
                lblError.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('Please Select Reason and Remark');", true);
            }
        }
        else
        {
            UpdateFreightDetail();
        }
    }

    protected void UpdateFreightDetail()
    {
        string strCustomer, strShipper, strConsignee;
        string strShipperAddress, strConsigneeAddress, strAgentName, strDescription, strBookingDetails;
        int Count20 = 0, Count40 = 0, NoOfpackages = 0, PackagesType = 0, BranchId = 0, TermsId = 0, ContainerTypeId = 0;
        int PortOfLoadingId = 0, PortOfDischargeId = 0, AgentID = 0, Division = 0, Plant = 0;
        string strLCLVolume = "0", strGrossWeight = "0", strChargeWeight = "0";
        string strInvoiceNo = "", strPONumber = "";
        DateTime dtInvoiceDate = DateTime.MinValue, dtBookingDate = DateTime.MinValue;

        string strDebitNoteAmount = "0"; String strDebitNoteRemark = "";

        int ConsigneeStateID = 0; string strGSTN = "";
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        if (EnqId > 0)
        {
            string strBookingNo = ((TextBox)FVFreightDetail.FindControl("txtJobNo")).Text.Trim();

            int intFreightMode = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddFreightMode")).SelectedValue);
            PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
            PortOfDischargeId = Convert.ToInt32(hdnPortOfDischargedId.Value);

            strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.Trim();
            strConsignee = ((TextBox)FVFreightDetail.FindControl("txtConsignee")).Text.Trim();
            strShipper = ((TextBox)FVFreightDetail.FindControl("txtShipper")).Text.Trim();

            strConsigneeAddress = ((TextBox)FVFreightDetail.FindControl("txtConsigneeAddress")).Text.Trim();
            strShipperAddress = ((TextBox)FVFreightDetail.FindControl("txtShipperAddress")).Text.Trim();

            if (((DropDownList)FVFreightDetail.FindControl("ddConsigneeState")).SelectedValue != "0")
                ConsigneeStateID = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddConsigneeState")).SelectedValue);
            strGSTN = ((TextBox)FVFreightDetail.FindControl("txtGSTN")).Text.Trim();

            AgentID = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddAgent")).SelectedValue);
            strAgentName = ((DropDownList)FVFreightDetail.FindControl("ddAgent")).SelectedItem.Text.Trim();

            TermsId = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddTerms")).SelectedValue);
            BranchId = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddBranch")).SelectedValue);

            Division = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddDivision")).SelectedValue);
            Plant = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddPlant")).SelectedValue);
            RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
            RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
            TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
            TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");

            strInvoiceNo = ((TextBox)FVFreightDetail.FindControl("txtInvoiceNo")).Text.Trim();
            strPONumber = ((TextBox)FVFreightDetail.FindControl("txtPONumber")).Text.Trim();
            strDescription = ((TextBox)FVFreightDetail.FindControl("txtCargoDescription")).Text.Trim();
            strBookingDetails = ((TextBox)FVFreightDetail.FindControl("txtBookingDetails")).Text.Trim();
            TextBox txtNoOfPkgs = (TextBox)FVFreightDetail.FindControl("txtNoOfPkgs");
            TextBox txtLCLVolume = (TextBox)FVFreightDetail.FindControl("txtLCLVolume");
            TextBox txtGrossWeight = (TextBox)FVFreightDetail.FindControl("txtGrossWeight");
            TextBox txtChargWeight = (TextBox)FVFreightDetail.FindControl("txtChargWeight");
            TextBox txtCont20 = (TextBox)FVFreightDetail.FindControl("txtCont20");
            TextBox txtCont40 = (TextBox)FVFreightDetail.FindControl("txtCont40");
            DropDownList ddContainerType = (DropDownList)FVFreightDetail.FindControl("ddContainerType");
            DropDownList ddPackageType = (DropDownList)FVFreightDetail.FindControl("ddPackageType");

            TextBox txtInvoiceDate = (TextBox)FVFreightDetail.FindControl("txtInvoiceDate");
            TextBox txtBookingDate = (TextBox)FVFreightDetail.FindControl("txtBookingDate");

            // TextBox txtDebitNoteAmount = (TextBox)FVFreightDetail.FindControl("txtDebitNoteAmount");
            // TextBox txtDebitNoteRemark = (TextBox)FVFreightDetail.FindControl("txtDebitNoteRemark");

            if (txtNoOfPkgs.Text.Trim() != "")
            {
                NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
            }

            if (ddPackageType.SelectedIndex > 0)
            {
                PackagesType = Convert.ToInt32(ddPackageType.SelectedValue);
            }

            if (txtLCLVolume.Text.Trim() != "")
            {
                strLCLVolume = txtLCLVolume.Text.Trim();
            }

            if (txtGrossWeight.Text.Trim() != "")
            {
                strGrossWeight = txtGrossWeight.Text.Trim();
            }

            if (txtChargWeight.Text.Trim() != "")
            {
                strChargeWeight = txtChargWeight.Text.Trim();
            }

            if (intFreightMode != 1) // SEA / Breakbulk
            {
                ContainerTypeId = Convert.ToInt32(ddContainerType.SelectedValue);
            }
            if (txtCont20.Text.Trim() != "")
            {
                Count20 = Convert.ToInt32(txtCont20.Text.Trim());
            }
            if (txtCont40.Text.Trim() != "")
            {
                Count40 = Convert.ToInt32(txtCont40.Text.Trim());
            }

            if (txtInvoiceDate.Text.Trim() != "")
            {
                dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
            }
            if (txtBookingDate.Text.Trim() != "")
            {
                dtBookingDate = Commonfunctions.CDateTime(txtBookingDate.Text.Trim());
            }

            //if (txtDebitNoteAmount.Text.Trim() != "")
            //    strDebitNoteAmount = txtDebitNoteAmount.Text.Trim();

            //strDebitNoteRemark = txtDebitNoteRemark.Text.Trim();

            hdnGSTNo.Value = strGSTN;

            int result = DBOperations.UpdateFreightBooking(strBookingNo, EnqId, intFreightMode, strCustomer, strConsignee, strShipper,
                         strConsigneeAddress, ConsigneeStateID, strGSTN, strShipperAddress, PortOfLoadingId, PortOfDischargeId, TermsId, BranchId, strAgentName,
                         AgentID, ContainerTypeId, Count20, Count40, strLCLVolume,
                         NoOfpackages, PackagesType, strGrossWeight, strChargeWeight, strInvoiceNo, dtInvoiceDate, strPONumber, strDescription, Division, Plant,
                         Convert.ToInt16(rdlbtnTransport.SelectedValue), Convert.ToInt16(rdlbtnCHABy.SelectedValue), txtTransportBy.Text, txtCHABy.Text,
                         dtBookingDate, strBookingDetails, LoggedInUser.glUserId);


            if (result == 0)
            {
                //if (strDebitNoteAmount != "")
                //{
                //    DBOperations.UpdateFreightDebitNote(EnqId, strDebitNoteAmount, strDebitNoteRemark, LoggedInUser.glUserId);
                //}
                // get port of dischargeid

                DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
                if (ddlCancelAllow.SelectedValue == "1")
                {
                    DropDownList ddlReason1 = (DropDownList)FVFreightDetail.FindControl("ddlReason");
                    string Remark = ((TextBox)FVFreightDetail.FindControl("txtCancelRemark")).Text.Trim();
                    result = DBOperations.FR_InsJobStatus(EnqId, 25, ddlReason1.SelectedItem.Text, Remark, LoggedInUser.glModuleId, LoggedInUser.glFinYearId, LoggedInUser.glUserId);
                }

                DataSet dsDetail = DBOperations.GetOperationDetail(EnqId);

                if (dsDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
                    {
                        hdnCountryCode.Value = dsDetail.Tables[0].Rows[0]["CountryCode"].ToString();
                    }
                }

                gvCanInvoice.DataBind();
                // UPDATE CAN INVOICE ITEMS IF GSTN OR CONSIGNEE STATE BEEN CHANGED
                if (gvCanInvoice.Rows.Count > 0)
                {
                    for (int i = 0; i < gvCanInvoice.Rows.Count; i++)
                    {
                        if (gvCanInvoice.DataKeys[i].Value.ToString() != "")
                        {
                            int InvoiceId = Convert.ToInt32(gvCanInvoice.DataKeys[i].Value);

                            #region UPDATE CAN INVOICE ITEMS IF GSTN OR CONSIGNEE STATE BEEN CHANGED

                            string strExchangeRate = "1", strTaxAmount = "0", strAmount = "0", strTotalAmount = "0";
                            int UnitOfMeasure = 0, InvoiceItemId = 0;
                            decimal ChargeableWeight = 1, decRate = 0, ExchangeRate = 0, Amount = 0, Volume = 0;

                            DataSet dsGetInvcDetail = DBOperations.GetCANInvoiceDetailAsPerLid(InvoiceId);
                            //Label lblInvoiceItemId = (Label)gvCanInvoice.Rows[i].FindControl("lblInvoiceItemId");

                            if (dsGetInvcDetail.Tables[0].Rows[0]["InvoiceItemId"].ToString() != "")
                                InvoiceItemId = Convert.ToInt32(dsGetInvcDetail.Tables[0].Rows[0]["InvoiceItemId"].ToString());

                            if (dsGetInvcDetail.Tables[0].Rows[0]["ExchangeRate"].ToString() != "")
                                strExchangeRate = dsGetInvcDetail.Tables[0].Rows[0]["ExchangeRate"].ToString();

                            if (dsGetInvcDetail.Tables[0].Rows[0]["TaxAmount"].ToString() != "")
                                strTaxAmount = dsGetInvcDetail.Tables[0].Rows[0]["TaxAmount"].ToString();

                            if (dsGetInvcDetail.Tables[0].Rows[0]["Amount"].ToString() != "")
                                strAmount = dsGetInvcDetail.Tables[0].Rows[0]["Amount"].ToString();

                            if (dsGetInvcDetail.Tables[0].Rows[0]["TotalAmount"].ToString() != "")
                                strTotalAmount = dsGetInvcDetail.Tables[0].Rows[0]["TotalAmount"].ToString();

                            if (dsGetInvcDetail.Tables[0].Rows[0]["UOM"].ToString() != "")
                                UnitOfMeasure = Convert.ToInt32(dsGetInvcDetail.Tables[0].Rows[0]["UOM"].ToString());

                            if (dsGetInvcDetail.Tables[0].Rows[0]["Amount"].ToString() != "")
                                Amount = Convert.ToDecimal(dsGetInvcDetail.Tables[0].Rows[0]["Amount"].ToString());

                            if (dsGetInvcDetail.Tables[0].Rows[0]["ExchangeRate"] != DBNull.Value)
                                ExchangeRate = Convert.ToDecimal(dsGetInvcDetail.Tables[0].Rows[0]["ExchangeRate"]);

                            if (dsGetInvcDetail.Tables[0].Rows[0]["Rate"] != DBNull.Value)
                                decRate = Convert.ToDecimal(dsGetInvcDetail.Tables[0].Rows[0]["Rate"]);

                            // start - update chargeable weight as well (for per kg items)
                            if (UnitOfMeasure == (Int32)EnumUnit.perKG)
                            {
                                ChargeableWeight = Convert.ToDecimal(strChargeWeight);

                                if (ChargeableWeight != 0)
                                {
                                    if (decRate != 0)
                                    {
                                        Amount = decRate * ChargeableWeight * ExchangeRate;
                                        strAmount = Amount.ToString();
                                    }
                                }
                            }
                            else if (UnitOfMeasure == (Int32)EnumUnit.PercentOf)
                            {
                                DataSet dsGetCANPercentDetail = DBOperations.GetCANPercentDetail(EnqId, InvoiceItemId);
                                if (dsGetCANPercentDetail != null && dsGetCANPercentDetail.Tables[0].Rows.Count > 0)
                                {
                                    int PercentDetailId = 0;
                                    decimal TotalAmnt = 0, CalTotalAmnt = 0;
                                    for (int c = 0; c < dsGetCANPercentDetail.Tables[0].Rows.Count; c++)
                                    {
                                        if (dsGetCANPercentDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                                            PercentDetailId = Convert.ToInt32(dsGetCANPercentDetail.Tables[0].Rows[0]["lid"]);
                                        TotalAmnt = Convert.ToDecimal(dsGetCANPercentDetail.Tables[0].Rows[c]["TotalAmount"].ToString());
                                        CalTotalAmnt = CalTotalAmnt + (System.Math.Round((TotalAmnt * (decRate / 100.00m)), 2, MidpointRounding.AwayFromZero));

                                        // start - update amount in percent CAN table
                                        if (PercentDetailId != 0)
                                        {
                                            int UpdateMsg = DBOperations.UpdateCANPercentDetail(PercentDetailId, TotalAmnt);
                                        }
                                        // end - update amount in percent CAN table
                                    }

                                    Amount = CalTotalAmnt + ExchangeRate;
                                    strAmount = Amount.ToString();
                                }
                            }
                            else if (UnitOfMeasure == (Int32)EnumUnit.perCBM)
                            {
                                Volume = Convert.ToDecimal(hdnVolume.Value);

                                //if (Volume == 0)
                                //{
                                //    lblError.Text = "Please check CBM Value! " + hdnVolume.Value;
                                //    lblInvoiceError.Text = "Please check CBM Value! " + hdnVolume.Value;

                                //    lblError.CssClass = "errorMsg";
                                //    lblInvoiceError.CssClass = "errorMsg";
                                //    return false;
                                //}

                                Amount = (decRate * Volume) * ExchangeRate;
                                strAmount = Amount.ToString();
                            }
                            // end - update chargeable weight as well (for per kg items)

                            // start - All gst tax amount calculations
                            decimal TaxRate = 0;
                            if (dsGetInvcDetail.Tables[0].Rows[0]["InvoiceItemId"].ToString() != "")
                                InvoiceItemId = Convert.ToInt32(dsGetInvcDetail.Tables[0].Rows[0]["InvoiceItemId"].ToString());

                            decimal CGstTax = 0, CGstTaxAmt = 0, SGstTax = 0, SGstTaxAmt = 0, IGstTax = 0, IGstTaxAmt = 0;

                            if (hdnCountryCode.Value.Trim().ToLower() == "india")
                            {
                                if (strGSTN != "")
                                {
                                    int StateCode = Convert.ToInt32(strGSTN.Substring(0, 2));
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
                                }
                            }
                            // end - All gst tax amount calculations

                            // Total = Amount + CGST (Rs) + SGST (Rs) + IGST (Rs)
                            strTotalAmount = Convert.ToDecimal(Amount + CGstTaxAmt + SGstTaxAmt + IGstTaxAmt).ToString();

                            if (strExchangeRate != "0" && strAmount != "0" && strTotalAmount != "0")
                            {
                                int Result = DBOperations.UpdateCANInvoiceDetail(InvoiceId, decRate, strExchangeRate, strTaxAmount, strAmount, strTotalAmount, LoggedInUser.glUserId,
                                                                                     CGstTax, CGstTaxAmt, SGstTax, SGstTaxAmt, IGstTax, IGstTaxAmt);

                                if (Result == 0)
                                {

                                }
                            }
                            #endregion
                        }
                    }
                }

                EditBtnInvisible();

                gvCanInvoice.DataBind();
                lblError.Text = "Freight Detail Updated Successfully !";
                lblError.CssClass = "success";
                FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);
                GetFreightDetail(EnqId);
            }
            else if (result == 2)
            {
                lblError.Text = "Job No Already Exist!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Booking Details Not Found!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_EnqId Check
        else
        {
            Response.Redirect("OperationTrack.aspx");
        }
    }

    protected void btnUpdateAgentPreAlert_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strMBLNo, strHBLNo, strVesselName, strVesselNumber, strFinalAgent;
        int FinalAgentID = 0;

        DateTime dtMBLDate = DateTime.MinValue, dtHBLDate = DateTime.MinValue,
        dtETDDate = DateTime.MinValue, dtETADate = DateTime.MinValue;

        strMBLNo = ((TextBox)fvAgentPreAlert.FindControl("txtMBLNo")).Text.Trim();
        strHBLNo = ((TextBox)fvAgentPreAlert.FindControl("txtHBLNo")).Text.Trim();
        strVesselName = ((TextBox)fvAgentPreAlert.FindControl("txtVesselName")).Text.Trim();
        strVesselNumber = ((TextBox)fvAgentPreAlert.FindControl("txtVesselNumber")).Text.Trim();

        DropDownList ddFinalAgent = (DropDownList)fvAgentPreAlert.FindControl("ddFinalAgent");
        strFinalAgent = ddFinalAgent.SelectedItem.Text;
        FinalAgentID = Convert.ToInt32(ddFinalAgent.SelectedValue);

        TextBox txtMBLDate = (TextBox)fvAgentPreAlert.FindControl("txtMBLDate");
        TextBox txtHBLDate = (TextBox)fvAgentPreAlert.FindControl("txtHBLDate");
        TextBox txtETDDate = (TextBox)fvAgentPreAlert.FindControl("txtETDDate");
        TextBox txtETADate = (TextBox)fvAgentPreAlert.FindControl("txtETADate");


        if (txtMBLDate.Text.Trim() != "")
        {
            dtMBLDate = Commonfunctions.CDateTime(txtMBLDate.Text.Trim());
        }

        if (txtHBLDate.Text.Trim() != "")
        {
            dtHBLDate = Commonfunctions.CDateTime(txtHBLDate.Text.Trim());
        }

        if (txtETDDate.Text.Trim() != "")
        {
            dtETDDate = Commonfunctions.CDateTime(txtETDDate.Text.Trim());
        }
        if (txtETADate.Text.Trim() != "")
        {
            dtETADate = Commonfunctions.CDateTime(txtETADate.Text.Trim());
        }

        int result = DBOperations.UpdateAgentPreAlert(EnqId, strMBLNo, strHBLNo, strVesselName, strVesselNumber,
               dtMBLDate, dtHBLDate, dtETDDate, dtETADate, strFinalAgent, FinalAgentID, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Agent PreAlert Detail Updated Successfully!";
            lblError.CssClass = "success";

            fvAgentPreAlert.ChangeMode(FormViewMode.ReadOnly);
            GetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Agent PreAlert Details Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnUpdateCustomerPreAlert_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        DateTime ShippedOnBoardDate = DateTime.MinValue, PreAlertToCustDate = DateTime.MinValue;

        string strCustomerEmail = ((TextBox)fvCustomerPreAlert.FindControl("txtCustEmail")).Text.Replace(";", ",").Trim();

        strCustomerEmail = strCustomerEmail.Replace(" ", "");
        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();

        strCustomerEmail = strCustomerEmail.TrimEnd(',');

        TextBox txtShippedDate = (TextBox)fvCustomerPreAlert.FindControl("txtShippedDate");
        TextBox txtPreAlertDate = (TextBox)fvCustomerPreAlert.FindControl("txtPreAlertDate");

        if (txtShippedDate.Text.Trim() != "")
        {
            ShippedOnBoardDate = Commonfunctions.CDateTime(txtShippedDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Shipped On Board Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtPreAlertDate.Text.Trim() != "")
        {
            PreAlertToCustDate = Commonfunctions.CDateTime(txtPreAlertDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter PreAlert Date!";
            lblError.CssClass = "errorMsg";

            return;
        }

        int result = DBOperations.UpdateCustomerPreAlert(EnqId, ShippedOnBoardDate, PreAlertToCustDate, strCustomerEmail, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Customer PreAlert Detail Updated Successfully!";
            lblError.CssClass = "success";

            fvCustomerPreAlert.ChangeMode(FormViewMode.ReadOnly);
            GetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Customer PreAlert Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnUpdateCAN_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strIGMNo, strItemNo, strRemark;
        DateTime dtIGMDate = DateTime.MinValue;
        DateTime dtATADate = DateTime.MinValue;

        strIGMNo = ((TextBox)fvCAN.FindControl("txtIGMNo")).Text.Trim();
        strItemNo = ((TextBox)fvCAN.FindControl("txtItemNo")).Text.Trim();
        strRemark = ((TextBox)fvCAN.FindControl("txtCANRemark")).Text.Trim();

        TextBox txtIGMDate = (TextBox)fvCAN.FindControl("txtIGMDate");
        TextBox txtATADate = (TextBox)fvCAN.FindControl("txtATA");

        if (txtIGMDate.Text.Trim() == "" || txtATADate.Text.Trim() == "")
        {
            lblError.Text = "Please Enter IGM and ATA Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtIGMDate.Text.Trim() != "")
        {
            dtIGMDate = Commonfunctions.CDateTime(txtIGMDate.Text.Trim());
        }
        if (txtATADate.Text.Trim() != "")
        {
            dtATADate = Commonfunctions.CDateTime(txtATADate.Text.Trim());
        }

        int result = DBOperations.UpdateCargoArrival(EnqId, strIGMNo, dtIGMDate, dtATADate, strItemNo, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "CAN Detail Updated Successfully!";
            lblError.CssClass = "success";

            fvCAN.ChangeMode(FormViewMode.ReadOnly);
            GetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "CAN Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnUpdateDODetail_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strCHAName, strDOIssuedTo, strChequeNo, strRemark;
        int PaymentTerm = 0, PaymentId = 0;
        decimal DOAmount = 0;
        DateTime dtChequeDate = DateTime.MinValue;

        TextBox txtChequeDate = (TextBox)fvDelivery.FindControl("txtChequeDate");
        TextBox txtAmount = (TextBox)fvDelivery.FindControl("txtAmount");

        strCHAName = ((TextBox)fvDelivery.FindControl("txtChaName")).Text.Trim();
        PaymentTerm = Convert.ToInt32(((DropDownList)fvDelivery.FindControl("ddPaymentsTerms")).SelectedValue);
        PaymentId = Convert.ToInt32(((DropDownList)fvDelivery.FindControl("ddPaymentType")).SelectedValue);

        strDOIssuedTo = ((TextBox)fvDelivery.FindControl("txtDoIssuedTo")).Text.Trim();
        strChequeNo = ((TextBox)fvDelivery.FindControl("txtChequeNo")).Text.Trim();
        strRemark = ((TextBox)fvDelivery.FindControl("txtDORemark")).Text.Trim();

        if (txtAmount.Text.Trim() != "")
        {
            DOAmount = Convert.ToDecimal(txtAmount.Text.Trim());
        }

        if (txtChequeDate.Text.Trim() != "")
        {
            dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());
        }

        int result = DBOperations.UpdateDeliveryOrder(EnqId, strCHAName, PaymentTerm, strDOIssuedTo, PaymentId, strChequeNo,
            dtChequeDate, DOAmount, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "DO Detail Updated Successfully!";
            lblError.CssClass = "success";

            fvDelivery.ChangeMode(FormViewMode.ReadOnly);
            GetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "DO Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnUpdateAgentInvoice_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strBillNumber, strRemark;
        decimal decBillAmount = 0;

        DateTime dtBillDate = DateTime.MinValue;

        TextBox txtBillNumber = (TextBox)fvAgentInvoice.FindControl("txtBillNumber");
        TextBox txtBillAmount = (TextBox)fvAgentInvoice.FindControl("txtBillAmount");
        TextBox txtBillDate = (TextBox)fvAgentInvoice.FindControl("txtBillDate");

        TextBox txtRemark = (TextBox)fvAgentInvoice.FindControl("txtRemark");
        DropDownList ddCurrency = (DropDownList)fvAgentInvoice.FindControl("ddCurrency");

        strBillNumber = txtBillNumber.Text.Trim();
        strRemark = txtRemark.Text.Trim();

        if (txtBillAmount.Text.Trim() != "")
        {
            decBillAmount = Convert.ToDecimal(txtBillAmount.Text.Trim());
        }

        if (txtBillDate.Text.Trim() != "")
        {
            dtBillDate = Commonfunctions.CDateTime(txtBillDate.Text.Trim());
        }

        int result = DBOperations.UpdateBillingDetail(EnqId, strBillNumber, dtBillDate, decBillAmount, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Billing  Updated Successfully!";
            lblError.CssClass = "success";
            fvAgentInvoice.ChangeMode(FormViewMode.ReadOnly);
            GetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Billing Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void ddPaymentsTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddPaymentsTerms = (DropDownList)fvDelivery.FindControl("ddPaymentsTerms");
        RequiredFieldValidator RFVPaytype = (RequiredFieldValidator)fvDelivery.FindControl("RFVPaytype");

        if (ddPaymentsTerms.SelectedValue == "2")
            RFVPaytype.Enabled = true;
        else
            RFVPaytype.Enabled = false;

    }

    #endregion

    #region CANCEL Event

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("OperationTrack.aspx");
    }

    protected void btnCancelFreightDetail_Click(object sender, EventArgs e)
    {
        FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnCancelAgentPreAlert_Click(object sender, EventArgs e)
    {
        fvAgentPreAlert.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnCancelCustomerPreAlert_Click(object sender, EventArgs e)
    {
        fvCustomerPreAlert.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnCancelCAN_Click(object sender, EventArgs e)
    {
        fvCAN.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnCancelDODetail_Click(object sender, EventArgs e)
    {
        fvDelivery.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    protected void btnCancelAgentInvoice_Click(object sender, EventArgs e)
    {
        fvAgentInvoice.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    #endregion

    #region Agent Invoice GridView

    protected void GridViewInvoiceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int InvoiceID = 0;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int CurrencyId = 0; decimal decInvoiceAmount = 0;

        string strJBNumber = "", strAgentName = "", strInvoiceNo = "";

        DateTime dtJBDate = DateTime.MinValue, dtInvoiceDate = DateTime.MinValue;

        GridViewRow gvrow = GridViewInvoiceDetail.Rows[e.RowIndex];

        InvoiceID = Convert.ToInt32(GridViewInvoiceDetail.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtJBNumber = (TextBox)gvrow.FindControl("txtJBNumber");
        TextBox txtJBDate = (TextBox)gvrow.FindControl("txtJBDate");
        TextBox txtAgentInvoiceName = (TextBox)gvrow.FindControl("txtAgentInvoiceName");
        TextBox txtAgentInvoiceNo = (TextBox)gvrow.FindControl("txtAgentInvoiceNo");
        TextBox txtInvoiceAmount = (TextBox)gvrow.FindControl("txtInvoicAmount");
        TextBox txtAgentInvoiceDate = (TextBox)gvrow.FindControl("txtAgentInvoiceDate");
        DropDownList ddAgentCurrency = (DropDownList)gvrow.FindControl("ddAgentCurrency");

        strJBNumber = txtJBNumber.Text.Trim();
        strAgentName = txtAgentInvoiceName.Text.Trim();
        strInvoiceNo = txtAgentInvoiceNo.Text.Trim();
        CurrencyId = Convert.ToInt32(ddAgentCurrency.SelectedValue);

        if (txtJBDate.Text.Trim() != "")
        {
            dtJBDate = Commonfunctions.CDateTime(txtJBDate.Text.Trim());
        }

        if (txtAgentInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtAgentInvoiceDate.Text.Trim());
        }

        if (txtInvoiceAmount.Text.Trim() != "")
        {
            decInvoiceAmount = Convert.ToDecimal(txtInvoiceAmount.Text.Trim());
        }

        if (txtInvoiceAmount.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Invoice Amount!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtAgentInvoiceDate.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Invoice Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (strAgentName == "")
        {
            lblError.Text = "Please Enter Agent Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (strInvoiceNo == "")
        {
            lblError.Text = "Please Enter Agent Invoice No!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (CurrencyId == 0)
        {
            lblError.Text = "Please Select Invoice Currency!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int result = DBOperations.UpdateFreightAgentInvoice(InvoiceID, EnqId, strJBNumber, dtJBDate, strAgentName, strInvoiceNo,
                dtInvoiceDate, decInvoiceAmount, CurrencyId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Agent Invoice Detail Updated Successfully!";
            lblError.CssClass = "success";

            e.Cancel = true;

            GridViewInvoiceDetail.EditIndex = -1;
            GridViewInvoiceDetail.DataBind();

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion
    
    #region Job Cancel
    protected void ddlCancelAllow_SelectedIndexChanged(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
        string Remark = ((TextBox)FVFreightDetail.FindControl("txtCancelRemark")).Text;
        if (ddlCancelAllow.SelectedValue == "1")
        {
            DataTable dtCustPreAlert = new DataTable();
            dtCustPreAlert = DBOperations.Get_FRPrealertDetail(EnqId);
            if (dtCustPreAlert.Rows.Count > 0)
            {
                FVFreightDetail.FindControl("trCancel").Visible = true;
            }
            else
            {
                FVFreightDetail.FindControl("trCancel").Visible = false;
                ddlCancelAllow.SelectedValue = "2";
                lblError.Text = "Can Not Allow to cancel job!!!!";
                lblError.CssClass = "errorMsg";
            }

            if (Remark != "")
            {
                Button btnUpdate = (Button)FVFreightDetail.FindControl("btnUpdateFreightDetail");
                btnUpdate.Visible = false;
                FVFreightDetail.FindControl("trCancel").Visible = false;
                ddlCancelAllow.SelectedValue = "2";
                lblError.Text = "Job has cancelled!!!!";
                lblError.CssClass = "errorMsg";
            }
            //FVJobDetail.FindControl("trCancel").Visible = true;
        }
        else
        {
            FVFreightDetail.FindControl("trCancel").Visible = false;
        }
    }
    #endregion

    protected void lbnAgentInvoice_Click(object sender, EventArgs e)
    {
        LinkButton DocPath = fvAdvice.FindControl("lnkAgentInvoice") as LinkButton;
        string FilePath = DocPath.CommandArgument;
        lblError.Text = FilePath;
        DownloadDocument(FilePath);
    }

    #region E-bill dispatch
    protected void gvBillDispatchDocDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewMailDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string Msg = e.CommandArgument.ToString();
            dvMailSend.InnerHtml = Msg;
            dvMailSend.Visible = true;
            ModalPopupExtender1.Show();
        }
    }

    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    protected void GetEBillDetail()
    {
        DataView dvDoc = (DataView)SqlDataSourceBillDispatchDoc.Select(DataSourceSelectArguments.Empty);
        DataTable dtBillDoc = dvDoc.Table;
        if (!dtBillDoc.Columns.Contains("NewColumn"))
        {
            DataColumn newCol = new DataColumn("NewColumn", typeof(string));
            newCol.AllowDBNull = true;
            dtBillDoc.Columns.Add(newCol);
        }

        int j = 0;
        foreach (DataRow rows in dtBillDoc.Rows)
        {
            string DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtBillDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            j++;
        }
        gvBillDispatchDocDetail.DataSource = dtBillDoc;
        if (dtBillDoc.Columns.Contains("NewColumn"))
        {
            if (gvBillDispatchDocDetail.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtBillDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvBillDispatchDocDetail.Columns.Add(test);
            }
        }
        gvBillDispatchDocDetail.DataBind();
    }

    protected void btnMailResend_Click(object sender, EventArgs e)
    {
        GenerateEbillEmailDraft();
        //divResendEmail.Visible = true;
        ModalPopupExtender1.Show();
    }

    private void GenerateEbillEmailDraft()
    {
        int JobId = Convert.ToInt32(Session["EnqId"]);

        string MessageBody = "";
        string strJobRefNo = "", strCustName = "", strConsigneeName = "", strCustRefNo = "", strToMail = "", strJobType = "";
        int strAgencyCnt = 0, strRIMCnt = 0, strColSpan = 0, strCols = 0;
        string args; string[] args1; int tot; int AmtTot = 0;
        DataView dv = DBOperations.GetUserDetail(Convert.ToString(LoggedInUser.glUserId));
        DataTable dt = dv.ToTable(true, "sEmail");
        DataSet dsAlertDetail = DBOperations.GetBillDispatchDetail(JobId, JobRefNO);

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strCustName = dsAlertDetail.Tables[0].Rows[0]["Customer"].ToString();
            strConsigneeName = dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strJobRefNo = dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            strCustRefNo = dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            strJobType = dsAlertDetail.Tables[0].Rows[0]["JobType"].ToString();
            txtSubject1.Text = "E-Bill Dispatch/Job No :- " + strJobRefNo + " /Customer Reference No :- " + strCustRefNo + "";
            strAgencyCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntAgency"].ToString());
            strRIMCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntRIM"].ToString());
            strToMail = dsAlertDetail.Tables[0].Rows[0]["Email"].ToString();
            txtMailTo.Text = strToMail;
            //txtMailTo.Text = "developer1@babajishivram.com, developer2@babajishivram.com";
            txtMailCC1.Text = dt.Rows[0]["sEmail"].ToString() + "," + "query.billing@babajishivram.com";
        }
        else
        {
            lblError.Text = "Booking Details Not Found! Please check details.";
            lblError.CssClass = "errorMsg";
            return;
        }

        //////////////////////////////////////////////////////////////////////////////////
        if (strAgencyCnt > strRIMCnt)
        {
            strCols = strAgencyCnt;
        }
        else
        {
            strCols = strRIMCnt;
        }
        StringBuilder strStyle = new StringBuilder();
        strStyle = strStyle.Append("<html><body style='height:100%; width:100%; font-family:Arial; font-style:normal; font-size:9pt; color:#000;");

        // body header
        strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
        strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");

        strStyle = strStyle.Append(@"<tr><td>" + "Dear Sir / Madam, " + "<br />");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Kindly find the attached E-Invoices of Subject Shipment and details are as below. ");
        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td>" + "Customer Name :- " + strCustName);
        strStyle = strStyle.Append(@"</td></tr>");
        if (strJobType == "1")
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Consignee  Name :- " + strConsigneeName + "<br />");
        }
        else
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Shipper  Name :- " + strConsigneeName + "<br />");
        }

        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
        strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
        strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='50%'><colgroup><col width='40%' /><col width='35%' /><col width='30%' /><col width='30%' /></colgroup>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Job Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString() + "</td></tr>");
        strStyle = strStyle.Append(@"<tr><td style='border: 1px solid #ccc; background-color:#99CCFF'>Cust Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString() + "</td></tr>");

        ///Agency Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'  colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        tot = 0; AmtTot = 0;

        //RIM Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        AmtTot = 0; tot = 0;

        strStyle = strStyle.Append(@"</table></p></div></div></td>");

        // body footer
        strStyle = strStyle.Append(@"<tr><td>" + "Any billing related query or issue, kindly drop an e-mail to query.billing@babajishivram.com" + "<br/><br/>");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Thanks & Regards");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Babaji Shivram Clearing And Carriers Pvt Ltd");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"</table></td></tr>");
        strStyle = strStyle.Append(@"</center></body></html>");

        MessageBody = strStyle.ToString();

        /////////////////////////////////////////////////////////////////////////////////////
        divPreviewEmailBillDispatch.InnerHtml = MessageBody;

        DataTable dtDoc = DBOperations.GetBillDoc(JobId);
        DataColumn newCol = new DataColumn("NewColumn", typeof(string));
        newCol.AllowDBNull = true;
        dtDoc.Columns.Add(newCol);
        int j = 0;
        string DocPath = "";
        foreach (DataRow rows in dtDoc.Rows)
        {
            DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            //FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            //j++;
        }
        gvDocAttach.DataSource = dtDoc;
        if (dtDoc.Columns.Contains("NewColumn"))
        {
            if (gvDocAttach.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvDocAttach.Columns.Add(test);
            }
        }
        gvDocAttach.DataBind();

        ModalPopupExtender1.Show();
    }

    protected void btnSendEmail_Click1(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        //lblStatus.Text = "Processing...";
        //lblStatus.Visible = true;

        if (txtMailTo.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Customer Email & Subject!";
            lblError.CssClass = "errorMsg";
            ModalPopupExtender1.Hide();
        }
        else
        {

            // Send Email
            bool bEMailSucess = SendPreAlertEmail1();
            //bool bEMailSucess = true;
            // Update PreAlert Email Sent Status and Customer Email 

            if (bEMailSucess == true)
            {
                int Result = DBOperations.AddJobNotofication(JobId, 1, 14, txtMailTo.Text, txtMailCC1.Text, txtSubject1.Text, divPreviewEmailBillDispatch.InnerHtml, "0", LoggedInUser.glUserId);
                ModalPopupExtender1.Hide();
                //lblStatus.Text = "";

                if (Result == 0)
                {
                    lblError.Text = "Customer Email Sent Successfully!";
                    lblError.CssClass = "success";
                    //dvMailSend.Visible = false;
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "PreAlert Email Already Sent!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void ImageClose_Click(object sender, ImageClickEventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    private bool SendPreAlertEmail1()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtMailTo.Text.Trim();
        strCCEmail = txtMailCC1.Text.Trim();
        strSubject = txtSubject1.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", ",").Trim();
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();

        bool bEmailSucces = false;

        if (strCustomerEmail == "" || strSubject == "")
        {
            //lblPopMessageEmail.Text = "Please Enter Customer Email!";
            //       lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            MessageBody = divPreviewEmailBillDispatch.InnerHtml;

            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvDocAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }
    #endregion


    #region Dispatch Billing Dept

    protected void lnkPODCopyDownoad_Click(object sender, EventArgs e)
    {
        LinkButton lnkPODDownload = (LinkButton)sender;

        string FilePath = lnkPODDownload.CommandArgument;

        DownloadDocument(FilePath);
    }
    #endregion

    
    #region Vendor Payment Request
    protected void btnAllowFundRequest_Click(object sender, EventArgs e)
    {
        if (rblFundRequest.SelectedIndex == -1)
        {
            lblError.Text = "Please Check atleast One Option For Payment Request!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int EnqId = Convert.ToInt32(Session["EnqId"]);
            bool isAllowed = false;

            if (rblFundRequest.SelectedValue == "1")
            {
                isAllowed = true;
            }

            int Result = DBOperations.UpdateJobFundRequestStatus(EnqId, isAllowed, 2, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Payment Request Status Updated Successfully!";
                lblError.CssClass = "success";

                GetFreightDetail(EnqId);
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Job Detail not found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + lblError.Text + "')", true);
    }
    #endregion
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

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        int CustomerId = 0;
        TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
        DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;

        DataSet dsGetCustIdByName = DBOperations.GetCustomerIdByName(CustName.Text);

        if (dsGetCustIdByName.Tables[0].Rows.Count > 0)
        {
            string strCustId = "";
            strCustId = Convert.ToString(dsGetCustIdByName.Tables[0].Rows[0]["CustomerId"]);
            if (strCustId != "")
            {
                CustomerId = Convert.ToInt32(strCustId);
            }
        }

        DBOperations.FillCustomerDivision(dddiv, CustomerId);
        DropDownList ddplant = (DropDownList)FVFreightDetail.FindControl("ddPlant");
        DataSet dsCust = DBOperations.GetCustomerIdByName(CustName.Text);
        if (dsCust.Tables[0].Rows.Count > 0)
        {
            int CustId = Convert.ToInt32(dsCust.Tables[0].Rows[0]["CustomerId"].ToString());
            DBOperations.FillCustomerDivision(dddiv, CustId);
            // dddiv.Items.Insert(0, "Select");
            dddiv.SelectedValue = "0";
            // dddiv.DataBind();
            ddplant.SelectedValue = "0";
            dddiv.Enabled = true;
            ddplant.Enabled = true;
        }
        else
        {
            Session["CustId"] = "";
            CustName.Text = "";
            CustName.Focus();
            dddiv.Enabled = false;
            ddplant.Enabled = false;
            lblError.Text = "Please enter correct customer name";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;
        int DivisonId = Convert.ToInt32(dddiv.SelectedValue);
        DropDownList ddplant = (DropDownList)FVFreightDetail.FindControl("ddPlant"); ;
        DBOperations.FillCustomerPlant(ddplant, DivisonId);
        //ddDivision.Focus();
    }

    protected void rdlbtnTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
        TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
        Label lblTransName = (Label)FVFreightDetail.FindControl("lblTransName");
        if (rdlbtnTransport.SelectedValue == "1")
        {
            txtTransportBy.Text = "";
            txtTransportBy.Visible = false;
            lblTransName.Visible = false;
        }
        else if (rdlbtnTransport.SelectedValue == "2")
        {
            txtTransportBy.Visible = true;
            lblTransName.Visible = true;
        }
    }

    protected void rdlbtnCHABy_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
        TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
        Label lblChaName = (Label)FVFreightDetail.FindControl("lblChaName");
        if (rdlbtnCHABy.SelectedValue == "1")
        {
            txtCHABy.Text = "";
            txtCHABy.Visible = false;
            lblChaName.Visible = false;
        }
        else if (rdlbtnCHABy.SelectedValue == "2")
        {
            txtCHABy.Visible = true;
            lblChaName.Visible = false;
        }
    }

    protected void btnJobDelivery_Click(object sender, EventArgs e)
    {
        DateTime TruckRequestDate = DateTime.MinValue, JobDeliveryDate = DateTime.MinValue;
        string DeliveryIns = "", DeliveryDestination = "", DeliveryAddress = "";

        bool bTransportationByBabaji = false;

        int JobId = Convert.ToInt32(Session["JobId"]);
        int Result = -123;

        if (txtTruckRequestDate.Text.Trim() != "")
        {
            TruckRequestDate = Commonfunctions.CDateTime(txtTruckRequestDate.Text.Trim());
        }

        if (txtJobDeliveryDate.Text.Trim() != "")
        {
            JobDeliveryDate = Commonfunctions.CDateTime(txtJobDeliveryDate.Text.Trim());
        }
        if (txtDeliveryIns.Text.Trim() != "")
        {
            DeliveryIns = txtDeliveryIns.Text.Trim();
        }
        if (txtDeliveryDestination.Text.Trim() != "")
        {
            DeliveryDestination = txtDeliveryDestination.Text.Trim();
        }
        if (txtDeliveryAddress.Text.Trim() != "")
        {
            DeliveryAddress = txtDeliveryAddress.Text.Trim();
        }

        if (lblTransportationBy.Text.Trim().ToLower() == "babaji")
        {
            bTransportationByBabaji = true;
        }

        if (JobDeliveryDate == DateTime.MinValue && DeliveryIns == "" && DeliveryDestination == "" && DeliveryAddress == "")
        {
            lblError.Text = "Please Enter Delivery Information!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            Result = DBOperations.FR_UPDDeliveryDetails(JobId, 0, 0, TruckRequestDate, JobDeliveryDate, DeliveryIns,
                DeliveryDestination, DeliveryAddress, bTransportationByBabaji, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Delivery Detail Updated Successfully";
                lblError.CssClass = "success";

            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Please First Update Job Planning Details!";
                lblError.CssClass = "warning";
            }
            else
            {
                lblError.Text = "Please Enter Required Details!";
                lblError.CssClass = "warning";
            }
        }
    }

    protected void GridViewDelivery_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "update")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewDelivery.DataKeys[gvrow.RowIndex].Value.ToString();

            TextBox txtDeliveryDate = (TextBox)gvrow.FindControl("txtDeliveryDate");
            string strDeliveryDate = txtDeliveryDate.Text.Trim();
            TextBox txtEmptyContainerDate = (TextBox)gvrow.FindControl("txtEmptyContainerDate");
            string strEmptyContainerDate = txtEmptyContainerDate.Text.Trim();

            DataSourceDelivery.UpdateParameters["lId"].DefaultValue = strlId;

            if (strDeliveryDate != "")
            {
                strDeliveryDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["DeliveryDate"].DefaultValue = strDeliveryDate;
            }

            if (strEmptyContainerDate != "")
            {
                strEmptyContainerDate = Commonfunctions.CDateTime(txtEmptyContainerDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["EmptyContRetrunDate"].DefaultValue = strEmptyContainerDate;
            }

            TextBox txtCargoRecvdby = (TextBox)gvrow.FindControl("txtCargoRecvdby");
            string CargoRecvdby = txtCargoRecvdby.Text.Trim();
            DataSourceDelivery.UpdateParameters["CargoReceivedby"].DefaultValue = CargoRecvdby;
            //
            string PODPath = "";
            FileUpload fuattach = (FileUpload)gvrow.FindControl("fuPODAttchment");
            HiddenField hdnPODPath = (HiddenField)gvrow.FindControl("hdnPODPath");

            string FileName = fuattach.FileName;
            FileName = FileServer.ValidateFileName(FileName);

            if (fuattach.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "PODFiles\\";

                string ServerFilePath = FileServer.GetFileServerDir();

                if (ServerFilePath == "")
                {
                    // Application Directory Path
                    ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
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

                if (fuattach.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);
                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;

                    }

                    fuattach.SaveAs(ServerFilePath + FileName);
                }

                PODPath = FilePath + FileName;
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }

            else if (hdnPODPath.Value != "")
            {
                PODPath = hdnPODPath.Value;
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }

            //

            TextBox txtNFormNo = (TextBox)gvrow.FindControl("txtNFormNo");
            string NFormNo = txtNFormNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["NFormNo"].DefaultValue = NFormNo;

            TextBox txtNFormDate = (TextBox)gvrow.FindControl("txtNFormDate");
            string strNFormDate = txtNFormDate.Text.Trim();
            if (strNFormDate != "")
            {
                strNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["NFormDate"].DefaultValue = strNFormDate;
            }

            TextBox txtNClosingDate = (TextBox)gvrow.FindControl("txtNClosingDate");
            string strNClosingDate = txtNClosingDate.Text.Trim();
            if (strNClosingDate != "")
            {
                strNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["NClosingDate"].DefaultValue = strNClosingDate;
            }

            TextBox txtSFormNo = (TextBox)gvrow.FindControl("txtSFormNo");
            string SFormNo = txtSFormNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["SFormNo"].DefaultValue = SFormNo;

            TextBox txtSFormDate = (TextBox)gvrow.FindControl("txtSFormDate");
            string strSFormDate = txtSFormDate.Text.Trim();
            if (strSFormDate != "")
            {
                strSFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["SFormDate"].DefaultValue = strSFormDate;
            }

            TextBox txtSClosingDate = (TextBox)gvrow.FindControl("txtSClosingDate");
            string strSClosingDate = txtSClosingDate.Text.Trim();
            if (strSClosingDate != "")
            {
                strSClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["SClosingDate"].DefaultValue = strSClosingDate;
            }

            TextBox txtOctroiAmount = (TextBox)gvrow.FindControl("txtOctroiAmount");
            string strOctroiAmount = txtOctroiAmount.Text.Trim();
            DataSourceDelivery.UpdateParameters["OctroiAmount"].DefaultValue = strOctroiAmount;

            TextBox txtOctroiReceiptNo = (TextBox)gvrow.FindControl("txtOctroiReceiptNo");
            string strOctroiReceiptNo = txtOctroiReceiptNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["OctroiReceiptNo"].DefaultValue = strOctroiReceiptNo;

            TextBox txtOctroiPaidDate = (TextBox)gvrow.FindControl("txtOctroiPaidDate");
            string strOctroiPaidDate = txtOctroiPaidDate.Text.Trim();
            if (strOctroiPaidDate != "")
            {
                strOctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["OctroiPaidDate"].DefaultValue = strOctroiPaidDate;
            }

        }

        if (e.CommandName == "Cancel")
        {
            lblError.Visible = false;
            lblError.Text = "";
            GridViewDelivery.EditIndex = -1;
        }

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            // DownloadPODDocument(DocPath);
        }

    }

    protected void GridViewDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");
            bool IsNForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsNForm");
            bool IsSForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsSForm");
            bool IsOctroi = (bool)DataBinder.Eval(e.Row.DataItem, "IsOctroi");
            bool IsRoadPermit = (bool)DataBinder.Eval(e.Row.DataItem, "IsRoadPermit");

            if (DeliveryTypeId == (Int32)DeliveryType.Loaded) // Delivery Type
            {
                GridViewDelivery.Columns[2].Visible = true; // Container NO
                GridViewDelivery.Columns[3].Visible = false; // No Of Packages
            }
            else
            {
                GridViewDelivery.Columns[2].Visible = false; // Container NO
                GridViewDelivery.Columns[3].Visible = true; // No Of Packages
            }

            if (IsNForm == true) // NForm Applicable
            {
                GridViewDelivery.Columns[15].Visible = true; // N Form No
                GridViewDelivery.Columns[16].Visible = true; // N Form Date
                GridViewDelivery.Columns[17].Visible = true; // N Closing Date
            }
            else
            {
                GridViewDelivery.Columns[15].Visible = false; // N Form No
                GridViewDelivery.Columns[16].Visible = false; // N Form Date
                GridViewDelivery.Columns[17].Visible = false; // N Closing Date
            }

            if (IsSForm == true) // SForm Applicable
            {
                GridViewDelivery.Columns[18].Visible = true; // S Form No
                GridViewDelivery.Columns[19].Visible = true; // S Form Date
                GridViewDelivery.Columns[20].Visible = true; // S Closing Date
            }
            else
            {
                GridViewDelivery.Columns[18].Visible = false; // S Form No
                GridViewDelivery.Columns[19].Visible = false; // S Form Date
                GridViewDelivery.Columns[20].Visible = false; // S Closing Date
            }
            if (IsOctroi == true) // Octroi Applicable
            {
                GridViewDelivery.Columns[21].Visible = true; // Octroi Amount
                GridViewDelivery.Columns[22].Visible = true; // Octroi Receipt No	
                GridViewDelivery.Columns[23].Visible = true; // Octroi Paid Date
            }
            else
            {
                GridViewDelivery.Columns[21].Visible = false; // Octroi Amount
                GridViewDelivery.Columns[22].Visible = false; // Octroi Receipt No	
                GridViewDelivery.Columns[23].Visible = false; // Octroi Paid Date
            }
            if (IsRoadPermit == true) // Road Permit Applicable
            {
                GridViewDelivery.Columns[24].Visible = true; // Road Permit No
                GridViewDelivery.Columns[25].Visible = true; // Road Permit Date
            }
            else
            {
                GridViewDelivery.Columns[24].Visible = false; // Road Permit No
                GridViewDelivery.Columns[25].Visible = false; // Road Permit Date
            }
        }
    }
    protected void DataSourceDelivery_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

        if (Result == 0)
        {
            lblError.Text = "Delivery Detail Updated Successfully";
            lblError.CssClass = "success";
            GridViewDelivery.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void gvTruckRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "packingdocs")
        {
            if (e.CommandArgument.ToString() != "")
            {
                int TransRequestId = Convert.ToInt32(e.CommandArgument.ToString());
                if (TransRequestId > 0)
                {
                    string FilePath = "";
                    String ServerPath = FileServer.GetFileServerDir();
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectoryByName("TransportFiles");
                        DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransRequestId);
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
            }
        }
        else if (e.CommandName.ToLower().Trim() == "select")
        {
            if (e.CommandArgument.ToString() != "")
            {
                string RefNo = e.CommandArgument.ToString();
                lblTransRefNo.Text = RefNo;
                if (Convert.ToInt32(Session["EnqId"].ToString()) > 0)
                {
                    DataView dsGetJobDetail = DBOperations.GetTransportDetailByJobId(Convert.ToInt32(Session["EnqId"].ToString()));
                    if (dsGetJobDetail != null && dsGetJobDetail.Count > 0)                           // For getting freight get Pincode drop and pickup detail         
                    {
                        txtpickPincode.Text = dsGetJobDetail[0]["PickupPincode"].ToString();
                        txtdropPincode.Text = dsGetJobDetail[0]["DropPincode"].ToString();
                    }
                    if (dsGetJobDetail != null)
                    {
                        ddDeliveryType.Visible = true;
                        ddlExportType.Visible = false;
                        if (dsGetJobDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
                        {
                            ddDeliveryType.SelectedValue = dsGetJobDetail.Table.Rows[0]["DeliveryType"].ToString();
                            ddDeliveryType_SelectedIndexChanged(null, EventArgs.Empty);
                        }
                        lblType_Title.Text = "Delivery Type";

                        string VehiclePlaced = dsGetJobDetail.Table.Rows[0]["IsVehiclePlaced"].ToString();
                        if (VehiclePlaced != "1")
                        {

                            dvtruckDetail.Visible = true;
                            tblTruckRequest.Visible = true;
                            txtDestination.Text = dsGetJobDetail.Table.Rows[0]["Destination"].ToString();
                            txtDimension.Text = dsGetJobDetail.Table.Rows[0]["Dimension"].ToString();
                            lblJobNumber.Text = dsGetJobDetail.Table.Rows[0]["JobRefNo"].ToString();
                            txtVehiclePlaceDate.Text = dsGetJobDetail.Table.Rows[0]["VehiclePlaceRequireDate"].ToString();
                            txtRemark1.Text = dsGetJobDetail.Table.Rows[0]["Remark"].ToString();
                            txtpickState.Text = dsGetJobDetail.Table.Rows[0]["PickupState"].ToString();   //Add new details stete city pincode get job details
                            txtpickCity.Text = dsGetJobDetail.Table.Rows[0]["PickupCity"].ToString();
                            txtdropState.Text = dsGetJobDetail.Table.Rows[0]["DropState"].ToString();
                            txtdropCity.Text = dsGetJobDetail.Table.Rows[0]["DropCity"].ToString();
                        }
                        else
                        {

                            dvtruckDetail.Visible = false;
                        }
                    }
                }

            }
        }
    }

    #region truck request changes
    protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
        }
        if (ddDeliveryType.SelectedValue == "1")     //Loaded , '1' represents the Loaded option
        {
            loadedDocuments.Visible = true;
            lblEmpty_Letter.Visible = true;
            //  UpdBtn.Visible = true;
        }
        else
        {
            loadedDocuments.Visible = false;
            lblEmpty_Letter.Visible = false;
            //   UpdBtn.Visible = false;
        }
    }

    protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            btnSaveDocument_Click(fuDocument, EventArgs.Empty);
        }

        DateTime dtVehiclePlaceRequireDate = DateTime.MinValue;
        int JobType = 0, TotalContainers = 0, VehicleRequired = 1, Mode = 0, DeliveryType = 0, ExportType = 0;
        int PickupPincode = 0, DropPincode = 0;
        string PickupState = "", PickupCity = "", DropState = "", DropCity = "", EmptyLetter = "", EmptyDocPath = "", FileName = "", DocName = "";

        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
        }

        if (ddDeliveryType.SelectedValue != "0")
        {
            DeliveryType = Convert.ToInt32(ddDeliveryType.SelectedValue);
        }

        if (ddlExportType.SelectedValue != "0")
        {
            ExportType = Convert.ToInt32(ddlExportType.SelectedValue);
        }

        if (Session["EnqId"].ToString() != "" && Session["EnqId"].ToString() != "0")
        {
            //if (hdnJobType.Value != "0" && hdnJobType.Value != "")
            //{
            //    JobType = Convert.ToInt32(hdnJobType.Value);
            //}


            if (txtVehiclePlaceDate.Text.Trim() != "")
                dtVehiclePlaceRequireDate = Commonfunctions.CDateTime(txtVehiclePlaceDate.Text.Trim());

            PickupPincode = Convert.ToInt32(txtpickPincode.Text);
            DropPincode = Convert.ToInt32(txtdropPincode.Text);
            PickupState = txtpickState.Text;
            PickupCity = txtpickCity.Text;
            DropState = txtdropState.Text;
            DropCity = txtdropCity.Text;
            dvtruckDetail.Visible = true;
            EmptyLetter = loadedDocuments.FileName;

            if (ddDeliveryType.SelectedValue == "1")
            {
                EmptyDocPath = Regex.Replace(lblJobNumber.Text, "[^a-zA-Z0-9]", "");
                EmptyDocPath = "FreightDoc\\" + EmptyDocPath + "\\" + EmptyLetter;
                if (loadedDocuments != null && loadedDocuments.HasFile)
                    FileName = UploadEmptyDocuments(EmptyDocPath);
            }
            //int result = DBOperations.UpdateTransportRequest(Convert.ToInt32(Session["EnqId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
            //    txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, LoggedInUser.glUserId);

            int result = DBOperations.UpTransFreightRequest(Convert.ToInt32(Session["EnqId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
               txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, FileName, EmptyDocPath, 122, PickupPincode, PickupState, PickupCity, DropPincode, DropState, DropCity, EmptyLetter, DocName, LoggedInUser.glUserId);

            if (result == 0)
            {
                tblTruckRequest.Visible = false;
                lblError.Text = "Truck detail Added successfully!";
                lblError.CssClass = "success";
                gvTruckRequest.DataBind();
                // Add packing list documents
                if (Convert.ToString(ViewState["PackingList"]) != "")
                {
                    DataTable dtPackingList = (DataTable)ViewState["PackingList"];
                    if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                    {
                        string DocPath = "";
                        for (int i = 0; i < dtPackingList.Rows.Count; i++)
                        {
                            if (dtPackingList.Rows[i]["DocPath"] != null)
                                DocPath = dtPackingList.Rows[i]["DocPath"].ToString();
                            int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["EnqId"].ToString()), DocPath, LoggedInUser.glUserId);
                        }
                    }
                }
            }

            //if (result > 0)
            //{


            //    //if (loggedInUser.glModuleId == 1)
            //    //{
            //    //    if (hdnJobId.Value != "" && hdnJobId.Value != "0")
            //    //    {
            //    //        int TransportBabaji = DBOperations.TR_updJobTransportBabaji(Convert.ToInt32(hdnJobId.Value), loggedInUser.glUserId);
            //    //    }
            //    //}
            //    Response.Redirect("SuccessPage.aspx?Request=" + result.ToString());

            //}
            //else if (result == -2)
            //{
            //    lblError.Text = "Truck request already exists! Please complete delivery for existing request first.";
            //    lblError.CssClass = "errorMsg";
            //}
            //else if (result == -3)
            //{
            //    lblError.Text = "No of containers to be dispatched exceed available containers.";
            //    lblError.CssClass = "errorMsg";
            //}
            //else
            //{
            //    lblError.Text = "Error while adding truck request. Please try again later.";
            //    lblError.CssClass = "errorMsg";
            //}
        }
    }

    protected void rptDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["PackingList"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["PackingList"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.Text = "Successfully Deleted Document.";
                lblError.CssClass = "success";
                rptDocument.DataBind();
            }
            else
            {
                lblError.Text = "Error while deleting document. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadPackingList(FilePath);
        }
    }

    protected void BindGrid()
    {
        //if (ViewState["PackingList"].ToString() != "")
        //{
        DataTable dtPackingList = (DataTable)ViewState["PackingList"];
        rptDocument.DataSource = dtPackingList;
        rptDocument.DataBind();
        //}
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (FileUpload1 != null && FileUpload1.HasFile)
            fileName = UploadFiles(FileUpload1);
        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["PackingList"];
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

            dtAnnexure.Rows.Add(PkId, fileName, FileUpload1.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["PackingList"] = dtAnnexure;

            // Add packing list documents
            if (Convert.ToString(ViewState["PackingList"]) != "")
            {
                DataTable dtPackingList = (DataTable)ViewState["PackingList"];
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    string DocPath = "";
                    for (int i = 0; i < dtPackingList.Rows.Count; i++)
                    {
                        if (dtPackingList.Rows[i]["DocPath"] != null)
                            DocPath = dtPackingList.Rows[i]["DocPath"].ToString();
                        int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["JobId"].ToString()), DocPath, LoggedInUser.glUserId);
                    }
                }
            }
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

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("\\UploadFiles\\Transport\\" + FilePath);
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

    protected void DownloadPackingList(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("\\UploadFiles\\Transport\\" + DocumentPath);
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
    #endregion

    protected void txtpickPincode_TextChanged(object sender, EventArgs e)   //Get State and city by using Pincode API 
    {
        string pincode = txtpickPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnPincodeId.Value = "0";
            txtpickState.Text = "Invalid Pincode";
            txtpickCity.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;

                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {
                            txtpickState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtpickCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnPincodeId.Value = pincode;
                        }
                        else
                        {
                            txtpickState.Text = "State Not Found";
                            txtpickCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtpickState.Text = "State Not Found";
                        txtpickCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtpickState.Text = "Error";
                    txtpickCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
    }

    protected void txtdropPincode_TextChanged(object sender, EventArgs e)    // Get State and City By using Pincode API
    {
        string pincode = txtdropPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnpinid.Value = "0";
            txtdropState.Text = "Invalid Pincode";
            txtdropCity.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {

                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;


                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {

                            txtdropState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtdropCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnpinid.Value = pincode;
                        }
                        else
                        {
                            txtdropState.Text = "State Not Found";
                            txtdropCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtdropState.Text = "State Not Found";
                        txtdropCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtdropState.Text = "Error";
                    txtdropCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {

            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
    }
    #region Empty letter 
    //private string UploadEmptyDocuments(string FilePath)
    //{

    //    string FileName = loadedDocuments.FileName;
    //    FileName = FileServer.ValidateFileName(FileName);

    //    string ServerFilePath = FileServer.GetFileServerDir();

    //    if (ServerFilePath == "")
    //    {
    //        // Application Directory Path
    //        // ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath );
    //        ServerFilePath = Server.MapPath(""..\\UploadFiles", filePath);

    //    }
    //    else
    //    {
    //        // File Server Path
    //        ServerFilePath = ServerFilePath + FilePath;
    //    }

    //    if (!System.IO.Directory.Exists(ServerFilePath))
    //    {
    //        System.IO.Directory.CreateDirectory(ServerFilePath);
    //    }
    //    if (loadedDocuments.FileName != string.Empty)
    //    {
    //        if (System.IO.File.Exists(ServerFilePath + FileName))
    //        {
    //            string ext = Path.GetExtension(FileName);
    //            FileName = Path.GetFileNameWithoutExtension(FileName);
    //            string FileId = RandomString(5);

    //            FileName += "_" + FileId + ext;
    //        }

    //        loadedDocuments.SaveAs(ServerFilePath + FileName);

    //        return FileName;
    //    }

    //    else
    //    {
    //        return "";
    //    }

    //}

    private string UploadEmptyDocuments(string filePath)    // for Upload Empty Letter Documents
    {
        // Validate the file name
        string fileName = loadedDocuments.FileName;
        //   fileName = FileServer.ValidateFileName(fileName);

        // Get the base directory for saving files
        string ServerFilePath = FileServer.GetFileServerDir();

        if (string.IsNullOrEmpty(ServerFilePath))
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath(Path.Combine("..\\UploadFiles", filePath));
        }
        else
        {
            // File Server Path
            ServerFilePath = Path.Combine(ServerFilePath, filePath);
        }

        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(ServerFilePath);
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Full path to save the file
        string fullPath = Path.Combine(directoryPath, fileName);

        // Check if the file already exists and modify the file name if necessary
        if (System.IO.File.Exists(fullPath))
        {
            string ext = Path.GetExtension(fileName);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string fileId = RandomString(5);

            fileName = $"{nameWithoutExt}_{fileId}{ext}";
            fullPath = Path.Combine(directoryPath, fileName);
        }

        // Save the file to the designated path
        loadedDocuments.SaveAs(fullPath);

        return fileName;
    }
    #endregion
}