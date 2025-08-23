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
public partial class AccountExpense_ViewInvoiceMemo : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["InvoiceMemoViewId"] == null)
        {
            // Do Nothing
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
        lnkViewDoc.Enabled = true;
        lnkViewDoc.Text = "View Doc";

        if (Session["InvoiceMemoViewId"] != null)
        {
            int MemoId = Convert.ToInt32(Session["InvoiceMemoViewId"].ToString());

            DataSet dsDetail = AccountExpense.GetInvoiceMemoDetail(MemoId);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblMemoRefNo.Text = dsDetail.Tables[0].Rows[0]["InvoiceMemoRefNo"].ToString();
                lblVendorName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
                lblTotalAmount.Text = dsDetail.Tables[0].Rows[0]["MemoAmount"].ToString();
                
                txtRemark.Text = dsDetail.Tables[0].Rows[0]["Remark"].ToString();

                if (dsDetail.Tables[0].Rows[0]["FilePath"].ToString() != "")
                {
                    string ProfitFilePath = dsDetail.Tables[0].Rows[0]["FilePath"].ToString() + dsDetail.Tables[0].Rows[0]["FileName"].ToString();

                    lnkViewDoc.CommandArgument = ProfitFilePath;
                }
                else
                {
                    lnkViewDoc.Text = "N.A.";
                    lnkViewDoc.Enabled = false;
                }
            }
        }
    }
    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        
        if (e.CommandName.ToLower() == "viewadjustment")
        {
            string strBJVNo = e.CommandArgument.ToString();

            DataSet dsBJVAdjustment = BillingOperation.FAGetJobAdjustmentBilled(strBJVNo);

            if (dsBJVAdjustment.Tables[0].Rows.Count > 0)
            {
                gvBJVAdjustment.DataSource = dsBJVAdjustment;
                gvBJVAdjustment.DataBind();
            }
            else
            {
                gvBJVAdjustment.DataSource = null;
                gvBJVAdjustment.DataBind();
            }

            ModalPopupExtender2.Show();
        }
        else if (e.CommandName.ToLower() == "viewbjv")
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
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }
    protected void lnkViewDoc_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)(sender);

        string DocPath = lnk.CommandArgument;

        if (DocPath != "")
        {
            DownloadDocument(DocPath);
        }
        else
        {
            lblMessage.Text = "Document Not Found!";
            lblMessage.CssClass = "errorMsg";
        }
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
    protected void btnUpdateMemoRemark_Click(object sender, EventArgs e)
    {
        if (Session["InvoiceMemoViewId"] == null)
        {
            lblMessage.Text = "Session Time Out! Please Refresh!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            int MemoId = Convert.ToInt32(Session["InvoiceMemoViewId"].ToString());

            // Add Memo Remark and Job Profit Excel

            string remark = txtRemark.Text.Trim();// "Memo Audit";

            string strDirPath = "", strInvoiceFilePath = "", FileName1 = "";

            if (fuProfitDoc.HasFile)
            {
                // Upload Document

                strDirPath = lblMemoRefNo.Text.Replace("/", "");

                strDirPath = strDirPath.Replace("-", "");

                strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

                FileName1 = UploadDocument(fuProfitDoc, strInvoiceFilePath);
            }

            int result2 = AccountExpense.AddInvoiceMemoProfit(MemoId, remark, strInvoiceFilePath, FileName1, LoggedInUser.glUserId);
                        
            if (result2 == 0)
            {
                lblMessage.Text = "Invoice Memo Detail Updated Successfully!";
                lblMessage.CssClass = "success";
            }
            else if (result2 == 2)
            {
                lblMessage.Text = "Detail Not Updated! Memo Payment Initiated!!";
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
    protected void btnCheckBJVExpense_Click(object sender, EventArgs e)
    {
        int MemoId = Convert.ToInt32(Session["InvoiceMemoViewId"].ToString());

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