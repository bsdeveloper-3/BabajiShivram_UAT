using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class CRMReports_CustWiseLeadStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExport_LeadDetail);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Wise Lead Status";
            pnlFilter.Visible = false;

            ddlMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
            ddlMonth_SelectedIndexChanged(null, EventArgs.Empty);
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "CustWiseLeadStatus.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvLeads.DataBind();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvLeads.DataBind();
    }

    protected void gvLeads_PreRender(object sender, EventArgs e)
    {
        if (gvLeads.Rows.Count > 2)
        {
            gvLeads.Rows[gvLeads.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
            gvLeads.Rows[gvLeads.Rows.Count - 1].Cells[0].Text = "";
        }
    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeLeadStatus.Hide();
        hdnCustomerId.Value = "0";
        hdnStageId.Value = "0";
        lblStatusTitle.Text = "";
        gvLeadDetail.DataBind();
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "status_1")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "1";
                lblStatusTitle.Text = "Customer Wise Lead Status - MGMT Approval Pending";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_2")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "2";
                lblStatusTitle.Text = "Customer Wise Lead Status - Lead Approved";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_3")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "3";
                lblStatusTitle.Text = "Customer Wise Lead Status - Lead Rejected";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_4")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "4";
                lblStatusTitle.Text = "Customer Wise Lead Status - Follow Up";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_5")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "5";
                lblStatusTitle.Text = "Customer Wise Lead Status - Under Progress";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_6")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "6";
                lblStatusTitle.Text = "Customer Wise Lead Status - Quote Pending";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_7")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "7";
                lblStatusTitle.Text = "Customer Wise Lead Status - KYC Pending";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "status_8")
        {
            if (e.CommandArgument.ToString() != "")
            {
                hdnCustomerId.Value = e.CommandArgument.ToString();
                hdnStageId.Value = "8";
                lblStatusTitle.Text = "Customer Wise Lead Status - Contract Pending";
                mpeLeadStatus.Show();
                gvLeadDetail.DataBind();
            }
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "CustWiseLeadStatusOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeads.Caption = "Customer Wise Lead Status On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "CustWiseLeadStatus.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }


    protected void lnkExport_LeadDetail_Click(object sender, EventArgs e)
    {
        string strFileName = "CustWiseLeadStatusOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeadDetail.Caption = "Customer Wise Lead Status Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

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
            DataFilter1.FilterSessionID = "CustWiseLeadStatus.aspx";
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