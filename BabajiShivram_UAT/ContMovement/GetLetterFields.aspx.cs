using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class ContMovement_GetLetterFields : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvFields);
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Shipping Letter Fields";
        if (!IsPostBack)
        {
            if (gvFields.Rows.Count == 0)
            {
                lblError.Text = "No fields found!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = SqlDataSourceFields;
        DataFilter1.DataColumns = gvFields.Columns;
        DataFilter1.FilterSessionID = "GetLetterFields.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnAddNewField_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddShippingField.aspx");
    }

    protected void gvFields_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "delete")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());

            int Result = CMOperations.DeleteShippingLetter(lid, loggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Successfully deleted field name.";
                lblError.CssClass = "success";
                gvFields.DataBind();
            }
            else if (Result == 2)
            {
                lblError.Text = "Field does not exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up field name.";
                lblError.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "addcolumn")
        {
            int ShippingId = Convert.ToInt32(e.CommandArgument.ToString());
            if (ShippingId > 0)
            {
                Session["FieldId"] = ShippingId.ToString();
                Response.Redirect("AddTableHeader.aspx");
            }
        }
    }

    protected void gvFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Structure") != DBNull.Value)
            {
                string Structure = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Structure"));
                if (Structure.ToLower().Trim() == "field")
                {
                    e.Row.Cells[1].Text = "";
                }
            }
        }
    }

    protected void gvFields_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #region EXPORT METHODS
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "ShippingFields_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvFields.AllowPaging = false;
        gvFields.AllowSorting = false;
        gvFields.Columns[0].Visible = false;
        gvFields.Caption = "Shipping Letter Fields On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "GetLetterFields.aspx";
        DataFilter1.FilterDataSource();
        gvFields.DataBind();
        gvFields.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

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
            DataFilter1.FilterSessionID = "GetLetterFields.aspx";
            DataFilter1.FilterDataSource();
            gvFields.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}