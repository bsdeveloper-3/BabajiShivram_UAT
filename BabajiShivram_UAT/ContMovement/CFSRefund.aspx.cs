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
using System.Collections.Generic;

public partial class ContMovement_CFSRefund : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginClass loggedInUser = new LoginClass();
        ScriptManager1.RegisterPostBackControl(lnkexport);
        
        Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "val", "validateAndHighlight();");

        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "CFS Refund Follow-Up";

        if (!IsPostBack)
        {
            if (gvMovementDetail.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For CFS Refund!";
                lblError.CssClass = "errorMsg";
            }
        }

        DataFilter1.DataSource = DataSourceRefund;
        DataFilter1.DataColumns = gvMovementDetail.Columns;
        DataFilter1.FilterSessionID = "CFSRefund.aspx";
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
            DataFilter1.FilterSessionID = "CFSRefund.aspx";
            DataFilter1.FilterDataSource();
            gvMovementDetail.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Pop-up Events

    protected void btnJobCheck_Click(object sender, EventArgs e)
    {
        int JobId = 0, count = 0;

        Session["JobList"] = null;

        List<int> JobList = new List<int>();

        if (gvMovementDetail.Rows.Count > 0)
        {
            for (int i = 0; i < gvMovementDetail.Rows.Count; i++)
            {
                CheckBox chkSelectJob = (CheckBox)gvMovementDetail.Rows[i].FindControl("chkSelectJob");
                JobId = Convert.ToInt32(gvMovementDetail.DataKeys[i].Value.ToString());

                if (chkSelectJob != null && JobId > 0)
                {
                    if (chkSelectJob.Checked)
                    {
                        count = count + 1;
                        JobList.Add(JobId);
                    }
                }
            }

            if (count > 0)
            {
                Session["JobList"] = JobList;
                mpeStatus.Show();

            }
            else
            {
                lblError.Text = "Please Select Job for Status Update!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        if (Session["JobList"] != null)
        {
            bool IsActive = true;

            List<int> JobList = new List<int>();
            JobList = (List<int>)Session["JobList"];

            string strFollowUpRemark = txtFollowUpRemark.Text.Trim();
            int StatusId = Convert.ToInt32(ddRefundStauts.SelectedValue);

            DateTime dtFollowUpDate = DateTime.MinValue;

            if (strFollowUpRemark == "")
            {
                lblError.Text = "Please Enter Follow Up Remark!";
                lblError.CssClass = "errorMsg";
                return;
            }

            if (txtFollowUpDate.Text.Trim() != "")
            {
                dtFollowUpDate = Commonfunctions.CDateTime(txtFollowUpDate.Text.Trim());
            }
            else
            {
                lblError.Text = "Please Enter Follow Up Date!";
                lblError.CssClass = "errorMsg";

                return;
            }

            if(StatusId == 2) // Refund In Process
            {
                IsActive = true;
            }
            else
            {
                IsActive = false; // Refund NA or Processed
            }

            // Update Follow Up Status

            int Result = 1;

            foreach (int JobId in JobList)
            {
                Result =  CMOperations.AddCFSRefundStatus(JobId, dtFollowUpDate, strFollowUpRemark, IsActive, StatusId, 1);

            }

            if(Result == 0)
            {
                lblError.Text = "Status Updated Successfully!";
                lblError.CssClass = "success";
            }
            else
            {
                lblError.Text = "System Error! Please Try After Sometime!";
                lblError.CssClass = "errorMsg";
            }
        }
    }
    
    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {
        Session["JobList"] = null;
        mpeStatus.Hide();
    }
    #endregion

    #region Export Events
    protected void lnkexport_Click(object sender, EventArgs e)
    {
        string strFileName = "CFS_Refund_FollowUp" + "_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
        ExportData("attachment;filename=" + strFileName, "application/vnd.ms-excel");
    }

    protected void ExportData(string header, string contentType)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", header);
        Response.Charset = "";
        this.EnableViewState = false;
        Response.ContentType = contentType;
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvMovementDetail.AllowPaging = false;
        gvMovementDetail.AllowSorting = false;
        gvMovementDetail.Columns[0].Visible = false;
        gvMovementDetail.Columns[1].Visible = false;
        
        DataFilter1.FilterSessionID = "CFSRefund.aspx";
        DataFilter1.FilterDataSource();
        gvMovementDetail.DataBind();
        gvMovementDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    protected void RemoveControls(Control grid)
    {
        Literal literal = new Literal();
        for (int i = 0; i < grid.Controls.Count; i++)
        {
            if (grid.Controls[i] is LinkButton)
            {
                literal.Text = (grid.Controls[i] as LinkButton).Text;
                grid.Controls.Remove(grid.Controls[i]);
                grid.Controls.AddAt(i, literal);
            }
            if (grid.Controls[i].HasControls())
            {
                RemoveControls(grid.Controls[i]);
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    #endregion
            
}