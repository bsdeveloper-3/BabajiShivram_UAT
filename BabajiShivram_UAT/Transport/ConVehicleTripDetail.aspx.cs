using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Ionic.Zip;

public partial class Transport_ConVehicleTripDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveExpense);
        ViewState["FreightAmt"] = 0;
        ViewState["VaraiAmt"] = 0;
        ViewState["DetentionAmt"] = 0;
        ViewState["EmptyContReturnAmt"] = 0;
        ViewState["TollCharges"] = 0;
        ViewState["OtherCharges"] = 0;
        ViewState["AdvanceAmt"] = 0;
        ViewState["TotalAmt"] = 0;
        ViewState["SavingAmt"] = 0;
        ViewState["MarketRate"] = 0;

        if (Session["TransReqId"] == null)
        {
            Response.Redirect("TripDetail.aspx");
        }

        if (!IsPostBack)
        {
            int TranRequestId = Convert.ToInt32(Session["TransReqId"]);
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Consolidate Trip Detail";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["TRConsolidateId"] = null;
        Session["TransReqId"] = null;
        Response.Redirect("TripDetail.aspx");
    }

    protected void btnSaveExpense_Click(object sender, EventArgs e)
    {
        int TranRequestId = Convert.ToInt32(Session["TransReqId"]);
        if (TranRequestId > 0)
        {
            bool IsValid = false;
            decimal decFuel = 0m, decFuel2 = 0m, decFuelLiter = 0m, decFuel2Liter = 0m,
                    decTollCharges = 0m, decFineWithoutCleaner = 0m, decXerox = 0m,
                    decVaraiUnloading = 0m, decEmptyContainer = 0m, decParking = 0m, decGarage = 0m, decBhatta = 0m,
                    decODCOverweight = 0m, decOtherCharges = 0m, decDamageContainer = 0m;

            if (txtFuel2.Text.Trim() != "")
            {
                decFuel2 = Convert.ToDecimal(txtFuel2.Text.Trim());
                IsValid = true;
            }
            if (txtFuelLiter2.Text.Trim() != "")
            {
                decFuel2Liter = Convert.ToDecimal(txtFuelLiter2.Text.Trim());
                IsValid = true;
            }
            if (txtTollCharges.Text.Trim() != "")
            {
                decTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
                IsValid = true;
            }
            if (txtWithoutCleaner.Text.Trim() != "")
            {
                decFineWithoutCleaner = Convert.ToDecimal(txtWithoutCleaner.Text.Trim());
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
                int cnt = 0;
                DataSet dsGetDetail = DBOperations.GetTransRateDetailForRequest(TranRequestId);
                if (dsGetDetail != null && dsGetDetail.Tables.Count > 0)
                {
                    for (int i = 0; i < dsGetDetail.Tables[0].Rows.Count; i++)
                    {
                        if (dsGetDetail.Tables[0].Rows[i]["lid"] != DBNull.Value)
                        {
                            int result = DBOperations.TR_AddVehicleRateExpense(Convert.ToInt32(dsGetDetail.Tables[0].Rows[i]["lid"].ToString()), decFuel, decFuelLiter, decFuel2, decFuel2Liter,
                                                 decTollCharges, decFineWithoutCleaner, decXerox, decVaraiUnloading, decEmptyContainer, decParking,
                                                 decGarage, decBhatta, decOtherCharges, decODCOverweight, decDamageContainer, LoggedInUser.glUserId);
                            if (result == 0)
                            {
                                cnt++;
                            }
                            else if (result == 2)
                            {
                                cnt =- 2;
                                lblError.Text = "Expense already added for vehicle no.";
                                lblError.CssClass = "errorMsg";
                            }
                            else
                            {
                                cnt = -1;
                                lblError.Text = "Error while adding expense for vehicle. Please try again later!";
                                lblError.CssClass = "errorMsg";
                                break;
                            }
                        }
                    }
                }

                if (cnt > 0)
                {
                    Response.Redirect("SuccessPage.aspx?Expense=101");
                }
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void ResetControls()
    {
        txtBhatta.Text = "";
        txtDamageContainer.Text = "";
        txtEmptyContainer.Text = "";
        txtFuel2.Text = "";
        txtFuelLiter2.Text = "";
        txtGarage.Text = "";
        txtODCOverweight.Text = "";
        txtOtherCharges.Text = "";
        txtParking.Text = "";
        txtTollCharges.Text = "";
        txtVaraiUnloading.Text = "";
        txtWithoutCleaner.Text = "";
        txtXerox.Text = "";
    }

    #region GRIDVIEW EVENT
    protected void gvVehicleExpense_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "edit")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                hdnLid.Value = lid.ToString();
                DataView dvExpenseDetail = DBOperations.TR_GetVehicleRateExpenseById(lid);
                if (dvExpenseDetail != null)
                {

                }
            }
        }
        else if (e.CommandName.ToLower().Trim() == "delete")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = DBOperations.TR_DeleteVehicleRateExpense(lid, LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted rate expense.";
                    lblError.CssClass = "success";
                }
                else if (result == 2)
                {
                    lblError.Text = "Rate expense does not exists!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while deleting expense for vehicle. Please try again later!";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0, dcTollCharges = 0, dcOtherCharges = 0,
                dcAdvanceAmount = 0, dcTotalAmount = 0, dcMarketRate = 0, dcSavingAmt = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Rate") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Rate"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "VaraiExpense") != DBNull.Value)
            {
                dcVaraiExpenses = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VaraiExpense"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "DetentionAmount") != DBNull.Value)
            {
                dcDetentionAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DetentionAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges") != DBNull.Value)
            {
                dcEmptyContRecdCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            {
                dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            {
                dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "AdvanceAmount") != DBNull.Value)
            {
                dcAdvanceAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AdvanceAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "SavingAmt") != DBNull.Value)
            {
                dcSavingAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SavingAmt"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "MarketBillingRate") != DBNull.Value)
            {
                dcMarketRate = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "MarketBillingRate"));
            }

            ViewState["FreightAmt"] = Convert.ToDecimal(ViewState["FreightAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcOtherCharges;
            ViewState["AdvanceAmt"] = Convert.ToDecimal(ViewState["AdvanceAmt"]) + dcAdvanceAmount;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;
            ViewState["SavingAmt"] = Convert.ToDecimal(ViewState["SavingAmt"]) + dcSavingAmt;
            ViewState["MarketRate"] = Convert.ToDecimal(ViewState["MarketRate"]) + dcMarketRate;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[5].Text = "<b>" + Convert.ToDecimal(ViewState["MarketRate"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[6].Text = "<b>" + Convert.ToDecimal(ViewState["FreightAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[7].Text = "<b>" + Convert.ToDecimal(ViewState["SavingAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[9].Text = "<b>" + Convert.ToDecimal(ViewState["AdvanceAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[10].Text = "<b>" + Convert.ToDecimal(ViewState["DetentionAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[11].Text = "<b>" + Convert.ToDecimal(ViewState["VaraiAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[12].Text = "<b>" + Convert.ToDecimal(ViewState["EmptyContReturnAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[13].Text = "<b>" + Convert.ToDecimal(ViewState["TollCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[14].Text = "<b>" + Convert.ToDecimal(ViewState["OtherCharges"]).ToString("#,##0.00") + "</b>";
        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int TranRequestId = Convert.ToInt32(Session["TransReqId"]);

        Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        string strVehicleNo = lblVehicleNo.Text.Trim();
        int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = 0;// DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, logg.glUserId);
        if (result == 0)
        {
            lblError.Text = "Rate Detail Added Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;
            e.Cancel = true;

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else if (result == 2)
        {
            lblError.Text = "Details Already Updated!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
    }

    #endregion
}