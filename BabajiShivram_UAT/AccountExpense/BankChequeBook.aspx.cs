using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class AccountExpense_BankChequeBook : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Cheque Book Entry";

            // Fill Bank Name MS

            AccountExpense.FillBankMS(ddBabajiBankName, 1); // Show Only FA Babaji Bank Name

            DBOperations.FillBranch(ddBranch);
        }
    }
    protected void ddBabajiBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int BabajiBankID = Convert.ToInt32(ddBabajiBankName.SelectedValue);

        if (BabajiBankID > 0)
        {
            AccountExpense.FillBankAccountByBankId(ddBabajiBankAccount, BabajiBankID);
        }
        else
        {
            ddBabajiBankAccount.Items.Clear();
        }
    }
    protected void btnSaveCheque_Click(object sender, EventArgs e)
    {
        int BankId = 0, AccountId = 0, StartChequeNo = 0, EndChequeNo = 0;
        int BranchId = 0;

        BankId = Convert.ToInt32(ddBabajiBankName.SelectedValue);
        BranchId = Convert.ToInt32(ddBranch.SelectedValue);

        if (ddBabajiBankAccount.SelectedIndex > 0)
        {
            AccountId = Convert.ToInt32(ddBabajiBankAccount.SelectedValue);
        }

        Int32.TryParse(txtStartChequNo.Text.Trim(), out StartChequeNo);
        Int32.TryParse(txtEndChequNo.Text.Trim(), out EndChequeNo);

        if (txtStartChequNo.Text.Trim().Length < 6 || txtStartChequNo.Text.Trim().Length > 6)
        {
            lblError.Text = "Invalid Start Cheque No!";
            lblError.CssClass = "errorMsg";

            return;
        }
        if (txtEndChequNo.Text.Trim().Length < 6 || txtEndChequNo.Text.Trim().Length > 6)
        {
            lblError.Text = "Invalid End Cheque No!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (BranchId == 0)
        {
            lblError.Text = " Please Select Branch Name!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (BankId  == 0 ||  AccountId == 0)
        {
            lblError.Text = " Please Select Bank Details!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (StartChequeNo == 0 || EndChequeNo == 0 )
        {
            lblError.Text = "Please Enter Six Digit Cheque No!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (StartChequeNo == 0 || EndChequeNo == 0)
        {
            lblError.Text = "Please Enter Six Digit Cheque No!";
            lblError.CssClass = "errorMsg";

            return;
        }
        if (StartChequeNo > EndChequeNo)
        {
            lblError.Text = "Invalid Start Cheque No!";
            lblError.CssClass = "errorMsg";

            return;
        }

        int Result = AccountExpense.AddBankChequeBook(BankId, BranchId, AccountId, StartChequeNo, EndChequeNo, LoggedInUser.glUserId);

        if(Result> 0 )
        {
            lblError.Text = Result.ToString() + " Cheque No Added Successfully!";
            lblError.CssClass = "success";

            ScriptManager.RegisterStartupScript(this, GetType(), "Cheque Success", "alert('" + lblError.Text + "');", true);

        }
        else
        {
            lblError.Text = " Cheque Number Error! No Record Added";
            lblError.CssClass = "errorMsg";

            ScriptManager.RegisterStartupScript(this, GetType(), "Cheque Error", "alert('" + lblError.Text + "');", true);
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Cheque_Detail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    private void ExportFunction(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvChequeDetail.AllowPaging = false;
        gvChequeDetail.AllowSorting = false;
        //gvChequeDetail.Columns[0].Visible = false;

        gvChequeDetail.DataBind();
        gvChequeDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}