using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class FreightExport_ExpPendingVGM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["EnqId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending VGM/Form13";
        }
        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = gvFreight.Columns;
        DataFilter1.FilterSessionID = "ExpPendingVGM.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void gvFreight_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            string strEnqId = args[0];
            string strJobMode = args[1];
            Session["EnqId"] = strEnqId;
            Session["JobMode"] = strJobMode;

            Response.Redirect("ExpVGMForm13.aspx");
        }
    }

    protected void gvFreight_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "ExpPendingVGM.aspx";
            DataFilter1.FilterDataSource();
            gvFreight.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
    #endregion
}