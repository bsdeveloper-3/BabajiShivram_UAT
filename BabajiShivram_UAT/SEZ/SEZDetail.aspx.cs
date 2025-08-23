using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data.OleDb;
using System.Linq;
using System.Collections.Generic;

public partial class SEZ_SEZDetail : System.Web.UI.Page
{
    public Dictionary<long, string> ExcelErrorList = new Dictionary<long, string>();

    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    static int CustomerId = 0;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager1.RegisterPostBackControl(grdDocument);

        if (!IsPostBack)
        {
            string script = "$(document).ready(function () { $('[id*=btnUploadDSR]').click(); });";
            ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);

            SetInitialRow();          // For Invoice Detail
            SetInitialCantainerRow(); // For Container Details
            ImportDSRExcel.Visible = false;
            filldsetbasic.Visible = false;
            filldsetInvoice.Visible = false;
            filldsetUploadDoc.Visible = false;
            filldsetDownloadDoc.Visible = false;

            DocumentTypeBind();
            ddDocument.Items.Insert(0, new ListItem("--Select Type--", "0"));

            //if(Double.TryParse(txtJobRefNo.text),out ABC)
            //{
            //    abc = 0;
            //}
            //else
            //{
            //    abc = 1; // Error
            //}

            DataTable dtAnnexureDoc2 = new DataTable();
            dtAnnexureDoc2.Columns.AddRange(new DataColumn[6] { new DataColumn("PkId"), new DataColumn("DocType"), new DataColumn("DocTypeName"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
            ViewState["AnnexureDoc3"] = dtAnnexureDoc2;
            ViewState["AnnexureDoc4"] = dtAnnexureDoc2;
        }

        if (ViewState["ExcelError"] != null)
        {
            ExcelErrorList = (Dictionary<long, string>)ViewState["ExcelError"];
        }
    }

    public void InwardClear()
    {
        txtInwardBENo.Text = "";
        txtInwardJobNo.Text = "";
        txtInwardBEDate.Text = "";

        lblInwardBENo.Visible = false;
        txtInwardBENo.Visible = false;
        lblInJobNo.Visible = false;
        txtInwardJobNo.Visible = false;

        lblInwardBEDate.Visible = false;
        txtInwardBEDate.Visible = false;
        imgInwardBEDate.Visible = false;
    }

    public void OutwardClear()
    {
        lblInwardBENo.Visible = true;
        txtInwardBENo.Visible = true;

        lblInJobNo.Visible = true;
        txtInwardJobNo.Visible = true;
        lblInwardBEDate.Visible = true;
        txtInwardBEDate.Visible = true;
        imgInwardBEDate.Visible = true;
    }

    protected void rdbSEZtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        filldsetbasic.Visible = true;
        filldsetInvoice.Visible = true;
        filldsetUploadDoc.Visible = true;
        filldsetDownloadDoc.Visible = true;

        ImportDSRExcel.Visible = true;
        btnSubmit.Visible = true;
        btnNew.Visible = false;

        ViewState["AnnexureDoc2"] = null;
        DataTable dtAnnexureDoc3 = new DataTable();
        dtAnnexureDoc3.Columns.AddRange(new DataColumn[6] { new DataColumn("PkId"), new DataColumn("DocType"), new DataColumn("DocTypeName"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
        ViewState["AnnexureDoc2"] = dtAnnexureDoc3;

        if (rdbSEZtype.SelectedValue == "1")
        {
            int i = 1;
            string Result = SEZOperation.GetGenerateSEZJobNo(i);
            if (Result != "")
            {
                txtJobNo.Text = Result;

                CustomerId = 0;
            }

            lblTitle.Text = "SEZ Detail For Inward";
            InwardClear();
            //txtOutwardDate.Visible = false;
            //imgOutwardDate.Visible = false;

            GrdInvoiceDetail.Columns[7].Visible = false;
            trOutwardBE.Visible = false;
            ClearAll();
        }
        else if (rdbSEZtype.SelectedValue == "2")
        {
            int j = 2;
            string Result = SEZOperation.GetGenerateSEZJobNo(j);
            if (Result != "")
            {
                txtJobNo.Text = Result;
                CustomerId = 0;
            }
            GrdInvoiceDetail.Columns[7].Visible = false;
            //GrdInvoiceDetail.Columns[7].Visible = true;
            //GrdInvoiceDetail.Columns[10].

            lblTitle.Text = "SEZ Detail For Outward";
            OutwardClear();
            txtOutwardDate.Visible = true;
            imgOutwardDate.Visible = true;

            trOutwardBE.Visible = true;
            ClearAll();
        }
    }

    public void DocumentTypeBind()
    {
        DataSet dsDocType = SEZOperation.GetJobDocumentMS();

        if (dsDocType.Tables[0].Rows.Count > 0)
        {
            ddDocument.DataSource = dsDocType;
            ddDocument.DataBind();

            ddDocument.DataSource = dsDocType.Tables[0];
            ddDocument.DataTextField = "DocumentName";
            ddDocument.DataValueField = "lid";
            ddDocument.DataBind();
            //JobID = Convert.ToInt32(dsDocType.Tables[0].Rows[0]["lid"]);
        }
    }

    protected void grdDocument_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string item = e.Row.Cells[4].Text;
            foreach (Button button in e.Row.Cells[7].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                }
            }
        }
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        DataTable dt = ViewState["AnnexureDoc2"] as DataTable;
        dt.Rows[index].Delete();
        ViewState["dt"] = dt;
        BindGrid();
    }

    protected void txtClientName_TextChanged(object sender, EventArgs e)
    {
        if (hdnCustId.Value != "")
        {
            CustomerId = Convert.ToInt32(hdnCustId.Value);

            if (CustomerId > 0)
            {
                if (txtClientName.Text != "")
                {
                    CustomerId = Convert.ToInt32(hdnCustId.Value);
                }
                else
                {
                    CustomerId = 0;
                }

                if (CustomerId > 0)
                {
                    DBOperations.FillCustomerDivision(ddlDivisionInd, CustomerId);
                    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                    ddlPlantInd.Items.Clear();
                    ddlPlantInd.Items.Add(lstSelect);
                }
                else
                {
                    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                    ddlDivisionInd.Items.Clear();
                    ddlDivisionInd.Items.Add(lstSelect);

                    System.Web.UI.WebControls.ListItem lstSelect1 = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                    ddlPlantInd.Items.Clear();
                    ddlPlantInd.Items.Add(lstSelect1);
                }
            }
        }
        else
        {
            lblError.Text = "Please Select the Customer";
            lblError.CssClass = "errorMsg";
        }
    }


