using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class CRM_Converted : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        ScriptManager1.RegisterPostBackControl(btnSaveStatus);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Lead - Active";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Lead Approved!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "Converted.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["LeadId"] = commandArgs[0].ToString();
            string RfqReceived = commandArgs[1].ToString();

            if (RfqReceived.ToLower().Trim() == "yes") // is RFQ received
            {
                Response.Redirect("AddEnquiry.aspx");
            }
            else
            {
                Response.Redirect("ApprovedLead.aspx");
            }
        }
        else if (e.CommandName.ToLower().Trim() == "followup")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnLeadId_ForStatus.Value = commandArgs[0].ToString();
            lblLeadNo_ForStatus.Text = commandArgs[1].ToString();
            mpeFollowup.Show();
            mpeLeadStatus.Hide();
            chkIsActive.Checked = true;
            gvFollowupHistory.DataBind();
            txtFollowupDate.Text = "";
            txtRemark.Text = "";
        }
        else if (e.CommandName.ToLower().Trim() == "status")
        {
            ddlStatus.SelectedIndex = -1;
            txtCloseDate.Text = "";
            tdDate.Visible = false;
            txtCloseDate.Visible = false;
            ddlStatus.DataBind();
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnLeadId_ForStatus.Value = commandArgs[0].ToString();
            lblLeadNo_ForStatus.Text = commandArgs[1].ToString();
            string Services = commandArgs[2].ToString();
            if (Services == "")
            {
                ddlStatus.Items[3].Attributes.Add("style", "display:none");
            }
            else
            {
                ddlStatus.Items[3].Attributes.Add("style", "display:block");
            }
            mpeFollowup.Hide();
            mpeLeadStatus.Show();
            txtRemark_Forstatus.Text = "";
            gvLeadStatusHistory.DataBind();
        }
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Services") == DBNull.Value)  // Services
            {
                ImageButton imgbtnStatus = (ImageButton)e.Row.FindControl("imgbtnStatus");
                if (imgbtnStatus != null)
                {
                    //imgbtnStatus.Visible = false;
                    e.Row.BackColor = System.Drawing.Color.FromName("#ff000059");
                    e.Row.ToolTip = "No services added yet!";
                    e.Row.Cells[2].ToolTip = "Click to add services here.";
                    LinkButton lnkLead = (LinkButton)e.Row.FindControl("lnkLead");
                    if (lnkLead != null)
                    {
                        lnkLead.ToolTip = "Click to add services here.";
                    }
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "RfqReceived") != DBNull.Value)  // LeadStageID
            {
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "RfqReceived")).ToLower().Trim() == "yes") // Ad-hoc enquiry
                {
                    e.Row.Cells[1].Text = "";
                }
            }
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "ApprovedLeadReportOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeads.Enabled = false;
        gvLeads.Caption = "Approved Leads Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Converted.aspx";
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
            DataFilter1.FilterSessionID = "Converted.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Followup Task

    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeFollowup.Hide();
    }

    protected void btnAddFollowup_Click(object sender, EventArgs e)
    {
        if (hdnLeadId_ForStatus.Value != "" && hdnLeadId_ForStatus.Value != "0")
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

                int result = DBOperations.CRM_AddFollowupHistory(Convert.ToInt32(hdnLeadId_ForStatus.Value), dtFollowup, IsActive, txtRemark.Text.Trim(), loggedInUser.glUserId);
                if (result == 0)
                {
                    lblError_Followup.Text = "Followup added successfully.";
                    lblError_Followup.CssClass = "success";
                    mpeFollowup.Show();
                    gvFollowupHistory.DataBind();
                    txtFollowupDate.Text = "";
                    txtRemark.Text = "";
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
        else
        {
            lblError_Followup.Text = "Lead not found. Please select lead first to create follow up for.";
            lblError_Followup.CssClass = "errorMsg";
            mpeFollowup.Show();
            gvFollowupHistory.DataBind();
        }
    }

    #endregion

    #region Lead Status

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {

        DateTime dtClose = DateTime.MinValue;
        if(txtCloseDate.Text!="")
        {
            dtClose = Convert.ToDateTime(txtCloseDate.Text.Trim());
        }
        int result = DBOperations.CRM_AddLeadStageHistory(Convert.ToInt32(hdnLeadId_ForStatus.Value), Convert.ToInt32(ddlStatus.SelectedValue),
                                              dtClose, txtRemark_Forstatus.Text.Trim(), loggedInUser.glUserId);
        if (result == 0)
        {
            mpeFollowup.Hide();
            mpeLeadStatus.Hide();

            if (Convert.ToInt32(ddlStatus.SelectedValue) == 9) // lost
            {
                string message = "Opportunity lost! Account moved to lost tab.";
                string url = "LostLeads.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            else if (Convert.ToInt32(ddlStatus.SelectedValue) == 7) // quoted
            {
                int LeadStage = DBOperations.CRM_UpdateLeadStatus(result, true, true, true, false, false, false, loggedInUser.glUserId);
                string message = "Account forwarded to quote tab.";
                string url = "Quote.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            else if (Convert.ToInt32(ddlStatus.SelectedValue) == 16) // cold
            {
                string message = "Account moved to Lead tab.";
                string url = "Leads.aspx";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            else
            {
                mpeFollowup.Hide();
                mpeLeadStatus.Hide();
                ddlStatus.DataBind();
                txtRemark_Forstatus.Text = "";
                gvLeadStatusHistory.DataBind();
            }
        }
        else if (result == 1)
        {
            lblError_ForStatus.Text = "System Error! Please try after sometime";
            lblError_ForStatus.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError_ForStatus.Text = "Lead Status Already Updated.";
            lblError_ForStatus.CssClass = "errorMsg";
        }
    }

    protected void imgClose_Forstatus_Click(object sender, ImageClickEventArgs e)
    {
        mpeLeadStatus.Hide();
        hdnLeadId_ForStatus.Value = "0";
        gvLeads.DataBind();
        gvLeadStatusHistory.DataBind();
    }

    #endregion

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlStatus.SelectedValue!="0")
        {
            if(ddlStatus.SelectedValue=="18" || ddlStatus.SelectedValue=="19")
            {
                tdDate.Visible = true;
                txtCloseDate.Visible = true;
                mpeLeadStatus.Show();
            }
            else
            {
                tdDate.Visible = false;
                txtCloseDate.Visible = false;
                mpeLeadStatus.Show();
            }
        }
        else
        {
            lblError_ForStatus.Text = "Please select Status";
            lblError_ForStatus.CssClass = "errorMsg";
        }
    }
}