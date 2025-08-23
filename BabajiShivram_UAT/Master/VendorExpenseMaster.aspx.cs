using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_VendorExpenseMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Expense Type Detail";

            int ExpId = Convert.ToInt32(Session["lId"]);
        }
       
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvExpense.Columns;
        DataFilter1.FilterSessionID = "VendorExpenseMaster.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);


    }
    
    #region FormView Event
    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "cancel")
        {
            gvExpense.SelectedIndex = -1;
            gvExpense.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;
            FormViewDataSource.SelectParameters[0].DefaultValue = "-1"; ;
        }
        else
        {
            gvExpense.Visible = false;
            DataFilter1.Visible = false;
            fsMainBorder.Visible = false;
        }
    }
    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        // Always Show Required Field Validator Message 

        Page.Validate("Required");
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
    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;
        }
    }
    #region FormView Insert,Update Delete
    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {

        //int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        //lberror.Visible = true;
        //if (Result > 0)
        //{
        //    lberror.Text = "Expesne Details Added Successfully.";
        //    lberror.CssClass = "success";
        //    FormViewDataSource.SelectParameters[0].DefaultValue = "-1"; ;
        //    gvExpense.SelectedIndex = -1;

        //    gvExpense.Visible = true;
        //    DataFilter1.Visible = true;
        //    fsMainBorder.Visible = true;
        //}
        //else if (Result == 0)
        //{
        //    lberror.Text = "System Error! Please try after sometime";
        //    lberror.CssClass = "errorMsg";
        //}
        //else if (Result == -1)
        //{
        //    lberror.Text = "Charge Code Already Exists. Please Use Different one!";
        //    lberror.CssClass = "errorMsg";

        //}
    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        //lberror.Visible = true;

        //int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        //if (Result == 0)
        //{
        //    lberror.Text = "Expesne Details Updated Successfully !";
        //    lberror.CssClass = "success";

        //    gvExpense.SelectedIndex = -1;
        //    gvExpense.DataBind();
        //    gvExpense.Visible = true;
        //    DataFilter1.Visible = true;
        //    fsMainBorder.Visible = true;
        //}
        //else if (Result == 1)
        //{
        //    lberror.Text = "System Error! Please try after sometime";
        //    lberror.CssClass = "errorMsg";
        //}
        //else if (Result == 2)
        //{
        //    lberror.Text = "Chaege Code Already Exists.!";
        //    lberror.CssClass = "errorMsg";
        //}

    }

    protected void FormviewSqlDataSource_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        //lberror.Visible = true;

        //int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        //if (Result == 0)
        //{
        //    lberror.Text = "Expense Details Deleted Successfully !";
        //    lberror.CssClass = "success";

        //    gvExpense.SelectedIndex = -1;
        //    gvExpense.DataBind();
        //    gvExpense.Visible = true;
        //    fsMainBorder.Visible = true;
        //}
        //else if (Result == 1)
        //{
        //    lberror.Text = "System Error! Please try after sometime";
        //    lberror.CssClass = "errorMsg";
        //}
    }
    #endregion

    protected void FormviewSqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            e.ExceptionHandled = true;

            lberror.Visible = true;
            lberror.Text = e.Exception.Message;
            lberror.CssClass = "errorMsg";
        }
    }

    #endregion

    #region GridView Event

    protected void gvExpesne_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvExpense.Visible = false;
        DataFilter1.Visible = false;
        fsMainBorder.Visible = false;
        if (gvExpense.SelectedIndex == -1)
        {
            FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }
        FormView1.DataBind();
    }

    protected void gvExpesne_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "VendorExpenseMaster.aspx";
            DataFilter1.FilterDataSource();
            gvExpense.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    protected void gvExpense_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            string ExpId =e.CommandArgument.ToString();
            
               Session["lId"] = ExpId;
        }
    }

    #region ExportData
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "ExpenseTypeMasterDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        gvExpense.AllowPaging = false;
        gvExpense.AllowSorting = false;
        gvExpense.Columns[1].Visible = false;
        gvExpense.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "VendorExpenseMaster.aspx";
        DataFilter1.FilterDataSource();
        gvExpense.DataBind();

        gvExpense.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region  Insert, Update and Delete

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int ExpId = 0;
        string ExpenseName = "", ChargeCode = "", ChargeName = "", ChargeHSN = "";

        var txtExpenseName = (TextBox)FormView1.FindControl("txtExpenseName");
        var txtChargecode = (TextBox)FormView1.FindControl("txtChargecode");
        var txtChargeName = (TextBox)FormView1.FindControl("txtChargeName");
        var txtHSNCode = (TextBox)FormView1.FindControl("txtHSNCode");

        ExpenseName = txtExpenseName.Text.Trim();
        ChargeCode = txtChargecode.Text.Trim();
        ChargeName = txtChargeName.Text.Trim();
        ChargeHSN = txtHSNCode.Text.Trim();

        int result = DBOperations.AddExpenseType(ExpenseName, ChargeCode, ChargeName, ChargeHSN, LoggedInUser.glUserId);

        if (result > 0)
        {
            lberror.Text = "Expense  Details Added Successfully.";
            lberror.CssClass = "success";
            ResetControls();
        }
        else if (result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (result == -1)
        {
            lberror.Text = "Expense Name  Already Exists. Please Use Different Name!";
            lberror.CssClass = "errorMsg";

        }
        else if (result == -2)
        {
            lberror.Text = "Charge Code Already Exists. Please Use Different one!";
            lberror.CssClass = "errorMsg";

        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int ExpId = 0;
        string ExpenseName = "", ChargeCode = "", ChargeName = "", ChargeHSN = "";

        ExpId = Convert.ToInt32(Session["lId"]);
        var txtExpenseName = (TextBox)FormView1.FindControl("txtExpenseName");
        var txtChargecode = (TextBox)FormView1.FindControl("txtChargecode");
        var txtChargeName = (TextBox)FormView1.FindControl("txtChargeName");
        var txtHSNCode = (TextBox)FormView1.FindControl("txtHSNCode");

        ExpenseName = txtExpenseName.Text.Trim();
        ChargeCode = txtChargecode.Text.Trim();
        ChargeName = txtChargeName.Text.Trim();
        ChargeHSN = txtHSNCode.Text.Trim();

        int result = DBOperations.UpdExpenseType(ExpId,ExpenseName, ChargeCode, ChargeName, ChargeHSN, LoggedInUser.glUserId);

        if (result == 0)
        {
            lberror.Text = "Expense Details Updated Successfully !";
            lberror.CssClass = "success";

            gvExpense.SelectedIndex = -1;
            gvExpense.DataBind();
            gvExpense.Visible = true;
            DataFilter1.Visible = true;
            fsMainBorder.Visible = true;

            ResetControls();
        }
        else if (result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        //else if (result == 2)
        //{
        //    lberror.Text = "Chaege Code Already Exists.!";
        //    lberror.CssClass = "errorMsg";
        //}

    }
    protected void btnDelete_Click(object sender, EventArgs e)
        {
        int ExpId = 0;

        ExpId = Convert.ToInt32(Session["lId"]);

        int result = DBOperations.DeleteExpenseType(ExpId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lberror.Text = "Expense Details Deleted Successfully !";
            lberror.CssClass = "success";          
        }
        else if (result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }

    }
    private void ResetControls()
    {
        var txtExpenseName = (TextBox)FormView1.FindControl("txtExpenseName");
        var txtChargecode = (TextBox)FormView1.FindControl("txtChargecode");
        var txtChargeName = (TextBox)FormView1.FindControl("txtChargeName");
        var txtHSNCode = (TextBox)FormView1.FindControl("txtHSNCode");

        txtExpenseName.Text = "";
        txtChargecode.Text = "";
        txtChargeName.Text="";
        txtHSNCode.Text = "";
    }


}
#endregion