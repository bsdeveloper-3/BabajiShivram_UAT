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


public partial class PCA_BillStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(rptDocument);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Billing Job Status";

        //if (LoggedInUser.glRoleId == 40 || LoggedInUser.glRoleId == 19 || LoggedInUser.glUserId == 1 || LoggedInUser.glUserId == 3)
        //{
        //    lnkexport.Visible = true;

        //}
        //else
        //{
        //    lnkexport.Visible = false;
        //}


        if (!IsPostBack)
        {
            Session["JobId"] = null;

            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Detail Found For Billing!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //

        DataFilter1.DataSource = JobDetailSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "BillStatus.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            //string strJobId = (string)e.CommandArgument;       
            //Session["JobId"] = strJobId;
            //Response.Redirect("BillStatusDetail.aspx");

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobId = "", strJobType = "";

            if (commandArgs[0].ToString() != "")
                strJobId = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strJobType = commandArgs[1].ToString();
           
            if (strJobType.ToLower().Trim() == "import") // Import Job
            {
                Session["JobId"] = strJobId;
                Response.Redirect("BillStatusDetail.aspx");
            }
            
            else if (strJobType.ToLower().Trim() == "export")
            {
                Session["JobId"] = strJobId;
                Response.Redirect("ExpBillStatusDetail.aspx");
            }
            else if (strJobType.ToLower().Trim() == "freight")
            {
                Session["JobId"] = strJobId;
                Response.Redirect("BillStatusDetailFr.aspx");
            }
            else if (strJobType.ToLower().Trim() == "additional") // additional Job
            {
                Session["JobId"] = strJobId;
                Response.Redirect("AddtnlBillStatus.aspx");
            }
            else if (strJobType.ToLower().Trim() == "pn movement") // pn movement Job
            {
                Session["JobId"] = strJobId;
                Response.Redirect("PnBillStatus.aspx");
            }
            else if (strJobType.ToLower().Trim() == "transport") // transport Job
            {
                Session["JobId"] = strJobId;
                Response.Redirect("TransportBillStatus.aspx");
            }
            else if (strJobType.ToLower().Trim() == "misc") // Misc Job
            {
                Session["JobId"] = strJobId;
                Response.Redirect("MiscBillStatus.aspx");
            }
            else if(strJobType.ToLower().Trim()== "sez")    //SEZ JOB
            {
                Session["JobId"] = strJobId;
                Response.Redirect("BillStatusDetailSez.aspx");
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            string strJobId = (string)e.CommandArgument;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            string strCustomer = "", strJobrefno = "";

            if (commandArgs[0].ToString() != "")
                strJobId = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustomer = commandArgs[1].ToString();
            if (commandArgs[2].ToString() != "")
                strJobrefno = commandArgs[2].ToString();

            Session["JobId"] = strJobId;

            rptDocument.DataSource = BillingOperation.FillPCDDocumentForonlineBill(Convert.ToInt32(Session["JobId"]), 4);
            rptDocument.DataBind();

            Label label1 = rptDocument.Controls[0].FindControl("lbljobrefno2") as Label;
            label1.Text = strJobrefno;
            Label label2 = rptDocument.Controls[0].FindControl("lblcustomer2") as Label;
            label2.Text = strCustomer;

            //string DocPath = e.CommandArgument.ToString();
            //DownloadDocument(DocPath);
            ModalPopupDocument.Show();
        }
        else if (e.CommandName.ToLower() == "showbjv")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobrefno = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strJobrefno = commandArgs[1].ToString();

            lbldiv.Text = "BJV Details - " + strJobrefno;
            ViewState["lblDEBITAMT"] = "0";
            rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(JobId));
            rptBJVDetails.DataBind();
            mpeBJVDetails.Show();
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

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[8].ToolTip = "Today – FSB"; // Aging1
            e.Row.Cells[9].ToolTip = "Today – FSS"; // Aging2
            e.Row.Cells[11].ToolTip = "Today – Current Stage Date"; // StageAging
            //e.Row.Cells[7].ToolTip = "Today – Clearance Date"; // Aging3

            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkview");
            if (lnk.Text == "N")
            {
                lnk.Enabled = false;
                lnk.Text = "";
            }
            else
            {
                lnk.Text = "View Document";
                lnk.Enabled = true;
            }

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[11].ToolTip = "Today – Document Hand Over To Shipping Line Date.";
                }
                else
                {
                    e.Row.Cells[11].ToolTip = "Today – Clearance Date.";
                }
            }
        }

    }

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
            DataFilter1.FilterSessionID = "BillStatus.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
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
        string strFileName = "Billing_JobStatus" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void rpDocument_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");
            //chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));
            //chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));
            //chkDuplicate.Items[1].Enabled = false;
            //chkDuplicate.Items[0].Enabled = false;
        }
    }
    //billing
    protected void rpDocument_ItemCommand(Object Sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "view")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
            ModalPopupDocument.Show();
        }
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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
        gvJobDetail.Columns[13].Visible = false; // Document

        DataFilter1.FilterSessionID = "BillStatus.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        gvJobDetail.RenderControl(hw);
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

    protected void BtnConsolidated_Click(object sender, EventArgs e)
    {
        int i = 0;
        bool flag = true;
        string Customer = "";
        lblConsolidatederror.Text = "";
        drpjobno.Items.Clear();
        drpjobno.DataBind();
        ListItem lst1 = new ListItem("Select", "0");
        //drpjobno.Items.Insert(0, lst1);
        RememberOldValues();
        RePopulateValues();
        //gvRecievedJobDetail.AllowPaging = false;
        //gvRecievedJobDetail.DataBind();
        foreach (GridViewRow gvr in gvJobDetail.Rows)
        {

            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {
                if (i > 0)
                {
                    if (Customer != gvJobDetail.Rows[gvr.RowIndex].Cells[5].Text)
                    {

                        flag = false;

                    }
                }

                Customer = gvJobDetail.Rows[gvr.RowIndex].Cells[5].Text;

                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobRefNo");
                string[] Value = Recv.CommandArgument.ToString().Split(new char[] { ';' });

                string JobId = Value[0];
                string s = string.Empty;
                ListItem lst = new ListItem(Recv.Text, JobId);
                drpjobno.Items.Insert(drpjobno.Items.Count, lst);

                if (Value[1] == "5" || Value[1] == "1")
                {
                    //string ReturnMsg = AllowforReceive(Convert.ToInt32(JobId));
                    ////  lblreceivemsg.Text = "@@";
                    //if (ReturnMsg != "")
                    //{
                    //    lblreceivemsg.Text = ReturnMsg;
                    //    lblreceivemsg.CssClass = "errorMsg";
                    //    break;
                    //}
                }

                i++;

            }

        }

        if (i == 0)
        {
            if (lblreceivemsg.Text == "")
            {
                lblreceivemsg.Text = "Please Checked atleast 1 checkbox.";
                lblreceivemsg.CssClass = "errorMsg";
            }
        }
        else if (flag == false)
        {
            lblreceivemsg.Text = "Please Select Same Customer.";
            lblreceivemsg.CssClass = "errorMsg";
            ModalPopupExtender2.Hide();
        }
        else if (lblreceivemsg.Text != "")
        {
            lblreceivemsg.Text = lblreceivemsg.Text;
            lblreceivemsg.CssClass = "errorMsg";
        }
        else
        {
            lblreceivemsg.Text = "";

            ModalPopupExtender2.Show();
        }

        gvJobDetail.AllowPaging = true;
        gvJobDetail.DataBind();
    }

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvJobDetail.Rows)
        {
            index = Convert.ToInt32(gvJobDetail.DataKeys[row.RowIndex].Value);
            //if (index == 58360)
            //{
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

    }

    private void RePopulateValues()
    {
        //gvNonRecievedJobDetail.AllowPaging = false;
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
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        int Result = 0;

        if (drpjobno.SelectedItem.Text == "Select")
        {
            lblConsolidatederror.Text = "Please Select Job First!.";
            lblConsolidatederror.CssClass = "errorMsg";
            ModalPopupExtender2.Show();
        }
        else
        {

            ViewState["Jobid"] = "";
            string[] arrItems = new string[drpjobno.Items.Count];
            if (drpjobno.Items.Count > 0)
            {
                for (int i = 0; i < drpjobno.Items.Count; i++)
                {
                    if (drpjobno.Items[i].Selected == false)
                    {

                        ViewState["Jobid"] = ViewState["Jobid"].ToString() + ',' + drpjobno.Items[i].Value.ToString();

                    }

                }
            }

            ModalPopupExtender2.Show();

            lblConsolidatederror.Text = drpjobno.SelectedItem.Text + '-' + drpjobno.SelectedItem.Value;
            lblreceivemsg.Text = ViewState["Jobid"].ToString();

            Result = BillingOperation.consolidatedfile(Convert.ToInt32(drpjobno.SelectedItem.Value), LoggedInUser.glUserId, ViewState["Jobid"].ToString(), Convert.ToInt16(EnumBilltype.DraftInvoice));
            if (Result == 0)
            {

                lblreceivemsg.Text = "File Condolidated Successfully!";
                lblreceivemsg.CssClass = "success";
                ModalPopupExtender2.Hide();
                //gvJobDetail.DataSource = JobDetailSqlDataSource;
                gvJobDetail.DataBind();
            }
            else if (Result == 1)
            {
                lblreceivemsg.Text = "System Error! Please try after sometime!";
                lblreceivemsg.CssClass = "errorMsg";
            }
            gvJobDetail.DataBind();
        }
        gvJobDetail.DataBind();
    }
}
