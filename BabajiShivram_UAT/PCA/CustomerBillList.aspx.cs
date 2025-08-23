using System;
using System.Collections.Generic;
using QueryStringEncryption;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class PCA_CustomerBillList : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    List<Control> controls = new List<Control>();
    private static Random _random = new Random();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Open Bill List";

        }

        DataFilter2.DataSource = SqlDataSourceCustomer;
        DataFilter2.DataColumns = gvRecievedJobDetail.Columns;
        DataFilter2.FilterSessionID = "CustomerBillList.aspx";
        DataFilter2.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter2_OnDataBound);
    }
    protected void gvRecievedJobDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        if (e.CommandName.ToLower() == "reject")
        {
            int BillId = Convert.ToInt32(e.CommandArgument);

            DataSet dsGetBillDetail = BillingOperation.GetBJVBillDetailByID(BillId);

            if (dsGetBillDetail.Tables[0].Rows.Count > 0)
            {
                hdnBillId.Value = BillId.ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["BJVNo"] != null)
                {
                    lblBJVNumber.Text = dsGetBillDetail.Tables[0].Rows[0]["BJVNo"].ToString();
                }
                if (dsGetBillDetail.Tables[0].Rows[0]["INVNO"] != null)
                    lblBJVBillNo.Text = dsGetBillDetail.Tables[0].Rows[0]["INVNO"].ToString();

                if (dsGetBillDetail.Tables[0].Rows[0]["INVDATE"] != null)
                    lblBJVBillDate.Text = Convert.ToDateTime(dsGetBillDetail.Tables[0].Rows[0]["INVDATE"]).ToString("dd/MM/yyyy");
                if (dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"] != null)
                    lblBJVAmount.Text = dsGetBillDetail.Tables[0].Rows[0]["INVAMOUNT"].ToString();

                BillRejectModalPopup.Show();
            }
            else
            {
                lblMessage.Text = "Bill Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
            }
        }
        else if (e.CommandName.ToLower() == "view")
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
            DataFilter2.FilterSessionID = "CustomerBillList.aspx";
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

    protected void btnAccept_Click(object sender, EventArgs e)
    {
       
        int rowCount = 0;

        foreach (GridViewRow rw in gvRecievedJobDetail.Rows)
        {
            int BillId = Convert.ToInt32(gvRecievedJobDetail.DataKeys[rw.RowIndex].Value);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if(chk.Checked)
            {
                rowCount = rowCount + 1;

                int result =  BillingOperation.AddBillStatus(0, BillId, 40, "", LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblMessage.Text = "Status Updated Successfully!";
                    lblMessage.CssClass = "success";
                }
                else if (result == 1)
                {
                    lblMessage.Text = "System Error! Please Try after sometime";
                    lblMessage.CssClass = "errorMsg";
                }
                if (result == 2)
                {
                    lblMessage.Text = BillId.ToString() + "Bill Detail Not Found!";
                    lblMessage.CssClass = "errorMsg";
                }
            }

        }

        if(rowCount == 0)
        {
            lblMessage.Text = "Please Select Bill!";
            lblMessage.CssClass = "errorMsg";
        }
        else
        {
            gvRecievedJobDetail.DataBind();
        }
    }
    protected void btnProcessed_Click(object sender, EventArgs e)
    {
        int BillID = 0;
        int rowCount = 0;

        foreach (GridViewRow rw in gvRecievedJobDetail.Rows)
        {
            int BillId = Convert.ToInt32(gvRecievedJobDetail.DataKeys[rw.RowIndex].Value);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                rowCount = rowCount + 1;

                int result = BillingOperation.AddBillStatus(0, BillID, 41, "", LoggedInUser.glUserId);
                if (result == 0)
                {
                    lblMessage.Text = "Status Updated Successfully!";
                    lblMessage.CssClass = "success";
                }
                else if (result == 1)
                {
                    lblMessage.Text = "System Error! Please Try after sometime";
                    lblMessage.CssClass = "errorMsg";
                }
                if (result == 2)
                {
                    lblMessage.Text = "Bill Detail Not Found!";
                    lblMessage.CssClass = "errorMsg";
                }
            }

        }

        if (rowCount == 0)
        {
            lblMessage.Text = "Please Select Bill!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    protected void btnRejectBill_Click(object sender, EventArgs e)
    {
        int BillId = Convert.ToInt32(hdnBillId.Value);

        string strRemark = txtRejectionRemark.Text.Trim();
        
        int result = BillingOperation.AddBillStatus(0, BillId, 30, strRemark, LoggedInUser.glUserId);
        
        if (result == 0)
        {
            lblMessage.Text = "Status Updated Successfully!";
            lblMessage.CssClass = "success";
        }
        else if (result == 1)
        {
            lblMessage.Text = "System Error! Please Try after sometime";
            lblMessage.CssClass = "errorMsg";
        }
        if (result == 2)
        {
            lblMessage.Text = "Bill Detail Not Found!";
            lblMessage.CssClass = "errorMsg";
        }
    }
}