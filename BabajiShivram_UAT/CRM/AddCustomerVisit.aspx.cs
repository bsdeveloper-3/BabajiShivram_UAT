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
using Ionic.Zip;

public partial class CRM_AddCustomerVisit : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnAddVisitReport);                

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Add Customer Visit";
            txtVisitDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            calVisitDate.StartDate = DateTime.Today.AddDays(-10);
            calVisitDate.EndDate = DateTime.Now;

            mevVisitDate.MinimumValue = DateTime.Now.AddDays(-10).ToString("dd/MM/yyyy");
            mevVisitDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void btnAddVisitReport_Click(object sender, EventArgs e)
    {
        DateTime dtVisitDate = DateTime.MinValue;
        if (txtVisitDate.Text.Trim() != "")
            dtVisitDate = Commonfunctions.CDateTime(txtVisitDate.Text.Trim());

        if (ddlCustomer.SelectedValue != "0")
        {
            int result = DBOperations.CRM_AddCustomerVisitReport(Convert.ToInt32(ddlCustomer.SelectedValue), dtVisitDate, Convert.ToInt32(ddCategory.SelectedValue), txtVisitRemark.Text.Trim(), LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Visit Report Added Successfully";
                lblError.CssClass = "success";
                txtVisitRemark.Text = "";
                txtVisitDate.Text = "";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Visit Report Already Exists for same date!";
                lblError.CssClass = "errorMsg";
            }
        }
        else 
        {
            lblError.Text = "Please select customer!";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnCancelVisitReport_Click(object sender, EventArgs e)
    {
        txtVisitDate.Text = "";
        txtVisitRemark.Text = "";
        lblError.Text = "";
    }

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerVisit.aspx");
    }

}