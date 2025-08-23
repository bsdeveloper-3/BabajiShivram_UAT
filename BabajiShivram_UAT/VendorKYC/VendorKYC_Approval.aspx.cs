using AjaxControlToolkit;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

public partial class Service_VendorKYC_Approval : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    DataSet dsReport = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
     //   ScriptManager1.RegisterPostBackControl(GridViewDoc);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "KYC Pending Approval ";

            BindVendorDetails();
        }
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        string KYCId = lnk.CommandArgument.ToString();
        Session["KYCAPRID"]= KYCId;
       
        Response.Redirect("Vendor_ApproveReject.aspx");
    
    }
    private void BindVendorDetails()
    {
        DataSourcePendingApproval.DataBind();
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
                //Text = "Vendor Approval Request",
                //HorizontalAlign = HorizontalAlign.Center,
                //ColumnSpan = ColCount
            };
            headerRow.Cells.Add(headerCell);

            gvVendordetails.Controls[0].Controls.AddAt(0, headerRow);
        }
    }

    //Approval and Rejection Operation
    //protected void gvApprovalDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    lblvendorMessage.Text = "";
    //    ModalPopupExtender.Show();

    //    string commandName = e.CommandName.ToLower();
    //    int VendorKycId = Convert.ToInt32(e.CommandArgument);
    //    string Remark = "";
    //    int StatusId = 0;

    //    foreach (GridViewRow gvRow in gvApprovalDetails.Rows)
    //    {
    //        if ((int)gvApprovalDetails.DataKeys[gvRow.DataItemIndex].Value == VendorKycId)
    //        {
    //            TextBox txtApprovalRemark = (TextBox)gvRow.FindControl("txtApprovalRemark");
    //            if (txtApprovalRemark != null)
    //            {
    //                Remark = txtApprovalRemark.Text.Trim();
    //            }

    //            break;
    //        }
    //    }
    //    if (commandName == "approve")
    //    {
    //        if (string.IsNullOrEmpty(Remark))
    //        {
    //            lblvendorMessage.Text = "Please Enter an Approval Remark!";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Red;
    //            lblvendorMessage.CssClass = "errorMsg";
    //            return;
    //        }

    //        int approvalResult = DBOperations.UpdVendorApproveStatus(VendorKycId, 211, Remark, loggedInUser.glUserId);  // Approval StatusId = 211 
    //        if (approvalResult == 0)
    //        {
    //            lblvendorMessage.Text = "Vendor Details approved successfully!";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Green;
    //            lblvendorMessage.CssClass = "success";
    //        }
    //        else
    //        {
    //            lblvendorMessage.Text = "Error approving vendor. Please try again.";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Red;
    //            lblvendorMessage.CssClass = "errorMsg";
    //        }
    //    }
    //    else if (commandName == "reject")
    //    {
    //        if (string.IsNullOrEmpty(Remark))
    //        {
    //            lblvendorMessage.Text = "Please Enter a Rejection Remark!";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Red;
    //            lblvendorMessage.CssClass = "errorMsg";
    //            return;
    //        }

    //        int rejectionResult = DBOperations.UpdVendorRejectStatus(VendorKycId, 212, Remark, loggedInUser.glUserId);  //Rejection StatusId= 212  
    //        if (rejectionResult == 0)
    //        {
    //            lblvendorMessage.Text = "Vendor Detail rejected successfully!";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Green;
    //            lblvendorMessage.CssClass = "success";
    //        }
    //        else
    //        {
    //            lblvendorMessage.Text = "Error rejecting vendor. Please try again.";
    //            lblvendorMessage.ForeColor = System.Drawing.Color.Red;
    //            lblvendorMessage.CssClass = "errorMsg";
    //        }
    //    }

    //    gvApprovalDetails.DataBind();
    //    ModalPopupExtender.Show();
    //}

    //protected void btnClosePopup_Click(object sender, EventArgs e)
    //{
    //    ModalPopupExtender.Hide();
    //}

    //#region Documents Download 
    //protected void GridViewDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Download")
    //    {
    //        string docPath = e.CommandArgument.ToString();
    //        DownloadDocument(docPath);
    //    }
    //}

    //protected void DownloadDocument(string ExeCertificatePath)
    //{
    //    string ServerPath = FileServer.GetFileServerDir();
    //    if (ServerPath == "")
    //        ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\VendorDoc\\" + ExeCertificatePath);  //VendorDoc\\
    //    else
    //        ServerPath = ServerPath + ExeCertificatePath;
    //    try
    //    {
    //        System.Web.HttpResponse response = Page.Response;
    //        FileDownload.Download(response, ServerPath, ExeCertificatePath);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

  //  #endregion

}
