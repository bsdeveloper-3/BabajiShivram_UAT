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
public partial class Transport_TransDeliveryDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);

        if (Session["JobId"] == null)
        {
            Response.Redirect("TransPendingDelivery.aspx");
        }

        if (!IsPostBack)
        {
            ViewState["BranchId"] = "";
            ViewState["ClearedStatus"] = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Delivery - Clearance";

            string strJobId = Session["JobId"].ToString();

            // Fill Job Details

            DBOperations.FillVehicleType(ddVehicleType);
            // DBOperations.FillVehicleNo(ddVehicleNo, Convert.ToInt32(strJobId));
            JobDetail(Convert.ToInt32(strJobId));

        }

        // Maximum date is Today
        MEditValExaminDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MEditValOutDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

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
        string strCustDocFolder = "", strJobFileDir = "";
        int BabajiBranchId = 0;
        DataView dvDetail = DBOperations.GetJobDetailForDelivery(JobId);

        bool bTransportationByBabaji = false;

        if (dvDetail.Table.Rows.Count > 0)
        {
            bool bIsOctroi = false, bIsSForm = false, bIsNForm = false, bIsRoadPermit = false;
            int TransitType = 0; int WarehouseId = 0;

            int intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]);
            int intBoeType = Convert.ToInt32(dvDetail.Table.Rows[0]["BoeType"]);
            int PortId = Convert.ToInt32(dvDetail.Table.Rows[0]["PortId"]);

            if (dvDetail.Table.Rows[0]["TransportationByBabaji"] != DBNull.Value)
            {
                bTransportationByBabaji = Convert.ToBoolean(dvDetail.Table.Rows[0]["TransportationByBabaji"]);

                // Fill All Transporter List
                if (Convert.ToInt32(dvDetail.Table.Rows[0]["BabajiBranchId"]) == 2)
                {
                    DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
                }
                else
                {
                    DBOperations.FillTransporterList(ddTransporter, JobId);
                }
                // Get Transporter List Assigne By Transport Dept
                // DBOperations.FillTransporterPlaced(ddTransporter, JobId, 0);

                if (bTransportationByBabaji == true)
                {
                    ddTransporter.Visible = true;
                    RFVTransID.Enabled = true;

                    txtTransporterName.Visible = false;
                    RFVTransName.Enabled = false;
                }
                else
                {
                    txtTransporterName.Visible = true;
                    RFVTransName.Enabled = true;

                    ddTransporter.Visible = false;
                    RFVTransID.Enabled = false;
                }

            }
            else
            {
                btnSubmit.Visible = false;
                lblError.Text = "Transport By Babaji Or Customer Not Updated!";
                lblError.CssClass = "errorMsg";
            }
            if (dvDetail.Table.Rows[0]["TransitType"] != DBNull.Value)
            {
                TransitType = Convert.ToInt32(dvDetail.Table.Rows[0]["TransitType"]);
            }

            if (TransitType == 1) // Moved to customer place
            {
                intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveredPackages"]);
                lblDeliveredContainer.Text = dvDetail.Table.Rows[0]["DeliveredContainer"].ToString();
            }
            else if (TransitType == 2 || TransitType == 3) // Moved to Warehouse
            {
                intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["WarehousePackages"]);
                lblDeliveredContainer.Text = dvDetail.Table.Rows[0]["WarehouseContainer"].ToString();
            }

            lblBalancePackage.Text = intBalancePackage.ToString() + "/" + dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            hdnBalancePkg.Value = intBalancePackage.ToString();

            if (dvDetail.Table.Rows[0]["TransitType"] != DBNull.Value)
            {
                TransitType = Convert.ToInt32(dvDetail.Table.Rows[0]["TransitType"]);
            }
            if (dvDetail.Table.Rows[0]["WarehouseId"] != DBNull.Value)
            {
                WarehouseId = Convert.ToInt32(dvDetail.Table.Rows[0]["WarehouseId"]);
            }

            lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Delivery - Clearance - " + lblJobRefNo.Text;

            lblCustRefNo.Text = dvDetail.Table.Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            lblConsigneeName.Text = dvDetail.Table.Rows[0]["Consignee"].ToString();
            lblFFName.Text = dvDetail.Table.Rows[0]["FFName"].ToString();
            lblMode.Text = dvDetail.Table.Rows[0]["Mode"].ToString();
            lblShortDesc.Text = dvDetail.Table.Rows[0]["ShortDescription"].ToString();
            lblNoOfPackages.Text = dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWT"].ToString();
            lblTranportBy.Text = dvDetail.Table.Rows[0]["TransportationBy"].ToString();
            lblTranportBy2.Text = dvDetail.Table.Rows[0]["TransportationBy"].ToString();
            lblRMSNonRms.Text = dvDetail.Table.Rows[0]["RMSNonRms"].ToString();
            lblConReExamine.Text = dvDetail.Table.Rows[0]["ContReExamine"].ToString();
            lblCFSName.Text = dvDetail.Table.Rows[0]["sName"].ToString();
            if (dvDetail.Table.Rows[0]["TransportationBy"].ToString().ToLower().Trim() == "customer")
            {
                txtVehicleNo.Visible = true;
                ddVehicleNo.Visible = false;
            }
            else
            {
                txtVehicleNo.Visible = false;
                ddVehicleNo.Visible = true;
                // Get Vehicle No From Transport Dept Filled List
                DBOperations.FillVehicleNo(ddVehicleNo, JobId);

            }

            if (dvDetail.Table.Rows[0]["PackageTypeName"] != DBNull.Value)
            {
                lblPackageType.Text = dvDetail.Table.Rows[0]["PackageTypeName"].ToString();
            }

            lblDeliveryInstruction.Text = dvDetail.Table.Rows[0]["DeliveryInstruction"].ToString();
            //lblDeliveryDestination.Text = dvDetail.Table.Rows[0]["DeliveryDestination"].ToString();
            lblDeliveryAddress.Text = dvDetail.Table.Rows[0]["DeliveryAddress"].ToString();
            lblShipmentType.Text = dvDetail.Table.Rows[0]["ShipmentType"].ToString();

            if (intBoeType == (Int32)EnumBOEType.Inbond)
            {
                txtDeliveryPoint.Text = dvDetail.Table.Rows[0]["WarehouseName"].ToString();
            }
            else
            {
                txtDeliveryPoint.Text = dvDetail.Table.Rows[0]["DeliveryDestination"].ToString();
            }

            if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
            {
                hdnDeliveryTypeId.Value = dvDetail.Table.Rows[0]["DeliveryType"].ToString();
            }

            //if (dvDetail.Table.Rows[0]["TruckRequestDate"] != DBNull.Value)
            //{
            //    lblTruckReqDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");
            //}

            if (dvDetail.Table.Rows[0]["OutOfChargeDate"] != DBNull.Value)
            {
                txtOutOfChargeDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["OutOfChargeDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                btnSubmit.Visible = false;
                lblError.Text = "OOC Date Not Updated!";
                lblError.CssClass = "errorMsg";
            }
            if (dvDetail.Table.Rows[0]["ExamineDate"] != DBNull.Value)
            {
                txtExamineDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["ExamineDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                btnSubmit.Visible = false;
                lblError.Text = "Examine Date Not Updated!";
                lblError.CssClass = "errorMsg";
            }

            txtExamineDate.Enabled = true;
            txtOutOfChargeDate.Enabled = true;

            BabajiBranchId = Convert.ToInt32(dvDetail.Table.Rows[0]["BabajiBranchId"].ToString());

            if (BabajiBranchId != 0 && BabajiBranchId == 3) // mumbai saki naka branch
            {
                txtExamineDate.Enabled = false;
                txtOutOfChargeDate.Enabled = false;
                imgExamineDt.Visible = false;
                imgOutOfChargeDt.Visible = false;
            }

            //if (txtExamineDate.Text.Trim() != "" && txtOutOfChargeDate.Text.Trim() != "")
            //{
            //    btnSaveOutOfCharge.Visible = false;
            //    txtExamineDate.Enabled = false;
            //    txtOutOfChargeDate.Enabled = false;
            //    imgExamineDt.Visible = false;
            //    imgOutOfChargeDt.Visible = false;
            //}

            // Job Directory Path

            if (dvDetail.Table.Rows[0]["DocFolder"] != DBNull.Value)
                strCustDocFolder = dvDetail.Table.Rows[0]["DocFolder"].ToString() + "\\";

            if (dvDetail.Table.Rows[0]["FileDirName"] != DBNull.Value)
                strJobFileDir = dvDetail.Table.Rows[0]["FileDirName"].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            // Show Hide Applicable Fields Panel
            //if (dvDetail.Table.Rows[0]["IsOctroi"] != DBNull.Value)
            //{
            //    bIsOctroi = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsOctroi"]);
            //    if (bIsOctroi == true)
            //        pnlOctroiApplicable.Visible = true;
            //}
            //if (dvDetail.Table.Rows[0]["IsSForm"] != DBNull.Value)
            //{
            //    bIsSForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsSForm"]);
            //    if (bIsSForm == true)
            //        pnlSFormApplicable.Visible = true;
            //}
            //if (dvDetail.Table.Rows[0]["IsNForm"] != DBNull.Value)
            //{
            //    bIsNForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsNForm"]);
            //    if (bIsNForm == true)
            //        pnlNFormApplicable.Visible = true;
            //}
            //if (dvDetail.Table.Rows[0]["IsRoadPermit"] != DBNull.Value)
            //{
            //    bIsRoadPermit = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRoadPermit"]);
            //    if (bIsRoadPermit == true)
            //        pnlRoadPermitApplicable.Visible = true;
            //}

            // Container Detail for Sea Mode Only and Loaded Type Delivery

            if (Convert.ToInt32(dvDetail.Table.Rows[0]["TransMode"]) == (Int32)TransMode.Sea)
            {
                if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value && Convert.ToInt16(dvDetail.Table.Rows[0]["DeliveryType"]) != 0)
                {
                    btnSubmit.Visible = true;

                    DeliveryType DODeliveryType = (DeliveryType)dvDetail.Table.Rows[0]["DeliveryType"];

                    lblLoaded.Text = DODeliveryType.ToString();
                    lblDeliveryType.Text = DODeliveryType.ToString();

                    if (Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveryType"]) == (int)DeliveryType.Loaded)
                    {
                        // Show/hide Container/Package Div

                        DivContainer.Visible = true;
                        DivPackages.Visible = false;
                        RFVNoOfPkgs.Enabled = false;
                        RFVContainer.Enabled = true;
                        DBOperations.FillPendingContainerDetail(ddContainerNo, JobId, TransitType);
                    }
                    else
                    {
                        // Show/hide Container/Package Div

                        DivContainer.Visible = false;
                        DivPackages.Visible = true;
                        RFVNoOfPkgs.Enabled = true;
                        RFVContainer.Enabled = false;
                    }

                    // For JNPT - DeStuff Labour Expense
                    if (PortId == 5 && DODeliveryType == DeliveryType.DeStuff) // Mumbai_JNPT And Destuff Delivery
                    {
                        divExpenseSea.Visible = true;
                    }
                }
                else
                {
                    btnSubmit.Visible = false;
                    lblError.Text = "Delivery Type Detail Not Found!";
                    lblError.CssClass = "errorMsg";
                }

                pnlForSea.Visible = true;
                pnlForSea2.Visible = true;

                lblCon20.Text = dvDetail.Table.Rows[0]["Con20"].ToString();
                lblCon40.Text = dvDetail.Table.Rows[0]["Con40"].ToString();
                lblLCL.Text = dvDetail.Table.Rows[0]["LCL"].ToString();
            }
            else
            {
                DivContainer.Visible = false;
            }

            // Expense Detail For Mumbai AIR

            if (PortId == 4) // Mumbai_AIR
            {
                divExpenseAir.Visible = true;

                if (dvDetail.Table.Rows[0]["IsRunwayDelivery"] != DBNull.Value)
                {
                    if (Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRunwayDelivery"]) == true)
                        rdlRunwayDelivery.SelectedValue = "true";
                    else
                        rdlRunwayDelivery.SelectedValue = "false";
                }
            }

            /************** Oct.12.2015 Start ******************/

            int BoeType = Convert.ToInt32(dvDetail.Table.Rows[0]["BOEType"].ToString());
            int JobType = Convert.ToInt32(dvDetail.Table.Rows[0]["JobType"].ToString());
            int intTransMode = Convert.ToInt32(dvDetail.Table.Rows[0]["TransMode"].ToString());
            int DeliveryTypeId = Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveryType"].ToString());
            hdnBranchId.Value = dvDetail.Table.Rows[0]["BabajiBranchId"].ToString();
            ViewState["BranchId"] = Convert.ToString(dvDetail.Table.Rows[0]["BabajiBranchId"]);    // ViewState For Branchid (UnderDelivery To Shipment Clear)
            ViewState["ClearedStatus"] = Convert.ToString(dvDetail.Table.Rows[0]["ClearedStatus"]);


            if (dvDetail.Table.Rows[0]["TransitType"] != DBNull.Value)
            {
                ddTransitType.SelectedValue = dvDetail.Table.Rows[0]["TransitType"].ToString();
            }

            if ((BoeType == (int)EnumBOEType.Exbond))
            {
                ddTransitType.SelectedValue = "1"; //Move to Customer Place
                ddTransitType.Enabled = false;
            }
            else if ((BoeType == (int)EnumBOEType.Home))
            {
                if (ddTransitType.Items.Count > 3)
                {
                    ddTransitType.Items.RemoveAt(3);
                }

                ddTransitType.Enabled = true;

                if (JobType == 13) // DPD
                {
                    ddTransitType.SelectedValue = "1"; //Move to Customer Place
                    ddTransitType.Enabled = false;
                }
                else
                {
                    DBOperations.FillWarehouse(ddWarehouse, (Int16)EnumWarehouseType.General, Convert.ToInt32(hdnBranchId.Value));
                    ddWarehouse.Visible = true;
                }
            }
            else if (BoeType == (int)EnumBOEType.Inbond)
            {
                DBOperations.FillWarehouse(ddWarehouse, (Int16)EnumWarehouseType.Bonded, Convert.ToInt32(hdnBranchId.Value));

                ddTransitType.Enabled = false;
                ddTransitType.SelectedValue = "3";
                ddWarehouse.Visible = true;
            }

            // Set Warehouse ID and Visibility

            WarehouseVisibility();

            if (dvDetail.Table.Rows[0]["WarehouseId"] != DBNull.Value)
            {
                ddWarehouse.SelectedValue = dvDetail.Table.Rows[0]["WarehouseId"].ToString();
            }

            if (TransitType > 0)
                ddTransitType.Enabled = false;
            if (WarehouseId > 0)
                ddWarehouse.Enabled = false;

        } //END_IF_RowCount
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Result = -123;

        int JobId = Convert.ToInt32(Session["JobId"]);
        int NoOfPackages = 0, ContainerId = 0, VehicleType = 0, LabourTypeId = 0;
        int WarehouseId = 0, TransitType = 0, TransporterID = 0;

        string strIsRunwayDelivery = "";

        string strTransporterName, strLRNo, strDeliveryPoint, VehicleNo, RoadPermitNo, CargoReceivedBy,
            PODPath = "", ChallanPath = "", DamageCopyPath = "",
            NFormNo, SFormNo, OctroiReciptNo, OctroiAmount, strBabajiChallanNo, strDriverName, strDriverPhone;

        DateTime TruckReqDate = DateTime.MinValue, VehicleRcvdDate = DateTime.MinValue,
            LRDate = DateTime.MinValue, DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
            EmptyContRetrunDate = DateTime.MinValue, RoadPermitDate = DateTime.MinValue,
            NFormDate = DateTime.MinValue, SFormDate = DateTime.MinValue,
            NClosingDate = DateTime.MinValue, SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue,
            BabajiChallanDate = DateTime.MinValue, OutOfChargeDate = DateTime.MinValue;
        TransitType = Convert.ToInt32(ddTransitType.SelectedValue);

        if (ddWarehouse.SelectedIndex != -1)
            WarehouseId = Convert.ToInt32(ddWarehouse.SelectedValue);

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

        if (txtDispatchDate.Text.Trim() != "")
        {
            DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
            if (txtOutOfChargeDate.Text.Trim() != "")
            {
                OutOfChargeDate = Commonfunctions.CDateTime(txtOutOfChargeDate.Text.Trim());

                int result = DateTime.Compare(DispatchDate, OutOfChargeDate);
                if (result < 0) // dispatch date is less than out of charge date
                {
                    lblError.Text = "Dispatch Date should be greater than Out Of Charge Date.";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }
        }
        else
        {
            lblError.Text = "Please Enter Dispatch Date!";
            lblError.CssClass = "errorMsg";
            return;
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
        if (txtRoadPermitDate.Text.Trim() != "")
        {
            RoadPermitDate = Commonfunctions.CDateTime(txtRoadPermitDate.Text.Trim());
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
        else
        {
            TransporterID = 0;
            strTransporterName = txtTransporterName.Text.ToUpper().Trim();
        }

        strLRNo = txtLRNo.Text.ToUpper().Trim();
        strDeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
        RoadPermitNo = txtRoadPermitNo.Text.ToUpper().Trim();
        CargoReceivedBy = txtCargoPersonName.Text.ToUpper().Trim();

        NFormNo = txtNFormNo.Text.ToUpper().Trim();
        SFormNo = txtSFormDate.Text.ToUpper().Trim();
        OctroiReciptNo = txtOctroiReceiptNo.Text.ToUpper().Trim();
        OctroiAmount = txtOctroiAmount.Text.Trim();
        VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);
        strBabajiChallanNo = txtBabajiChallanNo.Text.ToUpper().Trim();

        //if (hdnTruckRequestDate.Value != "")
        //{
        //    TruckReqDate = Commonfunctions.CDateTime(hdnTruckRequestDate.Value);
        //}
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

        Result = DBOperations.AddDeliveryDetail(JobId, ContainerId, NoOfPackages, VehicleNo, TruckReqDate, VehicleRcvdDate,
            strTransporterName, TransporterID, strLRNo, LRDate, strDeliveryPoint, DispatchDate, DeliveryDate, EmptyContRetrunDate, RoadPermitNo, RoadPermitDate,
            CargoReceivedBy, PODPath, NFormNo, NFormDate, NClosingDate, SFormNo, SFormDate, SClosingDate, OctroiAmount, OctroiReciptNo, OctroiPaidDate,
            VehicleType, strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath, strIsRunwayDelivery, LabourTypeId,
            TransitType, WarehouseId, strDriverName, strDriverPhone, LoggedInUser.glUserId);

        if (Result == 0)
        {
            if (lblTranportBy2.Text.Trim().ToLower() == "babaji")
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
            //txtVehicleNo.Text = "";
            ddVehicleType.SelectedIndex = 0;
            ddTransporter.SelectedIndex = -1;
            txtTransporterName.Text = "";
            txtBabajiChallanNo.Text = "";
            txtBabajiChallanDate.Text = "";
            txtDriverName.Text = "";
            txtDriverPhone.Text = "";

            JobDetail(JobId);

            GridViewVehicle.DataBind();

            // ------------ Job Move From Delivery Details to PCD

            string BSbranchid = ViewState["BranchId"].ToString();
            string ClearedStatus = ViewState["ClearedStatus"].ToString();
            int res = 0;
            if (BSbranchid != "3")
            {
                if (ClearedStatus == "True")
                {
                    res = Result = DBOperations.AddDeliveryToPCD(JobId, LoggedInUser.glUserId);

                    if (res == 0)
                    {
                        lblError.Text = "Delivery Completed! & JOb Move to PCD";
                        lblError.CssClass = "success";
                    }
                }
            }

            // -------------------

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
        Response.Redirect("PendingTransDelivery.aspx");
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
                    //txtVehicleNo.Focus();
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
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
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

    private void DownloadPODDocument(string DocumentPath)
    {
        //  String ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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

    /************ Oct.12.2015 START ***********/
    protected void ddTransitType_SelectedIndexChanged(object sender, EventArgs e)
    {
        WarehouseVisibility();
    }

    private void WarehouseVisibility()
    {
        int BranchId = Convert.ToInt32(hdnBranchId.Value);

        if (ddTransitType.SelectedValue == "1") //Move to Customer Place
        {
            ddWarehouse.Visible = false;
            ddWarehouse.SelectedIndex = -1;
            RFVBonded.Enabled = false;
        }
        else if (ddTransitType.SelectedValue == "2") //Move To General Warehouse
        {
            DBOperations.FillWarehouse(ddWarehouse, (Int32)EnumWarehouseType.General, BranchId);
            ddWarehouse.Visible = true;
            RFVBonded.Enabled = true;
        }
        else if (ddTransitType.SelectedValue == "3") //Move To In Bonded Warehouse
        {
            DBOperations.FillWarehouse(ddWarehouse, (Int32)EnumWarehouseType.Bonded, BranchId);
            ddWarehouse.Visible = true;
            RFVBonded.Enabled = true;
        }

    }

    protected void ddWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        // If Job Type is In-Bond then Delivery Destination = WareHouse Name,Delivery Address = WareHouse Address

        if (ddTransitType.SelectedValue == "3")
        {
            DataSet dsWarehouse = DBOperations.GetWareHouseById(Convert.ToInt32(ddWarehouse.SelectedValue));

            if (dsWarehouse.Tables[0].Rows.Count > 0)
            {
                // txtDeliveryAddress.Text = dsWarehouse.Tables[0].Rows[0]["sAddress"].ToString();
                // txtDeliveryDestination.Text = dsWarehouse.Tables[0].Rows[0]["sName"].ToString();
            }
        }
    }

    /*********** Oct.12.2015 END *************/

    protected void btnSaveOutOfCharge_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);

        DateTime ExamineDate = DateTime.MinValue;
        DateTime OutOfChargeDate = DateTime.MinValue;

        bool bExamineStatus = false;

        if (txtExamineDate.Text.Trim() == "" && txtOutOfChargeDate.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Examine or Out of Charge Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        try
        {
            if (txtExamineDate.Text.Trim() != "")
            {
                ExamineDate = Commonfunctions.CDateTime(txtExamineDate.Text.Trim());
            }
            if (txtOutOfChargeDate.Text.Trim() != "")
            {
                OutOfChargeDate = Commonfunctions.CDateTime(txtOutOfChargeDate.Text.Trim());
            }

            if (txtExamineDate.Text.Trim() != "" && txtOutOfChargeDate.Text.Trim() != "")
            {
                bExamineStatus = true;
            }

            int result = DBOperations.AddOutOfChargeExamineDetail(JobId, ExamineDate, OutOfChargeDate, bExamineStatus, false, "", LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Examine Details Updated Successfully!";
                lblError.CssClass = "success";
            }
            else if (result == 1)
            {
                lblError.Text = "Invalid OOC/Examine Date!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }

        }
        catch (Exception ex)
        {
            lblError.Text = "Invalid Date Value! Please Enter Valid Date DD/MM/YYYYY";
            lblError.CssClass = "errorMsg";
            return;
        }

    }

    /*********************** Not Required *************************
        private void UpdateExpenseDetail()
        {
            string AdditionalExpenseIdList  =   "";

            int JobId = Convert.ToInt32(Session["JobId"]);
                
            foreach (ListItem item in chkAdditionalField.Items)
            {
                if (item.Selected)
                {
                    AdditionalExpenseIdList += item.Value + ",";
                }
            }

            if (AdditionalExpenseIdList != "")
            {
                int Result = DBOperations.AddJobAdditionalExpense(AdditionalExpenseIdList, JobId, LoggedInUser.glUserId);
            }
        }
        *****************************************************************/

    protected void ddVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDeliveryPoint.Enabled = true;
        txtTransporterName.Enabled = true;
        ddVehicleType.Enabled = true;
        ddTransporter.Enabled = true;

        DataView dsGetDetails = DBOperations.GetTransRateDetailById(Convert.ToInt32(ddVehicleNo.SelectedValue));

        if (dsGetDetails.Table.Rows.Count > 0)
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
            hdnTruckRequestDate.Value = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");

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
}