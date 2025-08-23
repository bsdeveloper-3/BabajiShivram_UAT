using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

public partial class FreightEnqModule_ViewAwardedEnq : System.Web.UI.Page
{
    string str_Message = "";
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["VendorId"]) != null)
            {
                LoadGridData();
                if (loggedInUser.glEmpName.ToString() != "")
                    lblUserName.Text = loggedInUser.glEmpName.ToString();

                GetInboxMsgs();
                if (Request.QueryString.Count > 0)
                {
                    string op = "";
                    op = Decrypt(HttpUtility.UrlDecode(Request.QueryString["op"]));

                    if (op == "1")
                    {
                        Session["MessageTo"] = "0";
                        GetRecentChats();
                        GetOnlineUsers();
                        ModalPopupExtender1.Show();
                    }
                    else if (op == "0")
                    {
                        Session["MessageTo"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));
                        GetRecentChats();
                        GetOnlineUsers();
                        ReadMessages();
                        ModalPopupExtender1.Show();
                    }
                }
                else
                {
                    if (Convert.ToString(Session["MessageTo"]) != null &&
                        Convert.ToString(Session["MessageTo"]) != "" &&
                        Convert.ToString(Session["MessageTo"]) != "0")
                    {
                        GetRecentChats();
                        GetOnlineUsers();
                        ReadMessages();
                        ModalPopupExtender1.Show();
                    }
                }
            }
            else
                Response.Redirect("FRlogin.aspx");
        }
    }

    protected void LoadGridData()
    {
        try
        {
            if (loggedInUser.glRoleId.ToString() != "")
            {
                if (loggedInUser.glRoleId.ToString() == "53")
                {
                    List<AlibabaFreightTracking> mList_FreightTracking = new List<AlibabaFreightTracking>();
                    mList_FreightTracking = DBOperations.GetAlibabaAwardedEnquiry("SelectAll", 0);
                    if (mList_FreightTracking.Count > 0)
                    {
                        ViewState["FreightTracking"] = mList_FreightTracking;
                    }
                    else
                        ViewState["FreightTracking"] = "";
                }
                else
                {
                    List<AlibabaFreightTracking> mList_FreightTracking = new List<AlibabaFreightTracking>();
                    mList_FreightTracking = DBOperations.GetAlibabaAwardedEnquiry("SelectUser", Convert.ToInt32(Session["VendorId"]));
                    if (mList_FreightTracking.Count > 0)
                    {
                        ViewState["FreightTracking"] = mList_FreightTracking;
                    }
                    else
                        ViewState["FreightTracking"] = "";
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    #region CHAT MESSAGE EVENTS

    protected void ReadMessages()
    {
        try
        {
            if (Convert.ToString(Session["Messageto"]) != null && Convert.ToString(Session["Messageto"]) != "0" &&
                Convert.ToString(Session["VendorId"]) != "" && Convert.ToString(Session["VendorId"]) != "0")
            {
                int MessageTo = 0, lUser = 0;
                MessageTo = Convert.ToInt32(Session["MessageTo"]);
                lUser = Convert.ToInt32(Session["VendorId"]);

                int ReadMsg = DBOperations.UpdateMsgAsRead(MessageTo, lUser);
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void GetInboxMsgs()
    {
        try
        {
            StringBuilder strBuilder = new StringBuilder();
            DataSet dsGetTotalMsgsRec = new DataSet();
            dsGetTotalMsgsRec = DBOperations.GetTotalMsgReceived(Convert.ToInt32(Session["VendorId"]));
            if (dsGetTotalMsgsRec != null && dsGetTotalMsgsRec.Tables.Count > 0)
            {
                if (dsGetTotalMsgsRec.Tables[0].Rows.Count > 0)
                {
                    string op = HttpUtility.UrlEncode(Encrypt("1"));
                    strBuilder.Append(@"<a href='ViewAwardedEnq.aspx?op=" + op + "'><i class='fa fa-envelope'></i> Inbox" +
                                        "<span class='pull-right label label-default'>" + dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString() + "</span>" +
                                        "</a>");

                    li_Inbox.InnerHtml = strBuilder.ToString();

                    int Msgs = Convert.ToInt32(dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString());
                    //if (Msgs > 0)
                    //{
                    //    str_Message = "<script type='text/javascript' language='javascript'>" +
                    //                         "_toastr('You have " + Msgs + " messages received in inbox...!!!','top-full-width','info',false); </script>";
                    //    ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                    //    return;
                    //}
                }
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetRecentChats()
    {
        try
        {
            StringBuilder strRecentChat = new StringBuilder();
            List<FreightChat> lstChatDates = new List<FreightChat>();
            lstChatDates = DBOperations.GetEnquiryChatDates(Convert.ToInt32(Session["VendorId"]), Convert.ToInt32(Session["MessageTo"]));
            if (lstChatDates.Count > 0)
            {
                strRecentChat.Append(@"<ul class='media-list slimscroll height-250' data-slimscroll-visible='true'>");
                for (int d = 0; d < lstChatDates.Count; d++)
                {
                    strRecentChat.Append(@"<div style='text-align: center; font-size: 12px;padding-top:5px'>
                                             <span style='background-color: #d9edf7; padding: 5px; border-radius: 5px; border: 1px solid lightblue;'>" +
                                             lstChatDates[d].FormattedDate.ToString() + "</span></div>");

                    List<FreightChat> lstGetChatHistory = new List<FreightChat>();
                    lstGetChatHistory = DBOperations.GetDayWsChatDetails(Convert.ToInt32(Session["VendorId"]), Convert.ToInt32(Session["MessageTo"]), lstChatDates[d].Date);
                    if (lstGetChatHistory.Count > 0)
                    {
                        strRecentChat.Append(@"<div style='padding: 15px; background-color: floralwhite; border-radius: 25px; border-bottom-right-radius: inherit;
                                                                        border-top-left-radius: inherit; border: 1px brown; border-style: dotted; margin-bottom: 15px'>");

                        for (int i = 0; i < lstGetChatHistory.Count; i++)
                        {
                            if (lstGetChatHistory[i].PersonNaming.ToString() != "" && lstGetChatHistory[i].PersonNaming.ToString() == "1")
                            {
                                strRecentChat.Append(@"<li class='media'>
                                                                <div class='media-body'>
                                                                <div class='media'>
                                                                    <a class='pull-left' href='#'></a>
                                                                    <div class='media-body'>" + lstGetChatHistory[i].Message.ToString());
                                strRecentChat.Append(@"<br /><small class='text-muted'>" + lstGetChatHistory[i].MessageTime.ToString() + "</small><hr/>");
                                strRecentChat.Append(@"</div></div></div></li>");
                            }
                            else
                            {
                                strRecentChat.Append(@"<li class='media' style='text-align: right'>
                                                               <div class='media-body'>
                                                                <div class='media'>
                                                                    <a class='pull-left' href='#'></a>
                                                                    <div class='media-body'>" + lstGetChatHistory[i].Message.ToString());
                                strRecentChat.Append(@"<br /><small class='text-muted'>" + lstGetChatHistory[i].MessageTime.ToString() + "</small><hr/>");
                                strRecentChat.Append(@"</div></div></div></li>");
                            }
                        }

                        strRecentChat.Append(@"</div>");
                    }
                }
                strRecentChat.Append(@"</ul>");
                dvRecentChats.InnerHtml = strRecentChat.ToString();
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetOnlineUsers()
    {
        try
        {
            StringBuilder strOnlineUsers = new StringBuilder();
            List<FreightChat> lstGetOnlineUsers = new List<FreightChat>();
            lstGetOnlineUsers = DBOperations.GetOnlineUsers(Convert.ToInt32(Session["VendorId"]));
            if (lstGetOnlineUsers.Count > 0)
            {
                strOnlineUsers.Append(@"<ul class='media-list slimscroll height-300' data-slimscroll-visible='true'>");
                for (int i = 0; i < lstGetOnlineUsers.Count; i++)
                {
                    if (lstGetOnlineUsers[i].UserName.ToString() != "")
                    {
                        string op = HttpUtility.UrlEncode(Encrypt("0"));
                        string lid = HttpUtility.UrlEncode(Encrypt(lstGetOnlineUsers[i].lUser.ToString()));
                        strOnlineUsers.Append(@"<li class='media'><div class='media-body'><div class='media'>
                                                <a class='pull-left' href='ViewAwardedEnq.aspx?op=" + op + "&id=" + lid + "' style='margin-right: 0px'>");

                        if (lstGetOnlineUsers[i].IsAvailable.ToString().ToLower() == "true")
                            strOnlineUsers.Append(@"<i style='font-size: 18px; padding-top: 12px; color: green;' class='fa fa-user'>");
                        else
                            strOnlineUsers.Append(@"<i style='font-size: 18px; padding-top: 12px; color: red;' class='fa fa-user'>");

                        strOnlineUsers.Append(@"</i></a><div class='media-body'>");
                        strOnlineUsers.Append(@"<h5 style='margin-bottom: 2px'>" + lstGetOnlineUsers[i].UserName.ToString());
                        if (lstGetOnlineUsers[i].TotalMsgs.ToString() != "0")
                            strOnlineUsers.Append(@"<span class='badge'>" + lstGetOnlineUsers[i].TotalMsgs.ToString() + "</span>");
                        strOnlineUsers.Append(@"</h5><small class='text-muted'>" + lstGetOnlineUsers[i].CurrentStatus.ToString() + "</small>");
                        strOnlineUsers.Append(@"</div></div></div></li>");
                    }
                }
                strOnlineUsers.Append(@"</ul>");
                dvOnlineUsers.InnerHtml = strOnlineUsers.ToString();
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void btnSendMsg_OnClick(object sender, EventArgs e)
    {
        try
        {
            int MessageTo = 0, lUser = 0, StatusId = 0;
            if (Convert.ToString(Session["MessageTo"]) != null && Convert.ToString(Session["MessageTo"]) != "")
                MessageTo = Convert.ToInt32(Session["MessageTo"]);
            lUser = Convert.ToInt32(Session["VendorId"]);
            StatusId = 0;
            if (MessageTo != 0)
            {
                if (txtMessage.Text.ToString().Trim() == "")
                {
                    ModalPopupExtender1.Show();
                    str_Message = "<script type='text/javascript' language='javascript'>" +
                                          "_toastr('Please type message!!','top-full-width','error',false); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                    return;
                }
                else
                {
                    int result = DBOperations.SendChatMessage(txtMessage.Text.Trim(), MessageTo, StatusId, lUser);
                    if (result == 0)
                    {
                        txtMessage.Text = "";
                        GetRecentChats();
                        GetOnlineUsers();
                        ModalPopupExtender1.Show();
                        str_Message = "<script type='text/javascript' language='javascript'>" +
                                           "_toastr('Message Sent..!!!','top-full-width','success',false); </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                        return;
                    }
                    else
                    {
                        str_Message = "<script type='text/javascript' language='javascript'>" +
                                           "_toastr('Message Sending Failed.','top-full-width','error',false); </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                        return;
                    }
                }
            }
            else
            {
                ModalPopupExtender1.Show();
                str_Message = "<script type='text/javascript' language='javascript'>" +
                                      "_toastr('Please select the user you want to chat with..!!!','top-full-width','error',false); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                return;
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void btnCancelPopup_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        if (Convert.ToString(Session["VendorId"]) != null)
        {
            LoadGridData();
            if (loggedInUser.glEmpName.ToString() != "")
                lblUserName.Text = loggedInUser.glEmpName.ToString();
            GetInboxMsgs();
            Session["MessageTo"] = "";
        }
        else
            Response.Redirect("FRlogin.aspx");
    }

    protected void msgAll(int msgType, string msg)
    {
        StringBuilder strDeviceList = new StringBuilder();

        switch (msgType)
        {
            case 1: // warning msg

                strDeviceList.Append(@" <div class='alert alert-mini alert-info margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                  <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Warning!</strong> " + msg + "</div>");//warning                   
                break;
            case 2: // error msg
                strDeviceList.Append(@" <div class='alert alert-mini alert-danger margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                    <span aria-hidden='true'>×</span><span class='sr-only'>Close</span> </button> <strong>Error!</strong> " + msg + "</div>");//error
                break;

            case 3: // success msg
                strDeviceList.Append(@" <div class='alert alert-mini alert-success margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                     <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Success!</strong> " + msg + "</div>");//Successes
                break;
        }
        dvMsgAll.InnerHtml = strDeviceList.ToString();
    }

    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    #endregion
}