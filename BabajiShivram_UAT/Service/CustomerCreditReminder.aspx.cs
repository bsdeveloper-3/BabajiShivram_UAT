using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_CustomerCreditReminder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Credit Reminder History";

            gvCustomers.DataSource = DBOperations.GetCreditActiveCustomer();
            gvCustomers.DataBind();
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strCustomerName = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvCredit = e.Row.FindControl("gvCredit") as GridView;
            gvCredit.DataSource = DBOperations.GetCreditByCustomerName(strCustomerName);
            gvCredit.DataBind();
        }
    }
}