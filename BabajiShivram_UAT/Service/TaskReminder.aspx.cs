using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class Service_TaskReminder : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnReminder);
        ScriptManager1.RegisterPostBackControl(btnAddProjectUsers);
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnAddProjectUsers2);
        ScriptManager1.RegisterPostBackControl(gvSharedWithUsersList);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Task Reminder";
            gvReminder.DataBind();
        }

        // Allow  Only Future Date For Reminder
        calRemindDate.StartDate = DateTime.Today;

        // Set Minimum Reminder Date Today
        MskValRemindDate.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");

        DataFilter1.DataSource = ReminderDetailSqlDataSource;
        DataFilter1.DataColumns = gvReminder.Columns;
        DataFilter1.FilterSessionID = "TaskReminder.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region REMINDER

    protected void gvReminder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            Boolean RemindStatus = Convert.ToBoolean(rowView["RemindStatus"].ToString());

            foreach (TableCell cell in e.Row.Cells)
            {
                LinkButton lblRemindStatus = (LinkButton)e.Row.FindControl("lblRemindStatus");
                LinkButton lblRemoveReminder = (LinkButton)e.Row.FindControl("lnlRemoveReminder");
                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                LinkButton lblUsersList = (LinkButton)e.Row.FindControl("lblUsersList");

                if (RemindStatus == true)
                {
                    lblRemindStatus.Text = "Stop";
                    lblRemindStatus.Enabled = true;
                    lblRemoveReminder.Text = "Remove";
                    lblRemoveReminder.Enabled = true;
                    if (lnkEdit != null)
                        lnkEdit.Visible = true;
                    if (lblUsersList != null)
                        lblUsersList.Visible = true;
                }
                else
                {
                    lblRemindStatus.Text = "Stopped";
                    lblRemindStatus.Enabled = false;
                    lblRemoveReminder.Text = "";
                    lblRemoveReminder.Enabled = false;
                    if (lnkEdit != null)
                        lnkEdit.Visible = false;
                    if (lblUsersList != null)
                        lblUsersList.Visible = false;
                }
            }
        }
    }

    protected void gvReminder_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "removeremind")
        {
            lblError.Text = "";
            int ReminderId = Convert.ToInt32(e.CommandArgument);
            int result = DBOperations.DeleteReminder(ReminderId, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Reminder deleted successfully!";
                lblError.CssClass = "success";
                gvReminder.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Can Not Delete Reminder. Status Is Closed.";
                lblError.CssClass = "errorMsg";
                gvReminder.DataBind();
            }
        }
        else if (e.CommandName.ToLower() == "remindstatus")
        {
            lblError.Text = "";
            int ReminderId = Convert.ToInt32(e.CommandArgument);
            int result = DBOperations.UpdateReminderStatus(ReminderId, LoggedInUser.glUserId);
            if (result == 2)
            {
                lblError.Text = "Reminder deleted successfully!";
                lblError.CssClass = "success";
                gvReminder.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! please try after sometime";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 0)
            {
                lblError.Text = "Can Not Delete Reminder. Status Is Closed.";
                lblError.CssClass = "errorMsg";
                gvReminder.DataBind();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "userlist")
        {
            int ReminderId = Convert.ToInt32(e.CommandArgument);
            if (ReminderId != 0)
            {
                txtUserName.Text = "";
                lblErrorPopup.Text = "";
                mpeUserList.Show();
                hdnReminderId.Value = ReminderId.ToString();
                gvSharedWithUsersList.DataBind();
            }
        }
    }

    protected void gvReminder_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DateTime dtRemindDate = DateTime.MinValue;
        int RepeatMonth = 0;

        int Lid = Convert.ToInt32(gvReminder.DataKeys[e.RowIndex].Value.ToString());
        DropDownList ddlCategory = (DropDownList)gvReminder.Rows[e.RowIndex].FindControl("ddlCategory");
        TextBox txtNotes = (TextBox)gvReminder.Rows[e.RowIndex].FindControl("txtNotes");
        TextBox txtRemindDate = (TextBox)gvReminder.Rows[e.RowIndex].FindControl("txtRemindDate");
        TextBox txtRepeatMonth = (TextBox)gvReminder.Rows[e.RowIndex].FindControl("txtRepeatMonth");

        if (ddlCategory.SelectedValue != "0")
        {
            if (txtRemindDate.Text.Trim() != "")
            {
                int intDateCompare = DateTime.Compare(Convert.ToDateTime(txtRemindDate.Text.Trim()), DateTime.Today);
                if (intDateCompare < 0)
                {
                    lblError.Text = "Reminder date is earlier than today! Please change to future Date.";
                    lblError.CssClass = "errorMsg";
                    return;
                }
                else
                {
                    dtRemindDate = Commonfunctions.CDateTime(txtRemindDate.Text.Trim());
                }
            }

            if (txtRepeatMonth.Text.Trim() != "")
                RepeatMonth = Convert.ToInt32(txtRepeatMonth.Text.Trim());

            int result = DBOperations.UpdateTaskReminder(Lid, Convert.ToInt32(ddlCategory.SelectedValue), dtRemindDate, RepeatMonth,
                                            txtNotes.Text.Trim(), LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.CssClass = "success";
                lblError.Text = "Reminder Updated Successfully..!!";
                gvReminder.EditIndex = -1;
                gvReminder.DataBind();
                e.Cancel = true;
            }
            else if (result == 2)
            {
                lblError.Text = "Cannot update reminder as it has been already stopped.!!";
                lblError.CssClass = "warning";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please Enter Category Name.";
        }
    }

    protected void gvReminder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvReminder.EditIndex = e.NewEditIndex;
        gvReminder.DataBind();
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvReminder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvReminder.EditIndex = -1;
        gvReminder.DataBind();
        lblError.Text = "";
        lblError.Visible = false;
    }

    protected void gvReminder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReminder.PageIndex = e.NewPageIndex;
        gvReminder.DataBind();
    }

    protected void btnAddReminder_Click(object sender, EventArgs e)
    {
        int RemindResult = -123, NotifyMode = 0, RepeatMonth = 0;
        DateTime dtRemindDate = DateTime.MinValue;

        if (chkRemindMode.SelectedIndex == -1)
        {
            lblError.Text = "Please Select Reminder Type Email Or SMS!";
            lblError.CssClass = "errorMsg";
            return;
        }

        int CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);

        if (txtRemindDate.Text.Trim() != "")
        {
            int intDateCompare = DateTime.Compare(Convert.ToDateTime(txtRemindDate.Text.Trim()), DateTime.Today);

            if (intDateCompare < 0)
            {
                lblError.Text = "Reminder date is earlier than today! Please change to future Date.";
                lblError.CssClass = "errorMsg";
                return;
            }
            else
            {
                dtRemindDate = Commonfunctions.CDateTime(txtRemindDate.Text.Trim());
            }
        }

        if (chkRemindMode.Items[0].Selected) // Email
            NotifyMode = 1;

        //if (chkRemindMode.Items[1].Selected) // SMS
        //    NotifyMode = 2;

        if (txtRepeatMonth.Text.Trim() != "")
            RepeatMonth = Convert.ToInt32(txtRepeatMonth.Text.Trim());

        RemindResult = DBOperations.AddTaskReminder(CategoryId, NotifyMode, dtRemindDate, RepeatMonth, txtRemindNote.Text.Trim(), LoggedInUser.glUserId);

        if (RemindResult > 0)
        {
            // Start => Add User List

            if (gvUserList != null && gvUserList.Rows.Count > 0)
            {
                for (int i = 0; i < gvUserList.Rows.Count; i++)
                {
                    int SharedUserId = 0;
                    Label lblUserName = (Label)gvUserList.Rows[i].Cells[1].FindControl("lblUserName");
                    Label lblUserId = (Label)gvUserList.Rows[i].Cells[2].FindControl("lblUserId");

                    if (lblUserId.Text.Trim() != "")
                    {
                        SharedUserId = Convert.ToInt32(lblUserId.Text.Trim());
                    }

                    if (SharedUserId != 0)
                    {
                        int SaveUserList = DBOperations.AddReminderUser(RemindResult, SharedUserId, LoggedInUser.glUserId);
                    }
                }
            }

            // End   => Add User List

            lblError.Text = "Reminder Added Successfully.";
            lblError.CssClass = "success";

            txtRemindDate.Text = "";
            txtRemindNote.Text = "";
            chkRemindMode.SelectedIndex = -1;
            gvReminder.DataBind();
            gvUserList.DataSource = "";
            gvUserList.DataBind();
            ViewState["ReminderUsers"] = null;
            chkRemindMode.Items[0].Selected = true;
        }
        else if (RemindResult == 1)
        {
            lblError.Text = "System Error. Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
        else if (RemindResult == 2)
        {
            lblError.Text = "Reminder Detail Already Added!";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancelReminder_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void ResetControls()
    {
        txtRemindDate.Text = "";
        txtRemindNote.Text = "";
        txtRepeatMonth.Text = "";
        ddlCategory.DataBind();
        chkRemindMode.DataBind();
        chkRemindMode.Items[0].Selected = true;
        gvReminder.DataBind();
        lblError.Text = "";
        Session["ReminderId"] = null;
    }

    protected void gvReminder_PreRender(object sender, EventArgs e)
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

    #region ADD Shared USERS

    protected void btnAddProjectUsers_OnClick(object sender, EventArgs e)
    {
        if (hdnUserId.Value != "0")
        {
            if (ViewState["ReminderUsers"] != null)
            {
                AddUserNewRow();
            }
            else
            {
                TransportationInitialRow();
            }
        }
    }

    protected void lnkDeleteRow_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
        int rowID = gvRow.RowIndex;
        if (ViewState["ReminderUsers"] != null)
        {
            DataTable dt = (DataTable)ViewState["ReminderUsers"];
            if (dt.Rows.Count > 1)
            {
                if (gvRow.RowIndex < dt.Rows.Count - 1)
                {
                    //Remove the Selected Row data and reset row number  
                    dt.Rows.Remove(dt.Rows[rowID]);
                    ResetRowID(dt);
                }

                ViewState["ReminderUsers"] = dt;
                gvUserList.DataSource = dt;
                gvUserList.DataBind();
            }
            else
            {
                ViewState["ReminderUsers"] = null;
                gvUserList.DataSource = "";
                gvUserList.DataBind();
            }
        }
        SetPreviousData();
    }

    protected void ResetRowID(DataTable dt)
    {
        int rowNumber = 1;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                row[0] = rowNumber;
                rowNumber++;
            }
        }
    }

    protected void TransportationInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("UserName", typeof(string)));     // UserName   
        dt.Columns.Add(new DataColumn("UserId", typeof(string)));     // UserId  

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["UserName"] = txtUser.Text.Trim();
        dr["UserId"] = hdnUserId.Value;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState for future reference   
        ViewState["ReminderUsers"] = dt;

        //Bind the Gridview   
        gvUserList.DataSource = dt;
        gvUserList.DataBind();

        if (gvUserList.Rows.Count > 0)
        {
            Label lblUserName = (Label)gvUserList.Rows[0].Cells[1].FindControl("lblUserName");
            Label lblUserId = (Label)gvUserList.Rows[0].Cells[2].FindControl("lblUserId");

            gvUserList.Rows[0].Cells[0].Text = "1";
            lblUserName.Text = txtUser.Text.Trim();
            lblUserId.Text = hdnUserId.Value;
        }
    }

    protected void AddUserNewRow()
    {
        if (ViewState["ReminderUsers"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["ReminderUsers"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {
                //add new row to DataTable   
                drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                dtCurrentTable.Rows.Add(drCurrentRow);

                //Store the current data to ViewState for future reference   
                ViewState["ReminderUsers"] = dtCurrentTable;
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    if (i < gvUserList.Rows.Count)
                    {
                        Label lblUserName = (Label)gvUserList.Rows[i].Cells[1].FindControl("lblUserName");
                        Label lblUserId = (Label)gvUserList.Rows[i].Cells[2].FindControl("lblUserId");

                        dtCurrentTable.Rows[i]["UserName"] = lblUserName.Text.Trim();
                        dtCurrentTable.Rows[i]["UserId"] = lblUserId.Text.Trim();
                    }
                    else
                    {
                        //add the username to grid view row   
                        dtCurrentTable.Rows[i]["UserName"] = txtUser.Text.Trim();
                        dtCurrentTable.Rows[i]["UserId"] = hdnUserId.Value;
                    }
                }
                //Rebind the Grid with the current data to reflect changes   
                gvUserList.DataSource = dtCurrentTable;
                gvUserList.DataBind();
            }
        }
        SetPreviousData();
    }

    protected void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["ReminderUsers"] != null)
        {
            DataTable dt = (DataTable)ViewState["ReminderUsers"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label lblUserName = (Label)gvUserList.Rows[i].Cells[1].FindControl("lblUserName");
                    Label lblUserId = (Label)gvUserList.Rows[i].Cells[2].FindControl("lblUserId");

                    //if (i < dt.Rows.Count)
                    //{
                    lblUserName.Text = dt.Rows[i]["UserName"].ToString();
                    lblUserId.Text = dt.Rows[i]["UserId"].ToString();
                    //}
                    rowIndex++;
                }
            }
        }
        txtUser.Text = "";
        hdnUserId.Value = "0";
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "TaskReminders_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvReminder.AllowPaging = false;
        gvReminder.AllowSorting = false;
        gvReminder.Columns[1].Visible = false;
        gvReminder.Columns[11].Visible = false;
        gvReminder.Columns[12].Visible = false;
        gvReminder.Columns[13].Visible = false;
        gvReminder.Columns[14].Visible = true;

        DataFilter1.FilterSessionID = "TaskReminder.aspx";
        DataFilter1.FilterDataSource();
        gvReminder.DataBind();

        gvReminder.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
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
            DataFilter1.FilterSessionID = "TaskReminder.aspx";
            DataFilter1.FilterDataSource();
            gvReminder.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region SHARED USERS LIST POPUP

    protected void gvSharedWithUsersList_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "remove")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid != 0)
            {
                int DeleteSharedUser = DBOperations.DeleteReminderUser(lid);
                if (DeleteSharedUser == 0)
                {
                    lblErrorPopup.Text = "Successfully deleted user.";
                    lblErrorPopup.CssClass = "success";
                    gvSharedWithUsersList.DataBind();
                    mpeUserList.Show();
                }
                else if (DeleteSharedUser == 2)
                {
                    lblErrorPopup.Text = "Record does not exists.";
                    lblErrorPopup.CssClass = "errorMsg";
                    mpeUserList.Show();
                }
                else
                {
                    lblErrorPopup.Text = "Error while deleting user. Please try again later.";
                    lblErrorPopup.CssClass = "errorMsg";
                    mpeUserList.Show();
                }
            }
        }
    }

    protected void btnAddProjectUsers2_OnClick(object sender, EventArgs e)
    {
        int UserId = 0;
        if (hdnUserId2.Value != "0")
            UserId = Convert.ToInt32(hdnUserId2.Value);

        if (UserId != 0)
        {
            int SaveUser = DBOperations.AddReminderUser(Convert.ToInt32(hdnReminderId.Value), UserId, LoggedInUser.glUserId);
            if (SaveUser == 0)
            {
                txtUserName.Text = "";
                gvSharedWithUsersList.DataBind();
                mpeUserList.Show();
                lblErrorPopup.Text = "Successfully added user.";
                lblErrorPopup.CssClass = "success";
            }
            else if (SaveUser == 2)
            {
                lblErrorPopup.Text = "User already exists..!!";
                lblErrorPopup.CssClass = "errorMsg";
                mpeUserList.Show();
            }
            else
            {
                lblErrorPopup.Text = "Error while deleting user. Please try again later.";
                lblErrorPopup.CssClass = "errorMsg";
                mpeUserList.Show();
            }
        }
        else
        {
            lblErrorPopup.Text = "Please select user.";
            lblErrorPopup.CssClass = "errorMsg";
            mpeUserList.Show();
        }
    }

    protected void gvSharedWithUsersList_PreRender(object sender, EventArgs e)
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

    protected void imgbtnApproval_Click(object sender, EventArgs e)
    {
        mpeUserList.Hide();
    }
}