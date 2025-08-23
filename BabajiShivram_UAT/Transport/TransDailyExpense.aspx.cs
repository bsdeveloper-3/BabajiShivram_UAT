using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_TransDailyExpense : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Vehicle Daily Expense";
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExportMonth);
        //
        if (!IsPostBack)
        {
            calStatusDate.SelectedDate = DateTime.Now;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int result = 0; bool IsValid = false;

        decimal decFuel = 0m, decFuel2 = 0m, decFuelLiter = 0m, decFuel2Liter = 0m,
            decTollCharges = 0m, decFineWithoutCleaner = 0m, decXerox = 0m,
            decVaraiUnloading = 0m, decEmptyContainer = 0m, decParking = 0m, decGarage = 0m, decBhatta = 0m,
            decODCOverweight = 0m, decOtherCharges = 0m, decDamageContainer = 0m;
        DateTime dtExpenseDate = DateTime.MinValue;

        dtExpenseDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

        foreach (GridViewRow row in GridViewExpense.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    IsValid = false;
                    decFuel = 0m; decFuel2 = 0m; decFuelLiter = 0m; decFuel2Liter = 0m;
                    decTollCharges = 0m; decFineWithoutCleaner = 0m; decXerox = 0m;
                    decVaraiUnloading = 0m; decEmptyContainer = 0m; decParking = 0m; decGarage = 0m; decBhatta = 0m;
                    decODCOverweight = 0m; decOtherCharges = 0m; decDamageContainer = 0m;

                    string VehicleID = GridViewExpense.DataKeys[row.RowIndex].Value.ToString();

                    TextBox txtFuel                 = (row.FindControl("txtFuel") as TextBox);
                    TextBox txtFuel2                = (row.FindControl("txtFuel2") as TextBox);
                    TextBox txtFuelLiter            = (row.FindControl("txtFuelLiter") as TextBox);
                    TextBox txtFuel2Liter           = (row.FindControl("txtFuel2Liter") as TextBox);
                    TextBox txtTollCharges          = (row.FindControl("txtTollCharges") as TextBox);
                    TextBox txtFineWithoutCleaner   = (row.FindControl("txtWithoutCleaner") as TextBox);
                    TextBox txtXerox                = (row.FindControl("txtXerox") as TextBox);
                    TextBox txtVaraiUnloading       = (row.FindControl("txtVaraiUnloading") as TextBox);
                    TextBox txtEmptyContainer       = (row.FindControl("txtEmptyContainer") as TextBox);
                    TextBox txtParking              = (row.FindControl("txtParking") as TextBox);
                    TextBox txtGarage               = (row.FindControl("txtGarage") as TextBox);
                    TextBox txtBhatta               = (row.FindControl("txtBhatta") as TextBox);
                    TextBox txtODCOverweight        = (row.FindControl("txtODCOverweight") as TextBox);
                    TextBox txtOtherCharges         = (row.FindControl("txtOtherCharges") as TextBox);
                    TextBox txtDamageContainer      = (row.FindControl("txtDamageContainer") as TextBox);

                    if (txtFuel.Text.Trim() != "")
                    {
                        decFuel = Convert.ToDecimal(txtFuel.Text.Trim());
                        IsValid = true;
                    }
                    if (txtFuel2.Text.Trim() != "")
                    {
                        decFuel2 = Convert.ToDecimal(txtFuel2.Text.Trim());
                        IsValid = true;
                    }
                    if (txtFuelLiter.Text.Trim() != "")
                    {
                        decFuelLiter = Convert.ToDecimal(txtFuelLiter.Text.Trim());
                        IsValid = true;
                    }
                    if (txtFuel2Liter.Text.Trim() != "")
                    {
                        decFuel2Liter = Convert.ToDecimal(txtFuel2Liter.Text.Trim());
                        IsValid = true;
                    }

                    if (txtTollCharges.Text.Trim() != "")
                    {
                        decTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
                        IsValid = true;
                    }
                    if (txtFineWithoutCleaner.Text.Trim() != "")
                    {
                        decFineWithoutCleaner = Convert.ToDecimal(txtFineWithoutCleaner.Text.Trim());
                        IsValid = true;
                    }
                    if (txtXerox.Text.Trim() != "")
                    {
                        decXerox = Convert.ToDecimal(txtXerox.Text.Trim());
                        IsValid = true;
                    }
                    if (txtVaraiUnloading.Text.Trim() != "")
                    {
                        decVaraiUnloading = Convert.ToDecimal(txtVaraiUnloading.Text.Trim());
                        IsValid = true;
                    }
                    if (txtEmptyContainer.Text.Trim() != "")
                    {
                        decEmptyContainer = Convert.ToDecimal(txtEmptyContainer.Text.Trim());
                        IsValid = true;
                    }
                    if (txtParking.Text.Trim() != "")
                    {
                        decParking = Convert.ToDecimal(txtParking.Text.Trim());
                        IsValid = true;
                    }
                    if (txtGarage.Text.Trim() != "")
                    {
                        decGarage = Convert.ToDecimal(txtGarage.Text.Trim());
                        IsValid = true;
                    }
                    if (txtBhatta.Text.Trim() != "")
                    {
                        decBhatta = Convert.ToDecimal(txtBhatta.Text.Trim());
                        IsValid = true;
                    }
                    if (txtODCOverweight.Text.Trim() != "")
                    {
                        decODCOverweight = Convert.ToDecimal(txtODCOverweight.Text.Trim());
                        IsValid = true;
                    }
                    if (txtOtherCharges.Text.Trim() != "")
                    {
                        decOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());
                        IsValid = true;
                    }
                    if (txtDamageContainer.Text.Trim() != "")
                    {
                        decDamageContainer = Convert.ToDecimal(txtDamageContainer.Text.Trim());
                        IsValid = true;
                    }

                    if (IsValid == true)// Atleast one Expense updated
                    {
                        result = DBOperations.AddVehicleDailyExpense(Convert.ToInt32(VehicleID), dtExpenseDate, decFuel, decFuel2, decFuelLiter, decFuel2Liter,
                            decTollCharges, decFineWithoutCleaner, decXerox, decVaraiUnloading, decEmptyContainer, decParking, 
                            decGarage, decBhatta, decODCOverweight, decOtherCharges, decDamageContainer, LoggedInUser.glUserId);

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

        // IF No Error in New Exbond Quantity Update Invoice Details
        if (result == 0)
        {
            lblError.Text = "Vehicle Daily Expense Updated Successfully!";
            lblError.CssClass = "success";

            GridViewExpense.DataBind();
        }
    }
    protected void txtStatusDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtReportDate = DateTime.MinValue;

        if (txtStatusDate.Text != "")
        {
            dtReportDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

            TimeSpan ts = dtReportDate.Subtract(DateTime.Now);

            int days = ts.Days;

            if (days >= -30 && days <= 0)
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        calStatusDate.SelectedDate = DateTime.Now;
    }

    #region Export Data Daily

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Vehicle_Daily_Expense_" + txtStatusDate.Text.Trim() + ".xls";

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

        gvExport.Caption = "Vehicle Daily Expense Details - " + txtStatusDate.Text.Trim();

        gvExport.Visible = true;
        gvExport.DataBind();

        gvExport.Visible = true;

        gvExport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Export Data Month

    protected void lnkExportMonth_Click(object sender, EventArgs e)
    {

        string strFileName = "Vehicle Month Expense-" + Commonfunctions.CDateTime(txtStatusDate.Text.Trim()).ToString("MMM/yyyy")+".xls";

        ExportFunctionMonth("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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

        gvExportMonth.Caption = "Vehicle Month Expense Details - " + Commonfunctions.CDateTime(txtStatusDate.Text.Trim()).ToString("MMM/yyyy");

        gvExportMonth.Visible = true;
        gvExportMonth.DataBind();

        gvExportMonth.Visible = true;

        gvExportMonth.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

}