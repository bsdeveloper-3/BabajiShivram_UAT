using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using iTextSharp.text;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class NForm_NFormDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    int count_dtTable = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(txtJobNumber);
        ScriptManager1.RegisterPostBackControl(btnGenReport);
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkPdf);

        txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (!IsPostBack)
        {
            fsDeliveryDetails.Visible = false;
            txtJobNumber.Focus();
            gvNformDet_Report.Visible = false;
        }
    }

    #region UPDATE N FORM DELIVERY DETIALS

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("NFormLogin.aspx");
    }

    protected void btnSearchJobNformDet_OnClick(object sender, EventArgs e)
    {
        if (txtJobNumber.Text != null && txtJobNumber.Text != "")
        {
            lblError.Text = "";
            int result = DBOperations.GetJobDetailForNForm(Convert.ToString(txtJobNumber.Text.Trim()));
            if (result == -3)
            {
                lblError.Text = "Delivery Details not updated.";
                lblError.CssClass = "errorMsg";
                fsDeliveryDetails.Visible = false;
            }
            else if (result == -2)
            {
                lblError.Text = "N Form not applicable for this job.";
                lblError.CssClass = "errorMsg";
                fsDeliveryDetails.Visible = false;
            }
            else if (result == -1)
            {
                lblError.Text = "Job Ref Number not found.";
                lblError.CssClass = "errorMsg";
                fsDeliveryDetails.Visible = false;
            }
            else
            {
                lblError.Text = "";
                Session["JobId"] = result.ToString();
                FillGridViewDeliveryDetails(Convert.ToInt32(Session["JobId"]));

            }
        }
        else
        {
            fsDeliveryDetails.Visible = false;
            txtJobNumber.Focus();
            lblError.Text = "Enter Job Number.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancelJobNo_OnClick(object sender, EventArgs e)
    {
        lblError.Text = "";
        txtJobNumber.Text = "";
        GridViewDelivery.Visible = false;
        fsDeliveryDetails.Visible = false;
    }

    protected void FillGridViewDeliveryDetails(int JobId)
    {
        DataSet ds = new DataSet();
        ds = DBOperations.GetDeliveryDetailsForNForm(JobId);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (count_dtTable == 2)
                {
                    lblError.Text = "Successfully Updated Delivery Details.";
                    lblError.CssClass = "success";
                    GridViewDelivery.DataSource = ds;
                    GridViewDelivery.DataBind();
                    fsDeliveryDetails.Visible = true;
                    GridViewDelivery.Visible = true;

                }
                else
                {
                    lblError.Text = "";
                    GridViewDelivery.DataSource = ds;
                    GridViewDelivery.DataBind();
                    fsDeliveryDetails.Visible = true;
                    GridViewDelivery.Visible = true;
                }
            }
            else
            {
                lblError.Text = "N Form Details Already Updated for this Job No.";
                lblError.CssClass = "errorMsg";
                GridViewDelivery.Visible = false;
                fsDeliveryDetails.Visible = false;
            }
        }
        else
        {
            if (count_dtTable == 2)
            {
                lblError.Text = "Successfully Updated Delivery Details.";
                lblError.CssClass = "success";
                fsDeliveryDetails.Visible = false;
                GridViewDelivery.Visible = false;
            }
            else
            {
                lblError.Text = "N Form Details Already Updated for this Job No.";
                lblError.CssClass = "errorMsg";
                fsDeliveryDetails.Visible = false;
                GridViewDelivery.Visible = false;
            }
        }
    }

    protected void GridViewDelivery_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "downloaddoc")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != null)
                DownloadDocument(DocPath);
        }

    }

    protected void GridViewDelivery_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblError.Visible = false;
        lblError.Text = "";
        GridViewDelivery.EditIndex = e.NewEditIndex;
        FillGridViewDeliveryDetails(Convert.ToInt32(Session["JobId"]));
    }

    protected void GridViewDelivery_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblError.Visible = false;
        lblError.Text = "";
        GridViewDelivery.EditIndex = -1;
        FillGridViewDeliveryDetails(Convert.ToInt32(Session["JobId"]));
    }

    protected void GridViewDelivery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewDelivery.PageIndex = e.NewPageIndex;
        GridViewDelivery.DataBind();
        FillGridViewDeliveryDetails(Convert.ToInt32(Session["JobId"]));
    }

    protected void GridViewDelivery_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string strCustDocFolder = "", strJobFileDir = "", strFilePath = "", strUploadPath = "";
        int Result_NForm = -2, Result_HandNForm = -2;
        int lid = Convert.ToInt32(GridViewDelivery.DataKeys[e.RowIndex].Value.ToString());

        //TextBox txtNFormNo = (TextBox)GridViewDelivery.Rows[e.RowIndex].FindControl("txtNFormNo");
        //TextBox txtNFormDate = (TextBox)GridViewDelivery.Rows[e.RowIndex].FindControl("txtNFormDate");
        TextBox txtNClosingDate = (TextBox)GridViewDelivery.Rows[e.RowIndex].FindControl("txtNClosingDate");
        TextBox txtAmount = (TextBox)GridViewDelivery.Rows[e.RowIndex].FindControl("txtAmount");
        FileUpload fuNformDoc = (FileUpload)GridViewDelivery.Rows[e.RowIndex].FindControl("fuDocument_NForm");
        FileUpload fuHandNformDoc = (FileUpload)GridViewDelivery.Rows[e.RowIndex].FindControl("fuDocument_HandNForm");

        DateTime dtNClosingDate = DateTime.MinValue;
        DateTime dtNFormDate = DateTime.MinValue;

        if (txtNClosingDate.Text.Trim() != "")
            dtNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim());
        //if (txtNFormDate.Text.Trim() != "")
        //    dtNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim());

        #region DOC UPLOAD

        // Job Detail
        if (fuNformDoc.HasFile != false || fuHandNformDoc.HasFile != false)
        {
            DataSet dsJobDetail = DBOperations.GetJobDetail(Convert.ToInt32(Session["JobId"]));
            if (dsJobDetail.Tables[0].Rows.Count > 0)
            {
                if (dsJobDetail.Tables[0].Rows[0]["DocFolder"] != DBNull.Value)
                    strCustDocFolder = dsJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\";

                if (dsJobDetail.Tables[0].Rows[0]["FileDirName"] != DBNull.Value)
                    strJobFileDir = dsJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
                strUploadPath = strCustDocFolder + strJobFileDir;
            }

            if (fuNformDoc.FileName.Trim() != "")
            {
                strFilePath = UploadDocument(strUploadPath, fuNformDoc);
                Result_NForm = DBOperations.AddPCDDocument_Nform(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(47), Convert.ToString(""), strFilePath, Convert.ToInt32(Session["VendorId"]));
            }

            if (fuHandNformDoc.FileName.Trim() != "")
            {
                strFilePath = UploadDocument(strUploadPath, fuHandNformDoc);
                Result_HandNForm = DBOperations.AddPCDDocument_Nform(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(48), Convert.ToString(""), strFilePath, Convert.ToInt32(Session["VendorId"]));
            }
        }

        #endregion

        int result = DBOperations.UpdateDeliveryDetForNForm(lid, Convert.ToDateTime(dtNClosingDate), Convert.ToInt32(Session["VendorId"]),
                                                            Convert.ToInt32(Session["JobId"]), Convert.ToString(txtAmount.Text));

        if (result == 0)
        {
            count_dtTable = 2;
            lblError.Text = "Successfully Updated Delivery Details.";
            lblError.CssClass = "success";
            GridViewDelivery.EditIndex = -1;
            FillGridViewDeliveryDetails(Convert.ToInt32(Session["JobId"]));
        }
        else if (result == 2)
        {
            lblError.Text = "Delivery detail does not exists.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void GridViewDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Edit) ||
                   (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
        {
            ImageButton imgbtnDelNform = (ImageButton)e.Row.Cells[10].FindControl("imgbtnDelNformDoc");
            FileUpload fuNForm = (FileUpload)e.Row.Cells[10].FindControl("fuDocument_NForm");

            ImageButton imgbtnDelHandNform = (ImageButton)e.Row.Cells[11].FindControl("imgbtnDelHandNformDoc");
            FileUpload fuHandNForm = (FileUpload)e.Row.Cells[11].FindControl("fuDocument_HandNForm");

            imgbtnDelNform.Attributes.Add("OnClick", "javascript:return imgDelNformDoc_OnClick('" + fuNForm.ClientID + "');");
            imgbtnDelHandNform.Attributes.Add("OnClick", "javascript:return imgDelNformDoc_OnClick('" + fuHandNForm.ClientID + "');");
        }
    }

    #endregion

    #region DOCUMENT UPLOAD

    private string UploadDocument(string FilePath, FileUpload fuUpload)
    {
        string FileName = fuUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PCA_" + Session["JobId"].ToString() + "\\";

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
        if (fuUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuUpload.SaveAs(ServerFilePath + FileName);
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

    #region GENERATE REPORT

    protected void btnGenReport_OnClick(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (txtDateFrom.Text != "" && txtDateTo.Text != "")
        {
            DateTime dtClosingDateFrom = new DateTime();
            DateTime dtClosingDateTo = new DateTime();

            if (txtDateFrom.Text.Trim() != "")
                dtClosingDateFrom = Commonfunctions.CDateTime(txtDateFrom.Text.Trim());
            if (txtDateTo.Text.Trim() != "")
                dtClosingDateTo = Commonfunctions.CDateTime(txtDateTo.Text.Trim());

            DataSet dsNformDelReport = new DataSet();
            dsNformDelReport = DBOperations.GetDeliveryNform_Report(dtClosingDateFrom, dtClosingDateTo);

            if (dsNformDelReport != null)
            {
                if (dsNformDelReport.Tables[0].Rows.Count > 0)
                {
                    object TotalAmnt;
                    TotalAmnt = dsNformDelReport.Tables[0].Compute("Sum(Amount)", "");
                    DataRow dr_GranTotal = dsNformDelReport.Tables[0].NewRow();
                    dr_GranTotal["N Form Closing Date"] = "GRAND TOTAL :";
                    dr_GranTotal["Amount"] = TotalAmnt;
                    int lastRow = dsNformDelReport.Tables[0].Rows.Count;
                    dsNformDelReport.Tables[0].Rows.InsertAt(dr_GranTotal, lastRow + 1);

                    gvNformDet_Report.DataSource = dsNformDelReport;
                    gvNformDet_Report.DataBind();
                    gvNformDet_Report.Visible = true;
                }

                if (gvNformDet_Report.Rows.Count > 0)
                {
                    gvNformDet_Report.Columns[2].ItemStyle.Wrap = true;
                    var lastRow = gvNformDet_Report.Rows[gvNformDet_Report.Rows.Count - 1];
                    lastRow.Cells[0].Text = "";
                }

            }
            else
                gvNformDet_Report.Visible = false;
        }
        else
            gvNformDet_Report.Visible = false;
    }

    protected void lnkPdf_Click(object sender, EventArgs e)
    {
        try
        {
            string BillNo = "";

            if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                DateTime dtClosingDateFrom = new DateTime();
                DateTime dtClosingDateTo = new DateTime();

                if (txtDateFrom.Text.Trim() != "")
                    dtClosingDateFrom = Commonfunctions.CDateTime(txtDateFrom.Text.Trim());
                if (txtDateTo.Text.Trim() != "")
                    dtClosingDateTo = Commonfunctions.CDateTime(txtDateTo.Text.Trim());
                if (txtBillNo_Report.Text != "")
                    BillNo = txtBillNo_Report.Text.Trim();

                DataSet dsNformDelReport = new DataSet();
                dsNformDelReport = DBOperations.GetDeliveryNform_Report(dtClosingDateFrom, dtClosingDateTo);

                if (dsNformDelReport != null)
                {
                    if (dsNformDelReport.Tables[0].Rows.Count > 0)
                    {
                        object TotalAmnt;
                        TotalAmnt = dsNformDelReport.Tables[0].Compute("Sum(Amount)", "");
                        DataRow dr_GranTotal = dsNformDelReport.Tables[0].NewRow();
                        dr_GranTotal["N Form Closing Date"] = "GRAND TOTAL :";
                        dr_GranTotal["Amount"] = TotalAmnt;
                        int lastRow = dsNformDelReport.Tables[0].Rows.Count;
                        dsNformDelReport.Tables[0].Rows.InsertAt(dr_GranTotal, lastRow + 1);

                        //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF.jpg"));
                        //string date = DateTime.Today.ToShortDateString();

                        // Generate PDF
                        int i = 0; // Auto Increment Table Cell For Serial number
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=NForm" + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        StringWriter sw = new StringWriter();
                        sw.Write("<br/>");
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        StringReader sr = new StringReader(sw.ToString());

                        Rectangle recPDF = new Rectangle(PageSize.LEGAL_LANDSCAPE);

                        // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                        // Set PDF Document size and Left,Right,Top and Bottom margin

                        Document pdfDoc = new Document(recPDF);
                        pdfDoc.SetPageSize(iTextSharp.text.PageSize.LEGAL_LANDSCAPE.Rotate());

                        //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();

                        Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                        Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                        Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                        Paragraph ParaSpacing = new Paragraph();
                        ParaSpacing.SpacingBefore = 0;
                        Paragraph paraLogo = new Paragraph();
                        pdfDoc.Add(new Paragraph("Babaji Shivram CLearing & Carriers Pvt Ltd.", GridHeadingFont));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("Plot No. 2, Behind Excom House", TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("Saki Vihar Road, Sakinaka                                                                                                        Bill No. : " + BillNo, TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        pdfDoc.Add(new Paragraph("Andheri East.                                                                                                                             Date : " + DateTime.Now.ToString("dd.MM.yyyy"), TextFontformat));
                        pdfDoc.Add(ParaSpacing);

                        PdfPTable pdftable = new PdfPTable(12);
                        pdftable.TotalWidth = 1000f;
                        pdftable.LockedWidth = true;
                        float[] widths = new float[] { 0.1f, 0.3f, 0.7f, 0.2f, 0.6f, 0.1f, 0.1f, 0.2f, 0.125f, 0.15f, 0.15f, 0.125f };
                        pdftable.SetWidths(widths);
                        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Set Table Spacing Before And After html text
                        pdftable.SpacingBefore = 10f;
                        pdftable.SpacingAfter = 10f;

                        #region Create Table Column Header Cell with Text

                        // Header: Serial Number
                        PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr. No.", GridHeadingFont));
                        cellwithdata.Colspan = 1;
                        cellwithdata.BorderWidth = 1f;
                        //  cellwithdata.BackgroundColor = GrayColor.LIGHT_GRAY;

                        cellwithdata.HorizontalAlignment = 0;//left
                        pdftable.AddCell(cellwithdata);

                        // Header: BS Job No
                        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("BS Job No", GridHeadingFont));
                        cellwithdata1.Colspan = 1;
                        cellwithdata1.BorderWidth = 1f;
                        cellwithdata1.HorizontalAlignment = 0;
                        cellwithdata1.VerticalAlignment = 0;// Center
                        pdftable.AddCell(cellwithdata1);

                        // Header: Invoice
                        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Invoice No.", GridHeadingFont));
                        cellwithdata2.Colspan = 1;
                        cellwithdata2.BorderWidth = 1f;
                        cellwithdata2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        cellwithdata2.VerticalAlignment = Element.ALIGN_RIGHT;// Center
                        pdftable.AddCell(cellwithdata2);

                        // Header: BOE
                        PdfPCell cellwithdata3 = new PdfPCell(new Phrase("BOE", GridHeadingFont));
                        cellwithdata3.Colspan = 1;
                        cellwithdata3.BorderWidth = 1f;
                        cellwithdata3.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata3);

                        // Header: Customer Name
                        PdfPCell cellwithdata4 = new PdfPCell(new Phrase("Customer Name", GridHeadingFont));
                        cellwithdata4.Colspan = 1;
                        cellwithdata4.BorderWidth = 1f;
                        cellwithdata4.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata4);

                        // Header: No of PKG
                        PdfPCell cellwithdata5 = new PdfPCell(new Phrase("No of PKG", GridHeadingFont));
                        cellwithdata5.Colspan = 1;
                        cellwithdata5.BorderWidth = 1f;
                        cellwithdata5.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata5);

                        // Header: Weight
                        PdfPCell cellwithdata6 = new PdfPCell(new Phrase("Weight", GridHeadingFont));
                        cellwithdata6.Colspan = 1;
                        cellwithdata6.BorderWidth = 1f;
                        cellwithdata6.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata6);

                        // Header: Vehicle No
                        PdfPCell cellwithdata7 = new PdfPCell(new Phrase("Vehicle No", GridHeadingFont));
                        cellwithdata7.Colspan = 1;
                        cellwithdata7.BorderWidth = 1f;
                        cellwithdata7.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata7);

                        // Header: N Form No
                        PdfPCell cellwithdata8 = new PdfPCell(new Phrase("'N' Form No", GridHeadingFont));
                        cellwithdata8.Colspan = 1;
                        cellwithdata8.BorderWidth = 1f;
                        cellwithdata8.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata8);

                        // Header: N Form Opening Date
                        PdfPCell cellwithdata9 = new PdfPCell(new Phrase("'N' Form Opening Date", GridHeadingFont));
                        cellwithdata9.Colspan = 1;
                        cellwithdata9.BorderWidth = 1f;
                        cellwithdata9.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata9);

                        // Header: N Form Closing Date
                        PdfPCell cellwithdata10 = new PdfPCell(new Phrase("'N' Form Closing Date", GridHeadingFont));
                        cellwithdata10.Colspan = 1;
                        cellwithdata10.BorderWidth = 1f;
                        cellwithdata10.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata10);

                        // Header: Amount
                        PdfPCell cellwithdata11 = new PdfPCell(new Phrase("Amount", GridHeadingFont));
                        cellwithdata11.Colspan = 1;
                        cellwithdata11.BorderWidth = 1f;
                        cellwithdata11.HorizontalAlignment = 0;
                        pdftable.AddCell(cellwithdata11);
                        #endregion

                        #region  DATA CELLS
                        //Data Cell: Serial Number - Auto Increment Cell

                        PdfPCell SrnoCell = new PdfPCell();
                        SrnoCell.Colspan = 1;
                        SrnoCell.HorizontalAlignment = 0;//1

                        // Data Cell: BS Job No PdfCell
                        PdfPCell BSJobNoCell = new PdfPCell();
                        BSJobNoCell.Colspan = 1;
                        BSJobNoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        BSJobNoCell.VerticalAlignment = Element.ALIGN_LEFT;

                        // Data Cell: Invoice PdfCell
                        PdfPCell InvoiceCell = new PdfPCell();
                        InvoiceCell.Colspan = 1;
                        InvoiceCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        InvoiceCell.VerticalAlignment = Element.ALIGN_LEFT;

                        // Data Cell: Invoice Date PdfCell
                        //PdfPCell InvDateCell = new PdfPCell();
                        //InvDateCell.Colspan = 1;
                        //InvDateCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //InvDateCell.VerticalAlignment = Element.ALIGN_LEFT;

                        // Data Cell: BOE PdfCell
                        PdfPCell ShippingBOECell = new PdfPCell();
                        ShippingBOECell.Colspan = 1;
                        ShippingBOECell.HorizontalAlignment = Element.ALIGN_CENTER;
                        ShippingBOECell.VerticalAlignment = Element.ALIGN_CENTER;

                        // Data Cell: Customer Name PdfCell
                        PdfPCell CustNameCell = new PdfPCell();
                        CustNameCell.Colspan = 1;
                        CustNameCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        CustNameCell.VerticalAlignment = Element.ALIGN_LEFT;

                        // Data Cell: No Of PKG PdfCell
                        PdfPCell NoofPkgCell = new PdfPCell();
                        NoofPkgCell.Colspan = 1;
                        NoofPkgCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        NoofPkgCell.VerticalAlignment = Element.ALIGN_RIGHT;

                        // Data Cell: Weight PdfCell
                        PdfPCell WeightCell = new PdfPCell();
                        WeightCell.Colspan = 1;
                        WeightCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        WeightCell.VerticalAlignment = Element.ALIGN_RIGHT;

                        // Data Cell: VehicleNo PdfCell
                        PdfPCell VehicleNoCell = new PdfPCell();
                        VehicleNoCell.Colspan = 1;
                        VehicleNoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        VehicleNoCell.VerticalAlignment = Element.ALIGN_CENTER;

                        // Data Cell: NformNo PdfCell
                        PdfPCell NformNoCell = new PdfPCell();
                        NformNoCell.Colspan = 1;
                        NformNoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        NformNoCell.VerticalAlignment = Element.ALIGN_CENTER;

                        // Data Cell: Nform Opening date PdfCell
                        PdfPCell NformOpeningDtCell = new PdfPCell();
                        NformOpeningDtCell.Colspan = 1;
                        NformOpeningDtCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        NformOpeningDtCell.VerticalAlignment = Element.ALIGN_CENTER;

                        // Data Cell: Nform Closing date PdfCell
                        PdfPCell NformClosingDtCell = new PdfPCell();
                        NformClosingDtCell.Colspan = 1;
                        NformClosingDtCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        NformClosingDtCell.VerticalAlignment = Element.ALIGN_CENTER;

                        // Data Cell: Amount PdfCell
                        PdfPCell AmountCell = new PdfPCell();
                        AmountCell.Colspan = 1;
                        AmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        AmountCell.VerticalAlignment = Element.ALIGN_RIGHT;
                        #endregion

                        // Generae Table Data from PCA Document Dataset
                        foreach (DataRow dr in dsNformDelReport.Tables[0].Rows)
                        {
                            i = i + 1;
                            // Serial number #
                            SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                            pdftable.AddCell(SrnoCell);

                            // BS Job No
                            BSJobNoCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["BS Job No"]), TextFontformat);
                            pdftable.AddCell(BSJobNoCell);

                            // Invoice
                            InvoiceCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["Invoice No"]), TextFontformat);
                            pdftable.AddCell(InvoiceCell);

                            // Shipping Bill No/ BOE 
                            ShippingBOECell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["BOE"]), TextFontformat);
                            pdftable.AddCell(ShippingBOECell);

                            // Customer Name
                            CustNameCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["Customer Name"]), TextFontformat);
                            pdftable.AddCell(CustNameCell);

                            // No of pkg
                            NoofPkgCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["No of PKG"]), TextFontformat);
                            pdftable.AddCell(NoofPkgCell);

                            // Weight
                            WeightCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["Weight"]), TextFontformat);
                            pdftable.AddCell(WeightCell);

                            // Vehicle No
                            VehicleNoCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["Vehicle No"]), TextFontformat);
                            pdftable.AddCell(VehicleNoCell);

                            // N Form No
                            NformNoCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["N Form No"]), TextFontformat);
                            pdftable.AddCell(NformNoCell);

                            // N Form Opening Date
                            NformOpeningDtCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["N Form Opening Date"]), TextFontformat);
                            pdftable.AddCell(NformOpeningDtCell);

                            // N Form Closing Date
                            NformClosingDtCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["N Form Closing Date"]), TextFontformat);
                            pdftable.AddCell(NformClosingDtCell);

                            // Amount
                            AmountCell.Phrase = new Phrase(Convert.ToString(dsNformDelReport.Tables[0].Rows[i - 1]["Amount"]), TextFontformat);
                            pdftable.AddCell(AmountCell);

                        }// END_
                        pdfDoc.Add(pdftable);
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.Write(pdfDoc);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancelReport_OnClick(object sender, EventArgs e)
    {
        gvNformDet_Report.Visible = false;
        txtBillNo_Report.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                DateTime dtClosingDateFrom = new DateTime();
                DateTime dtClosingDateTo = new DateTime();

                if (txtDateFrom.Text.Trim() != "")
                    dtClosingDateFrom = Commonfunctions.CDateTime(txtDateFrom.Text.Trim());
                if (txtDateTo.Text.Trim() != "")
                    dtClosingDateTo = Commonfunctions.CDateTime(txtDateTo.Text.Trim());
                //if (txtBillNo_Report.Text != "")
                //  BillNo = txtBillNo_Report.Text.Trim();

                DataSet dsNformDelReport = new DataSet();
                dsNformDelReport = DBOperations.GetDeliveryNform_Report(dtClosingDateFrom, dtClosingDateTo);
                if (dsNformDelReport.Tables[0].Rows.Count > 0)
                {
                    string strFileName = "NformDeliveryDet_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
                    ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dsNformDelReport.Tables[0]);
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType, DataTable dtReport)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            dtReport.TableName = "Nform Delivery Details";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (dtReport.Rows.Count > 0)
            {
                dtReport.Columns.Add("Sr.No.", typeof(int));
                int i = 1;
                foreach (DataRow dr in dtReport.Rows)
                {
                    dr["Sr.No."] = i;
                    i = i + 1;
                }
                dtReport.Columns["Sr.No."].SetOrdinal(0);
                object TotalAmnt;
                TotalAmnt = dtReport.Compute("Sum(Amount)", "");                            //calculates the grand total of amount column
                DataRow dr_GranTotal = dtReport.NewRow();
                dr_GranTotal["N Form Closing Date"] = "GRAND TOTAL :";
                dr_GranTotal["Amount"] = TotalAmnt;
                int lastRow = dtReport.Rows.Count;
                dtReport.Rows.InsertAt(dr_GranTotal, lastRow + 1);                          // assign grand total to the footer row below amount column

                var workSheet = wb.Worksheets.Add(dtReport);
                var SrNo_Col = workSheet.Column("A");                                       //Sr. No. column
                SrNo_Col.Width = 8;
                var Requirement_Col = workSheet.Column("B");                                //Job no column
                Requirement_Col.Width = 20;
                Requirement_Col.Style.Alignment.WrapText = true;
                var InvNo_col = workSheet.Column("C");                                      //Invoice no column
                InvNo_col.Width = 35;
                InvNo_col.Style.Alignment.WrapText = true;
                var BOE_col = workSheet.Column("D");                                        //BOE column
                BOE_col.Width = 25;
                BOE_col.Style.Alignment.WrapText = true;
                var Amnt_col = workSheet.Column("L");                                       //amount column
                Amnt_col.Cell(lastRow + 2).Style.Font.Bold = true;
                Amnt_col.Cell(lastRow + 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                var NClosingDateLastRow_col = workSheet.Column("L");                         //n form closing date column
                NClosingDateLastRow_col.Cell(lastRow + 2).Style.Font.Bold = true;            // highlight the grand total cell in nform closing date column

                string lastRange = "L" + (lastRow + 2);
                var rngData = workSheet.Range("A1", lastRange);                              // gets range from first cell 'A1' to last cell in excel sheet
                workSheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // set border to the entire table in excel
                workSheet.Style.Alignment.WrapText = true;
            }

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    #endregion
}