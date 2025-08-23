using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public partial class Freight_FreightDashboard : System.Web.UI.Page
{
    string str_Message = "";
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkTotalSummaryExport);
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkExportSummary);
        ScriptManager1.RegisterPostBackControl(lnkExportSummaryDetail);

        ScriptManager1.RegisterPostBackControl(lnkOlShipment);
        ScriptManager1.RegisterPostBackControl(lnkCurrentJob1Lac);
        ScriptManager1.RegisterPostBackControl(lnkShipmentComplete);
        ScriptManager1.RegisterPostBackControl(lnkExOlShipment);
        ScriptManager1.RegisterPostBackControl(lnkCurrentJob1LacForExport);
        ScriptManager1.RegisterPostBackControl(lnkShipmentCompleteForexport);

        if (!IsPostBack)
        {
            Label lblTitle  =   (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   =   "Freight Dashboard";

            ddMonth.SelectedValue = DateTime.Now.Month.ToString();
                        
        }

        // CreateChart();
    }

    protected void ddPendingFreight_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindFreightSummaryByType();
    }

    protected void ddReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Bind Freight Summary Detail GridView for Selected Type

        BindFreightSummaryByType();
        BindFreightSummaryDetail();
    }
            
    protected void gvSummaryFreight_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSummaryStatusDetail.PageIndex = e.NewPageIndex;

        // Bind Freight Summary Detail GridView for Selected Type

        BindFreightSummaryByType();
    }

    protected void gvSummaryFreight_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindFreightSummaryByType();
    }
    
    private void BindFreightSummaryByType()
    {
        int ReportType = Convert.ToInt32(ddReportType.SelectedValue);

        if (ReportType == 0)
        {
            //legendName.InnerText = "Freight All Summary Status";
            legendName.InnerText = "All " + ddPendingFreight.SelectedItem.Text;

            DataSourceFreightSummary.SelectCommand = "FR_DSSummaryFreightAll";
            gvSummaryFreight.DataBind();
        }
        else if (ReportType == 1)
        {
            //legendName.InnerText = "Freight User Summary Status";
            legendName.InnerText = "User " + ddPendingFreight.SelectedItem.Text;
            DataSourceFreightSummary.SelectCommand = "FR_DSSummaryFreightUser";
            gvSummaryFreight.DataBind();
        }
        else if (ReportType == 2)
        {
           // legendName.InnerText = "Freight Mode Summary";
            legendName.InnerText = "Mode " + ddPendingFreight.SelectedItem.Text;
            DataSourceFreightSummary.SelectCommand = "FR_DSSummaryMode";
            gvSummaryFreight.DataBind();
        }
        else if (ReportType == 3)
        {
           // legendName.InnerText = "Freight Type Summary";
            legendName.InnerText = "Type " + ddPendingFreight.SelectedItem.Text;
            DataSourceFreightSummary.SelectCommand = "FR_DSSummaryType";
            gvSummaryFreight.DataBind();
        }
        else if (ReportType == 4)
        {
           // legendName.InnerText = "Customer Summary Status";
            legendName.InnerText = "Customer " + ddPendingFreight.SelectedItem.Text;
            DataSourceFreightSummary.SelectCommand = "FR_DSSummaryCustomer";
            gvSummaryFreight.DataBind();
        }
        else if (ReportType == 5)
        {
            //legendName.InnerText = "Sales Representative Summary Status";
            legendName.InnerText = "Sales Reps " + ddPendingFreight.SelectedItem.Text;
            DataSourceFreightSummary.SelectCommand = "FR_DSSummarySalesRep";
            gvSummaryFreight.DataBind();
        }
    }

    private void BindFreightSummaryDetail()
    {
        int ReportType = Convert.ToInt32(ddReportType.SelectedValue);
        
        if (ReportType == 0)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummaryFreightAllDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 1)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummaryFreightUserDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 2)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummaryModeDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 3)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummaryTypeDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 4)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummaryCustomerDetail";
            DataSourceSummaryDetail.DataBind();
        }
        else if (ReportType == 5)
        {
            DataSourceSummaryDetail.SelectCommand = "FR_DSSummarySalesRepDetail";
            DataSourceSummaryDetail.DataBind();
        }
    }

    #region Popup Month Stage
    protected void gvSummaryMonth_RowCommand(object sender, GridViewCommandEventArgs e )
    {
        if (e.CommandName.ToLower() == "monthdetail")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strStageId = "", strMonthId = "", strSummaryType = "";

            strStageId      = commandArgs[0].ToString();
            strMonthId      = commandArgs[1].ToString();
            strSummaryType  = ddPendingFreight.SelectedValue;

            DataSourceMonthDetail.SelectParameters["MonthId"].DefaultValue      = strMonthId;
            DataSourceMonthDetail.SelectParameters["StatusId"].DefaultValue     = strStageId;
            DataSourceMonthDetail.SelectParameters["ReportType"].DefaultValue   = strSummaryType;

            gvMonthStatusDetail.PageIndex = 0;
           // DataSourceMonthDetail.DataBind();
            gvMonthStatusDetail.DataBind();

            if(gvMonthStatusDetail.Rows.Count > 0)
                ModalPopupMonthStatus.Show();
        }
    }

    protected void btnCancelMonthPopup_Click(object sender, EventArgs e)
    {
        ModalPopupMonthStatus.Hide();
    }

    protected void gvMonthStatusDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ModalPopupMonthStatus.Show();
    }

    protected void gvMonthStatusDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        ModalPopupMonthStatus.Show();
    }
    #endregion

    #region Popup Summary Detail

    protected void gvSummaryFreight_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "summarydetail")
        {
            ModalPopupSummaryDetail.Show();

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            string strDetailFor = "", strStageId = "", strMonthId = "", strSummaryType = "", strStageName="";
            int ReportType = 0;
            
            strDetailFor    = commandArgs[0].ToString();
            strStageId      = commandArgs[1].ToString();
            strMonthId      = ddMonth.SelectedValue;
            strSummaryType  = ddPendingFreight.SelectedValue;

            ReportType      = Convert.ToInt32(ddReportType.SelectedValue);

            GridViewRow row     =   (GridViewRow)((Control)e.CommandSource).Parent.Parent;
            //LinkButton lnk  =   (LinkButton)(e.CommandSource);
            
            Label lblName = (Label)gvSummaryFreight.Rows[row.RowIndex].FindControl("lnksName");

            if (strStageId == "1")
                strStageName = "Enquiry";
            else if (strStageId == "2")
                strStageName = "Quoted";
            else if (strStageId == "3")
                strStageName = "Awarded";
            else if (strStageId == "4")
                strStageName = "Lost";
            else if (strStageId == "5")
                strStageName = "Executed";
            else if (strStageId == "6")
                strStageName = "Budgetary";
            else if (strStageId == "7")
                strStageName = "Lead";

            lblSummaryStatus.Text = lblName.Text + " - " + ddMonth.SelectedItem.Text + " - " + ddPendingFreight.SelectedItem.Text + " - " + strStageName;
                        
            DataSourceSummaryDetail.SelectParameters["lId"].DefaultValue        =   strDetailFor;
            DataSourceSummaryDetail.SelectParameters["MonthId"].DefaultValue    =   strMonthId;
            DataSourceSummaryDetail.SelectParameters["StatusId"].DefaultValue   =   strStageId;
            DataSourceSummaryDetail.SelectParameters["ReportType"].DefaultValue =   strSummaryType;

            gvSummaryStatusDetail.PageIndex = 0;
            
            BindFreightSummaryDetail();
        }
    }

    protected void btnCancelSummaryPopup_Click(object sender, EventArgs e)
    {
        ModalPopupSummaryDetail.Hide();
    }

    protected void gvSummaryStatusDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ModalPopupSummaryDetail.Show();
        BindFreightSummaryDetail();
    }

    protected void gvSummaryStatusDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        ModalPopupSummaryDetail.Show();
        BindFreightSummaryDetail();
    }
    #endregion

    #region DataSource Event
    protected void DataSourceSummaryDetail_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            //Show error message
            
            //Set the exception handled property so it doesn't bubble-up
            e.ExceptionHandled = true;
        }
    }
    #endregion
        
    #region ExportData
    
    protected void lnkTotalSummaryExport_Click(object sender, EventArgs e)
    {
        string strReportType = ddPendingFreight.SelectedItem.Text +"_";
        string strReportFor = ddReportType.SelectedItem.Text +"_";

        string strFileName = "FreightStatus_" + strReportType +DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportTotal("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
        
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strReportType = ddPendingFreight.SelectedItem.Text +"_";
        string strMonth = ddMonth.SelectedItem.Text +"_";
        string strReportFor = ddReportType.SelectedItem.Text +"_";

        string strFileName = "FreightStatus_" + strReportType + strMonth + strReportFor+DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkExportSummary_Click(object sender, EventArgs e)
    {
        string strFileName = "Freight_" + ddPendingFreight.SelectedItem.Text + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportSummary("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkExportSummaryDetail_Click(object sender, EventArgs e)
    {
        string strFileName = lblSummaryStatus.Text +"_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportSummaryDetail("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportTotal(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        
        // Bind Grid
        BindFreightSummaryByType();

        //Remove LinkButtong Controls
        this.RemoveControls(gvSummaryMonth);

        gvSummaryMonth.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }

    private void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvSummaryFreight.AllowPaging = false;
        gvSummaryFreight.AllowSorting = false;
        
        // Bind Grid
        BindFreightSummaryByType();

        //Remove LinkButtong Controls
        this.RemoveControls(gvSummaryFreight);

        gvSummaryFreight.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }

    private void ExportSummary(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvMonthStatusDetail.AllowPaging = false;
        gvMonthStatusDetail.AllowSorting = false;

        // Bind Summary Grid
        gvMonthStatusDetail.DataBind();
        DataSourceMonthDetail.DataBind();

        //Remove LinkButtong Controls
        this.RemoveControls(gvMonthStatusDetail);

        gvMonthStatusDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }

    private void ExportSummaryDetail(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvSummaryStatusDetail.AllowPaging = false;
        gvSummaryStatusDetail.AllowSorting = false;

        // Bind Summary Grid
        BindFreightSummaryDetail();
                
        //Remove LinkButtong Controls
        this.RemoveControls(gvSummaryStatusDetail);

        gvSummaryStatusDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }

    private void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }
    #endregion

    protected void lnkShipmentComplete_Click(object sender, EventArgs e)
    {
        string strFileName = "ImportShipmentOngoing_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        lnkShipmentCompleteExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkCurrentJob1Lac_Click(object sender, EventArgs e)
    {
        string strFileName = "ImportCurrentJobAmountMorethan1Lac_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        lnkCurrentJob1LacExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkOlShipment_Click(object sender, EventArgs e)
    {
        string strFileName = "ImportShipmentCompleted_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        lnkOlShipmentExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkExOlShipment_Click(object sender, EventArgs e)
    {
        string strFileName = "ExportShipmentOngoing_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        lnkExOlShipmentExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkCurrentJob1LacForExport_Click(object sender, EventArgs e)
    {
        string strFileName = "ExportCurrentJobAmountMorethan1Lac_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        EXlnkCurrentJob1LacExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkShipmentCompleteForexport_Click(object sender, EventArgs e)
    {
        string strFileName = "ExportShipmentCompleted_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        EXlnkShipmentCompleteExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void lnkOlShipmentExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvOlShipment.AllowPaging = false;
        gvOlShipment.AllowSorting = false;
        gvOlShipment.Caption = "";

        gvOlShipment.DataSourceID = "DataSourceOlShipment";
        gvOlShipment.DataBind();

        //Remove Controls
        this.RemoveControls(gvOlShipment);

        gvOlShipment.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void lnkCurrentJob1LacExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvCurrentJob.AllowPaging = false;
        gvCurrentJob.AllowSorting = false;
        gvCurrentJob.Caption = "";

        gvCurrentJob.DataSourceID = "DataSourceOlShipmentAmtGreaterThan1Lac";
        gvCurrentJob.DataBind();

        //Remove Controls
        this.RemoveControls(gvCurrentJob);

        gvCurrentJob.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void lnkShipmentCompleteExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvShipmentCleared.AllowPaging = false;
        gvShipmentCleared.AllowSorting = false;
        gvShipmentCleared.Caption = "";

        gvShipmentCleared.DataSourceID = "DataSourceShipmentCleared";
        gvShipmentCleared.DataBind();

        //Remove Controls
        this.RemoveControls(gvShipmentCleared);

        gvShipmentCleared.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void lnkExOlShipmentExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvExOlShipment.AllowPaging = false;
        gvExOlShipment.AllowSorting = false;
        gvExOlShipment.Caption = "";

        gvExOlShipment.DataSourceID = "DataSourceOlShipmentForExport";
        gvExOlShipment.DataBind();

        //Remove Controls
        this.RemoveControls(gvExOlShipment);

        gvExOlShipment.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void EXlnkCurrentJob1LacExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvCurrentJobForExport.AllowPaging = false;
        gvCurrentJobForExport.AllowSorting = false;
        gvCurrentJobForExport.Caption = "";

        gvCurrentJobForExport.DataSourceID = "DataSourceOlShipmentAmtGreaterThan1LacForExport";
        gvCurrentJobForExport.DataBind();

        //Remove Controls
        this.RemoveControls(gvCurrentJobForExport);

        gvCurrentJobForExport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void EXlnkShipmentCompleteExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvShipmentClearedForExport.AllowPaging = false;
        gvShipmentClearedForExport.AllowSorting = false;
        gvShipmentClearedForExport.Caption = "";

        gvShipmentClearedForExport.DataSourceID = "DataSourceShipmentClearedForExport";
        gvShipmentClearedForExport.DataBind();

        //Remove Controls
        this.RemoveControls(gvShipmentClearedForExport);

        gvShipmentClearedForExport.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

}