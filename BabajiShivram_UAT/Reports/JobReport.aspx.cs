using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
public partial class Reports_JobReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Report";
        }
    }

    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        BindGridView();
    }
    private void BindGridView()
    {
    //  DataSet dsReport;       
        string strFieldQuery = "";
        int lastindex = 0;
        foreach (ListItem lstField in chkField.Items)
        {
            if (lstField.Selected)
            {
                strFieldQuery += lstField.Value + ",";
            }
        }
        if (strFieldQuery != "")
        {
            lastindex = strFieldQuery.LastIndexOf(",");

            strFieldQuery = strFieldQuery.Remove(lastindex);

            ReportSqlDataSource.SelectParameters["ColumnList"].DefaultValue = strFieldQuery;

             
          //  ReportSqlDataSource.Select();

            /*
            dsReport = ReportOperations.GetCustomerQueryField(strFieldQuery);

            if (dsReport.Tables[0].Rows.Count > 0)
            {
                gvReportField.DataSource = dsReport;
                gvReportField.DataBind();
            }
            else
            {
                lblError.Text = "No Record Found!";
                lblError.CssClass = "errorMsg";
            }
            */
        }
        else
        {
            lblError.Text = "Please Select Field For Report!";
            lblError.CssClass = "errorMsg";
        }
    }
        
    protected void gvReportField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView dRowView = (DataRowView)e.Row.DataItem;
        bool IsInvoiceColumn = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int colCount = -1;
            foreach (TableCell TCColumn in e.Row.Cells)
            {
                colCount += 1;
                string cellName = gvReportField.HeaderRow.Cells[colCount].Text;
                if (cellName == "InvoiceNoDate")
                {
                    IsInvoiceColumn = true;
                    break;
                }
            }
            
           // if (dRowView["InvoiceNoDate"] != null)
            if (IsInvoiceColumn == true)
            {
                string decodedText = String.Format(e.Row.Cells[colCount].Text, "<br/> <hr noshade size='3' align=left>");
                // String.Format("This is line one{0}This is line two{0}This is line three", "<br/>")
                e.Row.Cells[colCount].Text = decodedText;
            }
        }
    }
    
    protected void gvReportField_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    
    protected void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect("~/Reports/JobReport.aspx");
    }
        
    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        ExportFunction("attachment;filename=JobReport.xls", "application/vnd.ms-excel");
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
        gvReportField.AllowPaging = false;
        gvReportField.AllowSorting = false;
        
        BindGridView();

        gvReportField.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}
