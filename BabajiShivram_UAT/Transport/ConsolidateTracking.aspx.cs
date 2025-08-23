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
using Ionic.Zip;

public partial class Transport_ConsolidateTracking : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        //lblError.Text = Session["TRId"].ToString();
        ViewState["TotalAmt"] = 0;
        if (Session["TRConsolidateId"] == null)
        {
            Response.Redirect("BillHistory.aspx");
        }

        if (!IsPostBack)
        {
            lblError.Text = Session["TRConsolidateId"].ToString();
            TruckRequestDetail(Convert.ToInt32(Session["TRConsolidateId"]));
        }
    }

    private void TruckRequestDetail(int ConsolidateId)
    {
        
        DataView dvDetail = DBOperations.GetConsolidateRequestById(ConsolidateId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TransRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Consolidate Tracking - " + lblTRRefNo.Text;

            lblTransporter.Text = dvDetail.Table.Rows[0]["TransporterName"].ToString();
            lblJobCreatedBy.Text = dvDetail.Table.Rows[0]["CreatedBy"].ToString();
            if (dvDetail.Table.Rows[0]["CreatdDate"] != DBNull.Value)
                lblJobCreatedDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["CreatdDate"]).ToString("dd/MM/yyyy");
            int TransReqId = Convert.ToInt32(Session["TRId"]);
            //lblError.Text = TransReqId.ToString();
            GridViewVehicle.DataBind();
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

    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpeDailyStatus.Hide();
        gvVehicleDailyStatus.DataBind();
    }

    protected void btnCancelEmailPp_Click(object sender, EventArgs e)
    {
        mpeDailyStatus.Hide();
        gvVehicleDailyStatus.DataBind();
    }

    #region SELLING RATE

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
}