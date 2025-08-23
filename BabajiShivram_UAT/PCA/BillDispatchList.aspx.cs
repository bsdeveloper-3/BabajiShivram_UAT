using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Data;
using MyPacco.API;
public partial class PCA_BillDispatchList : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Bill Dispatch";

        HtmlAnchor hrefGoBack = (HtmlAnchor)Page.Master.FindControl("hrefGoBack");
        if(hrefGoBack != null)
        {
            hrefGoBack.HRef = "PCA/PendingBillDispatch.aspx";
        }

        if (!IsPostBack)
        {
            if (Session["BillJobIdList"] != null)
            {
                hdnJobIdList.Value = Session["BillJobIdList"].ToString();
            }

            DataSourceBillJobList.SelectParameters[0].DefaultValue = hdnJobIdList.Value; // "200117"; //346660
            DataSourceBillJobList.SelectParameters[1].DefaultValue = "1";
            DataSourceBillJobList.DataBind();
            gvJobDetail.DataBind();

            // Get Bill Dispatch Status

            //GetBillDispatchStatus(Convert.ToInt32(hdnJobId.Value));
        }
    }

    #region Event
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        hdnBillId.Value = "0";
        string strBJVNo = "";
        string strBJVBillNo = "";

        if (e.CommandName.ToLower() == "upload")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsGetBillDetail = BillingOperation.GetBJVBillDetailByID(BillId);

            if (dsGetBillDetail.Tables[0].Rows.Count > 0)
            {
                hdnBillId.Value = BillId.ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["BJVNo"] != null)
                {
                    lblBJVNumber.Text = dsGetBillDetail.Tables[0].Rows[0]["BJVNo"].ToString();
                }
                if (dsGetBillDetail.Tables[0].Rows[0]["INVNO"] != null)
                    lblBJVBillNo.Text = dsGetBillDetail.Tables[0].Rows[0]["INVNO"].ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["INVDATE"] != null)
                    lblBJVBillDate.Text = Convert.ToDateTime(dsGetBillDetail.Tables[0].Rows[0]["INVDATE"]).ToString("dd/MM/yyyy");
                if (dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"] != null)
                    lblBJVAmount.Text = dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"].ToString();

                BillUploadModalPopup.Show();
            }
            else
            {
                lblMessage.Text = "Bill Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
            }

        }
        else if (e.CommandName.ToLower() == "download")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(0, BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath ;

                DownloadDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Document Not Uploaded!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(0, BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                //string strFilePath = strDocPath + "//" + strFileName;

                string strFilePath = strDocPath;

                ViewDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Document Not Uploaded!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "ebillemail")
        {
            GetBillList();
        }
        else if (e.CommandName.ToLower() == "physicaldispatch")
        {
            MyPaccoAWBGeneration();
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DocId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DocId"));
                if (DocId == 0)
                {
                    CheckBox chkBillNo = (CheckBox)e.Row.FindControl("chkBillNo");
                    LinkButton lnkViewDoc = (LinkButton)e.Row.FindControl("lnkBillView");
                    LinkButton lnkBillDownload = (LinkButton)e.Row.FindControl("lnkBillDownload");

                    if (chkBillNo != null)
                    {
                        chkBillNo.Visible = false;
                    }
                    if (lnkViewDoc != null)
                    {
                        lnkViewDoc.Visible = false;
                    }
                    if (lnkBillDownload != null)
                    {
                        lnkBillDownload.Visible = false;
                    }


                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Document Not Uploaded";
                }
            }
        }
    }
    
    /************************************
    protected void btnUpdateBillStatus_Click(object sender, EventArgs e)
    {
        foreach(GridView row in gvJobDetail.Rows)
        {
            foreach (GridViewRow gvr1 in gvJobDetail.Rows)
            {
                if (gvr1.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                    if (chk1.Checked)
                    {
                       // strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString() + ",";
                    }
                }
            }
        }

        int JobId = 0;// Convert.ToInt32(hdnJobId.Value);

        int PhysicalBillStatus = 2; // Pending Dispatch
        int EBillStatus = 2; // Pending E-Bill
        int ClientPortalStatus = 0; // // Pending Client Portal Upload

        if (rblPhysicalBillRequired.SelectedValue == "1")
        {
            PhysicalBillStatus = 0;
        }
        if (rblEBillRequired.SelectedValue == "1")
        {
            EBillStatus = 0;
        }

        if (PhysicalBillStatus == 2 && EBillStatus == 2)
        {
            lblMessage.Text = "Error! At least one Type of Bill Dispatch Required - YES";
            lblMessage.CssClass = "errorMsg";

            return;
        }
        else
        {
            int result = 0;// DBOperations.AddBillDispatchCondition(JobId, PhysicalBillStatus, EBillStatus, "", 1, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Detail Updated Successfully!";
                lblMessage.CssClass = "success";
            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime";
                lblMessage.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblMessage.Text = "Physical Bill Already Sent";
                lblMessage.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblMessage.Text = "E-Bill Already Sent!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime";
                lblMessage.CssClass = "errorMsg";
            }
        }

    }
    ************************************/
    #endregion

    #region Bill Document
    protected void btnUploadBill_Click(object sender, EventArgs e)
    {
        int JobId = 0;// Convert.ToInt32(hdnJobId.Value);
        int BillId = Convert.ToInt32(hdnBillId.Value);

        DateTime dtBJVBillDate = Commonfunctions.CDateTime(lblBJVBillDate.Text.Trim());

        string strDirPath = lblBJVNumber.Text.Replace("/", "");

        strDirPath = strDirPath.Replace("-", "");

        string strInvoiceFilePath = "BillDocument\\" + strDirPath + "\\";

        if (fuReceipt.HasFile)
        {
            string FileName10 = UploadDocument(fuReceipt, strInvoiceFilePath);
            strInvoiceFilePath = strInvoiceFilePath + FileName10;

            int Result = BillingOperation.AddBillDocPath(JobId, BillId, FileName10, strInvoiceFilePath, 10, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblMessage.Text = "Bill Uploaded Successfully!.";
                lblMessage.CssClass = "success";

                BillUploadModalPopup.Hide();
            }
            else if (Result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime.";
                lblMessage.CssClass = "errormsg";

                BillUploadModalPopup.Hide();
            }
            else if (Result == 2)
            {
                lblMessage.Text = "Bill Already Uploaded!";
                lblMessage.CssClass = "errormsg";

                BillUploadModalPopup.Hide();
            }

            gvJobDetail.DataBind();

        }
        else
        {
            lblMessage.Text = "Please Upload Bill!";
            lblMessage.CssClass = "errorMsg";

            BillUploadModalPopup.Show();
        }
    }
    
    public string UploadDocument(FileUpload fuDocument, string FilePath)
    {

        string FileName = fuDocument.FileName;

        FileName = FileName.Replace(",", "");

        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath =  FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("../UploadFiles\\" + FilePath);
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

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FileName;
        }
        else
        {
            FileName = "";

            return FileName;
        }
    }
    
    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..//UploadFiles\\" + DocumentPath);
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
    
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
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

    #region E-Bill
    protected void btnPreviewEBill_Click(object sender, EventArgs e)
    {
        GetBillList();
    }
    private void GetBillList()
    {
        int JobId = 0;// Convert.ToInt32(hdnJobId.Value);

        List<BJVBill> lstBillList = new List<BJVBill>();

        DataTable dtDispatchBill = new DataTable();

        // Check Selected Bill

        foreach (GridViewRow rw in gvJobDetail.Rows)
        {
            // Find Selected CheckBox

            JobId       = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[0]);
            int BillId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                string strBJVNo1 = "", strBJVBillNo1 = "";

                Label lblBJVNo = (Label)rw.FindControl("lblBJVNo");
                Label lblBillNumber = (Label)rw.FindControl("lblBillNumber");

                strBJVNo1 = lblBJVNo.Text;
                strBJVBillNo1 = lblBillNumber.Text;

                BJVBill objBJVBill = new BJVBill();
                objBJVBill.JobId = JobId;
                objBJVBill.BJVNo = strBJVNo1;
                objBJVBill.BJVBillNo = strBJVBillNo1;

                lstBillList.Add(objBJVBill);

                DataSet dsBillDetail = BillingOperation.GetBillJobDetailForEBill(JobId, BillId);

                if (dsBillDetail.Tables.Count > 0)
                {
                    if (dsBillDetail.Tables[0].Rows.Count > 0)
                    {
                        if (dtDispatchBill.Rows.Count == 0)
                        {
                            dtDispatchBill = dsBillDetail.Tables[0];

                            dtDispatchBill.AcceptChanges();
                        }
                        else
                        {
                            dtDispatchBill.Rows.Add(dsBillDetail.Tables[0].Rows[0].ItemArray);

                            dtDispatchBill.AcceptChanges();
                        }
                    }
                }

            }

        }//END_ForEach

        if (dtDispatchBill.Rows.Count > 0)
        {
            GenerateEmailDraft(dtDispatchBill);

            BindAttachBillDoc(lstBillList);
        }
        else
        {
            lblMessage.Text = "Please Select Bill No!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    private void GenerateEmailDraft(DataTable dtDispatchBill)
    {
        lblErrorEmail.Text = "";
        btnSendEmail.Visible = true;

        string[] JobList  = hdnJobIdList.Value.Split(',');

        int JobId = Convert.ToInt32(JobList[0]);

        DataSet dsCustomerEmail = DBOperations.GetBillDispatchEmail(JobId, "");

        if (dsCustomerEmail.Tables.Count > 0)
        {
            if (dsCustomerEmail.Tables[0].Rows.Count > 0)
            {
                string strCustName = dsCustomerEmail.Tables[0].Rows[0]["Customer"].ToString();
                string strConsigneeName = dsCustomerEmail.Tables[0].Rows[0]["Consignee"].ToString();
                string strJobRefNo = dsCustomerEmail.Tables[0].Rows[0]["JobRefNo"].ToString();
                string strCustRefNo = dsCustomerEmail.Tables[0].Rows[0]["CustRefNo"].ToString();
                string strJobType = dsCustomerEmail.Tables[0].Rows[0]["JobType"].ToString();
                string strToMail = dsCustomerEmail.Tables[0].Rows[0]["Email"].ToString();

                txtMailTo.Text = strToMail;

                txtMailCC.Text = LoggedInUser.glUserName + "," + "CreditControl@BabajiShivram.com";

                txtSubject.Text = "E-Bill Dispatch/Job No :- " + strJobRefNo + " /Customer Reference No :- " + strCustRefNo + "";

                string strHTMLBill = ConvertDataTableToHTML(dtDispatchBill);

                divPreviewEmail.InnerHtml = strHTMLBill;

                ModalPopupEmail.Show();

            }

            else
            {
                lblMessage.Text = "Customer Email Details Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }

    }
    public static string ConvertDataTableToHTML(DataTable dt)
    {
        StringBuilder strStyle = new StringBuilder();
        strStyle = strStyle.Append("<html><body style='height:100%; width:100%; font-family:Arial; font-style:normal; font-size:9pt; color:#000;'>");

        // body header
        //strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
        //strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
        //strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");

        strStyle = strStyle.Append(@"Dear Sir / Madam, " + "<br />");
        //strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<br />" + "Kindly find the attached E-Invoices of Subject Shipment and details are as below. ");
        strStyle = strStyle.Append(@"<br /><br />");

        string html = "<table class='table' border='1'>";
        //add header row
        html += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            html += "<td>" + dt.Columns[i].ColumnName + "</td>";
        }

        html += "</tr>";
        //add rows
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
                html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
            html += "</tr>";
        }
        html += "</table>";

        strStyle = strStyle.Append(html);

        strStyle = strStyle.Append(@"<BR>Any billing related query or issue, kindly drop an e-mail to query.billing@babajishivram.com" + "<br/><br/>");
        strStyle = strStyle.Append(@"<BR>" + "Thanks & Regards");
        strStyle = strStyle.Append(@"<BR>" + "Babaji Shivram Clearing And Carriers Pvt Ltd");
        strStyle = strStyle.Append(@"</body></html>");

        string MessageBody = strStyle.ToString();
        return MessageBody;
    }

    private void BindAttachBillDoc(List<BJVBill> lstBillList)
    {
        decimal decMaxAttachSize = 14m;
        decimal decEmailAttachSize = 0m; // Max Attachment Size 14 MB

        DataSet dsDocDetail = new DataSet();
        DataTable dtDoc = new DataTable();

        foreach (BJVBill item in lstBillList)
        {
            // Get Uploaded Document Against Bill

            dsDocDetail = BillingOperation.GetBillDocPathByJobId(Convert.ToInt32(item.JobId), 1);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

               // string strFilePath = strDocPath + "//" + strFileName;
                
                string strFilePath = strDocPath;

                if (dtDoc.Rows.Count == 0)
                {
                    dtDoc = dsDocDetail.Tables[0];
                }
                else
                {
                    dtDoc.Rows.Add(dsDocDetail.Tables[0].Rows[0].ItemArray);
                }
            }
        }

        //DataTable dtDoc = DBOperations.GetBillDoc(0);

        DataColumn newCol = new DataColumn("NewColumn", typeof(string));
        newCol.AllowDBNull = true;
        dtDoc.Columns.Add(newCol);
        int j = 0;
        string DocPath = "";
        foreach (DataRow rows in dtDoc.Rows)
        {
            DocPath = rows["DocPath"].ToString();
            string strFileName = rows["FileName"].ToString();
            //DocPath = DocPath + "//" + strFileName; ;

            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("..//UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            FileInfo info = new FileInfo(ServerPath);
            decimal decFileSize = 0m;

            if (info.Exists)
            { 
                decimal length = info.Length;
                decFileSize = decimal.Round(length / (1000000), 2);

                decEmailAttachSize = decEmailAttachSize + decFileSize;
            }

            dtDoc.Rows[j]["NewColumn"] = decFileSize.ToString() + " Mb";
            
            j++;
        }
        gvDocAttach.DataSource = dtDoc;

        if (dtDoc.Columns.Contains("NewColumn"))
        {
            if (gvDocAttach.Columns.Count == 6)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtDoc.Columns["NewColumn"]);
                test.HeaderText = "File Size";
                gvDocAttach.Columns.Add(test);
            }
        }

        if(decEmailAttachSize > decMaxAttachSize)
        {
            // E-Mail Sending Disabled
            btnSendEmail.Visible = false;
            lblErrorEmail.Text = "Max Bill Attachment Size Per E-Mail - 14 MB. Please revise Bill List.";
            lblErrorEmail.CssClass = "errorMsg";
        }

        gvDocAttach.DataBind();
    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        string strJobIdList = "";

        List<BJVBill> EmailBillList = GetJobBillList();

        foreach(BJVBill item in EmailBillList)
        {
            strJobIdList = strJobIdList + item.JobId + ",";
        }

        if (txtMailTo.Text.Trim() == "")
        {
            lblMessage.Text = "Please Enter Customer Email & Subject!";
            lblMessage.CssClass = "errorMsg";
            ModalPopupEmail.Hide();
        }
        else
        {
            // Send Email
            bool bEMailSucess = SendDispatchEmail();
            //bool bEMailSucess = true;
            // Update PreAlert Email Sent Status and Customer Email 

            // ModalPopupEmail.Hide();

            if (bEMailSucess == true)
            {
                BillingOperation.updateBillingEBillList(strJobIdList, DateTime.Now, "", LoggedInUser.glUserId);

                int Result = DBOperations.AddJobNotoficationList(strJobIdList, 1, 14, txtMailTo.Text, txtMailCC.Text, txtSubject.Text, divPreviewEmail.InnerHtml, "0", LoggedInUser.glUserId);

                ModalPopupEmail.Hide();
                //lblStatus.Text = "";

                if (Result == 0)
                {
                    lblMessage.Text = "E-Bill Email Sent Successfully!";
                    lblMessage.CssClass = "success";
                    //dvMailSend.Visible = false;
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after sometime!";
                    lblMessage.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblMessage.Text = "E-Bill Email Already Sent!";
                    lblMessage.CssClass = "errorMsg";
                }

                gvJobDetail.DataBind();
            }
            else
            {
                lblMessage.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    private bool SendDispatchEmail()
    {
        //int JobId = Convert.ToInt32(Session["JobId"]);
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtMailTo.Text.Trim();
        strCCEmail = txtMailCC.Text.Trim();
        strSubject = txtSubject.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", ",").Trim();
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();

        bool bEmailSucces = false;

        if (strCustomerEmail == "" || strSubject == "")
        {
            lblPopMessageEmail.Text = "Please Enter Customer Email!";
            //       lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            MessageBody = divPreviewEmail.InnerHtml;

            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvDocAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailEbill("Query.Billing@BabajiShivram.com", strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }
    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }
    public class BJVBill
    {
        public int JobId;
        public string BJVNo;
        public string BJVBillNo;

        // public BJVBill(int jobid, string bjvNumber, string bjvBillNo) => (JobId, BJVNo, BJVBillNo) = (jobid, bjvNumber, bjvBillNo);
    }
    #endregion

    #region MyPacco Dispatch

    private List<BJVBill> GetJobBillList()
    {
        int JobId = 0;// Convert.ToInt32(hdnJobId.Value);

        List<BJVBill> lstBillList = new List<BJVBill>();

        DataTable dtDispatchBill = new DataTable();

        // Check Selected Bill

        foreach (GridViewRow rw in gvJobDetail.Rows)
        {
            // Find Selected CheckBox

            JobId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[0]);
            int BillId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                string strBJVNo1 = "", strBJVBillNo1 = "";

                Label lblBJVNo = (Label)rw.FindControl("lblBJVNo");
                Label lblBillNumber = (Label)rw.FindControl("lblBillNumber");

                strBJVNo1 = lblBJVNo.Text;
                strBJVBillNo1 = lblBillNumber.Text;

                BJVBill objBJVBill = new BJVBill();
                objBJVBill.JobId = JobId;
                objBJVBill.BJVNo = strBJVNo1;
                objBJVBill.BJVBillNo = strBJVBillNo1;

                lstBillList.Add(objBJVBill);
            }

        }//END_ForEach

        return lstBillList;
    }
    protected void MyPaccoAWBGeneration()
    {
        List<BJVBill> lstBillList = GetJobBillList();

        int i = 0;

        string JobType = "";
        hdnCustomerId.Value = "0";
        hdnCustomerName.Value = "";
        ViewState["AWBJobid"] = "";

        string Customerid = "";
        string Customername = "";

        ViewState["AWBJobid"] = "";

        foreach (BJVBill item in lstBillList)
        {
            if (ViewState["AWBJobid"].ToString() == "")
            {
                ViewState["AWBJobid"] = item.JobId;
            }
            else
            {
                ViewState["AWBJobid"] = ViewState["AWBJobid"].ToString() + ',' + item.JobId;
            }
        }

        if (ViewState["AWBJobid"].ToString() != "")
        {
            String strjobIdList = ViewState["AWBJobid"].ToString();

            // Check IF AWB Already Generated Against Job List

            DataSet dsJobAWB = DBOperations.MyPaccoCheckAWBForJobList(strjobIdList);

            if (dsJobAWB.Tables.Count > 0)
            {
                if (dsJobAWB.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Text = "My Pacco Order Already Created.";
                    lblMessage.CssClass = "errorMsg";

                    return;
                }
                else
                {

                    DataSet dsPlantAddressList = DBOperations.GetJobPlantAddressList(strjobIdList);

                    gvDispatchPlantAddress.DataSource = dsPlantAddressList;
                    gvDispatchPlantAddress.DataBind();

                    ModalPopupDispatch.Show();
                }
            }
            else
            {

                DataSet dsPlantAddressList = DBOperations.GetJobPlantAddressList(strjobIdList);

                gvDispatchPlantAddress.DataSource = dsPlantAddressList;
                gvDispatchPlantAddress.DataBind();

                ModalPopupDispatch.Show();
            }
        }
        else
        {
            lblMessage.Text = "Please Select Job Number!";
            lblMessage.CssClass = "errorMsg";
        }

    }

    protected void btnGenerateAWB_Click(object sender, EventArgs e)
    {
        int BranchID = Convert.ToInt32(ddBranch.SelectedValue);
        int PlantAddressID = 0;
        string strWarehouseCode = "MWH0000012448"; // Test Code

        if (BranchID == 3)// Mumbai
        {
            strWarehouseCode = "MWH0000012511";
        }
        else if (BranchID == 5)// Delhi
        {
            strWarehouseCode = "MWH0000008110";
        }
        else if (BranchID == 6)//Chennai
        {
            strWarehouseCode = "MWH0000003885";
        }
        foreach (GridViewRow row in gvDispatchPlantAddress.Rows)
        {
            CheckBox chk = row.Cells[0].Controls[1] as CheckBox;
            if (chk != null && chk.Checked)
            {
                PlantAddressID = Convert.ToInt32(gvDispatchPlantAddress.DataKeys[row.RowIndex].Value);
            }
        }

        if (PlantAddressID == 0)
        {
            ModalPopupDispatch.Show();
            lblDispatchMessage.Text = "Please check Delivery Address!";
            lblDispatchMessage.CssClass = "errorMsg";

            lblMessage.Text = "Please check Delivery Address!";
            lblMessage.CssClass = "errorMsg";
            return;
        }
        //else
        //{
        //    ModalPopupDispatch.Show();
        //    lblDispatchMessage.Text = "Please Address Found!";
        //    lblDispatchMessage.CssClass = "errorMsg";

        //    return;
        //}

        String strjobIdList = ViewState["AWBJobid"].ToString();

        DataSet dsPlantAddress = DBOperations.GetCustomerPlantAddressById(PlantAddressID);

        if (strjobIdList != "")
        {
            if (dsPlantAddress.Tables.Count > 0 && dsPlantAddress.Tables[0].Rows.Count > 0)
            {
                // Generate AWB
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                Root MyPaccoRoot = new Root();
                Datum MyPaccoDatum = new Datum();
                Order MyPaccoOrder = new Order();

                List<Datum> lstDatum = new List<Datum>();
                List<Order> lstOrder = new List<Order>();

                string strBabajiOrderNo = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")).ToString(); // DateTime.Today.TimeOfDay.ToString();
                DateTime dtAWBDate = DateTime.Now.AddDays(1);

                MyPaccoRoot.access_token = "";

                // MyPaccoResqAddOrderPl AWBBillDetail = new MyPaccoResqAddOrderPl();

                // MyPaccoOrder.seller_email = "amit.bakshi@babajishivram.com";

                MyPaccoOrder.seller_order_number = strBabajiOrderNo;
                MyPaccoOrder.seller_warehouse_code = strWarehouseCode;

                //MyPaccoOrder.pickup_fullname = "DO NOT PICKUP";// "Babaji Shivram";
                //MyPaccoOrder.pickup_mobile = "9833708840";
                //MyPaccoOrder.pickup_email = "amit.bakshi@babajishivram.com";
                //MyPaccoOrder.pickup_address = "For Testing, Pls Do Not Pickup "; //"Plot No 2A, Sakinka Andheri E Mumbai 450072";
                //MyPaccoOrder.pickup_pincode = "450072"
                ;
                //MyPaccoOrder.pickup_landmark = "Test";
                //MyPaccoOrder.pickup_city = "Test";
                //MyPaccoOrder.pickup_state = "Test";
                //MyPaccoOrder.pickup_country = "IN";

                MyPaccoOrder.delivery_fullname = dsPlantAddress.Tables[0].Rows[0]["ContactPerson"].ToString();
                MyPaccoOrder.delivery_mobile = dsPlantAddress.Tables[0].Rows[0]["MobileNo"].ToString();

                // Check If Customer Email Address is Valid

                string strDeliveryEmail = "sajid.shaikh@babajishivram.com";

                if (dsPlantAddress.Tables[0].Rows[0]["Email"] != DBNull.Value)
                {
                    strDeliveryEmail = dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                if (IsValidEmail(strDeliveryEmail))
                {
                    MyPaccoOrder.delivery_email = strDeliveryEmail;
                }
                else
                {
                    MyPaccoOrder.delivery_email = "sajid.shaikh@babajishivram.com";// dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                MyPaccoOrder.delivery_address = hdnCustomerName.Value + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine1"].ToString() + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine2"].ToString();
                MyPaccoOrder.delivery_pincode = dsPlantAddress.Tables[0].Rows[0]["Pincode"].ToString();
                MyPaccoOrder.delivery_landmark = "-";
                MyPaccoOrder.delivery_city = dsPlantAddress.Tables[0].Rows[0]["City"].ToString();
                MyPaccoOrder.delivery_state = "-";
                MyPaccoOrder.delivery_country = "IN";
                MyPaccoOrder.pickup_date = dtAWBDate.ToString("yyyy-MM-dd");

                MyPaccoOrder.transport_mode = "1";
                MyPaccoOrder.payment_type = "1";
                MyPaccoOrder.currency_unit = "INR";

                MyPaccoOrder.item_title = "Document";
                MyPaccoOrder.item_desc = "Document";
                MyPaccoOrder.item_quantity = "1";
                MyPaccoOrder.length = "15";
                MyPaccoOrder.height = "10";
                MyPaccoOrder.width = "8";
                MyPaccoOrder.weight = "0.5";
                MyPaccoOrder.base_price = "500";
                MyPaccoOrder.shipp_handling_charges = "0";
                MyPaccoOrder.other_charges = "0";
                MyPaccoOrder.total_amount = "500";

                lstOrder.Add(MyPaccoOrder);

                MyPaccoDatum.rts_order = true;
                MyPaccoDatum.orders = lstOrder;

                lstDatum.Add(MyPaccoDatum);
                MyPaccoRoot.data = lstDatum;

                MyPaccoSession objMyPaccoSession = new MyPaccoSession();

                if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) > 0 || objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken == "")
                {

                    MyPaccoResqAccessToken objMyPaccoResqAccessToken = new MyPaccoResqAccessToken();
                    MyPaccoClientID objMyPaccoClientID = new MyPaccoClientID();
                    List<MyPaccoClientID> lstMyPaccoClientID = new List<MyPaccoClientID>();

                    lstMyPaccoClientID.Add(objMyPaccoClientID);
                    objMyPaccoResqAccessToken.access_token = "";

                    objMyPaccoClientID.client_id = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientId;// "9702420066";
                    objMyPaccoClientID.client_secret = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientPassword;// "Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA";

                    objMyPaccoResqAccessToken.data = lstMyPaccoClientID;

                    //string strJsonPayLoad= "{\"access_token\": \"\",\"data\": [{\"client_id\": \"9702420066\",\"client_secret\": \"Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA\"}]}";

                    string strJsonPayLoad = serializer.Serialize(objMyPaccoResqAccessToken);

                    MyPaccoRespAccessToken txnRespWithObj = MYPACCOAPI.GetAuthTokenAsync(objMyPaccoSession, strJsonPayLoad);

                    if (txnRespWithObj.IsSuccess)
                    {
                        MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                        MyPaccoShared.SaveAPILoginDetails(objMyPaccoSession.MyPaccoApiLoginDetails);
                    }
                    else //if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) >= 0)
                    {
                        txnRespWithObj.IsSuccess = false;
                        lblMessage.Text = "Error : MyPacco Token Expired. Please Get AuthToken for Login";
                        lblMessage.CssClass = "errorMsg";

                        ModalPopupDispatch.Show();

                        return;
                    }
                }

                // Generate MyPacco AWB

                MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                string strAWBPayload = serializer.Serialize(MyPaccoRoot);

                //MyPaccoRespAddOrder objMyPaccoRespAddOrder = MYPACCOAPI.GenAWBAsync(objMyPaccoSession, strAWBPayload);

                /*********** Test ********************/
                MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();

                objMyPaccoRespAddOrder.IsSuccess = true;

                MyPaccoRespOrderDeail objOrderDetail = new MyPaccoRespOrderDeail();

                objOrderDetail.mypacco_order_id = "BM" + strBabajiOrderNo;
                objOrderDetail.seller_order_number = strBabajiOrderNo.ToString();

                List<MyPaccoRespOrderDeail> lstOrderDetail = new List<MyPaccoRespOrderDeail>();
                lstOrderDetail.Add(objOrderDetail);

                objMyPaccoRespAddOrder.Data = lstOrderDetail;
                /**********************************/

                if (objMyPaccoRespAddOrder.IsSuccess == true)
                {
                    // Save Job Details against AWB No
                    string strOrderNo = "", strAWBNo = "", strLSPName = "";

                    int CustomerId = 0;

                    Int32.TryParse(hdnCustomerId.Value, out CustomerId);

                    strOrderNo = objMyPaccoRespAddOrder.Data[0].mypacco_order_id;

                    if (objMyPaccoRespAddOrder.Data[0].awb_number != null)
                    {
                        strAWBNo = objMyPaccoRespAddOrder.Data[0].awb_number;
                    }
                    if (objMyPaccoRespAddOrder.Data[0].lsp_name != null)
                    {
                        strLSPName = objMyPaccoRespAddOrder.Data[0].lsp_name;
                    }

                    DBOperations.MyPaccoAddAWBNo(strOrderNo, strAWBNo, dtAWBDate, strLSPName, strjobIdList, CustomerId, BranchID, PlantAddressID, LoggedInUser.glUserId);

                    lblMessage.Text = "Success : My Pacco Order No: " + strOrderNo;
                    lblMessage.CssClass = "success";

                    ModalPopupDispatch.Hide();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('" + lblMessage.Text + "');", true);
                }
                else if (objMyPaccoRespAddOrder.Message != "")
                {
                    lblMessage.Text = "Error : " + objMyPaccoRespAddOrder.Message;
                    lblMessage.CssClass = "errorMsg";

                    ModalPopupDispatch.Show();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
                }
                else
                {
                    lblMessage.Text = "Error : MyPacco AWB Generation Error";
                    lblMessage.CssClass = "errorMsg";

                    ModalPopupDispatch.Show();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
                }
            }
            else
            {
                lblMessage.Text = "Plant Address Not Found!";
                lblMessage.CssClass = "errorMsg!";
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
            }
        }// Job Type Not Found
        else
        {
            lblMessage.Text = "Please Select Job for MyPacco Dispatch!";
            lblMessage.CssClass = "errorMsg!";

        }

    }
    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    protected void btnPhysicalDispatch_Click(object sender, EventArgs e)
    {

    }

    protected void btnClientPortalUpload_Click(object sender, EventArgs e)
    {

    }
}