using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using Ionic.Zip;
public partial class PCA_CoverDispatchList : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
        lblTitle.Text = "Bill Covering Letter";

        HtmlAnchor hrefGoBack = (HtmlAnchor)Page.Master.FindControl("hrefGoBack");
        if (hrefGoBack != null)
        {
            hrefGoBack.HRef = "PCA/PendingBillDispatch.aspx";
        }

        if (!IsPostBack)
        {
            if (Session["CoverJobIdList"] != null && Session["CoverCustomerId"] != null)
            {
                hdnJobIdList.Value = Session["CoverJobIdList"].ToString();
                hdnCustomerId.Value = Session["CoverCustomerId"].ToString();
            }

            DataSourceBillJobList.SelectParameters[0].DefaultValue = hdnJobIdList.Value; // "200117"; //346660
            DataSourceBillJobList.SelectParameters[1].DefaultValue = "1";
            DataSourceBillJobList.DataBind();
            gvJobDetail.DataBind();

            // Get Customer Plant Address

            BindPlantAddress();

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
            int BillCoverId = Convert.ToInt32(e.CommandArgument);

            DataSet dsDocDetail = BillingOperation.GetBillCoverDocById(BillCoverId);

            if (dsDocDetail.Tables[0].Rows.Count > 0)
            {
                string strDocPath = dsDocDetail.Tables[0].Rows[0]["DocPath"].ToString();
                string strFileName = dsDocDetail.Tables[0].Rows[0]["FileName"].ToString();

                string strFilePath = strDocPath + "//" + strFileName;

                DownloadDocument(strFilePath);
            }
            else
            {
                lblMessage.Text = "Bill Covering Letter Not Generated!";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }
    protected void gvJobDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "CoverId") != DBNull.Value)
            {
                int DocId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CoverId"));
                if (DocId == 0)
                {
                    //CheckBox chkBillNo = (CheckBox)e.Row.FindControl("chkBillNo");
                    LinkButton lnkCoverView = (LinkButton)e.Row.FindControl("lnkCoverView");
                    
                    if (lnkCoverView != null)
                    {
                        lnkCoverView.Visible = false;
                    }

                    e.Row.BackColor = System.Drawing.Color.Yellow;
                    e.Row.ToolTip = "Convering Letter Not Generated!";
                }
            }
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
    protected void btnGenerateCover_Click(object sender, EventArgs e)
    {
        GetCoverBillList();
    }

    private void GetCoverBillList()
    {
        hdnJobIdList.Value = "";
        int JobId = 0;// Convert.ToInt32(hdnJobId.Value);

        foreach (GridViewRow rw in gvJobDetail.Rows)
        {
            // Find Selected CheckBox

            JobId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);
            int BillId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                if (hdnJobIdList.Value == "")
                {
                    hdnJobIdList.Value = JobId.ToString();
                }
                else
                {
                    hdnJobIdList.Value = hdnJobIdList.Value + "," + JobId.ToString();
                }
            }

        }//END_ForEach

        if (hdnJobIdList.Value != "")
        {
            BillCoveringLetter();

            gvJobDetail.DataBind();
        }
        else
        {
            lblMessage.Text = "Please Select Bill No!";
            lblMessage.CssClass = "errorMsg";
        }
    }
    private void BillCoveringLetter()
    {
        int PlantAddressID = 0;
        string strBillIdList = "" ;

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
            lblMessage.Text = "Please Check Delivery Address!";
            lblMessage.CssClass = "errorMsg";

            return;
        }
        foreach (GridViewRow rw in gvJobDetail.Rows)
        {
            // Find Selected CheckBox

            int JobId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[0]);
            int BillId = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

            CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

            if (chk.Checked)
            {
                if(strBillIdList == "")
                {
                    strBillIdList = BillId.ToString();
                }
                else
                {
                    strBillIdList = strBillIdList + "," + BillId.ToString();
                }
            }

        }//END_ForEach

        if(strBillIdList == "")
        {
            lblMessage.Text = "Please Select Bill For Covering Letter!";
            lblMessage.CssClass = "errorMsg";

            return;
        }

        using (ZipFile zip = new ZipFile())
        {
            // Get Customer Detail
            if (hdnCustomerId.Value != "")
            {
                DataView dvCustomer = DBOperations.GetCustomerDetail(hdnCustomerId.Value);

                if (dvCustomer.Table.Rows.Count > 0)
                {
                    string strCustomerName = dvCustomer.Table.Rows[0]["CustName"].ToString();
                    string strCustomerDocDIR = dvCustomer.Table.Rows[0]["DocFolder"].ToString();

                    strCustomerName = strCustomerName.Replace("M/S", "");
                    strCustomerName = strCustomerName.Replace("/", "");

                    string fileName = strCustomerName.Replace("&amp;", "").Trim().ToString() + ".doc";

                    string strUploadPath = strCustomerDocDIR;
                    string ServerFilePath = FileServer.GetFileServerDir();

                    if (ServerFilePath == "")
                    {
                        // Application Directory Path
                        ServerFilePath = Server.MapPath("..\\UploadFiles\\" + strUploadPath+"\\");
                    }
                    else
                    {
                        // File Server Path
                        ServerFilePath = ServerFilePath + strUploadPath + "\\";
                    }

                    if (!System.IO.Directory.Exists(ServerFilePath))
                    {

                        System.IO.Directory.CreateDirectory(ServerFilePath);
                    }
                    if (fileName != string.Empty)
                    {
                        if (System.IO.File.Exists(ServerFilePath + fileName))
                        {
                            //string ext = Path.GetExtension(fuPCDUpload.FileName);
                            string ext = Path.GetExtension(fileName);
                            //FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                            fileName = Path.GetFileNameWithoutExtension(fileName);
                            string FileId = RandomString(5);

                            fileName += "_" + FileId + ext;
                        }
                    }

                    ReportDocument crystalReport = new ReportDocument();

                    crystalReport.Load(Server.MapPath("~/Reports/CoveringLetter.rpt"));
                    standardCoveringLetter dsCustomers = new standardCoveringLetter();

                    String strjobid = hdnJobIdList.Value;
                    string JobType = "import";
                    if (JobType != "")
                    {
                        dsCustomers = GetCoveringData(Convert.ToInt16(hdnCustomerId.Value), PlantAddressID, strBillIdList);

                        if (dsCustomers.Tables.Count > 0 && dsCustomers.Tables[0].Rows.Count > 0)
                        {
                            crystalReport.SetDataSource(dsCustomers);
                            string masterinvoiceno = dsCustomers.Tables[0].Rows[0]["MasterInvoiceNo"].ToString();
                            zip.AddFile(ServerFilePath + fileName, "Files");
                            
                            crystalReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, ServerFilePath + fileName);

                            foreach (GridViewRow rw in gvJobDetail.Rows)
                            {
                                // Find Selected CheckBox

                                int JobId1 = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[0]);
                                int BillId1 = Convert.ToInt32(gvJobDetail.DataKeys[rw.RowIndex].Values[1]);

                                CheckBox chk = (CheckBox)rw.FindControl("chkBillNo");

                                if (chk.Checked)
                                {
                                    int result = BillingOperation.AddBillCoverletterPath(BillId1, JobId1, Convert.ToInt32(hdnCustomerId.Value), fileName, masterinvoiceno, strUploadPath, LoggedInUser.glUserId);
                                }

                            }//END_ForEach
                        }
                        else
                        {
                            lblMessage.Text = "Plant Address Not Found!";
                            lblMessage.CssClass = "errorMsg";
                        }
                    }// Job Type Not Found
                    else
                    {
                        lblMessage.Text = "Job Type Error!";
                        lblMessage.CssClass = "errorMsg";
                    }

                    crystalReport.Close();
                    crystalReport.Clone();
                    crystalReport.Dispose();
                    crystalReport = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

            }
            else
            {
                lblMessage.Text = "Session Expired! Please Select Job List.";
                lblMessage.CssClass = "errorMsg";
            }
        }
    }

    private standardCoveringLetter GetCoveringData(int CustomerId, int AddressId, string BillIdList)
    {
        string strFinYearID = "0";

        if (Session["FinYearId"] != null)
        {
            strFinYearID = Session["FinYearId"].ToString();
        }
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand("BL_GetCoveringletterDetails");

        cmd.Parameters.Add(new SqlParameter("@CustomerId", CustomerId));
        cmd.Parameters.Add(new SqlParameter("@AddressId", AddressId));
        cmd.Parameters.Add(new SqlParameter("@BillIdList", BillIdList));

        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {

                    sda.Fill(dsCustomers, "Coveringletter");

                    return dsCustomers;
                }
            }
        }
    }

    private standardCoveringLetter GetData(int Customerid, string jobid)
    {
        string strFinYearID = "0";

        if (Session["FinYearId"] != null)
        {
            strFinYearID = Session["FinYearId"].ToString();
        }
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;

        SqlCommand cmd = new SqlCommand("GetCoveringletterdetailsforAll");

        cmd.Parameters.Add(new SqlParameter("@Custid", Customerid));
        cmd.Parameters.Add(new SqlParameter("@jobid", jobid));
        cmd.Parameters.Add(new SqlParameter("@FinYearID", strFinYearID));

        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (standardCoveringLetter dsCustomers = new standardCoveringLetter())
                {

                    sda.Fill(dsCustomers, "Coveringletter");

                    return dsCustomers;
                }
            }
        }
    }

    public class BJVBill
    {
        public int JobId;
        public string BJVNo;
        public string BJVBillNo;

        // public BJVBill(int jobid, string bjvNumber, string bjvBillNo) => (JobId, BJVNo, BJVBillNo) = (jobid, bjvNumber, bjvBillNo);
    }
}