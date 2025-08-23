using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using System.Drawing;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
public partial class EWayBill_PrintEWayBill : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "E-Way Bill Print";

        if (!IsPostBack)
        {
            if (Session["EwayPrint"] != null)
            {
                long EwbNo = Convert.ToInt64(Session["EwayPrint"]); //211010556881;

                GetBillResponse(EwbNo);
            }
            else
            {
                btnDownloadPDF.Visible = false;
                lblErrorMsg.Text = "Session Expired! Please Go back and click on Eway Bill Number.";
                lblErrorMsg.CssClass = "errorMsg";
            }
        }
        else if(hdnResponse.Value != "")
        {
            ParseEwayResponseJson(hdnResponse.Value);
        }
    }
    private void GetBillResponse(long EwbNo)
    {        
        // Get EWay Bill API GSTIN

        string strAPIGSTIN = DBOperations.GetEwayAPIGstinByBillNo(EwbNo);

        if (strAPIGSTIN == "")
        {
            if (Session["userGSTIN"] != null)
            {
                strAPIGSTIN = Session["userGSTIN"].ToString(); // User Selected API GSTIN
            }
            else
            {
                // SET NAVBHARAT GSTIN AS Default Value
                strAPIGSTIN = "27AAACN1163G1ZR";
                Session["userGSTIN"] = strAPIGSTIN;
            }

        }
        else
        {
            Session["userGSTIN"] = strAPIGSTIN; // 
        }

        EWBSession EwbSession = new EWBSession(strAPIGSTIN);

        TxnRespWithObj<RespGetEWBDetail> TxnResp = EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo);

        if (TxnResp.IsSuccess)
        {
            hdnResponse.Value = TxnResp.RawData;

            // View Bill Detail    

            ViewBillDetail(TxnResp.RespObj);

            SaveEwayBillDetail(TxnResp.RespObj, TxnResp.RawData, EwbSession, strAPIGSTIN);
        }
        else if(TxnResp.TxnOutcome.StartsWith("325"))
        {
            // Check With Navjeen GSTIN

            strAPIGSTIN = "27AAFFN5296A1ZA";//NAVJEEVAN
            Session["userGSTIN"] = strAPIGSTIN;

            EwbSession = new EWBSession(strAPIGSTIN);

            TxnResp = EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo);

            if (TxnResp.IsSuccess)
            {
                hdnResponse.Value = TxnResp.RawData;

                // View Bill Detail    

                ViewBillDetail(TxnResp.RespObj);

                SaveEwayBillDetail(TxnResp.RespObj, TxnResp.RawData, EwbSession, strAPIGSTIN);
            }
            else
            {
                lblErrorMsg.Text = TxnResp.TxnOutcome;
                lblErrorMsg.CssClass = "errorMsg";
            }
        }
        else
        {
            lblErrorMsg.Text = TxnResp.TxnOutcome;
            lblErrorMsg.CssClass = "errorMsg";
        }
    }
    private void ParseEwayResponseJson (string ResponseJSON)
    {
        RespGetEWBDetail getEwbDetail;
        getEwbDetail = JsonConvert.DeserializeObject<RespGetEWBDetail>(ResponseJSON, APITools.jsonSettings);

        ViewBillDetail(getEwbDetail);
    }
    private void ViewBillDetail(RespGetEWBDetail RespObj)
    {
        string strSupplyType = "Inward", strSubSupplyType = "";
        string strFromState = ""; string strToState = "";
        
        if (RespObj.supplyType.ToUpper().Trim() == "O")
        {
            strSupplyType = "Outward";
        }
        if(RespObj.subSupplyType != "")
        {
            EWBBillCode.EwbBillCodes.TryGetValue(RespObj.subSupplyType.Trim(), out strSubSupplyType);
        }
        if (RespObj.status.ToUpper() == "ACT")
        {
            lblErrorMsg.Text = "Status: Active";
            lblErrorMsg.CssClass = "success";
        }
        else if (RespObj.status.ToUpper() == "CNL")
        {
            lblErrorMsg.Text = "Status: Inactive";
            lblErrorMsg.CssClass = "errorMsg";
        }
        else if (RespObj.rejectStatus.ToUpper() == "Y")
        {
            lblErrorMsg.Text = "Status: Rejected";
            lblErrorMsg.CssClass = "errorMsg";
        }

        lblBillNoDetails.Text   =   RespObj.ewbNo.ToString();
        lblGenDateDetails.Text  =   RespObj.ewayBillDate;
        lblGenDetails.Text      =   RespObj.userGstin;
        lblValidUPtoDetails.Text =  RespObj.validUpto;
        lblModeDetails.Text     =   RespObj.genMode;
        lblApxDistDetails.Text  =   RespObj.actualDist.ToString() +" KM";     

        lblTypeDetails.Text = strSupplyType + " - " + strSubSupplyType;

        lblDocDet.Text = RespObj.docType + " - " + RespObj.docNo + " - " + RespObj.docDate;

        if (RespObj.fromStateCode > 0)
        {
            strFromState = DBOperations.GetStateNameByCode(RespObj.fromStateCode);
        }
        if (RespObj.toStateCode > 0)
        {
            strToState = DBOperations.GetStateNameByCode(RespObj.toStateCode);
        }

        txtGenBy.Value = "GSTIN:" + RespObj.fromGstin + "\n"+ RespObj.fromTrdName +
            "\n" + RespObj.fromAddr1 + "\n" + RespObj.fromAddr2 +
            "\n" + RespObj.fromPlace + "\n" + strFromState + " " + RespObj.fromPincode;

        txtSypplyTo.Value = "GSTIN:" + RespObj.toGstin + "\n" + RespObj.toTrdName +
            "\n" + RespObj.toAddr1 + "\n" + RespObj.toAddr2 +
            "\n" + RespObj.toPlace + "\n" + strToState + " " + RespObj.toPincode;

        lblValue.Text   =   RespObj.totalValue.ToString();
        lblcgst.Text    =   RespObj.cgstValue.ToString();
        lblsgst.Text    =   RespObj.sgstValue.ToString();
        lbligst.Text    =   RespObj.igstValue.ToString();
        lblcess.Text    =   RespObj.cessValue.ToString();
        
        //lblCessAdvol.Text = RespObj.ce
        // Transporter

        lblTransportor.Text = RespObj.transporterId + " & " + RespObj.transporterName;

        if(RespObj.VehiclListDetails.Count > 0)
        {
            lblTransDocNo.Text = RespObj.VehiclListDetails[0].transDocNo + " & " + RespObj.VehiclListDetails[0].transDocDate;
        }
        if (RespObj.VehiclListDetails.Count > 0)
        {
            string strModeCode = RespObj.VehiclListDetails[0].transMode.Trim();

            //if (strModeCode == "1")
            //    lblMode.Text = "Road";
            //else if (strModeCode == "2")
            //    lblMode.Text = "Rail";
            //else if (strModeCode == "3")
            //    lblMode.Text = "Air";
            //else if (strModeCode == "4")
            //    lblMode.Text = "Ship";
        }

        //    lblTransDocNo.Text = RespObj.VehiclListDetails[0].transDocNo + " & " + RespObj.VehiclListDetails[0].transDocDate;

        //    lblVehicleNo.Text = RespObj.VehiclListDetails[0].vehicleNo;
        //    lblVehicleDate.Text = RespObj.VehiclListDetails[0].transDocDate;
        //    lblTransportFrom.Text = RespObj.VehiclListDetails[0].fromPlace;
        //    lblVehicleEnterDate.Text = RespObj.VehiclListDetails[0].enteredDate;
        //    lblVehicleEnteredBy.Text = RespObj.VehiclListDetails[0].userGSTINTransin;
        //}

        gvVehicleHistory.DataSource = RespObj.VehiclListDetails;
        gvVehicleHistory.DataBind();
        // Product Detail
        RespGetEWBDetail.ItmList ProductInfo = new RespGetEWBDetail.ItmList();
            
        GVItemList.DataSource = RespObj.itemList;
        GVItemList.DataBind();

        // Generate Bar Code
        //GenerateBarCode(RespObj.ewbNo.ToString());

        // Generate QR Code
        //GeneratedQRCode(RespObj.ewbNo.ToString());
    }       
    private void GenerateBarCode(string strEwayBillNo)
    {
        string barCode = strEwayBillNo;

        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

        using (Bitmap bitMap = new Bitmap(barCode.Length * 40, 80))
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M", 12);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();

                Convert.ToBase64String(byteImage);
                imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
            //plBarCode.Controls.Add(imgBarCode);
        }
    }
    private void GeneratedQRCode (string strEwayBillNo)
    {
        //imgQrCode.ImageUrl = "https://chart.googleapis.com/chart?cht=qr&chl=" + 
        //   WebUtility.HtmlEncode(strEwayBillNo) + "&choe=UTF-8&chs=" + imgQrCode.Height.ToString().Replace("px", "") + "x" + imgQrCode.Width.ToString().Replace("px", "");
    }   
    protected void btnDownloadPDF_Click(object sender, EventArgs e)
    {
        string ServerPath = FileServer.GetFileServerDir();
        string FileName = lblBillNoDetails.Text + ".pdf";

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\");
        }

        ServerPath = ServerPath + "\\EwayPDF";

        string UploadPath = ServerPath + "\\" + FileName;

        string BillResponse = hdnResponse.Value.Trim();
        string strUserGSTIN = "27AAACN1163G1ZR";

        if (Session["userGSTIN"] != null)
        {
            strUserGSTIN = Session["userGSTIN"].ToString();
        }

        EWBSession EwbSession = new EWBSession(strUserGSTIN);

        //string strURL = @"https://api.taxprogsp.co.in/aspapi/v1.0/printewb";
        string strURL = @"https://einvapi.charteredinfo.com/aspapi/v1.0/printewb";

        RestClient client = new RestClient(strURL);

        RestRequest request = new RestRequest(Method.POST);
        request.AddParameter("aspid", EwbSession.EwbApiSetting.ID.ToString(), ParameterType.QueryString);
        request.AddParameter("Gstin", EwbSession.EwbApiLoginDetails.EwbGstin, ParameterType.QueryString);
        request.AddParameter("password", EwbSession.EwbApiSetting.AspPassword, ParameterType.QueryString);
        request.AddParameter("undefined", BillResponse, ParameterType.RequestBody); // EWay Detail Response JSON

        try
        {
            // Upload File On Server
            client.DownloadData(request).SaveAs(UploadPath);

            // File Download ON Client

            DownloadDocument(UploadPath);
        }
        catch(Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.CssClass = "errorMsg";
        }

        // Upload File on Server

        //byte[] response = client.DownloadData(request);
        //File.WriteAllBytes(ServerPath, response);
        
        // TxnRespWithObj<Object> TxnResp = EWBAPI.PrintEWBAsync(EwbSession, BillResponse);
    }
    private void DownloadDocument(string ServerPath)
    {
        try
        {
           System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, "");
        }
        catch (Exception ex)
        {
        }
    }
    protected void gvVehicleHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblMode = (Label)e.Row.FindControl("lblMode");

            if(lblMode != null)
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
    protected void GVItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow)
        {
            Label CBOlblCRate = (Label)e.Row.FindControl("lblCRate");
            Label CBOlblSRate = (Label)e.Row.FindControl("lblSRate");
            Label CBOlblIRate = (Label)e.Row.FindControl("lblIRate");

            if(CBOlblCRate != null && CBOlblSRate != null && CBOlblIRate != null )
            {
                if(CBOlblCRate.Text.Trim() == "0" || CBOlblCRate.Text.Trim() == "-1")
                {
                    CBOlblCRate.Text = "NA";
                }
                if (CBOlblSRate.Text.Trim() == "0" || CBOlblSRate.Text.Trim() == "-1")
                {
                    CBOlblSRate.Text = "NA";
                }
                if (CBOlblIRate.Text.Trim() == "0" || CBOlblIRate.Text.Trim() == "-1")
                {
                    CBOlblIRate.Text = "NA";
                }
            }

        }
    }
    private static void SaveEwayBillDetail(RespGetEWBDetail RespObj, string strRawData, EWBSession EwbSession1, string strAPIGSTIN)
    {

        int intTransType = 1; // "I" Inward
        int intTansSubType = 0;

        string strEWayBillNo = RespObj.ewbNo.ToString();
        string strEWayBillDate = RespObj.ewayBillDate;
        string strStatus = RespObj.status.ToUpper();
        string strRejectStatus = RespObj.rejectStatus.ToUpper();
        string strValidityDate = RespObj.validUpto;

        if (RespObj.supplyType.ToUpper().Trim() == "O")
        {
            intTransType = 2; // Outward";
        }
        if (RespObj.subSupplyType != "")
        {
            intTansSubType = Convert.ToInt32(RespObj.subSupplyType.Trim());
        }

        string strValidUPto = RespObj.validUpto;

        string strDocType = RespObj.docType;
        string strDocNo = RespObj.docNo;
        string strDocDate = RespObj.docDate;

        DateTime dtDocDate = Commonfunctions.CDateTime(strDocDate);

        // Delivery

        int intFromStateCode = RespObj.fromStateCode;
        int intToState = RespObj.toStateCode;

        string strUserGSTIN = RespObj.userGstin;
        string strFromGSTIN = RespObj.fromGstin;
        string strToGSTIN = RespObj.toGstin;
        string strFromPlace = RespObj.fromPlace;
        string strToPlace = RespObj.toPlace;

        int intFromPin = Convert.ToInt32(RespObj.fromPincode);
        int intToPin = Convert.ToInt32(RespObj.toPincode);

        // GST

        decimal decTotalinvoicevalue = Convert.ToDecimal(RespObj.totInvValue);
        decimal decCGST = Convert.ToDecimal(RespObj.cgstValue);
        decimal decSGST = Convert.ToDecimal(RespObj.sgstValue);
        decimal decIGST = Convert.ToDecimal(RespObj.igstValue);
        decimal decCESS = Convert.ToDecimal(RespObj.cessValue);

        // Transporter

        string strTransportorGSTIN = RespObj.transporterId;

        string strVehicleNo = "";
        string strLRNo = "";
        string strLRDate = "";

        int intExtendedTimes = Convert.ToInt32(RespObj.extendedTimes);

        if (RespObj.VehiclListDetails.Count > 0)
        {
            strVehicleNo = RespObj.VehiclListDetails[0].vehicleNo;
            strLRNo = RespObj.VehiclListDetails[0].transDocNo;
            strLRDate = RespObj.VehiclListDetails[0].transDocDate;
        }

        DBOperations.UpdateEWayBill(strEWayBillNo, intTransType, intTansSubType, strDocType, strDocNo, dtDocDate,
            strFromGSTIN, strToGSTIN, strToPlace, intToPin, intToState, decTotalinvoicevalue, decCGST, decSGST,
            decIGST, decCESS, strTransportorGSTIN, strVehicleNo, strLRDate, strStatus, strRejectStatus, strValidityDate,
            intExtendedTimes, strEWayBillDate, strRawData, strUserGSTIN, strAPIGSTIN, 1);

    }
}