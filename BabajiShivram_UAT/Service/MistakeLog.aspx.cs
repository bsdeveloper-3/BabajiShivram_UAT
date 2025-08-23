using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MistakeLog : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Success"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "User Mistake Log";

            txtMistakeDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        
            DBOperations.FillBabajiUser(ddMistakeUser);

            Page.Validate();
        }
    }

    protected void btnSubmit_Click(Object sender, EventArgs e)
    {
        int MistakeUserId = Convert.ToInt32(ddMistakeUser.SelectedValue);
        int Amount = 0, StatusId = 0; 
        string CustomerName = "", MistakeRemarks = "" ;
        DateTime dtMistakeDate = DateTime.Today;

        if (txtMistakeDate.Text != "")
        {
            dtMistakeDate = Commonfunctions.CDateTime(txtMistakeDate.Text.Trim());
        }
        
        CustomerName = txtCustomer.Text.Trim();
        MistakeRemarks = txtRemarks.Text.Trim();
        StatusId = Convert.ToInt32(ddStatus.SelectedValue);

        if (txtMistakeAmount.Text.Trim() != "")
        {
            Amount = Convert.ToInt32(txtMistakeAmount.Text.Trim());
        }

        if (MistakeUserId <= 0)
        {
            lblError.Text = "Please select user name.";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (MistakeRemarks == "")
        {
            lblError.Text = "Please Enter Mistake Remarks!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int result = DBOperations.AddMistakeLog(MistakeUserId,dtMistakeDate, Amount, MistakeRemarks, CustomerName,StatusId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "User Mistake Logged Successfully";
            lblError.CssClass = "success";

            gvUserMistkeHistory.DataBind();
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Mistake Log Already Exists For " + ddMistakeUser.SelectedItem.Text;
            lblError.CssClass = "errorMsg";
        }
    }
    
    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        ddMistakeUser.SelectedIndex = 0;
        txtMistakeAmount.Text = "";
        txtCustomer.Text = "";
        txtRemarks.Text = "";
       
    }

    protected void ddMistakeUser_SelectedIndexChanged(Object sender, EventArgs e)
    {
        string MistakeBy = ddMistakeUser.SelectedItem.Text.Trim();

        if (ddMistakeUser.SelectedValue != "0")
        {
            legendLog.InnerText = MistakeBy + "- Mistake Log";
        }
        else
        {
            legendLog.InnerText =   "User Mistake Log";
        }
    }
}