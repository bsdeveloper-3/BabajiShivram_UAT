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

public partial class ExportCHA_ExportDashboard : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
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
        //   ScriptManager1.RegisterPostBackControl(lnkBillPendinglist);
        ScriptManager1.RegisterPostBackControl(lnkStageWisePendinglist);
        ScriptManager1.RegisterPostBackControl(lnkPriorityWiseList);
        ScriptManager1.RegisterPostBackControl(lnkStageDetail);
        ScriptManager1.RegisterPostBackControl(grvPopupForSagePending);
        ScriptManager1.RegisterPostBackControl(lnkbtnJobOpeningExport);
        ScriptManager1.RegisterPostBackControl(lnkPriorityClearenace);
        ScriptManager1.RegisterPostBackControl(gvPendingJob);
        ScriptManager1.RegisterPostBackControl(lnkCurrentAmtXls);
        ScriptManager1.RegisterPostBackControl(lnkShipmentCleared);
        ScriptManager1.RegisterPostBackControl(lnkUnderClearance);

    }
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomer.Text.Trim() == "")
            hdnCustId.Value = "0";

        if (txtBranch.Text.Trim() == "")
            hdnBranchId.Value = "0";
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
        tbShipmentGetInDetails.Style.Add("display", "Block");
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
        tbShipmentGetInDetails.Style.Add("display", "Block");
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

    #region Job Opening

    protected void gvJobOpening_PreRender(Object Sender, EventArgs e)
    {
        // if (gvJobOpening.Rows.Count > 0)
        // gvJobOpening.Rows[gvJobOpening.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");  //"#CBCBDC";
    }

    protected void DataSourcePortwise_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = true;
    }

    protected void gvJobOpening_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text != null && e.Row.Cells[2].Text != "")
            {
                Decimal TotalOpenBal = Convert.ToDecimal(e.Row.Cells[2].Text);
                ViewState["OpenBal"] = Convert.ToDecimal(ViewState["OpenBal"]) + TotalOpenBal;
            }
            if (e.Row.Cells[3].Text != null && e.Row.Cells[3].Text != "")
            {
                Decimal TotalNewFile = Convert.ToDecimal(e.Row.Cells[3].Text);
                ViewState["NewFile"] = Convert.ToDecimal(ViewState["NewFile"]) + TotalNewFile;
            }
            if (e.Row.Cells[4].Text != null && e.Row.Cells[4].Text != "")
            {
                Decimal TotalCompleteToday = Convert.ToDecimal(e.Row.Cells[4].Text);
                ViewState["CompleteToday"] = Convert.ToDecimal(ViewState["CompleteToday"]) + TotalCompleteToday;
            }
            if (e.Row.Cells[5].Text != null && e.Row.Cells[5].Text != "")
            {
                Decimal TotalCloseBALy = Convert.ToDecimal(e.Row.Cells[5].Text);
                ViewState["CloseBAL"] = Convert.ToDecimal(ViewState["CloseBAL"]) + TotalCloseBALy;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "";
            e.Row.Cells[1].Text = "<b>Total</b>";
            e.Row.Cells[1].ColumnSpan = 1;
            e.Row.Cells[2].Text = "<b>" + ViewState["OpenBal"].ToString() + "</b>";
            e.Row.Cells[3].Text = "<b>" + ViewState["NewFile"].ToString() + "</b>";
            e.Row.Cells[4].Text = "<b>" + ViewState["CompleteToday"].ToString() + "</b>";
            e.Row.Cells[5].Text = "<b>" + ViewState["CloseBAL"].ToString() + "</b>";
        }
    }

    protected void lnkbtnJobOpeningExport_Click(object sender, EventArgs e)
    {
        string strFileName = "JobOpeningList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        JobOpeningExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void JobOpeningExport(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobOpening.AllowPaging = false;
        gvJobOpening.AllowSorting = false;
        gvJobOpening.Caption = "";
        gvJobOpening.DataSourceID = "SqlDataSourceJobOpening";
        gvJobOpening.DataBind();

        //Remove Controls
        this.RemoveControls(gvJobOpening);
        gvJobOpening.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region No. of Job Pending - Stage Wise

    protected void gvPendingJob_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "show")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            Label lblstatus = (Label)gvrow.FindControl("lblStatus");
            lblTitle.Text = "Pending " + " " + gvrow.Cells[1].Text + " " + "Details";//lblstatus.Text;
            int Status = Convert.ToInt32(lblstatus.Text.Trim());
            int FinYearId = Convert.ToInt32(Session["FinYearId"]);

            DataSet dsStatusJob = EXOperations.GetPendingStageWsJobDetails(FinYearId, Status, Convert.ToInt16(Session["UserId"]));

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
            Response.Redirect("EXJobDetail.aspx");
        }
    }

    protected void lnkStageWisePendinglist_Click(object sender, EventArgs e)
    {
        string strFileName = "Detail Of Stage Wise Pending List_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        StagePendinglistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void StagePendinglistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvPendingJob.AllowPaging = false;
        gvPendingJob.AllowSorting = false;
        gvPendingJob.Caption = "";
        gvPendingJob.DataSourceID = "DataSourcePendingDeptWise";
        gvPendingJob.DataBind();

        //Remove Controls
        this.RemoveControls(gvPendingJob);
        gvPendingJob.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkStageDetail_Click(object sender, EventArgs e)
    {
        string strFileName = lblTitle.Text + " " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        PendingPerticularStageExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void PendingPerticularStageExport(string header, string contentType)
    {
        DataSet dsStage = ViewState["StagePending"] as DataSet;
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        grvPopupForSagePending.AllowPaging = false;
        grvPopupForSagePending.AllowSorting = false;
        grvPopupForSagePending.Caption = "";
        grvPopupForSagePending.DataSource = dsStage;
        grvPopupForSagePending.DataBind();
        //Remove Controls
        this.RemoveControls(grvPopupForSagePending);
        grvPopupForSagePending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

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

    protected void lnkPriorityClearenace_Click(object sender, EventArgs e)
    {
        string strFileName = "SummaryofBillPending_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        PriorityClearenaceExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void PriorityClearenaceExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvDispatchdays.AllowPaging = false;
        gvDispatchdays.AllowSorting = false;
        gvDispatchdays.Caption = "";
        gvDispatchdays.Columns[1].Visible = true;
        gvDispatchdays.Columns[0].Visible = false;
        gvDispatchdays.DataSourceID = "sqlDispatchdays";
        gvDispatchdays.DataBind();
        //Remove Controls
        this.RemoveControls(gvDispatchdays);
        gvDispatchdays.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvDispatchdays_PreRender(Object Sender, EventArgs e)
    {

        if (gvDispatchdays.Rows.Count > 0)
            gvDispatchdays.Rows[gvDispatchdays.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
        else
            lblDispatchdays.Text = "<br/><br/><br/><b>No Record Available</b>";
        lblDispatchdays.CssClass = "errorMsg";
    }

    protected void gvDispatchdays_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "showjob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            Session["Status"] = "";
            Response.Redirect("EXJobDetail.aspx?ActiveTab=BillingDetail");
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
        tbShipmentGetInDetails.Style.Add("display", "none");
        mpeShipmentGetIn.Hide();
    }

    protected void gvShipmentDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShipmentJob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = e.CommandArgument.ToString();
            Session["JobId"] = strJobId;
            Session["Status"] = "";
            Response.Redirect("EXJobDetail.aspx");
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
        tbShipmentGetInDetails.Style.Add("display", "none");
        ModalPopupExtender2.Hide();
    }

    protected void gvsummarylist_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "BillJob")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            Response.Redirect("EXJobDetail.aspx");
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

            //string strJobId = (string)e.CommandArgument;
            //string[] words = strJobId.Split(',');
            //string jobid = words[0].ToUpperInvariant();
            //string status = words[1].ToString();

            //Session["JobId"] = jobid;
            //Session["Status"] = status;


            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = (string)e.CommandArgument;

            string[] words = strJobId.Split(',');
            string jobid = words[0].ToUpperInvariant();
            string status = words[1].ToString();

            Session["JobId"] = jobid;
            Session["Status"] = status;

            // Session["JobId"] = strJobId;
            Response.Redirect("EXJobDetail.aspx");
        }
    }

    #endregion

    protected void lnkCurrentAmtXls_Click(object sender, EventArgs e)
    {
        string strFileName = "CurrentAmtGreaterthan1lac_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        CurrentAmtGreaterthan1LacExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkShipmentCleared_Click(object sender, EventArgs e)
    {
        string strFileName = "ShipmentCleared_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ShipmentClearedExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void CurrentAmtGreaterthan1LacExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Caption = "";

        gvJobDetail.DataSourceID = "FASqlDataSource";
        gvJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvJobDetail);

        gvJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void ShipmentClearedExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvShipmentCleared.AllowPaging = false;
        gvShipmentCleared.AllowSorting = false;
        gvShipmentCleared.Caption = "";

        gvShipmentCleared.DataSourceID = "SqlDataPendingList";
        gvShipmentCleared.DataBind();

        //Remove Controls
        this.RemoveControls(gvShipmentCleared);

        gvShipmentCleared.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // For Live Tracking Status
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strJobNo = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ref"));

            if (strJobNo != "") // Duty Payment Completed
            {
                DataSet dsStatus = DBOperations.GetJobStatusForDashboard(strJobNo);

                if (dsStatus.Tables.Count > 0)
                {
                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        //e.Row.BackColor = System.Drawing.Color.FromName("#8585f7");

                        Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                        Label lblOOCDate = (Label)e.Row.FindControl("lblOOCDate");
                        Label lblDispatchDate = (Label)e.Row.FindControl("lblDispatchDate");
                        Label lblKAM = (Label)e.Row.FindControl("lblKAM");
                        Label lblAdvanceAmount = (Label)e.Row.FindControl("lblAdvanceAmount");

                        if (lblStatus != null)
                        {
                            lblStatus.Text = dsStatus.Tables[0].Rows[0]["JobStatus"].ToString();
                        }

                        if (lblOOCDate != null)
                        {
                            if (dsStatus.Tables[0].Rows[0]["OOCDate"] != DBNull.Value)
                            {
                                lblOOCDate.Text = dsStatus.Tables[0].Rows[0]["OOCDate"].ToString();
                            }
                        }
                        if (lblDispatchDate != null)
                        {
                            if (dsStatus.Tables[0].Rows[0]["DispatchDate"] != DBNull.Value)
                            {
                                lblDispatchDate.Text = dsStatus.Tables[0].Rows[0]["DispatchDate"].ToString();
                            }
                        }
                        if (lblKAM != null)
                        {
                            lblKAM.Text = dsStatus.Tables[0].Rows[0]["KAM"].ToString();
                        }
                    }
                }//END_TableCount
            }//END_JobNo
        }//END_DataRow
    }

    protected void lnkUnderClearance_Click(object sender, EventArgs e)
    {
        string strFileName = "UnderClearance_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        UnderClearanceExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void UnderClearanceExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvUnderClearance.AllowPaging = false;
        gvUnderClearance.AllowSorting = false;
        gvUnderClearance.Caption = "";

        gvUnderClearance.DataSourceID = "SqlDataSourceUnderClearance";
        gvUnderClearance.DataBind();

        //Remove Controls
        this.RemoveControls(gvUnderClearance);

        gvUnderClearance.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
}

