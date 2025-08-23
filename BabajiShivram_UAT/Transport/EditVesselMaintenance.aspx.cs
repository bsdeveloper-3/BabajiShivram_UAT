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
public partial class Transport_EditVesselMaintenance : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TrMaintainId"] == null)
        {
            Response.Redirect("VesselMaintenance.aspx");
        }

        if (!IsPostBack)
        {
            MaintenanceDetail(Convert.ToInt32(Session["TrMaintainId"]));

            DBOperations.FillMaintenanceCategory(ddCategory, 2); // Maintenance Category
        }
    }

    private void MaintenanceDetail(int MaintainId)
    {
        DataSet dsDetail = DBOperations.GetMaintenanceWork(MaintainId);

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            string RefNo = "";
            int TotalHours = 0;
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");

            // Fill Maintenance Details

            RefNo = dsDetail.Tables[0].Rows[0]["RefNo"].ToString();

            lblTitle.Text = "Maintenance Detail - " + RefNo;
            hdnUploadPath.Value = RefNo;

            lblRefNo.Text = RefNo;
            lblWorkDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["WorkDate"]).ToString("dd/MM/yyyy");
            lblWorkEndDate.Text = Convert.ToDateTime(dsDetail.Tables[0].Rows[0]["WorkDateEnd"]).ToString("dd/MM/yyyy");
            // lblVehicleNo.Text       =   dsDetail.Tables[0].Rows[0]["VehicleNo"].ToString();
            // lblVehicleType.Text     =   dsDetail.Tables[0].Rows[0]["VehicleType"].ToString();
            lblWorkLocation.Text = dsDetail.Tables[0].Rows[0]["WorkLocation"].ToString();
            lblDesc.Text = dsDetail.Tables[0].Rows[0]["WorkDesc"].ToString();
            TotalHours = Convert.ToInt32(dsDetail.Tables[0].Rows[0]["TotalHours"]);

            //lblStartTime.Text = dsDetail.Tables[0].Rows[0]["StartTime"].ToString();
            //lblEndTime.Text = dsDetail.Tables[0].Rows[0]["EndTime"].ToString();
            //lblBillNo.Text = dsDetail.Tables[0].Rows[0]["BillNumber"].ToString();
            //lblPaidTo.Text = dsDetail.Tables[0].Rows[0]["PaidTo"].ToString();
            //lblSupportBill.Text = dsDetail.Tables[0].Rows[0]["SupportBillPaidTo"].ToString();
            //lblPayType.Text = dsDetail.Tables[0].Rows[0]["PayType"].ToString();
        }
        else
        {
            Response.Redirect("VehicleExpense.aspx");
        }

    }

    protected void btnFileUpload_Click(Object sender, EventArgs e)
    {
        int MaintainID = Convert.ToInt32(Session["TrMaintainId"]);
        int DocResult = -123;

        if (txtDocName.Text.Trim() == "")
        {
            lblError.Text = "Please enter the document name.";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (MaintainID > 0)
        {
            if (fuDocument.HasFile) // Add Maintenance Document
            {
                string strDocFolder = "TransDoc\\" + hdnUploadPath.Value + "\\";

                string strFilePath = UploadDocument(strDocFolder);

                if (strFilePath != "")
                {
                    DocResult = DBOperations.AddMaintenanceDocument(MaintainID, txtDocName.Text.Trim(), strFilePath, LoggedInUser.glUserId);

                    if (DocResult == 0)
                    {
                        lblError.Text = "Document uploaded successfully.";
                        lblError.CssClass = "success";
                        blDocument.DataBind();
                    }
                    else if (DocResult == 1)
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                    else if (DocResult == 2)
                    {
                        lblError.Text = "Document Name Already Exists! Please change the document name!.";
                        lblError.CssClass = "errorMsg";
                    }
                    else
                    {
                        lblError.Text = "System Error! Please try after sometime.";
                        lblError.CssClass = "errorMsg";
                    }
                }
            }//END_IF_FileExists
            else
            {
                lblError.Text = "Please attach the document for upload!";
                lblError.CssClass = "errorMsg";
            }

        }//END_IF_Enquiry
        else
        {
            Response.Redirect("VehicleExpense.aspx");
        }

    }

    public string UploadDocument(string FilePath)
    {
        if (FilePath == "")
        {
            FilePath = "TransDoc\\";
        }

        string FileName = fuDocument.FileName;
        FileName = FileServer.ValidateFileName(FileName);

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            // File Server Path
            ServerFilePath = ServerFilePath + FilePath;
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
        }

        return FilePath + FileName;
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

    protected void blDocument_Click(object sender, BulletedListEventArgs e)
    {
        ListItem li = blDocument.Items[e.Index];

        string FilePath = li.Value;

        String ServerPath = FileServer.GetFileServerDir();

        if (ServerPath == "")
        {
            ServerPath = HttpContext.Current.Server.MapPath("..\\UploadFiles\\" + FilePath);
        }
        else
        {
            ServerPath = ServerPath + FilePath;
        }
        try
        {
            HttpResponse response = Page.Response;
            FileDownload.Download(response, ServerPath, FilePath);
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnBack_OnClick(object sender, EventArgs e)
    {
        Session["TrMaintainId"] = null;
        Response.Redirect("VesselExpense.aspx");
    }

    protected void btnSaveVehicle_Click(object sender, EventArgs e)
    {
        if (Session["TrMaintainId"] != null)
        {
            int MaintainId = Convert.ToInt32(Session["TrMaintainId"]);
            int VehicleID = 1120;
            int CategoryID = Convert.ToInt32(ddCategory.SelectedValue);

            int result = DBOperations.AddMaintenanceByRefID(MaintainId, VehicleID, CategoryID, LoggedInUser.glUserId);

            if (result == 0)
            {
                gvCategory.DataBind();
                lblError.Text = "Vessel/Category Details Added Successfully";
                lblError.CssClass = "success";
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please Try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            else if (result == 2)
            {
                lblError.Text = "Vessel/Category Details Aleardy Added";
                lblError.CssClass = "errorMsg";
            }

        }
    }
}