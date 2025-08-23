using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for Randoms

/// </summary>

public class Randoms
{

    private int RandomNumber(int min, int max)
    {
        Random random = new Random(); 
        return random.Next(min, max);
    }

    protected string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();

        Random random = new Random();
        char ch;

        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }

        if (lowerCase)
            return builder.ToString().ToLower();

        return builder.ToString();
    }

    protected string RandomString2(int sizes, bool lowerCases)
    {
        StringBuilder builders = new StringBuilder();
        Random randoms = new Random();

        char chs; for (int i = 0; i < sizes; i++)
        {
            chs = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * randoms.NextDouble() + 65)));
            builders.Append(chs);
        }

        if (lowerCases)
            return builders.ToString().ToLower();

        return builders.ToString();
    }
}