//protected void gvDispatchWeek_PreRender(Object Sender, EventArgs e)
//{
//    if (gvDispatchWeek.Rows.Count > 0)
//        gvDispatchWeek.Rows[gvDispatchWeek.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
//    else
//        lblDispatchWeek.Text = "<br/><br/><br/><b>No Record Available</b>";
//    lblDispatchWeek.CssClass = "errorMsg";
//}
//protected void gvhighcreditdays_PreRender(Object Sender, EventArgs e)
//{
//    if (gvhighcreditdays.Rows.Count > 0)
//        gvhighcreditdays.Rows[gvhighcreditdays.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
//    else
//        lblhighcreditdays.Text = "<br/><br/><br/><b>No Record Available</b>";
//    lblhighcreditdays.CssClass = "errorMsg";
//}
//protected void gvmorethan3days_PreRender(Object Sender, EventArgs e)
//{
//    if (gvmorethan3days.Rows.Count <= 0)
//        lblmorethan1days.Text = "<br/><br/><br/><b>No Record Available</b>";
//    //  gvmorethan3days.Rows[gvmorethan3days.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");

//    lblmorethan3days.CssClass = "errorMsg";
//}
//protected void gvuserKeepmorethan1days_PreRender(Object Sender, EventArgs e)
//{
//    if (gvuserKeepmorethan1days.Rows.Count <= 0)
//        lblmorethan1days.Text = "<br/><br/><br/><b>No Record Available</b>";
//    //gvuserKeepmorethan1days.Rows[gvuserKeepmorethan1days.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");


