using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class BSCCPLTransport_PendingTransRejected : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(lnkJobExport);

        if (!IsPostBack)
        {
            Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   = "Payment Rejected";

            Session["TransPayId"] = null;
            Session["TransReqId"] = null;
            Session["TransporterID"] = null;
        }

        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingTransRejected.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strTransPayId = (string)e.CommandArgument;

            Session["TransPayId"] = strTransPayId;

            Response.Redirect("BillSubmissionEdit.aspx");

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
            DataFilter1.FilterSessionID = "PendingTransRejected.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}