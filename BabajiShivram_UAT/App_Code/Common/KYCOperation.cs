using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using BSImport;

/// <summary>
/// Summary description for KYCOperation
/// </summary>
public class KYCOperation
{
    public KYCOperation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataSet GetCountryByState(int StateId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@StateId", SqlDbType.Int).Value = StateId;
        return CDatabase.GetDataSet("KYC_GetCountryByState", command);
    }

    public static int AddVendorDetails(int lType, int StateId, int CountryId, int ConstitutionId, int SectorId, int NatureofBusinessId, string CompanyName,
        string Address1, string Address2, string City, string Pincode, string TelephoneNo, string FaxNo, string WebsiteAdd, string Email, string PANNo,
        string VATNo, string ServiceTaxNo, string ExciseNo, string CSTNo, string TANNo, string SSINo, string BankName, string AccountNo, string IFSCCode,
        string MICRCode, string IECCode, string PaymentTerms, string PANCopyPath, string IECCopyPath, string OtherCopyPath, string KYCCopyPath, string KYCScannedCopyPath,
        string BillingName, string BillingEmail, string BillingPhoneNo, string BillingAddress, string BillingPincode, string BillingCity, bool IsScanned, int lUserId,
        string IpAddress)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;
        command.Parameters.Add("@StateId", SqlDbType.Int).Value = StateId;
        command.Parameters.Add("@CountryId", SqlDbType.Int).Value = CountryId;
        command.Parameters.Add("@ConstitutionId", SqlDbType.Int).Value = ConstitutionId;
        command.Parameters.Add("@SectorId", SqlDbType.Int).Value = SectorId;
        command.Parameters.Add("@NatureofBusinessId", SqlDbType.Int).Value = NatureofBusinessId;
        command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = CompanyName;
        command.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = Address1;
        command.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = Address2;
        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
        command.Parameters.Add("@Pincode", SqlDbType.NVarChar).Value = Pincode;
        command.Parameters.Add("@TelephoneNo", SqlDbType.NVarChar).Value = TelephoneNo;
        command.Parameters.Add("@FaxNo", SqlDbType.NVarChar).Value = FaxNo;
        command.Parameters.Add("@WebsiteAdd", SqlDbType.NVarChar).Value = WebsiteAdd;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@PANNo", SqlDbType.NVarChar).Value = PANNo;
        command.Parameters.Add("@VATNo", SqlDbType.NVarChar).Value = VATNo;
        command.Parameters.Add("@ServiceTaxNo", SqlDbType.NVarChar).Value = ServiceTaxNo;
        command.Parameters.Add("@ExciseNo", SqlDbType.NVarChar).Value = ExciseNo;
        command.Parameters.Add("@CSTNo", SqlDbType.NVarChar).Value = CSTNo;
        command.Parameters.Add("@TANNo", SqlDbType.NVarChar).Value = TANNo;
        command.Parameters.Add("@SSINo", SqlDbType.NVarChar).Value = SSINo;
        command.Parameters.Add("@BankName", SqlDbType.NVarChar).Value = BankName;
        command.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = AccountNo;
        command.Parameters.Add("@IFSCCode", SqlDbType.NVarChar).Value = IFSCCode;
        command.Parameters.Add("@MICRCode", SqlDbType.NVarChar).Value = MICRCode;
        command.Parameters.Add("@IECCode", SqlDbType.NVarChar).Value = IECCode;
        command.Parameters.Add("@PaymentTerms", SqlDbType.NVarChar).Value = PaymentTerms;
        command.Parameters.Add("@PANCopyPath", SqlDbType.NVarChar).Value = PANCopyPath;
        command.Parameters.Add("@IECCopyPath", SqlDbType.NVarChar).Value = IECCopyPath;
        command.Parameters.Add("@OtherCopyPath", SqlDbType.NVarChar).Value = OtherCopyPath;
        command.Parameters.Add("@KYCCopyPath", SqlDbType.NVarChar).Value = KYCCopyPath;
        command.Parameters.Add("@KYCScannedCopyPath", SqlDbType.NVarChar).Value = KYCScannedCopyPath;
        command.Parameters.Add("@IsScanned", SqlDbType.Bit).Value = IsScanned;
        command.Parameters.Add("@BillingName", SqlDbType.NVarChar).Value = BillingName;
        command.Parameters.Add("@BillingEmail", SqlDbType.NVarChar).Value = BillingEmail;
        command.Parameters.Add("@BillingPhoneNo", SqlDbType.NVarChar).Value = BillingPhoneNo;
        command.Parameters.Add("@BillingAddress", SqlDbType.NVarChar).Value = BillingAddress;
        command.Parameters.Add("@BillingPincode", SqlDbType.NVarChar).Value = BillingPincode;
        command.Parameters.Add("@BillingCity", SqlDbType.NVarChar).Value = BillingCity;
        command.Parameters.Add("@IpAddress", SqlDbType.NVarChar).Value = IpAddress;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insVendorDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddGSTDetails(int VendorId, int BranchId, string Name, string Address, string PersonName, string MobileNo, string Email,
        string GSTProvisionNo, string ARNNo, int lUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@BranchId", SqlDbType.Int).Value = BranchId;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@PersonName", SqlDbType.NVarChar).Value = PersonName;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@GSTProvisionNo", SqlDbType.NVarChar).Value = GSTProvisionNo;
        command.Parameters.Add("@ARNNo", SqlDbType.NVarChar).Value = ARNNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insGSTDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddGSTCopyPath(int VendorId, int GSTDetailId, string DocPath, string DocName, int lUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@GSTDetailId", SqlDbType.Int).Value = GSTDetailId;
        command.Parameters.Add("@DocPath", SqlDbType.NVarChar).Value = DocPath;
        command.Parameters.Add("@DocName", SqlDbType.NVarChar).Value = DocName;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insGSTCopyDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddContactDetails(int VendorId, int lType, string Name, string Email, string MobileNo, string LandlineNo, int lUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@lType", SqlDbType.Int).Value = lType;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@LandlineNo", SqlDbType.NVarChar).Value = LandlineNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insContactDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddMaterialDetails(string MaterialSupplied, string CommodityName, string HSNCode, int VendorId, int lUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@MaterialSupplied", SqlDbType.NVarChar).Value = MaterialSupplied;
        command.Parameters.Add("@CommodityName", SqlDbType.NVarChar).Value = CommodityName;
        command.Parameters.Add("@HSNCode", SqlDbType.NVarChar).Value = HSNCode;
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insGstMaterialDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddServiceDetails(int VendorId, string ServiceProvided, string ServiceCatg, string SACCode, int lUserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@ServiceProvided", SqlDbType.NVarChar).Value = ServiceProvided;
        command.Parameters.Add("@ServiceCatg", SqlDbType.NVarChar).Value = ServiceCatg;
        command.Parameters.Add("@SACCode", SqlDbType.NVarChar).Value = SACCode;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insGstServiceDetails", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetVendorDetailById(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetVendorDetailById", command);
    }

    public static int UpdateKYCFormPath(int VendorId, string KYCCopyPath)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@KYCCopyPath", SqlDbType.NVarChar).Value = KYCCopyPath;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_updKYCFormPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    #region REPORTS 
    public static DataTable rpt_Vendor(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetVendorDetailById", command);
    }

    public static DataTable rpt_OperationContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetOperationContactDetails", command);
    }

