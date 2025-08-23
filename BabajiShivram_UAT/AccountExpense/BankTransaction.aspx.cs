using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BankAPI.YesBank;
using BankAPI.Open;
public partial class AccountExpense_BankTransaction : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGetToken_Click(object sender, EventArgs e)
    {
        lblToken.Text = GenerateToken();
    }

    protected void btnGetBalance_Click(object sender, EventArgs e)
    {

        lblToken.Text = UATGetAccountBalance();

        //lblToken.Text = GetAccountBalance();

    }

    protected void btnTransferFund_Click(object sender, EventArgs e)
    {
        decimal NetPayable = 100;
        //lblToken.Text  = TransferFund(NetPayable);

        lblToken.Text = UATTransferFund(NetPayable);

    }
    protected void btnGetTransferStatus_Click(object sender, EventArgs e)
    {
        lblToken.Text = GetTransferStatus("");
    }
    private string GenerateToken()
    {
        BNRespGenAccessToken responseToken = BNAPIMethods.BankGetAuthToken("", "");

        return responseToken.access_token;
    }

    private string GetAccountBalance()
    {
        if (ddAccoutNo.SelectedIndex > 0)
        {
            string strToken = GenerateToken();

            BNReqGetBalance getBalance = new BNReqGetBalance();

            getBalance.version = "1.0";
            getBalance.appID = "1";
            getBalance.customerID = "1";
            getBalance.AccountNumber = ddAccoutNo.SelectedItem.Text;

            //  getBalance.AccountNumber = "000000000000005";
            //   getBalance.AccountNumber = "000000000000002";

            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var json = serializer.Serialize(getBalance);


            BNRespGetBalance result = BNAPIMethods.BankGetBalance(getBalance.customerID, getBalance.AccountNumber, strToken);

            return result.accountBalanceAmount.ToString();
        }
        else
        {
            return "Please Select Account No!";
        }
    }


    private string GetTransferStatus(string strRefNo)
    {
        if (ddAccoutNo.SelectedIndex > 0)
        {
            string strToken = GenerateToken();

            BNReqTransferStatus getStatus = new BNReqTransferStatus();

            getStatus.customerID = "1";
            getStatus.requestReferenceNo = "01";


            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var json = serializer.Serialize(getBalance);


            BNRespTransferStatus result = BNAPIMethods.BankGetTransferStatus(getStatus.requestReferenceNo, getStatus.customerID, strToken);

            return result.transactionStatus;
        }
        else
        {
            return "Please Select Account No!";
        }
    }

    #region UAT

    private string UATGetAccountBalance()
    {
          //  string strToken = GenerateToken();

            BankReqBalance getUATBalance = new BankReqBalance();

            BankReqBalance.Root objRoot = new BankReqBalance.Root();
            BankReqBalance.Data objData = new BankReqBalance.Data();
            BankReqBalance.DebtorAccount objDebAccount = new BankReqBalance.DebtorAccount();

            objRoot.Data = objData;
            objData.DebtorAccount = objDebAccount;

            objDebAccount.ConsentId = "527274";
            objDebAccount.Identification = "001790600004039";
            objDebAccount.SecondaryIdentification = "527274";

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonPayLoad = serializer.Serialize(objRoot);

           BankRespBalance.Root result = new BankRespBalance.Root();

          result = BankAPIMethods.UATBankGetBalance(jsonPayLoad);

        //string certFile = System.Web.HttpContext.Current.Server.MapPath("~") + "//Live.Certificte_PrivateKey.pfx";

        //return certFile;

           return result.Data.FundsAvailableResult.BalanceAmount;

    }

    protected void btnUATStatus_Click(object sender, EventArgs e)
    {
        //  string strToken = GenerateToken();

        string strRefNo = txtReferenceNo.Text.Trim();

        if (strRefNo == "")
        {
            lblToken.Text = "Enter Ref No";
        }
        else
        {
            BankReqStatus.Root objRoot = new BankReqStatus.Root();
            BankReqStatus.Data objData = new BankReqStatus.Data();

            objRoot.Data = objData;

            objData.ConsentId = "527274";
            objData.InstrId = strRefNo;
            objData.SecondaryIdentification = "527274";

            
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonPayLoad = @serializer.Serialize(objRoot);

            lblReqest.Text = jsonPayLoad;

            BankAPIError.Root objError = new BankAPIError.Root();
            BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();


            string jsonResponse = "";

            string result = BankAPIMethods.UATBankGetStatus(jsonPayLoad,ref objError, ref objSuccess);

            if (objError.Code != null)
            {
                jsonResponse = objError.Message;
            }
            else if (objSuccess.Data != null)
            {
                jsonResponse = objSuccess.Data.Status;

                int IsSuccess = -1; // NULL
                DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());

                int StatusId = (int)Enum.Parse(typeof(EnumBankStatus), objSuccess.Data.Status);

                if (StatusId == (int)EnumBankStatus.SettlementCompleted)
                    IsSuccess = 1;
                else if (StatusId == (int)EnumBankStatus.FAILED)
                {
                    IsSuccess = 0;
                }

                int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
                    objSuccess.Data.Initiation.EndToEndIdentification, objSuccess.Data.Status, objSuccess.Data.StatusUpdateDateTime, StatusId, IsSuccess, LoggedInUser.glUserId);
            }

            lblToken.Text = result;
        }
        

    }

    private string UATTransferFund(decimal decAmount)
    {
        if (ddAccoutNo.SelectedIndex > 0)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string strReqestRefNo = unixTimestamp.ToString();


            BankReqTransfer.Root objTransferFund = new BankReqTransfer.Root();

            BankReqTransfer.Data objData = new BankReqTransfer.Data();

            BankReqTransfer.Risk objRisk = new BankReqTransfer.Risk();

            objTransferFund.Data = objData;
            objTransferFund.Risk = objRisk;

            BankReqTransfer.Initiation objInitiation = new BankReqTransfer.Initiation();

            BankReqTransfer.DebtorAccount objDebtorAccount = new BankReqTransfer.DebtorAccount();
            BankReqTransfer.CreditorAccount objCreditorAccount = new BankReqTransfer.CreditorAccount();
            BankReqTransfer.RemittanceInformation objRemittanceInformation = new BankReqTransfer.RemittanceInformation();

            BankReqTransfer.Unstructured objUnstructured = new BankReqTransfer.Unstructured();
            BankReqTransfer.Unstructured2 objUnstructured2 = new BankReqTransfer.Unstructured2();
            BankReqTransfer.ContactInformation objContactInformation = new BankReqTransfer.ContactInformation();
            BankReqTransfer.DeliveryAddress objDeliveryAddress = new BankReqTransfer.DeliveryAddress();


            objData.Initiation = objInitiation;
            objInitiation.DebtorAccount = objDebtorAccount;
            objInitiation.CreditorAccount = objCreditorAccount;
            objInitiation.RemittanceInformation = objRemittanceInformation;

            objRisk.DeliveryAddress = objDeliveryAddress;

            objUnstructured.ContactInformation = objContactInformation;
            objCreditorAccount.Unstructured = objUnstructured;

            objRemittanceInformation.Unstructured = objUnstructured2;

            BankReqTransfer.InstructedAmount objInstructedAmount = new BankReqTransfer.InstructedAmount();

            objInitiation.InstructedAmount = objInstructedAmount;
            objInitiation.DebtorAccount = objDebtorAccount;
            objInitiation.CreditorAccount = objCreditorAccount;

            objData.ConsentId = "527274";

            // Debit
            objInitiation.InstructionIdentification = strReqestRefNo;
            objInitiation.EndToEndIdentification = "";
            
            objInstructedAmount.Amount = "200";
            objInstructedAmount.Currency = "INR";

            objDebtorAccount.Identification = "001790600004039";
            objDebtorAccount.SecondaryIdentification = "527274";

            // Credit

            objCreditorAccount.SchemeName = "yesb0000270";
            objCreditorAccount.Identification = "026291800001191";
            
            objCreditorAccount.Name = "Navbharat";
            objContactInformation.EmailAddress = "amit.bakshi@babajishivram.com";
            objContactInformation.MobileNumber = "98333708840";

            // Payment Reference

            objRemittanceInformation.Reference = "CB00001MBOI2021";
            objUnstructured2.CreditorReferenceInformation = "Duty Payment";
            // Payment Mode

            objInitiation.ClearingSystemIdentification = "NEFT";

            // Delivery Address

            List<string> lstAddressLine = new List<string>();

            lstAddressLine.Add("Plot No 2A");
            lstAddressLine.Add("Behind Excom House");

            objDeliveryAddress.AddressLine = lstAddressLine;

            objDeliveryAddress.StreetName = "Sakinaka";
            objDeliveryAddress.BuildingNumber = "2A";
            objDeliveryAddress.PostCode = "400073";
            objDeliveryAddress.TownName= "Mumbai";

            List<string> lstCountySubDivision = new List<string>();

            lstCountySubDivision.Add("MH");
            objDeliveryAddress.CountySubDivision = lstCountySubDivision;
            objDeliveryAddress.Country = "IN";

            ////////////////////////////////////////////////////////


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonRequest = serializer.Serialize(objTransferFund);


            BankAPIError.Root objError = new BankAPIError.Root();
            BankAPIRespTransfer.Root objSuccess = new BankAPIRespTransfer.Root();


            int AccountResult =  AccountExpense.AddBankPaymentAPIRequest(1,0, objInitiation.InstructionIdentification, objInstructedAmount.Amount, objInstructedAmount.Currency,
                objCreditorAccount.SchemeName, objCreditorAccount.Identification, objCreditorAccount.Name, LoggedInUser.glUserId);

            if (AccountResult > 0)
            {
                string result = BankAPIMethods.UATBankPayment(jsonRequest, ref objError, ref objSuccess);

                if (objError.Code != null)
                {
                    jsonRequest = objError.Message;
                }
                else if (objSuccess.Data != null)
                {
                    jsonRequest = objSuccess.Data.Status;

                    int IsSuccess = -1; // NULL
                    DateTime dtResponseDate = DateTime.Parse(objSuccess.Data.StatusUpdateDateTime.ToString());

                    int StatusId = (int)Enum.Parse(typeof(EnumBankStatus), objSuccess.Data.Status);

                    if (StatusId == (int)EnumBankStatus.SettlementCompleted)
                        IsSuccess = 1;
                    else if (StatusId == (int)EnumBankStatus.FAILED)
                    {
                        IsSuccess = 0;
                    }

                    int AccountUpdResult = AccountExpense.UpdateBankPaymentAPIResponse(objSuccess.Data.Initiation.InstructionIdentification, objSuccess.Data.TransactionIdentification,
                        objSuccess.Data.Initiation.EndToEndIdentification, objSuccess.Data.Status, dtResponseDate, IsSuccess, StatusId, LoggedInUser.glUserId);
                }
            }
            else if(AccountResult == 0)
            {
                jsonRequest = "Request Reference no Already Exists!";
            }
            else
            {
                jsonRequest = "System Erro! Payment Request Not Initiated!";
            }
            //   return result.requestReferenceNo.ToString();

            return jsonRequest;
        }
        else
        {
            return "Please Select Account No!";
        }
    }

    #endregion

    protected void btnGetLiveBalance_Click(object sender, EventArgs e)
    {
        //  string strToken = GenerateToken();

        BankReqBalance getUATBalance = new BankReqBalance();

        BankReqBalance.Root objRoot = new BankReqBalance.Root();
        BankReqBalance.Data objData = new BankReqBalance.Data();
        BankReqBalance.DebtorAccount objDebAccount = new BankReqBalance.DebtorAccount();

        objRoot.Data = objData;
        objData.DebtorAccount = objDebAccount;

        objDebAccount.ConsentId = "418585";
        objDebAccount.Identification = "007881300000296";
        objDebAccount.SecondaryIdentification = "418585";

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var jsonPayLoad = serializer.Serialize(objRoot);

        BankRespBalance.Root result = new BankRespBalance.Root();

        lblReqest.Text = jsonPayLoad;
        lblToken.Text = BankAPIMethods.BankGetLiveBalance(jsonPayLoad);

        //lblToken.Text =  result.Data.FundsAvailableResult.BalanceAmount;
    }

    protected void btnGetLiveStatus_Click(object sender, EventArgs e)
    {
        //  string strToken = GenerateToken();

        string strRefNo = txtReferenceNo.Text.Trim();

        if (strRefNo == "")
        {
            lblToken.Text = "Enter Ref No";
        }
        else
        {
            BankReqStatus.Root objRoot = new BankReqStatus.Root();
            BankReqStatus.Data objData = new BankReqStatus.Data();
           
            objRoot.Data = objData;

            objData.ConsentId   =   "418585";
            objData.InstrId     =   strRefNo;
            objData.SecondaryIdentification = "418585";

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonPayLoad = @serializer.Serialize(objRoot);

            lblReqest.Text = jsonPayLoad;

            lblToken.Text = BankAPIMethods.BankGetLiveStatus(jsonPayLoad);
        }
        //lblToken.Text =  result.Data.FundsAvailableResult.BalanceAmount;
    }

    protected void btnGetLiveTransfer_Click(object sender, EventArgs e)
    {
        lblToken.Text = BankAPIMethods.BankPayment("");
    }
}