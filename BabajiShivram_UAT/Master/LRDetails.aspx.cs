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
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
public partial class Master_LRDetails : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkexport);
        ScriptManager1.RegisterPostBackControl(gvLRDetail);

        if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "LR Details";

            if (gvLRDetail.Rows.Count == 0)
            {
                lblError.Text = "No Record Found For LR Details!";
                lblError.CssClass = "errorMsg";
                pnlFilter.Visible = false;
            }
        }
        DataFilter1.DataSource = LRDetailsSqlDataSource;
        DataFilter1.DataColumns = gvLRDetail.Columns;
        DataFilter1.FilterSessionID = "LRDetails.aspx";
        DataFilter1.OnDataBound += new DynamicData_Content_DataFilter.BindDataGridView(DataFilter1_OnDataBound);
    }

    protected void gvLRDetail_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "download")
        {
            string strJobId1 = (string)e.CommandArgument;
            string[] strJobId = strJobId1.Split(',');
            string strlid = strJobId[0].ToString();
            string strCNNo = strJobId[1].ToString();
            string strCompId = strJobId[2].ToString();

            if (strlid != "")
            {
                LRCopyPDF(strCNNo,strlid, strCompId);
            }            
        }
        else if (e.CommandName.ToLower() == "detail")
        {
            string strJobId1 = (string)e.CommandArgument;
            string[] strJobId = strJobId1.Split(',');
            string strlid = strJobId[0].ToString();
            string strCNNo = strJobId[1].ToString();        

            if (strlid != "")
            {
                Session["lid"] = strlid;
                Response.Redirect("LRDetailById.aspx");
            }
        }
    }

    protected void gvLRDetail_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }


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
            DataFilter1.FilterSessionID = "LRDetails.aspx";
            DataFilter1.FilterDataSource();
            gvLRDetail.DataBind();
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
        string strFileName = "LRDetails_" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + ".xls";

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
        gvLRDetail.AllowPaging = false;
        gvLRDetail.AllowSorting = false;
        gvLRDetail.Columns[1].Visible = false;
        gvLRDetail.Columns[2].Visible = true;

        gvLRDetail.Caption = "LR Detail On " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

        DataFilter1.FilterSessionID = "LRDetails.aspx";
        DataFilter1.FilterDataSource();
        gvLRDetail.DataBind();
    
        gvLRDetail.RenderControl(hw);
        Response.Output.Write(sw.ToString());
        Response.End();
    }
    #endregion

    #region ------  LR Copy (PDF) -------

    private void LRCopyPDF(string CNNo, string lid1, string CompId)
    {
        lblError.Text = "";
        ReportDocument crystalReport = new ReportDocument();        
        string fileName = CNNo + ".pdf";
        int j = 0;
        int lid = Convert.ToInt32(lid1);    
        MemoryStream oStream;
        DataSet ds = new DataSet();

        if (CompId == "1")
        {
            crystalReport.Load(Server.MapPath("~/Reports/LRCopyDetails.rpt"));
        }
        else if (CompId == "2")
        {
            crystalReport.Load(Server.MapPath("~/Reports/LRCopyNVJ.rpt"));
        }

        LRCopy dsCustomers = GetDataLRDetails(lid);
        DataTable dt = dsCustomers.Tables["dt_LRDetails"];
        dt.TableName = "dt_LRDetails";
        ds.Tables.Add(dt.Copy());

        LRCopy dsLRPackages = GetDataLRPackages(lid);
        DataTable dt1 = dsLRPackages.Tables["dt_LRPackage"];
        dt1.TableName = "dt_LRPackage";
        ds.Tables.Add(dt1.Copy());

        crystalReport.SetDataSource(ds);      
      
        if (dsCustomers.Tables[0].Rows.Count > 0)
        {          
            string CNNo1 = dsCustomers.Tables[0].Rows[0]["CNNo"].ToString();
            //JobRefNo = JobRefNo.Replace("/", ""); // data = data.Replace("\\", "\\\\");
            fileName = "LR_"  + fileName;
            j = 1;
            crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, fileName);

            //objRpt3.PrintToPrinter(1, false, 0, 0);
            crystalReport.PrintToPrinter(3, false, 1, 1);
            //crystalReport.PrintToPrinter()

            crystalReport.Close();
            crystalReport.Clone();
            crystalReport.Dispose();
            crystalReport = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            lblError.Text = "LR Details Found Successfully";
            lblError.CssClass = "success";
            // ModalPopupDocument.Show();         
        }
        else
        {
            lblError.Text = "LR Details Not Found";
            lblError.CssClass = "errorMsg";
           // ModalPopupDocument.Show();
        }
        //  PDF ---Zip  
        if (j == 1)
        {
            lblError.Text = "LR Details Found Successfully";
            lblError.CssClass = "success";
           // ModalPopupDocument.Show();
        }
        // }
    }

    private LRCopy GetDataLRDetails(int lid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("GetLRDetailsById");

        cmd.Parameters.Add(new SqlParameter("@lid", lid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (LRCopy dsCustomers = new LRCopy())
                {
                    sda.Fill(dsCustomers, "dt_LRDetails");
                    return dsCustomers;
                }
            }
        }
    }

    private LRCopy GetDataLRPackages(int lid)
    {
        string conString = ConfigurationManager.ConnectionStrings["ConBSImport"].ConnectionString;
        SqlCommand cmd = new SqlCommand("GetLRPackageDetailsById");

        cmd.Parameters.Add(new SqlParameter("@lid", lid));
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand = cmd;
                using (LRCopy dsCustomers1 = new LRCopy())
                {
                    sda.Fill(dsCustomers1, "dt_LRPackage");
                    return dsCustomers1;
                }
            }
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

    #endregion  ------  END LR Invoice --------

}