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

public partial class Transport_VehicleBillDetails_Chr : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnRejectBill);
        ScriptManager1.RegisterPostBackControl(btnApprove);
        ScriptManager1.RegisterPostBackControl(btnReject);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["UserId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Vehicle Bill Details";
            }

            //if (Convert.ToString(Session["PageRedirect"]) != null && Convert.ToString(Session["PageRedirect"]) != "")
            //{
            //    if (Convert.ToString(Session["PageRedirect"]) == "RejectedBill")
            //    {
            //        btnReject.Visible = false;
            //        btnApprove.Visible = false;
            //    }
            //}
        }

        DataFilter1.DataSource = DataSourceVehicles;
        DataFilter1.DataColumns = gvVehicleDetails.Columns;
        DataFilter1.FilterSessionID = "VehicleBillDetails_Chr.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnBack_OnClick(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["PageRedirect"]) != null && Convert.ToString(Session["PageRedirect"]) != "")
        {
            if (Convert.ToString(Session["PageRedirect"]) == "ByDept")
                Response.Redirect("ApprovalBillByDept.aspx");
            else if (Convert.ToString(Session["PageRedirect"]) == "ByMngt")
                Response.Redirect("ApprovalBillByDept_Chr.aspx");
        }
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
                btnApprove.Visible = false;
                btnReject.Visible = false;
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

    protected void btnReject_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(Session["TpJobId"]) != null && Convert.ToString(Session["TpJobId"]) != "")
            {
                hdnJobId.Value = Convert.ToString(Session["TpJobId"]);
                ModalPopupRejection.Show();
            }
        }
        catch (Exception en)
        {
        }
    }

    #region REJECT MODAL POPUP

    protected void btnRejectBill_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRejectRemark.Text != "")
            {
                int approvedResult = DBOperations.UpdateJobBillStatus(Convert.ToInt32(hdnJobId.Value), Convert.ToInt32(2), Convert.ToInt32(LoggedInUser.glUserId),
                                   Convert.ToInt32(0), Convert.ToDateTime(DateTime.MinValue), Convert.ToString(txtRejectRemark.Text.Trim()), Convert.ToString(""));
                if (approvedResult == 0)
                {
                    gvVehicleDetails.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    lblError.Text = "Successfully rejected bill. Bill moved to Rejected Bills Tab.";
                    lblError.CssClass = "success";
                }
                else if (approvedResult == 1)
                {
                    lblError.Text = "Error while approving bill status. Please try again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        catch (Exception en)
        {
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupRejection.Hide();
    }

    #endregion

    #region GRID VIEW EVENTS

    protected void FillGridView()
    {
        try
        {
            if (Convert.ToString(Session["TpJobId"]) != null && Convert.ToString(Session["TpJobId"]) != "")
            {
                DataSet dsVehicleDetails = new DataSet();
                dsVehicleDetails = DBOperations.GetDetailedVehicleDetails(Convert.ToInt32(Session["TpJobId"]));

                if (dsVehicleDetails != null)
                {
                    if (dsVehicleDetails.Tables[0].Rows.Count > 0)
                    {
                        object total_Freight, Total_Detention, Total_Warai, Total_EmptyOffLoading, Total_TempoUnion;
                        total_Freight = dsVehicleDetails.Tables[0].Compute("Sum(TPFrightRate)", "");
                        Total_Detention = dsVehicleDetails.Tables[0].Compute("Sum(DetentionCharges)", "");
                        Total_Warai = dsVehicleDetails.Tables[0].Compute("Sum(WaraiCharges)", "");
                        Total_EmptyOffLoading = dsVehicleDetails.Tables[0].Compute("Sum(EmptyOffLoadingCharges)", "");
                        Total_TempoUnion = dsVehicleDetails.Tables[0].Compute("Sum(TempoUnionCharges)", "");

                        DataRow dr_GranTotal = dsVehicleDetails.Tables[0].NewRow();
                        dr_GranTotal["TPFrightRate"] = total_Freight;
                        dr_GranTotal["DetentionCharges"] = Total_Detention;
                        dr_GranTotal["WaraiCharges"] = Total_Warai;
                        dr_GranTotal["EmptyOffLoadingCharges"] = Total_EmptyOffLoading;
                        dr_GranTotal["TempoUnionCharges"] = Total_TempoUnion;

                        int lastRow = dsVehicleDetails.Tables[0].Rows.Count;
                        dsVehicleDetails.Tables[0].Rows.InsertAt(dr_GranTotal, lastRow + 1);

                        gvVehicleDetails.DataSource = dsVehicleDetails;
                        gvVehicleDetails.DataBind();
                        gvVehicleDetails.Visible = true;
                    }

                    if (gvVehicleDetails.Rows.Count > 0)
                    {
                        //gvVehicleDetails.Columns[2].ItemStyle.Wrap = true;
                        var lastRow = gvVehicleDetails.Rows[gvVehicleDetails.Rows.Count - 1];
                        lastRow.Cells[0].Text = "";
                        lastRow.Cells[1].Text = "Sub Total";
                        lastRow.Cells[1].Font.Bold = true;
                        lastRow.Cells[16].Text = "";
                        lastRow.Cells[17].Text = "";

                        lastRow.Cells[10].Font.Bold = true;
                        //lastRow.Cells[10].Style.Add("border-top", "2px inset");
                        lastRow.Cells[11].Font.Bold = true;
                        // lastRow.Cells[11].Style.Add("border-top", "2px inset");
                        lastRow.Cells[12].Font.Bold = true;
                        // lastRow.Cells[12].Style.Add("border-top", "2px inset");
                        lastRow.Cells[13].Font.Bold = true;
                        // lastRow.Cells[13].Style.Add("border-top", "2px inset");
                        lastRow.Cells[14].Font.Bold = true;
                        //lastRow.Cells[14].Style.Add("border-top", "2px inset");
                    }
                }
            }
        }
        catch (Exception en)
        {
        }
    }

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
                e.Row.Cells[17].Text = "";

            string strReceipt = (string)DataBinder.Eval(e.Row.DataItem, "ReceiptDocPath").ToString();
            if (strReceipt == "")
                e.Row.Cells[18].Text = "";

            string strRemark = (string)DataBinder.Eval(e.Row.DataItem, "Remark").ToString();
            if (strRemark == "")
            {
                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemarks");
                if (txtRemark != null)
                    txtRemark.Visible = false;
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

    #endregion
            
    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "VehicleBillDetails_Chr_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
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

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "VehicleBillDetails_Chr.aspx";
            DataFilter1.FilterDataSource();
            gvVehicleDetails.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}