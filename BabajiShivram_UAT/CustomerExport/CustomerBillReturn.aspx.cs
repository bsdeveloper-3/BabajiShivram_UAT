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

public partial class CustomerBillReturn : System.Web.UI.Page
{
    LoginClass loggedInClass = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gridBillReturn);
        AutoCompleteExtender10.ContextKey = Convert.ToString(loggedInClass.glFinYearId) +"," + Convert.ToString(loggedInClass.glCustUserId) ;

        if (!IsPostBack)
        {
            string FinYear = loggedInClass.glFinYearId.ToString();
            //lblFinYear.Text= loggedInClass.glFinYearId.ToString();
            //hdnFinyear.Value=loggedInClass.glFinYearId.ToString();
        }
    }

    protected void txtSearchBillNo_TextChanged(object sender, EventArgs e)
    {
        int Finyear = 0;

        string strFinyear = Convert.ToString(loggedInClass.glFinYearId);
        if (strFinyear != "")
        {
            Finyear = Convert.ToInt32(strFinyear);
        }

        string strBillNo = txtSearchBillNo.Text.Trim();

      
        DataSet dsJobBillReturn = DBOperations.GetBillReturnDetails(strBillNo, Finyear);

        if (dsJobBillReturn.Tables.Count > 0)
        {
            if (dsJobBillReturn.Tables[0].Rows.Count > 0)
            {
                gridBillReturn.DataSource = dsJobBillReturn;
                gridBillReturn.DataBind();
            }
        }
    }

    //protected void btnOK_Click(object sender, EventArgs e)
    //{
    //    string strlid="", strJobId = "", strBJVNo = "", strBillNo = "", strRemark = "";
    //    int Reason = 0, Result = -123;

    //    Label lblid = (Label)gridBillReturn.FindControl("lblJobId");
    //    Label lblJobId = (Label)gridBillReturn.FindControl("lblJobId");
    //    Label lblBJVNo = (Label)gridBillReturn.FindControl("lblBJVNo");
    //    Label lblBillNo = (Label)gridBillReturn.FindControl("lblBillNo");

    //    CheckBox chkIsReturn = (CheckBox)gridBillReturn.FindControl("chkIsReturn");
    //    DropDownList ddlReason = (DropDownList)gridBillReturn.FindControl("ddlReason");
    //    TextBox txtRemark = (TextBox)gridBillReturn.FindControl("txtRemark");

    //    strlid = lblid.Text.Trim();
    //    strJobId = lblJobId.Text.Trim();
    //    strBJVNo = lblBJVNo.Text.Trim();
    //    strBillNo = lblBillNo.Text.Trim();
    //    Reason = Convert.ToInt32(ddlReason.SelectedValue);
    //    strRemark = txtRemark.Text.Trim();
    //    int lUser = loggedInClass.glUserId;
    //    int lUser1 = loggedInClass.glCustUserId;

    //    if (chkIsReturn.Checked == true)
    //    {
    //        Result = DBOperations.AddBillReturnbyCust(Convert.ToInt32(strlid), Convert.ToInt32(strJobId), strBJVNo, Reason, strRemark, loggedInClass.glCustUserId);

    //        if (Result == 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Bill Return Update Successfully');</script>", false);
    //        }
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please Check Is Return or not');</script>", false);
    //    }
    //}

    protected void gridBillReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = -123;

        if(e.CommandName.ToLower()== "updbillreturn")
        {
            string strlid = "", strJobId = "", strBJVNo = "", strBillNo = "", strRemark = "";
            int Reason = 0;
            decimal INVAmount = 0;

            GridViewRow gvrow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            
            Label lblid = (Label)gridBillReturn.Rows[gvrow.RowIndex].FindControl("lbllid");
            Label lblJobId = (Label)gridBillReturn.Rows[gvrow.RowIndex].FindControl("lblJobId");
            Label lblBJVNo = (Label)gridBillReturn.Rows[gvrow.RowIndex].FindControl("lblBJVNo");
            Label lblBillNo = (Label)gridBillReturn.Rows[gvrow.RowIndex].FindControl("lblBillNo");
            Label lblINVAMOUNT = (Label)gridBillReturn.Rows[gvrow.RowIndex].FindControl("lblINVAMOUNT");

            CheckBox chkIsReturn = (CheckBox)gridBillReturn.Rows[gvrow.RowIndex].FindControl("chkIsReturn");
            DropDownList ddlReason = (DropDownList)gridBillReturn.Rows[gvrow.RowIndex].FindControl("ddlReason");
            TextBox txtRemark = (TextBox)gridBillReturn.Rows[gvrow.RowIndex].FindControl("txtRemark");

            strlid = lblid.Text.Trim();
            strJobId = lblJobId.Text.Trim();
            strBJVNo = lblBJVNo.Text.Trim();
            strBillNo = lblBillNo.Text.Trim();
            Reason = Convert.ToInt32(ddlReason.SelectedValue);
            strRemark = txtRemark.Text.Trim();            
            int lUser = loggedInClass.glCustUserId;

            if(lblINVAMOUNT.Text.Trim()!="")
            {
                INVAmount = Convert.ToDecimal(lblINVAMOUNT.Text.Trim());
            }

            if (chkIsReturn.Checked == true) 
            {
                if (Reason > 0 && strRemark != "")
                {
                    Result = DBOperations.AddBillReturnbyCust(Convert.ToInt32(strlid), Convert.ToInt32(strJobId), strBJVNo, INVAmount, Reason, strRemark, loggedInClass.glCustUserId);

                    if (Result > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Bill Return Update Successfully');</script>", false);

                        Result = DBOperations.SendMailBillReturn(Convert.ToInt32(strlid), Convert.ToInt32(strJobId), Result,Convert.ToInt32(loggedInClass.glFinYearId));

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('"+ Result + "');</script>", false);

                        txtSearchBillNo.Text = string.Empty;
                        gridBillReturn.DataSource = null;
                        gridBillReturn.DataBind();
                    }
                    else if(Result < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Bill Return Update Successfully');</script>", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Select Reason/ Remark');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('Please Check Is Return or not');</script>", false);
            }
        }
    }
}