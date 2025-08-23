using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit;
using ClosedXML.Excel;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Data.Common;
using System.Drawing;
using Ionic.Zip;

public partial class Transport_ApproveBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnReject);
        ScriptManager1.RegisterPostBackControl(btnApprove);
        ScriptManager1.RegisterPostBackControl(imgbtnPackingList);
        ViewState["TotalAmt"] = 0;
        if (Session["TransBillId"] == null && Session["TRId"] == null)
        {
            Response.Redirect("TransBillApproval.aspx");
        }

        if (!IsPostBack)
        {
            TruckRequestDetail(Convert.ToInt32(Session["TRId"]));
        }
    }

    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequestDetail(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            gvTransportJobDetail.Visible = false;
            fsConsolidateJobs.Visible = false;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Approve Bill";

            lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
                lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
            lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
            lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
            lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            if (Session["TRConsolidateId"] != null)
            {
                fsGeneralTransportDetails.Visible = false;
                gvTransportJobDetail.DataBind();
                if (gvTransportJobDetail != null)
                {
                    if (gvTransportJobDetail.Rows.Count > 0)
                    {
                        gvTransportJobDetail.Visible = true;
                        fsConsolidateJobs.Visible = true;
                    }
                }

                if (GridViewVehicle != null)
                {
                    GridViewVehicle.HeaderRow.Cells[8].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[9].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[10].Visible = false;
                    GridViewVehicle.HeaderRow.Cells[11].Visible = false;

                    for (int i = 0; i < GridViewVehicle.Rows.Count; i++)
                    {
                        GridViewVehicle.Rows[i].Cells[8].Visible = false;
                        GridViewVehicle.Rows[i].Cells[9].Visible = false;
                        GridViewVehicle.Rows[i].Cells[10].Visible = false;
                        GridViewVehicle.Rows[i].Cells[11].Visible = false;
                    }
                }
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("TransBillApproval.aspx");
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        decimal dcApprovedAmt = 0;
        int TransBillId = 0;

        if (txtApprovedAmount.Text.Trim() != "")
        {
            dcApprovedAmt = Convert.ToDecimal(txtApprovedAmount.Text.Trim());
        }

        if (Session["TransBillId"].ToString() != "")
        {
            TransBillId = Convert.ToInt32(Session["TransBillId"]);
        }

        if (TransBillId > 0)
        {
            int result = DBOperations.AddTransApproveRejectBill(TransBillId, 1, dcApprovedAmt, LoggedInUser.glUserId);
            if (result == 0)
            {
                // add approval history in table
                int AddHistory = DBOperations.AddTransBillApprovalHistory(TransBillId, 1, txtApprovalRemark.Text.Trim(), LoggedInUser.glUserId);
                if (AddHistory == 0)
                {
                    int result_BillNonReceive = DBOperations.AddBillReceivedDetail(TransBillId, LoggedInUser.glUserId, 0, DateTime.Now, DateTime.MinValue, 0, "", DateTime.MinValue, "", DateTime.MinValue, LoggedInUser.glUserId);
                    lblError.Text = "Successfully approved transport bill!";
                    lblError.CssClass = "success";
                    mpePopup.Hide();
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
            }
        }
        else
        {
            lblPopup_Error.Text = "Error while approving bill. Please try again later.";
            lblPopup_Error.CssClass = "errorMsg";
            mpePopup.Show();
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        int TransBillId = 0;
        if (Session["TransBillId"].ToString() != "")
        {
            TransBillId = Convert.ToInt32(Session["TransBillId"]);
        }

        if (TransBillId > 0)
        {
            if (txtRemark_Rejected.Text.Trim() != "")
            {
                int result = DBOperations.AddTransApproveRejectBill(TransBillId, 2, 0, LoggedInUser.glUserId);
                if (result == 0)
                {
                    // add approval history in table
                    int AddHistory = DBOperations.AddTransBillApprovalHistory(TransBillId, 2, txtRemark_Rejected.Text.Trim(), LoggedInUser.glUserId);
                    if (AddHistory == 0)
                    {
                        lblError.Text = "Successfully approved transport bill!";
                        lblError.CssClass = "success";
                        mpePopup.Hide();
                        btnReject.Visible = false;
                        btnApprove.Visible = false;
                    }
                }
            }
            else
            {
                lblPopup_Error.Text = "Please enter remark.";
                lblPopup_Error.CssClass = "errorMsg";
                mpePopup.Show();
            }
        }
        else
        {
            lblPopup_Error.Text = "Error while rejecting bill. Please try again later.";
            lblPopup_Error.CssClass = "errorMsg";
            mpePopup.Show();
        }
    }

    protected void gvBillDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcTotalAmt = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmt;
            lblActualTotalAmt.Text = ViewState["TotalAmt"].ToString();
            txtApprovedAmount.Text = ViewState["TotalAmt"].ToString();
        }
    }

    protected void btnRejectPopup_Click(object sender, EventArgs e)
    {
        dvRemark.Visible = true;
        dvApprovalAmount.Visible = false;
        mpePopup.Show();
        lblPopup_Title.Text = "Reject Bill";
        lblPopup_Title.Font.Bold = true;
    }

    protected void btnApprovePopup_Click(object sender, EventArgs e)
    {
        dvRemark.Visible = false;
        dvApprovalAmount.Visible = true;
        mpePopup.Show();
        lblPopup_Title.Text = "Approve Bill";
        lblPopup_Title.Font.Bold = true;
    }

    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpePopup.Hide();
    }

    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument(TransRequestId);
        }
    }

    private void DownloadDocument(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "TransportFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion
}