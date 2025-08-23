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

public partial class Service_MiscJobTrack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job Tracking";
        if (!IsPostBack)
        {
            Session["MiscJobId"] = null;

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
        DataFilter1.FilterSessionID = "MiscJobTrack.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.ToLower() == "select")
        //{
        //    string strJobId = (string)e.CommandArgument;            
        //    Session["JobId"] = strJobId;
        //    Response.Redirect("JobDetail.aspx"); ;
        //}
        if (e.CommandName.ToLower() == "select")
        {
            //string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            //string strJobId = "", strJobType = "";

            //if (commandArgs[0].ToString() != "")
            //    strJobId = commandArgs[0].ToString();
            //if (commandArgs[1].ToString() != "")
            //    strJobType = commandArgs[1].ToString();

            Session["MiscJobId"] = e.CommandArgument.ToString();

            Response.Redirect("MiscJobDetail.aspx");

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
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "MiscJobDetail.aspx";
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
        string strFileName = "Misc_JobDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        //gvJobDetail.Columns[1].Visible = false;
        //gvJobDetail.Columns[2].Visible = true;
        //gvJobDetail.Columns[3].Visible = false;

        //gvJobDetail.Columns[13].Visible = true; // MBL
        //gvJobDetail.Columns[14].Visible = false; // MBL

        // Excel Header Not Requierd-- Issue in excel header format after export
        // gvJobDetail.Caption = "Job Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "MiscJobDetail.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvJobDetail);
        gvJobDetail.RenderControl(hw);

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
    #endregion
}