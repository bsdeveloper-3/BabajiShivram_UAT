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
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

public partial class ContMovement_CDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkbtnNoofJobs);
        ScriptManager1.RegisterPostBackControl(lnkbtnJobDetail);
        ScriptManager1.RegisterPostBackControl(lnkbtnContCFSRecdPending);
        ScriptManager1.RegisterPostBackControl(lnkbtnEmptyContPending2);
        ScriptManager1.RegisterPostBackControl(lnkbtnNoofJobs);
        ScriptManager1.RegisterPostBackControl(lnkbtnJobDetail);
        ScriptManager1.RegisterPostBackControl(lnkbtnJobDetail);
        ScriptManager1.RegisterPostBackControl(gvContCFSRecdPending);

        if (!IsPostBack)
        {
            if (gvNotification != null && gvNotification.Rows.Count > 0)
            {
                int Count = 0;
                Count = gvNotification.Rows.Count;
                lblNotificationCount.Text = "(" + Count.ToString() + ")";
            }
        }

        DataFilter1.DataSource = DataSourceJobDetail;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "CDashboard.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Notification
    protected void gvNotification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            int JobId = Convert.ToInt32(commandArgs[0].ToString());
            int StatusId = Convert.ToInt32(commandArgs[1].ToString());

            if (JobId > 0 && StatusId > 0)
            {
                if (StatusId == 1) // New job created
                {
                    Response.Redirect("MovementDetail.aspx");
                }
                else if (StatusId == 2) // Get Processed Jobs
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("MovementComplete.aspx");
                }
                else if (StatusId == 3) // Get Un-Processed Jobs
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("UnProcessJobs.aspx");
                }
                else if (StatusId == 4) // Get Jobs With Pending Movement Complete Date
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("MovementComplete.aspx");
                }
                else if (StatusId == 5) // Get Jobs With Movement Completed Date
                {
                    Response.Redirect("ContReceived.aspx");
                }
                else if (StatusId == 6) // Get Movement Job Sent to Billing Scrutiny
                {
                    Session["JobId"] = JobId.ToString();
                    Response.Redirect("JobDetail.aspx");
                }
            }
        }
    }
    #endregion

    #region FORM VIEW - No. of Jobs Count
    protected void lnkbtnNoofJobs_Click(object sender, EventArgs e)
    {
        string strFileName = "NoofJobCount" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        fvJobDetail.AllowPaging = false;
        fvJobDetail.DataBind();
        fvJobDetail.ChangeMode(FormViewMode.ReadOnly);

        this.RemoveControls(fvJobDetail);  //Remove Controls
        fvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    protected void lnkbtnNewJobs_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - Newly Created";
        hdnParticularId.Value = "1";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[6].Visible = false; //LastDispatchDate
        gvJobDetail.Columns[13].Visible = false;
        gvJobDetail.Columns[14].Visible = false;
        gvJobDetail.Columns[15].Visible = false;
        gvJobDetail.Columns[16].Visible = false;
        gvJobDetail.Columns[17].Visible = false;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = true;
    }
    protected void lnkbtnETAValidity_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - ETA within 7 days";
        hdnParticularId.Value = "2";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[6].Visible = false;
        gvJobDetail.Columns[13].Visible = false;
        gvJobDetail.Columns[14].Visible = false;
        gvJobDetail.Columns[15].Visible = false;
        gvJobDetail.Columns[16].Visible = false;
        gvJobDetail.Columns[17].Visible = false;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = false;
    }
    protected void lnkbtnMovementCompPending_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - Movement Completion Pending";
        hdnParticularId.Value = "3";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[6].Visible = false;
        gvJobDetail.Columns[13].Visible = true;
        gvJobDetail.Columns[14].Visible = true;
        gvJobDetail.Columns[15].Visible = true;
        gvJobDetail.Columns[16].Visible = false;
        gvJobDetail.Columns[17].Visible = false;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = false;
    }
    protected void lnkbtnContCFSDatePending_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - Container Received at Yard Date Pending";
        hdnParticularId.Value = "5";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[13].Visible = true;
        gvJobDetail.Columns[14].Visible = true;
        gvJobDetail.Columns[15].Visible = true;
        gvJobDetail.Columns[16].Visible = true;
        gvJobDetail.Columns[17].Visible = true;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = false;
    }
    protected void lnkbtnScrutinyPending_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - Billing Scrutiny Pending";
        hdnParticularId.Value = "6";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[13].Visible = true;
        gvJobDetail.Columns[14].Visible = true;
        gvJobDetail.Columns[15].Visible = true;
        gvJobDetail.Columns[16].Visible = true;
        gvJobDetail.Columns[17].Visible = true;
        gvJobDetail.Columns[18].Visible = true;
        gvJobDetail.Columns[19].Visible = false;
    }
    protected void lnkbtnEmptyContReturnPending_Click(object sender, EventArgs e)
    {
        lblTitle.Text = "Number of Jobs Count - Empty Container Return Date Pending";
        hdnParticularId.Value = "4";
        gvJobDetail.DataBind();
        mpeJobDetail.Show();

        gvJobDetail.Columns[13].Visible = true;
        gvJobDetail.Columns[14].Visible = true;
        gvJobDetail.Columns[15].Visible = true;
        gvJobDetail.Columns[16].Visible = true;
        gvJobDetail.Columns[17].Visible = false;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = false;
    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hdnParticularId.Value != "" && hdnParticularId.Value != "0")
            {
                //if (hdnParticularId.Value == "1") // Newly created jobs
                //{
                bool MovementReq = false;
                if ((DataBinder.Eval(e.Row.DataItem, "MovementRequired")) != DBNull.Value)
                {
                    MovementReq = (bool)(DataBinder.Eval(e.Row.DataItem, "MovementRequired"));
                    if (MovementReq == true)
                    {
                        e.Row.BackColor = System.Drawing.Color.FromName("#2266bb82");
                        e.Row.ToolTip = "PN Movement in our scope.";
                    }
                }
                //}
            }
        }
    }
    #endregion

    #region Empty Container Return Date Pending - More than 3 days 
    protected void lnkbtnEmptyContPending2_Click(object sender, EventArgs e)
    {
        string strFileName = "EmptyContReturnDatePending" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvEmptyContDatePending.AllowPaging = false;
        gvEmptyContDatePending.AllowSorting = false;
        gvEmptyContDatePending.Caption = "Empty Container Return Date Pending (More than 3 days) On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "CDashboard.aspx";
        DataFilter1.FilterDataSource();
        gvEmptyContDatePending.DataBind();
        gvEmptyContDatePending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
        mpeJobDetail.Show();
    }
    protected void gvEmptyContDatePending_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvEmptyContDatePending_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().ToString().Trim() == "select")
        {
            int JobId = Convert.ToInt32(e.CommandArgument.ToString());
            if (JobId > 0)
            {
                Session["JobId"] = JobId.ToString();
                Response.Redirect("MovementComplete.aspx");
            }
        }
    }
    protected void gvEmptyContDatePending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[17].ToolTip = "Today's date - Job processed date";
        }
    }
    #endregion

    #region Container Received At Yard Date Pending - More than 3 days 
    protected void lnkbtnContCFSRecdPending_Click(object sender, EventArgs e)
    {
        string strFileName = "ContRecdCFSPending" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gvContCFSRecdPending.AllowPaging = false;
        gvContCFSRecdPending.AllowSorting = false;
        gvContCFSRecdPending.Caption = "Container Received Yard Date Pending (More than 3 days) On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        DataFilter1.FilterSessionID = "CDashboard.aspx";
        DataFilter1.FilterDataSource();
        gvContCFSRecdPending.DataBind();
        gvContCFSRecdPending.Columns[1].Visible = false;
        gvContCFSRecdPending.Columns[2].Visible = true;
        gvContCFSRecdPending.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
        mpeJobDetail.Show();
    }
    protected void gvContCFSRecdPending_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "container")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            Session["JobId"] = commandArgs[0].ToString();
            lblUpdatedContForJob.Text = commandArgs[1].ToString();
            //gvContainerDetail.DataBind();
            //mpeJobDetail.Hide();
            //mpeUpdatedCont.Show();
            Response.Redirect("ContainerDetail.aspx");
        }
    }
    protected void gvContCFSRecdPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[19].ToolTip = "Todays date - movement completed date";
            LinkButton lnkbtnContRecdCFSDate = (LinkButton)e.Row.FindControl("lnkbtnContRecdCFSDate");
            if (lnkbtnContRecdCFSDate != null)
            {
                int TotalCont = 0, UpdatedCont = 0, DeliveryType = 0;
                TotalCont = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalContainers"));
                UpdatedCont = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CFSContainerCount"));
                DeliveryType = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DeliveryType"));

                if (DeliveryType == 2)     // de-stuff delivery type
                {
                    e.Row.Cells[2].Text = TotalCont + "/" + UpdatedCont;
                    lnkbtnContRecdCFSDate.Text = TotalCont + "/" + UpdatedCont;
                    lnkbtnContRecdCFSDate.ToolTip = "Total Container / Container Updated With Received At Yard Date";
                    //if (TotalCont == UpdatedCont)
                    //{
                    //    chkSelectJob.Visible = true;
                    //}
                }
                else
                {
                    e.Row.Cells[2].Text = "";
                    lnkbtnContRecdCFSDate.Text = "";
                    //chkSelectJob.Visible = true;
                }
            }
        }
    }
    protected void gvContCFSRecdPending_PreRender(object sender, EventArgs e)
    {

    }
    protected void gvContainerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "ContRecdAtCFSDate") == DBNull.Value)
            {
                e.Row.Cells[9].Text = "";
            }
        }
    }
    protected void imgEmailClose_Click(object sender, EventArgs e)
    {
        mpeUpdatedCont.Hide();
    }
    #endregion

    #region Job Detail Pop-Up
    protected void imgbtnCancelJobDetail_Click(object sender, ImageClickEventArgs e)
    {
        mpeJobDetail.Hide();
    }
    protected void lnkbtnJobDetail_Click(object sender, EventArgs e)
    {
        if (hdnParticularId.Value != "" && hdnParticularId.Value != "0")
        {
            int ParticularId = Convert.ToInt32(hdnParticularId.Value);
            if (ParticularId > 0)
            {
                string strFileName = "NoofJobInDetail" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.Charset = "";
                this.EnableViewState = false;
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvJobDetail.AllowPaging = false;
                gvJobDetail.AllowSorting = false;

                if (ParticularId == 1 || ParticularId == 2)      // Newly created jobs OR ETA within 4 days
                {
                    gvJobDetail.Columns[13].Visible = false;
                    gvJobDetail.Columns[14].Visible = false;
                    gvJobDetail.Columns[15].Visible = false;
                    gvJobDetail.Columns[16].Visible = false;
                    gvJobDetail.Columns[17].Visible = false;
                    gvJobDetail.Columns[18].Visible = false;
                    if (ParticularId == 2)
                        gvJobDetail.Columns[19].Visible = false;
                    else
                        gvJobDetail.Columns[19].Visible = true;
                }
                else if (ParticularId == 3) // Movement Completion Pending
                {
                    gvJobDetail.Columns[13].Visible = true;
                    gvJobDetail.Columns[14].Visible = true;
                    gvJobDetail.Columns[15].Visible = true;
                    gvJobDetail.Columns[16].Visible = false;
                    gvJobDetail.Columns[17].Visible = false;
                    gvJobDetail.Columns[18].Visible = false;
                    gvJobDetail.Columns[19].Visible = false;
                }
                else if (ParticularId == 4) // Empty Container Return Date Pending
                {
                    gvJobDetail.Columns[13].Visible = true;
                    gvJobDetail.Columns[14].Visible = true;
                    gvJobDetail.Columns[15].Visible = true;
                    gvJobDetail.Columns[16].Visible = true;
                    gvJobDetail.Columns[17].Visible = true;
                    gvJobDetail.Columns[18].Visible = false;
                    gvJobDetail.Columns[19].Visible = false;
                }
                else if (ParticularId == 5) // Container Received at yard Pending
                {
                    gvJobDetail.Columns[13].Visible = true;
                    gvJobDetail.Columns[14].Visible = true;
                    gvJobDetail.Columns[15].Visible = true;
                    gvJobDetail.Columns[16].Visible = true;
                    gvJobDetail.Columns[17].Visible = true;
                    gvJobDetail.Columns[18].Visible = true;
                    gvJobDetail.Columns[19].Visible = false;
                }
                else if (ParticularId == 6) // Billing Scrutiny Pending
                {
                    gvJobDetail.Columns[13].Visible = true;
                    gvJobDetail.Columns[14].Visible = true;
                    gvJobDetail.Columns[15].Visible = true;
                    gvJobDetail.Columns[16].Visible = true;
                    gvJobDetail.Columns[17].Visible = true;
                    gvJobDetail.Columns[18].Visible = true;
                    gvJobDetail.Columns[19].Visible = false;
                }

                gvJobDetail.Caption = "No Of Jobs Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                DataFilter1.FilterSessionID = "CDashboard.aspx";
                DataFilter1.FilterDataSource();
                gvJobDetail.DataBind();
                gvJobDetail.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.End();
                mpeJobDetail.Show();
            }
        }
    }
    protected void gvJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    #endregion

    #region Export To Excel In General Methods
    protected void RemoveControls(Control grid)
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
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
            //mpeJobDetail.Show();
        }
    }
    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "CDashboard.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
    #endregion
}