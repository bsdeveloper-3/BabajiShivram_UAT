using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.Text;
using System.Data.SqlClient;
using System.IO;
public partial class Reports_Thermax_Pre_Alert_Report : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        ScriptManager1.RegisterPostBackControl(gvReport);
        //Session["FINYEAR"] = 9;
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Report";
            txtFromDate.Text = "01-APR-2023";//DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        //txtFromDate.Text = "01/04/2023";
        //txtToDate.Text = DateTime.Now.ToString();
    }

    #region Data Bind
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        BindFilter();
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

        if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
                + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex == 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

            DataSourceReport.FilterExpression = strFilter;
        }
        else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

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

        // DataSourceReport.DataBind();
        gvReport.DataBind();

        return dvRecord;

    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        DataSourceReport.FilterExpression = "";

        DataSourceReport.DataBind();
        gvReport.DataBind();

    }
    #endregion

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Thermax_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");

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


    private void ExportWorkbook(string header, string contentType)
    {
        DataTable dtReport = DBOperations.Thermaxprealert(LoggedInUser.glFinYearId, Convert.ToInt16(ddClearedStatus.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "ThermaxPrealert_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            if (dtReport.Rows.Count > 0)
            {
                int totalColumn = dtReport.Columns.Count;

                //dtReport.Columns.RemoveAt(totalColumn - 1); // JobDate
                dtReport.Columns.RemoveAt(totalColumn - 1); //  ClearedStatusName


                dtReport.TableName = "ThermaxPrealert_Report";
                string strFileName = "ThermaxPrealert_Report.xlsx";

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

        }
        }


        protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        DataBind();
    }

    protected void DataBind()
    {
        DataTable dt = DBOperations.Thermaxprealert(LoggedInUser.glFinYearId, Convert.ToInt16(ddClearedStatus.SelectedValue),  Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        gvReport.DataSource = dt;
        gvReport.DataBind();
    }
}