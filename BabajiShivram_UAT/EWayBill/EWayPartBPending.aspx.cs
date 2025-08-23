using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
using Newtonsoft.Json;
public partial class EWayBill_EWayPartBPending : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Update Vehicle Number";
        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);

            Session["EwayPartB"] = null;

            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    private void GetEWayForTransport()
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
    private void GetEWayForTransportByGSTIN()
    {
        EWBSession EwbSession = new EWBSession(ddTrasnporter.SelectedValue);

        string strDate = DateTime.Now.ToString("dd/MM/yyyy");
        string strGSTIN = txtGSTIN.Text.Trim();

        if(strGSTIN == "")
        {
            strGSTIN = ddTrasnporter.SelectedValue;
        }

        if (txtDate.Text.Trim() != "")
        {
            strDate = txtDate.Text.Trim();
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
    protected void gvPendingPartB_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strBillNo = (string)e.CommandArgument;

            Session["EwayPartB"] = strBillNo;
            Session["userGSTIN"] = ddTrasnporter.SelectedValue;

            Response.Redirect("UpdatePartB.aspx");
        }
    }
    protected void btnMultiVehicle_Click(object sender, EventArgs e)
    {
        Session["EwayPartB"] = txtBillNumber.Text.Trim();
        Session["userGSTIN"] = ddTrasnporter.SelectedValue;

        Response.Redirect("UpdatePartB_Multi.aspx");

    }
    protected void btnShowBill_Click(object sender, EventArgs e)
    {
        if (rbCategory.SelectedValue == "EWBNO")
        {
            long EBillNo = 0;

            Int64.TryParse(txtBillNumber.Text.Trim(), out EBillNo);

            if (txtBillNumber.Text.Trim().Length == 12 && EBillNo > 0)
            {
                string strBillNo = txtBillNumber.Text.Trim();

                Session["EwayPartB"] = EBillNo;
                Session["userGSTIN"] = ddTrasnporter.SelectedValue;

                Response.Redirect("UpdatePartB.aspx");
            }
            else
            {
                lblErrorMsg.Text = "Invalid Bill Number! Please Enter 12 Digit Numeric Value.";
                lblErrorMsg.CssClass = "errorMsg";
            }
        }
        else if (rbCategory.SelectedValue == "GENDT")
        {
            GetEWayForTransport();
        }
        else if (rbCategory.SelectedValue == "GENTR")
        {
            GetEWayForTransportByGSTIN();
        }
    }

    protected void gvPendingPartB_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void rbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(rbCategory.SelectedItem.Value == "EWBNO")
        {
            pnlBillNo.Visible = true;
            
            pnlGenDate.Visible = false;
            pnlGenGSTN.Visible = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENDT")
        {
            pnlGenDate.Visible = true;

            pnlBillNo.Visible = false;
            pnlGenGSTN.Visible = false;
        }
        else if (rbCategory.SelectedItem.Value == "GENTR")
        {
            pnlGenGSTN.Visible = true;
            pnlGenDate.Visible = true;

            pnlBillNo.Visible = false;
        }
    }
}
