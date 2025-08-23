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

public partial class Transport_VehicleStatusReport : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnGenerateReport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vardi Daily Expense Detail";

            mevReportDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            txtReportDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        ReportDocument crystalReport = new ReportDocument();
        DateTime dtReportDate = DateTime.MinValue;
        DateTime dtReportDateTo = DateTime.MinValue;
        dsTripDetail dsReportData = new dsTripDetail();
        int TransporterId = 0;

        if (txtReportDate.Text.Trim() != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtReportDate.Text.Trim());
        }
        if (txtReportDateTo.Text.Trim() != "")
        {
            dtReportDateTo = Commonfunctions.CDateTime(txtReportDateTo.Text.Trim());
        }
        if (dtReportDate != DateTime.MinValue && ddlTransporter.SelectedValue != "0")
        {
            TransporterId = Convert.ToInt32(ddlTransporter.SelectedValue);
            dsReportData = GetReportData(dtReportDate, dtReportDateTo, TransporterId);
            crystalReport.Load(Server.MapPath("TripDetail.rpt"));

            if (dsReportData.Tables.Count > 0 && dsReportData.Tables[0].Rows.Count > 0)
            {
                crystalReport.SetDataSource(dsReportData);
                string fileName = "VehicleStatusReport_" + DateTime.Now.ToString("dd/MM/yyyy");

                crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, fileName);
                crystalReport.Close();
                crystalReport.Clone();
                crystalReport.Dispose();
                crystalReport = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            else
            {
                lblError.Text = "Report Detail Not Found!";
                lblError.CssClass = "errorMsg!";
            }
        }
        else
        {
            lblError.Text = "Please select transporter name.";
            lblError.CssClass = "errorMsg!";
        }
    }

    private dsTripDetail GetReportData(DateTime Date, DateTime DateTo, int TransporterId)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("TR_rptTripDetailDate");

        cmd.Parameters.Add(new SqlParameter("@Date", Date));
        cmd.Parameters.Add(new SqlParameter("@DateTo", DateTo));
        cmd.Parameters.Add(new SqlParameter("@TransporterId", TransporterId));
        cmd.Parameters.Add(new SqlParameter("@lUser", LoggedInUser.glUserId));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (dsTripDetail dsTripDetail_Loc = new dsTripDetail())
                {
                    sda.Fill(dsTripDetail_Loc, "dtTripDetail");
                    return dsTripDetail_Loc;
                }
            }
        }
    }
}