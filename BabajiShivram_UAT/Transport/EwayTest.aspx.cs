using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Newtonsoft.Json;
public partial class Transport_EwayTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            Label lblTitle  = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text   = "E-WayBill Entry Form";

            DBOperations.FillStateGSTID(ddlFromState);
            DBOperations.FillStateGSTID(ddlToState);

            ListItem lstCountry = new ListItem("OTHER COUNTRIES","99");
            ListItem lstTerritory = new ListItem("Other Territory", "97");

            ddlToState.Items.Add(lstCountry);
            ddlToState.Items.Add(lstTerritory);

            ddlToState.Items.Add(lstCountry);
            ddlToState.Items.Add(lstTerritory);
        }
    }

    #region Eway Json
    protected void btnEwayJson_Click(object sender, EventArgs e)
    {
        GenerateEwayJson();

        if (Session["FilePath"] != null)
        {
            string strJsonPath = Session["FilePath"].ToString();

            if (strJsonPath != "")
            {
                DownloadDocument(strJsonPath);
            }
        }
    }
    private void GenerateEwayJson()
    {       
        DataTable dtJson = (DataTable)ViewState["vwsFinalEway"];

        string strFileName = @"EwayJson.json";

        string strJsonPath = WriteJsonClass(dtJson, strFileName);

        if (strJsonPath != "")
        {
            Session["FilePath"] = strJsonPath;

            lblError.Text = "File Created Successfully";
            lblError.CssClass = "success";
        }
        else
        {
            lblError.Text = "File Creation Error!";
            lblError.CssClass = "errorMsg";
        }
    }
    public string WriteJsonClass(DataTable dtEWay, string path)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        int TotalVehicle = 1;

        EWayRootObject root = new EWayRootObject();

        List<EWayBillList> VehicleList = new List<EWayBillList>();

        // Get Distinct Vehicle No

        DataView viewVehicle = new DataView(dtEWay);
        
        root.version = "1.0.0123";
        try
        {
            for (int i = 0; i < TotalVehicle; i++)
            {
                List<EwayItemList> itm = new List<EwayItemList>();
                EWayBillList BillList = new EWayBillList();

                string strVehicleNo = txtVehicleNo.Text.Trim(); //distinctValues.Rows[i]["Vehicle No"].ToString();

                string expression;
                expression = "[Vehicle No]='" + strVehicleNo + "'";
                DataRow[] itemRows;

                itemRows = dtEWay.Select(expression);

                BillList.genMode        =   "Excel";
                BillList.userGstin      =   txtTransid.Text.Trim();
                BillList.supplyType     =   rdlInSubType.SelectedValue;
                BillList.subSupplyType  =   rdlInSubType.SelectedValue;
                BillList.docType        =   ddDocumentType.SelectedValue;
                BillList.docNo          =   txtDocNo.Text.Trim();
                BillList.docDate        =   txtDocDate.Text.Trim();

                BillList.fromGstin      =   txtFromGSTIN.Text.Trim();
                BillList.fromTrdName    =   txtFromTrdName.Text.Trim();
                BillList.fromAddr1      =   txtFromAddr1.Text.Trim();
                BillList.fromAddr2      =   txtFromAddr2.Text.Trim();
                BillList.fromPlace      =   txtFromPlace.Text.Trim();
                BillList.fromPincode    =   Convert.ToInt32(txtFromPincode.Text.Trim());
                BillList.fromStateCode  =   Convert.ToInt32(ddlFromState.SelectedItem.Text);

                BillList.toGstin        =   txtToGSTIN.Text.Trim();
                BillList.toTrdName      =   txtToTrdName.Text.Trim();
                BillList.toAddr1        =   txtToAddr1.Text.Trim();
                BillList.toAddr2        =   txtToAddr2.Text.Trim();
                BillList.toPlace        =   txtToPlace.Text.Trim();
                BillList.toPincode      =   Convert.ToInt32(txtToPincode.Text.Trim());
                BillList.toStateCode    =   Convert.ToInt32(ddlToState.SelectedValue);
                
                BillList.totalValue     =   0;
                BillList.cgstValue      =   0;
                BillList.sgstValue      =   0;
                BillList.igstValue      =   500;
                BillList.cessValue      =   0;

                BillList.transMode      =   rdlTransMode.SelectedValue;
                BillList.transDistance  =   txtDistance.Text.Trim();
                BillList.transporterName =  txtTransporter.Text.Trim();
                BillList.transporterId  =   txtTransid.Text.Trim();
                BillList.transDocNo     =   txtTransDocNo.Text.Trim();
                BillList.transDocDate   =   txtTransDocDt.Text.Trim();
                BillList.vehicleNo      =   txtVehicleNo.Text.Trim();

                int j = 1;
                foreach (DataRow dr in itemRows)
                {
                    // itemList

                    EwayItemList ProductList = new EwayItemList();

                    ProductList.itemNo = j;
                    ProductList.productName = dr["Product"].ToString();
                    ProductList.productDesc = dr["Description"].ToString();
                    if (dr["HSN"].ToString() != "")
                        ProductList.hsnCode = Convert.ToInt32(dr["HSN"].ToString());
                    else
                        ProductList.hsnCode = 3912;

                    ProductList.quantity = Convert.ToDouble(dr["qty"].ToString());

                    ProductList.qtyUnit = "BOX";
                    ProductList.taxableAmount = 0;

                    if (dr["AssessableValue"].ToString() != "")
                    {
                        ProductList.taxableAmount = Convert.ToDouble(dr["AssessableValue"].ToString());
                        BillList.totalValue = BillList.totalValue + Convert.ToDouble(dr["AssessableValue"].ToString());
                    }
                    if (dr["IGSTAmount"].ToString() != "")
                    {
                        BillList.igstValue = BillList.igstValue + Convert.ToDouble(dr["IGSTAmount"].ToString());
                    }

                    if (dr["GSTDutyRate"].ToString() != "")
                    {
                        ProductList.igstRate = Convert.ToInt32(dr["GSTDutyRate"].ToString());
                    }
                    else
                    {
                        ProductList.igstRate = 0;
                    }

                    ProductList.sgstRate = 0;
                    ProductList.cgstRate = 0;
                    ProductList.cessRate = 0;

                    itm.Add(ProductList);
                    j = j + 1;

                }//END_ForEach Item list

                BillList.itemList = itm;
                VehicleList.Add(BillList);

            }// END_For_VehicleCount

            root.billLists = VehicleList;

            var json = serializer.Serialize(root);

            // Upload File To Server
            string ServerPath = FileServer.GetFileServerDir();

            if (ServerPath == "")
            {
                ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + path);
            }
            else
            {
                ServerPath = ServerPath + path;
            }
            using (var file = new StreamWriter(ServerPath, false))
            {
                file.Write(json);
                file.Close();
                file.Dispose();

                return path;
            }
        }//END_Try

        catch (Exception ex) { return ""; }
    }

    #region Download Json
    private void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + DocumentPath);
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

    #endregion
    #endregion
        
}