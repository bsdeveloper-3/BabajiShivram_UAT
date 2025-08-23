using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;

public partial class PCA_PCDBillingScrutiny : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int ModuleId = 0;
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["lblDEBITAMT"] = "0";
        ViewState["lblCREDITAMT"] = "0";
        ViewState["lblAMOUNT"] = "0";

        // ScriptManager1.RegisterPostBackControl(lnknonreceive);
        ScriptManager1.RegisterPostBackControl(lnkReceiveXls);
        ScriptManager1.RegisterPostBackControl(rptDocument); ////billing

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending For Billing Scrutiny";

            //DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
            //DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            //DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            //DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);


            DataFilter2.DataSource = PCDReceivedSqlDataSource;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);


        }

        else
        {
            //if (TabPCDBilling.TabIndex == 0)
            //{
            //    if (TabJobDetail.ActiveTabIndex == 0)
            //    {
            //        lblreceivemsg.Text = "";
            //        lblMsgforApproveReject.Text = "";
            //        DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
            //        DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            //        DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            //        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

            //    }

            //}
            if (TabPCDBilling1.TabIndex == 0)
            {
                if (TabJobDetail.ActiveTabIndex == 0)
                {
                    //  lblreceivemsg.Text = "";
                    lblMsgforReceived.Text = "";
                    lblMsgforApproveReject.Text = "";
                    DataFilter2.DataSource = PCDReceivedSqlDataSource;
                    DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
                    DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
                    DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);

                }
            }

        }
        //if (gvNonRecievedJobDetail.Rows.Count == 0)
        //{
        //    lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Billing Scrutiny!";
        //    lblMsgforNonReceived.CssClass = "errorMsg";
        //}
        //else
        //{
        //    lblMsgforNonReceived.Text = "";
        //}
        if (gvRecievedJobDetail.Rows.Count == 0)
        {
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                lblMsgforReceived.Text = "No Job Found For Recieved file for Billing Scrutiny!";
                lblMsgforReceived.CssClass = "errorMsg";
            }
            else
            {
                lblMsgforReceived.Text = "";
            }
        }
        else
        {
            lblMsgforReceived.Text = "";
        }
        //Session["JobRefNo"] = "CB01193/BLAI/24-25";
        //SaveBilling_Invoice(644696);

    }

    #region Non Received

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

    protected void lnkNonreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "BillingScrutiny_nonreceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        NonreceivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void NonreceivejoblistExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        //gvNonRecievedJobDetail.AllowPaging = false;
        //gvNonRecievedJobDetail.AllowSorting = false;
        //gvNonRecievedJobDetail.Caption = "";

        //gvNonRecievedJobDetail.Columns[1].Visible = false;
        //gvNonRecievedJobDetail.Columns[10].Visible = false;

        //gvNonRecievedJobDetail.DataSourceID = "PCDNonReceivedSqlDataSource";
        //gvNonRecievedJobDetail.DataBind();

        ////Remove Controls
        //this.RemoveControls(gvNonRecievedJobDetail);

        //gvNonRecievedJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvNonRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        RePopulateValues(); //Checkbox

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status1 = (Label)e.Row.FindControl("rulename");

            string status = status1.Text.ToString();

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
                e.Row.BackColor = System.Drawing.Color.BurlyWood;
                e.Row.ToolTip = "Quick Paymaster";
            }

            if (status == "6")
            {
                e.Row.BackColor = System.Drawing.Color.Khaki;
                e.Row.ToolTip = "High Credit Days";
            }
            if (status == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                e.Row.ToolTip = "amount";
            }
            if (status == "8")
            {
                e.Row.BackColor = System.Drawing.Color.LightSlateGray;
                e.Row.ToolTip = "User Days";
            }

            e.Row.Cells[8].ToolTip = "Today - FSB"; // Aging1
            // e.Row.Cells[9].ToolTip = "Today - Clearance Date"; // Aging2
            e.Row.Cells[10].CssClass = "hidden";   //rulename   

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[9].ToolTip = "Today - Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[6].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[9].ToolTip = "Today - Job came in Additional tab.";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "transport")
                {
                    e.Row.Cells[9].ToolTip = "Today - Transport job created.";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else
                {
                    e.Row.Cells[9].ToolTip = "Today - Clearance Date.";
                    e.Row.Cells[7].Text = "";
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[10].CssClass = "hidden";   //rulename   
        }
    }

    protected void gvNonRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv1 = (GridView)sender;
        GridViewRow gvr1 = (GridViewRow)gv1.BottomPagerRow;
        if (gvr1 != null)
        {
            gvr1.Visible = true;
        }
    }

    protected void gvNonRecievedJobDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RememberOldValues();
        // gvNonRecievedJobDetail.PageIndex = e.NewPageIndex;
        RePopulateValues();


    } //Checkbox

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvRecievedJobDetail.Rows)
        {
            index = Convert.ToInt32(gvRecievedJobDetail.DataKeys[row.RowIndex].Value);

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
            foreach (GridViewRow row in gvRecievedJobDetail.Rows)
            {
                int index = Convert.ToInt32(gvRecievedJobDetail.DataKeys[row.RowIndex].Value);

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
        gvRecievedJobDetail.AllowPaging = true;
    }//Checkbox

    protected void Receive_Click(object sender, EventArgs e)
    {
        int i = 0;
        int Result = 0;
        string JobRefNo = "";
        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
                           //gvNonRecievedJobDetail.AllowPaging = false;//Checkbox
                           //gvNonRecievedJobDetail.DataBind();//Checkbox

        //foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chk1")).Checked)
        //    {
        //        LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");
        //        string JobId = Recv.CommandArgument;
        //        string s = string.Empty;
        //        JobRefNo = Recv.Text;
        //        ModuleId = 1;

        //        if (JobRefNo.Contains("FF"))
        //        {
        //            ModuleId = 2;
        //        }
        //        else if (JobRefNo.Contains("OE") || JobRefNo.Contains("AE"))
        //        {
        //            ModuleId = 5;
        //        }

        //        Result = BillingOperation.receivedfile(Convert.ToInt32(JobId), LoggedInUser.glUserId, Convert.ToInt16(EnumBilltype.BillingScrutiny),ModuleId);

        //        if (Result == 0 )
        //        {

        //            lblreceivemsg.Text = "File Recieved Successfully!";
        //            lblreceivemsg.CssClass = "success";
        //            gvNonRecievedJobDetail.DataBind();
        //            gvRecievedJobDetail.DataBind();
        //        }
        //        else if (Result == 1)
        //        {
        //            lblreceivemsg.Text = "System Error! Please try after sometime!";
        //            lblreceivemsg.CssClass = "errorMsg";
        //        }
        //        else if (Result == 3)
        //        {
        //            lblreceivemsg.Text = "This is freight Export job. Hence you can not receive";
        //            lblreceivemsg.CssClass = "errorMsg";
        //        }
        //        i++;
        //    }
        //    else
        //    {
        //        if (i == 0)
        //        {
        //            lblreceivemsg.Text = "Please Checked atleast 1 checkbox.";
        //            lblreceivemsg.CssClass = "errorMsg";
        //        }
        //    }
        //}

        //gvNonRecievedJobDetail.AllowPaging = true;//Checkbox
        //gvNonRecievedJobDetail.DataBind();//Checkbox
    }

    protected void BtnFreight_Click(object sender, EventArgs e)
    {
        int i = 0;
        int j = 0;
        bool flag = true;
        string Customer = "";
        lblfreighterror.Text = "";
        //drpjobno.Items.Clear();
        //drpjobno.DataBind();
        //ListItem lst1 = new ListItem("Select", "0");
        //drpjobno.Items.Insert(0, lst1);
        RememberOldValues();
        RePopulateValues();
        //gvRecievedJobDetail.AllowPaging = false;
        //gvRecievedJobDetail.DataBind();
        foreach (GridViewRow gvr in gvRecievedJobDetail.Rows)
        {

            if (((CheckBox)gvr.FindControl("Chk1")).Checked)
            {

                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");

                ////  int JobId = Convert.ToInt32(Recv.CommandArgument);
                //  string[] commandArgs = gvr.ToString().Split(new char[] { ';' });
                //  string strCustDocFolder = "", strJobFileDir = "", strCustomer = "", strJobrefno = "", strRMSNONRMS = "";
                //  int JobId = Convert.ToInt32(commandArgs[0].ToString());
                //  hdnJobId.Value = commandArgs[0].ToString();
                //  Session["JobId"] = commandArgs[0].ToString();
                //  ViewState["JobId"] = Session["JobId"];

                //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                ViewState["Currentid"] = RowIndex;
                string[] commandArgs = Recv.CommandArgument.ToString().Split(new char[] { ';' });
                string strCustDocFolder = "", strJobFileDir = "", strCustomer = "", strJobrefno = "", strRMSNONRMS = "";
                int JobId = Convert.ToInt32(commandArgs[0].ToString());
                hdnJobId.Value = commandArgs[0].ToString();
                Session["JobId"] = commandArgs[0].ToString();
                ViewState["JobId"] = Session["JobId"];
                DataSet dsDetail = BillingOperation.GetFreightDetail(Convert.ToInt32(hdnJobId.Value));
                i++;
                if (dsDetail.Tables[0].Rows.Count > 0)
                {
                    txtjobno.Text = dsDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
                    ModalPopupExtender2.Show();

                }
                else
                {
                    j = -1;

                }


            }
            else
            {
                if (i == 0)
                {
                    lblreceivemsg.Text = "Please Checked atleast 1 checkbox.";
                    lblreceivemsg.CssClass = "errorMsg";
                }
                else if (i > 1)
                {
                    lblreceivemsg.Text = "Please Select only 1 Job.";
                    lblreceivemsg.CssClass = "errorMsg";
                    ModalPopupExtender2.Hide();
                }

                else if (j == -1)
                {
                    lblreceivemsg.Text = "Freight Job Not Found.";
                    lblreceivemsg.CssClass = "errorMsg";
                    ModalPopupExtender2.Hide();
                }

                else
                {
                    lblreceivemsg.Text = "";

                    ModalPopupExtender2.Show();
                }
            }
        }
        gvRecievedJobDetail.AllowPaging = true;
        gvRecievedJobDetail.DataBind();
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        int Result = 0;

        if (txtjobno.Text == "")
        {
            lblfreighterror.Text = "Please Select Job First!.";
            lblfreighterror.CssClass = "errorMsg";
            ModalPopupExtender2.Show();
        }
        else
        {

            //  ViewState["Jobid"] = "";
            //string[] arrItems = new string[drpjobno.Items.Count];
            //if (drpjobno.Items.Count > 0)
            //{
            //    for (int i = 0; i < drpjobno.Items.Count; i++)
            //    {
            //        if (drpjobno.Items[i].Selected == false)
            //        {

            //            ViewState["Jobid"] = ViewState["Jobid"].ToString() + ',' + drpjobno.Items[i].Value.ToString();

            //        }

            //    }
            //}

            //ModalPopupExtender2.Show();

            //lblfreighterror.Text = drpjobno.SelectedItem.Text + '-' + drpjobno.SelectedItem.Value;
            //lblreceivemsg.Text = ViewState["Jobid"].ToString();
            int jobid = Convert.ToInt32(Session["JobId"].ToString());
            Result = BillingOperation.freightJob(jobid, LoggedInUser.glUserId, txtjobno.Text, Convert.ToInt16(EnumBilltype.BillingScrutiny));

            if (Result == 0)
            {

                lblreceivemsg.Text = "This Jobs Billed In Freight JobNo: <b>" + txtjobno.Text + "</b> Successfully !";
                lblreceivemsg.CssClass = "success";
                gvRecievedJobDetail.DataBind();
                gvRecievedJobDetail.DataBind();
            }
            else if (Result == 1)
            {
                lblreceivemsg.Text = "System Error! Please try after sometime!";
                lblreceivemsg.CssClass = "errorMsg";
            }

        }
    }

    #endregion

    #region Received

    protected void lnkreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "BillingScrutiny_ReceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        receivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void receivejoblistExport(string header, string contentType)
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
        gvRecievedJobDetail.Caption = "";

        gvRecievedJobDetail.Columns[1].Visible = false;
        gvRecievedJobDetail.Columns[12].Visible = false;
        gvRecievedJobDetail.Columns[13].Visible = false;
        //gvRecievedJobDetail.Columns[11].Visible = false;

        gvRecievedJobDetail.DataSourceID = "PCDReceivedSqlDataSource";
        gvRecievedJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvRecievedJobDetail);

        gvRecievedJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gvRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
        }
    }

    protected void gvRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label status1 = (Label)e.Row.FindControl("rulename");

            string status = status1.Text.ToString();
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
                e.Row.BackColor = System.Drawing.Color.BurlyWood;
                e.Row.ToolTip = "Quick Paymaster";
            }

            if (status == "6")
            {
                e.Row.BackColor = System.Drawing.Color.Khaki;
                e.Row.ToolTip = "High Credit Days";
            }
            if (status == "7")
            {
                e.Row.BackColor = System.Drawing.Color.LightSeaGreen;
                e.Row.ToolTip = "amount";
            }
            if (status == "8")
            {
                e.Row.BackColor = System.Drawing.Color.LightSlateGray;
                e.Row.ToolTip = "User Days";
            }

            foreach (DataControlField col in gvRecievedJobDetail.Columns)
            {
                //if (col.HeaderText == "")
                //{
                //    col.Visible = false;

                //}
                e.Row.Cells[09].ToolTip = "Today - FSB"; // Aging1
                e.Row.Cells[10].ToolTip = "Today - FRB"; // Aging2
                //e.Row.Cells[11].ToolTip = "Today - Clearance Date"; // Aging3
                e.Row.Cells[12].CssClass = "hidden";
                e.Row.Cells[13].CssClass = "hidden";
            }

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[11].ToolTip = "Today - Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[6].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[11].ToolTip = "Today- Job came in Additional Tab.";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "transport")
                {
                    e.Row.Cells[11].ToolTip = "Today - Job created on.";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else
                {
                    e.Row.Cells[11].ToolTip = "Today - Clearance Date.";
                    e.Row.Cells[7].Text = "";
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[12].CssClass = "hidden";
            e.Row.Cells[13].CssClass = "hidden";
        }

    }

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            lblerror.Text = "";
            lblMsgforReceived.Text = "";
            //  lblMsgforNonReceived.Text = "";
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            ViewState["Currentid"] = RowIndex;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "", strCustomer = "", strJobrefno = "", strRMSNONRMS = "";
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            ViewState["JobId"] = Session["JobId"];
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";

            if (commandArgs[3].ToString() != "")
                strCustomer = commandArgs[3].ToString();
            if (commandArgs[4].ToString() != "")
                strJobrefno = commandArgs[4].ToString();
            if (commandArgs[5].ToString() != "")
                strRMSNONRMS = commandArgs[5].ToString();
            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;
            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);

            /// Cotract checking swamini
            Get_ContractChecking(Convert.ToInt32(Session["JobId"]));
            /////////////////////////////////

            rptDocument.DataSource = BillingOperation.FillPCDDocumentForRecieved(Convert.ToInt32(hdnJobId.Value), PCDDocType);
            rptDocument.DataBind();
            string JobType = Convert.ToString(gvRecievedJobDetail.Rows[RowIndex].Cells[5].Text.Trim());
            if (JobType != "" && JobType.ToLower().Trim() == "additional")
            {
                rptBJVDetails.DataSource = DBOperations.FillExpenseDetails(Convert.ToInt32(hdnJobId.Value));
                rptBJVDetails.DataBind();
            }
            else
            {
                rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(hdnJobId.Value));
                rptBJVDetails.DataBind();
            }

            hdnJobType.Value = gvRecievedJobDetail.Rows[RowIndex].Cells[5].Text.ToString();
            ModalPopupDocument.Show();
            Label label1 = rptDocument.Controls[0].FindControl("lbljobrefno2") as Label;
            label1.Text = strJobrefno;
            Session["JobRefNo"] = label1.Text;
            Label label2 = rptDocument.Controls[0].FindControl("lblcustomer2") as Label;
            label2.Text = strCustomer;
            //Label label3 = rptDocument.Controls[0].FindControl("lblRMSNonRms2") as Label;
            //label3.Text = strRMSNONRMS;
        }



    }

    protected void rpDocument_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBoxList chkDuplicate = (CheckBoxList)e.Item.FindControl("chkDuplicate");
            CustomValidator CVCheckBoxList = (CustomValidator)e.Item.FindControl("CVCheckBoxList");
            chkDuplicate.Items[1].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsCopy"));
            chkDuplicate.Items[0].Selected = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "IsOriginal"));
            chkDuplicate.Items[1].Enabled = false;
            chkDuplicate.Items[0].Enabled = false;
        }
    }

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

    protected void rptBJVDetails_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Label lblDEBITAMT = ((Label)e.Item.FindControl("lblDEBITAMT"));
            if (lblDEBITAMT.Text == "")
                lblDEBITAMT.Text = "0";
            ViewState["lblDEBITAMT"] = Convert.ToInt64(ViewState["lblDEBITAMT"]) + int.Parse(lblDEBITAMT.Text);



            //Label lblCREDITAMT = ((Label)e.Item.FindControl("lblCREDITAMT"));            
            //ViewState["lblCREDITAMT"] = Convert.ToInt64(ViewState["lblCREDITAMT"]) + int.Parse(lblCREDITAMT.Text);

            //Label lblAMOUNT = ((Label)e.Item.FindControl("lblAMOUNT"));
            //ViewState["lblAMOUNT"] = Convert.ToInt64(ViewState["lblAMOUNT"]) + int.Parse(lblAMOUNT.Text);        
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            ((Label)e.Item.FindControl("lbltotDebitamt")).Text = ViewState["lblDEBITAMT"].ToString();
            //((Label)e.Item.FindControl("lbltotCREDITAMT")).Text = ViewState["lblCREDITAMT"].ToString();
            //((Label)e.Item.FindControl("lbltotAMOUNT")).Text = ViewState["lblAMOUNT"].ToString();           
        }
    }

    protected void lnkApprove_click(object sender, EventArgs e)
    {
        int result = 0;
        lblerror.Text = "";
        int JobId = Convert.ToInt32(Session["JobId"]);
        int reasonforPendency = 0;
        string interests = string.Empty;
        string JobRefNo = "";

        LinkButton btnsubmit = sender as LinkButton;
        bool bApprove = true;

        string JobType = hdnJobType.Value;
        if (JobType.ToLower().Trim() == "additional")
        {
            result = BillingOperation.AD_ApproveRejectScrutiny(JobId, bApprove, "", LoggedInUser.glUserId, reasonforPendency, "");
        }
        else if (JobType.ToLower().Trim() == "pn movement")
        {
            result = CMOperations.ApproveRejectScrutiny(JobId, bApprove, "", reasonforPendency, "", LoggedInUser.glUserId);
        }
        else
        {
            LinkButton jobrefNo = (LinkButton)gvRecievedJobDetail.Rows[Convert.ToInt16(ViewState["Currentid"])].FindControl("lnkJobNo");
            JobRefNo = jobrefNo.Text;
            Session["JobRefNo"] = JobRefNo;
            GetModuleId();

            result = BillingOperation.ApproveRejectScrutiny(JobId, bApprove, "", reasonforPendency, "", 0, LoggedInUser.glUserId, ModuleId);
            //Session["JobRefNo"] = "";

            if (JobType.Trim().ToLower() == "export")
            {
                DataTable dtContractCheck = new DataTable();
                string ContractCheck = DBOperations.Get_EXContractChecking(JobId);
                lblMessage.CssClass = "success";
                if (ContractCheck == "0")
                {
                    Session["Contractchk"] = "No";
                }
                else
                {
                    Session["Contractchk"] = "Yes";
                }
            }

            ///  Added LT Data by swamini 19 Jan 2023
            if (Session["Contractchk"] == "Yes")
            {
                SaveBilling_Invoice(JobId);
            }
            ////////////////////

        }

        if (result == 0)
        {
            LinkButton jobrefNo = (LinkButton)gvRecievedJobDetail.Rows[Convert.ToInt16(ViewState["Currentid"])].FindControl("lnkJobNo");
            nextclick();
            lblMsgforApproveReject.Text = "Billing Scrutiny Completed for Job No:-" + jobrefNo.Text + " ! Job Moved To Draft Invoice!.";
            lblMsgforApproveReject.CssClass = "success";
            gvRecievedJobDetail.DataBind();
            ModuleId = 0;
        }
        else if (result == 1)
        {
            lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
            lblMsgforApproveReject.CssClass = "errorMsg";
        }
        else if (result == 2)
        {
            lblMsgforApproveReject.Text = "Billing Scrutiny Already Completed!";
            lblMsgforApproveReject.CssClass = "errorMsg";
        }
    }

    protected void lnkReject_click(object sender, EventArgs e)
    {
        lblMsgforReceived.Text = "";
        LinkButton btnsubmit = sender as LinkButton;
        Session["status"] = "Reject";
        lblerror.Text = "";
        this.ModalPopupExtender1.Show();


        foreach (RepeaterItem ri in rpReason.Items)
        {
            CheckBox chkreason = (CheckBox)ri.FindControl("chkreasonofpendency");
            TextBox txtreason = (TextBox)ri.FindControl("txtReason");
            DropDownList Drplrdctype = (DropDownList)ri.FindControl("Drplrdctype");
            Label lbltype = (Label)ri.FindControl("lbltype");
            //Label lblReason = (Label)ri.FindControl("lblReason");
            Label lblmailto = (Label)ri.FindControl("lblmailto");
            TextBox txtUser = (TextBox)ri.FindControl("txtUser");
            HiddenField hnduserid = (HiddenField)ri.FindControl("hdnUserId");
            txtUser.Visible = true;
            lblmailto.Visible = true;
            txtUser.Text = "";
            hnduserid.Value = "";
            if (chkreason.Checked == true)
            {
                chkreason.Checked = false;
                txtreason.Text = "";
                Drplrdctype.SelectedIndex = 0;
            }
        }
    }

    protected void btnOkay_Click(object sender, EventArgs e)
    {
        lblerror.Text = "";
        ViewState["reasonforPendency"] = "";
        int result = 0;
        int result1 = 0;
        lblerror.Text = "";
        lblerror.CssClass = "";
        int reasonforPendency = 0;
        int checkedCount = 0;
        string interests = string.Empty;
        // string reasonforPendency1 = "";
        string Remark = "";
        string LRDC_Type = "";
        string Mailtoid = "", JobRefNo = "";
        bool IsLRPending = false;

        if (Session["status"].ToString() == "Reject")
        {
            bool bApprove = false;
            for (int i = 0; i < rpReason.Items.Count; i++)
            {
                RequiredFieldValidator RFVRejectReason = (RequiredFieldValidator)rpReason.Items[i].FindControl("RFVRejectReason");

                CheckBox chk = (CheckBox)rpReason.Items[i].FindControl("chkreasonofpendency");
                if (chk.Checked)
                {

                    if (RFVRejectReason != null)
                    {
                        RFVRejectReason.Enabled = true;
                    }
                    checkedCount += chk.Checked ? 1 : 0;
                    TextBox txtReason = (TextBox)rpReason.Items[i].FindControl("txtReason");
                    HiddenField hdnDocId = (HiddenField)rpReason.Items[i].FindControl("hdnDocId");
                    DropDownList Drplrdctype = (DropDownList)rpReason.Items[i].FindControl("Drplrdctype");
                    DropDownList CBOddCategory = (DropDownList)rpReason.Items[i].FindControl("ddCategory");

                    Remark = txtReason.Text;
                    reasonforPendency = Convert.ToInt16(hdnDocId.Value);
                    HiddenField hdnUserId = (HiddenField)rpReason.Items[i].FindControl("hdnUserId");
                    if (chk.Text == "Others")
                    {
                        Mailtoid = hdnUserId.Value;
                    }
                    else
                    {
                        Mailtoid = "0";
                    }

                    if (chk.Text == "LR/DC")
                    {
                        IsLRPending = true;
                        LRDC_Type = Drplrdctype.SelectedValue;
                    }
                    else
                    {
                        LRDC_Type = "0";
                    }

                    if (checkedCount != 0)
                    {
                        int CategotyId = 0;

                        CategotyId = Convert.ToInt32(CBOddCategory.SelectedValue);

                        JobRefNo = Session["JobRefNo"].ToString();

                        GetModuleId();

                        result = BillingOperation.ApproveRejectScrutiny(Convert.ToInt32(ViewState["JobId"]), bApprove, Remark, Convert.ToInt16(reasonforPendency), LRDC_Type, CategotyId, LoggedInUser.glUserId, ModuleId);

                        if (result == 0)
                        {
                            if (IsLRPending == true)
                            {
                                int result_LR = DBOperations.AddAdviceToLRPending(Convert.ToInt32(ViewState["JobId"]), 0, LoggedInUser.glUserId);
                            }

                            ModalPopupExtender1.Hide();
                            lblMsgforApproveReject.Text = "Bill Scrutiny Rejected! Job Moved Back To Bill Rejected!.";
                            lblMsgforApproveReject.CssClass = "success";
                            //reasonforPendency1 = null;
                        }
                        else if (result == 1)
                        {
                            lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                        }
                        else if (result == 2)
                        {
                            lblMsgforApproveReject.Text = "Bill Scrutiny Rejected Already Pending!";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                            ModalPopupExtender1.Hide();
                        }//if (result == 0)

                    }//if (checkedCount != 0 && checkedCount != null)
                    //}// if (Remark == "")
                }// if (chk.Checked)
            }//for (int i = 0; i < rpReason.Items.Count; i++)



            if (checkedCount == 0)
            {
                ModalPopupExtender1.Show();
                lblerror.Text = "Please select atleast 1 checkbox";
                lblerror.CssClass = "errorMsg";
            }

            if (ModuleId != 2)
            {
                int rejectedby = Convert.ToInt16(EnumBilltype.BillingScrutiny);
                result1 = BillingOperation.Rejectmailnotification(Convert.ToInt32(ViewState["JobId"]), LoggedInUser.glUserId, rejectedby, Convert.ToInt16(Session["FinYearId"]), Convert.ToString(Mailtoid));
                if (result1 != 0)
                {
                    lblMsgforApproveReject.Text = "System Error in Mail Sending! Please try after sometime.";
                    lblMsgforApproveReject.CssClass = "errorMsg";
                }
            }
            ModuleId = 0;
        }

        gvRecievedJobDetail.DataBind();

    }

    protected void rpReason_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        TextBox txtUser = (TextBox)e.Item.FindControl("txtUser");
        int count = 2341;
        foreach (RepeaterItem ri in rpReason.Items)
        {
            AjaxControlToolkit.AutoCompleteExtender UserExtender = (AjaxControlToolkit.AutoCompleteExtender)ri.FindControl("UserExtender");

            //txtUser = (TextBox)ri.FindControl("txtUser"); 

            HtmlControl divwidthCust = (HtmlControl)ri.FindControl("divwidthCust");

            if (txtUser != null)
            {
                if (txtUser.Visible != false)
                {
                    UserExtender.TargetControlID = txtUser.ClientID;
                }
                UserExtender.BehaviorID = divwidthCust.ClientID;

                UserExtender.ContextKey = count.ToString();//LoggedInUser.glUserId.ToString();
                count = count + 1;
            }
        }

        CheckBox chkReasonType = (CheckBox)e.Item.FindControl("chkreasonofpendency");
        RequiredFieldValidator RFVRejectReason = (RequiredFieldValidator)e.Item.FindControl("RFVRejectReason");
        TextBox txtReason = (TextBox)e.Item.FindControl("txtReason");

        //  Label lblReason = (Label)e.Item.FindControl("lblReason");
        Label lbltype = (Label)e.Item.FindControl("lbltype");
        DropDownList Drplrdctype = (DropDownList)e.Item.FindControl("Drplrdctype");

        Label lblMailTo = (Label)e.Item.FindControl("lblMailTo");

        HiddenField hdnUserId = (HiddenField)e.Item.FindControl("hdnUserId");

        // Reject Department Category
        DropDownList cboDDCategory = (DropDownList)e.Item.FindControl("ddCategory");
        RequiredFieldValidator cboRFVCatgory = (RequiredFieldValidator)e.Item.FindControl("RFVCatgory");

        if (chkReasonType != null && RFVRejectReason != null)
        {
            chkReasonType.Attributes.Add("OnClick", "javascript:toggleDiv('" + chkReasonType.ClientID + "','" + cboDDCategory.ClientID + "','" + cboRFVCatgory.ClientID + "','" + txtReason.ClientID + "','" + lbltype.ClientID + "','" + Drplrdctype.ClientID + "','" + RFVRejectReason.ClientID + "','" + chkReasonType.Text + "','" + txtUser.ClientID + "','" + hdnUserId.ClientID + "','" + lblMailTo.ClientID + "');");

            //  lblReason.Style.Add("display", "none");
            txtReason.Style.Add("display", "none");
            cboDDCategory.Style.Add("display", "none");
            lbltype.Style.Add("display", "none");
            Drplrdctype.Style.Add("display", "none");
            txtUser.Style.Add("display", "none");
            lblMailTo.Style.Add("display", "none");

        }
    }

    protected void btnPrevious_click(object sender, EventArgs e)
    {
        btnNext.Visible = true;
        int i = Convert.ToInt16(ViewState["Currentid"]) - 1;
        int count = gvRecievedJobDetail.Rows.Count - 1;
        if (i < 0)
        {
            i = count;
        }
        ViewState["Currentid"] = i;

        string JobId = gvRecievedJobDetail.DataKeys[i].Value.ToString();
        Session["JobId"] = JobId;
        string customer = gvRecievedJobDetail.Rows[i].Cells[4].Text.ToString();
        string RmsNonRMS = gvRecievedJobDetail.Rows[i].Cells[12].Text.ToString();
        hdnJobType.Value = gvRecievedJobDetail.Rows[i].Cells[5].Text.ToString();
        LinkButton jobrefNo = (LinkButton)gvRecievedJobDetail.Rows[i].FindControl("lnkJobNo");
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        rptDocument.DataSource = BillingOperation.FillPCDDocumentForRecieved(Convert.ToInt32(JobId), PCDDocType);
        rptDocument.DataBind();
        rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(JobId));
        rptBJVDetails.DataBind();
        ModalPopupDocument.Show();
        Label label1 = rptDocument.Controls[0].FindControl("lbljobrefno2") as Label;
        label1.Text = jobrefNo.Text;
        Label label2 = rptDocument.Controls[0].FindControl("lblcustomer2") as Label;
        label2.Text = customer;
        //Label label3 = rptDocument.Controls[0].FindControl("lblRMSNonRms2") as Label;
        //label3.Text = RmsNonRMS;
    }

    protected void btnNext_click(object sender, EventArgs e)
    {
        nextclick();
    }

    public void nextclick()
    {
        lblMsgforReceived.Text = "";
        btnPrevious.Visible = true;
        int i = Convert.ToInt16(ViewState["Currentid"]) + 1;
        int count = gvRecievedJobDetail.Rows.Count;
        if (count == i)
        {
            i = 0;
        }
        ViewState["Currentid"] = i;
        string JobId = gvRecievedJobDetail.DataKeys[i].Value.ToString();
        Session["JobId"] = JobId;
        string customer = gvRecievedJobDetail.Rows[i].Cells[4].Text.ToString();
        string RmsNonRMS = gvRecievedJobDetail.Rows[i].Cells[12].Text.ToString();
        hdnJobType.Value = gvRecievedJobDetail.Rows[i].Cells[5].Text.ToString();
        LinkButton jobrefNo = (LinkButton)gvRecievedJobDetail.Rows[i].FindControl("lnkJobNo");

        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);

        rptDocument.DataSource = BillingOperation.FillPCDDocumentForRecieved(Convert.ToInt32(JobId), PCDDocType);
        rptDocument.DataBind();
        rptBJVDetails.DataSource = BillingOperation.FillBJVDetails(Convert.ToInt32(JobId));
        rptBJVDetails.DataBind();

        if (count == 1)
        {
            ModalPopupDocument.Hide();
        }
        else
        {
            ModalPopupDocument.Show();
        }

        Label label1 = rptDocument.Controls[0].FindControl("lbljobrefno2") as Label;
        label1.Text = jobrefNo.Text;
        Label label2 = rptDocument.Controls[0].FindControl("lblcustomer2") as Label;
        label2.Text = customer;
        //Label label3 = rptDocument.Controls[0].FindControl("lblRMSNonRms2") as Label;
        //label3.Text = RmsNonRMS;
        gvRecievedJobDetail.DataBind();
    }

    #endregion

    #region Data Filter1

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                DataFilter2_OnDataBound();
            }
            //else
            //{
            //    DataFilter2_OnDataBound();
            //}
        }
    }

    void DataFilter1_OnDataBound()
    {

        try
        {
            //DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            //DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            //DataFilter1.FilterDataSource();
            //gvNonRecievedJobDetail.DataBind();
            //if (gvNonRecievedJobDetail.Rows.Count == 0)
            //{
            //    lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Billing Scrutiny!";
            //    lblMsgforNonReceived.CssClass = "errorMsg";
            //}
            //else
            //{
            //    lblMsgforNonReceived.Text = "";
            //}


        }
        catch (Exception ex)
        {
            //DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }


    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.FilterDataSource();
            gvRecievedJobDetail.DataBind();
            if (gvRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforReceived.Text = "No Job Found For Recieved file for Billing Scrutiny!";
                lblMsgforReceived.CssClass = "errorMsg";
            }
            else
            {
                lblMsgforReceived.Text = "";
            }

        }
        catch (Exception ex)
        {
            //DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Print Expense Non Receive
    protected void btnPrintExpenseNonRCV_Click(object sender, EventArgs e)
    {
        int i = 0;
        string strJobList = "";

        //foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chk1")).Checked)
        //    {
        //        LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");

        //        string[] commmandArgs = Recv.CommandArgument.ToString().Split(new char[] { ';' });

        //        string JobId = commmandArgs[0].ToString();

        //        strJobList = strJobList + JobId + ",";

        //        i++;
        //    }
        //    else
        //    {
        //        if (i == 0)
        //        {
        //            lblreceivemsg.Text = "Please Checked atleast 1 checkbox to Print BJV detail.";
        //            lblreceivemsg.CssClass = "errorMsg";
        //        }
        //    }
        //}

        if (strJobList != "")
        {
            Session["BJVJobList"] = strJobList;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BJV PRINT", "window.open('PrintBJV.aspx');", true);

        }
    }

    #endregion

    #region Print Expense Receive

    protected void btnPrintExpense_Click(object sender, EventArgs e)
    {
        int i = 0;
        string strJobList = "";
        //gvRecievedJobDetail.AllowPaging = false;//Checkbox
        //gvRecievedJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvRecievedJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkPrint")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");

                string[] commmandArgs = Recv.CommandArgument.ToString().Split(new char[] { ';' });

                string JobId = commmandArgs[0].ToString();

                strJobList = strJobList + JobId + ",";

                i++;
            }
            else
            {
                if (i == 0)
                {
                    //lblreceivemsg.Text = "Please Checked atleast 1 checkbox to Print BJV detail.";
                    //lblreceivemsg.CssClass = "errorMsg";
                }
            }
        }

        if (strJobList != "")
        {
            Session["BJVJobList"] = strJobList;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "BJV PRINT", "window.open('PrintBJV.aspx');", true);

        }
    }

    #endregion

    protected void TabJobDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        //if (TabJobDetail.ActiveTabIndex == 0)
        //{
        //    Session["CHECKED_ITEMS"] = null; //Checkbox
        //    lblreceivemsg.Text = "";
        //    lblMsgforApproveReject.Text = "";
        //    DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
        //    DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
        //    DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
        //    DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //    gvNonRecievedJobDetail.DataBind();

        //}
        if (TabJobDetail.ActiveTabIndex == 0)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            //lblreceivemsg.Text = "";
            lblMsgforApproveReject.Text = "";
            DataFilter2.DataSource = PCDReceivedSqlDataSource;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvRecievedJobDetail.DataBind();

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
            else if (lblpriorities.Text == "Quick Paymaster")
            {
                txtcolor.BackColor = System.Drawing.Color.Bisque;
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void GetModuleId()
    {
        DataTable dtModule = BillingOperation.GetModuleId(Session["JobRefNo"].ToString());
        if (dtModule.Rows.Count > 0)
        {
            foreach (DataRow row in dtModule.Rows)
            {
                ModuleId = Convert.ToInt32(row["MODULEID"].ToString());
            }
        }
        else
        {
            ModuleId = LoggedInUser.glModuleId;
        }
    }

    #region writeLTData
    #region Contract Checking
    protected void Get_ContractChecking(int JobId)
    {
        int strJobId = JobId;
        int count1 = 0;
        DataTable dtContractCheck = new DataTable();
        lblMessage.Text = DBOperations.Get_ContractChecking_withexpired(strJobId);
        lblMessage.CssClass = "success";
        if (lblMessage.Text == "")
        {
            Session["Contractchk"] = "Yes";
        }
        else
        {
            Session["Contractchk"] = "No";
        }
        //count1 = DBOperations.Get_ContractChecking_withexpired(strJobId);
        //if (count1 == 0)
        //{
        //    lblMessage.Text = "No Contract Is Available In System.";
        //    lblMessage.CssClass = "success";
        //}
    }
    #endregion
    // Add Invoice code by swamini 19 Jan 2023
    SqlConnection OpenDB = CDatabase.getConnection();
    DataTable dtInvoiceMstData = new DataTable();
    DataTable dtInvoiceDetData = new DataTable();
    protected void Get_JobDetailForContractBilling(int JobId)
    {
        int strJobId = JobId;
        DataSet dtbilling = DBOperations.GetJobDetailForContractBilling(JobId);
        //if (dtbilling.Tables[0].Rows.Count > 0)
        //{
        //    dtStartDate.Text = dtbilling.Tables[0].Rows[0][1].ToString();
        //    dtEndDate.Text = dtbilling.Tables[0].Rows[0][2].ToString();
        //    txtconsigneename.Text = dtbilling.Tables[0].Rows[0][3].ToString();
        //}
        dtInvoiceMstData = dtbilling.Tables[2];
        //if (dtbilling.Tables[1].Rows.Count > 0)
        //{
        //    grdbillinglinedetails.DataSource = dtbilling.Tables[1];
        //    grdbillinglinedetails.DataBind();
        //}
        dtInvoiceDetData = dtbilling.Tables[1];
    }
    protected void SaveBilling_Invoice(int jobid)
    {
        try
        {
            hdnJobId.Value = jobid.ToString();
            dtInvoiceDetData.Rows.Clear();
            dtInvoiceDetData.Columns.Clear();
            dtInvoiceMstData.Rows.Clear();
            dtInvoiceMstData.Columns.Clear();
            GetModuleId();
            if (ModuleId == 1)
            {
                Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));
            }
            else if (ModuleId == 5)
            {
                Get_EXJobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));
            }
            //Get_JobDetailForContractBilling(Convert.ToInt32(hdnJobId.Value));
            if (dtInvoiceDetData.Rows.Count > 0)
            {
                for (int i = 0; i < dtInvoiceMstData.Rows.Count; i++)
                {
                    DBOperations.DisconnectSQL(OpenDB);
                    string IsGST = string.Empty;
                    OpenDB.Open();
                    SqlCommand cmd = new SqlCommand("[usp_Inserttbl_InvoiceMst]", OpenDB) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@InvId", null);
                    cmd.Parameters.AddWithValue("@InvoiceNo", dtInvoiceMstData.Rows[i]["InvoiceNo"].ToString());
                    //cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["InvoiceDate"].ToString()).ToString("dd/MMM/yyyy"));;
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoiceMstData.Rows[i]["InvoiceDate"].ToString());
                    cmd.Parameters.AddWithValue("@ConsigneeName", dtInvoiceMstData.Rows[i]["ConsigneeName"].ToString());
                    cmd.Parameters.AddWithValue("@JobNo", dtInvoiceMstData.Rows[i]["JobNo"].ToString());
                    cmd.Parameters.AddWithValue("@BLNo", dtInvoiceMstData.Rows[i]["BL No"].ToString());
                    //cmd.Parameters.AddWithValue("@BLDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["BL Date"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@BLDate", dtInvoiceMstData.Rows[i]["BL Date"].ToString());
                    cmd.Parameters.AddWithValue("@ShipperName", dtInvoiceMstData.Rows[i]["ShipperName"].ToString());
                    cmd.Parameters.AddWithValue("@Mode", dtInvoiceMstData.Rows[i]["Mode"].ToString());
                    //cmd.Parameters.AddWithValue("@DispatchDate", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["DispatchDate"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@DispatchDate", dtInvoiceMstData.Rows[i]["DispatchDate"].ToString());
                    cmd.Parameters.AddWithValue("@ContainerType", dtInvoiceMstData.Rows[i]["ContainerType"].ToString());
                    cmd.Parameters.AddWithValue("@TotalQty", dtInvoiceMstData.Rows[i]["TotalQty"].ToString());
                    cmd.Parameters.AddWithValue("@TotalAmt", dtInvoiceMstData.Rows[i]["TotalAmt"].ToString());
                    cmd.Parameters.AddWithValue("@GSTAmt", dtInvoiceMstData.Rows[i]["GSTAmt"].ToString());
                    cmd.Parameters.AddWithValue("@PayableAmt", dtInvoiceMstData.Rows[i]["PayableAmt"].ToString());
                    cmd.Parameters.AddWithValue("@Job_Type", dtInvoiceMstData.Rows[i]["Job_Type"].ToString());
                    cmd.Parameters.AddWithValue("@BE_No", dtInvoiceMstData.Rows[i]["BE_No"].ToString());
                    //cmd.Parameters.AddWithValue("@BE_Date", Convert.ToDateTime(dtInvoiceMstData.Rows[i]["BE_Date"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@BE_Date", dtInvoiceMstData.Rows[i]["BE_Date"].ToString());
                    cmd.Parameters.AddWithValue("@Port_Of_Discharge", dtInvoiceMstData.Rows[i]["Port_Of_Discharge"].ToString());
                    cmd.Parameters.AddWithValue("@InvoiceNumber", dtInvoiceMstData.Rows[i]["NumericInvNo"].ToString());
                    cmd.Parameters.AddWithValue("@FABookId", dtInvoiceMstData.Rows[i]["fabookid"].ToString());
                    cmd.Parameters.AddWithValue("@AddedBy", LoggedInUser.glUserId.ToString());
                    cmd.Parameters.AddWithValue("@AddedOn", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@ModifiedBy", LoggedInUser.glUserId.ToString());
                    cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.ExecuteNonQuery();
                    DBOperations.DisconnectSQL(OpenDB);

                    if (dtInvoiceMstData.Rows[i]["ContractLID"].ToString() != "")
                    {
                        var var1 = dtInvoiceMstData.Rows[i]["ContractLID"].ToString().Split('-');
                        for (int j = 0; j < var1.Length; j++)
                        {
                            string sql = "update CB_BillingDetail set IsBillDone = 'Y' where CMID = '" + var1[j].ToString() + "'";
                            DBOperations.InsertDeleteCommand(sql);
                        }
                    }
                }

                for (int i = 0; i < dtInvoiceDetData.Rows.Count; i++)
                {
                    DBOperations.DisconnectSQL(OpenDB);
                    OpenDB.Open();
                    SqlCommand cmd = new SqlCommand("[usp_Inserttbl_InvoiceDet]", OpenDB) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@InvDetId", null);
                    cmd.Parameters.AddWithValue("@InvoiceNo", dtInvoiceDetData.Rows[i]["InvoiceNo"].ToString());
                    //cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(dtInvoiceDetData.Rows[i]["InvoiceDate"].ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@InvoiceDate", dtInvoiceDetData.Rows[i]["InvoiceDate"].ToString());
                    cmd.Parameters.AddWithValue("@chargecode", dtInvoiceDetData.Rows[i]["chargecode"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Particulars", dtInvoiceDetData.Rows[i]["Particulars"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Rate", dtInvoiceDetData.Rows[i]["Rate"].ToString());
                    cmd.Parameters.AddWithValue("@Qty", dtInvoiceDetData.Rows[i]["Qty"].ToString());
                    cmd.Parameters.AddWithValue("@GST", dtInvoiceDetData.Rows[i]["GST"].ToString());
                    cmd.Parameters.AddWithValue("@Amt", dtInvoiceDetData.Rows[i]["Amt"].ToString());
                    cmd.Parameters.AddWithValue("@AddedBy", LoggedInUser.glUserId.ToString());
                    cmd.Parameters.AddWithValue("@AddedOn", Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@ModifiedBy", LoggedInUser.glUserId.ToString());
                    cmd.Parameters.AddWithValue("@ModifiedOn", Convert.ToDateTime(DateTime.Now.ToString()).ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@AmountWithOutGST", dtInvoiceDetData.Rows[i]["AmountWithoutGST"].ToString());
                    cmd.Parameters.AddWithValue("@cblid", dtInvoiceDetData.Rows[i]["cblid"].ToString());
                    cmd.Parameters.AddWithValue("@JobNo", Session["JobRefNo"].ToString());
                    cmd.ExecuteNonQuery();
                    DBOperations.DisconnectSQL(OpenDB);
                }
                dtInvoiceDetData.Rows.Clear();
                dtInvoiceDetData.Columns.Clear();
                dtInvoiceMstData.Rows.Clear();
                dtInvoiceMstData.Columns.Clear();
                string message = "Your details have been saved successfully.";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "')};";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
            else
            {
                string message = "Line Item field Mismatch !";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "')};";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SaveData", "1", ex.Message);
            string script = "window.onload = function(){ alert('";
            script += ex.Message + Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
            script += "')};";
            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
        }
    }

    ////
    #endregion

    protected void Get_EXJobDetailForContractBilling(int JobId)
    {
        int strJobId = JobId;
        DataSet dtbilling = DBOperations.EX_GetJobDetailForContractBilling(JobId);
        //if (dtbilling.Tables[0].Rows.Count > 0)
        //{

        //}
        dtInvoiceMstData = dtbilling.Tables[2];
        //if (dtbilling.Tables[1].Rows.Count > 0)
        //{
        //    grdbillinglinedetails.DataSource = dtbilling.Tables[1];
        //    grdbillinglinedetails.DataBind();
        //}
        dtInvoiceDetData = dtbilling.Tables[1];
    }
}