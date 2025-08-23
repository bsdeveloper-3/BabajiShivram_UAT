using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
public partial class Reports_ViewExpensesDetail : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor hrefGoBack = (HtmlAnchor)Page.Master.FindControl("hrefGoBack");
            if (hrefGoBack != null)
            {
                if (Request.UrlReferrer != null)
                {
                    int startIndex = Request.UrlReferrer.AbsolutePath.LastIndexOf("/");

                    if (startIndex > 0)
                    {
                        string strReturnURL = Request.UrlReferrer.AbsolutePath;

                        hrefGoBack.HRef = strReturnURL;
                    }
                }
            }
        }

        if (Session["JobIdE"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Session Expired! Please try again');</script>", false);
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Expense Detail";

            int ExpenseId = Convert.ToInt32(Session["ExpenseId"]);
            DataSet dsExpense = DBOperations.GetExpenseDetailBylId(ExpenseId);

            fvExpense.DataSource = dsExpense;
            fvExpense.DataBind();

            // Get Job Detail
            JobDetailMS(Convert.ToInt32(Session["JobIdE"]));

        }
    }
    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Job Detail
        DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Session["JobIdE"] = null;
        }
        else
        {
            FVJobDetail.DataSource = dsJobDetail;
            FVJobDetail.DataBind();
        }        
    }
    protected void FVJobDetail_DataBound(object sender, EventArgs e)
    {
        if (FVJobDetail.CurrentMode == FormViewMode.ReadOnly)
        {
            Label lblInbondJobNo = (Label)FVJobDetail.FindControl("lblInbondJobNo");

            //if (Convert.ToInt32(hdnBoeTypeId.Value) == (Int32)EnumBOEType.Exbond)
            //{
            //    lblInbondJobNo.Visible = true;
            //}
        }

        DropDownList ddCustomer = (DropDownList)FVJobDetail.FindControl("ddCustomer");
        HiddenField hdnCustomerId = (HiddenField)FVJobDetail.FindControl("hdnCustomerId");

        DropDownList ddConsignee = (DropDownList)FVJobDetail.FindControl("ddConsignee");
        HiddenField hdnConsigneeId = (HiddenField)FVJobDetail.FindControl("hdnConsigneeId");

        DropDownList ddDivision = (DropDownList)FVJobDetail.FindControl("ddDivision");
        HiddenField hdnDivision = (HiddenField)FVJobDetail.FindControl("hdnDivision");

        DropDownList ddPlant = (DropDownList)FVJobDetail.FindControl("ddPlant");
        HiddenField hdnPlant = (HiddenField)FVJobDetail.FindControl("hdnPlant");

        DropDownList ddMode = (DropDownList)FVJobDetail.FindControl("ddMode");
        //HiddenField hdnMode = (HiddenField)FVJobDetail.FindControl("hdnMode");

        DropDownList ddPort = (DropDownList)FVJobDetail.FindControl("ddPort");
        HiddenField hdnPort = (HiddenField)FVJobDetail.FindControl("hdnPort");

        DropDownList ddBabajiBranch = (DropDownList)FVJobDetail.FindControl("ddBabajiBranch");
        HiddenField hdnBabajiBranch = (HiddenField)FVJobDetail.FindControl("hdnBabajiBranch");

        // Duty Details
        RadioButtonList rdlDutyRequired = (RadioButtonList)FVJobDetail.FindControl("rdlDutyRequired");
        HiddenField hdnDutyRequired = (HiddenField)FVJobDetail.FindControl("hdnDutyRequired");
        if (hdnDutyRequired != null)
        {
            int DutyStatusId = Convert.ToInt32(hdnDutyRequired.Value);
            if (DutyStatusId == 1)
            {
                rdlDutyRequired.SelectedValue = "1";//Duty Required No
            }
            else
            {
                rdlDutyRequired.SelectedValue = "2";//Duty Required Yes
            }
        }
        //END Duty Detail

        // Job Type Dropdown Fill
        DropDownList ddJobType = (DropDownList)FVJobDetail.FindControl("ddJobType");
        HiddenField hdnJobType = (HiddenField)FVJobDetail.FindControl("hdnJobType");
        if (ddJobType != null)
        {
            DBOperations.FillJobType(ddJobType);
            ddJobType.SelectedValue = hdnJobType.Value;
        }
        //End Job Type

        // Measurment Unit dropdwon Fill
        DropDownList ddMeasurmentUnit = (DropDownList)FVJobDetail.FindControl("ddMeasurmentUnit");
        HiddenField hdnMeasurmentUnit = (HiddenField)FVJobDetail.FindControl("hdnMeasurmentUnit");
        if (ddMeasurmentUnit != null)
        {
            DBOperations.FillPackageType(ddMeasurmentUnit);
            ddMeasurmentUnit.SelectedValue = hdnMeasurmentUnit.Value;
        }
        //END Measurment Unit


        // IncoTerms dropdwon Fill
        DropDownList ddIncoTerms = (DropDownList)FVJobDetail.FindControl("ddIncoTerms");
        HiddenField hdnIncoTerms = (HiddenField)FVJobDetail.FindControl("hdnIncoTerms");
        if (ddIncoTerms != null)
        {
            DBOperations.FillIncoTermDetails(ddIncoTerms);
            ddIncoTerms.SelectedValue = hdnIncoTerms.Value;
        }
        //END IncoTerms

        // Delivery Type dropdwon Fill

        DropDownList ddDeliveryType = (DropDownList)FVJobDetail.FindControl("ddDeliveryType");
        HiddenField hdnDeliveryType = (HiddenField)FVJobDetail.FindControl("hdnDeliveryType");
        if (ddDeliveryType != null)
        {
            int intTransMode = Convert.ToInt32(hdnMode.Value);

            if (intTransMode == (int)TransMode.Air)
            {
                ddDeliveryType.Visible = false;
            }
            else
            {
                DBOperations.FillDeliveryType(ddDeliveryType);
                ddDeliveryType.SelectedValue = hdnDeliveryType.Value;
            }
        }
        //END Delivery Type 

        //Priority DropDown Fill
        DropDownList ddPriority = (DropDownList)FVJobDetail.FindControl("ddPriority");
        HiddenField hdnPriority = (HiddenField)FVJobDetail.FindControl("hdnPriority");
        if (ddPriority != null)
        {
            DBOperations.FillPriority(ddPriority);
            ddPriority.SelectedValue = hdnPriority.Value;
        }
        //END Priority 


        if (ddCustomer != null)
        {
            DBOperations.FillCustomer(ddCustomer);
            ddCustomer.SelectedValue = hdnCustomerId.Value;
            int CustomerId = Convert.ToInt32(hdnCustomerId.Value);

            if (CustomerId > 0)
            {
                DBOperations.FillConsignee(ddConsignee, CustomerId);
                ddConsignee.SelectedValue = hdnConsigneeId.Value;

                DBOperations.FillCustomerDivision(ddDivision, CustomerId);
                ddDivision.SelectedValue = hdnDivision.Value;
                int DivisionId = Convert.ToInt32(hdnDivision.Value);

                if (DivisionId > 0)
                {
                    DBOperations.FillCustomerPlant(ddPlant, DivisionId);
                    ddPlant.SelectedValue = hdnPlant.Value;

                }
                else
                {
                    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                    ddPlant.Items.Clear();
                    ddPlant.Items.Add(lstSelect);
                }

            }
            else
            {
                System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");

                ddConsignee.Items.Clear();
                ddConsignee.Items.Add(lstSelect);

                ddDivision.Items.Clear();
                ddDivision.Items.Add(lstSelect);


            }//End CustomerIf

        }

        if (ddMode != null)
        {
            //DBOperations.FillMode(ddMode);

            ddMode.SelectedValue = hdnMode.Value;
            int Mode = Convert.ToInt32(hdnMode.Value);


            if (Mode > 0)
            {
                DBOperations.FillPort(ddPort, Mode);
                ddPort.SelectedValue = hdnPort.Value;
                int Port = Convert.ToInt32(hdnPort.Value);


                if (Port > 0)
                {
                    DBOperations.FillBranchByPort(ddBabajiBranch, Port);
                    ddBabajiBranch.SelectedValue = hdnBabajiBranch.Value;
                    int BabajiBranch = Convert.ToInt32(hdnBabajiBranch.Value);
                }
                else
                {
                    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                    ddBabajiBranch.Items.Clear();
                    ddBabajiBranch.Items.Add(lstSelect);
                }
            }
            else
            {
                System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
                ddPort.Items.Clear();
                ddPort.Items.Add(lstSelect);
            }//End Mode If

        }

    }
    
}