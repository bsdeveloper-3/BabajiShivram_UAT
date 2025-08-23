using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_ReportVehicleTrip : System.Web.UI.Page
{
    int CurrentMonth = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vehicle Trip Detail";

        ScriptManager1.RegisterPostBackControl(lnkXls);

        CurrentMonth = DateTime.Now.Month - 2;

        if (CurrentMonth <= 1)
        {
            CurrentMonth = 12 + CurrentMonth;
        }
    }

    protected void gvTransportMonth_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int rowIndex = 0;
            int TotalColumn = 13;
            int TripCount = 0;
            DateTime dtStatusDate = DateTime.MinValue;

            for (int i = 3; i <= CurrentMonth; i++)
            {
                TripCount = Convert.ToInt32(e.Row.Cells[i].Text.Trim());

                if (TripCount >= 8 && TripCount <= 15)
                {
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                }
                else if (TripCount < 8)
                {
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
                }

                rowIndex = i + 1;

            }
        }
    }

    protected void lnkXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Trip_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportToExcel("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportToExcel(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvTransportMonth.AllowPaging = false;
        gvTransportMonth.AllowSorting = false;

        gvTransportMonth.DataSourceID = "DataSourceVehicleMonth";
        gvTransportMonth.DataBind();

        //Remove Controls
        this.RemoveControls(gvTransportMonth);

        gvTransportMonth.RenderControl(hw);

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
}