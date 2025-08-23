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

public partial class Transport_PendingTransDelivery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExportConsolidate);

        if (!IsPostBack)
        {
            Session["JobId"] = null;
            Session["ConsolidateId"] = null;
            Session["ConsolidateJob"] = null;
            Session["CommonCustomer"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Delivery - Clearance";

            DataFilter1.DataSource = PendingDeliverySqlDataSource;
            DataFilter1.DataColumns = gvJobDetail.Columns;
            DataFilter1.FilterSessionID = "PendingTransDelivery.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvJobDetail.DataBind();

            DataFilter2.DataSource = DataSourceConsolidateJobs;
            DataFilter2.DataColumns = gvConsolidateJob.Columns;
            DataFilter2.FilterSessionID = "PendingTransDelivery1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvConsolidateJob.DataBind();
        }
        if (TabPanelNormalJob.TabIndex == 0)
        {
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                lblMessage.Text = "";
                DataFilter1.DataSource = PendingDeliverySqlDataSource;
                DataFilter1.DataColumns = gvJobDetail.Columns;
                DataFilter1.FilterSessionID = "PendingTransDelivery.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            }
        }
        if (TabPanelConsolidateJob.TabIndex == 1)
        {
            if (TabRequestRecd.ActiveTabIndex == 1)
            {
                lblMessage.Text = "";
                DataFilter2.DataSource = DataSourceConsolidateJobs;
                DataFilter2.DataColumns = gvConsolidateJob.Columns;
                DataFilter2.FilterSessionID = "PendingTransDelivery1.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);

            }
        }
    }

    #region Consolidate Delivery

    protected void gvConsolidateJob_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string strJobId = "";
        string strCustomerId = "0";
        bool isCommonCustomer = false;
        int JobCount = 0;

        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            int ConsolidateID = Convert.ToInt32(commandArgs[0].ToString());
            int TransReqId = Convert.ToInt32(commandArgs[1].ToString());
            if (e.CommandArgument.ToString() != "")
            {
                DataSet dsGetDetail = DBOperations.GetTransConsolidateJobDetail(ConsolidateID);
                if (dsGetDetail != null)
                {
                    for (int i = 0; i < dsGetDetail.Tables[0].Rows.Count; i++)
                    {
                        JobCount++;
                        strJobId = strJobId + dsGetDetail.Tables[0].Rows[i]["JobId"].ToString() + ",";
                        if (dsGetDetail.Tables[0].Rows[i]["CustomerId"].ToString() != "")
                        {
                            if (dsGetDetail.Tables[0].Rows[i]["CustomerId"].ToString() != strCustomerId)
                            {
                                isCommonCustomer = false;
                            }
                            else
                            {
                                isCommonCustomer = true;
                            }
                        }
                        strCustomerId = dsGetDetail.Tables[0].Rows[i]["CustomerId"].ToString();
                    }
                }

                if (strJobId != "" && JobCount > 0)
                {
                    Session["ConsolidateJob"] = strJobId;
                    Session["CommonCustomer"] = isCommonCustomer;
                    Session["TrConsolidateId"] = TransReqId.ToString();
                    Response.Redirect("TransClearance.aspx");
                }
            }
        }
    }

    protected void btnConsolidate_Click(object sender, EventArgs e)
    {
        string strJobId = "";
        string strCustomerId = "0";
        bool isCommonCustomer = false;

        int JobCount = 0;
        foreach (GridViewRow gvRow in gvJobDetail.Rows)
        {
            CheckBox chkSelect = (gvRow.FindControl("chkSelect") as CheckBox);

            if (chkSelect.Checked)
            {

                JobCount = JobCount + 1;
                strJobId = strJobId + gvJobDetail.DataKeys[gvRow.RowIndex]["JobId"].ToString() + ",";

                if (gvJobDetail.DataKeys[gvRow.RowIndex]["CustomerId"].ToString() != strCustomerId)
                {
                    isCommonCustomer = false;
                }
                else
                {
                    isCommonCustomer = true;
                }

                strCustomerId = gvJobDetail.DataKeys[gvRow.RowIndex]["CustomerId"].ToString();
            }
        }

        if (JobCount < 2)
        {
            lblMessage.Text = "Please select atleast two Job for consolidated delivery!";
            lblMessage.CssClass = "errorMsg";
            return;
        }
        else if (strJobId != "")
        {
            Session["ConsolidateJob"] = strJobId;
            Session["CommonCustomer"] = isCommonCustomer;
            Response.Redirect("TransClearance.aspx");
        }
    }

    #endregion

    #region GridView Event
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;

            Session["JobId"] = strJobId;

            Response.Redirect("TransDeliveryDetail.aspx");
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DeliveryType") != DBNull.Value)
            {
                // Hide Consolidate delivery checkbox selection for Loaded type delivery

                int DeliveryTypeId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DeliveryType"));
                if (DeliveryTypeId == (int)DeliveryType.Loaded)
                {
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    chkSelect.Visible = false;
                }
            }
        }

    }
    protected void gvJobDetail_PreRender(object sender, EventArgs e)
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

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                DataFilter1_OnDataBound();
            }
            else
            {
                DataFilter2_OnDataBound();
            }
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "PendingTransDelivery.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "PendingTransDelivery1.aspx";
            DataFilter2.DataColumns = gvConsolidateJob.Columns;
            DataFilter2.FilterDataSource();
            gvConsolidateJob.DataBind();
            if (gvConsolidateJob.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Consolidate Job Detail!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "PendingDelivery_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        JobExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    protected void lnkExportConsolidate_Click(object sender, EventArgs e)
    {
        string strFileName = "PendingConsoDelivery_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ConsolidateExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void JobExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = false;
        gvJobDetail.Columns[3].Visible = true;

        gvJobDetail.Caption = "Pending Delivery Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingTransDelivery.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //gvJobDetail.DataSourceID = "PendingDeliverySqlDataSource";
        //gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void ConsolidateExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvConsolidateJob.AllowPaging = false;
        gvConsolidateJob.AllowSorting = false;
        gvConsolidateJob.Columns[1].Visible = false;
        gvConsolidateJob.Columns[2].Visible = true;

        gvConsolidateJob.Caption = "Pending Consolidate Delivery Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter2.FilterSessionID = "PendingDelivery1.aspx";
        DataFilter2.FilterDataSource();
        gvConsolidateJob.DataBind();

        //gvJobDetail.DataSourceID = "PendingDeliverySqlDataSource";
        //gvJobDetail.DataBind();

        gvConsolidateJob.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}