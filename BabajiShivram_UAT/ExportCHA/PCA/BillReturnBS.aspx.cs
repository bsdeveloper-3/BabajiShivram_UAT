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
public partial class PCA_BillReturnBS : System.Web.UI.Page
{
    LoginClass LoggedInClass = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        if (!IsPostBack)
        {
            if (gridBillReturn.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Detail Found For Billing!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = BillReturnSqlDataSource;
        DataFilter1.DataColumns = gridBillReturn.Columns;
        DataFilter1.FilterSessionID = "BillReturnBS.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    protected void gridBillReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblReturnReason = (Label)e.Row.FindControl("lblRetutnReason");
            DropDownList ddlReason = (DropDownList)e.Row.FindControl("ddlReason");
            Label lblReason = (Label)e.Row.FindControl("lblReason");

            if (lblReturnReason != null)
            {
                ddlReason.SelectedValue = lblReturnReason.Text.Trim();
                lblReason.Text = ddlReason.SelectedItem.Text.Trim();
            }
        }
    }

    protected void gridBillReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strJobId = "", strBJVlid = "", strBillRetLid = "";

            if (commandArgs[0].ToString() != "")
            {
                strJobId = commandArgs[0].ToString();
                //Session["JobId"] = strJobId;
            }
            if (commandArgs[1].ToString() != "")
            {
                strBJVlid = commandArgs[1].ToString();
                //Session["BJVlid"] = strBJVlid;
            }
            if (commandArgs[2].ToString() != "")
            {
                strBillRetLid = commandArgs[2].ToString();
                //Session["BJVlid"] = strBJVlid;
            }

            Session["JobId"] = strJobId;
            Session["BJVlid"] = strBJVlid;
            Session["BillRetLid"] = strBillRetLid;

            if (strJobId != "" && strBJVlid != "")
            {
                Response.Redirect("BillReturnDetailBS.aspx");
            }
            else
            {
                Response.Redirect("BillReturnBS.aspx");
            }
        }
    }

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
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
            DataFilter1.FilterSessionID = "BillReturnBS.aspx";
            DataFilter1.FilterDataSource();
            gridBillReturn.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "BillReturn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
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
        gridBillReturn.AllowPaging = false;
        gridBillReturn.AllowSorting = false;
       // gridBillReturn.Columns[1].Visible = false;
       // gridBillReturn.Columns[2].Visible = false;
       // gridBillReturn.Columns[3].Visible = true;
       //// gridBillReturn.Columns[13].Visible = false; // Document

        DataFilter1.FilterSessionID = "BillReturnBS.aspx";
        DataFilter1.FilterDataSource();
        gridBillReturn.DataBind();

        gridBillReturn.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void gridBillReturn_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
}