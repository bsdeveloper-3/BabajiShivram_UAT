using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

public partial class Transport_UpdateDelivery : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);
        if (!IsPostBack)
        {
            ViewState["ClearedStatus"] = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Update Delivery";
            string strJobId = Session["JobId"].ToString();

            JobDetail(Convert.ToInt32(strJobId));
        }
      
        MEditValDispatchDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValReceivedDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValLRDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

        // Container return date & Delivered Date maximum value allowed - Today + One Year
        MEditValReturnDate.MaximumValue = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
        MEditValDeliveredDate.MaximumValue = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
    }

    protected void JobDetail(int JobId)
    {
        DBOperations.FillVehicleType(ddVehicleType);
        DBOperations.FillVehicleForDelivery(ddVehicleNo, JobId);

        // Get new transport job details
        DataView dvJobDetail = DBOperations.GetTransportRequestDetail(JobId);
        if (dvJobDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvJobDetail.Table.Rows[0]["TRRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Update Delivery - " + dvJobDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblTruckRequestDate.Text = Convert.ToDateTime(dvJobDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvJobDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvJobDetail.Table.Rows[0]["CustName"].ToString();
            lblDivision.Text = dvJobDetail.Table.Rows[0]["Division"].ToString();
            lblPlant.Text = dvJobDetail.Table.Rows[0]["Plant"].ToString();
            lblLocationFrom.Text = dvJobDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvJobDetail.Table.Rows[0]["Destination"].ToString();
            lblGrossWeight.Text = dvJobDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvJobDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvJobDetail.Table.Rows[0]["Count40"].ToString();
            lblDeliveryType_Loc.Text = dvJobDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            txtDeliveryPoint.Text = dvJobDetail.Table.Rows[0]["Destination"].ToString();

            lblPickAdd.Text = dvJobDetail.Table.Rows[0]["PickUpAddress"].ToString();
            lblDropAdd.Text = dvJobDetail.Table.Rows[0]["DropAddress"].ToString();
            lblpickPincode.Text = dvJobDetail.Table.Rows[0]["PickupPincode"].ToString();           //added new pickup and drop pincode city and state for transport request detail Updated delivery
            lblpickState.Text = dvJobDetail.Table.Rows[0]["PickupState"].ToString();
            lblpickCity.Text = dvJobDetail.Table.Rows[0]["PickupCity"].ToString();
            lblDropPincode.Text = dvJobDetail.Table.Rows[0]["DropPincode"].ToString();
            lblDropState.Text = dvJobDetail.Table.Rows[0]["DropState"].ToString();
            lblDropCity.Text = dvJobDetail.Table.Rows[0]["DropCity"].ToString();
        }

        // Get job delivery details
        DataView dvDetail = DBOperations.TR_GetJobDetailForDelivery(JobId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            int intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]);
            DBOperations.FillTransporterList(ddTransporter, JobId);
            ddTransporter.Visible = true;
            RFVTransID.Enabled = true;

            intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveredPackages"]);
            lblDeliveredContainer.Text = dvDetail.Table.Rows[0]["DeliveredContainer"].ToString();

            lblBalancePackage.Text = intBalancePackage.ToString() + "/" + dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            hdnBalancePkg.Value = intBalancePackage.ToString();

            lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
            {
                hdnDeliveryTypeId.Value = dvDetail.Table.Rows[0]["DeliveryType"].ToString();
            }

            if (dvDetail.Table.Rows[0]["TransMode"] != DBNull.Value && Convert.ToInt16(dvDetail.Table.Rows[0]["TransMode"]) == 2) //sea
            {
                if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value && Convert.ToInt16(dvDetail.Table.Rows[0]["DeliveryType"]) != 0)
                {
                    btnSubmit.Visible = true;
                    DeliveryType DODeliveryType = (DeliveryType)dvDetail.Table.Rows[0]["DeliveryType"];
                    lblDeliveryType.Text = DODeliveryType.ToString();

                    if (Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveryType"]) == (int)DeliveryType.Loaded)
                    {
                        // Show Container Div
                        DivContainer.Visible = true;
                        DivPackages.Visible = false;
                        RFVNoOfPkgs.Enabled = false;
                        RFVContainer.Enabled = true;
                        DBOperations.TR_GetPendingContainerDetail(ddContainerNo, JobId);
                    }
                    else
                    {
                        // Show Package Div
                        DivContainer.Visible = false;
                        DivPackages.Visible = true;
                        RFVNoOfPkgs.Enabled = true;
                        RFVContainer.Enabled = false;
                    }
                }
                else
                {
                    btnSubmit.Visible = false;
                    lblError.Text = "Delivery Type Detail Not Found!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblDeliveryType.Text = "Air";
                lblDeliveryType_Loc.Text = "Air";
                DivContainer.Visible = false;
                DivPackages.Visible = true;
                RFVNoOfPkgs.Enabled = true;
                RFVContainer.Enabled = false;
            }

            lblCon20.Text = dvDetail.Table.Rows[0]["Con20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Con40"].ToString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Result = -123;
        int JobId = Convert.ToInt32(Session["JobId"]);
        int NoOfPackages = 0, ContainerId = 0, VehicleType = 0;
        int TransporterID = 0;
        string strTransporterName = "", strLRNo, strDeliveryPoint, VehicleNo, CargoReceivedBy, PODPath = "", ChallanPath = "", DamageCopyPath = "",
               strBabajiChallanNo, strDriverName, strDriverPhone;

        DateTime TruckReqDate = DateTime.MinValue, VehicleRcvdDate = DateTime.MinValue, LRDate = DateTime.MinValue, DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
            EmptyContRetrunDate = DateTime.MinValue, RoadPermitDate = DateTime.MinValue, NFormDate = DateTime.MinValue, SFormDate = DateTime.MinValue, NClosingDate = DateTime.MinValue,
            SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue, BabajiChallanDate = DateTime.MinValue, OutOfChargeDate = DateTime.MinValue;

        strDriverName = txtDriverName.Text.Trim();
        strDriverPhone = txtDriverPhone.Text.Trim();

        if (Convert.ToInt32(hdnDeliveryTypeId.Value) != (int)DeliveryType.Loaded)
        {
            if (Convert.ToInt32(txtNoOfPackages.Text.Trim()) == 0)
            {
                lblError.Text = "Please Enter Number of Packages!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else if (txtNoOfPackages.Text.Trim() == "" || txtNoOfPackages.Text.Trim() == "0")
            {
                lblError.Text = "Please Enter No of Packages!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());
            }
        }
        else // For Loaded Delivery - Container Number Required
        {
            if (ddContainerNo.SelectedIndex > 0)
            {
                ContainerId = Convert.ToInt32(ddContainerNo.SelectedValue);
            }
            else
            {
                lblError.Text = "Please Select Container!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        if (ddVehicleNo.SelectedValue != "0")
        {
            VehicleNo = ddVehicleNo.SelectedItem.Text.Trim();
        }
        else
        {
            lblError.Text = "Please Select Vehicle Number!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtDispatchDate.Text.Trim() != "")
        {
            DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
        }
        if (txtVehicleRecdDate.Text.Trim() != "")
        {
            VehicleRcvdDate = Commonfunctions.CDateTime(txtVehicleRecdDate.Text.Trim());
        }
        if (txtLRDate.Text.Trim() != "")
        {
            LRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());
        }
        if (txtDeliveryDate.Text.Trim() != "")
        {
            DeliveryDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim());
        }
        if (txtReturnDate.Text.Trim() != "")
        {
            EmptyContRetrunDate = Commonfunctions.CDateTime(txtReturnDate.Text.Trim());
        }
        if (txtBabajiChallanDate.Text.Trim() != "")
        {
            BabajiChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());
        }

        if (fuPOD.HasFile)
        {
            PODPath = UploadPODFiles(fuPOD, hdnUploadPath.Value);
        }
        if (fuChallanCopy.HasFile)
        {
            ChallanPath = UploadPODFiles(fuChallanCopy, hdnUploadPath.Value);
        }
        if (fuDamageCopy.HasFile)
        {
            DamageCopyPath = UploadPODFiles(fuDamageCopy, hdnUploadPath.Value);
        }

        if (ddTransporter.SelectedIndex > 0)
        {
            TransporterID = Convert.ToInt32(ddTransporter.SelectedValue);
            strTransporterName = ddTransporter.SelectedItem.Text;
        }

        strLRNo = txtLRNo.Text.ToUpper().Trim();
        strDeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
        CargoReceivedBy = txtCargoPersonName.Text.ToUpper().Trim();
        VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);
        strBabajiChallanNo = txtBabajiChallanNo.Text.ToUpper().Trim();

        

        if (Session["ModuleId"].ToString() == "2")
        {
            // if freight forwording job
            Result = DBOperations.TR_AddDeliveryDetailForFreight(JobId, ContainerId, NoOfPackages, VehicleNo, VehicleType, VehicleRcvdDate,
                strTransporterName, TransporterID, strLRNo, LRDate, strDeliveryPoint, DispatchDate, DeliveryDate, EmptyContRetrunDate,
                CargoReceivedBy, strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath, PODPath, strDriverName, strDriverPhone, LoggedInUser.glUserId);
        }
        else
        {
            Result = DBOperations.TR_AddDeliveryDetail(JobId, ContainerId, NoOfPackages, VehicleNo, VehicleType, VehicleRcvdDate,
            strTransporterName, TransporterID, strLRNo, LRDate, strDeliveryPoint, DispatchDate, DeliveryDate, EmptyContRetrunDate,
            CargoReceivedBy, strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath, PODPath, strDriverName, strDriverPhone, LoggedInUser.glUserId);
        }

        if (Result == 0)
        {
            //  /int Fr_Result = DBOperations.FR_UPDDeliveryDetails(JobId, Convert.ToInt32(hdnDeliveryTypeId.Value), 1, LoggedInUser.glUserId);
            // Add delivery detail in transport rate table
            if (ddVehicleNo != null && ddVehicleNo.SelectedValue != "0")
            {
                int AddRateDelivered = DBOperations.UpdateRateDeliveryStatus(Convert.ToInt32(ddVehicleNo.SelectedValue), LoggedInUser.glUserId);
            }

            lblError.Text = "Delivery Detail Added Successfully!";
            lblError.CssClass = "success";
            txtNoOfPackages.Text = "";
            ddVehicleType.SelectedIndex = 0;
            ddTransporter.SelectedIndex = -1;
            txtBabajiChallanNo.Text = "";
            txtBabajiChallanDate.Text = "";
            txtDriverName.Text = "";
            txtDriverPhone.Text = "";
            DBOperations.FillVehicleForDelivery(ddVehicleNo, JobId);
            JobDetail(JobId);
            GridViewVehicle.DataBind();
        }
        else if (Result == 10)
        {
            lblError.Text = "Delivery Completed!";
            lblError.CssClass = "success";
            txtNoOfPackages.Text = "";
            btnSubmit.Visible = false;
            btnCancel.Text = "Close";
            DBOperations.FillVehicleForDelivery(ddVehicleNo, JobId);
            JobDetail(JobId);
            GridViewVehicle.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "SystemError! Please check the required field.";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Delivery Detail Already Completed!";
            lblError.CssClass = "errorMsg";
            btnSubmit.Visible = false;
            btnCancel.Text = "Close";
        }
        else if (Result == 4)
        {
            lblError.Text = "Supplied Package Count Exceed Available Packages.";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 5)
        {
            lblError.Text = "Supplied Container Count Exceed Available Container.";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 6)
        {
            lblError.Text = "Supplied Container Already Delivered!";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 7)
        {
            lblError.Text = "Please Enter and Save Examine and Out Charge Date!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("VehicleDelivery.aspx");
    }

    protected void txtNoOfPackages_TextChanged(object sender, EventArgs e)
    {
        if (txtNoOfPackages.Text.Trim() != "" && hdnBalancePkg.Value != "")
        {
            try
            {
                int intBalancePkgs = Convert.ToInt32(hdnBalancePkg.Value);
                int intDeliverPkgs = Convert.ToInt32(txtNoOfPackages.Text.Trim());
                if ((intBalancePkgs - intDeliverPkgs) < 0)
                {
                    lblError.Text = "No of Packages Exceed Balance Packages!";
                    lblError.CssClass = "errorMsg";
                    txtNoOfPackages.Focus();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Please enter valid no of packages!";
                lblError.CssClass = "errorMsg";
                txtNoOfPackages.Focus();
            }
        }
    }

    protected void ddVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataView dsGetDetails = DBOperations.GetTransRateDetailById(Convert.ToInt32(ddVehicleNo.SelectedValue));
        if (dsGetDetails != null)
        {
            DBOperations.FillCompanyByCategory(ddTransporter, 6);
            txtDeliveryPoint.Text = dsGetDetails.Table.Rows[0]["Destination"].ToString();
            ddTransporter.SelectedValue = dsGetDetails.Table.Rows[0]["TransporterId"].ToString();
            ddVehicleType.SelectedValue = dsGetDetails.Table.Rows[0]["VehicleTypeId"].ToString();
            hdnTruckRequestDate.Value = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");

            txtDeliveryPoint.Enabled = false;
            ddVehicleType.Enabled = false;
            ddTransporter.Enabled = false;
        }
    }

    protected void GridViewVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadPODDocument(DocPath);
        }
    }

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");
            if (DeliveryTypeId == (Int32)DeliveryType.Loaded) // Delivery Type
            {
                GridViewVehicle.Columns[1].Visible = true; // Container NO
                GridViewVehicle.Columns[2].Visible = false; // No Of Packages
            }
            else
            {
                GridViewVehicle.Columns[1].Visible = false; // Container NO
                GridViewVehicle.Columns[2].Visible = true; // No Of Packages
            }
        }
    }

    #region Documents Upload
    protected void DownloadPODDocument(string DocumentPath)
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

    public string UploadPODFiles(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PODFiles\\";

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

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;

            }

            fuDocument.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
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
}