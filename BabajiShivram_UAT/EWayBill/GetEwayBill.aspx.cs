using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using TaxProEWB.API;
using System.IO;
public partial class EWayBill_GetEwayBill : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Get EWay Bill Detail";

        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);

            Session["EwayPrint"] = null;

            Session["userGSTIN"] = null;
        }
    }

    protected void btnGetewayBill_Click(object sender, EventArgs e)
    {
        long EwbNo = 0;// Convert.ToInt64(txtEwayBillNo.Text.Trim());

        Int64.TryParse(txtEwayBillNo.Text.Trim(), out EwbNo);

        if (EwbNo > 0)
        {
            Session["EwayPrint"] = EwbNo;
            Session["userGSTIN"] = ddTrasnporter.SelectedValue;

            Response.Redirect("PrintEwayBill.aspx");
        }
        else
        {
            lblErrorMsg.Text = "Invalid EWay Bill Number!";
            lblErrorMsg.CssClass = "errorMsg";
        }

        //TxnRespWithObj<RespGetEWBDetail> TxnResp = EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo);

        //if (TxnResp.IsSuccess)
        //{
        //    string JsonFilePath = @"d:\\EWayJson\\EWayResponse_211010556881.txt";
        //    StreamReader sr = new StreamReader(JsonFilePath);
        //    sr = File.OpenText(JsonFilePath);
        //    string responseBody = sr.ReadToEnd();
        //    sr.Close();
        //    sr.Dispose();
        //    GC.Collect();
        //    //rtbResponce.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
        //    rtbResponce.Text = responseBody;
        //}
        //else
        //    rtbResponce.Text = "";// TxnResp.TxnOutcome;
        //txtHeading.Text = "Get e-Way Bill Details Responce";
    }
}