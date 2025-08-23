using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class PCA_TransportBillStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("BillStatus.aspx");
        }

        else if (!IsPostBack)
        {
            int TranRequestId = Convert.ToInt32(Session["JobId"]);
            TruckRequestDetail(TranRequestId);
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Job Detail";
        }
    }

    protected void TruckRequestDetail(int TransportId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TransportId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Request Received - " + lblTRRefNo.Text;
            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
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
            lblDeliveryType.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            if (dvDetail.Table.Rows[0]["DocFolder"] != DBNull.Value)
                strCustDocFolder = dvDetail.Table.Rows[0]["DocFolder"].ToString() + "\\";
            if (dvDetail.Table.Rows[0]["FileDirName"] != DBNull.Value)
                strJobFileDir = dvDetail.Table.Rows[0]["FileDirName"].ToString() + "\\";
        }
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("BillStatus.aspx");
    }

}