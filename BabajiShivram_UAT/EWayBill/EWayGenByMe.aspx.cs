using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using Newtonsoft.Json;
public partial class EWayBill_EWayGenByMe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Get Eway Bill Assigned";
        if (!IsPostBack)
        {
            Session["EwayPrint"] = null;

            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            DBOperations.FillStateGSTID(ddState);

            ddState.SelectedValue = "27";// Maharashtra
        }
    }

    private void GetEWayAssigneByDate()
    {
        EWBSession EwbSession = new EWBSession(ddTrasnporter.SelectedValue);

        string strDate = DateTime.Now.ToString("dd/MM/yyyy");

        if (txtDate.Text.Trim() != "")
        {
            strDate = txtDate.Text.Trim();
        }

        TxnRespWithObj<List<AssignedEWBItem>> TxnResp = EWBAPI.GetEWBAssignedForTransAsync(EwbSession, strDate);

        if (TxnResp.IsSuccess)
        {
            //lblErrorMsg.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
            gvPendingPartB.DataSource = TxnResp.RespObj;
            gvPendingPartB.DataBind();
        }
        else
        {
            gvPendingPartB.DataSource = null;
            gvPendingPartB.DataBind();

            //lblErrorMsg.Text = "No Record Found For Selected Date & Transporter - " + ddTrasnporter.SelectedItem.Text;
            lblErrorMsg.Text = TxnResp.TxnOutcome;
            lblErrorMsg.CssClass = "errorMsg";
        }
        //txtHeading.Text = "Get e-Way Bill assigned to you for transportation";
    }
    private void GetEWayAssigneByGSTIN()
    {
        EWBSession EwbSession = new EWBSession(ddTrasnporter.SelectedValue);

        string strDate = DateTime.Now.ToString("dd/MM/yyyy");
        string strGSTIN = ddTrasnporter.SelectedValue;

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

        if (txtGSTIN.Text.Trim() != "")
        {
            strGSTIN = txtGSTIN.Text.Trim();
        }

        TxnRespWithObj<List<AssignedEWBItem>> TxnResp = EWBAPI.GetEWBAssignedForTransByGstinAsync(EwbSession, strDate, strGSTIN);

        if (TxnResp.IsSuccess)
        {
            //lblErrorMsg.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
            gvPendingPartB.DataSource = TxnResp.RespObj;
            gvPendingPartB.DataBind();
        }
        else
        {
            gvPendingPartB.DataSource = null;
            gvPendingPartB.DataBind();

            //lblErrorMsg.Text = "No Record Found For Selected Date & Transporter - " + ddTrasnporter.SelectedItem.Text;
            lblErrorMsg.Text = TxnResp.TxnOutcome;
            lblErrorMsg.CssClass = "errorMsg";
        }
        //txtHeading.Text = "Get e-Way Bill assigned to you for transportation";
    }

    private void GetEWayAssigneBySTATE()
    {
        EWBSession EwbSession = new EWBSession(ddTrasnporter.SelectedValue);

        string strDate = DateTime.Now.ToString("dd/MM/yyyy");
        int StateCode = 0;
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
        if (ddState.SelectedIndex > 0)
        {
            StateCode = Convert.ToInt32(ddState.SelectedValue);
        }
        else
        {
            lblErrorMsg.Text = "Please Select State!";
            lblErrorMsg.CssClass = "errorMsg";
            return;
        }

        TxnRespWithObj<List<RespGetEwayBillsForTranByState>> TxnResp = EWBAPI.GetEwayBillsForTransporterByState(EwbSession, strDate, StateCode);

        if (TxnResp.IsSuccess)
        {
            //lblErrorMsg.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
            gvPendingPartB.DataSource = TxnResp.RespObj;
            gvPendingPartB.DataBind();
        }
        else
        {
            gvPendingPartB.DataSource = null;
            gvPendingPartB.DataBind();

            //lblErrorMsg.Text = "No Record Found For Selected Date & Transporter - " + ddTrasnporter.SelectedItem.Text;
            lblErrorMsg.Text = TxnResp.TxnOutcome;
            lblErrorMsg.CssClass = "errorMsg";
        }
        //txtHeading.Text = "Get e-Way Bill assigned to you for transportation";
    }

    protected void gvPendingPartB_RowCommand(Object sender, GridViewCommandEventArgs e)
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
        if (rbCategory.SelectedValue == "GENDT")
        {
            GetEWayAssigneByDate();
        }
        else if (rbCategory.SelectedValue == "GENTR")
        {
            GetEWayAssigneByGSTIN();
        }
        else if (rbCategory.SelectedValue == "GENST")
        {
            GetEWayAssigneBySTATE();
        }
    }

    protected void gvPendingPartB_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void rbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvPendingPartB.DataSource = null;
        gvPendingPartB.DataBind();

        if (rbCategory.SelectedItem.Value == "GENDT")
        {
            pnlGenDate.Visible  = true;

            pnlBillNo.Visible   = false;
            pnlGenGSTN.Visible  = false;
            pnlGenState.Visible = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENTR")
        {
            pnlGenGSTN.Visible  = true;
            pnlGenDate.Visible  = true;

            pnlGenState.Visible = false;
            pnlBillNo.Visible   = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENST")
        {
            pnlGenState.Visible = true;
            pnlGenDate.Visible  = true;

            pnlGenGSTN.Visible  = false;
            pnlBillNo.Visible   = false;

        }
    }
}