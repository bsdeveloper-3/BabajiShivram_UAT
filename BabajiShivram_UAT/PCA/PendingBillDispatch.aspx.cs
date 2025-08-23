using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Syncfusion.Licensing;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Syncfusion.XlsIO;
public partial class PCA_PendingBillDispatch : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkreceive);
        ScriptManager1.RegisterPostBackControl(btnExportBillDetail);
        ScriptManager1.RegisterPostBackControl(btnCoveringLetter);
        ScriptManager1.RegisterPostBackControl(gvRecievedJobDetail);

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;
            Session["CoverJobIdList"] = null;
            Session["CoverCustomerId"] = null;

            Session["DispatchJobIdList"] = null;
            Session["DispatchCustomerId"] = null;

            Session["BillJobIdList"] = null;
            Session["BillJobId"] = null;
            Session["CoverCustomerId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Dispatch";
            
        }

        DataFilter2.DataSource = SqlDataSourceCustomer;
        DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
        DataFilter2.FilterSessionID = "PendingBillDispatch.aspx";
        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
    }
    protected void gvNonRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
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
            DataFilter2_OnDataBound();
        }
    }
    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "PendingBillDispatch.aspx";
            DataFilter2. FilterDataSource();
            gvRecievedJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName.ToLower() == "jobselect")
        {
            Session["BillJobId"] = e.CommandArgument.ToString();

            Response.Redirect("BillDispatch.aspx");
        }
        else if (e.CommandName.ToLower().Trim() == "hold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobRefNo = "";
            int JobId = 0, JobType = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strJobRefNo = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                JobType = Convert.ToInt32(commandArgs[2].ToString());

            if (JobId != 0)
            {
                lblMessage.Text = "";
                hdnJobId.Value = JobId.ToString();

                fvHoldJobDetail.DataSource = DBOperations.GetJobDetail(JobId);
                fvHoldJobDetail.DataBind();

                mpeHoldExpense.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "unhold")
        {
            int JobId = Convert.ToInt32(e.CommandArgument.ToString());

            if (JobId > 0)
            {
                int result = DBOperations.AddHoldBillingAdvice(JobId, "", "", LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblMessage.Text = "Job Successfully unholded!";
                    lblMessage.CssClass = "success";
                }
                else
                {
                    lblMessage.Text = "System error. Please try again later.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void gvRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Hold Status
            if (DataBinder.Eval(e.Row.DataItem, "HoldStatus") != DBNull.Value)
            {
                ImageButton imgbtnHoldJob = (ImageButton)e.Row.FindControl("imgbtnHoldJob");
                ImageButton imgbtnUnholdJob = (ImageButton)e.Row.FindControl("imgbtnUnholdJob");
                CheckBox chkjoball = (CheckBox)e.Row.FindControl("chkjoball");

                string HoldStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "HoldStatus"));
                if (HoldStatus.ToLower().Trim() == "hold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = true;
                        imgbtnUnholdJob.Visible = false;
                        chkjoball.Visible = true;
                    }
                }
                else if (HoldStatus.ToLower().Trim() == "unhold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = false;
                        imgbtnUnholdJob.Visible = true;
                        chkjoball.Visible = false;
                        chkjoball.Enabled = false;
                    }
                }
            }
        }
    }

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        int JobId = 0;
        if (hdnJobId.Value != "")
        {
            JobId = Convert.ToInt32(hdnJobId.Value);
        }

        if (JobId > 0)
        {
            if (txtHoldReason.Text != "")
            {
                int result = DBOperations.AddHoldBillingAdvice(JobId, txtHoldReason.Text.Trim(), "", LoggedInUser.glUserId);
                if (result == 0)
                {
                    fvHoldJobDetail.DataBind();
                    mpeHoldExpense.Hide();
                    lblMessage.Text = "Bill Dispatch Successfully On Hold";
                    lblMessage.CssClass = "success";
                }
                else
                {
                    lblMessage.Text = "System error. Please try again later.";
                    lblMessage.CssClass = "errorMsg";
                }
            }
            else
            {
                lblMessage.Text = "Please Enter Hold Reason.";
                lblMessage.CssClass = "errorMsg";
                mpeHoldExpense.Show();
            }
        }
    }
    protected void btnCoveringLetter_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        string strCustomerId = "";

        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    strJobidList    = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";

                    if (strCustomerId == "")
                    {
                        strCustomerId = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString();
                    }
                    else if(strCustomerId != gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString())
                    {
                        // Different Customer Selected For Covering Letter Generation
                        lblMessage.Text = "Please Select Job for One Customer Only!";
                        lblMessage.CssClass = "errorMsg";

                       // break;
                    }
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for Covering Letter.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["CoverJobIdList"] = strJobidList;
            Session["CoverCustomerId"] = strCustomerId;

            Response.Redirect("CoverDispatchList.aspx");

        }
    }
    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    protected void btnEBill_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString() + ",";
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for E-Bill";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["BillJobIdList"] = strJobidList;

            Response.Redirect("BillDispatchList.aspx");

        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        bool bApprove = true;
        int reasonforPendency = 0;
        
        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    int jobid = Convert.ToInt32(gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value);

                    int result = 1;// BillingOperation.ApproveRejectDispatching(jobid, bApprove, "", reasonforPendency, "", 0, LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Bill Dispatch Completed! Job Moved To Dispatch Department!.";
                        lblMessage.CssClass = "success";
                        //---------------------start Covering letter-------------------
                        //gvRecievedJobDetail.DataBind();
                        gvRecievedJobDetail.DataBind();
                        //---------------------end Covering letter---------------------
                    }
                    else if (result == 1)
                    {
                        lblMessage.Text = "System Error! Please try after sometime.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else if (result == 2)
                    {
                        lblMessage.Text = "Bill Dispatch Already Completed!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
    }

    protected void btnMyPaccoAWBGeneration_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        string strCustomerId = "";

        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";

                    if (strCustomerId == "")
                    {
                        strCustomerId = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString();
                    }
                    else if (strCustomerId != gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString())
                    {
                        // Different Customer Selected For Covering Letter Generation
                        lblMessage.Text = "Please Select Job for One Customer Only!";
                        lblMessage.CssClass = "errorMsg";

                        //break;
                    }
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for MyPacco Dispatch.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["DispatchJobIdList"] = strJobidList;
            Session["DispatchCustomerId"] = strCustomerId;

            Response.Redirect("MyPaccoDispatchList.aspx");

        }
    }

    #region Export To Excel
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkreceive_Click(object sender, EventArgs e)
    {
        string strFileName = "Bill_Dispatch_Pending" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ReceivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ReceivejoblistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvRecievedJobDetail.AllowPaging = false;
        gvRecievedJobDetail.AllowSorting = false;

        gvRecievedJobDetail.Columns[1].Visible = false;
        gvRecievedJobDetail.Columns[2].Visible = false;
        gvRecievedJobDetail.Columns[3].Visible = false;
        gvRecievedJobDetail.Columns[4].Visible = true;

        gvRecievedJobDetail.DataSourceID = "SqlDataSourceCustomer";
        gvRecievedJobDetail.DataBind();

        //Remove Controls
        //this.RemoveControls(gvRecievedJobDetail);

        gvRecievedJobDetail.RenderControl(hw);

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

    #region Export To Excel Bill Detail

    protected void btnExportBillDetail_Click(object sender, EventArgs e)
    {
        ExportToExcelBillDetail();
    }
    private void ExportToExcelBillDetail()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense("MDAxQDMxMzkyZTMyMmUzMEw0YkNCVGlYWER5dmVWYmUwV0RMc1dvMzlISmN0VTRLK3VRY2JoQ1VrTG89");

        DataSet dsBillDetail = BillingOperation.GetPendingBillDispatchForExcel(LoggedInUser.glUserId, LoggedInUser.glFinYearId);

        string strOutputFile = "Pending_Bill_" + DateTime.Now.ToString("dd-MM hh-mm") + ".xlsx";

        if (dsBillDetail.Tables.Count > 0)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a new workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet sheet = workbook.Worksheets[0];
                sheet.Name = "Pending Bill Dispatch";

                if (sheet.ListObjects.Count == 0)
                {
                    sheet.ImportDataTable(dsBillDetail.Tables[0], true, 1, 1);
                }

                //Refresh Excel table to get updated values from database
                // sheet.ListObjects[0].Refresh();

                sheet.UsedRange.AutofitColumns();

                workbook.SaveAs(strOutputFile, ExcelSaveType.SaveAsXLS, Response);
            }
        }
        else
        {
            lblMessage.Text = "Pending Bill Dispatch Details Not Found";
            lblMessage.CssClass = "errorMsg";
        }

    }

    #endregion
}