    protected void ddlDivisionInd_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddlDivisionInd.SelectedValue);

        if (DivisonId > 0)
        {
            DBOperations.FillCustomerPlant(ddlPlantInd, DivisonId);
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddlPlantInd.Items.Clear();
            ddlPlantInd.Items.Add(lstSelect);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //try {
        lblError.Text = "";
        if (txtJobNo.Text != "")
        {
            int DaysStore = 0, NoOfPackages = 0, NoOfVehicles = 0, jobid = Convert.ToInt32(Session["JobId"]), Division = 0, Plant = 0;
            decimal ExRate = 0, AssesableValue = 0, DutyAmount = 0, GrossWeight = 0, CIFValue = 0;
            bool Discount = false, ReImport = false, PrevImport = false, PrevExpGoods = false, CessDetail = false,
                    LicenceRegNo = false, ReExport = false, PrevExport = false;

            DateTime InwardBEDate = DateTime.MinValue,
                BEDate = DateTime.MinValue, InwardDate = DateTime.MinValue, OutwardDate = DateTime.MinValue,
                PCDFrDahej = DateTime.MinValue, PCDSentClient = DateTime.MinValue, FileSentToBilling = DateTime.MinValue;

            string JobRefNo = txtJobNo.Text.Trim();
            int SEZType = Convert.ToInt32(rdbSEZtype.SelectedValue);
            int SEZMode = Convert.ToInt32(ddlMode.SelectedValue);
            int RequestType = Convert.ToInt32(ddlIndiviReqType.SelectedValue);
            //string Person = txtPerson.Text.Trim();
            string ClientName = txtClientName.Text.Trim();
            Division = Convert.ToInt32(ddlDivisionInd.SelectedValue);
            Plant = Convert.ToInt32(ddlPlantInd.SelectedValue);
            // string InvoiceNo = txtInvoiceNo.Text.Trim();
            string SupplierName = txtSupplierName.Text.Trim();

            //int DutyPF = Convert.ToInt32(ddlDutyCustom.SelectedValue);
            //int PackageUnit = Convert.ToInt32(ddlPackagesUnit.SelectedValue);         

            int GrossWt = Convert.ToInt32(ddlGrossWt.SelectedValue);
            //int Destination = Convert.ToInt32(ddlDestination.SelectedValue);
            //int CountyOrigin = Convert.ToInt32(ddlCountyOrigin.SelectedValue);
            //int PlaceOrigin = Convert.ToInt32(ddlPlaceOrigin.SelectedValue);

            if (rdlDiscountAppli.SelectedValue == "1")
            {
                Discount = true;
            }
            if (rdlReImport.SelectedValue == "1")
            {
                ReImport = true;
            }
            if (rdlPreviousImport.SelectedValue == "1")
            {
                PrevImport = true;
            }

            //

            string BuyerName = txtBuyerName.Text.Trim();
            string SchemeCode = txtSchemeCode.Text.Trim();
            
            if (rdlPrevExpGoods.SelectedValue == "1")
            {
                PrevExpGoods = true;
            }
            if (rdlCessDetail.SelectedValue == "1")
            {
                CessDetail = true;
            }
            if (rdlLicenceRegNo.SelectedValue == "1")
            {
                LicenceRegNo = true;
            }
            if (rdlReExport.SelectedValue == "1")
            {
                ReExport = true;
            }
            if (rdlPrevExport.SelectedValue == "1")
            {
                PrevExport = true;
            }

            //

            int Currency = Convert.ToInt32(ddlCurrency.SelectedValue);

            if (txtCIFValue.Text.Trim() != "")
            {
                CIFValue = Convert.ToDecimal(txtCIFValue.Text.Trim());
            }
            if (txtExRate.Text.Trim() != "")
            {
                ExRate = Convert.ToDecimal(txtExRate.Text.Trim());
            }

            if (txtAssesValue.Text.Trim() != "")
            {
                AssesableValue = Convert.ToDecimal(txtAssesValue.Text.Trim());
            }

            string InwardBENo = txtInwardBENo.Text.Trim();
            string InwardJobNo = txtInwardJobNo.Text.Trim();

            if (txtInwardBEDate.Text.Trim() != "")
            {
                InwardBEDate = Commonfunctions.CDateTime(txtInwardBEDate.Text.Trim());
            }

            //if (txtDaysStore.Text.Trim() != "")
            //{
            //    DaysStore = Convert.ToInt32(txtDaysStore.Text.Trim());
            //}

            string BENo = txtBeNo.Text.Trim();

            if (txtBEDate.Text.Trim() != "")
            {
                BEDate = Commonfunctions.CDateTime(txtBEDate.Text.Trim());
            }



            string RequestId = txtRequestId.Text.Trim();

            //if (txtDutyAmnt.Text.Trim() != "")
            //{
            //    DutyAmount = Convert.ToDecimal(txtDutyAmnt.Text.Trim());
            //}

            //if (txtInwardDate.Text.Trim() != "")
            //{
            //    InwardDate = Commonfunctions.CDateTime(txtInwardDate.Text.Trim());
            //}

            //if (txtPackages.Text.Trim() != "")
            //{
            //    NoOfPackages = Convert.ToInt32(txtPackages.Text.Trim());
            //}

            //if (txtGrossWt.Text.Trim() != "")
            //{
            //    GrossWeight = Convert.ToDecimal(txtGrossWt.Text.Trim());
            //}

            //if (txtVehicles.Text.Trim() != "")
            //{
            //    NoOfVehicles = Convert.ToInt32(txtVehicles.Text.Trim());
            //}

            //string BEType = ddlBEType.SelectedValue;

            if (txtOutwardDate.Text.Trim() != "")
            {
                OutwardDate = Commonfunctions.CDateTime(txtOutwardDate.Text.Trim());
            }

            if (txtPCDDahej.Text.Trim() != "")
            {
                PCDFrDahej = Commonfunctions.CDateTime(txtPCDDahej.Text.Trim());
            }
            if (txtPCDSentClient.Text.Trim() != "")
            {
                PCDSentClient = Commonfunctions.CDateTime(txtPCDSentClient.Text.Trim());
            }
            if (txtFileSentBilling.Text.Trim() != "")
            {
                FileSentToBilling = Commonfunctions.CDateTime(txtFileSentBilling.Text.Trim());
            }

            string BillingStatus = txtBillingStatus.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string RNLogistics = txtRNLogistics.Text.Trim();


            int result = SEZOperation.AddSEZJobDetail(JobRefNo, SEZType, RequestType, SEZMode, CustomerId, Division, Plant,
                          Currency, ExRate, AssesableValue, InwardBENo, InwardJobNo, InwardBEDate, BENo, BEDate, RequestId,
                          OutwardDate, PCDFrDahej, PCDSentClient, FileSentToBilling, BillingStatus, Remark, CIFValue,
                          GrossWt, Discount, ReImport, PrevImport, SupplierName,
                          BuyerName, SchemeCode, PrevExpGoods, CessDetail, LicenceRegNo, ReExport, PrevExport, 
                          LoggedInUser.glFinYearId, LoggedInUser.glUserId, LoggedInUser.glUserId);

            if (result == 1)
            {
                int result1 = 0; // JobID = 0 

                // ----------------------- Insert Gridview For Document Details -------------------------               

                if (grdDocument.Rows.Count > 0)
                {
                    for (int i = 0; i < grdDocument.Rows.Count; i++)
                    {
                        Label strDocTypeID = (Label)grdDocument.Rows[i].Cells[1].FindControl("lblDocType");
                        Label strDocPath = (Label)grdDocument.Rows[i].Cells[1].FindControl("lblDocPath");
                        Label strFileName = (Label)grdDocument.Rows[i].Cells[2].FindControl("lblDocName");                        

                        result1 = SEZOperation.AddSEZDocument(jobid, JobRefNo, Convert.ToInt32(strDocTypeID.Text.Trim()), strDocPath.Text.Trim(), strFileName.Text.Trim(), LoggedInUser.glUserId);

                    }
                    if (result1 == 1)
                    {
                        ViewState["AnnexureDoc2"] = null;
                    }
                }
                // -------------------------------------------------------------------------------------

                // ----------------------- Insert Gridview For Invoice Details -------------------------

                if (GrdInvoiceDetail.Rows.Count > 0)
                {
                    //int SEZType = Convert.ToInt32(rdbSEZtype.SelectedValue);
                    for (int i = 0; i < GrdInvoiceDetail.Rows.Count; i++)
                    {
                        DateTime dtInvoice = DateTime.MinValue;
                        decimal InvoiceValue1 = 0;
                        decimal Quantity = 0, ItemPrice = 0, ProductValue = 0, CTH = 0;

                        TextBox strInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtInvoiceNum");
                        TextBox strInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtInvoiceDt");
                        if (strInvoiceDt.Text.Trim() != "")
                        {
                            dtInvoice = Commonfunctions.CDateTime(strInvoiceDt.Text.Trim());
                        }
                        TextBox strValueInvoice = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtValueInvoice");

                        if (strValueInvoice.Text.Trim() != "")
                        {
                            InvoiceValue1 = Convert.ToDecimal(strValueInvoice.Text.Trim());
                        }

                        DropDownList strddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("ddlTermInvoice");
                        TextBox strDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtDescriptionProd");
                        string Qty = "0";
                        if (SEZType == 1)
                        {
                            TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtQuantity");
                            Qty = strQuantity.Text.Trim();
                        }
                        if (SEZType == 2)
                        {
                            if (txtInwardJobNo.Text.Trim() != "")
                            {
                                TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtRemainingQty");
                                Qty = strQuantity.Text.Trim();
                            }
                            else
                            {
                                TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtQuantity");
                                Qty = strQuantity.Text.Trim();
                            }
                        }
                        if (Qty != "")
                        {
                            Quantity = Convert.ToDecimal(Qty);
                        }

                        TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtItemPrice");
                        if (txtItemPrice.Text.Trim() != "")
                        {
                            ItemPrice = Convert.ToDecimal(txtItemPrice.Text.Trim());
                        }

                        TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtProductVal");
                        if (txtProductVal.Text.Trim() != "")
                        {
                            ProductValue = Convert.ToDecimal(txtProductVal.Text.Trim());
                        }

                        TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtCTH");
                        if (txtCTH.Text.Trim() != "")
                        {
                            CTH = Convert.ToDecimal(txtCTH.Text.Trim());
                        }

                        DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("ddlItemType");

                        if (strInvoiceNum.Text.Trim() != "")
                        {
                            result1 = SEZOperation.AddSEZInvoice(jobid, JobRefNo, SEZType, strInvoiceNum.Text.Trim(), dtInvoice, InvoiceValue1,
                                Convert.ToInt32(strddlTermInvoice.SelectedValue), strDescriptionProd.Text.Trim(), Quantity, ItemPrice,
                                ProductValue, CTH, Convert.ToInt32(ddlItemType.SelectedValue), LoggedInUser.glUserId);
                        }
                    }
                }
                // -------------------------------------------------------------------------------------------------------------  

                // --------------- Insert Container Details --------------------------------------------------------------------                

                if (GrvCantainerDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < GrvCantainerDetail.Rows.Count; i++)
                    {
                        TextBox strContainerID = (TextBox)GrvCantainerDetail.Rows[i].Cells[1].FindControl("TxtCantainerNO");
                        DropDownList ContainerType = (DropDownList)GrvCantainerDetail.Rows[i].Cells[1].FindControl("ddlCantainerType");
                        DropDownList ContainerSize = (DropDownList)GrvCantainerDetail.Rows[i].Cells[2].FindControl("ddlSize");

                        if (strContainerID.Text.Trim() != "")
                        {
                            result1 = SEZOperation.ADDSEZContainer(JobRefNo, strContainerID.Text.Trim(), Convert.ToInt32(ContainerType.SelectedValue), Convert.ToInt32(ContainerSize.SelectedValue), LoggedInUser.glUserId);
                        }
                    }
                }

                // ------------------------------------------------------------------------------------------------------------


                if (result == 1)
                {
                    lblError.Text = "Successfully Added SEZ Job Detail..!!";
                    lblError.CssClass = "success";

                    btnSubmit.Visible = false;
                    btnNew.Visible = true;
                }
                else if (result == -1)
                {
                    lblError.Text = "Successfully Added SEZ Job Detail But Document is not Properly added";
                    lblError.CssClass = "success";

                    btnSubmit.Visible = false;
                    btnNew.Visible = true;
                }
            }
            else if (result == -2)
            {
                lblError.Text = "Job Number Already Created.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Enter Job No.!";
            lblError.CssClass = "errorMsg";
        }
        //}
        //catch ( Exception ex)
        //{

        //}
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("SEZDetail.aspx");
    }

    #region Document
    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        // String ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            //ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\SEZ\\" + DocumentPath);
            DocumentPath = "..\\UploadFiles\\" + DocumentPath;
            ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);
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

    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        lblError.Text = "";
        int PkId = 1;

        #region ADD ROWS To DT FROM GRID VIEW 

        if (grdDocument != null && grdDocument.Rows.Count > 0)
        {

            ViewState["AnnexureDoc2"] = ViewState["AnnexureDoc3"];
            DataTable dtAnnexure1 = (DataTable)ViewState["AnnexureDoc2"];
            if (dtAnnexure1 != null)
            {
                dtAnnexure1.Rows.Clear();
                for (int i = 0; i < grdDocument.Rows.Count; i++)
                {
                    Label lblDocTypeName = (Label)grdDocument.Rows[i].FindControl("lblDocTypeName");
                    string DocTypeName = lblDocTypeName.Text.Trim();

                    Label lblDocType = (Label)grdDocument.Rows[i].FindControl("lblDocType");
                    string DocType1 = lblDocType.Text.Trim();
                    int DocType = 0;
                    if (DocType1 != "")
                    {
                        DocType = Convert.ToInt32(DocType1);
                    }

                    Label lblDocId = (Label)grdDocument.Rows[i].FindControl("lblDocPath");
                    string DocPath = lblDocId.Text.Trim();

                    Label lblDocName = (Label)grdDocument.Rows[i].FindControl("lblDocName");
                    string DocName = lblDocName.Text.Trim();
                    //string jobid = "1000001";
                    dtAnnexure1.Rows.Add(PkId, DocType, DocTypeName, DocPath, DocName, LoggedInUser.glEmpName);
                    PkId++;
                }
                ViewState["AnnexureDoc2"] = dtAnnexure1;
            }
        }

        #endregion

        int OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";
        string strFile = "";

        if (fuDocument != null && fuDocument.HasFile)
        {
            fileName = UploadFiles(fuDocument, "");
        }

        if (fileName != "")
        {
            DataTable dtAnnexure = new DataTable();
            dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];

            if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
            {
                for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                {
                    if (dtAnnexure.Rows[i]["PkId"] != null)
                    {
                        PkId = Convert.ToInt32(dtAnnexure.Rows[i]["PkId"].ToString());
                        PkId++;
                    }
                }
            }
            if (dtAnnexure != null)
                OriginalRows = dtAnnexure.Rows.Count;              //get original rows of grid view.

            string DocFolder = txtJobNo.Text.Trim();
            DocFolder = DocFolder.Replace("/", "");
            DocFolder = DocFolder.Replace("-", "");

            // fileName = "UploadFiles\\SEZ\\" + DocFolder + "\\" + fileName;
            fileName = "SEZ\\" + fileName;
            int doctype = Convert.ToInt32(ddDocument.SelectedValue);
            string doctypeName = ddDocument.SelectedItem.Text;

            dtAnnexure.Rows.Add(PkId, doctype, doctypeName, fileName, fuDocument.FileName, LoggedInUser.glEmpName);
            AfterInsertedRows = dtAnnexure.Rows.Count;     //get present rows after deleting particular row from grid view.
            ViewState["AnnexureDoc2"] = dtAnnexure;
            BindGrid();
            if (OriginalRows < AfterInsertedRows)
            {
                lblError.Text = "Document Added successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void BindGrid()
    {
        if (ViewState["AnnexureDoc2"].ToString() != "")
        {
            if (grdDocument.Rows.Count > 0)
            {
                //DataTable dt = new DataTable();
                //dt.Columns.Add("ID");
                //dt.Columns.Add("Name");
                //dt.Columns.Add("Age");
                //DataRow dataRow;
                //dataRow = dt.NewRow();
                //int i2 = 1;
                //for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                //{
                //    dataRow[i] = GridView1.SelectedRow.Cells[i2].Text;
                //    i2++;
                //}
                //dt.Rows.Add(dataRow);
            }

            DataTable dtAnnexureDoc = (DataTable)ViewState["AnnexureDoc2"];
            if (dtAnnexureDoc.Rows.Count > 0)
            {
                grdDocument.DataSource = dtAnnexureDoc;
                grdDocument.DataBind();
            }
            else
            {
                grdDocument.DataBind();
            }
        }
    }

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        //string FileName = FU.FileName;
        //string ServerFilePath = FileServer.GetFileServerDir();
        string DocFolder = txtJobNo.Text.Trim();
        DocFolder = DocFolder.Replace("/", "");
        DocFolder = DocFolder.Replace("-", "");

        //if (ServerFilePath == "")
        //    ServerFilePath = Server.MapPath("..\\UploadFiles\\SEZ\\" + DocFolder + "\\" + FilePath);
        //else
        //    ServerFilePath = ServerFilePath + DocFolder + "\\" + FilePath;

        string FileName = FU.FileName;

        FileName = FileServer.ValidateFileName(FileName);

        string FilePath2 = DocFolder + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\SEZ\\" + FilePath2);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "\\SEZ\\" + FilePath2;
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

            //return FilePath + FileName;
            return FilePath2 + FileName;
        }
        else
            return "";
    }

    private string UploadDocument(string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PreAlertDoc\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }
        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            try
            {
                DirectoryInfo dirDoc = System.IO.Directory.CreateDirectory(ServerFilePath);
            }
            catch (Exception ex)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);
                string strDesc = "Dir Path:" + ServerFilePath;
                DBOperations.AddErrorLog(JobId, "Job Dir Create Error", "", ex.Message, strDesc, LoggedInUser.glUserId);
                lblError.Text = ex.Message;
                return "";
            }
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
            return "";
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

    #region  Invoice Detail

    private void SetInitialRow()
    {
        //GrdInvoiceDetail.DataSource = null;
        //GrdInvoiceDetail.DataBind();

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceValue", typeof(string)));
        dt.Columns.Add(new DataColumn("Term", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("Quantity", typeof(string)));

        dt.Columns.Add(new DataColumn("NewQty", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemPrice", typeof(string)));
        dt.Columns.Add(new DataColumn("ProdValue", typeof(string)));
        dt.Columns.Add(new DataColumn("CTH", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemType", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["InvoiceNo"] = string.Empty;
        dr["InvoiceDate"] = string.Empty;
        dr["InvoiceValue"] = string.Empty;
        dr["Term"] = string.Empty;
        dr["Description"] = string.Empty;
        dr["Quantity"] = string.Empty;

        dr["NewQty"] = string.Empty;
        dr["ItemPrice"] = string.Empty;
        dr["ProdValue"] = string.Empty;
        dr["CTH"] = string.Empty;
        dr["ItemType"] = string.Empty;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();
        ViewState["CurrentTable"] = null;
        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        GrdInvoiceDetail.DataSource = dt;
        GrdInvoiceDetail.DataBind();

    }

    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[1].FindControl("txtInvoiceNum");
                    TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[2].FindControl("txtInvoiceDt");
                    TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtValueInvoice");
                    DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[4].FindControl("ddlTermInvoice");
                    TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[5].FindControl("txtDescriptionProd");
                    TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtQuantity");

                    TextBox txtRemainingQty = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtRemainingQty");
                    TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtItemPrice");
                    TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtProductVal");
                    TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtCTH");
                    DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("ddlItemType");

                    drCurrentRow = dtCurrentTable.NewRow();
                    // drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["InvoiceNo"] = txtInvoiceNum.Text;
                    dtCurrentTable.Rows[i - 1]["InvoiceDate"] = txtInvoiceDt.Text;
                    dtCurrentTable.Rows[i - 1]["InvoiceValue"] = txtValueInvoice.Text;
                    dtCurrentTable.Rows[i - 1]["Term"] = ddlTermInvoice.Text;
                    dtCurrentTable.Rows[i - 1]["Description"] = txtDescriptionProd.Text;
                    dtCurrentTable.Rows[i - 1]["Quantity"] = txtQuantity.Text;

                    dtCurrentTable.Rows[i - 1]["NewQty"] = txtRemainingQty.Text;
                    dtCurrentTable.Rows[i - 1]["ItemPrice"] = txtItemPrice.Text;
                    dtCurrentTable.Rows[i - 1]["ProdValue"] = txtProductVal.Text;
                    dtCurrentTable.Rows[i - 1]["CTH"] = txtCTH.Text;
                    dtCurrentTable.Rows[i - 1]["ItemType"] = ddlItemType.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                GrdInvoiceDetail.DataSource = dtCurrentTable;
                GrdInvoiceDetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
    }

    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[1].FindControl("txtInvoiceNum");
                    TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[2].FindControl("txtInvoiceDt");
                    TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtValueInvoice");
                    DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("ddlTermInvoice");
                    TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtDescriptionProd");
                    TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtQuantity");

                    TextBox txtRemainingQty = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtRemainingQty");
                    TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtItemPrice");
                    TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtProductVal");
                    TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtCTH");
                    DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("ddlItemType");

                    txtInvoiceNum.Text = dt.Rows[i]["InvoiceNo"].ToString();
                    txtInvoiceDt.Text = dt.Rows[i]["InvoiceDate"].ToString();
                    txtValueInvoice.Text = dt.Rows[i]["InvoiceValue"].ToString();
                    ddlTermInvoice.SelectedValue = dt.Rows[i]["Term"].ToString();
                    txtDescriptionProd.Text = dt.Rows[i]["Description"].ToString();
                    txtQuantity.Text = dt.Rows[i]["Quantity"].ToString();

                    txtRemainingQty.Text = dt.Rows[i]["NewQty"].ToString();
                    txtItemPrice.Text = dt.Rows[i]["ItemPrice"].ToString();
                    txtProductVal.Text = dt.Rows[i]["ProdValue"].ToString();
                    txtCTH.Text = dt.Rows[i]["CTH"].ToString();
                    ddlItemType.SelectedValue = dt.Rows[i]["ItemType"].ToString();

                    rowIndex++;
                }
            }
        }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        // ModalPopupContainer.Show();
    }

    protected void GrdInvoiceDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        GrdInvoiceDetail.Caption = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        }
    }

    #endregion

    #region Invoice Excel Operation

    protected void btnUploadInvoice_Click(object sender, EventArgs e)
    {
        string fileName = "";

        if (fuDocument != null && FuInvoice.HasFile)
        {
            fileName = UploadExcelFiles(FuInvoice, "");
        }

        //if (FuInvoice.HasFile)
        //{
        //    string FileName = Path.GetFileName(FuInvoice.PostedFile.FileName);
        //    string Extension = Path.GetExtension(FuInvoice.PostedFile.FileName);
        //    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

        //    string DocFolder = txtJobNo.Text.Trim();
        //    DocFolder = DocFolder.Replace("/", "");
        //    DocFolder = DocFolder.Replace("-", "");

        //    string FilePath = Server.MapPath("..\\UploadFiles\\SEZ\\InvoiceDoc\\" + DocFolder + "\\" + FileName);
        //   // string FilePath = Server.MapPath(FolderPath + FileName);
        //    FuInvoice.SaveAs(FilePath);
        //    Import_To_Grid(FilePath, Extension);
        //}
    }

    public string UploadExcelFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();
        // string FileName = Path.GetFileName(FuInvoice.PostedFile.FileName);

        string DocFolder = txtJobNo.Text.Trim();
        DocFolder = DocFolder.Replace("/", "");
        DocFolder = DocFolder.Replace("-", "");


        if (ServerFilePath == "")
            ServerFilePath = Server.MapPath("..\\UploadFiles\\SEZ\\InvoiceExcel\\" + DocFolder + "\\" + FilePath);
        else
            ServerFilePath = ServerFilePath + "\\SEZ\\InvoiceExcel\\" + DocFolder + "\\" + FilePath;

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


            string Extension = Path.GetExtension(FU.PostedFile.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            string FileNameDSR = ServerFilePath + FileName;
            // FileNameDSR = FileNameDSR.Replace("\\", "\"");
            // FileNameDSR = FileNameDSR.Replace("\"", "\\");
            FileNameDSR = FileNameDSR.Replace("\\\\", "\\");

            Import_To_Grid(FileNameDSR, Extension);
            //Import_To_Grid(ServerFilePath + FileName, Extension);
            return FilePath + FileName;
        }
        else
            return "";
    }
    private void Import_To_Grid(string FilePath, string Extension)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls":
                //conStr = ConfigurationManager.ConnectionStrings["XLSConString"].ConnectionString;
                //break;
                conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;"
                           + @"Data Source=" + FilePath + ";" +
                           @"Extended Properties='Excel 8.0;HDR=Yes;'";
                break;
            case ".xlsx":
                //conStr = ConfigurationManager.ConnectionStrings["XLSXConString"].ConnectionString;
                //break;
                conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;"
                           + @"Data Source=" + FilePath + ";" +
                           @"Extended Properties='Excel 8.0;HDR=Yes;'";
                break;
        }
        conStr = String.Format(conStr, FilePath, FilePath);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        if (Extension.ToLower() == ".xls" || Extension.ToLower() == ".xlsx")
        {
            //GrdInvoiceDetail.Controls.Clear();        

            if (GrdInvoiceDetail.Rows.Count > 1)
            {
                GrdInvoiceDetail.DataSource = null;
                GrdInvoiceDetail.DataBind();

                SetInitialRow();
            }

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet


            try
            {

                connExcel.Open();
                cmdExcel.CommandText = "SELECT * FROM [" + SheetName + "] WHERE [Invoice No] IS NOT NULL AND [Invoicedate] IS NOT NULL";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "Please check Excel column or Format";
                lblError.CssClass = "errorMsg";
                return;
            }

            //Bind Data to GridView
            GrdInvoiceDetail.Caption = Path.GetFileName(FilePath);

            if (GrdInvoiceDetail.Rows.Count == 1)
            {
                //int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() != "" && i > 0)
                    {
                        //j++;
                        //if (j > 1)
                        AddNewRowToGrid();
                    }
                }
            }

            if (GrdInvoiceDetail.Rows.Count > 0)
            {
                for (int i = 0; i < GrdInvoiceDetail.Rows.Count; i++)
                {
                    TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtInvoiceNum");
                    TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtInvoiceDt");
                    TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtValueInvoice");
                    DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[i].FindControl("ddlTermInvoice");
                    TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtDescriptionProd");
                    TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtQuantity");

                    if (dt.Rows[i][0].ToString() != "")
                    {
                        if (dt.Columns.Contains("AAAA"))
                        {
                            int aa = dt.Columns.Count;
                        }

                        txtInvoiceNum.Text = dt.Rows[i][0].ToString();
                        txtInvoiceDt.Text = dt.Rows[i][1].ToString();
                        string InvDate = Convert.ToDateTime(txtInvoiceDt.Text).ToString("dd/MM/yyyy");
                        txtInvoiceDt.Text = InvDate;
                        txtValueInvoice.Text = dt.Rows[i][2].ToString();

                        string Term = "";
                        int TermID = 0;
                        if (dt.Rows[i][3] != DBNull.Value)
                        {
                            Term = dt.Rows[i][3].ToString();


                            DataSet dsTermID = SEZOperation.GetTermIdFromTerm(Term.Trim());


                            if (dsTermID.Tables[0].Rows.Count > 0)
                            {
                                TermID = Convert.ToInt32(dsTermID.Tables[0].Rows[0]["lid"]);
                            }
                        }

                        ddlTermInvoice.SelectedValue = Convert.ToString(TermID);
                        txtDescriptionProd.Text = dt.Rows[i][4].ToString();
                        txtQuantity.Text = dt.Rows[i][5].ToString();
                    }
                }
            }
            //GrdInvoiceDetail.DataSource = dt;
            //GrdInvoiceDetail.DataBind();
        }
    }

    #endregion

    protected void txtInwardJobNo_TextChanged(object sender, EventArgs e)
    {
        string JobRefNo = txtInwardJobNo.Text.ToUpper().Trim();
        int count = 0;
        if (JobRefNo.Contains("/"))
        {
            //Code to process string with \

             count = JobRefNo.Split('/').Length - 1;
        }
        else
        {
            JobRefNo = "";
            string message = "Please Check Inward Job Reference Number";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
        }

        if (JobRefNo != "" && count == 2)
        { 
            string[] words = JobRefNo.Split('/');
            string JobNo = words[0].ToString().Trim();
            string JobLoc = words[1].ToString().ToUpper().Trim();
            string JobYear = words[2].ToString().Trim();

            if (JobLoc == "DSIB")
            {
                DataSet dsJobDetail = SEZOperation.GetSEZJobDetailByJobRefNo(JobRefNo);
                DateTime InwardBEDate = DateTime.MinValue;

                string BEDate = "", InwardDate = "", OutwardDate = "", PCDFrDahej = "", PCDSentClient = "", FileSentToBilling = "", SEZCurrency = "0";
                string DutyPF = "0", PackageUnit = "0", Buyer = "", GrossWtUnit = "0", Destination = "0", CountryOfOrigin = "0", PlaceOfOrigin = "0";
                int DaysStore = 0, NoOfPackages = 0, GrossWeight = 0, NoOfVehicles = 0, SEZServicesProvide = 0;
                decimal ExRate = 0, AssesableValue = 0, DutyAmount = 0, CIFAmount = 0;

                if (dsJobDetail.Tables[0].Rows.Count > 0)
                {
                    int lid = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["lid"]);
                    Session["lid"] = lid;
                    int SEZType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["SEZTypeID"]);
                    string SEZMode = dsJobDetail.Tables[0].Rows[0]["SEZMode"].ToString();
                    if(SEZMode == "")
                    {
                        SEZMode = "0";
                    }
                    string Person = dsJobDetail.Tables[0].Rows[0]["Person"].ToString();
                    int ClientName = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["ClientName"]);
                    string SupplierName = dsJobDetail.Tables[0].Rows[0]["SupplierName"].ToString();

                    if (dsJobDetail.Tables[0].Rows[0]["SEZCurrency"] != DBNull.Value)
                    {
                        SEZCurrency = dsJobDetail.Tables[0].Rows[0]["SEZCurrency"].ToString();
                    }
                    //-------------------------

                    Buyer = dsJobDetail.Tables[0].Rows[0]["BuyerName"].ToString();

                    if (dsJobDetail.Tables[0].Rows[0]["CIFValue"] != DBNull.Value)
                    {
                        CIFAmount = Convert.ToDecimal(dsJobDetail.Tables[0].Rows[0]["CIFValue"]);
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["DutyPF"] != DBNull.Value)
                    {
                        DutyPF = dsJobDetail.Tables[0].Rows[0]["DutyPF"].ToString();
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["PackageUnit"] != DBNull.Value)
                    {
                        PackageUnit = dsJobDetail.Tables[0].Rows[0]["PackageUnit"].ToString();
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["GrossWtUnit"] != DBNull.Value)
                    {
                        GrossWtUnit = dsJobDetail.Tables[0].Rows[0]["GrossWtUnit"].ToString();
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["Destination"] != DBNull.Value)
                    {
                        Destination = dsJobDetail.Tables[0].Rows[0]["Destination"].ToString();
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["Country"] != DBNull.Value)
                    {
                        CountryOfOrigin = dsJobDetail.Tables[0].Rows[0]["Country"].ToString();
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["Place"] != DBNull.Value)
                    {
                        PlaceOfOrigin = dsJobDetail.Tables[0].Rows[0]["Place"].ToString();
                    }

                    //---------------------------------------

                    if (dsJobDetail.Tables[0].Rows[0]["ExRate"] != DBNull.Value)
                    {
                        ExRate = Convert.ToDecimal(dsJobDetail.Tables[0].Rows[0]["ExRate"]);
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["AssesableValue"] != DBNull.Value)
                    {
                        AssesableValue = Convert.ToDecimal(dsJobDetail.Tables[0].Rows[0]["AssesableValue"]);
                    }

                    string InwardBENo = dsJobDetail.Tables[0].Rows[0]["InwardBENo"].ToString();

                    if (dsJobDetail.Tables[0].Rows[0]["InwardBEDate"] != DBNull.Value)
                    {
                        // string IBD = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["InwardBEDate"]).ToString("dd-MM-yyyy");
                        InwardBEDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["InwardBEDate"]);
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["DaysStore"] != DBNull.Value)
                    {
                        DaysStore = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["DaysStore"]);
                    }

                    string BENo = dsJobDetail.Tables[0].Rows[0]["BENo"].ToString();

                    if (dsJobDetail.Tables[0].Rows[0]["BEDate"] != DBNull.Value)
                    {
                        BEDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["BEDate"]).ToString("dd/MM/yyyy");
                        // BEDate = Convert.ToDateTime(BD);
                    }

                    string RequestId = dsJobDetail.Tables[0].Rows[0]["RequestId"].ToString();

                    if (dsJobDetail.Tables[0].Rows[0]["DutyAmount"] != DBNull.Value)
                    {
                        DutyAmount = Convert.ToDecimal(dsJobDetail.Tables[0].Rows[0]["DutyAmount"]);
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["InwardDate"] != DBNull.Value)
                    {
                        // InwardDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["InwardDate"]);
                        InwardDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["InwardDate"]).ToString("dd/MM/yyyy");
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["NoOfPackages"] != DBNull.Value)
                    {
                        NoOfPackages = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["NoOfPackages"]);
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["GrossWeight"] != DBNull.Value)
                    {
                        GrossWeight = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["GrossWeight"]);
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["NoOfVehicles"] != DBNull.Value)
                    {
                        NoOfVehicles = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["NoOfVehicles"]);
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["SEZServicesProvide"] != DBNull.Value)
                    {
                        SEZServicesProvide = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["SEZServicesProvide"]);
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["OutwardDate"] != DBNull.Value)
                    {
                        //OutwardDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["OutwardDate"]);
                        OutwardDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["OutwardDate"]).ToString("dd/MM/yyyy");
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["PCDFrDahej"] != DBNull.Value)
                    {
                        //PCDFrDahej1 = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["PCDFrDahej"]).ToString("dd/MM/yyyy");
                        PCDFrDahej = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["PCDFrDahej"]).ToString("dd/MM/yyyy");
                    }

                    if (dsJobDetail.Tables[0].Rows[0]["PCDSentClient"] != DBNull.Value)
                    {
                        PCDSentClient = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["PCDSentClient"]).ToString("dd/MM/yyyy");
                    }
                    if (dsJobDetail.Tables[0].Rows[0]["FileSentToBilling"] != DBNull.Value)
                    {
                        FileSentToBilling = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["FileSentToBilling"]).ToString("dd/MM/yyyy");
                    }
                    string BillingStatus = dsJobDetail.Tables[0].Rows[0]["BillingStatus"].ToString();
                    string Remark = dsJobDetail.Tables[0].Rows[0]["Remark"].ToString();

                    ddlMode.SelectedValue = SEZMode;
                    ddlCurrency.SelectedValue = SEZCurrency;
                    txtCIFValue.Text = Convert.ToString(CIFAmount);
                    ddlGrossWt.SelectedValue = GrossWtUnit;

                    txtExRate.Text = Convert.ToString(ExRate);
                    txtAssesValue.Text = Convert.ToString(AssesableValue);
                    txtInwardBENo.Text = InwardBENo;
                    if (InwardBEDate != DateTime.MinValue)
                    {
                        txtInwardBEDate.Text = Convert.ToString(InwardBEDate);
                    }
                    else
                    {
                        txtInwardBEDate.Text = "";
                    }

                    txtBeNo.Text = BENo;
                    txtBEDate.Text = BEDate;
                    txtRequestId.Text = RequestId;

                    txtOutwardDate.Text = OutwardDate;
                    txtPCDDahej.Text = PCDFrDahej;
                    txtPCDSentClient.Text = PCDSentClient;
                    txtFileSentBilling.Text = FileSentToBilling;
                    txtBillingStatus.Text = BillingStatus;
                    txtRemark.Text = Remark;

                    // ------- Container Details ----------

                    DataSet dsContainerDetail = SEZOperation.GetContainerDetailById(lid);
                    DataTable dtContainer = new DataTable();
                    if (dsContainerDetail.Tables.Count > 0)
                    {
                        dtContainer = dsContainerDetail.Tables[0];
                    }

                    if (GrvCantainerDetail.Rows.Count > 1)
                    {
                        GrvCantainerDetail.DataSource = null;
                        GrvCantainerDetail.DataBind();
                        SetInitialCantainerRow();
                    }


                    if (dsContainerDetail.Tables.Count > 0)
                    {

                        if (GrvCantainerDetail.Rows.Count == 1)
                        {   //int j = 0;
                            for (int i = 0; i < dsContainerDetail.Tables[0].Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    AddNewRowToCantainerGrid();
                                }
                            }
                        }

                        if (GrvCantainerDetail.Rows.Count > 0)
                        {
                            for (int i = 0; i < GrvCantainerDetail.Rows.Count; i++)
                            {

                                TextBox TxtCantainerNO = (TextBox)GrvCantainerDetail.Rows[i].FindControl("TxtCantainerNO");
                                DropDownList ddlCantainerType = (DropDownList)GrvCantainerDetail.Rows[i].FindControl("ddlCantainerType");
                                DropDownList ddlSize = (DropDownList)GrvCantainerDetail.Rows[i].FindControl("ddlSize");

                                if (dtContainer.Rows[i][0].ToString() != "")
                                {
                                    if (dtContainer.Columns.Contains("AAAA"))
                                    {
                                        int aa = dtContainer.Columns.Count;
                                    }

                                    TxtCantainerNO.Text = dtContainer.Rows[i][1].ToString();
                                    ddlCantainerType.SelectedValue = dtContainer.Rows[i][2].ToString();
                                    ddlSize.SelectedValue = dtContainer.Rows[i][3].ToString();
                                }
                            }

                        }
                    }

                    // END----------- Container Details------------------

                    // Start -------- Invoice Detail --------------------

                    DataSet dsInvoiceDetail = SEZOperation.GetInvoiceDetail(lid);
                    DataTable dtInvoice = new DataTable();
                    if (dsInvoiceDetail.Tables.Count > 0)
                    {
                        dtInvoice = dsInvoiceDetail.Tables[0];
                    }

                    //if (dsInvoiceDetail.Tables.Count > 0)
                    //{
                    if (GrdInvoiceDetail.Rows.Count > 1)
                    {
                        GrdInvoiceDetail.DataSource = null;
                        GrdInvoiceDetail.DataBind();

                        SetInitialRow();
                    }
                    //if (dsInvoiceDetail.Tables[0].Rows.Count > 0)
                    //    {

                    if (dsInvoiceDetail.Tables.Count > 0)
                    {
                        if (GrdInvoiceDetail.Rows.Count == 1)
                        {   //int j = 0;
                            for (int i = 0; i < dsInvoiceDetail.Tables[0].Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    //j++;
                                    //if (j > 1)
                                    AddNewRowToGrid();
                                    //SetPreviousData();
                                }
                            }
                        }

                        if (GrdInvoiceDetail.Rows.Count > 0)
                        {
                            for (int i = 0; i < GrdInvoiceDetail.Rows.Count; i++)
                            {
                                GrdInvoiceDetail.Columns[7].Visible = true;

                                string InvDate = "";

                                TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtInvoiceNum");
                                TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtInvoiceDt");
                                TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtValueInvoice");
                                DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[i].FindControl("ddlTermInvoice");
                                TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtDescriptionProd");
                                TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtQuantity");
                                txtQuantity.Enabled = false;

                               // TextBox txtRemainingQty = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtRemainingQty");
                                TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtItemPrice");
                               // TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtProductVal");
                                TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[i].FindControl("txtCTH");
                                DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[i].FindControl("ddlItemType");

                                if (dtInvoice.Rows[i][0].ToString() != "")
                                {
                                    if (dtInvoice.Columns.Contains("AAAA"))
                                    {
                                        int aa = dtInvoice.Columns.Count;
                                    }

                                    txtInvoiceNum.Text = dtInvoice.Rows[i][0].ToString();
                                    // txtInvoiceDt.Text = Convert.ToDateTime(dt.Rows[i][1]).ToShortDateString();
                                    txtInvoiceDt.Text = dtInvoice.Rows[i][1].ToString();
                                    if (txtInvoiceDt.Text != "")
                                    {
                                        InvDate = Convert.ToDateTime(txtInvoiceDt.Text).ToString("dd/MM/yyyy");
                                    }

                                    txtInvoiceDt.Text = InvDate;

                                    txtValueInvoice.Text = dtInvoice.Rows[i][2].ToString();
                                    ddlTermInvoice.SelectedValue = dtInvoice.Rows[i][3].ToString();
                                    txtDescriptionProd.Text = dtInvoice.Rows[i][4].ToString();
                                    txtQuantity.Text = dtInvoice.Rows[i][5].ToString();

                                    txtItemPrice.Text= dtInvoice.Rows[i][6].ToString();
                                    txtCTH.Text = dtInvoice.Rows[i][7].ToString();
                                    ddlItemType.SelectedValue = dtInvoice.Rows[i][8].ToString();
                                }
                            }
                        }
                        // }
                        // }
                    }

                    //--End -------- Invoice Detail -------------------------------


                    // DataSet dsInvoiceDetail = SEZOperation.GetInvoiceDetail(lid);

                    BindDocGrid();
                }
                else
                {
                    lblError.Text = "This Job Is Not Exist..Please Check";
                    lblError.CssClass = "errorMsg";
                    ClearAll();
                }
            }
            else
            {
                lblError.Text = "Allow Only Inward Job";
                lblError.CssClass = "errorMsg";
                txtInwardJobNo.Text = "";
                ClearAll();
            }
        }
        else
        {
            lblError.Text = "Inward Job Not Exist";
            lblError.CssClass = "errorMsg";
            ClearAll();
        }
    }

    public void ClearAll()
    {
        ddlMode.SelectedValue = "0";
        txtClientName.Text = "";
        ddlCurrency.SelectedValue = "0";
        txtExRate.Text = "";
        txtAssesValue.Text = "";
        txtInwardBENo.Text = "";
        txtInwardBEDate.Text = "";
        txtBeNo.Text = "";
        txtBEDate.Text = "";
        txtRequestId.Text = "";
        txtOutwardDate.Text = "";
        txtPCDDahej.Text = "";
        txtPCDSentClient.Text = "";
        txtFileSentBilling.Text = "";
        txtBillingStatus.Text = "";
        txtRemark.Text = "";

        GrdInvoiceDetail.DataSource = null;
        GrdInvoiceDetail.DataBind();
        SetInitialRow();

        grdDocument.DataSource = null;
        grdDocument.DataBind();
    }
    protected void BindDocGrid()
    {
        if (txtInwardJobNo.Text.Trim() != "")
        {
            DataSet dsJobID = SEZOperation.GetJobIdFromJobRefNo(txtInwardJobNo.Text.Trim());

            int JobID = 0;
            if (dsJobID.Tables.Count > 0)
            {
                if (dsJobID.Tables[0].Rows.Count > 0)
                {
                    JobID = Convert.ToInt32(dsJobID.Tables[0].Rows[0]["lid"]);
                }
            }
            else
            {
                ClearAll();
            }

            if (JobID != 0)
            {
                DataSet dsDocDetail = SEZOperation.GetDocDetail(JobID);
                if (dsDocDetail.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = dsDocDetail.Tables[0];
                    ViewState["AnnexureDoc2"] = dt;
                    grdDocument.DataSource = dsDocDetail;
                    grdDocument.DataBind();
                    //  grdDocument.DataBind();
                }
                else
                {

                }
            }
        }
    }

    #region DSR Excel Upload
    protected void btnUploadDSR_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        lblError.Text = "";

        if (Convert.ToInt32(ddlCustomer.SelectedValue) > 0)
        {

            if (Convert.ToInt32(ddlDivision.SelectedValue) > 0)
            {
                if (Convert.ToInt32(ddlPlant.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(ddlRequestType.SelectedValue) > 0)
                    {
                        string fileName = "";

                        if (FUDSRImport != null && FUDSRImport.HasFile)
                        {
                            fileName = UploadDSRExcelFiles(FUDSRImport, "");
                        }
                    }
                    else
                    {
                        lblError.Text = "Please Select Request Type";
                        lblError.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError.Text = "Please Select Plant";
                    lblError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError.Text = "Please Select Division";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please Select The Customer First";
            lblError.CssClass = "errorMsg";
        }
    }

    public string UploadDSRExcelFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string Extension1 = Path.GetExtension(FU.PostedFile.FileName);
        if (Extension1.ToLower().Trim() == ".xls" || Extension1.ToLower().Trim() == ".xlsx")
        {

            FileName = FileServer.ValidateFileName(FileName);

            string FilePath2 = "";

            string ServerFilePath = FileServer.GetFileServerDir();

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\SEZ\\UploadExcel\\" + FilePath2);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + "SEZ\\UploadExcel\\" + FilePath2;
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

                string Extension = Path.GetExtension(FU.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FileNameDSR = ServerFilePath + FileName;
                // FileNameDSR = FileNameDSR.Replace("\\", "\"");

                FileNameDSR = FileNameDSR.Replace("\\\\", "\\");

                ImportAutoRefNo(FileNameDSR, Extension);

                return FilePath2 + FileName;
            }
            else
                return "";
        }
        else
        {
            lblError.Text = "Please Select Excel File Only";
            lblError.CssClass = "errorMsg";
            return "";
        }
    }

    private void ImportAutoRefNo(string FilePath, string Extension)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls":
                //conStr = ConfigurationManager.ConnectionStrings["XLSConString"].ConnectionString;
                conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;"
                              + @"Data Source=" + FilePath + ";" +
                              @"Extended Properties='Excel 8.0;HDR=Yes;'";
                break;

            case ".xlsx":
                //conStr = ConfigurationManager.ConnectionStrings["XLSXConString"].ConnectionString;
                conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;"
                             + @"Data Source=" + FilePath + ";" +
                             @"Extended Properties='Excel 8.0;HDR=Yes;'";
                break;
        }
        conStr = String.Format(conStr, FilePath, FilePath);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        int RequestType = Convert.ToInt32(ddlRequestType.SelectedValue);

        if (Extension.ToLower() == ".xls" || Extension.ToLower() == ".xlsx")
        {
            //try
            //{

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet 
            try
            {
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "] WHERE [Request ID] IS NOT NULL";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
            }
            catch (Exception e)
            {
                lblError.Text = "Check Excel Column Name OR Format";
                lblError.CssClass = "errorMsg";
                return;
            }

            int dtRowCnt = dt.Rows.Count;
            int dtColCnt = dt.Columns.Count;

            connExcel.Close();

            int SEZezType = 0;
            if (rdbSEZtype.SelectedValue == "1")
            {
                SEZezType = 1;
            }
            else if (rdbSEZtype.SelectedValue == "2")
            {
                SEZezType = 2;
            }

            int TotalCnt = dt.Rows.Count;
            int ErrorCnt = 0;
            int SuccessCnt = 0;
            //int RepeatReqID = 0;

            if (SEZezType != 0)
            {
                //try
                //{
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (RequestType == 3)   // For DTA Sales Detail
                    {
                        int Result1;
                        string GoodsMeasureUnit = Convert.ToString(dt.Rows[k]["Goods measurement Unit"]).ToUpper().Trim();
                        Result1 = SEZOperation.AddSEZExcelFillMasters(GoodsMeasureUnit, LoggedInUser.glUserId);
                        if (Result1 == 1)
                        {
                            lblError.Text = "Masters Fill Successfully To Excel File";
                            lblError.CssClass = "success";
                        }
                        else
                        {
                            lblError.Text = "Error In Execution At The Time of Fill Masters Data To Excel File..!";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                    else if (RequestType == 2)   // For SB (Shipping Bill)
                    {
                        int Result1;
                        string GoodsMeasureUnit = Convert.ToString(dt.Rows[k]["Unit of Measurement"]).ToUpper().Trim();
                        Result1 = SEZOperation.AddSEZExcelFillMasters(GoodsMeasureUnit, LoggedInUser.glUserId);
                        if (Result1 == 1)
                        {
                            lblError.Text = "Masters Fill Successfully To Excel File";
                            lblError.CssClass = "success";
                        }
                        else
                        {
                            lblError.Text = "Error In Execution At The Time of Fill Masters Data To Excel File..!";
                            lblError.CssClass = "errorMsg";
                        }
                    }

                    //else if (RequestType == 1)  // For BOE Details
                    //{
                    //    int Result1;
                    //    string GoodsMeasureUnit = Convert.ToString(dt.Rows[k]["Item measurement Unit"]).ToUpper().Trim();
                    //    Result1 = SEZOperation.AddSEZExcelFillMasters(GoodsMeasureUnit, LoggedInUser.glUserId);
                    //    if (Result1 == 1)
                    //    {
                    //        lblError.Text = "Masters Fill Successfully To Excel File";
                    //        lblError.CssClass = "success";
                    //    }
                    //    else
                    //    {
                    //        lblError.Text = "Error In Execution At The Time of Fill Masters Data To Excel File..!";
                    //        lblError.CssClass = "errorMsg";
                    //    }
                    //}

                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string strReqID1 = "";
                    int inc = 0;
                    string strBENo = "";

                    try
                    {
                        int SEZType = Convert.ToInt32(rdbSEZtype.SelectedValue);
                        string JobRefNo = SEZOperation.GetGenerateSEZJobNo(SEZezType).Trim();

                        //int 
                        string GoodsMeasureUnit = "", SchemeCode = "", SupplierName = "";
                        decimal strAssesableVal = 0, strCIFVal = 0, Quantity = 0, ItemPrice = 0, Productvalue = 0, CTH = 0, DutyAmnt = 0, ExRate = 0, InvoiceValue = 0; ;
                        DateTime BEDate = DateTime.MinValue, InvoiceDate = DateTime.MinValue;
                        Boolean Discount = false, ReImport = false, PrevImport = false, PrevExpGoods = false, CessDetail = false, LicenceRegNo = false, ReExport = false, PrevExport = false;

                        if (JobRefNo != "")
                        {
                            if (RequestType == 3 || RequestType == 1)  // 3-DTA Sales && 1-BOE Details
                            {
                                strReqID1 = dt.Rows[j]["Request id"].ToString().Trim(); //ToString().Trim();
                                string strReqID = dt.Rows[j]["Request id"].ToString().Trim(); //ToString().Trim();                              

                                strBENo = dt.Rows[j]["Thoka no"].ToString().Trim();
                                string strBEDate = Convert.ToDateTime(dt.Rows[j]["Thoka No# Date & time"]).ToString("dd-MM-yyyy");
                                if (strBEDate != "")
                                {
                                    BEDate = Convert.ToDateTime(strBEDate);
                                }
                                string strInvoiceNo = dt.Rows[j]["Invoice No"].ToString().Trim();

                                string strInvDate = dt.Rows[j]["Invoice Date"].ToString().Trim();
                                if (strInvDate != "")
                                {
                                    string strInvoiceDate = Convert.ToDateTime(strInvDate).ToString("dd-MM-yyyy");
                                    InvoiceDate = Convert.ToDateTime(strInvoiceDate);
                                }
                                string InvoiceType = dt.Rows[j]["Invoice Type"].ToString().Trim();
                                string Description = dt.Rows[j]["Description"].ToString().Trim();
                                string ItemType = dt.Rows[j]["Item Type"].ToString().Trim();

                                string InvoiceValues = dt.Rows[j]["Invoice Value"].ToString().Trim();
                                if (InvoiceValues != "")
                                {
                                    InvoiceValue = Convert.ToDecimal(InvoiceValues);
                                }
                                string Qty1 = dt.Rows[j]["Qty"].ToString().Trim();
                                if (Qty1 != "")
                                {
                                    Quantity = Convert.ToDecimal(Qty1);
                                }
                                if (RequestType == 3)  // 3-DTA Sales && 1-BOE Details
                                {
                                    GoodsMeasureUnit = Convert.ToString(dt.Rows[j]["Goods measurement Unit"]).ToUpper().Trim();
                                }
                                else if (RequestType == 1)  // 3-DTA Sales && 1-BOE Details
                                {
                                    //GoodsMeasureUnit = Convert.ToString(dt.Rows[j]["Item measurement Unit"]).ToUpper().Trim();
                                    SupplierName = Convert.ToString(dt.Rows[j]["Supplier  Name"]).ToUpper().Trim();
                                }

                                string ItemPrice1 = dt.Rows[j]["Item Price_"].ToString().Trim();
                                if (ItemPrice1 != "")
                                {
                                    ItemPrice = Convert.ToDecimal(ItemPrice1);
                                }
                                string Productvalue1 = dt.Rows[j]["Product Value"].ToString().Trim();
                                if (Productvalue1 != "")
                                {
                                    Productvalue = Convert.ToDecimal(Productvalue1);
                                }
                                string CTH1 = dt.Rows[j]["CTH"].ToString().Trim();
                                if (CTH1 != "")
                                {
                                    CTH = Convert.ToDecimal(CTH1);
                                }
                                string ExRate1 = dt.Rows[j]["Exchange Rate"].ToString().Trim();
                                if (ExRate1 != "")
                                {
                                    ExRate = Convert.ToDecimal(ExRate1);
                                }
                                string strDiscount = Convert.ToString(dt.Rows[j]["Discount Applicable"]).Trim();
                                if (strDiscount != "")
                                {
                                    if (strDiscount.ToLower() == "no")
                                    {
                                        Discount = false;
                                    }
                                    else if (strDiscount.ToLower() == "yes")
                                    {
                                        Discount = true;
                                    }
                                }
                                string strReImport = Convert.ToString(dt.Rows[j]["Re- Import"]).Trim();
                                if (strReImport != "")
                                {
                                    if (strReImport.ToLower() == "no")
                                    {
                                        ReImport = false;
                                    }
                                    else if (strReImport.ToLower() == "yes")
                                    {
                                        ReImport = true;
                                    }
                                }
                                string strPrevImport = Convert.ToString(dt.Rows[j]["Previous Import"]).Trim();
                                if (strPrevImport != "")
                                {
                                    if (strPrevImport.ToLower() == "no")
                                    {
                                        PrevImport = false;
                                    }
                                    else if (strPrevImport.ToLower() == "yes")
                                    {
                                        PrevImport = true;
                                    }
                                }
                                string Currency = Convert.ToString(dt.Rows[j]["Currency"]).Trim();
                                int Mode = 0;
                                string AssesVal = Convert.ToString(dt.Rows[j]["Assessable value"]).Trim();
                                if (AssesVal != "")
                                {
                                    strAssesableVal = Convert.ToDecimal(AssesVal);
                                }
                                string CIFVa = Convert.ToString(dt.Rows[j]["CIF Value in INR"]).Trim();
                                if (CIFVa != "")
                                {
                                    strCIFVal = Convert.ToDecimal(CIFVa);
                                }
                                int ResultOut = 0;
                                ResultOut = SEZOperation.AddSEZExcelJobDetail(JobRefNo, SEZType,
                                            Convert.ToInt32(ddlCustomer.SelectedValue), Convert.ToInt32(ddlDivision.SelectedValue),
                                            Convert.ToInt32(ddlPlant.SelectedValue), Convert.ToInt32(ddlRequestType.SelectedValue),
                                            strReqID, strBENo, BEDate, Mode, ExRate, GoodsMeasureUnit, Discount, ReImport, PrevImport, Currency, SupplierName,
                                            strAssesableVal, strCIFVal, LoggedInUser.glFinYearId, LoggedInUser.glUserId, LoggedInUser.glUserId);

                                SuccessCnt++;

                                //if (ResultOut == 1)
                                //{
                                int JobId = 0;
                                //DataSet dsJobId = new DataSet();
                                //dsJobId = SEZOperation.GetJobIdFromJobRefNo(JobRefNo);
                                //JobId = Convert.ToInt32(dsJobId.Tables[0].Rows[0]["lid"]);
                                //if (JobId != 0)
                                //{
                                int ResultInv = 0;
                                ResultInv = SEZOperation.AddSEZExcelInvoice(JobId, JobRefNo, strReqID, SEZType, strInvoiceNo, InvoiceDate,
                                            InvoiceType, InvoiceValue, Description, Quantity, ItemPrice, Productvalue, CTH, ItemType, LoggedInUser.glUserId);


                                lblError.Text = "Successfully Added SEZ Job Detail..!!";
                                lblError.CssClass = "success";
                                btnSubmit.Visible = false;
                                btnNew.Visible = true;
                                //}
                                //}
                                //else
                                //{
                                //    //lblError.Text = "System Error. Please try again later..!!";
                                //    //lblError.CssClass = "errorMsg";
                                //}
                            }  // END RequestType == 3
                            else if (RequestType == 5)  // Request Type - DTAP
                            {
                                //strReqID1 = Convert.ToInt64(dt.Rows[j]["Request ID"].ToString().Trim()); //ToString().Trim();
                                strReqID1 = dt.Rows[j]["Request id"].ToString().Trim(); //ToString().Trim();
                                string strReqID = dt.Rows[j]["Request ID"].ToString().Trim(); //ToString().Trim();
                                strBENo = dt.Rows[j]["DTA Procurement No_"].ToString().Trim();
                                string strBEDate = Convert.ToDateTime(dt.Rows[j]["DTA Procurement No Date & Time_"]).ToString("dd-MM-yyyy");
                                if (strBEDate != "")
                                {
                                    BEDate = Convert.ToDateTime(strBEDate);
                                }
                                string strInvoiceNo = dt.Rows[j]["Invoice Number"].ToString().Trim();

                                string strInvDate = dt.Rows[j]["Invoice date"].ToString().Trim();
                                if (strInvDate != "")
                                {
                                    string strInvoiceDate = Convert.ToDateTime(strInvDate).ToString("dd-MM-yyyy");
                                    InvoiceDate = Convert.ToDateTime(strInvoiceDate);
                                }
                                string InvoiceCurrency = dt.Rows[j]["Invoice Currency"].ToString().Trim();
                                string InvoiceType = "";// dt.Rows[j]["Invoice Type"].ToString().Trim(); //
                                string ExRate1 = dt.Rows[j]["Exchange Rate"].ToString().Trim();
                                if (ExRate1 != "")
                                {
                                    ExRate = Convert.ToDecimal(ExRate1);
                                }
                                string Description = dt.Rows[j]["Item Description 1_"].ToString().Trim();
                                string ItemType = dt.Rows[j]["Item Type___"].ToString().Trim();

                                string CTH1 = dt.Rows[j]["RITC/ITCHS Number"].ToString().Trim();
                                if (CTH1 != "")
                                {
                                    CTH = Convert.ToDecimal(CTH1);
                                }
                                string Qty1 = dt.Rows[j]["Quantity_"].ToString().Trim();
                                if (Qty1 != "")
                                {
                                    Quantity = Convert.ToDecimal(Qty1);
                                }

                                string ItemPrice1 = dt.Rows[j]["Unit Price"].ToString().Trim();
                                if (ItemPrice1 != "")
                                {
                                    ItemPrice = Convert.ToDecimal(ItemPrice1);
                                }
                                string Productvalue1 = dt.Rows[j]["Product Value"].ToString().Trim();
                                if (Productvalue1 != "")
                                {
                                    Productvalue = Convert.ToDecimal(Productvalue1);
                                }
                                string DutyAmount = dt.Rows[j]["Total Duty Amount(as per ARE-1)"].ToString().Trim();
                                if (DutyAmount != "")
                                {
                                    DutyAmnt = Convert.ToDecimal(DutyAmount);
                                }

                                int ResultOut = 0;
                                ResultOut = SEZOperation.AddSEZExcelDTAPJobDetail(JobRefNo, SEZType,
                                            Convert.ToInt32(ddlCustomer.SelectedValue), Convert.ToInt32(ddlDivision.SelectedValue),
                                            Convert.ToInt32(ddlPlant.SelectedValue), Convert.ToInt32(ddlRequestType.SelectedValue),
                                            strReqID, strBENo, BEDate, ExRate, InvoiceCurrency,
                                            DutyAmnt, LoggedInUser.glFinYearId, LoggedInUser.glUserId, LoggedInUser.glUserId);

                                SuccessCnt++;

                                //if (ResultOut == 1)
                                //{
                                

                                int JobId = 0;
                                //DataSet dsJobId = new DataSet();
                                //dsJobId = SEZOperation.GetJobIdFromJobRefNo(JobRefNo);
                                //JobId = Convert.ToInt32(dsJobId.Tables[0].Rows[0]["lid"]);
                                //if (JobId != 0)
                                //{
                                int ResultInv = 0;
                                ResultInv = SEZOperation.AddSEZExcelInvoice(JobId, JobRefNo, strReqID, SEZType, strInvoiceNo, InvoiceDate,
                                            InvoiceType, InvoiceValue, Description, Quantity, ItemPrice, Productvalue, CTH, ItemType, LoggedInUser.glUserId);

                                lblError.Text = "Successfully Added SEZ Job Detail..!!";
                                lblError.CssClass = "success";
                                btnSubmit.Visible = false;
                                btnNew.Visible = true;
                                //}
                                //}
                                //else
                                //{
                                //    //lblError.Text = "System Error. Please try again later..!!";
                                //    //lblError.CssClass = "errorMsg";
                                //}

                            } // END RequestType == 5

                            else if (RequestType == 2)  // Request Type - Shipping Bill
                            {
                                //strReqID1 = Convert.ToInt64(dt.Rows[j]["Request ID"].ToString().Trim()); //ToString().Trim();
                                strReqID1 = dt.Rows[j]["Request id"].ToString().Trim(); //ToString().Trim();
                                string strReqID = dt.Rows[j]["Request ID"].ToString().Trim(); //ToString().Trim();
                             
                                strBENo = dt.Rows[j]["Thoka no"].ToString().Trim();
                                string strBEDate = Convert.ToDateTime(dt.Rows[j]["Thoka No# Date & time"]).ToString("dd-MM-yyyy");

                                if (strBEDate != "")
                                {
                                    BEDate = Convert.ToDateTime(strBEDate);
                                }
                                string InvoiceType = dt.Rows[j]["Invoice Type"].ToString().Trim();

                                string strInvoiceNo = dt.Rows[j]["Invoice Number"].ToString().Trim();

                                string strInvDate = dt.Rows[j]["Invoice date"].ToString().Trim();
                                if (strInvDate != "")
                                {
                                    string strInvoiceDate = Convert.ToDateTime(strInvDate).ToString("dd-MM-yyyy");
                                    InvoiceDate = Convert.ToDateTime(strInvoiceDate);
                                }
                                string InvoiceCurrency = dt.Rows[j]["Currency"].ToString().Trim();
                                
                                string ExRate1 = dt.Rows[j]["Exchange Rate"].ToString().Trim();
                                if (ExRate1 != "")
                                {
                                    ExRate = Convert.ToDecimal(ExRate1);
                                }

                                string BuyerName= dt.Rows[j]["Buyer Name"].ToString().Trim();

                                string Description = dt.Rows[j]["Item Description 1"].ToString().Trim();
                                string ItemType = dt.Rows[j]["Item Type"].ToString().Trim();

                                string CTH1 = dt.Rows[j]["CTH No#"].ToString().Trim();
                                if (CTH1 != "")
                                {
                                    CTH = Convert.ToDecimal(CTH1);
                                }
                                string Qty1 = dt.Rows[j]["Qty"].ToString().Trim();
                                if (Qty1 != "")
                                {
                                    Quantity = Convert.ToDecimal(Qty1);
                                }

                                GoodsMeasureUnit = Convert.ToString(dt.Rows[j]["Unit of Measurement"]).ToUpper().Trim();
                                string ItemPrice1 = dt.Rows[j]["Item Price"].ToString().Trim();
                                if (ItemPrice1 != "")
                                {
                                    ItemPrice = Convert.ToDecimal(ItemPrice1);
                                }
                                string Productvalue1 = dt.Rows[j]["Product Value"].ToString().Trim();
                                if (Productvalue1 != "")
                                {
                                    Productvalue = Convert.ToDecimal(Productvalue1);
                                }

                                SchemeCode = dt.Rows[j]["Scheme Code"].ToString().Trim();

                                string strExpGoods = Convert.ToString(dt.Rows[j]["Previous export of identical goods/similar goods"]).Trim();
                                if (strExpGoods != "")
                                {
                                    if (strExpGoods.ToLower() == "no")
                                    {
                                        PrevExpGoods = false;
                                    }
                                    else if (strExpGoods.ToLower() == "yes")
                                    {
                                        PrevExpGoods = true;
                                    }
                                }
                                string strCessDetail = Convert.ToString(dt.Rows[j]["Cess Details"]).Trim();
                                if (strCessDetail != "")
                                {
                                    if (strCessDetail.ToLower() == "no")
                                    {
                                        CessDetail = false;
                                    }
                                    else if (strCessDetail.ToLower() == "yes")
                                    {
                                        CessDetail = true;
                                    }
                                }
                                string strLicenceNo = Convert.ToString(dt.Rows[j]["Licence/Registration No"]).Trim();
                                if (strLicenceNo != "")
                                {
                                    if (strLicenceNo.ToLower() == "no")
                                    {
                                        LicenceRegNo = false;
                                    }
                                    else if (strLicenceNo.ToLower() == "yes")
                                    {
                                        LicenceRegNo = true;
                                    }
                                }
                                string strReExport = Convert.ToString(dt.Rows[j]["Re-Export"]).Trim();
                                if (strReExport != "")
                                {
                                    if (strReExport.ToLower() == "no")
                                    {
                                        ReExport = false;
                                    }
                                    else if (strReExport.ToLower() == "yes")
                                    {
                                        ReExport = true;
                                    }
                                }
                                string strPrevExport = Convert.ToString(dt.Rows[j]["Previous Export"]).Trim();
                                if (strPrevExport != "")
                                {
                                    if (strPrevExport.ToLower() == "no")
                                    {
                                        PrevExport = false;
                                    }
                                    else if (strPrevExport.ToLower() == "yes")
                                    {
                                        PrevExport = true;
                                    }
                                }

                                int ResultOut = 0;
                                ResultOut = SEZOperation.AddSEZExcelShippingBillJobDetail(JobRefNo, SEZType,
                                            Convert.ToInt32(ddlCustomer.SelectedValue), Convert.ToInt32(ddlDivision.SelectedValue),
                                            Convert.ToInt32(ddlPlant.SelectedValue), Convert.ToInt32(ddlRequestType.SelectedValue),
                                            strReqID, strBENo, BEDate, ExRate, InvoiceCurrency, BuyerName, GoodsMeasureUnit,
                                            SchemeCode, PrevExpGoods, CessDetail, LicenceRegNo, ReExport, PrevExport,
                                            LoggedInUser.glFinYearId, LoggedInUser.glUserId, LoggedInUser.glUserId);

                                SuccessCnt++;

                                int JobId = 0;                            
                                int ResultInv = 0;
                                ResultInv = SEZOperation.AddSEZExcelInvoice(JobId, JobRefNo, strReqID, SEZType, strInvoiceNo, InvoiceDate,
                                            InvoiceType, InvoiceValue, Description, Quantity, ItemPrice, Productvalue, CTH, ItemType, LoggedInUser.glUserId);

                                lblError.Text = "Successfully Added SEZ Job Detail..!!";
                                lblError.CssClass = "success";
                                btnSubmit.Visible = false;
                                btnNew.Visible = true;                             

                            } // END RequestType == 2 Shipping Bill

                        }  //  if (JobRefNo != "") END 
                    }
                    catch (Exception e)
                    {
                        //if (RequestType == 3)  // Request Type - DTA Sales
                        //{
                        //if (!ExcelErrorList.ContainsKey(strReqID1))
                        //{
                        //    ExcelErrorList.Add(strReqID1, strBENo);
                        //    ErrorCnt++;
                        //}
                        //}
                        //if (RequestType == 3)  // Request Type - DTAP
                        //{
                        //}

                        if (!ExcelErrorList.ContainsValue(strReqID1))
                        {
                            ExcelErrorList.Add(ErrorCnt, strBENo);
                            ErrorCnt++;
                        }


                    }

                    lblError.Text = "Please Check your excel Column Name OR Data, Missing Someone Rows";
                    lblError.CssClass = "errorMsg";
                }

                

                lblError.Text = "Successfully Added SEZ Job Detail..!! </br> Total Count = " + TotalCnt + "  &nbsp;&nbsp;  Error Count = " + ErrorCnt + "  &nbsp;&nbsp;  Success Count = " + SuccessCnt + " &nbsp;&nbsp; ";
                lblError.CssClass = "success";
            }
            //}
            //catch
            //{
            //    lblError.Text = "Please Check your excel Column Name";
            //    lblError.CssClass = "errorMsg";
            //}


            string Result = SEZOperation.GetGenerateSEZJobNo(SEZezType);
            if (Result != "")
            {
                txtJobNo.Text = Result;
            }

            DataTable dtError = new DataTable();

            //dtError.Columns.Add(new DataColumn("SrNo", typeof(long)));
            dtError.Columns.Add(new DataColumn("RequestID", typeof(long)));
            dtError.Columns.Add(new DataColumn("ThokaNo", typeof(string)));

            foreach (KeyValuePair<long, string> item in ExcelErrorList)
            {
                DataRow dtRow = null;
                dtRow = dtError.NewRow();

                //dtRow["SrNo"] = ErrorCnt;
                dtRow["RequestID"] = item.Key;
                dtRow["ThokaNo"] = item.Value;

                dtError.Rows.Add(dtRow);

                GrdError.DataSource = dtError;
                GrdError.DataBind();
            }

            if (ErrorCnt != 0)
            {
                ModalPopupExtender1.Show();
            }

            lblExcelError.Text = "Total Count = " + TotalCnt + " &nbsp;&nbsp;    Error Count = " + ErrorCnt + "  &nbsp;&nbsp;   Success Count = " + SuccessCnt + " ";
            lblExcelError.CssClass = "errorMsg";

        }
        else
        {
            lblError.Text = "Please Select SEZ Type";
            lblError.CssClass = "errorMsg";
        }
        //}
        //catch
        //{
        //    lblError.Text = "Your Excel Is Not Proper Format";
        //    lblError.CssClass = "errorMsg";
        //}            

    }



    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";

        int CustomerId = Convert.ToInt32(ddlCustomer.SelectedValue);

        if (CustomerId > 0)
        {
            DBOperations.FillCustomerDivision(ddlDivision, CustomerId);
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddlPlant.Items.Clear();
            ddlPlant.Items.Add(lstSelect);
        }
        else
        {
            ddlDivision.SelectedValue = "0";
            ddlPlant.SelectedValue = "0";
        }

    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddlDivision.SelectedValue);

        if (DivisonId > 0)
        {
            DBOperations.FillCustomerPlant(ddlPlant, DivisonId);
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddlPlant.Items.Clear();
            ddlPlant.Items.Add(lstSelect);
        }
    }


    #endregion 



    #region Container Details

    // ----------- For  Container Details
    private void SetPreviousCantainerData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable1"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable1"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox TxtCantainerNO = (TextBox)GrvCantainerDetail.Rows[rowIndex].Cells[1].FindControl("TxtCantainerNO");
                    DropDownList ddlSize1 = (DropDownList)GrvCantainerDetail.Rows[rowIndex].Cells[2].FindControl("ddlSize");
                    DropDownList ddlType = (DropDownList)GrvCantainerDetail.Rows[rowIndex].Cells[3].FindControl("ddlCantainerType");

                    TxtCantainerNO.Text = dt.Rows[i]["ColCantainerNO"].ToString();
                    ddlSize1.SelectedValue = dt.Rows[i]["ColSize"].ToString();
                    ddlType.SelectedValue = dt.Rows[i]["ColCantainerType"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    private void SetInitialCantainerRow()
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ColCantainerNO", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSize", typeof(string)));
        dt.Columns.Add(new DataColumn("ColCantainerType", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["ColCantainerNO"] = string.Empty;
        dr["ColSize"] = string.Empty;
        dr["ColCantainerType"] = string.Empty;

        dt.Rows.Add(dr);
        ViewState["CurrentTable1"] = dt;
        GrvCantainerDetail.DataSource = dt;
        GrvCantainerDetail.DataBind();
    }

    private void AddNewRowToCantainerGrid()
    {
        lblError.Text = "";
        int rowIndex = 0;
        if (ViewState["CurrentTable1"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable1"];
            DataRow drCurrentRow1 = null;

            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    TextBox TxtCantainerNO = (TextBox)GrvCantainerDetail.Rows[rowIndex].Cells[1].FindControl("TxtCantainerNO");
                    DropDownList ddlSize = (DropDownList)GrvCantainerDetail.Rows[rowIndex].Cells[2].FindControl("ddlSize");
                    DropDownList ddlCantainerType = (DropDownList)GrvCantainerDetail.Rows[rowIndex].Cells[3].FindControl("ddlCantainerType");

                    drCurrentRow1 = dt.NewRow();
                    drCurrentRow1["RowNumber"] = i + 1;
                    dt.Rows[i - 1]["ColCantainerNO"] = TxtCantainerNO.Text;
                    dt.Rows[i - 1]["ColSize"] = ddlSize.SelectedValue;
                    dt.Rows[i - 1]["ColCantainerType"] = ddlCantainerType.SelectedValue;

                    rowIndex++;
                }
                dt.Rows.Add(drCurrentRow1);
                ViewState["CurrentTable1"] = dt;
                GrvCantainerDetail.DataSource = dt;
                GrvCantainerDetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousCantainerData();
    }

    protected void btnContainerDetail_Click(object sender, EventArgs e)
    {
        ModalPopupContainer.Show();
    }

    protected void btnContainer_Click(object sender, EventArgs e)
    {
        AddNewRowToCantainerGrid();
        ModalPopupContainer.Show();
    }

    // ----------------------------------

    #endregion

    protected void txtRemainingQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal ProductValue = 0, ItemPrice1 = 0, Qty = 0, NewQty = 0;
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)thisTextBox.Parent.Parent;
            int rowindex = 0;
            rowindex = currentRow.RowIndex;

            // following line will get the label value in current row   

            TextBox txtQty = (TextBox)currentRow.FindControl("txtQuantity");
            TextBox txtQty2 = (TextBox)currentRow.FindControl("txtRemainingQty");
            TextBox txtPrice = (TextBox)currentRow.FindControl("txtItemPrice");
            TextBox ProdValue = (TextBox)currentRow.FindControl("txtProductVal");

             Qty = Convert.ToDecimal(txtQty.Text.Trim());
             NewQty = Convert.ToDecimal(txtQty2.Text.Trim());
            if (txtPrice.Text.Trim() != "")
            {
                ItemPrice1 = Convert.ToDecimal(txtPrice.Text.Trim());
            }             

            if (Qty < NewQty)
            {
                txtQty2.Text = "";
                txtPrice.Text = "";
                lblError.Text = "New Quantity is always Less Than Total Quantity ";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                if(txtPrice.Text!="" || txtPrice.Text != null)
                {
                    ProductValue = NewQty * ItemPrice1;
                    ProdValue.Text = Convert.ToString(ProductValue);
                }
                lblError.Text = "";
            }
        }
        catch
        {
            //Response.Redirect("error.aspx");
        }
    }

    protected void txtItemPrice_TextChanged(object sender, EventArgs e)
    {
        decimal ProductValue = 0, TotQty = 0, ItemPrice1 = 0, RemQty = 0;
        try
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)thisTextBox.Parent.Parent;

            TextBox TotalQuantity = (TextBox)currentRow.FindControl("txtQuantity");
            TextBox RemQuantity = (TextBox)currentRow.FindControl("txtRemainingQty");
            TextBox ItemPrice = (TextBox)currentRow.FindControl("txtItemPrice");
            TextBox ProdValue = (TextBox)currentRow.FindControl("txtProductVal");
                        
            if (TotalQuantity.Text !="")
            {
                TotQty = Convert.ToDecimal(TotalQuantity.Text.Trim());
            }

            if (ItemPrice.Text != "")
            {
                 ItemPrice1 = Convert.ToDecimal(ItemPrice.Text.Trim());
            }

            if (rdbSEZtype.SelectedValue == "2")
            {
                if (txtInwardJobNo.Text != "")
                {                    
                    if (RemQuantity.Text != "")
                    {
                        RemQty = Convert.ToDecimal(RemQuantity.Text.Trim());
                        ProductValue = RemQty * ItemPrice1;
                    }
                }
                else
                {
                    ProductValue = TotQty * ItemPrice1;
                }
            }
            else if (rdbSEZtype.SelectedValue == "1")
            {
                ProductValue = TotQty * ItemPrice1;
            }

            ProdValue.Text = Convert.ToString(ProductValue);            
        }
        catch
        {
            Response.Redirect("error.aspx");
        }
    }

    protected void ddlRequestType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int ReqTypeId = Convert.ToInt32(ddlIndiviReqType.SelectedValue);

        if (ReqTypeId == 3 || ReqTypeId == 1)  // DTA Sales OR BOE
        {
            trCIF.Visible = true;
            trDiscountAppl.Visible = true;
            trDutyAmnt.Visible = false;
            trSB2.Visible = false;
            trSB1.Visible = false;
            trSB.Visible = false;
            txtDutyAmount.Text = "";
           

            txtBuyerName.Text = "";
            txtSchemeCode.Text = "";
            rdlPrevExpGoods.SelectedValue = "2";
            rdlCessDetail.SelectedValue = "2";
            rdlLicenceRegNo.SelectedValue = "2";
            rdlReExport.SelectedValue = "2";
            rdlPrevExport.SelectedValue = "2";

            if (ReqTypeId == 3 )  // DTA Sales 
            {
                trBOE.Visible = false;
                txtSupplierName.Text = "";
            }
            else if (ReqTypeId == 1)  // BOE
            {
                trBOE.Visible = true;
            }
        }
        else if (ReqTypeId == 5)  // DTAP
        {
            trCIF.Visible = false;
            trDiscountAppl.Visible = false;
            trSB2.Visible = false;
            trSB1.Visible = false;
            trSB.Visible = false;
            trDutyAmnt.Visible = true;
            trBOE.Visible = false;

            txtSupplierName.Text = "";
            txtBuyerName.Text = "";
            txtSchemeCode.Text = "";
            rdlPrevExpGoods.SelectedValue = "2";
            rdlCessDetail.SelectedValue = "2";
            rdlLicenceRegNo.SelectedValue = "2";
            rdlReExport.SelectedValue = "2";
            rdlPrevExport.SelectedValue = "2";

            txtCIFValue.Text = "";
            ddlGrossWt.SelectedIndex = 0;
            rdlDiscountAppli.SelectedValue = "2";
            rdlPreviousImport.SelectedValue = "2";
            rdlReImport.SelectedValue = "2";
        }

        else if (ReqTypeId == 2)   // Shipping Bill
        {
            trCIF.Visible = false;
            trDiscountAppl.Visible = false;
            trDutyAmnt.Visible = false;
            trBOE.Visible = false;

            txtDutyAmount.Text = "";
            txtSupplierName.Text = "";

            trSB2.Visible = true;
            trSB1.Visible = true;
            trSB.Visible = true;

            txtCIFValue.Text = "";
            ddlGrossWt.SelectedIndex = 0;
            rdlDiscountAppli.SelectedValue = "2";
            rdlPreviousImport.SelectedValue = "2";
            rdlReImport.SelectedValue = "2";
        }

        //string message = ddlFruits.SelectedItem.Text + " - " + ddlFruits.SelectedItem.Value;
        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
    }
}


