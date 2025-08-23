using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CurrencyToWords
/// </summary>
public static class CurrencyToWords
{
    public static string AmountInWords(decimal Num)
    {
        string returnValue;
        string strNum;
        string strNumDec;
        string StrWord = "";
        strNum = Num.ToString();

        if (strNum.IndexOf(".") + 1 != 0)
        {
            strNumDec = strNum.Substring(strNum.IndexOf(".") + 2 - 1);

            if (strNumDec.Length == 1)
            {
                strNumDec = strNumDec + "0";
            }
            if (strNumDec.Length > 2)
            {
                strNumDec = strNumDec.Substring(0, 2);
            }


            strNum = strNum.Substring(0, strNum.IndexOf(".") + 0);
           // StrWord = ((double.Parse(strNum) == 1) ? " Rupee " : " Rupees ") + NumToWord((decimal)(double.Parse(strNum))) + ((double.Parse(strNumDec) > 0) ? (" and Paise" + cWord3((decimal)(double.Parse(strNumDec)))) : "");
            StrWord =  NumToWord((decimal)(double.Parse(strNum))) + ((double.Parse(strNumDec) > 0) ? (" and Paise" + cWord3((decimal)(double.Parse(strNumDec)))) : "");
        }
        else
        {
          //  StrWord = ((double.Parse(strNum) == 1) ? " Rupee " : " Rupees ") + NumToWord((decimal)(double.Parse(strNum)));

            StrWord =  NumToWord((decimal)(double.Parse(strNum)));
        }

        returnValue = "Rs. " + Num + " /- (Rupees " + StrWord + " Only)";
        return returnValue;
    }
    
    static public string NumToWord(decimal Num)
    {
        string returnValue;


        //I divided this function in two part.
        //1. Three or less digit number.
        //2. more than three digit number.
        string strNum;
        string StrWord;
        strNum = Num.ToString();


        if (strNum.Length <= 3)
        {
            StrWord = cWord3((decimal)(double.Parse(strNum)));
        }
        else
        {
            StrWord = cWordG3((decimal)(double.Parse(strNum.Substring(0, strNum.Length - 3)))) + " " + cWord3((decimal)(double.Parse(strNum.Substring(strNum.Length - 2 - 1))));
        }
        returnValue = StrWord;
        return returnValue;
    }
    
    static public string cWordG3(decimal Num)
    {
        string returnValue;
        //2. more than three digit number.
        string strNum = "";
        string StrWord = "";
        string readNum = "";
        strNum = Num.ToString();
        if (strNum.Length % 2 != 0)
        {
            readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 1)));
            if (readNum != "0")
            {
                StrWord = retWord(decimal.Parse(readNum));
                readNum = System.Convert.ToString(double.Parse("1" + strReplicate("0", strNum.Length - 1) + "000"));
                StrWord = StrWord + " " + retWord(decimal.Parse(readNum));
            }
            strNum = strNum.Substring(1);
        }
        while (!System.Convert.ToBoolean(strNum.Length == 0))
        {
            readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 2)));
            if (readNum != "0")
            {
                StrWord = StrWord + " " + cWord3(decimal.Parse(readNum));
                readNum = System.Convert.ToString(double.Parse("1" + strReplicate("0", strNum.Length - 2) + "000"));
                StrWord = StrWord + " " + retWord(decimal.Parse(readNum));
            }
            strNum = strNum.Substring(2);
        }
        returnValue = StrWord;
        return returnValue;
    }
    
    static public string cWord3(decimal Num)
    {
        string returnValue;
        //1. Three or less digit number.
        string strNum = "";
        string StrWord = "";
        string readNum = "";
        if (Num < 0)
        {
            Num = Num * -1;
        }
        strNum = Num.ToString();


        if (strNum.Length == 3)
        {
            readNum = System.Convert.ToString(double.Parse(strNum.Substring(0, 1)));
            StrWord = retWord(decimal.Parse(readNum)) + " Hundred";
            strNum = strNum.Substring(1, strNum.Length - 1);
        }


        if (strNum.Length <= 2)
        {
            if (double.Parse(strNum) >= 0 && double.Parse(strNum) <= 20)
            {
                StrWord = StrWord + " " + retWord((decimal)(double.Parse(strNum)));
            }
            else
            {
                StrWord = StrWord + " " + retWord((decimal)(System.Convert.ToDouble(strNum.Substring(0, 1) + "0"))) + " " + retWord((decimal)(double.Parse(strNum.Substring(1, 1))));
            }
        }


        strNum = Num.ToString();
        returnValue = StrWord;
        return returnValue;
    }
    
    static public string retWord(decimal Num)
    {
	string returnValue = "";
		
	//This two dimensional array store the primary word convertion of number.
        

        //object[,] ArrWordList = new object[,] { { 0, "Zero" }, { 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" }, { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" }, { 10, "Ten" }, { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" }, { 14, "Fourteen" }, { 15, "Fifteen" }, { 16, "Sixteen" }, { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen" }, { 20, "Twenty" }, { 30, "Thirty" }, { 40, "Forty" }, { 50, "Fifty" }, { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Eighty" }, { 90, "Ninety" }, { 100, "Hundred" }, { 1000, "Thousand" }, { 100000, "Lakh" }, { 10000000, "Crore" } };
        // Zero Issue In "80081" = (Rupees Eighty Zero Thousand Eighty One Only)

        object[,] ArrWordList = new object[,] { { 0, "" }, { 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" }, { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" }, { 10, "Ten" }, { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" }, { 14, "Fourteen" }, { 15, "Fifteen" }, { 16, "Sixteen" }, { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen" }, { 20, "Twenty" }, { 30, "Thirty" }, { 40, "Forty" }, { 50, "Fifty" }, { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Eighty" }, { 90, "Ninety" }, { 100, "Hundred," }, { 1000, "Thousand," }, { 100000, "Lakh," }, { 10000000, "Crore," } };


        int i;
        for (i = 0; i <= (ArrWordList.Length - 1); i++)
        {
            if (Num == System.Convert.ToDecimal(ArrWordList[i, 0]))
            {
                returnValue = (string)(ArrWordList[i, 1]);
                break;
            }
        }
        return returnValue;
    }
    
    static public string strReplicate(string str, int intD)
    {
        string returnValue;
        //This fucntion padded "0" after the number to evaluate hundred, thousand and on....
        //using this function you can replicate any Charactor with given string.
        int i;
        returnValue = "";
        for (i = 1; i <= intD; i++)
        {
            returnValue = returnValue + str;
        }
        return returnValue;
    }
}

