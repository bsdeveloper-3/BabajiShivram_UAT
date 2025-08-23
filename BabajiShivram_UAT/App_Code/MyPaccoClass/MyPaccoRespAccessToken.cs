using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoRespAccessToken
/// </summary>
namespace MyPacco.API
{
    
        public class Data
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        public class MyPaccoRespAccessToken
        {
            public bool IsSuccess { get; set; }
            public Data Data { get; set; }
            public string Message { get; set; }
        }


    
}