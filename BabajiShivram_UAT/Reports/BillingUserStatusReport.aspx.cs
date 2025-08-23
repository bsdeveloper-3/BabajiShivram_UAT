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
using System.IO;


public partial class Reports_BillingUserStatusReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        LoginClass LoggedInUser = new LoginClass();

        gvUserStatusReport.Visible = true;
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["JobId"] = null;
            Session["ReportId"] = null;
            Session["ReportName"] = null;
            Session["ReportUserId"] = null;
            Session["Lid"] = null;
            Session["MISPortId"] = null;
            Session["PendingPortId"] = null;
            Session["PendingCustId"] = null;

            UserExtender.ContextKey = LoggedInUser.glUserId.ToString();
        }
    }

    protected void txtUser_TextChanged(object sender, EventArgs e)
    {
        if (txtUser.Text.Trim() == "")
            hdnUserId.Value = "0";
    }
    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        gvUserStatusReport.Visible = true;
        if (drp1.SelectedValue == "0")
        {
            string strFromDate = "";
            if (txtDateFrom.Text.Trim() != "")
            {
                strFromDate = Commonfunctions.CDateTime(txtDateFrom.Text.Trim()).ToShortDateString();
            }
            DsUserFileDetails.SelectParameters["Day"].DefaultValue = strFromDate;

        }
        else if (drp1.SelectedValue == "2")
        {

            DsUserFileDetails.SelectParameters["Month"].DefaultValue = drpMonth.SelectedValue;
            DsUserFileDetails.SelectParameters["Year"].DefaultValue = drpYear.SelectedValue;
        }
        else if (drp1.SelectedValue == "3")
        {
            RFVFomDate.Enabled = false;
            RFVFomDate.IsValid = false;
            ComValFromDate.Enabled = false;
            DsUserFileDetails.SelectParameters["Day"].DefaultValue = Convert.ToString(DateTime.Now);
            DsUserFileDetails.SelectParameters["Year"].DefaultValue = drpYear.SelectedValue;
        }
       gvUserStatusReport.DataSource = DsUserFileDetails;        
        gvUserStatusReport.DataBind();

        if (gvUserStatusReport.Rows.Count == 0)
        {
            lblMessage.Text = "No Records Found";
            lblMessage.CssClass = "errorMsg";
        }
    }

    protected void Cancel_OnClick(Object sender, EventArgs e)
    {
        gvUserStatusReport.Visible = false;
    }
    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "UserReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /*Verifies that the control is rendered */
    //}



    private void ExportFunction(string header, string contentType)
    {

        gvUserStatusReport.DataBind();

        if (gvUserStatusReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvUserStatusReport.AllowPaging = false;
            gvUserStatusReport.AllowSorting = false;

            gvUserStatusReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
    protected void drp1_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvUserStatusReport.Visible = false;
        if (drp1.SelectedValue == "0")
        {
            td_Day.Visible = true;
            td_Week.Visible = false;
            td_Month.Visible = false;
            td_Year.Visible = false;
        }
        else if (drp1.SelectedValue == "1")
        {
            td_Day.Visible = false;
            td_Week.Visible = true;
            td_Month.Visible = false;
            td_Year.Visible = false;
        }
        else if (drp1.SelectedValue == "2")
        {
            txtDateFrom.Text = "";
            td_Day.Visible = false;
            td_Week.Visible = false;
            td_Month.Visible = true;
            td_Year.Visible = true;
        }
        else if (drp1.SelectedValue == "3")
        {
            txtDateFrom.Text = "";
            RFVToDate1.Enabled = false;
            RFVFomDate.Enabled = false;
            RFVFomDate1.Enabled = false;           
            td_Day.Visible = false;
            td_Week.Visible = false;
            td_Month.Visible = false;
            td_Year.Visible = true;
        }

    }
}