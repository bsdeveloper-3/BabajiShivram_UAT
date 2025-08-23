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
using System.Collections.Generic;
using ClosedXML.Excel;


public partial class AccountExpense_PaymentRequests : System.Web.UI.Page
{
    LoginClass LoggedIn = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["TotalAmount"] = 0;

        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvJobExpDetail);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Payment Request Details";
        if (!IsPostBack)
        {
            if (gvJobExpDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Payment Request Details..!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceJobExpenseDetails;
        DataFilter1.DataColumns = gvJobExpDetail.Columns;
        DataFilter1.FilterSessionID = "PaymentRequests.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region DOCUMENT GRIDVIEW EVENT

    protected void gvDocuments_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    protected void imgbtnDoc_Click(object sender, EventArgs e)
    {
        mpeContractCopy.Hide();
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
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

    #region GridView Event

    protected void gvJobExpDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId").ToString());
                string strReason = DataBinder.Eval(e.Row.DataItem, "Reason").ToString();

                if (StatusId == 3) // Hold
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#87AFC7");    //.LightSalmon;
                    e.Row.ToolTip = "Holded fund request.";
                }
                else if (StatusId == 1) // Pending for approval
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    e.Row.ToolTip = "Request pending for approval.";
                }
                else if (StatusId == 6) // Rejected Job
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#E8ADAA");
                    //e.Row.ToolTip = "Rejected job Reason - "+ strReason + "";
                    e.Row.ToolTip = "Rejected Payment";
                }
                else                    // Approved request
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    e.Row.ToolTip = "Request approved.";
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "IsPaymentComplete") != DBNull.Value)
            {
                int PaymentComplete = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsPaymentComplete").ToString());

                if (PaymentComplete == 1)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#FFBCFF");
                    e.Row.ToolTip = "Payment Completed";
                }

            }

                // Total Amount Calculation
                Decimal Amount = Convert.ToDecimal(e.Row.Cells[7].Text);
            ViewState["TotalAmount"] = Convert.ToDecimal(ViewState["TotalAmount"]) + Convert.ToDecimal(Amount);
            lblTotalAmount.Text = Convert.ToString(ViewState["TotalAmount"]);
        }
    }

    protected void gvJobExpDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";      
        if (e.CommandName.ToLower() == "downloaddoc")
        {
            int PaymentId = Convert.ToInt32(e.CommandArgument.ToString());
            Session["PaymentId"] = PaymentId.ToString();
            gvDocuments.DataBind();
            mpeContractCopy.Show();
        }
        else if (e.CommandName.ToLower().Trim() == "addpayment")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int lid = Convert.ToInt32(commandArgs[0]);
            int status = Convert.ToInt32(commandArgs[1]);           
            if (lid != 0)
            {
                Session["PaymentId"] = lid.ToString();
                Session["StatusId"] = status.ToString();
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
            DataFilter1.FilterSessionID = "PaymentRequests.aspx";
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
        string strFileName = "PaymentRequests_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvJobExpDetail.AllowPaging = false;
        gvJobExpDetail.AllowSorting = false;
        //gvJobExpDetail.Columns[1].Visible = false;
        gvJobExpDetail.Caption = "Payment Request Details On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PaymentRequests.aspx";
        DataFilter1.FilterDataSource();
        gvJobExpDetail.DataBind();
        gvJobExpDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    //protected void ExportFunction(string header, string contentType)
    //{
    //    int UserId = 0;
    //    if (Convert.ToString(Session["UserId"]) != null)
    //    {
    //        UserId = Convert.ToInt32(Convert.ToString(Session["UserId"]));
    //    }

    //    DataSet dsDutyPayments = AccountExpense.GetDutyPaymentReport(UserId);
    //    DataSet dsStampDutyPayments = AccountExpense.GetStampDutyPaymentReport(UserId);

    //    DataTable dtDutyPayments = dsDutyPayments.Tables[0];
    //    DataTable dtStampDutyPayments = dsStampDutyPayments.Tables[0];
    //    dtDutyPayments.TableName = "Duty Payments";
    //    dtStampDutyPayments.TableName = "Stamp Duty Payments";
    //    using (XLWorkbook wb = new XLWorkbook())
    //    {
    //        wb.Worksheets.Add(dtDutyPayments);
    //        wb.Worksheets.Add(dtStampDutyPayments);
    //        Response.Clear();
    //        Response.Buffer = true;
    //        Response.Charset = "";
    //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //        Response.AddHeader("content-disposition", "attachment;filename=FundDetails_" + DateTime.Now.ToShortDateString().Replace(@"/", "_").Replace(" ", "_") + ".xls");
    //        using (MemoryStream memoryStream = new MemoryStream())
    //        {
    //            wb.SaveAs(memoryStream);
    //            byte[] bytes = memoryStream.ToArray();
    //            memoryStream.WriteTo(Response.OutputStream);
    //            memoryStream.Close();
    //            Response.Flush();
    //            Response.End();
    //        }
    //    }
    //}

    #endregion
}