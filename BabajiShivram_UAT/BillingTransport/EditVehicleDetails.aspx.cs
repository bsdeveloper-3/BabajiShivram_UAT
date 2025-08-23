using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class BillingTransport_EditVehicleDetails : System.Web.UI.Page
{
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle Detail";

            DBOperations.FillVehicleType(ddVehicleType);
            fsEditDelivery.Visible = false;
            fsVehicleDetail.Visible = false;
            fsAllVehicles.Visible = true;
            btnBackToVehicleDetail.Visible = false;
        }
    }

    protected void btnBackToJobDet_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    protected void btnBackToVehicleDetail_OnClick(object sender, EventArgs e)
    {
        ResetControls();
        fsVehicleDetail.Visible = false;
        fsEditDelivery.Visible = false;
        fsAllVehicles.Visible = true;
        btnBackToVehicleDetail.Visible = false;
        if (Convert.ToString(Session["JobId"]) != "")
            GridViewDelivery.DataBind();
        GridViewDelivery.Visible = true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
        fsVehicleDetail.Visible = false;
        fsEditDelivery.Visible = false;
        fsAllVehicles.Visible = true;
        btnBackToVehicleDetail.Visible = false;
        if (Convert.ToString(Session["JobId"]) != "")
            GridViewDelivery.DataBind();
        GridViewDelivery.Visible = true;
    }

    protected void btnUpdateDelivery_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(Session["JobId"]) != "" && lblDeliveryLid.Text != "")
            {
                int JobId = 0, DeliveryId = 0, Packages = 0, ContainerId = 0, VehicleType = 0, DetentionDays = 0, BillRequestId = 0;

                DateTime dtReportDate = DateTime.MinValue;
                DateTime dtUnloadDate = DateTime.MinValue;
                DateTime dtDeliveredDate = DateTime.MinValue;
                DateTime dtEmptyReturnDate = DateTime.MinValue;
                DateTime dtDispatchDate = DateTime.MinValue;

                if (txtReportDate.Text.Trim() != "")
                    dtReportDate = Commonfunctions.CDateTime(txtReportDate.Text.Trim());
                if (txtUnloadDate.Text.Trim() != "")
                    dtUnloadDate = Commonfunctions.CDateTime(txtUnloadDate.Text.Trim());
                if (txtDeliveryDate.Text.Trim() != "")
                    dtDeliveredDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim());
                if (txtEmptyReturnDate.Text.Trim() != "")
                    dtEmptyReturnDate = Commonfunctions.CDateTime(txtEmptyReturnDate.Text.Trim());
                if (lblDispatchDate.Text.Trim() != "")
                    dtDispatchDate = Commonfunctions.CDateTime(lblDispatchDate.Text.Trim());

                //if (txtContainerId.Text != "" && txtContainerId.Text != "0")
                //    ContainerId = Convert.ToInt32(txtContainerId.Text);
                JobId = Convert.ToInt32(Session["JobId"]);
                DeliveryId = Convert.ToInt32(lblDeliveryLid.Text);
                if (ddVehicleType.SelectedValue != "0")
                    VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);
                if (txtDetentionDay.Text != "")
                    DetentionDays = Convert.ToInt32(txtDetentionDay.Text);

                #region CALCULATE TOTAL

                double DetentionCharges = 0.0, OtherCharges = 0.0, Total = 0.0, FrightRate = 0.0, VaraiCharges = 0.0, EmptyOffLoadingCharges = 0.0,
                       TempoUnionCharges = 0.0;

                if (txtFrightRate.Text != "")                                                       // TP Fright Rate
                    FrightRate = Convert.ToDouble(txtFrightRate.Text);
                if (txtDetentionCharges.Text != "")                                                 // Detention Charges
                    DetentionCharges = Convert.ToDouble(txtDetentionCharges.Text);
                if (txtVaraiCharges.Text != "")                                                     // Varai Charges
                    VaraiCharges = Convert.ToDouble(txtVaraiCharges.Text);
                if (txtEmptyOffLoadingCharges.Text != "")                                           // Empty Of Loading Charges
                    EmptyOffLoadingCharges = Convert.ToDouble(txtEmptyOffLoadingCharges.Text);
                if (txtTempoUnionCharges.Text != "")                                                // Tempo Union charges
                    TempoUnionCharges = Convert.ToDouble(txtTempoUnionCharges.Text);
                OtherCharges = VaraiCharges + EmptyOffLoadingCharges + TempoUnionCharges;           // Other Charges = Varai + Empty Of Loading + Tempo Union

                Total = Convert.ToDouble((DetentionCharges + OtherCharges) * FrightRate * Convert.ToDouble(DetentionDays));
                // Total of both w.r.t. fright rate and detention days

                #endregion

                #region DOC UPLOAD

                string strCustDocFolder = "", strJobFileDir = "", strFilePath_LRCopies = "", strUploadPath = "", strFilePath_Receipt = "", strFilePath_Invoice = ""; ;
                DataSet dsJobDetail = DBOperations.GetJobDetail(Convert.ToInt32(Session["JobId"]));                   // Get Job Detail
                if (dsJobDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsJobDetail.Tables[0].Rows[0]["DocFolder"] != DBNull.Value)
                        strCustDocFolder = dsJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\";

                    if (dsJobDetail.Tables[0].Rows[0]["FileDirName"] != DBNull.Value)
                        strJobFileDir = dsJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
                    strUploadPath = strCustDocFolder + strJobFileDir;
                }

                if (fuUploadLrCopies.HasFile != false && fuUploadLrCopies.FileName.Trim() != "")
                    strFilePath_LRCopies = UploadDocument(strUploadPath, fuUploadLrCopies);
                if (fuUploadReceipt.HasFile != false && fuUploadReceipt.FileName.Trim() != "")
                    strFilePath_Receipt = UploadDocument(strUploadPath, fuUploadReceipt);

                #endregion

                int result = DBOperations.AddTransportVehicleDetail(JobId, lblTransporterName.Text.Trim(), DeliveryId, Packages, 0,
                             lblVehicleNo.Text.Trim(), Convert.ToInt32(ddVehicleType.SelectedValue), lblDeliveryFrom.Text.Trim(), lblDestination.Text.Trim(),
                             dtDispatchDate, dtDeliveredDate, Convert.ToInt32(Session["VendorId"]), txtFrightRate.Text.Trim(), dtReportDate, dtUnloadDate,
                             DetentionDays, DetentionCharges.ToString(), VaraiCharges.ToString(), EmptyOffLoadingCharges.ToString(), TempoUnionCharges.ToString(),
                             Total.ToString(), txtRemarks.Text.Trim(), dtEmptyReturnDate, strFilePath_LRCopies, strFilePath_Receipt);
                if (result > 0)
                {
                    DataSet dsGetBillReqDetail = new DataSet();
                    dsGetBillReqDetail = DBOperations.GetBillRequestDetail(Convert.ToInt32(JobId));
                    if (dsGetBillReqDetail != null && dsGetBillReqDetail.Tables[0].Rows.Count > 0)
                    {
                        if (dsGetBillReqDetail.Tables[0].Rows[0]["lid"].ToString() != "")
                        {
                            BillRequestId = Convert.ToInt32(dsGetBillReqDetail.Tables[0].Rows[0]["lid"].ToString());
                        }
                    }

                    hdnDeliveryLid.Value = result.ToString();
                    if (gvJobDetail != null && gvJobDetail.Rows.Count > 0)
                    {
                        int ConsBillJob = -5;
                        for (int i = 0; i < gvJobDetail.Rows.Count; i++)
                        {
                            Label lblJobId = (Label)gvJobDetail.Rows[i].FindControl("lblJobId");
                            if (hdnDeliveryLid.Value != "" && lblJobId.Text != "")
                            {
                                ConsBillJob = DBOperations.AddConsolidateJobBill(Convert.ToInt32(hdnDeliveryLid.Value), Convert.ToInt32(lblJobId.Text), BillRequestId, Convert.ToInt32(Session["VendorId"]));
                            }
                        }

                        if (ConsBillJob == 0)
                        {
                            ResetControls();
                            lberror.Text = "Successfully saved billing vehicle details.";
                            lberror.CssClass = "success";
                            fsVehicleDetail.Visible = false;
                            btnBackToVehicleDetail.Visible = false;
                            fsEditDelivery.Visible = false;
                            fsAllVehicles.Visible = true;
                            GridViewDelivery.Visible = true;
                            if (Convert.ToString(Session["JobId"]) != "")
                                GridViewDelivery.DataBind();
                        }
                        else
                        {
                            lberror.Text = "Error while saving billing vehicle details.";
                            lberror.CssClass = "errorMsg";
                        }
                    }
                }
                else if (result == 1)
                {
                    lberror.Text = "Error while saving billing vehicle details.";
                    lberror.CssClass = "errorMsg";
                }
                else
                {
                    lberror.Text = "Record already exists for transporter and vehicle.";
                    lberror.CssClass = "errorMsg";
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void ResetControls()
    {
        lblVehicleType.Text = "";
        // txtContainerId.Text = "";
        // txtContainerNo.Text = "";
        //txtContainerSize.Text = "";
        txtDeliveryDate.Text = "";
        lblDeliveryFrom.Text = "";
        lblDestination.Text = "";
        txtDetentionCharges.Text = "";
        txtDetentionDay.Text = "";
        lblDispatchDate.Text = "";
        txtEmptyReturnDate.Text = "";
        txtFrightRate.Text = "";
        //txtNoOfPackages.Text = "";
        txtVaraiCharges.Text = "";
        txtEmptyOffLoadingCharges.Text = "";
        txtTempoUnionCharges.Text = "";
        txtRemarks.Text = "";
        txtReportDate.Text = "";
        lblTransporterName.Text = "";
        txtUnloadDate.Text = "";
        lblVehicleNo.Text = "";
        ddVehicleType.SelectedValue = "0";
        lblDeliveryLid.Text = "";
        lblTotal.Text = "";
    }

    #region DOCUMENT UPLOAD

    private string UploadDocument(string FilePath, FileUpload fuUpload)
    {
        string FileName = fuUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "BillVehicle_" + Session["JobId"].ToString() + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuUpload.SaveAs(ServerFilePath + FileName);
            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    public string RandomString(int size)
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

    #region GRID VIEW EVENTS

    protected void GridViewDelivery_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewDelivery.DataKeys[gvrow.RowIndex].Values[0].ToString();
            hdnJobId.Value = GridViewDelivery.DataKeys[gvrow.RowIndex].Values[1].ToString();
            Session["JobId"] = GridViewDelivery.DataKeys[gvrow.RowIndex].Values[1].ToString();
            hdnVehicleNo.Value = GridViewDelivery.DataKeys[gvrow.RowIndex].Values[2].ToString();

            if (strlId != "")
            {
                fsVehicleDetail.Visible = true;
                fsEditDelivery.Visible = true;
                fsAllVehicles.Visible = false;
                btnBackToVehicleDetail.Visible = true;

                lblDeliveryLid.Text = strlId;
                DataSet dsGetJobDelDetails = new DataSet();
                dsGetJobDelDetails = DBOperations.GetBS_JobDeliveryDetailAsPerLid(Convert.ToInt32(strlId), Convert.ToInt32(hdnJobId.Value));

                if (dsGetJobDelDetails != null && dsGetJobDelDetails.Tables[0].Rows.Count > 0)
                {
                    fsEditDelivery.Visible = true;
                    fsVehicleDetail.Visible = true;
                    fsAllVehicles.Visible = false;

                    // if (dsGetJobDelDetails.Tables[0].Rows[0]["NoOfPackages"].ToString() != "0")
                    //     txtNoOfPackages.Text = dsGetJobDelDetails.Tables[0].Rows[0]["NoOfPackages"].ToString();
                    // else
                    //     txtNoOfPackages.Text = "0";
                    // txtContainerNo.Text = dsGetJobDelDetails.Tables[0].Rows[0]["ContainerNo"].ToString();
                    // if (dsGetJobDelDetails.Tables[0].Rows[0]["ContainerId"].ToString() != "0")
                    //      txtContainerId.Text = dsGetJobDelDetails.Tables[0].Rows[0]["ContainerId"].ToString();
                    //  txtContainerSize.Text = dsGetJobDelDetails.Tables[0].Rows[0]["ContainerSize"].ToString();
                    lblVehicleNo.Text = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleNo"].ToString();
                    ddVehicleType.SelectedValue = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleType"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["VehicleTypeName"].ToString() != "")
                        lblVehicleType.Text = dsGetJobDelDetails.Tables[0].Rows[0]["VehicleTypeName"].ToString();
                    lblTransporterName.Text = dsGetJobDelDetails.Tables[0].Rows[0]["TransporterName"].ToString();
                    lblDeliveryFrom.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryFrom"].ToString();
                    lblDestination.Text = dsGetJobDelDetails.Tables[0].Rows[0]["DeliveryPoint"].ToString();
                    if (dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"].ToString() != null && dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"].ToString() != "")
                        lblDispatchDate.Text = Convert.ToDateTime(dsGetJobDelDetails.Tables[0].Rows[0]["DispatchDate"]).ToString("dd/MM/yyyy");
                }
            }
        }
    }

    //protected void GridViewDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string strContainerType = (string)DataBinder.Eval(e.Row.DataItem, "ContainerType").ToString();

    //        if (strContainerType == "1")
    //        {
    //            e.Row.Cells[3].Visible = false;
    //        }
    //        else
    //            e.Row.Cells[3].Visible = true;
    //    }
    //}

    #endregion
}