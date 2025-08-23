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

public partial class Quotation_SavedQuotation : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvQuotationDetails);
        ScriptManager1.RegisterPostBackControl(gvContractCopy);
        ScriptManager1.RegisterPostBackControl(btnAddContractCopy);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Quotations";
        if (!IsPostBack)
        {
            //FillQuotationLists();
            if (gvQuotationDetails.Rows.Count == 0)
            {
                lblError.Text = "No Quotation Found!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceQuotedDetail2;
        DataFilter1.DataColumns = gvQuotationDetails.Columns;
        DataFilter1.FilterSessionID = "SavedQuotation.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Gridview Events    

    protected void gvQuotationDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvQuotationDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "getquote")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            int StatusId = 0;

            Session["QuotationId"] = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                StatusId = Convert.ToInt32(commandArgs[1].ToString());

            if (StatusId == 1) // Draft Quotation
                Response.Redirect("PendingQuotation.aspx");
            else if (StatusId == 4 || StatusId == 8 || StatusId == 9) // Rejected or negotiated quote or invalid quote
                Response.Redirect("EditQuotation.aspx");
            else if (StatusId > 4 || StatusId == 3)
                Response.Redirect("ApprovedQuote.aspx");
            else
                Response.Redirect("EditQuotation.aspx");
        }
        else if (e.CommandName.ToLower() == "downloadquote")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
        }
        else if (e.CommandName.ToLower() == "getstatus")
        {
            int QuotationId = Convert.ToInt32(e.CommandArgument.ToString());
            if (QuotationId != 0)
            {
                lbError_Popup.Text = "";
                lbError_Popup.CssClass = "";
                Session["QuotationId"] = QuotationId.ToString();
                ModalPopupDocument.Show();
            }
        }
        else if (e.CommandName.ToLower() == "getdoc")
        {
            mpeContractCopy.Show();
            txtContractEndDt.Text = "";
            txtContractStartDt.Text = "";
            txtContractStartDt.Enabled = true;
            txtContractEndDt.Enabled = true;
            Session["QuotationId"] = e.CommandArgument.ToString();
            gvContractCopy.DataBind();
            DataSet dsGetQuoteDetails = QuotationOperations.GetParticularQuotation(Convert.ToInt32(Session["QuotationId"]));
            if (dsGetQuoteDetails != null)
            {
                if (dsGetQuoteDetails.Tables[0].Rows[0]["ContractEndDt"] != DBNull.Value)
                {
                    txtContractEndDt.Text = Convert.ToDateTime(dsGetQuoteDetails.Tables[0].Rows[0]["ContractEndDt"]).ToShortDateString();
                    txtContractEndDt.Enabled = false;
                    imgEndDt.Visible = false;
                }

                if (dsGetQuoteDetails.Tables[0].Rows[0]["ContractStartDt"] != DBNull.Value)
                {
                    txtContractStartDt.Text = Convert.ToDateTime(dsGetQuoteDetails.Tables[0].Rows[0]["ContractStartDt"]).ToShortDateString();
                    txtContractStartDt.Enabled = false;
                    imgStartDt.Visible = false;
                }
            }
        }
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

    protected void gvQuotationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string IsTenderQuote = rowView["IsTenderQuote"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsTenderQuote.Trim().ToLower() == "true")
                {
                    e.Row.Cells[4].Text = "";
                    // e.Row.Cells[3].Text = "Approved";
                    // e.Row.Cells[3].Enabled = false;
                }
            }

            string IsValidDraft = rowView["IsValidDraft"].ToString();
            foreach (TableCell cell in e.Row.Cells)
            {
                if (IsValidDraft.Trim().ToLower() == "0")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(238, 170, 144, 0.87)");
                    e.Row.ToolTip = "Invalid Draft Quotation.";
                }
                else if (IsValidDraft.Trim().ToLower() == "1")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(247, 233, 134, 0.29)");
                    e.Row.ToolTip = "Valid Draft Quotation.";
                }
            }

            int Status = Convert.ToInt32(rowView["ApprovalStatusId"].ToString());
            foreach (TableCell cell in e.Row.Cells)
            {
                if (Status == 2) // approval pending or draft quotation 
                {
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[3].Enabled = false;
                }

                //if (Status < 4)
                //{
                //    e.Row.Cells[14].Text = "";
                //}
            }
        }
    }

    #endregion

    #region POPUP EVENT

    protected bool SendEmail(int QuoteId, string EmailTo, int UserId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();

        if (QuoteId > 0)
        {
            string strCustomerEmail = "", strCCEmail = "", strSubject = "", MessageBody = "", strCustomerName = "", strSentEmailTo_Name = "",
                    strSentEmailTo_mail = "", strQuotedBy = "", strQuotedBy_Email = "";

            DataSet dsGetQuoteDetails = QuotationOperations.GetParticularQuotation(QuoteId);
            if (dsGetQuoteDetails != null)
            {
                if (Convert.ToBoolean(dsGetQuoteDetails.Tables[0].Rows[0]["IsOutsideEnq"]) == false)
                {
                    if (dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value)
                    {
                        strCustomerName = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"].ToString();
                        strSentEmailTo_Name = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteApprovalTo"].ToString();
                        strSentEmailTo_mail = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteApprovalTo_Email"].ToString();
                        strQuotedBy = dsGetQuoteDetails.Tables[0].Rows[0]["QuotedUpdatedBy"].ToString();
                        strQuotedBy_Email = dsGetQuoteDetails.Tables[0].Rows[0]["QuotedUpdatedBy_Email"].ToString();
                    }

                    strbuilder = strbuilder.Append("<BR><html><body style='margin - left: 10px; height: 100%; width: 100%; font - family:Arial; font - style:normal; font - size:9pt; color:#000;'>" +
                                "Dear Sir / Madam,<BR><BR>" +
                                "Please " + "<a href='http://crm.babajishivram.com:86/Forms/Approval.aspx?i=" + UserId.ToString() + "&p=" + QuoteId.ToString() + "'>click here</a>"
                                + " for approving quotation for customer <b>" + strCustomerName.ToUpper().Trim() + "</b>.<BR><BR>");

                    if (dsGetQuoteDetails.Tables[0].Rows[0]["LeadRefNo"] != DBNull.Value)
                    {
                        if (Convert.ToString(dsGetQuoteDetails.Tables[0].Rows[0]["LeadRefNo"]) != "")
                        {
                            strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:60%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                            strbuilder = strbuilder.Append("<tr><th colspan='2' style='font-weight: bold; background-color: #FCD5B4; color:black; padding-left: 3px; text-align: left;'>Lead Information</th></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Lead Ref No</th><td style='padding-left: 3px'>" + dsGetQuoteDetails.Tables[0].Rows[0]["LeadRefNo"].ToString() + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Added By</th><td style='padding-left: 3px'>" + dsGetQuoteDetails.Tables[0].Rows[0]["LeadCreatedBy"].ToString() + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Added On</th><td style='padding-left: 3px'>" + Convert.ToDateTime(dsGetQuoteDetails.Tables[0].Rows[0]["LeadCreatedDate"]).ToString("dd/MM/yyyy") + "</td></tr>");
                            strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>User Email</th><td style='padding-left: 3px'>" + dsGetQuoteDetails.Tables[0].Rows[0]["LeadCreatedByMail"].ToString() + "</td></tr>");
                            strbuilder = strbuilder.Append("</table><BR>");
                        }
                    }

                    //strbuilder = strbuilder.Append("<BR><html><body style='margin - left: 10px; height: 100%; width: 100%; font - family:Arial; font - style:normal; font - size:9pt; color:#000;'>" +
                    //                                "Dear Sir / Madam,<BR><BR>" +
                    //                                "Please follow the link for approving quotation with a glance of enquiry details : - <BR><BR>" +
                    //                                "<a href='http://crm.babajishivram.com:86/Forms/Approval.aspx?i=" + UserId.ToString() + "&p=" + QuoteId.ToString() + "'>Click here</a>");
                    // "<a href = '" + Request.Url.AbsoluteUri.Replace("CS.aspx", "CS_Activation.aspx?ActivationCode=" + activationCode) + "'>Click here to activate your account.</a>"; "
                    strbuilder = strbuilder.Append("<BR>Thanks & Regards,<BR>" + strQuotedBy + "<BR/></body></html>");

                    // Email Format
                    strCustomerEmail = EmailTo; //strSentEmailTo_mail + " , " + "kirti@babajishivram.com"; // "javed.shaikh@babajishivram.com ";
                    strCCEmail = strQuotedBy_Email + " , " + "javed.shaikh@babajishivram.com"; // , " + strQuotedBy_Email;
                    strSubject = "Quotation approval for customer - " + strCustomerName.ToUpper().Trim();

                    if (strCustomerEmail == "" || strSubject == "")
                        return false;
                    else
                    {
                        MessageBody = strbuilder.ToString();
                        List<string> lstFileDoc = new List<string>();

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

        if (ddlStatus.SelectedValue != "0")
        {
            int Save = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(Session["QuotationId"]), Convert.ToInt32(ddlStatus.SelectedValue), txtRemark.Text.Trim(), dtClose, loggedInUser.glUserId);

            //lbError_Popup.Text = "Successfully updated status for Quotation.";
            //lbError_Popup.CssClass = "success";
            //gvStatusHistory.DataBind();
            //txtRemark.Text = "";
            //ModalPopupDocument.Show();

            if (Save == 1)
            {
                if (ddlStatus.SelectedValue == "7") // customer approved
                {
                    //bool bSentMail = SendEmail(Convert.ToInt32(Session["QuotationId"]));
                    //if (bSentMail == true)
                    //{
                    //    lblError.Text = "Successfully updated status for Quotation. Quote has been sent for management approval.";
                    //    lblError.CssClass = "success";
                    //}

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

                            bool bSentMail = SendEmail(Convert.ToInt32(Session["QuotationId"]), strCustomerEmail, UserId);
                            if (bSentMail == true)
                            {
                                lblError.Text = "Successfully updated status for Quotation. Quote has been sent for management approval.";
                                lblError.CssClass = "success";
                            }
                        }
                    }
                }
                else
                {
                    lblError.Text = "Successfully updated status for Quotation.";
                    lblError.CssClass = "success";
                    gvStatusHistory.DataBind();
                    txtRemark.Text = "";
                    //ModalPopupDocument.Show();
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
                rfvrem.Visible = false;
                ModalPopupDocument.Show();
            }
            else
            {
                rfvrem.Visible = true;
                ModalPopupDocument.Show();
            }
        }
    }

    #endregion

    #region CRYSTAL REPORT EVENTS

    //protected void DownloadQuotePdf(int QuotationId)
    //{
    //    string CustomerName = "", testPath = "";
    //    ReportDocument crystalReport = new ReportDocument();
    //    DataSet dsGetQuotationDetails = DBOperations.GetParticularQuotation(QuotationId);
    //    if (dsGetQuotationDetails != null)
    //    {
    //        if (dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value && dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString() != "")
    //        {
    //            CustomerName = dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString();
    //        }
    //    }

    //    #region UPLOAD PDF GENERATED IN A FOLDER
    //    string fileName = CustomerName.Replace("&amp;", "").ToString();
    //    string strUploadPath = "";//CustomerName.Replace("&amp;", "").ToString();
    //    string ServerFilePath = FileServer.GetFileServerDir();

    //    if (ServerFilePath == "")
    //    {
    //        ServerFilePath = Server.MapPath("..\\UploadQuotation\\" + strUploadPath);  // Application Directory Path
    //    }
    //    else
    //    {
    //        ServerFilePath = ServerFilePath + strUploadPath; // File Server Path
    //    }

    //    if (!System.IO.Directory.Exists(ServerFilePath))
    //    {
    //        System.IO.Directory.CreateDirectory(ServerFilePath);
    //    }
    //    if (fileName != string.Empty)
    //    {
    //        if (System.IO.File.Exists(ServerFilePath + fileName))
    //        {
    //            //string ext = Path.GetExtension(fuPCDUpload.FileName);
    //            string ext = Path.GetExtension(fileName);
    //            //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
    //            fileName = Path.GetFileNameWithoutExtension(fileName);
    //            string FileId = RandomString(5);

    //            fileName += "_" + FileId + ext;
    //        }
    //    }
    //    #endregion

    //    crystalReport.Load(Server.MapPath("QuotationReport.rpt"));
    //    dsQuotation dsQuotationRpt = new dsQuotation();
    //    if (QuotationId != 0) // Import job covering letter detail
    //        dsQuotationRpt = GetReportData(Convert.ToInt32(QuotationId));
    //    if (dsQuotationRpt.Tables.Count > 0 && dsQuotationRpt.Tables[0].Rows.Count > 0)
    //    {
    //        crystalReport.SetDataSource(dsQuotationRpt);
    //        // testPath = @"C:\inetpub\wwwroot\QuotationProject\UploadQuotation\" + fileName;
    //        crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ServerFilePath + fileName);
    //        PdfDocument pdf = new PdfDocument();
    //        PdfPTable SummaryTable = new PdfPTable(2);
    //        using (FileStream fs = new FileStream(Server.MapPath("..\\UploadQuotation\\" + fileName), FileMode.Create))
    //        {
    //            using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
    //            {

    //                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

    //                doc.Open();
    //                doc.Add(SummaryTable);

    //                doc.Close();

    //            }
    //            fs.Close();
    //        }


    //        crystalReport.Close();
    //        crystalReport.Clone();
    //        crystalReport.Dispose();
    //        crystalReport = null;
    //        GC.Collect();
    //        GC.WaitForPendingFinalizers();
    //    }
    //    else
    //    {
    //        lblError.Text = "Error while generating quotation. Please try again later..!!";
    //        lblError.CssClass = "errorMsg!";
    //    }
    //}

    //protected dsQuotation GetReportData(int QuotationId)
    //{
    //    string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
    //    SqlCommand cmd = new SqlCommand("rpt_Quotation");

    //    cmd.Parameters.Add(new SqlParameter("@QuotationId", QuotationId));
    //    using (SqlConnection con = new SqlConnection(conString))
    //    {
    //        using (SqlDataAdapter sda = new SqlDataAdapter())
    //        {
    //            cmd.Connection = con;
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            sda.SelectCommand = cmd;
    //            using (dsQuotation ds = new dsQuotation())
    //            {
    //                sda.Fill(ds, "GnQuotation");
    //                return ds;
    //            }
    //        }
    //    }
    //}

    //public string RandomString(int size)
    //{
    //    StringBuilder builder = new StringBuilder();
    //    for (int i = 0; i < size; i++)
    //    {

    //        //26 letters in the alfabet, ascii + 65 for the capital letters
    //        builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

    //    }
    //    return builder.ToString();
    //}

    #endregion

    #region PDF EVENTS

    protected void DownloadQuotation(int QuotationId)
    {
        string CustomerName = "", TodaysDate = "";

        DataSet dsGetQuotationDetails = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuotationDetails != null)
        {
            if (dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value && dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString() != "")
            {
                CustomerName = dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString();
            }

            Boolean IsLumpSumCode = false;
            if (dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"] != DBNull.Value)
            {
                if (dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"].ToString() == "0")
                {
                    IsLumpSumCode = false;
                }
                else
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

            string PaymentTerms = dsGetQuotationDetails.Tables[0].Rows[0]["BodyContent"].ToString();

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
                    string filePath = UploadFiles(CustomerName.Replace("&amp;", "").ToString());
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=" + CustomerName.Replace("&amp;", "").ToString() + ".pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
                    Document pdfDoc = new Document(recPDF);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                    pdfDoc.Open();

                    // PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                    logo.SetAbsolutePosition(380, 700);
                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    if (IsLumpSumCode == true)
                    {
                        #region LUMP SUM CODE

                        string contents = "";
                        contents = File.ReadAllText(Server.MapPath("Quotation.htm"));
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
                        float[] widths = new float[] { 0.18f, 1.1f, 0.9f };
                        pdftable.SetWidths(widths);
                        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Set Table Spacing Before And After html text
                        pdftable.SpacingBefore = 10f;
                        pdftable.SpacingAfter = 4f;

                        // Create Table Column Header Cell with Text
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

                        Decimal dcTotal = 0, dcMinTotal = 0;
                        #region ADD CELL TO DATATABLE IN PDF
                        foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                        {
                            // Data Cell: Serial Number - Auto Increment Cell
                            PdfPCell SrnoCell = new PdfPCell();
                            SrnoCell.Colspan = 1;
                            SrnoCell.HorizontalAlignment = 1;
                            //SrnoCell.VerticalAlignment = Element.ALIGN_MIDDLE;

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

                            decimal LumpSumAmt = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LumpSumAmt"]));
                            if (LumpSumAmt != 0)
                            {
                                dcTotal = dcTotal + LumpSumAmt;
                            }

                            decimal Mintotal = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinAmt"]));
                            if (Mintotal != 0)
                            {
                                dcMinTotal = dcMinTotal + Mintotal;
                            }

                            // SrnoCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);

                            // Serial number #
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
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

                        // 2nd Data Cell: Total
                        PdfPCell Cell2 = new PdfPCell();
                        Cell2.Border = Rectangle.NO_BORDER;
                        table_Total.AddCell(Cell2);

                        // 3rd Data Cell: Total
                        PdfPCell Total = new PdfPCell();
                        Total.Colspan = 1;
                        Total.HorizontalAlignment = 0;
                        Total.Phrase = new Phrase(Convert.ToString("Total:" + " " + dcTotal), GridHeadingFont);
                        Total.Padding = 5;
                        if (dcMinTotal != 0 && dcTotal != 0)
                        {
                            if (dcTotal < dcMinTotal)
                                Total.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                        }
                        table_Total.AddCell(Total);

                        // 1st Data Cell: Blank column
                        PdfPCell Cell3 = new PdfPCell();
                        Cell3.Border = Rectangle.NO_BORDER;
                        table_Total.AddCell(Cell3);

                        // 2nd Data Cell: Total
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

                        Paragraph ParaSpacing = new Paragraph();
                        ParaSpacing.SpacingBefore = 20;//5
                        Font NoteHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED);

                        pdfDoc.Add(new Paragraph("    NOTE: The total of charges applicable (i.e., " + dcTotal + ") is less than the actual minimum total (i.e., " + dcMinTotal + ").", NoteHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    We hope the above rates are in order and look forward for logn lasting association with your esteemed ", TextFontformat));
                        pdfDoc.Add(new Paragraph("    organization.", TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    " + dsGetReportData.Tables[0].Rows[0]["CreatedBy"].ToString(), TextFontformat));
                        pdfDoc.Add(new Paragraph("    Authorized Signatory", GridHeadingFont));

                        Font FooterHeadingFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                        pdfDoc.Add(new Paragraph("     Babaji Shivram Clearing & Carriers Pvt Ltd", FooterHeadingFont));

                        // Footer Image Commented
                        iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterET5.png"));
                        //footer = 50;
                        footer.SetAbsolutePosition(35, 0);
                        pdfDoc.Add(footer);

                        #region ADD WATERMARK

                        string imageFilePath = Server.MapPath("~/Images/Draft.png");

                        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                        //Resize image depend upon your need
                        //For give the size to image
                        jpg.ScaleToFit(3000, 770);

                        //If you want to choose image as background then,
                        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                        //If you want to give absolute/specified fix position to image.
                        jpg.SetAbsolutePosition(7, 69);
                        pdfDoc.Add(jpg);
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region OTHER THAN LUMP SUM CODE

                        string contents = "";
                        contents = File.ReadAllText(Server.MapPath("Quotation.htm"));
                        contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                        contents = contents.Replace("[Today's Date]", date.ToString());
                        contents = contents.Replace("[CustomerName]", CustomerName);
                        contents = contents.Replace("[CustomerAddress]", CustAddress);
                        //if (PlantAddress2 == String.Empty)
                        //{
                        //    contents = contents.Replace("[CustomerPCAAddress2]", PlantCity + " - " + PlantPinCode);
                        //    contents = contents.Replace("[CustomerPCACity]", String.Empty);
                        //}
                        //else
                        //{
                        //    contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                        //    contents = contents.Replace("[CustomerPCACity]", PlantCity + " - " + PlantPinCode);
                        //}
                        contents = contents.Replace("[PersonName]", PersonName);
                        contents = contents.Replace("[Subject]", Subject);
                        contents = contents.Replace("[BodyContent]", BodyContent);

                        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                        foreach (var htmlelement in parsedContent)
                            pdfDoc.Add(htmlelement as IElement);

                        PdfPTable pdftable = new PdfPTable(4);

                        pdftable.TotalWidth = 500f;
                        pdftable.LockedWidth = true;
                        float[] widths = new float[] { 0.15f, 1.0f, 0.5f, 0.5f };
                        pdftable.SetWidths(widths);
                        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Set Table Spacing Before And After html text
                        pdftable.SpacingBefore = 10f;
                        pdftable.SpacingAfter = 20f;

                        // Create Table Column Header Cell with Text
                        // Header: Serial Number
                        PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                        cellwithdata.Colspan = 1;
                        cellwithdata.BorderWidth = 1f;
                        cellwithdata.HorizontalAlignment = 0;//left
                        cellwithdata.VerticalAlignment = 0;// Center
                        cellwithdata.Padding = 5;
                        pdftable.AddCell(cellwithdata);

                        // Header: Particulars
                        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellwithdata1.Colspan = 1;
                        cellwithdata1.BorderWidth = 1f;
                        cellwithdata1.HorizontalAlignment = 0;
                        cellwithdata1.Padding = 5;
                        //cellwithdata.VerticalAlignment = 0;// Center
                        pdftable.AddCell(cellwithdata1);

                        // Header: Charges Applicable
                        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellwithdata2.Colspan = 1;
                        cellwithdata2.BorderWidth = 1f;
                        cellwithdata2.HorizontalAlignment = 0;
                        cellwithdata2.Padding = 5;
                        //cellwithdata.VerticalAlignment = Element.ALIGN_RIGHT;// Center
                        //cellwithdata2.BackgroundColor = GrayColor.LIGHT_GRAY;
                        pdftable.AddCell(cellwithdata2);

                        // Header: Minimum Charges
                        PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Minimum Charges", GridHeadingFont));
                        cellwithdata3.Colspan = 1;
                        cellwithdata3.BorderWidth = 1f;
                        cellwithdata3.HorizontalAlignment = 0;
                        cellwithdata3.Padding = 5;
                        pdftable.AddCell(cellwithdata3);

                        #region ADD CELL TO DATATABLE IN PDF
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

                            // Data Cell:  Minimum Charges Applicable
                            PdfPCell MinCell = new PdfPCell();
                            MinCell.Colspan = 1;
                            MinCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            MinCell.VerticalAlignment = Element.ALIGN_LEFT;

                            TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                            i = i + 1;

                            Boolean IsValidDraft = Convert.ToBoolean(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["IsValidDraft"]));
                            if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]) != "")
                            {
                                if (IsValidDraft == false)
                                {
                                    SrnoCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                    ParticularCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                    ChargesCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                    MinCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                }
                            }

                            // Serial number #
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
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

                            // Minimum Charges Applicable
                            MinCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]), TextFontformat);
                            MinCell.Padding = 5;
                            pdftable.AddCell(MinCell);
                        }
                        #endregion

                        pdfDoc.Add(pdftable);
                        Paragraph ParaSpacing = new Paragraph();
                        ParaSpacing.SpacingBefore = 20;//5

                        pdfDoc.Add(new Paragraph("    We hope the above rates are in order and look forward for logn lasting association with your esteemed ", TextFontformat));
                        pdfDoc.Add(new Paragraph("    organization.", TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("    " + dsGetReportData.Tables[0].Rows[0]["CreatedBy"].ToString(), TextFontformat));
                        pdfDoc.Add(new Paragraph("    Authorized Signatory", GridHeadingFont));

                        Font FooterHeadingFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                        pdfDoc.Add(new Paragraph("     Babaji Shivram Clearing & Carriers Pvt Ltd", FooterHeadingFont));

                        // Footer Image Commented
                        iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterET5.png"));
                        //footer = 50;
                        footer.SetAbsolutePosition(35, 0);
                        pdfDoc.Add(footer);
                        //pdfwriter.PageEvent = new PDFFooter();

                        //Font FooterHeadingFont2 = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                        //pdfDoc.Add(new Paragraph("     Corporate Office: Plot 2, Behind Excom House, Saki Vihar Road, Sakinaka, Andheri East,", FooterHeadingFont2));
                        //pdfDoc.Add(new Paragraph("     Mumbai 400072. Tel 91-22-6648 Fax: 91-22-6648 5656. Email: customercare@babajishivram.com", FooterHeadingFont2));

                        //Font FooterHeadingFont3 = FontFactory.GetFont("Arial", 7, Font.NORMAL);
                        //pdfDoc.Add(new Paragraph("      BRANCHES: NHAVA SHEVA  CHENNAI NEW DELHI KOLKATA VISHAKAPATNAM KAKINADA BANGALORE GOA", FooterHeadingFont3));
                        //pdfDoc.Add(new Paragraph("      REGISTERED OFFICE: 407,REX CHAMBERS,4th FLOOR, WALCHAND HIRACHAND MARG,BALLARD ESTATE,MUMBAI 400038.", FooterHeadingFont3));
                        //pdfDoc.Add(new Paragraph("     Website: www.babajishivram.com", FooterHeadingFont2));

                        #region ADD WATERMARK

                        string imageFilePath = Server.MapPath("~/Images/Draft.png");

                        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                        //Resize image depend upon your need
                        //For give the size to image
                        jpg.ScaleToFit(3000, 770);

                        //If you want to choose image as background then,

                        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                        //If you want to give absolute/specified fix position to image.
                        jpg.SetAbsolutePosition(7, 69);
                        pdfDoc.Add(jpg);
                        #endregion                      

                        #endregion
                    }

                    htmlparser.Parse(sr);
                    if (QuotationId != 0 && filePath != "")
                    {
                        int DocPath = QuotationOperations.AddQuotationCopy(QuotationId, filePath);
                    }
                    pdfDoc.Close();
                    //Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
    }

    protected string UploadFiles(string GetFileName)
    {
        string FileName = GetFileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadQuotation\\");
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = ".pdf";
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            //FU.SaveAs(ServerFilePath + FileName);

            return ServerFilePath + FileName;
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

    #endregion

    #region CONTRACT COPY EVENTS

    protected void imgbtnContract_Click(object sender, EventArgs e)
    {
        mpeContractCopy.Hide();
    }

    protected void btnAddContractCopy_OnClick(object sender, EventArgs e)
    {
        string fileName = "";
        int result = 0;
        if (fuUploadContractCopy != null && fuUploadContractCopy.HasFile)
            fileName = UploadFiles(fuUploadContractCopy, "");

        if (fileName != "")
        {
            result = QuotationOperations.AddQuotationAnnexure(Convert.ToInt32(Session["QuotationId"]), fileName, Convert.ToInt32(loggedInUser.glUserId.ToString()));
            if (result == 1)
            {
                lblErrorContract.Text = "Document added successfully!";
                lblErrorContract.CssClass = "success";
                gvContractCopy.DataBind();
                mpeContractCopy.Show();
            }
            else
            {
                lblErrorContract.Text = "System Error! Please Try After Sometime.";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
        }

        if (txtContractStartDt.Text.Trim() != "")
        {
            DateTime dtContractStart = DateTime.MinValue;
            DateTime dtContractEnd = DateTime.MinValue;
            if (txtContractStartDt.Text.Trim() != "")
                dtContractStart = Commonfunctions.CDateTime(txtContractStartDt.Text.Trim());
            if (txtContractEndDt.Text.Trim() != "")
                dtContractEnd = Commonfunctions.CDateTime(txtContractEndDt.Text.Trim());

            int SaveUpdateDates = QuotationOperations.UpdateQuotationContractDates(Convert.ToInt32(Session["QuotationId"]), dtContractStart, dtContractEnd, fileName, Convert.ToInt32(loggedInUser.glUserId.ToString()));
            if (SaveUpdateDates == 2)
            {
                lblErrorContract.Text = "Document added successfully!";
                lblErrorContract.CssClass = "success";
                gvContractCopy.DataBind();
                mpeContractCopy.Show();
            }
            else if (SaveUpdateDates == 3)
            {
                lblErrorContract.Text = "Quotation does not exists..!!";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
            else
            {
                lblErrorContract.Text = "System error. Please try again later..!!";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
        }
    }

    protected void gvContractCopy_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            mpeContractCopy.Show();
        }
        else if (e.CommandName.ToLower() == "deletedoc")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int result = QuotationOperations.DeleteAnnexureDocs(lid, Convert.ToInt32(loggedInUser.glUserId.ToString()));
                if (result == 1)
                {
                    lblErrorContract.Text = "Successfully deleted document!";
                    lblErrorContract.CssClass = "success";
                    gvContractCopy.DataBind();
                    mpeContractCopy.Show();
                }
                else
                {
                    lblErrorContract.Text = "System Error! Please Try After Sometime.";
                    lblErrorContract.CssClass = "errorMsg";
                    mpeContractCopy.Show();
                }
            }
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  QuotationOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
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

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Quotation\\" + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }
        else
        {
            return "";
        }

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
            DataFilter1.FilterSessionID = "SavedQuotation.aspx";
            DataFilter1.FilterDataSource();
            gvQuotationDetails.DataBind();
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
        string strFileName = "Quotations_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvQuotationDetails.AllowPaging = false;
        gvQuotationDetails.AllowSorting = false;
        gvQuotationDetails.Columns[0].Visible = false;
        gvQuotationDetails.Columns[1].Visible = false;
        gvQuotationDetails.Columns[2].Visible = true;
        gvQuotationDetails.Columns[10].Visible = false;
        gvQuotationDetails.Caption = "Quotation Lists On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "SavedQuotation.aspx";
        DataFilter1.FilterDataSource();
        gvQuotationDetails.DataBind();
        gvQuotationDetails.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}

//Font FooterHeadingFont2 = FontFactory.GetFont("Arial", 8, Font.NORMAL);
//pdfDoc.Add(new Paragraph("     Corporate Office: Plot 2, Behind Excom House, Saki Vihar Road, Sakinaka, Andheri East,", FooterHeadingFont2));
//pdfDoc.Add(new Paragraph("     Mumbai 400072. Tel 91-22-6648 Fax: 91-22-6648 5656. Email: customercare@babajishivram.com", FooterHeadingFont2));

//Font FooterHeadingFont3 = FontFactory.GetFont("Arial", 7, Font.NORMAL);
//pdfDoc.Add(new Paragraph("     BRANCHES: NHAVA SHEVA  CHENNAI NEW DELHI KOLKATA VISHAKAPATNAM KAKINADA BANGALORE GOA", FooterHeadingFont3));
//pdfDoc.Add(new Paragraph("     REGISTERED OFFICE: 407,REX CHAMBERS,4th FLOOR, WALCHAND HIRACHAND MARG,BALLARD ESTATE,MUMBAI 400038.", FooterHeadingFont3));
//pdfDoc.Add(new Paragraph("     Website: www.babajishivram.com", FooterHeadingFont2));