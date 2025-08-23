using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class CRMReports_LocationHead : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Product Wise Business Leads Potential";

            if (gvLocationHead.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Location Head Wise Services!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLocationHead;
        DataFilter1.DataColumns = gvLocationHead.Columns;
        DataFilter1.FilterSessionID = "LocationHead.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvLocationHead.DataBind();
    }

    protected void ddlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        gvLocationHead.DataBind();
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "HeadServicesReport On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLocationHead.AllowPaging = false;
        gvLocationHead.AllowSorting = false;
        gvLocationHead.Columns[0].Visible = false;
        gvLocationHead.Enabled = false;
        gvLocationHead.Caption = "Location Head Wise Services Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "LocationHead.aspx";
        DataFilter1.FilterDataSource();
        gvLocationHead.DataBind();
        gvLocationHead.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "LocationHead.aspx";
            DataFilter1.FilterDataSource();
            gvLocationHead.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        gvLocationHead.DataBind();
    }
}