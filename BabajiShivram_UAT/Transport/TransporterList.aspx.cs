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

public partial class Transport_TransporterList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["TR_TransporterID"] = null; // Used Only For Transport Master 
    
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transporter Detail";

        }
        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = GridView1.Columns;
        DataFilter1.FilterSessionID = "TransporterList.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    #region GridViewEvent

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {
            string strCustomerId = e.CommandArgument.ToString();
            Session["TR_TransporterID"] = strCustomerId;

            Response.Redirect("TransporterTab.aspx");
        }
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "TransporterList.aspx";
            DataFilter1.FilterDataSource();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        ExportFunction("attachment;filename=Transporter_List.xls", "application/vnd.ms-excel");
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
        GridView1.AllowPaging = false;
        GridView1.AllowSorting = false;
        //GridView1.Columns[0].Visible = false;
        GridView1.Columns[1].Visible = false;
        GridView1.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "TransporterList.aspx";
        DataFilter1.FilterDataSource();

        //GridView1.DataSourceID = "GridViewSqlDataSource";
        GridView1.DataBind();

        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //// gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}


        GridView1.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}