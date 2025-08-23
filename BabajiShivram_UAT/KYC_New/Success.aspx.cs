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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class KYC_New_Success : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(lnkbtnDownload);
        if (!IsPostBack)
        {
            if (Session["KycVendorId"] != null)
            {
                if (Convert.ToString(Session["KycVendorId"]) != "")
                {
                    GetData(Convert.ToInt32(Session["KycVendorId"]));
                }
            }
        }
    }

    protected void GetData(int VendorId)
    {
        DataSet dsGetDetail = KYCOperation.GetVendorDetailById(VendorId);
        if (dsGetDetail != null && dsGetDetail.Tables[0].Rows.Count > 0)
        {
            if (dsGetDetail.Tables[0].Rows[0]["lid"] != DBNull.Value)
            {
                if (dsGetDetail.Tables[0].Rows[0]["KYCCopyPath"] != DBNull.Value)
                {
                    hdnKYCPath.Value = dsGetDetail.Tables[0].Rows[0]["KYCCopyPath"].ToString();
                }
            }
        }
    }

    protected void lnkbtnDownload_Click(object sender, EventArgs e)
    {
        if (hdnKYCPath.Value != "")
        {
            DownloadDocument(hdnKYCPath.Value);
        }
    }

    protected void DownloadDocument(string DocumentPath)
    {
        string ServerPath = FileServer.GetFileServerDir();
        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("UploadFiles\\" + DocumentPath);
        }
        else
        {
            ServerPath = ServerPath + "\\" + DocumentPath;
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
}