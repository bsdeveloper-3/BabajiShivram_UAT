using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using System.Collections;
using Ionic.Zip;

public partial class ExportCHA_EXJobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    public static int a = 0;
    static string JobRefNO = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnUpload);
        ScriptManager1.RegisterPostBackControl(btnPCDUpload);
        ScriptManager1.RegisterPostBackControl(GridViewDocument); //btnPCDUpload
                                                                  // ScriptManager1.RegisterPostBackControl(gvInvoiceProduct);
                                                                  //ScriptManager1.RegisterPostBackControl(GridViewBackOfficeDoc);
        ScriptManager1.RegisterPostBackControl(GridViewPCADoc);
        ScriptManager1.RegisterPostBackControl(GridViewBillingAdvice);
        ScriptManager1.RegisterPostBackControl(fvPCD);
        ScriptManager1.RegisterPostBackControl(GridViewBillingDept);
        ScriptManager1.RegisterPostBackControl(FVJobHistory);//lnkDownload17   
        ScriptManager1.RegisterPostBackControl(btnActivityPrint);
        ScriptManager1.RegisterPostBackControl(FVSBPrepare);
        ScriptManager1.RegisterPostBackControl(FVShipmentGetIN);
        ScriptManager1.RegisterPostBackControl(GridViewWarehouse);
        ScriptManager1.RegisterPostBackControl(btnPrintJobExpense);
        //ScriptManager1.RegisterPostBackControl(lnkInstructionCopy1);
        //ScriptManager1.RegisterPostBackControl(lnkInstructionCopy);
        //ScriptManager1.RegisterPostBackControl(lnkInstructionCopy2);
        //ScriptManager1.RegisterPostBackControl(lnkInstructionCopy3);
        ScriptManager1.RegisterPostBackControl(gvBillDispatchDocDetail);

        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("JobTracking.aspx");
        }

        if (!IsPostBack)
        {
            if (Session["JobId"] != null)
            {
                JobDetailMS(Convert.ToInt32(Session["JobId"]));
                Get_BillingInstruction(Convert.ToInt32(Session["JobId"]));
                EXOperations.EX_FillChekListDocDetail(ddDocument);
                EXOperations.EX_FillPCDDocument(ddPCDDocument);

                if (Session["Status"] != null && Convert.ToString(Session["Status"]) != "")
                {
                    if (Session["Status"].ToString() != "" && Session["Status"].ToString() == "7")
                        ddActivityStatus.SelectedValue = "0";
                    else
                        ddActivityStatus.SelectedValue = Session["Status"].ToString();

                    if (Session["ActivityStatus"].ToString() != null)
                    {
                        if (Session["ActivityStatus"].ToString() == "7")
                        {
                            ddActivityStatus.SelectedValue = Session["ActivityStatus"].ToString();
                        }
                    }

                    if (Session["Status"].ToString().Trim() == "7")
                    {
                        fieldJobActivity.Visible = false;
                        ddActivityStatus.Enabled = false;
                    }
                    else
                    {
                        fieldJobActivity.Visible = true;
                        ddActivityStatus.Enabled = false;
                    }
                }

                if (gvDailyJob.Rows.Count > 0)
                    btnActivityPrint.Visible = true;
                else
                    btnActivityPrint.Visible = false;

                if (Request.QueryString["ActiveTab"] != null)
                {
                    if (Request.QueryString["ActiveTab"].ToLower() == "activity")
                    {
                        Tabs.ActiveTab = TabPanelDailyActivity;
                    }
                    else if (Request.QueryString["ActiveTab"].ToLower() == "billingdetail")
                    {
                        Tabs.ActiveTab = TabBiiling;
                    }
                }
            }
            else
            {
                Response.Redirect("JobTracking.aspx");
            }
        }

        DataTable dtJobCancel = new DataTable();
        dtJobCancel = DBOperations.GetEXJobCancelDetail(Convert.ToInt32(Session["JobId"]), LoggedInUser.glFinYearId);
        if (dtJobCancel.Rows.Count > 0)
        {
            lblError.Text = "Job has Cancelled!!!";
            lblError.CssClass = "errorMsg";
        }
    }

    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("SBPreparation.aspx");
            Session["JobId"] = null;
        }

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            // Allow Vendor Fund Request
            if (LoggedInUser.glUserId == 81 || LoggedInUser.glUserId == 1) // Billing Dept HOD
            {
                if (Convert.ToBoolean(dsJobDetail.Tables[0].Rows[0]["IsFundAllowed"]) == true)
                {
                    rblFundRequest.SelectedValue = "1";
                }
                else
                {
                    rblFundRequest.SelectedValue = "0";
                }

                fldFundRequest.Visible = true;
                btnAllowFundRequest.Enabled = true;
            }

            lblTitle.Text = "Shipment Tracking - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            ViewState["JobNum"] = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();

            hdnJobRefNo.Value = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            JobRefNO = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            //DataSet dsPerticularJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
            hdnCustId.Value = dsJobDetail.Tables[0].Rows[0]["CustomerId"].ToString();
            txtDeliveryDestination.Text = dsJobDetail.Tables[0].Rows[0]["Destination"].ToString();
            rdlTransport.SelectedValue = dsJobDetail.Tables[0].Rows[0]["TransportById"].ToString();

            // enable edit button if stage is no cleared till filing 
            if (dsJobDetail.Tables[0].Rows[0]["FilingStatus"] != DBNull.Value)
            {
                int FilingStatus = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["FilingStatus"]);
                if (FilingStatus == 1) // filing stage cleared
                {
                    Button btnEditJobDetail = (Button)FVJobDetail.FindControl("btnEditJobDetail");
                    Button btnEditPrepareDetail = (Button)FVSBPrepare.FindControl("btnEditPrepareDetail");
                    if (btnEditJobDetail != null)
                    {
                        btnEditJobDetail.Visible = false;
                    }

                    //if (btnEditPrepareDetail != null)
                    //{
                    //    btnEditPrepareDetail.Visible = false;
                    //}
                }

                string dtLEODate = "";
                DataView dv = (DataView)DataSourceJobDetail.Select(DataSourceSelectArguments.Empty);
                DataTable dtCustomProcess = new DataTable();
                dtCustomProcess = dv.ToTable();
                if (dtCustomProcess.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCustomProcess.Rows)
                    {
                        dtLEODate = row["LEODate"].ToString();
                    }
                }

                if (dtLEODate == "")
                {
                    Button btnEditJobDetail = (Button)FVJobDetail.FindControl("btnEditJobDetail");
                    if (btnEditJobDetail != null)
                    {
                        btnEditJobDetail.Visible = true;
                    }
                }
            }

            if (dsJobDetail.Tables[0].Rows.Count > 0)
            {
                if (dsJobDetail.Tables[0].Rows[0]["Form13Required"] != DBNull.Value &&
                    dsJobDetail.Tables[0].Rows[0]["Form13Required"].ToString().Trim().ToLower() == "yes")
                {
                    // TAB : FORM 13 TAB 
                    accForm13.Visible = true;
                }
                else
                {
                    accForm13.Visible = false; // TAB : FORM 13 TAB 
                }

                if (dsJobDetail.Tables[0].Rows[0]["ActivityStatus"] != DBNull.Value && dsJobDetail.Tables[0].Rows[0]["ActivityStatus"].ToString() != "0")
                {
                    if (dsJobDetail.Tables[0].Rows[0]["ActivityStatus"].ToString() == "7")
                        fieldJobActivity.Visible = false;
                    else
                    {
                        ddActivityStatus.SelectedValue = dsJobDetail.Tables[0].Rows[0]["ActivityStatus"].ToString();
                    }

                    Session["ActivityStatus"] = dsJobDetail.Tables[0].Rows[0]["ActivityStatus"].ToString();
                }
            }
        }

        DataSet dsPCDDetail = EXOperations.EXGetPCDDetail(JobId);

        if (dsPCDDetail.Tables[0].Rows.Count > 0)
        {
            fvPCD.DataSource = dsPCDDetail;
            fvPCD.DataBind();

            int BillingDeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["BillingDeliveryId"]);
            int PCADeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["PCADeliveryId"]);

            Panel pnlDispatchBillingHand = (Panel)fvPCD.FindControl("pnlDispatchBillingHand");
            Panel pnlDispatchBillingCour = (Panel)fvPCD.FindControl("pnlDispatchBillingCour");

            Panel pnlDispatchPCAHand = (Panel)fvPCD.FindControl("pnlDispatchPCAHand");
            Panel pnlDispatchPCACour = (Panel)fvPCD.FindControl("pnlDispatchPCACour");


            if (BillingDeliveryId == 1)
                pnlDispatchBillingHand.Visible = true;
            else if (BillingDeliveryId == 2)
                pnlDispatchBillingCour.Visible = true;

            if (PCADeliveryId == 1)
                pnlDispatchPCAHand.Visible = true;
            else if (PCADeliveryId == 2)
                pnlDispatchPCACour.Visible = true;
        }

        // TAB : HISTORY
        FVJobHistory.DataSource = dsJobDetail;
        FVJobHistory.DataBind();
    }

    private void GetJobDetail(int JobId)
    {
        DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();
        }
    }

    public string GetBooleanToYesNo(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "YES";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "NO";
        }

        return strReturnText;
    }

    public bool CheckNullBooleanToTrueFalse(object mybValue)
    {
        bool bReturnValue = false;

        if (mybValue != DBNull.Value)
        {
            bReturnValue = Convert.ToBoolean(mybValue);
        }

        return bReturnValue;
    }

    public string GetBooleanToCompletedPending(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Completed";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Pending";
        }

        return strReturnText;
    }

    public string GetBooleanToApprovedRejected(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Approved";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Rejected";
        }

        return strReturnText;
    }

    protected void btnEditButton_Click(object sender, EventArgs e)
    {
        FVJobDetail.ChangeMode(FormViewMode.Edit);

        if (Session["JobId"] != null)
        {
            GetJobDetail(Convert.ToInt32(Session["JobId"]));
        }
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        string strReutrnUrl = ((Button)sender).CommandArgument.ToString();
        Response.Redirect(strReutrnUrl);
    }

    #region FORM VIEW EVENTS
    protected void FVJobHistory_DataBound(object sender, EventArgs e)
    {
        if (FVJobHistory.CurrentMode == FormViewMode.ReadOnly)
        {
            // Checklist Download

            LinkButton lnkChecklistDoc = (LinkButton)FVJobHistory.FindControl("lnkChecklistDoc");

            HiddenField hdnChecklistDocPath = (HiddenField)FVJobHistory.FindControl("hdnChecklistDocPath");
            ScriptManager1.RegisterPostBackControl(lnkChecklistDoc);

            if (hdnChecklistDocPath.Value.Trim() != "")
            {
                lnkChecklistDoc.Text = "Download";
                lnkChecklistDoc.Enabled = true;
            }
        }
    }

    #endregion

    #region JOB DETAIL FORM VIEW EVENTS
    protected void ddlMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMode = (DropDownList)FVJobDetail.FindControl("ddlMode");
        DropDownList ddlContainerLoaded = (DropDownList)FVJobDetail.FindControl("ddlContainerLoaded");
        if (ddlMode.SelectedValue != "0")
        {
            if (ddlMode.SelectedValue == "1") //Air
            {
                ddlContainerLoaded.Enabled = false;
                //trHBL.Visible = true;
                //trMBL.Visible = true;
            }
            else                                        //Sea
            {
                ddlContainerLoaded.Enabled = true;
                //trHBL.Visible = false;
                //trMBL.Visible = false;
            }
        }
        else
        {
            ddlContainerLoaded.Enabled = true;
            //trHBL.Visible = false;
            //trMBL.Visible = false;
        }
        ddlMode.Focus();
    }

    protected void ddlShippingBillType_OnSelectedIndexchanged(object sender, EventArgs e)
    {
        DropDownList ddlShippingBillType = (DropDownList)FVJobDetail.FindControl("ddlShippingBillType");
        DropDownList ddlContainerLoaded = (DropDownList)FVJobDetail.FindControl("ddlContainerLoaded");
        if (ddlShippingBillType.SelectedValue != "0" && ddlShippingBillType.SelectedValue == "2") //Docs Stuff
        {
            ddlContainerLoaded.SelectedValue = "1"; //FCL
            ddlContainerLoaded.Enabled = false;
        }
        else
        {
            ddlContainerLoaded.SelectedValue = "0";
            ddlContainerLoaded.Enabled = true;
        }
        ddlShippingBillType.Focus();
    }

    protected void ddlForwardedBy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlForwardedBy = (DropDownList)FVJobDetail.FindControl("ddlForwardedBy");
        RequiredFieldValidator rfvForwardedName = (RequiredFieldValidator)FVJobDetail.FindControl("rfvForwardedName");
        TextBox txtForwardedName = (TextBox)FVJobDetail.FindControl("txtForwardedName");
        if (ddlForwardedBy.SelectedValue == "1") // Babaji Shivram Forwarder
        {
            rfvForwardedName.Enabled = false;
            txtForwardedName.Enabled = false;
        }
        else
        {
            rfvForwardedName.Enabled = true;
            txtForwardedName.Enabled = true;
        }
        ddlForwardedBy.Focus();
    }

    //protected void btnUpdateJobDetail_OnClick(object sender, EventArgs e)
    //{
    //    int DivisionId = 0, PlantId = 0, Priority = 0, BabajiBranchId = 0, JobId = 0, CustomerId = 0, ShipperId = 0, TransMode = 0, PortOfLoadingId = 0,
    //        PortOfDischargeId = 0, CountryConsignmentId = 0, CountryDestinationId = 0, NoOfPkg = 0, PackageType = 0, ShippingBillType = 0, ContainerLoadedId = 0,
    //        TransportBy = 0, lUser = 0, IsBabajiForwarder = 0, ExportType = 0;
    //    double GrossWT = 0.0, NetWT = 0.0;
    //    string strFilePath = "", MAWBNo = "", HAWBNo = "", ForwarderName = "", CustRefNo = "", ConsigneeName = "", PickUpFrom = "", Destination = "",
    //        ProductDesc = "", BuyerName = "", PickUpPersonName = "", PickUpMobileNo = "", Dimension = "";
    //    DateTime dtHAWBDate = DateTime.MinValue, dtMAWBDate = DateTime.MinValue, dtPickUpDate = DateTime.MinValue;
    //    bool IsOctroi = false, IsSForm = false, IsNForm = false, IsRoadPermit = false;

    //    JobId = Convert.ToInt32(Session["JobId"]);
    //    if (JobId > 0)
    //    {
    //        strFilePath = hdnDocPath.Value;
    //        BabajiBranchId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlBabajiBranch")).SelectedValue);
    //        CustRefNo = ((TextBox)FVJobDetail.FindControl("txtCustRefNo")).Text.Trim();
    //        CustomerId = Convert.ToInt32(hdnCustId.Value);
    //        ShipperId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlShipper")).SelectedValue);
    //        DivisionId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddDivision")).SelectedValue);
    //        PlantId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddPlant")).SelectedValue);
    //        ConsigneeName = ((TextBox)FVJobDetail.FindControl("txtConsignee")).Text.Trim();
    //        TransMode = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlMode")).SelectedValue);
    //        PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
    //        PortOfDischargeId = Convert.ToInt32(hdnDischargePortId.Value);
    //        CountryConsignmentId = Convert.ToInt32(hdnCountryId.Value);
    //        CountryDestinationId = Convert.ToInt32(hdnDestCountryId.Value);
    //        ExportType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlExportType")).SelectedValue);
    //        ShippingBillType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlShippingBillType")).SelectedValue);
    //        Priority = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPriority")).SelectedValue);
    //        if ((((DropDownList)FVJobDetail.FindControl("ddlTransportBy")).SelectedValue) != "0")
    //        {
    //            if ((((DropDownList)FVJobDetail.FindControl("ddlTransportBy")).SelectedValue) == "1")
    //                TransportBy = 1;  // Babaji Shivram Transport
    //            else
    //                TransportBy = 0;  // Customer Transport
    //        }
    //        PickUpFrom = ((TextBox)FVJobDetail.FindControl("txtPickupLocation")).Text.Trim();
    //        Destination = ((TextBox)FVJobDetail.FindControl("txtLocationTo")).Text.Trim();
    //        ProductDesc = ((TextBox)FVJobDetail.FindControl("txtProductDesc")).Text.Trim();
    //        BuyerName = ((TextBox)FVJobDetail.FindControl("txtBuyerName")).Text.Trim();
    //        if (((TextBox)FVJobDetail.FindControl("txtNoOfPackages")).Text.Trim().ToString() != "")
    //            NoOfPkg = Convert.ToInt32(((TextBox)FVJobDetail.FindControl("txtNoOfPackages")).Text.Trim());
    //        PackageType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPackageType")).SelectedValue);
    //        if (((DropDownList)FVJobDetail.FindControl("ddlForwardedBy")).SelectedValue == "1") // Is Babaji forwarder
    //        {
    //            IsBabajiForwarder = 1;
    //            ForwarderName = "";
    //        }
    //        else
    //            ForwarderName = ((TextBox)FVJobDetail.FindControl("txtForwardedName")).Text.Trim();
    //        if (((DropDownList)FVJobDetail.FindControl("ddlContainerLoaded")).SelectedValue != "0")
    //            ContainerLoadedId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlContainerLoaded")).SelectedValue);
    //        if (((TextBox)FVJobDetail.FindControl("txtGrossWT")).Text.Trim().ToString() != "")
    //            GrossWT = Convert.ToDouble(((TextBox)FVJobDetail.FindControl("txtGrossWT")).Text.Trim());
    //        if (((TextBox)FVJobDetail.FindControl("txtNetWT")).Text.Trim().ToString() != "")
    //            NetWT = Convert.ToDouble(((TextBox)FVJobDetail.FindControl("txtNetWT")).Text.Trim());
    //        MAWBNo = ((TextBox)FVJobDetail.FindControl("txtMblNo")).Text.Trim();
    //        HAWBNo = ((TextBox)FVJobDetail.FindControl("txtHblNo")).Text.Trim();
    //        if (((TextBox)FVJobDetail.FindControl("txtMAWBDate")).Text != "")
    //            dtMAWBDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtMAWBDate")).Text.Trim());
    //        if (((TextBox)FVJobDetail.FindControl("txtHAWBDate")).Text != "")
    //            dtHAWBDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtHAWBDate")).Text.Trim());
    //        if (((TextBox)FVJobDetail.FindControl("txtPickUpDate")).Text != "")
    //            dtPickUpDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtPickUpDate")).Text.Trim());
    //        lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());
    //        Dimension = ((TextBox)FVJobDetail.FindControl("txtDimension")).Text.Trim();
    //        IsOctroi = ((CheckBox)FVJobDetail.FindControl("chkOctroi")).Checked;
    //        IsSForm = ((CheckBox)FVJobDetail.FindControl("chkSFrom")).Checked;
    //        IsNForm = ((CheckBox)FVJobDetail.FindControl("chkNFrom")).Checked;
    //        IsRoadPermit = ((CheckBox)FVJobDetail.FindControl("chkRoadPermit")).Checked;
    //        PickUpPersonName = ((TextBox)FVJobDetail.FindControl("txtPickupPersonName")).Text.Trim();
    //        PickUpMobileNo = ((TextBox)FVJobDetail.FindControl("txtPickupMobileNo")).Text.Trim();

    //        int result = EXOperations.EX_UpdateExportJob(JobId, BabajiBranchId, CustRefNo, CustomerId, ShipperId, DivisionId, PlantId, ConsigneeName, TransMode, ProductDesc, BuyerName, PortOfLoadingId, PortOfDischargeId,
    //                                    CountryConsignmentId, CountryDestinationId, ExportType, PackageType, ShippingBillType, TransportBy, Priority, PickUpFrom, Destination, NoOfPkg,
    //                                    IsBabajiForwarder, ForwarderName, ContainerLoadedId, GrossWT, NetWT, MAWBNo, dtMAWBDate, HAWBNo, dtHAWBDate,
    //                                    Dimension, lUser, IsOctroi, IsSForm, IsNForm, IsRoadPermit, PickUpPersonName, dtPickUpDate, PickUpMobileNo);

    //        if (result == 0)
    //        {
    //            lblError.Text = "Job Detail Updated Successfully !";
    //            lblError.CssClass = "success";
    //            FVJobDetail.ChangeMode(FormViewMode.ReadOnly);
    //            FVJobDetail.DataBind();
    //        }
    //        else if (result == 2)
    //        {
    //            lblError.Text = "Job Ref No Already Exist!";
    //            lblError.CssClass = "errorMsg";
    //        }
    //        else if (result == 1)
    //        {
    //            lblError.Text = "System Error! Please Try After Sometime.";
    //            lblError.CssClass = "errorMsg";
    //        }
    //    }//END_IF_JobId Check
    //    else
    //    {
    //        Response.Redirect("ShipmentTracking.aspx");
    //    }
    //}

    protected void btnEditJobDetail_OnClick(object sender, EventArgs e)
    {
        if (Session["JobId"] != null)
        {
            GetJobCancelDetail();
            FVJobDetail.ChangeMode(FormViewMode.Edit);
            //GetJobDetail(Convert.ToInt32(Session["JobId"]));
            FVJobDetail.DataBind();
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");
        DropDownList ddPlant = (DropDownList)FVJobDetail.FindControl("ddPlant");

        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);
        if (DivisonId != 0)
            DBOperations.FillCustomerPlant(ddPlant, DivisonId);
        ddDivision.Focus();
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");
        DropDownList ddlShipper = (DropDownList)FVJobDetail.FindControl("ddlShipper");
        TextBox txtCustomer = (TextBox)FVJobDetail.FindControl("txtCustomer");
        if (txtCustomer.Text == "")
        {
            ddlShipper.Items.Clear();
            hdnCustId.Value = "0";
        }

        int CustomerId = Convert.ToInt32(hdnCustId.Value);
        if (CustomerId > 0)
        {
            DBOperations.FillCustomerDivision(ddDivision, CustomerId);
            ddlShipper.DataBind();
            ddlShipper.Focus();
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddDivision.Items.Clear();
            ddDivision.Items.Add(lstSelect);
            ddlShipper.Items.Clear();
            txtCustomer.Focus();
        }
    }

    protected void ddlTransportBy_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlTransportBy = (DropDownList)FVJobDetail.FindControl("ddlTransportBy");
        if (ddlTransportBy.SelectedValue != "0")
        {
            if (ddlTransportBy.SelectedValue == "1") //Babaji Shivram
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = true;
                FVJobDetail.FindControl("trPickUpPersonDetails").Visible = true;
                FVJobDetail.FindControl("trPickUpPersonDetails2").Visible = true;
                FVJobDetail.FindControl("rfvDimension").Visible = true;
            }
            else //Customer
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = false;
                FVJobDetail.FindControl("trPickUpPersonDetails").Visible = false;
                FVJobDetail.FindControl("trPickUpPersonDetails2").Visible = false;
                FVJobDetail.FindControl("rfvDimension").Visible = false;
            }
        }
        else
        {
            FVJobDetail.FindControl("trPickUpDetails").Visible = false;
            FVJobDetail.FindControl("trPickUpPersonDetails").Visible = false;
            FVJobDetail.FindControl("trPickUpPersonDetails2").Visible = false;
            FVJobDetail.FindControl("rfvDimension").Visible = false;
        }
        ddlTransportBy.Focus();
    }

    protected void ddlExportType_OnSelectedIndexchanged(object sender, EventArgs e)
    {
        DropDownList ddlExportType = (DropDownList)FVJobDetail.FindControl("ddlExportType");
        DropDownList ddlContainerLoaded = (DropDownList)FVJobDetail.FindControl("ddlContainerLoaded");
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

    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {
        if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
        {
            Button btnEditJobDetail = (Button)FVJobDetail.FindControl("btnEditJobDetail");
            if (btnEditJobDetail != null)
                ScriptManager1.RegisterPostBackControl(btnEditJobDetail);

            DataRowView drv = (DataRowView)FVJobDetail.DataItem;
            if (drv["TransportBy"] != DBNull.Value && drv["TransportBy"].ToString().Trim().ToLower() == "babaji shivram")
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = true;
            }
            else
            {
                FVJobDetail.FindControl("trPickUpDetails").Visible = false;
            }

            if (drv["TransMode"] != DBNull.Value && drv["TransMode"].ToString().Trim().ToLower() == "air")
            {
                TabSeaContainer.Visible = false;
            }
            else
            {
                TabSeaContainer.Visible = true;
            }

            string dtLEODate = "", Remark = "";
            DataView dv = (DataView)DataSourceJobDetail.Select(DataSourceSelectArguments.Empty);
            DataTable dtCustomProcess = new DataTable();
            dtCustomProcess = dv.ToTable();
            if (dtCustomProcess.Rows.Count > 0)
            {
                foreach (DataRow row in dtCustomProcess.Rows)
                {
                    dtLEODate = row["LEODate"].ToString();
                    Remark = row["Remark"].ToString();
                }
            }

            if (dtLEODate == "")
            {
                btnEditJobDetail.Visible = true;
            }

            if (drv["FilingStatus"] != DBNull.Value)
            {
                if (Convert.ToInt32(drv["FilingStatus"]) == 1)
                {
                    btnEditJobDetail.Visible = false;
                }

                if (drv["Remark"].ToString() != null && drv["Remark"].ToString() != "")
                {
                    btnEditJobDetail.Visible = false;
                }
                else if (dtLEODate == "")
                {
                    btnEditJobDetail.Visible = true;
                }
            }
        }

        if (FVJobDetail.CurrentMode == FormViewMode.Edit)
        {
            Button btnUpdateJobDetail = (Button)FVJobDetail.FindControl("btnUpdateJobDetail");
            if (btnUpdateJobDetail != null)
                ScriptManager1.RegisterPostBackControl(btnUpdateJobDetail);

            txtCustomer_TextChanged(null, EventArgs.Empty);
            DataRowView drv = (DataRowView)FVJobDetail.DataItem;

            if (drv["ShipperId"] != DBNull.Value)
            {
                DropDownList ddlShipper = (DropDownList)FVJobDetail.FindControl("ddlShipper");
                ddlShipper.SelectedValue = drv["ShipperId"].ToString();
            }

            if (drv["DivisionId"] != DBNull.Value)
            {
                DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");
                ddDivision.SelectedValue = drv["DivisionId"].ToString();
            }

            ddDivision_SelectedIndexChanged(null, EventArgs.Empty);
            if (drv["PlantId"] != DBNull.Value)
            {
                DropDownList ddPlant = (DropDownList)FVJobDetail.FindControl("ddPlant");
                ddPlant.SelectedValue = drv["PlantId"].ToString();
            }

            if (drv["TransportById"] != DBNull.Value)
            {
                DropDownList ddlTransportBy = (DropDownList)FVJobDetail.FindControl("ddlTransportBy");
                int TransportBy = Convert.ToInt32(drv["TransportById"].ToString());
                if (TransportBy == 0) // Customer Transport
                    ddlTransportBy.SelectedValue = "2";
                else  // Babaji Shivram Transport               
                    ddlTransportBy.SelectedValue = "1";
            }

            ddlTransportBy_OnSelectedIndexChanged(null, EventArgs.Empty);
            ddlForwardedBy_OnSelectedIndexChanged(null, EventArgs.Empty);
            ddlExportType_OnSelectedIndexchanged(null, EventArgs.Empty);
            ddlMode_OnSelectedIndexChanged(null, EventArgs.Empty);

            if (drv["PortOfLoadingId"] != DBNull.Value)
                hdnLoadingPortId.Value = drv["PortOfLoadingId"].ToString();
            if (drv["PortOfDischargeId"] != DBNull.Value)
                hdnDischargePortId.Value = drv["PortOfDischargeId"].ToString();
            if (drv["ShipperId"] != DBNull.Value)
                hdnShipperId.Value = drv["ShipperId"].ToString();
            if (drv["ConsignmentCountryId"] != DBNull.Value)
                hdnCountryId.Value = drv["ConsignmentCountryId"].ToString();
            if (drv["DestinationCountryId"] != DBNull.Value)
                hdnDestCountryId.Value = drv["DestinationCountryId"].ToString();
        }
    }

    protected void FVJobDetail_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }
    }

    protected void FVJobDetail_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;

            lblError.Visible = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else if (e.AffectedRows == -1)
        {
            e.KeepInEditMode = true;
        }
    }

    protected void DataSourceJobDetail_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblError.CssClass = "success";
            lblError.Text = "Job Details Updated Successfully!";
        }
        else if (Result == 1)
        {
            lblError.Text = "Job detail does not exists..!!!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceJobDetail_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection UpdParams = ((SqlDataSourceView)sender).UpdateParameters;

        Hashtable ht = new Hashtable();
        foreach (Parameter UpdParam in UpdParams)
            ht.Add(UpdParam.Name, true);

        for (int i = 0; i < CmdParams.Count; i++)
        {
            if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
                CmdParams.Remove(CmdParams[i--]);
        }

    }

    #endregion

    #region SB PREPARE FORM VIEW EVENTS

    protected void btnUpdatePrepareDetail_OnClick(object sender, EventArgs e)
    {
        string FOBValue = "", CIFValue = "", Remark = "", CheckListPath = "";
        int JobId = 0, lUser = 0;
        JobId = Convert.ToInt32(Session["JobId"]);

        if (JobId > 0)
        {
            FileUpload fuChecklist = (FileUpload)FVSBPrepare.FindControl("fuUploadChecklist");
            HiddenField hdnCheckListPath = (HiddenField)FVSBPrepare.FindControl("hdnCheckListPath");
            lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());
            DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
            if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
                hdnCheckListPath.Value = dsGetJobDetail.Tables[0].Rows[0]["ChecklistDocPath"].ToString() + "\\" +
                                         dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

            if (fuChecklist.HasFile)
                CheckListPath = UploadCheckListFiles(hdnCheckListPath.Value, fuChecklist);

            FOBValue = ((TextBox)FVSBPrepare.FindControl("txtFOBValue")).Text.Trim();
            CIFValue = ((TextBox)FVSBPrepare.FindControl("txtCIFValue")).Text.Trim();
            CheckListPath = UploadCheckListFiles(hdnCheckListPath.Value, fuChecklist);

            int Result = EXOperations.EX_UpdateChecklistDetail(JobId, CheckListPath, FOBValue, CIFValue, lUser);
            if (Result == 0)
            {
                lblError.Text = "SB Preparation Detail Updated Successfully.";
                lblError.CssClass = "success";
                FVSBPrepare.ChangeMode(FormViewMode.ReadOnly);
                // JobDetailMS(Convert.ToInt32(Session["JobId"]));
                FVSBPrepare.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error!! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Checklist already sent for customer approval!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Checklist Not Ready For Preparation. Please Release the Hold Lock..!!";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("ShipmentTracking.aspx");
        }
    }

    protected void FVSBPrepare_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        if (e.CommandName == "Cancel")
        {
            FVSBPrepare.DefaultMode = FormViewMode.ReadOnly;
            FVSBPrepare.DataBind();
        }
    }

    protected void btnEditPrepareDetail_OnClick(object sender, EventArgs e)
    {
        if (Session["JobId"] != null)
        {
            FVSBPrepare.ChangeMode(FormViewMode.Edit);
            FVSBPrepare.DataBind();
        }
    }

    protected void FVSBPrepare_DataBound(object sender, EventArgs e)
    {
        if (FVSBPrepare.CurrentMode == FormViewMode.ReadOnly)
        {
            Button btnEditPrepareDetail = (Button)FVSBPrepare.FindControl("btnEditPrepareDetail");
            if (btnEditPrepareDetail != null)
                ScriptManager1.RegisterPostBackControl(btnEditPrepareDetail);

            // Checklist Download
            LinkButton lnkChecklistDoc = (LinkButton)FVSBPrepare.FindControl("lnkChecklistDoc_JobDetail");
            HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnChecklistDocPath2");
            ScriptManager1.RegisterPostBackControl(lnkChecklistDoc);

            if (hdnChecklistDocPath.Value.Trim() != "")
            {
                lnkChecklistDoc.Text = "Download";
                lnkChecklistDoc.Enabled = true;
            }

            DataRowView drv = (DataRowView)FVSBPrepare.DataItem;
            if (drv["FilingStatus"] != DBNull.Value)
            {
                if (Convert.ToInt32(drv["FilingStatus"]) == 1)
                {
                    btnEditPrepareDetail.Visible = false;
                }
            }
        }
        if (FVSBPrepare.CurrentMode == FormViewMode.Edit)
        {
            Button btnUpdatePrepareDetail = (Button)FVSBPrepare.FindControl("btnUpdatePrepareDetail");
            if (btnUpdatePrepareDetail != null)
                ScriptManager1.RegisterPostBackControl(btnUpdatePrepareDetail);

            DataRowView drv = (DataRowView)FVSBPrepare.DataItem;
            HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnEditChecklistDocPath");
            if (drv["ChecklistDocPath"] != DBNull.Value)
            {
                if (hdnChecklistDocPath != null)
                    hdnChecklistDocPath.Value = drv["ChecklistDocPath"].ToString();
            }

            // Checklist Download
            LinkButton lnkEditChecklistDoc_JobDetail = (LinkButton)FVSBPrepare.FindControl("lnkEditChecklistDoc_JobDetail");
            ScriptManager1.RegisterPostBackControl(lnkEditChecklistDoc_JobDetail);

            if (hdnChecklistDocPath.Value.Trim() != "")
            {
                lnkEditChecklistDoc_JobDetail.Text = "Download";
                lnkEditChecklistDoc_JobDetail.Enabled = true;
                lnkEditChecklistDoc_JobDetail.Visible = true;
            }
            else
            {
                lnkEditChecklistDoc_JobDetail.Text = "Not Uploaded";
                lnkEditChecklistDoc_JobDetail.Enabled = false;
                lnkEditChecklistDoc_JobDetail.Visible = true;
            }
        }
    }

    public string UploadCheckListFiles(string FilePath, FileUpload fuChecklist)
    {
        string FileName = fuChecklist.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + FilePath);
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

        if (fuChecklist.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuChecklist.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
    }

    #endregion

    #region SB FILING/CUSTOM PROCESS FORM VIEW EVENTS

    protected void btnUpdateFilingDetail_OnClick(object sender, EventArgs e)
    {
        TextBox txtSBNo = (TextBox)FVFilingCustomProcess.FindControl("txtSBNo");
        TextBox txtSBDate = (TextBox)FVFilingCustomProcess.FindControl("txtSBDate");
        DropDownList ddMarkAppraising = (DropDownList)FVFilingCustomProcess.FindControl("ddMarkAppraising");
        TextBox txtFilingRemark = (TextBox)FVFilingCustomProcess.FindControl("txtFilingRemark");
        DateTime dtSBDate = DateTime.MinValue, dtMarkPassingDate = DateTime.MinValue, dtRegistrationDate = DateTime.MinValue,
                 dtExamineDate = DateTime.MinValue, dtExamineReportdate = DateTime.MinValue, dtLEODate = DateTime.MinValue;

        int JobId = Convert.ToInt32(Session["JobId"]);
        int lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtSBDate")).Text.Trim().ToString() != "")
            dtSBDate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtSBDate")).Text.Trim());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtMarkPassingDate")).Text.Trim() != "")
            dtMarkPassingDate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtMarkPassingDate")).Text.Trim());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtRegistrationDate")).Text.Trim() != "")
            dtRegistrationDate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtRegistrationDate")).Text.Trim());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtExamineDate")).Text.Trim() != "")
            dtExamineDate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtExamineDate")).Text.Trim());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtExamineReportDate")).Text.Trim() != "")
            dtExamineReportdate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtExamineReportDate")).Text.Trim());
        if (((TextBox)FVFilingCustomProcess.FindControl("txtLEODate")).Text.Trim() != "")
            dtLEODate = Commonfunctions.CDateTime(((TextBox)FVFilingCustomProcess.FindControl("txtLEODate")).Text.Trim());

        if (JobId > 0)
        {
            int Result = EXOperations.EX_UpdateFilingCustomDetail(JobId, txtSBNo.Text.Trim(), dtSBDate, Convert.ToInt32(ddMarkAppraising.SelectedValue),
                                                        dtMarkPassingDate, dtRegistrationDate, dtExamineDate, dtExamineReportdate, dtLEODate, txtFilingRemark.Text.Trim(), lUser);
            if (Result == 0)
            {
                lblError.Text = "SB Filing/Custom Process Detail Updated Successfully.";
                lblError.CssClass = "success";
                FVFilingCustomProcess.ChangeMode(FormViewMode.ReadOnly);
                FVFilingCustomProcess.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error!! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Checklist already sent for customer approval!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Checklist Not Ready For Preparation. Please Release the Hold Lock..!!";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("ShipmentTracking.aspx");
        }
    }

    protected void FVFilingCustomProcess_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        if (e.CommandName == "Cancel")
        {
            FVFilingCustomProcess.DefaultMode = FormViewMode.ReadOnly;
            FVFilingCustomProcess.DataBind();
        }
    }

    protected void btnEditFilingDetail_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (Session["JobId"] != null)
            {
                FVFilingCustomProcess.ChangeMode(FormViewMode.Edit);
                FVFilingCustomProcess.DataBind();
            }
        }
        catch (Exception en)
        {
            throw;
        }
    }

    protected void FVFilingCustomProcess_DataBound(object sender, EventArgs e)
    {
        if (FVFilingCustomProcess.CurrentMode == FormViewMode.ReadOnly)
        {
            Button btnEditFilingDetail = (Button)FVFilingCustomProcess.FindControl("btnEditFilingDetail");
            if (btnEditFilingDetail != null)
                ScriptManager1.RegisterPostBackControl(btnEditFilingDetail);
        }

        if (FVFilingCustomProcess.CurrentMode == FormViewMode.Edit)
        {
            Button btnUpdateFilingDetail = (Button)FVFilingCustomProcess.FindControl("btnUpdateFilingDetail");
            if (btnUpdateFilingDetail != null)
                ScriptManager1.RegisterPostBackControl(btnUpdateFilingDetail);

            DataRowView drv = (DataRowView)FVFilingCustomProcess.DataItem;

            //// Exporter Copy
            //HiddenField hdnEditExporterCopy = (HiddenField)FVFilingCustomProcess.FindControl("hdnEditExporterCopy");
            //if (drv["ExporterCopyPath"] != DBNull.Value)
            //{
            //    if (hdnEditExporterCopy != null)
            //        hdnEditExporterCopy.Value = drv["ExporterCopyPath"].ToString();
            //}

            //LinkButton lnkEditDwnloadExporterCopy = (LinkButton)FVFilingCustomProcess.FindControl("lnkEditDwnloadExporterCopy");
            //ScriptManager1.RegisterPostBackControl(lnkEditDwnloadExporterCopy);

            //if (hdnEditExporterCopy.Value.Trim() != "")
            //{
            //    lnkEditDwnloadExporterCopy.Text = "Download";
            //    lnkEditDwnloadExporterCopy.Enabled = true;
            //    lnkEditDwnloadExporterCopy.Visible = true;
            //}
            //else
            //{
            //    lnkEditDwnloadExporterCopy.Text = "Not Uploaded";
            //    lnkEditDwnloadExporterCopy.Enabled = false;
            //    lnkEditDwnloadExporterCopy.Visible = true;
            //}

            //// VGMCopy Path
            //HiddenField hdnVGMCopy = (HiddenField)FVFilingCustomProcess.FindControl("hdnVGMCopy");
            //if (drv["VGMCopyPath"] != DBNull.Value)
            //{
            //    if (hdnVGMCopy != null)
            //        hdnVGMCopy.Value = drv["VGMCopyPath"].ToString();
            //    lblDisplay.Text = drv["VGMCopyPath"].ToString();
            //}

            //LinkButton lnkDwnloadVGMCopy = (LinkButton)FVFilingCustomProcess.FindControl("lnkDwnloadVGMCopy");
            //ScriptManager1.RegisterPostBackControl(lnkDwnloadVGMCopy);

            //if (hdnVGMCopy.Value.Trim() != "")
            //{
            //    lnkDwnloadVGMCopy.Text = "Download";
            //    lnkDwnloadVGMCopy.Enabled = true;
            //    lnkDwnloadVGMCopy.Visible = true;
            //}
            //else
            //{
            //    lnkDwnloadVGMCopy.Text = "Not Uploaded";
            //    lnkDwnloadVGMCopy.Enabled = false;
            //    lnkDwnloadVGMCopy.Visible = true;
            //}
        }
    }
    #endregion

    #region FORM 13 FORM VIEW EVENTS

    protected void btnUpdateForm13Detail_OnClick(object sender, EventArgs e)
    {
        TextBox txtForm13Date = (TextBox)FVForm13Detail.FindControl("txtForm13Date");
        TextBox txtTransHandOverDate = (TextBox)FVForm13Detail.FindControl("txtTransHandOverDate");
        TextBox txtContainerGetInDate = (TextBox)FVForm13Detail.FindControl("txtContainerGetInDate");
        DateTime dtForm13Date = DateTime.MinValue, dtTransHandOverDate = DateTime.MinValue, dtContainerGetIndate = DateTime.MinValue;
        if (txtForm13Date.Text.Trim().ToString() != "")
            dtForm13Date = Commonfunctions.CDateTime(txtForm13Date.Text.Trim());
        if (txtTransHandOverDate.Text.Trim().ToString() != "")
            dtTransHandOverDate = Commonfunctions.CDateTime(txtTransHandOverDate.Text.Trim());
        if (txtContainerGetInDate.Text.Trim().ToString() != "")
            dtContainerGetIndate = Commonfunctions.CDateTime(txtContainerGetInDate.Text.Trim());

        int JobId = Convert.ToInt32(Session["JobId"]);
        int lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());

        if (JobId > 0)
        {
            int Result = EXOperations.EX_UpdateForm13JobDetail(JobId, dtForm13Date, dtTransHandOverDate, dtContainerGetIndate, lUser);
            if (Result == 0)
            {
                lblError.Text = "Form 13 Detail Updated Successfully.";
                lblError.CssClass = "success";
                FVForm13Detail.ChangeMode(FormViewMode.ReadOnly);
                FVForm13Detail.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error!! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Checklist already sent for customer approval!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Checklist Not Ready For Preparation. Please Release the Hold Lock..!!";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("ShipmentTracking.aspx");
        }
    }

    protected void FVForm13Detail_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        if (e.CommandName == "Cancel")
        {
            FVForm13Detail.DefaultMode = FormViewMode.ReadOnly;
            FVForm13Detail.DataBind();
        }
    }

    protected void btnEditForm13Detail_OnClick(object sender, EventArgs e)
    {
        if (Session["JobId"] != null)
        {
            FVForm13Detail.ChangeMode(FormViewMode.Edit);
            FVForm13Detail.DataBind();
        }
    }

    protected void FVForm13Detail_DataBound(object sender, EventArgs e)
    {
        if (FVForm13Detail.CurrentMode == FormViewMode.ReadOnly)
        {
            Button btnEditForm13Detail = (Button)FVForm13Detail.FindControl("btnEditForm13Detail");
            if (btnEditForm13Detail != null)
                ScriptManager1.RegisterPostBackControl(btnEditForm13Detail);
        }
        if (FVForm13Detail.CurrentMode == FormViewMode.Edit)
        {
            Button btnUpdateForm13Detail = (Button)FVForm13Detail.FindControl("btnUpdateForm13Detail");
            if (btnUpdateForm13Detail != null)
                ScriptManager1.RegisterPostBackControl(btnUpdateForm13Detail);
        }
    }
    #endregion

    #region SHIPMENT GET IN FORM VIEW EVENTS

    protected void btnUpdateShipmentDetail_OnClick(object sender, EventArgs e)
    {
        DateTime dtDocHandOverDate = DateTime.MinValue, dtFreightForwarderDate = DateTime.MinValue;
        string strFilePath = "", ExporterfilePath = "", VGMfilePath = "", ForwardToEmail = "";
        TextBox txtDocHandOverDate = (TextBox)FVShipmentGetIN.FindControl("txtDocHandOverDate");
        TextBox txtFreightForwarderDate = (TextBox)FVShipmentGetIN.FindControl("txtFreightForwarderDate");
        TextBox txtForwarderPerson = (TextBox)FVShipmentGetIN.FindControl("txtForwarderPerson");
        FileUpload fuExporterCopy = (FileUpload)FVShipmentGetIN.FindControl("fuExporterCopy");
        FileUpload fuVGMCopy = (FileUpload)FVShipmentGetIN.FindControl("fuVGMcopy");

        if (txtDocHandOverDate.Text.Trim().ToString() != "")
            dtDocHandOverDate = Commonfunctions.CDateTime(txtDocHandOverDate.Text.Trim());
        if (txtFreightForwarderDate.Text.Trim().ToString() != "")
            dtFreightForwarderDate = Commonfunctions.CDateTime(txtFreightForwarderDate.Text.Trim());

        int JobId = Convert.ToInt32(Session["JobId"]);
        int lUser = Convert.ToInt32(LoggedInUser.glUserId.ToString());
        DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
        if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
            strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
        if (fuExporterCopy.HasFile)
        {
            ExporterfilePath = UploadFiles(fuExporterCopy, strFilePath);
        }

        if (fuVGMCopy.HasFile)
        {
            VGMfilePath = UploadFiles(fuVGMCopy, strFilePath);
        }

        if (JobId > 0)
        {
            int Result = EXOperations.EX_AddShippingGetInDetail(JobId, dtDocHandOverDate, ExporterfilePath, VGMfilePath, dtFreightForwarderDate, txtForwarderPerson.Text.Trim(),
                                                           ForwardToEmail, LoggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Shipment Get IN Detail Updated Successfully.";
                lblError.CssClass = "success";
                FVShipmentGetIN.ChangeMode(FormViewMode.ReadOnly);
                FVShipmentGetIN.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error!! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Checklist already sent for customer approval!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Checklist Not Ready For Preparation. Please Release the Hold Lock..!!";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("ShipmentTracking.aspx");
        }
    }

    protected void FVShipmentGetIN_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        if (e.CommandName == "Cancel")
        {
            FVShipmentGetIN.DefaultMode = FormViewMode.ReadOnly;
            FVShipmentGetIN.DataBind();
        }
    }

    protected void btnEditShipmentDetail_OnClick(object sender, EventArgs e)
    {
        if (Session["JobId"] != null)
        {
            FVForm13Detail.ChangeMode(FormViewMode.Edit);
            FVForm13Detail.DataBind();
        }
    }

    protected void FVShipmentGetIN_DataBound(object sender, EventArgs e)
    {
        if (FVShipmentGetIN.CurrentMode == FormViewMode.ReadOnly)
        {
            Button btnEditForm13Detail = (Button)FVShipmentGetIN.FindControl("btnEditForm13Detail");
            if (btnEditForm13Detail != null)
                ScriptManager1.RegisterPostBackControl(btnEditForm13Detail);

            // Exporter Copy Download
            LinkButton lnkDwnloadExporterCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkDwnloadExporterCopy");
            HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnExporterCopy");
            ScriptManager1.RegisterPostBackControl(lnkDwnloadExporterCopy);

            if (hdnExporterCopy.Value.Trim() != "")
            {
                lnkDwnloadExporterCopy.Text = "Download";
                lnkDwnloadExporterCopy.Enabled = true;
            }

            // VGM Copy Download
            LinkButton lnkDwnloadVGMCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkDwnloadVGMCopy");
            HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
            ScriptManager1.RegisterPostBackControl(lnkDwnloadVGMCopy);

            if (hdnVGMCopy.Value.Trim() != "")
            {
                lnkDwnloadVGMCopy.Text = "Download";
                lnkDwnloadVGMCopy.Enabled = true;
            }
        }
        if (FVShipmentGetIN.CurrentMode == FormViewMode.Edit)
        {
            Button btnUpdateForm13Detail = (Button)FVShipmentGetIN.FindControl("btnUpdateForm13Detail");
            if (btnUpdateForm13Detail != null)
                ScriptManager1.RegisterPostBackControl(btnUpdateForm13Detail);

            // Exporter Copy Download
            LinkButton lnkEditDwnloadExporterCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkEditDwnloadExporterCopy");
            HiddenField hdnEditExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnEditExporterCopy");
            ScriptManager1.RegisterPostBackControl(lnkEditDwnloadExporterCopy);

            if (hdnEditExporterCopy.Value.Trim() != "")
            {
                lnkEditDwnloadExporterCopy.Text = "Download";
                lnkEditDwnloadExporterCopy.Enabled = true;
            }

            // VGM Copy Download
            LinkButton lnkDwnloadVGMCopy = (LinkButton)FVShipmentGetIN.FindControl("lnkDwnloadVGMCopy");
            HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
            ScriptManager1.RegisterPostBackControl(lnkDwnloadVGMCopy);

            if (hdnVGMCopy.Value.Trim() != "")
            {
                lnkDwnloadVGMCopy.Text = "Download";
                lnkDwnloadVGMCopy.Enabled = true;
            }
        }
    }
    #endregion

    #region Update Job Event

    protected void btnUpdateJob_Click(object sender, EventArgs e)
    {
        DropDownList ddlCancelAllow = (DropDownList)FVJobDetail.FindControl("ddlCancelAllow");
        DropDownList ddlReason = (DropDownList)FVJobDetail.FindControl("ddlReason");
        TextBox txtCancelRemark = (TextBox)FVJobDetail.FindControl("txtCancelRemark");
        if (ddlCancelAllow.SelectedValue == "1")
        {
            if (txtCancelRemark.Text != "" && ddlReason.SelectedValue != "0")
            {
                SaveUpdatedJobDetails();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please enter cancel Remark and select cancel Reason";
                lblMessage.Text = "Please enter cancel Remark and select cancel Reason";
                lblError.CssClass = "errorMsg";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('msg');", true);
            }
        }
        else
        {
            SaveUpdatedJobDetails();
        }

    }


    protected void SaveUpdatedJobDetails()
    {
        int CountFields = 0, DivisionId = 0, PlantId = 0, Priority = 0, BabajiBranchId = 0, JobId = 0, DocumentId = 0, CustomerId = 0, ShipperId = 0, TransMode = 0, PortOfLoadingId = 0, PortOfDischargeId = 0, CountryConsignmentId = 0, CountryDestinationId = 0, NoOfPkg = 0,
            PackageType = 0, ShippingBillType = 0, ContainerLoadedId = 0, TransportBy = 0, lUser = 0, IsBabajiForwarder = 0, ExportType = 0, ModeId = 0;
        double GrossWT = 0.0, NetWT = 0.0;
        string filePath = "", strCustRefNo = "", strFilePath = "", MAWBNo = "", HAWBNo = "", ForwarderName = "", PickupPersonName = "",
            PickupMobileNo = "", Dimension = "", ConsigneeName = "", PickUpLocation = "", DropLocation = "", ProductDesc = "", BuyerName = "",
            SealNo = "", JobRemark = "";
        DateTime dtHAWBDate = DateTime.MinValue, dtMAWBDate = DateTime.MinValue, dtPickUpDate = DateTime.MinValue;
        bool IsOctroi = false, IsSForm = false, IsNForm = false, IsRoadPermit = false;

        JobId = Convert.ToInt32(Session["JobId"]);
        if (JobId > 0)
        {
            BabajiBranchId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlBabajiBranch")).SelectedValue);
            strCustRefNo = ((TextBox)FVJobDetail.FindControl("txtCustRefNo")).Text.Trim();
            CustomerId = Convert.ToInt32(hdnCustId.Value);
            ShipperId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlShipper")).SelectedValue);
            DivisionId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddDivision")).SelectedValue);
            PlantId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddPlant")).SelectedValue);
            ConsigneeName = ((TextBox)FVJobDetail.FindControl("txtConsignee")).Text.Trim();
            ModeId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlMode")).SelectedValue);
            PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
            PortOfDischargeId = Convert.ToInt32(hdnDischargePortId.Value);
            CountryConsignmentId = Convert.ToInt32(hdnCountryId.Value);
            CountryDestinationId = Convert.ToInt32(hdnDestCountryId.Value);
            ExportType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlExportType")).SelectedValue);
            ShippingBillType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlShippingBillType")).SelectedValue);
            Priority = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPriority")).SelectedValue);
            TransportBy = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlTransportBy")).SelectedValue);
            PickUpLocation = ((TextBox)FVJobDetail.FindControl("txtPickupLocation")).Text.Trim();
            DropLocation = ((TextBox)FVJobDetail.FindControl("txtLocationTo")).Text.Trim();
            PickupPersonName = ((TextBox)FVJobDetail.FindControl("txtPickupPersonName")).Text.Trim();
            PickupMobileNo = ((TextBox)FVJobDetail.FindControl("txtPickupMobileNo")).Text.Trim();
            ProductDesc = ((TextBox)FVJobDetail.FindControl("txtProductDesc")).Text.Trim();
            BuyerName = ((TextBox)FVJobDetail.FindControl("txtBuyerName")).Text.Trim();
            NoOfPkg = Convert.ToInt32(((TextBox)FVJobDetail.FindControl("txtNoOfPackages")).Text.Trim());
            PickupMobileNo = ((TextBox)FVJobDetail.FindControl("txtPickupMobileNo")).Text.Trim();
            PackageType = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPackageType")).SelectedValue);

            if (((DropDownList)FVJobDetail.FindControl("ddlForwardedBy")).SelectedValue == "1") // Is Babaji forwarder
            {
                IsBabajiForwarder = 1;
                ForwarderName = "";
            }
            else
            {
                ForwarderName = ((TextBox)FVJobDetail.FindControl("txtForwardedName")).Text.Trim();
            }

            ContainerLoadedId = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlContainerLoaded")).SelectedValue);
            SealNo = ((TextBox)FVJobDetail.FindControl("txtSealNo")).Text.Trim();

            if (((TextBox)FVJobDetail.FindControl("txtGrossWT")).Text.Trim() != "")
            {
                GrossWT = Convert.ToDouble(((TextBox)FVJobDetail.FindControl("txtGrossWT")).Text.Trim());
            }
            if (((TextBox)FVJobDetail.FindControl("txtNetWT")).Text.Trim() != "")
            {
                NetWT = Convert.ToDouble(((TextBox)FVJobDetail.FindControl("txtNetWT")).Text.Trim());
            }

            MAWBNo = ((TextBox)FVJobDetail.FindControl("txtMblNo")).Text.Trim();
            HAWBNo = ((TextBox)FVJobDetail.FindControl("txtHblNo")).Text.Trim();

            if (((TextBox)FVJobDetail.FindControl("txtPickUpDate")).Text.Trim() != "")
                dtPickUpDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtPickUpDate")).Text.Trim());

            if (((TextBox)FVJobDetail.FindControl("txtMAWBDate")).Text.Trim() != "")
                dtMAWBDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtMAWBDate")).Text.Trim());

            if (((TextBox)FVJobDetail.FindControl("txtHAWBDate")).Text.Trim() != "")
                dtHAWBDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtHAWBDate")).Text.Trim());

            if (((TextBox)FVJobDetail.FindControl("txtDimension")).Text.Trim() != "")
            {
                Dimension = ((TextBox)FVJobDetail.FindControl("txtDimension")).Text.Trim();
            }
            JobRemark = ((TextBox)FVJobDetail.FindControl("txtJobRemark")).Text.Trim();

            int result = EXOperations.EX_UpdateExportJob(JobId, BabajiBranchId, strCustRefNo, CustomerId, ShipperId, DivisionId, PlantId,
                ConsigneeName, ModeId, ProductDesc, BuyerName, PortOfLoadingId, PortOfDischargeId, CountryConsignmentId, CountryDestinationId,
                ExportType, PackageType, ShippingBillType, TransportBy, Priority, PickUpLocation, DropLocation, NoOfPkg, IsBabajiForwarder,
                ForwarderName, ContainerLoadedId, GrossWT, NetWT, MAWBNo, dtMAWBDate, HAWBNo, dtHAWBDate, Dimension, LoggedInUser.glUserId,
                IsOctroi, IsSForm, IsNForm, IsRoadPermit, PickupPersonName, dtPickUpDate, PickupMobileNo, JobRemark);

            if (result == 0)
            {
                DropDownList ddlCancelAllow = (DropDownList)FVJobDetail.FindControl("ddlCancelAllow");
                if (ddlCancelAllow.SelectedValue == "1")
                {

                    DropDownList ddlReason1 = (DropDownList)FVJobDetail.FindControl("ddlReason");
                    string Remark = ((TextBox)FVJobDetail.FindControl("txtCancelRemark")).Text.Trim();
                    result = DBOperations.Ex_InsJobStatus(Convert.ToInt32(Session["JobId"]), 7, ddlReason1.SelectedItem.Text, Remark, LoggedInUser.glModuleId, LoggedInUser.glFinYearId, LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        result = DBOperations.AddJobDailyActivity(Convert.ToInt32(Session["JobId"]), Remark, "", 7, true, LoggedInUser.glUserId);
                    }
                }

                if (result == 0)
                {
                    Button btnEditJobDetail = (Button)FVJobDetail.FindControl("btnEditJobDetail");
                    Button btnEditPrepareDetail = (Button)FVSBPrepare.FindControl("btnEditPrepareDetail");
                    if (btnEditJobDetail != null)
                    {
                        btnEditJobDetail.Visible = false;
                    }
                    //FVJobDetail.FindControl("btnEditJobDetail").Visible = false;
                    lblError.Text = "Job Detail Updated Successfully !";
                    lblError.CssClass = "success";
                    FVJobDetail.ChangeMode(FormViewMode.ReadOnly);
                    FVJobDetail.DataBind();
                }
            }
            else if (result == 2)
            {
                lblError.Text = "Job Detail Not Found!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("JobTracking.aspx");
        }
    }

    #endregion

    #region Delivery Details    
    protected void btnUpdateJobDelivery_Click(object sender, EventArgs e)
    {
        string Destination = "";
        int TransportationByBabaji = 0;
        int JobId = Convert.ToInt32(Session["JobId"].ToString());

        if (txtDeliveryDestination.Text.Trim() != "")
            Destination = txtDeliveryDestination.Text.Trim();

        if (rdlTransport.SelectedValue == "1")
            TransportationByBabaji = 1;

        if (Destination == "")
        {
            lblError.Text = "Please Enter Delivery Destination.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int result = EXOperations.UpdateDeliveryDetail(JobId, Destination, TransportationByBabaji, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Delivery Detail Updated Successfully..!!";
                lblError.CssClass = "success";
                JobDetailMS(JobId);
            }
            else if (result == 1)
            {
                lblError.Text = "Error while updating delivery detail. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void GridViewWarehouse_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        int vehicleId = 0, NoOfPackages = 0;
        string vehiclename = "";
        if (e.CommandName.ToLower() == "update")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewWarehouse.DataKeys[gvrow.RowIndex].Value.ToString();
            DataSourceWarehouse.UpdateParameters["lId"].DefaultValue = strlId;
            // Update Delivered Package Except Loaded Shipment
            TextBox txtPackages = (TextBox)gvrow.FindControl("txtPackages");
            if (txtPackages.Text.Trim() != "")
            {
                NoOfPackages = Convert.ToInt32(txtPackages.Text.Trim());
                DataSourceWarehouse.UpdateParameters["NoOfPackages"].DefaultValue = NoOfPackages.ToString();
            }

            TextBox txtVehicleNo = (TextBox)gvrow.FindControl("txtVehicleNo");
            string strVehicleNo = txtVehicleNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["VehicleNo"].DefaultValue = strVehicleNo;

            DropDownList ddVehicleType = (DropDownList)gvrow.FindControl("ddVehicleType");
            if (ddVehicleType.SelectedValue != "0")
            {
                vehiclename = ddVehicleType.SelectedItem.Text;
                DataSourceWarehouse.UpdateParameters["VehicleType"].DefaultValue = Convert.ToString(ddVehicleType.SelectedValue);
            }

            TextBox txtVehicleRcvdDate = (TextBox)gvrow.FindControl("txtVehicleRcvdDate");
            string strVehicleRcvdDate = txtVehicleRcvdDate.Text.Trim();
            if (strVehicleRcvdDate != "")
            {
                strVehicleRcvdDate = Commonfunctions.CDateTime(strVehicleRcvdDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["VehicleRcvdDate"].DefaultValue = strVehicleRcvdDate;
            }

            DropDownList ddTransporter = (DropDownList)gvrow.FindControl("ddTransporter");
            if (ddTransporter.SelectedValue != "0")
            {
                DataSourceWarehouse.UpdateParameters["TransporterID"].DefaultValue = Convert.ToString(ddTransporter.SelectedValue);
            }

            TextBox txtLRNo = (TextBox)gvrow.FindControl("txtLRNo");
            string LRNo = txtLRNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["LRNo"].DefaultValue = LRNo;

            TextBox txtLRDate = (TextBox)gvrow.FindControl("txtLRDate");
            string strLRDate = txtLRDate.Text.Trim();
            if (strLRDate != "")
            {
                strLRDate = Commonfunctions.CDateTime(strLRDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["LRDate"].DefaultValue = strLRDate;
            }

            TextBox txtDeliveryPoint = (TextBox)gvrow.FindControl("txtDeliveryPoint");
            string strDeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DeliveryPoint"].DefaultValue = strDeliveryPoint;

            TextBox txtDispatchDate = (TextBox)gvrow.FindControl("txtDispatchDate");
            string strDispatchDate = txtDispatchDate.Text.Trim();
            if (strDispatchDate != "")
            {
                strDispatchDate = Commonfunctions.CDateTime(strDispatchDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["DispatchDate"].DefaultValue = strDispatchDate;
            }

            TextBox txtDeliveryDate = (TextBox)gvrow.FindControl("txtDeliveryDate");
            string strDeliveryDate = txtDeliveryDate.Text.Trim();
            if (strDeliveryDate != "")
            {
                strDeliveryDate = Commonfunctions.CDateTime(strDeliveryDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["DeliveryDate"].DefaultValue = strDeliveryDate;
            }

            TextBox txtRoadPermitNo = (TextBox)gvrow.FindControl("txtRoadPermitNo");
            string RoadPermitNo = txtRoadPermitNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["RoadPermitNo"].DefaultValue = RoadPermitNo;

            TextBox txtRoadPermitDate = (TextBox)gvrow.FindControl("txtRoadPermitDate");
            string strRoadPermitDate = txtRoadPermitDate.Text.Trim();
            if (strRoadPermitDate != "")
            {
                strRoadPermitDate = Commonfunctions.CDateTime(strRoadPermitDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["RoadPermitDate"].DefaultValue = strRoadPermitDate;
            }

            TextBox txtCargoRecBy = (TextBox)gvrow.FindControl("txtCargoRecBy");
            string CargoRecby = txtCargoRecBy.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["CargoReceivedBy"].DefaultValue = CargoRecby;

            string PODPath = "";
            FileUpload fuattach = (FileUpload)gvrow.FindControl("fuPODAttchment");
            HiddenField hdnPODPath = (HiddenField)gvrow.FindControl("hdnPODPath");

            string FileName = fuattach.FileName;
            FileName = FileServer.ValidateFileName(FileName);

            if (fuattach.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = hdnUploadPath.Value;
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

                if (fuattach.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuattach.SaveAs(ServerFilePath + FileName);
                }

                PODPath = FilePath + FileName;
                DataSourceWarehouse.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }
            else if (hdnPODPath.Value != "")
            {
                PODPath = hdnPODPath.Value;
                DataSourceWarehouse.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }

            string BCCPath_Warehouse = "";
            FileUpload fuattach_Warehouse = (FileUpload)gvrow.FindControl("fuBCCAttchment_Warehouse");
            HiddenField hdnBccPath_Warehouse = (HiddenField)gvrow.FindControl("hdnBCCPath_Warehouse");

            string FileName1 = fuattach_Warehouse.FileName;
            FileName = FileServer.ValidateFileName(FileName1);

            if (fuattach_Warehouse.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "BCCFiles\\";

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

                if (fuattach_Warehouse.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuattach_Warehouse.SaveAs(ServerFilePath + FileName);
                }

                BCCPath_Warehouse = FilePath + FileName;
                DataSourceWarehouse.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath_Warehouse;
            }
            else if (hdnBccPath_Warehouse.Value != "")
            {
                BCCPath_Warehouse = hdnBccPath_Warehouse.Value;
                DataSourceWarehouse.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath_Warehouse;
            }

            TextBox txtNFormNo = (TextBox)gvrow.FindControl("txtNFormNo");
            string NFormNo = txtNFormNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["NFormNo"].DefaultValue = NFormNo;

            TextBox txtNFormDate = (TextBox)gvrow.FindControl("txtNFormDate");
            string strNFormDate = txtNFormDate.Text.Trim();
            if (strNFormDate != "")
            {
                strNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["NFormDate"].DefaultValue = strNFormDate;
            }

            TextBox txtNClosingDate = (TextBox)gvrow.FindControl("txtNClosingDate");
            string strNClosingDate = txtNClosingDate.Text.Trim();
            if (strNClosingDate != "")
            {
                strNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["NClosingDate"].DefaultValue = strNClosingDate;
            }

            TextBox txtSFormNo = (TextBox)gvrow.FindControl("txtSFormNo");
            string SFormNo = txtSFormNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["SFormNo"].DefaultValue = SFormNo;

            TextBox txtSFormDate = (TextBox)gvrow.FindControl("txtSFormDate");
            string strSFormDate = txtSFormDate.Text.Trim();
            if (strSFormDate != "")
            {
                strSFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["SFormDate"].DefaultValue = strSFormDate;
            }

            TextBox txtSClosingDate = (TextBox)gvrow.FindControl("txtSClosingDate");
            string strSClosingDate = txtSClosingDate.Text.Trim();
            if (strSClosingDate != "")
            {
                strSClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["SClosingDate"].DefaultValue = strSClosingDate;
            }

            TextBox txtOctroiAmount = (TextBox)gvrow.FindControl("txtOctroiAmount");
            string strOctroiAmount = txtOctroiAmount.Text.Trim();
            DataSourceWarehouse.UpdateParameters["OctroiAmount"].DefaultValue = strOctroiAmount;

            TextBox txtOctroiReceiptNo = (TextBox)gvrow.FindControl("txtOctroiReceiptNo");
            string strOctroiReceiptNo = txtOctroiReceiptNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["OctroiReceiptNo"].DefaultValue = strOctroiReceiptNo;

            TextBox txtOctroiPaidDate = (TextBox)gvrow.FindControl("txtOctroiPaidDate");
            string strOctroiPaidDate = txtOctroiPaidDate.Text.Trim();
            if (strOctroiPaidDate != "")
            {
                strOctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["OctroiPaidDate"].DefaultValue = strOctroiPaidDate;
            }

            TextBox txtChallanNo = (TextBox)gvrow.FindControl("txtChallanNo");
            string ChallanNo = txtChallanNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["BabajiChallanNo"].DefaultValue = ChallanNo;

            TextBox txtChalanDate = (TextBox)gvrow.FindControl("txtChalanDate");
            string strBabajiChallanDate = txtChalanDate.Text.Trim();
            if (strBabajiChallanDate != "")
            {
                strBabajiChallanDate = Commonfunctions.CDateTime(strBabajiChallanDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["BabajiChallanDate"].DefaultValue = strBabajiChallanDate;
            }

            TextBox txtdrivername = (TextBox)gvrow.FindControl("txtdrivername");
            string strdrivername = txtdrivername.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DriverName"].DefaultValue = strdrivername;

            TextBox txtdriverphoneno = (TextBox)gvrow.FindControl("txtdriverphoneno");
            string strdriverphoneno = txtdriverphoneno.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DriverPhone"].DefaultValue = strdriverphoneno;

            DataSourceWarehouse.Update();
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            lblError.Visible = false;
            lblError.Text = "";
            GridViewWarehouse.EditIndex = -1;
        }

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadPODDocument(DocPath);
        }

        if (e.CommandName.ToLower() == "delete1")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int lid = Convert.ToInt32(GridViewWarehouse.DataKeys[gvrow.RowIndex].Value.ToString());
            int Result = -123;

            Result = EXOperations.EX_DeleteDeliveryWarehouseDetail(lid, LoggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Delivery to Warehouse Deleted Successfully!";
                lblError.CssClass = "success";
                GridViewWarehouse.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = "Job Details Not Found!";
            }
        }
    }
    protected void GridViewWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int Mode = (Int32)DataBinder.Eval(e.Row.DataItem, "TransModeId");
            bool IsNForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsNForm");
            bool IsSForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsSForm");
            bool IsOctroi = (bool)DataBinder.Eval(e.Row.DataItem, "IsOctroi");
            bool IsRoadPermit = (bool)DataBinder.Eval(e.Row.DataItem, "IsRoadPermit");

            if (Mode == (Int32)TransMode.Sea) // Delivery Type
            {
                GridViewWarehouse.Columns[2].Visible = true; // Container NO
                GridViewWarehouse.Columns[3].Visible = false; // No Of Packages
            }
            else
            {
                GridViewWarehouse.Columns[2].Visible = false; // Container NO
                GridViewWarehouse.Columns[3].Visible = true; // No Of Packages
            }

            if (IsNForm == true) // NForm Applicable
            {
                GridViewWarehouse.Columns[17].Visible = true; // N Form No
                GridViewWarehouse.Columns[18].Visible = true; // N Form Date
                GridViewWarehouse.Columns[19].Visible = true; // N Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[17].Visible = false; // N Form No
                GridViewWarehouse.Columns[18].Visible = false; // N Form Date
                GridViewWarehouse.Columns[19].Visible = false; // N Closing Date
            }

            if (IsSForm == true) // SForm Applicable
            {
                GridViewWarehouse.Columns[20].Visible = true; // S Form No
                GridViewWarehouse.Columns[21].Visible = true; // S Form Date
                GridViewWarehouse.Columns[22].Visible = true; // S Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[20].Visible = false; // S Form No
                GridViewWarehouse.Columns[21].Visible = false; // S Form Date
                GridViewWarehouse.Columns[22].Visible = false; // S Closing Date
            }
            if (IsOctroi == true) // Octroi Applicable
            {
                GridViewWarehouse.Columns[23].Visible = true; // Octroi Amount
                GridViewWarehouse.Columns[24].Visible = true; // Octroi Receipt No	
                GridViewWarehouse.Columns[25].Visible = true; // Octroi Paid Date
            }
            else
            {
                GridViewWarehouse.Columns[23].Visible = false; // Octroi Amount
                GridViewWarehouse.Columns[24].Visible = false; // Octroi Receipt No	
                GridViewWarehouse.Columns[25].Visible = false; // Octroi Paid Date
            }
            if (IsRoadPermit == true) // Road Permit Applicable
            {
                GridViewWarehouse.Columns[13].Visible = true; // Road Permit No
                GridViewWarehouse.Columns[14].Visible = true; // Road Permit Date
            }
            else
            {
                GridViewWarehouse.Columns[13].Visible = false; // Road Permit No
                GridViewWarehouse.Columns[14].Visible = false; // Road Permit Date
            }
        }
    }
    private void DownloadPODDocument(string DocumentPath)
    {
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
    protected void DataSourceWarehouse_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

        if (Result == 0)
        {
            lblError.Text = "Delivery Detail Updated Successfully";
            lblError.CssClass = "success";
            GridViewWarehouse.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Please Check No Of Delivered Packages. Supply Count Exceed Available Packages!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }

    }

    #endregion

    #region Additional Field

    protected void Page_Init(object sender, EventArgs e)
    {
        int JobId = 0;
        if (Session["JobId"] != null)
        {
            JobId = Convert.ToInt32(Session["JobId"]);
        }
        SqlDataReader drFields = EXOperations.EX_GetJobAdditionalFields(JobId);
        if (drFields.HasRows)
        {
            while (drFields.Read())
            {
                int FieldId = Convert.ToInt32(drFields["FieldId"]);
                string FieldName = drFields["Fieldname"].ToString();
                FieldDataType FieldType = (FieldDataType)drFields["FieldDataType"];
                AddAdditionalField(FieldId, FieldType, FieldName);
            }
        }
    }

    private void AddAdditionalField(int FieldId, FieldDataType FieldType, string FieldName)
    {
        TableRow tr = new TableRow();

        // Field Name
        TableCell tdName = new TableCell();
        tdName.Text = FieldName;
        tdName.VerticalAlign = VerticalAlign.Top;
        tr.Cells.Add(tdName);

        // Field Type

        List<Control> UIControl = CreateCustomAttributeUI(FieldId, FieldType);

        TableCell tdUI = new TableCell();
        tdUI.VerticalAlign = VerticalAlign.Top;
        tdUI.ColumnSpan = 3;
        foreach (Control ctrl in UIControl)
        {
            tdUI.Controls.Add(ctrl);
        }

        tr.Cells.Add(tdUI);
        CustomUITable.Rows.Add(tr);
    }

    private List<Control> CreateCustomAttributeUI(int FieldId, FieldDataType DataTypeId)
    {
        List<Control> Ctrls = new List<Control>();

        switch (DataTypeId)
        {
            case FieldDataType.Alphanumeric:
                // use a  Text Box

                TextBox tbBox = new TextBox();
                tbBox.ID = FieldId.ToString(); //GetId(FieldId);
                tbBox.MaxLength = 200;
                Ctrls.Add(tbBox);
                break;
            case FieldDataType.Numeric:
                // user Numeric Text Box with validation
                TextBox tbBoxNum = new TextBox();
                tbBoxNum.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxNum.MaxLength = 20;
                Ctrls.Add(tbBoxNum);
                // Add a numeric CompareValidator
                Ctrls.Add(CreateDataTypeCheckCompareValidator(FieldId.ToString(), ValidationDataType.Double, "Invalid numeric value."));
                break;
            case FieldDataType.Date:
                // user Text Box with validation
                TextBox tbBoxDate = new TextBox();
                tbBoxDate.ID = FieldId.ToString(); //GetId(FieldId);
                Ctrls.Add(tbBoxDate);
                // Add Ajax Date Extender

                AjaxControlToolkit.CalendarExtender calDate = new CalendarExtender();
                calDate.ID = "Cal" + FieldId.ToString();
                calDate.TargetControlID = FieldId.ToString();
                calDate.Format = "dd/MM/yyyy";
                //  calDate.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                Ctrls.Add(calDate);
                // Add a Date CompareValidator
                Ctrls.Add(CreateDataTypeCheckCompareValidator(FieldId.ToString(), ValidationDataType.Date, "Invalid date value."));
                break;
            case FieldDataType.Percent:
                // use a  Text Box

                TextBox tbBoxPercnt = new TextBox();
                tbBoxPercnt.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxPercnt.MaxLength = 50;
                Ctrls.Add(tbBoxPercnt);
                break;
            case FieldDataType.Currency:
                // use a  Text Box

                TextBox tbBoxCurrency = new TextBox();
                tbBoxCurrency.ID = FieldId.ToString(); //GetId(FieldId);
                tbBoxCurrency.MaxLength = 50;
                Ctrls.Add(tbBoxCurrency);
                break;
            case FieldDataType.CheckBox:
                // use a  CheckBox

                CheckBox chkBox = new CheckBox();
                chkBox.Text = "YES";
                chkBox.ID = FieldId.ToString(); //GetId(FieldId);
                Ctrls.Add(chkBox);
                break;
        }

        return Ctrls;

    }

    private RequiredFieldValidator CreateRequiredFieldValidator(string DynamicFieldid, string ErrorMessage)
    {
        return CreateRequiredFieldValidator(DynamicFieldid, ErrorMessage, String.Empty);
    }

    private RequiredFieldValidator CreateRequiredFieldValidator(string DynamicFieldid, string ErrorMessage, string InitialValue)
    {
        RequiredFieldValidator rfv = new RequiredFieldValidator();
        rfv.ID = ("ReqVal_" + DynamicFieldid.ToString());
        rfv.ControlToValidate = DynamicFieldid.ToString();
        rfv.Display = ValidatorDisplay.Dynamic;
        rfv.ErrorMessage = ErrorMessage;
        rfv.InitialValue = InitialValue;
        return rfv;
    }

    private CompareValidator CreateDataTypeCheckCompareValidator(string DynamicFieldid, ValidationDataType DataType, string ErrorMessage)
    {
        CompareValidator cv = new CompareValidator();
        cv.ID = ("CompVal_" + DynamicFieldid);
        cv.ControlToValidate = DynamicFieldid;
        cv.Display = ValidatorDisplay.Dynamic;
        cv.Operator = ValidationCompareOperator.DataTypeCheck;
        cv.Type = DataType;
        cv.ErrorMessage = ErrorMessage;

        return cv;
    }

    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        // Find each custom attribute control

        Dictionary<int, SqlParameter> AttributeValues = new Dictionary<int, SqlParameter>();

        SqlDataReader drField = EXOperations.EX_GetJobAdditionalFields(Convert.ToInt32(Session["JobId"]));

        if (drField.HasRows)
        {
            while (drField.Read())
            {
                int FieledId = Convert.ToInt32(drField["FieldId"]);
                FieldDataType FieldType = (FieldDataType)drField["FieldDataType"];
                AttributeValues[FieledId] = GetValueFromCustomField(FieledId, FieldType);
            }
        }

        // Finally, add or update each custom attribute value
        int result = -123;

        foreach (int fieldid in AttributeValues.Keys)
        {
            if (AttributeValues[fieldid].Value.ToString().Trim() != "")
            {
                result = DBOperations.AddJobAdditionalFields(Convert.ToInt32(Session["JobId"]), fieldid, AttributeValues[fieldid].Value.ToString().ToUpper(), LoggedInUser.glUserId);
            }
        }

        if (result == 0)
        {
            lblError.Text = "Additional Field Added Successfully";
            lblError.CssClass = "success";
            GridViewJobField.DataBind();
        }
        else if (result == -123)
        {
            lblError.Text = "No Additional Field Found!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    protected SqlParameter GetValueFromCustomField(int FieldId, FieldDataType FieldType)
    {
        SqlParameter userInpurParam = new SqlParameter();

        // Find Control By FieldId

        Control ctrlId = CustomUITable.FindControl(FieldId.ToString());

        switch (FieldType)
        {
            case FieldDataType.Alphanumeric:
                TextBox tbBox = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBox.Text.Trim().ToUpper();
                break;
            case FieldDataType.Numeric:
                TextBox tbBoxNum = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxNum.Text.Trim();
                break;
            case FieldDataType.Date:
                TextBox tbBoxDate = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.Date;
                userInpurParam.Value = tbBoxDate.Text.Trim();

                break;
            case FieldDataType.Percent:
                // use a  Text Box

                TextBox tbBoxPercnt = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxPercnt.Text.Trim().ToUpper();
                break;
            case FieldDataType.Currency:
                // use a  Text Box

                TextBox tbBoxCurrency = (TextBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                userInpurParam.Value = tbBoxCurrency.Text.Trim().ToUpper();
                break;
            case FieldDataType.CheckBox:
                // use a  CheckBox

                CheckBox chkBox = (CheckBox)ctrlId;
                userInpurParam.DbType = DbType.String;
                if (chkBox.Checked)
                {
                    userInpurParam.Value = "YES";
                }
                else
                {
                    userInpurParam.Value = "NO";
                }
                break;
        }

        return userInpurParam;
    }

    #endregion

    #region Daily Activity

    protected void btnActivityPrint_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        string strJobRefNo = lblTitle.Text.Replace("Job Detail -", "");

        try
        {
            if (gvDailyJob.Rows.Count > 0)
            {
                DataSet dsActivity = EXOperations.EX_GetJobActivityDetail(JobId);

                if (dsActivity.Tables[0].Rows.Count > 0)
                {
                    int i = 0; // Auto increment Table Cell value
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=JobActivity-" + strJobRefNo + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:tt") + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    Font TextBoldformat = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                    Rectangle recPDF = new Rectangle(PageSize.A4);

                    // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                    // Set PDF Document size and Left,Right,Top and Bottom margin

                    Document pdfDoc = new Document(recPDF);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();

                    Paragraph ParaSpacing1 = new Paragraph();
                    ParaSpacing1.SpacingBefore = 5;

                    pdfDoc.Add(ParaSpacing1);

                    pdfDoc.Add(new Paragraph("Job Activity" + " - " + strJobRefNo, GridHeadingFont));

                    pdfDoc.Add(ParaSpacing1);

                    PdfPTable pdftable = new PdfPTable(5);
                    pdftable.TotalWidth = 525f;
                    pdftable.LockedWidth = true;

                    float[] widths = new float[] { 0.07f, 0.2f, 0.2f, 1f, 0.2f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                    // Set Table Spacing Before And After html text
                    // pdftable.SpacingBefore = 10f;

                    pdftable.SpacingAfter = 8f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    pdftable.AddCell(cellwithdata);

                    // Header: Desctiption
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("DATE", GridHeadingFont));
                    pdftable.AddCell(cellwithdata1);

                    // Header: Rate
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("STATUS", GridHeadingFont));

                    pdftable.AddCell(cellwithdata2);

                    // Header: Currency

                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("PROGRESS", GridHeadingFont));
                    pdftable.AddCell(cellwithdata3);

                    // Header: Amount
                    //PdfPCell cellwithdata4 = new PdfPCell(new Phrase("CUSTOMER", GridHeadingFont));
                    //cellwithdata4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //pdftable.AddCell(cellwithdata4);

                    // Header: Tax

                    PdfPCell cellwithdata5 = new PdfPCell(new Phrase("USER", GridHeadingFont));
                    pdftable.AddCell(cellwithdata5);

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell CellSl = new PdfPCell();
                    CellSl.Colspan = 1;
                    CellSl.UseVariableBorders = false;

                    // Data Cell: DATE

                    PdfPCell CellDate = new PdfPCell();
                    CellDate.UseVariableBorders = false;

                    // Data Cell: Status

                    PdfPCell CellStatus = new PdfPCell();
                    CellStatus.UseVariableBorders = false;

                    // Data Cell: Progress

                    PdfPCell CellProgress = new PdfPCell();
                    CellProgress.UseVariableBorders = false;

                    // Data Cell: Visibility

                    //PdfPCell CellVisible = new PdfPCell();
                    //CellVisible.UseVariableBorders = false;

                    //  Data Cell: User

                    PdfPCell CellUser = new PdfPCell();
                    CellUser.UseVariableBorders = false;

                    //  Generate Table Data For Job Activity

                    foreach (DataRow dr in dsActivity.Tables[0].Rows)
                    {
                        i = i + 1;
                        // pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                        // Add Cell Data To Table

                        // Serial number #

                        CellSl.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                        pdftable.AddCell(CellSl);

                        // Status Date

                        CellDate.Phrase = new Phrase(Convert.ToString(dsActivity.Tables[0].Rows[i - 1]["dtDate"]), TextFontformat);

                        pdftable.AddCell(CellDate);

                        // CellStatus

                        CellStatus.Phrase = new Phrase(Convert.ToString(dsActivity.Tables[0].Rows[i - 1]["StatusName"]), TextFontformat);

                        pdftable.AddCell(CellStatus);

                        // CellProgress

                        CellProgress.Phrase = new Phrase(Convert.ToString(dsActivity.Tables[0].Rows[i - 1]["DailyProgress"]), TextFontformat);
                        pdftable.AddCell(CellProgress);

                        // CellVisibility

                        /****************************************
                        if(Convert.ToBoolean(dsActivity.Tables[0].Rows[i - 1]["IsCustomerVisible"]))
                            CellVisible.Phrase = new Phrase("Yes", TextFontformat);
                        else
                            CellVisible.Phrase = new Phrase("No", TextFontformat);    

                        pdftable.AddCell(CellVisible);
                        ****************************************/
                        // CellUser

                        CellUser.Phrase = new Phrase(Convert.ToString(dsActivity.Tables[0].Rows[i - 1]["UserName"]), TextFontformat);
                        pdftable.AddCell(CellUser);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);


                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 10;//5

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("User   : " + LoggedInUser.glEmpName, TextFontformat));

                    pdfDoc.Add(ParaSpacing);

                    //  htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                lblError.Text = "There is no Job activity to print!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Print Functionality Requires Full Trust Level On Server!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnSaveActivity_Click(object sender, EventArgs e)
    {
        int Jobid = Convert.ToInt32(Session["JobId"]);

        int SummaryStatus = Convert.ToInt32(ddActivityStatus.SelectedValue);
        Boolean bVisibleToCustomer = Convert.ToBoolean(RDLActivityCustomer.SelectedValue);

        string strFilePath = "";

        if (SummaryStatus > 0 && txtActivityProgress.Text.Trim() != "")
        {
            int result = DBOperations.AddJobDailyActivity(Jobid, txtActivityProgress.Text.Trim(), strFilePath, SummaryStatus, bVisibleToCustomer, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Job Activity Added Successfully!";
                lblError.CssClass = "success";
                txtActivityProgress.Text = "";
                ddActivityStatus.SelectedIndex = 0;
                gvDailyJob.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Error!! Please Fill Required Job Activity Details!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnClearActivity_OnClick(object sender, EventArgs e)
    {
        ddActivityStatus.SelectedValue = "0";
        txtActivityProgress.Text = "";
    }

    protected void gvDailyJob_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvDailyJob_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "DailyProgress").ToString();
                HyperLink lnkMoreProgress = (HyperLink)e.Row.FindControl("lnkMoreProgress");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }
                if (strDocPath == "")
                {
                    LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkActDownload");
                    lnkDownload.Visible = false;
                }

                if (strProgressText.Length > 30)
                {
                    lnkMoreProgress.ToolTip = strProgressText;

                    //NameHyperLink.Attributes.Add("onmouseover", "ShowToolTip(event, " +
                    //"'" + Server.HtmlEncode(strProgressText) + "','Right');");

                    //NameHyperLink.Attributes.Add("onmouseout", "HideTooTip(event);");
                    //NameHyperLink.Attributes.Add("onmousemove", "MoveToolTip(event,'Right');");
                }
                else
                {
                    lnkMoreProgress.Visible = false;
                }
            }
        }

    }

    protected void gvDailyJob_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "updateactivity")
        {
            GridViewRow gvRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int ActivityId = Convert.ToInt32(gvDailyJob.DataKeys[gvRow.RowIndex].Value);

            DropDownList ddStatusId = (DropDownList)gvRow.FindControl("ddStatusId");

            TextBox txtProgressDetail = (TextBox)gvRow.FindControl("txtProgressDetail");
            RadioButtonList RDLCustomer = (RadioButtonList)gvRow.FindControl("RDLCustomer");

            string strProgress = txtProgressDetail.Text.Trim();
            int StatusId = Convert.ToInt32(ddStatusId.SelectedValue);

            Boolean bVisibleToCustomer = Convert.ToBoolean(RDLCustomer.SelectedValue);

            if (StatusId == 0)
            {
                lblError.Text = "Please Select Current Status!";
                lblError.CssClass = "errorMsg";
            }
            else if (strProgress != "")
            {
                int result = DBOperations.UpdateJobDailyActivity(ActivityId, StatusId, strProgress, bVisibleToCustomer, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Job Activity Updated Successfully!";
                    lblError.CssClass = "success";
                    gvDailyJob.EditIndex = -1;
                    gvDailyJob.DataBind();
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Enter Progress Detail!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    #region Print

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvDailyJob.Rows.Count > 0)
            {

                //StringBuilder sb = new StringBuilder();
                //sb.Append("<u><b>Job Number</b> &nbsp;<b>" +ViewState["JobNum"].ToString()+"</b></u>");

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=JobActivityDetails" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //StringWriter sw = new StringWriter(sb);
                StringWriter sw = new StringWriter();
                sw.Write("<br/>");

                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvDailyJob.AllowPaging = false;
                gvDailyJob.AllowSorting = false;
                gvDailyJob.DataBind();
                gvDailyJob.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                iTextSharp.text.Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("Job Number: " + ViewState["JobNum"].ToString()));

                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);

                // Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblError.Text = "There is no record to print!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Print Functionality Requires Full Trust Level On Server!";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

    #region Billing Status

    protected void btnSaveBillStatus_Click(object sender, EventArgs e)
    {
        int Jobid = Convert.ToInt32(Session["JobId"]);

        int BillStatusID = Convert.ToInt32(ddBillStatus.SelectedValue);

        if (BillStatusID > 0 && txtRemark.Text.Trim() != "")
        {
            int result = BillingOperation.AddBillingStatus(Jobid, BillStatusID, txtRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Billling Status Added Successfully!";
                lblError.CssClass = "success";
                txtRemark.Text = "";
                ddBillStatus.SelectedIndex = 0;
                gvBillingStatus.DataBind();
                gvDailyJob.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Error!! Please Fill Required Status Details!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnClearBillStatus_OnClick(object sender, EventArgs e)
    {
        ddBillStatus.SelectedValue = "0";
        txtRemark.Text = "";
    }

    protected void gvBillingStatus_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvBillingStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
            {
                bool IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                string strProgressText = (string)DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
                HyperLink lnkBillProgress = (HyperLink)e.Row.FindControl("lnkBillProgress");

                if (IsActive == true)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Current Status";
                }

                if (strProgressText.Length > 40)
                {
                    lnkBillProgress.ToolTip = strProgressText;
                }
                else
                {
                    lnkBillProgress.Visible = false;
                }
            }
        }
    }

    #endregion

    #endregion

    #region Document Download / Upload
    protected void FVJobHistory_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void FVJobHistory_prerender(Object Sender, EventArgs e)
    {
        GridView gv = (GridView)Sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            //gv.TopPagerRow.Visible = true;
        }
    }
    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewDocument_RowDataBound(Object Sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    LinkButton lnk = (LinkButton)e.Row.FindControl("lnkDownload");
        //    ScriptManager1.RegisterPostBackControl(GridViewDocument);
        //}

    }
    protected void GridViewDocument_prerender(Object Sender, EventArgs e)
    {
        GridView gv = (GridView)Sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            //gv.TopPagerRow.Visible = true;
        }
    }
    protected void GridViewPCADoc_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            //DownloadPCADocument(DocPath);
        }
    }
    protected void GridViewBillingAdvice_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            //DownloadBillAdviceDocument(DocPath);
        }
    }
    protected void GridViewBillingDept_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewBackOfficeDoc_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            // DownloadPCADocument(DocPath);
        }
    }
    protected void GrvShipmentDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void lnkChecklistDoc_Click(object sender, EventArgs e)
    {
        HiddenField hdnChecklistDocPath = (HiddenField)FVJobHistory.FindControl("hdnChecklistDocPath");
        string FilePath = hdnChecklistDocPath.Value.Trim();

        DownloadDocumentChecklist(FilePath);
    }
    protected void lnkChecklistDoc_JobDetail_Click(object sender, EventArgs e)
    {
        HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnChecklistDocPath2");
        string FilePath = hdnChecklistDocPath.Value.Trim();
        DownloadDocumentChecklist(FilePath);
    }
    protected void lnkEditChecklistDoc_JobDetail_Click(object sender, EventArgs e)
    {
        HiddenField hdnChecklistDocPath = (HiddenField)FVSBPrepare.FindControl("hdnEditChecklistDocPath");
        string FilePath = hdnChecklistDocPath.Value.Trim();
        DownloadDocumentChecklist(FilePath);
    }
    protected void lnkDwnloadExporterCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnExporterCopy");
        string FilePath = hdnExporterCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkDwnloadVGMCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
        string FilePath = hdnVGMCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkEditDwnloadExporterCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnExporterCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnEditExporterCopy");
        string FilePath = hdnExporterCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void lnkEditDwnloadVGMCopy_Click(object sender, EventArgs e)
    {
        HiddenField hdnVGMCopy = (HiddenField)FVShipmentGetIN.FindControl("hdnVGMCopy");
        string FilePath = hdnVGMCopy.Value.Trim();
        DownloadDocument2(FilePath);
    }
    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
            ServerPath = HttpContext.Current.Server.MapPath("~/UploadExportFiles/" + DocumentPath);
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
    protected void DownloadDocumentChecklist(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\ChecklistDoc\\" + DocumentPath);
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
    protected void DownloadDocument2(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

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
    protected void DownloadDocument3(string DocumentPath)
    {
        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

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
    protected void DownloadBillAdviceDocument(string DocumentPath)
    {

        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("~/UploadFiles/" + DocumentPath);
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
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        lblError.Visible = true;
        string strFilePath = "", filePath = "";
        int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);
        int JobId = Convert.ToInt32(Session["JobId"]);

        DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
        if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
            strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";

        if (JobId > 0)
        {
            if (DocumentId == 0)
            {
                lblError.Text = "Please Select Document Type!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (fuDocument.FileName.Trim() == "")
            {
                lblError.Text = "Please Browse The Document!";
                lblError.CssClass = "errorMsg";
                return;
            }

            filePath = UploadFiles(fuDocument, strFilePath);
            if (filePath != "")
            {
                int result_DocSaved = EXOperations.Ex_AddPreAlertDocs(filePath, DocumentId, JobId, LoggedInUser.glUserId);
                if (result_DocSaved == 0)
                {
                    lblError.Text = "Document uploaded successfully!";
                    lblError.CssClass = "success";
                    GridViewDocument.DataBind();
                    ddDocument.SelectedValue = "0";
                }
                else if (result_DocSaved == 2)
                {
                    lblError.Text = "Document with same type already exists!!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System Error. Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Select File!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            Response.Redirect("SBPreparation.aspx");
        }
    }
    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        FileName = FileName.Replace(",", "");
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

    #region PCA & PCA Document  
    protected void gvPCDDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void btnPCDUpload_Click(Object Sender, EventArgs e)
    {
        string a = fuPCDUpload1.FileName;
        int Result = -123;
        lblError.Visible = true;
        string strFilePath = "";

        int DocumentId = Convert.ToInt32(ddPCDDocument.SelectedValue);
        int PCDDocType = Convert.ToInt32(ddPCDDocFlowType.SelectedValue);

        bool IsCopy = false, IsOriginal = false;

        if (chkDuplicate.Items[0].Selected)
            IsOriginal = true;

        if (chkDuplicate.Items[1].Selected)
            IsCopy = true;

        int JobId = Convert.ToInt32(Session["JobId"]);

        int CustomerId = Convert.ToInt32(hdnCustId.Value);

        string strServerDirectory = hdnUploadPath.Value;

        if (JobId == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("JobTracking.aspx");
        }
        if (DocumentId == 0)
        {
            lblError.Text = "Please Select PCA Document Type!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (PCDDocType > 0)
        {
            if (IsCopy == false && IsOriginal == false)
            {
                lblError.Text = "Please Select atleast one check box for Original/Copy !";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        if (PCDDocType == 0 && fuPCDUpload1.FileName == "")
        {
            lblError.Text = "Please Select File To Upload Or Select Document For!";
            lblError.CssClass = "errorMsg";
            return;
        }

        strFilePath = UploadPCDDocument(fuPCDUpload1, strServerDirectory);

        Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblError.Text = "PCA Document uploaded successfully!";
            lblError.CssClass = "success";
            //  gvPCDDocument.DataBind();
            // GridViewBackOfficeDoc.DataBind();
            //  GridViewPCADoc.DataBind();
            // GridViewBillingAdvice.DataBind();
            //  GridViewBillingDept.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after some time.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "PCA Document not uploaded. Please try again!";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void lnkPODCopyDownoad_Click(object sender, EventArgs e)
    {
        LinkButton lnkPODDownload = (LinkButton)sender;

        string FilePath = lnkPODDownload.CommandArgument;

        DownloadDocument3(FilePath);
    }
    protected string UploadPCDDocument(FileUpload FU, string FilePath)
    {
        //string FileName = fuPCDUpload.FileName;
        string FileName = FU.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PCA_" + Session["JobId"].ToString() + "\\";

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
        if (fuPCDUpload1.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload1.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }
    protected void lnkPCALetter_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        //string dispatchDate = "", BoeDate = "";

        GeneratePDFDocument(JobId);
    }
    protected void GeneratePDFDocument(int JobId)
    {
        DataSet dsPCAPrint = EXOperations.EX_GetJobDetailforPCALetter(JobId);
        if (dsPCAPrint.Tables[0].Rows[0]["AddressLine1"] != DBNull.Value)
        {
            string BranchName = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["BranchName"]);
            string Customer = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Customer"]);
            string PlantName = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PlantName"]);
            string PlantPerson = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["ContactPerson"]);
            string PlantAddress1 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine1"]);
            string PlantAddress2 = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["AddressLine2"]);
            string PlantCity = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["City"]);
            string PlantPinCode = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PinCode"]);
            string PlantMobile = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["MobileNo"]);
            string PlantEmail = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["Email"]);

            string BSJobNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["JobRefNo"]);
            string CustRefNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["CustRefNo"]);
            string NoofPkg = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["NoOfPackages"]);
            string Mode = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["TransMode"]);
            string InvoiceDate = dsPCAPrint.Tables[0].Rows[0]["InvoiceDate"].ToString();
            string POL = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PortOfLoading"]);
            string POD = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["PortOfDischarge"]);
            string ShippingLineDate = dsPCAPrint.Tables[0].Rows[0]["ShippingLineDate"] != System.DBNull.Value ? Convert.ToDateTime(dsPCAPrint.Tables[0].Rows[0]["ShippingLineDate"]).ToShortDateString() : "";
            string SBNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["SBNo"]);
            string SBDate = dsPCAPrint.Tables[0].Rows[0]["SBDate"].ToString();
            string strInvoiceNo = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["InvoiceNo"]).Trim();
            //string strECDetail = Convert.ToString(dsPCAPrint.Tables[0].Rows[0]["SchemeNoDate"]).Trim();

            //strECDetail = strECDetail.TrimEnd(',');
            strInvoiceNo = strInvoiceNo.TrimEnd(',');
            string BsUser = LoggedInUser.glEmpName;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            //logo.Width = 80;
            //logo.Height = 40.00;
            //logo.SetAbsolutePosition();
            string date = DateTime.Today.ToShortDateString();

            DataSet dsPCADcoument = DBOperations.FillPCDDocumentByWorkFlow(JobId, Convert.ToInt32(EnumPCDDocType.PCACustomer));

            try
            {
                if (dsPCADcoument.Tables[0].Rows.Count > 0)
                {
                    // Generate PDF
                    int i = 0; // Auto Increment Table Cell For Serial number
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=PCA Letter-" + BSJobNo + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    Rectangle recPDF = new Rectangle(PageSize.A4);

                    // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
                    // Set PDF Document size and Left,Right,Top and Bottom margin

                    Document pdfDoc = new Document(recPDF);

                    //  Document pdfDoc = new Document(PageSize.A4, 30, 10, 10, 80);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);


                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                    Font FooterFontformat = FontFactory.GetFont("Arial", 7, Font.NORMAL);

                    logo.SetAbsolutePosition(380, 700);

                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    string contents = "";
                    contents = File.ReadAllText(Server.MapPath("../ExportCHA/PCACoverLetter.htm"));
                    contents = contents.Replace("[Today's Date]", date.ToString());
                    contents = contents.Replace("[CustomerName]", Customer);
                    contents = contents.Replace("[CustomerPCAAddress1]", PlantAddress1);
                    if (PlantAddress2 == String.Empty)
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantCity + " - " + PlantPinCode);
                        contents = contents.Replace("[CustomerPCACity]", String.Empty);
                    }
                    else
                    {
                        contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                        contents = contents.Replace("[CustomerPCACity]", PlantCity + " - " + PlantPinCode);
                    }
                    //contents = contents.Replace("[CustomerPCAAddress2]", PlantAddress2);
                    // contents = contents.Replace("[CustomerPCACity]",PlantCity +" - "+PlantPinCode);
                    contents = contents.Replace("[PCAContactPersonName]", PlantPerson);
                    contents = contents.Replace("[BSJOBNO]", BSJobNo);
                    contents = contents.Replace("[CustomerRefNo]", CustRefNo);
                    contents = contents.Replace("[ShippingLineDate]", ShippingLineDate);
                    contents = contents.Replace("[SB NO]", SBNo);
                    contents = contents.Replace("[SBDate]", SBDate);
                    contents = contents.Replace("[ShipmentInvoice]", strInvoiceNo);
                    contents = contents.Replace("[NoofPkg]", NoofPkg);
                    contents = contents.Replace("[Mode]", Mode);
                    contents = contents.Replace("[POL]", POL);
                    contents = contents.Replace("[POD]", POD);
                    contents = contents.Replace("[InvoiceDate]", InvoiceDate);
                    //contents = contents.Replace("[ECDetail]", strECDetail);

                    var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                    foreach (var htmlelement in parsedContent)
                        pdfDoc.Add(htmlelement as IElement);

                    PdfPTable pdftable = new PdfPTable(4);

                    pdftable.TotalWidth = 500f;
                    pdftable.LockedWidth = true;
                    float[] widths = new float[] { 0.1f, 1.5f, 0.2f, 0.2f };
                    pdftable.SetWidths(widths);
                    pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Set Table Spacing Before And After html text
                    pdftable.SpacingBefore = 10f;
                    pdftable.SpacingAfter = 10f;

                    // Create Table Column Header Cell with Text

                    // Header: Serial Number
                    PdfPCell cellwithdata = new PdfPCell(new Phrase("Sl", GridHeadingFont));
                    cellwithdata.Colspan = 1;
                    cellwithdata.BorderWidth = 1f;
                    //  cellwithdata.BackgroundColor = GrayColor.LIGHT_GRAY;

                    cellwithdata.HorizontalAlignment = 0;//left
                    pdftable.AddCell(cellwithdata);

                    // Header: Document Name
                    PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Document Type", GridHeadingFont));
                    cellwithdata1.Colspan = 1;
                    cellwithdata1.BorderWidth = 1f;
                    cellwithdata1.HorizontalAlignment = 0;
                    cellwithdata1.VerticalAlignment = 0;// Center
                    pdftable.AddCell(cellwithdata1);

                    // Header: Document Type - Original
                    PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Original", GridHeadingFont));
                    cellwithdata2.Colspan = 1;
                    cellwithdata2.BorderWidth = 1f;
                    cellwithdata2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cellwithdata2.VerticalAlignment = Element.ALIGN_RIGHT;// Center
                    pdftable.AddCell(cellwithdata2);

                    // Header: Document Type - Copy
                    PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Copy", GridHeadingFont));
                    cellwithdata3.Colspan = 1;
                    cellwithdata3.BorderWidth = 1f;
                    cellwithdata3.HorizontalAlignment = 0;
                    pdftable.AddCell(cellwithdata3);


                    // Tick Mark Sign Font Creation For Originall/Copy
                    Phrase CheckMarkPhrase = new Phrase();
                    Font zapfdingbats = new Font(Font.FontFamily.ZAPFDINGBATS, 10, 3);
                    CheckMarkPhrase.Add(new Chunk("\u0033", zapfdingbats)); //"\u0033" For Tick Mark"

                    // Data Cell: Serial Number - Auto Increment Cell

                    PdfPCell SrnoCell = new PdfPCell();
                    SrnoCell.Colspan = 1;
                    SrnoCell.HorizontalAlignment = 0;//1

                    // Data Cell: Document Name Cell
                    PdfPCell DocNameCell = new PdfPCell();
                    DocNameCell.Colspan = 1;

                    // Data Cell: Original PdfCell
                    PdfPCell OriginalCell = new PdfPCell();
                    OriginalCell.Colspan = 1;
                    OriginalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    OriginalCell.VerticalAlignment = Element.ALIGN_CENTER;

                    // Data Cell: Copy PdfCell
                    PdfPCell CopyCell = new PdfPCell();
                    CopyCell.Colspan = 1;
                    CopyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    CopyCell.VerticalAlignment = Element.ALIGN_CENTER;

                    // Generae Table Data from PCA Document Dataset

                    foreach (DataRow dr in dsPCADcoument.Tables[0].Rows)
                    {
                        i = i + 1;
                        //  pdftable.DefaultCell.FixedHeight = 10f;//for spacing b/w two cell

                        // Add Cell Data To Table

                        // Serial number #
                        SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);

                        pdftable.AddCell(SrnoCell);

                        // Document Name

                        DocNameCell.Phrase = new Phrase(Convert.ToString(dsPCADcoument.Tables[0].Rows[i - 1]["DocumentName"]), TextFontformat);

                        pdftable.AddCell(DocNameCell);

                        // Original Cell
                        if ((dsPCADcoument.Tables[0].Rows[i - 1]["IsOriginal"]).ToString().ToLower() == "yes")
                        {
                            OriginalCell.Phrase = CheckMarkPhrase; // Add Tick Mark Sign
                        }
                        else
                        {
                            OriginalCell.Phrase = new Phrase(); // Add Blank Text Cell
                        }

                        pdftable.AddCell(OriginalCell);

                        // Copy Cell
                        if ((dsPCADcoument.Tables[0].Rows[i - 1]["IsCopy"]).ToString().ToLower() == "yes")
                        {
                            CopyCell.Phrase = CheckMarkPhrase; // Add Tick Mark Sign
                        }
                        else
                        {
                            CopyCell.Phrase = new Phrase(); // Add Blank Text Cell
                        }

                        pdftable.AddCell(CopyCell);

                    }// END_ForEach

                    pdfDoc.Add(pdftable);

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 10;

                    pdfDoc.Add(new Paragraph("    Kindly acknowledge the receipt of the above documents.", TextFontformat));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    Authorised Signatory", GridHeadingFont));

                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    User      : " + BsUser, TextFontformat));
                    pdfDoc.Add(new Paragraph("    Branch  : " + BranchName, TextFontformat));


                    // Footer Image Commented
                    // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterPdf.png"));

                    // footer.SetAbsolutePosition(30, 0);
                    // pdfDoc.Add(footer);
                    //// pdfwriter.PageEvent = new PDFFooter();

                    string footerText = "   Corporate Office: Plot No 2, Behind Excom House, Saki Vihar Road, Sakinaka, Andheri East.\n" +
                        "   Mumbai 400072. India. Tel.: +91 22 66485600. Email: info@babajishivram.com, WEBSITE: WWW.BabajiShivram.com\n" +
                        "   BRANCHES: NHAVA SHEVA, CHENNAI, KOLKATA, VISAKHAPATNAM, KAKINADA, BANGLORE, GOA\n" +
                        "   REGISTERED OFFICE: 407, REX CHAMBERS, 4TH FLOOR, WALCHAND HIRACHAND MARG, BALLARD ESTATE MUMBAI - 400038";

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph(footerText, FooterFontformat));

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.Write(pdfDoc);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }//END_IF

                else
                {
                    lblMessage.Text = "Please First Update Document List!";
                    lblMessage.CssClass = "errorMsg";
                }

            }//END_Try

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblMessage.Text = "PCA Address Not Found For Plant Name " + dsPCAPrint.Tables[0].Rows[0]["PlantName"].ToString();
            lblMessage.CssClass = "errorMsg";
        }
    }
    protected void PCDDocumentSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            lblError.Text = "System Error! Please contact system administrator. Event Name:PCDDocumentSqlDataSource_Selected.";
            lblError.CssClass = "errorMsg";

            e.ExceptionHandled = true;
        }
    }

    #endregion

    #region Billing
    protected void gvbillingscrutiny_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string freightjobno = e.Row.Cells[8].Text;

        //    if (freightjobno != "&nbsp;")
        //    {
        //        lblfreight.Text = "<center><b>This Job Billed in Freight JobNo. " + freightjobno + "</b></center>";
        //        lblfreight.CssClass = "success";
        //        DraftInvoice.Visible = false;
        //        DraftCheck.Visible = false;
        //        FinalInvoiceCheck.Visible = false;
        //        FinalInvoiceTyping.Visible = false;
        //        Billdispatch.Visible = false;
        //        BillRejection.Visible = false;
        //    }


        //}
    }

    protected void gvDraftInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //string Consolidatedjobno = "";

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Consolidatedjobno = e.Row.Cells[8].Text;

        //    if (Consolidatedjobno != "&nbsp;")
        //    {


        //        lblConsolidated.Text = "<center><b>This job Consolidated With JobNo. " + Consolidatedjobno + "</b></center> ";
        //        lblConsolidated.CssClass = "success";
        //        DraftCheck.Visible = false;
        //        FinalInvoiceCheck.Visible = false;
        //        FinalInvoiceTyping.Visible = false;
        //        Billdispatch.Visible = false;
        //        BillRejection.Visible = false;
        //    }
        //}


    }

    protected void gvDraftInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "draftinvoicenext")
        {
            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = BillingOperation.EX_DraftInvoicejobmovetoDraftcheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Draft Check!.";
                lblError.CssClass = "success";
                gvDraftInvoice.DataBind();
                gvDraftcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Draft Invoice!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblError.Text = "Draft Invoice Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvFinaltyping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "finaltypingnext")
        {
            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = BillingOperation.EX_FinalTypingjobmovetoFinalcheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Final Draft Check!.";
                lblError.CssClass = "success";
                gvFinaltyping.DataBind();
                gvfinalcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Final typing!";
                lblError.CssClass = "errorMsg";
            }

            else if (result == 4)
            {
                lblError.Text = "Final typing Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    #endregion

    #region Billing Expense
    protected void gvjobexpenseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int64 lblDEBITAMT = Convert.ToInt64(e.Row.Cells[6].Text);
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + Convert.ToInt64(lblDEBITAMT);

            //Int64 lblCREDITAMT = Convert.ToInt64(e.Row.Cells[7].Text);
            //ViewState["lblCREDITAMT"] = Convert.ToInt64(ViewState["lblCREDITAMT"]) + Convert.ToInt64(lblCREDITAMT);
            //Int64 lblAMOUNT = Convert.ToInt64(e.Row.Cells[8].Text);
            //ViewState["lblAMOUNT"] = Convert.ToInt64(ViewState["lblAMOUNT"]) + Convert.ToInt64(lblAMOUNT);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[6].Text = ViewState["lblDEBITAMT"].ToString();
            //e.Row.Cells[7].Text = ViewState["lblCREDITAMT"].ToString();
            //e.Row.Cells[8].Text = ViewState["lblAMOUNT"].ToString();
        }
    }

    #endregion 

    #region CONTAINER DETAILS EVENTS

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        int JobId = Convert.ToInt32(Session["JobId"]);
        int ContainerSize, ContainerType;

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

        if (ContainerType == 1) //FCL
        {
            if (ContainerSize == 0)
            {
                lblError.Text = "Please Select FCL Container Size!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }
        else if (ContainerType == 2) //LCL
        {
            ddContainerSize.SelectedValue = "0";
            ContainerSize = 0;
        }

        if (ContainerNo != "")
        {
            int result = EXOperations.EX_AddContainerDetail(JobId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId, txtSealNo.Text.Trim());

            if (result == 0)
            {
                lblError.Text = "Container No " + ContainerNo + " Added successfully!";
                lblError.CssClass = "success";
                gvContainer.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Container No " + ContainerNo + " Already Added!";
                lblError.CssClass = "warning";
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please Enter Container No.!";
        }
    }

    protected void btnCancelContainer_Click(object sender, EventArgs e)
    {
        txtContainerNo.Text = "";
        ddContainerType.SelectedValue = "1";
        ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
        gvContainer.DataBind();
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

    }

    protected void ddEditContainerType_SelectedIndexChanged(object sender, EventArgs e)
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

    }

    protected void gvContainer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.Visible = true;
        int ContainerSize, ContainerType;
        string ContainerNo = "";

        int JobId = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Values[0].ToString());
        TextBox txtContainerNo = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtEditContainerNo");
        DropDownList ddlContainerType = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerType");
        DropDownList ddlContainerSize = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerSize");
        TextBox txtSealNo = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtSealNo_Edit");

        ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddlContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddlContainerType.SelectedValue);

        if (ContainerType == 1) //FCL
        {
            if (ContainerSize == 0)
            {
                lblError.Text = "Please Select FCL Container Size!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
                return;
            }
        }
        if (ContainerType == 2) //LCL
        {
            ddlContainerSize.SelectedValue = "0";
            ContainerSize = 0;
        }
        if (ContainerNo != "")
        {
            int result = EXOperations.EX_AddContainerDetail(JobId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId, txtSealNo.Text.Trim());
            if (result == 0)
            {
                lblError.Text = "Container Detail Updated Successfully!";
                lblError.CssClass = "success";
                gvContainer.EditIndex = -1;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            e.Cancel = true;
        }
        else
        {
            lblError.Text = "Please Enter Container No!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvContainer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;

        int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Values[1].ToString());
        int result = EXOperations.EX_DeleteContainerDetail(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Container Deleted Successfully!";
            lblError.CssClass = "success";
            e.Cancel = true;
            gvContainer.DataBind();
        }
        else if (result == 2)
        {
            lblError.Text = "Delivery Completed. Can not delete container details.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
            gvContainer.DataBind();
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

    #region JOB EXPENSE DETAILS

    #region Print
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void btnPrintJobExpense_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvExpenseDetails.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=JobExpenseDetail-" + ViewState["JobNum"].ToString() + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                sw.Write("<br/>");
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvExpenseDetails.AllowPaging = false;
                //gvExpenseDetails.Columns[1].Visible = false; // Edit Button
                gvExpenseDetails.DataBind();
                gvExpenseDetails.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 0, 0, 15, 15);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("Expense Job Number: " + ViewState["JobNum"].ToString() + "  " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")));
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblError.Text = "There is no expense record to print!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Print Functionality Requires Full Trust Level On Server!";
            lblError.CssClass = "errorMsg";
        }

    }

    #endregion

    #endregion

    #region INVOICE DETAILS EVENTS

    protected void btnAddInvoice_Click(object sender, EventArgs e)
    {
        int ShipmentTermsId = 0;
        DateTime dtInvoiceDate = DateTime.MinValue;
        DateTime dtLicenseDate = DateTime.MinValue;

        if (txtInvoiceDate.Text.Trim().ToString() != "")
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        if (txtLicenseDate.Text.Trim().ToString() != "")
            dtLicenseDate = Commonfunctions.CDateTime(txtLicenseDate.Text.Trim());
        if (ddlShipmentTerm.SelectedValue != "0")
            ShipmentTermsId = Convert.ToInt32(ddlShipmentTerm.SelectedValue);

        if (txtInvoiceNo.Text.Trim() != "" && dtInvoiceDate != DateTime.MinValue)
        {
            int result = EXOperations.Ex_AddInvoiceDetail(Convert.ToInt32(Session["JobId"]), 0, txtInvoiceNo.Text.Trim(), dtInvoiceDate, txtInvoiceValue.Text.Trim(), ShipmentTermsId,
                txtDBKAmount.Text.Trim(), txtLicenseNo.Text.Trim(), dtLicenseDate, txtFreightAmount.Text.Trim(), txtInsuranceAmount.Text.Trim(), LoggedInUser.glUserId,
                txtInvoiceCurrency.Text.Trim());

            if (result == 0)
            {
                lblError.Text = "Successfully added invoice details.";
                lblError.CssClass = "success";
                gvInvoiceDetail.DataBind();
                //gvContainer.DataBind();
                btnCancelInvoice_Click(null, EventArgs.Empty);
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }

        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = "Please enter mandatory fields!!";
        }

    }

    protected void btnCancelInvoice_Click(object sender, EventArgs e)
    {
        txtInvoiceNo.Text = "";
        txtInvoiceDate.Text = "";
        txtInvoiceValue.Text = "";
        ddlShipmentTerm.SelectedValue = "0";
        txtDBKAmount.Text = "";
        txtLicenseDate.Text = "";
        txtLicenseNo.Text = "";
        txtFreightAmount.Text = "";
        txtInsuranceAmount.Text = "";
    }

    protected void gvInvoiceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int JobId = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[0].ToString());
        int Lid = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[1].ToString());
        int ShipmentTermsId = 0;
        TextBox txtInvoiceNo = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceNo") as TextBox;
        TextBox txtInvoiceDate = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceDate") as TextBox;
        TextBox txtInvoiceValue = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceValue") as TextBox;
        DropDownList ddlShipmentTerm = gvInvoiceDetail.Rows[e.RowIndex].FindControl("ddlShipmentTerm") as DropDownList;
        TextBox txtDBKAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtDBKAmount") as TextBox;
        TextBox txtLicenseNo = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtLicenseNo") as TextBox;
        TextBox txtLicenseDate = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtLicenseDate") as TextBox;
        TextBox txtFreightAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtFreightAmount") as TextBox;
        TextBox txtInsuranceAmount = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInsuranceAmount") as TextBox;
        TextBox txtInvoiceCurrency = gvInvoiceDetail.Rows[e.RowIndex].FindControl("txtInvoiceCurrency") as TextBox;

        DateTime dtInvoiceDate = DateTime.MinValue;
        DateTime dtLicenseDate = DateTime.MinValue;
        if (txtInvoiceDate.Text.Trim().ToString() != "")
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        if (txtLicenseDate.Text.Trim().ToString() != "")
            dtLicenseDate = Commonfunctions.CDateTime(txtLicenseDate.Text.Trim());
        if (ddlShipmentTerm.SelectedValue != "0")
            ShipmentTermsId = Convert.ToInt32(ddlShipmentTerm.SelectedValue);

        if (txtInvoiceNo.Text.Trim() != "" && dtInvoiceDate != DateTime.MinValue)
        {
            int result = EXOperations.Ex_AddInvoiceDetail(JobId, Lid, txtInvoiceNo.Text.Trim(), dtInvoiceDate, txtInvoiceValue.Text.Trim(), ShipmentTermsId,
                txtDBKAmount.Text.Trim(), txtLicenseNo.Text.Trim(), dtLicenseDate, txtFreightAmount.Text.Trim(), txtInsuranceAmount.Text.Trim(), LoggedInUser.glUserId,
                txtInvoiceCurrency.Text.Trim());

            if (result == 0)
            {
                lblError.Text = "Successfully updated invoice details.";
                lblError.CssClass = "success";
                gvInvoiceDetail.EditIndex = -1;
                btnCancelInvoice_Click(null, EventArgs.Empty);
                //gvInvoiceDetail.DataBind();
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }

        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = "Please enter mandatory fields!!";
        }
        e.Cancel = true;
    }

    protected void gvInvoiceDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;
        int lid = Convert.ToInt32(gvInvoiceDetail.DataKeys[e.RowIndex].Values[1].ToString());
        int result = EXOperations.EX_DeleteInvoiceDetail(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            e.Cancel = true;
            lblError.Text = "Invoice Detail Deleted Successfully!";
            lblError.CssClass = "success";
            gvInvoiceDetail.DataBind();
            btnCancelInvoice_Click(null, EventArgs.Empty);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvInvoiceDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvInvoiceDetail.EditIndex = e.NewEditIndex;
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvInvoiceDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvInvoiceDetail.EditIndex = -1;
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvInvoiceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvInvoiceDetail.PageIndex = e.NewPageIndex;
        gvInvoiceDetail.DataBind();
    }

    #endregion

    #region Billing Repository
    protected void btnBillingRepoDocCheck_Click(object sender, EventArgs e)
    {
        BindBillingDocFromRepository(hdnJobRefNo.Value);
    }
    private void BindBillingDocFromRepository(string JobRefNo)
    {
        // CB00016-CNAI-18-19_20180521022448.pdf

        fsRepository.Visible = true;

        string searchPattern = JobRefNo; // JobRefNo.Remove(0, 2);  //  Job XML File

        searchPattern = searchPattern.Replace("/", "-");

        searchPattern = searchPattern + "*";

        //lblBillMsg.Text = searchPattern;
        //  string RemoteServerPath = @"\\192.168.6.116\f$\Babaji-shares\BS-Scan Document\";

        string RemoteServerPath = @"\\\\babaji-storage\\BS-Scan Document\";

        try
        {
            DirectoryInfo di = new DirectoryInfo(RemoteServerPath);

            // Get List of Billing Document

            var fileList = di.GetFiles(searchPattern, SearchOption.AllDirectories);

            gvBillingRepository.DataSource = fileList;

            gvBillingRepository.DataBind();

            if (fileList.Length == 0)
            {
                lblError.Text = "Document Not Found in Billing Repository!";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvBillingRepository_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "downloadrepo")
        {
            string DocPath = e.CommandArgument.ToString();

            DownloadBillingRepo(DocPath);
        }

    }

    private void DownloadBillingRepo(string DocumentPath)
    {
        String BIllingServerPath = DocumentPath;

        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, BIllingServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void GetJobCancelDetail()
    {
        string dtLEODate = "", Remark = "";
        DataView dv = (DataView)DataSourceJobDetail.Select(DataSourceSelectArguments.Empty);
        DataTable dtCustomProcess = new DataTable();
        dtCustomProcess = dv.ToTable();
        if (dtCustomProcess.Rows.Count > 0)
        {
            foreach (DataRow row in dtCustomProcess.Rows)
            {
                dtLEODate = row["LEODate"].ToString();
                Remark = row["Remark"].ToString();
            }
        }

        if (dtLEODate == "")
        {
            //FVJobDetail.FindControl("trCancel").Visible = true;
            //FVJobDetail.FindControl("btnCancel").Visible = true;
        }
        else if (Remark == "")
        {
            //FVJobDetail.FindControl("trCancel").Visible = true;
            lblError.Text = "Can Not Allow to cancel job!!!!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            //FVJobDetail.FindControl("trJobCancel").Visible = false;
            lblError.Text = "Can Not Allow to cancel job!!!!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void ddlCancelAllow_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCancelAllow = (DropDownList)FVJobDetail.FindControl("ddlCancelAllow");
        if (ddlCancelAllow.SelectedValue == "1")
        {
            //GetJobCancelDetail();

            string dtLEODate = "", Remark = "";
            DataView dv = (DataView)DataSourceJobDetail.Select(DataSourceSelectArguments.Empty);
            DataTable dtCustomProcess = new DataTable();
            dtCustomProcess = dv.ToTable();
            if (dtCustomProcess.Rows.Count > 0)
            {
                foreach (DataRow row in dtCustomProcess.Rows)
                {
                    dtLEODate = row["LEODate"].ToString();
                    Remark = row["Remark"].ToString();
                }
            }

            if (dtLEODate == "")
            {
                FVJobDetail.FindControl("trCancel").Visible = true;
                //FVJobDetail.FindControl("btnCancel").Visible = true;
                if (Remark != "")
                {
                    FVJobDetail.FindControl("trCancel").Visible = false;
                    lblError.Text = "Job has cancelled!!!!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                FVJobDetail.FindControl("trCancel").Visible = false;
                ddlCancelAllow.SelectedValue = "2";
                lblError.Text = "Can Not Allow to cancel job!!!!";
                lblError.CssClass = "errorMsg";
            }

            if (Remark != "")
            {
                FVJobDetail.FindControl("trCancel").Visible = false;
            }
            //FVJobDetail.FindControl("trCancel").Visible = true;
        }
        else
        {
            FVJobDetail.FindControl("trCancel").Visible = false;
        }
    }

    #region billing instruction
    protected void Get_BillingInstruction(int JobId)
    {
        int strJobId = JobId;
        DataTable dtBillInstruction = new DataTable();
        dtBillInstruction = DBOperations.Get_BillingInstructionDetail(strJobId);
        if (dtBillInstruction.Rows.Count > 0)
        {
            //dvBillInstruction.Visible = false;
            dvResult.Visible = true;
            // btnSaveInstruction.Visible = false;

            foreach (DataRow rw in dtBillInstruction.Rows)
            {
                //lblAgencyApply.Text = rw["AlliedAgencyApply"].ToString();
                //lblRefNo.Text = rw["JobRefNo"].ToString();
                if (rw["AlliedAgencyService"].ToString() == "") { lblAlliedAgencyService.Text = "-"; }
                else
                {
                    string args = rw["AlliedAgencyService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblAlliedAgencyService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                //lblAlliedAgencyService.Text = rw["AlliedAgencyService"].ToString(); }
                if (rw["AlliedAgencyRemark"].ToString() == "") { lblAlliedAgencyRemark.Text = "-"; }
                else { lblAlliedAgencyRemark.Text = rw["AlliedAgencyRemark"].ToString(); };

                if (rw["OtherService"].ToString() == "") { lblOtherService.Text = "-"; }
                else
                {
                    string args = rw["OtherService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblOtherService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                if (rw["OtherServiceRemark"].ToString() == "") { lblOtherServiceRemark.Text = "-"; }
                else { lblOtherServiceRemark.Text = rw["OtherServiceRemark"].ToString(); }
                if (rw["InstructionCopy"].ToString() == "") { lnkInstructionCopy.Text = "-"; }
                else { lnkInstructionCopy.Text = rw["InstructionCopy"].ToString(); }
                if (rw["InstructionCopy1"].ToString() == "") { lnkInstructionCopy1.Text = "-"; }
                else { lnkInstructionCopy1.Text = rw["InstructionCopy1"].ToString(); }
                if (rw["InstructionCopy2"].ToString() == "") { lnkInstructionCopy2.Text = "-"; }
                else { lnkInstructionCopy2.Text = rw["InstructionCopy2"].ToString(); }
                if (rw["InstructionCopy3"].ToString() == "") { lnkInstructionCopy3.Text = "-"; }
                else { lnkInstructionCopy3.Text = rw["InstructionCopy3"].ToString(); }
                if (rw["Instruction"].ToString() == "") { lblInstruction.Text = "-"; }
                else { lblInstruction.Text = rw["Instruction"].ToString(); }
                if (rw["Instruction1"].ToString() == "") { lblInstruction1.Text = "-"; }
                else { lblInstruction1.Text = rw["Instruction1"].ToString(); }
                if (rw["Instruction2"].ToString() == "") { lblInstruction2.Text = "-"; }
                else { lblInstruction2.Text = rw["Instruction2"].ToString(); }
                if (rw["Instruction3"].ToString() == "") { lblInstruction3.Text = "-"; }
                else { lblInstruction3.Text = rw["Instruction3"].ToString(); }
                lblUserDate.Text = rw["Userdate"].ToString();
                lblUserId.Text = rw["sName"].ToString();

                lblReadBy1.Text = rw["READBY"].ToString() + Environment.NewLine + rw["READDATE"].ToString();
                lblChargeBy1.Text = rw["CHARGEBY"].ToString() + "\n" + rw["ChargeDATE"].ToString();
                lblReadBy2.Text = rw["READBY1"].ToString() + "\n" + rw["READDATE1"].ToString();
                lblChargeBy2.Text = rw["CHARGEBY1"].ToString() + "\n" + rw["ChargeDATE1"].ToString();
                lblReadBy3.Text = rw["READBY2"].ToString() + "\n" + rw["READDATE2"].ToString();
                lblChargeBy3.Text = rw["CHARGEBY2"].ToString() + "\n" + rw["ChargeDATE2"].ToString();
                lblReadBy4.Text = rw["READBY3"].ToString() + "\n" + rw["READDATE3"].ToString();
                lblChargeBy4.Text = rw["CHARGEBY3"].ToString() + "\n" + rw["ChargeDATE3"].ToString();
                lblAlliedReadBy.Text = rw["READBY4"].ToString() + "\n" + rw["READDATE4"].ToString();
                lblAlliedChargeBy.Text = rw["CHARGEBY4"].ToString() + "\n" + rw["ChargeDATE4"].ToString();
                lblOtherServiceReadBy.Text = rw["READBY5"].ToString() + "\n" + rw["READDATE5"].ToString();
                lblOtherServiceChargeBy.Text = rw["CHARGEBY5"].ToString() + "\n" + rw["ChargeDATE5"].ToString();
            }
            // ModalPopupInstruction.Show();
        }
        else
        {
            dvResult.Visible = false;
        }

    }

    protected string FindFileName(string FilePath)
    {
        string[] args = FilePath.Split('\\');

        return args[2];
    }
    protected void lnkInstructionCopy_Click(object sender, EventArgs e)
    {
        DownloadDocument3(hdnInstructionCopy.Value);
    }

    protected void lnkInstructionCopy1_Click(object sender, EventArgs e)
    {
        DownloadDocument3(hdnInstructionCopy1.Value);
    }

    protected void lnkInstructionCopy2_Click(object sender, EventArgs e)
    {
        DownloadDocument3(hdnInstructionCopy2.Value);
    }

    protected void lnkInstructionCopy3_Click(object sender, EventArgs e)
    {
        DownloadDocument3(hdnInstructionCopy3.Value);
    }

    #endregion

    #region E-bill dispatch
    protected void gvBillDispatchDocDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void GridViewMailDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string Msg = e.CommandArgument.ToString();
            divPreviewEmailBillDispatch.InnerHtml = Msg;
            divPreviewEmailBillDispatch.Visible = true;
            ModalPopupExtender1.Show();
        }
    }

    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    protected void GetEBillDetail()
    {
        DataView dvDoc = (DataView)SqlDataSourceBillDispatchDoc.Select(DataSourceSelectArguments.Empty);
        DataTable dtBillDoc = dvDoc.Table;
        if (!dtBillDoc.Columns.Contains("NewColumn"))
        {
            DataColumn newCol = new DataColumn("NewColumn", typeof(string));
            newCol.AllowDBNull = true;
            dtBillDoc.Columns.Add(newCol);
        }

        int j = 0;
        foreach (DataRow rows in dtBillDoc.Rows)
        {
            string DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtBillDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            j++;
        }
        gvBillDispatchDocDetail.DataSource = dtBillDoc;
        if (dtBillDoc.Columns.Contains("NewColumn"))
        {
            if (gvBillDispatchDocDetail.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtBillDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvBillDispatchDocDetail.Columns.Add(test);
            }
        }
        gvBillDispatchDocDetail.DataBind();
    }

    protected void btnMailResend_Click(object sender, EventArgs e)
    {
        GenerateEbillEmailDraft();
        //divResendEmail.Visible = true;
        ModalPopupExtender1.Show();
    }

    private void GenerateEbillEmailDraft()
    {
        int JobId = Convert.ToInt32(Session["EnqId"]);

        string MessageBody = "";
        string strJobRefNo = "", strCustName = "", strConsigneeName = "", strCustRefNo = "", strToMail = "", strJobType = "";
        int strAgencyCnt = 0, strRIMCnt = 0, strColSpan = 0, strCols = 0;
        string args; string[] args1; int tot; int AmtTot = 0;
        DataView dv = DBOperations.GetUserDetail(Convert.ToString(LoggedInUser.glUserId));
        DataTable dt = dv.ToTable(true, "sEmail");
        DataSet dsAlertDetail = DBOperations.GetBillDispatchDetail(JobId, JobRefNO);

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strCustName = dsAlertDetail.Tables[0].Rows[0]["Customer"].ToString();
            strConsigneeName = dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strJobRefNo = dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            strCustRefNo = dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            strJobType = dsAlertDetail.Tables[0].Rows[0]["JobType"].ToString();
            txtSubject.Text = "E-Bill Dispatch/Job No :- " + strJobRefNo + " /Customer Reference No :- " + strCustRefNo + "";
            strAgencyCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntAgency"].ToString());
            strRIMCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntRIM"].ToString());
            strToMail = dsAlertDetail.Tables[0].Rows[0]["Email"].ToString();
            txtMailTo.Text = strToMail;
            //txtMailTo.Text = "developer1@babajishivram.com, developer2@babajishivram.com";
            txtMailCC.Text = dt.Rows[0]["sEmail"].ToString() + "," + "query.billing@babajishivram.com";
        }
        else
        {
            lblError.Text = "Booking Details Not Found! Please check details.";
            lblError.CssClass = "errorMsg";
            return;
        }

        //////////////////////////////////////////////////////////////////////////////////
        if (strAgencyCnt > strRIMCnt)
        {
            strCols = strAgencyCnt;
        }
        else
        {
            strCols = strRIMCnt;
        }
        StringBuilder strStyle = new StringBuilder();
        strStyle = strStyle.Append("<html><body style='height:100%; width:100%; font-family:Arial; font-style:normal; font-size:9pt; color:#000;");

        // body header
        strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
        strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");

        strStyle = strStyle.Append(@"<tr><td>" + "Dear Sir / Madam, " + "<br />");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Kindly find the attached E-Invoices of Subject Shipment and details are as below. ");
        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td>" + "Customer Name :- " + strCustName);
        strStyle = strStyle.Append(@"</td></tr>");
        if (strJobType == "1")
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Consignee  Name :- " + strConsigneeName + "<br />");
        }
        else
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Shipper  Name :- " + strConsigneeName + "<br />");
        }

        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
        strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
        strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='50%'><colgroup><col width='40%' /><col width='35%' /><col width='30%' /><col width='30%' /></colgroup>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Job Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString() + "</td></tr>");
        strStyle = strStyle.Append(@"<tr><td style='border: 1px solid #ccc; background-color:#99CCFF'>Cust Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString() + "</td></tr>");

        ///Agency Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'  colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        tot = 0; AmtTot = 0;

        //RIM Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        AmtTot = 0; tot = 0;

        strStyle = strStyle.Append(@"</table></p></div></div></td>");

        // body footer
        strStyle = strStyle.Append(@"<tr><td>" + "Any billing related query or issue, kindly drop an e-mail to query.billing@babajishivram.com" + "<br/><br/>");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Thanks & Regards");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Babaji Shivram Clearing And Carriers Pvt Ltd");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"</table></td></tr>");
        strStyle = strStyle.Append(@"</center></body></html>");

        MessageBody = strStyle.ToString();

        /////////////////////////////////////////////////////////////////////////////////////
        divPreviewEmailBillDispatch.InnerHtml = MessageBody;

        DataTable dtDoc = DBOperations.GetBillDoc(JobId);
        DataColumn newCol = new DataColumn("NewColumn", typeof(string));
        newCol.AllowDBNull = true;
        dtDoc.Columns.Add(newCol);
        int j = 0;
        string DocPath = "";
        foreach (DataRow rows in dtDoc.Rows)
        {
            DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            //FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            //j++;
        }
        gvDocAttach.DataSource = dtDoc;
        if (dtDoc.Columns.Contains("NewColumn"))
        {
            if (gvDocAttach.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvDocAttach.Columns.Add(test);
            }
        }
        gvDocAttach.DataBind();

        ModalPopupExtender1.Show();
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        //lblStatus.Text = "Processing...";
        //lblStatus.Visible = true;

        if (txtMailTo.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Customer Email & Subject!";
            lblError.CssClass = "errorMsg";
            ModalPopupExtender1.Hide();
        }
        else
        {

            // Send Email
            bool bEMailSucess = SendPreAlertEmail();
            //bool bEMailSucess = true;
            // Update PreAlert Email Sent Status and Customer Email 

            if (bEMailSucess == true)
            {
                int Result = DBOperations.AddJobNotofication(JobId, 1, 14, txtMailTo.Text, txtMailCC.Text, txtSubject.Text, divPreviewEmailBillDispatch.InnerHtml, "0", LoggedInUser.glUserId);
                ModalPopupExtender1.Hide();
                //lblStatus.Text = "";

                if (Result == 0)
                {
                    lblError.Text = "Customer Email Sent Successfully!";
                    lblError.CssClass = "success";
                    //dvMailSend.Visible = false;
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "PreAlert Email Already Sent!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void ImageClose_Click(object sender, ImageClickEventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    private bool SendPreAlertEmail()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtMailTo.Text.Trim();
        strCCEmail = txtMailCC.Text.Trim();
        strSubject = txtSubject.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", ",").Trim();
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();

        bool bEmailSucces = false;

        if (strCustomerEmail == "" || strSubject == "")
        {
            //lblPopMessageEmail.Text = "Please Enter Customer Email!";
            //       lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            MessageBody = divPreviewEmailBillDispatch.InnerHtml;

            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvDocAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }

    #region Vendor Payment Request
    protected void btnAllowFundRequest_Click(object sender, EventArgs e)
    {
        if (rblFundRequest.SelectedIndex == -1)
        {
            lblMessage.Text = "Please Check atleast One Option for Fund Request!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            int JobId = Convert.ToInt32(Session["JobId"]);
            bool isAllowed = false;

            if (rblFundRequest.SelectedValue == "1")
            {
                isAllowed = true;
            }

            int Result = DBOperations.UpdateJobFundRequestStatus(JobId, isAllowed, 5, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblMessage.Text = "Fund Request Status Updated Successfully!";
                lblMessage.CssClass = "success";

                JobDetailMS(JobId);
            }
            else if (Result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime";
                lblMessage.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblMessage.Text = "Job Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
    #endregion
    #endregion

    #region truck request changes
    protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }
    }

    protected void ddlExportType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }
    }


    protected void gvTruckRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "packingdocs")
        {
            if (e.CommandArgument.ToString() != "")
            {
                int TransRequestId = Convert.ToInt32(e.CommandArgument.ToString());
                if (TransRequestId > 0)
                {
                    string FilePath = "";
                    String ServerPath = FileServer.GetFileServerDir();
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectoryByName("TransportFiles");
                        DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransRequestId);
                        if (dsGetDoc != null)
                        {
                            for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                            {
                                if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                                {
                                    if (ServerPath == "")
                                    {
                                        FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                                    }
                                    else
                                    {
                                        FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                                    }
                                    zip.AddFile(FilePath, "TransportFiles");
                                }
                            }

                            Response.Clear();
                            Response.BufferOutput = false;
                            string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }
                    }
                }
            }
        }
        else if (e.CommandName.ToLower().Trim() == "select")
        {
            if (e.CommandArgument.ToString() != "")
            {
                string RefNo = e.CommandArgument.ToString();
                lblTransRefNo.Text = RefNo;
                if (Convert.ToInt32(Session["JobId"].ToString()) > 0)
                {
                    DataView dsGetJobDetail = DBOperations.GetTransportDetailByJobId(Convert.ToInt32(Session["JobId"].ToString()));
                    if (dsGetJobDetail != null)
                    {
                        ddDeliveryType.Visible = true;
                        ddlExportType1.Visible = false;
                        if (dsGetJobDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
                        {
                            ddDeliveryType.SelectedValue = dsGetJobDetail.Table.Rows[0]["DeliveryType"].ToString();
                            ddDeliveryType_SelectedIndexChanged(null, EventArgs.Empty);
                        }
                        lblType_Title.Text = "Delivery Type";

                        string VehiclePlaced = dsGetJobDetail.Table.Rows[0]["IsVehiclePlaced"].ToString();
                        if (VehiclePlaced != "1")
                        {

                            dvtruckDetail.Visible = true;
                            tblTruckRequest.Visible = true;
                            txtDestination.Text = dsGetJobDetail.Table.Rows[0]["Destination"].ToString();
                            txtDimension1.Text = dsGetJobDetail.Table.Rows[0]["Dimension"].ToString();
                            lblJobNumber.Text = dsGetJobDetail.Table.Rows[0]["JobRefNo"].ToString();
                            txtVehiclePlaceDate.Text = dsGetJobDetail.Table.Rows[0]["VehiclePlaceRequireDate"].ToString();
                            txtRemark1.Text = dsGetJobDetail.Table.Rows[0]["Remark"].ToString();
                        }
                        else
                        {

                            dvtruckDetail.Visible = false;
                        }
                    }
                }

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            btnSaveDocument_Click(fuDocument, EventArgs.Empty);
        }

        DateTime dtVehiclePlaceRequireDate = DateTime.MinValue;
        int JobType = 0, TotalContainers = 0, VehicleRequired = 1, Mode = 0, DeliveryType = 0, ExportType = 0;

        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }

        if (ddDeliveryType.SelectedValue != "0")
        {
            DeliveryType = Convert.ToInt32(ddDeliveryType.SelectedValue);
        }

        if (ddlExportType1.SelectedValue != "0")
        {
            ExportType = Convert.ToInt32(ddlExportType1.SelectedValue);
        }

        if (Session["JobId"].ToString() != "" && Session["JobId"].ToString() != "0")
        {
            //if (hdnJobType.Value != "0" && hdnJobType.Value != "")
            //{
            //    JobType = Convert.ToInt32(hdnJobType.Value);
            //}


            if (txtVehiclePlaceDate.Text.Trim() != "")
                dtVehiclePlaceRequireDate = Commonfunctions.CDateTime(txtVehiclePlaceDate.Text.Trim());

            int result = DBOperations.UpdateTransportRequest(Convert.ToInt32(Session["JobId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
                txtDimension1.Text.Trim(), dtVehiclePlaceRequireDate, LoggedInUser.glUserId);

            if (result == 0)
            {
                tblTruckRequest.Visible = false;
                lblError.Text = "Truck detail Added successfully!";
                lblError.CssClass = "success";
                gvTruckRequest.DataBind();
                // Add packing list documents
                if (Convert.ToString(ViewState["PackingList"]) != "")
                {
                    DataTable dtPackingList = (DataTable)ViewState["PackingList"];
                    if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                    {
                        string DocPath = "";
                        for (int i = 0; i < dtPackingList.Rows.Count; i++)
                        {
                            if (dtPackingList.Rows[i]["DocPath"] != null)
                                DocPath = dtPackingList.Rows[i]["DocPath"].ToString();
                            int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["JobId"].ToString()), DocPath, LoggedInUser.glUserId);
                        }
                    }
                }
            }
        }
    }

    protected void rptDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["PackingList"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["PackingList"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.Text = "Successfully Deleted Document.";
                lblError.CssClass = "success";
                rptDocument.DataBind();
            }
            else
            {
                lblError.Text = "Error while deleting document. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadPackingList(FilePath);
        }
    }

    protected void BindGrid()
    {
        //if (ViewState["PackingList"].ToString() != "")
        //{
        DataTable dtPackingList = (DataTable)ViewState["PackingList"];
        rptDocument.DataSource = dtPackingList;
        rptDocument.DataBind();
        // }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (FileUpload1 != null && FileUpload1.HasFile)
            fileName = UploadFiles1(FileUpload1);
        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["PackingList"];
            if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
            {
                for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                {
                    if (dtAnnexure.Rows[i]["PkId"] != null)
                    {
                        PkId = Convert.ToInt32(dtAnnexure.Rows[i]["PkId"].ToString());
                        PkId++;
                    }
                }
            }
            if (dtAnnexure != null)
                OriginalRows = dtAnnexure.Rows.Count;              //get original rows of grid view.

            dtAnnexure.Rows.Add(PkId, fileName, FileUpload1.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["PackingList"] = dtAnnexure;

            // Add packing list documents
            if (Convert.ToString(ViewState["PackingList"]) != "")
            {
                DataTable dtPackingList = (DataTable)ViewState["PackingList"];
                if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                {
                    string DocPath = "";
                    for (int i = 0; i < dtPackingList.Rows.Count; i++)
                    {
                        if (dtPackingList.Rows[i]["DocPath"] != null)
                            DocPath = dtPackingList.Rows[i]["DocPath"].ToString();
                        int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["JobId"].ToString()), DocPath, LoggedInUser.glUserId);
                    }
                }
            }
            BindGrid();
            if (OriginalRows < AfterInsertedRows)
            {
                lblError.Text = "Document Added successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == null)
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("\\UploadFiles\\Transport\\" + FilePath);
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
            return FilePath + FileName;
        }
        else
        {
            return "";
        }
    }

    protected void DownloadPackingList(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("\\UploadFiles\\Transport\\" + DocumentPath);
        else
            ServerPath = ServerPath + DocumentPath;
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    private string UploadFiles1(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == null)
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Transport\\" + FilePath);
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
            return FilePath + FileName;
        }
        else
        {
            return "";
        }
    }
    #endregion
}








