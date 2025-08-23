using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
public partial class FreightOperation_DeliveryOrder : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkCreateDOPDF);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingDO.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Delivery Order";
            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            if (dsBookingDetail.Tables[0].Rows[0]["FRJobNo"] != DBNull.Value)
            {
                lblJobNo.Text = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();

                lblBookingMonth.Text =  Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");
		txtChaName.Text     =   dsBookingDetail.Tables[0].Rows[0]["CHAName"].ToString();
                txtDoIssuedTo.Text  =   dsBookingDetail.Tables[0].Rows[0]["DOIssuedTo"].ToString();
                txtChequeNo.Text    =   dsBookingDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
                txtAmount.Text      =   dsBookingDetail.Tables[0].Rows[0]["DOAmount"].ToString();
                txtRemark.Text      =   dsBookingDetail.Tables[0].Rows[0]["DORemark"].ToString();

                if (dsBookingDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                    txtChequeDate.Text  =   Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");
                
                ddPaymentsTerms.SelectedValue = dsBookingDetail.Tables[0].Rows[0]["PaymentTerm"].ToString();
                ddPaymentType.SelectedValue = dsBookingDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        /**************************************/

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strCHAName, strDOIssuedTo ,strChequeNo, strRemark; 
        int PaymentTerm = 0, PaymentId = 0;
        decimal DOAmount = 0;
        DateTime dtChequeDate = DateTime.MinValue;
        
        strCHAName      =   txtChaName.Text.Trim();
        PaymentTerm     =   Convert.ToInt32(ddPaymentsTerms.SelectedValue);
        PaymentId       =   Convert.ToInt32(ddPaymentType.SelectedValue); 
        strDOIssuedTo   =   txtDoIssuedTo.Text.Trim();
        strChequeNo     =   txtChequeNo.Text.Trim();
	strRemark       =   txtRemark.Text.Trim();

	if (txtAmount.Text.Trim() != "")
        {
            DOAmount = Convert.ToDecimal(txtAmount.Text.Trim());
        }

        if (txtChequeDate.Text.Trim() != "")
        { 
            dtChequeDate    =   Commonfunctions.CDateTime(txtChequeDate.Text.Trim());
        }

        int result = DBOperations.AddDeliveryOrder(EnqId, strCHAName, PaymentTerm, strDOIssuedTo,PaymentId,strChequeNo,
            dtChequeDate, DOAmount, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "DO Detail Updated Successfully!";
            lblError.CssClass = "success";
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "DO Detail Already Exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
        /*******************************************************/
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingDO.aspx");
    }

    protected void ddPaymentsTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddPaymentsTerms.SelectedValue == "2")
            RFVPaytype.Enabled = true;
        else
            RFVPaytype.Enabled = false;
        
    }

    protected void lnkCreateDOPDF_Click(object sender, EventArgs e)
    { 
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        DataSet dsDOPrint = DBOperations.GetOperationDetail(EnqId);

        if (dsDOPrint.Tables[0].Rows.Count > 0)
        {
            int ModeId = Convert.ToInt32(dsDOPrint.Tables[0].Rows[0]["lMode"]);

            if (ModeId == (Int32)TransMode.Air)
            {
                GeneratePdfAIR(EnqId, dsDOPrint);
            }
            else
            {
                GeneratePdfSEA(EnqId, dsDOPrint);
            }
        }
        else
        {
            lblError.Text       = "Job Details Not Found!";
            lblError.CssClass   = "errorMsg";
        }
    }

