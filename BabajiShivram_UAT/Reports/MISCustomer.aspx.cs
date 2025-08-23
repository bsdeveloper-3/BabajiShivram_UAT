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
public partial class Reports_MISCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkCustomerXls);
        if (!IsPostBack)
        {
            Session["MISCustomerId"] = null;
            Session["MISTransMode"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "MIS Customer";
        }

        //
        DataFilter1.DataSource = MISCustomerSqlDataSource;
        DataFilter1.DataColumns = gvCustomerWiseJob.Columns;
        DataFilter1.FilterSessionID = "MISCustomer.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    protected void gvCustomerWiseJob_SelectedIndexChanged(object sender, EventArgs e)
    {
        int rowIndex = gvCustomerWiseJob.SelectedRow.RowIndex;

        string strCustomerId = gvCustomerWiseJob.DataKeys[rowIndex]["CustomerId"].ToString();
        string strTransModeId = gvCustomerWiseJob.DataKeys[rowIndex]["TransModeId"].ToString();

        if (strCustomerId != "0")
        {
            Session["MISCustomerId"]    = strCustomerId;
            Session["MISTransMode"]     = strTransModeId;
            Response.Redirect("MISCustomerDetail.aspx");
        }
    }

    protected void gvCustomerWiseJob_PreRender(Object Sender, EventArgs e)
    {        
        if(gvCustomerWiseJob.Rows.Count > 2)
            gvCustomerWiseJob.Rows[gvCustomerWiseJob.Rows.Count - 1].BackColor = System.Drawing.Color.FromName("#CBCBDC");
    }

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
            DataFilter1.FilterSessionID = "MISCustomer.aspx";
            DataFilter1.FilterDataSource();
            gvCustomerWiseJob.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
    #region Export To Excel

    protected void lnkCustomerXls_Click(object sender, EventArgs e)
    {
        string strFileName = "MIS_Customer_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExcelExport("attachment;filename=" +strFileName, "application/vnd.ms-excel");
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
        gvCustomerWiseJob.AllowPaging = false;

        gvCustomerWiseJob.Columns[1].Visible = false;
        gvCustomerWiseJob.Columns[2].Visible = true;

        gvCustomerWiseJob.DataSourceID = "MISCustomerSqlDataSource";
        gvCustomerWiseJob.DataBind();

        //Remove Controls
        this.RemoveControls(gvCustomerWiseJob);

        gvCustomerWiseJob.RenderControl(hw);

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
