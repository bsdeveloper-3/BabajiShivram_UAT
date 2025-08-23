using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
public partial class Freight_FreightDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnUpload);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("FreightTracking.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Detail";

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }

        // Allow  Only Future Date For Reminder
        calRemindDate.StartDate = DateTime.Today;

        // Set Minimum Reminder Date Today
        MskValRemindDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
    }

    #region Freight Status

    protected void btnStatusChange_Click(object sender, EventArgs e)
    {
        fvFreightStatus.ChangeMode(FormViewMode.Edit);

        if (Session["EnqId"] != null)
        {
            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));

            DropDownList ddFreightStatus = (DropDownList)fvFreightStatus.FindControl("ddFreightStatus");
            TextBox txtStatusDate = (TextBox)fvFreightStatus.FindControl("txtStatusDate");

            if (ddFreightStatus != null)
            {
                int StatusId = 0;

                if (hdnStatusId.Value != "")
                    StatusId = Convert.ToInt32(hdnStatusId.Value);

                DBOperations.FillFreightStatus(ddFreightStatus, StatusId);

            }
            if (txtStatusDate != null)
            {
                txtStatusDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreightTracking.aspx");
    }

    protected void btnStatusUpdate_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int result = -123;

        if (EnqId > 0) // If EnquiryId Session Not Expired. Update Status Details
        {

            DateTime dtStatusDate = DateTime.Today;

            DropDownList ddFreightStatus = (DropDownList)fvFreightStatus.FindControl("ddFreightStatus");
            TextBox txtStatusDate = (TextBox)fvFreightStatus.FindControl("txtStatusDate");
            TextBox txtStatusRemark = (TextBox)fvFreightStatus.FindControl("txtStatusRemark");
            TextBox txtEnquiryValue = (TextBox)fvFreightStatus.FindControl("txtEnquiryValue");

            int StatusId = Convert.ToInt32(ddFreightStatus.SelectedValue);

            int LostStatusID = 0;

            if (StatusId == 4)// LOST
            {
                DropDownList ddLostStaus = (DropDownList)fvFreightStatus.FindControl("ddLostStaus");

                LostStatusID = Convert.ToInt32(ddLostStaus.SelectedValue);
            }

            if (txtStatusDate.Text.Trim() != "")
            {
                dtStatusDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());
            }

            DataSet dsFreightDetail = DBOperations.GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            if (dsFreightDetail.Tables[0].Rows.Count > 0)
            {
                string EnquiryValue = dsFreightDetail.Tables[0].Rows[0]["EnquiryValue"].ToString();
                if (EnquiryValue == "" || EnquiryValue == null || EnquiryValue == "0")
                {
                    txtEnquiryValue.Visible = true;

                }
                else
                {
                    txtEnquiryValue.Visible = false;
                    txtEnquiryValue.Text = EnquiryValue;
                }
            }

            if (StatusId <= 0)
            {
                lblError.Text = "Please Select Current Freight Status!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                // For Awarded Enquiry Check if Quotation copy uploaded
                if (Convert.ToInt32(txtEnquiryValue.Text) != 0)
                {
                    result = DBOperations.UpdateFreightStatus(EnqId, StatusId, dtStatusDate, LostStatusID, Convert.ToInt32(txtEnquiryValue.Text), txtStatusRemark.Text.Trim(), LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblError.Text = "Status Changed Successfully!";
                        lblError.CssClass = "success";

                        gvStatusHistory.DataBind();
                        fvFreightStatus.ChangeMode(FormViewMode.ReadOnly);
                        SetFreightDetail(EnqId);
                    }
                    else if (result == 1)
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (result == 2)
                    {
                        lblError.Text = "Status Already Updated!";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (result == 4)
                    {
                        lblError.Text = "Please Upload Quotation Copy For Quoted Shipment";
                        lblError.CssClass = "errorMsg";

                        //txtDocName.Text = "Quotation Copy";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError.Text = "Enquiry value is not allowed 0";
                    lblError.CssClass = "errorMsg";
                    txtEnquiryValue.Text = "";
                }
            }
        }// END_IF
        else
        {
            Response.Redirect("FreightTracking.aspx");
        }
    }

    protected void btnStatusCancel_Click(object semder, EventArgs e)
    {
        fvFreightStatus.ChangeMode(FormViewMode.ReadOnly);
        SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
    }

    protected void ddFreightStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddFreightStatus = (DropDownList)fvFreightStatus.FindControl("ddFreightStatus");
        Label lblResult = (Label)fvFreightStatus.FindControl("lblResult");
        lblResult.Text = "";
        if (ddFreightStatus.SelectedValue == "4")
        {
            DropDownList ddLostStaus = (DropDownList)fvFreightStatus.FindControl("ddLostStaus");
            RequiredFieldValidator RFVLostStatus = (RequiredFieldValidator)fvFreightStatus.FindControl("RFVLostStatus");

            ddLostStaus.Visible = true;
            RFVLostStatus.Enabled = true;
        }

        TextBox txtEnquiryValue = (TextBox)fvFreightStatus.FindControl("txtEnquiryValue");
        Label lblEnqValue=(Label)fvFreightStatus.FindControl("lblEnqValue");
        if (ddFreightStatus.SelectedValue=="2" || ddFreightStatus.SelectedValue=="6")
        {
            lblEnqValue.Visible = true;
            txtEnquiryValue.Visible = true;
        }
        else
        {
            lblEnqValue.Visible = false;
            txtEnquiryValue.Visible = false;
        }

        DataSet dsFreightDetail = DBOperations.GetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        if (dsFreightDetail.Tables[0].Rows.Count > 0)
        {
            string EnquiryValue = dsFreightDetail.Tables[0].Rows[0]["EnquiryValue"].ToString();
            if (EnquiryValue == "" || EnquiryValue == null || EnquiryValue=="0")
            {
                lblEnqValue.Visible = true;
                txtEnquiryValue.Visible = true;
            }
            else
            {
                lblEnqValue.Visible = false;
                txtEnquiryValue.Visible = false;
            }
        }
    }
    #endregion

    #region Freight Detail
    private void SetFreightDetail(int EnqId)
    {
        DataSet dsFreightDetail = DBOperations.GetFreightDetail(EnqId);

        FillEnquiryAgent(EnqId);

        if (dsFreightDetail.Tables[0].Rows.Count > 0)
        {
            hdnUploadPath.Value = dsFreightDetail.Tables[0].Rows[0]["DocDir"].ToString();
            hdnModeId.Value = dsFreightDetail.Tables[0].Rows[0]["lMode"].ToString();

            if (dsFreightDetail.Tables[0].Rows[0]["PortOfLoadingId"] != DBNull.Value)
            {
                hdnLoadingPortId.Value = dsFreightDetail.Tables[0].Rows[0]["PortOfLoadingId"].ToString();
            }

            if (dsFreightDetail.Tables[0].Rows[0]["PortOfDischargeId"] != DBNull.Value)
            {
                hdnPortOfDischargedId.Value = dsFreightDetail.Tables[0].Rows[0]["PortOfDischargeId"].ToString();
            }

            if (dsFreightDetail.Tables[0].Rows[0]["CountryId"] != DBNull.Value)
            {
                hdnCountryId.Value = dsFreightDetail.Tables[0].Rows[0]["CountryId"].ToString();
            }
            if (dsFreightDetail.Tables[0].Rows[0]["SalesRepId"] != DBNull.Value)
            {
                hdnSalesRepId.Value = dsFreightDetail.Tables[0].Rows[0]["SalesRepId"].ToString();
            }
            if (dsFreightDetail.Tables[0].Rows[0]["lStatus"] != DBNull.Value)
            {
                hdnStatusId.Value = dsFreightDetail.Tables[0].Rows[0]["lStatus"].ToString();
            }
            if (dsFreightDetail.Tables[0].Rows[0]["lType"] != DBNull.Value)
            {
                hdnTypeId.Value = dsFreightDetail.Tables[0].Rows[0]["lType"].ToString();
            }

            // Bind Freight Detail & Status

            FVFreightDetail.DataSource = dsFreightDetail;
            FVFreightDetail.DataBind();

            fvFreightStatus.DataSource = dsFreightDetail;
            fvFreightStatus.DataBind();

            // Fill Agent Detail

            if (hdnCountryId.Value != "")
            {
                FillAgentDetail(Convert.ToInt32(hdnCountryId.Value));
            }

            // Hide Freight Status Update For Agent

            if (LoggedInUser.glType == (Int32)EnumUserType.FreightAgent)
            {
                Button btnFreightEdit = (Button)FVFreightDetail.FindControl("btnFreightEdit");
                Panel pnlSharedWith = (Panel)FVFreightDetail.FindControl("pnlSharedWith");

                if (btnFreightEdit != null)
                {
                    btnFreightEdit.Visible = false;
                }
                if (pnlSharedWith != null)
                {
                    pnlSharedWith.Visible = false;
                }

                fvFreightStatus.Enabled = false;
                fieldStatus.Visible = false;

                Button btnStatusChange = (Button)fvFreightStatus.FindControl("btnStatusChange");

                if (btnStatusChange != null)
                    btnStatusChange.Visible = false;
            }
        }
    }

    protected void btnFreightEdit_Click(object sender, EventArgs e)
    {
        FVFreightDetail.ChangeMode(FormViewMode.Edit);

        SetFreightDetail(Convert.ToInt32(Session["EnqId"]));

    }

    protected void btnFreightUpdate_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(FVFreightDetail.SelectedValue);

        int intFreightMode = Convert.ToInt32(hdnModeId.Value); // 1 For Air,2 Sea And 3 Breakbulk

        string strCustRefNo, strCustomer, strShipper, strConsignee, strAgentName, strRemarks;
        int FreightType, SalesRepId, CountryId, PortOfLoadingId, PortOfDischargeId, TermsId;
        int Count20 = 0, Count40 = 0, ContainerType = 0, NoOfpackages = 0;
        string strLCLVolume = "0", strGrossWeight = "0", strChargeableWeight = "0", strContainerSubType = "";
        bool IsDangerousGood = false;

        CountryId = Convert.ToInt32(hdnCountryId.Value);
        PortOfLoadingId = Convert.ToInt32(hdnLoadingPortId.Value);
        PortOfDischargeId = Convert.ToInt32(hdnPortOfDischargedId.Value);

        strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.ToUpper().Trim();
        strCustRefNo = ((TextBox)FVFreightDetail.FindControl("txtCustRefNo")).Text.Trim();

        strShipper = ((TextBox)FVFreightDetail.FindControl("txtShipper")).Text.ToUpper().Trim();
        strConsignee = ((TextBox)FVFreightDetail.FindControl("txtConsignee")).Text.ToUpper().Trim();
        strAgentName = ((TextBox)FVFreightDetail.FindControl("txtAgent")).Text.Trim();

        FreightType = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddFreightType")).SelectedValue);
        TermsId = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddTerms")).SelectedValue);
        TextBox txtGrossWeight = (TextBox)FVFreightDetail.FindControl("txtGrossWeight");
        TextBox txtChargWeight = (TextBox)FVFreightDetail.FindControl("txtChargWeight");
        IsDangerousGood = Convert.ToBoolean(((RadioButtonList)FVFreightDetail.FindControl("rdlGoodsType")).SelectedValue);
        strRemarks = ((TextBox)FVFreightDetail.FindControl("txtRemark")).Text.Trim();

        SalesRepId = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddSalesRep")).SelectedValue);

        if (strCustomer == "")
        {
            lblError.Text = "Please Enter Customer Name!.";
            lblError.CssClass = "errorMsg";
            return;
        }
        else if (strCustRefNo == "")
        {
            lblError.Text = "Please Enter Ref No/Email Ref!.";
            lblError.CssClass = "errorMsg";
            return;
        }
        else if (SalesRepId == 0)
        {
            lblError.Text = "Please Select Sales Reps Name!.";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (intFreightMode == 1) // AIR
        {
            TextBox txtNoOfPkgs = (TextBox)FVFreightDetail.FindControl("txtNoOfPkgs");

            if (txtNoOfPkgs.Text.Trim() != "")
            {
                NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
            }
        }
        else
        {
            ContainerType = Convert.ToInt32(((DropDownList)FVFreightDetail.FindControl("ddContainerType")).SelectedValue);
            strContainerSubType = ((DropDownList)FVFreightDetail.FindControl("ddSubType")).SelectedValue;

            TextBox txtCont20 = (TextBox)FVFreightDetail.FindControl("txtCont20");
            TextBox txtCont40 = (TextBox)FVFreightDetail.FindControl("txtCont40");
            TextBox txtLCLVolume = (TextBox)FVFreightDetail.FindControl("txtLCLVolume");

            if (txtCont20.Text.Trim() != "")
            {
                Count20 = Convert.ToInt32(txtCont20.Text.Trim());
            }

            if (txtCont40.Text.Trim() != "")
            {
                Count40 = Convert.ToInt32(txtCont40.Text.Trim());
            }

            if (txtLCLVolume.Text.Trim() != "")
            {
                strLCLVolume = txtLCLVolume.Text.Trim();
            }
        }

        if (txtGrossWeight.Text.Trim() != "")
        {
            strGrossWeight = txtGrossWeight.Text.Trim();
        }

        if (txtChargWeight.Text.Trim() != "")
        {
            strChargeableWeight = txtChargWeight.Text.Trim();
        }

        int result = DBOperations.UpdateFreightEnquiry(EnqId, intFreightMode, FreightType, strCustRefNo, strCustomer, strShipper, strConsignee,
            CountryId, PortOfLoadingId, PortOfDischargeId, TermsId, strAgentName, SalesRepId, Count20, Count40, ContainerType, strContainerSubType,
            strLCLVolume, NoOfpackages, strGrossWeight, strChargeableWeight, IsDangerousGood, strRemarks, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Freight Detail Updated Successfully!";
            lblError.CssClass = "success";

            FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);

            SetFreightDetail(EnqId);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Freight Detail Not Found!";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 3)
        {
            lblError.Text = "Ref No already exists. Please change Ref No/Email Ref No!.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnFreightCancel_Click(object sender, EventArgs e)
    {
        FVFreightDetail.ChangeMode(FormViewMode.ReadOnly);

        SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
    }

    protected void FVFreightDetail_DataBound(object sender, EventArgs e)
    {
        if (FVFreightDetail.CurrentMode == FormViewMode.ReadOnly)
        {
            if (hdnModeId.Value == "1") // AIR
            {
                Panel pnlAir = (Panel)(FVFreightDetail.FindControl("pnlAir"));

                if (pnlAir != null)
                    pnlAir.Visible = true;
            }
            else // Sea and Breakbulk
            {
                Panel pnlSea = (Panel)(FVFreightDetail.FindControl("pnlSea"));

                if (pnlSea != null)
                    pnlSea.Visible = true;
            }

        }
        else if (FVFreightDetail.CurrentMode == FormViewMode.Edit)
        {
            DropDownList ddSalesRep = (DropDownList)FVFreightDetail.FindControl("ddSalesRep");

            if (hdnModeId.Value == "1") // AIR
            {
                Panel pnlAirUpdate = (Panel)(FVFreightDetail.FindControl("pnlAirUpdate"));

                if (pnlAirUpdate != null)
                    pnlAirUpdate.Visible = true;
            }
            else // Sea and Breakbulk
            {
                Panel pnlSeaUpdate = (Panel)(FVFreightDetail.FindControl("pnlSeaUpdate"));

                if (pnlSeaUpdate != null)
                    pnlSeaUpdate.Visible = true;
            }

            if (ddSalesRep != null)// Edit Mode
            {
                DBOperations.FillBabajiUser(ddSalesRep);

                ddSalesRep.SelectedValue = hdnSalesRepId.Value;
            }
        }
    }

    protected void ddFreightMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddFreightMode = (DropDownList)(FVFreightDetail.FindControl("ddFreightMode"));
        hdnModeId.Value = ddFreightMode.SelectedValue;

        Panel pnlAirUpdate = (Panel)(FVFreightDetail.FindControl("pnlAirUpdate"));
        Panel pnlSeaUpdate = (Panel)(FVFreightDetail.FindControl("pnlSeaUpdate"));

        if (hdnModeId.Value == "1") // AIR
        {
            if (pnlAirUpdate != null)
                pnlAirUpdate.Visible = true;
            pnlSeaUpdate.Visible = false;
        }
        else // Sea and Breakbulk
        {
            if (pnlSeaUpdate != null)
                pnlSeaUpdate.Visible = true;
            pnlAirUpdate.Visible = false;
        }

    }
    #endregion

    #region Freight Document

    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
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

    protected void btnUpload_Click(Object sender, EventArgs e)
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

    #region Reminder

    protected void gvReminder_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "removeremind")
        {
            int ReminderId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightReminder(ReminderId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Reminder deleted successfully!";
                lblError.CssClass = "success";

                gvReminder.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Reminder. Status Is Closed.";
                lblError.CssClass = "errorMsg";

                gvReminder.DataBind();
            }
        }
    }

    protected void btnAddReminder_Click(object sender, EventArgs e)
    {
        int EnqId = 0;
        int RemindResult = -123;
        DateTime dtRemindDate = DateTime.MinValue;

        EnqId = Convert.ToInt32(Session["EnqId"]);

        if (EnqId > 0)
        {
            if (chkRemindMode.SelectedIndex == -1)
            {
                lblError.Text = "Please Select Reminder Type Email Or SMS!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (txtRemindDate.Text.Trim() != "")
            {
                int intDateCompare = DateTime.Compare(Convert.ToDateTime(txtRemindDate.Text.Trim()), DateTime.Today);

                if (intDateCompare < 0)
                {
                    lblError.Text = "Reminder date is earlier than today! Please change to future Date.";
                    lblError.CssClass = "errorMsg";
                    return;
                }
                else
                {
                    dtRemindDate = Commonfunctions.CDateTime(txtRemindDate.Text.Trim());
                }
            }

            if (chkRemindMode.Items[0].Selected) // Email
                RemindResult = DBOperations.AddFreightReminder(EnqId, 1, LoggedInUser.glUserId, dtRemindDate, txtRemindNote.Text.Trim(), LoggedInUser.glUserId);

            if (chkRemindMode.Items[1].Selected) // SMS
                RemindResult = DBOperations.AddFreightReminder(EnqId, 2, LoggedInUser.glUserId, dtRemindDate, txtRemindNote.Text.Trim(), LoggedInUser.glUserId);

            if (RemindResult == 0)
            {
                lblError.Text = "Reminder Added Successfully.";
                lblError.CssClass = "success";

                txtRemindDate.Text = "";
                txtRemindNote.Text = "";
                chkRemindMode.SelectedIndex = -1;

                gvReminder.DataBind();
            }
            else if (RemindResult == 1)
            {
                lblError.Text = "System Error. Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (RemindResult == 2)
            {
                lblError.Text = "Reminder Detail Already Added!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            Response.Redirect("FreightTracking.aspx");
        }
    }

    #endregion

    #region Agent
    
    private void FillEnquiryAgent(int EnqID)
    {
        DataSet dsEnquiryAgent = DBOperations.GetFreightAgent(EnqID);

        lbEnquiryAgent.DataSource       = dsEnquiryAgent;
        lbEnquiryAgent.DataTextField    = "DisplayName";
        lbEnquiryAgent.DataValueField   = "AgentID";
        lbEnquiryAgent.DataBind();
    }

    private void FillAgentDetail(int CountryID)
    {
        Int32 CompanyCategoryID = (Int32)EnumCompanyType.Agent;

        // Fill Agent Company by Country

        DBOperations.FillCompanyCategoryByCountryID(lbAgentCompany, CompanyCategoryID, CountryID);

        DBOperations.FillCompanyUserByCountryID(lbAgentContact, CompanyCategoryID, CountryID);
    }

    protected void btnSendAgentEmail_Click(object sender, EventArgs e)
    {
        if (Session["EnqID"] != null)
        {
            if (lbAgentContact.SelectedIndex != -1)
            {
                int EnqID = Convert.ToInt32(Session["EnqID"]);

                DataSet dsEnquiry = DBOperations.GetFreightDetail(EnqID);

                int AgentID = 0;
                string strAgentName = "", strAgentEmail = "";
                string EmailContent = "", strReturnMessage = "";
                string EmailBody = "", MessageBody = "", strSubject = "";

                string strEnqRefNo      = dsEnquiry.Tables[0].Rows[0]["ENQRefNo"].ToString();
                string strFreightType   = dsEnquiry.Tables[0].Rows[0]["TypeName"].ToString();
                string strFreightMode   = dsEnquiry.Tables[0].Rows[0]["ModeName"].ToString();
                string strCustRefNo     = dsEnquiry.Tables[0].Rows[0]["CustRefNo"].ToString();
                string strPortOfLoading = dsEnquiry.Tables[0].Rows[0]["LoadingPortName"].ToString();
                string strPortOfDischarge = dsEnquiry.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                string strTerms         = dsEnquiry.Tables[0].Rows[0]["TermsName"].ToString();
                string strNoOfPackages  = dsEnquiry.Tables[0].Rows[0]["NoOfPackages"].ToString();
                string strGrossWeight   = dsEnquiry.Tables[0].Rows[0]["GrossWeight"].ToString();
                string strRemark        = dsEnquiry.Tables[0].Rows[0]["Remarks"].ToString();

                strReturnMessage = "Enquiry Email For Agent Sent To:";

                try
                {
                    string strFileName = "../EmailTemplate/Freight_EmailAgent.txt";

                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message;
                    lblError.Text = strReturnMessage;
                    lblError.CssClass = "errorMsg";
                }

                // Email Template

                MessageBody = EmailContent.Replace("@EnqRefNo", strEnqRefNo);

                MessageBody = MessageBody.Replace("@FreightType", strFreightType);
                MessageBody = MessageBody.Replace("@FreightMode", strFreightMode);
                MessageBody = MessageBody.Replace("@CustRefNo", strCustRefNo);
                MessageBody = MessageBody.Replace("@PortOfLoading", strPortOfLoading);
                MessageBody = MessageBody.Replace("@PortOfDischarge", strPortOfDischarge);
                MessageBody = MessageBody.Replace("@Terms", strTerms);
                MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPackages);
                MessageBody = MessageBody.Replace("@GrossWeight", strGrossWeight);
                MessageBody = MessageBody.Replace("@Remark", strRemark);
                MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);

                foreach (ListItem item in lbAgentContact.Items)
                {
                    if (item.Selected)
                    {
                        AgentID = Convert.ToInt32(item.Value);

                        DataView dvEmpDetail = DBOperations.GetCustomerUserDetail(AgentID.ToString());

                        if (dvEmpDetail.Table.Rows.Count > 0)
                        {
                            strAgentName = dvEmpDetail.Table.Rows[0]["sName"].ToString();
                            strAgentEmail = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();

                            try
                            {
                                strSubject = "ENQ // " + strEnqRefNo + "// " + strPortOfLoading + "// TO // " + strPortOfDischarge + "//" +
                                strFreightMode + "// " + strCustRefNo;

                                MessageBody = MessageBody.Replace("@AgentName", strAgentName);

                                EmailBody = MessageBody;
                                //strAgentEmail = "amit.bakshi@babajishivram.com";

                                // Send Email To Agent and Copy To Freight SPC -- "amit.bakshi@babajishivram.com"
                                bool bMailSuccess = EMail.SendMail(LoggedInUser.glUserName, LoggedInUser.glUserName, strSubject, EmailBody, "");

                                int result = DBOperations.AddFreightAgent(EnqID, AgentID, bMailSuccess, LoggedInUser.glUserId);

                                if (bMailSuccess == true)
                                {
                                    strReturnMessage += ", " + strAgentName;
                                }
                                else
                                {
                                    strReturnMessage += "<br> Enquiry Sending Failed For - " + strAgentName;
                                }

                            }
                            catch (System.Exception ex)
                            {
                                strReturnMessage += ex.Message.ToString();
                                //return strReturnMessage;
                            }
                        }//END_IF_UserDetail

                    }// END_IF_Selected

                }//END_ForEach

                lblError.Text = strReturnMessage;
                lblError.CssClass = "errorMsg";
                FillEnquiryAgent(EnqID);
            }// END_IF_Selected != -1
            else
            {
                lblError.Text = "Please Select Agent Contact To Send Email";
                lblError.CssClass = "errorMsg";
                
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "Popup", "ShowPopup('" + lblError.Text + "');", true);
            }
        }//END_IF

    }

    #endregion


    #region Shared User
    protected void btnUpdParticipant_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        string strProjectEmpList = "";

        if (lbEmployee.SelectedIndex != -1)
        {
            List<string> UserIdList = new List<string>();

            foreach (ListItem emp in lbEmployee.Items)
            {
                if (emp.Selected)
                {
                    if (emp.Value != LoggedInUser.glUserId.ToString())
                    {
                        strProjectEmpList += emp.Value + ",";

                        UserIdList.Add(emp.Value);
                    }
                }
            }

            if (strProjectEmpList != "")
            {
                int resultProject = DBOperations.AddEnquiryUser(EnqId, strProjectEmpList, LoggedInUser.glUserId);

                if (resultProject == 0 && UserIdList.Count > 0)
                {
                    string strMessage = ParticipantsEmail(UserIdList);

                    lblError.Text = "Enquired Shared With Participant! " + strMessage;
                    lblError.CssClass = "success";
                }
            }
        }
    }

    protected string ParticipantsEmail(List<string> items)
    {
        string strEmpName = "", strEmpEmail = "", strRefNo = "";
        string EmailContent = "", strReturnMessage = "";
        string EmailBody = "", MessageBody = "", strSubject = "";
        string strCustomer = "";

        strRefNo = ((Label)FVFreightDetail.FindControl("lblEnquiryRefNo")).Text;

        if (FVFreightDetail.FindControl("lblCustomer") != null)
        {
            strCustomer = ((Label)FVFreightDetail.FindControl("lblCustomer")).Text.ToUpper().Trim();
        }
        else if (FVFreightDetail.FindControl("txtCustomer") != null)
        {
            strCustomer = ((TextBox)FVFreightDetail.FindControl("txtCustomer")).Text.ToUpper().Trim();
        }

        strReturnMessage = " Notification Email Sent To:";
        try
        {
            string strFileName = "../EmailTemplate/Freight_EmailEnqShared.txt";

            StreamReader sr = new StreamReader(Server.MapPath(strFileName));
            sr = File.OpenText(Server.MapPath(strFileName));
            EmailContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            strReturnMessage = ex.Message;
        }

        foreach (string UserId in items)
        {
            DataView dvEmpDetail = DBOperations.GetUserDetail(UserId);

            if (dvEmpDetail.Table.Rows.Count > 0)
            {
                strEmpEmail = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();
                strEmpName = dvEmpDetail.Table.Rows[0]["sName"].ToString();

                MessageBody = EmailContent.Replace("<EnqRefNo>", strRefNo);

                MessageBody = MessageBody.Replace("<EmpName>", LoggedInUser.glEmpName);

                try
                {
                    strSubject = "New Freight Enquiry Shared # " + strRefNo + " / Customer Name: " + strCustomer;

                    EmailBody = MessageBody;

                    EMail.SendMail(LoggedInUser.glUserName, strEmpEmail, strSubject, EmailBody, "");

                    strReturnMessage += "," + strEmpName;

                }
                catch (System.Exception ex)
                {
                    strReturnMessage = ex.Message.ToString();
                    return strReturnMessage;
                }
            }
        }//END_FOrEach

        return strReturnMessage;
    }

    #endregion

    #region Container Detail
    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int ContainerType; //1-FCL, 2 - LCL 
        int ContainerSize; //1 -Twenty, 2 - Forty

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

        if (EnqId > 0)
        {
            if (ContainerType == (Int16)EnumContainerType.FCL)
            {
                if (ContainerSize == 0)
                {
                    lblError.Text = "Please Select FCL Container Size!";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }
            else if (ContainerType == (Int16)EnumContainerType.LCL) //LCL
            {
                ddContainerSize.SelectedValue = "0";
                ContainerSize = 0;
            }

            if (ContainerNo != "")
            {
                int result = DBOperations.AddFreightContainerMS(EnqId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Container No. " + ContainerNo + " Added Successfully.";
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
                    lblError.Text = "Container No. " + ContainerNo + " Already Added!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = " Please Enter Container No.!";
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Freight Details Not Found.!";
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

            ddContainerSize.Items.Add(lstSelect);
            ddContainerSize.Items.Add(lstSelect20);
            ddContainerSize.Items.Add(lstSelect40);
        }
    }

    protected void gvContainer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDelete = (LinkButton)e.Row.Cells[6].Controls[2];

            //Delete Button index is 1 
            if (lnkDelete != null && lnkDelete.Text.ToLower() == "delete")
            {
                lnkDelete.OnClientClick = "return confirm('Do you really want to delete?');";

            }
        }
    }

    protected void gvContainer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.Visible = true;
        int ContainerSize, ContainerType;
        string ContainerNo = "";

        int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtContainerNo = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtEditContainerNo");
        DropDownList ddlContainerType = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerType");
        DropDownList ddlContainerSize = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerSize");

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
            int result = DBOperations.UpdateFreightContainerMS(lid, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Container Detail Updated Successfully!";
                lblError.CssClass = "success";
                e.Cancel = true;
                gvContainer.EditIndex = -1;
            }
            else if (result == 2)
            {
                lblError.Text = "Container No Already Exists!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = " Please Enter Container No!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvContainer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;

        int lid = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = DBOperations.DeleteFreightContainerMS(lid, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Container Deleted Successfully!";
            lblError.CssClass = "success";
            e.Cancel = true;
            gvContainer.DataBind();
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "success";
        }
    }

    #endregion

    protected void txtEnquiryValue_TextChanged(object sender, EventArgs e)
    {
        TextBox txtEnquiryValue = (TextBox)fvFreightStatus.FindControl("txtEnquiryValue");
        Label lblResult = (Label)fvFreightStatus.FindControl("lblResult");
        int Value = Convert.ToInt32(txtEnquiryValue.Text);
        if(Value==0)
        {
            lblResult.Text = "0 Value is not allow";
            lblResult.CssClass = "errorMsg";
            txtEnquiryValue.Text = "";
        }
        else
        {
            lblResult.Text = "";
        }

    }

    }