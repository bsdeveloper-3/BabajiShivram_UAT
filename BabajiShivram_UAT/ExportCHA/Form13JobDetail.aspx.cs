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
using System.Text;


public partial class ExportCHA_Form13JobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Jobid"] == null)
            {
                Response.Redirect("PendingForm13.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Form 13";
                GetJobdetail(Convert.ToInt32(Session["JobId"].ToString()));
                Page.Validate();
            }
        }
    }

    private void GetJobdetail(int JobId)
    {
        DataSet dsJobDetail = EXOperations.GetShippingDetailByJobId(JobId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            lblJobRefNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            lblCustRefNo.Text = dsJobDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsigneeName.Text = dsJobDetail.Tables[0].Rows[0]["ConsigneeName"].ToString();
            lblCountryConsgn.Text = dsJobDetail.Tables[0].Rows[0]["ConsignmentCountry"].ToString();
            lblDestCountry.Text = dsJobDetail.Tables[0].Rows[0]["DestinationCountry"].ToString();
            lblForwarderName.Text = dsJobDetail.Tables[0].Rows[0]["ForwarderName"].ToString();
            lblGrossWT.Text = dsJobDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            lblMode.Text = dsJobDetail.Tables[0].Rows[0]["TransMode"].ToString();
            lblNetWT.Text = dsJobDetail.Tables[0].Rows[0]["NetWT"].ToString();
            lblNoOfPkg.Text = dsJobDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            lblPOD.Text = dsJobDetail.Tables[0].Rows[0]["PortOfDischarge"].ToString();
            lblPOL.Text = dsJobDetail.Tables[0].Rows[0]["PortOfLoading"].ToString();
            lblSBDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["SBDate"]).ToString("dd/MM/yyyy");
            lblSBNo.Text = dsJobDetail.Tables[0].Rows[0]["SBNo"].ToString();
            lblShipper.Text = dsJobDetail.Tables[0].Rows[0]["Shipper"].ToString();
            if (dsJobDetail.Tables[0].Rows[0]["LEODate"] != DBNull.Value)
                lblLEODate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["LEODate"]).ToString("dd/MM/yyyy");
        }
    }

}