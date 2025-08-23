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
using System.Security.Cryptography;
using System.Threading;
public partial class CRM_Dashboard : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int FinYearId;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport_LeadDetail);
        ScriptManager1.RegisterPostBackControl(lnkCustOnBoardExport);
        ScriptManager1.RegisterPostBackControl(lnkTotVisit);
        ScriptManager1.RegisterPostBackControl(lnkTargetMonth);
        ScriptManager1.RegisterPostBackControl(lnkCustOnboardVolSummary);
        ScriptManager1.RegisterPostBackControl(lnkContractExpiry);
        ScriptManager1.RegisterPostBackControl(lnkFollowupDate);
        //Thread.Sleep(-10000);

        if (!IsPostBack)
        {
            // Bind Sales Person

            ddlSalesPerson.DataSourceID = "DataSourceSalesPerson";
            DataSourceSalesPerson.SelectParameters[0].DefaultValue = Session["UserId"].ToString();
            DataSourceSalesPerson.SelectParameters[1].DefaultValue = Session["FinYearId"].ToString();

            //ddlSalesPerson.DataBind();

            ddlMonth.SelectedValue = "0"; //Convert.ToString(DateTime.Now.Month);
            LoggedInUser.glFinYearId = Convert.ToInt32(Session["FinYearId"].ToString()); //added by SAYALI on 18-11-2019
        }
        else
        {
            DropDownList ddlTemp = Master.FindControl("ddFinYear") as DropDownList; //added by SAYALI on 16-11-2019
            LoggedInUser.glFinYearId = Convert.ToInt32(ddlTemp.SelectedValue);  //added by SAYALI on 16-11-2019
        }
        hdnFinYearId.Value = Convert.ToString(LoggedInUser.glFinYearId);
        GetLeadCount();

    }

    protected void GetLeadCount()
    {
        int CompanyId = 0, MonthId = 0;

        if (hdnCustId.Value != "" && hdnCustId.Value != "0")
            CompanyId = Convert.ToInt32(hdnCustId.Value);
        if (ddlMonth.SelectedValue != "0")
            MonthId = Convert.ToInt32(ddlMonth.SelectedValue);


        //DataSet dsGetLeadDetail = DBOperations.CRM_GetDashboardStagesCount(MonthId, CompanyId, LoggedInUser.glUserId, Convert.ToInt32(Session["FinYearId"]), Convert.ToInt32(ddlSalesPerson.SelectedValue));  comment by sayali 08-11-2019
        //if (dsGetLeadDetail.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < dsGetLeadDetail.Tables[0].Rows.Count; i++)
        //    {
        //        if (i == 0)
        //            lnkbtnLead.Text = "Lead (" + dsGetLeadDetail.Tables[0].Rows[i]["Count"].ToString() + ")"; //column name=Count
        //        if (i == 1)
        //            lnkbtnLeadApproved.Text = "Lead Approved (" + dsGetLeadDetail.Tables[0].Rows[i]["Count"].ToString() + ")";//column name=Count
        //        if (i == 2)
        //            lnkbtnQuote.Text = "Quote (" + dsGetLeadDetail.Tables[0].Rows[i]["Count"].ToString() + ")";//column name=Count
        //        if (i == 3)
        //            lnkbtnContractApproval.Text = "Contract Approval (" + dsGetLeadDetail.Tables[0].Rows[i]["Count"].ToString() + ")";//column name=Count
        //        if (i == 4)
        //            lnkbtnRejected.Text = "Rejected (" + dsGetLeadDetail.Tables[0].Rows[i]["Count"].ToString() + ")";//column name=Count
        //    }
        //}
        DataSet dsGetLeadDetail = DBOperations.GetCRMOperationCount(LoggedInUser.glUserId, LoggedInUser.glFinYearId);   //added by sayali 08-11-2019
        if (dsGetLeadDetail.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in dsGetLeadDetail.Tables[0].Rows)
            {
                lnkbtnLead.Text = "Lead (" + dsGetLeadDetail.Tables[0].Rows[0]["LEAD_COUNT"].ToString() + ")";
                lnkbtnLeadApproved.Text = "Lead Approved (" + dsGetLeadDetail.Tables[0].Rows[0]["LEAD_APPROVED"].ToString() + ")";
                lnkbtnQuote.Text = "Quote (" + dsGetLeadDetail.Tables[0].Rows[0]["PENDING_QUOTE"].ToString() + ")";
                lnkbtnContractApproval.Text = "Contract Approval (" + dsGetLeadDetail.Tables[0].Rows[0]["CONTRACT_APPOVED"].ToString() + ")";
                lnkbtnRejected.Text = "Rejected (" + dsGetLeadDetail.Tables[0].Rows[0]["LEAD_REJECTED"].ToString() + ")";//column name=Count
            }
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLeadCount();
    }

    protected void txtSearchCompany_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchCompany.Text.Trim() != "")
        {
            gvCustomerOnBoard.DataBind();
        }
        else
        {
            hdnCustId.Value = "0";
            gvCustomerOnBoard.DataBind();
        }
        GetLeadCount();
    }

    protected void lnkbtnLeadStatus_Click(object sender, EventArgs e)
    {
        lblPopupTitle.Text = "Lead Status Updated";
        mpeWhatsNew.Show();
        gvWhatsNewLog.DataSourceID = "DataSourceWhatsNew_LeadStatus";
        gvWhatsNewLog.DataBind();
    }

    protected void lnkbtnContractExpiry_Click(object sender, EventArgs e)
    {
        lblPopupTitle.Text = "3 months to go for Contract Expiry";
        mpeWhatsNew.Show();
        gvWhatsNewLog.DataSourceID = "DataSourceWhatsNew_Followup";
        gvWhatsNewLog.DataBind();
    }

    protected void lnkbtnUpcomingFollowup_Click(object sender, EventArgs e)
    {
        lblPopupTitle.Text = "New/Upcoming Due Date for Follow ups";
        mpeWhatsNew.Show();
        gvWhatsNewLog.DataSourceID = "DataSourceWhatsNew_Followup";
        gvWhatsNewLog.DataBind();
    }

    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeWhatsNew.Hide();
        gvCustomerOnBoard.DataBind();
        gvTotalVisits.DataBind();
    }

    protected void lnkbtnRejected_Click(object sender, EventArgs e)
    {
        gvLeadDetail.DataSource = null;
        gvLeadDetail.DataBind();
        hdnStageId.Value = "5";
        mpeLeadStatus.Show();
    }

    protected void lnkbtnContractApproval_Click(object sender, EventArgs e)
    {
        gvLeadDetail.DataSource = null;
        gvLeadDetail.DataBind();
        hdnStageId.Value = "4";
        mpeLeadStatus.Show();
    }

    protected void lnkbtnQuote_Click(object sender, EventArgs e)
    {
        gvLeadDetail.DataSource = null;
        gvLeadDetail.DataBind();
        hdnStageId.Value = "3";
        mpeLeadStatus.Show();
    }

    protected void lnkbtnLeadApproved_Click(object sender, EventArgs e)
    {
        gvLeadDetail.DataSource = null;
        gvLeadDetail.DataBind();
        hdnStageId.Value = "2";
        mpeLeadStatus.Show();
    }

    protected void lnkbtnLead_Click(object sender, EventArgs e)
    {
        gvLeadDetail.DataSource = null;
        gvLeadDetail.DataBind();
        hdnStageId.Value = "1";
        mpeLeadStatus.Show();
    }

    protected void ddlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvCustomerOnBoard.DataBind();
        gvTotalVisits.DataBind();
        GetLeadCount();
        gvOnBoardSummary.DataBind();
    }

    protected void imgClose_LeadDetail_Click(object sender, ImageClickEventArgs e)
    {
        mpeLeadStatus.Hide();
        hdnStageId.Value = "0";
    }

    protected void lnkExport_LeadDetail_Click(object sender, EventArgs e)
    {
        string strFileName = "SPWiseLeadStatusOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportStatusFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
        mpeLeadStatus.Show();
    }

    protected void ExportStatusFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvLeadDetail.AllowPaging = false;
        gvLeadDetail.AllowSorting = false;
        gvLeadDetail.Columns[0].Visible = false;
        gvLeadDetail.Enabled = false;
        gvLeadDetail.Caption = "Lead Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        gvLeadDetail.DataBind();
        gvLeadDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkCustOnBoardExport_Click(object sender, EventArgs e)
    {
        string strFileName = "CustomerOnBoard_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        PortExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void PortExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvCustomerOnBoard.AllowPaging = false;
        gvCustomerOnBoard.AllowSorting = false;
        gvCustomerOnBoard.Caption = "";

        gvCustomerOnBoard.DataSourceID = "DataSourceCustomerOnBoard";
        gvCustomerOnBoard.DataBind();

        //Remove Controls
       this.RemoveControls(gvCustomerOnBoard);

        gvCustomerOnBoard.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void RemoveControls(Control grid)
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
    protected void lnkTotVisit_Click(object sender, EventArgs e)
    {
        string strFileName = "TotalVisitList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        VisitExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void VisitExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvTotalVisits.AllowPaging = false;
        gvTotalVisits.AllowSorting = false;
        gvTotalVisits.Caption = "";

        gvTotalVisits.DataSourceID = "DataSourceVisits";
        gvTotalVisits.DataBind();

        //Remove Controls
        this.RemoveControls(gvTotalVisits);

        gvTotalVisits.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkTargetMonth_Click(object sender, EventArgs e)
    {
        string strFileName = "TargetMonthList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        TargetMonthExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void TargetMonthExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvTargetMonth.AllowPaging = false;
        gvTargetMonth.AllowSorting = false;
        gvTargetMonth.Caption = "";

        gvTargetMonth.DataSourceID = "DataSourceLead_TargetMonth";
        gvTargetMonth.DataBind();

        //Remove Controls
        this.RemoveControls(gvTargetMonth);

        gvTargetMonth.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkCustOnboardVolSummary_Click(object sender, EventArgs e)
    {
        string strFileName = "CustomerOnboardVolumnSummary_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        CustOnboardVolSummaryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void CustOnboardVolSummaryExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvOnBoardSummary.AllowPaging = false;
        gvOnBoardSummary.AllowSorting = false;
        gvOnBoardSummary.Caption = "";

        gvOnBoardSummary.DataSourceID = "SQLOnBoardSummary";
        gvOnBoardSummary.DataBind();

        //Remove Controls
        this.RemoveControls(gvOnBoardSummary);

        gvOnBoardSummary.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    protected void lnkContractExpiry_Click(object sender, EventArgs e)
    {
        string strFileName = "ContractExpiry_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ContractExpiryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ContractExpiryExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvContractExpiry.AllowPaging = false;
        gvContractExpiry.AllowSorting = false;
        gvContractExpiry.Caption = "";

        gvContractExpiry.DataSourceID = "DataSourceWhatsNew_Followup";
        gvContractExpiry.DataBind();

        //Remove Controls
        this.RemoveControls(gvContractExpiry);

        gvContractExpiry.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkFollowupDate_Click(object sender, EventArgs e)
    {
        string strFileName = "FollowUpDate_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        FolloupDateExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void FolloupDateExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvFollowupLead.AllowPaging = false;
        gvFollowupLead.AllowSorting = false;
        gvFollowupLead.Caption = "";

        gvFollowupLead.DataSourceID = "DataSourceLead_Followup";
        gvFollowupLead.DataBind();

        //Remove Controls
        this.RemoveControls(gvFollowupLead);

        gvFollowupLead.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
}