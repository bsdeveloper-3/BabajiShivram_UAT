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

public partial class FreightEnqModule_FreightTrackingDetails : System.Web.UI.Page
{
    string str_Message = "";
    private static Random _random = new Random();
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["VendorId"]) != null)
            {
                txtStatusDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["id"].ToString() != null && Request.QueryString["id"].ToString() != "")
                    {
                        hdnEnqId.Value = Request.QueryString["id"].ToString();
                        GetEnqDetails();
                        GetFreightEnqDoc();
                        GetStatusHistory();
                        if (loggedInUser.glEmpName.ToString() != "")
                            lblUserName.Text = loggedInUser.glEmpName.ToString();
                    }

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

    protected void btnSaveStatus_OnClick(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(hdnEnqId.Value);
        int result = -123;

        if (EnqId > 0) // If EnquiryId Session Not Expired. Update Status Details
        {
            DateTime dtStatusDate = DateTime.Today;

            int StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
            if (txtStatusDate.Text.Trim() != "")
                dtStatusDate = Commonfunctions.CDateTime(txtStatusDate.Text.Trim());

            if (StatusId <= 0)
            {
                msgAll_Status(2, "Please Select Current Freight Status..!!");
                return;
            }
            else
                result = DBOperations.UpdateFreightStatus(EnqId, StatusId, dtStatusDate,0, txtRemarks.Text.Trim(), Convert.ToInt32(Session["VendorId"]));

            if (result == 0)
            {
                msgAll_Status(3, "Status Changed Successfully.");
                GetEnqDetails();
                GetFreightEnqDoc();
                GetStatusHistory();
            }

            else if (result == 2)
            {
                msgAll_Status(2, "Status already been updated..!!");
            }

            else
            {
                msgAll_Status(2, "System Error! Please try after sometime.");
            }
        }
        else
        {
            Response.Redirect("EnquiryTracking.aspx");
        }
    }

    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        txtRemarks.Text = "";
        ddlStatus.SelectedValue = "0";
        txtStatusDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void GetEnqDetails()
    {
        List<AlibabaFreightTracking> mList = new List<AlibabaFreightTracking>();
        mList = DBOperations.GetEnqDetailAsPerEnqId(Convert.ToInt32(hdnEnqId.Value.ToString().Trim()));

        StringBuilder strDeviceList = new StringBuilder();
        strDeviceList.Append(@"<table id='sample_2' class='table table-striped table-bordered table-hover'><tbody>");
        foreach (var i in mList)
        {
            strDeviceList.Append(@"<tr><th>Enquiry Ref No</th><td><b>" +
                  i.EnqRefNo + "</b></td><th>Enquiry Date</th><td>" +
                  Convert.ToDateTime(i.EnqDate).ToString("dd/MMM/yyyy") + "</td></tr><tr><th>Customer</th><td>" +
                  i.CustomerName.ToUpper() + "</td><th>Consignee</th><td>" +
                  i.Consignee.ToUpper() + "</td></tr><th>Enquiry Mode</th><td>" +
                  i.Mode.ToUpper() + "</td><th>Terms</th><td>" +
                  i.Terms.ToUpper() + "</td></tr><tr><th>Port Of Loading</th><td>" +
                  i.PortOfLoading.ToUpper() + "</td><th>Port Of Discharge</th><td>" +
                  i.PortOfDischarge.ToUpper() + "</td></tr><tr><th>IEC</th><td>" +
                  i.IEC.ToUpper() + "</td><th>Shipper</th><td>" +
                  i.Shipper.ToUpper() + "</td></tr><th>Shipper Address</th><td>" +
                  i.ShipperAddress + "</td><th>Shipper Address Pin Code</th><td>" +
                  i.ShipperAddPinCode + "</td></tr><th>Commodity</th><td>" +
                  i.Commodity + "</td><th>Shipment Type</th><td>" +
                  i.ShipmentType.ToUpper() + "</td></tr><th>No of Pkgs</th><td>" +
                  i.NoofPkgs + "</td><th>Quantity</th><td>" +
                  i.Quantity + "</td></tr><th>Weight (Kgs)</th><td>" +
                  i.Weight + "</td><th>Is Dangerous Goods?</th><td>" +
                  i.IsDgGoods.ToUpper() + "</td></tr><tr><th>HS Code</th>");
            if (i.HsCode.ToString() == "0.00")
                strDeviceList.Append(@"<td></td>");
            else
                strDeviceList.Append(@"<td>" + i.HsCode + "</td>");
            if (i.InvoiceValue.ToString() == "0.00")
                strDeviceList.Append(@"<th>Invoice Value</th><td></td>");
            else
                strDeviceList.Append(@"<th>Invoice Value</th><td>" + i.InvoiceValue + "</td>");
            strDeviceList.Append(@"</tr><tr><th>Delivery Address</th><td>" +
            i.DeliveryAddress + "</td><th>Delivery Address Pin Code</th><td>" +
            i.DeliveryAddPincode + "</td></tr><tr><th>Product Link</th><td>" +
            i.ProductLink + "</td><th>Dimension</th><td>" + i.DimensionLength + "(L) * " + i.DimensionWidth + "(W) * " + i.DimensionHeight + "(H) " + "cms</td></tr>");
            hdnDocDir.Value = i.DocDir.ToString();
            hdnStatusId.Value = i.StatusId.ToString();
        }

        strDeviceList.Append("</tbody></table>");
        EnqDetails.InnerHtml = strDeviceList.ToString();

    }

    protected void GetFreightEnqDoc()
    {
        try
        {
            List<AlibabaFreightTracking> mDocList = new List<AlibabaFreightTracking>();
            mDocList = DBOperations.GetFreightEnqDoc(Convert.ToInt32(hdnEnqId.Value.Trim()));
            if (mDocList.Count > 0)
                ViewState["FreightDocList"] = mDocList;
            else
                ViewState["FreightDocList"] = "";
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetStatusHistory()
    {
        List<AlibabaFreightTracking> mListStatusHistory = new List<AlibabaFreightTracking>();
        mListStatusHistory = DBOperations.GetEnqStatusHistory(Convert.ToInt32(hdnEnqId.Value.ToString().Trim()));

        StringBuilder strDeviceList = new StringBuilder();
        strDeviceList.Append(@"<table id='sample_3' class='table table-striped table-bordered table-hover' style='margin-bottom:5px'><tbody>");
        strDeviceList.Append(@"<tr><th>Sr.No.</th><th>Status</th><th>Date</th><th>User</th><th>Remarks</th></tr>");
        for (int i = 0; i < mListStatusHistory.Count; i++)
        {
            int cnt = i + 1;
            strDeviceList.Append(@"<tr><td>"
                  + cnt.ToString() + "</b></td><td>" + mListStatusHistory[i].Status + "</td><td>" +
                  Convert.ToDateTime(mListStatusHistory[i].StatusDate).ToString("dd/MMM/yyyy") + "</td><td>" + mListStatusHistory[i].lUser + "</td><td>"
                  + mListStatusHistory[i].Remarks + "</td></tr>");
        }
        strDeviceList.Append(@"</tbody></table>");
        if (hdnStatusId.Value == "2")
            strDeviceList.Append(@"<div class='row'><div class='pull-right' style='padding-right: 25px;'><button type='button' class='btn btn-xs btn-primary' onclick='javascript:ResetControls();' data-toggle='modal' data-target='.bs-example-modal-lg'>Change Status</button></div></div>");
        dvStatusHistory.InnerHtml = strDeviceList.ToString();
    }

    #region FREIGHT DOCUMENTS EVENTS

    protected void btnUploadDoc_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (fil_doc.HasFile)
            {
                int EnqId = Convert.ToInt32(hdnEnqId.Value.ToString().Trim());
                int DocResult = -123;
                string strDocFolder = "FreightDoc\\" + hdnDocDir.Value.Trim() + "\\";

                string strFilePath = UploadDocument(strDocFolder);
                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddFreightDocument(EnqId, txtDocumentName.Text.Trim(), strFilePath, Convert.ToInt32(Session["VendorId"]));

                    if (DocResult == 0)
                    {
                        msgAll(3, "Document Uploaded Successfully.");
                        txtDocumentName.Text = "";
                        GetEnqDetails();
                        GetFreightEnqDoc();
                    }
                    else if (DocResult == 2)
                    {
                        msgAll(1, "Document Name Already Exists! Please change the document name.");
                    }
                    else
                    {
                        msgAll(2, "Error while uploading document. Please try again later.");
                    }
                }
            }
            else
            {
                msgAll(2, "Please select document.");
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void btnClearDoc_OnClick(object sender, EventArgs e)
    {
        txtDocumentName.Text = "";
        dvErrorsList.InnerHtml = "";
        dvFreightErrors.InnerHtml = "";
        GetEnqDetails();
        GetFreightEnqDoc();
    }

    protected string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }

        string FileName = fil_doc.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fil_doc.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fil_doc.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
    }

    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #endregion

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
        dvErrorsList.InnerHtml = strDeviceList.ToString();
    }

    protected void msgAll_Status(int msgType, string msg)
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
        dvErrorStatus.InnerHtml = strDeviceList.ToString();
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

    //protected void GetInboxMsgs()
    //{
    //    try
    //    {
    //        StringBuilder strBuilder = new StringBuilder();
    //        DataSet dsGetTotalMsgsRec = new DataSet();
    //        dsGetTotalMsgsRec = DBOperations.GetTotalMsgReceived(Convert.ToInt32(Session["VendorId"]));
    //        if (dsGetTotalMsgsRec != null && dsGetTotalMsgsRec.Tables.Count > 0)
    //        {
    //            if (dsGetTotalMsgsRec.Tables[0].Rows.Count > 0)
    //            {
    //                string op = HttpUtility.UrlEncode(Encrypt("1"));
    //                strBuilder.Append(@"<a href='Dashboard.aspx?op=" + op + "'><i class='fa fa-envelope'></i> Inbox" +
    //                                    "<span class='pull-right label label-default'>" + dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString() + "</span>" +
    //                                    "</a>");

    //                li_Inbox.InnerHtml = strBuilder.ToString();

    //                int Msgs = Convert.ToInt32(dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString());
    //                if (Msgs > 0)
    //                {
    //                    str_Message = "<script type='text/javascript' language='javascript'>" +
    //                                         "_toastr('You have " + Msgs + " messages received in inbox...!!!','top-full-width','info',false); </script>";
    //                    ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception en)
    //    {
    //        throw (en);
    //    }
    //}

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
                                                    <a class='pull-left' href='Dashboard.aspx?op=" + op + "&id=" + lid + "' style='margin-right: 0px'>");

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
            if (loggedInUser.glEmpName.ToString() != "")
                lblUserName.Text = loggedInUser.glEmpName.ToString();
            Session["MessageTo"] = "";
        }
        else
            Response.Redirect("FRlogin.aspx");
    }

//    protected void msgAll_popup(int msgType, string msg)
//    {
//        StringBuilder strDeviceList = new StringBuilder();

//        switch (msgType)
//        {
//            case 1: // warning msg

//                strDeviceList.Append(@" <div class='alert alert-mini alert-info margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
//                                      <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Warning!</strong> " + msg + "</div>");//warning                   
//                break;
//            case 2: // error msg
//                strDeviceList.Append(@" <div class='alert alert-mini alert-danger margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
//                                        <span aria-hidden='true'>×</span><span class='sr-only'>Close</span> </button> <strong>Error!</strong> " + msg + "</div>");//error
//                break;

//            case 3: // success msg
//                strDeviceList.Append(@" <div class='alert alert-mini alert-success margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
//                                         <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Success!</strong> " + msg + "</div>");//Successes
//                break;
//        }
//        dvMsgAll.InnerHtml = strDeviceList.ToString();
//    }

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