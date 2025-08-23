using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Transport_TransDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "BSCCPL";
    }

    #region Export To Excel

    protected void lnkCategoryXls_Click(object sender, EventArgs e)
    {
        string strFileName = "MaintenanceCategoryExpense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        CategoryExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void lnkVehicleXls_Click(object sender, EventArgs e)
    {
        string strFileName = "MaintenanceVehicleExpense_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        VehicleExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void CategoryExport(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvCategoryMonth.AllowPaging = false;
        gvCategoryMonth.AllowSorting = false;
        
        gvCategoryMonth.DataSourceID = "DataSourceCategory";
        gvCategoryMonth.DataBind();

        //Remove Controls
        this.RemoveControls(gvCategoryMonth);

        gvCategoryMonth.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    private void VehicleExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvVehicleMonth.AllowPaging = false;
        gvVehicleMonth.AllowSorting = false;

        gvVehicleMonth.DataSourceID = "DataSourceVehicle";
        gvVehicleMonth.DataBind();

        //Remove Controls
        this.RemoveControls(gvCategoryMonth);

        gvVehicleMonth.RenderControl(hw);

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