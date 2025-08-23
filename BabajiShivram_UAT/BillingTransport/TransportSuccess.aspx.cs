using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccountTransport_TransportSuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Status";

        Session["TransPayId"]   =   null;
        Session["JobId"]        =   null;
        Session["InvoiceId"]    =   null;

        if (!IsPostBack)
        {
            if (Session["Error"] != null)
            {
                lblMessage.Text = Session["Error"].ToString();
                lblMessage.CssClass = "errorMsgImg";
                Session["Error"] = null;
            }
            else if (Session["Success"] != null)
            {
                lblMessage.Text = Session["Success"].ToString();
                Session["Success"] = null;
            }

        }
    }
}