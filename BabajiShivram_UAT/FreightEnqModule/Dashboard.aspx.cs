using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

public partial class FreightEnqModule_Dashboard : System.Web.UI.Page
{
    int Month = 0;
    string str_Message = "";

    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["VendorId"]) != null)
            {
                ddMonth.SelectedValue = DateTime.Now.Month.ToString();
                GetFreightSummary();
                if (loggedInUser.glEmpName.ToString() != "")
                    lblUserName.Text = loggedInUser.glEmpName.ToString();
                GetEnquiryDetails();
                GetInboxMsgs();

                if (Request.QueryString.Count > 0)
                {
                    string op = "";
                    op = Decrypt(HttpUtility.UrlDecode(Request.QueryString["op"]));

                    if (op == "1")
                    {
                        Session["MessageTo"] = "0";
                        GetRecentChats();
                        GetOnlineUsers();
                        ModalPopupExtender1.Show();
                        int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                    }
                    else if (op == "0")
                    {
                        Session["MessageTo"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));
                        GetRecentChats();
                        GetOnlineUsers();
                        ReadMessages();
                        ModalPopupExtender1.Show();
                        int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                    }
                }
                else
                {
                    if (Convert.ToString(Session["MessageTo"]) != null &&
                        Convert.ToString(Session["MessageTo"]) != "" &&
                        Convert.ToString(Session["MessageTo"]) != "0")
                    {
                        GetRecentChats();
                        GetOnlineUsers();
                        ReadMessages();
                        ModalPopupExtender1.Show();
                        int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                    }
                }
            }
            else
                Response.Redirect("FRlogin.aspx");
        }
    }

    protected void ddMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Month = Convert.ToInt32(ddMonth.SelectedValue);
            GetFreightSummary();
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Month = Convert.ToInt32(ddMonth.SelectedValue);
            GetFreightSummary();
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void txtCustomer_OntextChanged(object sender, EventArgs e)
    {
        try
        {
            Month = Convert.ToInt32(ddMonth.SelectedValue);
            GetFreightSummary();
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetEnquiryDetails()
    {
        try
        {
            StringBuilder strBuildEnquiry = new StringBuilder();
            if (loggedInUser.glRoleId.ToString() != "")
            {
                if (loggedInUser.glRoleId.ToString() == "53")
                {
                    #region NO OF NEW ENQUIRY
                    DataSet dsGetNoofEnquiries = new DataSet();
                    dsGetNoofEnquiries = DBOperations.GetNoOfEnquiries("SelectAll", 0);
                    if (dsGetNoofEnquiries != null && dsGetNoofEnquiries.Tables.Count > 0)
                    {
                        if (dsGetNoofEnquiries.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-3' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF ENQUIRIES -->
							                        <div class='box default'>
								                        <div class='box-title'>
									                        <h4>" + dsGetNoofEnquiries.Tables[0].Rows[0]["TotalEnquiry"].ToString() + " Enquiry</h4>" +
                                                            "<small class='block'>" + dsGetNoofEnquiries.Tables[0].Rows[0]["TodaysEnquiry"].ToString() + " New enquiry today</small>" +
                                                            "<i class='fa fa-phone'></i>" +
                                                        "</div>" +
                                                        "<div class='box-body text-center' style='border: 2px solid #999999; color: #999999; padding: 8px; font-size: 13px; text-align: inherit;'>" +
                                                            "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewAllEnquiry.aspx' style='color: #999999;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF QUOTED ENQUIRY
                    DataSet dsGetQuotedEnquiry = new DataSet();
                    dsGetQuotedEnquiry = DBOperations.GetQuotedEnquiryCnt("SelectAll", 0);
                    if (dsGetQuotedEnquiry != null && dsGetQuotedEnquiry.Tables.Count > 0)
                    {
                        if (dsGetQuotedEnquiry.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF QUOTED ENQUIRIES -->
							                        <div class='box warning'>
								                        <div class='box-title'>
									                        <h4>" + dsGetQuotedEnquiry.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Quoted</h4>" +
                                                            "<small class='block'>" + dsGetQuotedEnquiry.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry quoted today</small>" +
                                                            "<i class='fa fa-check'></i>" +
                                                        "</div>" +
                                                        "<div class='box-body text-center' style='border: 2px solid #f4b04f; color: #f4b04f; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='AllQuotedEnquiry.aspx' style='color: #f4b04f;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF QUOTED ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF AWARDED ENQUIRY
                    DataSet dsGetAwardedEnq = new DataSet();
                    dsGetAwardedEnq = DBOperations.GetCntOfEnquiryStatus(3, "SelectAll", 0);
                    if (dsGetAwardedEnq != null && dsGetAwardedEnq.Tables.Count > 0)
                    {
                        if (dsGetAwardedEnq.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF AWARDED ENQUIRIES -->
							                        <div class='box info'>
								                        <div class='box-title'>
									                        <h4>" + dsGetAwardedEnq.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Awarded</h4>" +
                                                            "<small class='block'>" + dsGetAwardedEnq.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry awarded today</small>" +
                                                            "<i class='fa fa-trophy'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #1D89CF; color: #1D89CF; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewAwardedEnq.aspx' style='color: #1D89CF;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF AWARDED ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF EXECUTED ENQUIRY
                    DataSet dsGetExecEnquiry = new DataSet();
                    dsGetExecEnquiry = DBOperations.GetCntOfEnquiryStatus(5, "SelectAll", 0);
                    if (dsGetExecEnquiry != null && dsGetExecEnquiry.Tables.Count > 0)
                    {
                        if (dsGetExecEnquiry.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF AWARDED ENQUIRIES -->
							                        <div class='box success'>
								                        <div class='box-title'>
									                        <h4>" + dsGetExecEnquiry.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Executed</h4>" +
                                                            "<small class='block'>" + dsGetExecEnquiry.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry executed today</small>" +
                                                            "<i class='fa fa-thumbs-up'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #5CB85C; color: green; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewExecEnquiry.aspx' style='color: #5CB85C;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF AWARDED ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF LOST ENQUIRY
                    DataSet dsGetLostEnq = new DataSet();
                    dsGetLostEnq = DBOperations.GetCntOfEnquiryStatus(4, "SelectAll", 0);
                    if (dsGetLostEnq != null && dsGetLostEnq.Tables.Count > 0)
                    {
                        if (dsGetLostEnq.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-3' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF LOST ENQUIRIES -->
							                        <div class='box danger'>
								                        <div class='box-title'>
									                        <h4>" + dsGetLostEnq.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Lost</h4>" +
                                                            "<small class='block'>" + dsGetLostEnq.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry lost today</small>" +
                                                            "<i class='fa fa-thumbs-down'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #E66454; color: #E66454; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewLostEnquiry.aspx' style='color: #E66454;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF LOST ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    dvPendingNumbers.InnerHtml = strBuildEnquiry.ToString();
                }
                else
                {
                    #region NO OF NEW ENQUIRY
                    DataSet dsGetNoofEnquiries = new DataSet();
                    dsGetNoofEnquiries = DBOperations.GetNoOfEnquiries("SelectOne", Convert.ToInt32(Session["VendorId"]));
                    if (dsGetNoofEnquiries != null && dsGetNoofEnquiries.Tables.Count > 0)
                    {
                        if (dsGetNoofEnquiries.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-3' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF ENQUIRIES -->
							                        <div class='box default'>
								                        <div class='box-title'>
									                        <h4>" + dsGetNoofEnquiries.Tables[0].Rows[0]["TotalEnquiry"].ToString() + " Enquiry</h4>" +
                                                             "<small class='block'>" + dsGetNoofEnquiries.Tables[0].Rows[0]["TodaysEnquiry"].ToString() + " New enquiry today</small>" +
                                                             "<i class='fa fa-phone'></i>" +
                                                         "</div>" +
                                                         "<div class='box-body text-center' style='border: 2px solid #999999; color: #999999; padding: 8px; font-size: 13px; text-align: inherit;'>" +
                                                             "View Details" +
                                                             "<span class='pull-right'>" +
                                                               "<a href='ViewAllEnquiry.aspx' style='color: #999999;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                             "</span>" +
                                                         "</div>" +
                                                     "</div>" +
                                                     "<!-- /NO OF ENQUIRIES -->" +
                                                     "</div>");
                        }
                    }
                    #endregion

                    #region NO OF QUOTED ENQUIRY
                    DataSet dsGetQuotedEnquiry = new DataSet();
                    dsGetQuotedEnquiry = DBOperations.GetQuotedEnquiryCnt("SelectOne", Convert.ToInt32(Session["VendorId"]));
                    if (dsGetQuotedEnquiry != null && dsGetQuotedEnquiry.Tables.Count > 0)
                    {
                        if (dsGetQuotedEnquiry.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF QUOTED ENQUIRIES -->
							                        <div class='box warning'>
								                        <div class='box-title'>
									                        <h4>" + dsGetQuotedEnquiry.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Quoted</h4>" +
                                                           "<small class='block'>" + dsGetQuotedEnquiry.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry quoted today</small>" +
                                                           "<i class='fa fa-check'></i>" +
                                                       "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #f4b04f; color: #f4b04f; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                            "View Details" +
                                                           "<span class='pull-right'>" +
                                                             "<a href='AllQuotedEnquiry.aspx' style='color: #f4b04f;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                           "</span>" +
                                                       "</div>" +
                                                   "</div>" +
                                                   "<!-- /NO OF QUOTED ENQUIRIES -->" +
                                                   "</div>");
                        }
                    }
                    #endregion

                    #region NO OF AWARDED ENQUIRY
                    DataSet dsGetAwardedEnq = new DataSet();
                    dsGetAwardedEnq = DBOperations.GetCntOfEnquiryStatus(3, "SelectOne", Convert.ToInt32(Session["VendorId"]));
                    if (dsGetAwardedEnq != null && dsGetAwardedEnq.Tables.Count > 0)
                    {
                        if (dsGetAwardedEnq.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF AWARDED ENQUIRIES -->
							                        <div class='box info'>
								                        <div class='box-title'>
									                        <h4>" + dsGetAwardedEnq.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Awarded</h4>" +
                                                            "<small class='block'>" + dsGetAwardedEnq.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry awarded today</small>" +
                                                            "<i class='fa fa-trophy'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #1D89CF; color: #1D89CF; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewAwardedEnq.aspx' style='color: #1D89CF;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF AWARDED ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF EXECUTED ENQUIRY
                    DataSet dsGetExecEnquiry = new DataSet();
                    dsGetExecEnquiry = DBOperations.GetCntOfEnquiryStatus(5, "SelectOne", Convert.ToInt32(Session["VendorId"]));
                    if (dsGetExecEnquiry != null && dsGetExecEnquiry.Tables.Count > 0)
                    {
                        if (dsGetExecEnquiry.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-2' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF AWARDED ENQUIRIES -->
							                        <div class='box success'>
								                        <div class='box-title'>
									                        <h4>" + dsGetExecEnquiry.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Executed</h4>" +
                                                            "<small class='block'>" + dsGetExecEnquiry.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry executed today</small>" +
                                                            "<i class='fa fa-thumbs-up'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #5CB85C; color: green; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewExecEnquiry.aspx' style='color: #5CB85C;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF AWARDED ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    #region NO OF LOST ENQUIRY
                    DataSet dsGetLostEnq = new DataSet();
                    dsGetLostEnq = DBOperations.GetCntOfEnquiryStatus(4, "SelectOne", Convert.ToInt32(Session["VendorId"]));
                    if (dsGetLostEnq != null && dsGetLostEnq.Tables.Count > 0)
                    {
                        if (dsGetLostEnq.Tables[0].Rows.Count > 0)
                        {
                            strBuildEnquiry.Append(@"<div class='col-sm-3' style='width:20%; padding-right: 2px'>
							                        <!-- NO OF LOST ENQUIRIES -->
							                        <div class='box danger'>
								                        <div class='box-title'>
									                        <h4>" + dsGetLostEnq.Tables[0].Rows[0]["TotalQuoted"].ToString() + " Enquiry Lost</h4>" +
                                                            "<small class='block'>" + dsGetLostEnq.Tables[0].Rows[0]["TodaysQuoted"].ToString() + " Enquiry lost today</small>" +
                                                            "<i class='fa fa-thumbs-down'></i>" +
                                                        "</div>" +
                                                       "<div class='box-body text-center' style='border: 2px solid #E66454; color: #E66454; padding: 8px; font-size: 13px;text-align: inherit;'>" +
                                                             "View Details" +
                                                            "<span class='pull-right'>" +
                                                              "<a href='ViewLostEnquiry.aspx' style='color: #E66454;'><i class='fa fa-arrow-circle-right' style='font-size: 16px;'></i></a>" +
                                                            "</span>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<!-- /NO OF LOST ENQUIRIES -->" +
                                                    "</div>");
                        }
                    }
                    #endregion

                    dvPendingNumbers.InnerHtml = strBuildEnquiry.ToString();
                }
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetFreightSummary()
    {
        int lUser = 0, MonthId = 0;
        string MonthName = "";
        DataSet dsGetEnqDetail = new DataSet();
        StringBuilder strBuilder = new StringBuilder();
        if (Convert.ToString(Session["VendorId"]) != null && Convert.ToString(Session["VendorId"]) != "")
            lUser = Convert.ToInt32(Session["VendorId"]);

        strBuilder.Append(@"window.onload = function () {
                            loadScript(plugin_path + 'raphael-min.js', function () {
                                loadScript(plugin_path + 'chart.morris/morris.min.js', function () {
                                    if (jQuery('#graph-normal-bar').length > 0) {

                                    Morris.Bar({
                                    element: 'graph-normal-bar',
                                    data: [");
        for (int i = 1; i < 13; i++)
        {
            MonthId = i;
            #region Get month name
            if (i == 1)
                MonthName = "Jan";
            else if (i == 2)
                MonthName = "Feb";
            else if (i == 3)
                MonthName = "Mar";
            else if (i == 4)
                MonthName = "Apr";
            else if (i == 5)
                MonthName = "May";
            else if (i == 6)
                MonthName = "Jun";
            else if (i == 7)
                MonthName = "Jul";
            else if (i == 8)
                MonthName = "Aug";
            else if (i == 9)
                MonthName = "Sep";
            else if (i == 10)
                MonthName = "Oct";
            else if (i == 11)
                MonthName = "Nov";
            else if (i == 12)
                MonthName = "Dec";
            #endregion

            if (loggedInUser.glRoleId.ToString() != "")
            {
                if (loggedInUser.glRoleId.ToString() == "53")
                    dsGetEnqDetail = DBOperations.GetAlibabaFreightSummary(MonthId, "SelectAll", 0);
                else
                    dsGetEnqDetail = DBOperations.GetAlibabaFreightSummary(MonthId, "SelectOne", lUser);
            }

            if (dsGetEnqDetail.Tables.Count > 0 && dsGetEnqDetail.Tables[0].Rows.Count > 0)
            {
                string Enquiry = "null", Quoted = "null", Awarded = "null", Executed = "null", Lost = "null";
                if (dsGetEnqDetail.Tables[0].Rows[0]["Enquiry"].ToString() != "")
                    Enquiry = dsGetEnqDetail.Tables[0].Rows[0]["Enquiry"].ToString();
                if (dsGetEnqDetail.Tables[0].Rows[0]["Quoted"].ToString() != "")
                    Quoted = dsGetEnqDetail.Tables[0].Rows[0]["Quoted"].ToString();
                if (dsGetEnqDetail.Tables[0].Rows[0]["Awarded"].ToString() != "")
                    Awarded = dsGetEnqDetail.Tables[0].Rows[0]["Awarded"].ToString();
                if (dsGetEnqDetail.Tables[0].Rows[0]["Executed"].ToString() != "")
                    Executed = dsGetEnqDetail.Tables[0].Rows[0]["Executed"].ToString();
                if (dsGetEnqDetail.Tables[0].Rows[0]["Lost"].ToString() != "")
                    Lost = dsGetEnqDetail.Tables[0].Rows[0]["Lost"].ToString();

                if (i != 12)
                    strBuilder.Append(@"{ x: '" + MonthName + "', a: " + Enquiry + ", b: " + Quoted + ", " + "c: " + Awarded + ", y: " + Executed + "," + "z: " + Lost + " },");
                else
                    strBuilder.Append(@"{ x: '" + MonthName + "', a: " + Enquiry + ", b: " + Quoted + ", " + "c: " + Awarded + ", y: " + Executed + "," + "z: " + Lost + " }");
            }
            else
                strBuilder.Append(@"{ x: '" + MonthName + "', a: null, b: null, c: null, y: null, z: null },");
        }

        strBuilder.Append(@" ],
                        xkey: 'x',
                        ykeys: ['a', 'b', 'c', 'y', 'z'],
                        labels: ['Enquiry', 'Quoted', 'Awarded', 'Executed', 'Lost']
                    });
                }");

        if (loggedInUser.glRoleId.ToString() != "")
        {
            #region IF USER IS ADMIN
            if (loggedInUser.glRoleId.ToString() == "53")
            {
                tblIconDetail.Visible = true;
                dvCustomer.Visible = false;
                dvMode.Visible = false;
                List<AlibabaFreightTracking> lstGetUserSummaryDet = new List<AlibabaFreightTracking>();
                lstGetUserSummaryDet = DBOperations.GetFreightUserDetail("SelectAll", 0, Convert.ToInt32(ddMonth.SelectedValue), Convert.ToString(""), Convert.ToInt32(0));
                if (lstGetUserSummaryDet.Count > 0)
                {                    
                    strBuilder.Append(@"if (jQuery('#graph-stacked').length > 0){ 
			                                Morris.Bar({
			                                  element: 'graph-stacked',
			                                  axes: true,
			                                  grid: true,
			                                  data: [");
                    for (int i = 0; i < lstGetUserSummaryDet.Count; i++)
                    {
                        if (i == lstGetUserSummaryDet.Count - 1)
                        {
                            strBuilder.Append(@"{x: '" + lstGetUserSummaryDet[i].UserName.ToString() + "', a: " + lstGetUserSummaryDet[i].Enquiry.ToString() + ",");
                            strBuilder.Append(@"b: " + lstGetUserSummaryDet[i].Quoted.ToString() + ", c: " + lstGetUserSummaryDet[i].Awarded.ToString() + ", ");
                            strBuilder.Append(@"y: " + lstGetUserSummaryDet[i].Executed.ToString() + ", z: " + lstGetUserSummaryDet[i].Lost.ToString() + "}");
                        }
                        else
                        {
                            strBuilder.Append(@"{x: '" + lstGetUserSummaryDet[i].UserName.ToString() + "', a: " + lstGetUserSummaryDet[i].Enquiry.ToString() + ",");
                            strBuilder.Append(@"b: " + lstGetUserSummaryDet[i].Quoted.ToString() + ", c: " + lstGetUserSummaryDet[i].Awarded.ToString() + ", ");
                            strBuilder.Append(@"y: " + lstGetUserSummaryDet[i].Executed.ToString() + ", z: " + lstGetUserSummaryDet[i].Lost.ToString() + "},");
                        }

                    }
                    strBuilder.Append(@" ],
			                                  xkey: 'x',
                                              ykeys: ['a', 'b', 'c', 'y', 'z'],
                                              labels: ['Enquiry', 'Quoted', 'Awarded', 'Executed', 'Lost'],
			                                  stacked: true
			                                });
		                                }");
                }
            }
            #endregion
            else
            {
                tblIconDetail.Visible = false;
                List<AlibabaFreightTracking> lstGetUserSummaryDet = new List<AlibabaFreightTracking>();
                lstGetUserSummaryDet = DBOperations.GetFreightUserDetail("SelectOne", Convert.ToInt32(Session["VendorId"]), Convert.ToInt32(ddMonth.SelectedValue),
                                                                               txtCustomer.Text.Trim(), Convert.ToInt32(ddlMode.SelectedValue));
                if (lstGetUserSummaryDet.Count > 0)
                {
                    /////////////////////////////////////////////   Morris Doughnut   ///////////////////////////////////////////////////////////
                    strBuilder.Append(@"if (jQuery('#graph-donut').length > 0) { " +
                                                " Morris.Donut({ " +
                                                     " element: 'graph-donut', " +
                                                     " data: [ " +
                                                         " { value: " + lstGetUserSummaryDet[0].Enquiry.ToString() + ", label: 'Enquiry' }, " +
                                                         " { value: " + lstGetUserSummaryDet[0].Quoted.ToString() + ", label: 'Quoted' }, " +
                                                         " { value: " + lstGetUserSummaryDet[0].Awarded.ToString() + ", label: 'Awarded' }, " +
                                                         " { value: " + lstGetUserSummaryDet[0].Lost.ToString() + ", label: 'Lost' }, " +
                                                         " { value: " + lstGetUserSummaryDet[0].Executed.ToString() + ", label: 'Executed' } " +
                                                       " ], " +
                                                     " formatter: function (x) { return x } " +
                                                 " }); " +
                                             " } ");

                }               
            }
        }

        strBuilder.Append(@"});
                        });
                    }");
        string registerKey = strBuilder.ToString();
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisterClientScriptBlock", registerKey, true);
        lblFreightSummaryMsg.Visible = false;      
    }

    #region CHAT MESSAGE EVENTS

    protected void ReadMessages()
    {
        try
        {
            if (Convert.ToString(Session["Messageto"]) != null && Convert.ToString(Session["Messageto"]) != "0" &&
                Convert.ToString(Session["VendorId"]) != "" && Convert.ToString(Session["VendorId"]) != "0")
            {
                int MessageTo = 0, lUser = 0;
                MessageTo = Convert.ToInt32(Session["MessageTo"]);
                lUser = Convert.ToInt32(Session["VendorId"]);

                int ReadMsg = DBOperations.UpdateMsgAsRead(MessageTo, lUser);
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void GetInboxMsgs()
    {
        try
        {
            StringBuilder strBuilder = new StringBuilder();
            DataSet dsGetTotalMsgsRec = new DataSet();
            dsGetTotalMsgsRec = DBOperations.GetTotalMsgReceived(Convert.ToInt32(Session["VendorId"]));
            if (dsGetTotalMsgsRec != null && dsGetTotalMsgsRec.Tables.Count > 0)
            {
                if (dsGetTotalMsgsRec.Tables[0].Rows.Count > 0)
                {
                    string op = HttpUtility.UrlEncode(Encrypt("1"));
                    strBuilder.Append(@"<a href='Dashboard.aspx?op=" + op + "'><i class='fa fa-envelope'></i> Inbox" +
                                        "<span class='pull-right label label-default'>" + dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString() + "</span>" +
                                        "</a>");

                    li_Inbox.InnerHtml = strBuilder.ToString();

                    int Msgs = Convert.ToInt32(dsGetTotalMsgsRec.Tables[0].Rows[0]["NoofMsgs"].ToString());
                    if (Msgs > 0)
                    {
                        str_Message = "<script type='text/javascript' language='javascript'>" +
                                             "_toastr('You have " + Msgs + " messages received in inbox...!!!','top-full-width','info',false); </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                        return;
                    }
                }
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetRecentChats()
    {
        try
        {
            StringBuilder strRecentChat = new StringBuilder();
            List<FreightChat> lstChatDates = new List<FreightChat>();
            lstChatDates = DBOperations.GetEnquiryChatDates(Convert.ToInt32(Session["VendorId"]), Convert.ToInt32(Session["MessageTo"]));
            if (lstChatDates.Count > 0)
            {
                strRecentChat.Append(@"<ul class='media-list slimscroll height-250' data-slimscroll-visible='true'>");
                for (int d = 0; d < lstChatDates.Count; d++)
                {
                    strRecentChat.Append(@"<div style='text-align: center; font-size: 12px;padding-top:5px'>
                                             <span style='background-color: #d9edf7; padding: 5px; border-radius: 5px; border: 1px solid lightblue;'>" +
                                             lstChatDates[d].FormattedDate.ToString() + "</span></div>");

                    List<FreightChat> lstGetChatHistory = new List<FreightChat>();
                    lstGetChatHistory = DBOperations.GetDayWsChatDetails(Convert.ToInt32(Session["VendorId"]), Convert.ToInt32(Session["MessageTo"]), lstChatDates[d].Date);
                    if (lstGetChatHistory.Count > 0)
                    {
                        strRecentChat.Append(@"<div style='padding: 15px; background-color: floralwhite; border-radius: 25px; border-bottom-right-radius: inherit;
                                                                        border-top-left-radius: inherit; border: 1px brown; border-style: dotted; margin-bottom: 15px'>");

                        for (int i = 0; i < lstGetChatHistory.Count; i++)
                        {
                            if (lstGetChatHistory[i].PersonNaming.ToString() != "" && lstGetChatHistory[i].PersonNaming.ToString() == "1")
                            {
                                strRecentChat.Append(@"<li class='media'>
                                                                <div class='media-body'>
                                                                <div class='media'>
                                                                    <a class='pull-left' href='#'></a>
                                                                    <div class='media-body'>" + lstGetChatHistory[i].Message.ToString());
                                strRecentChat.Append(@"<br /><small class='text-muted'>" + lstGetChatHistory[i].MessageTime.ToString() + "</small><hr/>");
                                strRecentChat.Append(@"</div></div></div></li>");
                            }
                            else
                            {
                                strRecentChat.Append(@"<li class='media' style='text-align: right'>
                                                               <div class='media-body'>
                                                                <div class='media'>
                                                                    <a class='pull-left' href='#'></a>
                                                                    <div class='media-body'>" + lstGetChatHistory[i].Message.ToString());
                                strRecentChat.Append(@"<br /><small class='text-muted'>" + lstGetChatHistory[i].MessageTime.ToString() + "</small><hr/>");
                                strRecentChat.Append(@"</div></div></div></li>");
                            }
                        }

                        strRecentChat.Append(@"</div>");
                    }
                }
                strRecentChat.Append(@"</ul>");
                dvRecentChats.InnerHtml = strRecentChat.ToString();
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void GetOnlineUsers()
    {
        try
        {
            StringBuilder strOnlineUsers = new StringBuilder();
            List<FreightChat> lstGetOnlineUsers = new List<FreightChat>();
            lstGetOnlineUsers = DBOperations.GetOnlineUsers(Convert.ToInt32(Session["VendorId"]));
            if (lstGetOnlineUsers.Count > 0)
            {
                strOnlineUsers.Append(@"<ul class='media-list slimscroll height-300' data-slimscroll-visible='true'>");
                for (int i = 0; i < lstGetOnlineUsers.Count; i++)
                {
                    if (lstGetOnlineUsers[i].UserName.ToString() != "")
                    {
                        string op = HttpUtility.UrlEncode(Encrypt("0"));
                        string lid = HttpUtility.UrlEncode(Encrypt(lstGetOnlineUsers[i].lUser.ToString()));
                        strOnlineUsers.Append(@"<li class='media'><div class='media-body'><div class='media'>
                                                <a class='pull-left' href='Dashboard.aspx?op=" + op + "&id=" + lid + "' style='margin-right: 0px'>");

                        if (lstGetOnlineUsers[i].IsAvailable.ToString().ToLower() == "true")
                            strOnlineUsers.Append(@"<i style='font-size: 18px; padding-top: 12px; color: green;' class='fa fa-user'>");
                        else
                            strOnlineUsers.Append(@"<i style='font-size: 18px; padding-top: 12px; color: red;' class='fa fa-user'>");

                        strOnlineUsers.Append(@"</i></a><div class='media-body'>");
                        strOnlineUsers.Append(@"<h5 style='margin-bottom: 2px'>" + lstGetOnlineUsers[i].UserName.ToString());
                        if (lstGetOnlineUsers[i].TotalMsgs.ToString() != "0")
                            strOnlineUsers.Append(@"<span class='badge'>" + lstGetOnlineUsers[i].TotalMsgs.ToString() + "</span>");
                        strOnlineUsers.Append(@"</h5><small class='text-muted'>" + lstGetOnlineUsers[i].CurrentStatus.ToString() + "</small>");
                        strOnlineUsers.Append(@"</div></div></div></li>");
                    }
                }
                strOnlineUsers.Append(@"</ul>");
                dvOnlineUsers.InnerHtml = strOnlineUsers.ToString();
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void btnSendMsg_OnClick(object sender, EventArgs e)
    {
        try
        {
            int MessageTo = 0, lUser = 0, StatusId = 0;
            if (Convert.ToString(Session["MessageTo"]) != null && Convert.ToString(Session["MessageTo"]) != "")
                MessageTo = Convert.ToInt32(Session["MessageTo"]);
            lUser = Convert.ToInt32(Session["VendorId"]);
            StatusId = 0;
            if (MessageTo != 0)
            {
                if (txtMessage.Text.ToString().Trim() == "")
                {
                    ModalPopupExtender1.Show();
                    int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                    str_Message = "<script type='text/javascript' language='javascript'>" +
                                          "_toastr('Please type message!!','top-full-width','error',false); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                    return;
                }
                else
                {
                    int result = DBOperations.SendChatMessage(txtMessage.Text.Trim(), MessageTo, StatusId, lUser);
                    if (result == 0)
                    {
                        txtMessage.Text = "";
                        GetRecentChats();
                        GetOnlineUsers();
                        ModalPopupExtender1.Show();
                        int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                        str_Message = "<script type='text/javascript' language='javascript'>" +
                                           "_toastr('Message Sent..!!!','top-full-width','success',false); </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                        return;
                    }
                    else
                    {
                        str_Message = "<script type='text/javascript' language='javascript'>" +
                                           "_toastr('Message Sending Failed.','top-full-width','error',false); </script>";
                        ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                        return;
                    }
                }
            }
            else
            {
                ModalPopupExtender1.Show();
                int UserActive = DBOperations.SetUserAvailability("Login", Convert.ToInt32(Session["VendorId"]));   //SET USER's ACTIVE MODE
                str_Message = "<script type='text/javascript' language='javascript'>" +
                                      "_toastr('Please select the user you want to chat with..!!!','top-full-width','error',false); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Button), "Message", str_Message, false);
                return;
            }
        }
        catch (Exception en)
        {
            throw (en);
        }
    }

    protected void btnCancelPopup_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
        if (Convert.ToString(Session["VendorId"]) != null)
        {
            ddMonth.SelectedValue = DateTime.Now.Month.ToString();
            GetFreightSummary();
            if (loggedInUser.glEmpName.ToString() != "")
                lblUserName.Text = loggedInUser.glEmpName.ToString();
            GetEnquiryDetails();
            GetInboxMsgs();
            Session["MessageTo"] = "";
            /***********  RESET USER's ACTIVE MODE  *********/
            int UserInActive = DBOperations.SetUserAvailability("Logout", Convert.ToInt32(Session["VendorId"]));
            /***********  RESET USER's ACTIVE MODE  *********/
        }
        else
            Response.Redirect("FRlogin.aspx");
    }

    protected void msgAll(int msgType, string msg)
    {
        StringBuilder strDeviceList = new StringBuilder();

        switch (msgType)
        {
            case 1: // warning msg

                strDeviceList.Append(@" <div class='alert alert-mini alert-info margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                  <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Warning!</strong> " + msg + "</div>");//warning                   
                break;
            case 2: // error msg
                strDeviceList.Append(@" <div class='alert alert-mini alert-danger margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                    <span aria-hidden='true'>×</span><span class='sr-only'>Close</span> </button> <strong>Error!</strong> " + msg + "</div>");//error
                break;

            case 3: // success msg
                strDeviceList.Append(@" <div class='alert alert-mini alert-success margin-bottom-30'><button type='button' class='close' data-dismiss='alert'>
                                     <span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button> <strong>Success!</strong> " + msg + "</div>");//Successes
                break;
        }
        dvMsgAll.InnerHtml = strDeviceList.ToString();
    }

    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    #endregion
}