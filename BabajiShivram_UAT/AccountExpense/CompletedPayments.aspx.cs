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
using System.Text;
using ClosedXML.Excel;

public partial class AccountExpense_CompletedPayments : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkexport1);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Completed Payments";
        if (!IsPostBack)
        {
            if (gvJobExpDetail.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Job Expense!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceJobExpenseDetails;
        DataFilter1.DataColumns = gvJobExpDetail.Columns;
        DataFilter1.FilterSessionID = "CompletedPayments.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                Response.Redirect("CompPaymentById.aspx");
            }
        }
    }

    protected void gvJobExpDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvJobExpDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Status") != DBNull.Value)
            {
                string Status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId").ToString());
                if (Status != "")
                {
                    if (Status.ToLower().Trim() == "approved") //approved request
                    {
                        e.Row.Cells[14].Text = "";
                    }
                }
                if (StatusId == 3)                        // Hold
                {
                    e.Row.BackColor = System.Drawing.Color.LightSalmon;
                    e.Row.ToolTip = "Holded fund request.";
                }
                else if (StatusId == 1)                  // Pending for approval
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    e.Row.ToolTip = "Request pending for approval.";
                }
                else if (StatusId == 2)                  // Approved request
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    e.Row.ToolTip = "Request approved.";
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                    e.Row.ToolTip = "Payment Completed";
                }
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
            DataFilter1.FilterSessionID = "CompletedPayments.aspx";
            DataFilter1.FilterDataSource();
            gvJobExpDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        //string strFilterValue = ((TextBox)DataFilter1.FindControl("txtColumnValue")).Text;
        //string strFilterName = ((DropDownList)DataFilter1.FindControl("ddlColumnName")).SelectedItem.Text.Trim();
        //if (strFilterValue != "" && strFilterName.ToLower().Trim() == "expense type")
        //{
        //    string strFileName = "FundDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        //    DataSet dsFundDetails = AccountExpense.GetReportByRequestType(strFilterValue);
        //    ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel", dsFundDetails.Tables[0]);
        //}
        //else
        //{
        //    lblError.Text = "Please select expense type to proceed further!!";
        //    lblError.CssClass = "errorMsg";
        //}
        string strFileName = "FundDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction1(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobExpDetail.AllowPaging = false;
        gvJobExpDetail.AllowSorting = false;
        gvJobExpDetail.Columns[2].Visible = true;
        gvJobExpDetail.Columns[1].Visible = false;
        gvJobExpDetail.Columns[15].Visible = false;
        gvJobExpDetail.Columns[16].Visible = false;
        gvJobExpDetail.Columns[17].Visible = false;
        gvJobExpDetail.Columns[18].Visible = false;
        gvJobExpDetail.Columns[19].Visible = false;
        gvJobExpDetail.Columns[20].Visible = false;
        gvJobExpDetail.Columns[21].Visible = false;
        gvJobExpDetail.Columns[22].Visible = false;
        gvJobExpDetail.Columns[23].Visible = false;
        gvJobExpDetail.Columns[24].Visible = false;
        gvJobExpDetail.Columns[25].Visible = false;
        gvJobExpDetail.Columns[26].Visible = false;
        gvJobExpDetail.Columns[27].Visible = false;
        gvJobExpDetail.Columns[28].Visible = false;
        gvJobExpDetail.Columns[29].Visible = false;
        gvJobExpDetail.Columns[30].Visible = false;
        gvJobExpDetail.Columns[31].Visible = false;
        gvJobExpDetail.Columns[32].Visible = false;
        gvJobExpDetail.Columns[33].Visible = false;
        gvJobExpDetail.Columns[34].Visible = false;
        gvJobExpDetail.Columns[35].Visible = false;
        gvJobExpDetail.Columns[36].Visible = false;
        gvJobExpDetail.Columns[37].Visible = false;
        gvJobExpDetail.Columns[38].Visible = false;
        gvJobExpDetail.Columns[39].Visible = false;
        gvJobExpDetail.Columns[40].Visible = false;

        gvJobExpDetail.Caption = "Fund Request Details On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "CompletedPayments.aspx";
        DataFilter1.FilterDataSource();
        gvJobExpDetail.DataBind();
        gvJobExpDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void ExportFunction(string header, string contentType)
    {
        int UserId = 0;
        if (Convert.ToString(Session["UserId"]) != null)
        {
            UserId = Convert.ToInt32(Convert.ToString(Session["UserId"]));
        }

        DataSet dsDutyPayments = AccountExpense.GetDutyPaymentReport(UserId);
        //DataSet dsStampDutyPayments = AccountExpense.GetStampDutyPaymentReport(UserId);

        DataTable dtDutyPayments = dsDutyPayments.Tables[0];
       // DataTable dtStampDutyPayments = dsStampDutyPayments.Tables[0];
        dtDutyPayments.TableName = "Payments Details";
       // dtStampDutyPayments.TableName = "Stamp Duty Payments";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtDutyPayments);
        //    wb.Worksheets.Add(dtStampDutyPayments);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=FundDetails_" + DateTime.Now.ToShortDateString().Replace(@"/", "_").Replace(" ", "_") + ".xls");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                memoryStream.WriteTo(Response.OutputStream);
                memoryStream.Close();
                Response.Flush();
                Response.End();
            }
        }
    }

    #endregion

    protected void lnkexport1_Click(object sender, EventArgs e)
    {
        string strFileName = "FundDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction1("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
}