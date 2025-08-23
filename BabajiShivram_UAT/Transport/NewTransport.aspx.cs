using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using AjaxControlToolkit;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using iTextSharp.text.pdf.qrcode;
using Newtonsoft.Json.Linq;
using System.Net.Http;


public partial class Transport_NewTransport : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnAddContainer);
        lblRefNo.Text = DBOperations.GetNewTransportNo();
        //txtJobNo.Text = DBOperations.TR_GetNewJobRefNo(Convert.ToInt32(Session["FinYearId"]));
        
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Request";

            DataTable dtContainer = new DataTable();
            dtContainer.Columns.AddRange(new DataColumn[8] {new DataColumn("PkId"), new DataColumn("JobId"), new DataColumn("ContainerNo"),
                                                            new DataColumn("ContainerSize"), new DataColumn("ContainerType"), new DataColumn("UserId"),
                                                            new DataColumn("ContainerSizeId"), new DataColumn("ContainerTypeId")});
            ViewState["Container"] = dtContainer;
            DBOperations.FillUserBranch(ddlBranch, LoggedInUser.glUserId);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        /****************************************************/
        string strTRRefNo, strCustomer = "", strJobNo = "", strLocFrom = "", strLocTo = "", strRemarks = "";
        int TransportType, CustomerId, DivisionId, PlantId, DeliveryType, TransMode, Pincode1 = 0, Pincode2 = 0;
        int Count20 = 0, Count40 = 0, NoOfpackages = 0;
        string strGrossWeight = "0";
        decimal GrossWeight = 0;
        string PickupAdd = "", PickState = "", PickupCity = "", DropAdd = "", DropState = "", DropCity = ""; // string Pickup and drop added
        string EmptyLetter = "", FileName = "", EmptyDocPath = "", DocType = "", IsMobile = "", updDate = "", updUser = "";

        if (!string.IsNullOrEmpty(txtPincode1.Text.Trim()))        //pincode1  save code
        {
            if (!int.TryParse(txtPincode1.Text.Trim(), out Pincode1))
            {
                lblError.Text = "Invalid input for Pincode 1.";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtPincode2.Text.Trim()))     //pincode2 save code
        {
            if (!int.TryParse(txtPincode2.Text.Trim(), out Pincode2))
            {

                lblError.Text = "Invalid input for Pincode 2.";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        TransportType = 2; // New Transport Request
        strTRRefNo = lblRefNo.Text.Trim();
        strCustomer = txtCustomer.Text.ToUpper().Trim();
        strJobNo = txtJobNo.Text.Trim();
        strLocFrom = txtFrom.Text.Trim();
        strLocTo = txtTo.Text.Trim();
        strRemarks = txtRemark.Text.Trim();
        DivisionId = Convert.ToInt32(ddDivision.SelectedValue);
        PlantId = Convert.ToInt32(ddPlant.SelectedValue);
        TransMode = Convert.ToInt32(ddMode.SelectedValue);

        PickupAdd = txtPickupAdd.Text;                                        //pincode ,pick, drop and loaded empty doc added changes for save operation
        PickState = txtState1.Text;
        PickupCity = txtCity1.Text;
        DropAdd = txtDropAdd.Text;
        DropState = txtState2.Text;
        DropCity = txtCity2.Text;
        EmptyLetter = loadedDocuments.FileName;
        Pincode1 = Convert.ToInt32(txtPincode1.Text);
        Pincode2 = Convert.ToInt32(txtPincode2.Text);

        if (ddDeliveryType.SelectedValue == "1")
        {
            EmptyDocPath = Regex.Replace(txtCustomer.Text, "[^a-zA-Z0-9]", "");
            EmptyDocPath = EmptyDocPath + "\\";
            if (loadedDocuments != null && loadedDocuments.HasFile)
                FileName = UploadEmptyDocuments(EmptyDocPath);
        }

        if (TransMode == 1) // Air
        {
            DeliveryType = 0;
        }
        else // Sea
        {
            DeliveryType = Convert.ToInt32(ddDeliveryType.SelectedValue);
            if (DeliveryType == 1) // loaded
            {
                if (gvContainer.Rows.Count == 0)
                {
                    lblError.Text = "Please enter atleast 1 container details!";
                    lblError.CssClass = "errorMsg";
                    return;
                }
            }
        }

        if (strCustomer == "")
        {
            lblError.Text = "Please Search & Select Customer Name!.";
            lblError.CssClass = "errorMsg";
            return;
        }
        else
        {
            if (hdnCustId.Value != "0" && hdnCustId.Value != "")
            {
                CustomerId = Convert.ToInt32(hdnCustId.Value);
            }
            else
            {
                lblError.Text = "Please Search & Select Customer Name Properly!.";
                lblError.CssClass = "errorMsg";
                return;
            }
        }

        if (txtJobNo.Text.Trim() != "")
        {
            strJobNo = txtJobNo.Text.Trim();
        }
        else 
        {
            lblError.Text = "Job No Can not be Blank!!.";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (txtNoOfPkgs.Text.Trim() != "")
        {
            NoOfpackages = Convert.ToInt32(txtNoOfPkgs.Text.Trim());
        }

        if (txtGrossWeight.Text.Trim() != "")
        {
            GrossWeight = Convert.ToDecimal(txtGrossWeight.Text.Trim());
        }

        int TransportId = DBOperations.AddNewTransportRequest(strTRRefNo, TransMode, TransportType, CustomerId, DivisionId, PlantId, strJobNo, strLocFrom, strLocTo, Count20, Count40,
                                                    NoOfpackages, GrossWeight, strRemarks, DeliveryType, LoggedInUser.glUserId);

        int results = DBOperations.AddTransAddDetails(Convert.ToInt32(hdnJobId.Value), TransportId, FileName, EmptyDocPath + "\\", 122, PickupAdd, Pincode1, PickState, PickupCity, DropAdd, Pincode2, DropState, DropCity, EmptyLetter, DeliveryType, LoggedInUser.glUserId);

        if (TransportId > 0)
        {
            // Add container details
            DataTable dtContainer = (DataTable)ViewState["Container"];
            if (dtContainer != null && dtContainer.Rows.Count > 0)
            {
                for (int c = 0; c < dtContainer.Rows.Count; c++)
                {
                    DBOperations.AddContainerDetailTR(TransportId, dtContainer.Rows[c]["ContainerNo"].ToString(), Convert.ToInt32(dtContainer.Rows[c]["ContainerSizeId"].ToString()),
                                 Convert.ToInt32(dtContainer.Rows[c]["ContainerTypeId"].ToString()), Convert.ToInt32(dtContainer.Rows[c]["UserId"].ToString()));
                }
            }
            //Redirect to another page
            Response.Redirect("SuccessPage.aspx?NewTransport=393");
        }
        else if (TransportId == -123)
        {
            lblError.Text = "Cannot create 18-19 financial job!";
            lblError.CssClass = "errorMsg";
        }
        else if (TransportId == -2)
        {
            lblError.Text = " Transport Ref No " + lblRefNo.Text + " Already Created. Please Submit Again!";
            lblError.CssClass = "errorMsg";
            lblRefNo.Text = DBOperations.GetNewTransportNo();
        }
        else if (TransportId == -1)
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please check required fields.";
            lblError.CssClass = "errorMsg";
        }
        /***********************/
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        int CustomerId = 0;
        if (hdnCustId.Value != "" && hdnCustId.Value != "0")
        {
            CustomerId = Convert.ToInt32(hdnCustId.Value);
        }

        if (CustomerId > 0)
        {
            DBOperations.FillCustomerDivision(ddDivision, CustomerId);
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddDivision.Items.Clear();
            ddDivision.Items.Add(lstSelect);
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }

    protected void ddMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddDeliveryType.Enabled = true;
        ddDeliveryType.SelectedIndex = 0;
        if (ddMode.SelectedValue == "1")        // Air
        {
            ddDeliveryType.SelectedItem.Text = "Air";
            ddDeliveryType.Enabled = false;
            fsContainerDetails.Visible = false;
            rfvDeliveryType.Enabled = false;
        }
        else if (ddMode.SelectedValue == "2")   // Sea
        {
            ddDeliveryType.SelectedItem.Text = "--select--";
            ddDeliveryType.SelectedValue = "0";
            fsContainerDetails.Visible = true;
        }
    }

    protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }

        if (ddDeliveryType.SelectedValue == "1")    //Loaded , '1' represents the Loaded option
        {
            loadedDocuments.Visible = true;
            lblEmpty_Letter.Visible = true;
            //  UpdBtn.Visible = true;
        }
        else
        {
            loadedDocuments.Visible = false;
            lblEmpty_Letter.Visible = false;
            //   UpdBtn.Visible = false;
        }
    }

        protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);
        if (DivisonId > 0)
        {
            DBOperations.FillCustomerPlant(ddPlant, DivisonId);
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }

    protected void Clear()
    {
        lblRefNo.Text = DBOperations.GetNewTransportNo();
        txtJobNo.Text = DBOperations.TR_GetNewJobRefNo(Convert.ToInt32(Session["FinYearId"]));

        txtCustomer.Text = "";
        txtJobNo.Text = "";
        txtTo.Text = "";
        txtFrom.Text = "";
        txtNoOfPkgs.Text = "";
        txtGrossWeight.Text = "";
        txtRemark.Text = "";
        hdnCustId.Value = "";
    }

    #region CONTAINER DETAILS EVENTS

    protected void btnAddContainerDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception en)
        {

        }
    }

    protected void BindGrid()
    {
        DataTable dtContainerLists = (DataTable)ViewState["Container"];
        gvContainer.DataSource = dtContainerLists;
        gvContainer.DataBind();
    }

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        int PkId = 1, count = 0;
        lblError.Visible = true;
        int JobId = Convert.ToInt32(Session["JobId"]);
        string ContainerSize = "", ContainerType = "", SealNo = "";
        int ContainerSizeId = 0, ContainerTypeId = 0;

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerTypeId = Convert.ToInt32(ddContainerType.SelectedValue);

        if (ContainerTypeId == 1) //FCL
        {
            if (ddContainerSize.SelectedValue == "0")
            {
                lblError.Text = "Please Select FCL Container Size!";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                ContainerSizeId = Convert.ToInt32(ddContainerSize.SelectedValue);
            }
        }
        else if (ContainerTypeId == 2) //LCL
        {
            ddContainerSize.SelectedValue = "0";
            ContainerSizeId = 0;
        }

        if (ContainerNo != "")
        {
            int OriginalRows = 0, AfterInsertedRows = 0;
            if (ddContainerSize.SelectedValue == "0")
                ContainerSize = "";
            else
                ContainerSize = ddContainerSize.SelectedItem.Text.Trim();
            ContainerType = ddContainerType.SelectedItem.Text.Trim();
            //SealNo = txtSealNo.Text.Trim();
            DataTable dtContainer = (DataTable)ViewState["Container"];
            if (dtContainer != null && dtContainer.Rows.Count > 0)
                PkId++;
            if (dtContainer != null)
                OriginalRows = dtContainer.Rows.Count;              //get original rows of grid view.
            if (dtContainer != null)
            {
                foreach (DataRow dr in dtContainer.Rows)
                {
                    if (dr["ContainerNo"].ToString() == ContainerNo)
                    {
                        lblError.Text = "Container No " + ContainerNo + " Already Added!";
                        lblError.CssClass = "warning";
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            if (count == 0)
            {
                dtContainer.Rows.Add(PkId, JobId, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId, ContainerSizeId, ContainerTypeId);
                //insert row into datatable.
                AfterInsertedRows = dtContainer.Rows.Count;     //get present rows after deleting particular row from grid view.
                ViewState["Container"] = dtContainer;
                BindGrid();
                if (OriginalRows < AfterInsertedRows)
                {
                    lblError.Text = "Container No " + ContainerNo + " Added successfully!";
                    lblError.CssClass = "success";
                    txtContainerNo.Text = "";
                    ddContainerSize.SelectedValue = "0";
                    ddContainerType.SelectedValue = "1";
                    //txtSealNo.Text = "";
                    ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
                }
                else
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please Enter Container No.!";
        }
    }

    protected void ddContainerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddContainerType.SelectedValue == "2")
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddContainerSize.Items.Clear();
            ddContainerSize.Items.Add(lstSelect);
        }
        else
        {
            ddContainerSize.Items.Clear();
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            System.Web.UI.WebControls.ListItem lstSelect20 = new System.Web.UI.WebControls.ListItem("20", "1");
            System.Web.UI.WebControls.ListItem lstSelect40 = new System.Web.UI.WebControls.ListItem("40", "2");
            System.Web.UI.WebControls.ListItem lstSelect45 = new System.Web.UI.WebControls.ListItem("45", "3");
            ddContainerSize.Items.Add(lstSelect);
            ddContainerSize.Items.Add(lstSelect20);
            ddContainerSize.Items.Add(lstSelect40);
            ddContainerSize.Items.Add(lstSelect45);
        }
    }

    protected void OnContainerDelete(object sender, EventArgs e)
    {
        int OriginalRows = 0, AfterDeletedRows = 0;

        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["Container"] as DataTable;
        OriginalRows = dt.Rows.Count;       // get original rows of grid view
        dt.Rows.RemoveAt(row.RowIndex);     // delete row from datatable using specific rowindex id
        AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
        ViewState["Container"] = dt;
        BindGrid();
        if (OriginalRows > AfterDeletedRows)
        {
            lblError.Text = "Successfully Deleted Container Details.";
            lblError.CssClass = "success";
            gvContainer.DataBind();
        }
        else
        {
            lblError.Text = "Error while deleting container details. Please try again later..!!";
            lblError.CssClass = "success";
        }
    }

    protected void btnCancelContainer_Click(object sender, EventArgs e)
    {
        txtContainerNo.Text = "";
        ddContainerSize.SelectedValue = "0";
        ddContainerType.SelectedValue = "1";
        BindGrid();
        gvContainer.DataBind();
    }

    #endregion

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        txtJobNo.Text = "";
        string Result = "";
        int BranchID = Convert.ToInt32(ddlBranch.SelectedValue);

        if (BranchID > 0)
        {
            Result = DBOperations.TR_GetNextOtherJobNo(BranchID);

            txtJobNo.Text = Result;

            if (Result == "")
            {
                lblError.Text = "Please select Branch";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please select Branch";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void txtPincode1_TextChanged(object sender, EventArgs e)   //Get State and city by using Pincode API 
    {
        string pincode = txtPincode1.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnPincodeId.Value = "0";
            txtState1.Text = "Invalid Pincode";
            txtCity1.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;

                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {
                            txtState1.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtCity1.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnPincodeId.Value = pincode;
                        }
                        else
                        {
                            txtState1.Text = "State Not Found";
                            txtCity1.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtState1.Text = "State Not Found";
                        txtCity1.Text = "City Not Found";
                    }
                }
                else
                {
                    txtState1.Text = "Error";
                    txtCity1.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtState1.Text = "Error";
            txtCity1.Text = "Error";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtState1.Text = "Error";
            txtCity1.Text = "Error";
        }
    }

    protected void txtPincode2_TextChanged(object sender, EventArgs e)    // Get State and City By using Pincode API
    {
        string pincode = txtPincode2.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnpinid.Value = "0";
            txtState2.Text = "Invalid Pincode";
            txtCity2.Text = "Invalid Pincode";
            return;
        }

        string apiKey = "579b464db66ec23bdd000001cdd3946e44ce4aad7209ff7b23ac571b";
        string apiUrl = $"https://api.data.gov.in/resource/5c2f62fe-5afa-4119-a499-fec9d604d5bd?api-key={apiKey}&format=json&filters%5Bpincode%5D={pincode}";

        try
        {
            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage responseMessage = httpClient.GetAsync(apiUrl).Result;

                if (responseMessage.IsSuccessStatusCode)
                {

                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    JObject apiResponse = JObject.Parse(responseString);
                    JArray records = apiResponse["records"] as JArray;


                    if (records != null && records.Count > 0)
                    {
                        var record = records.FirstOrDefault(r => r["pincode"]?.ToString() == pincode);
                        if (record != null)
                        {

                            txtState2.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtCity2.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnpinid.Value = pincode;
                        }
                        else
                        {
                            txtState2.Text = "State Not Found";
                            txtCity2.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtState2.Text = "State Not Found";
                        txtCity2.Text = "City Not Found";
                    }
                }
                else
                {
                    txtState2.Text = "Error";
                    txtCity2.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {

            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtState2.Text = "Error";
            txtCity2.Text = "Error";
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtState2.Text = "Error";
            txtCity2.Text = "Error";
        }

    }

    private string UploadEmptyDocuments(string FilePath)
    {
        string FileName = loadedDocuments.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath + "\\");
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
        if (loadedDocuments.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            loadedDocuments.SaveAs(ServerFilePath + FileName);

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
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));
        }
        return builder.ToString();
    }
}
