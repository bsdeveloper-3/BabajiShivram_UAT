using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
public partial class EWayBill_ConfirmRejectEwayBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Reject EWay Bill";
        if (!IsPostBack)
        {
            if (Session["EwayReject"] != null)
            {
                long EwbNo = Convert.ToInt64(Session["EwayReject"]); //211010556881;

                ViewBillDetail(EwbNo);
            }
        }
    }

    private void ViewBillDetail(long EwbNo)
    {
        if (Session["userGSTIN"] != null)
        {
            EWBSession EwbSession = new EWBSession(Session["userGSTIN"].ToString());

            TxnRespWithObj<RespGetEWBDetail> TxnResp = EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo);

            if (TxnResp.IsSuccess)
            {
                string strSupplyType = "Inward", strSubSupplyType = "";
                string strFromState = ""; string strToState = "";

                if (TxnResp.RespObj.supplyType.ToUpper().Trim() == "O")
                {
                    strSupplyType = "Outward";
                }
                if (TxnResp.RespObj.subSupplyType != "")
                {
                    EWBBillCode.EwbBillCodes.TryGetValue(TxnResp.RespObj.subSupplyType.Trim(), out strSubSupplyType);
                }
                if (TxnResp.RespObj.status.ToUpper() == "ACT")
                {
                    lblErrorMsg.Text = "Status: Active";
                    lblErrorMsg.CssClass = "success";
                }
                else if (TxnResp.RespObj.status.ToUpper() == "CNL")
                {
                    lblErrorMsg.Text = "Status: Inactive";
                    lblErrorMsg.CssClass = "errorMsg";
                }
                else if (TxnResp.RespObj.rejectStatus.ToUpper() == "Y")
                {
                    lblErrorMsg.Text = "Status: Rejected";
                    lblErrorMsg.CssClass = "errorMsg";
                }
                lblBillNoDetails.Text = TxnResp.RespObj.ewbNo.ToString();
                lblGenDateDetails.Text = TxnResp.RespObj.ewayBillDate;
                lblGenDetails.Text = TxnResp.RespObj.userGstin;
                lblGenDateDetails.Text = TxnResp.RespObj.ewayBillDate;
                lblValidUPtoDetails.Text = TxnResp.RespObj.validUpto;
                lblModeDetails.Text = TxnResp.RespObj.genMode;
                lblApxDistDetails.Text = TxnResp.RespObj.actualDist.ToString() + " KM";

                lblTypeDetails.Text = strSupplyType + " - " + strSubSupplyType;

                lblDocDet.Text = TxnResp.RespObj.docType + " - " + TxnResp.RespObj.docNo + " - " + TxnResp.RespObj.docDate;

                if (TxnResp.RespObj.fromStateCode > 0)
                {
                    strFromState = DBOperations.GetStateNameByCode(TxnResp.RespObj.fromStateCode);
                }
                if (TxnResp.RespObj.toStateCode > 0)
                {
                    strToState = DBOperations.GetStateNameByCode(TxnResp.RespObj.toStateCode);
                }
                txtGenBy.Value = "GSTIN:" + TxnResp.RespObj.fromGstin + "\n" + TxnResp.RespObj.fromTrdName +
                "\n" + TxnResp.RespObj.fromAddr1 + "\n" + TxnResp.RespObj.fromAddr2 +
                "\n" + TxnResp.RespObj.fromPlace + "\n" + strFromState + " " + TxnResp.RespObj.fromPincode;

                txtSypplyTo.Value = "GSTIN:" + TxnResp.RespObj.toGstin + "\n" + TxnResp.RespObj.toTrdName +
                    "\n" + TxnResp.RespObj.toAddr1 + "\n" + TxnResp.RespObj.toAddr2 +
                    "\n" + TxnResp.RespObj.toPlace + "\n" + strToState + " " + TxnResp.RespObj.toPincode;

                lblValue.Text = TxnResp.RespObj.totalValue.ToString();
                lblcgst.Text = TxnResp.RespObj.cgstValue.ToString();
                lblsgst.Text = TxnResp.RespObj.sgstValue.ToString();
                lbligst.Text = TxnResp.RespObj.igstValue.ToString();
                lblcess.Text = TxnResp.RespObj.cessValue.ToString();

                // Transporter

                lblTransportor.Text = TxnResp.RespObj.transporterId + " & " + TxnResp.RespObj.transporterName;

                if (TxnResp.RespObj.VehiclListDetails.Count > 0)
                {
                    lblTransDocNo.Text = TxnResp.RespObj.VehiclListDetails[0].transDocNo + " & " + TxnResp.RespObj.VehiclListDetails[0].transDocDate;
                }

                // Vehicle Detail
                //lblMode.Text = TxnResp.RespObj.VehiclListDetails[0].transMode;
                //lblVehicleNo.Text = TxnResp.RespObj.VehiclListDetails[0].vehicleNo;
                //lblVehicleDate.Text = TxnResp.RespObj.VehiclListDetails[0].transDocDate;
                //lblVehicleEnterDate.Text = TxnResp.RespObj.ewayBillDate;
                //lblVehicleEnteredBy.Text = TxnResp.RespObj.userGstin;

                gvVehicleHistory.DataSource = TxnResp.RespObj.VehiclListDetails;
                gvVehicleHistory.DataBind();
                // Product Detail
                RespGetEWBDetail.ItmList ProductInfo = new RespGetEWBDetail.ItmList();

                GVItemList.DataSource = TxnResp.RespObj.itemList;
                GVItemList.DataBind();
            }
            else
            {
                lblErrorMsg.Text = TxnResp.TxnOutcome;
                lblErrorMsg.CssClass = "errorMsg";
            }
        }//END_IF_Session
        else
        {
            lblErrorMsg.Text = "Session Expired! Please Try Again.";
            lblErrorMsg.CssClass = "errorMsg";
        }
    }
    protected void btnRejectBill_Click(object sender, EventArgs e)
    {
        if (Session["userGSTIN"] != null && Session["EwayCancel"] != null)
        {
            EWBSession EwbSession = new EWBSession(Session["userGSTIN"].ToString());

            long EwbNo = Convert.ToInt64(Session["EwayCancel"]); //211010556881;

            ReqRejectEwbPl reqRejectEWB = new ReqRejectEwbPl();
            reqRejectEWB.ewbNo = EwbNo;


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(reqRejectEWB);

            TxnRespWithObj<RespRejectEwbPl> respRejectEWB = EWBAPI.RejectEWBAsync(EwbSession, json);
            if (respRejectEWB.IsSuccess)
            {
                lblErrorMsg.Text = respRejectEWB.TxnOutcome;
                lblErrorMsg.CssClass = "success";

                Session["EwayMessage"] = "EWay Bill No:" + respRejectEWB.RespObj.ewayBillNo + " Rejected On " + respRejectEWB.RespObj.ewbRejectedDate;

                Response.Redirect("EWaySuccess.aspx");
            }
            else
            {
                lblErrorMsg.Text = respRejectEWB.TxnOutcome;
                lblErrorMsg.CssClass = "errorMsg";
            }
        }//END_IF_Session
        else
        {
            lblErrorMsg.Text = "Session Expired! Please Try again";
            lblErrorMsg.CssClass = "errorMsg";
        }
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("RejectEwayBill.aspx");
    }
    protected void gvVehicleHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblMode = (Label)e.Row.FindControl("lblMode");

            if (lblMode != null)
            {
                string strModeCode = lblMode.Text.Trim();

                if (strModeCode == "1")
                    lblMode.Text = "Road";
                else if (strModeCode == "2")
                    lblMode.Text = "Rail";
                else if (strModeCode == "3")
                    lblMode.Text = "Air";
                else if (strModeCode == "4")
                    lblMode.Text = "Ship";
            }
        }
    }
}