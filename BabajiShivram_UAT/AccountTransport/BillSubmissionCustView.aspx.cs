using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class AccountTransport_BillSubmissionCustView : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LoggedInUser.glUserId == 0 || Session["TransReqId"] == null)
        {
            Response.Redirect("Bill2.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Customer Draft Bill Detail";
            if (Session["TransReqId"] != null)
            {
                TruckRequestDetail(Convert.ToInt32(Session["TransReqId"]));
                JobDetailMS(Convert.ToInt32(Session["JobId"]));
            }
            else
            {
                Session["TransReqId"] = null;
                Session["JobId"] = null;
                Session["TransporterId"] = null;
                Response.Redirect("Bill2.aspx");
            }
        }
    }
    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequest(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            hdnJobId.Value = dvDetail.Table.Rows[0]["JobId"].ToString();
            Session["JobId"] = dvDetail.Table.Rows[0]["JobId"].ToString();
        }
    }
    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = DBOperations.GetJobDetailForTransport(JobId);
        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Session["TransReqId"] = null;
            Session["JobId"] = null;
            Session["TransporterId"] = null;

            Response.Redirect("Bill2.aspx");

        }

        lblTitle.Text = "Customer Bill Detail - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();

        lblJobNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
        lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
        lblDestination.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryDestination"].ToString();

        if (dsJobDetail.Tables[0].Rows[0]["IsTransBillToBabaji"] != DBNull.Value)
        {
            if (Convert.ToBoolean(dsJobDetail.Tables[0].Rows[0]["IsTransBillToBabaji"]))
            {
                lblTransportBillTo.Text = "Babaji";
            }
            else
            {
                lblTransportBillTo.Text = "Customer";
            }
        }
    }
}