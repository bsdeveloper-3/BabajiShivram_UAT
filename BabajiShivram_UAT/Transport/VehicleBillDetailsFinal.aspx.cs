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

public partial class Transport_VehicleBillDetailsFinal : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
       // ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnApprove);
        ScriptManager1.RegisterPostBackControl(gvNFormDetail);

        if (!IsPostBack)
        {
            if (Convert.ToString(Session["UserId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Vehicle Bill Details";
                if (Session["TpJobId"] != null)
                    JobDetailMS(Convert.ToInt32(Session["TpJobId"]));

                #region INCLUDE NFORM DETAIL

                int BranchId = DBOperations.GetJobBranchDetail(Convert.ToInt32(Session["TpJobId"]));
                if (BranchId != 0 && BranchId == 2) // (if branch = Mumbai Cargo)
                {
                    fsNform.Visible = true;
                    gvNFormDetail.Visible = true;
                    gvNFormDetail.DataBind();
                }
                else
                {
                    fsNform.Visible = false;
                    gvNFormDetail.Visible = false;
                }

                #endregion
            }
        }
    }

    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);
        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("Dashboard.aspx");
            Session["TpJobId"] = null;
        }

        lblTitle.Text = "Vehicle Bill Details - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
    }

    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        try
        {
            int approvedResult = DBOperations.UpdateJobBillStatus(Convert.ToInt32(Session["TpJobId"].ToString()), Convert.ToInt32(3), Convert.ToInt32(LoggedInUser.glUserId),
                      Convert.ToInt32(LoggedInUser.glUserId), Convert.ToDateTime(DateTime.Now), Convert.ToString(""), Convert.ToString(""));

            if (approvedResult == 0)
            {
                gvVehicleDetails.Visible = false;
                GridViewVehicle.Visible = false;
                btnApprove.Visible = false;
                gvJobDetail.Visible = false;
                gvVehicleDetails.Visible = false;
                fsApprovedRate.Visible = false;
                fsNform.Visible = false;
                lblError.Text = "Successfully approved bill status. Bill submitted to Accounts Department.";
                lblError.CssClass = "success";
            }
            else if (approvedResult == 1)
            {
                lblError.Text = "Error while approving bill status. Please try again later.";
                lblError.CssClass = "errorMsg";
            }
        }
        catch (Exception en)
        {
        }
    }

    #region GRID VIEW EVENTS

    protected void gvVehicleDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgbtnLRDoc = (ImageButton)e.Row.FindControl("imgbtnLRDoc");
            if (imgbtnLRDoc != null)
                ScriptManager1.RegisterPostBackControl(imgbtnLRDoc);

            ImageButton imgbtnReceipt = (ImageButton)e.Row.FindControl("imgbtnReceipt");
            if (imgbtnReceipt != null)
                ScriptManager1.RegisterPostBackControl(imgbtnReceipt);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strLrCopies = (string)DataBinder.Eval(e.Row.DataItem, "LrCopiesDocPath").ToString();
            if (strLrCopies == "")
                e.Row.Cells[19].Text = "";

            string strReceipt = (string)DataBinder.Eval(e.Row.DataItem, "ReceiptDocPath").ToString();
            if (strReceipt == "")
            {
                e.Row.Cells[1].Text = "";
                e.Row.Cells[20].Text = "";
            }

            string strRemark = (string)DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
            if (strRemark == "")
            {
                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemarks");
                if (txtRemark != null)
                    txtRemark.Visible = false;
            }

            // Check If Submitted bill is valid

            //if (DataBinder.Eval(e.Row.DataItem, "IsValidBill") != DBNull.Value)
            //{
            //    bool IsValid = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsValidBill"));
            //    if (IsValid == false)
            //    {
            //        e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
            //        e.Row.ToolTip = "Not Valid Freight Bill Amount";
            //    }
            //}

            if (DataBinder.Eval(e.Row.DataItem, "ApprovedRate") != DBNull.Value && DataBinder.Eval(e.Row.DataItem, "TPFrightRate") != DBNull.Value)
            {
                decimal dcApprovedRate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ApprovedRate"));
                decimal dcFreightRate = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TPFrightRate"));

                int result = Decimal.Compare(dcApprovedRate, dcFreightRate);

                if (result < 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Approved Rate and Freight Rate does not match.";
                }
                else if (result > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#edf66b");
                    e.Row.ToolTip = "Approved Rate is more than Freight Rate.";
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#ffffff");
                }
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.FromName("#ffffff");
            }
        }
    }

    protected void gvVehicleDetails_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "downloadcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
    }

    protected void gvVehicleDetails_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvVehicleDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblError.Visible = false;
        lblError.Text = "";
        gvVehicleDetails.EditIndex = -1;
        gvVehicleDetails.DataBind();
    }

    protected void gvVehicleDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblError.Visible = false;
        lblError.Text = "";
        gvVehicleDetails.EditIndex = -1;
        gvVehicleDetails.DataBind();
    }

    protected void gvVehicleDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int lid = Convert.ToInt32(gvVehicleDetails.DataKeys[e.RowIndex].Values[1].ToString());

        TextBox txtFreightRate = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtFreightRate");
        TextBox txtDetentionDays = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtDetentionDays");
        TextBox txtDetentionCharges = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtDetentionCharges");
        TextBox txtWaraiCharges = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtWaraiCharges");
        TextBox txtEmptyOffLoadingCharges = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtEmptyOffLoadingCharges");
        TextBox txtTempoUnionCharges = (TextBox)gvVehicleDetails.Rows[e.RowIndex].FindControl("txtTempoUnionCharges");

        //if (txtDetentionDays.Text != "0")
        //{
        //    if (txtDetentionCharges.Text == "0.00")
        //    {
        //        lblError.Text = "Please enter detention charges.";
        //        lblError.CssClass = "errorMsg";
        //    }
        //}

        int result = DBOperations.UpdateVehicleBillFinal(lid, txtFreightRate.Text.Trim(), Convert.ToInt32(txtDetentionDays.Text), txtDetentionCharges.Text,
                                  txtWaraiCharges.Text, txtEmptyOffLoadingCharges.Text, txtTempoUnionCharges.Text, Convert.ToInt32(Session["UserId"]));

        if (result == 0)
        {
            lblError.Text = "Successfully Updated Vehicle Details.";
            lblError.CssClass = "success";

            gvVehicleDetails.DataBind();
            gvVehicleDetails.EditIndex = -1;
            e.Cancel = true;
        }

        else
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void gvNFormDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "downloadnformcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "VehicleBillDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        DataSet dsDeliveredVehicle = DBOperations.GetTPVehicleDetailReport(Convert.ToInt32(Session["TpJobId"]));

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dsDeliveredVehicle.Tables[0]);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType, DataTable dtReport)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            dtReport.TableName = "Delivered Vehicle Details";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (dtReport.Rows.Count > 0)
            {
                dtReport.Columns.Add("Sr.No.", typeof(int));
                int i = 1;
                foreach (DataRow dr in dtReport.Rows)
                {
                    dr["Sr.No."] = i;
                    i = i + 1;
                }
                dtReport.Columns["Sr.No."].SetOrdinal(0);

                ////////////////////////////////////////  CALCULATE TOTAL OF CHARGES COLUMN   /////////////////////////////////////////
                object total_Freight, Total_Detention, Total_Warai, Total_EmptyOffLoading, Total_TempoUnion;
                total_Freight = dtReport.Compute("Sum([Transporter Freight Rate])", "");
                Total_Detention = dtReport.Compute("Sum([Detention Charges])", "");
                Total_Warai = dtReport.Compute("Sum([Warai Charges])", "");
                Total_EmptyOffLoading = dtReport.Compute("Sum([Empty Off Loading Charges])", "");
                Total_TempoUnion = dtReport.Compute("Sum([Tempo Union Charges])", "");

                DataRow dr_GranTotal = dtReport.NewRow();
                dr_GranTotal["Transporter Freight Rate"] = total_Freight;
                dr_GranTotal["Detention Charges"] = Total_Detention;
                dr_GranTotal["Warai Charges"] = Total_Warai;
                dr_GranTotal["Empty Off Loading Charges"] = Total_EmptyOffLoading;
                dr_GranTotal["Tempo Union Charges"] = Total_TempoUnion;

                int lastRow = dtReport.Rows.Count;
                dtReport.Rows.InsertAt(dr_GranTotal, lastRow + 1);
                ////////////////////////////////////////  CALCULATE TOTAL OF CHARGES COLUMN   /////////////////////////////////////////

                var workSheet = wb.Worksheets.Add(dtReport);
                var SrNo_Col = workSheet.Column("A");
                SrNo_Col.Width = 7;
                var JobRefNo = workSheet.Column("B");
                JobRefNo.Width = 21;
                var VehicleNo = workSheet.Column("C");
                VehicleNo.Width = 16;
                var VehicleType = workSheet.Column("D");
                VehicleType.Width = 16;
                var ContNo = workSheet.Column("E");
                ContNo.Width = 13;
                var ContSize = workSheet.Column("F");
                ContSize.Width = 13;
                var Pkg = workSheet.Column("G");
                Pkg.Width = 9;
                var DeliveryPoint = workSheet.Column("H");
                ContSize.Width = 18;
                var DispatchDate = workSheet.Column("I");
                DispatchDate.Width = 13;
                var DeliveryDate = workSheet.Column("J");
                DeliveryDate.Width = 13;
                var ReportDate = workSheet.Column("K");
                ReportDate.Width = 13;
                var UnloadDate = workSheet.Column("L");
                UnloadDate.Width = 18;
                var approvedrate = workSheet.Column("M");
                approvedrate.Width = 15;
                var Freight = workSheet.Column("N");
                Freight.Width = 25;
                var DetDays = workSheet.Column("O");
                DetDays.Width = 18;
                var DetChg = workSheet.Column("P");
                DetChg.Width = 18;
                var WaraiChg = workSheet.Column("Q");
                WaraiChg.Width = 18;
                var EmptyOffChg = workSheet.Column("R");
                EmptyOffChg.Width = 26;
                var Tempo = workSheet.Column("S");
                Tempo.Width = 22;
                var EmptyRetDate = workSheet.Column("T");
                EmptyRetDate.Width = 41;

                workSheet.Style.Alignment.WrapText = true;
                workSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    #endregion

    #region DOCUMENTS DOWNLOAD EVENTS

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