using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using QueryStringEncryption;

public partial class AccountExpense_JobViewFr : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(GridViewDocument);
        ScriptManager1.RegisterPostBackControl(gvInvoiceDocument);

        if (Session["JobIdV"] == null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Session Expired! Please try again');</script>", false);
        }
        else if (!IsPostBack)
        {
            JobDetailMS(Convert.ToInt32(Session["JobIdV"]));

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
        // DataSet dsJobDetail = DBOperations.GetJobDetail(JobId);
        DataSet dsJobDetail = DBOperations.GetOperationDetail(JobId);

        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Session["JobIdV"] = null;
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


    protected void gvJobExpDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "documents")
        {
            //LinkButton DocName = gvJobExpDetail.FindControl("PODDocPath") as LinkButton;
            string DocName = e.CommandArgument.ToString();
            if (DocName != "")
            {
                DownloadDoc(DocName);
            }
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\ExpenseUpload\\" + DocumentPath);
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

    private void DownloadDocument(string DocumentPath)
    {

        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
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

    protected void GridViewDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument2(DocPath);
        }
        else if (e.CommandName.ToLower() == "view")
        {
            string DocPath = e.CommandArgument.ToString();
            ViewDocument(DocPath);
        }
    }
    private void DownloadDocument2(string DocumentPath)
    {

        //DocumentPath =  DBOperations.GetDocumentPath(Convert.ToInt32(DocumentId));

        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            string strURL = "../ViewDoc.aspx?ref= " + DocumentPath;

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }

    #region Invoice Document Download

    protected void gvInvoiceDocument_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                DownloadDocumentInvoice(strDocpath);
            }
        }
        else if (e.CommandName.ToLower() == "view")
        {
            int DocumentId = Convert.ToInt32(e.CommandArgument.ToString());

            DataSet dsGetDocPath = AccountExpense.GetInvoiceDocument(0, DocumentId);

            if (dsGetDocPath.Tables.Count > 0)
            {
                String strDocpath = "";
                String strFilePath = dsGetDocPath.Tables[0].Rows[0]["FilePath"].ToString();

                String strFileName = dsGetDocPath.Tables[0].Rows[0]["FileName"].ToString();

                strDocpath = strFilePath + strFileName;
                ViewDocumentInvoice(strDocpath);
            }
        }
    }
    private void DownloadDocumentInvoice(string DocumentPath)
    {
        string ServerPath = "";// FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("../UploadFiles\\" + DocumentPath);
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

    private void ViewDocumentInvoice(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
}