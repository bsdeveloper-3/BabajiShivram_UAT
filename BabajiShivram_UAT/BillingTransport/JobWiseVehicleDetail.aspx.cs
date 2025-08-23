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
public partial class BillingTransport_JobWiseVehicleDetail : System.Web.UI.Page
{
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["UserId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Job Wise Vehicle Details";
            }
            else
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("BillTransportLogin.aspx");
            }
        }

        DataFilter1.DataSource = DataSourceVendorJobs;
        DataFilter1.DataColumns = gvVendorJobDetail.Columns;
        DataFilter1.FilterSessionID = "JobWiseVehicleDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GRID VIEW EVENTS

    protected void gvVendorJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();
            if (strDocPath != "")
            {
                LinkButton lnkUploadDoc = (LinkButton)e.Row.FindControl("lnkDocUpload");
                lnkUploadDoc.Visible = false;

                LinkButton lnkDownloadInvoice = (LinkButton)e.Row.FindControl("lnkDownloadInvoice");
                lnkDownloadInvoice.Visible = true;
                ScriptManager1.RegisterPostBackControl(lnkDownloadInvoice);
            }
            else
            {
                LinkButton lnkUploadDoc = (LinkButton)e.Row.FindControl("lnkDocUpload");
                lnkUploadDoc.Visible = true;
                ScriptManager1.RegisterPostBackControl(lnkUploadDoc);

                LinkButton lnkDownloadInvoice = (LinkButton)e.Row.FindControl("lnkDownloadInvoice");
                lnkDownloadInvoice.Visible = false;
            }

            LinkButton lnkbtnVehicle = (LinkButton)e.Row.FindControl("lnkbtnVehicle");
            if (lnkbtnVehicle.Text.Trim() != null && lnkbtnVehicle.Text.Trim() != "")
            {
                if (lnkbtnVehicle.Text.Trim() == "0")
                    lnkbtnVehicle.Enabled = false;
                else
                    lnkbtnVehicle.Enabled = true;
            }

            string TotalUpdVehicle = (string)DataBinder.Eval(e.Row.DataItem, "TotalUpdVehicle").ToString();
            if (TotalUpdVehicle != "")
            {
                LinkButton lnkbtnUpdatedVehicle = (LinkButton)e.Row.FindControl("lnkbtnUpdatedVehicle");
                if (TotalUpdVehicle == "0")
                    lnkbtnUpdatedVehicle.Enabled = false;
                else
                    lnkbtnUpdatedVehicle.Enabled = true;
            }


        }
    }

    protected void gvVendorJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "getvehicle")
        {
            //string JobId = e.CommandArgument.ToString();

            //if (JobId != "")
            //{
            //    Session["JobId"] = JobId.ToString();
            //    Response.Redirect("BillingVehicle.aspx");
            //}
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string JobId = commandArgs[0].ToString();
            string strStatus = commandArgs[1].ToString();

            if (JobId != "")
            {
                if (strStatus != "" && strStatus == "2")
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("EditTransporterBill.aspx");
                }
                else
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("BillingVehicle.aspx");
                }
            }
        }
        else if (e.CommandName.ToLower() == "showvehicle")
        {
            //string JobId = e.CommandArgument.ToString();
            //if (JobId != "")
            //{
            //    Session["JobId"] = JobId.ToString();
            //    Response.Redirect("VehicleDelivery.aspx");
            //}

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string JobId = commandArgs[0].ToString();
            string strStatus = commandArgs[1].ToString();

            if (JobId != "")
            {
                if (strStatus != "" && strStatus == "2")
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("EditTransporterBill.aspx");
                }
                else
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("VehicleDelivery.aspx");
                }
            }
        }
        else if (e.CommandName == "documentpopup")
        {
            //GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //int Index = row.RowIndex;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string strJobId = commandArgs[0].ToString();
            string PendingVehicle = commandArgs[1].ToString();
            string strVehicleDeliveryId = commandArgs[2].ToString();
            string strBillStatus = commandArgs[3].ToString();

            if (PendingVehicle != "" && strJobId != "")
            {
                if (PendingVehicle != "0")
                {
                    lberror.Text = "Please update vehicle delivery details before uploading invoice copy!!!";
                    lberror.CssClass = "errorMsg";
                    ModalPopupDocument.Hide();
                }
                else
                {
                    hdnVehicleDeliveryId.Value = strVehicleDeliveryId;
                    hdnTransportBillStatus.Value = strBillStatus;
                    if (hdnTransportBillStatus.Value != "" && hdnTransportBillStatus.Value == "2")
                    {
                        int count = DBOperations.GetRejectedVehicle(Convert.ToInt32(strJobId), Convert.ToInt32(Session["VendorId"]));
                        if (count == 0)
                        {
                            hdnJobId.Value = strJobId;
                            ModalPopupDocument.Show();
                            lbError_Popup.Text = "";
                        }
                        else
                        {
                            ModalPopupDocument.Hide();
                            lberror.Text = "Please update the rejected bill details before uploading invoice copy.";
                            lberror.CssClass = "errorMsg";
                        }
                    }
                    else
                    {
                        hdnJobId.Value = strJobId;
                        ModalPopupDocument.Show();
                        lbError_Popup.Text = "";
                    }
                }
            }
        }
        else if (e.CommandName == "downloaddoc")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
    }

    protected void gvVendorJobDetail_PreRender(object sender, EventArgs e)
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

        gvVendorJobDetail.AllowPaging = false;
        gvVendorJobDetail.AllowSorting = false;
        gvVendorJobDetail.Columns[16].Visible = false;
        gvVendorJobDetail.Columns[5].Visible = false;
        gvVendorJobDetail.Columns[6].Visible = false;
        gvVendorJobDetail.Columns[3].Visible = true;
        gvVendorJobDetail.Columns[4].Visible = true;

        gvVendorJobDetail.Caption = "Vehicle Detail " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "JobWiseVehicleDetail.aspx";
        DataFilter1.FilterDataSource();
        gvVendorJobDetail.DataBind();

        gvVendorJobDetail.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "JobWiseVehicleDetail.aspx";
            DataFilter1.FilterDataSource();
            gvVendorJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}