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
public partial class Reports_MISPort : System.Web.UI.Page
{
    /*
    int grdTotalJob = 0, grdTotalCon40 = 0, grdTotalCon20 = 0,
        grdTotalLCL = 0, grdTotalNoOfPackages = 0, grdTotalGrossWt = 0;
    */
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkPortXls);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MIS Port";

            Session["PendingPortId"] = null;
        }

    }

        //protected void ddFCL_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddFCL.SelectedValue == "1")
    //    {
    //        gvPortWiseJob.Columns[5].Visible = false;
    //        gvPortWiseJob.Columns[3].Visible = true;
    //        gvPortWiseJob.Columns[4].Visible = true;
    //    }
    //    else if (ddFCL.SelectedValue == "2")
    //    {
    //        gvPortWiseJob.Columns[5].Visible = true;
    //        gvPortWiseJob.Columns[3].Visible = false;
    //        gvPortWiseJob.Columns[4].Visible = false;
    //    }
    //    else
    //    {
    //        gvPortWiseJob.Columns[3].Visible = true;
    //        gvPortWiseJob.Columns[4].Visible = true;
    //        gvPortWiseJob.Columns[5].Visible = true;
    //    }
    //}

    protected void gvPortWiseJob_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            int PortId  =   Convert.ToInt32(e.CommandArgument);
            
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

    protected void lnkPortXls_Click(object sender, EventArgs e)
    {
        string strFileName = "PortJobReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExcelExport("attachment;filename="+strFileName, "application/vnd.ms-excel");
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

    /*
    protected void gvPortWiseJob_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int rowTotalJob = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "NoOfJobs"));
            grdTotalJob += rowTotalJob;

            int rowCon40 = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Con40"));
            grdTotalCon40 += rowCon40;

            int rowCon20 = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Con20"));
            grdTotalCon20 += rowCon20;

            int rowLCL = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LCL"));
            grdTotalLCL += rowLCL;

            int rowNoOfPackages = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "NoOfPackages"));
            grdTotalNoOfPackages += rowNoOfPackages;

            int rowGrossWt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GrossWt"));
            grdTotalGrossWt += rowGrossWt;

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblgrdTotalJob = (Label)e.Row.FindControl("lblgrdJobCount");
            lblgrdTotalJob.Text = grdTotalJob.ToString();

            Label lblgrdCon40 = (Label)e.Row.FindControl("lblgrdCon40");
            lblgrdCon40.Text = grdTotalCon40.ToString();

            Label lblgrdCon20 = (Label)e.Row.FindControl("lblgrdCon20");
            lblgrdCon20.Text = grdTotalCon20.ToString();

            Label lblLCL = (Label)e.Row.FindControl("lblgrdLCL");
            lblLCL.Text = grdTotalLCL.ToString();

            Label lblNoOfPackages = (Label)e.Row.FindControl("lblgrdNoOfPackages");
            lblNoOfPackages.Text = grdTotalNoOfPackages.ToString();

            Label lblGrossWt = (Label)e.Row.FindControl("lblgrdGrossWt");
            lblGrossWt.Text = grdTotalGrossWt.ToString();
        }
    }
    */
}
