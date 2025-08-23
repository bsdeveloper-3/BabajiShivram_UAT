using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
public partial class Reports_Wabtec_BillingReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            //txtFromDate.Text = "01-APR-2024";//DateTime.Now.ToString("dd/MM/yyyy");
            //txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy"); //DateTime.Now.ToString("dd-MMM-yyyy");
            txtFromDate.Text = "01/04/2024";//DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "WabTec_DSR_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    private void ExportWorkbook(string header, string contentType)
    {
       // DataView view = BindFilter();
        //DataView view = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
       // DataTable dtReport = view.ToTable();
        DataTable dtReport = DBOperations.WabTecDSRForBilling(LoggedInUser.glUserId, LoggedInUser.glFinYearId, (ddConsignee.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "WabTec_DSRBilling_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            if (dtReport.Rows.Count > 0)
            {
                int totalColumn = dtReport.Columns.Count;

                //dtReport.Columns.RemoveAt(totalColumn - 1); // JobDate
                dtReport.Columns.RemoveAt(totalColumn - 1); //  ClearedStatusName


                dtReport.TableName = "WabTec_DSRBilling_Report";
                string strFileName = "WabTec_DSRBilling_Report.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

                var sheet = wb.Worksheets.Add(dtReport);
                var Container_No = sheet.Column("X");
                Container_No.Width = 50;
                var SupplierInvoice_No = sheet.Column("AM");
                SupplierInvoice_No.Width = 50;
                var SupplierInvoice_DT = sheet.Column("AN");
                SupplierInvoice_DT.Width = 50;
                var SupplierInvoice_Value = sheet.Column("AO");
                SupplierInvoice_Value.Width = 50;
                var RemarkDayWiseDetailAction = sheet.Column("CF");
                RemarkDayWiseDetailAction.Width = 50;

                sheet.Style.Alignment.WrapText = true;

                sheet.Style.Font.SetFontName("Calibri Light");

                sheet.Style.Font.SetFontSize(10);

                //wb.Worksheets.Add(dtReport);

                wb.SaveAs(ServerFilePath);

                DownloadDocument(ServerFilePath, strFileName);
            }

            //using (MemoryStream MyMemoryStream = new MemoryStream())
            //{
            //    wb.SaveAs(MyMemoryStream);
            //    MyMemoryStream.WriteTo(Response.OutputStream);
            //    Response.Flush();
            //    Response.End();
            //}
        }
    }

    protected void DownloadDocument(string FileServerPath, string FileName)
    {
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, FileServerPath, FileName);
        }
        catch (Exception ex)
        {
        }
    }

    private DataView BindFilter()
    {
        string strFilter = "";

        DataSourceReport.FilterExpression = "";

        DateTime dtFromDate = DateTime.MinValue;
        DateTime dtToDate = DateTime.MinValue;

        if (txtFromDate.Text.Trim() != "")
        {
            dtFromDate = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        }
        if (txtToDate.Text.Trim() != "")
        {
            dtToDate = Commonfunctions.CDateTime(txtToDate.Text.Trim());
        }

        if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" )
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" )
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" )
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" )
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" )
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" )
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ;

            DataSourceReport.FilterExpression = strFilter;
        }


        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)  + " AND DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern);

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND DivisionId = " + ddConsignee.SelectedValue;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddConsignee.SelectedIndex > 0)
        {
            strFilter =" DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == ""  && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)  + " AND DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == ""  & ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + " AND DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != ""  && ddConsignee.SelectedIndex == 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) ;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == ""  && ddConsignee.SelectedIndex > 0)
        {
            strFilter = "  DivisionId = " + ddConsignee.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else
        {
            strFilter = "";

            DataSourceReport.FilterExpression = "";
        }
        DataView dvRecord = new DataView();

        //DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        //dvRecord.RowFilter = DataSourceReport.FilterExpression;

        //gvReport.DataSourceID = DataSourceReport;
        DataTable dt = DBOperations.WabTecDSRForBilling(LoggedInUser.glUserId, LoggedInUser.glFinYearId, (ddConsignee.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        gvReport.DataSource = dt;
        gvReport.DataBind();

        return dvRecord;

    }
    protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        BindFilter();
        //DataBind();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddConsignee.SelectedIndex = 0;
        DataSourceReport.FilterExpression = "";

        DataSourceReport.DataBind();
        gvReport.DataBind();

    }
}