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
public partial class PCDScrutiny : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["JobId"] == null)
            {
                Response.Redirect("PendingPCDScrutiny.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "PCA Scrutiny";

                GetJobDetail(Convert.ToInt32(Session["JobId"]));

            }
        }
    }

    private void GetJobDetail(int JobId)
    {
        DataView dvDetail = DBOperations.GetJobDetailForPCDToScrutiny(JobId);

        if (dvDetail.Table.Rows.Count > 0)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text += " - " + dvDetail.Table.Rows[0]["JobRefNo"].ToString();

            lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();

            lblCustRefNo.Text = dvDetail.Table.Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            lblPCDToCustomer.Text = dvDetail.Table.Rows[0]["PCDRequired"].ToString();
            lblAdvicedate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestedDate"]).ToString("dd/MM/yyyy");
            lblPersonName.Text = dvDetail.Table.Rows[0]["RequestedBy"].ToString();
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int JobId = Convert.ToInt32(Session["JobId"]);
            bool bApprove = true;

           int result = DBOperations.ApproveRejectScrutiny(JobId, bApprove, txtRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                Session["Success"] = "Billing Scrutiny Completed! Job Moved To Billing Department!.";

                Response.Redirect("../Success.aspx");
            }
            else if(result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Billing Scrutiny Already Completed!";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int JobId = Convert.ToInt32(Session["JobId"]);
            bool bApprove = false;

           int result = DBOperations.ApproveRejectScrutiny(JobId, bApprove, txtRemark.Text.Trim(), LoggedInUser.glUserId);

            if (result == 0)
            {
                Session["Success"] = "Billing Scrutiny Rejected! Job Moved Back To Billing Advice!.";

                Response.Redirect("../Success.aspx");
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Billing Scrutiny Already Completed!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    
    protected void gvPCDDocument_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string strDocPath = (string)DataBinder.Eval(e.Row.DataItem, "DocPath").ToString();

            if (strDocPath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");
                lnkDownload.Visible = false;
            }
        }
    }

    protected void gvPCDDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingPCDScrutiny.aspx");
    }
        
}
