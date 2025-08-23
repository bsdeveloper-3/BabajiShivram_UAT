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

public partial class Reports_BillingTurnaround : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    List<string> FromDatevalue = new List<string>();
    List<string> ToDatevalue = new List<string>();
    string[] Fname, fvalue;
    string FilterOtherField, FilterDate, Filter1, FilterTextField;
    DateTime FromDate, ToDate;
    string[] FDate, TDate, Date;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        ScriptManager1.RegisterPostBackControl(gvReport);
        //Session["FINYEAR"] = 9;
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Turnaround";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataView dv = new DataView();
        dv = DBOperations.GetBillingTurnaroundReport(7189, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), null, "", LoggedInUser.glFinYearId, LoggedInUser.glUserId);
        DataTable dt = dv.ToTable();
        gvReport.DataSource = dt;
        gvReport.DataBind();
        gvReport.Visible = true;
    }
    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Billing_Report" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    private void ExportWorkbook(string header, string contentType)
    {
        DataView view = BindFilter();
        DataTable dtReport = view.ToTable();

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "Billing_Turnaround" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            if (dtReport.Rows.Count > 0)
            {
                int totalColumn = dtReport.Columns.Count;

                //dtReport.Columns.RemoveAt(totalColumn - 1); // JobDate
                //dtReport.Columns.RemoveAt(totalColumn - 2); //  ClearedStatusName


                dtReport.TableName = "Billing_Turnaround";
                string strFileName = "Billing_Turnaround.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strFileName);

                var sheet = wb.Worksheets.Add(dtReport);

                //sheet.Style.Font.SetFontName("Calibri Light");

                //sheet.Style.Font.SetFontSize(10);

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

    private DataView BindFilter()
    {
        string strFilter = "";

       // DataSourceReport.FilterExpression = "";

        DateTime dtFromDate = DateTime.MinValue;
        DateTime dtToDate = DateTime.MinValue;


        strFilter = "";

       // DataSourceReport.FilterExpression = "";

        //DataView dvRecord = (DataView)DataSourceReport.Select(DataSourceSelectArguments.Empty);
        //dvRecord.RowFilter = DataSourceReport.FilterExpression;

        DataView dv = new DataView();
        dv = DBOperations.GetBillingTurnaroundReport(7189, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), null, "", LoggedInUser.glFinYearId, LoggedInUser.glUserId);
        DataTable dt = dv.ToTable();
        gvReport.DataSource = dt;
        gvReport.DataBind();

        //DataSourceReport.DataBind();
        //gvReport.DataBind();

        return dv;

    }

}