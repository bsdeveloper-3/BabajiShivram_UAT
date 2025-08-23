using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
public partial class EWayBill_ConfirmMultiVehicle : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        lblTitle.Text = "Multi Vehicle Update";

        if (!IsPostBack)
        {
            if (Session["userGSTIN"] != null && Session["EwayMultiVeh"] != null)
            {
                EWBSession EwbSession = new EWBSession(Session["userGSTIN"].ToString());

                long EwbNo = Convert.ToInt64(Session["EwayMultiVeh"]); //211010556881;

                ViewBillDetail(EwbNo);
                                
                Page.Validate();
            }
            else
            {
                lblErrorMsg.Text = "Session Expired! Please Try Again.";
                lblErrorMsg.CssClass = "errorMsg";
            }
        }
    }
    private void ViewBillDetail(long EwbNo)
    {
        if (Session["userGSTIN"] != null && Session["EwayMultiVeh"] != null)
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

                    hdnFrmStateCode.Value = TxnResp.RespObj.fromStateCode.ToString();
                }
                if (TxnResp.RespObj.toStateCode > 0)
                {
                    strToState = DBOperations.GetStateNameByCode(TxnResp.RespObj.toStateCode);
                    hdnToStateCode.Value = TxnResp.RespObj.toStateCode.ToString();
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
                    //Vehicle Detail
                    lblTransDocNo.Text = TxnResp.RespObj.VehiclListDetails[0].transDocNo + " & " + TxnResp.RespObj.VehiclListDetails[0].transDocDate;
                    
                    gvVehicleHistory.DataSource = TxnResp.RespObj.VehiclListDetails;
                    gvVehicleHistory.DataBind();
                }

                // Product Detail
                RespGetEWBDetail.ItmList ProductInfo = new RespGetEWBDetail.ItmList();

                GVItemList.DataSource = TxnResp.RespObj.itemList;
                GVItemList.DataBind();

                // Part B Detail

                lblEWayNo.Text = TxnResp.RespObj.ewbNo.ToString();

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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Session["userGSTIN"] != null && Session["EwayMultiVeh"] != null)
        {
            string strEWayBillNo = Session["EwayMultiVeh"].ToString();

            EWBSession EwbSession = new EWBSession(Session["userGSTIN"].ToString());

            ReqVehicleNoUpdtPl reqVehicleNo = new ReqVehicleNoUpdtPl();

            //reqVehicleNo.EwbNo = Convert.ToInt64(strEWayBillNo);
            
            //reqVehicleNo.FromPlace = txtFromPlace.Text.Trim();
            
            //reqVehicleNo.ReasonCode = ddReason.SelectedValue;
            //reqVehicleNo.ReasonRem = ddReason.SelectedItem.Text;
            
            //reqVehicleNo.TransMode = rblMode.SelectedValue;

            /*******************Multi Vehicle Request*******************/

            ReqIniMultiVehicleMov objIniMultiVehUpd = new ReqIniMultiVehicleMov();

            objIniMultiVehUpd.ewbNo = Convert.ToInt64(strEWayBillNo); 
            objIniMultiVehUpd.reasonCode = ddReason.SelectedValue;
            objIniMultiVehUpd.reasonRem = txtRemark.Text.Trim();
            objIniMultiVehUpd.fromPlace = txtFromPlace.Text.Trim();
            objIniMultiVehUpd.fromState = Convert.ToInt32(hdnToStateCode.Value);
            objIniMultiVehUpd.toPlace = txtToPlace.Text.Trim();
            objIniMultiVehUpd.toState = Convert.ToInt32(hdnToStateCode.Value); 
            objIniMultiVehUpd.transMode = rblMode.SelectedValue;
            objIniMultiVehUpd.totalQuantity = Convert.ToInt32(txtTotalQuantity.Text.Trim());
            objIniMultiVehUpd.unitCode = ddUnit.SelectedValue;


            /**********************************************************/

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(objIniMultiVehUpd);

            TxnRespWithObj<RespIniMultiVehicleMov> responceIniMulVeh = new TxnRespWithObj<RespIniMultiVehicleMov>();

            responceIniMulVeh = EWBAPI.InitiateMultiVehMovntAsync(EwbSession, json);

            if (responceIniMulVeh.IsSuccess)
            {
                lblErrorMsg.Text = responceIniMulVeh.TxnOutcome;
                lblErrorMsg.CssClass = "success";

                Session["EwayMessage"] = "Movment Updated with Group Name: " + responceIniMulVeh.RespObj.groupNo;

                lblErrorMsg.Text = Session["EwayMessage"].ToString();
                lblErrorMsg.CssClass = "success";

                //DBOperations.EWAYAddStatus(0, strEWayBillNo, 1, DateTime.Now.ToString("dd/MM/yyyy"), ddReason.SelectedItem.Text, "From Place-" + txtFromPlace.Text.Trim() + "-" + ddState.SelectedItem.Text, loggedInUser.glUserId);

                //DBOperations.EWAYAddVehicle(0, strEWayBillNo, txtVehicleNo.Text.Trim(), txtVehicleDate.Text.Trim(), resVehicleNoUpdt.RespObj.validUpto,
                //txtFromPlace.Text.Trim(), Convert.ToInt32(ddState.SelectedValue), ddReason.Text.Trim(), loggedInUser.glUserId);

                //Response.Redirect("EwaySuccess.aspx");
            }
            else
            {
                lblErrorMsg.Text = responceIniMulVeh.TxnOutcome;
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
}