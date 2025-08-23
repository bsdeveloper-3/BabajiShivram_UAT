using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;

public partial class FreightOperation_AgentInvoice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingBilling.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle  =   (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   =   "Billing & Back Office";

            //MskValRcvdDate.MinimumValue = DateTime.Now.AddMonths(-4).ToString("dd/MM/yyyyy").ToString();
            //MskValRcvdDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();
            
            MskValBillDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();
            MskValJBDate.MaximumValue   = DateTime.Now.ToString("dd/MM/yyyyy").ToString();
            MskValInvoiceDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();

	        ddCurrency.SelectedValue = "46"; // Rs

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
        }
    }

    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            if (dsBookingDetail.Tables[0].Rows[0]["FRJobNo"] != DBNull.Value)
            {
                lblJobNo.Text   =   dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            }
            if (dsBookingDetail.Tables[0].Rows[0]["FileReceivedDate"] != DBNull.Value)
            {
                txtFileRcvdDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["FileReceivedDate"]).ToString("dd/MM/yyyy");
                txtFileRcvdDate.Enabled = false;
            }
            
            if (dsBookingDetail.Tables[0].Rows[0]["BillNumber"] != DBNull.Value)
                txtBillNumber.Text      =   dsBookingDetail.Tables[0].Rows[0]["BillNumber"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["BillAmount"] != DBNull.Value)
                txtBillAmount.Text = dsBookingDetail.Tables[0].Rows[0]["BillAmount"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["BillDate"] != DBNull.Value)
                txtBillDate.Text        =   Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BillDate"]).ToString("dd/MM/yyyy");
            
            if (dsBookingDetail.Tables[0].Rows[0]["BillDispatchDate"] != DBNull.Value)
                lblDispatchDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["BillDispatchDate"]).ToString("dd/MM/yyyy");

            if (dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"] != DBNull.Value)
                txtBillingRemark.Text = dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"].ToString();

	    // Atleast one agent Invoice required for billing completion

            if (GridViewInvoiceDetail.Rows.Count > 0)
            {
                Label lblJBNumber = (Label)GridViewInvoiceDetail.Rows[0].Cells[1].FindControl("lblJBNumber");

                if (lblJBNumber != null)
                {
                    if (lblJBNumber.Text.Trim() != "")
                    {
                        rdlAgentInvoice.Enabled = true;
                    }
                }
            }

            // Fill Agent Details
            DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));

            if (dsBookingDetail.Tables[0].Rows[0]["AgentCompID"] != DBNull.Value)
            {
                string strAgentID = dsBookingDetail.Tables[0].Rows[0]["AgentCompID"].ToString();

                ddAgent.SelectedValue = strAgentID;

                if (GridViewInvoiceDetail.Rows.Count == 0)
                    ddAgent.Enabled = false;
            }

            /****************** Old Code - Agent Invoice Detail **************************************************
            if (dsBookingDetail.Tables[0].Rows[0]["BillAmount"] != DBNull.Value)
                txtBillAmount.Text  =   dsBookingDetail.Tables[0].Rows[0]["BillAmount"].ToString();
            
            if (dsBookingDetail.Tables[0].Rows[0]["JBNumber"] != DBNull.Value)
                txtJBNumber.Text    =   dsBookingDetail.Tables[0].Rows[0]["JBNumber"].ToString();
            
            if (dsBookingDetail.Tables[0].Rows[0]["JBDate"] != DBNull.Value)
                txtJBDate.Text      =   Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["JBDate"]).ToString("dd/MM/yyyy");

            if (dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceName"] != DBNull.Value)
                txtAgentName.Text   =   dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceName"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceNo"] != DBNull.Value)
                txtInvoiceNo.Text = dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceNo"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceDate"] != DBNull.Value)
                txtInvoiceDate.Text = Convert.ToDateTime(dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceDate"]).ToString("dd/MM/yyyy");

            if (dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceAmount"] != DBNull.Value)
                txtInvoiceAmount.Text = dsBookingDetail.Tables[0].Rows[0]["AgentInvoiceAmount"].ToString();

            if (dsBookingDetail.Tables[0].Rows[0]["InvoiceCurrencyId"] != DBNull.Value)
                ddCurrency.SelectedValue = dsBookingDetail.Tables[0].Rows[0]["InvoiceCurrencyId"].ToString();

            ********************************************************************************************/
        }
    }

    protected void btnSaveBilling_Click(object sender, EventArgs e)
    {
        /***************************************************************************/

        Boolean bBillingStatus      = false;
        Boolean bAgentInvoiceStatus = false;

        int EnqId   =   Convert.ToInt32(Session["EnqId"]);

        string strBillNumber, strRemark;
        
        decimal  decBillAmount = 0;
        DateTime dtFileReceivedDate = DateTime.MinValue, dtBillDate = DateTime.MinValue;
            
        strBillNumber   =   txtBillNumber.Text.Trim();

        strRemark       =   txtBillingRemark.Text.Trim();
                
        bAgentInvoiceStatus     = Convert.ToBoolean(rdlAgentInvoice.SelectedValue);

        if (txtBillAmount.Text.Trim() != "")
        {
            decBillAmount = Convert.ToDecimal(txtBillAmount.Text.Trim());
        }

        if (txtFileRcvdDate.Text.Trim() != "")
        {
            dtFileReceivedDate = Commonfunctions.CDateTime(txtFileRcvdDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter File Received Date!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (txtBillDate.Text.Trim() != "")
        {
            dtBillDate = Commonfunctions.CDateTime(txtBillDate.Text.Trim());
        }

        if (txtBillAmount.Text.Trim() != "" && strBillNumber != "" && dtBillDate != DateTime.MinValue)
        {
            bBillingStatus = true;
        }
        
        int result = DBOperations.AddFreightBilling(EnqId, dtFileReceivedDate, strBillNumber, dtBillDate,decBillAmount,
                strRemark, bBillingStatus, bAgentInvoiceStatus,LoggedInUser.glUserId);

        if (result == 0 && bBillingStatus == false)
        {
            lblError.Text = "Billing Detail Updated Successfully!";
            lblError.CssClass = "success";
        }
        else if (result == 0 && bAgentInvoiceStatus == true)
        {
            string strReturnMessage = "Billing Details Completed!";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='AwaitingBilling.aspx';", true);
        }
        else if (result == 0 && bBillingStatus == true)
        {
            string strReturnMessage = "Billing Detail Updated Successfully! Agent Invoice Pending";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + strReturnMessage + "'); window.location='AwaitingBilling.aspx';", true);
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblError.Text = "Billing Detail Already Updated!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        /*******************************************************/
    }

    protected void btnCancelBilling_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingBilling.aspx");
    }

    #region Agent Invoice

    protected void btnSaveAgentInvoice_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int CurrencyId = 0; int AgentID = 0;
        string strJBNumber="", strAgentName="", strInvoiceNo="", strRemark=""; 

        decimal decInvoiceAmount = 0;
        DateTime dtInvoiceReceivedDate = DateTime.MinValue;
        DateTime dtJBDate = DateTime.MinValue, dtInvoiceDate = DateTime.MinValue;

        strJBNumber     =   txtJBNumber.Text.Trim();
        AgentID         = Convert.ToInt32(ddAgent.SelectedValue);
        strAgentName    = ddAgent.SelectedItem.Text;
        strInvoiceNo    =   txtInvoiceNo.Text.Trim();
        CurrencyId      =   Convert.ToInt32(ddCurrency.SelectedValue);
        strRemark       =   txtInvoiceRemark.Text.Trim();

        if (txtReceivedDate.Text.Trim() != "")
        {
            dtInvoiceReceivedDate = Commonfunctions.CDateTime(txtReceivedDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Invoice Received Date!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (txtInvoiceAmount.Text.Trim() != "")
        {
            decInvoiceAmount = Convert.ToDecimal(txtInvoiceAmount.Text.Trim());
        }

        if (txtJBDate.Text.Trim() != "")
        {
            dtJBDate = Commonfunctions.CDateTime(txtJBDate.Text.Trim());
        }
        if (txtInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }

        if (dtJBDate != DateTime.MinValue && strJBNumber != "")
        {
            if (txtInvoiceAmount.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Invoice Amount!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (txtInvoiceDate.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Invoice Date!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (strAgentName == "")
            {
                lblError.Text = "Please Enter Agent Name!";
                lblError.CssClass = "errorMsg";
                return;
            }
            if (strInvoiceNo == "")
            {
                lblError.Text = "Please Enter Agent Invoice No!";
                lblError.CssClass = "errorMsg";
                return;
            }
            if (CurrencyId == 0)
            {
                lblError.Text = "Please Select Invoice Currency!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        int result = DBOperations.AddFreightAgentInvoice(EnqId, dtInvoiceReceivedDate, strJBNumber, dtJBDate, AgentID, strAgentName, 
            strInvoiceNo, dtInvoiceDate, decInvoiceAmount, CurrencyId, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Agent Invoice Detail Updated Successfully!";
            lblError.CssClass = "success";

	        txtJBNumber.Text 	= "" ;
            txtJBDate.Text 	= "" ;
            //txtAgentName.Text 	= "" ;
            txtInvoiceDate.Text = "" ;
            txtInvoiceAmount.Text = "";
            txtInvoiceRemark.Text = "";

            GridViewInvoiceDetail.DataBind();

	   // Agent Invoice Completed
           
	    rdlAgentInvoice.Enabled = true;
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
        /*******************************************************/
    }

    protected void btnCancelInvoice_Click(object sender, EventArgs e)
    {

    }

    protected void GridViewInvoiceDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int InvoiceID = 0;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        int CurrencyId = 0; decimal decInvoiceAmount = 0;

        string strJBNumber = "", strAgentName = "", strInvoiceNo = "";

        DateTime dtJBDate = DateTime.MinValue, dtInvoiceDate = DateTime.MinValue;

        GridViewRow gvrow = GridViewInvoiceDetail.Rows[e.RowIndex];

        InvoiceID = Convert.ToInt32(GridViewInvoiceDetail.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtJBNumber = (TextBox)gvrow.FindControl("txtJBNumber");
        TextBox txtJBDate = (TextBox)gvrow.FindControl("txtJBDate");
        TextBox txtAgentInvoiceName = (TextBox)gvrow.FindControl("txtAgentInvoiceName");
        TextBox txtAgentInvoiceNo = (TextBox)gvrow.FindControl("txtAgentInvoiceNo");
        TextBox txtInvoiceAmount = (TextBox)gvrow.FindControl("txtInvoicAmount");
        TextBox txtAgentInvoiceDate = (TextBox)gvrow.FindControl("txtAgentInvoiceDate");
        DropDownList ddAgentCurrency = (DropDownList)gvrow.FindControl("ddAgentCurrency");

        strJBNumber = txtJBNumber.Text.Trim();
        strAgentName = txtAgentInvoiceName.Text.Trim();
        strInvoiceNo = txtAgentInvoiceNo.Text.Trim();
        CurrencyId = Convert.ToInt32(ddAgentCurrency.SelectedValue);

        if (strJBNumber == "")
        {
            lblError.Text = "Please Enter JB Number!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (txtJBDate.Text.Trim() != "")
        {
            dtJBDate = Commonfunctions.CDateTime(txtJBDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter JB Date!";
            lblError.CssClass = "errorMsg";

            return;
        }

        if (txtAgentInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtAgentInvoiceDate.Text.Trim());
        }

        if (txtInvoiceAmount.Text.Trim() != "")
        {
            decInvoiceAmount = Convert.ToDecimal(txtInvoiceAmount.Text.Trim());
        }

        if (txtInvoiceAmount.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Invoice Amount!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtAgentInvoiceDate.Text.Trim() == "")
        {
            lblError.Text = "Please Enter Invoice Date!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (strAgentName == "")
        {
            lblError.Text = "Please Enter Agent Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (strInvoiceNo == "")
        {
            lblError.Text = "Please Enter Agent Invoice No!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (CurrencyId == 0)
        {
            lblError.Text = "Please Select Invoice Currency!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int result = DBOperations.UpdateFreightAgentInvoice(InvoiceID, EnqId, strJBNumber, dtJBDate, strAgentName, strInvoiceNo,
                dtInvoiceDate, decInvoiceAmount, CurrencyId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Agent Invoice Detail Updated Successfully!";
            lblError.CssClass = "success";

            e.Cancel = true;

            GridViewInvoiceDetail.EditIndex = -1;
            GridViewInvoiceDetail.DataBind();

        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }
    #endregion
}
