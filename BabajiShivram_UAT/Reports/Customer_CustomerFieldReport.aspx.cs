using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Reports_Customer_CustomerFieldReport : System.Web.UI.Page
{
    LoginClass loggedInCustomer = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Field Report";

            int Result = -123;
            int CustomerId = Convert.ToInt32(loggedInCustomer.glCustId);

            if (CustomerId > 0)
            {
                DataSet dsReport = ReportOperations.GetCustomerAdditionalJobField(CustomerId);
                if (dsReport.Tables.Count > 0)
                {
                    if (dsReport.Tables[0].Rows.Count > 0)
                    {
                        GridViewJobReport.DataSource = dsReport;
                        GridViewJobReport.DataBind();
                    }
                    else
                    {
                        lblError.Text = "No Additional Field Detail Found!";
                        lblError.CssClass = "errorMsg";
                    }
                }//END third IF
                else
                {
                    lblError.Text = "No Additional Field Detail Found!";
                    lblError.CssClass = "errorMsg";
                }
            }//END second IF
          
        }//END First IF

    }
}
