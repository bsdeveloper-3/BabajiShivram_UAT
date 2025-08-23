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

public partial class ExportCHA_PendingSBFiling : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "SB Filing";
        if (!IsPostBack)
        {
            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Filing!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = PendingNotingSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingSBFiling.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Priority") != DBNull.Value)
            {
                // Change row color based on job priority

                int prioroty = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority"));
                if (prioroty == (int)JobPriority.High)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "High Job Priority";
                }
                else if (prioroty == (int)JobPriority.Intense)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#85f7f7");
                    e.Row.ToolTip = "Intense Job Priority";
                }
            }
        }
    }
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = gvJobDetail.DataKeys[gvrow.RowIndex].Value.ToString();
        }

        if (e.CommandName.ToLower() == "cancel")
            gvJobDetail.EditIndex = -1;
    }
    protected void gvJobDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvJobDetail.EditIndex = e.NewEditIndex;
    }
    protected void gvJobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int JobId = Convert.ToInt32(gvJobDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtSBNo = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtSBNo");
        TextBox txtSBDate = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtSBDate");
        DropDownList ddMarkAppraising = (DropDownList)gvJobDetail.Rows[e.RowIndex].FindControl("ddMarkAppraising");
        DateTime dtSBDate = DateTime.MinValue;
        if (txtSBDate.Text.Trim().ToString() != "")
            dtSBDate = Commonfunctions.CDateTime(txtSBDate.Text.Trim());

        if (ddMarkAppraising.SelectedValue != "0")
        {
            int result = EXOperations.EX_AddShippingBillDetail(JobId, txtSBNo.Text.Trim(), dtSBDate, Convert.ToInt32(ddMarkAppraising.SelectedValue), loggedinuser.glUserId);

            if (result == 0)
            {
                DataSet dsJobDetail = EXOperations.EX_GetJobDetail(JobId);
                if (dsJobDetail != null && dsJobDetail.Tables[0].Rows.Count > 0)
                {
                    int ExportType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["ExportTypeId"].ToString());
                    if(ExportType == 4)
                    {
                        lblMessage.Text = "SB Filing Detail Added Successfully. Job Moved To Form 13.";
                        lblMessage.CssClass = "success";
                    }
                    else
                    {
                        lblMessage.Text = "SB Filing Detail Added Successfully. Job Moved To Custom Process.";
                        lblMessage.CssClass = "success";
                    }
                }      
                gvJobDetail.EditIndex = -1;
                e.Cancel = true;
            }
            else
            {
                lblMessage.Text = "System Error! Please Try After Sometime.";
                lblMessage.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }//END_IF
        else
        {
            lblMessage.CssClass = "errorMsg";
            lblMessage.Text = " Please Select Marked/Appraising.";
        }
    }
    protected void gvJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvJobDetail.EditIndex = -1;
    }
    protected void gvJobDetail_PreRender(object sender, EventArgs e)
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
    private string EmailSMSNotification(int JobId, string strDutyAmount, string JobRefNo, string CustomerName, string CustRefNo, string strBOENo, string strBOEDate)
    {
        string EmailContent = "", strReturnMessage = "";
        string EmailBody = "", MessageBody = "", strSubject = "", strSMSText = "";
        string strCustomerUserEmail = "", strCustomerUserMobile = "";

        int intNotificationType = (int)NotificationType.DutyRequest;
        int intNotificationMode = (int)NotificationMode.Email;

        // Get Customer User Email For Duty Request Notification

        DataSet dsCustomerUserEmail = DBOperations.GetCustUserEmailMobileForNotification(JobId, intNotificationType, intNotificationMode);

        if (dsCustomerUserEmail.Tables.Count > 0)
        {
            foreach (DataRow dtRow in dsCustomerUserEmail.Tables[0].Rows)
            {
                strCustomerUserEmail += dtRow["sEmail"].ToString() + ",";
            }
        }
        // Get Customer User Mobile No For Duty Request Notification

        intNotificationMode = (int)NotificationMode.SMS;

        DataSet dsCustomerUserMobile = DBOperations.GetCustUserEmailMobileForNotification(JobId, intNotificationType, intNotificationMode);

        if (dsCustomerUserMobile.Tables.Count > 0)
        {
            foreach (DataRow dtRow in dsCustomerUserMobile.Tables[0].Rows)
            {
                strCustomerUserMobile += dtRow["MobileNo"].ToString() + ",";
            }
        }
        if (strCustomerUserEmail != "" || strCustomerUserMobile != "")
        {
            //************************************************
            // Customer Duty Email Notification
            //************************************************

            if (strCustomerUserEmail != "")
            {
                //int lastIndex = strCustomerUserEmail.LastIndexOf(",");
                //strCustomerUserEmail = strCustomerUserEmail.Remove(lastIndex);

                strCustomerUserEmail += loggedinuser.glUserName; // Email Copy To Noting User

                try
                {
                    string strFileName = "EmailTemplate/EmailDutyRequest.txt";

                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message;
                }

                MessageBody = EmailContent.Replace("<DutyAmount>", strDutyAmount);
                MessageBody = MessageBody.Replace("<EmpName>", loggedinuser.glEmpName);

                try
                {
                    strSubject = "Custom Duty Request / Babaji Job # " + JobRefNo +
                        " / Customer Name " + CustomerName + " / Customer Ref # " + CustRefNo;

                    EmailBody = MessageBody;

                    EMail.SendMail(loggedinuser.glUserName, strCustomerUserEmail, strSubject, EmailBody, "");

                    strReturnMessage = "Duty Request Email Sent To:" + strCustomerUserEmail;
                }
                catch (System.Exception ex)
                {
                    strReturnMessage = ex.Message.ToString();
                }
            }//END_IF_Email
            else
            {
                strReturnMessage = "Customer Email Not Found For Duty Request Notification.";
            }

            //***********************************
            // END - Email Customer Duty
            //***********************************

            //***********************************
            // START - SMS Customer Duty
            //***********************************
            if (strCustomerUserMobile != "")
            {
                int lastIndexMob = strCustomerUserMobile.LastIndexOf(",");
                strCustomerUserMobile = strCustomerUserMobile.Remove(lastIndexMob);

                strSMSText = "DUTY REQUEST, Your Ref No.: " + CustRefNo + ", BS Job No.: " + JobRefNo +
                   ", BOE No.:" + strBOENo + " Dt. " + strBOEDate + ", Duty Amount " + strDutyAmount;

                bool bSuccess = SMS.SendSMS(strSMSText, strCustomerUserMobile);

                if (bSuccess == true)
                    strReturnMessage += "Customer Duty Request SMS Sent To:" + strCustomerUserMobile;
                else
                {
                    strReturnMessage += "Customer Duty Request SMS Sending System Error! SMS Not Sent To:" + strCustomerUserMobile;
                }
            }
            else
            {
                strReturnMessage += "Customer Mobile Not Found For Duty Request Notification.";
            }
            //***********************************
            // END - SMS Customer Duty
            //***********************************

        }
        else
        {
            strReturnMessage = "Customer Email/Mobile Not Found For Duty Request Notification.";

        }

        // Add Job Notificatin History
        // DBOperations.AddJobNotofication(JobId, 1, intNotificationType, strCustomerUserEmail, strSubject, EmailBody, strReturnMessage, loggedinuser.glUserId);
        // DBOperations.AddJobNotofication(JobId, 2, intNotificationType, strCustomerUserMobile, "", strSMSText, strReturnMessage, loggedinuser.glUserId);

        return strReturnMessage;

        //************************************************
        // END - Customer Duty Request Email/SMS
        //************************************************

    }
    private string NotingSMS(int JobId, string JobRefNo, bool IsNonRMS, string strConsignee,
            string strBOENo, string strBoeDate, string PriorityId, string strCustomsGroup)
    {
        string straBabajiUserMobile = "", strReturnMessage = "", strSMSText2 = "";

        DataSet dsBabajiUserMobile;
        // If BOE Tpe Is Non-RMS - Send SMS To Customs Group User
        if (IsNonRMS == true)
        {
            dsBabajiUserMobile = DBOperations.GetCustomsGroupUserMobile(JobId);

            if (dsBabajiUserMobile.Tables.Count > 0)
            {
                foreach (DataRow dtRow in dsBabajiUserMobile.Tables[0].Rows)
                {
                    straBabajiUserMobile += dtRow["GroupUserMobile"].ToString() + ",";
                }
            }
        }

        //***********************************
        // START - SMS Babaji User - Non-RMS Jobs
        //***********************************
        if (straBabajiUserMobile != "")
        {
            int lastIndexMob2 = straBabajiUserMobile.LastIndexOf(",");
            straBabajiUserMobile = straBabajiUserMobile.Remove(lastIndexMob2);

            if (PriorityId == JobPriority.High.ToString())
            {
                strSMSText2 = "NOTING DTLS, Priority: High, BS Job No.: " + JobRefNo + ", Consignee Name:" + strConsignee
                   + ", BOE No.: " + strBOENo + " Dt. " + strBoeDate + ", Customs Group:" + strCustomsGroup;
            }
            else
            {
                strSMSText2 = "NOTING DTLS, BS Job No.: " + JobRefNo + ", Consignee Name:" + strConsignee
                    + ", BOE No.: " + strBOENo + " Dt. " + strBoeDate + ", Customs Group:" + strCustomsGroup;
            }

            bool bSuccess = SMS.SendSMS(strSMSText2, straBabajiUserMobile);

            if (bSuccess == true)
                strReturnMessage += "Customs Group Noting SMS Sent To : " + straBabajiUserMobile;
            else
                strReturnMessage += "Customs Group Noting SMS Sending Failed To : " + straBabajiUserMobile;
        }
        else
        {
            // strReturnMessage += "Customs Group User Not Found For Noting SMS Notification.<BR>";
        }

        // **** Add To Job Notification History *****/
        int Mode = (Int32)NotificationMode.SMS;
        int lType = (Int32)NotificationType.Noting;

        // DBOperations.AddJobNotofication(JobId, Mode, lType, straBabajiUserMobile, "", strSMSText2, strReturnMessage, loggedinuser.glUserId);
        //***********************************
        // END - SMS Babaji User - Non-RMS Jobs
        //***********************************

        // ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('" + strReturnMessage + "');</script>", false);

        return strReturnMessage;

    }
    private string NotingEmail(int JobId, string JobRefNo)
    {
        // Send Noting Email For JNPT Port
        string strReturnMessage = "";
        string strSubject = "BE Filing Details- " + JobRefNo;
        // **** Add To Job Notification History *****/

        int Mode = (Int32)NotificationMode.Email;
        int lType = (Int32)NotificationType.Noting;

        int result = DBOperations.SendNotingEmail(JobId);

        if (result == 0)
            strReturnMessage = "Success";
        else if (result == 1)
            strReturnMessage = "Error";
        else if (result == 2)
            strReturnMessage = "Noting Email Inactive";
        else if (result == 3)
            strReturnMessage = "System Email Configuration Inactive";
        else
            strReturnMessage = "System Error In Sending Noting Email To User";

        DBOperations.AddJobNotofication(JobId, Mode, lType, "", "", "", strSubject, strReturnMessage, loggedinuser.glUserId);

        //  ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('" + strReturnMessage + "');</script>", false);

        return strReturnMessage;

    }

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
            DataFilter1.FilterSessionID = "PendingSBFiling.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
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
        string strFileName = "PendingSBFiling_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Caption = "Pending SB Filing On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingSBFiling.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}