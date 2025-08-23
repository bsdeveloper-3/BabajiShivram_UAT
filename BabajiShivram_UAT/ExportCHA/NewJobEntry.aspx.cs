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
using AjaxControlToolkit;

public partial class ExportCHA_NewJobEntry : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("PendingExportJob.aspx");

        ScriptManager1.RegisterPostBackControl(rpDocument);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnAddContainer);
        ScriptManager1.RegisterPostBackControl(btnCancelContainer);
        ScriptManager1.RegisterPostBackControl(ddlPreAlertExport);

        if (!IsPostBack)
        {
            // ViewState["PreCustId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Creation";
            rpDocument.DataSource = DBOperations.FillChekListDocDetail(11);
            rpDocument.DataBind();
            ddlTransportBy_OnSelectedIndexChanged(null, EventArgs.Empty);
            ddlMode_OnSelectedIndexChanged(null, EventArgs.Empty);
            ddlForwardedBy_OnSelectedIndexChanged(null, EventArgs.Empty);

            ///////////////////////////  CONTAINER DATATABLE FOR TEMPORARY DATA  ////////////////////////////////////////
            DataTable dtContainer = new DataTable();
            dtContainer.Columns.AddRange(new DataColumn[9] {new DataColumn("PkId"), new DataColumn("JobId"), new DataColumn("ContainerNo"),
                                                            new DataColumn("ContainerSize"), new DataColumn("ContainerType"), new DataColumn("UserId"),
                                                            new DataColumn("ContainerSizeId"), new DataColumn("ContainerTypeId"),
                                                            new DataColumn("SealNo")});
            ViewState["Container"] = dtContainer;

            // ---  Freight Export

            //DBOperations.FillPreAlertExport(ddlPreAlertExport); 
            DBOperations.FillPreAlertExport(ddlPreAlertExport, Convert.ToInt32(LoggedInUser.glUserId));

            // ---  End Freight Export          
        }
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        int CustomerId = Convert.ToInt32(hdnCustId.Value);
        if (txtCustomer.Text.Trim() != "")
        {
            if (CustomerId > 0)
            {
                DBOperations.FillCustomerDivision(ddDivision, CustomerId);

                ddlShipper.DataSourceID = "DataSourceShipper";
                ddlShipper.DataBind();
                ddlShipper.Focus();
            }
            else
            {
                ListItem lstSelect = new ListItem("-Select-", "0");
                ddDivision.Items.Clear();
                ddDivision.Items.Add(lstSelect);
                ddlShipper.Items.Clear();
                txtCustomer.Focus();
            }

            //if (Convert.ToInt32(ViewState["PreCustId"].ToString()) != CustomerId)
            //{
            //    lblError.Text = "PreAlert AND Customer name not match";
            //    lblError.CssClass = "errorMsg";
            //    ddlPreAlertExport.SelectedValue = "0";
            //    lblFrJobNo.Text = "";
            //    return;
            //}
            //else
            //{
            //    lblError.Text = "";
            //}
        }
        else
        {
            ListItem lstSelect = new ListItem("-Select-", "0");
            ddDivision.Items.Clear();
            ddDivision.Items.Add(lstSelect);
            ddlShipper.Items.Clear();
            txtCustomer.Focus();
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);
        DBOperations.FillCustomerPlant(ddPlant, DivisonId);
        ddDivision.Focus();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bool IsValidJobNo = true;
        int JobId = 0;
        try
        {
            //if (hdnDischargePortId.Value == "" || hdnDischargePortId.Value == "0")
            //{
            //    lblError.Text = "Please Upload Atleast 1 Document.";
            //    lblError.CssClass = "errorMsg";
            //}

            if (txtJobNumber.Text == "")
            {
                IsValidJobNo = false;
            }

            if (!txtJobNumber.Text.Trim().ToUpper().StartsWith("CB"))
            {
                IsValidJobNo = false;
            }

            if (txtJobNumber.Text.Trim().Length != 20)
            {
                IsValidJobNo = false;
            }

            if (IsValidJobNo == true)
            {
                int CountFields = 0, DivisionId = 0, PlantId = 0, Priority = 0, BabajiBranchId = 0, DocumentId = 0, CustomerId = 0, ShipperId = 0, TransMode = 0, PortOfLoadingId = 0, PortOfDischargeId = 0, CountryConsignmentId = 0, CountryDestinationId = 0, NoOfPkg = 0,
                    PackageType = 0, ShippingBillType = 0, ContainerLoadedId = 0, FinYear = 0, TransportBy = 0, lUser = 0, IsBabajiForwarder = 0, ExportType = 0;
                double GrossWT = 0.0, NetWT = 0.0;
                string filePath = "", strFilePath = "", MAWBNo = "", HAWBNo = "", ForwarderName = "", PickupPersonName = "", PickupMobileNo = "", Dimension = "";
                DateTime dtHAWBDate = DateTime.MinValue, dtMAWBDate = DateTime.MinValue, dtPickUpDate = DateTime.MinValue;
                bool TransportationByBabaji = false;
                bool IsAdC = false, IsHazeCargo = false;

                // Freight Export
                int PreAlertId = 0;
                if (ddlPreAlertExport.SelectedValue != "")
                {
                    PreAlertId = Convert.ToInt32(ddlPreAlertExport.SelectedValue);
                }

                FinYear = Convert.ToInt32(Session["FinYearId"]);
                strFilePath = hdnDocPath.Value;
                DivisionId = Convert.ToInt32(ddDivision.SelectedValue);
                PlantId = Convert.ToInt32(ddPlant.SelectedValue);
                Priority = Convert.ToInt32(ddlPriority.SelectedValue);
                BabajiBranchId = Convert.ToInt32(ddlBabajiBranch.SelectedValue);
                lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());
                CustomerId = Convert.ToInt32(hdnCustId.Value);
                ShipperId = Convert.ToInt32(ddlShipper.SelectedValue);
                TransMode = Convert.ToInt32(ddlMode.SelectedValue);
                PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
                PortOfDischargeId = Convert.ToInt32(hdnDischargePortId.Value);
                CountryConsignmentId = Convert.ToInt32(hdnCountryId.Value);
                CountryDestinationId = Convert.ToInt32(hdnDestCountryId.Value);
                PackageType = Convert.ToInt32(ddlPackageType.SelectedValue);
                ShippingBillType = Convert.ToInt32(ddlShippingBillType.SelectedValue);
                MAWBNo = txtMblNo.Text.Trim();
                HAWBNo = txtHblNo.Text.Trim();

                if (rblADC.SelectedValue == "1")
                {
                    IsAdC = true;
                }
                if (rblHazeCargo.SelectedValue == "1")
                {
                    IsHazeCargo = true;
                }

                if (txtMAWBDate.Text != "")
                    dtMAWBDate = Commonfunctions.CDateTime(txtMAWBDate.Text.Trim());
                if (txtHAWBDate.Text != "")
                    dtHAWBDate = Commonfunctions.CDateTime(txtHAWBDate.Text.Trim());
                if (txtPickUpDate.Text != "")
                    dtPickUpDate = Commonfunctions.CDateTime(txtPickUpDate.Text.Trim());
                if (txtNoOfPackages.Text.Trim().ToString() != "")
                    NoOfPkg = Convert.ToInt32(txtNoOfPackages.Text.Trim());
                if (ddlContainerLoaded.SelectedValue != "0")
                    ContainerLoadedId = Convert.ToInt32(ddlContainerLoaded.SelectedValue);
                if (txtGrossWT.Text.Trim().ToString() != "")
                    GrossWT = Convert.ToDouble(txtGrossWT.Text.Trim());
                if (txtNetWT.Text.Trim().ToString() != "")
                    NetWT = Convert.ToDouble(txtNetWT.Text.Trim());

                ExportType = Convert.ToInt32(ddlExportType.SelectedValue);
                PickupPersonName = txtPickupPersonName.Text.Trim();
                PickupMobileNo = txtPickupMobileNo.Text.Trim();

                if (ddlTransportBy.SelectedValue != "0")
                {
                    if (ddlTransportBy.SelectedValue == "1")
                    {
                        TransportBy = 1;  // Babaji Shivram Transport
                        TransportationByBabaji = true;
                    }
                    else
                    {
                        TransportBy = 0;  // Customer Transport
                        TransportationByBabaji = false;
                    }
                }
                if (ddlForwardedBy.SelectedValue == "1") // Is Babaji forwarder
                    IsBabajiForwarder = 1;
                if (ddlForwardedBy.SelectedValue == "1")
                    ForwarderName = "";
                else
                    ForwarderName = txtForwardedName.Text.Trim();
                if (txtDimension.Text.Trim() != "")
                    Dimension = txtDimension.Text.Trim();

                bool IsOctroi = false; // chkApplicable.Items[0].Selected;
                bool IsSForm = false; //chkApplicable.Items[1].Selected;
                bool IsNForm = false; //chkApplicable.Items[2].Selected;
                bool IsRoadPermit = false; // chkApplicable.Items[3].Selected;

                int count_Doc = 0;
                // check atleast 1 document been uploaded
                foreach (RepeaterItem itm in rpDocument.Items)
                {
                    CheckBox chkDocType = (CheckBox)itm.FindControl("chkDocType");
                    FileUpload fuDocument = (FileUpload)itm.FindControl("fuDocument");
                    if (chkDocType.Checked)
                    {
                        count_Doc++;
                    }
                }

                if (ddlMode.SelectedValue != "0")
                {
                    if (ddlMode.SelectedValue == "2") // Sea
                    {
                        if (ViewState["Container"] != null)
                        {
                            DataTable dtcontainer = (DataTable)ViewState["Container"];
                            if (dtcontainer == null)
                            {
                                lblError.Text = "Please enter atleast one container detail!";
                                lblError.CssClass = "errorMsg";
                                return;
                            }
                        }
                        else
                        {
                            lblError.Text = "Please enter atleast one container detail!";
                            lblError.CssClass = "errorMsg";
                            return;
                        }
                    }
                }

                if (count_Doc != 0)
                {
                    int result = EXOperations.EX_AddExportNewJob(BabajiBranchId, txtJobNumber.Text.Trim(), CustomerId, ShipperId, txtConsignee.Text.Trim(), TransMode, txtBuyerName.Text.Trim(), txtProductDesc.Text.Trim(),
                    PortOfLoadingId, PortOfDischargeId, CountryConsignmentId, CountryDestinationId, NoOfPkg, PackageType, ShippingBillType, ForwarderName, txtCustRefNo.Text.Trim(),
                    ContainerLoadedId, GrossWT, NetWT, TransportBy, Priority, DivisionId, PlantId, lUser, txtPickupLocation.Text.Trim(), txtLocationTo.Text.Trim(), IsBabajiForwarder,
                    MAWBNo, HAWBNo, dtMAWBDate, dtHAWBDate, ExportType, Dimension, PickupPersonName, dtPickUpDate, PickupMobileNo, IsAdC, IsHazeCargo, txtJobRemark.Text.Trim(), PreAlertId, FinYear);

                    if (result > 0)
                    {
                        JobId = Convert.ToInt32(result);
                        string JobRefNo = EXOperations.EX_GetBabajiJobRefNo(txtJobNumber.Text.Trim());
                        /////////////////////////////  Update checklist status details  /////////////////////////////////////////////
                        EXOperations.AddChecklistStatusDetail(JobId, 1, 0, Convert.ToDateTime(DateTime.MinValue), Convert.ToString(""), lUser, Convert.ToString(""), Convert.ToDateTime(DateTime.Now), 1);

                        /////////////////////////////  Insert container details  ////////////////////////////////////////////////////
                        DataTable dtcontainer = (DataTable)ViewState["Container"];
                        if (dtcontainer != null && dtcontainer.Rows.Count > 0)
                        {
                            for (int c = 0; c < dtcontainer.Rows.Count; c++)
                            {
                                EXOperations.EX_AddContainerDetail(JobId, dtcontainer.Rows[c]["ContainerNo"].ToString(), Convert.ToInt32(dtcontainer.Rows[c]["ContainerSizeId"].ToString()),
                                             Convert.ToInt32(dtcontainer.Rows[c]["ContainerTypeId"].ToString()), Convert.ToInt32(dtcontainer.Rows[c]["UserId"].ToString()),
                                             dtcontainer.Rows[c]["SealNo"].ToString());
                            }
                        }

                        /////////////////////////////// Add Delivery Fields Applicable  ////////////////////////////////////////////////////////
                        int Result_ApplicableFields = EXOperations.EX_AddExamineDetail(JobId, IsOctroi, IsSForm, IsNForm, IsRoadPermit, lUser);

                        ////////////////////////////// Add Babaji Transport Request  ////////////////////////////////////////////////

                        //if (TransportBy == 1)
                        //{
                        //    int TransResult = DBOperations.AddJobTransportRequest(JobId, JobRefNo, Convert.ToDateTime(DateTime.Now), 3, CustomerId, txtPickupLocation.Text.Trim(), txtLocationTo.Text.Trim(), 0,
                        //        0, NoOfPkg, GrossWT.ToString(), "", LoggedInUser.glUserId);
                        //}

                        /////////////////////////////  Insert Documents  ///////////////////////////////////////////////////////////
                        DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
                        if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
                            strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

                        foreach (RepeaterItem itm in rpDocument.Items)
                        {
                            CheckBox chkDocType = (CheckBox)itm.FindControl("chkDocType");
                            FileUpload fuDocument = (FileUpload)itm.FindControl("fuDocument");
                            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
                            DocumentId = Convert.ToInt32(hdnDocId.Value);

                            if (chkDocType.Checked)
                            {
                                if (fuDocument.HasFile)
                                {
                                    filePath = UploadFiles(fuDocument, strFilePath);
                                    if (filePath != "")
                                    {
                                        int result_DocSaved = EXOperations.Ex_AddPreAlertDocs(filePath, DocumentId, JobId, lUser);
                                        if (result_DocSaved != 0)
                                        {
                                            lblError.Text = "Error while inserting record. Please try again later.";
                                            lblError.CssClass = "errorMsg";
                                        }
                                    }
                                }
                            }
                        }
                        lblError.Text = "Successfully Added Job - " + txtJobNumber.Text.Trim() + ".";
                        lblError.CssClass = "success";
                        ResetControls();

                    }
                    else if (result == -1)
                    {
                        lblError.Text = "Job Number Already Exists.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "Error while inserting record. Please try again later.";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError.Text = "Please Upload Atleast 1 Document.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Invalid Job Number. Please Enter Valid Job Number!";
                lblError.CssClass = "errorMsg";
            }

        }
        catch (Exception en)
        {
            lblError.Text = "System Error! Please Try After some Time.<BR>" + en.Message.ToString();
            lblError.CssClass = "errorMsg";

            ErrorLog.LogToDatabase(JobId, "Export New Job", "New Job Entry", en.Message, en, en.InnerException.Message, LoggedInUser.glUserId);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewJobEntry.aspx");
    }

    protected void ddlTransportBy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransportBy.SelectedValue != "0")
        {
            if (ddlTransportBy.SelectedValue == "1") //Babaji Shivram
            {
                trTpPickupLocation.Visible = true;
                trTpPickupPersonDetails.Visible = true;
                trTpPickupPersonDetails2.Visible = true;
                rfvpickupfrom.Enabled = true;
                rfvdropto.Enabled = true;
                rfvDimension.Enabled = true;
            }
            else //Customer
            {
                trTpPickupLocation.Visible = false;
                trTpPickupPersonDetails.Visible = false;
                trTpPickupPersonDetails2.Visible = false;
                rfvpickupfrom.Enabled = false;
                rfvdropto.Enabled = false;
                rfvDimension.Enabled = false;
            }
        }
        else
        {
            trTpPickupLocation.Visible = false;
            trTpPickupPersonDetails.Visible = false;
            trTpPickupPersonDetails2.Visible = false;
            rfvpickupfrom.Enabled = false;
            rfvdropto.Enabled = false;
            rfvDimension.Enabled = false;
        }
        ddlTransportBy.Focus();
    }

    protected void ddlBabajiBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMode.SelectedValue != "0" && ddlBabajiBranch.SelectedValue != "0")
        {
            if (ddlMode.SelectedValue == "1") //Air
            {
                if (ddlBabajiBranch.SelectedValue == "2") // Mumbai - Cargo 
                {
                    // tr_ApplicableFields.Visible = true;
                }
                else
                {
                    // tr_ApplicableFields.Visible = false;
                }
            }
            else
            {
                if (ddlBabajiBranch.SelectedValue == "3") // Mumbai - Saki Naka
                {
                    // tr_ApplicableFields.Visible = true;
                }
                else
                {
                    // tr_ApplicableFields.Visible = false;
                }
            }
        }
    }

    protected void ddlMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExportType.Enabled = true;
        ddlExportType.SelectedIndex = 0;

        if (ddlMode.SelectedValue != "0")
        {
            if (ddlMode.SelectedValue == "1") //Air
            {
                ddlContainerLoaded.Visible = false;
                lblddlContainer.Visible = false;
                rfvcontloaded.Enabled = false;
                trHBL.Visible = true;
                trMBL.Visible = true;
                btnAddContainerDetails.Visible = false;
                ddlExportType.SelectedValue = "2"; // Docs Stuff
                ddlExportType.Enabled = false;

                if (ddlBabajiBranch.SelectedValue != "0")
                {
                    if (ddlBabajiBranch.SelectedValue == "2") // Mumbai - Cargo 
                    {
                        // tr_ApplicableFields.Visible = true;
                    }
                    else
                    {
                        //  tr_ApplicableFields.Visible = false;
                    }
                }
            }
            else                                        //Sea
            {
                ddlContainerLoaded.Visible = true;
                lblddlContainer.Visible = true;
                rfvcontloaded.Enabled = true;
                trHBL.Visible = false;
                trMBL.Visible = false;
                btnAddContainerDetails.Visible = true;
                ddlExportType.SelectedIndex = 0;
                ddlExportType.Enabled = true;

                if (ddlBabajiBranch.SelectedValue != "0")
                {
                    if (ddlBabajiBranch.SelectedValue == "3") // Mumbai - Saki Naka
                    {
                        // tr_ApplicableFields.Visible = true;
                    }
                    else
                    {
                        // tr_ApplicableFields.Visible = false;
                    }
                }
            }
        }
        else
        {
            ddlContainerLoaded.Enabled = true;
            trHBL.Visible = false;
            trMBL.Visible = false;
            btnAddContainerDetails.Visible = false;
        }
        ddlMode.Focus();
    }

    protected void ddlExportType_OnSelectedIndexchanged(object sender, EventArgs e)
    {
        if (ddlExportType.SelectedValue != "0" && ddlExportType.SelectedValue == "2") //Docs Stuff
        {
            ddlContainerLoaded.SelectedValue = "1"; //FCL
            ddlContainerLoaded.Enabled = false;
        }
        else
        {
            ddlContainerLoaded.SelectedValue = "0";
            ddlContainerLoaded.Enabled = true;
        }
        ddlExportType.Focus();
    }

    protected void ddlForwardedBy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlForwardedBy.SelectedValue == "1") // Babaji Shivram Forwarder
        {
            rfvForwardedName.Enabled = false;
            txtForwardedName.Enabled = false;
            txtForwardedName.Text = "Babaji Shivram";
        }
        else
        {
            txtForwardedName.Text = "";
            rfvForwardedName.Enabled = true;
            txtForwardedName.Enabled = true;
        }
        ddlForwardedBy.Focus();
    }

    protected void ResetControls()
    {
        txtCustomer.Text = "";
        txtPickupLocation.Text = "";
        txtLocationTo.Text = "";
        txtPickUpDate.Text = "";
        txtPickupPersonName.Text = "";
        txtPickupMobileNo.Text = "";
        txtDimension.Text = "";
        txtMAWBDate.Text = "";
        txtMblNo.Text = "";
        txtHAWBDate.Text = "";
        txtHblNo.Text = "";
        txtBuyerName.Text = "";
        txtConsignee.Text = "";
        txtContainerNo.Text = "";
        txtCountry.Text = "";
        txtCustRefNo.Text = "";
        txtDestinationCountry.Text = "";
        txtForwardedName.Text = "";
        txtGrossWT.Text = "";
        txtNetWT.Text = "";
        txtNoOfPackages.Text = "";
        txtPortOfDischarge.Text = "";
        txtPortOfLoading.Text = "";
        txtProductDesc.Text = "";
        txtJobNumber.Text = "";
        ddlPriority.SelectedIndex = 0;
        //ddlPackageType.DataBind();
        hdnConsigneeId.Value = "";
        hdnCountryId.Value = "";
        hdnCustDocFolder.Value = "";
        hdnCustId.Value = "";
        hdnDestCountryId.Value = "";
        hdnDischargePortId.Value = "";
        hdnDocPath.Value = "";
        hdnJobFileDir.Value = "";
        hdnLoadingPortId.Value = "";
        hdnMode.Value = "";
        hdnShipperId.Value = "";
        txtSealNo.Text = "";
        ddlExportType.SelectedIndex = 0;
        ddlExportType.Enabled = true;
        ViewState["Container"] = null;

        //ddlBabajiBranch.Focus();
        //ddlBabajiBranch.DataBind();
        //ddDivision_SelectedIndexChanged(null, EventArgs.Empty);
        ddlMode.SelectedIndex = 0;
        ddlTransportBy.SelectedIndex = 0;
        ddlForwardedBy.SelectedIndex = 0;
        //ddlTransportBy_OnSelectedIndexChanged(null, EventArgs.Empty);
        //ddlMode_OnSelectedIndexChanged(null, EventArgs.Empty);
        //ddlForwardedBy_OnSelectedIndexChanged(null, EventArgs.Empty);
    }

    #region CONTAINER DETAILS EVENTS

    protected void btnAddContainerDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtender2.Show();
            BindGrid();
            lblError_Popup.Text = "";
        }
        catch (Exception en)
        {

        }
    }

    protected void btnCancelPopup_Click1(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
        lblError_Popup.Text = "";
    }

    protected void BindGrid()
    {
        DataTable dtContainerLists = (DataTable)ViewState["Container"];
        gvContainer.DataSource = dtContainerLists;
        gvContainer.DataBind();
    }

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        int PkId = 1, count = 0;
        lblError_Popup.Visible = true;
        int JobId = Convert.ToInt32(Session["JobId"]);
        string ContainerSize = "", ContainerType = "", SealNo = "";
        int ContainerSizeId = 0, ContainerTypeId = 0;

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSizeId = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerTypeId = Convert.ToInt32(ddContainerType.SelectedValue);

        if (ContainerTypeId == 1) //FCL
        {
            if (ContainerSizeId == 0)
            {
                lblError_Popup.Text = "Please Select FCL Container Size!";
                lblError_Popup.CssClass = "errorMsg";
                ModalPopupExtender2.Show();
                return;
            }
        }
        else if (ContainerTypeId == 2) //LCL
        {
            if (ddlExportType.SelectedValue == "1") //Factory Stuff / LCL Not Allowed
            {
                lblError_Popup.Text = "LCL - Invalid Type Selection For Factory Stuff!";
                lblError_Popup.CssClass = "errorMsg";
                ModalPopupExtender2.Show();
                return;
            }

            ddContainerSize.SelectedValue = "0";
            ContainerSizeId = 0;
        }

        if (ContainerNo != "")
        {
            int OriginalRows = 0, AfterInsertedRows = 0;
            if (ddContainerSize.SelectedValue == "0")
                ContainerSize = "";
            else
                ContainerSize = ddContainerSize.SelectedItem.Text.Trim();
            ContainerType = ddContainerType.SelectedItem.Text.Trim();
            SealNo = txtSealNo.Text.Trim();
            DataTable dtContainer = (DataTable)ViewState["Container"];
            if (dtContainer != null && dtContainer.Rows.Count > 0)
                PkId++;
            if (dtContainer != null)
                OriginalRows = dtContainer.Rows.Count;              //get original rows of grid view.
            if (dtContainer != null)
            {
                foreach (DataRow dr in dtContainer.Rows)
                {
                    if (dr["ContainerNo"].ToString() == ContainerNo)
                    {
                        lblError_Popup.Text = "Container No " + ContainerNo + " Already Added!";
                        lblError_Popup.CssClass = "warning";
                        count = 1;
                        ModalPopupExtender2.Show();
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            if (count == 0)
            {
                dtContainer.Rows.Add(PkId, JobId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId, ContainerSizeId, ContainerTypeId, SealNo);
                //insert row into datatable.
                AfterInsertedRows = dtContainer.Rows.Count;     //get present rows after deleting particular row from grid view.
                ViewState["Container"] = dtContainer;
                BindGrid();
                if (OriginalRows < AfterInsertedRows)
                {
                    lblError_Popup.Text = "Container No " + ContainerNo + " Added successfully!";
                    lblError_Popup.CssClass = "success";
                    txtContainerNo.Text = "";
                    ddContainerSize.SelectedValue = "0";
                    ddContainerType.SelectedValue = "1";
                    txtSealNo.Text = "";
                    ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
                }
                else
                {
                    lblError_Popup.Text = "System Error! Please Try After Sometime.";
                    lblError_Popup.CssClass = "errorMsg";
                    ModalPopupExtender2.Show();
                }
            }
        }
        else
        {
            lblError_Popup.CssClass = "errorMsg";
            lblError_Popup.Text = " Please Enter Container No.!";
            ModalPopupExtender2.Show();
        }
    }

    protected void ddContainerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddContainerType.SelectedValue == "2")
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddContainerSize.Items.Clear();
            ddContainerSize.Items.Add(lstSelect);
        }
        else
        {
            ddContainerSize.Items.Clear();
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            System.Web.UI.WebControls.ListItem lstSelect20 = new System.Web.UI.WebControls.ListItem("20", "1");
            System.Web.UI.WebControls.ListItem lstSelect40 = new System.Web.UI.WebControls.ListItem("40", "2");
            System.Web.UI.WebControls.ListItem lstSelect45 = new System.Web.UI.WebControls.ListItem("45", "3");
            ddContainerSize.Items.Add(lstSelect);
            ddContainerSize.Items.Add(lstSelect20);
            ddContainerSize.Items.Add(lstSelect40);
            ddContainerSize.Items.Add(lstSelect45);
        }
        ModalPopupExtender2.Show();
    }

    protected void OnContainerDelete(object sender, EventArgs e)
    {
        int OriginalRows = 0, AfterDeletedRows = 0;

        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["Container"] as DataTable;
        OriginalRows = dt.Rows.Count;       // get original rows of grid view
        dt.Rows.RemoveAt(row.RowIndex);     // delete row from datatable using specific rowindex id
        AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
        ViewState["Container"] = dt;
        BindGrid();
        if (OriginalRows > AfterDeletedRows)
        {
            lblError_Popup.Text = "Successfully Deleted Container Details.";
            lblError_Popup.CssClass = "success";
            gvContainer.DataBind();
            ModalPopupExtender2.Show();
        }
        else
        {
            lblError_Popup.Text = "Error while deleting container details. Please try again later..!!";
            lblError_Popup.CssClass = "success";
            ModalPopupExtender2.Show();
        }
    }

    protected void btnCancelContainer_Click(object sender, EventArgs e)
    {
        txtContainerNo.Text = "";
        ddContainerSize.SelectedValue = "0";
        ddContainerType.SelectedValue = "1";
        BindGrid();
        gvContainer.DataBind();
        ModalPopupExtender2.Hide();
    }

    #endregion

    #region Document Upload / Download / Delete

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");
        FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");
        RequiredFieldValidator RFVDocument = (RequiredFieldValidator)e.Item.FindControl("RFVDocument");

        if (chkDocumentType != null && fileUploadDocument != null && RFVDocument != null)
        {
            chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + RFVDocument.ClientID + "');");
        }
    }

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

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
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }
        else
        {
            return "";
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

    #region  Freight Export

    public void ClearData()
    {
        ddlBabajiBranch.SelectedValue = "0";
        ddPlant.SelectedValue = "0";
        ddlMode.SelectedValue = "0";
        ddlTransportBy.SelectedValue = "0";
        //ddlTransportBy_OnSelectedIndexChanged(sender, e);
        txtPortOfLoading.Text = "";
        hdnLoadingPortId.Value = "";
        txtPortOfDischarge.Text = "";
        hdnDischargePortId.Value = "";
        ddlExportType.SelectedValue = "0";
        //ddlExportType_OnSelectedIndexchanged(sender, e);
        txtConsignee.Text = "";
    }

    protected void ddlPreAlertExport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPreAlertExport.SelectedValue != "0")
        {
            string CustId = "0";

            string enqid = ddlPreAlertExport.SelectedValue;
            if (enqid != "")
            {
                DataView dvCustomerRequest = DBOperations.GetFreightExportJobNo(Convert.ToInt32(enqid));
                string strFrJobNo = Convert.ToString(dvCustomerRequest.Table.Rows[0]["FRJobNo"]);
                CustId = Convert.ToString(dvCustomerRequest.Table.Rows[0]["CustomerId"]);

                ViewState["PreCustId"] = CustId;


                lblFrJobNoTitle.Visible = true;
                lblFrJobNo.Visible = true;
                lblFrJobNo.Text = strFrJobNo;

                if (strFrJobNo == "")
                {
                    lblError.Text = "Freight Job No Not Found";
                    lblError.CssClass = "errorMsg";
                }

                if (hdnCustId.Value != CustId)
                {
                    //Response.Redirect("NewJobEntry.aspx.cs");
                    lblError.Text = "PreAlert AND Customer name not match";
                    lblError.CssClass = "errorMsg";
                    ddlPreAlertExport.SelectedValue = "0";
                    lblFrJobNo.Text = "";
                    ClearData();
                    return;
                }
                else
                {
                    lblError.Text = "";
                }


                DataSet dsFrExportDetails = DBOperations.GetExpBookingDetail(Convert.ToInt32(enqid));

                int brachId = 0;
                string strBranchId = "", strMode = "", strExportType = "", strTransportBy = "", strDivision = "",
                    PortOfLoadingId = "", PortOfDischargeId = "", PortOfLoading = "", PortOfDischarge = "", strPlant = "",
                    NoOfPackages = "", strPackageType = "0", strGrossWt = "", strChargeableWt = "", strDescription = "",
                    LoadingCountryName = "", LoadingCountryId = "", DestinationCountryName = "", DestinationCountryId = "";

                if (dsFrExportDetails.Tables[0].Rows.Count > 0)
                {
                    brachId = Convert.ToInt32(dsFrExportDetails.Tables[0].Rows[0]["BranchId"]);
                    strBranchId = dsFrExportDetails.Tables[0].Rows[0]["BranchId"].ToString();
                    //ddlBabajiBranch.SelectedValue = strBranchId;

                    strMode = dsFrExportDetails.Tables[0].Rows[0]["lMode"].ToString();
                    ddlMode.SelectedValue = strMode;

                    if (ddlMode.SelectedValue == "1")
                    {
                        trHBL.Visible = true;
                        trMBL.Visible = true;
                    }
                    else
                    {
                        trHBL.Visible = false;
                        trMBL.Visible = false;
                    }
                    txtConsignee.Text = dsFrExportDetails.Tables[0].Rows[0]["Consignee"].ToString();
                    txtCustRefNo.Text = dsFrExportDetails.Tables[0].Rows[0]["CustRefNo"].ToString();
                    PortOfLoadingId = dsFrExportDetails.Tables[0].Rows[0]["PortOfLoadingId"].ToString();
                    PortOfDischargeId = dsFrExportDetails.Tables[0].Rows[0]["PortOfDischargeId"].ToString();
                    strExportType = dsFrExportDetails.Tables[0].Rows[0]["ExportType"].ToString();
                    strTransportBy = dsFrExportDetails.Tables[0].Rows[0]["TransportBy"].ToString();

                    strDivision = dsFrExportDetails.Tables[0].Rows[0]["Division"].ToString();
                    strPlant = dsFrExportDetails.Tables[0].Rows[0]["Plant"].ToString();

                    NoOfPackages = dsFrExportDetails.Tables[0].Rows[0]["NoOfPackages"].ToString();
                    txtNoOfPackages.Text = NoOfPackages;
                    strPackageType = dsFrExportDetails.Tables[0].Rows[0]["PackageTypeId"].ToString();
                    ddlPackageType.SelectedValue = strPackageType;
                    strGrossWt = dsFrExportDetails.Tables[0].Rows[0]["GrossWeight"].ToString();
                    txtGrossWT.Text = strGrossWt;
                    strChargeableWt = dsFrExportDetails.Tables[0].Rows[0]["ChargeableWeight"].ToString();
                    //txtNetWT.Text = strChargeableWt;
                    strDescription = dsFrExportDetails.Tables[0].Rows[0]["CargoDescription"].ToString();
                    txtProductDesc.Text = strDescription;

                    DestinationCountryName = dsFrExportDetails.Tables[0].Rows[0]["LoadingCountryName"].ToString();
                    txtDestinationCountry.Text = DestinationCountryName;
                    DestinationCountryId = dsFrExportDetails.Tables[0].Rows[0]["LoadingCountry"].ToString();
                    if (DestinationCountryId != "")
                    {
                        hdnDestCountryId.Value = DestinationCountryId;
                    }
                    else
                    {
                        hdnDestCountryId.Value = "0";
                    }

                    txtCountry.Text = "India";
                    hdnCountryId.Value = "10";

                    if (strExportType != "0")
                    {
                        ddlExportType.SelectedValue = strExportType;
                        ddlExportType_OnSelectedIndexchanged(sender, e);
                    }

                    ddlTransportBy.SelectedValue = strTransportBy;
                    ddlTransportBy_OnSelectedIndexChanged(sender, e);

                    ddDivision.SelectedValue = strDivision;
                    ddDivision_SelectedIndexChanged(sender, e);
                    ddPlant.SelectedValue = strPlant;

                    hdnLoadingPortId.Value = PortOfLoadingId;
                    hdnDischargePortId.Value = PortOfDischargeId;

                    txtPortOfLoading.Text = dsFrExportDetails.Tables[0].Rows[0]["LoadingPortName"].ToString();

                    txtPortOfDischarge.Text = dsFrExportDetails.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                }
            }
            else
            {
                lblError.Text = "Enquiry Id Not Exist";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblFrJobNoTitle.Visible = false;
            lblFrJobNo.Visible = false;
        }
    }
    #endregion

}

