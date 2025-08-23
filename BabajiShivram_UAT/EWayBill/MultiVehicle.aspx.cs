using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EWayBill_MultiVehicle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Multi Vehicle E-Way Bill";

        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);
            Session["EwayMultiVeh"] = null;
            Session["userGSTIN"]    = null;
        }
    }

    protected void btnGetewayBill_Click(object sender, EventArgs e)
    {
        long EwbNo = Convert.ToInt64(txtEwayBillNo.Text.Trim());

        Session["EwayMultiVeh"] = EwbNo;
        Session["userGSTIN"] = ddTrasnporter.SelectedValue;
        Response.Redirect("ConfirmMultiVehicle.aspx");

    }
}