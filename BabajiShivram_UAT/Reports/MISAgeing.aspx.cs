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
public partial class Reports_rptMISAgeing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkAgeingXls);
        ScriptManager1.RegisterPostBackControl(lnkAgeingDetailXls);
        if (!IsPostBack)
        {
            Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   = "MIS Aging";
        }
    }
    
    protected void gvAgeing_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            GridViewRow rowUser = gvAgeing.SelectedRow;

            int rowIndex = Convert.ToInt32(commandArgs[0]);
            string strReportType = commandArgs[1];

            string strRangeLow = gvAgeing.DataKeys[rowIndex].Values["RangeLow"].ToString();
            string strRangeHigh = gvAgeing.DataKeys[rowIndex].Values["RangeHigh"].ToString();

            if (strRangeHigh == "0")
            {
                strRangeHigh = "9999";
            }
            
            BindDetailReport(strRangeLow, strRangeHigh, strReportType);
        }
    }

    private void BindDetailReport(string RangeLow, string RangeHigh, string ReportType)
    {
        string strCaptionText = "";
        pnlJobDetailXLS.Visible = false;

        if (Convert.ToInt32(RangeHigh) > 0)
        {
            AgeingDetailSqlDataSource.SelectParameters["RangeLow"].DefaultValue = RangeLow;
            AgeingDetailSqlDataSource.SelectParameters["RangeHigh"].DefaultValue = RangeHigh;
            AgeingDetailSqlDataSource.SelectParameters["ReportType"].DefaultValue = ReportType;

          //  gvAgeingJobDetail.DataSource = AgeingDetailSqlDataSource;
            gvAgeingJobDetail.DataBind();

            if (gvAgeingJobDetail.Rows.Count > 0)
            {
                pnlJobDetailXLS.Visible = true;

                if (ReportType == "1")
                {
                    strCaptionText = "Job Opening: PreAlert Received Date To Job Opening Date";
                }
                else if (ReportType == "2")
                {
                    strCaptionText = "Checklist Preparation: Job Opening To Checklist Preparation Date";
                }
                else if (ReportType == "3")
                {
                    strCaptionText = "Checklist Audit/Approval: Checklist Request Date To Audit/Approval";
                }
                else if (ReportType == "4")
                {
                    strCaptionText = "Noting: Date of Checklist Audit/Approval To Noting Date";
                }
                else if (ReportType == "5")
                {
                    strCaptionText = "DO Collection: IGM Date To Final DO Date";
                }
                else if (ReportType == "6")
                {
                    strCaptionText = "Duty: Duty Request Date To Duty Payment Date";
                }
                else if (ReportType == "7")
                {
                    strCaptionText = "IGM Clearance: IGM Date To Clearance Date";
                }
                else if (ReportType == "8")
                {
                    strCaptionText = "DO Clearance: Final DO Date To Clearance Date";
                }
                else if (ReportType == "9")
                {
                    strCaptionText = "Duty Clearance: Duty Payment Date To Clearance Date";
                }
                else if (ReportType == "10")
                {
                    strCaptionText = "Job Clearance: Job Date To Clearance Date";
                }

                gvAgeingJobDetail.Caption = "<Div style='background-color:#CBCBDC;color:Balck;font-weight: bold;font-family: Arial;" +
                    "font-style: normal;font-size: 9pt;'>" + strCaptionText + "</Div>";
            }//END_IF
        }//END_IF
        else
        {
            gvAgeingJobDetail.DataSource = null;
            gvAgeingJobDetail.DataBind();
            gvAgeingJobDetail.Caption = "No Record Found!";
        }
    }

    protected void gvAgeing_PreRender(Object Sender, EventArgs e)
    {
        gvAgeing.Rows[gvAgeing.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
    }

    protected void gvAgeingJobDetail_PreRender(object sender, EventArgs e)
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    
    protected void lnkAgeingXls_Click(object sender, EventArgs e)
    {
        string strFileName = "JobAgeing_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        AgeingExport("attachment;filename=" +strFileName, "application/vnd.ms-excel");
    }
    
    private void AgeingExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvAgeing.AllowPaging = false;
        gvAgeing.AllowSorting = false;

        gvAgeing.DataSourceID = "AgeingSqlDataSource";
        gvAgeing.DataBind();

        //Remove Controls
        this.RemoveControls(gvAgeing);

        gvAgeing.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void lnkAgeingDetailXls_Click(object sender, EventArgs e)
    {
        string strFileName = "JobAgeingDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        string header = "attachment;filename="+strFileName;
        string contentType = "application/vnd.ms-excel";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        
        gvAgeingJobDetail.AllowPaging = false;
        gvAgeingJobDetail.AllowSorting = false;

        gvAgeingJobDetail.DataSourceID = "AgeingDetailSqlDataSource";
        gvAgeingJobDetail.DataBind();

        gvAgeingJobDetail.RenderControl(hw);

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
