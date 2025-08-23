using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class FreightOperation_BillingAdvice : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveAdvice);
        ScriptManager1.RegisterPostBackControl(rdlAgentInvoice);
        ScriptManager1.RegisterPostBackControl(rdlSentToBilling);
        ScriptManager1.RegisterPostBackControl(gvFreightDocument);
        ScriptManager1.RegisterPostBackControl(btnCancelAdvice);
        ScriptManager1.RegisterPostBackControl(GridViewInvoiceDetail);
        //ScriptManager1.RegisterPostBackControl(rptDocument.FindControl("fuDocument"));

        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingAdvice.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Advice";

            //MskValInvoiceDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();
            //MskValRcvdDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();

            //ddCurrency.SelectedValue = "46"; // Rs

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            divDocument.Visible = false;
            dvAgentInvoice.Visible = false;

            rdlAgentInvoice.SelectedIndex = 0;
            rdlSentToBilling.SelectedIndex = 0;

            FillAgentInvoice();
            CheckFinalInvoicePending(Convert.ToInt32(Session["EnqId"]));
        }
        else
        {

        }
    }

    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);
        string JobType = "", TransMode = "", strAirLine = "", HoldStatus="";

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            if (dsBookingDetail.Tables[0].Rows[0]["FRJobNo"] != DBNull.Value)
            {
                lblJobNo.Text = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            }

            JobType = dsBookingDetail.Tables[0].Rows[0]["lType"].ToString();
            TransMode = dsBookingDetail.Tables[0].Rows[0]["lMode"].ToString();
            strAirLine = dsBookingDetail.Tables[0].Rows[0]["AirLineId"].ToString();

            if (JobType == "2")
            {
                Session["JobType"] = "Export";
                if (TransMode == "1")
                {
                    Session["JobMode"] = "Air";
                    if (strAirLine != "")
                    {
                        Agentfield.Visible = false;
                    }
                    else
                    {
                        Agentfield.Visible = true;
                    }
                }
                else
                {
                    Session["JobMode"] = "Sea";
                    Agentfield.Visible = true;
                }
            }
            else
            {
                Session["JobType"] = "Import";
            }

            HoldStatus = dsBookingDetail.Tables[0].Rows[0]["HoldStatus"].ToString();
            //lblError.Text = HoldStatus;
            if ( HoldStatus == "False"|| HoldStatus=="0" || HoldStatus=="")
            {
                btnSaveAdvice.Visible = true;
            }
            else
            {
                btnSaveAdvice.Visible = false;
            }

            if (dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"] != DBNull.Value)
                txtBillingRemark.Text = dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"].ToString();

            // Atleast one agent Invoice required for billing completion

            if (GridViewInvoiceDetail.Rows.Count > 0)
            {
                rdlAgentInvoice.Enabled = true;
            }

            DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));

            if (dsBookingDetail.Tables[0].Rows[0]["AgentCompID"] != DBNull.Value)
            {
                string strAgentID = dsBookingDetail.Tables[0].Rows[0]["AgentCompID"].ToString();

                ddAgent.SelectedValue = strAgentID;

                if (GridViewInvoiceDetail.Rows.Count == 0)
                    ddAgent.Enabled = false;
            }
        }
    }
    private bool CheckFinalInvoicePending(int JobId)
    {
        bool isPending = false;

        // Check if any Vendor Final Invoice Pending for Submission against Proforma Invoice 

        DataSet dsPending = AccountExpense.CheckFinalInvoicePending("", JobId, 2);

        if (dsPending.Tables.Count > 0)
        {
            if (dsPending.Tables[0].Rows.Count > 0)
            {
                isPending = true;

                gvProformaDetail.DataSource = dsPending;
                gvProformaDetail.DataBind();

                ModalPopupProforma.Show();
            }
        }
        return isPending;

    }
    private bool CheckVendorInvoicePending(int JobId)
    {
        bool isPending = false;

        // Check if any Vendor Invoice Pending for Submission 

        DataSet dsPending = AccountExpense.CheckVendorInvoicePending(JobId, 2);

        if (dsPending.Tables.Count > 0)
        {
            if (dsPending.Tables[0].Rows.Count > 0)
            {
                isPending = true;

                gvProformaDetail.DataSource = dsPending;
                gvProformaDetail.DataBind();

                ModalPopupProforma.Show();
            }
        }

        return isPending;

    }
    protected void SaveDetail()
    {
        // Check If any Vendor Final Invoice Pending
        bool IsValidAdvice = true;
        int EnqId = Convert.ToInt32(Session["EnqId"]);
        bool isFinalInvoicePending = CheckFinalInvoicePending(EnqId);

        if (isFinalInvoicePending == true)
        {
            IsValidAdvice = false;
            lblError.Text = "Please Provide Final Invoice against Proforma to Accounts Department!";
            lblError.CssClass = "errorMsg";

            return;
        }

        // Check If Vendor Payment Receipt Pending

        int IsVendorReceiptPending = DBOperations.CheckPaymentReceiptPending(EnqId, 1);

        if (IsVendorReceiptPending == 1)
        {
            IsValidAdvice = false;
            lblError.Text = "Please Upload Vendor Payment Receipt On Ops Accounting Module!";
            lblError.CssClass = "errorMsg";

            return;
        }

        // Check If Any Vendor Invoice Audit L2 Not Completed
        bool isVendorInvoicePending = CheckVendorInvoicePending(EnqId);

        if (isVendorInvoicePending == true)
        {
            IsValidAdvice = false;
            lblError.Text = "Please Complete Audit L2 Or Cancel Vendor Payment Invoice On Ops Accounting Module!";
            lblError.CssClass = "errorMsg";

            return;
        }

        /******** If Vendor Invoice Not Pending - Allow Billing Advice ********/
        if (IsValidAdvice == true)
        {
            Boolean bAgentInvoiceStatus = false;
            Boolean bSentToBilling = false;
                        
            string strReturnMessage = "";
            string strRemark = txtBillingRemark.Text.Trim();

            bAgentInvoiceStatus = Convert.ToBoolean(rdlAgentInvoice.SelectedValue);
            bSentToBilling = Convert.ToBoolean(rdlSentToBilling.SelectedValue);

            //ADDED BY 20032020
            int result = DBOperations.AddFreightAdvice(EnqId, bAgentInvoiceStatus, bSentToBilling, strRemark, 2, 2, LoggedInUser.glUserId);

            if (result == 0)
            {
                if (bSentToBilling == true)
                {
                    SaveDocument();
                }

                if (bAgentInvoiceStatus == true)
                {
                    SaveAgentInvoice();
                }
                if (bSentToBilling == true && bAgentInvoiceStatus == false)
                {
                    strReturnMessage = "Billing Advice Completed for JobNo:-" + lblJobNo.Text + " ! Job Moved To Billing Process! But Agent Details Are Pending.";
                }
                else if (bSentToBilling == true && bAgentInvoiceStatus == true)
                {
                    strReturnMessage = "Billing Advice Completed for JobNo:-" + lblJobNo.Text + " ! Job Moved To Billing Process! And Agent Details Are Completed.";
                }
                else if (bSentToBilling == false && bAgentInvoiceStatus == true)
                {
                    strReturnMessage = "Agent Detail Completed for JobNo:-" + lblJobNo.Text + " ! But Billing Advice Are Pending!.";
                }
                //strReturnMessage = "Billing Advice Completed for JobNo:-" + lblJobNo.Text + " ! Job Moved To Billing Process!.";
                Response.Redirect("AwaitingAdvice.aspx?mess=" + strReturnMessage);
            }
            else if (result == 0 && bAgentInvoiceStatus == true)
            {
                strReturnMessage = "Billing Advice Completed for JobNo:-" + lblJobNo.Text + " ! Job Moved To Billing Process! And Agent Details Are Completed.";
                Response.Redirect("AwaitingAdvice.aspx?mess=" + strReturnMessage);
            }
            else if (result == 0 && bAgentInvoiceStatus == false)
            {
                strReturnMessage = "Billing Advice Completed for JobNo:-" + lblJobNo.Text + " ! Job Moved To Billing Process! But Agent Details Are Pending.";
                Response.Redirect("AwaitingAdvice.aspx?mess=" + strReturnMessage);
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Billing Advice Detail Already Updated!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Billing Document Not Found! Please Upload Document!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            
            divDocument.Visible = false;
            dvAgentInvoice.Visible = false;
            
        }
    }
    protected void btnSaveAdvice_Click(object sender, EventArgs e)
    {
        int count = 0, FileCount = 0;
        string FileName = "";
        if (rdlSentToBilling.SelectedValue == "true")
        {
            foreach (RepeaterItem itm in rptDocument.Items)
            {
                CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                if (chk.Checked)
                {
                    //count = 0;
                    count = count + 1;
                    FileName = fuDocument.FileName;
                    if (FileName == "")
                    {
                        FileCount = FileCount + 1;
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('Please upload "+chk.Text+"');", true);
                        break;
                    }
                }
            }
            if (count >= 0 && FileCount == 0)
            {
                SaveDetail();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "Alert", "alert('Billing Document Select and Upload Compulsory');", true);
            }
        }
        else if (rdlSentToBilling.SelectedValue == "false")
        {
            SaveDetail();
        }
    }

    protected void btnCancelAdvice_Click(object sender, EventArgs e)
    {
        Session["JobType"] = "";
        Response.Redirect("AwaitingAdvice.aspx");


    }
    #region BJV POPUP EVENTS
    protected void btnCancelBJVdetails_Click(object sender, EventArgs e)
    {
        mpeBJVDetails.Hide();
    }
    protected void rptBJVDetails_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblDEBITAMT = ((Label)e.Item.FindControl("lblDEBITAMT"));
            if (lblDEBITAMT.Text == "")
                lblDEBITAMT.Text = "0";
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + int.Parse(lblDEBITAMT.Text);
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((Label)e.Item.FindControl("lbltotDebitamt")).Text = ViewState["lblDEBITAMT"].ToString();
        }
    }

    #endregion
    #region Agent Invoice

    protected void SaveAgentInvoice()
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int CurrencyId = 0; int AgentID = 0;
        string strJBNumber = "", strAgentName = "", strInvoiceNo = "", strRemark = "";

        decimal decInvoiceAmount = 0;
        DateTime dtInvoiceReceivedDate = DateTime.MinValue;
        DateTime dtJBDate = DateTime.MinValue, dtInvoiceDate = DateTime.MinValue;

        strJBNumber = "";
        AgentID = Convert.ToInt32(ddAgent.SelectedValue);
        strAgentName = ddAgent.SelectedItem.Text;
        strInvoiceNo = txtInvoiceNo.Text.Trim();
        CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
        strRemark = txtInvoiceRemark.Text.Trim();

        if (strAgentName == "")
        {
            lblError.Text = "Please Enter Agent Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
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
        else
        {
            lblError.Text = "Please Enter Invoice Amount!";
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
        if (txtInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }

        int result = DBOperations.AddFreightAgentInvoice(EnqId, dtInvoiceReceivedDate, strJBNumber, dtJBDate, AgentID, strAgentName,
            strInvoiceNo, dtInvoiceDate, decInvoiceAmount, CurrencyId, strRemark, LoggedInUser.glUserId);

        AgentInvoiceUpload();

        if (result == 0)
        {
            lblError.Text = "Agent Invoice Detail Updated Successfully!";
            lblError.CssClass = "success";

            //txtAgentName.Text = "";
            txtInvoiceDate.Text = "";
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

    #endregion


    protected void rdlAgentInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlAgentInvoice.SelectedValue == "true")
        {
            dvAgentInvoice.Visible = true;
            Agentfield.Visible = true;
            FillAgentInvoice();
        }
        else
        {
            dvAgentInvoice.Visible = false;
            Agentfield.Visible = false;
        }
    }

    protected void FillAgentInvoice()
    {
        int i = 1;
        DataTable dtAgent = DBOperations.FillAgentDetails(Convert.ToInt32(Session["EnqId"]));
        if (dtAgent.Rows.Count > 0)
        {
            rdlAgentInvoice.SelectedValue = "true";
            rdlAgentInvoice.Enabled = false;
            tblAgentDetails.Visible = false;
            dvAgentInvoice.Visible = true;
            Agentfield.Visible = true;
            AgentInvoiceDisable();
        }
    }

    protected void AgentInvoiceDisable()
    {
        ddCurrency.Enabled = false;
        ddAgent.Enabled = false;
        txtReceivedDate.Enabled = false;
        txtInvoiceDate.Enabled = false;
        txtInvoiceNo.Enabled = false;
        txtInvoiceAmount.Enabled = false;
        fuAgentInvoice.Enabled = false;
        txtInvoiceRemark.Enabled = false;
    }

    protected void rdlSentToBilling_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlSentToBilling.SelectedValue == "true")
        {
            string value = Session["JobType"].ToString();
            if (btnSaveAdvice.Visible == true)
            {
                divDocument.Visible = true;
            }
            else
            {
                divDocument.Visible = false;
                lblError.Text = "Job is hold so cant move to billing.";
                lblError.CssClass = "errorMsg";
            }

        }
        else
        {
            divDocument.Visible = false;
        }
    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");
            Label lblCheck = e.Item.FindControl("lblCheck") as Label;
            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");
            RequiredFieldValidator RFVFileUpload = (RequiredFieldValidator)e.Item.FindControl("RFVFile");

            if (chkDocumentType != null && fileUploadDocument != null)
            {
                if (hdnBranchId.Value == "3" || hdnBranchId.Value == "2")// Mumbai AIR/Sea - File Upload Not Required
                {
                    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "');");
                }
                else
                {
                    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + RFVFileUpload.ClientID + "');");
                }

                DataTable dt = DBOperations.Get_CompFRDocumentType(Session["JobType"].ToString(), Session["JobMode"].ToString());

                foreach (DataRow row in dt.Rows)
                {
                    if (chkDocumentType.Text == row["sName"].ToString())
                    {
                        chkDocumentType.Checked = true;
                        chkDocumentType.Enabled = false;
                        lblCheck.Text = "<font color='red'>*</font>";
                    }
                }
            }
        }
    }

    protected void SaveDocument()
    {
        //int JobId = Convert.ToInt32(hdnJobId.Value);
        int JobId = Convert.ToInt32(Session["EnqId"].ToString());
        hdnJobId.Value = Session["EnqId"].ToString();
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;

        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
            string strCustDocFolder = "", strJobFileDir = "";
            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;
            if (chk.Checked)
            {
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                string strFilePath = "", FileName = "";

                int DocumentId = Convert.ToInt32(hdnDocId.Value);

                string strDocFolder = "FreightDoc\\";// + hdnUploadPath.Value + "\\";
                FileName = fuDocument.FileName;


                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strDocFolder, fuDocument);
                    //strFilePath = UploadDocument(strDocFolder,FileName);
                }

                //Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);
                Result = DBOperations.AddFreightDocument(JobId, chk.Text, strFilePath, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblError.Text = "Document List Updated For Billing Advice.";
                    lblError.CssClass = "success";
                    //gvFreightDocument.DataBind();
                }
                else if (Result == 1)
                {
                    lblError.Text = "System Error! Please try after some time.";
                    lblError.CssClass = "errorMsg";

                }

                chk.Checked = false;
            }
        }
    }

    protected void AgentInvoiceUpload()
    {
        int JobId = Convert.ToInt32(Session["EnqId"].ToString());
        hdnJobId.Value = Session["EnqId"].ToString();
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;

        string strCustDocFolder = "", strJobFileDir = "";
        string strFilePath = "", FileName = "";
        hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

        if (fuAgentInvoice != null && fuAgentInvoice.HasFile)
        {
            string DocumentName = "Agent Invoice";
            string strDocFolder = "FreightDoc\\";// + hdnUploadPath.Value + "\\";
            FileName = fuAgentInvoice.FileName;

            if (fuAgentInvoice.FileName.Trim() != "")
            {
                strFilePath = UploadPCDDocument(strDocFolder, fuAgentInvoice);
                //strFilePath = UploadDocument(strDocFolder,FileName);
            }

            //Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);
            Result = DBOperations.AddFreightDocument(JobId, DocumentName, strFilePath, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Document List Updated For Billing Advice.";
                lblError.CssClass = "success";
                //gvFreightDocument.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after some time.";
                lblError.CssClass = "errorMsg";

            }
        }
    }




    #region Documnet Upload/Download/Delete
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        string FileName = fuPCDUpload.FileName;

        if (FilePath == "")
            //FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank
            FilePath = "FreightDoc\\";// + hdnUploadPath.Value + "\\";

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
                string ext = Path.GetExtension(fuPCDUpload.FileName);
                FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
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
    #endregion

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        Session["JobType"] = "";
    }



    protected void lnkDocument_Click(object sender, EventArgs e)
    {
        DataTable dtGetPCDDocs = new DataTable();
        dtGetPCDDocs = DBOperations.FillPCDDocument(Convert.ToInt32(Session["JobType"]));
        rptDocument.DataSource = dtGetPCDDocs;
        rptDocument.DataBind();
        rptDocument.Visible = true;
        // ModalPopupDocument.Show();
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


    protected void GridViewInvoiceDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    protected void gvFreightDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }


}