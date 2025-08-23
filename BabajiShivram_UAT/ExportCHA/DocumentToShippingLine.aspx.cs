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

public partial class ExportCHA_DocumentToShippingLine : System.Web.UI.Page
{
    LoginClass LoggedInUser = new LoginClass();
    private static Random _random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Jobid"] == null)
            {
                Response.Redirect("ShipmentGetIn.aspx");
            }
            else
            {
                Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
                lblTitle.Text = "Shipment Get In";
                CustomProcessDetail(Convert.ToInt32(Session["JobId"].ToString()));
                Page.Validate();
            }
        }
    }

    private void CustomProcessDetail(int JobId)
    {
        DataSet dsJobDetail = EXOperations.GetJobDetailForCustomProcess(JobId);
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            lblJobRefNo.Text = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            lblCustRefNo.Text = dsJobDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            lblCustName.Text = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
            lblConsigneeName.Text = dsJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
            lblCountryConsgn.Text = dsJobDetail.Tables[0].Rows[0]["ConsignmentCountry"].ToString();
            lblDestCountry.Text = dsJobDetail.Tables[0].Rows[0]["DestinationCountry"].ToString();
            lblForwarderName.Text = dsJobDetail.Tables[0].Rows[0]["ForwarderName"].ToString();
            lblGrossWT.Text = dsJobDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            lblMode.Text = dsJobDetail.Tables[0].Rows[0]["TransMode"].ToString();
            lblNetWT.Text = dsJobDetail.Tables[0].Rows[0]["NetWT"].ToString();
            lblNoOfPkg.Text = dsJobDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            lblPOD.Text = dsJobDetail.Tables[0].Rows[0]["PortOfDischarge"].ToString();
            lblPOL.Text = dsJobDetail.Tables[0].Rows[0]["PortOfLoading"].ToString();
            lblSBDate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["SBDate"]).ToString("dd/MM/yyyy");
            lblSBNo.Text = dsJobDetail.Tables[0].Rows[0]["SBNo"].ToString();
            lblShipper.Text = dsJobDetail.Tables[0].Rows[0]["Shipper"].ToString();
            if (dsJobDetail.Tables[0].Rows[0]["LEODate"] != DBNull.Value)
                lblLEODate.Text = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["LEODate"]).ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["CustomerId"] != DBNull.Value)
                hdnCustId.Value = Convert.ToString(dsJobDetail.Tables[0].Rows[0]["CustomerId"]);
            if (dsJobDetail.Tables[0].Rows[0]["IsBabajiForwarder"] != DBNull.Value)
            {
                string BabajiForwarder = dsJobDetail.Tables[0].Rows[0]["IsBabajiForwarder"].ToString();
                if (BabajiForwarder == "True") // forward to babaji freight
                {
                    // hide forwarder email option
                    txtForwardToEmail.Visible = false;
                    RegExeditEmail.Visible = false;
                    hdnForwardToBabaji.Value = "1";

                    rptFreightDeptList.DataSource = DataSourceFreightOp;
                    rptFreightDeptList.DataBind();
                }
                else
                {
                    // show forwarder email option
                    txtForwardToEmail.Visible = true;
                    RegExeditEmail.Visible = true;

                    hdnForwardToBabaji.Value = "0";
                    lblFreightFwds.Visible = false;
                    rptFreightDeptList.Visible = false;
                }
            }
            rptCustomerEmails.DataSource = DataSourceCustEmails;
            rptCustomerEmails.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime dtShippingHandOverDate = DateTime.MinValue, dtFreightForwarderDate = DateTime.MinValue;
        string strFilePath = "", ExporterfilePath = "", VGMfilePath = "", ForwardToEmail = "";
        int count = 0;

        int JobId = Convert.ToInt32(Session["JobId"]);
        DataSet dsGetJobDetail = EXOperations.EX_GetParticularJobDetail(JobId);
        if (dsGetJobDetail.Tables.Count > 0 && dsGetJobDetail.Tables[0].Rows.Count > 0)
            strFilePath = dsGetJobDetail.Tables[0].Rows[0]["DocFolder"].ToString() + "\\" + dsGetJobDetail.Tables[0].Rows[0]["FileDirName"].ToString() + "\\";
        if (fuExporterCopy.HasFile)
        {
            ExporterfilePath = UploadFiles(fuExporterCopy, strFilePath);
        }

        if (fuVGMCopy.HasFile)
        {
            VGMfilePath = UploadFiles(fuVGMCopy, strFilePath);
        }

        foreach (RepeaterItem itm in rptCustomerEmails.Items)
        {
            CheckBox chkSelect = (CheckBox)itm.FindControl("chkSelect");
            Label lblFreightOpEmail = (Label)itm.FindControl("lblFreightOpEmail");

            if (chkSelect.Checked)
            {
                count = 1;
                if (ForwardToEmail == "")
                    ForwardToEmail = lblFreightOpEmail.Text.Trim();
                else
                    ForwardToEmail = ForwardToEmail + "; " + lblFreightOpEmail.Text.Trim();
            }
        }

        if (hdnForwardToBabaji.Value == "1")
        {
            foreach (DataListItem itm in rptFreightDeptList.Items)
            {
                CheckBox chkSelect = (CheckBox)itm.FindControl("chkSelect");
                Label lblFreightOpEmail = (Label)itm.FindControl("lblFreightOpEmail");

                if (chkSelect.Checked)
                {
                    count = 1;
                    if (ForwardToEmail == "")
                        ForwardToEmail = lblFreightOpEmail.Text.Trim();
                    else
                        ForwardToEmail = ForwardToEmail + "; " + lblFreightOpEmail.Text.Trim();
                }
            }

            if (count == 0)
            {
                lblError.Text = "Please check atleast one name to whom email needs to be sent for Shipment Get In Details.";
                lblError.CssClass = "errorMsg";
                rptFreightDeptList.Focus();
                return;
            }
        }

        if (txtDocHandOverDate.Text.Trim() != "")
            dtShippingHandOverDate = Commonfunctions.CDateTime(txtDocHandOverDate.Text.Trim());
        if (txtFreightForwarderDate.Text.Trim() != "")
            dtFreightForwarderDate = Commonfunctions.CDateTime(txtFreightForwarderDate.Text.Trim());
        if (txtForwardToEmail.Text != "")
            ForwardToEmail = ForwardToEmail + "; " + txtForwardToEmail.Text.Trim();

        int Result = EXOperations.EX_AddShippingGetInDetail(JobId, dtShippingHandOverDate, ExporterfilePath, VGMfilePath, dtFreightForwarderDate, txtForwarderPerson.Text.Trim(),
                                                            ForwardToEmail, LoggedInUser.glUserId);
        if (Result == 0)
        {
            //Update the PCD Back Office Status For Job
            int result_PcdDoc = EXOperations.EX_AddPCDToBackOffice(JobId, LoggedInUser.glUserId);
            if (result_PcdDoc == 0)
            {
                Response.Redirect("../Success.aspx?ShippingHandOver=4005");
            }
            else if (Result == 1)
            {
                lblError.Text = "System Error! Please try after some time.";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 2)
            {
                lblError.Text = "Please Update Back Office Document List!";
                lblError.CssClass = "errorMsg";
            }
            else if (Result == 3)
            {
                lblError.Text = "Job Already Moved To Back Office!";
                lblError.CssClass = "errorMsg";
            }
        }
        else if (Result == 1)
        {
            lblError.Text = "System Error! Please try after some time!";
            lblError.CssClass = "errorMsg";
        }
        else if (Result == 2)
        {
            lblError.Text = "First Check Already Completed!";
            lblError.CssClass = "warning";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["JobId"] = null;
        Response.Redirect("ShipmentGetIn.aspx");
    }

    protected void lnkbtnShowEmailFormat_Click(object sender, EventArgs e)
    {
        string ForwardToEmail = "";

        foreach (RepeaterItem itm in rptCustomerEmails.Items)
        {
            CheckBox chkSelect = (CheckBox)itm.FindControl("chkSelect");
            Label lblFreightOpEmail = (Label)itm.FindControl("lblFreightOpEmail");

            if (chkSelect.Checked)
            {
                if (ForwardToEmail == "")
                    ForwardToEmail = lblFreightOpEmail.Text.Trim();
                else
                    ForwardToEmail = ForwardToEmail + "; " + lblFreightOpEmail.Text.Trim();
            }
        }

        foreach (DataListItem itm in rptFreightDeptList.Items)
        {
            CheckBox chkSelect = (CheckBox)itm.FindControl("chkSelect");
            Label lblFreightOpEmail = (Label)itm.FindControl("lblFreightOpEmail");

            if (chkSelect.Checked)
            {
                if (ForwardToEmail == "")
                    ForwardToEmail = lblFreightOpEmail.Text.Trim();
                else
                    ForwardToEmail = ForwardToEmail + "; " + lblFreightOpEmail.Text.Trim();
            }
        }

        if (txtForwardToEmail.Text != "")
            ForwardToEmail = ForwardToEmail + "; " + txtForwardToEmail.Text.Trim();

        GenerateEmailDraft(ForwardToEmail);
    }

    private void GenerateEmailDraft(string ForwardToEmail)
    {
        string JobRefNo = "", CustRefNo = "", CustName = "", ConsigneeName = "", CountryConsgn = "", DestCountry = "", ForwarderName = "",
           GrossWT = "", Mode = "", NetWT = "", NoOfPkg = "", POD = "", POL = "", SBDate = "", SBNo = "", Shipper = "", LEODate = "",
           ShipmentGetInDate = "", MailCC = "";

        DataSet dsJobDetail = EXOperations.GetJobDetailForCustomProcess(Convert.ToInt32(Session["JobId"].ToString()));
        if (dsJobDetail.Tables[0].Rows.Count > 0)
        {
            //ShipmentGetInDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (dsJobDetail.Tables[0].Rows[0]["ShippingLineDate"] != DBNull.Value)
            {
                ShipmentGetInDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["ShippingLineDate"]).ToString("dd/MM/yyyy");
            }
            JobRefNo = dsJobDetail.Tables[0].Rows[0]["JobRefNo"].ToString();
            CustRefNo = dsJobDetail.Tables[0].Rows[0]["CustRefNo"].ToString();
            CustName = dsJobDetail.Tables[0].Rows[0]["Customer"].ToString();
            ConsigneeName = dsJobDetail.Tables[0].Rows[0]["Consignee"].ToString();
            CountryConsgn = dsJobDetail.Tables[0].Rows[0]["ConsignmentCountry"].ToString();
            DestCountry = dsJobDetail.Tables[0].Rows[0]["DestinationCountry"].ToString();
            ForwarderName = dsJobDetail.Tables[0].Rows[0]["ForwarderName"].ToString();
            GrossWT = dsJobDetail.Tables[0].Rows[0]["GrossWT"].ToString();
            Mode = dsJobDetail.Tables[0].Rows[0]["TransMode"].ToString();
            NetWT = dsJobDetail.Tables[0].Rows[0]["NetWT"].ToString();
            NoOfPkg = dsJobDetail.Tables[0].Rows[0]["NoOfPackages"].ToString();
            POD = dsJobDetail.Tables[0].Rows[0]["PortOfDischarge"].ToString();
            POL = dsJobDetail.Tables[0].Rows[0]["PortOfLoading"].ToString();
            SBDate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["SBDate"]).ToString("dd/MM/yyyy");
            SBNo = dsJobDetail.Tables[0].Rows[0]["SBNo"].ToString();
            Shipper = dsJobDetail.Tables[0].Rows[0]["Shipper"].ToString();
            if (dsJobDetail.Tables[0].Rows[0]["LEODate"] != DBNull.Value)
                LEODate = Convert.ToDateTime(dsJobDetail.Tables[0].Rows[0]["LEODate"]).ToString("dd/MM/yyyy");
            string strCustomerEmail = ForwardToEmail;

            strCustomerEmail = strCustomerEmail.Replace(";", ",").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\r", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\n", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace("\t", "").Trim();
            strCustomerEmail = strCustomerEmail.Replace(" ", "");
            strCustomerEmail = strCustomerEmail.Replace(",,", ",").Trim();
            lblCustomerEmail.Text = strCustomerEmail;

            string EmailContent = "";
            string MessageBody = "";

            txtSubject.Text = "Intimation of Document Hand Over for Job No - " + JobRefNo + " and Party Ref No - " + CustRefNo;

            try
            {
                string strFileName = "../EmailTemplate/ExportIntimation.txt";

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

            txtMailCC.Text = LoggedInUser.glUserName;
            MessageBody = EmailContent.Replace("@JobRefNo", JobRefNo);
            MessageBody = MessageBody.Replace("@SBNo", SBNo);
            MessageBody = MessageBody.Replace("@SBDate", SBDate);
            MessageBody = MessageBody.Replace("@Customer", CustName);
            MessageBody = MessageBody.Replace("@CustRefNo", CustRefNo);
            MessageBody = MessageBody.Replace("@Shipper", Shipper);
            MessageBody = MessageBody.Replace("@Consignee", ConsigneeName);
            MessageBody = MessageBody.Replace("@Mode", Mode);
            MessageBody = MessageBody.Replace("@POL", POL);
            MessageBody = MessageBody.Replace("@POD", POD);
            MessageBody = MessageBody.Replace("@ShipmentGetInDate", ShipmentGetInDate);
            MessageBody = MessageBody.Replace("@EmpName", LoggedInUser.glEmpName);
            divPreviewEmail.InnerHtml = MessageBody;
            ModalPopupEmail.Show();
        }
    }

    protected void btnEMailCancel_Click(object sender, EventArgs e)
    {
        ModalPopupEmail.Hide();
    }

    #region Documnet Upload/Download/Delete

    public string UploadFiles(FileUpload FU, string FilePath)
    {
        string FileName = FU.FileName;
        FileName = FileName.Replace(",", "");

        string ServerFilePath = FileServer.GetFileServerDir();

        if (ServerFilePath == "")
        {
            // Application Directory Path
            ServerFilePath = Server.MapPath("..\\UploadExportFiles\\" + FilePath);
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
        if (FU.FileName != string.Empty)
        {
            if (System.IO.File.Exists(ServerFilePath + FileName))
            {
                string ext = Path.GetExtension(FU.FileName);
                FileName = Path.GetFileNameWithoutExtension(FU.FileName);

                string FileId = RandomString(5);

                FileName += "_" + FileId + ext;
            }

            FU.SaveAs(ServerFilePath + FileName);

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

    #endregion
}