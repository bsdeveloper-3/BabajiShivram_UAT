using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web;
public partial class Transport_Equipment : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lberror.Text    = "";
            lberror.Visible = false;
            Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   = "Transport Equipment";
        }

        //
        DataFilter1.DataSource  = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvEquipment.Columns;
        DataFilter1.FilterSessionID = "Equipment.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    #region FormView Event

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "cancel")
        {
            gvEquipment.SelectedIndex = -1;
            gvEquipment.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;
            FormViewDataSource.SelectParameters[0].DefaultValue = "-1"; ;
        }
        else
        {
            gvEquipment.Visible = false;
            DataFilter1.Visible = false;
            fsMainBorder.Visible = false;
        }

    }

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        if (FormView1.CurrentMode == FormViewMode.Edit)
        {
            DataRowView drv = (DataRowView)FormView1.DataItem;
            if (drv != null)
            {
                
            }
        }

        // Always Show Required Field Validator Message 

        Page.Validate("Required");
    }
    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }
    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }

    }
    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
        }
    }
    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;
        if (Result > 0)
        {
            lberror.Text = "Equipment Detail Added Successfully.";
            lberror.CssClass = "success";
            FormViewDataSource.SelectParameters[0].DefaultValue = Result.ToString();
            gvEquipment.SelectedIndex = -1;
            gvEquipment.DataBind();
        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Vehicle No Already Exists!. Please Check Record!";
            lberror.CssClass = "errorMsg";

        }

    }
    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Equipment Detail Updated Successfully !";
            lberror.CssClass = "success";

            gvEquipment.SelectedIndex = -1;
            gvEquipment.DataBind();
            gvEquipment.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Equipment Already Exists.!";
            lberror.CssClass = "errorMsg";
        }
    }
    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Equipment Deleted Successfully !";
            lberror.CssClass = "success";

            gvEquipment.SelectedIndex = -1;
            gvEquipment.DataBind();
            gvEquipment.Visible = true;
            fsMainBorder.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
    }
    protected void FormviewSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lberror.Visible = true;
            lberror.Text = e.Exception.Message;
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    #region User Branch/Customer
    protected void btnAddBranch_Cick(object sender, EventArgs e)
    {
        int Result = -123;
        HiddenField cboBranchId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnBranchId");
        TextBox cboBranchName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtBranchName");
        int BranchId = int.Parse(cboBranchId.Value);

        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");
        int UserId = int.Parse(cboUserId.Value);
        int ldate = Convert.ToInt32(Commonfunctions.date_Format());
        DateTime CurrentDate = System.DateTime.Now;
        lberror.Visible = true;
        if (BranchId > 0 && UserId > 0)
        {
            Result = DBOperations.AddUserBranch(UserId, BranchId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "Branch Successfully Added";
                lberror.CssClass = "success";
                cboBranchName.Text = String.Empty;
                cboBranchId.Value = "0";

                SqlDataSource cboDataSourceBranch = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceBranch");
                cboDataSourceBranch.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewBranch = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvBranch");

                cboGridViewBranch.DataBind();
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please try after sometime.";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "Branch already exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lberror.Text = "Please select Branch!";
            lberror.CssClass = "errorMsg";
        }

    }

    protected void btnAddAllBranch_Cick(object seder, EventArgs e)
    {
        int Result = -123;
        lberror.Text = "";
        lberror.Visible = true;

        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");
        CheckBox cblAllBranch = (CheckBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$chkAllBranch");

        int UserId = Int32.Parse(cboUserId.Value);
        bool IsAllChecked = cblAllBranch.Checked;

        if (IsAllChecked)
        {
            Result = DBOperations.AddUserBranchAll(UserId, true, LoggedInUser.glUserId); // Add All Branch for User

            if (Result == 0)
            {
                cblAllBranch.Checked = false;
                lberror.Text = "All Branches Added Successfully!";
                lberror.CssClass = "success";

                SqlDataSource cboDataSourceBranch = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceBranch");
                cboDataSourceBranch.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewBranch = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvBranch");
                cboGridViewBranch.DataBind();
            }
            else
            {
                lberror.Text = "System Error!. Please Try Again!";
                lberror.CssClass = "errorMsg";
            }

        }
        else
        {
            lberror.Text = "Please Select All Branch Check Box!";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void btnAddCustomer_Cick(object sender, EventArgs e)
    {
        int Result = -123;
        HiddenField cboCustomerId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnCustId");
        TextBox cboCustomerName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtCustomerName");
        int CustomerId = int.Parse(cboCustomerId.Value);

        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");
        int UserId = int.Parse(cboUserId.Value);
        int ldate = Convert.ToInt32(Commonfunctions.date_Format());
        DateTime CurrentDate = System.DateTime.Now;
        lberror.Visible = true;
        if (CustomerId > 0 && UserId > 0)
        {
            Result = DBOperations.AddUserCustomer(UserId, CustomerId, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "Customer Successfully Added!";
                lberror.CssClass = "success";
                cboCustomerName.Text = String.Empty;
                cboCustomerId.Value = "0";

                SqlDataSource cboDataSourceCustomer = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceCustomer");
                cboDataSourceCustomer.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewCustomer = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvCustomer");
                cboGridViewCustomer.DataBind();
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please try after sometime.";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "Customer already exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            // Error
            lberror.Text = "Please select Customer!";
            lberror.CssClass = "errorMsg";
        }

    }

    protected void btnAddAllCustomer_Click(object seder, EventArgs e)
    {
        int Result = -123;
        lberror.Text = "";
        lberror.Visible = true;

        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");
        CheckBox cblAllCustomer = (CheckBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$chkAllCustomer");

        int UserId = Int32.Parse(cboUserId.Value);
        bool IsAllChecked = cblAllCustomer.Checked;

        if (IsAllChecked)
        {
            Result = DBOperations.AddUserCustomerAll(UserId, true, LoggedInUser.glUserId); // Add All customer to user


            if (Result == 0)
            {
                lberror.Text = "All Customer Added Successfully!";
                lberror.CssClass = "success";
                cblAllCustomer.Checked = false;

                SqlDataSource cboDataSourceCustomer = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceCustomer");
                cboDataSourceCustomer.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewCustomer = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvCustomer");
                cboGridViewCustomer.DataBind();
            }
            else
            {
                lberror.Text = "Operaiont Time Out. Please Try Again!";
                lberror.CssClass = "errorMsg";
            }

        }
        else
        {
            lberror.Text = "Please Select Check Box To Add All Customer!";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void btnRemoveAllCust_Click(object sender, EventArgs e)
    {
        int Result = -123;
        lberror.Text = "";
        lberror.Visible = true;

        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");
        CheckBox cblRemoveAllCustomer = (CheckBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$chkRemoveAllCust");

        int UserId = Int32.Parse(cboUserId.Value);
        bool IsAllChecked = cblRemoveAllCustomer.Checked;

        if (IsAllChecked)
        {
            Result = DBOperations.DeleteUserCustomerAll(UserId, true, LoggedInUser.glUserId); // Remove All customer to user


            if (Result == 0)
            {
                lberror.Text = "All Customer Removed Successfully!";
                lberror.CssClass = "success";
                cblRemoveAllCustomer.Checked = false;

                SqlDataSource cboDataSourceCustomer = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceCustomer");
                cboDataSourceCustomer.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewCustomer = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvCustomer");
                cboGridViewCustomer.DataBind();
            }
            else if (Result == 2)
            {
                lberror.Text = "Please Select Check Box To Remove All Customer!";
                lberror.CssClass = "errorMsg";
            }
            else
            {
                lberror.Text = "System Error. Please Try Again!";
                lberror.CssClass = "errorMsg";
            }

        }
        else
        {
            lberror.Text = "Please Select Check Box To Remove All Customer!";
            lberror.CssClass = "errorMsg";
        }
    }
    #endregion

    #region User Notification
    protected void btnAddNofication_Click(object sender, EventArgs e)
    {
        DropDownList cboddNotificationType = (DropDownList)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$ddNotificationType");
        CheckBoxList cbochkNotifyType = (CheckBoxList)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$chkNotifyType");
        HiddenField cboUserId = (HiddenField)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$hdnUserId");

        bool IsEmail = false, IsSMS = false;

        IsEmail = cbochkNotifyType.Items[0].Selected;
        IsSMS = cbochkNotifyType.Items[1].Selected;

        int NotificationType = Convert.ToInt32(cboddNotificationType.SelectedValue);
        int UserId = int.Parse(cboUserId.Value);

        if (UserId == 0)
        {
            lberror.Text = "User Details Not Found!";
            lberror.CssClass = "errorMsg";
            return;
        }
        else if (IsEmail == false && IsSMS == false)
        {
            lberror.Text = "Please Check Email or SMS For Notification!";
            lberror.CssClass = "errorMsg";
            return;
        }
        else
        {
            int Result = DBOperations.AddUserNotification(UserId, NotificationType, IsEmail, IsSMS, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lberror.Text = "Notification Added Successfully!";
                lberror.CssClass = "success";

                SqlDataSource cboDataSourceUserNotify = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourceUserNotify");
                cboDataSourceUserNotify.SelectParameters[0].DefaultValue = UserId.ToString();

                GridView cboGridViewNotify = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvNotification");
                cboGridViewNotify.DataBind();

                cbochkNotifyType.Items[0].Selected = false;
                cbochkNotifyType.Items[1].Selected = false;
            }
            else if (Result == 1)
            {
                lberror.Text = "System Error! Please try after sometime.";
                lberror.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lberror.Text = "Notification Already Exists!";
                lberror.CssClass = "errorMsg";
            }
        }

    }

    protected void DataSourceNotification_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (e.AffectedRows <= 0)
        {
            e.ExceptionHandled = true;
            lberror.Text = e.Exception.Message;
            lberror.CssClass = "errorMsg";
        }

        else if (Result == 0)
        {
            lberror.Text = "Notification Removed Successfully !";
            lberror.CssClass = "success";

        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
    }
    #endregion

    #region GridView Event
    protected void gvEquipment_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvEquipment.Visible = false;
        DataFilter1.Visible = false;
        fsMainBorder.Visible = false;
        if (gvEquipment.SelectedIndex == -1)
        {
            FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }

        FormView1.DataBind();
    }

    protected void gvEquipment_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    #endregion

    #region Reset Passord

    protected void btnPasswrd_Click(object sender, EventArgs e)
    {
        lberror.Visible = true;

        int UserId = Convert.ToInt32(FormView1.DataKey["lid"].ToString());

        TextBox cboPassCode = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$txtPassCode");

        if (cboPassCode.Text.Trim() != "")
        {
            int result = LoginClass.UpdateUserPassword(UserId, cboPassCode.Text.Trim());

            if (result == 0)
            {
                lberror.Text = "Password Changed Successfully!";
                lberror.CssClass = "success";
            }
            else if (result == 1)
            {
                lberror.Text = "System Error! Please try after sometime.";
                lberror.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lberror.Text = "User Detail Not Found.";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            lberror.Text = "Please Enter Password!";
            lberror.CssClass = "errorMsg";
        }
    }
    #endregion

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "Equipment.aspx";
            DataFilter1.FilterDataSource();
            gvEquipment.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData
    
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "BabajiUserDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvEquipment.AllowPaging = false;
        gvEquipment.AllowSorting = false;
        gvEquipment.Columns[1].Visible = false;
        gvEquipment.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "UserDetail.aspx";
        DataFilter1.FilterDataSource();
        gvEquipment.DataBind();

        gvEquipment.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Document Upload/Download/Delete

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int VehicleID = 0, DocTypeID = 0, RenewalMonth = 0;

        string strDocTypeName = "", strDocPath = "", strRemark = "";
        string strFileUploadPath = "VehicleDoc\\";

        DateTime dtValidFrom = DateTime.MinValue;
        DateTime dtValidTill = DateTime.MinValue;

        HiddenField hdnVehicleID = (HiddenField) FormView1.FindControl("hdnVehicleID");

        VehicleID = Convert.ToInt32(hdnVehicleID.Value);

        strDocTypeName = txtDocName.Text.Trim();

        if (txtValidFrom.Text.Trim() != "")
        {
            dtValidFrom = Commonfunctions.CDateTime(txtValidFrom.Text.Trim());
        }

        if (txtValidTo.Text.Trim() != "")
        {
            dtValidTill = Commonfunctions.CDateTime(txtValidTo.Text.Trim());
        }

        if (txtRenewalMonth.Text.Trim() != "")
        {
            RenewalMonth = Convert.ToInt32(txtRenewalMonth.Text.Trim());
        }

        if (fuDocument.HasFile)
        {
           string fileName  = UploadFiles(fuDocument, strFileUploadPath);

            if (fileName != "")
            {
                strDocPath = strFileUploadPath + fileName;

                int result = DBOperations.AddEquipmentDocument(VehicleID, strDocTypeName, DocTypeID, dtValidFrom, dtValidTill, RenewalMonth, strDocPath, strRemark, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lberror.Text = "Document Uploaded Successfully!";
                    lberror.CssClass = "success";

                    gvTransportDocument.DataBind();
                }
                else
                {
                    lberror.Text = "System Error! Please Try After Sometime.";
                    lberror.CssClass = "errorMsg";

                }
            }
        }
        else
        {
            lberror.Text = "Please Upload Document!";
            lberror.CssClass = "errorMsg";
        }
    }
    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
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
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

            return FileName;
        }
        else
        {
            return "";
        }

    }

    protected void gvTransportDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "removedocument")
        {
            int DocID = Convert.ToInt32(e.CommandArgument.ToString());

            int result = DBOperations.DeleteEquipmentDocument(DocID, LoggedInUser.glUserId);

            if (result == 0)
            {
                lberror.Text = "Document Deleted Successfully!";
                lberror.CssClass = "success";

                gvTransportDocument.DataBind();
            }
            else
            {
                lberror.Text        = "System Error! Please try after sometime";
                lberror.CssClass    = "errorMsg";
            }

        }
    }

    private void DownloadDocument(string DocumentPath)
    {

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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

    public string DeleteFiles(string FilePath)
    {
        string ServerFilePath = FileServer.GetFileServerDir();
        string Message = "Success";

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        try
        {
            if (System.IO.File.Exists(ServerFilePath))
            {
                System.IO.File.Delete(ServerFilePath);
            }
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }

        return Message;


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
    
}