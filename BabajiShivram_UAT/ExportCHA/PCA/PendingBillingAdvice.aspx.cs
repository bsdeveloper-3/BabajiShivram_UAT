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
public partial class PCA_PendingBillingAdvice : System.Web.UI.Page
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
        ScriptManager1.RegisterPostBackControl(btnSaveInstruction);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy1);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy2);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy3);
        ScriptManager1.RegisterPostBackControl(gvPCDDocument);

        if (!IsPostBack)
        {
            Session["JobId"] = null;
            ViewState["lblDEBITAMT"] = "0";

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
            Response.Redirect("PCABillingAdvice.aspx");
        }
        else if (e.CommandName.ToLower() == "sendforscrutiny")
        {
            //int JobId = Convert.ToInt32(e.CommandArgument);
            //SendDocumentForScrutiny(JobId);
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

            // Check PCD Document Required

            DataSet dsRequiredPCD = DBOperations.GetPCDRequiredDocument(JobId);
            
            if (dsRequiredPCD.Tables.Count > 0)
            {
                if (dsRequiredPCD.Tables[0].Rows.Count > 0)
                {
                    string strPCDDocName = "";

                    foreach (DataRow dr in dsRequiredPCD.Tables[0].Rows)
                    {
                        strPCDDocName += dr["sName"].ToString() + ",";
                    }

                    lblMessage.Text = "Please Upload Required Document - " + strPCDDocName;
                    lblMessage.CssClass = "errorMsg";

                    return;
                }
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
                        if (PCDDocId != 0)  /* && PCDDocId != 9*/
                        {
                            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                            DropDownList ddlLrPending = (DropDownList)row.FindControl("ddlLrPending");


                            if (ddlLrPending != null)
                            {
                                if (ddlLrPending.SelectedValue == "0")
                                {
                                    lblMessage.Text = "Select LR Pending!";
                                    lblMessage.CssClass = "errorMsg";
                                }
                                else
                                {
                                    //DataSet dsGetPCDDocs = DBOperations.GetUploadedPCDDocument(JobId);
                                    //if (dsGetPCDDocs != null && dsGetPCDDocs.Tables.Count > 0)
                                    //{
                                    if (ddlLrPending.SelectedValue == "1") // YES
                                    {
                                        int result = DBOperations.AddAdviceToLRPending(JobId, 0, LoggedInUser.glUserId);
                                        if (result == 0)
                                        {
                                            lblMessage.Text = "Job moved to LR Pending.";
                                            lblMessage.CssClass = "success";
                                        }
                                        else if (result == 2)
                                        {
                                            lblMessage.Text = "Job already added to LR Pending tab!";
                                            lblMessage.CssClass = "errorMsg";
                                        }
                                        else
                                        {
                                            lblMessage.Text = "System Error! Please try after sometime!";
                                            lblMessage.CssClass = "errorMsg";
                                        }
                                    }
                                    else // NO
                                    {
                                        SendDocumentForScrutiny(JobId);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Please upload Billing Advice Document";
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
            hdnBranchId.Value = "0";
            Session["BillingInstruction"] = "0";
            lblDocMsg.Text = "";
            ScriptManager1.RegisterPostBackControl(btnSaveDocument);

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            hdnJobId.Value = commandArgs[0].ToString();

            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";

            if (strJobFileDir.Contains("MBOI"))
            {
                hdnBranchId.Value = "3"; // Mumbai Sea
            }
            else if (strJobFileDir.Contains("MBAI"))
            {
                hdnBranchId.Value = "2"; // Mumbai AIR - Exclude
            }
            else
            {
                hdnBranchId.Value = "0";
            }

            //hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            //int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);

            //rptDocument.DataSource = DBOperations.FillPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            //rptDocument.DataBind();

            //ModalPopupDocument.Show();
            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);

            DataSet dsGetPCDDocs = DBOperations.FillPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            if (dsGetPCDDocs != null)
            {
                for (int i = 0; i < dsGetPCDDocs.Tables[0].Rows.Count; i++)
                {
                    if (dsGetPCDDocs.Tables[0].Rows[i]["lId"].ToString() == "9" || dsGetPCDDocs.Tables[0].Rows[i]["PCDDocId"].ToString() != "0")
                    {
                        if (dsGetPCDDocs.Tables[0].Rows[i]["lId"].ToString() == "9") // Lorry Receipts
                        {
                            dsGetPCDDocs.Tables[0].Rows[i].Delete();
                        }
                        else if (dsGetPCDDocs.Tables[0].Rows[i]["PCDDocId"].ToString() != "0")
                        {
                            dsGetPCDDocs.Tables[0].Rows[i].Delete();
                        }
                        else
                        {
                            lblDocMsg.Text = "Selected Document already uploaded";
                            lblDocMsg.CssClass = "success";
                        }

                    }

                }
            }
            rptDocument.DataSource = dsGetPCDDocs.Tables[0];
            rptDocument.DataBind();

            Session["JobId"] = commandArgs[0].ToString();
            gvPCDDocument.DataSource = PCDDocumentSqlDataSource;
            gvPCDDocument.DataBind();

            ModalPopupDocument.Show();

        }
        else if (e.CommandName.ToLower() == "showbjv")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobrefno = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strJobrefno = commandArgs[1].ToString();

            lbldiv.Text = "BJV Details - " + strJobrefno;
            ViewState["lblDEBITAMT"] = "0";
            rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(JobId));
            rptBJVDetails.DataBind();
            mpeBJVDetails.Show();
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
                hdnJobId_hold.Value = JobId.ToString();
                fvHoldJobDetail.DataBind();
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
                    int result = DBOperations.AddHoldBillingAdvice(JobId, "", "", LoggedInUser.glUserId);
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
        else if (e.CommandName.ToLower() == "instructionpopup" && e.CommandArgument != null)
        {
            hdnBranchId.Value = "0";
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

            AddCell(FIRST_EDITABLE_CELL + 8, "Instructions", e);
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
            LinkButton lnkJobNo = (LinkButton)gvJobDetail.Rows[row].Cells[6].FindControl("lnkJobNo");
            //get the new value
            TextBox txt = (TextBox)gvJobDetail.Rows[row].Cells[cell].FindControl(txtName);
            //update the label, if data are not re-bounded
            Label lab = (Label)gvJobDetail.Rows[row].Cells[cell].FindControl(labName);
            lab.Text = txt.Text;

            string strJobRefno = lnkJobNo.Text.Trim();

            int result = DBOperations.UpdateBillingInstructions(JobId, txt.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                lblMessage.Text = "Billing Instructions Updated!";
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
            RequiredFieldValidator RFVFileUpload = (RequiredFieldValidator)e.Item.FindControl("RFVFile");

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
                // File Upload Required
                
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "');");

                //chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "');");

                //if (hdnBranchId.Value == "3" || hdnBranchId.Value == "2")// Mumbai AIR/Sea - File Upload Not Required
                //{
                //    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "');");
                //}
                //else
                //{
                //    chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "');");
                //}

                //chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + CVCheckBoxList.ClientID + "','" + RFVFileUpload.ClientID + "','" + "');");

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
        if (Session["BillingInstruction"].ToString() == "1")
        {
            lblBillMsg.Text = "Document List Updated For Billing Advice.";
            lblBillMsg.CssClass = "success";
            ModalPopupInstruction.Show();
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

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        // ModalPopupDocument.Hide();
        if (Session["BillingInstruction"].ToString() == "1")
        {
            ModalPopupDocument.Hide();
            ModalPopupInstruction.Show();
        }
        else
        {
            ModalPopupDocument.Hide();
        }

    }

    private void SendDocumentForScrutiny(Int32 JobId)
    {
      //  int result_LR = DBOperations.AddAdviceToLRPending(JobId, 2, LoggedInUser.glUserId);

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

                gvProformaDetail.DataSource = dsPending;
                gvProformaDetail.DataBind();

                ModalPopupProforma.Show();
            }
        }
        return isPending;

    }
    #endregion

    #region HOLD JOB EXPENSE

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        int JobId = 0;
        if (hdnJobId_hold.Value != "")
        {
            JobId = Convert.ToInt32(hdnJobId_hold.Value);
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
                    int result = DBOperations.AddHoldBillingAdvice(JobId, txtReason.Text.Trim(), RejectType, LoggedInUser.glUserId);
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

    #region Print BJV Expense

    protected void btnPrintExpense_Click(object sender, EventArgs e)
    {
        int i = 0;
        string strJobList = "";
        //gvRecievedJobDetail.AllowPaging = false;//Checkbox
        //gvRecievedJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkPrint")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");

                string[] commmandArgs = Recv.CommandArgument.ToString().Split(new char[] { ';' });

                string JobId = commmandArgs[0].ToString();

                strJobList = strJobList + JobId + ",";

                i++;
            }
            else
            {
                if (i == 0)
                {
                    lblMessage.Text = "Please Checked atleast 1 checkbox to Print BJV detail.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }

        if (strJobList != "")
        {
            Session["BJVJobList"] = strJobList;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BJV PRINT", "window.open('PrintBJV.aspx');", true);

        }
    }

    #endregion

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
        gvJobDetail.Columns[2].Visible = false; // Check Box Print BJV
        gvJobDetail.Columns[3].Visible = false; // LR Pending Drop down
        gvJobDetail.Columns[4].Visible = false; // hold imagebutton
        gvJobDetail.Columns[5].Visible = false; // Billing Instruction
        //gvJobDetail.Columns[7].Visible = false; // bjv link button
        //gvJobDetail.Columns[8].Visible = false;  // Link Button job Ref NO
        //gvJobDetail.Columns[9].Visible = true;  // Bound Field Job Ref NO
        gvJobDetail.Columns[10].Visible = false; // bjv link button
        gvJobDetail.Columns[11].Visible = false; // Job link button
        gvJobDetail.Columns[12].Visible = true; // Job link button
        gvJobDetail.Columns[18].Visible = false; // Billing Documents
        gvJobDetail.Columns[25].Visible = false;  // Send Document For Scrutiny Link Button
        //gvJobDetail.Columns[16].Visible = false;  // Billling Document Link Button
        //gvJobDetail.Columns[18].Visible = false;  // Date on Job Forwarded For Billing Scrutiny column
        //gvJobDetail.Columns[19].Visible = false;  // Rejection Date
        //gvJobDetail.Columns[20].Visible = false;  // Rejection Remark
        //gvJobDetail.Columns[21].Visible = false;  // Rejected By

        //gvJobDetail.Caption = "Pending Billing Advice On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingPCDBilling.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion


    protected void btnSaveInstruction_Click(object sender, EventArgs e)
    {
        
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

        result = DBOperations.BS_AddBillingInstruction(JobId, 0, AlliedAgencyService, AlliedAgencyRemark, 0, "",
            0, "", "", 0, "", "", WithoutLRStatus,
            "", "", Instruction, Instruction1, Instruction2, Instruction3,
            InstructionCopy, InstructionCopy1, InstructionCopy2, InstructionCopy3, OtherService, OtherServiceRemark, LoggedInUser.glUserId);
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

    protected void ImgInstructionClose_Click(object sender, ImageClickEventArgs e)
    {
        ModalPopupInstruction.Hide();
    }

    protected void btnCanInstruction_Click(object sender, EventArgs e)
    {
        ModalPopupInstruction.Hide();
    }

    protected void lnkBillingDocument_Click(object sender, EventArgs e)
    {
        Session["BillingInstruction"] = "1";
        gvPCDDocument.DataSource = PCDDocumentSqlDataSource;
        gvPCDDocument.DataBind();
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);

        DataSet dsGetPCDDocs = DBOperations.FillPCDDocumentForWorkFlow(Convert.ToInt32(hdnJobId.Value), PCDDocType);
        if (dsGetPCDDocs != null)
        {
            for (int i = 0; i < dsGetPCDDocs.Tables[0].Rows.Count; i++)
            {
                if (dsGetPCDDocs.Tables[0].Rows[i]["lId"].ToString() == "9" || dsGetPCDDocs.Tables[0].Rows[i]["PCDDocId"].ToString() != "0")
                {
                    if (dsGetPCDDocs.Tables[0].Rows[i]["lId"].ToString() == "9") // Lorry Receipts
                    {
                        dsGetPCDDocs.Tables[0].Rows[i].Delete();
                    }
                    else if (dsGetPCDDocs.Tables[0].Rows[i]["PCDDocId"].ToString() != "0")
                    {
                        dsGetPCDDocs.Tables[0].Rows[i].Delete();
                    }
                    else
                    {
                        lblDocMsg.Text = "Selected Document already uploaded";
                        lblDocMsg.CssClass = "success";
                    }

                }
            }
        }
        rptDocument.DataSource = dsGetPCDDocs.Tables[0];
        rptDocument.DataBind();

        ModalPopupDocument.Show();

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
                else { lnkInstructionCopy.Text = rw["InstructionCopy"].ToString(); }
                if (rw["InstructionCopy1"].ToString() == "") { lnkInstructionCopy1.Text = "-"; }
                else { lnkInstructionCopy1.Text = rw["InstructionCopy1"].ToString(); }
                if (rw["InstructionCopy2"].ToString() == "") { lnkInstructionCopy2.Text = "-"; }
                else { lnkInstructionCopy2.Text = rw["InstructionCopy2"].ToString(); }
                if (rw["InstructionCopy3"].ToString() == "") { lnkInstructionCopy3.Text = "-"; }
                else { lnkInstructionCopy3.Text = rw["InstructionCopy3"].ToString(); }
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

    protected void lnkInstructionCopy_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy.Text);
    }

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
            DownloadDocument(DocPath);
            ModalPopupDocument.Show();
        }
    }

    protected void lnkInstructionCopy1_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy1.Text);
    }

    protected void lnkInstructionCopy2_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy2.Text);
    }

    protected void lnkInstructionCopy3_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy3.Text);
    }
}
