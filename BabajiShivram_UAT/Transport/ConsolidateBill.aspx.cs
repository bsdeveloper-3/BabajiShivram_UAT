using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Ionic.Zip;

public partial class Transport_ConsolidateBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    int UnloadingJobCount = 0;
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["FreightAmt"] = 0;
        ViewState["VaraiAmt"] = 0;
        ViewState["DetentionAmt"] = 0;
        ViewState["EmptyContReturnAmt"] = 0;
        ViewState["TollCharges"] = 0;
        ViewState["OtherCharges"] = 0;
        ViewState["AdvanceAmt"] = 0;
        ViewState["TotalAmt"] = 0;

        if (Session["TRConsolidateId"] == null)
        {
            Response.Redirect("TransBill.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Detail - " + Session["TRRefNo"].ToString();

            lblFreightValidator.Visible = false;
            lblVaraiValidator.Visible = false;
            lblEmptyContValidator.Visible = false;
            lblDetentionValidator.Visible = false;
            lblTollChargesValidator.Visible = false;
            lblOtherChargesValidator.Visible = false;
            int TranRequestId = Convert.ToInt32(Session["TransReqId"]);
            gvTransportJobDetail.DataBind();
            txtBillSubmitDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtBillSubmitDate.Enabled = false;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["TransReqId"] = null;
        Session["TRConsolidateId"] = null;
        Response.Redirect("TransBill.aspx");
    }

    protected void btnBillSubmit_Click(object sender, EventArgs e)
    {
        bool IsValid = true;
        int TotalAmt = 0, ApprovedTotalAmt = 0, TransReqId = 0, TransporterID = 0, TransitDays = 0, ConsolidateID = 0;
        string BillNumber = "", BillAmount = "", DetentionAmount = "", VaraiAmount = "",
                EmptyContRcptCharges = "", TotalAmount = "", BillPersonName = "", TollCharges = "", OtherCharges = "",
                fileName = "", filePath = "";
        DateTime BillSubmitDate = DateTime.MinValue, BillDate = DateTime.MinValue;

        if (fuDocument != null && fuDocument.HasFile)
        {
            fileName = UploadFiles(fuDocument, "");
            filePath = "\\Transport\\" + fileName;
        }


        if (hdnTotalAmount.Value != "0" && hdnTotalAmount.Value != "")
        {
            ApprovedTotalAmt = Convert.ToInt32(hdnTotalAmount.Value);
        }

        TransReqId = Convert.ToInt32(Session["TransReqId"]);
        ConsolidateID = Convert.ToInt32(Session["TRConsolidateId"]);
        TransporterID = Convert.ToInt16(ddTransporter.SelectedValue);
        BillNumber = txtBillNo.Text.Trim();

        BillAmount = txtBillAmount.Text.Trim();
        VaraiAmount = txtVaraiExp.Text.Trim();
        DetentionAmount = txtDetentionAmount.Text.Trim();
        EmptyContRcptCharges = txtEmptyContCharges.Text.Trim();
        TollCharges = txtTollCharges.Text.Trim();
        OtherCharges = txtOtherCharges.Text.Trim();
        TotalAmount = txtTotalAmount.Text.Trim();
        BillPersonName = txtBillingEmpoyee.Text.Trim();

        if (hdnPageValid.Value == "1")
        {
            IsValid = false;
        }

        if (IsValid == false && txtJustification.Text.Trim() == "")
        {
            lblError.Text = "Please enter justification for the same.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            if (hdnTotalAmount.Value != "" && hdnTotalAmount.Value != "0")
            {
                decimal dcActualTotalAmount = Convert.ToDecimal(hdnTotalAmount.Value);
                decimal dcTotalAmount = Convert.ToDecimal(TotalAmount);
                if (dcActualTotalAmount > 0 && dcActualTotalAmount > 0)
                {
                    if (dcTotalAmount > dcActualTotalAmount)
                    {
                        IsValid = false;
                    }
                }
            }

            if (txtBillSubmitDate.Text.Trim() != "")
                BillSubmitDate = Commonfunctions.CDateTime(txtBillSubmitDate.Text.Trim());
            if (txtBillDate.Text.Trim() != "")
                BillDate = Commonfunctions.CDateTime(txtBillDate.Text.Trim());

            int result_BillID = DBOperations.AddTransBillDetail(TransReqId, TransporterID, TransitDays, BillSubmitDate, BillNumber, BillDate, BillAmount,
                        DetentionAmount, VaraiAmount, EmptyContRcptCharges, TollCharges, OtherCharges, TotalAmount, BillPersonName, IsValid, txtJustification.Text.Trim(), true, ConsolidateID,fileName,filePath, LoggedInUser.glUserId);

            if (result_BillID > 0)
            {
                if (IsValid == false)
                {
                    int result_ApprovalSend = DBOperations.AddTransApproveRejectBill(result_BillID, 0, 0, LoggedInUser.glUserId);
                    if (result_ApprovalSend == 0)
                    {
                        SendApprovalMail(TransReqId, ConsolidateID, TransporterID);
                    }
                }
                else
                {
                    int result_BillNonReceive = DBOperations.AddBillReceivedDetail(result_BillID, LoggedInUser.glUserId, 0, DateTime.Now, DateTime.MinValue, 0, "", DateTime.MinValue, "", DateTime.MinValue, LoggedInUser.glUserId);

                    lblError.Text = "Bill Detail Updated Successfully.";
                    lblError.CssClass = "success";

                    GridViewVehicle.EditIndex = -1;
                    GridViewBillDetail.DataBind();
                }
            }
            else if (result_BillID == -1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
            else if (result_BillID == -2)
            {
                lblError.Text = "Billing Detail Already Updated For Transporter - " + ddTransporter.SelectedItem.Text;
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
            }
        }
    }

    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\Transport\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + "Transport\\" + FilePath;
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
        {
            return "";
        }

    }

    protected string RandomString(int size)
    {

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    protected void btnCancelBill_Click(object sender, EventArgs e)
    {
        ddTransporter.DataBind();
        txtBillSubmitDate.Text = "";
        txtBillNo.Text = "";
        txtBillDate.Text = "";
        txtBillAmount.Text = "";
        txtDetentionAmount.Text = "";
        txtVaraiExp.Text = "";
        txtEmptyContCharges.Text = "";
        txtTotalAmount.Text = "";
        txtBillingEmpoyee.Text = "";
    }

    protected void txtBillAmount_TextChanged(object sender, EventArgs e)
    {
        lblFreightValidator.Visible = false;
        hdnPageValid.Value = "0";
        if (txtBillAmount.Text.Trim() != "")
        {
            decimal dcFreightAmt = 0, dcActualFreightAmt = 0;
            dcFreightAmt = Convert.ToDecimal(txtBillAmount.Text.Trim());
            dcActualFreightAmt = Convert.ToDecimal(hdnFreightAmt.Value);

            if (dcFreightAmt > 0 && dcActualFreightAmt > 0)
            {
                if (dcFreightAmt > dcActualFreightAmt)
                {
                    lblFreightValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtBillAmount.Focus();
    }

    protected void txtDetentionAmount_TextChanged(object sender, EventArgs e)
    {
        lblDetentionValidator.Visible = false;
        hdnPageValid.Value = "0";
        if (txtDetentionAmount.Text.Trim() != "")
        {
            decimal dcDetentionAmt = 0, dcActualDetentionAmt = 0;
            dcDetentionAmt = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
            dcActualDetentionAmt = Convert.ToDecimal(hdnDetentionAmt.Value);

            if (dcDetentionAmt > 0 && dcActualDetentionAmt > 0)
            {
                if (dcDetentionAmt > dcActualDetentionAmt)
                {
                    lblDetentionValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtDetentionAmount.Focus();
    }

    protected void txtVaraiExp_TextChanged(object sender, EventArgs e)
    {
        lblVaraiValidator.Visible = true;
        hdnPageValid.Value = "0";
        if (txtVaraiExp.Text.Trim() != "")
        {
            decimal dcVarai = 0, dcActualVaraiAmt = 0;
            dcVarai = Convert.ToDecimal(txtVaraiExp.Text.Trim());
            dcActualVaraiAmt = Convert.ToDecimal(hdnVaraiAmt.Value);

            if (dcVarai > 0 && dcActualVaraiAmt > 0)
            {
                if (dcVarai > dcActualVaraiAmt)
                {
                    lblVaraiValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtVaraiExp.Focus();
    }

    protected void txtEmptyContCharges_TextChanged(object sender, EventArgs e)
    {
        lblEmptyContValidator.Visible = false;
        hdnPageValid.Value = "0";
        if (txtEmptyContCharges.Text.Trim() != "")
        {
            decimal dcEmptyContAmt = 0, dcActualEmptyContAmt = 0;
            dcEmptyContAmt = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
            dcActualEmptyContAmt = Convert.ToDecimal(hdnEmptyContReturnAmt.Value);

            if (dcEmptyContAmt > 0 && dcActualEmptyContAmt > 0)
            {
                if (dcEmptyContAmt > dcActualEmptyContAmt)
                {
                    lblEmptyContValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtEmptyContCharges.Focus();
    }

    protected void GetTotalAmount()
    {
        decimal dcFreightAmt = 0, dcDetentionAmt = 0, dcVaraiExpense = 0, dcEmptyContCharges = 0, dcAdvanceAmt = 0, dcTollCharges = 0, dcOtherCharges = 0;

        if (hdnAdvanceAmt.Value != "" && hdnAdvanceAmt.Value != "0")
            dcAdvanceAmt = Convert.ToDecimal(hdnAdvanceAmt.Value);
        if (txtBillAmount.Text.Trim() != "")
            dcFreightAmt = Convert.ToDecimal(txtBillAmount.Text.Trim());
        if (txtDetentionAmount.Text.Trim() != "")
            dcDetentionAmt = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
        if (txtVaraiExp.Text.Trim() != "")
            dcVaraiExpense = Convert.ToDecimal(txtVaraiExp.Text.Trim());
        if (txtEmptyContCharges.Text.Trim() != "")
            dcEmptyContCharges = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
        if (txtTollCharges.Text.Trim() != "")
            dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
        if (txtOtherCharges.Text.Trim() != "")
            dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());

        txtTotalAmount.Text = Convert.ToDecimal((dcFreightAmt + dcDetentionAmt + dcVaraiExpense + dcEmptyContCharges + dcTollCharges + dcOtherCharges) - (dcAdvanceAmt)).ToString();
        txtTotalAmount.Enabled = false;
    }

    protected void SendApprovalMail(int TransReqId, int ConsolidateID, int TransporterId)
    {
        int TransBillId = 0;
        bool bEmailSuccess = false;
        StringBuilder strbuilder = new StringBuilder();
        StringBuilder strbuilderBilling = new StringBuilder();

        if (TransReqId > 0)
        {
            string strCustomerEmail = "", strCCEmail = "", strSubject = "", MessageBody = "", EmailContent = "",
                    strTRRefNo = "", strTruckRequestDate = "", strJobNo = "", strCustName = "", strVehiclePlaceDate = "", strVehicleRequired = "",
                    strLocation = "", strDest = "", strDimension = "", strGrossWT = "", strCont20 = "", strCont40 = "", strDispatch_Value = "", strDelExportType_Value = "", strDelExportType_Title = "";
            strSubject = "Transport Bill Approval for Consolidate Jobs";

            #region Transport Job details
            DataSet dsGetJobDetails = DBOperations.GetConsolidateJobDetail(ConsolidateID);
            if (dsGetJobDetails != null && dsGetJobDetails.Tables.Count > 0)
            {
                strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>TR Ref No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Job No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Customer</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle Place Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>From</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Destination</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>No Of Pkgs</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Gross Weight</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Cont 20</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Cont 40</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Planning Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Requested By</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Request Date</th></tr>");

                for (int i = 0; i < dsGetJobDetails.Tables[0].Rows.Count; i++)
                {
                    if (dsGetJobDetails.Tables[0].Rows[i]["ConsolidateJobId"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["TRRefNo"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["JobRefNo"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["CustName"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["VehiclePlaceDate"]).ToString("dd/MM/yyyy") + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["LocationFrom"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["Destination"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["NoOfPkgs"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["GrossWeight"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["Count20"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["Count40"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["PlanningDate"]).ToString("dd/MM/yyyy") + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["LRNo"].ToString() + "</th>");
                        if (dsGetJobDetails.Tables[0].Rows[i]["LRDate"] != DBNull.Value)
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["LRDate"]).ToString("dd/MM/yyyy") + "</th>");
                        else
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'></th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["BabajiChallanNo"].ToString() + "</th>");
                        if (dsGetJobDetails.Tables[0].Rows[i]["BabajiChallanDate"] != DBNull.Value)
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["BabajiChallanDate"]).ToString("dd/MM/yyyy") + "</th>");
                        else
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'></th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["RequestedBy"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["RequestedDate"]).ToString("dd/MM/yyyy") + "</th></tr>");
                    }
                }
                strbuilder = strbuilder.Append("</table><BR>");
            }
            #endregion

            #region Get Vehicle Bill details
            DataSet dsGetBillDetails = DBOperations.GetBillVehicleDetail(TransReqId);
            if (dsGetBillDetails != null && dsGetBillDetails.Tables.Count > 0)
            {
                strbuilder = strbuilder.Append("<BR><table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Transporter</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle No</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle Type</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Rate</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Advance (%)</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Advance Amt</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Detention Amt</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Varai Exp</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Empty Cont Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Toll Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Other Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Total</th></tr>");

                for (int i = 0; i < dsGetBillDetails.Tables[0].Rows.Count; i++)
                {
                    if (dsGetBillDetails.Tables[0].Rows[i]["lid"] != DBNull.Value)
                    {
                        strbuilder = strbuilder.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Transporter"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VehicleNo"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VehicleType"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Rate"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["Advance"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["AdvanceAmount"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["DetentionAmount"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["VaraiExpense"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["EmptyContRecptCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["TollCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["OtherCharges"].ToString() + "</th>");
                        strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetBillDetails.Tables[0].Rows[i]["TotalAmount"].ToString() + "</th></tr>");
                    }
                }
                strbuilder = strbuilder.Append("</table><BR>");
            }
            #endregion

            #region Bill details
            DataSet dsGetTransBillDetails = DBOperations.GetTransBillDetail(TransReqId, TransporterId);
            if (dsGetTransBillDetails != null && dsGetTransBillDetails.Tables.Count > 0)
            {
                TransBillId = Convert.ToInt32(dsGetTransBillDetails.Tables[0].Rows[0]["lid"].ToString());
                strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Number</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Submit Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Date</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Amount</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Detention Amount</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Varai Amount</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Empty Cont Rcpt Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Toll Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Other Charges</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Total Amount</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Justification</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Bill Person</th></tr>");

                strbuilder = strbuilder.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillNumber"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetTransBillDetails.Tables[0].Rows[0]["BillSubmitDate"]).ToString("dd/MM/yyyy") + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetTransBillDetails.Tables[0].Rows[0]["BillDate"]).ToString("dd/MM/yyyy") + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillAmount"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["DetentionAmount"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["VaraiAmount"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["EmptyContRcptCharges"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["TollCharges"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["OtherCharges"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["TotalAmount"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["Justification"].ToString() + "</th>");
                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetTransBillDetails.Tables[0].Rows[0]["BillPersonName"].ToString() + "</th></tr>");
                strbuilder = strbuilder.Append("</table>");
            }
            #endregion

            List<string> lstFileDoc = new List<string>();
            MessageBody = "Dear Sir, <BR><BR>Please find consolidated transport bill detail followed by vehicle rates and bill amount.<BR><BR>" +
                strbuilder + " <BR><BR>Kindly <strong><a href='http://192.168.6.107/TRBillApprove.aspx?o=1&t=" + TransporterId + "&p=" + TransBillId + "'> Approve</a></strong>" + "  or  " +
                "<strong><a href='http://192.168.6.107/TRBillApprove.aspx?o=2&t=" + TransporterId + "&p=" + TransBillId + "'>Reject</a></strong> the bill here. <BR><BR>";
            MessageBody = MessageBody + "<BR><BR>Thank You<BR><b>This is system generated mail, please do not reply to this message via e-mail.</b>";

            bEmailSuccess = EMail.SendMailBCC("ashwani.singh@babajishivram.com", "ashwani.singh@babajishivram.com", "", "javed.shaikh@babajishivram.com", strSubject, MessageBody, "");
            //bEmailSuccess = EMail.SendMailMultiAttach("kivisha.jain@babajishivram.com", "javed.shaikh@babajishivram.com", "", strSubject, MessageBody, lstFileDoc);
            if (bEmailSuccess == true)
            {
                lblError.Text = "Amount is more than approved amount. Successfully sent mail for bill approval.";
                lblError.CssClass = "success";
            }
        }

    }

    protected void gvTransportJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        btnBillSubmit.Visible = true;
        btnCancelBill.Visible = true;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "UnloadingDate") == DBNull.Value && DataBinder.Eval(e.Row.DataItem, "ReportingDate") == DBNull.Value)
            {
                UnloadingJobCount++;
            }
        }

        if (UnloadingJobCount > 0)
        {
            btnBillSubmit.Visible = false;
            btnCancelBill.Visible = false;
            lblError.Text = "Please complete movement for all consolidated jobs in order to proceed with billing.";
            lblError.CssClass = "errorMsg";
        }
    }

    protected void txtTollCharges_TextChanged(object sender, EventArgs e)
    {
        lblTollChargesValidator.Visible = false;
        if (txtTollCharges.Text.Trim() != "")
        {
            decimal dcTollCharges = 0, dcActualTollCharges = 0;
            dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
            dcActualTollCharges = Convert.ToDecimal(hdnTollCharges.Value);

            if (dcTollCharges > 0 && dcActualTollCharges > 0)
            {
                if (dcTollCharges > dcActualTollCharges)
                {
                    lblTollChargesValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtTollCharges.Focus();
    }

    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        lblOtherChargesValidator.Visible = false;
        if (txtOtherCharges.Text.Trim() != "")
        {
            decimal dcOtherCharges = 0, dcActualOtherCharges = 0;
            dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());
            dcActualOtherCharges = Convert.ToDecimal(hdnOtherCharges.Value);

            if (dcOtherCharges > 0 && dcActualOtherCharges > 0)
            {
                if (dcOtherCharges > dcActualOtherCharges)
                {
                    lblOtherChargesValidator.Visible = true;
                    hdnPageValid.Value = "1";
                    rfvJustification.Enabled = true;
                }
                else
                {
                    rfvJustification.Enabled = false;
                }
            }
        }
        GetTotalAmount();
        txtOtherCharges.Focus();
    }

    #region GRIDVIEW EVENT
    protected void GridViewVehicle_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {

        }
    }

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0, dcAdvanceAmount = 0, dcTollCharges = 0, dcOtherCharges = 0, dcTotalAmount = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Rate") != DBNull.Value)
            {
                dcFreightAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Rate"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "VaraiExpense") != DBNull.Value)
            {
                dcVaraiExpenses = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "VaraiExpense"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "DetentionAmount") != DBNull.Value)
            {
                dcDetentionAmt = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DetentionAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges") != DBNull.Value)
            {
                dcEmptyContRecdCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "EmptyContRecptCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TollCharges") != DBNull.Value)
            {
                dcTollCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TollCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "OtherCharges") != DBNull.Value)
            {
                dcOtherCharges = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "OtherCharges"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "AdvanceAmount") != DBNull.Value)
            {
                dcAdvanceAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AdvanceAmount"));
            }

            if (DataBinder.Eval(e.Row.DataItem, "TotalAmount") != DBNull.Value)
            {
                dcTotalAmount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }

            ViewState["FreightAmt"] = Convert.ToDecimal(ViewState["FreightAmt"]) + dcFreightAmt;
            ViewState["VaraiAmt"] = Convert.ToDecimal(ViewState["VaraiAmt"]) + dcVaraiExpenses;
            ViewState["DetentionAmt"] = Convert.ToDecimal(ViewState["DetentionAmt"]) + dcDetentionAmt;
            ViewState["EmptyContReturnAmt"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcEmptyContRecdCharges;
            ViewState["TollCharges"] = Convert.ToDecimal(ViewState["TollCharges"]) + dcTollCharges;
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["EmptyContReturnAmt"]) + dcOtherCharges;
            ViewState["AdvanceAmt"] = Convert.ToDecimal(ViewState["AdvanceAmt"]) + dcAdvanceAmount;
            ViewState["TotalAmt"] = Convert.ToDecimal(ViewState["TotalAmt"]) + dcTotalAmount;

            hdnFreightAmt.Value = Convert.ToString(ViewState["FreightAmt"]);
            hdnDetentionAmt.Value = Convert.ToString(ViewState["DetentionAmt"]);
            hdnVaraiAmt.Value = Convert.ToString(ViewState["VaraiAmt"]);
            hdnEmptyContReturnAmt.Value = Convert.ToString(ViewState["EmptyContReturnAmt"]);
            hdnTollCharges.Value = Convert.ToString(ViewState["TollCharges"]);
            hdnOtherCharges.Value = Convert.ToString(ViewState["OtherCharges"]);
            hdnAdvanceAmt.Value = Convert.ToString(ViewState["AdvanceAmt"]);
            hdnTotalAmount.Value = Convert.ToString(ViewState["TotalAmt"]);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "<b>Total</b>";
            e.Row.Cells[5].Text = "<b>" + Convert.ToDecimal(ViewState["FreightAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[7].Text = "<b>" + Convert.ToDecimal(ViewState["AdvanceAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[8].Text = "<b>" + Convert.ToDecimal(ViewState["DetentionAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[9].Text = "<b>" + Convert.ToDecimal(ViewState["VaraiAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[10].Text = "<b>" + Convert.ToDecimal(ViewState["EmptyContReturnAmt"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[11].Text = "<b>" + Convert.ToDecimal(ViewState["TollCharges"]).ToString("#,##0.00") + "</b>";
            e.Row.Cells[12].Text = "<b>" + Convert.ToDecimal(ViewState["OtherCharges"]).ToString("#,##0.00") + "</b>";
        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int TranRequestId = Convert.ToInt32(Session["TransReqId"]); ;

        Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        string strVehicleNo = lblVehicleNo.Text.Trim();
        int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, LoggedInUser.glUserId);

        if (result == 0)
        {
            lblError.Text = "Rate Detail Added Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;
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
            lblError.Text = "Details Already Updated!";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
    }

    #endregion

    #region PACKING LIST
    protected void imgbtnPackingList_Click(object sender, ImageClickEventArgs e)
    {
        int TransRequestId = Convert.ToInt32(Session["TRId"]);
        if (TransRequestId > 0)
        {
            DownloadDocument(TransRequestId);
        }
    }

    private void DownloadDocument(int TransReqId)
    {
        string FilePath = "";
        String ServerPath = FileServer.GetFileServerDir();
        using (ZipFile zip = new ZipFile())
        {
            zip.AddDirectoryByName("TransportFiles");
            DataSet dsGetDoc = DBOperations.GetPackingListDocs(TransReqId);
            if (dsGetDoc != null)
            {
                for (int i = 0; i < dsGetDoc.Tables[0].Rows.Count; i++)
                {
                    if (dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString() != "")
                    {
                        if (ServerPath == "")
                        {
                            FilePath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString());
                        }
                        else
                        {
                            FilePath = ServerPath + "Transport\\" + dsGetDoc.Tables[0].Rows[i]["DocPath"].ToString();
                        }
                        zip.AddFile(FilePath, "TransportFiles");
                    }
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("TransportZip_{0}.zip", DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
    }

    #endregion

    protected void gvSellingDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected bool DecideHere(string Str)
    {
        if (Str == "0" || Str=="")
            return false;
        else
            return true;
    }

    protected void gvSellingDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "detentioncopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "varaicopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emptycontcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "tollcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "othercopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "emailapprovalcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
        else if (e.CommandName.ToLower().Trim() == "contractcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadMultipleDocument(commandArgs[0].ToString(), e.CommandName.ToLower());
        }
    }

    private void DownloadMultipleDocument(string DocumentPath, string Documentname)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (Documentname == "detentioncopy")
        {
            DocumentPath = "TransportDetentionDoc\\" + DocumentPath;
        }
        else if (Documentname == "varaicopy")
        {
            DocumentPath = "TransportVaraiDoc\\" + DocumentPath;
        }
        else if (Documentname == "emptycontcopy")
        {
            DocumentPath = "TransportEmptyContDoc\\" + DocumentPath;
        }
        else if (Documentname == "tollcopy")
        {
            DocumentPath = "TransportTollDoc\\" + DocumentPath;
        }
        else if (Documentname == "othercopy")
        {
            DocumentPath = "TransportOtherDoc\\" + DocumentPath;
        }
        else if (Documentname == "emailapprovalcopy")
        {
            DocumentPath = "EmailApprovalUpload\\" + DocumentPath;
        }
        else if (Documentname == "contractcopy")
        {
            DocumentPath = "TransportContractCopyUpload\\" + DocumentPath;
        }

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath(DocumentPath);
            ServerPath = ServerPath.Replace("Transport\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

}