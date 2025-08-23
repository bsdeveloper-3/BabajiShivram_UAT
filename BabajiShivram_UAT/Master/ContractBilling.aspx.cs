using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Collections;
using iTextSharp.text;
using System.Configuration;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
//using myApp.ns.pages;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Word;
using TransportTrack;
//using static iTextSharp.text.pdf.PRTokeniser;

public partial class Master_ContractBilling : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    string message = "";
    
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
        {
        //lberror.Text = "Testing...................";
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(lnkexport1);
        ScriptManager1.RegisterPostBackControl(btnSave);
        ScriptManager1.RegisterPostBackControl(lnkDownload);
        PanelContractBilling.Visible = false;
        if (!IsPostBack)
        {
            Session["Status"] = "";
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Detail";
            ViewState["GridTable"] = null;
            ViewState["ContractID"] = null;
            cmdEditContractLine.Enabled = false;
            //cmdsave.Enabled = false;
            DataSet dsPlant;
            dsPlant = DBOperations.GetContractMasterData(1);
            cboCustomerName.DataSource = dsPlant;
            cboCustomerName.DataTextField = "CustName";
            cboCustomerName.DataValueField = "lid";
            cboCustomerName.DataBind();
            dsPlant = DBOperations.GetContractMasterData(2);
            cboDivision.DataSource = dsPlant;
            cboDivision.DataTextField = "DivisionName";
            cboDivision.DataValueField = "lid";
            cboDivision.DataBind();
            cboDivision.Items.Insert(0, "--Select--");
            //Filldropdown();
            createdatatable();
            //ddlPageSize.SelectedValue = "1";
            //if(Session["chksave"] == "ok")
            //{
            //    UpdMain.Update();
            //    updAll.Update();
            //    //Fieldset1.Visible = true;
            //    //btnNew.Visible = true;
            //    //PanelContractBilling.Visible = false;
            //    //clear();
            //    //Response.Redirect("ContractBilling.aspx", false);
            //}
        }
        Session["chksave"] = "Nok";
        //
        DataFilter1.DataSource = GridviewSqlDataSource;
        DataFilter1.DataColumns = dgvDetailData_1.Columns;
        DataFilter1.FilterSessionID = "ContractBilling.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
        //
    }
    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            //  DataFilter1.AddFirstFilter();
            // DataFilter1.AddNewFilter();
        }
        else
        {
            DataFilter1_OnDataBound();
        }
    }

    void DataFilter1_OnDataBound()
    {
        try
        {
            DataFilter1.FilterSessionID = "ContractBilling.aspx";
            DataFilter1.FilterDataSource();
            dgvDetailData_1.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion
    #region Loadevent
    protected void createdatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[39] { new DataColumn("DivisionName"), new DataColumn("ChargeName"),     new DataColumn("Currency"),
            new DataColumn("UOM"),    new DataColumn("Mode"), new DataColumn("Port"),    new DataColumn("ContainerType"),    new DataColumn("TypeOfShipment"),
            new DataColumn("Rate"), new DataColumn("ChargeCode"), new DataColumn("Heading"), new DataColumn("RangeCriteria"), new DataColumn("RangeFrom"),
            new DataColumn("RangeTo"), new DataColumn("CapCriteria"), new DataColumn("CapMin"), new DataColumn("CapMax"), new DataColumn("UserSystem"),
             new DataColumn("JobType"), new DataColumn("TypeOfBE"), new DataColumn("RMSNonRMSName"), new DataColumn("LoadedDeStuff"),
                new DataColumn("lid"),new DataColumn("UOMID"),new DataColumn("ModeID")
        ,new DataColumn("CurrencyID"),new DataColumn("PortID"),new DataColumn("ContainerTypeID"),new DataColumn("TypeOfShipmentID"),
            new DataColumn("RangeCriteriaID"),new DataColumn("CapCriteriaID"),new DataColumn("UserSystemID")
        ,new DataColumn("JobTypeID"),new DataColumn("TypeOfBEID"),new DataColumn("RMSNonRMSID"),new DataColumn("LoadedDeStuffID"),new DataColumn("DivisionId")
        ,new DataColumn("Condition"),new DataColumn("FABookid")});
        //dt.Columns.AddRange(new DataColumn[39] { new DataColumn("DivisionName"), new DataColumn("ChargeName"),     new DataColumn("Currency"),
        //    new DataColumn("UOM"),    new DataColumn("Mode"), new DataColumn("Port"),    new DataColumn("ContainerType"),    new DataColumn("TypeOfShipment"),
        //    new DataColumn("Rate"), new DataColumn("ChargeCode"), new DataColumn("Heading"), new DataColumn("RangeCriteria"), new DataColumn("RangeFrom"),
        //    new DataColumn("RangeTo"), new DataColumn("CapCriteria"), new DataColumn("CapMin"), new DataColumn("CapMax"), new DataColumn("UserSystem"),
        //     new DataColumn("JobType"), new DataColumn("TypeOfBE"), new DataColumn("RMSNonRMSName"), new DataColumn("LoadedDeStuff"),
        //        new DataColumn("lid"),new DataColumn("UOMID"),new DataColumn("ModeID")
        //,new DataColumn("CurrencyID"),new DataColumn("PortID"),new DataColumn("ContainerTypeID"),new DataColumn("TypeOfShipmentID"),
        //    new DataColumn("RangeCriteriaID"),new DataColumn("CapCriteriaID"),new DataColumn("UserSystemID")
        //,new DataColumn("JobTypeID"),new DataColumn("TypeOfBEID"),new DataColumn("RMSNonRMSID"),new DataColumn("LoadedDeStuffID"),new DataColumn("DivisionId")
        //,new DataColumn("Condition"),new DataColumn("FABookid")});
        //dt.Columns.AddRange(new DataColumn[39] { new DataColumn("DivisionName"), new DataColumn("ChargeName"),     new DataColumn("Currency"),
        //    new DataColumn("UOM"),    new DataColumn("Mode"), new DataColumn("Port"),    new DataColumn("ContainerType"),    new DataColumn("TypeOfShipment"),
        //    new DataColumn("Rate"), new DataColumn("ChargeCode"), new DataColumn("Heading"), new DataColumn("RangeCriteria"), new DataColumn("RangeFrom"),
        //    new DataColumn("RangeTo"), new DataColumn("CapCriteria"), new DataColumn("CapMin"), new DataColumn("CapMax"), new DataColumn("UserSystem"),
        //     new DataColumn("JobType"), new DataColumn("TypeOfBE"), new DataColumn("RMSNonRMSName"), new DataColumn("IsMultiple"),
        //    new DataColumn("IsMultipleOnKG"),new DataColumn("LoadedDeStuff"),new DataColumn("lid"),new DataColumn("UOMID"),new DataColumn("ModeID")
        //,new DataColumn("CurrencyID"),new DataColumn("PortID"),new DataColumn("ContainerTypeID"),new DataColumn("TypeOfShipmentID"),
        //    new DataColumn("RangeCriteriaID"),new DataColumn("CapCriteriaID"),new DataColumn("UserSystemID")
        //,new DataColumn("JobTypeID"),new DataColumn("TypeOfBEID"),new DataColumn("RMSNonRMSID"),new DataColumn("LoadedDeStuffID"),new DataColumn("DivisionId")});
        ViewState["GridTable"] = dt;
        this.BindGrid();
    }
    protected void Filldropdown()
    {
        DataSet dsPlant;
        //dsPlant = DBOperations.GetContractMasterData(1);
        //cboCustomerName.DataSource = dsPlant;
        //cboCustomerName.DataTextField = "CustName";
        //cboCustomerName.DataValueField = "lid";
        //cboCustomerName.DataBind();
        cboUserSystem.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(2);
        cboDivision.DataSource = dsPlant;
        cboDivision.DataTextField = "DivisionName";
        cboDivision.DataValueField = "lid";
        cboDivision.DataBind();
        cboDivision.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(3);
        cboChargeName.DataSource = dsPlant;
        cboChargeName.DataTextField = "name";
        cboChargeName.DataValueField = "charge";
        cboChargeName.DataBind();
        cboChargeName.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(4);
        cboUOM.DataSource = dsPlant;
        cboUOM.DataTextField = "sname";
        cboUOM.DataValueField = "lid";
        cboUOM.DataBind();
        cboUOM.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(5);
        cboJobType.DataSource = dsPlant;
        cboJobType.DataTextField = "sname";
        cboJobType.DataValueField = "lid";
        cboJobType.DataBind();
        cboJobType.Items.Insert(0, "--Select--");
        //dsPlant = DBOperations.GetContractMasterData(6);
        //cboTypeOfBE.DataSource = dsPlant;
        //cboTypeOfBE.DataTextField = "BETypeName";
        //cboTypeOfBE.DataValueField = "BETypeId";
        //cboTypeOfBE.DataBind();
        dsPlant = DBOperations.GetContractMasterData(7);
        cboRMSNonRMS.DataSource = dsPlant;
        cboRMSNonRMS.DataTextField = "RMSNonRMS";
        cboRMSNonRMS.DataValueField = "RMSNonRMSID";
        cboRMSNonRMS.DataBind();
        cboRMSNonRMS.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(8);
        cboLoadedDeStuff.DataSource = dsPlant;
        cboLoadedDeStuff.DataTextField = "sname";
        cboLoadedDeStuff.DataValueField = "lid";
        cboLoadedDeStuff.DataBind();
        cboLoadedDeStuff.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(9);
        cboCurrency.DataSource = dsPlant;
        cboCurrency.DataTextField = "Currency";
        cboCurrency.DataValueField = "lId";
        cboCurrency.DataBind();
        cboCurrency.Items.Insert(0, "--Select--");
        cboCurrency.SelectedValue = "46";
        dsPlant = DBOperations.GetContractMasterData(10);
        cboRangeCriteria.DataSource = dsPlant;
        cboRangeCriteria.DataTextField = "RangeCriteriaName";
        cboRangeCriteria.DataValueField = "RCId";
        cboRangeCriteria.DataBind();
        cboRangeCriteria.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(11);
        cboCAPCriteria.DataSource = dsPlant;
        cboCAPCriteria.DataTextField = "CapCriteriaName";
        cboCAPCriteria.DataValueField = "CapCId";
        cboCAPCriteria.DataBind();
        cboCAPCriteria.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(12);
        cboMode.DataSource = dsPlant;
        cboMode.DataTextField = "sName";
        cboMode.DataValueField = "lid";
        cboMode.DataBind();
        cboMode.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(13);
        cboPort.DataSource = dsPlant;
        cboPort.DataTextField = "PortName";
        cboPort.DataValueField = "lid";
        cboPort.DataBind();
        cboPort.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(14);
        cboContainerType.DataSource = dsPlant;
        cboContainerType.DataTextField = "sName";
        cboContainerType.DataValueField = "lid";
        cboContainerType.DataBind();
        cboContainerType.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(15);
        cboTypeOfShipment.DataSource = dsPlant;
        cboTypeOfShipment.DataTextField = "sName";
        cboTypeOfShipment.DataValueField = "lid";
        cboTypeOfShipment.DataBind();
        cboTypeOfShipment.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(17);
        //cboUserSystem.SelectedIndex = 0;
        cboUserSystem.DataSource = dsPlant;
        cboUserSystem.DataTextField = "EntryMode";
        cboUserSystem.DataValueField = "lid";
        cboUserSystem.DataBind();
        cboUserSystem.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(18);
        cboFABook.DataSource = dsPlant;
        cboFABook.DataTextField = "BookName";
        cboFABook.DataValueField = "BookCode";
        cboFABook.DataBind();
        cboFABook.Items.Insert(0, "--Select--");
        SearchGridData();
        //clear();
    }

    protected void FillExportDropdown()
    {
        DataSet dsPlant;
        //dsPlant = DBOperations.GetContractMasterDataForExport(1);
        //cboCustomerName.DataSource = dsPlant;
        //cboCustomerName.DataTextField = "CustName";
        //cboCustomerName.DataValueField = "lid";
        //cboCustomerName.DataBind();
        dsPlant = DBOperations.GetContractMasterDataForExport(2);
        cboDivision.DataSource = dsPlant;
        cboDivision.DataTextField = "DivisionName";
        cboDivision.DataValueField = "lid";
        cboDivision.DataBind();
        cboDivision.Items.Insert(0, "--Select--");
        //cboJobType.SelectedIndex = 0;
        dsPlant = DBOperations.GetContractMasterDataForExport(3);
        cboChargeName.DataSource = dsPlant;
        cboChargeName.DataTextField = "name";
        cboChargeName.DataValueField = "charge";
        cboChargeName.DataBind();
        cboChargeName.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(4);
        cboUOM.DataSource = dsPlant;
        cboUOM.DataTextField = "sname";
        cboUOM.DataValueField = "lid";
        cboUOM.DataBind();
        cboUOM.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(5);
        cboJobType.DataSource = dsPlant;
        cboJobType.DataTextField = "sname";
        cboJobType.DataValueField = "lid";
        cboJobType.DataBind();
        cboJobType.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(6);
        cboTypeOfBE.DataSource = dsPlant;
        cboTypeOfBE.DataTextField = "BETypeName";
        cboTypeOfBE.DataValueField = "BETypeId";
        cboTypeOfBE.DataBind();
        cboTypeOfBE.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(7);
        cboRMSNonRMS.DataSource = dsPlant;
        cboRMSNonRMS.DataTextField = "RMSNonRMS";
        cboRMSNonRMS.DataValueField = "RMSNonRMSID";
        cboRMSNonRMS.DataBind();
        cboRMSNonRMS.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(8);
        cboLoadedDeStuff.DataSource = dsPlant;
        cboLoadedDeStuff.DataTextField = "sname";
        cboLoadedDeStuff.DataValueField = "lid";
        cboLoadedDeStuff.DataBind();
        cboLoadedDeStuff.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(9);
        cboCurrency.DataSource = dsPlant;
        cboCurrency.DataTextField = "Currency";
        cboCurrency.DataValueField = "lId";
        cboCurrency.DataBind();
        cboCurrency.Items.Insert(0, "--Select--");
        cboCurrency.SelectedValue = "46";
        dsPlant = DBOperations.GetContractMasterDataForExport(10);
        cboRangeCriteria.DataSource = dsPlant;
        cboRangeCriteria.DataTextField = "RangeCriteriaName";
        cboRangeCriteria.DataValueField = "RCId";
        cboRangeCriteria.DataBind();
        cboRangeCriteria.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(11);
        cboCAPCriteria.DataSource = dsPlant;
        cboCAPCriteria.DataTextField = "CapCriteriaName";
        cboCAPCriteria.DataValueField = "CapCId";
        cboCAPCriteria.DataBind();
        cboCAPCriteria.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(12);
        cboMode.DataSource = dsPlant;
        cboMode.DataTextField = "sName";
        cboMode.DataValueField = "lid";
        cboMode.DataBind();
        cboMode.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterData(13);
        cboPort.DataSource = dsPlant;
        cboPort.DataTextField = "PortName";
        cboPort.DataValueField = "lid";
        cboPort.DataBind();
        cboPort.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(14);
        cboContainerType.DataSource = dsPlant;
        cboContainerType.DataTextField = "sName";
        cboContainerType.DataValueField = "lid";
        cboContainerType.DataBind();
        cboContainerType.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(15);
        cboTypeOfShipment.DataSource = dsPlant;
        cboTypeOfShipment.DataTextField = "sName";
        cboTypeOfShipment.DataValueField = "lid";
        cboTypeOfShipment.DataBind();
        cboTypeOfShipment.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(17);
        //cboUserSystem.SelectedIndex = 0;
        cboUserSystem.DataSource = dsPlant;
        cboUserSystem.DataTextField = "EntryMode";
        cboUserSystem.DataValueField = "lid";
        cboUserSystem.DataBind();
        cboUserSystem.Items.Insert(0, "--Select--");
        dsPlant = DBOperations.GetContractMasterDataForExport(18);
        cboFABook.DataSource = dsPlant;
        cboFABook.DataTextField = "BookName";
        cboFABook.DataValueField = "BookCode";
        cboFABook.DataBind();
        cboFABook.Items.Insert(0, "--Select--");
        SearchGridData();
    }
    private void SearchGridData()
    {
        try
        {
            dgvDetailData_1.DataSource = null;
            string sql;

            sql = "select cm.lid,cust.CustName as CustomerName,cm.ContractName,";
            sql = sql + " Replace(Convert(nvarchar(11), cm.ContractStartDate, 113),' ','-') as ContractStartDate,";
            sql = sql + " Replace(Convert(nvarchar(11), cm.ContractEndDate, 113),' ','-') as ContractEndDate,cm.ContractUID";
            sql = sql + " from ContractMaster cm";
            sql = sql + " inner join BS_CustomerMS cust on cm.CustomerId = cust.lid";
            sql = sql + " inner join CB_DivisionMS div on cm.DivisionId = div.Division_Manual_ID";
            sql = sql + " order by cm.lid,cm.CreatedDate desc";

            DataTable dt = DBOperations.FillTableData(sql);
            dgvDetailData_1.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count < 5)
                {
                    for (int i = 1; i <= 5 - dgvDetailData_1.Rows.Count; i++)
                    {
                        //dt.Rows.Add("");
                    }
                }
                //dgvDetailData_1.DataSource = dt;
                //dgvDetailData_1.DataBind();
            }
            else
            {
                //dgvDetailData_1.DataSource = dt;
                //dgvDetailData_1.DataBind();
            }
            //ViewState["Gridsearch"] = dt;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(UserLogin, "", "cmdSearch_Click", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlSearchBy.SelectedIndex > 0)
            //{
            //    DataTable dt = ((DataTable)ViewState["Gridsearch"]);
            //    // Presuming the DataTable has a column named Date.
            //    string expression = ddlSearchBy.SelectedItem.Text + "='" + txtSearch.Text + "'";
            //    // string expression = "OrderQuantity = 2 and OrderID = 2";

            //    // Sort descending by column named CompanyName.
            //    string sortOrder = ddlSearchBy.SelectedItem.Text + " ASC";
            //    DataRow[] foundRows;

            //    // Use the Select method to find all rows matching the filter.
            //    foundRows = dt.Select(ddlSearchBy.SelectedItem.Text + " LIKE '%" + txtSearch.Text.Trim() + "%'"); //dt.Select(expression, sortOrder);
            //    if (foundRows.Count() == 0)
            //    {
            //        dgvDetailData_1.DataSource = "";
            //    }
            //    else
            //    {
            //        dgvDetailData_1.DataSource = foundRows.CopyToDataTable();
            //    }
            //    //dgvDetailData_1.DataBind();
            //}
        }
        catch (Exception ex)
        {
            ErrorLog.DisplayExcetions(ex, false, updAll);
        }
    }
    protected void gvUser_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }
    protected void gvUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        dgvDetailData_1.Visible = false;
        DataFilter1.Visible = false;
        //fsMainBorder.Visible = false;
        //if (gvUser.SelectedIndex == -1)
        //{
        //    FormView1.ChangeMode(FormView1.DefaultMode);
        //}
        //else
        //{
        //    FormView1.ChangeMode(FormViewMode.ReadOnly);
        //}
        //FormView1.DataBind();
    }
    string addvalid;
    protected void cmdAddLine_Click(object sender, EventArgs e)
    {
        try
        {
           // string fileName = Path.GetFileName(fuContractFile.PostedFile.FileName);
            //if (cboFABook.SelectedValue==ValidData() || ValidData()=="")
            //{
                addvalid = "false";
                AddContractLine();
                //ContractCopyUpload();
            //}
            //else
            //{
            //    lberror.Text = "Please select correct fabookid";
            //    lberror.CssClass = "errorMsg";
            //}
            
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdAddLine_Click", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void validation_AddLine()
    {
        try
        {
            if (cboDivision.SelectedItem.Text == "" || cboDivision.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select division !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select division !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select division !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboDivision.Focus();
                return;
            }

            if (cboChargeName.SelectedItem.Text == "" || cboChargeName.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select charge name !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select charge name !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select charge name !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboChargeName.Focus();
                return;
            }

            if (cboUOM.SelectedItem.Text == "" || cboUOM.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select UOM !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select UOM !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select UOM !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboUOM.Focus();
                return;
            }

            if (cboCurrency.SelectedItem.Text == "" || cboCurrency.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select currency !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select currency !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select currency !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboCurrency.Focus();
                return;
            }

            if (cboJobType.SelectedItem.Text == "" || cboJobType.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select Job Type !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select Job Type !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select Job Type !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboJobType.Focus();
                return;
            }

            if (cboTypeOfBE.SelectedItem.Text == "" || cboTypeOfBE.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select Type of BE !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select Type of BE !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select Type of BE !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboTypeOfBE.Focus();
                return;
            }
            if (cboUOM.SelectedItem.Text.ToUpper() == ("% of Value").ToUpper())
            {
                if (cboRangeCriteria.SelectedItem.Text == "" || cboRangeCriteria.SelectedItem.Text == "--Select--")
                {
                    //MessageBox.Show("Kindly select range criteria !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly select range criteria !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select range criteria !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    cboRangeCriteria.Focus();
                    return;
                }

                if (txtRangeFrom.Text == string.Empty || txtRangeFrom.Text == "")
                {
                    //MessageBox.Show("", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly enter range from !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    txtRangeFrom.Focus();
                    return;
                }

                if (txtRangeto.Text == string.Empty || txtRangeto.Text == "")
                {
                    //MessageBox.Show("Kindly enter range to !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly enter range to !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly enter range to !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    txtRangeto.Focus();
                    return;
                }
            }
            if (cboUOM.SelectedItem.Text.ToUpper() == ("% of CIF Value").ToUpper())
            {
                if (cboCAPCriteria.SelectedItem.Text == "" || cboCAPCriteria.SelectedItem.Text == "--Select--")
                {
                    //MessageBox.Show("Kindly select cap criteria !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly select cap criteria !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select cap criteria !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    cboCAPCriteria.Focus();
                    return;
                }

                if (txtCapMin.Text == string.Empty || txtCapMin.Text == "")
                {
                    //MessageBox.Show("Kindly enter cap min !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly enter cap min !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly enter cap min !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    txtCapMin.Focus();
                    return;
                }

                if (txtCapMax.Text == string.Empty || txtCapMax.Text == "")
                {
                    //MessageBox.Show("Kindly enter cap max !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly enter cap max !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly enter cap max !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    txtCapMax.Focus();
                    return;
                }

            }
            if (txtRangeFrom.Text != "" && txtRangeto.Text != "")
            {
                if (Convert.ToDouble(txtRangeFrom.Text) > Convert.ToDouble(txtRangeto.Text))
                {
                    //MessageBox.Show("Range from should not greater than range to !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Range from should not greater than range to !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Range from should not greater than range to !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    return;
                }
            }
            if (txtCapMin.Text != string.Empty && txtCapMax.Text != string.Empty)
            {
                if (Convert.ToDouble(txtCapMin.Text) > Convert.ToDouble(txtCapMax.Text))
                {
                    //MessageBox.Show("Cap min should not be greater than cap max !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Cap min should not be greater than cap max !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Cap min should not be greater than cap max !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    return;
                }
            }

            if (cboMode.SelectedItem.Text == "" || cboMode.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select mode !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select mode !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select mode !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboMode.Focus();
                return;
            }

            if (cboMode.SelectedItem.Text.ToUpper() == ("Sea").ToUpper())
            {
                if (cboLoadedDeStuff.SelectedItem.Text.ToUpper() != ("LCL").ToUpper())
                {
                    if (cboUOM.SelectedItem.Text != "Direct")
                    {
                        if (cboTypeOfShipment.SelectedItem.Text == "" || cboTypeOfShipment.SelectedItem.Text == "--Select--")
                        {
                            //MessageBox.Show("Kindly select container size !", "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //lberror.Text = "Kindly select container size !";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select container size !');", true);
                            PanelContractBilling.Visible = true;
                            Session["Operation"] = "No";
                            addvalid = "true";
                            cboTypeOfShipment.Focus();
                            return;
                        }
                    }

                    if (cboContainerType.SelectedItem.Text == "" || cboContainerType.SelectedItem.Text == "--Select--")
                    {
                        //MessageBox.Show("Kindly select container type !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //lberror.Text = "Kindly select container type !";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select container type !');", true);
                        PanelContractBilling.Visible = true;
                        Session["Operation"] = "No";
                        addvalid = "true";
                        cboMode.Focus();
                        return;
                    }
                }
                if (cboLoadedDeStuff.SelectedItem.Text == "" || cboLoadedDeStuff.SelectedItem.Text == "--Select--")
                {
                    //MessageBox.Show("Kindly select Delivery type !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Kindly select Delivery type !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select Delivery type !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    cboLoadedDeStuff.Focus();
                    return;
                }
            }
            if (cboPort.SelectedItem.Text == "" || cboPort.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select port !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select port !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select port !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboPort.Focus();
                return;
            }
            if (cboUserSystem.SelectedItem.Text == "" || cboUserSystem.SelectedItem.Text == "--Select--")
            {
                //MessageBox.Show("Kindly select system type !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly select system type !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select system type !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboMode.Focus();
                return;
            }

            if (txtRate.Text == string.Empty || txtRate.Text == "")
            {
                //MessageBox.Show("Kindly enter rate !", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lberror.Text = "Kindly enter rate !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly enter rate !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                txtRate.Focus();
                return;
            }

            if (cboLoadedDeStuff.SelectedItem.Text.ToUpper() == ("DeStuff") || cboLoadedDeStuff.SelectedItem.Text.ToUpper() == ("Loaded").ToUpper())
            {
                if (cboContainerType.SelectedItem.Text.ToUpper() != ("FCL").ToUpper())
                {
                    //MessageBox.Show("Container type should be FCL in case of Destuff/Loaded delivery type !", "Logistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lberror.Text = "Container type should be FCL in case of Destuff/Loaded delivery type !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Container type should be FCL in case of Destuff/Loaded delivery type !');", true);
                    PanelContractBilling.Visible = true;
                    Session["Operation"] = "No";
                    addvalid = "true";
                    return;
                }
            }

            if (cboFABook.SelectedItem.Text == "" || cboFABook.SelectedItem.Text == "--Select--")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select FA Book Name !');", true);
                PanelContractBilling.Visible = true;
                Session["Operation"] = "No";
                addvalid = "true";
                cboFABook.Focus();
                return;
            }

        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdAddLine_Click", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void AddContractLine()
    {
        try
        {
            int curRow = 0;string Result="";
            validation_AddLine();
            SqlCommand command = new SqlCommand();
            SqlCommand cmdCon = new SqlCommand();
            SqlConnection con = new SqlConnection();
            if (addvalid == "false")
            {
                con = CDatabase.getConnection();
                con.Open();
                string sql;
                DataTable dt = (DataTable)ViewState["GridTable"];
                ddlPageSize.SelectedValue = "0";
                //string RangeCriteria; float RangeCriteria1 = 0;
                //if (cboRangeCriteria.SelectedItem.ToString() == "--Select--") { RangeCriteria = ""; RangeCriteria1 = 0; txtRangeFrom.Text = "0"; txtRangeto.Text = "0"; }
                //else { RangeCriteria = cboRangeCriteria.SelectedItem.ToString(); RangeCriteria1 = Convert.ToInt32(cboRangeCriteria.SelectedValue); }
                //string CAPCriteria; float CAPCriteria1 = 0;
                //if (cboCAPCriteria.SelectedItem.ToString() == "--Select--") { CAPCriteria = ""; CAPCriteria1 = 0; txtCapMax.Text = "0"; txtCapMin.Text = "0"; }
                //else { CAPCriteria = cboCAPCriteria.SelectedItem.ToString(); CAPCriteria1 = Convert.ToInt32(cboCAPCriteria.SelectedValue); }
                //string contype; int contype1 = 0;
                //if (cboContainerType.SelectedItem.ToString() == "--Select--") { contype = ""; contype1 = 0; }
                //else { contype = cboContainerType.SelectedItem.ToString(); contype1 = Convert.ToInt32(cboContainerType.SelectedValue); }
                //string typeofship; int typeofship1 = 0;
                //if (cboTypeOfShipment.SelectedItem.ToString() == "--Select--") { typeofship = ""; typeofship1 = 0; }
                //else { typeofship = cboTypeOfShipment.SelectedItem.ToString(); typeofship1 = Convert.ToInt32(cboTypeOfShipment.SelectedValue); }
                //string Deliverytype; int Deliverytype1 = 0;
                //if (cboLoadedDeStuff.SelectedItem.ToString() == "--Select--") { Deliverytype = ""; Deliverytype1 = 0; }
                //else { Deliverytype = cboLoadedDeStuff.SelectedItem.ToString(); Deliverytype1 = Convert.ToInt32(cboLoadedDeStuff.SelectedValue); }

                //string Jobtype; int Jobtype1 = 0;
                //if (cboJobType.SelectedItem.ToString() == "--Select--") { Jobtype = ""; Jobtype1 = 0; }
                //else { Jobtype = cboJobType.SelectedItem.ToString(); Jobtype1 = Convert.ToInt32(cboJobType.SelectedValue); }

                //string RMSNRMS; int RMSNRMS1 = 0;
                //if (cboRMSNonRMS.SelectedItem.ToString() == "--Select--") { RMSNRMS = ""; RMSNRMS1 = 0; }
                //else { RMSNRMS = cboRMSNonRMS.SelectedItem.ToString(); RMSNRMS1 = Convert.ToInt32(cboRMSNonRMS.SelectedValue); }

                string RangeCriteria; float RangeCriteria1 = 0;
                if (cboRangeCriteria.SelectedItem.ToString() == "--Select--") { RangeCriteria = ""; RangeCriteria1 = 0; txtRangeFrom.Text = "0"; txtRangeto.Text = "0"; }
                else { RangeCriteria = cboRangeCriteria.SelectedItem.ToString(); RangeCriteria1 = Convert.ToInt32(cboRangeCriteria.SelectedValue); }
                string CAPCriteria; float CAPCriteria1 = 0;
                if (cboCAPCriteria.SelectedItem.ToString() == "--Select--") { CAPCriteria = ""; CAPCriteria1 = 0; txtCapMax.Text = "0"; txtCapMin.Text = "0"; }
                else { CAPCriteria = cboCAPCriteria.SelectedItem.ToString(); CAPCriteria1 = Convert.ToInt32(cboCAPCriteria.SelectedValue); }
                string contype; int contype1 = 0;
                if (cboContainerType.SelectedItem.ToString() == "--Select--") { contype = ""; contype1 = 0; }
                else { contype = cboContainerType.SelectedItem.ToString(); contype1 = Convert.ToInt32(cboContainerType.SelectedValue); }
                string typeofship; int typeofship1 = 0;
                if (cboTypeOfShipment.SelectedItem.ToString() == "--Select--") { typeofship = ""; typeofship1 = 0; }
                else { typeofship = cboTypeOfShipment.SelectedItem.ToString(); typeofship1 = Convert.ToInt32(cboTypeOfShipment.SelectedValue); }
                string Deliverytype; int Deliverytype1 = 0;
                if (cboLoadedDeStuff.SelectedItem.ToString() == "--Select--") { Deliverytype = ""; Deliverytype1 = 0; }
                else { Deliverytype = cboLoadedDeStuff.SelectedItem.ToString(); Deliverytype1 = Convert.ToInt32(cboLoadedDeStuff.SelectedValue); }


                string bookname; string bookname1 = "";
                if (cboFABook.SelectedItem.ToString() == "--Select--") { bookname = ""; bookname1 = "TA"; }
                else { bookname = cboFABook.SelectedItem.ToString(); bookname1 = cboFABook.SelectedValue; }

                if (txtRate.Text == "0" || txtRate.Text == "0.00")
                {
                    Session["Operation"] = "No";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly Add Rate greater than Zero.');", true);
                }
                else
                {
                    //sql = "Select count(*) from  CB_BillingDetail  where CMID = " + Session["strlid"] + " and ChargeCode = '" + txtChargeCode.Text + "' and Heading = '" + txtHeading.Text + "' " +
                    //    " and CurrencyID = " + cboCurrency.SelectedValue + " and UOMID = " + cboUOM.SelectedValue + " and isnull(RangeCriteriaID,0)= " + RangeCriteria1 + "" +
                    //    " and isnull(RangeFrom,0)= " + txtRangeFrom.Text + " and isnull(RangeTo,0)= " + txtRangeto.Text + " and isnull(CapCriteriaID,0)= " + CAPCriteria1 + " and isnull(CapMax,0)= " + txtCapMax.Text + " and isnull(CapMin,0)= " + txtCapMin.Text + "" +
                    //    " and UserSystemID = " + cboUserSystem.SelectedValue + " and ModeID = " + cboMode.SelectedValue + " and PortID = " + cboPort.SelectedValue + " and isnull(TypeOfShipmentID,0)= " + typeofship1 + "" +
                    //    " and isnull(ContainerTypeID,0)= " + contype1 + " and JobTypeID = " + Jobtype1 + " and TypeOfBEID = " + cboTypeOfBE.SelectedValue + "" +
                    //    " and RMSNonRMSID = " + RMSNRMS1 + " and LoadedDeStuffID = " + Deliverytype1 + " and DivisionId = " + cboDivision.SelectedValue + "  and Rate =" + txtRate.Text + " and Condition='" + txtcondition.Text + "' and bdel=0 and fabookid='" + cboFABook.SelectedValue + "'";
                    sql = "Select count(*) from  CB_BillingDetail  where CMID = " + Session["strlid"] + " and ChargeCode = '" + txtChargeCode.Text + "' and Heading = '" + txtHeading.Text + "' " +
                        " and CurrencyID = " + cboCurrency.SelectedValue + " and UOMID = " + cboUOM.SelectedValue + " and isnull(RangeCriteriaID,0)= " + RangeCriteria1 + "" +
                        " and isnull(RangeFrom,0)= " + txtRangeFrom.Text + " and isnull(RangeTo,0)= " + txtRangeto.Text + " and isnull(CapCriteriaID,0)= " + CAPCriteria1 + " and isnull(CapMax,0)= " + txtCapMax.Text + " and isnull(CapMin,0)= " + txtCapMin.Text + "" +
                        " and UserSystemID = " + cboUserSystem.SelectedValue + " and ModeID = " + cboMode.SelectedValue + " and PortID = " + cboPort.SelectedValue + " and isnull(TypeOfShipmentID,0)= " + typeofship1 + "" +
                        " and isnull(ContainerTypeID,0)= " + contype1 + " and JobTypeID = " + cboJobType.SelectedValue + " and TypeOfBEID = " + cboTypeOfBE.SelectedValue + "" +
                        " and RMSNonRMSID = " + cboRMSNonRMS.SelectedValue + " and LoadedDeStuffID = " + Deliverytype1 + " and DivisionId = " + cboDivision.SelectedValue + "  and Rate =" + txtRate.Text + " and Condition='" + txtcondition.Text + "' and bdel=0 and fabookid='" + cboFABook.SelectedValue + "'";

                    cmdCon.CommandText = sql;
                    cmdCon.Connection = con;
                    cmdCon.CommandType = CommandType.Text;
                    int countcont = Convert.ToInt32(cmdCon.ExecuteScalar());
                    if (countcont == 0)
                    {

                        //dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                        //                cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                        //                contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text,
                        //                cboUserSystem.SelectedItem.ToString(), Jobtype, cboTypeOfBE.SelectedItem.ToString(),
                        //                RMSNRMS, Deliverytype,
                        //                RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text, 0,
                        //                cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                        //                cboPort.SelectedValue.ToString(), contype1, typeofship1,
                        //                RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                        //                Jobtype1, cboTypeOfBE.SelectedValue.ToString(), RMSNRMS1,
                        //                Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(),  bookname1);

                        //dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                        //                cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                        //                contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text, 
                        //                RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text,
                        //                cboUserSystem.SelectedItem.ToString(), cboJobType.SelectedItem.ToString(), cboTypeOfBE.SelectedItem.ToString(),
                        //                cboRMSNonRMS.SelectedItem.ToString(), Deliverytype,
                        //                0,
                        //                cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                        //                cboPort.SelectedValue.ToString(), contype1, typeofship1,
                        //                RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                        //                cboJobType.SelectedValue.ToString(), cboTypeOfBE.SelectedValue.ToString(), cboRMSNonRMS.SelectedValue.ToString(),
                        //                Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(), bookname1);
                        if(cboDivision.SelectedValue=="1")
                        {
                            dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                            cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                            contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text,
                            RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text,
                            cboUserSystem.SelectedItem.ToString(), cboJobType.SelectedItem.ToString(), cboTypeOfBE.SelectedItem.ToString(),
                            cboRMSNonRMS.SelectedItem.ToString(), Deliverytype,
                            0,
                            cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                            cboPort.SelectedValue.ToString(), contype1, typeofship1,
                            RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                            cboJobType.SelectedValue.ToString(), cboTypeOfBE.SelectedValue.ToString(), cboRMSNonRMS.SelectedValue.ToString(),
                            Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(), bookname1);
                            Result = "Success";
                        }
                        else
                        {
                            String ChargeName = "";
                            DataTable dtExistRecord = new DataTable();
                            dtExistRecord = ViewState["GridTable"] as DataTable;
                            if(dtExistRecord.Rows.Count>0)
                            {
                                foreach (DataRow row in dtExistRecord.Rows)
                                {
                                    if(cboPort.SelectedItem.Text == row["Port"].ToString()  && row["ChargeCode"].ToString()== "A01")
                                    {
                                        dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                                        cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                                        contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text,
                                        RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text,
                                        cboUserSystem.SelectedItem.ToString(), cboJobType.SelectedItem.ToString(), cboTypeOfBE.SelectedItem.ToString(),
                                        cboRMSNonRMS.SelectedItem.ToString(), Deliverytype,
                                        0,
                                        cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                                        cboPort.SelectedValue.ToString(), contype1, typeofship1,
                                        RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                                        cboJobType.SelectedValue.ToString(), cboTypeOfBE.SelectedValue.ToString(), cboRMSNonRMS.SelectedValue.ToString(),
                                        Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(), bookname1);
                                        lberror.Text="";
                                        Result = "Success";
                                        break;
                                    }
                                    else if (cboPort.SelectedItem.Text != row["Port"].ToString() && cboChargeName.SelectedValue == "A01")
                                    {
                                        dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                                        cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                                        contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text,
                                        RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text,
                                        cboUserSystem.SelectedItem.ToString(), cboJobType.SelectedItem.ToString(), cboTypeOfBE.SelectedItem.ToString(),
                                        cboRMSNonRMS.SelectedItem.ToString(), Deliverytype,
                                        0,
                                        cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                                        cboPort.SelectedValue.ToString(), contype1, typeofship1,
                                        RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                                        cboJobType.SelectedValue.ToString(), cboTypeOfBE.SelectedValue.ToString(), cboRMSNonRMS.SelectedValue.ToString(),
                                        Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(), bookname1);
                                        lberror.Text = "";
                                        Result = "Success";
                                        break;
                                    }
                                    else
                                    {
                                        //please add attendance agency 
                                       // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + cboPort.SelectedValue + "'port need to add attendance agency');", true);
                                        lberror.Text = cboPort.SelectedItem.Text + " port need to add attendance agency";
                                    }
                                }
                            }
                            else
                            {
                                if(cboChargeName.SelectedValue == "A01")
                                {
                                    dt.Rows.Add(cboDivision.SelectedItem.ToString(), cboChargeName.SelectedItem.ToString(), cboCurrency.SelectedItem.ToString(),
                                    cboUOM.SelectedItem.ToString(), cboMode.SelectedItem.ToString(), cboPort.SelectedItem.ToString(),
                                    contype, typeofship, Convert.ToDouble(txtRate.Text), txtChargeCode.Text, txtHeading.Text,
                                    RangeCriteria, Convert.ToDecimal(txtRangeFrom.Text), Convert.ToDecimal(txtRangeto.Text), CAPCriteria, txtCapMin.Text, txtCapMax.Text,
                                    cboUserSystem.SelectedItem.ToString(), cboJobType.SelectedItem.ToString(), cboTypeOfBE.SelectedItem.ToString(),
                                    cboRMSNonRMS.SelectedItem.ToString(), Deliverytype,
                                    0,
                                    cboUOM.SelectedValue.ToString(), cboMode.SelectedValue.ToString(), cboCurrency.SelectedValue.ToString(),
                                    cboPort.SelectedValue.ToString(), contype1, typeofship1,
                                    RangeCriteria1, CAPCriteria1, cboUserSystem.SelectedValue.ToString(),
                                    cboJobType.SelectedValue.ToString(), cboTypeOfBE.SelectedValue.ToString(), cboRMSNonRMS.SelectedValue.ToString(),
                                    Deliverytype1, cboDivision.SelectedValue.ToString(), txtcondition.Text.ToString(), bookname1);
                                    lberror.Text = "";
                                    Result = "Success";
                                }
                                else
                                {
                                    //please add attendance agency
                                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('"+ cboPort.SelectedValue + "'port need to add attendance agency');", true);
                                    lberror.Text = cboPort.SelectedItem.Text + " port need to add attendance agency";
                                }
                            }
                        }

                        if(Result == "Success")
                        {
                            ViewState["GridTable"] = dt;
                            this.BindGrid();
                            ClearDetailData();
                            //ModalPopupContractBilling.Show();
                            Session["Operation"] = "Yes";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Details added successfully!!');", true);
                        }
                        
                    }
                    else
                    {
                        //lblerror1.Text = "Already Line Item......";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Already Line Item......');", true);
                    }
                }
            }
            Fieldset1.Visible = false;
            addvalid = "false";
            PanelContractBilling.Visible = true;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "AddContractLine", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lberror.Text = "" + ex.Message + "";
        }
    }
    private void clear_linedetails()
    {
        cboDivision.SelectedIndex = 0;
        cboCustomerName.Enabled = true;
        txtContractName.Enabled = true;
        txtHeading.Text = "";
        cboChargeName.SelectedIndex = 0;
        txtChargeCode.Text = "";
        cboUOM.SelectedIndex = 0;
        cboJobType.SelectedIndex = 0;
        cboTypeOfBE.SelectedIndex = 0;
        cboRMSNonRMS.SelectedIndex = 0;
        cboLoadedDeStuff.SelectedIndex = 0;
        cboRangeCriteria.SelectedIndex = 0;
        txtRangeFrom.Text = "";
        txtRangeto.Text = "";
        cboCAPCriteria.SelectedIndex = 0;
        txtCapMin.Text = "";
        txtCapMax.Text = "";
        cboMode.SelectedIndex = 0;
        cboPort.SelectedIndex = 0;
        cboContainerType.SelectedIndex = 0;
        cboTypeOfShipment.SelectedIndex = 0;
        cboUserSystem.SelectedIndex = 0;
        txtRate.Text = "";
        cboTypeOfBE.Enabled = false;
        cboLoadedDeStuff.Enabled = true;
        cboTypeOfShipment.Enabled = true;
        cboTypeOfShipment.SelectedIndex = 0;
        cboContainerType.Enabled = true;
        cboContainerType.SelectedIndex = 0;
        txtcondition.Text = "";
        cboFABook.SelectedIndex = 0;
    }
    private void clear()
    {
        cboCustomerName.SelectedIndex = 0;
        txtContractName.Text = "";
        dtStartDate.Text = "";
        dtEndDate.Text = "";

        cboDivision.SelectedIndex = 0;
        txtHeading.Text = "";
        cboChargeName.SelectedIndex = 0;
        txtChargeCode.Text = "";
        cboUOM.SelectedIndex = 0;
        cboJobType.SelectedIndex = 0;
        cboTypeOfBE.SelectedIndex = 0;
        cboRMSNonRMS.SelectedIndex = 0;
        cboLoadedDeStuff.SelectedIndex = 0;
        cboRangeCriteria.SelectedIndex = 0;
        txtRangeFrom.Text = "";
        txtRangeto.Text = "";
        cboCAPCriteria.SelectedIndex = 0;
        txtCapMin.Text = "";
        txtCapMax.Text = "";
        cboMode.SelectedIndex = 0;
        cboPort.SelectedIndex = 0;
        cboContainerType.SelectedIndex = 0;
        cboTypeOfShipment.SelectedIndex = 0;
        cboUserSystem.SelectedIndex = 0;
        txtRate.Text = "";
        cboRangeCriteria.Enabled = false;
        txtRangeFrom.Enabled = false;
        txtRangeto.Enabled = false;

        cboCAPCriteria.Enabled = false;
        txtCapMax.Enabled = false;
        txtCapMin.Enabled = false;
        dgvDetailData1.DataSource = null;
        dgvDetailData1.DataBind();
        //ddlSearchBy.SelectedIndex = 0;
        //txtSearch.Text = "";
        lberror.Text = "";
        lblerror1.Text = "";
        txtContractName.Enabled = true;
        txtContractName.Enabled = true;
        dtStartDate.Text = "";
        dtEndDate.Text = "";
        ddlPageSize.SelectedValue = "0";
        txtcondition.Text = "";
        cboFABook.SelectedIndex = 0;
        lnkDownload.Text = "";
        SearchGridData();
    }
    private void ClearDetailData()
    {
        try
        {
            cboUOM.SelectedIndex = 0;
            txtHeading.Text = string.Empty; ;
            cboChargeName.SelectedIndex = 0;
            txtChargeCode.Text = string.Empty; ;
            cboTypeOfShipment.SelectedIndex = 0;
            txtRate.Text = "0";
            txtcondition.Text = "";
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "ClearDetailData", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #region validation
    private bool validaterangeValues()
    {
        if ((!string.IsNullOrEmpty(txtRangeFrom.Text) && txtRangeFrom.Enabled && !string.IsNullOrEmpty(txtRangeto.Text) && txtRangeto.Enabled))
        {
            decimal rngFrom = Convert.ToDecimal(txtRangeFrom.Text);
            decimal rngTo = Convert.ToDecimal(txtRangeto.Text);
            if (rngFrom > rngTo)
            {
                //lberror.Text = "Range To should be higher than Range From";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Range To should be higher than Range From');", true);
                //dgvDetailData.DataSource = (DataTable)ViewState["GridTable"];
                //dgvDetailData.DataBind();
                return false;
            }
            else
                return true;
        }
        else
            return true;

    }
    private bool validateCapValues()
    {
        if ((!string.IsNullOrEmpty(txtCapMin.Text) && txtCapMin.Enabled && !string.IsNullOrEmpty(txtCapMax.Text) && txtCapMax.Enabled))
        {
            decimal CapMin = Convert.ToDecimal(txtCapMin.Text);
            decimal CapMax = Convert.ToDecimal(txtCapMax.Text);
            if (CapMin > CapMax)
            {
                //lberror.Text = "Cap Max should be higher than Cap Min";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Cap Max should be higher than Cap Min');", true);
                //dgvDetailData.DataSource = (DataTable)ViewState["GridTable"];
                //dgvDetailData.DataBind();
                return false;
            }
            else
                return true;
        }
        else
            return true;

    }
    protected void cboUOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cboUOM.SelectedItem.Text.ToUpper() == ("% of Value").ToUpper() || cboUOM.SelectedItem.Text.ToUpper() == ("% of CIF Value").ToUpper()
                || cboUOM.SelectedItem.Text.ToUpper() == ("% of FOB Value").ToUpper() || cboUOM.SelectedItem.Text.ToUpper() == ("Assessable value").ToUpper()
                || cboUOM.SelectedItem.Text.ToUpper() == ("Per Bill of Entry").ToUpper() || cboUOM.SelectedItem.Text.ToUpper() == ("Per Container").ToUpper()
                || cboUOM.SelectedItem.Text.ToUpper() == ("Per KG").ToUpper() || cboUOM.SelectedItem.Text.ToUpper() == ("Per MT").ToUpper()
                || cboUOM.SelectedItem.Text.ToUpper() == ("Per Package").ToUpper())
            {
                cboRangeCriteria.SelectedIndex = 0;
                txtRangeFrom.Text = "";
                txtRangeto.Text = "";

                cboCAPCriteria.SelectedIndex = 0;
                txtCapMax.Text = "";
                txtCapMin.Text = "";

                cboRangeCriteria.Enabled = true;
                txtRangeFrom.Enabled = true;
                txtRangeto.Enabled = true;

                cboCAPCriteria.Enabled = true;
                txtCapMax.Enabled = true;
                txtCapMin.Enabled = true;
            }
            else
            {
                cboRangeCriteria.SelectedIndex = 0;
                txtRangeFrom.Text = "";
                txtRangeto.Text = "";

                cboCAPCriteria.SelectedIndex = 0;
                txtCapMax.Text = "";
                txtCapMin.Text = "";

                cboRangeCriteria.Enabled = false;
                txtRangeFrom.Enabled = false;
                txtRangeto.Enabled = false;

                cboCAPCriteria.Enabled = false;
                txtCapMax.Enabled = false;
                txtCapMin.Enabled = false;
            }
            PanelContractBilling.Visible = true;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cboChargeName_Leave", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    protected void cboMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cboLoadedDeStuff.SelectedItem.Text != "" && cboLoadedDeStuff.SelectedItem.Text != "--Select--")
            {
                if (cboMode.SelectedItem.Text.ToUpper() == ("Sea").ToUpper() && cboLoadedDeStuff.SelectedItem.Text.ToUpper() != ("LCL").ToUpper())
                {
                    cboTypeOfShipment.Enabled = true;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = true;
                    cboContainerType.SelectedIndex = 0;
                }
                else
                {
                    cboTypeOfShipment.Enabled = false;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = false;
                    cboContainerType.SelectedIndex = 0;
                }

                if (cboMode.SelectedItem.Text.ToUpper() == ("Air").ToUpper())
                {
                    cboLoadedDeStuff.SelectedIndex = 0;
                    cboLoadedDeStuff.Enabled = false;
                }
                else
                {
                    cboLoadedDeStuff.Enabled = true;
                }

            }
            else
            {
                if (cboMode.SelectedItem.Text.ToUpper() == ("Air").ToUpper())
                {
                    cboTypeOfShipment.Enabled = false;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = false;
                    cboContainerType.SelectedIndex = 0;
                    cboLoadedDeStuff.SelectedIndex = 0;
                    cboLoadedDeStuff.Enabled = false;
                }
                else
                {
                    cboTypeOfShipment.Enabled = true;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = true;
                    cboContainerType.SelectedIndex = 0;
                    cboLoadedDeStuff.Enabled = true;
                }
            }
            ValidData();
            PanelContractBilling.Visible = true;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cboMode_Leave", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    protected void cboLoadedDeStuff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cboMode.SelectedItem.Text != "" && cboMode.SelectedItem.Text != "--Select--")
            {
                if (cboMode.SelectedItem.Text.ToUpper() == ("Sea").ToUpper() && cboLoadedDeStuff.SelectedItem.Text.ToUpper() == ("LCL").ToUpper())
                {
                    cboTypeOfShipment.Enabled = false;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = false;
                    cboContainerType.SelectedIndex = 0;
                }
                else
                {
                    cboTypeOfShipment.Enabled = true;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = true;
                    cboContainerType.SelectedIndex = 0;
                }
            }
            else
            {
                if (cboLoadedDeStuff.SelectedItem.Text.ToUpper() == ("LCL").ToUpper())
                {
                    cboTypeOfShipment.Enabled = false;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = false;
                    cboContainerType.SelectedIndex = 0;
                }
                else
                {
                    cboTypeOfShipment.Enabled = true;
                    cboTypeOfShipment.SelectedIndex = 0;
                    cboContainerType.Enabled = true;
                    cboContainerType.SelectedIndex = 0;
                }
            }
            if (cboContainerType.SelectedItem.Text.ToUpper() == ("FCL").ToUpper() && (cboLoadedDeStuff.SelectedItem.Text.ToUpper() != ("Loaded").ToUpper() || cboLoadedDeStuff.SelectedItem.Text.ToUpper() != ("DeStuff").ToUpper()))
            {
                cboContainerType.SelectedIndex = 0;
            }
            PanelContractBilling.Visible = true;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cboLoadedDeStuff_SelectedIndexChanged", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    protected void cboChargeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cboChargeName.SelectedItem.Text != "" || cboChargeName.SelectedItem.Text != "--Select--")
            {
                txtChargeCode.Text = cboChargeName.SelectedValue.ToString();
            }
            PanelContractBilling.Visible = true;

        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cboChargeName_SelectedIndexChanged", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    #endregion

    int ContractID = 0;
    //string ContractID = string.Empty;
    string ComContractUID = string.Empty;
    public static string UserLogin = "1";
    //protected void cmdClearline_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (dgvDetailData1.Rows.Count > 0)
    //        //{
    //        //    if (dgvDetailData1.Rows[0].Cells[1].Text != null)
    //        //    {
    //        //        if (dgvDetailData1.Rows[0].Cells[1].Text.ToString() != "")
    //        //        {
    //        //            dgvDetailData1.DeleteRow(Convert.ToInt32(ViewState["rowIndex"]));
    //        //        }
    //        //    }
    //        //}
    //        clear_linedetails();
    //        cmdAddLine.Enabled = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdClear_Click", "1", ex.Message);
    //        //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        lberror.Text = "" + ex.Message + "";
    //    }
    //}

    #region GridView Event
    // CB Billing Details Grid Filling
    protected void BindGrid()
    {
        dgvDetailData1.DataSource = (DataTable)ViewState["GridTable"];
        dgvDetailData1.DataBind();
    }
    private void GetCustomersPageWise(int pageIndex)
    {
        SqlConnection con = CDatabase.getConnection();
        //using (SqlConnection con = new SqlConnection(constring.ToString()))
        //{
        using (SqlCommand cmd = new SqlCommand("GetCustomersPageWise", con))
        {
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            //cmd.Parameters.AddWithValue("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            //cmd.Parameters.AddWithValue("@lid", Convert.ToInt32(Session["strlid"]));
            //cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
            //cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
            //con.Open();
            //IDataReader idr = cmd.ExecuteReader();
            DataTable dtDetailSearch = DBOperations.GetcontractBillingdata_Pageing(pageIndex, int.Parse(ddlPageSize.SelectedValue), Convert.ToInt32(Session["strlid"]));
            //ViewState["GridTable"] = dtDetailSearch;
            //dgvDetailData1.PageSize = int.Parse(ddlPageSize.SelectedValue);
            dgvDetailData1.DataSource = dtDetailSearch;
            dgvDetailData1.DataBind();
            //idr.Close();
            //con.Close();
            //ModalPopupContractBilling.Show();
            Fieldset1.Visible = false;
            PanelContractBilling.Visible = true;
            //int recordCount = Convert.ToInt32(4);
            //this.PopulatePager(recordCount, pageIndex);
        }
        //}
    }
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        this.GetCustomersPageWise(1);
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.GetCustomersPageWise(pageIndex);
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        //double dblPageCount = (double)((decimal)recordCount / decimal.Parse(ddlPageSize.SelectedValue));
        //int pageCount = (int)Math.Ceiling(dblPageCount);
        //List<DocumentFormat.OpenXml.Office2010.Excel.ListItem> pages = new List<ListItem>();
        //if (pageCount > 0)
        //{
        //    pages.Add(new ListItem("First", "1", currentPage > 1));
        //    for (int i = 1; i <= pageCount; i++)
        //    {
        //        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        //    }
        //    pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
        //}
        //rptPager.DataSource = pages;
        //rptPager.DataBind();
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = dgvDetailData_1.SelectedRow;
        txtHeading.Text = row.Cells[0].Text;
        try
        {
            //SearchData(Convert.ToInt32(e.CommandArgument.ToString()));
            //SearchData(Convert.ToInt32(row.Cells[0].Text));
            //updAll.Update();
            //UpdMain.Update();
            //ModalPopupContractBilling.Show();
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "dgvSearch_CellDoubleClick", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    protected void dgvDetailData_1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            Session["strlid"] = e.CommandArgument.ToString();
            SearchData(Convert.ToInt32(e.CommandArgument.ToString()));
            //Fieldset1.Visible = false;
            Session["Operation"] = "No";
            Session["savecon"] = "Yes";
            updAll.Update();
            cmdEditContractLine.Enabled = false;
            Fieldset1.Visible = false;
            btnNew.Visible = false;
            PanelContractBilling.Visible = true;
            //ModalPopupContractBilling.Show();
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        clear_linedetails();
        Session["strlid"] = "0";
        //ModalPopupContractBilling.Show();
        updAll.Update();
        Fieldset1.Visible = false;
        cboCustomerName.SelectedIndex = 0;
        txtContractName.Text = "";
        dtStartDate.Text = "";
        dtEndDate.Text = "";
        btnNew.Visible = false;
        dgvDetailData1.DataSource = "";
        dgvDetailData1.DataBind();
        PanelContractBilling.Visible = true;
    }
    protected void btncloseContractBilling_Click(object sender, EventArgs e)
    {
        //grdbillinglinedetails.DataSource = null;
        //grdbillinglinedetails.DataBind();
        //grdcontract_User.DataSource = null;
        //grdcontract_User.DataBind();
        //clear();
        //updAll.Update();
        UpdMain.Update();
        clear();
        Fieldset1.Visible = true;
        dgvDetailData_1.Visible = true;
        btnNew.Visible = true;
        PanelContractBilling.Visible = false;
        //ModalPopupContractBilling.Hide();
    }
    private void SearchData(int rowindex)
    {
        try
        {
            string sql = string.Empty;
            int curRow = rowindex;
            //Cursor.Current = Cursors.WaitCursor;
            //clear();
            sql = "select * from ContractMaster A LEFT JOIN CB_ContractDoc B ON A.lid=B.CBId AND B.bDel=0 where A.lid = '" + rowindex + "'";
            //DataTable dtSearchMain = DBOperations.FillTableData(sql);
            DataSet dtSearchMain = CDatabase.GetDataSet(sql);
            if (dtSearchMain.Tables[0].Rows.Count > 0)
            {
                cboCustomerName.SelectedValue = dtSearchMain.Tables[0].Rows[0]["CustomerId"].ToString(); //dtSearchMain.Rows[0]["CustomerId"].ToString();
                txtContractName.Text = dtSearchMain.Tables[0].Rows[0]["ContractName"].ToString(); //dtSearchMain.Rows[0]["ContractName"].ToString();
                cboCustomerName.Enabled = false;
                txtContractName.Enabled = false;
                dtStartDate.Text = Convert.ToDateTime(dtSearchMain.Tables[0].Rows[0]["ContractStartDate"].ToString()).ToString("dd/MMM/yyyy");
                dtEndDate.Text = Convert.ToDateTime(dtSearchMain.Tables[0].Rows[0]["ContractEndDate"].ToString()).ToString("dd/MMM/yyyy");
                ComContractUID = dtSearchMain.Tables[0].Rows[0]["ContractUID"].ToString();
                ContractID = Convert.ToInt32(dtSearchMain.Tables[0].Rows[0]["lid"].ToString());
                lnkDownload.Text = dtSearchMain.Tables[0].Rows[0]["DocPath"].ToString();  //FileName
                ViewState["ContractID"] = dtSearchMain.Tables[0].Rows[0]["lid"].ToString();
                //sql = "select * from CB_BillingDetail where CMID = '" + GridView1.Rows[curRow].Cells[0].Text.ToString() + "' order by lid";
                Session["lid"] = rowindex;
                DataTable dtDetailSearch = DBOperations.GetcontractBillingdata(rowindex);
                ViewState["GridTable"] = dtDetailSearch;
                //dgvDetailData1.DataSource = dtDetailSearch;
                //dgvDetailData1.DataBind();
                //dgvDetailData1.PageSize= int.Parse(ddlPageSize.SelectedValue);


                ///////
                DataTable dtDetailSearch1 = DBOperations.GetcontractBillingdata_Pageing(1, int.Parse(ddlPageSize.SelectedValue), Convert.ToInt32(Session["strlid"]));
                //ViewState["GridTable"] = dtDetailSearch1;
                //dgvDetailData1.PageSize = int.Parse(ddlPageSize.SelectedValue);
                dgvDetailData1.DataSource = dtDetailSearch1;
                dgvDetailData1.DataBind();
                //ModalPopupContractBilling.Show();
                Fieldset1.Visible = false;
                PanelContractBilling.Visible = true;
                //////////////
                cmdsave.Enabled = true;
                cmdAddLine.Enabled = true;
                cmdEditContractLine.Enabled = true;
                //cmdRemove.Enabled = false;
            }
            //Cursor.Current = Cursors.Default;
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SearchData", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Billing Details Line item Grid
    protected void OnSelectedIndexChanged_edit(object sender, EventArgs e)
    {
        GridViewRow row = dgvDetailData1.SelectedRow;
        txtHeading.Text = row.Cells[0].Text;
        string strValue = ((HiddenField)dgvDetailData1.SelectedRow.Cells[23].FindControl("hflid")).Value;
        try
        {
            SearchData_edit(Convert.ToInt32(strValue));
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "dgvSearch_CellDoubleClick", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void SearchData_edit(int rowindex)
    {
        try
        {
            string sql = string.Empty;
            int curRow = rowindex;
            //cmdAddLine.Enabled = false;
            //Cursor.Current = Cursors.WaitCursor;
            clear();
            DataTable dtDetailSearch = DBOperations.Usp_GetContractbillingrData_edit(rowindex);
            if (dtDetailSearch.Rows.Count > 0)
            {
                cboCustomerName.SelectedValue = dtDetailSearch.Rows[0][1].ToString();
                txtContractName.Text = dtDetailSearch.Rows[0][2].ToString();
                dtStartDate.Text = dtDetailSearch.Rows[0][4].ToString();
                dtEndDate.Text = dtDetailSearch.Rows[0][5].ToString();
                cboDivision.SelectedValue = dtDetailSearch.Rows[0][7].ToString();
                txtHeading.Text = dtDetailSearch.Rows[0][8].ToString();
                if (dtDetailSearch.Rows[0][10].ToString() == "")
                {
                    cboChargeName.SelectedValue = "0";
                }
                else
                {
                    cboChargeName.SelectedValue = dtDetailSearch.Rows[0][10].ToString();
                }
                txtChargeCode.Text = dtDetailSearch.Rows[0][10].ToString();
                cboUOM.SelectedValue = dtDetailSearch.Rows[0][11].ToString();
                cboJobType.SelectedValue = dtDetailSearch.Rows[0][12].ToString();
                cboTypeOfBE.SelectedValue = dtDetailSearch.Rows[0][13].ToString();
                cboRMSNonRMS.SelectedValue = dtDetailSearch.Rows[0][14].ToString();
                cboLoadedDeStuff.SelectedValue = dtDetailSearch.Rows[0][15].ToString();
                cboCurrency.SelectedValue = dtDetailSearch.Rows[0][16].ToString();
                cboRangeCriteria.SelectedValue = dtDetailSearch.Rows[0][17].ToString();
                txtRangeFrom.Text = dtDetailSearch.Rows[0][18].ToString();
                txtRangeto.Text = dtDetailSearch.Rows[0][19].ToString();
                cboCAPCriteria.SelectedValue = dtDetailSearch.Rows[0][20].ToString();
                txtCapMin.Text = dtDetailSearch.Rows[0][21].ToString();
                txtCapMax.Text = dtDetailSearch.Rows[0][22].ToString();
                cboMode.SelectedValue = dtDetailSearch.Rows[0][23].ToString();
                cboPort.SelectedValue = dtDetailSearch.Rows[0][24].ToString();
                cboContainerType.SelectedValue = dtDetailSearch.Rows[0][25].ToString();
                cboTypeOfShipment.SelectedValue = dtDetailSearch.Rows[0][26].ToString();
                cboUserSystem.SelectedValue = dtDetailSearch.Rows[0][27].ToString();
                txtRate.Text = dtDetailSearch.Rows[0][28].ToString();
                cboFABook.SelectedValue = dtDetailSearch.Rows[0][29].ToString();
            }
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SearchData", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // Line item edit any information
    protected void cmdEditContractLine_Click(object sender, EventArgs e)
    {
        try
        {
            addvalid = "false";
            DataTableandGridBind(Convert.ToInt32(ViewState["rowIndex"]));
            cmdsave.Enabled = true;
            Session["Operation"] = "Yes";
            updAll.Update();
            Fieldset1.Visible = false;
            btnNew.Visible = false;
            //cmdAddLine.Enabled = true;
            PanelContractBilling.Visible = true;
            //EditContractLine();
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "dgvDetailData_CellDoubleClick", "1", ex.Message);
            //MessageBox.Show(ex.Message, "Platinum", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void DataTableandGridBind(int rowIndex)
    {
        SqlCommand command = new SqlCommand();
        SqlCommand cmdCon = new SqlCommand();
        SqlConnection con = new SqlConnection();
        validation_AddLine();
        if (addvalid == "false")
        {
            con = CDatabase.getConnection();
            con.Open();
            string sql;
            DataTable dtGridVIew;
            ArrayList aList = new ArrayList();
            int lid = 0;
            string chargeName = string.Empty;
            string RangeCriteria; int RangeCriteria1 = 0;
            if (cboRangeCriteria.SelectedItem.ToString() == "--Select--") { RangeCriteria = ""; RangeCriteria1 = 0; txtRangeFrom.Text = "0"; txtRangeto.Text = "0"; }
            else { RangeCriteria = cboRangeCriteria.SelectedItem.ToString(); RangeCriteria1 = Convert.ToInt32(cboRangeCriteria.SelectedValue); }
            string CAPCriteria; int CAPCriteria1 = 0;
            if (cboCAPCriteria.SelectedItem.ToString() == "--Select--") { CAPCriteria = ""; CAPCriteria1 = 0; txtCapMax.Text = "0"; txtCapMin.Text = "0"; }
            else { CAPCriteria = cboCAPCriteria.SelectedItem.ToString(); CAPCriteria1 = Convert.ToInt32(cboCAPCriteria.SelectedValue); }
            string contype; int contype1 = 0;
            if (cboContainerType.SelectedItem.ToString() == "--Select--") { contype = ""; contype1 = 0; }
            else { contype = cboContainerType.SelectedItem.ToString(); contype1 = Convert.ToInt32(cboContainerType.SelectedValue); }
            string typeofship; int typeofship1 = 0;
            if (cboTypeOfShipment.SelectedItem.ToString() == "--Select--") { typeofship = ""; typeofship1 = 0; }
            else { typeofship = cboTypeOfShipment.SelectedItem.ToString(); typeofship1 = Convert.ToInt32(cboTypeOfShipment.SelectedValue); }
            string Deliverytype; int Deliverytype1 = 0;
            if (cboLoadedDeStuff.SelectedItem.ToString() == "--Select--") { Deliverytype = ""; Deliverytype1 = 0; }
            else { Deliverytype = cboLoadedDeStuff.SelectedItem.ToString(); Deliverytype1 = Convert.ToInt32(cboLoadedDeStuff.SelectedValue); }

            if (txtRate.Text == "0" || txtRate.Text == "0.00")
            {
                Session["Operation"] = "No";
                cmdAddLine.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly Add Rate greater than Zero.');", true);
            }
            else
            {
                sql = "Select count(*) from  CB_BillingDetail  where CMID = " + Session["strlid"] + " and ChargeCode = '" + txtChargeCode.Text + "' and Heading = '" + txtHeading.Text + "' " +
                "and CurrencyID = " + cboCurrency.SelectedValue + " and UOMID = " + cboUOM.SelectedValue + " and isnull(RangeCriteriaID,0)= " + RangeCriteria1 + "" +
                "and isnull(RangeFrom,0)= " + txtRangeFrom.Text + " and isnull(RangeTo,0)= " + txtRangeto.Text + " and isnull(CapCriteriaID,0)= " + CAPCriteria1 + " and isnull(CapMax,0)= " + txtCapMax.Text + " and isnull(CapMin,0)= " + txtCapMin.Text + "" +
                "and UserSystemID = " + cboUserSystem.SelectedValue + " and ModeID = " + cboMode.SelectedValue + " and PortID = " + cboPort.SelectedValue + " and isnull(TypeOfShipmentID,0)= " + typeofship1 + "" +
                "and isnull(ContainerTypeID,0)= " + contype1 + " and JobTypeID = " + cboJobType.SelectedValue + " and TypeOfBEID = " + cboTypeOfBE.SelectedValue + "" +
                "and RMSNonRMSID = " + cboRMSNonRMS.SelectedValue + " and LoadedDeStuffID = " + cboLoadedDeStuff.SelectedIndex + " and DivisionId = " + cboDivision.SelectedValue + " and Rate =" + txtRate.Text + " and Condition='" + txtcondition.Text + "' and bdel=0 and fabookid='" + cboFABook.SelectedValue +"'";
                cmdCon.CommandText = sql;
                cmdCon.Connection = con;
                cmdCon.CommandType = CommandType.Text;
                int countcont = Convert.ToInt32(cmdCon.ExecuteScalar());
                if (countcont == 0)
                {

                    lberror.Text = "";
                    if (ViewState["GridTable"] != null)
                    {
                        dtGridVIew = ((DataTable)ViewState["GridTable"]);
                        lid = Convert.ToInt32(dtGridVIew.Rows[rowIndex][22].ToString());
                        //if (rowIndex >= 0)
                        //    dtGridVIew.Rows.RemoveAt(rowIndex);
                    }

                    else
                    {
                        dtGridVIew = new DataTable();
                        dtGridVIew.Columns.Add("Sl");
                        if (dgvDetailData1.Columns.Count > 0)
                            for (int i = 1; i < dgvDetailData1.Columns.Count; i++)
                            {
                                if (!dtGridVIew.Columns.Contains(dgvDetailData1.Columns[i].SortExpression) && !String.IsNullOrEmpty(dgvDetailData1.Columns[i].SortExpression))
                                    dtGridVIew.Columns.Add(dgvDetailData1.Columns[i].SortExpression);
                            }
                    }


                    if (dtGridVIew.Rows.Count > 0)

                        for (int i = 0; i < dtGridVIew.Rows.Count; i++)
                        {
                            int dtRowIndex = dtGridVIew.Rows.Count <= i ? dtGridVIew.Rows.Count - 1 : i;
                            chargeName = dtGridVIew.Rows[dtRowIndex]["ChargeName"].ToString();

                            if (dtGridVIew.Rows[dtRowIndex]["TypeOfBE"].ToString().ToUpper().Contains("N/A"))
                                aList.Add(chargeName + "TypeOfBE");
                            if (dtGridVIew.Rows[dtRowIndex]["Port"].ToString().ToUpper().Contains("N/A"))
                                aList.Add(chargeName + "Port");
                            if (dtGridVIew.Rows[dtRowIndex]["RMSNonRMSName"].ToString().ToUpper().Contains("N/A"))
                                aList.Add(chargeName + "RMSNonRMSName");
                            if (dtGridVIew.Rows[dtRowIndex]["LoadedDeStuff"].ToString().ToUpper().Contains("N/A"))
                                aList.Add(chargeName + "LoadedDeStuff");
                            if (dtGridVIew.Rows[dtRowIndex]["TypeOfShipment"].ToString().ToUpper().Contains("N/A"))
                                aList.Add(chargeName + "TypeOfShipment");


                            // }
                        }

                    //dtGridVIew.Rows.Add();
                    int gridRowIndex = dtGridVIew.Rows.Count - 1;
                    gridRowIndex = rowIndex;
                    string error = string.Empty;
                    chargeName = cboChargeName.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["ChargeName"] = chargeName;
                    dtGridVIew.Rows[gridRowIndex]["DivisionName"] = cboDivision.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["DivisionId"] = cboDivision.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["Heading"] = txtHeading.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["ChargeCode"] = cboChargeName.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["Currency"] = cboCurrency.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["CurrencyID"] = cboCurrency.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["UOM"] = cboUOM.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["UOMID"] = cboUOM.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["JobType"] = cboJobType.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["JobTypeID"] = cboJobType.SelectedItem.Value;
                    string TypeOfBE = cboTypeOfBE.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["TypeOfBE"] = TypeOfBE;
                    dtGridVIew.Rows[gridRowIndex]["TypeOfBEID"] = cboTypeOfBE.SelectedValue;
                    string RMSNonRMSName = cboRMSNonRMS.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["RMSNonRMSName"] = RMSNonRMSName;
                    if (!RMSNonRMSName.ToUpper().Contains("N/A"))
                    {
                        if (aList.Contains(chargeName + "RMSNonRMSName"))
                            error = error + ", RMSNonRMS";
                    }
                    dtGridVIew.Rows[gridRowIndex]["RMSNonRMSID"] = cboRMSNonRMS.SelectedItem.Value;
                    
                    if (cboLoadedDeStuff.SelectedItem.Text == "--Select--")
                    {
                        dtGridVIew.Rows[gridRowIndex]["LoadedDeStuff"] = 0;
                        dtGridVIew.Rows[gridRowIndex]["LoadedDeStuffID"] = 0;
                    }
                    else
                    {
                        string LoadedDeStuff = cboLoadedDeStuff.SelectedItem.Text.Trim().Replace("amp;", "");
                        dtGridVIew.Rows[gridRowIndex]["LoadedDeStuff"] = LoadedDeStuff;
                        dtGridVIew.Rows[gridRowIndex]["LoadedDeStuffID"] = cboLoadedDeStuff.SelectedItem.Value;
                    }
                    //if (!LoadedDeStuff.ToUpper().Contains("N/A"))
                    //{
                    //    if (aList.Contains(chargeName + "LoadedDeStuff"))
                    //        error = error + ", LoadedDeStuff";
                    //}
                    if (cboRangeCriteria.SelectedItem.Text == "--Select--")
                    {
                        dtGridVIew.Rows[gridRowIndex]["RangeCriteria"] = "";
                    }
                    else
                    {
                        dtGridVIew.Rows[gridRowIndex]["RangeCriteria"] = cboRangeCriteria.SelectedItem.Text.Trim().Replace("amp;", "");
                    }
                    dtGridVIew.Rows[gridRowIndex]["RangeCriteriaID"] = cboRangeCriteria.SelectedIndex;// cboRangeCriteria.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["RangeTo"] = txtRangeto.Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                    dtGridVIew.Rows[gridRowIndex]["RangeFrom"] = txtRangeFrom.Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                    if (cboCAPCriteria.SelectedItem.Text == "--Select--")
                    {
                        dtGridVIew.Rows[gridRowIndex]["CapCriteria"] = "";
                    }
                    else
                    {
                        dtGridVIew.Rows[gridRowIndex]["CapCriteria"] = cboCAPCriteria.SelectedItem.Text.Trim().Replace("amp;", "");
                    }
                    dtGridVIew.Rows[gridRowIndex]["CapCriteriaID"] = cboCAPCriteria.SelectedIndex;// cboCAPCriteria.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["CapMin"] = txtCapMin.Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                    dtGridVIew.Rows[gridRowIndex]["CapMax"] = txtCapMax.Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                    dtGridVIew.Rows[gridRowIndex]["Mode"] = cboMode.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["ModeID"] = cboMode.SelectedItem.Value;
                    string port = cboPort.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["Port"] = port;
                    if (!port.ToUpper().Contains("N/A"))
                    {
                        if (aList.Contains(chargeName + "Port"))
                            error = "Port";
                    }
                    dtGridVIew.Rows[gridRowIndex]["PortID"] = cboPort.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["UserSystem"] = cboUserSystem.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["UserSystemID"] = cboUserSystem.SelectedItem.Value;
                    dtGridVIew.Rows[gridRowIndex]["Rate"] = Convert.ToDecimal(txtRate.Text.Trim().Replace("amp;", ""));
                    //
                    if (cboContainerType.SelectedItem.Value == "--Select--")
                    {
                        dtGridVIew.Rows[gridRowIndex]["ContainerType"] = 0;
                        dtGridVIew.Rows[gridRowIndex]["ContainerTypeID"] = 0;
                    }
                    else
                    {
                        dtGridVIew.Rows[gridRowIndex]["ContainerType"] = cboContainerType.SelectedItem.Text.Trim().Replace("amp;", "");
                        dtGridVIew.Rows[gridRowIndex]["ContainerTypeID"] = cboContainerType.SelectedItem.Value;
                    }
                   // 
                    if (cboTypeOfShipment.SelectedItem.Value == "--Select--")
                    {
                        dtGridVIew.Rows[gridRowIndex]["TypeOfShipment"] = 0;
                        dtGridVIew.Rows[gridRowIndex]["TypeOfShipmentID"] = 0;
                    }
                    else
                    {
                        string TypeOfShipment = cboTypeOfShipment.SelectedItem.Text.Trim().Replace("amp;", "");
                        dtGridVIew.Rows[gridRowIndex]["TypeOfShipment"] = TypeOfShipment;
                        dtGridVIew.Rows[gridRowIndex]["TypeOfShipmentID"] = cboTypeOfShipment.SelectedItem.Value;
                        //if (!TypeOfShipment.ToUpper().Contains("N/A"))
                        //{
                        //    if (aList.Contains(chargeName + "TypeOfShipment"))
                        //        error = error + ", TypeOfShipment";
                        //}
                    }
                    dtGridVIew.Rows[gridRowIndex]["Condition"] = txtcondition.Text.ToString();
                    if (!string.IsNullOrEmpty(error))
                    {
                        lberror.Text = "Invalid value(s) in " + error;
                        gridBind();
                        return;
                    }

                   // dtGridVIew.Rows[gridRowIndex]["fabookname"] = cboFABook.SelectedItem.Text.Trim().Replace("amp;", "");
                    dtGridVIew.Rows[gridRowIndex]["fabookid"] = cboFABook.SelectedItem.Value;

                    ViewState["GridTable"] = dtGridVIew;
                    dgvDetailData1.DataSource = dtGridVIew;
                    dgvDetailData1.DataBind();
                    clear_linedetails();
                    cmdAddLine.Enabled = true;
                    //cmdAddLine.Enabled = false;
                    //gridBind();
                    updAll.Update();
                    //ModalPopupContractBilling.Show();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Details Updated Successfully!!');", true);
                }
                else
                {
                    //lblerror1.Text = "Already Line Item......"; 
                    Session["Operation"] = "No";
                    clear_linedetails();
                    cmdAddLine.Enabled = true;
                    updAll.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Already Line Item......');", true);
                }
            }
        }
        Fieldset1.Visible = false;
        addvalid = "false";
        PanelContractBilling.Visible = true;
    }
    private void gridBind()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["GridTable"];

            var distinctContainer = dt.AsEnumerable()
                         .Select(row => new
                         {
                             ChargeName = row.Field<string>("ChargeName"),
                             ContainerType = row.Field<string>("ContainerType"),
                             TypeOfShipment = row.Field<string>("TypeOfShipment"),
                             TypeOfBE = row.Field<string>("TypeOfBE"),
                             RMSNonRMSName = row.Field<string>("RMSNonRMSName"),
                             LoadedDeStuff = row.Field<string>("LoadedDeStuff")
                         }).ToList();

            var distinctValues = dt.AsEnumerable()
                         .Select(row => new
                         {
                             ChargeName = row.Field<string>("ChargeName"),
                             Currency = row.Field<string>("Currency"),
                             UOM = row.Field<string>("UOM"),
                             Mode = row.Field<string>("Mode"),
                             Port = row.Field<string>("Port"),
                             ContainerType = row.Field<string>("ContainerType"),

                             TypeOfShipment = row.Field<string>("TypeOfShipment"),
                             RangeCriteria = row.Field<string>("RangeCriteria"),
                             CapCriteria = row.Field<string>("CapCriteria"),
                             UserSystem = row.Field<string>("UserSystem"),
                             JobType = row.Field<string>("JobType"),
                             TypeOfBE = row.Field<string>("TypeOfBE"),

                             RMSNonRMSName = row.Field<string>("RMSNonRMSName"),
                             LoadedDeStuff = row.Field<string>("LoadedDeStuff"),
                             //FactoryStuffedDockStuffed = row.Field<string>("FactoryStuffedDockStuffed"),
                             ChargeCode = row.Field<string>("ChargeCode"),
                             Heading = row.Field<string>("Heading"),
                             CurrencyID = row.Field<string>("CurrencyID"),

                             UOMID = row.Field<string>("UOMID"),
                             RangeCriteriaID = row.Field<string>("RangeCriteriaID"),
                             RangeFrom = row.Field<string>("RangeFrom"),
                             RangeTo = row.Field<string>("RangeTo"),
                             CapCriteriaID = row.Field<string>("CapCriteriaID"),
                             CapMin = row.Field<string>("CapMin"),
                             CapMax = row.Field<string>("CapMax"),

                             UserSystemID = row.Field<string>("UserSystemID"),
                             ModeID = row.Field<string>("ModeID"),
                             PortID = row.Field<string>("PortID"),
                             TypeOfShipmentID = row.Field<string>("TypeOfShipmentID"),
                             ContainerTypeID = row.Field<string>("ContainerTypeID"),
                             JobTypeID = row.Field<string>("JobTypeID"),
                             TypeOfBEID = row.Field<string>("TypeOfBEID"),
                             RMSNonRMSID = row.Field<string>("RMSNonRMSID"),
                             LoadedDeStuffID = row.Field<string>("LoadedDeStuffID"),
                             divisionms = row.Field<string>("DivisionId"),
                             //StuffedID = row.Field<string>("StuffedID"),
                             Rate = row.Field<string>("Rate")
                         }).ToList();
            //             .Distinct().ToList();

            if (dt.Rows.Count > distinctValues.Count)
            {
                lberror.Text = "Duplicate values cannot be added!!";
                lberror.CssClass = "errorMsg";
            }
            else
            {
                lberror.Text = "Details added successfully!!";
                lberror.CssClass = "success";

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + lberror.Text + "');", true);
            dgvDetailData1.PageIndex = 0;
            dgvDetailData1.DataSource = distinctValues;
            dgvDetailData1.DataBind();
        }
        catch (Exception ex)
        {
            dgvDetailData1.DataBind();
            ErrorLog.DisplayExcetions(ex, false, updAll);
        }
    }

    protected void dgvDetailData1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvDetailData1.PageIndex = e.NewPageIndex;
        dgvDetailData1.DataSource = (DataTable)ViewState["GridTable"];
        dgvDetailData1.DataBind();
        //if (ViewState["contId"] != null && !PnlAdd.Visible)
        if (ViewState["contId"] != null)
        {
            //btnEditCancel.Visible = true;
            //btnModify.Visible = true;
            dgvDetailData1.Columns[dgvDetailData1.Columns.Count - 2].Visible = false;
            dgvDetailData1.Columns[dgvDetailData1.Columns.Count - 1].Visible = false;

        }
        PanelContractBilling.Visible = true;
        //ModalPopupContractBilling.Show();
        Fieldset1.Visible = false;
        PanelContractBilling.Visible = true;
    }

    // Line item remove 
    protected void dgvDetailData1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditItem")
            {
                Session["Status"] = e.CommandName;
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                ViewState["rowIndex"] = rowIndex;
                int pageIndex = Convert.ToInt32(dgvDetailData1.PageIndex);

                if (pageIndex > 0)
                {
                    rowIndex = rowIndex % dgvDetailData1.PageSize;
                }

                cboDivision.SelectedIndex = cboDivision.Items.IndexOf(cboDivision.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["DivisionId"].ToString()));
                cboDivision_SelectedIndexChanged(sender, e);
                cboJobType.SelectedIndex = cboJobType.Items.IndexOf(cboJobType.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["JobTypeID"].ToString()));
                cboTypeOfBE.SelectedIndex = cboTypeOfBE.Items.IndexOf(cboTypeOfBE.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["TypeOfBEID"].ToString()));
                cboLoadedDeStuff_SelectedIndexChanged(sender, e);
                cboChargeName.SelectedIndex = cboChargeName.Items.IndexOf(cboChargeName.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["ChargeCode"].ToString()));
                txtChargeCode.Text = dgvDetailData1.DataKeys[rowIndex].Values["ChargeCode"].ToString();
                cboCurrency.SelectedIndex = cboCurrency.Items.IndexOf(cboCurrency.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["CurrencyID"].ToString()));
                cboUOM.SelectedIndex = cboUOM.Items.IndexOf(cboUOM.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["UOMID"].ToString()));
                cboRMSNonRMS.SelectedIndex = cboRMSNonRMS.Items.IndexOf(cboRMSNonRMS.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["RMSNonRMSID"].ToString()));
                cboLoadedDeStuff.SelectedIndex = cboLoadedDeStuff.Items.IndexOf(cboLoadedDeStuff.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["LoadedDeStuffID"].ToString()));

                cboMode.SelectedIndex = cboMode.Items.IndexOf(cboMode.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["ModeID"].ToString()));
                cboMode_SelectedIndexChanged(sender, e);
                cboPort.SelectedIndex = cboPort.Items.IndexOf(cboPort.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["PortID"].ToString()));
                cboRangeCriteria.SelectedIndex = cboRangeCriteria.Items.IndexOf(cboRangeCriteria.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["RangeCriteriaID"].ToString()));
                cboCAPCriteria.SelectedIndex = cboCAPCriteria.Items.IndexOf(cboCAPCriteria.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["CapCriteriaID"].ToString()));


                cboContainerType.SelectedIndex = cboContainerType.Items.IndexOf(cboContainerType.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["ContainerTypeID"].ToString()));
                cboTypeOfShipment.SelectedIndex = cboTypeOfShipment.Items.IndexOf(cboTypeOfShipment.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["TypeOfShipmentID"].ToString()));
                cboUserSystem.SelectedIndex = cboUserSystem.Items.IndexOf(cboUserSystem.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["UserSystemID"].ToString()));
                cboFABook.SelectedIndex = cboFABook.Items.IndexOf(cboFABook.Items.FindByValue(dgvDetailData1.DataKeys[rowIndex].Values["fabookid"].ToString()));

                GridViewRow row = dgvDetailData1.Rows[rowIndex];
                txtHeading.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "Heading")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtRate.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "Rate")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtRangeFrom.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "RangeFrom")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtRangeFrom.Enabled = string.IsNullOrEmpty(txtRangeFrom.Text) ? false : true;
                txtRangeto.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "RangeTo")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtRangeto.Enabled = string.IsNullOrEmpty(txtRangeto.Text) ? false : true;
                txtCapMin.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "CapMin")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtCapMin.Enabled = string.IsNullOrEmpty(txtCapMin.Text) ? false : true;
                txtCapMax.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "CapMax")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");
                txtCapMax.Enabled = string.IsNullOrEmpty(txtCapMax.Text) ? false : true;
                txtcondition.Text = dgvDetailData1.Rows[rowIndex].Cells[GetColumnIndexByName(row, "Condition")].Text.Trim().Replace("amp;", "").Replace("&nbsp;", "");

                cmdEditContractLine.Enabled = true;
                //cmdRemove.Enabled = true;
                cmdsave.Enabled = true;
                cmdAddLine.Enabled = false;
                //updAll.Update();
                Fieldset1.Visible = false;
                PanelContractBilling.Visible = true;
            }
            if (e.CommandName == "Delete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                ViewState["deleterowindex"] = rowIndex;
                ////OnSelectedIndexChanged(sender, e);
                //DataTable dt = (DataTable)ViewState["GridTable"];
                //dt.Rows.RemoveAt(rowIndex);
                ////DataTable dtDetailSearch1 = DBOperations.GetcontractBillingdata_Pageing(1, int.Parse(ddlPageSize.SelectedValue), Convert.ToInt32(Session["strlid"]));
                ////ViewState["GridTable"] = dtDetailSearch1;
                ////updAll.Update();
                //dgvDetailData1.DataSource = dt;
                //dgvDetailData1.DataBind();
                //updAll.Update();
                //Fieldset1.Visible = false;
                //btnNew.Visible = false;
                //PanelContractBilling.Visible = true;
                ////ModalPopupContractBilling.Show();
                ////Fieldset1.Visible = false;
                ////PanelContractBilling.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Line item details Remove Sucessfully.');", true);
                //// ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Contract saved successfully !');", true);
            }
        }
        catch (Exception ex)
        {

            ErrorLog.DisplayExcetions(ex, false, updAll);
        }
    }

    protected void dgvDetailData1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        SqlConnection con = CDatabase.getConnection();
        int index = Convert.ToInt32(dgvDetailData1.DataKeys[e.RowIndex].Values["lid"].ToString());
        //Convert.ToInt32(ViewState["deleterowindex"]);
        //GridViewRow row = dgvDetailData1.Rows[index];
        //Convert.ToInt32(e.RowIndex);
        //int lid = row.;
        DataTable dt = (DataTable)ViewState["GridTable"];
        //dt.Rows[index].Delete();
        //ViewState["GridTable"] = dt;
        //dgvDetailData1.DataSource = dt;
        //dgvDetailData1.DataBind();

        SqlCommand cmd = new SqlCommand("update CB_BillingDetail set bDel=1 where lid=" + index + "", con);
        cmd.Parameters.AddWithValue("id", index);
        con.Open();
        int id = cmd.ExecuteNonQuery();
        con.Close();

        //string sql = "select * from CB_BillingDetail where CMID = " + Session["lid"] + " and bDel=0";
        //DataSet dscbbill = CDatabase.GetDataSet(sql);

        //ViewState["GridTable"] = dscbbill.Tables[0];
        DataTable dtDetailSearch = DBOperations.GetcontractBillingdata(Convert.ToInt32(Session["lid"]));
        ViewState["GridTable"] = dtDetailSearch;
        dgvDetailData1.DataSource = ViewState["GridTable"];
        dgvDetailData1.DataBind();


        updAll.Update();
        Fieldset1.Visible = false;
        btnNew.Visible = false;
        PanelContractBilling.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Remove Contract line successfully !');", true);
    }
    int GetColumnIndexByName(GridViewRow row, string columnName)
    {
        int columnIndex = 0;
        foreach (DataControlFieldCell cell in row.Cells)
        {
            if (cell.ContainingField is BoundField)
                if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                    break;
            columnIndex++; // keep adding 1 while we don't have the correct name
        }
        return columnIndex;
    }
    protected void deletecontractline_Click(object sender, EventArgs e)
    {
        SqlConnection con = CDatabase.getConnection();
        foreach (GridViewRow gvrow in dgvDetailData1.Rows)
        {
            if (gvrow.RowIndex == Convert.ToInt32(ViewState["deleterowindex"]) && gvrow.RowIndex != 0)
            {
                var hflid = gvrow.FindControl("hflid") as HiddenField;

                ///SqlCommand cmd = new SqlCommand("delete from CB_BillingDetail where lid=@id", con);
                //SqlCommand cmd = new SqlCommand("update CB_BillingDetail set bDel=1 where lid=@id", con);
                //cmd.Parameters.AddWithValue("id", int.Parse(hflid.Value));
                //con.Open();
                //int id = cmd.ExecuteNonQuery();
                //con.Close();
            }
        }
        ViewState["deleterowindex"] = "";


    }
    #endregion

    #region SavedContract

    protected void cmdsave_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand command = new SqlCommand();
            SqlCommand cmdCon = new SqlCommand();
            SqlConnection con = new SqlConnection();
            con = CDatabase.getConnection();
            con.Open();
            string sql;
            sql = "Select count(*)  from ContractMaster where CustomerId = " + cboCustomerName.SelectedValue + " and ('" + dtStartDate.Text.ToString() + "' between ContractStartDate and  ContractEndDate)  and ('" + dtStartDate.Text.ToString() + "' between ContractStartDate and  ContractEndDate)";
            cmdCon.CommandText = sql;
            cmdCon.Connection = con;
            cmdCon.CommandType = CommandType.Text;
            int countcont;
            if (Session["savecon"] == "Yes")
            {
                countcont = 0;
            }
            else
            {
                countcont = Convert.ToInt32(cmdCon.ExecuteScalar());
            }
            if(Session["Operation"].ToString() != "Yes")
            {
                int CheckContract = DBOperations.CheckContractExist(Convert.ToInt32(cboCustomerName.SelectedValue), dtStartDate.Text.ToString(), dtEndDate.Text.ToString());
                if (CheckContract == 0)
                {
                    countcont = 0;
                }
                else
                {
                    countcont = CheckContract;
                }
            }
            

            if (countcont == 0)
            {
                if (Session["Operation"] == "Yes")
                {
                    if (dgvDetailData1.Rows.Count > 0)
                    {
                        SaveData();
                        if (Session["chksave"] == "ok")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "<script> alert('Contract saved successfully111 !');</script>", true);

                            PanelContractBilling.Visible = false;
                            clear();
                            UpdMain.Update();
                            updAll.Update();
                            //
                            DataFilter1.DataSource = GridviewSqlDataSource;
                            DataFilter1.DataColumns = dgvDetailData_1.Columns;
                            DataFilter1.FilterSessionID = "ContractBilling.aspx";
                            DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
                            //
                            Fieldset1.Visible = true;
                            dgvDetailData_1.Visible = true;
                            btnNew.Visible = true;
                            Panel6.Visible = true;
                            //Response.Redirect("ContractBilling.aspx", false);
                        }
                    }
                    else
                    {
                        updAll.Update();
                        Fieldset1.Visible = false;
                        cboCustomerName.SelectedIndex = 0;
                        txtContractName.Text = "";
                        dtStartDate.Text = "";
                        dtEndDate.Text = "";
                        btnNew.Visible = false;
                        dgvDetailData1.DataSource = "";
                        dgvDetailData1.DataBind();
                        PanelContractBilling.Visible = true;
                        Session["savecon"] = "No";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Contract Not Save because please Enter Line item..');", true);
                    }
                    Session["Operation"] = "No";
                }
                else
                {
                    if(Session["Status"].ToString()== "EditItem")
                    {
                        Session["Status"] = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please first update line item then save contract.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please first add line item then save contract.');", true);
                    }
                    
                    PanelContractBilling.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert(Already Contract');", true);
                lberror.Text = "Already Available contract";
                PanelContractBilling.Visible = true;
            }
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdsave_Click", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    string UID;
    public string FindUniqueCode(string ID, string ShortName)
    {
        if (ID.Length == 1)
        {
            UID = ShortName + "000000" + ID;
        }
        if (ID.Length == 2)
        {
            UID = ShortName + "00000" + ID;
        }
        if (ID.Length == 3)
        {
            UID = ShortName + "0000" + ID;
        }
        if (ID.Length == 4)
        {
            UID = ShortName + "000" + ID;
        }
        if (ID.Length == 5)
        {
            UID = ShortName + "00" + ID;
        }
        if (ID.Length == 6)
        {
            UID = ShortName + "0" + ID;
        }
        if (ID.Length == 7)
        {
            UID = ShortName + "" + ID;
        }
        return UID.ToUpper().Trim();
    }

    private string UploadDocument(string FilePath)
    {
        string FileName = fuContractFile.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == null)
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\ContractCopy\\" + FilePath);
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
        if (fuContractFile.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuContractFile.SaveAs(ServerFilePath + FileName);

            return FileName;
        }

        else
        {
            return "";
        }

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
    private void validation_Save()
    {
        try
        {
            if (Convert.ToDateTime(dtStartDate.Text) > Convert.ToDateTime(dtEndDate.Text))
            {
                //lberror.Text = "Start date should not be greater than start date !";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Start date should not be greater than End date !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Start date should not be greater than End date ! !');", true);
                PanelContractBilling.Visible = true;
                Session["chksave"] = "Nok";
                dtEndDate.Focus();
                return;
            }

            if (cboCustomerName.Text == "" || cboCustomerName.Text == "--Select--")
            {
                //lberror.Text = "Kindly select customer name !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly select customer name !');", true);
                cboCustomerName.Focus();
                Session["chksave"] = "Nok";
                return;
            }
            if (txtContractName.Text == string.Empty || txtContractName.Text == "")
            {
                //lberror.Text = "Kindly enter conytract name !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly enter contract name !');", true);
                PanelContractBilling.Visible = true;
                Session["chksave"] = "Nok";
                txtContractName.Focus();
                return;
            }

            if (dgvDetailData1.Rows.Count > 0)
            {
                if (dgvDetailData1.Rows[0].Cells[1].Text != null)
                {
                    if (dgvDetailData1.Rows[0].Cells[1].ToString() == "")
                    {
                        //lberror.Text = "Kindly add contract line !";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly add contract line !');", true);
                        return;
                    }
                }
                else
                {
                    //lberror.Text = "Kindly add contract line !";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly add contract line !');", true);
                    return;
                }
            }
            else
            {
                //lberror.Text = "Kindly add contract line !";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kindly add contract line !');", true);
                return;
            }
            //if (ContractID == 0 || ContractID == null)
            //{

            //    string selsql = "Select * from ContractMaster where '" + Convert.ToDateTime(dtStartDate.Text).ToString("yyyy-MM-dd") + "' between ContractStartDate and ContractEndDate";
            //    selsql = selsql + " and '" + Convert.ToDateTime(dtEndDate.Text).ToString("yyyy-MM-dd") + "' between ContractStartDate and ContractEndDate and CustomerId = '" + cboCustomerName.SelectedValue.ToString() + "'";
            //    DataTable dtDup = DBOperations.FillTableData(selsql);
            //    if (dtDup.Rows.Count > 0)
            //    {
            //        //lberror.Text = "Contract is already enterred for selected customer and date range !";
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Contract is already enterred for selected customer and date range !');", true);
            //        return;
            //    }
            //}
            //else
            //{
            //    string selsql = "Select * from ContractMaster where '" + Convert.ToDateTime(dtStartDate.Text).ToString("yyyy-MM-dd") + "' between ContractStartDate and ContractEndDate";
            //    selsql = selsql + " and '" + Convert.ToDateTime(dtEndDate.Text).ToString("yyyy-MM-dd") + "' between ContractStartDate and ContractEndDate and CustomerId = '" + cboCustomerName.SelectedValue.ToString() + "'";
            //    selsql = selsql + " and lid <> '" + ContractID + "'";
            //    DataTable dtDup = DBOperations.FillTableData(selsql);
            //    if (dtDup.Rows.Count > 0)
            //    {
            //        //lberror.Text = "Contract is already enterred for selected customer and date range !";
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Contract is already enterred for selected customer and date range !');", true);
            //        return;
            //    }
            //}

            //DBOperations dbopr = new DBOperations();
        }
        catch (Exception ex)
        {

            DateTime dt = Convert.ToDateTime(dtStartDate.Text.ToString());
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SaveData", "1", ex.Message + Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' '))) + dt);
            lberror.Text = "" + ex.Message + "";
        }

    }
    private void SaveData()
    {
        try
        {
            validation_Save();
            string strValue;
            DataTable dTable = new DataTable();
            dTable.Columns.Add("ChargeCode");
            dTable.Columns.Add("Heading");
            dTable.Columns.Add("CurrencyID");
            dTable.Columns.Add("UOMID");
            dTable.Columns.Add("RangeCriteriaID");
            dTable.Columns.Add("RangeFrom");
            dTable.Columns.Add("RangeTo");
            dTable.Columns.Add("CapCriteriaID");
            dTable.Columns.Add("CapMin");
            dTable.Columns.Add("CapMax");
            dTable.Columns.Add("UserSystemID");
            dTable.Columns.Add("ModeID");
            dTable.Columns.Add("PortID");
            dTable.Columns.Add("TypeOfShipmentID");
            dTable.Columns.Add("ContainerTypeID");
            dTable.Columns.Add("JobTypeID");
            dTable.Columns.Add("TypeOfBEID");
            dTable.Columns.Add("RMSNonRMSID");
            dTable.Columns.Add("LoadedDeStuffID");
            dTable.Columns.Add("DivisionId");
            dTable.Columns.Add("Rate");
            dTable.Columns.Add("CMID");
            dTable.Columns.Add("lid");
            dTable.Columns.Add("Condition");
            dTable.Columns.Add("fabookid");

            DataTable dtt = ((DataTable)ViewState["GridTable"]);
            int divisionid = 0;
            foreach (DataRow dRow in dtt.Rows)
            {
                DataRow dR = dTable.NewRow();
                dR["ChargeCode"] = dRow["ChargeCode"];
                dR["Heading"] = dRow["Heading"];
                dR["CurrencyID"] = dRow["CurrencyID"];
                dR["UOMID"] = dRow["UOMID"];
                dR["RangeCriteriaID"] = dRow["RangeCriteriaID"];
                dR["RangeFrom"] = string.IsNullOrEmpty(dRow["RangeFrom"].ToString()) ? DBNull.Value : dRow["RangeFrom"];
                dR["RangeTo"] = string.IsNullOrEmpty(dRow["RangeTo"].ToString()) ? DBNull.Value : dRow["RangeTo"];
                dR["CapCriteriaID"] = dRow["CapCriteriaID"];
                dR["CapMin"] = string.IsNullOrEmpty(dRow["CapMin"].ToString()) ? DBNull.Value : dRow["CapMin"];
                dR["CapMax"] = string.IsNullOrEmpty(dRow["CapMax"].ToString()) ? DBNull.Value : dRow["CapMax"];
                dR["UserSystemID"] = dRow["UserSystemID"];
                dR["ModeID"] = dRow["ModeID"];
                dR["PortID"] = dRow["PortID"];
                dR["TypeOfShipmentID"] = dRow["TypeOfShipmentID"];
                dR["ContainerTypeID"] = dRow["ContainerTypeID"];
                dR["JobTypeID"] = dRow["JobTypeID"];
                dR["TypeOfBEID"] = dRow["TypeOfBEID"];
                dR["RMSNonRMSID"] = dRow["RMSNonRMSID"];
                dR["LoadedDeStuffID"] = dRow["LoadedDeStuffID"];
                dR["DivisionId"] = dRow["DivisionId"];
                dR["Rate"] = dRow["Rate"];
                dR["lid"] = dRow["lid"];
                dR["Condition"] = dRow["Condition"];
                dR["fabookid"] = dRow["fabookid"];
                dTable.Rows.Add(dR);
            }
            if (ViewState["ContractID"] != null)
            {
                ContractID = Convert.ToInt32(ViewState["ContractID"].ToString());
            }
            divisionid = Convert.ToInt32(dTable.Rows[0][19].ToString());

            SqlCommand command = new SqlCommand();
            SqlCommand cmdCon = new SqlCommand();
            SqlConnection con = new SqlConnection();
            con = CDatabase.getConnection();
            con.Open();
            string sql;

            sql = "Select count(*)  from ContractMaster where CustomerId = " + cboCustomerName.SelectedValue + " and ('" + dtStartDate.Text.ToString() + "' between ContractStartDate and  ContractEndDate)  and ('" + dtStartDate.Text.ToString() + "' between ContractStartDate and  ContractEndDate)";
            cmdCon.CommandText = sql;
            cmdCon.Connection = con;
            cmdCon.CommandType = CommandType.Text;
            int countcont;
            if (Session["savecon"] == "Yes")
            {
                countcont = 0;
                //int CheckContract = DBOperations.CheckContractExist(Convert.ToInt32(cboCustomerName.SelectedValue), dtStartDate.Text.ToString(), dtEndDate.Text.ToString());
                //if (CheckContract == 0)
                //{
                //    countcont = 0;
                //}
                //else
                //{
                //    countcont = CheckContract;
                //}
            }
            else
            {
                countcont = Convert.ToInt32(cmdCon.ExecuteScalar());
            }
            if (countcont == 0)
            {
                cmdCon.Parameters.Clear();
                if (ContractID == 0 || ContractID == null)
                {
                    ContractID = 0;
                    sql = "Select isnull(max(lid),0)+1 lid from ContractMaster";
                    cmdCon.CommandText = sql;
                    cmdCon.Connection = con;
                    cmdCon.CommandType = CommandType.Text;
                    ID = Convert.ToString(cmdCon.ExecuteScalar());

                    //ID = DBOperations.FindUniqueID("lid", "ContractMaster", "");
                    ComContractUID = FindUniqueCode(ID, cboCustomerName.SelectedItem.Text.Replace(" ", "").Substring(0, 3));
                }
                //DateTime date1 = Convert.ToDateTime(dtStartDate.Text.ToString());
                //DateTime date2 = Convert.ToDateTime(dtEndDate.Text.ToString());
                //DateTime dt = Convert.ToDateTime(dtStartDate.Text.ToString()); 
                //DateTime dt1 = DateTime.Parse(dtEndDate.Text);
                //string stdt = DateTime.Parse(dtStartDate.Text.Trim()).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                //string enddt = dt1.ToString("yyyy/MM/dd");

                cmdCon.CommandText = "usp_InsertUpdatetbl_ContractMaster";
                cmdCon.Connection = con;
                cmdCon.CommandType = CommandType.StoredProcedure;
                cmdCon.Parameters.AddWithValue("@lid", ContractID);
                cmdCon.Parameters.AddWithValue("@ContractUID", ComContractUID);
                cmdCon.Parameters.AddWithValue("@CustomerId", cboCustomerName.SelectedValue.ToString());
                cmdCon.Parameters.AddWithValue("@ContractName", txtContractName.Text);
                cmdCon.Parameters.AddWithValue("@DivisionId", divisionid);
                cmdCon.Parameters.AddWithValue("@ContractStartDate", dtStartDate.Text.ToString());
                cmdCon.Parameters.AddWithValue("@ContractEndDate", dtEndDate.Text.ToString());
                cmdCon.Parameters.AddWithValue("@lUser", DBOperations.UserLogin.ToString());
                //cmdCon.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("MM/dd/yyyy"));
                cmdCon.Parameters.AddWithValue("@bDel", "0");
                cmdCon.ExecuteNonQuery();


                string ContractLid = "0";
                //string sql = string.Empty;
                if (ContractID == 0 || ContractID == null)
                {
                    cmdCon.Parameters.Clear();
                    sql = "Select * from ContractMaster where ContractUID = '" + ComContractUID + "'";
                    cmdCon.CommandText = sql;
                    cmdCon.CommandType = CommandType.Text;
                    SqlDataReader rdr = cmdCon.ExecuteReader();
                    rdr.Read();
                    if (rdr.HasRows)
                    {
                        ContractID = Convert.ToInt32(rdr["lid"].ToString());
                    }
                    rdr.Close();
                }
                //else
                //{
                //    ContractLid = ContractID.ToString();

                //    //cmdCon.Parameters.Clear();
                //    //sql = "delete from CB_BillingDetail where CMID = '" + ContractLid + "' and bDel=0";
                //    //cmdCon.CommandText = sql;
                //    //cmdCon.CommandType = CommandType.Text;
                //    //cmdCon.ExecuteNonQuery();

                //}

                //-------------------------CONTRACT COPY UPLOADD----------------------------------------------------------------
                string DocPath = "";
                int Result = ContractOperation.UpdateCBIdForContractDoc(Convert.ToInt32(Session["DocId"]), Convert.ToInt32(ContractID));
                if (Result == 0)
                {
                    lberror.Text = "Successfully added document";
                }
               
                //int DocId=ContractOperation.AddContractdocPath(fuContractCopy.fil, ContractID)
                //--------------------------------------------------------------------------------------------------------------

                ViewState["ContractID"] = ContractID;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Usp_InsertCBBillingDetails";
                command.Connection = CDatabase.getConnection();
                command.Connection.Open();
                if (dTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dTable.Rows)
                    {
                        if (row[1].ToString() != null)
                        {

                            command.Parameters.Clear();
                            command.Parameters.Add("@lid", SqlDbType.VarChar).Value = Convert.ToInt32(row[22].ToString());
                            // final insert data
                            command.Parameters.Add("@ChargeCode", SqlDbType.VarChar).Value = row[0].ToString();
                            command.Parameters.Add("@Heading", SqlDbType.VarChar).Value = row[1].ToString();
                            command.Parameters.Add("@CurrencyID", SqlDbType.VarChar).Value = row[2].ToString();
                            command.Parameters.Add("@UOMID", SqlDbType.VarChar).Value = row[3].ToString();
                            if (row[4].ToString() == "--Select--" || row[4].ToString() == "0" || row[4].ToString() == "")
                            {
                                command.Parameters.Add("@RangeCriteriaID", SqlDbType.VarChar).Value = "0";
                                command.Parameters.Add("@RangeFrom", SqlDbType.Int).Value = 0;
                                command.Parameters.Add("@RangeTo", SqlDbType.Int).Value = 0;
                            }
                            else
                            {
                                command.Parameters.Add("@RangeCriteriaID", SqlDbType.VarChar).Value = row[4].ToString();
                                command.Parameters.Add("@RangeFrom", SqlDbType.VarChar).Value = row[5].ToString();
                                command.Parameters.Add("@RangeTo", SqlDbType.VarChar).Value = row[6].ToString();
                            }
                            if (row[7].ToString() == "--Select--" || row[7].ToString() == "0" || row[7].ToString() == "")
                            {
                                command.Parameters.Add("@CapCriteriaID", SqlDbType.VarChar).Value = "0";
                                command.Parameters.Add("@CapMin", SqlDbType.Int).Value = 0;
                                command.Parameters.Add("@CapMax", SqlDbType.Int).Value = 0;
                            }
                            else
                            {
                                command.Parameters.Add("@CapCriteriaID", SqlDbType.VarChar).Value = row[7].ToString();
                                command.Parameters.Add("@CapMin", SqlDbType.VarChar).Value = row[8].ToString();
                                command.Parameters.Add("@CapMax", SqlDbType.VarChar).Value = row[9].ToString();
                            }
                            command.Parameters.Add("@UserSystemID", SqlDbType.VarChar).Value = row[10].ToString();
                            command.Parameters.Add("@ModeID", SqlDbType.VarChar).Value = row[11].ToString();
                            command.Parameters.Add("@PortID", SqlDbType.VarChar).Value = row[12].ToString();
                            command.Parameters.Add("@TypeOfShipmentID", SqlDbType.VarChar).Value = row[13].ToString();
                            command.Parameters.Add("@ContainerTypeID", SqlDbType.VarChar).Value = row[14].ToString();
                            command.Parameters.Add("@JobTypeID", SqlDbType.VarChar).Value = row[15].ToString();
                            command.Parameters.Add("@TypeOfBEID", SqlDbType.VarChar).Value = row[16].ToString();
                            command.Parameters.Add("@RMSNonRMSID", SqlDbType.VarChar).Value = row[17].ToString();
                            command.Parameters.Add("@LoadedDeStuffID", SqlDbType.VarChar).Value = row[18].ToString();
                            command.Parameters.Add("@Rate", SqlDbType.Decimal).Value = row[20].ToString();
                            //command.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("MM/dd/yyyy"));
                            command.Parameters.Add("@bDel", SqlDbType.VarChar).Value = "0";
                            command.Parameters.Add("@CMID", SqlDbType.VarChar).Value = ViewState["ContractID"];
                            command.Parameters.Add("@DivisionId", SqlDbType.VarChar).Value = divisionid;
                            command.Parameters.Add("@IsBillDone", SqlDbType.VarChar).Value = "N";
                            command.Parameters.Add("@Condition", SqlDbType.VarChar).Value = row[23].ToString();
                            command.Parameters.Add("@fabookid", SqlDbType.VarChar).Value = row[24].ToString();
                            //command.Parameters.Add("@Result ", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                            //command.Parameters["@Result"].Value ="";
                            //command.ExecuteNonQuery();
                            string con1 = command.ExecuteScalar().ToString();
                            //string con1 = Convert.ToString(command.Parameters["@Result"].Value);
                        }
                    }
                }

                //SearchGridData();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messearchsage", "alert('Contract saved successfully !');", true);

                Session["chksave"] = "ok";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert(Already Contract');", true);
            }
        }
        catch (Exception ex)
        {

            DateTime dt = Convert.ToDateTime(dtStartDate.Text.ToString());
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "SaveData", "1", ex.Message + Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' '))) + dt);
            lberror.Text = "" + ex.Message + "";
        }
    }
    #endregion
    protected void cmdclear_Click(object sender, EventArgs e)
    {
        clear();
        cmdAddLine.Enabled = true;
        cmdsave.Enabled = true;
    }

    protected void cboDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = string.Empty;
        DataSet dsPlant;
        //if (cboDivision.SelectedItem.Text == "CB – Exports")
        if (cboDivision.SelectedItem.Text.ToUpper() == ("CB – Exports").ToUpper())
        {
            cboTypeOfBE.Enabled = true;
            cboTypeOfBE.Items.Clear();
            dsPlant = DBOperations.GetContractMasterData(6);
            cboTypeOfBE.Items.Insert(0, "--Select--");
            cboTypeOfBE.DataSource = dsPlant;
            cboTypeOfBE.DataTextField = "BETypeName";
            cboTypeOfBE.DataValueField = "BETypeId";
            cboTypeOfBE.DataBind();
            cboRMSNonRMS.Items.Clear();
            cboContainerType.Items.Clear();
            cboTypeOfShipment.Items.Clear();
            //clear_linedetails();
            FillExportDropdown();
            cboDivision.SelectedValue = "2";
        }
        //else if (cboDivision.SelectedItem.Text == "CB – Imports")
        else if (cboDivision.SelectedItem.Text.ToUpper() == ("CB – Imports").ToUpper())
        {
            //clear_linedetails();
            Filldropdown();
            cboTypeOfBE.Enabled = true;
            cboTypeOfBE.Items.Clear();
            dsPlant = DBOperations.GetContractMasterData(61);
            cboTypeOfBE.Items.Insert(0, "--Select--");
            cboTypeOfBE.DataSource = dsPlant;
            cboTypeOfBE.DataTextField = "BETypeName";
            cboTypeOfBE.DataValueField = "BETypeId";
            cboTypeOfBE.DataBind();
            cboDivision.SelectedValue = "1";

        }
        else { cboTypeOfBE.Enabled = false; }
        PanelContractBilling.Visible = true;
    }
    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        if (dgvDetailData1.Rows.Count > 0)
        {
            string strFileName = "ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
        }
        else
        {
            updAll.Update();
            Fieldset1.Visible = false;
            cboCustomerName.SelectedIndex = 0;
            txtContractName.Text = "";
            dtStartDate.Text = "";
            dtEndDate.Text = "";
            btnNew.Visible = false;
            dgvDetailData1.DataSource = "";
            dgvDetailData1.DataBind();
            PanelContractBilling.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Data is blank so Excel not genrated !');", true);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            //string FileName = "d:\\ContractExcel\\ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            string FileName = "ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            dgvDetailData1.GridLines = GridLines.Both;
            dgvDetailData1.HeaderStyle.Font.Bold = true;

            //dgvDetailData1.Columns[37].Visible = false;
            //dgvDetailData1.Columns[38].Visible = false;
            dgvDetailData1.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdClear_Click", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }

    protected void lnkexport1_Click(object sender, EventArgs e)
    {
        if (dgvDetailData_1.Rows.Count > 0)
        {
            string strFileName = "ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

            ExportFunction1("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
        }
        else
        {
            updAll.Update();
            Fieldset1.Visible = false;
            cboCustomerName.SelectedIndex = 0;
            txtContractName.Text = "";
            dtStartDate.Text = "";
            dtEndDate.Text = "";
            btnNew.Visible = false;
            dgvDetailData_1.DataSource = "";
            dgvDetailData_1.DataBind();
            PanelContractBilling.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Data is blank so Excel not genrated !');", true);
        }
    }

    private void ExportFunction1(string header, string contentType)
    {
        try
        {
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            ////string FileName = "d:\\ContractExcel\\ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            //string FileName = "ContractList_" + cboCustomerName.Text + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
            //StringWriter strwritter = new StringWriter();
            //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            ////dgvDetailData_1.GridLines = GridLines.Both;
            ////dgvDetailData_1.HeaderStyle.Font.Bold = true;

            ////dgvDetailData1.Columns[37].Visible = false;
            ////dgvDetailData1.Columns[38].Visible = false;
            //dgvDetailData_1.RenderControl(htmltextwrtter);
            //Response.Write(strwritter.ToString());
            //Response.End();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            dgvDetailData_1.AllowPaging = false;
            dgvDetailData_1.AllowSorting = false;

            dgvDetailData_1.Columns[1].Visible = false;
            dgvDetailData_1.Columns[2].Visible = true;

            dgvDetailData_1.DataBind();

            dgvDetailData_1.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            DBOperations.UpdateErrorLog(DBOperations.UserLogin, "", "cmdClear_Click", "1", ex.Message);
            lberror.Text = "" + ex.Message + "";
        }
    }
    #endregion

    public string ValidData()
    {
        string Mode="", Port="", BEType="",fabookid="";
        
        DataTable dt = new DataTable();
        if(dgvDetailData1.Rows.Count>0)
        {
            // Add columns to DataTable
            foreach (TableCell cell in dgvDetailData1.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text); // Or add column names as needed
                if (cell.Text == "&nbsp;")
                {
                    break;
                }
            }

            // Add data rows to DataTable
            foreach (GridViewRow row in dgvDetailData1.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < row.Cells.Count - 2; i++)
                {
                    dataRow[i] = row.Cells[i].Text; // Or row.Cells[i].Value for other types
                }
                dt.Rows.Add(dataRow);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Mode = row["Mode"].ToString();
                    Port = row["Port"].ToString();
                    BEType = row["Type Of BE"].ToString();
                    fabookid = row["fabookid"].ToString();
                    if (Mode == cboMode.SelectedItem.ToString() && Port == cboPort.SelectedItem.ToString() && BEType == cboTypeOfBE.SelectedItem.ToString())
                    {
                        cboFABook.SelectedValue = fabookid;

                    }
                }
            }
        }
        
        PanelContractBilling.Visible = true;
        return fabookid;
    }

    public void ContractCopyUpload()
    {
        string DocPath = "";
        string fileName = Path.GetFileName(fuContractFile.FileName);
        DataTable dtCustDetail = ContractOperation.GetCustDetail(Convert.ToInt32(cboCustomerName.SelectedValue));
        if (dtCustDetail.Rows.Count > 0)
        {
            DocPath = dtCustDetail.Rows[0]["DocFolder"].ToString();
        }
        string strFilePath = DocPath + "\\";
        string strFileName = "";
        if (strFilePath == "")
            strFilePath = "PreAlertDoc\\";

        strFileName = UploadDocument(strFilePath);

        if (strFileName != "")
        {
            int Result = ContractOperation.AddContractdocPath(fuContractFile.FileName, strFilePath, ContractID, LoggedInUser.glUserId);
            if(Result>0)
            {
                Session["DocId"] = Result;
            }
            lnbContractCopy.Text = fuContractFile.FileName;
        }
    }

    protected void cboTypeOfBE_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidData();
    }

    protected void cboPort_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidData();
    }

    protected void lnbContractCopy_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        PanelContractBilling.Visible = true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string FileName = fuContractFile.FileName;

        string DocPath = "";
        string fileName = Path.GetFileName(fuContractFile.FileName);
        DataTable dtCustDetail = ContractOperation.GetCustDetail(Convert.ToInt32(cboCustomerName.SelectedValue));
        if (dtCustDetail.Rows.Count > 0)
        {
            DocPath = dtCustDetail.Rows[0]["DocFolder"].ToString();
        }
        string strFilePath = DocPath + "\\";
        string strFileName = "";
        if (strFilePath == "")
            strFilePath = "ContractDoc\\";

        strFileName = UploadDocument(strFilePath);

        if (strFileName != "")
        {
            int Result = ContractOperation.AddContractdocPath(fuContractFile.FileName, strFilePath, Convert.ToInt32(ViewState["ContractID"]), LoggedInUser.glUserId);
            lblContractCopyerror.Text = "Success";
            lblContractCopyerror.Text = "Success";
        }

        ModalPopupExtender1.Show();
        PanelContractBilling.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string FileName = fuContractFile.FileName;

        string DocPath = "";
        string fileName = Path.GetFileName(fuContractFile.FileName);
        DataTable dtCustDetail = ContractOperation.GetCustDetail(Convert.ToInt32(cboCustomerName.SelectedValue));
        if (dtCustDetail.Rows.Count > 0)
        {
            DocPath = dtCustDetail.Rows[0]["DocFolder"].ToString();
        }
        string strFilePath = DocPath + "\\";
        string strFileName = "";
        if (strFilePath == "")
            strFilePath = "..\\UploadFiles\\ContractCopy\\";

        strFileName = UploadDocument(strFilePath);

        if (strFileName != "")
        {
            int Result = ContractOperation.AddContractdocPath(fuContractFile.FileName, strFilePath+ fileName, Convert.ToInt32(ViewState["ContractID"]), LoggedInUser.glUserId);
            if (Result > 0)
            {
                Session["DocId"] = Result;
            }
            lnbContractCopy.Text = fuContractFile.FileName;
        }

        PanelContractBilling.Visible = true;
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == null)
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\ContractCopy" + DocumentPath);
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

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        string ContractCopy = lnkDownload.Text;
        DownloadDocument(ContractCopy);
        PanelContractBilling.Visible = true;
    }
}