using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TransportTrack;

public partial class Service_VendorKYC_Details : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(PanCopy);

        if (!IsPostBack)
        {           
            VendorKYCOps.FillBabajiUserByDivisonID(ddHOD, 18);
            VendorKYCOps.FillCountry(ddlCountry);
            VendorKYCOps.FillStates(ddlState);
            VendorKYCOps.FillVendorCatagory(ddlvendorcatagory);         
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int VendorKycId = 0;

        int KYCTypeID = 0; // 1 - Vendor / 2 Customer

        KYCTypeID = Convert.ToInt32(ddlVendor.SelectedValue);
        int VendorCatagoryId = Convert.ToInt32(ddlvendorcatagory.SelectedValue);

        int CreditDays = 0, GSTRegType = 0;
        Boolean IsTDS = false, IsMSME = false;

        string CompanyName = "", ContactPerson = "", LegalName = "", TradeName = "", Address = "", Email = "", 
            GSTN = "", Division = "", KAM = "", HOD = "", CCM = "", 
            BankName = "", MICRCode = "", IFSCCode = "", TypeOfAccount = "", 
            ContactNo = "", OfficeTeliphone = "", PanNo = "", 
            PANCopy = "", Country = "", State = "", City = "", Pincode = "", AccountNo = "";

        string strRemark = "";
        string ExemptCertificatePath = "", ExemptCirteficateName = "";
        int ExemptTypeId = 0;

        CompanyName = txtCompanyName.Text.Trim();
        ContactPerson = txtContactPerson.Text.Trim();
        LegalName = txtLegalName.Text.Trim();
        TradeName = txtTradeName.Text.Trim();
        Address = txtAddress.Text.Trim();
        Email = txtEmail.Text.Trim();
        GSTN = txtGSTN.Text.Trim();
        
        Division = ddlDivision.Text.Trim();
        KAM = txtKAM.Text.Trim();
        HOD = ddHOD.SelectedItem.Text;
        CCM = txtCCM.Text.Trim();
        BankName = txtBankName.Text.Trim();
        MICRCode = txtMICR.Text.Trim();
        IFSCCode = txtIFSC.Text.Trim();
        PanNo = txtPan.Text.Trim();

        TypeOfAccount   =   txtAccountType.Text.Trim();
        ContactNo       =   txtContactNo.Text.Trim();
        OfficeTeliphone =   txtOfficeTelephone.Text.Trim();
        CreditDays      =   Convert.ToInt32(txtCrDays.Text);
        GSTRegType      =   Convert.ToInt32(ddlGSTRegType.Text);
        AccountNo       =   txtAcctNo.Text.Trim();

        Country         =   ddlCountry.SelectedItem.ToString();
        State           =   ddlState.SelectedItem.ToString();
        City            =   txtCity.Text;
        Pincode         =   txtPincode.Text;
      //strRemark       =   txtRemark.Text;
        // Check if the TDS radio buttons are selected

        if (rblTDS.SelectedIndex == -1)
        {
            lblError.Text = "Please Select TDS Applicable Yes/No ?";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (rblMSME.SelectedIndex == -1)
        {
            lblError.Text = "Please Select MSME Applicable Yes/No ?";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (rblTDS.SelectedValue == "1")
        {
            IsTDS = true;

            if(fuTDSCirteficate.HasFile == false)
            {
                lblError.Text = "Please Upload TDS Certificate!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        // Check if the MSME radio buttons are selected
        if (rblMSME.SelectedValue == "1")
        {
            IsMSME = true;

            if (fuMSMECirteficate.HasFile == false)
            {
                lblError.Text = "Please Upload MSME Certificate!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }
        
        VendorKycId = VendorKYCOps.AddVendorKYC(KYCTypeID, CompanyName, VendorCatagoryId, ContactPerson, ContactNo, 
            OfficeTeliphone, CreditDays, GSTRegType, LegalName, TradeName, Address, Email, GSTN, Division, KAM,HOD, 
            CCM, IsMSME, IsTDS, AccountNo, BankName, MICRCode, IFSCCode, TypeOfAccount, PanNo, Country,
            State, City, Pincode, strRemark,loggedInUser.glUserId);

        //BankAddress,NineDigitCode,NameofBranch,RTGS,GstCopyPath, GstCopyName,BlankChequePath, BlankChequeName, BranchCode, 

        if (VendorKycId > 0)
        {
            lblError.Text = "Vendor Details Successfully Submitted For Approval!";
            lblError.CssClass = "errorMsg";

            strRemark = txtRemark.Text;
            // Add Status History
            VendorKYCOps.AddKYCStatus(VendorKycId,200,strRemark,"",loggedInUser.glUserId);

            // Upload Document

            UploadKYCDocument(VendorKycId);

            Session["VendorKYCSuccess"] = lblError.Text;

            Response.Redirect("ServiceSuccess.aspx");
        }
        else if (VendorKycId == -1)
        {
            lblError.Text = "System Error! Please Try After Some Time.";
            lblError.CssClass = "errorMsg";
        }
        else if (VendorKycId == -2)
        {
            lblError.Text = "Vendor Name Already Exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Some Time.";
            lblError.CssClass = "errorMsg";
        }
    }

    private void UploadKYCDocument(int VendorKYCID)
    {
        int DocTypeID = 0;
        DataView dtKYCDetail = VendorKYCOps.GetKYCDetailById(VendorKYCID);
        String KYCDirName = "Other";

        if (dtKYCDetail.Table.Rows.Count > 0)
        {
            KYCDirName = dtKYCDetail.Table.Rows[0]["FileDirName"].ToString();
        }

        string strKYCFilePath = "VendorKYC//" + KYCDirName + "//";

        if(fuTDSCirteficate.HasFile)
        {
            // TDS Certificate

            string strTDSCertFileName  = UploadDocument(fuTDSCirteficate, strKYCFilePath);
            DocTypeID = (Int32)EnumVendorKYCDocType.TDSCertificate;

            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strTDSCertFileName, strKYCFilePath, DocTypeID, loggedInUser.glUserId);
        }
        if (fuMSMECirteficate.HasFile)
        {
            // MSME Certificate

            string strTDSCertFileName = UploadDocument(fuMSMECirteficate, strKYCFilePath);
            DocTypeID = (Int32)EnumVendorKYCDocType.MSMECertificate;

            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strTDSCertFileName, strKYCFilePath, DocTypeID, loggedInUser.glUserId);
        }
        if (fuPanCopy.HasFile)
        {
            // Pan Copy Certificate

            string strTDSCertFileName = UploadDocument(fuPanCopy, strKYCFilePath);
            DocTypeID = (Int32)EnumVendorKYCDocType.PANCopy;

            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strTDSCertFileName, strKYCFilePath, DocTypeID, loggedInUser.glUserId);
        }
        if (fuGstCopy.HasFile)
        {
            // Gst Copy Certificate

            string strTDSCertFileName = UploadDocument(fuGstCopy, strKYCFilePath);
            DocTypeID = (Int32)EnumVendorKYCDocType.GSTCertificate;

            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strTDSCertFileName, strKYCFilePath, DocTypeID, loggedInUser.glUserId);
        }
        if (fuBlankChequecopy.HasFile)
        {
            // Blank Cheque Certificate

            string strTDSCertFileName = UploadDocument(fuBlankChequecopy, strKYCFilePath);
            DocTypeID = (Int32)EnumVendorKYCDocType.BlankCheque;

            VendorKYCOps.AddVendorKYCDocument(VendorKYCID, strTDSCertFileName, strKYCFilePath, DocTypeID, loggedInUser.glUserId);
        }
    }

    //Cancle Operation
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
        btnSubmit.Text = "Save";
    }

    private void ResetControls()
    {
        txtCompanyName.Text = "";
        ddlDivision.SelectedIndex = 0; 
        txtContactPerson.Text = "";
        txtLegalName.Text = "";
        txtTradeName.Text = "";
        txtAddress.Text = "";
        txtEmail.Text = "";
        txtGSTN.Text = "";
        txtKAM.Text = "";
        txtCCM.Text = "";
        txtBankName.Text = "";
        txtMICR.Text = "";
        txtIFSC.Text = "";
        txtAccountType.Text = "";
        txtContactNo.Text = "";
        txtOfficeTelephone.Text = "";
        txtCrDays.Text = "";
        txtAcctNo.Text = "";
        txtPan.Text = "";
        txtPincode.Text = "";
        txtCity.Text = "";
        txtRemark.Text ="";
        ddlGSTRegType.SelectedIndex = 0;
        ddHOD.SelectedIndex=0;
        ddlCountry.SelectedIndex = 0;

        rblTDS.SelectedIndex = -1;
        rblMSME.SelectedIndex = -1;
        
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlvendorcatagory.SelectedIndex = 0;
        UPDTDS.Text = "";    // Clear upload status labels
        UPDTDS.Visible = false;
        UPDMSME.Text = "";
        UPDMSME.Visible = false;

    }

    protected void ddlGSTRegType_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handle GSTN field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ") 
        {
            txtGSTN.Enabled = true;
            lblGstn.Visible = true;
            txtGSTN.Visible = true;
        }
        else
        {
            txtGSTN.Enabled = false;
            txtGSTN.Visible = false;
            lblGstn.Visible = false;
          //  txtGSTN.Text = ""; 
        }

        // Handle Pan No field visibility
        if (ddlGSTRegType.SelectedItem.Text == "Registered" || ddlGSTRegType.SelectedItem.Text == "Un-Registered" || ddlGSTRegType.SelectedItem.Text == "SEZ") 
        {
            txtPan.Visible = true;
            lblPan.Visible = true;
        }
        else
        {
            txtPan.Visible = false;
            lblPan.Visible = false;
          //  txtPan.Text = ""; 
        }

        if(ddlGSTRegType.SelectedItem.Text == "Foreign")
        {
            RFVIFSC.Enabled = false;
        }
        else
        {
            RFVIFSC.Enabled = true;
        }
    }
    //   int countryId = Convert.ToInt32(ddlCountry.SelectedValue);
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
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


    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #region Upload documents

    private string UploadDocument(FileUpload fileUpload, string FilePath)
    {
        string FileName = fileUpload.FileName;
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
        if (fileUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fileUpload.SaveAs(ServerFilePath + FileName);

            return FileName;
        }

        else
        {
            return "";
        }

    }
  
    #endregion
    #region Documents Download 
    protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string docPath = e.CommandArgument.ToString();
            DownloadDocument(docPath);
        }
    }

    private void DownloadDocument(string docPath)
    {
        string ExeCirteficateName = Path.GetFileName(docPath); 
        string ExeCirteficatePath = Server.MapPath(docPath); 

        if (File.Exists(ExeCirteficatePath))
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + ExeCirteficateName);
            Response.TransmitFile(ExeCirteficatePath);
            Response.End();
        }
        else
        {
            lblError.Text = "File Not Found! Please Try After Some Time.";
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }

    #endregion
    private void BindGridView()
    {
        //  GridViewDoc.DataSource = DocumentSqlDataSource(); 
        //GridViewDoc.DataBind();
    }
    protected void rblTDS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(rblTDS.SelectedValue == "1")
        {
            fuTDSCirteficate.Visible = true;
            UPDTDS.Visible = true;
        }
        else
        {

            fuTDSCirteficate.Visible = false;
            UPDTDS.Visible = false;
            //  tdsbtnUpload.Visible = false;

        }
    }

    protected void rblMSME_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblMSME.SelectedValue == "1")
        {
            fuMSMECirteficate.Visible = true;
            UPDMSME.Visible = true;
            //   Msmebtnupd.Visible = true;

        }
        else
        {
            fuMSMECirteficate.Visible = false;
            UPDMSME.Visible = false;
            //  Msmebtnupd.Visible = false;
        }
    }

}

