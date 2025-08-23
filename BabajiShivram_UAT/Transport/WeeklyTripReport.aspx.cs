using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Threading;
using System.Net;
using System.ComponentModel;
using Ionic.Zip;
using ClosedXML.Excel;

public partial class Transport_WeeklyTripReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnGenerateReport);
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Weekly Trip Report";

            mevReportDateFrom.MaximumValue = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtReportDateFrom.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            mevReportDateTo.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            txtReportDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DBOperations.FillCompanyByCategory(ddlTransporter, 6);
        }
    }

    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        gvWeeklyTripReport.DataBind();
    }

    protected void txtReportDateTo_TextChanged(object sender, EventArgs e)
    {
        if (txtReportDateTo.Text.Trim() != "")
        {
            DateTime dtReportDateFrom = DateTime.MinValue, dtReportDateTo = DateTime.MinValue, dtCurrentReportTo = DateTime.MinValue;
            if (txtReportDateFrom.Text.Trim() != "")
                dtReportDateFrom = Commonfunctions.CDateTime(txtReportDateFrom.Text.Trim());
            if (txtReportDateTo.Text.Trim() != "")
                dtCurrentReportTo = Commonfunctions.CDateTime(txtReportDateTo.Text.Trim());

            if (dtCurrentReportTo != DateTime.MinValue)
            {
                dtReportDateFrom = dtCurrentReportTo.AddDays(-6);
                txtReportDateFrom.Text = dtReportDateFrom.ToString("dd/MM/yyyy");
                mevReportDateFrom.MaximumValue = dtReportDateFrom.ToString("dd/MM/yyyy");
            }
        }
        else
        {
            mevReportDateFrom.MaximumValue = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtReportDateFrom.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            mevReportDateTo.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            txtReportDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void txtReportDateFrom_TextChanged(object sender, EventArgs e)
    {
        if (txtReportDateFrom.Text.Trim() != "")
        {
            DateTime dtReportDateFrom = DateTime.MinValue, dtReportDateTo = DateTime.MinValue, dtCurrentReportTo = DateTime.MinValue;
            if (txtReportDateFrom.Text.Trim() != "")
                dtReportDateFrom = Commonfunctions.CDateTime(txtReportDateFrom.Text.Trim());
            if (txtReportDateTo.Text.Trim() != "")
                dtCurrentReportTo = Commonfunctions.CDateTime(txtReportDateTo.Text.Trim());

            if (dtReportDateFrom != DateTime.MinValue)
            {
                dtReportDateTo = dtReportDateFrom.AddDays(6);
                txtReportDateTo.Text = dtReportDateTo.ToString("dd/MM/yyyy");
                mevReportDateTo.MaximumValue = dtReportDateTo.ToString("dd/MM/yyyy");
            }
        }
        else
        {
            mevReportDateFrom.MaximumValue = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            txtReportDateFrom.Text = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            mevReportDateTo.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            txtReportDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "WeeklyTrip_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvWeeklyTripReport.AllowPaging = false;
        gvWeeklyTripReport.AllowSorting = false;
        gvWeeklyTripReport.Caption = "Weekly trip details for transporter " + ddlTransporter.SelectedItem.Text.Trim().ToUpper() + " on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        gvWeeklyTripReport.DataBind();
        gvWeeklyTripReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}