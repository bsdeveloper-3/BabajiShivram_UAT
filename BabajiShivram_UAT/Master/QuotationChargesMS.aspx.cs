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
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

public partial class Master_QuotationChargesMS : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExportRangeDetail);
        ScriptManager1.RegisterPostBackControl(gvQuotationCharge);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Quotation Charges Setup";

            lblresult.Visible = false;
            FillSearchForCatg();
            ddlSearchForCatg_OnSelectedIndexChanged(null, EventArgs.Empty);
        }
    }

    protected void FillSearchForCatg()
    {
        DataSet dsGetQuoteCatgs = QuotationOperations.GetQuotationCatg();
        ddlSearchForCatg.DataSource = dsGetQuoteCatgs;
        ddlSearchForCatg.DataTextField = "sName";
        ddlSearchForCatg.DataValueField = "lid";
        ddlSearchForCatg.DataBind();
        ddlSearchForCatg.SelectedIndex = -1;
    }

    protected void btnAddCategory_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("QuotationCategoryMS.aspx");
    }

    protected void ddlSearchForCatg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSearchForCatg.SelectedValue != null && ddlSearchForCatg.SelectedValue != "0")
            {
                FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
                gvQuotationCharge.DataBind();
            }
        }
        catch (Exception en)
        {

        }
    }

    #region QUOTATION MASTER EVENTS
    protected void gvQuotationCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string RangesCount = DataBinder.Eval(e.Row.DataItem, "RangeCount").ToString();

            if (RangesCount != "" && RangesCount == "0")
            {
                e.Row.Cells[2].ToolTip = "Add ranges for this charges here.";
            }
            else
            {
                e.Row.Cells[2].ToolTip = "Click here to show list of ranges for this charge.";
            }
        }
    }

    protected void gvQuotationCharge_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            TextBox lid = gvQuotationCharge.FooterRow.FindControl("txtlidfooter") as TextBox;
            TextBox txtChargeName = gvQuotationCharge.FooterRow.FindControl("txtQuotationChargefooter") as TextBox;
            TextBox txtDescription = gvQuotationCharge.FooterRow.FindControl("txtDescriptionfooter") as TextBox;
            //DropDownList ddlQuoteCatgFooter = gvQuotationCharge.FooterRow.FindControl("ddlQuoteCatgFooter") as DropDownList;

            if (txtChargeName.Text.Trim() != "" && txtChargeName.Text.Trim() != "")
            {
                int result = QuotationOperations.AddQuotationChargeMS(txtChargeName.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt32(ddlSearchForCatg.SelectedValue), LoggedInUser.glUserId);
                if (result == 2)
                {
                    lblresult.Text = txtChargeName.Text.Trim() + " Charge Added Successfully..!!";
                    lblresult.CssClass = "success";
                    //FillSearchForCatg();
                    FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 3)
                {
                    lblresult.Text = "Quotation Charge Already Exist!";
                    lblresult.CssClass = "warning";
                }

            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
        else if (e.CommandName.Trim().ToLower() == "getrange")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(',');
            string ChargeId = commandArgs[0].ToString();
            ViewState["ChargeId"] = ChargeId;
            FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
            lblChargeName.Text = commandArgs[1].ToString();
            ModalPopupExtender3.Show();
        }
        else if (e.CommandName.Trim().ToLower() == "getcatg")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(',');
            string ChargeId = commandArgs[0].ToString();
            ViewState["ChargeId"] = ChargeId;
            FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
            lblChargeName2.Text = commandArgs[1].ToString();

            if (cblCategories.Items.Count > 0)
            {
                for (int c = 0; c < cblCategories.Items.Count; c++)
                {
                    cblCategories.Items[c].Selected = false;
                }
            }

            //SELECT THE EXSTING CATEGORIES FOR GIVEN CHARGE
            DataSet dsGetCatgLists = QuotationOperations.GetCatgAsPerCharge(Convert.ToInt32(ChargeId));
            if (dsGetCatgLists != null && dsGetCatgLists.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsGetCatgLists.Tables[0].Rows.Count; i++)
                {
                    for (int c = 0; c < cblCategories.Items.Count; c++)
                    {
                        //cblCategories.Items[c].Selected = false;
                        if (cblCategories.Items[c].Value == dsGetCatgLists.Tables[0].Rows[i]["QuoteCatgId"].ToString())
                        {
                            cblCategories.Items[c].Selected = true;
                            break;
                        }
                    }
                }
            }

            ModalPopupExtender3.Hide();
            mpeCatgForCharge.Show();
        }
    }

    protected void gvQuotationCharge_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int Lid = Convert.ToInt32(gvQuotationCharge.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtQuotationCharge = (TextBox)gvQuotationCharge.Rows[e.RowIndex].FindControl("txtQuotationCharge");
        TextBox txtDescription = (TextBox)gvQuotationCharge.Rows[e.RowIndex].FindControl("txtDescription");
        // DropDownList ddlQuoteCatg = (DropDownList)gvQuotationCharge.Rows[e.RowIndex].FindControl("ddlQuoteCatg");

        if (txtQuotationCharge.Text.Trim() != "" && txtQuotationCharge.Text.Trim() != "")
        {
            int result = QuotationOperations.UpdateQuotationChargeMS(Lid, txtQuotationCharge.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt32(ddlSearchForCatg.SelectedValue), LoggedInUser.glUserId);
            if (result == 2)
            {
                lblresult.CssClass = "success";
                lblresult.Text = txtQuotationCharge.Text.Trim() + " Charge Updated Successfully..!!";
                gvQuotationCharge.EditIndex = -1;
                FillSearchForCatg();
                FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 3)
            {
                lblresult.Text = "Quotation Charge Does Not Exists..!!";
                lblresult.CssClass = "warning";
            }
        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvQuotationCharge_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvQuotationCharge.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteQuotationCharges(lid, LoggedInUser.glUserId);
        if (result == 2)
        {
            lblresult.Text = "Quotation Charge Deleted Successfully!";
            lblresult.CssClass = "success";
            FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
        else if (result == 3)
        {
            lblresult.Text = "Quotation Charge Does Not Exists..!!";
            lblresult.CssClass = "warning";
        }
    }

    protected void FillCharges(int CatgId)
    {
        DataSet ds = new DataSet();
        ds = QuotationOperations.GetQuotationCharges(CatgId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuotationCharge.DataSource = ds;
            gvQuotationCharge.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvQuotationCharge.DataSource = ds;
            gvQuotationCharge.DataBind();
            int columncount = gvQuotationCharge.Rows[0].Cells.Count;
            gvQuotationCharge.Rows[0].Cells.Clear();
            gvQuotationCharge.Rows[0].Cells.Add(new TableCell());
            gvQuotationCharge.Rows[0].Cells[0].ColumnSpan = columncount;
            gvQuotationCharge.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvQuotationCharge_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvQuotationCharge.EditIndex = e.NewEditIndex;
        FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationCharge_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvQuotationCharge.EditIndex = -1;
        FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvQuotationCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvQuotationCharge.PageIndex = e.NewPageIndex;
        gvQuotationCharge.DataBind();
        FillCharges(Convert.ToInt32(ddlSearchForCatg.SelectedValue));
    }

    protected void btnCancelPopup_Click1(object sender, EventArgs e)
    {
        ModalPopupExtender3.Hide();
    }

    protected void imgdelCatg_Click(object sender, EventArgs e)
    {
        mpeCatgForCharge.Hide();
    }

    #endregion

    #region CATEGORY FOR CHARGES

    protected void btnSaveCatg_OnClick(object sender, EventArgs e)
    {
        int count = 0;
        if (cblCategories.Items.Count > 0)
        {
            for (int i = 0; i < cblCategories.Items.Count; i++)
            {
                if (cblCategories.Items[i].Selected == true)
                {
                    int result_Save = QuotationOperations.AddCatgAsPerCharge(Convert.ToInt32(ViewState["ChargeId"]), Convert.ToInt32(cblCategories.Items[i].Value), LoggedInUser.glUserId);
                    if (result_Save == 0)
                        count = 1;
                }
                else
                {
                    int result_Upd = QuotationOperations.UpdateCatgAsPerCharge(Convert.ToInt32(ViewState["ChargeId"]), Convert.ToInt32(cblCategories.Items[i].Value), LoggedInUser.glUserId);
                    if (result_Upd == 0)
                        count = 1;
                }
            }
        }
    }


    #endregion

    #region RANGES EVENTS

    protected void btnAddAppFields_Onclick(object sender, EventArgs e)
    {
        Response.Redirect("QuoteApplicableFieldsMS.aspx");
    }

    protected void gvChargeWsRanges_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;
        if (e.CommandName == "Insert")
        {
            int ChargeId = 0;
            ChargeId = Convert.ToInt32(ViewState["ChargeId"].ToString());
            DropDownList ddlCurrencyfooter = gvChargeWsRanges.FooterRow.FindControl("ddlCurrencyfooter") as DropDownList;
            TextBox txtMinRangefooter = gvChargeWsRanges.FooterRow.FindControl("txtMinRangefooter") as TextBox;
            TextBox txtMaxRangefooter = gvChargeWsRanges.FooterRow.FindControl("txtMaxRangefooter") as TextBox;
            DropDownList ddlApplicableOnFooter = gvChargeWsRanges.FooterRow.FindControl("ddlApplicableOnFooter") as DropDownList;
            decimal dcMaxRange = 0;
            if (txtMaxRangefooter.Text.Trim() != "")
                dcMaxRange = Convert.ToDecimal(txtMaxRangefooter.Text.Trim());

            string Currency = "";
            if (ddlCurrencyfooter.SelectedValue == "0")
                Currency = "Rs.";
            else
                Currency = "$";
            int result = QuotationOperations.AddChargeWsRanges(ChargeId, Currency, Convert.ToDecimal(txtMinRangefooter.Text.Trim()),
                                         dcMaxRange, Convert.ToInt32(ddlApplicableOnFooter.SelectedValue), LoggedInUser.glUserId);
            if (result == 1)
            {
                lblresult.Text = "Range Added Successfully..!!";
                lblresult.CssClass = "success";
                FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
                ModalPopupExtender3.Show();
            }
            else
            {
                lblresult.Text = "Range for charge already exist!";
                lblresult.CssClass = "warning";
            }

        }
    }

    protected void gvChargeWsRanges_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvChargeWsRanges.EditIndex = e.NewEditIndex;
        FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
        lblresult.Text = "";
        lblresult.Visible = false;
        ModalPopupExtender3.Show();
    }

    protected void gvChargeWsRanges_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvChargeWsRanges.EditIndex = -1;
        FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
        lblresult.Text = "";
        lblresult.Visible = false;
        ModalPopupExtender3.Show();
    }

    protected void gvChargeWsRanges_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ChargeId = 0;
        int Lid = Convert.ToInt32(gvChargeWsRanges.DataKeys[e.RowIndex].Value.ToString());
        ChargeId = Convert.ToInt32(ViewState["ChargeId"].ToString());
        DropDownList ddlCurrencyfooter = (DropDownList)gvChargeWsRanges.Rows[e.RowIndex].FindControl("ddlCurrency");
        TextBox txtMinRangefooter = (TextBox)gvChargeWsRanges.Rows[e.RowIndex].FindControl("txtMinRange");
        TextBox txtMaxRangefooter = (TextBox)gvChargeWsRanges.Rows[e.RowIndex].FindControl("txtMaxRange");
        DropDownList ddlApplicableOn = (DropDownList)gvChargeWsRanges.Rows[e.RowIndex].FindControl("ddlApplicableOn");
        decimal dcMaxRange = 0;
        if (txtMaxRangefooter.Text.Trim() != "")
            dcMaxRange = Convert.ToDecimal(txtMaxRangefooter.Text.Trim());

        string Currency = "";
        if (ddlCurrencyfooter.SelectedValue == "0")
            Currency = "Rs.";
        else
            Currency = "$";
        int result = QuotationOperations.UpdateChargeWsRanges(Lid, Currency, Convert.ToDecimal(txtMinRangefooter.Text.Trim()),
                                     dcMaxRange, Convert.ToInt32(ddlApplicableOn.SelectedValue), LoggedInUser.glUserId);
        if (result == 1)
        {
            lblresult.Text = "Range Updated Successfully..!!";
            lblresult.CssClass = "success";
            gvChargeWsRanges.EditIndex = -1;
            FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
            ModalPopupExtender3.Show();
        }
        else
        {
            lblresult.Text = "Range for charge does not exist!";
            lblresult.CssClass = "warning";
            ModalPopupExtender3.Show();
        }
    }

    protected void gvChargeWsRanges_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        int lid = Convert.ToInt32(gvChargeWsRanges.DataKeys[e.RowIndex].Values["lid"].ToString());
        int result = QuotationOperations.DeleteChargeWsRanges(lid, LoggedInUser.glUserId);
        if (result == 1)
        {
            lblresult.Text = "Range Deleted Successfully!";
            lblresult.CssClass = "success";
            FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
            ModalPopupExtender3.Show();
        }
        else
        {
            lblresult.Text = "Range for charge does not exist!";
            lblresult.CssClass = "warning";
            ModalPopupExtender3.Show();
        }
    }

    protected void gvChargeWsRanges_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChargeWsRanges.PageIndex = e.NewPageIndex;
        FillChargeWsRanges(Convert.ToInt32(ViewState["ChargeId"]));
        ModalPopupExtender3.Show();
    }

    protected void FillChargeWsRanges(int ChargeId)
    {
        DataSet ds = new DataSet();
        ds = QuotationOperations.GetChargeWsRangeDetails(Convert.ToInt32(ChargeId));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvChargeWsRanges.DataSource = ds;
            gvChargeWsRanges.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvChargeWsRanges.DataSource = ds;
            gvChargeWsRanges.DataBind();
            int columncount = gvChargeWsRanges.Rows[0].Cells.Count;
            gvChargeWsRanges.Rows[0].Cells.Clear();
            gvChargeWsRanges.Rows[0].Cells.Add(new TableCell());
            gvChargeWsRanges.Rows[0].Cells[0].ColumnSpan = columncount;
            gvChargeWsRanges.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    //protected void btnAddPort_Cick(object sender, EventArgs e)
    //{
    //    int Result = -123;
    //    TextBox cboPortName = (TextBox)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$ddlCurrency");

    //    lblresult.Visible = true;
    //    if (PortId > 0 && BranchId > 0)
    //    {
    //        Result = DBOperations.AddBranchPort(BranchId, PortId, LoggedInUser.glUserId);

    //        if (Result == 0)
    //        {
    //            lblresult.Text = "Port Added Successfully";
    //            lblresult.CssClass = "success";
    //            cboPortName.Text = String.Empty;
    //            cboPortId.Value = "0";

    //            SqlDataSource cboDataSourcePort = (SqlDataSource)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$DataSourcePort");
    //            cboDataSourcePort.SelectParameters[0].DefaultValue = BranchId.ToString();

    //            GridView cboGridViewPort = (GridView)this.FindControl("ctl00$ContentPlaceHolder1$FormView1$gvPort");
    //            cboGridViewPort.DataBind();
    //        }
    //        else if (Result == 1)
    //        {
    //            lblresult.Text = "System Error! Please Try After Sometime";
    //            lblresult.CssClass = "errorMsg";
    //        }
    //        else if (Result == 2)
    //        {
    //            lblresult.Text = "Port Already Exist!";
    //            lblresult.CssClass = "errorMsg";
    //        }
    //    }
    //    else
    //    {
    //        // Error
    //        lblresult.Text = "Please Select Port Name From List!";
    //        lblresult.CssClass = "errorMsg";
    //    }
    //}

    #endregion

    #region ExportData

    protected void lnkExportRangeDetail_Click(object sender, EventArgs e)
    {
        string strFileName = "ChargesRange_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void ExportFunction(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvChargeWsRanges.AllowPaging = false;
        gvChargeWsRanges.AllowSorting = false;
        gvChargeWsRanges.Columns[1].Visible = false;
        gvChargeWsRanges.Columns[4].Visible = false;
        gvChargeWsRanges.Caption = "Charge Wise Range Details On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        ddlSearchForCatg_OnSelectedIndexChanged(null, EventArgs.Empty);
        gvQuotationCharge.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}