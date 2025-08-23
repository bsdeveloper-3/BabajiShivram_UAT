using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Configuration;


public partial class Reports_LetterGenerate : System.Web.UI.Page
{
    DataTable dtAnnexure = new DataTable();
    private static String[] units = { "Zero", "One", "Two", "Three",
    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
    "Seventeen", "Eighteen", "Nineteen" };
    private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
    protected void Page_Load(object sender, EventArgs e)
    {
        dtAnnexure = DBOperations.GetAnnexureDetail(txtJobId.Text, 6, Convert.ToInt32(ddlBEType.SelectedValue));
        ScriptManager1.RegisterPostBackControl(btnExport);
        dvRequestLetter.Visible = false;
        GetAnnexureDetail();
        gvTrippleDuty.DataSource = dtAnnexure;
        gvTrippleDuty.DataBind();
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        if(dtAnnexure.Rows.Count>0)
        {
            if (ddlLetterType.SelectedValue == "1")
            {
                GetAnnexurePDFFormat();
            }
            else if (ddlLetterType.SelectedValue == "2")
            {
                GetConsignmentPDFFormat();
            }
            else if (ddlLetterType.SelectedValue == "3")
            {
                GetSalePurchaseLetterFormat();
            }
        }
        else
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //GetAnnexureDetail();
        if(dtAnnexure.Rows.Count>0)
        {
            if (ddlLetterType.SelectedValue == "1")
            {
                dvAnnexure.Visible = true;
                dvTripleDutyBond.Visible = false;
                dvSalePurchaseLetter.Visible = false;
            }
            else if (ddlLetterType.SelectedValue == "2")
            {
                dvTripleDutyBond.Visible = true;
                dvAnnexure.Visible = false;
                dvSalePurchaseLetter.Visible = false;
            }
            else if (ddlLetterType.SelectedValue == "3")
            {
                dvSalePurchaseLetter.Visible = true;
                dvAnnexure.Visible = false;
                dvTripleDutyBond.Visible = false;
            }
        }
        else
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void GetAnnexureDetail()
    {
        //DataTable dtAnnexure = DBOperations.GetAnnexureDetail(txtJobId.Text, 6);
        gvAnnexure.DataSource = dtAnnexure;
        gvAnnexure.DataBind();

        foreach (DataRow row in dtAnnexure.Rows)
        {
            //Letter Generate
            lblbndNum.Text = row["BondNum"].ToString();
            lblToWh.Text = row["WAREHOUSE_NAME"].ToString();
            lbldtofbondReg.Text = row["BondReg"].ToString();
            lblNameAdd.Text = row["Port"].ToString();
            lblFromWH.Text = row["WAREHOUSE_NAME"].ToString();
            lblFmNameAdd.Text = row["Port"].ToString();
            lblSS.Text = row["CFSName"].ToString();
            lblPresentOwner.Text = row["Customer"].ToString();
            lblMS.Text = row["InBondCustName"].ToString();
            lblPort.Text = row["Port"].ToString();
            lblRegdOff.Text = row["InBondAddress"].ToString();
            //lblIGMDetail.Text = row[""].ToString();
            lblIGMNo.Text = row["IGMNo"].ToString();
            lblDt.Text = row["IGMDate"].ToString();
            lbllineNo145.Text = row["BOENo"].ToString();
            lblBOEDt.Text = row["BOEDate"].ToString();
            lblInBondCust.Text = row["InBondCustName"].ToString();
            lblConsigneeNm.Text = row["Customer"].ToString();

            //Consignment Bond
            lblConName.Text = row["Customer"].ToString();
            lblConsigneeAdd.Text = row["EXCustADD"].ToString();
            lblIECNo.Text = row["IECBranchCode"].ToString();
            lblBOEDetail.Text = "Bill Of Entry No. " + row["BOENo"].ToString() + " Dated : " + row["BOEDate"].ToString();
            lblTotalDuty.Text = row["ItemTotalDuty"].ToString() + " (" + ConvertAmount(Convert.ToDouble(row["ItemTotalDuty"].ToString())) + ")";

            //sale Purchase Letter
            lblSub1.Text = row["WEIGHT"].ToString() + "/" + row["Unit"].ToString() + " " + row["DESCRIPTION"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arrived per:" + row["CFSName"].ToString() + "  Under B/L No. " + row["MAWBNo"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; dated " + row["MAWBDate"].ToString();
            lblRef1.Text = "Bond No. " + row["BondNum"].ToString() + " Dated" + row["BondReg"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bond B/E No. " + row["BOENo"].ToString() + "dated " + row["BOEDate"].ToString();
            lblSub2.Text = row["WEIGHT"].ToString() + "/" + row["Unit"].ToString() + " " + row["DESCRIPTION"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arrived per:" + row["CFSName"].ToString() + "  Under B/L No. " + row["MAWBNo"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; dated " + row["MAWBDate"].ToString();
            lblRef2.Text = "Bond No. " + row["BondNum"].ToString() + " Dated" + row["BondReg"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bond B/E No. " + row["BOENo"].ToString() + "dated " + row["BOEDate"].ToString();
            lblSub3.Text = row["WEIGHT"].ToString() + "/" + row["Unit"].ToString() + " " + row["DESCRIPTION"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arrived per:" + row["CFSName"].ToString() + "  Under B/L No. " + row["MAWBNo"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; dated " + row["MAWBDate"].ToString();
            lblRef3.Text = "Bond No. " + row["BondNum"].ToString() + " Dated" + row["BondReg"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bond B/E No. " + row["BOENo"].ToString() + "dated " + row["BOEDate"].ToString();
            lblSub4.Text = row["WEIGHT"].ToString() + "/" + row["Unit"].ToString() + " " + row["DESCRIPTION"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arrived per:" + row["CFSName"].ToString() + "  Under B/L No. " + row["MAWBNo"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; dated " + row["MAWBDate"].ToString();
            lblRef4.Text = "Bond No. " + row["BondNum"].ToString() + " Dated" + row["BondReg"].ToString() + "<br/>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bond B/E No. " + row["BOENo"].ToString() + "dated " + row["BOEDate"].ToString();

            lblExCustNm.Text = row["Customer"].ToString() + " ," + row["EXCustADD"].ToString();
            lblEXCustNm1.Text = row["Customer"].ToString();
            lblInbondDetail.Text = row["InBondCustName"].ToString() + " ," + row["InBondAddress"].ToString();
            lblDetail.Text = row["Customer"].ToString() + "<br/>" + row["EXCustADD"].ToString();
            lblInDetail1.Text = row["InBondCustName"].ToString() + " ," + row["InBondAddress"].ToString();

            lblYoursFaithfully1.Text = row["InBondCustName"].ToString();
            lblYoursFaithfully2.Text = row["Customer"].ToString();
            lblYoursFaithfully3.Text = row["InBondCustName"].ToString();
            lblYoursFaithfully4.Text = row["Customer"].ToString();

            lblTodayDt1.Text = DateTime.Now.ToString("dd.MM.yyyy");
            lblTodayDt2.Text = DateTime.Now.ToString("dd.MM.yyyy");
            lblTodayDt3.Text = DateTime.Now.ToString("dd.MM.yyyy");
            lblTodayDt4.Text = DateTime.Now.ToString("dd.MM.yyyy");
            break;
        }

    }

    protected void GetAnnexurePDFFormat()
    {
        //DataTable dtAnnexure = DBOperations.GetAnnexureDetail(txtJobId.Text, 6);

        int NoOfColumn = 0, i = 0;
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Annexure-" + txtJobId.Text + ".pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        sw.Write("<br/>");
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        StringReader sr = new StringReader(sw.ToString());

        iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
        Document pdfDoc = new Document(recPDF);

        //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();
        //pdfDoc.SetPageSize(new Rectangle(850f, 1100f));
        Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
        Font TextFontformat = FontFactory.GetFont("Arial", 12, Font.NORMAL);
        Font TextBoldformat = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.RED);
        Font ColorMsg = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED);
        Font HeaderFontformat = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font SubHeadFormat = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLUE);

        string contents = "";
        contents = File.ReadAllText(Server.MapPath("LetterGenerate.htm"));
        contents = contents.Replace("[lblbndNum]", lblbndNum.Text);
        contents = contents.Replace("[lblToWh]", lblToWh.Text);
        contents = contents.Replace("[lbldtofbondReg]", lbldtofbondReg.Text);
        contents = contents.Replace("[lblNameAdd]", lblNameAdd.Text);
        contents = contents.Replace("[lblFromWH]", lblFromWH.Text);
        contents = contents.Replace("[lblFmNameAdd]", lblFmNameAdd.Text);
        contents = contents.Replace("[lblSS]", lblSS.Text);
        contents = contents.Replace("[lblPresentOwner]", lblPresentOwner.Text);
        contents = contents.Replace("[lblMS]", lblMS.Text);
        contents = contents.Replace("[lblDtOfTransferGranted]", lblDtOfTransferGranted.Text);
        contents = contents.Replace("[lblRegdOff]", lblRegdOff.Text);
        contents = contents.Replace("[lblIGMDetail]", lblIGMDetail.Text);
        contents = contents.Replace("[lblIGMNo]", lblIGMNo.Text);
        contents = contents.Replace("[lblDt]", lblDt.Text);
        contents = contents.Replace("[lbllineNo145]", lbllineNo145.Text);
        contents = contents.Replace("[lblBOEDt]", lblBOEDt.Text);

        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
        foreach (var htmlelement in parsedContent)
            pdfDoc.Add(htmlelement as IElement);

        /***********************************  Letter Generate  *****************************************/
        NoOfColumn = 10;
        Paragraph ParaSpace = new Paragraph();
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(ParaSpace);

        PdfPTable pdftable = new PdfPTable(NoOfColumn);   //Create new table
        pdftable.TotalWidth = 550;
        pdftable.LockedWidth = true;

        //float[] widths = new float[] { 0.15f, 0.6f, 0.15f, 0.3f, 0.60f, 1.8f, 0.4f, 0.4f, 0.4f };
        float[] widths = new float[] { 0.15f, 0.3f, 0.15f, 0.15f, 0.3f, 1.0f, 0.3f, 0.3f, 0.3f,0.3f };
        pdftable.SetWidths(widths);
        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

        // Set Table Spacing Before And After html text
        // pdftable.SpacingAfter = 8f;

        // Create Table Column Header Cell with Text

        // Header:SERIAL NUMBER
        PdfPCell cellwithdata0 = new PdfPCell(new Phrase("Packages", GridHeadingFont));
        cellwithdata0.Colspan = 2;
        pdftable.AddCell(cellwithdata0);

        // Header:Unit
        PdfPCell cellwithdata = new PdfPCell(new Phrase("Item No", GridHeadingFont));
        cellwithdata.Rowspan = 2;
        cellwithdata.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata);

        // Header: SERVICE
        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Quantity", GridHeadingFont));
        cellwithdata1.Colspan = 2;
        cellwithdata1.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata1);

        // Header: ItemNo
        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Description", GridHeadingFont));
        cellwithdata2.Rowspan = 2;
        cellwithdata2.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata2);

        // Header: Weight
        PdfPCell cellwithdata3 = new PdfPCell(new Phrase("CTH", GridHeadingFont));
        cellwithdata3.Rowspan = 2;
        cellwithdata3.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata3);

        // Header: DESCRIPTION
        PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Assess. Value RS.", GridHeadingFont));
        cellwithdata4.Rowspan = 2;
        cellwithdata4.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata4);

        // Header: CTHNo
        PdfPCell cellwithdata5 = new PdfPCell(new Phrase("Rate Of Duty", GridHeadingFont));
        cellwithdata5.Rowspan = 2;
        cellwithdata5.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata5);

