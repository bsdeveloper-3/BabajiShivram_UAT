using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class Transport_VehiclePlaced : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(gvJobDetail);
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(lnkExport_Consolidate);
        if (!IsPostBack)
        {
            Session["TRId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Vehicle placed";

            //if (gvConsolidateJobs.Rows.Count == 0)
            //{
            //    lblError_ConsolidateJob.Text = "No  jobs found for consolidate job detail.";
            //    lblError_ConsolidateJob.CssClass = "errorMsg";
            //    pnlFilter2.Visible = false;
            //}
            DataFilter1.DataSource = TruckRequestSqlDataSource;
            DataFilter1.DataColumns = gvJobDetail.Columns;
            DataFilter1.FilterSessionID = "RequestReceived.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            DataFilter2.DataSource = DataSourceConsolidateVehicle;
            DataFilter2.DataColumns = gvConsolidateJobDetail.Columns;
            DataFilter2.FilterSessionID = "RequestReceived.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        }
        if (TabPanelNormalJob.TabIndex == 0)
        {
            if (TabRequestRecd.ActiveTabIndex == 0)
            {
                lblError_Job.Text = "";
                DataFilter1.DataSource = TruckRequestSqlDataSource;
                DataFilter1.DataColumns = gvJobDetail.Columns;
                DataFilter1.FilterSessionID = "RequestReceived.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            }
        }
        if (TabPanelConsolidateJob.TabIndex == 1)
        {
            if (TabRequestRecd.ActiveTabIndex == 1)
            {
                lblError_ConsolidateJob.Text = "";
                DataFilter2.DataSource = DataSourceConsolidateVehicle;
                DataFilter2.DataColumns = gvConsolidateJobDetail.Columns;
                DataFilter2.FilterSessionID = "RequestReceived.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);

            }
        }
    }

    protected void btnVehiclePlaced_Click(object sender, EventArgs e)
    {
        int result = 0;
        String TransportId = "";
        string strTRIdList = "";
        int DocCount = 0;

        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TransportId = gvJobDetail.DataKeys[row.RowIndex].Value.ToString();

                    CheckBox chkSelect = (row.FindControl("chkSelect") as CheckBox);

                    if (chkSelect.Checked)
                    {
                        strTRIdList += TransportId + ",";

                        DocCount += 1;
                    }//END_IF
                }//END_IF
            }//END_IF
        }//END_ForEarch

        // IF No Error 
        if (DocCount > 0)
        {
            result = DBOperations.AddVehiclePlaced(strTRIdList, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError_Job.Text = "Details Updated Successfully!";
                lblError_Job.CssClass = "success";
            }
            if (result == 1)
            {
                lblError_Job.Text = "System Error! Please try after sometime.";
                lblError_Job.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError_Job.Text = "Please Select Job For Status Update!";
            lblError_Job.CssClass = "errorMsg";
        }
    }

    protected void btnConsolidate_Click(object sender, EventArgs e)
    {
        int TransReqId = 0, JobCount = 0, result = 0;
        string strJobIdList = "", TRRefNo = "", JobRefNo = "", Customer = "", Location = "", Destination = "", JobType = "", JobType_Loc = "", DeliveryType = "", ContainerNo_Loc = "", ContainerNo = "";
        DBOperations.FillCompanyByCategory(ddlTransporter, Convert.ToInt32(EnumCompanyType.Transporter));

        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (JobType == "")
                {
                    JobType = row.Cells[7].Text.Trim();
                }

                JobType_Loc = row.Cells[7].Text.Trim();
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                HiddenField hdnContainerNo = (HiddenField)row.FindControl("hdnContainerNo");
                if (chkSelect.Checked)
                {
                    if (JobType.ToLower().Trim() == JobType_Loc.ToLower().Trim())
                    {
                        // check same container exists if loaded job
                        DeliveryType = row.Cells[15].Text;
                        if (DeliveryType.ToLower().Trim() == "loaded")
                        {
                            if (ContainerNo == "")
                            {
                                ContainerNo = hdnContainerNo.Value;
                            }
                            ContainerNo_Loc = hdnContainerNo.Value;
                            if (ContainerNo.ToLower().Trim() == ContainerNo_Loc.ToLower().Trim())
                            {
                                JobCount++;
                            }
                            else
                            {
                                JobCount = -2;
                                break;
                            }
                        }

                        else
                        {
                            JobCount++;
                        }
                    }
                    else
                    {
                        JobCount = -1;
                        break;
                    }
                }
            }
        }

        if (JobCount == 0)
        {
            lblMessage.Text = "Please select atleast one truck request!";
            lblMessage.CssClass = "errorMsg";
        }
        else if (JobCount == -1)
        {
            lblMessage.Text = "Please select jobs of same job type!";
            lblMessage.CssClass = "errorMsg";
        }
        else if (JobCount == -2)
        {
            lblMessage.Text = "Containers do not match for selected jobs! Failed to create consolidate job.";
            lblMessage.CssClass = "errorMsg";
        }
        else if (JobCount == 1)
        {
            lblMessage.Text = "Please select more than one job for consolidate truck request!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            lblMessage.Text = "";
            foreach (GridViewRow row in gvJobDetail.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TransReqId = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value.ToString());
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    if (chkSelect.Checked)
                    {
                        TRRefNo = ((LinkButton)row.FindControl("lnkRefNo")).Text.Trim();
                        Customer = row.Cells[5].Text;
                        JobRefNo = row.Cells[4].Text;
                        Location = row.Cells[8].Text;
                        Destination = row.Cells[9].Text;
                        AddNewRow(TransReqId, TRRefNo, JobRefNo, Customer, Location, Destination);
                    }
                }
            }
            mpeConsolidateJob.Show();
        }
    }

    #region TAB PANEL 1 => Request Job Detail

    #region GridView Event

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((DataBinder.Eval(e.Row.DataItem, "JobTypeId")) != DBNull.Value)
            {
                int JobTypeId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "JobTypeId"));
                if (JobTypeId == 3) // new transport job
                {
                    e.Row.Cells[1].Text = "";
                }
            }

            if ((DataBinder.Eval(e.Row.DataItem, "IsVehiclePlaced")) != DBNull.Value)
            {
                bool IsVehiclePlaced = (bool)(DataBinder.Eval(e.Row.DataItem, "IsVehiclePlaced"));
                if (IsVehiclePlaced == true)
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.BackColor = System.Drawing.Color.FromName("#2266aa63"); // #2266bb
                    e.Row.ToolTip = "Vehicle Placed";
                }
            }

            if ((DataBinder.Eval(e.Row.DataItem, "DeliveryStatus")) == DBNull.Value) //planning not completed
            {
                e.Row.Cells[1].Text = "";
                e.Row.Cells[25].Text = "N";
                e.Row.ToolTip = "Planning not completed yet";
            }
            else
            {
                e.Row.Cells[25].Text = "Y";
            }

            //if ((DataBinder.Eval(e.Row.DataItem, "VehiclesUpdated")) != DBNull.Value)
            //{
            //    int Vehicles = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VehiclesUpdated"));
            //    if (Vehicles > 0)
            //    {
            //        e.Row.Cells[1].Text = "";
            //        e.Row.BackColor = System.Drawing.Color.FromName("#2266aa63"); // #2266bb
            //        e.Row.ToolTip = "Vehicle Placed";
            //    }
            //}

            //if ((DataBinder.Eval(e.Row.DataItem, "DeliveryType")) != DBNull.Value)
            //{
            //    int CountLCL = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CountLCL"));
            //    string strLoaded = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DeliveryType"));
            //    if (strLoaded.ToLower().Trim() == "loaded") //&& CountLCL == 1)
            //    {
            //        e.Row.Cells[1].Text = "";
            //    }
            //}

            //if ((DataBinder.Eval(e.Row.DataItem, "DeliveryStatus")) != DBNull.Value)
            //{
            //    bool bDeliveryStatus = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DeliveryStatus"));
            //    if (bDeliveryStatus == true)
            //    {
            //        e.Row.Cells[1].Text = "";
            //    }
            //}

            //bool IsVehiclePlaced = false;
            ////if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ContDispatched")) > 0)
            ////{
            ////    e.Row.Cells[7].ToolTip = "Number of vehicles dispatched";
            ////}

            ////if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VehiclesDispatched")) > 0)
            ////{
            ////    e.Row.Cells[7].ToolTip = "Number of vehicles dispatched";
            ////}

        }
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
        int JobTypeId = 0;

        Session["TRId"] = commandArgs[0].ToString();
        JobTypeId = Convert.ToInt32(commandArgs[1].ToString());

        if (JobTypeId == 3) // new transport jobs
        {
            Response.Redirect("NewTransVehicleDetail.aspx");
        }
        else
        {
            Response.Redirect("VehiclePlace.aspx?Page=VehiclePlace");
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

    #region Consolidate Jobs
    protected void imgbtnClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeConsolidateJob.Hide();
        gvJobDetail.DataBind();
        ViewState["ConsolidateJobs"] = null;
    }
    protected void btnDeleteRow_Click(object sender, EventArgs e)
    {
        Button lb = (Button)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowID]);
            }

            if (dt.Rows.Count == 0)
            {
                ViewState["ConsolidateJobs"] = null;
            }
            else
            {
                ViewState["ConsolidateJobs"] = dt;
            }
            gvAddConsolidateJobDetail.DataSource = dt;
            gvAddConsolidateJobDetail.DataBind();
        }
        SetPreviousData();
        mpeConsolidateJob.Show();
    }
    protected void AddNewRow(int TransReqId, string TRRefNo, string JobRefNo, string Customer, string Location, string Destination)
    {
        int rowIndex = 0;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            DataRow dr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblTransReqId = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[1].FindControl("lblTransReqId");
                    Label lblTransRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[2].FindControl("lblTransRefNo");
                    Label lblJobRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[3].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[4].FindControl("lblCustomer");
                    Label lblLocation = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[5].FindControl("lblLocation");
                    Label lblDestination = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[6].FindControl("lblDestination");

                    dr = dt.NewRow();
                    dr["RowNumber"] = i + 1;
                    dr["TransReqId"] = TransReqId.ToString();
                    dr["TransRefNo"] = TRRefNo;
                    dr["JobRefNo"] = JobRefNo;
                    dr["Customer"] = Customer;
                    dr["Location"] = Location;
                    dr["Destination"] = Destination;

                    dt.Rows[i - 1]["RowNumber"] = lblRowNumber.Text;
                    dt.Rows[i - 1]["TransReqId"] = lblTransReqId.Text;
                    dt.Rows[i - 1]["TransRefNo"] = lblTransRefNo.Text;
                    dt.Rows[i - 1]["JobRefNo"] = lblJobRefNo.Text;
                    dt.Rows[i - 1]["Customer"] = lblCustomer.Text;
                    dt.Rows[i - 1]["Location"] = lblLocation.Text;
                    dt.Rows[i - 1]["Destination"] = lblDestination.Text;
                    rowIndex++;
                }

                dt.Rows.Add(dr);
                ViewState["ConsolidateJobs"] = dt;
                gvAddConsolidateJobDetail.DataSource = dt;
                gvAddConsolidateJobDetail.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("TransReqId", typeof(string)));
            dt.Columns.Add(new DataColumn("TransRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("JobRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer", typeof(string)));
            dt.Columns.Add(new DataColumn("Location", typeof(string)));
            dt.Columns.Add(new DataColumn("Destination", typeof(string)));

            DataRow dr = null;
            dr = dt.NewRow();

            dr["RowNumber"] = 1;
            dr["TransReqId"] = TransReqId.ToString();
            dr["TransRefNo"] = TRRefNo;
            dr["JobRefNo"] = JobRefNo;
            dr["Customer"] = Customer;
            dr["Location"] = Location;
            dr["Destination"] = Destination;

            dt.Rows.Add(dr);
            ViewState["ConsolidateJobs"] = dt;
            gvAddConsolidateJobDetail.DataSource = dt;
            gvAddConsolidateJobDetail.DataBind();

            Label lblRowNumber = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
            Label lblTransReqId = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[1].FindControl("lblTransReqId");
            Label lblTransRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[2].FindControl("lblTransRefNo");
            Label lblJobRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[3].FindControl("lblJobRefNo");
            Label lblCustomer = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[4].FindControl("lblCustomer");
            Label lblLocation = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[5].FindControl("lblLocation");
            Label lblDestination = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[6].FindControl("lblDestination");

            lblTransReqId.Text = TransReqId.ToString();
            lblTransRefNo.Text = TRRefNo.ToString();
            lblJobRefNo.Text = JobRefNo.ToString();
            lblCustomer.Text = Customer.ToString();
            lblLocation.Text = Location.ToString();
            lblDestination.Text = Destination.ToString();
        }
        SetPreviousData();
    }
    protected void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["ConsolidateJobs"] != null)
        {
            DataTable dt = (DataTable)ViewState["ConsolidateJobs"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblTransReqId = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[1].FindControl("lblTransReqId");
                    Label lblTransRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[2].FindControl("lblTransRefNo");
                    Label lblJobRefNo = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[3].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[4].FindControl("lblCustomer");
                    Label lblLocation = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[5].FindControl("lblLocation");
                    Label lblDestination = (Label)gvAddConsolidateJobDetail.Rows[rowIndex].Cells[6].FindControl("lblDestination");

                    lblRowNumber.Text = dt.Rows[i]["RowNumber"].ToString();
                    lblTransReqId.Text = dt.Rows[i]["TransReqId"].ToString();
                    lblTransRefNo.Text = dt.Rows[i]["TransRefNo"].ToString();
                    lblJobRefNo.Text = dt.Rows[i]["JobRefNo"].ToString();
                    lblCustomer.Text = dt.Rows[i]["Customer"].ToString();
                    lblLocation.Text = dt.Rows[i]["Location"].ToString();
                    lblDestination.Text = dt.Rows[i]["Destination"].ToString();
                    rowIndex++;
                }
            }
        }
    }
    #endregion

    #endregion

    #region TAB PANEL 2 => Consolidate Job Detail

    protected void btnAddConsolidate_Click(object sender, EventArgs e)
    {
        if (ddlTransporter.SelectedValue != "0")
        {
            mpeConsolidateJob.Show();
            string TransRefNo = "";
            int JobResult = 0;

            TransRefNo = DBOperations.GetConsolidateRefNo(Convert.ToInt32(Session["FinYearId"]));
            if (gvAddConsolidateJobDetail.Rows.Count > 0 && TransRefNo != "")
            {
                // Add consolidate request
                int ConsolidateRequestId = DBOperations.AddConsolidateRequest(TransRefNo, Convert.ToInt32(ddlTransporter.SelectedValue), Convert.ToInt32(Session["FinYearId"]), LoggedInUser.glUserId);
                if (ConsolidateRequestId > 0)
                {
                    for (int i = 0; i < gvAddConsolidateJobDetail.Rows.Count; i++)
                    {
                        Label lblTransReqId = (Label)gvAddConsolidateJobDetail.Rows[i].FindControl("lblTransReqId");
                        if (lblTransReqId.Text.Trim() != "")
                        {
                            // Update consolidate request id in TR_TransportMS table (w.r.to alle of these consolidate jobs)
                            int AddConsolidateJob = DBOperations.UpdateConsolidateJob(Convert.ToInt32(lblTransReqId.Text.Trim()), ConsolidateRequestId, LoggedInUser.glUserId);
                            if (AddConsolidateJob == 0)
                            {
                                JobResult++;
                            }
                            else
                            {
                                break;
                            }
                            // Add consolidate job details
                            //int ConsolidateJob = DBOperations.AddConsolidateJobDetail(ConsolidateRequestId, Convert.ToInt32(lblTransReqId.Text.Trim()), LoggedInUser.glUserId);
                            //if (ConsolidateJob == 0)
                            //{
                            //    JobResult++;
                            //}
                            //else
                            //{
                            //    break;
                            //}
                        }
                    }

                    if (JobResult > 0)
                    {
                        lblMessage.Text = "Successfully created consolidate job.";
                        lblMessage.CssClass = "success";
                        gvJobDetail.DataBind();
                        //gvConsolidateJobs.DataBind();
                        mpeConsolidateJob.Hide();
                    }
                    else
                    {
                        lblError_Popup.Text = "Error while creating consolidate job.";
                        lblError_Popup.CssClass = "errorMsg";
                    }
                }
                else if (ConsolidateRequestId == -1)
                {
                    lblError_Popup.Text = "System error while adding job. Please try again later.";
                    lblError_Popup.CssClass = "errorMsg";
                }
                else
                {
                    lblError_Popup.Text = "Consolidate job already exists!";
                    lblError_Popup.CssClass = "errorMsg";
                }
            }
        }
        else
        {
            lblError_Popup.Text = "NOTE : Please Select Transporter!";
            lblError_Popup.CssClass = "errorMsg";
            mpeConsolidateJob.Show();
        }
    }

    protected void gvConsolidateJobs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            Session["TRId"] = e.CommandArgument.ToString();
            if (Session["TRId"] != null)
            {
                Response.Redirect("ConsolidateRequest.aspx");
            }
        }
    }

    protected void gvConsolidateJobs_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvConsolidateJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((DataBinder.Eval(e.Row.DataItem, "VehicleNo")) != DBNull.Value)
            {
                e.Row.BackColor = System.Drawing.Color.FromName("#2266aa63"); // #2266bb
                e.Row.ToolTip = "Vehicle Placed";
            }
        }
    }

    protected void gvConsolidateJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            Session["TRConsolidateId"] = commandArgs[0].ToString();
            Session["TRId"] = Convert.ToInt32(commandArgs[1].ToString());

            if (Session["TRId"] != null)
            {
                Response.Redirect("ConsolidateRequest.aspx");
            }
        }
    }

    protected void gvConsolidateJobDetail_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "RequestReceived.aspx";
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

    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "RequestReceived.aspx";
            DataFilter2.DataColumns = gvConsolidateJobDetail.Columns;
            DataFilter2.FilterDataSource();
            gvConsolidateJobDetail.DataBind();
            if (gvConsolidateJobDetail.Rows.Count == 0)
            {
                lblError_ConsolidateJob.Text = "No Job Found For Consolidate Job Detail!";
                lblError_ConsolidateJob.CssClass = "errorMsg";
            }
            else
            {
                lblError_ConsolidateJob.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "NormalJobRequest_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
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
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = false;
        gvJobDetail.Columns[3].Visible = true;
        gvJobDetail.Caption = "Request Received On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "RequestReceived.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkExport_Consolidate_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "ConsolidateJobRequest_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction_Consolidate("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ExportFunction_Consolidate(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvConsolidateJobDetail.AllowPaging = false;
        gvConsolidateJobDetail.AllowSorting = false;
        gvConsolidateJobDetail.Columns[1].Visible = false;
        gvConsolidateJobDetail.Caption = "Request Received On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter2.FilterSessionID = "RequestReceived.aspx";
        DataFilter2.FilterDataSource();
        gvConsolidateJobDetail.DataBind();
        gvConsolidateJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}