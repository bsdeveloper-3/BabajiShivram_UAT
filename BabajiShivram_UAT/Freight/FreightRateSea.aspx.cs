using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Freight_FreightRateSea : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Freight Sea Rate";

        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            txtStatusDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        DataFilter1.DataSource = SqlDataSourceSEA;
        DataFilter1.DataColumns = GridViewSEARate.Columns;
        DataFilter1.FilterSessionID = "FreightRateSea.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnSubmitSEA_Click(object sender, EventArgs e)
    {
        string strCountry = "", strPOL = "", strPOD = "", strShippingline = "",  strTransitDays = "",
        str20GP = "", str40GPHQ = "",  strCurrency = "", strAgent = "", strRemark = "";

        DateTime dtRateValidity = DateTime.MinValue;

        if (txtValidityDateSEA.Text.Trim() != "")
        {
            dtRateValidity = Commonfunctions.CDateTime(txtValidityDateSEA.Text.Trim());
        }

        strCountry  =   txtCountrySea.Text.Trim();
        strPOL      =   txtPOLSEA.Text.Trim();
        strPOD      =   ddPODSEA.SelectedItem.Text;
        strShippingline =   txtShippingLineSEA.Text.Trim();
        strTransitDays = txtTransitDaysSEA.Text.Trim();
        str20GP     =   txt20GPSEA.Text.Trim();
        str40GPHQ   =   txt40GPSEA.Text.Trim();
        strCurrency =   txtCurrencySEA.Text.Trim();
        strAgent    =   txtAgentSEA.Text.Trim();
        strRemark   =   txtRemarkSEA.Text.Trim();

        int result = DBOperations.AddFreightSeaRate(strCountry, strPOL, strPOD, strShippingline, strTransitDays, 
            str20GP, str40GPHQ, strCurrency, strAgent, strRemark, dtRateValidity, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Rate details added successfully!";
            lblError.CssClass = "success";
            GridViewSEARate.DataBind();
            ClearFields();
        }
        else if(result == 1)
        {
            lblError.Text = "Rate details already added!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }
    }

    private void ClearFields()
    {
        txtCountrySea.Text      = "";
        txtPOLSEA.Text          = "";
        ddPODSEA.SelectedIndex  = 0;
        txtShippingLineSEA.Text = "";
        txtTransitDaysSEA.Text = "";
        txt20GPSEA.Text         = "";
        txt40GPSEA.Text         = "";
        txtCurrencySEA.Text     = "";
        txtAgentSEA.Text        = "";
        txtRemarkSEA.Text       = "";
        txtValidityDateSEA.Text = "";
    }

    protected void GridViewSEARate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "remove")
        {
            int FreightRateID = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightSeaRate(FreightRateID, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Rate details remove successfully!";
                lblError.CssClass = "success";
                GridViewSEARate.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            if (result == 2)
            {
                lblError.Text = "Rate details not found!";
                lblError.CssClass = "errorMsg";
            }
        }
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
            DataFilter1.FilterSessionID = "FreightRateSea.aspx";
            DataFilter1.FilterDataSource();
            GridViewSEARate.DataBind();
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
        string strFileName = "Freight_Sea_Rate_" + txtStatusDate.Text.Trim() + ".xls";

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
        GridViewSEARate.AllowPaging = false;
        GridViewSEARate.AllowSorting = false;
        GridViewSEARate.Columns[12].Visible = false; // Remove Delete Link Button

        GridViewSEARate.Caption = "Freight Sea Rate - " + txtStatusDate.Text.Trim();

        DataFilter1.FilterSessionID = "FreightRateSea.aspx";
        DataFilter1.FilterDataSource();
        GridViewSEARate.DataBind();
        
        GridViewSEARate.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}