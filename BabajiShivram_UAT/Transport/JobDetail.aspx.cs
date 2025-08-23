using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using BSImport.CountryManager.BO;

public partial class Transport_JobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(fvDispatchBilling);
        ScriptManager1.RegisterPostBackControl(btnAddContainer);
        ScriptManager1.RegisterPostBackControl(gvTruckRequest);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        ScriptManager1.RegisterPostBackControl(btnMemoSubmit);
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);
        if (Session["TRId"] == null)
        {
            Response.Redirect("JobTracking.aspx");
        }

        if (!IsPostBack)
        {
            TruckRequestDetail(Convert.ToInt32(Session["TRId"]));
        }

        // Allow  Only Future Date For Reminder
        calVehiclePlaceDate.StartDate = DateTime.Today;

        // Set the minimum value for the MaskedEditValidator to today's date
        mevVehiclePlaceDate.MinimumValue = DateTime.Today.ToString("dd/MM/yyyy");
    }

    protected void TruckRequestDetail(int TransportId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TransportId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            if (dvDetail.Table.Rows[0]["JobId"] != DBNull.Value)
            {
                int JobIDDelivery = Convert.ToInt32(dvDetail.Table.Rows[0]["JobId"]);
                Session["TrJobId"] = JobIDDelivery;
                JobDetailMS(JobIDDelivery);
            }

            ddContainerType_SelectedIndexChanged(null, EventArgs.Empty);
            btnSendToScrutiny.Visible = false;
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Detail - " + lblTRRefNo.Text;
            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblNoofPkg.Text = dvDetail.Table.Rows[0]["NoOfPackages"].ToString();
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            if (lblJobNo.Text.StartsWith("TS"))
            {
                fsContainerDetails.Visible = true;
            }
            else
            {
                fsContainerDetails.Visible = false;
            }
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            lblDivision.Text = dvDetail.Table.Rows[0]["Division"].ToString();
            lblPlant.Text = dvDetail.Table.Rows[0]["Plant"].ToString();
            //if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
            //    lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();

            lblPickAdd.Text = dvDetail.Table.Rows[0]["PickUpAddress"].ToString();
            lblDropAdd.Text = dvDetail.Table.Rows[0]["DropAddress"].ToString();
            lblpickPincode.Text = dvDetail.Table.Rows[0]["PickupPincode"].ToString();                  //added new pickup and drop pincode city and state for transport request detail job tracking
            lblpickState.Text = dvDetail.Table.Rows[0]["PickupState"].ToString();
            lblpickCity.Text = dvDetail.Table.Rows[0]["PickupCity"].ToString();
            lblDropPincode.Text = dvDetail.Table.Rows[0]["DropPincode"].ToString();
            lblDropState.Text = dvDetail.Table.Rows[0]["DropState"].ToString();
            lblDropCity.Text = dvDetail.Table.Rows[0]["DropCity"].ToString();

            lblDeliveryType.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            if (dvDetail.Table.Rows[0]["DocFolder"] != DBNull.Value)
                strCustDocFolder = dvDetail.Table.Rows[0]["DocFolder"].ToString() + "\\";
            if (dvDetail.Table.Rows[0]["FileDirName"] != DBNull.Value)
                strJobFileDir = dvDetail.Table.Rows[0]["FileDirName"].ToString() + "\\";
            if (Convert.ToBoolean(dvDetail.Table.Rows[0]["MovementCompleted"]) == true) // 1
            {
                btnSendToScrutiny.Visible = true;
                if (gvbillingscrutiny != null)
                {
                    if (gvbillingscrutiny.Rows.Count > 0)
                    {
                        btnSendToScrutiny.Visible = false;
                    }
                }

                // bind dispatch grid
                DataSet dsPCDDetail = DBOperations.TR_GetPCDDetail(TransportId);
                if (dsPCDDetail.Tables[0].Rows.Count > 0)
                {
                    fvDispatchBilling.DataSource = dsPCDDetail;
                    fvDispatchBilling.DataBind();

                    int BillingDeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["BillingDeliveryId"]);
                    int PCADeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["PCADeliveryId"]);

                    Panel pnlDispatchBillingHand = (Panel)fvDispatchBilling.FindControl("pnlDispatchBillingHand");
                    Panel pnlDispatchBillingCour = (Panel)fvDispatchBilling.FindControl("pnlDispatchBillingCour");
                    Button btnEditDispatchBilling = (Button)this.fvDispatchBilling.FindControl("btnEditDispatchBilling");

                    // Dispatch - Billing
                    if (BillingDeliveryId == 1)
                    {
                        if (pnlDispatchBillingHand != null)
                            pnlDispatchBillingHand.Visible = true;
                    }
                    else if (BillingDeliveryId == 2)
                    {
                        if (pnlDispatchBillingCour != null)
                            pnlDispatchBillingCour.Visible = true;
                    }

                    if (PCADeliveryId == 0)
                    {
                        if (btnEditDispatchBilling != null)
                        {
                            btnEditDispatchBilling.Visible = false;
                        }
                    }
                }
            }
        }
    }

    protected void gvVehicleDailyStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "dailystatus")
        {
            string strCustomerEmail = "", strVehicleNo = "", strVehicleType = "", strDeliveryFrom = "", strDeliveryTo = "", strDispatchDate = "", strTransReqId = "",
                    strJobRefNo = "", strCustomer = "", strCustRefNo = "", strCurrentStatus = "", strStatusCreatedBy = "", EmailContent = "", MessageBody = "", strEmailTo = "", strEmailCC = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            strVehicleNo = commandArgs[0].ToString();
            strVehicleType = commandArgs[1].ToString();
            strDeliveryFrom = commandArgs[2].ToString();
            strDeliveryTo = commandArgs[3].ToString();
            strDispatchDate = commandArgs[4].ToString();
            strCustomerEmail = commandArgs[5].ToString();
            strTransReqId = commandArgs[6].ToString();
            strJobRefNo = commandArgs[7].ToString();
            strCustomer = commandArgs[8].ToString();
            strCustRefNo = commandArgs[9].ToString();
            strCurrentStatus = commandArgs[10].ToString();
            strStatusCreatedBy = commandArgs[11].ToString();
            strEmailTo = commandArgs[12].ToString();
            strEmailCC = commandArgs[13].ToString();

            txtSubject.Text = "Daily Status for Job No " + strJobRefNo + " Vehicle No " + strVehicleNo + " and Dispatch Date " + strDispatchDate;
            try
            {
                string strFileName = "../EmailTemplate/TransportStatus.txt";
                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
                return;
            }

            lblCustomerEmail.Text = strEmailTo;
            txtMailCC.Text = strEmailCC;
            MessageBody = EmailContent.Replace("@DailyStatus", strCurrentStatus);
            MessageBody = MessageBody.Replace("@JobRefNo", strJobRefNo);
            MessageBody = MessageBody.Replace("@Customer", strCustomer);
            MessageBody = MessageBody.Replace("@CustRefNo", strCustRefNo);
            MessageBody = MessageBody.Replace("@VehicleNo", strVehicleNo);
            MessageBody = MessageBody.Replace("@DispatchDate", strDispatchDate);
            MessageBody = MessageBody.Replace("@VehicleType", strVehicleType);
            MessageBody = MessageBody.Replace("@DeliveryFrom", strDeliveryFrom);
            MessageBody = MessageBody.Replace("@DeliveryTo", strDeliveryTo);
            MessageBody = MessageBody.Replace("@EmpName", strStatusCreatedBy);
            divPreviewEmail.InnerHtml = MessageBody;
            mpeDailyStatus.Show();
        }
    }

    protected void btnCancelEmailPp_Click(object sender, EventArgs e)
    {
        mpeDailyStatus.Hide();
        gvVehicleDailyStatus.DataBind();
    }

    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpeDailyStatus.Hide();
        gvVehicleDailyStatus.DataBind();
    }

    #region DELIVERY DETAIL

    protected void GridViewDelivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");
        }

        if ((e.Row.RowState == DataControlRowState.Edit) || (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
        {
            if (DataBinder.Eval(e.Row.DataItem, "TransporterID") != DBNull.Value)
            {
                DropDownList ddTransporter = (DropDownList)e.Row.FindControl("ddTransporter");
                DropDownList ddVehicleNo = (DropDownList)e.Row.FindControl("ddVehicleNo");
                TextBox txtVehicleNo = (TextBox)e.Row.FindControl("txtVehicleNo");

                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TransporterID")) == 524 ||
                    Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TransporterID")) == 17304) // navbharat or navjeevan transporter
                {
                    if (ddTransporter != null)
                    {
                        DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
                        ddTransporter.SelectedValue = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TransporterID"));
                        if (ddTransporter.SelectedValue != "0")
                        {
                            if (ddTransporter.SelectedValue == "524") // NAVBHARAT TRANSPORTER
                            {
                                DBOperations.FillVehicleForNavbharat(ddVehicleNo);
                                ddVehicleNo.Visible = true;
                                txtVehicleNo.Visible = false;
                            }
                            else if (ddTransporter.SelectedValue == "17304") // NAVJEEVAN AGENCY Transporter
                            {
                                DBOperations.FillVehicleForNAVJEEVAN(ddVehicleNo);
                                ddVehicleNo.Visible = false;
                                txtVehicleNo.Visible = true;
                            }
                            else
                            {
                                ddVehicleNo.Visible = false;
                                txtVehicleNo.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    DBOperations.FillCompanyByCategory(ddTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
                    ddTransporter.SelectedValue = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TransporterID"));
                    if (ddTransporter.SelectedValue != "0")
                    {
                        ddVehicleNo.Visible = false;
                        txtVehicleNo.Visible = true;
                    }
                }
            }
        }
    }

    protected void GridViewDelivery_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        int vehicleTypeId = 0;
        string vehiclename = "";


        if (e.CommandName.ToLower() == "update")
        {
            int NoOfPackages = 0;

            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewDelivery.DataKeys[gvrow.RowIndex].Value.ToString();
            DataSourceDelivery.UpdateParameters["DeliveryId"].DefaultValue = strlId;

            // Update Delivered Package Except Loaded Shipment
            TextBox txtDeliveredPackages = (TextBox)gvrow.FindControl("txtDeliveredPackages");

            if (txtDeliveredPackages.Text.Trim() != "")
            {
                NoOfPackages = Convert.ToInt32(txtDeliveredPackages.Text.Trim());
                DataSourceDelivery.UpdateParameters["NoOfPackages"].DefaultValue = NoOfPackages.ToString();
            }

            DropDownList ddTransporter = (DropDownList)gvrow.FindControl("ddTransporter");
            int TransporterId = Convert.ToInt32(ddTransporter.SelectedValue);
            DataSourceDelivery.UpdateParameters["TransporterId"].DefaultValue = TransporterId.ToString();
            DataSourceDelivery.UpdateParameters["TransporterName"].DefaultValue = ddTransporter.SelectedItem.Text.Trim();

            if (TransporterId == 524 || TransporterId == 17304) // navbharat 
            {
                DropDownList ddVehicleNo = (DropDownList)gvrow.FindControl("ddVehicleNo");
                if (ddVehicleNo != null)
                {
                    DataSourceDelivery.UpdateParameters["VehicleNo"].DefaultValue = Convert.ToString(ddVehicleNo.SelectedItem.Text.Trim());
                }
            }
            else
            {
                TextBox txtVehicleNo = (TextBox)gvrow.FindControl("txtVehicleNo");
                DataSourceDelivery.UpdateParameters["VehicleNo"].DefaultValue = txtVehicleNo.Text.ToUpper().Trim();
            }

            DataSourceDelivery.Update();

            //lblError.Visible = true;
            //lblError.Text = "Delivery Detail Updated Successfully!";
            //GridViewDelivery.EditIndex = -1;

            /*********************************************
            int NoOfPackages = 0;

            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewDelivery.DataKeys[gvrow.RowIndex].Value.ToString();
            DataSourceDelivery.UpdateParameters["lId"].DefaultValue = strlId;

            // Update Delivered Package Except Loaded Shipment
            TextBox txtDeliveredPackages = (TextBox)gvrow.FindControl("txtDeliveredPackages");

            if (txtDeliveredPackages.Text.Trim() != "")
            {
                NoOfPackages = Convert.ToInt32(txtDeliveredPackages.Text.Trim());
                DataSourceDelivery.UpdateParameters["NoOfPackages"].DefaultValue = NoOfPackages.ToString();
            }

            TextBox txtDeliveryDate = (TextBox)gvrow.FindControl("txtDeliveryDate");
            TextBox txtEmptyContainerDate = (TextBox)gvrow.FindControl("txtEmptyContainerDate");
            TextBox txtVehicleRcvdDate = (TextBox)gvrow.FindControl("txtVehicleRcvdDate");
            TextBox txtLRDate = (TextBox)gvrow.FindControl("txtLRDate");
            TextBox txtDispatchDate = (TextBox)gvrow.FindControl("txtDispatchDate");
            TextBox txtRoadPermitDate = (TextBox)gvrow.FindControl("txtRoadPermitDate");
            TextBox txtBabajiChallanDate = (TextBox)gvrow.FindControl("txtChallanDate");

            string strDeliveryDate = txtDeliveryDate.Text.Trim();
            string strEmptyContainerDate = txtEmptyContainerDate.Text.Trim();
            string strVehicleRcvdDate = txtVehicleRcvdDate.Text.Trim();
            string strLRDate = txtLRDate.Text.Trim();
            string strDispatchDate = txtDispatchDate.Text.Trim();
            string strBabajiChallanDate = txtBabajiChallanDate.Text.Trim();

            DropDownList ddTransporter = (DropDownList)gvrow.FindControl("ddTransporter");
            int TransporterId = Convert.ToInt32(ddTransporter.SelectedValue);
            DataSourceDelivery.UpdateParameters["TransporterId"].DefaultValue = TransporterId.ToString();
            DataSourceDelivery.UpdateParameters["TransporterName"].DefaultValue = ddTransporter.SelectedItem.Text.Trim();

            if (TransporterId == 524 || TransporterId == 17304) // navbharat 
            {
                DropDownList ddVehicleNo = (DropDownList)gvrow.FindControl("ddVehicleNo");
                if (ddVehicleNo != null)
                {
                    DataSourceDelivery.UpdateParameters["VehicleNo"].DefaultValue = Convert.ToString(ddVehicleNo.SelectedItem.Text.Trim());
                }
            }
            else
            {
                TextBox txtVehicleNo = (TextBox)gvrow.FindControl("txtVehicleNo");
                DataSourceDelivery.UpdateParameters["VehicleNo"].DefaultValue = txtVehicleNo.Text.ToUpper().Trim();
            }

            DropDownList ddVehicleType1 = (DropDownList)gvrow.FindControl("ddVehicleType1");
            if (ddVehicleType1.SelectedValue != "0")
            {
                vehicleTypeId = Convert.ToInt32(ddVehicleType1.SelectedValue);
                vehiclename = ddVehicleType1.SelectedItem.Text;
                DataSourceDelivery.UpdateParameters["VehicleType"].DefaultValue = Convert.ToString(vehicleTypeId);
            }
            else
            {
                DataSourceDelivery.UpdateParameters["VehicleType"].DefaultValue = "-Select-";
                return;
            }

            if (strDeliveryDate != "")
            {
                strDeliveryDate = Commonfunctions.CDateTime(strDeliveryDate).ToShortDateString();
                DataSourceDelivery.UpdateParameters["DeliveryDate"].DefaultValue = strDeliveryDate;
            }

            if (strEmptyContainerDate != "")
            {
                strEmptyContainerDate = Commonfunctions.CDateTime(strEmptyContainerDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["EmptyContRetrunDate"].DefaultValue = strEmptyContainerDate;
            }

            if (strVehicleRcvdDate != "")
            {
                strVehicleRcvdDate = Commonfunctions.CDateTime(strVehicleRcvdDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["VehicleRcvdDate"].DefaultValue = strVehicleRcvdDate;
            }

            if (strLRDate != "")
            {
                strLRDate = Commonfunctions.CDateTime(strLRDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["LRDate"].DefaultValue = strLRDate;
            }

            if (strDispatchDate != "")
            {
                strDispatchDate = Commonfunctions.CDateTime(strDispatchDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["DispatchDate"].DefaultValue = strDispatchDate;
            }

            if (strBabajiChallanDate != "")
            {
                strBabajiChallanDate = Commonfunctions.CDateTime(strBabajiChallanDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["BabajiChallanDate"].DefaultValue = strBabajiChallanDate;
            }

            TextBox txtCargoRecvdby = (TextBox)gvrow.FindControl("txtCargoRecvdby");
            string CargoRecvdby = txtCargoRecvdby.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["CargoReceivedby"].DefaultValue = CargoRecvdby;

            TextBox txtLRNo = (TextBox)gvrow.FindControl("txtLRNo");
            string LRNo = txtLRNo.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["LRNo"].DefaultValue = LRNo;

            TextBox txtDeliveryPoint = (TextBox)gvrow.FindControl("txtDeliveryPoint");
            string DeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["DeliveryPoint"].DefaultValue = DeliveryPoint;

            TextBox txtChallanNo = (TextBox)gvrow.FindControl("txtChallanNo");
            string ChallanNo = txtChallanNo.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["BabajiChallanNo"].DefaultValue = ChallanNo;

            TextBox txtRemark = (TextBox)gvrow.FindControl("txtRemark");

            string PODPath = "";
            FileUpload fuattach = (FileUpload)gvrow.FindControl("fuPODAttchment");
            HiddenField hdnPODPath = (HiddenField)gvrow.FindControl("hdnPODPath");

            string FileName = fuattach.FileName;
            FileName = FileServer.ValidateFileName(FileName);

            if (fuattach.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = ""; //hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "PODFiles\\";

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
                    System.IO.Directory.CreateDirectory(ServerFilePath);
                }

                if (fuattach.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuattach.SaveAs(ServerFilePath + FileName);
                }

                PODPath = FilePath + FileName;
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }
            else if (hdnPODPath.Value != "")
            {
                PODPath = hdnPODPath.Value;
                DataSourceDelivery.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }
            //

            string BCCPath = "";
            FileUpload fuBccattach = (FileUpload)gvrow.FindControl("fuBCCAttchment");
            HiddenField hdnBCCPath = (HiddenField)gvrow.FindControl("hdnBCCPath");

            string FileName1 = fuBccattach.FileName;
            FileName = FileServer.ValidateFileName(FileName1);

            if (fuBccattach.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = "";// hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "PODFiles\\";

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
                    System.IO.Directory.CreateDirectory(ServerFilePath);
                }

                if (fuBccattach.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuBccattach.SaveAs(ServerFilePath + FileName);
                }

                BCCPath = FilePath + FileName;
                DataSourceDelivery.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath;
            }
            else if (hdnBCCPath.Value != "")
            {
                BCCPath = hdnBCCPath.Value;
                DataSourceDelivery.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath;
            }

            TextBox txtdrivername = (TextBox)gvrow.FindControl("txtdrivername");
            string strdrivername = txtdrivername.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["DriverName"].DefaultValue = strdrivername;

            TextBox txtdriverphoneno = (TextBox)gvrow.FindControl("txtdriverphoneno");
            string strdriverphoneno = txtdriverphoneno.Text.ToUpper().Trim();
            DataSourceDelivery.UpdateParameters["DriverPhoneno"].DefaultValue = strdriverphoneno;

            DataSourceDelivery.Update();

            ****************************************/
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            lblError.Visible = false;
            lblError.Text = "";
            GridViewDelivery.EditIndex = -1;
        }

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadPODDocument(DocPath);
        }

        if (e.CommandName.ToLower() == "delete")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int lid = Convert.ToInt32(GridViewDelivery.DataKeys[gvrow.RowIndex].Value.ToString());
            int Result = -123;

            Result = DBOperations.TR_DeleteDeliveryDetail(lid, LoggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Delivery to Customer Deleted Successfully!";
                lblError.CssClass = "success";
                GridViewDelivery.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = "Job Details Not Found!";
            }
        }
    }

    protected void DataSourceDelivery_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        System.Data.Common.DbParameterCollection CmdParams = e.Command.Parameters;
        ParameterCollection UpdParams = ((SqlDataSourceView)sender).UpdateParameters;

        //Hashtable ht = new Hashtable();
        //foreach (Parameter UpdParam in UpdParams)
        //    ht.Add(UpdParam.Name, true);

        //for (int i = 0; i < CmdParams.Count; i++)
        //{
        //    if (!ht.Contains(CmdParams[i].ParameterName.Substring(1)))
        //        CmdParams.Remove(CmdParams[i--]);
        //}

    }

    protected void DataSourceDelivery_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

        if (Result == 0)
        {
            lblDeliveryMsg.Text = "Delivery Detail Updated Successfully";
            lblDeliveryMsg.CssClass = "success";
            GridViewDelivery.DataBind();
        }
        else if (Result == 1)
        {
            lblDeliveryMsg.Text = "System Error! Please try after sometime!";
            lblDeliveryMsg.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblDeliveryMsg.Text = "Please Check No Of Delivered Packages. Supply Count Exceed Available Packages!";
            lblDeliveryMsg.CssClass = "errorMsg";
            //GridViewDelivery.DataBind();
            //return;

        }
        else if (Result == 3)
        {
            lblDeliveryMsg.Text = "Enter Vehicle not placed!";
            lblDeliveryMsg.CssClass = "errorMsg";
        }
        else if (Result == 4)
        {
            lblDeliveryMsg.Text = "Fund request already process, first reject this fund request!";
            lblDeliveryMsg.CssClass = "errorMsg";
        }
        else
        {
            lblDeliveryMsg.Text = "System Error! Please try after sometime!";
            lblDeliveryMsg.CssClass = "errorMsg";
        }

    }

    #endregion

    #region SELLING RATE
    protected void gvSellingDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gvSellingDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion

    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument(TransRequestId);
        }
    }

    private void DownloadDocument(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "TransportFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion

    #region CONTAINER DETAILS EVENTS

    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        int PkId = 1, count = 0;
        lblError.Visible = true;
        int JobId = Convert.ToInt32(Session["JobId"]);
        string ContainerSize = "", ContainerType = "", SealNo = "";
        int ContainerSizeId = 0, ContainerTypeId = 0;

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerTypeId = Convert.ToInt32(ddContainerType.SelectedValue);
        ContainerSizeId = Convert.ToInt32(ddContainerSize.SelectedValue);

        if (ContainerTypeId == 1) //FCL
        {
            if (ContainerSizeId == 0)
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
            if (ddContainerSize.SelectedValue == "0")
                ContainerSize = "";
            else
                ContainerSize = ddContainerSize.SelectedItem.Text.Trim();
            ContainerType = ddContainerType.SelectedItem.Text.Trim();

            int result = DBOperations.AddContainerDetailTR(Convert.ToInt32(Session["TRId"]), txtContainerNo.Text.Trim(), ContainerSizeId, ContainerTypeId, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added container.";
                lblError.CssClass = "success";
                gvContainer.DataBind();
            }
            else if (result == 2)
            {
                lblError.Text = "Container already exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System error! Please try again later.";
                lblError.CssClass = "errorMsg";
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

    protected void btnCancelContainer_Click(object sender, EventArgs e)
    {
        txtContainerNo.Text = "";
        ddContainerSize.SelectedValue = "0";
        ddContainerType.SelectedValue = "1";
        gvContainer.DataBind();
    }

    protected void gvContainer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "deleterow")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = DBOperations.DeleteContainerDetailTR(lid, LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted container.";
                    lblError.CssClass = "success";
                    txtContainerNo.Text = "";
                    ddContainerSize.SelectedValue = "0";
                    ddContainerType.SelectedValue = "1";
                    gvContainer.DataBind();
                    gvContainer.DataBind();
                }
                else if (result == 2)
                {
                    lblError.Text = "Container does not exists!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System error! Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    #endregion

    #region DOCUMENTS DOWNLOAD

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

    private void DownloadPODDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    #endregion

    #region BILLING
    protected void gvjobexpenseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int64 lblDEBITAMT = Convert.ToInt64(e.Row.Cells[6].Text);
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + Convert.ToInt64(lblDEBITAMT);

            Int64 lblCREDITAMT = Convert.ToInt64(e.Row.Cells[7].Text);
            ViewState["lblCREDITAMT"] = Convert.ToInt64(ViewState["lblCREDITAMT"]) + Convert.ToInt64(lblCREDITAMT);


            Int64 lblAMOUNT = Convert.ToInt64(e.Row.Cells[8].Text);
            ViewState["lblAMOUNT"] = Convert.ToInt64(ViewState["lblAMOUNT"]) + Convert.ToInt64(lblAMOUNT);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[6].Text = ViewState["lblDEBITAMT"].ToString();
            e.Row.Cells[7].Text = ViewState["lblCREDITAMT"].ToString();
            e.Row.Cells[8].Text = ViewState["lblAMOUNT"].ToString();
        }
    }

    protected void gvbillingscrutiny_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string freightjobno = e.Row.Cells[8].Text;
            if (freightjobno != "&nbsp;")
            {
                lblfreight.Text = "<center><b>This Job Billed in Freight JobNo. " + freightjobno + "</b></center>";
                lblfreight.CssClass = "success";
                DraftInvoice.Visible = false;
                DraftCheck.Visible = false;
                FinalInvoiceCheck.Visible = false;
                FinalInvoiceTyping.Visible = false;
                Billdispatch.Visible = false;
                BillRejection.Visible = false;
            }
        }
    }

    protected void gvDraftInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string Consolidatedjobno = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Consolidatedjobno = e.Row.Cells[8].Text;
            if (Consolidatedjobno != "&nbsp;")
            {
                e.Row.Cells[9].Visible = false;
                lblConsolidated.Text = "<center><b>This job Consolidated With JobNo. " + Consolidatedjobno + "</b></center> ";
                lblConsolidated.CssClass = "success";
                DraftCheck.Visible = false;
                FinalInvoiceCheck.Visible = false;
                FinalInvoiceTyping.Visible = false;
                Billdispatch.Visible = false;
                BillRejection.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Consolidatedjobno != "&nbsp;")
            {
                e.Row.Cells[9].Visible = false;
            }
        }
    }

    protected void gvDraftInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "draftinvoicenext")
        {
            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = BillingOperation.TR_DraftInvoicejobmovetoDraftcheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Draft Check!.";
                lblError.CssClass = "success";
                gvDraftInvoice.DataBind();
                gvDraftcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Draft Invoice!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblError.Text = "Draft Invoice Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvFinaltyping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "finaltypingnext")
        {

            int result = 0;
            lblError.Text = "";
            int JobId = Convert.ToInt32(Session["JobId"]);

            result = BillingOperation.TR_FinalTypingjobmovetoFinalcheck(JobId);
            if (result == 0)
            {
                lblError.Text = "Job Moved To Final Draft Check!.";
                lblError.CssClass = "success";
                gvFinaltyping.DataBind();
                gvfinalcheck.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Job Already Moved!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblError.Text = "Please First Receive File for Final typing!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 4)
            {
                lblError.Text = "Final typing Date not Punch in FA!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnSendToScrutiny_Click(object sender, EventArgs e)
    {
        if (Session["TRId"] != null)
        {
            int Backoffice = DBOperations.TR_MoveToBackOffice(Convert.ToInt32(Session["TRId"]), LoggedInUser.glUserId);
            int Scrutiny = DBOperations.TR_MoveToScrutiny(Convert.ToInt32(Session["TRId"]), LoggedInUser.glUserId);
            if (Scrutiny == 0)
            {
                lblMessage.Text = "Job Forwarded For Scrutiny!";
                lblMessage.CssClass = "success";
            }//END_IF
            else if (Scrutiny == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    #endregion BILLING

    #region BILLING REPOSITORY

    protected void BindBillingDocFromRepository(string JobRefNo)
    {
        // CB00016-CNAI-18-19_20180521022448.pdf

        fsRepository.Visible = true;

        string searchPattern = JobRefNo.Remove(0, 2);  //  Job XML File

        searchPattern = searchPattern.Replace("/", "-");

        searchPattern = searchPattern + "*";

        //lblBillMsg.Text = searchPattern;
        //  string RemoteServerPath = @"\\192.168.6.116\f$\Babaji-shares\BS-Scan Document\";

        string RemoteServerPath = @"\\\\babaji-storage\\BS-Scan Document\";

        try
        {
            DirectoryInfo di = new DirectoryInfo(RemoteServerPath);

            // Get List of Billing Document

            var fileList = di.GetFiles(searchPattern, SearchOption.AllDirectories);

            gvBillingRepository.DataSource = fileList;

            gvBillingRepository.DataBind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void gvBillingRepository_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "downloadrepo")
        {
            string DocPath = e.CommandArgument.ToString();

            DownloadBillingRepo(DocPath);
        }

    }

    protected void DownloadBillingRepo(string DocumentPath)
    {
        String BIllingServerPath = DocumentPath;

        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, BIllingServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region BILLING_DISPATCH_DEPT

    protected void btnEditDispatchBilling_Click(object sender, EventArgs e)
    {
        fvDispatchBilling.ChangeMode(FormViewMode.Edit);

        if (Session["JobId"] != null)
        {
            GetPCDDetail(Convert.ToInt32(Session["JobId"]));
        }
    }

    private void GetPCDDetail(int JobId)
    {
        DataSet dsPCDDetail = DBOperations.GetPCDDetail(JobId);
        if (dsPCDDetail.Tables[0].Rows.Count > 0)
        {
            fvDispatchBilling.DataSource = dsPCDDetail;
            fvDispatchBilling.DataBind();

            int BillingDeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["BillingDeliveryId"]);
            int PCADeliveryId = Convert.ToInt32(dsPCDDetail.Tables[0].Rows[0]["PCADeliveryId"]);

            Panel pnlDispatchBillingHand = (Panel)fvDispatchBilling.FindControl("pnlDispatchBillingHand");
            Panel pnlDispatchBillingCour = (Panel)fvDispatchBilling.FindControl("pnlDispatchBillingCour");

            Panel pnlEditDispatchBillingHand = (Panel)fvDispatchBilling.FindControl("pnlEditDispatchBillingHand");
            Panel pnlEditDispatchBillingCour = (Panel)fvDispatchBilling.FindControl("pnlEditDispatchBillingCour");

            Button btnEditDispatchBilling = (Button)this.fvDispatchBilling.FindControl("btnEditDispatchBilling");

            if (BillingDeliveryId == 1)//Hand Delivery
            {
                if (pnlEditDispatchBillingHand != null)
                    pnlEditDispatchBillingHand.Visible = true;

                else if (pnlDispatchBillingHand != null)
                    pnlDispatchBillingHand.Visible = true;
            }
            else if (BillingDeliveryId == 2)//Courier delivery
            {
                // PCA To Billing Dept
                if (pnlEditDispatchBillingCour != null)
                    pnlEditDispatchBillingCour.Visible = true;
                else if (pnlDispatchBillingCour != null)
                    pnlDispatchBillingCour.Visible = true;

            }
            else if (BillingDeliveryId == 0)
            {
                if (btnEditDispatchBilling != null)
                {
                    btnEditDispatchBilling.Visible = false;
                }
            }
        }
    }

    protected void lnkPODCopyDownoad_Click(object sender, EventArgs e)
    {
        LinkButton lnkPODDownload = (LinkButton)sender;
        string FilePath = lnkPODDownload.CommandArgument;
        DownloadDocument(FilePath);
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    public string GetBooleanToCompletedPending(object myValue)
    {
        string strReturnText = "";
        if (myValue == DBNull.Value)
        {
            strReturnText = "";
        }
        else if (Convert.ToBoolean(myValue) == true)
        {
            strReturnText = "Completed";
        }
        else if (Convert.ToBoolean(myValue) == false)
        {
            strReturnText = "Pending";
        }
        return strReturnText;
    }

    protected void btnUpdateDispatchBilling_Click(object sender, EventArgs e)
    {
        bool PCDToCustomer = false;
        UpdateDispatchCourier(PCDToCustomer);
        fvDispatchBilling.ChangeMode(FormViewMode.ReadOnly);
        GetPCDDetail(Convert.ToInt32(Session["JobId"]));
    }

    private void UpdateDispatchCourier(bool PCDToCustomer)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);

        bool bDispatchStatus = false; // If all fields filled up then status completed otherwise pending
        int TypeOfDelivery = 2; // Courier Delivery

        string strCourierName = "", strCourierDocketNo = "", strCourierReceivedBy = "";
        string PODCopyPath = "";
        string strUploadPath = ""; //hdnUploadPath.Value; //File Upload Path

        DateTime dtDispatchDate, dtPCDDeliveryDate;
        dtDispatchDate = DateTime.MinValue;
        dtPCDDeliveryDate = DateTime.MinValue;

        Panel pnlEditDispatchBillingCour = (Panel)fvDispatchBilling.FindControl("pnlEditDispatchBillingCour");
        if (pnlEditDispatchBillingCour != null && pnlEditDispatchBillingCour.Visible == true) // PCA To Billing 
        {

            strCourierName = ((TextBox)fvDispatchBilling.FindControl("txtEditBillingCourierName")).Text.Trim();
            strCourierDocketNo = ((TextBox)fvDispatchBilling.FindControl("txtEditBillingDocketNo")).Text.Trim();
            strCourierReceivedBy = ((TextBox)fvDispatchBilling.FindControl("txtEditBillingReceivedPersonName")).Text.Trim();

            TextBox txtEditBillingDispatchDate = (TextBox)fvDispatchBilling.FindControl("txtEditBillingDispatchDate");
            TextBox txtEditBillingPCDRecvdDate = (TextBox)fvDispatchBilling.FindControl("txtEditBillingPCDRecvdDate");
            FileUpload fuEditPODDispatchBillingCour = (FileUpload)fvDispatchBilling.FindControl("fuEditPODDispatchBillingCour");
            HiddenField hdnEditPODDispatchBillingCour = (HiddenField)fvDispatchBilling.FindControl("hdnEditPODDispatchBillingCour");

            if (txtEditBillingDispatchDate.Text.Trim() != "")
                dtDispatchDate = Commonfunctions.CDateTime(txtEditBillingDispatchDate.Text.Trim());

            if (txtEditBillingPCDRecvdDate.Text.Trim() != "")
                dtPCDDeliveryDate = Commonfunctions.CDateTime(txtEditBillingPCDRecvdDate.Text.Trim());

            if (fuEditPODDispatchBillingCour.FileName.Trim() != "")
            {
                PODCopyPath = UploadPCDDocument(strUploadPath, fuEditPODDispatchBillingCour);
            }
            else
                PODCopyPath = hdnEditPODDispatchBillingCour.Value;

            // Check Courier Dispatch Status

            if (strCourierName != "" && strCourierDocketNo != "" && strCourierReceivedBy != "" && txtEditBillingDispatchDate.Text.Trim() != "" && PODCopyPath != "")
            {
                bDispatchStatus = true;
            }
        }

        int result = DBOperations.AddPCDToDispatch(JobId, strCourierName, strCourierDocketNo, strCourierReceivedBy, TypeOfDelivery,
                 dtPCDDeliveryDate, dtDispatchDate, PCDToCustomer, PODCopyPath, bDispatchStatus, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Dispatch Detail Updated Successfully!";
            lblError.CssClass = "success";
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
    }


    protected void btnCancelDispatchBilling_Click(object sender, EventArgs e)
    {
        fvDispatchBilling.ChangeMode(FormViewMode.ReadOnly);
        if (Session["JobId"] != null)
        {
            GetPCDDetail(Convert.ToInt32(Session["JobId"]));
        }
    }

    private string UploadPCDDocument(string FilePath, FileUpload fuDocument)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "PCA_" + Session["JobId"].ToString() + "\\";

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

    #endregion

    protected bool DecideHereImg(string Str)
    {
        if (Str == "0" || Str == "")
            return false;
        else
            return true;
    }

    protected void gvSellingDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "detentioncopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "varaicopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emptycontcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
            //abc.Text = DocPath+"@@@"+ commandArgs[0].ToString()+"@@@"+ e.CommandName.ToLower();
        }
        else if (e.CommandName.ToLower().Trim() == "tollcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "othercopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emailapprovalcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "contractcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }

    }

    private void DownloadMultipleDocument(string DocumentPath, string Documentname)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (Documentname == "detentioncopy")
        {
            DocumentPath = "TransportDetentionDoc\\" + DocumentPath;
        }
        else if (Documentname == "varaicopy")
        {
            DocumentPath = "TransportVaraiDoc\\" + DocumentPath;
        }
        else if (Documentname == "emptycontcopy")
        {
            DocumentPath = "TransportEmptyContDoc\\" + DocumentPath;
        }
        else if (Documentname == "tollcopy")
        {
            DocumentPath = "TransportTollDoc\\" + DocumentPath;
        }
        else if (Documentname == "othercopy")
        {
            DocumentPath = "TransportOtherDoc\\" + DocumentPath;
        }
        else if (Documentname == "emailapprovalcopy")
        {
            DocumentPath = "EmailApprovalUpload\\" + DocumentPath;
        }
        else if (Documentname == "contractcopy")
        {
            DocumentPath = "TransportContractCopyUpload\\" + DocumentPath;

        }

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath(DocumentPath);
            ServerPath = ServerPath.Replace("Transport\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }

    }

    #region truck request changes
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
            //  UpdBtn.Visible = true;                                 //changes for JobDetails Fill job Details transport/ truck Request
        }
        else
        {
            loadedDocuments.Visible = false;
            lblEmpty_Letter.Visible = false;
            //   UpdBtn.Visible = false;
        }
    }

    protected void ddlExportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Mode = 0;
        if (hdnMode.Value != "" && hdnMode.Value != "0")
        {
            Mode = Convert.ToInt32(hdnMode.Value);
        }
    }


    protected void gvTruckRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "packingdocs")
        {
            if (e.CommandArgument.ToString() != "")
            {
                int TransRequestId = Convert.ToInt32(e.CommandArgument.ToString());
                if (TransRequestId > 0)
                {
                    string FilePath = "";
                    String ServerPath = FileServer.GetFileServerDir();
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectoryByName("TransportFiles");
                        DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransRequestId);
                        if (dsGetDoc != null)
                        {
                            for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                            {
                                if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                                {
                                    if (ServerPath == "")
                                    {
                                        FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                                    }
                                    else
                                    {
                                        FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                                    }
                                    zip.AddFile(FilePath, "TransportFiles");
                                }
                            }

                            Response.Clear();
                            Response.BufferOutput = false;
                            string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }
                    }
                }
            }
        }
        else if (e.CommandName.ToLower().Trim() == "select")
        {
            if (e.CommandArgument.ToString() != "")
            {
                string RefNo = e.CommandArgument.ToString();
                lblTransRefNo.Text = RefNo;
                if (Convert.ToInt32(Session["TRId"].ToString()) > 0)
                {
                    DataView dsGetJobDetail = DBOperations.GetTransportDetailById(Convert.ToInt32(Session["TRId"]));
                    if (dsGetJobDetail != null && dsGetJobDetail.Count > 0)                           //added pickpincode and droppincode          
                    {
                        txtpickPincode.Text = dsGetJobDetail[0]["PickupPincode"].ToString();
                        txtdropPincode.Text = dsGetJobDetail[0]["DropPincode"].ToString();
                    }
                    if (dsGetJobDetail != null)
                    {
                        ddDeliveryType.Visible = true;
                        ddlExportType.Visible = false;
                        if (dsGetJobDetail.Table.Rows[0]["DeliveryType"] != DBNull.Value)
                        {
                            ddDeliveryType.SelectedValue = dsGetJobDetail.Table.Rows[0]["DeliveryType"].ToString();
                            ddDeliveryType_SelectedIndexChanged(null, EventArgs.Empty);
                        }
                        lblType_Title.Text = "Delivery Type";

                        string VehiclePlaced = dsGetJobDetail.Table.Rows[0]["IsVehiclePlaced"].ToString();
                        string DeliverStatus = dsGetJobDetail.Table.Rows[0]["DeliveryStatus"].ToString();
                        if (DeliverStatus != "1")
                        {
                            if (VehiclePlaced != "1")
                            {

                                dvtruckDetail.Visible = true;
                                tblTruckRequest.Visible = true;
                                txtDestination.Text = dsGetJobDetail.Table.Rows[0]["Destination"].ToString();
                                txtDimension.Text = dsGetJobDetail.Table.Rows[0]["Dimension"].ToString();
                                lblJobNumber.Text = dsGetJobDetail.Table.Rows[0]["JobRefNo"].ToString();
                                txtVehiclePlaceDate.Text = dsGetJobDetail.Table.Rows[0]["VehiclePlaceRequireDate"].ToString();
                                txtRemark1.Text = dsGetJobDetail.Table.Rows[0]["Remark"].ToString();
                                txtpickState.Text = dsGetJobDetail.Table.Rows[0]["PickupState"].ToString();                    //Add new details  stete city pincode for gt job Detail
                                txtpickCity.Text = dsGetJobDetail.Table.Rows[0]["PickupCity"].ToString();
                                txtdropState.Text = dsGetJobDetail.Table.Rows[0]["DropState"].ToString();
                                txtdropCity.Text = dsGetJobDetail.Table.Rows[0]["DropCity"].ToString();
                            }
                            else
                            {

                                dvtruckDetail.Visible = false;
                            }
                        }
                        else
                        {
                            lblTruckerr.Text = "Vehicle already delivered, so not allow to modify the truck request";
                            lblTruckerr.CssClass = "errorMsg";
                        }
                    }
                }

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            btnSaveDocument_Click(FileUpload1, EventArgs.Empty);
        }

        DateTime dtVehiclePlaceRequireDate = DateTime.MinValue;
        int JobType = 0, TotalContainers = 0, VehicleRequired = 1, Mode = 0, DeliveryType = 0, ExportType = 0;
        int PickupPincode = 0, DropPincode = 0;
        string PickupState = "", PickupCity = "", DropState = "", DropCity = "", EmptyLetter = "", EmptyDocPath = "", FileName = "";

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

        if (Session["TRId"].ToString() != "" && Session["TRId"].ToString() != "0")
        {
            //if (hdnJobType.Value != "0" && hdnJobType.Value != "")
            //{
            //    JobType = Convert.ToInt32(hdnJobType.Value);
            //}


            if (txtVehiclePlaceDate.Text.Trim() != "")
                dtVehiclePlaceRequireDate = Commonfunctions.CDateTime(txtVehiclePlaceDate.Text.Trim());

            PickupPincode = Convert.ToInt32(txtpickPincode.Text);               //Changes for update operation
            DropPincode = Convert.ToInt32(txtdropPincode.Text);
            PickupState = txtpickState.Text;
            PickupCity = txtpickCity.Text;
            DropState = txtdropState.Text;
            DropCity = txtdropCity.Text;
            EmptyLetter = loadedDocuments.FileName;

            if (ddDeliveryType.SelectedValue == "1")
            {
                EmptyDocPath = Regex.Replace(lblJobNumber.Text, "[^a-zA-Z0-9]", "");
                EmptyDocPath = EmptyDocPath + "\\";
                if (loadedDocuments != null && loadedDocuments.HasFile)
                    FileName = UploadEmptyDocuments(EmptyDocPath);
            }

            //int result = DBOperations.UpdTransportRequestById(Convert.ToInt32(Session["TRId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
            //    txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, LoggedInUser.glUserId);

            int result = DBOperations.UpdateTransRequest(Convert.ToInt32(Session["TRId"].ToString()), txtDestination.Text.Trim(), txtRemark1.Text.Trim(), DeliveryType,
                txtDimension.Text.Trim(), dtVehiclePlaceRequireDate, FileName, EmptyDocPath, 118, PickupPincode, PickupState, PickupCity, DropPincode, DropState, DropCity, EmptyLetter, LoggedInUser.glUserId);


            if (result == 0)
            {
                tblTruckRequest.Visible = false;
                lblError.Text = "Truck detail Added successfully!";
                lblError.CssClass = "success";
                gvTruckRequest.DataBind();
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
                            int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["JobId"].ToString()), DocPath, LoggedInUser.glUserId);
                        }
                    }
                }
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
            DownloadPackingList(FilePath);
        }
    }

    protected void BindGrid()
    {
        //if (ViewState["PackingList"].ToString() != "")
        //{
        DataTable dtPackingList = (DataTable)ViewState["PackingList"];
        rptDocument.DataSource = dtPackingList;
        rptDocument.DataBind();
        //}
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (FileUpload1 != null && FileUpload1.HasFile)
            fileName = UploadFiles(FileUpload1);
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

            dtAnnexure.Rows.Add(PkId, fileName, FileUpload1.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["PackingList"] = dtAnnexure;
            BindGrid();
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
                        int result_Doc = DBOperations.AddPackingListDocs(Convert.ToInt32(Session["JobId"].ToString()), DocPath, LoggedInUser.glUserId);
                    }
                }
            }

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

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("\\UploadFiles\\Transport\\" + FilePath);
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

    protected void DownloadPackingList(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("\\UploadFiles\\Transport\\" + DocumentPath);
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
    #endregion

    protected void txtpickPincode_TextChanged(object sender, EventArgs e)   //Get State and city by using Pincode API 
    {
        string pincode = txtpickPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnPincodeId.Value = "0";
            txtpickState.Text = "Invalid Pincode";
            txtpickCity.Text = "Invalid Pincode";
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
                            txtpickState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtpickCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnPincodeId.Value = pincode;
                        }
                        else
                        {
                            txtpickState.Text = "State Not Found";
                            txtpickCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtpickState.Text = "State Not Found";
                        txtpickCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtpickState.Text = "Error";
                    txtpickCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtpickState.Text = "Error";
            txtpickCity.Text = "Error";
        }
    }

    protected void txtdropPincode_TextChanged(object sender, EventArgs e)    // Get State and City By using Pincode API
    {
        string pincode = txtdropPincode.Text.Trim();

        if (string.IsNullOrEmpty(pincode))
        {
            hdnpinid.Value = "0";
            txtdropState.Text = "Invalid Pincode";
            txtdropCity.Text = "Invalid Pincode";
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

                            txtdropState.Text = record["statename"]?.ToString() ?? "State Not Found";
                            txtdropCity.Text = record["district"]?.ToString() ?? "City Not Found";
                            hdnpinid.Value = pincode;
                        }
                        else
                        {
                            txtdropState.Text = "State Not Found";
                            txtdropCity.Text = "City Not Found";
                        }
                    }
                    else
                    {
                        txtdropState.Text = "State Not Found";
                        txtdropCity.Text = "City Not Found";
                    }
                }
                else
                {
                    txtdropState.Text = "Error";
                    txtdropCity.Text = "Error";
                }
            }
        }
        catch (HttpRequestException httpEx)
        {

            System.Diagnostics.Debug.WriteLine($"HTTP Request Error: {httpEx.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
        catch (Exception ex)
        {

            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            txtdropState.Text = "Error";
            txtdropCity.Text = "Error";
        }
    }
    #region Empty letter 
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
    #endregion

    #region Delivery Update
    private void JobDetailMS(int JobId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        string strPreAlertId = "0"; bool bTransportationByBabaji = true;
        int BranchId = 0;

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Job Detail
        DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);

        // Move customer delivery to warehouse
        btnMoveToWarehouse.Visible = false;
        ddWarehouse.Visible = false;
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            BranchId = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["BabajiBranchId"].ToString());
            int BoeType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["BOEType"].ToString());
            int JobType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["JobTypeId"].ToString());
            if ((BoeType == (int)EnumBOEType.Home))
            {
                if (JobType != 13) // DPD
                {
                    DBOperations.FillWarehouse(ddWarehouse, (Int16)EnumWarehouseType.General, BranchId);
                    ddWarehouse.Visible = true;
                    btnMoveToWarehouse.Visible = true;
                }
            }
            else if (BoeType == (int)EnumBOEType.Inbond)
            {
                DBOperations.FillWarehouse(ddWarehouse, (Int16)EnumWarehouseType.Bonded, BranchId);
                ddWarehouse.Visible = true;
                btnMoveToWarehouse.Visible = true;
            }
        }
    }
    protected void btnMoveToWarehouse_Click(object sender, EventArgs e)
    {
        if (Session["TrJobId"] != null)
        {
            int WarehouseId = 0;
            if (ddWarehouse != null && ddWarehouse.SelectedValue != "0")
            {
                WarehouseId = Convert.ToInt32(ddWarehouse.SelectedValue);
            }

            int result = DBOperations.TR_MoveDeliveryToWarehouse(Convert.ToInt32(Session["TrJobId"]), WarehouseId);
            if (result == 0)
            {
                lblError.Text = "Successfully moved customer delivery to warehouse!";
                lblError.CssClass = "success";
                GridViewDelivery.DataBind();
                GridViewWarehouse.DataBind();
            }
            else if (result == -1)
            {
                lblError.Text = "Error while updating warehouse delivery! Please try again later.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == -2)
            {
                lblError.Text = "Cannot update delivery as Customer delivery does not exists OR Job is consolidated with other job!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Delivery not completed yet!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void GridViewWarehouse_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        int vehicleId = 0;
        string vehiclename = "";
        if (e.CommandName.ToLower() == "update")
        {
            int NoOfPackages = 0;

            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strlId = GridViewWarehouse.DataKeys[gvrow.RowIndex].Value.ToString();
            DataSourceWarehouse.UpdateParameters["lId"].DefaultValue = strlId;

            // Update Delivered Package Except Loaded Shipment
            TextBox txtPackages = (TextBox)gvrow.FindControl("txtPackages");
            if (txtPackages.Text.Trim() != "")
            {
                NoOfPackages = Convert.ToInt32(txtPackages.Text.Trim());
                DataSourceWarehouse.UpdateParameters["NoOfPackages"].DefaultValue = NoOfPackages.ToString();
            }

            TextBox txtVehicleNo = (TextBox)gvrow.FindControl("txtVehicleNo");
            string strVehicleNo = txtVehicleNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["VehicleNo"].DefaultValue = strVehicleNo;


            DropDownList ddVehicleType = (DropDownList)gvrow.FindControl("ddVehicleType");


            if (ddVehicleType.SelectedValue != "0")
            {
                vehicleId = Convert.ToInt32(ddVehicleType.SelectedValue);
                vehiclename = ddVehicleType.SelectedItem.Text;
                DataSourceWarehouse.UpdateParameters["VehicleType"].DefaultValue = Convert.ToString(vehicleId);
            }
            else
            {
                DataSourceWarehouse.UpdateParameters["VehicleType"].DefaultValue = "-Select-";
                //lblMessage.Text = "Please Select Customs Group";
                //lblMessage.CssClass = "errorMsg";
                return;
            }

            TextBox txtVehicleRcvdDate = (TextBox)gvrow.FindControl("txtVehicleRcvdDate");
            string strVehicleRcvdDate = txtVehicleRcvdDate.Text.Trim();
            if (strVehicleRcvdDate != "")
            {
                strVehicleRcvdDate = Commonfunctions.CDateTime(strVehicleRcvdDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["VehicleRcvdDate"].DefaultValue = strVehicleRcvdDate;
            }

            TextBox txtTransporter = (TextBox)gvrow.FindControl("txtTransporter");
            string Transporter = txtTransporter.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["TransporterName"].DefaultValue = Transporter;

            TextBox txtLRNo = (TextBox)gvrow.FindControl("txtLRNo");
            string LRNo = txtLRNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["LRNo"].DefaultValue = LRNo;

            TextBox txtLRDate = (TextBox)gvrow.FindControl("txtLRDate");
            string strLRDate = txtLRDate.Text.Trim();
            if (strLRDate != "")
            {
                strLRDate = Commonfunctions.CDateTime(strLRDate.Trim()).ToShortDateString();
                DataSourceDelivery.UpdateParameters["LRDate"].DefaultValue = strLRDate;
            }

            TextBox txtDeliveryPoint = (TextBox)gvrow.FindControl("txtDeliveryPoint");
            string strDeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DeliveryPoint"].DefaultValue = strDeliveryPoint;

            TextBox txtDispatchDate = (TextBox)gvrow.FindControl("txtDispatchDate");
            string strDispatchDate = txtDispatchDate.Text.Trim();
            if (strDispatchDate != "")
            {
                strDispatchDate = Commonfunctions.CDateTime(strDispatchDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["DispatchDate"].DefaultValue = strDispatchDate;
            }

            TextBox txtRoadPermitNo = (TextBox)gvrow.FindControl("txtRoadPermitNo");
            string RoadPermitNo = txtRoadPermitNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["RoadPermitNo"].DefaultValue = RoadPermitNo;

            TextBox txtRoadPermitDate = (TextBox)gvrow.FindControl("txtRoadPermitDate");
            string strRoadPermitDate = txtRoadPermitDate.Text.Trim();
            if (strRoadPermitDate != "")
            {
                strRoadPermitDate = Commonfunctions.CDateTime(strRoadPermitDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["RoadPermitDate"].DefaultValue = strRoadPermitDate;
            }

            string PODPath = "";
            FileUpload fuattach = (FileUpload)gvrow.FindControl("fuPODAttchment");
            HiddenField hdnPODPath = (HiddenField)gvrow.FindControl("hdnPODPath");

            string FileName = fuattach.FileName;
            FileName = FileServer.ValidateFileName(FileName);

            if (fuattach.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = "";// hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "PODFiles\\";

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
                    System.IO.Directory.CreateDirectory(ServerFilePath);
                }

                if (fuattach.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuattach.SaveAs(ServerFilePath + FileName);
                }

                PODPath = FilePath + FileName;
                DataSourceWarehouse.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }
            else if (hdnPODPath.Value != "")
            {
                PODPath = hdnPODPath.Value;
                DataSourceWarehouse.UpdateParameters["PODAttachment"].DefaultValue = PODPath;
            }


            //---------------

            string BCCPath_Warehouse = "";
            FileUpload fuattach_Warehouse = (FileUpload)gvrow.FindControl("fuBCCAttchment_Warehouse");
            HiddenField hdnBccPath_Warehouse = (HiddenField)gvrow.FindControl("hdnBCCPath_Warehouse");

            string FileName1 = fuattach_Warehouse.FileName;
            FileName = FileServer.ValidateFileName(FileName1);

            if (fuattach_Warehouse.HasFile)
            {
                int JobId = Convert.ToInt32(Session["JobId"]);

                string FilePath = "";// hdnUploadPath.Value;
                if (FilePath == "")
                    FilePath = "BCCFiles\\";

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
                    System.IO.Directory.CreateDirectory(ServerFilePath);
                }

                if (fuattach_Warehouse.FileName != string.Empty)
                {
                    if (System.IO.File.Exists(ServerFilePath + FileName))
                    {
                        string ext = Path.GetExtension(FileName);
                        FileName = Path.GetFileNameWithoutExtension(FileName);

                        string FileId = RandomString(5);

                        FileName += "_" + FileId + ext;
                    }

                    fuattach_Warehouse.SaveAs(ServerFilePath + FileName);
                }

                BCCPath_Warehouse = FilePath + FileName;
                DataSourceWarehouse.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath_Warehouse;
            }
            else if (hdnBccPath_Warehouse.Value != "")
            {
                BCCPath_Warehouse = hdnBccPath_Warehouse.Value;
                DataSourceWarehouse.UpdateParameters["BabajiChallanCopyFile"].DefaultValue = BCCPath_Warehouse;
            }



            TextBox txtNFormNo = (TextBox)gvrow.FindControl("txtNFormNo");
            string NFormNo = txtNFormNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["NFormNo"].DefaultValue = NFormNo;

            TextBox txtNFormDate = (TextBox)gvrow.FindControl("txtNFormDate");
            string strNFormDate = txtNFormDate.Text.Trim();
            if (strNFormDate != "")
            {
                strNFormDate = Commonfunctions.CDateTime(txtNFormDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["NFormDate"].DefaultValue = strNFormDate;
            }

            TextBox txtNClosingDate = (TextBox)gvrow.FindControl("txtNClosingDate");
            string strNClosingDate = txtNClosingDate.Text.Trim();
            if (strNClosingDate != "")
            {
                strNClosingDate = Commonfunctions.CDateTime(txtNClosingDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["NClosingDate"].DefaultValue = strNClosingDate;
            }

            TextBox txtSFormNo = (TextBox)gvrow.FindControl("txtSFormNo");
            string SFormNo = txtSFormNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["SFormNo"].DefaultValue = SFormNo;

            TextBox txtSFormDate = (TextBox)gvrow.FindControl("txtSFormDate");
            string strSFormDate = txtSFormDate.Text.Trim();
            if (strSFormDate != "")
            {
                strSFormDate = Commonfunctions.CDateTime(txtSFormDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["SFormDate"].DefaultValue = strSFormDate;
            }

            TextBox txtSClosingDate = (TextBox)gvrow.FindControl("txtSClosingDate");
            string strSClosingDate = txtSClosingDate.Text.Trim();
            if (strSClosingDate != "")
            {
                strSClosingDate = Commonfunctions.CDateTime(txtSClosingDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["SClosingDate"].DefaultValue = strSClosingDate;
            }

            TextBox txtOctroiAmount = (TextBox)gvrow.FindControl("txtOctroiAmount");
            string strOctroiAmount = txtOctroiAmount.Text.Trim();
            DataSourceWarehouse.UpdateParameters["OctroiAmount"].DefaultValue = strOctroiAmount;

            TextBox txtOctroiReceiptNo = (TextBox)gvrow.FindControl("txtOctroiReceiptNo");
            string strOctroiReceiptNo = txtOctroiReceiptNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["OctroiReceiptNo"].DefaultValue = strOctroiReceiptNo;

            TextBox txtOctroiPaidDate = (TextBox)gvrow.FindControl("txtOctroiPaidDate");
            string strOctroiPaidDate = txtOctroiPaidDate.Text.Trim();
            if (strOctroiPaidDate != "")
            {
                strOctroiPaidDate = Commonfunctions.CDateTime(txtOctroiPaidDate.Text.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["OctroiPaidDate"].DefaultValue = strOctroiPaidDate;
            }

            TextBox txtChallanNo = (TextBox)gvrow.FindControl("txtChallanNo");
            string ChallanNo = txtChallanNo.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["BabajiChallanNo"].DefaultValue = ChallanNo;

            TextBox txtChalanDate = (TextBox)gvrow.FindControl("txtChalanDate");
            string strBabajiChallanDate = txtChalanDate.Text.Trim();
            if (strBabajiChallanDate != "")
            {
                strBabajiChallanDate = Commonfunctions.CDateTime(strBabajiChallanDate.Trim()).ToShortDateString();
                DataSourceWarehouse.UpdateParameters["BabajiChallanDate"].DefaultValue = strBabajiChallanDate;
            }

            TextBox txtdrivername = (TextBox)gvrow.FindControl("txtdrivername");
            string strdrivername = txtdrivername.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DriverName"].DefaultValue = strdrivername;

            TextBox txtdriverphoneno = (TextBox)gvrow.FindControl("txtdriverphoneno");
            string strdriverphoneno = txtdriverphoneno.Text.ToUpper().Trim();
            DataSourceWarehouse.UpdateParameters["DriverPhoneno"].DefaultValue = strdriverphoneno;


            //TextBox txtDeliveryDate = (TextBox)gvrow.FindControl("txtDeliveryDate");
            //TextBox txtEmptyContainerDate = (TextBox)gvrow.FindControl("txtEmptyContainerDate");
            //string strDeliveryDate = txtDeliveryDate.Text.Trim();
            //string strEmptyContainerDate = txtEmptyContainerDate.Text.Trim();
            //if (strDeliveryDate != "")
            //{
            //    strDeliveryDate = Commonfunctions.CDateTime(strDeliveryDate).ToShortDateString();
            //    DataSourceWarehouse.UpdateParameters["DeliveryDate"].DefaultValue = strDeliveryDate;
            //}

            //if (strEmptyContainerDate != "")
            //{
            //    strEmptyContainerDate = Commonfunctions.CDateTime(strEmptyContainerDate.Trim()).ToShortDateString();
            //    DataSourceWarehouse.UpdateParameters["EmptyContRetrunDate"].DefaultValue = strEmptyContainerDate;
            //}


            //TextBox txtDeliveryPoint = (TextBox)gvrow.FindControl("txtDeliveryPoint");
            //string DeliveryPoint = txtDeliveryPoint.Text.ToUpper().Trim();
            //DataSourceWarehouse.UpdateParameters["DeliveryPoint"].DefaultValue = DeliveryPoint;
            //TextBox txtRemark = (TextBox)gvrow.FindControl("txtRemark");
            //string strRemark = txtRemark.Text.Trim();
            //DataSourceDelivery.UpdateParameters["Remark"].DefaultValue = strRemark;

            //

            DataSourceWarehouse.Update();


        }

        if (e.CommandName.ToLower() == "cancel")
        {
            lblError.Visible = false;
            lblError.Text = "";
            GridViewWarehouse.EditIndex = -1;
        }

        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadPODDocument(DocPath);
        }

        if (e.CommandName.ToLower() == "delete1")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int lid = Convert.ToInt32(GridViewWarehouse.DataKeys[gvrow.RowIndex].Value.ToString());
            //int lid = Convert.ToInt32(e.CommandArgument.ToString());
            int Result = -123;

            Result = DBOperations.DeleteDeliveryWarehouseDetail(lid, LoggedInUser.glUserId);

            if (Result == 0)
            {
                lblError.Text = "Delivery to Warehouse Deleted Successfully!";
                lblError.CssClass = "success";
                GridViewWarehouse.DataBind();
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.CssClass = "errorMsg";
                lblError.Text = "Job Details Not Found!";
            }
        }

    }

    protected void GridViewWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int DeliveryTypeId = (Int32)DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId");
            bool IsNForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsNForm");
            bool IsSForm = (bool)DataBinder.Eval(e.Row.DataItem, "IsSForm");
            bool IsOctroi = (bool)DataBinder.Eval(e.Row.DataItem, "IsOctroi");
            bool IsRoadPermit = (bool)DataBinder.Eval(e.Row.DataItem, "IsRoadPermit");

            if (DeliveryTypeId == (Int32)DeliveryType.Loaded) // Delivery Type
            {
                GridViewWarehouse.Columns[2].Visible = true; // Container NO
                GridViewWarehouse.Columns[3].Visible = false; // No Of Packages
            }
            else
            {
                GridViewWarehouse.Columns[2].Visible = false; // Container NO
                GridViewWarehouse.Columns[3].Visible = true; // No Of Packages
            }

            if (IsNForm == true) // NForm Applicable
            {
                GridViewWarehouse.Columns[15].Visible = true; // N Form No
                GridViewWarehouse.Columns[16].Visible = true; // N Form Date
                GridViewWarehouse.Columns[17].Visible = true; // N Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[15].Visible = false; // N Form No
                GridViewWarehouse.Columns[16].Visible = false; // N Form Date
                GridViewWarehouse.Columns[17].Visible = false; // N Closing Date
            }

            if (IsSForm == true) // SForm Applicable
            {
                GridViewWarehouse.Columns[18].Visible = true; // S Form No
                GridViewWarehouse.Columns[19].Visible = true; // S Form Date
                GridViewWarehouse.Columns[20].Visible = true; // S Closing Date
            }
            else
            {
                GridViewWarehouse.Columns[18].Visible = false; // S Form No
                GridViewWarehouse.Columns[19].Visible = false; // S Form Date
                GridViewWarehouse.Columns[20].Visible = false; // S Closing Date
            }
            if (IsOctroi == true) // Octroi Applicable
            {
                GridViewWarehouse.Columns[21].Visible = true; // Octroi Amount
                GridViewWarehouse.Columns[22].Visible = true; // Octroi Receipt No	
                GridViewWarehouse.Columns[23].Visible = true; // Octroi Paid Date
            }
            else
            {
                GridViewWarehouse.Columns[21].Visible = false; // Octroi Amount
                GridViewWarehouse.Columns[22].Visible = false; // Octroi Receipt No	
                GridViewWarehouse.Columns[23].Visible = false; // Octroi Paid Date
            }
            if (IsRoadPermit == true) // Road Permit Applicable
            {
                GridViewWarehouse.Columns[24].Visible = true; // Road Permit No
                GridViewWarehouse.Columns[25].Visible = true; // Road Permit Date
            }
            else
            {
                GridViewWarehouse.Columns[24].Visible = false; // Road Permit No
                GridViewWarehouse.Columns[25].Visible = false; // Road Permit Date
            }
        }
    }
    protected void DataSourceWarehouse_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lblError.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

        if (Result == 0)
        {
            lblError.Text = "Delivery Detail Updated Successfully";
            lblError.CssClass = "success";
            GridViewWarehouse.DataBind();
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "Please Check No Of Delivered Packages. Supply Count Exceed Available Packages!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime!";
            lblError.CssClass = "errorMsg";
        }

    }
    #endregion

    protected void GridViewVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["TRLId"] = commandArgs[0].ToString();
            string Memo = commandArgs[1].ToString();
            lblJobRefNo.Text = commandArgs[2].ToString();
            mpeMemoUpload.Show();
        }
        else if (e.CommandName.ToLower() == "download")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DownloadDoc(e.CommandArgument.ToString());
            }
        }
    }

    protected void btnMemoSubmit_Click(object sender, EventArgs e)
    {
        string MemoFilePath = "";

        if (fuMemo.HasFile)
            MemoFilePath = MemoUploadFiles(fuMemo);
        int result = DBOperations.UpdateMemoDetail(Convert.ToInt32(Session["TRLId"].ToString()), MemoFilePath, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblResult.Text = "Memo Uploaded successfully";
            lblResult.CssClass = "success";
        }
        GridViewVehicle.DataBind();
    }

    private string MemoUploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string strDirPath = lblJobRefNo.Text.Replace("/", "");
        strDirPath = strDirPath.Replace("-", "");
        string strInvoiceFilePath = "TransMemo\\" + strDirPath + "\\";

        //if (FilePath == "")
        //    FilePath = "Expense_" + hdnNewPaymentLid.Value + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == null)
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strInvoiceFilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + strInvoiceFilePath;
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

                FileName += "_" +  ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return strInvoiceFilePath + FileName;
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
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\TransMemo\\" + DocumentPath);
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
}

