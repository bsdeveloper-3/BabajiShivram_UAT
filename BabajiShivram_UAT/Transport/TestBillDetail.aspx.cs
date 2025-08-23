using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Ionic.Zip;

public partial class Transport_TestBillDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(imgbtnPackingList);
        ScriptManager1.RegisterPostBackControl(btnAddTransporter);
        ViewState["FreightAmt"] = 0;
        ViewState["VaraiAmt"] = 0;
        ViewState["DetentionAmt"] = 0;
        ViewState["EmptyContReturnAmt"] = 0;
        ViewState["TollCharges"] = 0;
        ViewState["OtherCharges"] = 0;
        ViewState["AdvanceAmt"] = 0;
        ViewState["TotalAmt"] = 0;
        ViewState["SavingAmt"] = 0;
        ViewState["MarketRate"] = 0;

        if (Session["TransReqId"] == null)
        {
            Response.Redirect("TransBill.aspx");
        }

        if (!IsPostBack)
        {
            lblFreightValidator.Visible = false;
            lblVaraiValidator.Visible = false;
            lblEmptyContValidator.Visible = false;
            lblDetentionValidator.Visible = false;
            lblTollChargesValidator.Visible = false;
            lblOtherChargesValidator.Visible = false;
            int TranRequestId = Convert.ToInt32(Session["TransReqId"]);
            // Fill Vehicle Details
            TruckRequestDetail(TranRequestId);
        }
    }

    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Detail - " + lblTRRefNo.Text;

            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
            lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();

            lblPickAdd.Text = dvDetail.Table.Rows[0]["PickUpAddress"].ToString();
            lblDropAdd.Text = dvDetail.Table.Rows[0]["DropAddress"].ToString();
            lblpickPincode.Text = dvDetail.Table.Rows[0]["PickupPincode"].ToString();           //added new pickup and drop pincode city and state for transport request detail Updated delivery
            lblpickState.Text = dvDetail.Table.Rows[0]["PickupState"].ToString();
            lblpickCity.Text = dvDetail.Table.Rows[0]["PickupCity"].ToString();
            lblDropPincode.Text = dvDetail.Table.Rows[0]["DropPincode"].ToString();
            lblDropState.Text = dvDetail.Table.Rows[0]["DropState"].ToString();
            lblDropCity.Text = dvDetail.Table.Rows[0]["DropCity"].ToString();
            GridViewVehicle.DataBind();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["TransReqId"] = null;
        //Response.Redirect("TestBilling.aspx");
        Response.Redirect("TransBill.aspx");
    }

    protected void btnBillSubmit_Click(object sender, EventArgs e)
    {
        bool IsValid = true;
        int TotalAmt = 0, ApprovedTotalAmt = 0, TransReqId = 0, TransporterID = 0, TransitDays = 0;
        string BillNumber = "", BillAmount = "", DetentionAmount = "", VaraiAmount = "",
                EmptyContRcptCharges = "", TotalAmount = "", BillPersonName = "", TollCharges = "", OtherCharges = "",
                fileName = "",filePath="";
        DateTime BillSubmitDate = DateTime.MinValue, BillDate = DateTime.MinValue;

        if (fuDocument != null && fuDocument.HasFile)
        {
            fileName = UploadFiles(fuDocument, "");
            filePath = "\\Transport\\" + fileName;

        }

        if (hdnTotalAmount.Value != "0" && hdnTotalAmount.Value != "")
        {
            ApprovedTotalAmt = Convert.ToInt32(hdnTotalAmount.Value);
        }

        TransReqId = Convert.ToInt32(Session["TransReqId"]);
        TransporterID = Convert.ToInt16(ddTransporter.SelectedValue);
        BillNumber = txtBillNo.Text.Trim();

        BillAmount = txtBillAmount.Text.Trim();
        VaraiAmount = txtVaraiExp.Text.Trim();
        DetentionAmount = txtDetentionAmount.Text.Trim();
        EmptyContRcptCharges = txtEmptyContCharges.Text.Trim();
        TollCharges = txtTollCharges.Text.Trim();
        OtherCharges = txtOtherCharges.Text.Trim();
        TotalAmount = txtTotalAmount.Text.Trim();
        BillPersonName = txtBillingEmpoyee.Text.Trim();
        GetTotalAmount();

        if (hdnPageValid.Value == "1")
        {
            IsValid = false;
        }

        if (hdnTotalAmount.Value != "" && hdnTotalAmount.Value != "0")
        {
            decimal dcActualTotalAmount = Convert.ToDecimal(hdnTotalAmount.Value);
            decimal dcTotalAmount = Convert.ToDecimal(TotalAmount);
            if (dcTotalAmount > 0 && dcActualTotalAmount > 0)
            {
                if (dcTotalAmount > dcActualTotalAmount)
                {
                    IsValid = false;
                }
            }
        }

        if (txtBillSubmitDate.Text.Trim() != "")
            BillSubmitDate = Commonfunctions.CDateTime(txtBillSubmitDate.Text.Trim());
        if (txtBillDate.Text.Trim() != "")
            BillDate = Commonfunctions.CDateTime(txtBillDate.Text.Trim());

        if (IsValid == false)
        {
            if (txtJustification.Text.Trim() == "")
            {
                lblError.Text = "Please enter justification for amount entered is more than actual amount!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        int result_BillID = DBOperations.AddTransBillDetail(TransReqId, TransporterID, TransitDays, BillSubmitDate, BillNumber, BillDate, BillAmount,
            DetentionAmount, VaraiAmount, EmptyContRcptCharges, TollCharges, OtherCharges, TotalAmount, BillPersonName, IsValid, txtJustification.Text.Trim(), false, 0,fileName,filePath, LoggedInUser.glUserId);

        if (result_BillID > 0)
        {
            if (IsValid == false)
            {
                int result_ApprovalSend = DBOperations.AddTransApproveRejectBill(result_BillID, 0, 0, LoggedInUser.glUserId);
                if (result_ApprovalSend == 0)
                {
                    SendApprovalMail(TransReqId, TransporterID);
                }
            }
            else
            {
                int result = DBOperations.AddTransApproveRejectBill(result_BillID, 1, Convert.ToDecimal(TotalAmount), 3);
                if (result == 0)
                {
                    // add approval history in table
                    int AddHistory = DBOperations.AddTransBillApprovalHistory(result_BillID, 1, "", 3);
                    if (AddHistory == 0)
                    {
                        int result_BillNonReceive = DBOperations.AddBillReceivedDetail(result_BillID, LoggedInUser.glUserId, 0, DateTime.Now, DateTime.MinValue, 0, "", DateTime.MinValue, "", DateTime.MinValue, LoggedInUser.glUserId);
                        if (result_BillNonReceive == 0)
                        {
                            Response.Redirect("SuccessPage.aspx?Bill=122");
                        }
                        else
                        {
                            lblError.Text = "Error while adding bill detail. Please try again later.";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                }
            }
        }
        else if (result_BillID == -1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result_BillID == -2)
        {
            lblError.Text = "Billing Detail Already Updated For Transporter - " + ddTransporter.SelectedItem.Text;
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancelBill_Click(object sender, EventArgs e)
    {
        ddTransporter.DataBind();
        txtBillSubmitDate.Text = "";
        txtBillNo.Text = "";
        txtBillDate.Text = "";
        txtBillAmount.Text = "";
        txtDetentionAmount.Text = "";
        txtVaraiExp.Text = "";
        txtEmptyContCharges.Text = "";
        txtTotalAmount.Text = "";
        txtBillingEmpoyee.Text = "";
    }

    protected void txtBillAmount_TextChanged(object sender, EventArgs e)
    {
        lblFreightValidator.Visible = false;
        if (txtBillAmount.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtBillAmount.Focus();
    }

    protected void txtDetentionAmount_TextChanged(object sender, EventArgs e)
    {
        lblDetentionValidator.Visible = false;
        if (txtDetentionAmount.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtDetentionAmount.Focus();
    }

    protected void txtVaraiExp_TextChanged(object sender, EventArgs e)
    {
        lblVaraiValidator.Visible = true;
        if (txtVaraiExp.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtVaraiExp.Focus();
    }

    protected void txtEmptyContCharges_TextChanged(object sender, EventArgs e)
    {
        lblEmptyContValidator.Visible = false;
        if (txtEmptyContCharges.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtEmptyContCharges.Focus();
    }

    protected void txtTollCharges_TextChanged(object sender, EventArgs e)
    {
        lblTollChargesValidator.Visible = false;
        if (txtTollCharges.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtTollCharges.Focus();
    }

    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        lblOtherChargesValidator.Visible = false;
        if (txtOtherCharges.Text.Trim() != "")
        {
            GetTotalAmount();
        }
        txtOtherCharges.Focus();
    }

    protected void GetTotalAmount()
    {
        decimal dcFreightAmt = 0, dcDetentionAmt = 0, dcVaraiExpense = 0, dcEmptyContCharges = 0, dcAdvanceAmt = 0, dcTollCharges = 0, dcOtherCharges = 0;
        decimal dcActualFreightAmt = 0, dcActualDetentionAmt = 0, dcActualVaraiAmt = 0, dcActualEmptyContAmt = 0, dcActualTollCharges = 0, dcActualOtherCharges = 0;

        // set page is valid or not
        if (hdnFreightAmt.Value != "" && hdnFreightAmt.Value != "0")
            dcActualFreightAmt = Convert.ToDecimal(hdnFreightAmt.Value);
        if (hdnDetentionAmt.Value != "" && hdnDetentionAmt.Value != "0")
            dcActualDetentionAmt = Convert.ToDecimal(hdnDetentionAmt.Value);
        if (hdnVaraiAmt.Value != "" && hdnVaraiAmt.Value != "0")
            dcActualVaraiAmt = Convert.ToDecimal(hdnVaraiAmt.Value);
        if (hdnEmptyContReturnAmt.Value != "" && hdnEmptyContReturnAmt.Value != "0")
            dcActualEmptyContAmt = Convert.ToDecimal(hdnEmptyContReturnAmt.Value);
        if (hdnTollCharges.Value != "" && hdnTollCharges.Value != "0")
            dcActualTollCharges = Convert.ToDecimal(hdnTollCharges.Value);
        if (hdnOtherCharges.Value != "" && hdnOtherCharges.Value != "0")
            dcActualOtherCharges = Convert.ToDecimal(hdnOtherCharges.Value);

        if (hdnAdvanceAmt.Value != "" && hdnAdvanceAmt.Value != "0")
            dcAdvanceAmt = Convert.ToDecimal(hdnAdvanceAmt.Value);
        if (txtBillAmount.Text.Trim() != "")
            dcFreightAmt = Convert.ToDecimal(txtBillAmount.Text.Trim());
        if (txtDetentionAmount.Text.Trim() != "")
            dcDetentionAmt = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
        if (txtVaraiExp.Text.Trim() != "")
            dcVaraiExpense = Convert.ToDecimal(txtVaraiExp.Text.Trim());
        if (txtEmptyContCharges.Text.Trim() != "")
            dcEmptyContCharges = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
        if (txtTollCharges.Text.Trim() != "")
            dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
        if (txtOtherCharges.Text.Trim() != "")
            dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());

        // Freight Amount
        if (dcFreightAmt > 0 && dcActualFreightAmt > 0)
        {
            if (dcFreightAmt > dcActualFreightAmt)
            {
                lblFreightValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        // Detention Amount
        if (dcDetentionAmt > 0 && dcActualDetentionAmt > 0)
        {
            if (dcDetentionAmt > dcActualDetentionAmt)
            {
                lblDetentionValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        // Varai Amount
        if (dcVaraiExpense > 0 && dcActualVaraiAmt > 0)
        {
            if (dcVaraiExpense > dcActualVaraiAmt)
            {
                lblVaraiValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        // Empty Cont Amount
        if (dcEmptyContCharges > 0 && dcActualEmptyContAmt > 0)
        {
            if (dcEmptyContCharges > dcActualEmptyContAmt)
            {
                lblEmptyContValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        // Toll Amount
        if (dcTollCharges > 0 && dcActualTollCharges > 0)
        {
            if (dcTollCharges > dcActualTollCharges)
            {
                lblTollChargesValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        //Other Amount
        if (dcOtherCharges > 0 && dcActualOtherCharges > 0)
        {
            if (dcOtherCharges > dcActualOtherCharges)
            {
                lblOtherChargesValidator.Visible = true;
                hdnPageValid.Value = "1";
                rfvJustification.Enabled = true;
            }
            else
            {
                hdnPageValid.Value = "0";
                rfvJustification.Enabled = false;
            }
        }

        txtTotalAmount.Text = Convert.ToDecimal((dcFreightAmt + dcDetentionAmt + dcVaraiExpense + dcEmptyContCharges + dcTollCharges + dcOtherCharges) - (dcAdvanceAmt)).ToString();
        txtTotalAmount.Enabled = false;
    }

    protected void SendApprovalMail(int TransReqId, int TransporterId)
    {
        int TransBillId = 0;
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilderBilling = new StringBuilder();

        if (TransReqId > 0)
        {
            string strCustomerEmail = "", strCCEmail = "", strSubject = "", MessageBody = "", EmailContent = "",
                    strTRRefNo = "", strTruckRequestDate = "", strJobNo = "", strCustName = "", strVehiclePlaceDate = "", strVehicleRequired = "",
                    strLocation = "", strDest = "", strDimension = "", strGrossWT = "", strCont20 = "", strCont40 = "", strDispatch_Value = "", strDelExportType_Value = "", strDelExportType_Title = "";

            #region Transport Request details
            DataView dvDetail = DBOperations.GetTransportRequestDetail(TransReqId);
            if (dvDetail.Table.Rows.Count > 0)
            {
                strTRRefNo = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
                strTruckRequestDate = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
                strJobNo = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
                strCustName = dvDetail.Table.Rows[0]["CustName"].ToString();
                if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                    strVehiclePlaceDate = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
                strDispatch_Value = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
                strLocation = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
                strDest = dvDetail.Table.Rows[0]["Destination"].ToString();
                strDimension = dvDetail.Table.Rows[0]["Dimension"].ToString();
                strGrossWT = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
                strCont20 = dvDetail.Table.Rows[0]["Count20"].ToString();
                strCont40 = dvDetail.Table.Rows[0]["Count40"].ToString();
                strDelExportType_Value = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
                strDelExportType_Title = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            }

            strSubject = "Transport Bill Approval for TR Ref No " + strTRRefNo + " and Babaji Job No " + strJobNo + "";
            try
            {
                string strFileName = "../EmailTemplate/TransportBillApproval.txt";
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
            }

            MessageBody = EmailContent.Replace("@TRRefNo", strTRRefNo);
            MessageBody = MessageBody.Replace("@BabajiJobNo", strJobNo);
            MessageBody = MessageBody.Replace("@TruckRequestDate", strTruckRequestDate);
            MessageBody = MessageBody.Replace("@CustName", strCustName);
            MessageBody = MessageBody.Replace("@VehiclePlaceDate", strVehiclePlaceDate);
            MessageBody = MessageBody.Replace("@Dispatch_Value", strDispatch_Value);
            MessageBody = MessageBody.Replace("@LocationFrom", strLocation);
            MessageBody = MessageBody.Replace("@Destination", strDest);
            MessageBody = MessageBody.Replace("@Dimension", strDimension);
            MessageBody = MessageBody.Replace("@GrossWeight", strGrossWT);
            MessageBody = MessageBody.Replace("@Con20", strCont20);
            MessageBody = MessageBody.Replace("@Con40", strCont40);
            MessageBody = MessageBody.Replace("@DelExportType_Title", strDelExportType_Title);
            MessageBody = MessageBody.Replace("@DelExportType_Value", strDelExportType_Value);

            List<string> lstFileDoc = new List<string>();
            #endregion

            MessageBody = MessageBody + "<BR>";

            #region Get Vehicle Bill details
            DataSet dsGetBillDetails = DBOperations.GetBillVehicleDetailByTP(TransReqId, TransporterId);
            if (dsGetBillDetails != null && dsGetBillDetails.Tables.Count > 0)
            {
                strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Transporter</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Type</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Delivery From</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Delivery Point</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Unloading Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Rate</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Advance (%)</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Advance Amt</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Detention Amt</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Varai Exp</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Empty Cont Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Toll Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Other Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Total</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Billing Instruction</th></tr>");

                for (int i = 0; i < dsGetBillDetails.Tables[0].Rows.Count; i++)
                {
                    if (dsGetBillDetails.Tables[0].Rows[i]["lid"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Transporter"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VehicleNo"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VehicleType"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["DeliveryFrom"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["DeliveryTo"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["LRNo"].ToString() + "</th>");
                        if (dsGetBillDetails.Tables[0].Rows[i]["LRDate"] == DBNull.Value)
                        {
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'></th>");
                        }
                        else
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetBillDetails.Tables[0].Rows[i]["LRDate"]).ToString("dd/MM/yyyy") + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["ChallanNo"].ToString() + "</th>");
                        if (dsGetBillDetails.Tables[0].Rows[i]["ChallanDate"] == DBNull.Value)
                        {
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'></th>");
                        }
                        else
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetBillDetails.Tables[0].Rows[i]["ChallanDate"]).ToString("dd/MM/yyyy") + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetBillDetails.Tables[0].Rows[i]["UnloadingDate"]).ToString("dd/MM/yyyy") + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Rate"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Advance"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["AdvanceAmount"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["DetentionAmount"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VaraiExpense"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["EmptyContRecptCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["TollCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["OtherCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["TotalAmount"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Instruction"].ToString() + "</th></tr>");
                    }
                }
            }
            MessageBody = MessageBody + "<BR>" + strbuilder + "</table><BR>";
            #endregion

            #region Bill details
            DataSet dsGetTransBillDetails = DBOperations.GetTransBillDetail(TransReqId, TransporterId);
            if (dsGetTransBillDetails != null && dsGetTransBillDetails.Tables.Count > 0)
            {
                TransBillId = Convert.ToInt32(dsGetTransBillDetails.Tables[0].Rows[0]["lid"].ToString());
                strbuilderBilling = strbuilderBilling.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                //headers
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Transporter</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Number</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Submit Date</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Date</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Amount</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Detention Amount</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Varai Amount</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Empty Cont Rcpt Charges</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Toll Charges</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Other Charges</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Total Amount</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Justification</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Person</th></tr>");
                //rows
                strbuilderBilling = strbuilderBilling.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["Transporter"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillNumber"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetTransBillDetails.Tables[0].Rows[0]["BillSubmitDate"]).ToString("dd/MM/yyyy") + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetTransBillDetails.Tables[0].Rows[0]["BillDate"]).ToString("dd/MM/yyyy") + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillAmount"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["DetentionAmount"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["VaraiAmount"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["EmptyContRcptCharges"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["TollCharges"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["OtherCharges"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["TotalAmount"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["Justification"].ToString() + "</th>");
                strbuilderBilling = strbuilderBilling.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillPersonName"].ToString() + "</th></tr>");
                strbuilderBilling = strbuilderBilling.Append("</table>");
            }
            #endregion

            MessageBody = MessageBody + "<BR>" + strbuilderBilling + "<BR><BR>Kindly <strong><a href='http://192.168.6.107/TRBillApprove.aspx?o=1&t=" + TransporterId + "&p=" + TransBillId + "'> Approve</a></strong>" + "  or  " +
                "<strong><a href='http://192.168.6.107/TRBillApprove.aspx?o=2&t=" + TransporterId + "&p=" + TransBillId + "'>Reject</a></strong> the bill here. <BR><BR>";
            MessageBody = MessageBody + "<BR><BR>Thank You<BR><b>This is system generated mail, please do not reply to this message via e-mail.</b>";

            //bEmailSuccess = EMail.SendMailMultiAttach("kivisha.jain@babajishivram.com", "kivisha.jain@babajishivram.com", "", strSubject, MessageBody, lstFileDoc);
            //bEmailSuccess = EMail.SendMailBCC("dhaval@babajishivram.com", "dhaval@babajishivram.com", "", "javed.shaikh@babajishivram.com", strSubject, MessageBody, "");
            bEmailSuccess = EMail.SendMailBCC("ashwani.singh@babajishivram.com", "ashwani.singh@babajishivram.com", "", "javed.shaikh@babajishivram.com", strSubject, MessageBody, "");
            if (bEmailSuccess == true)
            {
                lblError.Text = "Amount is more than approved amount. Successfully sent mail for bill approval.";
                lblError.CssClass = "success";
            }
        }

    }

    #region GridView Event

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0, dcTollCharges = 0, dcOtherCharges = 0,
                dcAdvanceAmount = 0, dcTotalAmount = 0, dcMarketRate = 0, dcSavingAmt = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Rate") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Rate"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "VaraiExpense") != DBNull.Value)
            {
                dcVaraiExpenses = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VaraiExpense"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "DetentionAmount") != DBNull.Value)
            {
                dcDetentionAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DetentionAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges") != DBNull.Value)
            {
                dcEmptyContRecdCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            {
                dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            {
                dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "AdvanceAmount") != DBNull.Value)
            {
                dcAdvanceAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AdvanceAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "SavingAmt") != DBNull.Value)
            {
                dcSavingAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SavingAmt"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "MarketBillingRate") != DBNull.Value)
            {
                dcMarketRate = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "MarketBillingRate"));
            }

            ViewState["FreightAmt"] = Convert.ToDecimal(ViewState["FreightAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcOtherCharges;
            ViewState["AdvanceAmt"] = Convert.ToDecimal(ViewState["AdvanceAmt"]) + dcAdvanceAmount;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;
            ViewState["SavingAmt"] = Convert.ToDecimal(ViewState["SavingAmt"]) + dcSavingAmt;
            ViewState["MarketRate"] = Convert.ToDecimal(ViewState["MarketRate"]) + dcMarketRate;

            hdnFreightAmt.Value = Convert.ToString(ViewState["FreightAmt"]);
            hdnDetentionAmt.Value = Convert.ToString(ViewState["DetentionAmt"]);
            hdnVaraiAmt.Value = Convert.ToString(ViewState["VaraiAmt"]);
            hdnEmptyContReturnAmt.Value = Convert.ToString(ViewState["EmptyContReturnAmt"]);
            hdnTollCharges.Value = Convert.ToString(ViewState["TollCharges"]);
            hdnOtherCharges.Value = Convert.ToString(ViewState["OtherCharges"]);
            hdnAdvanceAmt.Value = Convert.ToString(ViewState["AdvanceAmt"]);
            hdnTotalAmount.Value = Convert.ToString(ViewState["TotalAmt"]);
            hdnSavingAmt.Value = Convert.ToString(ViewState["SavingAmt"]);
            hdnMarketRate.Value = Convert.ToString(ViewState["MarketRate"]);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[5].Text = "<b>" + Convert.ToDecimal(ViewState["MarketRate"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[6].Text = "<b>" + Convert.ToDecimal(ViewState["FreightAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[7].Text = "<b>" + Convert.ToDecimal(ViewState["SavingAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[9].Text = "<b>" + Convert.ToDecimal(ViewState["AdvanceAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[10].Text = "<b>" + Convert.ToDecimal(ViewState["DetentionAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[11].Text = "<b>" + Convert.ToDecimal(ViewState["VaraiAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[12].Text = "<b>" + Convert.ToDecimal(ViewState["EmptyContReturnAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[13].Text = "<b>" + Convert.ToDecimal(ViewState["TollCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[14].Text = "<b>" + Convert.ToDecimal(ViewState["OtherCharges"]).ToString("#,##0.00") + "</b>";
        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int TranRequestId = Convert.ToInt32(Session["TransReqId"]);

        Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        string strVehicleNo = lblVehicleNo.Text.Trim();
        int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Rate Detail Added Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;
            e.Cancel = true;

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else if (result == 2)
        {
            lblError.Text = "Details Already Updated!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
    }

    #endregion

    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument(TransRequestId);
        }
    }

    private void DownloadDocument(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
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

    protected void btnFileUpload_Click(Object sender, EventArgs e)
    {
        string fileName = "";
        //int result = 0;
        if (fuDocument != null && fuDocument.HasFile)
        {
            fileName = UploadFiles(fuDocument, "");
        }
        else
        {
            lblError.Text = "Please add contract copy..!!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Transport\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Transport\\" + FilePath;
        }


        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

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

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #endregion

    //protected void txtBillAmount_TextChanged(object sender, EventArgs e)
    //{
    //    lblFreightValidator.Visible = false;
    //    if (txtBillAmount.Text.Trim() != "")
    //    {
    //        decimal dcFreightAmt = 0, dcActualFreightAmt = 0;
    //        dcFreightAmt = Convert.ToDecimal(txtBillAmount.Text.Trim());
    //        dcActualFreightAmt = Convert.ToDecimal(hdnFreightAmt.Value);

    //        if (dcFreightAmt > 0 && dcActualFreightAmt > 0)
    //        {
    //            if (dcFreightAmt > dcActualFreightAmt)
    //            {
    //                lblFreightValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtBillAmount.Focus();
    //}

    //protected void txtDetentionAmount_TextChanged(object sender, EventArgs e)
    //{
    //    lblDetentionValidator.Visible = false;
    //    if (txtDetentionAmount.Text.Trim() != "")
    //    {
    //        decimal dcDetentionAmt = 0, dcActualDetentionAmt = 0;
    //        dcDetentionAmt = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
    //        dcActualDetentionAmt = Convert.ToDecimal(hdnDetentionAmt.Value);

    //        if (dcDetentionAmt > 0 && dcActualDetentionAmt > 0)
    //        {
    //            if (dcDetentionAmt > dcActualDetentionAmt)
    //            {
    //                lblDetentionValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtDetentionAmount.Focus();
    //}

    //protected void txtVaraiExp_TextChanged(object sender, EventArgs e)
    //{
    //    lblVaraiValidator.Visible = true;
    //    if (txtVaraiExp.Text.Trim() != "")
    //    {
    //        decimal dcVarai = 0, dcActualVaraiAmt = 0;
    //        dcVarai = Convert.ToDecimal(txtVaraiExp.Text.Trim());
    //        dcActualVaraiAmt = Convert.ToDecimal(hdnVaraiAmt.Value);

    //        if (dcVarai > 0 && dcActualVaraiAmt > 0)
    //        {
    //            if (dcVarai > dcActualVaraiAmt)
    //            {
    //                lblVaraiValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtVaraiExp.Focus();
    //}

    //protected void txtEmptyContCharges_TextChanged(object sender, EventArgs e)
    //{
    //    lblEmptyContValidator.Visible = false;
    //    if (txtEmptyContCharges.Text.Trim() != "")
    //    {
    //        decimal dcEmptyContAmt = 0, dcActualEmptyContAmt = 0;
    //        dcEmptyContAmt = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
    //        dcActualEmptyContAmt = Convert.ToDecimal(hdnEmptyContReturnAmt.Value);

    //        if (dcEmptyContAmt > 0 && dcActualEmptyContAmt > 0)
    //        {
    //            if (dcEmptyContAmt > dcActualEmptyContAmt)
    //            {
    //                lblEmptyContValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtEmptyContCharges.Focus();
    //}

    //protected void txtTollCharges_TextChanged(object sender, EventArgs e)
    //{
    //    lblTollChargesValidator.Visible = false;
    //    if (txtTollCharges.Text.Trim() != "")
    //    {
    //        decimal dcTollCharges = 0, dcActualTollCharges = 0;
    //        dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
    //        dcActualTollCharges = Convert.ToDecimal(hdnTollCharges.Value);

    //        if (dcTollCharges > 0 && dcActualTollCharges > 0)
    //        {
    //            if (dcTollCharges > dcActualTollCharges)
    //            {
    //                lblTollChargesValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtTollCharges.Focus();
    //}

    //protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    //{
    //    lblOtherChargesValidator.Visible = false;
    //    if (txtOtherCharges.Text.Trim() != "")
    //    {
    //        decimal dcOtherCharges = 0, dcActualOtherCharges = 0;
    //        dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());
    //        dcActualOtherCharges = Convert.ToDecimal(hdnOtherCharges.Value);

    //        if (dcOtherCharges > 0 && dcActualOtherCharges > 0)
    //        {
    //            if (dcOtherCharges > dcActualOtherCharges)
    //            {
    //                lblOtherChargesValidator.Visible = true;
    //                hdnPageValid.Value = "1";
    //                rfvJustification.Enabled = true;
    //            }
    //            else
    //            {
    //                rfvJustification.Enabled = false;
    //            }
    //        }
    //    }
    //    GetTotalAmount();
    //    txtOtherCharges.Focus();
    //}

    //protected void GetTotalAmount()
    //{
    //    decimal dcFreightAmt = 0, dcDetentionAmt = 0, dcVaraiExpense = 0, dcEmptyContCharges = 0, dcAdvanceAmt = 0, dcTollCharges = 0, dcOtherCharges = 0;

    //    if (hdnAdvanceAmt.Value != "" && hdnAdvanceAmt.Value != "0")
    //        dcAdvanceAmt = Convert.ToDecimal(hdnAdvanceAmt.Value);
    //    if (txtBillAmount.Text.Trim() != "")
    //        dcFreightAmt = Convert.ToDecimal(txtBillAmount.Text.Trim());
    //    if (txtDetentionAmount.Text.Trim() != "")
    //        dcDetentionAmt = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
    //    if (txtVaraiExp.Text.Trim() != "")
    //        dcVaraiExpense = Convert.ToDecimal(txtVaraiExp.Text.Trim());
    //    if (txtEmptyContCharges.Text.Trim() != "")
    //        dcEmptyContCharges = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
    //    if (txtTollCharges.Text.Trim() != "")
    //        dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
    //    if (txtOtherCharges.Text.Trim() != "")
    //        dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());

    //    txtTotalAmount.Text = Convert.ToDecimal((dcFreightAmt + dcDetentionAmt + dcVaraiExpense + dcEmptyContCharges + dcTollCharges + dcOtherCharges) - (dcAdvanceAmt)).ToString();
    //    txtTotalAmount.Enabled = false;
    //}

    protected void btnAddTransporter_Click(object sender, EventArgs e)
    {
        if (Session["TransReqId"] != null)
        {
            int result = DBOperations.TR_UpdateTransporterPlaced(Convert.ToInt32(Session["TransReqId"]), LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added transporter!";
                lblError.CssClass = "success";
                ddTransporter.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "Error while addng up transporter!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Transporter already exists!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected bool DecideHere(string Str)
    {
        if (Str == "0"|| Str=="")
            return false;
        else
            return true;
    }

    private void DownloadMultipleDocument(string DocumentPath, string Documentname)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (Documentname == "detentioncopy")
        {
            DocumentPath = "TransportDetentionDoc\\" + DocumentPath;
        }
        else if (Documentname == "varaicopy")
        {
            DocumentPath = "TransportVaraiDoc\\" + DocumentPath;
        }
        else if (Documentname == "emptycontcopy")
        {
            DocumentPath = "TransportEmptyContDoc\\" + DocumentPath;
        }
        else if (Documentname == "tollcopy")
        {
            DocumentPath = "TransportTollDoc\\" + DocumentPath;
        }
        else if (Documentname == "othercopy")
        {
            DocumentPath = "TransportOtherDoc\\" + DocumentPath;
        }
        else if (Documentname == "emailapprovalcopy")
        {
            DocumentPath = "EmailApprovalUpload\\" + DocumentPath;
        }
        else if (Documentname == "contractcopy")
        {
            DocumentPath = "TransportContractCopyUpload\\" + DocumentPath;
        }

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath(DocumentPath);
            ServerPath = ServerPath.Replace("Transport\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSellDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "detentioncopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "varaicopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emptycontcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "tollcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "othercopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emailapprovalcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "contractcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
    }

}