using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
public partial class AccountExpense_PaymentRequest : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Payment Request";

        CalExtInvoiceDate.EndDate = DateTime.Now;
        CalExtPayReqrdDate.StartDate = DateTime.Now;

        MskEdtValPayDueDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskEdtValPayReqrdDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
        MskEdtValInvoice.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");


        if(!Page.IsPostBack)
        {
            ddCurrency.SelectedValue = "46";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //string strVendorName = "", strVendorCode = "", strGSTNo = "", strInvoiceNo = "", InvoiceFilePath =             // Foregin Currency
        int InvoiceCurrencyId = 46; // INR

        bool bIsInputValid = ValidateInput();

        if (bIsInputValid)
        {
            int JobId; string strJobRefNO; int ModuleID; int InvoiceType = 1; bool isRIM = false;
            int VendorGSTNType, PaymentTypeId = 0, ExpenseTypeId = 0;
            string strConsigneeGSTIN = "", strConsigneeName = "", strConsigneeCode = "", strConsigneePAN = "";

            string strVendorName, strVendorCode, strVendorGSTNo, strVendorPAN, strInvoiceNo, strRemark;
            bool IsAdvanceReceived = false; decimal AdvanceAmount = 0m;
            bool IsInterest = false; decimal InterestAmount = 0m;
            string strPaymentTerms; string strInvoiceFilePath = "";

            Decimal decInvoiceAmount = 0m, decTaxableAmount = 0m, decGSTAmount = 0m;

            DateTime dtInvoiceDate = DateTime.MinValue, dtPaymentRequiredDate = DateTime.MinValue, dtPaymentDueDate = DateTime.MinValue;

            JobId = Convert.ToInt32(hdnJobId.Value);
            ModuleID = Convert.ToInt32(hdnModuleId.Value);
            strJobRefNO = txtJobNumber.Text.Trim();
            VendorGSTNType = Convert.ToInt32(ddGSTINType.SelectedValue);

            strConsigneeGSTIN = txtCongisgneeGSTIN.Text.Trim();
            strConsigneeName = txtCongisgneeName.Text.Trim();
            strConsigneeCode = hdnConsigneeCode.Value.Trim();
            strConsigneePAN = hdnConsigneePANNo.Value.Trim();

            strRemark = txtRemark.Text.Trim();

            InvoiceType = Convert.ToInt32(ddInvoiceType.SelectedValue);
            PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);
            ExpenseTypeId = Convert.ToInt32(ddlExpenseType.SelectedValue);

            if (ddRIM.SelectedValue == "1")
            {
                isRIM = true;
            }

            if (rdlInterest.SelectedValue == "1")
            {
                IsInterest = true;

                if (txtInterestAmnt.Text.Trim() != "")
                {
                    InterestAmount = Convert.ToDecimal(txtInterestAmnt.Text.Trim());
                }
            }
            if (rblAdvanceReceived.SelectedValue == "1")
            {
                IsAdvanceReceived = true;

                if (txtAdvanceAmount.Text.Trim() != "")
                {
                    AdvanceAmount = Convert.ToDecimal(txtAdvanceAmount.Text.Trim());
                }
            }

            if (txtInvoiceDate.Text != "")
            {
                dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
            }
            if (txtPaymentRequiredDate.Text != "")
            {
                dtPaymentRequiredDate = Commonfunctions.CDateTime(txtPaymentRequiredDate.Text.Trim());
            }
            if (txtPaymentDueDate.Text != "")
            {
                dtPaymentDueDate = Commonfunctions.CDateTime(txtPaymentDueDate.Text.Trim());
            }

            if (ExpenseTypeId == 1 || ExpenseTypeId == 9) // Duty Or Interest
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim()) + InterestAmount;

                    decInvoiceAmount = decTaxableAmount;
                     

                }
                if (lblTotalTaxableValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());
                }
                if (lblTotalGST.Text.Trim() != "")
                {
                    decGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
                }
            }
            else if (InvoiceType == 1) // Final Invoice
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
                }
                if (lblTotalTaxableValue.Text.Trim() != "")
                {
                    decTaxableAmount = Convert.ToDecimal(lblTotalTaxableValue.Text.Trim());
                }
                if (lblTotalGST.Text.Trim() != "")
                {
                    decGSTAmount = Convert.ToDecimal(lblTotalGST.Text.Trim());
                }
            }
            else
            {
                if (txtTotalInvoiceValue.Text.Trim() != "")
                {
                    decInvoiceAmount = Convert.ToDecimal(txtTotalInvoiceValue.Text.Trim());
                    decTaxableAmount = decInvoiceAmount;
                }
            }

            strVendorName = txtVendorName.Text.Trim();
            strVendorCode = hdnVendorCode.Value.Trim();
            strVendorGSTNo = txtSupplierGSTIN.Text.Trim();
            strVendorPAN = txtVendorPANNo.Text.Trim();

            strInvoiceNo = txtInvoiceNo.Text.Trim();

            strPaymentTerms = txtPaymentTerms.Text.Trim();

            int RequestId = AccountExpense.AddInvoiceDetail(JobId, strJobRefNO, ModuleID, ExpenseTypeId,
                isRIM, InvoiceType, PaymentTypeId, VendorGSTNType, strConsigneeGSTIN, strConsigneeName, strConsigneeCode, strConsigneePAN,
                IsInterest, InterestAmount, IsAdvanceReceived, AdvanceAmount, dtPaymentRequiredDate, dtPaymentDueDate, strVendorName, strVendorCode, strVendorGSTNo, 
                strVendorPAN, strInvoiceNo, dtInvoiceDate, decInvoiceAmount, decTaxableAmount, decGSTAmount,InvoiceCurrencyId,0,0, strPaymentTerms, 
                strInvoiceFilePath, strRemark, LoggedInUser.glUserId);

            if (RequestId > 0)
            {
                lblError.Text = "Invoice Detail Added Successfully!";
                lblError.CssClass = "success";

                AccountExpense.AddInvoiceStatus(RequestId, 100, strRemark, LoggedInUser.glUserId);

                if (InvoiceType == 1) // Final
                {
                    AddInvoiceItem(RequestId);
                }

                //if (fuDocument.HasFile)
                //{
                //    string InvoiceFilePath = "VendorInvoice//";

                //    string FileName = UploadInvoice(fuDocument, "InvoceRcvd");
                //    string FileUploadPath = InvoiceFilePath + FileName;

                //    //  int FileOutput = DBOperations.UpdateVendorInvoiceCopy(TokanNo, FileUploadPath);
                //}

                Session["Success"] = lblError.Text;

                Response.Redirect("AccountSuccess.aspx");

            }
            else if (RequestId == 0)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "System Error", "alert('System Error! Please try after sometime!');", true);
            }
            else if (RequestId == -1)
            {
                lblError.Text = "Invoice Details Already Exists!";
                lblError.CssClass = "errorMsg";

                ScriptManager.RegisterStartupScript(this, GetType(), "Invoice Error", "alert('Invoice Details Already Exists!');", true);
            }
        }//END_IF_InputValid
    }
    private void AddInvoiceItem(int InvoiceId)
    {
        int PaymentId = 0; string strChargeName = "", strChargeCode = "", strHSN = "", strRemark="";
        decimal decTotalAmount = 0m, decTaxAmount = 0m, decIGSTRate = 0m, decCGSTRate = 0m, decSGSTRate = 0m,
        decIGSTAmount = 0m, decCGSTAmount = 0m, decSGSTAmount = 0m, decOtherDeduction=0m;

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];

            foreach(DataRow dr in dtCharges.Rows)
            {
                strChargeCode =  dr["ChargeCode"].ToString();
                strChargeName = dr["ChargeName"].ToString();
                strHSN = dr["HSN"].ToString();
                decTaxAmount = Convert.ToDecimal(dr["TaxableValue"]) ;
                decCGSTRate = Convert.ToDecimal(dr["CGSTRate"]);
                decSGSTRate = Convert.ToDecimal(dr["SGSTRate"])  ;
                decIGSTRate = Convert.ToDecimal(dr["IGSTRate"])  ;

                decCGSTAmount= Convert.ToDecimal(dr["CGSTAmt"]) ;
                decSGSTAmount = Convert.ToDecimal(dr["SGSTAmt"]) ;
                decIGSTAmount = Convert.ToDecimal(dr["IGSTAmt"])  ;

                decTotalAmount = Convert.ToDecimal(dr["ChargeTotal"]) ;

                strRemark = dr["ChargeRemark"].ToString() ; 

                int result = AccountExpense.AddInvoiceItem(InvoiceId, PaymentId, strChargeName, strChargeCode,
                    strHSN, decTotalAmount, decTaxAmount, decIGSTRate, decCGSTRate, decSGSTRate,
                    decIGSTAmount, decCGSTAmount, decSGSTAmount, decOtherDeduction, strRemark, LoggedInUser.glUserId);
            }
        }
    }

    private bool ValidateInput()
    {
        bool IsValid = true;
        decimal decTotalValue = 0;

        if (ddlExpenseType.SelectedValue == "1" || ddlExpenseType.SelectedValue == "9")
        {

        }
        else
        {
            if (txtVendorName.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Vendor Name!";
                txtVendorName.Attributes.Add("SetFocus", "True");
                lblError.CssClass = "errorMsg";

                IsValid = false;
                return IsValid;
            }
            if (hdnVendorCode.Value.Trim() == "")
            {
                lblError.Text = "Vendor Details Not Found! Please Select Vendor Name/GSTIN From List";
                lblError.CssClass = "errorMsg";

                IsValid = false;
                return IsValid;
            }

            if (ddInvoiceType.SelectedValue == "1") // Final Invoice
            {
                decimal decChargeTotal = 0m;

                decimal.TryParse(lblTotalValue.Text.Trim(), out decChargeTotal);

                decimal decInvoiceTotal = 0m;

                decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decInvoiceTotal);

                decimal decDiff = decChargeTotal - decInvoiceTotal;

                if (decChargeTotal == 0 || decInvoiceTotal == 0)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                    lblError.CssClass = "errorMsg";

                    IsValid = false;
                    return IsValid;

                }
                else if (decDiff > 1 || decDiff < -1)
                {
                    lblError.Text = "Total Invoice Value Mismatched! Please Check Entered Charge Details";
                    lblError.CssClass = "errorMsg";

                    IsValid = false;
                    return IsValid;
                }

            }

        }

        if (txtTotalInvoiceValue.Text.Trim() == "" || txtTotalInvoiceValue.Text.Trim() == "0") 
        {
            lblError.Text = "Please Enter Total Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }
        else if(decimal.TryParse(txtTotalInvoiceValue.Text.Trim(), out decTotalValue) == false)
        {
            lblError.Text = "Please Enter Valid Total Value!";
            txtTotalInvoiceValue.Attributes.Add("SetFocus", "True");
            lblError.CssClass = "errorMsg";

            IsValid = false;
            return IsValid;
        }

        
        return IsValid;

    }
    public string UploadInvoice(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

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
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

            return FileName;
        }
        else
        {
            return "";
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        ddlExpenseType.SelectedIndex = 0;
        if (txtJobNumber.Text.Trim() != "")
        {
            string ModuleId = hdnModuleId.Value;
            if (hdnJobId.Value != "0")
            {
                DataSet dsGetJobDetail = AccountExpense.GetJobdetailById(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(hdnModuleId.Value));
                
                if (dsGetJobDetail.Tables.Count > 0)
                {
                    lblConsgneeGSTIN.Text = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();
                    lblBENo.Text = dsGetJobDetail.Tables[0].Rows[0]["BOENo"].ToString();
                    lblBLNo.Text = dsGetJobDetail.Tables[0].Rows[0]["MAWBNo"].ToString();
                    lblBranchName.Text = dsGetJobDetail.Tables[0].Rows[0]["BranchName"].ToString();
                    lblIECNumber.Text = dsGetJobDetail.Tables[0].Rows[0]["IECNo"].ToString();

                    
                    //Block Fund Request for Job - Flag - IsFundAllowed 1 = Alloswd, ) -Block
                    if (dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"] != DBNull.Value)
                    {
                        string IsAllowed = dsGetJobDetail.Tables[0].Rows[0]["IsFundAllowed"].ToString();

                        if (IsAllowed == "0")
                        {
                            lblError.Text = "<b>Fund Request Blocked For Job No - </b>" + txtJobNumber.Text;
                            lblError.CssClass = "errorMsg";

                            txtJobNumber.Text = "";
                            hdnJobId.Value = "0";
                  
                            return;
                        }
                    }

                    else if (ModuleId == "1")
                    {
                        string strNotingStatus = dsGetJobDetail.Tables[0].Rows[0]["NotingStatus"].ToString();
                        string strJobType = dsGetJobDetail.Tables[0].Rows[0]["JobType"].ToString();

                        if (strJobType != "14" && strJobType != "101" && strJobType != "201")
                        {
                            if (strNotingStatus != "True")
                            {
                                ViewState["JobrefNO"] = txtJobNumber.Text;
                                lblError.Text = "<b>First Complete The Noting Details</b>";
                                lblError.CssClass = "errorMsg";
                                txtJobNumber.Text = ViewState["JobrefNO"].ToString();
                                return;
                            }
                        }
                        
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"] != DBNull.Value)
                    {
                        hdnBranchId.Value = dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString();
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"] != DBNull.Value)
                    {
                        if (Convert.ToString(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]) != "")
                        {
                            lblPlanningDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["DeliveryPlanningDate"]).ToString("dd/MM/yyyy");
                        }
                    }

                    if (dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
                    {
                        lblDutyAmount.Text = dsGetJobDetail.Tables[0].Rows[0]["DutyAmount"].ToString();
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["AssessableValue"] != DBNull.Value)
                    {
                        lblAssessableValue.Text = dsGetJobDetail.Tables[0].Rows[0]["AssessableValue"].ToString();
                    }
                    if (dsGetJobDetail.Tables[0].Rows[0]["IGSTAmt"] != DBNull.Value)
                    {
                        lblIGSTAmount.Text = dsGetJobDetail.Tables[0].Rows[0]["IGSTAmt"].ToString();
                    }

                    lblCustomer.Text= dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                    lblConsignee.Text = dsGetJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
                }
            }
        }
        else
        {
            hdnBranchId.Value = "0";
            lblConsignee.Text = "";
            lblCustomer.Text = "";
        }
    }
    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        AddCharges();
    }
    private void AddCharges()
    {
        decimal decTaxableAmount = 0, decGSTRate = 0, decGSTAmount = 0, decTotalAmount = 0;
        decimal decIGSTRate = 0, decCGSTRate = 0, decSGSTRate = 0;
        decimal decIGSTAmt = 0, decCGSTAmt = 0, decSGSTAmt = 0;

        decimal decExchangeRate = 1;

        decExchangeRate = Convert.ToDecimal(txtExchangeRate.Text.Trim());

        decTaxableAmount = Convert.ToDecimal(txtTaxableValue.Text.Trim()) * decExchangeRate;

        decGSTRate = Convert.ToDecimal(ddGSTRate.SelectedValue);

        decGSTAmount = (decTaxableAmount * decGSTRate) / 100;

        decTotalAmount = decTaxableAmount + decGSTAmount;

        if (rblGSTType.SelectedValue == "1")
        {
            decIGSTRate = decGSTRate;
            decIGSTAmt = decGSTAmount;
        }
        else 
        {
            decCGSTRate = decGSTRate / 2;
            decSGSTRate = decGSTRate / 2;

            decCGSTAmt = decGSTAmount / 2;
            decSGSTAmt = decGSTAmount / 2;
        }
     

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        DataRow dr = dtCharges.NewRow();

        dr["ChargeCode"] = txtChargeCode.Text.Trim();
        dr["ChargeName"] = txtChargeName.Text.Trim();
        dr["HSN"] = hdnChargeHSN.Value.Trim();

        dr["TaxableValue"] = txtTaxableValue.Text.Trim();
        dr["CGSTRate"] = decCGSTRate.ToString();
        dr["SGSTRate"] = decSGSTRate.ToString();
        dr["IGSTRate"] = decIGSTRate.ToString();

        dr["CGSTAmt"] = decCGSTAmt.ToString();
        dr["SGSTAmt"] = decSGSTAmt.ToString();
        dr["IGSTAmt"] = decIGSTAmt.ToString();

        dr["ChargeTotal"] = decTotalAmount.ToString();
        dr["ChargeRemark"] = txtChargeRemark.Text.Trim();

        //OtherDeduction - Not Required On Payment Request Page
        dr["OtherDeduction"] = "0";

        dtCharges.Rows.Add(dr);
        dtCharges.AcceptChanges();

        ViewState["vwsCharges"] = dtCharges;

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        // Calculate Total Charges
        CalcuateTotalCharges();

        // Reset Charge TextBox Value

        ResetCharges();
    }

    private DataTable GetChargeCodeDataTable()
    {
        DataTable dtChargeCode = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colChargeCode = new DataColumn("ChargeCode", Type.GetType("System.String"));
        DataColumn colChargeName = new DataColumn("ChargeName", Type.GetType("System.String"));
        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colTaxableValue = new DataColumn("TaxableValue", Type.GetType("System.String"));
        DataColumn colOtherDeduction = new DataColumn("OtherDeduction", Type.GetType("System.String"));
        DataColumn colIGSTRate = new DataColumn("IGSTRate", Type.GetType("System.String"));
        DataColumn colIGSTAmt = new DataColumn("IGSTAmt", Type.GetType("System.String"));
        DataColumn colCGSTRate = new DataColumn("CGSTRate", Type.GetType("System.String"));
        DataColumn colCGSTAmt = new DataColumn("CGSTAmt", Type.GetType("System.String"));
        DataColumn colSGSTRate = new DataColumn("SGSTRate", Type.GetType("System.String"));
        DataColumn colSGSTAmt = new DataColumn("SGSTAmt", Type.GetType("System.String"));

        DataColumn colChargeTotal = new DataColumn("ChargeTotal", Type.GetType("System.String"));
        DataColumn colChargeRemark = new DataColumn("ChargeRemark", Type.GetType("System.String"));

        dtChargeCode.Columns.Add(colSL);
        dtChargeCode.Columns.Add(colChargeCode);
        dtChargeCode.Columns.Add(colChargeName);
        dtChargeCode.Columns.Add(colHSN);
        dtChargeCode.Columns.Add(colTaxableValue);
        dtChargeCode.Columns.Add(colOtherDeduction);

        dtChargeCode.Columns.Add(colCGSTRate);
        dtChargeCode.Columns.Add(colSGSTRate);
        dtChargeCode.Columns.Add(colIGSTRate);

        dtChargeCode.Columns.Add(colCGSTAmt);
        dtChargeCode.Columns.Add(colSGSTAmt);
        dtChargeCode.Columns.Add(colIGSTAmt);

        dtChargeCode.Columns.Add(colChargeTotal);
        dtChargeCode.Columns.Add(colChargeRemark);

        dtChargeCode.PrimaryKey = new DataColumn[] { colSL };
        dtChargeCode.AcceptChanges();

        return dtChargeCode;
    }
    private void CalcuateTotalCharges()
    {
        decimal decTotalTaxableValue = 0, decTotalGSTValue = 0, decTotalInvoiceValue = 0;
 
        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        
            foreach(DataRow dr in dtCharges.Rows)
            {
                if (dr["TaxableValue"].ToString() != "")
                {
                    decTotalTaxableValue = decTotalTaxableValue + Convert.ToDecimal(dr["TaxableValue"]);
                }
                if (dr["CGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["CGSTAmt"]);
                }
                if (dr["SGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["SGSTAmt"]);
                }
                if (dr["IGSTAmt"].ToString() != "")
                {
                    decTotalGSTValue = decTotalGSTValue + Convert.ToDecimal(dr["IGSTAmt"]);
                }
                if (dr["ChargeTotal"].ToString() != "")
                {
                    decTotalInvoiceValue = decTotalInvoiceValue + Convert.ToDecimal(dr["ChargeTotal"]);
                }
            }
        }

        lblTotalTaxableValue.Text = decTotalTaxableValue.ToString();
        lblTotalGST.Text = decTotalGSTValue.ToString();
        lblTotalValue.Text = decTotalInvoiceValue.ToString();

        // Allow Add more Charges

        int ChargeRowCount = gvCharges.Rows.Count;

        if (rblMultiChargeCode.SelectedValue == "0")
        {
            // Max one Row Allowed

            if (ChargeRowCount > 0)
            {
                btnAddCharges.Visible = false;
            }
            else
            {
                btnAddCharges.Visible = true;
            }
        }
        else
        {
            btnAddCharges.Visible = true;
        }
    }

    private void ResetCharges()
    {
        txtChargeCode.Text      =   "";
        txtChargeName.Text      =   "";
        hdnChargeHSN.Value      =   "";
        txtTaxableValue.Text    =   "";
        txtChargeRemark.Text    =   "";
        ddGSTRate.SelectedIndex = -1;

    }
    protected void lnlRemove_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string strChargeId = lnk.CommandArgument.ToString();

        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }
        
        dtCharges.Rows.Find(strChargeId).Delete();

        dtCharges.AcceptChanges();

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();

        ViewState["vwsCharges"] = dtCharges;

        // Calculate Total Charges
        CalcuateTotalCharges();
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

    protected void ddInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddInvoiceType.SelectedValue == "1")
        {
            fldInvoiceItem.Visible = true;

            rblMultiChargeCode.Enabled = true;
        }
        else
        {
            fldInvoiceItem.Visible = false;

            rblMultiChargeCode.SelectedValue = "0";
            rblMultiChargeCode.Enabled = false;
        }
    }

    protected void ddGSTINType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSupplierGSTIN.Text = "";
        txtVendorName.Text = "";
        txtVendorPANNo.Text = "";

        if (ddGSTINType.SelectedValue =="1")
        {
            txtSupplierGSTIN.Enabled = true;
            txtVendorName.Enabled = false;
            txtVendorPANNo.Enabled = false;

            ddCurrency.SelectedValue = "46";
            txtExchangeRate.Text = "1";
            txtExchangeRate.Enabled = false;
        }
        else if(ddGSTINType.SelectedValue == "2")
        {
            txtSupplierGSTIN.Enabled = false;
            txtVendorName.Enabled = true;
            txtVendorPANNo.Enabled = true;

            txtExchangeRate.Text = "1";
            txtExchangeRate.Enabled = true;
        }
        else if (ddGSTINType.SelectedValue == "3")
        {
            txtSupplierGSTIN.Enabled = false;
            txtVendorName.Enabled = true;
            txtVendorPANNo.Enabled = false;

            txtExchangeRate.Enabled = true;
        }

        MultiChargeCodeVendorTypeNonRIM();
    }

    protected void rblAdvanceReceived_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(rblAdvanceReceived.SelectedValue == "1")
        {
            txtAdvanceAmount.Enabled = true;
        }
        else
        {
            txtAdvanceAmount.Enabled = false;
            txtAdvanceAmount.Text = "";

        }
    }

    protected void txtPaymentDueDate_TextChanged(object sender, EventArgs e)
    {
        if(txtPaymentDueDate.Text.Trim() != "")
        {
            if(txtPaymentTerms.Text.Trim() == "" || txtPaymentTerms.Text.Trim() == "0")
            {
                txtPaymentRequiredDate.Text = txtPaymentDueDate.Text.Trim();
            }
            else
            {

            }
        }
    }

    protected void ddlExpenseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // 1 - Duty, 2 - CFS

        txtChargeName.Enabled = true;
        txtChargeCode.Enabled = true;
        // fldVendor
        if (ddlExpenseType.SelectedValue =="1") // Duty Payment - Vendor Info Not Required
        {
            // 

            txtChargeName.Text = "THC/DO/PROCESSING/STAMP DUTY";
            txtChargeCode.Text = "Y10";

            txtChargeName.Enabled = false;
            txtChargeCode.Enabled = false;

            fldVendor.Visible = false;

            txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
            txtCongisgneeName.Text = lblConsignee.Text;
            chkNoGSTINCustomer.Enabled = false;
            trDutyInterest.Visible = true;

            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = false;
            ddRIM.SelectedValue = "1";
            ddRIM.Enabled = false;
            
            lblInvoiceNo.Text = "Challan No";
            lblInvoiceDate.Text = "Challan Date";
            lblTotalInvoiceValue.Text = "Duty Amount";
            fldInvoiceItem.Visible = false;
        }
        // fldVendor
        else if (ddlExpenseType.SelectedValue == "9") // Panelty - Vendor Info Not Requuired
        {
            fldVendor.Visible = false;

            txtCongisgneeGSTIN.Text = lblConsgneeGSTIN.Text;
            txtCongisgneeName.Text = lblConsignee.Text;
            chkNoGSTINCustomer.Enabled = false;

            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = false;
            ddRIM.SelectedValue = "1";
            ddRIM.Enabled = false;

            lblInvoiceNo.Text = "Challan No";
            lblInvoiceDate.Text = "Challan Date";
            lblTotalInvoiceValue.Text = "Penalty Amount";
            fldInvoiceItem.Visible = false;
        }
        else
        {
            fldVendor.Visible = true;
            txtCongisgneeGSTIN.Text ="";
            txtCongisgneeName.Text = "";
            chkNoGSTINCustomer.Enabled = true;

            trDutyInterest.Visible = false;
            txtInterestAmnt.Text = "0";

            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.SelectedValue = "1";
            ddInvoiceType.Enabled = true;
            ddRIM.Enabled = true;

            lblInvoiceNo.Text = "Invoice No";
            lblInvoiceDate.Text = "Invoice Date";
            lblTotalInvoiceValue.Text = "Total Invoice Value";
            fldInvoiceItem.Visible = true;
        }
    }

    protected void rdlInterest_OnSelectedIndexChanged(object sender, EventArgs e)  //radio button click
    {
        if (rdlInterest.SelectedItem.Text.ToLower() == "yes")
        {
            //lblInterestAmnt.Visible = true;
            //txtInterestAmnt.Visible = true;   

            tdlblInterestAmnt.Visible = true;
            tdtxtInterestAmnt.Visible = true;
        }
        else
        {
            //lblInterestAmnt.Visible = false;            
            //txtInterestAmnt.Visible = false;
            tdlblInterestAmnt.Visible = false;
            tdtxtInterestAmnt.Visible = false;

            txtInterestAmnt.Text = "0";
        }
    }

    public void TotalAmnt()
    {
        double DutyAmnt = 0;
        double interestAmnt = 0;
        double peneltyAmnt = 0;
        double TotalAmnt = 0;
        double Amount = 0;

        if (ddlExpenseType.SelectedValue == "1")
        {
            if (txtTotalInvoiceValue.Text.Trim() != "")
            {
                DutyAmnt = Convert.ToDouble(txtTotalInvoiceValue.Text.Trim());
            }

            if (txtInterestAmnt.Text.Trim() != "")
            {
                interestAmnt = Convert.ToDouble(txtInterestAmnt.Text.Trim());
            }


            TotalAmnt = DutyAmnt + interestAmnt + peneltyAmnt;
            lblTotalTaxableValue.Text = Convert.ToString(TotalAmnt);
            lblTotalValue.Text = Convert.ToString(TotalAmnt);
        }
    }

    protected void txtInterestAmnt_OnTextChanged(object sender, EventArgs e)
    {
        TotalAmnt();
    }

    protected void rblMultiChargeCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        MultiChargeCodeVendorTypeNonRIM();
    }

    #region Server Validation

    private void MultiChargeCodeVendorTypeNonRIM()
    {
        if(ddGSTINType.SelectedValue == "1") // Registered - Disable GST Rate
        {
            ddGSTRate.Enabled = true;
            ddGSTRate.SelectedIndex = 0;
        }
        else
        {
            ddGSTRate.Enabled = false;
            ddGSTRate.SelectedValue= "0"; // No GST
        }
        // Vendor Type, Multi Charge Code and Non-RIM Validation

        //1.  When vendor type registered and Multi Charge Code - No  then only total taxable value, GST rate drop down , 
        // GST Amount(auto calculated) and total invoice value(auto calculated) should be asked in charge detail table.
        // No option shall be avaialble for add charges

    }
    #endregion

}