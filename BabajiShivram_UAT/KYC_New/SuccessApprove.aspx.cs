using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class KYC_New_SuccessApprove : System.Web.UI.Page
{
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["KycVendorId"] != null)
            {
                if (Convert.ToString(Session["KycVendorId"]) != "")
                {
                    DataSet dsGetVendor = KYCOperation.GetVendorDetailById(Convert.ToInt32(Session["KycVendorId"]));
                    if (dsGetVendor != null && dsGetVendor.Tables[0].Rows.Count > 0)
                    {
                        if (dsGetVendor.Tables[0].Rows[0]["CompanyName"] != DBNull.Value)
                        {
                            if (Request.QueryString.ToString() != null && Request.QueryString.Count > 0)
                            {
                                if (Convert.ToString(Request.QueryString["op"]) == "1") // approve
                                {
                                    lblNote.Text = "Customer entry has been created successfully for " +
                                                    dsGetVendor.Tables[0].Rows[0]["CompanyName"].ToString().ToUpper().Trim() +
                                                    " . Please check for the same in Customer Tab.";

                                    lblMessage.Text = "Successfully approved KYC form!";
                                    AddCustomer(Convert.ToInt32(Session["KycVendorId"]));
                                }
                                else // reject
                                {
                                    lblNote.Text = "Customer entry rejected!";
                                    lblMessage.Text = "Successfully rejected KYC form!";
                                    SendApproval(Convert.ToInt32(Session["KycVendorId"]));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void AddCustomer(int VendorId)
    {
        if (VendorId > 0)
        {
            string CustomerCode = "", CustName = "", Address = "", ContactPerson = "", MobileNo = "", ContactNo = "", Email = "", IECNo = "", PANNO = "",
                KYC_OperationPerson = "", KYC_BillingAddress = "", KYC_BillingCity = "", KYC_BillingEmail = "", KYC_BillingPhoneNo = "", KYC_BillingPincode = "";
            int CreatedBy = 0, SectorId = 0, EnquiryId = 0, LeadId = 0;

            Random objrand = new Random();
            char[] arr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < 3; i++)
            {
                CustomerCode = CustomerCode + arr[objrand.Next(26)].ToString() + objrand.Next(10).ToString();
            }

            DataSet dsGetVendorDetails = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetVendorDetails != null && dsGetVendorDetails.Tables[0].Rows.Count > 0)
            {
                if (dsGetVendorDetails.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    CustName = dsGetVendorDetails.Tables[0].Rows[0]["CompanyName"].ToString();
                    Address = dsGetVendorDetails.Tables[0].Rows[0]["Address1"].ToString();
                    ContactPerson = dsGetVendorDetails.Tables[0].Rows[0]["FinanceContactPerson"].ToString();
                    MobileNo = dsGetVendorDetails.Tables[0].Rows[0]["FinanceContactMobileNo"].ToString();
                    ContactNo = dsGetVendorDetails.Tables[0].Rows[0]["FinanceContactLandlineNo"].ToString();
                    Email = dsGetVendorDetails.Tables[0].Rows[0]["FinanceContactEmail"].ToString();
                    IECNo = dsGetVendorDetails.Tables[0].Rows[0]["IECCode"].ToString();
                    PANNO = dsGetVendorDetails.Tables[0].Rows[0]["PANNo"].ToString();
                    CreatedBy = Convert.ToInt32(dsGetVendorDetails.Tables[0].Rows[0]["lUser"].ToString());
                    SectorId = Convert.ToInt32(dsGetVendorDetails.Tables[0].Rows[0]["SectorId"].ToString());
                    KYC_OperationPerson = dsGetVendorDetails.Tables[0].Rows[0]["OperationContactPerson"].ToString();
                    KYC_BillingAddress = dsGetVendorDetails.Tables[0].Rows[0]["BillingAddress"].ToString();
                    KYC_BillingCity = dsGetVendorDetails.Tables[0].Rows[0]["BillingCity"].ToString();
                    KYC_BillingEmail = dsGetVendorDetails.Tables[0].Rows[0]["BillingEmail"].ToString();
                    KYC_BillingPhoneNo = dsGetVendorDetails.Tables[0].Rows[0]["BillingPhoneNo"].ToString();
                    KYC_BillingPincode = dsGetVendorDetails.Tables[0].Rows[0]["BillingPincode"].ToString();
                    EnquiryId = Convert.ToInt32(dsGetVendorDetails.Tables[0].Rows[0]["EnquiryId"].ToString());
                    LeadId = Convert.ToInt32(dsGetVendorDetails.Tables[0].Rows[0]["EnquiryId"].ToString());

                    int CustomerId = KYCOperation.AddCustomer(VendorId, CustName.ToUpper().Trim(), Address, ContactPerson, MobileNo, ContactNo, Email, IECNo, PANNO, CreatedBy, CustomerCode);
                    if (CustomerId > 0)
                    {
                        // Update Lead Stage
                        int LeadStage = DBOperations.CRM_UpdateLeadStatus(LeadId, true, true, true, false, true, false, CreatedBy);

                        // update quote status
                        DataSet dsGetQuoteByEnquiry = DBOperations.CRM_GetQuoteByEnquiry(EnquiryId);
                        if (dsGetQuoteByEnquiry != null && dsGetQuoteByEnquiry.Tables[0].Rows[0]["QuotationId"] != DBNull.Value)
                        {
                            int Save = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(dsGetQuoteByEnquiry.Tables[0].Rows[0]["QuotationId"].ToString()), Convert.ToInt32(15), "",dtClose, CreatedBy);
                        }

                        int Result = DBOperations.AddCustomerSector(CustomerId, SectorId, CreatedBy);
                        int Result_Catg = DBOperations.AddCompanyCategory(CustomerId, 1, CreatedBy);
                        if (IECNo != "" && IECNo != "0")
                        {
                            //int Result_Consignee = DBOperations.AddCompanyCategory(CustomerId, 2, CreatedBy);
                            //int Result_Shipper = DBOperations.AddCompanyCategory(CustomerId, 4, CreatedBy);
                            int Result_Consignee = DBOperations.AddCompanyCategory(CustomerId, 2, CreatedBy);
                            int AddConsignee = DBOperations.AddCustomerConsignee(CustomerId, CustomerId, CreatedBy);
                            int Result_Shipper = DBOperations.AddCompanyCategory(CustomerId, 4, CreatedBy);
                            int AddShipper = DBOperations.AddCustomerShipper(CustomerId, CustomerId, CreatedBy);
                        }
                        int BranchDivisionId = KYCOperation.AddCustomerDivision(CustomerId, "CUSTOM CLEARANCE", CustomerCode.Substring(0, 2) + "CC01", CreatedBy);
                        if (BranchDivisionId != 2)
                        {
                            // Insert sub branch plant description
                            DataSet dsGetGSTDetails = KYCOperation.GetGSTDetails(VendorId);
                            if (dsGetGSTDetails != null && dsGetGSTDetails.Tables[0].Rows.Count > 0)
                            {
                                int Count = 0;
                                for (int i = 0; i < dsGetGSTDetails.Tables[0].Rows.Count; i++)
                                {
                                    Count = Count++;
                                    if (dsGetGSTDetails.Tables[0].Rows[i]["lid"].ToString() != "")
                                    {
                                        int Result_BranchPlant = KYCOperation.AddCustomerPlant(CustomerId, BranchDivisionId, dsGetGSTDetails.Tables[0].Rows[i]["Name"].ToString(),
                                           CustomerCode.Substring(0, 2) + "CCP" + Count.ToString(), dsGetGSTDetails.Tables[0].Rows[i]["GSTProvisionNo"].ToString(), false, CreatedBy);

                                        //Billing Address Details
                                        int Result_BPlantAddress = DBOperations.AddPlantAddress(Result_BranchPlant, 1, KYC_OperationPerson, KYC_BillingAddress, "", KYC_BillingCity, KYC_BillingPincode,
                                                                                  KYC_BillingPhoneNo, KYC_BillingEmail, true, false, CreatedBy);

                                        //PCA Address Details
                                        int Result_PCAPlantAddress = DBOperations.AddPlantAddress(Result_BranchPlant, 2, KYC_OperationPerson, KYC_BillingAddress, "", KYC_BillingCity, KYC_BillingPincode,
                                              KYC_BillingPhoneNo, KYC_BillingEmail, true, false, CreatedBy);
                                    }
                                }
                            }

                            // finance contact   
                            DataSet dsGetFinance = KYCOperation.GetFinanceContact(VendorId);
                            if (dsGetFinance != null && dsGetFinance.Tables[0].Rows.Count > 0)
                            {
                                int AddUser = KYCOperation.AddCustomerUser(CustomerId, dsGetFinance.Tables[0].Rows[0]["Name"].ToString(), "", true, dsGetFinance.Tables[0].Rows[0]["Email"].ToString(), DateTime.MinValue, "",
                                     dsGetFinance.Tables[0].Rows[0]["MobileNo"].ToString(), "", "", "", 0, false, 0, CreatedBy);
                            }

                            // operation contact
                            DataSet dsGetOper = KYCOperation.GetOperationContact(VendorId);
                            if (dsGetOper != null && dsGetOper.Tables[0].Rows.Count > 0)
                            {
                                int AddUser = KYCOperation.AddCustomerUser(CustomerId, dsGetFinance.Tables[0].Rows[0]["Name"].ToString(), "", true, dsGetFinance.Tables[0].Rows[0]["Email"].ToString(), DateTime.MinValue, "",
                                    dsGetFinance.Tables[0].Rows[0]["MobileNo"].ToString(), "", "", "", 0, false, 0, CreatedBy);
                            }

                            // other contact
                            DataSet dsGetOthr = KYCOperation.GetOtherContact(VendorId);
                            if (dsGetOthr != null && dsGetOthr.Tables[0].Rows.Count > 0)
                            {
                                int AddUser = KYCOperation.AddCustomerUser(CustomerId, dsGetFinance.Tables[0].Rows[0]["Name"].ToString(), "", true, dsGetFinance.Tables[0].Rows[0]["Email"].ToString(), DateTime.MinValue, "",
                                    dsGetFinance.Tables[0].Rows[0]["MobileNo"].ToString(), "", "", "", 0, false, 0, CreatedBy);
                            }
                        }
                    }
                }
            }
        }
    }

    protected bool SendApproval(int VendorId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();

        if (VendorId > 0)
        {
            string strCompanyName = "", strlType = "", strAddress1 = "", strAddress2 = "", strCity = "", strState = "", strCountry = "", strPincode = "",
                    strConstitution = "", strSector = "", strNOB = "", strWebsiteAddress = "", strCompanyEmail = "", strKYCPath = "", strCustomerEmail = "",
                    strCCEmail = "", strSubject = "", MessageBody = "", strObservers = "", strResources = "", strNotes = "", strCreatedBy = "",
                    strCreatedByMail = "", strRejectedRemark = "";

            //Get Basic Detail - Title, date, time
            DataSet dsGetMeetingDetail = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetMeetingDetail != null && dsGetMeetingDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetMeetingDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    strCompanyName = dsGetMeetingDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    strlType = dsGetMeetingDetail.Tables[0].Rows[0]["lTypeName"].ToString();
                    strAddress1 = dsGetMeetingDetail.Tables[0].Rows[0]["Address1"].ToString();
                    strAddress2 = dsGetMeetingDetail.Tables[0].Rows[0]["Address2"].ToString();
                    strCity = dsGetMeetingDetail.Tables[0].Rows[0]["City"].ToString();
                    strState = dsGetMeetingDetail.Tables[0].Rows[0]["StateName"].ToString();
                    strCountry = dsGetMeetingDetail.Tables[0].Rows[0]["CountryName"].ToString();
                    strPincode = dsGetMeetingDetail.Tables[0].Rows[0]["Pincode"].ToString();
                    strConstitution = dsGetMeetingDetail.Tables[0].Rows[0]["ConstitutionName"].ToString();
                    strSector = dsGetMeetingDetail.Tables[0].Rows[0]["SectorName"].ToString();
                    strNOB = dsGetMeetingDetail.Tables[0].Rows[0]["NatureofBusinessName"].ToString();
                    strWebsiteAddress = dsGetMeetingDetail.Tables[0].Rows[0]["WebsiteAdd"].ToString();
                    strCompanyEmail = dsGetMeetingDetail.Tables[0].Rows[0]["Email"].ToString();
                    strCustomerEmail = dsGetMeetingDetail.Tables[0].Rows[0]["sEmail"].ToString();
                    strCreatedBy = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedBy"].ToString();
                    strCreatedByMail = dsGetMeetingDetail.Tables[0].Rows[0]["CreatedByEmail"].ToString();
                    strRejectedRemark = dsGetMeetingDetail.Tables[0].Rows[0]["RejectedRemark"].ToString();
                }
            }
            else
                return false;

            DataSet dsGetKYCDetails = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetKYCDetails != null && dsGetKYCDetails.Tables[0].Rows[0]["KYCScannedCopyPath"] != DBNull.Value)
            {
                strKYCPath = dsGetKYCDetails.Tables[0].Rows[0]["KYCScannedCopyPath"].ToString();
            }

            // Email Format
            strCustomerEmail = strCreatedByMail;
            strCCEmail = "javed.shaikh@babajishivram.com";
            strSubject = "KYC Rejected for customer " + strCompanyName.Trim().ToUpper();

            if (strCustomerEmail == "" || strSubject == "")
                return false;
            else
            {
                MessageBody = "<BR/>Dear Sir,<BR/><BR/>KYC form for company named " + strCompanyName.ToUpper().Trim() + " has been rejected.<BR/>" +
                                "The reason is - " + strRejectedRemark.Trim();
                MessageBody = MessageBody + "<BR/><BR/>" + strbuilder;
                MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + strCreatedBy + "<BR/>";

                bEmailSuccess = EMail.SendMail(strCustomerEmail, strCustomerEmail, strSubject, MessageBody, "");
                return bEmailSuccess;
            }
        }
        else
            return false;
    }

}