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
using System.Net;
using System.Text;
using ClosedXML.Excel;

public partial class AccountExpense_PaymentCancel : System.Web.UI.Page
{
    LoginClass loggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Payment Cancel";
        if (!IsPostBack)
        {
            if (gvJobExpDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found for Payment Cancel..!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceJobExpenseDetails;
        DataFilter1.DataColumns = gvJobExpDetail.Columns;
        DataFilter1.FilterSessionID = "PaymentCancel.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

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
            DataFilter1.FilterSessionID = "PaymentCancel.aspx";
            DataFilter1.FilterDataSource();
            gvJobExpDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "Cancelled Payment_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    private void ExportFunction(string header, string contentType)
    {
        ViewState["TotalAmount"] = "0";
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvJobExpDetail.AllowPaging = false;
        gvJobExpDetail.AllowSorting = false;
        gvJobExpDetail.Columns[1].Visible = false;
        gvJobExpDetail.Caption = "Payment Cancelled Summary " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PaymentCancel.aspx";
        DataFilter1.FilterDataSource();
        gvJobExpDetail.DataBind();
        gvJobExpDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion
}

