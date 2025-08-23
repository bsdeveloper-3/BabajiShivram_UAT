using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
public partial class Freight_ViewFreight : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(fvCAN);
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("FreightAwarded.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Detail";
                        
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }

        // Allow  Only Future Date For Reminder
        calRemindDate.StartDate = DateTime.Today;

        // Set Minimum Reminder Date Today
        MskValRemindDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");

    }
        
    #region Freight Detail
    
    private void GetFreightDetail(int Enqid)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Get Agent Details
        FillEnquiryAgent(Enqid);

        // Freight Operation Detail

        DataSet dsDetail = DBOperations.GetOperationDetail(Enqid);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            FVFreightDetail.DataSource = dsDetail;
            FVFreightDetail.DataBind();

            fvAgentPreAlert.DataSource = dsDetail;
            fvAgentPreAlert.DataBind();

            fvCustomerPreAlert.DataSource = dsDetail;
            fvCustomerPreAlert.DataBind();

            fvCAN.DataSource = dsDetail;
            fvCAN.DataBind();

            fvDelivery.DataSource = dsDetail;
            fvDelivery.DataBind();

            fvAdvice.DataSource = dsDetail;
            fvAdvice.DataBind();

            fvAgentInvoice.DataSource = dsDetail;
            fvAgentInvoice.DataBind();

            fvFreightStatus.DataSource = dsDetail;
            fvFreightStatus.DataBind();

            lblTitle.Text = "Freight Detail - " + dsDetail.Tables[0].Rows[0]["EnqRefNo"].ToString();
            
            hdnUploadPath.Value = dsDetail.Tables[0].Rows[0]["DocDir"].ToString();

            hdnModeId.Value = dsDetail.Tables[0].Rows[0]["lMode"].ToString();

            if (dsDetail.Tables[0].Rows[0]["lStatus"] != DBNull.Value)
            {
                hdnStatusId.Value = dsDetail.Tables[0].Rows[0]["lStatus"].ToString();

                if (Convert.ToInt32(hdnStatusId.Value) > 15)
                {
                    Label lblStatusMsg = (Label) fvFreightStatus.FindControl("lblStatusMsg");
                    Button btnStatusChange = (Button)fvFreightStatus.FindControl("btnStatusChange");

                    btnStatusChange.Visible = false;
                    lblStatusMsg.Text = "Billing Advice Completed! Status not allowed to change! Please contact Administrator!";
                    lblStatusMsg.CssClass = "errorMsg";

                }
            }

            if (dsDetail.Tables[0].Rows[0]["CountryId"] != DBNull.Value)
            {
                int CountryID = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["CountryId"]);

                hdnCountryId.Value = CountryID.ToString();
                
                FillAgentDetail(CountryID);
            }

            if (Convert.ToInt32(hdnModeId.Value) > 1)
            {
                accContainer.Visible = true;

                Panel pnlSea = (Panel)FVFreightDetail.FindControl("pnlSea");

                if (pnlSea != null)
                    pnlSea.Visible = true;
            }
            else
            {
                // For AIR  -- Container Details Not Required
                accContainer.Visible = false;
            }
            // Fill Pkgs Type

            DropDownList ddPackageType = (DropDownList)FVFreightDetail.FindControl("ddPackageType");

            if (ddPackageType != null)
            {
                string strPackageTypeId = dsDetail.Tables[0].Rows[0]["PackageTypeId"].ToString();

                DBOperations.FillPackageType(ddPackageType);

                ddPackageType.SelectedValue = strPackageTypeId;
            }
        }
    }

    protected void btnFreightCancel_Click(object sender, EventArgs e)
    {        
    }

    protected void FVFreightDetail_DataBound(object sender, EventArgs e)
    {
        if (FVFreightDetail.CurrentMode == FormViewMode.ReadOnly)
        {
            if (hdnModeId.Value == "1") // AIR
            {
                Panel pnlAir = (Panel)(FVFreightDetail.FindControl("pnlAir"));

                if (pnlAir != null)
                    pnlAir.Visible = true;
            }
            else // Sea and Breakbulk
            {
                Panel pnlSea = (Panel)(FVFreightDetail.FindControl("pnlSea"));

                if (pnlSea != null)
                    pnlSea.Visible = true;
            }
        }
    }

    public string GetBooleanToYesNo(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Yes";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "No";
        }

        return strReturnText;
    }

    public bool CheckNullBooleanToTrueFalse(object mybValue)
    {
        bool bReturnValue = false;

        if (mybValue != DBNull.Value)
        {
            bReturnValue = Convert.ToBoolean(mybValue);
        }

        return bReturnValue;
    }

    public string GetBooleanToCompletedPending(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Completed";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Pending";
        }

        return strReturnText;
    }

    public string GetBooleanToApprovedRejected(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Approved";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Rejected";
        }

        return strReturnText;
    }

    #endregion

    #region CAN
                
    protected void lnkCreateCANPdf_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        GenerateCANPdf(EnqId);
    }

    private void GenerateCANPdf(int EnqId)
    {
        DataSet dsCANPrint = DBOperations.GetOperationDetail(EnqId);

        if (dsCANPrint.Tables[0].Rows.Count > 0)
        {
            int FreightMode = Convert.ToInt32(dsCANPrint.Tables[0].Rows[0]["lMode"]);

            string FRJobNo = dsCANPrint.Tables[0].Rows[0]["FRJobNo"].ToString();
            string Customer = dsCANPrint.Tables[0].Rows[0]["Customer"].ToString();
            string Consignee = dsCANPrint.Tables[0].Rows[0]["Consignee"].ToString();
            string Shipper = dsCANPrint.Tables[0].Rows[0]["Shipper"].ToString();
            string ConsigneeAddress = dsCANPrint.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
            string ShipperAddress = dsCANPrint.Tables[0].Rows[0]["ShipperAddress"].ToString();

            string MBLNo = dsCANPrint.Tables[0].Rows[0]["MBLNo"].ToString();
            string HBLNo = dsCANPrint.Tables[0].Rows[0]["HBLNo"].ToString();
            string InvoiceNo = dsCANPrint.Tables[0].Rows[0]["InvoiceNo"].ToString();
            string PONumber = dsCANPrint.Tables[0].Rows[0]["PONumber"].ToString();
            string VesselName = dsCANPrint.Tables[0].Rows[0]["VesselName"].ToString();
            string VesselNo = dsCANPrint.Tables[0].Rows[0]["VesselNumber"].ToString();
            string LoadingPort = dsCANPrint.Tables[0].Rows[0]["LoadingPortName"].ToString();
            string PortOfDischarged = dsCANPrint.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            string NoOfPackages = dsCANPrint.Tables[0].Rows[0]["NoOfPackages"].ToString();
            string GrossWeight = dsCANPrint.Tables[0].Rows[0]["GrossWeight"].ToString();
            string ChargeableWeight = dsCANPrint.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            string LCLCBM = dsCANPrint.Tables[0].Rows[0]["LCLVolume"].ToString();
            string IGMNo = dsCANPrint.Tables[0].Rows[0]["IGMNo"].ToString();
            string ItemNo = dsCANPrint.Tables[0].Rows[0]["ItemNo"].ToString();
            string strDescription = dsCANPrint.Tables[0].Rows[0]["CargoDescription"].ToString();
            //string strServiceTax    =   "14 %";  // 
            //string strSBCTax        =   "0.5 %"; // Swatchh Bharat Cess - 0.5%
            string ATA = "", IGMDate = "";

            if (dsCANPrint.Tables[0].Rows[0]["IGMDate"] != DBNull.Value)
                IGMDate = " & " + Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["IGMDate"]).ToShortDateString();

            if (dsCANPrint.Tables[0].Rows[0]["ATADate"] != DBNull.Value)
                ATA = Convert.ToDateTime(dsCANPrint.Tables[0].Rows[0]["ATADate"]).ToShortDateString();

            string CanUserName = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));

            string date = DateTime.Today.ToShortDateString();
            TextBox txtCANDate = (TextBox)fvCAN.FindControl("txtCANDate");

            if (txtCANDate != null)
            {
                if (txtCANDate.Text.Trim() != "")
                    date = txtCANDate.Text.Trim();
            }

            DataSet dsCanInvoice = DBOperations.GetFreightCANPrintInvoice(EnqId);

            try
            {
                if (dsCanInvoice.Tables[0].Rows.Count > 0)
                {
              
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=CAN Letter-" + FRJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
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
                    contents = File.ReadAllText(Server.MapPath("../FreightOperation/CANLetter.htm"));
                    contents = contents.Replace("[TodayDate]", date.ToString());
                    contents = contents.Replace("[JobRefNO]", FRJobNo);
                    //   contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[ConsigneeName]", Consignee);
                    contents = contents.Replace("[ShipperName]", Shipper);

                    contents = contents.Replace("[ConsigneeAddress]", ConsigneeAddress);
                    contents = contents.Replace("[ShipperAddress]", ShipperAddress);

                    contents = contents.Replace("[MAWBL]", MBLNo);
                    contents = contents.Replace("[HAWBL]", HBLNo);
                    contents = contents.Replace("[InvoiceNo]", InvoiceNo);
                    contents = contents.Replace("[PONo]", PONumber);

                    contents = contents.Replace("[VesselName]", VesselName);
                    contents = contents.Replace("[VesselNo]", VesselNo);
                    contents = contents.Replace("[OriginPort]", LoadingPort);
                    contents = contents.Replace("[DestinationPort]", PortOfDischarged);

                    contents = contents.Replace("[NoofPkgs]", NoOfPackages);
                    contents = contents.Replace("[GrossWeight]", GrossWeight);

                    contents = contents.Replace("[ArrivalDate]", ATA);
                    contents = contents.Replace("[IGMNo]", IGMNo);
                    contents = contents.Replace("[IGMDate]", IGMDate);
                    contents = contents.Replace("[ITEMNo]", ItemNo);
                    contents = contents.Replace("[CargoDescription]", strDescription);

                    if (FreightMode == 1) // AIR
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CHARG WEIGHT (KGS)");
                        contents = contents.Replace("[ValueChargCBM]", ChargeableWeight);
                    }
                    else // SEA/ Breakbulk
                    {
                        contents = contents.Replace("[lblChargeCBM]", "CBM");
                        contents = contents.Replace("[ValueChargCBM]", LCLCBM);
                    }

                    var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                    foreach (var htmlelement in parsedContent)
                        pdfDoc.Add(htmlelement as IElement);

                    PdfPTable pdftable = new PdfPTable(8);

                    pdftable.TotalWidth = 520f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.1f, 0.6f, 0.2f, 0.2f, 0.4f, 0.3f, 0.3f, 0.3f };
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

                    // Header: Desctiption
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("DESCRIPTION", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: Unit of Measurement
                    PdfPCell cellwithdata21 = new PdfPCell(new Phrase("UOM", GridHeadingFont));
                    cellwithdata21.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata21);

                    // Header: Rate
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("RATE", GridHeadingFont));
                    cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata2);

                    // Header: Currency

                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("CURRENCY", GridHeadingFont));
                    cellwithdata3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata3);

                    // Header: Amount
                    PdfPCell cellwithdata4 = new PdfPCell(new Phrase("AMOUNT (Rs)", GridHeadingFont));
                    cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata4);

                    /*************************************
                    // Header: Service Tax

                    PdfPCell cellwithdata51 = new PdfPCell(new Phrase("SERVICE TAX", GridHeadingFont));
                    cellwithdata51.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata51);

                    // Header: SBC - Swatchh Bharat Cess

                    PdfPCell cellwithdata52 = new PdfPCell(new Phrase("SBC", GridHeadingFont));
                    cellwithdata52.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata52);
                    *************************************/

                    // Header: Tax

                    PdfPCell cellwithdata5 = new PdfPCell(new Phrase("TAX (Rs)", GridHeadingFont));
                    cellwithdata5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata5);

                    // Header: Total Amount
                    PdfPCell cellwithdata6 = new PdfPCell(new Phrase("TOTAL (Rs)", GridHeadingFont));
                    cellwithdata6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdftable.AddCell(cellwithdata6);

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.UseVariableBorders = false;

                    // Data Cell: Description Of Charges

                    PdfPCell CellDescription = new PdfPCell();
                    CellDescription.Colspan = 1;
                    CellDescription.UseVariableBorders = false;

                    // Data Cell: Unit of Measurement

                    PdfPCell CellUOM = new PdfPCell();
                    CellUOM.Colspan = 1;
                    CellUOM.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellUOM.UseVariableBorders = false;

                    // Data Cell: Rate

                    PdfPCell CellRate = new PdfPCell();
                    CellRate.Colspan = 1;
                    CellRate.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellRate.UseVariableBorders = false;

                    // Data Cell: Currency
                    PdfPCell CellCurrency = new PdfPCell();
                    CellCurrency.Colspan = 1;
                    CellCurrency.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellCurrency.UseVariableBorders = false;

                    /*************************************************************
                    // Data Cell: Service Tax
                    PdfPCell CellServiceTax = new PdfPCell();
                    CellServiceTax.Colspan  = 1;
                    CellServiceTax.HorizontalAlignment  =   Element.ALIGN_RIGHT;
                    CellServiceTax.UseVariableBorders   =   false;

                    // Data Cell: SBC Tax (Swatchh Bharat CESS)
                    PdfPCell CellSBC    = new PdfPCell();
                    CellSBC.Colspan     = 1;
                    CellSBC.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellSBC.UseVariableBorders  = false;
                    *************************************************************/

                    // Data Cell: Tax
                    PdfPCell CellTax = new PdfPCell();
                    CellTax.Colspan = 1;
                    CellTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellTax.UseVariableBorders = false;

                    //  Data Cell: Amount

                    PdfPCell CellAmount = new PdfPCell();
                    CellAmount.Colspan = 1;
                    CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellAmount.UseVariableBorders = false;

                    // Data Cell: Total Amount

                    PdfPCell CellTotalAmount = new PdfPCell();
                    CellTotalAmount.Colspan = 1;
                    CellTotalAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellTotalAmount.UseVariableBorders = false;

                    //  Generate Table Data from CAN Invoice 

                    int rowCount = dsCanInvoice.Tables[0].Rows.Count;

                    foreach (DataRow dr in dsCanInvoice.Tables[0].Rows)
                    {
                        i = i + 1;
                        // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                        // Add Cell Data To Table

                        // Serial number #
                        if (rowCount == i) // last row blank
                        {
                            SrnoCell.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                        }

                        pdftable.AddCell(SrnoCell);

                        // Field Description - Report Header
                        if (rowCount == i) // last row font Bold
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextBoldformat);
                            // CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextBoldformat);
                        }
                        else
                        {
                            CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["ReportHeader"]), TextFontformat);
                            //CellDescription.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["FieldName"]), TextFontformat);
                        }

                        pdftable.AddCell(CellDescription);

                        // CellUOM

                        CellUOM.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["UnitOfMeasurement"]), TextFontformat);

                        pdftable.AddCell(CellUOM);

                        // CellRate

                        if (rowCount == i) // last row blank
                        {
                            CellRate.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            CellRate.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Rate"]), TextFontformat);
                        }

                        pdftable.AddCell(CellRate);

                        // CellCurrency
                        if (rowCount == i) // last row blank
                        {
                            CellCurrency.Phrase = new Phrase("", TextFontformat);
                        }
                        else
                        {
                            string strCurrencyRate = dsCanInvoice.Tables[0].Rows[i - 1]["Currency"].ToString() + " - " +
                                    dsCanInvoice.Tables[0].Rows[i - 1]["ExchangeRate"].ToString();

                            CellCurrency.Phrase = new Phrase(Convert.ToString(strCurrencyRate), TextFontformat);
                        }

                        pdftable.AddCell(CellCurrency);

                        // CellAmount

                        if (rowCount == i) // last row font Bold
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextBoldformat);
                        }
                        else
                        {
                            CellAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["Amount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellAmount);

                        // CellTax // CellServiceTax // CellSBC
                        if (rowCount == i) // last row font Bold
                        {
                            //CellServiceTax.Phrase = new Phrase("", TextFontformat);
                            //CellSBC.Phrase = new Phrase("", TextFontformat);

                            CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextBoldformat);
                        }
                        else
                        {
                            //CellServiceTax.Phrase = new Phrase(Convert.ToString(strServiceTax), TextFontformat);
                            //CellSBC.Phrase = new Phrase(Convert.ToString(strSBCTax), TextFontformat);
                            CellTax.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TaxAmount"]), TextFontformat);
                        }

                        //pdftable.AddCell(CellServiceTax);
                        //pdftable.AddCell(CellSBC);

                        pdftable.AddCell(CellTax);

                        // CellTotalAmount
                        if (rowCount == i) // last row font Bold
                        {
                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextBoldformat);
                        }
                        else
                        {
                            CellTotalAmount.Phrase = new Phrase(Convert.ToString(dsCanInvoice.Tables[0].Rows[i - 1]["TotalAmount"]), TextFontformat);
                        }

                        pdftable.AddCell(CellTotalAmount);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 5;//10

                    pdfDoc.Add(new Paragraph("For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + CanUserName, TextFontformat));

                    string footerText1 = "1. Shipment consigned to the bank, BRO (Bank Release Order) is Mandatory.\n" +
                        "2. Your cargo has not been checked while issuing this notice.\n" +
                        "3. Please produce a letter of authority at the time of collection of your documents & D.O.S.\n" +
                        "4. Please don't pay duty without checking on complete arrival of goods as per document.\n" +
                        "5. This transaction is covered under jurisdiction of the arrival port state.\n";

                    string footerText2 = "E. & O.E.\n";

                    string footerText3 = "Please contact our office between 1000/1630 hours on all working days, except second saturdays &" +
                        " customs holidays, For charges collect shipments, the Delivery Order will be issued against the amount indicated above" +
                        " on the receipt of the D.I.C. and Delivery Order will be only issued after the cargo is forwarded to the customs warehouse and the agent is not liable for any claim" +
                        " on warehouse charges as a result of delay on part of air carriers to check the freight and issue D.O.\n" +
                        " Demurrage will be charges after free storage period as per rates published by the M.I.A.L Till customs clearance is effected. please note your cargo" +
                        " will be shifted to the M.I.A.L warehouse after 14 days of warehousing. Also note that if the said consignment is not cleared on production of" +
                        " proper documents within 30 days from the date of arrival of the consignment. it is liable to be disposed of under provision of section 38 & 150 of the custom's Act, 1962.";

                    string footerText4 = "PAN No:   AAACB0466A  SERVICE TAX NO: AAACB0466AST001, SERVICE TAX CATEGORY - BUSINESS   AUXILIARY SERVICES";
                    //  string footerText5 = "SERVICE TAX CATEGORY - BUSINESS   AUXILIARY SERVICES";
                    string footerText6 = "BABAJI SHIVRAM CLEARING & CARRIERS PVT. LTD." +
                            "PLOT NO.2 CTS No. 5/7, SAKI VIHAR ROAD, SAKINAKA, ANDHERI EAST, MUMBAI 400072.";

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText1, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText2, TextFontformat));
                    pdfDoc.Add(new Paragraph(footerText3, FooterFontformat));
                    pdfDoc.Add(new Paragraph(footerText4, FooterFontformat));
                    //  pdfDoc.Add(new Paragraph(footerText5, TextFontformat));
                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText6, FooterFontformat));

                    // Footer Image Commented
                    // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));
                    // footer.SetAbsolutePosition(30, 0);
                    // pdfDoc.Add(footer);
                    // pdfwriter.PageEvent = new PDFFooter();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }//END_IF

                else
                {
                    lblError.Text = "Please Enter CAN Invoice Details!";
                    lblError.CssClass = "errorMsg";
                }

            }//END_Try

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblError.Text = "CAN Details Not Found";
            lblError.CssClass = "errorMsg";
        }
    }
    
    #endregion

    #region DO PDF

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
            lblError.Text = "Job Details Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    private void GeneratePdfAIR(int EnqId, DataSet dsDOAirPrint)
    {
        string strHOAddress1 = "Babaji Shivram Clearing and Carriers Pvt. Ltd.";
        string strHOAddress2 = "Plot no. 2, Behind Excom House";
        string strHOAddress3 = "Saki Vihar Road, Saki Naka";
        string strHOAddress4 = "Mumbai - 400072, INDIA.";

        string FRJobNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["FRJobNo"]);
        string Consignee = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["Consignee"]);
        string ConsigneeAddress = Consignee + ", " + Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ConsigneeAddress"]);

        string VesselNumber = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["VesselNumber"]);
        string IGMNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["IGMNo"]);
        string NoOfPkgs = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["NoOfPackages"]);
        string GrossWeight = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["GrossWeight"]);
        string ChargeableWeight = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["ChargeableWeight"]);
        string CargoDescription = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["CargoDescription"]);
        string MBLNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["MBLNo"]);
        string HBLNo = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["HBLNo"]);
        string PortCity = Convert.ToString(dsDOAirPrint.Tables[0].Rows[0]["PortCity"]);

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
        string FRJobNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FRJobNo"]).Trim();
        string Customer = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["Customer"]).Trim();
        string FinalAgent = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["FinalAgent"]).Trim();
        string VesselName = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["VesselName"]).Trim();
        string IGMNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["IGMNo"]).Trim();
        string MBLNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["MBLNo"]).Trim();
        string HBLNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["HBLNo"]).Trim();
        string ItemNo = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["ItemNo"]).Trim();
        string ChaName = Convert.ToString(dsDOSeaPrint.Tables[0].Rows[0]["CHAName"]).Trim();

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

            contents = contents.Replace("[FinalAgent]", FinalAgent);
            contents = contents.Replace("[VesselName]", VesselName);

            contents = contents.Replace("[MAWBL]", MBLNo);
            contents = contents.Replace("[HAWBL]", HBLNo);

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

    #endregion

    #region Freight Document

    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "removedocument")
        {
            int DocId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightDocument(DocId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Document deleted successfully!";
                lblError.CssClass = "success";

                gvFreightDocument.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Document details not found";
                lblError.CssClass = "errorMsg";

                gvFreightDocument.DataBind();
            }
        }
    }

    protected void btnUpload_Click(Object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int DocResult = -123;

        //if (txtDocName.Text.Trim() == "")
        //{
        //    lblError.Text = "Please enter the document name.";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

        if (EnqId > 0) // If EnquiryId Session Not Expired.Update Status Details
        {
            if (fuDocument.HasFile) // Add Enquiry Document
            {
                string strDocFolder = "FreightDoc\\" + hdnUploadPath.Value + "\\";

                string strFilePath = UploadDocument(strDocFolder);
                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddFreightDocument(EnqId, ddl_DocumentType.SelectedItem.ToString(), strFilePath, LoggedInUser.glUserId);

                    if (DocResult == 0)
                    {
                        lblError.Text = "Document Uploaded Auccessfully.";
                        lblError.CssClass = "success";
                        gvFreightDocument.DataBind();
                    }
                    else if (DocResult == 1)
                    {
                        lblError.Text = "System Error! Please Try After Sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (DocResult == 2)
                    {
                        lblError.Text = "Document Name Already Exists! Please change the document name!.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }//END_IF_FileExists
            else
            {
                lblError.Text = "Please attach the document for upload!";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF_Enquiry
        else
        {
            Response.Redirect("FreightAwarded.aspx");
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

    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

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
        }

        return FilePath + FileName;
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

    #region Daily Activity

    protected void btnSaveActivity_Click(object semder, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int result = DBOperations.AddFreightActivity(EnqId, txtDailyProgress.Text.Trim(), "", 0, true, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Daily activity added successfully!";
            lblError.CssClass = "success";
            txtDailyProgress.Text = "";
            gvDailyActivity.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Daily activity addedd successfully!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvDailyActivity_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvDailyActivity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "DailyProgress").ToString();
                LinkButton lnkMoreProgress = (LinkButton)e.Row.FindControl("lnkMoreProgress");
                // HtmlAnchor lnkDataTooltip = (HtmlAnchor)e.Row.FindControl("lnkDataTooltip");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }

                if (strProgressText.Length > 30)
                {
                    lnkMoreProgress.ToolTip = strProgressText;
                    // lnkDataTooltip.Attributes.Add("data-tooltip", strProgressText);

                    //NameHyperLink.Attributes.Add("onmouseover", "ShowToolTip(event, " +
                    //"'" + Server.HtmlEncode(strProgressText) + "','Right');");

                    //NameHyperLink.Attributes.Add("onmouseout", "HideTooTip(event);");
                    //NameHyperLink.Attributes.Add("onmousemove", "MoveToolTip(event,'Right');");
                }
                else
                {
                    lnkMoreProgress.Visible = false;
                }
            }
        }
    }

    protected void gvDailyActivity_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "progresspopup")
        {
            // ModalPopupProgress.Show();
            // lblPopProgress.Text = e.CommandArgument.ToString();
        }
        else if (e.CommandName.ToLower() == "activitydelete")
        {
            int ActivityId = Convert.ToInt32(e.CommandArgument.ToString());

            int result = DBOperations.DeleteFreightActivity(ActivityId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Acitivity Removed Successfully!";
                lblError.CssClass = "success";
                gvDailyActivity.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Current Activity!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    #endregion

    #region Reminder

    protected void gvReminder_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "removeremind")
        {
            int ReminderId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightReminder(ReminderId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Reminder deleted successfully!";
                lblError.CssClass = "success";

                gvReminder.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Reminder. Status Is Closed.";
                lblError.CssClass = "errorMsg";

                gvReminder.DataBind();
            }
        }
    }

    protected void btnAddReminder_Click(object sender, EventArgs e)
    {
        int EnqId = 0;
        int RemindResult = -123;
        DateTime dtRemindDate = DateTime.MinValue;

        EnqId = Convert.ToInt32(Session["EnqId"]);

        if (EnqId > 0)
        {
            if (chkRemindMode.SelectedIndex == -1)
            {
                lblError.Text = "Please Select Reminder Type Email Or SMS!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (txtRemindDate.Text.Trim() != "")
            {
                int intDateCompare = DateTime.Compare(Convert.ToDateTime(txtRemindDate.Text.Trim()), DateTime.Today);

                if (intDateCompare < 0)
                {
                    lblError.Text = "Reminder date is earlier than today! Please change to future Date.";
                    lblError.CssClass = "errorMsg";
                    return;
                }
                else
                {
                    dtRemindDate = Commonfunctions.CDateTime(txtRemindDate.Text.Trim());
                }
            }

            if (chkRemindMode.Items[0].Selected) // Email
                RemindResult = DBOperations.AddFreightReminder(EnqId, 1, LoggedInUser.glUserId, dtRemindDate, txtRemindNote.Text.Trim(), LoggedInUser.glUserId);

            if (chkRemindMode.Items[1].Selected) // SMS
                RemindResult = DBOperations.AddFreightReminder(EnqId, 2, LoggedInUser.glUserId, dtRemindDate, txtRemindNote.Text.Trim(), LoggedInUser.glUserId);

            if (RemindResult == 0)
            {
                lblError.Text = "Reminder Added Successfully.";
                lblError.CssClass = "success";

                txtRemindDate.Text = "";
                txtRemindNote.Text = "";
                chkRemindMode.SelectedIndex = -1;

                gvReminder.DataBind();
            }
            else if (RemindResult == 1)
            {
                lblError.Text = "System Error. Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (RemindResult == 2)
            {
                lblError.Text = "Reminder Detail Already Added!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            Response.Redirect("FreightAwarded.aspx");
        }
    }

    #endregion

    #region Agent
    protected void lbAgentCompany_IndexChanged(object sender, EventArgs e)
    {
        string strAgentIDList = "";

        int CountryID = Convert.ToInt32(hdnCountryId.Value);

        foreach (System.Web.UI.WebControls.ListItem item in lbAgentCompany.Items)
        {
            if (item.Selected)
            {
                strAgentIDList += item.Value + ",";
            }
        }

        if (strAgentIDList != "")
        {
            // Get All Contact For Agent
            FillAgentDetailByCompany(strAgentIDList, 0);
        }
    }
    private void FillEnquiryAgent(int EnqID)
    {
        DataSet dsEnquiryAgent = DBOperations.GetFreightAgent(EnqID);

        lbEnquiryAgent.DataSource = dsEnquiryAgent;
        lbEnquiryAgent.DataTextField = "DisplayName";
        lbEnquiryAgent.DataValueField = "AgentID";
        lbEnquiryAgent.DataBind();
    }
    private void FillAgentDetail(int CountryID)
    {
        Int32 CompanyCategoryID = (Int32)EnumCompanyType.Agent;

        // Fill Agent Company by Country
        // Fill All Agent Contact  - CountryID = 0
        DBOperations.FillCompanyCategoryByCountryID(lbAgentCompany, CompanyCategoryID, 0);

        DBOperations.FillCompanyUserByCountryID(lbAgentContact, CompanyCategoryID, CountryID);
    }
    private void FillAgentDetailByCompany(string CompanyIDList, int CountryID)
    {
        Int32 CompanyCategoryID = (Int32)EnumCompanyType.Agent;

        // Fill Agent Company by Country

        DBOperations.FillCompanyUserByListID(lbAgentContact,CompanyIDList, CompanyCategoryID, CountryID);
    }
    protected void btnSendAgentEmail_Click(object sender, EventArgs e)
    {
        if (Session["EnqID"] != null)
        {
            if (lbAgentContact.SelectedIndex != -1)
            {
                int EnqID = Convert.ToInt32(Session["EnqID"]);

                DataSet dsEnquiry = DBOperations.GetFreightDetail(EnqID);

                int AgentID = 0;
                string strAgentName = "", strAgentEmail = "";
                string EmailContent = "", strReturnMessage = "";
                string EmailBody = "", MessageBody = "", strSubject = "";

                string strEnqRefNo = dsEnquiry.Tables[0].Rows[0]["ENQRefNo"].ToString();
                string strFreightType = dsEnquiry.Tables[0].Rows[0]["TypeName"].ToString();
                string strFreightMode = dsEnquiry.Tables[0].Rows[0]["ModeName"].ToString();
                string strCustRefNo = dsEnquiry.Tables[0].Rows[0]["CustRefNo"].ToString();
                string strPortOfLoading = dsEnquiry.Tables[0].Rows[0]["LoadingPortName"].ToString();
                string strPortOfDischarge = dsEnquiry.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                string strTerms = dsEnquiry.Tables[0].Rows[0]["TermsName"].ToString();
                string strNoOfPackages = dsEnquiry.Tables[0].Rows[0]["NoOfPackages"].ToString();
                string strGrossWeight = dsEnquiry.Tables[0].Rows[0]["GrossWeight"].ToString();
                string strRemark = dsEnquiry.Tables[0].Rows[0]["Remarks"].ToString();

                strReturnMessage = "Enquiry Email For Agent Sent To:";

                try
                {
                    string strFileName = "../EmailTemplate/Freight_EmailAgent.txt";

                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message;
                    lblError.Text = strReturnMessage;
                    lblError.CssClass = "errorMsg";
                }

                // Email Template

                MessageBody = EmailContent.Replace("@EnqRefNo", strEnqRefNo);

                MessageBody = MessageBody.Replace("@FreightType", strFreightType);
                MessageBody = MessageBody.Replace("@FreightMode", strFreightMode);
                MessageBody = MessageBody.Replace("@CustRefNo", strCustRefNo);
                MessageBody = MessageBody.Replace("@PortOfLoading", strPortOfLoading);
                MessageBody = MessageBody.Replace("@PortOfDischarge", strPortOfDischarge);
                MessageBody = MessageBody.Replace("@Terms", strTerms);
                MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPackages);
                MessageBody = MessageBody.Replace("@GrossWeight", strGrossWeight);
                MessageBody = MessageBody.Replace("@Remark", strRemark);
                MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);

                foreach (System.Web.UI.WebControls.ListItem item in lbAgentContact.Items)
                {
                    if (item.Selected)
                    {
                        AgentID = Convert.ToInt32(item.Value);

                        DataView dvEmpDetail = DBOperations.GetCustomerUserDetail(AgentID.ToString());

                        if (dvEmpDetail.Table.Rows.Count > 0)
                        {
                            strAgentName = dvEmpDetail.Table.Rows[0]["sName"].ToString();
                            strAgentEmail = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();

                            try
                            {
                                strSubject = "ENQ // " + strEnqRefNo + "// " + strPortOfLoading + "// TO // " + strPortOfDischarge + "//" +
                                strFreightMode + "// " + strCustRefNo;

                                MessageBody = MessageBody.Replace("@AgentName", strAgentName);

                                EmailBody = MessageBody;
                                //strAgentEmail = "amit.bakshi@babajishivram.com";

                                // Send Email To Agent and Copy To Freight SPC -- "amit.bakshi@babajishivram.com"
                                bool bMailSuccess = EMail.SendMail(LoggedInUser.glUserName, LoggedInUser.glUserName, strSubject, EmailBody, "");

                                int result = DBOperations.AddFreightAgent(EnqID, AgentID, bMailSuccess, LoggedInUser.glUserId);

                                if (bMailSuccess == true)
                                {
                                    strReturnMessage += ", " + strAgentName;
                                }
                                else
                                {
                                    strReturnMessage += "<br> Enquiry Sending Failed For - " + strAgentName;
                                }

                            }
                            catch (System.Exception ex)
                            {
                                strReturnMessage += ex.Message.ToString();
                                //return strReturnMessage;
                            }
                        }//END_IF_UserDetail

                    }// END_IF_Selected

                }//END_ForEach

                lblError.Text = strReturnMessage;
                lblError.CssClass = "errorMsg";
                FillEnquiryAgent(EnqID);

            }// END_IF_Selected != -1
            else
            {
                lblError.Text = "Please Select Agent Contact To Send Email";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup('" + lblError.Text + "');", true);
            }
        }//END_IF

    }

    #endregion

    #region Freight Status
    protected void btnStatusChange_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvFreightStatus.ChangeMode(FormViewMode.Edit);

            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
                        
            DropDownList ddFreightStatus = (DropDownList)fvFreightStatus.FindControl("ddFreightStatus");
            TextBox txtStatusDate = (TextBox)fvFreightStatus.FindControl("txtStatusDate");

            if (ddFreightStatus != null)
            {
                int StatusId = 0;

                if (hdnStatusId.Value != "")
                {
                    StatusId = Convert.ToInt32(hdnStatusId.Value);

                    if(StatusId > 15)
                    {
                        lblError.Text = "Billing Advice Completed! Please Contact Administrator for Status Change!";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        if (txtStatusDate != null)
                        {
                            txtStatusDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                        }
                        
                    }
                }
            }
        }
    }
    
    protected void btnStatusUpdate_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int result = -123;

        if (EnqId > 0) // If EnquiryId Session Not Expired. Update Status Details
        {
            DateTime dtStatusDate = DateTime.Today;
           
            DropDownList ddFreightStatus = (DropDownList)fvFreightStatus.FindControl("ddFreightStatus");
            TextBox txtStatusDate = (TextBox)fvFreightStatus.FindControl("txtStatusDate");
            TextBox txtStatusRemark = (TextBox)fvFreightStatus.FindControl("txtStatusRemark");

            int StatusId = Convert.ToInt32(ddFreightStatus.SelectedValue);

            int LostStatusID = 0;

            if (StatusId == 4)// LOST
            {
                DropDownList ddLostStaus = (DropDownList)fvFreightStatus.FindControl("ddLostStaus");

                LostStatusID = Convert.ToInt32(ddLostStaus.SelectedValue);
            }

            if (txtStatusDate.Text.Trim() != "")
            {
                dtStatusDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());
            }

            if (StatusId <= 0)
            {
                lblError.Text = "Please Select Current Freight Status!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                // For Awarded Enquiry Check if Quotation copy uploaded

                result = DBOperations.UpdateFreightStatus(EnqId, StatusId, dtStatusDate, 0,0, txtStatusRemark.Text.Trim(), LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Status Changed Successfully!";
                    lblError.CssClass = "success";

                    gvStatusHistory.DataBind();
                    fvFreightStatus.ChangeMode(FormViewMode.ReadOnly);
                    GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Status Already Updated!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 4)
                {
                    lblError.Text = "Please Upload Quotation Copy For Quoted Shipment";
                    lblError.CssClass = "errorMsg";

                    //txtDocName.Text = "Quotation Copy";
                }
                else
                {
                    lblError.Text = "System Error! Please try after sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }// END_IF
        else
        {
            Response.Redirect("FreightAwarded.aspx");
        }
    }

    protected void btnStatusCancel_Click(object semder, EventArgs e)
    {
        fvFreightStatus.ChangeMode(FormViewMode.ReadOnly);
        GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
    }

    
    #endregion
    protected void btnUpdParticipant_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        string strProjectEmpList = "";

        if (lbEmployee.SelectedIndex != -1)
        {
            List<string> UserIdList = new List<string>();

            foreach (System.Web.UI.WebControls.ListItem emp in lbEmployee.Items)
            {
                if (emp.Selected)
                {
                    if (emp.Value != LoggedInUser.glUserId.ToString())
                    {
                        strProjectEmpList += emp.Value + ",";

                        UserIdList.Add(emp.Value);
                    }
                }
            }

            if (strProjectEmpList != "")
            {
                int resultProject = DBOperations.AddEnquiryUser(EnqId, strProjectEmpList, LoggedInUser.glUserId);

                if (resultProject == 0 && UserIdList.Count > 0)
                {
                    string strMessage = ParticipantsEmail(UserIdList);

                    lblError.Text = "Enquired Shared With Participant! " + strMessage;
                    lblError.CssClass = "success";
                }
            }
        }
    }

    protected string ParticipantsEmail(List<string> items)
    {
        string strEmpName = "", strEmpEmail = "", strRefNo = "";
        string EmailContent = "", strReturnMessage = "";
        string EmailBody = "", MessageBody = "", strSubject = "";
        string strCustomer = "";

        strRefNo = ((Label)FVFreightDetail.FindControl("lblEnquiryRefNo")).Text;

        if (FVFreightDetail.FindControl("lblCustomer") != null)
        {
            strCustomer = ((Label)FVFreightDetail.FindControl("lblCustomer")).Text.ToUpper().Trim();
        }
        else if (FVFreightDetail.FindControl("txtCustomer") != null)
        {
            strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.ToUpper().Trim();
        }

        strReturnMessage = " Notification Email Sent To:";
        try
        {
            string strFileName = "../EmailTemplate/Freight_EmailEnqShared.txt";

            StreamReader sr = new StreamReader(Server.MapPath(strFileName));
            sr = File.OpenText(Server.MapPath(strFileName));
            EmailContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            strReturnMessage = ex.Message;
        }

        foreach (string UserId in items)
        {
            DataView dvEmpDetail = DBOperations.GetUserDetail(UserId);

            if (dvEmpDetail.Table.Rows.Count > 0)
            {
                strEmpEmail = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();
                strEmpName = dvEmpDetail.Table.Rows[0]["sName"].ToString();

                MessageBody = EmailContent.Replace("<EnqRefNo>", strRefNo);

                MessageBody = MessageBody.Replace("<EmpName>", LoggedInUser.glEmpName);

                try
                {
                    strSubject = "Freight Enquiry Shared # " + strRefNo + " / Customer Name: " + strCustomer;

                    EmailBody = MessageBody;

                    EMail.SendMail(LoggedInUser.glUserName, strEmpEmail, strSubject, EmailBody, "");

                    strReturnMessage += "," + strEmpName;

                }
                catch (System.Exception ex)
                {
                    strReturnMessage = ex.Message.ToString();
                    return strReturnMessage;
                }
            }
        }//END_FOrEach

        return strReturnMessage;
    }
}