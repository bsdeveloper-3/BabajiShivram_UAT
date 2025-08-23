using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;


public partial class CRM_UserLocation : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(gvLocationHeads);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Location Wise Head";
        }

        DataFilter1.DataSource = DataSourceHeadLocation;
        DataFilter1.DataColumns = gvLocationHeads.Columns;
        DataFilter1.FilterSessionID = "UserLocation.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLocationHeads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "deleterow")
        {
            if (e.CommandArgument.ToString() != "")
            {
                int result = DBOperations.CRM_DeleteUserLocation(Convert.ToInt32(e.CommandArgument.ToString()), LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblError.Text = "Successfully deleted record.";
                    lblError.CssClass = "success";
                    ResetControls();
                }
                else if (result == -2)
                {
                    lblError.Text = "Service location not added for this user!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "Error saving. Please try again later!";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int ServiceId = 0, LocationId = 0, UserId = 0;

        if (ddlService.SelectedValue != "0")
            ServiceId = Convert.ToInt32(ddlService.SelectedValue);
        if (ddlLocation.SelectedValue != "0")
            LocationId = Convert.ToInt32(ddlLocation.SelectedValue);
        if (ddlSalesPerson.SelectedValue != "0")
            UserId = Convert.ToInt32(ddlSalesPerson.SelectedValue);

        if (ServiceId > 0 && LocationId > 0)
        {
            int result = DBOperations.CRM_AddUserLocation(ServiceId, LocationId, UserId, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Successfully added head for service location.";
                lblError.CssClass = "success";
                ResetControls();
            }
            else if (result == -2)
            {
                lblError.Text = "Service location already been added for this user!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "Error saving. Please try again later!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "Please select service and service location";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void ResetControls()
    {
        ddlLocation.Items.Clear();
        ddlLocation.DataBind();
        ddlSalesPerson.Items.Clear();
        ddlSalesPerson.DataBind();
        ddlService.Items.Clear();
        ddlService.DataBind();
        gvLocationHeads.DataBind();
    }

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "UserLocationOn" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        gvLocationHeads.AllowPaging = false;
        gvLocationHeads.AllowSorting = false;
        gvLocationHeads.Columns[0].Visible = false;
        gvLocationHeads.Columns[4].Visible = false;
        gvLocationHeads.Enabled = false;
        gvLocationHeads.Caption = "Location Report On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        //DataFilter1.FilterSessionID = "Leads.aspx";
        //DataFilter1.FilterDataSource();
        gvLocationHeads.DataBind();
        gvLocationHeads.RenderControl(hw);
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
            DataFilter1.FilterSessionID = "UserLocation.aspx";
            DataFilter1.FilterDataSource();
            gvLocationHeads.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

}