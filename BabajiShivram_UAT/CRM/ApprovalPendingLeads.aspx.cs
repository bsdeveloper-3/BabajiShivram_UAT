using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class CRM_PendingLeads : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        ScriptManager1.RegisterPostBackControl(GridViewDocument);
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Lead Approval";

            if(gvLeads.Rows.Count==0)
            {
                lblError.Text = "No Record Found for Lead";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "PendingLeads.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected override void OnLoadComplete(EventArgs e)
    {
        if(!Page.IsPostBack)
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
            DataFilter1.FilterSessionID = "PendingLeads.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName.ToLower().Trim()== "select")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            //DataTable dtQuote = DBOperations.CRM_GetQuoteApprovalLead(Convert.ToInt32(Session["LeadId"]), loggedInUser.glUserId, loggedInUser.glFinYearId);
            //if(dtQuote.Rows.Count>0)
            //{
            //    foreach (DataRow row in dtQuote.Rows)
            //    {
            //        string count = row["TOTAL"].ToString();
            //        if(count!="0")
            //        {
            //            //FOR QUOTATION APPROVAL
            //            Response.Redirect("QuoteApproval.aspx?i=" + Session["LeadId"] + "&a=" + Session["UserId"] + "");
            //        }
            //        else
            //        {
            //            // FOR LEAD APPROVAL
            //            Response.Redirect("LeadApproval.aspx?i=" + Session["LeadId"] + "&a=" + Session["UserId"] + "");
            //            //Response.Redirect("NewLeadApproval.aspx?i=" + Session["LeadId"] + "&a=" + Session["UserId"] + "");
            //        }
            //    }
            //}
            // Response.Redirect("Approval.aspx");
            Response.Redirect("LeadApproval.aspx?i=" + Session["LeadId"]+ "&a=" + Session["UserId"] + "");
            //Response.Redirect("NewLeadApproval.aspx?i=" + Session["LeadId"] + "&a=" + Session["UserId"] + "");
        }
        else if (e.CommandName.ToLower().ToString().Trim() == "showdocs")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            hdnLeadId.Value = commandArgs[0].ToString();
            //lblLeadRefNo.Text = commandArgs[2].ToString();
            mpeDocument.Show();
            GridViewDocument.DataBind();
        }
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType==DataControlRowType.DataRow)
        {

        }
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Pending Leads Report On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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
        gvLeads.Caption = "Pending Leads Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingLeads.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    #region Pop-up Events
    protected void GridViewDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            mpeDocument.Show();
        }
    }

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeDocument.Hide();
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        else
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;

        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception)
        {
        }
    }
    #endregion

}