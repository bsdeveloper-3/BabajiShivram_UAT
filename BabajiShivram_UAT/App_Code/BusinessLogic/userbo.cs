using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

public class USERSBO
{
    private string UID;
    private string UNAME;
    private string EMAIL;
    private string QUESTION;
    private string ANSWER;
    private bool APPROVED;

    private string pssword;

    public string USERID
    {
        get
        {
            return UID;
        }
        set
        {
            UID = value;
        }
    }

    public string USERNAME
    {
        get
        {
            return UNAME;
        }
        set
        {
            UNAME = value;
        }
    }

    public string USEREMAIL
    {
        get
        {
            return EMAIL;
        }
        set
        {
            EMAIL = value;
        }
    }

    public string USERQUEST
    {
        get
        {
            return QUESTION;

        }
        set
        {
            QUESTION = value;
        }
    }

    public string USERANSWER
    {
        get
        {
            return ANSWER;
        }
        set
        {
            ANSWER = value;
        }
    }

    public bool ISAPPROVED
    {
        get
        {
            return APPROVED;
        }
        set
        {
            APPROVED = value;
        }
    }

    public string Pssword
    {
        get
        {
            return pssword;
        }

        set
        {
            pssword = value;
        }

    }



}
