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
using System.Text;
public partial class AccountExpense_VendorDetailBank : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["VendorId"] == null)
        {
            Response.Redirect("VendorDetail.aspx");
        }
        if (!IsPostBack)
        {
            int VendorId = Convert.ToInt32(Session["VendorId"]);
            string VendorName = "";

            DataSet ds = AccountExpense.GetBJVVendorById(VendorId);

            if(ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows[0]["sName"] != DBNull.Value)
                {
                    VendorName = ds.Tables[0].Rows[0]["sName"].ToString();
                }
            }
            lberror.Text = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bank Details - "+ VendorName;
        }

        //
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = gvUser.Columns;
        DataFilter1.FilterSessionID = "VendorDetailBank.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }

    
    protected string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #region FormView Event

    protected void FormViewDataSource_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
       
    }

    protected void FormViewDataSource_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
    }

    protected void FormView1_ItemCommand(Object sender, FormViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "cancel")
        {
            gvUser.SelectedIndex = -1;
        }
        else
        {

        }

    }

    protected void FormView1_DataBound(object sender, EventArgs e)
    {
        if (FormView1.CurrentMode == FormViewMode.Edit)
        {
            DataRowView drv = (DataRowView)FormView1.DataItem;
            if (drv != null)
            {
                if (drv["AccountType"] != DBNull.Value)
                    ((DropDownList)FormView1.FindControl("ddAccountType")).SelectedValue = drv["AccountType"].ToString();
        
                //if (drv["RoleId"] != DBNull.Value)
                //    ((DropDownList)FormView1.FindControl("ddRole")).SelectedValue = drv["RoleId"].ToString();
                //if (drv["DeptId"] != DBNull.Value)
                //    ((DropDownList)FormView1.FindControl("ddDept")).SelectedValue = drv["DeptId"].ToString();
                //if (drv["ResetCode"] != DBNull.Value)
                //    ((RadioButtonList)FormView1.FindControl("rdPasswordReset")).SelectedValue = drv["ResetCode"].ToString();
                //if (drv["AccessContract"] != DBNull.Value)
                //{
                //    if (Convert.ToBoolean(drv["AccessContract"]) == true)
                //    {
                //        ((CheckBox)FormView1.FindControl("chkViewContract")).Checked = true;
                //    }
                //    else
                //    {
                //        ((CheckBox)FormView1.FindControl("chkViewContract")).Checked = false;
                //    }
                //}
            }
        }

        if (FormView1.CurrentMode == FormViewMode.ReadOnly)
        {
       
        }

        // Always Show Required Field Validator Message 

        Page.Validate("Required");
    }

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {

        if (e.Exception != null | e.AffectedRows == -1)
        {
            e.ExceptionHandled = false;
            e.KeepInInsertMode = true;

            lberror.Text = e.Exception.Message;
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

    protected void FormviewSqlDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        lberror.Visible = true;
        if (Result > 0)
        {
            lberror.Text = "Account Detail Added Successfully.";
            lberror.CssClass = "success";
            FormViewDataSource.SelectParameters[0].DefaultValue = Result.ToString();
            gvUser.SelectedIndex = -1;
            gvUser.DataBind();
        }
        else if (Result == 0)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == -1)
        {
            lberror.Text = "Acount Already Exists For Vendor.";
            lberror.CssClass = "errorMsg";

        }

    }

    protected void FormviewSqlDataSource_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        lberror.Visible = true;

        int Result = Convert.ToInt32(e.Command.Parameters["@OutPut"].Value);

        if (Result == 0)
        {
            lberror.Text = "Account Detail Updated Successfully !";
            lberror.CssClass = "success";

            gvUser.SelectedIndex = -1;
            gvUser.DataBind();
            gvUser.Visible = true;
            DataFilter1.Visible = true;
        }
        else if (Result == 1)
        {
            lberror.Text = "System Error! Please try after sometime";
            lberror.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lberror.Text = "Account Already Exists.!";
            lberror.CssClass = "errorMsg";
        }
    }

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

    protected void gvUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvUser.SelectedIndex == -1)
        {
            FormView1.ChangeMode(FormView1.DefaultMode);
        }
        else
        {
            FormView1.ChangeMode(FormViewMode.ReadOnly);
        }

        FormView1.DataBind();
    }

    protected void gvUser_PreRender(object sender, EventArgs e)
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
            DataFilter1.FilterSessionID = "VendorDetailBank.aspx";
            DataFilter1.FilterDataSource();
            gvUser.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData


    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "VendorAccountDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        gvUser.AllowPaging = false;
        gvUser.AllowSorting = false;
        gvUser.Columns[1].Visible = false;
        gvUser.Columns[2].Visible = true;

        DataFilter1.FilterSessionID = "VendorDetailBank.aspx";
        DataFilter1.FilterDataSource();
        gvUser.DataBind();

        gvUser.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("VendorDetail.aspx");
    }
}