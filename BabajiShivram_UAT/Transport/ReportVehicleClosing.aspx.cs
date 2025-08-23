using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Transport_ReportVehicleClosing : System.Web.UI.Page
{
    int TotalDaysInMonth = 0;
    int intMonth = 0;
    int intYear = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkXls);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Closing Status";

            txtReportDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        intMonth = Convert.ToDateTime(txtReportDate.Text).Month;
        intYear = Convert.ToDateTime(txtReportDate.Text).Year;

        TotalDaysInMonth = DateTime.DaysInMonth(intYear,intMonth);
    }

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int rowIndex = 0;
            string StatusID = "", VehicleNo = "";
            DateTime dtStatusDate = DateTime.MinValue;
            string strDriver = "", strRemark = "";

            for (int i = 1; i <= TotalDaysInMonth; i++)
            {
                StatusID = DataBinder.Eval(e.Row.DataItem, i.ToString()).ToString();
                VehicleNo = DataBinder.Eval(e.Row.DataItem, "VehicleNo").ToString();
                
                dtStatusDate = Commonfunctions.CDateTime(i.ToString() + "/" + intMonth + "/" + intYear);
                strRemark = "";

                if (StatusID != "")
                {
                    DataSet dsDetails = DBOperations.GetVehicleDailyStatus(VehicleNo, dtStatusDate);

                    if (dsDetails.Tables[0].Rows.Count > 0)
                    {
                        strDriver = dsDetails.Tables[0].Rows[0]["DriverName"].ToString();
                        strRemark = dsDetails.Tables[0].Rows[0]["Remark"].ToString();
                    }
                }

                rowIndex = i + 1;

                if (StatusID == "")
                {
                    e.Row.Cells[rowIndex].ToolTip = "NO UPDATE - ";
                }
                else if (StatusID == "1")
                {
                    e.Row.Cells[rowIndex].BackColor = System.Drawing.Color.Green;
                    e.Row.Cells[rowIndex].Text = "V";
                    e.Row.Cells[rowIndex].ToolTip = "VARDI - " + strDriver + " - "+ strRemark;
                }
                else if (StatusID == "2")
                {
                    e.Row.Cells[rowIndex].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[rowIndex].Text = "B";
                    e.Row.Cells[rowIndex].ToolTip = "BREAKDOWN - " + strDriver + " - " + strRemark;
                }
                else if (StatusID == "3")
                {
                    e.Row.Cells[rowIndex].BackColor = System.Drawing.Color.Yellow;
                    e.Row.Cells[rowIndex].Text = "D";
                    e.Row.Cells[rowIndex].ToolTip = "DETAINED - " + strDriver + " - " + strRemark;
                }
                else if (StatusID == "4")
                {
                    e.Row.Cells[rowIndex].BackColor = System.Drawing.Color.Blue;
                    e.Row.Cells[rowIndex].Text = "A";
                    e.Row.Cells[rowIndex].ToolTip = "ABSENT - " + strDriver + " - " + strRemark;
                }
                else if (StatusID == "5")
                {
                    e.Row.Cells[rowIndex].BackColor = System.Drawing.Color.Gray;
                    e.Row.Cells[rowIndex].Text = "I";
                    e.Row.Cells[rowIndex].ToolTip = "IDLE - " + strDriver + " - " + strRemark;
                }
            }           
        }

    }

    protected void lnkXls_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Month_Closing_" + txtReportDate.Text + ".xls";
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
        GridViewVehicle.AllowPaging = false;
        GridViewVehicle.AllowSorting = false;

        GridViewVehicle.DataSourceID = "SqlDataSourceExp";
        GridViewVehicle.DataBind();

        GridViewVehicle.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
}