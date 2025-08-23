using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Service_ListKPI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    #region GridView Event

    protected void gvKPIList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvKPIList.EditIndex = e.NewEditIndex;

        gvKPIList.DataBind();
    }

    protected void gvKPIList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int KPIID = Convert.ToInt32(gvKPIList.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtEmpName  = (TextBox)gvKPIList.Rows[e.RowIndex].FindControl("txtEmpName");
        TextBox txtEmpCode  = (TextBox)gvKPIList.Rows[e.RowIndex].FindControl("txtEmpCode");
        TextBox txtEmpEmail = (TextBox)gvKPIList.Rows[e.RowIndex].FindControl("txtEmpEmail");
        DropDownList ddHOD = (DropDownList)gvKPIList.Rows[e.RowIndex].FindControl("ddHOD");

        string strEmpName = "", strEmpCode = "", strEmpEmail = "";
        int HODID   = 0;

        strEmpName = txtEmpName.Text.Trim();
        strEmpCode = txtEmpCode.Text.Trim();
        strEmpEmail = txtEmpEmail.Text.Trim();

        HODID = Convert.ToInt32(ddHOD.SelectedValue);
        
        if (strEmpName== "" || strEmpCode == "" || strEmpEmail == "" || HODID == 0)
        {

            lblError.Text = "Please Enter Required Details!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else
        {

            int result =DBOperations.KPI_UpdateEmpTarget(KPIID, strEmpName, strEmpEmail, strEmpCode,HODID,"", 1);

            if (result == 0)
            {
                lblError.Text = "Detail Updated Successfully.";
                lblError.CssClass = "success";

                gvKPIList.EditIndex = -1;
                e.Cancel = true;

            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
       
        }//END_ELSE

    }

    protected void gvKPI_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvKPIList.EditIndex = -1;
    }

    #endregion

    #region Export Data

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "KPI_EMP_LIST_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvKPIList.AllowPaging = false;
        gvKPIList.AllowSorting = false;

        gvKPIList.Columns[0].Visible = false;


        gvKPIList.DataSourceID = "DataSourceKPI";
        gvKPIList.DataBind();


        gvKPIList.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
     
}