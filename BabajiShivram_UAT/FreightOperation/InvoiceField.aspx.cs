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

public partial class FreightOperation_InvoiceField : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Session["SortExp"] = null;
            //Session["SortDir"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Invoice Line Item Master";

            lblresult.Visible = false;

            FieldDetails();
        }
    }

    protected void FieldDetails()
    {
        //string SortExp = "", SortDir = "";

        //if (Session["SortExp"] != null)
        //{
        //    SortExp = Session["SortExp"].ToString();

        //    SortDir = Session["SortDir"].ToString();
        //}

        DataSet ds = new DataSet();

        ds = DBOperations.GetInvoiceFieldDetails();

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv = ds.Tables[0].AsDataView();

            //if (SortExp != "")
            //{
            //    dv.Sort = SortExp +" " + SortDir;

            //}

            gvField.DataSource = dv;
            gvField.DataBind();
        }
        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvField.DataSource = ds;
            gvField.DataBind();
            int columncount = gvField.Rows[0].Cells.Count;
            gvField.Rows[0].Cells.Clear();
            gvField.Rows[0].Cells.Add(new TableCell());
            gvField.Rows[0].Cells[0].ColumnSpan = columncount;
            gvField.Rows[0].Cells[0].Text = "No Records Found!";
        }
    }

    protected void gvField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Edit) ||
                   (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
        {
            TextBox txtAirSACCode = (TextBox)e.Row.FindControl("txtAirSACCode");

            if (DataBinder.Eval(e.Row.DataItem, "AirSacId") != DBNull.Value)
            {
                int AirSacId = (int)DataBinder.Eval(e.Row.DataItem, "AirSacId");
                // txtSACCode.Attributes.Add("OnClick", "javascript:return imgDelNformDoc_OnClick('" + fuNForm.ClientID + "');");
                //Page.ClientScript.RegisterStartupScript(GetType(), "Key", "OnSacSelected();", true);
                if (AirSacId != 0)
                    hdnAirSacId.Value = AirSacId.ToString();
            }

            TextBox txtSeaSACCode = (TextBox)e.Row.FindControl("txtSeaSACCode");

            if (DataBinder.Eval(e.Row.DataItem, "SeaSacId") != DBNull.Value)
            {
                int SeaSacId = (int)DataBinder.Eval(e.Row.DataItem, "SeaSacId");
                // txtSACCode.Attributes.Add("OnClick", "javascript:return imgDelNformDoc_OnClick('" + fuNForm.ClientID + "');");
                //Page.ClientScript.RegisterStartupScript(GetType(), "Key", "OnSacSelected();", true);
                if (SeaSacId != 0)
                    hdnSeaSacId.Value = SeaSacId.ToString();
            }
        }
    }

    protected void gvField_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName.ToLower() == "insert")
        {
            TextBox txtFieldId              = gvField.FooterRow.FindControl("txtFieldIdFooter") as TextBox;
            TextBox txtFieldName            = gvField.FooterRow.FindControl("txtField_Namefooter") as TextBox;
            TextBox txtHeader               = gvField.FooterRow.FindControl("txtHeader_Namefooter") as TextBox;
            DropDownList ddFieldUnit        = gvField.FooterRow.FindControl("ddFieldUnitFooter") as DropDownList;
            DropDownList ddTaxApplicable    = gvField.FooterRow.FindControl("ddTaxApplicableFooter") as DropDownList;
            TextBox txtSACCodeFooter        = gvField.FooterRow.FindControl("txtSACCodeFooter") as TextBox;
            TextBox txtTaxRateFooter        = gvField.FooterRow.FindControl("txtTaxRateFooter") as TextBox;
            TextBox txtRemark               = gvField.FooterRow.FindControl("txtRemarkFooter") as TextBox;
            TextBox txtAirSACCodeFooter     = gvField.FooterRow.FindControl("txtAirSACCodeFooter") as TextBox;
            TextBox txtSeaSACCodeFooter     = gvField.FooterRow.FindControl("txtSeaSACCodeFooter") as TextBox;


            bool isTaxable = false;
            decimal dcTaxRate = 0;

            if (ddTaxApplicable.SelectedValue == "True")
                isTaxable = true;

            string strFiledName = txtFieldName.Text.Trim();
            string strHeader = txtHeader.Text.Trim();
            int FieldUnit = Convert.ToInt32(ddFieldUnit.SelectedValue);
            string strTaxSelected = ddTaxApplicable.SelectedValue;
            string strSACCode = "";// txtSACCodeFooter.Text.Trim();
            if (txtTaxRateFooter.Text.Trim() != "")
                dcTaxRate = Convert.ToDecimal(txtTaxRateFooter.Text.Trim());
            string strRemark = txtRemark.Text.Trim();

            int AirSacId = 0, SeaSacId = 0;
            if (txtAirSACCodeFooter.Text.Trim() != "" && hdnAirSacId.Value != "0" && hdnAirSacId.Value != "")
                AirSacId = Convert.ToInt32(hdnAirSacId.Value);
            if (txtSeaSACCodeFooter.Text.Trim() != "" && hdnSeaSacId.Value != "0" && hdnSeaSacId.Value != "")
                SeaSacId = Convert.ToInt32(hdnSeaSacId.Value);

            if (strFiledName != "" && strHeader != "" && FieldUnit != 0 && strTaxSelected != "0")
            {
                int result = DBOperations.AddInvoiceFieldMaster(strFiledName, strHeader, FieldUnit, isTaxable, strSACCode, dcTaxRate,
                    strRemark, LoggedInUser.glUserId, AirSacId, SeaSacId);

                if (result == 0)
                {
                    lblresult.Text = txtFieldName.Text.Trim() + " Field added successfully.";
                    lblresult.CssClass = "success";

                    FieldDetails();
                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Field Name Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }

            }//END_IF
            else
            {
                lblresult.CssClass = "errorMsg";
                lblresult.Text = " Please fill all the details!";
            }
        }
    }

    protected void gvField_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int FieldId                     = Convert.ToInt32(gvField.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtField_Name           = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtField_Name");
        TextBox txtHeader_Name          = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtHeader_Name");
        DropDownList ddFieldUnit        = (DropDownList)gvField.Rows[e.RowIndex].FindControl("ddFieldUnit");
        DropDownList ddTaxApplicable    = (DropDownList)gvField.Rows[e.RowIndex].FindControl("ddTaxApplicable");
        TextBox txtSACCode              = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtSACCode");
        TextBox txtTaxRate              = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtTaxRate");
        TextBox txtRemark               = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtRemark");
        TextBox txtAirSACCode           = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtAirSACCode");
        TextBox txtSeaSACCode           = (TextBox)gvField.Rows[e.RowIndex].FindControl("txtSeaSACCode");
        
        bool isTaxable = false;
        decimal dcTaxRate = 0;

        if (ddTaxApplicable.SelectedValue == "True")
            isTaxable = true;

        string strFiledName     = txtField_Name.Text.Trim();
        string strHeader        = txtHeader_Name.Text.Trim();
           int FieldUnit        = Convert.ToInt32(ddFieldUnit.SelectedValue);
        string strTaxSelected   = ddTaxApplicable.SelectedValue;
        if (txtTaxRate.Text.Trim() != "")
                dcTaxRate       = Convert.ToDecimal(txtTaxRate.Text.Trim());
        string strSACCode       = ""; //txtSACCode.Text.Trim();
        string strRemark        = txtRemark.Text.Trim();

        int AirSacId = 0, SeaSacId = 0;
        if (txtAirSACCode.Text.Trim() != "" && hdnAirSacId.Value != "0" && hdnAirSacId.Value != "")
            AirSacId = Convert.ToInt32(hdnAirSacId.Value);
        if (txtSeaSACCode.Text.Trim() != "" && hdnSeaSacId.Value != "0" && hdnSeaSacId.Value != "")
            SeaSacId = Convert.ToInt32(hdnSeaSacId.Value);

        if (txtField_Name.Text.Trim() != "" && ddFieldUnit.SelectedValue != "0")
        {
            int result = DBOperations.UpdateInvoiceFieldMaster(FieldId, strFiledName, strHeader, FieldUnit, isTaxable, strSACCode, dcTaxRate,
                strRemark, LoggedInUser.glUserId, AirSacId, SeaSacId);

            if (result == 0)
            {
                lblresult.CssClass = "success";

                lblresult.Text = txtField_Name.Text.Trim() + " Field Details Updated Successfully.";

                gvField.EditIndex = -1;

                FieldDetails();

            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime.";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Invoice Field Name Already Added!";
                lblresult.CssClass = "errorMsg";
            }
        }//END_IF
        else
        {
            lblresult.CssClass = "errorMsg";
            lblresult.Text = " Please fill all the details!";
        }
    }

    protected void gvField_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;

        int FieldId = Convert.ToInt32(gvField.DataKeys[e.RowIndex].Values["lId"].ToString());

        int result = DBOperations.DeleteInvoiceFieldMaster(FieldId, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Invoice Field Deleted Successfully!";
            lblresult.CssClass = "success";

            FieldDetails();

        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime.";
            lblresult.CssClass = "errorMsg";
        }
    }

    protected void gvField_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvField.EditIndex = e.NewEditIndex;

        FieldDetails();

        lblresult.Text = "";

        lblresult.Visible = false;
    }

    protected void gvField_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvField.EditIndex = -1;

        FieldDetails();
        lblresult.Text = "";
        lblresult.Visible = false;
    }

    protected void gvField_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvField.PageIndex = e.NewPageIndex;

        gvField.DataBind();
        FieldDetails();
    }

    protected void gvField_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Session["SortExp"] = e.SortExpression;
        //Session["SortDir"] = e.SortDirection;
        //gvField.DataBind();
        //FieldDetails();
    }
}
