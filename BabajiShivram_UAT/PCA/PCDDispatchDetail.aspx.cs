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
public partial class PCDDispatchDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["JobId"] == null)
            {
                Response.Redirect("PendingPCDDispatch.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "PCA Dispatch Detail";

             //   GetJobDetail(Convert.ToInt32(Session["JobId"]), DispatchType);

            }
        }
    }

    //private void GetJobDetail(int JobId, Boolean DispatchType)
    //{
    //    DataView dvDetail = DBOperations.GetJobDetailForPCDDispatch(JobId, DispatchType);

    //    if (dvDetail.Table.Rows.Count > 0)
    //    {
    //        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
    //        lblTitle.Text += " - " + dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        hdnPreAlertId.Value = dvDetail.Table.Rows[0]["AlertId"].ToString();
    //        hdnCustomerId.Value = dvDetail.Table.Rows[0]["CustomerId"].ToString();
    //        lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
    //        txtDispatchLocation.Text = dvDetail.Table.Rows[0]["CustomerDispatchLocation"].ToString();
    //    }
    //}

    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingPCDDispatch.aspx");
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

    #region Documnet Download
    
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
        
    #endregion
}
