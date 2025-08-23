using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_HOVehicleLog : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vehicle Daily Log";

        Label lblQuickLink = (Label)Page.Master.FindControl("lblQuickLink"); 
        DropDownList ddlQuickLink = (DropDownList)Page.Master.FindControl("ddlQuickLink");

        if (ddlQuickLink != null)
        {
            ddlQuickLink.Visible = false;
        }
        if (lblQuickLink != null)
        {
            lblQuickLink.Visible = false;
        }


        //
        if (!IsPostBack)
        {
            calStatusDate.SelectedDate = DateTime.Now;
        }
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int result = 0; bool IsValid = false; bool bOneRecord = false;

        String strDriver = "", strEmployee = "", strLocation = "", strOutTime = "",
            strInTime = "", strOpenReading = "", strCloseReading = "", strFuel = "", strAmount = "", strFuelType = "";

        DateTime dtLogDate = DateTime.MinValue;

        dtLogDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

        foreach (GridViewRow row in GridViewExpense.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    IsValid = false;
                    strDriver = ""; strEmployee = ""; strLocation = ""; strOutTime = ""; strInTime = "";
                    strOpenReading = ""; strCloseReading = ""; strFuel = ""; strAmount = "";

                    string VehicleID = GridViewExpense.DataKeys[row.RowIndex].Values[0].ToString();
                    string LogID = GridViewExpense.DataKeys[row.RowIndex].Values[1].ToString();

                    TextBox txtDriver       = (row.FindControl("txtDriver") as TextBox);
                    TextBox txtEmployee     = (row.FindControl("txtEmployee") as TextBox);
                    TextBox txtLocation     = (row.FindControl("txtLocation") as TextBox);
                    TextBox txtOutTime      = (row.FindControl("txtOutTime") as TextBox);
                    TextBox txtInTime       = (row.FindControl("txtInTime") as TextBox);
                    TextBox txtOpenReading  = (row.FindControl("txtOpenReading") as TextBox);
                    TextBox txtCloseReading = (row.FindControl("txtCloseReading") as TextBox);
                    TextBox txtFuel         = (row.FindControl("txtFuel") as TextBox);
                    TextBox txtAmount       = (row.FindControl("txtAmount") as TextBox);
                    DropDownList ddFuelType = (row.FindControl("ddFuelType") as DropDownList);

                    if (txtDriver.Text.Trim() != "")
                    {
                        strDriver = txtDriver.Text.Trim();
                        IsValid = true;
                    }
                    if (txtOutTime.Text.Trim() != "")
                    {
                        strOutTime = txtOutTime.Text.Trim();
                        IsValid = true;
                    }
                    if (txtFuel.Text.Trim() != "")
                    {
                        strFuel = txtFuel.Text.Trim();
                    }

                    strEmployee     =   txtEmployee.Text.Trim();
                    strLocation     =   txtLocation.Text.Trim();
                    
                    strInTime       =   txtInTime.Text.Trim();
                    strOpenReading  =   txtOpenReading.Text.Trim();
                    strCloseReading =   txtCloseReading.Text.Trim();
                    
                    strAmount       =   txtAmount.Text.Trim();
                    strFuelType     =   ddFuelType.SelectedValue;

                    if (IsValid == true)// Atleast one record updated
                    {
                        bOneRecord = true;

                        result =  DBOperations.AddVehicleTravelLog(Convert.ToInt32(VehicleID), Convert.ToInt32(LogID), dtLogDate, strDriver, strEmployee, 
                            strLocation, strOutTime, strInTime,strOpenReading, strCloseReading, strFuel, strAmount, strFuelType, LoggedInUser.glUserId);

                    }
                }
                catch (Exception ex)
                {
                    result = 1;
                    lblError.Text = ex.Message;
                    lblError.CssClass = "errorMsg";
                    break;

                }
            }//END_IF
        }//END_ForEarch

        // Atleast One Vehicle Required For Updated
        if (result == 0 && bOneRecord)
        {
            lblError.Text = "Vehicle Daily Log Updated Successfully!";
            lblError.CssClass = "success";

            GridViewExpense.DataBind();
        }
        else if (bOneRecord == false)
        {
            lblError.Text = "Please Enter Vehicle Driver Name/Out Time Detail!";
            lblError.CssClass = "errorMsg";
            GridViewExpense.DataBind();
        }
    }

    protected void txtStatusDate_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        DateTime dtReportDate = DateTime.MinValue;

        if (txtStatusDate.Text != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

            TimeSpan ts = dtReportDate.Subtract(DateTime.Now);

            int days = ts.Days;

            if (LoggedInUser.glUserId > 1)
            {
                if (days >= -4 && days <= 0)
                {
                    btnSubmit.Visible = true;

                    GridViewExpense.Enabled = true;
                }
                else
                {
                    GridViewExpense.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        calStatusDate.SelectedDate = DateTime.Now;
    }
    
    #region Additional Trip Detail

    protected void btnSaveAdditional_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        String strDriver = "", strEmployee = "", strLocation = "", strOutTime = "",
            strInTime = "", strOpenReading = "", strCloseReading = "", strFuel = "", strAmount = "", strFuelType = "";
        
        DateTime dtLogDate = DateTime.MinValue;

        dtLogDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());
    
        strDriver = ""; strEmployee = ""; strLocation = ""; strOutTime = ""; strInTime = "";
        strOpenReading = ""; strCloseReading = ""; strFuel = ""; strAmount = "";

        string VehicleID = ddNewVehicleNo.SelectedValue;
        string LogID = "0";
                
        strDriver = txtNewDriverName.Text.Trim();
           
        strFuel = txtNewFuel.Text.Trim();
       
        strEmployee     =   txtNewEmployee.Text.Trim();
        strLocation     =   txtNewLocation.Text.Trim();
        strOutTime      =   txtNewOutTime.Text.Trim();
        strInTime       =   txtNewInTime.Text.Trim();
        strOpenReading  =   txtNewOpenReading.Text.Trim();
        strCloseReading =   txtNewCloseReading.Text.Trim();

        strAmount   = txtNewAmount.Text.Trim();
        strFuelType = ddNewFuelType.SelectedValue;

         int   result = DBOperations.AddVehicleTravelLog(Convert.ToInt32(VehicleID), Convert.ToInt32(LogID), dtLogDate, strDriver, strEmployee,
                strLocation, strOutTime, strInTime, strOpenReading, strCloseReading, strFuel, strAmount, strFuelType, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Vehicle Daily Log Updated Successfully!";
            lblError.CssClass = "success";

            GridViewExpense.DataBind();

            ddNewVehicleNo.SelectedIndex = 0;
            txtNewDriverName.Text = "";
            txtNewEmployee.Text = "";
            txtNewLocation.Text = "";
            txtNewOutTime.Text = "";
            txtNewInTime.Text = "";
            txtNewOpenReading.Text = "";
            txtNewCloseReading.Text = "";
            txtNewFuel.Text = "";
            txtNewAmount.Text = "";
        }
        else
        {
            lblError.Text = "System Error Please Try After SomeTime!";
            lblError.CssClass = "errorMsg";
            GridViewExpense.DataBind();
        }

    }
    
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ModalPopupNewTrip.Hide();
    }
    #endregion

}