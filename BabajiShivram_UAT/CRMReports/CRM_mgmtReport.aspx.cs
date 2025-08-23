using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using AjaxControlToolkit;
using QueryStringEncryption;
using Ionic.Zip;
using ClosedXML.Excel;


public partial class CRMReports_CRM_mgmtReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    DateTime dtStartDate = new DateTime();
    DateTime dtEndDate = new DateTime();
    int CurrentStatusType;
    string Action_Perform = "", StatusName = "";
    DataTable dtLead = new DataTable();
    string msg = "No Record Found!";
    //private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnExportToPDF);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblSalePerson.Text = "All";
            //txtStartDate.Text = Convert.ToString("01/04/2019");
            txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());
            //string value= LoggedInUser.glUserId.ToString();
            //ddlUser.SelectedValue = value;
            //ddlUser.SelectedIndex = LoggedInUser.glUserId;
            Bind_Data();
            //lblCurrentStatus5.Text = "No Status";
        }
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        //DateTime dtStartDate = DateTime.MinValue;
        if (txtStartDate.Text.Trim() != "")
        {
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            //txtEndDate.Text = dtStartDate.AddDays(6).ToString("dd/MM/yyyy");
        }
        else
        {
            lblError.Text = "Select Start Date";
            lblError.CssClass = "errorMsg";
        }
        Bind_Data();
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        //DateTime dtEndDate = DateTime.MinValue;
        if (txtEndDate.Text.Trim() != "")
        {
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());
            //txtStartDate.Text = dtEndDate.AddDays(-6).ToString("dd/MM/yyyy");
        }
        else
        {
            lblError.Text = "Select End Date";
            lblError.CssClass = "errorMsg";
        }
        Bind_Data();
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUser.SelectedValue != "" || ddlUser.SelectedValue != null)
        {
            //InvisibleFiled();
            if (ddlUser.SelectedValue == "0")
            {
                lblSalePerson.Text = "All";
            }
            else
            {
                lblSalePerson.Text = ddlUser.SelectedItem.Text;
            }
        }
        Bind_Data();
    }

    protected void btnExportToPDF_Click(object sender, EventArgs e)
    {
        //string CanUserName = LoggedInUser.glEmpName;
        int NoOfColumn = 0, i = 0;
        //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
        string date = DateTime.Today.ToShortDateString();
        string UserName = "";

        try
        {
            //**********************************************************************************************
            // Generate PDF
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CRM_mgmt_Report_" + ddlUser.SelectedItem + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            //sw.Write("<br/>");
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            StringReader sr = new StringReader(sw.ToString());

            iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4.Rotate());
            Document pdfDoc = new Document(recPDF);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //***********************************************************************************************

            pdfDoc.Open();

            Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.RED);
            Font ColorMsg = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED);
            Font HeaderFontformat = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font SubHeadFormat = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLUE);

            if (ddlUser.SelectedValue == "0")
            {
                UserName = "All";
            }
            else
            {
                UserName = ddlUser.SelectedItem.Text;
            }

            NoOfColumn = 6;
            string contents = "";
            contents = File.ReadAllText(Server.MapPath("CRM_mgmtReport.htm"));
            contents = contents.Replace("[TodayDate]", date.ToString());
            contents = contents.Replace("[SalesPerson]", UserName);
            contents = contents.Replace("[DateBetween]", txtStartDate.Text.Trim() + " TO " + txtEndDate.Text.Trim());
            contents = contents.Replace("[SalesPerson]", lblSalePerson.Text);

            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);

            /***********************************  Lead  *****************************************/
            string Type1 = lblLead.Text;
            pdfDoc.Add(new Paragraph(Type1, SubHeadFormat));

            Paragraph ParaSpace = new Paragraph();
            ParaSpace.SpacingBefore = 5;

            pdfDoc.Add(ParaSpace);
            Action_Perform = "Lead";
            dtLead = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdftable = new PdfPTable(NoOfColumn);   //Create new table

            pdftable.TotalWidth = 800f;
            pdftable.LockedWidth = true;

            float[] widths = new float[] { 0.1f, 0.6f, 0.5f, 0.4f, 0.35f, 1.6f };
            pdftable.SetWidths(widths);
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

            // Set Table Spacing Before And After html text
            pdftable.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text

            // Header:SERIAL NUMBER
            PdfPCell cellwithdata0 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdftable.AddCell(cellwithdata0);

            // Header:LEAD NAME
            PdfPCell cellwithdata = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            cellwithdata.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata);

            // Header: SERVICE
            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("SERVICE", GridHeadingFont));
            cellwithdata1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata1);

            // Header: LATEST STATUS
            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("LATEST STATUS", GridHeadingFont));
            cellwithdata2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata2);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata3 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata3.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata3);

            // Header: LATEST STATUS REMARK
            PdfPCell cellwithdata4 = new PdfPCell(new Phrase("LATEST STATUS REMARK", GridHeadingFont));
            cellwithdata4.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata4);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl = new PdfPCell();
            CellSl.Colspan = 1;
            CellSl.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellLeadName = new PdfPCell();
            CellLeadName.Colspan = 1;
            CellLeadName.UseVariableBorders = false;

            // Data Cell: Service

            PdfPCell CellService = new PdfPCell();
            CellService.Colspan = 1;
            CellService.UseVariableBorders = false;

            // Data Cell: Lead Status

            PdfPCell CellLeadStatus = new PdfPCell();
            CellLeadStatus.Colspan = 1;
            CellLeadStatus.UseVariableBorders = false;

            // Data Cell:Expected Volumn

            PdfPCell CellExpectedVolumn = new PdfPCell();
            CellExpectedVolumn.Colspan = 1;
            CellExpectedVolumn.UseVariableBorders = false;

            // Data Cell:CellRemark

            PdfPCell CellRemark = new PdfPCell();
            CellRemark.Colspan = 1;
            CellRemark.UseVariableBorders = false;

            if (dtLead.Rows.Count > 0)
            {
                foreach (DataRow row in dtLead.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdftable.AddCell(CellSl);

                    //cellLeadName
                    CellLeadName.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdftable.AddCell(CellLeadName);

                    // CellService
                    CellService.Phrase = new Phrase(Convert.ToString(row["Services"]), TextFontformat);
                    pdftable.AddCell(CellService);

                    // CellLeadStatus
                    CellLeadStatus.Phrase = new Phrase(Convert.ToString(row["LeadStatus"]), TextFontformat);
                    pdftable.AddCell(CellLeadStatus);

                    // CellExpectedVolumn
                    CellExpectedVolumn.Phrase = new Phrase(Convert.ToString(row["Expected_Volume"]), TextFontformat);
                    pdftable.AddCell(CellExpectedVolumn);

                    // CellRemark
                    CellRemark.Phrase = new Phrase(Convert.ToString(row["Remark"]), TextFontformat);
                    pdftable.AddCell(CellRemark);

                } //end of foreach
                i = 0;
                pdfDoc.Add(pdftable);
            }
            else
            {
                pdfDoc.Add(pdftable);
                pdfDoc.Add(new Paragraph(lblErrMsg1.Text, ColorMsg));
            }

            /***********************************  Lead approved  *****************************************/

            string Type2 = lblLeadApproved.Text;
            pdfDoc.Add(new Paragraph(Type2, SubHeadFormat));
            NoOfColumn = 8;

            Action_Perform = "LeadApproved";
            DataTable dtLeadApproved = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdfLeadApproved = new PdfPTable(NoOfColumn);   //Create new table
            float[] widths1 = new float[] { 0.1f, 0.7f, 0.5f, 0.35f, 0.5f, 1.2f, 1.2f, 0.5f };
            pdfLeadApproved.SetWidths(widths1);
            pdfLeadApproved.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.TotalWidth = 800f;
            pdfLeadApproved.LockedWidth = true;

            // Set Table Spacing Before And After html text
            pdfLeadApproved.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata10 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfLeadApproved.AddCell(cellwithdata10);

            // Header:LEAD NAME
            PdfPCell cellwithdata11 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfLeadApproved.AddCell(cellwithdata11);

            // Header: SERVICE
            PdfPCell cellwithdata12 = new PdfPCell(new Phrase("SERVICE", GridHeadingFont));
            pdfLeadApproved.AddCell(cellwithdata12);

            // Header: TARGET MONTH
            PdfPCell cellwithdata13 = new PdfPCell(new Phrase("TARGET MONTH", GridHeadingFont));
            cellwithdata13.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.AddCell(cellwithdata13);

            // Header: LATEST STATUS
            PdfPCell cellwithdata14 = new PdfPCell(new Phrase("LATEST STATUS", GridHeadingFont));
            cellwithdata14.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.AddCell(cellwithdata14);

            // Header: LATEST STATUS REMARK
            PdfPCell cellwithdata15 = new PdfPCell(new Phrase("LATEST STATUS REMARK", GridHeadingFont));
            cellwithdata15.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.AddCell(cellwithdata15);

            // Header: LAST VISIT REMARK
            PdfPCell cellwithdata16 = new PdfPCell(new Phrase("LAST VISIT REMARK", GridHeadingFont));
            cellwithdata16.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.AddCell(cellwithdata16);

            // Header: LAST VISIT DATE
            PdfPCell cellwithdata17 = new PdfPCell(new Phrase("LAST VISIT DATE", GridHeadingFont));
            cellwithdata17.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfLeadApproved.AddCell(cellwithdata17);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl1 = new PdfPCell();
            CellSl1.Colspan = 1;
            CellSl1.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellDescription1 = new PdfPCell();
            CellDescription1.Colspan = 1;
            CellDescription1.UseVariableBorders = false;

            // Data Cell: Service

            PdfPCell CellService1 = new PdfPCell();
            CellService1.Colspan = 1;
            CellService1.UseVariableBorders = false;

            // Data Cell: Target Month

            PdfPCell CellTargetMonth1 = new PdfPCell();
            CellTargetMonth1.Colspan = 1;
            CellTargetMonth1.UseVariableBorders = false;

            // Data Cell: Latest Status

            PdfPCell CellLatestStatus1 = new PdfPCell();
            CellLatestStatus1.Colspan = 1;
            CellLatestStatus1.UseVariableBorders = false;

            // Data Cell: Latest Status Remark

            PdfPCell CellLatestStatusRemark1 = new PdfPCell();
            CellLatestStatusRemark1.Colspan = 1;
            CellLatestStatusRemark1.UseVariableBorders = false;

            // Data Cell: Latest Status Remark

            PdfPCell CellvisitStatus1 = new PdfPCell();
            CellvisitStatus1.Colspan = 1;
            CellvisitStatus1.UseVariableBorders = false;

            // Data Cell: Latest Status Remark

            PdfPCell CellVisitDate1 = new PdfPCell();
            CellVisitDate1.Colspan = 1;
            CellVisitDate1.UseVariableBorders = false;

            if (dtLeadApproved.Rows.Count > 0)
            {
                foreach (DataRow row in dtLeadApproved.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl1.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfLeadApproved.AddCell(CellSl1);
                    //cellLeadName
                    CellDescription1.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellDescription1);

                    // CellService
                    CellService1.Phrase = new Phrase(Convert.ToString(row["Services"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellService1);

                    // CellTargetMonth
                    CellTargetMonth1.Phrase = new Phrase(Convert.ToString(row["TARGET_MON"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellTargetMonth1);

                    // CellLatestStatus
                    CellLatestStatus1.Phrase = new Phrase(Convert.ToString(row["LeadStatus"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellLatestStatus1);

                    // Cellremark
                    CellRemark.Phrase = new Phrase(Convert.ToString(row["Remark"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellRemark);

                    // CellVisitRemark
                    CellvisitStatus1.Phrase = new Phrase(Convert.ToString(row["VISIT_REMARK"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellvisitStatus1);

                    // CellVisitDate
                    CellVisitDate1.Phrase = new Phrase(Convert.ToString(row["VisitDate"]), TextFontformat);
                    pdfLeadApproved.AddCell(CellVisitDate1);

                } //end of foreach
                i = 0;
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfLeadApproved);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfLeadApproved);
                pdfDoc.Add(new Paragraph(lblErrMsg2.Text, ColorMsg));
            }

            ///***********************************  Type 3  *****************************************/

            string Type3 = lblQuote.Text;
            pdfDoc.Add(new Paragraph(Type3, SubHeadFormat));
            Action_Perform = "Quote";
            DataTable dtQuote = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            NoOfColumn = 8;
            PdfPTable pdfQuote = new PdfPTable(NoOfColumn);   //Create new table

            pdfQuote.TotalWidth = 800f;
            pdfQuote.LockedWidth = true;

            float[] widths2 = new float[] { 0.1f, 0.7f, 0.5f, 0.35f, 0.5f, 1.2f, 1.2f, 0.5f };
            pdfQuote.SetWidths(widths2);
            pdfQuote.HorizontalAlignment = Element.ALIGN_LEFT;


            // Set Table Spacing Before And After html text
            //   pdftable.SpacingBefore = 10f;
            pdfQuote.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata20 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfQuote.AddCell(cellwithdata20);

            // Header:LEAD NAME
            PdfPCell cellLeadName21 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfQuote.AddCell(cellLeadName21);

            // Header: Service
            PdfPCell cellService22 = new PdfPCell(new Phrase("SERVICES", GridHeadingFont));
            pdfQuote.AddCell(cellService22);

            // Header: Quote Date
            PdfPCell cellQuoteDate23 = new PdfPCell(new Phrase("QUOTE DATE", GridHeadingFont));
            cellQuoteDate23.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfQuote.AddCell(cellQuoteDate23);

            // Header: Quote Status
            PdfPCell cellQuoteStatus24 = new PdfPCell(new Phrase("QUOTE STATUS", GridHeadingFont));
            cellQuoteStatus24.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfQuote.AddCell(cellQuoteStatus24);

            // Header: latest Status Remark
            PdfPCell cellLatestStatusRemark25 = new PdfPCell(new Phrase("LATEST STATUS REMARK", GridHeadingFont));
            cellLatestStatusRemark25.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfQuote.AddCell(cellLatestStatusRemark25);

            PdfPCell celllastVisitRemark26 = new PdfPCell(new Phrase("LAST VISIT REMARK", GridHeadingFont));
            celllastVisitRemark26.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfQuote.AddCell(celllastVisitRemark26);

            PdfPCell cellLastVisitDate27 = new PdfPCell(new Phrase("LAST VISIT DATE", GridHeadingFont));
            cellLastVisitDate27.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfQuote.AddCell(cellLastVisitDate27);


            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl2 = new PdfPCell();
            CellSl2.Colspan = 1;
            CellSl2.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellLeadName2 = new PdfPCell();
            CellLeadName2.Colspan = 1;
            CellLeadName2.UseVariableBorders = false;

            // Data Cell: Service

            PdfPCell CellService2 = new PdfPCell();
            CellService2.Colspan = 1;
            CellService2.UseVariableBorders = false;

            // Data Cell: Quote Date

            PdfPCell CellQuoteDate2 = new PdfPCell();
            CellQuoteDate2.Colspan = 1;
            CellQuoteDate2.UseVariableBorders = false;

            // Data Cell: Quote Status

            PdfPCell CellQuoteStatus2 = new PdfPCell();
            CellQuoteStatus2.Colspan = 1;
            CellQuoteStatus2.UseVariableBorders = false;

            // Data Cell: Latest status Remark

            PdfPCell CellLatestStatusRemark2 = new PdfPCell();
            CellLatestStatusRemark2.Colspan = 1;
            CellLatestStatusRemark2.UseVariableBorders = false;

            // Data Cell: Visit Date

            PdfPCell CellVisitDate2 = new PdfPCell();
            CellVisitDate2.Colspan = 1;
            CellVisitDate2.UseVariableBorders = false;

            // Data Cell: Visit Remark

            PdfPCell CellVisitRemark2 = new PdfPCell();
            CellVisitRemark2.Colspan = 1;
            CellVisitRemark2.UseVariableBorders = false;

            if (dtQuote.Rows.Count > 0)
            {
                foreach (DataRow row in dtQuote.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl2.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfQuote.AddCell(CellSl2);

                    //cellLeadName
                    CellLeadName2.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfQuote.AddCell(CellLeadName2);

                    // CellService
                    CellService2.Phrase = new Phrase(Convert.ToString(row["SERVICES"]), TextFontformat);
                    pdfQuote.AddCell(CellService2);

                    // CellQuoteDate
                    CellQuoteDate2.Phrase = new Phrase(Convert.ToString(row["dtDate"]), TextFontformat);
                    pdfQuote.AddCell(CellQuoteDate2);

                    // CellQuotestatus
                    CellQuoteStatus2.Phrase = new Phrase(Convert.ToString(row["QuotationStatus"]), TextFontformat);
                    pdfQuote.AddCell(CellQuoteStatus2);

                    // CellActivityDetails
                    CellLatestStatusRemark2.Phrase = new Phrase(Convert.ToString(row["Remark"]), TextFontformat);
                    pdfQuote.AddCell(CellLatestStatusRemark2);

                    // CellActivityDetails
                    CellVisitRemark2.Phrase = new Phrase(Convert.ToString(row["VISIT_REMARK"]), TextFontformat);
                    pdfQuote.AddCell(CellVisitRemark2);

                    // CellActivityDetails
                    CellVisitDate2.Phrase = new Phrase(Convert.ToString(row["VisitDate"]), TextFontformat);
                    pdfQuote.AddCell(CellVisitDate2);

                } //end of foreach
                i = 0;
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfQuote);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfQuote);
                pdfDoc.Add(new Paragraph(lblErrMsg3.Text, ColorMsg));
            }

            pdfDoc.Add(ParaSpace);

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Successfully Downloaded ');", true);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void Repeater_Lead_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell cellHead1 = (HtmlTableCell)e.Item.FindControl("lblUName1");
            if (cellHead1 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellHead1.Visible = true;
                }
                else
                {
                    cellHead1.Visible = false;
                }

            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlTableCell cellDetail1 = (HtmlTableCell)e.Item.FindControl("lblName1");
            if (cellDetail1 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellDetail1.Visible = true;
                }
                else
                {
                    cellDetail1.Visible = false;
                }

            }
        }
    }

    protected void Repeater_LeadApproved_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell cellHead2 = (HtmlTableCell)e.Item.FindControl("lblUName2");
            if (cellHead2 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellHead2.Visible = true;
                }
                else
                {
                    cellHead2.Visible = false;
                }

            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlTableCell cellDetail2 = (HtmlTableCell)e.Item.FindControl("lblName2");
            if (cellDetail2 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellDetail2.Visible = true;
                }
                else
                {
                    cellDetail2.Visible = false;
                }

            }
        }
    }

    protected void Repeater_Quote_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell cellHead3 = (HtmlTableCell)e.Item.FindControl("lblUName3");
            if (cellHead3 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellHead3.Visible = true;
                }
                else
                {
                    cellHead3.Visible = false;
                }

            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlTableCell cellDetail3 = (HtmlTableCell)e.Item.FindControl("lblName3");
            if (cellDetail3 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellDetail3.Visible = true;
                }
                else
                {
                    cellDetail3.Visible = false;
                }

            }
        }
    }
    protected void Bind_Data()
    {
        Action_Perform = "Lead"; // FindRecord for particular status id
        dtLead = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
        if (dtLead.Rows.Count > 0)
        {
            lblLead.Text = Action_Perform;
            Repeater_Lead.DataSource = dtLead;
            Repeater_Lead.DataBind();
            Repeater_Lead.Visible = true;
            ViewState["dt"] = dtLead;
            lblErrMsg1.Visible = false;
            lblErrMsg1.Text = "";
        }
        else
        {
            lblLead.Text = Action_Perform;
            Repeater_Lead.Visible = false;
            lblErrMsg1.Text = "No Record found for " + lblLead.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
            lblErrMsg1.Visible = true;
        }

        Action_Perform = "LeadApproved";
        DataTable dtLeadApproved = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
        if (dtLeadApproved.Rows.Count > 0)
        {
            lblLeadApproved.Text = Action_Perform;
            Repeater_LeadApproved.DataSource = dtLeadApproved;
            Repeater_LeadApproved.DataBind();
            Repeater_LeadApproved.Visible = true;
            lblErrMsg2.Visible = false;
            lblErrMsg2.Text = "";
        }
        else
        {
            lblLeadApproved.Text = Action_Perform;
            Repeater_LeadApproved.Visible = false;
            lblErrMsg2.Text = "No Record found for " + lblLeadApproved.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
            lblErrMsg2.Visible = true;
        }

        Action_Perform = "Quote";
        DataTable dtQuote = DBOperations.CRM_rptMgmtReport(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

        if (dtQuote.Rows.Count > 0)
        {
            lblQuote.Text = Action_Perform;
            Repeater_Quote.DataSource = dtQuote;
            Repeater_Quote.DataBind();
            Repeater_Quote.Visible = true;
            lblErrMsg3.Visible = false;
            lblErrMsg3.Text = "";
        }
        else
        {
            lblQuote.Text = Action_Perform;
            Repeater_Quote.Visible = false;
            lblErrMsg3.Text = "No Record found for " + lblQuote.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
            lblErrMsg3.Visible = true;
        }
    }
}
