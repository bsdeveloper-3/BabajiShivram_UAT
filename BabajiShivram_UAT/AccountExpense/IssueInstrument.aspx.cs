using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Collections.Generic;
public partial class AccountExpense_IssueInstrument : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Issue Instrument";
    }

    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        if (txtJobNumber.Text.Trim() != "")
        {
            if (hdnJobId.Value != "0")
            {
                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value));

                if (dsGetJobDetail.Tables.Count > 0)
                {
                    if (dsGetJobDetail.Tables[0].Rows.Count > 0)
                    {
                        lblCustomer.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                        hdnCustomerId.Value = dsGetJobDetail.Tables[0].Rows[0]["CustomerID"].ToString();
                    }
                }

                ChequeSqlDataSource.DataBind();
                gvDetail.DataBind();
            }
        }
        else
        {
            // Clear Field
            lblCustomer.Text = "";
            
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("TrackIssueInstrument.aspx");

    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        int JobId = 0, CustomerId = 0;
        decimal ChequeAmount = 0, JobAmount = 0;
        string strChequeNo = "", strBankName = "", strJobRefNo = "", strPayTo = "", strPayToCode = "";
        DateTime dtChequeDate = DateTime.MinValue;
        
        if(hdnJobId.Value.Trim() != "")
        {
            JobId = Convert.ToInt32(hdnJobId.Value);
        }
        if(hdnCustomerId.Value.Trim() != "")
        {
            CustomerId = Convert.ToInt32(hdnCustomerId.Value.Trim());
        }

        if (txtChequeDate.Text.Trim() != "")
            dtChequeDate = Commonfunctions.CDateTime(txtChequeDate.Text.Trim());

        strJobRefNo = txtJobNumber.Text.Trim();
        strChequeNo = txtChequeNo.Text.Trim();
        strBankName = txtBankName.Text.Trim();

        strPayTo = txtPayTo.Text.Trim();
        strPayToCode = hdnPayToCode.Value;

        int result  = AccountExpense.FA_AddIssueInstrument(JobId, strJobRefNo, strChequeNo, dtChequeDate, strBankName, ChequeAmount,
         CustomerId, JobAmount, strPayTo, strPayToCode, LoggedInUser.glUserId);

        if(result == 0)
        {
            lblError.Text = "Details Added Successfully!";
            lblError.CssClass = "success";

            ChequeSqlDataSource.DataBind();
        }
        else if(result == 1)
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Job No And Cheque No Already Exists!";
            lblError.CssClass = "errorMsg";
        }
    }
}