using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class FA_FAEntry : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {     
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Invoice - Booking";
            MEditValChequeDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {

    }
    protected void rdlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlCompany.SelectedValue == "1")
            lblGSTN.Text = "27AAACB0466A1ZB";
        else if (rdlCompany.SelectedValue == "2")
            lblGSTN.Text = "27AAACN1163G1ZR";
        else if (rdlCompany.SelectedValue == "2")
            lblGSTN.Text = "27AAACN1975C1ZN";
    }

    #region Wizard Event
    protected void wzDelivery_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {

    }
    protected void wzDelivery_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
     
    }

    #endregion   

    protected void btnAddCharges_Click(object sender, EventArgs e)
    {
        DataTable dtCharges = GetChargeCodeDataTable();

        if (ViewState["vwsCharges"] != null)
        {
            dtCharges = (DataTable)ViewState["vwsCharges"];
        }

        DataRow dr = dtCharges.NewRow();

        dr["VendorName"]    = txtVendorName.Text.Trim();
        dr["VendorCode"]    = txtVendorName.Text.Trim();
        dr["ChargeCode"]    = txtchargeCode.Text.Trim();
        dr["HSN"]           = txtHSN.Text.Trim();
        dr["SubTotal"]      = txtChargeAmount.Text.Trim();
        dr["CGST"]          = txtCGST.Text.Trim();
        dr["SGST"]          = txtSGST.Text.Trim();
        dr["IGST"]          = txtIGST.Text.Trim();
        dr["ChargeTotal"]   = lblChargeTotal.Text.Trim();
        dr["ChargeRemark"]  = txtChargeRemark.Text.Trim();

        dtCharges.Rows.Add(dr);
        dtCharges.AcceptChanges();

        ViewState["vwsCharges"] = dtCharges;

        gvCharges.DataSource = dtCharges;
        gvCharges.DataBind();
    }

    private DataTable GetChargeCodeDataTable()
    {
        DataTable dtChargeCode = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colVendorName = new DataColumn("VendorName", Type.GetType("System.String"));
        DataColumn colVendorCode = new DataColumn("VendorCode", Type.GetType("System.String"));
        
        DataColumn colChargeCode = new DataColumn("ChargeCode", Type.GetType("System.String"));
        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colSubTotal = new DataColumn("SubTotal", Type.GetType("System.String"));
        DataColumn colCGST = new DataColumn("CGST", Type.GetType("System.String"));
        DataColumn colSGST = new DataColumn("SGST", Type.GetType("System.String"));
        DataColumn colIGST = new DataColumn("IGST", Type.GetType("System.String"));

        DataColumn colChargeTotal = new DataColumn("ChargeTotal", Type.GetType("System.String"));
        DataColumn colChargeRemark = new DataColumn("ChargeRemark", Type.GetType("System.String"));

        dtChargeCode.Columns.Add(colVendorName);
        dtChargeCode.Columns.Add(colVendorCode);
        dtChargeCode.Columns.Add(colChargeCode);
        dtChargeCode.Columns.Add(colHSN);
        dtChargeCode.Columns.Add(colSubTotal);

        dtChargeCode.Columns.Add(colCGST);
        dtChargeCode.Columns.Add(colSGST);
        dtChargeCode.Columns.Add(colIGST);

        dtChargeCode.Columns.Add(colChargeTotal);
        dtChargeCode.Columns.Add(colChargeRemark);

        dtChargeCode.AcceptChanges();

        return dtChargeCode;
    }
}