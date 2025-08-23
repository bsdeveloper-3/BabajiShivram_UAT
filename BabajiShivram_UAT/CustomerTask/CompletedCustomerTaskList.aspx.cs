using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
public partial class CustomerTask_CompletedCustomerTaskList : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Completed Customer Task";
        if (!IsPostBack)
        {

            Session["TaskID"] = null;

            int UserId = LoggedInUser.glUserId;


        }

        DataFilter1.DataSource = PendingMiscellaneousCustomerTaskDataSorce;
        DataFilter1.DataColumns = GridComplitedMiscellaneousCustomerTask.Columns;
        // DataFilter1.FilterSessionID = "StockHistory.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }
    protected void GridComplitedMiscellaneousCustomerTask_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;

        }
    }

    protected void GridComplitedMiscellaneousCustomerTask_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strCustomerId = (string)e.CommandArgument;
            Session["TaskID"] = strCustomerId;
            Response.Redirect("UpdatePendingMiscellanceCustomerTask.aspx");
        }

        if (e.CommandName.ToLower() == "download")
        {
            string strFilePath = e.CommandArgument.ToString();
            DownloadDocument(strFilePath);
            GridComplitedMiscellaneousCustomerTask.DataBind();

        }
        else
        {


        }
    }

    protected void GridComplitedMiscellaneousCustomerTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strFilePath = "";

            if (DataBinder.Eval(e.Row.DataItem, "CustFilePath") != DBNull.Value)
                strFilePath = (string)DataBinder.Eval(e.Row.DataItem, "CustFilePath");


            if (strFilePath == "")
            {
                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");

                lnkDownload.Visible = false;

            }

        }

    }


    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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

    #region Export To Excel

    //protected void lnkGSTServicessXls_Click(object sender, EventArgs e)
    //{
    //    string strFileName3 = "GSTDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
    //    ExcelExport3("attachment;filename=" + strFileName3, "application/vnd.ms-excel");
    //}


    //private void ExcelExport3(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);

    //    GrvServicess.AllowPaging = false;
    //    GrvServicess.AllowSorting = false;

    //    GrvServicess.DataSourceID = "DataSorceServic";
    //    GrvServicess.DataBind();

    //    //Remove Controls
    //    this.RemoveControls(GrvServicess);

    //    GrvServicess.RenderControl(hw);

    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}
    

    //protected void lnkGSTMaterialXls_Click(object sender, EventArgs e)
    //{
    //    string strFileName2 = "GSTDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
    //    ExcelExport1("attachment;filename=" + strFileName2, "application/vnd.ms-excel");
    //}


    //private void ExcelExport2(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);

    //    GrvGSTMaterial.AllowPaging = false;
    //    GrvGSTMaterial.AllowSorting = false;

    //    GrvGSTMaterial.DataSourceID = "DataSorceGSTMaterial";
    //    GrvGSTMaterial.DataBind();

    //    //Remove Controls
    //    this.RemoveControls(GrvGSTMaterial);

    //    GrvGSTMaterial.RenderControl(hw);

    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}
    
    //protected void lnkGSTXls_Click(object sender, EventArgs e)
    //{
    //    string strFileName1 = "GSTDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
    //    ExcelExport1("attachment;filename=" + strFileName1, "application/vnd.ms-excel");
    //}


    //private void ExcelExport1(string header, string contentType)
    //{
    //    Response.Clear();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", header);
    //    Response.Charset = "";
    //    this.EnableViewState = false;
    //    Response.ContentType = contentType;
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);

    //    GrVGSTDetails.AllowPaging = false;
    //    GrVGSTDetails.AllowSorting = false;

    //    GrVGSTDetails.DataSourceID = "DataSorceGSTDetails";
    //    GrVGSTDetails.DataBind();

    //    //Remove Controls
    //    this.RemoveControls(GrVGSTDetails);

    //    GrVGSTDetails.RenderControl(hw);

    //    Response.Output.Write(sw.ToString());
    //    Response.End();
    //}



    protected void lnkPortXls_Click(object sender, EventArgs e)
    {
        string strFileName = "CustomerTask_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExcelExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        GridComplitedExelCustomerTask.Visible = true;
        GridComplitedExelCustomerTask.AllowPaging = false;
        GridComplitedExelCustomerTask.AllowSorting = false;

        GridComplitedExelCustomerTask.DataSourceID = "PendingMiscellaneousCustomerTaskDataSorce";
        GridComplitedExelCustomerTask.DataBind();

        //Remove Controls
        this.RemoveControls(GridComplitedExelCustomerTask);


        GridComplitedExelCustomerTask.RenderControl(hw);

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
            // DataFilter1.FilterSessionID = "StockHistory.aspx";
            DataFilter1.FilterDataSource();
            GridComplitedMiscellaneousCustomerTask.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}