    public static DataTable rpt_FinanceContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetFinanceContactDetails", command);
    }

    public static DataTable rpt_OtherContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetOtherContactDetails", command);
    }

    public static DataTable rpt_GSTDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetGSTDetails", command);
    }

    public static DataTable rpt_MaterialDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetGstMaterialDetails", command);
    }

    public static DataTable rpt_ServiceDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataTable("KYC_GetGstServiceDetails", command);
    }

    #endregion

    public static DataSet GetOperationContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetOperationContactDetails", command);
    }

    public static DataSet GetFinanceContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetFinanceContactDetails", command);
    }

    public static DataSet GetOtherContact(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetOtherContactDetails", command);
    }

    public static DataSet GetGSTDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetGSTDetails", command);
    }

    public static DataSet GetMaterialDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetGstMaterialDetails", command);
    }

    public static DataSet GetServicesDetails(int VendorId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        return CDatabase.GetDataSet("KYC_GetGstServiceDetails", command);
    }

    public static DataSet GetGSTCopyDetails(int GSTDetailId)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@GSTDetailId", SqlDbType.Int).Value = GSTDetailId;
        return CDatabase.GetDataSet("KYC_GetGSTCopyDetails", command);
    }

    public static int UpdateScannedKYCPath(int VendorId, string KYCCopyPath)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@KYCScannedCopyPath", SqlDbType.NVarChar).Value = KYCCopyPath;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_updScannedKYCPath", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int ApproveRejectKYC(int VendorId, int IsApproved, string RejectedRemark)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@IsApproved", SqlDbType.Int).Value = IsApproved;
        command.Parameters.Add("@RejectedRemark", SqlDbType.NVarChar).Value = RejectedRemark;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_updApproveRejectKYC", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomer(int VendorId, string CustName, string Address, string ContactPerson, string MobileNo, string ContactNo, string Email, string IECNo, string PANNo, int lUser, string CustCode)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@CustName", SqlDbType.NVarChar).Value = CustName;
        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
        command.Parameters.Add("@ContactPerson", SqlDbType.NVarChar).Value = ContactPerson;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = ContactNo;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        command.Parameters.Add("@IECNo", SqlDbType.NVarChar).Value = IECNo;
        command.Parameters.Add("@PANNo", SqlDbType.NVarChar).Value = PANNo;
        command.Parameters.Add("@lUser", SqlDbType.Int).Value = lUser;
        command.Parameters.Add("@CustCode", SqlDbType.NVarChar).Value = CustCode;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("KYC_insCustomerMS", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int UpdateVendorForEnquiry(int VendorId, int EnquiryId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";
        command.Parameters.Add("@VendorId", SqlDbType.Int).Value = VendorId;
        command.Parameters.Add("@EnquiryId", SqlDbType.Int).Value = EnquiryId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;
        SPresult = CDatabase.GetSPOutPut("CRM_updVendorForEnquiry", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static DataSet GetEnquiryByLid(int Lid)
    {
        SqlCommand command = new SqlCommand();
        command.Parameters.Add("@lid", SqlDbType.Int).Value = Lid;
        return CDatabase.GetDataSet("CRM_GetLeadEnquiryByLid", command);
    }

    public static int AddCustomerDivision(int CustomerId, string strDivisionName, string strDivisionCode, int UserId)
    {
        SqlCommand command = new SqlCommand();

        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionName", SqlDbType.NVarChar).Value = strDivisionName;
        command.Parameters.Add("@DivisionCode", SqlDbType.NVarChar).Value = strDivisionCode;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;

        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KYC_insCustomerDivision", command, "@OutPut");

        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerPlant(int CustomerId, int DivisonId, string strPlantName, string strPlantCode, string strGSTNNo, bool ChecklistApproval, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@DivisionId", SqlDbType.Int).Value = DivisonId;
        command.Parameters.Add("@PlantName", SqlDbType.NVarChar).Value = strPlantName;
        command.Parameters.Add("@PlantCode", SqlDbType.NVarChar).Value = strPlantCode;
        command.Parameters.Add("@GSTNNo", SqlDbType.NVarChar).Value = strGSTNNo;
        command.Parameters.Add("@ChecklistApproval", SqlDbType.NVarChar).Value = ChecklistApproval;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("KYC_insCustomerPlant", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }

    public static int AddCustomerUser(int CustomerId, string EmpName, string Designation, bool LoginActive, string Email, DateTime dtBirthDate, string Password,
                      string MobileNo, string FaxNo, string OfficeNo, string CityName, int CountryID, bool ResetCode, int ResetDays, int UserId)
    {
        SqlCommand command = new SqlCommand();
        string SPresult = "";

        command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CustomerId;
        command.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = EmpName;
        command.Parameters.Add("@Designation", SqlDbType.NVarChar).Value = Designation;
        command.Parameters.Add("@LoginActive", SqlDbType.Bit).Value = LoginActive;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
        if (dtBirthDate != DateTime.MinValue)
            command.Parameters.Add("@dtBirthDate", SqlDbType.DateTime).Value = dtBirthDate;
        command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;
        command.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = MobileNo;
        command.Parameters.Add("@FaxNo", SqlDbType.NVarChar).Value = FaxNo;
        command.Parameters.Add("@OfficeNo", SqlDbType.NVarChar).Value = OfficeNo;
        command.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = CityName;
        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
        command.Parameters.Add("@ResetCode", SqlDbType.Bit).Value = ResetCode;
        command.Parameters.Add("@ResetDays", SqlDbType.Int).Value = ResetDays;
        command.Parameters.Add("@lUserId", SqlDbType.Int).Value = UserId;
        command.Parameters.Add("@OutPut", SqlDbType.Int).Direction = ParameterDirection.Output;

        SPresult = CDatabase.GetSPOutPut("insCustomerUser", command, "@OutPut");
        return Convert.ToInt32(SPresult);
    }
}