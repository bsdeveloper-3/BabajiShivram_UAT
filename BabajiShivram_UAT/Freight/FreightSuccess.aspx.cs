using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Freight_FreightSuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Freight Status";
                
        if (!IsPostBack)
        {
            if (Session["Success"] != null)
            {
                lblFreightMessage.Text = Session["Success"].ToString();
                Session["Success"] = null;
            }
            else if (Request.QueryString["FEnquiry"] != null)
            {
                lblFreightMessage.Text = "Freight Enquiry Added Succssfully!";
            }
        }
    }
}