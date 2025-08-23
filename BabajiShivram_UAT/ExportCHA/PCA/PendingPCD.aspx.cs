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
public partial class PendingPCD : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        ScriptManager1.RegisterPostBackControl(gvJobDetail);
	    ScriptManager1.RegisterPostBackControl(lnkConsoleCover);
        ScriptManager1.RegisterPostBackControl(btnConsolidatedCoverPDF);
        ScriptManager1.RegisterPostBackControl(btnConsolidatedCoverXLS);
        
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
        DataFilter1.FilterSessionID = "PendingPCD.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void gvJobDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        // Show Document List For PCA To Customer
        if (e.CommandName.ToLower() == "select")
        {
            Session["JobId"] = e.CommandArgument.ToString();
            Response.Redirect("PCDDetail.aspx");
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

            if (strJobFileDir.Contains("MBOI"))
            {
                hdnBranchId.Value = "3"; // Mumbai SEA - Exclude
            }
            else if (strJobFileDir.Contains("MBAI"))
            {
                hdnBranchId.Value = "2"; // Mumbai AIR - Exclude
            }
            else
            {
                hdnBranchId.Value = "0";
            }

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int PCDDocType = Convert.ToInt32(EnumPCDDocType.PCACustomer);

            rptDocument.DataSource = DBOperations.FillPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();

            ModalPopupDocument.Show();
        }
        // Forwd Job To Dispatch
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

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");
            RequiredFieldValidator RFVFileUpload = (RequiredFieldValidator)e.Item.FindControl("RFVFile");

            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");

            chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));

            chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));

            string DocumentID = DataBinder.Eval(e.Item.DataItem, "lId").ToString();

            if (DataBinder.Eval(e.Item.DataItem, "PCDDocId").ToString() != "0")
            {
                chkDocumentType.Checked = true;
                fileUploadDocument.Enabled = true;
            }

            if (chkDocumentType != null && fileUploadDocument != null && CVCheckBoxList != null)
            {
                // CheckBox OnClientClick java Script Functino
                // chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");

                // File Upload Required
                if (DocumentID == "27") // Duplicate Bill of Entry - File Upload Required
                {
                    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "','" + "');");
                }
                else if (hdnBranchId.Value == "3" || hdnBranchId.Value == "2")// Mumbai Air/Sea - File Upload Not Required
                {
                    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                }
                else // File Upload Required
                {
                    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "','" + "');");
                }

                //if (DocumentID == "27") // Duplicate Bill of Entry - File Upload Required
                //{
                //    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "','" + "');");
                //}
                //else
                //{
                //    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                //}

                //chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "','" + "');");

                // CheckBoxList Customer Validation Control "ClientValidationFunction="ValidateCheckBoxList"
                // Add Parameter for Javascript Function ValidateCheckBoxList("Update Panel Id","CustomerValidatorId","Control Identifier","CheckBoxlistId","IsValid"); 

                ScriptManager.RegisterExpandoAttribute(upShipment, CVCheckBoxList.ClientID, "checklistId", chkDuplicate.ClientID, false);

                // Add Javascript On Click Event For Checkbox List Copy/Original
                foreach (System.Web.UI.WebControls.ListItem lstitem in chkDuplicate.Items)
                {
                    lstitem.Attributes.Add("OnClick", "javascript:chkDuplicateChecked('" + chkDuplicate.ClientID + "','" + chkDocumentType.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                }
            }
        }
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

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
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
    
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    
    private void GeneratePDFDocument(int JobId)
    {
        DataSet dsPCAPrint = DBOperations.GetJobDetailforPCAPrint(JobId);
        
        if (dsPCAPrint.Tables[0].Rows[0]["AddressLine1"] != DBNull.Value)
        {

            string BranchName   = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["BranchName"]);
            string Customer     = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Customer"]);
            string PlantName    = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PlantName"]);
            string PlantPerson  = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["ContactPerson"]);
            string PlantAddress1 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine1"]);
            string PlantAddress2 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine2"]);
            string PlantCity    = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["City"]);
            string PlantPinCode = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PinCode"]);
            string PlantMobile  = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["MobileNo"]);
            string PlantEmail   = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Email"]);

            string BSJobNo      = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["JobRefNo"]);
            string CustRefNo    = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["CustRefNo"]);
            string dispatchDate = dsPCAPrint.Tables[0].Rows[0]["LastDispatchDate"] != System.DBNull.Value ? Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["LastDispatchDate"]).ToShortDateString() : "";
            string BoeNo        = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["BOENo"]);
            string BoeDate      = Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["BOEDate"]).ToShortDateString();
            string MAWBL        = dsPCAPrint.Tables[0].Rows[0]["MAWBNo"] != System.DBNull.Value ? Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["MAWBNo"]) : "None";
            string MAWBLDate    = dsPCAPrint.Tables[0].Rows[0]["MAWBDate"] != System.DBNull.Value ? Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["MAWBDate"]).ToShortDateString() : "None";
            string HAWBL        = dsPCAPrint.Tables[0].Rows[0]["HAWBNo"] != System.DBNull.Value ? Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["HAWBNo"]) : "None";
            string HAWBLDate    = dsPCAPrint.Tables[0].Rows[0]["HAWBDate"] != System.DBNull.Value ? Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["HAWBDate"]).ToShortDateString() : "None";
            string strInvoiceNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["InvoiceNo"]).Trim();
            string strECDetail   =   Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["SchemeNoDate"]).Trim();

            strECDetail         = strECDetail.TrimEnd(',');
	    strInvoiceNo        = strInvoiceNo.TrimEnd(',');
	    string BsUser       = LoggedInUser.glEmpName;
            
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
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
                    contents = File.ReadAllText(Server.MapPath("CoverLetterPCA.htm"));
                    contents = contents.Replace("[Today's Date]", date.ToString());
                    contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[CustomerPCAAddress1]",PlantAddress1 );
                    if (PlantAddress2 == String.Empty)
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantCity + " - " + PlantPinCode);
                        contents = contents.Replace("[CustomerPCACity]",String.Empty);
                    }
                    else
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                        contents = contents.Replace("[CustomerPCACity]", PlantCity + " - " + PlantPinCode);
                    }
                    //contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                   // contents = contents.Replace("[CustomerPCACity]",PlantCity +" - "+PlantPinCode);
                    contents = contents.Replace("[PCAContactPersonName]",PlantPerson);
                    contents = contents.Replace("[BSJOBNO]", BSJobNo);
                    contents = contents.Replace("[CustomerRefNo]", CustRefNo);
                    contents = contents.Replace("[LastDispatchDate]", dispatchDate);
                    contents = contents.Replace("[BOE NO]", BoeNo);
                    contents = contents.Replace("[BoeDate]", BoeDate);
                    contents = contents.Replace("[MAWBL NO]", MAWBL);
                    contents = contents.Replace("[MAWBLDate]", MAWBLDate);
                    contents = contents.Replace("[HAWBL NO]", HAWBL);
                    contents = contents.Replace("[HAWBLDate]", HAWBLDate);
                    contents = contents.Replace("[ShipmentInvoice]", strInvoiceNo);
                    contents = contents.Replace("[ECDetail]", strECDetail);

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
                    Font zapfdingbats = new Font(Font.FontFamily.ZAPFDINGBATS,10,3);
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
                    pdfDoc.Add(new Paragraph("    Branch    : " + BranchName, TextFontformat));

                    
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

    #region for footer
    // public class PDFFooter : PdfPageEventHelper
