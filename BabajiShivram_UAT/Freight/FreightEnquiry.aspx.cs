using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using AjaxControlToolkit;

public partial class Freight_FreightEnquiry : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Enquiry";
            lblEnquiryRefNo.Text = DBOperations.GetNewFreightEnquiryNo();
        
            // Allow  Only Future Date For Reminder
            CalRemindDate.StartDate = DateTime.Today.AddDays(1);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strEnqRefNo,strCustRefNo, strCustomer, strShipper, strConsignee, strAgentName, strRemarks;
        int FreightType, FreightMode = 0,SalesRepId = 0, CountryId = 0, PortOfLoadingId = 0, PortOfDischargeId = 0,
            TermsId = 0, AssignedToEmpId = 0,EnquiryValue = 0;
        int Count20 = 0, Count40 = 0, ContainerType = 0, NoOfpackages = 0;
        string strLCLVolume = "0", strGrossWeight = "0", strChargeableWeight = "0", strContainerSubType = "";

        bool IsDangerousGood = false; string strFilePath = "";

        strEnqRefNo     =   lblEnquiryRefNo.Text.Trim();
        strCustRefNo    =   txtCustRefNo.Text.Trim(); 
        strCustomer     =   txtCustomer.Text.ToUpper().Trim();

        strShipper      =   txtShipper.Text.ToUpper().Trim();
        strConsignee    =   txtConsignee.Text.ToUpper().Trim(); 
        strAgentName    =   txtAgent.Text.Trim();
        
        if(hdnSalesRepId.Value != "")
            SalesRepId      =   Convert.ToInt32(hdnSalesRepId.Value);
        
        if(ddTransferEmp.SelectedValue != "0")
            AssignedToEmpId = Convert.ToInt32(ddTransferEmp.SelectedValue);

        if (AssignedToEmpId == 0)   // SET Logged In User AS Enquiry User
            AssignedToEmpId = LoggedInUser.glUserId;


        
        FreightType     =   Convert.ToInt32(ddFreightType.SelectedValue); 
        FreightMode     =   Convert.ToInt32(ddFreightMode.SelectedValue); 
        CountryId       =   Convert.ToInt32(hdnCountryId.Value);  
        PortOfLoadingId =   Convert.ToInt32(hdnLoadingPortId.Value);
        PortOfDischargeId = Convert.ToInt32(hdnPortOfDischargedId.Value);   
        TermsId         =   Convert.ToInt32(ddTerms.SelectedValue);
        if(txtEnquiryValue.Text!=null && txtEnquiryValue.Text!="")
            EnquiryValue = Convert.ToInt32(txtEnquiryValue.Text);
        strRemarks      =   txtRemark.Text.Trim();

	if (lbAgentContact.SelectedIndex == -1 & FreightType == 1)
        {
            lblError.Text = "Agent Information Not Found! Please Select Agent Details.";
            lblError.CssClass = "errorMsg";
            return ;
        }

        if (strCustomer == "")
        {
            lblError.Text       = "Please Enter Customer Name!.";
            lblError.CssClass   = "errorMsg";
            return;
        }
        else if (strCustRefNo == "")
        {
            lblError.Text       = "Please Enter Ref No/Email Ref!.";
            lblError.CssClass   = "errorMsg";
            return;
        }
        else if (PortOfDischargeId == 0)
        {
            lblError.Text       = "Please Select Port Of Discharged From Search List!.";
            lblError.CssClass   = "errorMsg";
            txtPortOfDischarged.Focus();
            return;
        }
        else if (SalesRepId == 0)
        {
            lblError.Text       = "Please Select Sales Reps Name From Search List!.";
            lblError.CssClass   = "errorMsg";
            txtSalesRep.Focus();
            return;
        }

        if (rdlGoodsType.SelectedIndex != -1)
        {
            IsDangerousGood = Convert.ToBoolean(rdlGoodsType.SelectedValue);
        }

        if (ddFreightMode.SelectedValue == "1")// For Air
        {
            if (txtNoOfPkgs.Text.Trim() != "")
            {
                NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
            }
        }
        else // For Sea Or Breakbulk
        {
            ContainerType = Convert.ToInt32(ddFCL.SelectedValue);
            strContainerSubType = ddSubType.SelectedValue;

            if (txtCont20.Text.Trim() != "")
            {
                Count20 = Convert.ToInt32(txtCont20.Text.Trim());
            }

            if (txtCont40.Text.Trim() != "")
            {
                Count40 = Convert.ToInt32(txtCont40.Text.Trim());
            }

            if (txtLCLVolume.Text.Trim() != "")
            {
                strLCLVolume = txtLCLVolume.Text.Trim();
            }
        }
        if (txtGrossWeight.Text.Trim() != "")
        {
            strGrossWeight = txtGrossWeight.Text.Trim();
        }

        if (txtChargWeight.Text.Trim() != "")
        {
            strChargeableWeight = txtChargWeight.Text.Trim();
        }

        int EnqId =  DBOperations.AddFreightEnquiry(strEnqRefNo, FreightType, FreightMode, strCustRefNo, strCustomer, strShipper, strConsignee,
                 CountryId, PortOfLoadingId, PortOfDischargeId, TermsId,EnquiryValue, strAgentName, SalesRepId, Count20, Count40, ContainerType, strContainerSubType,
                 strLCLVolume, NoOfpackages, strGrossWeight, strChargeableWeight, IsDangerousGood, strRemarks, AssignedToEmpId,LoggedInUser.glUserId);

        if (EnqId > 0)
        {
            if (fuAttachment.HasFile) // Add Enquiry Document
            {
                string strDocFolder = "FreightDoc\\" + lblEnquiryRefNo.Text + "\\";

                strFilePath = UploadDocument(strDocFolder);
                string strDocumentName = txtAttachDocName.Text.Trim();

                if (strFilePath != "")
                {
                    if (strDocumentName == "")
                    {
                        strDocumentName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
                    }

                    int DocResult = DBOperations.AddFreightDocument(EnqId, strDocumentName, strFilePath, LoggedInUser.glUserId);
                }
            }

            // Add Emai/SMS Reminder Details
            if (txtRemindDate.Text.Trim() != "")
            {
                if (chkReminMode.SelectedIndex != -1)
                {
                    int RemindResult = -1;
                    DateTime dtRmDate = Commonfunctions.CDateTime(txtRemindDate.Text.Trim());

                    if (chkReminMode.Items[0].Selected) // Email
                        RemindResult = DBOperations.AddFreightReminder(EnqId, 1, LoggedInUser.glUserId, dtRmDate, txtRemindNotes.Text.Trim(), LoggedInUser.glUserId);

                    if (chkReminMode.Items[1].Selected) // SMS
                        RemindResult = DBOperations.AddFreightReminder(EnqId, 2, LoggedInUser.glUserId, dtRmDate, txtRemindNotes.Text.Trim(), LoggedInUser.glUserId);
                }
            }
            // Send Email To Enquiry Transfered User

            if (ddTransferEmp.SelectedValue != "0")
            {
                if (ddTransferEmp.SelectedValue != LoggedInUser.glUserId.ToString())
                {
                    TransferEmail(AssignedToEmpId);
                }
            }

            // Send Enquiry Email To Agent

            if (lbAgentContact.SelectedIndex != -1)
            {
                AgentNotificationEmail(EnqId, strFilePath);
            }
            
            // Redirect To Enquiry Success Page

            Response.Redirect("FreightSuccess.aspx?FEnquiry=343");

        }//END_IF
        else if (EnqId == -1)
        {
            lblError.Text = lblEnquiryRefNo.Text + " Enquiry Ref No " + lblEnquiryRefNo.Text + "Already Created. Please Submit Again!";
            lblError.CssClass = "errorMsg";
            
            lblEnquiryRefNo.Text = DBOperations.GetNewFreightEnquiryNo();
        }
        else if (EnqId == -2)
        {
            lblError.Text = "Customer Ref/Email No Already Exists. Please Change Ref Number!";
            lblError.CssClass = "errorMsg";

            lblEnquiryRefNo.Text = DBOperations.GetNewFreightEnquiryNo();
        }
        else if (EnqId == -123)
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }
        else 
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FreightTracking.aspx");
    }

    protected void ddFreightMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddFreightMode.SelectedValue == "1") // For AIR
        {
            pnlAir.Visible = true;
            pnlSea.Visible = false;
        }
        else // For Sea and Breakbulk
        {
            pnlAir.Visible = false;
            pnlSea.Visible = true;
        }
    }
     
    protected void TransferEmail(int AssignedUserId)
    {
        if (AssignedUserId == LoggedInUser.glUserId)
        {
            return;
        }
        
        string strEmpName="", strEmpEmail = "", strRefNo = "";
        string EmailContent = "", strReturnMessage = "";
        string EmailBody = "", MessageBody = "", strSubject = "";

        DataView dvEmpDetail = DBOperations.GetUserDetail(AssignedUserId.ToString());

        if (dvEmpDetail.Table.Rows.Count > 0)
        {
            strEmpName = dvEmpDetail.Table.Rows[0]["sName"].ToString();
            strEmpEmail = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();
            strRefNo = lblEnquiryRefNo.Text.Trim();

            try
            {
                string strFileName = "../EmailTemplate/Freight_EmailEnqAssigned.txt";

                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                strReturnMessage = ex.Message;
            }

            MessageBody = EmailContent.Replace("<AssignedToName>", strEmpName);

            MessageBody = MessageBody.Replace("<EnqRefNo>", strRefNo);
            
            MessageBody = MessageBody.Replace("<EmpName>", LoggedInUser.glEmpName);

            try
            {
                strSubject = "New Enquiry Generated # " + strRefNo + " / Customer Name: " + txtCustomer.Text.Trim();

                EmailBody = MessageBody;

                EMail.SendMail(LoggedInUser.glUserName, strEmpEmail, strSubject, EmailBody, "");

                strReturnMessage = "Enquiry Assigned Email Sent To:" + strEmpName;
            }
            catch (System.Exception ex)
            {
                strReturnMessage = ex.Message.ToString();
            }
        }

    }
     
    protected void txtCountry_TextChanged(object sender, EventArgs e)
    {
        if (hdnCountryId.Value != "")
        {
            Int32 CompanyCategoryID = (Int32)EnumCompanyType.Agent;

            int CountryID = Convert.ToInt32(hdnCountryId.Value);

            // Fill Agent Company by Country

            DBOperations.FillCompanyCategoryByCountryID(lbAgentCompany, CompanyCategoryID, CountryID);

            DBOperations.FillCompanyUserByCountryID(lbAgentContact, CompanyCategoryID, CountryID);

            if (ddFreightType.SelectedValue == "1")
            {
                if (lbAgentCompany.Items.Count == 0)
                {
                    lblError.Text = "Agent Information Not Found! Please Add Agent Details.";
                    lblError.CssClass = "errorMsg";
                    //btnSave.Visible = false;
                }
            }
        }
    }

    protected void lbAgentCompany_IndexChanged(object sender, EventArgs e)
    {
        string strAgentIDList = "";

        int CountryID = Convert.ToInt32(hdnCountryId.Value);

        foreach (System.Web.UI.WebControls.ListItem item in lbAgentCompany.Items)
        {
            if (item.Selected)
            {
                strAgentIDList += item.Value + ",";
            }
        }

        if (strAgentIDList != "")
        {
            FillAgentDetailByCompany(strAgentIDList, CountryID);
        }
    }
    private void FillAgentDetailByCompany(string CompanyIDList, int CountryID)
    {
        Int32 CompanyCategoryID = (Int32)EnumCompanyType.Agent;

        // Fill Agent Company by Country

        DBOperations.FillCompanyUserByListID(lbAgentContact, CompanyIDList, CompanyCategoryID, CountryID);
    }

    protected string AgentNotificationEmail(int EnqId, string AttachFilePath)
    {
        int AgentID = 0;
        string strAgentName     = "", strAgentEmail = "";
        string EmailContent     = "", strReturnMessage  = "";
        string EmailBody        = "", MessageBody       = "", strSubject = "";

        string strEnqRefNo       =  lblEnquiryRefNo.Text.Trim();
        string strFreightType    =  ddFreightType.SelectedItem.Text;
        string strFreightMode    =  ddFreightMode.SelectedItem.Text;
        string strCustRefNo      =  txtCustRefNo.Text.Trim();
        string strPortOfLoading  =  txtPortLoading.Text.Trim();
        string strPortOfDischarge = txtPortOfDischarged.Text.Trim();
        string strTerms          =  ddTerms.SelectedItem.Text;
        string strNoOfPackages   =  txtNoOfPkgs.Text.Trim();
        string strGrossWeight    =  txtGrossWeight.Text.Trim();
        string strRemark         =  txtRemark.Text.Trim();
       
        strReturnMessage = "Enquiry Email For Agent Sent To:";

        try
        {
            string strFileName = "../EmailTemplate/Freight_EmailAgent.txt";

            StreamReader sr = new StreamReader(Server.MapPath(strFileName));
            sr = File.OpenText(Server.MapPath(strFileName));
            EmailContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            GC.Collect();
        }
        catch (Exception ex)
        {
            strReturnMessage = ex.Message;
            return strReturnMessage;
        }

        // Email Template

        MessageBody = EmailContent.Replace("@EnqRefNo", strEnqRefNo);
                
        MessageBody = MessageBody.Replace("@FreightType", strFreightType);
        MessageBody = MessageBody.Replace("@FreightMode", strFreightMode);
        MessageBody = MessageBody.Replace("@CustRefNo", strCustRefNo);
        MessageBody = MessageBody.Replace("@PortOfLoading", strPortOfLoading);
        MessageBody = MessageBody.Replace("@PortOfDischarge", strPortOfDischarge);
        MessageBody = MessageBody.Replace("@Terms", strTerms);
        MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPackages);
        MessageBody = MessageBody.Replace("@GrossWeight", strGrossWeight);
        MessageBody = MessageBody.Replace("@Remark", strRemark);
        MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);
        
        foreach (ListItem item in lbAgentContact.Items)
        {
            if (item.Selected)
            {
                AgentID = Convert.ToInt32(item.Value);

                DataView dvEmpDetail = DBOperations.GetCustomerUserDetail(AgentID.ToString());

                if (dvEmpDetail.Table.Rows.Count > 0)
                {
                    strAgentName    = dvEmpDetail.Table.Rows[0]["sName"].ToString();
                    strAgentEmail   = dvEmpDetail.Table.Rows[0]["sEmail"].ToString();
                    
                    try
                    {
                        strSubject = "ENQ // " + strEnqRefNo + "// " + strPortOfLoading + "// TO // " + strPortOfDischarge + "//" +
                        strFreightMode + "// " + strCustRefNo;

                        MessageBody = MessageBody.Replace("@AgentName", strAgentName);

                        EmailBody = MessageBody;
                        //strAgentEmail = "amit.bakshi@babajishivram.com";

                        // Send Email To Agent and Copy To Freight SPC -- "amit.bakshi@babajishivram.com"
                        bool bMailSuccess = EMail.SendMail2(LoggedInUser.glUserName, LoggedInUser.glUserName, strSubject, EmailBody, AttachFilePath);

                        int result = DBOperations.AddFreightAgent(EnqId,AgentID,bMailSuccess,LoggedInUser.glUserId);

                        if (result == 0)
                        {
                            strReturnMessage += "," + strAgentName;
                        }
                        else
                        {
                            strReturnMessage += ", Enquiry Sending Failed For - " + strAgentName;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        strReturnMessage += ex.Message.ToString();
                        //return strReturnMessage;
                    }
                }

            }// END_IF_Selected
        }//END_ForEach

        return strReturnMessage;
    }

    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "FreightDoc\\";
        }

        string FileName = fuAttachment.FileName;
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

        if (fuAttachment.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuAttachment.SaveAs(ServerFilePath + FileName);
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

    #region Freight Rate Popup
    protected void lnkViewRate_Click(object sender, EventArgs e)
    {
        int TransMode = Convert.ToInt32(ddFreightMode.SelectedValue);

        if (TransMode == 3)
        {
            TransMode = 2;
        }

        string Country = txtCountry.Text.Trim();
        string POL = txtPortLoading.Text.Trim();
        string POD = txtPortOfDischarged.Text.Trim();

        DataSet dsRateDetail = DBOperations.GetFreightRateByPort(TransMode, POL, POD,Country, DateTime.Now);

        gvRateSummary.DataSource = dsRateDetail;
        gvRateSummary.DataBind();

        ModalPopupRateDetail.Show();

    }

    protected void gvRateSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRateSummary.PageIndex = e.NewPageIndex;

        ModalPopupRateDetail.Show();
    }

    protected void gvRateSummary_Sorting(object sender, GridViewSortEventArgs e)
    {
        ModalPopupRateDetail.Show();
    }

    protected void btnCancelRatePopup_Click(object sender, EventArgs e)
    {
        ModalPopupRateDetail.Hide();
    }

    #endregion
}