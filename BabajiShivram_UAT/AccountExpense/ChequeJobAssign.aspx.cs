using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class AccountExpense_ChequeJobAssign : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        CalExtPayDueDate.EndDate = DateTime.Now;
        MskEdtValChequeDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job/Cheque Issue";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime dtChequeDate = DateTime.Now;

        if(txtPaymentDueDate.Text.Trim() != "")
        {
            dtChequeDate = Commonfunctions.CDateTime(txtPaymentDueDate.Text.Trim());
        }

        string strJobNo1 = ""; string strChequeNo1 = "";
        string strJobNo2 = ""; string strChequeNo2 = "";
        string strJobNo3 = ""; string strChequeNo3 = "";
        string strJobNo4 = ""; string strChequeNo4 = "";
        string strJobNo5 = ""; string strChequeNo5 = "";
        string strJobNo6 = ""; string strChequeNo6 = "";
        string strJobNo7 = ""; string strChequeNo7 = "";
        string strJobNo8 = ""; string strChequeNo8 = "";
        string strJobNo9 = ""; string strChequeNo9 = "";
        string strJobNo10 = ""; string strChequeNo10 = "";

        strJobNo1 = txtJobNumber1.Text.Trim();
        strChequeNo1 = txtChequeNo1.Text.Trim();

        strJobNo2 = txtJobNumber2.Text.Trim();
        strChequeNo2 = txtChequeNo2.Text.Trim();

        strJobNo3 = txtJobNumber3.Text.Trim();
        strChequeNo3 = txtChequeNo3.Text.Trim();

        strJobNo4 = txtJobNumber4.Text.Trim();
        strChequeNo4 = txtChequeNo4.Text.Trim();

        strJobNo5 = txtJobNumber5.Text.Trim();
        strChequeNo5 = txtChequeNo5.Text.Trim();

        strJobNo6 = txtJobNumber6.Text.Trim();
        strChequeNo6 = txtChequeNo6.Text.Trim();

        strJobNo7 = txtJobNumber7.Text.Trim();
        strChequeNo7 = txtChequeNo7.Text.Trim();

        strJobNo8 = txtJobNumber8.Text.Trim();
        strChequeNo8 = txtChequeNo8.Text.Trim();

        strJobNo9 = txtJobNumber9.Text.Trim();
        strChequeNo9 = txtChequeNo9.Text.Trim();

        strJobNo10 = txtJobNumber10.Text.Trim();
        strChequeNo10 = txtChequeNo10.Text.Trim();

        if (strJobNo1 != "" && strChequeNo1 != "")
        {
            int result1 = AccountExpense.AddBankChequeJobNo(strJobNo1, strChequeNo1, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo2 != "" && strChequeNo2 != "")
        {
            int result2 = AccountExpense.AddBankChequeJobNo(strJobNo2, strChequeNo2, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo3 != "" && strChequeNo3 != "")
        {
            int result3 = AccountExpense.AddBankChequeJobNo(strJobNo3, strChequeNo3, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo4 != "" && strChequeNo4 != "")
        {
            int result4 = AccountExpense.AddBankChequeJobNo(strJobNo4, strChequeNo4, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo5 != "" && strChequeNo5 != "")
        {
            int result5 = AccountExpense.AddBankChequeJobNo(strJobNo5, strChequeNo5, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo6 != "" && strChequeNo6 != "")
        {
            int result6 = AccountExpense.AddBankChequeJobNo(strJobNo6, strChequeNo6, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo7 != "" && strChequeNo7 != "")
        {
            int result7 = AccountExpense.AddBankChequeJobNo(strJobNo7, strChequeNo7, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo8 != "" && strChequeNo8 != "")
        {
            int result8 = AccountExpense.AddBankChequeJobNo(strJobNo8, strChequeNo8, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo9 != "" && strChequeNo9 != "")
        {
            int result9 = AccountExpense.AddBankChequeJobNo(strJobNo9, strChequeNo9, dtChequeDate, LoggedInUser.glUserId);
        }
        if (strJobNo10 != "" && strChequeNo10 != "")
        {
            int result10 = AccountExpense.AddBankChequeJobNo(strJobNo10, strChequeNo10, dtChequeDate, LoggedInUser.glUserId);
        }
    }

    protected void txtJobNumber1_TextChanged(object sender, EventArgs e)
    {
       string strJobNo = txtJobNumber1.Text.Trim();

        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo,0);

            if (dsDetail.Tables.Count > 0)
            {
                if (dsDetail.Tables[0].Rows.Count > 0)
                {
                    lblConsignee1.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                    lblCFS1.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
                }
            }
        }
    }

    protected void txtJobNumber2_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber2.Text.Trim();

        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee2.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS2.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber3_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber3.Text.Trim();
        
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee3.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS3.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber4_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber4.Text.Trim();
        
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee4.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS4.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber5_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber5.Text.Trim();
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee5.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS5.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber6_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber6.Text.Trim();
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee6.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS6.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber7_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber7.Text.Trim();
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee7.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS7.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber8_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber8.Text.Trim();
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee8.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS8.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber9_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber9.Text.Trim();
                
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee9.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS9.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }

    protected void txtJobNumber10_TextChanged(object sender, EventArgs e)
    {
        string strJobNo = txtJobNumber10.Text.Trim();
        int JobLenght = strJobNo.Length;
        
        if (JobLenght == 18)
        {
            DataSet dsDetail = AccountExpense.GetJobDetailByRefNo(strJobNo, 0);

            if (dsDetail.Tables[0].Rows.Count > 0)
            {
                lblConsignee10.Text = dsDetail.Tables[0].Rows[0]["Consignee"].ToString();
                lblCFS10.Text = dsDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
        }
    }
}