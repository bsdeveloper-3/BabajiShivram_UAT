using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
public partial class FreightOperation_AgentPreAlert : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnUpload);

        calMBLDate.EndDate = DateTime.Now;
        calHBLDate.EndDate = DateTime.Now;

        MskValMBLDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskValHBLDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingPreAlert.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Agent PreAlert Detail";

            Accordion1.SelectedIndex = -1;

            SetAlertDetail(Convert.ToInt32(Session["EnqId"]));

            Page.Validate();
        }
    }

    private void SetAlertDetail(int EnqId)
    {
        DataSet dsAgentPreDetail = DBOperations.GetAgenPreAlertDetail(EnqId);

        if (dsAgentPreDetail.Tables[0].Rows.Count > 0)
        {
            lblJobNo.Text = dsAgentPreDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            hdnUploadPath.Value = dsAgentPreDetail.Tables[0].Rows[0]["DocDir"].ToString();

            lblBookingMonth.Text = Convert.ToDateTime(dsAgentPreDetail.Tables[0].Rows[0]["BookingDate"]).ToString("MMMM");

            txtMBLNo.Text       =   dsAgentPreDetail.Tables[0].Rows[0]["MBLNo"].ToString();
            txtHBLNo.Text       =   dsAgentPreDetail.Tables[0].Rows[0]["HBLNo"].ToString();
            txtHBLDate.Text     =   dsAgentPreDetail.Tables[0].Rows[0]["HBLDate"].ToString();
            txtVesselName.Text  =   dsAgentPreDetail.Tables[0].Rows[0]["VesselName"].ToString();
            txtVesselNumber.Text =  dsAgentPreDetail.Tables[0].Rows[0]["VesselName"].ToString();
            txtETADate.Text     =   dsAgentPreDetail.Tables[0].Rows[0]["ETA"].ToString();
            txtETDDate.Text     =   dsAgentPreDetail.Tables[0].Rows[0]["ETD"].ToString();
            lblContainer20.Text = dsAgentPreDetail.Tables[0].Rows[0]["CountOf20"].ToString();
            lblContainer40.Text = dsAgentPreDetail.Tables[0].Rows[0]["CountOf40"].ToString();
            lblLCLFCL.Text = dsAgentPreDetail.Tables[0].Rows[0]["ContainerTypeName"].ToString();

            if (dsAgentPreDetail.Tables[0].Rows[0]["FinalAgentID"] != DBNull.Value)
            {
                ddFinalAgent.SelectedValue = dsAgentPreDetail.Tables[0].Rows[0]["FinalAgentID"].ToString();
            }

            //txtFinalAgent.Text  =   dsAgentPreDetail.Tables[0].Rows[0]["FinalAgent"].ToString();

            if (dsAgentPreDetail.Tables[0].Rows[0]["MBLDate"] != DBNull.Value)
                txtMBLDate.Text = Convert.ToDateTime(dsAgentPreDetail.Tables[0].Rows[0]["MBLDate"]).ToString("dd/MM/yyyy");
            if (dsAgentPreDetail.Tables[0].Rows[0]["HBLDate"] != DBNull.Value)
                txtHBLDate.Text = Convert.ToDateTime(dsAgentPreDetail.Tables[0].Rows[0]["HBLDate"]).ToString("dd/MM/yyyy");
            if (dsAgentPreDetail.Tables[0].Rows[0]["ETA"] != DBNull.Value)
                txtETADate.Text = Convert.ToDateTime(dsAgentPreDetail.Tables[0].Rows[0]["ETA"]).ToString("dd/MM/yyyy");
            if (dsAgentPreDetail.Tables[0].Rows[0]["ETD"] != DBNull.Value)
                txtETDDate.Text = Convert.ToDateTime(dsAgentPreDetail.Tables[0].Rows[0]["ETD"]).ToString("dd/MM/yyyy");

            hdnModeId.Value = dsAgentPreDetail.Tables[0].Rows[0]["lMode"].ToString();

            if (hdnModeId.Value == "1") //AIR -  Container Detail No Required
            {
                Accordion1.Panes[1].Visible = false;
                //RFVFinalAgent.Enabled = false; // Final Agent Not Required for AIR Shipment
            }

            if (dsAgentPreDetail.Tables[0].Rows[0]["CountOf20"].ToString() != "0")
            {
                lblCont20.Text = dsAgentPreDetail.Tables[0].Rows[0]["TotCount20"].ToString() + "/" + dsAgentPreDetail.Tables[0].Rows[0]["CountOf20"].ToString();
            }
            else
            {
                lblCont20.Text = "0/" + dsAgentPreDetail.Tables[0].Rows[0]["CountOf20"].ToString();
            }

            if (dsAgentPreDetail.Tables[0].Rows[0]["CountOf40"].ToString() != "0")
            {
                lblCont40.Text = dsAgentPreDetail.Tables[0].Rows[0]["TotCount40"].ToString() + "/" + dsAgentPreDetail.Tables[0].Rows[0]["CountOf40"].ToString();
            }
            else
            {
                lblCont40.Text = "0/" + dsAgentPreDetail.Tables[0].Rows[0]["CountOf40"].ToString();
            }

            ddContainerType.SelectedValue = dsAgentPreDetail.Tables[0].Rows[0]["ContainerType"].ToString();
            ddContainerType.Enabled = false;
        }

    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {        
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        bool IsCompleted = false;

        string strMBLNo = "", strHBLNo = "", strVesselName = "", strVesselNumber = "", strFinalAgent = "";
        int FinalAgentID = 0;

        DateTime dtMBLDate = DateTime.MinValue, dtHBLDate = DateTime.MinValue,
        dtETDDate = DateTime.MinValue, dtETADate = DateTime.MinValue;

        strMBLNo        = txtMBLNo.Text.ToUpper().Trim();
        strHBLNo        = txtHBLNo.Text.ToUpper().Trim();
        strVesselName   = txtVesselName.Text.Trim();
        strVesselNumber = txtVesselNumber.Text.Trim();

        if (strHBLNo == "")
        {
            lblError.Text = "Please Enter HBL Number!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (ddFinalAgent.SelectedIndex > 0)
        {
            strFinalAgent = ddFinalAgent.SelectedItem.Text;
            FinalAgentID = Convert.ToInt32(ddFinalAgent.SelectedValue);
        }
        
        if (txtMBLDate.Text.Trim() != "")
        {
            dtMBLDate = Commonfunctions.CDateTime(txtMBLDate.Text.Trim());
        }
       
        if (txtHBLDate.Text.Trim() != "")
        {
            dtHBLDate = Commonfunctions.CDateTime(txtHBLDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter HBL Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtETDDate.Text.Trim() != "")
        {
            dtETDDate = Commonfunctions.CDateTime(txtETDDate.Text.Trim());
        }
        if (txtETADate.Text.Trim() != "")
        {
            dtETADate = Commonfunctions.CDateTime(txtETADate.Text.Trim());
        }

        if (dtMBLDate != DateTime.MinValue && dtHBLDate != DateTime.MinValue && dtETDDate != DateTime.MinValue && dtETADate != DateTime.MinValue)
        {
            if (strVesselName != "")
            {
                IsCompleted = true;
            }
        }

        string args = lblCont20.Text;
        string[] Count20 = args.Split('/');

        string args1 = lblCont40.Text;
        string[] Count40 = args1.Split('/');

        if ((Count20[0].ToString() == Count20[1].ToString()) && (Count40[0].ToString() == Count40[1].ToString()))
        {
            int result = DBOperations.AddAgentPreAlert(EnqId, strMBLNo, strHBLNo, strVesselName, strVesselNumber,
               dtMBLDate, dtHBLDate, dtETDDate, dtETADate, strFinalAgent, FinalAgentID, IsCompleted, LoggedInUser.glUserId);

            if (result == 0)
            {
                string strReturnMessage = "Agent PreAlert Detail Updated Successfully!";

                // Set Shipment Arrival Reminder For Operation User and Enquiry User

                AddArrivalReminder(EnqId);

                // For Sea Only - Set IGM Filing Reminder For Operation User and Enquiry User
                if (Convert.ToInt32(hdnModeId.Value) > 1)
                {
                    AddIGMReminder(EnqId);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='AwaitingPreAlert.aspx';", true);
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Agent PreAlert Detail Already Exists!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "HBL No Already Exists! HBL No can not be duplicated!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error!";
                lblError.CssClass = "errorMsg";
            }
            /*******************************************************/
        }
        else
        {
            string strReturnMessage = "Please complete the container details";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingPreAlert.aspx");
    }

    #region Freight Document

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

    protected void btnUpload_Click(Object sender, EventArgs e)
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

        if (ServerPath == "")
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

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext  = Path.GetExtension(FileName);
                FileName    = Path.GetFileNameWithoutExtension(FileName);

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
    
    #endregion

    #region IGM Alert Reminder
    private void AddIGMReminder(int EnqId)
    {
        int RemindResult = -123;
        int lMode = 0, EnquiryUserId = 0, OperationUserId = 0;
        string EnqRefNo = "", JobRefNo ="", Customer = ""; 
        string EmailReminderNote = "", SmsReminderNote = "";

        DataSet dsDetail = DBOperations.GetOperationDetail(EnqId);

        DateTime dtETADate = DateTime.MinValue; DateTime dtReminderDate = DateTime.MinValue;
        OperationUserId =   LoggedInUser.glUserId;

        lMode = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lMode"]);

        if(dsDetail.Tables[0].Rows[0]["ETA"]!= DBNull.Value && lMode > 1) // For IGM Reminder Sea Mode Only
        {
            EnqRefNo = dsDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            JobRefNo = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            Customer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();

            dtETADate       =   Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["ETA"]);
            
            EnquiryUserId   =   Convert.ToInt32(dsDetail.Tables[0].Rows[0]["SPCId"]);
            
            // For SEA - IGM Reminder 7 Days Prior To ETA and For AIR 1 Day
                        
            dtReminderDate = dtETADate.AddDays(-7);
            
            // SET EMAIL Reminder

            EmailReminderNote = "IGM Filing - ETA: " + dtETADate.ToString("dd/MM/yyyy");
                
            RemindResult = DBOperations.AddFreightReminder(EnqId, 1, OperationUserId, dtReminderDate, EmailReminderNote, LoggedInUser.glUserId);
            RemindResult = DBOperations.AddFreightReminder(EnqId, 1, EnquiryUserId, dtReminderDate, EmailReminderNote, LoggedInUser.glUserId);
            
            // SMS
            //SmsReminderNote = JobRefNo + ": "+Customer +", IGM Filing Reminder, ETA: " + dtETADate.ToString("dd/MM/yyyy") + ","+ LoggedInUser.glEmpName;
            
            //RemindResult = DBOperations.AddFreightReminder(EnqId, 2, OperationUserId, dtReminderDate, SmsReminderNote, LoggedInUser.glUserId);
            //RemindResult = DBOperations.AddFreightReminder(EnqId, 2, EnquiryUserId, dtReminderDate, SmsReminderNote, LoggedInUser.glUserId);

            if (RemindResult == 0)
            {
                // SUCCESS
            }
        }
    }

    private void AddArrivalReminder(int EnqId)
    {
        int RemindResult = -123;
        int lMode = 0, EnquiryUserId = 0, OperationUserId = 0;
        string EnqRefNo = "", JobRefNo = "", Customer = "";
        string EmailReminderNote = "", SmsReminderNote = "";

        DataSet dsDetail = DBOperations.GetOperationDetail(EnqId);

        DateTime dtETADate = DateTime.MinValue; DateTime dtReminderDate = DateTime.MinValue;
        OperationUserId = LoggedInUser.glUserId;

        if (dsDetail.Tables[0].Rows[0]["ETA"] != DBNull.Value)
        {
            EnqRefNo = dsDetail.Tables[0].Rows[0]["ENQRefNo"].ToString();
            JobRefNo = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            Customer = dsDetail.Tables[0].Rows[0]["Customer"].ToString();

            dtETADate = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["ETA"]);
            lMode = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["lMode"]);
            EnquiryUserId = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["SPCId"]);

            //  Shipment Arrival Reminder 1 Days Prior To ETA

            dtReminderDate = dtETADate.AddDays(-1);
            
            // SET EMAIL Reminder

            EmailReminderNote = "Shipment Arrival - ETA: " + dtETADate.ToString("dd/MM/yyyy");
                
            RemindResult = DBOperations.AddFreightReminder(EnqId, 1, OperationUserId, dtReminderDate, EmailReminderNote, LoggedInUser.glUserId);
            RemindResult = DBOperations.AddFreightReminder(EnqId, 1, EnquiryUserId, dtReminderDate, EmailReminderNote, LoggedInUser.glUserId);

            // SMS
            //SmsReminderNote = JobRefNo + ": " + Customer + ", Shipment Arrival Reminder, ETA: " + dtETADate.ToString("dd/MM/yyyy") + "," + LoggedInUser.glEmpName;

            //RemindResult = DBOperations.AddFreightReminder(EnqId, 2, OperationUserId, dtReminderDate, SmsReminderNote, LoggedInUser.glUserId);
            //RemindResult = DBOperations.AddFreightReminder(EnqId, 2, EnquiryUserId, dtReminderDate, SmsReminderNote, LoggedInUser.glUserId);

            if (RemindResult == 0)
            {
                // SUCCESS
            }
        }
    }
    #endregion

    #region Container Detail

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int ContainerType; //1-FCL, 2 - LCL 
        int ContainerSize; //1 -Twenty, 2 - Forty

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

        if (EnqId > 0)
        {
            if (ContainerType == (Int16)EnumContainerType.FCL)
            {
                if (ContainerSize == 0)
                {
                    lblError.Text = "Please Select FCL Container Size!";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }
            else if (ContainerType == (Int16)EnumContainerType.LCL) //LCL
            {
                ddContainerSize.SelectedValue = "0";
                ContainerSize = 0;
            }

            if (ContainerNo != "")
            {
                int result = DBOperations.AddFreightContainerMS(EnqId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Container No. " + ContainerNo + " Added Successfully.";
                    lblError.CssClass = "success";
                    gvContainer.DataBind();
                    SetAlertDetail(EnqId);
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblError.Text = "Container No. " + ContainerNo + " Already Added!";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = " Please Enter Container No.!";
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Freight Details Not Found.!";
        }

    }

    #endregion

    protected void txtHBLNo_TextChanged(object sender, EventArgs e)
    {
        string returnMsg = DBOperations.GetHAWBDetail(Convert.ToInt32(Session["EnqId"]), txtHBLNo.Text);
        if (returnMsg != "0")
        {
            Disable_Field();
            ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('HBL Number is already exist against the Job No " + returnMsg + "');", true);
            btnUpdate.Visible = false;
        }
        else
        {
            btnUpdate.Visible = true;
            Enable_Field();
        }
    }

    protected void txtMBLNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = DBOperations.GetAgentDetail(1, txtMBLNo.Text);
        foreach (DataRow row in dt.Rows)
        {
            string OldJobNo = row["FRJobNo"].ToString();
            string OldEnqNo = row["ENQRefNo"].ToString();

            if (lblError.Text == "")
            {
                lblError.Text = "MBL Number is already exist against the Job No " + OldJobNo + "/Enq No " + OldEnqNo;
                lblError.CssClass = "errorMsg";
            }

        }
    }

    protected void txtMBLDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtMBLDate = DateTime.MinValue;
        DateTime dtHBLDate = DateTime.MinValue;
        if (txtMBLDate.Text != "")
        {
            dtMBLDate = Commonfunctions.CDateTime(txtMBLDate.Text.Trim());
            if (txtHBLDate.Text != "")
            {
                dtHBLDate = Commonfunctions.CDateTime(txtHBLDate.Text.Trim());
                if ((dtHBLDate.Date) < (dtMBLDate.Date))
                {
                    string strReturnMessage = "MBL date should be less than HBL Date";
                    //lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "');", true);
                    txtMBLDate.Text = "";
                }
            }
        }
    }

    protected void txtHBLDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtMBLDate = DateTime.MinValue;
        DateTime dtHBLDate = DateTime.MinValue;

        if (txtHBLDate.Text != "")
        {
            dtHBLDate = Commonfunctions.CDateTime(txtHBLDate.Text.Trim());
            if (txtMBLDate.Text != "")
            {
                dtMBLDate = Commonfunctions.CDateTime(txtMBLDate.Text.Trim());
                if (dtHBLDate.Date < dtMBLDate.Date)
                {
                    string strReturnMsg = "HBL Date should be greater than MBL Date";
                    //lblError.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMsg + "');", true);
                    txtHBLDate.Text = "";
                }
            }
        }
    }

    protected void Disable_Field()
    {
        txtMBLDate.Enabled = false;
        txtMBLNo.Enabled = false;
        txtHBLDate.Enabled = false;
        txtETADate.Enabled = false;
        txtETDDate.Enabled = false;
        txtVesselName.Enabled = false;
        txtVesselNumber.Enabled = false;
        ddFinalAgent.Enabled = false;
    }

    protected void Enable_Field()
    {
        txtMBLDate.Enabled = true;
        txtMBLNo.Enabled = true;
        txtHBLDate.Enabled = true;
        txtETADate.Enabled = true;
        txtETDDate.Enabled = true;
        txtVesselName.Enabled = true;
        txtVesselNumber.Enabled = true;
        ddFinalAgent.Enabled = true;
    }
}