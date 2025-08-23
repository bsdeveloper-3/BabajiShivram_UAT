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
public partial class Reports_UserReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "BS Group User Report";

            DBOperations.FillDivisionList(lstGroupList);
            
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        ShowReport();
    }
    
    private void ShowReport()
    {
        string strGroupId = "";

        if (lstGroupList.SelectedIndex != -1)
        {
            foreach (ListItem item in lstGroupList.Items)
            {
                if (item.Selected)
                {
                    strGroupId += item.Value + ",";
                }
            }
        }
        if (strGroupId != "")
        {
            datasrcUserReport.SelectParameters["strGroupId"].DefaultValue = strGroupId;
        }
        
        gvUserReport.DataSource = datasrcUserReport;
        gvUserReport.DataBind();

        if (gvUserReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }
            
    protected void btnCancel_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("UserReport.aspx");
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ExportFunction("attachment;filename=GroupUserReport.xls", "application/vnd.ms-excel");
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
        
        ShowReport();
        gvUserReport.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
     
    }
    #endregion
}
