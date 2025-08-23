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


public partial class AccountExpense_VendorDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "FA - Vendor Tracking";
        
        if (!IsPostBack)
        {
            Session["VendorId"] = null;

            if (gvVendorDetail.Rows.Count == 0)
            {
                lblMessage.Text = "FA - Vendor Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //

        DataFilter1.DataSource = VendorSqlDataSource;
        DataFilter1.DataColumns = gvVendorDetail.Columns;
        DataFilter1.FilterSessionID = "VendorDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvVendorDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")

            Session["VendorId"] = e.CommandArgument.ToString();

            Response.Redirect("VendorDetailBank.aspx");
    }

    protected void gvVendorDetail_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "VendorDetail.aspx";
            DataFilter1.FilterDataSource();
            gvVendorDetail.DataBind();
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
        string strFileName = "FA_Vendor_Detail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        gvVendorDetail.AllowPaging = false;
        gvVendorDetail.AllowSorting = false;

        gvVendorDetail.Columns[1].Visible = false;
        gvVendorDetail.Columns[2].Visible = true;

        // Excel Header Not Requierd-- Issue in excel header format after export
        // gvVendorDetail.Caption = "Job Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "VendorDetail.aspx";
        DataFilter1.FilterDataSource();
        gvVendorDetail.DataBind();

        gvVendorDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

        //  gvVendorDetail.DataSourceID = "VendorSqlDataSource";
        //  gvVendorDetail.DataBind();

        //  BindGridData();
        //  gvVendorDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //  gvVendorDetail.HeaderRow.Cells[0].Visible = false;
        //  for (int i = 0; i < gvVendorDetail.HeaderRow.Cells.Count; i++)
        //  {
        //    gvVendorDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvVendorDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //  }

    }
    #endregion
}