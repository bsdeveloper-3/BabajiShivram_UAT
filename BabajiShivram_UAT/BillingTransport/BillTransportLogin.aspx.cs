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
using System.Net;
using System.Xml;

public partial class BillingTransport_BillTransportLogin : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblErrorMsg.Visible = false;
            txtEmail.Focus();
            Session.Clear();
            Session.RemoveAll();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int result = loggedInUser.ValidateTransportLogin(Convert.ToString(txtEmail.Text), Convert.ToString(txtPassword.Text));
        if (result == 0)
        {
            Response.Redirect("DashBoard.aspx");
        }
        else if (result == 1)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.InnerHtml = "Wrong Password..!!";
            loggedInUser.ClearSession();
        }
        else if (result == 2)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.InnerHtml = "You are not authorised, Please contact administrator (admin@babajishivram.com)";
            loggedInUser.ClearSession();
        }
    }

    
}