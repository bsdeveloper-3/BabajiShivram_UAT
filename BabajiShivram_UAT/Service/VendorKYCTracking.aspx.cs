using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_VendorKYCTracking : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vendor KYC Tracking";
            BindVendorDetails();
        }
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        string lId = lnk.CommandArgument.ToString();
        Session["ViewvendorKYCID"] = lId;
        //  Response.Redirect("ViewApproveVendor_details.aspx" + lId);
        //DataSourceApprovalStatus.SelectParameters[0].DefaultValue = lId;
        //DataSourceApprovalStatus.DataBind();

        //   gvVendordetails.DataBind();
        Response.Redirect("VendorKYCDetail.aspx");

    }
    private void BindVendorDetails()
    {
        DataSourceApprovalStatus.DataBind();
        gvVendordetails.DataBind();
    }
    protected void gvVendorDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int ColCount = gvVendordetails.Columns.Count;

            GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell headerCell = new TableCell
            {
                //BackColor = System.Drawing.Color.LightGray,
                //Text = "Vendor Approval Status",
                //HorizontalAlign = HorizontalAlign.Center,
                //ColumnSpan = ColCount
            };
            headerRow.Cells.Add(headerCell);

            gvVendordetails.Controls[0].Controls.AddAt(0, headerRow);
        }
    }

}