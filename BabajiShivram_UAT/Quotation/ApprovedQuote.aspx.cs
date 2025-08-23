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

public partial class Quotation_ApprovedQuote : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvGenerateCharge);
        ScriptManager1.RegisterPostBackControl(gvRFQDocuments);
        ScriptManager1.RegisterPostBackControl(btnSaveDoc);
        ScriptManager1.RegisterPostBackControl(btnAddContractCopy);
        ScriptManager1.RegisterPostBackControl(gvContractCopy);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Draft Quotation";
            lblError.Visible = false;

            if (Session["QuotationId"].ToString() != null && Convert.ToString(Session["QuotationId"]) != "")
                GetQuotationDetails(Convert.ToInt32(Session["QuotationId"]));
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
                }
                else
                    chkLumpSum.Checked = false;

                // Quotation Format
                Boolean IsNormalQuote = true;
                if (dsGetQuoteDetails.Tables[0].Rows[0]["IsTenderQuote"].ToString().ToLower().Trim() == "true")
                    IsNormalQuote = false;

                if (IsNormalQuote == true)
                {
                    lblQuoteRefNo.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                    dvBabajiQuote.Visible = true;
                    dvCustomerQuote.Visible = false;

                    lblSalesPerson.Text = dsGetQuoteDetails.Tables[0].Rows[0]["SalesPersonName"].ToString();            // Sales Person Name
                    lblKAM.Text = dsGetQuoteDetails.Tables[0].Rows[0]["KAMPerson"].ToString();                     // KAMPerson
                    lblCustomer.Text = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"].ToString().ToUpper().Trim();           // Customer
                    lblAddressLine1.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine1"].ToString();              // Address Line 1
                    lblAddressLine2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine2"].ToString();              // Address Line 2
                    lblAddressLine3.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AddressLine3"].ToString();              // Address Line 3
                    lblKindAttn.Text = dsGetQuoteDetails.Tables[0].Rows[0]["AttendedPerson"].ToString();                // Kind Attention Name
                    lblSubject.Text = dsGetQuoteDetails.Tables[0].Rows[0]["Subject"].ToString();                        // Subject
                    lblTerms.Text = dsGetQuoteDetails.Tables[0].Rows[0]["PaymentTerms"].ToString();                     // Payment Terms                 
                    lblQuoteGeneratedFor.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteGeneratedFor"].ToString();    // QUOTE GENERATED FOR               
                    lblMode.Text = dsGetQuoteDetails.Tables[0].Rows[0]["Mode"].ToString();                              // Mode

                    // Charges Applicable Table
                    DataSet dsGetChargesApp = QuotationOperations.GetQuoteReportData(QuotationId);
                    if (dsGetChargesApp != null)
                    {
                        gvGenerateCharge.DataSource = dsGetChargesApp;
                        gvGenerateCharge.DataBind();
                    }

                    // Transportation charges
                    GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
                    lblDivision.Text = dsGetQuoteDetails.Tables[0].Rows[0]["ServiceName"].ToString();
                    lblBranch.Text = dsGetQuoteDetails.Tables[0].Rows[0]["BranchName"].ToString();
                }
                else
                {
                    lblsalesRep_Cust.Text = dsGetQuoteDetails.Tables[0].Rows[0]["SalesPersonName"].ToString();            // Sales Person Name
                    lblKAM_cust.Text = dsGetQuoteDetails.Tables[0].Rows[0]["KAMPerson"].ToString();                     // KAMPerson
                    lblDivision2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["ServiceName"].ToString();
                    lblBranch2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["BranchName"].ToString();
                    lblQuoteRefNo2.Text = dsGetQuoteDetails.Tables[0].Rows[0]["QuoteRefNo"].ToString();
                    dvBabajiQuote.Visible = false;
                    dvCustomerQuote.Visible = true;

                    lblTenderCustomer.Text = dsGetQuoteDetails.Tables[0].Rows[0]["CustomerName"].ToString();
                    lblNotes.Text = dsGetQuoteDetails.Tables[0].Rows[0]["OtherNotes"].ToString();
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


    #region STATUS HISTORY EVENTS

    protected void gvStatusHistory_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            Boolean IsActive = Convert.ToBoolean(rowView["IsActive"].ToString());
            if (IsActive == true)
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen;
                e.Row.ToolTip = "Current active status for quotation.";
            }
        }
    }

    #endregion

    #region GRID VIEW EVENTS

    protected void gvGenerateCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double number;
        if (chkLumpSum.Checked == true)
        {
            gvGenerateCharge.Columns[4].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal MinAmt = 0;
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                Boolean IsLumpSumField = Convert.ToBoolean(rowView["IsLumpSumField"].ToString());

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

                int ChargeId = Convert.ToInt32(rowView["ChargeId"].ToString());
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
                    ddlRanges.Width = 250;
                }
                else
                {
                    ddlRanges.Visible = false;
                    txtLumpsumAmount.Width = 350;
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
                    txtLumpsumAmount.Enabled = false;
                    ddlRanges.Enabled = false;
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                int ChargeId = Convert.ToInt32(rowView["ChargeId"].ToString());
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
                    ddlRanges.Width = 250;
                }
                else
                {
                    ddlRanges.Visible = false;
                    txtChargesApp.Width = 350;
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
                        ddlRanges.Enabled = false;
                        txtChargesApp.Enabled = false;
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

    protected void gvTransportChg_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransportChg.PageIndex = e.NewPageIndex;
        GetTransportChgs(Convert.ToInt32(Session["QuotationId"]));
    }

    protected void gvTransportChg_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Center;
        }
    }

    #endregion

    #region CONTRACT COPY EVENTS

    protected void btnShowContractCopy_OnClick(object sender, EventArgs e)
    {
        mpeContractCopy.Show();
        gvContractCopy.DataBind();
        DataSet dsGetQuoteDetails = QuotationOperations.GetParticularQuotation(Convert.ToInt32(Session["QuotationId"]));
        if (dsGetQuoteDetails != null)
        {
            if (dsGetQuoteDetails.Tables[0].Rows[0]["ContractEndDt"] != DBNull.Value)
            {
                txtContractEndDt.Text = Convert.ToDateTime(dsGetQuoteDetails.Tables[0].Rows[0]["ContractEndDt"]).ToShortDateString();
                txtContractEndDt.Enabled = false;
                imgEndDt.Visible = false;
            }

            if (dsGetQuoteDetails.Tables[0].Rows[0]["ContractStartDt"] != DBNull.Value)
            {
                txtContractStartDt.Text = Convert.ToDateTime(dsGetQuoteDetails.Tables[0].Rows[0]["ContractStartDt"]).ToShortDateString();
                txtContractStartDt.Enabled = false;
                imgStartDt.Visible = false;
            }
        }
    }

    protected void imgbtnContract_Click(object sender, EventArgs e)
    {
        mpeContractCopy.Hide();
    }

    protected void btnAddContractCopy_OnClick(object sender, EventArgs e)
    {
        string fileName = "";
        int result = 0;
        if (fuUploadContractCopy != null && fuUploadContractCopy.HasFile)
            fileName = UploadFiles(fuUploadContractCopy, "");

        if (fileName != "")
        {
            result = QuotationOperations.AddQuotationAnnexure(Convert.ToInt32(Session["QuotationId"]), fileName, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
            if (result == 1)
            {
                lblErrorContract.Text = "Document added successfully!";
                lblErrorContract.CssClass = "success";
                gvContractCopy.DataBind();
                mpeContractCopy.Show();
            }
            else
            {
                lblErrorContract.Text = "System Error! Please Try After Sometime.";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
        }

        if (txtContractStartDt.Text.Trim() != "")
        {
            DateTime dtContractStart = DateTime.MinValue;
            DateTime dtContractEnd = DateTime.MinValue;
            if (txtContractStartDt.Text.Trim() != "")
                dtContractStart = Commonfunctions.CDateTime(txtContractStartDt.Text.Trim());
            if (txtContractEndDt.Text.Trim() != "")
                dtContractEnd = Commonfunctions.CDateTime(txtContractEndDt.Text.Trim());

            int SaveUpdateDates = QuotationOperations.UpdateQuotationContractDates(Convert.ToInt32(Session["QuotationId"]), dtContractStart, dtContractEnd, fileName, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
            if (SaveUpdateDates == 2)
            {
                lblErrorContract.Text = "Document added successfully!";
                lblErrorContract.CssClass = "success";
                gvContractCopy.DataBind();
                mpeContractCopy.Show();
            }
            else if (SaveUpdateDates == 3)
            {
                lblErrorContract.Text = "Quotation does not exists..!!";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
            else
            {
                lblErrorContract.Text = "System error. Please try again later..!!";
                lblErrorContract.CssClass = "errorMsg";
                mpeContractCopy.Show();
            }
        }
    }

    protected void gvContractCopy_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            mpeContractCopy.Show();
        }
        else if (e.CommandName.ToLower() == "deletedoc")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int result = QuotationOperations.DeleteAnnexureDocs(lid, Convert.ToInt32(LoggedInUser.glUserId.ToString()));
                if (result == 1)
                {
                    lblErrorContract.Text = "Successfully deleted document!";
                    lblErrorContract.CssClass = "success";
                    gvContractCopy.DataBind();
                    mpeContractCopy.Show();
                }
                else
                {
                    lblErrorContract.Text = "System Error! Please Try After Sometime.";
                    lblErrorContract.CssClass = "errorMsg";
                    mpeContractCopy.Show();
                }
            }
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

    #endregion
}