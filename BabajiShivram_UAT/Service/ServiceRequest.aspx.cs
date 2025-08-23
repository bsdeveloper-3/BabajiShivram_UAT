using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_ServiceRequest : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Success"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "HO Dept Service Request";
                        
        }
    }

    protected void btnSubmit_Click(Object sender, EventArgs e)
    {
        int DeptId = Convert.ToInt32(ddDepartment.SelectedValue);

        string strEmpName      = txtEmpName.Text.Trim();
        string strBranchName   = txtBranchName.Text.Trim();
        string strIssueRemark  = txtRemarks.Text.Trim();

        if (DeptId <= 0)
        {
            lblError.Text = "Please Select Department Name";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (strBranchName == "")
        {
            lblError.Text = "Please Enter Branch Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (strEmpName == "")
        {
            lblError.Text = "Please Enter Branch Employee Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (strIssueRemark == "")
        {
            lblError.Text = "Please Enter Issue Detail!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int result = DBOperations.AddServiceRequest(DeptId, strEmpName,strBranchName, strIssueRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Employee Issue Details Forwarded To HO Dept Head!";
            lblError.CssClass = "success";
            ClearField();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Issue Details Already Registered! Please Wait for Head Office Resolution!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        ClearField();
    }

    private void ClearField()
    {
        ddDepartment.SelectedIndex = 0;
        txtBranchName.Text.Trim();
        txtEmpName.Text = "";
        txtRemarks.Text = "";
    }
      
}