//{
//    public override void OnEndPage(PdfWriter writer, Document document)
//    {   
//        base.OnEndPage(writer, document);
//        Paragraph footer1 = new Paragraph("Babaji Shivaram Clearing & Carriers Pvt Ltd", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD));
//        footer1.Alignment = Element.ALIGN_LEFT;
//        Paragraph footer2 = new Paragraph("Corporate office:Plot2,behind Excom office,saki vihar road,sakinaka,", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL));
//        footer2.Alignment = Element.ALIGN_LEFT;
//        Paragraph footer3 = new Paragraph("Andheri east,Mumbai 400072.Tel:91-22-6648 5656.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL));
//        footer3.Alignment = Element.ALIGN_LEFT;
//        Paragraph footer4 = new Paragraph("Babaji Shivaram Clearing & Carriers Pvt Ltd", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL));
//        footer4.Alignment = Element.ALIGN_LEFT;

//        PdfPTable footerTbl = new PdfPTable(1);
//        footerTbl.TotalWidth = 300;
//        footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

//        PdfPCell cell1 = new PdfPCell();
//        cell1.Border = 0;
//        cell1.PaddingLeft = 10;
//        cell1.PaddingTop =  7;
       
//        cell1.AddElement(footer1);
//        cell1.AddElement(footer2);
//        cell1.AddElement(footer3);
//        cell1.AddElement(footer4);
//        footerTbl.AddCell(cell1);
//       // footerTbl.WriteSelectedRows(0, -1, 40, 30, writer.DirectContent);
//        footerTbl.WriteSelectedRows(0, -1, 36,70, writer.DirectContent);


