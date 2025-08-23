using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.TextFormatting;
using System.Text;
using BSImport.ConsigneeManager.BO;
using QueryStringEncryption;
using System.IO;
using BSImport.CountryManager.BO;
using System.Web.UI.DataVisualization.Charting;

public partial class Service_ApproveRejected_Vendor : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager2.RegisterPostBackControl(GridViewDoc);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Rejected Vendor Details";

            if (Session["lId"] != null)
            {
                GetVendorDetail(Convert.ToInt32(Session["lId"]));
            }
            BindVendorDetails();
            int lId = Convert.ToInt32(Session["vendorKYCID"]);
            VendorKYCOps.FillVendorKYCDOCMS(ddDocument);

        }

    }
    private void GetVendorDetail(int lId)
    {
        DataSet VnDetail = VendorKYCOps.GetRejectedVendorDetails(lId);
        if (VnDetail.Tables[0].Rows.Count > 0)
        {
            RejectedDetails.DataSource = VnDetail;
            RejectedDetails.DataBind();
        }

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        RejectedDetails.ChangeMode(FormViewMode.Edit);
        if (Session["lId"] != null)
        {
            GetVendorDetail(Convert.ToInt32(Session["lId"]));
          
        }

        HiddenField hdngsttype = ((HiddenField)RejectedDetails.FindControl("hdngsttype"));
        DropDownList ddlGSTRegType = ((DropDownList)RejectedDetails.FindControl("ddlGSTRegType"));
        var txtPanNo = (TextBox)RejectedDetails.FindControl("txtPanNo");
        var txtGSTN = (TextBox)RejectedDetails.FindControl("txtGSTN");
        Label lblGstn = ((Label)RejectedDetails.FindControl("lblGstn"));
        Label lblPan = ((Label)RejectedDetails.FindControl("lblPan"));
        var RFVIFSCCode = (RequiredFieldValidator)RejectedDetails.FindControl("RFVIFSCCode");
        hdngsttype.Value = ddlGSTRegType.SelectedValue;

        // Handle GSTN field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ")
        {
            txtGSTN.Visible = true;
            lblGstn.Visible = true;
            txtGSTN.Visible = true;
        }
        else
        {
            txtGSTN.Visible = false;
            txtGSTN.Visible = false;
            lblGstn.Visible = false;

        }

        // Handle Pan No field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "Un-Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ")
        {
            txtPanNo.Visible = true;
            lblPan.Visible = true;
        }
        else
        {
            txtPanNo.Visible = false;
            lblPan.Visible = false;

        }

        if (ddlGSTRegType.SelectedItem.Text == "Foreign")
        {
            RFVIFSCCode.Enabled = false;
        }
        else
        {
            RFVIFSCCode.Enabled = true;
        }

        HiddenField hdnCountry = ((HiddenField)RejectedDetails.FindControl("hdngsttype"));
        DropDownList ddlCountry = ((DropDownList)RejectedDetails.FindControl("ddlCountry"));
        HiddenField hdnState = ((HiddenField)RejectedDetails.FindControl("hdnState"));
        DropDownList ddlState = ((DropDownList)RejectedDetails.FindControl("ddlState"));
        Label lblState = ((Label)RejectedDetails.FindControl("lblState"));
        
        hdnCountry.Value = ddlCountry.SelectedValue;
        ddlState.SelectedItem.Text = hdnState.Value;

        if (ddlCountry.SelectedItem.Text == "India")
        {
            ddlState.Visible = true;
            lblState.Visible = true;
        }
        else
        {
            ddlState.Visible = false;
            lblState.Visible = false;
        }

        

    }
    private void BindVendorDetails()
    {
        DataSourceDocDownload.DataBind();
        GridViewDoc.DataBind();
        DataSourceRejectHistory.DataBind();  
        gvRejectHistory.DataBind();

    }
    protected void GetVendorDetails(int VendorKYCID)
    {
        DataView dtKYCDetail = VendorKYCOps.GetKYCDetailById(VendorKYCID);

        if (dtKYCDetail.Table.Rows.Count > 0)
        {
            hdnUploadPath.Value = dtKYCDetail.Table.Rows[0]["FileDirName"].ToString();

        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        int CreditDays = 0, VendorId = 0, ExetypeId = 0, StatusId = 0, VendorKycId = 0,  GSTRegType = 0 ,VendorType=0, VendorCategoryID=0;
        string CompanyName = "", ContactPerson = "", LegalName = "", TradeName = "", Address = "", Email = "", GSTN = "", Location = "", Division = "",
               KAM = "" , HOD = "", CCM = "", TDS = "", MSME = "", TDSCertificate = "", MSMECertificate = "", ExeCirteficateName = "", ExeCirteficatePath = "",
               BankName = "", MICRCode = "", IFSCCode = "", NameofBranch = "", BankAddress = "", Vendor = "", BranchCode = "", NineDigitCode = "", TypeOfAccount = "", RTGS = "",
               ContactNo = "", OfficeTeliphone = "", PanNo = "", GstCopyPath = "", GstCopyName = "", GSTCopy = "", BlankCopy = "", PANCopy = "", PanDocpath = "", PanDocName = "", BlankChequeName = "", BlankChequePath = ""
              , Country = "", State = "", City = "", Pincode = "", strRemark = "", AccountNo = ""; /*GSTRegTypeName = ""*/

        VendorKycId = Convert.ToInt32(Session["lId"]);

        var txtCompanyName = (TextBox)RejectedDetails.FindControl("txtCompanyName");
        var txtLegalName = (TextBox)RejectedDetails.FindControl("txtLegalName");
        var txtTradeName = (TextBox)RejectedDetails.FindControl("txtTradeName");
        var division = (((DropDownList)RejectedDetails.FindControl("ddlDivison")).SelectedItem.Text);

        var txtContactPerson = (TextBox)RejectedDetails.FindControl("txtContactPerson");
        var txtMobileNo = (TextBox)RejectedDetails.FindControl("txtMobileNo");
        var txtContactNo = (TextBox)RejectedDetails.FindControl("txtContactNo");
        var txtEmail = (TextBox)RejectedDetails.FindControl("txtEmail");
        var txtCreditDays = (TextBox)RejectedDetails.FindControl("txtCreditDays");
       
        var txtAddress = (TextBox)RejectedDetails.FindControl("txtAddress");
        var txtCountry = (TextBox)RejectedDetails.FindControl("txtCountry");
        var txtState = (TextBox)RejectedDetails.FindControl("txtState");
        var txtCity = (TextBox)RejectedDetails.FindControl("txtCity");
        var txtPincode = (TextBox)RejectedDetails.FindControl("txtPincode");
        var txtAccountNo = (TextBox)RejectedDetails.FindControl("txtAccountNo");
        var txtBankName = (TextBox)RejectedDetails.FindControl("txtBankName");
        var txtIFSCCode = (TextBox)RejectedDetails.FindControl("txtIFSCCode");
        var txtMICRCode = (TextBox)RejectedDetails.FindControl("txtMICRCode");
      //  var txtTypeOfAccount = (TextBox)RejectedDetails.FindControl("txtTypeOfAccount");
        var Accounttype = (((DropDownList)RejectedDetails.FindControl("ddlAccounttype")).SelectedItem.Text);


        var txtPanNo = (TextBox)RejectedDetails.FindControl("txtPanNo");
        var txtGSTN = (TextBox)RejectedDetails.FindControl("txtGSTN");
        var txtDate = (TextBox)RejectedDetails.FindControl("txtDate");
        var txtremark = (TextBox)RejectedDetails.FindControl("txtremark");
        var txtKamName = (TextBox)RejectedDetails.FindControl("txtKamName");

        var HODName = (((DropDownList)RejectedDetails.FindControl("dlHOD")).SelectedItem.Text);
        DropDownList GSTRegTypeName = (DropDownList)RejectedDetails.FindControl("ddlGSTRegType");
        var CountryName = (((DropDownList)RejectedDetails.FindControl("ddlCountry")).SelectedItem.Text);
        var StateName= (((DropDownList)RejectedDetails.FindControl("ddlState")).SelectedItem.Text);
        var vendortype = Convert.ToInt32(((DropDownList)RejectedDetails.FindControl("ddlvendortype")).SelectedValue);

        CompanyName = txtCompanyName.Text.Trim();
        LegalName = txtLegalName.Text.Trim();
        VendorCategoryID = Convert.ToInt32(vendortype);
        TradeName = txtTradeName.Text.Trim();
        Division = division;
        ContactPerson = txtContactPerson.Text.Trim();
        OfficeTeliphone = txtMobileNo.Text.Trim();
        ContactNo = txtContactNo.Text.Trim();
        Email = txtEmail.Text.Trim();
        CreditDays = Convert.ToInt32(txtCreditDays.Text);
        PanNo = txtPanNo.Text.Trim();
        GSTN = txtGSTN.Text.Trim();
        Address = txtAddress.Text.Trim();
        Country = CountryName;
        State = StateName;
        City= txtCity.Text.Trim();
        Pincode = txtPincode.Text.Trim();
        AccountNo = txtAccountNo.Text.Trim();
        BankName = txtBankName.Text.Trim();
        IFSCCode = txtIFSCCode.Text.Trim();
        MICRCode = txtMICRCode.Text.Trim();
        TypeOfAccount = Accounttype;
     //   GSTRegType = Convert.ToInt32(GSTRegTypeName);     
        HOD = HODName;
        KAM = txtKamName.Text.Trim();

        strRemark = txtremark.Text;

        int result = VendorKYCOps.UpdateRejectedVendor(VendorKycId, ContactNo, OfficeTeliphone, CreditDays, Convert.ToInt32(GSTRegTypeName.SelectedValue), CompanyName, Vendor, ContactPerson, LegalName, TradeName, Address,
        Email, GSTN, Division, KAM, HOD, CCM, TDS, MSME, TDSCertificate, MSMECertificate, AccountNo, BankName, IFSCCode, MICRCode,
        BranchCode, TypeOfAccount, VendorId, ExeCirteficatePath, ExeCirteficateName, ExetypeId, StatusId, PanNo, GSTCopy, BlankCopy, PANCopy, Country, State, City, Pincode, strRemark, VendorType, VendorCategoryID, LoggedInUser.glUserId);


        if (result == 0)
        {
            lblError.Text = "Vendor Details Updated Successfully For Approval!";
            lblError.CssClass = "errorMsg";

            // Add Status History
            VendorKYCOps.AddKYCStatus(VendorKycId, 200, strRemark,"", LoggedInUser.glUserId);

            // Upload Document

          // UploadKYCDocument(VendorKycId);

            Session["VendorKYCSuccess"] = lblError.Text;

            Response.Redirect("ServiceSuccess.aspx");

            ResetControls();
        }
        else if (result == 1)
        {
            lblError.Text = "Error updating vendor details.";
            lblError.ForeColor = System.Drawing.Color.Red;

        }
    }
        
    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("RejectedVendor_Status.aspx");
    }
    protected void btnCancelButton_Click(object sender, EventArgs e)
    {
        ResetControls();
    }
    private void ResetControls()
    {
        RejectedDetails.ChangeMode(FormViewMode.ReadOnly);
        if (Session["lId"] != null)
        {
            GetVendorDetail(Convert.ToInt32(Session["lId"]));
        }
    }

    protected void FVRejectedDetails_DataBound(object sender, EventArgs e)
    {
        if (RejectedDetails.CurrentMode == FormViewMode.Edit)
        {

            {
                DropDownList ddlHOD = ((DropDownList)RejectedDetails.FindControl("dlHOD"));

                  HiddenField hdnhod = (HiddenField)RejectedDetails.FindControl("hdnhod");

              //  int DivisonId = Convert.ToInt32(ddlHOD.SelectedValue);

                if (ddlHOD != null)
                {
                    VendorKYCOps.FillBabajiUserByDivisonID(ddlHOD, 18);
                    ddlHOD.SelectedItem.Text = hdnhod.Value;

                }
            }
            {
                DropDownList ddlvendortype = ((DropDownList)RejectedDetails.FindControl("ddlvendortype"));

                HiddenField hdnVendortype = (HiddenField)RejectedDetails.FindControl("hdnVendortype");

                if (ddlvendortype != null)
                {
                    VendorKYCOps.FillVendorCatagory(ddlvendortype);
                    ddlvendortype.SelectedValue = hdnVendortype.Value;

                }
            }
            {
                DropDownList ddlCountry = ((DropDownList)RejectedDetails.FindControl("ddlCountry"));

                HiddenField hdnCountry = (HiddenField)RejectedDetails.FindControl("hdnCountry");

                if (ddlCountry != null)
                {
                    VendorKYCOps.FillCountry(ddlCountry);
                    ddlCountry.SelectedItem.Text = hdnCountry.Value;

                }
            }
            {
                DropDownList ddlState = ((DropDownList)RejectedDetails.FindControl("ddlState"));

                HiddenField hdnState = (HiddenField)RejectedDetails.FindControl("hdnState");

                if (ddlState != null)
                {
                    VendorKYCOps.FillStates(ddlState);
                    //  ddlState.SelectedValue = hdnState.Value;
                    ddlState.SelectedItem.Text = hdnState.Value;
                }
            }

            {
                DropDownList ddlGSTRegType = ((DropDownList)RejectedDetails.FindControl("ddlGSTRegType"));

                HiddenField hdngsttype = (HiddenField)RejectedDetails.FindControl("hdngsttype");

                if (ddlGSTRegType != null)
                {
                    VendorKYCOps.FillGSTRegType(ddlGSTRegType);
                    ddlGSTRegType.SelectedValue = hdngsttype.Value;
                    ddlGSTRegType.DataBind();   
                   // ddlGSTRegType.SelectedItem.Text = hdngsttype.Value;


                }
            }
            {
                DropDownList ddlDivison = ((DropDownList)RejectedDetails.FindControl("ddlDivison"));

                HiddenField hdnDivision = (HiddenField)RejectedDetails.FindControl("hdnDivision");

                if (ddlDivison != null)
                {
                    VendorKYCOps.FillDivision(ddlDivison);
                    //    ddlGSTRegType.SelectedValue = hdngsttype.Value;
                    ddlDivison.SelectedItem.Text = hdnDivision.Value;


                }
            }
            {
                DropDownList ddlAccounttype = ((DropDownList)RejectedDetails.FindControl("ddlAccounttype"));

                HiddenField hdnAccounttype = (HiddenField)RejectedDetails.FindControl("hdnAccounttype");

                if (ddlAccounttype != null)
                {
                    VendorKYCOps.FillAccountType(ddlAccounttype);
                    //    ddlGSTRegType.SelectedValue = hdngsttype.Value;
                    ddlAccounttype.SelectedItem.Text = hdnAccounttype.Value;


                }
            }
        }
    }

    //protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Download")
    //    {
    //        string docPath = e.CommandArgument.ToString();
    //        DownloadDocument(docPath);
    //    }
    //}
    protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string docPath = e.CommandArgument.ToString();

            /////////////////////////////
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = VendorKYCOps.GetVendorKYCDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                lblError.Text = "";

                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["DocPath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + "\\" + strFileName;
                DownloadDocument(strDocpath);
            }

        }
    }
    #region Document
    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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
            lblError.Text = "Download Document System Error!";
        }
    }
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            string strURL = "ViewDoc.aspx?ref= " + DocumentPath;

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        int VendorKYCID = Convert.ToInt32(Session["lId"]);
        int DocumentId = Convert.ToInt32(ddDocument.SelectedValue);

        GetVendorDetails(VendorKYCID);

        string strFilePath = hdnUploadPath.Value;

        if (strFilePath == "")
            strFilePath = "Other";

        string strKYCFilePath = "VendorKYC//" + strFilePath + "//";

        int Result = -123;
        string strFileName = "";

        if (fuDocument.FileName.Trim() == "")
        {
            lblError.Text = "Please Browse The Document!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (DocumentId == 0)
        {
            lblError.Text = "Please Select Document Type!";
            lblError.CssClass = "errorMsg";
            return;
        }

        strFileName = UploadDocument(strKYCFilePath);

        if (strFileName != "")
        {
            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strFileName, strKYCFilePath, DocumentId, LoggedInUser.glUserId);
            Result = 0;
        }
        else
        {
            Result = 1;
        }

        if (Result == 0)
        {
            lblError.Text = "Document uploaded successfully!";
            lblError.CssClass = "success";
            GridViewDoc.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "Please Select File!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "Document not uploaded. Please try again!";
            lblError.CssClass = "errorMsg";
        }
    }

    private string UploadDocument(string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
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

            return FileName;
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
    #region Text Change Events
    protected void ddlGSTRegType_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hdngsttype = ((HiddenField)RejectedDetails.FindControl("hdngsttype"));
        DropDownList ddlGSTRegType = ((DropDownList)RejectedDetails.FindControl("ddlGSTRegType"));
        var txtPanNo = (TextBox)RejectedDetails.FindControl("txtPanNo");
        var txtGSTN = (TextBox)RejectedDetails.FindControl("txtGSTN");
        Label lblGstn = ((Label)RejectedDetails.FindControl("lblGstn"));
        Label lblPan = ((Label)RejectedDetails.FindControl("lblPan"));
        var RFVIFSCCode = (RequiredFieldValidator)RejectedDetails.FindControl("RFVIFSCCode");
        hdngsttype.Value = ddlGSTRegType.SelectedValue;

        // Handle GSTN field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ")
        {
            txtGSTN.Visible = true;
            lblGstn.Visible = true;
            txtGSTN.Visible = true;
        }
        else
        {
            txtGSTN.Visible = false;
            txtGSTN.Visible = false;
            lblGstn.Visible = false;

        }

       // Handle Pan No field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "Un-Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ")
        {
            txtPanNo.Visible = true;
            lblPan.Visible = true;
        }
        else
        {
            txtPanNo.Visible = false;
            lblPan.Visible = false;

        }
        if(ddlGSTRegType.SelectedItem.Text=="Foreign")
        {
            RFVIFSCCode.Enabled = false;
        }
        else
        {
            RFVIFSCCode.Enabled = true;
        }
    }

    //int countryId = Convert.ToInt32(ddlCountry.SelectedValue);
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hdnCountry = ((HiddenField)RejectedDetails.FindControl("hdngsttype"));
        DropDownList ddlCountry = ((DropDownList)RejectedDetails.FindControl("ddlCountry"));
        HiddenField hdnState = ((HiddenField)RejectedDetails.FindControl("hdnState"));
        DropDownList ddlState = ((DropDownList)RejectedDetails.FindControl("ddlState"));
        Label lblState = ((Label)RejectedDetails.FindControl("lblState"));

        hdnCountry.Value = ddlCountry.SelectedValue;
        ddlState.SelectedItem.Text = hdnState.Value;

        if (ddlCountry.SelectedItem.Text == "India")
        {
            ddlState.Visible = true;
            lblState.Visible = true;
        }
        else
        {
            ddlState.Visible = false;
            lblState.Visible = false;
        }
    }
    #endregion
}
