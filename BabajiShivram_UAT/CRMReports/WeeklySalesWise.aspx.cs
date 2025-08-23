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

public partial class CRMReports_WeeklySalesWise : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    DateTime dtStartDate = new DateTime();
    DateTime dtEndDate = new DateTime();
    int CurrentStatusType;
    string Action_Perform = "", StatusName = "";
    DataTable dtWeeklySalesWise = new DataTable();
    string msg = "No Record Found!";
    private static Random _random = new Random();
    string value = ""; int User = 0; int UserId;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnExportToPDF);
       // ScriptManager1.RegisterPostBackControl(btnExportToExcel);

        if (!IsPostBack)
        {
            lblErrMsg1.Text = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Weekly Sales Report";
            lblSalePerson.Text = "All";
            txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());

            lblCurrentStatus5.Text = "No Status";
            value = LoggedInUser.glUserId.ToString();
            ddlUser.SelectedValue = value;
        }
        if (ddlUser.SelectedValue == "0")
        {
            UserId = LoggedInUser.glUserId;
        }
        else
        {
            UserId = Convert.ToInt32(ddlUser.SelectedValue);
        }
        Bind_Data();
        BindVisitData();
        BindPendingEnquiry();
        BindOnBoardCust();
        BindVolumeAnalysisImportCHA();
        BindVolumeAnalysisExportCHA();
        BindCRMVolumeAnalysis_Freight();
        BindGetPendingEnquiryLast2Month();
        BindGetPendingFRSummary();
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        //DateTime dtStartDate = DateTime.MinValue;
        if (txtStartDate.Text.Trim() != "")
        {
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            //txtEndDate.Text = dtStartDate.AddDays(6).ToString("dd/MM/yyyy");
            Bind_Data();
        }
        else
        {
            lblError.Text = "Select Start Date";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        //DateTime dtEndDate = DateTime.MinValue;
        if (txtEndDate.Text.Trim() != "")
        {
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());
            //txtStartDate.Text = dtEndDate.AddDays(-6).ToString("dd/MM/yyyy");
            Bind_Data();
        }
        else
        {
            lblError.Text = "Select End Date";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUser.SelectedValue != "" || ddlUser.SelectedValue != null)
        {
            InvisibleFiled();
            if (ddlUser.SelectedValue == "0")
            {
                lblSalePerson.Text = "All";
            }
            else
            {
                lblSalePerson.Text = ddlUser.SelectedItem.Text;
            }
            Bind_Data();
        }
    }

    protected void Bind_Data()
    {
        Action_Perform = "1";  //FindType for current status id
        if (Action_Perform == "1")
        {
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());
            if (value != "")
            {
                User = LoggedInUser.glUserId;
            }
            else
            {
                User = Convert.ToInt32(ddlUser.SelectedValue);
            }
            DataTable dt = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 0, dtStartDate, dtEndDate, Convert.ToInt32(User), LoggedInUser.glFinYearId);
            if (dt.Rows.Count > 0)
            {
                string Type = "";
                foreach (DataRow row in dt.Rows)
                {
                    Type = row["lid"].ToString();
                    StatusName = row["StatusName"].ToString();
                    FindRecordExist();
                    BindData_rptWeeklySaleWise1(Convert.ToInt32(Type), StatusName);
                }
                BindData_NotUpdatedStatus();
            }
            else
            {

            }

            if (ddlUser.SelectedValue == "0")
            {

            }
        }
    }

    protected void BindData_rptWeeklySaleWise1(int type, string StatusName)
    {

        Action_Perform = "2"; // FindRecord for particular status id
        CurrentStatusType = type;
        dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, CurrentStatusType, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(User), LoggedInUser.glFinYearId);
        if (dtWeeklySalesWise.Rows.Count > 0)
        {
            if (CurrentStatusType == 1)
            {
                lblCurrentStatus1.Text = StatusName;
                Repeater_rptWeeklySaleWise1.DataSource = dtWeeklySalesWise;
                Repeater_rptWeeklySaleWise1.DataBind();
                Repeater_rptWeeklySaleWise1.Visible = true;
                ViewState["dt"] = dtWeeklySalesWise;
                lblErrMsg1.Visible = false;
                lblErrMsg1.Text = "";
            }
            else if (CurrentStatusType == 2)
            {
                lblCurrentStatus2.Text = StatusName;
                Repeater_rptWeeklySaleWise2.DataSource = dtWeeklySalesWise;
                Repeater_rptWeeklySaleWise2.DataBind();
                Repeater_rptWeeklySaleWise2.Visible = true;
                lblErrMsg2.Visible = false;
                lblErrMsg2.Text = "";
            }
            else if (CurrentStatusType == 3)
            {
                lblCurrentStatus3.Text = StatusName;
                Repeater_rptWeeklySaleWise3.DataSource = dtWeeklySalesWise;
                Repeater_rptWeeklySaleWise3.DataBind();
                Repeater_rptWeeklySaleWise3.Visible = true;
                lblErrMsg3.Visible = false;
                lblErrMsg3.Text = "";
            }
            else if (CurrentStatusType == 4)
            {
                lblCurrentStatus4.Text = StatusName;
                Repeater_rptWeeklySaleWise4.DataSource = dtWeeklySalesWise;
                Repeater_rptWeeklySaleWise4.DataBind();
                Repeater_rptWeeklySaleWise4.Visible = true;
                lblErrMsg4.Visible = false;
                lblErrMsg4.Text = "";
            }
        }
        else
        {
            if (CurrentStatusType == 1)
            {
                lblCurrentStatus1.Text = StatusName;
                Repeater_rptWeeklySaleWise1.Visible = false;
                lblErrMsg1.Text = "No Record found for " + lblCurrentStatus1.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
                lblErrMsg1.Visible = true;
            }
            else if (CurrentStatusType == 2)
            {
                lblCurrentStatus2.Text = StatusName;
                Repeater_rptWeeklySaleWise2.Visible = false;
                lblErrMsg2.Text = "No Record found for " + lblCurrentStatus2.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
                lblErrMsg2.Visible = true;
            }
            else if (CurrentStatusType == 3)
            {
                lblCurrentStatus3.Text = StatusName;
                Repeater_rptWeeklySaleWise3.Visible = false;
                lblErrMsg3.Text = "No Record found for " + lblCurrentStatus3.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
                lblErrMsg3.Visible = true;
            }
            else if (CurrentStatusType == 4)
            {
                lblCurrentStatus4.Text = StatusName;
                Repeater_rptWeeklySaleWise4.Visible = false;
                lblErrMsg4.Text = "No Record found for " + lblCurrentStatus4.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
                lblErrMsg4.Visible = true;
            }
        }
    }
    protected void InvisibleFiled()
    {
        lblErrMsg1.Visible = false;
        lblErrMsg2.Visible = false;
        lblErrMsg3.Visible = false;
        lblErrMsg4.Visible = false;
    }

    protected void BindData_NotUpdatedStatus()
    {
        Action_Perform = "3";
        dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 0, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(User), LoggedInUser.glFinYearId);
        if (dtWeeklySalesWise.Rows.Count > 0)
        {
            lblCurrentStatus5.Text = "No Status";
            Repeater_rptWeeklySaleWise5.DataSource = dtWeeklySalesWise;
            Repeater_rptWeeklySaleWise5.DataBind();
            Repeater_rptWeeklySaleWise5.Visible = true;
            lblErrMsg5.Visible = false;
        }
        else
        {
            lblCurrentStatus5.Text = "No Status";
            Repeater_rptWeeklySaleWise5.Visible = false;
            lblErrMsg5.Text = "No Record found for " + lblCurrentStatus5.Text + " activity on " + txtStartDate.Text + " To " + txtEndDate.Text;
            lblErrMsg5.Visible = true;
        }
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
            Action_Perform = "2";
            //***********************************************************************************************
            //string ReportName = "Weekly_Crm_Report_"+ddlUser.SelectedItem;// + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
            //string Name = ReportName.Replace("&amp;", "").ToString();
            //string filePath = Name.Replace(".", "");
            ////filePath = GetFormattedName(filePath);
            //filePath = GetFileName(filePath);
            //string FileFullPath = GetPath(filePath);
            // FileStream fs = new FileStream(FileFullPath, FileMode.Create, FileAccess.Write);    //uncomment
            // PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, fs);  //uncomment
            //**********************************************************************************************
            // Generate PDF
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Weekly_Crm_Report_" + ddlUser.SelectedItem + ".pdf");
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

            //logo.SetAbsolutePosition(380, 720);
            //logo.Alignment = Convert.ToInt32(ImageAlign.Right);
            //pdfDoc.Add(logo);

            if (ddlUser.SelectedValue == "0")
            {
                UserName = "All";
                NoOfColumn = 7;
            }
            else
            {
                UserName = ddlUser.SelectedItem.Text;
                NoOfColumn = 6;
            }

            string contents = "";
            contents = File.ReadAllText(Server.MapPath("WeeklySalesWiseRpt.htm"));
            contents = contents.Replace("[TodayDate]", date.ToString());
            contents = contents.Replace("[SalesPerson]", UserName);
            contents = contents.Replace("[DateBetween]", txtStartDate.Text.Trim() + " TO " + txtEndDate.Text.Trim());
            contents = contents.Replace("[SalesPerson]", lblSalePerson.Text);

            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlelement in parsedContent)
                pdfDoc.Add(htmlelement as IElement);

            /***********************************  Type 1  *****************************************/
            string Type1 = lblCurrentStatus1.Text;
            pdfDoc.Add(new Paragraph(Type1, SubHeadFormat));

            Paragraph ParaSpace = new Paragraph();
            ParaSpace.SpacingBefore = 5;

            pdfDoc.Add(ParaSpace);
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 1, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdftable = new PdfPTable(NoOfColumn);   //Create new table

            pdftable.TotalWidth = 800f;
            pdftable.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.5f, 0.4f, 1.6f, 0.5f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.4f, 0.35f, 1.6f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdftable.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text

            // Header:SERIAL NUMBER
            PdfPCell cellwithdata0 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdftable.AddCell(cellwithdata0);

            // Header:LEAD NAME
            PdfPCell cellwithdata = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdftable.AddCell(cellwithdata);

            // Header: LOCATION
            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("LOCATION", GridHeadingFont));
            pdftable.AddCell(cellwithdata1);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata2);

            // Header: ACTIVITY DATE
            PdfPCell cellwithdata3 = new PdfPCell(new Phrase("ACTIVITY DATE", GridHeadingFont));
            cellwithdata3.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata3);

            // Header: ACTIVITY DETAILS
            PdfPCell cellwithdata4 = new PdfPCell(new Phrase("ACTIVITY DETAILS", GridHeadingFont));
            cellwithdata4.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.AddCell(cellwithdata4);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl = new PdfPCell();
            CellSl.Colspan = 1;
            CellSl.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellDescription = new PdfPCell();
            CellDescription.Colspan = 1;
            CellDescription.UseVariableBorders = false;

            // Data Cell: location

            PdfPCell CellLocation = new PdfPCell();
            CellLocation.Colspan = 1;
            CellLocation.UseVariableBorders = false;

            // Data Cell: expected volume

            PdfPCell CellExpectedVolume = new PdfPCell();
            CellExpectedVolume.Colspan = 1;
            CellExpectedVolume.UseVariableBorders = false;

            // Data Cell: Activity Date

            PdfPCell CellActivityDate = new PdfPCell();
            CellActivityDate.Colspan = 1;
            CellActivityDate.UseVariableBorders = false;

            // Data Cell: Activity Details

            PdfPCell CellActivityDetails = new PdfPCell();
            CellActivityDetails.Colspan = 1;
            CellActivityDetails.UseVariableBorders = false;

            PdfPCell cellwithdata5 = new PdfPCell(new Phrase("SALE PERSON NAME", GridHeadingFont));

            PdfPCell CellPersonName = new PdfPCell();

            if (ddlUser.SelectedValue == "0")
            {
                // Header: SALES PERSON NAME

                cellwithdata5.HorizontalAlignment = Element.ALIGN_LEFT;
                pdftable.AddCell(cellwithdata5);

                // Data Cell: SALES PERSON NAME


                CellPersonName.Colspan = 1;
                CellPersonName.UseVariableBorders = false;
            }

            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                foreach (DataRow row in dtWeeklySalesWise.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdftable.AddCell(CellSl);

                    //cellLeadName
                    CellDescription.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdftable.AddCell(CellDescription);

                    // CellLocation
                    CellLocation.Phrase = new Phrase(Convert.ToString(row["Location"]), TextFontformat);
                    pdftable.AddCell(CellLocation);

                    // CellExpectedVolume
                    CellExpectedVolume.Phrase = new Phrase(Convert.ToString(row["VolumeExpected"]), TextFontformat);
                    pdftable.AddCell(CellExpectedVolume);

                    // CellActivityDate
                    CellActivityDate.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DATE"]), TextFontformat);
                    pdftable.AddCell(CellActivityDate);

                    // CellActivityDetails
                    CellActivityDetails.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DETAILS"]), TextFontformat);
                    pdftable.AddCell(CellActivityDetails);

                    if (ddlUser.SelectedValue == "0")
                    {
                        // CellActivityDetails
                        CellPersonName.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                        pdftable.AddCell(CellPersonName);
                    }
                } //end of foreach
                i = 0;
                pdfDoc.Add(pdftable);
            }
            else
            {
                pdfDoc.Add(pdftable);
                pdfDoc.Add(new Paragraph(lblErrMsg1.Text, ColorMsg));
            }

            /***********************************  Type 2  *****************************************/
            dtWeeklySalesWise.Clear();
            string Type2 = lblCurrentStatus2.Text;
            pdfDoc.Add(new Paragraph(Type2, SubHeadFormat));
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 2, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);


            PdfPTable pdfActionFollowUp = new PdfPTable(NoOfColumn);   //Create new table

            pdfActionFollowUp.TotalWidth = 800f;
            pdfActionFollowUp.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.5f, 0.4f, 1.6f, 0.5f };
                pdfActionFollowUp.SetWidths(widths);
                pdfActionFollowUp.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.4f, 0.35f, 1.6f };
                pdfActionFollowUp.SetWidths(widths);
                pdfActionFollowUp.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            // Set Table Spacing Before And After html text
            pdfActionFollowUp.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata10 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfActionFollowUp.AddCell(cellwithdata10);

            // Header:LEAD NAME
            PdfPCell cellwithdata11 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfActionFollowUp.AddCell(cellwithdata11);

            // Header: LOCATION
            PdfPCell cellwithdata12 = new PdfPCell(new Phrase("LOCATION", GridHeadingFont));
            pdfActionFollowUp.AddCell(cellwithdata12);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata13 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata13.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfActionFollowUp.AddCell(cellwithdata13);

            // Header: ACTIVITY DATE
            PdfPCell cellwithdata14 = new PdfPCell(new Phrase("ACTIVITY DATE", GridHeadingFont));
            cellwithdata14.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfActionFollowUp.AddCell(cellwithdata14);

            // Header: ACTIVITY DETAILS
            PdfPCell cellwithdata15 = new PdfPCell(new Phrase("ACTIVITY DETAILS", GridHeadingFont));
            cellwithdata15.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfActionFollowUp.AddCell(cellwithdata15);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl1 = new PdfPCell();
            CellSl1.Colspan = 1;
            CellSl1.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellDescription1 = new PdfPCell();
            CellDescription1.Colspan = 1;
            CellDescription1.UseVariableBorders = false;

            // Data Cell: location

            PdfPCell CellLocation1 = new PdfPCell();
            CellLocation1.Colspan = 1;
            CellLocation1.UseVariableBorders = false;

            // Data Cell: expected volume

            PdfPCell CellExpectedVolume1 = new PdfPCell();
            CellExpectedVolume1.Colspan = 1;
            CellExpectedVolume1.UseVariableBorders = false;

            // Data Cell: Activity Date

            PdfPCell CellActivityDate1 = new PdfPCell();
            CellActivityDate1.Colspan = 1;
            CellActivityDate1.UseVariableBorders = false;

            // Data Cell: Activity Details

            PdfPCell CellActivityDetails1 = new PdfPCell();
            CellActivityDetails1.Colspan = 1;
            CellActivityDetails1.UseVariableBorders = false;

            PdfPCell cellwithdata16 = new PdfPCell(new Phrase("SALE PERSON NAME", GridHeadingFont));

            PdfPCell CellPersonName1 = new PdfPCell();

            if (ddlUser.SelectedValue == "0")
            {
                // Header: SALES PERSON NAME
                cellwithdata16.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfActionFollowUp.AddCell(cellwithdata16);

                // Data Cell: SALES PERSON NAME
                CellPersonName1.Colspan = 1;
                CellPersonName1.UseVariableBorders = false;
            }

            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                foreach (DataRow row in dtWeeklySalesWise.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl1.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfActionFollowUp.AddCell(CellSl1);
                    //cellLeadName
                    CellDescription1.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfActionFollowUp.AddCell(CellDescription1);

                    // CellLocation
                    CellLocation1.Phrase = new Phrase(Convert.ToString(row["Location"]), TextFontformat);
                    pdfActionFollowUp.AddCell(CellLocation1);

                    // CellExpectedVolume
                    CellExpectedVolume1.Phrase = new Phrase(Convert.ToString(row["VolumeExpected"]), TextFontformat);
                    pdfActionFollowUp.AddCell(CellExpectedVolume1);

                    // CellActivityDate
                    CellActivityDate1.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DATE"]), TextFontformat);
                    pdfActionFollowUp.AddCell(CellActivityDate1);

                    // CellActivityDetails
                    CellActivityDetails1.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DETAILS"]), TextFontformat);
                    pdfActionFollowUp.AddCell(CellActivityDetails1);

                    if (ddlUser.SelectedValue == "0")
                    {
                        // CellActivityDetails
                        CellPersonName1.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                        pdfActionFollowUp.AddCell(CellPersonName1);
                    }
                } //end of foreach
                i = 0;
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfActionFollowUp);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfActionFollowUp);
                pdfDoc.Add(new Paragraph(lblErrMsg2.Text, ColorMsg));
            }
            ///***********************************  Type 3  *****************************************/
            dtWeeklySalesWise.Clear();
            string Type3 = lblCurrentStatus3.Text;
            pdfDoc.Add(new Paragraph(Type3, SubHeadFormat));
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 3, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdfPassiveFollowUp = new PdfPTable(NoOfColumn);   //Create new table

            pdfPassiveFollowUp.TotalWidth = 800f;
            pdfPassiveFollowUp.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.5f, 0.4f, 1.6f, 0.5f };
                pdfPassiveFollowUp.SetWidths(widths);
                pdfPassiveFollowUp.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.4f, 0.35f, 1.6f };
                pdfPassiveFollowUp.SetWidths(widths);
                pdfPassiveFollowUp.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            //   pdftable.SpacingBefore = 10f;
            pdfPassiveFollowUp.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata20 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfPassiveFollowUp.AddCell(cellwithdata20);

            // Header:LEAD NAME
            PdfPCell cellwithdata21 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfPassiveFollowUp.AddCell(cellwithdata21);

            // Header: LOCATION
            PdfPCell cellwithdata22 = new PdfPCell(new Phrase("LOCATION", GridHeadingFont));
            pdfPassiveFollowUp.AddCell(cellwithdata22);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata23 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata23.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPassiveFollowUp.AddCell(cellwithdata23);

            // Header: ACTIVITY DATE
            PdfPCell cellwithdata24 = new PdfPCell(new Phrase("ACTIVITY DATE", GridHeadingFont));
            cellwithdata24.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPassiveFollowUp.AddCell(cellwithdata24);

            // Header: ACTIVITY DETAILS
            PdfPCell cellwithdata25 = new PdfPCell(new Phrase("ACTIVITY DETAILS", GridHeadingFont));
            cellwithdata25.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPassiveFollowUp.AddCell(cellwithdata25);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl2 = new PdfPCell();
            CellSl2.Colspan = 1;
            CellSl2.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellDescription2 = new PdfPCell();
            CellDescription2.Colspan = 1;
            CellDescription2.UseVariableBorders = false;

            // Data Cell: location

            PdfPCell CellLocation2 = new PdfPCell();
            CellLocation2.Colspan = 1;
            CellLocation2.UseVariableBorders = false;

            // Data Cell: expected volume

            PdfPCell CellExpectedVolume2 = new PdfPCell();
            CellExpectedVolume2.Colspan = 1;
            CellExpectedVolume2.UseVariableBorders = false;

            // Data Cell: Activity Date

            PdfPCell CellActivityDate2 = new PdfPCell();
            CellActivityDate2.Colspan = 1;
            CellActivityDate2.UseVariableBorders = false;

            // Data Cell: Activity Details

            PdfPCell CellActivityDetails2 = new PdfPCell();
            CellActivityDetails2.Colspan = 1;
            CellActivityDetails2.UseVariableBorders = false;

            PdfPCell cellwithdata26 = new PdfPCell(new Phrase("SALE PERSON NAME", GridHeadingFont));

            PdfPCell CellPersonName2 = new PdfPCell();

            if (ddlUser.SelectedValue == "0")
            {
                // Header: SALES PERSON NAME
                cellwithdata26.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPassiveFollowUp.AddCell(cellwithdata26);

                // Data Cell: SALES PERSON NAME
                CellPersonName2.Colspan = 1;
                CellPersonName2.UseVariableBorders = false;
            }

            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                foreach (DataRow row in dtWeeklySalesWise.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl2.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellSl2);

                    //cellLeadName
                    CellDescription2.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellDescription2);

                    // CellLocation
                    CellLocation2.Phrase = new Phrase(Convert.ToString(row["Location"]), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellLocation2);

                    // CellExpectedVolume
                    CellExpectedVolume2.Phrase = new Phrase(Convert.ToString(row["VolumeExpected"]), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellExpectedVolume2);

                    // CellActivityDate
                    CellActivityDate2.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DATE"]), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellActivityDate2);

                    // CellActivityDetails
                    CellActivityDetails2.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DETAILS"]), TextFontformat);
                    pdfPassiveFollowUp.AddCell(CellActivityDetails2);

                    if (ddlUser.SelectedValue == "0")
                    {
                        // CellActivityDetails
                        CellPersonName2.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                        pdfPassiveFollowUp.AddCell(CellPersonName2);
                    }

                } //end of foreach
                i = 0;
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfPassiveFollowUp);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfPassiveFollowUp);
                pdfDoc.Add(new Paragraph(lblErrMsg3.Text, ColorMsg));
            }

            ///***********************************  Type 4  *****************************************/
            dtWeeklySalesWise.Clear();
            string Type4 = lblCurrentStatus4.Text;
            pdfDoc.Add(new Paragraph(Type4, SubHeadFormat));
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 4, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdfNoResponse = new PdfPTable(NoOfColumn);   //Create new table

            pdfNoResponse.TotalWidth = 800f;
            pdfNoResponse.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.5f, 0.4f, 1.6f, 0.5f };
                pdfNoResponse.SetWidths(widths);
                pdfNoResponse.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.4f, 0.35f, 1.6f };
                pdfNoResponse.SetWidths(widths);
                pdfNoResponse.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            //   pdftable.SpacingBefore = 10f;
            pdfNoResponse.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata30 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfNoResponse.AddCell(cellwithdata30);

            // Header:LEAD NAME
            PdfPCell cellwithdata31 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfNoResponse.AddCell(cellwithdata31);

            // Header: LOCATION
            PdfPCell cellwithdata32 = new PdfPCell(new Phrase("LOCATION", GridHeadingFont));
            pdfNoResponse.AddCell(cellwithdata32);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata33 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata33.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoResponse.AddCell(cellwithdata33);

            // Header: ACTIVITY DATE
            PdfPCell cellwithdata34 = new PdfPCell(new Phrase("ACTIVITY DATE", GridHeadingFont));
            cellwithdata34.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoResponse.AddCell(cellwithdata34);

            // Header: ACTIVITY DETAILS
            PdfPCell cellwithdata35 = new PdfPCell(new Phrase("ACTIVITY DETAILS", GridHeadingFont));
            cellwithdata35.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoResponse.AddCell(cellwithdata35);

            //CELL ADDED
            //DATA CELL: LEAD NAME
            PdfPCell CellDescription3 = new PdfPCell();
            CellDescription3.Colspan = 1;
            CellDescription3.UseVariableBorders = false;

            // Data Cell: location
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl3 = new PdfPCell();
            CellSl3.Colspan = 1;
            CellSl3.UseVariableBorders = false;

            PdfPCell CellLocation3 = new PdfPCell();
            CellLocation3.Colspan = 1;
            CellLocation3.UseVariableBorders = false;

            // Data Cell: expected volume

            PdfPCell CellExpectedVolume3 = new PdfPCell();
            CellExpectedVolume3.Colspan = 1;
            CellExpectedVolume3.UseVariableBorders = false;

            // Data Cell: Activity Date

            PdfPCell CellActivityDate3 = new PdfPCell();
            CellActivityDate3.Colspan = 1;
            CellActivityDate3.UseVariableBorders = false;

            // Data Cell: Activity Details

            PdfPCell CellActivityDetails3 = new PdfPCell();
            CellActivityDetails3.Colspan = 1;
            CellActivityDetails3.UseVariableBorders = false;

            PdfPCell cellwithdata36 = new PdfPCell(new Phrase("SALE PERSON NAME", GridHeadingFont));

            PdfPCell CellPersonName3 = new PdfPCell();

            if (ddlUser.SelectedValue == "0")
            {
                // Header: SALES PERSON NAME

                cellwithdata36.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfNoResponse.AddCell(cellwithdata36);

                // Data Cell: SALES PERSON NAME
                CellPersonName3.Colspan = 1;
                CellPersonName3.UseVariableBorders = false;
            }

            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                foreach (DataRow row in dtWeeklySalesWise.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl3.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfNoResponse.AddCell(CellSl3);

                    //cellLeadName
                    CellDescription3.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfNoResponse.AddCell(CellDescription3);

                    // CellLocation
                    CellLocation3.Phrase = new Phrase(Convert.ToString(row["Location"]), TextFontformat);
                    pdfNoResponse.AddCell(CellLocation3);

                    // CellExpectedVolume
                    CellExpectedVolume3.Phrase = new Phrase(Convert.ToString(row["VolumeExpected"]), TextFontformat);
                    pdfNoResponse.AddCell(CellExpectedVolume3);

                    // CellActivityDate
                    CellActivityDate3.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DATE"]), TextFontformat);
                    pdfNoResponse.AddCell(CellActivityDate3);

                    // CellActivityDetails
                    CellActivityDetails3.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DETAILS"]), TextFontformat);
                    pdfNoResponse.AddCell(CellActivityDetails3);

                    if (ddlUser.SelectedValue == "0")
                    {
                        // CellActivityDetails
                        CellPersonName3.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                        pdfNoResponse.AddCell(CellPersonName3);
                    }
                } //end of foreach
                i = 0;
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoResponse);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoResponse);
                pdfDoc.Add(new Paragraph(lblErrMsg4.Text, ColorMsg));
            }

            ///***********************************  Type 5 (No status Updated)  *****************************************/
            dtWeeklySalesWise.Clear();
            string Type5 = lblCurrentStatus5.Text;
            pdfDoc.Add(new Paragraph(Type5, SubHeadFormat));
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise("3", 0, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);

            PdfPTable pdfNoStatus = new PdfPTable(NoOfColumn);   //Create new table

            pdfNoStatus.TotalWidth = 800f;
            pdfNoStatus.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.5f, 0.4f, 1.6f, 0.5f };
                pdfNoStatus.SetWidths(widths);
                pdfNoStatus.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.1f, 0.5f, 0.4f, 0.4f, 0.35f, 1.6f };
                pdfNoStatus.SetWidths(widths);
                pdfNoStatus.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            // Set Table Spacing Before And After html text
            pdfNoStatus.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:SERIAL NUMBER
            PdfPCell cellwithdata40 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            pdfNoStatus.AddCell(cellwithdata40);

            // Header:LEAD NAME
            PdfPCell cellwithdata41 = new PdfPCell(new Phrase("LEAD NAME", GridHeadingFont));
            pdfNoStatus.AddCell(cellwithdata41);

            // Header: LOCATION
            PdfPCell cellwithdata42 = new PdfPCell(new Phrase("LOCATION", GridHeadingFont));
            pdfNoStatus.AddCell(cellwithdata42);

            // Header: EXPECTED VOLUME
            PdfPCell cellwithdata43 = new PdfPCell(new Phrase("EXPECTED VOLUME", GridHeadingFont));
            cellwithdata43.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoStatus.AddCell(cellwithdata43);

            // Header: ACTIVITY DATE
            PdfPCell cellwithdata44 = new PdfPCell(new Phrase("ACTIVITY DATE", GridHeadingFont));
            cellwithdata44.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoStatus.AddCell(cellwithdata44);

            // Header: ACTIVITY DETAILS
            PdfPCell cellwithdata45 = new PdfPCell(new Phrase("ACTIVITY DETAILS", GridHeadingFont));
            cellwithdata45.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoStatus.AddCell(cellwithdata45);

            //CELL ADDED
            //DATA CELL: SERIAL NUMBER
            PdfPCell CellSl4 = new PdfPCell();
            CellSl4.Colspan = 1;
            CellSl4.UseVariableBorders = false;

            //DATA CELL: LEAD NAME
            PdfPCell CellDescription4 = new PdfPCell();
            CellDescription4.Colspan = 1;
            CellDescription4.UseVariableBorders = false;

            // Data Cell: location

            PdfPCell CellLocation4 = new PdfPCell();
            CellLocation4.Colspan = 1;
            CellLocation4.UseVariableBorders = false;

            // Data Cell: expected volume

            PdfPCell CellExpectedVolume4 = new PdfPCell();
            CellExpectedVolume4.Colspan = 1;
            CellExpectedVolume4.UseVariableBorders = false;

            // Data Cell: Activity Date

            PdfPCell CellActivityDate4 = new PdfPCell();
            CellActivityDate4.Colspan = 1;
            CellActivityDate4.UseVariableBorders = false;

            // Data Cell: Activity Details

            PdfPCell CellActivityDetails4 = new PdfPCell();
            CellActivityDetails4.Colspan = 1;
            CellActivityDetails4.UseVariableBorders = false;

            PdfPCell cellwithdata46 = new PdfPCell(new Phrase("SALE PERSON NAME", GridHeadingFont));
            PdfPCell CellPersonName4 = new PdfPCell();

            if (ddlUser.SelectedValue == "0")
            {
                // Header: SALES PERSON NAME
                cellwithdata46.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfNoStatus.AddCell(cellwithdata46);

                // Data Cell: SALES PERSON NAME
                CellPersonName4.Colspan = 1;
                CellPersonName4.UseVariableBorders = false;
            }

            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                foreach (DataRow row in dtWeeklySalesWise.Rows)
                {
                    i = i + 1;
                    //cellSerialNUmber
                    CellSl4.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    pdfNoStatus.AddCell(CellSl4);

                    //cellLeadName
                    CellDescription4.Phrase = new Phrase(Convert.ToString(row["Lead_Name"]), TextFontformat);
                    pdfNoStatus.AddCell(CellDescription4);

                    // CellLocation
                    CellLocation4.Phrase = new Phrase(Convert.ToString(row["Location"]), TextFontformat);
                    pdfNoStatus.AddCell(CellLocation4);

                    // CellExpectedVolume
                    CellExpectedVolume4.Phrase = new Phrase(Convert.ToString(row["VolumeExpected"]), TextFontformat);
                    pdfNoStatus.AddCell(CellExpectedVolume4);

                    // CellActivityDate
                    CellActivityDate4.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DATE"]), TextFontformat);
                    pdfNoStatus.AddCell(CellActivityDate4);

                    // CellActivityDetails
                    CellActivityDetails4.Phrase = new Phrase(Convert.ToString(row["ACTIVITY_DETAILS"]), TextFontformat);
                    pdfNoStatus.AddCell(CellActivityDetails4);

                    if (ddlUser.SelectedValue == "0")
                    {
                        // CellActivityDetails
                        CellPersonName4.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                        pdfNoStatus.AddCell(CellPersonName4);
                    }
                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoStatus);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoStatus);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }
            //-----------------------------------------------------Weekly Visit Summary-----------------------------------------------------------------------------------------
            string Type6 = "Weekly Visit Summary";
            pdfDoc.Add(new Paragraph(Type6, SubHeadFormat));
            DataTable dtVisitSummary = DBOperations.get_CRMWeeklyVisit(dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId, 1);
            PdfPTable pdfVisitSummary = new PdfPTable(5);   //Create new table

            pdfVisitSummary.TotalWidth = 800f;
            pdfVisitSummary.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.5f, 1.0f, 0.5f, 0.5f, 2.0f };
                pdfVisitSummary.SetWidths(widths);
                pdfVisitSummary.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.5f, 1.0f, 0.5f, 0.5f, 2.0f };
                pdfVisitSummary.SetWidths(widths);
                pdfVisitSummary.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfVisitSummary.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:LEAD NAME
            PdfPCell cellwithdata51 = new PdfPCell(new Phrase("Lead Ref No", GridHeadingFont));
            pdfVisitSummary.AddCell(cellwithdata51);

            // Header: company
            PdfPCell cellwithdata52 = new PdfPCell(new Phrase("Company", GridHeadingFont));
            pdfVisitSummary.AddCell(cellwithdata52);

            // Header: visit date
            PdfPCell cellwithdata53 = new PdfPCell(new Phrase("Visit Date", GridHeadingFont));
            cellwithdata43.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVisitSummary.AddCell(cellwithdata53);

            // Header: visit Category
            PdfPCell cellwithdata54 = new PdfPCell(new Phrase("VisitCategory", GridHeadingFont));
            cellwithdata44.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVisitSummary.AddCell(cellwithdata54);

            // Header: Remark
            PdfPCell cellwithdata55 = new PdfPCell(new Phrase("Remark", GridHeadingFont));
            cellwithdata45.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVisitSummary.AddCell(cellwithdata55);

            //CELL ADDED
            //DATA CELL: Lead Ref No
            PdfPCell CellSl5 = new PdfPCell();
            CellSl5.Colspan = 1;
            CellSl5.UseVariableBorders = false;

            //DATA CELL: company
            PdfPCell CellCompany = new PdfPCell();
            CellCompany.Colspan = 1;
            CellCompany.UseVariableBorders = false;

            // Data Cell: Visit date

            PdfPCell CellVisitdate = new PdfPCell();
            CellVisitdate.Colspan = 1;
            CellVisitdate.UseVariableBorders = false;

            // Data Cell: Visit Category

            PdfPCell CellVisitCategory = new PdfPCell();
            CellVisitCategory.Colspan = 1;
            CellVisitCategory.UseVariableBorders = false;

            // Data Cell: Remark

            PdfPCell CellRemark = new PdfPCell();
            CellRemark.Colspan = 1;
            CellRemark.UseVariableBorders = false;

            if (dtVisitSummary.Rows.Count > 0)
            {
                foreach (DataRow row in dtVisitSummary.Rows)
                {
                    i = i + 1;

                    //Lead ref no
                    CellSl5.Phrase = new Phrase(Convert.ToString(row["LeadRefNo"]), TextFontformat);
                    pdfVisitSummary.AddCell(CellSl5);

                    // company
                    CellCompany.Phrase = new Phrase(Convert.ToString(row["Company"]), TextFontformat);
                    pdfVisitSummary.AddCell(CellCompany);

                    // visit date
                    CellVisitdate.Phrase = new Phrase(Convert.ToString(row["VisitDate"]), TextFontformat);
                    pdfVisitSummary.AddCell(CellVisitdate);

                    // visit category
                    CellVisitCategory.Phrase = new Phrase(Convert.ToString(row["CategoryName"]), TextFontformat);
                    pdfVisitSummary.AddCell(CellVisitCategory);

                    // remark
                    CellRemark.Phrase = new Phrase(Convert.ToString(row["Remark"]), TextFontformat);
                    pdfVisitSummary.AddCell(CellRemark);


                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVisitSummary);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVisitSummary);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------No updates last 1 month-----------------------------------------------------------------------------------------
            string Type7 = "No Updates since last one month";
            pdfDoc.Add(new Paragraph(Type7, SubHeadFormat));
            DataTable dtNoUpd1Month = DBOperations.get_CRMPendingEnquiry(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
            PdfPTable pdfNoUpd1Month = new PdfPTable(5);   //Create new table

            pdfNoUpd1Month.TotalWidth = 800f;
            pdfNoUpd1Month.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.5f, 0.5f, 2.5f, 1.0f, 2.0f };
                pdfNoUpd1Month.SetWidths(widths);
                pdfNoUpd1Month.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.5f, 0.5f, 2.5f, 1.0f, 2.0f };
                pdfNoUpd1Month.SetWidths(widths);
                pdfNoUpd1Month.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfNoUpd1Month.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:LEAD NAME
            PdfPCell cellwithdata61 = new PdfPCell(new Phrase("Lead Ref No", GridHeadingFont));
            pdfNoUpd1Month.AddCell(cellwithdata61);

            // Header: QuotationStatus
            PdfPCell cellQuotationStatus = new PdfPCell(new Phrase("Quotation Status", GridHeadingFont));
            pdfNoUpd1Month.AddCell(cellQuotationStatus);

            // Header: CompanyName
            PdfPCell cellCompanyName = new PdfPCell(new Phrase("Company Name", GridHeadingFont));
            cellCompanyName.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoUpd1Month.AddCell(cellCompanyName);

            // Header: Services
            PdfPCell cellServices = new PdfPCell(new Phrase("Services", GridHeadingFont));
            cellServices.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoUpd1Month.AddCell(cellServices);

            // Header: CreatedDate
            PdfPCell cellCreatedDate = new PdfPCell(new Phrase("Created Date", GridHeadingFont));
            cellCreatedDate.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfNoUpd1Month.AddCell(cellCreatedDate);

            //CELL ADDED
            //DATA CELL: Lead Ref No
            PdfPCell CellSl6 = new PdfPCell();
            CellSl6.Colspan = 1;
            CellSl6.UseVariableBorders = false;

            //DATA CELL: QuotationStatus
            PdfPCell CellQuotationStatus = new PdfPCell();
            CellQuotationStatus.Colspan = 1;
            CellQuotationStatus.UseVariableBorders = false;

            // Data Cell: CompanyName

            PdfPCell CellCompanyName = new PdfPCell();
            CellCompanyName.Colspan = 1;
            CellCompanyName.UseVariableBorders = false;

            // Data Cell: Services

            PdfPCell CellServices = new PdfPCell();
            CellServices.Colspan = 1;
            CellServices.UseVariableBorders = false;

            // Data Cell: CreatedDate

            PdfPCell CellCreatedDate = new PdfPCell();
            CellCreatedDate.Colspan = 1;
            CellCreatedDate.UseVariableBorders = false;

            if (dtNoUpd1Month.Rows.Count > 0)
            {
                foreach (DataRow row in dtNoUpd1Month.Rows)
                {
                    i = i + 1;

                    //Lead ref no
                    CellSl5.Phrase = new Phrase(Convert.ToString(row["LeadRefNo"]), TextFontformat);
                    pdfNoUpd1Month.AddCell(CellSl5);

                    // company
                    CellCompany.Phrase = new Phrase(Convert.ToString(row["QuotationStatus"]), TextFontformat);
                    pdfNoUpd1Month.AddCell(CellCompany);

                    // visit date
                    CellVisitdate.Phrase = new Phrase(Convert.ToString(row["CompanyName"]), TextFontformat);
                    pdfNoUpd1Month.AddCell(CellVisitdate);

                    // visit category
                    CellVisitCategory.Phrase = new Phrase(Convert.ToString(row["Services"]), TextFontformat);
                    pdfNoUpd1Month.AddCell(CellVisitCategory);

                    // remark
                    CellRemark.Phrase = new Phrase(Convert.ToString(row["CreatedDate"]), TextFontformat);
                    pdfNoUpd1Month.AddCell(CellRemark);


                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoUpd1Month);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfNoUpd1Month);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------Onboarded Customer-----------------------------------------------------------------------------------------

            string Type8 = "Onboarded Customer";
            pdfDoc.Add(new Paragraph(Type8, SubHeadFormat));
            DataTable dtOnBoardCust = DBOperations.getCRMOnBoardCustomer(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
            PdfPTable pdfOnBoardCust = new PdfPTable(6);   //Create new table

            pdfOnBoardCust.TotalWidth = 800f;
            pdfOnBoardCust.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 0.5f, 1.0f, 1.5f, 0.5f, 0.5f, 0.5f };
                pdfOnBoardCust.SetWidths(widths);
                pdfOnBoardCust.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 0.5f, 1.0f, 1.5f, 0.5f, 0.5f, 0.5f };
                pdfOnBoardCust.SetWidths(widths);
                pdfOnBoardCust.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfOnBoardCust.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:LEAD ref no
            PdfPCell cellwithdata71 = new PdfPCell(new Phrase("Lead Ref No", GridHeadingFont));
            pdfOnBoardCust.AddCell(cellwithdata71);

            // Header: Company
            PdfPCell cellCompany = new PdfPCell(new Phrase("Company", GridHeadingFont));
            pdfOnBoardCust.AddCell(cellCompany);

            // Header: Services
            PdfPCell cellService = new PdfPCell(new Phrase("Services", GridHeadingFont));
            cellService.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfOnBoardCust.AddCell(cellService);

            // Header: Volume Expected
            PdfPCell cellVolumeExpected = new PdfPCell(new Phrase("Volume Expected", GridHeadingFont));
            cellVolumeExpected.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfOnBoardCust.AddCell(cellVolumeExpected);

            // Header: On Board Date
            PdfPCell cellOnBoardDate = new PdfPCell(new Phrase("On Board Date", GridHeadingFont));
            cellOnBoardDate.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfOnBoardCust.AddCell(cellOnBoardDate);

            // Header: KYCDate
            PdfPCell cellKYCDate = new PdfPCell(new Phrase("KYCDate", GridHeadingFont));
            cellKYCDate.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfOnBoardCust.AddCell(cellKYCDate);

            //CELL ADDED
            //DATA CELL: Lead Ref No
            PdfPCell CellSl7 = new PdfPCell();
            CellSl7.Colspan = 1;
            CellSl7.UseVariableBorders = false;

            //DATA CELL: Company
            PdfPCell CellCompany1 = new PdfPCell();
            CellCompany1.Colspan = 1;
            CellCompany1.UseVariableBorders = false;

            //DATA CELL: Services
            PdfPCell CellService = new PdfPCell();
            CellService.Colspan = 1;
            CellService.UseVariableBorders = false;

            // Data Cell: VolumeExpected

            PdfPCell CellVolumeExpected = new PdfPCell();
            CellVolumeExpected.Colspan = 1;
            CellVolumeExpected.UseVariableBorders = false;

            // Data Cell: On Board Date

            PdfPCell CellOnBoardDate = new PdfPCell();
            CellOnBoardDate.Colspan = 1;
            CellOnBoardDate.UseVariableBorders = false;

            // Data Cell: KYCDate

            PdfPCell CellKYCDate = new PdfPCell();
            CellKYCDate.Colspan = 1;
            CellKYCDate.UseVariableBorders = false;

            if (dtOnBoardCust.Rows.Count > 0)
            {
                foreach (DataRow row in dtOnBoardCust.Rows)
                {
                    i = i + 1;

                    //Lead ref no
                    CellSl7.Phrase = new Phrase(Convert.ToString(row["LeadRefNo"]), TextFontformat);
                    pdfOnBoardCust.AddCell(CellSl7);

                    // company
                    CellCompany1.Phrase = new Phrase(Convert.ToString(row["Customer"]), TextFontformat);
                    pdfOnBoardCust.AddCell(CellCompany1);

                    // Services
                    CellService.Phrase = new Phrase(Convert.ToString(row["Services"]), TextFontformat);
                    pdfOnBoardCust.AddCell(CellService);

                    // VolumeExpected
                    //  CellVolumeExpected.Phrase = new Phrase(Convert.ToString(row["CompanyName"]), TextFontformat);
                    CellVolumeExpected.Phrase = new Phrase("", TextFontformat);
                    pdfOnBoardCust.AddCell(CellVolumeExpected);

                    // OnBoardDate
                    CellOnBoardDate.Phrase = new Phrase(Convert.ToString(row["CreatedDate"]), TextFontformat);
                    pdfOnBoardCust.AddCell(CellOnBoardDate);

                    // CellKYCDate
                    CellKYCDate.Phrase = new Phrase(Convert.ToString(row["KYCDate"]), TextFontformat);
                    pdfOnBoardCust.AddCell(CellKYCDate);


                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfOnBoardCust);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfOnBoardCust);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------Freight Summary-----------------------------------------------------------------------------------------
            string Type12 = "Freight Summary";
            pdfDoc.Add(new Paragraph(Type12, SubHeadFormat));
            DataTable dtpendingFRSummary = DBOperations.get_PendingFRSummary(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
            PdfPTable pdfPendingFRSummary = new PdfPTable(8);   //Create new table

            pdfPendingFRSummary.TotalWidth = 800f;
            pdfPendingFRSummary.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 1.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f};
                pdfPendingFRSummary.SetWidths(widths);
                pdfPendingFRSummary.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 1.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfPendingFRSummary.SetWidths(widths);
                pdfPendingFRSummary.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfPendingFRSummary.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:Sr NO
            //PdfPCell cellSr = new PdfPCell(new Phrase("Sl", GridHeadingFont));
            //pdfPendingFRSummary.AddCell(cellSr);

            // Header: Name
            PdfPCell cellName = new PdfPCell(new Phrase("Name", GridHeadingFont));
            pdfPendingFRSummary.AddCell(cellName);

            // Header: Enquiry
            PdfPCell cellEnquiry = new PdfPCell(new Phrase("Enquiry", GridHeadingFont));
            cellEnquiry.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellEnquiry);

            // Header: Quoted
            PdfPCell cellQuoted = new PdfPCell(new Phrase("Quoted", GridHeadingFont));
            cellQuoted.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellQuoted);

            // Header: Awarded
            PdfPCell cellAwarded = new PdfPCell(new Phrase("Awarded", GridHeadingFont));
            cellAwarded.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellAwarded);

            // Header: Lost
            PdfPCell cellLost = new PdfPCell(new Phrase("Lost", GridHeadingFont));
            cellLost.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellLost);

            // Header: Executed
            PdfPCell cellExecuted = new PdfPCell(new Phrase("Executed", GridHeadingFont));
            cellExecuted.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellExecuted);

            // Header: Budgetary
            PdfPCell cellBudgetary = new PdfPCell(new Phrase("Budgetary", GridHeadingFont));
            cellBudgetary.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellBudgetary);

            // Header: Lead
            PdfPCell cellLead = new PdfPCell(new Phrase("Lead", GridHeadingFont));
            cellLead.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPendingFRSummary.AddCell(cellLead);

            //CELL ADDED
            //DATA CELL: Serial No
            //PdfPCell CellSdl = new PdfPCell();
            //CellSdl.Colspan = 1;
            //CellSdl.UseVariableBorders = false;

            //DATA CELL: Sname
            PdfPCell CellSname = new PdfPCell();
            CellSname.Colspan = 1;
            CellSname.UseVariableBorders = false;

            //DATA CELL: Enquiry
            PdfPCell CellEnquiry = new PdfPCell();
            CellEnquiry.Colspan = 1;
            CellEnquiry.UseVariableBorders = false;

            // Data Cell: Quoted
            PdfPCell CellQuoted = new PdfPCell();
            CellQuoted.Colspan = 1;
            CellQuoted.UseVariableBorders = false;

            // Data Cell: Awarded
            PdfPCell CellAwarded = new PdfPCell();
            CellAwarded.Colspan = 1;
            CellAwarded.UseVariableBorders = false;

            // Data Cell: Lost
            PdfPCell CellLost = new PdfPCell();
            CellLost.Colspan = 1;
            CellLost.UseVariableBorders = false;

            // Data Cell: Executed
            PdfPCell CellExecuted = new PdfPCell();
            CellExecuted.Colspan = 1;
            CellExecuted.UseVariableBorders = false;

            // Data Cell: Budgetary
            PdfPCell CellBudgetary = new PdfPCell();
            CellBudgetary.Colspan = 1;
            CellBudgetary.UseVariableBorders = false;

            // Data Cell: Lead
            PdfPCell CellLead = new PdfPCell();
            CellLead.Colspan = 1;
            CellLead.UseVariableBorders = false;

            if (dtpendingFRSummary.Rows.Count > 0)
            {
                foreach (DataRow row in dtpendingFRSummary.Rows)
                {
                    i = i + 1;

                    //Serial No
                    //CellSdl.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                    //pdfPendingFRSummary.AddCell(CellSdl);

                    // Sname
                    CellSname.Phrase = new Phrase(Convert.ToString(row["sName"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellSname);

                    // Enquiry
                    CellEnquiry.Phrase = new Phrase(Convert.ToString(row["Enquiry"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellEnquiry);

                    // Quoted
                    CellQuoted.Phrase = new Phrase(Convert.ToString(row["Quoted"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellQuoted);

                    // Awarded
                    CellAwarded.Phrase = new Phrase(Convert.ToString(row["Awarded"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellAwarded);

                    // Lost
                    CellLost.Phrase = new Phrase(Convert.ToString(row["Lost"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellLost);

                    // Executed
                    CellExecuted.Phrase = new Phrase(Convert.ToString(row["Executed"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellExecuted);

                    // Budgetary
                    CellBudgetary.Phrase = new Phrase(Convert.ToString(row["Budgetary"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellBudgetary);

                    // Lead
                    CellLead.Phrase = new Phrase(Convert.ToString(row["Lead"]), TextFontformat);
                    pdfPendingFRSummary.AddCell(CellLead);


                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfPendingFRSummary);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfPendingFRSummary);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //-----------------------------------------------------Business Volume Analysis-----------------------------------------------------------------------------------------

            string Type9 = "Business Volume Analysis";
            pdfDoc.Add(new Paragraph(Type9, SubHeadFormat));
            DataTable dtVolumeAnalysisImportCHA = DBOperations.getCRMVolumeAnalysisImportCHA(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
            PdfPTable pdfVolumeAnalysisImportCHA = new PdfPTable(10);   //Create new table

            pdfVolumeAnalysisImportCHA.TotalWidth = 800f;
            pdfVolumeAnalysisImportCHA.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfVolumeAnalysisImportCHA.SetWidths(widths);
                pdfVolumeAnalysisImportCHA.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfVolumeAnalysisImportCHA.SetWidths(widths);
                pdfVolumeAnalysisImportCHA.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfVolumeAnalysisImportCHA.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:Customer Name
            PdfPCell cellwithdata81 = new PdfPCell(new Phrase("Customer Name", GridHeadingFont));
            pdfVolumeAnalysisImportCHA.AddCell(cellwithdata81);

            // Header: No of Jobs
            PdfPCell cellNoofJobs = new PdfPCell(new Phrase("No of Jobs", GridHeadingFont));
            pdfVolumeAnalysisImportCHA.AddCell(cellNoofJobs);

            // Header: FCL Jobs
            PdfPCell cellFCLJobs = new PdfPCell(new Phrase("FCL Jobs", GridHeadingFont));
            cellFCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellFCLJobs);

            // Header: LCL Jobs
            PdfPCell cellLCLJobs = new PdfPCell(new Phrase("LCL Jobs", GridHeadingFont));
            cellLCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellLCLJobs);

            // Header: Cont 20
            PdfPCell cellCont20 = new PdfPCell(new Phrase("Cont 20", GridHeadingFont));
            cellCont20.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellCont20);

            // Header: Cont 40
            PdfPCell cellCont40 = new PdfPCell(new Phrase("Cont 40", GridHeadingFont));
            cellCont40.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellCont40);

            // Header: TEU
            PdfPCell cellTEU = new PdfPCell(new Phrase("TEU", GridHeadingFont));
            cellTEU.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellTEU);

            // Header: GrossWeight
            PdfPCell cellGrossWeight = new PdfPCell(new Phrase("Gross WT", GridHeadingFont));
            cellGrossWeight.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellGrossWeight);

            // Header: NoOfPKGS
            PdfPCell cellNoOfPKGS = new PdfPCell(new Phrase("Total Pkg", GridHeadingFont));
            cellNoOfPKGS.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellNoOfPKGS);

            // Header: CreatedBy
            PdfPCell cellCreatedBy = new PdfPCell(new Phrase("Referred By", GridHeadingFont));
            cellCreatedBy.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisImportCHA.AddCell(cellCreatedBy);

            //CELL ADDED
            //DATA CELL: Customer Name
            PdfPCell CellSl8 = new PdfPCell();
            CellSl8.Colspan = 1;
            CellSl8.UseVariableBorders = false;

            //DATA CELL: No of Jobs
            PdfPCell CellNoofJobs = new PdfPCell();
            CellNoofJobs.Colspan = 1;
            CellNoofJobs.UseVariableBorders = false;

            //DATA CELL: FCL Jobs
            PdfPCell cellFCLJobs1 = new PdfPCell();
            cellFCLJobs1.Colspan = 1;
            cellFCLJobs1.UseVariableBorders = false;

            // Data Cell: LCL Jobs

            PdfPCell cellLCLJobs1 = new PdfPCell();
            cellLCLJobs1.Colspan = 1;
            cellLCLJobs1.UseVariableBorders = false;

            // Data Cell: Cont20

            PdfPCell cellsCont20 = new PdfPCell();
            cellsCont20.Colspan = 1;
            cellsCont20.UseVariableBorders = false;

            // Data Cell: Cont40

            PdfPCell cellsCont40 = new PdfPCell();
            cellsCont40.Colspan = 1;
            cellsCont40.UseVariableBorders = false;

            //DATA CELL: TEU
            PdfPCell cellTEU1 = new PdfPCell();
            cellTEU1.Colspan = 1;
            cellTEU1.UseVariableBorders = false;

            // Data Cell: GrossWeight

            PdfPCell cellcellGrossWeight1 = new PdfPCell();
            cellcellGrossWeight1.Colspan = 1;
            cellcellGrossWeight1.UseVariableBorders = false;

            // Data Cell: NoOfPKGS

            PdfPCell cellsNoOfPKGS = new PdfPCell();
            cellsNoOfPKGS.Colspan = 1;
            cellsNoOfPKGS.UseVariableBorders = false;

            // Data Cell: CreatedBy

            PdfPCell cellsCreatedBy = new PdfPCell();
            cellsCreatedBy.Colspan = 1;
            cellsCreatedBy.UseVariableBorders = false;
            if (dtVolumeAnalysisImportCHA.Rows.Count > 0)
            {
                foreach (DataRow row in dtVolumeAnalysisImportCHA.Rows)
                {
                    i = i + 1;

                    //Customer
                    CellSl8.Phrase = new Phrase(Convert.ToString(row["Customer"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(CellSl8);

                    // NoofJobsAir
                    CellNoofJobs.Phrase = new Phrase(Convert.ToString(row["NoofJobsAir"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(CellNoofJobs);

                    // NOOFJOBSFCL
                    cellFCLJobs1.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSFCL"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellFCLJobs1);

                    // NOOFJOBSLCL
                    CellVolumeExpected.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSLCL"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellLCLJobs1);

                    // NOOFCONT20
                    cellsCont20.Phrase = new Phrase(Convert.ToString(row["NOOFCONT20"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellsCont20);

                    // NOOFCONT40
                    cellsCont40.Phrase = new Phrase(Convert.ToString(row["NOOFCONT40"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellsCont40);

                    // TEU
                    cellTEU1.Phrase = new Phrase(Convert.ToString(row["TEU"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellTEU1);

                    // GrossWeight
                    cellcellGrossWeight1.Phrase = new Phrase(Convert.ToString(row["GrossWeight"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellcellGrossWeight1);

                    // NoOfPKGS
                    cellsNoOfPKGS.Phrase = new Phrase(Convert.ToString(row["NoOfPKGS"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellsNoOfPKGS);

                    // CreatedBy
                    cellsCreatedBy.Phrase = new Phrase(Convert.ToString(row["CreatedBy"]), TextFontformat);
                    pdfVolumeAnalysisImportCHA.AddCell(cellsCreatedBy);

                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVolumeAnalysisImportCHA);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVolumeAnalysisImportCHA);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------Export CHA-----------------------------------------------------------------------------------------

            string Type13 = "Export CHA";
            pdfDoc.Add(new Paragraph(Type13, SubHeadFormat));
            DataTable dtVolumeAnalysisExportCHA = DBOperations.getCRMVolumeAnalysisExportCHA(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
            PdfPTable pdfVolumeAnalysisExportCHA = new PdfPTable(10);   //Create new table

            pdfVolumeAnalysisExportCHA.TotalWidth = 800f;
            pdfVolumeAnalysisExportCHA.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfVolumeAnalysisExportCHA.SetWidths(widths);
                pdfVolumeAnalysisExportCHA.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f};
                pdfVolumeAnalysisExportCHA.SetWidths(widths);
                pdfVolumeAnalysisExportCHA.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfVolumeAnalysisExportCHA.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:Customer Name
            PdfPCell cellCustomer = new PdfPCell(new Phrase("Customer Name", GridHeadingFont));
            pdfVolumeAnalysisExportCHA.AddCell(cellCustomer);

            // Header: No of Jobs
            PdfPCell cellNoofJobsAir = new PdfPCell(new Phrase("No of Jobs", GridHeadingFont));
            pdfVolumeAnalysisExportCHA.AddCell(cellNoofJobsAir);

            // Header: FCL Jobs
            PdfPCell cellNOOFJOBSFCL = new PdfPCell(new Phrase("FCL Jobs", GridHeadingFont));
            cellNOOFJOBSFCL.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellNOOFJOBSFCL);

            // Header: LCL Jobs
            PdfPCell cellNOOFJOBSLCL = new PdfPCell(new Phrase("LCL Jobs", GridHeadingFont));
            cellNOOFJOBSLCL.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellNOOFJOBSLCL);

            // Header: Cont 20
            PdfPCell cellNOOFCONT20 = new PdfPCell(new Phrase("Cont 20", GridHeadingFont));
            cellNOOFCONT20.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellNOOFCONT20);

            // Header: Cont 40
            PdfPCell cellNOOFCONT40 = new PdfPCell(new Phrase("Cont 40", GridHeadingFont));
            cellNOOFCONT40.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellNOOFCONT40);

            // Header: TEU
            PdfPCell cellTEU5 = new PdfPCell(new Phrase("TEU", GridHeadingFont));
            cellTEU5.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellTEU5);

            // Header: GrossWeight
            PdfPCell cellGrossWeight1 = new PdfPCell(new Phrase("Gross WT", GridHeadingFont));
            cellGrossWeight1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellGrossWeight1);

            // Header: NoOfPKGS
            PdfPCell cellNoOfPKGS1 = new PdfPCell(new Phrase("Total Pkg", GridHeadingFont));
            cellNoOfPKGS1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellNoOfPKGS1);

            // Header: CreatedBy
            PdfPCell cellCreatedBy1 = new PdfPCell(new Phrase("Referred By", GridHeadingFont));
            cellCreatedBy1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfVolumeAnalysisExportCHA.AddCell(cellCreatedBy1);

            //CELL ADDED
            //DATA CELL: Customer Name
            PdfPCell CellSl12 = new PdfPCell();
            CellSl12.Colspan = 1;
            CellSl12.UseVariableBorders = false;

            //DATA CELL: No of Jobs
            PdfPCell CellNoofJobs1 = new PdfPCell();
            CellNoofJobs1.Colspan = 1;
            CellNoofJobs1.UseVariableBorders = false;

            //DATA CELL: FCL Jobs
            PdfPCell cellFCLJobs11 = new PdfPCell();
            cellFCLJobs11.Colspan = 1;
            cellFCLJobs11.UseVariableBorders = false;

            // Data Cell: LCL Jobs

            PdfPCell cellLCLJobs11 = new PdfPCell();
            cellLCLJobs11.Colspan = 1;
            cellLCLJobs11.UseVariableBorders = false;

            // Data Cell: Cont20

            PdfPCell cellsNOOFCONT20 = new PdfPCell();
            cellsNOOFCONT20.Colspan = 1;
            cellsNOOFCONT20.UseVariableBorders = false;

            // Data Cell: Cont40

            PdfPCell cellsNOOFCONT40 = new PdfPCell();
            cellsNOOFCONT40.Colspan = 1;
            cellsNOOFCONT40.UseVariableBorders = false;

            //DATA CELL: TEU
            PdfPCell cellTEU6 = new PdfPCell();
            cellTEU6.Colspan = 1;
            cellTEU6.UseVariableBorders = false;

            // Data Cell: GrossWeight

            PdfPCell cellGrossWeight3 = new PdfPCell();
            cellGrossWeight3.Colspan = 1;
            cellGrossWeight3.UseVariableBorders = false;

            // Data Cell: NoOfPKGS

            PdfPCell cellsNoOfPKGS1 = new PdfPCell();
            cellsNoOfPKGS1.Colspan = 1;
            cellsNoOfPKGS1.UseVariableBorders = false;

            // Data Cell: CreatedBy

            PdfPCell cellsCreatedBy1 = new PdfPCell();
            cellsCreatedBy1.Colspan = 1;
            cellsCreatedBy1.UseVariableBorders = false;
            if (dtVolumeAnalysisExportCHA.Rows.Count > 0)
            {
                foreach (DataRow row in dtVolumeAnalysisExportCHA.Rows)
                {
                    i = i + 1;

                    //Customer
                    CellSl8.Phrase = new Phrase(Convert.ToString(row["Customer"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(CellSl8);

                    // NoofJobsAir
                    CellNoofJobs.Phrase = new Phrase(Convert.ToString(row["NoofJobsAir"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(CellNoofJobs);

                    // NOOFJOBSFCL
                    cellFCLJobs1.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSFCL"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellFCLJobs1);

                    // NOOFJOBSLCL
                    CellVolumeExpected.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSLCL"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellLCLJobs1);

                    // NOOFCONT20
                    cellsCont20.Phrase = new Phrase(Convert.ToString(row["NOOFCONT20"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellsCont20);

                    // NOOFCONT40
                    cellsCont40.Phrase = new Phrase(Convert.ToString(row["NOOFCONT40"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellsCont40);

                    // TEU
                    cellTEU1.Phrase = new Phrase(Convert.ToString(row["TEU"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellTEU1);

                    // GrossWeight
                    cellcellGrossWeight1.Phrase = new Phrase(Convert.ToString(row["GrossWeight"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellcellGrossWeight1);

                    // NoOfPKGS
                    cellsNoOfPKGS.Phrase = new Phrase(Convert.ToString(row["NoOfPKGS"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellsNoOfPKGS);

                    // CreatedBy
                    cellsCreatedBy.Phrase = new Phrase(Convert.ToString(row["CreatedBy"]), TextFontformat);
                    pdfVolumeAnalysisExportCHA.AddCell(cellsCreatedBy);

                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVolumeAnalysisExportCHA);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfVolumeAnalysisExportCHA);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------Freight Forwarding-----------------------------------------------------------------------------------------

            string Type10 = "Freight Forwarding";
            pdfDoc.Add(new Paragraph(Type10, SubHeadFormat));
            DataTable dtFreightForwarding = DBOperations.getCRMVolumeAnalysis_Freight(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
            PdfPTable pdfFreightForwarding = new PdfPTable(10);   //Create new table

            pdfFreightForwarding.TotalWidth = 800f;
            pdfFreightForwarding.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfFreightForwarding.SetWidths(widths);
                pdfFreightForwarding.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
                pdfFreightForwarding.SetWidths(widths);
                pdfFreightForwarding.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfFreightForwarding.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:Customer Name
            PdfPCell cellwithdata91 = new PdfPCell(new Phrase("Customer Name", GridHeadingFont));
            pdfFreightForwarding.AddCell(cellwithdata91);

            // Header: No of Jobs
            PdfPCell cellNoofJobs1 = new PdfPCell(new Phrase("No of Jobs", GridHeadingFont));
            pdfFreightForwarding.AddCell(cellNoofJobs1);

            // Header: FCL Jobs
            PdfPCell cellFCLJobs2 = new PdfPCell(new Phrase("FCL Jobs", GridHeadingFont));
            cellFCLJobs2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellFCLJobs2);

            // Header: LCL Jobs
            PdfPCell cellLCLJobs2 = new PdfPCell(new Phrase("LCL Jobs", GridHeadingFont));
            cellLCLJobs2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellLCLJobs2);

            // Header: Cont 20
            PdfPCell cellCont202 = new PdfPCell(new Phrase("Cont 20", GridHeadingFont));
            cellCont202.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellCont202);

            // Header: Cont 40
            PdfPCell cellCont402 = new PdfPCell(new Phrase("Cont 40", GridHeadingFont));
            cellCont402.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellCont402);

            // Header: TEU
            PdfPCell cellTEU2 = new PdfPCell(new Phrase("TEU", GridHeadingFont));
            cellTEU2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellTEU2);

            // Header: GrossWeight
            PdfPCell cellGrossWeight2 = new PdfPCell(new Phrase("Gross WT", GridHeadingFont));
            cellGrossWeight2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellGrossWeight2);

            // Header: NoOfPKGS
            PdfPCell cellNoOfPKGS2 = new PdfPCell(new Phrase("Total Pkg", GridHeadingFont));
            cellNoOfPKGS2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellNoOfPKGS2);

            // Header: CreatedBy
            PdfPCell cellCreatedBy2 = new PdfPCell(new Phrase("Referred By", GridHeadingFont));
            cellCreatedBy2.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfFreightForwarding.AddCell(cellCreatedBy2);

            //CELL ADDED
            //DATA CELL: Customer Name
            PdfPCell CellSl9 = new PdfPCell();
            CellSl9.Colspan = 1;
            CellSl9.UseVariableBorders = false;

            //DATA CELL: No of Jobs
            PdfPCell CellNoofJobs2 = new PdfPCell();
            CellNoofJobs2.Colspan = 1;
            CellNoofJobs2.UseVariableBorders = false;

            //DATA CELL: FCL Jobs
            PdfPCell cellFCLJobs12 = new PdfPCell();
            cellFCLJobs12.Colspan = 1;
            cellFCLJobs12.UseVariableBorders = false;

            // Data Cell: LCL Jobs

            PdfPCell cellLCLJobs12 = new PdfPCell();
            cellLCLJobs12.Colspan = 1;
            cellLCLJobs12.UseVariableBorders = false;

            // Data Cell: Cont20

            PdfPCell cellsCont202 = new PdfPCell();
            cellsCont202.Colspan = 1;
            cellsCont202.UseVariableBorders = false;

            // Data Cell: Cont40

            PdfPCell cellsCont402 = new PdfPCell();
            cellsCont402.Colspan = 1;
            cellsCont402.UseVariableBorders = false;

            //DATA CELL: TEU
            PdfPCell cellTEU12 = new PdfPCell();
            cellTEU12.Colspan = 1;
            cellTEU12.UseVariableBorders = false;

            // Data Cell: GrossWeight

            PdfPCell cellcellGrossWeight12 = new PdfPCell();
            cellcellGrossWeight12.Colspan = 1;
            cellcellGrossWeight12.UseVariableBorders = false;

            // Data Cell: NoOfPKGS

            PdfPCell cellsNoOfPKGS2 = new PdfPCell();
            cellsNoOfPKGS2.Colspan = 1;
            cellsNoOfPKGS2.UseVariableBorders = false;

            // Data Cell: CreatedBy

            PdfPCell cellsCreatedBy2 = new PdfPCell();
            cellsCreatedBy2.Colspan = 1;
            cellsCreatedBy2.UseVariableBorders = false;
            if (dtFreightForwarding.Rows.Count > 0)
            {
                foreach (DataRow row in dtFreightForwarding.Rows)
                {
                    i = i + 1;

                    //Customer
                    CellSl8.Phrase = new Phrase(Convert.ToString(row["Customer"]), TextFontformat);
                    pdfFreightForwarding.AddCell(CellSl8);

                    // NoofJobsAir
                    CellNoofJobs.Phrase = new Phrase(Convert.ToString(row["NoofJobsAir"]), TextFontformat);
                    pdfFreightForwarding.AddCell(CellNoofJobs);

                    // NOOFJOBSFCL
                    cellFCLJobs1.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSFCL"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellFCLJobs1);

                    // NOOFJOBSLCL
                    CellVolumeExpected.Phrase = new Phrase(Convert.ToString(row["NOOFJOBSLCL"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellLCLJobs1);

                    // NOOFCONT20
                    cellsCont20.Phrase = new Phrase(Convert.ToString(row["NOOFCONT20"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellsCont20);

                    // NOOFCONT40
                    cellsCont40.Phrase = new Phrase(Convert.ToString(row["NOOFCONT40"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellsCont40);

                    // TEU
                    cellTEU1.Phrase = new Phrase(Convert.ToString(row["TEU"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellTEU1);

                    // GrossWeight
                    cellcellGrossWeight1.Phrase = new Phrase(Convert.ToString(row["GrossWeight"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellcellGrossWeight1);

                    // NoOfPKGS
                    cellsNoOfPKGS.Phrase = new Phrase(Convert.ToString(row["NoOfPKGS"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellsNoOfPKGS);

                    // CreatedBy
                    cellsCreatedBy.Phrase = new Phrase(Convert.ToString(row["CreatedBy"]), TextFontformat);
                    pdfFreightForwarding.AddCell(cellsCreatedBy);

                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfFreightForwarding);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfFreightForwarding);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //-----------------------------------------------------Loss Business-----------------------------------------------------------------------------------------

            string Type11 = "Loss Business – No Operation since last 2 months";
            pdfDoc.Add(new Paragraph(Type11, SubHeadFormat));
            DataTable dtpendingEnquiryLast2Month = DBOperations.get_CRMPendingEnquiryLast2Month(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
            PdfPTable pdfpendingEnquiryLast2Month = new PdfPTable(6);   //Create new table

            pdfpendingEnquiryLast2Month.TotalWidth = 800f;
            pdfpendingEnquiryLast2Month.LockedWidth = true;
            if (ddlUser.SelectedValue == "0")
            {
                float[] widths = new float[] { 2.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.0f };
                pdfpendingEnquiryLast2Month.SetWidths(widths);
                pdfpendingEnquiryLast2Month.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                float[] widths = new float[] { 2.5f, 1.5f, 1.0f, 1.0f, 1.0f, 1.0f };
                pdfpendingEnquiryLast2Month.SetWidths(widths);
                pdfpendingEnquiryLast2Month.HorizontalAlignment = Element.ALIGN_LEFT;
            }

            // Set Table Spacing Before And After html text
            pdfpendingEnquiryLast2Month.SpacingAfter = 8f;

            // Create Table Column Header Cell with Text
            // Header:Customer Name
            PdfPCell cellwithdata101 = new PdfPCell(new Phrase("CompanyName", GridHeadingFont));
            pdfpendingEnquiryLast2Month.AddCell(cellwithdata91);

            // Header: MonthOfLastOperation
            PdfPCell cellMonthOfLastOperation = new PdfPCell(new Phrase("Month Of Last Operation", GridHeadingFont));
            pdfpendingEnquiryLast2Month.AddCell(cellMonthOfLastOperation);

            // Header: Services
            PdfPCell cellServices1 = new PdfPCell(new Phrase("Module", GridHeadingFont));
            cellFCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfpendingEnquiryLast2Month.AddCell(cellServices1);

            // Header: Package
            PdfPCell cellPackage = new PdfPCell(new Phrase("Package", GridHeadingFont));
            cellFCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfpendingEnquiryLast2Month.AddCell(cellPackage);

            // Header: TEU
            PdfPCell cellTEU3 = new PdfPCell(new Phrase("TEU", GridHeadingFont));
            cellFCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfpendingEnquiryLast2Month.AddCell(cellTEU3);

            // Header: Gross Weight
            PdfPCell cellGrossWt = new PdfPCell(new Phrase("GROSSWT", GridHeadingFont));
            cellFCLJobs.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfpendingEnquiryLast2Month.AddCell(cellGrossWt);

            //CELL ADDED
            //DATA CELL: Customer Name
            PdfPCell CellSl101 = new PdfPCell();
            CellSl101.Colspan = 1;
            CellSl101.UseVariableBorders = false;

            //DATA CELL: No of Jobs
            PdfPCell cellMonthOfLastOperation1 = new PdfPCell();
            cellMonthOfLastOperation1.Colspan = 1;
            cellMonthOfLastOperation1.UseVariableBorders = false;

            //DATA CELL: Services
            PdfPCell cellServices2 = new PdfPCell();
            cellServices2.Colspan = 1;
            cellServices2.UseVariableBorders = false;

            //DATA CELL: Package
            PdfPCell cellPackage1 = new PdfPCell();
            cellPackage1.Colspan = 1;
            cellPackage1.UseVariableBorders = false;

            //DATA CELL: TEU
            PdfPCell cellTEU4 = new PdfPCell();
            cellTEU4.Colspan = 1;
            cellTEU4.UseVariableBorders = false;

            //DATA CELL: Gross Weight
            PdfPCell cellGrossWt1 = new PdfPCell();
            cellGrossWt1.Colspan = 1;
            cellGrossWt1.UseVariableBorders = false;

            if (dtpendingEnquiryLast2Month.Rows.Count > 0)
            {
                foreach (DataRow row in dtpendingEnquiryLast2Month.Rows)
                {
                    i = i + 1;

                    //CompanyName
                    CellSl101.Phrase = new Phrase(Convert.ToString(row["CUSTNAME"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(CellSl101);

                    // MonthOfLastOperation
                    cellMonthOfLastOperation1.Phrase = new Phrase(Convert.ToString(row["MonthOfLastOperation"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(cellMonthOfLastOperation1);

                    // Services
                    cellServices2.Phrase = new Phrase(Convert.ToString(row["ModuleType"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(cellServices2);

                    // Package
                    cellPackage1.Phrase = new Phrase(Convert.ToString(row["Package"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(cellPackage1);

                    // TEU
                    cellTEU4.Phrase = new Phrase(Convert.ToString(row["TEU"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(cellTEU4);

                    //  Gross Weight
                    cellGrossWt1.Phrase = new Phrase(Convert.ToString(row["GROSSWT"]), TextFontformat);
                    pdfpendingEnquiryLast2Month.AddCell(cellGrossWt1);
                } //end of foreach
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfpendingEnquiryLast2Month);
            }
            else
            {
                pdfDoc.Add(ParaSpace);
                pdfDoc.Add(pdfpendingEnquiryLast2Month);
                pdfDoc.Add(new Paragraph("No Record Found", ColorMsg));
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
            pdfDoc.Add(ParaSpace);

            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            //***************************************************************************For sending mail function

            //SendEmail("WeeklySalesReport\\" + filePath + ".pdf",ReportName);
            //***************************************************************************
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Successfully Downloaded ');", true);
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void Repeater_rptWeeklySaleWise5_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell cellHead5 = (HtmlTableCell)e.Item.FindControl("lblUName5");
            if (cellHead5 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellHead5.Visible = true;
                }
                else
                {
                    cellHead5.Visible = false;
                }

            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlTableCell cellDetail5 = (HtmlTableCell)e.Item.FindControl("lblName5");
            if (cellDetail5 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellDetail5.Visible = true;
                }
                else
                {
                    cellDetail5.Visible = false;
                }

            }
        }
    }

    protected void Repeater_rptWeeklySaleWise4_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            HtmlTableCell cellHead4 = (HtmlTableCell)e.Item.FindControl("lblUName4");
            if (cellHead4 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellHead4.Visible = true;
                }
                else
                {
                    cellHead4.Visible = false;
                }

            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HtmlTableCell cellDetail4 = (HtmlTableCell)e.Item.FindControl("lblName4");
            if (cellDetail4 != null)
            {
                if (ddlUser.SelectedValue == "0")
                {
                    cellDetail4.Visible = true;
                }
                else
                {
                    cellDetail4.Visible = false;
                }

            }
        }
    }

    protected void Repeater_rptWeeklySaleWise1_ItemCreated(object sender, RepeaterItemEventArgs e)
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

    protected void Repeater_rptWeeklySaleWise2_ItemCreated(object sender, RepeaterItemEventArgs e)
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

    protected void Repeater_rptWeeklySaleWise3_ItemCreated(object sender, RepeaterItemEventArgs e)
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

    public string GetFileName(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\WeeklySalesReport\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "WeeklySalesReport\\" + FileName;

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

    public string GetPath(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\WeeklySalesReport\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "WeeklySalesReport\\" + FileName;

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

    #region EMAIL 
    private bool SendEmail(String FilePath, string FileName)
    {
        string MessageBody = "", strToEmail = "", strCCEmail = "", strSubject = "";
        StringBuilder strStyle = new StringBuilder();
        bool bEmailSucces = false;

        strToEmail = "developer2@babajishivram.com";//"javed.shaikh@BabajiShivram.com";
        //strCCEmail = "developer1@babajishivram.com";
        strSubject = ddlUser.SelectedItem + "_WEEKLY CRM REPORT_" + txtStartDate.Text + "To" + txtEndDate.Text;

        if (strToEmail == "" || strSubject == "")
            return bEmailSucces;
        else
        {
            //strStyle = strStyle.Append("<html><body class='setupTab' style='font-family:Calibri; font-style:normal; bEditID: b1st1; bLabel: body;'><left>");

            //// email css
            //strStyle = strStyle.Append(@"<style type='text/css'> p {margin-top: 0px; margin-bottom: 0px;} font {color: black;}");
            //strStyle = strStyle.Append(@".topTr {font-family:Calibri; font-style:normal;border: 2px solid darkgray;border-bottom: none;bEditID: r3st1;color: white;bLabel: main;font-size: 12pt;font-family: calibri;height: 35px;background-color: #268CD8;}");
            //strStyle = strStyle.Append(@".MeetingTitle {font-family:Calibri; font-style:normal;font-size: 19px;text-align: right;font-weight: 600;color: #268CD8;} .MeetingDate {font-size: 13px;text-align: right;color: black;font-weight: 400;} ");
            //strStyle = strStyle.Append(@".MOMHdr {font-family:Calibri; font-style:normal; color:black; padding:4px;} .csTitle {font-family:Calibri; font-style:normal;color: #268CD8;} .AgendaHdr {padding: 7px;background-color: #268CD8;color: white;} ");
            //strStyle = strStyle.Append(@".AgendaHdr {font-family:Calibri; font-style:normal;padding: 7px;background - color: #268CD8;color: white;font - weight: 500;}");
            //strStyle = strStyle.Append(@".AgendaBody {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;}");
            //strStyle = strStyle.Append(@".AgendaBodySL {font-family:Calibri; font-style:normal;padding-left: 7px;padding-right: 7px;color: black;font - weight: 400;text - align: center;}");
            //strStyle = strStyle.Append(@"</style>");

            //// body header
            //strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
            //strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
            //strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");
            ////strStyle = strStyle.Append(@"</span></td></tr><tr><td styleInsert='1' style='border-bottom:1px solid #268CD8'></td></tr>");

            //// body middle portion
            //strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
            //strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
            //strStyle = strStyle.Append(@"<p style='color: rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
            //strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='99%'><colgroup><col width='50%' /><col width='50%' /></colgroup>");
            //strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'></b>&nbsp;" + "Dear Sir,<BR><BR></b>&nbsp;</b>&nbsp;</b>&nbsp;</b>&nbsp;</b>&nbsp;</b>&nbsp;</b>&nbsp;</b>&nbsp;Please find attachment for weekly sales report.<BR><BR>" + "</td></tr>");
            //strStyle = strStyle.Append(@"<tr><td class='MOMHdr'><b class='csTitle'></b>&nbsp;" + "<BR><BR>Thanks & Regards,<BR>" + "</td><td style='color: black; padding: 4px'>");

            //strStyle = strStyle.Append(@"</table></p></div></div></td>");
            //// body footer
            //strStyle = strStyle.Append(@"</table></td></tr><tr><td styleInsert='1' height='80' class='topTr' style=''>");
            //strStyle = strStyle.Append(@"<p style='text-align: center; font-weight: 500;font-size: 12pt; padding: 15px;'>");
            //strStyle = strStyle.Append(@"<font style='color:white; padding-top: 10px'><b> &nbsp; &nbsp; Note*: </b>This is system generated mail, please do not reply to this message via e-mail.");
            //strStyle = strStyle.Append(@"</font></p></td></tr></table>");
            //strStyle = strStyle.Append(@"</left></body></html>");

            MessageBody = "Dear Sir,<BR><BR>Please find attachment for weekly CRM report on " + txtStartDate.Text + " to " + txtEndDate.Text + ". <BR><BR>";

            MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>";

            MessageBody = MessageBody + "<b></b>This is system generated mail, please do not reply to this message via e-mail.";

        }
        List<string> lstFilePath = new List<string>();
        MessageBody = strStyle.ToString();
        lstFilePath.Add("WeeklySalesReport\\" + FilePath + ".pdf");
        bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strToEmail, strCCEmail, strSubject, MessageBody, lstFilePath);//LoggedInUser.glUserName
        return bEmailSucces;
    }
    #endregion



    public void FindRecordExist()
    {
        Action_Perform = "Total";
        DataTable dt2 = new DataTable();
        dt2 = DBOperations.CRM_GetWeeklySalesWise(Action_Perform, 0, dtStartDate, dtEndDate, Convert.ToInt32(User), LoggedInUser.glFinYearId);
        if (dt2.Rows.Count > 0)
        {
            //ExportToPDF();
        }
        else
        {
            //no record for these sale person
        }
    }
   
    public void BindVisitData()
    {
        DataTable dtVisit = DBOperations.get_CRMWeeklyVisit(dtStartDate, dtEndDate, UserId, LoggedInUser.glFinYearId, 1);
        if (dtVisit.Rows.Count > 0)
        {
            Repeater_rptWeeklySaleWise6.DataSource = dtVisit;
            Repeater_rptWeeklySaleWise6.DataBind();
            Repeater_rptWeeklySaleWise6.Visible = true;
        }
        else
        {
            Repeater_rptWeeklySaleWise6.DataSource = "";
            Repeater_rptWeeklySaleWise6.DataBind();
            Repeater_rptWeeklySaleWise6.Visible = false;
            lblCurrentStatus6.Text = "No Record found ";
            lblCurrentStatus6.Visible = true;
        }
    }

    public void BindPendingEnquiry()
    {
        DataTable dtpendingEnquiry = DBOperations.get_CRMPendingEnquiry(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
        if(dtpendingEnquiry.Rows.Count>0)
        {
            Repeater_rptWeeklySaleWise7.DataSource = dtpendingEnquiry;
            Repeater_rptWeeklySaleWise7.DataBind();
        }
        else
        {
            Repeater_rptWeeklySaleWise7.DataSource = "";
            Repeater_rptWeeklySaleWise7.DataBind();
            Repeater_rptWeeklySaleWise7.Visible = false;
            lblCurrentStatus7.Text = "No Record found ";
            lblCurrentStatus7.Visible = true;
        }
    }

    public void BindOnBoardCust()
    {
        DataTable dtOnBoardCust = DBOperations.getCRMOnBoardCustomer(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
        if(dtOnBoardCust.Rows.Count>0)
        {
            gvCustomerOnBoard.DataSource = dtOnBoardCust;
            gvCustomerOnBoard.DataBind();
        }
        else
        {
            gvCustomerOnBoard.DataSource = "";
            gvCustomerOnBoard.DataBind();
            gvCustomerOnBoard.Visible = false;
            lblCurrentStatus8.Text = "No Record found ";
            lblCurrentStatus8.Visible = true;
        }
    }

    public void BindVolumeAnalysisImportCHA()
    {
        DataTable dtVolumeAnalysisImportCHA = DBOperations.getCRMVolumeAnalysisImportCHA(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
        if (dtVolumeAnalysisImportCHA.Rows.Count > 0)
        {
            gvImport.DataSource = dtVolumeAnalysisImportCHA;
            gvImport.DataBind();
        }
    }

    public void BindVolumeAnalysisExportCHA()
    {
        DataTable dtVolumeAnalysisExportCHA = DBOperations.getCRMVolumeAnalysisExportCHA(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
        if (dtVolumeAnalysisExportCHA.Rows.Count > 0)
        {
            gvExport.DataSource = dtVolumeAnalysisExportCHA;
            gvExport.DataBind();
            gvExport.Visible = true;
        }
        else
        {
            gvExport.Visible = false;
            lblCurrentStatus12.Text = "No Record found ";
            lblCurrentStatus12.Visible = true;
        }
    }

    //public void BindCRMVolumeAnalysis_Freight()
    //{
    //    DataTable dtCRMVolumeAnalysis_Freight = DBOperations.getCRMVolumeAnalysis_Freight(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
    //    if (dtCRMVolumeAnalysis_Freight.Rows.Count > 0)
    //    {
    //        gvFreight.DataSource = dtCRMVolumeAnalysis_Freight;
    //        gvFreight.DataBind();
    //    }
    //    else
    //    {
    //        gvFreight.DataSource = "";
    //        gvFreight.DataBind();
    //        gvFreight.Visible = false;
    //        lblCurrentStatus10.Text = "No Record found ";
    //        lblCurrentStatus10.Visible = true;
    //    }
    //}

    public void BindCRMVolumeAnalysis_Freight()
    {
        DataTable dtCRMVolumeAnalysis_Freight = DBOperations.getCRMVolumeAnalysis_Freight(LoggedInUser.glFinYearId, dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue));
        if (dtCRMVolumeAnalysis_Freight.Rows.Count > 0)
        {
            gvFreight.DataSource = dtCRMVolumeAnalysis_Freight;
            gvFreight.DataBind();
        }
        else
        {
            gvFreight.Visible = false;
            lblCurrentStatus10.Text = "No Record found ";
            lblCurrentStatus10.Visible = true;
        }
    }

    public void BindGetPendingEnquiryLast2Month()
    {
        DataTable dtpendingEnquiryLast2Month = DBOperations.get_CRMPendingEnquiryLast2Month(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
        if (dtpendingEnquiryLast2Month.Rows.Count > 0)
        {
            Repeater_rptWeeklySaleWise8.DataSource = dtpendingEnquiryLast2Month;
            Repeater_rptWeeklySaleWise8.DataBind();
        }
        else
        {
            Repeater_rptWeeklySaleWise8.DataSource = "";
            Repeater_rptWeeklySaleWise8.DataBind();
            Repeater_rptWeeklySaleWise8.Visible = false;
            lblCurrentStatus11.Text = "No Record found ";
            lblCurrentStatus11.Visible = true;
        }
    }

    //public void BindGetPendingFRSummary()
    //{
    //    DataTable dtpendingFRSummary = DBOperations.get_PendingFRSummary(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
    //    if (dtpendingFRSummary.Rows.Count > 0)
    //    {
    //        gvSummaryFreight.DataSource = dtpendingFRSummary;
    //        gvSummaryFreight.DataBind();
    //    }
    //    else
    //    {
    //        //gvSummaryFreight.DataSource = "";
    //        gvSummaryFreight.DataBind();
    //        gvSummaryFreight.Visible = false;
    //        lblCurrentStatus13.Text = "No Record found ";
    //        lblCurrentStatus13.Visible = true;
    //    }
    //}

    public void BindGetPendingFRSummary()
    {
        DataTable dtpendingFRSummary = DBOperations.get_PendingFRSummary(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
        if (dtpendingFRSummary.Rows.Count > 0)
        {
            gvSummaryFreight.DataSource = dtpendingFRSummary;
            gvSummaryFreight.DataBind();
        }
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            Repeater_rptWeeklySaleWise1.DataBind();
            Repeater_rptWeeklySaleWise2.DataBind();
            Repeater_rptWeeklySaleWise3.DataBind();
            Repeater_rptWeeklySaleWise4.DataBind();
            Repeater_rptWeeklySaleWise5.DataBind();
            Repeater_rptWeeklySaleWise6.DataBind();
            Repeater_rptWeeklySaleWise7.DataBind();
            gvCustomerOnBoard.DataBind();
            gvSummaryFreight.DataBind();
            gvImport.DataBind();
            gvExport.DataBind();
            gvFreight.DataBind();
            Repeater_rptWeeklySaleWise8.DataBind();

            #region Status
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise("2", 1, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtWeeklySalesWise, "Status");
            }
            DataTable dtWeeklySalesWise1 = new DataTable();
            dtWeeklySalesWise1 = DBOperations.CRM_GetWeeklySalesWise("2", 2, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            if (dtWeeklySalesWise1.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtWeeklySalesWise1, "Status1");
            }
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise("2", 3, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtWeeklySalesWise, "Status2");
            }
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise("2", 4, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtWeeklySalesWise, "Status3");
            }
            dtWeeklySalesWise = DBOperations.CRM_GetWeeklySalesWise("3", 0, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId);
            if (dtWeeklySalesWise.Rows.Count > 0)
            {
                //wb.Worksheets.Add(dtWeeklySalesWise1);
                wb.Worksheets.Add(dtWeeklySalesWise, "Status4");
            }
            #endregion

            #region VisitSummary
            DataTable dtVisitSummary = DBOperations.get_CRMWeeklyVisit(dtStartDate, dtEndDate, Convert.ToInt32(ddlUser.SelectedValue), LoggedInUser.glFinYearId, 1);
            if (dtVisitSummary.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtVisitSummary, "Visit Summary");
            }
            #endregion

            #region NoUpd1Mont
            DataTable dtNoUpd1Month = DBOperations.get_CRMPendingEnquiry(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
            if (dtNoUpd1Month.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtNoUpd1Month, "No update 1 month");
            }
            #endregion

            #region Customer onboard
            if (gvCustomerOnBoard.Rows.Count > 1)
            {
                DataTable dtCustomerOnBoard = new DataTable("Customer Onboard");
                foreach (TableCell cell in gvCustomerOnBoard.HeaderRow.Cells)
                {
                    dtCustomerOnBoard.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvCustomerOnBoard.Rows)
                {
                    dtCustomerOnBoard.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtCustomerOnBoard.Rows[dtCustomerOnBoard.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtCustomerOnBoard.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtCustomerOnBoard);
            }
            #endregion

            #region Summary Freight
            if (gvSummaryFreight.Rows.Count > 1)
            {
                DataTable dtSummaryFreight = new DataTable("Freight Summary");
                foreach (TableCell cell in gvSummaryFreight.HeaderRow.Cells)
                {
                    dtSummaryFreight.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvSummaryFreight.Rows)
                {
                    dtSummaryFreight.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtSummaryFreight.Rows[dtSummaryFreight.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtSummaryFreight.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtSummaryFreight);
            }
            #endregion

            #region gvImport
            if (gvImport.Rows.Count > 1)
            {
                DataTable dtImport = new DataTable("Import CHA");
                foreach (TableCell cell in gvImport.HeaderRow.Cells)
                {
                    dtImport.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvImport.Rows)
                {
                    dtImport.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtImport.Rows[dtImport.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtImport.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtImport);
            }
            #endregion

            #region gvExport
            if (gvExport.Rows.Count > 1)
            {
                DataTable dtExport = new DataTable("Export CHA");
                foreach (TableCell cell in gvExport.HeaderRow.Cells)
                {
                    dtExport.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvExport.Rows)
                {
                    dtExport.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtExport.Rows[dtExport.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtExport.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtExport);
            }
            #endregion

            #region gvFreight
            if (gvFreight.Rows.Count > 1)
            {
                DataTable dtFreight = new DataTable("Freight");
                foreach (TableCell cell in gvImport.HeaderRow.Cells)
                {
                    dtFreight.Columns.Add(cell.Text);
                }

                //Loop and add rows from GridView to DataTable.
                foreach (GridViewRow row in gvFreight.Rows)
                {
                    dtFreight.Rows.Add();
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        dtFreight.Rows[dtFreight.Rows.Count - 1][j] = row.Cells[j].Text;
                    }
                }
                dtFreight.Columns.RemoveAt(0);
                wb.Worksheets.Add(dtFreight);
            }
            #endregion

            #region Loss data
            DataTable dtpendingEnquiryLast2Month = DBOperations.get_CRMPendingEnquiryLast2Month(Convert.ToInt32(ddlUser.SelectedValue), dtStartDate, dtEndDate, LoggedInUser.glFinYearId);
            if (dtpendingEnquiryLast2Month.Rows.Count > 0)
            {
                wb.Worksheets.Add(dtpendingEnquiryLast2Month, "Loss Data");
            }
            #endregion

            //Export the Excel file.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        //string path =  "abc" + ".xlsx";
        //System.IO.FileInfo file = new System.IO.FileInfo(path);
        //string Outgoingfile = "abc" + ".xlsx";
        //if (file.Exists)
        //{
        //    Response.Clear();
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Outgoingfile);
        //    Response.AddHeader("Content-Length", file.Length.ToString());
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.WriteFile(file.FullName);

        //}
        //else
        //{
        //    Response.Write("This file does not exist.");
        //}

    }

}

