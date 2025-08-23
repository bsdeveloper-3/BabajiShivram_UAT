using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit;
using ClosedXML.Excel;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Data.Common;
using System.Drawing;
using System.Text;

public partial class BillingTransport_TripDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvVendorJobDetail_New);
        if (Convert.ToString(Session["UserId"]) != null)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Trip Details";
        }

        DataFilter1.DataSource = DataSourceVendorJobs2;
        DataFilter1.DataColumns = gvVendorJobDetail_New.Columns;
        DataFilter1.FilterSessionID = "TripDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GRID VIEW EVENTS

    protected void gvVendorJobDetail_New_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FreightCharges")) != 0)
            {
                // Change row color based on job priority

                e.Row.Cells[11].Text = "";
            }
        }
    }

    protected void txtDetentionCharges_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        int index = row.RowIndex;
        TextBox txtDetentionCharges = (TextBox)gvVendorJobDetail_New.Rows[index].FindControl("txtDetentionCharges");
        RequiredFieldValidator rfvDetentionDay = (RequiredFieldValidator)gvVendorJobDetail_New.Rows[index].FindControl("rfvDetentionDays");
        if (txtDetentionCharges.Text.Trim() != "")
        {
            rfvDetentionDay.Visible = true;
        }
        else
        {
            rfvDetentionDay.Visible = false;
        }
    }

    protected void gvVendorJobDetail_New_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim().ToString() == "movetrip")
        {
            int TripId = 0;
            DateTime DispatchDate = DateTime.MinValue;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string VehicleNo = commandArgs[0].ToString();
            string DeliveryPoint = commandArgs[1].ToString();
            if (Convert.ToDateTime(commandArgs[2]).ToString() != "")
                DispatchDate = Convert.ToDateTime(commandArgs[2].ToString());

            if (VehicleNo != "" && DeliveryPoint != "")
            {
                // get trip detail primary key
                //DataSet dsGetDeliveryDetails = DBOperations.GetTripDetails(VehicleNo, DispatchDate, DeliveryPoint);
                //if (dsGetDeliveryDetails != null && dsGetDeliveryDetails.Tables[0].Rows.Count > 0)
                //{
                //    TripId = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["TripId"].ToString());
                //    int result = DBOperations.UpdateJobBillStatus(TripId, Convert.ToInt32(0), Convert.ToInt32(Session["VendorId"]),
                //         Convert.ToInt32(0), Convert.ToDateTime(DateTime.MinValue), Convert.ToString(""), Convert.ToString(""));
                //    if (result == 0)
                //    {
                //        gvVendorJobDetail_New.DataBind();
                //        lberror.Text = "";
                //    }
                //}
            }
        }
    }

    protected void gvVendorJobDetail_New_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvVendorJobDetail_New_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int DetentionDays = 0, count = 0, JobId = 0, DeliveryId = 0, Packages = 0, VehicleType = 0, TransporterVehicleId = 0;
        string DocPath = "", DeliveryPoint = "", DeliveryFrom = "", VehicleNo = "", TransporterName = "";
        DateTime DispatchDate = DateTime.MinValue;

        VehicleNo = gvVendorJobDetail_New.Rows[e.RowIndex].Cells[1].Text;
        DeliveryPoint = gvVendorJobDetail_New.Rows[e.RowIndex].Cells[2].Text;
        if (gvVendorJobDetail_New.Rows[e.RowIndex].Cells[3].Text != "")
            DispatchDate = Convert.ToDateTime(gvVendorJobDetail_New.Rows[e.RowIndex].Cells[3].Text);
        TextBox txtDetentionCharges = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtDetentionCharges");
        TextBox txtDetentionDays = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtDetentionDays");
        TextBox txtWaraiCharges = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtWaraiCharges");
        TextBox txtEmptyOffLoadingCharges = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtEmptyOffLoadingCharges");
        TextBox txtTempoUnionCharges = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtTempoUnionCharges");
        TextBox txtFreightRate = (TextBox)gvVendorJobDetail_New.Rows[e.RowIndex].FindControl("txtFreightRate");

        if (txtDetentionCharges.Text.Trim() != "")
        {
            if (txtDetentionDays.Text.Trim() != "")
                DetentionDays = Convert.ToInt32(txtDetentionDays.Text.Trim());

            if (DetentionDays == 0)
            {
                count = 1;
            }
        }

        if (count == 0)
        {
            #region CALCULATE TOTAL

            double DetentionCharges = 0.0, OtherCharges = 0.0, Total = 0.0, FrightRate = 0.0, VaraiCharges = 0.0, EmptyOffLoadingCharges = 0.0,
                   TempoUnionCharges = 0.0;

            if (txtFreightRate.Text != "")                                                      // TP Fright Rate
                FrightRate = Convert.ToDouble(txtFreightRate.Text);
            if (txtDetentionCharges.Text != "")                                                 // Detention Charges
                DetentionCharges = Convert.ToDouble(txtDetentionCharges.Text);
            if (txtWaraiCharges.Text != "")                                                     // Warai Charges
                VaraiCharges = Convert.ToDouble(txtWaraiCharges.Text);
            if (txtEmptyOffLoadingCharges.Text != "")                                           // Empty Of Loading Charges
                EmptyOffLoadingCharges = Convert.ToDouble(txtEmptyOffLoadingCharges.Text);
            if (txtTempoUnionCharges.Text != "")                                                // Tempo Union charges
                TempoUnionCharges = Convert.ToDouble(txtTempoUnionCharges.Text);
            OtherCharges = VaraiCharges + EmptyOffLoadingCharges + TempoUnionCharges;           // Other Charges = Varai + Empty Of Loading + Tempo Union

            Total = Convert.ToDouble((Convert.ToDouble(DetentionCharges) * Convert.ToDouble(DetentionDays)) + Convert.ToDouble(FrightRate) + Convert.ToDouble(OtherCharges));

            #endregion

            //DataSet dsGetDeliveryDetails = DBOperations.GetDeliveryDetailAsPerVehicle(VehicleNo, DispatchDate, DeliveryPoint);
            //if (dsGetDeliveryDetails != null)
            //{
            //    JobId = 0; // Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["JobId"].ToString());
            //    DeliveryId = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["DeliveryId"].ToString());
            //    Packages = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["Packages"].ToString());
            //    VehicleType = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["VehicleType"].ToString());
            //    DeliveryFrom = dsGetDeliveryDetails.Tables[0].Rows[0]["DeliveryFrom"].ToString();
            //    TransporterName = ""; // dsGetDeliveryDetails.Tables[0].Rows[0]["TransporterName"].ToString();
            //}

            if (DeliveryId != 0)
            {
                DataSourceVendorJobs2.UpdateParameters["JobId"].DefaultValue = Convert.ToString(JobId);
                DataSourceVendorJobs2.UpdateParameters["JobDeliveryId"].DefaultValue = Convert.ToString(DeliveryId);
                DataSourceVendorJobs2.UpdateParameters["TransporterName"].DefaultValue = Convert.ToString(TransporterName);
                DataSourceVendorJobs2.UpdateParameters["Packages"].DefaultValue = Convert.ToString(Packages);
                DataSourceVendorJobs2.UpdateParameters["ContainerId"].DefaultValue = Convert.ToString("0");
                DataSourceVendorJobs2.UpdateParameters["VehicleType"].DefaultValue = Convert.ToString(VehicleType);
                DataSourceVendorJobs2.UpdateParameters["DeliveryFrom"].DefaultValue = Convert.ToString(DeliveryFrom);
                DataSourceVendorJobs2.UpdateParameters["DeliveryTo"].DefaultValue = Convert.ToString(DeliveryPoint);
                DataSourceVendorJobs2.UpdateParameters["DispatchDate"].DefaultValue = Convert.ToString(DispatchDate);
                DataSourceVendorJobs2.UpdateParameters["lUser"].DefaultValue = Convert.ToString(Session["VendorId"]);
                DataSourceVendorJobs2.UpdateParameters["VehicleNo"].DefaultValue = Convert.ToString(VehicleNo);
                DataSourceVendorJobs2.UpdateParameters["TPFrightRate"].DefaultValue = Convert.ToString(txtFreightRate.Text.Trim());
                DataSourceVendorJobs2.UpdateParameters["DetentionDays"].DefaultValue = Convert.ToString(DetentionDays);
                DataSourceVendorJobs2.UpdateParameters["DetentionCharges"].DefaultValue = Convert.ToString(DetentionCharges);
                DataSourceVendorJobs2.UpdateParameters["WaraiCharges"].DefaultValue = Convert.ToString(VaraiCharges);
                DataSourceVendorJobs2.UpdateParameters["EmptyOffLoadingCharges"].DefaultValue = Convert.ToString(EmptyOffLoadingCharges);
                DataSourceVendorJobs2.UpdateParameters["TempoUnionCharges"].DefaultValue = Convert.ToString(TempoUnionCharges);
                DataSourceVendorJobs2.UpdateParameters["Total"].DefaultValue = Convert.ToString(Convert.ToDecimal(Total));
                DataSourceVendorJobs2.UpdateParameters["Remarks"].DefaultValue = "";
                DataSourceVendorJobs2.UpdateParameters["LrCopiesDocPath"].DefaultValue = "";
                DataSourceVendorJobs2.UpdateParameters["ReceiptDocPath"].DefaultValue = "";
            }
        }
        else
        {
            lberror.Visible = true;
            lberror.Text = "Enter detention days.!!";
            lberror.CssClass = "errorMsg";
            txtDetentionDays.Focus();
        }
    }

    protected void gvVendorJobDetail_New_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lberror.Visible = true;
        int lid = 0; //Convert.ToInt32(gvVendorJobDetail_New.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteTermConditionMS(lid, LoggedInUser.glUserId);
        if (result == 2)
        {
            lberror.Text = "Terms & Condition Deleted Successfully!";
            lberror.CssClass = "success";
            //FillTermsCondition();
        }
        else if (result == 1)
        {
            lberror.Text = "System Error! Please Try After Sometime.";
            lberror.CssClass = "errorMsg";
        }
        else if (result == 3)
        {
            lberror.Text = "Quotation Category Does Not Exists..!!";
            lberror.CssClass = "warning";
        }
    }

    protected void gvVendorJobDetail_New_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVendorJobDetail_New.EditIndex = e.NewEditIndex;
        lberror.Text = "";
        lberror.Visible = false;
    }

    protected void gvVendorJobDetail_New_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVendorJobDetail_New.EditIndex = -1;
        lberror.Text = "";
        lberror.Visible = false;
    }

    protected void gvVendorJobDetail_New_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVendorJobDetail_New.PageIndex = e.NewPageIndex;
        gvVendorJobDetail_New.DataBind();
    }

    protected void DataSourceVendorJobs2_Updated(Object source, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result > 0)
        {
            lberror.Text = "Successfully Saved Billing Vehicle Details.";
            lberror.CssClass = "success";
            gvVendorJobDetail_New.EditIndex = -1;
            gvVendorJobDetail_New.DataBind();

        }
        else if (Result == 1)
        {
            lberror.Text = "Error While Saving Billing Vehicle Details.";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Record already exists for transporter and vehicle.";
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    #region DOCUMENT UPLOAD

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupDocument.Hide();
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        if (fuDocument.HasFile != false && fuDocument.PostedFile.FileName.ToString() != "")
        {
            #region  .... UPLOAD DOCUMENT IN FOLDER

            string strCustDocFolder = "", strJobFileDir = "", strFilePath = "", strUploadPath = "";
            DataSet dsJobDetail = DBOperations.GetJobDetail(Convert.ToInt32(Session["JobId"]));                   // Get Job Detail
            if (dsJobDetail.Tables[0].Rows.Count > 0)
            {
                if (dsJobDetail.Tables[0].Rows[0]["DocFolder"] != DBNull.Value)
                    strCustDocFolder = dsJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\";

                if (dsJobDetail.Tables[0].Rows[0]["FileDirName"] != DBNull.Value)
                    strJobFileDir = dsJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
                strUploadPath = strCustDocFolder + strJobFileDir;
            }

            if (fuDocument.HasFile != false && fuDocument.FileName.Trim() != "")
                strFilePath = UploadDocument(strUploadPath, fuDocument);

            #endregion

            if (strFilePath != "")
            {
                int result = -1, DeliveryId = 0;
                if (hdnVehicleDeliveryId.Value != "")
                    DeliveryId = Convert.ToInt32(hdnVehicleDeliveryId.Value);

                DataSet dsGetJobDetailForTransVendor = new DataSet();
                dsGetJobDetailForTransVendor = DBOperations.GetJobDetailForTransVendor(Convert.ToInt32(hdnJobId.Value), DeliveryId, Convert.ToInt32(Session["VendorId"]));
                if (dsGetJobDetailForTransVendor != null && dsGetJobDetailForTransVendor.Tables.Count > 0)
                {
                    if (dsGetJobDetailForTransVendor.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsGetJobDetailForTransVendor.Tables[0].Rows.Count; i++)
                        {
                            if (dsGetJobDetailForTransVendor.Tables[0].Rows[i]["lid"].ToString() != "")
                            {
                                result = DBOperations.AddTransportInvoiceCopy(Convert.ToInt32(dsGetJobDetailForTransVendor.Tables[0].Rows[i]["lid"].ToString()), strFilePath, fuDocument.FileName, Convert.ToInt32(Session["VendorId"]));
                            }
                        }
                    }
                }

                if (result == 0)
                {
                    if (hdnTransportBillStatus.Value != "" && hdnTransportBillStatus.Value == "2")
                    {
                        int result2 = DBOperations.UpdateJobBillStatus(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(0), Convert.ToInt32(Session["VendorId"]),
                                   Convert.ToInt32(0), Convert.ToDateTime(DateTime.MinValue), Convert.ToString(""), Convert.ToString(""));
                        if (result2 == 0)
                        {
                            ModalPopupDocument.Hide();
                            lberror.Text = "Successfully saved invoice copy. Job Submitted to transport department.";
                            lberror.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        ModalPopupDocument.Hide();
                        lberror.Text = "Successfully saved invoice copy. Job Submitted to transport department.";
                        lberror.ForeColor = Color.Green;
                    }
                }
                else if (result == 1)
                {
                    ModalPopupDocument.Hide();
                    lberror.Text = "System Error! Please Try After Sometime!";
                    lberror.ForeColor = Color.Red;
                }
                else if (result == 2)
                {
                    ModalPopupDocument.Show();
                    lbError_Popup.Text = "Document Already Exist.";
                    lbError_Popup.ForeColor = Color.Red;
                }
            }
        }
    }

    private string UploadDocument(string FilePath, FileUpload fuUpload)
    {
        string FileName = fuUpload.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "TransportInvoice_" + hdnJobId.Value + "\\";

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
        if (fuUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuUpload.SaveAs(ServerFilePath + FileName);
            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    public string UploadFiles(FileUpload FU, string FilePath)
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

            return FilePath + FileName;
        }
        else
        {
            return "";
        }

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

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "DeliveredVehicleDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvVendorJobDetail_New.AllowPaging = false;
        gvVendorJobDetail_New.AllowSorting = false;
        gvVendorJobDetail_New.Columns[16].Visible = false;
        gvVendorJobDetail_New.Columns[5].Visible = false;
        gvVendorJobDetail_New.Columns[6].Visible = false;
        gvVendorJobDetail_New.Columns[3].Visible = true;
        gvVendorJobDetail_New.Columns[4].Visible = true;

        gvVendorJobDetail_New.Caption = "Vehicle Detail " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TripDetail.aspx";
        DataFilter1.FilterDataSource();
        gvVendorJobDetail_New.DataBind();

        gvVendorJobDetail_New.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            DataFilter1.FilterSessionID = "TripDetail.aspx";
            DataFilter1.FilterDataSource();
            gvVendorJobDetail_New.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}