//#region CONTAINER DETAILS EVENTS

//protected void btnAddContainerDetails_OnClick(object sender, EventArgs e)
//{
//    try
//    {
//        ModalPopupExtender2.Show();
//        gvContainer.DataBind();
//        lblError_Popup.Text = "";
//    }
//    catch (Exception en)
//    {

//    }
//}

//protected void btnCancelPopup_Click1(object sender, EventArgs e)
//{
//    ModalPopupExtender2.Hide();
//    lblError_Popup.Text = "";
//}

//protected void btnAddContainer_Click(object sender, EventArgs e)
//{
//    lblError_Popup.Visible = true;
//    int JobId = 0;
//    if (hdnNewJobId.Value != "")
//        JobId = Convert.ToInt32(hdnNewJobId.Value);
//    int ContainerSize, ContainerType;

//    string ContainerNo = txtContainerNo.Text.Trim();
//    ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
//    ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

//    if (ContainerType == 1) //FCL
//    {
//        if (ContainerSize == 0)
//        {
//            lblError_Popup.Text = "Please Select FCL Container Size!";
//            lblError_Popup.CssClass = "errorMsg";
//            return;
//        }
//    }
//    else if (ContainerType == 2) //LCL
//    {
//        ddContainerSize.SelectedValue = "0";
//        ContainerSize = 0;
//    }

