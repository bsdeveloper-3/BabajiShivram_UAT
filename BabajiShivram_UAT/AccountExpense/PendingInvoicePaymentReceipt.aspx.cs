using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
public partial class AccountExpense_PendingInvoicePaymentReceipt : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["InvoiceId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Payment Receipt";

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
        DataFilter1.FilterSessionID = "PendingInvoicePaymentReceipt.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            int InvoiceID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnInvoiceId.Value = InvoiceID.ToString();

            DataSet dsGetPaymentDetails = AccountExpense.GetInvoiceDetail(InvoiceID);

            if (dsGetPaymentDetails.Tables.Count > 0)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["FARefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"] != null)
                    lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["InvoiceAmount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"] != null)
                    lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["VendorName"].ToString();
                                
                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedDate"] != null)
                    lblCreatedDate1.Text = Convert.ToDateTime(dsGetPaymentDetails.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy");

            }

            RejectModalPopupExtender.Show();

        }

    }

    protected void btnUploadReceipt_Click(object sender, EventArgs e)
    {
        int InvoiceId = Convert.ToInt32(hdnInvoiceId.Value);

        string strDirPath = lblJobNumber.Text.Trim().Replace("/", "");

        strDirPath = strDirPath.Replace("-", "");

        string strInvoiceFilePath = "VendorInvoice//" + strDirPath + "//";

        if (fuReceipt.HasFile)
        {
            string FileName10 = UploadDocument(fuReceipt, strInvoiceFilePath);

            int FileOutput1 = AccountExpense.AddInvoiceDocument(InvoiceId, 11, strInvoiceFilePath, FileName10, LoggedInUser.glUserId);

            gvDetail.DataBind();
            lblMessage.Text = "Receipt Uploaded Successfully!.";
            lblMessage.CssClass = "success";

            RejectModalPopupExtender.Hide();

        }
        else
        {
            lblError_RejectExp.Text = "Please Upload Receipt!";
            lblError_RejectExp.CssClass = "errorMsg";

            RejectModalPopupExtender.Show();
        }
    }
    public string UploadDocument(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;

        FileName = FileName.Replace(",", "");

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
            DataFilter1.FilterSessionID = "PendingInvoicePaymentReceipt.aspx";
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
        string strFileName = "PendingInvoicePayment" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        DataFilter1.FilterSessionID = "PendingInvoicePaymentReceipt.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();


        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

}