using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class FreightExport_FRExpTracking : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Session["EnqId"] = null;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Freight Detail";

            if (LoggedInUser.glType == (Int32)EnumUserType.FreightAgent)
            {
                btnNewEnquiry.Visible = false;
            }

        }
        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = gvFreight.Columns;
        DataFilter1.FilterSessionID = "FRExpTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void btnNewEnquiry_Click(object sender, EventArgs e)
    {
        Response.Redirect("FRExpEnquiry.aspx");
    }

    protected void gvFreight_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {
            string strEnqId = (string)e.CommandArgument;
            Session["EnqId"] = strEnqId;

            Response.Redirect("FRExpDetail.aspx");
        }
    }

    protected void gvFreight_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "FRExpTracking.aspx";
            DataFilter1.FilterDataSource();
            gvFreight.DataBind();
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
        string strFileName = "FreightStatus_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        gvFreight.AllowPaging = false;
        gvFreight.AllowSorting = false;

        gvFreight.Columns[1].Visible = false;
        gvFreight.Columns[2].Visible = true;

        gvFreight.Caption = "Freight Status Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "FRExpTracking.aspx";
        DataFilter1.FilterDataSource();
        gvFreight.DataBind();

        gvFreight.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

    }
    #endregion
}