//    if (ContainerNo != "")
//    {
//        if (JobId != 0)
//        {
//            int result = EXOperations.EX_AddContainerDetail(JobId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

//            if (result == 0)
//            {
//                lblError_Popup.Text = "Container No " + ContainerNo + " Added successfully!";
//                lblError_Popup.CssClass = "success";
//                gvContainer.DataBind();
//                txtContainerNo.Text = "";
//                ddContainerSize.SelectedValue = "0";
//                ddContainerType.SelectedValue = "1";
//                ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
//                ModalPopupExtender2.Show();
//            }
//            else if (result == 1)
//            {
//                lblError_Popup.Text = "System Error! Please Try After Sometime.";
//                lblError_Popup.CssClass = "errorMsg";
//                ModalPopupExtender2.Show();
//            }
//            else if (result == 2)
//            {
//                lblError_Popup.Text = "Container No " + ContainerNo + " Already Added!";
//                lblError_Popup.CssClass = "warning";
//                ModalPopupExtender2.Show();
//            }
//        }
//    }
//    else
//    {
//        lblError_Popup.CssClass = "errorMsg";
//        lblError_Popup.Text = " Please Enter Container No.!";
//        ModalPopupExtender2.Show();
//    }
//}

//protected void btnCancelContainer_Click(object sender, EventArgs e)
//{
//    txtContainerNo.Text = "";
//    ddContainerType.SelectedValue = "1";
//    ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
//    gvContainer.DataBind();
//    ModalPopupExtender2.Show();
//}

