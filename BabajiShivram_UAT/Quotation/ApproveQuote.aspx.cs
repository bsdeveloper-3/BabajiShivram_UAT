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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class Quotation_ApproveQuote : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    public string imgPath, imgLogout, LoginPath;
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["LumSumAmt"] = "0";
        ViewState["MinAmt"] = "0";
        if (Request.QueryString.Count > 0)
        {
            if (Convert.ToString(Request.QueryString["id"]) != null)
            {
                Session["QuotationId"] = Request.QueryString["id"];
            }
        }

        if (Convert.ToString(Session["QuotationId"]) != "")
        {
            GetQuoteDetails(Convert.ToInt32(Session["QuotationId"]));
            DataSet dsGetQuoteDetails = QuotationOperations.GetQuoteReportData(Convert.ToInt32(Session["QuotationId"]));
            if (dsGetQuoteDetails != null)
            {
                gvQuote.DataSource = dsGetQuoteDetails;
                gvQuote.DataBind();
            }
        }
        imgPath = System.Web.VirtualPathUtility.ToAbsolute("~/images/Babji-Logo.png");
    }

    protected void GetQuoteDetails(int QuotationId)
    {
        DataSet dsGetQuote = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuote != null)
        {
            if (dsGetQuote.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                lblQuoteRefNo.Text = dsGetQuote.Tables[0].Rows[0]["QuoteRefNo"].ToString();
            if (dsGetQuote.Tables[0].Rows[0]["CustomerName"].ToString() != "")
                lblCust.Text = dsGetQuote.Tables[0].Rows[0]["CustomerName"].ToString();
            if (dsGetQuote.Tables[0].Rows[0]["IsLumpSumCode"].ToString() != "")
            {
                lblLumpSumCode.Text = dsGetQuote.Tables[0].Rows[0]["IsLumpSumCode"].ToString();
                Boolean IsLumpSumCode = Convert.ToBoolean(dsGetQuote.Tables[0].Rows[0]["IsLumpSumCode"].ToString());

            }
        }
    }

    protected void gvQuote_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double number;
        decimal MinAmt = 0;
        if (lblLumpSumCode.Text.Trim().ToString() != "")
        {
            if (lblLumpSumCode.Text.Trim().ToString().ToLower() == "true") //true
            {
                gvQuote.Columns[2].Visible = false;
                tblTotal.Visible = false;
                tbltotal_Lp.Visible = true;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Double.TryParse(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "LumpSumAmt")), out number))
                    {
                        decimal LumpSumAmt = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "LumpSumAmt"));

                        if (DataBinder.Eval(e.Row.DataItem, "MinAmt") != DBNull.Value)
                            MinAmt = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MinAmt"));
                        if (LumpSumAmt != 0)
                        {
                            ViewState["LumSumAmt"] = Convert.ToDecimal(ViewState["LumSumAmt"]) + LumpSumAmt;
                        }

                        if (MinAmt != 0)
                        {
                            ViewState["MinAmt"] = Convert.ToDecimal(ViewState["MinAmt"]) + MinAmt;
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#cbcbdc");
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "<b>Total</b>";
                    e.Row.Cells[1].Font.Bold = true;
                    e.Row.Cells[1].ColumnSpan = 1;
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "<b>" + ViewState["LumSumAmt"].ToString() + "</b>";
                    e.Row.Cells[4].Text = "<b>" + ViewState["MinAmt"].ToString() + "</b>";
                }

                lblTotal2.Text = ViewState["LumSumAmt"].ToString();
                lblMinTotal2.Text = ViewState["MinAmt"].ToString();
            }
            else
            {
                gvQuote.Columns[3].Visible = false;
                tblTotal.Visible = true;
                tbltotal_Lp.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Boolean IsValidAmount = false;
                    IsValidAmount = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsValidAmount"));
                    if (e.Row.Cells[4].Text != "&nbsp;")
                    {
                        if (IsValidAmount == false)
                        {
                            e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                            // e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                            e.Row.Cells[2].Font.Bold = true;
                            // e.Row.Cells[4].Font.Bold = true;
                            e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#cbcbdc");
                }
            }
        }
    }

    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QuotationId"]) != null)
        {
            int QuotationId = 0;
            QuotationId = Convert.ToInt32(Session["QuotationId"].ToString());

            if (QuotationId != 0)
            {
                int result = QuotationOperations.UpdateDraftApprovalStatus(QuotationId, 3, LoggedInUser.glUserId);
                if (result == 1)
                {
                    if (QuotationId != 0)
                        UpdateQuotePDF(QuotationId);
                    lblError.Text = "Successfully Approved Quotation.";
                    lblError.CssClass = "success";
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (result == -1)
                {
                    lblError.Text = "Quotation does not exists..!!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System error! Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void btnReject_OnClick(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QuotationId"]) != null)
        {
            int QuotationId = 0;
            QuotationId = Convert.ToInt32(Session["QuotationId"].ToString());

            if (QuotationId != 0)
            {
                int result = QuotationOperations.UpdateDraftApprovalStatus(QuotationId, 4, LoggedInUser.glUserId);
                if (result == 1)
                {

                    lblError.Text = "Quotation has been rejected..!!";
                    lblError.CssClass = "success";
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (result == -1)
                {
                    lblError.Text = "Quotation does not exists..!!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System error! Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void UpdateQuotePDF(int QuotationId)
    {
        string CustomerName = "", TodaysDate = "";
        Boolean IsValidQuote = false;

        DataSet dsGetQuotationDetails = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuotationDetails != null)
        {
            if (dsGetQuotationDetails.Tables[0].Rows[0]["IsValidDraft"] != DBNull.Value)
            {
                if (dsGetQuotationDetails.Tables[0].Rows[0]["IsValidDraft"].ToString().ToLower().Trim() == "true")
                {
                    IsValidQuote = true;
                }
            }

            if (dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value && dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString() != "")
            {
                CustomerName = dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString();
            }

            Boolean IsLumpSumCode = false;
            if (dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"] != DBNull.Value)
            {
                if (Convert.ToBoolean(dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"]) == true)
                    IsLumpSumCode = true;
            }

            string QuoteRefNo = dsGetQuotationDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
            string CustAddress = dsGetQuotationDetails.Tables[0].Rows[0]["OrganisationAddress"].ToString();
            string PersonName = dsGetQuotationDetails.Tables[0].Rows[0]["AttendedPerson"].ToString();
            string Subject = dsGetQuotationDetails.Tables[0].Rows[0]["Subject"].ToString();
            string BodyContent = dsGetQuotationDetails.Tables[0].Rows[0]["BodyContent"].ToString();

            if (BodyContent == "")
            {
                BodyContent = "This is in reference to the above subject; we are pleased to offer you our most competitive rates as below:";
            }

            string PaymentTerms = dsGetQuotationDetails.Tables[0].Rows[0]["PaymentTerms"].ToString();

            if (dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"] != DBNull.Value)
            {
                TodaysDate = Convert.ToDateTime(dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"]).ToShortDateString();
            }

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            string date = DateTime.Today.ToShortDateString();

            DataSet dsGetReportData = QuotationOperations.GetQuoteReportData(QuotationId);
            if (dsGetReportData != null)
            {
                if (dsGetReportData.Tables[0].Rows.Count > 0)
                {
                    int i = 0;
                    string filePath = UploadFiles(CustomerName.Replace("&amp;", "").ToString()) + ".pdf";
                    string FileFullPath = Server.MapPath("..\\UploadFiles\\Quotation\\" + filePath);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
                    Document pdfDoc = new Document(recPDF);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, new FileStream(FileFullPath, FileMode.Create));
                    pdfDoc.Open();

                    // PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                    logo.SetAbsolutePosition(380, 700);
                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    if (IsValidQuote == true)
                    {
                        string contents = "";
                        contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                        contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                        contents = contents.Replace("[Today's Date]", date.ToString());
                        contents = contents.Replace("[CustomerName]", CustomerName);
                        contents = contents.Replace("[CustomerAddress]", CustAddress);
                        contents = contents.Replace("[PersonName]", PersonName);
                        contents = contents.Replace("[Subject]", Subject);
                        contents = contents.Replace("[BodyContent]", BodyContent);

                        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                        foreach (var htmlelement in parsedContent)
                            pdfDoc.Add(htmlelement as IElement);

                        PdfPTable pdftable = new PdfPTable(3);

                        pdftable.TotalWidth = 500f;
                        pdftable.LockedWidth = true;
                        float[] widths = new float[] { 0.18f, 1.2f, 0.8f };
                        pdftable.SetWidths(widths);
                        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Set Table Spacing Before And After html text
                        pdftable.SpacingBefore = 10f;
                        pdftable.SpacingAfter = 20f;

                        #region Create Table Column Header Cell with Text
                        // Header: Serial Number
                        PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                        cellwithdata.Colspan = 1;
                        cellwithdata.BorderWidth = 1f;
                        cellwithdata.HorizontalAlignment = 0;//left
                        cellwithdata.VerticalAlignment = 0;// Center
                        cellwithdata.Padding = 5;
                        cellwithdata.BackgroundColor = BaseColor.GRAY;
                        pdftable.AddCell(cellwithdata);

                        // Header: Particulars
                        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellwithdata1.Colspan = 1;
                        cellwithdata1.BorderWidth = 1f;
                        cellwithdata1.HorizontalAlignment = 0;
                        cellwithdata1.Padding = 5;
                        cellwithdata1.BackgroundColor = BaseColor.GRAY;
                        //cellwithdata.VerticalAlignment = 0;// Center
                        pdftable.AddCell(cellwithdata1);

                        // Header: Charges Applicable
                        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellwithdata2.Colspan = 1;
                        cellwithdata2.BorderWidth = 1f;
                        cellwithdata2.HorizontalAlignment = 0;
                        cellwithdata2.Padding = 5;
                        cellwithdata2.BackgroundColor = BaseColor.GRAY;
                        //cellwithdata.VerticalAlignment = Element.ALIGN_RIGHT;// Center
                        //cellwithdata2.BackgroundColor = GrayColor.LIGHT_GRAY;
                        pdftable.AddCell(cellwithdata2);

                        #endregion

                        #region ADD CELL TO DATATABLE IN PDF
                        int SrNo = 1;

                        foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                        {
                            // Data Cell: Serial Number - Auto Increment Cell
                            PdfPCell SrnoCell = new PdfPCell();
                            SrnoCell.Colspan = 1;
                            SrnoCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                            SrnoCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                            // Data Cell: Particulars Cell
                            PdfPCell ParticularCell = new PdfPCell();
                            ParticularCell.Colspan = 1;

                            // Data Cell:  Charges Applicable
                            PdfPCell ChargesCell = new PdfPCell();
                            ChargesCell.Colspan = 1;
                            ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;

                            TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                            i = i + 1;

                            if (IsLumpSumCode == true)
                            {
                                if (Convert.ToBoolean(dsGetReportData.Tables[0].Rows[i - 1]["IsLumpSumField"]).ToString().ToLower().Trim() == "true")
                                {
                                    // Serial number #
                                    SrnoCell.Phrase = new Phrase(Convert.ToString(SrNo), TextFontformat);
                                    SrnoCell.Padding = 5;
                                    pdftable.AddCell(SrnoCell);

                                    // Particulars
                                    ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                    ParticularCell.Padding = 5;
                                    pdftable.AddCell(ParticularCell);

                                    // Charges Applicable
                                    ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat);
                                    ChargesCell.Padding = 5;
                                    pdftable.AddCell(ChargesCell);
                                    SrNo = SrNo + 1;
                                }
                            }
                            else
                            {
                                // Serial number #
                                SrnoCell.Phrase = new Phrase(Convert.ToString(SrNo), TextFontformat);
                                SrnoCell.Padding = 5;
                                pdftable.AddCell(SrnoCell);

                                // Particulars
                                ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.Padding = 5;
                                pdftable.AddCell(ParticularCell);

                                // Charges Applicable
                                ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat);
                                ChargesCell.Padding = 5;
                                pdftable.AddCell(ChargesCell);
                                SrNo = SrNo + 1;
                            }
                        }

                        #endregion

                        pdfDoc.Add(pdftable);

                        #region PAYMENT TERMS
                        if (PaymentTerms != "")
                        {
                            PdfPTable tblTerms = new PdfPTable(1);
                            tblTerms.TotalWidth = 500f;
                            tblTerms.LockedWidth = true;
                            float[] widths_Terms = new float[] { 2.2f };
                            tblTerms.SetWidths(widths_Terms);
                            tblTerms.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Set Table Spacing Before And After html text
                            tblTerms.SpacingBefore = 10f;
                            tblTerms.SpacingAfter = 4f;

                            // Create Table Column Header Cell with Text
                            // Header: Serial Number
                            PdfPCell cell = new PdfPCell(new Phrase("Payment Terms", GridHeadingFont));
                            cell.Colspan = 1;
                            cell.Border = Rectangle.NO_BORDER;
                            cell.HorizontalAlignment = 0;//left
                            cell.VerticalAlignment = 0;// Center
                            cell.Padding = 5;
                            cell.BackgroundColor = BaseColor.GRAY;
                            tblTerms.AddCell(cell);

                            // Data Cell: Payment Terms
                            PdfPCell Terms = new PdfPCell();
                            Terms.Colspan = 1;
                            Terms.HorizontalAlignment = Element.ALIGN_LEFT;
                            Terms.VerticalAlignment = Element.ALIGN_LEFT;

                            // Payment Terms
                            Terms.Phrase = new Phrase(Convert.ToString(PaymentTerms), TextFontformat);
                            Terms.Padding = 5;
                            tblTerms.AddCell(Terms);
                            pdfDoc.Add(tblTerms);
                        }
                        #endregion

                        Paragraph ParaSpacing = new Paragraph();
                        ParaSpacing.SpacingBefore = 20;//5

                        pdfDoc.Add(new Paragraph("    We hope the above rates are in order and look forward for logn lasting association with your esteemed ", TextFontformat));
                        pdfDoc.Add(new Paragraph("    organization.", TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    " + dsGetReportData.Tables[0].Rows[0]["CreatedBy"].ToString(), TextFontformat));
                        pdfDoc.Add(new Paragraph("    Authorized Signatory", GridHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        // Footer Image Commented
                        iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterET5.png"));
                        //footer = 50;
                        footer.SetAbsolutePosition(30, 0);
                        pdfDoc.Add(footer);
                    }

                    htmlparser.Parse(sr);
                    if (QuotationId != 0 && filePath != "")
                    {
                        int DocPath = QuotationOperations.AddQuotationCopy(QuotationId, filePath);
                    }
                    pdfDoc.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
    }

    protected string UploadFiles(string GetFileName)
    {
        string FileName = GetFileName;
        FileName = FileName.Replace(".", "");

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\");
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Quotation\\";
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            //FU.SaveAs(ServerFilePath + FileName);

            //return FilePath + FileName;
            return FileName;
        }
        else
        {
            return "";
        }
    }

    protected string RandomString(int size)
    {

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
}