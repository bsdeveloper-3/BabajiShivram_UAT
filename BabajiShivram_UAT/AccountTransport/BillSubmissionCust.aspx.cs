using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;

public partial class AccountTransport_BillSubmissionCust : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LoggedInUser.glUserId == 0 || Session["TransReqId"] == null)
        {
            Response.Redirect("PendingCustBill.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Bill Detail";
            if (Session["TransReqId"] != null)
            {
                TruckRequestDetail(Convert.ToInt32(Session["TransReqId"]));
                JobDetailMS(Convert.ToInt32(Session["JobId"]));
            }
            else
            {
                Session["TransReqId"] = null;
                Session["JobId"] = null;
                Session["TransporterId"] = null;
                Response.Redirect("PendingCustBill.aspx");
            }
        }
    }
    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequest(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            hdnJobId.Value = dvDetail.Table.Rows[0]["JobId"].ToString();
            Session["JobId"] = dvDetail.Table.Rows[0]["JobId"].ToString();

            //lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            //Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            //lblTitle.Text = "Bill Detail - " + lblTRRefNo.Text;

            //lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            //lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            //lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            //if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
            //    lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            //lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
            //lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
            //lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            //lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            //lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
            //lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            //lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            //lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            //lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            //lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            //lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
            //GridViewVehicle.DataBind();
        }
    }
    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = DBOperations.GetJobDetailForTransport(JobId);
        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Session["TransReqId"] = null;
            Session["JobId"] = null;
            Session["TransporterId"] = null;

            Response.Redirect("PendingTransBill.aspx");

        }

        lblTitle.Text = "Customer Bill Detail - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();

        lblJobNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
        lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
        lblDestination.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryDestination"].ToString();

        if(dsJobDetail.Tables[0].Rows[0]["IsTransBillToBabaji"] != DBNull.Value)
        {
            if(Convert.ToBoolean(dsJobDetail.Tables[0].Rows[0]["IsTransBillToBabaji"]))
            {
                lblTransportBillTo.Text = "Babaji";
            }
            else
            {
                lblTransportBillTo.Text = "Customer";
            }
        }
        else
        {
            lblTransportBillTo.Text = "** Required Field";
            lblTransportBillTo.ForeColor = Color.Red;

            lblError.Text = "Bill To Field Not Updated for Customer!  Please Contact IT Dept!";
            lblError.CssClass = "errorMsg";

            btnBillSubmit.Visible = false;
        }
    }
    protected void btnBackToJobDet_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PendingCustBill.aspx");
    }

    #region Bill Submission
    protected void btnBillSubmit_Click(object sender, EventArgs e)
    {        
        int TransReqId = Convert.ToInt32(Session["TransReqId"]);
        int JobId = Convert.ToInt32(hdnJobId.Value);

        bool IsBillToBabaji = false;

        if (TransReqId > 0 && JobId>0)   
        {
            int resultRate = -1;
                                    
            string Remark = "";
            Decimal FreightRateTotal = 0, DetentionTotal = 0, WaraiTotal = 0, EmptyTotal = 0;
            Decimal TollTotal = 0, UnionTotal = 0;
            Decimal TotalAmount = 0;
                                    
            // Add Customer Billing Total

            int BillID = TransOperation.AddCustomerBillMS(TransReqId, JobId, FreightRateTotal, DetentionTotal, EmptyTotal, WaraiTotal, TotalAmount, IsBillToBabaji, Remark, LoggedInUser.glUserId);

            if (BillID > 0)
            {
                foreach (GridViewRow gr in GridViewVehicle.Rows)
                {

                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        int RateId = 0;
                        string VehicleNo = "", VehicleRemark = "", FreightChallanNo = "";
                        string Dest_From = "", Dest_To = "";
                        Decimal FreightExp = 0, WaraiExp = 0, DetentionCharges = 0, EmptyCharges = 0;
                        Decimal TollCharges = 0, UnionCharges = 0;

                        DateTime FreightChallanDate = DateTime.MinValue;

                        if (Session["TransReqId"] != null)
                        {

                            RateId = Convert.ToInt32(GridViewVehicle.DataKeys[gr.RowIndex].Value);
                            
                            VehicleNo = ((Label)gr.FindControl("lblVehicleNo")).Text;

                            Decimal.TryParse(((TextBox)gr.FindControl("lblFreightAmount")).Text.Trim(), out FreightExp);
                            Decimal.TryParse(((TextBox)gr.FindControl("txtDetention")).Text.Trim(), out DetentionCharges);
                            Decimal.TryParse(((TextBox)gr.FindControl("txtVarai")).Text.Trim(), out WaraiExp);
                            Decimal.TryParse(((TextBox)gr.FindControl("txtEmptyContRecpt")).Text.Trim(), out EmptyCharges);
                            Decimal.TryParse(((TextBox)gr.FindControl("txtToll")).Text.Trim(), out TollCharges);
                            Decimal.TryParse(((TextBox)gr.FindControl("txtUnion")).Text.Trim(), out UnionCharges);

                            FreightChallanNo = ((TextBox)gr.FindControl("txtChallanNo")).Text.Trim();
                            Dest_From = ((TextBox)gr.FindControl("txtFrom")).Text.Trim();
                            Dest_To = ((TextBox)gr.FindControl("txtTo")).Text.Trim();
                            VehicleRemark = ((TextBox)gr.FindControl("txtRemark")).Text.Trim();

                            if (((TextBox)gr.FindControl("txtChallanDate")).Text.Trim() != "")
                            {
                                FreightChallanDate = Commonfunctions.CDateTime(((TextBox)gr.FindControl("txtChallanDate")).Text.Trim());
                            }

                            FreightRateTotal =  FreightRateTotal + FreightExp;
                            DetentionTotal  =   DetentionTotal + DetentionCharges;
                            WaraiTotal      =   WaraiTotal + WaraiExp;
                            EmptyTotal      =   EmptyTotal + EmptyCharges;
                            TollTotal       =   TollTotal + TollCharges;
                            UnionTotal      =   UnionTotal + UnionCharges;

                            TotalAmount = FreightRateTotal + DetentionTotal + WaraiTotal + EmptyTotal + TollTotal + UnionTotal;

                            resultRate = TransOperation.AddCustomerBillDetail(BillID, TransReqId, RateId, VehicleNo, FreightExp, DetentionCharges, WaraiExp,
                                EmptyCharges, TollCharges, UnionCharges, FreightChallanNo, FreightChallanDate, Dest_From, Dest_To, VehicleRemark, LoggedInUser.glUserId);

                            if (resultRate == 0)
                            {
                                lblError.Text = "Successfully updated rate detail.";
                                lblError.CssClass = "success";
                            }
                            else if (resultRate == 2)
                            {
                                lblError.Text = "Rate detail does not exists.";
                                lblError.CssClass = "errorMsg";
                            }
                            else
                            {
                                lblError.Text = "System error. Please try again later.";
                                lblError.CssClass = "errorMsg";
                            }
                        }
                    }//END_IF
                }

                if (resultRate == 0)
                {
                    int result_BillID = TransOperation.AddCustomerBillMS(TransReqId, JobId, FreightRateTotal, DetentionTotal, EmptyTotal, WaraiTotal, TotalAmount, IsBillToBabaji, Remark, LoggedInUser.glUserId);

                    if (result_BillID > 0)
                    {
                        // Send Bill to Transport Dept Approval

                        lblError.Text = "Customer Bill Detail Added Successfully.!";
                        lblError.CssClass = "success";

                        Session["Success"] = "Bill Detail Added Successfully.!";

                        Response.Redirect("TransportSuccess.aspx");

                    }
                    else if (result_BillID == -1)
                    {
                        lblError.Text = "System Error! Please Try After Sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (result_BillID == -2)
                    {
                        lblError.Text = "Billing Detail Already Updated!";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (result_BillID == -3)
                    {
                        lblError.Text = "Billing Detail Already Submitted!";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please Try After Sometime.";
                        lblError.CssClass = "errorMsg";
                    }

                }
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Session Expired!";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnCancelBill_Click(object sender, EventArgs e)
    {
        Session["TransReqId"] = null;
        Session["JobId"] = null;
        Session["TransporterID"] = null;

        Response.Redirect("PendingCustBill.aspx");

    }
    protected void GetTotalAmount()
    {
        decimal dcFreightAmt = 0, dcDetentionAmt = 0, dcVaraiExpense = 0, dcEmptyContCharges = 0, dcAdvanceAmt = 0, dcTollCharges = 0, dcOtherCharges = 0;
        decimal dcActualFreightAmt = 0, dcActualDetentionAmt = 0, dcActualVaraiAmt = 0, dcActualEmptyContAmt = 0, dcActualTollCharges = 0, dcActualOtherCharges = 0;

        // set page is valid or not
        if (hdnFreightAmt.Value != "" && hdnFreightAmt.Value != "0")
            dcActualFreightAmt = Convert.ToDecimal(hdnFreightAmt.Value);
        if (hdnDetentionAmt.Value != "" && hdnDetentionAmt.Value != "0")
            dcActualDetentionAmt = Convert.ToDecimal(hdnDetentionAmt.Value);
        if (hdnVaraiAmt.Value != "" && hdnVaraiAmt.Value != "0")
            dcActualVaraiAmt = Convert.ToDecimal(hdnVaraiAmt.Value);
        if (hdnEmptyContReturnAmt.Value != "" && hdnEmptyContReturnAmt.Value != "0")
            dcActualEmptyContAmt = Convert.ToDecimal(hdnEmptyContReturnAmt.Value);

        //if (hdnTollCharges.Value != "" && hdnTollCharges.Value != "0")
        //    dcActualTollCharges = Convert.ToDecimal(hdnTollCharges.Value);
        //if (hdnOtherCharges.Value != "" && hdnOtherCharges.Value != "0")
        //    dcActualOtherCharges = Convert.ToDecimal(hdnOtherCharges.Value);
                
        //   txtTotalAmount.Text = Convert.ToDecimal((dcFreightAmt + dcDetentionAmt + dcVaraiExpense + dcEmptyContCharges + dcTollCharges + dcOtherCharges) - (dcAdvanceAmt)).ToString();
        //   txtTotalAmount.Enabled = false;
    }
    protected string RandomString(int size)
    {

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
    #endregion

    #region GridView Event
    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0,
            dcTollCharges = 0, dcOtherCharges = 0, dcAdvanceAmount = 0, dcTotalAmount = 0;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "FreightAmount") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FreightAmount"));
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

            //if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            //{
            //    dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            //}

            //if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            //{
            //    dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            //}

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            ViewState["FreightAmt"] = Convert.ToDecimal(ViewState["FreightAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcOtherCharges;
            ViewState["AdvanceAmt"] = Convert.ToDecimal(ViewState["AdvanceAmt"]) + dcAdvanceAmount;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;

            hdnFreightAmt.Value = Convert.ToString(ViewState["FreightAmt"]);
            hdnDetentionAmt.Value = Convert.ToString(ViewState["DetentionAmt"]);
            hdnVaraiAmt.Value = Convert.ToString(ViewState["VaraiAmt"]);
            hdnEmptyContReturnAmt.Value = Convert.ToString(ViewState["EmptyContReturnAmt"]);
            hdnTollCharges.Value = Convert.ToString(ViewState["TollCharges"]);
            hdnOtherCharges.Value = Convert.ToString(ViewState["OtherCharges"]);
            //hdnAdvanceAmt.Value = Convert.ToString(ViewState["AdvanceAmt"]);
            hdnTotalAmount.Value = Convert.ToString(ViewState["TotalAmt"]);

        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[3].Text = "<b>" + ViewState["FreightAmt"].ToString() + "</b>";
            e.Row.Cells[4].Text = "<b>" + ViewState["DetentionAmt"].ToString() + "</b>";
            e.Row.Cells[5].Text = "<b>" + ViewState["VaraiAmt"].ToString() + "</b>";
            e.Row.Cells[6].Text = "<b>" + ViewState["EmptyContReturnAmt"].ToString() + "</b>";
            //e.Row.Cells[7].Text = "<b>" + ViewState["TollCharges"].ToString() + "</b>";
            //e.Row.Cells[8].Text = "<b>" + ViewState["OtherCharges"].ToString() + "</b>";
        }
    }
    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }
    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        //int TranRequestId = Convert.ToInt32(Session["TransReqId"]);

        //Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        //TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        //string strVehicleNo = lblVehicleNo.Text.Trim();
        //int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = 0;// DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, LoggedInUser.glUserId);
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