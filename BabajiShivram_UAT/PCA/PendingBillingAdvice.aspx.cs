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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

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
            int count = 0;

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
                lblMessage.Text = "Please Complete Audit L2 Or Cancel Vendor Payment Invoice On Ops Accounting Module!";
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

                    if (PCDDocId != 0)
                    {
                        count = count + 1;
                    }
                    lblMessage.Text = "@@@"+count;
                }
                    if (count != 0)  /* && PCDDocId != 9*/
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
                            
                            if (ddlLrPending.SelectedValue == "Yes") // YES
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
                            }
                        }
                    }
                else
                {
                    lblMessage.Text = "Please upload Billing Advice Document";
                    lblMessage.CssClass = "errorMsg";
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
            mpeHoldExpense.Show();
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
        /// swamini 27 Jan 2023
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
                PrintBill(commandArgs[2].ToString());
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

                    ReportDocument cryRpt = new ReportDocument();
                    cryRpt.Load(Server.MapPath("\\CrystalReports\\rpt_Bill.rpt"));
                    cryRpt.SetDatabaseLogon("sa", "sa", "192.168.6.98", "ContractDBTest");
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
                int count1 = DBOperations.Get_JobDetailForContratcBillingByJobIdUser_Checking(Convert.ToInt32(hdnJobId.Value));
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

                //if (count1 == 3)
                //{
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Already Generate  Invoice........');", true);
                //    string message = "Already Generate  Invoice........";
                //    string script = "window.onload = function(){ alert('";
                //    script += message;
                //    script += "')};";
                //    ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                //}
                //else if (count1 == 0)
                //{
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('No selected User Details......');", true);
                //    string message = "No selected User Details......";
                //    string script = "window.onload = function(){ alert('";
                //    script += message;
                //    script += "')};";
                //    ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                //}
                //else if (count1 == 1 || count1 == 2)
                //{

                //    Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));

                //    if (grdbillinglinedetails.Rows.Count > 0)
                //    {
                //        Get_JobDetailForContractBillingUser(Convert.ToInt32(hdnJobId.Value));
                //    }
                //    if (grdbillinglinedetails.Rows.Count == 0)
                //    {
                //        btnsaveContractBilling.Enabled = false;
                //    }
                //    else
                //    {
                //        btnsaveContractBilling.Enabled = true;
                //    }
                //    //if (grdcontract_User.Rows.Count == 0)
                //    //{
                //    //    btnAdduser.Visible = false;
                //    //}
                //    //else
                //    //{
                //    //    btnAdduser.Visible = true;
                //    //}
                //    btnAdduser.Visible = false;
                //    grdbillinglinedetails.Visible = false;
                //    //ModalPopupContractBilling.Show();
                //    ModalPopupContractBilling.Show();
                //}
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
            PrintBill(commandArgs[2].ToString());
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

                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(Server.MapPath("\\CrystalReports\\rpt_Bill.rpt"));
                //CrystalReportViewer1.ReportSource = cryRpt;
                divContractBillingPrint.Visible = true;
                mainfield.Visible = false;
            }
            catch (Exception ex)
            {
                //    //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        ////
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

            //LinkButton lnkbiliing = (LinkButton)e.Row.FindControl("lnkbiliing");
            //if (lnkbiliing.Text == "1")
            //{
            //    lnkbiliing.Text = "Billing";
            //    lnkbiliing.Enabled = false;
            //}
            HiddenField hdnJOBID = (HiddenField)e.Row.FindControl("hdnJOBID");
            HiddenField statuscontract = (HiddenField)e.Row.FindControl("statuscontract");
            
            DataSet dtbilling = DBOperations.GetJobDetailForContractBillingUser(Convert.ToInt32(hdnJOBID.Value), loggedInUser.glEmpName, 1);
            
            // Check IF Customer Contract Exists

            //DataSet dtbilling = DBOperations.CheckCustomerContractActive(Convert.ToInt32(hdnJOBID.Value), 1);
            
            int hdnot = 0;
            if (dtbilling.Tables[0].Rows.Count > 0)
            {
                //if(hdnJobId.Value==dtbilling.Tables[0].Rows[0][]
                //hdnot = int.Parse(CDatabase.ResultInString("select count(*) Value from temp_Usercount where JobId=" + hdnJOBID.Value + " and Quantity>0 "));
                //if (hdnot == 0)
                //{
                //    hdnot = -1;
                //}
                //else if (hdnot > 0) { hdnot = 1; }
                // hdnot = DBOperations.GetUserCount(Convert.ToInt32(hdnJOBID.Value));
                if (dtbilling.Tables[1].Rows[0][0].ToString() == "1")
                {
                    hdnot = 1;
                }
                else { hdnot = 0; }
            }
            else if (dtbilling.Tables[0].Rows.Count == 0)
            {
                hdnot =2; //int.Parse(CDatabase.ResultInString("select count(*) Value from temp_Usercount where JobId='" + hdnJOBID.Value + "' and Quantity=0 "));
            }
            //HiddenField hdnot = (HiddenField)e.Row.FindControl("hdnot");
            LinkButton lnkScrutiny1 = (LinkButton)e.Row.FindControl("lnkScrutiny");
            LinkButton lnkotherinstru = (LinkButton)e.Row.FindControl("lnkotherinstru");
            //if (hdnot > 1)
            //{
            //    lnkScrutiny1.Enabled = true;
            //}
            //else if (hdnot == -1) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = false; }
            //else if (hdnot == 0) { lnkotherinstru.Enabled = false; lnkScrutiny1.Enabled = true; }
            //Get_JobDetailForContractBilling(Convert.ToInt32(hdnJOBID.Value));
            if (dtbilling.Tables[2].Rows[0][0].ToString() == "No Contract")
            {
                lnkotherinstru.Enabled = false;
                lnkScrutiny1.Enabled = true;
            }
            else if (dtbilling.Tables[2].Rows[0][0].ToString() == "Contract")
            {
                if (hdnot == 0) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = false; }
                else if (hdnot == 1) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = true; }
                //if (grdbillinglinedetails.Rows.Count == 0)
                //{
                //    lnkotherinstru.Enabled = false; lnkScrutiny1.Enabled = true;
                //}
                //else
                //{
                if (hdnot == 0) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = false; }
                else if (hdnot == 1) { lnkotherinstru.Enabled = true; lnkScrutiny1.Enabled = true; }
                else if (hdnot == 2) { lnkotherinstru.Enabled = false; lnkScrutiny1.Enabled = true; }
                //}


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

                //gvProformaDetail.DataSource = dsPending;
                //gvProformaDetail.DataBind();

                //ModalPopupProforma.Show();
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
            if (Session["lkpost"] != "lkpost")
            {
                DataFilter1_OnDataBound();
            }
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
        gvJobDetail.Columns[6].Visible = true; // Job Number
        //gvJobDetail.Columns[7].Visible = false; // bjv link button
        //gvJobDetail.Columns[8].Visible = false;  // Link Button job Ref NO
        //gvJobDetail.Columns[9].Visible = true;  // Bound Field Job Ref NO
        
        gvJobDetail.Columns[11].Visible = false; // bjv link button
        gvJobDetail.Columns[12].Visible = false; // Job link button
        gvJobDetail.Columns[20].Visible = false; // Billing Documents
        gvJobDetail.Columns[26].Visible = false;  // Send Document For Scrutiny Link Button
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


    #region ContractBilling
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
                                DBOperations.DisconnectSQL(OpenDB);
                                string IsGST = string.Empty;
                                OpenDB.Open();
                                SqlCommand command = new SqlCommand("[usp_Inserttbl_CBBilling_User]", OpenDB) { CommandType = CommandType.StoredProcedure };
                                command.Parameters.Add("@lid", SqlDbType.VarChar).Value = Convert.ToInt32(hdnlid.Value);
                                command.Parameters.Add("@jobid", SqlDbType.VarChar).Value = Session["JobId"].ToString();
                                //command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = gvrow.Cells[1].Text.ToString();
                                command.Parameters.Add("@chargecode", SqlDbType.VarChar).Value = hdnchargecode.Value.ToString();
                                command.Parameters.Add("@chargename", SqlDbType.VarChar).Value = gvrow.Cells[0].Text.ToString();
                                command.Parameters.Add("@qty", SqlDbType.VarChar).Value = Convert.ToInt32(txt1.Text);
                                command.Parameters.Add("@userid", SqlDbType.VarChar).Value = LoggedInUser.glUserId;
                                command.Parameters.Add("@status", SqlDbType.VarChar).Value = chk.SelectedValue;
                                command.ExecuteNonQuery();
                                DBOperations.DisconnectSQL(OpenDB);
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

    #region ContractBillingpopup 
    // create invoice master and details table
    DataTable dtInvoiceMstData = new DataTable();
    DataTable dtInvoiceDetData = new DataTable();
    protected void Get_JobDetailForContractBilling(int JobId)
    {
        int strJobId = JobId;
        DataSet dtbilling = DBOperations.GetJobDetailForContractBilling(JobId);
        if (dtbilling.Tables[0].Rows.Count > 0)
        {
            dtStartDate.Text = dtbilling.Tables[0].Rows[0][1].ToString();
            dtEndDate.Text = dtbilling.Tables[0].Rows[0][2].ToString();
            txtconsigneename.Text = dtbilling.Tables[0].Rows[0][3].ToString();
            DataSet dtjobdetails = DBOperations.GetJobDetail_Users(JobId);
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
    LoginClass loggedInUser = new LoginClass();
    protected void Get_JobDetailForContractBillingUser(int JobId)
    {
        int strJobId = JobId;

        DataSet dtbilling = DBOperations.GetJobDetailForContractBillingUser(JobId, loggedInUser.glEmpName, 1);
        if (dtbilling.Tables[0].Rows.Count > 0)
        {
            grdcontract_User.DataSource = dtbilling;
            grdcontract_User.DataBind();
            //grdcontract_User.Columns[2].HeaderText = "";
            //grdcontract_User.Columns[2].ItemStyle.Width = 1;
            btnAdduser.Visible = true;
        }
        //else
        //{
        //    btnAdduser.Visible = false;
        //}
    }
    protected void btncloseContractBilling_Click(object sender, EventArgs e)
    {
        grdbillinglinedetails.DataSource = null;
        grdbillinglinedetails.DataBind();
        grdcontract_User.DataSource = null;
        grdcontract_User.DataBind();
        ModalPopupContractBilling.Hide();
    }


    // Save Invoice Billing 
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
    // Genrate Invoice PDF and download PDF
    SqlConnection OpenDB = CDatabase.getConnection();
    protected void SaveBilling_Invoice(int jobid)
    {
        try
        {
            hdnJobId.Value = jobid.ToString();
            Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));
            if (dtInvoiceMstData.Rows.Count > 0)
            {
                for (int i = 0; i < dtInvoiceMstData.Rows.Count; i++)
                {
                    DBOperations.DisconnectSQL(OpenDB);
                    string IsGST = string.Empty;
                    OpenDB.Open();
                    SqlCommand cmd = new SqlCommand("[usp_Inserttbl_InvoiceMst]", OpenDB) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@InvId", null);
                    cmd.Parameters.AddWithValue("@InvoiceNo", dtInvoiceMstData.Rows[i]["InvoiceNo"].ToString());
                    //cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["InvoiceDate"].ToString()).ToString("dd/MMM/yyyy"));;
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoiceMstData.Rows[i]["InvoiceDate"].ToString());
                    cmd.Parameters.AddWithValue("@ConsigneeName", dtInvoiceMstData.Rows[i]["ConsigneeName"].ToString());
                    cmd.Parameters.AddWithValue("@JobNo", dtInvoiceMstData.Rows[i]["JobNo"].ToString());
                    cmd.Parameters.AddWithValue("@BLNo", dtInvoiceMstData.Rows[i]["BL No"].ToString());
                    //cmd.Parameters.AddWithValue("@BLDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["BL Date"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@BLDate", dtInvoiceMstData.Rows[i]["BL Date"].ToString());
                    cmd.Parameters.AddWithValue("@ShipperName", dtInvoiceMstData.Rows[i]["ShipperName"].ToString());
                    cmd.Parameters.AddWithValue("@Mode", dtInvoiceMstData.Rows[i]["Mode"].ToString());
                    //cmd.Parameters.AddWithValue("@DispatchDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["DispatchDate"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@DispatchDate", dtInvoiceMstData.Rows[i]["DispatchDate"].ToString());
                    cmd.Parameters.AddWithValue("@ContainerType", dtInvoiceMstData.Rows[i]["ContainerType"].ToString());
                    cmd.Parameters.AddWithValue("@TotalQty", dtInvoiceMstData.Rows[i]["TotalQty"].ToString());
                    cmd.Parameters.AddWithValue("@TotalAmt", dtInvoiceMstData.Rows[i]["TotalAmt"].ToString());
                    cmd.Parameters.AddWithValue("@GSTAmt", dtInvoiceMstData.Rows[i]["GSTAmt"].ToString());
                    cmd.Parameters.AddWithValue("@PayableAmt", dtInvoiceMstData.Rows[i]["PayableAmt"].ToString());
                    cmd.Parameters.AddWithValue("@Job_Type", dtInvoiceMstData.Rows[i]["Job_Type"].ToString());
                    cmd.Parameters.AddWithValue("@BE_No", dtInvoiceMstData.Rows[i]["BE_No"].ToString());
                    //cmd.Parameters.AddWithValue("@BE_Date", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["BE_Date"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@BE_Date", dtInvoiceMstData.Rows[i]["BE_Date"].ToString());
                    cmd.Parameters.AddWithValue("@Port_Of_Discharge", dtInvoiceMstData.Rows[i]["Port_Of_Discharge"].ToString());
                    cmd.Parameters.AddWithValue("@InvoiceNumber", dtInvoiceMstData.Rows[i]["NumericInvNo"].ToString());
                    cmd.Parameters.AddWithValue("@AddedBy", "1");
                    cmd.Parameters.AddWithValue("@AddedOn", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@ModifiedBy", "1");
                    cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.ExecuteNonQuery();
                    DBOperations.DisconnectSQL(OpenDB);

                    if (dtInvoiceMstData.Rows[i]["ContractLID"].ToString() != "")
                    {
                        var var1 = dtInvoiceMstData.Rows[i]["ContractLID"].ToString().Split('-');
                        for (int j = 0; j < var1.Length; j++)
                        {
                            string sql = "update CB_BillingDetail set IsBillDone = 'Y' where CMID = '" + var1[j].ToString() + "'";
                            DBOperations.InsertDeleteCommand(sql);
                        }
                    }
                }

                for (int i = 0; i < dtInvoiceDetData.Rows.Count; i++)
                {
                    DBOperations.DisconnectSQL(OpenDB);
                    OpenDB.Open();
                    SqlCommand cmd = new SqlCommand("[usp_Inserttbl_InvoiceDet]", OpenDB) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@InvDetId", null);
                    cmd.Parameters.AddWithValue("@InvoiceNo", dtInvoiceDetData.Rows[i]["InvoiceNo"].ToString());
                    //cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(dtInvoiceDetData.Rows[i]["InvoiceDate"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoiceDetData.Rows[i]["InvoiceDate"].ToString());
                    cmd.Parameters.AddWithValue("@chargecode", dtInvoiceDetData.Rows[i]["chargecode"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Particulars", dtInvoiceDetData.Rows[i]["Particulars"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Rate", dtInvoiceDetData.Rows[i]["Rate"].ToString());
                    cmd.Parameters.AddWithValue("@Qty", dtInvoiceDetData.Rows[i]["Qty"].ToString());
                    cmd.Parameters.AddWithValue("@GST", dtInvoiceDetData.Rows[i]["GST"].ToString());
                    cmd.Parameters.AddWithValue("@Amt", dtInvoiceDetData.Rows[i]["Amt"].ToString());
                    cmd.Parameters.AddWithValue("@AddedBy", "1");
                    cmd.Parameters.AddWithValue("@AddedOn", Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@ModifiedBy", "1");
                    cmd.Parameters.AddWithValue("@ModifiedOn", Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@AmountWithOutGST", dtInvoiceDetData.Rows[i]["AmountWithoutGST"].ToString());
                    cmd.Parameters.AddWithValue("@cblid", dtInvoiceDetData.Rows[i]["cblid"].ToString());
                    cmd.ExecuteNonQuery();
                    DBOperations.DisconnectSQL(OpenDB);
                }
                //MessageBox.Show("Data saved successfully !", "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdbillinglinedetails.DataSource = null;
                grdcontract_User.DataSource = null;
                dtInvoiceDetData.Rows.Clear();
                dtInvoiceDetData.Columns.Clear();
                dtInvoiceMstData.Rows.Clear();
                dtInvoiceMstData.Columns.Clear();
                ModalPopupContractBilling.Hide();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('Data saved successfully !');", true);
                string message = "Your details have been saved successfully.";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "')};";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Data Not Saved !');", true);
                string message = "Data Not Saved !";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "')};";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SaveData", "1", ex.Message);
            //string message = "Data Not Saved !";
            string script = "window.onload = function(){ alert('";
            script += ex.Message + Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
            script += "')};";
            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected void GenerateInvoicePDF(object sender, EventArgs e)
    {
        //Dummy data for Invoice (Bill).
        //string companyName = "ASPSnippets";
        //int orderNo = 2303;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        dt = dsInv.Tables[0].Copy();
        dt1 = dsInv.Tables[0].Copy();
        int desiredSize = 0;
        int desiredSize1 = 0;
        while (dt1.Columns.Count > desiredSize)
        {
            if (desiredSize1 <= 11)
            {
                dt1.Columns.RemoveAt(desiredSize);
            }
            else { break; }
            desiredSize1 += 1;
        }
        desiredSize = dt1.Columns.Count;
        desiredSize1 = 0;
        while (desiredSize1 <= desiredSize)
        {
            if (dt1.Columns[desiredSize1].ToString() == "Rate" || dt1.Columns[desiredSize1].ToString() == "Qty" || dt1.Columns[desiredSize1].ToString() == "TotalAmt")
            {

                if (dt1.Columns[desiredSize1].ToString() == "TotalAmt")
                {
                    dt1.Columns.RemoveAt(5);
                    dt1.Columns.RemoveAt(6 - 1);
                    dt1.Columns.RemoveAt(7 - 2);
                }
                dt1.Columns.RemoveAt(desiredSize1);
                desiredSize1 -= 1;
            }
            else if (dt1.Columns[desiredSize1].ToString() == "GSTAmt" && dt1.Columns[desiredSize1].ToString() == "PayableAmt" && dt1.Columns[desiredSize1].ToString() == "NoOfPackages")
            {
            }
            desiredSize1 += 1;
            desiredSize -= 1;
        }
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                StringBuilder sb = new StringBuilder();

                //Generate Invoice (Bill) Header.
                sb.Append("<table width='100%' border = '0' cellspacing='0' cellpadding='4' style='font-size:large;'>");
                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Babaji Shivram Clearing & Carriers Pvt Ltd.</b></td></tr>");
                //sb.Append("<tr><td colspan = '2'></td></tr>");
                sb.Append("<tr><td'>--------------------------------------------------------------------------------------------------------------------------</td></tr>");
                sb.Append("</table>");
                sb.Append("<br />");

                sb.Append("<table width='100%' border = '0' style='font-size:small; align - items:center;>");

                sb.Append("<tr style='border-top:soild;'><td style='width: 2500px;'><b>Job No: </ b>");
                sb.Append(dsInv.Tables[0].Rows[0][0].ToString());
                sb.Append("</td><td></td><td align = 'left'><b>BL No: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][1].ToString());
                sb.Append(" </td></tr>");

                sb.Append("<tr><td style='width: 300px;'><b>Shipment No: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][4].ToString());
                sb.Append("</td><td></td><td align = 'left'><b>BL Date: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][2].ToString());
                sb.Append(" </td></tr>");

                sb.Append("<tr><td colspan='2'><b>Consignee Name: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][3].ToString());
                sb.Append("</td><td align = 'left'><b>Shipper Name: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][4].ToString());
                sb.Append(" </td></tr>");

                sb.Append("<tr><td><b>Mode: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][5].ToString());
                sb.Append("</td><td></td><td align = 'left'><b>Dispatch Date: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][6].ToString());
                sb.Append(" </td></tr>");

                sb.Append("<tr><td><b>Container Type: </ b>");
                sb.Append(dsInv.Tables[0].Rows[0][8].ToString());
                sb.Append("</td><td></td><td align = 'left'><b>Port Of Discharge: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][11].ToString());
                sb.Append(" </td></tr>");

                sb.Append("<tr><td><b>BE No.: </ b>");
                sb.Append(dsInv.Tables[0].Rows[0][9].ToString());
                sb.Append("</td><td></td><td align = 'left'><b>BE Date.: </b>");
                sb.Append(dsInv.Tables[0].Rows[0][10].ToString());
                sb.Append(" </td></tr>");
                sb.Append("<tr><td colspan = '2'></td></tr>");

                sb.Append("</table>");
                sb.Append("<br />");
                //style='width: 10px; '
                //Generate Invoice (Bill) Items Grid.
                //sb.Append("<table cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:Arial'>");
                sb.Append("<table width='100%'  border='1' style='border: 1px solid #ccc;font-size: 9pt;font-family:Arial'>");

                //sb.Append("<table width='100%'  border='1' style='font-size:small; '>");
                //sb.Append("<center><tr>");
                //sb.Append("<th style='width: 5%;'><b>Sr. No.</b></th>");
                //sb.Append("<th  ><b>Particular</b></th>");
                //sb.Append("<th ><b>Taxable Value</b></th>");
                //sb.Append("<th ><b>GST</b></th>");
                //sb.Append("<th ><b>Total Amount</b></th></tr><tr>");
                //sb.Append("<table border = '0' frame='border'><tr>");
                int srno = 1;

                sb.Append("<tr>");
                foreach (DataColumn column in dt1.Columns)
                {
                    if (column.ColumnName == "Particulars")
                    {
                        sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ff0066'>" + column.ColumnName + "</th>");
                    }
                    else
                    {
                        sb.Append("<th style='background-color: #B8DBFD;border: 2px solid #bcc'>" + column.ColumnName + "</th>");
                    }
                }
                sb.Append("</tr>");


                //Adding DataRow.
                foreach (DataRow row in dt1.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt1.Columns)
                    {
                        sb.Append("<td style='width:100px;border: 1px solid #ccc'>" + row[column.ColumnName].ToString() + "</td>");
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</table>");

                //foreach (DataRow row in dt1.Rows)
                //{
                //    sb.Append("<td style='width:100px;border: 1px solid #ccc'> ");
                //    sb.Append(srno);
                //    sb.Append("</td>");
                //    sb.Append("<td >");
                //    sb.Append(row[0]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right'>");
                //    sb.Append(row[1]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right'>");
                //    sb.Append(row[2]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right'>");
                //    sb.Append(row[3]);
                //    sb.Append("</td>");
                //    srno += 1;
                //    //}
                //}
                //foreach (DataRow row in dt1.Rows)
                //{ 
                //    sb.Append("<td align='center' style='border - bottom - style: hidden; border - top - style: hidden;'> ");
                //    sb.Append(srno);
                //    sb.Append("</td>");
                //    sb.Append("<td  style='border - bottom - style: hidden; border - top - style: hidden;'>");
                //    sb.Append(row[0]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right' style='border - bottom - style: hidden; border - top - style: hidden;'>");
                //    sb.Append(row[1]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right' style='border - bottom - style: hidden;'>");
                //    sb.Append(row[2]);
                //    sb.Append("</td>");
                //    sb.Append("<td  align = 'right' style='border - bottom - style: hidden;'>");
                //    sb.Append(row[3]);
                //    sb.Append("</td>");
                //    srno += 1;
                //    //}
                //}
                //sb.Append("</tr></center>");
                //sb.Append("<tr><td align = 'right' ");
                //sb.Append(dt1.Columns.Count - 1);
                //sb.Append("> </td>");
                //sb.Append("<td align = 'right'");
                //sb.Append(dt1.Columns.Count - 1);
                //sb.Append("> </td>");
                //sb.Append("<td align = 'right' >");
                //sb.Append(dt.Compute("sum(NetAmt)", ""));
                //sb.Append("</td>");
                //sb.Append("<td align = 'right' >");
                //sb.Append(dt.Compute("sum(GST)", ""));
                //sb.Append("</td>");
                //sb.Append("<td align = 'right' >");
                //sb.Append(dt.Compute("sum(AmtWithGST)", ""));
                //sb.Append("</td>");
                //sb.Append("</tr></table>");

                string word = ConvertNumbertoWords(Convert.ToInt32(dt.Compute("sum(AmtWithGST)", "")));
                sb.Append("<table border=0>");
                sb.Append("<tr><td align = 'left'>                                                             </ td></tr>");
                sb.Append("</table>");

                sb.Append("<table border=1 style='font-size:small;'>");
                sb.Append("<tr><td align = 'left'>Amount Due : " + word + "</ td></tr>");
                sb.Append("</table>");

                sb.Append("<table border=0 style='font-size:small;'>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td>* All disputes are subject to Mumbai jurisdiction. </td></tr>");
                sb.Append("<tr><td>* Interest @ 24% per annum will be charged on bills not settled as per the agreed payment norms. </td></tr>");
                sb.Append("<tr><td>* All the payments should be enclosed with the detailed payment advice facilitate us to adjust against the bills, failing which the payments received may be");
                sb.Append(" adjusted towards the bills on first in first out basis. </td></tr>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td align = 'right'>FOR Babaji Shivram Clearing & Carriers Pvt Ltd.</td></tr>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td align = 'right'>                                                 </ td></tr>");
                sb.Append("<tr><td align = 'right'>Authorised Signatory</ td></tr>");
                sb.Append("</table>");
                //Export HTML String as PDF.
                StringReader sr = new StringReader(sb.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + dsInv.Tables[0].Rows[0][0].ToString() + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                pdfDoc.Open();
                Response.End();
            }
        }
    }
    DataTable dtprint = new DataTable();
    DataSet dsInv = new DataSet();
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "AND ";
            var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    private void PrintBill(string name)
    {
        try
        {
            string sql = "select JobNo,BLNo,BLDate,ConsigneeName,ShipperName,Mode,DispatchDate,";
            sql = sql + " Job_Type,ContainerType,BE_No,BE_Date,Port_Of_Discharge,Particulars,id.Rate,";
            sql = sql + " Convert(Numeric(18,2),(id.Amt - GST)) as NetAmt,id.GST,Convert(numeric(18,2),id.Amt) as AmtWithGST,";
            sql = sql + " sum(id.Amt - GST)  as TotalAmt,sum(id.GST) as GSTAmt,id.Qty,";
            sql = sql + " sum(id.Amt) as PayableAmt,id.NoOfPackages";
            sql = sql + " from tbl_InvoiceMst im";
            sql = sql + " inner join tbl_InvoiceDet id on im.InvoiceNo = id.InvoiceNo";
            //if (cboCompanyName.Text != "" && cboCompanyName.Text != "--Select--")
            //{
            sql = sql + " where JobNo='" + txtjobno.Text + "' and im.ConsigneeName = '" + name + "'";
            //}

            sql = sql + " group by JobNo,BLNo,BLDate,ConsigneeName,ShipperName,Mode,DispatchDate,";
            sql = sql + " Job_Type,ContainerType,BE_No,BE_Date,Port_Of_Discharge,Particulars,id.Rate,id.Qty,id.GST,id.Amt,id.NoOfPackages,id.Qty";
            sql = sql + " order by JobNo";

            //DataTable dt = DBOperations.FillTableData(sql);
            dtprint = DBOperations.FillTableData(sql);

            //dsGrid dsInv = new dsGrid();
            //DataSet dsInv = new DataSet();
            DataTable t = dsInv.Tables.Add("Items");
            t.Columns.Add("JobNo", Type.GetType("System.String"));
            t.Columns.Add("BLNo", Type.GetType("System.String"));
            t.Columns.Add("BLDate", Type.GetType("System.String"));
            t.Columns.Add("ConsigneeName", Type.GetType("System.String"));
            t.Columns.Add("ShipperName", Type.GetType("System.String"));
            t.Columns.Add("Mode", Type.GetType("System.String"));
            t.Columns.Add("DispatchDate", Type.GetType("System.String"));
            t.Columns.Add("Job_Type", Type.GetType("System.String"));
            t.Columns.Add("ContainerType", Type.GetType("System.String"));
            t.Columns.Add("BE_No", Type.GetType("System.String"));
            t.Columns.Add("BE_Date", Type.GetType("System.String"));
            t.Columns.Add("Port_Of_Discharge", Type.GetType("System.String"));
            t.Columns.Add("Particulars", Type.GetType("System.String"));
            t.Columns.Add("Rate", Type.GetType("System.String"));
            t.Columns.Add("Qty", Type.GetType("System.String"));
            t.Columns.Add("NetAmt", Type.GetType("System.Double"));
            t.Columns.Add("GST", Type.GetType("System.Double"));
            t.Columns.Add("AmtWithGST", Type.GetType("System.Double"));
            t.Columns.Add("TotalAmt", Type.GetType("System.Double"));
            t.Columns.Add("GSTAmt", Type.GetType("System.Double"));
            t.Columns.Add("PayableAmt", Type.GetType("System.Double"));
            t.Columns.Add("NoOfPackages", Type.GetType("System.String"));
            for (int k = 0; k <= dtprint.Rows.Count - 1; k++)
            {
                DataRow drRow = t.NewRow();
                drRow["JobNo"] = dtprint.Rows[k]["JobNo"].ToString();
                drRow["BLNo"] = dtprint.Rows[k]["BLNo"].ToString();
                drRow["BLDate"] = dtprint.Rows[k]["BLDate"].ToString();
                drRow["ConsigneeName"] = dtprint.Rows[k]["ConsigneeName"].ToString();
                drRow["ShipperName"] = dtprint.Rows[k]["ShipperName"].ToString();
                drRow["Mode"] = dtprint.Rows[k]["Mode"].ToString();
                drRow["DispatchDate"] = dtprint.Rows[k]["DispatchDate"].ToString();
                drRow["Job_Type"] = dtprint.Rows[k]["Job_Type"].ToString();
                drRow["ContainerType"] = dtprint.Rows[k]["ContainerType"].ToString();
                drRow["BE_No"] = dtprint.Rows[k]["BE_No"].ToString();
                drRow["BE_Date"] = dtprint.Rows[k]["BE_Date"].ToString();
                drRow["Port_Of_Discharge"] = dtprint.Rows[k]["Port_Of_Discharge"].ToString();
                drRow["Particulars"] = dtprint.Rows[k]["Particulars"].ToString();
                drRow["Rate"] = dtprint.Rows[k]["Rate"].ToString();
                drRow["Qty"] = dtprint.Rows[k]["Qty"].ToString();
                drRow["NetAmt"] = Convert.ToDouble(dtprint.Rows[k]["NetAmt"].ToString());
                drRow["GST"] = Convert.ToDouble(dtprint.Rows[k]["GST"].ToString());
                drRow["AmtWithGST"] = Convert.ToDouble(dtprint.Rows[k]["AmtWithGST"].ToString());
                drRow["TotalAmt"] = Convert.ToDouble(dtprint.Rows[k]["TotalAmt"].ToString());
                drRow["GSTAmt"] = Convert.ToDouble(dtprint.Rows[k]["GSTAmt"].ToString());
                drRow["PayableAmt"] = Convert.ToDouble(dtprint.Rows[k]["PayableAmt"].ToString());
                drRow["NoOfPackages"] = dtprint.Rows[k]["NoOfPackages"].ToString();
                t.Rows.Add(drRow);
            }
            //if (t.Rows.Count > 0)
            //{
            //    ReportDocument objRpt = new ReportDocument();
            //    objRpt.Load(Server.MapPath("\\CrystalReports\\rpt_Bill.rpt"));
            //    objRpt.SetDataSource(dsInv);
            //    CrystalReportViewer1.ReportSource = objRpt;
            //    CrystalReportViewer1.RefreshReport();
            //}
        }
        catch (Exception ex)
        {
            //Global.UpdateErrorLog(Global.UserLogin, this.Text, "cmdPreview_Click", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    #endregion
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
    //protected void CheckBox_Changed(object sender, EventArgs e)
    //{
    //    RadioButtonList rbl = sender as RadioButtonList;
    //    GridViewRow row = rbl.NamingContainer as GridViewRow;
    //    TextBox txt = row.FindControl("txtqty") as TextBox;
    //    Label1.Text = "";
    //    if (rbl.SelectedValue == "2")
    //    {
    //        txt.Text = string.Empty;
    //        txt.Text = "0";
    //        txt.Enabled = false;
    //    }
    //    else if (rbl.SelectedValue == "1")
    //    {
    //        txt.Text = "0";
    //        txt.Focus();
    //        txt.Enabled = true;
    //        txt.ReadOnly = false;
    //    }
    //    Session["lkpost"] = "lkpost";
    //    ModalPopupContractBilling.Show();
    //}

    /// swamini 14 Feb 2023
    protected void CheckBox_Changed(object sender, EventArgs e)
    {
        RadioButtonList rbl = sender as RadioButtonList;
        GridViewRow row = rbl.NamingContainer as GridViewRow;
        TextBox txt = row.FindControl("txtqty") as TextBox;
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
                txt.Text = txtcontainer.Text;
                txt.ReadOnly = true;
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
