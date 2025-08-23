using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoAPISetting
/// </summary>
namespace MyPacco.API
{
    public class MyPaccoAPISetting
    {
        public int ID { get; set; }

        public string MyPaccoProviderName { get; set; }
        public string MyPaccoUserId { get; set; }

        public string MyPaccoPassword { get; set; }

        public string MyPaccoClientId { get; set; }

        public string MyPaccoClientPassword { get; set; }
                
        public string MyPaccoAuthUrl { get; set; }

        public string MyPaccoBaseUrl { get; set; }
                
    }
}