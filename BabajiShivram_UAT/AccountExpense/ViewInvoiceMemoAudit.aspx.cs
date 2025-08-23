using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using AjaxControlToolkit;
public partial class AccountExpense_ViewInvoiceMemoAudit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["InvoiceMemoAuditId"] == null)
        {
            Response.Redirect("PendingInvoiceMemoAudit.aspx");
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
        if (Session["InvoiceMemoAuditId"] == null)
        {
            Response.Redirect("PendingInvoiceMemoAudit.aspx");
        }

        int MemoId = Convert.ToInt32(Session["InvoiceMemoAuditId"].ToString());

        DataSet dsDetail = AccountExpense.GetInvoiceMemoDetail(MemoId);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            lblMemoRefNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceMemoRefNo"].ToString();
            lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblTotalAmount.Text = dsDetail.Tables[0].Rows[0]["MemoAmount"].ToString();
        }
    }
    protected void btnRejectMemo_Click(object sender, EventArgs e)
    {
        string remark = "Memo Cancelled";

        int MemoId = Convert.ToInt32(Session["InvoiceMemoAuditId"].ToString());

        int Result = AccountExpense.AddInvoiceMemoCancel(MemoId, remark, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblMessage.Text = "Vendor Payment Memo Cancelled!!";
            lblMessage.CssClass = "success";

            Session["InvoiceMemoAuditId"] = "0";
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
    protected void btnApproveAuditMemo_Click(object sender, EventArgs e)
    {
        if (Session["InvoiceMemoAuditId"] == null)
        {
            Response.Redirect("PendingInvoiceMemoAudit.aspx");
        }
        else
        {
            int MemoId = Convert.ToInt32(Session["InvoiceMemoAuditId"].ToString());

            int ApprovalStatus = 0; // Audit Completed
            int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoAuditCompleted;

            string remark = txtRemark.Text.Trim() ;// "Memo Audit";

            AccountExpense.AddInvoiceMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

            int Result = AccountExpense.AddInvoiceMemoAudit(MemoId, ApprovalStatus, InvoiceStatus, LoggedInUser.glUserId);

            // Add Memo Remark and Job Profit Excel

            string strDirPath = "", strInvoiceFilePath = "", FileName1 = "";
            
            if (fuProfitDoc.HasFile)
            {
                // Upload Document

                strDirPath = lblMemoRefNo.Text.Replace("/", "");

                strDirPath = strDirPath.Replace("-", "");

                strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

                FileName1 = UploadDocument(fuProfitDoc, strInvoiceFilePath);
            }

            int result2 = AccountExpense.AddInvoiceMemoProfit(MemoId,remark,strInvoiceFilePath,FileName1,LoggedInUser.glUserId);

            Session["InvoiceMemoAuditId"] = null;

            if (Result == 0)
            {
                lblMessage.Text = "Invoice Memo Audited Successfully!";
                lblMessage.CssClass = "success";
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
    }
    public string UploadDocument(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FileName;
        }
        else
        {
            FileName = "";

            return FileName;
        }
    }
    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }

        return builder.ToString();
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
         if (e.CommandName.ToLower() == "viewbjv")
        {
            string strBJVNo = e.CommandArgument.ToString();

            DataSet dsBJVDetail = BillingOperation.FAGetJobExpenseBilled(strBJVNo);

            if (dsBJVDetail.Tables[0].Rows.Count > 0)
            {
                gvBJVDetail.DataSource = dsBJVDetail;
                gvBJVDetail.DataBind();
            }
            else
            {
                gvBJVDetail.DataSource = null;
                gvBJVDetail.DataBind();
            }

            ModalPopupExtenderBJV.Show();
        }

    }
    protected void btnCancelBJVPopup2_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderBJV.Hide();
    }
    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        if (gvBJVDetail.Rows.Count > 1)
        {
            GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
            getRow.Cells[7].BackColor = System.Drawing.Color.Yellow;
            getRow.Cells[8].BackColor = System.Drawing.Color.Green;

            decimal decDebit = 0m;
            decimal decCredit = 0m;
            decimal decProfit = 0m;

            Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
            Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

            decProfit = (decCredit - decDebit);


            if (decProfit <= 0)
            {
                getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
                getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
            }
            else
            {
                getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
            }

        }
    }
    protected void btnCheckBJVExpense_Click(object sender, EventArgs e)
    {
        int MemoId = Convert.ToInt32(Session["InvoiceMemoAuditId"].ToString());

        int Successid = AccountExpense.ValidateVendorInvoiceMemoExpense(MemoId, LoggedInUser.glUserId);

        if (Successid == 0)
        {
            lblMessage.Text = "Job Expense Validated with BJV Successfully!";
            lblMessage.CssClass = "success";
            gvDetail.DataBind();
        }
        else
        {
            lblMessage.Text = "System Error Please try After Sometime!";
            lblMessage.CssClass = "errorMsg";
        }
    }
}