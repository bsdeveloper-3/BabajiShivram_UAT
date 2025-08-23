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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class ExportPCA_ExPendingPCD : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        //ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        ScriptManager1.RegisterPostBackControl(gvJobDetail);
        //ScriptManager1.RegisterPostBackControl(lnkConsoleCover);
        //ScriptManager1.RegisterPostBackControl(btnConsolidatedCoverPDF);
        //ScriptManager1.RegisterPostBackControl(btnConsolidatedCoverXLS);

        if (!IsPostBack)
        {
            Session["JobId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending PCA Document";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For PCA Document!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = PCDSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "ExPendingPCD.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    private void PCDForwardToDispatchDept(int JobId)
    {
        int Result = DBOperations.AddPCDToCustomer(JobId, LoggedInUser.glUserId);
        if (Result == 0)
        {
            lblMessage.Text = "Job Forwarded To Dispatch Department!";
            lblMessage.CssClass = "success";
            gvJobDetail.DataBind();
        }//END_IF
        else if (Result == 1)
        {
            lblMessage.Text = "System Error! Please try after sometime!";
            lblMessage.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblMessage.Text = "PCA Document Already Forwarded To Dispatch Department!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    private void GeneratePDFDocument(int JobId)
    {
        DataSet dsPCAPrint = EXOperations.EX_GetJobDetailforPCALetter(JobId);
        if (dsPCAPrint.Tables[0].Rows[0]["AddressLine1"] != DBNull.Value)
        {
            string BranchName = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["BranchName"]);
            string Customer = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Customer"]);
            string PlantName = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PlantName"]);
            string PlantPerson = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["ContactPerson"]);
            string PlantAddress1 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine1"]);
            string PlantAddress2 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine2"]);
            string PlantCity = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["City"]);
            string PlantPinCode = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PinCode"]);
            string PlantMobile = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["MobileNo"]);
            string PlantEmail = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Email"]);

            string BSJobNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["JobRefNo"]);
            string CustRefNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["CustRefNo"]);
            string NoofPkg = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["NoOfPackages"]);
            string Mode = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["TransMode"]);
            string InvoiceDate = dsPCAPrint.Tables[0].Rows[0]["InvoiceDate"].ToString();
            string POL = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PortOfLoading"]);
            string POD = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PortOfDischarge"]);
            string ShippingLineDate = dsPCAPrint.Tables[0].Rows[0]["ShippingLineDate"] != System.DBNull.Value ? Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["ShippingLineDate"]).ToShortDateString() : "";
            string SBNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["SBNo"]);
            string SBDate = dsPCAPrint.Tables[0].Rows[0]["SBDate"].ToString();
            string strInvoiceNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["InvoiceNo"]).Trim();
            //string strECDetail = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["SchemeNoDate"]).Trim();

            //strECDetail = strECDetail.TrimEnd(',');
            strInvoiceNo = strInvoiceNo.TrimEnd(',');
            string BsUser = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            //logo.Width = 80;
            //logo.Height = 40.00;
            //logo.SetAbsolutePosition();
            string date = DateTime.Today.ToShortDateString();

            DataSet dsPCADcoument = DBOperations.FillPCDDocumentByWorkFlow(JobId, Convert.ToInt32(EnumPCDDocType.PCACustomer));

            try
            {
                if (dsPCADcoument.Tables[0].Rows.Count > 0)
                {
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=PCA Letter-" + BSJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    Rectangle recPDF = new Rectangle(PageSize.A4);

                    // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                    // Set PDF Document size and Left,Right,Top and Bottom margin

                    Document pdfDoc = new Document(recPDF);

                    //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);


                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                    Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                    logo.SetAbsolutePosition(380, 700);

                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    string contents = "";
                    contents = File.ReadAllText(Server.MapPath("../ExportCHA/PCACoverLetter.htm"));
                    contents = contents.Replace("[Today's Date]", date.ToString());
                    contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[CustomerPCAAddress1]", PlantAddress1);
                    if (PlantAddress2 == String.Empty)
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantCity + " - " + PlantPinCode);
                        contents = contents.Replace("[CustomerPCACity]", String.Empty);
                    }
                    else
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                        contents = contents.Replace("[CustomerPCACity]", PlantCity + " - " + PlantPinCode);
                    }
                    //contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                    // contents = contents.Replace("[CustomerPCACity]",PlantCity +" - "+PlantPinCode);
                    contents = contents.Replace("[PCAContactPersonName]", PlantPerson);
                    contents = contents.Replace("[BSJOBNO]", BSJobNo);
                    contents = contents.Replace("[CustomerRefNo]", CustRefNo);
                    contents = contents.Replace("[ShippingLineDate]", ShippingLineDate);
                    contents = contents.Replace("[SB NO]", SBNo);
                    contents = contents.Replace("[SBDate]", SBDate);
                    contents = contents.Replace("[ShipmentInvoice]", strInvoiceNo);
                    contents = contents.Replace("[NoofPkg]", NoofPkg);
                    contents = contents.Replace("[Mode]", Mode);
                    contents = contents.Replace("[POL]", POL);
                    contents = contents.Replace("[POD]", POD);
                    contents = contents.Replace("[InvoiceDate]", InvoiceDate);
                    //contents = contents.Replace("[ECDetail]", strECDetail);

                    var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                    foreach (var htmlelement in parsedContent)
                        pdfDoc.Add(htmlelement as IElement);

                    PdfPTable pdftable = new PdfPTable(4);

                    pdftable.TotalWidth = 500f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.1f, 1.5f, 0.2f, 0.2f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Set Table Spacing Before And After html text
                    pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 10f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    cellwithdata.Colspan = 1;
                    cellwithdata.BorderWidth = 1f;
                    //  cellwithdata.BackgroundColor = GrayColor.LIGHT_GRAY;

                    cellwithdata.HorizontalAlignment = 0;//left
                    pdftable.AddCell(cellwithdata);

                    // Header: Document Name
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Document Type", GridHeadingFont));
                    cellwithdata1.Colspan = 1;
                    cellwithdata1.BorderWidth = 1f;
                    cellwithdata1.HorizontalAlignment = 0;
                    cellwithdata1.VerticalAlignment = 0;// Center
                    pdftable.AddCell(cellwithdata1);

                    // Header: Document Type - Original
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Original", GridHeadingFont));
                    cellwithdata2.Colspan = 1;
                    cellwithdata2.BorderWidth = 1f;
                    cellwithdata2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellwithdata2.VerticalAlignment = Element.ALIGN_RIGHT;// Center
                    pdftable.AddCell(cellwithdata2);

                    // Header: Document Type - Copy
                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Copy", GridHeadingFont));
                    cellwithdata3.Colspan = 1;
                    cellwithdata3.BorderWidth = 1f;
                    cellwithdata3.HorizontalAlignment = 0;
                    pdftable.AddCell(cellwithdata3);


                    // Tick Mark Sign Font Creation For Originall/Copy
                    Phrase CheckMarkPhrase = new Phrase();
                    Font zapfdingbats = new Font(Font.FontFamily.ZAPFDINGBATS, 10, 3);
                    CheckMarkPhrase.Add(new Chunk("\u0033", zapfdingbats)); //"\u0033" For Tick Mark"

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.HorizontalAlignment = 0;//1

                    // Data Cell: Document Name Cell
                    PdfPCell DocNameCell = new PdfPCell();
                    DocNameCell.Colspan = 1;

                    // Data Cell: Original PdfCell
                    PdfPCell OriginalCell = new PdfPCell();
                    OriginalCell.Colspan = 1;
                    OriginalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    OriginalCell.VerticalAlignment = Element.ALIGN_CENTER;

                    // Data Cell: Copy PdfCell
                    PdfPCell CopyCell = new PdfPCell();
                    CopyCell.Colspan = 1;
                    CopyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    CopyCell.VerticalAlignment = Element.ALIGN_CENTER;

                    // Generae Table Data from PCA Document Dataset

                    foreach (DataRow dr in dsPCADcoument.Tables[0].Rows)
                    {
                        i = i + 1;
                        //  pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                        // Add Cell Data To Table

                        // Serial number #
                        SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);

                        pdftable.AddCell(SrnoCell);

                        // Document Name

                        DocNameCell.Phrase = new Phrase(Convert.ToString(dsPCADcoument.Tables[0].Rows[i - 1]["DocumentName"]), TextFontformat);

                        pdftable.AddCell(DocNameCell);

                        // Original Cell
                        if ((dsPCADcoument.Tables[0].Rows[i - 1]["IsOriginal"]).ToString().ToLower() == "yes")
                        {
                            OriginalCell.Phrase = CheckMarkPhrase; // Add Tick Mark Sign
                        }
                        else
                        {
                            OriginalCell.Phrase = new Phrase(); // Add Blank Text Cell
                        }

                        pdftable.AddCell(OriginalCell);

                        // Copy Cell
                        if ((dsPCADcoument.Tables[0].Rows[i - 1]["IsCopy"]).ToString().ToLower() == "yes")
                        {
                            CopyCell.Phrase = CheckMarkPhrase; // Add Tick Mark Sign
                        }
                        else
                        {
                            CopyCell.Phrase = new Phrase(); // Add Blank Text Cell
                        }

                        pdftable.AddCell(CopyCell);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 10;

                    pdfDoc.Add(new Paragraph("    Kindly acknowledge the receipt of the above documents.", TextFontformat));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    Authorised Signatory", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    User      : " + BsUser, TextFontformat));
                    pdfDoc.Add(new Paragraph("    Branch  : " + BranchName, TextFontformat));


                    // Footer Image Commented
                    // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));

                    // footer.SetAbsolutePosition(30, 0);
                    // pdfDoc.Add(footer);
                    //// pdfwriter.PageEvent = new PDFFooter();

                    string footerText = "   Corporate Office: Plot No 2, Behind Excom House, Saki Vihar Road, Sakinaka, Andheri East.\n" +
                        "   Mumbai 400072. India. Tel.: +91 22 66485600. Email: info@babajishivram.com, WEBSITE: WWW.BabajiShivram.com\n" +
                        "   BRANCHES: NHAVA SHEVA, CHENNAI, KOLKATA, VISAKHAPATNAM, KAKINADA, BANGLORE, GOA\n" +
                        "   REGISTERED OFFICE: 407, REX CHAMBERS, 4TH FLOOR, WALCHAND HIRACHAND MARG, BALLARD ESTATE MUMBAI - 400038";

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText, FooterFontformat));

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }//END_IF

                else
                {
                    lblMessage.Text = "Please First Update Document List!";
                    lblMessage.CssClass = "errorMsg";
                }

            }//END_Try

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblMessage.Text = "PCA Address Not Found For Plant Name " + dsPCAPrint.Tables[0].Rows[0]["PlantName"].ToString();
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region GRID VIEW EVENTS

    protected void gvJobDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        // Show Document List For PCA To Customer
        if (e.CommandName.ToLower() == "getshippingdetail")
        {
            Session["JobId"] = e.CommandArgument.ToString();
            Response.Redirect("../ExportCHA/PCDDocDetail.aspx");
        }
        // Open Popup for Document List Add/Update for PCA To Customer Applicable
        else if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            ScriptManager1.RegisterPostBackControl(btnSaveDocument);

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;
            int PCDDocType = Convert.ToInt32(EnumPCDDocType.PCACustomer);

            rptDocument.DataSource = EXOperations.EX_GetPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();
            ModalPopupDocument.Show();
        }
        // Forward Job To Dispatch
        else if (e.CommandName.ToLower() == "forwdtodispatch" && e.CommandArgument != null)
        {
            int JobId = Convert.ToInt32(e.CommandArgument);
            PCDForwardToDispatchDept(JobId);
        }
        // Create PDF Document For PCA Convering Letter
        else if (e.CommandName.ToLower() == "createpcaletter" && e.CommandArgument != null)
        {
            int JobId = Convert.ToInt32(e.CommandArgument);
            GeneratePDFDocument(JobId);
        }
    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion

    #region DOCUMENT POPUP EVENTS
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(hdnJobId.Value);
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.PCACustomer);

        int Result = -1;

        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");

            int DocumentId = Convert.ToInt32(hdnDocId.Value);

            if (chk.Checked) // Add Document Type
            {
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                CheckBoxList chkDuplicate = (CheckBoxList)(itm.FindControl("chkDuplicate"));

                string strFilePath = "";
                bool IsCopy = false, IsOriginal = false;

                if (chkDuplicate.Items[0].Selected)
                    IsOriginal = true;

                if (chkDuplicate.Items[1].Selected)
                    IsCopy = true;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strUploadPath, fuDocument);
                }

                Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblMessage.Text = "Document List Updated For Customer.";
                    lblMessage.CssClass = "success";
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after some time.";
                    lblMessage.CssClass = "errorMsg";
                }
            }//END_IF
            else // Remove Document Type
            {
                Result = DBOperations.DeletePCDDocument(JobId, DocumentId, PCDDocType, LoggedInUser.glUserId);
            }
        }//END_ForEach

        gvJobDetail.DataBind();
    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");

            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");

            chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));

            chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));

            if (DataBinder.Eval(e.Item.DataItem, "PCDDocId").ToString() != "0")
            {
                chkDocumentType.Checked = true;
                fileUploadDocument.Enabled = true;
            }

            if (chkDocumentType != null && fileUploadDocument != null && CVCheckBoxList != null)
            {
                // CheckBox OnClientClick java Script Functino
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");

                // CheckBoxList Customer Validation Control "ClientValidationFunction="ValidateCheckBoxList"
                // Add Parameter for Javascript Function ValidateCheckBoxList("Update Panel Id","CustomerValidatorId","Control Identifier","CheckBoxlistId","IsValid"); 

                // ScriptManager.RegisterExpandoAttribute(upShipment, CVCheckBoxList.ClientID, "checklistId", chkDuplicate.ClientID, false);

                // Add Javascript On Click Event For Checkbox List Copy/Original
                foreach (System.Web.UI.WebControls.ListItem lstitem in chkDuplicate.Items)
                {
                    lstitem.Attributes.Add("OnClick", "javascript:chkDuplicateChecked('" + chkDuplicate.ClientID + "','" + chkDocumentType.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                }
            }
        }
    }

    #endregion

    #region Document Upload/Download
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        // int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);

        string FileName = fuPCDUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        // string ServerFilePath = Server.MapPath(FilePath);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
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
        if (fuPCDUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }

    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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
            DataFilter1.FilterSessionID = "ExPendingPCD.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
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
        string strFileName = "PendingPCA_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /*Verifies that the control is rendered */
    //}

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
        gvJobDetail.Columns[1].Visible = false; // Link Button job Ref NO
        gvJobDetail.Columns[2].Visible = true; // Show Job Number
        gvJobDetail.Columns[9].Visible = false; // PCA Document Link
        gvJobDetail.Columns[10].Visible = false; // Create LCA Letter (PDF) Link Button
        gvJobDetail.Columns[11].Visible = false; // Forward To Dispatch Link Button

        gvJobDetail.Caption = "Pending PCA On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "ExPendingPCD.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}