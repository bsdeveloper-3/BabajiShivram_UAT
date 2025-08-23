using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TaxProEWB.API;
public partial class EWayBill_EwaySuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "EWay Bill Message";
        string strLogMessage = "";
        try
        {
            if (Session["EwayMessage"] != null)
            {
                lblMessage.Text = Session["EwayMessage"].ToString();
                lblMessage.CssClass = "success";

                strLogMessage = "EwayMessage="+Session["EwayMessage"].ToString();
                Session["EwayMessage"] = null;
            }
            if (Session["EwaySuccessList"] != null)
            {
                strLogMessage = "EwaySuccessList=" + Session["EwaySuccessList"].ToString();
                List<RespGenEwbPl> EwaySuccessList = (List<RespGenEwbPl>)(Session["EwaySuccessList"]);
                
                if (EwaySuccessList.Count > 0)
                {
                    gvEwaySuccess.DataSource = EwaySuccessList;
                    gvEwaySuccess.DataBind();
                }
                else
                {
                    lblMessage.Text = "EWay Bill List Empty! Pleas Check EWay Tracking For Bill Number";
                    lblMessage.CssClass = "errorMsg";
                }

                Session["EwaySuccessList"] = null;
            }
            if (Session["EwayErrorList"] != null)
            {
                strLogMessage = "EwayErrorList=" + Session["EwayErrorList"].ToString();

                List<String> EwaySuccessList = (List<String>)(Session["EwayErrorList"]);
                gvEwayError.DataSource = EwaySuccessList;
                gvEwayError.DataBind();

                Session["EwayErrorList"] = null;
            }
            if (Session["EwayMessageCencel"] != null)
            {
                strLogMessage = "EwayMessageCencel=" + Session["EwayMessageCencel"].ToString();

                List<RespCancelEwbPl> respCancelList = (List<RespCancelEwbPl>)(Session["EwayMessageCencel"]);

                gvEwaySuccess.DataSource = respCancelList;

                gvEwaySuccess.DataBind();

                Session["EwayMessageCencel"] = null;

            }
        }

        catch(Exception ex)
        {
            DBOperations.AddErrorLog(0,"Eway Success","EWay Success",strLogMessage,ex.Message,0);
        }        
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transport/TransDashboard.aspx");
    }
}