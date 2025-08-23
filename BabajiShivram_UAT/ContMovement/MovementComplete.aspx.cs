using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using System.Web.UI;
//using iTextSharp.too.xml;

public partial class ContMovement_MovementComplete : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        ScriptManager1.RegisterPostBackControl(btnLetterPDF);
        ScriptManager1.RegisterPostBackControl(gvDocuments);
        System.Web.UI.WebControls.Label lblTitle = (System.Web.UI.WebControls.Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Movement Complete";
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["JobId"]) != null)
            {
                GetMovementData(Convert.ToInt32(Session["JobId"]));
            }
        }
    }

    protected void GetMovementData(int JobId)
    {
        DataSet dsGetMovementDetail = CMOperations.GetMovementDetailByJobId(JobId);
        if (dsGetMovementDetail != null && dsGetMovementDetail.Tables[0].Rows.Count > 0)
        {
            if (dsGetMovementDetail.Tables[0].Rows[0]["JobId"].ToString() != "")
            {
                if (dsGetMovementDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    hdnJobId.Value = Convert.ToString(dsGetMovementDetail.Tables[0].Rows[0]["lid"]);
                    gvDocuments.DataBind();
                }
                lblCFSName.Text = dsGetMovementDetail.Tables[0].Rows[0]["CFSName"].ToString();
                if (dsGetMovementDetail.Tables[0].Rows[0]["NominatedCFSId"] != DBNull.Value)
                {
                    DBOperations.FillCFS(ddlCFSName, Convert.ToInt32(dsGetMovementDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString()));
                    ddlCFSName.SelectedValue = dsGetMovementDetail.Tables[0].Rows[0]["NominatedCFSId"].ToString();
                }
                if (dsGetMovementDetail.Tables[0].Rows[0]["ShippingLineDate"] != DBNull.Value)
                {
                    txtShippingLineDate.Text = Convert.ToDateTime(dsGetMovementDetail.Tables[0].Rows[0]["ShippingLineDate"]).ToString("dd/MM/yyyy");
                }
                if (dsGetMovementDetail.Tables[0].Rows[0]["ConfirmedByLineDate"] != DBNull.Value)
                {
                    txtConfirmedByLineDate.Text = Convert.ToDateTime(dsGetMovementDetail.Tables[0].Rows[0]["ConfirmedByLineDate"]).ToString("dd/MM/yyyy");
                }
                if (dsGetMovementDetail.Tables[0].Rows[0]["MovementCompDate"] != DBNull.Value)
                {
                    txtCompleteDate.Text = Convert.ToDateTime(dsGetMovementDetail.Tables[0].Rows[0]["MovementCompDate"]).ToString("dd/MM/yyyy");
                    mevEmptyContReturnDate.IsValidEmpty = false;
                }
                if (dsGetMovementDetail.Tables[0].Rows[0]["EmptyContReturnDate"] != DBNull.Value)
                {
                    txtEmptyContReturnDate.Text = Convert.ToDateTime(dsGetMovementDetail.Tables[0].Rows[0]["EmptyContReturnDate"]).ToString("dd/MM/yyyy");
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                }
                lblCFSName.Enabled = false;
            }
        }

        DataSet dsGetJobDetail = DBOperations.GetJobDetailByJobId(JobId);
        if (dsGetJobDetail != null && dsGetJobDetail.Tables[0].Rows.Count > 0)
        {
            txtEmptyContReturnDate.Visible = true;
            imgEmptyContReturnDate.Visible = true;
            mevEmptyContReturnDate.IsValidEmpty = true;
            btnSubmit.Visible = true;
            btnCancel.Visible = true;
            if (dsGetJobDetail.Tables[0].Rows[0]["JobId"].ToString() != "")
            {
                if (Convert.ToInt32(dsGetJobDetail.Tables[0].Rows[0]["DeliveryTypeID"].ToString()) != 0)
                {
                    lblDeliveryType.Text = dsGetJobDetail.Tables[0].Rows[0]["DeliveryType"].ToString();
                    int DeliveryType = Convert.ToInt32(dsGetJobDetail.Tables[0].Rows[0]["DeliveryTypeID"].ToString());
                    if (DeliveryType == 2) // de-stuff delivery type
                    {
                        //mevEmptyContReturnDate.IsValidEmpty = false;
                    }
                    else
                    {
                        txtEmptyContReturnDate.Enabled = false;
                        imgEmptyContReturnDate.Visible = false;
                    }
                }
                else
                {
                    lblFlashError.Text = "Please update delivery type for this job to process ahead!";
                    lblFlashError.CssClass = "errorMsg";
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                }
                lblJobRefNo.Text = dsGetJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
                lblCustName.Text = dsGetJobDetail.Tables[0].Rows[0]["Customer"].ToString();
                lblConsigneeName.Text = dsGetJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
                if (dsGetJobDetail.Tables[0].Rows[0]["ETA"] != DBNull.Value)
                {
                    lblETADate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["ETA"]).ToString("dd/MM/yyyy");
                }
                lblBranch.Text = dsGetJobDetail.Tables[0].Rows[0]["BabajiBranch"].ToString();
                if (dsGetJobDetail.Tables[0].Rows[0]["Count20"] != DBNull.Value)
                {
                    lblSumof20.Text = dsGetJobDetail.Tables[0].Rows[0]["Count20"].ToString();
                }
                else
                {
                    lblSumof20.Text = "0";
                }
                if (dsGetJobDetail.Tables[0].Rows[0]["Count40"] != DBNull.Value)
                {
                    lblSumof40.Text = dsGetJobDetail.Tables[0].Rows[0]["Count40"].ToString();
                }
                else
                {
                    lblSumof40.Text = "0";
                }
                if (dsGetJobDetail.Tables[0].Rows[0]["CountLCL"].ToString() != "")
                {
                    if (dsGetJobDetail.Tables[0].Rows[0]["CountLCL"].ToString() == "0")
                    {
                        lblContType.Text = "FCL";
                    }
                    else
                    {
                        lblContType.Text = "LCL";
                    }
                }
                lblJobCreationDate.Text = Convert.ToDateTime(dsGetJobDetail.Tables[0].Rows[0]["JobDate"]).ToString("dd/MM/yyyy");
                lblJobCreatedBy.Text = dsGetJobDetail.Tables[0].Rows[0]["JobCreatedBy"].ToString();
                lblShippingName.Text = dsGetJobDetail.Tables[0].Rows[0]["ShippingName"].ToString();
                hdnShipperId.Value = dsGetJobDetail.Tables[0].Rows[0]["ShippingId"].ToString();
                gvDocuments.DataBind();
                DBOperations.FillCFS(ddlCFSName, Convert.ToInt32(dsGetJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString()));

                //if (hdnShipperId.Value != "" && hdnShipperId.Value != "0")
                //{
                //    //PrintLetter(Convert.ToInt32(hdnShipperId.Value), Convert.ToInt32(ddlLetters.SelectedValue));
                //    GetShippingData(Convert.ToInt32(hdnShipperId.Value));
                //    ddlLetters.DataBind();
                //}
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MovementProcess.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int MovementId = 0, NominatedCFSId = 0, count = 0;
        string JobRefNo = "";
        DateTime dtEmptyContReturnDate = DateTime.MinValue, dtMovementComplete = DateTime.MinValue, dtShippingLine = DateTime.MinValue, dtConfirmedByLine = DateTime.MinValue;

        if (Convert.ToString(Session["JobId"]) != "")
        {
            if (hdnJobId.Value != "" && hdnJobId.Value != "0")
                MovementId = Convert.ToInt32(hdnJobId.Value);
            if (ddlCFSName.SelectedValue != "0")
                NominatedCFSId = Convert.ToInt32(ddlCFSName.SelectedValue);

            if (txtShippingLineDate.Text.Trim() != "")
            {
                dtShippingLine = Commonfunctions.CDateTime(txtShippingLineDate.Text.Trim());
                count = 1;
            }
            if (txtConfirmedByLineDate.Text.Trim() != "")
            {
                dtConfirmedByLine = Commonfunctions.CDateTime(txtConfirmedByLineDate.Text.Trim());
                count = 1;
            }

            if (txtCompleteDate.Text.Trim() != "")
            {
                dtMovementComplete = Commonfunctions.CDateTime(txtCompleteDate.Text.Trim());
                count = 1;
            }

            if (txtEmptyContReturnDate.Text.Trim() != "")
            {
                dtEmptyContReturnDate = Commonfunctions.CDateTime(txtEmptyContReturnDate.Text.Trim());

                // document required mandatory
                DataSet dsGetDocs = CMOperations.GetDocuments(Convert.ToInt32(Session["JobId"]));
                if (gvDocuments != null && gvDocuments.Rows.Count > 0)
                {
                    count = 1;
                }
                else
                    count = 0;
            }

            if (count == 1)
            {
                int result = CMOperations.AddMovementDetail(Convert.ToInt32(Session["JobId"]), dtEmptyContReturnDate, dtMovementComplete, dtShippingLine,
                                                                dtConfirmedByLine, NominatedCFSId, loggedInUser.glUserId);
                if (result == 0)
                {
                    /////////// Update Nominated CFS Name in IGM /////////////////
                    if (NominatedCFSId > 0)
                    {
                        int result_CFSId = CMOperations.UpdateNominatedCFSName(Convert.ToInt32(Session["JobId"]), NominatedCFSId);
                    }

                    lblError.Text = "Successfully added detail.";
                    lblError.CssClass = "success";
                    gvDocuments.DataBind();
                }
                else if (result == 2)
                {
                    lblError.Text = "Document already exists.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while adding up document.";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Atleast 1 document required for processing job!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void lnkbtnCoscoLetter1_Click(object sender, EventArgs e)
    {
    }

    protected void btnLetterPDF_Click(object sender, EventArgs e)
    {
        int ShippingId = 31; // MSC
        //PrintLetter(ShippingId, Convert.ToInt32(ddlLetters.SelectedValue));
    }

    private void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int colIndex = 0; colIndex < e.Row.Cells.Count; colIndex++)
                {
                    int rowIndex = colIndex;
                    System.Web.UI.WebControls.TextBox txtName = new System.Web.UI.WebControls.TextBox();
                    txtName.Width = 16;
                    txtName.ID = "txtBox" + colIndex;
                    txtName.AutoPostBack = true;
                    e.Row.Cells[colIndex].Controls.Add(txtName);
                }
            }
        }
        catch (Exception en)
        { }
    }

    private void AddAdditionalField(int FieldId, FieldDataType FieldType, string FieldName)
    {
        TableRow tr = new TableRow();

        // Field Name
        TableCell tdName = new TableCell();
        tdName.Text = FieldName;
        tdName.VerticalAlign = VerticalAlign.Top;
        tr.Cells.Add(tdName);

        // Field Type

        List<System.Web.UI.Control> UIControl = CreateCustomAttributeUI(FieldId, FieldType);

        TableCell tdUI = new TableCell();
        tdUI.VerticalAlign = VerticalAlign.Top;
        tdUI.ColumnSpan = 3;
        foreach (System.Web.UI.Control ctrl in UIControl)
        {
            tdUI.Controls.Add(ctrl);
        }

        tr.Cells.Add(tdUI);
        CustomUITable.Rows.Add(tr);
    }

    private List<System.Web.UI.Control> CreateCustomAttributeUI(int FieldId, FieldDataType DataTypeId)
    {
        List<System.Web.UI.Control> Ctrls = new List<System.Web.UI.Control>();
        switch (DataTypeId)
        {
            case FieldDataType.Alphanumeric:
                // use a  Text Box
                System.Web.UI.WebControls.TextBox tbBox = new System.Web.UI.WebControls.TextBox();
                tbBox.ID = FieldId.ToString(); //GetId(FieldId);
                tbBox.MaxLength = 200;
                Ctrls.Add(tbBox);
                break;
            case FieldDataType.Date:
                // user Text Box with validation
                System.Web.UI.WebControls.TextBox tbBoxDate = new System.Web.UI.WebControls.TextBox();
                tbBoxDate.ID = FieldId.ToString(); //GetId(FieldId);
                Ctrls.Add(tbBoxDate);
                // Add Ajax Date Extender

                AjaxControlToolkit.CalendarExtender calDate = new AjaxControlToolkit.CalendarExtender();
                calDate.ID = "Cal" + FieldId.ToString();
                calDate.TargetControlID = FieldId.ToString();
                calDate.Format = "dd/MM/yyyy";
                //  calDate.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                Ctrls.Add(calDate);
                Ctrls.Add(CreateDataTypeCheckCompareValidator(FieldId.ToString(), ValidationDataType.Date, "Invalid date value."));
                break;
        }
        return Ctrls;
    }

    private CompareValidator CreateDataTypeCheckCompareValidator(string DynamicFieldid, ValidationDataType DataType, string ErrorMessage)
    {
        CompareValidator cv = new CompareValidator();
        cv.ID = ("CompVal_" + DynamicFieldid);
        cv.ControlToValidate = DynamicFieldid;
        cv.Display = ValidatorDisplay.Dynamic;
        cv.Operator = ValidationCompareOperator.DataTypeCheck;
        cv.Type = DataType;
        cv.ErrorMessage = ErrorMessage;
        return cv;
    }

    #region ADD NEW ROW TO GRID VIEW 

    protected void AddNewRowToGrid(GridView gvTable, int TableId)
    {
        int rowIndex = 0;
        string strViewState = "table" + gvTable.ID;
        if (ViewState[strViewState] != null)
        {
            DataTable dt = (DataTable)ViewState[strViewState];
            DataRow dr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //System.Web.UI.WebControls.TextBox txtBox = (System.Web.UI.WebControls.TextBox)gvTable.Rows[rowIndex].Cells[5].FindControl("txtChargesApp");

                }
            }
        }
        else
        {
            DataTable dt = new DataTable();
            // Add Columns
            SqlDataReader drColumns = CMOperations.GetLetterTablesHeaderByFieldId(TableId);
            if (drColumns.HasRows)
            {
                while (drColumns.Read())
                {
                    dt.Columns.Add(new DataColumn(drColumns["HeaderTitle"].ToString(), typeof(string)));
                }
            }

            DataRow dr = null;
            dr = dt.NewRow();

            // Add default values to columns
            SqlDataReader drColumnValue = CMOperations.GetLetterTablesHeaderByFieldId(TableId);
            if (drColumnValue.HasRows)
            {
                while (drColumnValue.Read())
                {
                    dr[drColumnValue["HeaderTitle"].ToString()] = "";
                }
            }

            dt.Rows.Add(dr);
            gvTable.DataSource = dt;
            gvTable.DataBind();
        }
        //SetPreviousData(gvTable);
    }

    protected void SetPreviousData(GridView gvTable)
    {
        int rowIndex = 0;
        string strViewState = "table" + gvTable.ID;
        if (ViewState[strViewState] != null)
        {
            DataTable dt = (DataTable)ViewState[strViewState];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++) // 0,1,2
                {
                    for (int j = 0; j < dt.Columns.Count; j++) // 0,1,2
                    {
                        System.Web.UI.WebControls.TextBox txtBox = new System.Web.UI.WebControls.TextBox();
                        //(System.Web.UI.WebControls.TextBox)gvTable.Rows[i].Cells[j].FindControl("txtBox" + j.ToString());
                        txtBox.Width = 16;
                        txtBox.ID = "txtBox" + j.ToString();
                        txtBox.AutoPostBack = true;
                        //e.Row.Cells[colIndex].Controls.Add(txtName);

                        if (i < dt.Rows.Count - 1)
                        {
                            //Assign the value from DataTable to the TextBox   
                            txtBox.Text = dt.Rows[i]["Column1"].ToString();
                        }
                    }
                    rowIndex++;
                }
            }
        }
    }

    #endregion

    #region Documents

    protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
        else if (e.CommandName.ToLower() == "deletedoc")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = CMOperations.DeleteDocument(lid, loggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted document.";
                    lblError.CssClass = "success";
                    gvDocuments.DataBind();
                }
                else if (result == 2)
                {
                    lblError.Text = "Document does not exists.";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error while adding up document.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int JobId = 0;
        if (fuUploadFile.HasFile)
        {
            DataSet dsGetMovementDetail = CMOperations.GetMovementDetailByJobId(Convert.ToInt32(Session["JobId"]));
            if (dsGetMovementDetail != null && dsGetMovementDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetMovementDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    JobId = Convert.ToInt32(dsGetMovementDetail.Tables[0].Rows[0]["lid"].ToString());
                }
            }

            string FilePath = UploadFiles(fuUploadFile, "");
            int result = CMOperations.AddDocument(JobId, FilePath, loggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added document.";
                lblError.CssClass = "success";
                gvDocuments.DataBind();
            }
            else if (result == 2)
            {
                lblError.Text = "Document already exists.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up document.";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please upload document.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        //DocumentPath =  QuotationOperations.GetDocumentPath(Convert.ToInt32(DocumentId));
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\PNMovement\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "PNMovement\\" + DocumentPath;
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

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\PNMovement\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "PNMovement\\" + FilePath;
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

    #endregion

    //protected void PrintLetter(int ShippingId, int LetterId)
    //{
    //    if (LetterId > 0)
    //    {
    //        string TemplatePath = "";
    //        DataSet dsGetTemplatePath = CMOperations.GetLetterTemplatePath(LetterId);
    //        if (dsGetTemplatePath != null && dsGetTemplatePath.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsGetTemplatePath.Tables[0].Rows[0]["TemplatePath"].ToString() != "")
    //            {
    //                TemplatePath = dsGetTemplatePath.Tables[0].Rows[0]["TemplatePath"].ToString();
    //            }
    //        }

    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", "attachment;filename=Cosco.pdf");
    //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //        StringWriter sw = new StringWriter();

    //        HtmlTextWriter hw = new HtmlTextWriter(sw);
    //        StringReader sr = new StringReader(sw.ToString());

    //        Rectangle recPDF = new Rectangle(PageSize.A4);

    //        // 36 point = 0.5 Inch, 72 Point = 1 Inch, 108 Point = 1.5 Inch, 180 Point = 2.5 Inch
    //        // Set PDF Document size and Left,Right,Top and Bottom margin

    //        //Document pdfDoc = new Docu ment(recPDF);

    //        Document pdfDoc = new Document(recPDF, 30, 10, 10, 80);
    //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //        PdfWriter pdfwriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

    //        pdfDoc.Open();
    //        string contents = "";
    //        contents = File.ReadAllText(Server.MapPath(TemplatePath));

    //        var parsedContent = HTMLWorker.ParseToList(new StringReader(contents), null);
    //        foreach (var htmlelement in parsedContent)
    //            pdfDoc.Add(htmlelement as IElement);

    //        PdfPTable pdftable = new PdfPTable(6);

    //        pdftable.TotalWidth = 520f;
    //        pdftable.LockedWidth = true;
    //        float[] widths = new float[] { 0.07f, 0.3f, 0.3f, 0.3f, 0.3f, 0.3f };
    //        pdftable.SetWidths(widths);
    //        pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

    //        // Set Table Spacing Before And After html text
    //        //   pdftable.SpacingBefore = 10f;
    //        pdftable.SpacingAfter = 8f;

    //        #region Get Shipping Field Details By Shipping Id
    //        #region Create Table Column Header Cell with Text

    //        DataSet dsGetLetterFields = CMOperations.GetLetterFields_ShippingId(ShippingId);
    //        if (dsGetLetterFields != null)
    //        {
    //            //PdfPCell cellwithdata = new PdfPCell(new Phrase(, GridHeadingFont));
    //            //cellwithdata.Colspan = 1;
    //            //cellwithdata.BorderWidth = 1f;
    //            //cellwithdata.HorizontalAlignment = 0;   //left
    //            //cellwithdata.VerticalAlignment = 0;     // Center
    //            //cellwithdata.Padding = 5;
    //            //cellwithdata.BackgroundColor = new iTextSharp.text.BaseColor(220, 240, 240);
    //            //pdftable.AddCell(cellwithdata);


    //        }
    //        #endregion
    //        #endregion

    //        pdfDoc.Add(pdftable);
    //        htmlparser.Parse(sr);
    //        pdfDoc.Close();
    //        Response.Write(pdfDoc);
    //        HttpContext.Current.ApplicationInstance.CompleteRequest();
    //    }
    //}

    //protected void ddlLetters_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //PrintLetter(Convert.ToInt32(hdnShipperId.Value), Convert.ToInt32(ddlLetters.SelectedValue));
    //    if (ddlLetters.SelectedValue != "0")
    //    {
    //        int LetterId = 0;
    //        LetterId = Convert.ToInt32(ddlLetters.SelectedValue);
    //        DataSet dsGetLetterFields = CMOperations.GetLetterFields_ShippingId(LetterId);
    //        if (dsGetLetterFields != null)
    //        {
    //            #region Add Fields For Letters
    //            SqlDataReader drFields = CMOperations.GetLetterFieldsByLetterId(LetterId);
    //            if (drFields.HasRows)
    //            {
    //                while (drFields.Read())
    //                {
    //                    int FieldId = Convert.ToInt32(drFields["lid"]);
    //                    string FieldName = drFields["FieldName"].ToString();
    //                    FieldDataType FieldType = (FieldDataType)drFields["lType"];

    //                    AddAdditionalField(FieldId, FieldType, FieldName);
    //                }
    //            }
    //            #endregion

    //            #region Add Tables For Letters
    //            SqlDataReader drTables = CMOperations.GetLetterTablesByLetterId(LetterId);
    //            if (drTables.HasRows)
    //            {
    //                while (drTables.Read())
    //                {
    //                    GridView gvTable = new GridView();
    //                    gvTable.ID = drTables["lid"].ToString();
    //                    gvTable.AutoGenerateColumns = false;
    //                    gvTable.Width = Unit.Percentage(100);
    //                    gvTable.CssClass = "table";

    //                    if (drTables["lid"].ToString() != "")
    //                    {
    //                        int TableId = Convert.ToInt32(drTables["lid"].ToString());
    //                        AddNewRowToGrid(gvTable, TableId);
    //                    }
    //                    if (gvTable.Rows.Count > 0)
    //                    {

    //                    }
    //                    gvTable.RowDataBound += new GridViewRowEventHandler(gvTable_RowDataBound);
    //                    pnlGrids.Controls.Add(gvTable);
    //                    pnlGrids.Visible = true;
    //                }
    //            }
    //            #endregion
    //        }
    //    }
    //}

}