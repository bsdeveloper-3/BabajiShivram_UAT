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


public partial class ExportCHA_PendingForm13 : System.Web.UI.Page
{
    LoginClass loggedinuser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Form 13";
        if (!IsPostBack)
        {
            if (gvJobDetail.Rows.Count == 0)
            {
                lblMessage.Text = "No Job Found For Form 13!";
                lblMessage.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = PendingNotingSqlDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "PendingForm13.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    #region GridView Event
    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName.ToLower() == "edit")
        {
            GridViewRow gvrow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            string strJobId = gvJobDetail.DataKeys[gvrow.RowIndex].Value.ToString();
        }
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;
            Session["JobId"] = strJobId;
            Response.Redirect("Form13JobDetail.aspx");
        }
        if (e.CommandName.ToLower() == "cancel")
            gvJobDetail.EditIndex = -1;
    }

    protected void gvJobDetail_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvJobDetail.EditIndex = e.NewEditIndex;
    }

    protected void gvJobDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int JobId = 0;
        JobId = Convert.ToInt32(gvJobDetail.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtForm13Date = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtForm13Date");
        TextBox txtTransHandOverDate = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtTransHandOverDate");
        TextBox txtContainerGetInDate = (TextBox)gvJobDetail.Rows[e.RowIndex].FindControl("txtContainerGetInDate");
        DateTime dtForm13Date = DateTime.MinValue, dtTransHandOverDate = DateTime.MinValue, dtContainerGetIndate = DateTime.MinValue;
        if (txtForm13Date.Text.Trim().ToString() != "")
            dtForm13Date = Commonfunctions.CDateTime(txtForm13Date.Text.Trim());
        if (txtTransHandOverDate.Text.Trim().ToString() != "")
            dtTransHandOverDate = Commonfunctions.CDateTime(txtTransHandOverDate.Text.Trim());
        if (txtContainerGetInDate.Text.Trim().ToString() != "")
            dtContainerGetIndate = Commonfunctions.CDateTime(txtContainerGetInDate.Text.Trim());

        if (JobId != 0)
        {
            int result = EXOperations.EX_AddForm13JobDetail(JobId, dtForm13Date, dtTransHandOverDate, dtContainerGetIndate, loggedinuser.glUserId);
            if (result == 3) // bill type 3 for on wheel job
            {
                lblMessage.Text = "Form 13 Detail Added Successfully. Job Moved To Shipment Get In.";
                lblMessage.CssClass = "success";
                gvJobDetail.EditIndex = -1;
                gvJobDetail.DataBind();
                e.Cancel = true;
            }
            else if (result == -1)
            {
                lblMessage.Text = "System Error! Please Try After Sometime.";
                lblMessage.CssClass = "errorMsg";
                gvJobDetail.EditIndex = -1;
                gvJobDetail.DataBind();
                e.Cancel = true;
            }
            else
            {
                lblMessage.Text = "Form 13 Detail Added Successfully. Job Moved To Shipment Get In.";
                lblMessage.CssClass = "success";
                gvJobDetail.EditIndex = -1;
                gvJobDetail.DataBind();
                e.Cancel = true;
            }
        }//END_IF
    }

    protected void gvJobDetail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvJobDetail.EditIndex = -1;
    }

    protected void gvJobDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;

        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "Priority") != DBNull.Value)
            {
                // Change row color based on job priority

                int prioroty = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Priority"));
                if (prioroty == (int)JobPriority.High)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "High Job Priority";
                }
                else if (prioroty == (int)JobPriority.Intense)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#85f7f7");
                    e.Row.ToolTip = "Intense Job Priority";
                }
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
            DataFilter1.FilterSessionID = "PendingForm13.aspx";
            DataFilter1.FilterDataSource();
            gvJobDetail.DataBind();
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
        string strFileName = "PendingForm13_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.AllowPaging = false;
        gvJobDetail.AllowSorting = false;
        gvJobDetail.Columns[0].Visible = false;
        gvJobDetail.Caption = "Pending Form13 On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "PendingForm13.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();
        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion
}