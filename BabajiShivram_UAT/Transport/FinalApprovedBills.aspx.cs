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

public partial class Transport_FinalApprovedBills : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvApprovedBills);
        if (!IsPostBack)
        {
            if (Convert.ToString(Session["UserId"]) != null)
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Final Approved Bills";
            }
        }

        DataFilter1.DataSource = DataSourceApprovedBills;
        DataFilter1.DataColumns = gvApprovedBills.Columns;
        DataFilter1.FilterSessionID = "FinalApprovedBills.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GRID VIEW EVENTS

    protected void gvApprovedBills_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "selectvehicles")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = gvApprovedBills.DataKeys[gvrow.RowIndex].Values[0].ToString();
            string strTransporterId = gvApprovedBills.DataKeys[gvrow.RowIndex].Values[1].ToString();
            string strVehicleNo = gvApprovedBills.DataKeys[gvrow.RowIndex].Values[2].ToString();

            if (strJobId != "")
            {
                Session["TpJobId"] = strJobId;
                Session["TransporterId"] = strTransporterId;
                Session["VehicleNo"] = strVehicleNo;
                Response.Redirect("ApprovedVehicle.aspx");
            }
        }
        else if (e.CommandName.ToLower() == "downloaddoc")
        {
            string DocPath = e.CommandArgument.ToString();
            if (DocPath != "")
                DownloadDocument(DocPath);
        }     
    }

    protected void gvApprovedBills_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvApprovedBills_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgbtnDwnInvoice = (ImageButton)e.Row.FindControl("imgbtnDwnInvoice");
            if (imgbtnDwnInvoice != null)
                ScriptManager1.RegisterPostBackControl(imgbtnDwnInvoice);

            string DocumentPath = (string)DataBinder.Eval(e.Row.DataItem, "DocumentPath").ToString();
            if (DocumentPath == "")
                e.Row.Cells[2].Text = "";
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

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "DeliveredJobVehicleDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        DataSet dsDeliveredVehicle = DBOperations.GetVehicleWsBillDetails();
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

                var workSheet = wb.Worksheets.Add(dtReport);
                var SrNo_Col = workSheet.Column("A");
                SrNo_Col.Width = 8;
                var JobRefNo = workSheet.Column("B");
                JobRefNo.Width = 21;
                var Vehicles = workSheet.Column("C");
                Vehicles.Width = 15;
                var UpdVehicles = workSheet.Column("D");
                UpdVehicles.Width = 15;
                var Deliveryfrom = workSheet.Column("E");
                Deliveryfrom.Width = 22;
                var DeliveryTo = workSheet.Column("F");
                DeliveryTo.Width = 25;
                var DispatchDate = workSheet.Column("G");
                DispatchDate.Width = 13;
                var Freightrate = workSheet.Column("H");
                Freightrate.Width = 15;
                var DetentionTot = workSheet.Column("I");
                DetentionTot.Width = 15;
                var WaraiTot = workSheet.Column("J");
                WaraiTot.Width = 13;
                var EmptyOffTot = workSheet.Column("K");
                EmptyOffTot.Width = 23;
                var TempoUnionTot = workSheet.Column("L");
                TempoUnionTot.Width = 18;

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
            DataFilter1.FilterSessionID = "FinalApprovedBills.aspx";
            DataFilter1.FilterDataSource();
            gvApprovedBills.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}