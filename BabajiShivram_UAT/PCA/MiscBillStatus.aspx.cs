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
using Ionic.Zip;
using QueryStringEncryption;
using ClosedXML.Excel;


public partial class PCA_MiscBillStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    { 
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy1);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy2);
        ScriptManager1.RegisterPostBackControl(lnkInstructionCopy3);

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

                //hrefGoBack.HRef = "AccountExpense/InvoiceTracking.aspx";
            }
        }

        if (Session["JobId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Job Session Expired! Please try again');</script>", false);
            Response.Redirect("BillStatus.aspx");
        }
        else if (!IsPostBack)
        {
            JobDetailMS(Convert.ToInt32(Session["JobId"]));
            Get_BillingInstruction(Convert.ToInt32(Session["JobId"]));
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Job Detail";

        }
    }

    private void JobDetailMS(int JobId)
    {
        string strCustDocFolder = "", strJobFileDir = "";
        string strPreAlertId = "0";
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

        // Job Detail
        DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);


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

        // Job History
        DataView dvHistory = DBOperations.GetJobHistory(JobId);

        // Delivery Detail
        DataView dvJobDelivery = DBOperations.GetJobDetailForDelivery(JobId);

        // PCD Detail
        DataSet dsPCDDetail = DBOperations.GetPCDDetail(JobId);


    }

    #region billing instruction
    protected void Get_BillingInstruction(int JobId)
    {
        int strJobId = JobId;
        DataTable dtBillInstruction = new DataTable();
        dtBillInstruction = DBOperations.Get_BillingInstructionDetail(strJobId);
        if (dtBillInstruction.Rows.Count > 0)
        {
            //dvBillInstruction.Visible = false;
            //dvResult.Visible = true;
            // btnSaveInstruction.Visible = false;

            foreach (DataRow rw in dtBillInstruction.Rows)
            {
                //lblAgencyApply.Text = rw["AlliedAgencyApply"].ToString();
                //lblRefNo.Text = rw["JobRefNo"].ToString();
                if (rw["AlliedAgencyService"].ToString() == "") { lblAlliedAgencyService.Text = "-"; }
                else
                {
                    string args = rw["AlliedAgencyService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblAlliedAgencyService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                //lblAlliedAgencyService.Text = rw["AlliedAgencyService"].ToString(); }
                if (rw["AlliedAgencyRemark"].ToString() == "") { lblAlliedAgencyRemark.Text = "-"; }
                else { lblAlliedAgencyRemark.Text = rw["AlliedAgencyRemark"].ToString(); };

                if (rw["OtherService"].ToString() == "") { lblOtherService.Text = "-"; }
                else
                {
                    string args = rw["OtherService"].ToString();
                    string[] arg = args.Split(';');
                    for (int i = 0; arg.Length - 2 >= i; i++)
                    {
                        lblOtherService.Text += i + 1 + ". " + arg[i] + "\n";
                    }
                }
                if (rw["OtherServiceRemark"].ToString() == "") { lblOtherServiceRemark.Text = "-"; }
                else { lblOtherServiceRemark.Text = rw["OtherServiceRemark"].ToString(); }
                if (rw["InstructionCopy"].ToString() == "") { lnkInstructionCopy.Text = "-"; }
                else { lnkInstructionCopy.Text = rw["InstructionCopy"].ToString(); }
                if (rw["InstructionCopy1"].ToString() == "") { lnkInstructionCopy1.Text = "-"; }
                else { lnkInstructionCopy1.Text = rw["InstructionCopy1"].ToString(); }
                if (rw["InstructionCopy2"].ToString() == "") { lnkInstructionCopy2.Text = "-"; }
                else { lnkInstructionCopy2.Text = rw["InstructionCopy2"].ToString(); }
                if (rw["InstructionCopy3"].ToString() == "") { lnkInstructionCopy3.Text = "-"; }
                else { lnkInstructionCopy3.Text = rw["InstructionCopy3"].ToString(); }
                if (rw["Instruction"].ToString() == "") { lblInstruction.Text = "-"; }
                else { lblInstruction.Text = rw["Instruction"].ToString(); }
                if (rw["Instruction1"].ToString() == "") { lblInstruction1.Text = "-"; }
                else { lblInstruction1.Text = rw["Instruction1"].ToString(); }
                if (rw["Instruction2"].ToString() == "") { lblInstruction2.Text = "-"; }
                else { lblInstruction2.Text = rw["Instruction2"].ToString(); }
                if (rw["Instruction3"].ToString() == "") { lblInstruction3.Text = "-"; }
                else { lblInstruction3.Text = rw["Instruction3"].ToString(); }
                lblUserDate.Text = rw["Userdate"].ToString();
                lblUserId.Text = rw["sName"].ToString();
            }
            // ModalPopupInstruction.Show();
        }
        else
        {
            //dvResult.Visible = false;
        }

    }
    protected void lnkInstructionCopy_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy.Text);
    }

    protected void lnkInstructionCopy1_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy1.Text);
    }

    protected void lnkInstructionCopy2_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy2.Text);
    }

    protected void lnkInstructionCopy3_Click(object sender, EventArgs e)
    {
        DownloadDocument(lnkInstructionCopy3.Text);
    }

    private void DownloadDocument(string DocumentPath)
    {

        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
            ServerPath = ServerPath.Replace("PCA\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    protected void btnBackButton_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("BillStatus.aspx");
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
            //int intTransMode = Convert.ToInt32(hdnMode.Value);

            //if (intTransMode == (int)TransMode.Air)
            //{
            //    ddDeliveryType.Visible = false;
            //}
            //else
            //{
            //    DBOperations.FillDeliveryType(ddDeliveryType);
            //    ddDeliveryType.SelectedValue = hdnDeliveryType.Value;
            //}
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

            //ddMode.SelectedValue = hdnMode.Value;
            //int Mode = Convert.ToInt32(hdnMode.Value);


            //if (Mode > 0)
            //{
            //    DBOperations.FillPort(ddPort, Mode);
            //    ddPort.SelectedValue = hdnPort.Value;
            //    int Port = Convert.ToInt32(hdnPort.Value);


            //    if (Port > 0)
            //    {
            //        DBOperations.FillBranchByPort(ddBabajiBranch, Port);
            //        ddBabajiBranch.SelectedValue = hdnBabajiBranch.Value;
            //        int BabajiBranch = Convert.ToInt32(hdnBabajiBranch.Value);
            //    }
            //    else
            //    {
            //        System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            //        ddBabajiBranch.Items.Clear();
            //        ddBabajiBranch.Items.Add(lstSelect);
            //    }
            //}
            //else
            //{
            //    System.Web.UI.WebControls.ListItem lstSelect = new System.Web.UI.WebControls.ListItem("-Select-", "0");
            //    ddPort.Items.Clear();
            //    ddPort.Items.Add(lstSelect);
            //}//End Mode If

        }

    }

    
}