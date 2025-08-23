using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoAuthRespPl
/// </summary>
namespace MyPacco.API
{
    public class MyPaccoAuthRespPl
    {
        public string status { get; set; }

        public string authtoken { get; set; }

        public string sek { get; set; }
    }
}