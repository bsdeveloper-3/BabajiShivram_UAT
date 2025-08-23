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

public partial class Reports_LicenceUtilizationReport : System.Web.UI.Page
{
    LoginClass LoggedInCustomer = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkXslSummary);
        ScriptManager1.RegisterPostBackControl(lnkCustomerXls);
        if (!IsPostBack)
        {


            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Licence Utilization Report";
        }

        //
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvCustomerLicense.Columns;
        DataFilter1.FilterSessionID = "LicenceUtilizationReport.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
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
            DataFilter1.FilterSessionID = "LicenceUtilizationReport.aspx";
            DataFilter1.FilterDataSource();
            gvCustomerLicense.DataBind();
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
        string strFileName = "Licence_Utilization_Report_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExcelExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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
        gvCustomerLicense.AllowPaging = false;

        gvCustomerLicense.Columns[1].Visible = false;
        gvCustomerLicense.Columns[2].Visible = true;

        gvCustomerLicense.DataSourceID = "GridviewSqlDataSource";
        gvCustomerLicense.DataBind();

        //Remove Controls
        this.RemoveControls(gvCustomerLicense);

        gvCustomerLicense.RenderControl(hw);

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

    #region GridView Event

    protected void gvCustomerLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvCustomerLicense.Visible = false;

        fsMainBorder.Visible = false;
        if (gvCustomerLicense.SelectedIndex == -1)
        {
            FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }
        FormView1.DataBind();
    }
    protected void gvCustomerLicense_PreRender(object sender, EventArgs e)
    {

        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion

    protected void lnkLicenseNo_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

        string strLicNumner = ((LinkButton)sender).Text.Trim();
        lblPopupMessage.Text = "Licnese No - " + strLicNumner;

        DsLicenseDetails.SelectParameters["LicenseNo"].DefaultValue = strLicNumner.ToString();
        gvLicenseDetails.DataSource = DsLicenseDetails;
        gvLicenseDetails.DataBind();
        ModalPopupExtender.Show();

    }

    protected void btnReturnLicense_Click(object sender, EventArgs e)
    {
        int LicenseID = Convert.ToInt32(FormView1.DataKey[0].ToString());
        //int LicenseId,DateTime dtReturnDate, string strDispatchAddress, int DispatchMode, string strDispatchName,int UserId

        string strReturnDate = ((TextBox)FormView1.FindControl("txtReturnDate")).Text.Trim();
        string strReturnAddress = ((TextBox)FormView1.FindControl("txtReturnAddress")).Text.Trim();
        int intDispatchMode = Convert.ToInt32(((DropDownList)FormView1.FindControl("ddDispatchMode")).SelectedValue);
        string strCourierName = ((TextBox)FormView1.FindControl("txtCourierName")).Text.Trim();

        DateTime dtReturnDate = DateTime.MinValue;

        if (strReturnDate != "")
        {
            dtReturnDate = Commonfunctions.CDateTime(strReturnDate);
        }
        else
        {
            lberror.Text = "Please Enter License Return Date!";
            lberror.CssClass = "errorMsg";
            return;
        }

        // Update License Return Date

        int Result = DBOperations.AddLicenseRetrun(LicenseID, dtReturnDate, strReturnAddress, intDispatchMode, strCourierName, LoggedInCustomer.glUserId);

        if (Result == 0)
        {
            lberror.Text = "License Return Details Updated Successfully";
            lberror.CssClass = "success";
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please Try After Sometime!";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "License Return Details Already Updated!";
            lberror.CssClass = "errorMsg";
        }
    }

    #region FormView Command
    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            gvCustomerLicense.Visible = true;
            gvCustomerLicense.SelectedIndex = -1;
            fsMainBorder.Visible = true;

        }
        else if (e.CommandName == "")
        {
            //btnNewLicense.Visible = true;
            //gvCustomerLicense.Visible = true;
            //fsMainBorder.Visible = true;
        }
    }
    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInInsertMode = true;
        }
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = true;
            e.KeepInEditMode = true;
        }
    }
    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Detail Updated Successfully !";
            lberror.CssClass = "success";
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Active License Detail Not Found!";
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    protected void lnkXslSummary_Click(object sender, EventArgs e)
    {
        string strFileName = lblPopupMessage.Text.Trim() + ".xls";
        LicenseListExport("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    private void LicenseListExport(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvLicenseDetails.AllowPaging = false;
        gvLicenseDetails.AllowSorting = false;
        gvLicenseDetails.Caption = "";

        //Remove Controls
        this.RemoveControls(gvLicenseDetails);

        gvLicenseDetails.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender.Hide();
    }
}