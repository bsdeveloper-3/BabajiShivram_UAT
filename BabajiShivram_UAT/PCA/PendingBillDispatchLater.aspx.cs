using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data.SqlClient;

public partial class PCA_PendingBillDispatchLater : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkreceive);
        ScriptManager1.RegisterPostBackControl(btnCoveringLetter);
        ScriptManager1.RegisterPostBackControl(gvRecievedJobDetail);

        if (!IsPostBack)
        {
            Session["CHECKED_ITEMS"] = null; //Checkbox
            Session["JobId"] = null;
            Session["CoverJobIdList"] = null;
            Session["CoverCustomerId"] = null;

            Session["DispatchJobIdList"] = null;
            Session["DispatchCustomerId"] = null;

            Session["BillJobIdList"] = null;
            Session["BillJobId"] = null;
            Session["CoverCustomerId"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Dispatch Later";

        }

        DataFilter2.DataSource = SqlDataSourceCustomer;
        DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
        DataFilter2.FilterSessionID = "PendingBillDispatchLater.aspx";
        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
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
    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter2_OnDataBound();
        }
    }
    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "PendingBillDispatch.aspx";
            DataFilter2.FilterDataSource();
            gvRecievedJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "jobselect")
        {
            Session["BillJobId"] = e.CommandArgument.ToString();

            Response.Redirect("BillDispatch.aspx");
        }                
    }

    protected void gvRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }

    protected void btnCoveringLetter_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        string strCustomerId = "";

        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";

                    if (strCustomerId == "")
                    {
                        strCustomerId = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString();
                    }
                    else if (strCustomerId != gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString())
                    {
                        // Different Customer Selected For Covering Letter Generation
                        lblMessage.Text = "Please Select Job for One Customer Only!";
                        lblMessage.CssClass = "errorMsg";

                        // break;
                    }
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for Covering Letter.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["CoverJobIdList"] = strJobidList;
            Session["CoverCustomerId"] = strCustomerId;

            Response.Redirect("CoverDispatchList.aspx");

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

    protected void btnEBill_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value.ToString() + ",";
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for E-Bill";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["BillJobIdList"] = strJobidList;

            Response.Redirect("BillDispatchList.aspx");

        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        bool bApprove = true;
        int reasonforPendency = 0;

        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");

                if (chk1.Checked)
                {
                    int jobid = Convert.ToInt32(gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Value);

                    int result = 1;// BillingOperation.ApproveRejectDispatching(jobid, bApprove, "", reasonforPendency, "", 0, LoggedInUser.glUserId);

                    if (result == 0)
                    {
                        lblMessage.Text = "Bill Dispatch Completed! Job Moved To Dispatch Department!.";
                        lblMessage.CssClass = "success";
                        //---------------------start Covering letter-------------------
                        //gvRecievedJobDetail.DataBind();
                        gvRecievedJobDetail.DataBind();
                        //---------------------end Covering letter---------------------
                    }
                    else if (result == 1)
                    {
                        lblMessage.Text = "System Error! Please try after sometime.";
                        lblMessage.CssClass = "errorMsg";
                    }
                    else if (result == 2)
                    {
                        lblMessage.Text = "Bill Dispatch Already Completed!";
                        lblMessage.CssClass = "errorMsg";
                    }
                }
            }
        }
    }

    protected void btnMyPaccoAWBGeneration_Click(object sender, EventArgs e)
    {
        string strJobidList = "";
        string strCustomerId = "";

        foreach (GridViewRow gvr1 in gvRecievedJobDetail.Rows)
        {
            if (gvr1.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)gvr1.Cells[1].FindControl("chkjoball");
                LinkButton lnkJobRefNo = (LinkButton)gvr1.Cells[2].FindControl("lnkJobNo");

                if (chk1.Checked)
                {
                    int CoverLetterGenerated = BillingOperation.CheckDispatchCoverLetter(Convert.ToInt32(gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[0]), 1);

                    if (CoverLetterGenerated == 0)
                    {
                        lblMessage.Text = lnkJobRefNo.Text + " - Please First Generate Dispatch Covering Letter!";
                        lblMessage.CssClass = "errorMsg";

                        return;
                    }

                    strJobidList = strJobidList + gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[0].ToString() + ",";

                    if (strCustomerId == "")
                    {
                        strCustomerId = gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString();
                    }
                    else if (strCustomerId != gvRecievedJobDetail.DataKeys[gvr1.RowIndex].Values[1].ToString())
                    {
                        // Different Customer Selected For Covering Letter Generation
                        lblMessage.Text = "Please Select Job for One Customer Only!";
                        lblMessage.CssClass = "errorMsg";

                        //break;
                    }
                }
            }
        }

        if (strJobidList == "")
        {
            lblMessage.Text = "Please Select Job for MyPacco Dispatch.";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            Session["DispatchJobIdList"] = strJobidList;
            Session["DispatchCustomerId"] = strCustomerId;

            Response.Redirect("MyPaccoDispatchList.aspx");

        }
    }

    #region Export To Excel
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkreceive_Click(object sender, EventArgs e)
    {
        string strFileName = "Bill_Dispatch_Pending" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ReceivejoblistExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    private void ReceivejoblistExport(string header, string contentType)
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

        gvRecievedJobDetail.Columns[1].Visible = false;
        gvRecievedJobDetail.Columns[2].Visible = false;
        gvRecievedJobDetail.Columns[3].Visible = true;

        gvRecievedJobDetail.DataSourceID = "SqlDataSourceCustomer";
        gvRecievedJobDetail.DataBind();

        //Remove Controls
        //this.RemoveControls(gvRecievedJobDetail);

        gvRecievedJobDetail.RenderControl(hw);

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
}