using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountTransport_PendingTransPaymentAPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["TransPayId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "NBCPL Transporter Payment";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;

            }
        }

        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "PendingTransPaymentAPI.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strTransPayId = (string)e.CommandArgument;

            Session["TransPayId"] = strTransPayId;

            Response.Redirect("TransPaymentAPI.aspx"); ;
        }

    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsBilled")) == true)
            //{
            //    e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
            //    e.Row.ToolTip = "Billed Job";
            //}

            if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId").ToString());
                string strStatusRemark = DataBinder.Eval(e.Row.DataItem, "Remark").ToString();

                if (StatusId == 149) // Payment On Hold - 
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = strStatusRemark;
                }
            }
        }
    }
    protected void btnBatchPayment_Click(object sender, EventArgs e)
    {
        int TotalBatchRequest = 0;
        int TotalBatchSuccess = 0;

        lblMessage.Text = "Bach Payment Initiated";

        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    TotalBatchRequest = TotalBatchRequest + 1;

                    int RequestId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);

                    int Output = DBOperations.AddTransPayBacth(RequestId, LoggedInUser.glUserId);

                    if (Output == 0)
                    {
                        TotalBatchSuccess = TotalBatchSuccess + 1;
                    }
                }
            }
        }//END_ForEach

        if (TotalBatchRequest == 0)
        {
            lblMessage.Text = "Please select Invoices For Bach Payment!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            lblMessage.Text = "Batch Payment Initiated for " + TotalBatchSuccess.ToString() + " Out Of " + TotalBatchRequest.ToString() + " Request!";
            lblMessage.CssClass = "success";
        }

    }

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
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
            DataFilter1.FilterSessionID = "PendingTransPaymentAPI.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
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
        string strFileName = "Pending_Transport_PaymentAPI" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;
        gvDetail.Columns[1].Visible = false;
        gvDetail.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "PendingTransPaymentAPI.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}