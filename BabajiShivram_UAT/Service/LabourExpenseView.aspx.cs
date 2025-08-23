using Syncfusion.Licensing;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Service_LabourExpenseView : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        //ScriptManager1.RegisterPostBackControl(lnkSummary`Excel);

        DateTime dtStartDate = DateTime.Now.AddDays(-7);

        MskEdtValFromDate.MinimumValue = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");

        calStatusDateFrom.StartDate = dtStartDate;
        calStatusDateTo.StartDate = dtStartDate;

        calStatusDateFrom.EndDate = DateTime.Now;
        calStatusDateTo.EndDate = DateTime.Now;

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "View Labour Expense";

            txtStatusDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtStatusDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
           
            }
        }
    }
    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        //if (gvBJVDetail.Rows.Count > 1)
        //{
        //    GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
        //    getRow.Cells[7].BackColor = System.Drawing.Color.Yellow;
        //    getRow.Cells[8].BackColor = System.Drawing.Color.Green;

        //    decimal decDebit = 0m;
        //    decimal decCredit = 0m;
        //    decimal decProfit = 0m;

        //    Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
        //    Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

        //    decProfit = (decCredit - decDebit);


        //    if (decProfit <= 0)
        //    {
        //        getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
        //        getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
        //    }
        //    else
        //    {
        //        getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
        //    }

        //}
    }
    protected void txtStatusDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtReportDate = DateTime.MinValue;

        if (txtStatusDateFrom.Text != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtStatusDateFrom.Text.Trim());
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "NhavaSheva_Expense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;

        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
        
    
}