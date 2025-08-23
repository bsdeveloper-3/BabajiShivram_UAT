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
public partial class Reports_JobClearanceAgeing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ScriptManager1.RegisterPostBackControl(lnkPortJobXls);
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MIS Clearance: Job Aging";

            if (Session["MISPortId"] == null)
            {
              //  Response.Redirect("MISPort.aspx");
            }
        }

    }

    protected void gvJobDetailAgeing_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    
    #region Export To Excel

    protected void lnkPortJobXls_Click(object sender, EventArgs e)
    {
        ExcelExport("attachment;filename=MIS_JobClearanceAging.xls", "application/vnd.ms-excel");
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

        gvJobDetailAgeing.AllowPaging = false;
        gvJobDetailAgeing.AllowSorting = false;

        gvJobDetailAgeing.DataSourceID = "JobDetailAgeingSqlDataSource";
        gvJobDetailAgeing.DataBind();

        gvJobDetailAgeing.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}
