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
public partial class Reports_CustomerReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Report";
        }
    }

    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        //  BindGridView();
    }

    protected void gvReportField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
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
        */
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "DSR_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        string strCaption = "DSR ON " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        ExportFunction("attachment;filename="+ strFileName, "application/vnd.ms-excel", strCaption);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType, string strCaption)
    {
        gvReportField.DataSourceID = "ReportSqlDataSource";
        gvReportField.DataBind();

        if (gvReportField.Rows.Count > 0)
        {
            gvReportField.Visible = true;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            
            gvReportField.Caption = strCaption; 

            gvReportField.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();

            gvReportField.Visible = false;
        }
        else
        {
            lblError.Text = "No Record Found!";
            lblError.CssClass = "errorMsg";
        }
    }
    #endregion
}
