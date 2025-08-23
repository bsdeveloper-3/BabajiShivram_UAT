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


public partial class Reports_JOHNSONDsrReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        ScriptManager1.RegisterPostBackControl(gvReport);

        if (!IsPostBack)
        {
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFromDate.Text = "01/04/2021";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "JOHNSON-HITACHI DSR Report";
            gvReport.DataBind();
        }
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

        //if (txtFromDate.Text.Trim() != "")
        //{
        //    dtFromDate = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        //}
        //if (txtToDate.Text.Trim() != "")
        //{
        //    dtToDate = Commonfunctions.CDateTime(txtToDate.Text.Trim());
        //}

        //if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        //{
        //    strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
        //        + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        //{
        //    strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)
        //        + "# AND JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex == 0)
        //{
        //    strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";
        //}
        //else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex == 0)
        //{
        //    strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "#";

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        //{
        //    strFilter = "ClearedStatus = " + ddClearedStatus.SelectedValue;

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "" && ddClearedStatus.SelectedIndex > 0)
        //{
        //    strFilter = "JobDate >= #" + dtFromDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "" && ddClearedStatus.SelectedIndex > 0)
        //{
        //    strFilter = "JobDate <= #" + dtToDate.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) + "# AND ClearedStatus = " + ddClearedStatus.SelectedValue;

        //    DataSourceReport.FilterExpression = strFilter;
        //}
        //else
        //{
            strFilter = "";

            DataSourceReport.FilterExpression = "";
        //}

        DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        dvRecord.RowFilter = DataSourceReport.FilterExpression;

        DataSourceReport.DataBind();
        gvReport.DataBind();

        return dvRecord;

    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddClearedStatus.SelectedIndex = 0;

        DataSourceReport.FilterExpression = "";

        DataSourceReport.DataBind();
        gvReport.DataBind();

    }
    #endregion


    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "JOHNSON_Hitachi_DSR_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
        //ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    private void ExportWorkbook(string header, string contentType)
    {
        DataView view = BindFilter();
        DataTable dtReport = view.ToTable();

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "JOHNSON_Hitachi_DSR_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            if (dtReport.Rows.Count > 0)
            {
                int totalColumn = dtReport.Columns.Count;

                //dtReport.Columns.RemoveAt(totalColumn - 1); // JobDate
                //dtReport.Columns.RemoveAt(totalColumn - 2); //  ClearedStatusName


                dtReport.TableName = "JOHNSON_Hitachi_DSR_Report";
                string strFileName = "JOHNSON_Hitachi_DSR_Report.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

                var sheet = wb.Worksheets.Add(dtReport);

                sheet.Style.Font.SetFontName("Calibri Light");

                sheet.Style.Font.SetFontSize(10);

                //wb.Worksheets.Add(dtReport);

                wb.SaveAs(ServerFilePath);

                DownloadDocument(ServerFilePath, strFileName);
            }
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


    protected void ddClearedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvReport.DataBind();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        gvReport.DataBind();
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        gvReport.DataBind();
    }
}