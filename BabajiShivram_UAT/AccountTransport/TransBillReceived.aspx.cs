using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountTransport_TransBillReceived : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Received";                        
        }

        DataFilter1.DataSource = DataSourceBill;
        DataFilter1.DataColumns = gvReceiveBill.Columns;
        DataFilter1.FilterSessionID = "TransBillReceived.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvReceiveBill_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["TRId"] = commandArgs[0].ToString();
            Session["TRConsolidateId"] = commandArgs[1].ToString();
            Session["TransBillId"] = commandArgs[2].ToString();
            Session["TransporterId"] = commandArgs[3].ToString();
            Response.Redirect("../BillingTransport/TransBillReceivedDetail.aspx");
        }
    }
        #region ExportData
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Bill_Received_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvReceiveBill.AllowPaging = false;
        gvReceiveBill.AllowSorting = false;
        gvReceiveBill.Columns[1].Visible = false;
        
        DataFilter1.FilterSessionID = "TransBillReceived.aspx";
        DataFilter1.FilterDataSource();
        gvReceiveBill.DataBind();
        gvReceiveBill.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "ViewTransporterBill.aspx";
            DataFilter1.DataColumns = gvReceiveBill.Columns;
            DataFilter1.FilterDataSource();
            gvReceiveBill.DataBind();
            if (gvReceiveBill.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Transport Bill!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
     
    #endregion
}