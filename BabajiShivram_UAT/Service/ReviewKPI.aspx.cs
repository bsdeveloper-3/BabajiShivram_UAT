using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Service_ReviewKPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "KPI Review";
    }

    protected void GridViewKPI_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "viewdetail")
        {            
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
            
            Label lblEmpName = (Label)GridViewKPI.Rows[row.RowIndex].FindControl("lblEmpName");

            lblViewName.Text = lblEmpName.Text;

            int intKPIID = Convert.ToInt32(e.CommandArgument.ToString());

            hdnKPIID.Value = intKPIID.ToString();

            DataSet dsTargetDetail = DBOperations.KPI_GETEmpTarget(intKPIID);

            txtEmpRemark.Text = dsTargetDetail.Tables[0].Rows[0]["EmpRemark"].ToString();

            if (dsTargetDetail.Tables[0].Rows[0]["ApproveRemark"] != DBNull.Value)
            {
                txtHODRemark.Text = dsTargetDetail.Tables[0].Rows[0]["ApproveRemark"].ToString();
            }
            
            DataSet dsParticular = DBOperations.KPI_GETEmpParticular(0, intKPIID);
                        
            gvParticulars.DataSource = dsParticular;
            gvParticulars.DataBind();

            //DataSourceParticular.SelectParameters["KPIID"].DefaultValue = intKPIID.ToString();
            //DataSourceParticular.DataBind();
            

            ModalPopupMonthStatus.Show();
            
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupMonthStatus.Hide();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int KPIID = Convert.ToInt32(hdnKPIID.Value);

        string strApproveRemark = txtHODRemark.Text.Trim();

        // Approve KPI Particular
        int result = DBOperations.KPI_ApproveEmpTarget(KPIID, strApproveRemark, LoggedInUser.glUserId);

        if (result == 0)
        {
            // Update approved KPI - Particular

            foreach(GridViewRow row in gvParticulars.Rows)
            {
                int ParticularId = Convert.ToInt32(gvParticulars.DataKeys[row.RowIndex].Value);

                TextBox txtParticular = (TextBox) row.FindControl("txtParticular");
                string strParticular = txtParticular.Text.Trim();

                if (strParticular != "")
                {
                    // Update KPI - Particular

                    int resultUpd = DBOperations.KPI_UpdateEmpParticular(ParticularId, strParticular, LoggedInUser.glUserId);

                    txtParticular.Text = "";
                }
            }

            // Databind

            lblError.Text = "KPI details updated successfully!";
            lblError.CssClass = "success";

            GridViewKPI.DataBind();
            
            ModalPopupMonthStatus.Hide();
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime";
            lblError.CssClass = "errorMsg";
        }

    }
}