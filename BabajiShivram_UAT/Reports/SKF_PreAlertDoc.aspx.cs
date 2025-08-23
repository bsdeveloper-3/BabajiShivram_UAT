using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using ClosedXML.Excel;
public partial class Reports_SKF_PreAlertDoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvReport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "SKF PreAlert Doc";
        }

        //
        DataFilter1.DataSource = DataSourceReport;
        DataFilter1.DataColumns = gvReport.Columns;
        DataFilter1.FilterSessionID = "SKF_PreAlertDoc.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }
        

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }
    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "SKF_PreAlertDoc.aspx";
            DataFilter1.FilterDataSource();
            gvReport.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region exportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "SKF_PreAlert_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

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
        gvReport .AllowPaging = false;
        gvReport.AllowSorting = false;

        gvReport.Columns[1].Visible = false;
        gvReport.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "SKF_PreAlertDoc.aspx";
        DataFilter1.FilterDataSource();
        gvReport.DataBind();
        
        gvReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    protected void gvReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName.ToLower() == "view")
        {
            int PreAlertID = Convert.ToInt32(e.CommandArgument);

            DataSet dsPreAlertInvoice = DBOperations.GetSKFPreAlertInvoice(PreAlertID);
            DataSet dsPreAlertItem = DBOperations.GetSKFPreAlertItem(PreAlertID);

            // Export To Excel

            ExportFunction(dsPreAlertInvoice.Tables[0].Rows[0]["DocumentID"].ToString()+".xls", dsPreAlertInvoice.Tables[0], dsPreAlertItem.Tables[0]);
        }
    }

    private void ExportFunction(string DocumentId, DataTable dtInvoice,DataTable dtItem)
    {
        string strheader = "attachment;filename=\"" + DocumentId + "\"";
        using (XLWorkbook wb = new XLWorkbook())
        {
            dtInvoice.TableName = "Invoice";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", strheader);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (dtInvoice.Rows.Count > 0)
            {
                dtItem.TableName = "Product";
                var workSheetInvoice = wb.Worksheets.Add(dtInvoice);
                workSheetInvoice.Style.Alignment.WrapText = true;
                workSheetInvoice.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            if (dtItem.Rows.Count > 0)
            {
                var workSheetItem = wb.Worksheets.Add(dtItem);
                workSheetItem.Style.Alignment.WrapText = true;
                workSheetItem.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}