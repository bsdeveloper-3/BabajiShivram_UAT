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

public partial class ExportCHA_FillCustomProcess : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Jobid"] == null)
            {
                Response.Redirect("FillCustomProcess.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Custom Process";
                CustomProcessDetail(Convert.ToInt32(Session["JobId"].ToString()));
                Page.Validate();
            }
        }
    }

    private void CustomProcessDetail(int JobId)
    {
        DataSet dsJobDetail = EXOperations.GetJobDetailForCustomProcess(JobId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            lblJobRefNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            lblCustRefNo.Text = dsJobDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsigneeName.Text = dsJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
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
            if (dsJobDetail.Tables[0].Rows[0]["DeliveryStatus"] != DBNull.Value)
            {
                if (dsJobDetail.Tables[0].Rows[0]["DeliveryStatus"].ToString().ToLower() == "true")
                    hdnDeliveryStatus.Value = "1";
                else
                    hdnDeliveryStatus.Value = "0";
            }
            if (dsJobDetail.Tables[0].Rows[0]["MarkAppraising"] != DBNull.Value)
            {
                if (dsJobDetail.Tables[0].Rows[0]["MarkAppraising"].ToString() == "1")
                {
                    lblMarkedPassingDate.Text = "To be Marked On Date";
                    MEditValMarkPassingDate.EmptyValueMessage = "Enter Mark Date.";
                }
                else
                {
                    lblMarkedPassingDate.Text = "Passing Date";
                    MEditValMarkPassingDate.EmptyValueMessage = "Enter Passing Date.";
                }
            }

            if (dsJobDetail.Tables[0].Rows[0]["CartingDate"] != DBNull.Value)
                txtCartDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["CartingDate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["MarkPassingDate"] != DBNull.Value)
                txtMarkPassingDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["MarkPassingDate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["RegisterationDate"] != DBNull.Value)
                txtRegistrationDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["RegisterationDate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["ExamineDate"] != DBNull.Value)
                txtExamineDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["ExamineDate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["ExamineReportDate"] != DBNull.Value)
                txtExamineReportDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["ExamineReportDate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["LEODate"] != DBNull.Value)
                txtLEODate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["LEODate"]).ToString("dd/MM/yyyy");

            if (hdnDeliveryStatus.Value != "")
            {
                if (hdnDeliveryStatus.Value == "0" && dsJobDetail.Tables[0].Rows[0]["TransportBy"].ToString() == "1") // delivery details not updated
                {
                    btnSubmit.Visible = false;
                    txtCartDate.Enabled = false;
                    txtMarkPassingDate.Enabled = false;
                    lblError.Text = "Please Update Delivery Details First..!!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    btnSubmit.Visible = true;
                    txtCartDate.Enabled = true;
                    txtMarkPassingDate.Enabled = true;
                    lblError.Text = "";
                }
            }

            if (dsJobDetail.Tables[0].Rows[0]["ExportTypeId"] != DBNull.Value && dsJobDetail.Tables[0].Rows[0]["ExportTypeId"].ToString() != "")
                hdnExportType.Value = dsJobDetail.Tables[0].Rows[0]["ExportTypeId"].ToString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime dtMarkPassingDate = DateTime.MinValue, dtRegistrationDate = DateTime.MinValue, dtExamineDate = DateTime.MinValue,
                 dtExamineReportdate = DateTime.MinValue, dtLEODate = DateTime.MinValue, dtCartDate = DateTime.MinValue;

        int JobId = Convert.ToInt32(Session["JobId"]);
        if (txtCartDate.Text.Trim() != "")
            dtCartDate = Commonfunctions.CDateTime(txtCartDate.Text.Trim());
        if (txtMarkPassingDate.Text.Trim() != "")
            dtMarkPassingDate = Commonfunctions.CDateTime(txtMarkPassingDate.Text.Trim());
        if (txtRegistrationDate.Text.Trim() != "")
            dtRegistrationDate = Commonfunctions.CDateTime(txtRegistrationDate.Text.Trim());
        if (txtExamineDate.Text.Trim() != "")
            dtExamineDate = Commonfunctions.CDateTime(txtExamineDate.Text.Trim());
        if (txtExamineReportDate.Text.Trim() != "")
            dtExamineReportdate = Commonfunctions.CDateTime(txtExamineReportDate.Text.Trim());
        if (txtLEODate.Text.Trim() != "")
            dtLEODate = Commonfunctions.CDateTime(txtLEODate.Text.Trim());

        int Result = EXOperations.EX_AddFilingDetail(JobId, lblSBNo.Text, Convert.ToDateTime(lblSBDate.Text), 0, dtMarkPassingDate, dtRegistrationDate, dtExamineDate, dtExamineReportdate,
                                                      dtLEODate, dtCartDate, txtRemark.Text.Trim(), LoggedInUser.glUserId);

        if (Result == 0)
        {
            if (hdnExportType.Value == "3")
            {
                Response.Redirect("../Success.aspx?ExpCustomProcessForm=4004");
            }
            else
            {
                Response.Redirect("../Success.aspx?ExpCustomProcess=4004");
            }
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after some time!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("PendingCustomProcess.aspx");
    }

    protected void txtCartDate_TextChanged(object sender, EventArgs e)
    {
        if (txtCartDate.Text.Trim() == "")
        {
            MEditValMarkPassingDate.IsValidEmpty = true;
        }
        else
        {
            MEditValMarkPassingDate.IsValidEmpty = false;
            MEditValMarkPassingDate.EmptyValueBlurredText = "";
            MEditValMarkPassingDate.InvalidValueMessage = "";
            MEditValMarkPassingDate.IsValidEmpty = true;

        }
    }
}