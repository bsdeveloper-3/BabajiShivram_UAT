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

public partial class WarehouseDelivery : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);

        if (Session["JobId"] == null)
        {
            Response.Redirect("InTransitWarehouse.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse Delivery Detail";

            string strJobId = Session["JobId"].ToString();

            // Fill Job Details

            DBOperations.FillVehicleType(ddVehicleType);
            DBOperations.FillVehicleNoForWarehouse(ddVehicleNo, Convert.ToInt32(strJobId));
            JobDetail(Convert.ToInt32(strJobId));

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
        string strCustDocFolder = "", strJobFileDir = "";

        DataView dvDetail = DBOperations.GetJobDetailForDelivery(JobId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            bool bIsOctroi = false, bIsSForm = false, bIsNForm = false, bIsRoadPermit = false;
            int TransitType = 0;
            bool bTransportationByBabaji = false;
            int intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveredPackages"]);
            int intBoeType = Convert.ToInt32(dvDetail.Table.Rows[0]["BoeType"]);
            int PortId = Convert.ToInt32(dvDetail.Table.Rows[0]["PortId"]);

            lblBalancePackage.Text = intBalancePackage.ToString() + "/" + dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            hdnBalancePkg.Value = intBalancePackage.ToString();

            lblDeliveredContainer.Text = dvDetail.Table.Rows[0]["DeliveredContainer"].ToString();

            if (dvDetail.Table.Rows[0]["TransportationByBabaji"] != DBNull.Value)
            {
                bTransportationByBabaji = Convert.ToBoolean(dvDetail.Table.Rows[0]["TransportationByBabaji"]);

                // Fill All Transporter List

                DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));

                // Get Transporter List Assigne By Transport Dept
                //DBOperations.FillTransporterPlaced(ddTransporter, JobId, 0);

                if (bTransportationByBabaji == true)
                {
                    rdlTransport.SelectedValue = "1";
                    ddTransporter.Visible = true;
                    RFVTransID.Enabled = true;

                    txtTransporterName.Visible = false;
                    RFVTransName.Enabled = false;
                }
                else
                {
                    rdlTransport.SelectedValue = "0";
                    txtTransporterName.Visible = true;
                    RFVTransName.Enabled = true;

                    ddTransporter.Visible = false;
                    RFVTransID.Enabled = false;
                }

            }
            if (dvDetail.Table.Rows[0]["TransitType"] != DBNull.Value)
            {
                TransitType = Convert.ToInt32(dvDetail.Table.Rows[0]["TransitType"]);
            }

            lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Warehouse Delivery - " + lblJobRefNo.Text;

            lblCustRefNo.Text = dvDetail.Table.Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            lblConsigneeName.Text = dvDetail.Table.Rows[0]["Consignee"].ToString();
            lblFFName.Text = dvDetail.Table.Rows[0]["FFName"].ToString();
            lblMode.Text = dvDetail.Table.Rows[0]["Mode"].ToString();
            lblShortDesc.Text = dvDetail.Table.Rows[0]["ShortDescription"].ToString();
            lblNoOfPackages.Text = dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWT"].ToString();
            lblShipmentType.Text = dvDetail.Table.Rows[0]["ShipmentType"].ToString();

            if (dvDetail.Table.Rows[0]["TransportationBy"].ToString().ToLower().Trim() == "customer")
            {
                txtVehicleNo.Visible = true;
                ddVehicleNo.Visible = false;
            }
            else
            {
                txtVehicleNo.Visible = false;
                ddVehicleNo.Visible = true;
            }

            if (dvDetail.Table.Rows[0]["PackageTypeName"] != DBNull.Value)
            {
                lblPackageType.Text = dvDetail.Table.Rows[0]["PackageTypeName"].ToString();
            }

            if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
            {
                hdnDeliveryTypeId.Value = dvDetail.Table.Rows[0]["DeliveryType"].ToString();
            }

            // Job Directory Path

            if (dvDetail.Table.Rows[0]["DocFolder"] != DBNull.Value)
                strCustDocFolder = dvDetail.Table.Rows[0]["DocFolder"].ToString() + "\\";

            if (dvDetail.Table.Rows[0]["FileDirName"] != DBNull.Value)
                strJobFileDir = dvDetail.Table.Rows[0]["FileDirName"].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

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
            //if (dvDetail.Table.Rows[0]["IsNForm"] != DBNull.Value)
            //{
            //    bIsNForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsNForm"]);
            //    if (bIsNForm == true)
            //        pnlNFormApplicable.Visible = true;
            //}
            if (dvDetail.Table.Rows[0]["IsRoadPermit"] != DBNull.Value)
            {
                bIsRoadPermit = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRoadPermit"]);
                if (bIsRoadPermit == true)
                    pnlRoadPermitApplicable.Visible = true;
            }
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

                        // Moved To Customer, Transit Type = 1
                        DBOperations.FillPendingContainerDetail(ddContainerNo, JobId, 1);
                    }
                    else
                    {
                        // Show/hide Container/Package Div

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

            DataSet dsGetTruckReqDetail = DBOperations.GetTruckRequestByJobId(JobId);
            if (dsGetTruckReqDetail != null && dsGetTruckReqDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetTruckReqDetail.Tables[0].Rows[0]["lId"] != DBNull.Value)
                {
                    if (dsGetTruckReqDetail.Tables[0].Rows[0]["RequestDate"] != DBNull.Value)
                    {
                        rdlTransport.SelectedValue = "1";
                        rdlTransport.Enabled = false;
                    }
                }
            }

            DisplayVehicleNo(JobId);
        }//END_IF_RowCount
        else
        {
            btnSubmit.Visible = false;
            lblError.Text = "Job Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DisplayVehicleNo(int JobId)
    {
        if (JobId > 0)
        {
            DataSet dsGetTruckReqDetail = DBOperations.GetTruckRequestByJobId(JobId);
            if (dsGetTruckReqDetail != null && dsGetTruckReqDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetTruckReqDetail.Tables[0].Rows[0]["lId"] != DBNull.Value)
                {
                    if (dsGetTruckReqDetail.Tables[0].Rows[0]["RequestDate"] != DBNull.Value)
                    {
                        DateTime dtRequestDate = DateTime.MinValue, dtCompareDateWith = DateTime.MinValue;
                        dtCompareDateWith = Convert.ToDateTime("06/02/2019");
                        dtRequestDate = Convert.ToDateTime(dsGetTruckReqDetail.Tables[0].Rows[0]["RequestDate"]);
                        if (dtRequestDate >= dtCompareDateWith)
                        {
                            rdlTransport.Enabled = false;
                            txtVehicleNo.Visible = false;
                            ddVehicleNo.Visible = true;
                            rfvddlVehicleNo.Visible = true;
                            RFVVehicleNo.Visible = false;
                        }
                        else
                        {
                            rdlTransport.Enabled = true;
                            txtVehicleNo.Visible = true;
                            ddVehicleNo.Visible = false;
                            rfvddlVehicleNo.Visible = false;
                            RFVVehicleNo.Visible = true;
                        }
                    }
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int Result = -123;

        int JobId = Convert.ToInt32(Session["JobId"]);
        int NoOfPackages = 0, ContainerId = 0, VehicleType = 0;
        int TransitType = 1; // Move To Customer Place
        int TransporterID = 0;

        Boolean TransportationByBabaji = false;
        string strTransporterName, strLRNo, strDeliveryPoint, VehicleNo, RoadPermitNo, CargoReceivedBy,
            PODPath = "", ChallanPath = "", DamageCopyPath = "",
            NFormNo, SFormNo, OctroiReciptNo, OctroiAmount, strBabajiChallanNo, strDriverName, strDriverPhone;

        DateTime TruckReqDate = DateTime.MinValue, VehicleRcvdDate = DateTime.MinValue,
            LRDate = DateTime.MinValue, DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
            EmptyContRetrunDate = DateTime.MinValue, RoadPermitDate = DateTime.MinValue,
            NFormDate = DateTime.MinValue, SFormDate = DateTime.MinValue,
            NClosingDate = DateTime.MinValue, SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue,
            BabajiChallanDate = DateTime.MinValue;

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
        }
        else
        {
            lblError.Text = "Please Enter Dispatch Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (rdlTransport.SelectedValue == "0")  // customer
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
        else //babaji
        {
            if (ddVehicleNo.Visible == true && ddVehicleNo != null)
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
            else
            {
                VehicleNo = txtVehicleNo.Text.Trim();
            }
        }

        //if (txtVehicleNo.Text.Trim() == "")
        //{
        //    lblError.Text = "Please Enter Vehicle Number!";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

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

        if (rdlTransport.SelectedValue == "1")
        {
            TransportationByBabaji = true;
            TransporterID = Convert.ToInt32(ddTransporter.SelectedValue);
            strTransporterName = ddTransporter.SelectedItem.Text;
        }
        else
        {
            TransportationByBabaji = false;
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


        Result = DBOperations.AddDeliveryWarehouse(JobId, ContainerId, NoOfPackages, VehicleNo, TruckReqDate, VehicleRcvdDate,
            strTransporterName, TransporterID, TransportationByBabaji,
            strLRNo, LRDate, strDeliveryPoint, DispatchDate, DeliveryDate, EmptyContRetrunDate, RoadPermitNo, RoadPermitDate,
            CargoReceivedBy, PODPath, NFormNo, NFormDate, NClosingDate, SFormNo, SFormDate, SClosingDate, OctroiAmount, OctroiReciptNo, OctroiPaidDate,
            VehicleType, strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath,
            TransitType, strDriverName, strDriverPhone, LoggedInUser.glUserId);

        if (Result == 0)
        {
            if (rdlTransport.SelectedValue == "1") // babaji transportation
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
            DBOperations.FillVehicleNoForWarehouse(ddVehicleNo, JobId);
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
            DBOperations.FillVehicleNoForWarehouse(ddVehicleNo, JobId);
            JobDetail(JobId);
            GridViewVehicle.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "Error! Please check the required field.";
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
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("InTransitWarehouse.aspx");
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
                txtVehicleNo.Focus();
            }
        }
    }

    protected void rdlTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlTransport.SelectedValue == "1")
        {
            ddTransporter.Visible = true;
            RFVTransID.Enabled = true;

            txtTransporterName.Visible = false;
            RFVTransName.Enabled = false;
            rfvddlVehicleNo.Visible = true;
            RFVVehicleNo.Visible = false;
            DisplayVehicleNo(Convert.ToInt32(Session["JobId"]));
        }
        else
        {
            txtTransporterName.Visible = true;
            RFVTransName.Enabled = true;

            ddTransporter.Visible = false;
            RFVTransID.Enabled = false;
            rfvddlVehicleNo.Visible = false;
            RFVVehicleNo.Visible = true;
            DisplayVehicleNo(Convert.ToInt32(Session["JobId"]));
        }
    }

    public string UploadPODFiles(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;

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
                string ext = Path.GetExtension(fuDocument.FileName);
                FileName = Path.GetFileNameWithoutExtension(fuDocument.FileName);
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

    protected void ddVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataView dsGetDetails = DBOperations.GetTransRateDetailById(Convert.ToInt32(ddVehicleNo.SelectedValue));
        if (dsGetDetails != null)
        {
            DBOperations.FillCompanyByCategory(ddTransporter, 6);
            DBOperations.FillVehicleType(ddVehicleType);
            txtDeliveryPoint.Text = dsGetDetails.Table.Rows[0]["Destination"].ToString();
            txtTransporterName.Text = dsGetDetails.Table.Rows[0]["TransporterName"].ToString();
            ddTransporter.SelectedValue = dsGetDetails.Table.Rows[0]["TransporterId"].ToString();
            ddVehicleType.SelectedValue = dsGetDetails.Table.Rows[0]["VehicleTypeId"].ToString();
            //txtLRNo.Text = dsGetDetails.Table.Rows[0]["LRNo"].ToString();
            //txtLRDate.Text = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["LRDate"]).ToString("dd/MM/yyyy");
            //txtBabajiChallanNo.Text = dsGetDetails.Table.Rows[0]["ChallanNo"].ToString();
            //txtBabajiChallanDate.Text = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["ChallanDate"]).ToString("dd/MM/yyyy");
            //hdnTruckRequestDate.Value = Convert.ToDateTime(dsGetDetails.Table.Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");

            //txtDeliveryPoint.Enabled = false;
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