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

public partial class PCDDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["JobId"] == null)
            {
                Response.Redirect("PendingPCD.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "PCA Document";

              //  GetJobDetail(Convert.ToInt32(Session["JobId"]));
                                
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

    //private void GetJobDetail(int JobId)
    //{
    //    DataView dvDetail = DBOperations.GetJobDetailForPCDToCustomer(JobId);

    //    if (dvDetail.Table.Rows.Count > 0)
    //    {
    //        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
    //        lblTitle.Text += " - " + dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        lblJobRefNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();

    //        hdnPreAlertId.Value = dvDetail.Table.Rows[0]["AlertId"].ToString();
    //        hdnCustomerId.Value = dvDetail.Table.Rows[0]["CustomerId"].ToString();
    //        lblCustRefNo.Text = dvDetail.Table.Rows[0]["CustRefNo"].ToString();
    //        lblCustName.Text = dvDetail.Table.Rows[0]["Customer"].ToString();
    //        lblPCDToCustomer.Text = dvDetail.Table.Rows[0]["PCDRequired"].ToString();
    //    }
    //}

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    if (Page.IsValid)
    //    {
    //        int result = setvalues();
    //        if (result == 0)
    //        {
    //            Response.Redirect("Success.aspx?PCDToCustomer=1324");
    //        }
    //        else
    //        {
    //            //   clear_val();
    //            lblError.Visible = true;
    //            lblError.CssClass = "errorMsg";
    //            // lblError.Text = "Please select the file to upload!";

    //        }//END if
    //    }
    //}

    //protected int setvalues()
    //{
    //    int JobId,CustomerId;

    //    string strDispatchLocation="";

    //    DateTime HandoverDate = DateTime.MinValue;

    //    int result = 1;

    //    string filename = "";

    //    JobId = Convert.ToInt32(Session["JobId"]);
        
    //    CustomerId = Convert.ToInt32(hdnCustomerId.Value);

    //    strDispatchLocation = txtDispatchLocation.Text.Trim();

    //    if (txtHandoverDate.Text.Trim() != "")
    //    {
    //        HandoverDate = Commonfunctions.CDateTime(txtHandoverDate.Text.Trim());
    //    }
        
    //    foreach (RepeaterItem itm in rptPCDDocument.Items)
    //    {
    //        CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
    //        FileUpload FU = (FileUpload)(itm.FindControl("fuDocument"));

    //        if (chk.Checked)
    //        {
    //            if (FU.HasFile)
    //            {
                
    //            }
    //            else
    //            {
    //                result = 0; // Page Not Valid
    //                lblError.Visible = true;
    //                lblError.CssClass = "errorMsg";
    //                lblError.Text = "Please select the file to upload!";
    //                chk.Checked = false;
    //            }
    //        }//END_IF
    //    }//END_ForEach
    //    //

    //    if (result > 0)
    //    {
    //        result = 0;// DBOperations.AddPCDToCustomer(JobId, strDispatchLocation, HandoverDate, LoggedInUser.glUserId);

    //        int doctype;

    //        if (result == 0)
    //        {
    //            foreach (RepeaterItem itm in rptPCDDocument.Items)
    //            {
    //                CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
    //                FileUpload FU = (FileUpload)(itm.FindControl("fuDocument"));
    //                HiddenField hf = (HiddenField)itm.FindControl("hdnDocId");

    //                doctype = Convert.ToInt32(hf.Value);

    //                if (chk.Checked)
    //                {
    //                    if (FU.HasFile)
    //                    {
    //                        filename = UploadFiles(FU, hf, CustomerId.ToString(), JobId.ToString());
    //                       // DBOperations.AddPCDdocPath(filename, doctype, JobId, true, LoggedInUser.glUserId);
    //                        DBOperations.AddPCDdocPath(filename, doctype, JobId, LoggedInUser.glUserId);
    //                    }
    //                    else
    //                    {
    //                        //lblError.Visible = true;
    //                        //lblError.CssClass = "errorMsg";
    //                        //lblError.Text = "Please select the file to upload!";
    //                        //chk.Checked = false;
    //                        //break;
    //                    }
    //                }//END_IF
    //            }//END_FOrEach
    //        }//END_IF
    //    }

    //    return result;
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingPCD.aspx");
    }

    #region Document Download
    
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
