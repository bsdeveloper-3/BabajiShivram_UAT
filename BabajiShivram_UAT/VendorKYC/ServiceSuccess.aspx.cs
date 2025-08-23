using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_ServiceSuccess : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Status";

        Session["JobId"] = null;
        Session["InvoiceId"] = null;

        if (!IsPostBack)
        {
            if (Session["VendorKYCError"] != null)
            {
                lblMessage.Text = Session["VendorKYCError"].ToString();
                lblMessage.CssClass = "errorMsgImg";
                Session["VendorKYCError"] = null;
            }
            else if (Session["VendorKYCSuccess"] != null)
            {
                lblMessage.Text = Session["VendorKYCSuccess"].ToString();
                Session["VendorKYCSuccess"] = null;

            }

        }
    }
}