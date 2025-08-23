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
using System.IO;
using System.Net;

public partial class AccountExpense_JobExpPayment : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvJobExpDetail);
        ScriptManager1.RegisterPostBackControl(btnPayProcess);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Expense Payment";
        if (!IsPostBack)
        {
            if (gvJobExpDetail.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Job Expense!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "";
            }
        }

        DataFilter1.DataSource = DataSourceJobExpenseDetails;
        DataFilter1.DataColumns = gvJobExpDetail.Columns;
        DataFilter1.FilterSessionID = "JobExpPayment.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            int lid = 0, JobId = 0;// Convert.ToInt32(e.CommandArgument.ToString());
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            lid = Convert.ToInt32(commandArgs[0]);
            JobId = Convert.ToInt32(commandArgs[1]);

            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                Session["JobId"] = JobId.ToString();
                Response.Redirect("ExpPaymentDetails.aspx");
                //gvJobExpDetail             
            }
        }
        else if (e.CommandName.ToLower() == "downloaddoc")
        {
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            gvDocuments.DataBind();
            mpeContractCopy.Show();
        }
        else if (e.CommandName.ToLower() == "cancel")
        {
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
            if (dsGetPaymentDetails != null)
            {
                lblPopupName.Text = "Cancel Fund Request";
                hdnRequestFor.Value = "hold";
                if (dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"] != null)
                    lblBSJobNo.Text = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName1"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblExpenseType.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Amount"] != null)
                    lblAmount.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"] != null)
                    lblPaidTo.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Remark"] != null)
                    lblRemark.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }
            mpeCancelExpense.Show();
        }      
    }
    protected void gvJobExpDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvJobExpDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Status") != DBNull.Value)
            {
                string Status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                LinkButton lnkJobRefNo = (LinkButton)e.Row.FindControl("lnkJobRefNo");
                Label lblIsPaymentChk = (Label)e.Row.FindControl("lblIsPaymentChk");
                CheckBox chkIsPayProcess = (CheckBox)e.Row.FindControl("chkIsProcess");

                if (lblIsPaymentChk != null && lblIsPaymentChk.Text.Trim() != "")
                {
                    chkIsPayProcess.Visible = false;
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    chkIsPayProcess.Visible = true;
                }

               if (lnkJobRefNo != null)
                {
                    if (Status.ToLower().Trim() == "hold") //hold request
                    {
                        lnkJobRefNo.Enabled = false;
                    }
                    else
                    {
                        lnkJobRefNo.Enabled = true;
                    }
                }
            }
        }
    }
    protected void btnCancelJob_OnClick(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int PaymentId = Convert.ToInt32(Convert.ToString(Session["PaymentId"]));
            if (PaymentId != 0)
            {
                int ApprovedJobExp = AccountExpense.AddPaymentStatus(PaymentId, 5, txtReason.Text.Trim(), true, loggedInUser.glUserId);
                if (ApprovedJobExp == 0)
                {                  
                    lblError.Text = "Successfully cancelled the fund request.";
                    lblError.CssClass = "success";
                    txtReason.Text = "";                  
                    gvJobExpDetail.DataBind();
                }
                else if (ApprovedJobExp == 2)
                {
                    lblError.Text = "Record does not exists for Job Number and Expense Type.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while approving fund request details. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblError_CancelExp.Text = "Please Enter Reason for Cancelling This Fund Request.";
            lblError_CancelExp.CssClass = "errorMsg";
            txtReason.Focus();
            mpeCancelExpense.Show();
        }
    }

    #endregion

    #region DOCUMENT GRIDVIEW EVENT

    protected void gvDocuments_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    protected void imgbtnDoc_Click(object sender, EventArgs e)
    {
        mpeContractCopy.Hide();
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "JobExpPayment.aspx";
            DataFilter1.FilterDataSource();
            gvJobExpDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "JobExpense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvJobExpDetail.AllowPaging = false;
        gvJobExpDetail.AllowSorting = false;
        gvJobExpDetail.Columns[1].Visible = false;
        gvJobExpDetail.Columns[2].Visible = true;
        gvJobExpDetail.Caption = "Pending Job Expense Payment On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "JobExpPayment.aspx";
        DataFilter1.FilterDataSource();
        gvJobExpDetail.DataBind();
        gvJobExpDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion   
    protected void btnPayProcess_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item in gvJobExpDetail.Rows)
        {
            CheckBox chk = (item.FindControl("chkIsProcess") as CheckBox);
            if (chk.Checked)
            {
                Label lblPaymentId = (Label)item.FindControl("lblPaymentId");
                if (lblPaymentId.Text.Trim() != "")
                {
                    int PaymentId = Convert.ToInt32(lblPaymentId.Text.Trim());
                    int result = AccountExpense.UpdIsPayProcess(PaymentId, loggedInUser.glUserId);
                }
            }
        }
    }
}