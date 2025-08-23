using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using MyPacco.API;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

public partial class PCA_MyPaccoOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);

        ScriptManager1.RegisterPostBackControl(gvOrderDetail);
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "My Pacco Order Detail";

        if (!IsPostBack)
        {
            Session["JobId"] = null;

            if (gvOrderDetail.Rows.Count == 0)
            {
                lblMessage.Text = "Order Detail Not Found!";
                lblMessage.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }

        //

        DataFilter1.DataSource = OrderSqlDataSource;
        DataFilter1.DataColumns = gvOrderDetail.Columns;
        DataFilter1.FilterSessionID = "MyPaccoOrder.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);

        //
    }

    protected void gvOrderDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "printawb")
        {
            string strOrderNo = e.CommandArgument.ToString();

            string ServerPath = FileServer.GetFileServerDir();
            string FileName = strOrderNo + ".pdf";

            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\");
            }

            ServerPath = ServerPath + "\\MyPaccoAWBPDF";

            string UploadPath = ServerPath + "\\" + FileName;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            MyPaccoSession objMyPaccoSession = new MyPaccoSession();

            if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) > 0 || objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken == "")
            {

                MyPaccoResqAccessToken objMyPaccoResqAccessToken = new MyPaccoResqAccessToken();
                MyPaccoClientID objMyPaccoClientID = new MyPaccoClientID();
                List<MyPaccoClientID> lstMyPaccoClientID = new List<MyPaccoClientID>();

                objMyPaccoClientID.client_id = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientId;// "9702420066";
                objMyPaccoClientID.client_secret = objMyPaccoSession.MyPaccoApiSetting.MyPaccoClientPassword;// "Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA";

                lstMyPaccoClientID.Add(objMyPaccoClientID);
                objMyPaccoResqAccessToken.access_token = "";

                objMyPaccoResqAccessToken.data = lstMyPaccoClientID;

                //string strJsonPayLoad= "{\"access_token\": \"\",\"data\": [{\"client_id\": \"9702420066\",\"client_secret\": \"Bxy2WQIcrr6apYSfr9cNJOLwDvXAkvjKSFBDl0DA\"}]}";

                string strJsonPayLoad = serializer.Serialize(objMyPaccoResqAccessToken);

                MyPaccoRespAccessToken txnRespWithObj = MYPACCOAPI.GetAuthTokenAsync(objMyPaccoSession, strJsonPayLoad);

                if (txnRespWithObj.IsSuccess)
                {
                    MyPaccoShared.SaveAPILoginDetails(objMyPaccoSession.MyPaccoApiLoginDetails);
                }
                else //if (DateTime.Compare(DateTime.Now, objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoTokenExp) >= 0)
                {
                    txnRespWithObj.IsSuccess = false;
              
                    return;
                }
            }

            //MYPACCOAPI.PrintAWBAsync(objMyPaccoSession, "");

            string strAccessToken = objMyPaccoSession.MyPaccoApiLoginDetails.MyPaccoAuthToken;
           string JsonPayload = "{\"access_token\": \""+strAccessToken+"\",\"data\": [{\"orders\": [\""+ strOrderNo + "\"]}]}";

            MyPaccoRespAddOrder objMyPaccoRespAddOrder = new MyPaccoRespAddOrder();
            objMyPaccoRespAddOrder.IsSuccess = false;

            RestClient client = new RestClient(objMyPaccoSession.MyPaccoApiSetting.MyPaccoBaseUrl + "getDocs");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonPayload, ParameterType.RequestBody);

            try
            {
                // var response = client.Execute(request);
                
                // response.RawBytes.SaveAs(UploadPath);
                // Upload File On Server
                    client.DownloadData(request).SaveAs(UploadPath);

                // File Download ON Client

                DownloadDocument(UploadPath);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.CssClass = "errorMsg";
            }

        }
        else if (e.CommandName.ToLower() == "select")
        {
            string strAWBId = e.CommandArgument.ToString();

            DataSet dsJobDetail = DBOperations.MyPaccoGetJobListByAWB(Convert.ToInt32(strAWBId));

            if (dsJobDetail.Tables[0].Rows.Count > 0)
            {
                gvJobBasic.DataSource = dsJobDetail;
                gvJobBasic.DataBind();

                ModalPopupExtender2.Show();
            }


        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    protected void gvOrderDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }

    private void DownloadDocument(string ServerPath)
    {
        try
        {
            System.Web.HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, "");
        }
        catch (Exception ex)
        {
        }
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
            DataFilter1.FilterSessionID = "MyPaccoOrder.aspx";
            DataFilter1.FilterDataSource();
            gvOrderDetail.DataBind();
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
        string strFileName = "MyPacco_OrderDetail_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + ".xls";

        ExportFunction("attachment;filename=\"" + strFileName + "\"", "application/vnd.ms-excel");
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

        gvOrderDetail.AllowPaging = false;
        gvOrderDetail.AllowSorting = false;

        gvOrderDetail.Columns[1].Visible = false;
        gvOrderDetail.Columns[2].Visible = true;
        gvOrderDetail.Columns[12].Visible = false; // PRINT

        // Excel Header Not Requierd-- Issue in excel header format after export
        // gvJobDetail.Caption = "Job Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "MyPaccoOrder.aspx";
        DataFilter1.FilterDataSource();
        gvOrderDetail.DataBind();

        gvOrderDetail.RenderControl(hw);

        Response.Output.Write(sw.ToString());
        Response.End();

        //  gvJobDetail.DataSourceID = "JobDetailSqlDataSource";
        //  gvJobDetail.DataBind();

        //  BindGridData();
        //  gvJobDetail.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //  gvJobDetail.HeaderRow.Cells[0].Visible = false;
        //  for (int i = 0; i < gvJobDetail.HeaderRow.Cells.Count; i++)
        //  {
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("background-color", "#328ACE");
        //    gvJobDetail.HeaderRow.Cells[i].Style.Add("color", "#FFFFFF");
        //  }

    }
    #endregion
}