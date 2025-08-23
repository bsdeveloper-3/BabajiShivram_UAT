using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Freight_FreightMISProfit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkProfitDetailXls);
        ScriptManager1.RegisterPostBackControl(lnkExportProfit);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MIS Buy / Sell";
        }
    }

    protected void gvProfit_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            GridViewRow rowUser = gvProfit.SelectedRow;

            int rowIndex = Convert.ToInt32(commandArgs[0]);
            string strReportMonth = commandArgs[1];

            string strFreightSPC = gvProfit.DataKeys[rowIndex].Value.ToString();


            BindProfitDetail(strFreightSPC, strReportMonth);
        }
    }

    private void BindProfitDetail(string SPCId, string MonthID)
    {
        string strCaptionText = "";
        pnlProfitDetailXLS.Visible = false;
        
        if (Convert.ToInt32(MonthID) >= 0)
        {
            ProfitDetailSqlDataSource.SelectParameters["SPCId"].DefaultValue    = SPCId;
            ProfitDetailSqlDataSource.SelectParameters["MonthId"].DefaultValue  = MonthID;
            
            gvUserDetail.DataBind();

            if (gvUserDetail.Rows.Count > 0)
            {
                pnlProfitDetailXLS.Visible = true;

            }//END_IF
        }//END_IF
        else
        {
            gvUserDetail.DataSource = null;
            gvUserDetail.DataBind();
            gvUserDetail.Caption = "No Record Found!";
        }
    }

    protected void gvUserDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Export To Excel

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkExportProfit_Click(object sender, EventArgs e)
    {
        string strFileName = "Freight_SPC_Profit" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        string header = "attachment;filename=" + strFileName;
        string contentType = "application/vnd.ms-excel";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvProfit.AllowPaging = false;
        gvProfit.AllowSorting = false;

        gvProfit.DataSourceID = "DataSourceProfit";
        gvProfit.DataBind();

        //Remove Controls
        this.RemoveControls(gvProfit);

        gvProfit.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    
    protected void lnkProfitDetailXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Freight_SPC_Buy-Sell-Detail" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        string header = "attachment;filename=" + strFileName;
        string contentType = "application/vnd.ms-excel";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvUserDetail.AllowPaging = false;
        gvUserDetail.AllowSorting = false;

        gvUserDetail.DataSourceID = "ProfitDetailSqlDataSource";
        gvUserDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvUserDetail);

        gvUserDetail.RenderControl(hw);

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
    #endregion
}