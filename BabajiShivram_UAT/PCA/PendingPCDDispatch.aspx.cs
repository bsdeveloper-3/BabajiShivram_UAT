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
using System.Text;

public partial class PendingPCDDispatch : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnSaveHandDelivery);
        ScriptManager1.RegisterPostBackControl(btnSaveCourier);
        
        if (!IsPostBack)
        {
            CalCourierDeliveryDate.EndDate = DateTime.Now;
            CalDispatchDate.EndDate = DateTime.Now;
            CalCourierDispatchDate.EndDate = DateTime.Now;
            Session["CHECKED_ITEMS"] = null; //Checkbox

            Session["JobId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Dispatch";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For PCA Dispatch!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = PCDSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingPCDDispatch.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        RePopulateValues(); //Checkbox

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddTypeOfDelivery = (DropDownList)e.Row.FindControl("ddTypeOfDelivery");

            LinkButton Lnkupdate = (LinkButton)e.Row.FindControl("lnkupdate");

            //if (Lnkupdate.Text == "Update Details")
            //{
            //Lnkupdate.Text = "";
            //}



            if (ddTypeOfDelivery != null)
            {
                if (DataBinder.Eval(e.Row.DataItem, "TypeOfDelivery").ToString() != "0")
                {
                    ddTypeOfDelivery.SelectedValue = DataBinder.Eval(e.Row.DataItem, "TypeOfDelivery").ToString();
                    ddTypeOfDelivery.Enabled = false;
                }
            }

            // Job No link disable if it is an additional job
            if (DataBinder.Eval(e.Row.DataItem, "JobType").ToString().ToLower().Trim() == "additional")
            {
                LinkButton lnkJobNo = (LinkButton)e.Row.FindControl("lnkJobNo");
                if (lnkJobNo != null)
                {
                    lnkJobNo.Enabled = false;
                }
            }
        }
    }

    protected void gvJobDetail_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["JobId"] = e.CommandArgument.ToString();

            Response.Redirect("PCDDispatchDetail.aspx");
        }
        else if (e.CommandName.ToLower() == "updatedispatch")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();
            hdnCustomerPCA.Value = commandArgs[1].ToString();

            if (commandArgs[2].ToString() != "")
                strCustDocFolder = commandArgs[2].ToString() + "\\";

            if (commandArgs[3].ToString() != "")
                strJobFileDir = commandArgs[3].ToString() + "\\";

            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int rowIndex = gvr.RowIndex;

            DropDownList ddTypeOfDelivery = (DropDownList)gvJobDetail.Rows[rowIndex].FindControl("ddTypeOfDelivery");

            GetDispatchDetail(Convert.ToInt32(hdnJobId.Value), Convert.ToBoolean(Convert.ToInt32(hdnCustomerPCA.Value)));

            string strTypeOfDelivery = ddTypeOfDelivery.SelectedValue;

            if (strTypeOfDelivery == "1") // Hand Delivery
            {
                ScriptManager1.RegisterPostBackControl(btnSaveHandDelivery);
                ModalPopupHandDelivery.Show();
            }
            else if (strTypeOfDelivery == "2") // Courier Delivery
            {
                ScriptManager1.RegisterPostBackControl(btnSaveCourier);
                ModalPopupCourier.Show();
            }
            else
            {
                lblMessage.Text = "Please Select Dispatch Type!";
                lblMessage.CssClass = "errorMsg";
            }
        }
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

    //protected void ddDispatchType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList dropDown = (DropDownList)sender;

    //    GridViewRow row = (GridViewRow)dropDown.NamingContainer;
    //    DataKey JobKey = gvJobDetail.DataKeys[row.RowIndex];

    //    string JobId = JobKey["JobId"].ToString();
    //    string DispatchType = JobKey["DispatchType"].ToString();

    //    if (dropDown.SelectedValue == "1")
    //        ModalPopupHandDelivery.Show();
    //    else if (dropDown.SelectedValue == "2")
    //        ModalPopupCourier.Show();
    //}

    private void GetDispatchDetail(int JobId, Boolean PCDCustomer)
    {
        if (JobId == 0)
        {
            Response.Redirect("PendingPCDDispatch.aspx");
        }

        DataSet dsDispatchDetail = DBOperations.GetPCDToDispatchByType(JobId, PCDCustomer);

        // For Reset Fields for Hand Delivery

        pnlHadDeliveryPartTwo.Visible = false;
        txtCarryingPersonName.Enabled = true;
        txtDispatchDate.Enabled = true;
        imgDispatchDate.Visible = true;

        // For Reset Fields for Courier Delivery

        pnlCourierPart2.Visible = false;
        txtCourierName.Enabled = true;
        txtDocketNo.Enabled = true;
        txtCourierDispatchDate.Enabled = true;
        imgCourierDispatchDate.Visible = true;

        if (dsDispatchDetail.Tables[0].Rows.Count > 0)
        {
            if (dsDispatchDetail.Tables[0].Rows[0]["TypeOfDelivery"].ToString() == "1")// Hand Delivery
            {
                txtCarryingPersonName.Text = dsDispatchDetail.Tables[0].Rows[0]["CarryingPerson"].ToString();
                txtReceivedPersonName.Text = dsDispatchDetail.Tables[0].Rows[0]["ReceivedBy"].ToString();

                if (dsDispatchDetail.Tables[0].Rows[0]["DispatchDate"] != DBNull.Value)
                    txtDispatchDate.Text = Convert.ToDateTime(dsDispatchDetail.Tables[0].Rows[0]["DispatchDate"]).ToString("dd/MM/yyyy");

                if (dsDispatchDetail.Tables[0].Rows[0]["PCDDeliveryDate"] != DBNull.Value)
                    txtPCDDeliveryDate.Text = Convert.ToDateTime(dsDispatchDetail.Tables[0].Rows[0]["PCDDeliveryDate"]).ToString("dd/MM/yyyy");

                // Hand Delivery Part Two Visibiliy Check
                if (txtCarryingPersonName.Text.Trim() != "" && txtDispatchDate.Text.Trim() != "")
                {
                    pnlHadDeliveryPartTwo.Visible = true;
                    txtCarryingPersonName.Enabled = false;
                    txtDispatchDate.Enabled = false;
                    imgDispatchDate.Visible = false;
                }
            }
            else if (dsDispatchDetail.Tables[0].Rows[0]["TypeOfDelivery"].ToString() == "2")// Courier Delivery
            {
                txtCourierName.Text = dsDispatchDetail.Tables[0].Rows[0]["CourierName"].ToString();
                txtDocketNo.Text = dsDispatchDetail.Tables[0].Rows[0]["DocketNo"].ToString();
                txtCourierReceivedBy.Text = dsDispatchDetail.Tables[0].Rows[0]["ReceivedBy"].ToString();

                if (dsDispatchDetail.Tables[0].Rows[0]["DispatchDate"] != DBNull.Value)
                    txtCourierDispatchDate.Text = Convert.ToDateTime(dsDispatchDetail.Tables[0].Rows[0]["DispatchDate"]).ToString("dd/MM/yyyy");

                if (dsDispatchDetail.Tables[0].Rows[0]["PCDDeliveryDate"] != DBNull.Value)
                    txtCourierDeliveryDate.Text = Convert.ToDateTime(dsDispatchDetail.Tables[0].Rows[0]["PCDDeliveryDate"]).ToString("dd/MM/yyyy");

                // Courier Part Two Visibiliy Check
                if (txtCourierName.Text.Trim() != "" && txtDocketNo.Text.Trim() != "" && txtCourierDispatchDate.Text.Trim() != "")
                {
                    pnlCourierPart2.Visible = true;
                    txtCourierName.Enabled = false;
                    txtDocketNo.Enabled = false;
                    txtCourierDispatchDate.Enabled = false;
                    imgCourierDispatchDate.Visible = false;
                }
                else
                {

                }
            }
        }
    }

    protected void btnSaveHandDelivery_Click(object sender, EventArgs e)
    {
        string JobId = "";
        int i = 0;

        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        //gvJobDetail.AllowPaging = false;//Checkbox
        //gvJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");
                if (JobId == "")
                {
                    JobId = Recv.CommandArgument + ',';
                }
                else
                {
                    JobId = JobId + Recv.CommandArgument + ',';
                }

                //-------------exising ------------------------------------------

                i++;

            }

            else
            {
                if (i == 0)
                {
                    lblMessage.Text = "Please Checked atleast 1 checkbox.";
                    lblMessage.CssClass = "errorMsg";
                }

            }
        }

        //gvJobDetail.AllowPaging = true;//Checkbox
        //gvJobDetail.DataBind();//Checkbox


        SaveHandDelivery(i, JobId);
    }

    protected void SaveHandDelivery(int Count, string JobId)
    {
        if (JobId == "")
        {
            JobId = hdnJobId.Value + ',';
        }

        //if (Count == 0)
        //{
        //    JobId = hdnJobId.Value;
        //}

        string s = string.Empty;

        //-------------exising ------------------------------------------


        int TypeOfDelivery = 1; // Hand Delivery

        bool PCDCustomer = false; // Dispatch For Billing Dept
        bool bPCDMail = false; // Check for email sending..
        bool bDispatchStatus = false; // If all fields filled up then status completed otherwise pending
        bool isPartTwo = pnlHadDeliveryPartTwo.Visible; // Check If Part2 s ready for update

        //File Upload
        string PODCopyPath = "";
        string strUploadPath = hdnUploadPath.Value;

        if (hdnCustomerPCA.Value == "1") // Dispatch For Customer - PCA
        {
            PCDCustomer = true;
        }

        string strCarryingPerson = "", strDocumentsReceivedBy = "";

        DateTime dtDispatchDate, dtPCDDeliveryDate;
        dtDispatchDate = DateTime.MinValue;
        dtPCDDeliveryDate = DateTime.MinValue;

        strCarryingPerson = txtCarryingPersonName.Text.Trim();
        string strDocketNo = "";// Not Required For Hand Delivery

        if (txtDispatchDate.Text.Trim() != "")
            dtDispatchDate = Commonfunctions.CDateTime(txtDispatchDate.Text.Trim());

        // Part Two Fields
        if (isPartTwo == true) // Update Part Two Fields
        {
            strDocumentsReceivedBy = txtReceivedPersonName.Text.Trim();

            if (txtPCDDeliveryDate.Text.Trim() != "")
                dtPCDDeliveryDate = Commonfunctions.CDateTime(txtPCDDeliveryDate.Text.Trim());

            if (fileUpHandDelivery.FileName.Trim() != "")
            {
                PODCopyPath = UploadPCDDocument(strUploadPath, fileUpHandDelivery);
            }
        }
        // Check if Dispatch PCA OR Dispatch Billing Email is ready to send
        if (strCarryingPerson != "" && txtDispatchDate.Text.Trim() != "" && isPartTwo == false)
        {
            bPCDMail = true;
        }

        // Check Hand Delivery Dispatch Status

        if (strCarryingPerson != "" && txtDispatchDate.Text.Trim() != "" && strDocumentsReceivedBy != "" && txtPCDDeliveryDate.Text.Trim() != "" && PODCopyPath != "")
        {
            bDispatchStatus = true;
        }

        int result = DBOperations.AddPCDToDispatchConsolidated(JobId, strCarryingPerson, strDocketNo, strDocumentsReceivedBy, TypeOfDelivery,
                         dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId);

        //if (Count == 0)
        //{
        //    result = DBOperations.AddPCDToDispatch(Convert.ToInt32(JobId), strCarryingPerson, strDocketNo, strDocumentsReceivedBy, TypeOfDelivery,
        //                 dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId); //Poonam

        //    JobId = JobId.ToString() + ",";
        //}
        //else
        //{
        //    result = DBOperations.AddPCDToDispatchConsolidated(JobId, strCarryingPerson, strDocketNo, strDocumentsReceivedBy, TypeOfDelivery,
        //                 dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId); //Poonam
        //}

        var JobId1 = JobId.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        // To Loop through
        foreach (string items in JobId1)
        {
            if (result == 0)
            {
                lblMessage.Text = "Dispatch Detail Updated Successfully!";
                lblMessage.CssClass = "success";
                gvJobDetail.DataBind();

                bPCDMail = true;

                if (bPCDMail == true) // Send Dispatch Email/SMS To Customer
                {
                    lblMessage.Text += "";//EmailSMSNotification(Convert.ToInt32(items), txtDispatchDate.Text.Trim(), "Hand Delivery", strCarryingPerson, "", PCDCustomer); //Poonam
                }


            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }


        // Clear Job Fields
        hdnJobId.Value = "0";
        txtCarryingPersonName.Text = "";
        txtDispatchDate.Text = "";
        txtReceivedPersonName.Text = "";
        txtPCDDeliveryDate.Text = "";
        gvJobDetail.DataBind();
        Session["CHECKED_ITEMS"] = null; //Checkbox
        strCarryingPerson = "";
        txtDispatchDate.Text = "";
        strDocumentsReceivedBy = "";
        txtPCDDeliveryDate.Text = "";
        PODCopyPath = "";
        pnlHadDeliveryPartTwo.Visible = false;
        ddTypeOfDelivery.SelectedValue = "0";

    }

    protected void btnSaveCourier_Click(object sender, EventArgs e)
    {
        string JobId = "";
        int i = 0;
        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        //gvJobDetail.AllowPaging = false;//Checkbox
        //gvJobDetail.DataBind();//Checkbox
            lblPopMessageCourier.Text = "";
            foreach (GridViewRow gvr in gvJobDetail.Rows)
            {
                if (((CheckBox)gvr.FindControl("chk1")).Checked)
                {
                    LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");
                    if (JobId == "")
                    {
                        JobId = Recv.CommandArgument + ',';
                    }
                    else
                    {
                        JobId = JobId + Recv.CommandArgument + ',';
                    }

                    //-------------exising ------------------------------------------

                    i++;

                }

                else
                {
                    if (i == 0)
                    {
                        lblMessage.Text = "Please Checked atleast 1 checkbox.";
                        lblMessage.CssClass = "errorMsg";
                    }

                }
            }

            //gvJobDetail.AllowPaging = true;//Checkbox
            //gvJobDetail.DataBind();//Checkbox

            SaveCourier(i, JobId);
    }

    protected void SaveCourier(int Count, string JobId)
    {

        if (JobId == "")
        {
            JobId = hdnJobId.Value + ',';
        }

        //if (Count == 0 )
        //{
        //    JobId = hdnJobId.Value;
        //}

        //string JobId = hdnJobId.Value;
        bool bDispatchStatus = false; // If all fields filled up then status completed otherwise pending

        bool isPartTwo = pnlCourierPart2.Visible; // Check If Part2 s ready for update
        bool PCDCustomer = false; // Dispatch For Billing Dept
        bool bPCDMail = false; // Check for email sending..
        int TypeOfDelivery = 2; // Courier Delivery

        string strCourierName = "", strCourierDocketNo = "", strCourierReceivedBy = "";
        string PODCopyPath = "";
        string strUploadPath = hdnUploadPath.Value; //File Upload Path
        DateTime dtDispatchDate, dtPCDDeliveryDate;
        dtDispatchDate = DateTime.MinValue;
        dtPCDDeliveryDate = DateTime.MinValue;

        if (hdnCustomerPCA.Value == "1") // Dispatch For Customer - PCA
        {
            PCDCustomer = true;
        }

        // Part One Fields
        strCourierName = txtCourierName.Text.Trim();
        strCourierDocketNo = txtDocketNo.Text.Trim();

        if (txtCourierDispatchDate.Text.Trim() != "")
            dtDispatchDate = Commonfunctions.CDateTime(txtCourierDispatchDate.Text.Trim());

        // Part Two Fields
        if (isPartTwo == true) // Update Part Two Fields
        {
            strCourierReceivedBy = txtCourierReceivedBy.Text.Trim();
            if (txtCourierDeliveryDate.Text.Trim() != "")
                dtPCDDeliveryDate = Commonfunctions.CDateTime(txtCourierDeliveryDate.Text.Trim());

            if (fileUploadCourier.FileName.Trim() != "")
            {
                PODCopyPath = UploadPCDDocument(strUploadPath, fileUploadCourier);
            }
        }
        // Check if PCD Email is ready to send
        if (strCourierName != "" && strCourierDocketNo != "" && txtCourierDispatchDate.Text.Trim() != "" && isPartTwo == false)
        {
            bPCDMail = true;
        }

        // Check Courier Dispatch Status

        if (strCourierName != "" && strCourierDocketNo != "" && strCourierReceivedBy != "" && txtCourierDeliveryDate.Text.Trim() != "" && PODCopyPath != "")
        {
            bDispatchStatus = true;
        }

        int result = 0;
        result = DBOperations.AddPCDToDispatchConsolidated(JobId, strCourierName, strCourierDocketNo, strCourierReceivedBy, TypeOfDelivery,
                    dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId);

        //if (Count == 0)
        //{
        //    result = DBOperations.AddPCDToDispatch(Convert.ToInt32(JobId), strCourierName, strCourierDocketNo, strCourierReceivedBy, TypeOfDelivery,
        //            dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId);

        //    JobId = JobId.ToString() + ',';
        //}
        //else
        //{
        //    result = DBOperations.AddPCDToDispatchConsolidated(JobId, strCourierName, strCourierDocketNo, strCourierReceivedBy, TypeOfDelivery,
        //            dtPCDDeliveryDate, dtDispatchDate, PCDCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId);
        //}

        var JobId1 = JobId.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        // To Loop through
        foreach (string items in JobId1)
        {
            if (result == 0)
            {
                lblMessage.Text = "Dispatch Detail Updated Successfully!";
                lblMessage.CssClass = "success";

                if (bPCDMail == true) // Send Dispatch Email/SMS To Customer
                {
                    lblMessage.Text += ""; //EmailSMSNotification(Convert.ToInt32(items), txtCourierDispatchDate.Text.Trim(), "Courier", strCourierName, strCourierDocketNo, PCDCustomer);
                }
            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        // Clear Job Fields
        hdnJobId.Value = "0";
        txtCourierName.Text = "";
        txtDocketNo.Text = "";
        txtCourierDispatchDate.Text = "";
        txtCourierReceivedBy.Text = "";
        txtCourierDeliveryDate.Text = "";
        gvJobDetail.DataBind();
        Session["CHECKED_ITEMS"] = null; //Checkbox
        ddTypeOfDelivery.SelectedValue = "0";
    }

    protected void btnCancelHandDelivery_Click(object sender, EventArgs e)
    {
        ModalPopupHandDelivery.Hide();
    }

    protected void btnCancelCourier_Click(object sender, EventArgs e)
    {
        ModalPopupCourier.Hide();
    }

    #region Email/SMS Notification

    private string EmailSMSNotification(int JobId, string strDispatchDate, string strDispatchType, string strPersonName, string strDocketNo, bool IsPCDToCustomer)
    {
        int intMode = Convert.ToInt32(NotificationMode.Email);
        int intType = 0; string strFileName = "";

        if (IsPCDToCustomer == true)
        {
            intType = Convert.ToInt32(NotificationType.PCADispatch);

            // File Name "EmailPCADispatch.txt" For PCADispatch Notification Type

            strFileName = "../EmailTemplate/EmailPCADispatch.txt"; // For PCADispatch
        }
        else
        {
            intType = Convert.ToInt32(NotificationType.BillingDispatch);

            // File Name "EmailBilllingDispatch.txt" For BillingDispatch Notification Type

            strFileName = "../EmailTemplate/EmailBillingDispatch.txt"; // For PCADispatch
        }

        string strJobRefNo = "", strCustomer = "", strCustRefNo = "";
        string strCustomerUserEmail = "", strCustomerUserMobile = "", strSMSText = "";

        string EmailContent = "", strReturnMessage = "", strSubject = "", MessageBody = "", EmailBody = "";
        string strDocList = ""; int DocCount = 1;

        // Get Job Detail Param

        DataSet dsJobDetail = DBOperations.GetJobBasicDetail(JobId);
        strJobRefNo = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
        strCustRefNo = dsJobDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
        strCustomer = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();


        // Get Customer User Email For PCA Billing Dispatch

        DataSet dsCustomerUserEmail = DBOperations.GetCustUserEmailMobileForNotification(JobId, intType, intMode);

        if (dsCustomerUserEmail.Tables.Count > 0)
        {
            foreach (DataRow dtRow in dsCustomerUserEmail.Tables[0].Rows)
            {
                strCustomerUserEmail += dtRow["sEmail"].ToString() + ",";
            }
        }
        // Get Customer User Mobile No For PCA Billing Dispatch

        intMode = Convert.ToInt32(NotificationMode.SMS);

        DataSet dsCustomerUserMobile = DBOperations.GetCustUserEmailMobileForNotification(JobId, intType, intMode);

        if (dsCustomerUserMobile.Tables.Count > 0)
        {
            foreach (DataRow dtRow in dsCustomerUserMobile.Tables[0].Rows)
            {
                strCustomerUserMobile += dtRow["MobileNo"].ToString() + ",";
            }
        }

        if (strCustomerUserEmail != "" || strCustomerUserMobile != "")
        {
            //***********************************
            // START - Email Customer PCD
            //***********************************

            if (strCustomerUserEmail != "")
            {
                // Get PCA Document List

                DataSet dsPCADcoument = DBOperations.FillPCDDocumentByWorkFlow(JobId, Convert.ToInt32(EnumPCDDocType.PCACustomer));


                if (dsPCADcoument.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPCADcoument.Tables[0].Rows)
                    {
                        strDocList += DocCount + ". " + dr["DocumentName"].ToString() + "<BR>";
                        DocCount += 1;

                    }//END_ForEach
                }//END_IF_PCA_Doc_Count

                int lastIndex = strCustomerUserEmail.LastIndexOf(",");

                if (lastIndex > 0)
                { strCustomerUserEmail = strCustomerUserEmail.Remove(lastIndex); }

                try
                {
                    // File Name "EmailPCADispatch.txt" For PCADispatch Notification Type
                    // AND File Name "EmailBilllingDispatch.txt" For BillingDispatch Notification Type
                    //string strFileName = "../EmailTemplate/EmailPCADispatch.txt"; // For PCADispatch

                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    strReturnMessage = "<BR>Dispatch Email Template Error. " + ex.Message;
                }

                MessageBody = EmailContent.Replace("<DispatchDate>", strDispatchDate);
                MessageBody = MessageBody.Replace("<DispatchName>", strDispatchType);
                MessageBody = MessageBody.Replace("<PersonName>", strPersonName);
                MessageBody = MessageBody.Replace("<DocumentList>", strDocList);
                if (strDocketNo != "")
                {
                    MessageBody = MessageBody.Replace("<DocketNo>", "Docket No: " + strDocketNo);
                }
                else
                {
                    MessageBody = MessageBody.Replace("<DocketNo>", "");
                }
                MessageBody = MessageBody.Replace("<EmpName>", LoggedInUser.glEmpName);

                try
                {
                    if (IsPCDToCustomer == true)
                    {
                        strSubject = "Dispatch of Post Clearance Documents / Babaji Job # " + strJobRefNo +
                          " / Customer Name " + strCustomer + " / Customer Ref # " + strCustRefNo;

                        strReturnMessage += "Dispatch Of Post Clearance Documents Email Sent To: " + strCustomerUserEmail;
                    }
                    else
                    {
                        strSubject = "Dispatch of Billing Documents / Babaji Job # " + strJobRefNo +
                          " / Customer Name " + strCustomer + " / Customer Ref # " + strCustRefNo;

                        strReturnMessage += "Billing Dispatch Email Sent To: " + strCustomerUserEmail;
                    }
                    EmailBody = MessageBody;

                    EMail.SendMail(LoggedInUser.glUserName, strCustomerUserEmail, strSubject, EmailBody, "");

                }
                catch (System.Exception ex)
                {
                    strReturnMessage += ex.Message.ToString();
                }
            }//END_IF_Email
            else
            {
                strReturnMessage = "Customer Dispatch Email Notification Not Found!";
            }
            //***********************************
            // END - Email Customer PCD
            //***********************************

            //***********************************
            // START - SMS Customer PCD
            //***********************************
            if (strCustomerUserMobile != "")
            {
                int lastIndexMob = strCustomerUserMobile.LastIndexOf(",");
                strCustomerUserMobile = strCustomerUserMobile.Remove(lastIndexMob);

                if (IsPCDToCustomer == true)
                {
                    strSMSText = "Dispatch of Post Clearance Documents, Your Ref No.: " + strCustRefNo + ", BS Job No.: " + strJobRefNo +
                       ", Dispatch Through.:" + strDispatchType + " Dt. " + strDispatchDate + ", Dispatch Type " + strPersonName;

                    strReturnMessage += "Dispatch of Post Clearance Documents SMS Sent To:" + strCustomerUserMobile;
                }
                else
                {
                    strSMSText = "Dispatch Billing Document, Your Ref No.: " + strCustRefNo + ", BS Job No.: " + strJobRefNo +
                       ", Dispatch Through.:" + strDispatchType + " Dt. " + strDispatchDate + ", Dispatch Type " + strPersonName;

                    strReturnMessage += "Billing Dispatch SMS Sent To:" + strCustomerUserMobile;
                }

                SMS.SendSMS(strSMSText, strCustomerUserMobile);

            }
            else
            {
                strReturnMessage += "SMS Notification Failed!.Mobile Not Found For Dispatch Request Notification.";
            }
            //***********************************
            // END - SMS Customer PCD
            //***********************************

        }//END_IF_Email_OR_SMS
        else
        {
            strReturnMessage = "Dispatch Email/SMS Notification Failed!.<BR> No Email Detail Found For Customer User!";

        }

        // Add Job Email Notificatin History

        if (strCustomerUserEmail != "")
            DBOperations.AddJobNotofication(JobId, 1, intType, strCustomerUserEmail, "", strSubject, EmailBody, strReturnMessage, LoggedInUser.glUserId);

        if (strCustomerUserMobile != "")
            DBOperations.AddJobNotofication(JobId, 2, intType, strCustomerUserMobile, "", "", strSMSText, strReturnMessage, LoggedInUser.glUserId);

        return strReturnMessage;

    }

    #endregion

    #region Documnet Upload/Download/Delete
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        string FileName = fuPCDUpload.FileName.Trim();
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuPCDUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }

    }
    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
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
            DataFilter1.FilterSessionID = "PendingPCDDispatch.aspx";
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
        string strFileName = "PendingPCADispatch_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.Columns[1].Visible = false; // Link Button Job ref No
        gvJobDetail.Columns[2].Visible = false;  // Bound Field Job Ref NO
        gvJobDetail.Columns[3].Visible = true;  // Bound Field Job Ref NO
        //gvJobDetail.Columns[8].Visible = false; // Dropdown list- for Delivery Type Update
        gvJobDetail.Columns[12].Visible = false;  // Hide Delivery Type Drop Down
        gvJobDetail.Columns[13].Visible = true;  // Duplicate Bound filed for Delivery Type Name
        gvJobDetail.Columns[14].Visible = false; // Dispatch Update Button

        gvJobDetail.Caption = "Pending PCA Dispatch On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingPCDDispatch.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //gvJobDetail.DataSourceID = "ShipmentClearedSqlDataSource";
        //gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Consolidated Update

    protected void Updatedetails_Click(object sender, EventArgs e)
    {
        txtCarryingPersonName.Enabled = true;
        txtDispatchDate.Enabled = true;

        int i = 0;
        string Customer = "";
        string Document = "";
        string Details = "";
        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox

        //gvJobDetail.AllowPaging = false;//Checkbox
        //gvJobDetail.DataBind();//Checkbox


        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                if (i > 0)
                {
                    if (Customer.ToString() != gvJobDetail.Rows[gvr.RowIndex].Cells[6].Text)
                    {
                        i = -1;
                        lblMessage.Text = "Please Checked Same Customer.";
                        lblMessage.CssClass = "errorMsg";
                        break;
                    }

                    if (Document.ToString() != gvJobDetail.Rows[gvr.RowIndex].Cells[9].Text)
                    {
                        i = -1;
                        lblMessage.Text = "Please Select Same Document Received From.";
                        lblMessage.CssClass = "errorMsg";
                        break;
                    }

                    if (Details.ToString() != gvJobDetail.Rows[gvr.RowIndex].Cells[12].Text)
                    {
                        i = -1;
                        lblMessage.Text = "Please Select Same Details.";
                        lblMessage.CssClass = "errorMsg";
                        break;
                    }

                }

                Customer = gvJobDetail.Rows[gvr.RowIndex].Cells[6].Text;
                Document = gvJobDetail.Rows[gvr.RowIndex].Cells[9].Text;

                Details = gvJobDetail.Rows[gvr.RowIndex].Cells[12].Text;

                if (Document == "")
                {
                    hdnCustomerPCA.Value = "";
                }
                else if (Document == "PCA")
                {
                    hdnCustomerPCA.Value = "1";
                }
                else
                {
                    hdnCustomerPCA.Value = "0";
                }

                i++;



            }

            else
            {
                if (i == 0)
                {
                    lblMessage.Text = "Please Checked atleast 1 checkbox.";
                    lblMessage.CssClass = "errorMsg";
                }

            }



        }

        //gvJobDetail.AllowPaging = true;//Checkbox
        //gvJobDetail.DataBind();//Checkbox

        if (i > 0)
        {

            if (Details == "Update Delivered Details")
            {
                lblMessage.Text = "Already Data Updated";
                lblMessage.CssClass = "errorMsg";
                return;
            }
            else
            {
                lblMessage.Text = "";
                ModalPopupExtenderdeliverytype.Show();
            }
        }

    }

    protected void BtnSavedeliveryType_Click(object sender, EventArgs e)
    {

        string strTypeOfDelivery = ddTypeOfDelivery.SelectedValue;

        if (strTypeOfDelivery == "1") // Hand Delivery
        {
            ScriptManager1.RegisterPostBackControl(btnSaveHandDelivery);
            ModalPopupHandDelivery.Show();
        }
        else if (strTypeOfDelivery == "2") // Courier Delivery
        {
            ScriptManager1.RegisterPostBackControl(btnSaveCourier);
            ModalPopupCourier.Show();
        }
        else
        {
            lblMessage.Text = "Please Select Dispatch Type!";
            lblMessage.CssClass = "errorMsg";
        }

    }

    protected void gvJobDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RememberOldValues();
        gvJobDetail.PageIndex = e.NewPageIndex;
        RePopulateValues();


    } //Checkbox

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        string Document = "";
        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            index = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value);

            bool result = ((CheckBox)row.FindControl("chk1")).Checked;
            Document = gvJobDetail.Rows[row.RowIndex].Cells[9].Text;

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                //bool result = ((CheckBox)row.FindControl("chk1")).Checked;

                if (!categoryIDList.Contains(index))
                    categoryIDList.Add(index);
                categoryIDList.Add(Document);
            }
            else if (categoryIDList.Contains(Document) && categoryIDList.Contains(index))
            {
                categoryIDList.Remove(Document);
                categoryIDList.Remove(index);
                countRow = countRow + 1;
            }

            // }
        }
        if (categoryIDList != null && categoryIDList.Count > 0)
            Session["CHECKED_ITEMS"] = categoryIDList;


        int countRow1 = countRow;

    } //Checkbox

    private void RePopulateValues()
    {
        ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];

        if (categoryIDList != null && categoryIDList.Count > 0)
        {
            foreach (GridViewRow row in gvJobDetail.Rows)
            {
                int index = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value);
                string Document = gvJobDetail.Rows[row.RowIndex].Cells[9].Text;
                bool result = ((CheckBox)row.FindControl("chk1")).Checked;
                if (categoryIDList.Contains(index) && categoryIDList.Contains(Document))
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = true;
                }
                else
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = false;

                }
            }
        }
        //gvNonRecievedJobDetail.AllowPaging = true;
    }//Checkbox

    #endregion

}
