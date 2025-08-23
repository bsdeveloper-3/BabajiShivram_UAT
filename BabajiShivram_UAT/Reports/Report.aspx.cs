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
using System.Data.SqlClient;
public partial class Reports_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "DSR Job Report";

            DBOperations.FillCustomer(ddCustomer);
            DBOperations.FillBranch(ddBabajiBranch);
        }
    }

    protected void btnDownloadExcel_Click(Object Sender, EventArgs e)
    {
        BindGridView();
    }

    private void BindGridView()
    {        
        if (ddCustomer.SelectedValue != "0")
        {
            string strCustName = ddCustomer.SelectedItem.Text;

            string strFileName = "DSR_"+strCustName+"_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            string strCaption =  "DSR " +strCustName + " " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
               bool bIsParamExists = false;
               string strDispatchFromDate = Commonfunctions.CDateTime(txtFromDate.Text.Trim()).ToShortDateString();
               string strDispatchToDate = Commonfunctions.CDateTime(txtToDate.Text.Trim()).ToShortDateString();

               ParameterCollection SelParams = ReportSqlDataSource.SelectParameters;

               foreach (Parameter SelParam in SelParams)
               {
                   if (SelParam.Name == "OutOfChargeFromDate")
                   {
                       bIsParamExists = true;
                       break;
                   }
               }

               if (bIsParamExists == false)
               {
                   ReportSqlDataSource.SelectParameters.Add("OutOfChargeFromDate", strDispatchFromDate);
                   ReportSqlDataSource.SelectParameters.Add("OutOfChargeToDate", strDispatchToDate);
               }
               else
               {
                   ReportSqlDataSource.SelectParameters["OutOfChargeFromDate"].DefaultValue =   strDispatchFromDate;
                   ReportSqlDataSource.SelectParameters["OutOfChargeToDate"].DefaultValue =  strDispatchToDate;
               }
            }

            ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel", strCaption);
           
        }
        else
        {
            lblError.Text = "Please Select Customer!";
            lblError.CssClass = "errorMsg";
        }
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
        
    protected void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect("~/Reports/Report.aspx");
    }

    #region DropDown Event

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvReportField.Visible = false;
        if (ddCustomer.SelectedIndex > 0)
        { 
            int CustomerId = Convert.ToInt32(ddCustomer.SelectedValue);
            DBOperations.FillCustomerDivision(ddDivision, CustomerId);
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvReportField.Visible = false;
        if (ddDivision.SelectedIndex > 0)
        {
            int DivisionId = Convert.ToInt32(ddDivision.SelectedValue);
            DBOperations.FillCustomerPlant(ddPlant, DivisionId);
        }
    }

    #endregion

    #region ExportData

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType,string strCaption)
    {
        lblError.Text = "";
        gvReportField.DataSourceID = "ReportSqlDataSource";
        gvReportField.DataBind();
        gvReportField.Visible = true;

        if (gvReportField.Rows.Count > 0)
        {
            lblError.Text = "";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            gvReportField.Caption = strCaption; 

            gvReportField.DataBind();

            gvReportField.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
           
        }
        else
        {
            lblError.Text = "No Record Found!";
            lblError.CssClass = "errorMsg";
        }
    }
    #endregion
}
