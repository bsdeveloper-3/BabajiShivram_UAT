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
public partial class PCA_PCABillingAdvice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["JobId"] == null)
            {
                Response.Redirect("PendingBillingAdvice.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "PCA Billing Advice";
    
            }
        }
    }
        
    //private void GetJobDetail(int JobId)
    //{
    //    DataView dvDetail = DBOperations.GetJobDetailForPCABillingAdvice(JobId);

    //    if (dvDetail.Table.Rows.Count > 0)
    //    {
    //        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
    //        lblTitle.Text += " - " + dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        hdnPreAlertId.Value = dvDetail.Table.Rows[0]["AlertId"].ToString();
    //        hdnCustomerId.Value = dvDetail.Table.Rows[0]["CustomerId"].ToString();
    //        lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
            
    //    }
    //}

    
        
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingBillingAdvice.aspx");
    }

#region Billing Expense

    protected void gvjobexpenseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Int64 lblDEBITAMT = Convert.ToInt64(e.Row.Cells[6].Text);
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + Convert.ToInt64(lblDEBITAMT);

            Int64 lblCREDITAMT = Convert.ToInt64(e.Row.Cells[7].Text);
            ViewState["lblCREDITAMT"] = Convert.ToInt64(ViewState["lblCREDITAMT"]) + Convert.ToInt64(lblCREDITAMT);


            Int64 lblAMOUNT = Convert.ToInt64(e.Row.Cells[8].Text);
            ViewState["lblAMOUNT"] = Convert.ToInt64(ViewState["lblAMOUNT"]) + Convert.ToInt64(lblAMOUNT);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = "<b>Total</b>";
            e.Row.Cells[0].ColumnSpan = 1;
            e.Row.Cells[6].Text = ViewState["lblDEBITAMT"].ToString();
            e.Row.Cells[7].Text = ViewState["lblCREDITAMT"].ToString();
            e.Row.Cells[8].Text = ViewState["lblAMOUNT"].ToString();
        }
    }


    #endregion Biiling expense

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
