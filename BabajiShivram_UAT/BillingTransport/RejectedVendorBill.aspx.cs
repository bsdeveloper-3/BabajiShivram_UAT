using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class BillingTransport_RejectedVendorBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["VendorId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Bill Submission";
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("VendorLogin.aspx");
            }
        }

        DataFilter1.DataSource = DataSourceRejectedJobs;
        DataFilter1.DataColumns = gvVendorRejectedBill.Columns;
        DataFilter1.FilterSessionID = "RejectedVendorBill.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvVendorRejectedBill_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string JobId = e.CommandArgument.ToString();

            Session["TransReqId"] = JobId.ToString();
            Response.Redirect("VendorBillSubmission.aspx");
        }
    }
    protected void gvVendorRejectedBill_PreRender(object sender, EventArgs e)
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
        string strFileName = "Pending_Bill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
            DataFilter1.FilterSessionID = "RejectedVendorBill.aspx";
            DataFilter1.FilterDataSource();
            gvVendorRejectedBill.DataBind();
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

        gvVendorRejectedBill.AllowPaging = false;
        gvVendorRejectedBill.AllowSorting = false;
        gvVendorRejectedBill.Columns[1].Visible = false;
        gvVendorRejectedBill.Columns[2].Visible = true;

        gvVendorRejectedBill.Caption = "Vehicle Detail " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "RejectedVendorBill.aspx";
        DataFilter1.FilterDataSource();
        gvVendorRejectedBill.DataBind();

        gvVendorRejectedBill.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}