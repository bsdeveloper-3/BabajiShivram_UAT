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

public partial class Master_ShippingDetails : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(FormView1);

        if(Session["SCode"] == null)
        {
            Response.Redirect("ShippingMaster.aspx");
        }
        if (!IsPostBack)
        {
            if(Session["SCode"] != null) 
            {
                //Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                //lblTitle.Text = "Shipping Company Detail";                
                string strSCode = Session["SCode"].ToString();
                FillDetails(strSCode);
            }
        }        
    }

    protected void FillDetails(string SCode)
    {
        DataSet ds = new DataSet();
        ds = DBOperations.GetShippingDetailsByCode(SCode);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvShippingDetail.DataSource = ds;
                gvShippingDetail.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvShippingDetail.DataSource = ds;
                gvShippingDetail.DataBind();
                int columncount = gvShippingDetail.Rows[0].Cells.Count;
                gvShippingDetail.Rows[0].Cells.Clear();
                gvShippingDetail.Rows[0].Cells.Add(new TableCell());
                gvShippingDetail.Rows[0].Cells[0].ColumnSpan = columncount;
                gvShippingDetail.Rows[0].Cells[0].Text = "No Records Found!";
               // gvShippingDetail.Rows[0].Cells[1].Text = "No Records Found!";
            }
        }
    }

    protected void gvShippingDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        string strSCode = Session["SCode"].ToString();
        lblresult.Visible = false;
        lblresult.Text = "";
        gvShippingDetail.EditIndex = e.NewEditIndex;
        FillDetails(strSCode);
        lblresult.Text = "";

    }

    protected void gvShippingDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        string strSCode = Session["SCode"].ToString();
        lblresult.Visible = false;
        lblresult.Text = "";
        gvShippingDetail.EditIndex = -1;

        FillDetails(strSCode);
        lblresult.Text = "";

    }

    protected void gvShippingDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string strSCode = Session["SCode"].ToString();
        gvShippingDetail.PageIndex = e.NewPageIndex;

        gvShippingDetail.DataBind();
        FillDetails(strSCode);
    }
    protected void gvShippingDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblresult.Visible = true;
        string strSCode = Session["SCode"].ToString();
        int lid = Convert.ToInt32(gvShippingDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtPName = (TextBox)gvShippingDetail.Rows[e.RowIndex].FindControl("txtPName");
        TextBox txtEmailId = (TextBox)gvShippingDetail.Rows[e.RowIndex].FindControl("txtEmailId");
        TextBox txtMobNo = (TextBox)gvShippingDetail.Rows[e.RowIndex].FindControl("txtMobNo");
        TextBox txtContactNo = (TextBox)gvShippingDetail.Rows[e.RowIndex].FindControl("txtContactNo");

        if (txtPName.Text.Trim() != "")
        {
            int result = DBOperations.UpdateShipContactDetails(lid, txtPName.Text.Trim(), txtEmailId.Text.Trim(), txtMobNo.Text.Trim(), txtContactNo.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lblresult.Text = "Record Updated Successfully.";

                lblresult.CssClass = "success";

                gvShippingDetail.EditIndex = -1;

                FillDetails(strSCode);
            }
            else if (result == 1)
            {
                lblresult.Text = "System Error! Please Try After Sometime!";
                lblresult.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblresult.Text = "Record Already Exist!";
                lblresult.CssClass = "errorMsg";
            }
        }
        else
        {
            lblresult.Text = " Please Enter Contact Name";
            lblresult.CssClass = "errorMsg";
        }

    }
    protected void gvShippingDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblresult.Visible = true;

        if (e.CommandName == "Insert")
        {
            string strSCode = Session["SCode"].ToString();
            
            TextBox lID = gvShippingDetail.FooterRow.FindControl("txtlidfooter") as TextBox;            

            TextBox txtPName = (TextBox)gvShippingDetail.FooterRow.FindControl("txtPNamefooter");
            TextBox txtEmailId = (TextBox)gvShippingDetail.FooterRow.FindControl("txtEmailIdfooter");
            TextBox txtMobNo = (TextBox)gvShippingDetail.FooterRow.FindControl("txtMobNofooter");
            TextBox txtContactNo = (TextBox)gvShippingDetail.FooterRow.FindControl("txtContactNofooter");



            if (txtPName.Text.Trim() != "")
            {
                int result = DBOperations.AddShipContactDetails(strSCode, txtPName.Text.Trim(), txtEmailId.Text.Trim(), txtMobNo.Text.Trim(), txtContactNo.Text.Trim(), loggedinuser.glUserId);

                if (result == 0)
                {
                    lblresult.Text = "Record Added Successfully.";
                    lblresult.CssClass = "success";

                    FillDetails(strSCode);

                }
                else if (result == 1)
                {
                    lblresult.Text = "System Error! Please Try After Sometime!";
                    lblresult.CssClass = "errorMsg";
                }
                else if (result == 2)
                {
                    lblresult.Text = "Destination Already Exist!";
                    lblresult.CssClass = "errorMsg";
                }
            }
            else
            {
                lblresult.Text = " Please Enter Contact Name!";
                lblresult.CssClass = "errorMsg";
            }
        }
    }

    protected void gvShippingDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblresult.Visible = true;
        
        string strScode = Session["SCode"].ToString();

        int Did = Convert.ToInt32(gvShippingDetail.DataKeys[e.RowIndex].Value.ToString());

        int lid = Convert.ToInt32(gvShippingDetail.DataKeys[e.RowIndex].Values["lid"].ToString());

        int result = DBOperations.DeleteShippingDetails(lid, loggedinuser.glUserId);

        if (result == 0)
        {
            lblresult.Text = "Destination Deleted Duccessfully.";
            lblresult.CssClass = "success";
            FillDetails(strScode);
        }
        else if (result == 1)
        {
            lblresult.Text = "System Error! Please Try After Sometime!";
            lblresult.CssClass = "errorMsg";
        }
    }

    //protected void FormView1_ItemUpdated(object sender, GridViewUpdateEventArgs e)
    //{        
    //}

    protected void btnUpdateButton_Click(object sender, EventArgs e)
    {
        lblresult.Visible = true;
        string strSCode = Session["SCode"].ToString();

        HiddenField hdnlid = (HiddenField)FormView1.FindControl("hdnlid");
        TextBox txtUpdCustomerName = (TextBox)FormView1.FindControl("txtUpdCustomerName");
        TextBox txtUpdAddress = (TextBox)FormView1.FindControl("txtUpdAddress");
        TextBox txtUpdShippingLineCode = (TextBox)FormView1.FindControl("txtShippingLineCode");

        string strlid = hdnlid.Value;

        int lid = Convert.ToInt32(strlid);

        if (txtUpdShippingLineCode.Text.Trim().Length != 4 )
        {
            lberror.Text = "Plese Enter 4 Char Shipping Line Code!";
            lberror.CssClass = "errorMsg";
        }
        else if (txtUpdCustomerName.Text.Trim().Length >10)
        {
            int result = DBOperations.UpdateShipMaster(lid, txtUpdCustomerName.Text.Trim(), txtUpdShippingLineCode.Text.Trim(), txtUpdAddress.Text.Trim(), loggedinuser.glUserId);

            if (result == 0)
            {
                lberror.Text = "Record Updated Successfully.";
                lberror.CssClass = "success";
                //  FormView1.ChangeMode(read)
            }
            else if (result == 1)
            {
                lberror.Text = "System Error! Please Try After Sometime!";
                lberror.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lberror.Text = "Record Already Exist!";
                lberror.CssClass = "errorMsg";
            }
        }
        else
        {
            lberror.Text = " Please Enter Shipping Line Name";
            lberror.CssClass = "errorMsg";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Master/ShippingMaster.aspx");
    }
}