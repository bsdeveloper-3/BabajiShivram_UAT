using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class Transport_VehicleDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TRId"] == null)
        {
            Response.Redirect("RequestReceived.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Detail";

            int TranRequestId = Convert.ToInt32(Session["TRId"]);

            // Fill Transporter / Vehicle Details

            TruckRequestDetail(TranRequestId);
            DBOperations.FillVehicleType(ddVehicleType);
            DBOperations.FillCompanyByCategory(lstTransport, Convert.ToInt32(EnumCompanyType.Transporter));
            DBOperations.FillTransporterPlaced(ddTransporter, 0, TranRequestId);
        }
    }

    protected void btnADDTransport_Click(object sender, EventArgs e)
    {
        int TransReqID = Convert.ToInt32(Session["TRId"]);

        foreach (ListItem lst in lstTransport.Items)
        {
            if (lst.Selected)
            {
                int TransporterID = Convert.ToInt32(lst.Value);

                int result = DBOperations.AddTransporterPlaced(TransReqID, TransporterID, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Transporter Added Successfully!";
                    lblError.CssClass = "success";
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime";
                    lblError.CssClass = "success";
                }
            }
        }// END_ForEach

        lstPlaced.DataBind();
        DBOperations.FillTransporterPlaced(ddTransporter, 0, TransReqID);
    }

    protected void btnRemoveTransport_Click(object sender, EventArgs e)
    {
        int TransReqID = Convert.ToInt32(Session["TRId"]);

        foreach (ListItem lst in lstPlaced.Items)
        {
            if (lst.Selected)
            {
                int TransporterID = Convert.ToInt32(lst.Value);

                int result = DBOperations.RemoveTransporterPlaced(TransReqID, TransporterID, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Transporter Removed Successfully!";
                    lblError.CssClass = "success";
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime";
                    lblError.CssClass = "success";
                }
            }
        }// END_ForEach

        lstPlaced.DataBind();
        DBOperations.FillTransporterPlaced(ddTransporter, 0, TransReqID);
    }

    private void TruckRequestDetail(int TransportId)
    {

        DataView dvDetail = DBOperations.GetTransportRequestDetail(TransportId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            lblVehRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport - Detail - " + lblTRRefNo.Text;

            lblNoOfPackages.Text = dvDetail.Table.Rows[0]["NoOfPkgs"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();

            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();

            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblDeliveryType.Text = dvDetail.Table.Rows[0]["DeliveryTypeName"].ToString();

            int JobID = 0;

            if (dvDetail.Table.Rows[0]["JobID"] != DBNull.Value)
            {
                JobID = Convert.ToInt32(dvDetail.Table.Rows[0]["JobID"]);
                DataSourceDocument.SelectParameters["JobID"].DefaultValue = JobID.ToString(); ;

                DataSourceDocument.DataBind();
            }

            if (dvDetail.Table.Rows[0]["lType"].ToString() == "1")
            {
                flNewVehicle.Visible = true;
            }
        }
    }

    protected void btnAddVehicle_Click(object sender, EventArgs e)
    {
        int TransReqId = 0, VehicleType = 0, Packages = 0, Con20 = 0, Con40 = 0, TrasnporterID = 0;

        string VehicleNo = "", TransporterName = "", DeliveryFrom = "", DeliveryTo = "";
        DateTime DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue;

        TransReqId = Convert.ToInt32(Session["TRId"]);

        VehicleNo = txtVehicleNo.Text.Trim();
        TransporterName = ddTransporter.SelectedItem.Text;
        DeliveryFrom = txtDeliveryFrom.Text.Trim();
        DeliveryTo = txtDestination.Text.Trim();

        VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);
        TrasnporterID = Convert.ToInt32(ddTransporter.SelectedValue.Trim());

        if (txtDispatchDate.Text.Trim() != "")
        {
            DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
        }
        if (txtDeliveryDate.Text.Trim() != "")
        {
            DeliveryDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim());
        }

        if (txtNoOfPackages.Text.Trim() != "")
            Packages = Convert.ToInt32(txtNoOfPackages.Text.Trim());

        if (txtCount20.Text.Trim() != "")
            Con20 = Convert.ToInt32(txtCount20.Text.Trim());

        if (txtCount40.Text.Trim() != "")
            Con40 = Convert.ToInt32(txtCount40.Text.Trim());


        int result = DBOperations.AddVehiclePlacedDetail(TransReqId, VehicleNo, VehicleType, Packages, Con20, Con40,
            TransporterName, TrasnporterID, DeliveryFrom, DeliveryTo, DispatchDate, DeliveryDate, LoggedInUser.glUserId);
    }

    protected void lnkViewDocument_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Show();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void gvDocumentDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int DocumentID = Convert.ToInt32(e.CommandArgument);

            string DocPath = DBOperations.GetDocumentPath(DocumentID);

            DownloadDocument(DocPath);


            ModalPopupDocument.Show();
        }

    }

    protected void GridViewVehicle_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "expenses")
        {
            hdnRateDetailId.Value = e.CommandArgument.ToString();
            DataView dvDetail = DBOperations.GetVehicleRateDetailByLid(Convert.ToInt32(hdnRateDetailId.Value));
            if (dvDetail.Table.Rows.Count > 0)
            {
                lblVehicleNo.Text = dvDetail.Table.Rows[0]["VehicleNo"].ToString();
                lblVehicleType.Text = dvDetail.Table.Rows[0]["VehicleType"].ToString();
                lblDeliveryFrom.Text = dvDetail.Table.Rows[0]["DeliveryFrom"].ToString();
                lblDeliveryTo.Text = dvDetail.Table.Rows[0]["DeliveryTo"].ToString();
                lblTransporter.Text = dvDetail.Table.Rows[0]["Transporter"].ToString();
                lblDispatchDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["DispatchDate"]).ToString("dd/MM/yyyy");

                if (dvDetail.Table.Rows[0]["VaraiAmount"] != DBNull.Value)
                    txtVaraiAmount.Text = dvDetail.Table.Rows[0]["VaraiAmount"].ToString();
                if (dvDetail.Table.Rows[0]["DetentionAmount"] != DBNull.Value)
                    txtDetentionAmount.Text = dvDetail.Table.Rows[0]["DetentionAmount"].ToString();
                if (dvDetail.Table.Rows[0]["EmptyContRcptCharges"] != DBNull.Value)
                    txtEmptyContCharges.Text = dvDetail.Table.Rows[0]["EmptyContRcptCharges"].ToString();
            }
            mpeExpenses.Show();
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
        int TranRequestId = Convert.ToInt32(Session["TRId"]); ;

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

    private void DownloadDocument(string FilePath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + FilePath);
        }
        else
        {
            ServerPath = ServerPath + FilePath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, FilePath);
        }
        catch (Exception ex)
        {
        }

    }

    #region Expenses
    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpeExpenses.Hide();
        GridViewVehicle.DataBind();
    }

    protected void btnSaveExpenses_Click(object sender, EventArgs e)
    {
        if (hdnRateDetailId.Value != "" && hdnRateDetailId.Value != "0")
        {
            int result = DBOperations.AddVehicleRateExpense(Convert.ToInt32(hdnRateDetailId.Value), txtVaraiAmount.Text.Trim(), txtDetentionAmount.Text.Trim(), txtEmptyContCharges.Text.Trim());
            if (result == 0)
            {
                lblError.Text = "Successfully added vehicle rate expenses.";
                lblError.CssClass = "success";
            }
            else if (result == 2)
            {
                lblError.Text = "Vehicle rate detail not found.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while updating vehicle rate expense.";
                lblError.CssClass = "errorMsg";
            }
        }
        mpeExpenses.Show();
    }
    #endregion
}