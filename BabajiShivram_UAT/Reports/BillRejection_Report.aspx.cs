using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_BillRejection_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            //  txtFromDate.Text = "01/04/2023"; //;DateTime.Now.ToString("dd/MM/yyyy");
            // txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Rejection Report";
        }
        gvReport.DataBind();
    }

    protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        string strFilter = "";

        DataSourceReport.FilterExpression = "";

        DateTime dtFromDate = DateTime.MinValue;
        DateTime dtToDate = DateTime.MinValue;
        strFilter = "";

        DataSourceReport.FilterExpression = "";
        dtFromDate = Convert.ToDateTime(txtFromDate.Text);
        dtToDate = Convert.ToDateTime(txtToDate.Text);

        DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        dvRecord.RowFilter = DataSourceReport.FilterExpression;

        DataSourceReport.DataBind();
        gvReport.DataBind();

        //return dvRecord;

        //DataSourceReport.DataBind();
        //gvReport.DataBind();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        DataSourceReport.FilterExpression = "";
        DataSourceReport.DataBind();
        gvReport.DataBind();

    }
    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "BillRejection_Report" + DateTime.Now.ToString("dd/MM/yyyy") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    private void ExportWorkbook(string header, string contentType)
    {
        DataView view = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        DataTable dvRecord = view.ToTable();

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "BillRejection_Report" + DateTime.Now.ToString("0:dd/MM/yyyy") + ".xls";

            if (dvRecord.Rows.Count > 0)
            {
                int totalColumn = dvRecord.Columns.Count;

                dvRecord.Columns.RemoveAt(totalColumn - 1); // JobDate
                //dtReport.Columns.RemoveAt(totalColumn - 2); //  ClearedStatusName


                dvRecord.TableName = "BillRejection_Report";
                string strFileName = "BillRejection_Report.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

                var sheet = wb.Worksheets.Add(dvRecord);

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


}