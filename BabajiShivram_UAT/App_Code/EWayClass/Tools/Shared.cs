
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Configuration;
using System.Data;
using System.Web;
namespace TaxProEWB.API
{
  public static class Shared
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
      return DateTime.ParseExact("01/04/" + strFY.Substring(0, 4), "dd/MM/yyyy", (IFormatProvider) null);
    }

    public static DateTime FYEndDate(string strFY)
    {
      return DateTime.ParseExact("31/03/" + strFY.Substring(5, 4), "dd/MM/yyyy", (IFormatProvider) null);
    }

    public static EWBAPISetting LoadAPISetting()
    {        
        EWBAPISetting APISetting = new EWBAPISetting();
        APISetting.ID =Convert.ToInt32(WebConfigurationManager.AppSettings["APIAspID"]);
        APISetting.AspPassword = WebConfigurationManager.AppSettings["APIAspPassword"];
        APISetting.BaseUrl = WebConfigurationManager.AppSettings["APIBaseURL"];
        //APISetting.AspUserId = WebConfigurationManager.AppSettings["APIUserName"];
        //APISetting.AspPassword = WebConfigurationManager.AppSettings["APIPassCode"];
        
        return  APISetting;
    }

    public static EWBAPILoginDetails LoadAPILoginDetails(string UserGSTIN)
    {
        
        string strAPIUserName = "", strAPIPasscode = "";
        string strValidDate = ""; string strAuthToken = "";
        DataSet ds = DBOperations.EWAYGetAuthTokan(UserGSTIN);

        if(ds.Tables.Count > 0)
        {
            if(ds.Tables[0].Rows.Count > 0)
            {
                strAPIUserName = ds.Tables[0].Rows[0]["APIUserName"].ToString();
                strAPIPasscode = ds.Tables[0].Rows[0]["APIPasscode"].ToString();
                strAuthToken = ds.Tables[0].Rows[0]["EWayAuthTokan"].ToString();
                strValidDate = ds.Tables[0].Rows[0]["ValidTillDate"].ToString();
            }
        }

        EWBAPILoginDetails APILoginDetail = new EWBAPILoginDetails();
        APILoginDetail.EwbGstin = UserGSTIN;
        APILoginDetail.EwbUserID = strAPIUserName;
        APILoginDetail.EwbPassword = strAPIPasscode;
        APILoginDetail.EwbAuthToken = strAuthToken;
        if(strValidDate != "")
            APILoginDetail.EwbTokenExp = Convert.ToDateTime(strValidDate);
        return APILoginDetail;
        
        //if (File.Exists(FileName))
        //  return JsonConvert.DeserializeObject<EWBAPILoginDetails>(File.ReadAllText(FileName));
        //return new EWBAPILoginDetails();
    }

    public static void SaveAPISetting(EWBAPISetting ApiSetting, string FileName = "EwbApiSetting.json")
    {
      File.WriteAllText(FileName, JsonConvert.SerializeObject((object) ApiSetting, Formatting.Indented));
    }

    public static void SaveAPILoginDetails(EWBAPILoginDetails ApiLoginDetails)
    {
        LoginClass loggedInUser = new LoginClass();
        DBOperations.EWAYUpdateAuthTokan(ApiLoginDetails.EwbGstin, ApiLoginDetails.EwbAuthToken, loggedInUser.glUserId);
    }
  }
}
