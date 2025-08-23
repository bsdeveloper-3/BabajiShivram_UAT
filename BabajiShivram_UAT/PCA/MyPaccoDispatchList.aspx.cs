using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MyPacco.API;
public partial class PCA_MyPaccoDispatchList : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "MyPacco Dispatch";

        HtmlAnchor hrefGoBack = (HtmlAnchor)Page.Master.FindControl("hrefGoBack");
        if (hrefGoBack != null)
        {
            hrefGoBack.HRef = "PCA/PendingBillDispatch.aspx";
        }

        if (!IsPostBack)
        {
            if (Session["DispatchJobIdList"] != null && Session["DispatchCustomerId"] != null)
            {
                hdnJobIdList.Value = Session["DispatchJobIdList"].ToString();
                hdnCustomerId.Value = Session["DispatchCustomerId"].ToString();
            }

            DataSourceBillJobList.SelectParameters[0].DefaultValue = hdnJobIdList.Value; // "200117"; //346660
            DataSourceBillJobList.SelectParameters[1].DefaultValue = "1";
            DataSourceBillJobList.DataBind();
            gvJobDetail.DataBind();

            // Get Customer Plant Address

            BindPlantAddress();

            // Get Customer Name

           DataView dvCustomer = DBOperations.GetCustomerDetail(hdnCustomerId.Value);

            if(dvCustomer.Table.Rows.Count> 0 )
            {
               hdnCustomerName.Value = dvCustomer.Table.Rows[0]["CustName"].ToString();
            }
            // Get Bill Dispatch Status

            //GetBillDispatchStatus(Convert.ToInt32(hdnJobId.Value));
        }
    }

    #region Event

    protected void gvJobDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        lblMessage.Text = "";
        hdnBillId.Value = "0";
        string strBJVNo = "";
        string strBJVBillNo = "";

        if (e.CommandName.ToLower() == "download")
        {
            //int BillCoverId = Convert.ToInt32(e.CommandArgument);

            //DataSet dsDocDetail = BillingOperation.GetBillCoverDocById(BillCoverId);

            //if (dsDocDetail.Tables[0].Rows.Count > 0)
            //{
            //    string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
            //    string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

            //    string strFilePath = strDocPath + "//" + strFileName;

            //    DownloadDocument(strFilePath);
            //}
            //else
            //{
            //    lblMessage.Text = "Bill Covering Letter Not Generated!";
            //    lblMessage.CssClass = "errorMsg";
            //}
        }
    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "DocId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DocId"));
                if (DocId == 0)
                {
                    CheckBox chkBillNo = (CheckBox)e.Row.FindControl("chkBillNo");

                    if (chkBillNo != null)
                    {
                        chkBillNo.Visible = false;
                    }

                    e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
                    e.Row.ToolTip = "Bill Not Uploaded!";
                }
            }
        
        //if (DataBinder.Eval(e.Row.DataItem, "CoverId") != DBNull.Value)
        //{
        //    int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CoverId"));
        //    if (DocId == 0)
        //    {
        //        //CheckBox chkBillNo = (CheckBox)e.Row.FindControl("chkBillNo");
        //        LinkButton lnkCoverView = (LinkButton)e.Row.FindControl("lnkCoverView");

        //        if (lnkCoverView != null)
        //        {
        //            lnkCoverView.Visible = false;
        //        }

        //        e.Row.BackColor = System.Drawing.Color.FromName("#f78686");
        //        e.Row.ToolTip = "Convering Letter Not Generated!";
        //    }
        //}
    }
    }

    #endregion

    #region Bill Cover Document

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..//UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + DocumentPath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, DocumentPath);
        }
        catch (Exception ex)
        {
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
    #endregion

    #region Plant Address
    private void BindPlantAddress()
    {
        string strJobidList = hdnJobIdList.Value;
        DataSet dsPlantAddressList = DBOperations.GetJobPlantAddressList(strJobidList);

        gvDispatchPlantAddress.DataSource = dsPlantAddressList;
        gvDispatchPlantAddress.DataBind();
    }
    #endregion
    
    #region MyPacco Dispatch
    protected void btnGenerateAWB_Click(object sender, EventArgs e)
    {
        MyPaccoAWBGeneration();
    }

    private List<BJVBill> GetJobBillList()
    {
        int JobId = 0;

        List<BJVBill> lstBillList = new List<BJVBill>();

        DataTable dtDispatchBill = new DataTable();

        // Check Selected Bill

        foreach (GridViewRow rw in gvJobDetail.Rows)
        {
            // Find Selected CheckBox
            JobId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[0]);
            int BillId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                string strBJVNo1 = "", strBJVBillNo1 = "";

                Label lblBJVNo = (Label)rw.FindControl("lblBJVNo");
                Label lblBillNumber = (Label)rw.FindControl("lblBillNumber");

                strBJVNo1 = lblBJVNo.Text;
                strBJVBillNo1 = lblBillNumber.Text;

                BJVBill objBJVBill = new BJVBill();
                objBJVBill.JobId = JobId;
                objBJVBill.BJVNo = strBJVNo1;
                objBJVBill.BJVBillNo = strBJVBillNo1;

                lstBillList.Add(objBJVBill);
            }

        }//END_ForEach

        return lstBillList;
    }
    protected void MyPaccoAWBGeneration()
    {
        List<BJVBill> lstBillList = GetJobBillList();

        int i = 0;

        string JobType = "";
        ViewState["AWBJobid"] = "";

        foreach (BJVBill item in lstBillList)
        {
            if (ViewState["AWBJobid"].ToString() == "")
            {
                ViewState["AWBJobid"] = item.JobId;
            }
            else
            {
                ViewState["AWBJobid"] = ViewState["AWBJobid"].ToString() + ',' + item.JobId;
            }
        }

        if (ViewState["AWBJobid"].ToString() != "")
        {
            String strjobIdList = ViewState["AWBJobid"].ToString();

            // Check IF AWB Already Generated Against Job List

            DataSet dsJobAWB = DBOperations.MyPaccoCheckAWBForJobList(strjobIdList);

            if (dsJobAWB.Tables.Count > 0)
            {
                if (dsJobAWB.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Text = "My Pacco Order Already Created.";
                    lblMessage.CssClass = "errorMsg";

                    return;
                }
                else
                {
                    GenerateAWB();
                }
            }
            else
            {
                GenerateAWB();
            }
        }
        else
        {
            lblMessage.Text = "Please Select Job Number!";
            lblMessage.CssClass = "errorMsg";
        }

    }

    private void GenerateAWB()
    {
        int BranchID = Convert.ToInt32(ddBranch.SelectedValue);
        int PlantAddressID = 0;
        string strWarehouseCode = "MWH0000012448"; // Test Code

        if(BranchID == 0)
        {
            lblMessage.Text = "Please check Dispatch From Location!";
            lblMessage.CssClass = "errorMsg";

            return;
        }
        else if (BranchID == 3)// Mumbai
        {
            strWarehouseCode = "MWH0000012511";
        }
        else if (BranchID == 5)// Delhi
        {
            strWarehouseCode = "MWH0000008110";
        }
        else if (BranchID == 6)//Chennai
        {
            strWarehouseCode = "MWH0000003885";
        }
        foreach (GridViewRow row in gvDispatchPlantAddress.Rows)
        {
            CheckBox chk = row.Cells[0].Controls[1] as CheckBox;
            if (chk != null && chk.Checked)
            {
                PlantAddressID = Convert.ToInt32(gvDispatchPlantAddress.DataKeys[row.RowIndex].Value);
            }
        }

        if (PlantAddressID == 0)
        {
            lblMessage.Text = "Please check Delivery Address!";
            lblMessage.CssClass = "errorMsg";

            return;
        }
        //else
        //{
        //    ModalPopupDispatch.Show();
        //    lblDispatchMessage.Text = "Please Address Found!";
        //    lblDispatchMessage.CssClass = "errorMsg";

        //    return;
        //}

        String strjobIdList = ViewState["AWBJobid"].ToString();

        DataSet dsPlantAddress = DBOperations.GetCustomerPlantAddressById(PlantAddressID);

        if (strjobIdList != "")
        {
            if (dsPlantAddress.Tables.Count > 0 && dsPlantAddress.Tables[0].Rows.Count > 0)
            {
                // Generate AWB
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                Root MyPaccoRoot = new Root();
                Datum MyPaccoDatum = new Datum();
                Order MyPaccoOrder = new Order();

                List<Datum> lstDatum = new List<Datum>();
                List<Order> lstOrder = new List<Order>();

                string strBabajiOrderNo = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")).ToString(); // DateTime.Today.TimeOfDay.ToString();
                DateTime dtAWBDate = DateTime.Now.AddDays(1);

                MyPaccoRoot.access_token = "";

                // MyPaccoResqAddOrderPl AWBBillDetail = new MyPaccoResqAddOrderPl();

                // MyPaccoOrder.seller_email = "amit.bakshi@babajishivram.com";

                MyPaccoOrder.seller_order_number = strBabajiOrderNo;
                MyPaccoOrder.seller_warehouse_code = strWarehouseCode;

                //MyPaccoOrder.pickup_fullname = "DO NOT PICKUP";// "Babaji Shivram";
                //MyPaccoOrder.pickup_mobile = "9833708840";
                //MyPaccoOrder.pickup_email = "amit.bakshi@babajishivram.com";
                //MyPaccoOrder.pickup_address = "For Testing, Pls Do Not Pickup "; //"Plot No 2A, Sakinka Andheri E Mumbai 450072";
                //MyPaccoOrder.pickup_pincode = "450072"
                ;
                //MyPaccoOrder.pickup_landmark = "Test";
                //MyPaccoOrder.pickup_city = "Test";
                //MyPaccoOrder.pickup_state = "Test";
                //MyPaccoOrder.pickup_country = "IN";

                MyPaccoOrder.delivery_fullname = dsPlantAddress.Tables[0].Rows[0]["ContactPerson"].ToString();
                MyPaccoOrder.delivery_mobile = dsPlantAddress.Tables[0].Rows[0]["MobileNo"].ToString();

                // Check If Customer Email Address is Valid

                string strDeliveryEmail = "sajid.shaikh@babajishivram.com";

                if (dsPlantAddress.Tables[0].Rows[0]["Email"] != DBNull.Value)
                {
                    strDeliveryEmail = dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                if (IsValidEmail(strDeliveryEmail))
                {
                    MyPaccoOrder.delivery_email = strDeliveryEmail;
                }
                else
                {
                    MyPaccoOrder.delivery_email = "sajid.shaikh@babajishivram.com";// dsPlantAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                MyPaccoOrder.delivery_address = hdnCustomerName.Value + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine1"].ToString() + " " + dsPlantAddress.Tables[0].Rows[0]["AddressLine2"].ToString();
                MyPaccoOrder.delivery_pincode = dsPlantAddress.Tables[0].Rows[0]["Pincode"].ToString();
                MyPaccoOrder.delivery_landmark = "-";
                MyPaccoOrder.delivery_city = dsPlantAddress.Tables[0].Rows[0]["City"].ToString();
                MyPaccoOrder.delivery_state = "-";
                MyPaccoOrder.delivery_country = "IN";
                MyPaccoOrder.pickup_date = dtAWBDate.ToString("yyyy-MM-dd");

                MyPaccoOrder.transport_mode = "1";
                MyPaccoOrder.payment_type = "1";
                MyPaccoOrder.currency_unit = "INR";

                MyPaccoOrder.item_title = "Document";
                MyPaccoOrder.item_desc = "Document";
                MyPaccoOrder.item_quantity = "1";
                MyPaccoOrder.length = "15";
                MyPaccoOrder.height = "10";
                MyPaccoOrder.width = "8";
                MyPaccoOrder.weight = "0.5";
                MyPaccoOrder.base_price = "500";
                MyPaccoOrder.shipp_handling_charges = "0";
                MyPaccoOrder.other_charges = "0";
                MyPaccoOrder.total_amount = "500";

                lstOrder.Add(MyPaccoOrder);

                MyPaccoDatum.rts_order = true;
                MyPaccoDatum.orders = lstOrder;

                lstDatum.Add(MyPaccoDatum);
                MyPaccoRoot.data = lstDatum;

                MyPaccoSession objMyPaccoSession = new MyPaccoSession();

                if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) > 0 || objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken == "")
                {

                    MyPaccoResqAccessToken objMyPaccoResqAccessToken = new MyPaccoResqAccessToken();
                    MyPaccoClientID objMyPaccoClientID = new MyPaccoClientID();
                    List<MyPaccoClientID> lstMyPaccoClientID = new List<MyPaccoClientID>();

                    lstMyPaccoClientID.Add(objMyPaccoClientID);
                    objMyPaccoResqAccessToken.access_token = "";

                    objMyPaccoClientID.client_id = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientId;// "9702420066";
                    objMyPaccoClientID.client_secret = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientPassword;// "Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA";

                    objMyPaccoResqAccessToken.data = lstMyPaccoClientID;

                    //string strJsonPayLoad= "{\"access_token\": \"\",\"data\": [{\"client_id\": \"9702420066\",\"client_secret\": \"Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA\"}]}";

                    string strJsonPayLoad = serializer.Serialize(objMyPaccoResqAccessToken);

                    MyPaccoRespAccessToken txnRespWithObj = MYPACCOAPI.GetAuthTokenAsync(objMyPaccoSession, strJsonPayLoad);

                    if (txnRespWithObj.IsSuccess)
                    {
                        MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                        MyPaccoShared.SaveAPILoginDetails(objMyPaccoSession.MyPaccoApiLoginDetails);
                    }
                    else //if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) >= 0)
                    {
                        txnRespWithObj.IsSuccess = false;
                        lblMessage.Text = "Error : MyPacco Token Expired. Please Get AuthToken for Login";
                        lblMessage.CssClass = "errorMsg";

                        return;
                    }
                }

                // Generate MyPacco AWB

                MyPaccoRoot.access_token = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
                string strAWBPayload = serializer.Serialize(MyPaccoRoot);

                MyPaccoRespAddOrder objMyPaccoRespAddOrder = MYPACCOAPI.GenAWBAsync(objMyPaccoSession, strAWBPayload);

                /*********** Test ********************
                MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();

                objMyPaccoRespAddOrder.IsSuccess = true;

                MyPaccoRespOrderDeail objOrderDetail = new MyPaccoRespOrderDeail();

                objOrderDetail.mypacco_order_id = "BM" + strBabajiOrderNo;
                objOrderDetail.seller_order_number = strBabajiOrderNo.ToString();

                List<MyPaccoRespOrderDeail> lstOrderDetail = new List<MyPaccoRespOrderDeail>();
                lstOrderDetail.Add(objOrderDetail);

                objMyPaccoRespAddOrder.Data = lstOrderDetail;
                **********************************/

                if (objMyPaccoRespAddOrder.IsSuccess == true)
                {
                    // Save Job Details against AWB No
                    string strOrderNo = "", strAWBNo = "", strLSPName = "";

                    int CustomerId = 0;

                    Int32.TryParse(hdnCustomerId.Value, out CustomerId);

                    strOrderNo = objMyPaccoRespAddOrder.Data[0].mypacco_order_id;

                    if (objMyPaccoRespAddOrder.Data[0].awb_number != null)
                    {
                        strAWBNo = objMyPaccoRespAddOrder.Data[0].awb_number;
                    }
                    if (objMyPaccoRespAddOrder.Data[0].lsp_name != null)
                    {
                        strLSPName = objMyPaccoRespAddOrder.Data[0].lsp_name;
                    }

                    DBOperations.MyPaccoAddAWBNo(strOrderNo, strAWBNo, dtAWBDate, strLSPName, strjobIdList, CustomerId, BranchID, PlantAddressID, LoggedInUser.glUserId);

                    lblMessage.Text = "Success : My Pacco Order No: " + strOrderNo;
                    lblMessage.CssClass = "success";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Success", "alert('" + lblMessage.Text + "');", true);
                }
                else if (objMyPaccoRespAddOrder.Message != "")
                {
                    lblMessage.Text = "Error : " + objMyPaccoRespAddOrder.Message;
                    lblMessage.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);

                    gvJobDetail.DataBind();
                }
                else
                {
                    lblMessage.Text = "Error : MyPacco AWB Generation Error";
                    lblMessage.CssClass = "errorMsg";

                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
                }
            }
            else
            {
                lblMessage.Text = "Plant Address Not Found!";
                lblMessage.CssClass = "errorMsg!";
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", "alert('" + lblMessage.Text + "');", true);
            }
        }// Job Type Not Found
        else
        {
            lblMessage.Text = "Please Select Job for MyPacco Disptch!";
            lblMessage.CssClass = "errorMsg!";

        }

    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public class BJVBill
    {
        public int JobId;
        public string BJVNo;
        public string BJVBillNo;

        // public BJVBill(int jobid, string bjvNumber, string bjvBillNo) => (JobId, BJVNo, BJVBillNo) = (jobid, bjvNumber, bjvBillNo);
    }
    #endregion
}