        // Header: AssessableValue
        PdfPCell cellwithdata6 = new PdfPCell(new Phrase("Amount Of Duty Rs.", GridHeadingFont));
        cellwithdata6.Rowspan = 2;
        cellwithdata6.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata6);

        pdftable.AddCell("No");
        pdftable.AddCell("Unit");
        pdftable.AddCell("No");
        pdftable.AddCell("Unit");




        //CELL ADDED
        //DATA CELL: SERIAL NUMBER
        PdfPCell CellSl = new PdfPCell();
        CellSl.Colspan = 1;
        CellSl.UseVariableBorders = false;

        //DATA CELL: Unit
        PdfPCell CellUnit = new PdfPCell();
        CellUnit.Colspan = 1;
        CellUnit.UseVariableBorders = false;

        // Data Cell: No
        PdfPCell CellNo = new PdfPCell();
        CellNo.Colspan = 1;
        CellNo.UseVariableBorders = false;

        // Data Cell: ItemNo
        PdfPCell CellItemNo = new PdfPCell();
        CellItemNo.Colspan = 1;
        CellItemNo.UseVariableBorders = false;

        // Data Cell:WEIGHT
        PdfPCell CellWEIGHT = new PdfPCell();
        CellWEIGHT.Colspan = 1;
        CellWEIGHT.UseVariableBorders = false;

        // Data Cell:DESCRIPTION
        PdfPCell CellDESCRIPTION = new PdfPCell();
        CellDESCRIPTION.Colspan = 1;
        CellDESCRIPTION.UseVariableBorders = false;

        // Data Cell: CTHNo
        PdfPCell CellCTHNo = new PdfPCell();
        CellCTHNo.Colspan = 1;
        CellCTHNo.UseVariableBorders = false;

        // Data Cell: AssessableValue
        PdfPCell CellAssessableValue = new PdfPCell();
        CellAssessableValue.Colspan = 1;
        CellAssessableValue.UseVariableBorders = false;

        // Data Cell: RateOfDuty
        PdfPCell CellRateOfDuty = new PdfPCell();
        CellRateOfDuty.Colspan = 1;
        CellRateOfDuty.UseVariableBorders = false;

        PdfPCell CellAmount = new PdfPCell();
        CellAmount.Colspan = 1;
        CellAmount.UseVariableBorders = false;

        if (dtAnnexure.Rows.Count > 0)
        {
            foreach (DataRow row in dtAnnexure.Rows)
            {
                i = i + 1;
                //cellSerialNUmber
                CellSl.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                pdftable.AddCell(CellSl);

                //cellUnit
                CellUnit.Phrase = new Phrase(Convert.ToString(row["Unit"]), TextFontformat);
                pdftable.AddCell(CellUnit);

                // CellNo
                CellNo.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                pdftable.AddCell(CellNo);

                // CellItemNo
                CellItemNo.Phrase = new Phrase(Convert.ToString(i), TextFontformat);//row["ItemNo"]
                pdftable.AddCell(CellItemNo);

                // CellWEIGHT
                CellWEIGHT.Phrase = new Phrase(Convert.ToString(row["WEIGHT"]), TextFontformat);
                pdftable.AddCell(CellWEIGHT);

                // CellDESCRIPTION
                CellDESCRIPTION.Phrase = new Phrase(Convert.ToString(row["DESCRIPTION"]), TextFontformat);
                pdftable.AddCell(CellDESCRIPTION);

                // CellCTHNo
                CellCTHNo.Phrase = new Phrase(Convert.ToString(row["CTHNo"]), TextFontformat);
                pdftable.AddCell(CellCTHNo);

                // CellAssessableValue
                CellAssessableValue.Phrase = new Phrase(Convert.ToString(row["AssessableValue"]), TextFontformat);
                pdftable.AddCell(CellAssessableValue);

                // CellRateOfDuty
                CellRateOfDuty.Phrase = new Phrase(Convert.ToString(row["RateOfDuty"]), TextFontformat);
                pdftable.AddCell(CellRateOfDuty);

                CellAmount.Phrase = new Phrase(Convert.ToString(row["ItemTotalDuty"]), TextFontformat);
                pdftable.AddCell(CellAmount);
            } //end of foreach
            i = 0;
            pdfDoc.Add(pdftable);

        pdfDoc.Add(new Paragraph("\n \n \n", TextFontformat));

        NoOfColumn = 3;
        Paragraph ParaSpace1 = new Paragraph();
        ParaSpace1.SpacingBefore = 5;

        pdfDoc.Add(ParaSpace1);

        PdfPTable pdftable1 = new PdfPTable(NoOfColumn);   //Create new table
        pdftable1.TotalWidth = 550f;
        pdftable1.LockedWidth = true;
        pdftable1.HorizontalAlignment = 0;

        float[] widths1 = new float[] { 0.2f, 0.3f, 0.3f };
        pdftable1.SetWidths(widths1);
        pdftable1.HorizontalAlignment = Element.ALIGN_LEFT;

        // Header:SERIAL NUMBER
        PdfPCell cellType1 = new PdfPCell(new Phrase("New Bond No.", GridHeadingFont));
        cellType1.Border = 0;
        cellType1.HorizontalAlignment = Element.ALIGN_LEFT;
        pdftable1.AddCell(cellType1);

        // Header:Unit
        PdfPCell cellType2 = new PdfPCell(new Phrase("Yours faithfully,", GridHeadingFont));
        cellType2.HorizontalAlignment = Element.ALIGN_CENTER;
        cellType2.Border = 0;
        pdftable1.AddCell(cellType2);

        // Header: SERVICE
        PdfPCell cellType3 = new PdfPCell(new Phrase("Yours faithfully,", GridHeadingFont));
        cellType3.HorizontalAlignment = Element.ALIGN_CENTER;
        cellType3.Border = 0;
        pdftable1.AddCell(cellType3);

        //DATA CELL: SERIAL NUMBER
        PdfPCell CellSTypel = new PdfPCell();
        CellSTypel.Colspan = 1;
        CellSTypel.UseVariableBorders = false;
        CellSTypel.Border = 0;

        //DATA CELL: Unit
        PdfPCell CellType2 = new PdfPCell();
        CellType2.HorizontalAlignment = Element.ALIGN_CENTER;
        CellType2.Colspan = 1;
        CellType2.UseVariableBorders = false;
        CellType2.Border = 0;

        // Data Cell: No
        PdfPCell CellType3 = new PdfPCell();
        CellType3.HorizontalAlignment = Element.ALIGN_CENTER;
        CellType3.Colspan = 1;
        CellType3.UseVariableBorders = false;
        CellType3.Border = 0;

        CellSTypel.Phrase = new Phrase("(To be filled up by Department)", TextFontformat);
        pdftable1.AddCell(CellSTypel);

        //cellUnit
        CellType2.Phrase = new Phrase(lblConsigneeNm.Text, TextFontformat);
        pdftable1.AddCell(CellType2);

        // CellNo
        CellType3.Phrase = new Phrase(lblInBondCust.Text, TextFontformat);
        pdftable1.AddCell(CellType3);



        pdfDoc.Add(pdftable1);
        }
        else
        {
            pdfDoc.Add(pdftable);
            //pdfDoc.Add(new Paragraph(lblErrMsg1.Text, ColorMsg));
        }

        /***********************************  Letter Generate  *****************************************/
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    protected void GetConsignmentPDFFormat()
    {
        string InBondCustAdd = "", InBondCust = "", IEC = "", BOEDt = "", BOENo = "", TotalDuty = "0", ExBondCustAdd = "", ExBondCust = "";
        //DataTable dtAnnexure = DBOperations.GetAnnexureDetail(txtJobId.Text, 6);
        gvAnnexure.DataSource = dtAnnexure;
        gvAnnexure.DataBind();

        foreach (DataRow row in dtAnnexure.Rows)
        {
            ExBondCust = row["Customer"].ToString();
            //bold.InnerText = ExBondCust;
            ExBondCustAdd = row["EXCustADD"].ToString();
            InBondCust = row["InBondCustName"].ToString();
            InBondCustAdd = row["InBondAddress"].ToString();
            IEC = row["IECBranchCode"].ToString();
            BOEDt = row["BOEDate"].ToString();
            BOENo = row["BOENo"].ToString();
            TotalDuty = row["ItemTotalDuty"].ToString();
            break;
        }

        int NoOfColumn = 0, i = 0;
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=TripleDuty-" + txtJobId.Text + ".pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        sw.Write("<br/>");
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        StringReader sr = new StringReader(sw.ToString());

        iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
        Document pdfDoc = new Document(recPDF);

        //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();

        Font GridHeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
        Font TextBoldformat = FontFactory.GetFont("Arial", 10, Font.BOLD);
        Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.RED);
        Font ColorMsg = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED);
        Font HeaderFontformat = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font SubHeadFormat = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLUE);

        string contents = "";
        contents = File.ReadAllText(Server.MapPath("ConsignMentBond.htm"));
        //contents = contents.Replace("[lblbndNum]", lblbndNum.Text);

        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
        foreach (var htmlelement in parsedContent)
            pdfDoc.Add(htmlelement as IElement);

        pdfDoc.Add(new Paragraph("(Bond to be executed by the importer under sub-section (1) of Section 59 of the Customs Act, 1962 for the purpose of warehousing of goods to be imported by them.)", TextFontformat));
        pdfDoc.Add(new Paragraph("                                                           [In terms of Circular No.18/2016-Customs]", TextBoldformat));


        pdfDoc.Add(new Paragraph("KNOW ALL MEN BY THESE PRESENTS THAT WE M/s. " + ExBondCust + ". having our office located at " + ExBondCustAdd + " . and  " + IEC + " , here in after referred to as the “importer”, (which expression shall include our successors, heirs, executors, administrators and legal representatives) hereby jointly and severally bind ourselves to the President of India hereinafter referred to as the “President” (which expression shall include his successors and assigns) in the sum of " + TotalDuty + " /-(RS. " + ConvertAmount(Convert.ToDouble(TotalDuty)) + " ) to be paid to the President, for which payment well and truly to be made, we bind ourselves, our successors, heirs, executors, administrators and legal representatives firmly by these presents.", TextFontformat));
        pdfDoc.Add(new Paragraph("\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Sealed with our seal(s) this 11TH day of JULY 2020.", TextBoldformat));
        pdfDoc.Add(new Paragraph("\n", TextFontformat));
        pdfDoc.Add(new Paragraph("WHEREAS, we the importers have filed a Bill Of Entry No. " + BOENo + " Dated : " + BOEDt + " for warehousing under section 46 of the Customs Act, (hereinafter referred to as the said Act) and the same has been assessed to duty under section 17 or section 18 of the said Act (strike which is not applicable) in respect of goods mentioned below.  ", TextFontformat));
        
        NoOfColumn = 6;
        Paragraph ParaSpace = new Paragraph();
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(ParaSpace);

        PdfPTable pdftable = new PdfPTable(NoOfColumn);   //Create new table
        pdftable.TotalWidth = 500f;
        pdftable.LockedWidth = true;

        float[] widths = new float[] { 0.18f, 0.3f, 0.25f, 0.25f, 0.2f, 0.3f };
        pdftable.SetWidths(widths);
        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

        // Set Table Spacing Before And After html text
        pdftable.SpacingAfter = 8f;

        // Create Table Column Header Cell with Text

        // Header:PORT OF IMPORT
        PdfPCell cellwithdata0 = new PdfPCell(new Phrase("Port Of Import ", GridHeadingFont));
        //cellwithdata0.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata0);

        // Header:DESCRIPTION OF GOODS
        PdfPCell cellwithdata = new PdfPCell(new Phrase("Description Of Goods", GridHeadingFont));
        //cellwithdata.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata);

        // Header: SI NO OF INVOICE
        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("SI No Of Invoice ", GridHeadingFont));
        //cellwithdata1.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata1);

        // Header: DESCRIPTION  AND NO OF PACKAGES
        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Description And No Of Packages", GridHeadingFont));
        //cellwithdata2.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata2);

        // Header: Values
        PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Value", GridHeadingFont));
        //cellwithdata3.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata3);

        // Header: WAREHOUSE CODE AND ADDRESS
        PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Warehouse Code And Address", GridHeadingFont));
        //cellwithdata4.HorizontalAlignment = Element.ALIGN_CENTER;
        pdftable.AddCell(cellwithdata4);

        pdftable.AddCell("1");
        pdftable.AddCell("2");
        pdftable.AddCell("3");
        pdftable.AddCell("4");
        pdftable.AddCell("5");
        pdftable.AddCell("6");

        //CELL ADDED
        //DATA CELL: CellPortOFImport
        PdfPCell CellPortOFImport = new PdfPCell();
        CellPortOFImport.Colspan = 1;
        CellPortOFImport.UseVariableBorders = false;

        //DATA CELL: CellDescOFGood
        PdfPCell CellDescOFGood = new PdfPCell();
        CellDescOFGood.Colspan = 1;
        CellDescOFGood.UseVariableBorders = false;

        // Data Cell: CellNoOfInvoice
        PdfPCell CellNoOfInvoice = new PdfPCell();
        CellNoOfInvoice.Colspan = 1;
        CellNoOfInvoice.UseVariableBorders = false;

        // Data Cell: CellDescNoOfPck
        PdfPCell CellDescNoOfPck = new PdfPCell();
        CellDescNoOfPck.Colspan = 1;
        CellDescNoOfPck.UseVariableBorders = false;

        // Data Cell:CellValue
        PdfPCell CellValue = new PdfPCell();
        CellValue.Colspan = 1;
        CellValue.UseVariableBorders = false;

        // Data Cell:CellWhCodeAndAdd
        PdfPCell CellWhCodeAndAdd = new PdfPCell();
        CellWhCodeAndAdd.Colspan = 1;
        CellWhCodeAndAdd.UseVariableBorders = false;

        //DataTable dtAnnexure = new DataTable();
        if (dtAnnexure.Rows.Count > 0)
        {
            foreach (DataRow row in dtAnnexure.Rows)
            {
                i = i + 1;
                //CellPortOFImport
                CellPortOFImport.Phrase = new Phrase(Convert.ToString(row["Port"]), TextFontformat);
                pdftable.AddCell(CellPortOFImport);

                //CellDescOFGood
                CellDescOFGood.Phrase = new Phrase(Convert.ToString(row["Description"]), TextFontformat);
                pdftable.AddCell(CellDescOFGood);

                // CellNoOfInvoice
                CellNoOfInvoice.Phrase = new Phrase(Convert.ToString(row["InvoiceNo"] + "\n" + "DT " + row["InvoiceDate"]), TextFontformat);
                pdftable.AddCell(CellNoOfInvoice);

                // CellDescNoOfPck
                CellDescNoOfPck.Phrase = new Phrase(Convert.ToString(row["Unit"] + "\n" + " GW :" + "\n" + row["GrossWT"]), TextFontformat);
                pdftable.AddCell(CellDescNoOfPck);

                // CellValue
                CellValue.Phrase = new Phrase("A.V." + "\n" + "RS" + "\n" + Convert.ToString(row["AssessableValue"]), TextFontformat);
                pdftable.AddCell(CellValue);

                // CellWhCodeAndAdd
                CellWhCodeAndAdd.Phrase = new Phrase(row["Code"] + "\n" + Convert.ToString(row["WHAdd"]), TextFontformat);
                pdftable.AddCell(CellWhCodeAndAdd);
            } //end of foreach
            i = 0;

        }
        pdfDoc.Add(pdftable);


        pdfDoc.Add(new Paragraph("\n", TextFontformat));
        pdfDoc.Add(new Paragraph("AND WHEREAS Section 59(1) of the said Act requires the execution of a bond equal to thrice the amount of duty assessed on goods for which a bill of entry for warehousing has been presented under the said section46.", TextFontformat));

        pdfDoc.Add(new Paragraph("NOW THE CONDITIONS of the above written bond is such that, if we:", TextBoldformat));

        pdfDoc.Add(new Paragraph("(1) Comply with all the provisions of the Act, the rules and regulations made thereunder in respect of such goods;", TextFontformat));
        pdfDoc.Add(new Paragraph("(2) Pay on or before the specified date in the notice of demand, all duties and interest payable under sub - section(2) of section 61; and", TextFontformat));
        pdfDoc.Add(new Paragraph("(3) Pay all penalties and fines incurred for contravention of the provisions of the Act or the rules or regulations made thereunder, in respect of such goods;", TextFontformat));
        pdfDoc.Add(new Paragraph("(4) In the event of our failure to discharge our obligation, pay the full amount of duty chargeable on account of such goods together with their interest, fine and penalties payable under section 72, in respect of such goods; Then the above written bond shall be void and of no effect otherwise the same shall remain in full force and virtue.", TextFontformat));

        pdfDoc.Add(new Paragraph("IT IS HEREBY AGREED AND DECLARED that:", TextBoldformat));
        pdfDoc.Add(new Paragraph("(1) The Bond shall continue in full force notwithstanding the transfer of the goods to another warehouse.", TextFontformat));
        pdfDoc.Add(new Paragraph("(2) The President through the Deputy / Assistant Commissioner or any other officer may recover any amount due under this Bond in the manner laid down under sub - section(1) of section 142 of the said Act without prejudice to any other mode of recovery.", TextFontformat));

        pdfDoc.Add(new Paragraph("IN THE WITNESS WHEREOF, the importer has herein, set and subscribed his hands and seals the day, month and year first above written.", TextFontformat));
        pdfDoc.Add(new Paragraph("SIGNED AND DELIVERED by or on behalf of the importer at Mumbai in the presence of:", TextFontformat));
        pdfDoc.Add(new Paragraph("Signature(s) of the importer / authorised signatory)", TextFontformat));

        pdfDoc.Add(new Paragraph("Witness:", TextBoldformat));
        pdfDoc.Add(new Paragraph("Name & Signature ", TextFontformat));
        pdfDoc.Add(new Paragraph("\n \n", TextFontformat));
        pdfDoc.Add(new Paragraph(" 1.  :  ", TextFontformat));
        pdfDoc.Add(new Paragraph("\n \n", TextFontformat));
        pdfDoc.Add(new Paragraph(" 2.  :", TextFontformat));
        pdfDoc.Add(new Paragraph("\n \n", TextFontformat));
        pdfDoc.Add(new Paragraph("Accepted by me this                                                                                     For and on behalf of the President of India.", TextFontformat));
        pdfDoc.Add(new Paragraph("                                                                                                                                    (Assistant/Deputy Commissioner)", TextFontformat));
        pdfDoc.Add(new Paragraph("                                                                                                                                    Signature and date", TextFontformat));
        pdfDoc.Add(new Paragraph("                                                                                                                                    Name:", TextFontformat));
        
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        HttpContext.Current.ApplicationInstance.CompleteRequest();

    }

    protected void GetSalePurchaseLetterFormat()
    {
        //DataTable dtAnnexure = DBOperations.GetAnnexureDetail(txtJobId.Text, 6);
        string Sub = "", Ref = "", EXConsigneeNm = "", InConsigneeNm = "", InAdd = "", EXAdd = "";

        foreach (DataRow row in dtAnnexure.Rows)
        {
            Sub = row["WEIGHT"].ToString() + "/" + row["Unit"].ToString() + " " + row["DESCRIPTION"].ToString() + "\n" + "arrived per:" + row["CFSName"].ToString() + "  Under B/L No. " + row["MAWBNo"].ToString() + "\n" + "dated " + row["MAWBDate"].ToString();
            Ref = "Bond No. " + row["BondNum"].ToString() + " Dated" + row["BondReg"].ToString() + "\n" + "Bond B/E No. " + row["BOENo"].ToString() + "dated " + row["BOEDate"].ToString();
            EXConsigneeNm = row["Customer"].ToString();
            InConsigneeNm = row["InBondCustName"].ToString();
            InAdd = row["InBondAddress"].ToString();
            EXAdd = row["EXCustADD"].ToString();
            break;
        }

        int NoOfColumn = 0, i = 0;
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=SalePurchaseLetter-" + txtJobId.Text + ".pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        sw.Write("<br/>");
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        StringReader sr = new StringReader(sw.ToString());

        iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
        Document pdfDoc = new Document(recPDF);

        //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();

        Font GridHeadingFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
        Font TextBoldformat = FontFactory.GetFont("Arial", 10, Font.BOLD);
        Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.RED);
        Font ColorMsg = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED);
        Font HeaderFontformat = FontFactory.GetFont("Arial", 12, Font.BOLD);
        Font SubHeadFormat = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLUE);

        string contents = "";
        contents = File.ReadAllText(Server.MapPath("SalePurchaseLetter.htm"));
        //contents = contents.Replace("[lblbndNum]", lblbndNum.Text);

        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
        foreach (var htmlelement in parsedContent)
            pdfDoc.Add(htmlelement as IElement);

        NoOfColumn = 1;
        Paragraph ParaSpace = new Paragraph();
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(new Paragraph("\n" + "\n", TextFontformat));

        PdfPTable pdftable = new PdfPTable(NoOfColumn);   //Create new table
        pdftable.TotalWidth = 550f;
        pdftable.LockedWidth = true;

        PdfPCell cellwithdata0 = new PdfPCell(new Phrase(lblTodayDt1.Text, TextFontformat));
        cellwithdata0.HorizontalAlignment = Element.ALIGN_RIGHT;
        cellwithdata0.Border = 0;
        pdftable.AddCell(cellwithdata0);

        pdfDoc.Add(pdftable);

        pdfDoc.Add(new Paragraph("To", TextFontformat));
        pdfDoc.Add(new Paragraph("The Assistant Commission of Customs", TextFontformat));
        pdfDoc.Add(new Paragraph("Bond Department", TextFontformat));
        pdfDoc.Add(new Paragraph("Jawahar Custom House,", TextFontformat));
        pdfDoc.Add(new Paragraph("Nhava Sheva" + "\n" + "\n", TextFontformat));

        pdfDoc.Add(new Paragraph("Dear Sir," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Sub : " + Sub + "\n" + "\n", TextBoldformat));
        pdfDoc.Add(new Paragraph("Ref : " + Ref + "\n" + "\n", TextBoldformat));

        pdfDoc.Add(new Paragraph("With reference to the captioned bonded of the goods we wish to bring to your kind notice that we intend to transfer the same to M/S ." + EXConsigneeNm + " .," + EXAdd + ". You are requested to allow the transfer as provided under provision to sub section of (3) section 59 of the customs act 1962. " + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("The said M/S " + EXConsigneeNm + " . will execute the necessary Bond in lieu of the double duty executed and submitted by us. And thereafter the transferee shall be entitled  to clear the goods as and when they deem fit or as may be directed by the authorities of customs. " + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("You are requested to permit the same and oblige." + "\n" + "\n", TextFontformat));

        pdfDoc.Add(new Paragraph("Thanking you," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Yours faithfully,", TextFontformat));
        pdfDoc.Add(new Paragraph(InConsigneeNm + "\n" + "\n" + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Authorized Signatory.", TextBoldformat));

        pdfDoc.NewPage();

        // 2nd letter
        NoOfColumn = 1;
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(new Paragraph("\n" + "\n", TextFontformat));

        PdfPTable pdftable1 = new PdfPTable(NoOfColumn);   //Create new table
        pdftable1.TotalWidth = 550f;
        pdftable1.LockedWidth = true;

        PdfPCell cellwithdata1 = new PdfPCell(new Phrase(lblTodayDt1.Text, TextFontformat));
        cellwithdata1.HorizontalAlignment = Element.ALIGN_RIGHT;
        cellwithdata1.Border = 0;
        pdftable1.AddCell(cellwithdata1);

        pdfDoc.Add(pdftable1);

        pdfDoc.Add(new Paragraph("To", TextFontformat));
        pdfDoc.Add(new Paragraph("The Assistant Commission of Customs", TextFontformat));
        pdfDoc.Add(new Paragraph("Bond Department", TextFontformat));
        pdfDoc.Add(new Paragraph("Jawahar Custom House,", TextFontformat));
        pdfDoc.Add(new Paragraph("Nhava Sheva" + "\n" + "\n", TextFontformat));

        pdfDoc.Add(new Paragraph("Dear Sir," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Sub : " + Sub + "\n" + "\n", TextBoldformat));
        pdfDoc.Add(new Paragraph("Ref : " + Ref + "\n" + "\n", TextBoldformat));

        pdfDoc.Add(new Paragraph("With reference to the above , please find enclosed herewith the letter of M/s. " + InConsigneeNm + InAdd + " with itself is self explanatory. We confirm having acquired the ownership of the subject goods as per provision to sub-section 3 of section 59 of the Customs act 1962. Read with Notification No. 64/93-CVS (N.T.) dated 27th December 1994. ", TextFontformat));
        pdfDoc.Add(new Paragraph("We submit here with the Double duty bond as required under the provision of section 59 of CA 62 for the entire consignment and request that the warehoused goods in question please be transferred in our name.", TextFontformat));
        pdfDoc.Add(new Paragraph("We shall be clearing the goods at the earliest.", TextFontformat));

        pdfDoc.Add(new Paragraph("Thanking you," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Yours faithfully,", TextFontformat));
        pdfDoc.Add(new Paragraph(EXConsigneeNm + "\n" + "\n" + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Authorized Signatory.", TextBoldformat));

        pdfDoc.NewPage();

        // 3rd letter
        NoOfColumn = 1;
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(new Paragraph("\n" + "\n", TextFontformat));

        PdfPTable pdftable2 = new PdfPTable(NoOfColumn);   //Create new table
        pdftable2.TotalWidth = 550f;
        pdftable2.LockedWidth = true;

        PdfPCell cellwithdata2 = new PdfPCell(new Phrase(lblTodayDt1.Text, TextFontformat));
        cellwithdata2.HorizontalAlignment = Element.ALIGN_RIGHT;
        cellwithdata2.Border = 0;
        pdftable2.AddCell(cellwithdata2);

        pdfDoc.Add(pdftable2);

        pdfDoc.Add(new Paragraph("To", TextFontformat));
        pdfDoc.Add(new Paragraph("The Assistant Commission of Customs", TextFontformat));
        pdfDoc.Add(new Paragraph("Bond Department", TextFontformat));
        pdfDoc.Add(new Paragraph("Jawahar Custom House,", TextFontformat));
        pdfDoc.Add(new Paragraph("Nhava Sheva" + "\n" + "\n", TextFontformat));

        pdfDoc.Add(new Paragraph("Dear Sir," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Sub : " + Sub + "\n" + "\n", TextBoldformat));
        pdfDoc.Add(new Paragraph("Ref : " + Ref + "\n" + "\n", TextBoldformat));

        pdfDoc.Add(new Paragraph("We wish to bring to your kind notice that we have sold the above consignment to :-", TextFontformat));
        pdfDoc.Add(new Paragraph(EXConsigneeNm, TextFontformat));
        pdfDoc.Add(new Paragraph(EXAdd, TextFontformat));

        pdfDoc.Add(new Paragraph("Thanking you," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Yours faithfully,", TextFontformat));
        pdfDoc.Add(new Paragraph(InConsigneeNm + "\n" + "\n" + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Authorized Signatory.", TextBoldformat));

        pdfDoc.NewPage();
        // 4th letter
        NoOfColumn = 1;
        ParaSpace.SpacingBefore = 5;

        pdfDoc.Add(new Paragraph("\n" + "\n", TextFontformat));

        PdfPTable pdftable3 = new PdfPTable(NoOfColumn);   //Create new table
        pdftable3.TotalWidth = 550f;
        pdftable3.LockedWidth = true;

        PdfPCell cellwithdata3 = new PdfPCell(new Phrase(lblTodayDt1.Text, TextFontformat));
        cellwithdata3.HorizontalAlignment = Element.ALIGN_RIGHT;
        cellwithdata3.Border = 0;
        pdftable3.AddCell(cellwithdata3);

        pdfDoc.Add(pdftable3);

        pdfDoc.Add(new Paragraph("To", TextFontformat));
        pdfDoc.Add(new Paragraph("The Assistant Commission of Customs", TextFontformat));
        pdfDoc.Add(new Paragraph("Bond Department", TextFontformat));
        pdfDoc.Add(new Paragraph("Jawahar Custom House,", TextFontformat));
        pdfDoc.Add(new Paragraph("Nhava Sheva" + "\n" + "\n", TextFontformat));

        pdfDoc.Add(new Paragraph("Dear Sir," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Sub : " + Sub + "\n" + "\n", TextBoldformat));
        pdfDoc.Add(new Paragraph("Ref : " + Ref + "\n" + "\n", TextBoldformat));

        pdfDoc.Add(new Paragraph("We have purchased the above goods from " + InConsigneeNm + InAdd, TextFontformat));

        pdfDoc.Add(new Paragraph("\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Thanking you," + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Yours faithfully,", TextFontformat));
        pdfDoc.Add(new Paragraph("For " + EXConsigneeNm + "\n" + "\n" + "\n" + "\n", TextFontformat));
        pdfDoc.Add(new Paragraph("Authorized Signatory.", TextBoldformat));

        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }



    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompletionList(string prefixText, int count)
    {
        return AutoFillProducts(prefixText);

    }

    private static List<string> AutoFillProducts(string prefixText)
    {
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

            using (SqlCommand com = new SqlCommand())
            {
                com.CommandText = "select JobRefNo from BS_JobDetail where " + "JobRefNo like'%'+ @Search + '%'";

                com.Parameters.AddWithValue("@Search", prefixText);
                com.Connection = con;
                con.Open();
                List<string> JobRefNo = new List<string>();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        JobRefNo.Add(sdr["JobRefNo"].ToString());
                    }
                }
                con.Close();
                return JobRefNo;


            }

        }
    }

    public static String ConvertAmount(double amount)
    {
        try
        {
            Int64 amount_int = (Int64)amount;
            Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
            if (amount_dec == 0)
            {
                return Convert1(amount_int) + " Only.";
            }
            else
            {
                return Convert1(amount_int) + " Point " + Convert1(amount_dec) + " Only.";
            }
        }
        catch (Exception e)
        {
            // TODO: handle exception  
        }
        return "";
    }

    public static String Convert1(Int64 i)
    {
        if (i < 20)
        {
            return units[i];
        }
        if (i < 100)
        {
            return tens[i / 10] + ((i % 10 > 0) ? " " + Convert1(i % 10) : "");
        }
        if (i < 1000)
        {
            return units[i / 100] + " Hundred"
                    + ((i % 100 > 0) ? " And " + Convert1(i % 100) : "");
        }
        if (i < 100000)
        {
            return Convert1(i / 1000) + " Thousand "
            + ((i % 1000 > 0) ? " " + Convert1(i % 1000) : "");
        }
        if (i < 10000000)
        {
            return Convert1(i / 100000) + " Lakh "
                    + ((i % 100000 > 0) ? " " + Convert1(i % 100000) : "");
        }
        if (i < 1000000000)
        {
            return Convert1(i / 10000000) + " Crore "
                    + ((i % 10000000 > 0) ? " " + Convert1(i % 10000000) : "");
        }
        return Convert1(i / 1000000000) + " Arab "
                + ((i % 1000000000 > 0) ? " " + Convert1(i % 1000000000) : "");
    }


}