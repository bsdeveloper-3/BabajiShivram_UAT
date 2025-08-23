using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
public partial class AccountTransport_BillSubmission : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        MEditValBillDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");

        if (LoggedInUser.glUserId == 0 || Session["TransReqId"] == null)
        {
            Response.Redirect("PendingTransBill.aspx");
        }

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Bill Detail";
            if (Session["TransReqId"] != null)
            {
                TruckRequestDetail(Convert.ToInt32(Session["TransReqId"]));
                JobDetailMS(Convert.ToInt32(Session["JobId"]));
            }
            else
            {
                Session["TransReqId"] = null;
                Session["JobId"] = null;
                Session["TransporterId"] = null;
                Response.Redirect("PendingTransBill.aspx");
            }
        }
        
    }
    private void TruckRequestDetail(int TranRequestId)
    {
        DataView dvDetail = DBOperations.GetTransportRequest(TranRequestId);
        if (dvDetail.Table.Rows.Count > 0)
        {
            Session["JobId"] = dvDetail.Table.Rows[0]["JobId"].ToString();

            //lblTRRefNo.Text = dvDetail.Table.Rows[0]["TRRefNo"].ToString();
            //Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            //lblTitle.Text = "Bill Detail - " + lblTRRefNo.Text;

            //lblTruckRequestDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["RequestDate"]).ToString("dd/MM/yyyy");
            //lblJobNo.Text = dvDetail.Table.Rows[0]["JobRefNo"].ToString();
            //lblCustName.Text = dvDetail.Table.Rows[0]["CustName"].ToString();
            //if (dvDetail.Table.Rows[0]["VehiclePlaceDate"] != DBNull.Value)
            //    lblVehiclePlaceDate.Text = Convert.ToDateTime(dvDetail.Table.Rows[0]["VehiclePlaceDate"]).ToString("dd/MM/yyyy");
            //lblDispatch_Title.Text = dvDetail.Table.Rows[0]["DispatchTitle"].ToString();
            //lblDispatch_Value.Text = dvDetail.Table.Rows[0]["DispatchValue"].ToString();
            //lblLocationFrom.Text = dvDetail.Table.Rows[0]["LocationFrom"].ToString();
            //lblDestination.Text = dvDetail.Table.Rows[0]["Destination"].ToString();
            //lblDimension.Text = dvDetail.Table.Rows[0]["Dimension"].ToString();
            //lblGrossWeight.Text = dvDetail.Table.Rows[0]["GrossWeight"].ToString();
            //lblCon20.Text = dvDetail.Table.Rows[0]["Count20"].ToString();
            //lblCon40.Text = dvDetail.Table.Rows[0]["Count40"].ToString();
            //lblDelExportType_Title.Text = dvDetail.Table.Rows[0]["DelExportType_Title"].ToString();
            //lblDelExportType_Value.Text = dvDetail.Table.Rows[0]["DelExportType_Value"].ToString();
            //lblConsigneeName.Text = dvDetail.Table.Rows[0]["ConsigneeName"].ToString();
            //GridViewVehicle.DataBind();
        }
    }
    private void JobDetailMS(int JobId)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        DataSet dsJobDetail = DBOperations.GetJobDetailForTransport(JobId);
        if (dsJobDetail.Tables[0].Rows.Count == 0)
        {
            Session["TransReqId"] = null;
            Session["JobId"] = null;
            Session["TransporterId"] = null;

            Response.Redirect("PendingTransBill.aspx");
            
        }

        lblTitle.Text = "Bill Detail - " + dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();

        lblJobNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
        lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
        lblDestination.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryDestination"].ToString();
    }
    protected void btnBackToJobDet_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("PendingTransBill.aspx");
    }

    #region Bill Submission
    protected void btnBillSubmit_Click(object sender, EventArgs e)
    {
        bool isValidBill = ValidateBill();

        if (isValidBill == true)
        {
            int resultRate = -1;

            bool IsValid = true;
            int TransReqId = 0, TransporterID = 0, TransitDays = 0;
            string BillNumber = "", BillAmount = "", Remark = "";
            Decimal FreightRateTotal = 0, DetentionTotal = 0, VaraiTotal = 0, EmptyTotal = 0;
            Decimal TotalAmount = 0, TollTotal = 0, OtherTotal = 0;

            string BillPersonName = "", fileName = "", filePath = "";

            DateTime BillSubmitDate = DateTime.Now, BillDate = DateTime.MinValue;

            if (fuDocument != null && fuDocument.HasFile)
            {
                fileName = UploadFiles(fuDocument, "");
                filePath = "\\Transport\\" + fileName;
            }

            TransporterID = Convert.ToInt32(Session["TransporterId"]);
            BillNumber = txtBillNo.Text.Trim();
            Decimal.TryParse(txtTotalAmount.Text.Trim(), out TotalAmount);
            Remark = txtJustification.Text.Trim();

            if (txtBillDate.Text.Trim() != "")
            {
                BillDate = Commonfunctions.CDateTime(txtBillDate.Text.Trim());
            }

            // Check Vehicle Rate with Total Bill Amount

            foreach (GridViewRow gr in GridViewVehicle.Rows)
            {
                int VehBillAmount = 0;

                if (gr.RowType == DataControlRowType.DataRow)
                {
                    int RateId = 0;
                    string VehicleNo = "";
                    Decimal Rate = 0, VaraExp = 0,
                        DetentionCharges = 0, EmptyCharges = 0, TollCharges = 0, OtherCharges = 0;

                    string LRCopy = "", ChallanCopy = "", EmptyReceiptCopy = "";

                    if (Session["TransReqId"] != null && Session["TransporterId"] != null)
                    {

                        RateId = Convert.ToInt32(GridViewVehicle.DataKeys[gr.RowIndex].Value);
                        TransReqId = Convert.ToInt32(Session["TransReqId"]);
                        VehicleNo = ((Label)gr.FindControl("lblVehicleNo")).Text;

                        Decimal.TryParse(((TextBox)gr.FindControl("lblRate")).Text.Trim(), out Rate);

                        Decimal.TryParse(((TextBox)gr.FindControl("txtDetention")).Text.Trim(), out DetentionCharges);
                        Decimal.TryParse(((TextBox)gr.FindControl("txtVarai")).Text.Trim(), out VaraExp);
                        Decimal.TryParse(((TextBox)gr.FindControl("txtEmptyContRecpt")).Text.Trim(), out EmptyCharges);
                        Decimal.TryParse(((TextBox)gr.FindControl("txtToll")).Text.Trim(), out TollCharges);
                        Decimal.TryParse(((TextBox)gr.FindControl("txtOtherCharges")).Text.Trim(), out OtherCharges);

                        FreightRateTotal = FreightRateTotal + Rate;
                        DetentionTotal = DetentionTotal + DetentionCharges;
                        VaraiTotal = VaraiTotal + VaraExp;
                        EmptyTotal = EmptyTotal + EmptyCharges;
                        TollTotal = TollTotal + TollCharges;
                        OtherTotal = OtherTotal + OtherCharges;

                        FileUpload objLRCopy = (FileUpload)gr.FindControl("updLRCopy");
                        FileUpload objChallanCopy = (FileUpload)gr.FindControl("updChallanCopy");
                        FileUpload objEptyReceipt = (FileUpload)gr.FindControl("updEptyReceipt");

                        if (objLRCopy.HasFile)
                            LRCopy = UploadFiles(objLRCopy, "");
                        if (objChallanCopy.HasFile)
                            ChallanCopy = UploadFiles(objChallanCopy, "");
                        if (objEptyReceipt.HasFile)
                            EmptyReceiptCopy = UploadFiles(objEptyReceipt, "");

                        resultRate = DBOperations.UpdateTransportRate(TransReqId, RateId, VehicleNo, Rate, Rate,
                            DetentionCharges, VaraExp, EmptyCharges, TollCharges, OtherCharges,
                            LRCopy, ChallanCopy, EmptyReceiptCopy, LoggedInUser.glUserId);

                        if (resultRate == 0)
                        {
                            lblError.Text = "Successfully updated rate detail.";
                            lblError.CssClass = "success";
                        }
                        else if (resultRate == 2)
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
                }//END_IF
            }

            if (resultRate == 0)
            {
                int result_BillID = DBOperations.AddTransBillDetail(TransReqId, TransporterID, TransitDays, BillSubmitDate, BillNumber, BillDate, FreightRateTotal.ToString(),
                    DetentionTotal.ToString(), VaraiTotal.ToString(), EmptyTotal.ToString(), TollTotal.ToString(), OtherTotal.ToString(), TotalAmount.ToString(),
                    BillPersonName, IsValid, txtJustification.Text.Trim(), false, 0, fileName, filePath, LoggedInUser.glUserId);
                
                if (result_BillID > 0)
                {
                    // Send Bill to Transport Dept Approval

                    int AddHistory = DBOperations.AddTransBillApprovalHistory(result_BillID, 0, Remark, LoggedInUser.glUserId);

                    if (AddHistory == 0)
                    {
                        lblError.Text = "Bill Detail Added Successfully.!";
                        lblError.CssClass = "success";

                        Session["Success"] = "Bill Detail Added Successfully.!";

                        Response.Redirect("TransportSuccess.aspx");

                        //int result_BillNonReceive = DBOperations.AddBillReceivedDetail(result_BillID, LoggedInUser.glUserId, 0, DateTime.Now, DateTime.MinValue, 0, "", DateTime.MinValue, "", DateTime.MinValue, LoggedInUser.glUserId);
                        //if (result_BillNonReceive == 0)
                        //{
                        //    Response.Redirect("SuccessPage.aspx?Bill=122");
                        //}
                        //else
                        //{
                        //    lblError.Text = "Error while adding bill detail. Please try again later.";
                        //    lblError.CssClass = "errorMsg";
                        //}
                    }
                    else
                    {
                        lblError.Text = "System Error! Please Try After Sometime.";
                        lblError.CssClass = "errorMsg";
                    }

                }
                else if (result_BillID == -1)
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }
                else if (result_BillID == -2)
                {
                    lblError.Text = "Billing Detail Already Updated!";
                    lblError.CssClass = "errorMsg";
                }
                else if (result_BillID == -3)
                {
                    lblError.Text = "Billing Detail Already Submitted!";
                    lblError.CssClass = "errorMsg";
                }
                else
                {
                    lblError.Text = "System Error! Please Try After Sometime.";
                    lblError.CssClass = "errorMsg";
                }

            }
        }
    }

    private bool ValidateBill()
    {
        bool isValidAmount = true;

        decimal TotalAmount = 0, VehicleTotal = 0;
        decimal FreightRateTotal = 0, DetentionTotal = 0, VaraiTotal = 0, EmptyTotal = 0;
        decimal TollTotal = 0, OtherTotal = 0;

        Decimal.TryParse(txtTotalAmount.Text.Trim(), out TotalAmount);

        // Calculate Vehicle Total Amount

        foreach (GridViewRow gr in GridViewVehicle.Rows)
        {
            if (gr.RowType == DataControlRowType.DataRow)
            {
                Decimal Rate = 0, VaraExp = 0, DetentionCharges = 0, EmptyCharges = 0; 
                Decimal TollCharges = 0, OtherCharges = 0;

                Decimal.TryParse(((TextBox)gr.FindControl("lblRate")).Text.Trim(), out Rate);

                Decimal.TryParse(((TextBox)gr.FindControl("txtDetention")).Text.Trim(), out DetentionCharges);
                Decimal.TryParse(((TextBox)gr.FindControl("txtVarai")).Text.Trim(), out VaraExp);
                Decimal.TryParse(((TextBox)gr.FindControl("txtEmptyContRecpt")).Text.Trim(), out EmptyCharges);
                Decimal.TryParse(((TextBox)gr.FindControl("txtToll")).Text.Trim(), out TollCharges);
                Decimal.TryParse(((TextBox)gr.FindControl("txtOtherCharges")).Text.Trim(), out OtherCharges);

                FreightRateTotal = FreightRateTotal + Rate;
                DetentionTotal = DetentionTotal + DetentionCharges;
                VaraiTotal = VaraiTotal + VaraExp;
                EmptyTotal = EmptyTotal + EmptyCharges;
                TollTotal = TollTotal + TollCharges;
                OtherTotal = OtherTotal + OtherCharges;
                 

            }//END_IF
        }//END_ForEach

        VehicleTotal = FreightRateTotal + DetentionTotal + VaraiTotal + EmptyTotal + TollTotal + OtherTotal;
        
        if (TotalAmount != VehicleTotal)
        {
            isValidAmount = false;
            lblError.Text = "Bill Amount - " + TotalAmount.ToString() + " and Vehicle Detail Amount - " + VehicleTotal.ToString() + " Not Matched.";
            lblError.CssClass = "errorMsg";
        }
        else if(TotalAmount == 0 || VehicleTotal ==0)
        {
            isValidAmount = false;
            lblError.Text = "Bill Amount and Vehicle Detail Amount Not Matched.";
            lblError.CssClass = "errorMsg";
        }
        else if (txtBillNo.Text.Trim() == "")
        {
            isValidAmount = false;
            lblError.Text = "Please Enter Bill Number!";
            lblError.CssClass = "errorMsg";
        }
        else if (txtBillDate.Text.Trim() == "")
        {
            isValidAmount = false;
            lblError.Text = "Please Enter Bill Date!";
            lblError.CssClass = "errorMsg";
        }

        return isValidAmount;
    }
    protected void btnCancelBill_Click(object sender, EventArgs e)
    {
        Session["TransReqId"] = null;
        Session["JobId"] = null;
        Session["TransporterID"] = null;

        Response.Redirect("PendingTransBill.aspx");

    }
    protected string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        FileName = FileServer.ValidateFileName(FileName);

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
    protected void GetTotalAmount()
    {
        decimal dcFreightAmt = 0, dcDetentionAmt = 0, dcVaraiExpense = 0, dcEmptyContCharges = 0, dcAdvanceAmt = 0, dcTollCharges = 0, dcOtherCharges = 0;
        decimal dcActualFreightAmt = 0, dcActualDetentionAmt = 0, dcActualVaraiAmt = 0, dcActualEmptyContAmt = 0, dcActualTollCharges = 0, dcActualOtherCharges = 0;

        // set page is valid or not
        if (hdnFreightAmt.Value != "" && hdnFreightAmt.Value != "0")
            dcActualFreightAmt = Convert.ToDecimal(hdnFreightAmt.Value);
        if (hdnDetentionAmt.Value != "" && hdnDetentionAmt.Value != "0")
            dcActualDetentionAmt = Convert.ToDecimal(hdnDetentionAmt.Value);
        if (hdnVaraiAmt.Value != "" && hdnVaraiAmt.Value != "0")
            dcActualVaraiAmt = Convert.ToDecimal(hdnVaraiAmt.Value);
        if (hdnEmptyContReturnAmt.Value != "" && hdnEmptyContReturnAmt.Value != "0")
            dcActualEmptyContAmt = Convert.ToDecimal(hdnEmptyContReturnAmt.Value);
        if (hdnTollCharges.Value != "" && hdnTollCharges.Value != "0")
            dcActualTollCharges = Convert.ToDecimal(hdnTollCharges.Value);
        if (hdnOtherCharges.Value != "" && hdnOtherCharges.Value != "0")
            dcActualOtherCharges = Convert.ToDecimal(hdnOtherCharges.Value);

        if (hdnAdvanceAmt.Value != "" && hdnAdvanceAmt.Value != "0")
            dcAdvanceAmt = Convert.ToDecimal(hdnAdvanceAmt.Value);

        txtTotalAmount.Text = Convert.ToDecimal((dcFreightAmt + dcDetentionAmt + dcVaraiExpense + dcEmptyContCharges + dcTollCharges + dcOtherCharges) - (dcAdvanceAmt)).ToString();
        txtTotalAmount.Enabled = false;
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
    #endregion

    #region GridView Event
    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int dcFreightAmt = 0, dcVaraiExpenses = 0, dcDetentionAmt = 0, dcEmptyContRecdCharges = 0,
            dcTollCharges = 0, dcOtherCharges = 0, dcAdvanceAmount = 0, dcTotalAmount = 0;

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
            ViewState["OtherCharges"] = Convert.ToDecimal(ViewState["OtherCharges"]) + dcOtherCharges;
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
            e.Row.Cells[3].Text = "<b>" + ViewState["FreightAmt"].ToString() + "</b>";
            e.Row.Cells[4].Text = "<b>" + ViewState["AdvanceAmt"].ToString() + "</b>";
            e.Row.Cells[5].Text = "<b>" + ViewState["DetentionAmt"].ToString() + "</b>";
            e.Row.Cells[6].Text = "<b>" + ViewState["VaraiAmt"].ToString() + "</b>";
            e.Row.Cells[7].Text = "<b>" + ViewState["EmptyContReturnAmt"].ToString() + "</b>";
            e.Row.Cells[8].Text = "<b>" + ViewState["TollCharges"].ToString() + "</b>";
            e.Row.Cells[9].Text = "<b>" + ViewState["OtherCharges"].ToString() + "</b>";
        }
    }
    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }
    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int TransRateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        //int TranRequestId = Convert.ToInt32(Session["TransReqId"]);

        //Label lblVehicleNo = (Label)GridViewVehicle.Rows[e.RowIndex].FindControl("lblVehicleNo");
        //TextBox txtRate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtRate");

        //string strVehicleNo = lblVehicleNo.Text.Trim();
        //int Amount = Convert.ToInt32(txtRate.Text.Trim());

        int result = 0;// DBOperations.AddTransportRate(TransRateId, TranRequestId, strVehicleNo, Amount, LoggedInUser.glUserId);
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
}