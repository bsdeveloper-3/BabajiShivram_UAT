
using RestSharp;
using System;
using System.Web;
using System.Web.SessionState;
namespace TaxProEWB.API
{
    public class EWBSession 
    {
        public EWBAPISetting EwbApiSetting { get; set; }

        public EWBAPILoginDetails EwbApiLoginDetails { get; set; }

        public bool CancleApiTxn { get; set; }

        public string CancelApiMsg { get; set; }

        public bool LoadAPISettingsFromConfigFile { get; set; }

        public bool LoadAPILoginDetailsFromConfigFile { get; set; }
                
        public EWBSession(string UserGSTIN = "27AAACN1163G1ZR")
        {    
            this.EwbApiSetting = Shared.LoadAPISetting();
            this.EwbApiLoginDetails = Shared.LoadAPILoginDetails(UserGSTIN);
        }

        public TxnResp StartAPITxn(bool CallFromAuthAPI = false)
        {
                        
            TxnResp startResp = new TxnResp()
            {
                IsSuccess = true
            };
            //if (this.EwbApiSetting == null || this.EwbApiLoginDetails == null || (string.IsNullOrEmpty(this.EwbApiLoginDetails.EwbUserID) || string.IsNullOrEmpty(this.EwbApiSetting.AspUserId)) || string.IsNullOrEmpty(this.EwbApiSetting.AspPassword) || string.IsNullOrEmpty(this.EwbApiSetting.BaseUrl))
            if (this.EwbApiSetting == null)
            {
                startResp.IsSuccess = false;
                startResp.TxnOutcome = "Start Transaction Failed. Please Check EWBAPISession Settings and EWBLogin Details.";
            }
            else if (this.CancleApiTxn)
            {
                startResp.IsSuccess = false;
                startResp.TxnOutcome = "Cancelled: " + this.CancelApiMsg;
            }
            else
            {
                TxnRespWithObj<EWBSession> txnResp = (TxnRespWithObj<EWBSession>)null;
                if (CallFromAuthAPI == true) // Regenerate Auth On Request
                {                    
                    TxnRespWithObj<EWBSession> txnRespWithObj = EWBAPI.GetAuthTokenAsync(this);
                    txnResp = txnRespWithObj;
                    txnRespWithObj = (TxnRespWithObj<EWBSession>)null;
                    if (txnResp.IsSuccess)
                    {                        
                         Shared.SaveAPILoginDetails(this.EwbApiLoginDetails);
                    }
                }
                else if (DateTime.Compare(DateTime.Now, this.EwbApiLoginDetails.EwbTokenExp) > 0 || this.EwbApiLoginDetails.EwbAuthToken == "")
                {                    
                    
                    TxnRespWithObj<EWBSession> txnRespWithObj = EWBAPI.GetAuthTokenAsync(this);
                    txnResp = txnRespWithObj;
                    txnRespWithObj = (TxnRespWithObj<EWBSession>)null;
                    if (txnResp.IsSuccess)
                    {                        
                        Shared.SaveAPILoginDetails(this.EwbApiLoginDetails);
                    }
                    if (DateTime.Compare(DateTime.Now, this.EwbApiLoginDetails.EwbTokenExp) >= 0)
                    {
                        startResp.IsSuccess = false;
                        startResp.TxnOutcome = "Error : TaxPayers Token Expired. Please Get AuthToken for TaxPayer";
                    }
                }
                if (txnResp != null)
                {
                    startResp.IsSuccess = txnResp.IsSuccess;
                    startResp.TxnOutcome = txnResp.TxnOutcome;
                }
                txnResp = (TxnRespWithObj<EWBSession>)null;
            }
            return startResp;
        }

        public virtual void AddApiHeaders(RestRequest request)
        {            
            request.AddParameter("aspid", this.EwbApiSetting.ID.ToString(), ParameterType.QueryString);
            request.AddParameter("password", this.EwbApiSetting.AspPassword, ParameterType.QueryString);
            request.AddParameter("Gstin", this.EwbApiLoginDetails.EwbGstin, ParameterType.QueryString);
            request.AddParameter("username", this.EwbApiLoginDetails.EwbUserID, ParameterType.QueryString);
            request.AddParameter("ewbpwd", this.EwbApiLoginDetails.EwbPassword, ParameterType.QueryString);
            request.AddParameter("Authtoken", this.EwbApiLoginDetails.EwbAuthToken, ParameterType.QueryString);
            //List<Parameter> parameters = request.Parameters;
            //Func<Parameter, bool> func = (Func<Parameter, bool>) (x => x.Name == "appver");
            //Func<Parameter, bool> predicate;
            //if (parameters.FirstOrDefault<Parameter>(predicate) == null)
            //  request.AddHeader("appver", "TPe.N" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //request.AddHeader("aspid", this.EwbApiSetting.AspUserId);
            //request.AddHeader("password", this.EwbApiSetting.AspPassword);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;

        }

        public virtual void LogAPITxn(APITxnLogArgs e)
        {
            LoginClass LoggedInUser = new LoginClass();
            int result = DBOperations.EWAYAddLog(e.ApiAction, e.AppUserName, e.ErrCode, e.IsSuccess, e.OutcomeMsg, e.TxnDateTime.ToString(), LoggedInUser.glUserId);
        }
    }
}
