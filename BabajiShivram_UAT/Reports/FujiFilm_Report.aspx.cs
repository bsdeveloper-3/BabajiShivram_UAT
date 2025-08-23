using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_FujiFilm_Report : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            txtFromDate.Text = "01/04/2024";//DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "FujiFilm Report";

        }
        //gvReport.DataSource = DataSourceReport;
        gvReport.DataBind();
        DataBind();
    }

    protected void DataBind()
    {
        // DataTable dt = DBOperations.WabTecDSR(LoggedInUser.glUserId, LoggedInUser.glFinYearId, Convert.ToInt16(ddClearedStatus.SelectedValue), (ddConsignee.SelectedValue), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
        //  gvReport.DataSource = dt;
       // gvReport.DataBind();
    }

    //protected void lnkReportXls_Click(object sender, EventArgs e)
    //{
    //    string strFileName = "FujiFilm_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
    //    ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    //}

    //private void ExportWorkbook(string header, string contentType)
    //{
    //    // DataView view = BindFilter();
    //    DataView view = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
    //    DataTable dtReport = view.ToTable();

    //    using (XLWorkbook wb = new XLWorkbook())
    //    {
    //        string Report = "FujiFilm_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

    //        if (dtReport.Rows.Count > 0)
    //        {
    //            int totalColumn = dtReport.Columns.Count;

    //            //dtReport.Columns.RemoveAt(totalColumn - 10); // JobDate
    //            //dtReport.Columns.RemoveAt(totalColumn - 2); //  ClearedStatusName


    //            dtReport.TableName = "FujiFilm_Report";
    //            string strFileName = "FujiFilm_Report.xlsx";

    //            string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

    //            var sheet = wb.Worksheets.Add(dtReport);

    //            sheet.Style.Font.SetFontName("Calibri Light");

    //            sheet.Style.Font.SetFontSize(10);

    //            //wb.Worksheets.Add(dtReport);

    //            wb.SaveAs(ServerFilePath);

    //            DownloadDocument(ServerFilePath, strFileName);
    //        }

    //        //using (MemoryStream MyMemoryStream = new MemoryStream())
    //        //{
    //        //    wb.SaveAs(MyMemoryStream);
    //        //    MyMemoryStream.WriteTo(Response.OutputStream);
    //        //    Response.Flush();
    //        //    Response.End();
    //        //}
    //    }
    //}

    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "FujiFilm_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExcelExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvReport.AllowPaging = false;
        gvReport.AllowSorting = false;

        gvReport.DataSourceID = "DataSourceReport";

        BindFilter();

        //gvReport.DataBind();

        //Remove Controls
        this.RemoveControls(gvReport);

        gvReport.RenderControl(hw);
      //  SaveFujiFilmReport(sw.ToString(), "FujiFilm_Report.xlsx");
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    //file Uload functionality
    //private void SaveFujiFilmReport(string content, string fileName)
    //{
    //    string filePath = Server.MapPath("..\\UploadFiles\\" + fileName);
    //    File.WriteAllText(filePath, content);
    //}

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
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

    private DataView BindFilter()
    {
        string strFilter = "";
        DataSourceReport.FilterExpression = "";

        DateTime dtFromDate = DateTime.MinValue;
        DateTime dtToDate = DateTime.MinValue;

        if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
        {
            dtFromDate = Commonfunctions.CDateTime(txtFromDate.Text.Trim());
        }
        if (!string.IsNullOrWhiteSpace(txtToDate.Text))
        {
            dtToDate = Commonfunctions.CDateTime(txtToDate.Text.Trim());
        }

        if (dtFromDate != DateTime.MinValue && dtToDate != DateTime.MinValue && ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = $"JobDate >= #{dtFromDate:MM/dd/yyyy}# AND JobDate <= #{dtToDate:MM/dd/yyyy}# AND ClearedStatus = {ddClearedStatus.SelectedValue}";
        }
        else if (dtFromDate != DateTime.MinValue && dtToDate != DateTime.MinValue)
        {
            strFilter = $"JobDate >= #{dtFromDate:MM/dd/yyyy}# AND JobDate <= #{dtToDate:MM/dd/yyyy}#";
        }
        else if (dtFromDate == DateTime.MinValue && dtToDate != DateTime.MinValue)
        {
            strFilter = $"JobDate <= #{dtToDate:MM/dd/yyyy}#";
        }
        else if (dtFromDate != DateTime.MinValue && dtToDate == DateTime.MinValue)
        {
            strFilter = $"JobDate >= #{dtFromDate:MM/dd/yyyy}#";
        }
        else if (ddClearedStatus.SelectedIndex > 0)
        {
            strFilter = $"ClearedStatus = {ddClearedStatus.SelectedValue}";
        }

        // Apply the filter
        DataSourceReport.FilterExpression = strFilter;

        // Retrieve and filter the data
        DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);

        if (dvRecord.Count == 0)
        {
            System.Diagnostics.Debug.WriteLine("No records found with the applied filter.");
        }

        DataSourceReport.DataBind();
        gvReport.DataBind();

        return dvRecord;
    }

    protected void btnAddFilter_Click(object sender, EventArgs e)
    {
        gvReport.DataSource = DataSourceReport;
        gvReport.DataBind();
        BindFilter();
       // DataBind();
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
}
