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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class Quotation_EditQuotation : System.Web.UI.Page
{
    public Dictionary<int, string> SelectedCharges = new Dictionary<int, string>();
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["LumSumAmt"] = "0";
        ViewState["LpChargesTotal"] = "0";
        ViewState["ChargesMinTotal"] = "0";
        ViewState["MinAmt"] = "0";

        ScriptManager1.RegisterPostBackControl(btnSave);
        ScriptManager1.RegisterPostBackControl(gvGenerateCharge);
        ScriptManager1.RegisterPostBackControl(gvRFQDocuments);
        ScriptManager1.RegisterPostBackControl(btnSaveDoc);
        ScriptManager1.RegisterPostBackControl(btnYes);

        if (!IsPostBack)
        {
            if (Session["QuotationId"] != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Draft Quotation";
                lblError.Visible = false;

                DBOperations.FillBabajiUserCustomer(ddCustomer, LoggedInUser.glUserId, true);
                DBOperations.FillBabajiUserCustomer(ddlTenderCustomer, LoggedInUser.glUserId, true);
                if (Convert.ToString(Session["QuotationId"]) != "")
                    GetQuotationDetails(Convert.ToInt32(Session["QuotationId"]));
            }
        }

        if (ViewState["VSCharges"] != null)
        {
            SelectedCharges = (Dictionary<int, string>)ViewState["VSCharges"];
        }
    }

    protected void GetQuotationDetails(int QuotationId)
    {
        if (QuotationId != 0)
        {
            DataSet dsGetQuoteDetails = QuotationOperations.GetParticularQuotation(QuotationId);
            if (dsGetQuoteDetails != null)
            {
                if (dsGetQuoteDetails.Tables[0].Rows[0]["IsLumpSumCode"].ToString().ToLower().Trim() == "true")
                {
                    chkLumpSum.Checked = true;
                    dvDisplayNote.Visible = false;
                }
                else
                {
                    chkLumpSum.Checked = false;
                    dvDisplayNote.Visible = true;
                }

                if (dsGetQuoteDetails.Tables[0].Rows[0]["KamId"].ToString() != "0")
                {
                    hdnKAMId.Value = dsGetQuoteDetails.Tables[0].Rows[0]["KamId"].ToString();
                    txtKAM.Text = dsGetQuoteDetails.Tables[0].Rows[0]["KAMPerson"].ToString();
                }

                if (dsGetQuoteDetails.Tables[0].Rows[0]["SalesPersonId"].ToString() != "0")
                {
                    hdnSalesPersonId.Value = dsGetQuoteDetails.Tables[0].Rows[0]["SalesPersonId"].ToString();
                }

                txtSalesPerson.Text = dsGetQuoteDetails.Tables[0].Rows[0]["SalesPersonName"].ToString();

                // Quotation Format
                Boolean IsNormalQuote = true;
                if (dsGetQuoteDetails.Tables[0].Rows[0]["IsTenderQuote"].ToString().ToLower().Trim() == "true")
                    IsNormalQuote = false;

                if (IsNormalQuote == true)
                {
                    QuotationOperations.FillBranchByUser(ddlBabajiBranch, LoggedInUser.glUserId);
                    ddlBabajiBranch.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["BranchId"].ToString();

                    lblQuoteRefNo.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                    dvBabajiQuote.Visible = true;
                    dvCustomerQuote.Visible = false;
                    ddlTermCondition.DataBind();
                    ddlTermCondition.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["TermConditionId"].ToString();

                    // Customer 
                    if (dsGetQuoteDetails.Tables[0].Rows[0]["CustomerId"].ToString() == "0")
                    {
                        ddCustomer.Visible = false;
                        txtCustomer.Text = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"].ToString();
                        txtCustomer.Enabled = false;
                    }
                    else
                    {
                        ddCustomer.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerId"].ToString();
                        ddCustomer_SelectedIndexChanged(ddCustomer, EventArgs.Empty);
                        ddCustomer.Enabled = false;
                    }

                    txtAddressLine1.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine1"].ToString();          // Address Line 1
                    txtAddressLine2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine2"].ToString();          // Address Line 2
                    txtAddressLine3.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine3"].ToString();          // Address Line 3
                    txtKindAttn.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AttendedPerson"].ToString();            // Kind Attention Name
                    txtSubject.Text = dsGetQuoteDetails.Tables[0].Rows[0]["Subject"].ToString();                    // Subject
                    txtTerms.Text = dsGetQuoteDetails.Tables[0].Rows[0]["PaymentTerms"].ToString();                 // Payment Terms

                    ddlIncludeDesc_OnSelectedIndexChanged(ddlIncludeDesc, EventArgs.Empty);
                    ddlDivision.DataBind();
                    ddlDivision.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["ServiceId"].ToString();             // DIVISION
                    lblQuoteGeneratedFor.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteGeneratedFor"].ToString();    // QUOTE GENERATED FOR

                    // MODES
                    cblModes.DataBind();
                    DataSet dsgetModes = QuotationOperations.GetQuotationModes(QuotationId);
                    if (dsgetModes != null)
                    {
                        for (int i = 0; i < dsgetModes.Tables[0].Rows.Count; i++)
                        {
                            if (dsgetModes.Tables[0].Rows[i]["ModeId"].ToString() != "")
                            {
                                for (int j = 0; j < cblModes.Items.Count; j++)
                                {
                                    if (cblModes.Items[j].Value == dsgetModes.Tables[0].Rows[i]["ModeId"].ToString())
                                        cblModes.Items[j].Selected = true;
                                }
                            }
                        }
                    }

                    // Charges Applicable Table
                    DataSet dsGetChargesApp = QuotationOperations.GetQuoteReportData(QuotationId);
                    if (dsGetChargesApp != null)
                    {
                        gvGenerateCharge.DataSource = dsGetChargesApp;
                        gvGenerateCharge.DataBind();
                    }

                    // Transportation charges
                    GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));

                    // Status For Quotation
                    if (dsGetQuoteDetails.Tables[0].Rows[0]["StatusId"] != DBNull.Value &&
                        dsGetQuoteDetails.Tables[0].Rows[0]["StatusId"].ToString() != "")
                    {
                        int StatusId = Convert.ToInt32(dsGetQuoteDetails.Tables[0].Rows[0]["StatusId"].ToString());
                        if (StatusId == 3 || StatusId == 2)  // quotation approved or system approval quote
                        {
                            btnSave.Visible = false;
                            gvGenerateCharge.Enabled = false;
                            if (gvTransportChg.Rows.Count > 0)
                            {
                                gvTransportChg.Columns[3].Visible = false;
                            }
                        }
                        else if (StatusId == 8) // negotiated quotation
                        {
                            btnSave.Visible = true;
                            gvGenerateCharge.Enabled = true;
                        }
                    }
                }
                else
                {
                    QuotationOperations.FillBranchByUser(ddlBabajiBranch2, LoggedInUser.glUserId);
                    ddlBabajiBranch2.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["BranchId"].ToString();

                    lblQuoteRefNo2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                    ddlDivision2.DataBind();
                    ddlDivision2.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["ServiceId"].ToString();
                    dvBabajiQuote.Visible = false;
                    dvCustomerQuote.Visible = true;

                    // Customer 
                    if (dsGetQuoteDetails.Tables[0].Rows[0]["CustomerId"].ToString() == "0")
                    {
                        ddlTenderCustomer.Visible = false;
                        txtTenderCustomer.Text = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"].ToString();
                        txtTenderCustomer.Enabled = false;
                    }
                    else
                    {
                        ddlTenderCustomer.SelectedValue = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerId"].ToString();
                        ddlTenderCustomer_SelectedIndexChanged(ddlTenderCustomer, EventArgs.Empty);
                        ddlTenderCustomer.Enabled = false;
                    }

                    // Other Notes
                    txtNotes.Text = dsGetQuoteDetails.Tables[0].Rows[0]["OtherNotes"].ToString();
                    gvRFQDocuments.DataBind();
                }
            }
        }
        else
        {
            lblError.Text = "Quotation Not Found!!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void ddlIncludeDesc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIncludeDesc.SelectedValue == "2")
        {
            //display popup
            trTexture.Visible = true;

            string Script =
           "$(document).ready(function(){" +
           "tinyMCE.init({" +
            " mode: 'specific_textareas'," +
           " editor_selector: 'mceEditor'," +
            "width: '100%'," +
            "theme: 'advanced'," +
            "height: '300px'," +
            "plugins: 'safari,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,imagemanager,filemanager'," +
            "theme_advanced_buttons1: 'newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect'," +
            "theme_advanced_buttons2: 'cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,cleanup,code,|,forecolor,backcolor,tablecontrols,print,ltr,rtl,|,fullscreen, absolute,spellchecker,abbr,del'," +
            "theme_advanced_buttons3: ''," +
            "theme_advanced_toolbar_location: 'top'," +
            "theme_advanced_toolbar_align: 'left'," +
            "theme_advanced_statusbar_location: 'bottom'," +
            "theme_advanced_resizing: false," +
            "template_external_list_url: 'js/template_list.js'," +
            "external_link_list_url: 'js/link_list.js'," +
            "external_image_list_url: 'js/image_list.js'," +
            "media_external_list_url: 'js/media_list.js'" +
                 "}); " +
            " });";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", Script, true);
            DataSet dsGetQuoteDetails = QuotationOperations.GetParticularQuotation(Convert.ToInt32(Session["QuotationId"]));
            if (dsGetQuoteDetails != null)
            {
                if (dsGetQuoteDetails.Tables[0].Rows[0]["BodyContent"].ToString() != "")
                {
                    // txtHTMLContent.Text = dsGetQuoteDetails.Tables[0].Rows[0]["BodyContent"].ToString();
                }
            }
        }
        else
        {
            trTexture.Visible = false;
        }
    }

    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCustomer.SelectedValue != "0")
        {
            txtCustomer.Enabled = false;
            txtCustomer.Text = ddCustomer.SelectedItem.Text.Trim();
        }
        else
        {
            txtCustomer.Enabled = true;
            txtCustomer.Text = "";
        }
    }

    protected void ddlTenderCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTenderCustomer.SelectedValue != "0")
        {
            txtTenderCustomer.Enabled = false;
            txtTenderCustomer.Text = ddlTenderCustomer.SelectedItem.Text.Trim();
        }
        else
        {
            txtTenderCustomer.Enabled = true;
            txtTenderCustomer.Text = "";
        }
    }

    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        int QuotationId = 0;
        if (Convert.ToString(Session["QuotationId"]) != null)
            QuotationId = Convert.ToInt32(Session["QuotationId"].ToString());

        int TermConditionId = 0, BranchId = 0, ServiceId = 0;
        if (ddlTermCondition.SelectedValue != "0")
            TermConditionId = Convert.ToInt32(ddlTermCondition.SelectedValue);

        if (QuotationId != 0)
        {
            Boolean IsNormalQuote = true;

            if (IsNormalQuote == false) // RFQ or Tender Quotation
            {
                if (ddlBabajiBranch2.SelectedValue != "0")
                    BranchId = Convert.ToInt32(ddlBabajiBranch2.SelectedValue);
                string Customer = "", OtherNotes = "";
                int CustomerId = 0;
                if (ddlTenderCustomer.SelectedValue != "0")
                    CustomerId = Convert.ToInt32(ddlTenderCustomer.SelectedValue);
                Customer = txtTenderCustomer.Text.Trim();
                if (Customer != "")
                {
                    if (txtNotes.Text.Trim() != "")
                    {
                        if (ddlDivision2.SelectedValue != "0")
                            ServiceId = Convert.ToInt32(ddlDivision2.SelectedValue);
                        OtherNotes = txtNotes.Text.Trim();
                        int SavedDraft = QuotationOperations.UpdateDraftQuotation(QuotationId, BranchId, ServiceId, CustomerId, Customer, "", "", "", "", "", false, "", "", true, OtherNotes, LoggedInUser.glUserId, 0,
                                                             Convert.ToInt32(hdnKAMId.Value), Convert.ToInt32(hdnSalesPersonId.Value), txtSalesPerson.Text.Trim());
                        if (SavedDraft == 2)
                        {
                            string QuotationRefNo = "";
                            int result_Draft = QuotationOperations.UpdateDraftApprovalStatus(SavedDraft, 6, LoggedInUser.glUserId);
                            DataSet dsGetQuoteRefNo = QuotationOperations.GetParticularQuotation(QuotationId);
                            if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                                QuotationRefNo = dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                            lblError.Text = "Successfully Updated Draft Quotation - " + QuotationRefNo.Trim() + " .";
                            lblError.CssClass = "success";
                            ResetControls();
                            dvErrorSection.Visible = true;
                            dvDraftSection.Visible = false;
                        }
                        else
                        {
                            lblError.Text = "Error while inserting record. Please try again later.";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please Enter Notes.";
                        lblError.CssClass = "errorMsg";
                        txtNotes.Focus();
                    }
                }
            }
            else // Normal Babaji Quotation
            {
                int ValidateSuccess = -123;
                ValidateSuccess = ValidateChargesApplicable();

                if (ValidateSuccess == 10) // validated successfully the charges grid view data
                {
                    if (txtCustomer.Text.Trim() != "")
                    {
                        // insert general quotation information
                        int CustomerId = 0, result = -123;
                        string CustomerName = "", AddressLine1 = "", AddressLine2 = "", AddressLine3 = "", AttendedPerson = "", Subject = "", PaymentTerms = "", BodyContent = "";
                        Boolean Includebody = false, IsValidDraft = true;
                        decimal dcLpChargesTotal = 0, dcChargesMinTotal = 0;

                        if (ddlBabajiBranch.SelectedValue != "0")
                            BranchId = Convert.ToInt32(ddlBabajiBranch.SelectedValue);
                        CustomerName = txtCustomer.Text.Trim();
                        if (ddCustomer.SelectedValue != "0")
                            CustomerId = Convert.ToInt32(ddCustomer.SelectedValue);
                        AddressLine1 = txtAddressLine1.Text.Trim();
                        AddressLine2 = txtAddressLine2.Text.Trim();
                        AddressLine3 = txtAddressLine3.Text.Trim();
                        AttendedPerson = txtKindAttn.Text.Trim();
                        Subject = txtSubject.Text.Trim();
                        PaymentTerms = txtTerms.Text.Trim();
                        if (ddlDivision.SelectedValue != "0")
                            ServiceId = Convert.ToInt32(ddlDivision.SelectedValue);
                        if (ddlIncludeDesc.SelectedValue == "2")
                        {
                            if (txtHTMLContent.Text.Trim() != "")
                            {
                                Includebody = true;
                                BodyContent = txtHTMLContent.Text.Trim();
                            }
                            else
                            {
                                Includebody = false;
                                BodyContent = "";
                            }
                        }
                        if (chkLumpSum.Checked == true)
                        {
                            double number;
                            // get total of all lump sum charges
                            if (gvGenerateCharge != null && gvGenerateCharge.Rows.Count > 0)
                            {
                                for (int i = 0; i < gvGenerateCharge.Rows.Count; i++)
                                {
                                    TextBox txtLumpSumAmt = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtLumpsumAmount");
                                    if (txtLumpSumAmt != null)
                                    {
                                        if (txtLumpSumAmt.Text.Trim() != "")
                                        {
                                            if (Double.TryParse(txtLumpSumAmt.Text.Trim(), out number))
                                                ViewState["LpChargesTotal"] = Convert.ToDecimal(ViewState["LpChargesTotal"]) + Convert.ToDecimal(txtLumpSumAmt.Text.Trim());
                                        }
                                    }

                                    Label ChargeId = (Label)gvGenerateCharge.Rows[i].FindControl("lblChargeId");
                                    DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[i].FindControl("ddlRanges_LumpSum");

                                    if (ddlRanges_LumpSum != null && ddlRanges_LumpSum.SelectedValue != "0")
                                    {
                                        DataSet dsGetMinValue = QuotationOperations.GetMinValueForRange(Convert.ToInt32(ChargeId.Text), Convert.ToInt32(ddlRanges_LumpSum.SelectedValue));
                                        if (dsGetMinValue != null)
                                        {
                                            if (dsGetMinValue.Tables[0].Rows[0]["MinRange"] != DBNull.Value)
                                            {
                                                ViewState["ChargesMinTotal"] = Convert.ToDecimal(ViewState["ChargesMinTotal"]) + Convert.ToDecimal(dsGetMinValue.Tables[0].Rows[0]["MinRange"]);
                                            }
                                        }
                                    }
                                }
                                dcChargesMinTotal = Convert.ToDecimal(Convert.ToString(ViewState["ChargesMinTotal"]));
                                dcLpChargesTotal = Convert.ToDecimal(Convert.ToString(ViewState["LpChargesTotal"]));
                            }

                            // if lump charges is less than actual minimum of all those charges than unvalid quote
                            if (dcChargesMinTotal > dcLpChargesTotal)
                                IsValidDraft = false;
                        }

                        int SavedDraft = QuotationOperations.UpdateDraftQuotation(QuotationId, BranchId, ServiceId, CustomerId, CustomerName, AddressLine1, AddressLine2, AddressLine3, AttendedPerson, Subject,
                                                                Includebody, PaymentTerms, BodyContent, IsValidDraft, "", LoggedInUser.glUserId, TermConditionId,
                                                                Convert.ToInt32(hdnKAMId.Value), Convert.ToInt32(hdnSalesPersonId.Value), txtSalesPerson.Text.Trim());
                        if (SavedDraft == 2)
                        {
                            #region ADD QUOTATION MODE DETAILS
                            if (cblModes != null && cblModes.Items.Count > 0)
                            {
                                for (int m = 0; m < cblModes.Items.Count; m++)
                                {
                                    if (cblModes.Items[m].Selected == true)
                                    {
                                        int AddModes = QuotationOperations.AddQuotationModes(QuotationId, Convert.ToInt32(cblModes.Items[m].Value.ToString()), LoggedInUser.glUserId);
                                    }
                                }
                            }
                            #endregion

                            // insert charges applicable grid data
                            if (gvGenerateCharge.Rows.Count > 0)
                            {
                                for (int i = 0; i < gvGenerateCharge.Rows.Count; i++)
                                {
                                    int RateId = Convert.ToInt32(gvGenerateCharge.DataKeys[i].Values[0]);
                                    int ChargeId = 0, QuotationCatgId = 0, ApplicableFieldId = 0;
                                    double dcCharges = 0.0, dcMinCharges = 0.0;
                                    string ExtraCharges = "";
                                    Boolean IsLumpSumField = false, IsValidAmt = true;
                                    decimal dcLumpSumCharges = 0;

                                    Label lblChargeId = (Label)gvGenerateCharge.Rows[i].FindControl("lblChargeId");
                                    CheckBox chkItemForLumpSum = (CheckBox)gvGenerateCharge.Rows[i].FindControl("chkItemForLumpSum");
                                    Label lblParticulars = (Label)gvGenerateCharge.Rows[i].FindControl("lblParticulars");
                                    TextBox txtChargesApp = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtChargesApp");
                                    DropDownList ddlRanges = (DropDownList)gvGenerateCharge.Rows[i].FindControl("ddlRanges");
                                    TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtLumpsumAmount");
                                    DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[i].FindControl("ddlRanges_LumpSum");
                                    Label lblCategoryId = (Label)gvGenerateCharge.Rows[i].FindControl("lblCategoryId");

                                    if (lblChargeId != null && lblChargeId.Text != "")
                                        ChargeId = Convert.ToInt32(lblChargeId.Text.Trim());
                                    if (chkItemForLumpSum != null)
                                    {
                                        if (chkItemForLumpSum.Checked == true || txtLumpsumAmount.Text.Trim() != "")
                                            IsLumpSumField = true;
                                    }
                                    if (txtChargesApp != null && txtChargesApp.Text.Trim() != "")
                                    {
                                        if (ddlRanges.Items.Count > 1)
                                        {
                                            dcCharges = Convert.ToDouble(txtChargesApp.Text.Trim());
                                            ExtraCharges = "";
                                        }
                                        else
                                        {
                                            dcCharges = 0.0;
                                            ExtraCharges = txtChargesApp.Text.Trim();
                                        }

                                        if (ddlRanges != null && ddlRanges.SelectedValue != "0")
                                            ApplicableFieldId = Convert.ToInt32(ddlRanges.SelectedValue);
                                    }
                                    else
                                    {
                                        double number;
                                        if (ddlRanges_LumpSum != null && ddlRanges_LumpSum.SelectedValue != "0")
                                            ApplicableFieldId = Convert.ToInt32(ddlRanges_LumpSum.SelectedValue);
                                        if (chkItemForLumpSum != null)
                                        {
                                            if (chkItemForLumpSum.Checked == true || txtLumpsumAmount.Text.Trim() != "")
                                            {
                                                if (Double.TryParse(txtLumpsumAmount.Text.Trim(), out number))
                                                {
                                                    dcLumpSumCharges = Convert.ToDecimal(txtLumpsumAmount.Text.Trim());
                                                    ExtraCharges = "";
                                                }
                                                else
                                                {
                                                    ExtraCharges = txtLumpsumAmount.Text.Trim();
                                                    dcLumpSumCharges = 0;
                                                }
                                            }
                                        }
                                    }

                                    if (lblCategoryId != null && lblCategoryId.Text.Trim() != "")
                                        QuotationCatgId = Convert.ToInt32(lblCategoryId.Text.Trim());

                                    if (chkLumpSum.Checked == false)
                                    {
                                        if (ddlRanges != null && ddlRanges.Items.Count > 0)
                                        {
                                            if (ddlRanges.SelectedValue != "0")
                                            {
                                                //get minimum amount for given charge
                                                DataSet dsGetMinValue = QuotationOperations.GetMinValueForRange(Convert.ToInt32(ChargeId), Convert.ToInt32(ddlRanges.SelectedValue));
                                                if (dsGetMinValue != null)
                                                {
                                                    if (dsGetMinValue.Tables[0].Rows[0]["MinRange"] != DBNull.Value)
                                                    {
                                                        dcMinCharges = Convert.ToDouble(dsGetMinValue.Tables[0].Rows[0]["MinRange"]);
                                                    }
                                                    else
                                                        dcMinCharges = 0.0;
                                                }
                                                else
                                                    dcMinCharges = 0.0;

                                                //compare minimum and entered amount for given charge
                                                //if (Convert.ToDouble(dcMinCharges) != 0.0 && Convert.ToDouble(dcCharges) != 0.0)
                                                // {
                                                if (Convert.ToDouble(dcMinCharges) > Convert.ToDouble(dcCharges))
                                                {
                                                    IsValidAmt = false;
                                                }
                                                // }
                                            }
                                        }
                                    }

                                    if (IsValidAmt == false)
                                    {
                                        IsValidDraft = false;
                                        QuotationOperations.UpdateDraftStatus(QuotationId, IsValidAmt, LoggedInUser.glUserId);
                                    }

                                    int result_ChargesApp = QuotationOperations.UpdateQuotationRates(RateId, QuotationId, QuotationCatgId, ChargeId, ApplicableFieldId, IsValidAmt, IsLumpSumField,
                                                                                   dcCharges, ExtraCharges, dcLumpSumCharges, LoggedInUser.glUserId);

                                    if (result_ChargesApp > 0)
                                    {
                                        result = 0;
                                    }
                                    else
                                    {
                                        result = -123; // error
                                    }
                                }

                                if (result == 0)
                                {
                                    DownloadQuotation(QuotationId);
                                    string QuotationRefNo = "";
                                    DataSet dsGetQuoteRefNo = QuotationOperations.GetParticularQuotation(QuotationId);
                                    if (dsGetQuoteRefNo != null && dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                                        QuotationRefNo = dsGetQuoteRefNo.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                                    lblError.Text = "Successfully Updated Draft Quotation - " + QuotationRefNo.Trim() + " .";
                                    lblError.CssClass = "success";

                                    if (IsValidDraft == false)
                                    {
                                        mpeApproval.Show();
                                    }
                                    else
                                    {
                                        int result_Status = QuotationOperations.UpdateDraftApprovalStatus(QuotationId, 6, LoggedInUser.glUserId);
                                        Response.Redirect("QuotationSuccess.aspx?id=" + QuotationId + "&op=1");
                                    }

                                    dvErrorSection.Visible = true;
                                    dvDraftSection.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Error while inserting record. Please try again later.";
                                    lblError.CssClass = "errorMsg";
                                }
                            }
                        }
                        else if (SavedDraft == -2)
                        {
                            lblError.Text = "Quotation already exists for Customer " + txtCustomer.Text.Trim() + " .";
                            lblError.CssClass = "errorMsg";
                        }
                        else
                        {
                            lblError.Text = "Error while inserting record. Please try again later.";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                    else
                    {
                        lblError.Text = "Please Enter Customer Name.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }
        }
    }

    protected void ResetControls()
    {
        ViewState["ChargesApplicable"] = null;
        ViewState["LpChargesTotal"] = "0";
        ViewState["ChargesMinTotal"] = "0";
        ViewState["QuotationId"] = null;
        ViewState["VSCharges"] = null;
        txtCustomer.Text = "";
        txtKindAttn.Text = "";
        txtNotes.Text = "";
        txtAddressLine1.Text = "";
        txtAddressLine2.Text = "";
        txtAddressLine3.Text = "";
        txtSubject.Text = "";
        txtTenderCustomer.Text = "";
        txtTerms.Text = "";
        ddlIncludeDesc.SelectedValue = "0";
        ddlIncludeDesc_OnSelectedIndexChanged(ddlIncludeDesc, EventArgs.Empty);
        cblModes.DataBind();
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" + DocumentPath;
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

    protected void chkLumpSum_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkLumpSum.Checked == true)
            {
                gvGenerateCharge.Columns[2].Visible = true;
                //gvGenerateCharge.Columns[5].Visible = false;
                gvGenerateCharge.Columns[5].Visible = true;
                gvGenerateCharge.Columns[4].Visible = false;
                // btnCreateLumpSum.Visible = true;

                if (gvGenerateCharge.Rows.Count > 0)
                {
                    for (int i = 0; i < gvGenerateCharge.Rows.Count; i++)
                    {
                        CheckBox chkItemForLumpSum = (CheckBox)gvGenerateCharge.Rows[i].FindControl("chkItemForLumpSum");
                        TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtLumpsumAmount");

                        if (chkItemForLumpSum != null)
                        {
                            if (chkItemForLumpSum.Checked == true)
                            {
                                txtLumpsumAmount.Enabled = true;
                            }
                            else
                                txtLumpsumAmount.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                gvGenerateCharge.Columns[2].Visible = false;
                // gvGenerateCharge.Columns[5].Visible = false;
                gvGenerateCharge.Columns[5].Visible = false;
                gvGenerateCharge.Columns[4].Visible = true;
                //btnCreateLumpSum.Visible = false;
            }
        }
        catch (Exception en)
        {

        }
    }

    protected int ValidateChargesApplicable()
    {
        int count = -1;
        Boolean CheckedLumpSum = false;
        string message = "";

        if (gvGenerateCharge.Rows.Count > 0)
        {
            if (chkLumpSum.Checked == true)
            {
                for (int i = 0; i < gvGenerateCharge.Rows.Count; i++)
                {
                    CheckBox chkItemForLumpSum = (CheckBox)gvGenerateCharge.Rows[i].FindControl("chkItemForLumpSum");
                    if (chkItemForLumpSum != null && chkItemForLumpSum.Checked == true)
                    {
                        CheckedLumpSum = true;
                        #region CHECK UNIT SELECTED OR NOT
                        DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[i].FindControl("ddlRanges_LumpSum");
                        Label lblParticulars = (Label)gvGenerateCharge.Rows[i].FindControl("lblParticulars");

                        if (ddlRanges_LumpSum != null)
                        {
                            if (ddlRanges_LumpSum.Items.Count > 1 && ddlRanges_LumpSum.SelectedValue == "0")
                            {
                                ddlRanges_LumpSum.Focus();
                                if (message == "")
                                    message = "- Please select atleast one unit for " + lblParticulars.Text.Trim();
                                else
                                    message = message + "\\n\\n" + "- Please select atleast one unit for " + lblParticulars.Text.Trim();
                                ShowErrorMessage(message);
                                count = -1;
                                break;
                            }
                            else
                            {
                                TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtLumpsumAmount");
                                if (txtLumpsumAmount != null)
                                {
                                    if (txtLumpsumAmount.Text == "")
                                    {
                                        txtLumpsumAmount.Focus();
                                        if (message == "")
                                            message = "- Please enter charges applicable for " + lblParticulars.Text.Trim();
                                        else
                                            message = message + "\\n\\n" + "- Please enter charges applicable for " + lblParticulars.Text.Trim();
                                        ShowErrorMessage(message);
                                        count = -1;
                                        break;
                                    }
                                    else
                                        count = 10;
                                }
                            }
                        }
                        #endregion

                        // count = 1;
                        // break;
                    }
                    else
                    {
                        if (CheckedLumpSum == false)
                            count = 0;
                    }
                }

                if (count == 0)
                {
                    message = "Please select atleast one line item to create lump sum for.";
                    ShowErrorMessage(message);
                }
            }
            else
            {
                for (int i = 0; i < gvGenerateCharge.Rows.Count; i++)
                {
                    DropDownList ddlRanges = (DropDownList)gvGenerateCharge.Rows[i].FindControl("ddlRanges");
                    Label lblParticulars = (Label)gvGenerateCharge.Rows[i].FindControl("lblParticulars");

                    if (ddlRanges != null)
                    {
                        if (ddlRanges.Items.Count > 1 && ddlRanges.SelectedValue == "0")
                        {
                            //chkItemForLumpSum.Checked = false;
                            ddlRanges.Focus();
                            if (message == "")
                                message = "- Please select atleast one unit for " + lblParticulars.Text.Trim();
                            else
                                message = message + "\\n\\n" + "- Please select atleast one unit for " + lblParticulars.Text.Trim();
                            ShowErrorMessage(message);
                            count = -1;
                            break;
                        }
                        else
                        {
                            TextBox txtCharges = (TextBox)gvGenerateCharge.Rows[i].FindControl("txtChargesApp");
                            if (txtCharges != null)
                            {
                                if (txtCharges.Text == "")
                                {
                                    txtCharges.Focus();
                                    if (message == "")
                                        message = "- Please enter charges applicable for " + lblParticulars.Text.Trim();
                                    else
                                        message = message + "\\n\\n" + "- Please enter charges applicable for " + lblParticulars.Text.Trim();
                                    ShowErrorMessage(message);
                                    count = -1;
                                    break;
                                }
                                else
                                    count = 10;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            count = -123;
            message = "Please select atleast one charges applicable for quotation..!!";
            ShowErrorMessage(message);
        }
        return count;
    }

    protected void ShowErrorMessage(string Message)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload=function(){");
        sb.Append("alert('");
        sb.Append(Message);
        sb.Append("')};");
        sb.Append("</script>");
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
    }

    protected void chkItemForLumpSum_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
        int index = row.RowIndex;
        CheckBox chkItemForLumpSum = (CheckBox)gvGenerateCharge.Rows[index].FindControl("chkItemForLumpSum");
        Label lblParticulars = (Label)gvGenerateCharge.Rows[index].FindControl("lblParticulars");

        if (chkLumpSum.Checked == true)
        {
            TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[index].FindControl("txtLumpsumAmount");
            DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[index].FindControl("ddlRanges_LumpSum");
            if (chkItemForLumpSum != null && chkItemForLumpSum.Checked == true)
            {
                txtLumpsumAmount.Enabled = true;
                if (ddlRanges_LumpSum != null && ddlRanges_LumpSum.SelectedValue == "0")
                {
                    if (ddlRanges_LumpSum.Items.Count == 2)
                    {
                        ddlRanges_LumpSum.SelectedIndex = 1;
                    }
                }
            }
            else
                txtLumpsumAmount.Enabled = false;
        }
        else
        {
            DropDownList ddlRanges = (DropDownList)gvGenerateCharge.Rows[index].FindControl("ddlRanges");
            TextBox txtChargesApp = (TextBox)gvGenerateCharge.Rows[index].FindControl("txtChargesApp");
            if (chkItemForLumpSum != null && chkItemForLumpSum.Checked == true)
            {
                txtChargesApp.Enabled = true;
                if (ddlRanges != null && ddlRanges.SelectedValue == "0")
                {
                    if (ddlRanges.Items.Count == 2)
                    {
                        ddlRanges.SelectedIndex = 1;
                    }
                }
            }
            else
                txtChargesApp.Enabled = false;
        }
    }

    #region GRID VIEW EVENTS

    protected void gvGenerateCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double number;
        if (chkLumpSum.Checked == true)
        {
            gvGenerateCharge.Columns[4].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Boolean IsLumpSumField = false;
                decimal MinAmt = 0;
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                if (rowView["IsLumpSumField"].ToString() != "")
                {
                    IsLumpSumField = Convert.ToBoolean(rowView["IsLumpSumField"].ToString());
                }

                if (Double.TryParse(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "LumpSumAmt")), out number))
                {
                    decimal LumpSumAmt = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "LumpSumAmt"));

                    if (DataBinder.Eval(e.Row.DataItem, "MinAmt") != DBNull.Value)
                        MinAmt = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "MinAmt"));
                    if (LumpSumAmt != 0)
                    {
                        ViewState["LumSumAmt"] = Convert.ToDecimal(ViewState["LumSumAmt"]) + LumpSumAmt;
                    }

                    if (MinAmt != 0)
                    {
                        ViewState["MinAmt"] = Convert.ToDecimal(ViewState["MinAmt"]) + MinAmt;
                    }
                }

                #region DROP DOWN EVENTS

                int ChargeId = 0;
                if (rowView["ChargeId"].ToString() != "")
                    ChargeId = Convert.ToInt32(rowView["ChargeId"].ToString());
                TextBox txtLumpsumAmount = (TextBox)e.Row.FindControl("txtLumpsumAmount");
                DropDownList ddlRanges = (DropDownList)e.Row.FindControl("ddlRanges_LumpSum");
                if (ddlRanges != null && ChargeId != 0)
                    FillDropDownList(ddlRanges, ChargeId);

                if (ddlRanges.Items.Count > 1)
                {
                    int drRange = Convert.ToInt32(rowView["ApplicableFieldId"].ToString());
                    if (drRange != 0)
                        ddlRanges.SelectedValue = drRange.ToString();

                    txtLumpsumAmount.Width = 100;
                    txtLumpsumAmount.TextMode = TextBoxMode.SingleLine;
                    ddlRanges.Width = 300;
                }
                else
                {
                    ddlRanges.Visible = false;
                    txtLumpsumAmount.Width = 410;
                    txtLumpsumAmount.TextMode = TextBoxMode.MultiLine;
                    txtLumpsumAmount.Rows = 2;
                }

                if (IsLumpSumField == true)
                {
                    txtLumpsumAmount.Enabled = true;
                    ddlRanges.Enabled = true;
                }
                else
                {
                    // + Modified On : 03-06-2017 
                    //txtLumpsumAmount.Enabled = false;
                    //ddlRanges.Enabled = false;
                    int StatusId = Convert.ToInt32(rowView["QuoteStatus"].ToString());
                    if (StatusId == 8) // Negotiated Quotation
                    {
                        txtLumpsumAmount.Enabled = true;
                        ddlRanges.Enabled = true;
                    }
                    else
                    {
                        txtLumpsumAmount.Enabled = false;
                        ddlRanges.Enabled = false;
                    }
                }

                #endregion

                #region CHECKBOX LUMPSUM

                CheckBox chkItemForLumpSum = (CheckBox)e.Row.FindControl("chkItemForLumpSum");
                if (chkItemForLumpSum != null)
                {
                    if (IsLumpSumField == true)
                        chkItemForLumpSum.Checked = true;
                    else
                        chkItemForLumpSum.Checked = false;
                }

                #endregion
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.BackColor = System.Drawing.Color.FromName("#5D7B9D");
                e.Row.Cells[0].Text = "";
                e.Row.Cells[3].Text = "";
                e.Row.Cells[3].Text = "<b>Min Total</b>";
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[3].ColumnSpan = 1;
                e.Row.Cells[5].Text = "<b>(" + ViewState["MinAmt"].ToString() + ")</b>";
            }

            if (ViewState["LumSumAmt"] != null)
                lblTotal2.Text = ViewState["LumSumAmt"].ToString();
            if (ViewState["MinAmt"] != null)
                lblMinTotal2.Text = ViewState["MinAmt"].ToString();
        }
        else
        {
            gvGenerateCharge.Columns[5].Visible = false;
            gvGenerateCharge.Columns[2].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                int ChargeId = 0;
                if (rowView["ChargeId"].ToString() != "")
                    ChargeId = Convert.ToInt32(rowView["ChargeId"].ToString());
                TextBox txtChargesApp = (TextBox)e.Row.FindControl("txtChargesApp");
                DropDownList ddlRanges = (DropDownList)e.Row.FindControl("ddlRanges");
                if (ddlRanges != null && ChargeId != 0)
                    FillDropDownList(ddlRanges, ChargeId);

                if (ddlRanges.Items.Count > 1)
                {
                    int drRange = Convert.ToInt32(rowView["ApplicableFieldId"].ToString());
                    if (drRange != 0)
                        ddlRanges.SelectedValue = drRange.ToString();

                    txtChargesApp.Width = 100;
                    txtChargesApp.TextMode = TextBoxMode.SingleLine;
                    ddlRanges.Width = 300;
                }
                else
                {
                    ddlRanges.Visible = false;
                    txtChargesApp.Width = 410;
                    txtChargesApp.TextMode = TextBoxMode.MultiLine;
                    txtChargesApp.Rows = 2;
                }

                // assign red color to invalid rows
                string IsValidAmount = rowView["IsValidAmount"].ToString();
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (IsValidAmount.Trim().ToLower() == "false")
                    {
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[3].Font.Bold = true;
                        e.Row.ToolTip = "Invalid charge..!! Minimum charge amount is " + rowView["MinAmt"].ToString() + " .";
                        ddlRanges.Enabled = true;
                        txtChargesApp.Enabled = true;
                    }
                    else
                    {
                        // + Modified On : 03-06-2017
                        //ddlRanges.Enabled = false;
                        //txtChargesApp.Enabled = false;
                        int StatusId = Convert.ToInt32(rowView["QuoteStatus"].ToString());
                        if (StatusId == 8) // Negotiated Quotation
                        {
                            ddlRanges.Enabled = true;
                            txtChargesApp.Enabled = true;
                        }
                        else
                        {
                            ddlRanges.Enabled = false;
                            txtChargesApp.Enabled = false;
                        }
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.BackColor = System.Drawing.Color.FromName("#5D7B9D");
            }
        }
    }

    protected void gvRFQDocuments_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "deletedoc")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int result = QuotationOperations.DeleteAnnexureDocs(lid, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
                if (result == 1)
                {
                    lblError.Text = "Successfully Document Deleted!";
                    lblError.CssClass = "success";
                    gvRFQDocuments.DataBind();
                }
                else
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void btnSaveDoc_OnClick(object sender, EventArgs e)
    {
        string fileName = "";
        if (fuRFQDoc != null && fuRFQDoc.HasFile)
        {
            fileName = UploadFiles(fuRFQDoc, "Annexure\\");
        }

        if (fileName != "")
        {
            int result = QuotationOperations.AddQuotationAnnexure(Convert.ToInt32(Session["QuotationId"]), fileName, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
            if (result == 1)
            {
                lblError.Text = "Document Added successfully!";
                lblError.CssClass = "success";
                gvRFQDocuments.DataBind();
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    private void FillDropDownList(DropDownList ddl, int ChargeId)
    {
        // ddl.Items.Clear();
        DataSet dsRangesList = QuotationOperations.GetChargeWsRangeDetails(ChargeId);
        if (dsRangesList != null)
        {
            ddl.DataSource = dsRangesList;
            ddl.DataTextField = "ApplicableOn";
            ddl.DataValueField = "ApplicableOnId";
            ddl.DataBind();
        }
    }

    protected void gvGenerateCharge_RowDataCommand(Object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        if (e.CommandName.ToLower().Trim() == "up")
        {
            index = Convert.ToInt32(e.CommandArgument);
            if (index != 0)
            {
                DataTable dtCurrent = (DataTable)ViewState["ChargesApplicable"];
                if (dtCurrent != null)
                {
                    for (int i = 0; i < dtCurrent.Rows.Count; i++)
                    {
                        Label lblchargeId = (Label)gvGenerateCharge.Rows[i].Cells[2].FindControl("lblChargeId");
                        Label txtParticulars = (Label)gvGenerateCharge.Rows[i].Cells[4].FindControl("lblParticulars");
                        TextBox txtChargesApp_Loc = (TextBox)gvGenerateCharge.Rows[i].Cells[5].FindControl("txtChargesApp");
                        DropDownList ddlRanges = (DropDownList)gvGenerateCharge.Rows[i].Cells[5].FindControl("ddlRanges");
                        TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[i].Cells[6].FindControl("txtLumpsumAmount");
                        DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[i].Cells[6].FindControl("ddlRanges_LumpSum");
                        Label lblCategoryId_Loc = (Label)gvGenerateCharge.Rows[i].Cells[7].FindControl("lblCategoryId");

                        dtCurrent.Rows[i]["ChargeId"] = lblchargeId.Text;
                        dtCurrent.Rows[i]["Particulars"] = txtParticulars.Text;
                        dtCurrent.Rows[i]["ChargesApplicable"] = txtChargesApp_Loc.Text;
                        dtCurrent.Rows[i]["ApplicableOn"] = ddlRanges.SelectedValue;
                        dtCurrent.Rows[i]["LpChargesApplicable"] = txtLumpsumAmount.Text;
                        dtCurrent.Rows[i]["LpApplicableOn"] = ddlRanges_LumpSum.SelectedValue;
                        dtCurrent.Rows[i]["CategoryId"] = lblCategoryId_Loc.Text;
                    }
                }

                // Set ChargeId
                int ChargeId = Convert.ToInt32(dtCurrent.Rows[index]["ChargeId"].ToString());
                int PrevChargeId = Convert.ToInt32(dtCurrent.Rows[index - 1]["ChargeId"].ToString());
                dtCurrent.Rows[index]["ChargeId"] = PrevChargeId;
                dtCurrent.Rows[index - 1]["ChargeId"] = ChargeId;
                Label lblChargeId = (Label)gvGenerateCharge.Rows[index].FindControl("lblChargeId");
                Label PrevlblChargeId = (Label)gvGenerateCharge.Rows[index - 1].FindControl("lblChargeId");

                if (PrevChargeId != 0)
                    PrevlblChargeId.Text = ChargeId.ToString();
                if (ChargeId != 0)
                    lblChargeId.Text = PrevChargeId.ToString();

                // Set Particulars
                string Particulars = dtCurrent.Rows[index]["Particulars"].ToString();
                string PrevParticulars = dtCurrent.Rows[index - 1]["Particulars"].ToString();
                dtCurrent.Rows[index]["Particulars"] = PrevParticulars;
                dtCurrent.Rows[index - 1]["Particulars"] = Particulars;
                Label lblParticulars = (Label)gvGenerateCharge.Rows[index].FindControl("lblParticulars");
                Label PrevlblParticulars = (Label)gvGenerateCharge.Rows[index - 1].FindControl("lblParticulars");

                if (Particulars != "")
                    PrevlblParticulars.Text = Particulars.ToString();
                if (PrevParticulars != "")
                    lblParticulars.Text = PrevParticulars.ToString();

                // Set ChargesApplicable
                string ChargesApplicable = dtCurrent.Rows[index]["ChargesApplicable"].ToString();
                string PrevChargesApplicable = dtCurrent.Rows[index - 1]["ChargesApplicable"].ToString();
                dtCurrent.Rows[index]["ChargesApplicable"] = PrevChargesApplicable;
                dtCurrent.Rows[index - 1]["ChargesApplicable"] = ChargesApplicable;
                TextBox txtChargesApp = (TextBox)gvGenerateCharge.Rows[index].FindControl("txtChargesApp");
                TextBox PrevtxtChargesApp = (TextBox)gvGenerateCharge.Rows[index - 1].FindControl("txtChargesApp");

                if (ChargesApplicable != "")
                    PrevtxtChargesApp.Text = ChargesApplicable.ToString();
                else
                    PrevtxtChargesApp.Text = "";
                if (PrevChargesApplicable != "")
                    txtChargesApp.Text = PrevChargesApplicable.ToString();
                else
                    txtChargesApp.Text = "";

                // Set ApplicableOn
                DropDownList ddlRange = (DropDownList)gvGenerateCharge.Rows[index].FindControl("ddlRanges");
                if (ddlRange != null)
                {
                    ddlRange.Items.Clear();
                }
                FillDropDownList(ddlRange, PrevChargeId);

                DropDownList ddlPrevRange = (DropDownList)gvGenerateCharge.Rows[index - 1].FindControl("ddlRanges");
                if (ddlPrevRange != null)
                {
                    ddlPrevRange.Items.Clear();
                }
                FillDropDownList(ddlPrevRange, ChargeId);

                int ApplicableOn = 0, PrevApplicableOn = 0;
                if (dtCurrent.Rows[index]["ApplicableOn"].ToString() != "")
                    ApplicableOn = Convert.ToInt32(dtCurrent.Rows[index]["ApplicableOn"].ToString());
                if (dtCurrent.Rows[index - 1]["ApplicableOn"].ToString() != "")
                    PrevApplicableOn = Convert.ToInt32(dtCurrent.Rows[index - 1]["ApplicableOn"].ToString());
                dtCurrent.Rows[index]["ApplicableOn"] = PrevApplicableOn;
                dtCurrent.Rows[index - 1]["ApplicableOn"] = ApplicableOn;

                if (ddlRange.Items.Count > 1)
                {
                    ddlRange.Visible = true;
                    txtChargesApp.Width = Unit.Pixel(100);
                    txtChargesApp.TextMode = TextBoxMode.SingleLine;
                    ddlRange.Width = Unit.Pixel(200);
                    if (PrevApplicableOn != 0)
                        ddlRange.SelectedValue = PrevApplicableOn.ToString();

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index].FindControl("revChargesApp");
                    revChargesApp.Visible = true;
                }
                else
                {
                    ddlRange.Visible = false;
                    txtChargesApp.Width = Unit.Pixel(310);
                    txtChargesApp.TextMode = TextBoxMode.MultiLine;
                    txtChargesApp.Rows = 2;

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index].FindControl("revChargesApp");
                    revChargesApp.Visible = false;
                }

                if (ddlPrevRange.Items.Count > 1)
                {
                    ddlPrevRange.Visible = true;
                    PrevtxtChargesApp.Width = Unit.Pixel(100);
                    PrevtxtChargesApp.TextMode = TextBoxMode.SingleLine;
                    ddlPrevRange.Width = Unit.Pixel(200);
                    if (ApplicableOn != 0)
                        ddlPrevRange.SelectedValue = ApplicableOn.ToString();

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index - 1].FindControl("revChargesApp");
                    revChargesApp.Visible = true;
                }
                else
                {
                    ddlPrevRange.Visible = false;
                    PrevtxtChargesApp.Width = Unit.Pixel(310);
                    PrevtxtChargesApp.TextMode = TextBoxMode.MultiLine;
                    PrevtxtChargesApp.Rows = 2;

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index - 1].FindControl("revChargesApp");
                    revChargesApp.Visible = false;
                }

                // Set CategoryId
                int CategoryId = Convert.ToInt32(dtCurrent.Rows[index]["CategoryId"].ToString());
                int PrevCategoryId = Convert.ToInt32(dtCurrent.Rows[index - 1]["CategoryId"].ToString());
                dtCurrent.Rows[index]["CategoryId"] = PrevCategoryId;
                dtCurrent.Rows[index - 1]["CategoryId"] = CategoryId;

                Label lblCategoryId = (Label)gvGenerateCharge.Rows[index].FindControl("lblCategoryId");
                Label PrevlblCategoryId = (Label)gvGenerateCharge.Rows[index - 1].FindControl("lblCategoryId");

                if (PrevCategoryId != 0)
                    PrevlblCategoryId.Text = CategoryId.ToString();
                if (CategoryId != 0)
                    lblCategoryId.Text = PrevCategoryId.ToString();

                ViewState["ChargesApplicable"] = dtCurrent;
            }
        }
        else if (e.CommandName.ToLower().Trim() == "down")
        {
            index = Convert.ToInt32(e.CommandArgument);
            int FinalCnt = gvGenerateCharge.Rows.Count - 1;
            if (index != FinalCnt)
            {
                DataTable dtCurrent = (DataTable)ViewState["ChargesApplicable"];
                if (dtCurrent != null)
                {
                    for (int i = 0; i < dtCurrent.Rows.Count; i++)
                    {
                        Label lblchargeId = (Label)gvGenerateCharge.Rows[i].Cells[2].FindControl("lblChargeId");
                        Label txtParticulars = (Label)gvGenerateCharge.Rows[i].Cells[4].FindControl("lblParticulars");
                        TextBox txtChargesApp_Loc = (TextBox)gvGenerateCharge.Rows[i].Cells[5].FindControl("txtChargesApp");
                        DropDownList ddlRanges = (DropDownList)gvGenerateCharge.Rows[i].Cells[5].FindControl("ddlRanges");
                        TextBox txtLumpsumAmount = (TextBox)gvGenerateCharge.Rows[i].Cells[6].FindControl("txtLumpsumAmount");
                        DropDownList ddlRanges_LumpSum = (DropDownList)gvGenerateCharge.Rows[i].Cells[6].FindControl("ddlRanges_LumpSum");
                        Label lblCategoryId_Loc = (Label)gvGenerateCharge.Rows[i].Cells[7].FindControl("lblCategoryId");

                        dtCurrent.Rows[i]["ChargeId"] = lblchargeId.Text;
                        dtCurrent.Rows[i]["Particulars"] = txtParticulars.Text;
                        dtCurrent.Rows[i]["ChargesApplicable"] = txtChargesApp_Loc.Text;
                        dtCurrent.Rows[i]["ApplicableOn"] = ddlRanges.SelectedValue;
                        dtCurrent.Rows[i]["LpChargesApplicable"] = txtLumpsumAmount.Text;
                        dtCurrent.Rows[i]["LpApplicableOn"] = ddlRanges_LumpSum.SelectedValue;
                        dtCurrent.Rows[i]["CategoryId"] = lblCategoryId_Loc.Text;
                    }
                }

                // Set ChargeId
                int ChargeId = Convert.ToInt32(dtCurrent.Rows[index]["ChargeId"].ToString());
                int PrevChargeId = Convert.ToInt32(dtCurrent.Rows[index + 1]["ChargeId"].ToString());
                dtCurrent.Rows[index]["ChargeId"] = PrevChargeId;
                dtCurrent.Rows[index + 1]["ChargeId"] = ChargeId;
                Label lblChargeId = (Label)gvGenerateCharge.Rows[index].FindControl("lblChargeId");
                Label PrevlblChargeId = (Label)gvGenerateCharge.Rows[index + 1].FindControl("lblChargeId");

                if (PrevChargeId != 0)
                    PrevlblChargeId.Text = ChargeId.ToString();
                if (ChargeId != 0)
                    lblChargeId.Text = PrevChargeId.ToString();

                // Set Particulars
                string Particulars = dtCurrent.Rows[index]["Particulars"].ToString();
                string PrevParticulars = dtCurrent.Rows[index + 1]["Particulars"].ToString();
                dtCurrent.Rows[index]["Particulars"] = PrevParticulars;
                dtCurrent.Rows[index + 1]["Particulars"] = Particulars;
                Label lblParticulars = (Label)gvGenerateCharge.Rows[index].FindControl("lblParticulars");
                Label PrevlblParticulars = (Label)gvGenerateCharge.Rows[index + 1].FindControl("lblParticulars");

                if (Particulars != "")
                    PrevlblParticulars.Text = Particulars.ToString();
                if (PrevParticulars != "")
                    lblParticulars.Text = PrevParticulars.ToString();

                // Set ChargesApplicable
                string ChargesApplicable = dtCurrent.Rows[index]["ChargesApplicable"].ToString();
                string PrevChargesApplicable = dtCurrent.Rows[index + 1]["ChargesApplicable"].ToString();
                dtCurrent.Rows[index]["ChargesApplicable"] = PrevChargesApplicable;
                dtCurrent.Rows[index + 1]["ChargesApplicable"] = ChargesApplicable;
                TextBox txtChargesApp = (TextBox)gvGenerateCharge.Rows[index].FindControl("txtChargesApp");
                TextBox PrevtxtChargesApp = (TextBox)gvGenerateCharge.Rows[index + 1].FindControl("txtChargesApp");

                if (ChargesApplicable != "")
                    PrevtxtChargesApp.Text = ChargesApplicable.ToString();
                else
                    PrevtxtChargesApp.Text = "";
                if (PrevChargesApplicable != "")
                    txtChargesApp.Text = PrevChargesApplicable.ToString();
                else
                    txtChargesApp.Text = "";

                // Set ApplicableOn
                DropDownList ddlRange = (DropDownList)gvGenerateCharge.Rows[index].FindControl("ddlRanges");
                if (ddlRange != null)
                {
                    ddlRange.Items.Clear();
                }
                FillDropDownList(ddlRange, PrevChargeId);

                DropDownList ddlPrevRange = (DropDownList)gvGenerateCharge.Rows[index + 1].FindControl("ddlRanges");
                if (ddlPrevRange != null)
                {
                    ddlPrevRange.Items.Clear();
                }
                FillDropDownList(ddlPrevRange, ChargeId);

                int ApplicableOn = 0, PrevApplicableOn = 0;
                if (dtCurrent.Rows[index]["ApplicableOn"].ToString() != "")
                    ApplicableOn = Convert.ToInt32(dtCurrent.Rows[index]["ApplicableOn"].ToString());
                if (dtCurrent.Rows[index + 1]["ApplicableOn"].ToString() != "")
                    PrevApplicableOn = Convert.ToInt32(dtCurrent.Rows[index + 1]["ApplicableOn"].ToString());
                dtCurrent.Rows[index]["ApplicableOn"] = PrevApplicableOn;
                dtCurrent.Rows[index + 1]["ApplicableOn"] = ApplicableOn;

                if (ddlRange.Items.Count > 1)
                {
                    ddlRange.Visible = true;
                    txtChargesApp.Width = Unit.Pixel(100);
                    txtChargesApp.TextMode = TextBoxMode.SingleLine;
                    ddlRange.Width = Unit.Pixel(200);

                    if (PrevApplicableOn != 0)
                        ddlRange.SelectedValue = PrevApplicableOn.ToString();

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index].FindControl("revChargesApp");
                    revChargesApp.Visible = true;
                }
                else
                {
                    ddlRange.Visible = false;
                    txtChargesApp.Width = Unit.Pixel(310);
                    txtChargesApp.TextMode = TextBoxMode.MultiLine;
                    txtChargesApp.Rows = 2;

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index].FindControl("revChargesApp");
                    revChargesApp.Visible = false;
                }

                if (ddlPrevRange.Items.Count > 1)
                {
                    ddlPrevRange.Visible = true;
                    PrevtxtChargesApp.Width = Unit.Pixel(100);
                    PrevtxtChargesApp.TextMode = TextBoxMode.SingleLine;
                    ddlPrevRange.Width = Unit.Pixel(200);

                    if (ApplicableOn != 0)
                        ddlPrevRange.SelectedValue = ApplicableOn.ToString();

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index + 1].FindControl("revChargesApp");
                    revChargesApp.Visible = true;
                }
                else
                {
                    ddlPrevRange.Visible = false;
                    PrevtxtChargesApp.Width = Unit.Pixel(310);
                    PrevtxtChargesApp.TextMode = TextBoxMode.MultiLine;
                    PrevtxtChargesApp.Rows = 2;

                    //set visibility of regular expression to allow only numbers
                    RegularExpressionValidator revChargesApp = (RegularExpressionValidator)gvGenerateCharge.Rows[index + 1].FindControl("revChargesApp");
                    revChargesApp.Visible = false;
                }

                // Set CategoryId
                int CategoryId = Convert.ToInt32(dtCurrent.Rows[index]["CategoryId"].ToString());
                int PrevCategoryId = Convert.ToInt32(dtCurrent.Rows[index + 1]["CategoryId"].ToString());
                dtCurrent.Rows[index]["CategoryId"] = PrevCategoryId;
                dtCurrent.Rows[index + 1]["CategoryId"] = CategoryId;

                Label lblCategoryId = (Label)gvGenerateCharge.Rows[index].FindControl("lblCategoryId");
                Label PrevlblCategoryId = (Label)gvGenerateCharge.Rows[index + 1].FindControl("lblCategoryId");

                if (PrevCategoryId != 0)
                    PrevlblCategoryId.Text = CategoryId.ToString();
                if (CategoryId != 0)
                    lblCategoryId.Text = PrevCategoryId.ToString();

                ViewState["ChargesApplicable"] = dtCurrent;
            }
        }
    }

    protected void btnDeleteCharge_OnClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.NamingContainer;

        int lid = Convert.ToInt32(gvGenerateCharge.DataKeys[gvRow.RowIndex].Value.ToString());
        if (lid != 0)
        {
            int result = QuotationOperations.DeleteQuotationRates(lid, LoggedInUser.glUserId);
            if (result == 1)
            {
                lblError.Text = "Successfully deleted charge..!!";
                lblError.CssClass = "success";
                DataSet dsGetChargesApp = QuotationOperations.GetQuoteReportData(Convert.ToInt32(Session["QuotationId"]));
                if (dsGetChargesApp != null)
                {
                    gvGenerateCharge.DataSource = dsGetChargesApp;
                    gvGenerateCharge.DataBind();
                }
            }
            else
            {
                lblError.Text = "System Error. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }


    }

    #endregion

    #region PDF EVENTS

    protected void DownloadQuotation(int QuotationId)
    {
        string CustomerName = "", TodaysDate = "", BodyContent_Addtnl = "";
        Boolean IsValidQuote = false, IsLumpSumCode = false;
        int StatusId = 0;

        DataSet dsGetQuotationDetails = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuotationDetails != null)
        {
            if (Convert.ToBoolean(dsGetQuotationDetails.Tables[0].Rows[0]["IsValidDraft"].ToString()) == true)
                IsValidQuote = true;

            if (dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"] != DBNull.Value && dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString() != "")
                CustomerName = dsGetQuotationDetails.Tables[0].Rows[0]["CustomerName"].ToString();

            if (Convert.ToBoolean(dsGetQuotationDetails.Tables[0].Rows[0]["IsLumpSumCode"]) == true)
                IsLumpSumCode = true;

            string date = DateTime.Today.ToShortDateString();
            string QuoteRefNo = dsGetQuotationDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
            string AddressLine1 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine1"].ToString();
            string AddressLine2 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine2"].ToString();
            string AddressLine3 = dsGetQuotationDetails.Tables[0].Rows[0]["AddressLine3"].ToString();
            string PersonName = dsGetQuotationDetails.Tables[0].Rows[0]["AttendedPerson"].ToString();
            string Subject = dsGetQuotationDetails.Tables[0].Rows[0]["Subject"].ToString();
            string BodyContent = dsGetQuotationDetails.Tables[0].Rows[0]["BodyContent"].ToString();
            string SignImgPath = dsGetQuotationDetails.Tables[0].Rows[0]["SignImgPath"].ToString();
            string PaymentTerms = dsGetQuotationDetails.Tables[0].Rows[0]["PaymentTerms"].ToString();
            int TermConditionId = Convert.ToInt32(dsGetQuotationDetails.Tables[0].Rows[0]["TermConditionId"].ToString());
            if (dsGetQuotationDetails.Tables[0].Rows[0]["StatusId"].ToString() != "")
                StatusId = Convert.ToInt32(dsGetQuotationDetails.Tables[0].Rows[0]["StatusId"].ToString());

            if (BodyContent == "")
                BodyContent = "This is in reference to the above subject; we are pleased to offer you our most competitive rates as below:";
            else
            {
                BodyContent_Addtnl = BodyContent;
                BodyContent = "";
            }

            if (dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"] != DBNull.Value)
                TodaysDate = Convert.ToDateTime(dsGetQuotationDetails.Tables[0].Rows[0]["QuoteDate"]).ToShortDateString();

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/LogoPDF1.jpg"));
            DataSet dsGetReportData = QuotationOperations.GetQuoteReportData(QuotationId);
            if (dsGetReportData != null)
            {
                if (dsGetReportData.Tables[0].Rows.Count > 0)
                {
                    int i = 0;
                    string Name = CustomerName.Replace("&amp;", "").ToString();
                    string filePath = Name.Replace(".", "");
                    filePath = GetFormattedName(filePath);
                    filePath = GetQuoteFileName(filePath);
                    string FileFullPath = GetQuotePath(filePath);
                    StringWriter sw = new StringWriter();
                    sw.Write("<br/>");
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    StringReader sr = new StringReader(sw.ToString());

                    iTextSharp.text.Rectangle recPDF = new iTextSharp.text.Rectangle(PageSize.A4);
                    Document pdfDoc = new Document(recPDF);

                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    FileStream fs = new FileStream(FileFullPath, FileMode.Create, FileAccess.Write);
                    PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, fs);
                    pdfDoc.Open();

                    Font GridHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    Font TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                    logo.SetAbsolutePosition(415, 715);
                    logo.ScaleAbsoluteHeight(100);
                    logo.ScaleAbsoluteWidth(130);
                    logo.Alignment = Convert.ToInt32(ImageAlign.Right);
                    pdfDoc.Add(logo);

                    if (IsValidQuote == true)
                    {
                        if (StatusId == 1)
                        {
                            #region ADD WATERMARK

                            string imageFilePath = Server.MapPath("~/Images/Draft.png");
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                            jpg.ScaleToFit(3000, 770);  // For give the size to image
                            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                            jpg.SetAbsolutePosition(7, 69);  // give absolute/specified fix position to image.
                            pdfwriter.PageEvent = new ImageBackgroundHelper(jpg);
                            pdfDoc.Add(jpg);

                            #endregion
                        }

                        string contents = "";
                        contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                        contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                        contents = contents.Replace("[Today's Date]", date.ToString());
                        contents = contents.Replace("[CustomerName]", CustomerName);
                        contents = contents.Replace("[AddressLine1]", AddressLine1);
                        if (AddressLine2 == String.Empty)
                            contents = contents.Replace("[AddressLine2]", String.Empty);
                        else
                            contents = contents.Replace("[AddressLine2]", AddressLine2);
                        if (AddressLine3 == String.Empty)
                            contents = contents.Replace("[AddressLine3]", String.Empty);
                        else
                            contents = contents.Replace("[AddressLine3]", AddressLine3);
                        contents = contents.Replace("[PersonName]", PersonName);
                        contents = contents.Replace("[Subject]", Subject);
                        contents = contents.Replace("[BodyContent]", BodyContent);
                        contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

                        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                        foreach (var htmlelement in parsedContent)
                            pdfDoc.Add(htmlelement as IElement);

                        PdfPTable pdftable = new PdfPTable(3);
                        pdftable.TotalWidth = 500f;
                        pdftable.LockedWidth = true;
                        float[] widths = new float[] { 0.18f, 1.2f, 0.8f };
                        pdftable.SetWidths(widths);
                        pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Set Table Spacing Before And After html text
                        pdftable.SpacingBefore = 10f;
                        pdftable.SpacingAfter = 20f;

                        #region Create Table Column Header Cell with Text
                        // Header: Serial Number
                        PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                        cellwithdata.Colspan = 1;
                        cellwithdata.BorderWidth = 1f;
                        cellwithdata.HorizontalAlignment = 0;//left
                        cellwithdata.VerticalAlignment = 0;// Center
                        cellwithdata.Padding = 5;
                        cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata);

                        // Header: Particulars
                        PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellwithdata1.Colspan = 1;
                        cellwithdata1.BorderWidth = 1f;
                        cellwithdata1.HorizontalAlignment = 0;
                        cellwithdata1.Padding = 5;
                        cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata1);

                        // Header: Charges Applicable
                        PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellwithdata2.Colspan = 1;
                        cellwithdata2.BorderWidth = 1f;
                        cellwithdata2.HorizontalAlignment = 0;
                        cellwithdata2.Padding = 5;
                        cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        pdftable.AddCell(cellwithdata2);

                        #endregion

                        #region ADD CELL TO DATATABLE IN PDF
                        int SrNo = 1;
                        foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                        {
                            // Data Cell: Serial Number - Auto Increment Cell
                            PdfPCell SrnoCell = new PdfPCell();
                            SrnoCell.Colspan = 1;
                            SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;

                            // Data Cell: Particulars Cell
                            PdfPCell ParticularCell = new PdfPCell();
                            ParticularCell.Colspan = 1;

                            // Data Cell:  Charges Applicable
                            PdfPCell ChargesCell = new PdfPCell();
                            ChargesCell.Colspan = 1;
                            ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;

                            TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                            i = i + 1;

                            if (IsLumpSumCode == true)
                            {
                                if (Convert.ToBoolean(dsGetReportData.Tables[0].Rows[i - 1]["IsLumpSumField"]).ToString().ToLower().Trim() == "true")
                                {
                                    // Serial number #
                                    SrnoCell.Phrase = new Phrase(Convert.ToString(SrNo), TextFontformat);
                                    SrnoCell.Padding = 5;
                                    SrnoCell.PaddingTop = 8;
                                    SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdftable.AddCell(SrnoCell);

                                    // Particulars
                                    Paragraph paraParticulars = new Paragraph();
                                    paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                    ParticularCell.AddElement(paraParticulars);
                                    //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                    ParticularCell.PaddingBottom = 8;
                                    ParticularCell.PaddingLeft = 5;
                                    ParticularCell.PaddingRight = 5;
                                    pdftable.AddCell(ParticularCell);

                                    // Charges Applicable
                                    Paragraph paraChargesApp = new Paragraph();
                                    paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat));
                                    ChargesCell.AddElement(paraChargesApp);
                                    //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat);
                                    ChargesCell.PaddingBottom = 8;
                                    ChargesCell.PaddingLeft = 5;
                                    ChargesCell.PaddingRight = 5;
                                    pdftable.AddCell(ChargesCell);
                                    SrNo = SrNo + 1;
                                }
                            }
                            else
                            {
                                // Serial number #
                                SrnoCell.Phrase = new Phrase(Convert.ToString(SrNo), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingTop = 8;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                pdftable.AddCell(SrnoCell);

                                // Particulars
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);

                                //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;
                                pdftable.AddCell(ParticularCell);

                                // Charges Applicable
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);

                                //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;
                                pdftable.AddCell(ChargesCell);
                                SrNo = SrNo + 1;
                            }
                        }

                        #endregion
                        pdfDoc.Add(pdftable);
                    }
                    else
                    {
                        if (IsLumpSumCode == true)
                        {
                            #region LUMP SUM CODE

                            #region ADD WATERMARK

                            string imageFilePath = Server.MapPath("~/Images/Draft.png");

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                            //Resize image depend upon your need
                            //For give the size to image
                            jpg.ScaleToFit(3000, 770);

                            //If you want to choose image as background then,
                            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                            //If you want to give absolute/specified fix position to image.
                            jpg.SetAbsolutePosition(7, 69);
                            pdfDoc.Add(jpg);
                            #endregion

                            string contents = "";
                            contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                            contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                            contents = contents.Replace("[Today's Date]", date.ToString());
                            contents = contents.Replace("[CustomerName]", CustomerName);
                            contents = contents.Replace("[AddressLine1]", AddressLine1);
                            if (AddressLine2 == String.Empty)
                                contents = contents.Replace("[AddressLine2]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine2]", AddressLine2);
                            if (AddressLine3 == String.Empty)
                                contents = contents.Replace("[AddressLine3]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine3]", AddressLine3);
                            contents = contents.Replace("[PersonName]", PersonName);
                            contents = contents.Replace("[Subject]", Subject);
                            contents = contents.Replace("[BodyContent]", BodyContent);
                            contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

                            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                            foreach (var htmlelement in parsedContent)
                                pdfDoc.Add(htmlelement as IElement);

                            PdfPTable pdftable = new PdfPTable(3);
                            pdftable.TotalWidth = 500f;
                            pdftable.LockedWidth = true;
                            float[] widths = new float[] { 0.18f, 1.1f, 0.9f };
                            pdftable.SetWidths(widths);
                            pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Set Table Spacing Before And After html text
                            pdftable.SpacingBefore = 10f;
                            pdftable.SpacingAfter = 4f;

                            #region  Create Table Column Header Cell with Text
                            // Header: Serial Number
                            PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                            cellwithdata.Colspan = 1;
                            cellwithdata.BorderWidth = 1f;
                            cellwithdata.HorizontalAlignment = 0;//left
                            cellwithdata.VerticalAlignment = 0;// Center
                            cellwithdata.Padding = 5;
                            cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata);

                            // Header: Particulars
                            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                            cellwithdata1.Colspan = 1;
                            cellwithdata1.BorderWidth = 1f;
                            cellwithdata1.HorizontalAlignment = 0;
                            cellwithdata1.Padding = 5;
                            cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata1);

                            // Header: Charges Applicable
                            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                            cellwithdata2.Colspan = 1;
                            cellwithdata2.BorderWidth = 1f;
                            cellwithdata2.HorizontalAlignment = 0;
                            cellwithdata2.Padding = 5;
                            cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata2);

                            #endregion

                            #region ADD CELL TO DATATABLE IN PDF
                            Decimal dcTotal = 0, dcMinTotal = 0;
                            foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                            {
                                i = i + 1;
                                TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                                // Data Cell: Serial Number - Auto Increment Cell
                                PdfPCell SrnoCell = new PdfPCell();
                                SrnoCell.Colspan = 1;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingBottom = 8;
                                pdftable.AddCell(SrnoCell);

                                // Data Cell: Particulars Cell
                                PdfPCell ParticularCell = new PdfPCell();
                                ParticularCell.Colspan = 1;
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);
                                // ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;
                                pdftable.AddCell(ParticularCell);

                                // Data Cell:  Charges Applicable
                                PdfPCell ChargesCell = new PdfPCell();
                                ChargesCell.Colspan = 1;
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);
                                ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;
                                // ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LpChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;
                                pdftable.AddCell(ChargesCell);

                                if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LumpSumAmt"]) != "")
                                {
                                    decimal LumpSumAmt = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["LumpSumAmt"]));
                                    if (LumpSumAmt != 0)
                                        dcTotal = dcTotal + LumpSumAmt;
                                }

                                if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinAmt"]) != "")
                                {
                                    decimal Mintotal = Convert.ToDecimal(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinAmt"]));
                                    if (Mintotal != 0)
                                        dcMinTotal = dcMinTotal + Mintotal;
                                }
                            }
                            #endregion
                            pdfDoc.Add(pdftable);

                            #region ADD TOTAL ROW

                            PdfPTable table_Total = new PdfPTable(3);
                            table_Total.TotalWidth = 500f;
                            table_Total.LockedWidth = true;
                            float[] widths2 = new float[] { 0.18f, 1.1f, 0.9f };
                            table_Total.SetWidths(widths2);
                            table_Total.HorizontalAlignment = Element.ALIGN_CENTER;
                            table_Total.SpacingBefore = 0f;
                            table_Total.SpacingAfter = 20f;

                            // 1st Data Cell: Blank column
                            PdfPCell Cell1 = new PdfPCell();
                            Cell1.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell1);

                            // 2nd Data Cell: Blank column
                            PdfPCell Cell2 = new PdfPCell();
                            Cell2.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell2);

                            // 3rd Data Cell: Total
                            PdfPCell Total = new PdfPCell();
                            Total.Colspan = 1;
                            Total.HorizontalAlignment = 0;
                            Total.Phrase = new Phrase(Convert.ToString("Total:" + " " + dcTotal), GridHeadingFont);
                            Total.Padding = 5;
                            if (dcTotal < dcMinTotal)
                                Total.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                            table_Total.AddCell(Total);

                            // 1st Data Cell: Blank column
                            PdfPCell Cell3 = new PdfPCell();
                            Cell3.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell3);

                            // 2nd Data Cell: Blank column
                            PdfPCell Cell4 = new PdfPCell();
                            Cell4.Border = Rectangle.NO_BORDER;
                            table_Total.AddCell(Cell4);

                            // 3rd Data Cell: Min Total
                            PdfPCell MinTotal = new PdfPCell();
                            MinTotal.Colspan = 1;
                            MinTotal.HorizontalAlignment = 0;
                            MinTotal.Phrase = new Phrase(Convert.ToString("Minimum Total:" + " " + dcMinTotal), GridHeadingFont);
                            MinTotal.Padding = 5;
                            table_Total.AddCell(MinTotal);
                            pdfDoc.Add(table_Total);

                            #endregion

                            Paragraph ParaSpacing_LumpSum = new Paragraph();
                            ParaSpacing_LumpSum.SpacingBefore = 20;//5
                            Font NoteHeadingFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.RED);

                            if (dcTotal < dcMinTotal)
                            {
                                pdfDoc.Add(new Paragraph("    NOTE: The total of charges applicable (i.e., " + dcTotal + ") is less than the actual minimum total (i.e., " + dcMinTotal + ").", NoteHeadingFont));
                                pdfDoc.Add(ParaSpacing_LumpSum);
                            }
                            #endregion
                        }
                        else
                        {
                            #region OTHER THAN LUMP SUM CODE

                            #region ADD WATERMARK

                            string imageFilePath = Server.MapPath("~/Images/Draft.png");

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                            jpg.ScaleToFit(3000, 770);   //For give the size to image
                            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                            jpg.SetAbsolutePosition(7, 69); //give absolute/specified fix position to image.
                            pdfwriter.PageEvent = new ImageBackgroundHelper(jpg);
                            pdfDoc.Add(jpg);
                            #endregion

                            string contents = "";
                            contents = File.ReadAllText(Server.MapPath("~/Quotation/Quotation.htm"));
                            contents = contents.Replace("[QuoteRefNo]", QuoteRefNo);
                            contents = contents.Replace("[Today's Date]", date.ToString());
                            contents = contents.Replace("[CustomerName]", CustomerName);
                            contents = contents.Replace("[AddressLine1]", AddressLine1);
                            if (AddressLine2 == String.Empty)
                                contents = contents.Replace("[AddressLine2]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine2]", AddressLine2);
                            if (AddressLine3 == String.Empty)
                                contents = contents.Replace("[AddressLine3]", String.Empty);
                            else
                                contents = contents.Replace("[AddressLine3]", AddressLine3);
                            contents = contents.Replace("[PersonName]", PersonName);
                            contents = contents.Replace("[Subject]", Subject);
                            contents = contents.Replace("[BodyContent]", BodyContent);
                            contents = contents.Replace("[BodyContent_Addtnl]", BodyContent_Addtnl);

                            var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
                            foreach (var htmlelement in parsedContent)
                                pdfDoc.Add(htmlelement as IElement);

                            PdfPTable pdftable = new PdfPTable(4);
                            pdftable.TotalWidth = 500f;
                            pdftable.LockedWidth = true;
                            float[] widths = new float[] { 0.16f, 1.0f, 0.5f, 0.5f };
                            pdftable.SetWidths(widths);
                            pdftable.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Set Table Spacing Before And After html text
                            pdftable.SpacingBefore = 10f;
                            pdftable.SpacingAfter = 20f;

                            #region Create Table Column Header Cell with Text
                            // Header: Serial Number
                            PdfPCell cellwithdata = new PdfPCell(new Phrase("Sr.No", GridHeadingFont));
                            cellwithdata.Colspan = 1;
                            cellwithdata.BorderWidth = 1f;
                            cellwithdata.HorizontalAlignment = 0;//left
                            cellwithdata.VerticalAlignment = 0;// Center
                            cellwithdata.Padding = 5;
                            cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata);

                            // Header: Particulars
                            PdfPCell cellwithdata1 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                            cellwithdata1.Colspan = 1;
                            cellwithdata1.BorderWidth = 1f;
                            cellwithdata1.HorizontalAlignment = 0;
                            cellwithdata1.Padding = 5;
                            cellwithdata1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata1);

                            // Header: Charges Applicable
                            PdfPCell cellwithdata2 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                            cellwithdata2.Colspan = 1;
                            cellwithdata2.BorderWidth = 1f;
                            cellwithdata2.HorizontalAlignment = 0;
                            cellwithdata2.Padding = 5;
                            cellwithdata2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata2);

                            // Header: Minimum Charges
                            PdfPCell cellwithdata3 = new PdfPCell(new Phrase("Minimum Charges", GridHeadingFont));
                            cellwithdata3.Colspan = 1;
                            cellwithdata3.BorderWidth = 1f;
                            cellwithdata3.HorizontalAlignment = 0;
                            cellwithdata3.Padding = 5;
                            cellwithdata3.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                            pdftable.AddCell(cellwithdata3);
                            #endregion

                            #region ADD CELL TO DATATABLE IN PDF
                            foreach (DataRow dr in dsGetReportData.Tables[0].Rows)
                            {
                                TextFontformat = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                                i = i + 1;

                                // Data Cell: Serial Number - Auto Increment Cell
                                PdfPCell SrnoCell = new PdfPCell();
                                SrnoCell.Colspan = 1;
                                SrnoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.VerticalAlignment = Element.ALIGN_CENTER;
                                SrnoCell.Phrase = new Phrase(Convert.ToString(i), TextFontformat);
                                SrnoCell.Padding = 5;
                                SrnoCell.PaddingBottom = 8;

                                // Data Cell: Particulars Cell
                                PdfPCell ParticularCell = new PdfPCell();
                                ParticularCell.Colspan = 1;
                                Paragraph paraParticulars = new Paragraph();
                                paraParticulars.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat));
                                ParticularCell.AddElement(paraParticulars);
                                //ParticularCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["Particulars"]), TextFontformat);
                                ParticularCell.PaddingBottom = 8;
                                ParticularCell.PaddingLeft = 5;
                                ParticularCell.PaddingRight = 5;

                                // Data Cell:  Charges Applicable
                                PdfPCell ChargesCell = new PdfPCell();
                                ChargesCell.Colspan = 1;
                                Paragraph paraChargesApp = new Paragraph();
                                paraChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat));
                                ChargesCell.AddElement(paraChargesApp);
                                ChargesCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                ChargesCell.VerticalAlignment = Element.ALIGN_LEFT;
                                //ChargesCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["ChargesApplicable"]), TextFontformat);
                                ChargesCell.PaddingBottom = 8;
                                ChargesCell.PaddingLeft = 5;
                                ChargesCell.PaddingRight = 5;

                                // Data Cell:  Minimum Charges Applicable
                                PdfPCell MinCell = new PdfPCell();
                                MinCell.Colspan = 1;
                                Paragraph paraMinChargesApp = new Paragraph();
                                paraMinChargesApp.Add(new Paragraph(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]), TextFontformat));
                                MinCell.AddElement(paraMinChargesApp);
                                MinCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                MinCell.VerticalAlignment = Element.ALIGN_LEFT;
                                //MinCell.Phrase = new Phrase(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]), TextFontformat);
                                MinCell.PaddingBottom = 8;
                                MinCell.PaddingLeft = 5;
                                MinCell.PaddingRight = 5;

                                Boolean IsValidDraft = Convert.ToBoolean(Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["IsValidAmount"]));
                                if (Convert.ToString(dsGetReportData.Tables[0].Rows[i - 1]["MinChargesApplicable"]) != "")
                                {
                                    if (IsValidDraft == false)
                                    {
                                        SrnoCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                        ParticularCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                        ChargesCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                        MinCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 200, 200);
                                    }
                                }

                                pdftable.AddCell(SrnoCell);
                                pdftable.AddCell(ParticularCell);
                                pdftable.AddCell(ChargesCell);
                                pdftable.AddCell(MinCell);
                            }

                            #endregion

                            pdfDoc.Add(pdftable);

                            #endregion
                        }
                    }

                    Paragraph ParaSpacing = new Paragraph();
                    ParaSpacing.SpacingBefore = 20;//5                         

                    #region TRANSPORTATION CHARGES

                    DataSet dsGetAnnexure = QuotationOperations.GetTransportationCharges(QuotationId);
                    if (dsGetAnnexure != null && dsGetAnnexure.Tables[0].Rows.Count > 0)
                    {
                        PdfPTable tblAnnexure = new PdfPTable(3);
                        tblAnnexure.TotalWidth = 500f;
                        tblAnnexure.LockedWidth = true;
                        float[] widths_Annexure = new float[] { 0.4f, 1.7f, 1.7f };
                        tblAnnexure.SetWidths(widths_Annexure);
                        tblAnnexure.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblAnnexure.SpacingBefore = 5f;
                        tblAnnexure.SpacingAfter = 4f;

                        // Header: Serial Number
                        PdfPCell cellAnnexure_Header = new PdfPCell(new Phrase("Transportation Charges", GridHeadingFont));
                        cellAnnexure_Header.Colspan = 3;
                        cellAnnexure_Header.HorizontalAlignment = 0;//left
                        cellAnnexure_Header.VerticalAlignment = 0;// Center
                        cellAnnexure_Header.Padding = 5;
                        cellAnnexure_Header.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure_Header);

                        PdfPCell cellAnnexure1 = new PdfPCell(new Phrase("Sr.No.", GridHeadingFont));
                        cellAnnexure1.Colspan = 1;
                        cellAnnexure1.HorizontalAlignment = 0;//left
                        cellAnnexure1.VerticalAlignment = 0;// Center
                        cellAnnexure1.Padding = 5;
                        cellAnnexure1.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure1);

                        // Header: Particulars
                        PdfPCell cellAnnexure2 = new PdfPCell(new Phrase("Particulars", GridHeadingFont));
                        cellAnnexure2.Colspan = 1;
                        cellAnnexure2.HorizontalAlignment = 0;//left
                        cellAnnexure2.VerticalAlignment = 0;// Center
                        cellAnnexure2.Padding = 5;
                        cellAnnexure2.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure2);

                        // Header: Charges Applicable
                        PdfPCell cellAnnexure3 = new PdfPCell(new Phrase("Charges Applicable", GridHeadingFont));
                        cellAnnexure3.Colspan = 1;
                        cellAnnexure3.HorizontalAlignment = 0;//left
                        cellAnnexure3.VerticalAlignment = 0;// Center
                        cellAnnexure3.Padding = 5;
                        cellAnnexure3.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblAnnexure.AddCell(cellAnnexure3);

                        int cnt_SrNo = 1;
                        foreach (DataRow dr in dsGetAnnexure.Tables[0].Rows)
                        {
                            // Data Cell: Sr.No.
                            PdfPCell SrNo_Annx = new PdfPCell();
                            SrNo_Annx.Colspan = 1;
                            SrNo_Annx.HorizontalAlignment = Element.ALIGN_LEFT;
                            SrNo_Annx.VerticalAlignment = Element.ALIGN_LEFT;
                            SrNo_Annx.Phrase = new Phrase(Convert.ToString(cnt_SrNo), TextFontformat);
                            SrNo_Annx.Padding = 5;
                            tblAnnexure.AddCell(SrNo_Annx);

                            // Data Cell: Particulars
                            PdfPCell Annx_Part = new PdfPCell();
                            Annx_Part.Colspan = 1;
                            Annx_Part.HorizontalAlignment = Element.ALIGN_LEFT;
                            Annx_Part.VerticalAlignment = Element.ALIGN_LEFT;
                            Annx_Part.Phrase = new Phrase(Convert.ToString(dr["Particulars"].ToString()), TextFontformat);
                            Annx_Part.Padding = 5;
                            tblAnnexure.AddCell(Annx_Part);

                            // Data Cell: Charges Applicable
                            PdfPCell Annx_ChgApp = new PdfPCell();
                            Annx_ChgApp.Colspan = 1;
                            Annx_ChgApp.HorizontalAlignment = Element.ALIGN_LEFT;
                            Annx_ChgApp.VerticalAlignment = Element.ALIGN_LEFT;
                            Annx_ChgApp.Phrase = new Phrase(Convert.ToString(dr["ChargesApplicable"].ToString()), TextFontformat);
                            Annx_ChgApp.Padding = 5;
                            tblAnnexure.AddCell(Annx_ChgApp);

                            cnt_SrNo = cnt_SrNo + 1;
                        }
                        pdfDoc.Add(tblAnnexure);
                        pdfDoc.Add(ParaSpacing);
                    }

                    #endregion

                    #region PAYMENT TERMS
                    if (PaymentTerms != "")
                    {
                        PdfPTable tblTerms = new PdfPTable(1);
                        tblTerms.TotalWidth = 500f;
                        tblTerms.LockedWidth = true;
                        float[] widths_Terms = new float[] { 2.2f };
                        tblTerms.SetWidths(widths_Terms);
                        tblTerms.HorizontalAlignment = Element.ALIGN_CENTER;
                        tblTerms.SpacingBefore = 10f;
                        tblTerms.SpacingAfter = 4f;

                        PdfPCell cell = new PdfPCell(new Phrase("Payment Terms", GridHeadingFont));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 0;//left
                        cell.VerticalAlignment = 0;// Center
                        cell.Padding = 5;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                        tblTerms.AddCell(cell);

                        // Data Cell: Payment Terms
                        PdfPCell Terms = new PdfPCell();
                        Terms.Colspan = 1;
                        Terms.HorizontalAlignment = Element.ALIGN_LEFT;
                        Terms.VerticalAlignment = Element.ALIGN_LEFT;
                        Terms.Phrase = new Phrase(Convert.ToString(PaymentTerms), TextFontformat);
                        Terms.Padding = 5;
                        tblTerms.AddCell(Terms);
                        pdfDoc.Add(tblTerms);
                        pdfDoc.Add(ParaSpacing);
                    }
                    #endregion

                    #region TERMS & CONDITIONS

                    PdfPTable tblTermsCondition = new PdfPTable(2);
                    tblTermsCondition.TotalWidth = 500f;
                    tblTermsCondition.LockedWidth = true;
                    float[] widths_TermsCon = new float[] { 0.2f, 2.1f };
                    tblTermsCondition.SetWidths(widths_TermsCon);
                    tblTermsCondition.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblTermsCondition.SpacingBefore = 5f;
                    tblTermsCondition.SpacingAfter = 4f;

                    PdfPCell cellTerms_Header = new PdfPCell(new Phrase("Terms & Conditions :", GridHeadingFont));
                    cellTerms_Header.Colspan = 2;
                    cellTerms_Header.HorizontalAlignment = 0;//left
                    cellTerms_Header.VerticalAlignment = 0;// Center
                    cellTerms_Header.Padding = 5;
                    cellTerms_Header.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
                    tblTermsCondition.AddCell(cellTerms_Header);

                    DataSet dsGetTermsDetails = QuotationOperations.GetTermConditionDetails(TermConditionId);
                    if (dsGetTermsDetails != null && dsGetTermsDetails.Tables[0].Rows.Count > 0)
                    {
                        int cnt_SrNo = 1;
                        foreach (DataRow dr in dsGetTermsDetails.Tables[0].Rows)
                        {
                            // Data Cell: Sr.No.
                            PdfPCell SrNo_Terms = new PdfPCell();
                            SrNo_Terms.Colspan = 1;
                            SrNo_Terms.HorizontalAlignment = Element.ALIGN_CENTER;
                            SrNo_Terms.VerticalAlignment = Element.ALIGN_CENTER;
                            SrNo_Terms.Phrase = new Phrase(Convert.ToString(cnt_SrNo), TextFontformat);
                            SrNo_Terms.Padding = 5;
                            tblTermsCondition.AddCell(SrNo_Terms);

                            // Data Cell: Payment Terms
                            PdfPCell Terms_Name = new PdfPCell();
                            Terms_Name.Colspan = 1;
                            Terms_Name.HorizontalAlignment = Element.ALIGN_LEFT;
                            Terms_Name.VerticalAlignment = Element.ALIGN_LEFT;
                            Terms_Name.Phrase = new Phrase(Convert.ToString(dr["sTermCondition"].ToString()), TextFontformat);
                            Terms_Name.Padding = 5;
                            tblTermsCondition.AddCell(Terms_Name);
                            cnt_SrNo = cnt_SrNo + 1;
                        }
                    }

                    pdfDoc.Add(tblTermsCondition);

                    #endregion

                    pdfDoc.Add(ParaSpacing);
                    pdfDoc.Add(new Paragraph("    We hope the above rates are in order and look forward for long lasting association with your esteemed ", TextFontformat));
                    pdfDoc.Add(new Paragraph("    organization.", TextFontformat));
                    pdfDoc.Add(ParaSpacing);

                    pdfDoc.Add(new Paragraph("    For Babaji Shivram Clearing & Carriers Pvt Ltd", GridHeadingFont));
                    pdfDoc.Add(ParaSpacing);

                    #region SIGNATURE IMAGE 
                    if (SignImgPath != "")
                    {
                        string ImagePath = DownloadImage(SignImgPath);
                        iTextSharp.text.Image imgSignImgPath = iTextSharp.text.Image.GetInstance(ImagePath);
                        imgSignImgPath.ScaleAbsoluteWidth(60);
                        PdfPTable tblSign = new PdfPTable(1);
                        tblSign.TotalWidth = 50f;
                        tblSign.LockedWidth = true;
                        float[] widths_Sign = new float[] { 2.0f };
                        tblSign.SetWidths(widths_Sign);
                        tblSign.HorizontalAlignment = Element.ALIGN_LEFT;
                        tblSign.SpacingBefore = 2;

                        PdfPCell Terms = new PdfPCell();
                        Terms.Colspan = 1;
                        Terms.HorizontalAlignment = Element.ALIGN_LEFT;
                        Terms.VerticalAlignment = Element.ALIGN_LEFT;
                        Terms.Border = Rectangle.NO_BORDER;
                        Terms.AddElement(imgSignImgPath);
                        Terms.PaddingLeft = 10;
                        tblSign.AddCell(Terms);
                        pdfDoc.Add(tblSign);
                        Paragraph ParaSpacing_Sign = new Paragraph();
                        ParaSpacing_Sign.SpacingAfter = 3;
                    }
                    #endregion

                    pdfDoc.Add(new Paragraph("    " + dsGetReportData.Tables[0].Rows[0]["CreatedBy"].ToString(), TextFontformat));
                    pdfDoc.Add(new Paragraph("    Authorized Signatory", GridHeadingFont));
                    pdfDoc.Add(ParaSpacing);

                    iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/FooterET.png"));
                    footer.SetAbsolutePosition(40, 10);
                    footer.ScaleAbsoluteWidth(480);
                    footer.ScaleAbsoluteHeight(100);
                    pdfDoc.Add(footer);
                    pdfDoc.Add(ParaSpacing);

                    htmlparser.Parse(sr);
                    int DocPath = 0;
                    if (QuotationId != 0 && filePath != "")
                        DocPath = QuotationOperations.AddQuotationCopy(QuotationId, filePath + ".pdf");

                    pdfDoc.Close();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
    }

    protected string GetFormattedName(string FileName)
    {
        FileName = FileName.Replace(".", "");
        FileName = FileName.Replace("/", "");
        FileName = FileName.Replace("\"", "");
        FileName = FileName.Replace("@", "");
        FileName = FileName.Replace("#", "");
        FileName = FileName.Replace("(", "");
        FileName = FileName.Replace(")", "");
        FileName = FileName.Replace(",", "");
        FileName = FileName.Replace(";", "");
        FileName = FileName.Replace(":", "");
        return FileName;
    }

    public string GetQuoteFileName(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (ServerFilePath != "")
        {
            if (System.IO.File.Exists(ServerFilePath + ".pdf"))
            {
                string FileId = RandomString(5);
                FileName += "_" + FileId;
            }
        }
        return FileName;
    }

    class ImageBackgroundHelper : PdfPageEventHelper
    {
        private iTextSharp.text.Image img;
        public ImageBackgroundHelper(iTextSharp.text.Image img)
        {
            this.img = img;
        }
        /**
         * @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
         *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
         */
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            writer.DirectContentUnder.AddImage(img);
        }
    }

    private string DownloadImage(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ImageSign\\" + DocumentPath);
        else
            ServerPath = ServerPath + "ImageSign\\" + DocumentPath;

        try
        {
            //HttpResponse response = Page.Response;
            //FileDownload.Download(response, ServerPath, DocumentPath);
            return ServerPath;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public string GetQuotePath(string FileName)
    {
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (ServerFilePath != "")
        {
            if (System.IO.File.Exists(ServerFilePath + ".pdf"))
            {
                string ext = ".pdf";
                string FileId = RandomString(5);
                ServerFilePath += "_" + FileId + ext;
            }
            else
                ServerFilePath = ServerFilePath + ".pdf";
        }

        return ServerFilePath;
    }

    protected string UploadFiles(string GetFileName)
    {
        string FileName = GetFileName;
        FileName = FileName.Replace(".", "");

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FileName);
        }
        else
            ServerFilePath = ServerFilePath + "Quotation\\" + FileName;

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = ".pdf";
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            //FU.SaveAs(ServerFilePath + FileName);

            return FileName;
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

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #endregion

    #region APPROVAL POPUP EVENTS

    protected void imgbtnApproval_Click(object sender, EventArgs e)
    {
        mpeApproval.Hide();
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        int QuotationId = Convert.ToInt32(Convert.ToString(Session["QuotationId"]));

        // 1 - Draft quotation , 2 - approval pending (sent to boss for approval) , 3 - approved , 4 - rejected
        int result = QuotationOperations.UpdateDraftApprovalStatus(QuotationId, 2, LoggedInUser.glUserId);
        if (result == 1)
        {
            bool SendMail = SendPreAlertEmail(QuotationId);
            if (SendMail == true)
            {
                lblError.Text = "Quotation has been forwarded to concerned authority for approval..!!";
                lblError.CssClass = "success";
                ResetControls();
            }
            else
            {
                lblError.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblError.CssClass = "errorMsg";
            }
        }
        else if (result == -1)
        {
            lblError.Text = "Quotation does not exists..!!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System error. Please try again later..!!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        int QuotationId = Convert.ToInt32(Convert.ToString(Session["QuotationId"]));
        int result = QuotationOperations.UpdateDraftApprovalStatus(QuotationId, 9, LoggedInUser.glUserId);
        Response.Redirect("EditQuotation.aspx");
    }

    #endregion

    #region TRANSPORTATION EVENTS

    protected void GetTransportChgs(int QuotationId)
    {
        if (QuotationId != 0)
        {
            DataSet ds = new DataSet();
            ds = QuotationOperations.GetTransportationCharges(QuotationId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                fsTransportCharges.Visible = true;
                gvTransportChg.DataSource = ds;
                gvTransportChg.DataBind();
            }
            else
            {
                fsTransportCharges.Visible = false;
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvTransportChg.DataSource = ds;
                gvTransportChg.DataBind();
                int columncount = gvTransportChg.Rows[0].Cells.Count;
                gvTransportChg.Rows[0].Cells.Clear();
                gvTransportChg.Rows[0].Cells.Add(new TableCell());
                gvTransportChg.Rows[0].Cells[0].ColumnSpan = columncount;
                gvTransportChg.Rows[0].Cells[0].Text = "No Records Found!";
            }
        }
    }

    protected void gvTransportChg_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Visible = true;
        if (e.CommandName.ToLower().Trim() == "insert")
        {
            TextBox txtParticulars_Footer = gvTransportChg.FooterRow.FindControl("txtParticulars_Footer") as TextBox;
            TextBox txtChargesApplicable_Footer = gvTransportChg.FooterRow.FindControl("txtChargesApplicable_Footer") as TextBox;

            int result = QuotationOperations.AddTransportationCharges(Convert.ToInt32(Session["QuotationId"]), txtParticulars_Footer.Text.Trim(),
                                                                txtChargesApplicable_Footer.Text.Trim(), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblError.Text = "Transportation Charges Added Successfully..!!";
                lblError.CssClass = "success";
                GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
            }
            else
            {
                lblError.Text = "Transportation Charges already exist!";
                lblError.CssClass = "warning";
            }
        }
    }

    protected void gvTransportChg_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTransportChg.EditIndex = e.NewEditIndex;
        GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
        lblError.Text = "";
    }

    protected void gvTransportChg_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTransportChg.EditIndex = -1;
        GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
        lblError.Text = "";
    }

    protected void gvTransportChg_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvTransportChg.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtParticulars = (TextBox)gvTransportChg.Rows[e.RowIndex].FindControl("txtParticulars");
        TextBox txtChargesApplicable = (TextBox)gvTransportChg.Rows[e.RowIndex].FindControl("txtChargesApplicable");

        int result = QuotationOperations.UpdateTransportationCharges(Lid, Convert.ToInt32(Session["QuotationId"]), txtParticulars.Text.Trim(), txtChargesApplicable.Text.Trim(),
                                                                    LoggedInUser.glUserId);
        if (result == 1)
        {
            lblError.Text = "Transportation Charges Updated Successfully..!!";
            lblError.CssClass = "success";
            gvTransportChg.EditIndex = -1;
            GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
        }
        else
        {
            lblError.Text = "Transportation Charges does not exist!";
            lblError.CssClass = "warning";
        }
    }

    protected void gvTransportChg_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblError.Visible = true;
        int lid = Convert.ToInt32(gvTransportChg.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteTransportationCharges(lid, LoggedInUser.glUserId);
        if (result == 1)
        {
            lblError.Text = "Transportation Charges Deleted Successfully!";
            lblError.CssClass = "success";
            GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
        }
        else
        {
            lblError.Text = "Transportation Charges does not exist!";
            lblError.CssClass = "warning";
        }
    }

    protected void gvTransportChg_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransportChg.PageIndex = e.NewPageIndex;
        GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
    }

    protected void gvTransportChg_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.BackColor = System.Drawing.Color.FromName("#1c698a");
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Center;
        }
    }

    #endregion

    #region Documnet Upload/Download/Delete

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Quotation\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Quotation\\" + FilePath;
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

            return FilePath + FileName;
        }
        else
        {
            return "";
        }

    }

    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  QuotationOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Quotation\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "Quotation\\" +DocumentPath;
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

    #region EMAIL EVENTS

    private bool SendPreAlertEmail(int QuotationId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", QuoteRefNo = "", CustomerName = "", QuotePath = "";

        DataSet dsGetQuotation = QuotationOperations.GetParticularQuotation(QuotationId);
        if (dsGetQuotation != null)
        {
            if (dsGetQuotation.Tables[0].Rows[0]["QuoteRefNo"].ToString() != "")
                QuoteRefNo = dsGetQuotation.Tables[0].Rows[0]["QuoteRefNo"].ToString();
            if (dsGetQuotation.Tables[0].Rows[0]["CustomerName"].ToString() != "")
                CustomerName = dsGetQuotation.Tables[0].Rows[0]["CustomerName"].ToString();
            if (dsGetQuotation.Tables[0].Rows[0]["QuotePath"].ToString() != "")
                QuotePath = dsGetQuotation.Tables[0].Rows[0]["QuotePath"].ToString();

            strCustomerEmail = "dhaval@babajishivram.com;kirti@babajishivram.com";
            strCCEmail = "javed.shaikh@babajishivram.com";
            strSubject = "Quotation (Ref No - " + QuoteRefNo + ") approval pending for Customer - " + CustomerName + " .";

            bool bEmailSucces = false;

            if (strCustomerEmail == "" || strSubject == "")
            {
                return bEmailSucces;
            }
            else
            {
                MessageBody = "Dear Sir,<BR><BR>Charges in some line items of mentioned quote are below the minimum expectation rates with quotation pdf file as attached. <BR><BR>";
                MessageBody = MessageBody +
                            "To approve/disapprove the quotation, please <u><b><a href='http://live.babajishivram.com/Quotation/ApproveQuote.aspx?id=" + Convert.ToString(QuotationId) + "'>click here</a></u></b>.";
                MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + LoggedInUser.glUserName;

                List<string> lstFilePath = new List<string>();

                DataSet dsGetAnnexureDoc = QuotationOperations.GetQuotationDocDetail(QuotationId);
                if (dsGetAnnexureDoc != null)
                {
                    if (dsGetAnnexureDoc.Tables[0].Rows.Count > 0)
                    {
                        for (int a = 0; a < dsGetAnnexureDoc.Tables[0].Rows.Count; a++)
                        {
                            lstFilePath.Add("Quotation\\" + dsGetAnnexureDoc.Tables[0].Rows[a]["DocPath"].ToString());
                        }
                    }
                    lstFilePath.Add("Quotation\\" + QuotePath);
                }

                //bEmailSucces = EMail.SendMail(LoggedInUser.glUserName, strCustomerEmail, strSubject, MessageBody, QuotePath);
                bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                return bEmailSucces;
            }
        }
        else
            return false;

    }

    #endregion
}
