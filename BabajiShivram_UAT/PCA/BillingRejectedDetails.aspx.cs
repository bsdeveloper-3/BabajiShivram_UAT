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
using System.Data.SqlClient;

public partial class PCA_BillingRejectedDetails : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    private static Random _random = new Random();
    ArrayList services = new ArrayList();
    int ModuleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager1.RegisterPostBackControl(lnk);
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblMessage.Text = "";
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Rejected Billing Advice";

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Rejected Billing!";
                lblMessage.CssClass = "errorMsg";
                //pnlFilter.Visible = false;
            }

        }

        DataFilter1.DataSource = PCDRejectedSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "BillingRejectedDetails.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        if (gvJobDetail.Rows.Count == 0)
        {
            lblMessage.Text = "No Job Found For Non Recieved file for Bill Rejected!";
            lblMessage.CssClass = "errorMsg"; ;
        }
        else
        {
            lblMessage.Text = "";
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
            DataFilter1.DataColumns = gvJobDetail.Columns;
            DataFilter1.FilterSessionID = "BillingRejectedDetails.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();

            if (gvJobDetail.Rows.Count == 0)
            {

                lblMessage.Text = "No Job Found for Billing Rejected!";
                lblMessage.CssClass = "errorMsg";
            }
            else
            {
                lblMessage.Text = "";
            }


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
        string strFileName = "RejectedBillingAdvice_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");

    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /*Verifies that the control is rendered */
    //}

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
        //gvJobDetail.Columns[0].Visible = false;     // Edit button
        //gvJobDetail.Columns[2].Visible = false;     // checkbox
        //gvJobDetail.Columns[6].Visible = false;     // bjv link button
        //gvJobDetail.Columns[17].Visible = false;    // rulename
        //gvJobDetail.Columns[19].Visible = false;    // Jobid
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;
        gvJobDetail.ShowHeader = false;
        gvJobDetail.Caption = "Rejected Billing Advice On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "BillingRejectedDetails.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Bill Rejection

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    protected void gvJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "update")
        {
            string JobId = e.CommandArgument.ToString();

            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

            TextBox txtfollowupdate = (TextBox)gvrow.FindControl("txtfollowupdate");
            TextBox txtremarks = (TextBox)gvrow.FindControl("txtremarks");

            string strfollowupdate = Request.Form[txtfollowupdate.UniqueID];
            string txtfollowupremarks = Request.Form[txtremarks.UniqueID];


            PCDRejectedSqlDataSource.UpdateParameters["JobId"].DefaultValue = JobId;

            if (strfollowupdate != "")
            {
                string strfollowupdate1 = Commonfunctions.CDateTime(strfollowupdate).ToShortDateString();
                PCDRejectedSqlDataSource.UpdateParameters["FollowupDate"].DefaultValue = strfollowupdate1;
            }
            if (txtfollowupremarks != "")
            {
                PCDRejectedSqlDataSource.UpdateParameters["FollowupRemark"].DefaultValue = txtfollowupremarks;
            }

            PCDRejectedSqlDataSource.Update();
            gvJobDetail.EditIndex = -1;
        }
        else if (e.CommandName.ToLower() == "cancel")
        {

            lblMessage.Text = "";
            gvJobDetail.EditIndex = -1;
        }
        else if (e.CommandName.ToLower() == "showbjv")
        {
            ViewState["lblDEBITAMT"] = "0";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobrefno = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strJobrefno = commandArgs[1].ToString();

            lbldiv.Text = "BJV Details - " + strJobrefno;
            rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(JobId));
            rptBJVDetails.DataBind();
            mpeBJVDetails.Show();
        }
    }
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvJobDetail.HeaderRow.FindControl("chkboxSelectAll");
        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chk1");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                lblmessage1.Text = "";
                ChkBoxRows.Checked = false;
            }
        }
    }
    protected void PCDRejectedSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {

        e.ExceptionHandled = false;

        if (e.Exception != null)
        {
            lblMessage.Text = e.Exception.Message;
            lblMessage.CssClass = "errorMsg";
        }
        else
        {

            lblMessage.Visible = true;

            int Result = Convert.ToInt32(e.Command.Parameters["@Output"].Value);

            if (Result == 0)
            {
                lblMessage.Text = "Billing Rejected Detail Updated Successfully";
                lblMessage.CssClass = "success";
            }
            else if (Result == 1)
            {
                lblMessage.Text = "System Error! Please try after sometime!";
                lblMessage.CssClass = "errorMsg";
            }
        }

    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        RePopulateValues(); //Checkbox

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status1 = (Label)e.Row.FindControl("rulename");
            string status = status1.Text.ToString();


            LinkButton lnkviewhistory = (LinkButton)e.Row.FindControl("lnkviewhistory");
            lnkviewhistory.Enabled = false;


            DataRowView rowView = (DataRowView)e.Row.DataItem;
            int JobId = Convert.ToInt32(rowView["JobId"]);

            Label lbl = (Label)e.Row.FindControl("lblReceipts");
            if (lbl.Text != "-")
            {
                int remarkid = BillingOperation.colorforfollowupdetaildone(JobId, 1);
                if (remarkid == 0)
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    lbl.Font.Bold = true;
                }
                else
                {
                    lbl.ForeColor = System.Drawing.Color.Green;
                    lbl.Font.Bold = true;
                    lnkviewhistory.Enabled = true;
                }

            }


            Label lbl1 = (Label)e.Row.FindControl("lblLRDC");
            if (lbl1.Text != "-")
            {
                int remarkid = BillingOperation.colorforfollowupdetaildone(JobId, 2);
                if (remarkid == 0)
                {
                    lbl1.ForeColor = System.Drawing.Color.Red;
                    lbl1.Font.Bold = true;

                }
                else
                {
                    lbl1.ForeColor = System.Drawing.Color.Green;
                    lbl1.Font.Bold = true;
                    lnkviewhistory.Enabled = true;
                }
            }



            Label lbl2 = (Label)e.Row.FindControl("lblPCDACK");
            if (lbl2.Text != "-")
            {
                int remarkid = BillingOperation.colorforfollowupdetaildone(JobId, 3);
                if (remarkid == 0)
                {
                    lbl2.ForeColor = System.Drawing.Color.Red;
                    lbl2.Font.Bold = true;
                }
                else
                {
                    lbl2.ForeColor = System.Drawing.Color.Green;
                    lbl2.Font.Bold = true;
                    lnkviewhistory.Enabled = true;
                }
            }


            Label lbl3 = (Label)e.Row.FindControl("lblMailApproval");
            if (lbl3.Text != "-")
            {
                int remarkid = BillingOperation.colorforfollowupdetaildone(JobId, 4);
                lbl3.ForeColor = System.Drawing.Color.Green;
                lbl3.Font.Bold = true;
                if (remarkid == 0)
                {
                    lbl3.ForeColor = System.Drawing.Color.Red;
                    lbl3.Font.Bold = true;
                }
                else
                {
                    lbl3.ForeColor = System.Drawing.Color.Green;
                    lbl3.Font.Bold = true;
                    lnkviewhistory.Enabled = true;
                }
            }


            Label lbl4 = (Label)e.Row.FindControl("lblOthers");
            if (lbl4.Text != "-")
            {
                int remarkid = BillingOperation.colorforfollowupdetaildone(JobId, 5);

                if (remarkid == 0)
                {
                    lbl4.ForeColor = System.Drawing.Color.Red;
                    lbl4.Font.Bold = true;
                }
                else
                {
                    lbl4.ForeColor = System.Drawing.Color.Green;
                    lbl4.Font.Bold = true;
                    lnkviewhistory.Enabled = true;
                }
            }

            if (status == "1")
            {
                e.Row.BackColor = System.Drawing.Color.Aqua;
                e.Row.ToolTip = "FIFO";
            }

            if (status == "2")
            {
                e.Row.BackColor = System.Drawing.Color.Tomato;
                e.Row.ToolTip = "WEEKDAYS";
            }
            if (status == "3")
            {
                e.Row.BackColor = System.Drawing.Color.SkyBlue;
                e.Row.ToolTip = "DAYS";
            }
            if (status == "4")
            {
                e.Row.BackColor = System.Drawing.Color.Pink;
                e.Row.ToolTip = "Urgent Bill";
            }

            if (status == "5")
            {
                e.Row.BackColor = System.Drawing.Color.Khaki;
                e.Row.ToolTip = "High Credit Days";
            }
            if (status == "6")
            {
                e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                e.Row.ToolTip = "amount";
            }
            if (status == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSlateGray;
                e.Row.ToolTip = "User Days";
            }

            e.Row.Cells[8].ToolTip = "Today-Billing Rejected Date";   //aging        
            //e.Row.Cells[17].CssClass = "hidden"; //rejected by
            e.Row.Cells[19].CssClass = "hidden";//rulename
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Cells[17].CssClass = "hidden";  //rejected by
            e.Row.Cells[19].CssClass = "hidden";//rulename
        }
    }  
    protected void gvJobDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RememberOldValues();
        gvJobDetail.PageIndex = e.NewPageIndex;
        RePopulateValues();

    } //Checkbox
    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            index = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value);

            bool result = ((CheckBox)row.FindControl("chk1")).Checked;

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!categoryIDList.Contains(index))
                    categoryIDList.Add(index);
            }
            else
            {
                categoryIDList.Remove(index);

                countRow = countRow + 1;
            }
            // }
        }
        if (categoryIDList != null && categoryIDList.Count > 0)
            Session["CHECKED_ITEMS"] = categoryIDList;


        int countRow1 = countRow;

    } //Checkbox
    private void RePopulateValues()
    {

        ArrayList categoryIDList = (ArrayList)Session["CHECKED_ITEMS"];

        if (categoryIDList != null && categoryIDList.Count > 0)
        {
            foreach (GridViewRow row in gvJobDetail.Rows)
            {
                int index = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value);

                bool result = ((CheckBox)row.FindControl("chk1")).Checked;
                if (categoryIDList.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = true;
                }
                else
                {
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chk1");
                    myCheckBox.Checked = false;

                }

            }
        }
        //gvNonRecievedJobDetail.AllowPaging = true;
    }//Checkbox
    protected void BtnComplete_Click(object sender, EventArgs e)
    {
        int i = 0;
        int Cnt = 0;
        int JobId = 0;
        string JobRefNo;
        int count = 0;
        lblmessage1.Text = "";
        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        gvJobDetail.AllowPaging = false;//Checkbox
        gvJobDetail.DataBind();//Checkbox


        ArrayList arrayRow = new ArrayList();

        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                arrayRow.Add(gvr);
                count++;
            }
        }

        if(count == 0)
        {
            lblChk.Text = "Please Checked Atleast one checkbox!";
            lblChk.CssClass = "errorMsg";
            return;
        }

        GridViewRowCollection rowCollection = new GridViewRowCollection(arrayRow);
        
        int prvId = Convert.ToInt16(hdnPrevId.Value);
        int currId = Convert.ToInt16(hdnCurrentId.Value);
        bool IsFollowEntered = true;

        //foreach (GridViewRow gvr in rowCollection)
        for (int k =0; k < rowCollection.Count; k++)
        {
            GridViewRow gvr = rowCollection[k];
            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                lblMessage.Text = "";
                CheckBox chklist1 = (CheckBox)gvr.FindControl("Chk1");
                hdnCurrentId.Value = gvr.RowIndex.ToString();

                JobId = Convert.ToInt32(gvJobDetail.DataKeys[gvr.RowIndex].Values["JobId"]);
                ViewState["JobId"] = JobId;
                ViewState["JobRefNo"] = gvJobDetail.Rows[gvr.RowIndex].Cells[3].Text;
                Label a = gvr.FindControl("lblfollowupdate") as Label;
                string follwupdate = a.Text;

                Label b = gvr.FindControl("lblfollowupdate") as Label;
                string follwupremark = b.Text;

                Label c = gvr.FindControl("lblRejectedby") as Label;
                ViewState["Rejectedby"] = Convert.ToInt16(c.Text);
                
                i++;

                if (follwupdate == "")
                {
                    IsFollowEntered = false;
                    lblChk.Text = "Please enter followup date for JobRefNo: " + ViewState["JobRefNo"];
                    lblChk.CssClass = "errorMsg";

                    break;
                }
                else if (follwupremark == "")
                {
                    IsFollowEntered = false;
                    lblChk.Text = "Please enter followup remarks for JobRefNo:" + ViewState["JobRefNo"];
                    lblChk.CssClass = "errorMsg";

                    break;
                }
                else
                {
                    ViewState["count"] = count;

                    if (count == 1)
                    {
                        btnPrevious.Visible = false;
                        btnNext.Visible = false;
                    }
                    else
                    {
                        btnPrevious.Visible = false;
                        btnNext.Visible = true;
                    }

                    ViewState["JobId"] = JobId.ToString();

                    services.Add(JobId + ',');
                    ViewState["services"] = services.Add(JobId + ',');

                    sqldatasourcebillingfollowup.SelectParameters["JobId"].DefaultValue = ViewState["JobId"].ToString();
                    RptBillingfollowup.DataBind();

                    //ModalPopupExtender1.Show();

                    break;
                }

            }

            else
            {
                if (i == 0)
                {
                    lblChk.Text = "Please Checked Atleast one checkbox!";
                    lblChk.CssClass = "errorMsg";
                }
            }
        }

        if(i == 1)
        {
            // Do Nothing
        }
        else if (i != 0)
        {
            lblChk.Text = "";
        }

        JobRefNo = ViewState["JobRefNo"].ToString();

        GetModuleId(JobRefNo);

        // Start => dont show popup for ok button if only 1 rejection reason is present
        if (RptBillingfollowup.Items.Count == 1)
        {
            foreach (RepeaterItem row in RptBillingfollowup.Items)
            {
                Label lblreasonId1 = (Label)row.FindControl("lblreasonId");
                int SaveReason = BillingOperation.insbillingrejectedcomplete(Convert.ToInt32(ViewState["JobId"]),
                                loggedInUser.glUserId, Convert.ToInt16(lblreasonId1.Text), Convert.ToInt16(ViewState["Rejectedby"]),ModuleId);
            }

            ModalPopupExtender1.Hide();
        }
        else if (IsFollowEntered == true)
        {
            ModalPopupExtender1.Show();
        }
        // End => dont show popup for ok button if only 1 rejection reason is present

        gvJobDetail.AllowPaging = true;//Checkbox
        gvJobDetail.DataBind();//Checkbox
    }
    protected void BtnSendmail_Click(object sender, EventArgs e)
    {
        int i = 0;

        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        gvJobDetail.AllowPaging = false;//Checkbox
        gvJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                i++;

                int JobId = Convert.ToInt32(gvJobDetail.DataKeys[gvr.RowIndex].Values["JobId"]);

                ViewState["JobRefNo"] = gvJobDetail.Rows[gvr.RowIndex].Cells[3].Text;

                Label a = gvr.FindControl("lblfollowupdate") as Label;
                string follwupdate = a.Text;

                Label b = gvr.FindControl("lblfollowupdate") as Label;
                string follwupremark = b.Text;

                Label c = gvr.FindControl("lblRejectedby") as Label;
                ViewState["Rejectedby"] = Convert.ToInt16(c.Text);


                int Result = BillingOperation.manualmailnotification(JobId, loggedInUser.glUserId, Convert.ToInt16(ViewState["Rejectedby"]), Convert.ToInt16(Session["FinYearId"]));

                if (Result == 0)
                {
                    lblMessage.Text = "Mail Sent Successfully!";
                    lblMessage.CssClass = "success";
                    gvJobDetail.DataBind();
                }
                else if (Result == 1)
                {
                    lblMessage.Text = "System Error! Please try after sometime!";
                    lblMessage.CssClass = "errorMsg";
                }
            }
        }
        if (i == 0)
        {
            lblMessage.Text = "Please Checked Checkbox First!";
            lblMessage.CssClass = "errorMsg";
        }
        gvJobDetail.AllowPaging = true;//Checkbox
        gvJobDetail.DataBind();//Checkbox

    }
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            Label label = e.Item.FindControl("lblrefno") as Label;
            label.Text = ViewState["JobRefNo"].ToString();

            Label label1 = e.Item.FindControl("lbljobid") as Label;
            label1.Text = ViewState["JobId"].ToString();
            label1.Visible = false;

            CheckBox chk = e.Item.FindControl("Chk1") as CheckBox;

        }
    }
    protected void btnPrevious_click(object sender, EventArgs e)
    {
        int i = 0;
        int Cnt = 0;
        int JobId = 0;
        string JobRefNo;
        int prvId = Convert.ToInt16(hdnPrevId.Value);
        int currId = Convert.ToInt16(hdnCurrentId.Value);

        int LastId = 0;


        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                JobId = Convert.ToInt32(gvJobDetail.DataKeys[gvr.RowIndex].Values["JobId"]);
                ViewState["JobId"] = JobId;
                ViewState["JobRefNo"] = gvJobDetail.Rows[gvr.RowIndex].Cells[3].Text;
                if (prvId == gvr.RowIndex)
                {
                    if (gvr.RowIndex == 0)
                    {
                        btnPrevious.Visible = false;
                    }

                    hdnCurrentId.Value = prvId.ToString();

                    hdnPrevId.Value = LastId.ToString();

                    Label a = gvr.FindControl("lblfollowupdate") as Label;
                    string follwupdate = a.Text;

                    Label b = gvr.FindControl("lblfollowupdate") as Label;
                    string follwupremark = b.Text;

                    Label c = gvr.FindControl("lblRejectedby") as Label;
                    ViewState["Rejectedby"] = Convert.ToInt16(c.Text);


                    i++;

                    if (follwupdate == "")
                    {
                        lblMessage.Text = "Please enter followup date for JobRefNo: " + ViewState["JobRefNo"];
                        lblMessage.CssClass = "errorMsg";
                    }
                    else if (follwupremark == "")
                    {
                        lblMessage.Text = "Please enter followup remarks for JobRefNo: " + ViewState["JobRefNo"];
                        lblMessage.CssClass = "errorMsg";
                    }
                    else
                    {
                        sqldatasourcebillingfollowup.SelectParameters["JobId"].DefaultValue = JobId.ToString();
                        ModalPopupExtender1.Show();

                    }

                    break;
                }//END_IF_Prev_Curr
                else
                {
                    LastId = gvr.RowIndex;
                    ModalPopupExtender1.Show();
                }
            }



        }
    }
    protected void btnNext_click(object sender, EventArgs e)
    {
        nextclick();

    }
    protected void btnok_click(object sender, EventArgs e)
    {
        
        int j = 0;
        int Result = 0, ModuleId = 1;
        string JobRefNo = "";

        foreach (RepeaterItem i in RptBillingfollowup.Items)
        {
           CheckBox chk = (CheckBox)i.FindControl("Chk1");
            Label lblreasonId1 = (Label)i.FindControl("lblreasonId");
            Label lblrefno = (Label)i.FindControl("lblrefno");
            JobRefNo = ViewState["JobRefNo"].ToString();
            
            if (JobRefNo.Contains("FF"))
            {
                ModuleId = 2;
            }

            if (chk.Checked)
            {
                //ModalPopupExtender1.Hide();
                lblreasonId1.Visible = false;
                lblMessage.CssClass = "errorMsg";
                Result = BillingOperation.insbillingrejectedcomplete(Convert.ToInt32(ViewState["JobId"]), loggedInUser.glUserId, Convert.ToInt16(lblreasonId1.Text), Convert.ToInt16(ViewState["Rejectedby"]), ModuleId);

                j++;
                if (Result == 0)
                {
                    ModalPopupExtender1.Show();
                    lblmessage1.Text = "File Completed Successfully!";
                    lblmessage1.CssClass = "success";
                    lblMessage.Text = "";
                }
                else if (Result == 1)
                {
                    lblmessage1.Text = "System Error! Please try after sometime!";
                    lblmessage1.CssClass = "errorMsg";
                }
            }
            else
            {
                if (j == 0)
                {
                    ModalPopupExtender1.Show();
                    lblmessage1.Text = "Please Check atleast one Checkbox!";
                    lblmessage1.CssClass = "errorMsg";
                }
            }

        }
        ////nextclick();
    }
    public void nextclick()
    {

        int i = 0;
        int Cnt = 0;
        int JobId = 0;
        int LastId = 0;
        int nxtId = Convert.ToInt16(hdnNxtId.Value);
        int currId = Convert.ToInt16(hdnCurrentId.Value);

        lblmessage1.Text = "";
        int FirstRowIndex = -1;
        bool IsLastRow = true;

        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {

            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                if (FirstRowIndex == -1)
                {
                    FirstRowIndex = gvr.RowIndex;
                }

                JobId = Convert.ToInt32(gvJobDetail.DataKeys[gvr.RowIndex].Values["JobId"]);
                ViewState["JobRefNo"] = gvJobDetail.Rows[gvr.RowIndex].Cells[3].Text;
                ViewState["JobId"] = JobId;
                nxtId = gvr.RowIndex;

                if (nxtId > currId)
                {
                    IsLastRow = false;
                    btnPrevious.Visible = true;
                    hdnPrevId.Value = currId.ToString();
                    hdnCurrentId.Value = nxtId.ToString();

                    Label a = gvr.FindControl("lblfollowupdate") as Label;
                    string follwupdate = a.Text;

                    Label b = gvr.FindControl("lblfollowupdate") as Label;
                    string follwupremark = b.Text;

                    Label c = gvr.FindControl("lblRejectedby") as Label;
                    ViewState["Rejectedby"] = Convert.ToInt16(c.Text);

                    i++;

                    if (follwupdate == "")
                    {
                        lblMessage.Text = "Please enter followup date for JobRefNo: " + ViewState["JobRefNo"];
                        lblMessage.CssClass = "errorMsg";
                    }
                    else if (follwupremark == "")
                    {
                        lblMessage.Text = "Please enter followup remarks for JobRefNo: " + ViewState["JobRefNo"];
                        lblMessage.CssClass = "errorMsg";
                    }

                    else
                    {
                        services.Add(JobId + ',');

                        sqldatasourcebillingfollowup.SelectParameters["JobId"].DefaultValue = JobId.ToString();
                        ModalPopupExtender1.Show();
                    }

                    break;
                }//END_IF_Nxt_Curr

                else
                {
                    LastId = gvr.RowIndex;
                    ModalPopupExtender1.Show();
                }

            }
        }// END_ForEach

        if (IsLastRow == true)
        {
            JobId = Convert.ToInt32(gvJobDetail.DataKeys[FirstRowIndex].Values["JobId"]);
            ViewState["JobRefNo"] = gvJobDetail.Rows[FirstRowIndex].Cells[3].Text;
            ViewState["JobId"] = JobId;

            btnPrevious.Visible = true;
            hdnPrevId.Value = currId.ToString();
            hdnCurrentId.Value = FirstRowIndex.ToString();

            Label a = gvJobDetail.Rows[FirstRowIndex].FindControl("lblfollowupdate") as Label;
            string follwupdate = a.Text;

            Label b = gvJobDetail.Rows[FirstRowIndex].FindControl("lblfollowupdate") as Label;
            string follwupremark = b.Text;

            Label c = gvJobDetail.Rows[FirstRowIndex].FindControl("lblRejectedby") as Label;
            ViewState["Rejectedby"] = Convert.ToInt16(c.Text);

            i++;

            if (follwupdate == "")
            {
                lblMessage.Text = "Please enter followup date for JobRefNo: " + ViewState["JobRefNo"];
                lblMessage.CssClass = "errorMsg";
            }
            else if (follwupremark == "")
            {
                lblMessage.Text = "Please enter followup remarks for JobRefNo: " + ViewState["JobRefNo"];
                lblMessage.CssClass = "errorMsg";
            }

            else
            {
                services.Add(JobId + ',');

                sqldatasourcebillingfollowup.SelectParameters["JobId"].DefaultValue = JobId.ToString();
                ModalPopupExtender1.Show();
            }
        }
    }
    protected void lnkviewhistory_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        Label lblJobID = (Label)clickedRow.FindControl("lblJobID");
        Label lblremark = (Label)clickedRow.FindControl("lblremark");

        SqlDataSourcefollowuphistory.SelectParameters["JobId"].DefaultValue = lblJobID.Text.ToString();
        ModalPopupExtenderFollowuphistory.Show();
    }
    protected void btncancel_click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        gvJobDetail.DataBind();
    }
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderFollowuphistory.Hide();
    }
    protected void gvJobDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();

            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 11;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "RECEIPTS";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.Text = "LR/DC";
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "PCD ACK.";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.Text = "Mail Approval";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.Text = "Others";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell = new TableCell();
            HeaderCell.Text = "Others";
            HeaderGridRow.Cells.Add(HeaderCell);


            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvJobDetail.Controls[0].Controls.AddAt(0, HeaderGridRow);

        }
    }
    protected void Rptpriorities_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblpriorities = (Label)e.Item.FindControl("lblpriorities");
            TextBox txtcolor = (TextBox)e.Item.FindControl("txtpriorities");

            if (lblpriorities.Text == "FIFO")
            {
                txtcolor.BackColor = System.Drawing.Color.Aqua;
            }
            else if (lblpriorities.Text == "Week Day")
            {
                txtcolor.BackColor = System.Drawing.Color.Tomato;
            }
            else if (lblpriorities.Text == "Day")
            {
                txtcolor.BackColor = System.Drawing.Color.SkyBlue;
            }
            else if (lblpriorities.Text == "Urgent Bill")
            {
                txtcolor.BackColor = System.Drawing.Color.Pink;
            }
            else if (lblpriorities.Text == "High Credit Days")
            {
                txtcolor.BackColor = System.Drawing.Color.Khaki;
            }
            else if (lblpriorities.Text == "Debit Amount")
            {
                txtcolor.BackColor = System.Drawing.Color.LightSeaGreen;
            }
            else
            {
                txtcolor.BackColor = System.Drawing.Color.LightSlateGray;
            }

        }


    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "BillRejected_ReceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        RejectjoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    //private void RejectjoblistExport(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    gvJobDetail.AllowPaging = false;
    //    gvJobDetail.AllowSorting = false;
    //    gvJobDetail.Caption = "";

    //    gvJobDetail.Columns[0].Visible = false;
    //    gvJobDetail.Columns[2].Visible = false;
    //    gvJobDetail.Columns[16].Visible = false;
    //    gvJobDetail.Columns[19].Visible = false;

    //    gvJobDetail.DataSourceID = "PCDRejectedSqlDataSource";
    //    gvJobDetail.DataBind();

    //    //Remove Controls
    //    this.RemoveControls(gvJobDetail);

    //    gvJobDetail.RenderControl(hw);

    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}

    private void RejectjoblistExport(string header, string contentType)
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
        gvJobDetail.Caption = "";

        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[2].Visible = false;
        gvJobDetail.Columns[18].Visible = false;
        gvJobDetail.Columns[19].Visible = false;
        gvJobDetail.Columns[22].Visible = false;


        gvJobDetail.DataSourceID = "PCDRejectedSqlDataSource";
        gvJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvJobDetail);

        gvJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
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

    #region BJV POPUP EVENTS

    protected void btnCancelBJVdetails_Click(object sender, EventArgs e)
    {
        mpeBJVDetails.Hide();
    }

    protected void rptBJVDetails_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblDEBITAMT = ((Label)e.Item.FindControl("lblDEBITAMT"));
            if (lblDEBITAMT.Text == "")
                lblDEBITAMT.Text = "0";
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + int.Parse(lblDEBITAMT.Text);
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((Label)e.Item.FindControl("lbltotDebitamt")).Text = ViewState["lblDEBITAMT"].ToString();
        }
    }


    #endregion

    #region Consolidate Bill
    protected void lnkConsolidateBill_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        Label lblJobID = (Label)clickedRow.FindControl("lblJobID");
        Label lblremark = (Label)clickedRow.FindControl("lblremark");

        SqlDataSourcefollowuphistory.SelectParameters["JobId"].DefaultValue = lblJobID.Text.ToString();
        ModalPopupExtenderConsolidate.Show();
    }

    #endregion
    protected void GetModuleId(string strJobRefNo)
    {
        DataTable dtModule = BillingOperation.GetModuleId(strJobRefNo);

        if (dtModule.Rows.Count > 0)
        {
            foreach (DataRow row in dtModule.Rows)
            {
                ModuleId = Convert.ToInt32(row["MODULEID"].ToString());
            }
        }
        else
        {
            ModuleId = loggedInUser.glModuleId;
        }
    }

    
}
