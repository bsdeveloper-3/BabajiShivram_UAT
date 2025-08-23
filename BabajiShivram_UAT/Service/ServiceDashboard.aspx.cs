using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class Service_ServiceDashboard : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
 
        if (!IsPostBack)
        {
            Label lblTitle  =   (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   =   "Dashboard";

        }

    }

}