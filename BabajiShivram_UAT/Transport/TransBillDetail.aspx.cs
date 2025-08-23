using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Transport_TransBillDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TransReqId"] == null)
        {
            Response.Redirect("TransBill.aspx");
        }

        if (!IsPostBack)
        {
            int TranRequestId = Convert.ToInt32(Session["TransReqId"]);
            int TransporterID = Convert.ToInt32(Session["TransporterID"]);

            // Fill Vehicle Details

            TruckRequestDetail(TranRequestId, TransporterID);

        }
    }

    private void TruckRequestDetail(int TranRequestId, int TransporterID)
    {

        DataView dvDetail = DBOperations.GetTransportRequestDetail(TranRequestId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Bill Detail - " + lblTRRefNo.Text;

            lblNoOfPackages.Text = dvDetail.Table.Rows[0]["NoOfPkgs"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();

            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();

            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();

            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["TransReqId"] = null;
        Response.Redirect("TransBill.aspx");
    }

    protected void btnBillSubmit_Click(object sender, EventArgs e)
    {
        int TransReqId = 0, TransporterID = 0, TransitDays = 0;
        string BillNumber = "", BillAmount = "", DetentionAmount = "", VaraiAmount = "",
                EmptyContRcptCharges = "", TotalAmount = "", BillPersonName = "";
        DateTime BillSubmitDate = DateTime.MinValue, BillDate = DateTime.MinValue;
        bool IsValid = true;

        TransReqId = Convert.ToInt32(Session["TransReqId"]);
        TransporterID = Convert.ToInt16(ddTransporter.SelectedValue);
        BillNumber = txtBillNo.Text.Trim();

        BillAmount = txtBillAmount.Text.Trim();
        VaraiAmount = txtVaraiExp.Text.Trim();
        DetentionAmount = txtDetentionAmount.Text.Trim();
        EmptyContRcptCharges = txtEmptyContCharges.Text.Trim();
        TotalAmount = txtTotalAmount.Text.Trim();
        BillPersonName = txtBillingEmpoyee.Text.Trim();

        BillSubmitDate = Commonfunctions.CDateTime(txtBillSubmitDate.Text.Trim());
        BillDate = Commonfunctions.CDateTime(txtBillSubmitDate.Text.Trim());

        int result = DBOperations.AddTransBillDetail(TransReqId, TransporterID, TransitDays, BillSubmitDate, BillNumber, BillDate,
            BillAmount, DetentionAmount, VaraiAmount, EmptyContRcptCharges, TotalAmount, BillPersonName, IsValid, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Bill Detail Updated Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;

            GridViewBillDetail.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Billing Detail Already Updated For Transporter - " + ddTransporter.SelectedItem.Text;
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    #region GridView Event
    protected void GridViewVehicle_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {

        }
    }

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int TranRequestId = Convert.ToInt32(Session["TransReqId"]); ;

        Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        string strVehicleNo = lblVehicleNo.Text.Trim();
        int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, LoggedInUser.glUserId);

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