//    lblmorethan1days.CssClass = "errorMsg";
//}

//private void PriorityClearenaceExport(string header, string contentType)
//{
//    if (Drpdashboard.SelectedIndex != 0)
//    {

//        Response.Clear();
//        Response.Buffer = true;
//        Response.AddHeader("content-disposition", header);
//        Response.Charset = "";
//        this.EnableViewState = false;
//        Response.ContentType = contentType;
//        StringWriter sw = new StringWriter();
//        HtmlTextWriter hw = new HtmlTextWriter(sw);


//        if (Drpdashboard.SelectedIndex == 1)
//        {
//            gvmorethan3days.AllowPaging = false;
//            gvmorethan3days.AllowSorting = false;
//            gvmorethan3days.Caption = "";

//            gvmorethan3days.DataSourceID = "sqlmorethan3days";
//            gvmorethan3days.DataBind();

//            //Remove Controls
//            this.RemoveControls(gvmorethan3days);

//            gvmorethan3days.RenderControl(hw);
//        }

//        else if (Drpdashboard.SelectedIndex == 2)
//        {
//            gvuserKeepmorethan1days.AllowPaging = false;
//            gvuserKeepmorethan1days.AllowSorting = false;
//            gvuserKeepmorethan1days.Caption = "";

//            gvuserKeepmorethan1days.DataSourceID = "sqluserkeepfilemorethan1days";
//            gvuserKeepmorethan1days.DataBind();

