using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Service_EmpKPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "KPI - Performance Appraisal";
            
            GetEmpDetail();

            //DBOperations.FillBabajiHOD(ddHOD);

            Check_If_KPI_Target_AlreadyAdded();
        }
    }
    private void Check_If_KPI_Target_AlreadyAdded()
    {
        // Check if KPI Target set for Current Fin Year

        DataSet dsDetail = DBOperations.KPI_GETEmpTarget(LoggedInUser.glUserName,"");

        if(dsDetail.Tables[0].Rows.Count > 0)
        {
            bool IsReviewed = false;

            // Emp KPI Already Set for Current Fin Year
            tblKPI.Visible = false;
            gvKPI.DataBind();

            tblKPI.Visible = false;
            btnSubmit.Visible = false;
                        
            lblError.Text = "KPI Details Already Forwarded To HOD For Review!";
            lblError.CssClass = "success";

            // Set HOD Name

            ddHOD.SelectedValue = dsDetail.Tables[0].Rows[0]["HODID"].ToString();

            // Check If KPI Particulars reviewed by HOD

            if (dsDetail.Tables[0].Rows[0]["IsApprove"] != DBNull.Value)
            {
                IsReviewed = Convert.ToBoolean(dsDetail.Tables[0].Rows[0]["IsApprove"]);

                if (IsReviewed == true)
                {
                    // Disable KPI - Particulars GridView Edit

                    gvKPI.Columns[0].Visible = false;

                    lblError.Text = "KPI Details Reviewed By HOD!";
                }
            }
        }
    }
    private void GetEmpDetail()
    {
        DataView dvUser = DBOperations.GetUserDetail(LoggedInUser.glUserId.ToString());

        if (dvUser.Table.Rows.Count > 0)
        {
            lblEmpName.Text = dvUser.Table.Rows[0]["sName"].ToString();
            lblEmpCode.Text = dvUser.Table.Rows[0]["EmpCode"].ToString();
            
            //lblDiv.Text = dvUser.Table.Rows[0]["DivisionName"].ToString();
            //lblAccessRole.Text = dvUser.Table.Rows[0]["RoleName"].ToString();
            ////  lblBranch.Text = dvUser.Table.Rows[0]["CreatedDate"].ToString();
            ////  lblPort.Text = dvUser.Table.Rows[0]["CreatedDate"].ToString();
            ////  lblAddress.Text = dvUser.Table.Rows[0]["Address"].ToString();
            ////  lblEmail.Text = dvUser.Table.Rows[0]["sEmail"].ToString();
            ////  lblPhone.Text = dvUser.Table.Rows[0]["MobileNo"].ToString();
        }
    }
    protected void btnSubmit_Click(Object sender, EventArgs e)
    {
        int EmpID       = LoggedInUser.glUserId;
        string EmpName  = LoggedInUser.glEmpName;
        string EmpEmail = LoggedInUser.glUserName;

        int HODID = Convert.ToInt32(ddHOD.SelectedValue);

        string Remark = txtRemarks.Text.Trim();

        if (HODID <= 0)
        {
            lblError.Text = "Please Select HOD Name";
            lblError.CssClass = "errorMsg";

            return;
        }
      
        
        int KPIID = DBOperations.KPI_ADDEmpTarget(EmpID, EmpName, EmpEmail,lblEmpCode.Text.Trim(),HODID, Remark, LoggedInUser.glUserId);

        if (KPIID > 0)
        {
            lblError.Text = "KPI Details Forwarded To HOD For Review!";
            lblError.CssClass = "success";

            // ADD KPI Particulars

            string strParticular = "";
            int result = 0;

            if (txtKPI1.Text.Trim() != "")
            {
                strParticular = txtKPI1.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI2.Text.Trim() != "")
            {
                strParticular = txtKPI2.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI3.Text.Trim() != "")
            {
                strParticular = txtKPI3.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI4.Text.Trim() != "")
            {
                strParticular = txtKPI4.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI5.Text.Trim() != "")
            {
                strParticular = txtKPI5.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI6.Text.Trim() != "")
            {
                strParticular = txtKPI6.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI7.Text.Trim() != "")
            {
                strParticular = txtKPI7.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI8.Text.Trim() != "")
            {
                strParticular = txtKPI8.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI9.Text.Trim() != "")
            {
                strParticular = txtKPI9.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }
            if (txtKPI10.Text.Trim() != "")
            {
                strParticular = txtKPI10.Text.Trim();
                result = DBOperations.KPI_ADDEmpParticular(KPIID, strParticular, LoggedInUser.glUserId);
            }

            tblKPI.Visible = false;
            gvKPI.DataBind();

            ClearField();
        }
        else if (KPIID == 0)
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
        else if (KPIID == -1)
        {
            lblError.Text = "KPI Details Already SET! Please Wait for HOD Review!";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please try after sometime.";
            lblError.CssClass = "errorMsg";
        }
    }
    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        ClearField();
    }
    private void ClearField()
    {
        ddHOD.SelectedIndex = 0;
        txtRemarks.Text = "";
        
        txtKPI1.Text = "";
        txtKPI2.Text = "";
        txtKPI3.Text = "";
        txtKPI4.Text = "";
        txtKPI5.Text = "";
        txtKPI6.Text = "";
        txtKPI7.Text = "";
        txtKPI8.Text = "";
        txtKPI9.Text = "";
        txtKPI10.Text = "";
    }

    #region GridView Event

    protected void gvKPI_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvKPI.EditIndex = e.NewEditIndex;

        gvKPI.DataBind();
    }

    protected void gvKPI_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ParticularID = Convert.ToInt32(gvKPI.DataKeys[e.RowIndex].Value.ToString());

        TextBox txtEdtParticulars = (TextBox)gvKPI.Rows[e.RowIndex].FindControl("txtEdtParticulars");

        if (txtEdtParticulars.Text.Trim() == "")
        {

            lblError.Text = "Please Enter KPI Particular!";
            lblError.CssClass = "errorMsg";
            return;
        }
        else
        {

            int result =  DBOperations.KPI_UpdateEmpParticular(ParticularID, txtEdtParticulars.Text.Trim(),LoggedInUser.glUserId);

            if (result == 0)
            {
                lblError.Text = "Detail Updated Successfully.";
                lblError.CssClass = "success";

                gvKPI.EditIndex = -1;
                e.Cancel = true;

            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try After Sometime.";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            else if (result == 2)
            {
                lblError.Text = "KPI Already Review By HOD!";
                lblError.CssClass = "errorMsg";
                e.Cancel = true;
            }
            
        }//END_ELSE
        
    }

    protected void gvKPI_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvKPI.EditIndex = -1;
    }

    #endregion
}