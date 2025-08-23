using System;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Web.UI;
public partial class InvoiceTrack_NewInvoice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveInvoice);

        Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text   = "New Invoice Detail";

        if (!IsPostBack)
        {
            DBOperations.FillBranch(ddBranch);
            MskValInvoiceDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        }

        DataFilter1.DataSource = DataSourcePendingInvoice;
        DataFilter1.DataColumns = gvInvoice.Columns;
        DataFilter1.FilterSessionID = "NewInvoice.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }
    protected void btnSaveInvoice_Click(object sender, EventArgs e)
    {
        string strVendorName = "", strVendorCode="", strGSTNo = "", strInvoiceNo = "", InvoiceFilePath = "";

        string strCompanyCode = "", strDivisionCode = "", strJobRefNo = "", strPaymentTerms = ""; 
        string strInvoiceFilePath = "",    strMSMECerficatePath = "";

        int BabajiBranchID = 0, intVendorCategory = 0;

        Boolean IsIGSTApplicable = false, IsMSME = false; 
        
        DateTime dtInvoiceDate = DateTime.MinValue;

        decimal decInvoiceAmount = 0m, decTotalAmount = 0m, decGSTAmount = 0m;
        decimal decGSTRate = 0m;

        strVendorName   = txtVendorName.Text.Trim();
        strVendorCode   = hdnPartyCode.Value.Trim();
        strGSTNo        = txtGSTN.Text.Trim();

        strInvoiceNo    = txtInvoiceNo.Text.Trim();
        strCompanyCode  = ddCompany.SelectedValue;
        strDivisionCode = ddDivision.SelectedValue;
        strJobRefNo     = txtJobRefNo.Text.Trim();
        strPaymentTerms = ddPaymentTerms.SelectedValue;

        BabajiBranchID  = Convert.ToInt32(ddBranch.SelectedValue);
        intVendorCategory = Convert.ToInt32(ddVendorCategory.SelectedValue);

        if(rdTaxType.SelectedValue == "1")
        {
            IsIGSTApplicable = true;
        }
        if (txtInvoiceDate.Text != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }
        if (txtTaxAmount.Text.Trim() != "")
        {
            decInvoiceAmount = Convert.ToDecimal(txtTaxAmount.Text.Trim());
        }

        if (txtGSTRate.Text.Trim() != "")
        {
            decGSTRate = Convert.ToDecimal(txtGSTRate.Text.Trim());
        }
        if (txtGSTAmount.Text.Trim() != "")
        {
            decGSTAmount = Convert.ToDecimal(txtGSTAmount.Text.Trim());
        }
        if (txtTotalAmount.Text.Trim() != "")
        {
            decTotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim());
        }
        
        if (rdMSME.SelectedValue == "1")
        {
            IsMSME = true;
        }

        string TokanNo = DBOperations.AddVendorInvoice(strCompanyCode, BabajiBranchID, strDivisionCode, strJobRefNo,
            strVendorName, strVendorCode, strGSTNo, intVendorCategory, strInvoiceNo, dtInvoiceDate,
        decInvoiceAmount, IsIGSTApplicable, decGSTRate, decGSTAmount, decTotalAmount,
         strPaymentTerms, IsMSME, strInvoiceFilePath, strMSMECerficatePath, LoggedInUser.glUserId);

        if (TokanNo != "")
        {
            lblError.Text = "Invoice Detail Added Successfully!";
            lblError.CssClass = "success";

            if (FileInvoice.HasFile)
            {
                InvoiceFilePath = "VendorInvoice//";

                string FileName  = UploadInvoice(FileInvoice, TokanNo);
                string FileUploadPath = InvoiceFilePath + FileName;

                int FileOutput = DBOperations.UpdateVendorInvoiceCopy(TokanNo, FileUploadPath);
            }
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
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
    protected void gvInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int32 lStatus = 0;
            string StatusName = "";

            if ((DataBinder.Eval(e.Row.DataItem, "lStatus")) != DBNull.Value)
            {
                lStatus = (Int32)(DataBinder.Eval(e.Row.DataItem, "lStatus"));
                StatusName = (String)(DataBinder.Eval(e.Row.DataItem, "StatusName"));

                if (lStatus > 1)
                {
                    LinkButton lnkForwardInvoice = (LinkButton)e.Row.FindControl("lnkForwardInvoice");

                    lnkForwardInvoice.Visible = false;

                    e.Row.BackColor = System.Drawing.Color.FromName("#2266bb");
                    e.Row.ToolTip = StatusName;
                }
            }
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

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "NewInvoice.aspx";
            DataFilter1.FilterDataSource();
            gvInvoice.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Forward Invoice

    protected void gvInvoice_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "forward")
        {
            int StatusID = 15; // Invoice Forwarded To HO, Pending Received From HO
            string strTokanId = (string)e.CommandArgument;
            int result = DBOperations.UpdateInvoiceForward(Convert.ToInt32(strTokanId), StatusID, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Invoice Forwarded to HO!";
                lblError.CssClass = "success";

                gvInvoice.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "errorMsg";

            }
            else if (result == 2)
            {
                lblError.Text = "Invoice Already Forwarded to HO!";
                lblError.CssClass = "success";

                gvInvoice.DataBind();
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime";
                lblError.CssClass = "success";

                gvInvoice.DataBind();
            }
        }

    }

    #endregion

}