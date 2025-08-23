using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;

public partial class Reports_FujiFilmDsrReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            txtFromDate.Text = "01/04/2023";//DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //  ddClearedStatus.SelectedValue = "0";
        }
        gvReport.DataSource = DataSourceReport;
        gvReport.DataBind();
        DataBind();
    }

    protected void DataBind()
    {
        // DataTable dt = DBOperations.WabTecDSR(LoggedInUser.glUserId, LoggedInUser.glFinYearId, Convert.ToInt16(ddClearedStatus.SelectedValue), (ddConsignee.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        //  gvReport.DataSource = dt;
        // gvReport.DataBind();
    }

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "FujiFilmDSR_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    private void ExportWorkbook(string header, string contentType)
    {
        // DataView view = BindFilter();
        DataView view = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        DataTable dtReport = view.ToTable();

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "FujiFilmDSR_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            if (dtReport.Rows.Count > 0)
            {
                int totalColumn = dtReport.Columns.Count;

                //dtReport.Columns.RemoveAt(totalColumn - 10); // JobDate
                //dtReport.Columns.RemoveAt(totalColumn - 2); //  ClearedStatusName


                dtReport.TableName = "FujiFilmDSR_Report";
                string strFileName = "FujiFilmDSR_Report.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

                var sheet = wb.Worksheets.Add(dtReport);

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
        //DataView dvRecord = new DataView();

        DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        dvRecord.RowFilter = DataSourceReport.FilterExpression;

        DataSourceReport.DataBind();
        gvReport.DataBind();

        return dvRecord;

    }
    protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        gvReport.DataSource = DataSourceReport;
        gvReport.DataBind();
        BindFilter();
        DataBind();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddClearedStatus.SelectedIndex = 0;
        //ddConsignee.SelectedIndex = 0;
        DataSourceReport.FilterExpression = "";

        DataSourceReport.DataBind();
        gvReport.DataBind();

    }
}