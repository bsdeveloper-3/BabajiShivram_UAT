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
using System.Net;

public partial class ContMovement_MovementDetail : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(GridViewDocument);
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Movement Detail";
        if (!IsPostBack)
        {
            if (gvMovementDetail.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Movement Detail!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceMovementDetail;
        DataFilter1.DataColumns = gvMovementDetail.Columns;
        DataFilter1.FilterSessionID = "MovementDetail.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvMovementDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
        }
    }

    protected void gvMovementDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().ToString().Trim() == "showdocs")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            hdnJobId.Value = commandArgs[0].ToString();
            lblJobRefNo.Text = commandArgs[1].ToString();
            mpeDocument.Show();
            GridViewDocument.DataBind();
        }
    }

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
            DataFilter1.FilterSessionID = "MovementDetail.aspx";
            DataFilter1.FilterDataSource();
            gvMovementDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Pop-up Events
    protected void GridViewDocument_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            mpeDocument.Show();
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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

    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        mpeDocument.Hide();
    }
    #endregion

    #region Export Events
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "MovementDetail" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportData("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void ExportData(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvMovementDetail.AllowPaging = false;
        gvMovementDetail.AllowSorting = false;
        gvMovementDetail.Columns[0].Visible = false;
        gvMovementDetail.Columns[1].Visible = false;
        gvMovementDetail.Caption = "Movement Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "MovementDetail.aspx";
        DataFilter1.FilterDataSource();
        gvMovementDetail.DataBind();
        gvMovementDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

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

    #region Process/UnProcess Job Events   
    protected void imgClose2_Click(object sender, ImageClickEventArgs e)
    {
        mpeUnProcess.Hide();
    }
    protected void btnUnProcessJob_Click(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() != "")
        {
            int JobId = 0, count = 0;
            if (gvUnProcessJobList != null && gvUnProcessJobList.Rows.Count > 0)
            {
                for (int i = 0; i < gvUnProcessJobList.Rows.Count; i++)
                {
                    Label lblJobId = (Label)gvUnProcessJobList.Rows[i].FindControl("lblJobId");
                    if (lblJobId != null && lblJobId.Text != "")
                    {
                        JobId = Convert.ToInt32(lblJobId.Text.Trim());
                    }

                    if (JobId > 0)
                    {
                        int Result = CMOperations.ProcessMovement(JobId, false, txtReason.Text.Trim(), loggedInUser.glUserId);
                        if (Result == 0)
                        {
                            count = 1;
                            int result_UnProcessJob = CMOperations.AddUnprocessJobHistory(JobId, false, txtReason.Text.Trim(), loggedInUser.glUserId);
                        }
                        else if (Result == 2)
                        {
                            lblError.Text = "Job already unprocessed..!!";
                            lblError.CssClass = "errorMsg";
                        }
                        else
                        {
                            lblError.Text = "Error while unprocessing job. Please try again later.";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                }
            }

            if (count == 1)
            {
                lblError.Text = "Successfully unprocessed jobs. Jobs moved to Movement UnProcessed tab.";
                lblError.CssClass = "success";
                gvMovementDetail.DataBind();
            }
        }
        else
        {
            mpeUnProcess.Show();
        }
    }
    protected void btnProcessJobs_Click(object sender, EventArgs e)
    {
        int JobId = 0, count = 0;
        if (gvMovementDetail.Rows.Count > 0)
        {
            for (int i = 0; i < gvMovementDetail.Rows.Count; i++)
            {
                CheckBox chkSelectJob = (CheckBox)gvMovementDetail.Rows[i].FindControl("chkSelectJob");
                JobId = Convert.ToInt32(gvMovementDetail.DataKeys[i].Value.ToString());

                if (chkSelectJob != null && JobId > 0)
                {
                    if (chkSelectJob.Checked)
                    {
                        int Result = CMOperations.ProcessMovement(JobId, true, "", loggedInUser.glUserId);
                        if (Result == 0)
                        {
                            count = 1;
                            int result_ProcessJob = CMOperations.AddUnprocessJobHistory(JobId, true, txtReason.Text.Trim(), loggedInUser.glUserId);
                        }
                        else if (Result == 2)
                        {
                            lblError.Text = "Job already processed..!!";
                            lblError.CssClass = "errorMsg";
                        }
                        else
                        {
                            lblError.Text = "Error while processing job. Please try again later.";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                }
            }

            if (count == 1)
            {
                lblError.Text = "Successfully processed job. Job moved to Movement Process tab.";
                lblError.CssClass = "success";
            }
        }
    }
    protected void btnUnProcessJobs_Click(object sender, EventArgs e)
    {
        int JobId = 0;
        string Customer = "", JobRefNo = "";
        if (gvMovementDetail.Rows.Count > 0)
        {
            for (int i = 0; i < gvMovementDetail.Rows.Count; i++)
            {
                CheckBox chkSelectJob = (CheckBox)gvMovementDetail.Rows[i].FindControl("chkSelectJob");
                if (chkSelectJob != null)
                {
                    if (chkSelectJob.Checked)
                    {
                        JobId = Convert.ToInt32(gvMovementDetail.DataKeys[i].Values[0].ToString());
                        JobRefNo = gvMovementDetail.Rows[i].Cells[3].Text.Trim().ToString();
                        Customer = gvMovementDetail.Rows[i].Cells[4].Text.Trim().ToString();

                        AddNewRow(JobId, JobRefNo, Customer);
                        mpeUnProcess.Show();
                    }
                }
            }
        }
    }
    protected void btnDeleteRow_Click(object sender, EventArgs e)
    {
        Button lb = (Button)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["UnProcessJobList"] != null)
        {
            DataTable dt = (DataTable)ViewState["UnProcessJobList"];
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Remove(dt.Rows[rowID]);
            }

            if (dt.Rows.Count == 0)
            {
                ViewState["UnProcessJobList"] = null;
            }
            else
            {
                ViewState["UnProcessJobList"] = dt;
            }
            gvUnProcessJobList.DataSource = dt;
            gvUnProcessJobList.DataBind();
        }
        SetPreviousData();
        mpeUnProcess.Show();
    }
    protected void AddNewRow(int JobId, string JobRefNo, string Customer)
    {
        int rowIndex = 0;
        if (ViewState["UnProcessJobList"] != null)
        {
            DataTable dt = (DataTable)ViewState["UnProcessJobList"];
            DataRow dr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblJobId = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[1].FindControl("lblJobId");
                    Label lblJobRefNo = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[3].FindControl("lblCustomer");

                    dr = dt.NewRow();
                    dr["RowNumber"] = i + 1;
                    dr["JobId"] = JobId.ToString();
                    dr["JobRefNo"] = JobRefNo;
                    dr["Customer"] = Customer;

                    dt.Rows[i - 1]["RowNumber"] = lblRowNumber.Text;
                    dt.Rows[i - 1]["JobId"] = lblJobId.Text;
                    dt.Rows[i - 1]["JobRefNo"] = lblJobRefNo.Text;
                    dt.Rows[i - 1]["Customer"] = lblCustomer.Text;
                    rowIndex++;
                }

                dt.Rows.Add(dr);
                ViewState["UnProcessJobList"] = dt;
                gvUnProcessJobList.DataSource = dt;
                gvUnProcessJobList.DataBind();
            }
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("JobId", typeof(string)));
            dt.Columns.Add(new DataColumn("JobRefNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer", typeof(string)));

            DataRow dr = null;
            dr = dt.NewRow();

            dr["RowNumber"] = 1;
            dr["JobId"] = JobId.ToString();
            dr["JobRefNo"] = JobRefNo;
            dr["Customer"] = Customer;

            dt.Rows.Add(dr);
            ViewState["UnProcessJobList"] = dt;
            gvUnProcessJobList.DataSource = dt;
            gvUnProcessJobList.DataBind();

            Label lblRowNumber = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
            Label lblJobId = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[1].FindControl("lblJobId");
            Label lblJobRefNo = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
            Label lblCustomer = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[3].FindControl("lblCustomer");

            lblJobId.Text = JobId.ToString();
            lblJobRefNo.Text = JobRefNo;
        }
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["UnProcessJobList"] != null)
        {
            DataTable dt = (DataTable)ViewState["UnProcessJobList"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lblRowNumber = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[0].FindControl("lblRowNumber");
                    Label lblJobId = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[1].FindControl("lblJobId");
                    Label lblJobRefNo = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[2].FindControl("lblJobRefNo");
                    Label lblCustomer = (Label)gvUnProcessJobList.Rows[rowIndex].Cells[3].FindControl("lblCustomer");

                    lblRowNumber.Text = dt.Rows[i]["RowNumber"].ToString();
                    lblJobId.Text = dt.Rows[i]["JobId"].ToString();
                    lblJobRefNo.Text = dt.Rows[i]["JobRefNo"].ToString();
                    lblCustomer.Text = dt.Rows[i]["Customer"].ToString();
                    rowIndex++;
                }
            }
        }
    }
    #endregion
}