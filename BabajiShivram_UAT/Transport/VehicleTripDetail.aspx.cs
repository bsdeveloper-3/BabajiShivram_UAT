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

public partial class Transport_VehicleTripDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(imgbtnPackingList);
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

        if (Session["RateId"] == null)
        {
            Response.Redirect("TripDetail.aspx");
        }

        if (!IsPostBack)
        {
            int RateId = Convert.ToInt32(Session["RateId"]);
            // Fill Vehicle Details
            TruckRequestDetail(RateId);
        }
    }

    private void TruckRequestDetail(int RateId)
    {
        DataView dvRateDetail = DBOperations.GetTransRateDetailById(RateId);
        if (dvRateDetail != null)
        {
            hdnRateId.Value = RateId.ToString();
            hdnTransReqId.Value = dvRateDetail.Table.Rows[0]["TransReqId"].ToString();

            DataView dvDetail = DBOperations.GetTransportRequestDetail(Convert.ToInt32(hdnTransReqId.Value));
            if (dvDetail.Table.Rows.Count > 0)
            {
                lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Bill Detail - " + lblTRRefNo.Text;

                lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
                lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
                lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
                if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                    lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
                lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
                lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
                lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
                lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
                lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
                lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
                lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
                lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
                lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
                lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
                lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
                //if (dvDetail.Table.Rows[0]["JobId"] != DBNull.Value)
                //    DBOperations.FillVehicleNo(ddVehicleNo, Convert.ToInt32(dvDetail.Table.Rows[0]["JobId"]));

                lblPickAdd.Text = dvDetail.Table.Rows[0]["PickUpAddress"].ToString();
                lblDropAdd.Text = dvDetail.Table.Rows[0]["DropAddress"].ToString();
                lblpickPincode.Text = dvDetail.Table.Rows[0]["PickupPincode"].ToString();           //added new pickup and drop pincode city and state for transport request detail Updated delivery
                lblpickState.Text = dvDetail.Table.Rows[0]["PickupState"].ToString();
                lblpickCity.Text = dvDetail.Table.Rows[0]["PickupCity"].ToString();
                lblDropPincode.Text = dvDetail.Table.Rows[0]["DropPincode"].ToString();
                lblDropState.Text = dvDetail.Table.Rows[0]["DropState"].ToString();
                lblDropCity.Text = dvDetail.Table.Rows[0]["DropCity"].ToString();
                GridViewVehicle.DataBind();
                gvVehicleExpense.DataBind();
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["RateId"] = null;
        Response.Redirect("TripDetail.aspx");
    }

    protected void btnSaveExpense_Click(object sender, EventArgs e)
    {
        if (hdnRateId.Value != "" && hdnRateId.Value != "0")
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
                int result = DBOperations.TR_AddVehicleRateExpense(Convert.ToInt32(hdnRateId.Value), decFuel, decFuelLiter, decFuel2, decFuel2Liter,
                                                             decTollCharges, decFineWithoutCleaner, decXerox, decVaraiUnloading, decEmptyContainer, decParking,
                                                             decGarage, decBhatta, decOtherCharges, decODCOverweight, decDamageContainer, LoggedInUser.glUserId);
                if (result == 0)
                {
                    Response.Redirect("SuccessPage.aspx?Expense=101");
                }
                else if (result == 2)
                {
                    lblError.Text = "Expense already added for vehicle no.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while adding expense for vehicle. Please try again later!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please enter atleast one rate!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Select Vehicle No.";
            lblError.CssClass = "errorMsg";
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
        //txtFuel.Text = "";
        txtFuel2.Text = "";
        //txtFuelLiter.Text = "";
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
                    txtFuel2.Text = dvExpenseDetail.Table.Rows[0]["Fuel2"].ToString();
                    txtFuelLiter2.Text = dvExpenseDetail.Table.Rows[0]["Fuel2Liter"].ToString();
                    txtTollCharges.Text = dvExpenseDetail.Table.Rows[0]["TollCharges"].ToString();
                    txtWithoutCleaner.Text = dvExpenseDetail.Table.Rows[0]["FineWithoutCleaner"].ToString();
                    txtXerox.Text = dvExpenseDetail.Table.Rows[0]["Xerox"].ToString();
                    txtVaraiUnloading.Text = dvExpenseDetail.Table.Rows[0]["VaraiUnloading"].ToString();
                    txtEmptyContainer.Text = dvExpenseDetail.Table.Rows[0]["EmptyContainerReceipt"].ToString();
                    txtParking.Text = dvExpenseDetail.Table.Rows[0]["ParkingGatePass"].ToString();
                    txtGarage.Text = dvExpenseDetail.Table.Rows[0]["Garage"].ToString();
                    txtBhatta.Text = dvExpenseDetail.Table.Rows[0]["Bhatta"].ToString();
                    txtODCOverweight.Text = dvExpenseDetail.Table.Rows[0]["AdditionalChargesForODCOverweight"].ToString();
                    txtOtherCharges.Text = dvExpenseDetail.Table.Rows[0]["OtherCharges"].ToString();
                    txtDamageContainer.Text = dvExpenseDetail.Table.Rows[0]["NakaPassingDamageContainer"].ToString();
                    btnSaveExpense.Text = "Update";
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

    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument(TransRequestId);
        }
    }

    private void DownloadDocument(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "TransportFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion  
}