using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;

using BSImport;


/// <summary>
/// Summary description for CSVReader
/// </summary>
public class CSVReader
{
	public CSVReader()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private Stream objStream;
    private StreamReader objStreamReader;

    public CSVReader(Stream fileStream): this(fileStream, null) 
    { 

    }

    public CSVReader(Stream fileStream, Encoding enc)
    {
        this.objStream = fileStream;

        if (!fileStream.CanRead) //check the Pass Stream whether it is readable or not
        {
            return;
        }
        objStreamReader = (enc != null) ? new StreamReader(fileStream, enc) : new StreamReader(fileStream);

    }

    public string[] GetCSVLine()//parse the line
    {
        string data = objStreamReader.ReadLine();

        if (data == null)
        {
            return null;
        }//End if

        if (data.Length == 0)
        {
            return new string[0];
        }//End If

        ArrayList result = new ArrayList();

          ParseCSVData(result, data);

          return (string[])result.ToArray(typeof(string));

    }

    public void ParseCSVData(ArrayList result, string data)
    {
        int position = -1;
        while (position < data.Length)
        {
            result.Add(ParseCSVField(ref data,ref position));
        }

    }

    private string ParseCSVField(ref  string data, ref int StartSeperatorPos)
    {
        if (StartSeperatorPos == data.Length - 1)
        {
            StartSeperatorPos++;
            return "";
        }//End If

        int fromPos = StartSeperatorPos + 1;

        if (data[fromPos] == '"')
        {
            int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
            int lines = 1;
            while (nextSingleQuote == -1)
            {
                data = data + "\n" + objStreamReader.ReadLine();
                nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                lines++;
                if (lines > 20)
                {
                    throw new Exception("lines overflow: " + data);
                }//end If
            }//END While

            StartSeperatorPos = nextSingleQuote + 1;
            string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
            tempString = tempString.Replace("'", "''");
            return tempString.Replace("\"\"", "\"");
        }//end If

        int nextComma = data.IndexOf(',', fromPos);
        if (nextComma == -1)
        {
            StartSeperatorPos = data.Length;
            return data.Substring(fromPos);
        }
        else
        {
            StartSeperatorPos = nextComma;
            return data.Substring(fromPos, nextComma - fromPos);
        }//End if
    }


     private int GetSingleQuote(string data, int SFrom)
      {

          int i = SFrom - 1;
            while(++i <data.Length)
             if (data[i] == '"')
             {
                 if (i < data.Length - 1 && data[i + 1] == '"')
                 {
                     i++;
                     continue;
                 }
                 else
                 {
                     return i;
                 }//EndIF

             }//End IF
         return -1;

     }


     
}
