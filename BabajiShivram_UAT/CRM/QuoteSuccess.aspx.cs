using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class CRM_QuoteSuccess : System.Web.UI.Page
{
    string QuotationRefNo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["op"] != null)
                {
                    int option = Convert.ToInt32(Request.QueryString["op"].ToString());
                    if (option == 1)   // SAVED QUOTATION MESSAGE
                    {
                        if (Request.QueryString["id"] != null)
                        {
                            int QuotationId = Convert.ToInt32(Request.QueryString["id"]);
                            DataSet dsGetQuoteRefNo = QuotationOperations.GetParticularQuotation(QuotationId);
                            if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                                QuotationRefNo = dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString();

                            lblTitle.Text = "New Quotation";
                            lblError.Text = "Successfully Saved Quotation - " + QuotationRefNo.Trim() + " .";
                            lblError.CssClass = "success";

                            if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuotePath"].ToString() != "")
                                hdnPath.Value = dsGetQuoteRefNo.Tables[0].Rows[0]["QuotePath"].ToString();
                        }
                    }
                    else if (option == 5)
                    {
                        lblError.Text = "Successfully Saved Quotation - " + QuotationRefNo.Trim() + " .";
                        lblError.CssClass = "success";
                        imgbtnDownloadCopy.Visible = false;
                    }
                    else  // EDIT QUOTATION MESSAGE
                    {
                        if (Request.QueryString["id"] != null)
                        {
                            int QuotationId = Convert.ToInt32(Request.QueryString["id"]);
                            DataSet dsGetQuoteRefNo = QuotationOperations.GetParticularQuotation(QuotationId);
                            if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                                QuotationRefNo = dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString();

                            lblTitle.Text = "Edit Quotation";
                            lblError.Text = "Successfully Saved Quotation - " + QuotationRefNo.Trim() + " As Draft.";
                            lblError.CssClass = "success";

                            if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuotePath"].ToString() != "")
                                hdnPath.Value = dsGetQuoteRefNo.Tables[0].Rows[0]["QuotePath"].ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("QuoteDashboard.aspx");
                }
            }
            else
            {
                Response.Redirect("QuoteDashboard.aspx");
            }
        }
    }

    protected void imgbtnDownloadCopy_OnClick(Object Sender, EventArgs e)
    {
        if (hdnPath.Value != "")
        {
            //DocumentPath =  QuotationOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
            string ServerPath = FileServer.GetFileServerDir();

            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + hdnPath.Value);
            }
            else
            {
                ServerPath = ServerPath + "Quotation\\" + hdnPath.Value;
            }
            try
            {
                HttpResponse response = Page.Response;
                FileDownload.Download(response, ServerPath, hdnPath.Value);
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            lblError.Text = "PDF Path Not Found..!!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;
        }

        // try
        //{
        HttpResponse response = Page.Response;
        Download(response, ServerPath, DocumentPath);
        // }
        //catch (Exception ex)
        //{
        //}
        //finally
        //{
        //    Response.End();
        //}
    }

    public static void Download(HttpResponse Response, string filepath, string FileName)
    {
        if (filepath != null)
        {
            if (File.Exists(filepath))
            {
                string filename = Path.GetFileName(filepath);
                byte[] bts = System.IO.File.ReadAllBytes(filepath);
                MemoryStream ms = new MemoryStream(bts);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.Write(filepath);
            }
        }
    }
}