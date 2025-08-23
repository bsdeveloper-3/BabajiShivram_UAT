using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using AjaxControlToolkit;
using QueryStringEncryption;
using Ionic.Zip;
using ClosedXML.Excel;


public partial class CRM_Quote : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLeads);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Quote";

            if (gvLeads.Rows.Count == 0)
            {
                lblError.Text = "No Data Found For Quote!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceLeads;
        DataFilter1.DataColumns = gvLeads.Columns;
        DataFilter1.FilterSessionID = "Quote.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLeads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[14].ToolTip = "KYC registered - Today's date";
            LinkButton lnkAddQuote = (LinkButton)e.Row.FindControl("lnkAddQuote");
            LinkButton lnkbtnStatus = (LinkButton)e.Row.FindControl("lnkbtnStatus");
            ImageButton imgbtnEditQuote = (ImageButton)e.Row.FindControl("imgbtnEditQuote");
            ImageButton imgbtnDownloadQuote = (ImageButton)e.Row.FindControl("imgbtnDownloadQuote");

            if (DataBinder.Eval(e.Row.DataItem, "QuotationId") == DBNull.Value)
            {
                lnkAddQuote.Visible = true;
                lnkbtnStatus.Visible = false;
                imgbtnEditQuote.Visible = false;
            }
            else
            {
                lnkAddQuote.Visible = false;
                lnkbtnStatus.Visible = true;
            }

            if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId")) > 10) // customer approved & kyc approval pending
                {
                    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId")) != 13)
                    {
                        imgbtnEditQuote.Visible = false;
                        lnkAddQuote.Visible = false;
                        lnkbtnStatus.Enabled = false;
                    }
                }
            }
            else
            {
                imgbtnDownloadQuote.Visible = false;
            }

            if (DataBinder.Eval(e.Row.DataItem, "RfqReceived") != DBNull.Value)
            {
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "RfqReceived")).ToLower().Trim() == "yes")
                {
                    e.Row.Cells[1].Text = "";
                }
            }

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton lnkImage = (LinkButton)e.Row.FindControl("lnkAddQuote");

            //    lnkImage.Attributes.Add("onClick", "javascript:window.open('PopUpWindow.aspx?Id=" + "',null,'left=162px, top=134px, width=500px, height=500px, status=no,              resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no');");
            //}

        }
    }

    protected void gvLeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "addquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["LeadId"] = commandArgs[1].ToString();
            //string RFQRecd = commandArgs[1].ToString();
            //int ExistEnquiry = Convert.ToInt32(commandArgs[2].ToString());
            //Session["EnquiryId"] = commandArgs[3].ToString();
            lbl_LeadRefNo.Text = commandArgs[0].ToString();

            //Session["LeadId"] = e.CommandArgument.ToString();
            //if (ExistEnquiry == 2)
            //{
            //    Response.Redirect("CustQuote.aspx");
            //}
            //else
            //{
            //    if (RFQRecd.ToLower().Trim() == "yes")
            //    {
            //        Response.Redirect("RfqQuote.aspx");
            //    }
            //    else
            //        Response.Redirect("AddQuote.aspx");
            //}
        }
        else if (e.CommandName.ToLower().Trim() == "viewlead")
        {
            Session["LeadId"] = e.CommandArgument.ToString();
            Response.Redirect("LeadDetail.aspx");
        }
        else if (e.CommandName.ToLower() == "downloadquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string DocPath = commandArgs[1].ToString();
            
            if (DocPath != "")
            {
                DownloadDoc(DocPath);
            }
            else
            {
                DownloadQuotation(Convert.ToInt16(commandArgs[0]));
            }
        }
        else if (e.CommandName.ToLower() == "getstatus")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnQuotationId.Value = commandArgs[0].ToString();
            lblLeadRefNo.Text = commandArgs[1].ToString();
            hdnEnquiryId.Value = commandArgs[2].ToString();
            hdnExistEnquiry.Value = commandArgs[3].ToString();

            if (hdnQuotationId.Value != "0" && hdnQuotationId.Value != "")
            {
                lbError_Popup.Text = "";
                lbError_Popup.CssClass = "";
                ModalPopupDocument.Show();
            }
        }
        else if (e.CommandName.ToLower() == "editquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            int StatusId = 0;

            Session["QuotationId"] = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                StatusId = Convert.ToInt32(commandArgs[1].ToString());

            if (StatusId == 1) // Draft Quotation
                Response.Redirect("DraftQuote.aspx");
            else if (StatusId == 4 || StatusId == 8 || StatusId == 9) // Rejected or negotiated quote or invalid quote
                Response.Redirect("EditQuote.aspx");
            else if (StatusId > 4 || StatusId == 3)
                Response.Redirect("../Quotation/ApprovedQuote.aspx");
            else
                Response.Redirect("EditQuote.aspx");
        }
    }


    protected void DownloadQuotation(int QuotationId)
    {
        string CustomerName = "", TodaysDate = "", BodyContent_Addtnl = "";
        Boolean IsValidQuote = false, IsLumpSumCode = false;
        int StatusId = 0;

        DataSet dsGetQuotationDetails = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuotationDetails != null)
        {
            if (Convert.ToBoolean(dsGetQuotationDetails.Tables[0].Rows[0]["IsValidDraft"].ToString()) == true)
                IsValidQuote = true;

            if (dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value && dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString() != "")
                CustomerName = dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString();

            if (Convert.ToBoolean(dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"]) == true)
                IsLumpSumCode = true;

            string date = DateTime.Today.ToShortDateString();
            string QuoteRefNo = dsGetQuotationDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
            string AddressLine1 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine1"].ToString();
            string AddressLine2 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine2"].ToString();
            string AddressLine3 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine3"].ToString();
            string PersonName = dsGetQuotationDetails.Tables[0].Rows[0]["AttendedPerson"].ToString();
            string Subject = dsGetQuotationDetails.Tables[0].Rows[0]["Subject"].ToString();
            string BodyContent = dsGetQuotationDetails.Tables[0].Rows[0]["BodyContent"].ToString();
            string SignImgPath = dsGetQuotationDetails.Tables[0].Rows[0]["SignImgPath"].ToString();
            string PaymentTerms = dsGetQuotationDetails.Tables[0].Rows[0]["PaymentTerms"].ToString();
            int TermConditionId = Convert.ToInt32(dsGetQuotationDetails.Tables[0].Rows[0]["TermConditionId"].ToString());
            if (dsGetQuotationDetails.Tables[0].Rows[0]["StatusId"].ToString() != "")
                StatusId = Convert.ToInt32(dsGetQuotationDetails.Tables[0].Rows[0]["StatusId"].ToString());

            if (BodyContent == "")
                BodyContent = "This is in reference to the above subject; we are pleased to offer you our most competitive rates as below:";
            else
            {
                BodyContent_Addtnl = BodyContent;
                BodyContent = "";
            }

            if (dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"] != DBNull.Value)
                TodaysDate = Convert.ToDateTime(dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"]).ToShortDateString();

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF.jpg"));
            DataSet dsGetReportData = QuotationOperations.GetQuoteReportData(QuotationId);
            if (dsGetReportData != null)
            {
                if (dsGetReportData.Tables[0].Rows.Count > 0)
                {
                    int i = 0;
                    string Name = CustomerName.Replace("&amp;", "").ToString();
                    string filePath = Name.Replace(".", "");
                    filePath = GetFormattedName(filePath);
                    filePath = GetQuoteFileName(filePath);
                    string FileFullPath = GetQuotePath(filePath);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
                    Document pdfDoc = new Document(recPDF);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    FileStream fs = new FileStream(FileFullPath, FileMode.Create, FileAccess.Write);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, fs);
                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                    logo.SetAbsolutePosition(415, 715);
                    logo.ScaleAbsoluteHeight(100);
                    logo.ScaleAbsoluteWidth(130);
                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    if (IsValidQuote == true)
                    {
                        if (StatusId == 1)
                        {
                            #region ADD WATERMARK

                            //string imageFilePath = Server.MapPath("~/Images/Draft.png");
                            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                            //jpg.ScaleToFit(3000, 770);  // For give the size to image
                            //jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                            //jpg.SetAbsolutePosition(7, 69);  // give absolute/specified fix position to image.
                            //pdfwriter.PageEvent = new ImageBackgroundHelper(jpg);
                            //pdfDoc.Add(jpg);

                            #endregion
                        }

                        string contents = "";
                        contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                        contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                        contents = contents.Replace("[Today's Date]", date.ToString());
                        contents = contents.Replace("[CustomerName]", CustomerName);
                        contents = contents.Replace("[AddressLine1]", AddressLine1);
                        if (AddressLine2 == String.Empty)
                            contents = contents.Replace("[AddressLine2]", String.Empty);
                        else
                            contents = contents.Replace("[AddressLine2]", AddressLine2);
                        if (AddressLine3 == String.Empty)
                            contents = contents.Replace("[AddressLine3]", String.Empty);
                        else
                            contents = contents.Replace("[AddressLine3]", AddressLine3);
                        contents = contents.Replace("[PersonName]", PersonName);
                        contents = contents.Replace("[Subject]", Subject);
                        contents = contents.Replace("[BodyContent]", BodyContent);
                        contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

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
                        cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata);

                        // Header: Particulars
                        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellwithdata1.Colspan = 1;
                        cellwithdata1.BorderWidth = 1f;
                        cellwithdata1.HorizontalAlignment = 0;
                        cellwithdata1.Padding = 5;
                        cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata1);

                        // Header: Charges Applicable
                        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellwithdata2.Colspan = 1;
                        cellwithdata2.BorderWidth = 1f;
                        cellwithdata2.HorizontalAlignment = 0;
                        cellwithdata2.Padding = 5;
                        cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata2);

                        #endregion

                        #region ADD CELL TO DATATABLE IN PDF
                        int SrNo = 1;
                        foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                        {
                            // Data Cell: Serial Number - Auto Increment Cell
                            PdfPCell SrnoCell = new PdfPCell();
                            SrnoCell.Colspan = 1;
                            SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;

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
                                    SrnoCell.PaddingTop = 8;
                                    SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdftable.AddCell(SrnoCell);

                                    // Particulars
                                    Paragraph paraParticulars = new Paragraph();
                                    paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                    ParticularCell.AddElement(paraParticulars);
                                    //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                    ParticularCell.PaddingBottom = 8;
                                    ParticularCell.PaddingLeft = 5;
                                    ParticularCell.PaddingRight = 5;
                                    pdftable.AddCell(ParticularCell);

                                    // Charges Applicable
                                    Paragraph paraChargesApp = new Paragraph();
                                    paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat));
                                    ChargesCell.AddElement(paraChargesApp);
                                    //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat);
                                    ChargesCell.PaddingBottom = 8;
                                    ChargesCell.PaddingLeft = 5;
                                    ChargesCell.PaddingRight = 5;
                                    pdftable.AddCell(ChargesCell);
                                    SrNo = SrNo + 1;
                                }
                            }
                            else
                            {
                                // Serial number #
                                SrnoCell.Phrase = new Phrase(Convert.ToString(SrNo), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingTop = 8;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                pdftable.AddCell(SrnoCell);

                                // Particulars
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);

                                //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;
                                pdftable.AddCell(ParticularCell);

                                // Charges Applicable
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);

                                //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;
                                pdftable.AddCell(ChargesCell);
                                SrNo = SrNo + 1;
                            }
                        }

                        #endregion
                        pdfDoc.Add(pdftable);
                    }
                    else
                    {
                        if (IsLumpSumCode == true)
                        {
                            #region LUMP SUM CODE

                            #region ADD WATERMARK

                            //string imageFilePath = Server.MapPath("~/Images/Draft.png");

                            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                            ////Resize image depend upon your need
                            ////For give the size to image
                            //jpg.ScaleToFit(3000, 770);

                            ////If you want to choose image as background then,
                            //jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                            ////If you want to give absolute/specified fix position to image.
                            //jpg.SetAbsolutePosition(7, 69);
                            //pdfDoc.Add(jpg);
                            #endregion

                            string contents = "";
                            contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                            contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                            contents = contents.Replace("[Today's Date]", date.ToString());
                            contents = contents.Replace("[CustomerName]", CustomerName);
                            contents = contents.Replace("[AddressLine1]", AddressLine1);
                            if (AddressLine2 == String.Empty)
                                contents = contents.Replace("[AddressLine2]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine2]", AddressLine2);
                            if (AddressLine3 == String.Empty)
                                contents = contents.Replace("[AddressLine3]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine3]", AddressLine3);
                            contents = contents.Replace("[PersonName]", PersonName);
                            contents = contents.Replace("[Subject]", Subject);
                            contents = contents.Replace("[BodyContent]", BodyContent);
                            contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

                            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                            foreach (var htmlelement in parsedContent)
                                pdfDoc.Add(htmlelement as IElement);

                            PdfPTable pdftable = new PdfPTable(3);
                            pdftable.TotalWidth = 500f;
                            pdftable.LockedWidth = true;
                            float[] widths = new float[] { 0.18f, 1.1f, 0.9f };
                            pdftable.SetWidths(widths);
                            pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Set Table Spacing Before And After html text
                            pdftable.SpacingBefore = 10f;
                            pdftable.SpacingAfter = 4f;

                            #region  Create Table Column Header Cell with Text
                            // Header: Serial Number
                            PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                            cellwithdata.Colspan = 1;
                            cellwithdata.BorderWidth = 1f;
                            cellwithdata.HorizontalAlignment = 0;//left
                            cellwithdata.VerticalAlignment = 0;// Center
                            cellwithdata.Padding = 5;
                            cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata);

                            // Header: Particulars
                            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                            cellwithdata1.Colspan = 1;
                            cellwithdata1.BorderWidth = 1f;
                            cellwithdata1.HorizontalAlignment = 0;
                            cellwithdata1.Padding = 5;
                            cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata1);

                            // Header: Charges Applicable
                            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                            cellwithdata2.Colspan = 1;
                            cellwithdata2.BorderWidth = 1f;
                            cellwithdata2.HorizontalAlignment = 0;
                            cellwithdata2.Padding = 5;
                            cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata2);

                            #endregion

                            #region ADD CELL TO DATATABLE IN PDF
                            Decimal dcTotal = 0, dcMinTotal = 0;
                            foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                            {
                                i = i + 1;
                                TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                                // Data Cell: Serial Number - Auto Increment Cell
                                PdfPCell SrnoCell = new PdfPCell();
                                SrnoCell.Colspan = 1;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingBottom = 8;
                                pdftable.AddCell(SrnoCell);

                                // Data Cell: Particulars Cell
                                PdfPCell ParticularCell = new PdfPCell();
                                ParticularCell.Colspan = 1;
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);
                                // ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;
                                pdftable.AddCell(ParticularCell);

                                // Data Cell:  Charges Applicable
                                PdfPCell ChargesCell = new PdfPCell();
                                ChargesCell.Colspan = 1;
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);
                                ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;
                                // ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;
                                pdftable.AddCell(ChargesCell);

                                if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LumpSumAmt"]) != "")
                                {
                                    decimal LumpSumAmt = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LumpSumAmt"]));
                                    if (LumpSumAmt != 0)
                                        dcTotal = dcTotal + LumpSumAmt;
                                }

                                if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinAmt"]) != "")
                                {
                                    decimal Mintotal = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinAmt"]));
                                    if (Mintotal != 0)
                                        dcMinTotal = dcMinTotal + Mintotal;
                                }
                            }
                            #endregion
                            pdfDoc.Add(pdftable);

                            #region ADD TOTAL ROW

                            PdfPTable table_Total = new PdfPTable(3);
                            table_Total.TotalWidth = 500f;
                            table_Total.LockedWidth = true;
                            float[] widths2 = new float[] { 0.18f, 1.1f, 0.9f };
                            table_Total.SetWidths(widths2);
                            table_Total.HorizontalAlignment = Element.ALIGN_CENTER;
                            table_Total.SpacingBefore = 0f;
                            table_Total.SpacingAfter = 20f;

                            // 1st Data Cell: Blank column
                            PdfPCell Cell1 = new PdfPCell();
                            Cell1.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell1);

                            // 2nd Data Cell: Blank column
                            PdfPCell Cell2 = new PdfPCell();
                            Cell2.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell2);

                            // 3rd Data Cell: Total
                            PdfPCell Total = new PdfPCell();
                            Total.Colspan = 1;
                            Total.HorizontalAlignment = 0;
                            Total.Phrase = new Phrase(Convert.ToString("Total:" + " " + dcTotal), GridHeadingFont);
                            Total.Padding = 5;
                            if (dcTotal < dcMinTotal)
                                Total.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                            table_Total.AddCell(Total);

                            // 1st Data Cell: Blank column
                            PdfPCell Cell3 = new PdfPCell();
                            Cell3.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell3);

                            // 2nd Data Cell: Blank column
                            PdfPCell Cell4 = new PdfPCell();
                            Cell4.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell4);

                            // 3rd Data Cell: Min Total
                            PdfPCell MinTotal = new PdfPCell();
                            MinTotal.Colspan = 1;
                            MinTotal.HorizontalAlignment = 0;
                            MinTotal.Phrase = new Phrase(Convert.ToString("Minimum Total:" + " " + dcMinTotal), GridHeadingFont);
                            MinTotal.Padding = 5;
                            table_Total.AddCell(MinTotal);
                            pdfDoc.Add(table_Total);

                            #endregion

                            Paragraph ParaSpacing_LumpSum = new Paragraph();
                            ParaSpacing_LumpSum.SpacingBefore = 20;//5
                            Font NoteHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED);

                            if (dcTotal < dcMinTotal)
                            {
                                pdfDoc.Add(new Paragraph("    NOTE: The total of charges applicable (i.e., " + dcTotal + ") is less than the actual minimum total (i.e., " + dcMinTotal + ").", NoteHeadingFont));
                                pdfDoc.Add(ParaSpacing_LumpSum);
                            }
                            #endregion
                        }
                        else
                        {
                            #region OTHER THAN LUMP SUM CODE

                            #region ADD WATERMARK

                            //string imageFilePath = Server.MapPath("~/Images/Draft.png");

                            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                            //jpg.ScaleToFit(3000, 770);   //For give the size to image
                            //jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                            //jpg.SetAbsolutePosition(7, 69); //give absolute/specified fix position to image.
                            //pdfwriter.PageEvent = new ImageBackgroundHelper(jpg);
                            //pdfDoc.Add(jpg);
                            #endregion

                            string contents = "";
                            contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                            contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                            contents = contents.Replace("[Today's Date]", date.ToString());
                            contents = contents.Replace("[CustomerName]", CustomerName);
                            contents = contents.Replace("[AddressLine1]", AddressLine1);
                            if (AddressLine2 == String.Empty)
                                contents = contents.Replace("[AddressLine2]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine2]", AddressLine2);
                            if (AddressLine3 == String.Empty)
                                contents = contents.Replace("[AddressLine3]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine3]", AddressLine3);
                            contents = contents.Replace("[PersonName]", PersonName);
                            contents = contents.Replace("[Subject]", Subject);
                            contents = contents.Replace("[BodyContent]", BodyContent);
                            contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

                            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                            foreach (var htmlelement in parsedContent)
                                pdfDoc.Add(htmlelement as IElement);

                            PdfPTable pdftable = new PdfPTable(3);
                            pdftable.TotalWidth = 500f;
                            pdftable.LockedWidth = true;
                            float[] widths = new float[] { 0.16f, 1.0f, 0.5f };
                            pdftable.SetWidths(widths);
                            pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Set Table Spacing Before And After html text
                            pdftable.SpacingBefore = 10f;
                            pdftable.SpacingAfter = 20f;

                            #region Create Table Column Header Cell with Text
                            // Header: Serial Number
                            PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No", GridHeadingFont));
                            cellwithdata.Colspan = 1;
                            cellwithdata.BorderWidth = 1f;
                            cellwithdata.HorizontalAlignment = 0;//left
                            cellwithdata.VerticalAlignment = 0;// Center
                            cellwithdata.Padding = 5;
                            cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata);

                            // Header: Particulars
                            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                            cellwithdata1.Colspan = 1;
                            cellwithdata1.BorderWidth = 1f;
                            cellwithdata1.HorizontalAlignment = 0;
                            cellwithdata1.Padding = 5;
                            cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata1);

                            // Header: Charges Applicable
                            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                            cellwithdata2.Colspan = 1;
                            cellwithdata2.BorderWidth = 1f;
                            cellwithdata2.HorizontalAlignment = 0;
                            cellwithdata2.Padding = 5;
                            cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata2);

                            // Header: Minimum Charges
                            //PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Minimum Charges", GridHeadingFont));
                            //cellwithdata3.Colspan = 1;
                            //cellwithdata3.BorderWidth = 1f;
                            //cellwithdata3.HorizontalAlignment = 0;
                            //cellwithdata3.Padding = 5;
                            //cellwithdata3.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            //pdftable.AddCell(cellwithdata3);
                            #endregion

                            #region ADD CELL TO DATATABLE IN PDF
                            foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                            {
                                TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                                i = i + 1;

                                // Data Cell: Serial Number - Auto Increment Cell
                                PdfPCell SrnoCell = new PdfPCell();
                                SrnoCell.Colspan = 1;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingBottom = 8;

                                // Data Cell: Particulars Cell
                                PdfPCell ParticularCell = new PdfPCell();
                                ParticularCell.Colspan = 1;
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);
                                //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;

                                // Data Cell:  Charges Applicable
                                PdfPCell ChargesCell = new PdfPCell();
                                ChargesCell.Colspan = 1;
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);
                                ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;
                                //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;

                                // Data Cell:  Minimum Charges Applicable
                                //PdfPCell MinCell = new PdfPCell();
                                //MinCell.Colspan = 1;
                                //Paragraph paraMinChargesApp = new Paragraph();
                                //paraMinChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]), TextFontformat));
                                //MinCell.AddElement(paraMinChargesApp);
                                //MinCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                //MinCell.VerticalAlignment = Element.ALIGN_LEFT;
                                ////MinCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]), TextFontformat);
                                //MinCell.PaddingBottom = 8;
                                //MinCell.PaddingLeft = 5;
                                //MinCell.PaddingRight = 5;

                                //Boolean IsValidDraft = Convert.ToBoolean(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["IsValidAmount"]));
                                //if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]) != "")
                                //{
                                //    if (IsValidDraft == false)
                                //    {
                                //        SrnoCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                //        ParticularCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                //        ChargesCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                //        MinCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                //    }
                                //}

                                pdftable.AddCell(SrnoCell);
                                pdftable.AddCell(ParticularCell);
                                pdftable.AddCell(ChargesCell);
                               // pdftable.AddCell(MinCell);
                            }

                            #endregion

                            pdfDoc.Add(pdftable);

                            #endregion
                        }
                    }

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 20;//5                         

                    #region TRANSPORTATION CHARGES

                    DataSet dsGetAnnexure = QuotationOperations.GetTransportationCharges(QuotationId);
                    if (dsGetAnnexure != null && dsGetAnnexure.Tables[0].Rows.Count > 0)
                    {
                        PdfPTable tblAnnexure = new PdfPTable(3);
                        tblAnnexure.TotalWidth = 500f;
                        tblAnnexure.LockedWidth = true;
                        float[] widths_Annexure = new float[] { 0.4f, 1.7f, 1.7f };
                        tblAnnexure.SetWidths(widths_Annexure);
                        tblAnnexure.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblAnnexure.SpacingBefore = 5f;
                        tblAnnexure.SpacingAfter = 4f;

                        // Header: Serial Number
                        PdfPCell cellAnnexure_Header = new PdfPCell(new Phrase("Transportation Charges", GridHeadingFont));
                        cellAnnexure_Header.Colspan = 3;
                        cellAnnexure_Header.HorizontalAlignment = 0;//left
                        cellAnnexure_Header.VerticalAlignment = 0;// Center
                        cellAnnexure_Header.Padding = 5;
                        cellAnnexure_Header.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure_Header);

                        PdfPCell cellAnnexure1 = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                        cellAnnexure1.Colspan = 1;
                        cellAnnexure1.HorizontalAlignment = 0;//left
                        cellAnnexure1.VerticalAlignment = 0;// Center
                        cellAnnexure1.Padding = 5;
                        cellAnnexure1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure1);

                        // Header: Particulars
                        PdfPCell cellAnnexure2 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellAnnexure2.Colspan = 1;
                        cellAnnexure2.HorizontalAlignment = 0;//left
                        cellAnnexure2.VerticalAlignment = 0;// Center
                        cellAnnexure2.Padding = 5;
                        cellAnnexure2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure2);

                        // Header: Charges Applicable
                        PdfPCell cellAnnexure3 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellAnnexure3.Colspan = 1;
                        cellAnnexure3.HorizontalAlignment = 0;//left
                        cellAnnexure3.VerticalAlignment = 0;// Center
                        cellAnnexure3.Padding = 5;
                        cellAnnexure3.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure3);

                        int cnt_SrNo = 1;
                        foreach (DataRow dr in dsGetAnnexure.Tables[0].Rows)
                        {
                            // Data Cell: Sr.No.
                            PdfPCell SrNo_Annx = new PdfPCell();
                            SrNo_Annx.Colspan = 1;
                            SrNo_Annx.HorizontalAlignment = Element.ALIGN_LEFT;
                            SrNo_Annx.VerticalAlignment = Element.ALIGN_LEFT;
                            SrNo_Annx.Phrase = new Phrase(Convert.ToString(cnt_SrNo), TextFontformat);
                            SrNo_Annx.Padding = 5;
                            tblAnnexure.AddCell(SrNo_Annx);

                            // Data Cell: Particulars
                            PdfPCell Annx_Part = new PdfPCell();
                            Annx_Part.Colspan = 1;
                            Annx_Part.HorizontalAlignment = Element.ALIGN_LEFT;
                            Annx_Part.VerticalAlignment = Element.ALIGN_LEFT;
                            Annx_Part.Phrase = new Phrase(Convert.ToString(dr["Particulars"].ToString()), TextFontformat);
                            Annx_Part.Padding = 5;
                            tblAnnexure.AddCell(Annx_Part);

                            // Data Cell: Charges Applicable
                            PdfPCell Annx_ChgApp = new PdfPCell();
                            Annx_ChgApp.Colspan = 1;
                            Annx_ChgApp.HorizontalAlignment = Element.ALIGN_LEFT;
                            Annx_ChgApp.VerticalAlignment = Element.ALIGN_LEFT;
                            Annx_ChgApp.Phrase = new Phrase(Convert.ToString(dr["ChargesApplicable"].ToString()), TextFontformat);
                            Annx_ChgApp.Padding = 5;
                            tblAnnexure.AddCell(Annx_ChgApp);

                            cnt_SrNo = cnt_SrNo + 1;
                        }
                        pdfDoc.Add(tblAnnexure);
                        pdfDoc.Add(ParaSpacing);
                    }

                    #endregion

                    #region PAYMENT TERMS
                    if (PaymentTerms != "")
                    {
                        PdfPTable tblTerms = new PdfPTable(1);
                        tblTerms.TotalWidth = 500f;
                        tblTerms.LockedWidth = true;
                        float[] widths_Terms = new float[] { 2.2f };
                        tblTerms.SetWidths(widths_Terms);
                        tblTerms.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblTerms.SpacingBefore = 10f;
                        tblTerms.SpacingAfter = 4f;

                        PdfPCell cell = new PdfPCell(new Phrase("Payment Terms", GridHeadingFont));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 0;//left
                        cell.VerticalAlignment = 0;// Center
                        cell.Padding = 5;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblTerms.AddCell(cell);

                        // Data Cell: Payment Terms
                        PdfPCell Terms = new PdfPCell();
                        Terms.Colspan = 1;
                        Terms.HorizontalAlignment = Element.ALIGN_LEFT;
                        Terms.VerticalAlignment = Element.ALIGN_LEFT;
                        Terms.Phrase = new Phrase(Convert.ToString(PaymentTerms), TextFontformat);
                        Terms.Padding = 5;
                        tblTerms.AddCell(Terms);
                        pdfDoc.Add(tblTerms);
                        pdfDoc.Add(ParaSpacing);
                    }
                    #endregion

                    #region TERMS & CONDITIONS

                    PdfPTable tblTermsCondition = new PdfPTable(2);
                    tblTermsCondition.TotalWidth = 500f;
                    tblTermsCondition.LockedWidth = true;
                    float[] widths_TermsCon = new float[] { 0.2f, 2.1f };
                    tblTermsCondition.SetWidths(widths_TermsCon);
                    tblTermsCondition.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblTermsCondition.SpacingBefore = 5f;
                    tblTermsCondition.SpacingAfter = 4f;

                    PdfPCell cellTerms_Header = new PdfPCell(new Phrase("Terms & Conditions :", GridHeadingFont));
                    cellTerms_Header.Colspan = 2;
                    cellTerms_Header.HorizontalAlignment = 0;//left
                    cellTerms_Header.VerticalAlignment = 0;// Center
                    cellTerms_Header.Padding = 5;
                    cellTerms_Header.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                    tblTermsCondition.AddCell(cellTerms_Header);

                    DataSet dsGetTermsDetails = QuotationOperations.GetTermConditionDetails(TermConditionId);
                    if (dsGetTermsDetails != null && dsGetTermsDetails.Tables[0].Rows.Count > 0)
                    {
                        int cnt_SrNo = 1;
                        foreach (DataRow dr in dsGetTermsDetails.Tables[0].Rows)
                        {
                            // Data Cell: Sr.No.
                            PdfPCell SrNo_Terms = new PdfPCell();
                            SrNo_Terms.Colspan = 1;
                            SrNo_Terms.HorizontalAlignment = Element.ALIGN_CENTER;
                            SrNo_Terms.VerticalAlignment = Element.ALIGN_CENTER;
                            SrNo_Terms.Phrase = new Phrase(Convert.ToString(cnt_SrNo), TextFontformat);
                            SrNo_Terms.Padding = 5;
                            tblTermsCondition.AddCell(SrNo_Terms);

                            // Data Cell: Payment Terms
                            PdfPCell Terms_Name = new PdfPCell();
                            Terms_Name.Colspan = 1;
                            Terms_Name.HorizontalAlignment = Element.ALIGN_LEFT;
                            Terms_Name.VerticalAlignment = Element.ALIGN_LEFT;
                            Terms_Name.Phrase = new Phrase(Convert.ToString(dr["sTermCondition"].ToString()), TextFontformat);
                            Terms_Name.Padding = 5;
                            tblTermsCondition.AddCell(Terms_Name);
                            cnt_SrNo = cnt_SrNo + 1;
                        }
                    }

                    pdfDoc.Add(tblTermsCondition);

                    #endregion

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph("    We hope the above rates are in order and look forward for long lasting association with your esteemed ", TextFontformat));
                    pdfDoc.Add(new Paragraph("    organization.", TextFontformat));
                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
                    pdfDoc.Add(ParaSpacing);

                    #region SIGNATURE IMAGE 
                    if (SignImgPath != "")
                    {
                        string ImagePath = DownloadImage(SignImgPath);
                        iTextSharp.text.Image imgSignImgPath = iTextSharp.text.Image.GetInstance(ImagePath);
                        imgSignImgPath.ScaleAbsoluteWidth(60);
                        PdfPTable tblSign = new PdfPTable(1);
                        tblSign.TotalWidth = 50f;
                        tblSign.LockedWidth = true;
                        float[] widths_Sign = new float[] { 2.0f };
                        tblSign.SetWidths(widths_Sign);
                        tblSign.HorizontalAlignment = Element.ALIGN_LEFT;
                        tblSign.SpacingBefore = 2;

                        PdfPCell Terms = new PdfPCell();
                        Terms.Colspan = 1;
                        Terms.HorizontalAlignment = Element.ALIGN_LEFT;
                        Terms.VerticalAlignment = Element.ALIGN_LEFT;
                        Terms.Border = Rectangle.NO_BORDER;
                        Terms.AddElement(imgSignImgPath);
                        Terms.PaddingLeft = 10;
                        tblSign.AddCell(Terms);
                        pdfDoc.Add(tblSign);
                        Paragraph ParaSpacing_Sign = new Paragraph();
                        ParaSpacing_Sign.SpacingAfter = 3;
                    }
                    #endregion

                    pdfDoc.Add(new Paragraph("    " + dsGetReportData.Tables[0].Rows[0]["CreatedBy"].ToString(), TextFontformat));
                    pdfDoc.Add(new Paragraph("    Authorized Signatory", GridHeadingFont));
                    pdfDoc.Add(ParaSpacing);

                    iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterET.png"));
                    footer.SetAbsolutePosition(40, 10);
                    footer.ScaleAbsoluteWidth(480);
                    footer.ScaleAbsoluteHeight(100);
                    pdfDoc.Add(footer);
                    pdfDoc.Add(ParaSpacing);

                    htmlparser.Parse(sr);
                    int DocPath = 0;
                    if (QuotationId != 0 && filePath != "")
                        DocPath = QuotationOperations.AddQuotationCopy(QuotationId, filePath + ".pdf");

                    pdfDoc.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
    }

    protected string GetFormattedName(string FileName)
    {
        FileName = FileName.Replace(".", "");
        FileName = FileName.Replace("/", "");
        FileName = FileName.Replace("\"", "");
        FileName = FileName.Replace("@", "");
        FileName = FileName.Replace("#", "");
        FileName = FileName.Replace("(", "");
        FileName = FileName.Replace(")", "");
        FileName = FileName.Replace(",", "");
        FileName = FileName.Replace(";", "");
        FileName = FileName.Replace(":", "");
        return FileName;
    }

    public string GetQuoteFileName(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (ServerFilePath != "")
        {
            if (System.IO.File.Exists(ServerFilePath + ".pdf"))
            {
                string FileId = RandomString(5);
                FileName += "_" + FileId;
            }
        }
        return FileName;
    }

    public string GetQuotePath(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (ServerFilePath != "")
        {
            if (System.IO.File.Exists(ServerFilePath + ".pdf"))
            {
                string ext = ".pdf";
                string FileId = RandomString(5);
                ServerFilePath += "_" + FileId + ext;
            }
            else
                ServerFilePath = ServerFilePath + ".pdf";
        }

        return ServerFilePath;
    }

    private string DownloadImage(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ImageSign\\" + DocumentPath);
        else
            ServerPath = ServerPath + "ImageSign\\" + DocumentPath;

        try
        {
            //HttpResponse response = Page.Response;
            //FileDownload.Download(response, ServerPath, DocumentPath);
            return ServerPath;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    class ImageBackgroundHelper : PdfPageEventHelper
    {
        private iTextSharp.text.Image img;
        public ImageBackgroundHelper(iTextSharp.text.Image img)
        {
            this.img = img;
        }
        /**
         * @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
         *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
         */
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            writer.DirectContentUnder.AddImage(img);
        }
    }

    protected string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }
        return builder.ToString();
    }




    protected void DownloadDoc(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;
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

    protected bool SendMailForKYC(int LeadId, int EnquiryId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", EmailContent = "";
        string EncryptedEnquiryId = HttpUtility.UrlEncode(Encrypt(Convert.ToString((EnquiryId))));

        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        DataSet dsGetLead = DBOperations.CRM_GetLeadById(LeadId);
        if (dsGetLead != null)
        {
            try
            {
                string strFileName = "../EmailTemplate/KYCRequest.txt";
                //string strFileName = "../EmailTemplate/KYCRequest.html";
                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
            }

            MessageBody = EmailContent.Replace("@KycLink", "http://live.babajishivram.com/KYC_New/index.aspx?p=" + EncryptedEnquiryId);
            MessageBody = MessageBody.Replace("@Company", dsGetLead.Tables[0].Rows[0]["CompanyName"].ToString());
            MessageBody = MessageBody.Replace("@Contact", dsGetLead.Tables[0].Rows[0]["ContactName"].ToString());
            MessageBody = MessageBody.Replace("@Email", dsGetLead.Tables[0].Rows[0]["Email"].ToString());
            MessageBody = MessageBody.Replace("@PhoneNo", dsGetLead.Tables[0].Rows[0]["MobileNo"].ToString());
            MessageBody = MessageBody.Replace("@CreatedBy", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());
            MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetLead.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
            MessageBody = MessageBody.Replace("@UserName", dsGetLead.Tables[0].Rows[0]["CreatedBy"].ToString());

            strSubject = "KYC Request from Babaji Shivram Clearing & Carriers Pvt. Ltd.";
            //strCustomerEmail = "kivisha.jain@babajishivram.com";
            strCCEmail = ""; //"kivisha.jain@babajishivram.com , javed.shaikh@babajishivram.com"; //" , " + dsGetLead.Tables[0].Rows[0]["CreatedByMail"].ToString();

            if (strCustomerEmail == "" || strSubject == "")
                return false;
            else
            {
                List<string> lstFileDoc = new List<string>();
                bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFileDoc);
                return bEmailSuccess;
            }
        }
        else
            return false;
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Pendingquote_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLeads.AllowPaging = false;
        gvLeads.AllowSorting = false;
        gvLeads.Columns[0].Visible = false;
        gvLeads.Columns[1].Visible = false;
        gvLeads.Enabled = false;
        gvLeads.Caption = "Pending Quote Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Quote.aspx";
        DataFilter1.FilterDataSource();
        gvLeads.DataBind();
        gvLeads.HeaderRow.Cells[15].Text = "Aging <br/>[Today - KYC Registered]";
        gvLeads.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

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
            DataFilter1.FilterSessionID = "Quote.aspx";
            DataFilter1.FilterDataSource();
            gvLeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region POPUP EVENT

    protected bool SendEmail(int LeadId, string strCustomerEmail, int UserId)
    {
        bool bEmailSuccess = false;
        if (LeadId > 0)
        {
            //int LeadId = 0;
            string strCCEmail = "", strSubject = "", MessageBody = "", EmailContent = "";
            DataSet dsGetQuote = DBOperations.CRM_GetLeadById(LeadId);
            if (dsGetQuote != null)
            {
                string EncryptedUserId = HttpUtility.UrlEncode(Encrypt(UserId.ToString()));
                string EncryptedLId = HttpUtility.UrlEncode(Encrypt(dsGetQuote.Tables[0].Rows[0]["lid"].ToString()));
                try
                {
                    string strFileName = "../EmailTemplate/QuoteApproval.txt";
                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    lblError.CssClass = "errorMsg";
                }

                MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/QuoteApproval.aspx?a=" + EncryptedUserId + "&i=" + EncryptedLId);
                //MessageBody = EmailContent.Replace("@a", "http://live.babajishivram.com/CRM/QuoteApproval.aspx?a=" + UserId.ToString() + "&i=" + dsGetQuote.Tables[0].Rows[0]["lid"].ToString());
                //MessageBody = MessageBody.Replace("@b", "http://live.babajishivram.com/CRM/QuoteRejection.aspx?a=" + UserId.ToString() + "&i=" + dsGetQuote.Tables[0].Rows[0]["lid"].ToString());
                MessageBody = MessageBody.Replace("@Company", dsGetQuote.Tables[0].Rows[0]["CompanyName"].ToString());
                MessageBody = MessageBody.Replace("@Contact", dsGetQuote.Tables[0].Rows[0]["ContactName"].ToString());
                MessageBody = MessageBody.Replace("@Email", dsGetQuote.Tables[0].Rows[0]["Email"].ToString());
                MessageBody = MessageBody.Replace("@PhoneNo", dsGetQuote.Tables[0].Rows[0]["MobileNo"].ToString());
                MessageBody = MessageBody.Replace("@CreatedBy", dsGetQuote.Tables[0].Rows[0]["CreatedBy"].ToString());
                MessageBody = MessageBody.Replace("@CreatedDate", Convert.ToDateTime(dsGetQuote.Tables[0].Rows[0]["CreatedDate"]).ToString("dd/MM/yyyy"));
                MessageBody = MessageBody.Replace("@UserName", dsGetQuote.Tables[0].Rows[0]["CreatedBy"].ToString());

                strSubject = "Pending Quote Approval";
                strCustomerEmail = strCustomerEmail; //"dhaval@babajishivram.comn ; kivisha.jain@babajishivram.com";
                strCCEmail = "javed.shaikh@babajishivram.com"; // + " , " + dsGetQuote.Tables[0].Rows[0]["CreatedByMail"].ToString();

                if (strCustomerEmail == "" || strSubject == "")
                    return false;
                else
                {
                    List<string> lstFileDoc = new List<string>();
                    if (dsGetQuote.Tables[0].Rows[0]["QuotePath"] != DBNull.Value)
                    {
                        if (dsGetQuote.Tables[0].Rows[0]["QuotePath"].ToString() != "")
                        {
                            lstFileDoc.Add("Quotation\\" + dsGetQuote.Tables[0].Rows[0]["QuotePath"].ToString());
                        }
                    }

                    DataSet dsGetRFQDocs = DBOperations.CRM_GetRFQDocuments(LeadId);
                    if (dsGetRFQDocs != null)
                    {
                        for (int i = 0; i < dsGetRFQDocs.Tables[0].Rows.Count; i++)
                        {
                            if (dsGetRFQDocs.Tables[0].Rows[i]["DocPath"].ToString() != "")
                            {
                                lstFileDoc.Add("Quotation\\" + dsGetRFQDocs.Tables[0].Rows[i]["DocPath"].ToString());
                            }
                        }
                    }

                    bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFileDoc);
                    return bEmailSuccess;
                }
            }
            else
                return false;
        }
        else
            return false;
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        DateTime dtClose = DateTime.MinValue;
        if (txtTargetDt.Text != "")
        {
            dtClose = Convert.ToDateTime(txtTargetDt.Text.Trim());
        }
        int LeadId = 0;
        if (ddlStatus.SelectedValue != "0")
        {
            int Save = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(hdnQuotationId.Value), Convert.ToInt32(ddlStatus.SelectedValue), txtRemark.Text.Trim(), dtClose, loggedInUser.glUserId);
            if (Save == 1)
            {
                if (ddlStatus.SelectedValue == "7") // customer approved
                {
                    int KYCResult = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(hdnQuotationId.Value), Convert.ToInt32(11), txtRemark.Text.Trim(), dtClose, loggedInUser.glUserId);

                    // get lead id by quote
                    DataSet dsGetQuote = DBOperations.CRM_GetQuoteByLid(Convert.ToInt32(hdnQuotationId.Value));
                    if (dsGetQuote != null && dsGetQuote.Tables.Count > 0)
                    {
                        if (dsGetQuote.Tables[0].Rows.Count > 0)
                        {
                            LeadId = Convert.ToInt32(dsGetQuote.Tables[0].Rows[0]["LeadId"].ToString());
                        }
                    }

                    // update existing customer enquiry status
                    if (hdnExistEnquiry.Value != "")
                    {
                        if (hdnExistEnquiry.Value == "2") // existing customer enquiry
                        {
                            int result_enquiryStatus = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(hdnQuotationId.Value), Convert.ToInt32(16), txtRemark.Text.Trim(), dtClose, loggedInUser.glUserId);
                        }
                    }

                    string strCustomerEmail = "";
                    int UserId = 0;
                    List<string> lstEmailTo = new List<string>();
                    lstEmailTo.Add("1");
                    lstEmailTo.Add("2");

                    if (lstEmailTo.Count > 0)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            if (lstEmailTo[k].ToString() == "1")
                            {
                                strCustomerEmail = "kirti@babajishivram.com";
                                UserId = 189;
                            }
                            else
                            {
                                strCustomerEmail = "dhaval@babajishivram.com";
                                UserId = 3;
                            }

                            //bool bSentMail = SendEmail(Convert.ToInt32(LeadId), strCustomerEmail, UserId);
                            //if (bSentMail == true)
                            //{
                            //    lblError.Text = "Successfully updated status for Quotation. Quote has been sent for management approval.";
                            //    lblError.CssClass = "success";
                            //    gvLeads.DataBind();
                            //}
                        }
                    }
                }
                else
                {
                    lblError.Text = "Successfully updated status for Quotation.";
                    lblError.CssClass = "success";
                    gvStatusHistory.DataBind();
                    txtRemark.Text = "";
                }
            }
            else
            {
                lblError.Text = "Please select status.";
                lblError.CssClass = "errorMsg";
                //ModalPopupDocument.Show();
            }
        }
        else
        {
            lblError.Text = "Please select status.";
            lblError.CssClass = "errorMsg";
            ddlStatus.Focus();
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue != "0")
        {
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 5)
            {
                tdTargetMonth.Visible = false;
                txtTargetDt.Visible = false;
                rfvrem.Visible = false;
                ModalPopupDocument.Show();
            }
            if (Convert.ToInt32(ddlStatus.SelectedValue) == 3 || Convert.ToInt32(ddlStatus.SelectedValue) == 8)
            {
                tdTargetMonth.Visible = true;
                txtTargetDt.Visible = true;
                ModalPopupDocument.Show();
            }
            else
            {
                tdTargetMonth.Visible = false;
                txtTargetDt.Visible = false;
                rfvrem.Visible = true;
                ModalPopupDocument.Show();
            }
        }
    }

    #endregion

    #region ENCRYPT/DECRYPT QUERYSTRING VARIABLES
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    private string Decrypt(string cipherText)
    {
        try
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
        catch (Exception en)
        {

        }
        return cipherText;
    }
    #endregion

    protected void imgClose_Forstatus_Click(object sender, ImageClickEventArgs e)
    {
        mpeRFQStatus.Hide();
    }
    protected void popup_Click(object sender, EventArgs e)
    {
        // Other Codes.

        // Show ModalPopUpExtender.
        mpeRFQStatus.Show();
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        if (ddlRFQ.SelectedValue != "0")
        {
            if (ddlRFQ.SelectedValue == "1")
            {
                //Session["LeadId"]=
                Response.Redirect("RfqQuote.aspx");


            }
            else if (ddlRFQ.SelectedValue == "2")
            {
                Response.Redirect("AddQuote.aspx");
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpeRFQStatus.Hide();
    }
}