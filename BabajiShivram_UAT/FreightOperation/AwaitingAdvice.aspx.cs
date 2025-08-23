using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class FreightOperation_AwaitingAdvice : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    const int FIRST_EDITABLE_CELL = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["EnqId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Awaiting Billing Advice";
        }
        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = gvFreight.Columns;
        DataFilter1.FilterSessionID = "AwaitingAdvice.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
        if (Request.QueryString["mess"] != null)
        {
            Response.Write("<script>alert('" + Request.QueryString["mess"] + "');</script>");
        }
    }

    protected void gvFreight_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            //string strEnqId = (string)e.CommandArgument;
            string strEnqId = args[0];
            string strJobType = args[1];
            string strJobMode = args[2];
            Session["JobType"] = strJobType;
            Session["EnqId"] = strEnqId;
            Session["JobMode"] = strJobMode;

            Response.Redirect("BillingAdvice.aspx");
        }
        else if (e.CommandName.ToLower().Trim() == "hold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strAmount = "", strJobRefNo = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strAmount = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                strJobRefNo = commandArgs[2].ToString();

            if (JobId != 0)
            {
                txtReason.Text = "";
                lblError_HoldExp.Text = "";
                Session["JobId"] = commandArgs[0].ToString();
                hdnJobId.Value = JobId.ToString();

                fvHoldJobDetail.DataBind();
                fvHoldJobDetail.Visible = true;
                Div3.Visible = true;
                hdnJobRefNo.Value = strJobRefNo;

                lblHoldPopupName.Text = "Hold Job";
                btnHoldJob.Text = "Hold Job";
                Label lblAmount = (Label)fvHoldJobDetail.FindControl("lblAmount");
                if (lblAmount != null)
                {
                    lblAmount.Text = strAmount.ToString();
                }
                mpeHoldExpense.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "unhold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strAmount = "", strJobRefNo = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strAmount = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                strJobRefNo = commandArgs[2].ToString();

            if (JobId != 0)
            {
                if (JobId != 0)
                {
                    int result = DBOperations.FR_AddHoldBillingAdvice(JobId, "", "", LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        fvHoldJobDetail.DataBind();
                        lblMessage.Text = "Successfully unholded job no " + strJobRefNo + ".";
                        lblMessage.CssClass = "success";
                        gvFreight.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System error. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
    }

    protected void gvFreight_PreRender(object sender, EventArgs e)
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
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "AwaitingAdvice.aspx";
            DataFilter1.FilterDataSource();
            gvFreight.DataBind();
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
        string strFileName = "Awaiting_Billing_Advice" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
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

        gvFreight.AllowPaging = false;
        gvFreight.AllowSorting = false;

        gvFreight.Columns[1].Visible = false;
        gvFreight.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "ArrivalNotice.aspx";
        DataFilter1.FilterDataSource();
        gvFreight.DataBind();

        gvFreight.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion

    #region HOLD JOB EXPENSE

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        int JobId = 0;
        if (hdnJobId.Value != "")
        {
            JobId = Convert.ToInt32(hdnJobId.Value);
        }

        if (JobId != 0)
        {
            if (txtReason.Text != "")
            {
                string RejectType = "0";
                DropDownList ddlRejectType = (DropDownList)fvHoldJobDetail.FindControl("ddlReasonHold");
                RejectType = ddlRejectType.SelectedValue;

                if (RejectType != "0")
                {
                    int result = DBOperations.FR_AddHoldBillingAdvice(JobId, txtReason.Text.Trim(), RejectType, LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        fvHoldJobDetail.DataBind();
                        lblMessage.Text = "Successfully holded job no " + hdnJobRefNo.Value + ".";
                        lblMessage.CssClass = "success";
                        gvFreight.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "System error. Please try again later.";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
                else
                {
                    lblError_HoldExp.Text = "Please Select Rejection Type.";
                    lblError_HoldExp.CssClass = "errorMsg";

                    rfvReason.Enabled = true;
                    mpeHoldExpense.Show();
                }
            }
            else
            {
                //lblError_HoldExp.Text = "Please enter reason.";
                //lblError_HoldExp.CssClass = "errorMsg";
                rfvReason.Enabled = true;
                mpeHoldExpense.Show();
            }
        }
    }

    #endregion

    protected void gvFreight_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        { 
            // Hold Status
            if (DataBinder.Eval(e.Row.DataItem, "HoldStatus") != DBNull.Value)
            {
                ImageButton imgbtnHoldJob = (ImageButton)e.Row.FindControl("imgbtnHoldJob");
                ImageButton imgbtnUnholdJob = (ImageButton)e.Row.FindControl("imgbtnUnholdJob");
                LinkButton lnkScrutiny = (LinkButton)e.Row.FindControl("lnkScrutiny");

                string HoldStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "HoldStatus"));
                if (HoldStatus.ToLower().Trim() == "hold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = true;
                        imgbtnUnholdJob.Visible = false;
                        // lnkScrutiny.Visible = true;
                    }
                }
                else if (HoldStatus.ToLower().Trim() == "unhold")
                {
                    if (imgbtnHoldJob != null && imgbtnUnholdJob != null)
                    {
                        imgbtnHoldJob.Visible = false;
                        imgbtnUnholdJob.Visible = true;
                        // lnkScrutiny.Visible = false;
                    }
                }
            }

            // Editable Cell

            AddCell(FIRST_EDITABLE_CELL + 5, "Instructions", e);
        }
    }

    void AddCell(int cellIndex, string cellName, GridViewRowEventArgs e)
    {
        //we will add javascript events to the controls in the cell
        Label lab = (Label)e.Row.Cells[cellIndex].FindControl(string.Format("lab{0}", cellName));
        TextBox txt = (TextBox)e.Row.Cells[cellIndex].FindControl(string.Format("txt{0}", cellName));
        Button btn = (Button)e.Row.Cells[cellIndex].FindControl(string.Format("btn{0}", cellName));

        if (lab != null)
        {
            //if text is empty, we need to grow label's width to ensure user can click this label
            if (string.IsNullOrEmpty(lab.Text))
            {
                lab.Width = 100;
                lab.Text = "Add";
            }

            //add javascript events to:
            //label (we want to hide this label and show textbox on click)
            lab.Attributes.Add("onclick", string.Format("return HideLabel('{0}', event, '{1}');", lab.ClientID, txt.ClientID));
            //and to textbox
            txt.Attributes.Add("onkeypress", string.Format("return SaveDataOnEnter('{0}', event, '{1}', '{2}');", txt.ClientID, lab.ClientID, btn.ClientID));
            //todo: there is an issue with onblur, I am not JS master, but hope it is just a small issue

            txt.Attributes.Add("onblur", string.Format("return SaveDataOnLostFocus('{0}', '{1}');", txt.ClientID, btn.ClientID));

            //highlight a text in textbox
            txt.Attributes.Add("onfocus", "select()");

            //we need to know what row and cell was edited
            //you can use anything else instead, e.g. session or whatever
            btn.CommandName = e.Row.RowIndex.ToString();
            btn.CommandArgument = cellIndex.ToString();

            //set a cursor style
            e.Row.Attributes["style"] += "cursor:pointer;cursor:hand;"; //just a cosmetic thing :)
        }
    }
}