using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Configuration;
using System.Data;
using System.Web;
/// <summary>
/// Summary description for MyPaccoShared
/// </summary>
namespace MyPacco.API
{
    public static class MyPaccoShared
    {
        public static string GetCurrFY()
        {
            if (DateTime.Today.Month >= 4)
            {
                DateTime today = DateTime.Today;
                string str1 = today.Year.ToString();
                string str2 = "-";
                today = DateTime.Today;
                string str3 = (today.Year + 1).ToString();
                return str1 + str2 + str3;
            }

            DateTime today1 = DateTime.Today;
            string str4 = (today1.Year - 1).ToString();
            string str5 = "-";
            today1 = DateTime.Today;
            string str6 = today1.Year.ToString();
            return str4 + str5 + str6;
        }
        public static DateTime FYStartDate(string strFY)
        {
            return DateTime.ParseExact("01/04/" + strFY.Substring(0, 4), "dd/MM/yyyy", (IFormatProvider)null);
        }
        public static DateTime FYEndDate(string strFY)
        {
            return DateTime.ParseExact("31/03/" + strFY.Substring(5, 4), "dd/MM/yyyy", (IFormatProvider)null);
        }
        public static MyPaccoAPISetting LoadAPISetting()
        {
            MyPaccoAPISetting APISetting    = new MyPaccoAPISetting();
                        
            APISetting.MyPaccoClientId  = WebConfigurationManager.AppSettings["MyPaccoClient_id"];
            APISetting.MyPaccoClientPassword = WebConfigurationManager.AppSettings["MyPaccoClient_secret"];
            APISetting.MyPaccoBaseUrl   = WebConfigurationManager.AppSettings["MyPaccoAPIBaseURL"];
                        
            return APISetting;
        }
        public static MyPaccoAPILoginDetails LoadAPILoginDetails()
        {
            MyPaccoAPILoginDetails APILoginDetail = new MyPaccoAPILoginDetails();
                        
            string strAPIUserName = "", strAPIPasscode = "";
            string strValidDate = ""; string strAuthToken = "";

            DataSet ds = DBOperations.MyPaccoGetAuthTokan();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strAPIUserName  = ds.Tables[0].Rows[0]["APIUserName"].ToString();
                    strAPIPasscode  = ds.Tables[0].Rows[0]["APIPasscode"].ToString();
                    strAuthToken    = ds.Tables[0].Rows[0]["AuthTokan"].ToString();
                    strValidDate    = ds.Tables[0].Rows[0]["ValidTillDate"].ToString();
                }
            }

            APILoginDetail.MyPaccoUserID    = strAPIUserName;
            APILoginDetail.MyPaccoPassword  = strAPIPasscode;
            APILoginDetail.MyPaccoAuthToken = strAuthToken;

            if (strValidDate != "")
                APILoginDetail.MyPaccoTokenExp = Convert.ToDateTime(strValidDate);
            
            return APILoginDetail;
            
            //if (File.Exists(FileName))
            //  return JsonConvert.DeserializeObject<EWBAPILoginDetails>(File.ReadAllText(FileName));
            //return new EWBAPILoginDetails();
        }

        public static void SaveAPISetting(MyPaccoAPISetting ApiSetting)
        {
            if (WebConfigurationManager.AppSettings["GSPName"].ToString().Contains("SandBox"))
            {
                string FileName = "EwbApiSetting.json";
                File.WriteAllText(HttpContext.Current.Server.MapPath(FileName), JsonConvert.SerializeObject((object)ApiSetting, Formatting.Indented));
            }
        }
        public static void SaveAPILoginDetails(MyPaccoAPILoginDetails ApiLoginDetails)
        {         
            LoginClass loggedInUser = new LoginClass();
            DBOperations.MyPaccoUpdateAuthTokan(ApiLoginDetails.MyPaccoAuthToken, loggedInUser.glUserId);
            
        }
    }
}