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
using System.Text;
using System.IO;

public partial class Service_OpenKPI : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int EmpID = Convert.ToInt32(hdnUserId.Value);
        string EmpName = txtEmpName.Text.Trim();
        string EmpEmail = txtEmail.Text.Trim();
        string EmpCode = txtEmpCode.Text.Trim();

        int HODID = Convert.ToInt32(ddHOD.SelectedValue);

        string Remark = txtRemark.Text.Trim();

        if (HODID <= 0)
        {
            lblError.Text = "Please Select HOD Name";
            lblError.CssClass = "errorMsg";

            return;
        }


        int KPIID = DBOperations.KPI_ADDEmpTarget(EmpID, EmpName, EmpEmail,EmpCode, HODID, Remark, LoggedInUser.glUserId);

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

            RestField();
            
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

    protected void txtEmpName_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        txtEmail.Text = "";
        txtEmail.Enabled = true;
        btnSubmit.Visible = true;

        if (hdnUserId.Value != "")
        {
            int UserID = Convert.ToInt32(hdnUserId.Value);

            // Get Emp Details

            DataView dvUser = DBOperations.GetUserDetail(UserID.ToString());

            if (dvUser.Table.Rows.Count > 0)
            {
                txtEmpName.Text = dvUser.Table.Rows[0]["sName"].ToString();
                txtEmail.Text   = dvUser.Table.Rows[0]["sEmail"].ToString();
                txtEmpCode.Text = dvUser.Table.Rows[0]["EmpCode"].ToString();
                txtEmail.Enabled = false;

                Check_If_KPI_Target_AlreadyAdded(UserID, dvUser.Table.Rows[0]["sEmail"].ToString());
            }
            else
            {
                txtEmail.Focus();
            }
        }
    }

    protected void txtEmail_TextChanged(object sender, EventArgs e)
    {

        // Get Emp Details

        DataView dvUser = DBOperations.GetUserDetailByEmail(txtEmail.Text.Trim());

            if (dvUser.Table.Rows.Count > 0)
            {
                int UserID = Convert.ToInt32(dvUser.Table.Rows[0]["sName"].ToString());
                txtEmpName.Text = dvUser.Table.Rows[0]["sName"].ToString();
                txtEmail.Text = dvUser.Table.Rows[0]["sEmail"].ToString();
            
              //  Check_If_KPI_Target_AlreadyAdded(UserID, dvUser.Table.Rows[0]["sEmail"].ToString());
            }
        
    }
    
    private void Check_If_KPI_Target_AlreadyAdded(int UserID, string sEmail)
    {

        // Check if KPI Target set for Current Fin Year

        int result = DBOperations.KPI_CheckEmpTarget(UserID, sEmail, 1);

        if (result == 1)
        {
            // Emp Already Filled up KPI Target details for Current Fin Year

            btnSubmit.Visible = false;

            lblError.Text = "KPI Details Already Forwarded To HOD For Review!";
            lblError.CssClass = "success";
        }
        
    }

    private void RestField()
    {
        hdnUserId.Value = "0";
        txtEmpName.Text = "";
        txtEmail.Text = "";
        ddHOD.SelectedValue = "0";

        txtKPI1.Text= "";
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
}