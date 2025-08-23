using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ClosedXML.Excel;
public partial class Transport_EWayBillNew : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnEwayJson);
        ScriptManager1.RegisterPostBackControl(btnEwayExcel);
        JobDetailExtender.ContextKey = Convert.ToString(LoggedInUser.glUserId);

        CalTransportDate.EndDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));

        if (!IsPostBack)
        {

            DBOperations.FillTransSubType(ddSubType, 1);

            ddSubType.SelectedValue = "2";

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "E-Way Bill JSon File";

            DBOperations.FillStateGSTID(ddState);
            DBOperations.FillStateGSTID(ddBillFromState);
            DBOperations.FillStateGSTID(ddDespatchFromState);
            DBOperations.FillStateGSTID(ddBillToState);

            ddDespatchFromState.Items[0].Text = "-Supplier State-";
            ddState.Items[0].Text = "-Select State-";
            ddDespatchFromState.Items[0].Text = "-Dispatch State-";

            //ListItem lstOtherCountry = new ListItem("Other Countries", "99");

            //ddBillFromState.Items.Add(lstOtherCountry);
            ddBillFromState.SelectedValue = "99";

            //ddDespatchFromState.Items.Add(lstOtherCountry);
            ddDespatchFromState.SelectedValue = "99";

            //ddBillToState.Items.Add(lstOtherCountry);
            ddBillToState.SelectedIndex = 0;
        }

        Page.Validate();
    }
    private void GetJobDetail(int JobId)
    {
        ddBillToState.SelectedIndex = 0;

        DataSet dsJobDetail = DBOperations.GetJobDetailByJobId(JobId);
        int InvoiceCount = DBOperations.GetInvoiceCountByJobId(JobId);

        lblNoOfInvoices.Text = InvoiceCount.ToString();

        if (dsJobDetail.Tables.Count > 0)
        {
            if (dsJobDetail.Tables[0].Rows[0]["JobRefNo"] != DBNull.Value)
            {
                lblJobRefNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["TransMode"] != DBNull.Value)
            {
                hdnTransMode.Value = dsJobDetail.Tables[0].Rows[0]["TransMode"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["BOENo"] != DBNull.Value)
            {
                lblBOENo.Text = dsJobDetail.Tables[0].Rows[0]["BOENo"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["BOEDate"] != DBNull.Value)
            {
                lblBOEDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["BOEDate"]).ToString("dd/MM/yyyy");
            }
            if (dsJobDetail.Tables[0].Rows[0]["Supplier"] != DBNull.Value)
            {
                txtBillFromName.Text = dsJobDetail.Tables[0].Rows[0]["Supplier"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["Consignee"] != DBNull.Value)
            {
                txtBillToName.Text = dsJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"] != DBNull.Value)
            {
                txtBillToGSTIN.Text = dsJobDetail.Tables[0].Rows[0]["ConsigneeGSTIN"].ToString();

                if (txtBillToGSTIN.Text.Length > 10)
                {
                    string strConsigneeState = txtBillToGSTIN.Text.Trim().Substring(0, 2);

                    int intStateCode = 0;

                    Int32.TryParse(strConsigneeState, out intStateCode);

                    ListItem lstConsState = ddBillToState.Items.FindByValue(intStateCode.ToString());

                    if (lstConsState != null)
                    {
                        ddBillToState.SelectedValue = intStateCode.ToString();
                    }
                }
            }
            if (dsJobDetail.Tables[0].Rows[0]["DeliveryType"] != DBNull.Value)
            {
                lblDeliveryType.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryType"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["DeliveryTypeID"] != DBNull.Value)
            {
                hdnDeliveryTypeId.Value = dsJobDetail.Tables[0].Rows[0]["DeliveryTypeID"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["TransportationByBabaji"] != DBNull.Value)
            {
                bool TransByBabaji = Convert.ToBoolean(dsJobDetail.Tables[0].Rows[0]["TransportationByBabaji"]);
                if (TransByBabaji == true)
                {
                    lblTransportationBy.Text = "Babaji";
                }
                else
                {
                    lblTransportationBy.Text = "Customer";
                }
            }

            if (dsJobDetail.Tables[0].Rows[0]["CountTotal"] != DBNull.Value)
            {
                string strContainerType = "FCL";

                if (dsJobDetail.Tables[0].Rows[0]["CountTotal"].ToString() == "0")
                    strContainerType = "";
                else if (dsJobDetail.Tables[0].Rows[0]["CountLCL"].ToString() == "1")
                    strContainerType = "LCL";

                lblContainerCount.Text = dsJobDetail.Tables[0].Rows[0]["CountTotal"].ToString();
                lblContainerType.Text = strContainerType;

            }
            if (dsJobDetail.Tables[0].Rows[0]["NoOfPackages"] != DBNull.Value)
            {
                lblNoOfPackages.Text = dsJobDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
                lblPackageType.Text = dsJobDetail.Tables[0].Rows[0]["PackageTypeName"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["GrossWT"] != DBNull.Value)
            {
                lblGrossWeight.Text = dsJobDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["ShortDescription"] != DBNull.Value)
            {
                lblShortDesc.Text = dsJobDetail.Tables[0].Rows[0]["ShortDescription"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["TruckRequestDate"] != DBNull.Value)
            {
                lblTruckReqDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["TruckRequestDate"]).ToString("dd/MM/yyyy");
            }
            if (dsJobDetail.Tables[0].Rows[0]["Port"] != DBNull.Value)
            {
                lblDeliveryFrom.Text = dsJobDetail.Tables[0].Rows[0]["Port"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["CFS"] != DBNull.Value)
            {
                lblCFS.Text = dsJobDetail.Tables[0].Rows[0]["CFS"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["DeliveryAddress"] != DBNull.Value)
            {
                txtDeliveryAddress.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryAddress"].ToString();
            }
            if (dsJobDetail.Tables[0].Rows[0]["DeliveryDestination"] != DBNull.Value)
            {
                txtDeliveryDestination.Text = dsJobDetail.Tables[0].Rows[0]["DeliveryDestination"].ToString();
            }

            // Set Vehicle Count For Loaded Delivery

            if (Convert.ToInt32(hdnDeliveryTypeId.Value) == (Int32)DeliveryType.Loaded && hdnTransMode.Value == "2")
            {
                //   txtVehicleCount.Text = lblContainerCount.Text;
            }
        }
    }
    private void ResetData()
    {
        ViewState["vwsFinalEway"] = null;

        // Reset Wizard Active Index

        wzDelivery.ActiveStepIndex = 0;

        // Clear Eway Bill Data

        gvProductDispatch.DataSource = null;
        gvProductDispatch.DataBind();
    }
    private void ShowJobDetail()
    {        
        int JobId = 0;

        if (txtJobNumber.Text.Trim() != "")
        {
            if (txtJobNumber.Text.Length == 7)
            {
                int BOEJobId = DBOperations.GetJobIDByBOENo(txtJobNumber.Text.Trim());

                if (BOEJobId > 0)
                {
                    hdnJobId.Value = BOEJobId.ToString();
                }
            }

            if (hdnJobId.Value != "" && hdnJobId.Value != "0")
            {
                JobId = Convert.ToInt32(hdnJobId.Value);

                GetJobDetail(JobId);

                // BindVehicle();

                BindProduct(JobId, true); // IGST

            }
        }
    }
    private void BindProduct(int JobId, bool IsIGST)
    {
        DataSet dsInvoiceProduct = DBOperations.GetProductInvoiceDetailHSN(JobId, IsIGST);

        GridView1.DataSource = dsInvoiceProduct;
        GridView1.DataBind();
    }
    private void BindProductGroupHSN(int JobId, bool IsIGST)
    {
        DataSet dsInvoiceProductHSN = DBOperations.GetProductInvoiceDetailGroupHSN(JobId, IsIGST);

        GridView1.DataSource = dsInvoiceProductHSN;
        GridView1.DataBind();

    }

    #region Event
    protected void txtJobNumber_TextChanged(object sender, EventArgs e)
    {
        ResetData();
        ShowJobDetail();
    }
    protected void btnShowJob_Click(object sender, EventArgs e)
    {
        ShowJobDetail();
    }
    protected void txtVehicleCount_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtDispatchQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

        CheckBox chk1 = (CheckBox)gvRow.FindControl("chk1");
        TextBox txtDispatchQuantity = (TextBox)gvRow.FindControl("txtDispatchQuantity");
        Label lblQuantity = (Label)gvRow.FindControl("lblQuantity");

        decimal decDispatch = 0m, decAvailable = 0m;

        if (chk1.Checked)
        {
            if (txtDispatchQuantity.Text.Trim() != "" && txtDispatchQuantity.Text.Trim() != "0")
            {
                decDispatch = Convert.ToDecimal(txtDispatchQuantity.Text.Trim());
                decAvailable = Convert.ToDecimal(lblQuantity.Text.Trim());

                if (decDispatch > decAvailable)
                {
                    hdnIsInvalidData.Value = "1"; // Invalid Data
                    ShowUserAlert("Invalid Quantity Selection!");
                }
            }
            else
            {
                ShowUserAlert("Please Select Quantity!");
            }
        }
    }

    #endregion

    #region dataTable
    private DataTable GetVehicleDataTable(int VehicleCount)
    {
        DataTable dtVehicle = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colVehicleNo = new DataColumn("VehicleNo", Type.GetType("System.String"));
        DataColumn colTransporter = new DataColumn("Transporter", Type.GetType("System.String"));
        DataColumn colTransporterID = new DataColumn("TransporterID", Type.GetType("System.String"));
        DataColumn colTransportDate = new DataColumn("TransportDate", Type.GetType("System.String"));

        dtVehicle.Columns.Add(colSL);
        dtVehicle.Columns.Add(colVehicleNo);
        dtVehicle.Columns.Add(colTransporter);
        dtVehicle.Columns.Add(colTransporterID);
        dtVehicle.Columns.Add(colTransportDate);

        for (int i = 1; i <= VehicleCount; i++)
        {
            DataRow dtRow = dtVehicle.NewRow();

            dtVehicle.Rows.Add(dtRow);
        }

        dtVehicle.AcceptChanges();

        return dtVehicle;
    }
    private DataTable GetBalanceDataTable()
    {
        DataTable dtBalProduct = new DataTable();

        DataColumn colLID = new DataColumn("ProductId", Type.GetType("System.Int32"));

        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colUnitOfProduct = new DataColumn("UnitOfProduct", Type.GetType("System.String"));
        DataColumn colQuantity = new DataColumn("Quantity", Type.GetType("System.String"));
        DataColumn colDescription = new DataColumn("Description", Type.GetType("System.String"));
        DataColumn colAssessableValue = new DataColumn("AssessableValue", Type.GetType("System.String"));
        DataColumn colGSTDutyRate = new DataColumn("GSTDutyRate", Type.GetType("System.String"));
        DataColumn colCGSTAmount = new DataColumn("CGSTAmount", Type.GetType("System.String"));
        DataColumn colSGSTAmount = new DataColumn("SGSTAmount", Type.GetType("System.String"));
        DataColumn colIGSTAmount = new DataColumn("IGSTAmount", Type.GetType("System.String"));
        DataColumn colCessAmount = new DataColumn("CessAmount", Type.GetType("System.String"));

        dtBalProduct.Columns.Add(colLID);
        dtBalProduct.Columns.Add(colHSN);
        dtBalProduct.Columns.Add(colUnitOfProduct);
        dtBalProduct.Columns.Add(colQuantity);
        dtBalProduct.Columns.Add(colDescription);
        dtBalProduct.Columns.Add(colAssessableValue);
        dtBalProduct.Columns.Add(colGSTDutyRate);
        dtBalProduct.Columns.Add(colCGSTAmount);
        dtBalProduct.Columns.Add(colSGSTAmount);
        dtBalProduct.Columns.Add(colIGSTAmount);
        dtBalProduct.Columns.Add(colCessAmount);

        dtBalProduct.AcceptChanges();

        return dtBalProduct;
    }
    private DataTable GetEwayBillDataTable()
    {
        DataTable dtEway = new DataTable();

        DataColumn colSL = new DataColumn("Sl", Type.GetType("System.Int32"));

        colSL.AutoIncrement = true;
        colSL.AutoIncrementSeed = 1;
        colSL.AutoIncrementStep = 1;

        DataColumn colProductId = new DataColumn("ProductID", Type.GetType("System.String"));
        DataColumn colVehicleID = new DataColumn("VehicleID", Type.GetType("System.String"));

        DataColumn colSupplyType = new DataColumn("Supply Type", Type.GetType("System.String"));
        DataColumn colSubType = new DataColumn("Sub Type", Type.GetType("System.String"));
        DataColumn colDocType = new DataColumn("Doc Type", Type.GetType("System.String"));
        DataColumn colDocNo = new DataColumn("Doc No", Type.GetType("System.String"));
        DataColumn colDocDate = new DataColumn("Doc Date", Type.GetType("System.String"));

        DataColumn colFrm_OtherPartyName = new DataColumn("Frm_OtherPartyName", Type.GetType("System.String"));
        DataColumn colFrom_GSTIN = new DataColumn("From_GSTIN", Type.GetType("System.String"));
        DataColumn colFrom_Address1 = new DataColumn("From_Address1", Type.GetType("System.String"));
        DataColumn colFrom_Address2 = new DataColumn("From_Address2", Type.GetType("System.String"));

        DataColumn colFrom_Place = new DataColumn("From_Place", Type.GetType("System.String"));
        DataColumn colFrom_PinCode = new DataColumn("From_PinCode", Type.GetType("System.String"));
        DataColumn colFrom_State = new DataColumn("From_State", Type.GetType("System.String"));

        DataColumn colTO_OtherPartyName = new DataColumn("TO_OtherPartyName", Type.GetType("System.String"));
        DataColumn colTO_GSTIN = new DataColumn("TO_GSTIN", Type.GetType("System.String"));
        DataColumn colTo_Address1 = new DataColumn("To_Address1", Type.GetType("System.String"));
        DataColumn colTo_Address2 = new DataColumn("To_Address2", Type.GetType("System.String"));
        DataColumn colTo_Place = new DataColumn("To_Place", Type.GetType("System.String"));
        DataColumn colTo_Pin_Code = new DataColumn("To_Pin_Code", Type.GetType("System.String"));
        DataColumn colTo_State = new DataColumn("To_State", Type.GetType("System.String"));

        DataColumn colProduct = new DataColumn("Product", Type.GetType("System.String"));
        DataColumn colDescription = new DataColumn("Description", Type.GetType("System.String"));
        DataColumn colHSN = new DataColumn("HSN", Type.GetType("System.String"));
        DataColumn colUnit = new DataColumn("Unit", Type.GetType("System.String"));
        DataColumn colQty = new DataColumn("Qty", Type.GetType("System.String"));
        DataColumn colAssessableValue = new DataColumn("AssessableValue", Type.GetType("System.String"));

        DataColumn colGSTDutyRate = new DataColumn("GSTDutyRate", Type.GetType("System.String"));
        DataColumn colCGSTAmount = new DataColumn("CGSTAmount", Type.GetType("System.String"));
        DataColumn colSGSTAmount = new DataColumn("SGSTAmount", Type.GetType("System.String"));
        DataColumn colIGSTAmount = new DataColumn("IGSTAmount", Type.GetType("System.String"));
        DataColumn colCessAmount = new DataColumn("CessAmount", Type.GetType("System.String"));

        DataColumn colTransMode = new DataColumn("Trans Mode", Type.GetType("System.String"));
        DataColumn colDistanceKM = new DataColumn("Distance km", Type.GetType("System.String"));
        DataColumn colTransName = new DataColumn("Trans Name", Type.GetType("System.String"));
        DataColumn colTransID = new DataColumn("Trans ID", Type.GetType("System.String"));
        DataColumn colTransDocNo = new DataColumn("Trans Doc No", Type.GetType("System.String"));
        DataColumn colTransDate = new DataColumn("Trans Date", Type.GetType("System.String"));
        DataColumn colVehicleNo = new DataColumn("Vehicle No", Type.GetType("System.String"));

        dtEway.Columns.Add(colSupplyType);
        dtEway.Columns.Add(colSubType);
        dtEway.Columns.Add(colDocType);
        dtEway.Columns.Add(colDocNo);
        dtEway.Columns.Add(colDocDate);

        dtEway.Columns.Add(colFrm_OtherPartyName);
        dtEway.Columns.Add(colFrom_GSTIN);
        dtEway.Columns.Add(colFrom_Address1);
        dtEway.Columns.Add(colFrom_Address2);
        dtEway.Columns.Add(colFrom_Place);
        dtEway.Columns.Add(colFrom_PinCode);
        dtEway.Columns.Add(colFrom_State);

        dtEway.Columns.Add(colTO_OtherPartyName);
        dtEway.Columns.Add(colTO_GSTIN);
        dtEway.Columns.Add(colTo_Address1);
        dtEway.Columns.Add(colTo_Address2);
        dtEway.Columns.Add(colTo_Place);
        dtEway.Columns.Add(colTo_Pin_Code);
        dtEway.Columns.Add(colTo_State);

        dtEway.Columns.Add(colProductId);
        dtEway.Columns.Add(colVehicleID);
        dtEway.Columns.Add(colSL);

        dtEway.Columns.Add(colProduct);
        dtEway.Columns.Add(colDescription);
        dtEway.Columns.Add(colHSN);
        dtEway.Columns.Add(colUnit);
        dtEway.Columns.Add(colQty);
        dtEway.Columns.Add(colAssessableValue);

        dtEway.Columns.Add(colGSTDutyRate);
        dtEway.Columns.Add(colCGSTAmount);
        dtEway.Columns.Add(colSGSTAmount);
        dtEway.Columns.Add(colIGSTAmount);
        dtEway.Columns.Add(colCessAmount);

        dtEway.Columns.Add(colTransMode);
        dtEway.Columns.Add(colDistanceKM);
        dtEway.Columns.Add(colTransName);
        dtEway.Columns.Add(colTransID);
        dtEway.Columns.Add(colTransDocNo);
        dtEway.Columns.Add(colTransDate);
        dtEway.Columns.Add(colVehicleNo);
        dtEway.AcceptChanges();

        return dtEway;
    }

    #endregion

    #region Export Data
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    private void ExportFunction(string header, string contentType)
    {

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";

        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        // gvProduct.Columns[1].Visible = false;
        // gvProduct.Columns[2].Visible = false;

        //gvProduct.DataBind();
        //gvProduct.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion   
    private void ShowUserAlert(string strMessage)
    {
        ScriptManager.RegisterStartupScript(upJobDetail, typeof(string), "Error Message", "alert('" + strMessage + "')", true);
    }

    #region Wizard Event
    protected void wzDelivery_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        btnEwayJson.Visible = false;
        btnEwayExcel.Visible = false;
        DataTable dtProduct; DataTable dtFinalEway;
        bool IsValid = false; int BalVehicle = 0;
        string strVehicleNo = "", strTranDocNo = "";
        int TotalVehicle = Convert.ToInt32(txtVehicleCount.Text.Trim());

        BalVehicle = ((TotalVehicle - 1) - e.CurrentStepIndex);

        WizardStepBase wsStep = wzDelivery.WizardSteps[e.CurrentStepIndex];

        if (wsStep.StepType == WizardStepType.Finish)
        {
            // Generate Json On Finish Button Click
            GenerateEwayJson();

            return;
        }
        else if (e.CurrentStepIndex == 0)
        {
            if (!Page.IsValid)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                strVehicleNo = txtVehicleNo1.Text.Trim();
                strTranDocNo = txtTransDocNo1.Text.Trim();

                dtProduct = GetBalanceProduct(GridView1, strVehicleNo, strTranDocNo, ref IsValid);

                if (dtProduct.Rows.Count > 0 && IsValid == true)
                {
                    GridView2.DataSource = dtProduct;
                    GridView2.DataBind();
                }
                else if (BalVehicle > 0)
                {
                    e.Cancel = true;
                    lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
                }
            }
        }
        else if (e.CurrentStepIndex == 1)
        {
            strVehicleNo = txtVehicleNo2.Text.Trim();
            strTranDocNo = txtTransDocNo2.Text.Trim();

            dtProduct = GetBalanceProduct(GridView2, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView3.DataSource = dtProduct;
                GridView3.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 2)
        {
            strVehicleNo = txtVehicleNo3.Text.Trim();
            strTranDocNo = txtTransDocNo3.Text.Trim();

            dtProduct = GetBalanceProduct(GridView3, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView4.DataSource = dtProduct;
                GridView4.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 3)
        {
            strVehicleNo = txtVehicleNo4.Text.Trim();
            strTranDocNo = txtTransDocNo4.Text.Trim();

            dtProduct = GetBalanceProduct(GridView4, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView5.DataSource = dtProduct;
                GridView5.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 4)
        {
            strVehicleNo = txtVehicleNo5.Text.Trim();
            strTranDocNo = txtTransDocNo5.Text.Trim();

            dtProduct = GetBalanceProduct(GridView5, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView6.DataSource = dtProduct;
                GridView6.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 5)
        {
            strVehicleNo = txtVehicleNo6.Text.Trim();
            strTranDocNo = txtTransDocNo6.Text.Trim();

            dtProduct = GetBalanceProduct(GridView6, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView7.DataSource = dtProduct;
                GridView7.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 6)
        {
            strVehicleNo = txtVehicleNo7.Text.Trim();
            strTranDocNo = txtTransDocNo7.Text.Trim();

            dtProduct = GetBalanceProduct(GridView7, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView8.DataSource = dtProduct;
                GridView8.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 7)
        {
            strVehicleNo = txtVehicleNo8.Text.Trim();
            strTranDocNo = txtTransDocNo8.Text.Trim();

            dtProduct = GetBalanceProduct(GridView8, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView9.DataSource = dtProduct;
                GridView9.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 8)
        {
            strVehicleNo = txtVehicleNo9.Text.Trim();
            strTranDocNo = txtTransDocNo9.Text.Trim();

            dtProduct = GetBalanceProduct(GridView9, strVehicleNo, strTranDocNo, ref IsValid);

            if (dtProduct.Rows.Count > 0 && IsValid == true)
            {
                GridView10.DataSource = dtProduct;
                GridView10.DataBind();
            }
            else if (BalVehicle > 0)
            {
                e.Cancel = true;
                lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
            }
        }
        else if (e.CurrentStepIndex == 9)
        {
            strVehicleNo = txtVehicleNo10.Text.Trim();
            strTranDocNo = txtTransDocNo10.Text.Trim();

            dtProduct = GetBalanceProduct(GridView10, strVehicleNo, strTranDocNo, ref IsValid);

            if (e.CurrentStepIndex < (TotalVehicle - 1))
            {
                if (dtProduct.Rows.Count > 0 && IsValid == true)
                {
                    e.Cancel = true;
                }
                else if (BalVehicle > 0)
                {
                    e.Cancel = true;
                    lblErrorWizard.Text = "No Product Balance For Next Vehicle!";
                }
            }
        }

        if (ViewState["vwsFinalEway"] != null)
        {
            dtFinalEway = (DataTable)ViewState["vwsFinalEway"];

            gvProductDispatch.DataSource = dtFinalEway;
            gvProductDispatch.DataBind();
        }

        if (e.CurrentStepIndex == (TotalVehicle - 1))
        {
            wzDelivery.MoveTo(this.WizardStepFinish);
            btnEwayJson.Visible = true;
            btnEwayExcel.Visible = true;
            return;
        }
    }
    protected void wzDelivery_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        GenerateEwayJson();

        if (Session["FilePath"] != null)
        {
            string strJsonPath = Session["FilePath"].ToString();

            if (strJsonPath != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "download",
                    "window.open('../Download.aspx', 'toolbar=no,location = no, status = no, menubar = no," +
                                    "scrollbars = no, resizable = yes, width = 350, height = 350');", true);
            }
        }

    }
    private DataTable GetBalanceProduct(GridView gvBalProduct, string strVehicleNo, string strTranDocNo, ref bool IsValid)
    {
        IsValid = false;
        bool isIGST = true;

        strVehicleNo = strVehicleNo.ToUpper();
        // Remove Space 
        strVehicleNo = strVehicleNo.Replace(" ", "");
        strVehicleNo = strVehicleNo.Replace("-", "");

        if (rblIGST.SelectedValue == "0")
        {
            isIGST = false;
        }

        DataTable dtFinalEway;

        DataTable dtBalProduct = GetBalanceDataTable();

        if (ViewState["vwsFinalEway"] != null)
        {
            dtFinalEway = (DataTable)ViewState["vwsFinalEway"];

            string searchPrevRow;
            searchPrevRow = "VehicleId='" + gvBalProduct.ID + "'";
            DataRow[] foundRows;

            foundRows = dtFinalEway.Select(searchPrevRow);

            if (foundRows.Length > 0)
            {
                // Remove Previous Row From ViewState for Product Grid
                for (int i = 0; i < foundRows.Length; i++)
                {
                    dtFinalEway.Rows.Remove(foundRows[i]);
                }

                dtFinalEway.AcceptChanges();
            }
        }
        else
        {
            dtFinalEway = GetEwayBillDataTable();

        }

        foreach (GridViewRow gvRow in gvBalProduct.Rows)
        {
            int ProductID = Convert.ToInt32(gvBalProduct.DataKeys[gvRow.RowIndex].Value);

            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chk1");

            //if (chkSelect.Checked)
            {
                DataRow dr = dtBalProduct.NewRow();

                DataRow drFinal = dtFinalEway.NewRow();

                decimal Quantity = 0m, DispatchQuantity = 0m, BalQuantity = 0m;

                TextBox txtDispatchQuantity = (TextBox)gvRow.FindControl("txtDispatchQuantity");

                Label lblQuantity = (Label)gvRow.FindControl("lblQuantity");
                TextBox txtDescription = (TextBox)gvRow.FindControl("txtDescription");

                Quantity = Convert.ToDecimal(lblQuantity.Text.ToString());

                if (chkSelect.Checked)
                {
                    DispatchQuantity = Convert.ToDecimal(txtDispatchQuantity.Text.ToString());

                    BalQuantity = (Quantity - DispatchQuantity);
                }
                else
                {
                    BalQuantity = Quantity;
                }
                // Add To Next GridView Data Table

                dr["Quantity"] = BalQuantity.ToString();
                dr["ProductID"] = ProductID.ToString();
                dr["HSN"] = gvRow.Cells[1].Text.Trim().Replace("&nbsp;", "");
                dr["UnitOfProduct"] = gvRow.Cells[2].Text.Trim().Replace("&nbsp;", "");
                dr["Description"] = txtDescription.Text.Trim().Replace("&nbsp;", "");
                dr["AssessableValue"] = gvRow.Cells[6].Text.Replace("&nbsp;", "");
                dr["GSTDutyRate"] = gvRow.Cells[7].Text.Trim().Replace("&nbsp;", "");
                dr["CGSTAmount"] = gvRow.Cells[8].Text.Trim().Replace("&nbsp;", "");
                dr["SGSTAmount"] = gvRow.Cells[9].Text.Trim().Replace("&nbsp;", "");
                dr["IGSTAmount"] = gvRow.Cells[10].Text.Trim().Replace("&nbsp;", "");

                // Calculate Item AssessableValue
                decimal dbAssessableValue = 0m; decimal itemAssessableValue = 0m; decimal BalAssessableValue = 0m;
                // Calculate IGST Duty Amount
                decimal dbDutyRate = 0m; decimal dbDutyAmount = 0m;
                decimal dbBalDutyAmount = 0m;
                Decimal.TryParse(gvRow.Cells[7].Text.Trim().Replace("&nbsp;", ""), out dbDutyRate);

                if (isIGST == true)
                {
                    Decimal.TryParse(gvRow.Cells[10].Text.Trim().Replace("&nbsp;", ""), out dbBalDutyAmount);
                }
                else
                {
                    Decimal.TryParse(gvRow.Cells[9].Text.Trim().Replace("&nbsp;", ""), out dbBalDutyAmount);
                }

                if (BalQuantity == Quantity)
                {
                    dr["AssessableValue"] = gvRow.Cells[6].Text.Trim().Replace("&nbsp;", "");
                }
                else
                {
                    Decimal.TryParse(dr["AssessableValue"].ToString(), out dbAssessableValue);

                    if (dbAssessableValue > 0)
                    {
                        BalAssessableValue = ((dbAssessableValue * BalQuantity) / Quantity);
                        itemAssessableValue = ((dbAssessableValue * DispatchQuantity) / Quantity);

                        BalAssessableValue = Math.Round(BalAssessableValue, 2);
                        itemAssessableValue = Math.Round(itemAssessableValue, 2);


                    }
                    if (dbBalDutyAmount > 0 && dbAssessableValue > 0)
                    {
                        dbDutyAmount = (itemAssessableValue * dbDutyRate) / 100;

                        dbDutyAmount = Math.Round(dbDutyAmount, 2);

                        dbBalDutyAmount = (BalAssessableValue * dbDutyRate) / 100;

                        dbBalDutyAmount = Math.Round(dbBalDutyAmount, 2);
                    }

                    dr["AssessableValue"] = BalAssessableValue.ToString("0.00");

                    if (isIGST == true)
                    {
                        dr["IGSTAmount"] = dbBalDutyAmount.ToString("0.00");

                        dr["CGSTAmount"] = "0"; ;
                        dr["SGSTAmount"] = "0";
                    }
                    else
                    {
                        dbBalDutyAmount = dbBalDutyAmount / 2;
                        dr["CGSTAmount"] = dbBalDutyAmount.ToString("0.00");
                        dr["SGSTAmount"] = dbBalDutyAmount.ToString("0.00");

                        dr["IGSTAmount"] = "0";
                    }
                }

                // Add To Final Bill Grid View Data Table
                drFinal["ProductId"] = ProductID.ToString();
                drFinal["VehicleID"] = gvBalProduct.ID;
                drFinal["Qty"] = DispatchQuantity.ToString();
                //drFinal["lid"]        =   ProductID.ToString();
                drFinal["HSN"] = gvRow.Cells[1].Text.Replace("&nbsp;", "");
                drFinal["Unit"] = gvRow.Cells[2].Text.Replace("&nbsp;", "");
                drFinal["Product"] = lblShortDesc.Text.Trim();
                drFinal["Description"] = txtDescription.Text;
                drFinal["AssessableValue"] = itemAssessableValue.ToString("0.00");
                drFinal["GSTDutyRate"] = gvRow.Cells[7].Text.Replace("&nbsp;", "");

                if (isIGST == true)
                {
                    drFinal["IGSTAmount"] = dbDutyAmount.ToString();

                    drFinal["CGSTAmount"] = "0";
                    drFinal["SGSTAmount"] = "0";
                }
                else
                {
                    dbDutyAmount = dbDutyAmount / 2;
                    drFinal["CGSTAmount"] = dbDutyAmount.ToString();
                    drFinal["SGSTAmount"] = dbDutyAmount.ToString();

                    drFinal["IGSTAmount"] = "0";
                }
                //
                drFinal["Supply Type"] = ddTransType.SelectedItem.Text;
                drFinal["Sub Type"] = ddSubType.SelectedItem.Text;
                drFinal["Doc Type"] = ddDocType.SelectedItem.Text;
                drFinal["Doc No"] = lblBOENo.Text.Trim();
                drFinal["Doc Date"] = lblBOEDate.Text.Trim();

                drFinal["Frm_OtherPartyName"] = txtBillFromName.Text.Trim(); // Supplier
                drFinal["From_GSTIN"] = txtBillFromGSTIN.Text.Trim(); //"URP";
                drFinal["From_Address1"] = txtDespatchFromAddr1.Text.Trim();
                drFinal["From_Address2"] = txtDespatchFromAddr2.Text.Trim();

                drFinal["From_Place"] = txtDespatchFromPlace.Text.Trim();
                drFinal["From_PinCode"] = txtDespatchFromPin.Text.Trim();
                drFinal["From_State"] = ddBillFromState.SelectedItem.Text;
                drFinal["TO_GSTIN"] = txtBillToGSTIN.Text.Trim(); // Consignee GSTIN

                drFinal["To_Address1"] = txtDeliveryAddress.Text.Trim();
                drFinal["To_Address2"] = "";
                drFinal["To_Place"] = txtDeliveryDestination.Text.Trim();
                drFinal["To_Pin_Code"] = txtPinCode.Text.Trim();
                drFinal["To_State"] = ddBillToState.SelectedItem.Text; // Consignee GSTN State

                drFinal["Trans Name"] = txtTransporterName.Text.Trim();
                drFinal["Trans ID"] = txtTransporterGSTIN.Text.Trim();
                drFinal["Trans Doc No"] = strTranDocNo;
                drFinal["Vehicle No"] = strVehicleNo;
                drFinal["Trans Mode"] = "Road";
                drFinal["Distance km"] = txtDistance.Text.ToString();
                drFinal["Trans Date"] = txtTransportDate.Text.ToString();

                string expression;
                expression = "ProductId=" + ProductID.ToString() + " AND VehicleId='" + gvBalProduct.ID + "'";
                DataRow[] foundRows;

                foundRows = dtFinalEway.Select(expression);

                if (chkSelect.Checked && BalQuantity >= 0)
                {
                    IsValid = true;
                    if (foundRows.Length == 0)
                    {
                        // Add New Row
                        dtFinalEway.Rows.Add(drFinal);
                        dtFinalEway.AcceptChanges();
                    }
                    else // Update Existing Row
                    {
                        foundRows[0]["Qty"] = DispatchQuantity.ToString();
                        foundRows[0]["Vehicle No"] = strVehicleNo;
                        foundRows[0]["AssessableValue"] = itemAssessableValue.ToString("0.00");

                        dtFinalEway.AcceptChanges();
                    }
                }
                else if (foundRows.Length > 0)
                {
                    // Check If Product added before and removed later

                    dtFinalEway.Rows.Remove(foundRows[0]);
                    dtFinalEway.AcceptChanges();

                }

                if (BalQuantity > 0)
                {
                    dtBalProduct.Rows.Add(dr);
                    dtBalProduct.AcceptChanges();
                }
            }
        }//END_ForEach

        ViewState["vwsFinalEway"] = dtFinalEway;

        return dtBalProduct;
    }
    #endregion

    #region Eway Json
    protected void btnEwayJson_Click(object sender, EventArgs e)
    {
        GenerateEwayJson();

        if (Session["FilePath"] != null)
        {
            string strJsonPath = Session["FilePath"].ToString();

            if (strJsonPath != "")
            {
                DownloadDocument(strJsonPath);
            }
        }
    }
    private void GenerateEwayJson()
    {
        if (ViewState["vwsFinalEway"] != null)
        {
            DataTable dtJson = (DataTable)ViewState["vwsFinalEway"];

            string strFileName = @"EwayJson\" + lblJobRefNo.Text.Replace("/", "").Trim() + ".json";

            string strJsonPath = WriteJsonClass(dtJson, strFileName);

            if (strJsonPath != "")
            {
                Session["FilePath"] = strJsonPath;

                lblError.Text = "File Created Successfully";
                lblError.CssClass = "success";

                int JobId = 0;
                if (hdnJobId.Value != "")
                {
                    JobId = Convert.ToInt32(hdnJobId.Value);
                }

             //   DBOperations.AddEWayBill(JobId, lblJobRefNo.Text.Trim(), "", strJsonPath, LoggedInUser.glUserId);
            }
            else
            {
                lblError.Text = "File Creation Error!";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            lblError.Text = "No Record Found!";
            lblError.CssClass = "errorMsg";
        }
    }
    public string WriteJsonClass(DataTable dtEWay, string path)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        int TotalVehicle = Convert.ToInt16(txtVehicleCount.Text.Trim());

        bool IsIGSTApplicable = false;

        if (rblIGST.SelectedValue == "1")
        {
            IsIGSTApplicable = true;
        }


        EWayRootObject root = new EWayRootObject();

        List<EWayBillList> VehicleList = new List<EWayBillList>();

        // Get Distinct Vehicle No

        DataView viewVehicle = new DataView(dtEWay);
        //DataTable distinctValues = viewVehicle.ToTable(true, "Vehicle No");

        DataTable distinctValues = viewVehicle.ToTable(true, "VehicleID");

        root.version = "1.0.0501"; //1.0.0123
        try
        {
            for (int i = 0; i < TotalVehicle; i++)
            {
                List<EwayItemList> itm = new List<EwayItemList>();
                EWayBillList BillList = new EWayBillList();

                //string strVehicleNo = distinctValues.Rows[i]["Vehicle No"].ToString();

                string strVehicleNo = "";
                string strVehicleID = distinctValues.Rows[i]["VehicleID"].ToString();

                string expression;
                //expression = "[Vehicle No]='" + strVehicleNo + "'";

                expression = "[VehicleID]='" + strVehicleID + "'";

                DataRow[] itemRows;

                itemRows = dtEWay.Select(expression);

                BillList.genMode = "Excel";
                BillList.userGstin = txtUserGSTIN.Text.Trim();
                BillList.supplyType = ddTransType.SelectedValue;
                BillList.subSupplyType = ddSubType.SelectedValue;
                BillList.docType = ddDocType.SelectedValue;
                BillList.docNo = lblBOENo.Text.Trim();
                BillList.docDate = lblBOEDate.Text.Trim();

                BillList.fromGstin = txtBillFromGSTIN.Text.Trim();
                BillList.fromTrdName = txtBillFromName.Text.Trim(); // Supplier
                BillList.fromAddr1 = txtDespatchFromAddr1.Text.Trim();
                BillList.fromAddr2 = txtDespatchFromAddr2.Text.Trim();
                BillList.fromPlace = txtDespatchFromPlace.Text.Trim();
                BillList.fromPincode = Convert.ToInt32(txtDespatchFromPin.Text.Trim());
                BillList.fromStateCode = Convert.ToInt32(ddBillFromState.SelectedValue);
                BillList.actualFromStateCode = Convert.ToInt32(ddDespatchFromState.SelectedValue);

                BillList.toGstin = txtBillToGSTIN.Text.Trim(); // Consignee GSTIN
                BillList.toTrdName = txtBillToName.Text.Trim(); // Consignee Name
                BillList.toAddr1 = txtDeliveryAddress.Text.Trim();
                BillList.toAddr2 = "";
                BillList.toPlace = txtDeliveryDestination.Text.Trim();
                BillList.toPincode = Convert.ToInt32(txtPinCode.Text.Trim());
                BillList.toStateCode = Convert.ToInt32(ddBillToState.SelectedValue); // Consignee GSTN State
                BillList.actualToStateCode = Convert.ToInt32(ddState.SelectedValue); // Delivery State

                BillList.totalValue = 0;
                BillList.cgstValue = 0;
                BillList.sgstValue = 0;
                BillList.igstValue = 0;
                BillList.cessValue = 0;
                BillList.totInvValue = 0;

                BillList.transMode = "1";
                BillList.transDistance = txtDistance.Text.Trim();
                BillList.transporterName = txtTransporterName.Text.Trim();
                BillList.transporterId = txtTransporterGSTIN.Text.Trim();
                BillList.transDocNo = "";
                BillList.transDocDate = txtTransportDate.Text.Trim();
                BillList.vehicleType = "R";
                //BillList.vehicleNo      =   strVehicleNo;

                int j = 1;
                foreach (DataRow dr in itemRows)
                {
                    // itemList

                    EwayItemList ProductList = new EwayItemList();

                    ProductList.igstRate = 0;
                    ProductList.sgstRate = 0;
                    ProductList.cgstRate = 0;
                    ProductList.cessRate = 0;

                    strVehicleNo = dr["Vehicle No"].ToString();

                    if (strVehicleNo == "")
                    {
                        BillList.transDocNo = "";
                    }

                    BillList.vehicleNo = strVehicleNo;

                    ProductList.itemNo = j;
                    ProductList.productName = dr["Product"].ToString();
                    ProductList.productDesc = dr["Description"].ToString();
                    if (dr["HSN"].ToString() != "")
                        ProductList.hsnCode = Convert.ToInt32(dr["HSN"].ToString());
                    else
                        ProductList.hsnCode = 0;

                    BillList.mainHsnCode = ProductList.hsnCode;
                    ProductList.quantity = Convert.ToDouble(dr["qty"].ToString());

                    ProductList.qtyUnit = "";
                    ProductList.taxableAmount = 0;

                    if (dr["AssessableValue"].ToString() != "")
                    {
                        ProductList.taxableAmount = Convert.ToDouble(dr["AssessableValue"].ToString());
                        BillList.totalValue = BillList.totalValue + Convert.ToDouble(dr["AssessableValue"].ToString());

                        //BillList.totInvValue = BillList.totInvValue + ProductList.taxableAmount;
                    }
                    if (dr["IGSTAmount"].ToString() != "")
                    {
                        BillList.igstValue = BillList.igstValue + Convert.ToDouble(dr["IGSTAmount"].ToString());

                        //BillList.totInvValue = BillList.totInvValue + Convert.ToDouble(dr["IGSTAmount"].ToString()); ;
                    }
                    if (dr["CGSTAmount"].ToString() != "")
                    {
                        BillList.cgstValue = BillList.cgstValue + Convert.ToDouble(dr["CGSTAmount"].ToString());

                        //BillList.totInvValue = BillList.totInvValue + Convert.ToDouble(dr["CGSTAmount"].ToString());
                    }
                    if (dr["SGSTAmount"].ToString() != "")
                    {
                        BillList.sgstValue = BillList.sgstValue + Convert.ToDouble(dr["SGSTAmount"].ToString());

                        //BillList.totInvValue = BillList.totInvValue + Convert.ToDouble(dr["SGSTAmount"].ToString());
                    }

                    if (dr["GSTDutyRate"].ToString() != "")
                    {
                        if (IsIGSTApplicable == true)
                        {
                            ProductList.igstRate = Convert.ToDouble(dr["GSTDutyRate"].ToString());
                        }
                        else
                        {
                            ProductList.cgstRate = Convert.ToDouble(dr["GSTDutyRate"].ToString()) / 2;
                            ProductList.sgstRate = Convert.ToDouble(dr["GSTDutyRate"].ToString()) / 2;
                        }
                    }
                  
                    itm.Add(ProductList);
                    j = j + 1;

                }//END_ForEach Item list

                BillList.totalValue =Math.Round(BillList.totalValue,2);
                BillList.cgstValue = Math.Round(BillList.cgstValue, 2);
                BillList.sgstValue = Math.Round(BillList.sgstValue, 2);
                BillList.igstValue = Math.Round(BillList.igstValue, 2);
                BillList.cessValue = Math.Round(BillList.cessValue, 2);

                BillList.totInvValue = BillList.totalValue + BillList.cgstValue + BillList.sgstValue + BillList.igstValue + BillList.cessValue;

                BillList.totInvValue = Math.Round(BillList.totInvValue,2);
                BillList.itemList = itm;
                VehicleList.Add(BillList);

            }// END_For_VehicleCount

            root.billLists = VehicleList;

            var json = serializer.Serialize(root);

            // Upload File To Server
            string ServerPath = FileServer.GetFileServerDir();

            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + path);
            }
            else
            {
                ServerPath = ServerPath + path;
            }
            using (var file = new StreamWriter(ServerPath, false))
            {
                file.Write(json);
                file.Close();
                file.Dispose();

                return path;

            }
        }//END_Try

        catch (Exception ex) { return ""; }
    }
    #endregion

    #region Download Json

    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

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

    #endregion

    #region Download Excel

    protected void btnEwayExcel_Click(object sender, EventArgs e)
    {
        GenerateExcel();
    }
    private void GenerateExcel()
    {
        string header = "attachment;filename=\"" + lblJobRefNo.Text.Trim() + ".xls" + "\"";

        if (ViewState["vwsFinalEway"] != null)
        {
            DataTable dtExcel = (DataTable)ViewState["vwsFinalEway"];

            DataColumn ProductID = new DataColumn("ProductID", Type.GetType("System.String"));
            DataColumn VehicleID = new DataColumn("VehicleID", Type.GetType("System.String"));

            dtExcel.Columns.Remove("Sl");
            dtExcel.Columns.Remove("ProductID");
            dtExcel.Columns.Remove("VehicleID");

            using (XLWorkbook wb = new XLWorkbook())
            {
                lblError.Text = "";
                string Report = lblJobRefNo.Text.Trim();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", header);
                Response.Charset = "";
                this.EnableViewState = false;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (dtExcel.Rows.Count > 0)
                {
                    // Compare function for Single Row View Against Job Ref No
                    //Remove Duplicate Cleared Row and Add DataTable as Worksheet.

                    wb.Worksheets.Add(dtExcel);
                }

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
    #endregion
    protected void wzDelivery_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        btnEwayJson.Visible = false;
        btnEwayExcel.Visible = false;
    }

    #region API

    protected void btnAPIEWay_Click(object sender, EventArgs e)
    {
        APIRequest();
    }
    protected void APIRequest()
    {
        // Encrypt Using IIS Publick Key

        // Encrypt Using IIS Publick Key

        var path = "d://testfinal.pfx";
        var filepassword = "M6XbLfTQV0";

        var clientPasscode = "1E8H1J547";
        var key = "12345678901234567890123456789012";
        var collection = new X509Certificate2Collection();

        collection.Import(path, filepassword, X509KeyStorageFlags.PersistKeySet);

        var certificate = collection[0];

        var publicKey = certificate.PublicKey.Key as RSACryptoServiceProvider;

        byte[] bteCrypt = Encoding.ASCII.GetBytes(clientPasscode);

        byte[] bteKey = Encoding.ASCII.GetBytes(key);

        var encryptedPass = publicKey.Encrypt(bteCrypt, true);
        var encryptedKey = publicKey.Encrypt(bteKey, true);

        string strPassword = Encoding.ASCII.GetString(encryptedPass);
        string strKey = Encoding.ASCII.GetString(encryptedKey);
        //M6XbLfTQV0

        HttpWebRequest APIWebRequest = (HttpWebRequest)WebRequest.Create("http://164.100.80.111/ewaybillapi/authenticate");

        string URI = "http://164.100.80.111/ewaybillapi/authenticate";

        CookieContainer CCCookies = new CookieContainer();
        NameValueCollection RequestHeader = new NameValueCollection();

        RequestHeader.Add("clientid", "BabajiShivram");
        RequestHeader.Add("client-secret", "M6XbLfTQV0");
        RequestHeader.Add("gstin", "27AAACN1163G1ZR");

        HttpWebResponse webResponse;

        HTTPSBaseClass BaseHttps = new HTTPSBaseClass();

        //string strPostData = @"{'action':'ACCESSTOKEN','username':'BSLive','password':'1E8H1J547','App_key':'lK+CkAzgNZ8O1W+v9d/KbFLas38kWWfLZyJy9IlAlZmANKAUqMQ3Hi35vInnPYZF'}";

        string strPostData = @"{""action"":""ACCESSTOKEN"",""username"":""BSLive"",""password"":" + strPassword + ",'app_key':" + strKey + "}";

        string RequestMethod = "POST";

        HttpWebRequest webrequest = BaseHttps.CreateWebRequest(URI, CCCookies, RequestMethod, RequestHeader, true, "");

        BuildRequestStream(ref webrequest, strPostData);

        webResponse = (HttpWebResponse)webrequest.GetResponse();

        StreamReader srData = new StreamReader(webResponse.GetResponseStream());
        string strHTMLData = srData.ReadToEnd();

        lblError.Text = strHTMLData;

    }

    // This method build the request stream for WebRequest
    private void BuildRequestStream(ref HttpWebRequest webrequest, string Request)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(Request);
        webrequest.ContentLength = bytes.Length;
        Stream oStreamOut = webrequest.GetRequestStream();
        oStreamOut.Write(bytes, 0, bytes.Length);
        oStreamOut.Close();
    }

    #endregion

    protected void ddTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddTransType.SelectedValue == "I")
            DBOperations.FillTransSubType(ddSubType, 1);
        else if (ddTransType.SelectedValue == "O")
            DBOperations.FillTransSubType(ddSubType, 2);
    }

    protected void rblGroupHSN_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool IsIGST = true;

        if (rblIGST.SelectedValue == "0")
        {
            IsIGST = false;
        }
        if (hdnJobId.Value != "" && hdnJobId.Value != "0")
        {
            int JobId = Convert.ToInt32(hdnJobId.Value);

            if (rblGroupHSN.SelectedValue == "1")
            {
                BindProductGroupHSN(JobId, IsIGST);
            }
            else
            {
                BindProduct(JobId, IsIGST);
            }

        }
    }

    protected void rblIGST_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool IsIGST = true;

        if (rblIGST.SelectedValue == "0")
        {
            IsIGST = false;
        }
        if (hdnJobId.Value != "" && hdnJobId.Value != "0")
        {
            int JobId = Convert.ToInt32(hdnJobId.Value);

            if (rblGroupHSN.SelectedValue == "1")
            {
                BindProductGroupHSN(JobId, IsIGST);
            }
            else
            {
                BindProduct(JobId, IsIGST);
            }

        }
    }
}