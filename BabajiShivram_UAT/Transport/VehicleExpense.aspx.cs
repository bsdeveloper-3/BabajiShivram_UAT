using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
public partial class Transport_VehicleExpense : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnPrintVoucher);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Transport Maintenance Expense";
        //
        if (!IsPostBack)
        {
            Session["TrMaintainId"] = null;

            if (gvMaintenance.Rows.Count == 0)
            {
                lblError.Text = "No Records Found !";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = SqlDataSourceExp;
        DataFilter1.DataColumns = gvMaintenance.Columns;
        DataFilter1.FilterSessionID = "VehicleExpense.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
    protected void btnNewWork_Click(object sender, EventArgs e)
    {
        Response.Redirect("VehicleMaintenance.aspx");
    }
    protected void gvMaintenance_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Description = DataBinder.Eval(e.Row.DataItem, "ShortExpenseDesc").ToString();

            //if (Description.Length < 50)
            //{ 
            //   LinkButton lnkMore = (LinkButton) e.Row.FindControl("lnkMore");
               
            //   if(lnkMore != null)
            //    lnkMore.Visible = false;
            //}

            // Disable Edit For Approved Expense
                        
            bool IsApproved = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsApproved"));

            if (LoggedInUser.glUserId != 1)
            {
                if (IsApproved == true)
                {
                    LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                    if (lnkEdit != null)
                        lnkEdit.Visible = false;
                }
            }
        }

         // if enter key is pressed (keycode==13) call __doPostBack on grid and with
            // 1st param = gvChild.UniqueID (Gridviews UniqueID)
            // 2nd param = CommandName=Update$  +  CommandArgument=RowIndex
            if ((e.Row.RowState == DataControlRowState.Edit) ||
               (e.Row.RowState == (DataControlRowState.Edit|DataControlRowState.Alternate)))
            {
                e.Row.Attributes.Add("onkeypress", "javascript:if (event.keyCode == 13) {__doPostBack('" + gvMaintenance.UniqueID + "', 'Update$" +  e.Row.RowIndex.ToString() + "'); return false; }");
            }
    }
    protected void gvMaintenance_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvMaintenance.EditIndex = e.NewEditIndex;
    }
    protected void gvMaintenance_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            DataKey dataKey = gvMaintenance.DataKeys[e.RowIndex];

            string strMaintainId = dataKey[0].ToString();
            string strExpenseId  = dataKey[1].ToString();

            DropDownList ddCategory = (DropDownList)gvMaintenance.Rows[e.RowIndex].FindControl("ddCategory");
            DropDownList ddPayType  = (DropDownList)gvMaintenance.Rows[e.RowIndex].FindControl("ddPayType");

            TextBox txtExpenseDisc  = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtExpenseDisc");
            TextBox txtAmount       = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtAmount");
            TextBox txtBillNo       = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtBillNo");
            TextBox txtPaidTo       = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtPaidTo");
            TextBox txtSupportBillPaidTo = (TextBox)gvMaintenance.Rows[e.RowIndex].FindControl("txtSupportBillPaidTo");
            
            if (ddCategory.SelectedValue != "0" && txtExpenseDisc.Text.Trim() != "" && txtAmount.Text.Trim() != "")
            {
                SqlDataSourceExp.UpdateParameters["MaintanceId"].DefaultValue       =   strMaintainId;
                SqlDataSourceExp.UpdateParameters["ExpenseId"].DefaultValue         =   strExpenseId;
                SqlDataSourceExp.UpdateParameters["Amount"].DefaultValue            =   txtAmount.Text.Trim();
                SqlDataSourceExp.UpdateParameters["ExpenseDesc"].DefaultValue       =   txtExpenseDisc.Text.Trim();
                SqlDataSourceExp.UpdateParameters["CategoryId"].DefaultValue        =   ddCategory.SelectedValue;
                SqlDataSourceExp.UpdateParameters["BillNumber"].DefaultValue        =   txtBillNo.Text.Trim();
                SqlDataSourceExp.UpdateParameters["PaidTo"].DefaultValue            =   txtPaidTo.Text.Trim();
                SqlDataSourceExp.UpdateParameters["SupportBillPaidTo"].DefaultValue =   txtSupportBillPaidTo.Text.Trim();
                SqlDataSourceExp.UpdateParameters["PayTypeId"].DefaultValue         =   ddPayType.SelectedValue;
            }
            else
            {
                e.Cancel = true;
                lblError.Text = "Please Enter Required Field!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            e.Cancel = true;
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }
    protected void gvMaintenance_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["TrMaintainId"] = e.CommandArgument.ToString();
            Response.Redirect("EditMaintenance.aspx");
        }
    }
    protected void btnPrintVoucher_Click(object sender, EventArgs e)
    {
        int MaintenanceId = 0;

        try
        {
            if (txtRefNo.Text.Trim() != "")
            {
                MaintenanceId = Convert.ToInt32(txtRefNo.Text.ToUpper().Replace("MT", ""));
                
                DataSet dsApprovedExpense = DBOperations.GetWorkExpense(MaintenanceId);

                DataView viewVehicle = new DataView(dsApprovedExpense.Tables[0]);
                DataTable distinctValues = viewVehicle.ToTable(true, "VehicleNo");

                if (distinctValues.Rows.Count > 1)
                {
                    // Multi Vehicle Print

                    GenerateVoucherPDFMulti(MaintenanceId, dsApprovedExpense);
                }
                else
                {
                    // Single Vehicle Print

                    GenerateVoucherPDFSingle(MaintenanceId, dsApprovedExpense);
                }
            }
            else
            {
                lblError.Text = "Please Enter Maintenance Ref No";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error! Invalid Maintenance Ref No!";
            lblError.CssClass = "errorMsg";
        }
    }
    private void GenerateVoucherPDFSingle(int MaintenanceId, DataSet dsApprovedExpense)
    {
        string date = DateTime.Today.ToShortDateString();

        string strApproveBy = "", strApproveDate = "";

        if (dsApprovedExpense.Tables[0].Rows.Count > 0)
        {
            int MaintanceId = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["MaintanceId"]);
            int PayTypeId   = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["PayTypeId"]);
            bool IsApproved = Convert.ToBoolean(dsApprovedExpense.Tables[0].Rows[0]["IsApproved"]);

            string strHeading = "";
            string RefNo = ""; string PayTo = "";
            string VehicleNo = ""; string VehicleType = ""; string VehicleCompany = "NA";

            int VehicleID = 0;

            string WorkDate = "", WorkEndDate = "";
            decimal TotalAmount = 0;

            RefNo = dsApprovedExpense.Tables[0].Rows[0]["RefNo"].ToString();

            VehicleID = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["VehicleID"]);
            VehicleNo = dsApprovedExpense.Tables[0].Rows[0]["VehicleNo"].ToString();

            VehicleType = dsApprovedExpense.Tables[0].Rows[0]["VehicleType"].ToString();
            VehicleCompany = dsApprovedExpense.Tables[0].Rows[0]["CompanyName"].ToString();

            if (IsApproved == true)
            {
                strApproveBy = dsApprovedExpense.Tables[0].Rows[0]["ApprovedUser"].ToString();

                if (dsApprovedExpense.Tables[0].Rows[0]["ApproveDate"] != DBNull.Value)
                    strApproveDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["ApproveDate"]).ToShortDateString();
            }
            if (PayTypeId == 1) // Cash
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["PaidTo"].ToString();
                strHeading = "ED Voucher";
            }
            else if (PayTypeId == 2) // Cheque
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "Cheque Voucher";
            }
            else if (PayTypeId == 3) // NEFT
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "NEFT Voucher";
            }
            else if (PayTypeId == 4) // RTGS
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "RTGS Voucher";
            }

            if (dsApprovedExpense.Tables[0].Rows[0]["WorkDate"] != DBNull.Value)
                WorkDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]).ToShortDateString();

            if (dsApprovedExpense.Tables[0].Rows[0]["WorkDateEnd"] != DBNull.Value)
                WorkEndDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDateEnd"]).ToShortDateString();

            try
            {
                // Generate PDF
                int i = 0; // Auto Increment Table Cell For Serial number
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ED Voucher Print-" + RefNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
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

                PdfWriterEvents writerEvent = new PdfWriterEvents("Babaji Shivram");
                pdfwriter.PageEvent = writerEvent;

                pdfDoc.Open();

                // OnStartPage(pdfwriter, pdfDoc);

                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);
                Font HeadFontLineformat = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("PrintVoucher.htm"));

                contents = contents.Replace("[TodayDate]", date);
                contents = contents.Replace("[Heading]", strHeading);
                contents = contents.Replace("[PayTo]", PayTo);
                contents = contents.Replace("[VehicleNo]", VehicleNo);
                contents = contents.Replace("[VehicleType]", VehicleType);
                contents = contents.Replace("[VehicleCompany]", VehicleCompany);
                contents = contents.Replace("[RefNo]", RefNo);

                contents = contents.Replace("[WorkDate]", WorkDate);
                contents = contents.Replace("[WorkEndDate]", WorkDate);

                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                foreach (var htmlelement in parsedContent)
                    pdfDoc.Add(htmlelement as IElement);

                PdfPTable pdftable = new PdfPTable(4);

                pdftable.TotalWidth = 520f;
                pdftable.LockedWidth = true;
                float[] widths = new float[] { 0.05f, 0.3f, 1f, 0.2f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                // Set Table Spacing Before And After html text
                // pdftable.SpacingBefore = 10f;

                pdftable.SpacingAfter = 4f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                pdftable.AddCell(cellwithdata);
                
                // Header: Maintenance Category
                PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Category", GridHeadingFont));
                cellwithdata3.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata3);

                // Header: Description
                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Description", GridHeadingFont));
                cellwithdata4.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata4);

                // Header: Amount
                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("Amount", GridHeadingFont));
                cellwithdata5.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata5);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell SrnoCell = new PdfPCell();
                SrnoCell.Colspan = 1;
                SrnoCell.UseVariableBorders = false;

                // Data Cell: Vehicle No
                PdfPCell CellVehicleNO = new PdfPCell();
                CellVehicleNO.Colspan = 1;
                CellVehicleNO.HorizontalAlignment = Element.ALIGN_LEFT;
                CellVehicleNO.UseVariableBorders = false;

                // Data Cell: Category
                PdfPCell CellCategory = new PdfPCell();
                CellCategory.Colspan = 1;
                CellCategory.HorizontalAlignment = Element.ALIGN_LEFT;
                CellCategory.UseVariableBorders = false;

                // Data Cell: Description
                PdfPCell CellDescription = new PdfPCell();
                CellDescription.Colspan = 1;
                CellDescription.HorizontalAlignment = Element.ALIGN_LEFT;
                CellDescription.UseVariableBorders = false;

                // Data Cell: Amount
                PdfPCell CellAmount = new PdfPCell();
                CellAmount.Colspan = 1;
                CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellAmount.UseVariableBorders = false;

                //  Generate Table Data from Voucher Detail

                foreach (DataRow dr in dsApprovedExpense.Tables[0].Rows)
                {
                    i = i + 1;
                    // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                    // Add Cell Data To Table

                    // Serial number #

                    SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);

                    pdftable.AddCell(SrnoCell);
                    
                    // CellCategory

                    CellCategory.Phrase = new Phrase(Convert.ToString(dr["CategoryName"]), TextFontformat);

                    pdftable.AddCell(CellCategory);

                    // CellDescription

                    CellDescription.Phrase = new Phrase(Convert.ToString(dr["ExpenseDesc"]), TextFontformat);

                    pdftable.AddCell(CellDescription);

                    // CellAmount
                    if (dr["Amount"] != DBNull.Value)
                    {
                        TotalAmount = TotalAmount + Convert.ToDecimal(dr["Amount"]);
                        CellAmount.Phrase = new Phrase(Convert.ToString(dr["Amount"]), TextFontformat);
                        pdftable.AddCell(CellAmount);
                    }
                    else
                    {
                        CellAmount.Phrase = new Phrase("", TextFontformat);
                        pdftable.AddCell(CellAmount);
                    }

                }// END_ForEach

                // Add Footer Row For Total Amount

                SrnoCell.Phrase = new Phrase("", TextFontformat);
                CellCategory.Phrase = new Phrase("", TextFontformat);
                CellDescription.Phrase = new Phrase("Total", TextBoldformat);
                CellAmount.Phrase = new Phrase(Convert.ToString(TotalAmount), TextBoldformat);

                pdftable.AddCell(SrnoCell);
                pdftable.AddCell(CellCategory);
                pdftable.AddCell(CellDescription);
                pdftable.AddCell(CellAmount);
                /////

                pdfDoc.Add(pdftable);

                Paragraph ParaSpacing = new Paragraph();
                ParaSpacing.SpacingBefore = 10;

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("User   : " + LoggedInUser.glEmpName, TextFontformat));

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("Approved By   : " + strApproveBy, TextFontformat));

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("Approved On   : " + strApproveDate, TextFontformat));

                #region Add Vehicle Vardi Status
                /************* Vehicle Daily Running Status Detail ******************************************/

                // Get Vehicle Vehicle Vardi Status

                DateTime StatusStartDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]);

                DateTime StatusEndDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]);

                StatusStartDate = StatusStartDate.AddDays(-2);
                StatusEndDate = StatusEndDate.AddDays(2);

                DataSet dsVehicleStatus = DBOperations.GetVehicleStatusByDate(VehicleID, StatusStartDate, StatusEndDate);

                PdfPTable pdfTableStatus = new PdfPTable(5);

                pdfTableStatus.TotalWidth = 520f;
                pdfTableStatus.LockedWidth = true;
                float[] widths2 = new float[] { 0.08f, 0.7f, 0.7f, 0.7f, 0.7f };
                pdfTableStatus.SetWidths(widths2);
                pdfTableStatus.HorizontalAlignment = Element.ALIGN_LEFT;

                pdfTableStatus.SpacingAfter = 8f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell celldataSL = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                pdfTableStatus.AddCell(celldataSL);

                // Header: Maintenance Category
                PdfPCell celldataVehicle = new PdfPCell(new Phrase("Vehicle No", GridHeadingFont));
                celldataVehicle.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTableStatus.AddCell(celldataVehicle);

                // Header: Description
                PdfPCell celldataDate = new PdfPCell(new Phrase("Status Date", GridHeadingFont));
                celldataDate.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTableStatus.AddCell(celldataDate);

                // Header: Daily Status
                PdfPCell celldataStatus = new PdfPCell(new Phrase("Status", GridHeadingFont));
                celldataStatus.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTableStatus.AddCell(celldataStatus);

                // Header: Status Remark
                PdfPCell celldataRemark = new PdfPCell(new Phrase("Remark", GridHeadingFont));
                celldataRemark.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTableStatus.AddCell(celldataRemark);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell CellSL = new PdfPCell();
                CellSL.Colspan = 1;
                CellSL.UseVariableBorders = false;

                PdfPCell CellVehicle = new PdfPCell();
                CellVehicle.Colspan = 1;
                CellVehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                CellVehicle.UseVariableBorders = false;

                PdfPCell CellStatusDate = new PdfPCell();
                CellStatusDate.Colspan = 1;
                CellStatusDate.HorizontalAlignment = Element.ALIGN_LEFT;
                CellStatusDate.UseVariableBorders = false;

                PdfPCell CellStatus = new PdfPCell();
                CellStatus.Colspan = 1;
                CellStatus.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellStatus.UseVariableBorders = false;

                PdfPCell CellRemark = new PdfPCell();
                CellRemark.Colspan = 1;
                CellRemark.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellRemark.UseVariableBorders = false;

                //  Generate Table Data from Vehicle Daily Status Detail

                int j = 0;

                foreach (DataRow dr in dsVehicleStatus.Tables[0].Rows)
                {
                    j = j + 1;
                    // Add Cell Data To Table

                    // SL number #

                    CellSL.Phrase = new Phrase(Convert.ToString(j), TextFontformat);

                    pdfTableStatus.AddCell(CellSL);

                    // CellVehicleNo

                    CellVehicle.Phrase = new Phrase(Convert.ToString(dr["VehicleNo"]), TextFontformat);

                    pdfTableStatus.AddCell(CellVehicle);

                    // CellStatusDate

                    CellStatusDate.Phrase = new Phrase(Convert.ToDateTime(dr["StatusDate"]).ToShortDateString(), TextFontformat);

                    pdfTableStatus.AddCell(CellStatusDate);

                    // CellStatus

                    CellStatus.Phrase = new Phrase(Convert.ToString(dr["Status"]), TextFontformat);

                    pdfTableStatus.AddCell(CellStatus);

                    // CellRemark

                    CellRemark.Phrase = new Phrase(Convert.ToString(dr["Remark"]), TextFontformat);

                    pdfTableStatus.AddCell(CellRemark);

                }// END_ForEach
                
                // ADD Para Spacing Before

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("TRANSPORT HISTORY", HeadFontLineformat));
                pdfDoc.Add(ParaSpacing);

                // Add Vehicle Status Table To PDF Doc

                pdfDoc.Add(pdfTableStatus);
                #endregion
                /**************END Vehicle Daily Status *****************************/

                /********************************************************************
                 ******** Get Vehcile Expense History ********
                *********************************************************************/

                #region Add Vehicle Expense History
                /************* Vehicle Daily Running Status Detail ******************************************/

                // Get Vehicle Expense For Current Year

                DateTime ExpStartDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]);

                DateTime ExpEndDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]);

                ExpStartDate = StatusStartDate.AddDays(-2);
                ExpEndDate = StatusEndDate.AddDays(2);

                DataSet dsVehicleExpense = DBOperations.GetVehicleExpepsneByDate(MaintenanceId, VehicleID, ExpStartDate, ExpEndDate);

                int TotalColumn = 0;
                if (dsVehicleExpense.Tables.Count > 0)
                {
                    if (dsVehicleExpense.Tables[0].Rows.Count > 0)
                    {
                        TotalColumn = dsVehicleExpense.Tables[0].Columns.Count;

                        // Add SL No and Total Column

                        PdfPTable pdfTableExp = new PdfPTable(TotalColumn + 2); // Add SL & Total Column

                        pdfTableExp.TotalWidth = 520f;
                        pdfTableExp.LockedWidth = true;
                        pdfTableExp.HorizontalAlignment = Element.ALIGN_LEFT;

                        pdfTableExp.SpacingAfter = 4f;

                        // Create Table Column Header Cell with Text

                        // Add PDF Cell SL

                        PdfPCell cellSL2 = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                        pdfTableExp.AddCell(cellSL2);

                        DataRow drFooterTotal = dsVehicleExpense.Tables[0].NewRow();

                        drFooterTotal[0] = "Total";

                        // Add Column Name Returned From Expense Dataset
                        for (int k = 0; k < TotalColumn; k++)
                        {
                            string strColName = dsVehicleExpense.Tables[0].Columns[k].ColumnName;
                            PdfPCell cellHeader = new PdfPCell(new Phrase(strColName, GridHeadingFont));
                            pdfTableExp.AddCell(cellHeader);

                            if (k > 0)
                            {
                                string strExpresssion = "Sum(" + strColName + ")";
                                drFooterTotal[k] = dsVehicleExpense.Tables[0].Compute(strExpresssion, String.Empty);
                            }
                        }

                        // Add Row Total Column Name

                        PdfPCell cellTotal2 = new PdfPCell(new Phrase("Total", GridHeadingFont));
                        pdfTableExp.AddCell(cellTotal2);

                        //  Generate Table Data from Vehicle Expense History

                        int SlCount = 0;
                        Decimal decRowTotal = 0m;

                        foreach (DataRow dr in dsVehicleExpense.Tables[0].Rows)
                        {
                            decRowTotal = 0m;
                            SlCount = SlCount + 1;
                            // Add Cell Data To Table

                            PdfPCell cellSLData = new PdfPCell(new Phrase(SlCount.ToString(), TextFontformat));
                            pdfTableExp.AddCell(cellSLData);

                            for (int m = 0; m < TotalColumn; m++) // Exclude SL & Total Column
                            {
                                // Add SL Number 
                                if (m == 0) // Category Name Column
                                {
                                    PdfPCell cellCategory = new PdfPCell(new Phrase(dr[m].ToString(), TextFontformat));
                                    pdfTableExp.AddCell(cellCategory);
                                }
                                else
                                {
                                    if (dr[m] != DBNull.Value)
                                    {
                                        decRowTotal = decRowTotal + Convert.ToDecimal(dr[m]);
                                    }

                                    PdfPCell cellMonth = new PdfPCell(new Phrase(dr[m].ToString(), TextFontformat));
                                    pdfTableExp.AddCell(cellMonth);
                                }

                            }//END_FOR_COLUMN

                            // Add Total Amount Row Data

                            PdfPCell cellRowTotal = new PdfPCell(new Phrase(decRowTotal.ToString(), TextFontformat));
                            pdfTableExp.AddCell(cellRowTotal);

                        }// END_ForEach_TableRow

                        // Add Footer Row For Total Amount/Month Wise

                        PdfPCell cellSLFooter = new PdfPCell(new Phrase("", TextFontformat));
                        pdfTableExp.AddCell(cellSLFooter);

                        PdfPCell cellTotalFooter = new PdfPCell(new Phrase("Total", GridHeadingFont));
                        pdfTableExp.AddCell(cellTotalFooter);


                        // Add Total Column Amount - Month Wise

                        Decimal decGrandTotal = 0m;

                        for (int k = 1; k < TotalColumn; k++)
                        {
                            PdfPCell cellSUMFooter = new PdfPCell(new Phrase(drFooterTotal[k].ToString(), TextFontformat));
                            pdfTableExp.AddCell(cellSUMFooter);

                            if (drFooterTotal[k] != DBNull.Value)
                            {
                                decGrandTotal = decGrandTotal + Convert.ToDecimal(drFooterTotal[k].ToString());
                            }
                        }

                        PdfPCell cellGrandTotal = new PdfPCell(new Phrase(decGrandTotal.ToString(), GridHeadingFont));
                        pdfTableExp.AddCell(cellGrandTotal);

                        pdfDoc.Add(new Paragraph("EXPENSE HISTORY", HeadFontLineformat));
                        pdfDoc.Add(ParaSpacing);

                        // Add Vehicle Status Table To PDF Doc

                        pdfDoc.Add(pdfTableExp);
                    }//END_IF_Expense Row Data Not Found
                }//EMD_IF_Expense Table Data Not Found
                #endregion

                if (TotalColumn == 0)
                {
                    pdfDoc.Add(new Paragraph("EXPENSE HISTORY", HeadFontLineformat));
                    pdfDoc.Add(ParaSpacing);
                }
                /****** END Vehicle Expense Histoy **********************************/
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
        }//END_IF
        else
        {
            lblError.Text = "No maintenance detail found for RefNo-" + txtRefNo.Text.Trim();
            lblError.CssClass = "errorMsg";
        }
    }
    private void GenerateVoucherPDFMulti(int MaintenanceId, DataSet dsApprovedExpense)
    {
        string date = DateTime.Today.ToShortDateString();

        string strApproveBy = "", strApproveDate = "";

        if (dsApprovedExpense.Tables[0].Rows.Count > 0)
        {
            int MaintanceId     = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["MaintanceId"]);
            int PayTypeId       = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["PayTypeId"]);
            bool IsApproved     = Convert.ToBoolean(dsApprovedExpense.Tables[0].Rows[0]["IsApproved"]);

            string strHeading = "";
            string RefNo = ""; string PayTo = "";
            string VehicleNo = ""; string VehicleType = ""; string VehicleCompany = "NA";

            int VehicleID = 0;

            string WorkDate = "", WorkEndDate = "";
            decimal TotalAmount = 0;

            RefNo           = dsApprovedExpense.Tables[0].Rows[0]["RefNo"].ToString();

            VehicleID       = Convert.ToInt32(dsApprovedExpense.Tables[0].Rows[0]["VehicleID"]);
            VehicleNo       = dsApprovedExpense.Tables[0].Rows[0]["VehicleNo"].ToString();

            VehicleType     = dsApprovedExpense.Tables[0].Rows[0]["VehicleType"].ToString();
            VehicleCompany  = dsApprovedExpense.Tables[0].Rows[0]["CompanyName"].ToString();

            if (IsApproved == true)
            {
                strApproveBy    = dsApprovedExpense.Tables[0].Rows[0]["ApprovedUser"].ToString();

                if (dsApprovedExpense.Tables[0].Rows[0]["ApproveDate"] != DBNull.Value)
                    strApproveDate   = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["ApproveDate"]).ToShortDateString();
            }
            if (PayTypeId == 1) // Cash
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["PaidTo"].ToString();
                strHeading = "ED Voucher";
            }
            else if (PayTypeId == 2) // Cheque
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "Cheque Voucher";
            }
            else if (PayTypeId == 3) // NEFT
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "NEFT Voucher";
            }
            else if (PayTypeId == 4) // RTGS
            {
                PayTo = dsApprovedExpense.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
                strHeading = "RTGS Voucher";
            }

            if (dsApprovedExpense.Tables[0].Rows[0]["WorkDate"] != DBNull.Value)
                WorkDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDate"]).ToShortDateString();

            if (dsApprovedExpense.Tables[0].Rows[0]["WorkDateEnd"] != DBNull.Value)
                WorkEndDate = Convert.ToDateTime(dsApprovedExpense.Tables[0].Rows[0]["WorkDateEnd"]).ToShortDateString();

            try
            {
                // Generate PDF
                int i = 0; // Auto Increment Table Cell For Serial number
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ED Voucher Print-" + RefNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
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
                                                
                PdfWriterEvents writerEvent = new PdfWriterEvents("Babaji Shivram");
                pdfwriter.PageEvent = writerEvent;

                pdfDoc.Open();

               // OnStartPage(pdfwriter, pdfDoc);

                Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);
                Font HeadFontLineformat = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);

                string contents = "";
                contents = File.ReadAllText(Server.MapPath("PrintVoucherMulti.htm"));
                
                contents = contents.Replace("[TodayDate]", date);
                contents = contents.Replace("[Heading]", strHeading);
                contents = contents.Replace("[PayTo]", PayTo);
               // contents = contents.Replace("[VehicleNo]", VehicleNo);
               // contents = contents.Replace("[VehicleType]", VehicleType);
                contents = contents.Replace("[VehicleCompany]", VehicleCompany);
                contents = contents.Replace("[RefNo]", RefNo);

                contents = contents.Replace("[WorkDate]", WorkDate);
                contents = contents.Replace("[WorkEndDate]", WorkDate);

                var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                foreach (var htmlelement in parsedContent)
                    pdfDoc.Add(htmlelement as IElement);

                PdfPTable pdftable = new PdfPTable(5);

                pdftable.TotalWidth = 520f;
                pdftable.LockedWidth = true;
                float[] widths = new float[] { 0.08f, 0.2f, 0.3f, 0.7f, 0.2f };
                pdftable.SetWidths(widths);
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                // Set Table Spacing Before And After html text
                // pdftable.SpacingBefore = 10f;

                pdftable.SpacingAfter = 8f;

                // Create Table Column Header Cell with Text

                // Header: Serial Number
                PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                pdftable.AddCell(cellwithdata);

                // Header: Vehicle No
                PdfPCell cellwithdata1 = new PdfPCell(new Phrase("VehicleNo", GridHeadingFont));
                pdftable.AddCell(cellwithdata1);

                // Header: Maintenance Category
                PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Category", GridHeadingFont));
                cellwithdata3.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata3);

                // Header: Description
                PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Description", GridHeadingFont));
                cellwithdata4.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata4);

                // Header: Amount
                PdfPCell cellwithdata5 = new PdfPCell(new Phrase("Amount", GridHeadingFont));
                cellwithdata5.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftable.AddCell(cellwithdata5);

                // Data Cell: Serial Number - Auto Increment Cell

                PdfPCell SrnoCell = new PdfPCell();
                SrnoCell.Colspan = 1;
                SrnoCell.UseVariableBorders = false;

                // Data Cell: Vehicle No
                PdfPCell CellVehicleNO = new PdfPCell();
                CellVehicleNO.Colspan = 1;
                CellVehicleNO.HorizontalAlignment = Element.ALIGN_LEFT;
                CellVehicleNO.UseVariableBorders = false;

                // Data Cell: Category
                PdfPCell CellCategory = new PdfPCell();
                CellCategory.Colspan = 1;
                CellCategory.HorizontalAlignment = Element.ALIGN_LEFT;
                CellCategory.UseVariableBorders = false;

                // Data Cell: Description
                PdfPCell CellDescription = new PdfPCell();
                CellDescription.Colspan = 1;
                CellDescription.HorizontalAlignment = Element.ALIGN_LEFT;
                CellDescription.UseVariableBorders = false;

                // Data Cell: Amount
                PdfPCell CellAmount = new PdfPCell();
                CellAmount.Colspan = 1;
                CellAmount.HorizontalAlignment = Element.ALIGN_RIGHT;
                CellAmount.UseVariableBorders = false;

                //  Generate Table Data from Voucher Detail

                foreach (DataRow dr in dsApprovedExpense.Tables[0].Rows)
                {
                    i = i + 1;
                    // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                    // Add Cell Data To Table

                    // Serial number #
                   
                    SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                   
                    pdftable.AddCell(SrnoCell);

                    // Vehicle number #

                    CellVehicleNO.Phrase = new Phrase(Convert.ToString(dr["VehicleNo"]), TextFontformat);

                    pdftable.AddCell(CellVehicleNO);

                    // CellCategory

                    CellCategory.Phrase = new Phrase(Convert.ToString(dr["CategoryName"]), TextFontformat);

                    pdftable.AddCell(CellCategory);

                    // CellDescription

                    CellDescription.Phrase = new Phrase(Convert.ToString(dr["ExpenseDesc"]), TextFontformat);

                    pdftable.AddCell(CellDescription);

                    // CellAmount
                    if (dr["Amount"] != DBNull.Value)
                    {
                        TotalAmount = TotalAmount + Convert.ToDecimal(dr["Amount"]);
                        CellAmount.Phrase = new Phrase(Convert.ToString(dr["Amount"]), TextFontformat);
                        pdftable.AddCell(CellAmount);
                    }
                    else
                    {
                        CellAmount.Phrase = new Phrase("", TextFontformat);
                        pdftable.AddCell(CellAmount);
                    }

                }// END_ForEach

                // Add Footer Row For Total Amount

                SrnoCell.Phrase         = new Phrase("", TextFontformat);
                CellVehicleNO.Phrase = new Phrase("", TextFontformat);
                CellCategory.Phrase     = new Phrase("", TextFontformat);
                CellDescription.Phrase  = new Phrase("Total", TextBoldformat);
                CellAmount.Phrase       = new Phrase(Convert.ToString(TotalAmount), TextBoldformat);

                pdftable.AddCell(SrnoCell);
                pdftable.AddCell(CellVehicleNO);
                pdftable.AddCell(CellCategory);
                pdftable.AddCell(CellDescription);
                pdftable.AddCell(CellAmount);
                /////

                pdfDoc.Add(pdftable);

                Paragraph ParaSpacing = new Paragraph();
                ParaSpacing.SpacingBefore = 20;

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("User   : " + LoggedInUser.glEmpName, TextFontformat));

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("Approved By   : " + strApproveBy, TextFontformat));

                pdfDoc.Add(ParaSpacing);
                pdfDoc.Add(new Paragraph("Approved On   : " + strApproveDate, TextFontformat));
                
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
        }//END_IF
        else
        {
            lblError.Text       = "No maintenance detail found for RefNo-" + txtRefNo.Text.Trim();
            lblError.CssClass   = "errorMsg";
        }
    }

    //public override void OnStartPage(PdfWriter writer, Document document)
    //{
        
    //    string watermarkText = "Babaji Shivram";
    //    float fontSize = 80;
    //    float xPosition = 300;
    //    float yPosition = 400;
    //    float angle = 45;
    //    try
    //    {
    //        PdfContentByte under = writer.DirectContentUnder;
    //        BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
    //        under.BeginText();
    //        under.SetColorFill(iTextSharp.text.pdf.CMYKColor.LIGHT_GRAY);
    //        under.SetFontAndSize(baseFont, fontSize);
    //        under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
    //        under.EndText();
    //    }
    //    catch (Exception ex)
    //    {
    //        //Console.Error.WriteLine(ex.Message);
    //    }
        
    //}

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "VehicleExpense.aspx";
            DataFilter1.FilterDataSource();
            gvMaintenance.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region DataSourceEvents

    protected void SqlDataSourceExp_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        e.ExceptionHandled = false;

        if (e.Exception != null)
        {
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

            if (Result == 0)
            {
                lblError.Text = "Expense Detail Updated Successfully !";
                lblError.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Maintenance Category Already Exists For Vehicle!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Maintenance Detail Already Approved! Update Not Allowed!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Expense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvMaintenance.AllowPaging = false;
        gvMaintenance.AllowSorting = false;

        gvMaintenance.Columns[1].Visible = false;
        gvMaintenance.Columns[2].Visible = false;
        gvMaintenance.Columns[3].Visible = true;

        // Description
        //gvMaintenance.Columns[14].Visible = true;
        //gvMaintenance.Columns[15].Visible = false;

        //gvMaintenance.Caption = "Vehicle Expense Details - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "VehicleExpense.aspx";
        DataFilter1.FilterDataSource();
        gvMaintenance.DataBind();

        //gvJobDetail.DataSourceID = "PendingDutySqlDataSource";
        //gvJobDetail.DataBind();

        gvMaintenance.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

}