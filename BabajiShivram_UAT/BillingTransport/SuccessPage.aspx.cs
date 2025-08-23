using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BillingTransport_SuccessPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Success Page";

        Session["TRConsolidateId"] = null;
        Session["TRId"] = null;
        Session["TransBillId"] = null;

        if (!IsPostBack)
        {
            if (Request.QueryString["Bill"] != null)
            {
                lblMessage.Text = "Bill Detail Added Successfully.";
            }
                  
        }
    }
}