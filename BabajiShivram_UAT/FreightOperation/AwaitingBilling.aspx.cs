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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class FreightOperation_AwaitingBilling : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["lblDEBITAMT"] = "0";
        ViewState["lblCREDITAMT"] = "0";
        ViewState["lblAMOUNT"] = "0";

        ScriptManager1.RegisterPostBackControl(lnknonreceive);
        ScriptManager1.RegisterPostBackControl(lnkreceive);
        

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending For Freight Billing";

            DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);


            DataFilter2.DataSource = PCDReceivedSqlDataSource;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
        }

        else
        {
            if (TabPCDBilling.TabIndex == 0)
            {
                if (TabJobDetail.ActiveTabIndex == 0)
                {
                    lblreceivemsg.Text = "";
                    lblerror.Text = "";
                    DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
                    DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
                    DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
                    DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

                }

            }
            if (TabPCDBilling1.TabIndex == 1)
            {
                if (TabJobDetail.ActiveTabIndex == 1)
                {
                    lblreceivemsg.Text = "";
                    lblMsgforReceived.Text = "";
                    lblerror.Text = "";
                    DataFilter2.DataSource = PCDReceivedSqlDataSource;
                    DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
                    DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
                    DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);

                }
            }

        }
        if (gvNonRecievedJobDetail.Rows.Count == 0)
        {
            lblMsgforNonReceived.Text = "No Job Found For Non Recieved file for Billing!";
            lblMsgforNonReceived.CssClass = "errorMsg";
        }
        else
        {
            lblMsgforNonReceived.Text = "";
        }
        if (gvRecievedJobDetail.Rows.Count == 0)
        {
            if (TabJobDetail.ActiveTabIndex == 1)
            {
                lblMsgforReceived.Text = "No Job Found For Recieved File For Billing!";
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

    }

    #region Non Recieved

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
        gvNonRecievedJobDetail.AllowPaging = false;
        gvNonRecievedJobDetail.AllowSorting = false;
        gvNonRecievedJobDetail.Caption = "";
        
        gvNonRecievedJobDetail.Columns[1].Visible = false;

        gvNonRecievedJobDetail.DataSourceID = "PCDNonReceivedSqlDataSource";
        gvNonRecievedJobDetail.DataBind();

        //Remove Controls
        this.RemoveControls(gvNonRecievedJobDetail);

        gvNonRecievedJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }


    protected void gvNonRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            
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
            index = (int)gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value;

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
                int index = (int)gvNonRecievedJobDetail.DataKeys[row.RowIndex].Value;

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


    protected void Receive_Click(object sender, EventArgs e)
    {
        int i = 0;
        int Result = 0;
      //  RememberOldValues();//Checkbox
      //  RePopulateValues();//Checkbox
      //  gvNonRecievedJobDetail.AllowPaging = false;//Checkbox
      //  gvNonRecievedJobDetail.DataBind();//Checkbox

        foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkEnqNo");
                string EnqID = Recv.CommandArgument;
                string s = string.Empty;

                Result = DBOperations.AddFreightBillingReceivedFile(Convert.ToInt32(EnqID), LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblreceivemsg.Text = "Billing File Recieved Successfully!";
                    lblreceivemsg.CssClass = "success";
                    gvNonRecievedJobDetail.DataBind();
                    gvRecievedJobDetail.DataBind();

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
        }// END_ForEach

        gvNonRecievedJobDetail.AllowPaging = true;//Checkbox
        gvNonRecievedJobDetail.DataBind();//Checkbox
    }

    #endregion

    #region Recieved

    protected void lnkreceiveExcel_Click(object sender, EventArgs e)
    {
        string strFileName = "FreightBilling_ReceiveJoblist" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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

        }

        if (e.Row.RowType == DataControlRowType.Header)
        {

        }

    }

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {
            string strEnqId = (string)e.CommandArgument;
            Session["EnqId"] = strEnqId;

            Response.Redirect("AgentInvoice.aspx");
        }
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
            DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            DataFilter1.FilterDataSource();
            gvNonRecievedJobDetail.DataBind();
            if (gvNonRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforNonReceived.Text = "No Job Found For Non Recieved File!";
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
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.FilterDataSource();
            gvRecievedJobDetail.DataBind();
            if (gvRecievedJobDetail.Rows.Count == 0)
            {
                lblMsgforReceived.Text = "No Job Found For Recieved file for Billing!";
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

    protected void TabJobDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabJobDetail.ActiveTabIndex == 0)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblerror.Text = "";
            DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
            DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
            DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
            gvNonRecievedJobDetail.DataBind();



        }
        if (TabJobDetail.ActiveTabIndex == 1)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            lblreceivemsg.Text = "";
            lblerror.Text = "";
            DataFilter2.DataSource = PCDReceivedSqlDataSource;
            DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
            DataFilter2.FilterSessionID = "PCDBillingScrutiny1.aspx";
            DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
            gvRecievedJobDetail.DataBind();

        }
    }
    
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
}