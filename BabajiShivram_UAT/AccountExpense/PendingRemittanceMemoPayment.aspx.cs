using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class AccountExpense_PendingRemittanceMemoPayment : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Session["InvoicePayId"] = null;
        Session["InvoiceMemoAuditId"] = null;

        if (!IsPostBack)
        {
            //Session["TransPayId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Remittance Memo Payment";

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
        DataFilter1.FilterSessionID = "PendingRemittanceMemoPayment.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["RemittanceMemoViewId"] = e.CommandArgument.ToString();
            Response.Redirect("ViewRemittanceMemo.aspx");
        }
    }

    protected void btnPayMemo11_Click(object sender, EventArgs e)
    {
        int TotalMemo = 0;
        int MemoPaymentStatus = 3;
        //int InvoiceStatus = 135;

        string remark = "Vendor Memo Payment";
        int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoPaymentInitiated;

        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoballTest");

                if (chk1.Checked)
                {
                    TotalMemo++;

                    int MemoId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);

                    AccountExpense.AddInvoiceMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

                    int Result = AccountExpense.AddInvoiceMemoPayment(MemoId, MemoPaymentStatus, InvoiceStatus, LoggedInUser.glUserId);

                    if (Result == 0)
                    {
                        // Initiate Batch Payment

                        AccountExpense.AddInvoiceMemoPayBacth(MemoId, LoggedInUser.glUserId);

                        lblMessage.Text = "Vendor Payment Initiated Successfully!";
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
            DataFilter1.FilterSessionID = "PendingInvoiceMemoPayment.aspx";
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
        string strFileName = "Vendor_Memo_For_Payment" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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

        DataFilter1.FilterSessionID = "PendingInvoiceMemoPayment.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}