//            //Remove Controls
//            this.RemoveControls(gvuserKeepmorethan1days);

//            gvuserKeepmorethan1days.RenderControl(hw);
//        }
//        Response.Output.Write(sw.ToString());
//        Response.End();
//    }


//}
//protected void lnkNoOfStagePending_Click(object sender, EventArgs e)
//{
//    tblStagePending.Style.Add("display", "Block");
//    GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
//    string Stage = clickedRow.Cells[0].Text;
//    if (Stage.Trim().ToLower() == "shipment get in")
//        Stage = "1";
//    else if (Stage.Trim().ToLower() == "billing advice")
//        Stage = "2";
//    else
//        Stage = "3";
//    DataSourceShipmentGetIn.SelectParameters["Stage"].DefaultValue = Stage.ToString();
//    mpeShipmentGetIn.Show();
//    gvShipmentDetails.DataSource = DataSourceShipmentGetIn;
//    gvShipmentDetails.DataBind();
//}
//protected void Drpdashboard_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (Drpdashboard.SelectedValue == "0")
//    {
//        if (gvDispatchdays.Rows.Count > 0)
//        {
//            lblDispatchdays.Text = "";
//        }
//        tbl1.Attributes.Add("style", "display:block;");
//        tbl2.Attributes.Add("style", "display:none;");
//        tbl3.Attributes.Add("style", "display:none;");
//        tbl4.Attributes.Add("style", "display:none;");
//        tbl7.Attributes.Add("style", "display:none;");
//        lblDispatchdays.Attributes.Add("style", "display:block;");
//        lblDispatchWeek.Attributes.Add("style", "display:none;");
//        lblhighcreditdays.Attributes.Add("style", "display:none;");
//        lblmorethan3days.Attributes.Add("style", "display:none;");
//        lblmorethan1days.Attributes.Add("style", "display:none;");
//    }
//    else if (Drpdashboard.SelectedValue == "1")
//    {
//        if (gvDispatchWeek.Rows.Count > 0)
//        {
//            lblDispatchWeek.Text = "";
//        }
//        tbl1.Attributes.Add("style", "display:none;");
//        tbl2.Attributes.Add("style", "display:block;");
//        tbl3.Attributes.Add("style", "display:none;");
//        tbl4.Attributes.Add("style", "display:none;");
//        tbl7.Attributes.Add("style", "display:none;");
//        lblDispatchdays.Attributes.Add("style", "display:none;");
//        lblDispatchWeek.Attributes.Add("style", "display:block;");
//        lblhighcreditdays.Attributes.Add("style", "display:none;");
//        lblmorethan3days.Attributes.Add("style", "display:none;");
//        lblmorethan1days.Attributes.Add("style", "display:none;");
//    }
//    else if (Drpdashboard.SelectedValue == "2")
//    {
//        if (gvhighcreditdays.Rows.Count > 0)
//        {
//            lblhighcreditdays.Text = "";
//        }
//        tbl1.Attributes.Add("style", "display:none;");
//        tbl2.Attributes.Add("style", "display:none;");
//        tbl3.Attributes.Add("style", "display:block;");
//        tbl4.Attributes.Add("style", "display:none;");
//        tbl7.Attributes.Add("style", "display:none;");
//        lblDispatchdays.Attributes.Add("style", "display:none;");
//        lblDispatchWeek.Attributes.Add("style", "display:none;");
//        lblhighcreditdays.Attributes.Add("style", "display:block;");
//        lblmorethan3days.Attributes.Add("style", "display:none;");
//        lblmorethan1days.Attributes.Add("style", "display:none;");
//    }
//    else if (Drpdashboard.SelectedValue == "3")
//    {

