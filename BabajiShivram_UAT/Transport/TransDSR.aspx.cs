using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BSImport.CountryManager.BO;

public partial class Transport_TransDSR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Session["TRId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transort DSR";

        }

        DataFilter1.DataSource = DataSourceTransDSR;
        DataFilter1.DataColumns = gvTransDSR.Columns;
        DataFilter1.FilterSessionID = "TransDSR.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["TRId"] = e.CommandArgument.ToString();
            Response.Redirect("JobDetail.aspx");
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
            DataFilter1.FilterSessionID = "TransDSR.aspx";
            DataFilter1.DataColumns = gvTransDSR.Columns;
            DataFilter1.FilterDataSource();
            gvTransDSR.DataBind();
            if (gvTransDSR.Rows.Count == 0)
            {
                lblError_Job.Text = "No Job Found For DSR!";
                lblError_Job.CssClass = "errorMsg";
            }
            else
            {
                lblError_Job.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Transport_DSR_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvTransDSR.AllowPaging = false;
        gvTransDSR.AllowSorting = false;
        //gvTransDSR.Columns[1].Visible = false;
        //gvTransDSR.Columns[2].Visible = false;
        gvTransDSR.Caption = "Transport_DSR " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "JobTracking.aspx";
        DataFilter1.FilterDataSource();
        gvTransDSR.DataBind();
        gvTransDSR.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

}