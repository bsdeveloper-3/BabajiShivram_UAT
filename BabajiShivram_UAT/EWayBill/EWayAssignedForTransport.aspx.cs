using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using System.IO;
public partial class EWayBill_EWayAssignedForTransport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Get Eway Bill Assigned";

        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);

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
            Session["EWayRespObj"] = TxnResp.RespObj;

            gvEWayBill.DataSource = TxnResp.RespObj;
            gvEWayBill.DataBind();
        }
        else
        {
            Session["EWayRespObj"] = null;
            gvEWayBill.DataSource = null;
            gvEWayBill.DataBind();

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
            Session["EWayRespObj"] = TxnResp.RespObj;
            gvEWayBill.DataSource = TxnResp.RespObj;
            gvEWayBill.DataBind();
        }
        else
        {
            Session["EWayRespObj"] = null;
            gvEWayBill.DataSource = null;
            gvEWayBill.DataBind();

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
            Session["EWayRespObj"] = TxnResp.RespObj;
            gvEWayBill.DataSource  = TxnResp.RespObj;
            gvEWayBill.DataBind();
        }
        else
        {
            Session["EWayRespObj"] = null;
            gvEWayBill.DataSource  = null;
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
        SearchEWayBill();
    }
    private void SearchEWayBill()
    {
        Session["EWayRespObj"] = null;

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
    protected void gvEWayBill_Sorting(object sender, GridViewSortEventArgs e)
    {

    }
    protected void rbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvEWayBill.DataSource = null;
        gvEWayBill.DataBind();

        if (rbCategory.SelectedItem.Value == "GENDT")
        {
            pnlGenDate.Visible  = true;
                        
            pnlGenGSTN.Visible  = false;
            pnlGenState.Visible = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENTR")
        {
            pnlGenGSTN.Visible  = true;
            pnlGenDate.Visible  = true;

            pnlGenState.Visible = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENST")
        {
            pnlGenState.Visible = true;
            pnlGenDate.Visible  = true;

            pnlGenGSTN.Visible  = false;
            
        }
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
        // Retrive Result From Session 

        if (Session["EWayRespObj"] != null)
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

        
            if (rbCategory.SelectedValue == "GENDT")
            {
                List<AssignedEWBItem> RespObj = new List<AssignedEWBItem>();
                RespObj = (List<AssignedEWBItem>)Session["EWayRespObj"];

                gvEWayBill.DataSource = RespObj;
                gvEWayBill.DataBind();
            }
            else if (rbCategory.SelectedValue == "GENTR")
            {
                List<AssignedEWBItem> RespObj = new List<AssignedEWBItem>();
                RespObj = (List<AssignedEWBItem>)Session["EWayRespObj"];

                gvEWayBill.DataSource = RespObj;
                gvEWayBill.DataBind();
            }
            else if (rbCategory.SelectedValue == "GENST")
            {
                GetEWayAssigneBySTATE();

                List<RespGetEwayBillsForTranByState> RespObj = new List<RespGetEwayBillsForTranByState>();
                RespObj = (List<RespGetEwayBillsForTranByState>)Session["EWayRespObj"];

                gvEWayBill.DataSource = RespObj;
                gvEWayBill.DataBind();
            }

            gvEWayBill.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
        //else
        //{
        //    SearchEWayBill();
        //}

    }
    #endregion
}