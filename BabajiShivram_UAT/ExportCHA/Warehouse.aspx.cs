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
using System.Net;
using System.Drawing;

public partial class ExportCHA_Warehouse : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkDeliveredItems);
        ScriptManager1.RegisterPostBackControl(GridViewVehicle);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Warehouse";
        if (!IsPostBack)
        {
            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Warehouse!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = PendingJobForWarehouse;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "Warehouse.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region Gridview Events    
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
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["JobId"] = e.CommandArgument.ToString();
            Response.Redirect("UpdateWarehouse.aspx");
        }
        else if (e.CommandName.ToLower() == "getdelivereditems")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });

            Session["JobId"] = commandArgs[0].ToString();
            int Mode = Convert.ToInt32(commandArgs[1].ToString());
            if (Mode == 1) //Air
            {
                lblDeliveredSubject.Text = "Delivered Packages List";
            }
            else
            {
                lblDeliveredSubject.Text = "Delivered Containers List";
            }

            ModalPopupExtender2.Show();
            GridViewVehicle.DataBind();
        }
    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string VehiclePlaced = rowView["VehiclePlaced"].ToString();

            foreach (TableCell cell in e.Row.Cells)
            {
                if (VehiclePlaced.Trim().ToLower() == "no")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(247, 233, 134, 0.29)");
                    e.Row.ToolTip = "Vehicle Pending To Be Placed";
                }
                else if (VehiclePlaced.Trim().ToLower() == "yes")
                {
                    cell.BackColor = System.Drawing.Color.FromName("rgba(238, 170, 144, 0.87)");
                    e.Row.ToolTip = "Vehicle Placed";
                }
            }


            LinkButton lnkDeliveredItems = (LinkButton)e.Row.FindControl("lnkDeliveredItems");
            if (lnkDeliveredItems != null)
            {
                string DeliveredItems = "";

                DataRowView rowView2 = (DataRowView)e.Row.DataItem;
                int DeliveredPackages = Convert.ToInt32(rowView2["DeliveredPackages"].ToString());
                int NoOfPackages = Convert.ToInt32(rowView2["NoOfPackages"].ToString());
                int TransModeId = Convert.ToInt32(rowView2["TransModeId"].ToString());
                int TotalContainers = Convert.ToInt32(rowView2["TotalContainers"].ToString());
                int DeliveredContainer = Convert.ToInt32(rowView2["DeliveredContainer"].ToString());

                if (TransModeId == 1) // AIR - include packages
                {
                    DeliveredItems = DeliveredPackages.ToString() + " / " + NoOfPackages.ToString();
                    lnkDeliveredItems.Text = DeliveredItems.ToString();
                }
                else // SEA - include containers loaded
                {
                    DeliveredItems = DeliveredContainer.ToString() + " / " + TotalContainers.ToString();
                    lnkDeliveredItems.Text = DeliveredItems.ToString();
                }
            }
        }
    }
    #endregion

    #region POP UP EVENTS

    protected void btnCancelPopup_Click1(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }
    protected void lnkDeliveredItems_Click(object sender, EventArgs e)
    {
        string strFileName = "DeliveredItems" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        DeliveredItemsExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    private void DeliveredItemsExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridViewVehicle.AllowPaging = false;
        GridViewVehicle.AllowSorting = false;
        GridViewVehicle.Caption = "";
        GridViewVehicle.DataSourceID = "DataSourceVehicle";
        GridViewVehicle.DataBind();

        this.RemoveControls(GridViewVehicle); //Remove Controls
        GridViewVehicle.RenderControl(hw);
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
    protected void GridViewVehicle_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadPODDocument(DocPath);
        }
    }
    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowView2 = (DataRowView)e.Row.DataItem;
            int TransModeId = Convert.ToInt32(rowView2["TransModeId"].ToString());
            if (TransModeId == 1) // AIR - include packages
                GridViewVehicle.Columns[1].Visible = false;
            else
                GridViewVehicle.Columns[2].Visible = false;
        }
    }
    private void DownloadPODDocument(string DocumentPath)
    {
        //  String ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadExportFiles\\" + DocumentPath);
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

    #endregion

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            DataFilter1.FilterSessionID = "Warehouse.aspx";
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
        string strFileName = "PendingWarehouse_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;
        gvJobDetail.Caption = "Pending Warehouse On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "Warehouse.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

}