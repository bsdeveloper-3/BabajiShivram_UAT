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

public partial class Master_LRDetailById : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnGeneratePDF);

        if (!IsPostBack)
        {
            //string lid = Session["lid"].ToString();
            //string lid = Session["lid"].ToString();
        }
    }

    protected void gvOtherInfo_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        GridViewRow gvr = (GridViewRow)gv.BottomPagerRow;
        if (gvr != null)
        {
            gvr.Visible = true;
            gv.TopPagerRow.Visible = true;
        }
    }


    protected void btnGeneratePDF_Click(object sender, EventArgs e)
    {
        HiddenField hdnlid = (HiddenField)FVJobDetail.FindControl("hdnlid");
        HiddenField hdnCompId = (HiddenField)FVJobDetail.FindControl("hdnCompId");
        HiddenField hdnCNNo = (HiddenField)FVJobDetail.FindControl("hdnCNNo");

        if(hdnlid.Value!="")
        {
            LRCopyPDF(hdnCNNo.Value, hdnlid.Value, hdnCompId.Value);
        }
    }
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
            fileName = "LR_" + fileName;
            j = 1;
            crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, fileName);

            //objRpt3.PrintToPrinter(1, false, 0, 0);
          //  crystalReport.PrintToPrinter(3, false, 1, 1);
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
}