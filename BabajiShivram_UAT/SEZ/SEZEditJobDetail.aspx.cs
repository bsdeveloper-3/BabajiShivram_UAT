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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Data.OleDb;
//using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;
public partial class SEZ_SEZEditJobDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(grdDocument);  // JobRefNo
        ScriptManager1.RegisterPostBackControl(gvContainer);
        ScriptManager1.RegisterPostBackControl(FVJobDetail);
        ScriptManager1.RegisterPostBackControl(btnUpload);    // gvJobDetail
        ScriptManager1.RegisterPostBackControl(gvJobDetail);
        //ScriptManager1.RegisterPostBackControl(fuDocument); //fuDocument

        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("SEZInfo.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "SEZ Job Detail";

            JobDetailMS(Convert.ToInt32(Session["JobId"]));
            DocumentTypeBind();
            ddDocument.Items.Insert(0, new ListItem("--Select Type--", "0"));

            SetInitialRow();          // For Invoice Detail
        }
    }


    private void JobDetailMS(int JobId)
    {
        // Job Detail
        DataSet dsJobDetail = SEZOperation.GetSEZJobDetail(JobId);

        string aa = FVJobDetail.CurrentMode.ToString();

        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            string InwardJobNo = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            Session["JobRefNo"] = InwardJobNo;

            int SEZType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["SEZTypeID"]);
            Session["SEZType"] = SEZType;

            string strInwardJobNo = dsJobDetail.Tables[0].Rows[0]["InwardJobNo"].ToString();
            Session["InwardJobNo"] = strInwardJobNo;
            
            int RequestType = Convert.ToInt32(dsJobDetail.Tables[0].Rows[0]["RequestType"]);
            Session["RequestType"] = RequestType;

            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();
        }
    }

    protected void btnEditJobDetail_Click(object sender, EventArgs e)
    {
        if (Session["JobId"] != null)
        {
            int jjj = Convert.ToInt32(Session["JobId"]);

            DateTime FileBilling = DateTime.MinValue;
            DataSet dsFileSentBill = SEZOperation.GetFileSentBilling(Convert.ToInt32(Session["JobId"]));

            if (dsFileSentBill.Tables[0].Rows.Count > 0)
            {
                string aa = Convert.ToString(dsFileSentBill.Tables[0].Rows[0]["FileSentToBilling"]);
                if (aa != "")
                {
                    FileBilling = Convert.ToDateTime(aa);
                }
            }

            if (FileBilling == DateTime.MinValue || FileBilling == null)
            {
                FVJobDetail.ChangeMode(FormViewMode.Edit);
                JobDetailMS(Convert.ToInt32(Session["JobId"]));
            }
            else
            {
                lblError.Text = "Already File Sent To Billing.. Hence Job Details Can not Modified";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancelJobDetail_Click(object sender, EventArgs e)
    {

        //if (Session["JobId"] != null)
        //{
        //    //JobDetailMS(Convert.ToInt32(Session["JobId"]));
        //   // FVJobDetail.ChangeMode(FormViewMode.ReadOnly);
        //}
        Session["JobId"] = null;
        Response.Redirect("SEZInfo.aspx");
    }

    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {
        if (FVJobDetail.CurrentMode == FormViewMode.Edit)
        {
            if (Convert.ToInt32(Session["RequestType"]) == 3 || Convert.ToInt32(Session["RequestType"]) == 1) // DTA Sales  && BOE
            {
                Label lblDutyAmount = (Label)FVJobDetail.FindControl("lblDutyAmount");
                TextBox txtDutyAmount = (TextBox)FVJobDetail.FindControl("txtDutyAmount");
                lblDutyAmount.Visible = false;
                txtDutyAmount.Visible = false;

                if (Convert.ToInt32(Session["RequestType"]) == 3) // DTA Sales
                {
                    Label lblSupplyName = (Label)FVJobDetail.FindControl("lblSupplyName");
                    TextBox txtSupplierName = (TextBox)FVJobDetail.FindControl("txtSupplierName");
                    lblSupplyName.Visible = false;
                    txtSupplierName.Visible = false;
                }
                else if (Convert.ToInt32(Session["RequestType"]) == 1) // BOE
                {
                    Label lblSupplyName = (Label)FVJobDetail.FindControl("lblSupplyName");
                    TextBox txtSupplierName = (TextBox)FVJobDetail.FindControl("txtSupplierName");
                    lblSupplyName.Visible = true;
                    txtSupplierName.Visible = true;
                }

                Label lblBuyerName = (Label)FVJobDetail.FindControl("lblBuyerName");
                TextBox txtBuyerName = (TextBox)FVJobDetail.FindControl("txtBuyerName");
                Label lblSchemeCode = (Label)FVJobDetail.FindControl("lblSchemeCode");
                TextBox txtSchemeCode = (TextBox)FVJobDetail.FindControl("txtSchemeCode");
                lblBuyerName.Visible = false;
                txtBuyerName.Visible = false;
                lblSchemeCode.Visible = false;
                txtSchemeCode.Visible = false;

                Label lblPrevExpGoods = (Label)FVJobDetail.FindControl("lblPrevExpGoods");
                Label lblCessDetail = (Label)FVJobDetail.FindControl("lblCessDetail");
                Label lblLicenceRegNo = (Label)FVJobDetail.FindControl("lblLicenceRegNo");
                Label lblReExport = (Label)FVJobDetail.FindControl("lblReExport");
                Label lblPrevExport = (Label)FVJobDetail.FindControl("lblPrevExport");

                RadioButtonList rdlPrevExpGoods1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExpGoods");
                RadioButtonList rdlCessDetail1 = (RadioButtonList)FVJobDetail.FindControl("rdlCessDetail");
                RadioButtonList rdlLicenceRegNo1 = (RadioButtonList)FVJobDetail.FindControl("rdlLicenceRegNo");
                RadioButtonList rdlReExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReExport");
                RadioButtonList rdlPrevExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExport");

                lblPrevExpGoods.Visible = false; lblCessDetail.Visible = false;
                lblLicenceRegNo.Visible = false; lblReExport.Visible = false;
                lblPrevExport.Visible = false;

                rdlPrevExpGoods1.Visible = false;
                rdlCessDetail1.Visible = false; rdlLicenceRegNo1.Visible = false;
                rdlReExport1.Visible = false; rdlPrevExport1.Visible = false;
            }
            else if (Convert.ToInt32(Session["RequestType"]) == 5)  // DTAP
            {

                Label lblSupplyName = (Label)FVJobDetail.FindControl("lblSupplyName");
                TextBox txtSupplierName = (TextBox)FVJobDetail.FindControl("txtSupplierName");
                lblSupplyName.Visible = false;
                txtSupplierName.Visible = false;

                Label lblCIFValue = (Label)FVJobDetail.FindControl("lblCIFValue");
                Label lblGrossUnit = (Label)FVJobDetail.FindControl("lblGrossUnit");
                Label lblDiscoutAppli = (Label)FVJobDetail.FindControl("lblDiscoutAppli");
                Label lblReImport1 = (Label)FVJobDetail.FindControl("lblReImport1");
                Label lblPreviousImport = (Label)FVJobDetail.FindControl("lblPreviousImport");

                TextBox txtCIFValue = (TextBox)FVJobDetail.FindControl("txtCIFValue");
                DropDownList ddlGrossUnit1 = (DropDownList)FVJobDetail.FindControl("ddlGrossUnit");
                RadioButtonList rdlDiscountAppli = (RadioButtonList)FVJobDetail.FindControl("rdlDiscountAppli");
                RadioButtonList rdlReImport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReImport");
                RadioButtonList rdlPreviousImport1 = (RadioButtonList)FVJobDetail.FindControl("rdlPreviousImport");

                lblCIFValue.Visible = false; txtCIFValue.Visible = false;
                lblGrossUnit.Visible = false; ddlGrossUnit1.Visible = false;

                lblDiscoutAppli.Visible = false; rdlDiscountAppli.Visible = false;
                lblReImport1.Visible = false; rdlReImport1.Visible = false;
                lblPreviousImport.Visible = false; rdlPreviousImport1.Visible = false;  //

                Label lblBuyerName = (Label)FVJobDetail.FindControl("lblBuyerName");
                TextBox txtBuyerName = (TextBox)FVJobDetail.FindControl("txtBuyerName");
                Label lblSchemeCode = (Label)FVJobDetail.FindControl("lblSchemeCode");
                TextBox txtSchemeCode = (TextBox)FVJobDetail.FindControl("txtSchemeCode");
                lblBuyerName.Visible = false;
                txtBuyerName.Visible = false;
                lblSchemeCode.Visible = false;
                txtSchemeCode.Visible = false;

                Label lblPrevExpGoods = (Label)FVJobDetail.FindControl("lblPrevExpGoods");
                Label lblCessDetail = (Label)FVJobDetail.FindControl("lblCessDetail");
                Label lblLicenceRegNo = (Label)FVJobDetail.FindControl("lblLicenceRegNo");
                Label lblReExport = (Label)FVJobDetail.FindControl("lblReExport");
                Label lblPrevExport = (Label)FVJobDetail.FindControl("lblPrevExport");

                RadioButtonList rdlPrevExpGoods1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExpGoods");
                RadioButtonList rdlCessDetail1 = (RadioButtonList)FVJobDetail.FindControl("rdlCessDetail");
                RadioButtonList rdlLicenceRegNo1 = (RadioButtonList)FVJobDetail.FindControl("rdlLicenceRegNo");
                RadioButtonList rdlReExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReExport");
                RadioButtonList rdlPrevExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExport");

                lblPrevExpGoods.Visible = false; lblCessDetail.Visible = false;
                lblLicenceRegNo.Visible = false; lblReExport.Visible = false;
                lblPrevExport.Visible = false;

                rdlPrevExpGoods1.Visible = false;
                rdlCessDetail1.Visible = false; rdlLicenceRegNo1.Visible = false;
                rdlReExport1.Visible = false; rdlPrevExport1.Visible = false;
            }

            else if (Convert.ToInt32(Session["RequestType"]) == 2)  // SB - Shipping Bill
            {
                Label lblDutyAmount = (Label)FVJobDetail.FindControl("lblDutyAmount");
                TextBox txtDutyAmount = (TextBox)FVJobDetail.FindControl("txtDutyAmount");
                lblDutyAmount.Visible = false;
                txtDutyAmount.Visible = false;

                Label lblSupplyName = (Label)FVJobDetail.FindControl("lblSupplyName");
                TextBox txtSupplierName = (TextBox)FVJobDetail.FindControl("txtSupplierName");
                lblSupplyName.Visible = false;
                txtSupplierName.Visible = false;

                Label lblCIFValue = (Label)FVJobDetail.FindControl("lblCIFValue");
                Label lblGrossUnit = (Label)FVJobDetail.FindControl("lblGrossUnit");
                Label lblDiscoutAppli = (Label)FVJobDetail.FindControl("lblDiscoutAppli");
                Label lblReImport1 = (Label)FVJobDetail.FindControl("lblReImport1");
                Label lblPreviousImport = (Label)FVJobDetail.FindControl("lblPreviousImport");

                TextBox txtCIFValue = (TextBox)FVJobDetail.FindControl("txtCIFValue");
                DropDownList ddlGrossUnit1 = (DropDownList)FVJobDetail.FindControl("ddlGrossUnit");
                RadioButtonList rdlDiscountAppli = (RadioButtonList)FVJobDetail.FindControl("rdlDiscountAppli");
                RadioButtonList rdlReImport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReImport");
                RadioButtonList rdlPreviousImport1 = (RadioButtonList)FVJobDetail.FindControl("rdlPreviousImport");

                lblCIFValue.Visible = false; txtCIFValue.Visible = false;
                lblGrossUnit.Visible = false; ddlGrossUnit1.Visible = false;

                lblDiscoutAppli.Visible = false; rdlDiscountAppli.Visible = false;
                lblReImport1.Visible = false; rdlReImport1.Visible = false;
                lblPreviousImport.Visible = false; rdlPreviousImport1.Visible = false;
            }


            if (Convert.ToInt32(Session["SEZType"]) == 1)
            {
                Label ddMode0 = (Label)FVJobDetail.FindControl("lblInwardBeno");
                Label ddMode1 = (Label)FVJobDetail.FindControl("lblInwardBeDate");
                Label ddMode2 = (Label)FVJobDetail.FindControl("lblInwardJobNo");
                TextBox ddMode4 = (TextBox)FVJobDetail.FindControl("txtInwardBENo");
                TextBox ddMode5 = (TextBox)FVJobDetail.FindControl("txtInwardBEDate");
                TextBox ddMode6 = (TextBox)FVJobDetail.FindControl("txtInwardJobNo");

                ddMode0.Visible = false;
                ddMode1.Visible = false;
                ddMode2.Visible = false;
                ddMode4.Visible = false;
                ddMode5.Visible = false;
                ddMode6.Visible = false;
            }
            else
            {
                Label ddMode0 = (Label)FVJobDetail.FindControl("lblInwardBeno");
                Label ddMode1 = (Label)FVJobDetail.FindControl("lblInwardBeDate");
                Label ddMode2 = (Label)FVJobDetail.FindControl("lblInwardJobNo");
                TextBox ddMode4 = (TextBox)FVJobDetail.FindControl("txtInwardBENo");
                TextBox ddMode5 = (TextBox)FVJobDetail.FindControl("txtInwardBEDate");
                TextBox ddMode6 = (TextBox)FVJobDetail.FindControl("txtInwardJobNo");
                TextBox ddMode8 = (TextBox)FVJobDetail.FindControl("txtOutwardDate");

                ddMode0.Visible = true;
                ddMode1.Visible = true;
                ddMode2.Visible = true;
                ddMode4.Visible = true;
                ddMode5.Visible = true;
                ddMode6.Visible = true;

            }

            DropDownList ddMode = (DropDownList)FVJobDetail.FindControl("ddMode");
            HiddenField hdnMode = (HiddenField)FVJobDetail.FindControl("hdnMode");
            if (ddMode != null)
            {
                ddMode.SelectedValue = hdnMode.Value;
            }

            RadioButtonList rdlDiscount = (RadioButtonList)FVJobDetail.FindControl("rdlDiscountAppli");
            HiddenField hdndiscount = (HiddenField)FVJobDetail.FindControl("hdndiscount");
            if (hdndiscount != null)
            {
                if (hdndiscount.Value == "False")
                {
                    rdlDiscount.SelectedValue = "2";
                }
                else if (hdndiscount.Value == "True")
                {
                    rdlDiscount.SelectedValue = "1";
                }
            }

            RadioButtonList rdlReImport = (RadioButtonList)FVJobDetail.FindControl("rdlReImport");
            HiddenField hdnReImport = (HiddenField)FVJobDetail.FindControl("hdnReImport");
            if (hdnReImport != null)
            {
                if (hdnReImport.Value == "False")
                {
                    rdlReImport.SelectedValue = "2";
                }
                else if (hdnReImport.Value == "True")
                {
                    rdlReImport.SelectedValue = "1";
                }
            }

            RadioButtonList rdlPreviousImport = (RadioButtonList)FVJobDetail.FindControl("rdlPreviousImport");
            HiddenField hdnPrevImport = (HiddenField)FVJobDetail.FindControl("hdnPrevImport");
            if (hdnPrevImport != null)
            {
                if (hdnPrevImport.Value == "False")
                {
                    rdlPreviousImport.SelectedValue = "2";
                }
                else if (hdnPrevImport.Value == "True")
                {
                    rdlPreviousImport.SelectedValue = "1";
                }
            }

            //-------------

            RadioButtonList rdlPrevExpGoods = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExpGoods");
            HiddenField hdnPrevExpGoods = (HiddenField)FVJobDetail.FindControl("hdnPrevExpGoods");
            if (hdnPrevExpGoods != null)
            {
                if (hdnPrevExpGoods.Value == "False")
                {
                    rdlPrevExpGoods.SelectedValue = "2";
                }
                else if (hdnPrevExpGoods.Value == "True")
                {
                    rdlPrevExpGoods.SelectedValue = "1";
                }
            }
            RadioButtonList rdlCessDetail = (RadioButtonList)FVJobDetail.FindControl("rdlCessDetail");
            HiddenField hdnCessDetail = (HiddenField)FVJobDetail.FindControl("hdnCessDetail");
            if (hdnCessDetail != null)
            {
                if (hdnCessDetail.Value == "False")
                {
                    rdlCessDetail.SelectedValue = "2";
                }
                else if (hdnCessDetail.Value == "True")
                {
                    rdlCessDetail.SelectedValue = "1";
                }
            }
            RadioButtonList rdlLicenceRegNo = (RadioButtonList)FVJobDetail.FindControl("rdlLicenceRegNo");
            HiddenField hdnLicenceRegNo = (HiddenField)FVJobDetail.FindControl("hdnLicenceRegNo");
            if (hdnLicenceRegNo != null)
            {
                if (hdnLicenceRegNo.Value == "False")
                {
                    rdlLicenceRegNo.SelectedValue = "2";
                }
                else if (hdnLicenceRegNo.Value == "True")
                {
                    rdlLicenceRegNo.SelectedValue = "1";
                }
            }
            RadioButtonList rdlReExport = (RadioButtonList)FVJobDetail.FindControl("rdlReExport");
            HiddenField hdnReExport = (HiddenField)FVJobDetail.FindControl("hdnReExport");
            if (hdnReExport != null)
            {
                if (hdnReExport.Value == "False")
                {
                    rdlReExport.SelectedValue = "2";
                }
                else if (hdnReExport.Value == "True")
                {
                    rdlReExport.SelectedValue = "1";
                }
            }
            RadioButtonList rdlPrevExport = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExport");
            HiddenField hdnPrevExport = (HiddenField)FVJobDetail.FindControl("hdnPrevExport");
            if (hdnPrevExport != null)
            {
                if (hdnPrevExport.Value == "False")
                {
                    rdlPrevExport.SelectedValue = "2";
                }
                else if (hdnPrevExport.Value == "True")
                {
                    rdlPrevExport.SelectedValue = "1";
                }
            }

            //--------------

            DropDownList ddlCurrency = (DropDownList)FVJobDetail.FindControl("ddlCurrency");
            SEZOperation.FillCurrency(ddlCurrency);
            HiddenField hdnCurrency = (HiddenField)FVJobDetail.FindControl("hdnCurrency");
            if (ddlCurrency != null)
            {
                ddlCurrency.SelectedValue = hdnCurrency.Value;
            }

            DropDownList ddlServicesProvide = (DropDownList)FVJobDetail.FindControl("ddlServicesProvide");
            HiddenField hdnServicesProvide = (HiddenField)FVJobDetail.FindControl("hdnServicesProvide");
            if (hdnServicesProvide != null)
            {
                ddlServicesProvide.SelectedValue = hdnServicesProvide.Value;
            }

            // DataSet dsDocType = DBOperations.FillPackageType(DropDownList ddlPackagesUnit);
            //DropDownList ddlDutyCustom = (DropDownList)FVJobDetail.FindControl("ddlDutyCustom");

            //HiddenField hdnDutyCustom = (HiddenField)FVJobDetail.FindControl("hdnDutyCustom");

            //if (hdnDutyCustom != null)
            //{
            //    ddlDutyCustom.SelectedValue = hdnDutyCustom.Value;
            //}

            //DropDownList ddlPackagesUnit = (DropDownList)FVJobDetail.FindControl("ddlPackagesUnit");
            //DBOperations.FillPackageType(ddlPackagesUnit);
            //HiddenField hdnPackagesUnit = (HiddenField)FVJobDetail.FindControl("hdnPackagesUnit");
            //if (hdnPackagesUnit != null)
            //{
            //    ddlPackagesUnit.SelectedValue = hdnPackagesUnit.Value;
            //}

            DropDownList ddlGrossUnit = (DropDownList)FVJobDetail.FindControl("ddlGrossUnit");
            SEZOperation.FillGrossWtUnit(ddlGrossUnit);
            HiddenField hdnGrossUnit = (HiddenField)FVJobDetail.FindControl("hdnGrossUnit");
            if (ddMode != null)
            {
                ddlGrossUnit.SelectedValue = hdnGrossUnit.Value;
            }

            //DropDownList ddlDestination = (DropDownList)FVJobDetail.FindControl("ddlDestination");
            //SEZOperation.FillDestinationMS(ddlDestination);
            //HiddenField hdnDestination = (HiddenField)FVJobDetail.FindControl("hdnDestination");
            //if (ddMode != null)
            //{
            //    ddlDestination.SelectedValue = hdnDestination.Value;
            //}

            //DropDownList ddlCountry = (DropDownList)FVJobDetail.FindControl("ddlCountry");
            //SEZOperation.FillCountryOriginMS(ddlCountry);
            //HiddenField hdnCountry = (HiddenField)FVJobDetail.FindControl("hdnCountry");
            //if (ddMode != null)
            //{
            //    ddlCountry.SelectedValue = hdnCountry.Value;
            //}

            //DropDownList ddlPlace = (DropDownList)FVJobDetail.FindControl("ddlPlace");
            //SEZOperation.FillPlaceOriginMS(ddlPlace);
            //HiddenField hdnPlace = (HiddenField)FVJobDetail.FindControl("hdnPlace");
            //if (ddMode != null)
            //{
            //    ddlPlace.SelectedValue = hdnPlace.Value;
            //}



        }
        else if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
        {

            if (Convert.ToInt32(Session["RequestType"]) == 3 || Convert.ToInt32(Session["RequestType"]) == 1) // DTA Sales  && BOE
            {
                Label lblDutyAmnt1 = (Label)FVJobDetail.FindControl("lblDutyAmnt1");
                Label lblDutyAmnt = (Label)FVJobDetail.FindControl("lblDutyAmnt");
                lblDutyAmnt1.Visible = false;
                lblDutyAmnt.Visible = false;

                if (Convert.ToInt32(Session["RequestType"]) == 3) // DTA Sales
                {
                    Label lblSupplierName1 = (Label)FVJobDetail.FindControl("lblSupplierName1");
                    Label lblSupplierName = (Label)FVJobDetail.FindControl("lblSupplierName");
                    lblSupplierName1.Visible = false;
                    lblSupplierName.Visible = false;
                }

                else if (Convert.ToInt32(Session["RequestType"]) == 1) // BOE
                {
                    Label lblSupplierName1 = (Label)FVJobDetail.FindControl("lblSupplierName1");
                    Label lblSupplierName = (Label)FVJobDetail.FindControl("lblSupplierName");
                    lblSupplierName1.Visible = true;
                    lblSupplierName.Visible = true;
                }

                Label lblBuyerName1 = (Label)FVJobDetail.FindControl("lblBuyerName1");
                Label lblBuyerName = (Label)FVJobDetail.FindControl("lblBuyerName");
                Label lblSchemeCode1 = (Label)FVJobDetail.FindControl("lblSchemeCode1");
                Label lblSchemeCode = (Label)FVJobDetail.FindControl("lblSchemeCode");
                Label lblPrevExpGoods1 = (Label)FVJobDetail.FindControl("lblPrevExpGoods1");
                Label lblPrevExpGoods = (Label)FVJobDetail.FindControl("lblPrevExpGoods");
                Label lblCessDetail1 = (Label)FVJobDetail.FindControl("lblCessDetail1");
                Label lblCessDetail = (Label)FVJobDetail.FindControl("lblCessDetail");
                Label lblLicenceRegNo1 = (Label)FVJobDetail.FindControl("lblLicenceRegNo1");
                Label lblLicenceRegNo = (Label)FVJobDetail.FindControl("lblLicenceRegNo");
                Label lblReExport1 = (Label)FVJobDetail.FindControl("lblReExport1");
                Label lblReExport = (Label)FVJobDetail.FindControl("lblReExport");
                Label lblPrevExport1 = (Label)FVJobDetail.FindControl("lblPrevExport1");
                Label lblPrevExport = (Label)FVJobDetail.FindControl("lblPrevExport");

                lblBuyerName1.Visible = false; lblBuyerName.Visible = false;
                lblSchemeCode1.Visible = false; lblSchemeCode.Visible = false;
                lblPrevExpGoods1.Visible = false; lblPrevExpGoods.Visible = false;
                lblCessDetail1.Visible = false; lblCessDetail.Visible = false;
                lblLicenceRegNo1.Visible = false; lblLicenceRegNo.Visible = false;
                lblReExport1.Visible = false; lblReExport.Visible = false;
                lblPrevExport1.Visible = false; lblPrevExport.Visible = false;

            }
            else if (Convert.ToInt32(Session["RequestType"]) == 5)  // DTAP
            {
                Label lblCIFVal1 = (Label)FVJobDetail.FindControl("lblCIFVal1");
                Label lblCIFVal = (Label)FVJobDetail.FindControl("lblCIFVal");
                Label lblGrossUnit1 = (Label)FVJobDetail.FindControl("lblGrossUnit1");
                Label lblGrossUnit = (Label)FVJobDetail.FindControl("lblGrossUnit");

                Label lblDiscount1 = (Label)FVJobDetail.FindControl("lblDiscount1");
                Label lblDiscount = (Label)FVJobDetail.FindControl("lblDiscount");
                Label lblReImport1 = (Label)FVJobDetail.FindControl("lblReImport1");
                Label lblReImport = (Label)FVJobDetail.FindControl("lblReImport");
                Label lblPrevImport1 = (Label)FVJobDetail.FindControl("lblPrevImport1");
                Label lblPrevImport = (Label)FVJobDetail.FindControl("lblPrevImport");

                lblCIFVal1.Visible = false; lblCIFVal.Visible = false;
                lblGrossUnit1.Visible = false; lblGrossUnit.Visible = false;

                lblDiscount1.Visible = false; lblDiscount.Visible = false;
                lblReImport1.Visible = false; lblReImport.Visible = false;
                lblPrevImport1.Visible = false; lblPrevImport.Visible = false;

                Label lblBuyerName1 = (Label)FVJobDetail.FindControl("lblBuyerName1");
                Label lblBuyerName = (Label)FVJobDetail.FindControl("lblBuyerName");
                Label lblSchemeCode1 = (Label)FVJobDetail.FindControl("lblSchemeCode1");
                Label lblSchemeCode = (Label)FVJobDetail.FindControl("lblSchemeCode");
                Label lblPrevExpGoods1 = (Label)FVJobDetail.FindControl("lblPrevExpGoods1");
                Label lblPrevExpGoods = (Label)FVJobDetail.FindControl("lblPrevExpGoods");
                Label lblCessDetail1 = (Label)FVJobDetail.FindControl("lblCessDetail1");
                Label lblCessDetail = (Label)FVJobDetail.FindControl("lblCessDetail");
                Label lblLicenceRegNo1 = (Label)FVJobDetail.FindControl("lblLicenceRegNo1");
                Label lblLicenceRegNo = (Label)FVJobDetail.FindControl("lblLicenceRegNo");
                Label lblReExport1 = (Label)FVJobDetail.FindControl("lblReExport1");
                Label lblReExport = (Label)FVJobDetail.FindControl("lblReExport");
                Label lblPrevExport1 = (Label)FVJobDetail.FindControl("lblPrevExport1");
                Label lblPrevExport = (Label)FVJobDetail.FindControl("lblPrevExport");

                lblBuyerName1.Visible = false; lblBuyerName.Visible = false;
                lblSchemeCode1.Visible = false; lblSchemeCode.Visible = false;
                lblPrevExpGoods1.Visible = false; lblPrevExpGoods.Visible = false;
                lblCessDetail1.Visible = false; lblCessDetail.Visible = false;
                lblLicenceRegNo1.Visible = false; lblLicenceRegNo.Visible = false;
                lblReExport1.Visible = false; lblReExport.Visible = false;
                lblPrevExport1.Visible = false; lblPrevExport.Visible = false;

            }

            else if (Convert.ToInt32(Session["RequestType"]) == 2) // SB - Shipping Bill
            {
                Label lblDutyAmnt1 = (Label)FVJobDetail.FindControl("lblDutyAmnt1");
                Label lblDutyAmnt = (Label)FVJobDetail.FindControl("lblDutyAmnt");
                lblDutyAmnt1.Visible = false;
                lblDutyAmnt.Visible = false;

                Label lblCIFVal1 = (Label)FVJobDetail.FindControl("lblCIFVal1");
                Label lblCIFVal = (Label)FVJobDetail.FindControl("lblCIFVal");
                Label lblGrossUnit1 = (Label)FVJobDetail.FindControl("lblGrossUnit1");
                Label lblGrossUnit = (Label)FVJobDetail.FindControl("lblGrossUnit");

                Label lblDiscount1 = (Label)FVJobDetail.FindControl("lblDiscount1");
                Label lblDiscount = (Label)FVJobDetail.FindControl("lblDiscount");
                Label lblReImport1 = (Label)FVJobDetail.FindControl("lblReImport1");
                Label lblReImport = (Label)FVJobDetail.FindControl("lblReImport");
                Label lblPrevImport1 = (Label)FVJobDetail.FindControl("lblPrevImport1");
                Label lblPrevImport = (Label)FVJobDetail.FindControl("lblPrevImport");

                lblCIFVal1.Visible = false; lblCIFVal.Visible = false;
                lblGrossUnit1.Visible = false; lblGrossUnit.Visible = false;

                lblDiscount1.Visible = false; lblDiscount.Visible = false;
                lblReImport1.Visible = false; lblReImport.Visible = false;
                lblPrevImport1.Visible = false; lblPrevImport.Visible = false;
            }

            if (Convert.ToInt32(Session["SEZType"]) == 1)
            {
                Label ddMode04 = (Label)FVJobDetail.FindControl("lbl1InwardBE");
                Label ddMode1 = (Label)FVJobDetail.FindControl("lbl2InwardBE");
                Label ddMode2 = (Label)FVJobDetail.FindControl("lbl1InwardDate");
                Label ddMode3 = (Label)FVJobDetail.FindControl("lbl2InwardDate");
                Label ddMode4 = (Label)FVJobDetail.FindControl("lbl1InwardJobNo");
                Label ddMode5 = (Label)FVJobDetail.FindControl("lbl2InwardJobNo");
                //Label ddMode6 = (Label)FVJobDetail.FindControl("lbl1DaysStore");
                //Label ddMode7 = (Label)FVJobDetail.FindControl("lbl2DaysStore");

                ddMode04.Visible = false;
                ddMode1.Visible = false;
                ddMode2.Visible = false;
                ddMode3.Visible = false;
                ddMode4.Visible = false;
                ddMode5.Visible = false;
                //ddMode6.Visible = false;
                //ddMode7.Visible = false;
            }
            else
            {
                Label ddMode04 = (Label)FVJobDetail.FindControl("lbl1InwardBE");
                Label ddMode1 = (Label)FVJobDetail.FindControl("lbl2InwardBE");
                Label ddMode2 = (Label)FVJobDetail.FindControl("lbl1InwardDate");
                Label ddMode3 = (Label)FVJobDetail.FindControl("lbl2InwardDate");
                Label ddMode4 = (Label)FVJobDetail.FindControl("lbl1InwardJobNo");
                Label ddMode5 = (Label)FVJobDetail.FindControl("lbl2InwardJobNo");
                //Label ddMode6 = (Label)FVJobDetail.FindControl("lbl1DaysStore");
                //Label ddMode7 = (Label)FVJobDetail.FindControl("lbl2DaysStore");

                ddMode04.Visible = true;
                ddMode1.Visible = true;
                ddMode2.Visible = true;
                ddMode3.Visible = true;
                ddMode4.Visible = true;
                ddMode5.Visible = true;
                //ddMode6.Visible = true;
                //ddMode7.Visible = true;
            }
        }
    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("SEZInfo.aspx");
    }

    protected void GrdDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }

    public void DocumentTypeBind()
    {
        DataSet dsDocType = SEZOperation.GetJobDocumentMS();

        if (dsDocType.Tables[0].Rows.Count > 0)
        {
            ddDocument.DataSource = dsDocType;
            ddDocument.DataBind();

            ddDocument.DataSource = dsDocType.Tables[0];
            ddDocument.DataTextField = "DocumentName";
            ddDocument.DataValueField = "lid";
            ddDocument.DataBind();
            //JobID = Convert.ToInt32(dsDocType.Tables[0].Rows[0]["lid"]);
        }
    }

    //public void PackagesUnit()
    //{
    //    DataSet dsDocType = SEZOperation.FillPackageType();

    //    if (dsDocType.Tables[0].Rows.Count > 0)
    //    {
    //        ddlddlDutyCustom.DataSource = dsDocType;
    //        ddDocument.DataBind();

    //        ddDocument.DataSource = dsDocType.Tables[0];
    //        ddDocument.DataTextField = "DocumentName";
    //        ddDocument.DataValueField = "lid";
    //        ddDocument.DataBind();
    //        //JobID = Convert.ToInt32(dsDocType.Tables[0].Rows[0]["lid"]);
    //    }
    //}

    //protected void GrdInvoiceDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //GrdInvoiceDetails.Caption = "";

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
    //        //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
    //    }

    //    string SEZJObType = "";
    //    SEZJObType = Session["SEZType"].ToString();
    //    if (SEZJObType == "1")
    //    {
    //       // GrdInvoiceDetails.Columns[7].Visible = true;
    //    }
    //    else
    //    {
    //       // GrdInvoiceDetails.Columns[7].Visible = false;
    //    }
    //}
    private void DownloadDocument(string DocumentPath)
    {

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            DocumentPath = "..\\UploadFiles\\" + DocumentPath;
            ServerPath = HttpContext.Current.Server.MapPath(DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }

        //ServerPath = ServerPath + DocumentPath;

        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnUpload_Click(Object Sender, EventArgs e)
    {
        string fileName = "";
        //FileUpload fuDocument = (FileUpload)FVJobDetail.FindControl("fuDocumentSEZ");
        string fuDocument = fuDocumentSEZ.FileName;

        if (fuDocumentSEZ != null && fuDocumentSEZ.HasFile)
            fileName = UploadFiles(fuDocumentSEZ, "");
        int doctype = Convert.ToInt32(ddDocument.SelectedValue);

        if (fileName != "")
        {
            //Label txtJobNo = (Label)FVJobDetail.FindControl("lblSEZJobNo");

            string jobrefno = "";

            if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
            {
                Label lblFolder = (Label)FVJobDetail.FindControl("lblSEZJobNo");
                if (lblFolder.Text.Trim() != "")
                {
                    jobrefno = lblFolder.Text.Trim();
                }
            }
            else if (FVJobDetail.CurrentMode == FormViewMode.Edit)
            {
                TextBox txtFolder = (TextBox)FVJobDetail.FindControl("txtJobNo");
                if (txtFolder.Text.Trim() != "")
                {
                    jobrefno = txtFolder.Text.Trim();
                }
            }

            string DocFolder = jobrefno;
            DocFolder = DocFolder.Replace("/", "");
            DocFolder = DocFolder.Replace("-", "");

            fileName = "SEZ\\" + DocFolder + "\\" + fileName;

            int result = 0;

            result = SEZOperation.AddSEZDocument(Convert.ToInt32(Session["JobId"]), jobrefno, doctype, fileName, fuDocumentSEZ.FileName, LoggedInUser.glUserId);
            // dtAnnexure.Rows.Add(PkId, doctype, doctypeName, fileName, fuDocument.FileName, LoggedInUser.glEmpName);
            if (result == 1)
            {
                lblError.Text = "Successfully Added SEZ Job Document..!!";
                lblError.CssClass = "success";

                BindGrid();  //Bind Gridview for Document
            }
            else if (result == -1)
            {
                lblError.Text = "Document is not Properly added";
                lblError.CssClass = "success";
            }

        }
    }
    protected void BindGrid()
    {
        DataSet dsDocDetail = SEZOperation.GetDocDetail(Convert.ToInt32(Session["JobId"]));
        if (dsDocDetail.Tables[0].Rows.Count > 0)
        {
            //SqlDataSource SqlDataSource1 = new SqlDataSource();
            //SqlDataSource1.ID = "DocumentSqlDataSource";
            //grdDocument.DataSourceID= "DocumentSqlDataSource";      //DocumentSqlDataSource; //dsDocDetail;
            grdDocument.DataBind();
        }
    }
    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        string ServerFilePath = FileServer.GetFileServerDir();

        string DocFolder = "";
        string jobrefno = "";

        if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
        {
            Label lblFolder = (Label)FVJobDetail.FindControl("lblSEZJobNo");
            if (lblFolder.Text.Trim() != "")
            {
                jobrefno = lblFolder.Text.Trim();
            }
        }
        else if (FVJobDetail.CurrentMode == FormViewMode.Edit)
        {
            TextBox txtFolder = (TextBox)FVJobDetail.FindControl("txtJobNo");
            if (txtFolder.Text.Trim() != "")
            {
                jobrefno = txtFolder.Text.Trim();
            }
        }

        DocFolder = jobrefno;
        DocFolder = DocFolder.Replace("/", "");
        DocFolder = DocFolder.Replace("-", "");


        if (ServerFilePath == "")
        {
            ServerFilePath = Server.MapPath("..\\UploadFiles\\SEZ\\" + DocFolder + "\\" + FilePath);
        }
        else
        {
            ServerFilePath = ServerFilePath + "\\SEZ\\" + DocFolder + "\\" + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);
                string FileId = RandomString(5);
                FileName += "_" + FileId + ext;
            }
            FU.SaveAs(ServerFilePath + FileName);
            return FilePath + FileName;
        }
        else
            return "";
    }

    protected void btnUpdateJobDetail_Click(object sender, EventArgs e)
    {
        string FileBilling = "";

        int Jobid = Convert.ToInt32(Session["JobId"]);

        // Check Is File Sent TO Billing Or Not 
        DataSet dsFileSentBill = SEZOperation.GetFileSentBilling(Jobid);

        if (dsFileSentBill.Tables[0].Rows.Count > 0)
        {
            FileBilling = Convert.ToString(dsFileSentBill.Tables[0].Rows[0]["FileSentToBilling"]);
        }

        if (FileBilling == "")
        {
            if (Jobid > 0)
            {
                int Mode = 0, Currency = 0, DaysStore = 0, NoOfPackage = 0, NoOfVehicles = 0, ServicesProvide = 0, DutyCustom = 0, PackagesUnit = 0,
                              GrossUnit = 0, Destination = 0, Country = 0, Place = 0;
                string Supplier = "", InwardBENo = "", InwardJobNo = "", BENo = "", RequestId = "", BillingStatus = "", RNLogistics = "", Remark = "",
                       BuyerName = "", SchemeCode = "";
                decimal AssesableValue = 0, ExRate = 0, DutyAmount = 0, GrossWeight = 0, CIFValue = 0;
                bool Discount = false, ReImport = false, PrevImport = false, PrevExpGoods = false, CessDetail = false,
                     LicenceRegNo = false, ReExport = false, PrevExport = false;


                DateTime InwardBEDate = DateTime.MinValue, BEDate = DateTime.MinValue, InwardDate = DateTime.MinValue,
                         OutwardDate = DateTime.MinValue, PCDFrDahej = DateTime.MinValue, PCDSentClient = DateTime.MinValue,
                         FileSentToBilling = DateTime.MinValue;

                Mode = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddMode")).SelectedValue);
                Currency = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlCurrency")).SelectedValue);
                Supplier = ((TextBox)FVJobDetail.FindControl("txtSupplierName")).Text.Trim();

                if (((TextBox)FVJobDetail.FindControl("txtAssesableValue")).Text.Trim() != "")
                    AssesableValue = Convert.ToDecimal(((TextBox)FVJobDetail.FindControl("txtAssesableValue")).Text.Trim());
                //  InvoiceValue = ((TextBox)FVJobDetail.FindControl("txtInvoiceValue")).Text.Trim();
                //  Term = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlTerm")).SelectedValue);

                if (((TextBox)FVJobDetail.FindControl("txtExRate")).Text.Trim() != "")
                    ExRate = Convert.ToDecimal(((TextBox)FVJobDetail.FindControl("txtExRate")).Text.Trim());

                InwardBENo = ((TextBox)FVJobDetail.FindControl("txtInwardBENo")).Text.Trim();

                if (((TextBox)FVJobDetail.FindControl("txtInwardBEDate")).Text.Trim() != "")
                    InwardBEDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtInwardBEDate")).Text.Trim());

                InwardJobNo = ((TextBox)FVJobDetail.FindControl("txtInwardJobNo")).Text.Trim();

                //TextBox txtDaysStore = (TextBox)FVJobDetail.FindControl("txtDaysStore");
                //if (txtDaysStore.Text.Trim() != "")
                //{
                //    DaysStore = Convert.ToInt32(txtDaysStore.Text.Trim());
                //}

                BENo = ((TextBox)FVJobDetail.FindControl("txtBENo")).Text.Trim();

                if (((TextBox)FVJobDetail.FindControl("txtBEDate")).Text.Trim() != "")
                    BEDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtBEDate")).Text.Trim());

                if (((TextBox)FVJobDetail.FindControl("txtRequestId")).Text.Trim() != "")
                    RequestId = ((TextBox)FVJobDetail.FindControl("txtRequestId")).Text.Trim();

                RadioButtonList DiscountAppli = (RadioButtonList)FVJobDetail.FindControl("rdlDiscountAppli");
                if (DiscountAppli.SelectedValue == "1")
                {
                    Discount = true;
                }

                RadioButtonList ReImport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReImport");
                if (ReImport1.SelectedValue == "1")
                {
                    ReImport = true;
                }

                RadioButtonList PreviousImport = (RadioButtonList)FVJobDetail.FindControl("rdlPreviousImport");
                if (PreviousImport.SelectedValue == "1")
                {
                    PrevImport = true;
                }

                if (((TextBox)FVJobDetail.FindControl("txtDutyAmount")).Text.Trim() != "")
                    DutyAmount = Convert.ToDecimal(((TextBox)FVJobDetail.FindControl("txtDutyAmount")).Text.Trim());

                //--- Start SB -----

                RadioButtonList PrevExpGoods1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExpGoods");
                if (PrevExpGoods1.SelectedValue == "1")
                {
                    PrevExpGoods = true;
                }

                RadioButtonList CessDetail1 = (RadioButtonList)FVJobDetail.FindControl("rdlCessDetail");
                if (CessDetail1.SelectedValue == "1")
                {
                    CessDetail = true;
                }

                RadioButtonList LicenceRegNo1 = (RadioButtonList)FVJobDetail.FindControl("rdlLicenceRegNo");
                if (LicenceRegNo1.SelectedValue == "1")
                {
                    LicenceRegNo = true;
                }

                RadioButtonList ReExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlReExport");
                if (ReExport1.SelectedValue == "1")
                {
                    ReExport = true;
                }

                RadioButtonList PrevExport1 = (RadioButtonList)FVJobDetail.FindControl("rdlPrevExport");
                if (PrevExport1.SelectedValue == "1")
                {
                    PrevExport = true;
                }

                //----End SB -------




                //if (((TextBox)FVJobDetail.FindControl("txtInwardDate")).Text.Trim() != "")
                //    InwardDate = Commonfunctions.CDateTime(((TextBox)FVJobDetail.FindControl("txtInwardDate")).Text.Trim());

                //TextBox NoOfPackages = (TextBox)FVJobDetail.FindControl("txtNoOfPackage");
                //if (NoOfPackages.Text.Trim() != "")
                //    NoOfPackage = Convert.ToInt32(NoOfPackages.Text.Trim());

                //TextBox GrossWeights = (TextBox)FVJobDetail.FindControl("txtGrossWeight");
                //if (GrossWeights.Text.Trim() != "")
                //    GrossWeight = Convert.ToDecimal(GrossWeights.Text.Trim());

                //TextBox NoOfVehicles1 = (TextBox)FVJobDetail.FindControl("txtNoOfVehicles");
                //if (NoOfVehicles1.Text.Trim() != "")
                //    NoOfVehicles = Convert.ToInt32(NoOfVehicles1.Text.Trim());

                //ServicesProvide = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlServicesProvide")).SelectedValue);

                TextBox txtoutward = (TextBox)FVJobDetail.FindControl("txtOutwardDate");
                if (txtoutward.Text.Trim() != "")
                    OutwardDate = Commonfunctions.CDateTime(txtoutward.Text.Trim());

                TextBox txPCDFrDahej = (TextBox)FVJobDetail.FindControl("txtPCDFrDahej");
                if (txPCDFrDahej.Text.Trim() != "")
                    PCDFrDahej = Commonfunctions.CDateTime(txPCDFrDahej.Text.Trim());

                TextBox ttPCDSentClient = (TextBox)FVJobDetail.FindControl("txtPCDSentClient");
                if (ttPCDSentClient.Text.Trim() != "")
                    PCDSentClient = Commonfunctions.CDateTime(ttPCDSentClient.Text.Trim());

                TextBox ttFileSentToBilling = (TextBox)FVJobDetail.FindControl("txtFileSentToBilling");
                if (ttFileSentToBilling.Text.Trim() != "")
                    FileSentToBilling = Commonfunctions.CDateTime(ttFileSentToBilling.Text.Trim());

                TextBox BillingStatuss = (TextBox)FVJobDetail.FindControl("txtBillingStatus");
                BillingStatus = Convert.ToString(BillingStatuss.Text.Trim());

                //TextBox RNLogisticss = (TextBox)FVJobDetail.FindControl("txtRNLogistics");
                //RNLogistics = Convert.ToString(RNLogisticss.Text.Trim());

                TextBox txtCIFValue = (TextBox)FVJobDetail.FindControl("txtCIFValue");
                // if(Convert.ToInt32(txtCIFValue.Text.Trim()) > 0)
                if (txtCIFValue.Text.Trim() != "")
                {
                    CIFValue = Convert.ToDecimal(txtCIFValue.Text.Trim());
                }

                //DutyCustom = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlDutyCustom")).SelectedValue);
                //PackagesUnit = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPackagesUnit")).SelectedValue);

                TextBox txtBuyerName = (TextBox)FVJobDetail.FindControl("txtBuyerName");
                BuyerName = Convert.ToString(txtBuyerName.Text.Trim());
                TextBox txtSchemeCode = (TextBox)FVJobDetail.FindControl("txtSchemeCode");
                SchemeCode = Convert.ToString(txtSchemeCode.Text.Trim());

                GrossUnit = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlGrossUnit")).SelectedValue);
                //Destination = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlDestination")).SelectedValue);
                //Country = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlCountry")).SelectedValue);
                //Place = Convert.ToInt32(((DropDownList)FVJobDetail.FindControl("ddlPlace")).SelectedValue);

                TextBox Remarkk = (TextBox)FVJobDetail.FindControl("txtRemark");
                Remark = Convert.ToString(Remarkk.Text.Trim());

                int result = SEZOperation.UpdateSEZJobDetail(Jobid, Mode, Supplier, Currency, AssesableValue,
                     ExRate, InwardBENo, InwardBEDate, InwardJobNo, DaysStore, BENo, BEDate, RequestId, DutyAmount,
                    InwardDate, NoOfPackage, GrossWeight, NoOfVehicles, ServicesProvide, OutwardDate, PCDFrDahej,
                    PCDSentClient, FileSentToBilling, BillingStatus, RNLogistics, DutyCustom, PackagesUnit, CIFValue, Remark,
                    BuyerName, SchemeCode, GrossUnit, Discount, ReImport, PrevImport, Destination, Country, Place,
                    PrevExpGoods, CessDetail, LicenceRegNo, ReExport, PrevExport, LoggedInUser.glUserId);

                if (result == 0)
                {
                    lblError.Text = "Job Detail Updated Successfully !";
                    lblError.CssClass = "success";

                    FVJobDetail.ChangeMode(FormViewMode.ReadOnly);
                    //GetJobDetail(JobId);
                    JobDetailMS(Convert.ToInt32(Session["JobId"]));
                }
                else if (result == 2)
                {
                    lblError.Text = "Job Ref No Already Exist!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
            }//END_IF_JobId Check
            else
            {
                Response.Redirect("EditJobTracking.aspx");
            }
        }
        else
        {
            lblError.Text = "After File Sent To Billing...Job Details Can Not Modified";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void txtFileSentToBilling_TextChanged(object sender, EventArgs e)
    {
        TextBox txtoutward = (TextBox)FVJobDetail.FindControl("txtOutwardDate");

        if (txtoutward.Text == "")
        {

            TextBox ttFileSentToBilling = (TextBox)FVJobDetail.FindControl("txtFileSentToBilling");

            ttFileSentToBilling.Text = "";

            lblError.Text = "Enter the Dispatch Date Before File Sent To Billing";
            lblError.CssClass = "errorMsg";
        }

    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = gvJobDetail.DataKeys[gvrow.RowIndex].Value.ToString();
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            gvJobDetail.EditIndex = -1;
        }
    }
    protected void gvJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvJobDetail.EditIndex = -1;
    }
    protected void gvJobDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvJobDetail.EditIndex = e.NewEditIndex;
    }

    protected void gvJobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int InvoiceId = 0;
       
        int JobId = Convert.ToInt32(gvJobDetail.DataKeys[e.RowIndex].Value.ToString());
        HiddenField hdnInvId = (HiddenField)gvJobDetail.Rows[e.RowIndex].FindControl("hdnInvId");
        HiddenField hdnJobId = (HiddenField)gvJobDetail.Rows[e.RowIndex].FindControl("hdnJobId");

        if (hdnInvId.Value != "")
        {
            InvoiceId = Convert.ToInt32(hdnInvId.Value);
        }
       

        TextBox txtInvDate = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtInvDate");
        TextBox txtInvVal = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtInvVal");
        TextBox txtDescription = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtDescription");
        TextBox txtQuantity = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtQuantity");
        TextBox txtremQuantity = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtremQuantity");
        TextBox txtItemPrice = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtItemPrice");
        TextBox txtProductValue = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtProductValue");
        TextBox txtCTH = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtCTH");

        DateTime dtInvDate = DateTime.MinValue;
        if (txtInvDate.Text.Trim() != "")
        {
            dtInvDate = Commonfunctions.CDateTime(txtInvDate.Text.Trim());
        }

        if (InvoiceId != 0)
        {
            int result = SEZOperation.AddInvoiceDetail(JobId, InvoiceId, dtInvDate, txtInvVal.Text.Trim(), txtDescription.Text.Trim(),
                txtQuantity.Text.Trim(), txtItemPrice.Text.Trim(), txtProductValue.Text.Trim(), txtCTH.Text.Trim(), LoggedInUser.glUserId);


            if (result == 0)
            {
                lblError.Text = "Noting Detail Added Successfully.";
                lblError.CssClass = "success";

                gvJobDetail.EditIndex = -1;
                e.Cancel = true;
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else if (result == 2)
            {
                lblError.Text = "Invoice Detail Already Created!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }//END_IF
        else
        {
            lblError.CssClass = "errormsg";
            lblError.Text = "InVoice No Not Found";
        }
    }
    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        lblError.Visible = true;
        //int EnqId = Convert.ToInt32(Session["EnqId"]);        
        Label lblFolder = (Label)FVJobDetail.FindControl("lblSEZJobNo");
        string JobRefNo = lblFolder.Text.Trim();
        int ContainerSize = 0, ContainerType = 0;

        string ContainerNo = txtContainerNo.Text.Trim();
        ContainerSize = Convert.ToInt32(ddContainerSize.SelectedValue);
        ContainerType = Convert.ToInt32(ddContainerType.SelectedValue);

        if (ContainerType == 1) //FCL
        {
            if (ContainerSize == 0)
            {
                lblError.Text = "Please Select FCL Container Size!";
                lblError.CssClass = "errorMsg";
                return;
            }
        }
        else if (ContainerType == 2) //LCL
        {
            ddContainerSize.SelectedValue = "0";
            ContainerSize = 0;
        }

        if (ContainerNo != "")
        {
            int result = SEZOperation.ADDSEZContainer(JobRefNo, ContainerNo, ContainerSize, ContainerType, LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Container No " + ContainerNo + " Added successfully!";
                lblError.CssClass = "success";
                gvContainer.DataBind();
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Container No " + ContainerNo + " Already Added!";
                lblError.CssClass = "warning";
            }
        }
        else
        {
            lblError.CssClass = "errorMsg";
            lblError.Text = " Please Enter Container No.!";
        }
    }

    protected void gvContainer_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            //gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvContainer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvContainer.EditIndex = e.NewEditIndex;
    }

    protected void gvContainer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvContainer.EditIndex = -1;
    }

    protected void gvContainer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int InvoiceId = 0;

        int JobId = Convert.ToInt32(gvContainer.DataKeys[e.RowIndex].Value.ToString());
        //HiddenField hdnInvId = (HiddenField)gvContainer.Rows[e.RowIndex].FindControl("hdnInvId");
        //HiddenField hdnJobId = (HiddenField)gvContainer.Rows[e.RowIndex].FindControl("hdnJobId");

        //if (hdnInvId.Value != "")
        //{
        //    InvoiceId = Convert.ToInt32(hdnInvId.Value);
        //}


        // TextBox txtInvDate = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtInvDate");

        string strContType = "0";
        string strContSize = "0";


        Label lblSEZJobNo = (Label)FVJobDetail.FindControl("lblSEZJobNo");
        TextBox txtEditContainerNo = (TextBox)gvContainer.Rows[e.RowIndex].FindControl("txtEditContainerNo");
        DropDownList ddEditContainerType = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerType");
        if (ddEditContainerType.SelectedValue != "")
        {
            strContType = ddEditContainerType.SelectedValue;
        }
        DropDownList ddEditContainerSize = (DropDownList)gvContainer.Rows[e.RowIndex].FindControl("ddEditContainerSize");
        if (ddEditContainerSize.SelectedValue != "")
        {
            strContSize = ddEditContainerSize.SelectedValue;
        }

        if (txtEditContainerNo.Text.Trim() !="")
        {
            int result = SEZOperation.updSEZContainer(lblSEZJobNo.Text.Trim(), txtEditContainerNo.Text.Trim(), strContSize, strContType, LoggedInUser.glUserId);


            if (result == 0)
            {
                lblError.Text = "Container Detail Added Successfully.";
                lblError.CssClass = "success";

                gvContainer.EditIndex = -1;
                e.Cancel = true;
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else if (result == 2)
            {
                lblError.Text = "Container Detail Already Created!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
        }//END_IF
        else
        {
            lblError.CssClass = "errormsg";
            lblError.Text = "InVoice No Not Found";
        }
    }

    protected void gvContainer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = gvContainer.DataKeys[gvrow.RowIndex].Value.ToString();
        }

        if (e.CommandName.ToLower() == "cancel")
        {
            gvContainer.EditIndex = -1;
        }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        // ModalPopupContainer.Show();
    }
    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[1].FindControl("txtInvoiceNum");
                    TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[2].FindControl("txtInvoiceDt");
                    TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtValueInvoice");
                    DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[4].FindControl("ddlTermInvoice");
                    TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[5].FindControl("txtDescriptionProd");
                    TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtQuantity");

                    TextBox txtRemainingQty = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtRemainingQty");
                    TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtItemPrice");
                    TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtProductVal");
                    TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtCTH");
                    DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("ddlItemType");

                    drCurrentRow = dtCurrentTable.NewRow();
                    // drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["InvoiceNo"] = txtInvoiceNum.Text;
                    dtCurrentTable.Rows[i - 1]["InvoiceDate"] = txtInvoiceDt.Text;
                    dtCurrentTable.Rows[i - 1]["InvoiceValue"] = txtValueInvoice.Text;
                    dtCurrentTable.Rows[i - 1]["Term"] = ddlTermInvoice.Text;
                    dtCurrentTable.Rows[i - 1]["Description"] = txtDescriptionProd.Text;
                    dtCurrentTable.Rows[i - 1]["Quantity"] = txtQuantity.Text;

                    dtCurrentTable.Rows[i - 1]["NewQty"] = txtRemainingQty.Text;
                    dtCurrentTable.Rows[i - 1]["ItemPrice"] = txtItemPrice.Text;
                    dtCurrentTable.Rows[i - 1]["ProdValue"] = txtProductVal.Text;
                    dtCurrentTable.Rows[i - 1]["CTH"] = txtCTH.Text;
                    dtCurrentTable.Rows[i - 1]["ItemType"] = ddlItemType.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                GrdInvoiceDetail.DataSource = dtCurrentTable;
                GrdInvoiceDetail.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData();
        TextBox txtInvoiceNum1 = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[1].FindControl("txtInvoiceNum");
        txtInvoiceNum1.Focus();


    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[1].FindControl("txtInvoiceNum");
                    TextBox txtInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[2].FindControl("txtInvoiceDt");
                    TextBox txtValueInvoice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtValueInvoice");
                    DropDownList ddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("ddlTermInvoice");
                    TextBox txtDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtDescriptionProd");
                    TextBox txtQuantity = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[3].FindControl("txtQuantity");

                    TextBox txtRemainingQty = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtRemainingQty");
                    TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtItemPrice");
                    TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtProductVal");
                    TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("txtCTH");
                    DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[rowIndex].Cells[6].FindControl("ddlItemType");

                    txtInvoiceNum.Text = dt.Rows[i]["InvoiceNo"].ToString();
                    txtInvoiceDt.Text = dt.Rows[i]["InvoiceDate"].ToString();
                    txtValueInvoice.Text = dt.Rows[i]["InvoiceValue"].ToString();
                    ddlTermInvoice.SelectedValue = dt.Rows[i]["Term"].ToString();
                    txtDescriptionProd.Text = dt.Rows[i]["Description"].ToString();
                    txtQuantity.Text = dt.Rows[i]["Quantity"].ToString();

                    txtRemainingQty.Text = dt.Rows[i]["NewQty"].ToString();
                    txtItemPrice.Text = dt.Rows[i]["ItemPrice"].ToString();
                    txtProductVal.Text = dt.Rows[i]["ProdValue"].ToString();
                    txtCTH.Text = dt.Rows[i]["CTH"].ToString();
                    ddlItemType.SelectedValue = dt.Rows[i]["ItemType"].ToString();

                    rowIndex++;
                }
            }
        }
    }

    private void SetInitialRow()
    {
        //GrdInvoiceDetail.DataSource = null;
        //GrdInvoiceDetail.DataBind();

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dt.Columns.Add(new DataColumn("InvoiceValue", typeof(string)));
        dt.Columns.Add(new DataColumn("Term", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("Quantity", typeof(string)));

        dt.Columns.Add(new DataColumn("NewQty", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemPrice", typeof(string)));
        dt.Columns.Add(new DataColumn("ProdValue", typeof(string)));
        dt.Columns.Add(new DataColumn("CTH", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemType", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["InvoiceNo"] = string.Empty;
        dr["InvoiceDate"] = string.Empty;
        dr["InvoiceValue"] = string.Empty;
        dr["Term"] = string.Empty;
        dr["Description"] = string.Empty;
        dr["Quantity"] = string.Empty;

        dr["NewQty"] = string.Empty;
        dr["ItemPrice"] = string.Empty;
        dr["ProdValue"] = string.Empty;
        dr["CTH"] = string.Empty;
        dr["ItemType"] = string.Empty;

        dt.Rows.Add(dr);
        //dr = dt.NewRow();
        ViewState["CurrentTable"] = null;
        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;

        GrdInvoiceDetail.DataSource = dt;
        GrdInvoiceDetail.DataBind();

    }

    protected void txtRemainingQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal ProductValue = 0, ItemPrice1 = 0, Qty = 0, NewQty = 0;
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)thisTextBox.Parent.Parent;
            int rowindex = 0;
            rowindex = currentRow.RowIndex;

            // following line will get the label value in current row   

            TextBox txtQty = (TextBox)currentRow.FindControl("txtQuantity");
            TextBox txtQty2 = (TextBox)currentRow.FindControl("txtRemainingQty");
            TextBox txtPrice = (TextBox)currentRow.FindControl("txtItemPrice");
            TextBox ProdValue = (TextBox)currentRow.FindControl("txtProductVal");

            Qty = Convert.ToDecimal(txtQty.Text.Trim());
            NewQty = Convert.ToDecimal(txtQty2.Text.Trim());
            if (txtPrice.Text.Trim() != "")
            {
                ItemPrice1 = Convert.ToDecimal(txtPrice.Text.Trim());
            }

            if (Qty < NewQty)
            {
                txtQty2.Text = "";
                txtPrice.Text = "";
                lblError.Text = "New Quantity is always Less Than Total Quantity ";
                lblError.CssClass = "errorMsg";
                txtQty2.Focus();
            }
            else
            {
                if (txtPrice.Text != "" || txtPrice.Text != null)
                {
                    ProductValue = NewQty * ItemPrice1;
                    ProdValue.Text = Convert.ToString(ProductValue);
                }
                lblError.Text = "";
                txtPrice.Focus();
            }
            
        }
        catch
        {
            //Response.Redirect("error.aspx");
        }
    }
    protected void txtItemPrice_TextChanged(object sender, EventArgs e)
    {
        decimal ProductValue = 0, TotQty = 0, ItemPrice1 = 0, RemQty = 0;
        try
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow currentRow = (GridViewRow)thisTextBox.Parent.Parent;

            TextBox TotalQuantity = (TextBox)currentRow.FindControl("txtQuantity");
            TextBox RemQuantity = (TextBox)currentRow.FindControl("txtRemainingQty");
            TextBox ItemPrice = (TextBox)currentRow.FindControl("txtItemPrice");
            TextBox ProdValue = (TextBox)currentRow.FindControl("txtProductVal");

            if (TotalQuantity.Text != "")
            {
                TotQty = Convert.ToDecimal(TotalQuantity.Text.Trim());
            }

            if (ItemPrice.Text != "")
            {
                ItemPrice1 = Convert.ToDecimal(ItemPrice.Text.Trim());
            }

            string SEZType = Session["SEZType"].ToString();
            string InwardJobNo = Session["InwardJobNo"].ToString();


            if (SEZType == "2")
            {
                if (InwardJobNo != "")
                {
                    if (RemQuantity.Text != "")
                    {
                        RemQty = Convert.ToDecimal(RemQuantity.Text.Trim());
                        ProductValue = RemQty * ItemPrice1;
                    }
                }
                else
                {
                    ProductValue = TotQty * ItemPrice1;
                }
            }
            else if (SEZType == "1")
            {
                ProductValue = TotQty * ItemPrice1;
            }

            ProdValue.Text = Convert.ToString(ProductValue);
            ProdValue.Focus();
        }
        catch
        {
            Response.Redirect("error.aspx");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (GrdInvoiceDetail.Rows.Count > 0)
        {
            int result = -123;
            //int SEZType = Convert.ToInt32(rdbSEZtype.SelectedValue);
            for (int i = 0; i < GrdInvoiceDetail.Rows.Count; i++)
            {
                DateTime dtInvoice = DateTime.MinValue;
                decimal InvoiceValue1 = 0;
                decimal Quantity = 0, ItemPrice = 0, ProductValue = 0, CTH = 0;

                TextBox strInvoiceNum = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtInvoiceNum");
                TextBox strInvoiceDt = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtInvoiceDt");
                if (strInvoiceDt.Text.Trim() != "")
                {
                    dtInvoice = Commonfunctions.CDateTime(strInvoiceDt.Text.Trim());
                }
                TextBox strValueInvoice = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtValueInvoice");

                if (strValueInvoice.Text.Trim() != "")
                {
                    InvoiceValue1 = Convert.ToDecimal(strValueInvoice.Text.Trim());
                }

                DropDownList strddlTermInvoice = (DropDownList)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("ddlTermInvoice");
                TextBox strDescriptionProd = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtDescriptionProd");
                string Qty = "0";
                string strSEZType = Session["SEZType"].ToString();
                string InwardJobNo = Session["InwardJobNo"].ToString();
                int jobid = Convert.ToInt32(Session["JobId"]);
                string BsJobNo = Session["JobRefNo"].ToString();

                if (strSEZType == "1")
                {
                    TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtQuantity");
                    Qty = strQuantity.Text.Trim();
                }
                if (strSEZType == "2")
                {
                    if (InwardJobNo != "")
                    {
                        TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtRemainingQty");
                        Qty = strQuantity.Text.Trim();
                    }
                    else
                    {
                        TextBox strQuantity = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtQuantity");
                        Qty = strQuantity.Text.Trim();
                    }
                }
                if (Qty != "")
                {
                    Quantity = Convert.ToDecimal(Qty);
                }

                TextBox txtItemPrice = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtItemPrice");
                if (txtItemPrice.Text.Trim() != "")
                {
                    ItemPrice = Convert.ToDecimal(txtItemPrice.Text.Trim());
                }

                TextBox txtProductVal = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtProductVal");
                if (txtProductVal.Text.Trim() != "")
                {
                    ProductValue = Convert.ToDecimal(txtProductVal.Text.Trim());
                }

                TextBox txtCTH = (TextBox)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("txtCTH");
                if (txtCTH.Text.Trim() != "")
                {
                    CTH = Convert.ToDecimal(txtCTH.Text.Trim());
                }

                DropDownList ddlItemType = (DropDownList)GrdInvoiceDetail.Rows[i].Cells[1].FindControl("ddlItemType");
                int SEZType = Convert.ToInt32(strSEZType);

                if (strInvoiceNum.Text.Trim() != "")
                {
                    result = SEZOperation.AddSEZInvoice(jobid, BsJobNo, SEZType, strInvoiceNum.Text.Trim(), dtInvoice, InvoiceValue1,
                        Convert.ToInt32(strddlTermInvoice.SelectedValue), strDescriptionProd.Text.Trim(), Quantity, ItemPrice,
                        ProductValue, CTH, Convert.ToInt32(ddlItemType.SelectedValue), LoggedInUser.glUserId);
                }
            }
        }
        gvJobDetail.DataBind();
        Response.Redirect("SEZEditJobDetail.aspx");
        AccINVDetails.Focus();
    }
}