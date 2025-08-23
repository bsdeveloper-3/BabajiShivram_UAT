using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_MiscJobCreation : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Job Detail";

        if (!IsPostBack)
        {
            DBOperations.FillUserBranch(ddlBranch, LoggedInUser.glUserId);
                        
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        string JobRefNo = lblJobNo.Text.Trim();
        int CustomerId = Convert.ToInt32(hdnCustId.Value);
        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);
        int PlantId = Convert.ToInt32(ddPlant.SelectedValue);

        int ModuleID = Convert.ToInt32(ddModule.SelectedValue);
        int BranchID = Convert.ToInt32(ddlBranch.SelectedValue);
        int ModeID = Convert.ToInt32(ddlTransMode.SelectedValue);
        int TypeID = Convert.ToInt32(ddlType.SelectedValue);

        string strConsigneeGSTN = "";//
        string strJobDescription = txtJobDescription.Text.Trim();
        if (ddConsigneeGSTN.SelectedIndex > 0)
        {
            strConsigneeGSTN = ddConsigneeGSTN.SelectedItem.Text;
        }

        int result = JobOperation.AddMiscJobDetail(ModuleID, BranchID, JobRefNo, ModeID, TypeID,
        CustomerId, DivisonId, PlantId, strConsigneeGSTN,
        strJobDescription, LoggedInUser.glUserId);

        if (result > 0)
        {
            lblError.Text = "Job Detail Successflly Created!";
            lblError.CssClass = "success";

        }
        else if (result == -2)
        {
            lblError.Text = "Job Number Already Created. Please Try  Again.";
            lblError.CssClass = "errorMsg";

        }
        else
        {
            lblError.Text = "System Error. Please try again later..!!";
            lblError.CssClass = "errorMsg";
        }
    }

    private void GenerateJobNo()
    {
        lblJobNo.Text = "";

        int ModuleID = Convert.ToInt32(ddModule.SelectedValue);
        int BranchID = Convert.ToInt32(ddlBranch.SelectedValue);
        int ModeID = Convert.ToInt32(ddlBranch.SelectedValue);
        int TypeID = Convert.ToInt32(ddlBranch.SelectedValue);

        if (ModuleID > 0 && BranchID > 0)
        {
            string Result = JobOperation.GetNextMiscJobNo(ModuleID, BranchID, ModeID, TypeID);

            if (Result != "")
            {
                lblJobNo.Text = Result;
            }
        }
    }
    protected void ddModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateJobNo();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateJobNo();
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        int CustomerId = Convert.ToInt32(hdnCustId.Value);

        if (CustomerId > 0)
        {
            // Fill Customer Division
            DBOperations.FillCustomerDivision(ddDivision, CustomerId);
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);

            // Fill Customer GST No

            System.Web.UI.WebControls.ListItem lstGST = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddConsigneeGSTN.Items.Clear();
            ddConsigneeGSTN.Items.Add(lstSelect);

            DBOperations.FillCustomerGSTN(ddConsigneeGSTN, CustomerId);

        }
    }

    protected void ddDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        int DivisonId = Convert.ToInt32(ddDivision.SelectedValue);

        if (DivisonId > 0)
        {
            DBOperations.FillCustomerPlant(ddPlant, DivisonId);
        }
        else
        {
            System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            ddPlant.Items.Clear();
            ddPlant.Items.Add(lstSelect);
        }
    }
}