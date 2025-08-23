using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EWayBill_ExtendValidityEWayBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Extend Validity - EWay Bill";

        if (!IsPostBack)
        {
            DBOperations.FillEWAYBillGSTIN2(ddTrasnporter, 0);

            Session["EwayExtend"] = null;
            Session["userGSTIN"] = null;
        }
    }

    protected void btnGetewayBill_Click(object sender, EventArgs e)
    {
        long EwbNo = Convert.ToInt64(txtEwayBillNo.Text.Trim());

        Session["EwayExtend"] = EwbNo;
        Session["userGSTIN"] = ddTrasnporter.SelectedValue;

        Response.Redirect("ConfirmExtendEwayBill.aspx");

    }
}