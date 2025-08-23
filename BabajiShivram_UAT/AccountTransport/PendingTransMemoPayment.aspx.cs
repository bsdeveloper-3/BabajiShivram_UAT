using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountTransport_PendingTransMemoPayment : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["TransPayId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transporter Memo Payment";

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
        DataFilter1.FilterSessionID = "PendingTransMemoPayment.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["MemoViewId"] = e.CommandArgument.ToString();
            Response.Redirect("ViewTransMemo.aspx");
        }
    }

    protected void btnPayMemo_Click(object sender, EventArgs e)
    {
        int TotalMemo = 0;
        int MemoPaymentStatus = 3;
        //int InvoiceStatus = 135;

        string remark = "Memo Payment";
        int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoPaymentInitiated;

        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    TotalMemo++;

                    int MemoId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);
                    
                    DBOperations.AddTransMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

                    int Result = DBOperations.AddTransPayMemoPayment(MemoId, MemoPaymentStatus, InvoiceStatus, LoggedInUser.glUserId);

                    if (Result == 0)
                    {
                        // Initiate Batch Payment

                        DBOperations.AddTransMempPayBacth(MemoId, LoggedInUser.glUserId);

                        lblMessage.Text = "Transporter Payment Initiated Successfully!";
                        lblMessage.CssClass = "success";

                        gvDetail.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System Error! Please try after sometime!";
                        lblMessage.CssClass = "errorMsg";
                        gvDetail.DataBind();
                    }
                }
            }
        }//END_ForEach

        if (TotalMemo == 0)
        {
            lblMessage.Text = "Please select Memo Ref No For Payment!";
            lblMessage.CssClass = "errorMsg";
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
            DataFilter1.FilterSessionID = "PendingTransMemoPayment.aspx";
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
        string strFileName = "Memo_For_Payment" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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
        //gvDetail.Columns[1].Visible = false;
        //gvDetail.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "PendingTransMemoPayment.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    protected void btnRejectMemo_Click(object sender, EventArgs e)
    {
        int TotalMemo = 0;
        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    TotalMemo++;

                    int MemoId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);

                    RejectMempPayment(MemoId);
                }
            }
        }

        if (TotalMemo == 0)
        {
            lblMessage.Text = "Please select Memo Ref No!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            gvDetail.DataBind();
        }
    }
    protected void RejectMempPayment(int TransMemoID)
    {
        string remark = "Memo Cancelled";

        int Result = DBOperations.AddTransPayMemoCancel(TransMemoID, remark, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblMessage.Text = "Transport Payment Memo Cancelled!!";
            lblMessage.CssClass = "success";
        }
        else if (Result == 2)
        {
            lblMessage.Text = "Memo details Not Found!";
            lblMessage.CssClass = "errorMsg";
            gvDetail.DataBind();
        }
        else
        {
            lblMessage.Text = "System Error! Please try after somtetime!";
            lblMessage.CssClass = "errorMsg";
            gvDetail.DataBind();
        }
    }
}