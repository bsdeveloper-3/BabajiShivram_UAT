using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using RestSharp;
using System.Text;

public partial class Transport_TransMovement : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();   //19-09-2020
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(gvVehicleDelivery);
        ScriptManager1.RegisterPostBackControl(lnkExport);
        ScriptManager1.RegisterPostBackControl(GVVehicle);//10102020

        //ScriptManager1.RegisterPostBackControl(pnlVehicleDeliveryAdd2);
        //ScriptManager1.RegisterPostBackControl(fuDetention);
        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Transport Movement";

            if (GridViewVehicle.Rows.Count == 0)
            {
                lblError.Text = "No Job Found For Transport Movement!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        DataFilter1.DataSource = DataSourceVehicle;
        DataFilter1.DataColumns = GridViewVehicle.Columns;
        DataFilter1.FilterSessionID = "TransMovement.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        GVVehicle.DataBind();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        int result = 0;

        foreach (GridViewRow row in GridViewVehicle.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string RequestId = GridViewVehicle.DataKeys[row.RowIndex].Value.ToString();

                    CheckBox chkSelect = (row.FindControl("chkSelect") as CheckBox);
                    TextBox txtApprovedRate = (row.FindControl("txtApprovedRate") as TextBox);

                    string strRate = txtApprovedRate.Text.Trim();

                    if (chkSelect.Checked)
                    {
                        try
                        {
                            if (strRate != "") // Check If Rate is not Zero
                            {
                                result = DBOperations.AddApprovareTransRate(Convert.ToInt32(RequestId), Convert.ToInt32(strRate), LoggedInUser.glUserId);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = ex.Message;
                            lblError.CssClass = "errorMsg";
                            result = -1;
                            return;
                        }
                    }//END_IF
                }//END_IF
            }//END_IF
        }//END_ForEarch

        // IF No Error in New Exbond Quantity Update Invoice Details
        if (result == 0)
        {
            lblError.Text = "Rate Approved Successfully!";
            lblError.CssClass = "success";

            GridViewVehicle.DataBind();
        }
        else
        {
            GridViewVehicle.DataBind();
        }
    }

    #region GridView Event

    protected void GridViewVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "JobDeliveryId") == DBNull.Value)
            {
                e.Row.Cells[2].Text = "";
            }
        }

        if ((e.Row.RowState == DataControlRowState.Edit) || (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)))
        {
            //LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
            //if (lnkUpdate != null)
            //{
            //    ScriptManager1.RegisterPostBackControl(lnkUpdate);
            //}

            if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DeliveryTypeId")) == 1) // delivery type is loaded & job type is import
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "JobType")) == 1 || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "JobType")) == 3)
                {
                    //AjaxControlToolkit.MaskedEditValidator MskValContReturnDate = (AjaxControlToolkit.MaskedEditValidator)e.Row.FindControl("MskValContReturnDate");
                    //if (MskValContReturnDate != null)
                    //{
                    //    MskValContReturnDate.IsValidEmpty = false;
                    //    MskValContReturnDate.ValidationGroup = "VehicleRequired";
                    //}
                    //else
                    //{
                    //    MskValContReturnDate.IsValidEmpty = true;
                    //}
                }
            }
        }
    }

    protected void GridViewVehicle_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName.ToLower() == "dailystatus")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), " onLoad", "initialize1();", true);
            string strCustomerEmail = "";
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            hdnVehicleNo.Value = commandArgs[0].ToString();
            hdnVehicleType.Value = commandArgs[1].ToString();
            hdnDeliveryFrom.Value = commandArgs[2].ToString();
            hdnDeliveryTo.Value = commandArgs[3].ToString();
            hdnDispatchDate.Value = commandArgs[4].ToString();
            strCustomerEmail = commandArgs[5].ToString();
            hdnTransReqId.Value = commandArgs[6].ToString();
            hdnJobRefNo.Value = commandArgs[7].ToString();
            hdnCustomer.Value = commandArgs[8].ToString();
            hdnCustRefNo.Value = commandArgs[9].ToString();

            strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace(" ", "");
            strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
            lblCustomerEmail.Text = strCustomerEmail;

            DataView dvGetUserDetail = DBOperations.GetUserDetail(LoggedInUser.glUserId.ToString());
            if (dvGetUserDetail.Table.Rows.Count > 0)
            {
                txtMailCC.Text = dvGetUserDetail.Table.Rows[0]["sEmail"].ToString();
            }
            mpeDailyStatus.Show();
        }
        else if (e.CommandName.ToLower().Trim() == "updatevehicle")
        {
            int JobId = 0;
            if (e.CommandArgument.ToString() != "")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(';');

                Session["TransReqId"] = commandArgs[0].ToString();
                hdnTransReqId_Vehicle.Value = commandArgs[0].ToString();
                hdnPopupConsolidateID.Value = commandArgs[1].ToString();
                gvVehicleDelivery.DataBind();
                GVVehicle.DataBind();
                mpeVehicleDelivery.Show();
            }
        }
        else if (e.CommandName.ToLower().Trim() == "addvehicle")//23-03-2020
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            //lblError.Text = commandArgs[3].ToString()+"@@"+ commandArgs[2].ToString();
            Session["TransReqId"] = commandArgs[2].ToString();
            lblJobNo.Text = commandArgs[0].ToString();
            lblRefNo.Text = commandArgs[1].ToString();
            lblVehicleNumber.Text = commandArgs[3].ToString();
            lblType.Text = commandArgs[4].ToString();
            lblTransporter.Text = commandArgs[5].ToString();
            lblConsigneeNm.Text = commandArgs[6].ToString();
            hdnTransporter.Value = commandArgs[7].ToString();
            hdnVehicleNo.Value = commandArgs[3].ToString();
            ClearField();
            lblmsg.Text = "";
            DataView dv = (DataView)DataSourceRate.Select(DataSourceSelectArguments.Empty);
            DataTable dt = new DataTable();
            dt = dv.ToTable();
            //GetDetail(Convert.ToInt32(Session["TransReqId"]));
            if(dt.Rows.Count>0)
            {
                //GVVehicle.DataSource = dt;
                GVVehicle.DataBind();
                dvgridDisplay.Visible = true;
                tblEntryTable.Visible = false;
                tblSave.Visible = false;
            }
            else
            {
                dvgridDisplay.Visible = false;
                tblEntryTable.Visible = true;
                tblSave.Visible = true;
            }
            
            mpeVehicleDeliveryAdd.Show();
        }
        else if (e.CommandName.ToLower().Trim() == "edit")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            //lblError.Text = commandArgs[3].ToString()+"@@"+ commandArgs[2].ToString();
            Session["TransRateId"] = commandArgs[2].ToString();
            lblJobRefNo1.Text = commandArgs[0].ToString();
            lblJobRefNo.Text = commandArgs[1].ToString();
            Session["JobType"] = commandArgs[3].ToString();
            Session["DeliveryType"] = commandArgs[4].ToString();
            //DataTable tbl = GridViewVehicle.DataSource as DataTable;
            //GridViewVehicle.DataSource = DataSourceVehicle;
            //ViewState["DataSourceVehicle"] = DataSourceVehicle;
            //DataTable dt = ViewState["DataSourceVehicle"] as DataTable;

            DataTable dt = new DataTable();

            // Get the data from the SqlDataSource using Select
            DataView dataView = DataSourceVehicle.Select(DataSourceSelectArguments.Empty) as DataView;
            dt = dataView.ToTable();
            foreach (DataRow row in dt.Rows)
            {
                if (row["TransRateId"].ToString() == Session["TransRateId"].ToString())
                {
                    txtRptDate.Text = row["ReportingDate"].ToString();
                    txtUnloadDate.Text = row["UnloadingDate"].ToString();
                    break;
                }
            }
            mpeMovementUpdate.Show();
        }
    }

    protected void GridViewVehicle_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewVehicle.EditIndex = e.NewEditIndex;
    }

    protected void GridViewVehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        bool MovementCompleted = false;
        int RateId = Convert.ToInt32(GridViewVehicle.DataKeys[e.RowIndex].Value.ToString());
        int DeliveryType = 0, JobType = 0;

        JobType = Convert.ToInt32(((HiddenField)GridViewVehicle.Rows[e.RowIndex].FindControl("hdnJobType")).Value.Trim());
        DeliveryType = Convert.ToInt32(((HiddenField)GridViewVehicle.Rows[e.RowIndex].FindControl("hdnDeliveryTypeId")).Value.Trim());

        TextBox txtReportingDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtReportingDate");
        TextBox txtUnLoadingDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtUnLoadingDate");
        TextBox txtContReturnDate = (TextBox)GridViewVehicle.Rows[e.RowIndex].FindControl("txtContReturnDate");

        DateTime dtReportingDate = DateTime.MinValue;
        DateTime dtUnLoadingDate = DateTime.MinValue;
        DateTime dtContReturnDate = DateTime.MinValue;

        if (txtReportingDate.Text.Trim() != "")
        {
            dtReportingDate = Commonfunctions.CDateTime(txtReportingDate.Text.Trim());
        }

        if (txtUnLoadingDate.Text.Trim() != "")
        {
            dtUnLoadingDate = Commonfunctions.CDateTime(txtUnLoadingDate.Text.Trim());
        }

        if (JobType == 1 && DeliveryType == 1)  // Import job and loaded delivery type
        {
            if (txtContReturnDate.Text.Trim() != "")
            {
                dtContReturnDate = Commonfunctions.CDateTime(txtContReturnDate.Text.Trim());
            }

            if (dtContReturnDate != DateTime.MinValue && dtReportingDate != DateTime.MinValue && dtUnLoadingDate != DateTime.MinValue)
            {
                MovementCompleted = true;
            }
        }
        else
        {
            if (dtReportingDate != DateTime.MinValue && dtUnLoadingDate != DateTime.MinValue)
            {
                MovementCompleted = true;
            }
        }

        int result = DBOperations.AddTransMovement(RateId, dtReportingDate, dtUnLoadingDate, dtContReturnDate, MovementCompleted, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Detail Updated Successfully.";
            lblError.CssClass = "success";

            GridViewVehicle.EditIndex = -1;
            e.Cancel = true;
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
            e.Cancel = true;
        }
    }

    protected void GridViewVehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridViewVehicle.EditIndex = -1;
    }

    protected void GridViewVehicle_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
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
            DataFilter1.FilterSessionID = "TransMovement.aspx";
            DataFilter1.FilterDataSource();
            GridViewVehicle.DataBind();
        }
        catch (Exception ex)
        {
            DataFilter1.Info = ex.Message; // Handle your exceptions
        }
    }

    #endregion

    #region Daily Status Events
    protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
    {
        mpeDailyStatus.Hide();
    }

    protected void btnShowDraftMail_Click(object sender, EventArgs e)
    {
        if (txtDailyStatus.Text != "")
        {
            string EmailContent = "";
            string MessageBody = "";
            txtSubject.Text = "Transport Status for Job " + hdnJobRefNo.Value + " (" + hdnVehicleNo.Value + ") as dispatched on - " + hdnDispatchDate.Value;
            try
            {
                string strFileName = "../EmailTemplate/TransportDraftStatus.txt";
                StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                sr = File.OpenText(Server.MapPath(strFileName));
                EmailContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.CssClass = "errorMsg";
                return;
            }

            MessageBody = EmailContent.Replace("@DailyStatus", txtDailyStatus.Text.Trim());
            MessageBody = MessageBody.Replace("@JobRefNo", hdnJobRefNo.Value);
            //MessageBody = MessageBody.Replace("@Customer", hdnCustomer.Value.ToUpper().Trim());
            MessageBody = MessageBody.Replace("@CustRefNo", hdnCustRefNo.Value);
            MessageBody = MessageBody.Replace("@VehicleNo", hdnVehicleNo.Value);
            MessageBody = MessageBody.Replace("@DispatchDate", hdnDispatchDate.Value);
            MessageBody = MessageBody.Replace("@VehicleType", hdnVehicleType.Value.ToUpper().Trim());
            MessageBody = MessageBody.Replace("@DeliveryFrom", hdnDeliveryFrom.Value.ToUpper().Trim());
            MessageBody = MessageBody.Replace("@DeliveryTo", hdnDeliveryTo.Value.ToUpper().Trim());
            MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);
            divPreviewEmail.InnerHtml = MessageBody;
            mpeDailyStatus.Show();
        }
        else
        {
            lblError_Popup.Text = "Enter daily status to view draft mail!";
            lblError_Popup.CssClass = "errorMsg";
            txtDailyStatus.Text = "";
            hdnJobRefNo.Value = "";
            hdnCustomer.Value = "";
            hdnCustRefNo.Value = "";
            hdnVehicleNo.Value = "";
            hdnDispatchDate.Value = "";
            hdnVehicleType.Value = "";
            hdnDeliveryFrom.Value = "";
            hdnDeliveryTo.Value = "";
            lblCustomerEmail.Text = "";
            divPreviewEmail.InnerHtml = "";
            mpeDailyStatus.Show();
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        bool bEmailSuccess = false;
        if (lblCustomerEmail.Text != "")
        {
            if (txtDailyStatus.Text.Trim() != "")
            {
                string EmailContent = "";
                string MessageBody = "";
                txtSubject.Text = "Transport Status for Job " + hdnJobRefNo.Value + " (" + hdnVehicleNo.Value + ") as dispatched on - " + hdnDispatchDate.Value;

                try
                {
                    string strFileName = "../EmailTemplate/TransportStatus.txt";
                    StreamReader sr = new StreamReader(Server.MapPath(strFileName));
                    sr = File.OpenText(Server.MapPath(strFileName));
                    EmailContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    lblError.CssClass = "errorMsg";
                    return;
                }

                MessageBody = EmailContent.Replace("@DailyStatus", txtDailyStatus.Text.Trim());
                MessageBody = MessageBody.Replace("@JobRefNo", hdnJobRefNo.Value);
                //MessageBody = MessageBody.Replace("@Customer", hdnCustomer.Value.ToUpper().Trim());
                MessageBody = MessageBody.Replace("@CustRefNo", hdnCustRefNo.Value);
                MessageBody = MessageBody.Replace("@VehicleNo", hdnVehicleNo.Value);
                MessageBody = MessageBody.Replace("@DispatchDate", hdnDispatchDate.Value);
                MessageBody = MessageBody.Replace("@VehicleType", hdnVehicleType.Value.ToUpper().Trim());
                MessageBody = MessageBody.Replace("@DeliveryFrom", hdnDeliveryFrom.Value.ToUpper().Trim());
                MessageBody = MessageBody.Replace("@DeliveryTo", hdnDeliveryTo.Value.ToUpper().Trim());
                MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);
                divPreviewEmail.InnerHtml = MessageBody;
                List<string> lstFileDoc = new List<string>();

                bEmailSuccess = EMail.SendMailMultiAttach(lblCustomerEmail.Text.Trim(), lblCustomerEmail.Text.Trim(), txtMailCC.Text.Trim(), txtSubject.Text.Trim(), MessageBody, lstFileDoc);
                //bEmailSuccess = EMail.SendMailMultiAttach("kivisha.jain@babajishivram.com", "kivisha.jain@babajishivram.com", "", txtSubject.Text.Trim(), MessageBody, lstFileDoc);
                if (bEmailSuccess == true)
                {
                    if (hdnTransReqId.Value != "" && hdnTransReqId.Value != "0")
                    {
                        int result = DBOperations.AddDailyStatusHistory(Convert.ToInt32(hdnTransReqId.Value), txtDailyStatus.Text.Trim(), lblCustomerEmail.Text.Trim(), txtMailCC.Text.Trim(), LoggedInUser.glUserId);
                    }
                    string url = "TransMovement.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += "Successfully sent mail!";
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else
                {
                    lblError_Popup.Text = "Error while sending mail. Please try again later!";
                    lblError_Popup.CssClass = "errorMsg";
                    mpeDailyStatus.Show();
                }
            }
        }
        else
        {
            lblError_Popup.Text = "Email To is missing!";
            lblError_Popup.CssClass = "errorMsg";
            mpeDailyStatus.Show();
        }
    }

    protected void btnCancelEmailPp_Click(object sender, EventArgs e)
    {
        mpeDailyStatus.Hide();
    }

    #endregion

    #region ExportData

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        // string strFileName = "ProjectTasksList_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xlsx";
        string strFileName = "Movement Pending On" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";
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
        GridViewVehicle.AllowPaging = false;
        GridViewVehicle.AllowSorting = false;
        GridViewVehicle.Columns[0].Visible = false;
        GridViewVehicle.Columns[1].Visible = false;
        GridViewVehicle.Caption = "Transport Movement Pending On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "TransMovement.aspx";
        DataFilter1.FilterDataSource();
        GridViewVehicle.DataBind();
        GridViewVehicle.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }

    #endregion

    #region Delivery Detail

    protected void imgClose_Delivery_Click(object sender, ImageClickEventArgs e)
    {
        mpeVehicleDelivery.Hide();
    }

    //protected void btnUpdateVehicle_Click(object sender, EventArgs e)
    //{
    //    int ConsolidateID = 0, result = -789, count = -123;
    //    if (hdnLid.Value != "" && hdnLid.Value != "0")
    //    {
    //        DateTime LRDate = DateTime.MinValue, ChallanDate = DateTime.MinValue;

    //        if (txtLRDate.Text.Trim() != "")
    //            LRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());
    //        if (txtBabajiChallanDate.Text.Trim() != "")
    //            ChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());

    //        if (hdnConsolidateID.Value != "" && hdnConsolidateID.Value != "0")
    //        {
    //            ConsolidateID = Convert.ToInt32(hdnConsolidateID.Value);
    //            DataSet dsGetTRJobDetail = DBOperations.GetConsolidateJobDetail(ConsolidateID);
    //            if (dsGetTRJobDetail != null && dsGetTRJobDetail.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < dsGetTRJobDetail.Tables[0].Rows.Count; i++)
    //                {
    //                    result = DBOperations.TR_UpdateVehicleDetail(Convert.ToInt32(hdnTransRateId.Value), ConsolidateID, Convert.ToInt32(dsGetTRJobDetail.Tables[0].Rows[i]["JobDeliveryId"].ToString()), txtVehicleNo.Text.Trim(), txtLRNo.Text.Trim(),
    //                                            txtBabajiChallanNo.Text.Trim(), LRDate, ChallanDate, Convert.ToInt32(hdnTransporterId.Value), LoggedInUser.glUserId);
    //                    if (result == 0)
    //                    {
    //                        count++;
    //                        lblErrorDeliveryPopup.Text = "Successfully updated vehicle detail.";
    //                        lblErrorDeliveryPopup.CssClass = "errorMsg";
    //                    }
    //                    else
    //                    {
    //                        count = 0;
    //                        lblErrorDeliveryPopup.Text = "System error. Please try again later.";
    //                        lblErrorDeliveryPopup.CssClass = "errorMsg";
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            result = DBOperations.TR_UpdateVehicleDetail(Convert.ToInt32(hdnTransRateId.Value), ConsolidateID, Convert.ToInt32(hdnLid.Value), txtVehicleNo.Text.Trim(), txtLRNo.Text.Trim(),
    //               txtBabajiChallanNo.Text.Trim(), LRDate, ChallanDate, Convert.ToInt32(hdnTransporterId.Value), LoggedInUser.glUserId);
    //            if (result == 0)
    //            {
    //                count++;
    //                lblErrorDeliveryPopup.Text = "Successfully updated vehicle detail.";
    //                lblErrorDeliveryPopup.CssClass = "errorMsg";
    //            }
    //            else
    //            {
    //                count = 0;
    //                lblErrorDeliveryPopup.Text = "System error. Please try again later.";
    //                lblErrorDeliveryPopup.CssClass = "errorMsg";
    //            }
    //        }
    //        mpeVehicleDelivery.Show();
    //    }
    //}

    protected void btnCancelVehicle_Click(object sender, EventArgs e)
    {
        mpeVehicleDelivery.Hide();
    }

    #endregion

    #region Grid View - Popup Vehicle Update

    protected void gvVehicleDelivery_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVehicleDelivery.EditIndex = e.NewEditIndex;
        mpeVehicleDelivery.Show();
    }

    protected void gvVehicleDelivery_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVehicleDelivery.EditIndex = -1;
        mpeVehicleDelivery.Show();
    }

    protected void gvVehicleDelivery_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int DeliveryId = 0, JobId = 0, RateId = 0, ConsolidateID = 0, DeliveryConsolidateID = 0;
        DateTime dtLRDate = DateTime.MinValue;
        DateTime dtChallanDate = DateTime.MinValue;
        string EmailCopy = "";//19-09-2020

        DeliveryId = Convert.ToInt32(gvVehicleDelivery.DataKeys[e.RowIndex].Values[0].ToString());
        JobId = Convert.ToInt32(gvVehicleDelivery.DataKeys[e.RowIndex].Values[1].ToString());

        HiddenField hdnRateId = (HiddenField)gvVehicleDelivery.Rows[e.RowIndex].FindControl("hdnRateId");
        HiddenField hdnDeliveryConsolidateID = (HiddenField)gvVehicleDelivery.Rows[e.RowIndex].FindControl("hdnDeliveryConsolidateID");
        TextBox txtLRNo = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtLRNo");
        TextBox txtLRDate = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtLRDate");
        TextBox txtBabajiChallanNo = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtBabajiChallanNo");
        TextBox txtBabajiChallanDate = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtBabajiChallanDate");
        TextBox txtVehicleNo = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtVehicleNo");

        //19-09-2020
        TextBox txtDetensionAmt = (TextBox)gvVehicleDelivery.Rows[e.RowIndex].FindControl("txtDetensionAmt");

        if (hdnRateId.Value != "" && hdnRateId.Value != "0")
            RateId = Convert.ToInt32(hdnRateId.Value);
        if (hdnPopupConsolidateID.Value != "" && hdnPopupConsolidateID.Value != "0")
            ConsolidateID = Convert.ToInt32(hdnPopupConsolidateID.Value);
        if (hdnDeliveryConsolidateID.Value != "" && hdnDeliveryConsolidateID.Value != "0")
            DeliveryConsolidateID = Convert.ToInt32(hdnDeliveryConsolidateID.Value);

        if (txtLRDate.Text.Trim() != "")
            dtLRDate = Commonfunctions.CDateTime(txtLRDate.Text.Trim());

        if (txtBabajiChallanDate.Text.Trim() != "")
            dtChallanDate = Commonfunctions.CDateTime(txtBabajiChallanDate.Text.Trim());

        int result = DBOperations.TR_UpdateVehicleNo(RateId, DeliveryConsolidateID, txtVehicleNo.Text.Trim(), LoggedInUser.glUserId);
        int result_Vehicle = DBOperations.TR_UpdateVehicleDetail(DeliveryId, ConsolidateID, JobId, txtVehicleNo.Text.Trim(), txtLRNo.Text.Trim(), txtBabajiChallanNo.Text.Trim(),
                                            dtLRDate, dtChallanDate, LoggedInUser.glUserId);

        //if (result == 0)
        //{
        //    lblErrorDeliveryPopup.Text = "Vehicle Detail Updated Successfully.";
        //    lblErrorDeliveryPopup.CssClass = "success";

        //    gvVehicleDelivery.EditIndex = -1;
        //    e.Cancel = true;
        //    mpeVehicleDelivery.Show();

        //    DataSet dsGetJobDetails = DBOperations.GetTransRateDetail(Convert.ToInt32(hdnTransReqId_Vehicle.Value));
        //    if (dsGetJobDetails != null && dsGetJobDetails.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsGetJobDetails.Tables[0].Rows.Count; i++)
        //        {
        //            int JobId_Loc = Convert.ToInt32(dsGetJobDetails.Tables[0].Rows[i]["JobId"].ToString());
        //            int CustomerId_Loc = Convert.ToInt32(dsGetJobDetails.Tables[0].Rows[i]["CustomerId"].ToString());
        //            int result_MailCust = DBOperations.TR_MailCustomerDelivery(CustomerId_Loc, JobId_Loc, txtVehicleNo.Text.Trim(), txtLRNo.Text.Trim(), txtBabajiChallanNo.Text.Trim(),
        //                                    dtLRDate, dtChallanDate);
        //        }
        //    }
        //}
        //else if (result == 1)
        //{
        //    lblErrorDeliveryPopup.Text = "System Error! Please Try After Sometime.";
        //    lblErrorDeliveryPopup.CssClass = "errorMsg";
        //    e.Cancel = true;
        //    mpeVehicleDelivery.Show();
        //}
    }

    #endregion

    #region GPRS Tracking

    //public string TrackVehicle()
    //{
    //    string strVehicleNo = hdnVehicleNo.Value;// ddVehicle.SelectedValue;

    //    strVehicleNo = strVehicleNo.Replace("-", "");

    //    strVehicleNo = strVehicleNo.Replace(" ", "");

    //    string strIMEINO = "", strResponse = "";

    //    VehicleIMEIDict.VehicleIMEI.TryGetValue(strVehicleNo.Trim(), out strIMEINO);

    //    if (strIMEINO != null)
    //    {
    //        var client = new RestClient("http://clients.letstrack.in/All/api/GetDeviceLocation");

    //        var request = new RestRequest(Method.POST);
    //        request.AddHeader("cache-control", "no-cache");
    //        request.AddHeader("authorization", "Basic YmFiYWppOjBVISE1MiMyQExU");
    //        request.AddHeader("content-type", "application/json");
    //        request.AddParameter("application/json", "{\"UserId\":\"76\",\"IMEI\":\"" + strIMEINO + "\"}", ParameterType.RequestBody);
    //        IRestResponse response = client.Execute(request);

    //        var resVehicleList = TransportTrack.resVehicleTrack.FromJson(response.Content);

    //        // List< TransportTrack.resVehicleDetail> test = resVehicleTrack.Details;

    //        if (resVehicleList.Result.Code == 1)
    //        {
    //            string Latitude = "";
    //            string Longitude = "";
    //            string Position = "map.setCenter(new GLatLng(18.8625,73.1667), 13);";

    //            foreach (var item in resVehicleList.Details)
    //            {
    //                Latitude = item.Latitude.ToString();
    //                Longitude = item.Longitude.ToString();

    //                // create a line of JavaScript for marker on map for this record	
    //                // Locations += Environment.NewLine + " map.addOverlay(new GMarker(new GLatLng(" + Latitude + "," + Longitude + ")));";

    //                // Position = Environment.NewLine + " map.setCenter(new GLatLng(" + Latitude + "," + Longitude + "), 11); ";
    //            }

    //            strResponse = response.Content;
    //        }
    //    }
    //    return strResponse;
    //}

    #endregion

    protected void txtDailyStatus_TextChanged(object sender, EventArgs e)
    {
        btnShowDraftMail_Click(null, EventArgs.Empty);
    }


    //19-09-2020
    private string UploadDoc(FileUpload fuDocument)
    {
        string FileName = "", FilePath = "";
        FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (Session["Docid"].ToString() == "106")
        {
            if (FilePath == "")
                FilePath = "Transport_DetentionDoc" + Session["TransReqId"] + "\\";

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportDetentionDoc\\" + FilePath);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + FilePath;
            }
        }
        else if (Session["Docid"].ToString() == "107")
        {
            if (FilePath == "")
                FilePath = "Transport_VaraiDoc" + Session["TransReqId"].ToString() + "\\";

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportVaraiDoc\\" + FilePath);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + FilePath;
            }

        }
        else if (Session["Docid"].ToString() == "108")
        {
            if (FilePath == "")
                FilePath = "Transport_EmptyContDoc" + Session["TransReqId"].ToString() + "\\";

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportEmptyContDoc\\" + FilePath);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + FilePath;
            }
        }
        else if (Session["Docid"].ToString() == "109")
        {
            if (FilePath == "")
                FilePath = "Transport_TollDoc" + Session["TransReqId"].ToString() + "\\";

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportTollDoc\\" + FilePath);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + FilePath;
            }
        }
        else if (Session["Docid"].ToString() == "110")
        {
            if (FilePath == "")
                FilePath = "Transport_OtherDoc" + Session["TransReqId"].ToString() + "\\";

            if (ServerFilePath == "")
            {
                // Application Directory Path
                ServerFilePath = Server.MapPath("..\\UploadFiles\\TransportOtherDoc\\" + FilePath);
            }
            else
            {
                // File Server Path
                ServerFilePath = ServerFilePath + FilePath;
            }
        }

        if (!System.IO.Directory.Exists(ServerFilePath))
        {
            System.IO.Directory.CreateDirectory(ServerFilePath);
        }
        if (fuDocument.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FileName);
                FileName = Path.GetFileNameWithoutExtension(FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuDocument.SaveAs(ServerFilePath + FileName);

            return FilePath + FileName;
        }

        else
        {
            return "";
        }
    }

    public string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {

            //26 letters in the alfabet, ascii + 65 for the capital letters
            builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65))));

        }
        return builder.ToString();
    }
    protected void imgClose_DeliveryAdd_Click(object sender, ImageClickEventArgs e)
    {
        mpeVehicleDeliveryAdd.Hide();
    }

    protected void chkDetention_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDetention.Checked == true)
        {
            txtDetention.Enabled = true;
            fuDetention.Enabled = true;
            sp1.Visible = true;
            sp2.Visible = true;
        }
        else
        {
            txtDetention.Enabled = false;
            fuDetention.Enabled = false;
            sp1.Visible = false;
            sp2.Visible = false;
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected void chkVarai_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVarai.Checked == true)
        {
            txtVarai.Enabled = true;
            fuVarai.Enabled = true;
            sp3.Visible = true;
            sp4.Visible = true;
        }
        else
        {
            txtVarai.Enabled = false;
            fuVarai.Enabled = false;
            sp3.Visible = false;
            sp4.Visible = false;
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        
        if (chkDetention.Checked == true)
        {
            if (txtDetention.Text == "" || fuDetention.FileName == "")
            {
                lblmsg.Text = "Please enter detention amount and upload detention document";
                lblmsg.CssClass = "errorMsg";
            }
        }

        if (chkVarai.Checked == true)
        {
            if (txtVarai.Text == "" || fuVarai.FileName == "")
            {
                lblmsg.Text = lblmsg.Text + "<br />" + "Please enter Varai amount and upload Varai document";
                lblmsg.CssClass = "errorMsg";
            }
        }

        if (chkEmptyContCharge.Checked == true)
        {
            if (txtEmptyContCharge.Text == "" || fuEmptyContCharge.FileName == "")
            {
                lblmsg.Text = lblmsg.Text + "<br />" + "Please enter Empty Cont amount and upload Emty Cont document";
                lblmsg.CssClass = "errorMsg";
            }
        }

        if (chkTollCharges.Checked == true)
        {
            if (txtTollCharge.Text == "" || fuTollCharge.FileName == "")
            {
                lblmsg.Text = lblmsg.Text + "<br />" + "Please enter toll amount and upload toll document";
                lblmsg.CssClass = "errorMsg";
            }
        }

        if (chkOtherCharge.Checked == true)
        {
            if (txtOtherCharge.Text == "" || fuOther.FileName == "")
            {
                lblmsg.Text = lblmsg.Text + "<br />" + "Please enter other amount and upload other document";
                lblmsg.CssClass = "errorMsg";
            }
        }



        if (lblmsg.Text == "")
        {
            SaveDetail();
            dvgridDisplay.Visible = true;
            GVVehicle.Visible = true;
            GVVehicle.DataBind();
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected void SaveDetail()
    {
        string strDetentionFilePath = "", strVaraiFilePath = "",strEmptyContFilePath="",strTollFilePath="",StrOtherFilePath="";
        int strDetentionDocId = 0, strVaraiDocId = 0,strEmptyContDocId=0,strTollDocId=0,strOtherDocID=0;
        decimal VaraiAmt = 0;
        decimal DetentionAmt = 0;
        decimal EmptyContAmt = 0,TollAmt=0,OtherAmt=0;

        if (fuDetention.HasFile)
        {
            strDetentionDocId = 106;
            Session["Docid"] = strDetentionDocId;
            strDetentionFilePath = UploadDoc(fuDetention);
            Session["Docid"] = 0;
        }

        if (fuVarai.HasFile)
        {
            strVaraiDocId = 107;
            Session["Docid"] = strVaraiDocId;
            strVaraiFilePath = UploadDoc(fuVarai);
            Session["Docid"] = 0;
        }

        if (fuEmptyContCharge.HasFile)
        {
            strEmptyContDocId = 108;
            Session["Docid"] = strEmptyContDocId;
            strEmptyContFilePath = UploadDoc(fuEmptyContCharge);
            Session["Docid"] = 0;
        }

        if (fuTollCharge.HasFile)
        {
            strTollDocId = 109;
            Session["Docid"] = strTollDocId;
            strTollFilePath = UploadDoc(fuTollCharge);
            Session["Docid"] = 0;
        }

        if (fuOther.HasFile)
        {
            strOtherDocID = 110;
            Session["Docid"] = strOtherDocID;
            StrOtherFilePath = UploadDoc(fuOther);
            Session["Docid"] = 0;
        }

        if (txtDetention.Text != "")
            DetentionAmt = Convert.ToDecimal(txtDetention.Text);
        if (txtVarai.Text != "")
            VaraiAmt = Convert.ToDecimal(txtVarai.Text);
        if (txtEmptyContCharge.Text != "")
            EmptyContAmt = Convert.ToDecimal(txtEmptyContCharge.Text);
        if (txtTollCharge.Text != "")
            TollAmt = Convert.ToDecimal(txtTollCharge.Text);
        if (txtOtherCharge.Text != "")
            OtherAmt = Convert.ToDecimal(txtOtherCharge.Text);

        int result = DBOperations.TR_updTransDetail(Convert.ToInt32(Session["TransReqId"]), DetentionAmt,
            VaraiAmt,EmptyContAmt,TollAmt,OtherAmt, strDetentionDocId, strDetentionFilePath, fuDetention.FileName, strVaraiDocId, strVaraiFilePath,fuVarai.FileName,
            strEmptyContDocId,strEmptyContFilePath,fuEmptyContCharge.FileName,strTollDocId,strTollFilePath,fuTollCharge.FileName,
            strOtherDocID,StrOtherFilePath, fuOther.FileName,txtBillingInstruction.Text, LoggedInUser.glUserId);

        if (result == 0)
        {
            tblEntryTable.Visible = false;
            tblSave.Visible = false;
            dvgridDisplay.Visible = true;
            //GVVehicle.DataSource = DataSourceRate;
            GVVehicle.DataBind();
            lblmsg.Text = "Successfully updated rate detail.";
            lblmsg.CssClass = "success";

            ClearField();
        }
    }

    protected void ClearField()
    {
        txtDetention.Text = "";
        txtVarai.Text = "";
        txtEmptyContCharge.Text = "";
        txtOtherCharge.Text = "";
        txtTollCharge.Text = "";
        txtBillingInstruction.Text = "";
        chkDetention.Checked = false;
        chkVarai.Checked = false;
        chkEmptyContCharge.Checked = false;
        chkOtherCharge.Checked = false;
        chkTollCharges.Checked = false;
        txtDetention.Enabled = false;
        txtVarai.Enabled = false;
        txtEmptyContCharge.Enabled = false;
        txtTollCharge.Enabled = false;
        txtOtherCharge.Enabled = false;
        fuDetention.Enabled = false;
        fuVarai.Enabled = false;
        fuEmptyContCharge.Enabled = false;
        fuOther.Enabled = false;
        fuTollCharge.Enabled = false;
        sp1.Visible = false;
        sp2.Visible = false;
        sp3.Visible = false;
        sp4.Visible = false;
        sp5.Visible = false;
        sp6.Visible = false;
        sp7.Visible = false;
        sp8.Visible = false;
        sp9.Visible = false;
        sp10.Visible = false;
    }

    protected void GVVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "downloadcopy")
        {
            string DocPath = e.CommandArgument.ToString();
            string[] commandArgs = e.CommandArgument.ToString().Split(';');
            DownloadDocument(commandArgs[0].ToString(),commandArgs[1].ToString());
        }
        
        mpeVehicleDeliveryAdd.Show();
    }

    private void DownloadDocument(string DocumentPath, string Documentname)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if(Documentname== "DetentionDoc")
        {
            DocumentPath = "TransportDetentionDoc\\" + DocumentPath;
        }
        else if(Documentname == "VaraiDoc")
        {
            DocumentPath = "TransportVaraiDoc\\" + DocumentPath;
        }
        else if (Documentname == "EmptyContDoc")
        {
            DocumentPath = "TransportEmptyContDoc\\" +DocumentPath; 
        }
        else if (Documentname == "TollDoc")
        {
            DocumentPath = "TransportTollDoc\\" + DocumentPath;
        }
        else if (Documentname == "OtherDoc")
        {
            DocumentPath = "TransportOtherDoc\\" + DocumentPath;
        }

        if (ServerPath == "")
        {
            ServerPath = Server.MapPath(DocumentPath);
            ServerPath = ServerPath.Replace("Transport\\", "");
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void chkTollCharges_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTollCharges.Checked == true)
        {
            txtTollCharge.Enabled = true;
            fuTollCharge.Enabled = true;
            sp7.Visible = true;
            sp8.Visible = true;
        }
        else
        {
            txtTollCharge.Enabled = false;
            fuTollCharge.Enabled = false;
            sp7.Visible = false;
            sp8.Visible = false;
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected void chkEmptyContCharge_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEmptyContCharge.Checked == true)
        {
            txtEmptyContCharge.Enabled = true;
            fuEmptyContCharge.Enabled = true;
            sp5.Visible = true;
            sp6.Visible = true;
        }
        else
        {
            txtEmptyContCharge.Enabled = false;
            fuEmptyContCharge.Enabled = false;
            sp5.Visible = false;
            sp6.Visible = false;
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected void chkOtherCharge_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOtherCharge.Checked == true)
        {
            txtOtherCharge.Enabled = true;
            fuOther.Enabled = true;
            sp9.Visible = true;
            sp10.Visible = true;
        }
        else
        {
            txtOtherCharge.Enabled = false;
            fuOther.Enabled = false;
            sp9.Visible = false;
            sp10.Visible = false;
        }
        mpeVehicleDeliveryAdd.Show();
    }

    protected bool DecideHere(string Str)
    {
        if (Str == "0" || Str =="")
            return false;
        else
            return true;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        bool MovementCompleted = false;
        //int RateId = Convert.ToInt32(GridViewVehicle.DataK.Value.ToString());
        int DeliveryType = 0, JobType = 0;

        DateTime dtReportingDate = DateTime.MinValue;
        DateTime dtUnLoadingDate = DateTime.MinValue;
        DateTime dtContReturnDate = DateTime.MinValue;
        JobType = Convert.ToInt32(Session["JobType"]);
        DeliveryType = Convert.ToInt32(Session["DeliveryType"]);

        if (txtRptDate.Text.Trim() != "")
        {
            dtReportingDate = Commonfunctions.CDateTime(txtRptDate.Text.Trim());
        }

        if (txtUnloadDate.Text.Trim() != "")
        {
            dtUnLoadingDate = Commonfunctions.CDateTime(txtUnloadDate.Text.Trim());
        }

        if (JobType == 1 && DeliveryType == 1)  // Import job and loaded delivery type
        {
            if (txtContReturndate.Text.Trim() != "")
            {
                dtContReturnDate = Commonfunctions.CDateTime(txtContReturndate.Text.Trim());
            }

            if (dtContReturnDate != DateTime.MinValue && dtReportingDate != DateTime.MinValue && dtUnLoadingDate != DateTime.MinValue)
            {
                MovementCompleted = true;
            }
        }
        else
        {
            if (dtReportingDate != DateTime.MinValue && dtUnLoadingDate != DateTime.MinValue)
            {
                MovementCompleted = true;
            }
        }

        int result = DBOperations.AddTransMovement(Convert.ToInt32(Session["TransRateId"]), dtReportingDate, dtUnLoadingDate, dtContReturnDate, MovementCompleted, LoggedInUser.glUserId);
        if (result == 0)
        {
            lblError.Text = "Detail Updated Successfully.";
            lblError.CssClass = "success";
        }
        else if (result == 1)
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
        else
        {
            lblError.Text = "System Error! Please Try After Sometime.";
            lblError.CssClass = "errorMsg";
        }
        mpeMovementUpdate.Hide();
    }
}