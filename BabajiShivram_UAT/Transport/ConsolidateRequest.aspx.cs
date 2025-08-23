using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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

public partial class Transport_ConsolidateRequest : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvPopup_JobDetail);
        ScriptManager1.RegisterPostBackControl(gvRateDetail);
        ScriptManager1.RegisterPostBackControl(btnSubmit);
        ScriptManager1.RegisterPostBackControl(btnSaveDocument2);
        ScriptManager1.RegisterPostBackControl(btnSaveRate);
        ScriptManager1.RegisterPostBackControl(gvPopup_Vehicle);

        if (Session["TRId"] == null)
        {
            Response.Redirect("RequestReceived.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Consolidate Transport Request";

            DBOperations.FillVehicleType(ddlVehicleType);
            DBOperations.FillCompanyByCategory(ddlTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
            int ConsolidateReqId = Convert.ToInt32(Session["TRConsolidateId"]);
            ConsolidateRequest(ConsolidateReqId);

            // Account Expense
            hdnNewPaymentLid.Value = Convert.ToString(AccountExpense.GetNewPaymentLid()); // Get New Payment PkId
            DataTable dtAnnexureDoc2 = new DataTable();
            dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
            ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
        }
    }

    protected void ConsolidateRequest(int ConsolidateReqId)
    {
        DataView dvDetail = DBOperations.GetConsolidateRequestById(ConsolidateReqId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            lblTRRefNo.Text = dvDetail.Table.Rows[0]["TransRefNo"].ToString();
            hdnTransporterId.Value = dvDetail.Table.Rows[0]["TransporterId"].ToString();
            if (hdnTransporterId.Value != "" && hdnTransporterId.Value != "0")
                ddlTransporter.SelectedValue = hdnTransporterId.Value;
            ddlTransporter_SelectedIndexChanged(null, EventArgs.Empty);
            ddlTransporter.Enabled = false;
            lblTransporter.Text = dvDetail.Table.Rows[0]["TransporterName"].ToString();
            lblCreatedBy.Text = dvDetail.Table.Rows[0]["CreatedBy"].ToString();
            if (dvDetail.Table.Rows[0]["CreatdDate"] != DBNull.Value)
                lblCreatedDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["CreatdDate"]).ToString("dd/MM/yyyy");
            gvRateDetail.DataBind();
            if (gvRateDetail != null && gvRateDetail.Rows.Count > 0)
            {
                btnSaveRate.Visible = true;
                btnCancelRate.Visible = true;
            }
        }
    }

    protected void ddlTransporter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransporter.SelectedValue == "524") // navbharat transporter
        {
            txtVehicleNo.Visible = false;
            ddVehicleNo.Visible = true;
            rfvVehicleNo.Enabled = false;
            DBOperations.FillVehicleForNavbharat(ddVehicleNo);
            ddVehicleNo.Focus();
            rfvVehicleId.Enabled = true;
        }
        else if (ddlTransporter.SelectedValue == "17304") // navbharat transporter
        {
            txtVehicleNo.Visible = false;
            ddVehicleNo.Visible = true;
            rfvVehicleNo.Enabled = false;
            DBOperations.FillVehicleForNAVJEEVAN(ddVehicleNo);
            ddVehicleNo.Focus();
            rfvVehicleId.Enabled = true;
        }
        else
        {
            rfvVehicleNo.Enabled = true;
            txtVehicleNo.Visible = true;
            ddVehicleNo.Visible = false;
            rfvVehicleId.Enabled = false;
            txtVehicleNo.Focus();
        }
        gvRateDetail.DataBind();
    }

    protected void lnkbtnFundRequest_Click(object sender, EventArgs e)
    {
        ViewState["RateDetailId"] = "";
        GridViewRow row = ((GridViewRow)((LinkButton)sender).NamingContainer);
        LinkButton lnkbtnFundRequest = (LinkButton)row.FindControl("lnkbtnFundRequest");

        txtPaidTo.Text = row.Cells[6].Text.Trim();
        DataSet dsGetJobDetails = DBOperations.GetConsolidateJobDetail(Convert.ToInt32(Session["TRConsolidateId"]));
        if (dsGetJobDetails != null && dsGetJobDetails.Tables[0].Rows.Count > 0)
        {
            int TransRateId = 0;
            for (int i = 0; i < dsGetJobDetails.Tables[0].Rows.Count; i++)
            {
                TransRateId = Convert.ToInt32(dsGetJobDetails.Tables[0].Rows[i]["TransRateId"].ToString());
                if (ViewState["RateDetailId"].ToString() == "")
                    ViewState["RateDetailId"] = TransRateId.ToString();
                else
                    ViewState["RateDetailId"] = ViewState["RateDetailId"].ToString() + ',' + TransRateId.ToString();
            }
            hdnRateDetailId.Value = ViewState["RateDetailId"].ToString();
        }

        //hdnRateDetailId.Value = gvRateDetail.DataKeys[row.RowIndex].Values[0].ToString();
        lblFreightRate.Text = row.Cells[8].Text.Trim();
        txtAmount.Text = row.Cells[16].Text.Trim();
        gvPopup_Vehicle.DataBind();
        int TransporterId = Convert.ToInt32(((HiddenField)row.FindControl("hdnTransporterId")).Value.Trim());
        int FundRequestId = Convert.ToInt32(((HiddenField)row.FindControl("hdnFundReqId")).Value.Trim());
        if (hdnMemoCopyPath.Value != "")
            hdnMemoCopyPath.Value = hdnMemoCopyPath.Value + " ; " + Convert.ToString(((HiddenField)row.FindControl("hdnMemoPath")).Value.Trim());
        else
            hdnMemoCopyPath.Value = Convert.ToString(((HiddenField)row.FindControl("hdnMemoPath")).Value.Trim());

        if (TransporterId > 0)
        {
            DataView dvDetail = DBOperations.GetTransporterBankDetails(TransporterId);
            if (dvDetail != null && dvDetail.Table.Rows.Count > 0)
            {
                if (dvDetail.Table.Rows[0]["lid"].ToString() != "")
                {
                    lblBankName.Text = dvDetail.Table.Rows[0]["BankName"].ToString();
                    lblAccountNo.Text = dvDetail.Table.Rows[0]["AccountNo"].ToString();
                    lblIFSCCode.Text = dvDetail.Table.Rows[0]["IFSCCode"].ToString();
                }
            }
        }

        // get existing data for fund request if present
        DataSet dsGetFundDetails = AccountExpense.GetPaymentRequestById(FundRequestId);
        if (dsGetFundDetails != null && dsGetFundDetails.Tables.Count > 0)
        {
            if (dsGetFundDetails.Tables[0].Rows.Count > 0)
            {
                if (dsGetFundDetails.Tables[0].Rows[0]["lid"] != DBNull.Value)
                {
                    lblFundError.Text = "You already sent request for this job";
                    lblFundError.CssClass = "errorMsg";
                    txtAmount.Text = dsGetFundDetails.Tables[0].Rows[0]["Amount"].ToString();
                    txtPaidTo.Text = dsGetFundDetails.Tables[0].Rows[0]["PaidTo"].ToString();
                    txtRemark.Text = dsGetFundDetails.Tables[0].Rows[0]["Remark"].ToString();
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                    ddlPaymentType.Enabled = false;
                    txtAmount.Enabled = false;
                    txtPaidTo.Enabled = false;
                    txtRemark.Enabled = false;
                }
            }
        }

        if (lblBankName.Text.Trim() == "" && lblAccountNo.Text.Trim() == "")
        {
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
            lblFundError.Text = "Please update bank details for transporter before sending fund request!!";
            lblFundError.CssClass = "errorMsg";
        }
        mpeFundRequest.Show();
    }

    #region RATE DETAIL

    protected void btnSaveRate_Click(object sender, EventArgs e)
    {
        string VehicleNo = "";
        decimal dcAdvance = 0, dcRate = 0, dcAdvanceAmount = 0, dcFreightAmount = 0, dcVaraExp = 0, dcDetentionCharges = 0, dcEmptyContRecptCharges = 0,
                dcTollCharges = 0, dcOtherCharges = 0, dcMarketRate = 0, dcContractPrice = 0, dcSellingPrice = 0;
        int TransReqId = 0, TransporterId = 0, VehicleType = 0, VehicleId = 0;
        DateTime LRDate = DateTime.MinValue, ChallanDate = DateTime.MinValue;
        if (Session["TRId"] != null)
        {
            VehicleNo = txtVehicleNo.Text.Trim();
            TransReqId = Convert.ToInt32(Convert.ToString(Session["TRId"]));
            TransporterId = Convert.ToInt32(ddlTransporter.SelectedValue);
            VehicleType = Convert.ToInt32(ddlVehicleType.SelectedValue);
            string MemoFilePath = "", EmailFilePath = "", ContractFilePath = ""; 

            if (ddlTransporter.SelectedValue == "524" || ddlTransporter.SelectedValue == "17304") // NAV BHARAT OR NAVJEEVAN TRANSPORTER
            {
                VehicleId = Convert.ToInt32(ddVehicleNo.SelectedValue);
                VehicleNo = ddVehicleNo.SelectedItem.Text.Trim();
            }

            if (txtMarketBillingRate.Text.Trim() != "")
            {
                dcMarketRate = Convert.ToDecimal(txtMarketBillingRate.Text.Trim());
            }

            if (txtAdvance.Text.Trim() != "")
            {
                dcAdvance = Convert.ToDecimal(txtAdvance.Text.Trim());
            }

            if (txtRate.Text.Trim() != "")
            {
                dcRate = Convert.ToDecimal(txtRate.Text.Trim());
            }

            if (dcRate > 0 && dcAdvance > 0)
            {
                double dcAdvancePercentage = Convert.ToDouble(dcAdvance) * 0.01;
                dcAdvanceAmount = Convert.ToDecimal(dcRate * Convert.ToDecimal(dcAdvancePercentage));
            }

            if (txtDetentionAmount.Text.Trim() != "")
                dcDetentionCharges = Convert.ToDecimal(txtDetentionAmount.Text.Trim());
            if (txtVaraiExp.Text.Trim() != "")
                dcVaraExp = Convert.ToDecimal(txtVaraiExp.Text.Trim());
            if (txtEmptyContCharges.Text.Trim() != "")
                dcEmptyContRecptCharges = Convert.ToDecimal(txtEmptyContCharges.Text.Trim());
            if (txtTollCharges.Text.Trim() != "")
                dcTollCharges = Convert.ToDecimal(txtTollCharges.Text.Trim());
            if (txtOtherCharges.Text.Trim() != "")
                dcOtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim());

            if (fuMemoDocument.HasFile)
                MemoFilePath = UploadMemoFile(fuMemoDocument, "Transport\\" + hdnFilePath.Value);
            //if (txtLRDate.Text.Trim() != "")
            //    LRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());
            //if (txtBabajiChallanDate.Text.Trim() != "")
            //    ChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());
            //DispatchedCount = Convert.ToInt32(lblDispatch_Value.Text.Trim());

            if (rdlRequestReceive.SelectedValue == "1")
            {
                dcContractPrice = Convert.ToDecimal(txtContractPrice.Text);
                if (fuContractUploadCopy.HasFile)
                    ContractFilePath = UploadContractFiles(fuContractUploadCopy);
            }
            else if (rdlRequestReceive.SelectedValue == "2")
            {
                dcSellingPrice = Convert.ToDecimal(txtEmailSellPrice.Text);
                if (fuEmailApprovalCopy.HasFile)
                    EmailFilePath = UploadEmailFiles(fuEmailApprovalCopy);
            }

            if (hdnIsUpdate.Value == "1")
            {
                if (gvTransportJobDetail != null && gvTransportJobDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < gvTransportJobDetail.Rows.Count; i++)
                    {
                        if (gvTransportJobDetail.DataKeys[i].Value.ToString() != "")
                        {
                            TransReqId = Convert.ToInt32(gvTransportJobDetail.DataKeys[i].Value.ToString());
                            int result = DBOperations.UpdateTransportRateDetailForConsolidateJob(TransReqId, TransporterId, VehicleType, VehicleId, VehicleNo, MemoFilePath, txtCity.Text.Trim(), dcMarketRate, "",
                                                "", dcRate, dcAdvance, dcAdvanceAmount, dcFreightAmount, dcDetentionCharges, dcVaraExp,
                                                dcEmptyContRecptCharges, dcTollCharges, dcOtherCharges, LRDate, ChallanDate,
                                                dcContractPrice, dcSellingPrice, EmailFilePath, ContractFilePath, LoggedInUser.glUserId);//2020
                            if (result == 0)
                            {
                                if (txtBillingInstruction.Text != "")
                                {
                                    int AddInstruction = DBOperations.TR_AddBillingInstructions(TransReqId, TransReqId,TransporterId, txtBillingInstruction.Text.Trim(), LoggedInUser.glUserId);
                                }
                                lblError.Text = "Successfully updated rate detail.";
                                lblError.CssClass = "success";
                                ResetControls();
                            }
                            else if (result == 2)
                            {
                                lblError.Text = "Rate detail does not exists.";
                                lblError.CssClass = "errorMsg";
                            }
                            else
                            {
                                lblError.Text = "System error. Please try again later.";
                                lblError.CssClass = "errorMsg";
                            }
                        }
                    }
                }
            }
            else
            {
                if (gvTransportJobDetail != null && gvTransportJobDetail.Rows.Count > 0)
                {
                    for (int i = 0; i < gvTransportJobDetail.Rows.Count; i++)
                    {
                        if (gvTransportJobDetail.DataKeys[i].Value.ToString() != "")
                        {
                            TransReqId = Convert.ToInt32(gvTransportJobDetail.DataKeys[i].Value.ToString());

                            int result = DBOperations.AddTransportRateDetail(true, TransReqId, VehicleId, TransporterId, VehicleType, VehicleNo, MemoFilePath, txtCity.Text.Trim(), dcMarketRate, "",
                                "", dcRate, dcAdvance, dcAdvanceAmount, dcFreightAmount, dcDetentionCharges, dcVaraExp,
                                dcEmptyContRecptCharges, dcTollCharges, dcOtherCharges, LRDate, ChallanDate, 1,
                                dcContractPrice, dcSellingPrice, EmailFilePath, ContractFilePath, LoggedInUser.glUserId);
                            if (result > 2)
                            {
                                int AddTP = DBOperations.AddTransporterPlaced(TransReqId, TransporterId, LoggedInUser.glUserId);
                                if (txtBillingInstruction.Text != "")
                                {
                                    int AddInstruction = DBOperations.TR_AddBillingInstructions(TransReqId, result,TransporterId, txtBillingInstruction.Text.Trim(), LoggedInUser.glUserId);
                                }
                                lblError.Text = "Successfully added rate detail.";
                                lblError.CssClass = "success";
                                ResetControls();
                                gvRateDetail.DataBind();
                            }
                            else if (result == 2)
                            {
                                lblError.Text = "Rate detail already exists for same transporter!";
                                lblError.CssClass = "errorMsg";
                            }
                            else
                            {
                                lblError.Text = "System error. Please try again later.";
                                lblError.CssClass = "errorMsg";
                            }
                            //else if (result == 3)
                            //{
                            //    lblError.Text = "Rate detail request exceeds number of dispatched count!";
                            //    lblError.CssClass = "errorMsg";
                            //}
                        }
                    }
                }
            }
        }
    }

    protected void btnCancelRate_Click(object sender, EventArgs e)
    {
        ResetControls();
        btnSaveRate.Text = "Save";
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetAdvanceAmount();
            txtAdvance.Focus();
        }
        catch (Exception en)
        {

        }
    }

    protected void txtAdvance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetAdvanceAmount();
            txtMarketBillingRate.Focus();
        }
        catch (Exception en)
        {

        }
    }

    protected void GetAdvanceAmount()
    {
        decimal dcAdvance = 0, dcRate = 0, dcAdvanceAmount = 0;
        if (txtAdvance.Text.Trim() != "")
        {
            dcAdvance = Convert.ToDecimal(txtAdvance.Text.Trim());
        }
        else
        {
            txtAdvanceAmount.Text = "";
        }

        if (txtRate.Text.Trim() != "")
        {
            dcRate = Convert.ToDecimal(txtRate.Text.Trim());
        }

        if (dcRate > 0 && dcAdvance > 0)
        {
            double dcAdvancePercentage = Convert.ToDouble(dcAdvance) * 0.01;
            dcAdvanceAmount = Convert.ToDecimal(dcRate * Convert.ToDecimal(dcAdvancePercentage));
            txtAdvanceAmount.Text = dcAdvanceAmount.ToString();
        }
        gvRateDetail.DataBind();
    }

    protected void ResetControls()
    {
        txtCity.Text = "";
        DBOperations.FillVehicleType(ddlVehicleType);
        DBOperations.FillCompanyByCategory(ddlTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
        ConsolidateRequest(Convert.ToInt32(Session["TRId"]));
        txtVehicleNo.Text = "";
        txtRate.Text = "";
        txtAdvance.Text = "";
        txtAdvanceAmount.Text = "";
        //txtLRNo.Text = "";
        //txtLRDate.Text = "";
        //txtBabajiChallanNo.Text = "";
        //txtBabajiChallanDate.Text = "";
        //txtFreightAmount.Text = "";
        txtDetentionAmount.Text = "";
        txtVaraiExp.Text = "";
        txtEmptyContCharges.Text = "";
        gvRateDetail.DataBind();
    }

    #endregion

    #region GRID VIEW EVENTS

    protected void gvRateDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkbtnDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
            lnkbtnDelete.Visible = true;

            if (DataBinder.Eval(e.Row.DataItem, "AdvanceAmount") != DBNull.Value)
            {
                decimal dcAdvanceAmount = 0;
                dcAdvanceAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AdvanceAmount"));
                if (dcAdvanceAmount == 0)
                {
                    e.Row.Cells[3].Text = "";
                    //e.Row.Cells[2].Text = "";
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "FundRequestId") != DBNull.Value)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FundRequestId")) > 0)
                {
                    e.Row.Cells[3].Text = "";
                    //e.Row.Cells[2].Text = "";
                    lnkbtnDelete.Visible = false;
                }
            }

            if (DataBinder.Eval(e.Row.DataItem, "MemoAttachment") != DBNull.Value)
            {
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "MemoAttachment")) == "")
                {
                    e.Row.Cells[4].Text = "";
                }
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }

            //if (DataBinder.Eval(e.Row.DataItem, "JobDeliveryVehicleNo") != DBNull.Value)
            //{
            //    if (lnkbtnDelete != null)
            //    {
            //        lnkbtnDelete.Visible = false;
            //    }

            //    if (lnkEdit != null)
            //    {
            //        lnkEdit.Visible = false;
            //    }
            //}
        }
    }

    protected void gvRateDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "edit")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                hdnLid.Value = lid.ToString();
                DataView dvGetRateDetail = DBOperations.GetTransRateDetailById(lid);
                if (dvGetRateDetail != null)
                {
                    DBOperations.FillCompanyByCategory(ddlTransporter, Convert.ToInt32(EnumCompanyType.Transporter));
                    ddlTransporter.SelectedValue = dvGetRateDetail.Table.Rows[0]["TransporterId"].ToString();
                    ddlTransporter_SelectedIndexChanged(null, EventArgs.Empty);
                    if (ddlTransporter.SelectedValue == "524" || ddlTransporter.SelectedValue == "17304") // NAV BHARAT OR NAVJEEVAN TRANSPORTER
                    {
                        if (dvGetRateDetail.Table.Rows[0]["VehicleId"] != DBNull.Value)
                            ddVehicleNo.SelectedValue = dvGetRateDetail.Table.Rows[0]["VehicleId"].ToString();
                    }
                    else
                    {
                        txtVehicleNo.Text = dvGetRateDetail.Table.Rows[0]["VehicleNo"].ToString();
                    }
                    DBOperations.FillVehicleType(ddlVehicleType);
                    ddlVehicleType.SelectedValue = dvGetRateDetail.Table.Rows[0]["VehicleTypeId"].ToString();
                    txtRate.Text = dvGetRateDetail.Table.Rows[0]["Rate"].ToString();
                    txtAdvance.Text = dvGetRateDetail.Table.Rows[0]["Advance"].ToString();
                    txtAdvanceAmount.Text = dvGetRateDetail.Table.Rows[0]["AdvanceAmount"].ToString();
                    txtCity.Text = dvGetRateDetail.Table.Rows[0]["City"].ToString();
                    GetAdvanceAmount();
                    //txtLRNo.Text = dvGetRateDetail.Table.Rows[0]["LRNo"].ToString();
                    //txtLRDate.Text = Convert.ToDateTime(dvGetRateDetail.Table.Rows[0]["LRDate"]).ToString("dd/MM/yyyy");
                    //txtBabajiChallanNo.Text = dvGetRateDetail.Table.Rows[0]["ChallanNo"].ToString();
                    //txtBabajiChallanDate.Text = Convert.ToDateTime(dvGetRateDetail.Table.Rows[0]["ChallanDate"]).ToString("dd/MM/yyyy");
                    txtMarketBillingRate.Text = dvGetRateDetail.Table.Rows[0]["MarketBillingRate"].ToString();
                    txtBillingInstruction.Text = dvGetRateDetail.Table.Rows[0]["Instruction"].ToString();
                    txtDetentionAmount.Text = dvGetRateDetail.Table.Rows[0]["DetentionAmount"].ToString();
                    txtVaraiExp.Text = dvGetRateDetail.Table.Rows[0]["VaraiExpense"].ToString();
                    txtEmptyContCharges.Text = dvGetRateDetail.Table.Rows[0]["EmptyContRecptCharges"].ToString();
                    btnSaveRate.Text = "Update";
                    hdnIsUpdate.Value = "1";
                    btnSaveRate.Visible = true;
                    btnCancelRate.Visible = true;
                }
            }
            else
            {
                ResetControls();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "memocopy")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DownloadMemoCopy(e.CommandArgument.ToString());
            }
        }
        else if (e.CommandName.ToLower().Trim() == "deleterow")
        {
            int lid = Convert.ToInt32(e.CommandArgument.ToString());
            if (lid > 0)
            {
                int result = DBOperations.DeleteTransportRateId(lid, LoggedInUser.glUserId);
                if (result == 1)
                {
                    lblError.Text = "Successfully deleted rate detail.";
                    lblError.CssClass = "success";
                    gvRateDetail.DataBind();
                }
                else if (result == 2)
                {
                    lblError.Text = "Rate detail does not exists.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 3)
                {
                    lblError.Text = "Details can't be removed! Advance Already Requested!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result == 4)
                {
                    lblError.Text = "Details can't be removed! Vehicle Status Updated as Delivered!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System error. Please rty again later.";
                    lblError.CssClass = "errorMsg";
                }
            }
        }
        else if (e.CommandName.ToLower().Trim() == "contractcopy")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DownloadContractCopy(e.CommandArgument.ToString());
            }
        }
        else if (e.CommandName.ToLower().Trim() == "emailcopy")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DownloadEmailCopy(e.CommandArgument.ToString());
            }
        }
    }

    protected void gvRateDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    #endregion

    #region FUND REQUEST

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (fuDocument2.HasFile)
        {
            btnSaveDocument2_Click(fuDocument2, EventArgs.Empty);
        }

        int result = -123, JobId = 0, TransporterId = 0, ExpenseTypeId = 11, BranchId = 0, PaymentTypeId = 0, count = 1, Isready = 0;
        int IsInterestReq = 0, IsPenaltyReq = 0, ACPNonACP = 0, RdDutyPenalty = 0;
        decimal dcAmount = 0, dcAdvanceAmount = 0, DutyAmount = 0;
        Boolean AdvanceRecd = false;
        string strPenaltyCopy = "", challanNo = "", JobRefNo = "";

        Int32.TryParse(hdnTransporterId.Value, out TransporterId);

        if (gvPopup_JobDetail != null && gvPopup_JobDetail.Rows.Count > 0)
        {
            for (int i = 0; i < gvPopup_JobDetail.Rows.Count; i++)
            {
                hdnJobId.Value = gvPopup_JobDetail.DataKeys[i].Values[0].ToString();
                hdnBranchId.Value = gvPopup_JobDetail.DataKeys[i].Values[1].ToString();
                break;
            }
        }

        if (hdnJobId.Value != "" && hdnJobId.Value != "0")
            JobId = Convert.ToInt32(hdnJobId.Value);
        if (hdnBranchId.Value != "" && hdnBranchId.Value != "0")
            BranchId = Convert.ToInt32(hdnBranchId.Value);
        if (ddlPaymentType.SelectedValue != "0")
            PaymentTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);

        if (ExpenseTypeId != 0 && JobId > 0) // && BranchId != 0
        {
            #region VALIDATE THE DOCUMENTS UPLOAD

            if (ViewState["AnnexureDoc2"] != null)
            {
                DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
                if (dtAnnexure != null)
                {
                    if (dtAnnexure.Rows.Count > 0)
                    {
                        count = 0;      // success
                    }
                    else
                    {
                        lblFundError.Text = "Please browse atleast 1 document.";
                        lblFundError.CssClass = "errorMsg";
                        count = 1;      // error
                    }
                }
                else
                {
                    lblFundError.Text = "Please browse atleast 1 document.";
                    lblFundError.CssClass = "errorMsg";
                    count = 1;      // error
                }
            }
            else
            {
                count = 1;          // error
                lblFundError.Text = "Please browse atleast 1 document.";
                lblFundError.CssClass = "errorMsg";
            }

            #endregion

            //if (txtJobNumber.Text.Trim() != "")
            //{
            //    JobRefNo = txtJobNumber.Text.Trim();
            //}

            if (txtAmount.Text.Trim() != "")
                dcAmount = Convert.ToDecimal(txtAmount.Text.Trim());

            if (count == 0)
            {
                int TranRequestId = Convert.ToInt32(Session["TRId"]);

                result = DBOperations.AddTransFunRequest(JobId, JobRefNo, TranRequestId, TransporterId, ExpenseTypeId, PaymentTypeId, dcAmount, BranchId,
                            txtRemark.Text.Trim(), AdvanceRecd, dcAdvanceAmount, dcAmount.ToString(), 1, LoggedInUser.glUserId);

                if (result != 1 && result != 2 && result != 3)
                {
                    //Add consolidate job payment expense
                    if (gvPopup_JobDetail != null && gvPopup_JobDetail.Rows.Count > 0)
                    {
                        for (int i = 0; i < gvPopup_JobDetail.Rows.Count; i++)
                        {
                            if (gvPopup_JobDetail.DataKeys[i].Value.ToString() != "")
                            {
                                int AddExpense = DBOperations.AddExpenseConsolidateJobs(result, Convert.ToInt32(gvPopup_JobDetail.DataKeys[i].Value.ToString()), LoggedInUser.glUserId);
                            }
                        }
                    }

                    // Add fund request id into transport rate detail table
                    if (hdnRateDetailId.Value != "" && hdnRateDetailId.Value != "0")
                    {
                        int AddFundId = DBOperations.UpdateTransportFundId(hdnRateDetailId.Value, result);
                    }
                    //if (txtPlanningDate.Text.Trim() != "")
                    //{
                    //    DateTime PlanningDate = DateTime.MinValue;
                    //    PlanningDate = Commonfunctions.CDateTime(txtPlanningDate.Text.Trim());

                    //    if (PlanningDate != DateTime.MinValue)
                    //    {
                    //        int AddExamineResult = AccountExpense.AddExamineDetails(JobId, PlanningDate, LoggedInUser.glUserId);
                    //    }
                    //}

                    #region ADD DOCUMENTS DETAILS
                    if (Convert.ToString(ViewState["AnnexureDoc2"]) != "")
                    {
                        DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
                        if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
                        {
                            string DocPath = "", FileName = "";
                            for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                            {
                                if (dtAnnexure.Rows[i]["DocPath"] != null)
                                    DocPath = dtAnnexure.Rows[i]["DocPath"].ToString();
                                if (dtAnnexure.Rows[i]["DocumentName"] != null)
                                    FileName = dtAnnexure.Rows[i]["DocumentName"].ToString();
                                int result_Doc = AccountExpense.AddExpenseDocDetails(result, DocPath, FileName, LoggedInUser.glUserId);
                            }
                        }
                    }
                    #endregion
                   // SendFundRequestMail(result, Convert.ToInt32(Session["TRConsolidateId"]), Convert.ToInt32(hdnTransporterId.Value));

                    ResetControls_Expense();
                    lblError.Text = "Advance Fund Request Sent For Mgmt Approval!";
                    lblError.CssClass = "success";

                    DataTable dtAnnexureDoc2 = new DataTable();
                    dtAnnexureDoc2.Columns.AddRange(new DataColumn[4] { new DataColumn("PkId"), new DataColumn("DocPath"), new DataColumn("DocumentName"), new DataColumn("UserId") });
                    ViewState["AnnexureDoc2"] = dtAnnexureDoc2;
                    mpeFundRequest.Hide();
                }
                else if (result == 1)
                {
                    lblFundError.Text = "Record already exists for Job Number and Expense Type and Amount.";
                    lblFundError.CssClass = "errorMsg";
                }
                else if (result == 3)
                {
                    lblFundError.Text = "Please Select Job Number In Search";
                    lblFundError.CssClass = "errorMsg";
                }
                else
                {
                    lblFundError.Text = "Error while saving record. Please try again later.";
                    lblFundError.CssClass = "errorMsg";
                }
            }
            else
            {
                lblFundError.Text = "Please browse atleast 1 document!";
                lblFundError.CssClass = "errorMsg";
                mpeFundRequest.Show();
            }
        }
    }

    private bool SendFundRequestMail(int PaymentId, int TransReqId, int TransporterId)
    {
        string MessageBody = "", strCustomerEmail = "", strCCEmail = "", strSubject = "", JobRefno = "", BranchName = "", PaymentTypeName = "", ConsigneeName = "",
            ExpenseTypeName = "", Amount = "", PaidTo = "", Remark = "", CreatedBy = "", CreatedByEmail = "", HoldedBy = "", HoldedByEmail = "", HoldReason = "", BankName = "",
            AccountNumber = "", IFSCCode = "";
        bool bEmailSucces = false;
        StringBuilder strbuilder = new StringBuilder();

        try
        {
            if (PaymentId != 0)
            {
                DataSet dsGetPaymentDetails = AccountExpense.GetPaymentRequestById(PaymentId);
                if (dsGetPaymentDetails != null)
                {
                    JobRefno = dsGetPaymentDetails.Tables[0].Rows[0]["JobRefNo"].ToString();
                    ConsigneeName = dsGetPaymentDetails.Tables[0].Rows[0]["ConsigneeName"].ToString();
                    BranchName = dsGetPaymentDetails.Tables[0].Rows[0]["BranchName"].ToString();
                    PaymentTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["PaymentTypeName1"].ToString();
                    ExpenseTypeName = dsGetPaymentDetails.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
                    Amount = dsGetPaymentDetails.Tables[0].Rows[0]["Amount"].ToString();
                    PaidTo = dsGetPaymentDetails.Tables[0].Rows[0]["PaidTo"].ToString();
                    BankName = lblBankName.Text.Trim();
                    AccountNumber = lblAccountNo.Text.Trim();
                    IFSCCode = lblIFSCCode.Text.Trim();
                    Remark = dsGetPaymentDetails.Tables[0].Rows[0]["Remark"].ToString();
                    CreatedBy = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedBy"].ToString();
                    CreatedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["CreatedByEmail"].ToString();
                    HoldedBy = dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByName"].ToString();
                    HoldedByEmail = dsGetPaymentDetails.Tables[0].Rows[0]["HoldedByEmail"].ToString();
                    HoldReason = dsGetPaymentDetails.Tables[0].Rows[0]["HoldReason"].ToString();
                    strCustomerEmail = "neeraj.mandowara@babajishivram.com, ho.accounts7@babajishivram.com"; 

                    //if (ExpenseTypeName.Trim().ToLower() == "duty payment")
                    //{
                    //    strCustomerEmail = // "bsavita@babajishivram.com";
                    //}
                    //else
                    //{
                    //    strCustomerEmail = //"neeraj.mandowara@babajishivram.com";
                    //}

                    strCCEmail = "ho.accounts7@babajishivram.com, ho.accounts9@babajishivram.com, bsameer@babajishivram.com , " + CreatedByEmail;    
                    strSubject = "New fund request for transporter " + txtPaidTo.Text.Trim() + " of amount " + Amount + " for Babaji Job " + JobRefno + " and expense type " + ExpenseTypeName + ".";

                    if (strCustomerEmail == "" || strSubject == "")
                        return bEmailSucces;
                    else
                    {
                        MessageBody = "Dear Sir,<BR><BR> Please find the below fund details been requested.<BR><BR>";

                        DataSet dsGetJobDetails = DBOperations.GetConsolidateJobDetail(TransReqId);
                        if (dsGetJobDetails != null && dsGetJobDetails.Tables[0].Rows.Count > 0)
                        {
                            strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Job No</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Consignee</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Branch</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>From</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Destination</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR No</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>LR Date</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan No</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Challan Date</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Planning Date</th>");

                            for (int i = 0; i < dsGetJobDetails.Tables[0].Rows.Count; i++)
                            {
                                strbuilder = strbuilder.Append("<tr style='padding: 3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["JobRefNo"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["ConsigneeName"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["BranchName"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["LocationFrom"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetJobDetails.Tables[0].Rows[i]["Destination"].ToString() + "</th>");
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
                                if (dsGetJobDetails.Tables[0].Rows[i]["PlanningDate"] != DBNull.Value)
                                    strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + Convert.ToDateTime(dsGetJobDetails.Tables[0].Rows[i]["PlanningDate"]).ToString("dd/MM/yyyy") + "</th></tr>");
                                else
                                    strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'></th>");
                            }
                            strbuilder = strbuilder.Append("</table><BR>");
                        }

                        DataSet dsGetVehicleDetail = DBOperations.GetTransRateDetailByTP(Convert.ToInt32(Session["TRId"]), TransporterId);
                        if (dsGetVehicleDetail != null)
                        {
                            strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:99%;border:1px solid black;font-family:Arial;style:normal;font-size:10pt'>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle No</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle Type</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Transporter</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Advance</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Vehicle Hire(Broker Rate)</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Balance</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Market Billing Rate</th>");
                            strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #99CCFF; color:white; text-align: center;'>Saving Amt</th>");

                            for (int i = 0; i < dsGetVehicleDetail.Tables[0].Rows.Count; i++)
                            {
                                strbuilder = strbuilder.Append("<tr style='padding:3px; height: 25px; font-size: 10pt'><th style='padding-left: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["VehicleNo"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["VehicleTypeName"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["TransporterName"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["AdvanceAmount"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["Rate"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["Balance"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["MarketBillingRate"].ToString() + "</th>");
                                strbuilder = strbuilder.Append("<th style='padding: 3px; background-color: #FCD5B4; color:black; text-align: center;'>" + dsGetVehicleDetail.Tables[0].Rows[i]["SavingAmt"].ToString() + "</th></tr>");
                            }
                            strbuilder = strbuilder.Append("</table><BR><BR>");
                        }

                        strbuilder = strbuilder.Append("<table style='text-align:left;margin-left-bottom:40px;width:60%;border:2px solid #d5d5d5;font-family:Arial;style:normal;font-size:10pt' border = 1>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Payment Type</th><td style='padding-left: 3px'>" + PaymentTypeName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Request Type</th><td style='padding-left: 3px'>" + ExpenseTypeName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Amount</th><td style='padding-left: 3px'>" + Amount + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Paid To</th><td style='padding-left: 3px'>" + PaidTo + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Bank Name</th><td style='padding-left: 3px'>" + BankName + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Account No</th><td style='padding-left: 3px'>" + AccountNumber + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>IFSC Code</th><td style='padding-left: 3px'>" + IFSCCode + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Remark</th><td style='padding-left: 3px'>" + Remark + "</td></tr>");
                        strbuilder = strbuilder.Append("<tr><th style='font-weight: bold; padding-left: 3px; text-align: left;'>Created By</th><td style='padding-left: 3px'>" + CreatedBy + "</td></tr>");
                        strbuilder = strbuilder.Append("</table>");

                        MessageBody = MessageBody + strbuilder;
                        MessageBody = MessageBody + "<BR><BR>Thanks & Regards,<BR>" + CreatedBy;

                        System.Collections.Generic.List<string> lstFilePath = new List<string>();
                        // Memo Copy Path
                        if (hdnMemoCopyPath.Value != "")
                        {
                            string[] MemoCopies = hdnMemoCopyPath.Value.ToString().Split(new char[] { ';' });
                            foreach (string MemoCopy in MemoCopies)
                            {
                                lstFilePath.Add(MemoCopy);
                            }
                        }

                        DataSet dsGetExpenseDocDetails = AccountExpense.GetExpenseDocDetails(PaymentId);
                        if (dsGetExpenseDocDetails != null && dsGetExpenseDocDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGetExpenseDocDetails.Tables[0].Rows.Count; i++)
                            {
                                if (dsGetExpenseDocDetails.Tables[0].Rows[i]["DocPath"] != DBNull.Value)
                                {
                                    lstFilePath.Add("ExpenseUpload\\" + dsGetExpenseDocDetails.Tables[0].Rows[i]["DocPath"].ToString());
                                }
                            }
                        }

                        bEmailSucces = EMail.SendMailMultiAttach(strCustomerEmail, strCustomerEmail, strCCEmail, strSubject, MessageBody, lstFilePath);
                        return bEmailSucces;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception en)
        {
            return false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetControls_Expense();
        //Response.Redirect("FillJobDetails.aspx");
        mpeFundRequest.Hide();
    }

    protected void ResetControls_Expense()
    {
        //txtJobNumber.Text = "";
        hdnJobId.Value = "0";
        txtAmount.Text = "";
        txtPaidTo.Text = "";
        txtRemark.Text = "";
        //txtPlanningDate.Text = "";
        //lblConsignee.Text = "";
        ddlPaymentType.Items.Clear();
        ddlPaymentType.DataBind();
        lblFundError.Text = "";
        hdnNewPaymentLid.Value = Convert.ToString(AccountExpense.GetNewPaymentLid()); // Get New Payment PkId
        ViewState["AnnexureDoc2"] = null;
        rptDocument2.DataSource = "";
        rptDocument2.DataBind();
        rptDocument2.Visible = false;
    }

    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpeFundRequest.Hide();
    }

    protected void btnFundRequest_Click(object sender, EventArgs e)
    {
        btnSubmit.Visible = true;
        btnCancel.Visible = true;
        lblFundError.Text = "";
        ViewState["RateDetailId"] = "";
        decimal dcAmount = 0, dcFreightRate = 0;
        int TransRateId = 0, JobCount = 0, result = 0, count = 0, TransporterId = 0, ActualTransporterId = 0;
        string strJobIdList = "", TRRefNo = "", JobRefNo = "", Customer = "", Location = "", Destination = "";
        DBOperations.FillCompanyByCategory(ddlTransporter, Convert.ToInt32(EnumCompanyType.Transporter));

        foreach (GridViewRow row in gvRateDetail.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                TransRateId = Convert.ToInt32(gvRateDetail.DataKeys[row.RowIndex].Value.ToString());
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                if (chkSelect.Checked)
                {
                    count++;

                    if (count == 1)
                    {
                        if (hdnMemoCopyPath.Value != "")
                            hdnMemoCopyPath.Value = hdnMemoCopyPath.Value + " ; " + Convert.ToString(((HiddenField)row.FindControl("hdnMemoPath")).Value.Trim());
                        else
                            hdnMemoCopyPath.Value = Convert.ToString(((HiddenField)row.FindControl("hdnMemoPath")).Value.Trim());
                        ActualTransporterId = Convert.ToInt32(((HiddenField)row.FindControl("hdnTransporterId")).Value.Trim());
                        hdnTransporterId.Value = ActualTransporterId.ToString();
                        gvPopup_Vehicle.DataBind();

                        #region Bank & rate details
                        DataView dvDetail = DBOperations.GetTransporterBankDetails(ActualTransporterId);
                        if (dvDetail != null && dvDetail.Table.Rows.Count > 0)
                        {
                            if (dvDetail.Table.Rows[0]["lid"].ToString() != "")
                            {
                                lblBankName.Text = dvDetail.Table.Rows[0]["BankName"].ToString();
                                lblAccountNo.Text = dvDetail.Table.Rows[0]["AccountNo"].ToString();
                                lblIFSCCode.Text = dvDetail.Table.Rows[0]["IFSCCode"].ToString();
                            }
                        }

                        txtPaidTo.Text = row.Cells[8].Text.Trim();
                        gvTransportJobDetail.DataBind();

                        if (dcFreightRate > 0)
                        {
                            dcFreightRate = dcFreightRate + Convert.ToDecimal(row.Cells[16].Text.Trim());
                        }
                        else
                        {
                            dcFreightRate = Convert.ToDecimal(row.Cells[16].Text.Trim());
                        }

                        if (dcAmount > 0)
                        {
                            dcAmount = dcAmount + Convert.ToDecimal(row.Cells[18].Text.Trim());
                        }
                        else
                        {
                            dcAmount = Convert.ToDecimal(row.Cells[18].Text.Trim());
                        }

                        txtAmount.Text = dcAmount.ToString();
                        lblFreightRate.Text = dcFreightRate.ToString();
                        if (ViewState["RateDetailId"].ToString() == "")
                            ViewState["RateDetailId"] = TransRateId.ToString();
                        else
                            ViewState["RateDetailId"] = ViewState["RateDetailId"].ToString() + ',' + TransRateId.ToString();
                        hdnRateDetailId.Value = ViewState["RateDetailId"].ToString();
                        #endregion

                        #region Get existing data for fund request if present
                        int FundRequestId = Convert.ToInt32(((HiddenField)row.FindControl("hdnFundReqId")).Value.Trim());
                        DataSet dsGetFundDetails = AccountExpense.GetPaymentRequestById(FundRequestId);
                        if (dsGetFundDetails != null && dsGetFundDetails.Tables.Count > 0)
                        {
                            if (dsGetFundDetails.Tables[0].Rows.Count > 0)
                            {
                                if (dsGetFundDetails.Tables[0].Rows[0]["lid"] != DBNull.Value)
                                {
                                    lblFundError.Text = "You already sent request for this job";
                                    lblFundError.CssClass = "errorMsg";
                                    txtAmount.Text = dsGetFundDetails.Tables[0].Rows[0]["Amount"].ToString();
                                    txtPaidTo.Text = dsGetFundDetails.Tables[0].Rows[0]["PaidTo"].ToString();
                                    txtRemark.Text = dsGetFundDetails.Tables[0].Rows[0]["Remark"].ToString();
                                    btnSubmit.Visible = false;
                                    btnCancel.Visible = false;
                                    ddlPaymentType.Enabled = false;
                                    txtAmount.Enabled = false;
                                    txtPaidTo.Enabled = false;
                                    txtRemark.Enabled = false;
                                }
                            }
                        }
                        #endregion

                    }
                }
            }
        }

        if (count > 0)
        {
            if (lblBankName.Text.Trim() == "" && lblAccountNo.Text.Trim() == "")
            {
                btnSubmit.Visible = false;
                btnCancel.Visible = false;
                lblFundError.Text = "Please update bank details for transporter before sending fund request!!";
                lblFundError.CssClass = "errorMsg";
            }
            mpeFundRequest.Show();
        }
        else if (count == -123)
        {
            mpeFundRequest.Hide();
            lblError_RateDetail.Text = "Fund request can only be allowed for same transporter!";
            lblError_RateDetail.CssClass = "errorMsg";
        }
    }

    protected void gvPopup_Vehicle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower().Trim() == "memocopy")
        {
            if (e.CommandArgument.ToString() != "")
            {
                DownloadMemoCopy(e.CommandArgument.ToString());
            }
            mpeFundRequest.Show();
        }
    }


    #region SUPPORTING DOCUMENT

    protected void btnSaveDocument2_Click(object sender, EventArgs e)
    {
        mpeFundRequest.Show();
        int PkId = 1, OriginalRows = 0, AfterInsertedRows = 0;
        string fileName = "";

        if (fuDocument2 != null && fuDocument2.HasFile)
            fileName = UploadFiles(fuDocument2);

        if (fileName != "")
        {
            DataTable dtAnnexure = (DataTable)ViewState["AnnexureDoc2"];
            if (dtAnnexure != null && dtAnnexure.Rows.Count > 0)
            {
                for (int i = 0; i < dtAnnexure.Rows.Count; i++)
                {
                    if (dtAnnexure.Rows[i]["PkId"] != null)
                    {
                        PkId = Convert.ToInt32(dtAnnexure.Rows[i]["PkId"].ToString());
                        PkId++;
                    }
                }
            }
            if (dtAnnexure != null)
                OriginalRows = dtAnnexure.Rows.Count;              //get original rows of grid view.

            dtAnnexure.Rows.Add(PkId, fileName, fuDocument2.FileName, LoggedInUser.glUserId);
            AfterInsertedRows = dtAnnexure.Rows.Count;              //get present rows after deleting particular row from grid view.
            ViewState["AnnexureDoc2"] = dtAnnexure;
            BindGrid();
            if (OriginalRows < AfterInsertedRows)
            {
                lblFundError.Text = "Document Added successfully!";
                lblFundError.CssClass = "success";
            }
            else
            {
                lblFundError.Text = "System Error! Please Try After Sometime.";
                lblFundError.CssClass = "errorMsg";
            }
        }
    }

    protected void rptDocument2_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        mpeFundRequest.Show();
        if (e.CommandName == "Delete")
        {
            int OriginalRows = 0, AfterDeletedRows = 0;
            HiddenField hdnDocLid = (HiddenField)e.Item.FindControl("hdnDocLid");
            LinkButton lnkDownload = (LinkButton)e.Item.FindControl("lnkDownload");
            DataTable dt = ViewState["AnnexureDoc2"] as DataTable;
            OriginalRows = dt.Rows.Count;       // get original rows of grid view

            DataRow[] drr = dt.Select("PkId='" + hdnDocLid.Value + "' "); // get particular row id to be deleted
            foreach (var row in drr)
                row.Delete(); // delete the row

            AfterDeletedRows = dt.Rows.Count;   // get present rows after deleting particular row from grid view
            ViewState["AnnexureDoc2"] = dt;
            BindGrid();
            if (OriginalRows > AfterDeletedRows)
            {
                lblFundError.Text = "Successfully Deleted Document.";
                lblFundError.CssClass = "success";
                rptDocument2.DataBind();
            }
            else
            {
                lblFundError.Text = "Error while deleting container details. Please try again later..!!";
                lblFundError.CssClass = "success";
            }
        }
        if (e.CommandName.ToLower().Trim() == "downloadfile")
        {
            LinkButton DownloadPath = (LinkButton)e.Item.FindControl("lnkDownload");
            string FilePath = e.CommandArgument.ToString();
            DownloadDoc(FilePath);
        }
    }

    private string UploadFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "Expense_" + hdnNewPaymentLid.Value + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    protected void DownloadDoc(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ExpenseUpload\\" + DocumentPath);
        else
            ServerPath = ServerPath + DocumentPath;
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }

    protected void BindGrid()
    {
        if (ViewState["AnnexureDoc2"].ToString() != "")
        {
            DataTable dtAnnexureDoc = (DataTable)ViewState["AnnexureDoc2"];
            rptDocument2.DataSource = dtAnnexureDoc;
            rptDocument2.DataBind();
        }
    }

    #endregion

    #endregion

    #region UPLOAD DOCUMENTS
    private void DownloadMemoCopy(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    public string UploadMemoFile(FileUpload fuDocument, string FilePath)
    {
        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();
        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }

        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;

            }

            fuDocument.SaveAs(ServerFilePath + FileName);
        }

        return FilePath + FileName;
    }

    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }

    #endregion

    #region UPDATE PLANNING DETAILS
    protected void gvPopup_JobDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPopup_JobDetail.EditIndex = e.NewEditIndex;
        mpeFundRequest.Show();
    }

    protected void gvPopup_JobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPopup_JobDetail.EditIndex = -1;
        mpeFundRequest.Show();
    }

    protected void gvPopup_JobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int JobId = Convert.ToInt32(gvPopup_JobDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtPlanningDate = (TextBox)gvPopup_JobDetail.Rows[e.RowIndex].FindControl("txtPlanningDate");

        DateTime PlanningDate = DateTime.MinValue;
        PlanningDate = Commonfunctions.CDateTime(txtPlanningDate.Text.Trim());

        if (PlanningDate != DateTime.MinValue)
        {
            int result = AccountExpense.AddExamineDetails(JobId, PlanningDate, LoggedInUser.glUserId);
            if (result == 0)
            {
                lblError.Text = "Planning Date Updated Successfully.";
                lblError.CssClass = "success";

                gvPopup_JobDetail.EditIndex = -1;
                e.Cancel = true;
            }
        }
        mpeFundRequest.Show();
    }

    #endregion

    private string UploadEmailFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "EmailApprovalUpload\\EmailApproval_" + hdnNewPaymentLid.Value + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\EmailApprovalUpload\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    private string UploadContractFiles(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        if (FilePath == "")
            FilePath = "TransportContractCopyUpload\\TransportContractCopy_" + hdnNewPaymentLid.Value + "\\";

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportContractCopyUpload\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    protected void DisplaySellingDetails()
    {
        //trsellDetail.Visible = true;
        if (rdlRequestReceive.SelectedValue == "1")
        {
            trContractDetail.Visible = true;
            trEmailDetail.Visible = false;
        }
        else if (rdlRequestReceive.SelectedValue == "2")
        {
            trContractDetail.Visible = false;
            trEmailDetail.Visible = true;
        }
    }

    protected void rdlRequestReceive_SelectedIndexChanged(object sender, EventArgs e)
    {
        DisplaySellingDetails();
    }

    protected bool DecideHere(string Str)
    {
        if (Str == "" || Str=="0")
            return false;
        else
            return true;
    }
    private void DownloadEmailCopy(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath("UploadFiles\\EmailApprovalUpload\\" + DocumentPath);
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

    private void DownloadContractCopy(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath("UploadFiles\\TransportContractCopyUpload\\" + DocumentPath);
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