//        if (gvmorethan3days.Rows.Count > 0)
//        {
//            lblmorethan3days.Text = "";
//        }
//        tbl1.Attributes.Add("style", "display:none;");
//        tbl2.Attributes.Add("style", "display:none;");
//        tbl3.Attributes.Add("style", "display:none;");
//        tbl4.Attributes.Add("style", "display:block;");
//        tbl7.Attributes.Add("style", "display:none;");
//        lblDispatchdays.Attributes.Add("style", "display:none;");
//        lblDispatchWeek.Attributes.Add("style", "display:none;");
//        lblhighcreditdays.Attributes.Add("style", "display:none;");
//        lblmorethan3days.Attributes.Add("style", "display:block;");
//        lblmorethan1days.Attributes.Add("style", "display:none;");
//    }

//    else
//    {
//        if (gvuserKeepmorethan1days.Rows.Count > 0)
//        {
//            lblmorethan1days.Text = "";
//        }
//        tbl1.Attributes.Add("style", "display:none;");
//        tbl2.Attributes.Add("style", "display:none;");
//        tbl3.Attributes.Add("style", "display:none;");
//        tbl4.Attributes.Add("style", "display:none;");
//        tbl7.Attributes.Add("style", "display:block;");
//        lblDispatchdays.Attributes.Add("style", "display:none;");
//        lblDispatchWeek.Attributes.Add("style", "display:none;");
//        lblhighcreditdays.Attributes.Add("style", "display:none;");
//        lblmorethan3days.Attributes.Add("style", "display:none;");
//        lblmorethan1days.Attributes.Add("style", "display:block;");
//    }
//}