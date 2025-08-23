using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountTransport_ViewTransMemoAudit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["MemoAuditId"] == null)
        {
            Response.Redirect("PendingTransMemoAudit.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Memo Detail";

            GetMemoDetail();
        }
    }
    private void GetMemoDetail()
    {
        if (Session["MemoAuditId"] == null)
        {
            Response.Redirect("PendingTransMemoAudit.aspx");
        }

        int MemoId = Convert.ToInt32(Session["MemoAuditId"].ToString());

        DataSet dsDetail = DBOperations.GetTransMemoDetail(MemoId);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            lblMemoRefNo.Text       =   dsDetail.Tables[0].Rows[0]["PayMemoRefNo"].ToString();
            lblTransporterName.Text =   dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblTotalAmount.Text     =   dsDetail.Tables[0].Rows[0]["MemoAmount"].ToString();
        }
    }
    protected void btnRejectMemo_Click(object sender, EventArgs e)
    {
        string remark = "Memo Cancelled";

        if (txtRemark.Text.Trim().Length > 4)
        {
            remark = txtRemark.Text.Trim();
            int MemoId = Convert.ToInt32(Session["MemoAuditId"].ToString());

            int Result = DBOperations.AddTransPayMemoCancel(MemoId, remark, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblMessage.Text = "Transport Memo Cancelled!!";
                lblMessage.CssClass = "success";

                Session["MemoAuditId"] = "0";
                gvDetail.DataBind();
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
        else
        {
            lblMessage.Text = "Please Enter Remark!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    protected void btnApproveAuditMemo_Click(object sender, EventArgs e)
    {
        int MemoId = Convert.ToInt32(Session["MemoAuditId"].ToString());

        int ApprovalStatus = 0; // Audit Completed
        int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoAuditCompleted;

        string remark = "Memo Audit";

        if (txtRemark.Text.Trim().Length > 1)
        {
            remark = txtRemark.Text.Trim();

            DBOperations.AddTransMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

            int Result = DBOperations.AddTransPayMemoApproval(MemoId, ApprovalStatus, InvoiceStatus, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblMessage.Text = "Transport Memo Audit Successfully!";
                lblMessage.CssClass = "success";

                gvDetail.DataBind();
            }
            else if (Result == 2)
            {
                lblMessage.Text = "Memo details Not Found For Audit!";
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
        else
        {
            lblMessage.Text = "Please Enter Remark!";
            lblMessage.CssClass = "errorMsg";
        }
    }
}