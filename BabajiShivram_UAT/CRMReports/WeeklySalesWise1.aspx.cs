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
    string value = ""; int User = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnExportToPDF);

        if (!IsPostBack)
        {
            lblErrMsg1.Text = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Weekly Sales Wise";
            lblSalePerson.Text = "All";
            txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            dtStartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            dtEndDate = Commonfunctions.CDateTime(txtEndDate.Text.Trim());

            lblCurrentStatus5.Text = "No Status";
            value = LoggedInUser.glUserId.ToString();
            ddlUser.SelectedValue = value;
        }
        //ddlUser.SelectedValue = value;
        //if (LoggedInUser.glUserId.ToString() != "0")
        //{

        //}
        Bind_Data();
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
                pdfDoc.Add(new Paragraph(lblErrMsg5.Text, ColorMsg));
            }


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


}

