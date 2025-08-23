using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class BSCCPLTransport_PendingTransBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkJobExport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Transport Bill";

            Session["TransReqId"] = null;
            Session["TransporterID"] = null;
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingTransBill.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    #region Job Detail
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string TransReqId = gvJobDetail.DataKeys[gvrow.RowIndex].Values[0].ToString();

            Session["TransReqId"] = TransReqId;

            Session["TransporterId"] = e.CommandArgument.ToString();
            Response.Redirect("BillSubmission.aspx");
        }
    }

    #endregion

    #region Data Filter

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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
            DataFilter1.FilterSessionID = "PendingTransBill.aspx";
            //DataFilter1.DataColumns = gvJobDetail.Columns;
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
            if (gvJobDetail.Rows.Count == 0)
            {
                lblError.Text = "No Pending Bill Detail Found!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "";
            }
        }
        catch (Exception ex)
        {
            // DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkJobExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls");
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;

        gvJobDetail.Caption = "Pending Transport Bill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm");
        DataFilter1.FilterSessionID = "PendingTransBill.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}