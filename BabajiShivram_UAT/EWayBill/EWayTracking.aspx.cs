using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class EWayBill_EWayTracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "EWay Bill Detail";
        }

        //
        DataFilter1.DataSource = SqlDataSourceBill;
        DataFilter1.DataColumns = GridViewEWay.Columns;
        DataFilter1.FilterSessionID = "EWayTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void GridViewEWay_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void GridViewEWay_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "print")
        {
            string strBillNo= (string)e.CommandArgument;
            Session["EwayPrint"] = strBillNo;

            //string strUserGSTIN = "27AAACN1163G1ZR"; // Navbharat
            //Session["userGSTIN"] = strUserGSTIN;

            Response.Redirect("PrintEWayBill.aspx");
        }
    }
    protected void GridViewEWay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // ACT - Active 
        // CNL: Cancelled
        // DIS: Discarded 
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "StatusName") != DBNull.Value)
            {
                string strStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StatusName"));
                if (strStatus.ToUpper() == "ACT")
                {
                    string strRejectStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "rejectStatus"));

                    if(strRejectStatus == "Y")
                    {
                        strRejectStatus = "Rejected";
                        e.Row.BackColor = System.Drawing.Color.FromName("#85f7f7");
                        e.Row.ToolTip = "STATUS: " + strRejectStatus;
                    }
                }
                else
                {
                    if(strStatus.ToUpper() == "CNL")
                    {
                        strStatus = "CNL: Cancelled";
                        e.Row.BackColor = System.Drawing.Color.FromName("#fc1140");
                    }
                    else if (strStatus.ToUpper() == "DIS")
                    {
                        strStatus = "DIS: Discarded";
                        e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    }
                                        
                    e.Row.ToolTip = "STATUS: " + strStatus;
                }
            }
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
            DataFilter1.FilterSessionID = "EWayTracking.aspx";
            DataFilter1.FilterDataSource();
            GridViewEWay.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "EWayBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        GridViewEWay.AllowPaging = false;
        GridViewEWay.AllowSorting = false;

        GridViewEWay.Columns[2].Visible = true;
        GridViewEWay.Columns[3].Visible = false;

        DataFilter1.FilterSessionID = "EWayTracking.aspx";
        DataFilter1.FilterDataSource();
        GridViewEWay.DataBind();

        GridViewEWay.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion
}