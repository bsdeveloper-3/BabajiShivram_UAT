using System;

/// <summary>
/// Summary description for MyPaccoAPILoginDetails
/// </summary>
namespace MyPacco.API
{
    public class MyPaccoAPILoginDetails
    {        
        public string MyPaccoUserID { get; set; }

        public string MyPaccoPassword { get; set; }

        public string MyPaccoAppKey { get; set; }

        public string MyPaccoAuthToken { get; set; }

        public DateTime MyPaccoTokenExp { get; set; }
                
    }
}