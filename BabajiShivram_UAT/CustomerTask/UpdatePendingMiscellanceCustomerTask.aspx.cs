using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Data;
public partial class UpdatePendingMiscellanceCustomerTask : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Update Pending Customer Task";
            
            if (Session["TaskID"] != null)
            {
                DBOperations.FillJobRefNum(ddlBillablalJobNo, 1);
                DBOperations.FillCustTaskStatus(ddlStatus);
                GETMiscellanceCustomerTask(Convert.ToInt32(Session["TaskID"]));
                GETMiscellanceComplitedTask(Convert.ToInt32(Session["TaskID"]));
                GridMiscellanceCustomerDetails.DataBind();
            }
            
        }
        
    }
    private void GETMiscellanceComplitedTask(int CustomerID)
    {
        DataSet dsCustomer = DBOperations.GETCustomerComplitedTaskById(CustomerID);
        if (dsCustomer.Tables[0].Rows.Count > 0)
        {
            lblChack.Text = dsCustomer.Tables[0].Rows[0]["StatusID"].ToString();

            if (lblChack.Text == "4")
            {
                Panl4.Visible = false;
                PanComBack.Visible = true;
                PanBack.Visible = false;
            }

        }
    }

    protected void rblBillabal_SelectedIndexChanged(object sender, EventArgs e)
    {
        //the Autopostback  property of the RadioButtonList1 should be True
        if (rblBillabal.SelectedValue == "1")
        {
           // lblJobNO.Visible = true;
            ddOperationJob.Enabled = true;
            RFVBillable.Enabled = true;

        }

        if (rblBillabal.SelectedValue == "0")
        {
            RFVBillable.Enabled = false;
            ddOperationJob.Enabled = false;
            ddlBillablalJobNo.Enabled = false;
            ddOperationJob.SelectedIndex = 0;
            ddlBillablalJobNo.SelectedIndex = 0;
         //   lblJobNO.Visible = false;

        }
    }
    protected void ddOperationJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddOperationJob.SelectedIndex > 0)
        {
            ddlBillablalJobNo.Enabled = true;
            int BranchId = 0;
             BranchId = Convert.ToInt32(lblbranchid.Text);
            int CustomerId = 0;
             CustomerId = Convert.ToInt32(lblcustid.Text);

            int OperationmmsId = 0;
             OperationmmsId = Convert.ToInt32(ddOperationJob.SelectedValue);
            int FinYearId = 0;
            if (Convert.ToString(Session["FinYearId"]) != null)
                FinYearId = Convert.ToInt32(Session["FinYearId"]);
            DBOperations.FillOperationByjobNo(ddlBillablalJobNo, BranchId, CustomerId, OperationmmsId, FinYearId);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {

        Response.Redirect("PendingMiscellaneousCustomerTask.aspx");

    }
    protected void btncompBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompletedCustomerTaskList.aspx");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlBillablalJobNo.SelectedIndex = 0;
        ddOperationJob.SelectedIndex = 0;
    }

    private void GETMiscellanceCustomerTask(int CustomerID)
    {
        DataView dsCustomer = DBOperations.GETMiscellanceCustomerDetail(CustomerID);
        lblCustomerName.Text = dsCustomer.Table.Rows[0]["CustomerName"].ToString();
        lblConsigneeName.Text = dsCustomer.Table.Rows[0]["ConsigneeName"].ToString();
        lblBranchName.Text = dsCustomer.Table.Rows[0]["BranchName"].ToString();
        lblContactPerson.Text = dsCustomer.Table.Rows[0]["ContactPerson"].ToString();
        lblStartDate.Text = dsCustomer.Table.Rows[0]["StartDate"].ToString();
        lblStartDate.Text= System.DateTime.Now.ToString("dd/MM/yyyy");
        lblestmatedate.Text = dsCustomer.Table.Rows[0]["EstimateDate"].ToString();
        lblestmatedate.Text=System.DateTime.Now.ToString("dd/MM/yyyy");
        lblPriority.Text = dsCustomer.Table.Rows[0]["Priority"].ToString();
        lblActivityType.Text = dsCustomer.Table.Rows[0]["ActivityType"].ToString();
        lblActivityDeatil.Text = dsCustomer.Table.Rows[0]["ActivityDetail"].ToString(); 
        lblSubject.Text = dsCustomer.Table.Rows[0]["Subject"].ToString();
        lblRefJobNO.Text = dsCustomer.Table.Rows[0]["JobRefNo"].ToString();
        lblBillable.Text = dsCustomer.Table.Rows[0]["IsBillable"].ToString();
        lbljobno.Text = dsCustomer.Table.Rows[0]["JobID"].ToString();
        LUserType.Text = dsCustomer.Table.Rows[0]["lUserType"].ToString();
        lblcustid.Text = dsCustomer.Table.Rows[0]["CustomerId"].ToString();
        lblbranchid.Text = dsCustomer.Table.Rows[0]["BranchId"].ToString();
        //lnkDepositReceipt. = dsCustomer.Table.Rows[0]["CustFilePath"].ToString();


        if (lblBillable.Text == "True")
        {
            lblBillable.Text = "YES";
        }
       else
        {
            lblBillable.Text = "NO";
        }
      
        if(LUserType.Text=="4")
        {
            PanBabaji.Visible = false;
            panUser.Visible = true;// Only Customer Bilabal Control Show 
            
        }
        else
        {
            PanBabaji.Visible = true;

        }
       

        GridMiscellanceCustomerDetails.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int TaskId = Convert.ToInt32(Session["TaskID"]);
        Boolean IsBillabale = false;
        int OperatioMMSId = 0;
        OperatioMMSId = Convert.ToInt32(ddOperationJob.SelectedValue);
        int JobID = Convert.ToInt32(ddlBillablalJobNo.SelectedValue);
      //if(ddlBillablalJobNo.SelectedValue=="0")
      //  {
      //      JobID = lbljobno.Text.Trim();
      //  }
        //string JobNo = lblJobNO.Text.Trim();
        if (rblBillabal.SelectedValue == "1")
        {
            IsBillabale = true;

        }
        if (rblBillabal.SelectedValue == "0")
        {
            IsBillabale = false;

        }

            string FollowuUpdate = TxtFollowupUpdate.Text.Trim();
            bool IsApproved = true;
            int StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
            DateTime EstimatedDate  = Commonfunctions.CDateTime(TxtEstimatedDate.Text.Trim());
            DateTime follouptDate = Commonfunctions.CDateTime(TxtFolloupDate.Text.Trim());
            string strFilePath = "";
            if (fuUplodedoc.HasFile)
            {
                strFilePath = UploadDocument(fuUplodedoc);
            }

            int CustomerID = DBOperations.ADDMiscellaneousCustomerTaskSummary(TaskId, IsBillabale, JobID ,OperatioMMSId, EstimatedDate, FollowuUpdate, StatusID, follouptDate, strFilePath, IsApproved, LoggedInUser.glUserId);
            // EmailNotification(CustomerName, Priority, FollowuUpdate, follouptDate, status, strFilePath);

            GridMiscellanceCustomerDetails.DataBind();

            if(StatusID == 4)
            {
                Panl4.Visible = false;
            }
            if (CustomerID == 0)
            {

                lblerror.Text = "Customer Task  Updated ! ";
                GridMiscellanceCustomerDetails.DataBind();
                TxtFolloupDate.Text = "";
                TxtFollowupUpdate.Text = "";
                ddlStatus.SelectedIndex = 0;
                TxtEstimatedDate.Text = "";
                ddlBillablalJobNo.SelectedIndex = 0;
              ddOperationJob.SelectedIndex = 0;

                lblChack.Text = "";
            
            }
            else if (CustomerID == -1)
            {
                lblerror.Text = "Customer Task Detaile Already Exists! ";

            }
            else if (CustomerID == 2)
            {

                lblerror.Text = "Customer Task  Already Complited.You Cant Update Task...!";

            }

    }

    


    private string UploadDocument(FileUpload fileUpload)
    {
        string FileName = fileUpload.FileName;

        FileName = FileServer.ValidateFileName(FileName);

        string FilePath = "";

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

        if (fileUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {

                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fileUpload.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    protected void GridMiscellanceCustomerDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

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
    //protected void lnkDepositReceipt_Click(object sender, EventArgs e)
    //{
    //    HiddenField hdnReceiptPath = (HiddenField)FindControl("hdnReceiptPath");

    //    string FilePath = hdnReceiptPath.Value.Trim();

    //    DownloadDocument(FilePath);
    //}

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

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

    protected void GridMiscellanceCustomerDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strFilePath = "";

            if (DataBinder.Eval(e.Row.DataItem, "TaskFilePath") != DBNull.Value)
                strFilePath = (string)DataBinder.Eval(e.Row.DataItem, "TaskFilePath");
            

            if (strFilePath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");

                lnkDownload.Visible = false;
            }
            
        }

    }


    protected void GridMiscellanceCustomerDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string strFilePath = e.CommandArgument.ToString();
            DownloadDocument(strFilePath);
            GridMiscellanceCustomerDetails.DataBind();

        }
        else
        {
            

        }
        
    }

    



    //private string EmailNotification(string CustomerName, string Priority,string  FollowuUpdate, DateTime follouptDate, string statu , string strFilePath)
    //{

    //    string strNameofCustomer = ""; string strCompany = ""; string strKYCRegister = "";
    //    string strComanyPANnO = ""; string strEmployeeForGst = ""; string strcontactNo = ""; string strEmailAddress = "";

    //    string folloupDate = Convert.ToString(follouptDate);

    //    List<string> lstFilePath = new List<string>();


    //    if (strFilePath != "")
    //    {
    //        lstFilePath.Add(strFilePath);
    //    }

    //    string strReturnMessage = "";
    //    string EmailContent = "";
    //    string strSubject = "", MessageBody = "", EmailBody = "";

    //    string strCustomerUserEmail = "";


    //    strCustomerUserEmail += LoggedInUser.glUserName; // Email Copy To Noting User

    //    try
    //    {
    //        string strFileName = "../EmailTemplate/TaskEmailCustomer.txt";

    //        StreamReader sr = new StreamReader(Server.MapPath(strFileName));
    //        sr = File.OpenText(Server.MapPath(strFileName));
    //        EmailContent = sr.ReadToEnd();
    //        sr.Close();
    //        sr.Dispose();
    //        GC.Collect();
    //    }
    //    catch (Exception ex)
    //    {
    //        strReturnMessage = ex.Message;
    //    }
    //    MessageBody = EmailContent.Replace("<CustomerName1>", CustomerName);
    //    MessageBody = MessageBody.Replace("<Priority1>", Priority);
    //    MessageBody = MessageBody.Replace("<FollowuUpdate1>", FollowuUpdate);
    //    MessageBody = MessageBody.Replace("<FollowuUpdateDate1>", folloupDate);
    //    MessageBody = MessageBody.Replace("<statu1>", statu);


    //    try
    //    {
    //        strSubject = "Customer Task Is Updated -" + CustomerName;
    //        EmailBody = MessageBody;

    //        // EMail.SendMailMultiAttach("", "Amit.Bakshi@BabajiShivram.com", strSubject, EmailBody,lstFilePath);
    //        EMail.SendMailMultiAttach("", "ops.developer@babajishivram.com","ops.developer@babajishivram.com", strSubject,EmailBody, lstFilePath);

    //        strReturnMessage = "Customer Task Email Send To - ";
    //    }

    //    catch (System.Exception ex)
    //    {

    //        strReturnMessage = "mail sending faild!";

    //    }

    //    return strReturnMessage;

    //}

}
