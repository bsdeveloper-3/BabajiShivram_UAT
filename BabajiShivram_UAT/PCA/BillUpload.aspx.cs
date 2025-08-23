using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
//using Syncfusion.Pdf.Parsing;
public partial class PCA_BillUpload : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Bill Upload";

        if(!Page.IsPostBack)
        {
            Session["BillJobId"] = null;
        }

        DataFilter1.DataSource = DataSourceBillJob;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "BillUpload.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        hdnBillId.Value = "0";
        hdnJobId.Value = "0";

        string strBJVNo = "";
        string strBJVBillNo = "";
        
        if (e.CommandName.ToLower() == "jobselect")
        {
            Session["BillJobId"] = e.CommandArgument.ToString();

            Response.Redirect("BillDispatch.aspx");
        }
        else if (e.CommandName.ToLower() == "upload")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsGetBillDetail = BillingOperation.GetBJVBillDetailByID(BillId);

            if (dsGetBillDetail.Tables[0].Rows.Count > 0)
            {
                hdnBillId.Value = BillId.ToString();
                hdnJobId.Value = dsGetBillDetail.Tables[0].Rows[0]["JobId"].ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["BJVNo"] != null)
                {
                    lblBJVNumber.Text = dsGetBillDetail.Tables[0].Rows[0]["BJVNo"].ToString();
                }
                if (dsGetBillDetail.Tables[0].Rows[0]["INVNO"] != null)
                    lblBJVBillNo.Text = dsGetBillDetail.Tables[0].Rows[0]["INVNO"].ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["INVDATE"] != null)
                    lblBJVBillDate.Text = Convert.ToDateTime(dsGetBillDetail.Tables[0].Rows[0]["INVDATE"]).ToString("dd/MM/yyyy");
                if (dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"] != null)
                    lblBJVAmount.Text = dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"].ToString();

                BillUploadModalPopup.Show();
            }
            else
            {
                lblMessage.Text = "Bill Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
            }

        }
        else if (e.CommandName.ToLower() == "download")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                DownloadDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Document Not Uploaded!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(Convert.ToInt32(hdnJobId.Value), BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;

                ViewDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Document Not Uploaded!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DocId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DocId"));
                if (DocId == 0)
                {
                    CheckBox chkBillNo = (CheckBox)e.Row.FindControl("chkBillNo");
                    LinkButton lnkViewDoc = (LinkButton)e.Row.FindControl("lnkBillView");
                    LinkButton lnkBillDownload = (LinkButton)e.Row.FindControl("lnkBillDownload");

                    if (chkBillNo != null)
                    {
                        chkBillNo.Visible = false;
                    }
                    if (lnkViewDoc != null)
                    {
                        lnkViewDoc.Visible = false;
                    }
                    if (lnkBillDownload != null)
                    {
                        lnkBillDownload.Visible = false;
                    }


                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Document Not Uploaded";
                }
            }
        }
    }
   
    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            DataFilter1.FilterSessionID = "BillUpload.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Bill Document
    protected void btnUploadBill_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(hdnJobId.Value);
        int BillId = Convert.ToInt32(hdnBillId.Value);

        DateTime dtBJVBillDate = Commonfunctions.CDateTime(lblBJVBillDate.Text.Trim());

        string strDirPath = lblBJVNumber.Text.Replace("/", "");

        strDirPath = strDirPath.Replace("-", "");

        string strInvoiceFilePath = "BillDocument\\" + strDirPath + "\\";

        if (fuReceipt.HasFile)
        {
            string FileName10 = "";

            //if (chkPDFSizeReduce.Checked == true)
            //{
            //    FileName10 = SyncFusionPDF(fuReceipt, strInvoiceFilePath);
            //}
            //else
            //{
            //    FileName10 = UploadDocument(fuReceipt, strInvoiceFilePath);
            //}

            FileName10 = UploadDocument(fuReceipt, strInvoiceFilePath);

            if (FileName10 != "")
            {
                strInvoiceFilePath = strInvoiceFilePath + FileName10;

                int Result = BillingOperation.AddBillDocPath(JobId, BillId, FileName10, strInvoiceFilePath, 10, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblMessage.Text = "Bill Uploaded Successfully!.";
                    lblMessage.CssClass = "success";

                    BillUploadModalPopup.Hide();
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after sometime.";
                    lblMessage.CssClass = "errorMsg";

                    BillUploadModalPopup.Hide();
                }
                else if (Result == 2)
                {
                    lblMessage.Text = "Bill Already Uploaded!";
                    lblMessage.CssClass = "errorMsg";

                    BillUploadModalPopup.Hide();
                }
            }
            else
            {
                lblMessage.Text = "File Upload Error! Please try after sometime.";
                lblMessage.CssClass = "errorMsg";

                BillUploadModalPopup.Hide();
            }
        }
        else
        {
            lblMessage.Text = "Please Upload Bill!";
            lblMessage.CssClass = "errorMsg";

            BillUploadModalPopup.Show();
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
    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..//UploadFiles\\" + DocumentPath);
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
    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Bill_Upload_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
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

        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;

        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;

        gvJobDetail.Columns[10].Visible = false;
        gvJobDetail.Columns[11].Visible = false;
        gvJobDetail.Columns[12].Visible = false;

        DataFilter1.FilterSessionID = "BillUpload.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();


    }
    #endregion

    /******************* SyncFusion ***************************
    private string SyncFusionPDF(FileUpload fuSyncFusion, string strFilePath)
    {
        string FileName = fuSyncFusion.FileName;

        FileName = FileName.Replace(",", "");

        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + strFilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + strFilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuSyncFusion.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            Stream stream1 = fuSyncFusion.PostedFile.InputStream;

            //Load a existing PDF document
            PdfLoadedDocument ldoc = new PdfLoadedDocument(stream1);

            //Create a new PDF compression options
            PdfCompressionOptions options = new PdfCompressionOptions();

            options.OptimizeFont = true;

            options.CompressImages = true;

            //if (this.compressImage.Checked)
            //{
            //    //Compress image.
            //    options.CompressImages = true;
            //    options.ImageQuality = int.Parse(this.imageQuality.SelectedValue);
            //}
            //else
            //{
            //    options.CompressImages = false;
            //}
            ////Compress the font data
            //if (this.optFont.Checked)
            //{
            //    options.OptimizeFont = true;
            //}
            //else
            //{
            //    options.OptimizeFont = false;
            //}
            ////Compress the page contents
            //if (this.optPageContents.Checked)
            //{
            //    options.OptimizePageContents = true;
            //}
            //else
            //{
            //    options.OptimizePageContents = false;
            //}

            //Remove the metadata information.    
            //if (this.removeMetadata.Checked)
            //{
            //    options.RemoveMetadata = true;
            //}
            //else
            //{
            //    options.RemoveMetadata = false;
            //}

            //Set the options to loaded PDF document
            ldoc.CompressionOptions = options;

            //Save to disk
            //if (this.CheckBox1.Checked)
            //{
            //    ldoc.Save("Document1.pdf", Response, HttpReadType.Open);lblBJVNumber
            //}
            //else
            //{
            //    ldoc.Save("Document1.pdf", Response, HttpReadType.Save);

            //}

            //ldoc.Save(strFilePath, Response, HttpReadType.Save);

            // ADD BOE Copy

            //Stream streamBOE = fuBOE.PostedFile.InputStream;

            //string strBOEPath = @"C:\inetpub\wwwroot\BabajiShivram_20210601\UploadFiles\BE_391353712052021INNSA1BE0120520211212.pdf";
            //Stream streamBOE = File.OpenRead(strBOEPath);

            //Load a BOE PDF document
            //PdfLoadedDocument ldocBOE = new PdfLoadedDocument(strBOEPath);

            //ldocBOE.Save(ServerFilePath + FileName);

            //ldoc.Append(ldocBOE);

            //ldoc.ImportPageRange(ldocBOE, 0, ldocBOE.Pages.Count - 1);

            ldoc.Save(ServerFilePath + FileName);
                        
            return FileName;
        }
        else
        {
            FileName = "";

            return FileName;
        }
    }

    ****************************/
}