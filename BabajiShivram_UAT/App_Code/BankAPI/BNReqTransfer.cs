using System.Collections.Generic;

/// <summary>
/// Summary description for BNReqTransfer
/// </summary>
namespace BankAPI.YesBank
{
    public class BNReqTransfer
    {
        public Transfer transfer { get; set; }
    }
    public class BeneficiaryAddress
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string stateOrProvince { get; set; }
        public string country { get; set; }

    }
    public class BeneficiaryContact
    {
        public string mobileNo { get; set; }
        public string emailID { get; set; }

    }
    public class BeneficiaryName
    {
        public string fullName { get; set; }
        public BeneficiaryAddress beneficiaryAddress { get; set; }
        public BeneficiaryContact beneficiaryContact { get; set; }
        public string beneficiaryAccountNo { get; set; }
        public string beneficiaryIFSC { get; set; }
        public string beneficiaryMobileNo { get; set; }
        public string beneficiaryMMID { get; set; }

    }
    public class AadhaarDetail
    {
        public string aadhaarNo { get; set; }
        public string mobileNo { get; set; }

    }
    public class BeneficiaryDetail
    {
        public BeneficiaryName beneficiaryName { get; set; }
        public AadhaarDetail aadhaarDetail { get; set; }

    }
    public class Beneficiary
    {
        public string beneficiaryCode { get; set; }
        public BeneficiaryDetail beneficiaryDetail { get; set; }
        public string transferType { get; set; }
        public string transferCurrencyCode { get; set; }
        public string transferAmount { get; set; }
        public string remitterToBeneficiaryInfo { get; set; }

    }
    public class Transfer
    {
        public string version { get; set; }
        public string uniqueRequestNo { get; set; }
        public string appID { get; set; }
        public string purposeCode { get; set; }
        public string customerID { get; set; }
        public string debitAccountNo { get; set; }
        public Beneficiary beneficiary { get; set; }

    }
}