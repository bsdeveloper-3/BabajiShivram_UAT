using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class BillingTransport_TransBillPaid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["VendorId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Bill Payment Detail";
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("BillTransportLogin.aspx");
            }
        }

        DataFilter1.DataSource = DataSourceVendorJobs;
        DataFilter1.DataColumns = gvVendorJobDetail.Columns;
        DataFilter1.FilterSessionID = "TransBillPaid.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvVendorJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.ToLower() == "select")
        //{
        //    string JobId = e.CommandArgument.ToString();

        //    Session["TransReqId"] = JobId.ToString();
        //    Response.Redirect("TransBillPaid.aspx");
        //}
    }
    protected void gvVendorJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Bill_Payment_Detail" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
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
            DataFilter1.FilterSessionID = "TransBillPaid.aspx";
            DataFilter1.FilterDataSource();
            gvVendorJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
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

        gvVendorJobDetail.AllowPaging = false;
        gvVendorJobDetail.AllowSorting = false;
        //gvVendorJobDetail.Columns[1].Visible = false;
        //gvVendorJobDetail.Columns[2].Visible = true;

        gvVendorJobDetail.Caption = "Payment Detail " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TransBillPaid.aspx";
        DataFilter1.FilterDataSource();
        gvVendorJobDetail.DataBind();

        gvVendorJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}