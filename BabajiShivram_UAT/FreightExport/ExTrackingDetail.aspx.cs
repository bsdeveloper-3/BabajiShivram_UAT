using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QueryStringEncryption;
using System.Data;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net.Http;

public partial class FreightExport_ExTrackingDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    static string JobRefNO = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnFileUpload);
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(gvTruckRequest);


        if (Session["EnqId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Session Expired! Please try again');</script>", false);
            Response.Redirect("ExOperationTrack.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
            else
            {
                Response.Redirect("ExOperationTrack.aspx");
            }
        }
        // Allow  Only Future Date For Reminder
        calVehiclePlaceDate.StartDate = DateTime.Today;

        // Set the minimum value for the MaskedEditValidator to today's date
        mevVehiclePlaceDate.MinimumValue = DateTime.Today.ToString("dd/MM/yyyy");

    }
    private void GetFreightDetail(int Enqid)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        hdnTransReqId.Value = Convert.ToString(Enqid);
        gvTruckRequest.DataBind();
        // Freight Operation Detail

        //DataSet dsDetail = DBOperations.GetOperationDetail(Enqid);   // GetExpBookingDetail
        DataSet dsDetail = DBOperations.GetExpBookingDetail(Enqid);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            // Billing Repository
            if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 3)
            {
                fsRepository.Visible = true;
                btnShowBillingRepository.Visible = true;
            }

            if (dsDetail.Tables[0].Rows[0]["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = dsDetail.Tables[0].Rows[0]["CountryCode"].ToString();
            }

            // Allow Vendor Fund Request
            if (LoggedInUser.glUserId == 81 || LoggedInUser.glUserId == 1) // Billing Dept HOD
            {
                if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsFundAllowed"]) == true)
                {
                    rblFundRequest.SelectedValue = "1";
                }
                else
                {
                    rblFundRequest.SelectedValue = "0";
                }

                fldFundRequest.Visible = true;
                accExpense.Visible = true;
                btnAllowFundRequest.Enabled = true;
            }

            int ddDivision1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["Division"]);
            int ddPlant1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["Plant"]);
            int CustomerId1 = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["CustomerId"]);
            Session["CustId"] = CustomerId1;
            DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;
            if (dddiv != null)
            {
                DBOperations.FillCustomerDivision(dddiv, CustomerId1);
            }

            FVFreightDetail.DataSource = dsDetail;
            FVFreightDetail.DataBind();

            fvOperation.DataSource = dsDetail;
            fvOperation.DataBind();

            fvVGMForm13.DataSource = dsDetail;
            fvVGMForm13.DataBind();

            fvCustPreAlert.DataSource = dsDetail;
            fvCustPreAlert.DataBind();

            fvShippedOnBoard.DataSource = dsDetail;
            fvShippedOnBoard.DataBind();

            fvAdvice.DataSource = dsDetail;
            fvAdvice.DataBind();

            //fvAgentInvoice.DataSource = dsDetail;
            //fvAgentInvoice.DataBind();
            hdnUploadPath.Value = dsDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            hdnFRJobNo.Value = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            hdnFRJobNo.Value = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            lblTitle.Text = "Operation Detail - " + dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            hdnWeight.Value = dsDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();
            hdnVolume.Value = dsDetail.Tables[0].Rows[0]["LCLVolume"].ToString();

            if (dsDetail.Tables[0].Rows[0]["PortOfLoadingId"] != DBNull.Value)
            {
                hdnLoadingPortId.Value = dsDetail.Tables[0].Rows[0]["PortOfLoadingId"].ToString();
            }

            if (dsDetail.Tables[0].Rows[0]["PortOfDischargeId"] != DBNull.Value)
            {
                hdnPortOfDischargedId.Value = dsDetail.Tables[0].Rows[0]["PortOfDischargeId"].ToString();
            }
            Panel pnlCancel = (Panel)FVFreightDetail.FindControl("pnlCancel");
            if (dsDetail.Tables[0].Rows[0]["REASON"].ToString() != "0")
            {
                pnlCancel.Visible = true;
            }

            //if (dsDetail.Tables[0].Rows[0]["IsGST"] != DBNull.Value)
            //{
            //    Label lblTaxType = (Label)fvCAN.FindControl("lblTaxType");

            //    if (lblTaxType != null)
            //    {
            //        if (Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsGST"]) == true)
            //        {
            //            hdnIsGST.Value = "1";
            //            txtTaxRate.Text = "";
            //            lblTaxType.Text = "GST";
            //        }
            //        else
            //        {
            //            hdnIsGST.Value = "0";
            //            lblTaxType.Text = "Service Tax";
            //            txtTaxRate.Text = "15.00";
            //        }
            //    }
            //}

            hdnPrevPOS.Value = dsDetail.Tables[0].Rows[0]["StateName"].ToString();
            hdnGSTNo.Value = dsDetail.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();
            //txtMailCC.Text = dsDetail.Tables[0].Rows[0]["EnquiryEmail"].ToString() + "," + LoggedInUser.glUserName + ",manish.radhakrishnan@babajishivram.com";
           // hdnUploadPath.Value = dsDetail.Tables[0].Rows[0]["DocDir"].ToString();

            hdnModeId.Value = dsDetail.Tables[0].Rows[0]["lMode"].ToString();

            if (dsDetail.Tables[0].Rows[0]["BranchID"] != DBNull.Value)
            {
                int BranchID = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["BranchID"]);

                if (BranchID == (Int32)EnumBranch.Delhi)
                {
                    hdnBranchEmail.Value = ",amit.thakur@babajishivram.com,sravinder@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Chennai)
                {
                    hdnBranchEmail.Value = ",vijayalakshmi@babajishivram.com, thaniga@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Ahmedabad)
                {
                    hdnBranchEmail.Value = ",mithun.sharma@babajishivram.com";
                }

                //txtMailCC.Text = txtMailCC.Text + hdnBranchEmail.Value;
            }

            if (Convert.ToInt32(hdnModeId.Value) > 1)
            {
                Panel pnlSea = (Panel)FVFreightDetail.FindControl("pnlSea");

                if (pnlSea != null)
                    pnlSea.Visible = true;
            }

            // Fill Consignee State

            DropDownList ddConsigneeState = (DropDownList)FVFreightDetail.FindControl("ddConsigneeState");

            if (ddConsigneeState != null)
            {
                // Fill State List

                DBOperations.FillState(ddConsigneeState);

                if (dsDetail.Tables[0].Rows[0]["ConsigneeStateID"] != DBNull.Value)
                {
                    string strStateID = dsDetail.Tables[0].Rows[0]["ConsigneeStateID"].ToString();

                    ddConsigneeState.SelectedValue = strStateID;
                }

            }

            // Fill Agent Master

            DropDownList ddAgent = (DropDownList)FVFreightDetail.FindControl("ddAgent");

            if (ddAgent != null)
            {
                // Fill Agent List By Enq ID
                DBOperations.FillFreightAgentCompany(ddAgent, Enqid);

                // Fill All Freight Agent List
                //DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));

                if (dsDetail.Tables[0].Rows[0]["AgentCompId"] != DBNull.Value)
                {
                    string strAgentID = dsDetail.Tables[0].Rows[0]["AgentCompId"].ToString();

                    ddAgent.SelectedValue = strAgentID;
                }

            }
            // Fill Pkgs Type

            DropDownList ddPackageType = (DropDownList)FVFreightDetail.FindControl("PackageType");

            if (ddPackageType != null)
            {
                string strPackageTypeId = dsDetail.Tables[0].Rows[0]["PackageTypeId"].ToString();
                DBOperations.FillPackageType(ddPackageType);
                ddPackageType.SelectedValue = strPackageTypeId;
            }

            // Fill Final Agent

            //DropDownList ddFinalAgent = (DropDownList)fvAgentPreAlert.FindControl("ddFinalAgent");

            //if (ddFinalAgent != null)
            //{
            //    // Fill Agent List By Enq ID
            //    DBOperations.FillCompanyByCategory(ddFinalAgent, 5);

            //    if (dsDetail.Tables[0].Rows[0]["FinalAgentID"] != DBNull.Value)
            //    {
            //        if (dsDetail.Tables[0].Rows[0]["FinalAgentID"] != DBNull.Value)
            //        {
            //            string strFinalAgentID = dsDetail.Tables[0].Rows[0]["FinalAgentID"].ToString();

            //            ddFinalAgent.SelectedValue = strFinalAgentID;
            //        }
            //    }

            //}

            EditBtnInvisible();
        }
    }
    protected void EditBtnInvisible()
    {
        DataSet dsDetail = DBOperations.GetExpBookingDetail(Convert.ToInt32(Session["EnqId"]));
        if (dsDetail.Tables[0].Rows[0]["Remark"] != DBNull.Value)
        {
            Button btnEditFreightDetail = (Button)FVFreightDetail.FindControl("btnEditFreightDetail");
            if (btnEditFreightDetail != null)
            {
                btnEditFreightDetail.Visible = false;
            }

            Button btnEditOperation = (Button)fvOperation.FindControl("btnEditOperation");
            if (btnEditOperation != null)
            {
                btnEditOperation.Visible = false;
            }

            Button btnEditVGMForm13 = (Button)fvVGMForm13.FindControl("btnEditVGMForm13");
            if (btnEditVGMForm13 != null)
            {
                btnEditVGMForm13.Visible = false;
            }

            Button btnEditCustPreAlert = (Button)fvCustPreAlert.FindControl("btnEditCustPreAlert");
            if (btnEditCustPreAlert != null)
            {
                btnEditCustPreAlert.Visible = false;
            }

            Button btnEditShippedOnBoard = (Button)fvShippedOnBoard.FindControl("btnEditShippedOnBoard");
            if (btnEditShippedOnBoard != null)
            {
                btnEditShippedOnBoard.Visible = false;
            }

            btnFileUpload.Visible = false;
        }
    }
    protected void FVFreightDetail_DataBound(object sender, EventArgs e)
    {
        if (FVFreightDetail.CurrentMode == FormViewMode.Edit)
        {
            DataRowView drv = (DataRowView)FVFreightDetail.DataItem;
            if (drv["CountryCode"] != DBNull.Value)
            {
                hdnCountryCode.Value = drv["CountryCode"].ToString();
                if (drv["CountryCode"].ToString().ToLower().Trim() == "india")
                {
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = true;
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = true;
                }
                else
                {
                    //((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = false;
                   // ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = false;
                }
            }
            else
            {
                ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVConsigneeState")).Visible = false;
                ((RequiredFieldValidator)FVFreightDetail.FindControl("RFVGSTN")).Visible = false;
            }
            DropDownList ddDiv = (DropDownList)FVFreightDetail.FindControl("ddDivision");
            DBOperations.FillCustomerDivision(ddDiv, Convert.ToInt32(Session["CustId"]));
            DataSet dsDetail = DBOperations.GetOperationDetail(Convert.ToInt32(Session["EnqId"]));
            int Divid = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["Division"].ToString());
            ddDiv.SelectedValue = Divid.ToString();

            DropDownList ddPlant = (DropDownList)FVFreightDetail.FindControl("ddPlant");
            DBOperations.FillCustomerPlant(ddPlant, Divid);
            int Plantid = Convert.ToInt16(dsDetail.Tables[0].Rows[0]["Plant"].ToString());
            ddPlant.SelectedValue = Plantid.ToString();
            //dd.DataBind();

            string IsBilling = dsDetail.Tables[0].Rows[0]["IsFileSentToBilling"].ToString();
            TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
            if (IsBilling == "True")
            {
                // string strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.Trim();
                CustName.Enabled = false;
                ddDiv.Enabled = false;
                ddPlant.Enabled = false;
                lblError.Text = "Job already proceed to billing, so you cant change customer name.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                CustName.Enabled = true;
                ddDiv.Enabled = true;
                ddPlant.Enabled = true;
            }
            TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
            TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
        }
    }  
    #region Document Upload
    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "viewdoc")
        {
            string DocPath = e.CommandArgument.ToString();
            ViewDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "removedocument")
        {
            int DocId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightDocument(DocId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Document deleted successfully!";
                lblError.CssClass = "success";

                gvFreightDocument.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Document details not found";
                lblError.CssClass = "errorMsg";

                gvFreightDocument.DataBind();
            }
        }
    }
    protected void btnFileUpload_Click(Object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int DocResult = -123;

        //if (txtDocName.Text.Trim() == "")
        //{
        //    lblError.Text = "Please enter the document name.";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

        if (EnqId > 0) // If EnquiryId Session Not Expired.Update Status Details
        {
            if (fuDocument.HasFile) // Add Enquiry Document
            {
                string strDocFolder = "FreightDoc\\" + hdnUploadPath.Value + "\\";

                string strFilePath = UploadDocument(strDocFolder);
                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddFreightDocument(EnqId, ddl_DocumentType.SelectedItem.ToString(), strFilePath, LoggedInUser.glUserId);

                    if (DocResult == 0)
                    {
                        lblError.Text = "Document uploaded successfully.";
                        lblError.CssClass = "success";
                        gvFreightDocument.DataBind();
                        //gvFreightAttach.DataBind();
                    }
                    else if (DocResult == 1)
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (DocResult == 2)
                    {
                        lblError.Text = "Document Name Already Exists! Please change the document name!.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }//END_IF_FileExists
            else
            {
                lblError.Text = "Please attach the document for upload!";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF_Enquiry
        else
        {
            Response.Redirect("FreightTracking.aspx");
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
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);
        }
        catch (Exception ex)
        {
        }
    }
    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

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

       // ServerFilePath = "C:\\inetpub\\wwwroot\\BabajiShivram\\" + FilePath;

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

    #region Edit Event
    protected void btnEditFreightDetail_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            FVFreightDetail.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
            CustName.Enabled = false;
            DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision");
            RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
            TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
            RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
            TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
            Label lblTransName = (Label)FVFreightDetail.FindControl("lblTransName");
            Label lblChaName = (Label)FVFreightDetail.FindControl("lblChaName");
            if (rdlbtnCHABy.SelectedValue == "2")
            {
                txtCHABy.Visible = true;
                lblChaName.Visible = true;
            }
            else
            {
                txtCHABy.Visible = false;
                lblChaName.Visible = false;
            }

            if (rdlbtnTransport.SelectedValue == "2")
            {
                txtTransportBy.Visible = true;
                lblTransName.Visible = true;
            }
            else
            {
                txtTransportBy.Visible = false;
                lblTransName.Visible = false;
            }
        }
    }
    protected void btnEditOperation_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvOperation.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnEditVGMForm13_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvVGMForm13.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnEditCustPreAlert_Click(object sender, EventArgs e)
    {   
        if (Session["EnqId"] != null)
        {
            fvCustPreAlert.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnEditShippedOnBoard_Click(object sender, EventArgs e)
    {
        if (Session["EnqId"] != null)
        {
            fvShippedOnBoard.ChangeMode(FormViewMode.Edit);
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    #endregion

    #region CANCEL Event
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExOperationTrack.aspx");
    }
    protected void btnCancelFreightDetail_Click(object sender, EventArgs e)
    {
        FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnCancelOperatione_Click(object sender, EventArgs e)
    {
        fvOperation.ChangeMode(FormViewMode.ReadOnly);
        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
        
    }
    protected void btnCancelVGMForm13_Click(object sender, EventArgs e)
    {
        fvVGMForm13.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnCancelCustPreAlert_Click(object sender, EventArgs e)
    {
        fvCustPreAlert.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnCancelShipOnBoard_Click(object sender, EventArgs e)
    {
        fvShippedOnBoard.ChangeMode(FormViewMode.ReadOnly);

        if (Session["EnqId"] != null)
        {
            GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    #endregion

    #region Update Event
    protected void btnUpdateFreightDetail_Click(object sender, EventArgs e)
    {
        DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
        DropDownList ddlReason = (DropDownList)FVFreightDetail.FindControl("ddlReason");
        TextBox txtCancelRemark = (TextBox)FVFreightDetail.FindControl("txtCancelRemark");
        if (ddlCancelAllow.SelectedValue == "1")
        {
            if (txtCancelRemark.Text != "" && ddlReason.SelectedValue != "0")
            {
                UpdateJobDetail();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please enter cancel Remark and select cancel Reason";
                lblError.CssClass = "errorMsg";
                lblError.Visible = true;
                //                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('msg');", true);
            }
        }
        else
        {
            UpdateJobDetail();
        }
    }
    protected void UpdateJobDetail()
    {
        string strNoOfPackage = "", strGrossWt = "", strChargeWeight = "", strLCLCBM = "", LCLFCL = "", strContainer20 = "",
            strPONo = "", strBookingDetail = "", strContainer40 = "", strInvoibeNo = "", strCargoDesc = "", strCustomer = ""; 
        DateTime dtInvoiceDate = DateTime.MinValue, dtBookingDate = DateTime.MinValue;
        int Division = 0, Plant = 0;

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        TextBox txtNoOfPackage = (TextBox)FVFreightDetail.FindControl("txtNoOfPkgs");
        TextBox txtGrossWeight = (TextBox)FVFreightDetail.FindControl("txtGrossWeight");
        TextBox txtChargableWeight = (TextBox)FVFreightDetail.FindControl("txtChargWeight");
        TextBox txtLCLVolume = (TextBox)FVFreightDetail.FindControl("txtLCLVolume");
        DropDownList ddContainerType = (DropDownList)FVFreightDetail.FindControl("ddContainerType");
        TextBox txtCont20 = (TextBox)FVFreightDetail.FindControl("txtCont20");
        TextBox txtCont40 = (TextBox)FVFreightDetail.FindControl("txtCont40");
        TextBox txtInvoiceNo = (TextBox)FVFreightDetail.FindControl("txtInvoiceNo");
        TextBox txtPONumber = (TextBox)FVFreightDetail.FindControl("txtPONumber");
        TextBox txtBookingDetails = (TextBox)FVFreightDetail.FindControl("txtBookingDetails");
        TextBox txtCargoDescription = (TextBox)FVFreightDetail.FindControl("txtCargoDescription");
        TextBox txtInvoiceDate = (TextBox)FVFreightDetail.FindControl("txtInvoiceDate");
        TextBox txtBookingDate = (TextBox)FVFreightDetail.FindControl("txtBookingDate");

        if (txtInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }
        if (txtBookingDate.Text.Trim() != "")
        {
            dtBookingDate = Commonfunctions.CDateTime(txtBookingDate.Text.Trim());
        }

        int ContainerType = 0, NoOfPackage = 0, Container20 = 0, Container40 = 0, result = -123;
        if (ddContainerType.SelectedValue != null)
        {
            ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);
        }
        if (txtNoOfPackage.Text.Trim() != "")
        {
            NoOfPackage = Convert.ToInt32(txtNoOfPackage.Text.Trim());
        }
        if (txtCont20.Text.Trim() != "")
        {
            Container20 = Convert.ToInt32(txtCont20.Text.Trim());
        }
        if (txtCont40.Text.Trim() != "")
        {
            Container40 = Convert.ToInt32(txtCont40.Text.Trim());
        }

        strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.Trim();
        Division = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddDivision")).SelectedValue);
        Plant = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddPlant")).SelectedValue);
        RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
        RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
        TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
        TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");

        result = DBOperations.UpdFreightBookingEx(EnqId, NoOfPackage, txtGrossWeight.Text.Trim(), txtChargableWeight.Text.Trim(),
            txtLCLVolume.Text.Trim(), ContainerType, Container20, Container40, txtInvoiceNo.Text.Trim(),
            txtPONumber.Text.Trim(), txtBookingDetails.Text.Trim(), txtCargoDescription.Text.Trim(), dtInvoiceDate, dtBookingDate,
            strCustomer, Division, Plant, Convert.ToInt16(rdlbtnTransport.SelectedValue), Convert.ToInt16(rdlbtnCHABy.SelectedValue),
            txtTransportBy.Text, txtCHABy.Text,
            LoggedInUser.glUserId);

        if (result == 0)
        {
            DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
            if (ddlCancelAllow.SelectedValue == "1")
            {
                DropDownList ddlReason1 = (DropDownList)FVFreightDetail.FindControl("ddlReason");
                string Remark = ((TextBox)FVFreightDetail.FindControl("txtCancelRemark")).Text.Trim();
                result = DBOperations.FR_InsJobStatus(EnqId, 25, ddlReason1.SelectedItem.Text, Remark, LoggedInUser.glModuleId, LoggedInUser.glFinYearId, LoggedInUser.glUserId);
            }

            EditBtnInvisible();

            lblError.Text = "Booking Detail Update Successfully";
            lblError.CssClass = "success";

            FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
        }
        else if (result == 2)
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Something Wrong";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnUpdateOperation_Click(object sender, EventArgs e)
    {
        int SBNo = 0, result = 0;
        string ASIBy = "";
        DateTime dtSBDate = DateTime.MinValue, dtContainerPickup = DateTime.MinValue, dtCustomPermission = DateTime.MinValue,
            dtStuffingDate = DateTime.MinValue, dtCLPDate = DateTime.MinValue, dtCartingDate = DateTime.MinValue, dtASIDate = DateTime.MinValue;

        int EnqId = Convert.ToInt32(Session["EnqId"]);

        TextBox txtSBNo = (TextBox)fvOperation.FindControl("txtSBNo");
        if (txtSBNo.Text.Trim() != "")
        {
            SBNo = Convert.ToInt32(txtSBNo.Text.Trim());
        }
        TextBox txtSBDate = (TextBox)fvOperation.FindControl("txtSBDate");
        TextBox txtContPickDate = (TextBox)fvOperation.FindControl("txtContPickDate");
        TextBox txtCustomPermiDate = (TextBox)fvOperation.FindControl("txtCustomPermiDate");
        TextBox txtStuffingDate = (TextBox)fvOperation.FindControl("txtStuffingDate");
        TextBox txtCLPDate = (TextBox)fvOperation.FindControl("txtCLPDate");

        TextBox txtCartingDate = (TextBox)fvOperation.FindControl("txtCartingDate");
        TextBox txtASIDate = (TextBox)fvOperation.FindControl("txtASIDate");

        if (txtSBDate.Text.Trim() != "")
        {
            dtSBDate = Commonfunctions.CDateTime(txtSBDate.Text.Trim());
        }
        if (txtContPickDate.Text.Trim() != "")
        {
            dtContainerPickup = Commonfunctions.CDateTime(txtContPickDate.Text.Trim());
        }
        if (txtCustomPermiDate.Text.Trim() != "")
        {
            dtCustomPermission = Commonfunctions.CDateTime(txtCustomPermiDate.Text.Trim());
        }
        if (txtStuffingDate.Text.Trim() != "")
        {
            dtStuffingDate = Commonfunctions.CDateTime(txtStuffingDate.Text.Trim());
        }
        if (txtCLPDate.Text.Trim() != "")
        {
            dtCLPDate = Commonfunctions.CDateTime(txtCLPDate.Text.Trim());
        }
        if (txtCartingDate.Text.Trim() != "")
        {
            dtCartingDate = Commonfunctions.CDateTime(txtCartingDate.Text.Trim());
        }

        TextBox txtASIBy = (TextBox)fvOperation.FindControl("txtASIBy");      

        if (txtASIDate.Text.Trim() != "")
        {
            dtASIDate = Commonfunctions.CDateTime(txtASIDate.Text.Trim());
        }

        result = DBOperations.UpdFreightOperationEx(EnqId, SBNo, dtSBDate, dtContainerPickup, dtCustomPermission,
                             dtStuffingDate, dtCLPDate, dtCartingDate, txtASIBy.Text.Trim(), dtASIDate, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Operation Detail Update Successfully";
            lblError.CssClass = "success";  
                     
            fvOperation.ChangeMode(FormViewMode.ReadOnly);
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
        }
        else if (result == 2)
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Something Wrong";
            lblError.CssClass = "errorMsg";
        }

    }
    protected void btnUpdateCustPreAlert_Click(object sender, EventArgs e)
    {
        int SBNo = 0, result = 0;
        string ASIBy = "";
        DateTime dtMBLDate = DateTime.MinValue, dtHBLDate = DateTime.MinValue, dtLEODate = DateTime.MinValue;
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        TextBox txtMBLNo = (TextBox)fvCustPreAlert.FindControl("txtMBLNo");
        TextBox txtHBLNo = (TextBox)fvCustPreAlert.FindControl("txtHBLNo");
        TextBox txtMBLDate = (TextBox)fvCustPreAlert.FindControl("txtMBLDate");
        TextBox txtHBLDate = (TextBox)fvCustPreAlert.FindControl("txtHBLDate");
        TextBox txtFlightSchedule = (TextBox)fvCustPreAlert.FindControl("txtFlightSchedule");
        TextBox txtLeoDate = (TextBox)fvCustPreAlert.FindControl("txtLeoDate");     

        if (txtMBLDate.Text.Trim() != "")
        {
            dtMBLDate = Commonfunctions.CDateTime(txtMBLDate.Text.Trim());
        }
        if (txtHBLDate.Text.Trim() != "")
        {
            dtHBLDate = Commonfunctions.CDateTime(txtHBLDate.Text.Trim());
        }
        if (txtLeoDate.Text.Trim() != "")
        {
            dtLEODate = Commonfunctions.CDateTime(txtLeoDate.Text.Trim());
        }    

        result = DBOperations.UpdFreighCustPreAlertEx(EnqId, txtMBLNo.Text.Trim(), txtHBLNo.Text.Trim(), dtMBLDate, dtHBLDate, txtFlightSchedule.Text.Trim(), dtLEODate, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Customer PreAlert Detail Update Successfully";
            lblError.CssClass = "success";

            fvCustPreAlert.ChangeMode(FormViewMode.ReadOnly);
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
        }
        else if (result == 2)
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Something Wrong";
            lblError.CssClass = "errorMsg";
        }

    }
    #endregion
    public string GetBooleanToYesNo(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Yes";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "No";
        }

        return strReturnText;
    }
    protected void btnUpdateVGMForm13_Click(object sender, EventArgs e)
    {
        int SBNo = 0, result = 0;
        
        DateTime dtSBDate = DateTime.MinValue, dtVGMDate = DateTime.MinValue, dtFORM13Date = DateTime.MinValue;
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        TextBox txtSBNo = (TextBox)fvVGMForm13.FindControl("txtSBNo");
        TextBox txtSBDate = (TextBox)fvVGMForm13.FindControl("txtSBDate");
        TextBox txtVGMDate = (TextBox)fvVGMForm13.FindControl("txtVGMDate");
        TextBox txtForm13Date = (TextBox)fvVGMForm13.FindControl("txtForm13Date");

        if (txtSBNo.Text.Trim() != "")
        {
            SBNo = Convert.ToInt32(txtSBNo.Text.Trim());
        }

        if (txtSBDate.Text.Trim() != "")
        {
            dtSBDate = Commonfunctions.CDateTime(txtSBDate.Text.Trim());
        }
        if (txtVGMDate.Text.Trim() != "")
        {
            dtVGMDate = Commonfunctions.CDateTime(txtVGMDate.Text.Trim());
        }
        if (txtForm13Date.Text.Trim() != "")
        {
            dtFORM13Date = Commonfunctions.CDateTime(txtForm13Date.Text.Trim());
        }

        result = DBOperations.UpdFreightVGMForm13Ex(EnqId, SBNo, dtSBDate, dtVGMDate, dtFORM13Date, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "VGM/Form13 Detail Update Successfully";
            lblError.CssClass = "success";

            fvVGMForm13.ChangeMode(FormViewMode.ReadOnly);
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
        }
        else if (result == 2)
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Something Wrong";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnUpdateAgentInvoice_Click(object sender, EventArgs e)
    {
        int result = 0;
        DateTime dtOnBoardDate = DateTime.MinValue;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
              
        TextBox txtOnBoardDate = (TextBox)fvShippedOnBoard.FindControl("txtOnBoardDate");       

        if (txtOnBoardDate.Text.Trim() != "")
        {
            dtOnBoardDate = Commonfunctions.CDateTime(txtOnBoardDate.Text.Trim());
        }       

        result = DBOperations.UpdFreightOnBoardEx(EnqId, dtOnBoardDate, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Shipped OnBoard Detail Update Successfully";
            lblError.CssClass = "success";

            fvShippedOnBoard.ChangeMode(FormViewMode.ReadOnly);
            if (Session["EnqId"] != null)
            {
                GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            }
        }
        else if (result == 2)
        {
            lblError.Text = "Record Not Found";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Something Wrong";
            lblError.CssClass = "errorMsg";
        }


    }

    #region Billing Repository

    protected void btnShowBillingRepository_Click(object sender, EventArgs e)
    {
        // Billing Repository

        if (LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 3)
        {
            BindBillingDocFromRepository();
        }

    }
    private void BindBillingDocFromRepository()
    {
        // FF00016-CNAI-18-19_20180521022448.pdf

        fsRepository.Visible = true;
        string strJobRefNo = hdnFRJobNo.Value;
        string searchPattern = strJobRefNo.Replace("/", "-");

        searchPattern = searchPattern + "*";

        //  string RemoteServerPath = @"\\192.168.6.116\f$\Babaji-shares\BS-Scan Document\";
        // string RemoteServerPath = @"\\\\babaji-storage\\BS-Scan Document\";

        //String RemoteServerPath = FileServer.GetFileServerDir();

        string RemoteServerPath = @"\\\\babaji-storage\\BS-Scan Document\";

        try
        {
            DirectoryInfo di = new DirectoryInfo(RemoteServerPath);

            // Get List of Billing Document

            var fileList = di.GetFiles(searchPattern, SearchOption.AllDirectories);

            gvBillingRepository.DataSource = fileList;

            gvBillingRepository.DataBind();
        }
        catch (Exception ex)
        {

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

    #region Job Cancel
    protected void ddlCancelAllow_SelectedIndexChanged(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        DropDownList ddlCancelAllow = (DropDownList)FVFreightDetail.FindControl("ddlCancelAllow");
        string Remark = ((TextBox)FVFreightDetail.FindControl("txtCancelRemark")).Text;
        if (ddlCancelAllow.SelectedValue == "1")
        {
            DataTable dtCustPreAlert = new DataTable();
            dtCustPreAlert = DBOperations.Get_FRPrealertDetail(EnqId);
            if (dtCustPreAlert.Rows.Count > 0)
            {
                FVFreightDetail.FindControl("trCancel").Visible = true;
            }
            else
            {
                FVFreightDetail.FindControl("trCancel").Visible = false;
                ddlCancelAllow.SelectedValue = "2";
                lblError.Text = "Can Not Allow to cancel job!!!!";
                lblError.CssClass = "errorMsg";
            }

            if (Remark != "")
            {
                Button btnUpdate = (Button)FVFreightDetail.FindControl("btnUpdateFreightDetail");
                btnUpdate.Visible = false;
                FVFreightDetail.FindControl("trCancel").Visible = false;
                ddlCancelAllow.SelectedValue = "2";
                lblError.Text = "Job has cancelled!!!!";
                lblError.CssClass = "errorMsg";
            }
            //FVJobDetail.FindControl("trCancel").Visible = true;
        }
        else
        {
            FVFreightDetail.FindControl("trCancel").Visible = false;
        }
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
            txtSubject1.Text = "E-Bill Dispatch/Job No :- " + strJobRefNo + " /Customer Reference No :- " + strCustRefNo + "";
            strAgencyCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntAgency"].ToString());
            strRIMCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntRIM"].ToString());
            strToMail = dsAlertDetail.Tables[0].Rows[0]["Email"].ToString();
            txtMailTo.Text = strToMail;
            //txtMailTo.Text = "developer1@babajishivram.com, developer2@babajishivram.com";
            txtMailCC1.Text = dt.Rows[0]["sEmail"].ToString() + "," + "query.billing@babajishivram.com";
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
            bool bEMailSucess = SendPreAlertEmail1();
            //bool bEMailSucess = true;
            // Update PreAlert Email Sent Status and Customer Email 

            if (bEMailSucess == true)
            {
                int Result = DBOperations.AddJobNotofication(JobId, 1, 14, txtMailTo.Text, txtMailCC1.Text, txtSubject1.Text, divPreviewEmailBillDispatch.InnerHtml, "0", LoggedInUser.glUserId);
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
    private bool SendPreAlertEmail1()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtMailTo.Text.Trim();
        strCCEmail = txtMailCC1.Text.Trim();
        strSubject = txtSubject1.Text.Trim();

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
    #endregion
    #region Vendor Payment Request
    protected void btnAllowFundRequest_Click(object sender, EventArgs e)
    {
        if (rblFundRequest.SelectedIndex == -1)
        {
            lblError.Text = "Please Check atleast One Option For Payment Request!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            int EnqId = Convert.ToInt32(Session["EnqId"]);
            bool isAllowed = false;

            if (rblFundRequest.SelectedValue == "1")
            {
                isAllowed = true;
            }

            int Result = DBOperations.UpdateJobFundRequestStatus(EnqId, isAllowed, 2, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Payment Request Status Updated Successfully!";
                lblError.CssClass = "success";

                GetFreightDetail(EnqId);
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Job Detail not found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
        }

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + lblError.Text + "')", true);
    }
    #endregion
    #region Dispatch Billing Dept
    protected void lnkPODCopyDownoad_Click(object sender, EventArgs e)
    {
        LinkButton lnkPODDownload = (LinkButton)sender;

        string FilePath = lnkPODDownload.CommandArgument;

        DownloadDocument(FilePath);
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
    #endregion

    protected void rdlbtnTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdlbtnTransport = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnTransport");
        TextBox txtTransportBy = (TextBox)FVFreightDetail.FindControl("txtTransportBy");
        Label lblTransName = (Label)FVFreightDetail.FindControl("lblTransName");
        if (rdlbtnTransport.SelectedValue == "1")
        {
            txtTransportBy.Text = "";
            txtTransportBy.Visible = false;
            lblTransName.Visible = false;
        }
        else if (rdlbtnTransport.SelectedValue == "2")
        {
            txtTransportBy.Visible = true;
            lblTransName.Visible = true;
        }
    }

    protected void rdlbtnCHABy_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdlbtnCHABy = (RadioButtonList)FVFreightDetail.FindControl("rdlbtnCHABy");
        TextBox txtCHABy = (TextBox)FVFreightDetail.FindControl("txtCHABy");
        Label lblChaName = (Label)FVFreightDetail.FindControl("lblChaName");
        if (rdlbtnCHABy.SelectedValue == "1")
        {
            txtCHABy.Text = "";
            txtCHABy.Visible = false;
            lblChaName.Visible = false;
        }
        else if (rdlbtnCHABy.SelectedValue == "2")
        {
            txtCHABy.Visible = true;
            lblChaName.Visible = true;
        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;
        int DivisonId = Convert.ToInt32(dddiv.SelectedValue);
        DropDownList ddplant = (DropDownList)FVFreightDetail.FindControl("ddPlant"); ;
        DBOperations.FillCustomerPlant(ddplant, DivisonId);
        //ddDivision.Focus();
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        int CustomerId = 0;
        TextBox CustName = (TextBox)FVFreightDetail.FindControl("txtCustomer") as TextBox;
        DropDownList dddiv = (DropDownList)FVFreightDetail.FindControl("ddDivision"); ;

        DataSet dsGetCustIdByName = DBOperations.GetCustomerIdByName(CustName.Text);

        if (dsGetCustIdByName.Tables[0].Rows.Count > 0)
        {
            string strCustId = "";
            strCustId = Convert.ToString(dsGetCustIdByName.Tables[0].Rows[0]["CustomerId"]);
            if (strCustId != "")
            {
                CustomerId = Convert.ToInt32(strCustId);
            }
        }

        DBOperations.FillCustomerDivision(dddiv, CustomerId);
        DropDownList ddplant = (DropDownList)FVFreightDetail.FindControl("ddPlant");
        DataSet dsCust = DBOperations.GetCustomerIdByName(CustName.Text);
        if (dsCust.Tables[0].Rows.Count > 0)
        {
            int CustId = Convert.ToInt32(dsCust.Tables[0].Rows[0]["CustomerId"].ToString());
            DBOperations.FillCustomerDivision(dddiv, CustId);
           // dddiv.Items.Insert(0, "Select");
            dddiv.SelectedValue="0";
           // dddiv.DataBind();
            ddplant.SelectedValue = "0";
            dddiv.Enabled = true;
            ddplant.Enabled = true;
        }
        else
        {
            Session["CustId"] = "";
            CustName.Text = "";
            CustName.Focus();
            dddiv.Enabled = false;
            ddplant.Enabled = false;
            lblError.Text = "Please enter correct customer name";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnJobDelivery_Click(object sender, EventArgs e)
    {
        DateTime TruckRequestDate = DateTime.MinValue, JobDeliveryDate = DateTime.MinValue;
        string DeliveryIns = "", DeliveryDestination = "", DeliveryAddress = "";

        bool bTransportationByBabaji = false;

        int JobId = Convert.ToInt32(Session["JobId"]);
        int Result = -123;

        if (txtTruckRequestDate.Text.Trim() != "")
        {
            TruckRequestDate = Commonfunctions.CDateTime(txtTruckRequestDate.Text.Trim());
        }

        if (txtJobDeliveryDate.Text.Trim() != "")
        {
            JobDeliveryDate = Commonfunctions.CDateTime(txtJobDeliveryDate.Text.Trim());
        }
        if (txtDeliveryIns.Text.Trim() != "")
        {
            DeliveryIns = txtDeliveryIns.Text.Trim();
        }
        if (txtDeliveryDestination.Text.Trim() != "")
        {
            DeliveryDestination = txtDeliveryDestination.Text.Trim();
        }
        if (txtDeliveryAddress.Text.Trim() != "")
        {
            DeliveryAddress = txtDeliveryAddress.Text.Trim();
        }

        if (lblTransportationBy.Text.Trim().ToLower() == "babaji")
        {
            bTransportationByBabaji = true;
        }

        if (JobDeliveryDate == DateTime.MinValue && DeliveryIns == "" && DeliveryDestination == "" && DeliveryAddress == "")
        {
            lblError.Text = "Please Enter Delivery Information!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            Result = DBOperations.FR_UPDDeliveryDetails(JobId, 0, 0, TruckRequestDate, JobDeliveryDate, DeliveryIns,
                DeliveryDestination, DeliveryAddress, bTransportationByBabaji, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Delivery Detail Updated Successfully";
                lblError.CssClass = "success";

            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Please First Update Job Planning Details!";
                lblError.CssClass = "warning";
            }
            else
            {
                lblError.Text = "Please Enter Required Details!";
                lblError.CssClass = "warning";
            }
        }
    }

    protected void GridViewDelivery_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "update")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewDelivery.DataKeys[gvrow.RowIndex].Value.ToString();

            TextBox txtDeliveryDate = (TextBox)gvrow.FindControl("txtDeliveryDate");
            string strDeliveryDate = txtDeliveryDate.Text.Trim();
            TextBox txtEmptyContainerDate = (TextBox)gvrow.FindControl("txtEmptyContainerDate");
            string strEmptyContainerDate = txtEmptyContainerDate.Text.Trim();

            DataSourceDelivery.UpdateParameters["lId"].DefaultValue = strlId;

            if (strDeliveryDate != "")
            {
                strDeliveryDate = Commonfunctions.CDateTime(txtDeliveryDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["DeliveryDate"].DefaultValue = strDeliveryDate;
            }

            if (strEmptyContainerDate != "")
            {
                strEmptyContainerDate = Commonfunctions.CDateTime(txtEmptyContainerDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["EmptyContRetrunDate"].DefaultValue = strEmptyContainerDate;
            }

            TextBox txtCargoRecvdby = (TextBox)gvrow.FindControl("txtCargoRecvdby");
            string CargoRecvdby = txtCargoRecvdby.Text.Trim();
            DataSourceDelivery.UpdateParameters["CargoReceivedby"].DefaultValue = CargoRecvdby;
            //
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
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }

            else if (hdnPODPath.Value != "")
            {
                PODPath = hdnPODPath.Value;
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }

            //

            TextBox txtNFormNo = (TextBox)gvrow.FindControl("txtNFormNo");
            string NFormNo = txtNFormNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["NFormNo"].DefaultValue = NFormNo;

            TextBox txtNFormDate = (TextBox)gvrow.FindControl("txtNFormDate");
            string strNFormDate = txtNFormDate.Text.Trim();
            if (strNFormDate != "")
            {
                strNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["NFormDate"].DefaultValue = strNFormDate;
            }

            TextBox txtNClosingDate = (TextBox)gvrow.FindControl("txtNClosingDate");
            string strNClosingDate = txtNClosingDate.Text.Trim();
            if (strNClosingDate != "")
            {
                strNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["NClosingDate"].DefaultValue = strNClosingDate;
            }

            TextBox txtSFormNo = (TextBox)gvrow.FindControl("txtSFormNo");
            string SFormNo = txtSFormNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["SFormNo"].DefaultValue = SFormNo;

            TextBox txtSFormDate = (TextBox)gvrow.FindControl("txtSFormDate");
            string strSFormDate = txtSFormDate.Text.Trim();
            if (strSFormDate != "")
            {
                strSFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["SFormDate"].DefaultValue = strSFormDate;
            }

            TextBox txtSClosingDate = (TextBox)gvrow.FindControl("txtSClosingDate");
            string strSClosingDate = txtSClosingDate.Text.Trim();
            if (strSClosingDate != "")
            {
                strSClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["SClosingDate"].DefaultValue = strSClosingDate;
            }

            TextBox txtOctroiAmount = (TextBox)gvrow.FindControl("txtOctroiAmount");
            string strOctroiAmount = txtOctroiAmount.Text.Trim();
            DataSourceDelivery.UpdateParameters["OctroiAmount"].DefaultValue = strOctroiAmount;

            TextBox txtOctroiReceiptNo = (TextBox)gvrow.FindControl("txtOctroiReceiptNo");
            string strOctroiReceiptNo = txtOctroiReceiptNo.Text.Trim();
            DataSourceDelivery.UpdateParameters["OctroiReceiptNo"].DefaultValue = strOctroiReceiptNo;

            TextBox txtOctroiPaidDate = (TextBox)gvrow.FindControl("txtOctroiPaidDate");
            string strOctroiPaidDate = txtOctroiPaidDate.Text.Trim();
            if (strOctroiPaidDate != "")
            {
                strOctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["OctroiPaidDate"].DefaultValue = strOctroiPaidDate;
            }

        }

        if (e.CommandName == "Cancel")
        {
            lblError.Visible = false;
            lblError.Text = "";
            GridViewDelivery.EditIndex = -1;
        }

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            // DownloadPODDocument(DocPath);
        }

    }

    protected void GridViewDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");
            bool IsNForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsNForm");
            bool IsSForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsSForm");
            bool IsOctroi = (bool)DataBinder.Eval(e.Row.DataItem, "IsOctroi");
            bool IsRoadPermit = (bool)DataBinder.Eval(e.Row.DataItem, "IsRoadPermit");

            if (DeliveryTypeId == (Int32)DeliveryType.Loaded) // Delivery Type
            {
                GridViewDelivery.Columns[2].Visible = true; // Container NO
                GridViewDelivery.Columns[3].Visible = false; // No Of Packages
            }
            else
            {
                GridViewDelivery.Columns[2].Visible = false; // Container NO
                GridViewDelivery.Columns[3].Visible = true; // No Of Packages
            }

            if (IsNForm == true) // NForm Applicable
            {
                GridViewDelivery.Columns[15].Visible = true; // N Form No
                GridViewDelivery.Columns[16].Visible = true; // N Form Date
                GridViewDelivery.Columns[17].Visible = true; // N Closing Date
            }
            else
            {
                GridViewDelivery.Columns[15].Visible = false; // N Form No
                GridViewDelivery.Columns[16].Visible = false; // N Form Date
                GridViewDelivery.Columns[17].Visible = false; // N Closing Date
            }

            if (IsSForm == true) // SForm Applicable
            {
                GridViewDelivery.Columns[18].Visible = true; // S Form No
                GridViewDelivery.Columns[19].Visible = true; // S Form Date
                GridViewDelivery.Columns[20].Visible = true; // S Closing Date
            }
            else
            {
                GridViewDelivery.Columns[18].Visible = false; // S Form No
                GridViewDelivery.Columns[19].Visible = false; // S Form Date
                GridViewDelivery.Columns[20].Visible = false; // S Closing Date
            }
            if (IsOctroi == true) // Octroi Applicable
            {
                GridViewDelivery.Columns[21].Visible = true; // Octroi Amount
                GridViewDelivery.Columns[22].Visible = true; // Octroi Receipt No	
                GridViewDelivery.Columns[23].Visible = true; // Octroi Paid Date
            }
            else
            {
                GridViewDelivery.Columns[21].Visible = false; // Octroi Amount
                GridViewDelivery.Columns[22].Visible = false; // Octroi Receipt No	
                GridViewDelivery.Columns[23].Visible = false; // Octroi Paid Date
            }
            if (IsRoadPermit == true) // Road Permit Applicable
            {
                GridViewDelivery.Columns[24].Visible = true; // Road Permit No
                GridViewDelivery.Columns[25].Visible = true; // Road Permit Date
            }
            else
            {
                GridViewDelivery.Columns[24].Visible = false; // Road Permit No
                GridViewDelivery.Columns[25].Visible = false; // Road Permit Date
            }
        }
    }
    protected void DataSourceDelivery_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

        if (Result == 0)
        {
            lblError.Text = "Delivery Detail Updated Successfully";
            lblError.CssClass = "success";
            GridViewDelivery.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
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
                if (Convert.ToInt32(Session["EnqId"].ToString()) > 0)
                {
                    DataView dsGetJobDetail = DBOperations.GetTransportDetailByJobId(Convert.ToInt32(Session["EnqId"].ToString()));
                    if (dsGetJobDetail != null && dsGetJobDetail.Count > 0)                           // For getting freight get Pincode drop and pickup detail         
                    {
                        txtpickPincode.Text = dsGetJobDetail[0]["PickupPincode"].ToString();
                        txtdropPincode.Text = dsGetJobDetail[0]["DropPincode"].ToString();
                    }
                    if (dsGetJobDetail != null)
                    {
                        ddDeliveryType.Visible = true;
                        ddlExportType.Visible = false;
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
                            txtDimension.Text = dsGetJobDetail.Table.Rows[0]["Dimension"].ToString();
                            lblJobNumber.Text = dsGetJobDetail.Table.Rows[0]["JobRefNo"].ToString();
                            txtVehiclePlaceDate.Text = dsGetJobDetail.Table.Rows[0]["VehiclePlaceRequireDate"].ToString();
                            txtRemark1.Text = dsGetJobDetail.Table.Rows[0]["Remark"].ToString();
                            txtpickState.Text = dsGetJobDetail.Table.Rows[0]["PickupState"].ToString();   //Add new details stete city pincode get job details
                            txtpickCity.Text = dsGetJobDetail.Table.Rows[0]["PickupCity"].ToString();
                            txtdropState.Text = dsGetJobDetail.Table.Rows[0]["DropState"].ToString();
                            txtdropCity.Text = dsGetJobDetail.Table.Rows[0]["DropCity"].ToString();
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

    #region truck request changes
    protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
        }
        if (ddDeliveryType.SelectedValue == "1")     //Loaded , '1' represents the Loaded option
        {
            loadedDocuments.Visible = true;
            lblEmpty_Letter.Visible = true;
            //  UpdBtn.Visible = true;
        }
        else
        {
            loadedDocuments.Visible = false;
            lblEmpty_Letter.Visible = false;
            //   UpdBtn.Visible = false;
        }
    }

    protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
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
        int PickupPincode = 0, DropPincode = 0;
        string PickupState = "", PickupCity = "", DropState = "", DropCity = "", EmptyLetter = "", EmptyDocPath = "", FileName = "", DocName = ""; //changes for update operation

        if (hdnModeId.Value != "" && hdnModeId.Value != "0")
        {
            Mode = Convert.ToInt32(hdnModeId.Value);
        }

        if (ddDeliveryType.SelectedValue != "0")
        {
            DeliveryType = Convert.ToInt32(ddDeliveryType.SelectedValue);
        }

        if (ddlExportType.SelectedValue != "0")
        {
            ExportType = Convert.ToInt32(ddlExportType.SelectedValue);
        }

        if (Session["JobId"].ToString() != "" && Session["JobId"].ToString() != "0")
        {
            //if (hdnJobType.Value != "0" && hdnJobType.Value != "")
            //{
            //    JobType = Convert.ToInt32(hdnJobType.Value);
            //}


            if (txtVehiclePlaceDate.Text.Trim() != "")
                dtVehiclePlaceRequireDate = Commonfunctions.CDateTime(txtVehiclePlaceDate.Text.Trim());

            PickupPincode = Convert.ToInt32(txtpickPincode.Text);
            DropPincode = Convert.ToInt32(txtdropPincode.Text);
            PickupState = txtpickState.Text;
            PickupCity = txtpickCity.Text;
            DropState = txtdropState.Text;
            DropCity = txtdropCity.Text;
            dvtruckDetail.Visible = true;
            EmptyLetter = loadedDocuments.FileName;

            if (ddDeliveryType.SelectedValue == "1")
            {
                EmptyDocPath = Regex.Replace(lblJobNumber.Text, "[^a-zA-Z0-9]", "");
                EmptyDocPath = "FreightDoc\\" + EmptyDocPath + "\\" + EmptyLetter;
                if (loadedDocuments != null && loadedDocuments.HasFile)
                    FileName = UploadEmptyDocuments(EmptyDocPath);
            }
            int result = DBOperations.UpTransFreightRequest(Convert.ToInt32(Session["EnqId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
                txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, FileName, EmptyDocPath, 122, PickupPincode, PickupState, PickupCity, DropPincode, DropState, DropCity, EmptyLetter, DocName, LoggedInUser.glUserId);

            //int result = DBOperations.UpdateTransportRequest(Convert.ToInt32(Session["JobId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
            //    txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, LoggedInUser.glUserId);

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

            //if (result > 0)
            //{


            //    //if (loggedInUser.glModuleId == 1)
            //    //{
            //    //    if (hdnJobId.Value != "" && hdnJobId.Value != "0")
            //    //    {
            //    //        int TransportBabaji = DBOperations.TR_updJobTransportBabaji(Convert.ToInt32(hdnJobId.Value), loggedInUser.glUserId);
            //    //    }
            //    //}
            //    Response.Redirect("SuccessPage.aspx?Request=" + result.ToString());

            //}
            //else if (result == -2)
            //{
            //    lblError.Text = "Truck request already exists! Please complete delivery for existing request first.";
            //    lblError.CssClass = "errorMsg";
            //}
            //else if (result == -3)
            //{
            //    lblError.Text = "No of containers to be dispatched exceed available containers.";
            //    lblError.CssClass = "errorMsg";
            //}
            //else
            //{
            //    lblError.Text = "Error while adding truck request. Please try again later.";
            //    lblError.CssClass = "errorMsg";
            //}
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
        if (ViewState["PackingList"].ToString() != "")
        {
            DataTable dtPackingList = (DataTable)ViewState["PackingList"];
            rptDocument.DataSource = dtPackingList;
            rptDocument.DataBind();
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (FileUpload1 != null && FileUpload1.HasFile)
            fileName = UploadFiles(FileUpload1);
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
    #endregion

    protected void txtpickPincode_TextChanged(object sender, EventArgs e)   //Get State and city by using Pincode API 
    {
        string pincode = txtpickPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnPincodeId.Value = "0";
            txtpickState.Text = "Invalid Pincode";
            txtpickCity.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;

                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {
                            txtpickState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtpickCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnPincodeId.Value = pincode;
                        }
                        else
                        {
                            txtpickState.Text = "State Not Found";
                            txtpickCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtpickState.Text = "State Not Found";
                        txtpickCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtpickState.Text = "Error";
                    txtpickCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
    }

    protected void txtdropPincode_TextChanged(object sender, EventArgs e)    // Get State and City By using Pincode API
    {
        string pincode = txtdropPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnpinid.Value = "0";
            txtdropState.Text = "Invalid Pincode";
            txtdropCity.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {

                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;


                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {

                            txtdropState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtdropCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnpinid.Value = pincode;
                        }
                        else
                        {
                            txtdropState.Text = "State Not Found";
                            txtdropCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtdropState.Text = "State Not Found";
                        txtdropCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtdropState.Text = "Error";
                    txtdropCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {

            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
    }
    #region Empty letter 
    //private string UploadEmptyDocuments(string FilePath)
    //{
    //    string FileName = loadedDocuments.FileName;
    //    FileName = FileServer.ValidateFileName(FileName);

    //    string ServerFilePath = FileServer.GetFileServerDir();

    //    if (ServerFilePath == "")
    //    {
    //        // Application Directory Path
    //        ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath + "\\");
    //        //ServerFilePath = Server.MapPath("..\\UploadFiles\\FreightDoc\\" + FilePath + "\\");


    //    }
    //    else
    //    {
    //        // File Server Path
    //        ServerFilePath = ServerFilePath + FilePath;
    //    }

    //    if (!System.IO.Directory.Exists(ServerFilePath))
    //    {
    //        System.IO.Directory.CreateDirectory(ServerFilePath);
    //    }
    //    if (loadedDocuments.FileName != string.Empty)
    //    {
    //        if (System.IO.File.Exists(ServerFilePath + FileName))
    //        {
    //            string ext = Path.GetExtension(FileName);
    //            FileName = Path.GetFileNameWithoutExtension(FileName);
    //            string FileId = RandomString(5);

    //            FileName += "_" + FileId + ext;
    //        }

    //        loadedDocuments.SaveAs(ServerFilePath + FileName);

    //        return FileName;
    //    }

    //    else
    //    {
    //        return "";
    //    }

    //  }

    private string UploadEmptyDocuments(string filePath)
    {
        // Validate the file name
        string fileName = loadedDocuments.FileName;
        //   fileName = FileServer.ValidateFileName(fileName);

        // Get the base directory for saving files
        string ServerFilePath = FileServer.GetFileServerDir();

        if (string.IsNullOrEmpty(ServerFilePath))
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath(Path.Combine("..\\UploadFiles", filePath));
        }
        else
        {
            // File Server Path
            ServerFilePath = Path.Combine(ServerFilePath, filePath);
        }

        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(ServerFilePath);
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Full path to save the file
        string fullPath = Path.Combine(directoryPath, fileName);

        // Check if the file already exists and modify the file name if necessary
        if (System.IO.File.Exists(fullPath))
        {
            string ext = Path.GetExtension(fileName);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string fileId = RandomString(5);

            fileName = $"{nameWithoutExt}_{fileId}{ext}";
            fullPath = Path.Combine(directoryPath, fileName);
        }

        // Save the file to the designated path
        loadedDocuments.SaveAs(fullPath);

        return fileName;
    }

    #endregion
}