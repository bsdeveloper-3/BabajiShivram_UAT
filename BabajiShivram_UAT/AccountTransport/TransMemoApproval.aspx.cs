using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using QueryStringEncryption;
public partial class AccountTransport_TransMemoApproval : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvDetail);

        if (Session["MemoApproveId"] == null)
        {
            Response.Redirect("PendingTransMemoApproval.aspx");
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
        int MemoId = Convert.ToInt32(Session["MemoApproveId"].ToString());

        DataSet dsDetail = DBOperations.GetTransMemoDetail(MemoId);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            lblMemoRefNo.Text = dsDetail.Tables[0].Rows[0]["PayMemoRefNo"].ToString();
            lblTransporterName.Text = dsDetail.Tables[0].Rows[0]["VendorName"].ToString();
            lblTotalAmount.Text = dsDetail.Tables[0].Rows[0]["MemoAmount"].ToString();
        }
    }
    protected void btnApproveMemo_Click(object sender, EventArgs e)
    {
        int MemoId = Convert.ToInt32(Session["MemoApproveId"].ToString());
        int ApprovalStatus = 1;

        string remark = "Memo Approved";

        if (txtRemark.Text.Trim().Length > 1)
        {
            remark = txtRemark.Text.Trim();
            int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoApproved;

            DBOperations.AddTransMemoStatus(MemoId, InvoiceStatus, remark, LoggedInUser.glUserId);

            int Result = DBOperations.AddTransPayMemoApproval(MemoId, ApprovalStatus, InvoiceStatus, LoggedInUser.glUserId);

            Session["MemoApproveId"] = null;

            if (Result == 0)
            {
                lblMessage.Text = "Transport Memo Approved Successfully!";
                lblMessage.CssClass = "success";

                ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('" + lblMessage.Text + "');", true);
            }
            else
            {
                lblMessage.Text = "Memo details Not Found For Approval!";
                lblMessage.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
            }

            Response.Redirect("PendingTransMemoApproval.aspx");
        }
        else
        {
            lblMessage.Text = "Please Enter Remark!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    protected void btnRejectMemo_Click(object sender, EventArgs e)
    {
        int MemoId = Convert.ToInt32(Session["MemoApproveId"].ToString());
        int ApprovalStatus = 2;// Rejected

        string remark = "Memo Rejected";

        if (txtRemark.Text.Trim().Length > 4)
        {
            remark = txtRemark.Text.Trim();
            // int InvoiceStatus = (Int32)EnumInvoiceStatus.MemoMgmtReject;

            int Result = DBOperations.AddTransPayMemoReject(MemoId, remark, LoggedInUser.glUserId);

            Session["MemoApproveId"] = null;

            if (Result == 0)
            {
                lblMessage.Text = "Transport Memo Rejected Successfully!";
                lblMessage.CssClass = "success";

                ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('" + lblMessage.Text + "');", true);
            }
            else
            {
                lblMessage.Text = "Memo details Not Found For Approval!";
                lblMessage.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
            }

            Response.Redirect("PendingTransMemoApproval.aspx");
        }
        else
        {
            lblMessage.Text = "Please Enter Remark!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region Document Download

    protected void gvDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        int DocTypeID = 1; // Invoice/Bill Copy

        if (e.CommandName.ToLower() == "download")
        {
            int TransPayId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = DBOperations.GetTransPayDocByTypeID(TransPayId, DocTypeID);

            if (dsGetDocPath.Tables[0].Rows.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath;

                //strDocpath = strFilePath + strFileName;
                DownloadDocument(strDocpath);
            }
            else
            {
                lblMessage.Text = "Invoice Copy Not Found!";
                lblMessage.CssClass = "errorMsg";
            }

        }
        else if (e.CommandName.ToLower() == "view")
        {
            int TransPayId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = DBOperations.GetTransPayDocByTypeID(TransPayId, DocTypeID);

            if (dsGetDocPath.Tables[0].Rows.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath;

                //strDocpath = strFilePath + strFileName;
                ViewDocument(strDocpath);
            }
        }
    }
    private void DownloadDocument(string DocumentPath)
    {
        lblMessage.Text = "";
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
            lblMessage.Text = ex.Message;
            lblMessage.CssClass = "errorMsg";
        }
    }
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Memo_Approval_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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
        //gvDetail.Columns[2].Visible = false;
        //gvDetail.Columns[3].Visible = false;
        //gvDetail.Columns[4].Visible = false;
        //gvDetail.Columns[5].Visible = true;

        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}