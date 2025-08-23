using System;
using System.Collections.Generic;
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

public partial class ExportReports_MISPort : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkbtnExport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MIS Port";
            Session["PendingPortId"] = null;
        }
    }

    protected void lnkbtnExport_Click(object sender, EventArgs e)
    {
        string strFileName = "MISPort_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void gvPortWiseJob_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            int PortId = Convert.ToInt32(e.CommandArgument);

            if (PortId > 0)
            {
                Session["PendingPortId"] = PortId;
                Response.Redirect("MISPortDetail.aspx");
            }
        }
    }

    protected void gvPortWiseJob_PreRender(Object Sender, EventArgs e)
    {
        gvPortWiseJob.Rows[gvPortWiseJob.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
    }

    #region Export To Excel

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

        gvPortWiseJob.AllowPaging = false;
        gvPortWiseJob.AllowSorting = false;

        gvPortWiseJob.DataSourceID = "DataSourcePortwise";
        gvPortWiseJob.DataBind();

        //Remove Controls
        this.RemoveControls(gvPortWiseJob);
        gvPortWiseJob.RenderControl(hw);

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