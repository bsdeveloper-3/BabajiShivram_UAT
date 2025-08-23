using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BankAPI.Open;
public partial class AccountExpense_BankStatement : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Bank Statement";

        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yy");

            AccountExpense.FillAPIBankAccount(ddAccount);

            ddAccount.Items[0].Text = "--Account No--";

        }

        int result = LoggedInUser.ValidateModulePage(LoggedInUser.glUserId, LoggedInUser.glModuleId, LoggedInUser.glRoleId);

        if (result == 1)
        {
            Response.Redirect("../Login.aspx");
            lblMessage.Text = "Invalid Access";
            Session.Clear();
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        if (ddAccount.SelectedIndex > 0)
        {
            GetStatement();
        }
        else
        {
            lblMessage.Text = "Please Select Account No for Bank Statement!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    private void GetStatement()
    {
        if (ddAccount.SelectedIndex == 0)
        {
            return;    
        }
        
        string TimeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
        int BankAccountID = Convert.ToInt32(ddAccount.SelectedValue);
        string BankAccountNo = "";
        string BankCustomerID = "";
        
        // Get Bank Customer ID against selected Account NO
        DataSet dsBankDetail =  AccountExpense.GetAPIDetailByAccountID(BankAccountID);

        if(dsBankDetail.Tables.Count >0)
        {
            if(dsBankDetail.Tables[0].Rows.Count > 0)
            {
                BankCustomerID  = dsBankDetail.Tables[0].Rows[0]["CustomerID"].ToString();
                BankAccountNo   = dsBankDetail.Tables[0].Rows[0]["AccountNo"].ToString();
            }
        }

        BankAPIReqStatement.Root objRoot = new BankAPIReqStatement.Root();
        BankAPIReqStatement.AdhocStatementReq objAdhocStatementReq = new BankAPIReqStatement.AdhocStatementReq();

        BankAPIReqStatement.ReqHdr objReqHdr = new BankAPIReqStatement.ReqHdr();
        BankAPIReqStatement.ReqBody objReqBody = new BankAPIReqStatement.ReqBody();

        BankAPIReqStatement.ConsumerContext objConsumerContext = new BankAPIReqStatement.ConsumerContext();
        BankAPIReqStatement.ServiceContext objServiceContext = new BankAPIReqStatement.ServiceContext();

        objRoot.AdhocStatementReq = objAdhocStatementReq;

        objReqHdr.ConsumerContext = objConsumerContext;
        objReqHdr.ServiceContext = objServiceContext;

        objAdhocStatementReq.ReqBody = objReqBody;
        objAdhocStatementReq.ReqHdr = objReqHdr;

        objConsumerContext.RequesterID = "APP";

        objServiceContext.ServiceName = "AdhocStatement";
        objServiceContext.ReqRefNum = DateTime.Now.ToLongDateString();
        objServiceContext.ReqRefTimeStamp = TimeStamp;
        objServiceContext.ServiceVersionNo = "1.0";

        objReqBody.customer_id = BankCustomerID;
        objReqBody.cod_acct_no = BankAccountNo;
        objReqBody.txn_start_date = txtDateFrom.Text.Replace("/","-");
        objReqBody.txn_end_date = txtDateTo.Text.Replace("/", "-"); ;

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        var jsonRequest = serializer.Serialize(objRoot);

        BankRespStatement.Root objStatement = new BankRespStatement.Root();

        string strMessge = BankAPIMethods.LiveBankStatement(jsonRequest, ref objStatement);

        if (objStatement.AdhocStatementRes != null)
        {
            gvStatement.DataSource = objStatement.AdhocStatementRes.ResBody.statement;
            gvStatement.DataBind();
        }
        else
        {
            // Error

            lblMessage.Text = "System Error! Please try after sometime.";
            lblMessage.CssClass = "errorMsg";

            ErrorLog.LogToDatabase(0, "GetStatement Function", "Bank Statement", strMessge, null, BankAccountNo, LoggedInUser.glUserId);
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "Statement_"+ddAccount.SelectedItem.Text+"_" + txtDateFrom.Text + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        GetStatement();

        if (gvStatement.Rows.Count < 1)
        {
             lblMessage.Text = "No Record Found!";
             lblMessage.CssClass = "errorMsg";
        }
        else
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvStatement.AllowPaging = false;
            gvStatement.AllowSorting = false;

            gvStatement.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion

    protected void btnSearchCheque_Click(object sender, EventArgs e)
    {
        ddAccount.SelectedIndex = 0;
        gvStatement.DataSource = null;
        gvStatement.DataBind();

        int ChequeNo = 0;

        Int32.TryParse(txtChequeNo.Text.Trim(), out ChequeNo);

        if (ChequeNo > 0)
        {
            
            string strChequeNo = txtChequeNo.Text.Trim();

            DataSet dsChequeDetail = AccountExpense.GetAPIBankStatementByChequeNo(ChequeNo);

            if (dsChequeDetail.Tables[0].Rows.Count > 0)
            {
                gvStatement.DataSource = dsChequeDetail;
                gvStatement.DataBind();
            }
            else
            {
                // Error

                lblMessage.Text = "Cheque Details Not Found!. Plase check Bank Statmenet";
                lblMessage.CssClass = "errorMsg";
            }

        }
        else
        {
            lblMessage.Text = "Please Enter UTR/Cheque No!";
            lblMessage.CssClass = "errorMsg";
        }
    }
}