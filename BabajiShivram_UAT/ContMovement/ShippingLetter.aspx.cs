using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class ContMovement_ShippingLetter : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvLetters);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Shipping Letters";
        if (!IsPostBack)
        {

        }
        DataFilter1.DataSource = SqlDataSourceLetters;
        DataFilter1.DataColumns = gvLetters.Columns;
        DataFilter1.FilterSessionID = "ShippingLetter.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int ShippingId = 0;
        if (ddlShippingMS.SelectedValue != "0")
            ShippingId = Convert.ToInt32(ddlShippingMS.SelectedValue);

        if (ShippingId > 0 && txtLetterName.Text.Trim() != "")
        {
            int Result = CMOperations.AddShipperLetter(ShippingId, txtLetterName.Text.Trim(), loggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Successfully added letter.";
                lblError.CssClass = "success";
                ddlShippingMS.DataBind();
                txtLetterName.Text = "";
                gvLetters.DataBind();
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
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShippingLetter.aspx");
    }

    protected void gvLetters_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "delete")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            int Result = CMOperations.DeleteShippingLetter(lid, loggedInUser.glUserId);
            if (Result == 0)
            {
                lblError.Text = "Successfully deleted letter.";
                lblError.CssClass = "success";
                gvLetters.DataBind();
            }
            else if (Result == 2)
            {
                lblError.Text = "Letter does not exists!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error while adding up letter.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void gvLetters_PreRender(object sender, EventArgs e)
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
        string strFileName = "ShippingLetters_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLetters.AllowPaging = false;
        gvLetters.AllowSorting = false;
        gvLetters.Columns[5].Visible = false;
        gvLetters.Caption = "Shipping Letters On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "ShippingLetter.aspx";
        DataFilter1.FilterDataSource();
        gvLetters.DataBind();
        gvLetters.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "ShippingLetter.aspx";
            DataFilter1.FilterDataSource();
            gvLetters.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
}