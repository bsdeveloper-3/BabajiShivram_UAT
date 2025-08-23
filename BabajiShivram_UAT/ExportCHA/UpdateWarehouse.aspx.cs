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

public partial class ExportCHA_UpdateWarehouse : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);

        if (Session["JobId"] == null)
        {
            Response.Redirect("Warehouse.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse/Transport Detail";

            string strJobId = Session["JobId"].ToString();

            // Fill Job Details

            DBOperations.FillVehicleType(ddVehicleType);
            DBOperations.FillVehicleNo(ddVehicleNo, Convert.ToInt32(strJobId));
            JobDetail(Convert.ToInt32(strJobId));
            txtLRNo_OnTextChanged(null, EventArgs.Empty);

        }

        // Maximum date is Today
        MEditValDispatchDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValReceivedDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValLRDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValOctroiDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValSFormDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValNFormDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

        // Container return date & Delivered Date maximum value allowed - Today + One Year
        MEditValReturnDate.MaximumValue = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
        MEditValDeliveredDate.MaximumValue = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
    }

    private void JobDetail(int JobId)
    {
        DataView dvDetail = EXOperations.EX_GetJobDetailForDelivery(JobId);
        bool bTransportationByBabaji = false;

        if (dvDetail.Table.Rows.Count > 0)
        {
            FVJobDetail.DataSource = dvDetail;
            FVJobDetail.DataBind();

            bool bIsOctroi = false, bIsSForm = false, bIsNForm = false, bIsRoadPermit = false;
            int intBalancePackage = 0;

            if (dvDetail.Table.Rows[0]["TransportById"] != DBNull.Value)
            {
                bTransportationByBabaji = Convert.ToBoolean(dvDetail.Table.Rows[0]["TransportById"]);
                // Fill All Transporter List

                // DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));

                EXOperations.EX_FillTransporter(ddTransporter, Convert.ToInt32(JobId), Convert.ToInt32(EnumCompanyType.Transporter));

                if (bTransportationByBabaji == true)
                {
                    hdnTransportBy.Value = "1"; // Babaji Transport
                    ddTransporter.Visible = true;
                    RFVTransID.Enabled = true;

                    txtTransporterName.Visible = false;
                    RFVTransName.Enabled = false;
                }
                else
                {
                    hdnTransportBy.Value = "0"; // Customer Transport
                    txtTransporterName.Visible = true;
                    RFVTransName.Enabled = true;

                    ddTransporter.Visible = false;
                    RFVTransID.Enabled = false;
                    RFVBabajiChalanNo.Enabled = false;
                }
            }

            lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse / Transport - " + lblJobRefNo.Text;
            lblTranportBy2.Text = dvDetail.Table.Rows[0]["TransportBy"].ToString();

            intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveredPackages"]);
            lblBalancePackage.Text = intBalancePackage.ToString() + "/" + dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            hdnBalancePkg.Value = intBalancePackage.ToString();
            lblDeliveredContainer.Text = dvDetail.Table.Rows[0]["DeliveredContainer"].ToString();
            txtDeliveryPoint.Text = dvDetail.Table.Rows[0]["Destination"].ToString();

            if (dvDetail.Table.Rows[0]["ContainerLoaded"] != DBNull.Value)
            {
                //hdnDeliveryTypeId.Value = dvDetail.Table.Rows[0]["ContainerLoaded"].ToString();
            }

            // Show Hide Applicable Fields Panel
            if (dvDetail.Table.Rows[0]["IsOctroi"] != DBNull.Value)
            {
                bIsOctroi = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsOctroi"]);
                if (bIsOctroi == true)
                    pnlOctroiApplicable.Visible = true;
            }
            if (dvDetail.Table.Rows[0]["IsSForm"] != DBNull.Value)
            {
                bIsSForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsSForm"]);
                if (bIsSForm == true)
                    pnlSFormApplicable.Visible = true;
            }
            if (dvDetail.Table.Rows[0]["IsNForm"] != DBNull.Value)
            {
                bIsNForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsNForm"]);
                if (bIsNForm == true)
                    pnlNFormApplicable.Visible = true;
            }
            if (dvDetail.Table.Rows[0]["IsRoadPermit"] != DBNull.Value)
            {
                bIsRoadPermit = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRoadPermit"]);
                if (bIsRoadPermit == true)
                    pnlRoadPermitApplicable.Visible = true;
            }

            // Container Detail for Sea Mode Only and Loaded Type Delivery
            if (Convert.ToInt32(dvDetail.Table.Rows[0]["TransModeId"]) == (Int32)TransMode.Sea)
            {
                string DeliveryType = dvDetail.Table.Rows[0]["ContainerLoaded"].ToString();
                hdnExportType.Value = dvDetail.Table.Rows[0]["ExportTypeId"].ToString();
                int ExportType = Convert.ToInt32(dvDetail.Table.Rows[0]["ExportTypeId"].ToString());

                if (ExportType == 2) // Doc Stuff && Sea
                {
                    DivContainer.Visible = false;
                    DivPackages.Visible = true;
                    RFVNoOfPkgs.Enabled = true;
                    RFVContainer.Enabled = false;
                    GridViewVehicle.Columns[1].Visible = false;
                }
                else
                {
                    DivContainer.Visible = true;
                    DivPackages.Visible = false;
                    RFVNoOfPkgs.Enabled = false;
                    RFVContainer.Enabled = true;
                    GridViewVehicle.Columns[2].Visible = false;
                    EXOperations.EX_FillPendingContainerDetail(ddContainerNo, JobId);
                }

                //if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value && Convert.ToInt16(dvDetail.Table.Rows[0]["DeliveryType"]) != 0)
                //{
                //    btnSubmit.Visible = true;

                //DeliveryType DODeliveryType = (DeliveryType)dvDetail.Table.Rows[0]["DeliveryType"];


                //lblLoaded.Text = DODeliveryType.ToString();
                //lblDeliveryType.Text = DODeliveryType.ToString();

                //if (Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveryType"]) == (int)DeliveryType.Loaded)
                //{
                //    // Show/hide Container/Package Div

                //    DivContainer.Visible = true;
                //    DivPackages.Visible = false;
                //    RFVNoOfPkgs.Enabled = false;
                //    RFVContainer.Enabled = true;
                //    DBOperations.FillPendingContainerDetail(ddContainerNo, JobId, TransitType);
                //}
                //else
                //{
                //    // Show/hide Container/Package Div

                //    DivContainer.Visible = false;
                //    DivPackages.Visible = true;
                //    RFVNoOfPkgs.Enabled = true;
                //    RFVContainer.Enabled = false;
                //}

                //// For JNPT - DeStuff Labour Expense
                //if (PortId == 5 && DODeliveryType == DeliveryType.DeStuff) // Mumbai_JNPT And Destuff Delivery
                //{
                //    divExpenseSea.Visible = true;
                //}
                //}
                //else
                //{
                //    btnSubmit.Visible = false;
                //    lblError.Text = "Delivery Type Detail Not Found!";
                //    lblError.CssClass = "errorMsg";
                //}

                //pnlForSea.Visible = true;
                pnlForSea2.Visible = true;

            }
            else
            {
                DivContainer.Visible = false;
                GridViewVehicle.Columns[1].Visible = false;
            }


            if (bTransportationByBabaji == true)
            {
                txtVehicleNo.Visible = false;
                ddVehicleNo.Visible = true;
            }
            else
            {
                txtVehicleNo.Visible = true;
                ddVehicleNo.Visible = false;
            }

            // Expense Detail For Mumbai AIR

            //if (PortId == 4) // Mumbai_AIR
            //{
            //    divExpenseAir.Visible = true;

            //    if (dvDetail.Table.Rows[0]["IsRunwayDelivery"] != DBNull.Value)
            //    {
            //        if (Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRunwayDelivery"]) == true)
            //            rdlRunwayDelivery.SelectedValue = "true";
            //        else
            //            rdlRunwayDelivery.SelectedValue = "false";
            //    }
            //}

            /************** Oct.12.2015 Start ******************/
            hdnBranchId.Value = dvDetail.Table.Rows[0]["BabajiBranchId"].ToString();

        } //END_IF_RowCount
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Result = -123, JobId = Convert.ToInt32(Session["JobId"]);
        int NoOfPackages = 0, ContainerId = 0, VehicleType = 0, LabourTypeId = 0;
        int WarehouseId = 0, TransitType = 0, TransporterID = 0;

        string strIsRunwayDelivery = "", strTransporterName, strLRNo, strDeliveryPoint, VehicleNo, RoadPermitNo, CargoReceivedBy, PODPath = "", ChallanPath = "",
                        DamageCopyPath = "", NFormNo, SFormNo, OctroiReciptNo, OctroiAmount, strBabajiChallanNo, strDriverName, strDriverPhone;

        DateTime VehicleRcvdDate = DateTime.MinValue, LRDate = DateTime.MinValue, DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
                EmptyContRetrunDate = DateTime.MinValue, RoadPermitDate = DateTime.MinValue, NFormDate = DateTime.MinValue, NClosingDate = DateTime.MinValue,
                SFormDate = DateTime.MinValue, SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue, BabajiChallanDate = DateTime.MinValue;

        strDriverName = txtDriverName.Text.Trim();
        strDriverPhone = txtDriverPhone.Text.Trim();

        Label lblTransMode = (Label)FVJobDetail.FindControl("lblTransMode");
        if (lblTransMode.Text.Trim().ToString().ToLower() == "air")
        {
            if (txtNoOfPackages.Text.Trim() == "" || txtNoOfPackages.Text.Trim() == "0")
            {
                lblError.Text = "Please Enter No of Packages!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        if (lblTranportBy2.Text.Trim().ToLower() == "customer")
        {
            if (txtVehicleNo.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Vehicle Number!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                VehicleNo = txtVehicleNo.Text.Trim();
            }
        }
        else
        {
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
        }

        //if (Convert.ToInt32(hdnDeliveryTypeId.Value) != (int)DeliveryType.Loaded)
        //{
        //    if (Convert.ToInt32(txtNoOfPackages.Text.Trim()) == 0)
        //    {
        //        lblError.Text = "Please Enter Number of Packages!";
        //        lblError.CssClass = "errorMsg";
        //        return;
        //    }
        //    else if (txtNoOfPackages.Text.Trim() == "" || txtNoOfPackages.Text.Trim() == "0")
        //    {
        //        lblError.Text = "Please Enter No of Packages!";
        //        lblError.CssClass = "errorMsg";
        //        return;
        //    }
        //    else
        //    {
        //        NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());
        //    }
        //}
        //else // For Loaded Delivery - Container Number Required
        //{
        //    if (ddContainerNo.SelectedIndex > 0)
        //    {
        //        ContainerId = Convert.ToInt32(ddContainerNo.SelectedValue);
        //    }
        //    else
        //    {
        //        lblError.Text = "Please Select Container!";
        //        lblError.CssClass = "errorMsg";
        //        return;
        //    }
        //}

        if (lblTransMode.Text.Trim().ToString().ToLower() == "sea")
        {
            if (hdnExportType.Value == "2") // doc suff
            {
                NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());
            }
            else
            {
                if (ddContainerNo.SelectedIndex > 0)
                    ContainerId = Convert.ToInt32(ddContainerNo.SelectedValue);
            }
        }
        else
            NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());

        if (txtDispatchDate.Text.Trim() != "")
            DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
        else
        {
            lblError.Text = "Please Enter Dispatch Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        //if (txtVehicleNo.Text.Trim() == "")
        //{
        //    lblError.Text = "Please Enter Vehicle Number!";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

        if (txtVehicleRecdDate.Text.Trim() != "")
            VehicleRcvdDate = Commonfunctions.CDateTime(txtVehicleRecdDate.Text.Trim());

        if (txtLRDate.Text.Trim() != "")
            LRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());

        if (txtDeliveryDate.Text.Trim() != "")
            DeliveryDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim());

        if (txtReturnDate.Text.Trim() != "")
            EmptyContRetrunDate = Commonfunctions.CDateTime(txtReturnDate.Text.Trim());

        if (txtRoadPermitDate.Text.Trim() != "")
            RoadPermitDate = Commonfunctions.CDateTime(txtRoadPermitDate.Text.Trim());

        if (fuPOD.HasFile)
            PODPath = UploadPODFiles(fuPOD, hdnUploadPath.Value);

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
        else
        {
            TransporterID = 0;
            strTransporterName = txtTransporterName.Text.ToUpper().Trim();
        }

        strLRNo = txtLRNo.Text.ToUpper().Trim();
        strDeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
        //VehicleNo = txtVehicleNo.Text.ToUpper().Trim();
        RoadPermitNo = txtRoadPermitNo.Text.ToUpper().Trim();
        CargoReceivedBy = txtCargoPersonName.Text.ToUpper().Trim();

        NFormNo = txtNFormNo.Text.ToUpper().Trim();
        SFormNo = txtSFormDate.Text.ToUpper().Trim();
        OctroiReciptNo = txtOctroiReceiptNo.Text.ToUpper().Trim();
        OctroiAmount = txtOctroiAmount.Text.Trim();
        VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);
        strBabajiChallanNo = txtBabajiChallanNo.Text.ToUpper().Trim();

        if (txtNFormDate.Text.Trim() != "")
        {
            NFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim());
        }
        if (txtSFormDate.Text.Trim() != "")
        {
            SFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim());
        }
        if (txtNClosingDate.Text.Trim() != "")
        {
            NClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim());
        }
        if (txtSClosingDate.Text.Trim() != "")
        {
            SClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim());
        }
        if (txtOctroiPaidDate.Text.Trim() != "")
        {
            OctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim());
        }
        if (txtBabajiChallanDate.Text.Trim() != "")
        {
            BabajiChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());
        }

        // For Air Job Expense Calculation
        if (divExpenseAir.Visible == true)
        {
            if (Convert.ToBoolean(rdlRunwayDelivery.SelectedValue) == true)
                strIsRunwayDelivery = "yes";
            else
                strIsRunwayDelivery = "no";
        }
        // For Sea JNPT Expense Calculation
        if (divExpenseSea.Visible == true)
        {
            LabourTypeId = Convert.ToInt16(ddLabourType.SelectedValue);
        }

        Result = EXOperations.EX_AddDeliveryDetail(JobId, ContainerId, NoOfPackages, VehicleNo, DateTime.MinValue, VehicleRcvdDate, strTransporterName,
             TransporterID, strLRNo, LRDate, strDeliveryPoint, DispatchDate, DeliveryDate, EmptyContRetrunDate, RoadPermitNo, RoadPermitDate, CargoReceivedBy,
             PODPath, NFormNo, NFormDate, NClosingDate, SFormNo, SFormDate, SClosingDate, OctroiAmount, OctroiReciptNo, OctroiPaidDate, VehicleType,
             strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath, strIsRunwayDelivery, LabourTypeId, TransitType, WarehouseId, strDriverName,
             strDriverPhone, LoggedInUser.glUserId);

        if (Result == 0)
        {
            if (lblTranportBy2.Text.Trim().ToLower() == "babaji shivram")
            {
                // Add delivery detail in transport rate table
                if (ddVehicleNo != null && ddVehicleNo.SelectedValue != "0")
                {
                    int AddRateDelivered = DBOperations.UpdateRateDeliveryStatus(Convert.ToInt32(ddVehicleNo.SelectedValue), LoggedInUser.glUserId);
                }
            }

            lblError.Text = "Delivery Detail Added Successfully!";
            lblError.CssClass = "success";
            txtNoOfPackages.Text = "";
            txtVehicleNo.Text = "";
            ddVehicleType.SelectedIndex = 0;
            ddTransporter.SelectedIndex = -1;
            txtTransporterName.Text = "";
            txtBabajiChallanNo.Text = "";
            txtBabajiChallanDate.Text = "";
            txtDriverName.Text = "";
            txtDriverPhone.Text = "";
            txtNFormDate.Text = "";
            txtNFormNo.Text = "";
            txtNClosingDate.Text = "";
            txtSClosingDate.Text = "";
            txtSFormDate.Text = "";
            txtSFormNo.Text = "";
            txtOctroiAmount.Text = "";
            txtOctroiPaidDate.Text = "";
            txtOctroiReceiptNo.Text = "";
            txtRoadPermitDate.Text = "";
            txtRoadPermitNo.Text = "";

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

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        if (hdnTransportBy.Value == "1")
            Response.Redirect("Warehouse.aspx");
        else if (hdnTransportBy.Value == "0")
            Response.Redirect("WarehouseCustomer.aspx");
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        if (hdnTransportBy.Value == "1")
            Response.Redirect("Warehouse.aspx");
        else if (hdnTransportBy.Value == "0")
            Response.Redirect("WarehouseCustomer.aspx");
    }

    protected void txtLRNo_OnTextChanged(object sender, EventArgs e)
    {
        if (txtLRNo.Text != "")
        {
            MEditValLRDate.IsValidEmpty = false;
            MEditValLRDate.EmptyValueMessage = "Please Enter LR Date.";
            MEditValLRDate.EmptyValueBlurredText = "*";
        }
        else
        {
            MEditValLRDate.IsValidEmpty = true;
        }
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
                else
                {
                    txtVehicleNo.Focus();
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
            txtTransporterName.Text = dsGetDetails.Table.Rows[0]["TransporterName"].ToString();
            ddTransporter.SelectedValue = dsGetDetails.Table.Rows[0]["TransporterId"].ToString();
            ddVehicleType.SelectedValue = dsGetDetails.Table.Rows[0]["VehicleTypeId"].ToString();
            //txtLRNo.Text = dsGetDetails.Table.Rows[0]["LRNo"].ToString();
            //txtLRDate.Text = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["LRDate"]).ToString("dd/MM/yyyy");
            //txtBabajiChallanNo.Text = dsGetDetails.Table.Rows[0]["ChallanNo"].ToString();
            //txtBabajiChallanDate.Text = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["ChallanDate"]).ToString("dd/MM/yyyy");
            //hdnTruckRequestDate.Value = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");

            txtDeliveryPoint.Enabled = false;
            txtTransporterName.Enabled = false;
            ddVehicleType.Enabled = false;
            ddTransporter.Enabled = false;
            //txtLRNo.Enabled = false;
            //txtLRDate.Enabled = false;
            //txtBabajiChallanNo.Enabled = false;
            //txtBabajiChallanDate.Enabled = false;
        }
    }

    #region UPLOAD EVENTS

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
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
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

    #region GRID VIEW EVENTS
    protected void GridViewVehicle_RowCommand(Object Sender, GridViewCommandEventArgs e)
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
            //int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");

            //if (DeliveryTypeId == (Int32)DeliveryType.Loaded) // Delivery Type
            //{
            //    GridViewVehicle.Columns[1].Visible = true; // Container NO
            //    GridViewVehicle.Columns[2].Visible = false; // No Of Packages
            //}
            //else
            //{
            //    GridViewVehicle.Columns[1].Visible = false; // Container NO
            //    GridViewVehicle.Columns[2].Visible = true; // No Of Packages
            //}

        }
    }

    private void DownloadPODDocument(string DocumentPath)
    {
        //  String ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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

    #endregion
}