using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class PCA_MyPaccoOrderReturn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "My Pacco Order Return";
        if (!IsPostBack)
        {
            Session["JobId"] = null;

            if (gvOrderDetail.Rows.Count == 0)
            {
                lblMessage.Text = "Order Return Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //

        DataFilter1.DataSource = OrderSqlDataSource;
        DataFilter1.DataColumns = gvOrderDetail.Columns;
        DataFilter1.FilterSessionID = "MyPaccoOrderReturn.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.ToLower() == "select")
        //{
        //    string strJobId = (string)e.CommandArgument;            
        //    Session["JobId"] = strJobId;
        //    Response.Redirect("JobDetail.aspx"); ;
        //}
        if (e.CommandName.ToLower() == "select")
        {
            //string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            //string strJobId = "", strJobType = "";

            //if (commandArgs[0].ToString() != "")
            //    strJobId = commandArgs[0].ToString();
            //if (commandArgs[1].ToString() != "")
            //    strJobType = commandArgs[1].ToString();

            //Session["JobId"] = e.CommandArgument.ToString();

           
            //if (strJobType.ToLower().Trim() == "additional")
            //{
            //    Response.Redirect("AddtnlJobDetail.aspx");
            //}
            //else
            //{
            //    Response.Redirect("JobDetail.aspx");
            //}
        }

    }

    protected void gvOrderDetail_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "MyPaccoOrderReturn.aspx";
            DataFilter1.FilterDataSource();
            gvOrderDetail.DataBind();
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
        string strFileName = "MyPacco_Order_Return_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

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

        gvOrderDetail.AllowPaging = false;
        gvOrderDetail.AllowSorting = false;

        gvOrderDetail.Columns[1].Visible = false;
        // gvOrderDetail.Columns[2].Visible = true;

        // Excel Header Not Requierd-- Issue in excel header format after export
        // gvJobDetail.Caption = "Job Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "MyPaccoOrderReturn.aspx";
        DataFilter1.FilterDataSource();
        gvOrderDetail.DataBind();

        gvOrderDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion
}