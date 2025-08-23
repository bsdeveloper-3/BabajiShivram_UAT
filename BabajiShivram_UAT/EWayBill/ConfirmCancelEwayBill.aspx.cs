using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using Newtonsoft.Json;
public partial class EWayBill_ConfirmCancelEwayBill : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Cancel EWay Bill";
        if (!IsPostBack)
        {            
            if (Session["EwayCancel"] != null)
            {
                long EwbNo = Convert.ToInt64(Session["EwayCancel"]); //211010556881;

                ViewBillDetail(EwbNo);

                Page.Validate();
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

                //if (TxnResp.RespObj.VehiclListDetails.Count > 0)
                //{
                //    // Vehicle Detail
                //    lblTransDocNo.Text = TxnResp.RespObj.VehiclListDetails[0].transDocNo + " & " + TxnResp.RespObj.VehiclListDetails[0].transDocDate;
                //    //lblMode.Text = TxnResp.RespObj.VehiclListDetails[0].transMode;
                //    lblVehicleNo.Text = TxnResp.RespObj.VehiclListDetails[0].vehicleNo;
                //    lblVehicleDate.Text = TxnResp.RespObj.VehiclListDetails[0].transDocDate;
                //    lblTransportFrom.Text = TxnResp.RespObj.VehiclListDetails[0].fromPlace;
                //    lblVehicleEnterDate.Text = TxnResp.RespObj.VehiclListDetails[0].enteredDate;
                //    lblVehicleEnteredBy.Text = TxnResp.RespObj.VehiclListDetails[0].userGSTINTransin;
                //}

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
    protected void btnCancelBill_Click(object sender, EventArgs e)
    {
        if (ddCancelReason.SelectedIndex > 0)
        {
            if(txtCancelRemark.Text.Trim() == "")
            {
                lblErrorMsg.Text = "Please Enter Remark For Cancellation!";
                lblErrorMsg.CssClass = "errorMsg";
            }
            else if (Session["userGSTIN"] != null && Session["EwayCancel"] != null)
            {
                List<RespCancelEwbPl> EwayCancelList = new List<RespCancelEwbPl>();

                EWBSession EwbSession = new EWBSession(Session["userGSTIN"].ToString());
                long EwbNo = Convert.ToInt64(Session["EwayCancel"]); //211010556881;

                ReqCancelEwbPl reqCancelEWB = new ReqCancelEwbPl();
                reqCancelEWB.ewbNo = EwbNo;
                reqCancelEWB.cancelRsnCode = Convert.ToInt32(ddCancelReason.SelectedValue);
                reqCancelEWB.cancelRmrk = txtCancelRemark.Text.Trim();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(reqCancelEWB);

                TxnRespWithObj<RespCancelEwbPl> respCancelEWB = EWBAPI.CancelEWBAsync(EwbSession, json);
                if (respCancelEWB.IsSuccess)
                {
                    lblErrorMsg.Text = respCancelEWB.TxnOutcome;
                    lblErrorMsg.CssClass = "success";

                    DBOperations.EWAYAddStatus(0, respCancelEWB.RespObj.ewayBillNo, 2, respCancelEWB.RespObj.cancelDate,
                        ddCancelReason.SelectedItem.Text, txtCancelRemark.Text.Trim(), loggedInUser.glUserId);

                    EwayCancelList.Add(respCancelEWB.RespObj);
                    Session["EwayMessageCencel"] = EwayCancelList;

                    Response.Redirect("EWaySuccess.aspx");
                }
                else
                {
                    lblErrorMsg.Text = respCancelEWB.TxnOutcome;
                    lblErrorMsg.CssClass = "errorMsg";
                }
            }//END_IF_Session
            else
            {
                lblErrorMsg.Text = "Session Expired! Please Try again";
                lblErrorMsg.CssClass = "errorMsg";
            }
        }
        else
        {
            lblErrorMsg.Text = "Please Select Cancellation Reason!";
            lblErrorMsg.CssClass = "errorMsg";
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("CancelEWayBill.aspx");
    }
}