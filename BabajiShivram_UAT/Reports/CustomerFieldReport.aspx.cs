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

public partial class Reports_CustomerFieldReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Additional Field Report";

            DBOperations.FillCustomer(ddCustomer);
        }
    }

    protected void btnShowReport_Click(Object Sender, EventArgs e)
    {
        int Result = -123;
        int CustomerId = Convert.ToInt32(ddCustomer.SelectedValue);
        
        if (CustomerId > 0)
        {
            DataSet dsReport = ReportOperations.GetCustomerAdditionalJobField(CustomerId);
            if (dsReport.Tables.Count > 0)
            {
                GridViewJobReport.Visible = true;
                GridViewJobReport.DataSource = dsReport;
                GridViewJobReport.DataBind();
                lblError.Text = "";
            }
            else
            {
                GridViewJobReport.Visible = false;
                lblError.Text = "No Additional Field Details Found!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Select Customer!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(Object Sender, EventArgs e)
    {

    }
}
