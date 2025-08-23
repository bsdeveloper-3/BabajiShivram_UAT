using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PCA_BillReturnDetailBS : System.Web.UI.Page
{
    LoginClass LoggedIn = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(FVBillReturn);
        if (!IsPostBack)
        {
            if (Session["JobId"] != null)
            {
                BSJobBillReturn(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(Session["BJVlid"]));
            }
        }
    }

    public void BSJobBillReturn(int JobId , int BJVlid)
    {
        // Bill Return table lid
        //Label lblBillRetlid = (Label)FVBillReturn.FindControl("lblBillReturnLid");
        if (Session["BillRetLid"].ToString() != "")
        {
            DataSet dsBillReturn = DBOperations.GetBillReturnDetailBS(Convert.ToInt32(Session["BillRetLid"]), JobId, BJVlid, Convert.ToInt32(LoggedIn.glFinYearId));



            if (dsBillReturn.Tables[0].Rows.Count == 0)
            {
                Response.Redirect("BillReturnBS.aspx");
                Session["JobId"] = null;
                Session["BJVlid"] = null;
            }
            else
            {
                if (dsBillReturn.Tables[0].Rows.Count > 0)
                {
                    FVBillReturn.DataSource = dsBillReturn;
                    FVBillReturn.DataBind();
                }
            }
        }
    }

    protected void FVBillReturn_DataBound(object sender, EventArgs e)
    {
        if (FVBillReturn.CurrentMode == FormViewMode.ReadOnly)
        {
            Label lblReturnReason = (Label)FVBillReturn.FindControl("lblRetutnReason");            
            DropDownList ddlReason = (DropDownList)FVBillReturn.FindControl("ddlReason");
            Label lblReason = (Label)FVBillReturn.FindControl("lblReason");

            if (lblReturnReason != null)
            {
                ddlReason.SelectedValue = lblReturnReason.Text.Trim();
                lblReason.Text = ddlReason.SelectedItem.Text.Trim();
            }
        }
        else if(FVBillReturn.CurrentMode == FormViewMode.Edit)
        {
            Label lblReturnReason = (Label)FVBillReturn.FindControl("lblRetutnReason");
            DropDownList ddlReason = (DropDownList)FVBillReturn.FindControl("ddlReason");
            Label lblReason = (Label)FVBillReturn.FindControl("lblReason");

            if (lblReturnReason != null)
            {
                ddlReason.SelectedValue = lblReturnReason.Text.Trim();
                lblReason.Text = ddlReason.SelectedItem.Text.Trim();
            }
        }
            
    }
    protected void btnEditJob_Click(object sender, EventArgs e)
    {
        FVBillReturn.ChangeMode(FormViewMode.Edit);

        if (Session["JobId"] != null)
        {
            BSJobBillReturn(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(Session["BJVlid"]));
        }
    }
    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        string strReutrnUrl = ((Button)sender).CommandArgument.ToString();     

        Response.Redirect(strReutrnUrl);   //BillReturnBS.aspx
    }

    protected void btnUpdateJob_Click(object sender, EventArgs e)
    {
        int JobId, BJVId, BillRetLid = 0;//, JobType, NoOfPckgs, PackageType, IncoTerms, intLoadingPortId = 0,
        //ConsigneeId = 0, DivisionId = 0, PlantId = 0;

        string strRemark = "";

        DateTime dtChange = DateTime.MinValue, dtNewDispatch = DateTime.MinValue;           

        JobId = Convert.ToInt32(Session["JobId"]);
        BJVId = Convert.ToInt32(Session["BJVlid"]);
        BillRetLid = Convert.ToInt32(Session["BillRetLid"]);
        if (JobId > 0)
        {
            if (((TextBox)FVBillReturn.FindControl("txtchangeDate")).Text.Trim() != "")
            {
                dtChange = Commonfunctions.CDateTime(((TextBox)FVBillReturn.FindControl("txtchangeDate")).Text.Trim());
            }
            if (((TextBox)FVBillReturn.FindControl("txtDispatchDate")).Text.Trim() != "")
            {
                dtNewDispatch = Commonfunctions.CDateTime(((TextBox)FVBillReturn.FindControl("txtDispatchDate")).Text.Trim());
            }

            TextBox txtRemark = (TextBox)FVBillReturn.FindControl("txtRemark");
            strRemark = txtRemark.Text.Trim();


            int result = DBOperations.UpdateBillReturn(JobId, BJVId, BillRetLid, dtChange, dtNewDispatch, strRemark, LoggedIn.glUserId);

            if (result == 0)
            {
                lblError.Text = "Job Detail Updated Successfully !";
                lblError.CssClass = "success";
                FVBillReturn.ChangeMode(FormViewMode.ReadOnly);
                BSJobBillReturn(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(Session["BJVlid"]));
            }
            else if (result == 2)
            {
                lblError.Text = "Job Detail Not Found!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected void btnCancelButton_Click(object sender, EventArgs e)
    {
        FVBillReturn.ChangeMode(FormViewMode.ReadOnly);
        if (Session["JobId"] != null)
        {
            BSJobBillReturn(Convert.ToInt32(Session["JobId"]), Convert.ToInt32(Session["BJVlid"]));
        }
    }
}