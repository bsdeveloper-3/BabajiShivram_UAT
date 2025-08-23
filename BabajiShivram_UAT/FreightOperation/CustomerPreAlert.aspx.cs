using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
public partial class FreightOperation_CustomerPreAlert : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingPreAlert.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer PreAlert Detail";

	    MskValShipDate.MaximumValue = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
            MskValAlertDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

            SetBookingDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    private void SetBookingDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            lblJobNo.Text = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();

            lblBookingMonth.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");
            
            txtMailCC.Text = dsBookingDetail.Tables[0].Rows[0]["EnquiryEmail"].ToString()+","+LoggedInUser.glUserName + ",manish.radhakrishnan@babajishivram.com";

            txtCustEmail.Text = dsBookingDetail.Tables[0].Rows[0]["CustomerEmail"].ToString();
	
	    if (dsBookingDetail.Tables[0].Rows[0]["BranchID"] != DBNull.Value)
            {
                int BranchID = Convert.ToInt16(dsBookingDetail.Tables[0].Rows[0]["BranchID"]);

                if (BranchID == (Int32)EnumBranch.Delhi)
                {
                    hdnBranchEmail.Value = ",amit.thakur@babajishivram.com, sravinder@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Chennai)
                {
                    hdnBranchEmail.Value = ",vijayalakshmi@babajishivram.com, thaniga@babajishivram.com";
                }
                else if (BranchID == (Int32)EnumBranch.Ahmedabad)
                {
                    hdnBranchEmail.Value = ",mithun.sharma@babajishivram.com";
                }

                txtMailCC.Text = txtMailCC.Text + hdnBranchEmail.Value;
            }
        }
    }

    protected void lnkPreAlertEmailDraft_Click(object sender, EventArgs e)
    {
        txtCustEmail.Text = txtCustEmail.Text.Replace(";", ",").Trim();

        txtCustEmail.Text = txtCustEmail.Text.Replace(" ", "");
        txtCustEmail.Text = txtCustEmail.Text.Replace(",,", "");
	
	    txtCustEmail.Text = txtCustEmail.Text.TrimEnd(',');

       // int intCommaIndex = txtCustEmail.Text.LastIndexOf(",");

       // if (intCommaIndex > 0)
       //     txtCustEmail.Text =   txtCustEmail.Text.Remove(intCommaIndex);

        REVEmail.Validate();

        if (REVEmail.IsValid)
            GenerateEmailDraft();
        else
        {
            lblError.Text = "Invalid Email Address, Please Enter Comma-Seperated Email";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);
                
        if (lblCustomerEmail.Text.Trim() == "")
        {
            lblError.Text       = "Please Enter Customer Email & Subject!";
            lblError.CssClass   = "errorMsg";
            ModalPopupEmail.Hide();
        }
        else
        {
            // Send Email
            bool bEMailSucess = SendPreAlertEmail();
             
            // Update PreAlert Email Sent Status and Customer Email 

            if (bEMailSucess == true)
            {
                int Result = DBOperations.UpdateCustomerPreAlertEmailStatus(EnqId, lblCustomerEmail.Text.Trim(), LoggedInUser.glUserId);

                ModalPopupEmail.Hide();

                if (Result == 0)
                {
                    lblError.Text = "Customer PreAlert Email Sent Successfully!";
                    lblError.CssClass = "success";
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after sometime!";
                    lblError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblError.Text = "PreAlert Email Already Sent!";
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        /********************************************************************************/

        int EnqId = Convert.ToInt32(Session["EnqId"]);
        string strCustomerEmail = txtCustEmail.Text.Trim();

        strCustomerEmail = strCustomerEmail.Replace(" ", "");
        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
	    strCustomerEmail = strCustomerEmail.TrimEnd(',');

        DateTime ShippedOnBoardDate = DateTime.MinValue, PreAlertToCustDate = DateTime.MinValue;
                
        if (txtShippedDate.Text.Trim() != "")
        {
            ShippedOnBoardDate = Commonfunctions.CDateTime(txtShippedDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Shipped On Board Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtPreAlertDate.Text.Trim() != "")
        {
            PreAlertToCustDate = Commonfunctions.CDateTime(txtPreAlertDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter PreAlert Date!";
            lblError.CssClass = "errorMsg";

            return;
        }

        int result = DBOperations.AddCustomerPreAlert(EnqId, ShippedOnBoardDate, PreAlertToCustDate,strCustomerEmail, LoggedInUser.glUserId);

        if (result == 0)
        {
            string strReturnMessage = "Customer PreAlert Detail Updated Successfully!";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='AwaitingCustomerPreAlert.aspx';", true);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Customer PreAlert Detail Already Exists!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error!";
            lblError.CssClass = "errorMsg";
        }
        /*******************************************************/
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingCustomerPreAlert.aspx");
    }

    private void GenerateEmailDraft()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string strCustomerEmail = txtCustEmail.Text.Trim();
        
        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", "");
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        lblCustomerEmail.Text = strCustomerEmail;
                
        string EmailContent =   "";
        string MessageBody  =   "";

        txtSubject.Text     =   "Customer PreAlert" +"-"+ lblJobNo.Text.Trim();

        string strEnqRefNo="", strHAWNo = "", strMAWNo = "", strShipper = "", strConsignee = "", strPortLoading = "",
            strPortDischarge = "", strVesselName = "", strETADate = "", strNoOfPkgs = "",
            strGrossWT = "", strInvoiceNo = "", strPONo = "", strDescription = "", strFinalAgent = "";

        string strGSTN = "", strPlaceOfSupply = "";

        DataSet dsAlertDetail = DBOperations.GetCustomerPreAlertDetail(EnqId);

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strHAWNo        =   dsAlertDetail.Tables[0].Rows[0]["HBLNo"].ToString();
            strMAWNo        =   dsAlertDetail.Tables[0].Rows[0]["MBLNo"].ToString();
            strShipper      =   dsAlertDetail.Tables[0].Rows[0]["Shipper"].ToString();
            strConsignee    =   dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strPortLoading  =   dsAlertDetail.Tables[0].Rows[0]["LoadingPortName"].ToString();
            strPortDischarge =  dsAlertDetail.Tables[0].Rows[0]["PortOfDischargedName"].ToString();
            strVesselName   =   dsAlertDetail.Tables[0].Rows[0]["VesselName"].ToString();
            strVesselName   +=  " - " + dsAlertDetail.Tables[0].Rows[0]["VesselNumber"].ToString();

            strNoOfPkgs     =   dsAlertDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            strGrossWT      =   dsAlertDetail.Tables[0].Rows[0]["GrossWeight"].ToString();
            strInvoiceNo    =   dsAlertDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
            strPONo         =   dsAlertDetail.Tables[0].Rows[0]["PONumber"].ToString();
            strDescription  =   dsAlertDetail.Tables[0].Rows[0]["CargoDescription"].ToString();
            strEnqRefNo     =   dsAlertDetail.Tables[0].Rows[0]["EnqRefNo"].ToString();
            strFinalAgent   =   dsAlertDetail.Tables[0].Rows[0]["FinalAgent"].ToString();

            strGSTN = dsAlertDetail.Tables[0].Rows[0]["ConsigneeGSTN"].ToString();
            strPlaceOfSupply = dsAlertDetail.Tables[0].Rows[0]["ConsigneeState"].ToString();

            if (dsAlertDetail.Tables[0].Rows[0]["ETA"] != DBNull.Value)
            {
                strETADate = Convert.ToDateTime(dsAlertDetail.Tables[0].Rows[0]["ETA"]).ToString("dd/MM/yyyy");
            }
        }
        else
        {
            lblError.Text = "Booking Details Not Found! Please check details.";
            lblError.CssClass = "errorMsg";
            return;
        }
        try
        {
            string strFileName = "../EmailTemplate/FOP_EmailCustPreAlert.txt";

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

        MessageBody = EmailContent.Replace("@HAWNumber", strHAWNo);
        MessageBody = MessageBody.Replace("@Shipper", strShipper);
        MessageBody = MessageBody.Replace("@CONSIGNEE", strConsignee);
        MessageBody = MessageBody.Replace("@MAWNumber", strMAWNo);
        MessageBody = MessageBody.Replace("@PortOfLoading", strPortLoading);
        MessageBody = MessageBody.Replace("@PortOfDischarge", strPortDischarge);
        MessageBody = MessageBody.Replace("@VesselName", strVesselName);
        MessageBody = MessageBody.Replace("@ETADate", strETADate);
        MessageBody = MessageBody.Replace("@NoOfPackages", strNoOfPkgs);
        MessageBody = MessageBody.Replace("@GrossWeight", strGrossWT);
        MessageBody = MessageBody.Replace("@InvoiceNo", strInvoiceNo);
        MessageBody = MessageBody.Replace("@PONumber", strPONo); 
        MessageBody = MessageBody.Replace("@Description", strDescription);
        MessageBody = MessageBody.Replace("@EnqRefNo", strEnqRefNo);
        MessageBody = MessageBody.Replace("@FinalAgent", strFinalAgent); 
        MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);

        MessageBody = MessageBody.Replace("@GSTN", strGSTN);
        MessageBody = MessageBody.Replace("@StateName", strPlaceOfSupply);

        divPreviewEmail.InnerHtml = MessageBody;

        ModalPopupEmail.Show();
    }

    private bool SendPreAlertEmail()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";
        
        strCustomerEmail    =   lblCustomerEmail.Text.Trim();
        strCCEmail          =   txtMailCC.Text.Trim();
        strSubject          =   txtSubject.Text.Trim();

        strCCEmail  =   strCCEmail.Replace(";", ",").Trim();
        strCCEmail  =   strCCEmail.Replace(" ", "");
        strCCEmail  =   strCCEmail.Replace(",,", ",").Trim();
        strCCEmail  =   strCCEmail.Replace("\r", "").Trim();
        strCCEmail  =   strCCEmail.Replace("\n", "").Trim();
        strCCEmail  =   strCCEmail.Replace("\t", "").Trim();

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

    #region Model Popup

    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }

    #endregion
}