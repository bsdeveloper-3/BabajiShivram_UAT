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
public partial class Reports_MISCustomerDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            if (Session["MISCustomerId"] != null)
            {
               string strCustName  = DBOperations.GetCustomerNameById(Session["MISCustomerId"].ToString());

               Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
               lblTitle.Text = "MIS - "+strCustName;
            }
            else
            {
                Response.Redirect("MISCustomer.aspx");
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("MISCustomer.aspx");
    }

    protected void gvCustomerJobDetail_PreRender(object sender, EventArgs e)
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
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        
        string strFileName = lblTitle.Text.Replace(" ","") + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExcelExport("attachment;filename="+ strFileName, "application/vnd.ms-excel");
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

	gvCustomerJobDetail.AllowPaging = false;
        
	gvCustomerJobDetail.DataSourceID = "CustomerJobDetailSqlDataSource";
        gvCustomerJobDetail.DataBind();

        gvCustomerJobDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}
