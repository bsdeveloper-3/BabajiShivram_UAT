using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Data;

public partial class Reports_StockReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkPortXls);
       
        if (!IsPostBack)
        {
            //DBOperations.FillCompanyByCategory(ddlCustomer, 1);
            gvStockReport.DataBind();
        }
        
    }

    protected string DefaultVal(string val)
    {
        if (val == null)
            return ("-");
        else
            return (val);

    }
    protected void lnkPortXls_Click(object sender, EventArgs e)
    {
        string strFileName = "SavitaStockReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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

        gvStockReport.AllowPaging = false;
        gvStockReport.AllowSorting = false;

        gvStockReport.DataSourceID = "StockReportSqlDataSource";
        gvStockReport.DataBind();

        //Remove Controls
        this.RemoveControls(gvStockReport);

        gvStockReport.RenderControl(hw);

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

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvStockReport.DataBind();
    }

    protected void gvStockReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string BOEDate = DataBinder.Eval(e.Row.DataItem, "BOEDate").ToString();
            string ExBOEDate = DataBinder.Eval(e.Row.DataItem, "ExBOEDate").ToString();
            string BOENo = DataBinder.Eval(e.Row.DataItem, "BOENo").ToString();
            string ExBOEno = DataBinder.Eval(e.Row.DataItem, "ExBOENo").ToString();
            string BEType = DataBinder.Eval(e.Row.DataItem, "BETypeName").ToString();
            if (ExBOEDate == "" || ExBOEDate == null)
            {
                e.Row.Cells[15].Text = "-";
            }

            if (BOEDate == "" || BOEDate == null)
            {
                e.Row.Cells[8].Text = "-";
            }

            if (BOENo == "0")
            {
                e.Row.Cells[9].Text = "-";
            }

            if (ExBOEno == "0")
            {
                e.Row.Cells[16].Text = "-";
            }

            if(BEType=="Home")
            {
                e.Row.Cells[13].Text = "-";
                e.Row.Cells[14].Text = "-";
            }



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Attributes.Add("class", "amount");
            }

        }
    }

   
}