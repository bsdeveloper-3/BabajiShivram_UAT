using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;

public partial class FreightExport_ExpOperation : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnUpload); 
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("ExpPendingOperation.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Export Operation";
            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExpPendingOperation.aspx");
    }

    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetExpBookingDetail(EnqId);

        string strMode = "";  // lMode

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            if (dsBookingDetail.Tables[0].Rows[0]["FRJobNo"] != DBNull.Value)
            {
                strMode =  dsBookingDetail.Tables[0].Rows[0]["lMode"].ToString();
                hdnModeId.Value = strMode;

                if (strMode=="1")  // Air
                {
                    lblContainerPick.Text = "Cargo Pickup Date";
                    lblCustomDate.Text = "Registration Date";
                    trAirMode.Visible = true;

                    trStuffing.Visible = false;
                    trCarting.Visible = true;

                }
                else
                {
                    trAirMode.Visible = false;
                    trStuffing.Visible = true;
                    trCarting.Visible = false;
                }            

                lblJobNo.Text       = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
                lblEnquiryNo.Text   = dsBookingDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();

                hdnUploadPath.Value = dsBookingDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
                //lblBookingMonth.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");

                lblBranch.Text      =   dsBookingDetail.Tables[0].Rows[0]["BranchName"].ToString();
                lblShipper.Text     =   dsBookingDetail.Tables[0].Rows[0]["ShipperName"].ToString();
                lblShipperAddr.Text =   dsBookingDetail.Tables[0].Rows[0]["ShipperAddress"].ToString();
                lblCountry.Text     =   dsBookingDetail.Tables[0].Rows[0]["CountryName"].ToString();
                lblConsignee.Text   =   dsBookingDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblTypeofExport.Text =  dsBookingDetail.Tables[0].Rows[0]["ExportTypeName"].ToString();
                lblConsignAddr.Text =   dsBookingDetail.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
                lblPortLoading.Text =   dsBookingDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
                lblPortDischarge.Text = dsBookingDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
                lblCustomer.Text    =   dsBookingDetail.Tables[0].Rows[0]["Customer"].ToString();
                lblTerms.Text       =   dsBookingDetail.Tables[0].Rows[0]["TermsName"].ToString();
                lblContainer20.Text =   dsBookingDetail.Tables[0].Rows[0]["CountOf20"].ToString();
                lblContainer40.Text =   dsBookingDetail.Tables[0].Rows[0]["CountOf40"].ToString();
                lblLCLCBL.Text      =   dsBookingDetail.Tables[0].Rows[0]["ContainerType"].ToString();
                lblLCLFCL.Text      =   dsBookingDetail.Tables[0].Rows[0]["ContainerTypeName"].ToString();
                lblNoPackages.Text  =   dsBookingDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
                lblTypePacking.Text =   dsBookingDetail.Tables[0].Rows[0]["PackageType"].ToString();

                lblGrossWt.Text     =   dsBookingDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
                lblChargeable.Text  =   dsBookingDetail.Tables[0].Rows[0]["ChargeableWeight"].ToString();

                txtSBNo.Text        =   dsBookingDetail.Tables[0].Rows[0]["SBNO"].ToString();

                txtSBNo.Text        =   dsBookingDetail.Tables[0].Rows[0]["SBNO"].ToString();
                txtShipBookingNo.Text = dsBookingDetail.Tables[0].Rows[0]["ShipLineBookingNo"].ToString();

                if (dsBookingDetail.Tables[0].Rows[0]["SBDate"] != DBNull.Value)
                    txtSBDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["SBDate"]).ToString("dd/MM/yyyy");

                //if (dsBookingDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                //    txtContPickDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");

                //if (dsBookingDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                //    txtCustomPermiDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");

                //if (dsBookingDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                //    txtStuffingDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");

                //if (dsBookingDetail.Tables[0].Rows[0]["ChequeDate"] != DBNull.Value)
                //    txtCLPDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["ChequeDate"]).ToString("dd/MM/yyyy");

            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        /********************************************************************************/

        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int ModeId = 0;
        string SBNo = "", ASIBy = "", ShipLineBookingNo = "", VesselName = "", PlaceOfDelivery = "", CHAJobNo = "";

        DateTime SBDate = DateTime.MinValue, ContPickupDate = DateTime.MinValue, CustomDate = DateTime.MinValue, 
            StuffingDate = DateTime.MinValue, CLPDate = DateTime.MinValue, ASIDate = DateTime.MinValue, CartingDate = DateTime.MinValue;

        if (txtSBDate.Text.Trim() != "")
        {
            SBDate = Commonfunctions.CDateTime(txtSBDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter SB Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtContPickDate.Text.Trim() != "")
        {
            ContPickupDate = Commonfunctions.CDateTime(txtContPickDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter PickUp Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtCustomPermiDate.Text.Trim() != "")
        {
            CustomDate = Commonfunctions.CDateTime(txtCustomPermiDate.Text.Trim());
        }       

        if (txtStuffingDate.Text.Trim() != "")
        {
            StuffingDate = Commonfunctions.CDateTime(txtStuffingDate.Text.Trim());
        }       

        if (txtCLPDate.Text.Trim() != "")
        {
            CLPDate = Commonfunctions.CDateTime(txtCLPDate.Text.Trim());
        }       

        SBNo = txtSBNo.Text.Trim();

        ASIBy = txtASIBy.Text.Trim();

        ShipLineBookingNo = txtShipBookingNo.Text.Trim();

        VesselName = txtVessel.Text.Trim();

        PlaceOfDelivery = txtPlaceOfDelivery.Text.Trim();

        CHAJobNo = txtCHAJobNo.Text.Trim();

        if (txtASIDate.Text.Trim() != "")
        {
            ASIDate = Commonfunctions.CDateTime(txtASIDate.Text.Trim());
        }

        if (txtCartingDate.Text.Trim() != "")
        {
            CartingDate = Commonfunctions.CDateTime(txtCartingDate.Text.Trim());
        }        

        if (hdnModeId.Value != "")
        {
            ModeId = Convert.ToInt32(hdnModeId.Value);
        }

        //int result = DBOperations.AddExportOperation(EnqId,SBNo, SBDate, ContPickupDate, CustomDate,StuffingDate,CLPDate, ASIBy, ASIDate, CartingDate, ModeId, LoggedInUser.glUserId);

        int result = DBOperations.AddExportOperation(EnqId, SBNo, SBDate, ContPickupDate, CustomDate, StuffingDate, CLPDate, ASIBy, ASIDate, CartingDate, ShipLineBookingNo, VesselName, PlaceOfDelivery, CHAJobNo, ModeId, LoggedInUser.glUserId);


        if (result == 0)
        {
            string strReturnMessage = "Export Operation Detail Updated Successfully!";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='ExpPendingOperation.aspx';", true);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Export Operation Detail Already Exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
        /*******************************************************/
    }

    #region Document

    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "removedocument")
        {
            int DocId = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightDocument(DocId, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Document deleted successfully!";
                lblError.CssClass = "success";

                gvFreightDocument.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Document details not found";
                lblError.CssClass = "errorMsg";

                gvFreightDocument.DataBind();
            }
        }
    }

    protected void btnFileUpload_Click(Object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int DocResult = -123;

        //if (txtDocName.Text.Trim() == "")
        //{
        //    lblError.Text = "Please enter the document name.";
        //    lblError.CssClass = "errorMsg";
        //    return;
        //}

        if (EnqId > 0) // If EnquiryId Session Not Expired.Update Status Details
        {
            if (fuDocument.HasFile) // Add Enquiry Document
            {
                string strDocFolder = "FreightDoc\\" + hdnUploadPath.Value + "\\";

                string strFilePath = UploadDocument(strDocFolder);
                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddFreightDocument(EnqId, ddl_DocumentType.SelectedItem.ToString(), strFilePath, LoggedInUser.glUserId);

                    if (DocResult == 0)
                    {
                        lblError.Text = "Document uploaded successfully.";
                        lblError.CssClass = "success";
                        gvFreightDocument.DataBind();
                        gvFreightAttach.DataBind();
                    }
                    else if (DocResult == 1)
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (DocResult == 2)
                    {
                        lblError.Text = "Document Name Already Exists! Please change the document name!.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }//END_IF_FileExists
            else
            {
                lblError.Text = "Please attach the document for upload!";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF_Enquiry
        else
        {
            Response.Redirect("FreightTracking.aspx");
        }

    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "" || ServerPath ==null)
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }
        string FileName = fuDocument.FileName;
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

       // ServerFilePath = "C:\\inetpub\\wwwroot\\BabajiShivram\\UploadFiles\\" + FilePath;

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }


        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);
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

    #endregion Document End

    protected void lnkPreAlertEmailDraft_Click(object sender, EventArgs e)
    {
        //txtCustEmail.Text = txtCustEmail.Text.Replace(";", ",").Trim();

        //txtCustEmail.Text = txtCustEmail.Text.Replace(" ", "");
        //txtCustEmail.Text = txtCustEmail.Text.Replace(",,", "");

        //txtCustEmail.Text = txtCustEmail.Text.TrimEnd(',');

       

        // int intCommaIndex = txtCustEmail.Text.LastIndexOf(",");

        // if (intCommaIndex > 0)
        //     txtCustEmail.Text =   txtCustEmail.Text.Remove(intCommaIndex);

      
            GenerateEmailDraft();
        //else
        //{
        //    lblError.Text = "Invalid Email Address, Please Enter Comma-Seperated Email";
        //    lblError.CssClass = "errorMsg";
        //}
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        if (txtCustomerEmail.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Customer Email & Subject!";
            lblError.CssClass = "errorMsg";
            ModalPopupEmail.Hide();
        }
        else
        {
            // Send Email
            bool bEMailSucess = SendPreAlertEmail();

            // Update PreAlert Email Sent Status and Customer Email 

            if (bEMailSucess == true)
            {
                int Result = 0;// DBOperations.UpdateCustomerPreAlertEmailStatus(EnqId, txtCustomerEmail.Text.Trim(), LoggedInUser.glUserId);

                ModalPopupEmail.Hide();

                if (Result == 0)
                {
                    lblError.Text = "Export Operation Email Sent Successfully!";
                    lblError.CssClass = "success";
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "Export Operation Email Already Sent!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    private void GenerateEmailDraft()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strCustomerEmail = "amit.bakshi@babajishivram.com";//txtCustEmail.Text.Trim();

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", "");
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        txtCustomerEmail.Text = strCustomerEmail;
        txtMailCC.Text = LoggedInUser.glUserName;

        string EmailContent = "";
        string MessageBody = "";
                
        txtSubject.Text = "Export Operation" + "-" + lblJobNo.Text.Trim() + " // " + lblEnquiryNo.Text.Trim() + " // " + lblCustomer.Text.Trim() + " // " + lblPortLoading.Text.Trim() + " // " + lblPortDischarge.Text.Trim();

        string strEnqRefNo = "", strHAWNo = "", strMAWNo = "", strShipper = "", strConsignee = "", strCustomer="", strPortLoading = "",
            strPortDischarge = "", strVesselName = "", strETADate = "", strNoOfPkgs = "",
            strGrossWT = "", strInvoiceNo = "", strPONo = "", strDescription = "", strFinalAgent = "";

        string BookingDate = "", ExportType = "", CartingPoint = "", CHABy = "", TransportBy = "";

        string strGSTN = "", strPlaceOfSupply = "", FRJobNo = "";
        string SBDate = "", ContPickDate = "", CustomPermiDate = "", StuffingDate = "", CLPDate = "", strSBNo = "";
        string strShipBookingNo = "", strVessel = "", strPlaceOfDelivery = "", strCHAJobNo = "";

        // DataSet dsAlertDetail = DBOperations.GetCustomerPreAlertDetail(EnqId);
        DataSet dsAlertDetail = DBOperations.GetExpBookingDetail(EnqId);

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strEnqRefNo = dsAlertDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            FRJobNo = dsAlertDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            strShipper = dsAlertDetail.Tables[0].Rows[0]["ShipperName"].ToString();
            strCustomer = dsAlertDetail.Tables[0].Rows[0]["Customer"].ToString();
            strConsignee = dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strPortLoading = dsAlertDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
            strPortDischarge = dsAlertDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            //strVesselName = dsAlertDetail.Tables[0].Rows[0]["VesselName"].ToString();
            //strVesselName += " - " + dsAlertDetail.Tables[0].Rows[0]["VesselNumber"].ToString();

            strNoOfPkgs = dsAlertDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            strGrossWT = dsAlertDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
            strInvoiceNo = dsAlertDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            strPONo = dsAlertDetail.Tables[0].Rows[0]["PONumber"].ToString();
            //strDescription = dsAlertDetail.Tables[0].Rows[0]["CargoDescription"].ToString();

            //BookingDate = dsAlertDetail.Tables[0].Rows[0]["BookingDate"].ToString();

            ExportType = dsAlertDetail.Tables[0].Rows[0]["ExportTypeName"].ToString();
            CartingPoint = dsAlertDetail.Tables[0].Rows[0]["CartingPoint"].ToString();
            CHABy = dsAlertDetail.Tables[0].Rows[0]["CHAByName"].ToString();
            TransportBy = dsAlertDetail.Tables[0].Rows[0]["TransportByName"].ToString();
                                    
            strSBNo = txtSBNo.Text.Trim();
            strShipBookingNo = txtShipBookingNo.Text.Trim();

            strVessel = txtVessel.Text.Trim();

            strPlaceOfDelivery = txtPlaceOfDelivery.Text.Trim();

            strCHAJobNo = txtCHAJobNo.Text.Trim();


            if (dsAlertDetail.Tables[0].Rows[0]["BookingDate"] != DBNull.Value)
            {
                BookingDate = Convert.ToDateTime(dsAlertDetail.Tables[0].Rows[0]["BookingDate"]).ToString("dd/MM/yyyy");
            }

            if (txtSBDate.Text.Trim() != "")
            {
                SBDate = Convert.ToDateTime(txtSBDate.Text.Trim()).ToString("dd/MM/yyyy");
            }
            if (txtContPickDate.Text.Trim() != "")
            {
                ContPickDate = Convert.ToDateTime(txtContPickDate.Text.Trim()).ToString("dd/MM/yyyy");
            }
            if (txtCustomPermiDate.Text.Trim() != "")
            {
                CustomPermiDate = Convert.ToDateTime(txtCustomPermiDate.Text.Trim()).ToString("dd/MM/yyyy");
            }
            if (txtStuffingDate.Text.Trim() != "")
            {
                StuffingDate = Convert.ToDateTime(txtStuffingDate.Text.Trim()).ToString("dd/MM/yyyy");
            }
            if (txtCLPDate.Text.Trim() != "")
            {
                CLPDate = Convert.ToDateTime(txtCLPDate.Text.Trim()).ToString("dd/MM/yyyy");
            }
        }
        else
            {
                lblError.Text = "Operation Details Not Found! Please check details.";
                lblError.CssClass = "errorMsg";
                return;
            }
            try
        {
            //string strFileName = "../EmailTemplate/FOP_EmailCustPreAlert.txt";
            string strFileName = "../EmailTemplate/FOP_OperationMail.txt";

            StreamReader sr = new StreamReader(Server.MapPath(strFileName));
            sr = File.OpenText(Server.MapPath(strFileName));
            EmailContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.CssClass = "errorMsg";   
            return;
        }

        MessageBody = EmailContent.Replace("@ENQRefNo", strEnqRefNo);
        MessageBody = MessageBody.Replace("@FRJobNo", FRJobNo);
        //MessageBody = MessageBody.Replace("@EnqRefNo", strEnqRefNo);
        MessageBody = MessageBody.Replace("@Shipper", strShipper);
        MessageBody = MessageBody.Replace("@Customer", strCustomer);
        MessageBody = MessageBody.Replace("@Consignee", strConsignee);
        MessageBody = MessageBody.Replace("@LoadingPortName", strPortLoading);
        MessageBody = MessageBody.Replace("@PortOfDischargedName", strPortDischarge);

        MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPkgs);
        MessageBody = MessageBody.Replace("@GrossWeight", strGrossWT);
        MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
        MessageBody = MessageBody.Replace("@PONumber", strPONo);

        MessageBody = MessageBody.Replace("@ExportType", ExportType);        
        MessageBody = MessageBody.Replace("@CartingPoint", CartingPoint);
        MessageBody = MessageBody.Replace("@CHABy", CHABy);
        MessageBody = MessageBody.Replace("@TransportBy", TransportBy);

        MessageBody = MessageBody.Replace("@SBNo", strSBNo);
        MessageBody = MessageBody.Replace("@SBDate", SBDate);
        MessageBody = MessageBody.Replace("@ContPickDate", ContPickDate);
        MessageBody = MessageBody.Replace("@CustomPermiDate", CustomPermiDate);
        MessageBody = MessageBody.Replace("@StuffingDate", StuffingDate);
        MessageBody = MessageBody.Replace("@ShipLineBookingNo", strShipBookingNo);
        MessageBody = MessageBody.Replace("@VesselName", strVessel);
        MessageBody = MessageBody.Replace("@PlaceOfDelivery", strPlaceOfDelivery);
        MessageBody = MessageBody.Replace("@CHAJobNo", strCHAJobNo);

        MessageBody = MessageBody.Replace("@CLPDate", CLPDate);

        MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);

        divPreviewEmail.InnerHtml = MessageBody;

        ModalPopupEmail.Show();
    }
    private bool SendPreAlertEmail()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtCustomerEmail.Text.Trim();
        strCCEmail = txtMailCC.Text.Trim();
        strSubject = txtSubject.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", "");
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();

        // int intIndex = strCCEmail.LastIndexOf(",");

        // if (intIndex > 0)
        //     strCCEmail = strCCEmail.Remove(intIndex);

        bool bEmailSucces = false;

        if (strCustomerEmail == "" || strSubject == "")
        {
            lblPopMessageEmail.Text = "Please Enter Customer Email!";
            lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            MessageBody = divPreviewEmail.InnerHtml;

            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvFreightAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }
    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }

    protected void txtContPickDate_TextChanged(object sender, EventArgs e)
    {
        if (txtSBDate.Text.Trim() != "")
        {
            DateTime SBDate = Convert.ToDateTime(txtSBDate.Text.Trim());
            DateTime ContainerPick = Convert.ToDateTime(txtContPickDate.Text.Trim());
            if (SBDate < ContainerPick)
            {
                lblError.Text = "PickUp Date is before than SB Date ";
                lblError.CssClass = "errorMsg";
                txtContPickDate.Text = "";
            }
        }
        else
        {
            lblError.Text = "Fill the first SB Date";
            lblError.CssClass = "errorMsg";
            txtCustomPermiDate.Text = "";
        }

    }

    protected void txtCustomPermiDate_TextChanged(object sender, EventArgs e)
    {
        if (txtSBDate.Text.Trim() != "")
        {
            DateTime SBDate = Convert.ToDateTime(txtSBDate.Text.Trim());
            DateTime CustomPermiDate = Convert.ToDateTime(txtCustomPermiDate.Text.Trim());
            if (SBDate > CustomPermiDate)
            {
                lblError.Text = "Custom Permission Date is greater than SB Date ";
                lblError.CssClass = "errorMsg";
                txtCustomPermiDate.Text = "";
            }
        }
        else
        {
            lblError.Text = "Fill the first SB Date";
            lblError.CssClass = "errorMsg";
            txtCustomPermiDate.Text = "";
        }
    }

    protected void txtStuffingDate_TextChanged(object sender, EventArgs e)
    {
        if(txtCustomPermiDate.Text.Trim()!="")
        {
            DateTime CustomPermiDate = Convert.ToDateTime(txtCustomPermiDate.Text.Trim());
            DateTime StuffingDate = Convert.ToDateTime(txtStuffingDate.Text.Trim());
            if (CustomPermiDate > StuffingDate)
            {
                lblError.Text = "Stuffing Date is greater than Custom Permission Date ";
                lblError.CssClass = "errorMsg";
                txtStuffingDate.Text = "";
            }
        }
        else
        {
            lblError.Text = "Fill the first Custom Permission Date";
            lblError.CssClass = "errorMsg";
            txtStuffingDate.Text = "";
        }
    }

    protected void txtCLPDate_TextChanged(object sender, EventArgs e)
    {
        if (txtStuffingDate.Text.Trim() != "")
        {
            DateTime StuffingDate = Convert.ToDateTime(txtStuffingDate.Text.Trim());
            DateTime CLPDate = Convert.ToDateTime(txtCLPDate.Text.Trim());
            if (StuffingDate > CLPDate)
            {
                lblError.Text = "CLP Date is greater than stuffing Date ";
                lblError.CssClass = "errorMsg";
                txtCLPDate.Text = "";
            }
        }
        else
        {
            lblError.Text = "Fill the first Stuffing Date";
            lblError.CssClass = "errorMsg";
            txtCLPDate.Text = "";
        }
    }
}