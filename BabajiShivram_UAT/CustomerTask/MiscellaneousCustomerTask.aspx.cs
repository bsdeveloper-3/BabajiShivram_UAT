using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class MiscellaneousCustomerTask : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text   = "New Customer Task";

        if (!IsPostBack)
        {
            DBOperations.FillCompanyByCategory(ddlCustomer, 1);
            DBOperations.FillBabajiUser(ddlAssignedemp);
            DBOperations.FillBranch(ddlBranch);
            DBOperations.FillPriority(ddlPriority);
            DBOperations.FillJobRefNum(ddlBillablalJobNo, 1);
        }
    }

    protected void btnRef_Click(object sender, EventArgs e)
    {
        ddlBillablalJobNo.SelectedIndex = 0;
        ddlBranch.SelectedIndex = 0;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        int CustomerId = Convert.ToInt32(ddlCustomer.SelectedValue);
        Boolean IsBillabale = false;
        if (rblBillabal.SelectedValue == "1")
        {
            IsBillabale = true;

        }
        string CustomerName = ddlCustomer.SelectedItem.Text.Trim();
        string EmployeeEmailId = lblEmployeeEmailId.Text.Trim();
        //string CustomerName = Convert.ToInt32(ddlCustomer.SelectedValue);
        int JobID = Convert.ToInt32(ddlBillablalJobNo.SelectedValue);
        int OperatioMMSId = 0;
        OperatioMMSId = Convert.ToInt32(ddOperationJobMMS.SelectedValue);

        string JobRefNo = ddlBillablalJobNo.SelectedItem.Text.Trim();

        int BranchId = Convert.ToInt32(ddlBranch.SelectedValue);
        string BranchName = ddlBranch.SelectedItem.Text.Trim();
        string ActivityDetail = txtACTIVTIdETAILS.Text.Trim();
        int priorityId = Convert.ToInt32(ddlPriority.SelectedValue);
        //int priorityId = 0;
        //priorityId = Convert.ToInt32(ddlPriority.SelectedValue);
        string priorityName = ddlPriority.SelectedItem.Text.Trim();
        int EmpID = Convert.ToInt32(ddlAssignedemp.SelectedValue);
        string EmployeeName = ddlAssignedemp.SelectedItem.Text.Trim();
        int ActivityTypeId = 0;
        ActivityTypeId = Convert.ToInt32(ddlActivityType.SelectedValue);
        string ActivityType = ddlActivityType.SelectedItem.Text.Trim();
        string Subject = TxtSubject.Text.Trim();
        string ContactPerson = TxtContactPerson.Text.Trim();
        DateTime StartDate = Commonfunctions.CDateTime(TxtStartDate.Text.Trim());
        DateTime EstimateDate = Commonfunctions.CDateTime(TxtEstimatedDate.Text.Trim());
        //string strCustFilePath ="";
        //if (FuUplodeCustDoc.HasFile)
        //{
        //    strCustFilePath = UploadDocument(FuUplodeCustDoc);
        //}
        string strCustFilePath = "";

        if (FuUplodeCustDoc.FileName.Trim() != "")
        {
            strCustFilePath = UploadDocument(FuUplodeCustDoc);
        }

        int Result = DBOperations.AddCustomerTaskBabajiToEmployee(CustomerId, BranchId, IsBillabale, JobID, ActivityDetail,
            priorityId, OperatioMMSId, EmpID, ActivityTypeId, Subject, ContactPerson, StartDate, EstimateDate, strCustFilePath, LoggedInUser.glUserId);

        EmailNotification(CustomerName, IsBillabale, EmployeeName, JobRefNo, BranchName, ActivityDetail, priorityName, ActivityType, Subject,
        ContactPerson, EmployeeEmailId, strCustFilePath);

        if (Result > 0)

        {
            Lblerror.Text = "Customer Task Added Successfully";
            txtACTIVTIdETAILS.Text = "";
            TxtContactPerson.Text = "";
            TxtEstimatedDate.Text = "";
            TxtStartDate.Text = "";
            TxtSubject.Text = "";
            ddlActivityType.SelectedValue = "0";
            ddlAssignedemp.SelectedValue = "0";
            ddlBranch.SelectedValue = "0";
            ddlPriority.SelectedValue = "0";
            ddlCustomer.SelectedValue = "0";
            ddlBillablalJobNo.SelectedValue = "0";
           
        }
        else if (Result == 0)
        {
            Lblerror.Text = "System Error! ";

        }
        else if (Result == -1)
        {
            Lblerror.Text = "Miscellaneous Customer Task Detaile Already Exists! ";

        }


    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedIndex > 0)
        {
            ddlBranch.SelectedIndex = 0;
            ddlBillablalJobNo.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
            ddOperationJobMMS.SelectedIndex = 0;
            TxtContactPerson.Text = "";
            txtACTIVTIdETAILS.Text = "";
            TxtStartDate.Text = "";
            TxtSubject.Text = "";
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        if (ddlBranch.SelectedIndex > 0)
        {

             int BranchId = 0;
             BranchId = Convert.ToInt32(ddlBranch.SelectedValue);
             int CustomerId = 0;
             CustomerId = Convert.ToInt32(ddlCustomer.SelectedValue);
             int OperationmmsId = 0;
              OperationmmsId = Convert.ToInt32(ddOperationJobMMS.SelectedValue);
             int FinYearId = 0;
              if (Convert.ToString(Session["FinYearId"]) != null)
                FinYearId = Convert.ToInt32(Session["FinYearId"]);
             DBOperations.FillBranchByjobNo(ddlBillablalJobNo, BranchId, CustomerId, OperationmmsId, FinYearId);
        }

       if (ddlBranch.SelectedValue !=null && rblBillabal.SelectedValue=="1" )
        {
            ddlBillablalJobNo.Enabled = true;
        }

    }

    protected void ddOperationJobMMS_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddOperationJobMMS.SelectedIndex > 0)
        {

            if (ddOperationJobMMS.SelectedValue == "1")
            {
                ddlBranch.Enabled = true;
            }
            else
            {
                ddlBranch.Enabled = true;
            }
        }
    }
    protected void ddlAssignedemp_SelectedIndexChanged(object sender, EventArgs e)
    {
        int EmployeeId = Convert.ToInt32(ddlAssignedemp.SelectedValue);
        DataSet dsAssign = DBOperations.EmployeeIdByEmailId(Convert.ToInt32(EmployeeId));
        if (dsAssign.Tables[0].Rows.Count > 0)
        {
            lblEmployeeEmailId.Text = dsAssign.Tables[0].Rows[0]["sEmail"].ToString();
        }

    }




    protected void rblBillabal_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        if (rblBillabal.SelectedValue == "1")
        {
            ddlBranch.Enabled = true;
            RFVBillable.Enabled = true;
            ddOperationJobMMS.Enabled = true;
        }
        else
        {
            ddlBillablalJobNo.Enabled = false;
            ddOperationJobMMS.Enabled = false;
            ddlBranch.Enabled = true;
            RFVBillable.Enabled = false;
            ddOperationJobMMS.SelectedIndex = 0;
            ddlBillablalJobNo.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;

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
    private string EmailNotification(string CustomerName, Boolean IsBillabale, string EmployeeName, string BillablJobID, string BranchName, string ActivityDetail, string priorityName, string ActivityType,
        string Subject, string ContactPerson, string EmployeeEmailId, string strCustFilePath)
    {

         string strBillable = ""; 
        string strReturnMessage = "";
        string EmailContent = "";
        string strSubject = "", MessageBody = "", EmailBody = "";

        string strCustomerUserEmail = "";
        string strTaskUser = "";

        List<string> lstFilePath = new List<string>();


        if (strCustFilePath != "")
        {
            lstFilePath.Add(strCustFilePath);
        }

        if (IsBillabale == true)
        {
            strBillable = "Yes";
        }
        else
        {
            strBillable = "No";
            BillablJobID = "";
        }
        strCustomerUserEmail += LoggedInUser.glUserName; // Email Copy To Noting User

        strTaskUser += LoggedInUser.glEmpName;

        try
        {
            string strFileName = "../EmailTemplate/TaskEmailEmployee.txt";

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
        MessageBody = EmailContent.Replace("<CustomerName1>", CustomerName);
        MessageBody = MessageBody.Replace("<strBillable1>", strBillable);
        MessageBody = MessageBody.Replace("<BranchName1>", BranchName);
        MessageBody = MessageBody.Replace("<BillablJobNo1>", BillablJobID);
        MessageBody = MessageBody.Replace("<ActivityDetail1>", ActivityDetail);
        MessageBody = MessageBody.Replace("<priorityName1>", priorityName);
        MessageBody = MessageBody.Replace("<ActivityType1>", ActivityType);
        MessageBody = MessageBody.Replace("<Subject1>", Subject);
        MessageBody = MessageBody.Replace("<EmployeeName1>", EmployeeName);
        MessageBody = MessageBody.Replace("<fromAssignetask1>", strTaskUser);
        MessageBody = MessageBody.Replace("<ContactPerson1>", ContactPerson);

        try
        {
            strSubject = "Customer Task Details For -" + CustomerName;
            EmailBody = MessageBody;


            EMail.SendMailBCC("",EmployeeEmailId,strCustomerUserEmail,"javed.shaikh@babajishivram.com", strSubject, EmailBody, strCustFilePath);
            strReturnMessage = "Customer Task Details Email Send To - ";
        }

        catch (System.Exception ex)
        {
            strReturnMessage = "mail sending faild!";
        }

        return strReturnMessage;

    }


}