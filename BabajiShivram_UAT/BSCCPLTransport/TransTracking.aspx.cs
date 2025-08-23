using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccountTransport_TransTracking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Session["TransPayId"] = null;
            Session["TransPayIdTrack"] = null;

            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "BSCCPL Tracking";

            if (gvDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Record Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //
        DataFilter1.DataSource = InvoiceSqlDataSource;
        DataFilter1.DataColumns = gvDetail.Columns;
        DataFilter1.FilterSessionID = "TransTracking.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";

        if (e.CommandName.ToLower() == "select")
        {
            string strTransPayId = (string)e.CommandArgument;

            Session["TransPayIdTrack"] = strTransPayId;

            Response.Redirect("TransDetail.aspx"); ;
        }

    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "StatusId") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "StatusId").ToString());
                string strStatusName = DataBinder.Eval(e.Row.DataItem, "StatusName").ToString();

                if (StatusId == 100) // Invoice Uploaded 
                {
                    e.Row.BackColor = System.Drawing.Color.Blue;  //LightSalmon;    
                    e.Row.ToolTip = strStatusName;
                }
                else if (StatusId == 111 || StatusId == 121 || StatusId == 141) // Mgmt/aAcounts/Finance Hold - 
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = strStatusName;
                }
                else if (StatusId == 112 || StatusId == 122 || StatusId == 142 || StatusId == 148) // Mgmt/Accounts/Finance Reject - 
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                    e.Row.ToolTip = strStatusName;
                }
            }

        }
    }

    #region Data Filter
    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // DataFilter1.AndNewFilter();
            // DataFilter1.AddFirstFilter();
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
            DataFilter1.FilterSessionID = "TransTracking.aspx";
            DataFilter1.FilterDataSource();
            gvDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Export Data

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Transport_Invoice_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
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
        gvDetail.AllowPaging = false;
        gvDetail.AllowSorting = false;
        gvDetail.Columns[1].Visible = false;
        gvDetail.Columns[2].Visible = true;
        
        DataFilter1.FilterSessionID = "TransTracking.aspx";
        DataFilter1.FilterDataSource();
        gvDetail.DataBind();

        gvDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}