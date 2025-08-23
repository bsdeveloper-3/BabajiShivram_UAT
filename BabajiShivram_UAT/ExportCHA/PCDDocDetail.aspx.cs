using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class ExportPCA_PCDDocDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["JobId"] == null)
            {
                Response.Redirect("ExPendingPCD.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Job Shipping Detail";
                GetShippingDetail(Convert.ToInt32(Session["JobId"]));
            }
        }
    }

    protected void GetShippingDetail(int JobId)
    {
        DataSet dsGetShippingDetails = new DataSet();
        dsGetShippingDetails = EXOperations.GetShippingDetailByJobId(JobId);

        if (dsGetShippingDetails.Tables.Count > 0 && dsGetShippingDetails.Tables[0].Rows.Count > 0)
        {
            lblJobRefNo.Text = dsGetShippingDetails.Tables[0].Rows[0]["JobRefNo"].ToString();
            lblCustRefNo.Text = dsGetShippingDetails.Tables[0].Rows[0]["CustRefNo"].ToString();
            lblCustomer.Text = dsGetShippingDetails.Tables[0].Rows[0]["Customer"].ToString();
            lblConsignee.Text = dsGetShippingDetails.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblShipper.Text = dsGetShippingDetails.Tables[0].Rows[0]["Shipper"].ToString();
            lblTransMode.Text = dsGetShippingDetails.Tables[0].Rows[0]["TransMode"].ToString();
            lblBuyerName.Text = dsGetShippingDetails.Tables[0].Rows[0]["BuyerName"].ToString();
            lblProductDesc.Text = dsGetShippingDetails.Tables[0].Rows[0]["ProductDesc"].ToString();
            lblPortOfLoading.Text = dsGetShippingDetails.Tables[0].Rows[0]["PortOfLoading"].ToString();
            lblPortOfDischarge.Text = dsGetShippingDetails.Tables[0].Rows[0]["PortOfDischarge"].ToString();
            lblConsignmentCountry.Text = dsGetShippingDetails.Tables[0].Rows[0]["ConsignmentCountry"].ToString();
            lblDestCountry.Text = dsGetShippingDetails.Tables[0].Rows[0]["DestinationCountry"].ToString();
            lblNoOfPkg.Text = dsGetShippingDetails.Tables[0].Rows[0]["NoOfPackages"].ToString();
            lblShippingBillType.Text = dsGetShippingDetails.Tables[0].Rows[0]["ShippingBillType"].ToString();
            lblForwarderName.Text = dsGetShippingDetails.Tables[0].Rows[0]["ForwarderName"].ToString();
            lblContainerLoaded.Text = dsGetShippingDetails.Tables[0].Rows[0]["ContainerLoaded"].ToString();
            lblGrossWT.Text = dsGetShippingDetails.Tables[0].Rows[0]["GrossWT"].ToString();
            lblNetWT.Text = dsGetShippingDetails.Tables[0].Rows[0]["NetWT"].ToString();
            lblSBNo.Text = dsGetShippingDetails.Tables[0].Rows[0]["SBNo"].ToString();
            if (dsGetShippingDetails.Tables[0].Rows[0]["SBDate"] != DBNull.Value)
                lblSBDate.Text = Convert.ToDateTime(dsGetShippingDetails.Tables[0].Rows[0]["SBDate"]).ToString("dd/MM/yyyy");
            if (dsGetShippingDetails.Tables[0].Rows[0]["LEODate"] != DBNull.Value)
                lblLEODate.Text = Convert.ToDateTime(dsGetShippingDetails.Tables[0].Rows[0]["LEODate"]).ToString("dd/MM/yyyy");
            if (dsGetShippingDetails.Tables[0].Rows[0]["ShippingLineDate"] != DBNull.Value)
                lblShippingLineDate.Text = Convert.ToDateTime(dsGetShippingDetails.Tables[0].Rows[0]["ShippingLineDate"]).ToString("dd/MM/yyyy");
            if (dsGetShippingDetails.Tables[0].Rows[0]["ExporterCopyPath"] != DBNull.Value &&
                dsGetShippingDetails.Tables[0].Rows[0]["ExporterCopyPath"].ToString() != "")
            {
                lnkbtnDownloadExpCopy.Text = "Download";
                hdnExporterCopyPath.Value = dsGetShippingDetails.Tables[0].Rows[0]["ExporterCopyPath"].ToString();
                lnkbtnDownloadExpCopy.Enabled = true;
            }
            else
            {
                lnkbtnDownloadExpCopy.Text = "Not Uploaded";
                lnkbtnDownloadExpCopy.Enabled = false;
            }
            if (dsGetShippingDetails.Tables[0].Rows[0]["VGMCopyPath"] != DBNull.Value &&
               dsGetShippingDetails.Tables[0].Rows[0]["VGMCopyPath"].ToString() != "")
            {
                lnkbtnDwnloadVGMCopy.Text = "Download";
                hdnVGMCopyPath.Value = dsGetShippingDetails.Tables[0].Rows[0]["VGMCopyPath"].ToString();
                lnkbtnDwnloadVGMCopy.Enabled = true;
            }
            else
            {
                lnkbtnDwnloadVGMCopy.Text = "Not Uploaded";
                lnkbtnDwnloadVGMCopy.Enabled = false;
            }
            if (dsGetShippingDetails.Tables[0].Rows[0]["FreightForwardedDate"] != DBNull.Value)
                FreightForDate.Text = Convert.ToDateTime(dsGetShippingDetails.Tables[0].Rows[0]["FreightForwardedDate"]).ToString("dd/MM/yyyy");
            lblForwarderPersonName.Text = dsGetShippingDetails.Tables[0].Rows[0]["ForwarderPersonName"].ToString();
            lblTransportBy.Text = dsGetShippingDetails.Tables[0].Rows[0]["TransportBy"].ToString();
            lblExportType.Text = dsGetShippingDetails.Tables[0].Rows[0]["ExportType"].ToString();
        }
    }

    #region DOCUMENT UPLOAD / DOWNLOAD EVENTS

    protected void lnkbtnDownloadExpCopy_OnClick(object sender, EventArgs e)
    {
        if (hdnExporterCopyPath.Value != "")
            DownloadDocument(hdnExporterCopyPath.Value);
    }

    protected void lnkbtnDwnloadVGMCopy_OnClick(object sender, EventArgs e)
    {
        if (hdnVGMCopyPath.Value != "")
            DownloadDocument(hdnVGMCopyPath.Value);
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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

    #endregion
}