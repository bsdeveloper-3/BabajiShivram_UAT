using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
public partial class InvoiceTrack_InvoiceReceivedAC : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Invoice Receive Detail";

        DataFilter1.DataSource = PCDNonReceivedSqlDataSource;
        DataFilter1.DataColumns = gvNonRecievedJobDetail.Columns;
        DataFilter1.FilterSessionID = "PCDBillingScrutiny.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }
        
    protected void TabJobDetail_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabJobDetail.ActiveTabIndex == 0)
        {
            gvNonRecievedJobDetail.DataBind();
            
        }
        if (TabJobDetail.ActiveTabIndex == 1)
        {            
            gvRecievedJobDetail.DataBind();

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
    
    }
    protected void btnReceive_Click(object sender, EventArgs e)
    {
        int i = 0;
        int Result = 0;
        int CurrentStatus = 2; // Forwarded To A/C
        foreach (GridViewRow gvr in gvNonRecievedJobDetail.Rows)
        {
            if (((CheckBox)gvr.FindControl("chk1")).Checked)
            {
                LinkButton Recv = (LinkButton)gvr.FindControl("lnkTokanNo");
                string TokanID = Recv.CommandArgument;
                string s = string.Empty;

                Result = DBOperations.UpdateInvoiceReceived(Convert.ToInt32(TokanID), CurrentStatus, LoggedInUser.glUserId);

                if (Result == 0)
                {
                    lblreceivemsg.Text = "A/C Invoice Recieved Successfully!";
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
                //DataFilter2_OnDataBound();
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
                lblMsgforNonReceived.Text = "No Tokan Found For Non Recieved Invoice!";
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
    
    
    #endregion
}