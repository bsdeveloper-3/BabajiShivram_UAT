using RestSharp;
using System;
using System.Web;
using System.Web.SessionState;

namespace MyPacco.API
{
    /// <summary>
    /// Summary description for MyPaccoSession
    /// </summary>  
    public class MyPaccoSession
    {
        public static string GspApiBaseUrl = "http://api.mypacco.com/api";
        public MyPaccoAPISetting MyPaccoApiSetting { get; set; }

        public MyPaccoAPILoginDetails MyPaccoApiLoginDetails { get; set; }

        public bool CancleApiTxn { get; set; }

        public string CancelApiMsg { get; set; }

        public bool LoadAPISettingsFromConfigFile { get; set; }

        public bool LoadAPILoginDetailsFromConfigFile { get; set; }

        public MyPaccoSession()
        {
            this.MyPaccoApiSetting = MyPaccoShared.LoadAPISetting();
            this.MyPaccoApiLoginDetails = MyPaccoShared.LoadAPILoginDetails();
        }

        public virtual void AddApiHeaders(RestRequest request)
        {
            //request.AddParameter("aspid", this.EwbApiSetting.ID.ToString(), ParameterType.QueryString);
            //request.AddParameter("password", this.EwbApiSetting.AspPassword, ParameterType.QueryString);
            //request.AddParameter("Gstin", this.EwbApiLoginDetails.EwbGstin, ParameterType.QueryString);
            //request.AddParameter("username", this.EwbApiLoginDetails.EwbUserID, ParameterType.QueryString);
            //request.AddParameter("ewbpwd", this.EwbApiLoginDetails.EwbPassword, ParameterType.QueryString);
            //request.AddParameter("Authtoken", this.EwbApiLoginDetails.EwbAuthToken, ParameterType.QueryString);
            //List<Parameter> parameters = request.Parameters;
            //Func<Parameter, bool> func = (Func<Parameter, bool>) (x => x.Name == "appver");
            //Func<Parameter, bool> predicate;
            //if (parameters.FirstOrDefault<Parameter>(predicate) == null)
            //  request.AddHeader("appver", "TPe.N" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //request.AddHeader("aspid", this.EwbApiSetting.AspUserId);
            //request.AddHeader("password", this.EwbApiSetting.AspPassword);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;
        }

        public virtual void MyPaccoLogAPITxn(MyPaccoLogArgs e)
        {
            LoginClass LoggedInUser = new LoginClass();
            int result = DBOperations.EWAYAddLog(e.ApiAction, e.AppUserName, e.ErrCode, e.IsSuccess, e.OutcomeMsg, e.TxnDateTime.ToString(), LoggedInUser.glUserId);
        }
    }
}