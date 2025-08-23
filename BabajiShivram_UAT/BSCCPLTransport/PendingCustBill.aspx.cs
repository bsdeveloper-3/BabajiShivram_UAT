using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class BSCCPLTransport_PendingCustBill : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkJobExport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Pending Customer Bill";

            Session["TransReqId"] = null;
            Session["TransporterID"] = null;
            Session["JobID"] = null;
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "Bill1.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

    }

    #region Job Detail
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName.ToLower() == "select")
        {            
            Session["TransReqId"] = e.CommandArgument.ToString();
            Response.Redirect("BillSubmissionCust.aspx");
        }
        else if(e.CommandName.ToLower() == "post")
        {
            int TransReqid = Convert.ToInt32(e.CommandArgument.ToString());

            int result =  TransOperation.AddCustomerBillPost(TransReqid,LoggedInUser.glUserId);

            if(result == 0)
            {
                lblError.Text = "Bill details sucessfully Posted to FA System!";
                lblError.CssClass = "success";
            }
            else if(result == 1)
            {
                lblError.Text = "System Error!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Bill details already posted!";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error!";
                lblError.CssClass = "errorMsg";
            }
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
        Response.AddHeader("content-disposition", "attachment;filename=" + "TransportBill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls");
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

        gvJobDetail.Caption = "Pending Customer Bill_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm");
        DataFilter1.FilterSessionID = "PendingTransBill.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow )
        {
            if(DataBinder.Eval(e.Row.DataItem, "BillAmount") != DBNull.Value)
            {
                Decimal BillAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BillAmount"));

                if(BillAmount > 0)
                {
                    // Alow To Post To FA System

                    LinkButton ObjlnkPost = (LinkButton) e.Row.FindControl("lnkPost");

                    ObjlnkPost.Visible = true;
                }
            }
        }
    }
}