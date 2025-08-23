using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Transport_TransBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkJobExport);
        ScriptManager1.RegisterPostBackControl(lnkConsolidateExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Bill";

            Session["TransReqId"] = null;
            Session["TransporterID"] = null;
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "TransBill.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //else
        //{
        //    //if (TabPanelNormalJob.TabIndex == 0)
        //    //{
        //    //    if (TabBilling.ActiveTabIndex == 0)
        //    //    {
        //    //        lblError_Job.Text = "";
        //    //        DataFilter1.DataSource = DataSourceVehicle;
        //    //        DataFilter1.DataColumns = gvJobDetail.Columns;
        //    //        DataFilter1.FilterSessionID = "TransBill.aspx";
        //    //        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //    //    }
        //    //}

        //    //if (TabPanelConsolidateJob.TabIndex == 1)
        //    //{
        //    //    if (TabBilling.ActiveTabIndex == 1)
        //    //    {
        //    //        lblError_Consolidate.Text = "";
        //    //        DataFilter2.DataSource = DataSourceConsolidateVehicle;
        //    //        DataFilter2.DataColumns = gvConsolidateBill.Columns;
        //    //        DataFilter2.FilterSessionID = "TransBill.aspx";
        //    //        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        //    //    }
        //    //}
        //}
    }

    #region Job Detail

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // For Edit Button Click. 
        // IF First Check Required BOE is Non-RMS
        /**********************************
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvJobDetail.EditIndex)
        {
            Label cboFirstCheck = (Label)e.Row.FindControl("lblFirstCheck");
            if (cboFirstCheck.Text.ToLower() == "yes")
            {
                DropDownList cboRMS = (DropDownList)e.Row.FindControl("ddRMS");
                cboRMS.SelectedValue = "2"; // Non-RMS
                cboRMS.Enabled = false;
            }
        }
        **********************************/

    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string TransReqId = gvJobDetail.DataKeys[gvrow.RowIndex].Values[0].ToString();
            string ConsolidateID = gvJobDetail.DataKeys[gvrow.RowIndex].Values[1].ToString();

            Session["TransReqId"] = TransReqId;
            if (ConsolidateID != "")
            {
                Session["TRConsolidateId"] = ConsolidateID;
                Response.Redirect("ConsolidateBill.aspx");
            }
            else
            {
                Session["TransporterId"] = e.CommandArgument.ToString();
                Response.Redirect("TestBillDetail.aspx");
            }
        }

    }

    #endregion

    #region Consolidate Job

    protected void gvConsolidateBill_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            if (e.CommandArgument.ToString() != "")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(';');
                Session["TRConsolidateId"] = commandArgs[0].ToString();
                Session["TransReqId"] = commandArgs[1].ToString();
                Session["TRRefNo"] = commandArgs[2].ToString();
                Response.Redirect("ConsolidateBill.aspx");
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
            if (TabBilling.ActiveTabIndex == 0)
            {
                DataFilter1_OnDataBound();
            }
            //else
            //{
            //    DataFilter2_OnDataBound();
            //}
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "TransBill.aspx";
            DataFilter1.DataColumns = gvJobDetail.Columns;
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
            if (gvJobDetail.Rows.Count == 0)
            {
                lblError_Job.Text = "No Job Found For Normal Job Detail!";
                lblError_Job.CssClass = "errorMsg";
            }
            else
            {
                lblError_Job.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    //void DataFilter2_OnDataBound()
    //{
    //    try
    //    {
    //        DataFilter2.FilterSessionID = "TransBill.aspx";
    //        DataFilter2.DataColumns = gvConsolidateBill.Columns;
    //        DataFilter2.FilterDataSource();
    //        gvConsolidateBill.DataBind();
    //        if (gvConsolidateBill.Rows.Count == 0)
    //        {
    //            lblError_Consolidate.Text = "No Job Found For Consolidate Job Detail!";
    //            lblError_Consolidate.CssClass = "errorMsg";
    //        }
    //        else
    //        {
    //            lblError_Consolidate.Text = "";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // DataFilter2.Info = ex.Message; // Handle your exceptions
    //    }
    //}

    #endregion

    #region Export Data

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkJobExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;

        gvJobDetail.Caption = "Transport Bill Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TransBill.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkConsolidateExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;

        gvJobDetail.Caption = "Transport Consolidate Bill Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "TransBill.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}