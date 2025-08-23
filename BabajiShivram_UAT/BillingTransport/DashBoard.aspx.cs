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
public partial class BillingTransport_DashBoard : System.Web.UI.Page
{
    LoginClass LoggedInVendor = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveDocument);
        if (Session["VendorId"] != null)
        {
            if (LoggedInVendor.glEmpName.ToString() != null && LoggedInVendor.glEmpName != "")
            {
                Session["Transporter"] = LoggedInVendor.glEmpName.ToString();
            }
        }
        else
        {
            Response.Redirect("~/VendorLogin.aspx");
        }
    }

    #region REJECTED BILLS

    protected void gvRejectedJobs_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvRejectedJobs_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int DetentionDays = 0, count = 0, lid = 0, DeliveryId = 0, Packages = 0, VehicleType = 0, TransporterVehicleId = 0;
        string DocPath = "", DeliveryPoint = "", DeliveryFrom = "", VehicleNo = "", TransporterName = "";
        DateTime DispatchDate = DateTime.MinValue;

        lid = Convert.ToInt32(gvRejectedJobs.DataKeys[e.RowIndex].Value.ToString());
        VehicleNo = gvRejectedJobs.Rows[e.RowIndex].Cells[1].Text;
        DeliveryPoint = gvRejectedJobs.Rows[e.RowIndex].Cells[2].Text;
        if (gvRejectedJobs.Rows[e.RowIndex].Cells[3].Text != "")
            DispatchDate = Convert.ToDateTime(gvRejectedJobs.Rows[e.RowIndex].Cells[3].Text);
        TextBox txtDetentionCharges = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtDetentionCharges");
        TextBox txtDetentionDays = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtDetentionDays");
        TextBox txtWaraiCharges = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtWaraiCharges");
        TextBox txtEmptyOffLoadingCharges = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtEmptyOffLoadingCharges");
        TextBox txtTempoUnionCharges = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtTempoUnionCharges");
        TextBox txtFreightRate = (TextBox)gvRejectedJobs.Rows[e.RowIndex].FindControl("txtFreightRate");

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
            //    //JobId = 0; // Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["JobId"].ToString());
            //    DeliveryId = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["DeliveryId"].ToString());
            //    Packages = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["Packages"].ToString());
            //    VehicleType = Convert.ToInt32(dsGetDeliveryDetails.Tables[0].Rows[0]["VehicleType"].ToString());
            //    DeliveryFrom = dsGetDeliveryDetails.Tables[0].Rows[0]["DeliveryFrom"].ToString();
            //    TransporterName = ""; // dsGetDeliveryDetails.Tables[0].Rows[0]["TransporterName"].ToString();
            //}

            if (DeliveryId != 0)
            {
                //DataSourceRejectedJobs.UpdateParameters["lid"].DefaultValue = Convert.ToString(lid);
                DataSourceRejectedJobs.UpdateParameters["lUser"].DefaultValue = Convert.ToString(Session["VendorId"]);
                // DataSourceRejectedJobs.UpdateParameters["VehicleNo"].DefaultValue = Convert.ToString(VehicleNo);
                DataSourceRejectedJobs.UpdateParameters["TPFrightRate"].DefaultValue = Convert.ToString(txtFreightRate.Text.Trim());
                DataSourceRejectedJobs.UpdateParameters["DetentionDays"].DefaultValue = Convert.ToString(DetentionDays);
                DataSourceRejectedJobs.UpdateParameters["DetentionCharges"].DefaultValue = Convert.ToString(DetentionCharges);
                DataSourceRejectedJobs.UpdateParameters["VaraiCharges"].DefaultValue = Convert.ToString(VaraiCharges);
                DataSourceRejectedJobs.UpdateParameters["EmptyOffLoadingCharges"].DefaultValue = Convert.ToString(EmptyOffLoadingCharges);
                DataSourceRejectedJobs.UpdateParameters["TempoUnionCharges"].DefaultValue = Convert.ToString(TempoUnionCharges);
                DataSourceRejectedJobs.UpdateParameters["Total"].DefaultValue = Convert.ToString(Convert.ToDecimal(Total));
                DataSourceRejectedJobs.UpdateParameters["Remarks"].DefaultValue = "";
                DataSourceRejectedJobs.UpdateParameters["LrCopiesDocPath"].DefaultValue = "";
                DataSourceRejectedJobs.UpdateParameters["ReceiptDocPath"].DefaultValue = "";
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

    protected void gvRejectedJobs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRejectedJobs.EditIndex = e.NewEditIndex;
        lberror.Text = "";
        lberror.Visible = false;
    }

    protected void gvRejectedJobs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRejectedJobs.EditIndex = -1;
        lberror.Text = "";
        lberror.Visible = false;
    }

    protected void gvRejectedJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRejectedJobs.PageIndex = e.NewPageIndex;
        gvRejectedJobs.DataBind();
    }

    protected void DataSourceRejectedJobs_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        // e.Command.Parameters.Remove("lid");
    }

    protected void DataSourceRejectedJobs_Updated(Object source, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);
        if (Result == 0)
        {
            lberror.Text = "Successfully Saved Billing Vehicle Details.";
            lberror.CssClass = "success";
            gvRejectedJobs.EditIndex = -1;
            gvRejectedJobs.DataBind();
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

    #region GRID VIEW EVENTS

    //protected void gvPendingUpdates_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();

    //        if (strDocPath == "" || strDocPath == null)
    //        {
    //            LinkButton lnkUploadDoc = (LinkButton)e.Row.FindControl("lnkDocUpload");
    //            lnkUploadDoc.Visible = true;
    //        }
    //        else
    //        {
    //            LinkButton lnkUploadDoc = (LinkButton)e.Row.FindControl("lnkDocUpload");
    //            lnkUploadDoc.Visible = false;
    //        }

    //        string UpdatedVehicle = (string)DataBinder.Eval(e.Row.DataItem, "UpdatedVehicle").ToString();
    //        string TotalVehicle = (string)DataBinder.Eval(e.Row.DataItem, "TotalVehicle").ToString();

    //        if (UpdatedVehicle == TotalVehicle)
    //        {
    //            LinkButton lnkbtnCompVehicle = (LinkButton)e.Row.FindControl("lnkbtnCompVehicle");
    //            LinkButton lnkbtnVehicle = (LinkButton)e.Row.FindControl("lnkbtnVehicle");
    //            lnkbtnVehicle.Visible = false;
    //            lnkbtnCompVehicle.Visible = true;
    //        }
    //        else if (Convert.ToInt32(UpdatedVehicle) < Convert.ToInt32(TotalVehicle))
    //        {
    //            LinkButton lnkUploadDoc = (LinkButton)e.Row.FindControl("lnkDocUpload");
    //            lnkUploadDoc.Visible = false;
    //            LinkButton lnkbtnCompVehicle = (LinkButton)e.Row.FindControl("lnkbtnCompVehicle");
    //            LinkButton lnkbtnVehicle = (LinkButton)e.Row.FindControl("lnkbtnVehicle");
    //            lnkbtnVehicle.Visible = true;
    //            lnkbtnCompVehicle.Visible = false;
    //        }
    //        else
    //        {
    //            //Label lblNoOfVehicle = (Label)e.Row.FindControl("lblNoOfVehicle");
    //            LinkButton lnkbtnVehicle = (LinkButton)e.Row.FindControl("lnkbtnVehicle");
    //            lnkbtnVehicle.Visible = true;
    //            // lblNoOfVehicle.Visible = false;
    //        }
    //    }
    //}

    protected void gvPendingUpdates_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "gototrip")
        {
            Response.Redirect("../BillingTransport/TripDetail.aspx");
        }
    }

    protected void gvRejectedJobs_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string JobId = e.CommandArgument.ToString();

            Session["TransReqId"] = JobId.ToString();
            Response.Redirect("VendorBillSubmission.aspx");
        }
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
                //        gvRejectedJobs.DataBind();
                //        lberror.Text = "";
                //    }
                //}
            }
        }
    }

    protected void gvRejectedJobs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void gvPendingVehicles_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "getjobs")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string VehicleNo = commandArgs[0];
            string DispatchDate = commandArgs[1];

            if (VehicleNo != "" && DispatchDate != "")
            {
                Session["VehicleNo"] = VehicleNo;
                Session["DispatchDate"] = DispatchDate;
                Response.Redirect("EditVehicleDetails.aspx");
            }
        }
    }

    protected void gvJobsApproved_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "selectvehicles")
        {
            string JobId = e.CommandArgument.ToString();
            if (JobId != "")
            {
                Session["JobId"] = JobId.ToString();
                Response.Redirect("VehicleDelivery.aspx");
            }
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
                            //gvPendingUpdates.DataBind();
                            gvRejectedJobs.DataBind();
                            gvJobsApproved.DataBind();
                            lberror.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        ModalPopupDocument.Hide();
                        lberror.Text = "Successfully saved invoice copy. Job Submitted to transport department.";
                        //gvPendingUpdates.DataBind();
                        gvRejectedJobs.DataBind();
                        gvJobsApproved.DataBind();
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
}