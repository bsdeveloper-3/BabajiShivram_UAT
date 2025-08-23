using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class CRM_Leads : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(btnNewLead);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        ScriptManager1.RegisterPostBackControl(btnAddFollowup);
        ScriptManager1.RegisterPostBackControl(btnAddEnquiry);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Lead";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Leads!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "Leads.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnNewLead_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddLead.aspx");
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            Response.Redirect("LeadStatus.aspx");
        }
        else if (e.CommandName.ToLower().Trim() == "followup")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnLeadId.Value = commandArgs[0].ToString();
            lblLeadNo.Text = commandArgs[1].ToString();
            mpeFollowup.Show();
            chkIsActive.Checked = true;
            gvFollowupHistory.DataBind();
        }
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LeadStageId")) == 10)  // rejected lead
            {
                e.Row.Cells[1].Text = "";
            }
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Leads Report On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeads.AllowPaging = false;
        gvLeads.AllowSorting = false;
        gvLeads.Columns[0].Visible = false;
        gvLeads.Columns[1].Visible = false;
        gvLeads.Columns[2].Visible = false;
        gvLeads.Columns[3].Visible = true;
        gvLeads.Enabled = false;
        gvLeads.Caption = "Leads Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Leads.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "Leads.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Followup Popup

    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeFollowup.Hide();
    }

    protected void btnAddFollowup_Click(object sender, EventArgs e)
    {
        if (txtFollowupDate.Text.Trim() != "")
        {
            bool IsActive = false;
            DateTime dtFollowup = DateTime.MinValue;
            DateTime dtTodaysDate = DateTime.MinValue;
            dtFollowup = Commonfunctions.CDateTime(txtFollowupDate.Text.Trim());
            dtTodaysDate = Commonfunctions.CDateTime(DateTime.Now.ToString("dd/MM/yyyy"));

            if (dtFollowup <= dtTodaysDate)
            {
                lblError_Followup.Text = "Follow up date should be greater than today's date.";
                lblError_Followup.CssClass = "errorMsg";
                mpeFollowup.Show();
                return;
            }

            if (chkIsActive.Checked)
                IsActive = true;

            int result = DBOperations.CRM_AddFollowupHistory(Convert.ToInt32(hdnLeadId.Value), dtFollowup, IsActive, txtRemark.Text.Trim(), loggedInUser.glUserId);
            if (result == 0)
            {
                lblError_Followup.Text = "Followup added successfully.";
                lblError_Followup.CssClass = "success";
                mpeFollowup.Show();
                gvFollowupHistory.DataBind();
                txtFollowupDate.Text = "";
                txtRemark.Text.Trim();
                chkIsActive.Checked = true;
            }
            else if (result == 2)
            {
                lblError_Followup.Text = "Followup already exists for this lead!";
                lblError_Followup.CssClass = "errorMsg";
                mpeFollowup.Show();
                gvFollowupHistory.DataBind();
            }
            else
            {
                lblError_Followup.Text = "System error! Please try again later.";
                lblError_Followup.CssClass = "errorMsg";
                mpeFollowup.Show();
                gvFollowupHistory.DataBind();
            }
        }
        else
        {
            lblError_Followup.Text = "Please enter followup date.";
            lblError_Followup.CssClass = "errorMsg";
            mpeFollowup.Show();
            gvFollowupHistory.DataBind();
        }
    }

    #endregion

    #region Customer Enquiry

    protected void btnAddEnquiry_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustEnquiry.aspx");
    }

    #endregion
}
