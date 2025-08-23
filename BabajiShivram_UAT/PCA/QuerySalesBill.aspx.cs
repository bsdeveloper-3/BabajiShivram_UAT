using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Reporting.WebForms;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Services;

public partial class PCA_QuerySalesBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["JobId"] = null;
            ViewState["lblDEBITAMT"] = "0";

            //Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            //lblTitle.Text = "Pending Billing Advice";

            if (gvotherDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Found For Query Sales Bill!";
                lblMessage.CssClass = "errorMsg"; ;
                pnlFilter.Visible = false;
            }
        }
        DataFilter1.DataSource = PCDSqlDataSource;
        DataFilter1.DataColumns = gvotherDetail.Columns;
        DataFilter1.FilterSessionID = "AView_Otherinstruction.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
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
            DataFilter1.FilterSessionID = "AView_Otherinstruction.aspx";
            DataFilter1.FilterDataSource();
            gvotherDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void gvotherDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "bill query resolve")
        {
            int invmstid = Convert.ToInt32(e.CommandArgument);
            int count1 = Set_BillSalesBillStatus(invmstid);
            string message = "Status Updated Successfully.";
            string script = "window.onload = function(){ alert('";
            script += message;
            script += "')};";
            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
        }
    }
    public static int Set_BillSalesBillStatus(int invmstid)
    {
        SqlCommand command = new SqlCommand();
        int SPresult;
        command.Parameters.Add("@invmstid", SqlDbType.VarChar).Value = invmstid;
        SPresult = CDatabase.GetSPCount("Set_BillSalesBillStatus", command);
        return Convert.ToInt32(SPresult);
    }
}