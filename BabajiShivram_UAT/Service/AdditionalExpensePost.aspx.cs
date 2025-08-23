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
using System.Text;
public partial class Service_AdditionalExpensePost : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["JobId"] = null;
            Session["JobIdE"] = null;
            Session["ExpenseID"] = null;

            ScriptManager1.RegisterPostBackControl(lnkexport);

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Additional Expense - FA Posting";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Additional Expense Found For Booking!";
                lblMessage.CssClass = "errorMsg";
                DataFilter1.Visible = false;
            }
        }

        //
        DataFilter1.DataSource = PendingExpenseSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingJobExpensePost.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            Session["JobIdE"] = commandArgs[0].ToString();

            Session["ExpenseID"] = commandArgs[1].ToString();

            string URI = "ViewExpensesDetail.aspx";

            Response.Redirect(URI);
        }
        else if (e.CommandName.ToLower() == "viewbjv")
        {
            string strBJVNo = e.CommandArgument.ToString();

            DataSet dsBJVDetail = BillingOperation.FAGetJobExpenseBilled(strBJVNo);

            if (dsBJVDetail.Tables[0].Rows.Count > 0)
            {
                gvBJVDetail.DataSource = dsBJVDetail;
                gvBJVDetail.DataBind();
            }
            else
            {
                gvBJVDetail.DataSource = null;
                gvBJVDetail.DataBind();
            }

            ModalPopupExtender2.Show();
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
            DataFilter1.FilterSessionID = "PendingJobExpensePost.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }
    #endregion
    protected void btnPost_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr1 in gvJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chk1");

                if (chk1.Checked)
                {
                    int ApprovalStatusId = (Int32)AdditionalExpenseStatus.PaymentCompleted;
                    int ExpenseID = Convert.ToInt32(gvJobDetail.DataKeys[gvr1.RowIndex].Value);

                    int result = DBOperations.AddJobExpensesStatus(ExpenseID, ApprovalStatusId, "", LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Expense Booking Completed!";
                        lblMessage.CssClass = "success";                                                
                    }
                    else if (result == 1)
                    {
                        lblMessage.Text = "System Error! Please try after sometime.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else if (result == 2)
                    {
                        lblMessage.Text = "Expense Already Booked!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }

        }

        // No Direct FA Posting Exe call from Application
        // Schedule in Sql Agent Job - InvoicePosting
        // int ExeResult10 = AccountExpense.FAPostAdditionalExp();

        PendingExpenseSqlDataSource.DataBind();

    }

    #region View BJV
    protected void gvBJVDetail_PreRender(object sender, EventArgs e)
    {
        if (gvBJVDetail.Rows.Count > 1)
        {
            GridViewRow getRow = gvBJVDetail.Rows[gvBJVDetail.Rows.Count - 1];
            getRow.Cells[7].BackColor = System.Drawing.Color.Yellow;
            getRow.Cells[8].BackColor = System.Drawing.Color.Green;

            decimal decDebit = 0m;
            decimal decCredit = 0m;
            decimal decProfit = 0m;

            Decimal.TryParse(getRow.Cells[7].Text.Trim(), out decDebit);
            Decimal.TryParse(getRow.Cells[8].Text.Trim(), out decCredit);

            decProfit = (decCredit - decDebit);


            if (decProfit <= 0)
            {
                getRow.Cells[8].BackColor = System.Drawing.Color.MediumVioletRed;
                getRow.Cells[9].Text = "Loss: " + decProfit.ToString();
            }
            else
            {
                getRow.Cells[9].Text = "Profit: " + decProfit.ToString();
            }

        }
    }
    protected void btnCancelBJVPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }
    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Additional_Expense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.Columns[3].Visible = false;
        gvJobDetail.Columns[4].Visible = true;

        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}