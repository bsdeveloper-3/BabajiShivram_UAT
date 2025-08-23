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
using System.Text;
using AjaxControlToolkit;
using System.Collections.Generic;

public partial class Master_LRCopyMaster : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSave);
        AutoCompleteJobNo.ContextKey = Convert.ToString(LoggedInUser.glUserId);

        if (!IsPostBack)
        {
            //GrdInvoiceDetail.DataSource = null;
            //GrdInvoiceDetail.DataBind();
            SetInitialRow();
            string strNextCNNo = DBOperations.GetNextCNNoForLRCopy();
            lblCNNO.Text = strNextCNNo;

            txtCNDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void GrdOtherInfo_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        }
    }

    private void SetInitialRow()
    {
        //GrdInvoiceDetail.DataSource = null;
        //GrdInvoiceDetail.DataBind();

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("Packages", typeof(string)));
        dt.Columns.Add(new DataColumn("Description", typeof(string)));
        dt.Columns.Add(new DataColumn("ActualWt", typeof(string)));
        dt.Columns.Add(new DataColumn("Charged", typeof(string)));

        dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dr["Packages"] = string.Empty;
        dr["Description"] = string.Empty;
        dr["ActualWt"] = string.Empty;
        dr["Charged"] = string.Empty;

        dt.Rows.Add(dr);
        ViewState["CurrentTable"] = null;
        ViewState["CurrentTable"] = dt;

        GrdOtherInfo.DataSource = dt;
        GrdOtherInfo.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
        // ModalPopupContainer.Show();
    }

    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox txtPackages = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[1].FindControl("txtPackages");
                    TextBox txtDescription = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                    TextBox txtActualWt = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[3].FindControl("txtActualWt");
                    TextBox txtCharged = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[4].FindControl("txtCharged");

                    if (txtPackages.Text.Trim() != "")
                    {
                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i - 1]["Packages"] = txtPackages.Text;
                        dtCurrentTable.Rows[i - 1]["Description"] = txtDescription.Text;
                        dtCurrentTable.Rows[i - 1]["ActualWt"] = txtActualWt.Text;
                        dtCurrentTable.Rows[i - 1]["Charged"] = txtCharged.Text;

                        rowIndex++;
                    }
                    else
                    {
                        lblOtherError.Text = "Please Enter the Package";
                        lblOtherError.CssClass = "errorMsg";

                        return;
                    }
                }

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                GrdOtherInfo.DataSource = dtCurrentTable;
                GrdOtherInfo.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
        //Set Previous Data on Postbacks
        SetPreviousData();
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox txtPackages = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[1].FindControl("txtPackages");
                    TextBox txtDescription = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[2].FindControl("txtDescription");
                    TextBox txtActualWt = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[3].FindControl("txtActualWt");
                    TextBox txtCharged = (TextBox)GrdOtherInfo.Rows[rowIndex].Cells[4].FindControl("txtCharged");

                    txtPackages.Text = dt.Rows[i]["Packages"].ToString();
                    txtDescription.Text = dt.Rows[i]["Description"].ToString();
                    txtActualWt.Text = dt.Rows[i]["ActualWt"].ToString();
                    txtCharged.Text = dt.Rows[i]["Charged"].ToString();

                    rowIndex++;
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime CNNoDate = DateTime.MinValue, InvoiceDate = DateTime.MinValue;

        string strCompany = ddlCompany.SelectedValue;
        int CompId = Convert.ToInt32(strCompany);

        string strRegnotoPay = ddlPersonLiable.SelectedValue;
        int RegnoToPay = Convert.ToInt32(strRegnotoPay);

        string strCNNo = lblCNNO.Text.Trim();
        string strInvoiceNo = txtInvoiceNo.Text.Trim();

        if (txtCNDate.Text.Trim() != "")
        {
            CNNoDate = Commonfunctions.CDateTime(txtCNDate.Text.Trim());
        }

        if (txtInvoiceDate.Text.Trim() != "")
        {
            InvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }

        string strFrom = txtFrom.Text.Trim();
        string strTo = txtTo.Text.Trim();
        string strconsignor = txtConsignorNmAddr.Text.Trim();
        string strDeliveryAddr = txtDeliveryAddr.Text.Trim();
        string strState = txtState.Text.Trim();
        string strTelNo = txtTelNo.Text.Trim();
        string strVehicleNo = txtVehicleNo.Text.Trim();
        string strJobRefNo = txtOurJobNo.Text.Trim();
        string strWayBillNo = txtWayBillNo.Text.Trim();
        string strVehicleType = txtVehicleType.Text.Trim();

        string strBENO = txtBENo.Text.Trim();
        string strBLNo = txtBLNo.Text.Trim();
        string strConsigneeAddr = txtConsigneeNmAddr.Text.Trim();

        int Lresult = DBOperations.AddLRDetails(strCNNo, CompId, RegnoToPay, strInvoiceNo, CNNoDate, InvoiceDate, strFrom, strTo,
            strconsignor, strDeliveryAddr, strState, strTelNo, strVehicleNo, strJobRefNo, strWayBillNo, strVehicleType,
             strBENO, strBLNo, strConsigneeAddr, LoggedInUser.glFinYearId, LoggedInUser.glUserId);

        int result = -123;

        if (Lresult > 0)
        {
            if (GrdOtherInfo.Rows.Count > 0)
            {
                for (int i = 0; i < GrdOtherInfo.Rows.Count; i++)
                {
                    TextBox txtPackages = (TextBox)GrdOtherInfo.Rows[i].Cells[1].FindControl("txtPackages");
                    TextBox txtDescription = (TextBox)GrdOtherInfo.Rows[i].Cells[2].FindControl("txtDescription");
                    TextBox txtActualWt = (TextBox)GrdOtherInfo.Rows[i].Cells[3].FindControl("txtActualWt");
                    TextBox txtCharged = (TextBox)GrdOtherInfo.Rows[i].Cells[4].FindControl("txtCharged");
                    if (txtPackages.Text.Trim() != "")
                    {
                        result = DBOperations.AddLRPackageDetails(Lresult, txtPackages.Text.Trim(), txtDescription.Text.Trim(),
                            txtActualWt.Text.Trim(), txtCharged.Text.Trim(), LoggedInUser.glUserId);

                        if (result == 1)
                        {
                            lblError.Text = "Successfully Added LR Detail..!!";
                            lblError.CssClass = "success";

                            //Clear();
                        }
                        else if (result == 0)
                        {
                            lblError.Text = "Something Went Wrong";
                            lblError.CssClass = "errorMsg";
                        }
                    }
                }
                if (result == 1)
                {
                    Clear();
                }
            }
        }
        else if (Lresult == 0)
        {
            lblError.Text = "Something Went Wrong";
            lblError.CssClass = "errorMsg";
        }
        else if (Lresult == -1)
        {
            lblError.Text = "Record Already Exist";
            lblError.CssClass = "errorMsg";
        }
    }

    public void Clear()
    {
        ddlCompany.SelectedValue = "0";
        ddlPersonLiable.SelectedValue = "0";

        txtCNDate.Text = "";
        txtInvoiceNo.Text = "";
        txtInvoiceDate.Text = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        txtConsigneeNmAddr.Text = "";
        txtDeliveryAddr.Text = "";
        txtState.Text = "";
        txtTelNo.Text = "";
        txtVehicleNo.Text = "";
        txtVehicleType.Text = "";
        txtBENo.Text = "";
        txtBLNo.Text = "";
        txtConsigneeNmAddr.Text = "";
        txtWayBillNo.Text = "";
        txtOurJobNo.Text = "";
        txtConsignorNmAddr.Text = "";

        GrdOtherInfo.DataSource = null;
        GrdOtherInfo.DataBind();

        SetInitialRow();
        string strNextCNNo = DBOperations.GetNextCNNoForLRCopy();
        lblCNNO.Text = strNextCNNo;

    }

    protected void txtOurJobNo_TextChanged(object sender, EventArgs e)
    {
        if (txtOurJobNo.Text.Trim() != "")
        {            
            if (hdnJobId.Value != "0")
            {
                DataSet dsGetJobDetail = DBOperations.GetLRConsigneeAddr(Convert.ToInt32(hdnJobId.Value));
                if (dsGetJobDetail != null)
                {
                    if (dsGetJobDetail.Tables[0].Rows[0]["ConsigneeAddr"] != DBNull.Value)
                    {
                        txtConsigneeNmAddr.Text = dsGetJobDetail.Tables[0].Rows[0]["ConsigneeAddr"].ToString();
                        txtDeliveryAddr.Text = dsGetJobDetail.Tables[0].Rows[0]["DeliveryAddress"].ToString();
                    }
                }
            }
            else
            {
                hdnJobId.Value = "0";
                txtConsigneeNmAddr.Text = "";
                lblError.Text = "Job Ref No is Not Valid";
                lblError.CssClass = "errorMsg";
            }
        }
        else
        {
            hdnJobId.Value = "0";
            txtConsigneeNmAddr.Text = "";
        }

        hdnJobId.Value = "0";
    }
}