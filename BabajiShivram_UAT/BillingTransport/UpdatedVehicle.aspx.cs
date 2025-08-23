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

public partial class BillingTransport_UpdatedVehicle : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInVendor = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["UserId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Delivered Vehicle";
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
        DataFilter1.FilterSessionID = "UpdatedVehicle.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GRID VIEW EVENTS

    protected void gvVendorJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "selectvehicles")
        {
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

        if (e.CommandName.ToLower() == "downloaddoc")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }

        else if (e.CommandName == "documentpopup")
        {
            //GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            //int Index = row.RowIndex;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string strJobId = commandArgs[0].ToString();
            string strVehicleDeliveryId = commandArgs[1].ToString();

            if (strJobId != "")
            {
                hdnVehicleDeliveryId.Value = strVehicleDeliveryId;
                hdnJobId.Value = strJobId;
                ModalPopupDocument.Show();
                lbError_Popup.Text = "";
            }
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

    protected void gvVendorJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string DocumentPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();
            LinkButton lnkDownloadInvoice = (LinkButton)e.Row.FindControl("lnkDownloadInvoice");
            LinkButton lnkDocUpload = (LinkButton)e.Row.FindControl("lnkDocUpload");

            if (DocumentPath == "")
            {
                lnkDownloadInvoice.Visible = false;
                lnkDocUpload.Visible = true;
            }
            else
            {
                lnkDownloadInvoice.Visible = true;
                lnkDocUpload.Visible = false;
                if (lnkDownloadInvoice != null)
                    ScriptManager1.RegisterPostBackControl(lnkDownloadInvoice);
            }
        }
    }

    #endregion

    #region DOCUMENTS DOWNLOAD EVENTS

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
                     int result2 = DBOperations.UpdateJobBillStatus(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(0), Convert.ToInt32(Session["VendorId"]),
                                    Convert.ToInt32(0), Convert.ToDateTime(DateTime.MinValue), Convert.ToString(""), Convert.ToString(""));
                     if (result2 == 0)
                     {
                         ModalPopupDocument.Hide();
                         lberror.Text = "Successfully saved invoice copy.";
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
        if (LoggedInVendor.glEmpName.ToString() != null && LoggedInVendor.glEmpName != "")
        {
            // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
            string strFileName = "DeliveredJobVehicleDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
            DataSet dsDeliveredVehicle = DBOperations.GetJobWsVehicleDetReport(Convert.ToInt32(Session["VendorId"]));
            ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dsDeliveredVehicle.Tables[0]);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType, DataTable dtReport)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            dtReport.TableName = "Delivered Vehicle Details";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (dtReport.Rows.Count > 0)
            {
                dtReport.Columns.Add("Sr.No.", typeof(int));
                int i = 1;
                foreach (DataRow dr in dtReport.Rows)
                {
                    dr["Sr.No."] = i;
                    i = i + 1;
                }
                dtReport.Columns["Sr.No."].SetOrdinal(0);

                var workSheet = wb.Worksheets.Add(dtReport);
                var SrNo_Col = workSheet.Column("A");
                SrNo_Col.Width = 8;
                var JobRefNo = workSheet.Column("B");
                JobRefNo.Width = 20;
                var Vehicles = workSheet.Column("C");
                Vehicles.Width = 52;
                var UpdVehicles = workSheet.Column("D");
                UpdVehicles.Width = 12;
                var Deliveryfrom = workSheet.Column("E");
                Deliveryfrom.Width = 17;
                var DeliveryTo = workSheet.Column("F");
                DeliveryTo.Width = 27;
                var DispatchDate = workSheet.Column("G");
                DispatchDate.Width = 16;
                var Freightrate = workSheet.Column("H");
                Freightrate.Width = 10;
                var DetentionTot = workSheet.Column("I");
                DetentionTot.Width = 14;
                var WaraiTot = workSheet.Column("J");
                WaraiTot.Width = 10;
                var EmptyOffTot = workSheet.Column("K");
                EmptyOffTot.Width = 15;
                var TempoUnionTot = workSheet.Column("L");
                TempoUnionTot.Width = 10;

                workSheet.Style.Alignment.WrapText = true;
                workSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
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
            DataFilter1.FilterSessionID = "UpdatedVehicle.aspx";
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