using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_RejectedVendor_Status : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Rejected Vendor Tracking";
            BindVendorDetails();
        }
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        string lId = lnk.CommandArgument.ToString();
        Session["lId"] = lId;

          Response.Redirect("ApproveRejected_Vendor.aspx");

    }
    private void BindVendorDetails()
    {
        DataSourceRejectedVendor.DataBind();
        gvRejectedVendor.DataBind();
    }
    protected void gvVendorDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int ColCount = gvRejectedVendor.Columns.Count;

            GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell headerCell = new TableCell
            {
                //BackColor = System.Drawing.Color.LightGray,
                //Text = "Vendor Approval Status",
                //HorizontalAlign = HorizontalAlign.Center,
                //ColumnSpan = ColCount
            };
            headerRow.Cells.Add(headerCell);

            gvRejectedVendor.Controls[0].Controls.AddAt(0, headerRow);
        }
    }
}