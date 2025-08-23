using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Web;
using ClosedXML.Excel;
public partial class Reports_Asian_Breakwall : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Asian Paint - Breakwall";

        }

    }

    #region Export To Excel
    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "AsianPaint_Breakwall_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportWorkbook("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
        
      //  string strFileName = "AsianPaint_Breakwall_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
      //  ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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
        gvReport.DataBind();

        //Remove Controls
        this.RemoveControls(gvReport);

        gvReport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
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
    private void ExportWorkbook(string header, string contentType)
    {
        DataSourceSelectArguments args = new DataSourceSelectArguments();
        DataView view = (DataView)DataSourceReport.Select(args);
        DataTable dtReport = view.ToTable();

        using (XLWorkbook wb = new XLWorkbook())
        {
            string Report = "AsianPaint_Breakwall_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            //   Response.Clear();
            //   Response.Buffer = true;
            //   Response.AddHeader("content-disposition", header);
            //   Response.Charset = "";
            //   this.EnableViewState = false;
            //   Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //   StringWriter sw = new StringWriter();
            //   HtmlTextWriter hw = new HtmlTextWriter(sw);
                        
            if (dtReport.Rows.Count > 0)
            {
                dtReport.TableName = "AsianPaint_BreakWall";
                string strFileName = "AsianPaint_BreakWall.xlsx";

                string ServerFilePath = Server.MapPath("..\\UploadFiles\\"+ strFileName);

                wb.Worksheets.Add(dtReport);
                
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
    #endregion

}