using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyPaccoConstants
/// </summary>

namespace MyPacco.API
{
    public static class MyPaccoConstants
    {
        public static string DecryptionError = "Error while decrypting or decoding received data.";
        public static int AuthTokenValidityMin = 3600; // Seconds
    }
}