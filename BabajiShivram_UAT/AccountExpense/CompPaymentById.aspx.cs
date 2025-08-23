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

public partial class AccountExpense_CompPaymentById : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(gvPaymentReqDocs);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Fund Request Details";
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["PaymentId"]) != null)
            {
                GetPaymentDetails();
            }
            else
            {
                Response.Redirect("CompPaymentById.aspx");
            }            

        }

        if (Convert.ToString(Session["StatusId"]) != null)
        {
            int StatusId = Convert.ToInt32(Session["StatusId"]);
            if (StatusId == 6)
            {
                try
                {
                    Button btnedit = (Button)FormView1.FindControl("btnEditJob");
                    btnedit.Visible = true;
                }
                catch
                {
                    //Button btnedit = (Button)FormView1.FindControl("btnEditJob");
                    //btnedit.Visible = true;
                }

                try
                {
                    Button btnDutyedit = (Button)fsDutyPayment.FindControl("btnEditDutyJob");
                    btnDutyedit.Visible = true;

                    fuDocument.Visible = true;
                    btnSaveDocument.Visible = true;
                }
                catch { }

            }
            else
            {
                Button btnedit = (Button)FormView1.FindControl("btnEditJob");
                btnedit.Visible = false;

                Button btnDutyedit = (Button)fsDutyPayment.FindControl("btnEditDutyJob");
                btnDutyedit.Visible = false;

                fuDocument.Visible = false;
                btnSaveDocument.Visible = false;
                // FileUpload fuDoc = (FileUpload)   fuDocument
            }
        }
    }

    protected void GetPaymentDetails()
    {
        DataSet dsGetPaymentDetail = new DataSet();
        int PaymentID = Convert.ToInt32(Session["PaymentId"]);
        dsGetPaymentDetail = AccountExpense.GetPaymentRequestById(Convert.ToInt32(Session["PaymentId"]));
        if (dsGetPaymentDetail.Tables[0].Rows.Count > 0)
        {
            if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"] != null)
                hdnPaymentTypeId.Value = dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString();
            if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "4")        //RTGS
            {
                fsRTGS.Visible = true;
                fsCheque_DD.Visible = false;
                fsCash.Visible = false;
            }
            else if (dsGetPaymentDetail.Tables[0].Rows[0]["PaymentTypeId"].ToString() == "1")   //Cash
            {
                fsCash.Visible = true;
                fsCheque_DD.Visible = false;
                fsRTGS.Visible = false;
            }
            else    // Cheque or DD
            {
                fsCheque_DD.Visible = true;
                fsCash.Visible = false;
                fsRTGS.Visible = false;
            }

            if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "1") // duty payment
            {
                string chequeno = "";
                fsDutyPayment.Visible = true;
                fsDutyPayment.DataBind();

                //chequeno = dsGetPaymentDetail.Tables[0].Rows[0]["ChequeNo"].ToString();
                //if (chequeno == "")
                //{
                //   // Label lblChequeNo = (Label)
                //}

                fsStampDuty.Visible = false;
                fsStampDuty.DataBind();
            }
            else if (dsGetPaymentDetail.Tables[0].Rows[0]["RequestTypeId"].ToString() == "5") // stamp duty payment
            {
                fsDutyPayment.Visible = false;
                fsDutyPayment.DataBind();
                fsStampDuty.Visible = true;
                fsStampDuty.DataBind();
            }
            else
            {
                fsDutyPayment.Visible = false;
                fsStampDuty.Visible = false;
            }
        }
    }

    //protected void btnCancel_Click(object sender, EventArgs e)
    //{

    //}

    protected void ResetControls()
    {
        hdnPaymentTypeId.Value = "0";
        fsCash.Visible = false;
        fsCheque_DD.Visible = false;
        fsRTGS.Visible = false;
    }

    protected void lnkPODCopy_OnClick(object sender, EventArgs e)
    {
        HiddenField hdnPODCopyPath = (HiddenField)fsRTGS.FindControl("hdnPODCopyPath");
        if (hdnPODCopyPath.Value != "")
        {
            DownloadDoc(hdnPODCopyPath.Value);
        }
    }

    protected void lnkPenaltyCopy_OnClick(object sender, EventArgs e)
    {
        HiddenField hdnPenaltyCopyPath = (HiddenField)fsDutyPayment.FindControl("hdnPenaltyCopyPath");
        if (hdnPenaltyCopyPath.Value != "")
        {
            DownloadDoc(hdnPenaltyCopyPath.Value);
        }
    }

    #region RTGS FORM-VIEW

    protected void fsRTGS_DataBound(object sender, EventArgs e)
    {
        if (fsRTGS.CurrentMode == FormViewMode.ReadOnly)
        {
            LinkButton lnkPODCopy = (LinkButton)fsRTGS.FindControl("lnkPODCopy");
            if (lnkPODCopy != null)
            {
                ScriptManager1.RegisterPostBackControl(lnkPODCopy);
            }

            Label lblBoeDate = (Label)fsDutyPayment.FindControl("lblBoeDate");
            if (lblBoeDate.Text.Trim() != "" && lblBoeDate.Text.Trim() == "01/01/1900")
            {
                lblBoeDate.Text = "";
            }
        }
    }

    #endregion

    #region DUTY AND STAMP DUTY FORM-VIEW DATABOUNDS

    protected void fsDutyPayment_DataBound(object sender,EventArgs e)
    {
        if (fsDutyPayment.CurrentMode == FormViewMode.ReadOnly)
        {
            LinkButton lnkPenaltyCopy = (LinkButton)fsDutyPayment.FindControl("lnkPenaltyCopy");
            if (lnkPenaltyCopy != null)
            {
                ScriptManager1.RegisterPostBackControl(lnkPenaltyCopy);
            }

            Label lblBoeDate = (Label)fsDutyPayment.FindControl("lblBoeDate");
            if (lblBoeDate.Text.Trim() != "" && lblBoeDate.Text.Trim() == "01/01/1900")
            {
                lblBoeDate.Text = "";
            }
        }
       else if (fsDutyPayment.CurrentMode == FormViewMode.Edit)
        {
            LinkButton lnkPenaltyCopy = (LinkButton)fsDutyPayment.FindControl("lnkPenaltyCopy");
            if (lnkPenaltyCopy != null)
            {
                ScriptManager1.RegisterPostBackControl(lnkPenaltyCopy);
            }

            //Label lblBoeDate = (Label)fsDutyPayment.FindControl("lblBoeDate");
            //if (lblBoeDate.Text.Trim() != "" && lblBoeDate.Text.Trim() == "01/01/1900")
            //{
            //    lblBoeDate.Text = "";
            //}

            ////////////////////////////  ACP / Non ACP  /////////////////////

            DropDownList ddlACPNonACP = (DropDownList)fsDutyPayment.FindControl("ddlACPNonACP");
            ListItemCollection collection = new ListItemCollection();
            collection.Add(new ListItem("-- Select --","0"));
            collection.Add(new ListItem("ACP","1"));
            collection.Add(new ListItem("Non ACP","2"));            
            
            ddlACPNonACP.DataSource = collection;
            ddlACPNonACP.DataTextField = "text";
            ddlACPNonACP.DataValueField = "value";
            ddlACPNonACP.DataBind();          

            FormView formview = fsDutyPayment;
            FormViewRow row = fsDutyPayment.Row;
            DataRowView rowview = (DataRowView)fsDutyPayment.DataItem;          
          
            var s_contractorId = "";
            s_contractorId = rowview["ACPNonACP"].ToString();

            if (s_contractorId !="")
            {
                ddlACPNonACP.SelectedValue = s_contractorId;
            }
            else
            {
                ddlACPNonACP.SelectedValue = "0";             
            }

            ////////////////////////////  RD / Duty / Penalty  /////////////////////

            DropDownList ddlRdDutyPenalty = (DropDownList)fsDutyPayment.FindControl("ddlRdDutyPenalty");
            ListItemCollection collectionRDP = new ListItemCollection();
            collectionRDP.Add(new ListItem("-- Select --", "0"));
            collectionRDP.Add(new ListItem("RD", "1"));
            collectionRDP.Add(new ListItem("Duty", "2"));
            collectionRDP.Add(new ListItem("Penalty", "3"));            

            ddlRdDutyPenalty.DataSource = collectionRDP;
            ddlRdDutyPenalty.DataTextField = "text";
            ddlRdDutyPenalty.DataValueField = "value";
            ddlRdDutyPenalty.DataBind();  
                   
            var RDDUTYPENALTY = "";
            RDDUTYPENALTY = rowview["PaymentType"].ToString();

            if (s_contractorId != "")
            {
                ddlRdDutyPenalty.SelectedValue = RDDUTYPENALTY;
            }
            else
            {
                ddlRdDutyPenalty.SelectedValue = "0";
            }
        }
    }

    protected void fsStampDuty_DataBound(object sender, EventArgs e)
    {
        if (fsStampDuty.CurrentMode == FormViewMode.ReadOnly)
        {
            Label lblBoeDate = (Label)fsStampDuty.FindControl("lblBOEDate_StampDuty");
            if (lblBoeDate.Text.Trim() != "" && lblBoeDate.Text.Trim() == "01/01/1900")
            {
                lblBoeDate.Text = "";
            }
        }
    }

    #endregion

    #region UPLOAD DOCUMENTS

    protected void gvPaymentReqDocs_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        int PaymentId = Convert.ToInt32(Session["PaymentId"]);

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDoc(DocPath);
        }
        else if (e.CommandName.ToLower() == "del")
        {  
            string index = e.CommandArgument.ToString();
            //int DocId = Conve
            if (index != "")
            {
                int DocId = Convert.ToInt32(index);

                int DelDoc = AccountExpense.DeletePaymentDocuments(DocId, LoggedInUser.glUserId);

                if (DelDoc == 0)
                {
                    lblError.Text = "Document Deleted Successfully !";
                    lblError.CssClass = "success";
                    gvPaymentReqDocs.DataSourceID = "DocumentSqlDataSource";
                    gvPaymentReqDocs.DataBind();
                }
                else if (DelDoc == 2)
                {
                    lblError.Text = "Record Not Found!";
                    lblError.CssClass = "errorMsg";
                }
                else if (DelDoc == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }            
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
        else
            ServerPath = ServerPath + DocumentPath;
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "Expense_" + Session["PaymentId"].ToString() + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + FilePath);
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

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    protected string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }
        return builder.ToString();
    }

    #endregion

    #region GridView Event

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                //Response.Redirect("ExpensePayment.aspx");
                Response.Redirect("ExpPaymentDetails.aspx");
            }
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

    #endregion

    #region  Basic Details Edit    
    protected void btnEditJob_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.Edit);

        if (Session["PaymentId"] != null)
        {
            GetJobDetailForCompPayment(Convert.ToInt32(Session["PaymentId"]));
        }

        DataSet dsBasicDetail = ViewState["dsJobDetail"] as DataSet;
        if (dsBasicDetail.Tables[0].Rows.Count > 0)
        {
            int RequestType = Convert.ToInt32(dsBasicDetail.Tables[0].Rows[0]["RequestTypeId"]);
            TextBox txt1 = (TextBox)FormView1.FindControl("txtAmount");

            if (RequestType == 1)
            {
                txt1.Enabled = false;
            }
            else
            {
                txt1.Enabled = true;
            }
        }
    }
    //protected void btnBackButton_Click(object sender, EventArgs e)
    //{
    //    Session["PaymentId"] = null;
    //    string strReutrnUrl = ((Button)sender).CommandArgument.ToString();
    //    Response.Redirect(strReutrnUrl);
    //}
    private void GetJobDetailForCompPayment(int PaymentId)
    {
        DataSet dsJobDetail = AccountExpense.GetJobDetailForCompPayment(PaymentId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            ViewState["dsJobDetail"] = dsJobDetail;            
            //FormView1.DataSource = dsJobDetail;
            FormView1.DataBind();
        }
    }
    protected void btnUpdateJob_Click(object sender, EventArgs e)
    {
        if (((TextBox)FormView1.FindControl("txtRemark")).Text.Trim() != "")
        {
            int JobId = 0, PaymentId = 0;
            string PaidTo = "", Remark = "";
            decimal Amount = 0;

            JobId = Convert.ToInt32(Session["JobId"]);
            PaymentId = Convert.ToInt32(Session["PaymentId"]);

            if (PaymentId > 0)
            {
                if (((TextBox)FormView1.FindControl("txtAmount")).Text.Trim() != "")
                    Amount = Convert.ToDecimal(((TextBox)FormView1.FindControl("txtAmount")).Text.Trim());

                if (((TextBox)FormView1.FindControl("txtPaidTo")).Text.Trim() != "")
                    PaidTo = ((TextBox)FormView1.FindControl("txtPaidTo")).Text.Trim();

                if (((TextBox)FormView1.FindControl("txtRemark")).Text.Trim() != "")
                    Remark = ((TextBox)FormView1.FindControl("txtRemark")).Text.Trim();

                int result = AccountExpense.UpdateJobDetailCompPayment(JobId, PaymentId, Amount, PaidTo, Remark, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Job Detail Updated Successfully !";
                    lblError.CssClass = "success";
                    FormView1.ChangeMode(FormViewMode.ReadOnly);
                    GetJobDetailForCompPayment(PaymentId);

                }
                else if (result == 2)
                {
                    lblError.Text = "Job Detail Not Found!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }//END_IF_JobId Check
            else
            {
                Response.Redirect("PaymentRequests.aspx");
            }
        }
        else
        {
            //spRemark.visible = true;
            lblError.Text = "Please Enter Rejection Remark";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void btnCancelButton_Click(object sender, EventArgs e)
    {
        FormView1.ChangeMode(FormViewMode.ReadOnly);
        if (Session["PaymentId"] != null)
        {
            GetJobDetailForCompPayment(Convert.ToInt32(Session["PaymentId"]));
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        string DocPath = "", FileName = "";
        int PaymentId = Convert.ToInt32(Session["PaymentId"]);

        if (fuDocument != null && fuDocument.HasFile)
        {
            DocPath = UploadFiles(fuDocument);
            FileName = fuDocument.FileName;
        }

        if (FileName != "")
        {
            int result_Doc = AccountExpense.AddExpenseDocDetails(PaymentId, DocPath, FileName, LoggedInUser.glUserId);

            if (result_Doc == 0)
            {
                DataSet dsJobDocDetails = AccountExpense.GetPaymentDocument(PaymentId);
                if (dsJobDocDetails.Tables[0].Rows.Count > 0)
                {
                    //gvPaymentReqDocs.DataSource = dsJobDocDetails;
                    gvPaymentReqDocs.DataSourceID = "DocumentSqlDataSource";
                    gvPaymentReqDocs.DataBind();
                }
            }
        }      
    }

    #endregion

    #region  Duty Request Edit
    protected void btnEditDutyJob_Click(object sender, EventArgs e)
    {
        fsDutyPayment.ChangeMode(FormViewMode.Edit);

        if (Session["PaymentId"] != null)
        {
            GetJobDetailForCompPayment(Convert.ToInt32(Session["PaymentId"]));
        }
    }

    protected void btnDutyCancelButton_Click(object sender, EventArgs e)
    {
        fsDutyPayment.ChangeMode(FormViewMode.ReadOnly);
        if (Session["PaymentId"] != null)
        {
            //GetJobDetailForCompPayment(Convert.ToInt32(Session["PaymentId"]));
            GetJobDetailForDutyPayment(Convert.ToInt32(Session["PaymentId"]));
        }
    }
    private void GetJobDetailForDutyPayment(int PaymentId)
    {
        DataSet dsJobDetailDuty = AccountExpense.GetJobDetailForCompPayment(PaymentId);
        if (dsJobDetailDuty.Tables[0].Rows.Count > 0)
        {
            //FormView1.DataSource = dsJobDetail;
            fsDutyPayment.DataBind();
        }
    }
    protected void btnUpdateJobDuty_Click(object sender, EventArgs e)
    {
        int JobId = 0, PaymentId = 0, ACPNonACP = 0, RdDutyPenalty = 0;
        string PaidTo = "", ChallanNo="";
        decimal DutyAmount = 0, IntAmount = 0, PenaltyAmount = 0, AdvanceDetails = 0;

        JobId = Convert.ToInt32(Session["JobId"]);
        PaymentId = Convert.ToInt32(Session["PaymentId"]);

        if (PaymentId > 0)
        {      
            ACPNonACP = Convert.ToInt32(((DropDownList)fsDutyPayment.FindControl("ddlACPNonACP")).SelectedValue);

            if (((TextBox)fsDutyPayment.FindControl("txtChallanNo")).Text.Trim() != "")
                ChallanNo = ((TextBox)fsDutyPayment.FindControl("txtChallanNo")).Text.Trim();

            if (((TextBox)fsDutyPayment.FindControl("txtDutyAmount")).Text.Trim() != "")
                DutyAmount = Convert.ToDecimal(((TextBox)fsDutyPayment.FindControl("txtDutyAmount")).Text.Trim());

            if (((TextBox)fsDutyPayment.FindControl("txtIntAmount")).Text.Trim() != "")
                IntAmount = Convert.ToDecimal(((TextBox)fsDutyPayment.FindControl("txtIntAmount")).Text.Trim());

            if (((TextBox)fsDutyPayment.FindControl("txtPenaltyAmount")).Text.Trim() != "")
                PenaltyAmount = Convert.ToDecimal(((TextBox)fsDutyPayment.FindControl("txtPenaltyAmount")).Text.Trim());

            RdDutyPenalty = Convert.ToInt32(((DropDownList)fsDutyPayment.FindControl("ddlRdDutyPenalty")).SelectedValue);

            if (((TextBox)fsDutyPayment.FindControl("txtAdvanceDetails")).Text.Trim() != "")
                AdvanceDetails = Convert.ToDecimal(((TextBox)fsDutyPayment.FindControl("txtAdvanceDetails")).Text.Trim());


            int result = AccountExpense.UpdateJobDetailDuty(JobId, PaymentId, ACPNonACP, ChallanNo, DutyAmount, IntAmount, 
                PenaltyAmount, RdDutyPenalty, AdvanceDetails,LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Duty Payment Detail Updated Successfully !";
                lblError.CssClass = "success";               
                GetJobDetailForDutyPayment(PaymentId);
                GetJobDetailForCompPayment(PaymentId);
                fsDutyPayment.ChangeMode(FormViewMode.ReadOnly);                
            }
            else if (result == 2)
            {
                lblError.Text = "Job Detail Not Found!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }//END_IF_JobId Check
        else
        {
            Response.Redirect("PaymentRequests.aspx");
        }
    }

    #endregion
}



