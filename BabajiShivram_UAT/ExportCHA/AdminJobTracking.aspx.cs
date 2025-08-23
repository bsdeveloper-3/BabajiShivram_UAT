using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using System.Collections;


public partial class ExportCHA_AdminJobTracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Shipment Tracking";
        if (!IsPostBack)
        {
            Session["JobId"] = null;
            gvJobDetail.DataBind();
            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Detail Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //

        DataFilter1.DataSource = JobDetailSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "AdminJobTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;
            string[] words = strJobId.Split(',');
            string jobid = words[0].ToUpperInvariant();
            string status = words[1].ToString();

            Session["JobId"] = jobid;
            Session["Status"] = status;

            Response.Redirect("EditExpJobDetail.aspx");
        }
        else if (e.CommandName.ToLower() == "statusredirect")
        {
            string strJobId = (string)e.CommandArgument;
            string[] words = strJobId.Split(',');
            string jobid = words[0].ToUpperInvariant();
            string status = words[1].ToString();

            Session["JobId"] = jobid;
            Session["Status"] = status;
            Response.Redirect("EditExpJobDetail.aspx?ActiveTab=activity");
        }
    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

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
            DataFilter1.FilterSessionID = "AdminJobTracking.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "JobDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
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

        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;

        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;
        DataFilter1.FilterSessionID = "AdminJobTracking.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}