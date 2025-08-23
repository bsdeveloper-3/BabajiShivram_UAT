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

public partial class Reports_BillingFileStatusReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!IsPostBack)
        {
            Drpbranch.DataSource = DsAllBranch;
            Drpbranch.DataValueField = "lid";
            Drpbranch.DataTextField = "BranchName";
            Drpbranch.DataBind();

            DrpCustomer.DataSource = DsCustomer;
            DrpCustomer.DataValueField = "lid";
            DrpCustomer.DataTextField = "CustName";
            DrpCustomer.DataBind();
        }
    }

    protected void btnShowReport_OnClick(Object sender, EventArgs e)
    {
        divincorrectFiles.Style.Add("display", "block");
        gvFileStatusReport.Visible = true;

        string strFromDate = "";
        string strToDate = "";
        if (txtDateFrom.Text.Trim() != "")
        {
            strFromDate = Commonfunctions.CDateTime(txtDateFrom.Text.Trim()).ToShortDateString();
        }
        if (txtDateTo.Text.Trim() != "")
        {
            strToDate = Commonfunctions.CDateTime(txtDateTo.Text.Trim()).ToShortDateString();
        }

        DsbillReports.SelectParameters["FromDate"].DefaultValue = strFromDate;
        DsbillReports.SelectParameters["ToDate"].DefaultValue = strToDate;
        

        gvFileStatusReport.DataSource = DsbillReports;
        gvFileStatusReport.DataBind();

        if (gvFileStatusReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }

    protected void Cancel_OnClick(Object sender, EventArgs e)
    {
        divincorrectFiles.Style.Add("display", "none");
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        Drpbranch.SelectedValue = "0";
        DrpCustomer.SelectedValue = "0";
        gvFileStatusReport.Visible = false;

    }

    #region ExportData

    protected void lnkexport_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string strFileName = "BillReport_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportFunction("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void lnkInCorrectFiles_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

        string Stage = clickedRow.Cells[1].Text;

        if (Stage == "Draft Check")
        {
            Stage = "3";
        }
        else
        {
            Stage = "5";
        }

        DsInCorrectFileDetails.SelectParameters["ReceivedBy"].DefaultValue = Stage.ToString();

        ModalPopupExtender1.Show();
        gvIncorrectfiledetails.DataBind();
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    private void ExportFunction(string header, string contentType)
    {
        gvFileStatusReport.DataSource = DsbillReports;
        gvFileStatusReport.DataBind();

        if (gvFileStatusReport.Rows.Count < 1)
        {
            lblMessage.Text = "No Record Found!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", header);
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = contentType;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gvFileStatusReport.AllowPaging = false;
            gvFileStatusReport.AllowSorting = false;

            gvFileStatusReport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
    #endregion
}