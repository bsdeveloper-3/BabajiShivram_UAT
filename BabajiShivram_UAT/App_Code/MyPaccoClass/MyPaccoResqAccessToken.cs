using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoResqAccessToken
/// </summary>
namespace MyPacco.API
{   
        public class MyPaccoClientID
    {
            public string client_id { get; set; }
            public string client_secret { get; set; }
        }

        public class MyPaccoResqAccessToken
        {
            public string access_token { get; set; }
            public List<MyPaccoClientID> data { get; set; }
        }
}