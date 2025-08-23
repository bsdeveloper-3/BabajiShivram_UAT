using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using System.IO;

public partial class EWayBill_EWayGenByDate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "EWay Bill By Date";

        CalBillDate.EndDate = DateTime.Now;

        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);

            Session["EwayPrint"] = null;

            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    private void GetEWayForTransport()
    {
        Session["EWayRespObj"] = null;

        EWBSession EwbSession = new EWBSession(ddTrasnporter.SelectedValue);

        string strDate = DateTime.Now.ToString("dd/MM/yyyy");

        if (txtDate.Text.Trim() != "")
        {
            strDate = txtDate.Text.Trim();
        }
        else
        {
            lblErrorMsg.Text = "Please Enter EWay Bill Date!";
            lblErrorMsg.CssClass = "errorMsg";
            return;
        }

        TxnRespWithObj<List<RespGetEwayBillsByDate>> TxnResp = EWBAPI.GetEwayBillsByDateAsync(EwbSession, strDate);

        if (TxnResp.IsSuccess)
        {
            // Store Result In Session For Excel Export
            Session["EWayRespObj"] = TxnResp.RespObj;
            //lblErrorMsg.Text = JsonConvert.SerializeObject(TxnResp.RespObj);

            gvEWayBill.DataSource = TxnResp.RespObj;
            gvEWayBill.DataBind();
        }
        else
        {
            gvEWayBill.DataSource = null;
            gvEWayBill.DataBind();

            //lblErrorMsg.Text = "No Record Found For Selected Date & Transporter - " + ddTrasnporter.SelectedItem.Text;
            lblErrorMsg.Text = TxnResp.TxnOutcome;
            lblErrorMsg.CssClass = "errorMsg";
        }
        //txtHeading.Text = "Get e-Way Bill assigned to you for transportation";
    }
    protected void gvEWayBill_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "print")
        {
            string strBillNo = (string)e.CommandArgument;
            Session["EwayPrint"] = strBillNo;

            Response.Redirect("PrintEWayBill.aspx");
        }
    }
    protected void btnShowBill_Click(object sender, EventArgs e)
    {
        GetEWayForTransport();
    }

    protected void gvEWayBill_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "EWayBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvEWayBill.AllowPaging = false;
        gvEWayBill.AllowSorting = false;

        gvEWayBill.Columns[1].Visible = false;
        gvEWayBill.Columns[2].Visible = true;

        // Retrive Resilt From Session 

        if (Session["EWayRespObj"] != null)
        {
            lblErrorMsg.Text = "success";
            lblErrorMsg.CssClass = "errorMsg";
            List<RespGetEwayBillsByDate> RespObj = new List<RespGetEwayBillsByDate>();
            RespObj = (List<RespGetEwayBillsByDate>)Session["EWayRespObj"];

            gvEWayBill.DataSource = RespObj;
            gvEWayBill.DataBind();
        }
        else
        {
            GetEWayForTransport();
        }

        gvEWayBill.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}