//    }


    //} 

    #endregion 

    #region Documnet Upload/Download/Delete
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        // int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);

	string FileName = fuPCDUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if(FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        // string ServerFilePath = Server.MapPath(FilePath);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
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
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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
            DataFilter1.FilterSessionID = "PendingPCD.aspx";
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
        gvJobDetail.Columns[2].Visible = true; // Bound Field Job Ref NO
        gvJobDetail.Columns[9].Visible = false; // PCA Document Link
        gvJobDetail.Columns[10].Visible = false; // Create LCA Letter (PDF) Link Button
        gvJobDetail.Columns[11].Visible = false; // Forward To Dispatch Link Button
        

        gvJobDetail.Caption = "Pending PCA On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingPCD.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //gvJobDetail.DataSourceID = "ShipmentClearedSqlDataSource";
        //gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

	#region Consolidated Cover

    protected void btnCancelCustomer_Click(object sender, EventArgs e)
    {
        ModalPopupCustomer.Hide();  
    }

    protected void lnkConsoleCover_Click(object sender, EventArgs e)
    {
        SqlDataSourceCustomer.SelectParameters["CustomerID"].DefaultValue = ddCustomer.SelectedValue;

        ModalPopupCustomer.Show();
    }

    protected void btnConsolidatedCoverPDF_Click(object sender, EventArgs e)
    {
        string strJobIDList = "";
        
        foreach (GridViewRow gvRow in gvCustomer.Rows)
        {
            int JobId = Convert.ToInt32(gvCustomer.DataKeys[gvRow.RowIndex].Value);
            CheckBox chk = (CheckBox)(gvRow.FindControl("chkJobNo"));

            if (chk.Checked) // Add Document Type
            {
                strJobIDList += JobId +",";
            }//END_IF
        }//END_ForEach

        if (strJobIDList != "")
        {
            if (ddCustomer.SelectedValue == "8")
                GenerateDOWPdf(strJobIDList);
            else if (ddCustomer.SelectedValue == "24")
                GenerateBASFPdf(strJobIDList);
            else if (ddCustomer.SelectedValue == "22")
                GenerateIntlFlavorPdf(strJobIDList);

	    // Add Consolidated Details

            DBOperations.AddPCDConsolidatedJob(strJobIDList, LoggedInUser.glUserId);
        }
        else
        {
            lblMessage.Text = "Please select atleast one Job for PCA Cover PDF!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    protected void btnConsolidatedCoverXLS_Click(object sender, EventArgs e)
    {
        string strJobIDList = "";
        
        foreach (GridViewRow gvRow in gvCustomer.Rows)
        {
            int JobId = Convert.ToInt32(gvCustomer.DataKeys[gvRow.RowIndex].Value);
            CheckBox chk = (CheckBox)(gvRow.FindControl("chkJobNo"));

            if (chk.Checked) // Add Document Type
            {
                strJobIDList += JobId + ",";
            }//END_IF
        }//END_ForEach

        if (strJobIDList != "")
        {
	    // Add Consolidated Details
            DBOperations.AddPCDConsolidatedJob(strJobIDList, LoggedInUser.glUserId);

            if (ddCustomer.SelectedValue == "8")
                GenerateDOWExcel(strJobIDList);
            else if (ddCustomer.SelectedValue == "24")
                GenerateBASFExcel(strJobIDList);
            else if (ddCustomer.SelectedValue == "22")
                GenerateIntlFlavorExcel(strJobIDList);
            
            
        }
        else
        {
            lblMessage.Text = "Please select atleast one Job for PCA Cover Excel!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    private void GenerateDOWPdf(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);

        string UserName = LoggedInUser.glEmpName;

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string date = DateTime.Today.ToShortDateString();

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            string JobRefNo = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            int rowCount = dsJobDetail.Tables[0].Rows.Count;
            
            string strConsignee     =   "";
            string strCustRefNo     =   "";
            string strBOENO         =   "";
            string strBOEDate       =   "";
            string strBETypeName    =   "";
            string strInvoiceNo     =   "";
            string strInvoiceDate   =   "";
            string strInvoiceValue  =   "";
            string strCurrency      =   "";
            string strMAWBLNo       =   "";
            
            try
            {
                // Generate PDF
                int i = 0; // Auto Increment Table Cell For Serial number
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Dow Chemical-Cover Letter-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);
                StringReader sr = new StringReader(sw.ToString());

                Rectangle recPDF = new Rectangle(PageSize.A4);

                // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                // Set PDF Document size and Left,Right,Top and Bottom margin

                Document pdfDoc = new Document(recPDF);

                // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                pdfDoc.Open();

                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                logo.SetAbsolutePosition(380, 720);

                logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                pdfDoc.Add(logo);

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDDowCover.htm"));
                contents = contents.Replace("[TodayDate]", date.ToString());
                contents = contents.Replace("[FROMName]", UserName);
                
                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                foreach (var htmlelement in parsedContent)
                    pdfDoc.Add(htmlelement as IElement);

                PdfPTable pdftable = new PdfPTable(10);

                pdftable.TotalWidth = 520f;
                pdftable.LockedWidth = true;
                float[] widths = new float[] { 0.07f, 0.24f, 0.2f, 0.2f, 0.2f, 0.22f, 0.18f, 0.2f, 0.18f, 0.2f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                // Set Table Spacing Before And After html text
                //   pdftable.SpacingBefore = 10f;
                pdftable.SpacingAfter = 8f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                pdftable.AddCell(cellwithdata);

                // cellwithdata.Colspan = 1;
                // cellwithdata.BorderWidth = 1f;
                // cellwithdata.HorizontalAlignment = Element.ALIGN_MIDDLE;
                // cellwithdata.VerticalAlignment = Element.ALIGN_CENTER;// Center

                // Header: Customer
                PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Customer", GridHeadingFont));
                pdftable.AddCell(cellwithdata1);

                
                // Header: Cust RefNo /PO No
                PdfPCell cellwithdata2 = new PdfPCell(new Phrase("PO/STO No", GridHeadingFont));
                cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata2);

                // Header: Invoice no
                PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Invoice No", GridHeadingFont));
                cellwithdata3.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata3);

                // Header: Invoice Date
                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Date", GridHeadingFont));
                cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata4);

                // Header: B/L No
                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("B/L No", GridHeadingFont));
                cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata5);

                // Header: BOE No.
                PdfPCell cellwithdata6 = new PdfPCell(new Phrase("BOE No", GridHeadingFont));
                cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata6);

                // Header: BOE Date.
                PdfPCell cellwithdata7 = new PdfPCell(new Phrase("BOE Date", GridHeadingFont));
                cellwithdata7.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata7);

                // Header: BE Status
                PdfPCell cellwithdata8 = new PdfPCell(new Phrase("BE Status", GridHeadingFont));
                cellwithdata8.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata8);

                // Header: BE Status
                PdfPCell cellwithdata9 = new PdfPCell(new Phrase("Amt (USD)", GridHeadingFont));
                cellwithdata9.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata9);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell SrnoCell = new PdfPCell();
                SrnoCell.Colspan = 1;
                SrnoCell.UseVariableBorders = false;

                // Data Cell: Customer

                PdfPCell CellCustomer = new PdfPCell();
                CellCustomer.Colspan = 1;
                CellCustomer.UseVariableBorders = false;

                // Data Cell: Customer RefNo/PO

                PdfPCell CellCustRefNo = new PdfPCell();
                CellCustRefNo.Colspan = 1;
                CellCustRefNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellCustRefNo.UseVariableBorders = false;

                // Data Cell: Invoice NO

                PdfPCell CellInvoiceNo = new PdfPCell();
                CellInvoiceNo.Colspan = 1;
                CellInvoiceNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellInvoiceNo.UseVariableBorders = false;

                // Data Cell: Invoice Date
                PdfPCell CellInvoiceDate = new PdfPCell();
                CellInvoiceDate.Colspan = 1;
                CellInvoiceDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellInvoiceDate.UseVariableBorders = false;

                // Data Cell: BL No
                PdfPCell CellBLNo = new PdfPCell();
                CellBLNo.Colspan = 1;
                CellBLNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBLNo.UseVariableBorders = false;

                // Data Cell: BOE No
                PdfPCell CellBOENo = new PdfPCell();
                CellBOENo.Colspan = 1;
                CellBOENo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBOENo.UseVariableBorders = false;

                // Data Cell: BOE Date
                PdfPCell CellBOEDate = new PdfPCell();
                CellBOEDate.Colspan = 1;
                CellBOEDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBOEDate.UseVariableBorders = false;

                // Data Cell: BE Status
                PdfPCell CellBEStatus = new PdfPCell();
                CellBEStatus.Colspan = 1;
                CellBEStatus.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBEStatus.UseVariableBorders = false;

                // Data Cell: Invoice Value
                PdfPCell CellInvoiceValue = new PdfPCell();
                CellInvoiceValue.Colspan = 1;
                CellInvoiceValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellInvoiceValue.UseVariableBorders = false;
                                
                //  Generate Table Data from Job Detail

                decimal totalInvoiceValue = 0;

                foreach (DataRow dr in dsJobDetail.Tables[0].Rows)
                {
                    i = i + 1;
                    // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                    strConsignee    =   dr["ConsigneeName"].ToString();
                    strCustRefNo    =   dr["CustRefNo"].ToString();
                    strBOENO        =   dr["BOENo"].ToString();
                    strBETypeName   =   dr["BETypeName"].ToString();
                    strInvoiceNo    =   dr["InvoiceNo"].ToString();
                    strInvoiceValue =   dr["InvoiceValue"].ToString();
                    strCurrency     =   dr["Currency"].ToString();
                    strMAWBLNo      =   dr["MAWBNo"].ToString();

                    if(dr["BOEDate"] != DBNull.Value)
                        strBOEDate = Convert.ToDateTime(dr["BOEDate"]).ToShortDateString();

                    if (dr["InvoiceDate"] != DBNull.Value)
                        strInvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToShortDateString();
                    
                    // Add Cell Data To Table

                    // Serial number #
                    SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdftable.AddCell(SrnoCell);

                    // Cell Customer
                    CellCustomer.Phrase = new Phrase(strConsignee, TextFontformat);
                    pdftable.AddCell(CellCustomer);

                    // Cell Cust Ref No
                    CellCustRefNo.Phrase = new Phrase(strCustRefNo, TextFontformat);
                    pdftable.AddCell(CellCustRefNo);

                    // Cell Invoice No
                    CellInvoiceNo.Phrase = new Phrase(strInvoiceNo, TextFontformat);
                    pdftable.AddCell(CellInvoiceNo);

                    // Cell Invoice Date
                    CellInvoiceDate.Phrase = new Phrase(strInvoiceDate, TextFontformat);
                    pdftable.AddCell(CellInvoiceDate);

                    // Cell BL NO
                    CellBLNo.Phrase = new Phrase(strMAWBLNo, TextFontformat);
                    pdftable.AddCell(CellBLNo);

                    // Cell BOE NO
                    CellBOENo.Phrase = new Phrase(strBOENO, TextFontformat);
                    pdftable.AddCell(CellBOENo);

                    // Cell BOE Date
                    CellBOEDate.Phrase = new Phrase(strBOEDate, TextFontformat);
                    pdftable.AddCell(CellBOEDate);

                    // Cell BOE Type
                    CellBEStatus.Phrase = new Phrase(strBETypeName, TextFontformat);
                    pdftable.AddCell(CellBEStatus);

                    // Cell Invoice Value /Amount
                    CellInvoiceValue.Phrase = new Phrase(strInvoiceValue, TextFontformat);
                    pdftable.AddCell(CellInvoiceValue);

		    if(strInvoiceValue != "")
                       totalInvoiceValue += Convert.ToDecimal(strInvoiceValue);

                }// END_ForEach

                // Add Footer Row
                // Serial number #
                SrnoCell.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(SrnoCell);

                // Cell Customer
                CellCustomer.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellCustomer);

                // Cell Cust Ref No
                CellCustRefNo.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellCustRefNo);

                // Cell Invoice No
                CellInvoiceNo.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellInvoiceNo);

                // Cell Invoice Date
                CellInvoiceDate.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellInvoiceDate);

                // Cell BL NO
                CellBLNo.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellBLNo);

                // Cell BOE NO
                CellBOENo.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellBOENo);

                // Cell BOE Date
                CellBOEDate.Phrase = new Phrase("", TextFontformat);
                pdftable.AddCell(CellBOEDate);
                // Cell Total
                CellBEStatus.Phrase = new Phrase("TOTAL", TextFontformat);
                pdftable.AddCell(CellBEStatus);

                // Cell Total Invoice Value
                CellInvoiceValue.Phrase = new Phrase(totalInvoiceValue.ToString(), TextFontformat);
                pdftable.AddCell(CellInvoiceValue);

                pdfDoc.Add(pdftable);

                Paragraph ParaSpacing = new Paragraph();
                ParaSpacing.SpacingBefore = 5;//10

                pdfDoc.Add(new Paragraph("Kindly acknowledge the receipt of the same.", TextFontformat));

                pdfDoc.Add(ParaSpacing);

                pdfDoc.Add(new Paragraph("Regards,", TextFontformat));
                                    
                pdfDoc.Add(ParaSpacing);

                string footerText = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD.";

                pdfDoc.Add(new Paragraph(footerText, GridHeadingFont));
                
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }//END_Try

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblMessage.Text = "Job Details Not Found";
            lblMessage.CssClass = "errorMsg";
        }
    }

    private void GenerateBASFPdf(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);

        string UserName = LoggedInUser.glEmpName;

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string date = DateTime.Today.ToShortDateString();

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {            
            int rowCount = dsJobDetail.Tables[0].Rows.Count;

            string strJobReNo = "";
            string strCustRefNo = "";
            string strBOENO = "";
            string strBOEDate = "";
            string strInvoiceNo = "";
            
            try
            {
                // Generate PDF
                int i = 0; // Auto Increment Table Cell For Serial number
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=BASF-PCD-Cover" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);
                StringReader sr = new StringReader(sw.ToString());

                Rectangle recPDF = new Rectangle(PageSize.A4);

                // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                // Set PDF Document size and Left,Right,Top and Bottom margin

                Document pdfDoc = new Document(recPDF);

                // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                pdfDoc.Open();

                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                logo.SetAbsolutePosition(380, 720);

                logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                pdfDoc.Add(logo);

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDBASFCover.htm"));
                contents = contents.Replace("[TodayDate]", date.ToString());
                
                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                foreach (var htmlelement in parsedContent)
                    pdfDoc.Add(htmlelement as IElement);

                PdfPTable pdftable = new PdfPTable(6);

                pdftable.TotalWidth = 520f;
                pdftable.LockedWidth = true;
                float[] widths = new float[] { 0.07f, 0.3f, 0.3f, 0.3f, 0.3f, 0.3f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                // Set Table Spacing Before And After html text
                //   pdftable.SpacingBefore = 10f;
                pdftable.SpacingAfter = 8f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell cellwithdata = new PdfPCell(new Phrase("SR NO", GridHeadingFont));
                pdftable.AddCell(cellwithdata);
                
                // Header: BSJObRefNo
                PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Babaji Ref.", GridHeadingFont));
                pdftable.AddCell(cellwithdata1);


                // Header: Cust RefNo /PO No
                PdfPCell cellwithdata2 = new PdfPCell(new Phrase("BASF Ref", GridHeadingFont));
                cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata2);

                // Header: BOE No.
                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Triplicate BE No.", GridHeadingFont));
                cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata4);

                // Header: BOE Date.
                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("BOE Date", GridHeadingFont));
                cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata5);

                // Header: Invoice Number
                PdfPCell cellwithdata6 = new PdfPCell(new Phrase("Supplier's Invoice Number", GridHeadingFont));
                cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata6);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell SrnoCell = new PdfPCell();
                SrnoCell.Colspan = 1;
                SrnoCell.UseVariableBorders = false;

                // Data Cell: Customer

                PdfPCell CellJobRefNo = new PdfPCell();
                CellJobRefNo.Colspan = 1;
                CellJobRefNo.UseVariableBorders = false;

                // Data Cell: Customer RefNo/PO

                PdfPCell CellCustRefNo = new PdfPCell();
                CellCustRefNo.Colspan = 1;
                CellCustRefNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellCustRefNo.UseVariableBorders = false;
                                                
                // Data Cell: BOE No
                PdfPCell CellBOENo = new PdfPCell();
                CellBOENo.Colspan = 1;
                CellBOENo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBOENo.UseVariableBorders = false;

                // Data Cell: BOE Date
                PdfPCell CellBOEDate = new PdfPCell();
                CellBOEDate.Colspan = 1;
                CellBOEDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBOEDate.UseVariableBorders = false;

                // Data Cell: Invoice NO

                PdfPCell CellInvoiceNo = new PdfPCell();
                CellInvoiceNo.Colspan = 1;
                CellInvoiceNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellInvoiceNo.UseVariableBorders = false;
                
                //  Generate Table Data from Job Detail

                foreach (DataRow dr in dsJobDetail.Tables[0].Rows)
                {
                    i = i + 1;
                    
                    strJobReNo      = dr["JobRefNo"].ToString();
                    strCustRefNo    = dr["CustRefNo"].ToString();
                    strBOENO        = dr["BOENo"].ToString();
                    
                    strInvoiceNo    = dr["InvoiceNo"].ToString();
                    
                    if (dr["BOEDate"] != DBNull.Value)
                        strBOEDate = Convert.ToDateTime(dr["BOEDate"]).ToShortDateString();
                                        
                    // Add Cell Data To Table

                    // Serial number #
                    SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdftable.AddCell(SrnoCell);

                    // Serial JobRefNo
                    CellJobRefNo.Phrase = new Phrase(strJobReNo, TextFontformat);
                    pdftable.AddCell(CellJobRefNo);
                    

                    // Cell Cust Ref No
                    CellCustRefNo.Phrase = new Phrase(strCustRefNo, TextFontformat);
                    pdftable.AddCell(CellCustRefNo);

                    // Cell BOE NO
                    CellBOENo.Phrase = new Phrase(strBOENO, TextFontformat);
                    pdftable.AddCell(CellBOENo);

                    // Cell BOE Date
                    CellBOEDate.Phrase = new Phrase(strBOEDate, TextFontformat);
                    pdftable.AddCell(CellBOEDate);
                    
                    // Cell Invoice No
                    CellInvoiceNo.Phrase = new Phrase(strInvoiceNo, TextFontformat);
                    pdftable.AddCell(CellInvoiceNo);
                    
                }// END_ForEach

                pdfDoc.Add(pdftable);

                Paragraph ParaSpacing = new Paragraph();
                ParaSpacing.SpacingBefore = 10;

                pdfDoc.Add(new Paragraph("Siddhi Bhosale,", GridHeadingFont));

                pdfDoc.Add(ParaSpacing);

                string footerText = "Jr.Mgr [Imports]";

                pdfDoc.Add(new Paragraph(footerText, GridHeadingFont));

                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }//END_Try

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblMessage.Text = "Job Details Not Found";
            lblMessage.CssClass = "errorMsg";
        }
    }

    private void GenerateIntlFlavorPdf(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);

        DataTable dtDetail = dsJobDetail.Tables[0].DefaultView.ToTable(true, "JobId", "JobRefNo", "CustRefNo", "BOENo", "TRIPLICATE", "DUPLICATE");
                

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string date = DateTime.Today.ToShortDateString();

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            int rowCount = dsJobDetail.Tables[0].Rows.Count;

            string strJobReNo   = "", strCustRefNo = "", strBOENO   = "",   strTriplicate = "", strDuplicate = "";

            try
            {
                // Generate Intl Flavors PDF
                int i = 0; // Auto Increment Table Cell For Serial number
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Intl-Flavors-PCD-Cover" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);
                StringReader sr = new StringReader(sw.ToString());

                Rectangle recPDF = new Rectangle(PageSize.A4);

                // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                // Set PDF Document size and Left,Right,Top and Bottom margin

                Document pdfDoc = new Document(recPDF);

                // Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                pdfDoc.Open();

                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                logo.SetAbsolutePosition(380, 720);

                logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                pdfDoc.Add(logo);

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDINTL-FLAVORSCover.htm"));
                contents = contents.Replace("[TodayDate]", date.ToString());

                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                foreach (var htmlelement in parsedContent)
                    pdfDoc.Add(htmlelement as IElement);

                PdfPTable pdftable = new PdfPTable(6);

                pdftable.TotalWidth = 520f;
                pdftable.LockedWidth = true;
                float[] widths = new float[] { 0.1f, 0.3f, 0.4f , 0.3f, .22f, .22f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                // Set Table Spacing Before And After html text
                //   pdftable.SpacingBefore = 10f;
                pdftable.SpacingAfter = 8f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell cellwithdata = new PdfPCell(new Phrase("S. NO", GridHeadingFont));
                pdftable.AddCell(cellwithdata);

                // Header: BSJObRefNo
                PdfPCell cellwithdata1 = new PdfPCell(new Phrase("JOB NO", GridHeadingFont));
                pdftable.AddCell(cellwithdata1);


                // Header: Cust RefNo /PO No
                PdfPCell cellwithdata2 = new PdfPCell(new Phrase("PO NO", GridHeadingFont));
                cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata2);

                // Header: BOE No.
                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("BE No.", GridHeadingFont));
                cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata4);

                // Header: TRIPLICATE
                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("TRIPLICATE", GridHeadingFont));
                cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata5);

                // Header: DUPLICATE
                PdfPCell cellwithdata6 = new PdfPCell(new Phrase("DUPLICATE", GridHeadingFont));
                cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdftable.AddCell(cellwithdata6);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell SrnoCell = new PdfPCell();
                SrnoCell.Colspan = 1;
                SrnoCell.UseVariableBorders = false;

                // Data Cell: Job RefNo

                PdfPCell CellJobRefNo = new PdfPCell();
                CellJobRefNo.Colspan = 1;
                CellJobRefNo.UseVariableBorders = false;

                // Data Cell: Customer RefNo/PO

                PdfPCell CellCustRefNo = new PdfPCell();
                CellCustRefNo.Colspan = 1;
                CellCustRefNo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellCustRefNo.UseVariableBorders = false;

                // Data Cell: BOE No
                PdfPCell CellBOENo = new PdfPCell();
                CellBOENo.Colspan = 1;
                CellBOENo.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellBOENo.UseVariableBorders = false;

                // Data Cell: TRIPLICATE
                PdfPCell CellTriple = new PdfPCell();
                CellTriple.Colspan = 1;
                CellTriple.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellTriple.UseVariableBorders = false;

                // Data Cell: Duplicate

                PdfPCell CellDuplicate = new PdfPCell();
                CellDuplicate.Colspan = 1;
                CellDuplicate.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellDuplicate.UseVariableBorders = false;

                //  Generate Table Data from Job Detail

                foreach (DataRow dr in dtDetail.Rows)
                {
                    i = i + 1;

                    strJobReNo      = dr["JobRefNo"].ToString();
                    strCustRefNo    = dr["CustRefNo"].ToString();
                    strBOENO        = dr["BOENo"].ToString();
                    strTriplicate   = dr["TRIPLICATE"].ToString();
                    strDuplicate    = dr["DUPLICATE"].ToString();

                    // Add Cell Data To Table

                    // Serial number #
                    SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdftable.AddCell(SrnoCell);

                    // Serial JobRefNo
                    CellJobRefNo.Phrase = new Phrase(strJobReNo, TextFontformat);
                    pdftable.AddCell(CellJobRefNo);


                    // Cell Cust Ref No
                    CellCustRefNo.Phrase = new Phrase(strCustRefNo, TextFontformat);
                    pdftable.AddCell(CellCustRefNo);

                    // Cell BOE NO
                    CellBOENo.Phrase = new Phrase(strBOENO, TextFontformat);
                    pdftable.AddCell(CellBOENo);

                    // Cell Triplicate
                    CellTriple.Phrase = new Phrase(strTriplicate, TextFontformat);
                    pdftable.AddCell(CellTriple);

                    // Cell Duplicate
                    CellDuplicate.Phrase = new Phrase(strDuplicate, TextFontformat);
                    pdftable.AddCell(CellDuplicate);

                }// END_ForEach

                pdfDoc.Add(pdftable);

                Paragraph ParaSpacing = new Paragraph();
                ParaSpacing.SpacingBefore = 5;//10

                pdfDoc.Add(new Paragraph("Please acknowledge the receipt of the same.", TextFontformat));

                pdfDoc.Add(ParaSpacing);

                pdfDoc.Add(new Paragraph("Thanking You.", TextFontformat));

                pdfDoc.Add(ParaSpacing);


                pdfDoc.Add(new Paragraph("Yours truly,", TextFontformat));

                pdfDoc.Add(ParaSpacing);

                string footerText = "For Babaji Shivram Clearing & Carriers Pvt Ltd";

                pdfDoc.Add(new Paragraph(footerText, GridHeadingFont));

                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }//END_Try

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblMessage.Text = "Job Details Not Found";
            lblMessage.CssClass = "errorMsg";
        }
    }

    private void GenerateDOWExcel(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);
        //Create a dummy GridView
                
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.AutoGenerateColumns = false;
        GridView1.ShowHeader = true;
        GridView1.ShowFooter = true;

        BoundField a = new BoundField();
        a.HeaderText = "Sr No";
        GridView1.Columns.Add(a);

        BoundField b = new BoundField();
        b.DataField = "CustName";
        b.HeaderText = "Cutstomer Name"; 
        GridView1.Columns.Add(b);

        BoundField c = new BoundField();
        c.DataField = "CustRefNo";
        c.HeaderText = "PO /STO No."; 
        GridView1.Columns.Add(c);

        BoundField d = new BoundField();
        d.DataField = "InvoiceNo";
        d.HeaderText = "Invoice No";
        GridView1.Columns.Add(d);

        BoundField e = new BoundField();
        e.DataField = "InvoiceDate";
        e.HeaderText = "Date";
        GridView1.Columns.Add(e);

        BoundField f = new BoundField();
        f.DataField = "MAWBNo";
        f.HeaderText = "B/L No";
        GridView1.Columns.Add(f);

        BoundField g = new BoundField();
        g.DataField = "BOENo";
        g.HeaderText = "BOE No";
        GridView1.Columns.Add(g);
        
        BoundField g1 = new BoundField();
        g1.DataField = "BOEDate";
        g1.HeaderText = "BOE Date";
        GridView1.Columns.Add(g1);

        BoundField h = new BoundField();
        h.DataField = "BETypeName";
        h.HeaderText = "BE Status";
        GridView1.Columns.Add(h);

        BoundField i = new BoundField();
        i.DataField = "InvoiceValue";
        i.HeaderText = "Amt(USD)";
        GridView1.Columns.Add(i);

        GridView1.DataSource = dsJobDetail;
        GridView1.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=DOW-PCD-Cover.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                Label lblheader = new Label();

                //
                string date = DateTime.Today.ToShortDateString();
                string UserName = LoggedInUser.glEmpName;

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDDow.txt"));
                contents = contents.Replace("[TodayDate]", date.ToString());
                contents = contents.Replace("[FROMName]", UserName);

                lblheader.Text = @contents;//@"<b style=""font-size:small"">" + Header + "</b>" + "<br/>";
                lblheader.RenderControl(htw);
                //  Create a form to contain the grid
                Table table = new Table();
                
                table.GridLines = GridView1.GridLines;

                //  add the header row to the table
                if (GridView1.HeaderRow != null)
                {
                    ExportControl_Grid(GridView1.HeaderRow);
                    table.Rows.Add(GridView1.HeaderRow);
                }

                int AutoInc = 1;
                decimal TotalInvoice = 0;

                //  add each of the data rows to the table
                foreach (GridViewRow row in GridView1.Rows)
                {
                  //  ExportControl_Grid(row);
                    row.Cells[0].Text = AutoInc.ToString();

		    row.Cells[9].Text = row.Cells[9].Text.Replace("&nbsp;", "");

                    if(row.Cells[9].Text.Trim() != "")
                    {
                        TotalInvoice = TotalInvoice + Convert.ToDecimal(row.Cells[9].Text.Trim());
                    }

                    table.Rows.Add(row);

                    AutoInc = AutoInc + 1;
                }

                //  add the footer row to the table
                if (GridView1.FooterRow != null)
                {
                    ExportControl_Grid(GridView1.FooterRow);

                    GridView1.FooterRow.Cells[8].Text = "TOTAL";
                    GridView1.FooterRow.Cells[9].Text = TotalInvoice.ToString();

                    table.Rows.Add(GridView1.FooterRow);
                }
                 
                //  render the table into the htmlwriter
                table.RenderControl(htw);

                Label lblFooter = new Label();
                lblFooter.Text = @"<BR><BR>Regards,<BR>BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD.";
                lblFooter.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();

            }
        }
    }

    private void GenerateBASFExcel(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);
        //Create a dummy GridView

        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.AutoGenerateColumns = false;
        GridView1.ShowHeader = true;
        GridView1.ShowFooter = false;

        BoundField a = new BoundField();
        a.HeaderText = "S R NO";
        GridView1.Columns.Add(a);

        BoundField b = new BoundField();
        b.DataField = "JobRefNo";
        b.HeaderText = "Babaji Ref";
        GridView1.Columns.Add(b);

        BoundField c = new BoundField();
        c.DataField = "CustRefNo";
        c.HeaderText = "BASF Ref";
        GridView1.Columns.Add(c);
                

        BoundField d = new BoundField();
        d.DataField = "BOENo";
        d.HeaderText = "Triplicate BE No.";
        GridView1.Columns.Add(d);

        BoundField e = new BoundField();
        e.DataField = "BOEDate";
        e.HeaderText = "Date";
        GridView1.Columns.Add(e);

        BoundField f = new BoundField();
        f.DataField = "InvoiceNo";
        f.HeaderText = "Supplier's Invoice Number";
        GridView1.Columns.Add(f);

        GridView1.DataSource = dsJobDetail;
        GridView1.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=BASF-PCD-Cover.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                Label lblheader = new Label();

                //
                string date = DateTime.Today.ToShortDateString();
                string UserName = LoggedInUser.glEmpName;

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDBASF.txt"));
                contents = contents.Replace("[TodayDate]", date.ToString());
                
                lblheader.Text = @contents;//@"<b style=""font-size:small"">" + Header + "</b>" + "<br/>";
                lblheader.RenderControl(htw);
                //  Create a form to contain the grid
                Table table = new Table();

                table.GridLines = GridView1.GridLines;

                //  add the header row to the table
                if (GridView1.HeaderRow != null)
                {
                    ExportControl_Grid(GridView1.HeaderRow);
                    table.Rows.Add(GridView1.HeaderRow);
                }

                int AutoInc = 1;
                
                //  add each of the data rows to the table
                foreach (GridViewRow row in GridView1.Rows)
                {
                    //  ExportControl_Grid(row);
                    row.Cells[0].Text = AutoInc.ToString();

                    table.Rows.Add(row);

                    AutoInc = AutoInc + 1;
                }

                
                //  render the table into the htmlwriter
                table.RenderControl(htw);

                Label lblFooter = new Label();
                lblFooter.Text = @"<BR><BR>Siddhi Bhosale,<BR>Jr.Mgr [Imports]";
                lblFooter.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();

            }
        }
    }

    private void GenerateIntlFlavorExcel(string strJobIDList)
    {
        DataSet dsJobDetail = DBOperations.GetJobDetailByJobIDList(strJobIDList);

        DataTable dtDetail = dsJobDetail.Tables[0].DefaultView.ToTable(true, "JobId","JobRefNo","CustRefNo","BOENo","TRIPLICATE","DUPLICATE");

        
        //Create a dummy GridView

        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.AutoGenerateColumns = false;
        GridView1.ShowHeader = true;
        GridView1.ShowFooter = false;

        BoundField a = new BoundField();
        a.HeaderText = "S. NO";
        GridView1.Columns.Add(a);

        BoundField b = new BoundField();
        b.DataField = "JobRefNo";
        b.HeaderText = "JOB NO";
        GridView1.Columns.Add(b);

        BoundField c = new BoundField();
        c.DataField = "CustRefNo";
        c.HeaderText = "PO NO";
        GridView1.Columns.Add(c);


        BoundField d = new BoundField();
        d.DataField = "BOENo";
        d.HeaderText = "BE NO";
        GridView1.Columns.Add(d);

        BoundField e = new BoundField();
        e.DataField = "TRIPLICATE";
        e.HeaderText = "TRIPLICATE";
        GridView1.Columns.Add(e);

        BoundField f = new BoundField();
        f.DataField = "DUPLICATE";
        f.HeaderText = "DUPLICATE";
        GridView1.Columns.Add(f);

        GridView1.DataSource = dtDetail;
        GridView1.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Intl-Flavors-PCD-Cover.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                Label lblheader = new Label();

                //
                string date = DateTime.Today.ToShortDateString();
                string UserName = LoggedInUser.glEmpName;

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("~//EmailTemplate//PCDINTL-FLAVORS.txt"));
                contents = contents.Replace("[TodayDate]", date.ToString());

                lblheader.Text = @contents;
                lblheader.RenderControl(htw);
                //  Create a form to contain the grid
                Table table = new Table();

                table.GridLines = GridView1.GridLines;

                //  add the header row to the table
                if (GridView1.HeaderRow != null)
                {
                    ExportControl_Grid(GridView1.HeaderRow);
                    table.Rows.Add(GridView1.HeaderRow);
                }

                int AutoInc = 1;

                //  add each of the data rows to the table
                foreach (GridViewRow row in GridView1.Rows)
                {
                    //  ExportControl_Grid(row);
                    row.Cells[0].Text = AutoInc.ToString();

                    table.Rows.Add(row);

                    AutoInc = AutoInc + 1;
                }


                //  render the table into the htmlwriter
                table.RenderControl(htw);

                Label lblFooter = new Label();
                lblFooter.Text = @"<BR><BR>Please acknowledge the receipt of the same.,<BR>Thanking You.<BR>Yours truly,"+
                    "<BR>For Babaji Shivram Clearing & Carriers Pvt Ltd";
                lblFooter.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();

            }
        }
    }

    private static void ExportControl_Grid(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }           
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
               
                else if (current is Panel)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl(" "));
                }
                if (current.HasControls())
                {
                    ExportControl_Grid(current);
                }
            }
        }
    
    #endregion
}
