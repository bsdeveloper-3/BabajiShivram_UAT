using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using System.Text;
public partial class Transport_TransporterTab : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvKYCCopys);
        ScriptManager1.RegisterPostBackControl(btnNotes);
        ScriptManager1.RegisterPostBackControl(gvNotes);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Add New Transporter";
            
            if (Session["TR_TransporterID"] == null)
            {
                Response.Redirect("TransporterList.aspx");
            }
            else
            {
                string ctrCustId = Session["TR_TransporterID"].ToString();
                DataSet dsCustInst = new DataSet();
                dsCustInst = BillingOperation.GetCustomerInstruction(ctrCustId);

                DBOperations.FillNotificationType(ddNotificationType, false, true);
                DBOperations.FillNotificationMode(ddNotificationMode);
                DBOperations.FillStatusActivity(ddlStatus);

            }
        }
    }
    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataRowView drView = (DataRowView)FormView1.DataItem;
        if (drView != null)
        {
            lblTitle.Text = drView["CustName"].ToString();
        }

        // Validate Form

        Page.Validate("Required");
    }
    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        Response.Redirect("TransporterList.aspx");
    }

    #region Company FormView Event

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInInsertMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null || e.AffectedRows == -1)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;

            if (e.Exception != null)
            {
                lblError.Text = e.Exception.Message;
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result > 0)
        {
            lblError.Text = "Transporter Detail Updated Successfully";
            lblError.CssClass = "success";
        }
        else if (Result == 0)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lblError.Text = "Transporter Name Already Exists.";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == -2)
        {
            lblError.Text = "Transporter Code Already Exists.";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

    #region GridView Customer User

    protected void btnNewUser_Click(object sender, EventArgs e)
    {
        SqlDataSourceUser.SelectParameters[0].DefaultValue = "0";
        FormViewUser.DataBind();
        FormViewUser.ChangeMode(FormViewMode.Insert);

        DataSourceUserPlant.SelectParameters[0].DefaultValue = ""; ;
        DataSourceNotification.DataBind();

        DataSourceUserPlant.SelectParameters[0].DefaultValue = ""; ;
        DataSourceUserPlant.DataBind();

        DataSourceNotification.SelectParameters[0].DefaultValue = ""; ;
        DataSourceNotification.DataBind();

        //  if (Session["CustId"] != null)
        //    Response.Redirect("CustomerUserTab.aspx");
        //  else
        //  {
        //    lblError.Text = "Please Add Customer!";
        //    lblError.CssClass = "errorMsg";
        //  }
    }

    protected void gvCustomerUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow rowUser = gvCustomerUser.SelectedRow;

        // Session["CustomerId"]   = gvCustomerUser.DataKeys[rowUser.RowIndex].Values["CustomerId"].ToString();
        // Session["UserIdCustomer"] = gvCustomerUser.DataKeys[rowUser.RowIndex].Values["lid"].ToString();

        //Response.Redirect("CustomerUserTab.aspx");

        string CustomerUserId = gvCustomerUser.DataKeys[rowUser.RowIndex].Values["lid"].ToString();

        SqlDataSourceUser.SelectParameters[0].DefaultValue = CustomerUserId;
        FormViewUser.DataBind();
        FormViewUser.ChangeMode(FormViewMode.Edit);


        DataSourceSelectArguments args = new DataSourceSelectArguments();
        DataView view = (DataView)SqlDataSourceUser.Select(args);
        DataTable dt = view.ToTable();
        hdnUserCountryId.Value = dt.Rows[0]["CountryID"].ToString();

    }

    #endregion

    #region Notes/Agent Contract
    protected void btnNotesAdd_Click(object sender, EventArgs e)
    {
        if (Session["TR_TransporterID"] != null)
        {

            string cNotes = txtNotes.Text.Trim();
            string strFilePath = "";
            string strFileFolder = hdnCustFilePath.Value.Trim();

            int CustomerID = 0;
            int NoteType = Convert.ToInt16(rdlNotType.SelectedValue); // 1 for document, 2 for Transorter Contract

            DateTime StartDate = DateTime.MinValue;
            DateTime ValidTillDate = DateTime.MinValue;

            //if(hdnCustId.Value.Trim() != "")
            //{
            //    CustomerID = Convert.ToInt32(hdnCustId.Value.Trim());
            //}

            if (txtStartDate.Text.Trim() != "")
            {
                StartDate = Commonfunctions.CDateTime(txtStartDate.Text.Trim());
            }

            if (rdlNotType.SelectedValue == "1")
            {
                if (txtStartDate.Text.Trim() == "")
                    StartDate = DateTime.Now.Date;
            }

            if (txtValidDate.Text.Trim() != "")
            {
                ValidTillDate = Commonfunctions.CDateTime(txtValidDate.Text.Trim());
            }

            if (fuNotesDoc.HasFile)
            {
                // Upload Customer Notes File To Customer Folder

                strFileFolder = strFileFolder + "\\NoteFiles\\";

                strFilePath = UploadNotesFiles(fuNotesDoc, strFileFolder);
            }

            if(NoteType == 2 && CustomerID == 0)
            {
                lblError.Text = "Please select Customer Name to Add Contract Copy!";
                lblError.CssClass = "errorMsg";
                return;
            }

            int outVal = DBOperations.AddCustomerNotes(Convert.ToInt32(Session["TR_TransporterID"]), cNotes, strFilePath, NoteType, StartDate, ValidTillDate, LoggedInUser.glUserId);

            //int outVal = DBOperations.AddTransporterNotes(Convert.ToInt32(Session["TR_TransporterID"]), CustomerID, cNotes, strFilePath, NoteType, StartDate, ValidTillDate, LoggedInUser.glUserId);

            if (outVal == 0)
            {
                if(NoteType == 1)
                    lblError.Text = "Notes Added Successfully !";
                else if(NoteType == 2)
                    lblError.Text = "Contract Added Successfully !";

                lblError.CssClass = "success";
                gvNotes.DataSourceID = "DataSourceNotes";
                gvNotes.DataBind();
                txtNotes.Text = string.Empty;
                txtStartDate.Text = string.Empty;
                txtValidDate.Text = string.Empty;
                //txtCustomer.Text = string.Empty;
                //hdnCustId.Value = "0";
            }
            else if (outVal == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (outVal == 2)
            {
                lblError.Text = "Notes Detail Already Exist !";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lblError.Text = "Please First Add Customer!";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void gvNotes_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();

            if (DocPath == "")
            {
                lblError.Text = "Document Not Uploaded!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                DownloadDocument(DocPath);
            }
        }
    }

    protected void DataSourceNotes_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblError.Text = "Note Removed Successfully !";
            lblError.CssClass = "success";

        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Note Details Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    public string UploadNotesFiles(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "NoteFiles\\";

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
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;

            }

            fuDocument.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
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

    protected void btnNewReport_Click(object sender, EventArgs e)
    {
        if (Session["CustId"] != null)
        {
            Session["ReportCustomerId"] = Session["CustId"];
            Response.Redirect("Reports/AdHocReport.aspx");
        }
        else
        {
            lblError.Text = "Please Add Customer!";
            lblError.CssClass = "errorMsg";
        }
    }

    //protected void txtCustomer_TextChanged(object sender, EventArgs e)
    //{
    //    if (hdnCustId.Value != "")
    //    {
    //        int CustomerId = Convert.ToInt32(hdnCustId.Value);
         
    //    }
    //    else
    //    {
    //        lblError.Text = "Customer Not Found.Please Select Customer Again";
    //        lblError.CssClass = "errorMsg";
    //    }
    //}
    #region Customer User

    #region FormView User Event

    protected void FormViewUser_DataBound(object sender, EventArgs e)
    {
        if (FormViewUser.CurrentMode == FormViewMode.Edit)
        {
            DataRowView drv = (DataRowView)FormViewUser.DataItem;

            if (drv["ResetCode"] != DBNull.Value)
            {
                ((RadioButtonList)FormViewUser.FindControl("rdPasswordReset")).SelectedValue = drv["ResetCode"].ToString();
            }
        }

        Page.Validate("Required");
    }

    protected void FormViewUser_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel" || e.CommandName == "New" || e.CommandName == "Edit")
        {
            lblError.Visible = false;
            lblError.Text = "";
        }
    }

    protected void FormViewUser_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        hdnUserCountryId.Value = "10";
        if (hdnUserCountryId.Value == "" || hdnUserCountryId.Value == "0")
        {
            lblError.Text = "Please Select Country Name!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
            return;
        }

        string strDateOfBirth = "";

        if (e.Values["dtBirthDate"] != null)
        {
            strDateOfBirth = e.Values["dtBirthDate"].ToString().Trim();
        }

        if (strDateOfBirth != "")
        {
            strDateOfBirth = Commonfunctions.CDateTime(strDateOfBirth).ToShortDateString();
            e.Values["dtBirthDate"] = strDateOfBirth;
        }

        e.Values["CountryID"] = hdnUserCountryId.Value;

    }

    protected void FormViewUser_Updating(object sender, FormViewUpdateEventArgs e)
    {
        string strDateOfBirth = e.NewValues["dtBirthDate"].ToString().Trim();

        if (strDateOfBirth != "")
        {
            strDateOfBirth = Commonfunctions.CDateTime(strDateOfBirth).ToShortDateString();
        }

        //e.NewValues["lid"] = UserId.ToString();
        e.NewValues["CountryID"] = hdnUserCountryId.Value;
        e.NewValues["dtBirthDate"] = strDateOfBirth;

    }

    protected void FormViewUser_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.KeepInInsertMode = true;
            e.ExceptionHandled = true;

            lblError.Visible = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else if (e.AffectedRows == -1)
        {
            e.KeepInInsertMode = true;
        }
    }

    protected void FormViewUser_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.KeepInEditMode = true;
            e.ExceptionHandled = true;

            lblError.Visible = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
        else if (e.AffectedRows == -1)
        {
            e.KeepInEditMode = true;
        }
    }

    protected void FormViewUser_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lblError.Visible = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }
    }

    protected void SqlDataSourceUser_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lblError.Visible = true;

        if (Result > 0)
        {
            lblError.Text = "Contact Detail Added Successfully. <BR>";
            lblError.CssClass = "success";

            FormViewUser.DataBind();

            gvCustomerUser.DataBind();

            string strEmpName = e.Command.Parameters["@EmpName"].Value.ToString();
            string strEmail = e.Command.Parameters["@Email"].Value.ToString();
            string strpassCode = e.Command.Parameters["@Password"].Value.ToString();

            //lblError.Text += EmailNotification(strEmail, strpassCode);
        }
        else if (Result == 0)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lblError.Text = "Email Already Exists. Please Enter Different Email Address";
            lblError.CssClass = "errorMsg";

        }
    }

    protected void SqlDataSourceUser_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection UpdParams = ((SqlDataSourceView)sender).UpdateParameters;

        Hashtable ht = new Hashtable();
        foreach (Parameter UpdParam in UpdParams)
            ht.Add(UpdParam.Name, true);

        for (int i = 0; i < CmdParams.Count; i++)
        {
            if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
                CmdParams.Remove(CmdParams[i--]);
        }

    }

    protected void SqlDataSourceUser_Deleting(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection DelParams = ((SqlDataSourceView)sender).DeleteParameters;

        Hashtable ht = new Hashtable();
        foreach (Parameter DelParam in DelParams)
            ht.Add(DelParam.Name, true);

        for (int i = 0; i < CmdParams.Count; i++)
        {
            if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
                CmdParams.Remove(CmdParams[i--]);
        }

    }

    protected void SqlDataSourceUser_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblError.CssClass = "success";
            lblError.Text = "User Details Updated Successfully!";

            gvCustomerUser.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";

        }
        else if (Result == 2)
        {
            lblError.Text = "Email Already Exists. Please Use Different Email Address";
            lblError.CssClass = "errorMsg";

        }
    }

    protected void SqlDataSourceUser_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lblError.CssClass = "success";
            lblError.Text = "User Details Deleted Successfully!";
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";

        }
        else if (Result == 2)
        {
            lblError.Text = "Employee Details Not Found!";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

    #region User function

    protected void btnAddUserPlant_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        lblError.Text = "";
        int CustomerUserId = Convert.ToInt32(FormViewUser.DataKey["lId"]);
        int Result = -123;

        if (CustomerUserId == 0)
        {
            lblError.Text = "Please Add Customer User!";
            lblError.CssClass = "errorMsg";
        }
        else if (ddUserPlant.SelectedIndex > 0)
        {
            int PlantId = Convert.ToInt32(ddUserPlant.SelectedValue);

            Result = DBOperations.AddCustomerUserPlant(CustomerUserId, PlantId, Convert.ToInt32(LoggedInUser.glUserId));

            if (Result == 0)
            {
                lblError.Text = "Plant Added Successfully!";
                lblError.CssClass = "success";

                //listPlant.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Plant Already Exists For User!!";
                lblError.CssClass = "info";
            }
        }
        else
        {
            lblError.Text = "Please Select Plant Name!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnAddNofication_Click(object sender, EventArgs e)
    {
        int NotificationType = Convert.ToInt32(ddNotificationType.SelectedValue);
        int NotificationMode = Convert.ToInt32(ddNotificationMode.SelectedValue);
        int CustomerUserId = Convert.ToInt32(FormViewUser.DataKey["lId"]);

        if (CustomerUserId == 0)
        {
            lblError.Text = "Please Add Customer Contact!";
            lblError.CssClass = "errorMsg";
        }
        else if (NotificationType > 0 && NotificationMode > 0)
        {
            int Result = DBOperations.AddCustomerUserNotification(CustomerUserId, NotificationType, NotificationMode, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Notification Added Successfully!";
                lblError.CssClass = "success";
                gvNotification.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Notification Already Exists!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Select Notification Detail!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DataSourceNotification_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (e.AffectedRows <= 0)
        {
            e.ExceptionHandled = true;
            lblError.Text = e.Exception.Message;
            lblError.CssClass = "errorMsg";
        }

        else if (Result == 0)
        {
            lblError.Text = "Notification Removed Successfully !";
            lblError.CssClass = "success";

        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    #endregion

    protected void btnPasswrd_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;

        int CustomerUserId = Convert.ToInt32(FormViewUser.DataKey["lId"]);

        //  foreach (Control ctrl in FormView1.Controls)
        //  {
        //      string ctrlClientid = ctrl.ClientID;
        //  }

        TextBox cboPassCode = (TextBox)FormViewUser.FindControl("txtPassCode");

        if (cboPassCode.Text.Trim() != "")
        {
            int result = LoginClass.UpdateUserPassword(CustomerUserId, cboPassCode.Text.Trim());

            if (result == 0)
            {
                lblError.Text = "Password Changed Successfully!";
                lblError.CssClass = "success";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "User Detail Not Found.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Enter Password!";
            lblError.CssClass = "errorMsg";
        }
    }

    public string GetCustomerName(int CustomerId)
    {
        string strCustomerName = DBOperations.GetCustomerNameById(CustomerId.ToString());

        return strCustomerName;
    }

    private string EmailNotification(string strEmail, string strPassword)
    {
        string EmailContent = "", strReturnMessage = "";

        if (strEmail != "" || strPassword != "")
        {
            //************************************************
            // Customer User Accunt Creation Email
            //************************************************

            try
            {
                string strFileName = "EmailTemplate/EmailCustUserCreation.txt";

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

            string MessageBody = "";

            MessageBody = EmailContent.Replace("<AccountEmail>", strEmail);
            MessageBody = MessageBody.Replace("<EmpName>", LoggedInUser.glEmpName);

            try
            {
                string strSubject = "Babaji Import Trakcing Login Detail";

                string EmailBody = MessageBody;

                EMail.SendMail(LoggedInUser.glUserName, strEmail, strSubject, EmailBody, "");

                strReturnMessage = "Account Creation Email Successfully Sent";
            }
            catch (System.Exception ex)
            {
                strReturnMessage = ex.Message.ToString();
                return strReturnMessage;
            }
        }

        return strReturnMessage;

    }
    #endregion

    protected void gvKYCCopys_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadKYCDoc("\\KYC\\" + DocPath);
        }
    }

    private void DownloadKYCDoc(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\KYC\\" + DocumentPath);
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

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        int CompanyId = Convert.ToInt32(Session["TR_TransporterID"]);
        int StatusActivity = Convert.ToInt32(ddlStatus.SelectedValue);
        int result = DBOperations.UpdStatusActivity(CompanyId, StatusActivity, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Status Activity Updated Successfully!";
            lblError.CssClass = "success";
            ddlStatus.SelectedIndex = 0;
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Status Activity Already Exist!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnBack_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("TransporterList.aspx");
    }

    protected void gvStatusActivity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == 0)
        {
            e.Row.BackColor = System.Drawing.Color.Green; // Set the background color to red for the first row
        }
    }

}