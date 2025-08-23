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
using BSImport.CustomControls;
using System.Text;
using System.Drawing;

public partial class SEZ_SEZInfo : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        if (!Page.IsPostBack)
        {
            //SetInitialRow();
            //if (gvJobDetail.Rows.Count == 0)
            //{
            //    lblMessage.Text = "No Job Found For Checklist Preparation!";
            //    lblMessage.CssClass = "errorMsg";
            //    pnlFilter.Visible = false;
            //}
        }
        DataFilter1.DataSource = PendingSEZDataSource;
        DataFilter1.DataColumns = gvJobDetail.Columns;
        DataFilter1.FilterSessionID = "SEZInfo.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
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
            DataFilter1.FilterSessionID = "SEZInfo.aspx";
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
        string strFileName = "SEZJobDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvJobDetail.Columns[1].Visible = false;
        gvJobDetail.Columns[2].Visible = true;

        gvJobDetail.Caption = "SEZ Job Information on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "SEZInfo.aspx";
        DataFilter1.FilterDataSource();
        gvJobDetail.DataBind();

        //-- gvJobDetail.DataSourceID = "PendingIGMSqlDataSource";
        //-- gvJobDetail.DataBind();
        // BindGridData();
        //gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //// gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //{
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //}


        gvJobDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

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

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "select")
        {
            string strJobId = (string)e.CommandArgument;
            //Label lblSEZType = (Label)gvJobDetail.FindControl("lblSEZType");
            //string kk = lblSEZType.Text;      
            //string kk 1= lblSEZType.Text;
            Session["JobId"] = strJobId;

            Response.Redirect("SEZEditJobDetail.aspx");
        }

        if (e.CommandName.ToLower() == "querypopup" && e.CommandArgument != null)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ';' });
            string strCustDocFolder = "", strJobRefNo = "";

            string Jobid = commandArgs[0].ToString();
            strJobRefNo = commandArgs[1].ToString();
            lblJobRefNo.Text = strJobRefNo;
            Session["JobId"] = Jobid;


            //LinkButton lnkJobRefNo = (LinkButton)gvJobDetail.FindControl("lnkJobRefNo");
            //string kk = lnkJobRefNo.Text.ToString();

            // 
            // string kk = lnkJobRefNo.Text;

            //if (commandArgs[1].ToString() != "")
            //    lblJobRefNo.Text = commandArgs[1].ToString();
            // strcustdocfolder = commandargs[1].tostring() + "//";
            //if (commandArgs[2].ToString() != "")
            //    strJobFileDir = commandArgs[2].ToString() + "//";

            // hdnUploadPath.Value = strCustDocFolder + strJobFileDir;

            // txtQuery.Text = "";
            // txtQueryDate.Text = "";
            // lblMessage.Text = "";

            DataSet dsSEZStatus = SEZOperation.GetSEZJobDetail(Convert.ToInt32(Jobid));

            if (dsSEZStatus.Tables[0].Rows.Count > 0)
            {
                // int aa = dsQueryDetail.Tables[0].Columns["JobStatus"].ToString();
                string JobStatus = dsSEZStatus.Tables[0].Rows[0]["JobStatus"].ToString();
                string JobStatusRemark = dsSEZStatus.Tables[0].Rows[0]["JobStatusRemark"].ToString();
                if (JobStatus == "")
                {
                    ddlStatus.SelectedValue = "0";
                }
                else
                {
                    ddlStatus.SelectedValue = JobStatus;
                }
                txtSEZStatus.Text = JobStatusRemark;
            }


            // gvPassingQuery.DataSource = dsQueryDetail;
            //gvPassingQuery.DataBind();

            ModalPopupStatus.Show();
        }
    }

    protected void BtnSaveStatus_Click(object sender, EventArgs e)
    {
        int JobId = Convert.ToInt32(Session["JobId"]);//Convert.ToInt32(hdnJobId.Value);
        int statusddl = Convert.ToInt32(ddlStatus.SelectedValue);
        string statusRemark = txtSEZStatus.Text.Trim();

        int Result = SEZOperation.AddSEZStatus(JobId, statusddl, statusRemark, LoggedInUser.glUserId);

        if (Result == 0)
        {
            lblPopMessageQuery.Visible = true;
            lblPopMessageQuery.Text = "Status Added Successfully.<BR>";
            lblPopMessageQuery.CssClass = "success";
        }
        else
        {
            lblPopMessageQuery.Visible = true;
            lblPopMessageQuery.Text = "System Error! Please check the required field.";
            lblPopMessageQuery.CssClass = "errorMsg";
        }
        ModalPopupStatus.Show();
    }

    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

}