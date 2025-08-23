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

public partial class ExportPCA_PendingBillingAdvice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    /// <summary>
    /// There is a ButtonField column and the Id column
    /// therefore first edit cell index is 2
    /// </summary>
    const int FIRST_EDITABLE_CELL = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        if (!IsPostBack)
        {
            Session["JobId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Billing Advice";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Billing Advice!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }

        //if (this.gvJobDetail.SelectedIndex > -1)
        //{
        //    // Call UpdateRow on every postback
        //    this.gvJobDetail.UpdateRow(this.gvJobDetail.SelectedIndex, false);
        //}

        DataFilter1.DataSource = PCDSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingBillingAdvice.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["JobId"] = e.CommandArgument.ToString();
            if (Convert.ToString(Session["JobId"]) != "")
            {
                Response.Redirect("../ExportCHA/PCDDocDetail.aspx");
            }
        }
        else if (e.CommandName.ToLower() == "sendforscrutiny")
        {
            int JobId = Convert.ToInt32(e.CommandArgument);

            // Check If any Vendor Final Invoice Pending

            bool isFinalInvoicePending = CheckFinalInvoicePending(JobId);

            if (isFinalInvoicePending == true)
            {
                lblMessage.Text = "Please Provide Final Invoice against Proforma to Accounts Department!";
                lblMessage.CssClass = "errorMsg";

                return;
            }

            // Check If Vendor Payment Receipt Pending

            int IsVendorReceiptPending = DBOperations.CheckPaymentReceiptPending(JobId, 1);

            if (IsVendorReceiptPending == 1)
            {
                lblMessage.Text = "Please Upload Vendor Payment Receipt On Ops Accounting Module!";
                lblMessage.CssClass = "errorMsg";

                return;
            }

            // Check If Any Vendor Invoice Audit L2 Not Completed
            bool isVendorInvoicePending = CheckVendorInvoicePending(JobId);

            if (isVendorInvoicePending == true)
            {
                lblMessage.Text = "Please Complete Audit L2 Or Cancel Vendor Payment Invoice / Blank Cheque On Ops Accounting Module!";
                lblMessage.CssClass = "errorMsg";

                return;
            }


            //DataTable dt = DBOperations.Get_BillingInstructionDetail(JobId);
            //if (dt.Rows.Count > 0)
            //{
            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
                DataSet dsGetPCDDocs = DBOperations.FillPCDDocumentForWorkFlow(JobId, PCDDocType);
                if (dsGetPCDDocs != null)
                {
                    //for (int i = 0; i < dsGetPCDDocs.Tables[0].Rows.Count; i++)
                    //{
                    DataTable table = dsGetPCDDocs.Tables[0];
                    foreach (DataRow rw in table.Rows)
                    {
                        int PCDDocId = Convert.ToInt32(rw["PCDDocId"].ToString());
                        if (PCDDocId != 0 && PCDDocId != 9)
                        {
                            SendDocumentForScrutiny(JobId);
                        }
                        else
                        {
                            lblMessage.Text = "Please upload Billing Document";
                            lblMessage.CssClass = "errorMsg";
                        }
                    }
                }
            //}
            //else
            //{
            //    lblMessage.Text = "Please fill billing instruction";
            //    lblMessage.CssClass = "errorMsg";
            //}
        }
        else if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            lblMessage.Text = "";
            ScriptManager1.RegisterPostBackControl(btnSaveDocument);
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
            rptDocument.DataSource = EXOperations.EX_GetPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();
            ModalPopupDocument.Show();
        }
        else if (e.CommandName.ToLower() == "instructionpopup" && e.CommandArgument != null)
        {
            // hdnBranchId.Value = "0";
            Session["BillingInstruction"] = "1";
            ScriptManager1.RegisterPostBackControl(btnSaveDocument);

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";

            SpanVisible();
            Get_JobDetailForBillingInstruction(Convert.ToInt32(hdnJobId.Value));
            Get_BillingInstruction(Convert.ToInt32(hdnJobId.Value));
            ModalPopupInstruction.Show();

        }
        else if (e.CommandName.ToLower().Trim() == "hold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strAmount = "", strJobRefNo = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strAmount = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                strJobRefNo = commandArgs[2].ToString();

            if (JobId != 0)
            {
                txtReason.Text = "";
                lblError_HoldExp.Text = "";
                Session["JobId"] = commandArgs[0].ToString();
                hdnJobId.Value = JobId.ToString();

                fvHoldJobDetail.DataBind();
                fvHoldJobDetail.Visible = true;
                Div3.Visible = true;
                hdnJobRefNo.Value = strJobRefNo;

                lblHoldPopupName.Text = "Hold Job";
                btnHoldJob.Text = "Hold Job";
                Label lblAmount = (Label)fvHoldJobDetail.FindControl("lblAmount");
                if (lblAmount != null)
                {
                    lblAmount.Text = strAmount.ToString();
                }
                mpeHoldExpense.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "unhold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strAmount = "", strJobRefNo = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strAmount = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                strJobRefNo = commandArgs[2].ToString();

            if (JobId != 0)
            {
                if (JobId != 0)
                {
                    int result = DBOperations.EX_AddHoldBillingAdvice(JobId, "", "", LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        fvHoldJobDetail.DataBind();
                        lblMessage.Text = "Successfully unholded job no " + strJobRefNo + ".";
                        lblMessage.CssClass = "success";
                        gvJobDetail.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System error. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
        else if (e.CommandName.ToLower() == "contractbilling" && e.CommandArgument != null)
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton lnktext = row.FindControl("lnkbiliing") as LinkButton;
            string consname = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            if (lnktext.Text == "Genrate Inovice")
            {
                hdnJobId.Value = commandArgs[0].ToString();
                Session["JobId"] = commandArgs[0].ToString();
                txtjobno.Text = commandArgs[1].ToString();
                //PrintBill(commandArgs[2].ToString());
                consname = commandArgs[2].ToString();
                DataSet dtgrid = DBOperations.ContractBilling_Pint(txtjobno.Text, consname);
                try
                {
                    // Crystal Report viwer Coding
                    MasterPage mspage = Master as MasterPage;
                    HtmlControl html1 = (HtmlControl)mspage.FindControl("prntesting");
                    html1.Attributes["style"] = "overflow-x:hidden; overflow-y:hidden;";

                    Session["jobno"] = txtjobno.Text;
                    Session["consname"] = consname;

                    //ReportDocument cryRpt = new ReportDocument();
                    //cryRpt.Load(Server.MapPath("\\CrystalReports\\rpt_Bill.rpt"));
                    //cryRpt.SetDatabaseLogon("sa", "sa", "192.168.6.98", "ContractDBTest");
                    //CrystalReportViewer1.ReportSource = cryRpt;
                    //CrystalReportViewer1.RefreshReport();
                    divContractBillingPrint.Visible = true;
                    mainfield.Visible = false;

                    //  Crystal Report open new Tab
                    //string url = "PendingBillingAdvice_print.aspx";
                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("<script type = 'text/javascript'>");
                    //sb.Append("window.open('");
                    //sb.Append(url);
                    //sb.Append("');");
                    //sb.Append("</script>");
                    //ClientScript.RegisterStartupScript(this.GetType(),"script", sb.ToString());
                }
                catch (Exception ex)
                {
                    //    //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (lnktext.Text == "Billing")
            {
                hdnBranchId.Value = "0";
                Session["BillingInstruction"] = "1";
                ScriptManager1.RegisterPostBackControl(btnSaveDocument);

                hdnJobId.Value = commandArgs[0].ToString();
                Session["JobId"] = commandArgs[0].ToString();
                txtjobno.Text = commandArgs[1].ToString();
                Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));

                if (grdbillinglinedetails.Rows.Count > 0)
                {
                    Get_JobDetailForContractBillingUser(Convert.ToInt32(hdnJobId.Value));
                }
                if (grdbillinglinedetails.Rows.Count == 0)
                {
                    btnsaveContractBilling.Enabled = false;
                }
                else
                {
                    btnsaveContractBilling.Enabled = true;
                }
                if (grdcontract_User.Rows.Count == 0)
                {
                    btnAdduser.Visible = false;
                }
                else
                {
                    btnAdduser.Visible = true;
                }
                ModalPopupContractBilling.Show();
            }

        }
        else if (e.CommandName.ToLower() == "contractualbillinginstruction" && e.CommandArgument != null)
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton lnktext = row.FindControl("lnkotherinstru") as LinkButton;
            string consname = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            if (lnktext.Text == "Contractual Billing Instruction")
            {
                hdnBranchId.Value = "0";
                Session["BillingInstruction"] = "1";
                ScriptManager1.RegisterPostBackControl(btnSaveDocument);

                hdnJobId.Value = commandArgs[0].ToString();
                Session["JobId"] = commandArgs[0].ToString();
                txtjobno.Text = commandArgs[1].ToString();

                Label1.Text = "";
                int count1 = DBOperations.Get_EXJobDetailForContratcBillingByJobIdUser_Checking(Convert.ToInt32(hdnJobId.Value));
                if (count1 == 1) { btnsaveContractBilling.Enabled = false; } else { btnsaveContractBilling.Enabled = true; }
                Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));

                if (grdbillinglinedetails.Rows.Count > 0)
                {
                    Get_JobDetailForContractBillingUser(Convert.ToInt32(hdnJobId.Value));
                }
                if (grdbillinglinedetails.Rows.Count == 0)
                {
                    btnsaveContractBilling.Enabled = false;
                }
                else
                {
                    btnsaveContractBilling.Enabled = true;
                }
                btnAdduser.Visible = false;
                grdbillinglinedetails.Visible = false;
                ModalPopupContractBilling.Show();

            }
        }
        if (e.CommandName.ToLower() == "contractbilling1" && e.CommandArgument != null)
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton lnktext = row.FindControl("lnkbiliing") as LinkButton;
            string consname = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('test1 !');", true);
            //lnktext.PostBackUrl = "View_Bill.aspx";
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            txtjobno.Text = commandArgs[1].ToString();
            //PrintBill(commandArgs[2].ToString());
            consname = commandArgs[2].ToString();
            DataSet dtgrid = DBOperations.ContractBilling_Pint(txtjobno.Text, consname);
            //ReportDocument reportDocument = new ReportDocument();
            try
            {
                MasterPage mspage = Master as MasterPage;
                HtmlControl html1 = (HtmlControl)mspage.FindControl("prntesting");
                html1.Attributes["style"] = "overflow-x:hidden; overflow-y:hidden;";

                Session["jobno"] = txtjobno.Text;
                Session["consname"] = consname;

                //ReportDocument cryRpt = new ReportDocument();
                //cryRpt.Load(Server.MapPath("\\CrystalReports\\rpt_Bill.rpt"));
                //CrystalReportViewer1.ReportSource = cryRpt;
                divContractBillingPrint.Visible = true;
                mainfield.Visible = false;
            }
            catch (Exception ex)
            {
                //    //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "ScrutinyStatus") != DBNull.Value)
            {
                int ScrutinyStatus = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ScrutinyStatus"));
                if (ScrutinyStatus == 3) // Rejected
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Scrutiny Status - Rejected";
                }
            }

            // Contract billing
            HiddenField hdnJOBID = (HiddenField)e.Row.FindControl("hdnJOBID");
            HiddenField statuscontract = (HiddenField)e.Row.FindControl("statuscontract");

            DataSet dtbilling = DBOperations.Ex_GetJobDetailForContractBillingUser(Convert.ToInt32(hdnJOBID.Value), LoggedInUser.glEmpName, 1);

            // Check IF Customer Contract Exists
            
            int hdnot = 0;
            if (dtbilling.Tables[0].Rows.Count > 0)
            {
                if (dtbilling.Tables[1].Rows[0][0].ToString() == "1")
                {
                    hdnot = 1;
                }
                else { hdnot = 0; }
            }
            else if (dtbilling.Tables[0].Rows.Count == 0)
            {
                hdnot = 2; //int.Parse(CDatabase.ResultInString("select count(*) Value from temp_Usercount where JobId='" + hdnJOBID.Value + "' and Quantity=0 "));
            }
            //HiddenField hdnot = (HiddenField)e.Row.FindControl("hdnot");
            LinkButton lnkScrutiny1 = (LinkButton)e.Row.FindControl("lnkScrutiny");
            LinkButton lnkotherinstru = (LinkButton)e.Row.FindControl("lnkotherinstru");
            if (dtbilling.Tables[2].Rows[0][0].ToString() == "No Contract")
            {
                lnkotherinstru.Enabled = false;
                lnkScrutiny1.Enabled = true;
            }
            else if (dtbilling.Tables[2].Rows[0][0].ToString() == "Contract")
            {
                if (hdnot == 0) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = false; }
                else if (hdnot == 1) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = true; }
                if (hdnot == 0) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = false; }
                else if (hdnot == 1) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = true; }
                else if (hdnot == 2) { lnkotherinstru.Enabled = false; lnkScrutiny1.Enabled = true; }
            }

            // Hold Status
            if (DataBinder.Eval(e.Row.DataItem, "HoldStatus") != DBNull.Value)
            {
                ImageButton imgbtnHoldJob = (ImageButton)e.Row.FindControl("imgbtnHoldJob");
                ImageButton imgbtnUnholdJob = (ImageButton)e.Row.FindControl("imgbtnUnholdJob");
                LinkButton lnkScrutiny = (LinkButton)e.Row.FindControl("lnkScrutiny");

                string HoldStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "HoldStatus"));
                if (HoldStatus.ToLower().Trim() == "hold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = true;
                        imgbtnUnholdJob.Visible = false;
                        lnkScrutiny.Visible = true;
                    }
                }
                else if (HoldStatus.ToLower().Trim() == "unhold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = false;
                        imgbtnUnholdJob.Visible = true;
                        lnkScrutiny.Visible = false;
                    }
                }
            }

            // Editable Cell

            AddCell(FIRST_EDITABLE_CELL + 5, "Instructions", e);
        }
    }

    void AddCell(int cellIndex, string cellName, GridViewRowEventArgs e)
    {
        //we will add javascript events to the controls in the cell
        Label lab = (Label)e.Row.Cells[cellIndex].FindControl(string.Format("lab{0}", cellName));
        TextBox txt = (TextBox)e.Row.Cells[cellIndex].FindControl(string.Format("txt{0}", cellName));
        Button btn = (Button)e.Row.Cells[cellIndex].FindControl(string.Format("btn{0}", cellName));

        if (lab != null)
        {
            //if text is empty, we need to grow label's width to ensure user can click this label
            if (string.IsNullOrEmpty(lab.Text))
            {
                lab.Width = 100;
                lab.Text = "Add";
            }

            //add javascript events to:
            //label (we want to hide this label and show textbox on click)
            lab.Attributes.Add("onclick", string.Format("return HideLabel('{0}', event, '{1}');", lab.ClientID, txt.ClientID));
            //and to textbox
            txt.Attributes.Add("onkeypress", string.Format("return SaveDataOnEnter('{0}', event, '{1}', '{2}');", txt.ClientID, lab.ClientID, btn.ClientID));
            //todo: there is an issue with onblur, I am not JS master, but hope it is just a small issue

            txt.Attributes.Add("onblur", string.Format("return SaveDataOnLostFocus('{0}', '{1}');", txt.ClientID, btn.ClientID));

            //highlight a text in textbox
            txt.Attributes.Add("onfocus", "select()");

            //we need to know what row and cell was edited
            //you can use anything else instead, e.g. session or whatever
            btn.CommandName = e.Row.RowIndex.ToString();
            btn.CommandArgument = cellIndex.ToString();

            //set a cursor style
            e.Row.Attributes["style"] += "cursor:pointer;cursor:hand;"; //just a cosmetic thing :)
        }
    }

    protected void txtInstructions_Changed(object sender, CommandEventArgs e)
    {
        //names of controls in GridView row
        string txtInstructions = "txtInstructions";	//textbox
        string labInstructions = "labInstructions";	//label
        //DB column
        string columnName = "Instructions";

        UpdateValue(txtInstructions, labInstructions, columnName, e);
    }

    protected void UpdateValue(string txtName, string labName, string columnName, CommandEventArgs e)
    {
        //todo: you can create e.g. a class and use it instead command argument

        int row = int.Parse(e.CommandName);
        int cell = int.Parse(e.CommandArgument.ToString());

        if (gvJobDetail.Rows[row].RowType == DataControlRowType.DataRow)
        {
            //get the JobID
            int JobId = Convert.ToInt32(gvJobDetail.DataKeys[row]["JobId"]);

            //get the JobRefNo
            LinkButton lnkJobNo = (LinkButton)gvJobDetail.Rows[row].Cells[0].FindControl("lnkJobNo");
            //get the new value
            TextBox txt = (TextBox)gvJobDetail.Rows[row].Cells[cell].FindControl(txtName);
            //update the label, if data are not re-bounded
            Label lab = (Label)gvJobDetail.Rows[row].Cells[cell].FindControl(labName);
            lab.Text = txt.Text;

            string strJobRefno = lnkJobNo.Text.Trim();

            int result = DBOperations.UpdateBillingInstructions(JobId, txt.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Billling Instructions Updated!";
                lblMessage.CssClass = "success";
            }
            else if (result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime.";
                lblMessage.CssClass = "errormsg";
            }
            else
            {
                lblMessage.Text = "Job Details Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");

            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");

            chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));

            chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));

            if (DataBinder.Eval(e.Item.DataItem, "PCDDocId").ToString() != "0")
            {
                chkDocumentType.Checked = true;
                fileUploadDocument.Enabled = true;
            }

            if (chkDocumentType != null && fileUploadDocument != null && CVCheckBoxList != null)
            {
                // CheckBox OnClientClick java Script Functino
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");

                // CheckBoxList Customer Validation Control "ClientValidationFunction="ValidateCheckBoxList"
                // Add Parameter for Javascript Function ValidateCheckBoxList("Update Panel Id","CustomerValidatorId","Control Identifier","CheckBoxlistId","IsValid"); 

                ScriptManager.RegisterExpandoAttribute(upShipment, CVCheckBoxList.ClientID, "checklistId", chkDuplicate.ClientID, false);

                // Add Javascript On Click Event For Checkbox List Copy/Original
                foreach (System.Web.UI.WebControls.ListItem lstitem in chkDuplicate.Items)
                {
                    lstitem.Attributes.Add("OnClick", "javascript:chkDuplicateChecked('" + chkDuplicate.ClientID + "','" + chkDocumentType.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                }
            }
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(hdnJobId.Value);
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;

        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");

            if (chk.Checked)
            {
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                CheckBoxList chkDuplicate = (CheckBoxList)(itm.FindControl("chkDuplicate"));
                string strFilePath = "";
                bool IsCopy = false, IsOriginal = false;

                int DocumentId = Convert.ToInt32(hdnDocId.Value);
                if (chkDuplicate.Items[0].Selected)
                    IsOriginal = true;

                if (chkDuplicate.Items[1].Selected)
                    IsCopy = true;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strUploadPath, fuDocument);
                }

                Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblMessage.Text = "Document List Updated For Billing Advice.";
                    lblMessage.CssClass = "success";
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after some time.";
                    lblMessage.CssClass = "errorMsg";

                }
            }
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    private void SendDocumentForScrutiny(Int32 JobId)
    {
        int Result = DBOperations.AddPCDToScrutiny(JobId, LoggedInUser.glUserId);
        if (Result == 0)
        {
            lblMessage.Text = "Job Forwarded For Scrutiny!";
            lblMessage.CssClass = "success";

            gvJobDetail.DataBind();

        }//END_IF
        else if (Result == 1)
        {
            lblMessage.Text = "System Error! Please try after sometime!";
            lblMessage.CssClass = "errorMsg";
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

    #region Documnet Upload/Download/Delete
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        string FileName = fuPCDUpload.FileName;

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
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

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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
            DataFilter1.FilterSessionID = "PendingBillingAdvice.aspx";
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
        string strFileName = "PendingBillingAdvice_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.Columns[0].Visible = false; // Link Button job Ref NO
        gvJobDetail.Columns[1].Visible = false; // Link Button job Ref NO
        gvJobDetail.Columns[2].Visible = false;  // Bound Field Job Ref NO
        gvJobDetail.Columns[3].Visible = true;  // Bound Field Job Ref NO
        gvJobDetail.Columns[4].Visible = false;  // Link button for instructions
        gvJobDetail.Columns[5].Visible = false;  // Link button for instructions

        gvJobDetail.Columns[15].Visible = false;  // Billling Document Link Button
        gvJobDetail.Columns[22].Visible = false;  // Send Document For Scrutiny Link Button
        gvJobDetail.Enabled = false;  

        gvJobDetail.Caption = "Pending Billing Advice On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingBillingAdvice.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region Billing instruction

    protected void btnCanInstruction_Click(object sender, EventArgs e)
    {
        ModalPopupInstruction.Hide();
    }

    protected void btnSaveInstruction_Click(object sender, EventArgs e)
    {
        lblBillMsg.Text = "";
        ModalPopupInstruction.Show();
        SaveBillingInstruction();
    }

    protected void SaveBillingInstruction()
    {
        int result = 0;
        int JobId, WithoutLRStatus = 0;
        string AlliedAgencyRemark = "", Instruction = "", Instruction1 = "", Instruction2 = "", Instruction3 = "", FilePath = "";
        string AlliedAgencyService = "", OtherService = "", OtherServiceRemark = "", InstructionCopy = "",
            InstructionCopy1 = "", InstructionCopy2 = "", InstructionCopy3 = "";

        JobId = Convert.ToInt32(Session["JobId"]);
        for (int i = 0; i < chkAgencyService.Items.Count; i++)
        {
            if (chkAgencyService.Items[i].Selected)
            {
                AlliedAgencyService += chkAgencyService.Items[i].Text + ";";
            }
        }
        AlliedAgencyRemark = txtAgencyServiceRemark.Text;

        for (int i = 0; i < chkOtherService.Items.Count; i++)
        {
            if (chkOtherService.Items[i].Selected)
            {
                OtherService += chkOtherService.Items[i].Text + ";";
            }
        }
        OtherServiceRemark = txtOtherServiceRemark.Text;

        Instruction = txtOtherRemark.Text;
        Instruction1 = txtOtherRemark1.Text;
        Instruction2 = txtOtherRemark2.Text;
        Instruction3 = txtOtherRemark3.Text;

        DataTable dtBillInstruction = DBOperations.Get_BillingInstructionDetail((Convert.ToInt32(hdnJobId.Value)));

        if (dtBillInstruction.Rows.Count > 0)
        {
            foreach (DataRow rw in dtBillInstruction.Rows)
            {
                InstructionCopy = rw["InstructionCopy"].ToString();
                InstructionCopy1 = rw["InstructionCopy1"].ToString();
                InstructionCopy2 = rw["InstructionCopy2"].ToString();
                InstructionCopy3 = rw["InstructionCopy3"].ToString();
            }
        }

        if (FuInstructionCopy.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy = UploadDoc(FuInstructionCopy, FilePath);
        }

        if (FuInstructionCopy1.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy1 = UploadDoc(FuInstructionCopy1, FilePath);
        }

        if (FuInstructionCopy2.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy2 = UploadDoc(FuInstructionCopy2, FilePath);
        }

        if (FuInstructionCopy3.HasFile)
        {
            FilePath = "BillingInstructionCopy" + "\\" + Session["JobId"] + "\\";
            InstructionCopy3 = UploadDoc(FuInstructionCopy3, FilePath);
        }

        if (txtOtherRemark.Text != "" && (InstructionCopy == "" && FuInstructionCopy.Enabled == true)) { lblMessage.Text = "Please complete Other instruction and upload instruction file"; }
        if (txtOtherRemark1.Text != "" && (InstructionCopy1 == "" && FuInstructionCopy1.Enabled == true)) { lblMessage.Text = "Please complete Other instruction and upload instruction file"; }
        if (txtOtherRemark2.Text != "" && (InstructionCopy2 == "" && FuInstructionCopy2.Enabled == true)) { lblMessage.Text = "Please complete Other instruction and upload instruction file"; }
        if (txtOtherRemark3.Text != "" && (InstructionCopy3 == "" && FuInstructionCopy3.Enabled == true)) { lblMessage.Text = "Please complete Other instruction and upload instruction file"; }

        if (lblMessage.Text == "")
        {
            result = DBOperations.EX_AddBillingInstruction(JobId, AlliedAgencyService, AlliedAgencyRemark, OtherService, OtherServiceRemark, Instruction, Instruction1, Instruction2, Instruction3,
                InstructionCopy, InstructionCopy1, InstructionCopy2, InstructionCopy3, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Billing Instruction Added Successfully.";
                lblMessage.CssClass = "success";
                ModalPopupInstruction.Hide();
            }
            else
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    protected void lnkBillingDocument_Click(object sender, EventArgs e)
    {
        Session["BillingInstruction"] = "1";
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        rptDocument.DataSource = EXOperations.EX_GetPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
        rptDocument.DataBind();
        ModalPopupDocument.Show();
    }

    protected void lnkInstructionCopy_Click(object sender, EventArgs e)
    {
        DownloadDocument(hdnInstructionCopy.Value);
        ModalPopupInstruction.Show();
    }

    protected void Get_JobDetailForBillingInstruction(int JobId)
    {
        int strJobId = JobId;
        DataTable dtbillingInstruction = DBOperations.Get_JobDetailForBillingInstruction(JobId);
        if (dtbillingInstruction.Rows.Count > 0)
        {
            foreach (DataRow row in dtbillingInstruction.Rows)
            {
                lblJobRefNo.Text = row["JobRefNo"].ToString();
                lblRefNo.Text = row["JobRefNo"].ToString();
                lblShipmentType.Text = row["ShipmentType"].ToString();
                lblShipmentCate.Text = row["ShipmentCategory"].ToString();
            }
        }
    }

    protected void SpanVisible()
    {
        chkAgencyService.ClearSelection();
        chkOtherService.ClearSelection();
        txtOtherRemark.Text = "";
        txtOtherRemark1.Text = "";
        txtOtherRemark2.Text = "";
        txtOtherRemark3.Text = "";
        txtOtherServiceRemark.Text = "";
        txtAgencyServiceRemark.Text = "";
        //txtJobNumber.Text = "";
        //lblConsignee.Text = "";
        //lblCust.Text = "";
        lblShipmentCate.Text = "";
        lblShipmentType.Text = "";
        chkAgencyService.Enabled = true;
        chkOtherService.Enabled = true;
        txtOtherServiceRemark.Enabled = true;
        txtOtherRemark.Enabled = true;
        txtOtherRemark1.Enabled = true;
        txtOtherRemark2.Enabled = true;
        txtOtherRemark3.Enabled = true;
        FuInstructionCopy.Enabled = true;
        FuInstructionCopy1.Enabled = true;
        FuInstructionCopy2.Enabled = true;
        FuInstructionCopy3.Enabled = true;
    }

    private string UploadDoc(FileUpload fuDocument, string Filepath)
    {
        string FileName = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (Filepath == "")
            Filepath = "BillingInstruction" + Session["JobId"] + "\\";

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + Filepath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + Filepath;
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

            return Filepath + FileName;
        }

        else
        {
            return "";
        }
    }

    protected void Get_BillingInstruction(int JobId)
    {
        int strJobId = JobId;
        DataTable dtBillInstruction = new DataTable();
        dtBillInstruction = DBOperations.Get_BillingInstructionDetail(strJobId);
        if (dtBillInstruction.Rows.Count > 0)
        {
            dvBillInstruction.Visible = false;
            dvResult.Visible = true;
            btnSaveInstruction.Visible = false;

            foreach (DataRow rw in dtBillInstruction.Rows)
            {
                if (rw["AlliedAgencyService"].ToString() == "") { lblAlliedAgencyService.Text = "-"; }
                else
                {
                    string args = rw["AlliedAgencyService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblAlliedAgencyService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }

                if (rw["AlliedAgencyRemark"].ToString() == "") { lblAlliedAgencyRemark.Text = "-"; }
                else { lblAlliedAgencyRemark.Text = rw["AlliedAgencyRemark"].ToString(); };

                if (rw["OtherService"].ToString() == "") { lblOtherService.Text = "-"; }
                else
                {
                    string args = rw["OtherService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblOtherService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                if (rw["OtherServiceRemark"].ToString() == "") { lblOtherServiceRemark.Text = "-"; }
                else { lblOtherServiceRemark.Text = rw["OtherServiceRemark"].ToString(); }

                if (rw["InstructionCopy"].ToString() == "") { lnkInstructionCopy.Text = "-"; }
                else
                {
                    hdnInstructionCopy.Value = rw["InstructionCopy"].ToString();
                    lnkInstructionCopy.Text = FindFileName(rw["InstructionCopy"].ToString());
                }
                if (rw["InstructionCopy1"].ToString() == "") { lnkInstructionCopy1.Text = "-"; }
                else
                {
                    hdnInstructionCopy1.Value = rw["InstructionCopy1"].ToString();
                    lnkInstructionCopy1.Text = FindFileName(rw["InstructionCopy1"].ToString());
                }
                if (rw["InstructionCopy2"].ToString() == "") { lnkInstructionCopy2.Text = "-"; }
                else
                {
                    hdnInstructionCopy2.Value = rw["InstructionCopy2"].ToString();
                    lnkInstructionCopy2.Text = FindFileName(rw["InstructionCopy2"].ToString());
                }
                if (rw["InstructionCopy3"].ToString() == "") { lnkInstructionCopy3.Text = "-"; }
                else
                {
                    hdnInstructionCopy3.Value = rw["InstructionCopy3"].ToString();
                    lnkInstructionCopy3.Text = FindFileName(rw["InstructionCopy3"].ToString());
                }

                if (rw["Instruction"].ToString() == "") { lblInstruction.Text = "-"; }
                else { lblInstruction.Text = rw["Instruction"].ToString(); }
                if (rw["Instruction1"].ToString() == "") { lblInstruction1.Text = "-"; }
                else { lblInstruction1.Text = rw["Instruction1"].ToString(); }
                if (rw["Instruction2"].ToString() == "") { lblInstruction2.Text = "-"; }
                else { lblInstruction2.Text = rw["Instruction2"].ToString(); }
                if (rw["Instruction3"].ToString() == "") { lblInstruction3.Text = "-"; }
                else { lblInstruction3.Text = rw["Instruction3"].ToString(); }
            }
            ModalPopupInstruction.Show();
        }
        else
        {
            dvBillInstruction.Visible = true;
            dvResult.Visible = false;
            btnSaveInstruction.Visible = true;
            ModalPopupInstruction.Show();
        }

    }
    protected string FindFileName(string FilePath)
    {
        string[] args = FilePath.Split('\\');

        return args[2];
    }
    protected void lnkInstructionCopy1_Click(object sender, EventArgs e)
    {
        DownloadDocument(hdnInstructionCopy1.Value);
        ModalPopupInstruction.Show();
    }

    protected void lnkInstructionCopy2_Click(object sender, EventArgs e)
    {
        DownloadDocument(hdnInstructionCopy2.Value);
        ModalPopupInstruction.Show();
    }

    protected void lnkInstructionCopy3_Click(object sender, EventArgs e)
    {
        DownloadDocument(hdnInstructionCopy3.Value);
        ModalPopupInstruction.Show();
    }
    #endregion

    #region HOLD JOB EXPENSE

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        int JobId = 0;
        if (hdnJobId.Value != "")
        {
            JobId = Convert.ToInt32(hdnJobId.Value);
        }

        if (JobId != 0)
        {
            if (txtReason.Text != "")
            {
                string RejectType = "0";
                DropDownList ddlRejectType = (DropDownList)fvHoldJobDetail.FindControl("ddlReasonHold");
                RejectType = ddlRejectType.SelectedValue;

                if (RejectType != "0")
                {
                    int result = DBOperations.EX_AddHoldBillingAdvice(JobId, txtReason.Text.Trim(), RejectType, LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        fvHoldJobDetail.DataBind();
                        lblMessage.Text = "Successfully holded job no " + hdnJobRefNo.Value + ".";
                        lblMessage.CssClass = "success";
                        gvJobDetail.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System error. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError_HoldExp.Text = "Please Select Rejection Type.";
                    lblError_HoldExp.CssClass = "errorMsg";

                    rfvReason.Enabled = true;
                    mpeHoldExpense.Show();
                }
            }
            else
            {
                //lblError_HoldExp.Text = "Please enter reason.";
                //lblError_HoldExp.CssClass = "errorMsg";
                rfvReason.Enabled = true;
                mpeHoldExpense.Show();
            }
        }
    }

    #endregion

    #region Vendor Final Invoice

    private bool CheckFinalInvoicePending(int JobId)
    {
        bool isPending = false;

        // Check if any Vendor Final Invoice Pending for Submission against Proforma Invoice 

        DataSet dsPending = AccountExpense.CheckFinalInvoicePending("", JobId, 1);

        if (dsPending.Tables.Count > 0)
        {
            if (dsPending.Tables[0].Rows.Count > 0)
            {
                isPending = true;
            }
        }

        return isPending;

    }
    private bool CheckVendorInvoicePending(int JobId)
    {
        bool isPending = false;

        // Check if any Vendor Invoice Pending for Submission 

        DataSet dsPending = AccountExpense.CheckVendorInvoicePending(JobId, 1);

        if (dsPending.Tables.Count > 0)
        {
            if (dsPending.Tables[0].Rows.Count > 0)
            {
                isPending = true;
            }
        }
        return isPending;

    }
    #endregion

    #region Document
    protected void gvPCDDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocPath").ToString();

            if (strDocPath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");
                lnkDownload.Visible = false;
            }
        }
    }

    protected void gvPCDDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadBillingDocument(DocPath);
            ModalPopupDocument.Show();
        }
    }

    private void DownloadBillingDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == null)
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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

    #region Contract Billing
    DataTable dtInvoiceMstData = new DataTable();
    DataTable dtInvoiceDetData = new DataTable();
    protected void Get_JobDetailForContractBilling(int JobId)
    {
        int strJobId = JobId;
        DataSet dtbilling = DBOperations.EX_GetJobDetailForContractBilling(JobId);
        if (dtbilling.Tables[0].Rows.Count > 0)
        {
            dtStartDate.Text = dtbilling.Tables[0].Rows[0][1].ToString();
            dtEndDate.Text = dtbilling.Tables[0].Rows[0][2].ToString();
            txtconsigneename.Text = dtbilling.Tables[0].Rows[0][3].ToString();
            DataSet dtjobdetails = DBOperations.GetEXJobDetail_Users(JobId);
            if (dtjobdetails.Tables[0].Rows.Count > 0)
            {
                txtcustname.Text = dtjobdetails.Tables[0].Rows[0][0].ToString();
                txtbranch.Text = dtjobdetails.Tables[0].Rows[0][2].ToString();
                txtport.Text = dtjobdetails.Tables[0].Rows[0][3].ToString();
                txtcontainer.Text = dtjobdetails.Tables[0].Rows[0][4].ToString();
                txtpackages.Text = dtjobdetails.Tables[0].Rows[0][5].ToString();
                txtsum20.Text = dtjobdetails.Tables[0].Rows[0][6].ToString();
                txtsum40.Text = dtjobdetails.Tables[0].Rows[0][7].ToString();
                txtgrossweight.Text = dtjobdetails.Tables[0].Rows[0][8].ToString();
                txttypeboe.Text = dtjobdetails.Tables[0].Rows[0][9].ToString();
                txtdeliverytype.Text = dtjobdetails.Tables[0].Rows[0][10].ToString();
                txtjobtype.Text = dtjobdetails.Tables[0].Rows[0][11].ToString();
                txtContainerType.Text = dtjobdetails.Tables[0].Rows[0][12].ToString();
            }
        }
        dtInvoiceMstData = dtbilling.Tables[2];
        if (dtbilling.Tables[1].Rows.Count > 0)
        {
            grdbillinglinedetails.DataSource = dtbilling.Tables[1];
            grdbillinglinedetails.DataBind();
        }
        dtInvoiceDetData = dtbilling.Tables[1];
    }
    protected void Get_JobDetailForContractBillingUser(int JobId)
    {
        int strJobId = JobId;
        string ChargeCode = "", UOM = "";

        DataSet dtbilling = DBOperations.Ex_GetJobDetailForContractBillingUser(JobId, LoggedInUser.glEmpName, 1);
        if (dtbilling.Tables[0].Rows.Count > 0)
        {
            grdcontract_User.DataSource = dtbilling;
            grdcontract_User.DataBind();
            //grdcontract_User.Columns[2].HeaderText = "";
            //grdcontract_User.Columns[2].ItemStyle.Width = 1;
            btnAdduser.Visible = true;

            foreach (DataRow row in dtbilling.Tables[0].Rows)
            {
                ChargeCode = row["ChargeCode"].ToString();
                UOM = row["UOM"].ToString();

                if (ChargeCode == "A01" && UOM == "Per Container" && txtjobtype.Text == "Docs Stuff" && txtcontainer.Text == "0")
                {
                    Label1.Text = "Billing Advice not allow";
                    break;
                }
                else
                {
                    Label1.Text = "";
                }
            }
        }
        //else
        //{
        //    btnAdduser.Visible = false;
        //}
    }

    protected void btncloseContractBillingprint_Click(object sender, EventArgs e)
    {
        //ModalPopupContractBillingprint.Hide();
        divContractBillingPrint.Visible = false;
        Response.Redirect("~\\PCA\\PendingBillingAdvice.aspx");
        upShipment.Update();
        mainfield.Visible = true;
        pnlHoldExpense.Visible = true;

        //divContractBilling.Visible = true;
        //pnlFilter.Visible = true;
        //Panel2Document.Visible = true;
        //pnlBJVDetails.Visible = true;
        //pnlProforma.Visible = true;
        //PanelBillingInvoice.Visible = true;
        //PanelAddCustomInvoice.Visible = true;
        //PanelInstruction.Visible = true;
    }

    protected void btncloseContractBilling_Click(object sender, EventArgs e)
    {
        grdbillinglinedetails.DataSource = null;
        grdbillinglinedetails.DataBind();
        grdcontract_User.DataSource = null;
        grdcontract_User.DataBind();
        ModalPopupContractBilling.Hide();
    }

    protected void btnsaveContractBilling_Click(object sender, EventArgs e)
    {
        int x = 0;
        if (grdbillinglinedetails.Rows.Count > 0)
        {
            lblBillMsg.Text = "";
            // ModalPopupContractBilling.Show();
            btnAdduser_Click(sender, e);

            //grdbillinglinedetails.DataSource = null;
            //grdbillinglinedetails.DataBind();
            //grdcontract_User.DataSource = null;
            //grdcontract_User.DataBind();
            //ModalPopupContractBilling.Hide();
            //SaveBilling_Invoice();
        }
    }

    protected void btnAdduser_Click(object sender, EventArgs e)
    {
        try
        {
            IncludeUserCharges();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
    private void IncludeUserCharges()
    {
        try
        {
            int CurRow = 0;
            string sql = string.Empty;
            int chking1 = 0;
            string str = string.Empty;
            bool chkdata = false;
            string strname = string.Empty;
            Boolean chkrd = false;
            int chkrd1 = 0;
            foreach (GridViewRow gvrow in grdcontract_User.Rows)
            {
                RadioButtonList chk = (RadioButtonList)gvrow.FindControl("rdselect");
                if (chk.SelectedValue == "1" || chk.SelectedValue == "2")
                {
                    chkrd1 += 1;
                }
                if (grdcontract_User.Rows.Count == chkrd1)
                {
                    chkrd = true;
                }
            }
            if (chkrd == true)
            {
                foreach (GridViewRow gvrow in grdcontract_User.Rows)
                {
                    RadioButtonList chk = (RadioButtonList)gvrow.FindControl("rdselect");
                    HiddenField hdnlid = (HiddenField)gvrow.FindControl("hdnlid");
                    HiddenField hdnchargecode = (HiddenField)gvrow.FindControl("hdnchargecode");
                    TextBox txt1 = (TextBox)gvrow.FindControl("txtqty");
                    if (chk.SelectedValue == "")
                    {
                        chking1 = 0;
                    }
                    else { chking1 = 1; }
                    if (chking1 > 0)
                    {
                        if (chk.SelectedValue == "1" && (txt1.Text == "0" || txt1.Text == ""))
                        {
                            chkdata = false;
                            //string message = "Please enter Qty.......";
                            //string script = "window.onload = function(){ alert('";
                            //script += message;
                            //script += "')};";
                            //ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                            Label1.Text = "If  Q-SELECTION 'Yes' ? THEN PLEASE ENTER QUNTITY.";
                            break;
                        }
                        else
                        {
                            if (chking1 == 1 && ((chk.SelectedValue == "1" && (txt1.Text != "0" || txt1.Text != "")) || (chk.SelectedValue == "2" && (txt1.Text != "0" || txt1.Text != ""))))
                            {

                                Label1.Text = "";
                                btnsaveContractBilling.Enabled = true;
                                chkdata = true;
                                strname = txt1.Text;
                                strname = gvrow.Cells[0].Text;
                                int Jobid = Convert.ToInt32(Session["JobId"].ToString());
                                DataSet dtResult = DBOperations.UpdateCBBillingDetail(Convert.ToInt32(hdnlid.Value), Convert.ToInt32(Session["JobId"].ToString()), hdnchargecode.Value.ToString(), gvrow.Cells[0].Text.ToString(), Convert.ToInt32(txt1.Text), LoggedInUser.glUserId, Convert.ToInt32(chk.SelectedValue));
                                //if(dtResult.Tables[0].Rows.Count>=0)
                                //{
                                //    //success
                                //}
                                //DBOperations.DisconnectSQL(OpenDB);
                                //string IsGST = string.Empty;
                                //OpenDB.Open();
                                //SqlCommand command = new SqlCommand("[usp_Inserttbl_CBBilling_User]", OpenDB) { CommandType = CommandType.StoredProcedure };
                                //command.Parameters.Add("@lid", SqlDbType.VarChar).Value = Convert.ToInt32(hdnlid.Value);
                                //command.Parameters.Add("@jobid", SqlDbType.VarChar).Value = Session["JobId"].ToString();
                                ////command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = gvrow.Cells[1].Text.ToString();
                                //command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = hdnchargecode.Value.ToString();
                                //command.Parameters.Add("@chargename", SqlDbType.VarChar).Value = gvrow.Cells[0].Text.ToString();
                                //command.Parameters.Add("@qty", SqlDbType.VarChar).Value = Convert.ToInt32(txt1.Text);
                                //command.Parameters.Add("@userid", SqlDbType.VarChar).Value = LoggedInUser.glUserId;
                                //command.Parameters.Add("@status", SqlDbType.VarChar).Value = chk.SelectedValue;
                                //command.ExecuteNonQuery();
                                //DBOperations.DisconnectSQL(OpenDB);
                            }
                        }
                    }
                    //}
                }
                if (chkdata == true)
                {
                    Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));

                    Get_JobDetailForContractBillingUser(Convert.ToInt32(hdnJobId.Value));
                    grdbillinglinedetails.DataSource = null;
                    grdbillinglinedetails.DataBind();
                    grdcontract_User.DataSource = null;
                    grdcontract_User.DataBind();
                    ModalPopupContractBilling.Hide();
                    //ModalPopupContractBilling.Show();
                }
                else { ModalPopupContractBilling.Show(); }
            }
            else { Label1.Text = "Kindly Select All Charges!"; ModalPopupContractBilling.Show(); }
        }
        catch (Exception ex)
        {
            // MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected void CheckBox_Changed(object sender, EventArgs e)
    {
        RadioButtonList rbl = sender as RadioButtonList;
        GridViewRow row = rbl.NamingContainer as GridViewRow;
        TextBox txt = row.FindControl("txtqty") as TextBox;
        //TextBox txtjobtype = row.FindControl("txtjobtype") as TextBox;
        //TextBox txtcontainer = row.FindControl("txtcontainer") as TextBox;
        string txtuom = row.Cells[1].Text.ToString();
        Label1.Text = "";
        if (rbl.SelectedValue == "2")
        {
            //txt.Text = string.Empty;
            txt.Text = "0";
            txt.Enabled = false;
        }
        else if (rbl.SelectedValue == "1")
        {
            if (txtuom == "Per Container")
            {
                if (txtjobtype.Text == "Docs Stuff" && txtcontainer.Text == "0")
                {
                    txt.ReadOnly = false;
                }
                else
                {
                    txt.Text = txtcontainer.Text;
                    txt.ReadOnly = true;
                }

            }
            else if (txtuom == "Per Bill of Entry")
            {
                txt.Text = "1";
                txt.ReadOnly = true;
            }
            else
            {
                //txt.Text = "0";
                txt.Focus();
                txt.Enabled = true;
                txt.ReadOnly = false;
            }
        }
        Session["lkpost"] = "lkpost";
        ModalPopupContractBilling.Show();
    }
    ///

    protected void CheckBox1_Changed(object sender, EventArgs e)
    {
        Label1.Text = "";
        foreach (GridViewRow row in grdcontract_User.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox chkSelection = (row.Cells[0].FindControl("chkSelection") as CheckBox);
                RadioButtonList rdselect = (row.Cells[1].FindControl("rdselect") as RadioButtonList);
                TextBox txtqty = (row.Cells[2].FindControl("txtqty") as TextBox);
                if (txtqty.Text == "0" | txtqty.Text == "")
                {
                    //rdselect.SelectedValue = "2";
                }
                else { rdselect.SelectedValue = "1"; }
                ModalPopupContractBilling.Show();
            }
        }
    }

    protected void grdcontract_User_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rdselect");
            //TextBox txtqty1 = (TextBox)e.Row.FindControl("txtqty");
            //if (txtqty1.Text.ToString() == "0")
            //{
            //    rbl.SelectedIndex = 1;
            //}
            //rbl.SelectedIndex = 0;
            int uValue = (int)DataBinder.Eval(e.Row.DataItem, "userstatus");
            RadioButtonList rb = (RadioButtonList)e.Row.FindControl("rdselect");
            if (uValue > 0)
            {
                rb.Items.FindByValue(uValue.ToString()).Selected = true;
            }

            TextBox txt = (TextBox)e.Row.FindControl("txtqty");
            if (rb.SelectedValue == "2")
            {
                txt.Text = "0";
                txt.Enabled = false;
            }
            else if (rb.SelectedValue == "1")
            {
                txt.Enabled = true;
                txt.ReadOnly = false;
            }

        }

    }

    
    #endregion
}