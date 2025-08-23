using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

public partial class Transport_TruckRequest : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        //JobDetailExtender.ContextKey = Convert.ToString(Session["FinYearId"]);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        ScriptManager1.RegisterPostBackControl(btnSubmit);  //btn submite save operation
        if (!IsPostBack)
        {
            lblTransRefNo.Text = DBOperations.GetNewTransportNo();

            DataTable dtPackingList = new DataTable();
            dtPackingList.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["PackingList"] = dtPackingList;

            // Allow  Only Future Date For Reminder
            calVehiclePlaceDate.StartDate = DateTime.Today;

            // Set the minimum value for the MaskedEditValidator to today's date
            mevVehiclePlaceDate.MinimumValue = DateTime.Today.ToString("dd/MM/yyyy");

        }
    }

    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        int JobId = 0;
        if (txtJobNumber.Text.Trim() == "")
            hdnJobId.Value = "0";
        else
            JobId = Convert.ToInt32(hdnJobId.Value);
        //lblBranch.Text = JobId.ToString()+"@@@";
        if (JobId > 0)
        {
            DataView dsGetJobDetail = DBOperations.GetTransportJobDetailByJobId(JobId);
            if (dsGetJobDetail != null)
            {
                hdnJobType.Value = dsGetJobDetail.Table.Rows[0]["JobType"].ToString();
                int JobType = Convert.ToInt32(dsGetJobDetail.Table.Rows[0]["JobType"].ToString());
                if (JobType == 1) // import job
                {
                    ddDeliveryType.Visible = true;
                    ddlExportType.Visible = false;
                    if (dsGetJobDetail.Table.Rows[0]["DeliveryTypeId"] != DBNull.Value)
                    {
                        ddDeliveryType.SelectedValue = dsGetJobDetail.Table.Rows[0]["DeliveryTypeId"].ToString();
                        ddDeliveryType_SelectedIndexChanged(null, EventArgs.Empty);
                    }
                    lblConsigneeShipper_Title.Text = "Consignee Name";
                    lblType_Title.Text = "Delivery Type";
                    lblBOEType_Title.Text = "Type Of BOE";
                    lblBOEType.Text = dsGetJobDetail.Table.Rows[0]["BOEType"].ToString();
                }
                else // export job
                {
                    ddDeliveryType.Visible = false;
                    ddlExportType.Visible = true;
                    if (dsGetJobDetail.Table.Rows[0]["ExportTypeId"] != DBNull.Value)
                    {
                        ddlExportType.SelectedValue = dsGetJobDetail.Table.Rows[0]["ExportTypeId"].ToString();
                        ddlExportType_SelectedIndexChanged(null, EventArgs.Empty);
                    }
                    lblConsigneeShipper_Title.Text = "Shipper Name";
                    lblType_Title.Text = "Export Type";
                    lblBOEType_Title.Text = "Shipping Bill Type";
                    lblBOEType.Text = dsGetJobDetail.Table.Rows[0]["ShippingBillType"].ToString();
                }

                if (dsGetJobDetail.Table.Rows[0]["TransMode"].ToString() != "")
                {
                    if (dsGetJobDetail.Table.Rows[0]["TransMode"].ToString() == "1") // AIR
                    {
                        ddDeliveryType.SelectedItem.Text = "Air";
                        RFVDeliveryType.Enabled = false;
                        ddDeliveryType.Enabled = false;
                        ddlExportType.Enabled = false;
                    }
                }
                lblCustomer.Text = dsGetJobDetail.Table.Rows[0]["Customer"].ToString();
                lblConsignee.Text = dsGetJobDetail.Table.Rows[0]["Consignee"].ToString();
                lblShipper.Text = dsGetJobDetail.Table.Rows[0]["ShipperName"].ToString();
                lblBranch.Text = dsGetJobDetail.Table.Rows[0]["BabajiBranch"].ToString();
                hdnMode.Value = dsGetJobDetail.Table.Rows[0]["TransMode"].ToString();
                lblPort.Text = dsGetJobDetail.Table.Rows[0]["PortName"].ToString();
                lblGrossWt.Text = dsGetJobDetail.Table.Rows[0]["GrossWT"].ToString();
                hdnCustId.Value = dsGetJobDetail.Table.Rows[0]["CustomerId"].ToString();
                if (dsGetJobDetail.Table.Rows[0]["TotalNoContainer"] != DBNull.Value)
                    lblNoOfContainers.Text = dsGetJobDetail.Table.Rows[0]["TotalNoContainer"].ToString();
                else
                    lblNoOfContainers.Text = "0";
                if (dsGetJobDetail.Table.Rows[0]["NoOfPackages"] != DBNull.Value)
                {
                    lblNoOfPackgs.Text = dsGetJobDetail.Table.Rows[0]["NoOfPackages"].ToString();
                    lblPackageType.Text = dsGetJobDetail.Table.Rows[0]["PackageTypeName"].ToString();
                }
                else
                    lblNoOfPackgs.Text = "0";
                lblSum20.Text = dsGetJobDetail.Table.Rows[0]["CountOf20"].ToString();
                lblSum40.Text = dsGetJobDetail.Table.Rows[0]["CountOf40"].ToString();
                if (dsGetJobDetail.Table.Rows[0]["NoOfPackages"] != DBNull.Value)
                    lblSum40.Text = dsGetJobDetail.Table.Rows[0]["CountOf40"].ToString();

                //if (dsGetJobDetail.Table.Rows[0]["DeliveryTypeId"] != DBNull.Value && dsGetJobDetail.Table.Rows[0]["DeliveryTypeId"].ToString() != "0")
                //{
                //    if (dsGetJobDetail.Table.Rows[0]["DeliveryTypeId"].ToString() == "1")   // Loaded
                //    {
                //        lblDispatchFor.Text = "No. of Containers dispatched";
                //    }
                //    else // other than loaded
                //    {
                //        lblDispatchFor.Text = "No. of Vehicles dispatched";
                //    }
                //}

                txtDeliveryDestination.Text = dsGetJobDetail.Table.Rows[0]["DeliveryDestination"].ToString();
                txtDimension.Text = dsGetJobDetail.Table.Rows[0]["Dimension"].ToString();
                if (dsGetJobDetail.Table.Rows[0]["CountLCL"] != DBNull.Value)
                {
                    ddDeliveryType.DataBind();
                    int CountLCL = Convert.ToInt32(dsGetJobDetail.Table.Rows[0]["CountLCL"]);
                    if (CountLCL == 1)
                    {
                        ddDeliveryType.Items.RemoveAt(1);   // Loaded
                        ddDeliveryType.Items.RemoveAt(3);   // Break Bulk
                    }
                }
              
            }
        }
        else
        {
            ResetControls();
        }
    }

    protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }

        //if (ddlExportType.SelectedValue != "0")
        //{
        //    if (Mode == 1) // Air mode
        //    {
        //        //lblDispatchFor.Text = "Number of vehicles required";
        //        rfvDispatchFor.Enabled = true;
        //        rfvDispatchFor.ErrorMessage = "Please enter number of vehicles dispatched.";
        //    }
        //    else
        //    {
        //        if (ddlExportType.SelectedValue == "1") // factory stuff
        //        {
        //            //lblDispatchFor.Text = "Number of vehicles required";
        //            rfvDispatchFor.Enabled = true;
        //            rfvDispatchFor.ErrorMessage = "Please enter number of containers dispatched.";
        //        }
        //        else // doc stuff
        //        {
        //            //lblDispatchFor.Text = "Number of vehicles required";
        //            rfvDispatchFor.Enabled = true;
        //            rfvDispatchFor.ErrorMessage = "Please enter number of vehicles dispatched.";
        //        }
        //    }
        //}
        //else
        //{
        //    //lblDispatchFor.Text = "";
        //    rfvDispatchFor.Enabled = false;
        //}
    }

    protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }


        if (ddDeliveryType.SelectedValue == "1")     //Loaded , '1' represents the Loaded option
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

        //if (ddDeliveryType.SelectedValue != "0")
        //{
        //    if (Mode == 1) // Air mode
        //    {
        //        //lblDispatchFor.Text = "Number of vehicles dispatched";
        //        rfvDispatchFor.Enabled = true;
        //        rfvDispatchFor.ErrorMessage = "Please enter number of vehicles dispatched.";
        //    }
        //    else
        //    {
        //        if (ddDeliveryType.SelectedValue == "1")   // Loaded
        //        {
        //            //lblDispatchFor.Text = "Number of containers dispatched";
        //            rfvDispatchFor.Enabled = true;
        //            rfvDispatchFor.ErrorMessage = "Please enter number of containers dispatched.";
        //        }
        //        else // other than loaded
        //        {
        //            //lblDispatchFor.Text = "Number of vehicles dispatched";
        //            rfvDispatchFor.Enabled = true;
        //            rfvDispatchFor.ErrorMessage = "Please enter number of vehicles dispatched.";
        //        }
        //    }
        //}
        //else
        //{
        //    //lblDispatchFor.Text = "";
        //    txtTotalDispatch.Text = "";
        //    rfvDispatchFor.Enabled = false;
        //}
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fuDocument.HasFile)
        {
            btnSaveDocument_Click(fuDocument, EventArgs.Empty);
        }

        DateTime dtVehiclePlaceRequireDate = DateTime.MinValue;
        int JobType = 0, TotalContainers = 0, VehicleRequired = 1, Mode = 0, DeliveryType = 0, ExportType = 0,Pincode1 = 0, Pincode2 = 0, JobId = 0, TranReqId = 0; // Added int Pincode1 and Pincode2
        string PickupAdd = "", PickState = "", PickupCity = "", DropAdd = "", DropState = "", DropCity = ""; //added string Pickup and drop 
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


        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }

        if (ddDeliveryType.SelectedValue != "0")
        {
            DeliveryType = Convert.ToInt32(ddDeliveryType.SelectedValue);
        }

        if (ddlExportType.SelectedValue != "0")
        {
            ExportType = Convert.ToInt32(ddlExportType.SelectedValue);
        }

        if (hdnJobId.Value != "" && hdnJobId.Value != "0")
        {
            if (hdnJobType.Value != "0" && hdnJobType.Value != "")
            {
                JobType = Convert.ToInt32(hdnJobType.Value);
            }

            //if (JobType == 1) // Import job
            //{
            //    if (Mode == 2) // Sea
            //    {
            //        if (DeliveryType == 1) // loaded --> container wise details
            //        {
            //            ContDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //            if (lblNoOfContainers.Text.Trim() != "")
            //            {
            //                TotalContainers = Convert.ToInt32(lblNoOfContainers.Text.Trim());
            //            }

            //        }
            //        else // other than loaded --> packages wise details
            //        {
            //            VehicleDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //        }
            //    }
            //    else // Air --> packages wise details
            //    {
            //        VehicleDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //    }
            //}
            //else // Export job
            //{
            //    if (Mode == 2) // Sea
            //    {
            //        if (ExportType == 1) // Factory Stuff --> container wise details
            //        {
            //            ContDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //        }
            //        else if (ExportType == 2) // Doc Stuff --> packages wise details
            //        {
            //            VehicleDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //        }
            //    }
            //    else // Air --> packages wise details
            //    {
            //        VehicleDispatched = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            //    }
            //}

            //if (txtTotalDispatch.Text.Trim() != "")
            //    VehicleRequired = Convert.ToInt32(txtTotalDispatch.Text.Trim());
            if (txtVehiclePlaceDate.Text.Trim() != "")
                dtVehiclePlaceRequireDate = Commonfunctions.CDateTime(txtVehiclePlaceDate.Text.Trim());


            PickupAdd = txtPickupAdd.Text;                                        //pincode ,pick, drop and loaded empty doc added changes for save operation
            PickState = txtState1.Text;
            PickupCity = txtCity1.Text;
            DropAdd = txtDropAdd.Text;
            DropState = txtState2.Text;
            DropCity = txtCity2.Text;
            EmptyLetter = loadedDocuments.FileName;
            Pincode1 = Convert.ToInt32(txtPincode1.Text);
            Pincode2 = Convert.ToInt32(txtPincode2.Text);

            EmptyDocPath = Regex.Replace(txtJobNumber.Text, "[^a-zA-Z0-9]", "");
            // EmptyDocPath = txtJobNumber.Text.Replace("[-+.^:,/-]", "");
            if (loadedDocuments != null && loadedDocuments.HasFile)
                FileName = UploadDocument(EmptyDocPath);

            int result = DBOperations.AddJobTransportRequest(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(hdnCustId.Value), txtJobNumber.Text.Trim(), DateTime.Now, 1, lblPort.Text.Trim(), txtDeliveryDestination.Text.Trim(),
                                    Convert.ToInt32(lblSum20.Text), Convert.ToInt32(lblSum40.Text), Convert.ToInt32(lblNoOfPackgs.Text), lblGrossWt.Text.Trim(), txtRemark.Text.Trim(), JobType, DeliveryType, ExportType, txtDimension.Text.Trim(),
                                    dtVehiclePlaceRequireDate, VehicleRequired, loggedInUser.glUserId);      //  changes about pickaup and drop by using Pincode1,Pincode2 and loaded documents

            //int results = DBOperations.AddTransAddDetails(Convert.ToInt32(hdnJobId.Value), result, FileName, EmptyDocPath + "\\", 118, PickupAdd, Pincode1, PickState, PickupCity, DropAdd, Pincode2, DropState, DropCity, EmptyLetter, loggedInUser.glUserId);


            if (result > 0)
            {
                // Add packing list documents
                if (Convert.ToString(ViewState["PackingList"]) != "")
                {
                    DataTable dtPackingList = (DataTable)ViewState["PackingList"];
                    if (dtPackingList != null && dtPackingList.Rows.Count > 0)
                    {
                        string DocPath = "";
                        for (int i = 0; i < dtPackingList.Rows.Count; i++)
                        {
                            if (dtPackingList.Rows[i]["DocPath"] != null)
                                DocPath = dtPackingList.Rows[i]["DocPath"].ToString();
                            int result_Doc = DBOperations.AddPackingListDocs(result, DocPath, loggedInUser.glUserId);
                        }
                    }
                }


                if (hdnJobId.Value != "" && hdnJobId.Value != "0")
                {
                    int TransportBabaji = DBOperations.TR_updJobTransportBabaji(Convert.ToInt32(hdnJobId.Value), loggedInUser.glUserId);
                }
                Response.Redirect("SuccessPage.aspx?Request=" + result.ToString());
                //Response.Redirect("SuccessPage.aspx?Request=" + lblTransRefNo.Text.Trim());
                //ResetControls();
                //lblError.Text = "Successfully added truck request - " + lblTransRefNo.Text.Trim() + ". Job moved to request received tab." ;
                //lblError.CssClass = "success";
                //lblTransRefNo.Text = DBOperations.GetNewTransportNo();
            }
            else if (result == -2)
            {
                lblError.Text = "Truck request already exists! Please complete delivery for existing request first.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == -3)
            {
                lblError.Text = "No of containers to be dispatched exceed available containers.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding truck request. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void ResetControls()
    {
        txtJobNumber.Text = "";
        hdnJobId.Value = "0";
        txtDeliveryDestination.Text = "";
        txtDimension.Text = "";
        //txtTotalDispatch.Text = "";
        txtVehiclePlaceDate.Text = "";
        lblBranch.Text = "";
        lblConsignee.Text = "";
        lblCustomer.Text = "";
        //lblDispatchFor.Text = "";
        lblNoOfContainers.Text = "";
        lblNoOfPackgs.Text = "";
        lblPackageType.Text = "";
        lblSum20.Text = "";
        lblSum40.Text = "";
        lblError.Text = "";
        lblShipper.Text = "";
        ViewState["PackingList"] = "";
        rptDocument.DataSource = "";
        rptDocument.DataBind();
    }

    #region DOCUMENT UPLOAD --> PACKING LIST

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (fuDocument != null && fuDocument.HasFile)
            fileName = UploadFiles(fuDocument);
        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["PackingList"];
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

            dtAnnexure.Rows.Add(PkId, fileName, fuDocument.FileName, loggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["PackingList"] = dtAnnexure;
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

    protected void rptDocument_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["PackingList"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["PackingList"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblError.Text = "Successfully Deleted Document.";
                lblError.CssClass = "success";
                rptDocument.DataBind();
            }
            else
            {
                lblError.Text = "Error while deleting document. Please try again later..!!";
                lblError.CssClass = "errorMsg";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadDoc(FilePath);
        }
    }

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Transport\\" + FilePath);
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
            return FilePath + FileName;
        }
        else
        {
            return "";
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + DocumentPath);
        else
            ServerPath = ServerPath + DocumentPath;
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    protected void BindGrid()
    {
        if (ViewState["PackingList"].ToString() != "")
        {
            DataTable dtPackingList = (DataTable)ViewState["PackingList"];
            rptDocument.DataSource = dtPackingList;
            rptDocument.DataBind();
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

    #endregion

    //protected void txtPincode1_TextChanged(object sender, EventArgs e)                    //Get State and city by searching pincode From DataBase
    //{

    //    int PincodeId = 0;
    //    if (txtPincode1.Text.Trim() == "")
    //        hdnPincodeId.Value = "0";
    //    else
    //       PincodeId = Convert.ToInt32(txtPincode1.Text.Trim());

    //    if (PincodeId > 0)
    //    {
    //        DataView dsGetpincode = DBOperations.GetPincodebysearching(PincodeId);
    //        if (dsGetpincode != null && dsGetpincode.Count > 0)
    //        {
    //            txtState1.Text = dsGetpincode[0]["State"].ToString();
    //            txtCity1.Text = dsGetpincode[0]["City"].ToString();
    //        }
    //        else
    //        {

    //            txtState1.Text = "State Not Found";
    //            txtCity1.Text = "City Not Found";
    //        }
    //    }
    //    else
    //    {

    //        txtState1.Text = "Invalid Pincode";
    //        txtCity1.Text = "Invalid Pincode";
    //    }

    //}
    //protected void txtPincode2_TextChanged(object sender, EventArgs e)    ////Get State and city by searching Pincode From Database
    //{

    //    int PincodeId = 0;
    //    if (txtPincode2.Text.Trim() == "")
    //        hdnpinid.Value = "0";
    //    else
    //     //   PincodeId = Convert.ToInt32(txtPincode2.Text.Trim());

    //    if (PincodeId > 0)
    //    {
    //        DataView dsGetpincode = DBOperations.GetPincodebysearching(PincodeId);
    //        if (dsGetpincode != null && dsGetpincode.Count > 0)
    //        {
    //            txtState2.Text = dsGetpincode[0]["State"].ToString();
    //            txtCity2.Text = dsGetpincode[0]["City"].ToString();
    //        }
    //        else
    //        {
    //            txtState2.Text = "State Not Found";
    //            txtCity2.Text = "City Not Found";
    //        }
    //    }
    //    else
    //    {
    //        txtState2.Text = "Invalid Pincode";
    //        txtCity2.Text = "Invalid Pincode";
    //    }
    //}
    protected void txtPincode1_TextChanged(object sender, EventArgs e)
    {
       // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

        //for .Net framework 4.5
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //for .Net framework 4.0
     //   System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

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
            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (var httpClient = new HttpClient(handler))
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
            // Log detailed error information
            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {httpEx.StackTrace}");
            txtState1.Text = "Error fetching state information";
            txtCity1.Text = "Error fetching city information";
        }
        catch (Exception ex)
        {
            // Log detailed error information
            System.Diagnostics.Debug.WriteLine($"General Error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            txtState1.Text = ex.ToString();
            txtCity1.Text = ex.ToString();
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
            txtState2.Text = ex.ToString();
            txtCity2.Text = ex.ToString();
        }
    }
    #region Empty letter 
    private string UploadDocument(string FilePath)
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
    #endregion
}