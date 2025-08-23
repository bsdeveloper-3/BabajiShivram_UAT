using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class FreightOperation_AgentDetail : System.Web.UI.Page
{
    private static Random _random = new Random();
    LoginClass LoggedInUser = new LoginClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager1.RegisterPostBackControl(btnSaveAgentInvoice);
        ScriptManager1.RegisterPostBackControl(GridViewInvoiceDetail);
        //ScriptManager1.RegisterPostBackControl(btnUpload);

        if (Session["EnqId"] == null)
        {
            Response.Redirect("AwaitingAdvice.aspx");
        }
        else if (!IsPostBack)
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "Billing Advice";

            //MskValInvoiceDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();
            //MskValRcvdDate.MaximumValue = DateTime.Now.ToString("dd/MM/yyyyy").ToString();

            ddCurrency.SelectedValue = "46"; // Rs

            SetFreightDetail(Convert.ToInt32(Session["EnqId"]));
            //dvAgentInvoice.Visible = false;
        }
        //gvFreightDocument.DataSource = FreightDocumentSqlDataSource;
        //gvFreightDocument.DataBind();
    }

    private void DownloadDocument(string DocumentPath)
    {
        String ServerPath = FileServer.GetFileServerDir();

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
    protected void GridViewInvoiceDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            string DocPath = e.CommandArgument.ToString();
            DownloadDocument(DocPath);
        }
    }
    protected void btnSaveAgentInvoice_Click(object sender, EventArgs e)
    {
        int EnqId = Convert.ToInt32(Session["EnqId"]);

        int CurrencyId = 0; int AgentID = 0;
        string strJBNumber = "", strAgentName = "", strInvoiceNo = "", strRemark = "";

        decimal decInvoiceAmount = 0;
        DateTime dtInvoiceReceivedDate = DateTime.MinValue;
        DateTime dtJBDate = DateTime.MinValue, dtInvoiceDate = DateTime.MinValue;

        strJBNumber = "";
        AgentID = Convert.ToInt32(ddAgent.SelectedValue);
        strAgentName = ddAgent.SelectedItem.Text;
        strInvoiceNo = txtInvoiceNo.Text.Trim();
        CurrencyId = Convert.ToInt32(ddCurrency.SelectedValue);
        strRemark = txtInvoiceRemark.Text.Trim();

        if (strAgentName == "")
        {
            lblError.Text = "Please Enter Agent Name!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (txtReceivedDate.Text.Trim() != "")
        {
            dtInvoiceReceivedDate = Commonfunctions.CDateTime(txtReceivedDate.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Invoice Received Date!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (txtInvoiceAmount.Text.Trim() != "")
        {
            decInvoiceAmount = Convert.ToDecimal(txtInvoiceAmount.Text.Trim());
        }
        else
        {
            lblError.Text = "Please Enter Invoice Amount!";
            lblError.CssClass = "errorMsg";
            return;
        }

        if (strInvoiceNo == "")
        {
            lblError.Text = "Please Enter Agent Invoice No!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (CurrencyId == 0)
        {
            lblError.Text = "Please Select Invoice Currency!";
            lblError.CssClass = "errorMsg";
            return;
        }
        if (txtInvoiceDate.Text.Trim() != "")
        {
            dtInvoiceDate = Commonfunctions.CDateTime(txtInvoiceDate.Text.Trim());
        }

        int result = DBOperations.AddFreightAgentInvoice(EnqId, dtInvoiceReceivedDate, strJBNumber, dtJBDate, AgentID, strAgentName,
          strInvoiceNo, dtInvoiceDate, decInvoiceAmount, CurrencyId, strRemark, LoggedInUser.glUserId);
        //int result = 0;
        if (result == 0)
        {
            string aaa = fuDocument.PostedFile.FileName;
           // FileUpload fuDocument = (FileUpload)FindControl("fuDocument");
            if (fuDocument.HasFile)
            {
                //FileUpload fuDocument = (FileUpload)FindControl("fuDocument");
                string strFilePath = "", FileName = "";

                //int DocumentId = Convert.ToInt32(hdnDocId.Value);

                string strDocFolder = "FreightDoc\\";// + hdnUploadPath.Value + "\\";
                FileName = fuDocument.FileName;

                if (fuDocument.FileName.Trim() != "")
                {
                    strFilePath = UploadPCDDocument(strDocFolder, fuDocument);
                    //strFilePath = UploadDocument(strDocFolder,FileName);
                }

                //Result = DBOperations.AddPCDDocument(JobId, DocumentId, PCDDocType, IsCopy, IsOriginal, strFilePath, LoggedInUser.glUserId);
                result = DBOperations.AddFreightDocument(EnqId, "Agent Invoice", strFilePath, LoggedInUser.glUserId);
            }
           
            if (result == 0)
            {
                string Msg = "Agent Invoice Detail Updated Successfully!";
                lblError.Text = Msg;
                lblError.CssClass = "success";

                //txtAgentName.Text = "";
                txtInvoiceDate.Text = "";
                txtInvoiceAmount.Text = "";
                txtInvoiceRemark.Text = "";
                txtReceivedDate.Text = "";
                txtInvoiceNo.Text = "";

                GridViewInvoiceDetail.DataBind();
                //Response.Redirect("AwaitingAgent.aspx?Msg="+Msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert",
                "alert('" + Msg + "');window.location ='AwaitingAgent.aspx';",
                true);
                // Agent Invoice Completed
                // rdlAgentInvoice.Enabled = true;
            }
            else if (result == 1)
            {
                lblError.Text = "System Error! Please try after sometime.";
                lblError.CssClass = "errorMsg";
            }
            else
            {
                lblError.Text = "System Error! Please try after sometime!";
                lblError.CssClass = "errorMsg";
            }
            /*******************************************************/
        }
    }
    private string UploadPCDDocument(string FilePath, FileUpload fuPCDUpload)
    {
        string FileName = fuPCDUpload.FileName;

        if (FilePath == "")
            //FilePath = "PCA_" + hdnJobId.Value + "\\"; // Alternate Path if Job path is blank
            FilePath = "FreightDoc\\";// + hdnUploadPath.Value + "\\";

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
        if (fuPCDUpload.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(fuPCDUpload.FileName);
                FileName = Path.GetFileNameWithoutExtension(fuPCDUpload.FileName);
                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            fuPCDUpload.SaveAs(ServerFilePath + FileName);

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

    protected void btnCancelInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("AwaitingAgent.aspx");
    }


    private void SetFreightDetail(int EnqId)
    {
        DataSet dsBookingDetail = DBOperations.GetBookingDetail(EnqId);
        string JobType = "", TransMode = "", strAirLine = "";

        if (dsBookingDetail.Tables[0].Rows.Count > 0)
        {
            if (dsBookingDetail.Tables[0].Rows[0]["FRJobNo"] != DBNull.Value)
            {
                lblJobNo.Text = dsBookingDetail.Tables[0].Rows[0]["FRJobNo"].ToString();
            }

            JobType = dsBookingDetail.Tables[0].Rows[0]["lType"].ToString();
            TransMode = dsBookingDetail.Tables[0].Rows[0]["lMode"].ToString();
            strAirLine = dsBookingDetail.Tables[0].Rows[0]["AirLineId"].ToString();

            if (JobType == "2")
            {
                if (TransMode == "1")
                {
                    if (strAirLine != "")
                    {
                       // Agentfield.Visible = false;
                    }
                    else
                    {
                        Agentfield.Visible = true;
                    }
                    //rdlAgentInvoice.SelectedValue = "1";
                    //lblAgentField.Visible = false;
                    //rdlAgentInvoice.Visible = false;

                }
                else
                {
                    //rdlAgentInvoice.SelectedValue = "0";
                    //lblAgentField.Visible = true;
                    //rdlAgentInvoice.Visible = true;
                    Agentfield.Visible = true;
                }
            }

            if (dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"] != DBNull.Value)
              //  txtBillingRemark.Text = dsBookingDetail.Tables[0].Rows[0]["BackOfficeRemark"].ToString();

            // Atleast one agent Invoice required for billing completion

            if (GridViewInvoiceDetail.Rows.Count > 0)
            {
              //  rdlAgentInvoice.Enabled = true;
            }


            DBOperations.FillCompanyByCategory(ddAgent, Convert.ToInt32(EnumCompanyType.Agent));

            if (dsBookingDetail.Tables[0].Rows[0]["AgentCompID"] != DBNull.Value)
            {
                string strAgentID = dsBookingDetail.Tables[0].Rows[0]["AgentCompID"].ToString();

                ddAgent.SelectedValue = strAgentID;

                if (GridViewInvoiceDetail.Rows.Count == 0)
                    ddAgent.Enabled = false;
            }
        }
    }
}