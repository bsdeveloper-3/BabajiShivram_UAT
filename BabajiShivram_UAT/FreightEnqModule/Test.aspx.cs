using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.SqlClient;
//using Microsoft.Reporting.WebForms;
using System.Web.Security;

public partial class Test : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //   ReportViewer1.ServerReport.ReportPath = "SSRS/BillClosingReport.rdl";
        //CheckCertificate();
        //SqlCommand conn= new SqlCommand() ;
        //DataSet DataSet1 =  CDatabase.GetDataSet("rpt_GetrptBillBalance", conn);

        //ReportDataSource reportDataSource = new ReportDataSource("DataSet1", DataSet1.Tables[0]);

        //ReportViewer1.LocalReport.DataSources.Clear();
        //ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
        //ReportViewer1. DataSource = reportDataSource;
        //ReportViewer1.RefreshReport();
    }

    private bool CheckCertificate()
    {
        bool isValidClient = false;
        try
        {
            var x509 = new X509Certificate2(this.Request.ClientCertificate.Certificate);
            var chain = new X509Chain(true);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
            chain.Build(x509);

            var validThumbprints = new HashSet<string>(
                System.Configuration.ConfigurationManager.AppSettings["ClientCertificateIssuerThumbprints"]
                    .Replace(" ", "").Split(',', ';'),
                StringComparer.OrdinalIgnoreCase);

            string[] strTest = System.Configuration.ConfigurationManager.AppSettings["ClientCertificateIssuerThumbprints"]
                    .Replace(" ", "").Split(',', ';');

            string strKeys = "";
            // if the certificate is self-signed, verify itself.
            
            for (int i = 0; i < chain.ChainElements.Count; i++)
            {
                //strKeys = strKeys + "<BR>"+ chain.ChainElements[i].Certificate.Thumbprint;

                if (validThumbprints.Contains(chain.ChainElements[i].Certificate.Thumbprint))
                {
                    isValidClient = true;
                }

            }

        }
        catch(Exception ex)
        {
            // Send Email
        }
       
        return  isValidClient;
        
        //if (isValidClient == false)
        //{

        // //   throw new UnauthorizedAccessException("The client certificate selected is not authorized for this system. Please restart the browser and pick the valid certificate");
        //}

        //lblMessage.Text = strKeys;
        // certificate Subject would contain some identifier of the user (an ID number, SIN number or anything else unique). here it is assumed that it contains the login name and nothing else
        //if (!string.Equals("CN=" + login, x509.Subject, StringComparison.OrdinalIgnoreCase))
        //    throw new UnauthorizedAccessException("The client certificate selected is authorized for another user. Please restart the browser and pick another certificate.");
    }

    protected void btncheck_Click(object sender, EventArgs e)
    {

        string[] test = txtUserName.Text.Split(';');
        string strName = test[0].ToString();

        if (!test[0].ToString().Trim().Contains("@"))
        {
            strName = strName + "@BabajiShivram.com";
        }

        int ModuleId = Convert.ToInt32(test[1].ToString());

        int FinYearId = Convert.ToInt32(test[2].ToString());
        Session["FinYearId"] = FinYearId.ToString();
        string FinYearName = "2023-2024";// ddYear.SelectedItem.Text;

        USERDAL objUserDAL = new USERDAL();
        int result = objUserDAL.ValidateUser(strName.Trim());

        if (result == 0) 
        {
            LoggedInUser.ValidUser = true;
            string strReturnURL = "";
            if (Request.QueryString["returnUrl"] != null)
            {
                strReturnURL = Request.QueryString["returnUrl"].ToString();
            }

            FormsAuthentication.SetAuthCookie(LoggedInUser.glUserId.ToString(), false);//to use persistent cookie, set "true".
            //add a username Cookie
            Response.Cookies["FinYearId"].Value = FinYearId.ToString();
            Response.Cookies["ModuleID"].Value = ModuleId.ToString();

            Response.Cookies["FinYearId"].Expires = DateTime.Now.AddHours(8);
            Response.Cookies["ModuleID"].Expires = DateTime.Now.AddHours(8);

            LoggedInUser.glFinYearId = FinYearId;
            LoggedInUser.glFinYearName = FinYearName;
            LoggedInUser.glModuleId = Convert.ToInt16(ModuleId);

            // redirect to Home Page based on User Type
            if (LoggedInUser.glType == 1 || LoggedInUser.glType == -1 || LoggedInUser.glType == 3)//Babaji User/Administrator/Agent login
            {
                // Check Module Access For Assigned Role To Babaji User
                if (LoggedInUser.glType != -1)
                {
                    int ModuleResult = LoggedInUser.ValidateModuleRole(LoggedInUser.glModuleId, LoggedInUser.glRoleId);

                }
                                
                if (strReturnURL != "")
                {
                    Response.Redirect(strReturnURL);
                }

                if (LoggedInUser.glModuleId == 1)
                {
                    // start Billing
                    int rolename1 = BillingOperation.GetRole(LoggedInUser.glUserId, LoggedInUser.glRoleId);

                    if (rolename1 == 0) //Billing
                    {
                        Response.Redirect("../DashboardBillingUser.aspx");
                        //Response.Redirect("Dashboard_Billing.aspx");
                    }
                    else if (rolename1 == 1) //HOD
                    {
                        Response.Redirect("../Dashboard.aspx");
                    }
                    else //Others
                    {
                        Response.Redirect("../Dashboard.aspx");
                    }
                    // end Billing
                }
                else if (LoggedInUser.glModuleId == 2)
                {
                    int UserActive = DBOperations.SetUserAvailability("Login", LoggedInUser.glUserId);   //SET USER's ACTIVE MODE
                    Response.Redirect("../Freight/FreightDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 3)
                {
                    Response.Redirect("../Transport/TransDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 4)
                {
                    Response.Redirect("../Service/ServiceDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 5)
                {
                    Response.Redirect("../ExportCHA/ExportDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 6)
                {
                    Response.Redirect("../Quotation/QuoteDashboard.aspx");
                }

                else if (LoggedInUser.glModuleId == 8)
                {
                    Response.Redirect("../SEZ/SEZDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 9)
                {
                    Response.Redirect("../AccountExpense/Dashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 10) // Container Movement
                {
                    Response.Redirect("../ContMovement/CDashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 11)
                    
                    Response.Redirect("../CRM/Dashboard.aspx");
                }
                else if (LoggedInUser.glModuleId == 15)
                {
                    Response.Redirect("../ReportDashboard.aspx");
                }
                else
                {
                    //lblError.Text = "Invalid Module Information!";
                    //lblError.Visible = true;
                    //return;

                    lblMessage.Visible = true;
                    LoggedInUser.ClearSession();

                    // ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
                    this.lblMessage.Text = "Invalid Module Information!";
                }
            }
        
    }
}