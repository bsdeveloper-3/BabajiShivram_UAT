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
using System.IO;

public partial class Master_ShippingMaster : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(btnNew);
        if (!IsPostBack)
        {
            Session["SCode"] = null; // Used Only For Customer Master 
            Session["ReportCustomerId"] = null; // Used Only For Customer Adhoc Report

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Shipping Line Detail";
            ScodeAutoGenerate();
        }
        //
        DataFilter1.DataSource = GridViewSqlDataSource;
        DataFilter1.DataColumns = GridView1.Columns;
        DataFilter1.FilterSessionID = "ShippingMaster.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "navigate")
        {

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            Session["SCode"] = commandArgs[0];   // CustId
            Session["ShipId"] = commandArgs[1];

            //string strShippingCustomerId = e.CommandArgument.ToString();
            //Session["SCode"] = strShippingCustomerId;

            Response.Redirect("ShippingDetails.aspx");
        }
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

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
            DataFilter1.FilterSessionID = "ShippingMaster.aspx";
            DataFilter1.FilterDataSource();
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        ExportFunction("attachment;filename=CompanyList.xls", "application/vnd.ms-excel");
    }
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
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView1.AllowPaging = false;
        GridView1.AllowSorting = false;
        //GridView1.Columns[0].Visible = false;
        GridView1.Columns[1].Visible = false;
        GridView1.Columns[2].Visible = true;

        //GridView1.Columns[7].Visible = true;
        //GridView1.Columns[8].Visible = true;
        //GridView1.Columns[9].Visible = true;
        //GridView1.Columns[10].Visible = true;
        //GridView1.Columns[11].Visible = true;

        GridView1.DataSourceID = "GridViewSqlDataSource";
        GridView1.DataBind();

        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //// gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}


        GridView1.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ScodeAutoGenerate();
    }
    public void ScodeAutoGenerate()
    {
        DataSet dsSCode = new DataSet();

        string strSCode = DBOperations.GetNextScodeAutoGenerate();
        //string Result = SEZOperation.GetGenerateSEZJobNo(i);
        if (strSCode != "")
        {                        
            lblSCode.Text = strSCode;
            // ModalPopupStatus.Show();   
            txtShipCompName.Text = string.Empty;
            txtAddress.Text = string.Empty;

            GridView1.DataSourceID = "GridViewSqlDataSource";
            GridView1.DataBind();
        }
    }

    protected void BtnSaveStatus_Click(object sender, EventArgs e)
    {
        Boolean IsValid = true;
        lblPopMessage.Visible = true;
        lblPopMessage.Text = "";

        string strSCode     = lblSCode.Text.Trim();//Convert.ToInt32(hdnJobId.Value);
        string CompName     = txtShipCompName.Text.Trim();
        string CompAddress  = txtAddress.Text.Trim();
        string ShippingLineCode = txtShipCompCode.Text.Trim();

        if (CompName == "" || CompName.Length < 10)
        {
            lblPopMessage.Text = "Please Enter Company Name.<BR>";
            lblPopMessage.CssClass = "errorMsg";

            IsValid = false;
        }
        else if(ShippingLineCode.Length != 4)
        {
            lblPopMessage.Text = "Please Enter 4 Char Shipping Line Code Code.<BR>";
            lblPopMessage.CssClass = "errorMsg";
            IsValid = false;
        }

        if (IsValid)
        {
            int Result = DBOperations.AddShippingCompanyMaster(strSCode, CompName, ShippingLineCode, CompAddress, loggedinuser.glUserId);

            if (Result == 1)
            {
                lblPopMessage.Text = "Record Added Successfully.<BR>";
                lblPopMessage.CssClass = "success";

                txtShipCompName.Text = "";
                txtAddress.Text = "";
                ScodeAutoGenerate();
            }
            else if (Result == 0)
            {
                lblPopMessage.Visible = true;
                lblPopMessage.Text = "System Error! Please check the required field.";
                lblPopMessage.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblPopMessage.Visible = true;
                lblPopMessage.Text = "Record Already Exist";
                lblPopMessage.CssClass = "errorMsg";
            }
        }

        ModalPopupStatus.Show();
        ScodeAutoGenerate();
    }

}