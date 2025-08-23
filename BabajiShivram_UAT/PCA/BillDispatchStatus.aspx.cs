using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
public partial class PCA_BillDispatchStatus : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkExport);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Dispatch Bill List";

        }

        DataFilter2.DataSource = SqlDataSourceCustomer;
        DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
        DataFilter2.FilterSessionID = "BillDispatchStatus.aspx";
        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
    }
    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName.ToLower() == "view")
        {

            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillDocById(0, BillId, 10);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath;// + "//" + strFileName;

                ViewDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Document Not Uploaded!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
    protected void gvRecievedJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "lStatus") != DBNull.Value)
            {
                int StatusId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "lStatus"));

                if (StatusId == 30)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Rejected";
                }
                //if (StatusId >= 40)
                //{
                //    // Customer Accept Bill and Started Bill Processing

                //    CheckBox chk = (CheckBox)e.Row.FindControl("chkBillNo");

                //    if (chk != null)
                //    {
                //        chk.Enabled = false;
                //    }
                //}
            }
        }

    }

    #region Data Filter
    protected void gvRecievedJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
        }
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            DataFilter2_OnDataBound();
        }
    }
    void DataFilter2_OnDataBound()
    {
        try
        {
            DataFilter2.FilterSessionID = "BillDispatchStatus.aspx";
            DataFilter2.FilterDataSource();
            gvRecievedJobDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter2.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Document
    private void ViewDocument(string DocumentPath)
    {
        try
        {
            DocumentPath = EncryptDecryptQueryString.EncryptQueryStrings2(DocumentPath);

            // Response.Redirect("ViewDoc.aspx?ref=" + DocumentPath);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('../ViewDoc.aspx?ref=" + DocumentPath + "' ,'_blank');", true);

        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region exportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        string strFileName = "Bill_Dispatch_Status_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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

        gvRecievedJobDetail.AllowPaging = false;
        gvRecievedJobDetail.AllowSorting = false;

        gvRecievedJobDetail.Columns[1].Visible = false;
        gvRecievedJobDetail.Columns[2].Visible = true;
        gvRecievedJobDetail.Columns[15].Visible = false;


        DataFilter2.FilterSessionID = "BillDispatchStatus.aspx";
        DataFilter2.FilterDataSource();
        gvRecievedJobDetail.DataBind();
        
        gvRecievedJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}