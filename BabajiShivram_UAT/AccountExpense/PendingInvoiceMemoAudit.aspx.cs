using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountExpense_PendingInvoiceMemoAudit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvDetail);

        Session["InvoicePayId"] = null;
        Session["InvoiceMemoAuditId"] = null;
        Session["InvoiceMemoViewId"] = null;

        if (!IsPostBack)
        {
            
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Invoice Memo Audit Detail";

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
        DataFilter1.FilterSessionID = "PendingInvoiceMemoAudit.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["InvoiceMemoAuditId"] = e.CommandArgument.ToString();
            Response.Redirect("ViewInvoiceMemoAudit.aspx");
        }
        else if (e.CommandName.ToLower() == "viewdoc")
        {
            string strDocPath = e.CommandArgument.ToString();

            if (strDocPath != "")
            {
                DownloadDocument(strDocPath);

                lblMessage.Text = strDocPath;
            }
            else
            {
                lblMessage.Text = "Document Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
    protected void btnAuditMemo_Click(object sender, EventArgs e)
    {
        int TotalMemo = 0;
        int ApprovalStatus = 0; // Audit Completed
        int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoAuditCompleted;

        string remark = "Memo Audit";

        foreach (GridViewRow gvr1 in gvDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    TotalMemo++;

                    int MemoId = Convert.ToInt32(gvDetail.DataKeys[gvr1.RowIndex].Value);

                    AccountExpense.AddInvoiceMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

                    int Result = AccountExpense.AddInvoiceMemoAudit(MemoId, ApprovalStatus, InvoiceStatus, LoggedInUser.glUserId);

                    if (Result == 0)
                    {
                        lblMessage.Text = "Invoice Memo Audit Successfully!";
                        lblMessage.CssClass = "success";
                    }
                    else
                    {
                        lblMessage.Text = "Memo details Not Found For Audit!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }//END_ForEach

        if (TotalMemo == 0)
        {
            lblMessage.Text = "Please select Memo Ref No For Audit!";
            lblMessage.CssClass = "errorMsg";
        }

        gvDetail.DataBind();
    }
    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
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
            DataFilter1.FilterSessionID = "PendingInvoiceMemoAudit.aspx";
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
        string strFileName = "Pending_Memo_Approval" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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
        gvDetail.Columns[2].Visible = false;
        gvDetail.Columns[7].Visible = false;
        gvDetail.Columns[3].Visible = true;


        DataFilter1.FilterSessionID = "PendingInvoiceMemoAudit.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}