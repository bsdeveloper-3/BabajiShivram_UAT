using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class Freight_FreightRateAir : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Freight Air Rate";

        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            txtStatusDate.Text = DateTime.Now.ToString("MMM/yy");
        }

        DataFilter1.DataSource = SqlDataSourceAIR;
        DataFilter1.DataColumns = GridViewAIRRate.Columns;
        DataFilter1.FilterSessionID = "FreightRateAir.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void btnSubmitAIR_Click(object sender, EventArgs e)
    {
        string strCountry = "", strPOL = "", strPOD = "", strAirline = "", strCurrency = "", strAgent = "", strRemark = "";
        string strMINCharge = "", str45kg = "", str100kg = "", str300kg = "", str500kg = "", str1000kg = "";
        string strFSCCharge = "", strSSCCharge = "", strOtherCharge = "";

        DateTime dtRateValidity = DateTime.MinValue;

        if (txtValidityDateAIR.Text.Trim() != "")
        {
            dtRateValidity = Commonfunctions.CDateTime(txtValidityDateAIR.Text.Trim());
        }

        strCountry      = txtCountryAir.Text.Trim();
        strPOL          = txtPOLAir.Text.Trim();
        strPOD          = ddPODAIR.SelectedItem.Text;
        strAirline      = txtAirline.Text.Trim();
        strCurrency     = txtCurrencyAIR.Text.Trim();
        strAgent        = txtAgentAIR.Text.Trim();
        strRemark       = txtRemarkAIR.Text.Trim();

        strMINCharge    = txtMin.Text.Trim();
        str45kg         = txt45KG.Text.Trim();
        str100kg        = txt100KG.Text.Trim();
        str300kg        = txt300KG.Text.Trim();
        str500kg        = txt500KG.Text.Trim();
        str1000kg       = txt1000KG.Text.Trim();
        strFSCCharge    = txtFSC.Text.Trim();
        strSSCCharge    = txtSSC.Text.Trim();
        strOtherCharge  = txtOther.Text.Trim();

        int result = DBOperations.AddFreightAirRate(strCountry, strPOL, strPOD, strMINCharge, str45kg, str100kg, str300kg,
                str500kg, str1000kg, strFSCCharge, strSSCCharge, strOtherCharge,strAirline, strCurrency, strAgent, dtRateValidity, strRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Rate details added successfully!";
            lblError.CssClass = "success";
            GridViewAIRRate.DataBind();
            ClearFields();
        }
        else if (result == 1)
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
        txtCountryAir.Text      = "";
        txtPOLAir.Text          = "";
        ddPODAIR.SelectedIndex  = 0;
        txtAirline.Text         = "";
        
        txtCurrencyAIR.Text     = "";
        txtAgentAIR.Text        = "";
        txtRemarkAIR.Text       = "";
        txtValidityDateAIR.Text = "";

        txtMin.Text     =   "";
        txt45KG.Text    =   "";
        txt100KG.Text   =   "";
        txt300KG.Text   =   "";
        txt500KG.Text   =   "";
        txt1000KG.Text  =   "";
        txtFSC.Text     =   "";
        txtSSC.Text     =   "";
        txtOther.Text   =   "";
    }

    protected void GridViewAIRRate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "remove")
        {
            int FreightRateID = Convert.ToInt32(e.CommandArgument);

            int result = DBOperations.DeleteFreightAirRate(FreightRateID, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Rate details remove successfully!";
                lblError.CssClass = "success";
                GridViewAIRRate.DataBind();
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
            DataFilter1.FilterSessionID = "FreightRateAir.aspx";
            DataFilter1.FilterDataSource();
            GridViewAIRRate.DataBind();
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
        string strFileName = "Freight_Air_Rate_" + txtStatusDate.Text.Trim() + ".xls";

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
        GridViewAIRRate.AllowPaging = false;
        GridViewAIRRate.AllowSorting = false;
        GridViewAIRRate.Columns[18].Visible = false; 

        GridViewAIRRate.Caption = "Freight Air Rate - " + txtStatusDate.Text.Trim();

        DataFilter1.FilterSessionID = "FreightRateAir.aspx";
        DataFilter1.FilterDataSource();
        GridViewAIRRate.DataBind();

        GridViewAIRRate.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}