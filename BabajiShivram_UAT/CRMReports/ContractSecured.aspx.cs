using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class CRMReports_ContractSecured : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        ScriptManager1.RegisterPostBackControl(lnkExport_LeadDetail);
        if (!IsPostBack)
        {
            pnlFilter.Visible = false;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Contract Secured";
        }
        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "ContractSecured.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvLeads.DataBind();
    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeLeadStatus.Hide();
        hdnMonthId.Value = "0";
        hdnYear.Value = "0";
        lblStatusTitle.Text = "";
        gvLeadDetail.DataBind();
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "month1")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "1";
                lblStatusTitle.Text = "Contract Secured Detail Report - January";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month2")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "2";
                lblStatusTitle.Text = "Contract Secured Detail Report - February";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month3")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "3";
                lblStatusTitle.Text = "Contract Secured Detail Report - March";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month4")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "4";
                lblStatusTitle.Text = "Contract Secured Detail Report - April";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month5")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "5";
                lblStatusTitle.Text = "Contract Secured Detail Report - May";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month6")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "6";
                lblStatusTitle.Text = "Contract Secured Detail Report - June";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month7")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "7";
                lblStatusTitle.Text = "Contract Secured Detail Report - July";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month8")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "8";
                lblStatusTitle.Text = "Contract Secured Detail Report - August";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month9")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "9";
                lblStatusTitle.Text = "Contract Secured Detail Report - September";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month10")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "10";
                lblStatusTitle.Text = "Contract Secured Detail Report - October";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month11")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "11";
                lblStatusTitle.Text = "Contract Secured Detail Report - November";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "month12")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnYear.Value = e.CommandArgument.ToString();
                hdnMonthId.Value = "12";
                lblStatusTitle.Text = "Contract Secured Detail Report - December";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "ContractSecuredOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvLeads.AllowPaging = false;
        gvLeads.AllowSorting = false;
        gvLeads.Columns[0].Visible = false;
        gvLeads.Enabled = false;
        gvLeads.Caption = "Contract Secured On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "ContractSecured.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }


    protected void lnkExport_LeadDetail_Click(object sender, EventArgs e)
    {
        string strFileName = "ContractSecuredDetailOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeadDetail.Caption = "Contract Secured Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        gvLeadDetail.DataBind();
        gvLeadDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "ContractSecured.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}