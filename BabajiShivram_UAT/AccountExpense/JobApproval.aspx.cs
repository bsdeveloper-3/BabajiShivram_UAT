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
using System.Text;
using System.Collections.Generic;

public partial class AccountExpense_JobApproval : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["TotalAmount"] = 0;

        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvDocuments);
        ScriptManager1.RegisterPostBackControl(gvJobExpDetail);
        ScriptManager1.RegisterPostBackControl(btnHoldJob);
        ScriptManager1.RegisterPostBackControl(btnRejectJob);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Expense Approval";
        if (!IsPostBack)
        {
            if (gvJobExpDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Fund Request!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceJobExpenseDetails;
        DataFilter1.DataColumns = gvJobExpDetail.Columns;
        DataFilter1.FilterSessionID = "JobApproval.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

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

    #region GridView Event

    protected void gvJobExpDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId").ToString());
                ImageButton imgbtnHoldJob = (ImageButton)e.Row.FindControl("imgbtnHoldJob");
                ImageButton imgRejectJob = (ImageButton)e.Row.FindControl("imgRejectJob");

                if (StatusId == 3) // Hold
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#87AFC7");  //LightSalmon;    
                    e.Row.ToolTip = "On Hold.";
                    imgbtnHoldJob.Visible = false;
                }
                else if (StatusId == 6) // Reject
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#E8ADAA");  //#E77471
                    e.Row.ToolTip = "Rejected fund request.";
                    imgRejectJob.Visible = false;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    e.Row.ToolTip = "Request pending for approval.";
                    imgbtnHoldJob.Visible = true;
                    imgRejectJob.Visible = true;
                }
            }

            // Total Amount Calculation
            Decimal Amount = 0;
            if (e.Row.Cells[11].Text.Trim() != "")
            {
                Amount = Convert.ToDecimal(e.Row.Cells[11].Text.Trim());
            }
            ViewState["TotalAmount"] = Convert.ToDecimal(ViewState["TotalAmount"]) + Convert.ToDecimal(Amount);
            lblTotalAmount.Text = Convert.ToString(ViewState["TotalAmount"]);
        }
    }

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName.ToLower().Trim() == "approvejob")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int ApprovedJobExp = AccountExpense.AddPaymentStatus(lid, 2, "", true, loggedinuser.glUserId);
                if (ApprovedJobExp == 0)
                {
                    gvJobExpDetail.DataBind();
                    lblMessage.Text = "Successfully Approved Fund Request!.";
                    lblMessage.CssClass = "success";
                }
                else if (ApprovedJobExp == 1)
                {
                    lblMessage.Text = "Request Already Approved!";
                    lblMessage.CssClass = "errorMsg";
                    gvJobExpDetail.DataBind();
                }
                else if (ApprovedJobExp == 2)
                {
                    lblMessage.Text = "Record does not exists for Job Number and Expense Type.";
                    lblMessage.CssClass = "errorMsg";
                }
                else
                {
                    lblMessage.Text = "Error while approving fund request details. Please try again later.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }

        else if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                Response.Redirect("ExpPaymentDetails.aspx?id=1");
            }
        }

        else if (e.CommandName.ToLower() == "downloaddoc")
        {
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            gvDocuments.DataBind();
            mpeContractCopy.Show();
        }
        else if (e.CommandName.ToLower() == "hold")
        {
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
            if (dsGetPaymentDetails != null)
            {
                lblPopupName.Text = "Hold Fund Request";
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
            mpeHoldExpense.Show();
        }
        else if (e.CommandName.ToLower() == "cancel")
        {
            hdnRequestFor.Value = "cancel";
            lblPopupName.Text = "Cancel Fund Request";
            btnHoldJob.Text = "Cancel Request";
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
            if (dsGetPaymentDetails != null)
            {
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
            mpeHoldExpense.Show();
        }

        else if (e.CommandName.ToLower() == "reject")
        {
            hdnRequestForReject.Value = "reject";
            lblREjectPopName.Text = "Reject Fund Request";
            btnRejectJob.Text = "Reject Request";
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
            if (dsGetPaymentDetails != null)
            {
                if (dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"] != null)
                    lblJobNumber.Text = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"] != null)
                    lblBranch1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"] != null)
                    lblPaymentType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName1"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"] != null)
                    lblexpenseType1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Amount"] != null)
                    lblAmount1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"] != null)
                    lblPaidTo1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["Remark"] != null)
                    lblRemark1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();

                if (dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"] != null)
                    lblCreatedBy1.Text = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            }
            RejectModalPopupExtender.Show();
        }

        else if (e.CommandName.ToLower() == "details")
        {
            hdnReqForJobDetails.Value = "details";
            lblDetailsPopName.Text = "Job Details/History";
            //btnRejectJob.Text = "Request Details/History";
            string strJobModuleId = e.CommandArgument.ToString();
            string[] words = strJobModuleId.Split(',');
            int JobId = 0;
            decimal ModuleId = 0;

            string strModuleId = words[0].ToString();
            if (strModuleId != "")
            {
                ModuleId = Convert.ToDecimal(strModuleId);
            }

            string strJobId = words[1].ToString();
            if (strJobId != "")
            {
                JobId = Convert.ToInt32(strJobId);
            }

            DataSet dsGetJobDetails = AccountExpense.GetJobDetailforjob(JobId, ModuleId);
            if (dsGetJobDetails != null)
            {
                if (dsGetJobDetails.Tables[0].Rows.Count > 0)
                {
                    grdJobDetails.DataSource = dsGetJobDetails;
                    grdJobDetails.DataBind();
                }
                else
                {
                    dsGetJobDetails.Tables[0].Rows.Add(dsGetJobDetails.Tables[0].NewRow());
                    grdJobDetails.DataSource = dsGetJobDetails;
                    grdJobDetails.DataBind();
                    int columncount = grdJobDetails.Rows[0].Cells.Count;
                    grdJobDetails.Rows[0].Cells.Clear();
                    grdJobDetails.Rows[0].Cells.Add(new TableCell());
                    grdJobDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                    grdJobDetails.Rows[0].Cells[0].Text = "No Details Found!";
                    grdJobDetails.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            ModalPopupDetailHistory.Show();
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

    #endregion

    #region HOLD JOB EXPENSE

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int PaymentId = Convert.ToInt32(Convert.ToString(Session["PaymentId"]));
            if (PaymentId != 0)
            {
                if (hdnRequestFor.Value.ToLower().Trim() == "hold")
                {
                    int ApprovedJobExp = AccountExpense.AddPaymentStatus(PaymentId, 3, txtReason.Text.Trim(), true, loggedinuser.glUserId);
                    if (ApprovedJobExp == 0)
                    {
                        // SEND EMAIL FOR HOLDING THE JOB EXPENSE
                        bool SendMail = SendPreAlertEmail(PaymentId);
                        if (SendMail == true)
                        {
                            lblMessage.Text = "Successfully holded the fund request.";
                            lblMessage.CssClass = "success";
                            txtReason.Text = "";
                            //ResetControls();
                        }
                        else
                        {
                            lblMessage.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                            lblMessage.CssClass = "errorMsg";
                        }

                        gvJobExpDetail.DataBind();
                    }
                    else if (ApprovedJobExp == 2)
                    {
                        lblMessage.Text = "Record does not exists for Job Number and Expense Type.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblMessage.Text = "Error while approving fund request details. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
                else if (hdnRequestFor.Value.ToLower().Trim() == "cancel")
                {
                    int ApprovedJobExp = AccountExpense.AddPaymentStatus(PaymentId, 5, txtReason.Text.Trim(), true, loggedinuser.glUserId);
                    if (ApprovedJobExp == 0)
                    {
                        //SEND EMAIL FOR HOLDING THE JOB EXPENSE
                        //bool SendMail = SendPreAlertEmail(PaymentId);
                        //if (SendMail == true)
                        //{
                        lblMessage.Text = "Successfully cancelled the fund request.";
                        lblMessage.CssClass = "success";
                        txtReason.Text = "";
                        //ResetControls();
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                        //    lblMessage.CssClass = "errorMsg";
                        //}

                        gvJobExpDetail.DataBind();
                    }
                    else if (ApprovedJobExp == 2)
                    {
                        lblMessage.Text = "Record does not exists for Job Number and Expense Type.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblMessage.Text = "Error while approving fund request details. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
        else
        {
            lblError_HoldExp.Text = "Please enter reason for holding this fund request.";
            lblError_HoldExp.CssClass = "errorMsg";
            txtReason.Focus();
            mpeHoldExpense.Show();
        }
    }

    private bool SendPreAlertEmail(int PaymentId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", JobRefno = "", BranchName = "", PaymentTypeName = "",
            ExpenseTypeName = "", Amount = "", PaidTo = "", Remark = "", CreatedBy = "", CreatedByEmail = "", HoldedBy = "", HoldedByEmail = "", HoldReason = "";
        bool bEmailSucces = false;
        StringBuilder strbuilder = new StringBuilder();

        DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
        if (dsGetPaymentDetails != null)
        {
            JobRefno = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();
            BranchName = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();
            PaymentTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName"].ToString();
            ExpenseTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
            Amount = dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();
            PaidTo = dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();
            Remark = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();
            CreatedBy = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
            CreatedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedByEmail"].ToString();
            HoldedBy = dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByName"].ToString();
            HoldedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByEmail"].ToString();
            HoldReason = dsGetPaymentDetails.Tables[0].Rows[0]["HoldReason"].ToString();

            strCustomerEmail = CreatedByEmail;
            strCCEmail = HoldedByEmail; // "";
            strSubject = "Fund Request On Hold - " + JobRefno;

            if (strCustomerEmail == "" || strSubject == "")
                return bEmailSucces;
            else
            {
                MessageBody = "Dear Sir,<BR><BR> Fund Request For Job No -"+ JobRefno +" is On Hold. <BR><BR>";

                strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:90%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%;'>Babaji Job No</th><td style='padding-left: 3px'>" + JobRefno + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%;'>Branch</th><td style='padding-left: 3px'>" + BranchName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Payment Type</th><td style='padding-left: 3px'>" + PaymentTypeName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Request Type</th><td style='padding-left: 3px'>" + ExpenseTypeName + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Amount</th><td style='padding-left: 3px'>" + Amount + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Paid To</th><td style='padding-left: 3px'>" + PaidTo + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Remark</th><td style='padding-left: 3px'>" + Remark + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Created By</th><td style='padding-left: 3px'>" + CreatedBy + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Hold By</th><td style='padding-left: 3px'>" + HoldedBy + "</td></tr>");
                strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; width:40%'>Hold Reason</th><td style='padding-left: 3px'>" + HoldReason + "</td></tr>");
                strbuilder = strbuilder.Append("</table>");

                MessageBody = MessageBody + strbuilder;
                MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + HoldedBy;

                System.Collections.Generic.List<string> lstFilePath = new List<string>();
                bEmailSucces = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                return bEmailSucces;
            }
        }
        else
            return false;
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
            //ViewState["TotalAmount"] = "0";
            DataFilter1.FilterSessionID = "JobApproval.aspx";
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
        ViewState["TotalAmount"] = "0";
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
        gvJobExpDetail.Caption = "Pending fund request approval on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "JobApproval.aspx";
        DataFilter1.FilterDataSource();
        gvJobExpDetail.DataBind();
        gvJobExpDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion


    protected void btnRejectJob_OnClick(object sender, EventArgs e)
    {
        if (txtRejectReason.Text.Trim() != "")
        {
            int PaymentId = Convert.ToInt32(Convert.ToString(Session["PaymentId"]));
            if (PaymentId != 0)
            {
                if (hdnRequestForReject.Value.ToLower().Trim() == "reject")
                {
                    int ApprovedJobExp = AccountExpense.AddPaymentStatus(PaymentId, 6, txtRejectReason.Text.Trim(), true, loggedinuser.glUserId);
                    if (ApprovedJobExp == 0)
                    {
                        //SEND EMAIL FOR HOLDING THE JOB EXPENSE
                        //bool SendMail = SendPreAlertEmail(PaymentId);
                        //if (SendMail == true)
                        //{
                        lblMessage.Text = "Successfully Reject the fund request.";
                        lblMessage.CssClass = "success";
                        txtRejectReason.Text = "";
                        //ResetControls();
                        //}
                        //else
                        //{
                        //    lblMessage.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                        //    lblMessage.CssClass = "errorMsg";
                        //}

                        gvJobExpDetail.DataBind();
                    }
                    else if (ApprovedJobExp == 2)
                    {
                        lblMessage.Text = "Record does not exists for Job Number and Expense Type.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblMessage.Text = "Error while approving fund request details. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
        else
        {
            lblError_RejectExp.Text = "Please enter reason for Reject this fund request.";
            lblError_RejectExp.CssClass = "errorMsg";
            txtRejectReason.Focus();
            RejectModalPopupExtender.Show();
        }
    }

}