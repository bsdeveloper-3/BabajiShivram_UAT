using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

public partial class CustomerExport_CustomerExportDashboard : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    public string imgPath, imgLogout, LoginPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["lbljobno"] = 0;
        ViewState["lblCreditAMT"] = 0;
        ViewState["lbldebitAMT"] = 0;

        ViewState["lbljobno1"] = 0;
        ViewState["lblCreditAMT1"] = 0;
        ViewState["lbldebitAMT1"] = 0;
        ViewState["OpenBal"] = 0;
        ViewState["NewFile"] = 0;
        ViewState["CompleteToday"] = 0;
        ViewState["CloseBAL"] = 0;

        ScriptManager1.RegisterPostBackControl(lnkExportShipmentDetails);
        ScriptManager1.RegisterPostBackControl(lnkjobSummarylist);
        ScriptManager1.RegisterPostBackControl(lnkShipmentClearancePending);
        
        ScriptManager1.RegisterPostBackControl(lnkPriorityWiseList);
        
    }
    protected void DataSourceAgeingDays_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = true;
    }
    protected void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)              
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    #region SHIPMENT GET IN

    protected void lnkNoOfJobs_Click(object sender, EventArgs e)
    {        
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

        string Stage = clickedRow.Cells[0].Text;

        lblbillingpendingFiles.Text = "Files Pending For Billing In -" + " " + Stage.Trim();

        if (Stage.Trim().ToLower() == "shipment get in")
            Stage = "1";
        else if (Stage.Trim().ToLower() == "billing advice")
            Stage = "2";
        else
            Stage = "3";

        DataSourceShipmentGetIn.SelectParameters["Stage"].DefaultValue = Stage.ToString();
        mpeShipmentGetIn.Show();
        gvShipmentDetails.DataSource = DataSourceShipmentGetIn;
        gvShipmentDetails.DataBind();
    }
    protected void lnkShipmentClearancePending_Click(object sender, EventArgs e)
    {
        string strFileName = "FilesPendingForBilling_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ShipmentGetInPendingExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    private void ShipmentGetInPendingExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvShipmentGetInPending.AllowPaging = false;
        gvShipmentGetInPending.AllowSorting = false;
        gvShipmentGetInPending.Caption = "";
        gvShipmentGetInPending.DataSourceID = "SQlDataSourceForShipmentGetIn";
        gvShipmentGetInPending.DataBind();

        this.RemoveControls(gvShipmentGetInPending);//Remove Controls
        gvShipmentGetInPending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region BILL PENDING LISTS

    protected void gvBillPendinglist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkNoOfBillPending = (LinkButton)e.Row.Cells[1].FindControl("lnkNoOfBillPending");
            if (lnkNoOfBillPending != null)
            {
                Decimal TotalNoOfJobs = Convert.ToDecimal(lnkNoOfBillPending.Text);
                ViewState["lbljobno1"] = Convert.ToDecimal(ViewState["lbljobno1"]) + TotalNoOfJobs;
            }
            if (e.Row.Cells[2].Text == "&nbsp;")
                e.Row.Cells[2].Text = "0";

            Decimal lbldebitAMT1 = Convert.ToDecimal(e.Row.Cells[3].Text);
            ViewState["lbldebitAMT1"] = Convert.ToDecimal(ViewState["lbldebitAMT1"]) + Convert.ToDecimal(lbldebitAMT1);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[1].Text = "<b>" + ViewState["lbljobno1"].ToString() + "</b>";
            //e.Row.Cells[2].Text = "<b>" + ViewState["lblCreditAMT1"].ToString() + "</b>";
            e.Row.Cells[3].Text = "<b>" + Convert.ToDecimal(ViewState["lbldebitAMT1"]).ToString("#,##0.00") + "</b>";
        }
    }

    protected void lnkNoOfBillPending_Click(object sender, EventArgs e)
    {        
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

        string Stage = clickedRow.Cells[0].Text;
        lblBillpendList.Text = "Summary Of Bill Pending List -" + " " + Stage.Trim();

        if (Stage.Trim().ToLower() == "shipment get in")
        {
            Stage = "1";
        }
        else if (Stage.Trim().ToLower() == "shipment pending")
        {
            Stage = "2";
        }
        else
        {
            Stage = "3";
        }

        DsSummarylist.SelectParameters["Stage"].DefaultValue = Stage.ToString();
        ModalPopupExtender2.Show();
        gvsummarylist.DataSource = DsSummarylist;
        gvsummarylist.DataBind();
    }

    protected void lnkBillPendinglist_Click(object sender, EventArgs e)
    {
        string strFileName = "Summary Of Bill Pending List_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        // PendinglistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    //private void PendinglistExport(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    gvBillPendinglist.AllowPaging = false;
    //    gvBillPendinglist.AllowSorting = false;
    //    gvBillPendinglist.Caption = "";
    //    gvBillPendinglist.DataSourceID = "DataSourceBillPendingList";
    //    gvBillPendinglist.DataBind();

    //    //Remove Controls
    //    this.RemoveControls(gvBillPendinglist);
    //    gvBillPendinglist.RenderControl(hw);
    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}

    #endregion
        
    #region Total No. Of Files Pending For Billing

    protected void gvShipmentGetInPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkNoOfJobs = (LinkButton)e.Row.Cells[1].FindControl("lnkNoOfJobs");
            if (lnkNoOfJobs.Text != "")
                ViewState["lbljobno"] = Convert.ToDecimal(ViewState["lbljobno"]) + Convert.ToDecimal(lnkNoOfJobs.Text);

            Decimal lbldebitAMT = Convert.ToDecimal(e.Row.Cells[3].Text);
            ViewState["lbldebitAMT"] = Convert.ToDecimal(ViewState["lbldebitAMT"]) + Convert.ToDecimal(lbldebitAMT);
        }

        if (e.Row.Cells[2].Text == "&nbsp;")
            e.Row.Cells[2].Text = "0";

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[1].Text = "<b>" + ViewState["lbljobno"].ToString() + "</b>";
            //e.Row.Cells[2].Text = "<b>" + ViewState["lblCreditAMT"].ToString() + "</b>";
            e.Row.Cells[3].Text = "<b>" + Convert.ToDecimal(ViewState["lbldebitAMT"]).ToString("#,##0.00") + "</b>";

        }
    }

    #endregion

    #region Summary Of Bill Pending List
    
    protected void gvDispatchdays_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "showjob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            
            Response.Redirect("CustomerEXJobDetail.aspx");
        }
    }

    protected void lnkExportShipmentDetails_Click(object sender, EventArgs e)
    {
        string strFileName = lblbillingpendingFiles.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportShipmentGetInDetails("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void ExportShipmentGetInDetails(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvShipmentDetails.AllowPaging = false;
        gvShipmentDetails.AllowSorting = false;
        gvShipmentDetails.Caption = "";
        gvShipmentDetails.DataSource = DataSourceShipmentGetIn;
        gvShipmentDetails.DataBind();

        this.RemoveControls(gvShipmentDetails);  //Remove Controls
        gvShipmentDetails.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        mpeShipmentGetIn.Hide();
    }

    protected void gvShipmentDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShipmentJob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = e.CommandArgument.ToString();
            Session["JobId"] = strJobId;
            
            Response.Redirect("CustomerEXJobDetail.aspx");
        }
    }

    protected void lnkjobSummarylist_Click(object sender, EventArgs e)
    {
        string strFileName = lblBillpendList.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        jobSummarylistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void jobSummarylistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvsummarylist.AllowPaging = false;
        gvsummarylist.AllowSorting = false;
        gvsummarylist.Caption = "";
        gvsummarylist.DataSourceID = "DsSummarylist";
        gvsummarylist.DataBind();

        this.RemoveControls(gvsummarylist); //Remove Controls
        gvsummarylist.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void btnCancelPopup_Click1(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    protected void gvsummarylist_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "BillJob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            Response.Redirect("CustomerEXJobDetail.aspx");
        }
    }

    #endregion

    #region Priority Shipment

    protected void lnkPriorityWiseList_Click(object sender, EventArgs e)
    {
        string strFileName = "Priority Shipment List_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        PrioritylistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void PrioritylistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvPriorityShipments.AllowPaging = false;
        gvPriorityShipments.AllowSorting = false;
        gvPriorityShipments.Caption = "";
        gvPriorityShipments.DataSourceID = "DataSourcePriorityShipment";
        gvPriorityShipments.DataBind();

        //Remove Controls
        this.RemoveControls(gvPriorityShipments);
        gvPriorityShipments.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvPriorityShipments_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "PriorityShipmentJob")
        {            
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;

            string[] words = strJobId.Split(',');
            string jobid = words[0].ToUpperInvariant();
            string status = words[1].ToString();

            Session["JobId"] = jobid;

            Response.Redirect("CustomerEXJobDetail.aspx");
        }
    }

    #endregion


    protected void gvPendingJob_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "show")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            Label lblstatus = (Label)gvrow.FindControl("lblStatus");
            lblTitle.Text = "Pending " + " " + gvrow.Cells[1].Text + " " + "Details";//lblstatus.Text;
            int Status = Convert.ToInt32(lblstatus.Text.Trim());
            int FinYearId = Convert.ToInt32(Session["FinYearId"]);

            DataSet dsStatusJob = DBOperations.GetPendingStageWsCustJobdetail(FinYearId, Status, Convert.ToInt16(Session["CustUserId"]));

            if (dsStatusJob.Tables[0].Rows.Count > 0)
            {
                ViewState["StagePending"] = dsStatusJob;
                grvPopupForSagePending.DataSource = dsStatusJob;
                grvPopupForSagePending.DataBind();
                ModalPopupExtender3.Show();
            }
        }
    }
    protected void gvPendingJob_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPendingJob = (Label)e.Row.Cells[2].FindControl("lblPendingJob");
            LinkButton lnkNoOfStagePending = (LinkButton)e.Row.Cells[3].FindControl("lnkNoOfStagePending");
            if (lblPendingJob != null)
            {
                if (lblPendingJob.Text.Trim().ToString() == "0")
                {
                    lnkNoOfStagePending.Enabled = false;
                }
                else
                {
                    lnkNoOfStagePending.Enabled = true;
                }
            }
        }
    }
    protected void btnCancelPopup1_Click(object sender, EventArgs e)
    {
        tbShipmentGetInDetails.Style.Add("display", "none");
        ModalPopupExtender2.Hide();
    }

    protected void grvPopupForSagePending_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RedirectJob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            string strJobId = commandArgs[0].ToString();
            string status = commandArgs[1].ToString();
            Session["JobId"] = strJobId;
            Session["Status"] = status;
            //Response.Redirect("EXJobDetail.aspx");
        }
    }
}

