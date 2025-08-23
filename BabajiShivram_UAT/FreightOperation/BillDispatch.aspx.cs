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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Threading;
using System.Net;
using System.ComponentModel;
using Ionic.Zip;
using ClosedXML.Excel;
using MyPacco.API;
public partial class FreightOperation_BillDispatch : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    int ModuleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager1.RegisterPostBackControl(lnknonreceive);
        ScriptManager1.RegisterPostBackControl(lnkreceive);

        //---------start Covering letter---------------
        ScriptManager1.RegisterPostBackControl(btnCoveringLetter);
        ScriptManager1.RegisterPostBackControl(gvcustomer);
        //---------end Covering letter---------------	
        ScriptManager1.RegisterPostBackControl(gvBillDispatchDocDetail);
        ScriptManager1.RegisterPostBackControl(btnSave);

        //DropDownList drp = this.Master.FindControl("ddFinYear") as DropDownList;
        //Session["FinYearId"] = drp.SelectedValue;

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending For Bill Dispatch";

            DataFilter1.DataSource = PCDSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDDispatch.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvNonRecievedJobDetail.DataBind();

            //----------------Start Covering Letter---------------------------
            //DataFilter2.DataSource = PCDSqlDataSource1;
            //DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            //DataFilter2.FilterSessionID = "PCDDispatch1.aspx";
            //DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView		//(DataFilter2_OnDataBound);
            //gvRecievedJobDetail.DataBind();

            DataFilter2.DataSource = PcdSqlDataCustomer;
            DataFilter2.DataColumns = gvcustomer.Columns;
            DataFilter2.FilterSessionID = "PCDDispatch1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvcustomer.DataBind();
            //----------------end Covering Letter---------------------------

        }
        if (TabPCDBilling.TabIndex == 0)
        {
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                lblreceivemsg.Text = "";
                lblMsgforApproveReject.Text = "";
                DataFilter1.DataSource = PCDSqlDataSource;
                DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
                DataFilter1.FilterSessionID = "PCDDispatch.aspx";
                DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            }
        }
        if (TabPCDBilling1.TabIndex == 1)
        {
            if (TabJobDetail.ActiveTabIndex == 1)
            {
                lblreceivemsg.Text = "";
                lblMsgforApproveReject.Text = "";

                //-------------Start Covering Letter------------ 

                //DataFilter2.DataSource = PCDSqlDataSource1;
                //DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
                //DataFilter2.FilterSessionID = "PCDDispatch1.aspx";
                //DataFilter2.OnDataBound += new //DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
                //gvRecievedJobDetail.DataBind();    

                DataFilter2.DataSource = PcdSqlDataCustomer;
                DataFilter2.DataColumns = gvcustomer.Columns;
                DataFilter2.FilterSessionID = "PCDDispatch1.aspx";
                DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);


                //-------------end Covering Letter------------ 

            }
        }
        if (gvNonRecievedJobDetail.Rows.Count == 0)
        {
            lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Bill Dispatch!";
            lblMsgforNonReceived.CssClass = "errorMsg"; ;
        }
        else
        {
            lblMsgforNonReceived.Text = "";
        }


        //-------------start Covering Letter------------ 			
        //if (gvRecievedJobDetail.Rows.Count == 0)
        if (gvcustomer.Rows.Count == 0)
        //-------------end Covering Letter------------ 			

        {
            lblMsgforReceived.Text = "No Job Found For Recieved file for Bill Dispatch!";
            lblMsgforReceived.CssClass = "errorMsg"; ;
        }
        else
        {
            lblMsgforReceived.Text = "";
        }
    }

    #region NonRecieved

    protected void lnkNonreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "BillDispatch_nonreceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvNonRecievedJobDetail.AllowPaging = false;
        gvNonRecievedJobDetail.AllowSorting = false;
        gvNonRecievedJobDetail.Caption = "";

        gvNonRecievedJobDetail.DataSourceID = "PCDSqlDataSource";
        gvNonRecievedJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvNonRecievedJobDetail);

        gvNonRecievedJobDetail.RenderControl(hw);

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

            e.Row.Cells[13].CssClass = "hidden";//rulename
            e.Row.Cells[10].ToolTip = "Today – FSB";//aging1
            e.Row.Cells[11].ToolTip = "Today – FSBD";//aging2
            //e.Row.Cells[10].ToolTip = "Today – Clearance Date";//aging3

            if (DataBinder.Eval(e.Row.DataItem, "JobType") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[11].ToolTip = "Today – Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[7].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[12].ToolTip = "Today – Job added to additional tab.";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Text = "";
                }
                else
                {
                    e.Row.Cells[12].ToolTip = "Today – Clearance Date.";
                    e.Row.Cells[8].Text = "";
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[13].CssClass = "hidden";//rulename
        }

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
    protected void gvNonRecievedJobDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RememberOldValues();
        gvNonRecievedJobDetail.PageIndex = e.NewPageIndex;
        RePopulateValues();


    } //Checkbox

    private void RememberOldValues()
    {
        int countRow = 0;
        ArrayList categoryIDList = new ArrayList();
        int index = -1;
        foreach (GridViewRow row in gvNonRecievedJobDetail.Rows)
        {
            index = Convert.ToInt32(gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value);

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
            foreach (GridViewRow row in gvNonRecievedJobDetail.Rows)
            {
                int index = Convert.ToInt32(gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value);

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

    protected void btnReceive_Click(object sender, EventArgs e)
    {
        lblShowError.Text = "";

        int i = 0;

        RememberOldValues();//Checkbox
        RePopulateValues();//Checkbox
        gvNonRecievedJobDetail.AllowPaging = false;//Checkbox
        gvNonRecievedJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkJobNo");
                //int JobId = Convert.ToInt16(Recv.CommandArgument);
                string JobId = Recv.CommandArgument;
                string s = string.Empty;

                Session["JobRefNo"] = Recv.Text;
               // GetModuleId();
                int Result = BillingOperation.receivedfile(Convert.ToInt32(JobId), LoggedInUser.glUserId, Convert.ToInt16(EnumBilltype.BillDispatch), LoggedInUser.glModuleId);
                Session["JobRefNo"] = "";
                ModuleId = 0;

                if (Result == 0)
                {
                    lblreceivemsg.Text = "File Recieved Successfully!";
                    lblreceivemsg.CssClass = "success";
                    gvNonRecievedJobDetail.DataBind();
                    //-----------------start Covering Letter------------------
                    //gvRecievedJobDetail.DataBind();
                    gvcustomer.DataBind();
                    //-----------------end Covering Letter------------------
                }
                else if (Result == 1)
                {
                    lblreceivemsg.Text = "System Error! Please try after sometime!";
                    lblreceivemsg.CssClass = "errorMsg";
                }
                i++;
            }
            else
            {
                if (i == 0)
                {
                    lblreceivemsg.Text = "Please Checked atleast 1 checkbox.";
                    lblreceivemsg.CssClass = "errorMsg";
                }
            }

        }
        gvNonRecievedJobDetail.AllowPaging = true;//Checkbox
        gvNonRecievedJobDetail.DataBind();//Checkbox
    }

    protected void gvNonRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblShowError.Text = "";
        if (e.CommandName.ToLower() == "ebillone" && e.CommandArgument != null)
        {
            btnEBillUpdate.Visible = true;
            txtEBillRemark.Text = "";
            txtEBillDate.Text = "";

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobReNo = "";

            hdnEBillJobId.Value = commandArgs[0].ToString();

            strJobReNo = commandArgs[1].ToString();

            if (strJobReNo != "")
            {
                DataSet dsEbill = BillingOperation.GetBillingEBill(Convert.ToInt32(hdnEBillJobId.Value));

                if (dsEbill.Tables.Count > 0)
                {
                    if (dsEbill.Tables[0].Rows.Count > 0)
                    {
                        if (dsEbill.Tables[0].Rows[0]["EBillDate"] != DBNull.Value)
                        {
                            txtEBillDate.Text = Convert.ToDateTime(dsEbill.Tables[0].Rows[0]["EBillDate"]).ToString("dd/MM/yyyy");
                            // btnEBillUpdate.Visible = false;
                        }
                        if (dsEbill.Tables[0].Rows[0]["Remark"] != DBNull.Value)
                        {
                            txtEBillRemark.Text = dsEbill.Tables[0].Rows[0]["Remark"].ToString();
                        }
                    }
                }

                lblEBillJobRefNo.Text = strJobReNo;

                ModalPopupEBill.Show();
            }
            else
            {
                lblShowError.Text = "Job Details Not Found!";
                lblShowError.CssClass = "errorMsg";

                // ModalPopupEBill.Hide();
            }
        }
    }

    #endregion

    #region Recieved

    protected void lnkreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "BillDispatch_ReceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        receivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void receivejoblistExport(string header, string contentType)
    {
        #region EARLIER CODE AS COMMENTED
        //Response.Clear();
        //Response.Buffer = true;
        //Response.AddHeader("content-disposition", header);
        //Response.Charset = "";
        //this.EnableViewState = false;
        //Response.ContentType = contentType;
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        ////Start Covering Letter
        ////gvRecievedJobDetail.AllowPaging = false;
        ////gvRecievedJobDetail.AllowSorting = false;
        ////gvRecievedJobDetail.Caption = "";
        ////gvRecievedJobDetail.DataSourceID = "PCDSqlDataSource1";
        ////gvRecievedJobDetail.DataBind();

        ////Remove Controls
        ////this.RemoveControls(gvRecievedJobDetail);

        ////gvRecievedJobDetail.RenderControl(hw);
        //gvcustomer.HeaderRow.Visible = false;

        //gvcustomer.AllowPaging = false;
        //gvcustomer.AllowSorting = false;
        //gvcustomer.Caption = "";
        //gvcustomer.Columns[0].ShowHeader = false;
        //gvcustomer.DataSourceID = "PcdSqlDataCustomer";
        //gvcustomer.DataBind();

        //for (int i = 0; i < gvcustomer.Rows.Count; i++)
        //{
        //    GridView gvReceivedJob = gvcustomer.Rows[i].FindControl("gvRecievedJobDetail") as GridView;
        //    if (gvReceivedJob != null)
        //    {
        //        gvReceivedJob.Columns[1].Visible = false;
        //        gvReceivedJob.Columns[2].Visible = false;
        //        gvReceivedJob.Columns[18].Visible = true; // customer column
        //    }
        //}

        ////Remove Controls
        //this.RemoveControls(gvcustomer);

        //gvcustomer.RenderControl(hw);
        ////end Covering Letter

        //Response.Output.Write(sw.ToString());
        //Response.End();
        #endregion

        DataSet dsGetReport = DBOperations.GetReportForBillDispatch(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["FinYearId"]));
        using (XLWorkbook wb = new XLWorkbook())
        {
            DataTable dtReport = dsGetReport.Tables[0];
            dtReport.TableName = "Bill Dispatch Received Jobs";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            if (dtReport.Rows.Count > 0)
            {
                // wb.Worksheets.Add(dtReport);
                //wb.Style.Alignment.WrapText = true;
                for (int i = 0; i < dtReport.Rows.Count; i++)
                {
                    if (dtReport.Rows[i][6].ToString().Trim() == "01-01-1900")
                    {
                        dtReport.Rows[i][6] = "";
                    }
                }
                var workSheet = wb.Worksheets.Add(dtReport);
                var SrNo_Col = workSheet.Column("A");
                SrNo_Col.Width = 8;
                workSheet.Style.Alignment.WrapText = true;
                //workSheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
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

            //------------start Covering letter--------- 


            LinkButton lnk = (LinkButton)e.Row.FindControl("txtUploadPath");

            string[] commandArgs = lnk.Text.Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";

            if (commandArgs[0].ToString() != "")
                strCustDocFolder = commandArgs[0].ToString();
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString();

            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString();

            LinkButton lnk1 = (LinkButton)e.Row.FindControl("lnkDocument");
            if (strJobFileDir.Contains(".doc") || strJobFileDir.Contains(".xls") || strJobFileDir.Contains(".xlsx"))
            {

                lnk1.Text = "View";
                lnk.Visible = false;
            }
            else
            {
                lnk.Visible = false;
                lnk1.Text = "";
            }

            //------------end Covering letter---------


            //LinkButton lnkbtnresult = (LinkButton)e.Row.FindControl("lnkviewletter");
            //ViewState["letter"] =lnkbtnresult.Text.ToString();


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


            //-------------Start Covering Letter-----------		
            //foreach (DataControlField col in gvRecievedJobDetail.Columns)
            foreach (DataControlField col in gvcustomer.Columns)
            //-------------end Covering Letter-----------
            {
                if (col.HeaderText == "View Covering Letter")
                {

                }
                e.Row.Cells[14].CssClass = "hidden";//rulename
                e.Row.Cells[11].ToolTip = "Today – FSB";//aging1
                e.Row.Cells[12].ToolTip = "Today – FRBD";//aging2
                //e.Row.Cells[13].ToolTip = "Today – Clearance Date";//aging3

            }

            if (DataBinder.Eval(e.Row.DataItem, "JobTypeName") != DBNull.Value)
            {
                string JobType = DataBinder.Eval(e.Row.DataItem, "JobTypeName").ToString();
                if (JobType != "" && JobType.Trim().ToLower() == "export")
                {
                    e.Row.Cells[13].ToolTip = "Today – Document Hand Over To Shipping Line Date.";
                    e.Row.Cells[8].Text = "";
                }
                else if (JobType != "" && JobType.Trim().ToLower() == "additional")
                {
                    e.Row.Cells[13].ToolTip = "Today – Job added to additional tab.";
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[9].Text = "";
                }
                else
                {
                    e.Row.Cells[13].ToolTip = "Today – Clearance Date.";
                    e.Row.Cells[9].Text = "";
                }
            }

            //ShippingLineDate Column
            //if (DataBinder.Eval(e.Row.DataItem, "ShippingLineDate") != DBNull.Value)
            //{
            //    if (Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "ShippingLineDate")).ToString("dd/MM/yyyy") == "01/01/1900")
            //    {
            //        e.Row.Cells[9].Text = "";
            //    }
            //}

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
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[14].CssClass = "hidden";//rulename
        }
    }
    protected void gvcustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblShowError.Text = "";
        if (e.CommandName.ToLower() == "instruction" && e.CommandArgument != null)
        {

            string CustId = e.CommandArgument.ToString();
            //string CustId = commandArgs[0].ToString();
            if (CustId != "")
            {
                DataSet dsCustInst = new DataSet();

                dsCustInst = BillingOperation.GetCustomerInstruction(CustId);

                if (dsCustInst.Tables.Count > 0 && dsCustInst.Tables[0].Rows.Count > 0)
                {
                    ModalPopupExtender2.Show();
                    string BillInstruction = dsCustInst.Tables[0].Rows[0]["Bill_Instruction"].ToString();
                    //HiddenField hdnDocId = (HiddenField)rpReason.Items[i].FindControl("hdnDocId");
                    txtInstruction.Text = BillInstruction;

                }
            }
            else
            {
                txtInstruction.Text = "";
                ModalPopupExtender2.Show();
            }
        }
    }

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "documentpopup" && e.CommandArgument != null)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            ViewState["JobId"] = Session["JobId"];
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString() + "\\";
            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;
            int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        }

        if (e.CommandName.ToLower() == "coveringletter")
        {

            //-------start Covering letter
            string DocPath = e.CommandArgument.ToString();

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobFileDir = "";
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            ViewState["JobId"] = Session["JobId"];
            if (commandArgs[1].ToString() != "")
                strCustDocFolder = commandArgs[1].ToString() + "\\";
            if (commandArgs[2].ToString() != "")
                strJobFileDir = commandArgs[2].ToString();
            hdnpath.Value = strCustDocFolder + strJobFileDir;

            DownloadDocument(hdnpath.Value);
            //-------end Covering letter
        }
        if (e.CommandName.ToLower() == "reject" && e.CommandArgument != null)
        {
            lblMsgforReceived.Text = "";
            lblerror.Text = "";
            ///ViewState["JobId"] = e.CommandArgument.ToString();

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            hdnJobId.Value = commandArgs[0].ToString();
            Session["JobId"] = commandArgs[0].ToString();
            ViewState["JobId"] = Session["JobId"];
            Session["JobRefNo"] = commandArgs[1].ToString();

            LinkButton btnsubmit = sender as LinkButton;
            Session["status"] = "Reject";
            this.ModalPopupExtender1.Show();

            foreach (RepeaterItem ri in rpReason.Items)
            {
                CheckBox chkreason = (CheckBox)ri.FindControl("chkreasonofpendency");
                TextBox txtreason = (TextBox)ri.FindControl("txtReason");
                DropDownList Drplrdctype = (DropDownList)ri.FindControl("Drplrdctype");
                Label lbltype = (Label)ri.FindControl("lbltype");
                Label lblReason = (Label)ri.FindControl("lblReason");
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
        if (e.CommandName.ToLower() == "approve" && e.CommandArgument != null)
        {
            ViewState["JobId"] = e.CommandArgument.ToString();

            int result = 0;
            lblerror.Text = "";

            int reasonforPendency = 0;
            string interests = string.Empty;
            LinkButton btnsubmit = sender as LinkButton;
            bool bApprove = true;

            GetModuleId();
            result = BillingOperation.ApproveRejectDispatching(Convert.ToInt32(ViewState["JobId"]), bApprove, "", reasonforPendency, "", 0, LoggedInUser.glUserId, ModuleId);
            Session["JobRefNo"] = "";
            ModuleId = 0;

            if (result == 0)
            {
                lblMsgforApproveReject.Text = "Bill Dispatch Completed! Job Moved To Dispatch Department!.";
                lblMsgforApproveReject.CssClass = "success";
                //---------------------start Covering letter-------------------
                //gvRecievedJobDetail.DataBind();
                gvcustomer.DataBind();
                //---------------------end Covering letter---------------------
            }
            else if (result == 1)
            {
                lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                lblMsgforApproveReject.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblMsgforApproveReject.Text = "Bill Dispatch Already Completed!";
                lblMsgforApproveReject.CssClass = "errorMsg";
            }

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
                txtHoldReason.Text = "";
                lblError_HoldExp.Text = "";
                hdnJobId_hold.Value = JobId.ToString();
                hdnJobRefNo.Value = strJobRefNo;
                lblHoldPopupName.Text = "Hold Job";
                btnHoldJob.Text = "Hold Job";
                if (JobType == 4) // Additional Job
                {
                    fvHoldJobDetail.DataSource = DBOperations.GetAdditionalJobDetail(JobId);
                    fvHoldJobDetail.DataBind();
                }
                else
                {
                    fvHoldJobDetail.DataSource = DBOperations.GetFRJobDetail(JobId);
                    fvHoldJobDetail.DataBind();
                }

                ImageButton img = (ImageButton)e.CommandSource as ImageButton;
                GridViewRow row = img.NamingContainer as GridViewRow;
                mpeHoldExpense.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "unhold")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobRefNo = "";
            int JobId = 0;

            if (commandArgs[0].ToString() != "")
                JobId = Convert.ToInt32(commandArgs[0].ToString());
            if (commandArgs[1].ToString() != "")
                strJobRefNo = commandArgs[1].ToString();

            if (JobId != 0)
            {
                if (JobId != 0)
                {
                    int result = DBOperations.FR_AddHoldBillingAdvice(JobId, "", "", LoggedInUser.glUserId);
                    if (result == 0)
                    {
                        fvHoldJobDetail.DataBind();
                        lblShowError.Text = "Successfully unholded job no " + strJobRefNo + ".";
                        lblShowError.CssClass = "success";
                        gvcustomer.DataBind();
                    }
                    else
                    {
                        lblShowError.Text = "System error. Please try again later.";
                        lblShowError.CssClass = "errorMsg";
                    }
                }
            }
        }
        if (e.CommandName.ToLower() == "mail" && e.CommandArgument != null)
        {
            lblMsg.Text = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            Session["JobId"] = commandArgs[0].ToString();
            Session["JobRefNo"] = commandArgs[1].ToString();
            Session["CustName"] = commandArgs[2].ToString();
            lblJobRegNo.Text = Session["JobRefNo"].ToString();
            lblcustName.Text = commandArgs[2].ToString();
            lblTitle.Text = "File Upload";
            PanelFileUpload.Height = 430;
            CheckDocExist(Convert.ToInt32(Session["JobId"]));
            CheckMailSend(Convert.ToInt32(Session["JobId"]), Session["JobRefNo"].ToString());
        }
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
        Label lblReason = (Label)e.Item.FindControl("lblReason");
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
            cboDDCategory.Style.Add("display", "none");
            txtReason.Style.Add("display", "none");
            lbltype.Style.Add("display", "none");
            Drplrdctype.Style.Add("display", "none");
            txtUser.Style.Add("display", "none");
            lblMailTo.Style.Add("display", "none");

        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        lblShowError.Text = "";
        int result1 = 0;

        int result = 0;
        lblerror.Text = "";
        lblerror.CssClass = "";
        int reasonforPendency = 0;
        int checkedCount = 0;
        string interests = string.Empty;
        bool IsLRPending = false;

        string Remark = "";
        string LRDC_Type = "";
        string Mailtoid = "";
        if (Session["status"].ToString() == "Reject")
        {
            bool bApprove = false;
            for (int i = 0; i < rpReason.Items.Count; i++)
            {
                CheckBox chk = (CheckBox)rpReason.Items[i].FindControl("chkreasonofpendency");
                if (chk.Checked)
                {
                    checkedCount += chk.Checked ? 1 : 0;
                    HiddenField hdnDocId = (HiddenField)rpReason.Items[i].FindControl("hdnDocId");
                    TextBox txtReason = (TextBox)rpReason.Items[i].FindControl("txtReason");
                    DropDownList Drplrdctype = (DropDownList)rpReason.Items[i].FindControl("Drplrdctype");
                    DropDownList CBOddCategory = (DropDownList)rpReason.Items[i].FindControl("ddCategory");

                    HiddenField hdnUserId = (HiddenField)rpReason.Items[i].FindControl("hdnUserId");

                    Remark = txtReason.Text.Trim();
                    reasonforPendency = Convert.ToInt16(hdnDocId.Value);

                    if (chk.Text == "LR/DC")
                    {
                        IsLRPending = true;
                        LRDC_Type = Drplrdctype.SelectedValue;
                    }
                    else
                    {
                        LRDC_Type = "0";
                    }

                    if (chk.Text == "Others")
                    {
                        Mailtoid = hdnUserId.Value;
                    }
                    else
                    {
                        Mailtoid = "0";
                    }

                    if (checkedCount != 0)
                    {
                        int CategoryId = 0;

                        CategoryId = Convert.ToInt32(CBOddCategory.SelectedValue);

                        GetModuleId();
                        result = BillingOperation.ApproveRejectDispatching(Convert.ToInt32(ViewState["JobId"]), bApprove, Remark, Convert.ToInt16(reasonforPendency), LRDC_Type, CategoryId, LoggedInUser.glUserId, ModuleId);

                        if (result == 0)
                        {
                            if (IsLRPending == true)
                            {
                                int result_LR = DBOperations.AddAdviceToLRPending(Convert.ToInt32(ViewState["JobId"]), 0, LoggedInUser.glUserId);
                            }
                            ModalPopupExtender1.Hide();
                            lblMsgforApproveReject.Text = "Bill Checking Rejected! Job Moved Back To Bill Rejected!.";
                            lblMsgforApproveReject.CssClass = "success";
                            gvcustomer.DataBind();
                        }
                        else if (result == 1)
                        {
                            lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                        }
                        else if (result == 2)
                        {
                            lblMsgforApproveReject.Text = "Bill Checking Rejected Already Pending!";
                            lblMsgforApproveReject.CssClass = "errorMsg";
                            ModalPopupExtender1.Hide();
                        }
                    }
                }
            }

            if (checkedCount == 0)
            {
                ModalPopupExtender1.Show();
                lblerror.Text = "Please select atleast 1 checkbox";
                lblerror.CssClass = "errorMsg";
            }

            if (ModuleId != 2)
            {
                int rejectedby = Convert.ToInt16(EnumBilltype.BillDispatch);

                result1 = BillingOperation.Rejectmailnotification(Convert.ToInt32(ViewState["JobId"]), LoggedInUser.glUserId, rejectedby, Convert.ToInt16(Session["FinYearId"]), Convert.ToString(Mailtoid));

                if (result1 != 0)
                {
                    lblMsgforApproveReject.Text = "System Error in Mail Sending! Please try after sometime.";
                    lblMsgforApproveReject.CssClass = "errorMsg";
                }
            }
            ModuleId = 0;
        }
        //--------------start Covering Letter ----------------------------
        //gvRecievedJobDetail.DataBind();
        gvcustomer.DataBind();
        //--------------end Covering Letter ----------------------------

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
            if (TabJobDetail.ActiveTabIndex == 0)
            {
                DataFilter1_OnDataBound();
            }
            else
            {
                DataFilter2_OnDataBound();
            }
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {

            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDDispatch.aspx";
            DataFilter1.FilterDataSource();
            gvNonRecievedJobDetail.DataBind();
            if (gvNonRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Bill Draft!";
                lblMsgforNonReceived.CssClass = "errorMsg";
            }
            else
            {
                lblMsgforNonReceived.Text = "";
            }
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
            DataFilter2.FilterSessionID = "PCDDispatch1.aspx";

            //-----start covering Letter---------------
            //DataFilter2.DataColumns = gvNonRecievedJobDetail.Columns;
            //DataFilter2.FilterDataSource();
            //gvRecievedJobDetail.DataBind();
            //if (gvRecievedJobDetail.Rows.Count == 0)

            DataFilter2.DataColumns = gvcustomer.Columns;
            DataFilter2.FilterDataSource();
            gvcustomer.DataBind();
            if (gvcustomer.Rows.Count == 0)
            //-----end covering Letter---------------
            {
                lblMsgforReceived.Text = "No Job Found For Recieved file for Bill Draft!";
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

    protected void TabJobDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabJobDetail.ActiveTabIndex == 0)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblMsgforApproveReject.Text = "";
            DataFilter1.DataSource = PCDSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "BillDispatch.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvNonRecievedJobDetail.DataBind();
        }
        if (TabJobDetail.ActiveTabIndex == 1)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblMsgforApproveReject.Text = "";
            DataFilter2.DataSource = PCDSqlDataSource1;
            //------------start Covering Letter----------------	
            //DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.DataColumns = gvcustomer.Columns;
            //------------end Covering Letter----------------		
            DataFilter2.FilterSessionID = "BillDispatch1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            //gvRecievedJobDetail.DataBind();
            gvcustomer.DataBind();
        }
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

    //-----------start Covering letter

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        lblShowError.Text = "";
        int i = 0;
        int result = 0;
        lblerror.Text = "";

        int reasonforPendency = 0;
        string interests = string.Empty;
        LinkButton btnsubmit = sender as LinkButton;
        bool bApprove = true;

        foreach (GridViewRow gvr in gvcustomer.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {

                //if (((CheckBox)gvr.FindControl("chk1")).Checked)
                //{
                Panel pnlgvreceivedjob = (Panel)gvr.FindControl("pnlCustomer");
                //pnlgvreceivedjob.Style.Add("display", "inline");
                GridView gvRecievedJobDetail = (GridView)gvr.FindControl("gvRecievedJobDetail");
                foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
                {
                    if (gvr1.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton lnkJobNo = (LinkButton)gvr1.FindControl("lnkJobNo");
                        // bool result = ((CheckBox)row.FindControl("chkjoball")).Checked;                       
                        CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");
                        if (chk1.Checked)
                        {
                            LinkButton lnk = (LinkButton)gvr1.FindControl("txtUploadPath");

                            string[] commandArgs = lnk.Text.Split(new char[] { ';' });
                            string strCustDocFolder = "", strJobFileDir = "";

                            if (commandArgs[0].ToString() != "")
                                strCustDocFolder = commandArgs[0].ToString();
                            if (commandArgs[1].ToString() != "")
                                strCustDocFolder = commandArgs[1].ToString();

                            if (commandArgs[2].ToString() != "")
                                strJobFileDir = commandArgs[2].ToString();

                            LinkButton lnk1 = (LinkButton)gvr1.FindControl("lnkDocument");

                            if (strJobFileDir.Contains(".doc") || strJobFileDir.Contains(".xls") || strJobFileDir.Contains(".xlsx")
                              || strCustDocFolder.Contains("DOWC") || strCustDocFolder.Contains("RELI_CcEJ") || strCustDocFolder.Contains("BASF")
                              || strCustDocFolder.Contains("LGEL") || strCustDocFolder.Contains("ASIA_E") || strCustDocFolder.Contains("AKZO")
                              || strCustDocFolder.Contains("HIND_WN") || strCustDocFolder.Contains("MIOV_") || strCustDocFolder.Contains("ESSE_X")
                              || strCustDocFolder.Contains("ULTR_dC") || strCustDocFolder.Contains("DEEP_O") || strCustDocFolder.Contains("JCBI_zL")
                              || strCustDocFolder.Contains("TATA_x9") || strCustDocFolder.Contains("LARS") || strCustDocFolder.Contains("GOOD_Sx")
                              || strCustDocFolder.Contains("MOND_mqrU") || strCustDocFolder.Contains("AKZO_Ic") || strCustDocFolder.Contains("LARS_ChwY")
                              || strCustDocFolder.Contains("CORN_zViF") || strCustDocFolder.Contains("PRAX_y4gF") || strCustDocFolder.Contains("MICR_HScn")
                              || strCustDocFolder.Contains("BALK_yQ0i") || strCustDocFolder.Contains("RELI_Haj0") || strCustDocFolder.Contains("SHEL_upx6")
                              || strCustDocFolder.Contains("PANT_Hg8l") || strCustDocFolder.Contains("AXAL_9kzN") || strCustDocFolder.Contains("SCHL_MZxP")
                              || strCustDocFolder.Contains("TRAN_pwzV") || strCustDocFolder.Contains("HALL_JAsA") || strCustDocFolder.Contains("WEAT")
                              || strCustDocFolder.Contains("STEE_2FTj"))
                            {
                                LinkButton Recv = (LinkButton)gvr1.FindControl("lnkJobNo");
                                string JobId = Recv.CommandArgument;
                                string s = string.Empty;

                                Session["JobRefNo"] = Recv.Text;
                                GetModuleId();
                                result = BillingOperation.ApproveRejectDispatching(Convert.ToInt32(JobId), bApprove, "", reasonforPendency, "", 0, LoggedInUser.glUserId, ModuleId);
                                Session["JobRefNo"] = "";
                                ModuleId = 0;

                                if (result == 0)
                                {
                                    lblMsgforApproveReject.Text = "Bill Dispatch Completed! Job Moved To Dispatch Department!.";
                                    lblMsgforApproveReject.CssClass = "success";
                                    gvcustomer.DataBind();
                                }
                                else if (result == 1)
                                {
                                    lblMsgforApproveReject.Text = "System Error! Please try after sometime.";
                                    lblMsgforApproveReject.CssClass = "errorMsg";
                                }
                                else if (result == 2)
                                {
                                    lblMsgforApproveReject.Text = "Bill Dispatch Already Completed!";
                                    lblMsgforApproveReject.CssClass = "errorMsg";
                                }

                            }
                            else
                            {
                                lblMsgforApproveReject.Text = "Please Generate Covering Letter First!.";
                                lblMsgforApproveReject.CssClass = "errorMsg";
                            }

                            i++;
                        }
                    }

                    if (i == 0)
                    {
                        lblMsgforApproveReject.Text = "Please Checked atleast 1 checkbox.";
                        lblMsgforApproveReject.CssClass = "errorMsg";
                    }
                }
            }
        }

    }

    protected void btnCoveringLetter_Click(object sender, EventArgs e)
    {

        foreach (GridViewRow gvr in gvcustomer.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                DataSet ds = new DataSet();
                string Customerid = gvcustomer.DataKeys[gvr.RowIndex].Value.ToString();
                string Customername = gvr.Cells[1].Text;

                GridView gvRecievedJobDetail = (GridView)gvr.FindControl("gvRecievedJobDetail");
                foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
                {
                    if (gvr1.RowType == DataControlRowType.DataRow)
                    {
                        string jobid = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString();
                        CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                        if (chk1.Checked)
                        {
                            Session["JobId"] = jobid;
                            DataView dvDoc = (DataView)SqlDataSourceBillDispatchDoc.Select(DataSourceSelectArguments.Empty);
                            DataTable dtBillDoc = dvDoc.Table;
                            DataColumn newCol = new DataColumn("NewColumn", typeof(string));
                            newCol.AllowDBNull = true;
                            dtBillDoc.Columns.Add(newCol);

                            gvBillDispatchDocDetail.DataSource = dtBillDoc;
                            gvBillDispatchDocDetail.DataBind();

                            break;
                        }
                    }
                }
            }
        }








        if (gvBillDispatchDocDetail.Rows.Count > 0)
        {
            //ViewState["JobId"] = e.CommandArgument.ToString();
            int i = 0;
            int result = 0;
            lblerror.Text = "";
            string strFilePath = "", JobType = "";
            ReportDocument crystalReport = new ReportDocument();

            using (ZipFile zip = new ZipFile())
            {
                foreach (GridViewRow gvr in gvcustomer.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        DataSet ds = new DataSet();
                        string Customerid = gvcustomer.DataKeys[gvr.RowIndex].Value.ToString();
                        string Customername = gvr.Cells[1].Text;
                        ViewState["jobid"] = "";
                        FileUpload fuDocument = (FileUpload)gvr.FindControl("fuDocument");
                        Panel pnlgvreceivedjob = (Panel)gvr.FindControl("pnlCustomer");
                        GridView gvRecievedJobDetail = (GridView)gvr.FindControl("gvRecievedJobDetail");

                        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
                        {
                            if (gvr1.RowType == DataControlRowType.DataRow)
                            {
                                string jobid = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString();

                                LinkButton lnkJobNo = (LinkButton)gvr1.FindControl("lnkJobNo");
                                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");
                                LinkButton txtUploadPath = (LinkButton)gvr1.FindControl("txtUploadPath");
                                string DocPath = txtUploadPath.Text;

                                string[] commandArgs = DocPath.ToString().Split(new char[] { ';' });
                                string strCustDocFolder = "", strJobFileDir = "";
                                if (commandArgs[1].ToString() != "")
                                    strCustDocFolder = commandArgs[1].ToString() + "\\";
                                if (commandArgs[2].ToString() != "")
                                    strJobFileDir = commandArgs[2].ToString() + "\\";
                                //hdnpath.Value = strCustDocFolder + strJobFileDir;
                                hdnpath.Value = strCustDocFolder;

                                if (chk1.Checked)
                                {
                                    JobType = gvr1.Cells[4].Text;
                                    LinkButton lnkJobNo1 = (LinkButton)gvr1.FindControl("lnkJobNo");

                                    if (ViewState["jobid"].ToString() == "")
                                    {
                                        ViewState["jobid"] = jobid;
                                    }
                                    else
                                    {
                                        ViewState["jobid"] = ViewState["jobid"].ToString() + ',' + jobid;
                                    }
                                    i++;
                                }//if (chk1.Checked)

                            } //if (gvr1.RowType == DataControlRowType.DataRow)

                            if (i == 0)
                            {
                                lblMsgforApproveReject.Text = "Please Checked atleast 1 checkbox.";
                                lblMsgforApproveReject.CssClass = "errorMsg";
                            }//if (i == 0)

                        } // foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)

                        if (i > 0)
                        {
                            if (Customerid != "93" && Customerid != "35" && Customerid != "8" && Customerid != "85" && Customerid != "84" && Customerid != "32" && Customerid != "24" && Customerid != "64")
                            {
                                Customername = Customername.Replace("M/S", "");
                                Customername = Customername.Replace("/", "");

                                string fileName = Customername.Replace("&amp;", "").Trim().ToString() + ".doc";

                                string strUploadPath = hdnpath.Value;
                                string ServerFilePath = FileServer.GetFileServerDir();

                                if (ServerFilePath == "")
                                {
                                    // Application Directory Path
                                    ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strUploadPath);
                                }
                                else
                                {
                                    // File Server Path
                                    ServerFilePath = ServerFilePath + strUploadPath;
                                }

                                if (!System.IO.Directory.Exists(ServerFilePath))
                                {

                                    System.IO.Directory.CreateDirectory(ServerFilePath);
                                }
                                if (fileName != string.Empty)
                                {
                                    if (System.IO.File.Exists(ServerFilePath + fileName))
                                    {
                                        //string ext = Path.GetExtension(fuPCDUpload.FileName);
                                        string ext = Path.GetExtension(fileName);
                                        //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                                        fileName = Path.GetFileNameWithoutExtension(fileName);
                                        string FileId = RandomString(5);

                                        fileName += "_" + FileId + ext;
                                    }
                                }

                                crystalReport.Load(Server.MapPath("~/Reports/CoveringLetter.rpt"));
                                standardCoveringLetter dsCustomers = new standardCoveringLetter();

                                String strjobid = ViewState["jobid"].ToString();

                                if (JobType != "")
                                {
                                    if (JobType.ToLower().Trim() == "import") // Import job covering letter detail
                                    {
                                        dsCustomers = GetData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "sez") // SEZ job covering letter detail
                                    {
                                        dsCustomers = GetSEZCoverLetterData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "export")  // Export job covering letter detail
                                    {
                                        dsCustomers = GetExportCoverLetterData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "additional")  // Additional job covering letter detail
                                    {
                                        dsCustomers = GetAdditionalCoverLetterData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "pn movement")  // Movement job covering letter detail
                                    {
                                        dsCustomers = GetMovementCoverLetterData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "transport")  // transport job covering letter detail
                                    {
                                        dsCustomers = GetTransportCoverLetterData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }
                                    else if (JobType.ToLower().Trim() == "freight")
                                    {
                                        dsCustomers = GetDataForFreight(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                    }

                                    if (dsCustomers.Tables.Count > 0 && dsCustomers.Tables[0].Rows.Count > 0)
                                    {
                                        crystalReport.SetDataSource(dsCustomers);
                                        string masterinvoiceno = dsCustomers.Tables[0].Rows[0]["MasterInvoiceNo"].ToString();

                                        zip.AddFile(ServerFilePath + fileName, "Files");
                                        crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, ServerFilePath + fileName);

                                        result = BillingOperation.AddCoverletterPath(Convert.ToInt32(Customerid), ViewState["jobid"].ToString(), fileName, masterinvoiceno, strUploadPath, LoggedInUser.glUserId);
                                        crystalReport.Close();
                                        crystalReport.Clone();
                                        crystalReport.Dispose();
                                        crystalReport = null;
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                    }
                                    else
                                    {
                                        lblreceivemsg.Text = "Plant Address Not Found!";
                                        lblreceivemsg.CssClass = "errorMsg!";
                                    }
                                }// Job Type Not Found
                                else
                                {
                                    lblreceivemsg.Text = "Job Type Error!";
                                    lblreceivemsg.CssClass = "errorMsg!";
                                }
                                //MemoryStream oStream;

                                //crystalReport.Load(Server.MapPath("~/Reports/CoveringLetter.rpt"));
                                //standardCoveringLetter dsCustomers = GetData(Convert.ToInt16(Customerid), '"' + ViewState["jobid"].ToString() + '"');
                                //crystalReport.SetDataSource(dsCustomers);

                                ////string masterinvoiceno = dsCustomers.Tables[0].Rows[0]["MasterInvoiceNo"].ToString();

                                //string masterinvoiceno = "";
                                //if (dsCustomers.Tables[0].Rows.Count == 0)
                                //{
                                //    lblreceivemsg.Text = "Plant Address Not Found!";
                                //    lblreceivemsg.CssClass = "errorMsg!";
                                //}
                                //else
                                //{
                                //    masterinvoiceno = dsCustomers.Tables[0].Rows[0]["MasterInvoiceNo"].ToString();


                                //    zip.AddFile(ServerFilePath + fileName, "Files");

                                //    crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, ServerFilePath + fileName);

                                //    result = BillingOperation.AddCoverletterPath(Convert.ToInt32(Customerid), ViewState["jobid"].ToString(), fileName, masterinvoiceno, strUploadPath, LoggedInUser.glUserId);
                                //}
                                ////crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, "");                       
                                //crystalReport.Close();
                                //crystalReport.Clone();
                                //crystalReport.Dispose();
                                //crystalReport = null;
                                //GC.Collect();
                                //GC.WaitForPendingFinalizers();
                            }
                            i = 0;
                        }
                    }
                }


                Response.Clear();

                Response.BufferOutput = false;
                string zipName = String.Format("CoveringLetter_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                //Response.Flush(); //First Response
                //Response.Redirect("PCDDispatch.aspx");  
                lblreceivemsg.Text = "CoveringLetter Generated Successfully!";
                lblreceivemsg.CssClass = "Success";
                gvcustomer.DataBind();
                //zip.ExtractAll(extractPath, ExtractExistingFileAction.DoNotOverwrite);
                Response.End();

            }
        }
        else
        {
            lblShowError.Text = "Please first upload document then generate covering letter!";
            lblShowError.CssClass = "errorMsg";
        }

    }

    private standardCoveringLetter GetData(int Customerid, string jobid)
    {
        //DataSet ds = BillingOperation.getCoveringletter(Customerid);
        ////SqlCommand cmd = new SqlCommand();
        //SqlDataAdapter sda = new SqlDataAdapter();
        ////sda.SelectCommand = ds;
        //using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
        //    {
        //                sda.Fill(ds, "CoveringLetter");
        //                return dsCustomers;
        //    }

        string strFinYearID = "0";

        if (Session["FinYearId"] != null)
        {
            strFinYearID = Session["FinYearId"].ToString();
        }
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand("GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        cmd.Parameters.Add(new SqlParameter("@FinYearID", strFinYearID));

        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {

                    sda.Fill(dsCustomers, "Coveringletter");

                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetMovementCoverLetterData(int Customerid, string jobid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("CM_GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {
                    sda.Fill(dsCustomers, "Coveringletter");
                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetExportCoverLetterData(int Customerid, string jobid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("EX_GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {
                    sda.Fill(dsCustomers, "Coveringletter");
                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetSEZCoverLetterData(int Customerid, string jobid)
    {
        string strFinYearID = "0";

        if (Session["FinYearId"] != null)
        {
            strFinYearID = Session["FinYearId"].ToString();
        }
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand("SEZ_GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        cmd.Parameters.Add(new SqlParameter("@FinYearID", strFinYearID));

        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {

                    sda.Fill(dsCustomers, "Coveringletter");

                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetAdditionalCoverLetterData(int Customerid, string jobid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("AD_GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {
                    sda.Fill(dsCustomers, "Coveringletter");
                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetTransportCoverLetterData(int Customerid, string jobid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("TR_GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {
                    sda.Fill(dsCustomers, "Coveringletter");
                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetDataForFreight(int Customerid, string jobid)
    {
        string strFinYearID = "0";

        if (Session["FinYearId"] != null)
        {
            strFinYearID = Session["FinYearId"].ToString();
        }
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand("GetCoveringletterdetailsforFreight");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        cmd.Parameters.Add(new SqlParameter("@FinYearID", strFinYearID));

        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {

                    sda.Fill(dsCustomers, "Coveringletter");

                    return dsCustomers;
                }
            }
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string customerId = gvcustomer.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvRecievedJobDetail = e.Row.FindControl("gvRecievedJobDetail") as GridView;

            PCDSqlDataSource1.SelectParameters["Customerid"].DefaultValue = customerId;
            gvRecievedJobDetail.DataSource = PCDSqlDataSource1;
            gvRecievedJobDetail.DataBind();

            ScriptManager1.RegisterPostBackControl(gvRecievedJobDetail);
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

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        lblreceivemsg.Text = "Progress";
        //progressBar.Value = e.ProgressPercentage;
    }

    private void Completed(object sender, AsyncCompletedEventArgs e)
    {
        lblreceivemsg.Text = "Download completed!";
        // MessageBox.Show("Download completed!");
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
            ServerPath = ServerPath.Replace("FreightOperation\\", "");
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

    private string UploadPCDDocument(string FilePath, string fileName)
    {
        //string FileName = fuDocument.FileName;

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fileName != string.Empty)
        {
            if (!System.IO.File.Exists(ServerFilePath + fileName))
            {
                //string ext = Path.GetExtension(fuPCDUpload.FileName);
                string ext = Path.GetExtension(fileName);
                //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                fileName = Path.GetFileNameWithoutExtension(fileName);
                string FileId = RandomString(5);

                fileName += "_" + FileId + ext;
            }

            //fuDocument.SaveAs(ServerFilePath + fileName);


        }

        return FilePath + fileName;
    }


    //-----------end Covering letter      

    #region HOLD JOB EXPENSE

    protected void btnHoldJob_OnClick(object sender, EventArgs e)
    {
        int JobId = 0;
        if (hdnJobId_hold.Value != "")
        {
            JobId = Convert.ToInt32(hdnJobId_hold.Value);
        }

        if (JobId != 0)
        {
            if (txtHoldReason.Text != "")
            {
                int result = DBOperations.FR_AddHoldBillingAdvice(JobId, txtHoldReason.Text.Trim(), "", LoggedInUser.glUserId);
                if (result == 0)
                {
                    fvHoldJobDetail.DataBind();
                    mpeHoldExpense.Hide();
                    lblShowError.Text = "Successfully holded job no " + hdnJobRefNo.Value + ".";
                    lblShowError.CssClass = "success";
                    gvcustomer.DataBind();
                }
                else
                {
                    lblError_HoldExp.Text = "System error. Please try again later.";
                    lblError_HoldExp.CssClass = "errorMsg";
                }
            }
            else
            {
                lblError_HoldExp.Text = "Please enter reason.";
                lblError_HoldExp.CssClass = "errorMsg";
                rfvReason.Enabled = true;
                mpeHoldExpense.Show();
            }
        }
    }
    protected void txtHoldReason_OnTextChanged(object sender, EventArgs e)
    {
        GridView gvReceivedJob = (GridView)gvcustomer.FindControl("gvRecievedJobDetail");
        GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
        int index = row.RowIndex;
        TextBox txtHoldReason = (TextBox)gvReceivedJob.Rows[index].FindControl("txtHoldReason");
        int JobId = 0;
        JobId = Convert.ToInt32(gvReceivedJob.DataKeys[index].ToString());

        if (txtHoldReason.Text.Trim() != "" && JobId > 0)
        {
            int result = DBOperations.AddHoldBillingAdvice(JobId, txtHoldReason.Text.Trim(), "", LoggedInUser.glUserId);
            if (result == 0)
            {
                lblerror.Text = "Successfully holded job no " + hdnJobRefNo.Value + ".";
                lblerror.CssClass = "success";
                gvcustomer.DataBind();
            }
            else
            {
                lblerror.Text = "System error. Please try again later.";
                lblerror.CssClass = "errorMsg";
            }
        }
        else
        {
            lblerror.Text = "Please enter reason for holding the job.";
            lblerror.CssClass = "errorMsg";
        }
    }

    #endregion

    #region freight
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
    #endregion

    #region EBill Detail

    protected void btnEBillUpdate_Click(object sender, EventArgs e)
    {
        int JobId = 0;
        string strBillRemark = "";

        JobId = Convert.ToInt32(hdnEBillJobId.Value);
        DateTime dtEbillDate = DateTime.MinValue;

        strBillRemark = txtEBillRemark.Text.Trim();

        if (txtEBillDate.Text.Trim() != "")
        {
            dtEbillDate = Commonfunctions.CDateTime(txtEBillDate.Text.Trim());
        }

        int result = BillingOperation.updateBillingEBill(JobId, dtEbillDate, strBillRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblShowError.Text = "E-Bill Detail Updated!";
            lblShowError.CssClass = "success";
            ModalPopupEBill.Hide();
        }
        else
        {
            lblMsgEBill.Text = "System Error! Please try after sometime!";
            lblerror.CssClass = "errorMsg";

            ModalPopupEBill.Show();
        }
    }

    protected void lnkPreAlertEmailDraft_Click(object sender, EventArgs e)
    {

        lblMsg.Text = Session["JobId"] + "@@" + Session["JobRefNo"].ToString();
        txtRemark.Text = "";
        chkDraft.ClearSelection();
        //MpeFileUpload.Show();
        GenerateEmailDraft();
    }

    private void GenerateEmailDraft()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);

        string MessageBody = "";
        string strJobRefNo = "", strCustName = "", strConsigneeName = "", strCustRefNo = "", strToMail = "", strJobType = "";
        int strAgencyCnt = 0, strRIMCnt = 0, strColSpan = 0, strCols = 0;
        string args; string[] args1; int tot; int AmtTot = 0;
        DataView dv = DBOperations.GetUserDetail(Convert.ToString(LoggedInUser.glUserId));
        DataTable dt = dv.ToTable(true, "sEmail");
        DataSet dsAlertDetail = DBOperations.GetBillDispatchDetail(JobId, Session["JobRefNo"].ToString());

        if (dsAlertDetail.Tables[0].Rows.Count > 0)
        {
            strCustName = dsAlertDetail.Tables[0].Rows[0]["Customer"].ToString();
            strConsigneeName = dsAlertDetail.Tables[0].Rows[0]["Consignee"].ToString();
            strJobRefNo = dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            strCustRefNo = dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            strJobType = dsAlertDetail.Tables[0].Rows[0]["JobType"].ToString();
            txtSubject.Text = "E-Bill Dispatch/Job No :- " + strJobRefNo + " /Customer Reference No :- " + strCustRefNo + "";
            strAgencyCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntAgency"].ToString());
            strRIMCnt = Convert.ToInt32(dsAlertDetail.Tables[0].Rows[0]["CntRIM"].ToString());
            strToMail = dsAlertDetail.Tables[0].Rows[0]["Email"].ToString();
            txtMailTo.Text = strToMail;
            //txtMailTo.Text = "developer1@babajishivram.com, developer2@babajishivram.com";
            txtMailCC.Text = dt.Rows[0]["sEmail"].ToString() + "," + "query.billing@babajishivram.com";
        }
        else
        {
            //lblError.Text = "Booking Details Not Found! Please check details.";
            //lblError.CssClass = "errorMsg";
            //return;
        }

        //////////////////////////////////////////////////////////////////////////////////
        if (strAgencyCnt > strRIMCnt)
        {
            strCols = strAgencyCnt;
        }
        else
        {
            strCols = strRIMCnt;
        }
        StringBuilder strStyle = new StringBuilder();
        strStyle = strStyle.Append("<html><body style='height:100%; width:100%; font-family:Arial; font-style:normal; font-size:9pt; color:#000;");

        // body header
        strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
        strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");

        strStyle = strStyle.Append(@"<tr><td>" + "Dear Sir / Madam, " + "<br />");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Kindly find the attached E-Invoices of Subject Shipment and details are as below. ");
        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td>" + "Customer Name :- " + strCustName);
        strStyle = strStyle.Append(@"</td></tr>");
        if (strJobType == "1")
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Consignee  Name :- " + strConsigneeName + "<br />");
        }
        else
        {
            strStyle = strStyle.Append(@"<tr><td>" + "Shipper  Name :- " + strConsigneeName + "<br />");
        }

        strStyle = strStyle.Append(@"</td></tr>");

        strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
        strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
        strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
        strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='50%'><colgroup><col width='40%' /><col width='35%' /><col width='30%' /><col width='30%' /></colgroup>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Job Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4' colspan=" + strCols + ">" + Session["JobRefNo"].ToString() + "</td></tr>");
        strStyle = strStyle.Append(@"<tr><td style='border: 1px solid #ccc; background-color:#99CCFF'>Cust Ref No </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + Session["CustName"] + "</td></tr>");

        ///Agency Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'  colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        tot = 0; AmtTot = 0;

        //RIM Details
        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice No </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVNO"].ToString();
        args1 = args.Split(',');
        tot = args1.Length;
        if (tot == strCols) { strColSpan = 0; }
        else { strColSpan = strCols; }
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Invoice Date </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVDate"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strColSpan + ">" + args1[i] + "</td>");
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice Amount </td>");
        args = dsAlertDetail.Tables[0].Rows[0]["RimINVAmt"].ToString();
        args1 = args.Split(',');
        for (int i = 0; i <= tot - 1; i++)
        {
            strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
            if (args1[i] == "") { args1[i] = "0"; }
            AmtTot = AmtTot + Convert.ToInt32(args1[i]);
        }
        strStyle = strStyle.Append(@"</tr>");

        strStyle = strStyle.Append(@"<tr>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Total Amount </td>");
        strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
        strStyle = strStyle.Append(@"</tr>");
        AmtTot = 0; tot = 0;

        ////Detension
        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Detention Invoice No </td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strCols + ">" + "-" + "</td></tr>");

        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Detention Invoice Date </td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + "-" + "</td></tr>");

        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Detention Invoice Amount</td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strCols + ">" + "-" + "</td></tr>");

        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Others Invoice No </td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + "-" + "</td></tr>");

        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Others Invoice Date</td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strCols + ">" + "-" + "</td></tr>");

        //strStyle = strStyle.Append(@"<tr>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Others Invoice Amount</td>");
        //strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + "-" + "</td></tr>");

        strStyle = strStyle.Append(@"</table></p></div></div></td>");

        // body footer
        strStyle = strStyle.Append(@"<tr><td>" + "Any billing related query or issue, kindly drop an e-mail to query.billing@babajishivram.com" + "<br/><br/>");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Thanks & Regards");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"<tr><td>" + "Babaji Shivram Clearing And Carriers Pvt Ltd");
        strStyle = strStyle.Append(@"</td></tr>");
        strStyle = strStyle.Append(@"</table></td></tr>");
        strStyle = strStyle.Append(@"</center></body></html>");

        MessageBody = strStyle.ToString();

        /////////////////////////////////////////////////////////////////////////////////////
        divPreviewEmail.InnerHtml = MessageBody;

        DataTable dtDoc = DBOperations.GetBillDoc(JobId);
        DataColumn newCol = new DataColumn("NewColumn", typeof(string));
        newCol.AllowDBNull = true;
        dtDoc.Columns.Add(newCol);
        int j = 0;
        string DocPath = "";
        foreach (DataRow rows in dtDoc.Rows)
        {
            DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            //FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            //j++;
        }
        gvDocAttach.DataSource = dtDoc;
        if (dtDoc.Columns.Contains("NewColumn"))
        {
            if (gvDocAttach.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvDocAttach.Columns.Add(test);
            }
        }
        gvDocAttach.DataBind();

        ModalPopupEmail.Show();
    }

    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        //lblStatus.Text = "Processing...";
        //lblStatus.Visible = true;

        if (txtMailTo.Text.Trim() == "")
        {
            //lblError.Text = "Please Enter Customer Email & Subject!";
            //lblError.CssClass = "errorMsg";
            ModalPopupEmail.Hide();
        }
        else
        {

            // Send Email
            bool bEMailSucess = SendPreAlertEmail();
            //bool bEMailSucess = true;
            // Update PreAlert Email Sent Status and Customer Email 

            // ModalPopupEmail.Hide();

            if (bEMailSucess == true)
            {
                int Result = DBOperations.AddJobNotofication(JobId, 1, 14, txtMailTo.Text, txtMailCC.Text, txtSubject.Text, divPreviewEmail.InnerHtml, "0", LoggedInUser.glUserId);
                ModalPopupEmail.Hide();
                //lblStatus.Text = "";

                if (Result == 0)
                {
                    lblShowError.Text = "Customer Email Sent Successfully!";
                    lblShowError.CssClass = "success";
                    //dvMailSend.Visible = false;
                }
                else if (Result == 1)
                {
                    lblShowError.Text = "System Error! Please try after sometime!";
                    lblShowError.CssClass = "errorMsg";
                }
                else if (Result == 2)
                {
                    lblShowError.Text = "PreAlert Email Already Sent!";
                    lblShowError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblShowError.Text = "Email Sending Failed! Please Enter Comma-Seperated Email Addresses";
                lblShowError.CssClass = "errorMsg";
            }
        }
    }

    private bool SendPreAlertEmail()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "";

        strCustomerEmail = txtMailTo.Text.Trim();
        strCCEmail = txtMailCC.Text.Trim();
        strSubject = txtSubject.Text.Trim();

        strCCEmail = strCCEmail.Replace(";", ",").Trim();
        strCCEmail = strCCEmail.Replace(" ", ",").Trim();
        strCCEmail = strCCEmail.Replace(",,", ",").Trim();
        strCCEmail = strCCEmail.Replace("\r", "").Trim();
        strCCEmail = strCCEmail.Replace("\n", "").Trim();
        strCCEmail = strCCEmail.Replace("\t", "").Trim();

        strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(" ", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
        strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();

        bool bEmailSucces = false;

        if (strCustomerEmail == "" || strSubject == "")
        {
            lblPopMessageEmail.Text = "Please Enter Customer Email!";
            //       lblError.CssClass = "errorMsg";
            return bEmailSucces;
        }
        else
        {
            if (chkDraft.SelectedValue == "1")
            {
                StringBuilder strStyle = new StringBuilder();
                strStyle = strStyle.Append("<html><body style='height:100%; width:100%; font-family:Arial; font-style:normal; font-size:9pt; color:#000;");

                // body header
                strStyle = strStyle.Append(@"<table cellpadding='0' width='850' cellspacing='0' id='topTable'><tr valign='top'>");
                strStyle = strStyle.Append(@"<td styleInsert='1' height='150' style='border:1px solid darkgray; border-radius: 6px; bEditID:r3st1; color:#000000; bLabel:main; font-size:12pt; font-family:calibri;'>");
                strStyle = strStyle.Append(@"<table border='0' cellpadding='5' width='850' cellspacing='5' height='150' style='padding:10px'>");

                strStyle = strStyle.Append(@"<tr><td>" + "Dear Sir / Madam, " + "<br />");
                strStyle = strStyle.Append(@"</td></tr>");

                strStyle = strStyle.Append(@"<tr><td>" + txtRemark.Text);
                strStyle = strStyle.Append(@"</td></tr>");

                string strJobRefNo = "", strCustName = "", strConsigneeName = "", strCustRefNo = "", strToMail = "", strJobType = "";
                int strAgencyCnt = 0, strRIMCnt = 0, strColSpan = 0, strCols = 0;
                string args; string[] args1; int tot; int AmtTot = 0;
                DataView dv = DBOperations.GetUserDetail(Convert.ToString(LoggedInUser.glUserId));
                DataTable dt = dv.ToTable(true, "sEmail");
                DataSet dsAlertDetail = DBOperations.GetBillDispatchDetail(JobId, Session["JobRefNo"].ToString());

                //////////////////////////////////////////////////////////////////////////////////
                if (strAgencyCnt > strRIMCnt)
                {
                    strCols = strAgencyCnt;
                }
                else
                {
                    strCols = strRIMCnt;
                }

                strStyle = strStyle.Append(@"<tr><td><div class='subtle-wrap' style='box-sizing: border-box; padding: 5px 10px 20px; margin-top: 2px;'>");
                strStyle = strStyle.Append(@"<div class='content-body article-body' style='box-sizing: border-box; word-wrap: break-word; line-height: 20px; margin-top: 6px;'>");
                strStyle = strStyle.Append(@"<p style='color:rgb(0, 0, 0); font-family: calibri; font-size: 12pt; box-sizing: border-box;'>");
                strStyle = strStyle.Append(@"<table border='0' cellpadding='0' cellspacing='0' width='50%'><colgroup><col width='40%' /><col width='35%' /><col width='30%' /><col width='30%' /></colgroup>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Job Ref No </td>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString() + "</td></tr>");
                strStyle = strStyle.Append(@"<tr><td style='border: 1px solid #ccc; background-color:#99CCFF'>Cust Ref No </td>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + dsAlertDetail.Tables[0].Rows[0]["CustRefNo"].ToString() + "</td></tr>");

                ///Agency Details
                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice No </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVNO"].ToString();
                args1 = args.Split(',');
                tot = args1.Length;
                if (tot == strCols) { strColSpan = 0; }
                else { strColSpan = strCols; }
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Invoice Date </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVDate"].ToString();
                args1 = args.Split(',');
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'  colspan=" + strColSpan + ">" + args1[i] + "</td>");
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>Agency Invoice Amount </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["AgencyINVAmt"].ToString();
                args1 = args.Split(',');
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
                    if (args1[i] == "") { args1[i] = "0"; }
                    AmtTot = AmtTot + Convert.ToInt32(args1[i]);
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>Agency Total Amount </td>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
                strStyle = strStyle.Append(@"</tr>");
                tot = 0; AmtTot = 0;

                //RIM Details
                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice No </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["RimINVNO"].ToString();
                args1 = args.Split(',');
                tot = args1.Length;
                if (tot == strCols) { strColSpan = 0; }
                else { strColSpan = strCols; }
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Invoice Date </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["RimINVDate"].ToString();
                args1 = args.Split(',');
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strColSpan + ">" + args1[i] + "</td>");
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;'>RIM Invoice Amount </td>");
                args = dsAlertDetail.Tables[0].Rows[0]["RimINVAmt"].ToString();
                args1 = args.Split(',');
                for (int i = 0; i <= tot - 1; i++)
                {
                    strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#FCD5B4;' colspan=" + strColSpan + ">" + args1[i] + "</td>");
                    if (args1[i] == "") { args1[i] = "0"; }
                    AmtTot = AmtTot + Convert.ToInt32(args1[i]);
                }
                strStyle = strStyle.Append(@"</tr>");

                strStyle = strStyle.Append(@"<tr>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF'>RIM Total Amount </td>");
                strStyle = strStyle.Append(@"<td style='border: 1px solid #ccc; background-color:#99CCFF' colspan=" + strCols + ">" + AmtTot + "</td></tr>");
                strStyle = strStyle.Append(@"</tr>");
                AmtTot = 0; tot = 0;

                strStyle = strStyle.Append(@"</table></p></div></div></td>");

                // body footer
                strStyle = strStyle.Append(@"<tr><td>" + "Any billing related query or issue, kindly drop an e-mail to query.billing@babajishivram.com" + "<br/><br/>");
                strStyle = strStyle.Append(@"</td></tr>");
                strStyle = strStyle.Append(@"<tr><td>" + "Thanks & Regards");
                strStyle = strStyle.Append(@"</td></tr>");
                strStyle = strStyle.Append(@"<tr><td>" + "Babaji Shivram Clearing And Carriers Pvt Ltd");
                strStyle = strStyle.Append(@"</td></tr>");
                strStyle = strStyle.Append(@"</table></td></tr>");
                strStyle = strStyle.Append(@"</center></body></html>");

                MessageBody = strStyle.ToString();

                //MessageBody = txtRemark.Text;
            }
            else
            {
                MessageBody = divPreviewEmail.InnerHtml;
            }


            List<string> lstFilePath = new List<string>();

            foreach (GridViewRow gvRow in gvDocAttach.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkAttach")).Checked == true)
                {
                    HiddenField hdnDocPath = (HiddenField)gvRow.FindControl("hdnDocPath");

                    lstFilePath.Add(hdnDocPath.Value);
                }
            }

            bEmailSucces = EMail.SendMailMultiAttach(LoggedInUser.glUserName, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);

            return bEmailSucces;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveDocumentDetails();
        //CheckDocExist(Convert.ToInt32(hdnJobId.Value));
        string DocId = "", DocLid = "";
        DataTable dtTotDOc = DBOperations.GetTotalBillDoc();
        rptDocument.DataSource = dtTotDOc;
        rptDocument.DataBind();
        DataTable dtDoc = DBOperations.GetBillDoc(Convert.ToInt32(hdnJobId.Value));
        if (dtDoc.Rows.Count > 0)
        {
            int i = 0;
            foreach (DataRow rows in dtDoc.Rows)
            {

                DocId = rows["DocId"].ToString();
                foreach (DataRow rw in dtTotDOc.Rows)
                {
                    DocLid = rw["lid"].ToString();
                    if (DocId == DocLid)
                    {
                        if (rw["lid"].ToString().Trim().Contains(DocId.ToString()))
                            dtTotDOc.Rows.Remove(rw);
                        break;
                    }
                }
                i++;
            }
            rptDocument.DataSource = dtTotDOc;
            rptDocument.DataBind();
        }

        trBillDispatchDocDetail.Visible = true;

        DataView dvDoc = (DataView)SqlDataSourceBillDispatchDoc.Select(DataSourceSelectArguments.Empty);
        DataTable dtBillDoc = dvDoc.Table;
        if (!dtBillDoc.Columns.Contains("NewColumn"))
        {
            DataColumn newCol = new DataColumn("NewColumn", typeof(string));
            newCol.AllowDBNull = true;
            dtBillDoc.Columns.Add(newCol);
        }

        int j = 0;
        foreach (DataRow rows in dtBillDoc.Rows)
        {
            string DocPath = rows["DocPath"].ToString();
            String ServerPath = FileServer.GetFileServerDir();
            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                ServerPath = ServerPath.Replace("PCA\\", "");
            }
            else
            {
                ServerPath = ServerPath + DocPath;
            }

            //FileInfo info = new FileInfo(ServerPath);
            //decimal length = info.Length;
            //dtBillDoc.Rows[j]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
            //j++;
        }
        gvBillDispatchDocDetail.DataSource = dtBillDoc;
        if (dtBillDoc.Columns.Contains("NewColumn"))
        {
            if (gvBillDispatchDocDetail.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtBillDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvBillDispatchDocDetail.Columns.Add(test);
            }
        }
        gvBillDispatchDocDetail.DataBind();
    }

    protected void lnkMailSend_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Show();
    }

    protected void CheckDocExist(int JobId)
    {
        string DocId = "", DocLid = "", DocName = "", DocNm = "";
        DataTable dtTotDOc = DBOperations.GetTotalBillDoc();
        rptDocument.DataSource = dtTotDOc;
        rptDocument.DataBind();
        DataTable dtDoc = DBOperations.GetBillDoc(JobId);
        if (dtDoc.Rows.Count > 0)
        {
            int i = 0;
            foreach (DataRow rows in dtDoc.Rows)
            {
                DocId = rows["DocId"].ToString();
                DocName = rows["DocName"].ToString();
                foreach (DataRow rw in dtTotDOc.Rows)
                {
                    DocLid = rw["lid"].ToString();
                    DocNm = rw["SName"].ToString();

                    if (DocName == DocNm)
                    {
                        if (rw["lid"].ToString().Trim().Contains(DocId.ToString()))
                        {
                            dtTotDOc.Rows.Remove(rw);
                            lblMsg.Text = "Some Document Already Added!!!";
                            lblMsg.CssClass = "success";
                            Div2.Visible = true;
                            lnkPreAlertEmailDraft.Visible = true;
                        }

                        break;
                    }

                }
                i++;
            }
            rptDocument.DataSource = dtTotDOc;
            rptDocument.DataBind();

            trBillDispatchDocDetail.Visible = true;
            trMailDetail.Visible = false;

            //lblMsg.Text = "Some Document Already Added!!!";
            //lblMsg.CssClass = "success";
            //Div2.Visible = true;
            //lnkPreAlertEmailDraft.Visible = true;
        }
        else
        {
            Div2.Visible = true;
            lnkPreAlertEmailDraft.Visible = false;
            trrptDoc.Visible = true;
            rptDocument.Visible = true;
            trBillDispatchDocDetail.Visible = false;
            trMailDetail.Visible = false;
            btnSave.Visible = true;
        }
        MpeFileUpload.Show();
    }

    protected void rpDocument_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkDocumentType = (CheckBox)e.Item.FindControl("chkDocType");

            FileUpload fileUploadDocument = (FileUpload)e.Item.FindControl("fuDocument");
            RequiredFieldValidator RFVFileUpload = (RequiredFieldValidator)e.Item.FindControl("RFVFile");

            if (chkDocumentType != null && fileUploadDocument != null)
            {
                chkDocumentType.Attributes.Add("OnClick", "javascript:toggleDiv1('" + chkDocumentType.ClientID + "','" + fileUploadDocument.ClientID + "','" + RFVFileUpload.ClientID + "');");
            }
        }
    }

    protected void SaveDocumentDetails()
    {
        int JobId = Convert.ToInt32(Session["JobId"]);
        hdnJobId.Value = Session["JobId"].ToString();
        string strUploadPath = hdnUploadPath.Value;
        int PCDDocType = Convert.ToInt32(EnumPCDDocType.BillingAdvice);
        int Result = -1;

        foreach (RepeaterItem itm in rptDocument.Items)
        {
            CheckBox chk = (CheckBox)(itm.FindControl("chkDocType"));
            HiddenField hdnDocId = (HiddenField)itm.FindControl("hdnDocId");
            string strCustDocFolder = "", strJobFileDir = "";
            hdnUploadPath.Value = strCustDocFolder + strJobFileDir;
            if (chk.Checked)
            {
                FileUpload fuDocument = (FileUpload)(itm.FindControl("fuDocument"));
                string strFilePath = "", FileName = "";

                int DocumentId = Convert.ToInt32(hdnDocId.Value);

                FileName = fuDocument.FileName;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDoc(strCustDocFolder, FileName);

                    string[] args = strFilePath.Split(',');
                    fuDocument.SaveAs(args[1]);

                    if (FileName != "")
                    {
                        Result = DBOperations.AddDocPathForBillDispatch(FileName, args[0], DocumentId, JobId, LoggedInUser.glUserId);
                    }
                }

                if (Result == 0)
                {
                    lblMsg.Text = "Document Uploaded Successfully!!!!";
                    lblMsg.CssClass = "success";
                    lnkPreAlertEmailDraft.Visible = true;
                }

                chk.Checked = false;
            }
        }
        MpeFileUpload.Show();
    }

    private string UploadPCDDoc(string FilePath, string fileName)
    {
        //string FileName = fuDocument.FileName;

        if (FilePath == "")
            FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fileName != string.Empty)
        {
            if (!System.IO.File.Exists(ServerFilePath + fileName))
            {
                //string ext = Path.GetExtension(fuPCDUpload.FileName);
                string ext = Path.GetExtension(fileName);
                //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                fileName = Path.GetFileNameWithoutExtension(fileName);
                string FileId = RandomString(5);

                fileName += "_" + FileId + ext;
            }

        }

        return FilePath + fileName + "," + ServerFilePath + fileName;
    }

    protected void CheckMailSend(int JobID, string JobRefNo)
    {
        //---
        string DocId = "", DocPath = "";

        DataView dvDoc = (DataView)SqlDataSourceBillDispatchDoc.Select(DataSourceSelectArguments.Empty);
        DataTable dtDoc = dvDoc.Table;

        DataColumn newCol = new DataColumn("NewColumn", typeof(string));
        newCol.AllowDBNull = true;
        dtDoc.Columns.Add(newCol);
        int i = 0;
        foreach (DataRow rows in dtDoc.Rows)
        {
            DocPath = rows["DocPath"].ToString();
            if (DocPath != null)
            {
                String ServerPath = FileServer.GetFileServerDir();
                if (ServerPath == "")
                {
                    ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocPath);
                    ServerPath = ServerPath.Replace("PCA\\", "");
                }
                else
                {
                    ServerPath = ServerPath + DocPath;
                }

                //FileInfo info = new FileInfo(ServerPath);
                //decimal length = info.Length;
                //dtDoc.Rows[i]["NewColumn"] = decimal.Round(length / (1000000), 2) + " Mb";
                //i++;
            }

        }
        gvBillDispatchDocDetail.DataSource = dtDoc;
        if (dtDoc.Columns.Contains("NewColumn"))
        {
            if (gvBillDispatchDocDetail.Columns.Count == 5)
            {
                BoundField test = new BoundField();
                test.DataField = Convert.ToString(dtDoc.Columns[5]);
                test.HeaderText = "File Size";
                gvBillDispatchDocDetail.Columns.Add(test);
            }
        }
        gvBillDispatchDocDetail.DataBind();

        GridViewMailDetail.DataSource = SqlDataSourceMailDetail;
        GridViewMailDetail.DataBind();

        DataSet dsAlertDetail = DBOperations.GetBillDispatchDetail(JobID, JobRefNo);
        if (dsAlertDetail.Tables.Count > 0)
        {
            if (dsAlertDetail.Tables[0].Rows.Count > 0)
            {
                string status = dsAlertDetail.Tables[0].Rows[0]["Mail"].ToString();
                lblcustName.Text = dsAlertDetail.Tables[0].Rows[0]["Customer"].ToString();
                lblJobRegNo.Text = JobRefNo;//dsAlertDetail.Tables[0].Rows[0]["JobRefNo"].ToString();

                if (gvBillDispatchDocDetail.Rows.Count > 0)
                {
                    if (status == "Upload & Mail Send")
                    {
                        rptDocument.Visible = true;
                        btnSave.Visible = true;
                        gvBillDispatchDocDetail.Visible = true;
                        trrptDoc.Visible = true;
                        trMailDetail.Visible = false;
                        trBillDispatchDocDetail.Visible = true;
                        lblTitle.Text = "File Upload";
                        PanelFileUpload.Height = 450;
                    }
                    else
                    {
                        rptDocument.Visible = false;
                        btnSave.Visible = false;
                        //gvBillDispatchDocDetail.DataSource = SqlDataSourceBillDispatchDoc;
                        gvBillDispatchDocDetail.DataBind();
                        trrptDoc.Visible = false;
                        trMailDetail.Visible = true;
                        trBillDispatchDocDetail.Visible = true;
                        lnkPreAlertEmailDraft.Visible = false;
                        lblMsg.Text = "Already Mail Send to Customer!!!";
                        lblMsg.CssClass = "success";
                        lblTitle.Text = "Mail Detail";
                        PanelFileUpload.Height = 300;
                    }
                }
                else
                {
                    lnkPreAlertEmailDraft.Visible = false;
                }
            }
            //else
            //{
            //    lnkPreAlertEmailDraft.Visible = false;
            //}

        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        LinkButton DocPath = gvBillDispatchDocDetail.FindControl("lnkDownload") as LinkButton;
        HiddenField doc = gvBillDispatchDocDetail.FindControl("hdnDoc") as HiddenField;
        string a = DocPath.Text;
    }

    protected void gvBillDispatchDocDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    #endregion

    #region MyPacco Dispatch
    protected void btnMyPaccoAWBGeneration_Click(object sender, EventArgs e)
    {
        int i = 0;

        string JobType = "";
        hdnCustomerId.Value = "0";
        hdnCustomerName.Value = "";
        ViewState["AWBJobid"] = "";

        foreach (GridViewRow gvr in gvcustomer.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                DataSet ds = new DataSet();
                string Customerid = gvcustomer.DataKeys[gvr.RowIndex].Value.ToString();
                string Customername = gvr.Cells[1].Text;
                ViewState["jobid"] = "";

                GridView gvRecievedJobDetail = (GridView)gvr.FindControl("gvRecievedJobDetail");

                foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
                {
                    if (gvr1.RowType == DataControlRowType.DataRow)
                    {
                        string jobid = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString();

                        LinkButton lnkJobNo = (LinkButton)gvr1.FindControl("lnkJobNo");
                        CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                        if (chk1.Checked)
                        {
                            JobType = gvr1.Cells[4].Text;
                            hdnCustomerId.Value = Customerid;
                            hdnCustomerName.Value = Customername;
                            LinkButton lnkJobNo1 = (LinkButton)gvr1.FindControl("lnkJobNo");

                            LinkButton lnk = (LinkButton)gvr1.FindControl("txtUploadPath");

                            string[] commandArgs = lnk.Text.Split(new char[] { ';' });
                            string strCustDocFolder = "", strJobFileDir = "";

                            if (commandArgs[0].ToString() != "")
                                strCustDocFolder = commandArgs[0].ToString();
                            if (commandArgs[1].ToString() != "")
                                strCustDocFolder = commandArgs[1].ToString();

                            if (commandArgs[2].ToString() != "")
                                strJobFileDir = commandArgs[2].ToString();

                            LinkButton lnk1 = (LinkButton)gvr1.FindControl("lnkDocument");

                            //if (strJobFileDir.Contains(".doc") || strJobFileDir.Contains(".xls") || strJobFileDir.Contains(".xlsx"))
                            //{
                            if (ViewState["AWBJobid"].ToString() == "")
                            {
                                ViewState["AWBJobid"] = jobid;
                                hdnJobId.Value = jobid.ToString();
                            }
                            else
                            {
                                ViewState["AWBJobid"] = ViewState["AWBJobid"].ToString() + ',' + jobid;
                            }

                            i++;
                            //}
                            //else
                            //{
                            //    lblMsgforApproveReject.Text = "Please First Generate Covering Letter For My Pacco Dispatch!.";
                            //    lblMsgforApproveReject.CssClass = "errorMsg";
                            //    i = -1;
                            //    break;
                            //}
                        }
                    }
                }

                if (i > 0)
                {
                    String strjobIdList = ViewState["AWBJobid"].ToString();

                    // Check IF AWB Already Generated Against Job List

                    DataSet dsJobAWB = DBOperations.MyPaccoCheckAWBForJobList(strjobIdList);

                    if (dsJobAWB.Tables.Count > 0)
                    {
                        if (dsJobAWB.Tables[0].Rows.Count > 0)
                        {
                            lblMsgforApproveReject.Text = "My Pacco Order Already Created.";
                            lblMsgforApproveReject.CssClass = "errorMsg";

                            return;
                        }
                    }

                    DataSet dsPlantAddressList = DBOperations.GetJobPlantAddressList(strjobIdList);

                    gvDispatchPlantAddress.DataSource = dsPlantAddressList;
                    gvDispatchPlantAddress.DataBind();

                    ModalPopupDispatch.Show();

                    i = 0;
                }
                else if (i == 0)
                {
                    // lblMsgforApproveReject.Text = "Please Check atleast one CheckBox For AWB Generation.";
                    // lblMsgforApproveReject.CssClass = "errorMsg";
                }
                else if (i == -1)
                {
                    lblMsgforApproveReject.Text = "Please First Generate Covering Letter For My Pacco Dispatch.";
                    lblMsgforApproveReject.CssClass = "errorMsg";
                }

            }
        }

    }

    protected void btnGenerateAWB_Click(object sender, EventArgs e)
    {
        int BranchID = Convert.ToInt32(ddBranch.SelectedValue);
        int PlantAddressID = 0;
        string strWarehouseCode = "MWH0000012448"; // Test Code

        if (BranchID == 3)// Mumbai
        {
            strWarehouseCode = "MWH0000012511";
        }
        else if (BranchID == 5)// Delhi
        {
            strWarehouseCode = "MWH0000008110";
        }
        else if (BranchID == 6)//Chennai
        {
            strWarehouseCode = "MWH0000003885";
        }
        foreach (GridViewRow row in gvDispatchPlantAddress.Rows)
        {
            CheckBox chk = row.Cells[0].Controls[1] as CheckBox;
            if (chk != null && chk.Checked)
            {
                PlantAddressID = Convert.ToInt32(gvDispatchPlantAddress.DataKeys[row.RowIndex].Value);
            }
        }

        if (PlantAddressID == 0)
        {
            ModalPopupDispatch.Show();
            lblDispatchMessage.Text = "Please check Delivery Address!";
            lblDispatchMessage.CssClass = "errorMsg";

            lblMsgforApproveReject.Text = "Please check Delivery Address!";
            lblMsgforApproveReject.CssClass = "errorMsg";
            return;
        }
        //else
        //{
        //    ModalPopupDispatch.Show();
        //    lblDispatchMessage.Text = "Please Address Found!";
        //    lblDispatchMessage.CssClass = "errorMsg";

        //    return;
        //}

        String strjobIdList = ViewState["AWBJobid"].ToString();

        DataSet dsPlantAddress = DBOperations.GetCustomerPlantAddressById(PlantAddressID);

        if (strjobIdList != "")
        {
            if (dsPlantAddress.Tables.Count > 0 && dsPlantAddress.Tables[0].Rows.Count > 0)
            {
                // Generate AWB
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                Root MyPaccoRoot = new Root();
                Datum MyPaccoDatum = new Datum();
                Order MyPaccoOrder = new Order();

                List<Datum> lstDatum = new List<Datum>();
                List<Order> lstOrder = new List<Order>();

                string strBabajiOrderNo = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")).ToString(); // DateTime.Today.TimeOfDay.ToString();
                DateTime dtAWBDate = DateTime.Now.AddDays(1);

                MyPaccoRoot.access_token = "";

                // MyPaccoResqAddOrderPl AWBBillDetail = new MyPaccoResqAddOrderPl();

                // MyPaccoOrder.seller_email = "amit.bakshi@babajishivram.com";

                MyPaccoOrder.seller_order_number = strBabajiOrderNo;
                MyPaccoOrder.seller_warehouse_code = strWarehouseCode;

                //MyPaccoOrder.pickup_fullname = "DO NOT PICKUP";// "Babaji Shivram";
                //MyPaccoOrder.pickup_mobile = "9833708840";
                //MyPaccoOrder.pickup_email = "amit.bakshi@babajishivram.com";
                //MyPaccoOrder.pickup_address = "For Testing, Pls Do Not Pickup "; //"Plot No 2A, Sakinka Andheri E Mumbai 450072";
                //MyPaccoOrder.pickup_pincode = "450072"
                ;
                //MyPaccoOrder.pickup_landmark = "Test";
                //MyPaccoOrder.pickup_city = "Test";
                //MyPaccoOrder.pickup_state = "Test";
                //MyPaccoOrder.pickup_country = "IN";

                MyPaccoOrder.delivery_fullname = dsPlantAddress.Tables[0].Rows[0]["ContactPerson"].ToString();
                MyPaccoOrder.delivery_mobile = dsPlantAddress.Tables[0].Rows[0]["MobileNo"].ToString();

                // Check If Customer Email Address is Valid

                string strDeliveryEmail = "sajid.shaikh@babajishivram.com";

                if (dsPlantAddress.Tables[0].Rows[0]["Email"] != DBNull.Value)
                {
                    strDeliveryEmail = dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                if (IsValidEmail(strDeliveryEmail))
                {
                    MyPaccoOrder.delivery_email = strDeliveryEmail;
                }
                else
                {
                    MyPaccoOrder.delivery_email = "sajid.shaikh@babajishivram.com";// dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                MyPaccoOrder.delivery_address = hdnCustomerName.Value + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine1"].ToString() + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine2"].ToString();
                MyPaccoOrder.delivery_pincode = dsPlantAddress.Tables[0].Rows[0]["Pincode"].ToString();
                MyPaccoOrder.delivery_landmark = "-";
                MyPaccoOrder.delivery_city = dsPlantAddress.Tables[0].Rows[0]["City"].ToString();
                MyPaccoOrder.delivery_state = "-";
                MyPaccoOrder.delivery_country = "IN";
                MyPaccoOrder.pickup_date = dtAWBDate.ToString("yyyy-MM-dd");

                MyPaccoOrder.transport_mode = "1";
                MyPaccoOrder.payment_type = "1";
                MyPaccoOrder.currency_unit = "INR";

                MyPaccoOrder.item_title = "Document";
                MyPaccoOrder.item_desc = "Document";
                MyPaccoOrder.item_quantity = "1";
                MyPaccoOrder.length = "15";
                MyPaccoOrder.height = "10";
                MyPaccoOrder.width = "8";
                MyPaccoOrder.weight = "0.5";
                MyPaccoOrder.base_price = "500";
                MyPaccoOrder.shipp_handling_charges = "0";
                MyPaccoOrder.other_charges = "0";
                MyPaccoOrder.total_amount = "500";

                lstOrder.Add(MyPaccoOrder);

                MyPaccoDatum.rts_order = true;
                MyPaccoDatum.orders = lstOrder;

                lstDatum.Add(MyPaccoDatum);
                MyPaccoRoot.data = lstDatum;

                MyPaccoSession objMyPaccoSession = new MyPaccoSession();

                if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) > 0 || objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken == "")
                {

                    MyPaccoResqAccessToken objMyPaccoResqAccessToken = new MyPaccoResqAccessToken();
                    MyPaccoClientID objMyPaccoClientID = new MyPaccoClientID();
                    List<MyPaccoClientID> lstMyPaccoClientID = new List<MyPaccoClientID>();

                    lstMyPaccoClientID.Add(objMyPaccoClientID);
                    objMyPaccoResqAccessToken.access_token = "";

                    objMyPaccoClientID.client_id = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientId;// "9702420066";
                    objMyPaccoClientID.client_secret = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientPassword;// "Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA";

                    objMyPaccoResqAccessToken.data = lstMyPaccoClientID;

                    //string strJsonPayLoad= "{\"access_token\": \"\",\"data\": [{\"client_id\": \"9702420066\",\"client_secret\": \"Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA\"}]}";

                    string strJsonPayLoad = serializer.Serialize(objMyPaccoResqAccessToken);

                    MyPaccoRespAccessToken txnRespWithObj = MYPACCOAPI.GetAuthTokenAsync(objMyPaccoSession, strJsonPayLoad);
                    txnRespWithObj.IsSuccess = true;
                    if (txnRespWithObj.IsSuccess)
                    {
                        MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                        MyPaccoShared.SaveAPILoginDetails(objMyPaccoSession.MyPaccoApiLoginDetails);
                    }
                    else //if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) >= 0)
                    {
                        txnRespWithObj.IsSuccess = false;
                        lblMsgforApproveReject.Text = "Error : MyPacco Token Expired. Please Get AuthToken for Login";
                        lblMsgforApproveReject.CssClass = "errorMsg";

                        ModalPopupDispatch.Show();

                        return;
                    }
                }

                // Generate MyPacco AWB

                MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                string strAWBPayload = serializer.Serialize(MyPaccoRoot);

                MyPaccoRespAddOrder objMyPaccoRespAddOrder = MYPACCOAPI.GenAWBAsync(objMyPaccoSession, strAWBPayload);

                /**********Test * *******************
                MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();

                objMyPaccoRespAddOrder.IsSuccess = true;

                MyPaccoRespOrderDeail objOrderDetail = new MyPaccoRespOrderDeail();

                objOrderDetail.mypacco_order_id = "BM" + strBabajiOrderNo;
                objOrderDetail.seller_order_number = strBabajiOrderNo.ToString();

                List<MyPaccoRespOrderDeail> lstOrderDetail = new List<MyPaccoRespOrderDeail>();
                lstOrderDetail.Add(objOrderDetail);

                objMyPaccoRespAddOrder.Data = lstOrderDetail;
                /*********************************/

                if (objMyPaccoRespAddOrder.IsSuccess == true)
                {
                    // Save Job Details against AWB No
                    string strOrderNo = "", strAWBNo = "", strLSPName = "";

                    int CustomerId = 0;

                    Int32.TryParse(hdnCustomerId.Value, out CustomerId);

                    strOrderNo = objMyPaccoRespAddOrder.Data[0].mypacco_order_id;

                    if (objMyPaccoRespAddOrder.Data[0].awb_number != null)
                    {
                        strAWBNo = objMyPaccoRespAddOrder.Data[0].awb_number;
                    }
                    if (objMyPaccoRespAddOrder.Data[0].lsp_name != null)
                    {
                        strLSPName = objMyPaccoRespAddOrder.Data[0].lsp_name;
                    }

                    DBOperations.MyPaccoAddAWBNo(strOrderNo, strAWBNo, dtAWBDate, strLSPName, strjobIdList, CustomerId, BranchID, PlantAddressID, LoggedInUser.glUserId);

                    lblMsgforApproveReject.Text = "Success : My Pacco Order No: " + strOrderNo;
                    lblMsgforApproveReject.CssClass = "success";

                    ModalPopupDispatch.Hide();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('" + lblMsgforApproveReject.Text + "');", true);
                }
                else if (objMyPaccoRespAddOrder.Message != "")
                {
                    lblMsgforApproveReject.Text = "Error : " + objMyPaccoRespAddOrder.Message;
                    lblMsgforApproveReject.CssClass = "errorMsg";

                    ModalPopupDispatch.Show();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMsgforApproveReject.Text + "');", true);
                }
                else
                {
                    lblMsgforApproveReject.Text = "Error : MyPacco AWB Generation Error";
                    lblMsgforApproveReject.CssClass = "errorMsg";

                    ModalPopupDispatch.Show();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMsgforApproveReject.Text + "');", true);
                }
            }
            else
            {
                lblreceivemsg.Text = "Plant Address Not Found!";
                lblreceivemsg.CssClass = "errorMsg!";
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblreceivemsg.Text + "');", true);
            }
        }// Job Type Not Found
        else
        {
            lblreceivemsg.Text = "Please Select Job for MyPacco Disptch!";
            lblreceivemsg.CssClass = "errorMsg!";

        }

    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    protected void chkDraft_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkDraft.SelectedValue == "1")
        {
            divPreviewEmail.Visible = false;
            txtRemark.Visible = true;
            ModalPopupEmail.Show();
        }
        else
        {
            divPreviewEmail.Visible = true;
            ModalPopupEmail.Show();
            txtRemark.Visible = false;
        }
    }
}