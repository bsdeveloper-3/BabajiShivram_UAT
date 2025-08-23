using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_ReportMaintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Employee Report";
    }

    protected void gvSummaryEmp_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvSummaryEmp_RowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvSummaryEmp.EditIndex = e.NewEditIndex;
    }

    protected void gvSummaryEmp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Description = DataBinder.Eval(e.Row.DataItem, "ShortDesc").ToString();

            if (Description.Length < 50)
            {
                LinkButton lnkMore = (LinkButton)e.Row.FindControl("lnkMore");

                if (lnkMore != null)
                    lnkMore.Visible = false;
            }
        }

    }
}