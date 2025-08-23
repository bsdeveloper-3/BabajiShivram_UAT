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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class PCA_AddtnlBillStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("BillStatus.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Job Detail";
        }
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("BillStatus.aspx");
    }

}