//protected void ddContainerType_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (ddContainerType.SelectedValue == "2")
//    {
//        System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
//        ddContainerSize.Items.Clear();
//        ddContainerSize.Items.Add(lstSelect);

//    }
//    else
//    {
//        ddContainerSize.Items.Clear();
//        System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
//        System.Web.UI.WebControls.ListItem lstSelect20 = new System.Web.UI.WebControls.ListItem("20", "1");
//        System.Web.UI.WebControls.ListItem lstSelect40 = new System.Web.UI.WebControls.ListItem("40", "2");
//        System.Web.UI.WebControls.ListItem lstSelect45 = new System.Web.UI.WebControls.ListItem("45", "3");
//        ddContainerSize.Items.Add(lstSelect);
//        ddContainerSize.Items.Add(lstSelect20);
//        ddContainerSize.Items.Add(lstSelect40);
//        ddContainerSize.Items.Add(lstSelect45);
//    }
//    ModalPopupExtender2.Show();
//}

//protected void ddEditContainerType_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (ddContainerType.SelectedValue == "2")
//    {
//        System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
//        ddContainerSize.Items.Clear();
//        ddContainerSize.Items.Add(lstSelect);

//    }
//    else
//    {
//        ddContainerSize.Items.Clear();
//        System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
//        System.Web.UI.WebControls.ListItem lstSelect20 = new System.Web.UI.WebControls.ListItem("20", "1");
//        System.Web.UI.WebControls.ListItem lstSelect40 = new System.Web.UI.WebControls.ListItem("40", "2");
//        System.Web.UI.WebControls.ListItem lstSelect45 = new System.Web.UI.WebControls.ListItem("45", "3");
//        ddContainerSize.Items.Add(lstSelect);
//        ddContainerSize.Items.Add(lstSelect20);
//        ddContainerSize.Items.Add(lstSelect40);
//        ddContainerSize.Items.Add(lstSelect45);
//    }
//    ModalPopupExtender2.Show();
//}

//protected void gvContainer_RowDeleting(object sender, GridViewDeleteEventArgs e)
//{
//    lblError_Popup.Visible = true;

//    int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Values[1].ToString());
//    int result = DBOperations.EX_DeleteContainerDetail(lid, LoggedInUser.glUserId);
//    if (result == 0)
//    {
//        lblError_Popup.Text = "Container Deleted Successfully!";
//        lblError_Popup.CssClass = "success";
//        e.Cancel = true;
//        gvContainer.DataBind();
//        ModalPopupExtender2.Show();
//    }
//    else if (result == 2)
//    {
//        lblError_Popup.Text = "Delivery Completed. Can not delete container details.";
//        lblError_Popup.CssClass = "errorMsg";
//        e.Cancel = true;
//        gvContainer.DataBind();
//        ModalPopupExtender2.Show();
//    }
//    else
//    {
//        lblError_Popup.Text = "System Error! Please Try After Sometime.";
//        lblError_Popup.CssClass = "errorMsg";
//        ModalPopupExtender2.Show();
//    }
//}

//#endregion