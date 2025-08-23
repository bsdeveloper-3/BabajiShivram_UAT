using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_ReportVehicleTripDaily : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            CalFromDate.SelectedDate = DateTime.Today;
            CalToDate.SelectedDate = DateTime.Today;

            CalFromDate.EndDate = DateTime.Today;
            CalToDate.EndDate = DateTime.Today;


            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Trip Report";
                        
            DBOperations.FillBranch(ddBranch);
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        gvTripReport.DataSource = datasrcTrip;
        gvTripReport.DataBind();

        if (gvTripReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "VehicleTripReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        gvTripReport.DataSource = datasrcTrip;
        gvTripReport.DataBind();

        if (gvTripReport.Rows.Count < 1)
        {
            // lblMessage.Text = "No Record Found!";
            // lblMessage.CssClass = "errorMsg";
        }
        else
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvTripReport.AllowPaging = false;
            gvTripReport.AllowSorting = false;

            gvTripReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}