using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.DataVisualization.Charting;

/// <summary>
/// Summary description for VendorKYCOps
/// </summary>
public class VendorKYCOps
{
    public VendorKYCOps()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void FillVendorKYCDOCMS(DropDownList DropDown)
    {
        CDatabase.BindControls(DropDown, "SELECT lId,sName FROM VN_KYCDOCMS Where bDel= 0 ORDER BY sName", "sName", "lId");
    }
    public static void FillVendorCatagory(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "VN_GetallVendorcatagory", command, "VendorType", "lId"); //
    }

    public static int AddVendorKYC(int KYCTypeID, string VendorName, int VendorCategoryID, string ContactPerson,  string ContactNo, string OfficeTeliphone, int CreditDays, int GSTRegType, 
    string LegalName, string TradeName, string Address, string Email, string GSTN, string Division, string KAM, string HOD, string CCM, bool IsMSME, bool IsTDS, 
    string AccountNo, string BankName, string MICRCode, string IFSCCode, string TypeOfAccount, 
    string PanNo, string Country,string State, string City, string Pincode, string Remark, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@KYCTypeID", SqlDbType.Int).Value = KYCTypeID;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = VendorName;
        command.Parameters.Add("@VendorCategoryID", SqlDbType.Int).Value = VendorCategoryID;
        
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;

        command.Parameters.Add("@LegalName", SqlDbType.NVarChar).Value = LegalName;
        command.Parameters.Add("@TradeName", SqlDbType.NVarChar).Value = TradeName;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@OfficeTelephone", SqlDbType.NVarChar).Value = OfficeTeliphone;

        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@GSTN", SqlDbType.NVarChar).Value = GSTN;
        command.Parameters.Add("@GSTRegTypeId", SqlDbType.Int).Value = GSTRegType;

        command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = Division;
        command.Parameters.Add("@CreditDays", SqlDbType.Int).Value = CreditDays;
        
        command.Parameters.Add("@IsMSME", SqlDbType.Bit).Value = IsMSME;
        command.Parameters.Add("@IsTDS", SqlDbType.Bit).Value = IsTDS;
        command.Parameters.Add("@HODName", SqlDbType.NVarChar).Value = HOD;
        command.Parameters.Add("@CCMName", SqlDbType.NVarChar).Value = CCM;
        command.Parameters.Add("@KAMName", SqlDbType.NVarChar).Value = KAM;

        command.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = AccountNo;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@IFSCode", SqlDbType.NVarChar).Value = IFSCCode;
        command.Parameters.Add("@MICRCode", SqlDbType.NVarChar).Value = MICRCode;
        command.Parameters.Add("@AccountType", SqlDbType.NVarChar).Value = TypeOfAccount;

        command.Parameters.Add("@PanNo", SqlDbType.NVarChar).Value = PanNo;
        command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
        command.Parameters.Add("@State", SqlDbType.NVarChar).Value = State;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@Pincode", SqlDbType.NVarChar).Value = Pincode;

        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("VN_insVendorMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddKYCStatus(int VendorKYCId, int StatusId, string Remark, string CompanyCode, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";
        command.Parameters.Add("@VendorKYCId", SqlDbType.Int).Value = VendorKYCId;
        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = StatusId;
        command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = Remark;
        command.Parameters.Add("@CompanyCode", SqlDbType.NVarChar).Value = CompanyCode; //Added new Company Code For the Tranporter customer

        command.Parameters.Add("@lUser", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("VN_insStatusHistory", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }


    public static DataView GetKYCDetailById(int VendorKYCID)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Parameters.Add("@VendorKYCID", SqlDbType.Int).Value = VendorKYCID;

        return CDatabase.GetDataView("VN_GetVendorDetails", cmd);
    }

    public static int AddVendorKYCDocument(int KYCId, string filename, string DocPath, int DocumentId, int lUser)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorKYCId", SqlDbType.Int).Value = KYCId;
        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = filename;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocTypeID", SqlDbType.Int).Value = DocumentId;
        command.Parameters.Add("@UserId", SqlDbType.Int).Value = lUser;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("VN_insKYCDocPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static DataSet GetVendorKYCDocument(int VendorKYCIDID, int DocId)
    {
        // Get All Job Document Set DocumentId = 0
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@VendorKYCID", SqlDbType.Int).Value = VendorKYCIDID;
        command.Parameters.Add("@DocId", SqlDbType.Int).Value = DocId;

        return CDatabase.GetDataSet("VN_GetKYCDocument", command);
    }
    public static int UpdateRejectedVendor(int KycVendorId, string ContactNo, string OfficeTeliphone, int CreditDays, int GSTRegType, string CompanyName, string Vendor, string ContactPerson,
       string LegalName, string TradeName, string Address, string Email, string GSTN, string Division, string KAM, string HOD, string CCM, string MSME, string TDS, string TDSCertificate,
       string MSMECertificate, string AccountNo, string BankName,  string IFSCCode, string MICRCode, string BranchCode
        , string TypeOfAccount, int VendorId, string ExeCirteficatePath, string ExeCirteficateName, int ExetypeId, int StatusId, string PanNo, string GSTCopy, string BlankCopy, string PANCopy, string Country,
         string State, string City, string Pincode, string strRemark,int VendorType,int VendorCategoryID, int lUser)
    {

        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@lid", SqlDbType.Int).Value = KycVendorId;
        command.Parameters.Add("@VendorName", SqlDbType.NVarChar).Value = CompanyName;
        command.Parameters.Add("@VendorCategoryID", SqlDbType.Int).Value = VendorCategoryID;

        command.Parameters.Add("@TradeName", SqlDbType.NVarChar).Value = TradeName;
        command.Parameters.Add("@LegalName", SqlDbType.NVarChar).Value = LegalName;
        command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = Division;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@OfficeTelephone", SqlDbType.NVarChar).Value = OfficeTeliphone;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@CreditDays", SqlDbType.Int).Value = CreditDays;
        command.Parameters.Add("@PanNo", SqlDbType.NVarChar).Value = PanNo;
        command.Parameters.Add("@GSTN", SqlDbType.NVarChar).Value = GSTN;
        command.Parameters.Add("@HODName", SqlDbType.NVarChar).Value = HOD;
        command.Parameters.Add("@KamName", SqlDbType.NVarChar).Value = KAM;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Country;
        command.Parameters.Add("@State", SqlDbType.NVarChar).Value = State;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@Pincode", SqlDbType.NVarChar).Value = Pincode;
        command.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = AccountNo;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@IFSCCode", SqlDbType.NVarChar).Value = IFSCCode;
        command.Parameters.Add("@MICRCode", SqlDbType.NVarChar).Value = MICRCode;
        command.Parameters.Add("@TypeOfAccount", SqlDbType.NVarChar).Value = TypeOfAccount;       
        command.Parameters.Add("@Remark", SqlDbType.NChar).Value = strRemark;
        command.Parameters.Add("@GSTRegTypeId", SqlDbType.Int).Value = GSTRegType;


        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("VN_updRejectedDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
    public static void FillCountry(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "KYC_GetCountryMS", command, "CountryName", "CountryId");
    }
    public static void FillStates(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "KYC_GetStateMS", command, "StateName", "StateId");
    }
    public static void FillBabajiUserByDivisonID(DropDownList DropDown, int DivisionID)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@DivisionID", SqlDbType.Int).Value = DivisionID;

        CDatabase.BindControls(DropDown, "GetUserByDivisionId", command, "UserName", "UserId");
    }
    public static void FillGSTRegType(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "VN_GetGSTTypeMS", command, "GSTRegTypeName", "lId"); //
    }
    public static void FillDivision(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "VN_GetDivisionType", command, "DivisionTypeName", "lId"); //
    }
    public static DataSet GetRejectedVendorDetails(int lId)
    {
        SqlCommand command = new SqlCommand();

        command.Parameters.Add("@VendorKycId", SqlDbType.Int).Value = lId;

        return CDatabase.GetDataSet("VN_GetRejectedVendorDetails", command);
    }
    public static void FillAccountType(DropDownList DropDown)
    {
        SqlCommand command = new SqlCommand();

        CDatabase.BindControls(DropDown, "VN_GetAccountType", command, "AccountType", "lId"); //
    }

}