private void GeneratePdfAIR(int EnqId, DataSet dsDOAirPrint)
    {
        string strHOAddress1    =   "Babaji Shivram Clearing and Carriers Pvt. Ltd.";
        string strHOAddress2    =   "Plot no. 2, Behind Excom House";
        string strHOAddress3    =   "Saki Vihar Road, Saki Naka";
        string strHOAddress4    =   "Mumbai - 400072, INDIA.";

        string FRJobNo          =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["FRJobNo"]);
        string Consignee         =  Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["Consignee"]);
        string ConsigneeAddress =   Consignee +", "+ Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ConsigneeAddress"]);

        string VesselNumber     =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["VesselNumber"]);
        string IGMNo            =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["IGMNo"]);
        string NoOfPkgs         =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["NoOfPackages"]);
        string GrossWeight      =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["GrossWeight"]);
        string ChargeableWeight =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ChargeableWeight"]);
        string CargoDescription =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["CargoDescription"]);
        string MBLNo            =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["MBLNo"]);
        string HBLNo            =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["HBLNo"]);
        string PortCity         =   Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["PortCity"]);

        string ATADate = "", IGMDate = "";

        if (dsDOAirPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
        {
            ATADate = Convert.ToDateTime(dsDOAirPrint.Tables[0].Rows[0]["ATADate"]).ToString("dd/MM/yyyy");
        }

        if (dsDOAirPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
        {
            IGMDate = Convert.ToDateTime(dsDOAirPrint.Tables[0].Rows[0]["IGMDate"]).ToString("dd/MM/yyyy");
        }
            
        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string dateToday = DateTime.Today.ToString("dd/MM/yyyy");
            
        try
        {
            // Generate PDF
                
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrder-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

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

            Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

            logo.SetAbsolutePosition(380, 700);

            logo.Alignment = Convert.ToInt32(ImageAlign.Right);
            pdfDoc.Add(logo);

            string contents = "";
            contents = File.ReadAllText(Server.MapPath("DOLetterAIR.htm"));
            contents = contents.Replace("[TodayDate]", dateToday.ToString());
            contents = contents.Replace("[HOAddress1]", strHOAddress1);
            contents = contents.Replace("[HOAddress2]", strHOAddress2);
            contents = contents.Replace("[HOAddress3]", strHOAddress3);
            contents = contents.Replace("[HOAddress4]", strHOAddress4);
            contents = contents.Replace("[PortCity]", PortCity);
                    
            contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
            contents = contents.Replace("[FlightNo]", VesselNumber);
            contents = contents.Replace("[ArrivalDate]", ATADate);

            contents = contents.Replace("[HAWBL]", HBLNo);
            contents = contents.Replace("[MAWBL]", MBLNo);
                    
            contents = contents.Replace("[IGMNo]", IGMNo);
            contents = contents.Replace("[IGMDate]", IGMDate);
            contents = contents.Replace("[NoOfPkgs]", NoOfPkgs);
            contents = contents.Replace("[GrossWeight]", GrossWeight);
            contents = contents.Replace("[ChargeWeight]", ChargeableWeight);
                    
            contents = contents.Replace("[Description]", CargoDescription);


            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);
                    
            Paragraph ParaSpacing = new Paragraph();
            ParaSpacing.SpacingBefore = 40;//5

            pdfDoc.Add(ParaSpacing);
                    
            // For Footer Signature

            PdfPTable pt = new PdfPTable(1);
            PdfPCell _cell;

            _cell = new PdfPCell(new Phrase("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            _cell.ExtraParagraphSpace = 40;
            pt.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("Authorised Signatory", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            pt.AddCell(_cell);

            pdfDoc.Add(pt);
                    
            // Footer Image Commented
            // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
            // footer.SetAbsolutePosition(30, 0);
            // pdfDoc.Add(footer);
            // pdfwriter.PageEvent = new PDFFooter();

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }//END_Try

        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    private void GeneratePdfSEA(int EnqId, DataSet dsDOSeaPrint)
    {
        string FRJobNo      =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FRJobNo"]).Trim();
        string Customer     =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["Customer"]).Trim();
        string FinalAgent   =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FinalAgent"]).Trim();
        string VesselName   =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["VesselName"]).Trim();
        string IGMNo        =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["IGMNo"]).Trim();
        string MBLNo        =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["MBLNo"]).Trim();
        string HBLNo        =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["HBLNo"]).Trim();
        string ItemNo       =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["ItemNo"]).Trim();
        string ChaName      =   Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["CHAName"]).Trim();
        string ConsigneeName =  Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["Consignee"]).Trim();

        if (ChaName == "")
            ChaName = txtChaName.Text.Trim();

        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

        string dateToday = DateTime.Now.ToString("dd/MM/yyyy");

        try
        {
            // Generate PDF
           
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DeliveryOrder-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();

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

            Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

            logo.SetAbsolutePosition(380, 700);

            logo.Alignment = Convert.ToInt32(ImageAlign.Right);
            pdfDoc.Add(logo);

            string contents = "";
            contents = File.ReadAllText(Server.MapPath("DOLetterSEA.htm"));
            contents = contents.Replace("[TodayDate]", dateToday.ToString());

            contents = contents.Replace("[JobRefNo]", FRJobNo); 
            contents = contents.Replace("[FinalAgent]", FinalAgent);
            contents = contents.Replace("[VesselName]", VesselName);

            contents = contents.Replace("[MAWBL]", MBLNo);
            contents = contents.Replace("[HAWBL]", HBLNo);

            contents = contents.Replace("[ConsigneeName]", ConsigneeName);
            contents = contents.Replace("[IGMNO]", IGMNo);
            contents = contents.Replace("[ItemNO]", ItemNo);
            contents = contents.Replace("[CHAName]", ChaName);
                    
            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);

            Paragraph ParaSpacing = new Paragraph();
            ParaSpacing.SpacingBefore = 40;//5

            pdfDoc.Add(ParaSpacing);

            // For Footer Signature

            /*************************************
            PdfPTable pt = new PdfPTable(1);
            PdfPCell _cell;

            _cell = new PdfPCell(new Phrase("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
            _cell.VerticalAlignment = Element.ALIGN_RIGHT;
            _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _cell.Border = 0;
            _cell.ExtraParagraphSpace = 40;
            pt.AddCell(_cell);

            pdfDoc.Add(pt);
            *************************************/
            // Footer Image Commented
            // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
            // footer.SetAbsolutePosition(30, 0);
            // pdfDoc.Add(footer);
            // pdfwriter.PageEvent = new PDFFooter();

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }//END_Try

        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }
}