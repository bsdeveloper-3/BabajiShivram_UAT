using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
public partial class Reports_EWayBoEReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkReportXls);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "EWay Bill BoE Data";

        }
    }

    protected void btnShowJob_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        hdnJobId.Value = "0";

        DataSet ds = DBOperations.GetJobDetailByJobRefNo(txtJobNumber.Text.Trim());

        if(ds.Tables.Count > 0)
        {
            if(ds.Tables[0].Rows.Count > 0)
            {
                hdnJobId.Value = ds.Tables[0].Rows[0]["JobId"].ToString();
                gvReport.DataBind();
            }
        }

        if(hdnJobId.Value == "0")
        {
            lblError.Text = "Job details not found!";
            lblError.CssClass = "errorMsg";
        }
        
    }
    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {

    }
    #region ExportData
    protected void lnkReportXls_Click(object sender, EventArgs e)
    {
        string strFileName = "E-Way_BoE_Report_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";
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
        gvReport.AllowPaging = false;
        gvReport.AllowSorting = false;

        gvReport.DataBind();

        gvReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}