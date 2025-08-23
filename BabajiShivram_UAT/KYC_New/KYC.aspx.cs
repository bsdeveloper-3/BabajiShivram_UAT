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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System.Security.Cryptography;


public partial class KYC_New_KYC : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    DateTime dtClose = DateTime.MinValue;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnAddGSTDetail);
        ScriptManager1.RegisterPostBackControl(btnAddMaterial);
        ScriptManager1.RegisterPostBackControl(btnAddServices);
        ScriptManager1.RegisterPostBackControl(btnSaveKYC);
        //ScriptManager1.RegisterPostBackControl(btnOversea_SaveKYC);

        if (!IsPostBack)
        {
            hdnEnquiryId.Value =  Convert.ToString(Session["EnqiryId"]);
            //txtGeneral_CompanyName.Text = hdnEnquiryId.Value;
            if (hdnEnquiryId.Value != "" && hdnEnquiryId.Value != "0")
            {
                DataSet dsGetEnquiryByLid = KYCOperation.GetEnquiryByLid(Convert.ToInt32(hdnEnquiryId.Value));
                if (dsGetEnquiryByLid != null && dsGetEnquiryByLid.Tables[0].Rows.Count > 0)
                {
                    if (dsGetEnquiryByLid.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                    {
                        txtGeneral_CompanyName.Text = dsGetEnquiryByLid.Tables[0].Rows[0]["CompanyName"].ToString();
                        txtGeneral_CompanyName.Enabled = false;
                        txtGeneral_Email.Text = dsGetEnquiryByLid.Tables[0].Rows[0]["LeadEmail"].ToString();
                        txtGeneral_CorporateAddress1.Text = dsGetEnquiryByLid.Tables[0].Rows[0]["LeadAddress"].ToString();
                        txtGeneral_WebsiteAdd.Text = dsGetEnquiryByLid.Tables[0].Rows[0]["LeadWebsite"].ToString();
                        if (dsGetEnquiryByLid.Tables[0].Rows[0]["BusinessCategoryID"] != DBNull.Value)
                            ddlNatureOfBusiness.SelectedValue = dsGetEnquiryByLid.Tables[0].Rows[0]["BusinessCategoryID"].ToString();
                        if (dsGetEnquiryByLid.Tables[0].Rows[0]["SectorID"] != DBNull.Value)
                            ddlSector.SelectedValue = dsGetEnquiryByLid.Tables[0].Rows[0]["SectorID"].ToString();
                    }
                }
            }
            ddlGeneral_State.DataBind();
            ddlGeneral_State_SelectedIndexChanged(null, EventArgs.Empty);
            SetInitiatRow_GSTDetail();
            SetInitiatRow_Material();
            SetInitiatRow_Services();
        }
    }

    protected void ddlGeneral_State_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGeneral_State.SelectedValue != "0")
        {
            DataSet dsGetCountryId = KYCOperation.GetCountryByState(Convert.ToInt32(ddlGeneral_State.SelectedValue));
            if (dsGetCountryId != null && dsGetCountryId.Tables[0].Rows.Count > 0)
            {
                //ddlCountry.DataBind();
                ddlCountry.SelectedValue = dsGetCountryId.Tables[0].Rows[0]["CountryId"].ToString();
            }
        }
    }

    #region GST DETAILS

    protected void lvGSTDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "remove")
        {
            int rowIndex = Convert.ToInt32(e.Item.DataItemIndex);
            if (rowIndex > 0)
            {
                int listcount = lvGSTDetails.Items.Count;
                if (listcount - 1 == rowIndex)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["GSTDetails"];
                    lvGSTDetails.Items.RemoveAt(rowIndex);
                    dtCurrentTable.Rows[rowIndex].Delete();
                    dtCurrentTable.AcceptChanges();
                    ViewState["GSTDetails"] = dtCurrentTable;
                    lvGSTDetails.DataSource = dtCurrentTable;
                    lvGSTDetails.DataBind();
                    SetPreviousRow_GSTDetail();
                }
            }
        }
    }

    protected void lvGSTDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            TextBox txtGST_CompanyName = (TextBox)e.Item.FindControl("txtGST_CompanyName");
            DropDownList ddlGST_Branch = (DropDownList)e.Item.FindControl("ddlGST_Branch");
            TextBox txtGST_Address = (TextBox)e.Item.FindControl("txtGST_Address");
            TextBox txtGST_ContactPerson = (TextBox)e.Item.FindControl("txtGST_ContactPerson");
            TextBox txtGST_ContactNo = (TextBox)e.Item.FindControl("txtGST_ContactNo");
            TextBox txtGST_ProvisionId = (TextBox)e.Item.FindControl("txtGST_ProvisionId");
            TextBox txtGST_Email = (TextBox)e.Item.FindControl("txtGST_Email");
            TextBox txtGST_ArnNo = (TextBox)e.Item.FindControl("txtGST_ArnNo");

            if (txtGST_CompanyName != null)
            {
                txtGST_CompanyName.Attributes.Add("onchange", "GSTRequireFields('" + txtGST_CompanyName.ClientID + "','" + ddlGST_Branch.ClientID +
                                                    "','" + txtGST_Address.ClientID + "','" + txtGST_ContactPerson.ClientID + "','" + txtGST_ContactNo.ClientID +
                                                    "','" + txtGST_ProvisionId.ClientID + "');");
            }

            if (txtGST_Email != null)
                txtGST_Email.Attributes.Add("onchange", "ValidateGST('" + txtGST_Email.ClientID + "', 1);");

            if (txtGST_ProvisionId != null)
                txtGST_ProvisionId.Attributes.Add("onchange", "ValidateGST('" + txtGST_ProvisionId.ClientID + "', 2);");

            if (txtGST_ArnNo != null)
                txtGST_ArnNo.Attributes.Add("onchange", "ValidateGST('" + txtGST_ArnNo.ClientID + "', 3);");
        }
    }

    protected void btnAddGSTDetail_OnClick(object sender, EventArgs e)
    {
        for (int i = 1; i < 4; i++)
        {
            AddNewRow_GSTDetail();
        }
    }

    private void SetInitiatRow_GSTDetail()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("CompanyName", typeof(string)));
        dt.Columns.Add(new DataColumn("StateBranch", typeof(string)));
        dt.Columns.Add(new DataColumn("Address", typeof(string)));
        dt.Columns.Add(new DataColumn("PersonName", typeof(string)));
        dt.Columns.Add(new DataColumn("PersonNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Email", typeof(string)));
        dt.Columns.Add(new DataColumn("ProvisonID", typeof(string)));
        dt.Columns.Add(new DataColumn("ARNNo", typeof(string)));

        for (int i = 1; i < 5; i++)
        {
            dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["CompanyName"] = string.Empty;
            dr["StateBranch"] = string.Empty;
            dr["Address"] = string.Empty;
            dr["PersonName"] = string.Empty;
            dr["PersonNumber"] = string.Empty;
            dr["Email"] = string.Empty;
            dr["ProvisonID"] = string.Empty;
            dr["ARNNo"] = string.Empty;
            dt.Rows.Add(dr);
        }

        ViewState["GSTDetails"] = dt;
        lvGSTDetails.DataSource = dt;
        lvGSTDetails.DataBind();
    }

    private void AddNewRow_GSTDetail()
    {
        int rowIndex = 0;
        if (ViewState["GSTDetails"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["GSTDetails"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox txtGST_CompanyName = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_CompanyName");
                    DropDownList ddlGST_Branch = (DropDownList)lvGSTDetails.Items[rowIndex].FindControl("ddlGST_Branch");
                    TextBox txtGST_Address = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_Address");
                    TextBox txtGST_ContactPerson = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ContactPerson");
                    TextBox txtGST_ContactNo = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ContactNo");
                    TextBox txtGST_Email = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_Email");
                    TextBox txtGST_ProvisionId = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ProvisionId");
                    TextBox txtGST_ArnNo = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ArnNo");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["CompanyName"] = txtGST_CompanyName.Text;
                    dtCurrentTable.Rows[i - 1]["StateBranch"] = ddlGST_Branch.SelectedValue;
                    dtCurrentTable.Rows[i - 1]["Address"] = txtGST_Address.Text;
                    dtCurrentTable.Rows[i - 1]["PersonName"] = txtGST_ContactPerson.Text;
                    dtCurrentTable.Rows[i - 1]["PersonNumber"] = txtGST_ContactNo.Text;
                    dtCurrentTable.Rows[i - 1]["Email"] = txtGST_Email.Text;
                    dtCurrentTable.Rows[i - 1]["ProvisonID"] = txtGST_ProvisionId.Text;
                    dtCurrentTable.Rows[i - 1]["ARNNo"] = txtGST_ArnNo.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["GSTDetails"] = dtCurrentTable;
                lvGSTDetails.DataSource = dtCurrentTable;
                lvGSTDetails.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousRow_GSTDetail();
    }

    private void SetPreviousRow_GSTDetail()
    {
        int rowIndex = 0;
        if (ViewState["GSTDetails"] != null)
        {
            DataTable dt = (DataTable)ViewState["GSTDetails"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtGST_CompanyName = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_CompanyName");
                    DropDownList ddlGST_Branch = (DropDownList)lvGSTDetails.Items[rowIndex].FindControl("ddlGST_Branch");
                    TextBox txtGST_Address = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_Address");
                    TextBox txtGST_ContactPerson = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ContactPerson");
                    TextBox txtGST_ContactNo = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ContactNo");
                    TextBox txtGST_Email = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_Email");
                    TextBox txtGST_ProvisionId = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ProvisionId");
                    TextBox txtGST_ArnNo = (TextBox)lvGSTDetails.Items[rowIndex].FindControl("txtGST_ArnNo");

                    txtGST_CompanyName.Text = dt.Rows[i]["CompanyName"].ToString();
                    ddlGST_Branch.SelectedValue = dt.Rows[i]["StateBranch"].ToString();
                    txtGST_Address.Text = dt.Rows[i]["Address"].ToString();
                    txtGST_ContactPerson.Text = dt.Rows[i]["PersonName"].ToString();
                    txtGST_ContactNo.Text = dt.Rows[i]["PersonNumber"].ToString();
                    txtGST_Email.Text = dt.Rows[i]["Email"].ToString();
                    txtGST_ProvisionId.Text = dt.Rows[i]["ProvisonID"].ToString();
                    txtGST_ArnNo.Text = dt.Rows[i]["ARNNo"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    #endregion

    #region MATERIALS

    protected void btnAddMaterial_OnClick(object sender, EventArgs e)
    {
        for (int i = 1; i < 6; i++)
        {
            AddNewRow_Material();
        }
    }

    private void SetInitiatRow_Material()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("MaterialSupplied", typeof(string)));
        dt.Columns.Add(new DataColumn("CommodityName", typeof(string)));
        dt.Columns.Add(new DataColumn("HSNCode", typeof(string)));

        for (int i = 1; i < 6; i++)
        {
            dr = dt.NewRow();
            dr["RowNumber"] = i;
            dr["MaterialSupplied"] = string.Empty;
            dr["CommodityName"] = string.Empty;
            dr["HSNCode"] = string.Empty;
            dt.Rows.Add(dr);
        }

        ViewState["MaterialDetails"] = dt;
        gvMaterial.DataSource = dt;
        gvMaterial.DataBind();
    }

    private void AddNewRow_Material()
    {
        int rowIndex = 0;
        if (ViewState["MaterialDetails"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["MaterialDetails"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox txtMaterial_Supplied = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtMaterialSupplied");
                    TextBox txtMaterial_CommodityName = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtCommodityName");
                    TextBox txtMaterial_HSNCode = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtHSNCode");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["MaterialSupplied"] = txtMaterial_Supplied.Text;
                    dtCurrentTable.Rows[i - 1]["CommodityName"] = txtMaterial_CommodityName.Text;
                    dtCurrentTable.Rows[i - 1]["HSNCode"] = txtMaterial_HSNCode.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["MaterialDetails"] = dtCurrentTable;
                gvMaterial.DataSource = dtCurrentTable;
                gvMaterial.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousRow_Material();
    }

    private void SetPreviousRow_Material()
    {
        int rowIndex = 0;
        if (ViewState["MaterialDetails"] != null)
        {
            DataTable dt = (DataTable)ViewState["MaterialDetails"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtMaterial_Supplied = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtMaterialSupplied");
                    TextBox txtMaterial_CommodityName = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtCommodityName");
                    TextBox txtMaterial_HSNCode = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtHSNCode");

                    txtMaterial_Supplied.Text = dt.Rows[i]["MaterialSupplied"].ToString();
                    txtMaterial_CommodityName.Text = dt.Rows[i]["CommodityName"].ToString();
                    txtMaterial_HSNCode.Text = dt.Rows[i]["HSNCode"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    #endregion

    #region SERVICES

    protected void btnAddServices_OnClick(object sender, EventArgs e)
    {
        for (int i = 1; i < 6; i++)
        {
            AddNewRow_Services();
        }
    }

    private void SetInitiatRow_Services()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ServiceProvided", typeof(string)));
        dt.Columns.Add(new DataColumn("Category", typeof(string)));
        dt.Columns.Add(new DataColumn("SACCode", typeof(string)));

        for (int i = 1; i < 6; i++)
        {
            dr = dt.NewRow();
            dr["RowNumber"] = i;
            dr["ServiceProvided"] = string.Empty;
            dr["Category"] = string.Empty;
            dr["SACCode"] = string.Empty;
            dt.Rows.Add(dr);
        }

        ViewState["ServiceDetails"] = dt;
        gvServices.DataSource = dt;
        gvServices.DataBind();
    }

    private void AddNewRow_Services()
    {
        int rowIndex = 0;
        if (ViewState["ServiceDetails"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["ServiceDetails"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox txtServiceProvided = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtServiceProvided");
                    TextBox txtServiceCatg = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtServiceCatg");
                    TextBox txtSACCode = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtSACCode");

                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RowNumber"] = i + 1;
                    dtCurrentTable.Rows[i - 1]["ServiceProvided"] = txtServiceProvided.Text;
                    dtCurrentTable.Rows[i - 1]["Category"] = txtServiceCatg.Text;
                    dtCurrentTable.Rows[i - 1]["SACCode"] = txtSACCode.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["ServiceDetails"] = dtCurrentTable;
                gvServices.DataSource = dtCurrentTable;
                gvServices.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        SetPreviousRow_Services();
    }

    private void SetPreviousRow_Services()
    {
        int rowIndex = 0;
        if (ViewState["ServiceDetails"] != null)
        {
            DataTable dt = (DataTable)ViewState["ServiceDetails"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtServiceProvided = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtServiceProvided");
                    TextBox txtServiceCatg = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtServiceCatg");
                    TextBox txtSACCode = (TextBox)gvMaterial.Rows[rowIndex].FindControl("txtSACCode");

                    txtServiceProvided.Text = dt.Rows[i]["ServiceProvided"].ToString();
                    txtServiceCatg.Text = dt.Rows[i]["Category"].ToString();
                    txtSACCode.Text = dt.Rows[i]["SACCode"].ToString();
                    rowIndex++;
                }
            }
        }
    }

    #endregion

    #region KYC PRINT COPY EVENTS

    //public string GetKYCPrintPath(string FileName)
    //{
    //    string ServerFilePath = FileServer.GetFileServerDir();

    //    if (ServerFilePath == "")
    //        ServerFilePath = Server.MapPath("UploadFiles\\KYC\\" + FileName);
    //    else
    //        ServerFilePath = ServerFilePath + "\\KYC\\" + FileName;

    //    if (ServerFilePath != "")
    //    {
    //        if (System.IO.File.Exists(ServerFilePath + ".pdf"))
    //        {
    //            string ext = ".pdf";
    //            string FileId = RandomString(5);
    //            ServerFilePath += "_" + FileId + ext;
    //        }
    //        else
    //            ServerFilePath = ServerFilePath + ".pdf";
    //    }

    //    return ServerFilePath;
    //}

    protected string GetKYCPrintPath(string FileName, string FilePath)
    {
        string ServerFilePath = FileServer.GetFileServerDir();
        if (ServerFilePath == "")
        {
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName.Trim() + ".pdf"))
            {
                //string ext = ".pdf";
                string FileId = RandomString(5);
                FileName += "_" + FileId; //+ ext;
            }

            hdnUploadFile.Value = FileName + ".pdf";
            return ServerFilePath + FileName + ".pdf";
        }
        else
        {
            return "";
        }
    }

    #endregion

    #region Documents Upload
    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            ServerFilePath = Server.MapPath("..\\UploadFiles\\KYC\\" + FilePath);
        }
        else
        {
            ServerFilePath = ServerFilePath + "\\KYC\\" + FilePath;
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
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\KYC\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "\\KYC\\" + DocumentPath;
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

    protected void btnSaveKYC_Click(object sender, EventArgs e)
    {
        bool bIsScanned = false;
        int ilType = 0, iStateId = 0, iCountryId = 0, iConstitutionId = 0, iSectorId = 0, iNatureofBusinessId = 0, VendorId = 0;
        string sCompanyCode = "", sCompanyName = "", sAddress1 = "", sAddress2 = "", sCity = "", sPincode = "", sTelephoneNo = "", sFaxNo = "", sWebsiteAdd = "", sEmail = "", sPANNo = "",
        sVATNo = "", sServiceTaxNo = "", sExciseNo = "", sCSTNo = "", sTANNo = "", sSSINo = "", sBankName = "", sAccountNo = "", sIFSCCode = "", sMICRCode = "",
        sIECCode = "", sPaymentTerms = "", sPANCopyPath = "", sIECCopyPath = "", sOtherCopyPath = "", sKYCCopyPath = "", KYCScannedCopyPath = "", sBillingName = "",
        sBillingEmail = "", sBillingPhoneNo = "", sBillingAddress = "", sBillingPincode = "", sBillingCity = "";

        ilType = Convert.ToInt32(ddlGeneral_KYCType.SelectedValue);
        if (ddlGeneral_KYCType.SelectedValue != "2") // other than overseas customers
            iStateId = Convert.ToInt32(ddlGeneral_State.SelectedValue);
        iCountryId = Convert.ToInt32(ddlCountry.SelectedValue);
        iConstitutionId = Convert.ToInt32(ddlConstitution.SelectedValue);
        iSectorId = Convert.ToInt32(ddlSector.SelectedValue);
        iNatureofBusinessId = Convert.ToInt32(ddlNatureOfBusiness.SelectedValue);
        sCompanyName = txtGeneral_CompanyName.Text.Trim();
        sCompanyCode = sCompanyName.Substring(0, 5).ToUpper().Trim();
        sAddress1 = txtGeneral_CorporateAddress1.Text.Trim();
        sAddress2 = txtGeneral_CorporateAddress2.Text.Trim();
        sCity = txtGeneral_City.Text.Trim();
        sPincode = txtPinCode.Text.Trim();
        sTelephoneNo = txtTelephoneNo.Text.Trim();
       // sFaxNo = txtFaxNo.Text.Trim();
        sWebsiteAdd = txtGeneral_WebsiteAdd.Text.Trim();
        sEmail = txtGeneral_Email.Text.Trim();
        sPANNo = txtPANNo.Text.Trim();
        sVATNo = txtVATNo.Text.Trim();
        sServiceTaxNo = txtServiceTaxNo.Text.Trim();
        sExciseNo = txtExciseNo.Text.Trim();
        sCSTNo = txtCSTNo.Text.Trim();
        sTANNo = txtTANNo.Text.Trim();
        sSSINo = txtSSINo.Text.Trim();
        sBankName = txtCompany_BankName.Text.Trim();
        sAccountNo = txtCompany_AccountNo.Text.Trim();
        sIFSCCode = txtIFSCCode.Text.Trim();
        sMICRCode = txtMICRCode.Text.Trim();
        sIECCode = txtCompany_IECCode.Text.Trim();
        sPaymentTerms = txtCompany_PaymentTerms.Text.Trim();
        sBillingAddress = txtBilling_Address.Text.Trim();
        sBillingCity = txtBilling_City.Text.Trim();
        sBillingEmail = txtBilling_Email.Text.Trim();
        sBillingName = txtBilling_Name.Text.Trim();
        sBillingPhoneNo = txtBilling_MobileNo.Text.Trim();
        sBillingPincode = txtBilling_PinCode.Text.Trim();

        if (fuPanCopy.HasFile)
            sPANCopyPath = UploadFiles(fuPanCopy, sCompanyCode + "\\");
        if (fuIECCopy.HasFile)
            sIECCopyPath = UploadFiles(fuIECCopy, sCompanyCode + "\\");
        if (fuOtherCopy.HasFile)
            sOtherCopyPath = UploadFiles(fuOtherCopy, sCompanyCode + "\\");

        string strIPAddress = GetUserIP();
        VendorId = KYCOperation.AddVendorDetails(ilType, iStateId, iCountryId, iConstitutionId, iSectorId, iNatureofBusinessId, sCompanyName, sAddress1,
                   sAddress2, sCity, sPincode, sTelephoneNo, sFaxNo, sWebsiteAdd, sEmail, sPANNo, sVATNo, sServiceTaxNo, sExciseNo, sCSTNo, sTANNo, sSSINo,
                   sBankName, sAccountNo, sIFSCCode, sMICRCode, sIECCode, sPaymentTerms, sPANCopyPath, sIECCopyPath, sOtherCopyPath, sKYCCopyPath, KYCScannedCopyPath,
                   sBillingName, sBillingEmail, sBillingPhoneNo, sBillingAddress, sBillingPincode, sBillingCity, bIsScanned, loggedInUser.glUserId, strIPAddress);
        if (VendorId > 0)
        {
            DataSet dsGetQuoteByEnquiry = DBOperations.CRM_GetQuoteByEnquiry(Convert.ToInt32(hdnEnquiryId.Value));
            if (dsGetQuoteByEnquiry != null && dsGetQuoteByEnquiry.Tables[0].Rows[0]["QuotationId"] != DBNull.Value)
            {
                int Save = QuotationOperations.UpdateQuoteStatus(Convert.ToInt32(dsGetQuoteByEnquiry.Tables[0].Rows[0]["QuotationId"].ToString()),
                                        Convert.ToInt32(14), "", dtClose, loggedInUser.glUserId);
            }

            if (hdnEnquiryId.Value != "" && hdnEnquiryId.Value != "0")
            {
                int AddVendorForEnquiry = KYCOperation.UpdateVendorForEnquiry(VendorId, Convert.ToInt32(hdnEnquiryId.Value));
            }

            #region ADD GST

            if (lvGSTDetails != null && lvGSTDetails.Items.Count > 0)
            {
                for (int i = 0; i < lvGSTDetails.Items.Count; i++)
                {
                    int BranchId = 0;
                    string GSTCopyPath = "", GSTCopyName = "";

                    TextBox txtGST_CompanyName = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_CompanyName");
                    DropDownList ddlGST_Branch = (DropDownList)lvGSTDetails.Items[i].FindControl("ddlGST_Branch");
                    TextBox txtGST_Address = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_Address");
                    TextBox txtGST_ContactPerson = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_ContactPerson");
                    TextBox txtGST_ContactNo = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_ContactNo");
                    TextBox txtGST_Email = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_Email");
                    TextBox txtGST_ProvisionId = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_ProvisionId");
                    TextBox txtGST_ArnNo = (TextBox)lvGSTDetails.Items[i].FindControl("txtGST_ArnNo");
                    FileUpload fuDocument = (FileUpload)lvGSTDetails.Items[i].FindControl("fuDocument");

                    if (ddlGST_Branch.SelectedValue != "0")
                        BranchId = Convert.ToInt32(ddlGST_Branch.SelectedValue);

                    if (txtGST_CompanyName.Text.Trim() != "")
                    {
                        int GstId = KYCOperation.AddGSTDetails(VendorId, BranchId, txtGST_CompanyName.Text.Trim(), txtGST_Address.Text.Trim(), txtGST_ContactPerson.Text.Trim(),
                                    txtGST_ContactNo.Text.Trim(), txtGST_Email.Text.Trim(), txtGST_ProvisionId.Text.Trim(), txtGST_ArnNo.Text.Trim(), loggedInUser.glUserId);
                        if (GstId > 0)
                        {
                            if (fuDocument.HasFile)
                            {
                                GSTCopyName = fuDocument.PostedFile.FileName;
                                GSTCopyPath = UploadFiles(fuDocument, sCompanyCode + "\\");
                                if (GSTCopyPath != "")
                                {
                                    KYCOperation.AddGSTCopyPath(VendorId, GstId, GSTCopyPath, GSTCopyName, loggedInUser.glUserId);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region ADD CONTACT

            if (txtOperation_ContactName.Text.Trim() != "")
            {
                int OperationCon = KYCOperation.AddContactDetails(VendorId, 1, txtOperation_ContactName.Text.Trim(), txtOperation_Email.Text.Trim(), txtOperation_MobileNo.Text.Trim(),
                                    txtOperation_LandlineNo.Text.Trim(), loggedInUser.glUserId);
            }

            if (txtFinance_ContactName.Text.Trim() != "")
            {
                int FinanceCon = KYCOperation.AddContactDetails(VendorId, 2, txtFinance_ContactName.Text.Trim(), txtFinance_Email.Text.Trim(), txtFinance_MobileNo.Text.Trim(),
                                    txtFinance_LandlineNo.Text.Trim(), loggedInUser.glUserId);
            }

            if (txtOther_ContactName.Text.Trim() != "")
            {
                int OtherCon = KYCOperation.AddContactDetails(VendorId, 3, txtOther_ContactName.Text.Trim(), txtOther_Email.Text.Trim(), txtOther_MobileNo.Text.Trim(),
                                    txtOther_LandlineNo.Text.Trim(), loggedInUser.glUserId);
            }
            #endregion

            #region ADD MATERIAL
            if (ddlGeneral_KYCType.SelectedValue == "0") // vendor
            {
                if (gvMaterial != null && gvMaterial.Rows.Count > 0)
                {
                    for (int i = 0; i < gvMaterial.Rows.Count; i++)
                    {
                        TextBox txtMaterialSupplied = (TextBox)gvMaterial.Rows[i].FindControl("txtMaterialSupplied");
                        TextBox txtCommodityName = (TextBox)gvMaterial.Rows[i].FindControl("txtCommodityName");
                        TextBox txtHSNCode = (TextBox)gvMaterial.Rows[i].FindControl("txtHSNCode");

                        if (txtMaterialSupplied != null && txtMaterialSupplied.Text.Trim() != "")
                        {
                            int Material = KYCOperation.AddMaterialDetails(txtMaterialSupplied.Text.Trim(), txtCommodityName.Text.Trim(), txtHSNCode.Text.Trim(), VendorId, loggedInUser.glUserId);
                        }
                    }
                }
            }
            #endregion

            #region ADD SERVICES
            if (ddlGeneral_KYCType.SelectedValue == "0") //vendor
            {
                if (gvServices != null && gvServices.Rows.Count > 0)
                {
                    for (int i = 0; i < gvServices.Rows.Count; i++)
                    {
                        TextBox txtServiceProvided = (TextBox)gvServices.Rows[i].FindControl("txtServiceProvided");
                        TextBox txtServiceCatg = (TextBox)gvServices.Rows[i].FindControl("txtServiceCatg");
                        TextBox txtSACCode = (TextBox)gvServices.Rows[i].FindControl("txtSACCode");

                        if (txtServiceProvided != null && txtServiceProvided.Text.Trim() != "")
                        {
                            int Service = KYCOperation.AddServiceDetails(VendorId, txtServiceProvided.Text.Trim(), txtServiceCatg.Text.Trim(), txtSACCode.Text.Trim(), loggedInUser.glUserId);
                        }
                    }
                }
            }
            #endregion

            #region ADD KYC PRINT COPY
            KYCPrint(VendorId, ilType);
            #endregion
        }
    }

    protected void btnShowKYC_Click(object sender, EventArgs e)
    {
        Response.Redirect("Success.aspx");
    }

    protected void KYCPrint(int VendorId, int lType)
    {
        string strKYCPath = "", strKYCCopyName = "", strFileName = "";
        DataSet dsGetKYCDetails = KYCOperation.GetVendorDetailById(VendorId);
        if (dsGetKYCDetails != null && dsGetKYCDetails.Tables[0].Rows[0]["CompanyName"] != DBNull.Value)
        {
            strFileName = dsGetKYCDetails.Tables[0].Rows[0]["CompanyName"].ToString().ToUpper().Substring(0, 5);
            strFileName = strFileName.Trim();
            strFileName = strFileName.Replace(" ", "");
        }
        //strKYCPath = GetKYCPrintPath(strFileName);
        strKYCPath = GetKYCPrintPath(strFileName, "KYC\\");
        strKYCCopyName = "KYC\\" + hdnUploadFile.Value;

        if (strKYCPath == "")
        {
            string script = "<script type=\"text/javascript\">alert('Error while designing the KYC Copy.');</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
        }
        else
        {
            if (lType == 1) // Customer
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport.Load(Server.MapPath("KYCFormCopy.rpt"));
                dsGetKYCForm dsKYCForm = new dsGetKYCForm();

                DataTable dtVendor = new DataTable();
                dtVendor = KYCOperation.rpt_Vendor(VendorId);
                DataTable dtOperationContact = new DataTable();
                dtOperationContact = KYCOperation.rpt_OperationContact(VendorId);
                DataTable dtFinanceContact = new DataTable();
                dtFinanceContact = KYCOperation.rpt_FinanceContact(VendorId);
                DataTable dtOtherContact = new DataTable();
                dtOtherContact = KYCOperation.rpt_OtherContact(VendorId);
                DataTable dtGST = new DataTable();
                dtGST = KYCOperation.rpt_GSTDetails(VendorId);

                crystalReport.Database.Tables[4].SetDataSource(dtGST);
                crystalReport.Database.Tables[3].SetDataSource(dtOtherContact);
                crystalReport.Database.Tables[2].SetDataSource(dtFinanceContact);
                crystalReport.Database.Tables[1].SetDataSource(dtOperationContact);
                crystalReport.Database.Tables[0].SetDataSource(dtVendor);

                crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strKYCPath);
                crystalReport.Close();
                crystalReport.Clone();
                crystalReport.Dispose();
                crystalReport = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                int SaveKYCCopy = KYCOperation.UpdateKYCFormPath(VendorId, strKYCCopyName);
                if (strKYCPath != "")
                {
                    bool bSendMail = SendEmail(VendorId);
                    if (bSendMail == true)
                    {
                        Session["KycVendorId"] = VendorId.ToString();
                        Response.Redirect("Success.aspx");
                    }
                }
                else
                {
                    string script = "<script type=\"text/javascript\">alert('Something went wrong while designing the KYC Copy. Please try again later.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                }
            }
            else if (lType == 0)// Vendor
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport.Load(Server.MapPath("VendorKYCFormCopy.rpt"));
                dsGetKYCForm dsKYCForm = new dsGetKYCForm();

                DataTable dtVendor = new DataTable();
                dtVendor = KYCOperation.rpt_Vendor(VendorId);
                DataTable dtOperationContact = new DataTable();
                dtOperationContact = KYCOperation.rpt_OperationContact(VendorId);
                DataTable dtFinanceContact = new DataTable();
                dtFinanceContact = KYCOperation.rpt_FinanceContact(VendorId);
                DataTable dtOtherContact = new DataTable();
                dtOtherContact = KYCOperation.rpt_OtherContact(VendorId);
                DataTable dtGST = new DataTable();
                dtGST = KYCOperation.rpt_GSTDetails(VendorId);
                DataTable dtMaterial = new DataTable();
                dtMaterial = KYCOperation.rpt_MaterialDetails(VendorId);
                DataTable dtService = new DataTable();
                dtService = KYCOperation.rpt_ServiceDetails(VendorId);

                crystalReport.Database.Tables[6].SetDataSource(dtService);
                crystalReport.Database.Tables[5].SetDataSource(dtMaterial);
                crystalReport.Database.Tables[4].SetDataSource(dtGST);
                crystalReport.Database.Tables[3].SetDataSource(dtOtherContact);
                crystalReport.Database.Tables[2].SetDataSource(dtFinanceContact);
                crystalReport.Database.Tables[1].SetDataSource(dtOperationContact);
                crystalReport.Database.Tables[0].SetDataSource(dtVendor);

                crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strKYCPath);
                crystalReport.Close();
                crystalReport.Clone();
                crystalReport.Dispose();
                crystalReport = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                int SaveKYCCopy = KYCOperation.UpdateKYCFormPath(VendorId, strKYCCopyName);
                if (strKYCPath != "")
                {
                    bool bSendMail = SendEmail(VendorId);
                    if (bSendMail == true)
                    {
                        Session["KycVendorId"] = VendorId.ToString();
                        Response.Redirect("Success.aspx");
                    }
                }
                else
                {
                    string script = "<script type=\"text/javascript\">alert('Something went wrong while designing the KYC Copy. Please try again later.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                }
            }
            else // Overseas customer
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport.Load(Server.MapPath("Overseas.rpt"));
                dsGetKYCForm dsKYCForm = new dsGetKYCForm();

                DataTable dtVendor = new DataTable();
                dtVendor = KYCOperation.rpt_Vendor(VendorId);
                DataTable dtOperationContact = new DataTable();
                dtOperationContact = KYCOperation.rpt_OperationContact(VendorId);
                DataTable dtFinanceContact = new DataTable();
                dtFinanceContact = KYCOperation.rpt_FinanceContact(VendorId);
                DataTable dtOtherContact = new DataTable();
                dtOtherContact = KYCOperation.rpt_OtherContact(VendorId);
                DataTable dtGST = new DataTable();
                dtGST = KYCOperation.rpt_GSTDetails(VendorId);

                crystalReport.Database.Tables[4].SetDataSource(dtGST);
                crystalReport.Database.Tables[3].SetDataSource(dtOtherContact);
                crystalReport.Database.Tables[2].SetDataSource(dtFinanceContact);
                crystalReport.Database.Tables[1].SetDataSource(dtOperationContact);
                crystalReport.Database.Tables[0].SetDataSource(dtVendor);

                crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strKYCPath);
                crystalReport.Close();
                crystalReport.Clone();
                crystalReport.Dispose();
                crystalReport = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                int SaveKYCCopy = KYCOperation.UpdateKYCFormPath(VendorId, strKYCCopyName);
                if (strKYCPath != "")
                {
                    bool bSendMail = SendEmail(VendorId);
                    if (bSendMail == true)
                    {
                        Session["KycVendorId"] = VendorId.ToString();
                        Response.Redirect("Success.aspx");
                    }
                }
                else
                {
                    string script = "<script type=\"text/javascript\">alert('Something went wrong while designing the KYC Copy. Please try again later.');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                }
            }
        }
    }

    protected bool SendEmail(int VendorId)
    {
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilder_Attendee = new StringBuilder();
        StringBuilder strAttendeeEmail = new StringBuilder();

        if (VendorId > 0)
        {
            string EncryptedVendorId = HttpUtility.UrlEncode(Encrypt(VendorId.ToString()));
            string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strCCEmail_Loc = "", strSubject = "", EmailContent = "", strKYCPath = "", strCompanyEmail = "";
            DataSet dsGetVendor = KYCOperation.GetVendorDetailById(VendorId);
            if (dsGetVendor != null)
            {
                try
                {
                    string strFileName = "../EmailTemplate/StampKYC.html";
                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    return false;
                }

                MessageBody = EmailContent.Replace("@KycLink", "http://live.babajishivram.com/KYC_New/SaveKYC.aspx?p=" + EncryptedVendorId);
                //MessageBody = EmailContent.Replace("@KycLink", "http://192.168.5.46/CRMProject/KYC_New/SaveKYC.aspx?p=" + VendorId);
                MessageBody = MessageBody.Replace("@Company", dsGetVendor.Tables[0].Rows[0]["CompanyName"].ToString());
                MessageBody = MessageBody.Replace("@Address1", dsGetVendor.Tables[0].Rows[0]["Address1"].ToString());
                MessageBody = MessageBody.Replace("@Address2", dsGetVendor.Tables[0].Rows[0]["Address2"].ToString());
                MessageBody = MessageBody.Replace("@Email", dsGetVendor.Tables[0].Rows[0]["Email"].ToString());
                MessageBody = MessageBody.Replace("@City", dsGetVendor.Tables[0].Rows[0]["City"].ToString());
                MessageBody = MessageBody.Replace("@State", dsGetVendor.Tables[0].Rows[0]["StateName"].ToString());
                MessageBody = MessageBody.Replace("@Country", dsGetVendor.Tables[0].Rows[0]["CountryName"].ToString());
                MessageBody = MessageBody.Replace("@Pincode", dsGetVendor.Tables[0].Rows[0]["Pincode"].ToString());
                MessageBody = MessageBody.Replace("@Website", dsGetVendor.Tables[0].Rows[0]["WebsiteAdd"].ToString());

                strCompanyEmail = dsGetVendor.Tables[0].Rows[0]["Email"].ToString();
                strCCEmail_Loc = dsGetVendor.Tables[0].Rows[0]["EnquiryPersonEmail"].ToString();

                // Email FormatdsGetVendor
                strCustomerEmail = strCompanyEmail; //"kivisha.jain@babajishivram.com"; //strCompanyEmail; // "jr.developer@babajishivram.com";
                strCCEmail = "javed.shaikh@babajishivram.com," + strCCEmail_Loc;
                strSubject = "Stamped KYC Request from Babaji Shivram Clearing & Carriers Pvt. Ltd.";

                if (strCustomerEmail == "" || strSubject == "")
                    return false;
                else
                {
                    List<string> lstFilePath = new List<string>();
                    strKYCPath = dsGetVendor.Tables[0].Rows[0]["KYCCopyPath"].ToString();
                    lstFilePath.Add(strKYCPath);

                    bEmailSuccess = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                    return bEmailSuccess;
                }
            }
            else
                return false;
        }
        else
            return false;
    }

    #region ENCRYPT/DECRYPT QUERYSTRING VARIABLES
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    private string Decrypt(string cipherText)
    {
        try
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
        catch (Exception en)
        {

        }
        return cipherText;
    }
    #endregion

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }

    protected void btnOversea_SaveKYC_Click(object sender, EventArgs e)
    {
        if (ddlGeneral_KYCType.SelectedValue == "2")   // overseas customer
        {
            bool bIsScanned = false;
            int ilType = 2, iStateId = 0, iCountryId = 0, iConstitutionId = 0, iSectorId = 0, iNatureofBusinessId = 0, VendorId = 0;
            string sCompanyCode = "", sCompanyName = "", sAddress1 = "", sAddress2 = "", sCity = "", sPincode = "", sTelephoneNo = "", sFaxNo = "", sWebsiteAdd = "", sEmail = "", sPANNo = "",
            sVATNo = "", sServiceTaxNo = "", sExciseNo = "", sCSTNo = "", sTANNo = "", sSSINo = "", sBankName = "", sAccountNo = "", sIFSCCode = "", sMICRCode = "",
            sIECCode = "", sPaymentTerms = "", sPANCopyPath = "", sIECCopyPath = "", sOtherCopyPath = "", sKYCCopyPath = "", KYCScannedCopyPath = "", sBillingName = "",
            sBillingEmail = "", sBillingPhoneNo = "", sBillingAddress = "", sBillingPincode = "", sBillingCity = "";

            //if (ddlGeneral_KYCType.SelectedValue != "2") // other than overseas customers
            //    iStateId = Convert.ToInt32(ddlGeneral_State.SelectedValue);
            iCountryId = Convert.ToInt32(ddlCountry.SelectedValue);
            iConstitutionId = Convert.ToInt32(ddlConstitution.SelectedValue);
            iSectorId = Convert.ToInt32(ddlSector.SelectedValue);
            iNatureofBusinessId = Convert.ToInt32(ddlNatureOfBusiness.SelectedValue);
            sCompanyName = txtGeneral_CompanyName.Text.Trim();
            sCompanyCode = sCompanyName.Substring(0, 3).ToUpper().Trim();
            sAddress1 = txtGeneral_CorporateAddress1.Text.Trim();
            sAddress2 = txtGeneral_CorporateAddress2.Text.Trim();
            sCity = txtGeneral_City.Text.Trim();
            sPincode = txtPinCode.Text.Trim();
            sTelephoneNo = txtTelephoneNo.Text.Trim();
           // sFaxNo = txtFaxNo.Text.Trim();
            sWebsiteAdd = txtGeneral_WebsiteAdd.Text.Trim();
            sEmail = txtGeneral_Email.Text.Trim();
            sPANNo = txtPANNo.Text.Trim();
            sVATNo = txtVATNo.Text.Trim();
            sServiceTaxNo = txtServiceTaxNo.Text.Trim();
            sExciseNo = txtExciseNo.Text.Trim();
            sCSTNo = txtCSTNo.Text.Trim();
            sTANNo = txtTANNo.Text.Trim();
            sSSINo = txtSSINo.Text.Trim();
            sBankName = txtCompany_BankName.Text.Trim();
            sAccountNo = txtCompany_AccountNo.Text.Trim();
            sIFSCCode = txtIFSCCode.Text.Trim();
            sMICRCode = txtMICRCode.Text.Trim();
            sIECCode = txtCompany_IECCode.Text.Trim();
            sPaymentTerms = txtCompany_PaymentTerms.Text.Trim();
            sBillingAddress = txtOversea_BillingAddress.Text.Trim();
            sBillingCity = txtOversea_BillingCity.Text.Trim();
            sBillingEmail = txtOversea_BillingEmail.Text.Trim();
            sBillingName = txtOversea_BillingName.Text.Trim();
            sBillingPhoneNo = txtOversea_BillingMobile.Text.Trim();
            sBillingPincode = txtOversea_BillingPinCode.Text.Trim();

            //if (fuPanCopy.HasFile)
            //    sPANCopyPath = UploadFiles(fuPanCopy, sCompanyCode + "\\");
            //if (fuIECCopy.HasFile)
            //    sIECCopyPath = UploadFiles(fuIECCopy, sCompanyCode + "\\");
            if (fuOversea_OtherCopy.HasFile)
                sOtherCopyPath = UploadFiles(fuOversea_OtherCopy, sCompanyCode + "\\");

            string strIPAddress = GetUserIP();
            VendorId = KYCOperation.AddVendorDetails(ilType, iStateId, iCountryId, iConstitutionId, iSectorId, iNatureofBusinessId, sCompanyName, sAddress1,
                       sAddress2, sCity, sPincode, sTelephoneNo, sFaxNo, sWebsiteAdd, sEmail, sPANNo, sVATNo, sServiceTaxNo, sExciseNo, sCSTNo, sTANNo, sSSINo,
                       sBankName, sAccountNo, sIFSCCode, sMICRCode, sIECCode, sPaymentTerms, sPANCopyPath, sIECCopyPath, sOtherCopyPath, sKYCCopyPath,
                       KYCScannedCopyPath, sBillingName, sBillingEmail, sBillingPhoneNo, sBillingAddress, sBillingPincode, sBillingCity, bIsScanned, loggedInUser.glUserId, strIPAddress);
            if (VendorId > 0)
            {
                if (hdnEnquiryId.Value != "" && hdnEnquiryId.Value != "0")
                {
                    int AddVendorForEnquiry = KYCOperation.UpdateVendorForEnquiry(VendorId, Convert.ToInt32(hdnEnquiryId.Value));
                }

                #region ADD CONTACT
                if (txtOversea_OperationContact.Text.Trim() != "")
                {
                    int OperationCon = KYCOperation.AddContactDetails(VendorId, 1, txtOversea_OperationContact.Text.Trim(), txtOversea_OperationEmail.Text.Trim(), txtOversea_OperationMobile.Text.Trim(),
                                        txtOversea_OperationLandline.Text.Trim(), loggedInUser.glUserId);
                }

                if (txtOversea_FinanceContact.Text.Trim() != "")
                {
                    int FinanceCon = KYCOperation.AddContactDetails(VendorId, 2, txtOversea_FinanceContact.Text.Trim(), txtOversea_FinanceEmail.Text.Trim(), txtOversea_FinanceMobile.Text.Trim(),
                                        txtOversea_FinanceLandline.Text.Trim(), loggedInUser.glUserId);
                }

                if (txtOversea_OtherContact.Text.Trim() != "")
                {
                    int OtherCon = KYCOperation.AddContactDetails(VendorId, 3, txtOversea_OtherContact.Text.Trim(), txtOversea_OtherEmail.Text.Trim(), txtOversea_OtherMobile.Text.Trim(),
                                        txtOversea_OtherLandline.Text.Trim(), loggedInUser.glUserId);
                }
                #endregion

                #region ADD KYC PRINT COPY
                KYCPrint(VendorId, ilType);
                #endregion
            }
            else if (VendorId == -1)
            {
                string script = "<script type=\"text/javascript\">alert('Something went wrong while designing the KYC Copy. Please try again later.');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
            }
            else if (VendorId == -2)
            {
                string script = "<script type=\"text/javascript\">alert('KYC already created for Customer!');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                btnOversea_SaveKYC.Enabled = true;
            }
        }
    }
}