using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class Transport_VehicleDailyStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int TotalDaysInMonth = 0;
    int intMonth = 0;
    int intYear = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vehicle Daily Status";

        ScriptManager1.RegisterPostBackControl(lnkExportMonth);
        ScriptManager1.RegisterPostBackControl(lnkExportStatus);

        //
        if (!IsPostBack)
        {
            calStatusDate.SelectedDate = DateTime.Now;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {         
        int result = 0, StatusID = 0, DriverID = 0;
        bool IsDriverPresent = false;
        string strRemark = "";
        DateTime dtStatusDate = DateTime.MinValue;

        dtStatusDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

        foreach (GridViewRow row in GridViewVehicle.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string VehicleID = GridViewVehicle.DataKeys[row.RowIndex].Value.ToString();

                    DropDownList ddStatus = (row.FindControl("ddStatus") as DropDownList);
                    DropDownList ddDriver = (row.FindControl("ddDriver") as DropDownList);
                    RadioButtonList rdlPresent = (row.FindControl("rdlPresent") as RadioButtonList);
                    TextBox txtRemark = (row.FindControl("txtRemark") as TextBox);

                    IsDriverPresent = false;

                    if (ddStatus.SelectedIndex > 0)
                    {
                        StatusID = Convert.ToInt16(ddStatus.SelectedValue);
                        DriverID = Convert.ToInt16(ddDriver.SelectedValue);
                        strRemark = txtRemark.Text.Trim();

                        if (DriverID > 0)
                        {
                            if (rdlPresent.SelectedValue.ToLower() == "true")
                            {
                                IsDriverPresent = true;
                            }
                        }
                        
                        result = DBOperations.AddVehicleDailyStatus(Convert.ToInt32(VehicleID), StatusID, dtStatusDate, DriverID, IsDriverPresent, strRemark, LoggedInUser.glUserId);   
                        
                    }
                    
                }//END_IF
            }//END_IF
        }//END_ForEarch

        // IF No Error in New Exbond Quantity Update Invoice Details
        if (result == 0)
        {
            lblError.Text = "Vehicle Status Updated Successfully!";
            lblError.CssClass = "success";

            GridViewVehicle.DataBind();
        }
        else
        {
            GridViewVehicle.DataBind();
        }
    }
    
    protected void txtStatusDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtReportDate = DateTime.MinValue;

        if (txtStatusDate.Text != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

            int Days = DateTime.Compare(dtReportDate, DateTime.Now.Date);

            if (LoggedInUser.glUserId == 1)
            {
                btnSubmit.Visible = true;
            }
            else if (Days == 0)
            {
                btnSubmit.Visible = true;
            }
            else
            {
                btnSubmit.Visible = false;
            }
            
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        calStatusDate.SelectedDate = DateTime.Now;
    }

    #region ExportDataMOnth

    protected void lnkExportMonth_Click(object sender, EventArgs e)
    {
        string strFileName = "Driver_Month_Attendance_" + txtStatusDate.Text.Trim() + ".xls";

        ExportFunctionMonth("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunctionMonth(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvExportMonth.Caption = "Driver_Month_Attendance_ - " + txtStatusDate.Text.Trim();

        gvExportMonth.Visible = true;
        gvExportMonth.DataBind();

        gvExportMonth.Visible = true;

        gvExportMonth.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Export Vehicle Status Month
    protected void lnkExportStatus_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Month_Status_" + txtStatusDate.Text.Trim() + ".xls";

        ExportFunctionStatus("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }
    
    private void ExportFunctionStatus(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvStatusReport.Caption = "Vehicle_Month_Status_ - " + txtStatusDate.Text.Trim();

        gvStatusReport.Visible = true;
        gvStatusReport.DataBind();

        gvStatusReport.Visible = true;

        gvStatusReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}