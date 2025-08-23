using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountTransport_PendingTransMemoAudit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Session["TransPayId"] = null;
        Session["MemoAuditId"] = null;

        if (!IsPostBack)
        {
            Session["MemoViewId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Audit Memo Detail";

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
        DataFilter1.FilterSessionID = "PendingTransMemoAudit.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["MemoAuditId"] = e.CommandArgument.ToString();
            Response.Redirect("ViewTransMemoAudit.aspx");
        }
    }
    protected void btnAuditMemo_Click(object sender, EventArgs e)
    {
        int TotalMemo = 0;
        int ApprovalStatus = 0; // Audit Completed
        int InvoiceStatus = (Int32) EnumInvoiceStatus.MemoAuditCompleted;

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

                    DBOperations.AddTransMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

                    int Result = DBOperations.AddTransPayMemoAudit(MemoId, ApprovalStatus, InvoiceStatus, LoggedInUser.glUserId);

                    if (Result == 0)
                    {
                        lblMessage.Text = "Transport Memo Audit Successfully!";
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
            lblMessage.Text = "Please select Memo Ref No For Payment Approval!";
            lblMessage.CssClass = "errorMsg";
        }

        gvDetail.DataBind();
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
            DataFilter1.FilterSessionID = "PendingTransMemoApproval.aspx";
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
        //gvDetail.Columns[1].Visible = false;
        //gvDetail.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "PendingTransMemoApproval.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}