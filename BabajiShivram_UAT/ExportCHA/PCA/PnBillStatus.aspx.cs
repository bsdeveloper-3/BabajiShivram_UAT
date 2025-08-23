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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using AjaxControlToolkit;

public partial class PCA_PnBillStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("BillStatus.aspx");
        }
        else if (!IsPostBack)
        {
            JobDetailMS(Convert.ToInt32(Session["JobId"]));
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Job Detail";

        }
    }

    private void JobDetailMS(int JobId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        string strPreAlertId = "0";
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Job Detail
        DataSet dsJobDetail = CMOperations.GetJobDetail(JobId);


        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Response.Redirect("BillStatus.aspx");
            Session["JobId"] = null;
        }
        else
        {
            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();
        }

    }

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("BillStatus.aspx");
    }

    #region Job Detail TabPanel

    protected void ddlCustomerMS_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCustomerMS = (DropDownList)FVJobDetail.FindControl("ddlCustomerMS");
        DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");

        if (ddlCustomerMS.SelectedValue != "0")
        {
            DBOperations.FillCustomerDivision(ddDivision, Convert.ToInt32(ddlCustomerMS.SelectedValue));
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddDivision.Items.Clear();
            ddDivision.Items.Add(lstSelect);
        }
    }
    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");
        DropDownList ddPlant = (DropDownList)FVJobDetail.FindControl("ddPlant");

        if (ddDivision.SelectedValue != "0")
        {
            DBOperations.FillCustomerPlant(ddPlant, Convert.ToInt32(ddDivision.SelectedValue));
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }
    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {
        if (FVJobDetail.CurrentMode == FormViewMode.Edit)
        {
            HiddenField hdnBranchId = ((HiddenField)FVJobDetail.FindControl("hdnBranchId"));
            HiddenField hdnCustId = ((HiddenField)FVJobDetail.FindControl("hdnCustId"));
            HiddenField hdnDivisionId = ((HiddenField)FVJobDetail.FindControl("hdnDivisionId"));
            HiddenField hdnPlantId = ((HiddenField)FVJobDetail.FindControl("hdnPlantId"));
            DropDownList ddlCustomerMS = ((DropDownList)FVJobDetail.FindControl("ddlCustomerMS"));
            DropDownList ddDivision = ((DropDownList)FVJobDetail.FindControl("ddDivision"));
            DropDownList ddPlant = ((DropDownList)FVJobDetail.FindControl("ddPlant"));

            if (ddlCustomerMS != null)
            {
                ddlCustomerMS.SelectedValue = hdnCustId.Value;
                ddlCustomerMS_SelectedIndexChanged(null, EventArgs.Empty);
            }

            if (ddDivision != null)
            {
                ddDivision.SelectedValue = hdnDivisionId.Value;
                ddDivision_SelectedIndexChanged(null, EventArgs.Empty);
            }

            if (ddPlant != null)
            {
                ddPlant.SelectedValue = hdnPlantId.Value;
            }
        }
    }
    #endregion
}