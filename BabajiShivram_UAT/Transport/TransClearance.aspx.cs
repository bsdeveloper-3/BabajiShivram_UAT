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
using System.Net.Mail;
using System.IO;
using System.Text;
using AjaxControlToolkit;
public partial class Transport_TransClearance : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        wzDelivery.PreRender += new EventHandler(wzDelivery_PreRender);

        if (Session["ConsolidateJob"] == null)
        {
            Response.Redirect("PendingTransDelivery.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Consolidate - Delivery Detail";

            DBOperations.FillVehicleType(ddVehicleType);
            string strConsolidateJobId = Session["ConsolidateJob"].ToString();

            MEditValReceivedDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
            MEditValDispatchDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
            if (Session["TrConsolidateId"] != null)
            {
                GetVehicleDetail(Convert.ToInt32(Session["TrConsolidateId"]));
            }
        }
    }

    protected void GetVehicleDetail(int TRConsolidateId)
    {
        if (TRConsolidateId > 0)
        {
            DataSet dsGetDetail = DBOperations.GetTransRateDetail(TRConsolidateId);
            if (dsGetDetail != null && dsGetDetail.Tables.Count > 0 && dsGetDetail.Tables[0].Rows.Count > 0)
            {
                hdnTransRateId.Value = dsGetDetail.Tables[0].Rows[0]["lid"].ToString();
                txtVehicleNo.Text = dsGetDetail.Tables[0].Rows[0]["VehicleNo"].ToString();
                ddTransporter.SelectedValue = dsGetDetail.Tables[0].Rows[0]["TransporterId"].ToString();
                ddVehicleType.DataBind();
                ddVehicleType.SelectedValue = dsGetDetail.Tables[0].Rows[0]["VehicleTypeId"].ToString();
                hdnDestination.Value = dsGetDetail.Tables[0].Rows[0]["DeliveryPoint"].ToString();
                txtVehicleNo.Enabled = false;
                ddTransporter.Enabled = false;
                ddVehicleType.Enabled = false;
                rdlTransport.Enabled = false;
            }
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Session["ConsolidateJob"] = null;
        Session["CommonCustomer"] = null;

        Response.Redirect("PendingTransDelivery.aspx");
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

    protected void rdlTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlTransport.SelectedValue == "1")
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

    #region Wizard Event

    private string[] StepCount;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["CommonCustomer"] != null)
        {
            DataView dvDetail = new DataView();
            StepCount = Session["ConsolidateJob"].ToString().Split(',');

            for (int i = 1; i < StepCount.Length; i++)
            {
                string strfff = StepCount[i].ToString();
                string strJobRefNo = "", strCustDocFolder = "", strJobFileDir = "";
                int JobId = Convert.ToInt32(StepCount[i - 1]);
                int DeliveryTypeId = 0, TransitType = 0, WarehouseId = 0;
                bool bIsOctroi = false, bIsSForm = false, bIsNForm = false, bIsRoadPermit = false, IsRunwayDelivery = false;

                dvDetail = DBOperations.GetJobDetailForDelivery(JobId);

                strJobRefNo = dvDetail.Table.Rows[0]["JobRefNo"].ToString(); ;

                int BranchId = Convert.ToInt32(dvDetail.Table.Rows[0]["BabajiBranchId"]);
                int PortId = Convert.ToInt32(dvDetail.Table.Rows[0]["PortId"]);

                WizardStepBase ws = new WizardStep();
                wzDelivery.WizardSteps.AddAt(i, ws);
                ws.StepType = WizardStepType.Step;

                ws.ID = "Stepp1" + i.ToString();
                ws.Title = strJobRefNo;

                /***********User Control For Delivery Detail **************************************/
                UserControl ucDelivery = (UserControl)LoadControl("~/DynamicData/Content/UCDelivery.ascx");

                /************ Find control in UserControl ************/

                Label lblJobRefNo = (Label)ucDelivery.FindControl("lblJobRefNo");
                HiddenField hdnJobId = (HiddenField)ucDelivery.FindControl("hdnJobId");
                HiddenField hdnBrachId = (HiddenField)ucDelivery.FindControl("hdnBranchId");
                Label lblBalancePackage = (Label)ucDelivery.FindControl("lblBalancePackage");
                HiddenField hdnBalPackage = (HiddenField)ucDelivery.FindControl("hdnBalPackage");
                HiddenField hdnUploadPath = (HiddenField)ucDelivery.FindControl("hdnUploadPath");
                Panel pnlLRDetail = (Panel)ucDelivery.FindControl("pnlLRDetail");
                Panel pnlOctroiApplicable = (Panel)ucDelivery.FindControl("pnlOctroiApplicable");
                Panel pnlSFormApplicable = (Panel)ucDelivery.FindControl("pnlSFormApplicable");
                Panel pnlNFormApplicable = (Panel)ucDelivery.FindControl("pnlNFormApplicable");
                Panel pnlRoadPermitApplicable = (Panel)ucDelivery.FindControl("pnlRoadPermitApplicable");
                HtmlGenericControl divExpenseAir = (HtmlGenericControl)ucDelivery.FindControl("divExpenseAir");
                HtmlGenericControl divExpenseSea = (HtmlGenericControl)ucDelivery.FindControl("divExpenseSea");
                RadioButtonList rdlRunwayDelivery = (RadioButtonList)ucDelivery.FindControl("rdlRunwayDelivery");
                DropDownList ddLabourType = (DropDownList)ucDelivery.FindControl("ddLabourType");

                DropDownList ddTransitType = (DropDownList)ucDelivery.FindControl("ddTransitType");
                DropDownList ddWarehouse = (DropDownList)ucDelivery.FindControl("ddWarehouse");

                MaskedEditValidator MEditValPackage = (MaskedEditValidator)ucDelivery.FindControl("MEditValPackage");

                TextBox txtDestination = (TextBox)ucDelivery.FindControl("txtDestination");
                DataSet dsGetDetail = DBOperations.GetTransRateDetail(Convert.ToInt32(Session["TrConsolidateId"]));
                if (dsGetDetail != null && dsGetDetail.Tables.Count > 0 && dsGetDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsGetDetail.Tables[0].Rows[0]["DeliveryPoint"] != DBNull.Value)
                    {
                        txtDestination.Text = dsGetDetail.Tables[0].Rows[0]["DeliveryPoint"].ToString();
                    }
                }

                // Out Of Charge Completion Completion Check

                TextBox txtExamineDate = (TextBox)ucDelivery.FindControl("txtExamineDate");
                TextBox txtOutOfChargeDate = (TextBox)ucDelivery.FindControl("txtOutOfChargeDate");

                if (dvDetail.Table.Rows[0]["ExamineDate"] != DBNull.Value)
                {
                    txtExamineDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["ExamineDate"]).ToString("dd/MM/yyyy");
                    txtExamineDate.Enabled = false;
                }

                if (dvDetail.Table.Rows[0]["OutOfChargeDate"] != DBNull.Value)
                {
                    txtOutOfChargeDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["OutOfChargeDate"]).ToString("dd/MM/yyyy");
                    txtOutOfChargeDate.Enabled = false;
                }

                if (dvDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
                {
                    DeliveryTypeId = Convert.ToInt16(dvDetail.Table.Rows[0]["DeliveryType"]);
                }

                if (dvDetail.Table.Rows[0]["TransitType"] != DBNull.Value)
                {
                    TransitType = Convert.ToInt32(dvDetail.Table.Rows[0]["TransitType"]);
                }

                if (TransitType == 1) // Moved to customer place
                {
                    ddTransitType.SelectedValue = "1";
                    ddTransitType.Enabled = false;
                }
                else if (TransitType == 2 || TransitType == 3) // Moved to Warehouse
                {
                    ddTransitType.SelectedValue = TransitType.ToString();
                    ddTransitType.Enabled = false;

                    if (dvDetail.Table.Rows[0]["WarehouseId"] != DBNull.Value)
                    {
                        WarehouseId = Convert.ToInt32(dvDetail.Table.Rows[0]["WarehouseId"]);
                        ddWarehouse.Visible = true;
                        ddWarehouse.SelectedValue = WarehouseId.ToString();

                    }

                }

                // If Job port is Mumbai - AIR show Runway Delivery Yes/No Checkbox list
                if (PortId == 4)
                {
                    divExpenseAir.Visible = true;
                }
                else if (PortId == 5 && DeliveryTypeId == (int)DeliveryType.DeStuff)
                {
                    // Port JNPT and for Delivery Type DeStuff Show Labour Charges
                    divExpenseSea.Visible = true;
                }

                // Fill Job delivery history

                SqlDataSource DataSourceBalance = (SqlDataSource)ucDelivery.FindControl("DataSourceBalance");
                DataSourceBalance.SelectParameters["JobId"].DefaultValue = JobId.ToString();

                lblJobRefNo.Text = strJobRefNo;
                hdnJobId.Value = JobId.ToString();
                hdnBrachId.Value = BranchId.ToString();

                int intBalancePackage = 0;

                if (TransitType > 1) // Balance From Warehouse Delivery
                {
                    intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["WarehousePackages"]);
                }
                else
                {
                    intBalancePackage = Convert.ToInt32(dvDetail.Table.Rows[0]["NoOfPackages"]) - Convert.ToInt32(dvDetail.Table.Rows[0]["DeliveredPackages"]);
                }

                lblBalancePackage.Text = intBalancePackage.ToString();
                hdnBalPackage.Value = intBalancePackage.ToString();

                // Package validation for Max allowed 

                MEditValPackage.MaximumValue = intBalancePackage.ToString();

                if (dvDetail.Table.Rows[0]["DocFolder"] != DBNull.Value)
                    strCustDocFolder = dvDetail.Table.Rows[0]["DocFolder"].ToString() + "\\";

                if (dvDetail.Table.Rows[0]["FileDirName"] != DBNull.Value)
                    strJobFileDir = dvDetail.Table.Rows[0]["FileDirName"].ToString() + "\\";

                hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

                // Show/Hide LR Detail Panel for Same Customer

                if (Session["CommonCustomer"] != null)
                {
                    if (Convert.ToBoolean(Session["CommonCustomer"]) == true)
                    {
                        pnlLRDetail.Visible = false;

                        if (i == 1)
                        {
                            hdnCommonUploadPath.Value = strCustDocFolder + strJobFileDir;
                        }
                    }
                    else
                    {
                        pnlLRDetail.Visible = true;
                    }
                }

                // Show Hide Applicable Fields Panel
                if (dvDetail.Table.Rows[0]["IsOctroi"] != DBNull.Value)
                {
                    bIsOctroi = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsOctroi"]);
                    if (bIsOctroi == true)
                        pnlOctroiApplicable.Visible = false;
                }
                if (dvDetail.Table.Rows[0]["IsSForm"] != DBNull.Value)
                {
                    bIsSForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsSForm"]);
                    if (bIsSForm == true)
                        pnlSFormApplicable.Visible = false;
                }
                if (dvDetail.Table.Rows[0]["IsNForm"] != DBNull.Value)
                {
                    bIsNForm = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsNForm"]);
                    if (bIsNForm == true)
                        pnlNFormApplicable.Visible = false;
                }
                if (dvDetail.Table.Rows[0]["IsRoadPermit"] != DBNull.Value)
                {
                    bIsRoadPermit = Convert.ToBoolean(dvDetail.Table.Rows[0]["IsRoadPermit"]);
                    if (bIsRoadPermit == true)
                        pnlRoadPermitApplicable.Visible = false;
                }

                ws.Controls.Add(ucDelivery);

                /****************************************/

            }

            /**************************************************
            // Add Finish Step
            WizardStepBase wsFinish = new WizardStep();
            wsFinish.StepType = WizardStepType.Finish;
            wsFinish.ID = "StepFinish";
            wsFinish.Title = "Update";
            
            wzDelivery.WizardSteps.Add(wsFinish);
            
            // Add Complete Step
            WizardStepBase wsComplete = new WizardStep();
            wsComplete.StepType = WizardStepType.Complete;
            wsComplete.ID = "StepComplete";
            wsComplete.Title = "Close";
            
            wzDelivery.WizardSteps.Add(wsComplete);
            *************************************************/
        }

        if (Session["CommonCustomer"] != null)
        {
            if (Convert.ToBoolean(Session["CommonCustomer"]) == true)
            {
                pnlCommonLRDetail.Visible = true;
            }
        }
    }

    protected void wzDelivery_PreRender(object sender, EventArgs e)
    {
        Repeater SideBarList1 = wzDelivery.FindControl("HeaderContainer").FindControl("SideBarList") as Repeater;
        SideBarList1.DataSource = wzDelivery.WizardSteps;
        SideBarList1.DataBind();
    }

    protected string GetClassForWizardStep(object wizardStep)
    {
        WizardStep step = wizardStep as WizardStep;

        if (step == null)
        {
            return "";
        }
        int stepIndex = wzDelivery.WizardSteps.IndexOf(step);

        if (stepIndex < wzDelivery.ActiveStepIndex)
        {
            return "prevStep";
        }
        else if (stepIndex > wzDelivery.ActiveStepIndex)
        {
            return "nextStep";
        }
        else
        {
            return "currentStep";
        }
    }

    protected void wzDelivery_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        //  string strUserControlUniqueID    =   "ctl00$ContentPlaceHolder1$wzDelivery$ctl0" + IndexId.ToString();

        int IndexId = e.CurrentStepIndex;

        int TotalSteps = wzDelivery.WizardSteps.Count;

        int ConsolidateId = 0, JobId = 0, DeliveryId = 0, NoOfPackages = 0, VehicleType = 0, LabourTypeId = 0, BalancePackage = 0;
        int WarehouseId = 0, TransitType = 0, TransporterId = 0;

        ConsolidateId = Convert.ToInt32(hdnConsolidateID.Value);

        Session["ConsolidateId"] = ConsolidateId.ToString();

        bool IsCommonLR = false;

        string strVehicleNo = "", strTransporterName = "", strDeliveryPoint = "", strLRNo = "";

        string RoadPermitNo = "", LRPath = "", ChallanPath = "", DamageCopyPath = "",
            NFormNo = "", SFormNo = "", OctroiReciptNo = "", OctroiAmount = "", strBabajiChallanNo = "", strIsRunwayDelivery = "";

        DateTime VehicleRcvdDate = DateTime.MinValue, LRDate = DateTime.MinValue, TruckReqDate = DateTime.MinValue,
            DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
            RoadPermitDate = DateTime.MinValue, NFormDate = DateTime.MinValue, SFormDate = DateTime.MinValue,
            NClosingDate = DateTime.MinValue, SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue,
            BabajiChallanDate = DateTime.MinValue, ExamineDate = DateTime.MinValue, OutOfChargeDate = DateTime.MinValue;

        TransporterId = Convert.ToInt32(ddTransporter.SelectedValue);

        if (TransporterId > 0)
        {
            strTransporterName = ddTransporter.SelectedItem.Text.Trim();
        }
        else
        {
            strTransporterName = txtTransporterName.Text.Trim();
        }


        strVehicleNo = txtVehicleNo.Text.Trim();
        VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);


        if (txtVehicleRecdDate.Text.Trim() != "")
        {
            VehicleRcvdDate = Commonfunctions.CDateTime(txtVehicleRecdDate.Text.Trim());
        }
        if (txtDispatchDate.Text.Trim() != "")
        {
            DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
        }

        if (Session["CommonCustomer"] != null)
        {
            if (Convert.ToBoolean(Session["CommonCustomer"]) == true)
            {
                IsCommonLR = true;

                strLRNo = txtCommonLRNo.Text.ToUpper().Trim();

                if (txtCommonLRDate.Text.Trim() != "")
                {
                    LRDate = Commonfunctions.CDateTime(txtCommonLRDate.Text.Trim());
                }

                // Upload Common LR

                if (fuCommonLRUpload.HasFile)
                {
                    hdnCommonLRPath.Value = UploadPODFiles(fuCommonLRUpload, hdnCommonUploadPath.Value);
                }
            }
            else
            {
                IsCommonLR = false;
            }
        }

        // Add/Update Delivery Detail Master Record and return Consolidate ID

        if (IndexId == 0)
        {
            hdnConsolidateID.Value = DBOperations.AddDeliveryConsolidateMS(ConsolidateId, strVehicleNo, DispatchDate,
                strTransporterName, TransporterId, IsCommonLR, LoggedInUser.glUserId, true).ToString();

        }
        else if (IndexId > 0)
        {

            string strNamePrefix = "ctl0" + IndexId.ToString();

            if (IndexId >= 10)
            {
                strNamePrefix = "ctl" + IndexId.ToString();
            }

            WizardStepBase wsStep = wzDelivery.WizardSteps[IndexId];

            UserControl ucControl = (UserControl)wsStep.FindControl(strNamePrefix);

            if (ucControl != null)
            {
                HiddenField hdnJobId = (HiddenField)ucControl.FindControl("hdnJobId");
                HiddenField hdnDeliveryId = (HiddenField)ucControl.FindControl("hdnDeliveryId");
                HiddenField hdnUploadPath = (HiddenField)ucControl.FindControl("hdnUploadPath");
                Label lblJobRefNo = (Label)ucControl.FindControl("lblJobRefNo");
                TextBox txtNoOfPackages = (TextBox)ucControl.FindControl("txtNoOfPackages");
                HiddenField hdnBalPackage = (HiddenField)ucControl.FindControl("hdnBalPackage");

                TextBox txtDestination = (TextBox)ucControl.FindControl("txtDestination");

                TextBox txtLRNo = (TextBox)ucControl.FindControl("txtLRNo");
                TextBox txtLRDate = (TextBox)ucControl.FindControl("txtLRDate");

                TextBox txtOctroiAmount = (TextBox)ucControl.FindControl("txtOctroiAmount");
                TextBox txtOctroiReceiptNo = (TextBox)ucControl.FindControl("txtOctroiReceiptNo");
                TextBox txtOctroiPaidDate = (TextBox)ucControl.FindControl("txtOctroiPaidDate");

                TextBox txtSFormNo = (TextBox)ucControl.FindControl("txtSFormNo");
                TextBox txtSFormDate = (TextBox)ucControl.FindControl("txtSFormDate");
                TextBox txtSClosingDate = (TextBox)ucControl.FindControl("txtSClosingDate");

                TextBox txtNFormNo = (TextBox)ucControl.FindControl("txtNFormNo");
                TextBox txtNFormDate = (TextBox)ucControl.FindControl("txtNFormDate");
                TextBox txtNClosingDate = (TextBox)ucControl.FindControl("txtNClosingDate");


                TextBox txtRoadPermitNo = (TextBox)ucControl.FindControl("txtRoadPermitNo");
                TextBox txtRoadPermitDate = (TextBox)ucControl.FindControl("txtRoadPermitDate");

                TextBox txtBabajiChallanNo = (TextBox)ucControl.FindControl("txtBabajiChallanNo");
                TextBox txtBabajiChallanDate = (TextBox)ucControl.FindControl("txtBabajiChallanDate");

                // File Upload

                FileUpload fuLRCopy = (FileUpload)ucControl.FindControl("fuLRCopy");
                FileUpload fuChallanCopy = (FileUpload)ucControl.FindControl("fuChallanCopy");
                FileUpload fuDamageCopy = (FileUpload)ucControl.FindControl("fuDamageCopy");

                HiddenField hdnLRPath = (HiddenField)ucControl.FindControl("hdnLRPath");
                HiddenField hdnChallanPath = (HiddenField)ucControl.FindControl("hdnChallanPath");
                HiddenField hdnDamagePath = (HiddenField)ucControl.FindControl("hdnDamagePath");

                // Expense
                RadioButtonList rdlRunwayDelivery = (RadioButtonList)ucControl.FindControl("rdlRunwayDelivery");
                DropDownList ddLabourType = (DropDownList)ucControl.FindControl("ddLabourType");

                // Panel
                Panel pnlLRDetail = (Panel)ucControl.FindControl("pnlLRDetail");
                Panel pnlOctroiApplicable = (Panel)ucControl.FindControl("pnlOctroiApplicable");
                Panel pnlSFormApplicable = (Panel)ucControl.FindControl("pnlSFormApplicable");
                Panel pnlNFormApplicable = (Panel)ucControl.FindControl("pnlNFormApplicable");
                Panel pnlRoadPermitApplicable = (Panel)ucControl.FindControl("pnlRoadPermitApplicable");


                // Examine Field

                DropDownList ddTransitType = (DropDownList)ucControl.FindControl("ddTransitType");
                DropDownList ddWarehouse = (DropDownList)ucControl.FindControl("ddWarehouse");

                TextBox txtExamineDate = (TextBox)ucControl.FindControl("txtExamineDate");
                TextBox txtOutOfChargeDate = (TextBox)ucControl.FindControl("txtOutOfChargeDate");


                JobId = Convert.ToInt32(hdnJobId.Value);
                DeliveryId = Convert.ToInt32(hdnDeliveryId.Value);
                NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());
                strDeliveryPoint = txtDestination.Text.Trim();
                strBabajiChallanNo = txtBabajiChallanNo.Text.Trim();

                TransitType = Convert.ToInt32(ddTransitType.SelectedValue);

                if (ddWarehouse.SelectedIndex != -1)
                    WarehouseId = Convert.ToInt32(ddWarehouse.SelectedValue);

                if (txtExamineDate.Text.Trim() != "")
                {
                    ExamineDate = Commonfunctions.CDateTime(txtExamineDate.Text.Trim());
                }

                if (txtOutOfChargeDate.Text.Trim() != "")
                {
                    OutOfChargeDate = Commonfunctions.CDateTime(txtOutOfChargeDate.Text.Trim());
                }

                if (txtBabajiChallanDate.Text.Trim() != "")
                {
                    BabajiChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());
                }

                if (rdlRunwayDelivery != null)
                {
                    if (rdlRunwayDelivery.SelectedIndex != -1)
                    {
                        if (Convert.ToBoolean(rdlRunwayDelivery.SelectedValue) == true)
                            strIsRunwayDelivery = "yes";
                        else
                            strIsRunwayDelivery = "no";
                    }
                }

                if (ddLabourType != null)
                {
                    LabourTypeId = Convert.ToInt32(ddLabourType.SelectedValue);
                }

                if (IsCommonLR == true)
                {
                    // Get Common LR Uploaded Path
                    LRPath = hdnCommonLRPath.Value;
                }
                else
                {
                    // Upload Job LR Copy

                    if (fuLRCopy.HasFile)
                    {
                        hdnLRPath.Value = UploadPODFiles(fuLRCopy, hdnUploadPath.Value);
                    }

                    LRPath = hdnLRPath.Value;
                }

                if (fuChallanCopy.HasFile)
                {
                    hdnChallanPath.Value = UploadPODFiles(fuChallanCopy, hdnUploadPath.Value);
                }

                if (fuDamageCopy.HasFile)
                {
                    hdnDamagePath.Value = UploadPODFiles(fuDamageCopy, hdnUploadPath.Value);
                }

                ChallanPath = hdnChallanPath.Value;
                DamageCopyPath = hdnDamagePath.Value;

                // Add Consolidate delivery details

                int result = DBOperations.AddDeliveryConsolidateDetail(ConsolidateId, DeliveryId, JobId, NoOfPackages, strVehicleNo, TruckReqDate,
                    VehicleRcvdDate, strTransporterName, TransporterId, strLRNo, LRDate, strDeliveryPoint, DispatchDate, RoadPermitNo, RoadPermitDate,
                    LRPath, NFormNo, NFormDate, NClosingDate, SFormNo, SFormDate, SClosingDate, OctroiAmount, OctroiReciptNo, OctroiPaidDate,
                    VehicleType, strBabajiChallanNo, BabajiChallanDate, ChallanPath, DamageCopyPath, strIsRunwayDelivery, LabourTypeId,
                    TransitType, WarehouseId, ExamineDate, OutOfChargeDate, LoggedInUser.glUserId, true);

                if (result > 0)
                {
                    hdnDeliveryId.Value = result.ToString();
                }
                else if (result == -1)
                {
                    e.Cancel = true;
                    lblError.Text = "System Error Please try again!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == -2)
                {
                    e.Cancel = true;
                    lblError.Text = lblJobRefNo.Text + " Job Already Delivered." + " Please Cancel and pick different Job.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == -4)
                {
                    e.Cancel = true;
                    lblError.Text = lblJobRefNo.Text + " Supplied Package count greater then Available packages!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "System Error Please try again!";
                    lblError.CssClass = "errorMsg";
                }

            }//END_IF_Control_Null

        }// END_ELSE

        // Bind Delivery Review GridView at Finish Step        
        int nextStepindex = e.NextStepIndex;

        WizardStepBase wsFinish = wzDelivery.WizardSteps[nextStepindex];

        if (wsFinish.StepType == WizardStepType.Finish)
        {
            Session["ConsolidateId"] = hdnConsolidateID.Value;
            gvJobReview.DataBind();

            lblVehicleNo.Text = txtVehicleNo.Text;
            lblVehicleType.Text = ddVehicleType.SelectedItem.Text;
            lblTransporterName.Text = strTransporterName;
            lblDispatchDate.Text = txtDispatchDate.Text.Trim();
        }
    }

    protected void wzDelivery_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (Session["ConsolidateJob"] == null || Session["CommonCustomer"] == null)
        {
            e.Cancel = true;
            Response.Redirect("PendingTransDelivery.aspx");
        }
        else
        {
            int TotalSteps = wzDelivery.WizardSteps.Count;

            int ConsolidateID = 0, JobId = 0, DeliveryID = 0, NoOfPackages = 0, VehicleType = 0, LabourTypeId = 0;
            int WarehouseId = 0, TransitType = 0, TransporterId = 0;

            bool IsCommonLR = false;
            string strVehicleNo = "", strTransporterName = "", strDeliveryPoint = "", strLRNo = "";

            string RoadPermitNo = "", LRPath = "", ChallanPath = "", DamageCopyPath = "",
                NFormNo = "", SFormNo = "", OctroiReciptNo = "", OctroiAmount = "", strBabajiChallanNo = "", strIsRunwayDelivery = "";

            DateTime VehicleRcvdDate = DateTime.MinValue, LRDate = DateTime.MinValue, TruckReqDate = DateTime.MinValue,
                DispatchDate = DateTime.MinValue, DeliveryDate = DateTime.MinValue,
                RoadPermitDate = DateTime.MinValue, NFormDate = DateTime.MinValue, SFormDate = DateTime.MinValue,
                NClosingDate = DateTime.MinValue, SClosingDate = DateTime.MinValue, OctroiPaidDate = DateTime.MinValue,
                BabajiChallanDate = DateTime.MinValue, ExamineDate = DateTime.MinValue, OutOfChargeDate = DateTime.MinValue;

            ConsolidateID = Convert.ToInt32(hdnConsolidateID.Value);
            strVehicleNo = txtVehicleNo.Text.Trim();
            VehicleType = Convert.ToInt32(ddVehicleType.SelectedValue);

            TransporterId = Convert.ToInt32(ddTransporter.SelectedValue);

            if (TransporterId > 0)
            {
                strTransporterName = ddTransporter.SelectedItem.Text.Trim();
            }
            else
            {
                strTransporterName = txtTransporterName.Text.Trim();
            }

            if (Session["CommonCustomer"] != null)
            {
                if (Convert.ToBoolean(Session["CommonCustomer"]) == true)
                {
                    IsCommonLR = true;
                }
                else
                {
                    IsCommonLR = false;
                }
            }

            if (txtVehicleRecdDate.Text.Trim() != "")
            {
                VehicleRcvdDate = Commonfunctions.CDateTime(txtVehicleRecdDate.Text.Trim());
            }
            if (txtDispatchDate.Text.Trim() != "")
            {
                DispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());
            }

            // Update Delivery Detail Master Record and return Consolidate ID

            hdnConsolidateID.Value = DBOperations.AddDeliveryConsolidateMS(ConsolidateID, strVehicleNo, DispatchDate, strTransporterName,
                TransporterId, IsCommonLR, LoggedInUser.glUserId, false).ToString();

            // Add delivery detail in transport rate table

            if (hdnTransRateId.Value != "" && hdnTransRateId.Value != "0")
            {
                int AddRateDelivered = DBOperations.UpdateRateDeliveryStatus(Convert.ToInt32(hdnTransRateId.Value), LoggedInUser.glUserId);
            }

            for (int i = 1; i < TotalSteps - 2; i++)
            {
                string strNamePrefix = "ctl0" + i.ToString();

                if (i >= 10)
                {
                    strNamePrefix = "ctl" + i.ToString();
                }
                WizardStepBase wsStep = wzDelivery.WizardSteps[i];

                UserControl ucControl = (UserControl)wsStep.FindControl(strNamePrefix);

                if (ucControl != null)
                {
                    HiddenField hdnJobId = (HiddenField)ucControl.FindControl("hdnJobId");
                    HiddenField hdnDeliveryId = (HiddenField)ucControl.FindControl("hdnDeliveryId");
                    HiddenField hdnUploadPath = (HiddenField)ucControl.FindControl("hdnUploadPath");
                    Label lblJobRefNo = (Label)ucControl.FindControl("lblJobRefNo");
                    TextBox txtNoOfPackages = (TextBox)ucControl.FindControl("txtNoOfPackages");
                    HiddenField hdnBalPackage = (HiddenField)ucControl.FindControl("hdnBalPackage");

                    TextBox txtDestination = (TextBox)ucControl.FindControl("txtDestination");
                    //   TextBox txtDeliveryDate     = (TextBox)ucControl.FindControl("txtDeliveryDate");
                    //   TextBox txtCargoPersonName  = (TextBox)ucControl.FindControl("txtCargoPersonName");

                    TextBox txtLRNo = (TextBox)ucControl.FindControl("txtLRNo");
                    TextBox txtLRDate = (TextBox)ucControl.FindControl("txtLRDate");

                    TextBox txtOctroiAmount = (TextBox)ucControl.FindControl("txtOctroiAmount");
                    TextBox txtOctroiReceiptNo = (TextBox)ucControl.FindControl("txtOctroiReceiptNo");
                    TextBox txtOctroiPaidDate = (TextBox)ucControl.FindControl("txtOctroiPaidDate");

                    TextBox txtSFormNo = (TextBox)ucControl.FindControl("txtSFormNo");
                    TextBox txtSFormDate = (TextBox)ucControl.FindControl("txtSFormDate");
                    TextBox txtSClosingDate = (TextBox)ucControl.FindControl("txtSClosingDate");

                    TextBox txtNFormNo = (TextBox)ucControl.FindControl("txtNFormNo");
                    TextBox txtNFormDate = (TextBox)ucControl.FindControl("txtNFormDate");
                    TextBox txtNClosingDate = (TextBox)ucControl.FindControl("txtNClosingDate");


                    TextBox txtRoadPermitNo = (TextBox)ucControl.FindControl("txtRoadPermitNo");
                    TextBox txtRoadPermitDate = (TextBox)ucControl.FindControl("txtRoadPermitDate");

                    TextBox txtBabajiChallanNo = (TextBox)ucControl.FindControl("txtBabajiChallanNo");
                    TextBox txtBabajiChallanDate = (TextBox)ucControl.FindControl("txtBabajiChallanDate");

                    // File Upload

                    FileUpload fuLRCopy = (FileUpload)ucControl.FindControl("fuLRCopy");
                    FileUpload fuChallanCopy = (FileUpload)ucControl.FindControl("fuChallanCopy");
                    FileUpload fuDamageCopy = (FileUpload)ucControl.FindControl("fuDamageCopy");

                    HiddenField hdnLRPath = (HiddenField)ucControl.FindControl("hdnLRPath");
                    HiddenField hdnChallanPath = (HiddenField)ucControl.FindControl("hdnChallanPath");
                    HiddenField hdnDamagePath = (HiddenField)ucControl.FindControl("hdnDamagePath");

                    // Expense
                    RadioButtonList rdlRunwayDelivery = (RadioButtonList)ucControl.FindControl("rdlRunwayDelivery");
                    DropDownList ddLabourType = (DropDownList)ucControl.FindControl("ddLabourType");

                    // Panel

                    Panel pnlLRDetail = (Panel)ucControl.FindControl("pnlLRDetail");
                    Panel pnlSFormApplicable = (Panel)ucControl.FindControl("pnlSFormApplicable");
                    Panel pnlNFormApplicable = (Panel)ucControl.FindControl("pnlNFormApplicable");
                    Panel pnlOctroiApplicable = (Panel)ucControl.FindControl("pnlOctroiApplicable");
                    Panel pnlRoadPermitApplicable = (Panel)ucControl.FindControl("pnlRoadPermitApplicable");

                    // Examine Field

                    DropDownList ddTransitType = (DropDownList)ucControl.FindControl("ddTransitType");
                    DropDownList ddWarehouse = (DropDownList)ucControl.FindControl("ddWarehouse");

                    TextBox txtExamineDate = (TextBox)ucControl.FindControl("txtExamineDate");

                    TextBox txtOutOfChargeDate = (TextBox)ucControl.FindControl("txtOutOfChargeDate");

                    JobId = Convert.ToInt32(hdnJobId.Value);
                    DeliveryID = Convert.ToInt32(hdnDeliveryId.Value);
                    NoOfPackages = Convert.ToInt32(txtNoOfPackages.Text.Trim());
                    strDeliveryPoint = txtDestination.Text.Trim();
                    //  strCargoReceivedBy  = txtCargoPersonName.Text.Trim();
                    strBabajiChallanNo = txtBabajiChallanNo.Text.Trim();
                    strLRNo = txtLRNo.Text.Trim();

                    TransitType = Convert.ToInt32(ddTransitType.SelectedValue);

                    if (ddWarehouse.SelectedIndex != -1)
                        WarehouseId = Convert.ToInt32(ddWarehouse.SelectedValue);

                    if (txtExamineDate.Text.Trim() != "")
                    {
                        ExamineDate = Commonfunctions.CDateTime(txtExamineDate.Text.Trim());
                    }

                    if (txtOutOfChargeDate.Text.Trim() != "")
                    {
                        OutOfChargeDate = Commonfunctions.CDateTime(txtOutOfChargeDate.Text.Trim());
                    }

                    if (txtLRDate.Text.Trim() != "")
                    {
                        LRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());
                    }

                    if (txtBabajiChallanDate.Text.Trim() != "")
                    {
                        BabajiChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());
                    }

                    if (rdlRunwayDelivery != null)
                    {
                        if (rdlRunwayDelivery.SelectedIndex != -1)
                        {
                            if (Convert.ToBoolean(rdlRunwayDelivery.SelectedValue) == true)
                                strIsRunwayDelivery = "yes";
                            else
                                strIsRunwayDelivery = "no";
                        }
                    }

                    if (ddLabourType != null)
                    {
                        LabourTypeId = Convert.ToInt32(ddLabourType.SelectedValue);
                    }

                    if (IsCommonLR == true)
                    {
                        strLRNo = txtCommonLRNo.Text.ToUpper().Trim();

                        if (txtCommonLRDate.Text.Trim() != "")
                        {
                            LRDate = Commonfunctions.CDateTime(txtCommonLRDate.Text.Trim());
                        }

                        // Get Common LR Uploaded Path
                        LRPath = hdnCommonLRPath.Value;
                    }
                    else
                    {
                        // Upload Job LR Copy

                        if (fuLRCopy.HasFile)
                        {
                            hdnLRPath.Value = UploadPODFiles(fuLRCopy, hdnUploadPath.Value);
                        }

                        LRPath = hdnLRPath.Value;
                    }

                    if (fuChallanCopy.HasFile)
                    {
                        hdnChallanPath.Value = UploadPODFiles(fuChallanCopy, hdnUploadPath.Value);
                    }

                    if (fuDamageCopy.HasFile)
                    {
                        hdnDamagePath.Value = UploadPODFiles(fuDamageCopy, hdnUploadPath.Value);
                    }

                    //

                    if (pnlNFormApplicable.Visible == true)
                    {
                        NFormNo = txtNFormNo.Text.ToUpper().Trim();

                        if (txtNFormDate.Text.Trim() != "")
                        {
                            NFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim());
                        }
                        if (txtNClosingDate.Text.Trim() != "")
                        {
                            NClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim());
                        }
                    }

                    if (pnlSFormApplicable.Visible == true)
                    {
                        SFormNo = txtSFormDate.Text.ToUpper().Trim();

                        if (txtSFormDate.Text.Trim() != "")
                        {
                            SFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim());
                        }

                        if (txtSClosingDate.Text.Trim() != "")
                        {
                            SClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim());
                        }
                    }

                    if (pnlOctroiApplicable.Visible == true)
                    {
                        OctroiReciptNo = txtOctroiReceiptNo.Text.ToUpper().Trim();
                        OctroiAmount = txtOctroiAmount.Text.Trim();

                        if (txtOctroiPaidDate.Text.Trim() != "")
                        {
                            OctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim());
                        }
                    }

                    if (pnlRoadPermitApplicable.Visible == true)
                    {
                        RoadPermitNo = txtRoadPermitNo.Text.ToUpper().Trim();

                        if (txtRoadPermitDate.Text.Trim() != "")
                        {
                            RoadPermitDate = Commonfunctions.CDateTime(txtRoadPermitDate.Text.Trim());
                        }
                    }

                    ChallanPath = hdnChallanPath.Value;
                    DamageCopyPath = hdnDamagePath.Value;

                    // Add Consolidate delivery details

                    hdnDeliveryId.Value = DBOperations.AddDeliveryConsolidateDetail(ConsolidateID, DeliveryID, JobId, NoOfPackages,
                            strVehicleNo, TruckReqDate, VehicleRcvdDate, strTransporterName, TransporterId, strLRNo, LRDate, strDeliveryPoint,
                            DispatchDate, RoadPermitNo, RoadPermitDate, LRPath, NFormNo, NFormDate, NClosingDate, SFormNo, SFormDate,
                            SClosingDate, OctroiAmount, OctroiReciptNo, OctroiPaidDate, VehicleType, strBabajiChallanNo, BabajiChallanDate,
                            ChallanPath, DamageCopyPath, strIsRunwayDelivery, LabourTypeId,
                            TransitType, WarehouseId, ExamineDate, OutOfChargeDate, LoggedInUser.glUserId, false).ToString();

                }//END_IF

            }// END_FOR

        }//END_ELSE

        // Clear all session values;
        Session["JobId"] = null;

    }

    #endregion
}